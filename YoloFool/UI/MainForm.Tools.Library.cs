using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DeploySharp.Engine;

namespace YoloFool;

/// <summary>“工具 · 我的模型”页里的三张新卡：模型库、针床定位V2 学习库、机种管理。</summary>
public sealed partial class MainForm
{
    private ComboBox _cboToolsLibCategory;
    private ListView _lvToolsLibModels;
    private Label _lblToolsLibInfo;
    private Label _lblToolsLibDir;
    private Button _btnToolsLibUse;
    private Button _btnToolsLibOpen;

    private ComboBox _cboToolsAnchorModel;
    private Label _lblToolsAnchorStats;
    private Label _lblToolsAnchorModelInfo;
    private Label _lblToolsAnchorCalibInfo;

    private ComboBox _cboToolsMachineKind;
    private ListView _lvToolsMachines;
    private Label _lblToolsMachineInfo;

    private readonly List<ToolModelEntry> _toolsLibEntries = new();
    private readonly List<ToolMachineEntry> _toolsMachineEntries = new();
    private bool _toolsLibSyncing;
    private bool _toolsMachineSyncing;

    // ---------------------------------------------------------------- 模型库

    private Control BuildToolsLibraryCard()
    {
        var (card, body) = MakeCard("模型库（磁盘上现有的都在这）");
        card.Name = "cardToolsLib";

        _lblToolsLibInfo = new Label
        {
            Text = "选一个类别，看这一类的模型 / 学习库现在有哪些。选中的 onnx 可以直接切过去识别。",
            AutoSize = true,
            Font = Theme.Small,
            ForeColor = Theme.Muted,
            Margin = new Padding(0, 0, 0, 6),
        };
        AddRow(body, _lblToolsLibInfo);

        var pick = new TableLayoutPanel
        {
            Dock = DockStyle.Top,
            AutoSize = true,
            ColumnCount = 2,
            RowCount = 1,
            Margin = new Padding(0, 0, 0, 6),
        };
        pick.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
        pick.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

        _cboToolsLibCategory = MakeCombo();
        _cboToolsLibCategory.Margin = new Padding(0, 2, 8, 2);
        _cboToolsLibCategory.SelectedIndexChanged += (_, _) =>
        {
            if (_toolsLibSyncing) return;
            RefreshToolsLibraryModels();
        };
        pick.Controls.Add(_cboToolsLibCategory, 0, 0);

        var btnOpen = Theme.Ghost("打开所在目录");
        Stretch(btnOpen, 128, 32);
        btnOpen.Margin = new Padding(0, 2, 0, 2);
        btnOpen.Click += (_, _) => OpenToolsLibFolder();
        pick.Controls.Add(btnOpen, 1, 0);
        AddRow(body, pick);

        _lvToolsLibModels = MakeToolsList(("名称", 230), ("大小", 84), ("修改时间", 116), ("说明", 420));
        _lvToolsLibModels.SelectedIndexChanged += (_, _) => SyncToolsLibButtons();
        _lvToolsLibModels.DoubleClick += (_, _) => UseToolsLibModel();
        AddRow(body, _lvToolsLibModels);

        var flow = ToolsFlow();
        _btnToolsLibUse = ToolsButton("用这个 ONNX 识别", (_, _) => UseToolsLibModel(), true, 150);
        _btnToolsLibOpen = ToolsButton("打开所在目录", (_, _) => OpenToolsLibFolder(), false, 128);
        flow.Controls.Add(_btnToolsLibUse);
        flow.Controls.Add(_btnToolsLibOpen);
        AddRow(body, flow);

        _lblToolsLibDir = new Label
        {
            Text = "",
            AutoSize = true,
            Font = Theme.Small,
            ForeColor = Theme.Muted,
            Margin = new Padding(0, 0, 0, 2),
        };
        AddRow(body, _lblToolsLibDir);
        return card;
    }

    private void RefreshToolsLibrary()
    {
        if (_cboToolsLibCategory == null || IsDisposed) return;

        _toolsLibSyncing = true;
        try
        {
            var keep = _cboToolsLibCategory.SelectedIndex;
            _cboToolsLibCategory.Items.Clear();
            foreach (ToolLibCategory c in Enum.GetValues<ToolLibCategory>())
            {
                var n = ToolsLibrary.ScanModels(c).Count;
                _cboToolsLibCategory.Items.Add($"{ToolsLibrary.Name(c)}（{n}）");
            }

            _cboToolsLibCategory.SelectedIndex = keep >= 0 && keep < _cboToolsLibCategory.Items.Count ? keep : 0;
        }
        finally
        {
            _toolsLibSyncing = false;
        }

        RefreshToolsLibraryModels();
    }

    private ToolLibCategory SelectedLibCategory() =>
        _cboToolsLibCategory != null && _cboToolsLibCategory.SelectedIndex >= 0
            ? (ToolLibCategory)_cboToolsLibCategory.SelectedIndex
            : ToolLibCategory.Trained;

    private void RefreshToolsLibraryModels()
    {
        if (_lvToolsLibModels == null || IsDisposed) return;
        var c = SelectedLibCategory();
        _toolsLibEntries.Clear();
        _lvToolsLibModels.Items.Clear();

        try
        {
            _toolsLibEntries.AddRange(ToolsLibrary.ScanModels(c));

            foreach (var e in _toolsLibEntries)
            {
                var item = new ListViewItem(e.Name);
                item.SubItems.Add(e.Bytes > 0 ? ToolsLibrary.FormatSize(e.Bytes) : "—");
                item.SubItems.Add(e.Modified.ToString("MM-dd HH:mm"));
                item.SubItems.Add(e.Summary);
                item.Tag = e;
                _lvToolsLibModels.Items.Add(item);
            }
        }
        catch (Exception ex)
        {
            _lblToolsLibInfo.Text = "读取失败：" + ex.Message;
            _lblToolsLibInfo.ForeColor = Theme.Danger;
        }

        _lblToolsLibInfo.ForeColor = Theme.Muted;
        _lblToolsLibInfo.Text = ToolsLibrary.IsMachine(c)
            ? "这里列的是机种/学习库目录；要看样本、训练和基准，用下面的“针床定位V2 · 学习库管理”卡。"
            : "选中的 onnx 可以直接点“用这个 ONNX 识别”切换过去，不会删除任何文件。";
        _lblToolsLibDir.Text = "目录：" + ToolsLibrary.Root(c);
        SyncToolsLibButtons();
    }

    private void SyncToolsLibButtons()
    {
        if (_btnToolsLibUse == null) return;
        var e = _lvToolsLibModels?.SelectedItems.Count > 0
            ? _lvToolsLibModels.SelectedItems[0].Tag as ToolModelEntry
            : null;
        _btnToolsLibUse.Enabled = e != null && !e.IsMachine && File.Exists(e.Path);
        if (_btnToolsLibOpen != null) _btnToolsLibOpen.Enabled = e != null;
    }

    private void OpenToolsLibFolder()
    {
        var e = _lvToolsLibModels?.SelectedItems.Count > 0
            ? _lvToolsLibModels.SelectedItems[0].Tag as ToolModelEntry
            : null;
        var dir = e?.Dir;
        if (string.IsNullOrEmpty(dir)) dir = ToolsLibrary.Root(SelectedLibCategory());
        OpenFolder(dir, "模型目录");
    }

    private async void UseToolsLibModel()
    {
        var e = _lvToolsLibModels?.SelectedItems.Count > 0
            ? _lvToolsLibModels.SelectedItems[0].Tag as ToolModelEntry
            : null;
        if (e == null || e.IsMachine || !File.Exists(e.Path)) return;

        var kind = OnnxInspector.DetectKind(e.Path) ?? YoloModelKind.YOLOv8Det;
        var ok = await LoadModelFileAsync(e.Path, kind, true);
        if (ok && _tabs != null && _pageRec != null) _tabs.SelectedTab = _pageRec;
        RefreshToolsPage();
    }

    // ---------------------------------------------------------------- V2 学习库

    private Control BuildToolsAnchorLibraryCard()
    {
        var (card, body) = MakeCard("针床定位V2 · 学习库管理");
        card.Name = "cardToolsAnchorLib";

        var pick = new TableLayoutPanel
        {
            Dock = DockStyle.Top,
            AutoSize = true,
            ColumnCount = 2,
            RowCount = 1,
            Margin = new Padding(0, 0, 0, 6),
        };
        pick.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 220));
        pick.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));

        _cboToolsAnchorModel = MakeCombo();
        _cboToolsAnchorModel.Margin = new Padding(0, 2, 12, 2);
        _cboToolsAnchorModel.SelectedIndexChanged += (_, _) =>
        {
            if (_toolsMachineSyncing) return;
            RefreshToolsAnchorDetail();
        };
        pick.Controls.Add(_cboToolsAnchorModel, 0, 0);

        _lblToolsAnchorStats = new Label
        {
            Text = "",
            AutoSize = true,
            Font = Theme.Body,
            ForeColor = Theme.Ink,
            Anchor = AnchorStyles.Left,
            Margin = new Padding(0, 7, 0, 4),
        };
        pick.Controls.Add(_lblToolsAnchorStats, 1, 0);
        AddRow(body, pick);

        _lblToolsAnchorModelInfo = new Label
        {
            Text = "",
            AutoSize = true,
            Font = Theme.Small,
            ForeColor = Theme.Muted,
            Margin = new Padding(0, 0, 0, 2),
        };
        AddRow(body, _lblToolsAnchorModelInfo);

        _lblToolsAnchorCalibInfo = new Label
        {
            Text = "",
            AutoSize = true,
            Font = Theme.Small,
            ForeColor = Theme.Muted,
            Margin = new Padding(0, 0, 0, 6),
        };
        AddRow(body, _lblToolsAnchorCalibInfo);

        var flow = ToolsFlow();
        flow.Controls.Add(ToolsButton("新建学习库", (_, _) => CreateToolsMachine(ToolLibCategory.AnchorV2), false, 112));
        flow.Controls.Add(ToolsButton("复制当前训练模型", (_, _) => CopyToolsAnchorModel(), false, 150));
        flow.Controls.Add(ToolsButton("训练并部署", (_, _) => TrainToolsAnchorModel(), true, 116));
        flow.Controls.Add(ToolsButton("打开目录", (_, _) => OpenToolsAnchorFolder(), false, 96));
        flow.Controls.Add(ToolsButton("压缩归档", (_, _) => ArchiveToolsMachine(ToolLibCategory.AnchorV2), false, 96));
        AddRow(body, flow);
        return card;
    }

    private void RefreshToolsAnchorLibrary()
    {
        if (_cboToolsAnchorModel == null || IsDisposed) return;

        _toolsMachineSyncing = true;
        try
        {
            var keep = _cboToolsAnchorModel.SelectedItem as string;
            var models = AnchorStore.ListModels();
            _cboToolsAnchorModel.Items.Clear();
            foreach (var m in models) _cboToolsAnchorModel.Items.Add(m);

            if (models.Count == 0)
            {
                _cboToolsAnchorModel.SelectedIndex = -1;
            }
            else
            {
                var idx = keep == null ? -1 : models.ToList().IndexOf(keep);
                _cboToolsAnchorModel.SelectedIndex = idx >= 0 ? idx : 0;
            }
        }
        finally
        {
            _toolsMachineSyncing = false;
        }

        RefreshToolsAnchorDetail();
    }

    private void RefreshToolsAnchorDetail()
    {
        if (_lblToolsAnchorStats == null) return;
        var model = _cboToolsAnchorModel?.SelectedItem as string;
        if (string.IsNullOrEmpty(model))
        {
            _lblToolsAnchorStats.Text = "还没有学习库，点“新建学习库”建一个。";
            _lblToolsAnchorModelInfo.Text = "";
            _lblToolsAnchorCalibInfo.Text = "";
            return;
        }

        try
        {
            var stats = new AnchorSampleStore(model).Stats();
            var photos = stats.Sum(s => s.Count);
            _lblToolsAnchorStats.Text = $"样本 {photos} 张 · {stats.Count} 类"
                + (stats.Count > 0 ? "（" + string.Join("、", stats.Select(s => $"{s.Name} {s.Count}")) + "）" : "");

            var mp = AnchorStore.LocalModelPath(model);
            _lblToolsAnchorModelInfo.Text = string.IsNullOrEmpty(mp)
                ? "模型：还没有（可以复制当前训练模型，或标注样本后训练）"
                : $"模型：{ToolsLibrary.FormatSize(new FileInfo(mp).Length)} · {File.GetLastWriteTime(mp):yyyy-MM-dd HH:mm}";

            var profile = AnchorStore.LoadOrCreate(model);
            _lblToolsAnchorCalibInfo.Text = profile.IsCalibrated
                ? $"基准：{profile.Calib.At} · {profile.Calib.Anchors.Count} 个锚点 · {profile.Calib.Width}×{profile.Calib.Height}"
                : "基准：还没有拍（到“③ 拍基准 · 对比”页拍）";
        }
        catch (Exception ex)
        {
            _lblToolsAnchorStats.Text = "读取学习库失败：" + ex.Message;
        }
    }

    private void OpenToolsAnchorFolder()
    {
        var model = _cboToolsAnchorModel?.SelectedItem as string;
        if (string.IsNullOrEmpty(model)) return;
        OpenFolder(AnchorStore.Dir(model), "学习库目录");
    }

    private void CopyToolsAnchorModel()
    {
        var model = _cboToolsAnchorModel?.SelectedItem as string;
        if (string.IsNullOrEmpty(model)) return;

        if (!AnchorStore.CopyFromGlobalModel(model, out var error))
        {
            MessageBox.Show(this, error, "复制模型", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        _lblStatus.Text = "模型已复制到学习库：" + model;
        Log.Info("复制当前训练模型到 V2 学习库：" + model);
        RefreshToolsAnchorDetail();
        if (string.Equals(_cboAnchorModel?.SelectedItem as string, model, StringComparison.OrdinalIgnoreCase))
            _ = LoadAnchorServiceAsync();
    }

    private async void TrainToolsAnchorModel()
    {
        var model = _cboToolsAnchorModel?.SelectedItem as string;
        if (string.IsNullOrEmpty(model)) return;

        var stats = new AnchorSampleStore(model).Stats();
        var photos = stats.Sum(s => s.Count);
        if (photos == 0)
        {
            MessageBox.Show(this, "这个学习库还没有样本。\n\n先到“针床定位V2 → ① 识别目标”选机种、采集几张样本。",
                "训练并部署", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var yes = MessageBox.Show(this,
            $"用学习库“{model}”的 {photos} 张样本（{stats.Count} 类）训练锚点模型？\n\n"
            + "训练在后台跑，完成后自动部署到这个学习库。",
            "训练并部署", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (yes != DialogResult.Yes) return;

        await TrainAnchorAsync(model);
        RefreshToolsAnchorDetail();
        RefreshToolsLibrary();
    }

    // ---------------------------------------------------------------- 机种管理

    private Control BuildToolsMachineCard()
    {
        var (card, body) = MakeCard("机种管理");
        card.Name = "cardToolsMachine";

        var pick = new TableLayoutPanel
        {
            Dock = DockStyle.Top,
            AutoSize = true,
            ColumnCount = 2,
            RowCount = 1,
            Margin = new Padding(0, 0, 0, 6),
        };
        pick.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 260));
        pick.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));

        _cboToolsMachineKind = MakeCombo();
        _cboToolsMachineKind.Margin = new Padding(0, 2, 12, 2);
        _cboToolsMachineKind.SelectedIndexChanged += (_, _) =>
        {
            if (_toolsMachineSyncing) return;
            RefreshToolsMachinesList();
        };
        pick.Controls.Add(_cboToolsMachineKind, 0, 0);

        _lblToolsMachineInfo = new Label
        {
            Text = "",
            AutoSize = true,
            Font = Theme.Small,
            ForeColor = Theme.Muted,
            Anchor = AnchorStyles.Left,
            Margin = new Padding(0, 7, 0, 4),
        };
        pick.Controls.Add(_lblToolsMachineInfo, 1, 0);
        AddRow(body, pick);

        _lvToolsMachines = MakeToolsList(("机种", 200), ("修改时间", 116), ("说明", 470));
        AddRow(body, _lvToolsMachines);

        var flow = ToolsFlow();
        flow.Controls.Add(ToolsButton("新建机种", (_, _) => CreateToolsMachine(MachineCategory()), true, 110));
        flow.Controls.Add(ToolsButton("重命名", (_, _) => RenameToolsMachine(), false, 96));
        flow.Controls.Add(ToolsButton("压缩归档（删除）", (_, _) => ArchiveToolsMachine(MachineCategory()), false, 156));
        flow.Controls.Add(ToolsButton("打开目录", (_, _) => OpenToolsMachineFolder(), false, 96));
        AddRow(body, flow);
        return card;
    }

    private ToolLibCategory MachineCategory() =>
        _cboToolsMachineKind != null && _cboToolsMachineKind.SelectedIndex == 1
            ? ToolLibCategory.AnchorV2
            : ToolLibCategory.Fixture;

    private void RefreshToolsMachines()
    {
        if (_cboToolsMachineKind == null || IsDisposed) return;

        if (_cboToolsMachineKind.Items.Count == 0)
        {
            _toolsMachineSyncing = true;
            try
            {
                _cboToolsMachineKind.Items.Add("针床检测（fixtures）");
                _cboToolsMachineKind.Items.Add("针床定位V2（fixtures_v2）");
                _cboToolsMachineKind.SelectedIndex = 0;
            }
            finally
            {
                _toolsMachineSyncing = false;
            }
        }

        RefreshToolsMachinesList();
    }

    private void RefreshToolsMachinesList()
    {
        if (_lvToolsMachines == null || IsDisposed) return;
        var cat = MachineCategory();
        _toolsMachineEntries.Clear();
        _lvToolsMachines.Items.Clear();

        try
        {
            _toolsMachineEntries.AddRange(ToolsLibrary.ScanMachines(cat));
            foreach (var m in _toolsMachineEntries)
            {
                var item = new ListViewItem(m.Name);
                item.SubItems.Add(m.Modified.ToString("MM-dd HH:mm"));
                item.SubItems.Add(m.Summary);
                item.Tag = m;
                _lvToolsMachines.Items.Add(item);
            }
        }
        catch (Exception ex)
        {
            _lblToolsMachineInfo.Text = "读取失败：" + ex.Message;
            return;
        }

        var backup = ToolsLibrary.ArchiveRoot(cat);
        var zips = Directory.Exists(backup) ? new DirectoryInfo(backup).GetFiles("*.zip") : Array.Empty<FileInfo>();
        _lblToolsMachineInfo.Text =
            $"{ToolsLibrary.Name(cat)}：{_toolsMachineEntries.Count} 个 · backup {zips.Length} 个压缩包"
            + $"（{ToolsLibrary.FormatSize(zips.Sum(f => f.Length))}，超 "
            + $"{ToolsLibrary.ArchiveCapBytes / 1024 / 1024}MB 自动删最旧）";
    }

    private void CreateToolsMachine(ToolLibCategory cat)
    {
        var name = PromptText(cat == ToolLibCategory.Fixture ? "新建针床检测机种" : "新建定位V2学习库",
            "机种编号（例如 B002）", "");
        if (string.IsNullOrWhiteSpace(name)) return;

        name = cat == ToolLibCategory.Fixture ? FixtureStore.Sanitize(name) : AnchorStore.Sanitize(name);
        if (string.IsNullOrWhiteSpace(name)) return;

        try
        {
            var dir = ToolsLibrary.MachineDir(cat, name);
            if (Directory.Exists(dir))
            {
                MessageBox.Show(this, "这个机种已经存在：\n" + dir, "新建机种",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (cat == ToolLibCategory.Fixture)
            {
                FixtureStore.EnsureModel(name);
                FixtureStore.Save(FixtureRecipe.CreateDefault(name));
            }
            else
            {
                Directory.CreateDirectory(AnchorStore.Dir(name));
            }

            Log.Info($"新建机种：{cat} / {name}");
            _lblStatus.Text = "已新建机种：" + name;
            AfterMachineChange(cat);
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, "新建失败：\n" + ex.Message, "新建机种",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void RenameToolsMachine()
    {
        var cat = MachineCategory();
        var item = _lvToolsMachines?.SelectedItems.Count > 0
            ? _lvToolsMachines.SelectedItems[0].Tag as ToolMachineEntry
            : null;
        if (item == null)
        {
            MessageBox.Show(this, "先在列表里选一个机种。", "重命名",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var name = PromptText("重命名机种", "新的机种编号", item.Name);
        if (string.IsNullOrWhiteSpace(name)) return;
        name = cat == ToolLibCategory.Fixture ? FixtureStore.Sanitize(name) : AnchorStore.Sanitize(name);
        if (string.IsNullOrWhiteSpace(name) || name == item.Name) return;

        try
        {
            var dst = ToolsLibrary.MachineDir(cat, name);
            if (Directory.Exists(dst))
            {
                MessageBox.Show(this, "已经有同名机种：\n" + dst, "重命名",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Directory.Move(item.Dir, dst);
            if (cat == ToolLibCategory.Fixture)
            {
                var recipe = FixtureStore.LoadOrCreate(name);
                recipe.Model = name;
                FixtureStore.Save(recipe);
            }
            else
            {
                var profile = AnchorStore.LoadOrCreate(name);
                profile.Model = name;
                AnchorStore.Save(profile);
            }

            Log.Info($"机种改名：{cat} / {item.Name} → {name}");
            _lblStatus.Text = $"机种已改名：{item.Name} → {name}";
            AfterMachineChange(cat);
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, "重命名失败：\n" + ex.Message, "重命名",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void ArchiveToolsMachine(ToolLibCategory cat)
    {
        var item = (cat == MachineCategory() ? _lvToolsMachines : null)?.SelectedItems.Count > 0
            ? _lvToolsMachines.SelectedItems[0].Tag as ToolMachineEntry
            : null;

        if (item == null)
        {
            // 从 V2 学习库卡的按钮进来时，用下拉里选中的学习库
            var model = _cboToolsAnchorModel?.SelectedItem as string;
            if (cat == ToolLibCategory.AnchorV2 && !string.IsNullOrEmpty(model))
                item = new ToolMachineEntry { Name = model, Dir = AnchorStore.Dir(model) };
        }

        if (item == null)
        {
            MessageBox.Show(this, "先在列表里选一个机种。", "压缩归档",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var size = ToolsLibrary.FormatSize(ToolsLibrary.DirSize(item.Dir));
        var yes = MessageBox.Show(this,
            $"把机种“{item.Name}”（{size}）压成一个 zip 放进 backup/，然后删掉原目录？\n\n"
            + $"压缩包位置：{ToolsLibrary.ArchiveRoot(cat)}\n"
            + $"累计超过 {ToolsLibrary.ArchiveCapBytes / 1024 / 1024}MB 会自动删最旧的压缩包。",
            "压缩归档", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (yes != DialogResult.Yes) return;

        if (!ToolsLibrary.ArchiveMachine(cat, item.Name, out var zip, out var error))
        {
            MessageBox.Show(this, "归档失败：\n" + error, "压缩归档",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var purged = ToolsLibrary.PurgeArchives(cat);
        _lblStatus.Text = "已归档机种：" + item.Name;
        MessageBox.Show(this,
            "已归档：\n" + zip + (purged > 0 ? $"\n\n超过容量，自动删了 {purged} 个最旧压缩包。" : ""),
            "压缩归档", MessageBoxButtons.OK, MessageBoxIcon.Information);
        AfterMachineChange(cat);
    }

    private void OpenToolsMachineFolder()
    {
        var item = _lvToolsMachines?.SelectedItems.Count > 0
            ? _lvToolsMachines.SelectedItems[0].Tag as ToolMachineEntry
            : null;
        OpenFolder(item?.Dir ?? ToolsLibrary.Root(MachineCategory()), "机种目录");
    }

    private void AfterMachineChange(ToolLibCategory cat)
    {
        try
        {
            if (cat == ToolLibCategory.AnchorV2) RefreshAnchorModels();
            else LoadFixModels();
        }
        catch
        {
            // 刷新失败不影响本次操作
        }

        RefreshToolsPage();
    }

    // ---------------------------------------------------------------- 小工具

    private static ListView MakeToolsList(params (string Text, int Width)[] cols)
    {
        var lv = new ListView
        {
            View = View.Details,
            FullRowSelect = true,
            MultiSelect = false,
            HideSelection = false,
            Dock = DockStyle.Top,
            Height = 150,
            Font = Theme.Body,
            BorderStyle = BorderStyle.FixedSingle,
            BackColor = Theme.Card,
            ForeColor = Theme.Ink,
            Margin = new Padding(0, 0, 0, 8),
        };
        foreach (var (text, width) in cols) lv.Columns.Add(text, width);
        return lv;
    }

    private static FlowLayoutPanel ToolsFlow() => new()
    {
        Dock = DockStyle.Top,
        AutoSize = true,
        WrapContents = true,
        BackColor = Theme.Card,
        Margin = new Padding(0, 0, 0, 6),
    };

    private Button ToolsButton(string text, EventHandler onClick, bool primary, int width)
    {
        var b = primary ? Theme.Primary(text) : Theme.Ghost(text);
        // 固定宽度会把长文案截掉（高 DPI 下更明显），这里只保证最小宽度，实际宽度跟着文字走。
        b.AutoSize = true;
        b.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        b.MinimumSize = new Size(width, 34);
        b.Padding = new Padding(10, 0, 10, 0);
        b.Margin = new Padding(0, 2, 8, 2);
        b.Click += onClick;
        return b;
    }
}
