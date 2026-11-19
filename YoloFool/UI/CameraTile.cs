using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using YoloFool.Core.MultiCam;

namespace YoloFool;

/// <summary>
/// 监控墙里的一格：画这一路的最新画面 + 叠在画面上的占用指标。
/// 画面数据由采集线程缩放到“格子尺寸”后放在 byte[] 里，这里只做一次拷贝进 Bitmap，
/// 所有 GDI+ 操作都留在 UI 线程，避免跨线程写同一个 Bitmap。
/// </summary>
internal sealed class CameraTile : Control
{
    private static readonly Font NameFont = new("Microsoft YaHei UI", 9.5f, FontStyle.Bold);
    private static readonly Font InfoFont = new("Microsoft YaHei UI", 8.5f);

    private static readonly SolidBrush BarBrush = new(Color.FromArgb(165, 0, 0, 0));
    private static readonly SolidBrush TextBrush = new(Color.FromArgb(240, 244, 250));
    private static readonly SolidBrush DimBrush = new(Color.FromArgb(178, 186, 198));
    private static readonly SolidBrush ErrBrush = new(Color.FromArgb(255, 120, 120));
    private static readonly SolidBrush OkBrush = new(Color.FromArgb(120, 230, 160));
    private static readonly SolidBrush WarnBrush = new(Color.FromArgb(255, 200, 90));
    private static readonly SolidBrush IdleBrush = new(Color.FromArgb(150, 160, 175));
    private static readonly SolidBrush EmptyBrush = new(Color.FromArgb(58, 64, 74));

    private readonly Pen _selPen = new(Theme.Accent, 2f);
    private readonly Pen _idlePen = new(Color.FromArgb(70, 78, 90), 1f);

    private Bitmap _bmp;
    private byte[] _buf;
    private long _lastId = -1;

    public CameraTile(CameraStream stream)
    {
        Stream = stream;
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                 ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
        BackColor = Color.FromArgb(30, 34, 41);
        Cursor = Cursors.Hand;
        TabStop = false;
    }

    public CameraStream Stream { get; }

    public bool Selected { get; set; }

    /// <summary>把最新一帧搬进本地 Bitmap，返回有没有新画面（没新画面就不用重绘）。</summary>
    public bool SyncFrame()
    {
        if (!Stream.TryCopyFrame(ref _buf, out int w, out int h, ref _lastId)) return false;
        if (_bmp == null || _bmp.Width != w || _bmp.Height != h)
        {
            _bmp?.Dispose();
            _bmp = new Bitmap(w, h, PixelFormat.Format24bppRgb);
        }

        var data = _bmp.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
        try
        {
            int rowBytes = w * 3;
            if (data.Stride == rowBytes)
            {
                Marshal.Copy(_buf, 0, data.Scan0, _buf.Length);
            }
            else
            {
                for (int y = 0; y < h; y++)
                    Marshal.Copy(_buf, y * rowBytes, data.Scan0 + y * data.Stride, rowBytes);
            }
        }
        finally
        {
            _bmp.UnlockBits(data);
        }
        return true;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.Clear(BackColor);
        g.SmoothingMode = SmoothingMode.None;

        var inner = ClientRectangle;
        inner.Inflate(-1, -1);

        if (_bmp != null)
        {
            var dest = FitRect(inner, _bmp.Width, _bmp.Height);
            g.InterpolationMode = InterpolationMode.Bilinear;
            g.PixelOffsetMode = PixelOffsetMode.HighSpeed;
            g.DrawImage(_bmp, dest);
        }

        var m = Stream.Snapshot();
        DrawOverlay(g, inner, m);
        g.DrawRectangle(Selected ? _selPen : _idlePen, inner);
    }

    private static Rectangle FitRect(Rectangle box, int w, int h)
    {
        if (w <= 0 || h <= 0) return box;
        double scale = Math.Min(box.Width / (double)w, box.Height / (double)h);
        int dw = Math.Max(1, (int)Math.Round(w * scale));
        int dh = Math.Max(1, (int)Math.Round(h * scale));
        return new Rectangle(box.X + (box.Width - dw) / 2, box.Y + (box.Height - dh) / 2, dw, dh);
    }

    private void DrawOverlay(Graphics g, Rectangle box, CameraMetrics m)
    {
        int pad = 8;
        int barH = 62;
        g.FillRectangle(BarBrush, box.X, box.Y, box.Width, Math.Min(barH, box.Height));
        g.FillRectangle(StateBrush(m.State), box.X + pad, box.Y + pad + 5, 8, 8);

        string title = "#" + Stream.DeviceIndex + "  " + StateText(m);
        g.DrawString(title, NameFont, m.State == CameraState.Failed ? ErrBrush : TextBrush,
            box.X + pad + 14, box.Y + pad - 1);

        string line2, line3;
        if (m.State == CameraState.Running)
        {
            line2 = (m.SrcWidth > 0 ? m.SrcWidth + "×" + m.SrcHeight : "—") +
                    (m.SizeHonored || m.SrcWidth == 0 ? "" : " ← " + m.ReqWidth + "×" + m.ReqHeight) +
                    "   " + m.Fps.ToString("0.0") + " fps" +
                    (m.PixelFormat.Length > 0 ? "   " + m.PixelFormat : "");
            line3 = "CPU 均 " + m.CpuAvgPercent.ToString("0.0") + "%  峰值 " + m.PeakCpuPercent.ToString("0.0") +
                    "%  缓冲 " + m.BufferMb.ToString("0.0") + "MB";
        }
        else if (m.State == CameraState.Opening)
        {
            line2 = "打开中… " + m.ElapsedSec.ToString("0.0") + "s";
            line3 = "有些 index 打不开，会卡一会儿才报错";
        }
        else if (m.State == CameraState.Failed)
        {
            line2 = Shorten(m.Error, 34);
            line3 = "点右侧“关闭选中”可以释放这一路";
        }
        else
        {
            line2 = "未打开";
            line3 = "右侧选“打开选中”开始采集";
        }

        g.DrawString(line2, InfoFont, m.State == CameraState.Failed ? ErrBrush : DimBrush,
            box.X + pad, box.Y + pad + 20);
        g.DrawString(line3, InfoFont, DimBrush, box.X + pad, box.Y + pad + 38);

        if (_bmp == null)
        {
            var center = new RectangleF(box.X, box.Y + box.Height / 2f - 12, box.Width, 24);
            using var sf = new StringFormat { Alignment = StringAlignment.Center };
            g.DrawString("无画面", InfoFont, EmptyBrush, center, sf);
        }
    }

    private static string StateText(CameraMetrics m) => m.State switch
    {
        CameraState.Running => "运行",
        CameraState.Opening => "打开中",
        CameraState.Failed => "失败",
        _ => "空闲",
    };

    private static Brush StateBrush(CameraState state) => state switch
    {
        CameraState.Running => OkBrush,
        CameraState.Opening => WarnBrush,
        CameraState.Failed => ErrBrush,
        _ => IdleBrush,
    };

    private static string Shorten(string s, int max)
    {
        if (string.IsNullOrEmpty(s)) return "";
        s = s.Replace("\r", " ").Replace("\n", " ");
        return s.Length <= max ? s : s.Substring(0, max) + "…";
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _bmp?.Dispose();
            _selPen.Dispose();
            _idlePen.Dispose();
        }
        base.Dispose(disposing);
    }
}
