using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DeploySharp.Engine;

namespace YoloFool;

/// <summary>
/// 傻瓜版主界面：所有布局都用 TableLayoutPanel / Dock / AutoSize 生成，
/// 不再依赖 Designer 的绝对坐标，窗口缩放、不同 DPI 下都不会互相遮挡。
/// </summary>
public sealed partial class MainForm : Form
{
    private readonly AppSettings _settings;
    private readonly LabelCatalog _labels;
    private readonly DatasetStore _store;
    private readonly TrainingService _training = new();

    private InferenceService _svc;
    private Bitmap _recBitmap;
    private Bitmap _learnBitmap;
    private string _recPath = "";
    /// <summary>画布上是拍照定格的内存照片（不落盘，_recPath 为空）。</summary>
    private bool _recInMemory;
    private string _learnPath = "";
    private IReadOnlyList<DetectionItem> _detections = Array.Empty<DetectionItem>();
    private IReadOnlyList<ProviderProbe> _probes;
    private bool _advExpanded;
    private bool _busy;
    private bool _trainBusy;
    private bool _syncing;
    private bool _splitInit;
    private bool _learnSplitInit;

    private Label _lblModelStatus;
    private Label _lblModelSub;
    private Button _btnSelfCheck;

    private TabControl _tabs;
    private TabPage _pageRec;
    private TabPage _pageLearn;
    private TabPage _pageTools;

    private TextBox _tbRecImage;
    private Button _btnSelectImage;
    private Button _btnRun;
    private Button _btnClear;
    private Button _btnToggleAdv;
    private Panel _pnlAdv;
    private TextBox _tbAdvModel;
    private Button _btnAdvModel;
    private Button _btnVerifySave;
    private Button _btnResetAdv;
    private ComboBox _cboKind;
    private ComboBox _cboDevice;
    private ComboBox _cboProvider;
    private NumericUpDown _numConf;
    private NumericUpDown _numNms;
    private Label _lblAdvInfo;
    private Label _lblAdvProbe;
    private SplitContainer _splitRec;
    private ImageCanvas _picRec;
    private DataGridView _grid;
    private Label _lblResultStats;

    private TextBox _tbLearnImage;
    private Button _btnOpenLearn;
    private Button _btnOpenFolder;
    private Button _btnClearLearn;
    private Button _btnPrevImage;
    private Button _btnNextImage;
    private ListView _lvImages;
    private Label _lblImageListInfo;
    private readonly List<string> _imageFiles = new();
    private bool _syncingImages;
    private SplitContainer _splitLearn;
    private ImageCanvas _picLearn;
    private ListView _lvBoxes;
    private Button _btnDelBox;
    private Button _btnRenameBox;
    private Button _btnReexport;
    private Label _lblAnnoInfo;
    private Label _lblDatasetStats;
    private Button _lnkOpenDataset;
    private NumericUpDown _numEpochs;
    private NumericUpDown _numBatch;
    private ComboBox _cboImgSize;
    private Button _btnInitEnv;
    private Button _btnTrain;
    private Button _btnCancelTrain;
    private ProgressBar _pbTrain;
    private Label _lblTrainStage;
    private TextBox _txtTrainLog;

    private Label _lblStatus;
    private Label _lblStatusRight;

    public MainForm()
    {
        _settings = AppSettings.Load();
        _labels = LabelCatalog.Load();
        _store = DatasetStore.Load(_labels);
        AppPaths.EnsureDirectories();
        _advExpanded = _settings.AdvancedExpanded;

        BuildUi();
        WireEvents();

        Load += MainForm_Load;
        Shown += (_, _) => InitSplitters();
        FormClosing += OnFormClosing;
    }

    // ---------------------------------------------------------------- layout

    private void BuildUi()
    {
        Text = "YoloFool · 傻瓜式视觉识别";
        Font = Theme.Body;
        BackColor = Theme.Bg;
        DoubleBuffered = true;
        AutoScaleMode = AutoScaleMode.Dpi;
        AutoScaleDimensions = new SizeF(96F, 96F);
        MinimumSize = new Size(1000, 620);

        var wa = Screen.PrimaryScreen == null ? new Rectangle(0, 0, 1366, 768) : Screen.PrimaryScreen.WorkingArea;
        Size = new Size(Math.Min(1320, Math.Max(1000, wa.Width - 40)), Math.Min(880, Math.Max(620, wa.Height - 40)));
        StartPosition = FormStartPosition.CenterScreen;

        var root = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 3,
            BackColor = Theme.Bg,
            Margin = Padding.Empty,
            Padding = Padding.Empty,
        };
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 30f));

        root.Controls.Add(BuildHeader(), 0, 0);
        _tabs = BuildTabs();
        _tabs.Margin = new Padding(10, 8, 10, 6);
        root.Controls.Add(_tabs, 0, 1);
        root.Controls.Add(BuildStatusBar(), 0, 2);
        Controls.Add(root);
    }

    private Control BuildHeader()
    {
        var header = new TableLayoutPanel
        {
            Dock = DockStyle.Top,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            ColumnCount = 3,
            RowCount = 1,
            BackColor = Color.White,
            Padding = new Padding(20, 10, 20, 10),
            Margin = Padding.Empty,
        };
        header.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
        header.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        header.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        header.Paint += (_, e) =>
        {
            using var pen = new Pen(Theme.Border);
            e.Graphics.DrawLine(pen, 0, header.Height - 1, header.Width, header.Height - 1);
        };

        var titleStack = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
            ColumnCount = 1,
            RowCount = 2,
            Margin = Padding.Empty,
        };
        titleStack.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        titleStack.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        titleStack.Controls.Add(new Label
        {
            Text = "YOLO 傻瓜识别",
            Font = Theme.Title,
            ForeColor = Theme.Ink,
            AutoSize = true,
            Margin = new Padding(0, 0, 0, 2),
        }, 0, 0);
        titleStack.Controls.Add(Theme.Hint("默认值按本机自动验证；需要时再展开“详细设置”。"), 0, 1);

        var modelStack = new TableLayoutPanel
        {
            AutoSize = true,
            Anchor = AnchorStyles.Top | AnchorStyles.Right,
            ColumnCount = 1,
            RowCount = 2,
            Margin = new Padding(0, 2, 12, 0),
        };
        _lblModelStatus = new Label
        {
            Text = "● 正在自检…",
            Font = Theme.BodyBold,
            ForeColor = Theme.Warn,
            AutoSize = true,
            Anchor = AnchorStyles.Right,
            TextAlign = ContentAlignment.MiddleRight,
            Margin = Padding.Empty,
        };
        _lblModelSub = new Label
        {
            Text = "",
            Font = Theme.Small,
            ForeColor = Theme.Muted,
            AutoSize = false,
            Size = new Size(380, 18),
            TextAlign = ContentAlignment.MiddleRight,
            AutoEllipsis = true,
            Anchor = AnchorStyles.Right,
            Margin = new Padding(0, 1, 0, 0),
        };
        modelStack.Controls.Add(_lblModelStatus, 0, 0);
        modelStack.Controls.Add(_lblModelSub, 0, 1);

        _btnSelfCheck = Theme.Ghost("重新自检");
        _btnSelfCheck.Name = "btnSelfCheck";
        _btnSelfCheck.Anchor = AnchorStyles.Right;
        _btnSelfCheck.Margin = new Padding(0, 5, 0, 0);

        header.Controls.Add(titleStack, 0, 0);
        header.Controls.Add(modelStack, 1, 0);
        header.Controls.Add(_btnSelfCheck, 2, 0);
        return header;
    }

    private TabControl BuildTabs()
    {
        var tabs = new TabControl
        {
            Dock = DockStyle.Fill,
            Name = "tabMain",
            Font = Theme.Body,
            Padding = new Point(14, 6),
        };
        _pageRec = new TabPage("识别测试")
        {
            Name = "tabRecognize",
            BackColor = Theme.Bg,
            Padding = new Padding(12, 10, 12, 10),
        };
        _pageLearn = new TabPage("学习训练")
        {
            Name = "tabLearn",
            BackColor = Theme.Bg,
            Padding = new Padding(12, 10, 12, 10),
        };
        _pageTools = new TabPage("工具 · 我的模型")
        {
            Name = "tabTools",
            BackColor = Theme.Bg,
            Padding = new Padding(12, 10, 12, 10),
        };
        _pageRec.Controls.Add(BuildCapturePage());
        _pageLearn.Controls.Add(BuildLearnPage());
        _pageTools.Controls.Add(BuildToolsPage());
        tabs.TabPages.Add(_pageRec);
        tabs.TabPages.Add(_pageLearn);
        tabs.TabPages.Add(_pageTools);
        return tabs;
    }

    private Control BuildRecognizePage()
    {
        var root = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 3,
            BackColor = Theme.Bg,
        };
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
        root.Controls.Add(BuildRecognizeToolbar(), 0, 0);

        _pnlAdv = BuildAdvancedPanel();
        _pnlAdv.Visible = _advExpanded;
        root.Controls.Add(_pnlAdv, 0, 1);
        root.Controls.Add(BuildRecognizeBody(), 0, 2);
        return root;
    }

    private Control BuildRecognizeToolbar()
    {
        var bar = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
            ColumnCount = 6,
            RowCount = 1,
            Margin = new Padding(0, 0, 0, 8),
        };
        bar.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        bar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
        for (int i = 0; i < 4; i++) bar.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

        bar.Controls.Add(new Label
        {
            Text = "图片",
            Font = Theme.BodyBold,
            ForeColor = Theme.Ink,
            AutoSize = true,
            Anchor = AnchorStyles.Left,
            Margin = new Padding(0, 0, 8, 0),
        }, 0, 0);

        _tbRecImage = new TextBox
        {
            Name = "tbRecImage",
            Dock = DockStyle.Fill,
            ReadOnly = true,
            Font = Theme.Body,
            BackColor = Color.White,
            Margin = new Padding(0, 6, 8, 6),
        };
        bar.Controls.Add(_tbRecImage, 1, 0);

        _btnSelectImage = Theme.Ghost("选择图片…");
        _btnSelectImage.Name = "btnSelectImage";
        Stretch(_btnSelectImage, 96, 34);
        _btnSelectImage.Anchor = AnchorStyles.Left;
        _btnSelectImage.Margin = new Padding(0, 4, 8, 4);
        bar.Controls.Add(_btnSelectImage, 2, 0);

        _btnRun = Theme.Primary("开始识别");
        _btnRun.Name = "btnRun";
        _btnRun.AutoSize = false;
        _btnRun.Size = new Size(126, 36);
        _btnRun.MinimumSize = new Size(126, 36);
        _btnRun.Anchor = AnchorStyles.Left;
        _btnRun.Margin = new Padding(0, 3, 8, 4);
        bar.Controls.Add(_btnRun, 3, 0);

        _btnClear = Theme.Ghost("清空");
        _btnClear.Name = "btnClear";
        Stretch(_btnClear, 62, 34);
        _btnClear.Anchor = AnchorStyles.Left;
        _btnClear.Margin = new Padding(0, 4, 8, 4);
        bar.Controls.Add(_btnClear, 4, 0);

        _btnToggleAdv = Theme.Ghost("▸ 详细设置");
        _btnToggleAdv.Name = "btnToggleAdv";
        _btnToggleAdv.Text = _advExpanded ? "▾ 收起设置" : "▸ 详细设置";
        Stretch(_btnToggleAdv, 112, 34);
        _btnToggleAdv.Anchor = AnchorStyles.Left;
        _btnToggleAdv.Margin = new Padding(0, 4, 0, 4);
        bar.Controls.Add(_btnToggleAdv, 5, 0);
        return bar;
    }

    private Panel BuildAdvancedPanel()
    {
        var card = Theme.CardPanel();
        card.Name = "pnlAdv";
        card.Margin = new Padding(0, 0, 0, 8);

        var g = new TableLayoutPanel
        {
            Dock = DockStyle.Top,
            AutoSize = true,
            ColumnCount = 4,
            RowCount = 6,
            BackColor = Color.White,
            Margin = Padding.Empty,
        };
        g.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 82f));
        g.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
        g.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 104f));
        g.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
        for (int i = 0; i < 6; i++) g.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        // 模型文件
        g.Controls.Add(FieldLabel("模型文件"), 0, 0);
        var pick = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
            ColumnCount = 2,
            RowCount = 1,
            Margin = new Padding(0, 3, 0, 3),
        };
        pick.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
        pick.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        _tbAdvModel = new TextBox
        {
            Dock = DockStyle.Fill,
            Font = Theme.Body,
            Margin = new Padding(0, 2, 6, 2),
        };
        _btnAdvModel = Theme.Ghost("选择…");
        Stretch(_btnAdvModel, 84, 30);
        _btnAdvModel.Margin = Padding.Empty;
        pick.Controls.Add(_tbAdvModel, 0, 0);
        pick.Controls.Add(_btnAdvModel, 1, 0);
        g.Controls.Add(pick, 1, 0);
        g.SetColumnSpan(pick, 3);

        // 模型类型 / 设备
        g.Controls.Add(FieldLabel("模型类型"), 0, 1);
        _cboKind = MakeCombo();
        _cboKind.Items.AddRange(new object[] { "YOLOv8 检测（推荐）", "YOLOv11 检测", "YOLOv8 分割" });
        _cboKind.SelectedIndex = 0;
        g.Controls.Add(_cboKind, 1, 1);
        g.Controls.Add(FieldLabel("推理设备"), 2, 1);
        _cboDevice = MakeCombo();
        _cboDevice.Items.AddRange(new object[] { "CPU", "GPU 0" });
        _cboDevice.SelectedIndex = 0;
        g.Controls.Add(_cboDevice, 3, 1);

        // 加速后端 / 阈值
        g.Controls.Add(FieldLabel("加速后端"), 0, 2);
        _cboProvider = MakeCombo();
        _cboProvider.Items.AddRange(new object[] { "CPU（本机已验证）", "DirectML（GPU）" });
        _cboProvider.SelectedIndex = 0;
        g.Controls.Add(_cboProvider, 1, 2);
        g.Controls.Add(FieldLabel("置信度 / NMS"), 2, 2);
        var thr = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoSize = false,
            Height = 30,
            WrapContents = false,
            Margin = new Padding(0, 2, 0, 2),
        };
        _numConf = MakeNum(0.5m);
        _numNms = MakeNum(0.5m);
        thr.Controls.Add(_numConf);
        thr.Controls.Add(new Label
        {
            Text = "NMS",
            Font = Theme.Small,
            ForeColor = Theme.Muted,
            AutoSize = true,
            Margin = new Padding(8, 8, 6, 0),
        });
        thr.Controls.Add(_numNms);
        g.Controls.Add(thr, 3, 2);

        _lblAdvInfo = new Label
        {
            Text = "当前：未加载模型",
            Font = Theme.Small,
            ForeColor = Theme.Muted,
            AutoSize = true,
            Margin = new Padding(0, 6, 0, 0),
        };
        g.Controls.Add(_lblAdvInfo, 0, 3);
        g.SetColumnSpan(_lblAdvInfo, 4);

        // 说明 + 按钮
        var footer = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
            ColumnCount = 2,
            RowCount = 1,
            Margin = new Padding(0, 6, 0, 0),
        };
        footer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
        footer.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        footer.Controls.Add(new Label
        {
            Text = "改动会先验证：通过后才保存并切换，失败则保留当前模型。",
            Font = Theme.Small,
            ForeColor = Theme.Muted,
            AutoSize = false,
            Dock = DockStyle.Fill,
            Height = 36,
            TextAlign = ContentAlignment.MiddleLeft,
        }, 0, 0);
        var footerButtons = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
            WrapContents = false,
            FlowDirection = FlowDirection.RightToLeft,
            Margin = Padding.Empty,
        };
        _btnVerifySave = Theme.Primary("验证并保存");
        Stretch(_btnVerifySave, 116, 36);
        _btnVerifySave.Margin = new Padding(8, 0, 0, 0);
        _btnResetAdv = Theme.Ghost("恢复默认");
        Stretch(_btnResetAdv, 94, 36);
        footerButtons.Controls.Add(_btnVerifySave);
        footerButtons.Controls.Add(_btnResetAdv);
        footer.Controls.Add(footerButtons, 1, 0);
        g.Controls.Add(footer, 0, 4);
        g.SetColumnSpan(footer, 4);

        _lblAdvProbe = new Label
        {
            Text = "",
            Font = Theme.Small,
            ForeColor = Theme.Muted,
            AutoSize = true,
            Margin = new Padding(0, 4, 0, 0),
        };
        g.Controls.Add(_lblAdvProbe, 0, 5);
        g.SetColumnSpan(_lblAdvProbe, 4);

        card.Controls.Add(g);
        return card;
    }

    private Control BuildRecognizeBody()
    {
        var body = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 1,
            BackColor = Theme.Bg,
        };
        body.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
        body.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
        _splitRec = new SplitContainer
        {
            Dock = DockStyle.Fill,
            Orientation = Orientation.Vertical,
            SplitterWidth = 8,
            // 先给一个足够大的初始宽度：WinForms 在构造时若 Panel2MinSize 超过当前 Width，
            // set_Panel2MinSize 会立刻重算 SplitterDistance 并抛 InvalidOperationException。
            Size = new Size(1000, 600),
            Panel1MinSize = 300,
            Panel2MinSize = 340,
            FixedPanel = FixedPanel.Panel2,
            BackColor = Theme.Border,
            Margin = Padding.Empty,
        };

        var left = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(8) };
        _picRec = new ImageCanvas
        {
            Name = "picRec",
            Dock = DockStyle.Fill,
            Editable = false,
            EmptyText = "① 选择图片  →  ② 点“开始识别”",
        };
        left.Controls.Add(_picRec);
        _splitRec.Panel1.Controls.Add(left);
        _splitRec.Panel2.Controls.Add(BuildResultPanel());
        body.Controls.Add(_splitRec, 0, 0);
        return body;
    }

    private Control BuildResultPanel()
    {
        var card = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White,
            Padding = new Padding(12, 10, 10, 10),
        };
        var t = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 4,
            BackColor = Color.White,
        };
        t.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        t.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        t.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
        t.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        t.Controls.Add(new Label
        {
            Text = "识别结果",
            Font = Theme.Section,
            ForeColor = Theme.Ink,
            AutoSize = true,
            Margin = new Padding(0, 0, 0, 2),
        }, 0, 0);

        _lblResultStats = new Label
        {
            Name = "lblResultStats",
            Text = "尚未识别。",
            Font = Theme.Small,
            ForeColor = Theme.Muted,
            AutoSize = true,
            Margin = new Padding(0, 0, 0, 6),
        };
        t.Controls.Add(_lblResultStats, 0, 1);

        _grid = new DataGridView
        {
            Name = "grid",
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
        Theme.StyleGrid(_grid);
        _grid.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "colName",
            HeaderText = "目标名称",
            FillWeight = 26,
            MinimumWidth = 88,
            SortMode = DataGridViewColumnSortMode.NotSortable,
        });
        _grid.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "colConf",
            HeaderText = "匹配度",
            FillWeight = 14,
            MinimumWidth = 68,
            SortMode = DataGridViewColumnSortMode.NotSortable,
        });
        _grid.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "colPos",
            HeaderText = "相对位置",
            FillWeight = 38,
            MinimumWidth = 118,
            SortMode = DataGridViewColumnSortMode.NotSortable,
        });
        _grid.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "colCenter",
            HeaderText = "中心坐标",
            FillWeight = 22,
            MinimumWidth = 96,
            SortMode = DataGridViewColumnSortMode.NotSortable,
        });
        t.Controls.Add(_grid, 0, 2);

        t.Controls.Add(new Label
        {
            Text = "点表格行可在图上高亮；位置为框中心相对原图的百分比。",
            Font = Theme.Small,
            ForeColor = Theme.Muted,
            AutoSize = false,
            Dock = DockStyle.Fill,
            Height = 30,
            TextAlign = ContentAlignment.MiddleLeft,
            Margin = new Padding(0, 4, 0, 0),
        }, 0, 3);

        card.Controls.Add(t);
        return card;
    }

    private Control BuildLearnPage()
    {
        var root = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 3,
            BackColor = Theme.Bg,
        };
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
        // 训练区高度固定，保证参数、按钮和日志在任何窗口高度下都完整可见。
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 216f));
        root.Controls.Add(BuildLearnToolbar(), 0, 0);

        _splitLearn = new SplitContainer
        {
            Dock = DockStyle.Fill,
            Orientation = Orientation.Vertical,
            SplitterWidth = 8,
            Size = new Size(1000, 600),
            Panel1MinSize = 300,
            Panel2MinSize = 380,
            FixedPanel = FixedPanel.Panel2,
            BackColor = Theme.Border,
            Margin = Padding.Empty,
        };
        var left = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(8) };
        _picLearn = new ImageCanvas
        {
            Name = "picLearn",
            Dock = DockStyle.Fill,
            Editable = true,
            EmptyText = "① 打开图片\r\n② 在图上拖动框选目标",
        };
        _picLearn.BoxLabel = b => string.IsNullOrEmpty(b.Cn) ? b.En : b.Cn;
        left.Controls.Add(_picLearn);
        _splitLearn.Panel1.Controls.Add(left);
        _splitLearn.Panel2.Controls.Add(BuildLearnSidebar());
        root.Controls.Add(_splitLearn, 0, 1);
        root.Controls.Add(BuildTrainingCard(), 0, 2);
        return root;
    }

    private Control BuildLearnToolbar()
    {
        var bar = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
            ColumnCount = 5,
            RowCount = 2,
            Margin = new Padding(0, 0, 0, 8),
        };
        bar.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        bar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
        bar.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        bar.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        bar.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

        bar.Controls.Add(new Label
        {
            Text = "图片",
            Font = Theme.BodyBold,
            ForeColor = Theme.Ink,
            AutoSize = true,
            Anchor = AnchorStyles.Left,
            Margin = new Padding(0, 0, 8, 0),
        }, 0, 0);

        _tbLearnImage = new TextBox
        {
            Name = "tbLearnImage",
            Dock = DockStyle.Fill,
            ReadOnly = true,
            Font = Theme.Body,
            BackColor = Color.White,
            Margin = new Padding(0, 6, 8, 6),
        };
        bar.Controls.Add(_tbLearnImage, 1, 0);

        _btnOpenLearn = Theme.Ghost("打开图片…");
        _btnOpenLearn.Name = "btnOpenLearnImage";
        Stretch(_btnOpenLearn, 96, 34);
        _btnOpenLearn.Anchor = AnchorStyles.Left;
        _btnOpenLearn.Margin = new Padding(0, 4, 8, 4);
        bar.Controls.Add(_btnOpenLearn, 2, 0);

        _btnOpenFolder = Theme.Ghost("打开文件夹…");
        _btnOpenFolder.Name = "btnOpenLearnFolder";
        Stretch(_btnOpenFolder, 116, 34);
        _btnOpenFolder.Anchor = AnchorStyles.Left;
        _btnOpenFolder.Margin = new Padding(0, 4, 8, 4);
        bar.Controls.Add(_btnOpenFolder, 3, 0);

        _btnClearLearn = Theme.Ghost("清空");
        _btnClearLearn.Name = "btnClearLearn";
        Stretch(_btnClearLearn, 62, 34);
        _btnClearLearn.Anchor = AnchorStyles.Left;
        _btnClearLearn.Margin = new Padding(0, 4, 0, 4);
        bar.Controls.Add(_btnClearLearn, 4, 0);

        var hint = Theme.Hint("① 打开图片  →  ② 拖动框选  →  ③ 拖边角调大小、框内拖动平移、拖圆点旋转（Shift 吸附 15°）→  ④ 回车 / 右键确认并命名；重新框选或 Esc 取消。已保存的框：单击选中后同样能拖手柄改大小/旋转，或点框内拖动。标注立即导出到 mydataset。");
        bar.Controls.Add(hint, 0, 1);
        bar.SetColumnSpan(hint, 5);
        return bar;
    }

    private Control BuildLearnSidebar()
    {
        var scroll = new Panel
        {
            Dock = DockStyle.Fill,
            AutoScroll = true,
            BackColor = Theme.Bg,
            Padding = new Padding(8, 0, 0, 0),
        };
        var stack = new TableLayoutPanel
        {
            Dock = DockStyle.Top,
            AutoSize = true,
            ColumnCount = 1,
            RowCount = 0,
            BackColor = Theme.Bg,
            Margin = Padding.Empty,
        };
        stack.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));

        // 图片列表（批量标注）
        var (c0, b0) = MakeCard("图片列表");
        _lvImages = new ListView
        {
            Name = "lvImages",
            Dock = DockStyle.Top,
            Height = 168,
            View = View.Details,
            FullRowSelect = true,
            MultiSelect = false,
            HideSelection = false,
            HeaderStyle = ColumnHeaderStyle.Nonclickable,
            BorderStyle = BorderStyle.FixedSingle,
            Font = Theme.Body,
            Margin = new Padding(0, 0, 0, 6),
        };
        _lvImages.Columns.Add("图片", 186);
        _lvImages.Columns.Add("标注", 54, HorizontalAlignment.Right);
        _lvImages.Columns.Add("状态", 76);
        AddRow(b0, _lvImages);

        var flowImg = new FlowLayoutPanel
        {
            Dock = DockStyle.Top,
            AutoSize = true,
            WrapContents = false,
            Margin = new Padding(0, 0, 0, 4),
        };
        _btnPrevImage = Theme.Ghost("上一张");
        _btnPrevImage.Name = "btnPrevImage";
        Stretch(_btnPrevImage, 74, 32);
        _btnNextImage = Theme.Ghost("下一张");
        _btnNextImage.Name = "btnNextImage";
        Stretch(_btnNextImage, 74, 32);
        flowImg.Controls.Add(_btnPrevImage);
        flowImg.Controls.Add(_btnNextImage);
        AddRow(b0, flowImg);

        // 半自动标注：先让模型猜一遍，再逐张复核。自动框确认前不进训练集。
        var flowAuto = new FlowLayoutPanel
        {
            Dock = DockStyle.Top,
            AutoSize = true,
            WrapContents = false,
            Margin = new Padding(0, 0, 0, 4),
        };
        _btnAutoLabel = Theme.Success("⚡ 自动预标注");
        _btnAutoLabel.Name = "btnAutoLabel";
        Stretch(_btnAutoLabel, 116, 32);
        _btnReview = Theme.Ghost("复核");
        _btnReview.Name = "btnReview";
        Stretch(_btnReview, 58, 32);
        _btnClearAuto = Theme.Ghost("清空自动框");
        _btnClearAuto.Name = "btnClearAuto";
        Stretch(_btnClearAuto, 92, 32);
        flowAuto.Controls.Add(_btnAutoLabel);
        flowAuto.Controls.Add(_btnReview);
        flowAuto.Controls.Add(_btnClearAuto);
        AddRow(b0, flowAuto);

        _lblImageListInfo = Theme.Hint("点“打开文件夹”一次载入整个文件夹；选中列表即可切换图片。");
        AddRow(b0, _lblImageListInfo);

        _lblReviewHint = Theme.Hint("复核中：Enter 确认并下一张 · D 丢弃本图自动框 · Delete 删选中框 · Esc 退出");
        _lblReviewHint.Name = "lblReviewHint";
        _lblReviewHint.Visible = false;
        AddRow(b0, _lblReviewHint);
        AddRow(stack, c0);

        // 本图标注
        var (c1, b1) = MakeCard("本图标注");
        _lvBoxes = new ListView
        {
            Name = "lvBoxes",
            Dock = DockStyle.Top,
            Height = 120,
            View = View.Details,
            FullRowSelect = true,
            MultiSelect = false,
            HideSelection = false,
            HeaderStyle = ColumnHeaderStyle.Nonclickable,
            BorderStyle = BorderStyle.FixedSingle,
            Font = Theme.Body,
            Margin = new Padding(0, 0, 0, 8),
        };
        _lvBoxes.Columns.Add("目标", 92);
        _lvBoxes.Columns.Add("存储名", 78);
        _lvBoxes.Columns.Add("框(x,y,w,h)", 152);
        AddRow(b1, _lvBoxes);

        var flow1 = new FlowLayoutPanel
        {
            Dock = DockStyle.Top,
            AutoSize = true,
            WrapContents = false,
            Margin = new Padding(0, 0, 0, 6),
        };
        _btnDelBox = Theme.Ghost("删除");
        Stretch(_btnDelBox, 68, 32);
        _btnRenameBox = Theme.Ghost("改名");
        Stretch(_btnRenameBox, 68, 32);
        _btnReexport = Theme.Ghost("重新导出");
        Stretch(_btnReexport, 92, 32);
        flow1.Controls.Add(_btnDelBox);
        flow1.Controls.Add(_btnRenameBox);
        flow1.Controls.Add(_btnReexport);
        AddRow(b1, flow1);

        _lblAnnoInfo = Theme.Hint("本图还没有标注：在左侧图片上拖动鼠标框选目标。");
        AddRow(b1, _lblAnnoInfo);
        AddRow(stack, c1);

        // 数据集
        var (c2, b2) = MakeCard("数据集");
        _lblDatasetStats = new Label
        {
            Text = "0 张图片 · 0 个框 · 0 类",
            Font = Theme.Body,
            ForeColor = Theme.Ink,
            AutoSize = true,
            Margin = new Padding(0, 0, 0, 6),
        };
        AddRow(b2, _lblDatasetStats);
        _lnkOpenDataset = Theme.Link("打开数据集文件夹");
        _lnkOpenDataset.Name = "lnkOpenDataset";
        _lnkOpenDataset.Margin = new Padding(0, 0, 0, 2);
        AddRow(b2, _lnkOpenDataset);
        AddRow(stack, c2);

        scroll.Controls.Add(stack);
        return scroll;
    }

    /// <summary>
    /// 训练区固定在页面底部、横跨整页：参数、按钮、日志始终可见，
    /// 不再因为右侧卡片超高而被滚出屏幕（这正是旧版“按钮看不见”的同类问题）。
    /// </summary>
    private Control BuildTrainingCard()
    {
        var (card, body) = MakeCard("训练模型");
        card.Name = "cardTrain";
        card.AutoSize = false;
        card.Dock = DockStyle.Fill;
        card.Margin = new Padding(0, 8, 0, 0);

        var grid = new TableLayoutPanel
        {
            Dock = DockStyle.Top,
            AutoSize = true,
            ColumnCount = 2,
            RowCount = 1,
            Margin = Padding.Empty,
        };
        grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55f));
        grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45f));

        var left = new TableLayoutPanel
        {
            Dock = DockStyle.Top,
            AutoSize = true,
            ColumnCount = 4,
            RowCount = 3,
            Margin = new Padding(0, 0, 12, 0),
        };
        left.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        left.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 26f));
        left.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        left.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 74f));

        left.Controls.Add(FieldLabel("轮数"), 0, 0);
        _numEpochs = new NumericUpDown
        {
            Minimum = 5,
            Maximum = 300,
            Increment = 5,
            Value = Math.Clamp(_settings.TrainEpochs, 5, 300),
            Dock = DockStyle.Fill,
            Font = Theme.Body,
            Margin = new Padding(0, 2, 12, 2),
        };
        left.Controls.Add(_numEpochs, 1, 0);
        left.Controls.Add(FieldLabel("尺寸"), 2, 0);
        _cboImgSize = MakeCombo();
        _cboImgSize.Margin = new Padding(0, 2, 0, 2);
        _cboImgSize.Items.AddRange(new object[] { "320", "416", "512", "640" });
        _cboImgSize.SelectedIndex = 3;
        left.Controls.Add(_cboImgSize, 3, 0);

        left.Controls.Add(FieldLabel("批大小"), 0, 1);
        _numBatch = new NumericUpDown
        {
            Minimum = 1,
            Maximum = 16,
            Value = Math.Clamp(_settings.TrainBatch, 1, 16),
            Dock = DockStyle.Fill,
            Font = Theme.Body,
            Margin = new Padding(0, 2, 12, 2),
        };
        left.Controls.Add(_numBatch, 1, 1);

        var flow = new FlowLayoutPanel
        {
            Dock = DockStyle.Top,
            AutoSize = true,
            WrapContents = false,
            Margin = new Padding(0, 4, 0, 2),
        };
        _btnInitEnv = Theme.Ghost("初始化环境");
        _btnInitEnv.Name = "btnInitEnv";
        Stretch(_btnInitEnv, 108, 34);
        _btnTrain = Theme.Success("开始训练");
        _btnTrain.Name = "btnTrain";
        Stretch(_btnTrain, 96, 34);
        _btnCancelTrain = Theme.Ghost("取消");
        _btnCancelTrain.Name = "btnCancelTrain";
        Stretch(_btnCancelTrain, 62, 34);
        _btnCancelTrain.Enabled = false;
        flow.Controls.Add(_btnInitEnv);
        flow.Controls.Add(_btnTrain);
        flow.Controls.Add(_btnCancelTrain);
        left.Controls.Add(flow, 0, 2);
        left.SetColumnSpan(flow, 4);

        var right = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
            ColumnCount = 1,
            RowCount = 3,
            Margin = Padding.Empty,
        };
        right.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        right.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        right.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

        _lblTrainStage = Theme.Hint("≥3 个标注框即可训练；CPU 50 轮约 5~15 分钟。");
        right.Controls.Add(_lblTrainStage, 0, 0);

        _pbTrain = new ProgressBar
        {
            Dock = DockStyle.Top,
            Height = 8,
            Style = ProgressBarStyle.Blocks,
            Margin = new Padding(0, 2, 0, 6),
        };
        right.Controls.Add(_pbTrain, 0, 1);

        _txtTrainLog = new TextBox
        {
            Name = "txtTrainLog",
            Dock = DockStyle.Fill,
            Height = 118,
            MinimumSize = new Size(0, 96),
            Multiline = true,
            ReadOnly = true,
            ScrollBars = ScrollBars.Vertical,
            WordWrap = false,
            BorderStyle = BorderStyle.FixedSingle,
            Margin = new Padding(0, 2, 0, 0),
        };
        Theme.StyleLog(_txtTrainLog);
        _txtTrainLog.Text = "训练日志会显示在这里。";
        right.Controls.Add(_txtTrainLog, 0, 2);

        grid.Controls.Add(left, 0, 0);
        grid.Controls.Add(right, 1, 0);
        body.Controls.Add(grid);
        return card;
    }

    private Control BuildStatusBar()
    {
        var bar = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 1,
            BackColor = Color.White,
            Padding = new Padding(14, 0, 14, 0),
            Margin = Padding.Empty,
        };
        bar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
        bar.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        bar.Paint += (_, e) =>
        {
            using var pen = new Pen(Theme.Border);
            e.Graphics.DrawLine(pen, 0, 0, bar.Width, 0);
        };
        _lblStatus = new Label
        {
            Text = "正在启动…",
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleLeft,
            Font = Theme.Small,
            ForeColor = Theme.Muted,
            AutoEllipsis = true,
            Margin = Padding.Empty,
        };
        _lblStatusRight = new Label
        {
            Text = "数据集：0 张图 · 0 个框",
            AutoSize = true,
            TextAlign = ContentAlignment.MiddleRight,
            Font = Theme.Small,
            ForeColor = Theme.Muted,
            Anchor = AnchorStyles.Right,
            Margin = Padding.Empty,
        };
        bar.Controls.Add(_lblStatus, 0, 0);
        bar.Controls.Add(_lblStatusRight, 1, 0);
        return bar;
    }

    // ---------------------------------------------------------------- helpers

    private static void Stretch(Button b, int width, int height)
    {
        b.AutoSize = false;
        b.Size = new Size(width, height);
        b.MinimumSize = new Size(width, height);
    }

    private static Label FieldLabel(string text) => new()
    {
        Text = text,
        AutoSize = true,
        Font = Theme.Body,
        ForeColor = Theme.Muted,
        Anchor = AnchorStyles.Left,
        Margin = new Padding(0, 7, 8, 6),
    };

    private static ComboBox MakeCombo() => new()
    {
        Dock = DockStyle.Fill,
        DropDownStyle = ComboBoxStyle.DropDownList,
        Font = Theme.Body,
        Margin = new Padding(0, 4, 12, 4),
    };

    private static NumericUpDown MakeNum(decimal value) => new()
    {
        DecimalPlaces = 2,
        Increment = 0.05m,
        Minimum = 0.01m,
        Maximum = 0.99m,
        Value = value,
        Width = 64,
        Font = Theme.Body,
        TextAlign = HorizontalAlignment.Right,
        Margin = new Padding(0, 2, 0, 2),
    };

    private static void AddRow(TableLayoutPanel t, Control c)
    {
        int row = t.RowCount;
        t.RowCount = row + 1;
        t.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        t.Controls.Add(c, 0, row);
    }

    private static (Panel Card, TableLayoutPanel Body) MakeCard(string title)
    {
        var card = Theme.CardPanel();
        card.Margin = new Padding(0, 0, 0, 10);
        var body = new TableLayoutPanel
        {
            Dock = DockStyle.Top,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            ColumnCount = 1,
            RowCount = 0,
            BackColor = Theme.Card,
            Margin = Padding.Empty,
        };
        body.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
        card.Controls.Add(body);
        AddRow(body, new Label
        {
            Text = title,
            Font = Theme.Section,
            ForeColor = Theme.Ink,
            AutoSize = true,
            Margin = new Padding(0, 0, 0, 8),
        });
        return (card, body);
    }

    private static void TrySetSplit(SplitContainer s, ref bool done, int rightWidth, int minRight)
    {
        if (done || s == null || s.Width < 760) return;
        try
        {
            int max = s.Width - minRight - s.SplitterWidth;
            int d = Math.Max(s.Panel1MinSize, Math.Min(s.Width - rightWidth, max));
            if (d > s.Panel1MinSize && d <= max)
            {
                s.SplitterDistance = d;
                done = true;
            }
        }
        catch (ArgumentOutOfRangeException)
        {
        }
        catch (InvalidOperationException)
        {
        }
    }

    private void InitSplitters()
    {
        TrySetSplit(_splitRec, ref _splitInit, 430, 340);
        TrySetSplit(_splitVideo, ref _videoSplitInit, 380, 330);
        TrySetSplit(_splitLearn, ref _learnSplitInit, 430, 380);
    }

    // ---------------------------------------------------------------- events

    private void WireEvents()
    {
        WireVideoEvents();
        _btnSelfCheck.Click += async (_, _) => await SelfCheckAsync();
        _btnSelectImage.Click += (_, _) => SelectRecognizeImage();
        // _btnRun 与 _btnStartVideo 是同一个按钮，点击由 WireVideoEvents 统一分发
        _btnClear.Click += (_, _) => ClearRecognition();
        _btnToggleAdv.Click += (_, _) => ToggleAdvanced();
        _btnAdvModel.Click += (_, _) => PickAdvancedModel();
        _btnVerifySave.Click += async (_, _) => await VerifyAndSaveAsync();
        _btnResetAdv.Click += (_, _) => ResetAdvanced();

        _picRec.DetectionLabel = d => $"{_labels.Display(d.RawCategory, d.ClassId)} {d.Confidence:P0}";
        _picRec.SelectionChanged += OnCanvasSelectionChanged;
        _grid.SelectionChanged += OnGridSelectionChanged;

        _btnOpenLearn.Click += (_, _) => SelectLearnImage();
        _btnOpenFolder.Click += (_, _) => OpenLearnFolder();
        _btnClearLearn.Click += (_, _) => ClearLearnImage();
        _btnPrevImage.Click += (_, _) => StepLearnImage(-1);
        _btnNextImage.Click += (_, _) => StepLearnImage(1);
        _lvImages.SelectedIndexChanged += (_, _) => OnImageListSelectionChanged();
        _picLearn.BoxDrawn += OnBoxDrawn;
        _picLearn.BoxEdited += OnBoxEdited;
        _picLearn.SelectionChanged += OnLearnCanvasSelectionChanged;
        _picLearn.BoxPending = b => b.Pending;
        _btnAutoLabel.Click += async (_, _) => await AutoLabelPendingAsync();
        _btnReview.Click += (_, _) => ToggleReview();
        _btnClearAuto.Click += (_, _) => ClearAllAutoBoxes();
        _lvBoxes.SelectedIndexChanged += OnBoxListSelectionChanged;
        _btnDelBox.Click += (_, _) => DeleteSelectedBox();
        _btnRenameBox.Click += (_, _) => RenameSelectedBox();
        _btnReexport.Click += (_, _) => ReexportAnnotations();
        _lnkOpenDataset.Click += (_, _) => OpenDatasetFolder();

        _btnInitEnv.Click += async (_, _) => await InitEnvironmentAsync();
        _btnTrain.Click += async (_, _) => await StartTrainingAsync();
        _btnCancelTrain.Click += (_, _) =>
        {
            _training.Cancel();
            AppendTrainLog("已请求取消训练…");
        };

        _training.LogLine += line => BeginInvokeSafe(() => AppendTrainLog(line));
        _training.Progress += p => BeginInvokeSafe(() =>
        {
            _pbTrain.Style = ProgressBarStyle.Continuous;
            _pbTrain.Value = Math.Max(0, Math.Min(100, p));
            _lblTrainStage.Text = $"正在训练… {p}%";
        });
        _training.Stage += s => BeginInvokeSafe(() => _lblTrainStage.Text = s);

        _splitRec.SizeChanged += (_, _) => InitSplitters();
        _splitLearn.SizeChanged += (_, _) => InitSplitters();
    }

    private async void MainForm_Load(object sender, EventArgs e)
    {
        RefreshAdvancedFromSettings();
        RefreshVideoSettings();
        RefreshDatasetStats();
        SetBusy(true);
        _lblStatus.Text = "正在自检本机模型与推理后端…";
        Text = "YoloFool · 傻瓜式视觉识别";
        try
        {
            var result = await Task.Run(() => SelfCheck.Run(_settings));
            if (IsDisposed)
            {
                result.Service?.Dispose();
                return;
            }
            ApplySelfCheckResult(result, false);

            var last = _settings.LastImagePath;
            if (!string.IsNullOrWhiteSpace(last) && File.Exists(last) && ImageUtil.IsSupported(last))
                LoadRecognitionImage(last, true);
            else if (File.Exists(AppPaths.SampleImage))
                LoadRecognitionImage(AppPaths.SampleImage, true);

            var lastLearn = _settings.LastLearnImagePath;
            if (!string.IsNullOrWhiteSpace(lastLearn) && File.Exists(lastLearn))
                LoadLearnImage(lastLearn);
        }
        catch (Exception ex)
        {
            Log.Error("启动自检异常", ex);
            UpdateModelStatus(false, "", "");
            Text = "YoloFool · 傻瓜式视觉识别 · 加载失败";
            _lblStatus.Text = "启动异常：" + ex.Message;
        }
        finally
        {
            SetBusy(false);
            UpdateRunEnabled();
        }
    }

    private async Task SelfCheckAsync()
    {
        if (_busy) return;
        SetBusy(true);
        _lblStatus.Text = "正在重新自检…";
        try
        {
            var result = await Task.Run(() => SelfCheck.Run(_settings));
            if (IsDisposed)
            {
                result.Service?.Dispose();
                return;
            }
            ApplySelfCheckResult(result, true);
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, "自检异常：" + ex.Message, "自检", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        finally
        {
            SetBusy(false);
            UpdateRunEnabled();
        }
    }

    private void ApplySelfCheckResult(SelfCheckResult r, bool announce)
    {
        if (r.Ok)
        {
            var old = _svc;
            _svc = r.Service;
            old?.Dispose();

            _settings.Apply(r.Options);
            _settings.SelfCheckAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            _settings.SelfCheckText = r.Message + (string.IsNullOrEmpty(r.Detail) ? "" : "；" + r.Detail);
            _settings.Save();

            UpdateModelStatus(true, r.Message, string.IsNullOrEmpty(r.Detail) ? "默认值已按本机验证" : r.Detail);
            _lblStatus.Text = "自检通过：" + r.Message;
            Text = "YoloFool · 傻瓜式视觉识别 · 就绪";

            if (announce)
                MessageBox.Show(this,
                    "验证通过，已保存并应用。\n\n" + r.Message +
                    (string.IsNullOrEmpty(r.Detail) ? "" : "\n\n" + r.Detail),
                    "自检通过", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        else
        {
            UpdateModelStatus(false, "", "");
            _lblStatus.Text = "自检失败：" + r.Message;
            Text = "YoloFool · 傻瓜式视觉识别 · 加载失败";
            if (announce)
                MessageBox.Show(this, r.Message + "\n\n" + r.Detail, "自检失败",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        RefreshAdvancedFromSettings();
        UpdateRunEnabled();
    }

    private void UpdateModelStatus(bool ok, string message, string detail)
    {
        _lblModelStatus.Text = ok
            ? "● 就绪" + (string.IsNullOrEmpty(message) ? "" : " · " + message)
            : "● 不可用";
        _lblModelStatus.ForeColor = ok ? Theme.Ok : Theme.Danger;
        _lblModelSub.Text = ok
            ? (string.IsNullOrEmpty(detail) ? "默认值已按本机验证" : detail)
            : "请检查模型文件，或点“重新自检”";
    }

    private void SetBusy(bool busy)
    {
        _busy = busy;
        UseWaitCursor = busy;
        _btnSelfCheck.Enabled = !busy;
        _btnSelectImage.Enabled = !busy;
        _btnClear.Enabled = !busy;
        _btnToggleAdv.Enabled = !busy;
        _btnAdvModel.Enabled = !busy;
        _btnVerifySave.Enabled = !busy;
        _btnResetAdv.Enabled = !busy;
        UpdateRunEnabled();
    }

    private void UpdateRunEnabled()
    {
        _btnRun.Enabled = !_busy && !_trainBusy && _svc != null;
    }

    private void ToggleAdvanced()
    {
        _advExpanded = !_advExpanded;
        _pnlAdv.Visible = _advExpanded;
        _btnToggleAdv.Text = _advExpanded ? "▾ 收起设置" : "▸ 详细设置";
        _settings.AdvancedExpanded = _advExpanded;
        _settings.Save();
        if (_advExpanded) _ = RefreshProbesAsync();
    }

    private async Task RefreshProbesAsync()
    {
        var path = _tbAdvModel.Text.Trim();
        if (!File.Exists(path)) path = _settings.ModelPath;
        if (!File.Exists(path))
        {
            _lblAdvProbe.Text = "未找到模型文件。";
            return;
        }
        _lblAdvProbe.Text = "正在探测本机推理后端…";
        var kind = (YoloModelKind)Math.Max(0, Math.Min(2, _cboKind.SelectedIndex));
        try
        {
            var list = await Task.Run(() => SelfCheck.ProbeProviders(path, kind));
            if (IsDisposed) return;
            _probes = list;
            _lblAdvProbe.Text = "本机后端：" + string.Join("   ·   ", list.Select(p =>
                p.Ok
                    ? $"{ProviderLabel(p.Provider)} 可用 {p.Milliseconds} ms"
                    : $"{ProviderLabel(p.Provider)} 不可用（{TrimText(p.Detail, 46)}）"));
        }
        catch (Exception ex)
        {
            _lblAdvProbe.Text = "后端探测失败：" + ex.Message;
        }
    }

    private void RefreshAdvancedFromSettings()
    {
        _tbAdvModel.Text = _settings.ModelPath ?? "";
        _cboKind.SelectedIndex = (int)ModelOptions.ParseKind(_settings.ModelKind);
        _cboDevice.SelectedIndex = ModelOptions.ParseDevice(_settings.Device) == DeviceType.GPU0 ? 1 : 0;
        _cboProvider.SelectedIndex = ModelOptions.ParseProvider(_settings.OnnxProvider) == OnnxRuntimeDeviceType.DML ? 1 : 0;
        _numConf.Value = Clamp((decimal)_settings.Confidence, 0.01m, 0.99m);
        _numNms.Value = Clamp((decimal)_settings.Nms, 0.01m, 0.99m);

        _lblAdvInfo.Text = _svc?.Info == null
            ? "当前：未加载模型"
            : $"当前：{_svc.Info.FileName} · {_svc.Info.ClassCount} 类 · {ProviderLabel(_svc.Info.Provider)} · {_svc.Info.LoadMilliseconds} ms 加载";
    }

    private void PickAdvancedModel()
    {
        using var dlg = new OpenFileDialog
        {
            Title = "选择 ONNX 模型",
            Filter = "ONNX 模型|*.onnx|所有文件|*.*",
            CheckFileExists = true,
        };
        var dir = Path.GetDirectoryName(_tbAdvModel.Text);
        if (!string.IsNullOrEmpty(dir) && Directory.Exists(dir)) dlg.InitialDirectory = dir;
        if (dlg.ShowDialog(this) == DialogResult.OK)
        {
            _tbAdvModel.Text = dlg.FileName;
            _lblAdvProbe.Text = "模型已更换，请点“验证并保存”。";
        }
    }

    private async Task VerifyAndSaveAsync()
    {
        if (_busy) return;
        var opt = OptionsFromAdvanced();
        if (!File.Exists(opt.ModelPath))
        {
            MessageBox.Show(this, "模型文件不存在：\n" + opt.ModelPath, "验证设置",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        SetBusy(true);
        _lblAdvProbe.Text = "正在验证新设置…";
        try
        {
            var (ok, svc, info, ms, error) = await Task.Run(() =>
            {
                InferenceService s = null;
                try
                {
                    s = new InferenceService();
                    var i = s.Load(opt);
                    s.Warmup();
                    long t = 0;
                    if (File.Exists(AppPaths.SampleImage)) t = s.Predict(AppPaths.SampleImage).ElapsedMilliseconds;
                    return (true, s, i, t, "");
                }
                catch (Exception ex)
                {
                    s?.Dispose();
                    return (false, (InferenceService)null, (ModelRuntimeInfo)null, 0L, ex.Message);
                }
            });

            if (!ok)
            {
                _lblAdvProbe.Text = "验证失败：" + error;
                MessageBox.Show(this, "新设置验证失败，未保存，当前模型保持不变。\n\n" + error,
                    "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var old = _svc;
            _svc = svc;
            old?.Dispose();

            _settings.Apply(opt);
            _settings.SelfCheckAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            _settings.SelfCheckText = $"{info.ClassCount} 类 · {ProviderLabel(info.Provider)} · {ms} ms";
            _settings.Save();

            _lblAdvProbe.Text = $"验证通过：{info.ClassCount} 类，{ProviderLabel(info.Provider)}，约 {ms} ms/张。已保存。";
            UpdateModelStatus(true, $"{ProviderLabel(info.Provider)} · {ms} ms/张 · {info.ClassCount} 类", "设置已验证并保存");
            _lblStatus.Text = "设置已保存并切换模型。";
            UpdateRunEnabled();
        }
        catch (Exception ex)
        {
            _lblAdvProbe.Text = "验证异常：" + ex.Message;
            MessageBox.Show(this, "验证异常：" + ex.Message, "验证设置",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        finally
        {
            SetBusy(false);
            RefreshAdvancedFromSettings();
        }
    }

    private ModelOptions OptionsFromAdvanced()
    {
        var kind = (YoloModelKind)Math.Max(0, Math.Min(2, _cboKind.SelectedIndex));
        var device = _cboDevice.SelectedIndex == 1 ? DeviceType.GPU0 : DeviceType.CPU;
        var provider = _cboProvider.SelectedIndex == 1 ? OnnxRuntimeDeviceType.DML : OnnxRuntimeDeviceType.Default;
        return new ModelOptions(
            _tbAdvModel.Text.Trim(),
            kind,
            InferenceBackend.OnnxRuntime,
            device,
            provider,
            (float)_numConf.Value,
            (float)_numNms.Value);
    }

    private void ResetAdvanced()
    {
        _tbAdvModel.Text = File.Exists(AppPaths.DefaultCocoModel) ? AppPaths.DefaultCocoModel : _settings.ModelPath;
        _cboKind.SelectedIndex = 0;
        _cboDevice.SelectedIndex = 0;
        _cboProvider.SelectedIndex = 0;
        _numConf.Value = 0.5m;
        _numNms.Value = 0.5m;
        _lblAdvProbe.Text = "已恢复内置默认值；点“验证并保存”后生效。";
    }

    // ---------------------------------------------------------------- recognize

    private void SelectRecognizeImage()
    {
        using var dlg = new OpenFileDialog
        {
            Title = "选择要识别的图片",
            Filter = "图片|*.jpg;*.jpeg;*.png;*.bmp;*.webp|所有文件|*.*",
            CheckFileExists = true,
        };
        var dir = Path.GetDirectoryName(_recPath);
        if (!string.IsNullOrEmpty(dir) && Directory.Exists(dir)) dlg.InitialDirectory = dir;
        else if (Directory.Exists(Path.Combine(AppPaths.WorkspaceRoot, "demo", "images")))
            dlg.InitialDirectory = Path.Combine(AppPaths.WorkspaceRoot, "demo", "images");
        if (dlg.ShowDialog(this) == DialogResult.OK) LoadRecognitionImage(dlg.FileName, false);
    }

    private void LoadRecognitionImage(string path, bool silent)
    {
        try
        {
            var bmp = ImageUtil.LoadBitmap(path);
            var old = _recBitmap;
            _recBitmap = bmp;
            _picRec.Image = bmp;
            old?.Dispose();

            _recPath = path;
            _recInMemory = false;
            _tbRecImage.Text = path;
            _detections = Array.Empty<DetectionItem>();
            _picRec.Detections = _detections;
            _picRec.SelectedIndex = -1;

            _syncing = true;
            _grid.Rows.Clear();
            _syncing = false;
            _lblResultStats.Text = "图片已载入，点“开始识别”。";
            _lblStatus.Text = $"图片：{Path.GetFileName(path)}（{bmp.Width}×{bmp.Height}）";

            _settings.LastImagePath = path;
            _settings.Save();

            _capSource = CaptureSource.Image;
            ApplyCaptureLayout();
        }
        catch (Exception ex)
        {
            if (!silent)
                MessageBox.Show(this, "无法打开图片：\n" + ex.Message, "图片", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            _lblStatus.Text = "图片读取失败：" + ex.Message;
        }
    }

    /// <summary>把内存里的位图（拍照定格）直接显示到画布，不写磁盘；之后“开始识别”直接从位图推理。</summary>
    private void LoadRecognitionBitmap(Bitmap bmp, string note)
    {
        if (bmp == null) return;

        var old = _recBitmap;
        _recBitmap = bmp;
        _picRec.Image = bmp;
        old?.Dispose();

        _recPath = "";
        _recInMemory = true;
        _tbRecImage.Text = "（拍照，未保存）";
        _detections = Array.Empty<DetectionItem>();
        _rawDetections = Array.Empty<DetectionItem>();
        _lastResult = null;
        _picRec.Detections = _detections;
        _picRec.SelectedIndex = -1;

        _syncing = true;
        _grid.Rows.Clear();
        _syncing = false;

        _lblResultStats.Text = "照片已定格，点“开始识别”。";
        _lblStatus.Text = string.IsNullOrEmpty(note) ? "照片已定格（未保存到本地）。" : note;
        _capSource = CaptureSource.Image;
        ApplyCaptureLayout();
    }

    private void ClearRecognition()
    {
        _recPath = "";
        _recInMemory = false;
        _tbRecImage.Text = "";
        var old = _recBitmap;
        _recBitmap = null;
        _picRec.Image = null;
        old?.Dispose();
        _detections = Array.Empty<DetectionItem>();
        _rawDetections = Array.Empty<DetectionItem>();
        _lastResult = null;
        _picRec.Detections = _detections;
        _syncing = true;
        _grid.Rows.Clear();
        _syncing = false;
        _lblResultStats.Text = "尚未识别。";
        _lblStatus.Text = "已清空识别图片。";
        _capSource = CaptureSource.None;
        ApplyCaptureLayout();
    }

    private async Task RunRecognitionAsync()
    {
        if (_busy || _svc == null) return;

        // 拍照定格的照片只在内存里，没有磁盘路径，直接从位图推理
        bool inMemory = _recInMemory && _recBitmap != null;
        if (!inMemory && (string.IsNullOrEmpty(_recPath) || !File.Exists(_recPath)))
        {
            SelectRecognizeImage();
            if (string.IsNullOrEmpty(_recPath)) return;
        }

        SetBusy(true);
        _lblStatus.Text = "正在识别…";
        try
        {
            var path = _recPath;
            Bitmap pending = inMemory ? new Bitmap(_recBitmap) : null;
            var result = await Task.Run(() =>
            {
                if (pending == null) return _svc.Predict(path);
                using (pending)
                using (var mat = ToMat(pending))
                    return _svc.Predict(mat);
            });
            if (IsDisposed) return;

            _rawDetections = result.Items;
            ShowDetections(result, inMemory ? "" : path);
            _lblStatus.Text = $"识别完成：{_detections.Count} 个目标 · {result.ElapsedMilliseconds} ms";

            if (!inMemory)
            {
                _settings.LastImagePath = path;
                _settings.Save();
            }
        }
        catch (Exception ex)
        {
            Log.Error("识别失败", ex);
            MessageBox.Show(this, "识别失败：\n" + ex.Message, "识别", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            _lblStatus.Text = "识别失败：" + ex.Message;
        }
        finally
        {
            SetBusy(false);
        }
    }

    private void FillResultGrid(PredictionResult result)
    {
        _syncing = true;
        try
        {
            _grid.Rows.Clear();
            foreach (var d in _detections)
            {
                var name = _labels.Display(d.RawCategory, d.ClassId);
                int i = _grid.Rows.Add();
                var cells = _grid.Rows[i].Cells;
                cells["colName"].Value = name;
                cells["colConf"].Value = d.Confidence.ToString("P1");
                cells["colPos"].Value = RelativePosition.Short(d.CenterXPercent, d.CenterYPercent);
                cells["colCenter"].Value = $"{d.CenterXPercent:F0}%,{d.CenterYPercent:F0}%";
                _grid.Rows[i].Tag = d;
            }

            if (_grid.Rows.Count > 0)
            {
                // colId 在图片模式下是隐藏列，不能用 Cells[0]
                _grid.CurrentCell = _grid.Rows[0].Cells["colName"];
                _grid.Rows[0].Selected = true;
            }
        }
        finally
        {
            _syncing = false;
        }

        var count = _detections.Count;
        _lblResultStats.Text = count == 0
            ? $"没有匹配到目标 · {result.ElapsedMilliseconds} ms。可展开“详细设置”降低置信度。"
            : $"{count} 个目标 · {result.ElapsedMilliseconds} ms · {result.ImageWidth}×{result.ImageHeight}";

        if (count > 0) OnGridSelectionChanged(_grid, EventArgs.Empty);
    }

    private void OnGridSelectionChanged(object sender, EventArgs e)
    {
        if (_syncing) return;
        var row = _grid.CurrentRow;
        if (row == null || row.Index < 0 || row.Index >= _detections.Count)
        {
            _picRec.SelectedIndex = -1;
            return;
        }
        _syncing = true;
        _picRec.SelectedIndex = row.Index;
        _syncing = false;

        var d = _detections[row.Index];
        _lblStatus.Text =
            $"{_labels.Display(d.RawCategory, d.ClassId)}：匹配度 {d.Confidence:P1}，{RelativePosition.Describe(d.CenterXPercent, d.CenterYPercent)}";
    }

    private void OnCanvasSelectionChanged(object sender, int index)
    {
        if (_syncing) return;
        if (index < 0 || index >= _grid.Rows.Count) return;
        _syncing = true;
        _grid.CurrentCell = _grid.Rows[index].Cells["colName"];
        _grid.Rows[index].Selected = true;
        _syncing = false;
    }

    // ---------------------------------------------------------------- learning

    private void SelectLearnImage()
    {
        using var dlg = new OpenFileDialog
        {
            Title = "选择要标注的图片",
            Filter = "图片|*.jpg;*.jpeg;*.png;*.bmp;*.webp|所有文件|*.*",
            CheckFileExists = true,
        };
        var dir = Path.GetDirectoryName(_learnPath);
        if (!string.IsNullOrEmpty(dir) && Directory.Exists(dir)) dlg.InitialDirectory = dir;
        if (dlg.ShowDialog(this) == DialogResult.OK) LoadLearnImage(dlg.FileName);
    }

    private void LoadLearnImage(string path)
    {
        try
        {
            var bmp = ImageUtil.LoadBitmap(path);
            var old = _learnBitmap;
            _learnBitmap = bmp;
            _picLearn.Image = bmp;
            old?.Dispose();

            _learnPath = path;
            _tbLearnImage.Text = path;
            _picLearn.Boxes = _store.GetBoxes(path);
            ReloadBoxList();
            _lblStatus.Text = $"标注图片：{Path.GetFileName(path)}（{bmp.Width}×{bmp.Height}）";

            _settings.LastLearnImagePath = path;
            _settings.Save();
            EnsureLearnListed(path);
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, "无法打开图片：\n" + ex.Message, "图片", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void ClearLearnImage()
    {
        _imageFiles.Clear();
        _syncingImages = true;
        try { _lvImages.Items.Clear(); }
        finally { _syncingImages = false; }
        _lblImageListInfo.Text = "还没有图片列表：点“打开文件夹”一次载入整个文件夹。";
        _learnPath = "";
        _tbLearnImage.Text = "";
        var old = _learnBitmap;
        _learnBitmap = null;
        _picLearn.Image = null;
        old?.Dispose();
        _picLearn.Boxes = Array.Empty<BoxRecord>();
        _lvBoxes.Items.Clear();
        _lblAnnoInfo.Text = "本图还没有标注：在左侧图片上拖动鼠标框选目标。";
    }

    private static readonly string[] LearnImageExts = { ".jpg", ".jpeg", ".png", ".bmp", ".webp" };

    /// <summary>选一个文件夹，把里面的图片排成列表，逐张标注。</summary>
    private void OpenLearnFolder()
    {
        using var dlg = new FolderBrowserDialog
        {
            Description = "选择要批量标注的图片文件夹",
            UseDescriptionForTitle = true,
            ShowNewFolderButton = false,
        };
        var dir = Path.GetDirectoryName(_learnPath);
        if (!string.IsNullOrEmpty(dir) && Directory.Exists(dir)) dlg.SelectedPath = dir;
        if (dlg.ShowDialog(this) != DialogResult.OK) return;
        LoadLearnFolder(dlg.SelectedPath);
    }

    private void LoadLearnFolder(string folder)
    {
        if (string.IsNullOrEmpty(folder) || !Directory.Exists(folder)) return;
        _imageFiles.Clear();
        try
        {
            _imageFiles.AddRange(EnumerateFolderImages(folder));
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, "读取文件夹失败：\n" + ex.Message, "图片列表",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (_imageFiles.Count == 0)
        {
            RebuildImageList();
            _lblImageListInfo.Text = "这个文件夹里没有图片（支持 jpg / jpeg / png / bmp / webp）。";
            _lblStatus.Text = "该文件夹没有可用图片：" + folder;
            return;
        }

        RebuildImageList();
        LoadLearnImage(_imageFiles[0]);
        _lblStatus.Text = $"已载入 {_imageFiles.Count} 张图片：{folder}";
    }

    private static IEnumerable<string> EnumerateFolderImages(string folder) =>
        Directory.EnumerateFiles(folder)
            .Where(p => LearnImageExts.Contains(Path.GetExtension(p), StringComparer.OrdinalIgnoreCase))
            .OrderBy(p => Path.GetFileName(p), StringComparer.OrdinalIgnoreCase);

    /// <summary>单张打开图片时，同步把它所在文件夹排进列表；已在列表里就只刷新行文字和选中项。</summary>
    private void EnsureLearnListed(string path)
    {
        if (_imageFiles.Any(p => string.Equals(p, path, StringComparison.OrdinalIgnoreCase)))
        {
            UpdateCurrentImageRow();
            SelectCurrentImageRow();
            return;
        }

        _imageFiles.Clear();
        var dir = Path.GetDirectoryName(path);
        if (!string.IsNullOrEmpty(dir) && Directory.Exists(dir))
        {
            try { _imageFiles.AddRange(EnumerateFolderImages(dir)); }
            catch (Exception ex) { Log.Error("枚举图片文件夹失败：" + dir, ex); }
        }
        if (_imageFiles.Count == 0) _imageFiles.Add(path);
        RebuildImageList();
    }

    private void RebuildImageList()
    {
        _syncingImages = true;
        try
        {
            _lvImages.BeginUpdate();
            _lvImages.Items.Clear();
            foreach (var f in _imageFiles)
                _lvImages.Items.Add(ImageRowItem(f));
            _lvImages.EndUpdate();
            SelectCurrentImageRow();
        }
        finally
        {
            _syncingImages = false;
        }

        RefreshImageListInfo();
    }

    /// <summary>列表下方那行统计：图片总数 / 已标注张数。</summary>
    private void RefreshImageListInfo()
    {
        if (_lblImageListInfo == null) return;
        if (_imageFiles.Count == 0)
        {
            _lblImageListInfo.Text = "还没有图片列表：点“打开文件夹”一次载入整个文件夹，或用“打开图片”逐张打开。";
            return;
        }
        int done = _imageFiles.Count(p => _store.GetExportableBoxes(p).Count > 0);
        int pending = _imageFiles.Count(p => _store.PendingCount(p) > 0);
        _lblImageListInfo.Text = $"{_imageFiles.Count} 张图片 · 已标注 {done} 张"
                                  + (pending > 0 ? $" · 待复核 {pending} 张" : "")
                                  + " · 选中列表即可切换。";
    }

    /// <summary>列表行：文件名 / 参与训练的框个数 / 状态（空、待复核、已确认、手工）。</summary>
    private ListViewItem ImageRowItem(string path)
    {
        var item = new ListViewItem(Path.GetFileName(path)) { Tag = path };
        item.SubItems.Add(ImageCountText(path));
        item.SubItems.Add(ImageStateText(path));
        return item;
    }

    /// <summary>只数会进训练集的框：待复核的自动框不算。</summary>
    private string ImageCountText(string path) => _store.GetExportableBoxes(path).Count.ToString();

    /// <summary>这张图的整体状态：有待复核的自动框时优先提示，因为复核是训练前的必经步骤。</summary>
    private string ImageStateText(string path)
    {
        var boxes = _store.GetBoxes(path);
        if (boxes.Count == 0) return "空";
        int pending = boxes.Count(b => b.Pending);
        if (pending > 0) return $"待复核 {pending}";
        return boxes.Any(b => b.Auto) ? "已确认" : "手工";
    }

    private void UpdateCurrentImageRow()
    {
        if (_lvImages == null || string.IsNullOrEmpty(_learnPath)) return;
        foreach (ListViewItem item in _lvImages.Items)
        {
            if (item.Tag is string p && string.Equals(p, _learnPath, StringComparison.OrdinalIgnoreCase))
            {
                item.Text = Path.GetFileName(_learnPath);
                string count = ImageCountText(_learnPath);
                string state = ImageStateText(_learnPath);
                if (item.SubItems.Count > 2)
                {
                    item.SubItems[1].Text = count;
                    item.SubItems[2].Text = state;
                }
                else
                {
                    item.SubItems.Add(count);
                    item.SubItems.Add(state);
                }
                RefreshImageListInfo();
                return;
            }
        }
    }

    private void SelectCurrentImageRow()
    {
        if (_lvImages == null || string.IsNullOrEmpty(_learnPath)) return;
        for (int i = 0; i < _lvImages.Items.Count; i++)
        {
            if (_lvImages.Items[i].Tag is string p && string.Equals(p, _learnPath, StringComparison.OrdinalIgnoreCase))
            {
                _lvImages.Items[i].Selected = true;
                _lvImages.Items[i].Focused = true;
                _lvImages.EnsureVisible(i);
                return;
            }
        }
    }

    private void StepLearnImage(int delta)
    {
        if (_imageFiles.Count == 0)
        {
            OpenLearnFolder();
            return;
        }

        int index = _imageFiles.FindIndex(p => string.Equals(p, _learnPath, StringComparison.OrdinalIgnoreCase));
        int next = index < 0 ? 0 : index + delta;
        if (next < 0)
        {
            _lblStatus.Text = "已经是第一张图片了。";
            return;
        }
        if (next >= _imageFiles.Count)
        {
            _lblStatus.Text = "已经是最后一张图片了。";
            return;
        }
        LoadLearnImage(_imageFiles[next]);
    }

    private void OnImageListSelectionChanged()
    {
        if (_syncingImages) return;
        if (_lvImages.SelectedItems.Count == 0) return;
        if (_lvImages.SelectedItems[0].Tag is not string path) return;
        if (string.Equals(path, _learnPath, StringComparison.OrdinalIgnoreCase)) return;
        LoadLearnImage(path);
    }

    private void ReloadBoxList()
    {
        var boxes = string.IsNullOrEmpty(_learnPath) ? Array.Empty<BoxRecord>() : _store.GetBoxes(_learnPath);
        _syncing = true;
        try
        {
            _lvBoxes.BeginUpdate();
            _lvBoxes.Items.Clear();
            foreach (var b in boxes)
                _lvBoxes.Items.Add(new ListViewItem(new[] { b.Cn, b.En, b.BoundsText }));
            _lvBoxes.EndUpdate();

            _picLearn.Boxes = boxes;
            if (_lvBoxes.Items.Count > 0)
            {
                _lvBoxes.Items[_lvBoxes.Items.Count - 1].Selected = true;
                _lvBoxes.Items[_lvBoxes.Items.Count - 1].Focused = true;
                _picLearn.SelectedIndex = _lvBoxes.Items.Count - 1;
            }
            else
            {
                _picLearn.SelectedIndex = -1;
            }
        }
        finally
        {
            _syncing = false;
        }

        _lblAnnoInfo.Text = boxes.Count == 0
            ? "本图还没有标注：在左侧图片上拖动鼠标框选目标。"
            : $"本图 {boxes.Count} 个框；标注已自动导出到 mydataset/images 与 mydataset/labels。";
        UpdateCurrentImageRow();
        RefreshDatasetStats();
    }

    private void OnBoxEdited(object sender, BoxEdit edit)
    {
        if (string.IsNullOrEmpty(_learnPath)) return;
        try
        {
            _store.UpdateBox(_learnPath, edit.Index, edit.Bounds, edit.Angle);
            ReloadBoxList();
            if (edit.Index >= 0 && edit.Index < _lvBoxes.Items.Count)
            {
                _lvBoxes.Items[edit.Index].Selected = true;
                _picLearn.SelectedIndex = edit.Index;
            }
            _lblStatus.Text = "已更新框的大小/角度，并重新导出标注。";
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, "保存框调整失败：\n" + ex.Message, "标注", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void OnBoxDrawn(object sender, BoxDraft draft)
    {
        if (string.IsNullOrEmpty(_learnPath)) return;
        using var dlg = new NameDialog(_labels);
        if (dlg.ShowDialog(this) != DialogResult.OK) return;
        try
        {
            int idx = _store.AddBox(_learnPath, dlg.DisplayName, draft.Bounds, draft.Angle);
            ReloadBoxList();
            if (idx >= 0 && idx < _lvBoxes.Items.Count)
            {
                _lvBoxes.Items[idx].Selected = true;
                _picLearn.SelectedIndex = idx;
            }
            _lblStatus.Text = $"已保存并自动导出：{dlg.DisplayName}（本图 {idx + 1} 个框）";
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, "保存标注失败：\n" + ex.Message, "标注", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private int SelectedBoxIndex()
    {
        if (_lvBoxes.SelectedIndices.Count == 0) return -1;
        return _lvBoxes.SelectedIndices[0];
    }

    private void OnBoxListSelectionChanged(object sender, EventArgs e)
    {
        if (_syncing) return;
        _syncing = true;
        _picLearn.SelectedIndex = SelectedBoxIndex();
        _syncing = false;
    }

    private void OnLearnCanvasSelectionChanged(object sender, int index)
    {
        if (_syncing) return;
        _syncing = true;
        if (index >= 0 && index < _lvBoxes.Items.Count)
        {
            _lvBoxes.SelectedIndices.Clear();
            _lvBoxes.Items[index].Selected = true;
        }
        _syncing = false;
    }

    private void DeleteSelectedBox()
    {
        var idx = SelectedBoxIndex();
        if (idx < 0 || string.IsNullOrEmpty(_learnPath))
        {
            _lblAnnoInfo.Text = "先在列表里选中一个框。";
            return;
        }
        var boxes = _store.GetBoxes(_learnPath);
        if (idx >= boxes.Count) return;
        if (MessageBox.Show(this, $"删除“{boxes[idx].Cn}”这个框？", "删除标注",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
        _store.RemoveBox(_learnPath, idx);
        ReloadBoxList();
        _lblStatus.Text = "已删除框，并重新导出标注。";
    }

    private void RenameSelectedBox()
    {
        var idx = SelectedBoxIndex();
        if (idx < 0 || string.IsNullOrEmpty(_learnPath)) return;
        var boxes = _store.GetBoxes(_learnPath);
        if (idx >= boxes.Count) return;
        using var dlg = new NameDialog(_labels, boxes[idx].Cn);
        if (dlg.ShowDialog(this) != DialogResult.OK) return;
        _store.RenameBox(_learnPath, idx, dlg.DisplayName);
        ReloadBoxList();
        if (idx < _lvBoxes.Items.Count) _picLearn.SelectedIndex = idx;
        _lblStatus.Text = "已改名，并重新导出标注。";
    }

    private void ReexportAnnotations()
    {
        if (string.IsNullOrEmpty(_learnPath)) return;
        try
        {
            _store.SyncImage(_learnPath);
            _lblStatus.Text = "已重新导出本图标注。";
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, "导出失败：\n" + ex.Message, "导出", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void RefreshDatasetStats()
    {
        var text = $"{_store.ImageCount} 张图片 · {_store.BoxCount} 个框 · {_store.Classes.Count} 类";
        if (_store.PendingBoxCount > 0) text += $" · 待复核 {_store.PendingBoxCount} 个框（不进训练集）";
        _lblDatasetStats.Text = text;
        _lblStatusRight.Text = "数据集：" + text;
    }

    /// <summary>
    /// “打开数据集文件夹”链接的点击处理。
    /// 旧版这个链接只创建了按钮、从未挂 Click 事件，点下去毫无反应（也不报错）。
    /// 这里补齐实现：目录不存在先创建，再交给系统资源管理器打开。沙箱工作区里的 exe
    /// 会被降为 Low 完整性而无法调起资源管理器，判断与多种打开方式见
    /// <see cref="ShellFolder"/>；失败时写日志、状态栏提示并弹窗给出真实路径，
    /// 避免再次出现“点了没反应”的静默失败。
    /// </summary>
    private void OpenDatasetFolder()
    {
        var dir = AppPaths.DatasetDir;
        try
        {
            AppPaths.EnsureDirectories();
            if (!Directory.Exists(dir))
                throw new DirectoryNotFoundException("文件夹不存在且无法创建：" + dir);

            if (!ShellFolder.TryOpen(dir, out var shellError))
                throw new InvalidOperationException("系统资源管理器没有响应打开请求（" + shellError + "）");

            _lblStatus.Text = "已在资源管理器中打开数据集文件夹：" + dir;
            Log.Info("打开数据集文件夹：" + dir);
        }
        catch (Exception ex)
        {
            Log.Error("打开数据集文件夹失败：" + dir, ex);
            _lblStatus.Text = "打开数据集文件夹失败：" + ex.Message;
            MessageBox.Show(this,
                "无法打开数据集文件夹：\n" + dir + "\n\n" + ex.Message +
                "\n\n可以按 Ctrl+C 复制上面的路径，自己到资源管理器里打开。",
                "打开文件夹", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    // ---------------------------------------------------------------- training

    private async Task InitEnvironmentAsync()
    {
        if (_trainBusy) return;
        _trainBusy = true;
        SetTrainingUi(true);
        _lblTrainStage.Text = "正在检查训练环境…";
        try
        {
            var msg = await _training.InitEnvironmentAsync();
            _lblTrainStage.Text = msg;
            MessageBox.Show(this, msg, "训练环境", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            _lblTrainStage.Text = "初始化失败：" + ex.Message;
            MessageBox.Show(this, "初始化失败：\n" + ex.Message, "训练环境", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        finally
        {
            _trainBusy = false;
            SetTrainingUi(false);
        }
    }

    private async Task StartTrainingAsync()
    {
        if (_trainBusy) return;
        if (_store.BoxCount < 3)
        {
            MessageBox.Show(this, "至少标注 3 个框再开始训练。\n\n到“学习训练”页，打开图片后在图上拖动框选目标即可。",
                "训练数据不足", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        if (!File.Exists(AppPaths.YoloExe))
        {
            MessageBox.Show(this, "还没有训练环境，请先点“初始化环境”。", "训练环境",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        if (_store.PendingBoxCount > 0 &&
            MessageBox.Show(this,
                $"还有 {_store.PendingBoxCount} 个自动框没复核，它们不会进训练集。\n" +
                "建议先点“复核”逐张确认；也可以带着它们继续训练。\n\n继续训练？",
                "有待复核的自动框", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

        int epochs = (int)_numEpochs.Value;
        int imgsz = int.TryParse(_cboImgSize.Text, out var s) ? s : 640;
        int batch = (int)_numBatch.Value;

        if (MessageBox.Show(this,
                $"将用 {_store.ImageCount} 张已有标注的图片（{_store.BoxCount} 个框）训练 {epochs} 轮（CPU）。\n" +
                "训练期间可以继续看图，但不要关闭程序；预计 5~20 分钟。\n\n现在开始？",
                "开始训练", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

        _trainBusy = true;
        _txtTrainLog.Clear();
        _lblTrainStage.Text = "正在准备训练集…";
        SetTrainingUi(true);
        try
        {
            var set = _store.BuildTrainingView();
            _lblTrainStage.Text = $"训练集：{set.TrainCount} 张训练 / {set.ValCount} 张验证 · {set.ClassCount} 类";
            AppendTrainLog($"类别：{string.Join(", ", set.ClassNames)}");

            var outcome = await _training.TrainAsync(set, epochs, imgsz, batch);
            AppendTrainLog(outcome.Message);
            if (!outcome.Ok)
            {
                _lblTrainStage.Text = "训练未完成。";
                if (!outcome.Message.Contains("取消"))
                    MessageBox.Show(this, outcome.Message, "训练未完成", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var opt = _settings.ToModelOptions() with
            {
                ModelPath = outcome.OnnxPath,
                Kind = YoloModelKind.YOLOv8Det,
                Device = DeviceType.CPU,
                Provider = OnnxRuntimeDeviceType.Default,
            };
            var fresh = new InferenceService();
            var info = fresh.Load(opt);
            fresh.Warmup();
            var old = _svc;
            _svc = fresh;
            old?.Dispose();
            _settings.Apply(opt);
            _settings.Save();
            RefreshAdvancedFromSettings();
            UpdateModelStatus(true, $"CPU · {info.ClassCount} 类", "已切换到新训练的模型");
            UpdateRunEnabled();
            _lblTrainStage.Text = "训练完成，新模型已加载。";
            MessageBox.Show(this,
                $"训练完成！\n\n新模型：{outcome.OnnxPath}\n类别数：{info.ClassCount}\n已自动切换为当前模型，可以回“识别测试”页试效果。",
                "训练完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            AppendTrainLog("ERROR: " + ex);
            _lblTrainStage.Text = "训练异常：" + ex.Message;
            MessageBox.Show(this, "训练异常：\n" + ex.Message, "训练", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        finally
        {
            _trainBusy = false;
            SetTrainingUi(false);
        }
    }

    private void SetTrainingUi(bool running)
    {
        _btnInitEnv.Enabled = !running;
        _btnTrain.Enabled = !running;
        _btnCancelTrain.Enabled = running;
        _numEpochs.Enabled = !running;
        _numBatch.Enabled = !running;
        _cboImgSize.Enabled = !running;
        if (running)
        {
            _pbTrain.Style = ProgressBarStyle.Marquee;
            _pbTrain.MarqueeAnimationSpeed = 40;
        }
        else
        {
            _pbTrain.Style = ProgressBarStyle.Blocks;
            _pbTrain.MarqueeAnimationSpeed = 0;
            _pbTrain.Value = 0;
        }
        UpdateRunEnabled();
    }

    private void AppendTrainLog(string line)
    {
        if (IsDisposed || string.IsNullOrEmpty(line)) return;
        _txtTrainLog.AppendText(line + Environment.NewLine);
        if (_txtTrainLog.Lines.Length > 4000)
        {
            var arr = _txtTrainLog.Lines;
            _txtTrainLog.Lines = arr.Skip(arr.Length - 2000).ToArray();
        }
        _txtTrainLog.SelectionStart = _txtTrainLog.TextLength;
        _txtTrainLog.ScrollToCaret();
    }

    // ---------------------------------------------------------------- misc

    private void BeginInvokeSafe(Action action)
    {
        if (IsDisposed || !IsHandleCreated) return;
        try
        {
            BeginInvoke(action);
        }
        catch (InvalidOperationException)
        {
        }
    }

    private void OnFormClosing(object sender, FormClosingEventArgs e)
    {
        if (_training.IsRunning)
        {
            if (MessageBox.Show(this, "训练仍在进行，确定要退出吗？", "退出",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
            {
                e.Cancel = true;
                return;
            }
            _training.Cancel();
        }
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        ShutdownVideo();
        _svc?.Dispose();
        _recBitmap?.Dispose();
        _learnBitmap?.Dispose();
        base.OnFormClosed(e);
    }

    private static string ProviderLabel(OnnxRuntimeDeviceType p) =>
        p == OnnxRuntimeDeviceType.Default ? "CPU" : p.ToString();

    private static string TrimText(string s, int max)
    {
        if (string.IsNullOrEmpty(s)) return "";
        s = s.Replace("\r", " ").Replace("\n", " ");
        return s.Length <= max ? s : s.Substring(0, max) + "…";
    }

    private static decimal Clamp(decimal v, decimal min, decimal max) =>
        v < min ? min : v > max ? max : v;
}
