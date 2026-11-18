using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DeploySharp.Engine;

namespace YoloFool;

/// <summary>
/// 半自动标注：预标注（P1）+ 逐张复核（P2）+ 只导出已确认框（P3）。
/// 固定流程：载入新图 → “⚡ 自动预标注” → “复核” → 再训练。
/// </summary>
public sealed partial class MainForm
{
    private Button _btnAutoLabel;
    private Button _btnReview;
    private Button _btnClearAuto;
    private Label _lblReviewHint;
    private bool _autoLabelBusy;
    private bool _reviewing;

    // ---------------------------------------------------------------- 预标注

    private async Task AutoLabelPendingAsync()
    {
        if (_autoLabelBusy) return;

        if (_imageFiles.Count == 0)
        {
            MessageBox.Show(this, "先在左边点“打开文件夹…”，把要标注的图片列出来。", "自动预标注",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        if (_svc == null || !_svc.IsLoaded)
        {
            MessageBox.Show(this, "模型还没加载好：等右上角状态变成“就绪”后再试。", "自动预标注",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var targets = _imageFiles.Where(p => _store.GetBoxes(p).Count == 0).ToList();
        int already = _imageFiles.Count - targets.Count;
        if (targets.Count == 0)
        {
            MessageBox.Show(this, "列表里每张图都已经有标注了，没有可预标注的图。\n\n要标注新图，先点“打开文件夹…”换一个文件夹。",
                "自动预标注", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var info = _svc.Info;
        var ask = $"用当前模型 {info.FileName}（置信度 {AutoLabeler.DefaultConfidence:0.00}）给 {targets.Count} 张“还没有标注”的图片预标注。"
                  + (already > 0 ? $"\n已有标注的 {already} 张会自动跳过。" : "")
                  + "\n\n自动框是“待复核”状态：确认之前不会导出，也不会进训练集。\n开始？";
        if (MessageBox.Show(this, ask, "自动预标注", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK) return;

        var options = new ModelOptions(info.ModelPath, info.Kind, InferenceBackend.OnnxRuntime,
            info.Device, info.Provider, AutoLabeler.DefaultConfidence, 0.5f);

        _autoLabelBusy = true;
        _btnAutoLabel.Enabled = false;
        _btnReview.Enabled = false;
        _btnClearAuto.Enabled = false;
        UseWaitCursor = true;
        _lblStatus.Text = $"自动预标注中… 0/{targets.Count}";

        AutoLabelStats stats = null;
        try
        {
            stats = await Task.Run(() => AutoLabeler.Run(_store, options, targets,
                (done, total) =>
                {
                    try
                    {
                        BeginInvoke(new Action(() =>
                        {
                            if (IsDisposed) return;
                            _lblStatus.Text = $"自动预标注中… {done}/{total}";
                        }));
                    }
                    catch { /* 窗体已关闭 */ }
                },
                () => IsDisposed));
        }
        catch (Exception ex)
        {
            Log.Error("自动预标注失败", ex);
            MessageBox.Show(this, "自动预标注失败：\n" + ex.Message, "自动预标注",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        finally
        {
            _autoLabelBusy = false;
            _btnAutoLabel.Enabled = true;
            _btnReview.Enabled = true;
            _btnClearAuto.Enabled = true;
            UseWaitCursor = false;
        }

        RebuildImageList();
        RefreshDatasetStats();
        if (stats == null) return;

        _lblStatus.Text = "自动预标注完成：" + stats.Summary;
        var detail = stats.Summary;
        if (stats.Errors.Count > 0) detail += "\n\n失败：\n" + string.Join("\n", stats.Errors.Take(5));

        if (stats.Boxes == 0)
        {
            MessageBox.Show(this, detail + "\n\n没有生成任何框：可以换训练好的模型，或把置信度调低后重试。",
                "自动预标注", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        if (MessageBox.Show(this,
                detail + "\n\n现在开始逐张复核？\nEnter 确认并下一张 · D 丢弃本图自动框 · Delete 删选中框 · Esc 退出",
                "自动预标注", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            StartReview();
    }

    /// <summary>一次清掉所有还没确认的自动框（防污染兜底）。</summary>
    private void ClearAllAutoBoxes()
    {
        if (_autoLabelBusy) return;

        int pending = _store.PendingBoxCount;
        if (pending == 0)
        {
            MessageBox.Show(this, "当前没有待复核的自动框。", "清空自动框",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        if (MessageBox.Show(this, $"丢弃全部 {pending} 个待复核自动框？\n\n手工框和已确认的框不受影响。",
                "清空自动框", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;

        int n = _store.RemoveAllAutoBoxes();
        RebuildImageList();
        if (!string.IsNullOrEmpty(_learnPath)) ReloadBoxList();
        RefreshDatasetStats();
        _lblStatus.Text = $"已丢弃 {n} 个待复核自动框。";
        if (_reviewing) StopReview(false);
    }

    // ---------------------------------------------------------------- 复核

    private void ToggleReview()
    {
        if (_reviewing) StopReview(true);
        else StartReview();
    }

    private void StartReview()
    {
        if (_imageFiles.Count == 0)
        {
            MessageBox.Show(this, "先在左边点“打开文件夹…”，把要标注的图片列出来。", "复核",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        if (!_store.HasPending)
        {
            MessageBox.Show(this, "当前没有待复核的自动框。\n\n先点“⚡ 自动预标注”给新图生成候选框。", "复核",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        _reviewing = true;
        _btnReview.Text = "退出复核";
        _btnAutoLabel.Enabled = false;
        _btnClearAuto.Enabled = false;
        _lblReviewHint.Visible = true;

        int idx = NextPendingImage(CurrentImageIndex(), includeCurrent: true);
        if (idx < 0)
        {
            StopReview(false);
            return;
        }
        LoadReviewImage(idx);
        _lblStatus.Text = "复核中：按 Enter 确认本图自动框，确认后才会进入训练集。";
    }

    private int CurrentImageIndex()
    {
        if (string.IsNullOrEmpty(_learnPath)) return -1;
        for (int i = 0; i < _imageFiles.Count; i++)
            if (string.Equals(_imageFiles[i], _learnPath, StringComparison.OrdinalIgnoreCase)) return i;
        return -1;
    }

    /// <summary>从 from 开始（可含当前张）循环找下一张“还有待复核框”的图，找不到返回 -1。</summary>
    private int NextPendingImage(int from, bool includeCurrent)
    {
        if (_imageFiles.Count == 0) return -1;
        int start = from < 0 ? 0 : from + (includeCurrent ? 0 : 1);
        for (int step = 0; step < _imageFiles.Count; step++)
        {
            int i = (start + step) % _imageFiles.Count;
            if (_store.PendingCount(_imageFiles[i]) > 0) return i;
        }
        return -1;
    }

    private void LoadReviewImage(int index)
    {
        LoadLearnImage(_imageFiles[index]);
        _picLearn.CancelPending();
        _picLearn.SelectedIndex = -1;
        _lvBoxes.SelectedIndices.Clear();
        UpdateReviewHint();
    }

    private void UpdateReviewHint()
    {
        if (!_reviewing) return;

        int total = 0, pos = 0;
        bool found = false;
        foreach (var p in _imageFiles)
        {
            if (_store.PendingCount(p) == 0) continue;
            total++;
            if (!found && string.Equals(p, _learnPath, StringComparison.OrdinalIgnoreCase))
            {
                pos = total;
                found = true;
            }
        }
        if (pos == 0) pos = total;

        _lblReviewHint.Text = $"复核中 {pos}/{total} 张 · 本图待复核 {_store.PendingCount(_learnPath)} 个 · "
                              + "Enter 确认并下一张 · D 丢弃本图自动框 · Delete 删选中框 · Esc 退出";
        _lblImageListInfo.Text = $"待复核 {total} 张图片 · 只有确认过的自动框才会进训练集。";
    }

    private void ConfirmAndAdvance()
    {
        int n = _store.ConfirmAutoBoxes(_learnPath);
        ReloadBoxList();
        _lblStatus.Text = n > 0
            ? $"已确认本图 {n} 个自动框，并导出到训练集。"
            : "本图没有待复核的自动框。";
        AdvanceReview();
    }

    private void DiscardAutoAndAdvance()
    {
        int n = _store.RemoveAutoBoxes(_learnPath);
        ReloadBoxList();
        _lblStatus.Text = n > 0
            ? $"已丢弃本图 {n} 个自动框：现在可以重新拖动框选正确目标。"
            : "本图没有待复核的自动框。";
        AdvanceReview();
    }

    private void AdvanceReview()
    {
        int next = NextPendingImage(CurrentImageIndex(), includeCurrent: false);
        if (next < 0)
        {
            StopReview(true);
            MessageBox.Show(this, "全部待复核的自动框都处理完了，可以点“开始训练”。", "复核完成",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        LoadReviewImage(next);
    }

    private void StopReview(bool announce)
    {
        if (!_reviewing) return;
        _reviewing = false;
        _btnReview.Text = "复核";
        _lblReviewHint.Visible = false;
        _btnAutoLabel.Enabled = !_autoLabelBusy;
        _btnClearAuto.Enabled = true;
        if (announce)
        {
            _lblStatus.Text = $"已退出复核：还有 {_store.PendingBoxCount} 个待复核框（不参与训练）。";
            RefreshImageListInfo();
        }
    }

    /// <summary>
    /// 复核快捷键。画布自己会吃掉方向键/回车（只在有选中框或正在画框时），
    /// 所以这里只用 Enter / D / Delete / Space / Esc，且输入框获得焦点时不拦截打字。
    /// </summary>
    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        if (_reviewing && !(ActiveControl is TextBoxBase))
        {
            if (keyData == Keys.Enter) { ConfirmAndAdvance(); return true; }
            if (keyData == Keys.D) { DiscardAutoAndAdvance(); return true; }
            if (keyData == Keys.Escape) { StopReview(true); return true; }
            if (keyData == Keys.Delete) { DeleteSelectedBox(); UpdateReviewHint(); return true; }
            if (keyData == Keys.Space) { AdvanceReview(); return true; }
        }
        return base.ProcessCmdKey(ref msg, keyData);
    }
}
