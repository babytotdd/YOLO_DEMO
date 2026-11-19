using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using DeploySharp.Engine;

namespace YoloFool;

/// <summary>
/// “工具 · 我的模型”页：把训练产物、数据集、模型备份、误检样本集中到一个地方，
/// 让用户不用记路径、不用手动复制 onnx 文件就能用上自己训练的模型。
/// </summary>
public sealed partial class MainForm
{
    private Label _lblToolsModelMain;
    private Label _lblToolsModelMeta;
    private Label _lblToolsModelClasses;
    private Label _lblToolsModelTrain;
    private Label _lblToolsDatasetStats;
    private Label _lblToolsDatasetClasses;
    private Label _lblToolsDatasetExtra;
    private Label _lblToolsLabelsInfo;
    private DataGridView _gridToolsLabels;
    private Label _lblToolsBackupInfo;
    private Label _lblToolsMisdetect;
    private ListBox _lvToolsBackups;
    private Button _btnToolsUseModel;
    private readonly List<string> _toolsBackups = new();

    // ---------------------------------------------------------------- 布局

    private Control BuildToolsPage()
    {
        var root = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 2,
            BackColor = Theme.Bg,
        };
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
        root.Controls.Add(BuildToolsToolbar(), 0, 0);

        var scroll = new Panel
        {
            Dock = DockStyle.Fill,
            AutoScroll = true,
            BackColor = Theme.Bg,
            Margin = Padding.Empty,
        };
        var stack = new TableLayoutPanel
        {
            Dock = DockStyle.Top,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            ColumnCount = 1,
            RowCount = 0,
            BackColor = Theme.Bg,
            Margin = Padding.Empty,
        };
        stack.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
        AddRow(stack, BuildToolsFirstRunCard());
        AddRow(stack, BuildToolsLibraryCard());
        AddRow(stack, BuildToolsModelCard());
        AddRow(stack, BuildToolsModelClassesCard());
        AddRow(stack, BuildToolsAnchorLibraryCard());
        AddRow(stack, BuildToolsMachineCard());
        AddRow(stack, BuildToolsDatasetCard());
        AddRow(stack, BuildToolsLabelCard());
        AddRow(stack, BuildToolsBackupCard());
        AddRow(stack, BuildToolsMisdetectCard());
        scroll.Controls.Add(stack);
        root.Controls.Add(scroll, 0, 1);

        if (_pageTools != null) _pageTools.Enter += (_, _) => RefreshToolsPage();
        RefreshToolsPage();
        return root;
    }

    private Control BuildToolsToolbar()
    {
        var bar = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
            ColumnCount = 3,
            RowCount = 1,
            Margin = new Padding(0, 0, 0, 8),
        };
        bar.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        bar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
        bar.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

        bar.Controls.Add(new Label
        {
            Text = "我的模型",
            Font = Theme.Section,
            ForeColor = Theme.Ink,
            AutoSize = true,
            Anchor = AnchorStyles.Left,
            Margin = new Padding(0, 4, 12, 4),
        }, 0, 0);

        bar.Controls.Add(new Label
        {
            Text = "训练好的模型、数据集、备份和误检样本都在这里，不用记路径。",
            Font = Theme.Small,
            ForeColor = Theme.Muted,
            AutoSize = true,
            Anchor = AnchorStyles.Left,
            Margin = new Padding(0, 7, 12, 4),
        }, 1, 0);

        var btnRefresh = Theme.Ghost("刷新");
        Stretch(btnRefresh, 84, 32);
        btnRefresh.Anchor = AnchorStyles.Right;
        btnRefresh.Margin = new Padding(0, 2, 0, 2);
        btnRefresh.Click += (_, _) => RefreshToolsPage();
        bar.Controls.Add(btnRefresh, 2, 0);
        return bar;
    }

    private Control BuildToolsModelCard()
    {
        var (card, body) = MakeCard("训练模型");
        card.Name = "cardToolsModel";

        _lblToolsModelMain = new Label
        {
            Text = "尚未训练模型",
            Font = Theme.BodyBold,
            ForeColor = Theme.Ink,
            AutoSize = true,
            Margin = new Padding(0, 0, 0, 6),
        };
        AddRow(body, _lblToolsModelMain);

        _lblToolsModelMeta = Theme.Hint("");
        AddRow(body, _lblToolsModelMeta);

        _lblToolsModelClasses = Theme.Hint("");
        AddRow(body, _lblToolsModelClasses);

        _lblToolsModelTrain = Theme.Hint("");
        AddRow(body, _lblToolsModelTrain);

        var flow = new FlowLayoutPanel
        {
            Dock = DockStyle.Top,
            AutoSize = true,
            WrapContents = false,
            BackColor = Theme.Card,
            Margin = new Padding(0, 8, 0, 0),
        };
        _btnToolsUseModel = Theme.Primary("用这个模型识别");
        _btnToolsUseModel.Name = "btnToolsUseModel";
        Stretch(_btnToolsUseModel, 140, 34);
        _btnToolsUseModel.Margin = new Padding(0, 2, 8, 2);
        _btnToolsUseModel.Click += async (_, _) => await UseTrainedModelAsync(true);
        flow.Controls.Add(_btnToolsUseModel);

        var btnBackup = Theme.Ghost("备份当前模型");
        Stretch(btnBackup, 120, 34);
        btnBackup.Margin = new Padding(0, 2, 8, 2);
        btnBackup.Click += (_, _) => BackupModelInteractive();
        flow.Controls.Add(btnBackup);

        var btnOpen = Theme.Ghost("打开模型文件夹");
        Stretch(btnOpen, 130, 34);
        btnOpen.Margin = new Padding(0, 2, 8, 2);
        btnOpen.Click += (_, _) => OpenFolder(AppPaths.ModelsDir, "模型文件夹");
        flow.Controls.Add(btnOpen);

        AddRow(body, flow);
        return card;
    }

    private Control BuildToolsDatasetCard()
    {
        var (card, body) = MakeCard("数据集");
        card.Name = "cardToolsDataset";

        _lblToolsDatasetStats = new Label
        {
            Text = "数据集：0 张图片 · 0 个框 · 0 类",
            Font = Theme.Body,
            ForeColor = Theme.Ink,
            AutoSize = true,
            Margin = new Padding(0, 0, 0, 6),
        };
        AddRow(body, _lblToolsDatasetStats);

        _lblToolsDatasetClasses = Theme.Hint("");
        AddRow(body, _lblToolsDatasetClasses);

        _lblToolsDatasetExtra = Theme.Hint("");
        AddRow(body, _lblToolsDatasetExtra);

        var flow = new FlowLayoutPanel
        {
            Dock = DockStyle.Top,
            AutoSize = true,
            WrapContents = false,
            BackColor = Theme.Card,
            Margin = new Padding(0, 8, 0, 0),
        };

        var btnRebuild = Theme.Ghost("重建训练集");
        Stretch(btnRebuild, 120, 34);
        btnRebuild.Margin = new Padding(0, 2, 8, 2);
        btnRebuild.Click += (_, _) => RebuildTrainingSet();
        flow.Controls.Add(btnRebuild);

        var btnOpen = Theme.Ghost("打开数据集文件夹");
        Stretch(btnOpen, 140, 34);
        btnOpen.Margin = new Padding(0, 2, 8, 2);
        btnOpen.Click += (_, _) => OpenFolder(AppPaths.DatasetDir, "数据集文件夹");
        flow.Controls.Add(btnOpen);

        AddRow(body, flow);
        return card;
    }

    private Control BuildToolsLabelCard()
    {
        var (card, body) = MakeCard("标签管理");
        card.Name = "cardToolsLabels";

        _lblToolsLabelsInfo = new Label
        {
            Text = "标签：还没有类别",
            Font = Theme.Body,
            ForeColor = Theme.Ink,
            AutoSize = true,
            Margin = new Padding(0, 0, 0, 6),
        };
        AddRow(body, _lblToolsLabelsInfo);

        _gridToolsLabels = new DataGridView
        {
            Name = "gridToolsLabels",
            Dock = DockStyle.Top,
            Height = 148,
            Margin = new Padding(0, 0, 0, 6),
            TabIndex = 0,
        };
        Theme.StyleGrid(_gridToolsLabels);
        _gridToolsLabels.Columns.Add(new DataGridViewTextBoxColumn { Name = "colLabelName", HeaderText = "标签", FillWeight = 28 });
        _gridToolsLabels.Columns.Add(new DataGridViewTextBoxColumn { Name = "colLabelEn", HeaderText = "存储名（训练用）", FillWeight = 30 });
        _gridToolsLabels.Columns.Add(new DataGridViewTextBoxColumn { Name = "colLabelBoxes", HeaderText = "框数", FillWeight = 14 });
        _gridToolsLabels.Columns.Add(new DataGridViewTextBoxColumn { Name = "colLabelImages", HeaderText = "图片数", FillWeight = 14 });
        _gridToolsLabels.Columns.Add(new DataGridViewTextBoxColumn { Name = "colLabelPending", HeaderText = "待复核", FillWeight = 14 });
        _gridToolsLabels.CellDoubleClick += (_, e) =>
        {
            if (e.RowIndex >= 0) OpenLabelDialog(RowLabelEn(e.RowIndex), false);
        };
        AddRow(body, _gridToolsLabels);

        AddRow(body, Theme.Hint("删标签会把对应的标注一起从数据集里删掉（删前自动备份 annotations.json / classes.txt），当前模型不变；想让它立刻不再显示，在弹窗里勾选“同时屏蔽”。"));

        var flow = new FlowLayoutPanel
        {
            Dock = DockStyle.Top,
            AutoSize = true,
            WrapContents = false,
            BackColor = Theme.Card,
            Margin = new Padding(0, 8, 0, 0),
        };

        var btnView = Theme.Primary("查看训练内容");
        Stretch(btnView, 140, 34);
        btnView.Margin = new Padding(0, 2, 8, 2);
        btnView.Click += (_, _) => ViewSelectedLabel();
        flow.Controls.Add(btnView);

        var btnDelete = Theme.Ghost("删除这个标签");
        Stretch(btnDelete, 132, 34);
        btnDelete.ForeColor = Theme.Danger;
        btnDelete.Margin = new Padding(0, 2, 8, 2);
        btnDelete.Click += (_, _) => DeleteSelectedLabel();
        flow.Controls.Add(btnDelete);

        AddRow(body, flow);
        return card;
    }

    private Control BuildToolsBackupCard()
    {
        var (card, body) = MakeCard("模型备份");
        card.Name = "cardToolsBackup";

        _lvToolsBackups = new ListBox
        {
            Name = "lvToolsBackups",
            Dock = DockStyle.Top,
            Height = 110,
            Font = Theme.Mono,
            BorderStyle = BorderStyle.FixedSingle,
            IntegralHeight = false,
            Margin = new Padding(0, 0, 0, 6),
        };
        AddRow(body, _lvToolsBackups);

        _lblToolsBackupInfo = Theme.Hint("");
        AddRow(body, _lblToolsBackupInfo);

        var flow = new FlowLayoutPanel
        {
            Dock = DockStyle.Top,
            AutoSize = true,
            WrapContents = false,
            BackColor = Theme.Card,
            Margin = new Padding(0, 8, 0, 0),
        };

        var btnRestore = Theme.Ghost("恢复选中备份");
        Stretch(btnRestore, 130, 34);
        btnRestore.Margin = new Padding(0, 2, 8, 2);
        btnRestore.Click += (_, _) => RestoreBackupInteractive();
        flow.Controls.Add(btnRestore);

        var btnDelete = Theme.Ghost("删除选中备份");
        Stretch(btnDelete, 130, 34);
        btnDelete.Margin = new Padding(0, 2, 8, 2);
        btnDelete.Click += (_, _) => DeleteBackupInteractive();
        flow.Controls.Add(btnDelete);

        var btnOpen = Theme.Ghost("打开备份文件夹");
        Stretch(btnOpen, 140, 34);
        btnOpen.Margin = new Padding(0, 2, 8, 2);
        btnOpen.Click += (_, _) => OpenFolder(AppPaths.ModelBackupDir, "备份文件夹");
        flow.Controls.Add(btnOpen);

        AddRow(body, flow);
        return card;
    }

    private Control BuildToolsMisdetectCard()
    {
        var (card, body) = MakeCard("误检样本");
        card.Name = "cardToolsMisdetect";

        _lblToolsMisdetect = Theme.Hint("");
        AddRow(body, _lblToolsMisdetect);

        var flow = new FlowLayoutPanel
        {
            Dock = DockStyle.Top,
            AutoSize = true,
            WrapContents = false,
            BackColor = Theme.Card,
            Margin = new Padding(0, 8, 0, 0),
        };

        var btnOpen = Theme.Ghost("打开误检文件夹");
        Stretch(btnOpen, 140, 34);
        btnOpen.Margin = new Padding(0, 2, 8, 2);
        btnOpen.Click += (_, _) => OpenFolder(AppPaths.MisdetectDir, "误检文件夹");
        flow.Controls.Add(btnOpen);

        var btnClear = Theme.Ghost("清空误检样本");
        Stretch(btnClear, 130, 34);
        btnClear.Margin = new Padding(0, 2, 8, 2);
        btnClear.Click += (_, _) => ClearMisdetectSamples();
        flow.Controls.Add(btnClear);

        AddRow(body, flow);
        return card;
    }

    // ---------------------------------------------------------------- 刷新

    private void RefreshToolsPage()
    {
        if (_lblToolsModelMain == null || IsDisposed) return;
        RefreshToolsModel();
        RefreshToolsDataset();
        RefreshToolsLabels();
        RefreshToolsModelClasses();
        RefreshToolsBackups();
        RefreshToolsMisdetect();
        RefreshToolsLibrary();
        RefreshToolsAnchorLibrary();
        RefreshToolsMachines();
    }

    private void RefreshToolsModel()
    {
        var path = AppPaths.MyModelOnnx;
        if (!File.Exists(path))
        {
            _lblToolsModelMain.Text = "尚未训练模型：my_models/mymodel.onnx 不存在";
            _lblToolsModelMain.ForeColor = Theme.Danger;
            _lblToolsModelMeta.Text = "先到“学习训练”页标注图片并训练，训练完成后会自动生成模型。";
            _lblToolsModelClasses.Text = "";
            _lblToolsModelTrain.Text = "";
            if (_btnToolsUseModel != null) _btnToolsUseModel.Enabled = false;
            return;
        }

        if (_btnToolsUseModel != null) _btnToolsUseModel.Enabled = true;

        var kind = OnnxInspector.DetectKind(path);
        FileInfo fi;
        try { fi = new FileInfo(path); } catch { fi = null; }
        _lblToolsModelMain.Text = $"训练模型：{Path.GetFileName(path)}（{OnnxInspector.Describe(kind)}模型）";
        _lblToolsModelMain.ForeColor = Theme.Ink;
        _lblToolsModelMeta.Text = fi == null
            ? "路径：" + path
            : $"路径：{fi.FullName}\n大小：{fi.Length / 1024.0 / 1024.0:F1} MB · 修改：{fi.LastWriteTime:yyyy-MM-dd HH:mm}";

        var info = ReadModelInfo();
        var classes = info.Classes != null && info.Classes.Count > 0
            ? info.Classes
            : (_svc?.Info?.ClassNames?.Where(x => !string.IsNullOrWhiteSpace(x)).ToList() ?? new List<string>());
        _lblToolsModelClasses.Text = classes.Count == 0
            ? "类别：未知（模型里没有类别名，也没有 model_info.json）"
            : $"类别：{string.Join(" / ", classes)}（共 {classes.Count} 类）";

        var parts = new List<string>();
        if (info.Epochs > 0) parts.Add($"训练 {info.Epochs} 轮");
        if (!string.IsNullOrWhiteSpace(info.Created)) parts.Add("生成 " + info.Created);
        if (!string.IsNullOrWhiteSpace(info.Source)) parts.Add("来源 " + info.Source);
        _lblToolsModelTrain.Text = parts.Count == 0
            ? "训练记录：没有 model_info.json，无法显示训练轮数与来源。"
            : "训练记录：" + string.Join(" · ", parts);
    }

    private void RefreshToolsDataset()
    {
        if (_lblToolsDatasetStats == null) return;
        _lblToolsDatasetStats.Text = $"数据集：{_store.ImageCount} 张图片 · {_store.BoxCount} 个框 · {_store.Classes.Count} 类";
        IReadOnlyDictionary<string, int> counts = null;
        try { counts = _store.ClassCounts(); } catch { }
        _lblToolsDatasetClasses.Text = counts == null || counts.Count == 0
            ? "类别分布：还没有标注框。"
            : "类别分布：" + string.Join("，", counts.OrderByDescending(kv => kv.Value).Select(kv => $"{kv.Key}×{kv.Value}"));
    }

    private void RefreshToolsBackups()
    {
        if (_lvToolsBackups == null) return;
        _lvToolsBackups.Items.Clear();
        _toolsBackups.Clear();

        if (!Directory.Exists(AppPaths.ModelBackupDir))
        {
            _lblToolsBackupInfo.Text = "还没有备份。每次训练成功后，程序会自动把上一版模型备份到这里。";
            return;
        }

        try
        {
            foreach (var f in new DirectoryInfo(AppPaths.ModelBackupDir)
                         .GetFiles("*.onnx")
                         .OrderByDescending(x => x.LastWriteTime)
                         .Take(50))
            {
                _toolsBackups.Add(f.FullName);
                _lvToolsBackups.Items.Add($"{f.Name}   {f.Length / 1024.0 / 1024.0:F1} MB   {f.LastWriteTime:MM-dd HH:mm}");
            }
        }
        catch (Exception ex)
        {
            _lblToolsBackupInfo.Text = "读取备份失败：" + ex.Message;
            return;
        }

        _lblToolsBackupInfo.Text = _lvToolsBackups.Items.Count == 0
            ? "还没有备份。每次训练成功后，程序会自动把上一版模型备份到这里。"
            : $"共 {_lvToolsBackups.Items.Count} 个备份（最新在上）。恢复前会先把当前模型再备份一次，不会丢东西。";
    }

    private void RefreshToolsMisdetect()
    {
        if (_lblToolsMisdetect == null) return;
        var n = CountMisdetectSamples();
        _lblToolsMisdetect.Text = n == 0
            ? "还没有误检样本。识别时选中结果点“删除选中”，这张图会被存下来，方便以后补标。"
            : $"已收集 {n} 条误检记录（图片副本 + misdetect.jsonl），可以拿去补标后重新训练。";
    }

    private void RefreshToolsLabels()
    {
        if (_gridToolsLabels == null || IsDisposed) return;

        var rows = new List<ClassStat>();
        try { rows = _store.ClassStats().ToList(); }
        catch (Exception ex) { Log.Warn("读取标签列表失败：" + ex.Message); }

        _gridToolsLabels.Rows.Clear();
        foreach (var s in rows)
        {
            int i = _gridToolsLabels.Rows.Add(
                s.Cn,
                s.En,
                s.Boxes,
                s.Images,
                s.Pending > 0 ? s.Pending.ToString() : "-");
            var row = _gridToolsLabels.Rows[i];
            row.Tag = s.En;
            row.Cells[1].Style.Font = Theme.Mono;
            if (s.Boxes == 0) row.Cells[2].Style.ForeColor = Theme.Muted;
            if (s.Pending > 0) row.Cells[4].Style.ForeColor = Theme.Warn;
        }

        int muted = 0;
        try { muted = _settings.MutedClasses?.Count ?? 0; } catch { }
        _lblToolsLabelsInfo.Text = rows.Count == 0
            ? "标签：还没有类别。到“学习训练”页框选目标并命名，标签就会出现在这里。"
            : $"标签：共 {rows.Count} 个 · {rows.Sum(r => r.Boxes)} 个框 · {rows.Sum(r => r.Images)} 张图（同一张图可能有多个标签）"
              + (muted > 0 ? $" · 已屏蔽 {muted} 类" : "");
    }

    private string RowLabelEn(int rowIndex)
    {
        if (_gridToolsLabels == null || rowIndex < 0 || rowIndex >= _gridToolsLabels.Rows.Count) return null;
        return _gridToolsLabels.Rows[rowIndex].Tag as string;
    }

    /// <summary>存储名 → 界面上显示的中文标签名（读 labels_map.json，没映射就原样显示）。</summary>
    private string DisplayLabelName(string en)
    {
        if (string.IsNullOrWhiteSpace(en)) return "";
        try
        {
            int id = -1;
            for (int i = 0; i < _store.Classes.Count; i++)
            {
                if (!string.Equals(_store.Classes[i], en, StringComparison.OrdinalIgnoreCase)) continue;
                id = i;
                break;
            }
            return id < 0 ? en : _store.Labels.Display(en, id);
        }
        catch { return en; }
    }

    private bool IsLabelMuted(string displayName) =>
        !string.IsNullOrWhiteSpace(displayName) &&
        (_settings.MutedClasses?.Contains(displayName, StringComparer.OrdinalIgnoreCase) ?? false);

    private void ViewSelectedLabel()
    {
        var en = RowLabelEn(_gridToolsLabels?.CurrentRow?.Index ?? -1);
        if (string.IsNullOrEmpty(en))
        {
            MessageBox.Show(this, "先在标签列表里选中一个标签。", "标签管理",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        OpenLabelDialog(en, false);
    }

    private void DeleteSelectedLabel()
    {
        var en = RowLabelEn(_gridToolsLabels?.CurrentRow?.Index ?? -1);
        if (string.IsNullOrEmpty(en))
        {
            MessageBox.Show(this, "先在标签列表里选中要删的标签。", "标签管理",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        OpenLabelDialog(en, true);
    }

    /// <summary>标签详情窗口：看训练内容 + 缩略图，底部可以删掉这个标签。</summary>
    private void OpenLabelDialog(string en, bool focusDelete)
    {
        if (string.IsNullOrWhiteSpace(en)) return;
        var cn = DisplayLabelName(en);
        using var dlg = new LabelDetailDialog(_store, en, cn, IsLabelMuted(cn));
        var r = dlg.ShowDialog(this);
        if (dlg.Deleted) AfterLabelRemoved(en, dlg.Muted);
        else if (focusDelete && r != DialogResult.OK)
            _lblStatus.Text = $"已取消删除标签“{cn}”，数据集没有改动。";
    }

    /// <summary>删完标签后的收尾：可选屏蔽、刷新界面、重建训练集、把结果交代清楚。</summary>
    private void AfterLabelRemoved(string en, bool mute)
    {
        var cn = DisplayLabelName(en);
        if (mute) MuteClass(cn);

        try { RefreshDatasetStats(); } catch { }
        RefreshToolsPage();
        RefreshFilterBadge();
        if (_lastResult != null && !VideoRunning) ShowDetections(_lastResult, _recPath);

        var rebuilt = "";
        try
        {
            var set = _store.BuildTrainingView();
            _lblToolsDatasetExtra.Text = $"训练集：{set.TrainCount} 张训练 / {set.ValCount} 张验证 · {set.ClassCount} 类 → {set.DataYaml}";
            rebuilt = "训练集已同步重建。";
        }
        catch (Exception ex)
        {
            _lblToolsDatasetExtra.Text = "训练集还没重建：" + ex.Message;
            rebuilt = "训练集需要你手动点“重建训练集”。";
        }

        _lblStatus.Text = $"已删除标签“{cn}”";
        MessageBox.Show(this,
            $"已从数据集删除标签“{cn}”。\n\n{rebuilt}\n\n备份在：\n{AppPaths.DatasetBackupDir}\n\n当前模型文件没有变，重新训练之后它才会彻底不再出现这个类。"
            + (mute ? "\n\n已加入屏蔽名单：现在的模型立刻不再显示这个类。" : ""),
            "标签管理", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void MuteClass(string displayName)
    {
        if (string.IsNullOrWhiteSpace(displayName)) return;
        _settings.MutedClasses ??= new List<string>();
        if (!_settings.MutedClasses.Contains(displayName, StringComparer.OrdinalIgnoreCase))
            _settings.MutedClasses.Add(displayName);
        _settings.Save();
        Log.Info("屏蔽类别：" + displayName);
    }

    // ---------------------------------------------------------------- 操作

    private async Task UseTrainedModelAsync(bool announce)
    {
        var path = AppPaths.MyModelOnnx;
        if (!File.Exists(path))
        {
            MessageBox.Show(this, "还没有训练好的模型。\n先到“学习训练”页标注图片并训练一次。",
                "我的模型", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var kind = OnnxInspector.DetectKind(path) ?? YoloModelKind.YOLOv8Det;
        var ok = await LoadModelFileAsync(path, kind, announce);
        if (ok && _tabs != null && _pageRec != null) _tabs.SelectedTab = _pageRec;
    }

    private async Task<bool> LoadModelFileAsync(string path, YoloModelKind kind, bool announce)
    {
        if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
        {
            MessageBox.Show(this, "模型文件不存在：\n" + path, "加载模型",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        var opt = _settings.ToModelOptions() with
        {
            ModelPath = path,
            Kind = kind,
            Device = DeviceType.CPU,
            Provider = OnnxRuntimeDeviceType.Default,
        };

        SetBusy(true);
        _lblStatus.Text = "正在加载模型…";
        try
        {
            var (ok, svc, info, error) = await Task.Run(() =>
            {
                InferenceService s = null;
                try
                {
                    s = new InferenceService();
                    var i = s.Load(opt);
                    s.Warmup();
                    return (true, s, i, "");
                }
                catch (Exception ex)
                {
                    s?.Dispose();
                    return (false, (InferenceService)null, (ModelRuntimeInfo)null, ex.Message);
                }
            });

            if (!ok)
            {
                _lblStatus.Text = "加载模型失败";
                MessageBox.Show(this, "加载模型失败，当前模型保持不变。\n\n" + error,
                    "加载模型", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            var old = _svc;
            _svc = svc;
            old?.Dispose();

            _settings.Apply(opt);
            _settings.Save();

            RefreshAdvancedFromSettings();
            UpdateModelStatus(true, $"{ProviderLabel(info.Provider)} · {info.ClassCount} 类", "已切换到训练模型");
            UpdateRunEnabled();
            RefreshToolsPage();
            _lblStatus.Text = "已加载模型：" + Path.GetFileName(path);

            if (announce)
                MessageBox.Show(this,
                    $"已切换到训练模型，可以直接识别了。\n\n模型：{Path.GetFileName(path)}\n类别数：{info.ClassCount}\n设备：{ProviderLabel(info.Provider)}",
                    "我的模型", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return true;
        }
        finally
        {
            SetBusy(false);
            UpdateRunEnabled();
        }
    }

    private string BackupCurrentModel()
    {
        var src = AppPaths.MyModelOnnx;
        if (!File.Exists(src)) return null;
        Directory.CreateDirectory(AppPaths.ModelBackupDir);
        var stamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        var dst = Path.Combine(AppPaths.ModelBackupDir, $"mymodel_{stamp}.onnx");
        var n = 1;
        while (File.Exists(dst))
            dst = Path.Combine(AppPaths.ModelBackupDir, $"mymodel_{stamp}_{n++}.onnx");
        File.Copy(src, dst);
        Log.Info("备份模型：" + dst);
        return dst;
    }

    private void BackupModelInteractive()
    {
        try
        {
            var dst = BackupCurrentModel();
            if (string.IsNullOrEmpty(dst))
            {
                MessageBox.Show(this, "还没有训练好的模型可以备份。", "备份模型",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            RefreshToolsPage();
            _lblStatus.Text = "已备份模型：" + Path.GetFileName(dst);
            MessageBox.Show(this, "已备份到：\n" + dst, "备份模型",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, "备份失败：\n" + ex.Message, "备份模型",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void RestoreBackupInteractive()
    {
        var i = _lvToolsBackups?.SelectedIndex ?? -1;
        if (i < 0 || i >= _toolsBackups.Count)
        {
            MessageBox.Show(this, "先在列表里选中一个备份。", "恢复备份",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var src = _toolsBackups[i];
        var yes = MessageBox.Show(this,
            $"用这个备份替换当前模型？\n\n备份：{Path.GetFileName(src)}\n当前：{Path.GetFileName(AppPaths.MyModelOnnx)}\n\n替换前会先把当前模型再备份一次。",
            "恢复备份", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (yes != DialogResult.Yes) return;

        try
        {
            var safety = BackupCurrentModel();
            Directory.CreateDirectory(AppPaths.ModelsDir);
            File.Copy(src, AppPaths.MyModelOnnx, true);
            Log.Info($"恢复模型备份：{src}（当前模型已备份到 {safety}）");
            RefreshToolsPage();
            _lblStatus.Text = "已恢复模型备份：" + Path.GetFileName(src);

            if (_svc?.Info != null &&
                string.Equals(_svc.Info.ModelPath, AppPaths.MyModelOnnx, StringComparison.OrdinalIgnoreCase))
            {
                var kind = OnnxInspector.DetectKind(AppPaths.MyModelOnnx) ?? YoloModelKind.YOLOv8Det;
                _ = LoadModelFileAsync(AppPaths.MyModelOnnx, kind, false);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, "恢复失败：\n" + ex.Message, "恢复备份",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void DeleteBackupInteractive()
    {
        var i = _lvToolsBackups?.SelectedIndex ?? -1;
        if (i < 0 || i >= _toolsBackups.Count)
        {
            MessageBox.Show(this, "先在列表里选中一个备份。", "删除备份",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var path = _toolsBackups[i];
        var yes = MessageBox.Show(this, "删除这个备份文件？\n\n" + Path.GetFileName(path),
            "删除备份", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (yes != DialogResult.Yes) return;

        try
        {
            File.Delete(path);
            Log.Info("删除模型备份：" + path);
            RefreshToolsPage();
            _lblStatus.Text = "已删除备份：" + Path.GetFileName(path);
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, "删除失败：\n" + ex.Message, "删除备份",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void RebuildTrainingSet()
    {
        try
        {
            var set = _store.BuildTrainingView();
            RefreshDatasetStats();
            RefreshToolsDataset();
            _lblToolsDatasetExtra.Text = $"训练集：{set.TrainCount} 张训练 / {set.ValCount} 张验证 · {set.ClassCount} 类 → {set.DataYaml}";
            _lblStatus.Text = $"训练集已重建：{set.TrainCount} 训练 / {set.ValCount} 验证";
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, "重建训练集失败：\n" + ex.Message, "数据集",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void ClearMisdetectSamples()
    {
        var dir = AppPaths.MisdetectDir;
        if (!Directory.Exists(dir))
        {
            MessageBox.Show(this, "还没有误检样本。", "误检样本",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var n = CountMisdetectSamples();
        var yes = MessageBox.Show(this, $"清空 {n} 条误检记录和对应图片副本？\n\n目录：{dir}",
            "清空误检样本", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (yes != DialogResult.Yes) return;

        try
        {
            foreach (var f in Directory.GetFiles(dir)) File.Delete(f);
            Log.Info("清空误检样本：" + dir);
            RefreshToolsPage();
            _lblStatus.Text = "已清空误检样本。";
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, "清空失败：\n" + ex.Message, "误检样本",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void OpenFolder(string dir, string what)
    {
        try
        {
            Directory.CreateDirectory(dir);
            if (!ShellFolder.TryOpen(dir, out var error))
            {
                MessageBox.Show(this, $"无法打开{what}：\n{dir}\n\n{error}", "打开文件夹",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            _lblStatus.Text = "已打开" + what + "：" + dir;
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, $"打开{what}失败：\n{dir}\n\n{ex.Message}", "打开文件夹",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    // ---------------------------------------------------------------- 小工具

    private static (string Created, string Source, int Epochs, List<string> Classes) ReadModelInfo()
    {
        try
        {
            var path = AppPaths.ModelInfoFile;
            if (!File.Exists(path)) return (null, null, 0, null);

            using var doc = JsonDocument.Parse(File.ReadAllText(path));
            var r = doc.RootElement;

            string Get(string name) =>
                r.TryGetProperty(name, out var v) && v.ValueKind == JsonValueKind.String ? v.GetString() : null;

            var epochs = r.TryGetProperty("epochs", out var e) && e.ValueKind == JsonValueKind.Number
                ? e.GetInt32()
                : 0;

            var classes = new List<string>();
            if (r.TryGetProperty("classes", out var c) && c.ValueKind == JsonValueKind.Array)
            {
                foreach (var item in c.EnumerateArray())
                    if (item.ValueKind == JsonValueKind.String) classes.Add(item.GetString());
            }

            return (Get("created"), Get("source"), epochs, classes);
        }
        catch
        {
            return (null, null, 0, null);
        }
    }

    private static int CountMisdetectSamples()
    {
        var file = Path.Combine(AppPaths.MisdetectDir, "misdetect.jsonl");
        if (!File.Exists(file)) return 0;
        try { return File.ReadLines(file).Count(l => !string.IsNullOrWhiteSpace(l)); }
        catch { return 0; }
    }
}
