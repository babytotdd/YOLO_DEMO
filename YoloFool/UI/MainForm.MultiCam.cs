using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using YoloFool.Core.MultiCam;

namespace YoloFool;

public partial class MainForm
{
    private sealed class McSlot
    {
        public CameraStream Stream;
        public CameraTile Tile;
        public string Name;
    }

    private TabPage _pageMultiCam;
    private readonly List<McSlot> _mcSlots = new();
    private FlowLayoutPanel _mcWall;
    private DataGridView _mcGrid;
    private Label _mcSelInfo;
    private Label _mcProcInfo;
    private NumericUpDown _mcNumIndex;
    private System.Windows.Forms.Timer _mcTimer;
    private Process _mcProc;
    private TimeSpan _mcLastProcCpu;
    private DateTime _mcLastProcAt;
    private double _mcProcCpuSum;
    private double _mcProcWallSum;
    private double _mcProcPeak;
    private int _mcTick;
    private bool _mcSyncing;
    private McSlot _mcSelected;

    private ComboBox _mcCmbRes;
    private ComboBox _mcCmbFmt;
    private NumericUpDown _mcNumW;
    private NumericUpDown _mcNumH;
    private NumericUpDown _mcNumFps;
    private NumericUpDown _mcNumExp;
    private NumericUpDown _mcNumWb;
    private NumericUpDown _mcNumGain;
    private NumericUpDown _mcNumBright;
    private NumericUpDown _mcNumContrast;
    private NumericUpDown _mcNumSat;
    private CheckBox _mcChkAutoExp;
    private CheckBox _mcChkAutoWb;
    private Button _mcBtnApply;
    private Label _mcLblActual;

    private const int McTileGap = 10;

    /// <summary>“多相机 · 资源”页：左边监控墙，右边开关与指标。</summary>
    private Control BuildMultiCamPage()
    {
        var root = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 1,
            BackColor = Theme.Bg,
        };
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 360));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        _mcWall = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoScroll = true,
            WrapContents = true,
            Padding = new Padding(6),
            BackColor = Color.FromArgb(24, 27, 33),
            Margin = new Padding(0),
        };
        _mcWall.Resize += (_, _) => RelayoutWall();
        root.Controls.Add(_mcWall, 0, 0);
        root.Controls.Add(BuildMultiCamSide(), 1, 0);

        _mcTimer = new System.Windows.Forms.Timer { Interval = 33 };
        _mcTimer.Tick += (_, _) => OnMultiCamTick();
        _mcTimer.Start();

        _mcProc = Process.GetCurrentProcess();
        _mcLastProcCpu = _mcProc.TotalProcessorTime;
        _mcLastProcAt = DateTime.UtcNow;

        AddCameraSlot(0);
        AddCameraSlot(1);
        return root;
    }

    private Control BuildMultiCamSide()
    {
        var side = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 9,
            BackColor = Theme.Bg,
            Padding = new Padding(10, 6, 4, 6),
            Margin = new Padding(0),
        };
        side.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        side.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        side.RowStyles.Add(new RowStyle(SizeType.Percent, 26));
        side.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        side.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        side.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        side.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        side.RowStyles.Add(new RowStyle(SizeType.Percent, 46));
        side.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        side.RowStyles.Add(new RowStyle(SizeType.Percent, 28));

        var title = Theme.SectionLabel("相机");
        title.Margin = new Padding(0, 0, 0, 4);
        side.Controls.Add(title, 0, 0);

        _mcGrid = new DataGridView { Dock = DockStyle.Fill, Margin = new Padding(0, 0, 0, 6) };
        Theme.StyleGrid(_mcGrid);
        _mcGrid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "相机", FillWeight = 34 });
        _mcGrid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "状态", FillWeight = 24 });
        _mcGrid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "分辨率", FillWeight = 22 });
        _mcGrid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "fps", FillWeight = 10 });
        _mcGrid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "CPU线程", FillWeight = 14 });
        _mcGrid.SelectionChanged += (_, _) => OnGridSelectionChanged();
        side.Controls.Add(_mcGrid, 0, 1);

        var row1 = new FlowLayoutPanel { AutoSize = true, Dock = DockStyle.Top, Margin = new Padding(0, 0, 0, 2) };
        var btnOpen = Theme.Primary("打开选中");
        btnOpen.AutoSize = true;
        btnOpen.Click += (_, _) => ToggleCamera(_mcSelected, true);
        var btnClose = Theme.Ghost("关闭选中");
        btnClose.AutoSize = true;
        btnClose.Click += (_, _) => ToggleCamera(_mcSelected, false);
        row1.Controls.Add(btnOpen);
        row1.Controls.Add(btnClose);
        side.Controls.Add(row1, 0, 2);

        var row2 = new FlowLayoutPanel { AutoSize = true, Dock = DockStyle.Top, Margin = new Padding(0, 0, 0, 2) };
        var btnAllOn = Theme.Success("全部打开");
        btnAllOn.AutoSize = true;
        btnAllOn.Click += (_, _) => { foreach (var s in _mcSlots) ToggleCamera(s, true); };
        var btnAllOff = Theme.Ghost("全部关闭");
        btnAllOff.AutoSize = true;
        btnAllOff.Click += (_, _) => { foreach (var s in _mcSlots) ToggleCamera(s, false); };
        row2.Controls.Add(btnAllOn);
        row2.Controls.Add(btnAllOff);
        side.Controls.Add(row2, 0, 3);

        var row3 = new FlowLayoutPanel { AutoSize = true, Dock = DockStyle.Top, Margin = new Padding(0, 0, 0, 2) };
        _mcNumIndex = new NumericUpDown
        {
            Minimum = 0,
            Maximum = 15,
            Value = 0,
            Width = 54,
            Font = Theme.Body,
            Margin = new Padding(0, 6, 6, 0),
        };
        var btnAdd = Theme.Ghost("＋ 添加");
        btnAdd.AutoSize = true;
        btnAdd.Click += (_, _) => AddCameraSlot((int)_mcNumIndex.Value);
        var btnRemove = Theme.Ghost("－ 移除");
        btnRemove.AutoSize = true;
        btnRemove.Click += (_, _) => RemoveSelectedSlot();
        var lblIndex = Theme.Hint("index");
        lblIndex.Margin = new Padding(0, 9, 4, 0);
        row3.Controls.Add(lblIndex);
        row3.Controls.Add(_mcNumIndex);
        row3.Controls.Add(btnAdd);
        row3.Controls.Add(btnRemove);
        side.Controls.Add(row3, 0, 4);

        _mcSelInfo = new Label
        {
            AutoSize = true,
            Font = Theme.Small,
            ForeColor = Theme.Muted,
            Margin = new Padding(0, 2, 0, 4),
        };
        side.Controls.Add(_mcSelInfo, 0, 5);

        side.Controls.Add(BuildMcSettingsPanel(), 0, 6);

        var procTitle = Theme.SectionLabel("本进程资源");
        procTitle.Margin = new Padding(0, 4, 0, 2);
        side.Controls.Add(procTitle, 0, 7);

        var bottom = new Panel { Dock = DockStyle.Fill, AutoScroll = true };
        _mcProcInfo = new Label
        {
            AutoSize = true,
            Dock = DockStyle.Top,
            Font = Theme.Small,
            ForeColor = Theme.Ink,
            Margin = new Padding(0),
        };
        var hint = Theme.Hint("CPU 的两种口径：表格里是“本路采集线程”（只含缩放+拷贝，所以很小），" +
                              "驱动内部的解码/传输入口线程不计在内；“整进程 CPU”才是整个程序真实占用，评估用后者。" +
                              "点左侧任意一格选中一路，右侧“采集设置”只改这一路，改完重新打开才生效。");
        hint.AutoSize = true;
        hint.MaximumSize = new Size(320, 0);
        hint.Dock = DockStyle.Top;
        bottom.Controls.Add(hint);
        bottom.Controls.Add(_mcProcInfo);
        side.Controls.Add(bottom, 0, 8);

        return side;
    }

    /// <summary>选中相机的采集参数（分辨率/帧率/像素格式/曝光/增益/亮度…）。</summary>
    private Control BuildMcSettingsPanel()
    {
        var host = new Panel
        {
            Dock = DockStyle.Fill,
            AutoScroll = true,
            BackColor = Theme.Bg,
            Margin = new Padding(0, 2, 0, 2),
        };

        var grid = new TableLayoutPanel
        {
            ColumnCount = 2,
            Dock = DockStyle.Top,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            BackColor = Theme.Bg,
            Margin = new Padding(0),
        };
        grid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 58));
        grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        void AddRow(string label, Control control)
        {
            int row = grid.RowCount++;
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            var lbl = new Label
            {
                Text = label,
                AutoSize = true,
                Font = Theme.Small,
                ForeColor = Theme.Muted,
                Margin = new Padding(0, 7, 4, 4),
            };
            control.Dock = DockStyle.Left;
            control.Margin = new Padding(0, 4, 0, 4);
            grid.Controls.Add(lbl, 0, row);
            grid.Controls.Add(control, 1, row);
        }

        var head = Theme.SectionLabel("采集设置（选中相机）");
        head.Margin = new Padding(0, 2, 0, 4);
        grid.RowCount = 1;
        grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        grid.Controls.Add(head, 0, 0);
        grid.SetColumnSpan(head, 2);

        _mcCmbRes = new ComboBox
        {
            DropDownStyle = ComboBoxStyle.DropDownList,
            Font = Theme.Body,
            Width = 108,
            Margin = new Padding(0, 4, 6, 4),
        };
        _mcCmbRes.Items.AddRange(new object[] { "640 × 480", "800 × 600", "1280 × 720", "1920 × 1080", "自定义" });
        _mcCmbRes.SelectedIndexChanged += (_, _) => OnResolutionPresetChanged();

        _mcNumW = NewSpin(160, 3840, 2, 1280, 64);
        _mcNumH = NewSpin(120, 2160, 2, 720, 64);
        var sizeRow = new FlowLayoutPanel { AutoSize = true, Margin = new Padding(0), BackColor = Theme.Bg };
        var times = new Label
        {
            Text = "×",
            AutoSize = true,
            Font = Theme.Body,
            ForeColor = Theme.Muted,
            Margin = new Padding(2, 8, 2, 0),
        };
        sizeRow.Controls.Add(_mcCmbRes);
        sizeRow.Controls.Add(_mcNumW);
        sizeRow.Controls.Add(times);
        sizeRow.Controls.Add(_mcNumH);
        AddRow("分辨率", sizeRow);

        _mcNumFps = NewSpin(1, 120, 1, 30, 58);
        _mcCmbFmt = new ComboBox
        {
            DropDownStyle = ComboBoxStyle.DropDownList,
            Font = Theme.Body,
            Width = 76,
            Margin = new Padding(6, 4, 0, 4),
        };
        _mcCmbFmt.Items.AddRange(new object[] { "自动", "MJPG", "YUY2" });
        _mcCmbFmt.SelectedIndex = 0;
        var fpsRow = new FlowLayoutPanel { AutoSize = true, Margin = new Padding(0), BackColor = Theme.Bg };
        fpsRow.Controls.Add(_mcNumFps);
        fpsRow.Controls.Add(_mcCmbFmt);
        AddRow("帧率/格式", fpsRow);

        _mcChkAutoExp = new CheckBox
        {
            Text = "自动",
            AutoSize = true,
            Font = Theme.Small,
            ForeColor = Theme.Ink,
            Margin = new Padding(0, 9, 6, 0),
            Checked = true,
        };
        _mcChkAutoExp.CheckedChanged += (_, _) => SyncSettingsEnabled();
        _mcNumExp = NewSpin(-13, 0, 1, -6, 58);
        var expRow = new FlowLayoutPanel { AutoSize = true, Margin = new Padding(0), BackColor = Theme.Bg };
        expRow.Controls.Add(_mcChkAutoExp);
        expRow.Controls.Add(_mcNumExp);
        AddRow("曝光", expRow);

        _mcChkAutoWb = new CheckBox
        {
            Text = "自动",
            AutoSize = true,
            Font = Theme.Small,
            ForeColor = Theme.Ink,
            Margin = new Padding(0, 9, 6, 0),
            Checked = true,
        };
        _mcChkAutoWb.CheckedChanged += (_, _) => SyncSettingsEnabled();
        _mcNumWb = NewSpin(2800, 6500, 100, 4600, 70);
        var wbRow = new FlowLayoutPanel { AutoSize = true, Margin = new Padding(0), BackColor = Theme.Bg };
        wbRow.Controls.Add(_mcChkAutoWb);
        wbRow.Controls.Add(_mcNumWb);
        AddRow("白平衡", wbRow);

        _mcNumGain = NewSpin(0, 255, 1, 0, 58);
        AddRow("增益", _mcNumGain);

        _mcNumBright = NewSpin(-64, 64, 1, 0, 58);
        AddRow("亮度", _mcNumBright);

        _mcNumContrast = NewSpin(0, 64, 1, 32, 58);
        AddRow("对比度", _mcNumContrast);

        _mcNumSat = NewSpin(0, 128, 1, 64, 58);
        AddRow("饱和度", _mcNumSat);

        int applyRow = grid.RowCount++;
        grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        var applyFlow = new FlowLayoutPanel { AutoSize = true, Margin = new Padding(0), BackColor = Theme.Bg };
        _mcBtnApply = Theme.Primary("应用并重开");
        _mcBtnApply.AutoSize = true;
        _mcBtnApply.Click += (_, _) => ApplySelectedSettings();
        applyFlow.Controls.Add(_mcBtnApply);
        grid.Controls.Add(applyFlow, 0, applyRow);
        grid.SetColumnSpan(applyFlow, 2);

        int actualRow = grid.RowCount++;
        grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _mcLblActual = new Label
        {
            Text = "实际：—",
            AutoSize = true,
            Font = Theme.Small,
            ForeColor = Theme.Muted,
            Margin = new Padding(0, 2, 0, 6),
        };
        grid.Controls.Add(_mcLblActual, 0, actualRow);
        grid.SetColumnSpan(_mcLblActual, 2);

        int noteRow = grid.RowCount++;
        grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        var note = Theme.Hint("USB2 相机线速约 18–20MB/s：720p 约 10fps、1080p 约 5fps，CPU 本身很低；驱动不接受的参数会被它自己改掉，看“实际”。");
        note.MaximumSize = new Size(315, 0);
        note.Margin = new Padding(0, 0, 0, 4);
        grid.Controls.Add(note, 0, noteRow);
        grid.SetColumnSpan(note, 2);

        host.Controls.Add(grid);
        return host;
    }

    private static NumericUpDown NewSpin(int min, int max, int step, int value, int width) => new()
    {
        Minimum = min,
        Maximum = max,
        Increment = step,
        Value = value,
        Width = width,
        Font = Theme.Body,
        Margin = new Padding(0, 4, 0, 4),
    };

    private void OnResolutionPresetChanged()
    {
        if (_mcSyncing) return;
        int index = _mcCmbRes.SelectedIndex;
        if (index < 0 || index > 3) return;
        var (w, h) = index switch
        {
            0 => (640, 480),
            1 => (800, 600),
            2 => (1280, 720),
            _ => (1920, 1080),
        };
        _mcNumW.Value = w;
        _mcNumH.Value = h;
    }

    private void SyncSettingsEnabled()
    {
        bool has = _mcSelected != null;
        if (_mcBtnApply != null) _mcBtnApply.Enabled = has;
        if (_mcNumExp != null) _mcNumExp.Enabled = has && _mcChkAutoExp != null && !_mcChkAutoExp.Checked;
        if (_mcNumWb != null) _mcNumWb.Enabled = has && _mcChkAutoWb != null && !_mcChkAutoWb.Checked;
        bool custom = _mcCmbRes != null && _mcCmbRes.SelectedIndex == 4;
        if (_mcNumW != null) _mcNumW.Enabled = has && custom;
        if (_mcNumH != null) _mcNumH.Enabled = has && custom;
    }

    private void LoadSettingsToUi()
    {
        if (_mcCmbRes == null) return;
        var cfg = _mcSelected != null ? _mcSelected.Stream.Settings : new CameraSettings();
        _mcSyncing = true;
        _mcNumW.Value = Math.Clamp(cfg.Width, (int)_mcNumW.Minimum, (int)_mcNumW.Maximum);
        _mcNumH.Value = Math.Clamp(cfg.Height, (int)_mcNumH.Minimum, (int)_mcNumH.Maximum);
        _mcNumFps.Value = Math.Clamp(cfg.Fps, (int)_mcNumFps.Minimum, (int)_mcNumFps.Maximum);
        _mcCmbRes.SelectedIndex = PresetIndex(cfg.Width, cfg.Height);
        _mcCmbFmt.SelectedIndex = cfg.PixelFormat switch
        {
            "MJPG" => 1,
            "YUY2" => 2,
            _ => 0,
        };
        _mcChkAutoExp.Checked = cfg.AutoExposure;
        _mcNumExp.Value = Math.Clamp((int)Math.Round(cfg.Exposure), (int)_mcNumExp.Minimum, (int)_mcNumExp.Maximum);
        _mcChkAutoWb.Checked = cfg.AutoWhiteBalance;
        _mcNumWb.Value = Math.Clamp((int)Math.Round(cfg.WhiteBalance), (int)_mcNumWb.Minimum, (int)_mcNumWb.Maximum);
        _mcNumGain.Value = Math.Clamp((int)Math.Round(cfg.Gain), (int)_mcNumGain.Minimum, (int)_mcNumGain.Maximum);
        _mcNumBright.Value = Math.Clamp((int)Math.Round(cfg.Brightness), (int)_mcNumBright.Minimum, (int)_mcNumBright.Maximum);
        _mcNumContrast.Value = Math.Clamp((int)Math.Round(cfg.Contrast), (int)_mcNumContrast.Minimum, (int)_mcNumContrast.Maximum);
        _mcNumSat.Value = Math.Clamp((int)Math.Round(cfg.Saturation), (int)_mcNumSat.Minimum, (int)_mcNumSat.Maximum);
        _mcSyncing = false;
        SyncSettingsEnabled();
    }

    private static int PresetIndex(int w, int h) => (w, h) switch
    {
        (640, 480) => 0,
        (800, 600) => 1,
        (1280, 720) => 2,
        (1920, 1080) => 3,
        _ => 4,
    };

    private CameraSettings ReadSettingsFromUi()
    {
        var cfg = _mcSelected != null ? _mcSelected.Stream.Settings : new CameraSettings();
        cfg.Width = (int)_mcNumW.Value;
        cfg.Height = (int)_mcNumH.Value;
        cfg.Fps = (int)_mcNumFps.Value;
        cfg.PixelFormat = _mcCmbFmt.SelectedIndex switch
        {
            1 => "MJPG",
            2 => "YUY2",
            _ => "自动",
        };
        cfg.AutoExposure = _mcChkAutoExp.Checked;
        cfg.Exposure = (double)_mcNumExp.Value;
        cfg.AutoWhiteBalance = _mcChkAutoWb.Checked;
        cfg.WhiteBalance = (double)_mcNumWb.Value;
        cfg.Gain = (double)_mcNumGain.Value;
        cfg.Brightness = (double)_mcNumBright.Value;
        cfg.Contrast = (double)_mcNumContrast.Value;
        cfg.Saturation = (double)_mcNumSat.Value;
        return cfg;
    }

    private void ApplySelectedSettings()
    {
        var slot = _mcSelected;
        if (slot == null) return;
        var cfg = ReadSettingsFromUi();
        bool wasBusy = slot.Stream.IsBusy;
        Cursor = Cursors.WaitCursor;
        try
        {
            if (wasBusy) slot.Stream.Stop();
            slot.Stream.SetSettings(cfg);
            if (wasBusy) slot.Stream.Start();
        }
        finally
        {
            Cursor = Cursors.Default;
        }

        Log.Info($"相机 {slot.Name} 应用采集参数 {cfg.Width}x{cfg.Height}@{cfg.Fps} 格式={cfg.PixelFormat} " +
                 $"自动曝光={cfg.AutoExposure} 曝光={cfg.Exposure} 增益={cfg.Gain} 亮度={cfg.Brightness} " +
                 $"对比度={cfg.Contrast} 饱和度={cfg.Saturation} 自动白平衡={cfg.AutoWhiteBalance} 白平衡={cfg.WhiteBalance}");
        UpdateStats();
    }


    private void AddCameraSlot(int index)
    {
        var stream = new CameraStream(index);
        var slot = new McSlot
        {
            Stream = stream,
            Name = "相机 " + index,
            Tile = new CameraTile(stream),
        };
        slot.Tile.Click += (_, _) => SelectSlot(slot);
        _mcSlots.Add(slot);
        _mcWall.Controls.Add(slot.Tile);
        RebuildGrid();
        RelayoutWall();
        SelectSlot(slot);
    }

    private void RemoveSelectedSlot()
    {
        var slot = _mcSelected;
        if (slot == null) return;
        _mcSlots.Remove(slot);
        _mcWall.Controls.Remove(slot.Tile);
        slot.Tile.Dispose();
        slot.Stream.Dispose();
        _mcSelected = null;
        RebuildGrid();
        RelayoutWall();
        if (_mcSlots.Count > 0) SelectSlot(_mcSlots[0]);
        else
        {
            UpdateStats();
            LoadSettingsToUi();
            SyncSettingsEnabled();
            if (_mcLblActual != null) _mcLblActual.Text = "实际：—";
        }
    }

    private void ToggleCamera(McSlot slot, bool open)
    {
        if (slot == null) return;
        if (open)
        {
            if (slot.Stream.IsBusy) return;
            slot.Stream.Start();
        }
        else
        {
            slot.Stream.Stop();
        }
        UpdateStats();
    }

    private void RebuildGrid()
    {
        _mcSyncing = true;
        _mcGrid.Rows.Clear();
        foreach (var slot in _mcSlots)
            _mcGrid.Rows.Add(slot.Name, "空闲", "-", "-", "-");
        _mcSyncing = false;
    }

    private void OnGridSelectionChanged()
    {
        if (_mcSyncing) return;
        int row = _mcGrid.SelectedRows.Count > 0 ? _mcGrid.SelectedRows[0].Index : -1;
        if (row < 0 || row >= _mcSlots.Count) return;
        SelectSlot(_mcSlots[row], false);
    }

    private void SelectSlot(McSlot slot, bool syncGrid = true)
    {
        _mcSelected = slot;
        foreach (var s in _mcSlots) s.Tile.Selected = ReferenceEquals(s, slot);
        foreach (var s in _mcSlots) s.Tile.Invalidate();
        if (syncGrid && slot != null)
        {
            int index = _mcSlots.IndexOf(slot);
            if (index >= 0 && index < _mcGrid.Rows.Count)
            {
                _mcSyncing = true;
                _mcGrid.ClearSelection();
                _mcGrid.Rows[index].Selected = true;
                _mcSyncing = false;
            }
        }
        UpdateStats();
        LoadSettingsToUi();
        SyncSettingsEnabled();
    }

    private void RelayoutWall()
    {
        if (_mcWall == null) return;
        int avail = _mcWall.ClientSize.Width - _mcWall.Padding.Horizontal - 4;
        if (avail < 140) return;
        int cols = Math.Max(1, (avail + McTileGap) / (360 + McTileGap));
        if (_mcSlots.Count > 0) cols = Math.Min(cols, _mcSlots.Count);
        int tileW = (avail - (cols - 1) * McTileGap) / cols;
        tileW = Math.Max(160, tileW);
        int tileH = Math.Max(90, tileW * 9 / 16);
        foreach (var slot in _mcSlots)
        {
            slot.Tile.Size = new Size(tileW, tileH);
            slot.Tile.Margin = new Padding(0, 0, McTileGap, McTileGap);
            slot.Stream.SetTargetSize(tileW, tileH);
        }
    }

    private void OnMultiCamTick()
    {
        if (_mcWall == null || !_mcWall.Visible) return;
        foreach (var slot in _mcSlots)
        {
            if (slot.Tile.SyncFrame()) slot.Tile.Invalidate();
        }
        if (++_mcTick % 30 != 0) return;
        UpdateStats();
        foreach (var slot in _mcSlots) slot.Tile.Invalidate();
    }

    private void UpdateStats()
    {
        if (_mcGrid == null) return;
        double cameraCpu = 0;
        for (int i = 0; i < _mcSlots.Count && i < _mcGrid.Rows.Count; i++)
        {
            var slot = _mcSlots[i];
            var m = slot.Stream.Snapshot();
            cameraCpu += m.State == CameraState.Running ? m.CpuAvgPercent : 0;
            _mcGrid.Rows[i].Cells[0].Value = slot.Name;
            _mcGrid.Rows[i].Cells[1].Value = StateLabel(m.State);
            _mcGrid.Rows[i].Cells[2].Value = m.State == CameraState.Running
                ? m.SrcWidth + "×" + m.SrcHeight + (m.SizeHonored ? "" : "←" + m.ReqWidth + "×" + m.ReqHeight)
                : "-";
            _mcGrid.Rows[i].Cells[3].Value = m.State == CameraState.Running ? m.Fps.ToString("0.0") : "-";
            _mcGrid.Rows[i].Cells[4].Value = m.State == CameraState.Running ? m.CpuAvgPercent.ToString("0.0") : "-";
        }

        if (_mcSelected != null)
        {
            var m = _mcSelected.Stream.Snapshot();
            string text;
            if (m.State == CameraState.Running)
            {
                text = "选中：" + _mcSelected.Name + "   打开 " + m.OpenMs.ToString("0") + "ms" +
                       "\r\n分辨率 " + m.SrcWidth + "×" + m.SrcHeight +
                       (m.SizeHonored ? "" : "（请求 " + m.ReqWidth + "×" + m.ReqHeight + " 未生效）") +
                       "\r\n本路 CPU 均 " + m.CpuAvgPercent.ToString("0.0") + "%" +
                       "   峰值 " + m.PeakCpuPercent.ToString("0.0") + "%" +
                       "   取帧 " + m.FrameMs.ToString("0.0") + "ms" +
                       "\r\n分配 " + m.AllocMbPerSec.ToString("0.0") + "MB/s" +
                       "   帧缓冲 " + m.BufferMb.ToString("0.0") + "MB" +
                       "   已取 " + m.Frames + " 帧";
            }
            else if (m.State == CameraState.Failed)
            {
                text = "选中：" + _mcSelected.Name + "\r\n失败：" + TrimText(m.Error, 160);
            }
            else
            {
                text = "选中：" + _mcSelected.Name + "   状态 " + StateLabel(m.State);
            }
            _mcSelInfo.Text = text;
            if (_mcLblActual != null)
            {
                _mcLblActual.Text = m.State == CameraState.Running
                    ? "实际：" + m.SrcWidth + "×" + m.SrcHeight + " " + m.PixelFormat + " @" + m.Fps.ToString("0.0") +
                      "fps  " + m.Api
                    : "实际：—";
            }
        }
        else
        {
            _mcSelInfo.Text = "未选中相机";
            if (_mcLblActual != null) _mcLblActual.Text = "实际：—";
        }

        if (_mcProc != null && _mcProcInfo != null)
        {
            _mcProc.Refresh();
            var now = DateTime.UtcNow;
            var cpu = _mcProc.TotalProcessorTime;
            double wall = (now - _mcLastProcAt).TotalSeconds;
            double used = wall > 0.001 ? (cpu - _mcLastProcCpu).TotalSeconds / wall * 100 : 0;
            _mcLastProcCpu = cpu;
            _mcLastProcAt = now;

            double dCpu = Math.Max(0, (cpu - _mcLastProcCpu).TotalSeconds);
            _mcProcCpuSum += dCpu;
            _mcProcWallSum += Math.Max(0, wall);
            if (used > _mcProcPeak) _mcProcPeak = used;
            double avg = _mcProcWallSum > 0.3 ? _mcProcCpuSum / _mcProcWallSum * 100 : used;
            int cores = Environment.ProcessorCount;

            _mcProcInfo.Text =
                "整进程 CPU 均 " + avg.ToString("0.0") + "%" +
                "   峰值 " + _mcProcPeak.ToString("0.0") + "%" +
                "\r\n（单核 = 100%，即占用 " + (avg / 100).ToString("0.00") + " 核；" +
                "整机 " + (avg / cores).ToString("0.0") + "% / " + cores + " 逻辑核）" +
                "\r\n内存 工作集 " + (_mcProc.WorkingSet64 / 1024.0 / 1024.0).ToString("0") + " MB" +
                "   私有 " + (_mcProc.PrivateMemorySize64 / 1024.0 / 1024.0).ToString("0") + " MB" +
                "   峰值 " + (_mcProc.PeakWorkingSet64 / 1024.0 / 1024.0).ToString("0") + " MB" +
                "\r\n线程 " + _mcProc.Threads.Count + "   句柄 " + _mcProc.HandleCount +
                "   GC 堆 " + (GC.GetTotalMemory(false) / 1024.0 / 1024.0).ToString("0.0") + " MB" +
                "\r\n其中采集中继线程合计 " + cameraCpu.ToString("0.0") + "%（只算换算+拷贝，" +
                "驱动/DSHOW 内部线程在上一行里）";
        }
    }

    private static string StateLabel(CameraState state) => state switch
    {
        CameraState.Running => "运行",
        CameraState.Opening => "打开中",
        CameraState.Failed => "失败",
        _ => "空闲",
    };

    private void ShutdownMultiCam()
    {
        _mcTimer?.Stop();
        foreach (var slot in _mcSlots)
        {
            slot.Stream.Dispose();
            slot.Tile.Dispose();
        }
        _mcSlots.Clear();
        _mcProc?.Dispose();
        _mcProc = null;
    }
}
