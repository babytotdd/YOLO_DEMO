using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Windows.Forms;

namespace YoloFool;

/// <summary>框选草稿：确认后才交给上层命名。Bounds 是未旋转的基框（图像坐标）。</summary>
internal sealed class BoxDraft
{
    public Rectangle Bounds { get; init; }
    public float Angle { get; init; }
}

/// <summary>已保存的框被拖动手柄调整后的结果。</summary>
internal sealed class BoxEdit
{
    public int Index { get; init; }
    public Rectangle Bounds { get; init; }
    public float Angle { get; init; }
}

/// <summary>
/// 自绘图片画布：等比缩放、检测框/标注框、中文标签、滚轮缩放、右键拖动、框选橡皮筋。
/// 学习页的框选是“两段式”：拖出框后先进入待确认状态，可以拖动顶部圆点旋转，
/// 按回车或右键才确认并弹命名；重新框选、Esc 或操作其他内容都会取消。
/// 没有用 PictureBox，因为要精确控制坐标换算、命中测试和中文标签。
/// </summary>
internal sealed class ImageCanvas : Control
{
    private static readonly Font TagFont = new("Microsoft YaHei UI", 8.5f, FontStyle.Bold);
    private const float HandleHalf = 4f;
    private const float EdgeHandleHalf = 3.5f;
    private const float RotateHandleDistance = 30f;
    private const float ResizeHitRadius = 9f;

    private Bitmap _image;
    private float _scale = 1f;
    private PointF _offset;
    private bool _autoFit = true;
    private bool _dragging;
    private bool _panning;
    private Point _dragStart;
    private Point _dragCurrent;
    private Point _panStart;
    private PointF _panOffsetStart;
    private MouseButtons _panButton = MouseButtons.None;
    private Point _rightDownPoint;
    private bool _rightMoved;
    private int _selected = -1;
    private Point? _lineA;
    private Point? _lineB;
    private Point? _lineStart;
    private Point? _lineHover;

    private bool _hasPending;
    private RectangleF _pendingBounds;
    private float _pendingAngle;
    private bool _rotating;
    private float _rotateStartAngle;
    private float _rotateStartPointer;
    private int _resizeHandle = -1;
    private bool _movingPending;
    private PointF _moveStartImage;
    private RectangleF _moveStartBounds;

    // 已保存框的编辑状态：_editIndex >= 0 时表示正在拖它。
    private int _editIndex = -1;
    private RectangleF _editBounds;
    private float _editAngle;
    private bool _editChanged;

    private static readonly Color[] TrackPalette =
    {
        Color.FromArgb(90, 200, 255), Color.FromArgb(130, 225, 120),
        Color.FromArgb(255, 180, 70), Color.FromArgb(210, 140, 255),
        Color.FromArgb(255, 120, 150), Color.FromArgb(120, 220, 220),
    };

    public ImageCanvas()
    {
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                 ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.Selectable, true);
        BackColor = Color.FromArgb(36, 40, 47);
        Font = Theme.Body;
        TabStop = true;
    }

    public Bitmap Image
    {
        get => _image;
        set
        {
            CancelPending();
            _image = value;
            _selected = -1;
            _autoFit = true;
            Fit();
        }
    }

    public IReadOnlyList<DetectionItem> Detections { get; set; } = Array.Empty<DetectionItem>();
    public IReadOnlyList<TrackedObject> Tracks { get; set; } = Array.Empty<TrackedObject>();
    public Func<TrackedObject, string> TrackLabel { get; set; }
    public bool ShowZoomInfo { get; set; } = true;
    public bool LineEditable { get; set; }

    /// <summary>
    /// 右键菜单。右键拖动仍然是平移画面：只有“按下后几乎没移动就抬起”才会弹出这个菜单。
    /// </summary>
    public ContextMenuStrip CanvasMenu { get; set; }

    /// <summary>Ctrl+S 触发，由上层决定保存什么。</summary>
    public event Action SaveRequested;

    public Point? LineA
    {
        get => _lineA;
        set { _lineA = value; Invalidate(); }
    }

    public Point? LineB
    {
        get => _lineB;
        set { _lineB = value; Invalidate(); }
    }

    private IReadOnlyList<BoxRecord> _boxes = Array.Empty<BoxRecord>();

    public IReadOnlyList<BoxRecord> Boxes
    {
        get => _boxes;
        set
        {
            _boxes = value ?? Array.Empty<BoxRecord>();
            _editIndex = -1;
            Invalidate();
        }
    }
    public Func<DetectionItem, string> DetectionLabel { get; set; }
    public Func<BoxRecord, string> BoxLabel { get; set; }

    /// <summary>自动预标注生成的待复核框：画成青色虚线并带置信度，提醒这是模型猜的、还没确认。</summary>
    public Func<BoxRecord, bool> BoxPending { get; set; }

    public bool Editable { get; set; }
    public string EmptyText { get; set; } = "① 选择一张图片\r\n② 点“开始识别”";

    /// <summary>是否有待确认的框选（回车/右键确认）。</summary>
    public bool HasPending => _hasPending;

    /// <summary>是否有正在拖动的东西：待确认的新框，或某个已保存的框。</summary>
    private bool HasActive => _hasPending || _editIndex >= 0;

    private RectangleF ActiveBounds
    {
        get => _editIndex >= 0 ? _editBounds : _pendingBounds;
        set
        {
            if (_editIndex >= 0) _editBounds = value;
            else _pendingBounds = value;
        }
    }

    private float ActiveAngle
    {
        get => _editIndex >= 0 ? _editAngle : _pendingAngle;
        set
        {
            if (_editIndex >= 0) _editAngle = value;
            else _pendingAngle = value;
        }
    }

    public int SelectedIndex
    {
        get => _selected;
        set
        {
            if (_selected == value) return;
            _selected = value;
            Invalidate();
        }
    }

    public event EventHandler<BoxDraft> BoxDrawn;
    public event EventHandler<BoxEdit> BoxEdited;
    public event EventHandler<int> SelectionChanged;
    public event Action<Point, Point> LineDrawn;

    /// <summary>视频帧入口：只在第一帧自动适配，之后保持用户的缩放/平移。</summary>
    public void SetFrame(Bitmap frame)
    {
        bool first = _image == null;
        _image = frame;
        if (first)
        {
            _selected = -1;
            _autoFit = true;
            Fit();
        }
        else
        {
            Invalidate();
        }
    }

    public void Fit()
    {
        if (_image == null)
        {
            _scale = 1f;
            _offset = PointF.Empty;
            Invalidate();
            return;
        }
        float pad = 18f;
        float w = Math.Max(1f, Width - pad * 2);
        float h = Math.Max(1f, Height - pad * 2);
        _scale = Math.Min(w / _image.Width, h / _image.Height);
        if (float.IsNaN(_scale) || _scale <= 0) _scale = 1f;
        _scale = Math.Min(_scale, 3f);
        _offset = new PointF((Width - _image.Width * _scale) / 2f, (Height - _image.Height * _scale) / 2f);
        _autoFit = true;
        Invalidate();
    }

    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
        if (_autoFit) Fit();
        else Invalidate();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
        using (var bg = new SolidBrush(BackColor)) g.FillRectangle(bg, ClientRectangle);

        if (_image == null)
        {
            TextRenderer.DrawText(g, EmptyText, Theme.Body, ClientRectangle, Color.FromArgb(168, 178, 194),
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.WordBreak);
            return;
        }

        var dest = new RectangleF(_offset.X, _offset.Y, _image.Width * _scale, _image.Height * _scale);
        g.InterpolationMode = _scale >= 1f ? InterpolationMode.NearestNeighbor : InterpolationMode.HighQualityBicubic;
        g.PixelOffsetMode = PixelOffsetMode.Half;
        using (var shadow = new SolidBrush(Color.FromArgb(70, 0, 0, 0)))
            g.FillRectangle(shadow, dest.X + 4, dest.Y + 4, dest.Width, dest.Height);
        g.DrawImage(_image, dest);

        for (int i = 0; i < Boxes.Count; i++)
        {
            var b = Boxes[i];
            bool editing = i == _editIndex && !_hasPending;
            float bx = editing ? _editBounds.X : b.X;
            float by = editing ? _editBounds.Y : b.Y;
            float bw = editing ? _editBounds.Width : b.W;
            float bh = editing ? _editBounds.Height : b.H;
            float ba = editing ? _editAngle : b.Angle;
            if (bw <= 0 || bh <= 0) continue;
            bool sel = i == _selected;
            bool pending = BoxPending != null && BoxPending(b);
            var color = pending ? Color.FromArgb(0, 198, 232)
                : sel ? Color.FromArgb(255, 186, 56) : Color.FromArgb(112, 214, 142);
            var pts = BoxClientPoints(bx, by, bw, bh, ba);
            using var pen = new Pen(color, sel ? 3f : 2f);
            if (pending) pen.DashStyle = DashStyle.Dash;
            g.DrawPolygon(pen, pts);
            if (sel)
            {
                using var fill = new SolidBrush(Color.FromArgb(46, color));
                g.FillPolygon(fill, pts);
            }
            var text = BoxLabel?.Invoke(b);
            if (string.IsNullOrWhiteSpace(text)) text = b.Cn;
            if (pending) text += $" ·待复核 {b.Conf:P0}";
            DrawTag(g, text, BoundsOf(pts), color);
        }

        // 选中的已保存框：把手柄画出来，可以直接拖。
        if (Editable && _editIndex < 0 && _selected >= 0 && _selected < Boxes.Count)
        {
            var sb = Boxes[_selected];
            if (sb.W > 0 && sb.H > 0)
                DrawHandles(g, BoxClientPoints(sb.X, sb.Y, sb.W, sb.H, sb.Angle),
                    Color.FromArgb(255, 186, 56), sb.Angle);
        }

        for (int i = 0; i < Detections.Count; i++)
        {
            var d = Detections[i];
            var r = ToClient(new RectangleF(d.X, d.Y, d.W, d.H));
            bool sel = i == _selected;
            var color = sel ? Color.FromArgb(255, 186, 56) : Color.FromArgb(64, 156, 255);
            using var pen = new Pen(color, sel ? 3f : 2f);
            g.DrawRectangle(pen, r.X, r.Y, r.Width, r.Height);
            if (sel)
            {
                using var fill = new SolidBrush(Color.FromArgb(40, color));
                g.FillRectangle(fill, r);
            }
            var text = DetectionLabel?.Invoke(d);
            if (string.IsNullOrWhiteSpace(text)) text = d.RawCategory;
            DrawTag(g, text, r, color);
        }

        for (int i = 0; i < Tracks.Count; i++)
        {
            var t = Tracks[i];
            if (t?.Item == null) continue;
            var color = TrackPalette[Math.Abs(t.Id) % TrackPalette.Length];
            if (t.Trail != null && t.Trail.Count > 1)
            {
                using var trailPen = new Pen(Color.FromArgb(130, color), 1.6f);
                for (int k = 1; k < t.Trail.Count; k++)
                {
                    var a = ToClientPoint(t.Trail[k - 1]);
                    var b = ToClientPoint(t.Trail[k]);
                    g.DrawLine(trailPen, a, b);
                }
            }
            var r = ToClient(new RectangleF(t.Item.X, t.Item.Y, t.Item.W, t.Item.H));
            using (var pen = new Pen(color, 2.2f))
                g.DrawRectangle(pen, r.X, r.Y, r.Width, r.Height);
            var text = TrackLabel?.Invoke(t);
            if (string.IsNullOrWhiteSpace(text)) text = "#" + t.Id + " " + t.Item.RawCategory;
            DrawTag(g, text, r, color);
        }

        DrawCountLine(g);

        if (_dragging)
        {
            var band = Normalize(_dragStart, _dragCurrent);
            using var fill = new SolidBrush(Color.FromArgb(56, 80, 170, 255));
            g.FillRectangle(fill, band);
            using var pen = new Pen(Color.White, 1f) { DashStyle = DashStyle.Dash };
            g.DrawRectangle(pen, band);
        }

        if (_hasPending) DrawPending(g);

        if (!ShowZoomInfo && !_hasPending) return;
        var info = _hasPending
            ? "拖边角改大小 · 框内拖动或方向键移动 · 圆点旋转 · 回车/右键确认 · Esc 取消"
            : Editable && _selected >= 0 && _selected < Boxes.Count
                ? "已选中框：拖手柄改大小 · 框内拖动移动 · 方向键微调 · 单击空白处取消选中"
                : $"{_scale * 100f:F0}%   滚轮缩放 · " +
                  (CanvasMenu == null ? "右键拖动" : "右键拖动/右键菜单") + " · 双击复位";
        var size = TextRenderer.MeasureText(g, info, Theme.Small);
        var overlay = new Rectangle(10, Height - size.Height - 12, size.Width + 14, size.Height + 4);
        using (var b = new SolidBrush(Color.FromArgb(170, 0, 0, 0))) g.FillRectangle(b, overlay);
        TextRenderer.DrawText(g, info, Theme.Small, overlay, Color.FromArgb(226, 232, 242),
            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
    }

    private void DrawPending(Graphics g)
    {
        var pts = BoxClientPoints(_pendingBounds.X, _pendingBounds.Y, _pendingBounds.Width, _pendingBounds.Height, _pendingAngle);
        var color = Color.FromArgb(255, 186, 56);
        using (var fill = new SolidBrush(Color.FromArgb(34, color)))
            g.FillPolygon(fill, pts);
        using (var pen = new Pen(color, 2.2f) { DashStyle = DashStyle.Dash })
            g.DrawPolygon(pen, pts);
        DrawHandles(g, pts, color, _pendingAngle);
    }

    /// <summary>8 个缩放/边中点手柄 + 顶部旋转手柄（新框与正在调整的已保存框共用）。</summary>
    private void DrawHandles(Graphics g, PointF[] pts, Color color, float angle)
    {
        var handle = RotateHandleClient(pts);
        var topMid = new PointF((pts[0].X + pts[1].X) / 2f, (pts[0].Y + pts[1].Y) / 2f);
        using (var link = new Pen(color, 1.4f))
            g.DrawLine(link, topMid, handle);

        using (var pen = new Pen(color, 2f))
        {
            foreach (var p in pts)
            {
                var r = new RectangleF(p.X - HandleHalf, p.Y - HandleHalf, HandleHalf * 2, HandleHalf * 2);
                using var fill = new SolidBrush(Color.White);
                g.FillRectangle(fill, r);
                g.DrawRectangle(pen, r.X, r.Y, r.Width, r.Height);
            }
            // 四条边的中点手柄：拖它只调整这一边。
            foreach (var p in EdgeHandlePoints(pts))
            {
                var r = new RectangleF(p.X - EdgeHandleHalf, p.Y - EdgeHandleHalf, EdgeHandleHalf * 2, EdgeHandleHalf * 2);
                using var fill = new SolidBrush(Color.White);
                g.FillRectangle(fill, r);
                g.DrawRectangle(pen, r.X, r.Y, r.Width, r.Height);
            }
        }

        using (var fill = new SolidBrush(Color.White))
            g.FillEllipse(fill, handle.X - 6f, handle.Y - 6f, 12f, 12f);
        using (var pen = new Pen(color, 2.4f))
            g.DrawEllipse(pen, handle.X - 6f, handle.Y - 6f, 12f, 12f);

        var angleText = Math.Abs(angle) < 0.05f ? "0°" : angle.ToString("F1") + "°";
        var label = "旋转 " + angleText;
        var size = TextRenderer.MeasureText(g, label, TagFont);
        var badge = new Rectangle((int)(handle.X + 10), (int)(handle.Y - size.Height / 2f - 2),
            size.Width + 10, size.Height + 4);
        if (badge.Right > Width - 4) badge.X = (int)(handle.X - badge.Width - 10);
        if (badge.Top < 2) badge.Y = 2;
        using (var bg = new SolidBrush(Color.FromArgb(220, 30, 34, 40)))
            g.FillRectangle(bg, badge);
        TextRenderer.DrawText(g, label, TagFont, badge, color,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
    }

    private void DrawTag(Graphics g, string text, RectangleF box, Color color)
    {
        if (string.IsNullOrWhiteSpace(text)) return;
        var size = TextRenderer.MeasureText(g, text, TagFont);
        var x = box.X;
        var y = box.Y - size.Height - 3;
        if (y < 2) y = box.Y + 2;
        if (x + size.Width + 8 > Width - 2) x = Math.Max(2, Width - size.Width - 10);
        var rect = new Rectangle((int)x, (int)y, size.Width + 8, size.Height + 3);
        using var brush = new SolidBrush(color);
        g.FillRectangle(brush, rect);
        var ink = color.GetBrightness() > 0.62f ? Color.FromArgb(20, 24, 30) : Color.White;
        TextRenderer.DrawText(g, text, TagFont, rect, ink,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
    }

    private PointF[] BoxClientPoints(float x, float y, float w, float h, float angle)
    {
        var corners = RotatedBox.Corners(x, y, w, h, angle);
        var pts = new PointF[corners.Length];
        for (int i = 0; i < corners.Length; i++) pts[i] = ToClientPoint(corners[i]);
        return pts;
    }

    private static RectangleF BoundsOf(PointF[] pts)
    {
        float minX = pts.Min(p => p.X), minY = pts.Min(p => p.Y);
        float maxX = pts.Max(p => p.X), maxY = pts.Max(p => p.Y);
        return RectangleF.FromLTRB(minX, minY, maxX, maxY);
    }

    private static PointF RotateHandleClient(PointF[] pts)
    {
        var topMid = new PointF((pts[0].X + pts[1].X) / 2f, (pts[0].Y + pts[1].Y) / 2f);
        var center = new PointF(pts.Average(p => p.X), pts.Average(p => p.Y));
        float dx = topMid.X - center.X, dy = topMid.Y - center.Y;
        float len = MathF.Sqrt(dx * dx + dy * dy);
        if (len < 0.01f) return new PointF(topMid.X, topMid.Y - RotateHandleDistance);
        return new PointF(topMid.X + dx / len * RotateHandleDistance, topMid.Y + dy / len * RotateHandleDistance);
    }

    private RectangleF ToClient(RectangleF imageRect) => new(
        _offset.X + imageRect.X * _scale,
        _offset.Y + imageRect.Y * _scale,
        imageRect.Width * _scale,
        imageRect.Height * _scale);

    private PointF ToImage(Point client) => new(
        _scale <= 0 ? 0 : (client.X - _offset.X) / _scale,
        _scale <= 0 ? 0 : (client.Y - _offset.Y) / _scale);

    private PointF ToClientPoint(PointF imagePoint) => new(
        _offset.X + imagePoint.X * _scale,
        _offset.Y + imagePoint.Y * _scale);

    private void DrawCountLine(Graphics g)
    {
        var a = _lineA ?? _lineStart;
        var b = _lineB ?? _lineHover;
        if (a == null || b == null || _image == null) return;
        var pa = ToClientPoint(new PointF(a.Value.X, a.Value.Y));
        var pb = ToClientPoint(new PointF(b.Value.X, b.Value.Y));
        using var pen = new Pen(Color.FromArgb(255, 165, 45), 2.4f) { DashStyle = DashStyle.Dash };
        g.DrawLine(pen, pa, pb);
        using var brush = new SolidBrush(Color.FromArgb(255, 165, 45));
        g.FillEllipse(brush, pa.X - 4, pa.Y - 4, 8, 8);
        g.FillEllipse(brush, pb.X - 4, pb.Y - 4, 8, 8);
        var text = "计数线";
        var size = TextRenderer.MeasureText(g, text, TagFont);
        var x = (pa.X + pb.X) / 2f - size.Width / 2f;
        var y = (pa.Y + pb.Y) / 2f - size.Height - 8;
        var rect = new Rectangle((int)x, (int)y, size.Width + 10, size.Height + 3);
        using var bg = new SolidBrush(Color.FromArgb(230, 30, 34, 40));
        g.FillRectangle(bg, rect);
        TextRenderer.DrawText(g, text, TagFont, rect, Color.FromArgb(255, 200, 120),
            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
    }

    private Point ToImagePoint(Point client)
    {
        var f = ToImage(client);
        return new Point((int)Math.Round(f.X), (int)Math.Round(f.Y));
    }

    private Point ClampImage(Point p) => _image == null
        ? p
        : new Point(Math.Clamp(p.X, 0, _image.Width - 1), Math.Clamp(p.Y, 0, _image.Height - 1));

    private static Rectangle Normalize(Point a, Point b) => Rectangle.FromLTRB(
        Math.Min(a.X, b.X), Math.Min(a.Y, b.Y), Math.Max(a.X, b.X), Math.Max(a.Y, b.Y));

    protected override void OnMouseDown(MouseEventArgs e)
    {
        base.OnMouseDown(e);
        Focus();

        if (e.Button == MouseButtons.Right)
        {
            // 有待确认的框时，右键就是“确认”，不再平移画面。
            if (_hasPending)
            {
                ConfirmPending();
                return;
            }
            _rightDownPoint = e.Location;
            _rightMoved = false;
            StartPan(e.Location, MouseButtons.Right);
            return;
        }
        if (e.Button == MouseButtons.Middle)
        {
            StartPan(e.Location, MouseButtons.Middle);
            return;
        }
        if (e.Button != MouseButtons.Left || _image == null) return;

        if (LineEditable)
        {
            var p = ClampImage(ToImagePoint(e.Location));
            if (_lineStart == null)
            {
                _lineStart = p;
                _lineHover = p;
            }
            else
            {
                var a = _lineStart.Value;
                _lineStart = null;
                _lineHover = null;
                LineDrawn?.Invoke(a, p);
            }
            Cursor = Cursors.Cross;
            Invalidate();
            return;
        }

        if (!Editable)
        {
            SelectAt(e.Location);
            return;
        }

        if (_hasPending)
        {
            if (IsOnRotateHandle(e.Location))
            {
                StartRotate(e.Location);
                return;
            }
            int handle = HitResizeHandle(e.Location);
            if (handle >= 0)
            {
                _resizeHandle = handle;
                Capture = true;
                Cursor = ResizeCursor(handle);
                Invalidate();
                return;
            }
            if (IsOnActiveBody(e.Location))
            {
                _movingPending = true;
                _moveStartImage = ToImage(e.Location);
                _moveStartBounds = _pendingBounds;
                Capture = true;
                Cursor = Cursors.SizeAll;
                return;
            }
            // 重新框选 = 放弃上一个待确认的框。
            CancelPending();
        }

        // 已保存的框：点它的手柄或框内，直接进入编辑。
        if (Editable && _editIndex < 0 && TryBeginSavedEdit(e.Location)) return;

        _dragging = true;
        _dragStart = _dragCurrent = e.Location;
        Capture = true;
        Cursor = Cursors.Cross;
        Invalidate();
    }

    private void StartPan(Point location, MouseButtons button)
    {
        _panning = true;
        _panButton = button;
        _panStart = location;
        _panOffsetStart = _offset;
        Cursor = Cursors.SizeAll;
        Capture = true;
    }

    private float PointerAngle(Point client)
    {
        if (_image == null) return 0;
        var b = ActiveBounds;
        var center = ToClientPoint(new PointF(b.X + b.Width / 2f, b.Y + b.Height / 2f));
        return (float)(Math.Atan2(client.Y - center.Y, client.X - center.X) * 180.0 / Math.PI);
    }

    private static bool OnRotateHandle(PointF[] pts, Point client)
    {
        var handle = RotateHandleClient(pts);
        float dx = client.X - handle.X, dy = client.Y - handle.Y;
        return dx * dx + dy * dy <= 11f * 11f;
    }

    private bool IsOnRotateHandle(Point client)
    {
        if (!HasActive) return false;
        var b = ActiveBounds;
        return OnRotateHandle(BoxClientPoints(b.X, b.Y, b.Width, b.Height, ActiveAngle), client);
    }

    /// <summary>四条边的中点（上、右、下、左），顺序与缩放手柄编号一致。</summary>
    private static PointF[] EdgeHandlePoints(PointF[] pts) => new[]
    {
        Mid(pts[0], pts[1]), Mid(pts[1], pts[2]), Mid(pts[2], pts[3]), Mid(pts[3], pts[0]),
    };

    private static PointF Mid(PointF a, PointF b) => new((a.X + b.X) / 2f, (a.Y + b.Y) / 2f);

    /// <summary>命中哪个缩放手柄：0-3 四角（左上/右上/右下/左下），4-7 四边（上/右/下/左），-1 没命中。</summary>
    private int HitResizeHandle(Point client)
    {
        if (!HasActive) return -1;
        var b = ActiveBounds;
        return HitResizeHandleIn(BoxClientPoints(b.X, b.Y, b.Width, b.Height, ActiveAngle), client);
    }

    private static int HitResizeHandleIn(PointF[] pts, Point client)
    {
        var edges = EdgeHandlePoints(pts);
        float r2 = ResizeHitRadius * ResizeHitRadius;
        for (int i = 0; i < 4; i++)
        {
            float dx = client.X - pts[i].X, dy = client.Y - pts[i].Y;
            if (dx * dx + dy * dy <= r2) return i;
        }
        for (int i = 0; i < 4; i++)
        {
            float dx = client.X - edges[i].X, dy = client.Y - edges[i].Y;
            if (dx * dx + dy * dy <= r2) return 4 + i;
        }
        return -1;
    }

    private static void ResizeAxes(int handle, out int sx, out int sy)
    {
        sx = 0;
        sy = 0;
        switch (handle)
        {
            case 0: sx = -1; sy = -1; break;
            case 1: sx = 1; sy = -1; break;
            case 2: sx = 1; sy = 1; break;
            case 3: sx = -1; sy = 1; break;
            case 4: sy = -1; break;
            case 5: sx = 1; break;
            case 6: sy = 1; break;
            case 7: sx = -1; break;
        }
    }

    private static Cursor ResizeCursor(int handle) => handle switch
    {
        0 or 2 => Cursors.SizeNWSE,
        1 or 3 => Cursors.SizeNESW,
        4 or 6 => Cursors.SizeNS,
        _ => Cursors.SizeWE,
    };

    /// <summary>拖动手柄调整待确认框：对边保持不动，另一边跟着鼠标，旋转过的框同样适用。</summary>
    private void ResizePending(int handle, Point client)
    {
        if (_image == null) return;
        ResizeAxes(handle, out int sx, out int sy);
        var b = ActiveBounds;
        var center = new PointF(b.X + b.Width / 2f, b.Y + b.Height / 2f);
        var p = ToImage(client);
        var local = RotateVector(p.X - center.X, p.Y - center.Y, -ActiveAngle);

        float halfW = b.Width / 2f, halfH = b.Height / 2f;
        float uc = 0f, vc = 0f;
        const float minHalf = 4f;
        if (sx != 0)
        {
            float u0 = -sx * halfW;
            halfW = Math.Clamp(Math.Abs(local.X - u0) / 2f, minHalf, Math.Max(8f, _image.Width * 2f));
            uc = (u0 + local.X) / 2f;
        }
        if (sy != 0)
        {
            float v0 = -sy * halfH;
            halfH = Math.Clamp(Math.Abs(local.Y - v0) / 2f, minHalf, Math.Max(8f, _image.Height * 2f));
            vc = (v0 + local.Y) / 2f;
        }

        var shift = RotateVector(uc, vc, ActiveAngle);
        // 只把中心限制在画面内：允许框伸出画面边缘，贴着边框的目标（尤其带角度）才框得住。
        float cx = Math.Clamp(center.X + shift.X, 0f, _image.Width);
        float cy = Math.Clamp(center.Y + shift.Y, 0f, _image.Height);
        ActiveBounds = new RectangleF(cx - halfW, cy - halfH, halfW * 2f, halfH * 2f);
        if (_editIndex >= 0) _editChanged = true;
        Invalidate();
    }

    /// <summary>在框内拖动等于整体平移，大小和角度不变。</summary>
    private void MovePending(Point client)
    {
        if (_image == null) return;
        var p = ToImage(client);
        float dx = p.X - _moveStartImage.X, dy = p.Y - _moveStartImage.Y;
        var b = _moveStartBounds;
        MovePendingCenterTo(b.X + b.Width / 2f + dx, b.Y + b.Height / 2f + dy, b.Width, b.Height);
        Invalidate();
    }

    /// <summary>平移待确认框：中心限制在画面内，框本身可以探出画面，方便贴边目标。</summary>
    private void MovePendingCenterTo(float cx, float cy, float w, float h)
    {
        if (_image == null) return;
        cx = Math.Clamp(cx, 0f, _image.Width);
        cy = Math.Clamp(cy, 0f, _image.Height);
        ActiveBounds = new RectangleF(cx - w / 2f, cy - h / 2f, w, h);
        if (_editIndex >= 0) _editChanged = true;
    }

    /// <summary>方向键微调：待确认框直接挪；已保存框挪完立即写回（Shift 一次 10 像素）。</summary>
    private void NudgeActive(int dx, int dy)
    {
        if (_image == null) return;
        int step = (ModifierKeys & Keys.Shift) != 0 ? 10 : 1;
        if (_hasPending)
        {
            var pb = _pendingBounds;
            MovePendingCenterTo(pb.X + pb.Width / 2f + dx * step, pb.Y + pb.Height / 2f + dy * step, pb.Width, pb.Height);
            Invalidate();
            return;
        }
        if (_selected < 0 || _selected >= Boxes.Count) return;
        var box = Boxes[_selected];
        if (box.W <= 0 || box.H <= 0) return;
        BeginEdit(_selected);
        MovePendingCenterTo(box.X + box.W / 2f + dx * step, box.Y + box.H / 2f + dy * step, box.W, box.H);
        CommitEdit();
        Invalidate();
    }

    private bool IsOnActiveBody(Point client)
    {
        if (!HasActive || _image == null) return false;
        var b = ActiveBounds;
        return RotatedBox.Contains(b.X, b.Y, b.Width, b.Height, ActiveAngle, ToImage(client));
    }

    /// <summary>点中已保存框的手柄 / 框内时，进入编辑（拖动直接改大小、移动、旋转）。</summary>
    private bool TryBeginSavedEdit(Point location)
    {
        if (_image == null || _selected < 0 || _selected >= Boxes.Count) return false;
        var sb = Boxes[_selected];
        if (sb.W <= 0 || sb.H <= 0) return false;
        var pts = BoxClientPoints(sb.X, sb.Y, sb.W, sb.H, sb.Angle);

        if (OnRotateHandle(pts, location))
        {
            BeginEdit(_selected);
            StartRotate(location);
            return true;
        }

        int handle = HitResizeHandleIn(pts, location);
        if (handle >= 0)
        {
            BeginEdit(_selected);
            _resizeHandle = handle;
            Capture = true;
            Cursor = ResizeCursor(handle);
            Invalidate();
            return true;
        }

        if (RotatedBox.Contains(sb.X, sb.Y, sb.W, sb.H, sb.Angle, ToImage(location)))
        {
            BeginEdit(_selected);
            _movingPending = true;
            _moveStartImage = ToImage(location);
            _moveStartBounds = _editBounds;
            Capture = true;
            Cursor = Cursors.SizeAll;
            return true;
        }
        return false;
    }

    private void BeginEdit(int index)
    {
        if (index < 0 || index >= Boxes.Count) return;
        var b = Boxes[index];
        _editIndex = index;
        _editBounds = new RectangleF(b.X, b.Y, b.W, b.H);
        _editAngle = b.Angle;
        _editChanged = false;
        _selected = index;
    }

    private void StartRotate(Point location)
    {
        _rotating = true;
        _rotateStartAngle = ActiveAngle;
        _rotateStartPointer = PointerAngle(location);
        Capture = true;
        Cursor = Cursors.Hand;
        Invalidate();
    }

    /// <summary>把拖动结果抛给上层写回 annotations.json 与 YOLO txt；返回是否真的改过。</summary>
    private bool CommitEdit()
    {
        if (_editIndex < 0) return false;
        int index = _editIndex;
        var bounds = Rectangle.Round(_editBounds);
        float angle = _editAngle;
        bool changed = _editChanged;
        _editIndex = -1;
        _editChanged = false;
        Invalidate();
        if (!changed || bounds.Width < 4 || bounds.Height < 4) return false;
        _selected = index;
        BoxEdited?.Invoke(this, new BoxEdit { Index = index, Bounds = bounds, Angle = angle });
        return true;
    }

    private static PointF RotateVector(float x, float y, float degrees)
    {
        if (Math.Abs(degrees) < 0.001f) return new PointF(x, y);
        double rad = degrees * Math.PI / 180.0;
        float cos = (float)Math.Cos(rad), sin = (float)Math.Sin(rad);
        return new PointF(x * cos - y * sin, x * sin + y * cos);
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        if (_panning)
        {
            if (_panButton == MouseButtons.Right && !_rightMoved)
            {
                int dx = e.X - _rightDownPoint.X, dy = e.Y - _rightDownPoint.Y;
                if (dx * dx + dy * dy > 16) _rightMoved = true;
            }
            _offset = new PointF(_panOffsetStart.X + (e.X - _panStart.X), _panOffsetStart.Y + (e.Y - _panStart.Y));
            Invalidate();
            return;
        }
        if (_rotating)
        {
            float delta = PointerAngle(e.Location) - _rotateStartPointer;
            float next = NormalizeAngle(_rotateStartAngle + delta);
            if ((ModifierKeys & Keys.Shift) != 0)
                next = NormalizeAngle((float)Math.Round(next / 15f) * 15f);
            else
                next = NormalizeAngle((float)Math.Round(next * 2f) / 2f);
            ActiveAngle = next;
            if (_editIndex >= 0) _editChanged = true;
            Invalidate();
            return;
        }
        if (_resizeHandle >= 0)
        {
            ResizePending(_resizeHandle, e.Location);
            return;
        }
        if (_movingPending)
        {
            MovePending(e.Location);
            return;
        }
        if (LineEditable && _lineStart != null)
        {
            _lineHover = ClampImage(ToImagePoint(e.Location));
            Invalidate();
            return;
        }
        if (_dragging)
        {
            _dragCurrent = e.Location;
            Invalidate();
            return;
        }
        if (IsOnRotateHandle(e.Location))
        {
            Cursor = Cursors.Hand;
            return;
        }
        if (_hasPending)
        {
            int handle = HitResizeHandle(e.Location);
            if (handle >= 0)
            {
                Cursor = ResizeCursor(handle);
                return;
            }
            if (IsOnActiveBody(e.Location))
            {
                Cursor = Cursors.SizeAll;
                return;
            }
        }
        // 选中的已保存框：光标提示可以直接拖手柄 / 框内移动。
        if (Editable && _editIndex < 0 && _selected >= 0 && _selected < Boxes.Count)
        {
            var sb = Boxes[_selected];
            if (sb.W > 0 && sb.H > 0)
            {
                var pts = BoxClientPoints(sb.X, sb.Y, sb.W, sb.H, sb.Angle);
                if (OnRotateHandle(pts, e.Location))
                {
                    Cursor = Cursors.Hand;
                    return;
                }
                int h = HitResizeHandleIn(pts, e.Location);
                if (h >= 0)
                {
                    Cursor = ResizeCursor(h);
                    return;
                }
                if (RotatedBox.Contains(sb.X, sb.Y, sb.W, sb.H, sb.Angle, ToImage(e.Location)))
                {
                    Cursor = Cursors.SizeAll;
                    return;
                }
            }
        }
        Cursor = (Editable || LineEditable) && _image != null ? Cursors.Cross : Cursors.Default;
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        base.OnMouseUp(e);
        if (_rotating && e.Button == MouseButtons.Left)
        {
            _rotating = false;
            Capture = false;
            Cursor = Cursors.Hand;
            if (CommitEdit()) return;
            Invalidate();
            return;
        }
        if ((_resizeHandle >= 0 || _movingPending) && e.Button == MouseButtons.Left)
        {
            _resizeHandle = -1;
            _movingPending = false;
            Capture = false;
            Cursor = Cursors.Cross;
            if (CommitEdit()) return;
            Invalidate();
            return;
        }
        if (_panning && (e.Button == MouseButtons.Right || e.Button == MouseButtons.Middle))
        {
            bool showMenu = e.Button == MouseButtons.Right && !_rightMoved &&
                            CanvasMenu != null && _image != null && !_rightMoved;
            _panning = false;
            _panButton = MouseButtons.None;
            Cursor = Cursors.Default;
            if (showMenu) CanvasMenu.Show(this, e.Location);
            return;
        }
        if (!_dragging || e.Button != MouseButtons.Left) return;

        _dragging = false;
        Capture = false;
        var band = Normalize(_dragStart, e.Location);
        Invalidate();

        // 只是单击、没有拖动：选中 / 取消选中已保存的框。
        if (Editable && band.Width < 5 && band.Height < 5)
        {
            SelectAt(e.Location);
            return;
        }

        if (_image == null || band.Width < 6 || band.Height < 6) return;

        var a = ToImage(band.Location);
        var b = ToImage(new Point(band.Right, band.Bottom));
        var rect = Rectangle.FromLTRB(
            (int)Math.Round(Math.Min(a.X, b.X)), (int)Math.Round(Math.Min(a.Y, b.Y)),
            (int)Math.Round(Math.Max(a.X, b.X)), (int)Math.Round(Math.Max(a.Y, b.Y)));
        rect.Intersect(new Rectangle(0, 0, _image.Width, _image.Height));
        if (rect.Width < 6 || rect.Height < 6) return;

        // 进入待确认状态：不立刻弹命名，允许先旋转。
        _pendingBounds = new RectangleF(rect.X, rect.Y, rect.Width, rect.Height);
        _pendingAngle = 0f;
        _hasPending = true;
        Cursor = Cursors.Cross;
        Invalidate();
    }

    protected override void OnMouseWheel(MouseEventArgs e)
    {
        base.OnMouseWheel(e);
        if (_image == null) return;
        float factor = e.Delta > 0 ? 1.15f : 1f / 1.15f;
        float next = Math.Clamp(_scale * factor, 0.02f, 12f);
        _offset = new PointF(
            e.X - (e.X - _offset.X) * (next / _scale),
            e.Y - (e.Y - _offset.Y) * (next / _scale));
        _scale = next;
        _autoFit = false;
        Invalidate();
    }

    protected override void OnDoubleClick(EventArgs e)
    {
        base.OnDoubleClick(e);
        _editIndex = -1;
        _editChanged = false;
        CancelPending();
        Fit();
    }

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        if (keyData == (Keys.Control | Keys.S) && SaveRequested != null && _image != null)
        {
            SaveRequested();
            return true;
        }
        if (_editIndex >= 0 && keyData == Keys.Escape)
        {
            _editIndex = -1;
            _editChanged = false;
            Invalidate();
            return true;
        }
        if (_hasPending || (Editable && _selected >= 0 && _selected < Boxes.Count))
        {
            if (keyData == Keys.Left) { NudgeActive(-1, 0); return true; }
            if (keyData == Keys.Right) { NudgeActive(1, 0); return true; }
            if (keyData == Keys.Up) { NudgeActive(0, -1); return true; }
            if (keyData == Keys.Down) { NudgeActive(0, 1); return true; }
        }
        if (_hasPending)
        {
            if (keyData == Keys.Enter)
            {
                ConfirmPending();
                return true;
            }
            if (keyData == Keys.Escape)
            {
                CancelPending();
                return true;
            }
        }
        return base.ProcessCmdKey(ref msg, keyData);
    }

    /// <summary>确认待选框：抛给上层去弹命名窗口。角度会一起带出去。</summary>
    public void ConfirmPending()
    {
        if (!_hasPending) return;
        var draft = new BoxDraft
        {
            Bounds = Rectangle.Round(_pendingBounds),
            Angle = _pendingAngle,
        };
        _hasPending = false;
        _rotating = false;
        _resizeHandle = -1;
        _movingPending = false;
        Capture = false;
        Invalidate();
        if (draft.Bounds.Width >= 6 && draft.Bounds.Height >= 6)
            BoxDrawn?.Invoke(this, draft);
    }

    /// <summary>取消待确认的框（Esc、重新框选、切换图片或操作其他内容时调用）。</summary>
    public void CancelPending()
    {
        if (!_hasPending && !_rotating) return;
        _hasPending = false;
        _rotating = false;
        _resizeHandle = -1;
        _movingPending = false;
        Capture = false;
        Invalidate();
    }

    private static float NormalizeAngle(float angle)
    {
        while (angle <= -180f) angle += 360f;
        while (angle > 180f) angle -= 360f;
        return angle;
    }

    private void SelectAt(Point client)
    {
        var p = ToImage(client);
        int hit = -1;
        for (int i = Detections.Count - 1; i >= 0; i--)
        {
            var d = Detections[i];
            if (new RectangleF(d.X, d.Y, d.W, d.H).Contains(p)) { hit = i; break; }
        }
        if (hit < 0 && Editable)
        {
            for (int i = Boxes.Count - 1; i >= 0; i--)
            {
                var b = Boxes[i];
                if (RotatedBox.Contains(b.X, b.Y, b.W, b.H, b.Angle, p)) { hit = i; break; }
            }
        }
        SelectedIndex = hit;
        SelectionChanged?.Invoke(this, hit);
    }
}
