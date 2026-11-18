using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace YoloFool;

/// <summary>“筛除”设置对话框：面积/长宽比/去重/连续帧/类别屏蔽。</summary>
internal sealed class FilterDialog : Form
{
    private readonly CheckBox _chkOn;
    private readonly NumericUpDown _numMinArea, _numMaxArea, _numMinAspect, _numMaxAspect, _numIou, _numConfirm;
    private readonly TextBox _tbMuted;

    public FilterOptions Result { get; private set; }

    public FilterDialog(FilterOptions current, IReadOnlyList<string> knownClasses = null)
    {
        var src = current ?? new FilterOptions();
        Text = "筛除与屏蔽";
        Font = Theme.Body;
        BackColor = Theme.Card;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        ClientSize = new Size(560, 392);

        var table = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 4,
            RowCount = 0,
            Padding = new Padding(18, 16, 18, 6),
            BackColor = Theme.Card,
        };
        table.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 96));
        table.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 96));

        _chkOn = new CheckBox
        {
            Text = "启用面积 / 长宽比 / 去重筛除",
            Checked = src.Enabled,
            AutoSize = true,
            Font = Theme.Body,
            Margin = new Padding(0, 2, 0, 10),
        };
        AddSpanRow(table, _chkOn);

        AddRow(table, "面积占比 %", _numMinArea = Num((decimal)src.MinAreaPercent, 0, 100, 1),
            "到", _numMaxArea = Num((decimal)(src.MaxAreaPercent <= 0 ? 100 : src.MaxAreaPercent), 0, 100, 1));
        AddRow(table, "长宽比 宽/高", _numMinAspect = Num((decimal)src.MinAspect, 0, 20, 1),
            "到（0=不限）", _numMaxAspect = Num((decimal)src.MaxAspect, 0, 20, 1));
        AddRow(table, "同类重叠去重 IoU", _numIou = Num((decimal)src.DedupIou, 0, 0.95m, 2),
            "（0=关闭）", null);
        AddRow(table, "视频连续帧确认", _numConfirm = Num(src.ConfirmFrames, 1, 30, 0),
            "帧（1=不确认）", null);

        _tbMuted = new TextBox
        {
            Text = string.Join(", ", src.Muted),
            Font = Theme.Body,
            Dock = DockStyle.Fill,
            Margin = new Padding(0, 6, 0, 6),
        };
        AddSpanRow(table, "屏蔽类别", _tbMuted);

        var hint = Theme.Hint(knownClasses != null && knownClasses.Count > 0
            ? "可填中文名、英文名或类别编号，逗号分隔。当前模型：" + string.Join(" / ", knownClasses.Where(x => !string.IsNullOrWhiteSpace(x)))
            : "可填中文名、英文名或类别编号，逗号分隔。屏蔽的类别在图片和视频里都不再出现。");
        hint.MaximumSize = new Size(508, 0);
        hint.Margin = new Padding(0, 4, 0, 8);
        AddSpanRow(table, hint);

        var buttons = new FlowLayoutPanel
        {
            Dock = DockStyle.Bottom,
            FlowDirection = FlowDirection.RightToLeft,
            Height = 56,
            Padding = new Padding(0, 8, 14, 10),
            BackColor = Theme.Card,
            WrapContents = false,
        };
        var ok = Theme.Primary("确定");
        ok.Width = 88;
        ok.Click += (_, _) => Commit();
        var cancel = Theme.Ghost("取消");
        cancel.Width = 88;
        cancel.DialogResult = DialogResult.Cancel;
        var reset = Theme.Link("恢复默认");
        reset.Margin = new Padding(0, 8, 12, 0);
        reset.Click += (_, _) => ResetValues();
        buttons.Controls.Add(ok);
        buttons.Controls.Add(cancel);
        buttons.Controls.Add(reset);

        Controls.Add(table);
        Controls.Add(buttons);
        AcceptButton = ok;
        CancelButton = cancel;
    }

    private static NumericUpDown Num(decimal value, decimal min, decimal max, int decimals) => new()
    {
        Value = Math.Clamp(value, min, max),
        Minimum = min,
        Maximum = max,
        DecimalPlaces = decimals,
        Increment = decimals == 0 ? 1 : (decimals == 1 ? 0.5m : 0.05m),
        Width = 88,
        Font = Theme.Body,
        TextAlign = HorizontalAlignment.Right,
        Margin = new Padding(0, 4, 6, 4),
    };

    private static void AddSpanRow(TableLayoutPanel t, Control control) => AddSpanRow(t, null, control);

    private static void AddSpanRow(TableLayoutPanel t, string label, Control control)
    {
        int r = t.RowCount;
        t.RowCount = r + 1;
        t.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        if (!string.IsNullOrEmpty(label))
        {
            t.Controls.Add(new Label
            {
                Text = label,
                AutoSize = true,
                Font = Theme.Body,
                ForeColor = Theme.Ink,
                Margin = new Padding(0, 10, 8, 6),
            }, 0, r);
            control.Dock = DockStyle.Fill;
            t.Controls.Add(control, 1, r);
            t.SetColumnSpan(control, 3);
            return;
        }
        control.Dock = DockStyle.Fill;
        t.Controls.Add(control, 0, r);
        t.SetColumnSpan(control, 4);
    }

    private static void AddRow(TableLayoutPanel t, string label, Control a, string mid, Control b)
    {
        int r = t.RowCount;
        t.RowCount = r + 1;
        t.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        t.Controls.Add(new Label
        {
            Text = label,
            AutoSize = true,
            Font = Theme.Body,
            ForeColor = Theme.Ink,
            Margin = new Padding(0, 10, 10, 6),
        }, 0, r);

        a.Margin = new Padding(0, 6, 8, 6);
        a.Anchor = AnchorStyles.Left;
        t.Controls.Add(a, 1, r);

        if (!string.IsNullOrEmpty(mid))
        {
            t.Controls.Add(new Label
            {
                Text = mid,
                AutoSize = true,
                Font = Theme.Small,
                ForeColor = Theme.Muted,
                Margin = new Padding(0, 10, 8, 6),
            }, 2, r);
        }

        if (b != null)
        {
            b.Margin = new Padding(0, 6, 6, 6);
            b.Anchor = AnchorStyles.Left;
            t.Controls.Add(b, 3, r);
        }
    }

    private void ResetValues()
    {
        _chkOn.Checked = false;
        _numMinArea.Value = 0;
        _numMaxArea.Value = 100;
        _numMinAspect.Value = 0;
        _numMaxAspect.Value = 0;
        _numIou.Value = 0.6m;
        _numConfirm.Value = 3;
        _tbMuted.Text = "";
    }

    private void Commit()
    {
        var o = new FilterOptions
        {
            Enabled = _chkOn.Checked,
            MinAreaPercent = (float)_numMinArea.Value,
            MaxAreaPercent = (float)_numMaxArea.Value,
            MinAspect = (float)_numMinAspect.Value,
            MaxAspect = (float)_numMaxAspect.Value,
            DedupIou = (float)_numIou.Value,
            ConfirmFrames = (int)_numConfirm.Value,
        };
        foreach (var token in (_tbMuted.Text ?? "").Split(new[] { ',', '，', ';', '；', ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries))
        {
            var t = token.Trim();
            if (t.Length > 0) o.Muted.Add(t);
        }
        Result = o;
        DialogResult = DialogResult.OK;
        Close();
    }
}
