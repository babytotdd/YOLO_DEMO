using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace YoloFool;

/// <summary>针床定位 V2 页内用的命名框：给框选的目标/区域起名，不写全局标签库。</summary>
internal sealed class AnchorNameDialog : Form
{
    private readonly ComboBox _combo;
    private readonly Label _error;

    private AnchorNameDialog(string title, string tip, IEnumerable<string> suggestions, string initial)
    {
        Text = title;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        StartPosition = FormStartPosition.CenterParent;
        MinimizeBox = false;
        MaximizeBox = false;
        ShowInTaskbar = false;
        ShowIcon = false;
        ClientSize = new Size(430, 210);
        BackColor = Theme.Card;
        Font = Theme.Body;
        AutoScaleMode = AutoScaleMode.Dpi;
        AutoScaleDimensions = new SizeF(96F, 96F);

        var root = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 4,
            Padding = new Padding(20, 18, 20, 14),
            BackColor = Theme.Card,
        };
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        var label = new Label
        {
            Text = tip,
            AutoSize = true,
            Font = Theme.Body,
            ForeColor = Theme.Ink,
            Margin = new Padding(0, 0, 0, 10),
        };
        root.Controls.Add(label, 0, 0);

        _combo = new ComboBox
        {
            Dock = DockStyle.Top,
            DropDownStyle = ComboBoxStyle.DropDown,
            Font = new Font("Microsoft YaHei UI", 11f),
            AutoCompleteMode = AutoCompleteMode.SuggestAppend,
            AutoCompleteSource = AutoCompleteSource.ListItems,
            DropDownHeight = 240,
            Margin = new Padding(0, 0, 0, 6),
        };
        foreach (var name in (suggestions ?? Enumerable.Empty<string>()).Where(s => !string.IsNullOrWhiteSpace(s)).Distinct())
            _combo.Items.Add(name);
        _combo.Text = initial ?? "";
        root.Controls.Add(_combo, 0, 1);

        _error = new Label
        {
            Text = "",
            AutoSize = true,
            Font = Theme.Small,
            ForeColor = Theme.Danger,
            Margin = new Padding(0, 4, 0, 4),
        };
        root.Controls.Add(_error, 0, 2);

        var bar = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.RightToLeft,
            AutoSize = true,
            WrapContents = false,
            Margin = new Padding(0),
        };
        var cancel = Theme.Ghost("取消");
        cancel.Width = 82;
        cancel.Click += (_, __) => { DialogResult = DialogResult.Cancel; Close(); };
        var ok = Theme.Primary("确定");
        ok.Width = 82;
        ok.Click += (_, __) => Confirm();
        bar.Controls.Add(cancel);
        bar.Controls.Add(ok);
        root.Controls.Add(bar, 0, 3);

        Controls.Add(root);
        AcceptButton = ok;
        _combo.KeyDown += (_, e) =>
        {
            if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; Confirm(); }
        };
        Shown += (_, __) => { _combo.Focus(); _combo.SelectAll(); };
    }

    public string Value => _combo.Text.Trim();

    private void Confirm()
    {
        var text = Value;
        if (text.Length == 0)
        {
            _error.Text = "名字不能为空";
            return;
        }
        if (text.IndexOfAny(new[] { '/', '\\', ':', '*', '?', '"', '<', '>', '|' }) >= 0)
        {
            _error.Text = "名字里不能有 \\ / : * ? \" < > | 这些字符";
            return;
        }
        DialogResult = DialogResult.OK;
        Close();
    }

    public static string Ask(IWin32Window owner, string title, string tip, IEnumerable<string> suggestions = null, string initial = "")
    {
        using var dlg = new AnchorNameDialog(title, tip, suggestions, initial);
        return dlg.ShowDialog(owner) == DialogResult.OK ? dlg.Value : "";
    }
}
