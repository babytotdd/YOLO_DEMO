using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DeploySharp.Engine;
using Cv2 = OpenCvSharp.Cv2;
using CvPoint = OpenCvSharp.Point;
using CvRect = OpenCvSharp.Rect;
using CvScalar = OpenCvSharp.Scalar;
using Hershey = OpenCvSharp.HersheyFonts;
using LineTypes = OpenCvSharp.LineTypes;
using Mat = OpenCvSharp.Mat;

namespace YoloFool;

public sealed partial class MainForm
{
    // ---------------- 状态 ----------------
    private TabPage _pageAnchor;
    private ImageCanvas _picAnchor;
    private ComboBox _cboAnchorModel;
    private ComboBox _cboAnchorCamIndex;
    private ComboBox _cboAnchorRes;
    private CheckBox _chkAnchorExposure;
    private NumericUpDown _numAnchorExposure;
    private Button _btnAnchorCam;
    private Button _btnAnchorCalib;
    private Button _btnAnchorJudge;
    private Label _lblAnchorCamInfo;
    private Label _lblAnchorModelInfo;
    private Label _lblAnchorVerdict;
    private Label _lblAnchorReason;
    private Label _lblAnchorDetail;
    private Label _lblAnchorCalibInfo;
    private Label _lblAnchorJitter;
    private ListView _lvAnchorHits;
    private ListView _lvAnchorLog;
    private NumericUpDown _numAnchorShift;
    private NumericUpDown _numAnchorRot;
    private NumericUpDown _numAnchorScale;
    private NumericUpDown _numAnchorRms;
    private NumericUpDown _numAnchorConf;
    private NumericUpDown _numAnchorMin;
    private ToolTip _anchorTip;

    private System.Windows.Forms.Timer _anchorTimer;
    private FixtureCamera _anchorCam;
    private InferenceService _anchorSvc;
    private AnchorProfile _anchorProfile;
    private AnchorVerdict _anchorLastVerdict;
    private readonly AnchorJitter _anchorJitter = new();
    private string _anchorModel = "";
    private long _anchorShownFrame = -1;
    private bool _anchorActive;
    private bool _anchorBusy;
    private bool _anchorCameraBusy;
    private bool _anchorModelLoading;
    private bool _anchorOverlayOn = true;
    private TabControl _anchorTabs;

    // ---------------- 检测范围 / 识别目标 ----------------
    private ListView _lvAnchorZones;
    private ListView _lvAnchorTargets;
    private Button _btnAnchorZonePick;
    private Button _btnAnchorZoneEdit;
    private CheckBox _chkAnchorAdv;
    private Panel _pnlAnchorTol;
    private PictureBox _picZoneRef;
    private Label _lblZoneRefInfo;
    private Label _lblAnchorZoneInfo;
    private Label _lblAnchorTargetInfo;
    private NumericUpDown _numZoneMatch;
    private NumericUpDown _numZoneSearch;
    private NumericUpDown _numZoneShift;
    private ComboBox _cboZoneClass;
    private CheckBox _chkZoneRequired;
    private TextBox _txtAnchorTrainLog;
    private NumericUpDown _numTrainEpochs;
    private NumericUpDown _numTrainImgsz;
    private AnchorZoneReport _anchorZoneReport;
    private bool _anchorZonePicking;
    private bool _anchorTargetPicking;
    private bool _anchorTraining;
    private bool _anchorTrainerHooked;
    private bool _zoneUiSync;
    private TrainingService _anchorTrainer;
    private Mat _anchorRaw;

    // ---------------- 页面骨架 ----------------
    private TabPage BuildAnchorPage()
    {
        _pageAnchor = new TabPage("针床定位 V2") { BackColor = Theme.Bg, Padding = new Padding(8) };

        var root = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 2,
            BackColor = Theme.Bg,
        };
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
        // 总判定放在右上角（与右侧操作栏同宽），工具栏在左：左边看画面，右边操作和看结果。
        var top = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 1, BackColor = Theme.Bg, Margin = Padding.Empty };
        top.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
        top.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 412f));
        top.Controls.Add(BuildAnchorToolbar(), 0, 0);
        top.Controls.Add(BuildAnchorVerdictCard(), 1, 0);
        root.Controls.Add(top, 0, 0);
        root.Controls.Add(BuildAnchorBody(), 0, 1);

        _pageAnchor.Controls.Add(root);
        return _pageAnchor;
    }

    private Control BuildAnchorToolbar()
    {
        var bar = new TableLayoutPanel
        {
            Dock = DockStyle.Top,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            ColumnCount = 1,
            RowCount = 2,
            BackColor = Theme.Bg,
            Margin = new Padding(0, 0, 0, 6),
        };
        bar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
        bar.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        bar.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        var row1 = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, WrapContents = false, BackColor = Theme.Bg, Margin = Padding.Empty };
        row1.Controls.Add(new Label { Text = "机种", AutoSize = true, ForeColor = Theme.Muted, Font = Theme.Body, Margin = new Padding(0, 9, 6, 0) });
        _cboAnchorModel = new ComboBox { Width = 150, DropDownStyle = ComboBoxStyle.DropDownList, Font = Theme.Body, Margin = new Padding(0, 5, 6, 0) };
        row1.Controls.Add(_cboAnchorModel);
        var btnNewModel = Theme.Ghost("新建机种");
        btnNewModel.Margin = new Padding(0, 5, 6, 0);
        btnNewModel.Click += (_, __) => CreateAnchorModel();
        row1.Controls.Add(btnNewModel);
        var btnCopyModel = Theme.Ghost("复制当前模型");
        btnCopyModel.Margin = new Padding(0, 5, 10, 0);
        btnCopyModel.Click += (_, __) => CopyAnchorModel();
        row1.Controls.Add(btnCopyModel);
        _lblAnchorModelInfo = new Label { Text = "模型：未加载", AutoSize = true, ForeColor = Theme.Muted, Font = Theme.Small, Margin = new Padding(0, 10, 0, 0) };
        row1.Controls.Add(_lblAnchorModelInfo);
        bar.Controls.Add(row1, 0, 0);

        var row2 = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, WrapContents = false, BackColor = Theme.Bg, Margin = Padding.Empty };
        row2.Controls.Add(new Label { Text = "相机", AutoSize = true, ForeColor = Theme.Muted, Font = Theme.Body, Margin = new Padding(0, 9, 6, 0) });
        _cboAnchorCamIndex = new ComboBox { Width = 52, DropDownStyle = ComboBoxStyle.DropDownList, Font = Theme.Body, Margin = new Padding(0, 5, 6, 0) };
        for (var i = 0; i < 6; i++) _cboAnchorCamIndex.Items.Add(i.ToString());
        _cboAnchorCamIndex.SelectedIndex = 0;
        row2.Controls.Add(_cboAnchorCamIndex);

        _cboAnchorRes = new ComboBox { Width = 108, DropDownStyle = ComboBoxStyle.DropDownList, Font = Theme.Body, Margin = new Padding(0, 5, 10, 0) };
        _cboAnchorRes.Items.Add("1920×1080");
        _cboAnchorRes.Items.Add("1280×720");
        _cboAnchorRes.SelectedIndex = 0;
        row2.Controls.Add(_cboAnchorRes);

        _chkAnchorExposure = new CheckBox { Text = "曝光", AutoSize = true, ForeColor = Theme.Muted, Font = Theme.Body, Margin = new Padding(0, 8, 4, 0) };
        row2.Controls.Add(_chkAnchorExposure);
        _numAnchorExposure = new NumericUpDown
        {
            Minimum = -13,
            Maximum = 0,
            Value = -5,
            Width = 58,
            Enabled = false,
            Font = Theme.Body,
            TextAlign = HorizontalAlignment.Right,
            Margin = new Padding(0, 5, 10, 0),
        };
        _chkAnchorExposure.CheckedChanged += (_, __) => _numAnchorExposure.Enabled = _chkAnchorExposure.Checked;
        row2.Controls.Add(_numAnchorExposure);

        var btnShot = Theme.Success("拍照");
        btnShot.Margin = new Padding(0, 5, 8, 0);
        btnShot.Click += async (_, __) => await CaptureAnchorPhotoAsync();
        row2.Controls.Add(btnShot);

        _btnAnchorCam = Theme.Primary("打开相机");
        _btnAnchorCam.Margin = new Padding(0, 5, 8, 0);
        _btnAnchorCam.Click += async (_, __) => await ToggleAnchorCameraAsync();
        row2.Controls.Add(_btnAnchorCam);

        // 「判定一次」放在相机旁边（m01839）：换板后不用切页签，直接点这里。
        _btnAnchorJudge = Theme.Primary("判定一次");
        _btnAnchorJudge.Margin = new Padding(0, 5, 8, 0);
        _btnAnchorJudge.Click += async (_, __) => await AnchorJudgeOnceAsync();
        row2.Controls.Add(_btnAnchorJudge);

        _lblAnchorCamInfo = new Label { Text = "相机未打开", AutoSize = true, ForeColor = Theme.Muted, Font = Theme.Small, Margin = new Padding(0, 10, 0, 0) };
        row2.Controls.Add(_lblAnchorCamInfo);
        bar.Controls.Add(row2, 0, 1);

        return bar;
    }

    private Control BuildAnchorBody()
    {
        var body = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 1, BackColor = Theme.Bg };
        body.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
        body.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 412f));

        _picAnchor = new ImageCanvas
        {
            Dock = DockStyle.Fill,
            Margin = new Padding(0, 0, 10, 0),
            EmptyText = "① 选机种，把板放上针床\r\n② 打开相机 → 点上方【拍照】\r\n③ 在「① 识别目标」里框选特征（右键确认后才入队）\r\n④ 在「② 检测范围 · 判定」里框范围、采集参考、设匹配率\r\n⑤ 换板后点“判定一次”，结果显示在右上角",
        };
        body.Controls.Add(_picAnchor, 0, 0);
        body.Controls.Add(BuildAnchorSide(), 1, 0);
        return body;
    }

    private Control BuildAnchorSide()
    {
        var host = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 1, BackColor = Theme.Bg };
        host.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
        host.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

        var tabs = new TabControl { Dock = DockStyle.Fill, Font = Theme.Body };
        var targetTab = new TabPage("① 识别目标") { BackColor = Theme.Bg, Padding = new Padding(4) };
        targetTab.Controls.Add(BuildAnchorLearnTab());
        var zoneTab = new TabPage("② 检测范围 · 判定") { BackColor = Theme.Bg, Padding = new Padding(4) };
        zoneTab.Controls.Add(BuildAnchorZoneTab());
        var calibTab = new TabPage("③ 拍基准 · 对比") { BackColor = Theme.Bg, Padding = new Padding(4) };
        calibTab.Controls.Add(BuildAnchorCalibTab());
        tabs.TabPages.Add(targetTab);
        tabs.TabPages.Add(zoneTab);
        tabs.TabPages.Add(calibTab);
        tabs.SelectedIndexChanged += (_, __) =>
        {
            // 离开第三页就解除“基准/对比画面冻结”，否则实时预览会一直停在对比图。
            if (!ReferenceEquals(tabs.SelectedTab, calibTab)) _anchorCalibView = false;
        };
        _anchorTabs = tabs;
        host.Controls.Add(tabs, 0, 0);
        return host;
    }

    /// <summary>判定结论固定显示在右上角，换页签也能看到。</summary>
    private Control BuildAnchorVerdictCard()
    {
        var card = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Theme.Card,
            MinimumSize = new Size(0, 96),
            Size = new Size(0, 96),
            Margin = new Padding(8, 0, 0, 6),
            Padding = new Padding(10, 6, 10, 6),
        };
        var grid = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 2, BackColor = Theme.Card };
        grid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 96f));
        grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
        grid.RowStyles.Add(new RowStyle(SizeType.Absolute, 44f));
        grid.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

        _lblAnchorVerdict = new Label
        {
            Dock = DockStyle.Fill,
            Text = "—",
            Font = new Font("Segoe UI", 24f, FontStyle.Bold),
            ForeColor = Theme.Muted,
            TextAlign = ContentAlignment.MiddleCenter,
            Margin = Padding.Empty,
        };
        _lblAnchorReason = new Label
        {
            Dock = DockStyle.Fill,
            Text = "还没有判定",
            Font = Theme.BodyBold,
            ForeColor = Theme.Muted,
            TextAlign = ContentAlignment.MiddleLeft,
            Margin = new Padding(8, 0, 0, 0),
        };
        _lblAnchorDetail = new Label
        {
            Dock = DockStyle.Fill,
            Text = "放好板 → 到「② 检测范围 · 判定」点“判定一次”。",
            Font = Theme.Small,
            ForeColor = Theme.Muted,
            AutoSize = false,
            TextAlign = ContentAlignment.TopLeft,
            Margin = new Padding(0, 4, 0, 0),
        };

        grid.Controls.Add(_lblAnchorVerdict, 0, 0);
        grid.Controls.Add(_lblAnchorReason, 1, 0);
        grid.Controls.Add(_lblAnchorDetail, 0, 1);
        grid.SetColumnSpan(_lblAnchorDetail, 2);
        card.Controls.Add(grid);
        return card;
    }

    // 旧版“③ 判定”页已被上方 BuildAnchorVerdictCard + BuildAnchorZoneTab 取代，保留备查，不再调用。
    private Control BuildAnchorJudgeTab()
    {
        var side = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 7, BackColor = Theme.Bg };
        side.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
        side.RowStyles.Add(new RowStyle(SizeType.Absolute, 92f));
        side.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        side.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        side.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        side.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
        side.RowStyles.Add(new RowStyle(SizeType.Absolute, 168f));
        side.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        // 结论
        var verdictPanel = new Panel { Dock = DockStyle.Fill, BackColor = Theme.Card, Margin = new Padding(0, 0, 0, 8), Padding = new Padding(10) };
        _lblAnchorVerdict = new Label
        {
            Dock = DockStyle.Fill,
            Text = "—",
            Font = new Font("Segoe UI", 26f, FontStyle.Bold),
            ForeColor = Theme.Muted,
            TextAlign = ContentAlignment.MiddleCenter,
        };
        verdictPanel.Controls.Add(_lblAnchorVerdict);
        side.Controls.Add(verdictPanel, 0, 0);

        // 原因 + 明细
        _lblAnchorReason = new Label
        {
            Text = "还没有拍基准",
            AutoSize = true,
            MaximumSize = new Size(400, 0),
            Font = Theme.BodyBold,
            ForeColor = Theme.Ink,
            Margin = new Padding(2, 0, 2, 3),
        };
        _lblAnchorDetail = new Label
        {
            Text = "只用「② 检测范围」也能判定；想同时看整体平移/旋转，先在放好的板上点一次“拍基准”。换板只需点“判定一次”。",
            AutoSize = true,
            MaximumSize = new Size(400, 0),
            Font = Theme.Small,
            ForeColor = Theme.Muted,
            Margin = new Padding(2, 0, 2, 8),
        };
        var reasonWrap = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            BackColor = Theme.Bg,
            Margin = Padding.Empty,
        };
        reasonWrap.Controls.Add(_lblAnchorReason);
        reasonWrap.Controls.Add(_lblAnchorDetail);
        side.Controls.Add(reasonWrap, 0, 1);

        // 按钮
        var buttons = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            WrapContents = false,
            BackColor = Theme.Bg,
            Margin = new Padding(0, 0, 0, 8),
        };
        _btnAnchorCalib = Theme.Ghost("拍基准");
        _btnAnchorCalib.Margin = new Padding(0, 0, 6, 0);
        _btnAnchorCalib.Click += async (_, __) => await AnchorCalibrateAsync();
        buttons.Controls.Add(_btnAnchorCalib);

        _btnAnchorJudge = Theme.Primary("判定一次");
        _btnAnchorJudge.Margin = new Padding(0, 0, 6, 0);
        _btnAnchorJudge.Click += async (_, __) => await AnchorJudgeOnceAsync();
        buttons.Controls.Add(_btnAnchorJudge);

        var btnSave = Theme.Ghost("保存容差");
        btnSave.Margin = new Padding(0, 0, 6, 0);
        btnSave.Click += (_, __) => SaveAnchorTolerance();
        buttons.Controls.Add(btnSave);

        var btnOverlay = Theme.Link("叠加：开");
        btnOverlay.AutoSize = true;
        btnOverlay.Margin = new Padding(0, 8, 0, 0);
        btnOverlay.Click += (_, __) =>
        {
            _anchorOverlayOn = !_anchorOverlayOn;
            btnOverlay.Text = _anchorOverlayOn ? "叠加：开" : "叠加：关";
        };
        buttons.Controls.Add(btnOverlay);
        side.Controls.Add(buttons, 0, 2);

        // 容差
        side.Controls.Add(AnchorCard("判定容差（像素）", BuildAnchorToleranceGrid(), DockStyle.Top), 0, 3);

        // 锚点表
        _lvAnchorHits = new ListView
        {
            Dock = DockStyle.Fill,
            View = View.Details,
            FullRowSelect = true,
            HeaderStyle = ColumnHeaderStyle.Nonclickable,
            BorderStyle = BorderStyle.None,
            Font = Theme.Small,
            BackColor = Theme.Card,
            ForeColor = Theme.Ink,
        };
        _lvAnchorHits.Columns.Add("锚点", 88);
        _lvAnchorHits.Columns.Add("期望", 82);
        _lvAnchorHits.Columns.Add("当前", 82);
        _lvAnchorHits.Columns.Add("偏移", 76);
        _lvAnchorHits.Columns.Add("置信", 52);
        side.Controls.Add(AnchorCard("锚点对照", _lvAnchorHits, DockStyle.Fill), 0, 4);

        // 最近记录
        _lvAnchorLog = new ListView
        {
            Dock = DockStyle.Fill,
            View = View.Details,
            FullRowSelect = true,
            HeaderStyle = ColumnHeaderStyle.Nonclickable,
            BorderStyle = BorderStyle.None,
            Font = Theme.Small,
            BackColor = Theme.Card,
            ForeColor = Theme.Ink,
        };
        _lvAnchorLog.Columns.Add("时间", 62);
        _lvAnchorLog.Columns.Add("结论", 44);
        _lvAnchorLog.Columns.Add("原因", 132);
        _lvAnchorLog.Columns.Add("偏移", 108);
        side.Controls.Add(AnchorCard("最近判定", _lvAnchorLog, DockStyle.Fill), 0, 5);

        // 提示
        var hints = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            BackColor = Theme.Bg,
            Margin = Padding.Empty,
        };
        _lblAnchorCalibInfo = new Label { Text = "基准：还没有拍", AutoSize = true, MaximumSize = new Size(400, 0), Font = Theme.Small, ForeColor = Theme.Muted, Margin = new Padding(2, 0, 2, 2) };
        _lblAnchorJitter = new Label { Text = "", AutoSize = true, MaximumSize = new Size(400, 0), Font = Theme.Small, ForeColor = Theme.Muted, Margin = new Padding(2, 0, 2, 0) };
        hints.Controls.Add(_lblAnchorCalibInfo);
        hints.Controls.Add(_lblAnchorJitter);
        side.Controls.Add(hints, 0, 6);

        _anchorTip = new ToolTip();
        return side;
    }

    private Control BuildAnchorToleranceGrid()
    {
        var grid = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            ColumnCount = 2,
            RowCount = 6,
            BackColor = Theme.Card,
        };
        grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
        grid.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        for (var i = 0; i < 6; i++) grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        _numAnchorShift = AnchorNum(1, 200, 12, 1, 0);
        _numAnchorRot = AnchorNum(0.1m, 30, 0.8m, 0.1m, 2);
        _numAnchorScale = AnchorNum(0.005m, 0.2m, 0.02m, 0.005m, 3);
        _numAnchorRms = AnchorNum(1, 100, 6, 1, 0);
        _numAnchorConf = AnchorNum(0.05m, 0.95m, 0.35m, 0.05m, 2);
        _numAnchorMin = AnchorNum(1, 6, 2, 1, 0);

        AddAnchorRow(grid, 0, "平移上限", _numAnchorShift);
        AddAnchorRow(grid, 1, "旋转上限 (°)", _numAnchorRot);
        AddAnchorRow(grid, 2, "间距变化上限", _numAnchorScale);
        AddAnchorRow(grid, 3, "多点残差 RMS", _numAnchorRms);
        AddAnchorRow(grid, 4, "检出置信度", _numAnchorConf);
        AddAnchorRow(grid, 5, "最少锚点数", _numAnchorMin);

        foreach (var num in new[] { _numAnchorShift, _numAnchorRot, _numAnchorScale, _numAnchorRms, _numAnchorConf, _numAnchorMin })
        {
            num.ValueChanged += (_, __) => ApplyAnchorToleranceToProfile();
        }
        return grid;
    }

    private static NumericUpDown AnchorNum(decimal min, decimal max, decimal value, decimal step, int decimals) => new NoWheelNumeric()
    {
        Minimum = min,
        Maximum = max,
        Value = Math.Clamp(value, min, max),
        Increment = step,
        DecimalPlaces = decimals,
        Width = 84,
        Font = Theme.Body,
        TextAlign = HorizontalAlignment.Right,
        Margin = new Padding(0, 2, 0, 2),
    };

    private static void AddAnchorRow(TableLayoutPanel grid, int row, string text, Control input)
    {
        var label = new Label
        {
            Text = text,
            AutoSize = true,
            ForeColor = Theme.Muted,
            Font = Theme.Small,
            Margin = new Padding(0, 6, 6, 2),
            Anchor = AnchorStyles.Left,
        };
        grid.Controls.Add(label, 0, row);
        grid.Controls.Add(input, 1, row);
    }

    private static Panel AnchorCard(string title, Control content, DockStyle dock = DockStyle.Top)
    {
        var card = new Panel
        {
            BackColor = Theme.Card,
            Dock = dock,
            Margin = new Padding(0, 0, 0, 8),
            Padding = new Padding(10, 8, 10, 8),
        };
        var autoSize = dock == DockStyle.Top;
        card.AutoSize = autoSize;
        if (autoSize) card.AutoSizeMode = AutoSizeMode.GrowAndShrink;

        var grid = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 2,
            BackColor = Theme.Card,
            AutoSize = autoSize,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
        };
        grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
        grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        grid.RowStyles.Add(autoSize ? new RowStyle(SizeType.AutoSize) : new RowStyle(SizeType.Percent, 100f));

        var head = new Label { Text = title, AutoSize = true, ForeColor = Theme.Muted, Font = Theme.BodyBold, Margin = new Padding(0, 0, 0, 6) };
        content.Dock = DockStyle.Fill;
        content.Margin = Padding.Empty;
        grid.Controls.Add(head, 0, 0);
        grid.Controls.Add(content, 0, 1);
        card.Controls.Add(grid);
        return card;
    }

    // ---------------- 事件接线 ----------------
    private void WireAnchorEvents()
    {
        _cboAnchorModel.SelectedIndexChanged += AnchorModelChanged;
        _picAnchor.BoxDrawn += OnAnchorBoxDrawn;
        _picAnchor.BoxEdited += OnAnchorBoxEdited;
        _picAnchor.SelectionChanged += OnAnchorDraftSelectionChanged;
        _picAnchor.KeyDown += AnchorCanvasKeyDown;
        _tabs.SelectedIndexChanged += (_, __) => OnAnchorTabChanged();
        FormClosing += (_, __) =>
        {
            _anchorTimer?.Stop();
            _anchorTrainer?.Cancel();
            ExitAnchorEditMode();
            CloseAnchorCamera();
            DisposeAnchorService();
            _anchorRaw?.Dispose();
            _anchorRaw = null;
        };
    }

    private void OnAnchorTabChanged()
    {
        var on = _tabs != null && ReferenceEquals(_tabs.SelectedTab, _pageAnchor);
        _anchorActive = on;
        if (on)
        {
            if (_anchorProfile == null) RefreshAnchorModels();
            else if (_anchorCam != null) _anchorTimer?.Start();
            _anchorTimer?.Start();
        }
        else
        {
            _anchorTimer?.Stop();
            ExitAnchorEditMode();
            CloseAnchorCamera();
        }
    }

    // ---------------- 机种与模型 ----------------
    private void RefreshAnchorModels()
    {
        var models = AnchorStore.ListModels().ToList();
        if (models.Count == 0)
        {
            AnchorStore.EnsureModel("B001");
            models.Add("B001");
        }

        _anchorModelLoading = true;
        try
        {
            _cboAnchorModel.Items.Clear();
            foreach (var name in models) _cboAnchorModel.Items.Add(name);
            var index = models.IndexOf(_anchorModel);
            _cboAnchorModel.SelectedIndex = index >= 0 ? index : 0;
        }
        finally
        {
            _anchorModelLoading = false;
        }

        _anchorModel = _cboAnchorModel.SelectedItem as string ?? models[0];
        _anchorProfile = AnchorStore.LoadOrCreate(_anchorModel);
        ApplyAnchorToUi();

        if (AnchorStore.LocalModelPath(_anchorModel).Length == 0)
        {
            _lblAnchorModelInfo.Text = "本机种还没有模型副本";
            _lblAnchorModelInfo.ForeColor = Theme.Warn;
            DisposeAnchorService();
            return;
        }
        _ = LoadAnchorServiceAsync();
    }

    private async void AnchorModelChanged(object sender, EventArgs e)
    {
        if (_anchorModelLoading) return;
        var model = _cboAnchorModel.SelectedItem as string;
        if (string.IsNullOrWhiteSpace(model) || string.Equals(model, _anchorModel, StringComparison.OrdinalIgnoreCase)) return;

        _anchorModel = model;
        _anchorProfile = AnchorStore.LoadOrCreate(model);
        _anchorJitter.Clear();
        ApplyAnchorToUi();
        _anchorZoneReport = null;
        RefreshZoneClassCombo();
        ShowAnchorZones();
        ShowAnchorTargets();
        DisposeAnchorService();
        await LoadAnchorServiceAsync();
    }

    private void CreateAnchorModel()
    {
        var name = "机种-" + DateTime.Now.ToString("MMdd-HHmm");
        AnchorStore.EnsureModel(name);
        _anchorModel = name;
        _anchorProfile = AnchorStore.LoadOrCreate(name);
        AnchorStore.Save(_anchorProfile);
        _anchorZoneReport = null;
        RefreshAnchorModels();
        RefreshZoneClassCombo();
        ShowAnchorZones();
        ShowAnchorTargets();
        SetAnchorCamInfo($"已新建机种 {name}，请先训练模型或点“复制当前模型”。");
    }

    private void CopyAnchorModel()
    {
        if (string.IsNullOrWhiteSpace(_anchorModel)) return;
        if (!AnchorStore.CopyFromGlobalModel(_anchorModel, out var error))
        {
            SetAnchorCamInfo(error);
            return;
        }
        SetAnchorCamInfo("模型已复制到本机种。");
        _ = LoadAnchorServiceAsync();
    }

    private async Task LoadAnchorServiceAsync()
    {
        var model = _anchorModel;
        var path = AnchorStore.LocalModelPath(model);
        if (path.Length == 0 || _anchorProfile == null) return;

        ApplyAnchorToleranceToProfile();
        var conf = (float)_anchorProfile.Infer.Conf;
        var nms = (float)_anchorProfile.Infer.Nms;
        _lblAnchorModelInfo.Text = "模型加载中…";
        _lblAnchorModelInfo.ForeColor = Theme.Muted;
        try
        {
            var svc = await Task.Run(() =>
            {
                var service = new InferenceService();
                service.Load(new ModelOptions(
                    path,
                    YoloModelKind.YOLOv8Det,
                    InferenceBackend.OnnxRuntime,
                    DeviceType.CPU,
                    OnnxRuntimeDeviceType.Default,
                    conf,
                    nms));
                service.Warmup();
                return service;
            });

            if (!string.Equals(model, _anchorModel, StringComparison.OrdinalIgnoreCase))
            {
                svc.Dispose();
                return;
            }

            DisposeAnchorService();
            _anchorSvc = svc;
            var info = svc.Info;
            var names = info?.ClassNames != null ? string.Join("、", info.ClassNames) : "?";
            _lblAnchorModelInfo.Text = $"{Path.GetFileName(path)} · {info?.Kind} · 类别 {info?.ClassNames?.Count ?? 0}：{names}";
            _lblAnchorModelInfo.ForeColor = Theme.Muted;
            RefreshZoneClassCombo();
            ShowAnchorZones();
            ShowAnchorTargets();
            UpdateAnchorClassLabel();
        }
        catch (Exception ex)
        {
            _lblAnchorModelInfo.Text = "模型加载失败：" + ex.Message;
            _lblAnchorModelInfo.ForeColor = Theme.Danger;
        }
    }

    private void DisposeAnchorService()
    {
        var svc = _anchorSvc;
        _anchorSvc = null;
        if (svc == null) return;
        try { svc.Dispose(); } catch { }
    }

    // ---------------- 相机 ----------------
    private FixtureCameraConfig ReadAnchorCameraConfig()
    {
        var cfg = new FixtureCameraConfig { FourCC = "MJPG" };
        if (_anchorProfile?.Camera != null)
        {
            cfg.Width = _anchorProfile.Camera.Width;
            cfg.Height = _anchorProfile.Camera.Height;
            cfg.Gain = _anchorProfile.Camera.Gain;
        }
        cfg.Index = Math.Max(0, _cboAnchorCamIndex.SelectedIndex);
        var res = _cboAnchorRes.SelectedItem as string ?? "";
        if (res.StartsWith("1920", StringComparison.Ordinal))
        {
            cfg.Width = 1920;
            cfg.Height = 1080;
        }
        else if (res.StartsWith("1280", StringComparison.Ordinal))
        {
            cfg.Width = 1280;
            cfg.Height = 720;
        }
        cfg.Exposure = _chkAnchorExposure.Checked ? (double)_numAnchorExposure.Value : null;
        return cfg;
    }

    private async Task<bool> ToggleAnchorCameraAsync()
    {
        if (_anchorCam != null)
        {
            CloseAnchorCamera();
            return false;
        }
        return await OpenAnchorCameraAsync();
    }

    private async Task<bool> EnsureAnchorCameraAsync()
    {
        if (_anchorCam != null) return true;
        return await OpenAnchorCameraAsync();
    }

    private async Task<bool> OpenAnchorCameraAsync()
    {
        if (_anchorCameraBusy) return false;
        _anchorCameraBusy = true;
        SetAnchorCamInfo("正在打开相机…");
        try
        {
            var cfg = ReadAnchorCameraConfig();
            var cam = new FixtureCamera();
            var error = await Task.Run(() =>
            {
                if (cam.Open(cfg, out var message)) return "";
                return string.IsNullOrWhiteSpace(message) ? "未知原因" : message;
            });
            if (error.Length > 0)
            {
                try { cam.Close(); } catch { }
                SetAnchorCamInfo("打开失败：" + error);
                return false;
            }

            _anchorCam = cam;
            _anchorShownFrame = -1;
            if (_anchorProfile != null)
            {
                cfg.Index = _anchorProfile.Camera.Index;
                _anchorProfile.Camera = cfg;
                AnchorStore.Save(_anchorProfile);
            }

            if (_anchorTimer == null)
            {
                _anchorTimer = new System.Windows.Forms.Timer { Interval = 66 };
                _anchorTimer.Tick += (_, __) => AnchorPreviewTick();
            }
            _anchorTimer.Start();
            if (_btnAnchorCam != null) _btnAnchorCam.Text = "关闭相机";
            SetAnchorCamInfo($"已连接：相机 {cfg.Index} · {cfg.Width}×{cfg.Height} · MJPG");
            return true;
        }
        catch (Exception ex)
        {
            SetAnchorCamInfo("打开失败：" + ex.Message);
            return false;
        }
        finally
        {
            _anchorCameraBusy = false;
        }
    }

    private void CloseAnchorCamera()
    {
        ExitAnchorEditMode();
        _anchorTimer?.Stop();
        var cam = _anchorCam;
        _anchorCam = null;
        if (cam != null)
        {
            try { cam.Close(); } catch { }
        }
        if (_btnAnchorCam != null) _btnAnchorCam.Text = "打开相机";
        SetAnchorCamInfo("相机已关闭");
    }

    private void AnchorPreviewTick()
    {
        if (_anchorCam == null || _anchorBusy) return;
        if (_anchorEditFrame != null || _zoneEditing) return;
        if (_anchorCalibView) return;
        if (_tabs == null || !ReferenceEquals(_tabs.SelectedTab, _pageAnchor)) return;
        if (!_anchorCam.TryGetLatest(out var frame, out var id)) return;
        try
        {
            if (id == _anchorShownFrame) return;
            _anchorShownFrame = id;
            _anchorRaw?.Dispose();
            _anchorRaw = frame.Clone();
            ShowAnchorFrame(frame, null);
        }
        finally
        {
            frame.Dispose();
        }
    }

    // ---------------- 拍基准 ----------------
    private async Task AnchorCalibrateAsync()
    {
        if (_anchorBusy) return;
        if (_anchorProfile == null)
        {
            SetAnchorCamInfo("还没有选机种。");
            return;
        }
        if (_anchorSvc == null || !_anchorSvc.IsLoaded)
        {
            SetAnchorCamInfo("模型还没加载：先点“复制当前模型”，或到训练页训练一个。");
            return;
        }
        if (!await EnsureAnchorCameraAsync()) return;

        var cam = _anchorCam;
        var svc = _anchorSvc;
        var profile = _anchorProfile;
        var model = _anchorModel;
        SetAnchorBusy(true, "正在拍基准…");
        Mat frame = null;
        long frameId = -1;
        try
        {
            frame = await Task.Run(() =>
            {
                var f = cam.WaitNewFrame(_anchorShownFrame, 1500, out var id);
                frameId = id;
                return f;
            });
            if (frame == null || frame.Empty())
            {
                SetAnchorCamInfo("1.5 秒内没有拿到新画面，检查相机连接。");
                return;
            }
            _anchorShownFrame = Math.Max(_anchorShownFrame, frameId);

            var result = await Task.Run(() => svc.Predict(frame));
            var anchors = PickAnchorItems(result, profile, out var note);
            var need = Math.Max(2, profile.Tolerance.MinAnchors);
            if (anchors.Count < need)
            {
                _lblAnchorVerdict.Text = "NG";
                _lblAnchorVerdict.ForeColor = Theme.Danger;
                _lblAnchorReason.Text = "拍基准失败：锚点不够";
                _lblAnchorReason.ForeColor = Theme.Danger;
                _lblAnchorDetail.Text = $"只检出 {anchors.Count} 个锚点，至少需要 {need} 个。{note}";
                return;
            }

            profile.Calib.Width = frame.Width;
            profile.Calib.Height = frame.Height;
            profile.Calib.At = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            profile.Calib.Anchors = anchors;
            AnchorStore.Save(profile);
            AnchorStore.SaveImage(model, AnchorStore.CalibFile, frame);

            using (var overlay = frame.Clone())
            {
                DrawAnchorBoxes(overlay, anchors.Select(a => (a.Cls, a.Ex, a.Ey, a.W, a.H)).ToList(), new CvScalar(0, 190, 60), true);
                OverlayText.Chip(overlay, 18, 10, "基准", OverlayText.ToColor(new CvScalar(0, 190, 60)), 15f);
                AnchorStore.SaveImage(model, AnchorStore.OverlayFile, overlay);
                // 拍完基准先停住画面，方便确认基准图；点【返回实时】或离开第三页再恢复预览。
                _anchorCalibView = true;
                ShowMatFrame(overlay);
            }

            _anchorJitter.Clear();
            _lblAnchorVerdict.Text = "已标定";
            _lblAnchorVerdict.ForeColor = Theme.Ok;
            _lblAnchorReason.Text = $"基准已保存：{anchors.Count} 个锚点";
            _lblAnchorReason.ForeColor = Theme.Ok;
            _lblAnchorDetail.Text = string.Join(" / ", anchors.Select(a => $"{a.Cls} @{a.Ex:0},{a.Ey:0}"))
                                    + (note.Length > 0 ? "；" + note : "");
            _lblAnchorCalibInfo.Text = $"基准：{profile.Calib.At} · {anchors.Count} 个锚点 · {profile.Calib.Width}×{profile.Calib.Height}";

            _lvAnchorHits.BeginUpdate();
            _lvAnchorHits.Items.Clear();
            foreach (var anchor in anchors)
            {
                var item = new ListViewItem(anchor.Cls);
                item.SubItems.Add($"{anchor.Ex:0},{anchor.Ey:0}");
                item.SubItems.Add("—");
                item.SubItems.Add("—");
                item.SubItems.Add($"{anchor.Conf:P0}");
                _lvAnchorHits.Items.Add(item);
            }
            _lvAnchorHits.EndUpdate();
        }
        catch (Exception ex)
        {
            SetAnchorCamInfo("拍基准出错：" + ex.Message);
        }
        finally
        {
            frame?.Dispose();
            SetAnchorBusy(false);
        }
    }

    // ---------------- 判定一次 ----------------
    private async Task AnchorJudgeOnceAsync()
    {
        if (_anchorBusy) return;
        if (_anchorProfile == null)
        {
            SetAnchorCamInfo("还没有选机种。");
            return;
        }
        if (!_anchorProfile.IsCalibrated && !_anchorProfile.HasZones)
        {
            SetAnchorCamInfo("还没有基准：先放一块确认到位的板点“拍基准”，或在“检测范围”里框一块特征。");
            return;
        }
        if ((_anchorSvc == null || !_anchorSvc.IsLoaded) && !_anchorProfile.HasZones)
        {
            SetAnchorCamInfo("模型还没加载：先点“复制当前模型”。");
            return;
        }
        if (!await EnsureAnchorCameraAsync()) return;

        ApplyAnchorToleranceToProfile();
        var cam = _anchorCam;
        var svc = _anchorSvc;
        var profile = _anchorProfile;
        var model = _anchorModel;
        SetAnchorBusy(true, "判定中…");
        Mat frame = null;
        long frameId = -1;
        try
        {
            frame = await Task.Run(() =>
            {
                var f = cam.WaitNewFrame(_anchorShownFrame, 1500, out var id);
                frameId = id;
                return f;
            });
            if (frame == null || frame.Empty())
            {
                SetAnchorCamInfo("1.5 秒内没有拿到新画面，检查相机连接。");
                return;
            }
            _anchorShownFrame = Math.Max(_anchorShownFrame, frameId);

            PredictionResult result = null;
            if (svc != null && svc.IsLoaded) result = await Task.Run(() => svc.Predict(frame));
            var verdict = JudgeAnchorFrame(frame, result);
            _anchorLastVerdict = verdict;

            if (verdict.Ok)
            {
                _anchorJitter.Add(verdict.MeanDx, verdict.MeanDy);
            }

            var shotName = $"{verdict.Time:yyyyMMdd-HHmmss}-{(verdict.Ok ? "OK" : "NG-" + verdict.ReasonCode)}.png";
            AnchorStore.SaveImage(model, Path.Combine(AnchorStore.ShotsDir, shotName), frame);
            AnchorHistory.Append(verdict, model, shotName);

            ShowAnchorVerdict(verdict, frame);
            ShowAnchorZones();
        }
        catch (Exception ex)
        {
            SetAnchorCamInfo("判定出错：" + ex.Message);
        }
        finally
        {
            frame?.Dispose();
            SetAnchorBusy(false);
        }
    }

    private void ShowAnchorVerdict(AnchorVerdict verdict, Mat frame)
    {
        if (verdict == null) return;
        _lblAnchorVerdict.Text = verdict.Ok ? "OK" : "NG";
        _lblAnchorVerdict.ForeColor = verdict.Ok ? Theme.Ok : Theme.Danger;
        _lblAnchorReason.Text = $"{verdict.ReasonName} · {verdict.ReasonCode}";
        _lblAnchorReason.ForeColor = verdict.Ok ? Theme.Ok : Theme.Danger;
        var detail = verdict.Message;
        if (_anchorZoneReport != null && _anchorZoneReport.Zones.Count > 0)
        {
            var lines = _anchorZoneReport.Zones
                .Select(r => r.Name + (r.Ok ? " ✓ " : " ✗ ") + r.ScoreText + (r.Ok ? "" : "（" + r.Code + "）"));
            var zoneLine = string.Join("  ", lines);
            detail = string.IsNullOrWhiteSpace(detail) ? zoneLine : zoneLine + "\n" + detail;
        }
        _lblAnchorDetail.Text = detail;
        _anchorTip?.SetToolTip(_lblAnchorReason, detail);

        var limit = _anchorProfile?.Tolerance?.ShiftPx ?? 12;
        _lvAnchorHits.BeginUpdate();
        _lvAnchorHits.Items.Clear();
        foreach (var hit in verdict.HitList)
        {
            var item = new ListViewItem(hit.ClsText);
            item.SubItems.Add($"{hit.Ex:0},{hit.Ey:0}");
            item.SubItems.Add($"{hit.Cx:0},{hit.Cy:0}");
            item.SubItems.Add($"{hit.Dx:+0;-0;0},{hit.Dy:+0;-0;0}");
            item.SubItems.Add($"{hit.Conf:P0}");
            item.ForeColor = hit.OffsetPx > limit ? Theme.Danger : Theme.Ink;
            _lvAnchorHits.Items.Add(item);
        }
        _lvAnchorHits.EndUpdate();

        AddAnchorLog(verdict);
        UpdateAnchorJitterLabel();
        if (frame != null) ShowAnchorFrame(frame, verdict);
    }

    private void AddAnchorLog(AnchorVerdict verdict)
    {
        if (_lvAnchorLog == null) return;
        var item = new ListViewItem(verdict.Time.ToString("HH:mm:ss"));
        item.SubItems.Add(verdict.Ok ? "OK" : "NG");
        item.SubItems.Add(verdict.ReasonCode);
        item.SubItems.Add($"{verdict.MeanDx:0.0},{verdict.MeanDy:0.0}");
        item.ForeColor = verdict.Ok ? Theme.Ok : Theme.Danger;
        _lvAnchorLog.Items.Insert(0, item);
        while (_lvAnchorLog.Items.Count > 200)
        {
            _lvAnchorLog.Items.RemoveAt(_lvAnchorLog.Items.Count - 1);
        }
    }

    private void UpdateAnchorJitterLabel()
    {
        if (_lblAnchorJitter == null) return;
        if (_anchorJitter.TrySuggest(out _, out var suggested, out var text))
        {
            var current = _anchorProfile?.Tolerance?.ShiftPx ?? 0;
            _lblAnchorJitter.Text = text + (current > suggested * 1.5 ? "（当前上限偏松）" : "");
            _lblAnchorJitter.ForeColor = current > suggested * 1.5 ? Theme.Warn : Theme.Muted;
        }
        else
        {
            _lblAnchorJitter.Text = $"连拍样本 {_anchorJitter.Count}/5，多判几次可估算抖动。";
            _lblAnchorJitter.ForeColor = Theme.Muted;
        }
    }

    // ---------------- 画面与叠加 ----------------
    private void ShowAnchorFrame(Mat frame, AnchorVerdict verdict)
    {
        if (frame == null || frame.Empty() || _picAnchor == null) return;
        using var view = BuildAnchorView(frame, verdict);
        ShowMatFrame(view);
    }

    private void ShowMatFrame(Mat frame)
    {
        if (frame == null || frame.Empty() || _picAnchor == null) return;
        var bmp = MatToBitmap(frame);
        var old = _picAnchor.Image;
        _picAnchor.SetFrame(bmp);
        old?.Dispose();
    }

    /// <summary>
    /// 把一帧画成带叠加层的显示图。showZones=false 用于样张照片编辑：照片上只保留画布自己画的草稿框。
    /// zone 判定结果只在带 verdict 的判定帧上渲染，避免上一次 NG 结果残留在实时预览或别的帧上。
    /// </summary>
    private Mat BuildAnchorView(Mat frame, AnchorVerdict verdict, bool showZones = true)
    {
        var canvas = frame.Clone();
        if (!_anchorOverlayOn) return canvas;
        if (showZones) DrawAnchorZones(canvas, verdict != null);
        if (verdict == null) return canvas;

        var color = verdict.Ok ? new CvScalar(0, 190, 60) : new CvScalar(40, 40, 220);
        DrawAnchorBoxes(canvas, verdict.HitList.Select(h => (h.ClsText, h.Ex, h.Ey, h.W, h.H)).ToList(), color, false);
        foreach (var hit in verdict.HitList)
        {
            var target = new CvPoint((int)Math.Round(hit.Ex), (int)Math.Round(hit.Ey));
            var center = new CvPoint((int)Math.Round(hit.Cx), (int)Math.Round(hit.Cy));
            Cv2.Circle(canvas, target, 8, new CvScalar(0, 200, 255), 2);
            Cv2.Line(canvas, target, center, new CvScalar(0, 200, 255), 1, LineTypes.AntiAlias);
        }
        var title = verdict.Ok ? "OK" : "NG " + verdict.ReasonCode;
        Cv2.PutText(canvas, title, new CvPoint(18, 48), Hershey.HersheyDuplex, 1.3, color, 3, LineTypes.AntiAlias);
        return canvas;
    }

    private static void DrawAnchorBoxes(Mat canvas, IReadOnlyList<(string Cls, double Ex, double Ey, double W, double H)> anchors, CvScalar color, bool marker)
    {
        foreach (var anchor in anchors)
        {
            var x = (int)Math.Round(anchor.Ex - anchor.W / 2);
            var y = (int)Math.Round(anchor.Ey - anchor.H / 2);
            var w = Math.Max(2, (int)Math.Round(anchor.W));
            var h = Math.Max(2, (int)Math.Round(anchor.H));
            Cv2.Rectangle(canvas, new CvRect(x, y, w, h), color, 2);
            var clsText = anchor.Cls ?? "?";
            if (OverlayText.IsAscii(clsText))
                Cv2.PutText(canvas, clsText, new CvPoint(x, Math.Max(16, y - 6)), Hershey.HersheySimplex, 0.55, color, 1, LineTypes.AntiAlias);
            else
                OverlayText.Chip(canvas, x, Math.Max(0, y - 24), clsText, OverlayText.ToColor(color), 12f);
            if (!marker) continue;
            var cx = (int)Math.Round(anchor.Ex);
            var cy = (int)Math.Round(anchor.Ey);
            Cv2.Line(canvas, new CvPoint(cx - 9, cy), new CvPoint(cx + 9, cy), color, 2, LineTypes.AntiAlias);
            Cv2.Line(canvas, new CvPoint(cx, cy - 9), new CvPoint(cx, cy + 9), color, 2, LineTypes.AntiAlias);
        }
    }

    // ---------------- 参数与文案 ----------------
    private void ApplyAnchorToUi()
    {
        var profile = _anchorProfile;
        if (profile == null) return;

        SetAnchorNum(_numAnchorShift, (decimal)profile.Tolerance.ShiftPx);
        SetAnchorNum(_numAnchorRot, (decimal)profile.Tolerance.RotDeg);
        SetAnchorNum(_numAnchorScale, (decimal)profile.Tolerance.ScaleTol);
        SetAnchorNum(_numAnchorRms, (decimal)profile.Tolerance.RmsPx);
        SetAnchorNum(_numAnchorConf, (decimal)profile.Infer.Conf);
        SetAnchorNum(_numAnchorMin, profile.Tolerance.MinAnchors);

        _lblAnchorCalibInfo.Text = profile.IsCalibrated
            ? $"基准：{profile.Calib.At} · {profile.Calib.Anchors.Count} 个锚点 · {profile.Calib.Width}×{profile.Calib.Height}"
            : "基准：还没有拍";

        if (_lvAnchorHits == null) return;
        _lvAnchorHits.Items.Clear();
        if (!profile.IsCalibrated) return;
        foreach (var anchor in profile.Calib.Anchors)
        {
            var item = new ListViewItem(anchor.Cls);
            item.SubItems.Add($"{anchor.Ex:0},{anchor.Ey:0}");
            item.SubItems.Add("—");
            item.SubItems.Add("—");
            item.SubItems.Add($"{anchor.Conf:P0}");
            _lvAnchorHits.Items.Add(item);
        }
    }

    private void ApplyAnchorToleranceToProfile()
    {
        var profile = _anchorProfile;
        if (profile == null) return;
        if (_numAnchorShift != null) profile.Tolerance.ShiftPx = (double)_numAnchorShift.Value;
        if (_numAnchorRot != null) profile.Tolerance.RotDeg = (double)_numAnchorRot.Value;
        if (_numAnchorScale != null) profile.Tolerance.ScaleTol = (double)_numAnchorScale.Value;
        if (_numAnchorRms != null) profile.Tolerance.RmsPx = (double)_numAnchorRms.Value;
        if (_numAnchorConf != null) profile.Infer.Conf = (double)_numAnchorConf.Value;
        if (_numAnchorMin != null) profile.Tolerance.MinAnchors = (int)_numAnchorMin.Value;
    }

    private void SaveAnchorTolerance()
    {
        if (_anchorProfile == null) return;
        ApplyAnchorToleranceToProfile();
        AnchorStore.Save(_anchorProfile);
        SetAnchorCamInfo("容差已保存。");
    }

    private static void SetAnchorNum(NumericUpDown num, decimal value)
    {
        if (num == null) return;
        if (value < num.Minimum) value = num.Minimum;
        if (value > num.Maximum) value = num.Maximum;
        num.Value = value;
    }

    private void SetAnchorBusy(bool busy, string text = null)
    {
        _anchorBusy = busy;
        if (_btnAnchorJudge != null) _btnAnchorJudge.Enabled = !busy;
        if (_btnAnchorCalib != null) _btnAnchorCalib.Enabled = !busy;
        if (busy && !string.IsNullOrEmpty(text)) SetAnchorCamInfo(text);
    }

    private void SetAnchorCamInfo(string text)
    {
        if (_lblAnchorCamInfo == null) return;
        _lblAnchorCamInfo.Text = text;
        _lblAnchorCamInfo.ForeColor = text.Contains("失败") || text.Contains("出错") ? Theme.Danger : Theme.Muted;
    }

    /// <summary>从一帧结果里挑锚点：只认 anchor* 类别，每类取最高分。</summary>
    private List<AnchorItem> PickAnchorItems(PredictionResult result, AnchorProfile profile, out string note)
    {
        note = "";
        var anchors = new List<AnchorItem>();
        var items = result?.Items ?? (IReadOnlyList<DetectionItem>)Array.Empty<DetectionItem>();
        var conf = Math.Clamp(profile?.Infer?.Conf ?? 0.35, 0.01, 0.99);
        var names = AnchorStore.AnchorClasses(_anchorSvc?.Info?.ClassNames ?? Array.Empty<string>());

        var best = new Dictionary<string, DetectionItem>(StringComparer.OrdinalIgnoreCase);
        foreach (var item in items)
        {
            if (item == null || item.Confidence < conf) continue;
            var cls = item.RawCategory ?? "";
            if (names.Count > 0 && !names.Contains(cls, StringComparer.OrdinalIgnoreCase)) continue;
            if (!best.TryGetValue(cls, out var current) || item.Confidence > current.Confidence) best[cls] = item;
        }

        foreach (var detect in best.Values.OrderBy(v => v.RawCategory, StringComparer.Ordinal))
        {
            anchors.Add(new AnchorItem
            {
                Cls = detect.RawCategory,
                Ex = detect.X + detect.W / 2.0,
                Ey = detect.Y + detect.H / 2.0,
                W = detect.W,
                H = detect.H,
                Conf = detect.Confidence,
            });
        }

        if (names.Count > 0)
        {
            var missed = names.Where(n => !best.ContainsKey(n)).ToList();
            if (missed.Count > 0) note = "本帧没检出：" + string.Join("、", missed);
        }
        return anchors;
    }
}
