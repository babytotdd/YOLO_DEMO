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
    private TabPage _pageVideo;
    private TextBox _tbVideoPath;
    private Button _btnPickVideo;
    private Button _btnCamera;
    private Button _btnStartVideo;
    private Button _btnStopVideo;
    private Button _btnDrawLine;
    private Button _btnClearLine;
    private Button _btnVideoModel;
    private CheckBox _chkVideoLoop;
    private NumericUpDown _numVideoStep;
    private SplitContainer _splitVideo;
    private ImageCanvas _picVideo;
    private DataGridView _gridVideo;
    private Label _lblVideoStats;
    private Label _lblVideoInfo;
    private Label _lblVideoModel;
    private Label _lblVideoArea;
    private Label _lblVideoActive;
    private Label _lblVideoTotal;
    private Label _lblVideoAtoB;
    private Label _lblVideoBtoA;
    private Label _lblVideoLine;
    private ComboBox _cboVideoTask;
    private NumericUpDown _numVideoConf;
    private TextBox _tbVideoClasses;
    private CheckBox _chkVideoBest;
    private bool _syncingVideoTask;

    private VideoRunner _video;
    private System.Windows.Forms.Timer _videoTimer;
    private bool _videoSplitInit;
    private bool _videoFirstFrame = true;
    private int _videoGridTick;
    private Point? _videoLineA;
    private Point? _videoLineB;

    private Control BuildVideoPage()
    {
        var root = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 3,
            BackColor = Theme.Bg,
            Padding = new Padding(2, 2, 2, 2),
        };
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
        root.Controls.Add(BuildVideoToolbar(), 0, 0);
        root.Controls.Add(BuildVideoOptions(), 0, 1);
        root.Controls.Add(BuildVideoBody(), 0, 2);
        return root;
    }

    private Control BuildVideoToolbar()
    {
        var bar = new TableLayoutPanel
        {
            Dock = DockStyle.Top,
            AutoSize = true,
            ColumnCount = 6,
            RowCount = 1,
            Margin = new Padding(0, 0, 0, 4),
            BackColor = Theme.Bg,
        };
        bar.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        bar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
        bar.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        bar.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        bar.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        bar.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        bar.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        bar.Controls.Add(FieldLabel("视频"), 0, 0);
        _tbVideoPath = new TextBox
        {
            Name = "tbVideoPath",
            Dock = DockStyle.Fill,
            Font = Theme.Body,
            Margin = new Padding(0, 4, 8, 4),
        };
        bar.Controls.Add(_tbVideoPath, 1, 0);

        _btnPickVideo = Theme.Ghost("选择视频");
        _btnPickVideo.Name = "btnPickVideo";
        Stretch(_btnPickVideo, 98, 34);
        _btnPickVideo.Anchor = AnchorStyles.Left;
        _btnPickVideo.Margin = new Padding(0, 3, 4, 3);
        bar.Controls.Add(_btnPickVideo, 2, 0);

        _btnCamera = Theme.Ghost("摄像头");
        _btnCamera.Name = "btnCamera";
        Stretch(_btnCamera, 82, 34);
        _btnCamera.Anchor = AnchorStyles.Left;
        _btnCamera.Margin = new Padding(0, 3, 4, 3);
        bar.Controls.Add(_btnCamera, 3, 0);

        _btnStartVideo = Theme.Primary("开始分析");
        _btnStartVideo.Name = "btnStartVideo";
        Stretch(_btnStartVideo, 108, 36);
        _btnStartVideo.Anchor = AnchorStyles.Left;
        _btnStartVideo.Margin = new Padding(0, 2, 4, 3);
        bar.Controls.Add(_btnStartVideo, 4, 0);

        _btnStopVideo = Theme.Ghost("停止");
        _btnStopVideo.Name = "btnStopVideo";
        Stretch(_btnStopVideo, 76, 36);
        _btnStopVideo.Anchor = AnchorStyles.Left;
        _btnStopVideo.Enabled = false;
        _btnStopVideo.Margin = new Padding(0, 2, 0, 3);
        bar.Controls.Add(_btnStopVideo, 5, 0);
        return bar;
    }

    private Control BuildVideoOptions()
    {
        var host = new TableLayoutPanel
        {
            Dock = DockStyle.Top,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            ColumnCount = 1,
            RowCount = 2,
            BackColor = Theme.Bg,
            Margin = new Padding(0, 0, 0, 6),
        };
        host.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
        host.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        host.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        var flow = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            WrapContents = false,
            BackColor = Theme.Bg,
            Margin = Padding.Empty,
            Padding = new Padding(0, 0, 0, 2),
        };
        host.Controls.Add(flow, 0, 0);

        _chkVideoLoop = new CheckBox
        {
            Text = "循环播放",
            AutoSize = true,
            Font = Theme.Body,
            ForeColor = Theme.Muted,
            Margin = new Padding(2, 8, 12, 4),
        };
        flow.Controls.Add(_chkVideoLoop);

        flow.Controls.Add(FieldLabel("跳帧间隔"));
        _numVideoStep = new NumericUpDown
        {
            Minimum = 1,
            Maximum = 12,
            Value = Math.Clamp(_settings.VideoFrameStep, 1, 12),
            Width = 54,
            Font = Theme.Body,
            TextAlign = HorizontalAlignment.Right,
            Margin = new Padding(0, 5, 14, 4),
        };
        flow.Controls.Add(_numVideoStep);

        _btnDrawLine = Theme.Ghost("画计数线");
        Stretch(_btnDrawLine, 92, 32);
        _btnDrawLine.Margin = new Padding(0, 2, 4, 2);
        _btnDrawLine.Enabled = false;
        flow.Controls.Add(_btnDrawLine);

        _btnClearLine = Theme.Ghost("清除线");
        Stretch(_btnClearLine, 76, 32);
        _btnClearLine.Margin = new Padding(0, 2, 12, 2);
        _btnClearLine.Enabled = false;
        flow.Controls.Add(_btnClearLine);

        _lblVideoLine = new Label
        {
            Text = "计数线：默认画面中线",
            AutoSize = true,
            Font = Theme.Small,
            ForeColor = Theme.Muted,
            Margin = new Padding(0, 11, 12, 4),
        };
        flow.Controls.Add(_lblVideoLine);

        var flow2 = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            WrapContents = false,
            BackColor = Theme.Bg,
            Margin = Padding.Empty,
            Padding = Padding.Empty,
        };

        flow2.Controls.Add(FieldLabel("模型类型"));
        _cboVideoTask = new ComboBox
        {
            Name = "cboVideoTask",
            DropDownStyle = ComboBoxStyle.DropDownList,
            Width = 96,
            Font = Theme.Body,
            Margin = new Padding(0, 6, 12, 4),
        };
        _cboVideoTask.Items.Add("分割（掩码）");
        _cboVideoTask.Items.Add("检测（方框）");
        _cboVideoTask.SelectedIndex = 0;
        flow2.Controls.Add(_cboVideoTask);

        flow2.Controls.Add(FieldLabel("置信度"));
        _numVideoConf = new NumericUpDown
        {
            Name = "numVideoConf",
            Minimum = 0.05m,
            Maximum = 0.95m,
            Increment = 0.05m,
            DecimalPlaces = 2,
            Value = (decimal)Math.Clamp(_settings.VideoConfidence, 0.05f, 0.95f),
            Width = 62,
            Font = Theme.Body,
            TextAlign = HorizontalAlignment.Right,
            Margin = new Padding(0, 6, 12, 4),
        };
        flow2.Controls.Add(_numVideoConf);

        flow2.Controls.Add(FieldLabel("类别"));
        _tbVideoClasses = new TextBox
        {
            Name = "tbVideoClasses",
            Width = 128,
            Font = Theme.Body,
            Text = _settings.VideoClassFilter ?? "",
            PlaceholderText = "留空=全部",
            Margin = new Padding(0, 7, 12, 4),
        };
        flow2.Controls.Add(_tbVideoClasses);

        _chkVideoBest = new CheckBox
        {
            Name = "chkVideoBest",
            Text = "只留最高分",
            AutoSize = true,
            Font = Theme.Body,
            ForeColor = Theme.Muted,
            Checked = _settings.VideoOnlyBest,
            Margin = new Padding(0, 9, 12, 4),
        };
        flow2.Controls.Add(_chkVideoBest);

        _btnVideoModel = Theme.Link("更换模型");
        _btnVideoModel.Margin = new Padding(0, 6, 4, 2);
        flow2.Controls.Add(_btnVideoModel);

        _lblVideoModel = new Label
        {
            Text = "分割模型：",
            AutoSize = true,
            Font = Theme.Small,
            ForeColor = Theme.Muted,
            Margin = new Padding(0, 12, 12, 4),
        };
        flow2.Controls.Add(_lblVideoModel);

        _lblVideoInfo = new Label
        {
            Text = "就绪",
            AutoSize = false,
            Width = 210,
            AutoEllipsis = true,
            Font = Theme.Small,
            ForeColor = Theme.Accent,
            Margin = new Padding(0, 12, 0, 4),
        };
        flow2.Controls.Add(_lblVideoInfo);

        host.Controls.Add(flow2, 0, 1);
        return host;
    }

    private Control BuildVideoBody()
    {
        _picVideo = new ImageCanvas
        {
            Dock = DockStyle.Fill,
            Name = "picVideo",
            Editable = false,
            ShowZoomInfo = false,
            EmptyText = "① 选择视频或摄像头\r\n② 点“开始分析”",
            TrackLabel = t => $"#{t.Id} {_labels.Display(t.Item.RawCategory, t.Item.ClassId)} {t.Item.Confidence:P0}",
            Margin = Padding.Empty,
        };
        _splitVideo = new SplitContainer
        {
            Dock = DockStyle.Fill,
            Size = new Size(1000, 600),
            FixedPanel = FixedPanel.Panel2,
            Panel1MinSize = 380,
            Panel2MinSize = 330,
            SplitterWidth = 6,
            Margin = new Padding(0, 2, 0, 0),
            BackColor = Theme.Bg,
        };
        _splitVideo.Panel1.Controls.Add(_picVideo);
        _splitVideo.Panel1.BackColor = Color.FromArgb(36, 40, 47);
        _splitVideo.Panel2.Controls.Add(BuildVideoSidePanel());
        return _splitVideo;
    }

    private Control BuildVideoSidePanel()
    {
        var panel = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White,
            Padding = new Padding(12, 10, 10, 10),
        };
        var t = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 5,
            BackColor = Color.White,
        };
        t.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        t.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        t.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        t.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
        t.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        t.Controls.Add(new Label
        {
            Text = "跟踪与分割",
            Font = Theme.Section,
            ForeColor = Theme.Ink,
            AutoSize = true,
            Margin = new Padding(0, 0, 0, 6),
        }, 0, 0);

        var counters = new TableLayoutPanel
        {
            Dock = DockStyle.Top,
            AutoSize = true,
            ColumnCount = 4,
            RowCount = 1,
            Margin = new Padding(0, 0, 0, 8),
        };
        for (int i = 0; i < 4; i++) counters.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
        counters.Controls.Add(Metric("活跃 ID", out _lblVideoActive), 0, 0);
        counters.Controls.Add(Metric("累计 ID", out _lblVideoTotal), 1, 0);
        counters.Controls.Add(Metric("A→B", out _lblVideoAtoB), 2, 0);
        counters.Controls.Add(Metric("B→A", out _lblVideoBtoA), 3, 0);
        _lblVideoActive.Name = "lblVideoActive";
        _lblVideoTotal.Name = "lblVideoTotal";
        _lblVideoAtoB.Name = "lblVideoAtoB";
        _lblVideoBtoA.Name = "lblVideoBtoA";
        t.Controls.Add(counters, 0, 1);

        var stats = new TableLayoutPanel
        {
            Dock = DockStyle.Top,
            AutoSize = true,
            ColumnCount = 1,
            RowCount = 2,
            Margin = new Padding(0, 0, 0, 6),
        };
        _lblVideoStats = new Label
        {
            Name = "lblVideoStats",
            Text = "尚未开始。",
            Font = Theme.Small,
            ForeColor = Theme.Muted,
            AutoSize = true,
            Margin = new Padding(0, 0, 0, 2),
        };
        _lblVideoArea = new Label
        {
            Name = "lblVideoArea",
            Text = "分割面积：—",
            Font = Theme.Small,
            ForeColor = Theme.Muted,
            AutoSize = true,
            Margin = new Padding(0, 0, 0, 4),
        };
        stats.Controls.Add(_lblVideoStats, 0, 0);
        stats.Controls.Add(_lblVideoArea, 0, 1);
        t.Controls.Add(stats, 0, 2);

        _gridVideo = new DataGridView
        {
            Name = "gridVideo",
            Dock = DockStyle.Fill,
            Margin = Padding.Empty,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            AllowUserToResizeRows = false,
            ReadOnly = true,
            RowHeadersVisible = false,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
        };
        Theme.StyleGrid(_gridVideo);
        AddVideoColumn("colId", "ID", 8, 44);
        AddVideoColumn("colName", "类别", 24, 64);
        AddVideoColumn("colConf", "匹配度", 20, 66);
        AddVideoColumn("colArea", "面积(px)", 26, 78);
        AddVideoColumn("colPct", "占比", 18, 56);
        t.Controls.Add(_gridVideo, 0, 3);

        t.Controls.Add(new Label
        {
            Text = "轨迹按 ID 着色；画计数线后，目标中心从 A 侧穿到 B 侧各计一次。",
            Font = Theme.Small,
            ForeColor = Theme.Muted,
            AutoSize = false,
            Dock = DockStyle.Fill,
            Height = 42,
            TextAlign = ContentAlignment.MiddleLeft,
            Margin = new Padding(0, 6, 0, 0),
        }, 0, 4);

        panel.Controls.Add(t);
        return panel;
    }

    private void AddVideoColumn(string name, string header, int weight, int minWidth) =>
        _gridVideo.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = name,
            HeaderText = header,
            FillWeight = weight,
            MinimumWidth = minWidth,
            SortMode = DataGridViewColumnSortMode.NotSortable,
        });

    private static Control Metric(string caption, out Label value)
    {
        var p = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
            ColumnCount = 1,
            RowCount = 2,
            BackColor = Theme.PaleBlue,
            Margin = new Padding(0, 0, 6, 0),
            Padding = new Padding(7, 4, 7, 5),
        };
        p.Controls.Add(new Label
        {
            Text = caption,
            Font = Theme.Small,
            ForeColor = Theme.Muted,
            AutoSize = true,
            Margin = new Padding(0, 0, 0, 1),
        }, 0, 0);
        value = new Label
        {
            Text = "0",
            Font = Theme.Section,
            ForeColor = Theme.Ink,
            AutoSize = true,
            Margin = Padding.Empty,
        };
        p.Controls.Add(value, 0, 1);
        return p;
    }

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
