using System;
using System.Drawing;
using System.Windows.Forms;

namespace YoloFool;

/// <summary>框选后的命名窗口：输入中文，实时预览训练用的拼音存储名；顶部给最近命名的快捷入口。</summary>
internal sealed class NameDialog : Form
{
    private readonly LabelCatalog _labels;
    private readonly ComboBox _combo;
    private readonly Label _preview;

    public string DisplayName => _combo.Text.Trim();

    public NameDialog(LabelCatalog labels, string initial = "")
    {
        _labels = labels;

        Text = "命名目标";
        FormBorderStyle = FormBorderStyle.FixedDialog;
        StartPosition = FormStartPosition.CenterParent;
        MinimizeBox = false;
        MaximizeBox = false;
        ShowInTaskbar = false;
        ShowIcon = false;
        ClientSize = new Size(430, 296);
        BackColor = Theme.Card;
        Font = Theme.Body;
        AutoScaleMode = AutoScaleMode.Dpi;
        AutoScaleDimensions = new SizeF(96F, 96F);

        var root = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 6,
            Padding = new Padding(20, 18, 20, 14),
            BackColor = Theme.Card,
        };
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        var tip = new Label
        {
            Text = "给框选的目标起个名字（中文即可）。\r\n这个名字会显示在识别结果里，训练时自动转成拼音。",
            AutoSize = true,
            Font = Theme.Body,
            ForeColor = Theme.Ink,
            Margin = new Padding(0, 0, 0, 10),
        };

        _combo = new ComboBox
        {
            Dock = DockStyle.Top,
            DropDownStyle = ComboBoxStyle.DropDown,
            Font = new Font("Microsoft YaHei UI", 11f),
            AutoCompleteMode = AutoCompleteMode.SuggestAppend,
            AutoCompleteSource = AutoCompleteSource.ListItems,
            DropDownHeight = 260,
            Margin = new Padding(0, 0, 0, 6),
        };
        foreach (var name in _labels.KnownDisplayNames) _combo.Items.Add(name);
        _combo.Text = initial ?? "";

        // 最近命名的 3 个名字：点一下直接填入，减少重复输入
        var recent = new FlowLayoutPanel
        {
            FlowDirection = FlowDirection.LeftToRight,
            AutoSize = true,
            WrapContents = false,
            Margin = new Padding(0, 0, 0, 6),
            BackColor = Theme.Card,
        };
        var recentNames = RecentNames.Items;
        if (recentNames.Count == 0)
        {
            recent.Visible = false;
        }
        else
        {
            recent.Controls.Add(new Label
            {
                Text = "最近：",
                AutoSize = true,
                Font = Theme.Small,
                ForeColor = Theme.Muted,
                Margin = new Padding(0, 7, 4, 0),
            });
            foreach (var name in recentNames)
            {
                var chip = Theme.Link(name);
                chip.Margin = new Padding(0, 3, 8, 0);
                chip.Click += (_, _) =>
                {
                    _combo.Text = name;
                    _combo.Focus();
                    _combo.SelectionStart = _combo.Text.Length;
                };
                recent.Controls.Add(chip);
            }
        }

        _preview = new Label
        {
            Text = "",
            AutoSize = true,
            Font = Theme.Small,
            ForeColor = Theme.Muted,
            Margin = new Padding(0, 0, 0, 8),
        };

        var buttons = new FlowLayoutPanel
        {
            FlowDirection = FlowDirection.RightToLeft,
            Dock = DockStyle.Bottom,
            AutoSize = true,
            WrapContents = false,
            Margin = new Padding(0, 8, 0, 0),
        };
        var ok = Theme.Primary("确定");
        ok.MinimumSize = new Size(96, 36);
        var cancel = Theme.Ghost("取消");
        cancel.MinimumSize = new Size(84, 36);
        ok.Click += (_, _) =>
        {
            if (DisplayName.Length == 0)
            {
                MessageBox.Show(this, "请输入目标名称。", "命名目标", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _combo.Focus();
                return;
            }
            RecentNames.Push(DisplayName);
            DialogResult = DialogResult.OK;
            Close();
        };
        cancel.Click += (_, _) => { DialogResult = DialogResult.Cancel; Close(); };
        buttons.Controls.Add(ok);
        buttons.Controls.Add(cancel);

        _combo.TextChanged += (_, _) => UpdatePreview();
        root.Controls.Add(tip, 0, 0);
        root.Controls.Add(_combo, 0, 1);
        root.Controls.Add(recent, 0, 2);
        root.Controls.Add(_preview, 0, 3);
        root.Controls.Add(buttons, 0, 5);
        Controls.Add(root);

        AcceptButton = ok;
        CancelButton = cancel;
        Shown += (_, _) =>
        {
            _combo.Focus();
            _combo.SelectAll();
        };
        UpdatePreview();
    }

    private void UpdatePreview()
    {
        var name = DisplayName;
        _preview.Text = name.Length == 0
            ? "训练用存储名：（输入后自动生成拼音）"
            : "训练用存储名：" + _labels.PreviewEnglish(name);
    }
}
