using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace YoloFool;

/// <summary>结果筛除（P2）：筛除条件编辑、类别屏蔽、手工删除误检并留存样本。</summary>
public sealed partial class MainForm
{
    private Button _btnFilter;
    private Button _btnBlockClass;
    private Button _btnDeleteResult;
    private Label _lblFilterState;

    private IReadOnlyList<DetectionItem> _rawDetections = Array.Empty<DetectionItem>();
    private PredictionResult _lastResult;

    private string DisplayOf(string raw, int classId)
    {
        try { return _labels.Display(raw, classId); }
        catch { return string.IsNullOrWhiteSpace(raw) ? classId.ToString() : raw; }
    }

    private bool VideoRunning => _video != null && _video.IsRunning;

    private void EditFilterOptions()
    {
        var current = FilterOptions.FromSettings(_settings);
        using var dlg = new FilterDialog(current, _svc?.Info?.ClassNames);
        if (dlg.ShowDialog(this) != DialogResult.OK || dlg.Result == null) return;

        var o = dlg.Result;
        _settings.FilterEnabled = o.Enabled;
        _settings.FilterMinArea = o.MinAreaPercent;
        _settings.FilterMaxArea = o.MaxAreaPercent;
        _settings.FilterMinAspect = o.MinAspect;
        _settings.FilterMaxAspect = o.MaxAspect;
        _settings.FilterDedupIou = o.DedupIou;
        _settings.FilterConfirmFrames = o.ConfirmFrames;
        _settings.MutedClasses = o.Muted.ToList();
        _settings.Save();

        RefreshFilterBadge();
        if (_lastResult != null && !VideoRunning) ShowDetections(_lastResult, _recPath);
        _lblStatus.Text = o.Enabled ? "筛除已启用：" + o.Summary : "筛除已关闭；" + o.Summary;
    }

    /// <summary>用当前设置重新过滤上一次的原始结果（不重新推理）。</summary>
    private void ShowDetections(PredictionResult result, string imagePath)
    {
        if (result == null) return;
        _lastResult = result;
        var opts = FilterOptions.FromSettings(_settings);
        var filtered = ResultFilter.Apply(_rawDetections, opts, DisplayOf);
        _detections = filtered;
        _picRec.Detections = _detections;
        _picRec.SelectedIndex = -1;
        FillResultGrid(result);
        RefreshFilterBadge();
        _recPath = imagePath ?? _recPath;
    }

    private void RefreshFilterBadge()
    {
        if (_lblFilterState == null) return;
        var o = FilterOptions.FromSettings(_settings);
        if (!o.Enabled && o.Muted.Count > 0)
        {
            _lblFilterState.Text = $"已屏蔽 {o.Muted.Count} 类";
            _lblFilterState.ForeColor = Theme.Warn;
            return;
        }
        _lblFilterState.Text = o.Enabled ? "筛除：开" : "筛除：关";
        _lblFilterState.ForeColor = o.Enabled ? Theme.Accent : Theme.Muted;
    }

    /// <summary>把当前选中框的类别加入屏蔽名单（等效“以后别再报这个类”）。</summary>
    private void BlockSelectedClass()
    {
        if (VideoRunning) { _lblStatus.Text = "视频运行中请先停止，再屏蔽类别。"; return; }
        var row = _grid.CurrentRow;
        if (row == null || row.Index < 0 || row.Index >= _detections.Count)
        {
            _lblStatus.Text = "先在结果表里选一个目标，再点“屏蔽该类”。";
            return;
        }
        var d = _detections[row.Index];
        var name = DisplayOf(d.RawCategory, d.ClassId);
        _settings.MutedClasses ??= new List<string>();
        if (!_settings.MutedClasses.Contains(name, StringComparer.OrdinalIgnoreCase))
        {
            _settings.MutedClasses.Add(name);
            _settings.Save();
        }
        RefreshFilterBadge();
        if (_lastResult != null) ShowDetections(_lastResult, _recPath);
        _lblStatus.Text = $"已屏蔽“{name}”，图片与视频里都不再显示该类。";
    }

    /// <summary>删除单个误检框；图片存副本到 my_models/misdetect 便于以后补训。</summary>
    private void RemoveSelectedResult()
    {
        if (VideoRunning) { _lblStatus.Text = "视频运行中请先停止，再删除单框。"; return; }
        var row = _grid.CurrentRow;
        if (row == null || row.Index < 0 || row.Index >= _detections.Count)
        {
            _lblStatus.Text = "先在结果表里选一个目标，再点“删除选中”。";
            return;
        }

        var list = _detections as List<DetectionItem> ?? _detections.ToList();
        var index = row.Index;
        var d = list[index];
        list.RemoveAt(index);
        _detections = list;
        _picRec.Detections = _detections;
        _picRec.SelectedIndex = -1;

        var saved = SaveMisdetectSample(d);
        FillResultGrid(_lastResult);
        RefreshFilterBadge();
        _lblStatus.Text = $"已删除误检“{DisplayOf(d.RawCategory, d.ClassId)}”"
            + (saved ? "，样本已存到 my_models/misdetect。" : "。");
    }

    private bool SaveMisdetectSample(DetectionItem d)
    {
        try
        {
            bool fromDisk = !string.IsNullOrEmpty(_recPath) && File.Exists(_recPath);
            bool fromMemory = !fromDisk && _recInMemory && _recBitmap != null;
            if (!fromDisk && !fromMemory) return false;

            Directory.CreateDirectory(AppPaths.MisdetectDir);
            var cls = SafeFileName(DisplayOf(d.RawCategory, d.ClassId));
            var name = $"{DateTime.Now:yyyyMMdd-HHmmss-fff}_{cls}_{d.Confidence:P0}.jpg";
            var dest = Path.Combine(AppPaths.MisdetectDir, name);
            if (fromDisk)
            {
                File.Copy(_recPath, dest, true);
            }
            else
            {
                // 拍照定格的照片只在内存里，这里按用户“保存误检样本”的明确动作写一份
                using var copy = new System.Drawing.Bitmap(_recBitmap);
                copy.Save(dest, System.Drawing.Imaging.ImageFormat.Jpeg);
            }

            var json = $"{{\"image\":\"{dest.Replace("\\", "/")}\",\"class\":\"{d.RawCategory}\",\"classId\":{d.ClassId},\"conf\":{d.Confidence:F3},\"box\":[{d.X},{d.Y},{d.W},{d.H}],\"at\":\"{DateTime.Now:yyyy-MM-dd HH:mm:ss}\"}}";
            File.AppendAllText(Path.Combine(AppPaths.MisdetectDir, "misdetect.jsonl"), json + Environment.NewLine, Encoding.UTF8);
            return true;
        }
        catch (Exception ex)
        {
            Log.Warn("保存误检样本失败：" + ex.Message);
            return false;
        }
    }

    private static string SafeFileName(string text)
    {
        var s = string.IsNullOrWhiteSpace(text) ? "obj" : text.Trim();
        foreach (var ch in Path.GetInvalidFileNameChars()) s = s.Replace(ch, '_');
        return s.Length > 24 ? s.Substring(0, 24) : s;
    }
}
