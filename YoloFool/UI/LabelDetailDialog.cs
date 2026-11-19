using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace YoloFool;

/// <summary>
/// “标签管理”的详情窗口：列出这个标签用到的训练内容（图片 + 框数 + 训练文件名），
/// 选中一行就在右边显示缩略图；底部可以直接把这个标签从数据集里删掉。
/// 删除会先自动备份 annotations.json / classes.txt，当前模型文件不动。
/// </summary>
internal sealed class LabelDetailDialog : Form
{
    private readonly DatasetStore _store;
    private readonly string _en;
    private readonly string _cn;
    private readonly bool _mutedBefore;
    private readonly DataGridView _grid;
    private readonly PictureBox _preview;
    private readonly Label _lblSummary;
    private readonly Label _lblFile;
    private readonly TextBox _txtPath;
    private readonly CheckBox _chkMute;
    private readonly int _boxTotal;
    private readonly int _imgTotal;

    /// <summary>用户确实把这个标签从数据集里删掉了。</summary>
    public bool Deleted { get; private set; }

    /// <summary>用户删除时勾选了“同时屏蔽”，由主窗写进屏蔽名单（当前模型立刻不再显示）。</summary>
    public bool Muted { get; private set; }

    public LabelDetailDialog(DatasetStore store, string en, string displayName, bool alreadyMuted)
    {
        _store = store ?? throw new ArgumentNullException(nameof(store));
        _en = (en ?? "").Trim();
        _cn = string.IsNullOrWhiteSpace(displayName) ? _en : displayName;
        _mutedBefore = alreadyMuted;

        IReadOnlyList<ClassImage> images;
        try { images = _store.ImagesOfClass(_en); }
        catch { images = Array.Empty<ClassImage>(); }
        foreach (var it in images) { _boxTotal += it.Boxes; _imgTotal++; }

        Text = "标签：" + _cn;
        FormBorderStyle = FormBorderStyle.Sizable;
        StartPosition = FormStartPosition.CenterParent;
        MinimizeBox = false;
        MaximizeBox = true;
        ShowInTaskbar = false;
        ShowIcon = false;
        ClientSize = new Size(940, 600);
        MinimumSize = new Size(760, 460);
        BackColor = Theme.Card;
        Font = Theme.Body;
        AutoScaleMode = AutoScaleMode.Dpi;
        AutoScaleDimensions = new SizeF(96F, 96F);

        var root = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 3,
            Padding = new Padding(16, 14, 16, 12),
            BackColor = Theme.Card,
        };
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        _lblSummary = new Label
        {
            Text = $"标签：{_cn}（{_en}）· {_imgTotal} 张图片 · {_boxTotal} 个框",
            Font = Theme.BodyBold,
            ForeColor = Theme.Ink,
            AutoSize = true,
            Margin = new Padding(0, 0, 0, 4),
        };
        var hint = Theme.Hint("下面就是这个标签用到的训练内容，选中一行看缩略图。重新训练时这些图片会参与训练。");

        var header = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 2,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            Margin = new Padding(0, 0, 0, 10),
            BackColor = Theme.Card,
        };
        header.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
        header.Controls.Add(_lblSummary, 0, 0);
        header.Controls.Add(hint, 0, 1);

        // ---- 列表 + 缩略图
        _grid = new DataGridView
        {
            Name = "gridLabelImages",
            Dock = DockStyle.Fill,
            Margin = new Padding(0, 0, 12, 0),
        };
        Theme.StyleGrid(_grid);
        var cImg = new DataGridViewTextBoxColumn { Name = "colImage", HeaderText = "图片", FillWeight = 55 };
        var cBoxes = new DataGridViewTextBoxColumn { Name = "colBoxes", HeaderText = "框数", FillWeight = 14 };
        var cPending = new DataGridViewTextBoxColumn { Name = "colPending", HeaderText = "待复核", FillWeight = 14 };
        var cExport = new DataGridViewTextBoxColumn { Name = "colExport", HeaderText = "训练集文件名", FillWeight = 55 };
        _grid.Columns.AddRange(cImg, cBoxes, cPending, cExport);

        foreach (var it in images)
        {
            int i = _grid.Rows.Add(it.FileName, it.Boxes, it.Pending > 0 ? it.Pending.ToString() : "-", it.ExportText);
            _grid.Rows[i].Tag = it;
            if (it.Pending > 0)
                _grid.Rows[i].Cells[2].Style.ForeColor = Theme.Warn;
        }

        _preview = new PictureBox
        {
            Name = "picLabelPreview",
            Dock = DockStyle.Fill,
            SizeMode = PictureBoxSizeMode.Zoom,
            BackColor = Color.FromArgb(247, 249, 252),
            BorderStyle = BorderStyle.FixedSingle,
            Margin = new Padding(0, 0, 0, 6),
        };

        _lblFile = Theme.Hint("选中左边的图片看缩略图。");

        _txtPath = new TextBox
        {
            Dock = DockStyle.Top,
            ReadOnly = true,
            Font = Theme.Mono,
            BorderStyle = BorderStyle.FixedSingle,
            BackColor = Color.White,
            Margin = new Padding(0, 0, 0, 6),
        };

        var btnCopy = Theme.Link("复制图片路径");
        btnCopy.Margin = new Padding(0, 0, 0, 0);
        btnCopy.Click += (_, _) =>
        {
            var text = _txtPath.Text ?? "";
            if (text.Length == 0) return;
            try { Clipboard.SetText(text); }
            catch (Exception ex) { MessageBox.Show(this, "复制失败：" + ex.Message, "标签", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        };

        var right = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 4,
            Margin = Padding.Empty,
            BackColor = Theme.Card,
        };
        right.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
        right.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
        right.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        right.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        right.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        right.Controls.Add(_preview, 0, 0);
        right.Controls.Add(_lblFile, 0, 1);
        right.Controls.Add(_txtPath, 0, 2);
        right.Controls.Add(btnCopy, 0, 3);

        var content = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 1,
            Margin = Padding.Empty,
            BackColor = Theme.Card,
        };
        content.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
        content.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 372f));
        content.Controls.Add(_grid, 0, 0);
        content.Controls.Add(right, 1, 0);

        // ---- 底部
        _chkMute = new CheckBox
        {
            Text = alreadyMuted ? "这个标签已在屏蔽名单里" : "同时屏蔽这个标签（当前模型立刻不再显示它）",
            AutoSize = true,
            Font = Theme.Body,
            ForeColor = Theme.Ink,
            Checked = alreadyMuted,
            Enabled = !alreadyMuted,
            Margin = new Padding(0, 10, 0, 0),
        };

        var leftFlow = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
            WrapContents = false,
            BackColor = Theme.Card,
            Margin = Padding.Empty,
        };
        leftFlow.Controls.Add(_chkMute);

        var btnDelete = Theme.Ghost("从数据集删除这个标签");
        btnDelete.ForeColor = Theme.Danger;
        btnDelete.MinimumSize = new Size(168, 36);
        btnDelete.Margin = new Padding(0, 4, 8, 4);
        btnDelete.Enabled = _en.Length > 0;
        btnDelete.Click += (_, _) => DeleteClicked();

        var btnClose = Theme.Primary("关闭");
        btnClose.MinimumSize = new Size(96, 36);
        btnClose.Margin = new Padding(0, 4, 0, 4);
        btnClose.Click += (_, _) => { DialogResult = DialogResult.Cancel; Close(); };

        var rightFlow = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
            WrapContents = false,
            FlowDirection = FlowDirection.RightToLeft,
            BackColor = Theme.Card,
            Margin = Padding.Empty,
        };
        rightFlow.Controls.Add(btnClose);
        rightFlow.Controls.Add(btnDelete);

        var footer = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 1,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            Margin = new Padding(0, 10, 0, 0),
            BackColor = Theme.Card,
        };
        footer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
        footer.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        footer.Controls.Add(leftFlow, 0, 0);
        footer.Controls.Add(rightFlow, 1, 0);

        root.Controls.Add(header, 0, 0);
        root.Controls.Add(content, 0, 1);
        root.Controls.Add(footer, 0, 2);
        Controls.Add(root);

        _grid.SelectionChanged += (_, _) => ShowPreview(_grid.CurrentRow?.Tag as ClassImage);
        _grid.CellDoubleClick += (_, _) => ShowPreview(_grid.CurrentRow?.Tag as ClassImage);
        AcceptButton = btnClose;
        CancelButton = btnClose;

        Shown += (_, _) =>
        {
            if (_grid.Rows.Count > 0)
            {
                _grid.Rows[0].Selected = true;
                _grid.CurrentCell = _grid.Rows[0].Cells[0];
                ShowPreview(_grid.Rows[0].Tag as ClassImage);
            }
        };
    }

    private void ShowPreview(ClassImage item)
    {
        var old = _preview.Image;
        _preview.Image = null;
        old?.Dispose();

        if (item == null)
        {
            _lblFile.Text = "选中左边的图片看缩略图。";
            _txtPath.Text = "";
            return;
        }

        var path = item.PreviewPath;
        _txtPath.Text = path;
        if (!File.Exists(path))
        {
            _lblFile.Text = "图片文件已丢失：" + item.FileName;
            return;
        }

        try
        {
            // 用 FileShare.ReadWrite 打开，避免图片正被别的程序占用时整窗卡死；再克隆一份解除文件句柄
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var img = Image.FromStream(fs))
                _preview.Image = new Bitmap(img);

            var size = "";
            try
            {
                var fi = new FileInfo(path);
                size = $" · {fi.Length / 1024.0:F0} KB";
            }
            catch { }
            _lblFile.Text = $"{item.FileName} · {item.Boxes} 个框{size}" + (item.Exported ? "" : " · 还没导出到训练集");
        }
        catch (Exception ex)
        {
            _lblFile.Text = "预览失败：" + ex.Message;
        }
    }

    private void DeleteClicked()
    {
        var yes = MessageBox.Show(this,
            $"确定要把标签“{_cn}”从数据集里删掉吗？\n\n" +
            $"会删掉 {_boxTotal} 个框、{_imgTotal} 张图上的这个标签，classes.txt 里也会去掉它，其他类别的编号依次前移。\n" +
            "删除前会自动把 annotations.json 和 classes.txt 备份到 mydataset/backup。\n\n" +
            "注意：当前模型文件不变，重新训练之前它仍然会报这个类。",
            "删除标签", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (yes != DialogResult.Yes) return;

        try
        {
            var r = _store.RemoveClass(_en);
            Deleted = true;
            Muted = _chkMute.Checked && !_mutedBefore;
            Log.Info($"删除标签 {r.Cn}（{r.En}）：{r.Boxes} 个框 / {r.Images} 张图，备份 {r.BackupFile}");
            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, "删除标签失败：\n" + ex.Message, "删除标签",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing && _preview != null)
        {
            var img = _preview.Image;
            _preview.Image = null;
            img?.Dispose();
        }
        base.Dispose(disposing);
    }
}