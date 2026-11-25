using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YoloFool;

/// <summary>
/// “工具 · 我的模型”页里的“模型标签（识别用）”卡：
/// 把当前 ONNX 内嵌的类别和数据集里的类别对照着列出来，
/// 并提供改显示名 / 改存储名 / 从数据集删除 / 加入屏蔽 / 再训练并部署。
/// </summary>
public sealed partial class MainForm
{
    /// <summary>表格里的一行：可能来自模型内嵌类别，也可能来自数据集（或两边都有）。</summary>
    private sealed record ToolsModelClassRow(
        string En, string Cn, int Id, bool InModel, bool InDataset,
        int Boxes, int Images, int Pending)
    {
        public string StatusText
        {
            get
            {
                if (InModel && InDataset) return "一致";
                if (InModel) return "模型有 · 数据集没有（重训后会消失）";
                return "数据集有 · 模型没有（重训后才会识别）";
            }
        }
    }

    private bool _toolsMcBusy;
    private readonly List<ToolsModelClassRow> _toolsMcRows = new();

    // ---------------------------------------------------------------- 布局


    // ---------------------------------------------------------------- 刷新

    private void RefreshToolsModelClasses()
    {
        if (_gridToolsMc == null || IsDisposed) return;

        _toolsMcRows.Clear();
        var modelClasses = ToolsModelClasses(out var modelHead);

        var stats = new List<ClassStat>();
        try { stats = _store.ClassStats().ToList(); }
        catch (Exception ex) { Log.Error("读取标签统计失败", ex); }

        var byEn = new Dictionary<string, ClassStat>(StringComparer.OrdinalIgnoreCase);
        foreach (var s in stats)
        {
            if (s == null || string.IsNullOrWhiteSpace(s.En) || byEn.ContainsKey(s.En)) continue;
            byEn[s.En] = s;
        }

        var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        for (int i = 0; i < modelClasses.Count; i++)
        {
            var en = (modelClasses[i] ?? "").Trim();
            if (en.Length == 0 || !seen.Add(en)) continue;
            byEn.TryGetValue(en, out var st);
            _toolsMcRows.Add(new ToolsModelClassRow(
                en,
                st?.Cn ?? SafeDisplayName(en, i),
                st?.Id ?? i,
                true,
                st != null,
                st?.Boxes ?? 0,
                st?.Images ?? 0,
                st?.Pending ?? 0));
        }
        foreach (var st in stats)
        {
            if (st == null || string.IsNullOrWhiteSpace(st.En) || !seen.Add(st.En)) continue;
            _toolsMcRows.Add(new ToolsModelClassRow(st.En, st.Cn, st.Id, false, true,
                st.Boxes, st.Images, st.Pending));
        }

        _gridToolsMc.Rows.Clear();
        foreach (var row in _toolsMcRows)
        {
            var cn = string.IsNullOrWhiteSpace(row.Cn) ? row.En : row.Cn;
            var status = IsLabelMuted(cn) ? row.StatusText + " · 已屏蔽" : row.StatusText;
            int index = _gridToolsMc.Rows.Add(
                row.Id >= 0 ? row.Id.ToString() : "—",
                row.InModel ? row.En : "—",
                cn,
                row.Boxes.ToString(),
                row.Images.ToString(),
                row.Pending > 0 ? row.Pending.ToString() : "",
                status);
            var gridRow = _gridToolsMc.Rows[index];
            gridRow.Tag = row;
            gridRow.DefaultCellStyle.ForeColor = !row.InDataset
                ? Theme.Danger
                : (!row.InModel ? Theme.Warn : Theme.Ink);
        }

        int modelOnly = _toolsMcRows.Count(r => r.InModel && !r.InDataset);
        int datasetOnly = _toolsMcRows.Count(r => !r.InModel && r.InDataset);
        var extra = new List<string>();
        if (modelOnly > 0) extra.Add("模型多出 " + modelOnly + " 类");
        if (datasetOnly > 0) extra.Add("数据集多出 " + datasetOnly + " 类");

        _lblToolsMcInfo.Text = modelHead + " · 数据集 " + stats.Count + " 类"
            + (extra.Count > 0 ? "（" + string.Join(" · ", extra) + "）" : "");
        _lblToolsMcInfo.ForeColor = modelOnly > 0 ? Theme.Warn : Theme.Ink;

        if (_gridToolsMc.Rows.Count > 0)
        {
            _gridToolsMc.CurrentCell = _gridToolsMc.Rows[0].Cells[0];
            _gridToolsMc.Rows[0].Selected = true;
        }

        SyncToolsMcButtons();
    }

    /// <summary>当前 ONNX 里内嵌的类别名；取不到就返回空表，并在 head 里说明原因。</summary>
    private List<string> ToolsModelClasses(out string head)
    {
        var path = AppPaths.MyModelOnnx;
        if (!File.Exists(path))
        {
            head = "尚未训练模型：my_models/mymodel.onnx 不存在（只能管理数据集标签）";
            return new List<string>();
        }

        var info = ReadModelInfo();
        var classes = info.Classes != null && info.Classes.Count > 0
            ? info.Classes
            : (_svc?.Info?.ClassNames?.Where(x => !string.IsNullOrWhiteSpace(x)).ToList() ?? new List<string>());

        var parts = new List<string> { Path.GetFileName(path), classes.Count + " 类" };
        if (info.Epochs > 0) parts.Add("训练 " + info.Epochs + " 轮");
        if (!string.IsNullOrWhiteSpace(info.Created)) parts.Add(info.Created + " 生成");
        head = "当前模型：" + string.Join(" · ", parts);
        if (classes.Count == 0) head += "（模型里没有类别名）";
        return classes;
    }

    private string SafeDisplayName(string en, int id)
    {
        try { return _store.Labels.Display(en, id); }
        catch { return en; }
    }

    private ToolsModelClassRow SelectedToolsModelClass() =>
        _gridToolsMc?.CurrentRow?.Tag as ToolsModelClassRow;

    private void SyncToolsMcButtons()
    {
        var row = SelectedToolsModelClass();
        if (_btnToolsMcRename != null) _btnToolsMcRename.Enabled = !_toolsMcBusy && row != null;
        if (_btnToolsMcDelete != null) _btnToolsMcDelete.Enabled = !_toolsMcBusy && row != null && row.InDataset;
        if (_btnToolsMcMute != null) _btnToolsMcMute.Enabled = !_toolsMcBusy && row != null && !IsLabelMuted(row.Cn);
        if (_btnToolsMcTrain != null) _btnToolsMcTrain.Enabled = !_toolsMcBusy && _store.BoxCount > 0;
    }

    private void SetToolsMcBusy(bool busy)
    {
        _toolsMcBusy = busy;
        if (_gridToolsMc != null) _gridToolsMc.Enabled = !busy;
        SyncToolsMcButtons();
        Cursor = busy ? Cursors.WaitCursor : Cursors.Default;
    }

    // ---------------------------------------------------------------- 操作

    private void RenameToolsModelClass()
    {
        if (_toolsMcBusy) return;
        var row = SelectedToolsModelClass();
        if (row == null)
        {
            _lblStatus.Text = "先在“模型标签”表格里选中一个标签。";
            return;
        }

        using var dlg = new ToolsClassRenameDialog(row.En, row.Cn, row.InDataset);
        if (dlg.ShowDialog(this) != DialogResult.OK) return;

        try
        {
            if (dlg.ChangeEnglish)
                _store.RenameClass(row.En, dlg.StorageName, dlg.ChangeDisplay ? dlg.DisplayName : null);
            else if (dlg.ChangeDisplay)
            {
                _store.Labels.SetDisplay(row.En, dlg.DisplayName);
                _store.Labels.Save();
            }
            else return;

            RefreshToolsPage();
            try { RefreshDatasetStats(); } catch { }

            var shown = dlg.ChangeDisplay ? dlg.DisplayName : row.Cn;
            _lblStatus.Text = "已修改标签：" + shown;
            var msg = "标签“" + row.Cn + "”已改。";
            if (dlg.ChangeEnglish)
                msg += "\n\n数据集（classes.txt、标注框、标签文件）已经同步；当前模型文件没变，重新训练之后模型里才会认新名字。";
            MessageBox.Show(this, msg, "修改标签", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, "修改失败：\n" + ex.Message, "修改标签",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void DeleteToolsModelClass()
    {
        if (_toolsMcBusy) return;
        var row = SelectedToolsModelClass();
        if (row == null || !row.InDataset) return;

        var yes = MessageBox.Show(this,
            "从数据集里删掉“" + row.Cn + "”（" + row.En + "）？\n\n"
            + "  · " + row.Boxes + " 个框、" + row.Images + " 张图里的这个类会被删除\n"
            + "  · 删前自动备份 annotations.json / classes.txt 到 mydataset/backup\n"
            + "  · 当前模型文件不变，重训后这个类才会彻底消失\n\n继续？",
            "从数据集删除标签", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (yes != DialogResult.Yes) return;

        try
        {
            _store.RemoveClass(row.En);
            AfterLabelRemoved(row.En, false);
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, "删除失败：\n" + ex.Message, "从数据集删除标签",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void MuteToolsModelClass()
    {
        if (_toolsMcBusy) return;
        var row = SelectedToolsModelClass();
        if (row == null) return;

        var cn = string.IsNullOrWhiteSpace(row.Cn) ? row.En : row.Cn;
        if (IsLabelMuted(cn))
        {
            _lblStatus.Text = "“" + cn + "”已经在屏蔽名单里，不用重复加。";
            return;
        }

        var yes = MessageBox.Show(this,
            "把“" + cn + "”加入屏蔽名单？\n\n屏蔽后程序不再把它的识别结果画到画面上（模型文件、数据集都不动），随时可以取消。",
            "加入屏蔽", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (yes != DialogResult.Yes) return;

        MuteClass(cn);
        RefreshFilterBadge();
        RefreshToolsModelClasses();
        _lblStatus.Text = "已屏蔽标签：" + cn;
    }

    private async void RetrainToolsModelClasses()
    {
        if (_toolsMcBusy) return;
        if (_trainBusy)
        {
            MessageBox.Show(this, "已经有训练任务在跑，等它结束再点。", "再训练",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        SetToolsMcBusy(true);
        try
        {
            // 只做“能不能训练”的检查；安装/初始化统一走工具页顶部的“首次部署 · 换电脑”卡。
            var items = await RunEnvCheckAsync(false);
            var fail = items.FirstOrDefault(i => i.State == EnvCheckState.Fail);
            if (fail != null)
            {
                MessageBox.Show(this,
                    "运行环境还有问题，先解决再训练：\n\n" + fail.Name + "：" + fail.Detail
                    + "\n\n到工具页顶部的“首次部署 · 换电脑”卡点“一键初始化”。",
                    "再训练", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var train = items.FirstOrDefault(i => i.Name == "训练环境");
            if (train == null || train.State != EnvCheckState.Ok || !File.Exists(AppPaths.YoloExe))
            {
                MessageBox.Show(this,
                    "再训练要用 ultralytics 训练环境，现在还没就绪：\n\n"
                    + (train?.Detail ?? "没有找到训练环境检查项")
                    + "\n\n先到工具页顶部的“首次部署 · 换电脑”卡点“一键初始化”（可顺带安装训练环境），装好再回来训练。",
                    "再训练", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var empty = _store.ClassStats().Where(s => s.Boxes == 0).Select(s => s.Cn).ToList();
            if (empty.Count > 0 &&
                MessageBox.Show(this,
                    "这些标签在数据集里没有标注框：" + string.Join("、", empty) + "。\n\n"
                    + "带着它们训练，模型里依然不会认这些类。要彻底去掉，先把样本标上框，或者从数据集删除。\n\n继续训练？",
                    "再训练", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            await StartTrainingAsync();
            RefreshToolsPage();
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, "再训练失败：\n" + ex.Message, "再训练",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        finally
        {
            SetToolsMcBusy(false);
        }
    }

    // ---------------------------------------------------------------- 改名弹窗

    /// <summary>改标签名的小弹窗：显示名默认勾上；只有数据集里有的类才能改存储名。</summary>
    private sealed class ToolsClassRenameDialog : Form
    {
        private readonly TextBox _txtCn;
        private readonly TextBox _txtEn;
        private readonly CheckBox _chkCn;
        private readonly CheckBox _chkEn;

        public bool ChangeDisplay => _chkCn.Checked;
        public bool ChangeEnglish => _chkEn.Checked;
        public string DisplayName => _txtCn.Text.Trim();
        public string StorageName => _txtEn.Text.Trim();

        public ToolsClassRenameDialog(string en, string cn, bool inDataset)
        {
            Text = "修改标签";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            BackColor = Theme.Card;
            Font = Theme.Body;
            ClientSize = new Size(500, 316);

            var body = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 0,
                AutoScroll = true,
                Padding = new Padding(16, 14, 16, 12),
                BackColor = Theme.Card,
            };
            body.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));

            AddRow(body, new Label
            {
                Text = "“" + cn + "” · 存储名 " + en,
                Font = Theme.BodyBold,
                ForeColor = Theme.Ink,
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 8),
            });

            _chkCn = new CheckBox
            {
                Text = "① 修改显示名（中文，界面立刻生效）",
                Checked = true,
                AutoSize = true,
                ForeColor = Theme.Ink,
                Margin = new Padding(0, 4, 0, 2),
            };
            AddRow(body, _chkCn);

            _txtCn = new TextBox
            {
                Text = cn,
                Dock = DockStyle.Top,
                Font = Theme.Body,
                Margin = new Padding(0, 0, 0, 8),
            };
            AddRow(body, _txtCn);

            _chkEn = new CheckBox
            {
                Text = inDataset
                    ? "② 同时修改存储名（训练用 ASCII 类名，会同步数据集）"
                    : "② 修改存储名（这个类不在数据集里，改不了）",
                Checked = false,
                Enabled = inDataset,
                AutoSize = true,
                ForeColor = inDataset ? Theme.Ink : Theme.Muted,
                Margin = new Padding(0, 4, 0, 2),
            };
            AddRow(body, _chkEn);

            _txtEn = new TextBox
            {
                Text = en,
                Dock = DockStyle.Top,
                Font = Theme.Body,
                Enabled = false,
                Margin = new Padding(0, 0, 0, 6),
            };
            AddRow(body, _txtEn);

            _chkEn.CheckedChanged += (_, _) => { _txtEn.Enabled = _chkEn.Checked && _chkEn.Enabled; };
            _chkCn.CheckedChanged += (_, _) => { _txtCn.Enabled = _chkCn.Checked; };

            AddRow(body, Theme.Hint("改存储名会动 classes.txt 和该类的所有标注框；模型文件不会自动变，重训后才认新名字。"));

            var flow = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                WrapContents = false,
                FlowDirection = FlowDirection.RightToLeft,
                BackColor = Theme.Card,
                Margin = new Padding(0, 8, 0, 0),
            };
            var ok = Theme.Primary("确定");
            ok.MinimumSize = new Size(96, 34);
            ok.Margin = new Padding(6, 0, 0, 0);
            ok.Click += (_, _) => Accept();

            var cancel = Theme.Ghost("取消");
            cancel.MinimumSize = new Size(96, 34);
            cancel.DialogResult = DialogResult.Cancel;

            flow.Controls.Add(ok);
            flow.Controls.Add(cancel);
            AddRow(body, flow);

            AcceptButton = ok;
            CancelButton = cancel;
            Controls.Add(body);
            Shown += (_, _) => { _txtCn.Focus(); _txtCn.SelectAll(); };
        }

        private void Accept()
        {
            if (!_chkCn.Checked && !_chkEn.Checked)
            {
                DialogResult = DialogResult.Cancel;
                return;
            }
            if (_chkCn.Checked && DisplayName.Length == 0)
            {
                MessageBox.Show(this, "显示名不能为空。", "修改标签", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtCn.Focus();
                return;
            }
            if (_chkEn.Checked && !DatasetStore.IsStorageName(StorageName))
            {
                MessageBox.Show(this, "存储名只能用英文字母、数字、下划线或连字符。\n\n比如：barcode_128",
                    "修改标签", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtEn.Focus();
                return;
            }
            DialogResult = DialogResult.OK;
        }
    }
}