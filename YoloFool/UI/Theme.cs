using System;
using System.Drawing;
using System.Windows.Forms;

namespace YoloFool;

/// <summary>统一视觉风格：浅色卡片、蓝色主操作、等宽日志。</summary>
internal static class Theme
{
    public static readonly Color Bg = Color.FromArgb(244, 246, 250);
    public static readonly Color Card = Color.White;
    public static readonly Color Ink = Color.FromArgb(30, 35, 44);
    public static readonly Color Muted = Color.FromArgb(108, 116, 130);
    public static readonly Color Accent = Color.FromArgb(36, 112, 210);
    public static readonly Color AccentHover = Color.FromArgb(28, 96, 186);
    public static readonly Color Ok = Color.FromArgb(22, 138, 88);
    public static readonly Color Warn = Color.FromArgb(190, 118, 18);
    public static readonly Color Danger = Color.FromArgb(190, 58, 58);
    public static readonly Color Border = Color.FromArgb(219, 224, 232);
    public static readonly Color PaleBlue = Color.FromArgb(232, 240, 252);

    public static readonly Font Body = new("Microsoft YaHei UI", 9.5f);
    public static readonly Font BodyBold = new("Microsoft YaHei UI", 9.5f, FontStyle.Bold);
    public static readonly Font Small = new("Microsoft YaHei UI", 8.5f);
    public static readonly Font Title = new("Microsoft YaHei UI", 15f, FontStyle.Bold);
    public static readonly Font Section = new("Microsoft YaHei UI", 10.5f, FontStyle.Bold);
    public static readonly Font Mono = new("Consolas", 8.5f);

    public static Button Primary(string text)
    {
        var b = BaseButton(text);
        b.BackColor = Accent;
        b.ForeColor = Color.White;
        b.FlatAppearance.BorderSize = 0;
        b.FlatAppearance.MouseOverBackColor = AccentHover;
        b.Font = BodyBold;
        return b;
    }

    public static Button Success(string text)
    {
        var b = BaseButton(text);
        b.BackColor = Ok;
        b.ForeColor = Color.White;
        b.FlatAppearance.BorderSize = 0;
        b.FlatAppearance.MouseOverBackColor = Color.FromArgb(18, 118, 74);
        b.Font = BodyBold;
        return b;
    }

    public static Button Ghost(string text)
    {
        var b = BaseButton(text);
        b.BackColor = Color.White;
        b.ForeColor = Ink;
        b.FlatAppearance.BorderSize = 1;
        b.FlatAppearance.BorderColor = Border;
        b.FlatAppearance.MouseOverBackColor = Color.FromArgb(238, 242, 248);
        return b;
    }

    public static Button Link(string text)
    {
        var b = BaseButton(text);
        b.BackColor = Color.Transparent;
        b.ForeColor = Accent;
        b.FlatAppearance.BorderSize = 0;
        b.FlatAppearance.MouseOverBackColor = PaleBlue;
        b.Font = Body;
        return b;
    }

    private static Button BaseButton(string text) => new()
    {
        Text = text,
        AutoSize = true,
        AutoSizeMode = AutoSizeMode.GrowAndShrink,
        MinimumSize = new Size(84, 34),
        Padding = new Padding(10, 0, 10, 0),
        Margin = new Padding(4, 4, 4, 4),
        FlatStyle = FlatStyle.Flat,
        UseVisualStyleBackColor = false,
        Cursor = Cursors.Hand,
        Font = Body,
    };

    public static Label SectionLabel(string text) => new()
    {
        Text = text,
        Font = Section,
        ForeColor = Ink,
        AutoSize = true,
        Margin = new Padding(0, 0, 0, 6),
    };

    public static Label Hint(string text) => new()
    {
        Text = text,
        Font = Small,
        ForeColor = Muted,
        AutoSize = true,
        Margin = new Padding(0, 2, 0, 2),
    };

    /// <summary>带细边框和圆角的卡片容器。</summary>
    public static Panel CardPanel(string title = null)
    {
        var p = new Panel
        {
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            BackColor = Card,
            Padding = new Padding(14, 12, 14, 12),
            Margin = new Padding(0, 0, 0, 10),
            Dock = DockStyle.Top,
        };
        p.Paint += (s, e) =>
        {
            using var pen = new Pen(Border);
            e.Graphics.DrawRectangle(pen, 0, 0, p.Width - 1, p.Height - 1);
        };
        if (!string.IsNullOrEmpty(title))
        {
            var lbl = SectionLabel(title);
            lbl.Dock = DockStyle.Top;
            p.Controls.Add(lbl);
        }
        return p;
    }

    public static void StyleGrid(DataGridView g)
    {
        g.EnableHeadersVisualStyles = false;
        g.BorderStyle = BorderStyle.None;
        g.BackgroundColor = Color.White;
        g.GridColor = Border;
        g.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        g.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
        g.RowHeadersVisible = false;
        g.AllowUserToAddRows = false;
        g.AllowUserToDeleteRows = false;
        g.AllowUserToResizeRows = false;
        g.ReadOnly = true;
        g.MultiSelect = false;
        g.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        g.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        g.ColumnHeadersHeight = 34;
        g.RowTemplate.Height = 30;
        g.Font = Body;
        g.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
        {
            BackColor = Color.FromArgb(247, 249, 252),
            ForeColor = Muted,
            Font = BodyBold,
            SelectionBackColor = Color.FromArgb(247, 249, 252),
            SelectionForeColor = Muted,
            Padding = new Padding(6, 0, 6, 0),
            Alignment = DataGridViewContentAlignment.MiddleLeft,
        };
        g.DefaultCellStyle = new DataGridViewCellStyle
        {
            BackColor = Color.White,
            ForeColor = Ink,
            SelectionBackColor = PaleBlue,
            SelectionForeColor = Ink,
            Font = Body,
            Padding = new Padding(6, 0, 6, 0),
        };
        g.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
        {
            BackColor = Color.FromArgb(251, 252, 254),
        };
    }

    public static void StyleLog(TextBox t)
    {
        t.Multiline = true;
        t.ReadOnly = true;
        t.ScrollBars = ScrollBars.Vertical;
        t.WordWrap = false;
        t.Font = Mono;
        t.BackColor = Color.FromArgb(28, 32, 39);
        t.ForeColor = Color.FromArgb(214, 221, 232);
        t.BorderStyle = BorderStyle.FixedSingle;
    }
}
