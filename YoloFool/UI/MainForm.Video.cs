using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DeploySharp.Engine;

namespace YoloFool;

/// <summary>视频分析页：分割 + IoU 跟踪 + 越线计数 + 面积统计。</summary>
public sealed partial class MainForm
{
    private TextBox _tbVideoPath;   // 别名：指向设计器里的 _tbRecImage
    private SplitContainer _splitVideo;
    private ImageCanvas _picVideo;
    private DataGridView _gridVideo;
    private Label _lblVideoStats;   // 别名：指向设计器里的 _lblResultStats
    private bool _syncingVideoTask;

    private VideoRunner _video;
    private System.Windows.Forms.Timer _videoTimer;
    private bool _videoSplitInit;
    private bool _videoFirstFrame = true;
    private int _videoGridTick;
    private Point? _videoLineA;
    private Point? _videoLineB;

    private void WireVideoEvents()
    {
        _btnPickVideo.Click += (_, _) => PickVideo();
        _btnCamera.Click += (_, _) => StartVideo("0");
        _btnStartVideo.Click += (_, _) => StartPrimaryAction();
        _btnSnapshot.Click += (_, _) => TakeSnapshot();
        _btnRecord.Click += (_, _) => ToggleRecording();
        _btnStopVideo.Click += (_, _) => StopVideo(true);
        _btnDrawLine.Click += (_, _) => BeginDrawLine();
        _btnClearLine.Click += (_, _) => ClearLine();
        _btnVideoModel.Click += (_, _) => PickVideoModel();
        _picVideo.LineDrawn += OnVideoLineDrawn;

        _cboVideoTask.SelectedIndexChanged += (_, _) =>
        {
            if (_syncingVideoTask) return;
            _settings.VideoTask = _cboVideoTask.SelectedIndex == 1 ? "det" : "seg";
            _settings.Save();
            RefreshVideoModelLabel();
            _lblVideoInfo.Text = VideoIsDetection
                ? "检测模式：只画方框，不做掩码面积统计。"
                : "分割模式：统计掩码面积与占比。";
        };
        _numVideoConf.ValueChanged += (_, _) =>
        {
            _settings.VideoConfidence = (float)_numVideoConf.Value;
            _settings.Save();
        };
        _chkVideoBest.CheckedChanged += (_, _) =>
        {
            _settings.VideoOnlyBest = _chkVideoBest.Checked;
            _settings.Save();
        };
        _tbVideoClasses.TextChanged += (_, _) =>
            _settings.VideoClassFilter = (_tbVideoClasses.Text ?? "").Trim();
        _tbVideoClasses.Leave += (_, _) => _settings.Save();

        _videoTimer = new System.Windows.Forms.Timer { Interval = 33 };
        _videoTimer.Tick += (_, _) => DrainVideoFrame();
        _splitVideo.SizeChanged += (_, _) => InitSplitters();
    }

    private void RefreshVideoSettings()
    {
        _tbVideoPath.Text = File.Exists(_settings.LastVideoPath)
            ? _settings.LastVideoPath
            : AppPaths.DefaultVideo;
        _chkVideoLoop.Checked = _settings.VideoLoop;
        _numVideoStep.Value = Math.Clamp(_settings.VideoFrameStep, 1, 12);
        _lblVideoInfo.Text = string.IsNullOrEmpty(_tbVideoPath.Text) ? "请选择视频源" : "就绪，点“开始分析”";
        _lblVideoLine.Text = "计数线：默认画面中线";
        SyncVideoTaskCombo();
        _numVideoConf.Value = (decimal)Math.Clamp(_settings.VideoConfidence, 0.05f, 0.95f);
        _tbVideoClasses.Text = _settings.VideoClassFilter ?? "";
        _chkVideoBest.Checked = _settings.VideoOnlyBest;
        RefreshVideoModelLabel();
    }

    private bool VideoIsDetection =>
        string.Equals(_settings.VideoTask, "det", StringComparison.OrdinalIgnoreCase);

    private YoloModelKind VideoModelKind =>
        VideoIsDetection ? YoloModelKind.YOLOv8Det : YoloModelKind.YOLOv8Seg;

    private void SyncVideoTaskCombo()
    {
        if (_cboVideoTask == null) return;
        _syncingVideoTask = true;
        try { _cboVideoTask.SelectedIndex = VideoIsDetection ? 1 : 0; }
        finally { _syncingVideoTask = false; }
    }

    private static IEnumerable<string> SplitVideoClasses(string text) =>
        (text ?? "").Split(new[] { ',', '，', ';', '；', ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.Trim())
            .Where(x => x.Length > 0);

    private void RefreshVideoModelLabel()
    {
        var path = ResolveVideoModel();
        var prefix = VideoIsDetection ? "检测模型：" : "分割模型：";
        _lblVideoModel.Text = string.IsNullOrEmpty(path)
            ? prefix + "未找到"
            : prefix + Path.GetFileName(path);
        _lblVideoModel.ForeColor = string.IsNullOrEmpty(path) ? Theme.Danger : Theme.Muted;
    }

    private string ResolveVideoModel()
    {
        if (VideoIsDetection)
        {
            if (!string.IsNullOrWhiteSpace(_settings.VideoDetModelPath) && File.Exists(_settings.VideoDetModelPath))
                return _settings.VideoDetModelPath;
            return AppPaths.DefaultCocoModel;
        }
        if (!string.IsNullOrWhiteSpace(_settings.VideoModelPath) && File.Exists(_settings.VideoModelPath))
            return _settings.VideoModelPath;
        return AppPaths.DefaultSegModel;
    }

    private void PickVideo()
    {
        using var dlg = new OpenFileDialog
        {
            Title = "选择视频",
            Filter = "视频文件|*.mp4;*.avi;*.mkv;*.mov;*.wmv;*.m4v|所有文件|*.*",
        };
        if (dlg.ShowDialog(this) != DialogResult.OK) return;
        StopVideo(false);
        _tbVideoPath.Text = dlg.FileName;
        _settings.LastVideoPath = dlg.FileName;
        _settings.Save();
        _lblVideoInfo.Text = "已选择：" + Path.GetFileName(dlg.FileName);
    }

    private void PickVideoModel()
    {
        using var dlg = new OpenFileDialog
        {
            Title = "选择模型（自动识别检测/分割）",
            Filter = "ONNX 模型|*.onnx|所有文件|*.*",
        };
        if (dlg.ShowDialog(this) != DialogResult.OK) return;
        StopVideo(false);

        // 以 ONNX 文件为准：选进来的检测模型不会因为下拉停在"分割"而被当成分割加载
        var actual = OnnxInspector.DetectKind(dlg.FileName);
        bool isDet = actual == null ? VideoIsDetection : actual.Value != YoloModelKind.YOLOv8Seg;
        if (isDet != VideoIsDetection)
        {
            _settings.VideoTask = isDet ? "det" : "seg";
            SyncVideoTaskCombo();
        }

        if (isDet)
            _settings.VideoDetModelPath = dlg.FileName;
        else
            _settings.VideoModelPath = dlg.FileName;
        _settings.Save();
        RefreshVideoModelLabel();

        var head = actual == null ? (isDet ? "检测" : "分割") : OnnxInspector.Describe(actual);
        _lblVideoInfo.Text = $"已切换{head}模型：" + Path.GetFileName(dlg.FileName);
        Log.Info($"视频页更换模型：{dlg.FileName}（{head}，任务={(VideoIsDetection ? "det" : "seg")}）");
    }

    /// <summary>
    /// 让"模型类型"跟随实际模型文件。返回 true 表示做了切换。
    /// </summary>
    private bool AlignVideoTaskToModel(string modelPath)
    {
        var actual = OnnxInspector.DetectKind(modelPath);
        if (actual == null) return false;

        bool wantDet = actual.Value != YoloModelKind.YOLOv8Seg;
        if (wantDet == VideoIsDetection) return false;

        _settings.VideoTask = wantDet ? "det" : "seg";
        if (wantDet)
            _settings.VideoDetModelPath = modelPath;
        else
            _settings.VideoModelPath = modelPath;
        _settings.Save();
        SyncVideoTaskCombo();
        RefreshVideoModelLabel();
        Log.Info($"视频页按模型自动切换为{(wantDet ? "检测" : "分割")}模式：{modelPath}");
        return true;
    }

    private void StartVideo(string forcedSource)
    {
        if (_video != null && _video.IsRunning) return;

        var source = (forcedSource ?? _tbVideoPath.Text ?? "").Trim();
        if (!string.IsNullOrEmpty(forcedSource)) _tbVideoPath.Text = source;
        if (string.IsNullOrEmpty(source))
        {
            PickVideo();
            source = _tbVideoPath.Text.Trim();
        }
        if (string.IsNullOrEmpty(source)) return;

        bool isCamera = source.Length <= 2 && int.TryParse(source, out _);
        if (!isCamera && !File.Exists(source))
        {
            MessageBox.Show(this, "视频文件不存在：\n" + source, "视频分析",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var model = ResolveVideoModel();
        if (string.IsNullOrWhiteSpace(model) || !File.Exists(model))
        {
            MessageBox.Show(this,
                VideoIsDetection
                    ? "未找到检测模型。请把 yolov8n.onnx 放到 demo/models/，或点“更换模型”选择。"
                    : "未找到分割模型。请把 yolov8n-seg.onnx 放到 demo/models/，或点“更换模型”选择。",
                "视频分析", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        // 设置里"模型类型"和实际模型文件可能对不上（例如把训练的检测模型选进了分割模式），
        // 直接跑会在 Warmup 抛 IndexOutOfRangeException，这里先按文件纠正。
        if (AlignVideoTaskToModel(model))
            _lblVideoInfo.Text = VideoIsDetection ? "已按模型自动切换为检测模式" : "已按模型自动切换为分割模式";

        SetCaptureSource(isCamera ? CaptureSource.Camera : CaptureSource.Video);
        _recBitmap = null;
        _recPath = "";
        _tbRecImage.Text = isCamera ? "摄像头 0" : source;

        StopVideo(false);
        _video?.Dispose();

        _video = new VideoRunner
        {
            FrameStep = (int)_numVideoStep.Value,
            Loop = _chkVideoLoop.Checked,
        };
        _video.Status += s => BeginInvokeSafe(() =>
        {
            if (!IsDisposed) _lblVideoInfo.Text = s;
        });
        _video.Error += ex => BeginInvokeSafe(() => OnVideoFailed(ex));

        _videoFirstFrame = true;
        _videoGridTick = 0;
        var old = _picVideo.Image;
        _picVideo.Image = null;
        old?.Dispose();
        _picVideo.Tracks = Array.Empty<TrackedObject>();
        _picVideo.Detections = Array.Empty<DetectionItem>();
        _detections = Array.Empty<DetectionItem>();
        _rawDetections = Array.Empty<DetectionItem>();
        _picVideo.SelectedIndex = -1;
        _picVideo.LineA = null;
        _picVideo.LineB = null;
        _videoLineA = null;
        _videoLineB = null;
        _gridVideo.Rows.Clear();
        _lblVideoLine.Text = "计数线：默认画面中线";
        _lblVideoInfo.Text = "正在启动…";
        _lblVideoStats.Text = VideoIsDetection ? "正在加载检测模型…" : "正在加载分割模型…";
        _lblVideoArea.Text = VideoIsDetection ? "面积：—（检测无掩码）" : "分割面积：—";
        _lblVideoActive.Text = "0";
        _lblVideoTotal.Text = "0";
        _lblVideoAtoB.Text = "0";
        _lblVideoBtoA.Text = "0";

        var confidence = (float)Math.Clamp(_numVideoConf.Value, 0.05m, 0.95m);
        _video.ClassFilter.Clear();
        foreach (var token in SplitVideoClasses(_tbVideoClasses.Text))
            _video.ClassFilter.Add(token);
        _video.OnlyBest = _chkVideoBest.Checked;
        _video.Filter = FilterOptions.FromSettings(_settings);
        _video.LabelOf = (raw, id) => _labels.Display(raw, id);

        _videoTimer.Start();
        SetVideoRunning(true);
        _video.Start(source, new ModelOptions(model, VideoModelKind,
            InferenceBackend.OnnxRuntime, DeviceType.CPU, OnnxRuntimeDeviceType.Default,
            confidence, 0.5f));

        _settings.LastVideoPath = source;
        _settings.VideoLoop = _chkVideoLoop.Checked;
        _settings.VideoFrameStep = (int)_numVideoStep.Value;
        _settings.VideoConfidence = confidence;
        _settings.VideoTask = VideoIsDetection ? "det" : "seg";
        _settings.VideoOnlyBest = _chkVideoBest.Checked;
        _settings.VideoClassFilter = (_tbVideoClasses.Text ?? "").Trim();
        _settings.Save();
    }

    private void SetVideoRunning(bool running)
    {
        _btnStartVideo.Enabled = !running;
        if (_btnRecord != null) _btnRecord.Enabled = running || _recordWriter != null;
        _btnStopVideo.Enabled = running;
        _btnPickVideo.Enabled = !running;
        _btnCamera.Enabled = !running;
        _btnVideoModel.Enabled = !running;
        _chkVideoLoop.Enabled = !running;
        _numVideoStep.Enabled = !running;
        _cboVideoTask.Enabled = !running;
        _numVideoConf.Enabled = !running;
        _tbVideoClasses.Enabled = !running;
        _chkVideoBest.Enabled = !running;
        if (!running)
        {
            _btnDrawLine.Enabled = false;
            _btnClearLine.Enabled = false;
        }
    }

    private void StopVideo(bool user)
    {
        _videoTimer?.Stop();
        StopRecording(false);
        if (_btnRecord != null) _btnRecord.Enabled = false;
        var v = _video;
        _video = null;
        if (v != null)
        {
            try { v.Stop(); } catch { }
            v.Dispose();
        }
        var old = _picVideo.Image;
        _picVideo.Image = null;
        old?.Dispose();
        _picVideo.Tracks = Array.Empty<TrackedObject>();
        _picVideo.Detections = Array.Empty<DetectionItem>();
        _detections = Array.Empty<DetectionItem>();
        _rawDetections = Array.Empty<DetectionItem>();
        _picVideo.SelectedIndex = -1;
        _picVideo.Invalidate();
        SetVideoRunning(false);
        if (user)
        {
            _lblVideoInfo.Text = "已停止";
            _lblStatus.Text = "视频分析已停止";
        }
    }

    private void OnVideoFailed(Exception ex)
    {
        Log.Error("视频分析失败", ex);
        _lblVideoInfo.Text = "失败：" + ex.Message;
        StopVideo(false);
        MessageBox.Show(this, "视频分析失败：\n" + ex.Message, "视频分析",
            MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }

    private void DrainVideoFrame()
    {
        var v = _video;
        if (v == null) return;
        var frame = v.TakeLatest();
        if (frame == null) return;

        if (_videoFirstFrame)
        {
            _picVideo.Image = null;
            _picVideo.SetFrame(frame.Bitmap);
            _videoFirstFrame = false;
            SetDefaultLine(frame.Width, frame.Height);
        }
        else
        {
            var old = _picVideo.Image;
            _picVideo.SetFrame(frame.Bitmap);
            old?.Dispose();
        }

        WriteRecordFrame(frame.Bitmap);

        _picVideo.Tracks = frame.Tracks;
        _picVideo.Invalidate();
        _btnDrawLine.Enabled = true;
        _btnClearLine.Enabled = true;

        _lblVideoStats.Text = $"{frame.Fps:F1} FPS · 推理 {frame.InferenceMs} ms · 帧 {frame.FrameIndex}";
        _lblVideoArea.Text = frame.Segmentation
            ? $"分割面积 {frame.MaskAreaPixels:N0} px · 占画面 {frame.MaskAreaPercent:F1}%"
            : "面积：—（检测无掩码）";
        _lblVideoActive.Text = frame.Tracks.Count.ToString();
        _lblVideoTotal.Text = frame.TotalTracks.ToString();
        _lblVideoAtoB.Text = frame.AtoB.ToString();
        _lblVideoBtoA.Text = frame.BtoA.ToString();
        if (++_videoGridTick % 6 == 0) FillVideoGrid(frame);
        _lblStatus.Text = $"视频：{Path.GetFileName(_tbVideoPath.Text)} · {frame.Tracks.Count} 个目标";
    }

    private void FillVideoGrid(VideoFrame frame)
    {
        _gridVideo.SuspendLayout();
        _gridVideo.Rows.Clear();
        foreach (var t in frame.Tracks.OrderBy(x => x.Id))
        {
            int row = _gridVideo.Rows.Add();
            var cells = _gridVideo.Rows[row].Cells;
            cells["colId"].Value = t.Id;
            cells["colName"].Value = _labels.Display(t.Item.RawCategory, t.Item.ClassId);
            cells["colConf"].Value = t.Item.Confidence.ToString("P0");
            cells["colPos"].Value = RelativePosition.Short(t.Item.CenterXPercent, t.Item.CenterYPercent);
            cells["colCenter"].Value = $"{t.Item.CenterXPercent:F0}%,{t.Item.CenterYPercent:F0}%";
            cells["colArea"].Value = t.AreaPixels > 0 ? t.AreaPixels.ToString("N0") : "—";
            cells["colPct"].Value = t.AreaPercent > 0 ? t.AreaPercent.ToString("F1") + "%" : "—";
            _gridVideo.Rows[row].Tag = t;
        }
        _gridVideo.ResumeLayout();
    }

    private void SetDefaultLine(int width, int height)
    {
        if (_videoLineA != null) return;
        var a = new Point(width / 2, (int)(height * 0.15));
        var b = new Point(width / 2, (int)(height * 0.85));
        _videoLineA = a;
        _videoLineB = b;
        _picVideo.LineA = a;
        _picVideo.LineB = b;
        _video?.SetLine(a, b);
        _lblVideoLine.Text = "计数线：默认画面中线";
    }

    private void BeginDrawLine()
    {
        if (_picVideo.Image == null)
        {
            MessageBox.Show(this, "先开始分析，画面出现后再画计数线。", "视频分析",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        _picVideo.LineEditable = true;
        _lblVideoInfo.Text = "在画面上点起点，再点终点（Esc 取消）";
    }

    private void OnVideoLineDrawn(Point a, Point b)
    {
        if (a == b) return;
        _picVideo.LineEditable = false;
        _videoLineA = a;
        _videoLineB = b;
        _picVideo.LineA = a;
        _picVideo.LineB = b;
        _video?.SetLine(a, b);
        _lblVideoLine.Text = $"计数线：({a.X},{a.Y}) → ({b.X},{b.Y})";
        _lblVideoInfo.Text = "计数线已更新；越线计数继续累计";
    }

    private void ClearLine()
    {
        _picVideo.LineEditable = false;
        _picVideo.LineA = null;
        _picVideo.LineB = null;
        _videoLineA = null;
        _videoLineB = null;
        _video?.SetLine(null, null);
        _lblVideoLine.Text = "计数线：未设置";
    }

    private void ShutdownVideo()
    {
        _videoTimer?.Stop();
        _videoTimer?.Dispose();
        _videoTimer = null;
        var v = _video;
        _video = null;
        if (v != null)
        {
            try { v.Stop(); } catch { }
            v.Dispose();
        }
    }
}
