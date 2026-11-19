using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
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
    // ---------------- 检测范围 + 判定页 ----------------
    private Control BuildAnchorZoneTab()
    {
        // 侧栏高度有限：整体可滚动，行高全部固定（AutoSize 行会互相压盖）。
        const int baseHeight = 992;
        var host = new Panel { Dock = DockStyle.Fill, BackColor = Theme.Bg, AutoScroll = true };
        var root = new TableLayoutPanel { Dock = DockStyle.Top, Height = baseHeight, ColumnCount = 1, RowCount = 10, BackColor = Theme.Bg };
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
        for (var i = 0; i < 10; i++) root.RowStyles.Add(new RowStyle(SizeType.Absolute, 40f));
        root.RowStyles[0] = new RowStyle(SizeType.Absolute, 88f);   // 按钮条（两行，放得下 6 个按钮）
        root.RowStyles[1] = new RowStyle(SizeType.Absolute, 132f);  // 范围列表
        root.RowStyles[2] = new RowStyle(SizeType.Absolute, 88f);   // 参考图预览
        root.RowStyles[3] = new RowStyle(SizeType.Absolute, 214f);  // 范围参数（每项下面带一行说明）
        root.RowStyles[4] = new RowStyle(SizeType.Absolute, 32f);   // 详细参数开关
        root.RowStyles[5] = new RowStyle(SizeType.Absolute, 0f);    // 判定容差（默认折叠）
        root.RowStyles[6] = new RowStyle(SizeType.Absolute, 88f);   // 拍基准 / 判定一次
        root.RowStyles[7] = new RowStyle(SizeType.Absolute, 160f);  // 锚点对照（拉高，列能看全）
        root.RowStyles[8] = new RowStyle(SizeType.Absolute, 112f);  // 最近判定
        root.RowStyles[9] = new RowStyle(SizeType.Absolute, 84f);   // 状态信息

        var bar = new FlowLayoutPanel { Dock = DockStyle.Fill, WrapContents = true, BackColor = Theme.Bg, Margin = new Padding(0, 0, 0, 6) };
        _btnAnchorZonePick = Theme.Primary("框选新范围");
        _btnAnchorZonePick.Padding = new Padding(8, 2, 8, 2);
        _btnAnchorZonePick.Click += (_, __) => BeginAnchorZonePick();
        var fromLabel = Theme.Success("按标签新建");
        fromLabel.Padding = new Padding(8, 2, 8, 2);
        fromLabel.Click += (_, __) => ShowAnchorZoneClassMenu(fromLabel);
        var capture = Theme.Ghost("采集参考");
        capture.Padding = new Padding(8, 2, 8, 2);
        capture.Click += (_, __) => CaptureAnchorZoneRef();
        _btnAnchorZoneEdit = Theme.Ghost("调整范围");
        _btnAnchorZoneEdit.Padding = new Padding(8, 2, 8, 2);
        _btnAnchorZoneEdit.Click += (_, __) => BeginAnchorZoneEdit();
        var rename = Theme.Ghost("改名");
        rename.Padding = new Padding(8, 2, 8, 2);
        rename.Click += (_, __) => RenameAnchorZone();
        var remove = Theme.Ghost("删除");
        remove.Padding = new Padding(8, 2, 8, 2);
        remove.Click += (_, __) => DeleteAnchorZone();
        bar.Controls.Add(_btnAnchorZonePick);
        bar.Controls.Add(fromLabel);
        bar.Controls.Add(capture);
        bar.Controls.Add(_btnAnchorZoneEdit);
        bar.Controls.Add(rename);
        bar.Controls.Add(remove);
        root.Controls.Add(bar, 0, 0);
        _anchorTip?.SetToolTip(capture, "把当前画面里这块区域的外观存成模板（采集时板必须放好）。判定时用它比对，匹配率低于下限或位置偏太多就判 NG。");
        _anchorTip?.SetToolTip(fromLabel, "从已经框好的标签里挑一个（例如 p1），直接用那个框的范围建检测范围。");
        _anchorTip?.SetToolTip(_btnAnchorZonePick, "在画面上拖出新范围，右键或回车确认；再点一次取消。");
        _anchorTip?.SetToolTip(_btnAnchorZoneEdit, "重新调这个范围的位置/大小/角度：拖框移动、拖角点改大小、拖顶部圆点旋转，右键保存（不保存不生效），Esc 放弃。");
        _anchorTip?.SetToolTip(rename, "给列表里选中的范围改名（参考图会一起改名，不会丢）。也可以右键列表或按 F2。");
        _anchorTip?.SetToolTip(remove, "删除列表里选中的范围及其参考图。也可以右键列表或按 Delete。");

        _lvAnchorZones = new ListView
        {
            Dock = DockStyle.Fill,
            View = View.Details,
            FullRowSelect = true,
            MultiSelect = false,
            HideSelection = false,
            HeaderStyle = ColumnHeaderStyle.Nonclickable,
            Font = Theme.Small,
            BackColor = Theme.Card,
            BorderStyle = BorderStyle.FixedSingle,
            Margin = new Padding(0, 0, 0, 6),
        };
        _lvAnchorZones.Columns.Add("范围", 98);
        _lvAnchorZones.Columns.Add("标签", 68);
        _lvAnchorZones.Columns.Add("匹配率", 62);
        _lvAnchorZones.Columns.Add("容许", 52);
        _lvAnchorZones.Columns.Add("状态", 84);
        _lvAnchorZones.SelectedIndexChanged += (_, __) =>
        {
            LoadZoneUiFromSelected();
            RefreshZoneRefPreview();
        };
        _lvAnchorZones.KeyDown += (_, e) =>
        {
            if (e.KeyCode == Keys.Delete)
            {
                DeleteAnchorZone();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.F2)
            {
                RenameAnchorZone();
                e.Handled = true;
            }
        };
        _lvAnchorZones.MouseUp += (_, e) =>
        {
            if (e.Button != MouseButtons.Right) return;
            var hit = _lvAnchorZones.GetItemAt(e.X, e.Y);
            if (hit != null)
            {
                hit.Selected = true;
                hit.Focused = true;
            }
            ShowZoneMenu();
        };
        root.Controls.Add(_lvAnchorZones, 0, 1);

        // 参考图预览：点「采集参考」后立刻能看到存下来的模板长什么样。
        var refRow = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 1, BackColor = Theme.Bg, Margin = new Padding(0, 0, 0, 6) };
        refRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 104f));
        refRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
        _picZoneRef = new PictureBox
        {
            Dock = DockStyle.Fill,
            SizeMode = PictureBoxSizeMode.Zoom,
            BackColor = Theme.Card,
            BorderStyle = BorderStyle.FixedSingle,
            Margin = new Padding(0, 0, 8, 0),
        };
        _lblZoneRefInfo = new Label
        {
            Dock = DockStyle.Fill,
            AutoSize = false,
            Font = Theme.Small,
            ForeColor = Theme.Muted,
            TextAlign = ContentAlignment.MiddleLeft,
            Margin = Padding.Empty,
            Text = "参考图：选中一个检测范围后显示",
        };
        refRow.Controls.Add(_picZoneRef, 0, 0);
        refRow.Controls.Add(_lblZoneRefInfo, 1, 0);
        root.Controls.Add(refRow, 0, 2);

        var grid = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, BackColor = Theme.Bg, Margin = new Padding(0, 0, 0, 6) };
        grid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 96f));
        grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
        for (var i = 0; i < 9; i++) grid.RowStyles.Add(new RowStyle(SizeType.Absolute, i % 2 == 0 ? 26f : 20f));
        grid.RowStyles[8] = new RowStyle(SizeType.Absolute, 30f);
        _numZoneMatch = ZoneNum(50m, 99.9m, 90m, 0.5m, 1);
        _numZoneSearch = ZoneNum(0m, 160m, 24m, 2m, 0);
        _numZoneShift = ZoneNum(0m, 120m, 12m, 1m, 0);
        _cboZoneClass = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Font = Theme.Body, Dock = DockStyle.Fill };
        _chkZoneRequired = new CheckBox { Text = "必须通过（不通过即判 NG）", AutoSize = true, Checked = true, Font = Theme.Body, ForeColor = Theme.Ink };
        AddZoneParamRow(grid, 0, "匹配率≥", _numZoneMatch, "和参考图要多像才算通过：调高更严，调低更松。");
        AddZoneParamRow(grid, 1, "搜索范围", _numZoneSearch, "在区域四周找参考的余量（px），太小易 NG。");
        AddZoneParamRow(grid, 2, "容许位移", _numZoneShift, "找到的位置最多允许偏多少像素。");
        AddZoneParamRow(grid, 3, "绑定标签", _cboZoneClass, "可选：这里必须检出某个目标（如 p1）。");
        grid.Controls.Add(_chkZoneRequired, 1, 8);
        root.Controls.Add(grid, 0, 3);
        _anchorTip?.SetToolTip(_cboZoneClass, "给这个范围备注它对应哪个识别目标（例如 p1）；留空只做模板匹配。");

        _chkAnchorAdv = new CheckBox
        {
            Text = "详细参数（判定容差）",
            AutoSize = true,
            Font = Theme.Body,
            ForeColor = Theme.Ink,
            Margin = new Padding(0, 4, 0, 4),
        };
        _chkAnchorAdv.CheckedChanged += (_, __) =>
        {
            var show = _chkAnchorAdv.Checked;
            if (_pnlAnchorTol != null) _pnlAnchorTol.Visible = show;
            root.RowStyles[5] = new RowStyle(SizeType.Absolute, show ? 186f : 0f);
            root.Height = baseHeight + (show ? 186 : 0);
        };
        root.Controls.Add(_chkAnchorAdv, 0, 4);

        _pnlAnchorTol = new Panel { Dock = DockStyle.Fill, BackColor = Theme.Bg, Visible = false, Margin = new Padding(0, 0, 0, 6) };
        var tolGrid = BuildAnchorToleranceGrid();
        tolGrid.Dock = DockStyle.Top;
        tolGrid.AutoSize = true;
        _pnlAnchorTol.Controls.Add(tolGrid);
        root.Controls.Add(_pnlAnchorTol, 0, 5);

        var judgeBar = new FlowLayoutPanel { Dock = DockStyle.Fill, WrapContents = true, BackColor = Theme.Bg, Margin = new Padding(0, 0, 0, 6) };
        // 「拍基准」已挪到第三页（绿色按钮），「判定一次」在页面顶部相机行，这里只留容差。
        var saveTol = Theme.Ghost("保存容差");
        saveTol.Padding = new Padding(8, 2, 8, 2);
        saveTol.Click += (_, __) => SaveAnchorTolerance();
        var overlay = Theme.Ghost("叠加：开");
        overlay.Padding = new Padding(8, 2, 8, 2);
        overlay.Click += (_, __) =>
        {
            _anchorOverlayOn = !_anchorOverlayOn;
            overlay.Text = _anchorOverlayOn ? "叠加：开" : "叠加：关";
            if (_anchorRaw != null && !_anchorRaw.Empty()) ShowAnchorFrame(_anchorRaw, _anchorLastVerdict);
        };
        judgeBar.Controls.Add(saveTol);
        judgeBar.Controls.Add(overlay);
        root.Controls.Add(judgeBar, 0, 6);

        _lvAnchorHits = new ListView
        {
            Dock = DockStyle.Fill,
            View = View.Details,
            FullRowSelect = true,
            MultiSelect = false,
            HideSelection = false,
            HeaderStyle = ColumnHeaderStyle.Nonclickable,
            Font = Theme.Small,
            BackColor = Theme.Card,
            BorderStyle = BorderStyle.None,
            Margin = Padding.Empty,
        };
        _lvAnchorHits.Columns.Add("锚点", 66);
        _lvAnchorHits.Columns.Add("基准", 76);
        _lvAnchorHits.Columns.Add("实测", 76);
        _lvAnchorHits.Columns.Add("偏差", 78);
        _lvAnchorHits.Columns.Add("置信", 50);
        var hitsCard = AnchorCard("锚点对照（拍基准后才有）", _lvAnchorHits, DockStyle.Fill);
        root.Controls.Add(hitsCard, 0, 7);

        _lvAnchorLog = new ListView
        {
            Dock = DockStyle.Fill,
            View = View.Details,
            FullRowSelect = true,
            MultiSelect = false,
            HideSelection = false,
            HeaderStyle = ColumnHeaderStyle.Nonclickable,
            Font = Theme.Small,
            BackColor = Theme.Card,
            BorderStyle = BorderStyle.None,
            Margin = Padding.Empty,
        };
        _lvAnchorLog.Columns.Add("时间", 66);
        _lvAnchorLog.Columns.Add("结论", 52);
        _lvAnchorLog.Columns.Add("原因", 108);
        _lvAnchorLog.Columns.Add("偏差", 84);
        var logCard = AnchorCard("最近判定", _lvAnchorLog, DockStyle.Fill);
        root.Controls.Add(logCard, 0, 8);

        var hints = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 3, BackColor = Theme.Bg, Margin = Padding.Empty };
        hints.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
        hints.RowStyles.Add(new RowStyle(SizeType.Absolute, 30f));
        hints.RowStyles.Add(new RowStyle(SizeType.Absolute, 26f));
        hints.RowStyles.Add(new RowStyle(SizeType.Absolute, 26f));
        _lblAnchorZoneInfo = Theme.Hint("还没有检测范围");
        _lblAnchorZoneInfo.AutoSize = false;
        _lblAnchorZoneInfo.Dock = DockStyle.Fill;
        _lblAnchorZoneInfo.Margin = Padding.Empty;
        _lblAnchorCalibInfo = new Label
        {
            Text = "基准：还没有拍",
            AutoSize = false,
            Dock = DockStyle.Fill,
            Font = Theme.Small,
            ForeColor = Theme.Muted,
            Margin = Padding.Empty,
        };
        _lblAnchorJitter = new Label
        {
            Text = "",
            AutoSize = false,
            Dock = DockStyle.Fill,
            Font = Theme.Small,
            ForeColor = Theme.Muted,
            Margin = Padding.Empty,
        };
        hints.Controls.Add(_lblAnchorZoneInfo, 0, 0);
        hints.Controls.Add(_lblAnchorCalibInfo, 0, 1);
        hints.Controls.Add(_lblAnchorJitter, 0, 2);
        root.Controls.Add(hints, 0, 9);

        _numZoneMatch.ValueChanged += OnZoneParamChanged;
        _numZoneSearch.ValueChanged += OnZoneParamChanged;
        _numZoneShift.ValueChanged += OnZoneParamChanged;
        _cboZoneClass.SelectedIndexChanged += OnZoneParamChanged;
        _chkZoneRequired.CheckedChanged += OnZoneParamChanged;

        root.VisibleChanged += (_, __) =>
        {
            if (!root.Visible) return;
            // 切到本页只做刷新：框选必须点“框选新范围”才开始，避免一拖就冒出范围。
            CancelAnchorPicking();
            RefreshZoneClassCombo();
            ShowAnchorZones();
            RefreshZoneRefPreview();
        };
        host.Controls.Add(root);
        return host;
    }

    private Control BuildAnchorTargetTab()
    {
        var root = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 5, BackColor = Theme.Bg };
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 42f));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 58f));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        var bar = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, WrapContents = true, Margin = new Padding(0, 0, 0, 6) };
        var pick = Theme.Primary("框选新目标");
        pick.AutoSize = true;
        pick.Padding = new Padding(8, 2, 8, 2);
        pick.Click += (_, __) => BeginAnchorTargetPick();
        var train = Theme.Success("训练并部署");
        train.AutoSize = true;
        train.Padding = new Padding(8, 2, 8, 2);
        train.Click += async (_, __) => await TrainAnchorAsync();
        var refresh = Theme.Ghost("刷新");
        refresh.AutoSize = true;
        refresh.Padding = new Padding(8, 2, 8, 2);
        refresh.Click += (_, __) => ShowAnchorTargets();
        bar.Controls.Add(pick);
        bar.Controls.Add(train);
        bar.Controls.Add(refresh);
        root.Controls.Add(bar, 0, 0);

        var row = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, WrapContents = true, Margin = new Padding(0, 0, 0, 6) };
        row.Controls.Add(new Label { Text = "轮数", AutoSize = true, Font = Theme.Body, ForeColor = Theme.Muted, Margin = new Padding(0, 7, 4, 0) });
        _numTrainEpochs = ZoneNum(5m, 300m, 60m, 5m, 0);
        _numTrainEpochs.Width = 64;
        row.Controls.Add(_numTrainEpochs);
        row.Controls.Add(new Label { Text = "尺寸", AutoSize = true, Font = Theme.Body, ForeColor = Theme.Muted, Margin = new Padding(12, 7, 4, 0) });
        _numTrainImgsz = ZoneNum(320m, 1280m, 640m, 64m, 0);
        _numTrainImgsz.Width = 70;
        row.Controls.Add(_numTrainImgsz);
        root.Controls.Add(row, 0, 1);

        _lvAnchorTargets = new ListView
        {
            Dock = DockStyle.Fill,
            View = View.Details,
            FullRowSelect = true,
            MultiSelect = false,
            HideSelection = false,
            HeaderStyle = ColumnHeaderStyle.Nonclickable,
            Font = Theme.Small,
            BackColor = Theme.Card,
            BorderStyle = BorderStyle.FixedSingle,
            Margin = new Padding(0, 0, 0, 6),
        };
        _lvAnchorTargets.Columns.Add("目标", 110);
        _lvAnchorTargets.Columns.Add("样本", 56);
        _lvAnchorTargets.Columns.Add("被区域引用", 90);
        root.Controls.Add(_lvAnchorTargets, 0, 2);

        _txtAnchorTrainLog = new TextBox
        {
            Dock = DockStyle.Fill,
            Multiline = true,
            ReadOnly = true,
            ScrollBars = ScrollBars.Vertical,
            WordWrap = false,
            Font = Theme.Mono,
            BackColor = Theme.Card,
            BorderStyle = BorderStyle.FixedSingle,
            Margin = new Padding(0, 0, 0, 6),
        };
        root.Controls.Add(_txtAnchorTrainLog, 0, 3);

        _lblAnchorTargetInfo = Theme.Hint("每换一次位置/光照就框一次，同一目标采 60 张以上再训练。");
        _lblAnchorTargetInfo.Dock = DockStyle.Fill;
        root.Controls.Add(_lblAnchorTargetInfo, 0, 4);

        root.VisibleChanged += (_, __) =>
        {
            if (!root.Visible) return;
            ShowAnchorTargets();
        };
        return root;
    }

    private static NumericUpDown ZoneNum(decimal min, decimal max, decimal value, decimal step, int digits) =>
        new NoWheelNumeric()
        {
            Minimum = min,
            Maximum = max,
            Value = Math.Min(max, Math.Max(min, value)),
            Increment = step,
            DecimalPlaces = digits,
            Font = Theme.Body,
            Width = 96,
            TextAlign = HorizontalAlignment.Right,
            Margin = new Padding(0, 4, 0, 4),
        };

    /// <summary>一行“参数名 + 控件”，下面紧跟一行灰色说明（用户不用猜这个参数管什么）。</summary>
    private static void AddZoneParamRow(TableLayoutPanel grid, int row, string label, Control editor, string hint)
    {
        var head = new Label
        {
            Text = label,
            AutoSize = false,
            Dock = DockStyle.Fill,
            Font = Theme.Body,
            ForeColor = Theme.Muted,
            TextAlign = ContentAlignment.MiddleLeft,
            AutoEllipsis = true,
            Margin = Padding.Empty,
        };
        grid.Controls.Add(head, 0, row * 2);
        editor.Dock = DockStyle.Fill;
        editor.Margin = new Padding(0, 1, 0, 1);
        grid.Controls.Add(editor, 1, row * 2);

        var tip = new Label
        {
            Text = hint,
            AutoSize = false,
            Dock = DockStyle.Fill,
            Font = Theme.Small,
            ForeColor = Theme.Muted,
            TextAlign = ContentAlignment.TopLeft,
            AutoEllipsis = true,
            Margin = Padding.Empty,
        };
        grid.Controls.Add(tip, 0, row * 2 + 1);
        grid.SetColumnSpan(tip, 2);
    }

    // ---------------- 调整已有范围（拖动/缩放/旋转 → 右键保存） ----------------
    private bool _zoneEditing;
    private AnchorZone _zoneEditZone;
    private Rectangle _zoneEditRect;
    private double _zoneEditAngle;
    private Mat _zoneEditFrame;

    /// <summary>进入范围微调：冻结当前画面，只画这一个框；改完右键保存（不保存不生效）。</summary>
    private void BeginAnchorZoneEdit()
    {
        if (_zoneEditing)
        {
            CancelAnchorZoneEdit("已取消调整，范围没有改动。");
            return;
        }
        var profile = _anchorProfile;
        var zone = SelectedAnchorZone();
        if (profile == null)
        {
            HintZone("还没有检测范围。");
            return;
        }
        if (zone == null)
        {
            HintZone("先在上面的列表里点中一个检测范围，再点「调整范围」。");
            return;
        }
        if (_anchorRaw == null || _anchorRaw.Empty())
        {
            HintZone("先打开相机，有画面后再调整范围。");
            return;
        }
        if (_anchorEditFrame != null) ExitAnchorEditMode();
        CancelAnchorPicking();

        _zoneEditing = true;
        _zoneEditZone = zone;
        _zoneEditRect = new Rectangle(zone.X, zone.Y, zone.W, zone.H);
        _zoneEditAngle = zone.Angle;
        _zoneEditFrame?.Dispose();
        _zoneEditFrame = _anchorRaw.Clone();
        ShowMatFrame(_zoneEditFrame);
        _picAnchor.Editable = true;
        _picAnchor.RightClickAction = SaveZoneEditByRightClick;
        SyncZoneEditBoxes();
        _picAnchor.Focus();
        if (_btnAnchorZoneEdit != null) _btnAnchorZoneEdit.Text = "取消调整";
        SetZoneHint("正在调整「" + zone.Name + "」：拖动移动、拖角点改大小、拖顶部圆点旋转；右键保存，Esc 放弃。", Theme.Accent);
    }

    private void SyncZoneEditBoxes()
    {
        if (_picAnchor == null) return;
        _anchorDraftSync = true;
        try
        {
            _picAnchor.Boxes = new[]
            {
                new BoxRecord
                {
                    Cn = (_zoneEditZone == null ? "范围" : _zoneEditZone.Name) + "（未保存）",
                    En = "zone",
                    X = _zoneEditRect.X,
                    Y = _zoneEditRect.Y,
                    W = _zoneEditRect.Width,
                    H = _zoneEditRect.Height,
                    Angle = (float)_zoneEditAngle,
                },
            };
            _picAnchor.SelectedIndex = 0;
        }
        finally
        {
            _anchorDraftSync = false;
        }
    }

    /// <summary>微调时的框变化只记在内存里，等右键保存。</summary>
    private void ApplyZoneEditBox(BoxEdit edit)
    {
        if (!_zoneEditing || edit == null) return;
        var fw = _zoneEditFrame?.Width ?? 0;
        var fh = _zoneEditFrame?.Height ?? 0;
        var rect = edit.Bounds;
        if (fw > 0 && fh > 0) rect = Rectangle.Intersect(rect, new Rectangle(0, 0, fw, fh));
        if (rect.Width < 12 || rect.Height < 12) return;
        _zoneEditRect = rect;
        _zoneEditAngle = edit.Angle;
        SyncZoneEditBoxes();
        var angle = Math.Abs(_zoneEditAngle) > 0.05 ? "，旋转 " + _zoneEditAngle.ToString("0.0") + "°" : "";
        SetZoneHint("「" + (_zoneEditZone?.Name ?? "范围") + "」改到 " + rect.Width + "×" + rect.Height + "px" + angle + "：右键保存生效，Esc 放弃。", Theme.Accent);
    }

    private bool SaveZoneEditByRightClick()
    {
        if (!_zoneEditing) return false;
        SaveZoneEdit();
        return true;
    }

    private void SaveZoneEdit()
    {
        var zone = _zoneEditZone;
        var profile = _anchorProfile;
        if (!_zoneEditing || zone == null || profile == null) return;

        var rect = _zoneEditRect;
        var angle = _zoneEditAngle;
        var changed = rect.X != zone.X || rect.Y != zone.Y || rect.Width != zone.W || rect.Height != zone.H
                      || Math.Abs(angle - zone.Angle) > 0.05;

        zone.X = rect.X;
        zone.Y = rect.Y;
        zone.W = rect.Width;
        zone.H = rect.Height;
        zone.Angle = angle;
        zone.Normalize();

        // 范围变了就用当前这一帧重采模板：位置挪了还拿旧模板比，匹配率会无端下降。
        var relabel = false;
        if (changed && _zoneEditFrame != null && !_zoneEditFrame.Empty()
            && AnchorZoneStore.SaveTemplate(_anchorModel, zone.Name, _zoneEditFrame, zone.X, zone.Y, zone.W, zone.H, zone.Angle))
        {
            zone.RefAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            relabel = true;
        }

        AnchorStore.Save(profile);
        _anchorZoneReport = null;
        EndZoneEdit();
        ShowAnchorZones();
        SelectAnchorZone(zone);
        RefreshZoneRefPreview();
        if (_anchorRaw != null && !_anchorRaw.Empty()) ShowAnchorFrame(_anchorRaw, _anchorLastVerdict);
        var angleText = Math.Abs(zone.Angle) > 0.05 ? "，旋转 " + zone.Angle.ToString("0.0") + "°" : "";
        SetZoneHint("已保存「" + zone.Name + "」：" + zone.W + "×" + zone.H + "px" + angleText
                    + (relabel ? "，参考图一起更新了" : "（范围没变，参考图不动）") + "。", Theme.Ok);
    }

    private void CancelAnchorZoneEdit(string message)
    {
        if (!_zoneEditing) return;
        EndZoneEdit();
        if (_anchorRaw != null && !_anchorRaw.Empty()) ShowAnchorFrame(_anchorRaw, _anchorLastVerdict);
        if (!string.IsNullOrEmpty(message)) SetZoneHint(message, Theme.Muted);
    }

    private void EndZoneEdit()
    {
        _zoneEditing = false;
        _zoneEditZone = null;
        if (_picAnchor != null)
        {
            _picAnchor.RightClickAction = null;
            _picAnchor.Editable = false;
            _anchorDraftSync = true;
            try
            {
                _picAnchor.Boxes = Array.Empty<BoxRecord>();
                _picAnchor.SelectedIndex = -1;
            }
            finally
            {
                _anchorDraftSync = false;
            }
        }
        _zoneEditFrame?.Dispose();
        _zoneEditFrame = null;
        if (_btnAnchorZoneEdit != null) _btnAnchorZoneEdit.Text = "调整范围";
    }

    private void SetZoneHint(string text, Color color)
    {
        if (_lblAnchorZoneInfo == null) return;
        _lblAnchorZoneInfo.Text = text;
        _lblAnchorZoneInfo.ForeColor = color;
    }

    // ---------------- 框选 ----------------
    private void BeginAnchorZonePick()
    {
        if (_anchorProfile == null) return;
        if (_anchorZonePicking)
        {
            CancelAnchorPicking();
            SetAnchorCamInfo("已取消框选。");
            return;
        }
        if (_picAnchor.Image == null)
        {
            SetAnchorCamInfo("先打开相机，有画面后再框。");
            return;
        }
        ExitAnchorEditMode();
        _anchorTargetPicking = false;
        _anchorZonePicking = true;
        _picAnchor.Editable = true;
        _picAnchor.Focus();
        if (_btnAnchorZonePick != null) _btnAnchorZonePick.Text = "取消框选";
        SetAnchorCamInfo("正在框选范围：拖出要检测的区域（例如 wifi 丝印），右键或回车确认；Esc 或再点一次按钮取消。");
    }

    /// <summary>退出所有框选状态：画布编辑态、待确认框、按钮文字一起复位。</summary>
    private void CancelAnchorPicking()
    {
        _anchorZonePicking = false;
        _anchorTargetPicking = false;
        if (_picAnchor != null)
        {
            _picAnchor.Editable = false;
            _picAnchor.ClearPendingBox();
        }
        if (_btnAnchorZonePick != null) _btnAnchorZonePick.Text = "框选新范围";
    }

    private void BeginAnchorTargetPick()
    {
        if (_anchorProfile == null) return;
        if (_anchorEditFrame == null || _anchorEditFrame.Empty() || string.IsNullOrEmpty(_anchorSampleStem))
        {
            SetAnchorCamInfo("先在页面上方点【拍照】存一张照片，再点“框选目标”。");
            return;
        }
        _anchorZonePicking = false;
        _anchorTargetPicking = true;
        _picAnchor.Editable = true;
        SyncDraftBoxes(_picAnchor.SelectedIndex);
        ShowAnchorDrafts();
        _picAnchor.Focus();
        SetAnchorCamInfo("在照片上拖动框选目标，右键或回车确认；一张照片可连续框多个。");
    }

    private void OnAnchorBoxDrawn(object sender, BoxDraft draft)
    {
        if (draft == null) return;
        if (_anchorZonePicking)
        {
            _anchorZonePicking = false;
            _picAnchor.Editable = false;
            _picAnchor.ClearPendingBox();
            CreateAnchorZone(draft.Bounds, draft.Angle);
            return;
        }
        if (_anchorTargetPicking)
        {
            _picAnchor.ClearPendingBox();
            _picAnchor.Editable = true;
            AddAnchorTargetDraft(draft);
        }
    }

    /// <summary>“按标签新建”：先弹菜单选标签，再按该标签的样本框建范围。</summary>
    private void ShowAnchorZoneClassMenu(Control anchor)
    {
        CancelAnchorPicking();
        var names = new List<string>();
        if (!string.IsNullOrEmpty(_anchorModel))
        {
            try
            {
                foreach (var cls in new AnchorSampleStore(_anchorModel).ReadClasses())
                    if (!string.IsNullOrWhiteSpace(cls) && !names.Contains(cls, StringComparer.OrdinalIgnoreCase))
                        names.Add(cls);
            }
            catch { }
        }
        var modelNames = _anchorSvc?.Info?.ClassNames;
        if (modelNames != null)
        {
            foreach (var cls in modelNames)
                if (!string.IsNullOrWhiteSpace(cls) && !names.Contains(cls, StringComparer.OrdinalIgnoreCase))
                    names.Add(cls);
        }
        if (names.Count == 0)
        {
            SetAnchorCamInfo("还没有可选的标签：先到「① 识别目标」拍照并框选一个特征（例如 p1）。");
            return;
        }

        var menu = new ContextMenuStrip { Font = Theme.Body };
        foreach (var name in names)
        {
            var item = new ToolStripMenuItem(name);
            item.Click += (_, __) => CreateAnchorZoneFromClass(name);
            menu.Items.Add(item);
        }
        menu.Show(anchor, new Point(0, anchor.Height));
    }

    /// <summary>从已标注样本里挑一个标签的框，按它建检测范围，参考图就用那张照片。</summary>
    private void CreateAnchorZoneFromClass(string cls)
    {
        var profile = _anchorProfile;
        if (profile == null) return;
        CancelAnchorPicking();
        if (string.IsNullOrWhiteSpace(cls))
        {
            SetAnchorCamInfo("没有选标签：点“按标签新建”后从菜单里选一个。");
            return;
        }

        var store = new AnchorSampleStore(_anchorModel);
        List<AnchorSampleEntry> list;
        try { list = store.ListSamples(400); }
        catch (Exception ex) { SetAnchorCamInfo("读取样本失败：" + ex.Message); return; }

        var candidates = list
            .Where(s => s.BoxCount > 0 && s.Classes.Split('、').Any(n => string.Equals(n, cls, StringComparison.OrdinalIgnoreCase)))
            .ToList();
        if (!string.IsNullOrEmpty(_anchorSampleStem))
        {
            var current = candidates.FirstOrDefault(s => string.Equals(s.Stem, _anchorSampleStem, StringComparison.OrdinalIgnoreCase));
            if (current != null)
            {
                candidates.Remove(current);
                candidates.Insert(0, current);
            }
        }
        if (candidates.Count == 0)
        {
            SetAnchorCamInfo("还没有给“" + cls + "”框过样本：先到第 1 步拍一张照片并框选。");
            return;
        }

        var pick = candidates[0];
        List<AnchorLabelBox> boxes;
        int width, height;
        using (var img = Cv2.ImRead(pick.ImagePath))
        {
            if (img == null || img.Empty()) { SetAnchorCamInfo("读不到样本图：" + pick.Stem); return; }
            width = img.Width;
            height = img.Height;
            boxes = store.ReadLabels(pick.Stem, width, height)
                .Where(b => string.Equals(b.ClassName, cls, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
        var box = boxes.OrderByDescending(b => (long)b.Rect.Width * b.Rect.Height).FirstOrDefault();
        if (box == null) { SetAnchorCamInfo("照片 " + pick.Stem + " 里没有“" + cls + "”的框。"); return; }

        var padX = Math.Max(4, box.Rect.Width / 10);
        var padY = Math.Max(4, box.Rect.Height / 10);
        var rect = Rectangle.Intersect(
            new Rectangle(box.Rect.X - padX, box.Rect.Y - padY, box.Rect.Width + padX * 2, box.Rect.Height + padY * 2),
            new Rectangle(0, 0, width, height));
        if (rect.Width < 12 || rect.Height < 12) { SetAnchorCamInfo("这个框太小了：至少 12×12 像素。"); return; }

        if (MessageBox.Show(this,
                "用照片 " + pick.Stem + " 里的“" + cls + "”框（" + rect.Width + "×" + rect.Height + "px）新建检测范围？\n参考图直接取这张照片，所以拍这张时板必须是放好的状态。",
                "按标签新建范围", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK) return;

        var name = cls + "区";
        for (var i = 2; profile.Zones.Any(z => z.Name == name) && i < 100; i++) name = cls + "区" + i;

        var zone = new AnchorZone
        {
            Name = name,
            X = rect.X,
            Y = rect.Y,
            W = rect.Width,
            H = rect.Height,
            MinMatch = (double)_numZoneMatch.Value / 100.0,
            SearchPx = (int)_numZoneSearch.Value,
            MaxShiftPx = (int)_numZoneShift.Value,
            Required = _chkZoneRequired.Checked,
            TargetClass = cls,
            Angle = box.Angle,
        };
        zone.Normalize();
        profile.Zones.Add(zone);

        using (var img = Cv2.ImRead(pick.ImagePath))
        {
            if (img != null && !img.Empty() && AnchorZoneStore.SaveTemplate(_anchorModel, zone.Name, img, zone.X, zone.Y, zone.W, zone.H, zone.Angle))
                zone.RefAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
        AnchorStore.Save(profile);
        ShowAnchorZones();
        SelectAnchorZone(zone);
        SetAnchorCamInfo(string.IsNullOrEmpty(zone.RefAt)
            ? "已新建 " + zone.Name + "，但参考图没存成；换一张照片再建。"
            : "已新建 " + zone.Name + " → 标签 " + cls + "，参考图 " + pick.Stem + "。判定时这块必须匹配 ≥ " + _numZoneMatch.Value.ToString("0.#") + "%。");
    }

    private void CreateAnchorZone(Rectangle bounds, float angle)
    {
        var profile = _anchorProfile;
        if (profile == null) return;
        CancelAnchorPicking();
        if (bounds.Width < 12 || bounds.Height < 12)
        {
            SetAnchorCamInfo("框太小了：至少 12×12 像素。");
            return;
        }

        var name = "区域";
        for (var i = 1; i <= 99; i++)
        {
            if (profile.Zones.Any(z => z.Name == "区域" + i)) continue;
            name = "区域" + i;
            break;
        }

        var zone = new AnchorZone
        {
            Name = name,
            X = bounds.X,
            Y = bounds.Y,
            W = bounds.Width,
            H = bounds.Height,
            MinMatch = (double)_numZoneMatch.Value / 100.0,
            SearchPx = (int)_numZoneSearch.Value,
            MaxShiftPx = (int)_numZoneShift.Value,
            Required = _chkZoneRequired.Checked,
            TargetClass = ZoneClassFromUi(),
            Angle = angle,
        };
        zone.Normalize();
        profile.Zones.Add(zone);
        AnchorStore.Save(profile);
        ShowAnchorZones();
        SelectAnchorZone(zone);
        if (_anchorRaw != null && !_anchorRaw.Empty() && AnchorZoneStore.SaveTemplate(_anchorModel, zone.Name, _anchorRaw, zone.X, zone.Y, zone.W, zone.H, zone.Angle))
        {
            zone.RefAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            AnchorStore.Save(profile);
            ShowAnchorZones();
        }
        SetAnchorCamInfo("已新建 " + zone.Name + "，并用当前画面存了参考。若这一刻板没放好，放好后点“采集参考”重取。");
    }

    private void CaptureAnchorZoneRef()
    {
        CancelAnchorPicking();
        var profile = _anchorProfile;
        var zone = SelectedAnchorZone();
        if (profile == null || zone == null)
        {
            SetAnchorCamInfo("先在上面的列表里选一个检测范围（没有就点“框选新范围”新建一个）。");
            return;
        }
        if (_anchorRaw == null || _anchorRaw.Empty())
        {
            SetAnchorCamInfo("还没有画面：先打开相机。");
            return;
        }
        if (!AnchorZoneStore.SaveTemplate(_anchorModel, zone.Name, _anchorRaw, zone.X, zone.Y, zone.W, zone.H, zone.Angle))
        {
            SetAnchorCamInfo("参考图保存失败：确认这个范围还完整落在画面内。");
            return;
        }
        zone.RefAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        ApplyZoneUiToSelected();
        AnchorStore.Save(profile);
        _anchorZoneReport = null;   // 参考换了，旧判定结果作废
        ShowAnchorZones();
        RefreshZoneRefPreview();
        _lblAnchorZoneInfo.Text = "参考已更新：" + zone.Name + " · " + zone.RefAt + " · 模板 " + zone.W + "×" + zone.H + "px";
        _lblAnchorZoneInfo.ForeColor = Theme.Ok;
        SetAnchorCamInfo("已采集参考：" + zone.Name + "（" + zone.W + "×" + zone.H + "px）。这一步只是把“放好的样子”存成模板，板放好时采一次就够。");
    }

    /// <summary>把选中范围的参考模板显示成缩略图 + 参数摘要，让“采集参考”有可见结果。</summary>
    private void RefreshZoneRefPreview()
    {
        if (_picZoneRef == null) return;
        var old = _picZoneRef?.Image;
        _picZoneRef.Image = null;
        old?.Dispose();

        var zone = SelectedAnchorZone();
        if (zone == null)
        {
            if (_lblZoneRefInfo != null)
                _lblZoneRefInfo.Text = "参考图：选中一个检测范围后显示。\n“采集参考”= 把当前画面里这块区域存成模板。";
            return;
        }
        var has = AnchorZoneStore.HasTemplate(_anchorModel, zone.Name);
        if (has)
        {
            using var tpl = AnchorZoneStore.LoadTemplate(_anchorModel, zone.Name);
            if (tpl != null && !tpl.Empty()) _picZoneRef.Image = MatToBitmap(tpl);
        }
        if (_lblZoneRefInfo != null)
        {
            _lblZoneRefInfo.Text =
                (has ? "参考图：" + zone.Name : "参考图：缺（点“采集参考”）") + "\n"
                + (string.IsNullOrEmpty(zone.RefAt) ? "还没采集过" : zone.RefAt) + " · " + zone.W + "×" + zone.H + "px\n"
                + "匹配率 ≥ " + (zone.MinMatch * 100).ToString("0.#") + "% · 容许位移 " + zone.MaxShiftPx + "px";
            _lblZoneRefInfo.ForeColor = has ? Theme.Muted : Theme.Danger;
        }
    }

    private void DeleteAnchorZone()
    {
        var profile = _anchorProfile;
        var zone = SelectedAnchorZone();
        if (profile == null)
        {
            HintZone("还没有检测范围。");
            return;
        }
        if (zone == null)
        {
            HintZone("请先在上面的列表里点中一个检测范围，再点「删除」。");
            return;
        }
        CancelAnchorPicking();
        if (MessageBox.Show(this, "删除检测范围“" + zone.Name + "”？", "检测范围", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK) return;
        var name = zone.Name;
        profile.Zones.RemoveAll(z => ReferenceEquals(z, zone) || z.Name == name);
        AnchorZoneStore.DeleteTemplate(_anchorModel, name);
        AnchorStore.Save(profile);
        _anchorZoneReport = null;
        ShowAnchorZones();
        SetAnchorCamInfo("已删除 " + name);
        HintZone("已删除「" + name + "」。");
    }

    /// <summary>给检测范围改名：参考图模板一起改名，避免改名后变成“缺参考”。</summary>
    private void RenameAnchorZone()
    {
        var profile = _anchorProfile;
        var zone = SelectedAnchorZone();
        if (profile == null)
        {
            HintZone("还没有检测范围。");
            return;
        }
        if (zone == null)
        {
            HintZone("请先在上面的列表里点中一个检测范围，再点「改名」。");
            return;
        }
        var oldName = zone.Name;
        var name = PromptText("检测范围改名", "新的名称", oldName);
        if (name == null) return;
        name = name.Trim();
        if (name.Length == 0)
        {
            HintZone("名称不能为空。");
            return;
        }
        if (string.Equals(name, oldName, StringComparison.Ordinal)) return;
        if (profile.Zones.Any(z => !ReferenceEquals(z, zone) && string.Equals(z.Name, name, StringComparison.OrdinalIgnoreCase)))
        {
            HintZone("已经有叫「" + name + "」的范围了，换个名字。");
            return;
        }

        AnchorZoneStore.RenameTemplate(_anchorModel, oldName, name);
        zone.Name = name;
        AnchorStore.Save(profile);
        _anchorZoneReport = null;   // 旧判定结果带的是旧名字，作废后重判
        ShowAnchorZones();
        SelectAnchorZone(zone);
        RefreshZoneRefPreview();
        SetAnchorCamInfo("已改名：" + oldName + " → " + name);
        HintZone("已改名为「" + name + "」，判定结果已作废，请重新「判定一次」。");
    }

    /// <summary>列表右键菜单：改名 / 采集参考 / 删除。</summary>
    private void ShowZoneMenu()
    {
        if (_lvAnchorZones == null) return;
        var has = SelectedAnchorZone() != null;
        var menu = new ContextMenuStrip { Font = Theme.Small };
        var miEdit = new ToolStripMenuItem("调整范围（拖/缩/转，右键保存）…") { Enabled = has };
        miEdit.Click += (_, __) => BeginAnchorZoneEdit();
        var miRename = new ToolStripMenuItem("改名…") { Enabled = has };
        miRename.Click += (_, __) => RenameAnchorZone();
        var miCapture = new ToolStripMenuItem("采集参考（板要放好）") { Enabled = has };
        miCapture.Click += (_, __) => CaptureAnchorZoneRef();
        var miDelete = new ToolStripMenuItem("删除") { Enabled = has };
        miDelete.Click += (_, __) => DeleteAnchorZone();
        menu.Items.AddRange(new ToolStripItem[] { miEdit, miRename, miCapture, new ToolStripSeparator(), miDelete });
        menu.Show(_lvAnchorZones, _lvAnchorZones.PointToClient(Control.MousePosition));
    }

    private void HintZone(string text) => SetZoneHint(text, Theme.Danger);

    private AnchorZone SelectedAnchorZone() =>
        _lvAnchorZones != null && _lvAnchorZones.SelectedItems.Count > 0 ? _lvAnchorZones.SelectedItems[0].Tag as AnchorZone : null;

    private void SelectAnchorZone(AnchorZone zone)
    {
        if (_lvAnchorZones == null) return;
        foreach (ListViewItem item in _lvAnchorZones.Items)
        {
            if (!ReferenceEquals(item.Tag, zone)) continue;
            item.Selected = true;
            item.Focused = true;
            _lvAnchorZones.EnsureVisible(item.Index);
            break;
        }
    }

    private void LoadZoneUiFromSelected()
    {
        var zone = SelectedAnchorZone();
        if (zone == null) return;
        _zoneUiSync = true;
        try
        {
            _numZoneMatch.Value = Math.Clamp((decimal)(zone.MinMatch * 100), 50m, 99.9m);
            _numZoneSearch.Value = Math.Clamp(zone.SearchPx, 0, 160);
            _numZoneShift.Value = Math.Clamp(zone.MaxShiftPx, 0, 120);
            _chkZoneRequired.Checked = zone.Required;
            var index = _cboZoneClass.Items.IndexOf(zone.TargetClass ?? "");
            _cboZoneClass.SelectedIndex = string.IsNullOrWhiteSpace(zone.TargetClass) ? 0 : Math.Max(0, index);
        }
        finally
        {
            _zoneUiSync = false;
        }
    }

    private void OnZoneParamChanged(object sender, EventArgs e)
    {
        if (_zoneUiSync || _anchorProfile == null) return;
        if (SelectedAnchorZone() == null) return;
        ApplyZoneUiToSelected();
        try { AnchorStore.Save(_anchorProfile); } catch { }
        ShowAnchorZones();
    }

    private string ZoneClassFromUi() =>
        _cboZoneClass == null || _cboZoneClass.SelectedIndex <= 0 ? "" : (_cboZoneClass.SelectedItem as string ?? "");

    private void ApplyZoneUiToSelected()
    {
        var zone = SelectedAnchorZone();
        if (zone == null) return;
        zone.MinMatch = (double)_numZoneMatch.Value / 100.0;
        zone.SearchPx = (int)_numZoneSearch.Value;
        zone.MaxShiftPx = (int)_numZoneShift.Value;
        zone.Required = _chkZoneRequired.Checked;
        zone.TargetClass = ZoneClassFromUi();
        zone.Normalize();
    }

    private void RefreshZoneClassCombo()
    {
        if (_cboZoneClass == null) return;
        var current = ZoneClassFromUi();
        var names = string.IsNullOrEmpty(_anchorModel) ? new List<string>() : new AnchorSampleStore(_anchorModel).ReadClasses();
        var modelNames = _anchorSvc?.Info?.ClassNames;
        if (modelNames != null)
            foreach (var cls in modelNames)
                if (!string.IsNullOrWhiteSpace(cls) && !names.Contains(cls, StringComparer.OrdinalIgnoreCase))
                    names.Add(cls);
        _zoneUiSync = true;
        try
        {
            _cboZoneClass.Items.Clear();
            _cboZoneClass.Items.Add("(不绑定)");
            foreach (var name in names) _cboZoneClass.Items.Add(name);
            var index = names.IndexOf(current);
            _cboZoneClass.SelectedIndex = index >= 0 ? index + 1 : 0;
        }
        finally
        {
            _zoneUiSync = false;
        }
    }

    // ---------------- 列表刷新 ----------------
    private void ShowAnchorZones()
    {
        if (_lvAnchorZones == null) return;
        var profile = _anchorProfile;
        var keep = SelectedAnchorZone();
        _lvAnchorZones.BeginUpdate();
        try
        {
            _lvAnchorZones.Items.Clear();
            if (profile == null) return;
            foreach (var zone in profile.Zones)
            {
                AnchorZoneResult result = null;
                if (_anchorZoneReport != null)
                    result = _anchorZoneReport.Zones.FirstOrDefault(r => r.Name == zone.Name);
                var hasRef = AnchorZoneStore.HasTemplate(_anchorModel, zone.Name);
                var refText = "已采样";
                if (hasRef && !string.IsNullOrEmpty(zone.RefAt))
                    refText = "参考 " + (DateTime.TryParse(zone.RefAt, out var at) ? at.ToString("HH:mm") : zone.RefAt);
                var state = !hasRef ? "缺参考" : result == null ? refText : (result.Ok ? "通过" : result.Code);
                var item = new ListViewItem(zone.Name) { Tag = zone };
                item.SubItems.Add(string.IsNullOrWhiteSpace(zone.TargetClass) ? "-" : zone.TargetClass);
                item.SubItems.Add((zone.MinMatch * 100).ToString("0.0") + "%");
                item.SubItems.Add(zone.MaxShiftPx + "px");
                item.SubItems.Add(state);
                item.ForeColor = result == null || !hasRef ? Theme.Ink : (result.Ok ? Theme.Ok : Theme.Danger);
                _lvAnchorZones.Items.Add(item);
            }
        }
        finally
        {
            _lvAnchorZones.EndUpdate();
        }
        if (keep != null) SelectAnchorZone(keep);
        _lblAnchorZoneInfo.Text = profile == null
            ? "还没有检测范围"
            : profile.Zones.Count + " 个范围；判定时会逐个比对外观，任一个「必须通过」不过就判 NG。";
        _lblAnchorZoneInfo.ForeColor = Theme.Muted;
        RefreshZoneRefPreview();
    }

    private void ShowAnchorTargets()
    {
        if (string.IsNullOrEmpty(_anchorModel)) return;
        var store = new AnchorSampleStore(_anchorModel);
        var stats = store.Stats();
        var profile = _anchorProfile;
        if (_lvAnchorTargets != null)
        {
            _lvAnchorTargets.BeginUpdate();
            try
            {
                _lvAnchorTargets.Items.Clear();
                foreach (var stat in stats)
                {
                    var used = profile == null ? 0 : profile.Zones.Count(z => string.Equals(z.TargetClass, stat.Name, StringComparison.OrdinalIgnoreCase));
                    var item = new ListViewItem(stat.Name) { Tag = stat };
                    item.SubItems.Add(stat.Count.ToString());
                    item.SubItems.Add(used == 0 ? "-" : used + " 个");
                    item.ForeColor = stat.Count == 0 ? Theme.Danger : Theme.Ink;
                    _lvAnchorTargets.Items.Add(item);
                }
            }
            finally
            {
                _lvAnchorTargets.EndUpdate();
            }
        }
        if (_lblAnchorTargetInfo != null)
            _lblAnchorTargetInfo.Text = stats.Count == 0
                ? "还没有识别目标：点“框选目标”，框住一个特征并命名（例如 p1）。"
                : "已标注 " + string.Join("，", stats.Select(s => s.Name + " " + s.Count)) + " 个框；1 张也能直接训练（样本越多样板越稳，建议每个目标 ≥60 个框）。";
    }

    // ---------------- 采样与训练 ----------------
    /* 旧版单框采样（crop 版）已被冻结帧草稿替代，保留备查
    private void SaveAnchorTargetSample(Rectangle bounds)
    {
        if (_anchorProfile == null) return;
        if (_anchorRaw == null || _anchorRaw.Empty())
        {
            SetAnchorCamInfo("没有画面可采样：先打开相机。");
            return;
        }
        if (bounds.Width < 12 || bounds.Height < 12)
        {
            SetAnchorCamInfo("框太小了：至少 12×12 像素。");
            return;
        }

        var store = new AnchorSampleStore(_anchorModel);
        var name = AnchorNameDialog.Ask(this, "新建识别目标", "给框里的特征起个名字（板上的丝印字，例如 wifi、SN、logo）：", store.ReadClasses());
        if (string.IsNullOrWhiteSpace(name)) return;

        try
        {
            var id = store.EnsureClass(name);
            store.SaveSample(_anchorRaw, new CvRect(bounds.X, bounds.Y, bounds.Width, bounds.Height), id);
        }
        catch (Exception ex)
        {
            SetAnchorCamInfo("采样失败：" + ex.Message);
            return;
        }

        ShowAnchorTargets();
        RefreshZoneClassCombo();
        var count = store.Stats().FirstOrDefault(s => string.Equals(s.Name, name, StringComparison.OrdinalIgnoreCase))?.Count ?? 0;
        SetAnchorCamInfo("已采样：" + name + "（第 " + count + " 张）。换一点位置/光照再框几次。");
    }
    */

    private async Task TrainAnchorAsync(string model = null)
    {
        if (_anchorTraining) return;
        model ??= _anchorModel;
        if (string.IsNullOrEmpty(model))
        {
            SetAnchorCamInfo("先选一个机种。");
            return;
        }

        var store = new AnchorSampleStore(model);
        TrainingSetInfo set;
        try
        {
            set = store.BuildDataset();
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, ex.Message, "识别目标", MessageBoxButtons.OK, MessageBoxIcon.Information);
            AppendAnchorTrainLine("数据集没准备好：" + ex.Message);
            return;
        }

        _anchorTraining = true;
        AppendAnchorTrainLine("数据集：训练 " + set.TrainCount + " 张 / 验证 " + set.ValCount + " 张，类别 " + string.Join("、", set.ClassNames));
        if (set.TrainCount < 10)
            AppendAnchorTrainLine("提示：样本只有 " + set.TrainCount + " 张，可以训练，但模型基本只记住这一张；判定前建议再拍几张不同位置/光照的。");
        SetAnchorBusy(true, "训练中…（CPU 上可能要几分钟）");
        try
        {
            _anchorTrainer ??= new TrainingService();
            if (!_anchorTrainerHooked)
            {
                _anchorTrainer.LogLine += AppendAnchorTrainLine;
                _anchorTrainer.Stage += stage => AppendAnchorTrainLine("· " + stage);
                _anchorTrainer.Progress += percent => AppendAnchorTrainLine("进度 " + percent + "%");
                _anchorTrainerHooked = true;
            }

            var outcome = await _anchorTrainer.TrainAsync(set, (int)_numTrainEpochs.Value, (int)_numTrainImgsz.Value, 8);
            if (!outcome.Ok || string.IsNullOrWhiteSpace(outcome.OnnxPath) || !File.Exists(outcome.OnnxPath))
            {
                AppendAnchorTrainLine("训练失败：" + outcome.Message);
                SetAnchorCamInfo("训练失败：" + outcome.Message);
                return;
            }

            Directory.CreateDirectory(AnchorStore.Dir(model));
            var target = AnchorStore.FileOf(model, AnchorStore.ModelFile);
            File.Copy(outcome.OnnxPath, target, true);
            AppendAnchorTrainLine("已部署：" + target);
            if (string.Equals(model, _anchorModel, StringComparison.OrdinalIgnoreCase))
                await LoadAnchorServiceAsync();
            SetAnchorCamInfo("训练完成，模型已部署到本机种。现在可以框检测范围并绑定目标了。");
        }
        catch (Exception ex)
        {
            AppendAnchorTrainLine("训练出错：" + ex.Message);
            SetAnchorCamInfo("训练出错：" + ex.Message);
        }
        finally
        {
            _anchorTraining = false;
            SetAnchorBusy(false);
        }
    }

    private void AppendAnchorTrainLine(string line)
    {
        if (_txtAnchorTrainLog == null) return;
        if (_txtAnchorTrainLog.InvokeRequired)
        {
            try { _txtAnchorTrainLog.BeginInvoke(new Action<string>(AppendAnchorTrainLine), line); } catch { }
            return;
        }
        _txtAnchorTrainLog.AppendText((line ?? "") + Environment.NewLine);
        _txtAnchorTrainLog.SelectionStart = _txtAnchorTrainLog.TextLength;
        _txtAnchorTrainLog.ScrollToCaret();
    }

    // ---------------- 判定组合 ----------------
    private AnchorVerdict JudgeAnchorFrame(Mat frame, PredictionResult result)
    {
        var profile = _anchorProfile;
        _anchorZoneReport = null;
        AnchorVerdict anchorVerdict = null;
        if (profile != null && profile.IsCalibrated && result != null)
            anchorVerdict = AnchorJudge.Judge(result, profile, frame.Width, frame.Height);

        if (profile != null && profile.HasZones)
        {
            var classes = _anchorSvc?.Info?.ClassNames ?? (IReadOnlyList<string>)Array.Empty<string>();
            var report = AnchorZoneJudge.Judge(frame, _anchorModel, profile.Zones, result, classes);
            _anchorZoneReport = report;
            return ComposeZoneVerdict(report, anchorVerdict, frame);
        }

        if (anchorVerdict != null) return anchorVerdict;
        return new AnchorVerdict(false, AnchorReason.NoCalib, "还没有基准", 0, 0, 1, 1, 0, 0, 0, frame.Width, frame.Height, 0, DateTime.Now, Array.Empty<AnchorHit>());
    }

    private static AnchorVerdict ComposeZoneVerdict(AnchorZoneReport report, AnchorVerdict anchorVerdict, Mat frame)
    {
        var anchorsOk = anchorVerdict == null || anchorVerdict.Ok;
        var ok = report.Ok && anchorsOk;
        var reason = AnchorReason.Ok;
        if (!report.Ok) reason = AnchorReason.Zone;
        else if (!anchorsOk) reason = anchorVerdict.Reason;

        var message = report.Ok ? "" : report.Message;
        if (!anchorsOk)
            message = string.IsNullOrWhiteSpace(message) ? anchorVerdict.Message : message + "；" + anchorVerdict.Message;

        return new AnchorVerdict(
            ok,
            reason,
            message,
            anchorVerdict?.MeanDx ?? 0,
            anchorVerdict?.MeanDy ?? 0,
            anchorVerdict?.RotDeg ?? 1,
            anchorVerdict?.Scale ?? 1,
            anchorVerdict?.RmsPx ?? 0,
            anchorVerdict?.HitCount ?? 0,
            anchorVerdict?.NeedCount ?? 0,
            frame.Width,
            frame.Height,
            report.ElapsedMs + (anchorVerdict?.ElapsedMs ?? 0),
            DateTime.Now,
            anchorVerdict?.HitList ?? Array.Empty<AnchorHit>());
    }

    private void DrawAnchorZones(Mat canvas, bool withResults)
    {
        var profile = _anchorProfile;
        if (profile == null || !profile.HasZones) return;
        // 只有判定帧才用上次结果上色；其它帧一律画“待判定”的中性黄框。
        var report = withResults ? _anchorZoneReport : null;
        foreach (var zone in profile.Zones)
        {
            AnchorZoneResult result = null;
            if (report != null) result = report.Zones.FirstOrDefault(r => r.Name == zone.Name);
            var color = result == null
                ? new CvScalar(0, 210, 255)
                : result.Ok ? new CvScalar(0, 190, 60) : new CvScalar(40, 40, 220);
            var corners = RotatedBox.Corners(zone.X, zone.Y, zone.W, zone.H, (float)zone.Angle);
            var cx = zone.X + zone.W / 2f;
            var cy = zone.Y + zone.H / 2f;
            var outer = new CvPoint[4];
            var inner = new CvPoint[4];
            for (var i = 0; i < 4; i++)
            {
                outer[i] = new CvPoint((int)Math.Round(corners[i].X), (int)Math.Round(corners[i].Y));
                inner[i] = new CvPoint((int)Math.Round(cx + (corners[i].X - cx) * 0.97), (int)Math.Round(cy + (corners[i].Y - cy) * 0.97));
            }
            Cv2.Polylines(canvas, new[] { outer }, true, color, 3);
            Cv2.Polylines(canvas, new[] { inner }, true, color, 1);

            var head = result == null
                ? zone.Name + " · 待判定"
                : result.Ok
                    ? zone.Name + " ✓ " + result.ScoreText
                    : zone.Name + " ✗ " + result.ScoreText + "（需 ≥" + (zone.MinMatch * 100).ToString("0.#") + "%）";
            var topY = (int)Math.Round(corners.Min(p => p.Y));
            var leftX = (int)Math.Round(corners.Min(p => p.X));
            var bottomY = (int)Math.Round(corners.Max(p => p.Y));
            DrawChip(canvas, leftX, topY - 32, head, color);
            if (result != null && !result.Ok)
            {
                var sub = result.Code + (string.IsNullOrWhiteSpace(result.Text) ? "" : " · " + result.Text);
                DrawChip(canvas, leftX, bottomY + 6, sub, color);
            }
        }
        if (report != null && report.Zones.Count > 0)
        {
            var ok = report.Ok;
            var summary = ok
                ? "检测范围 " + report.PassCount + "/" + report.Zones.Count + " 通过"
                : "检测范围 " + report.FailCount + " 个不通过";
            DrawChip(canvas, 18, 58, summary, ok ? new CvScalar(0, 190, 60) : new CvScalar(40, 40, 220));
        }
    }

    /// <summary>画一个带底色的小标签，保证在花哨板面上也读得清（中文走 GDI+，Cv2.PutText 不支持）。</summary>
    private static void DrawChip(Mat canvas, int x, int y, string text, CvScalar color)
    {
        if (canvas == null || string.IsNullOrEmpty(text)) return;
        OverlayText.Chip(canvas, x, y, text, OverlayText.ToColor(color), 13f);
    }
}
