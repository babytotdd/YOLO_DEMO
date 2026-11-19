using System.Drawing;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using OpenCvSharp;
using CvScalar = OpenCvSharp.Scalar;

namespace YoloFool;

/// <summary>
/// 在 Mat 上画文字（支持中文）。Cv2.PutText 只能接受 ASCII，
/// 传入 "区域1 ✓ 96%" 这类文本会抛 "Cannot marshal: Encountered unmappable character"。
/// 这里用 GDI+ 画到小 Bitmap 再拷回 Mat，中文/全角符号都正常。
/// </summary>
internal static class OverlayText
{
    private const string FontName = "Microsoft YaHei UI";

    public static bool IsAscii(string text)
    {
        if (string.IsNullOrEmpty(text)) return true;
        foreach (var ch in text) if (ch > 127) return false;
        return true;
    }

    /// <summary>量出文字像素尺寸。</summary>
    public static System.Drawing.Size Measure(string text, float px = 13f, FontStyle style = FontStyle.Bold)
    {
        using var bmp = new Bitmap(1, 1);
        using var g = Graphics.FromImage(bmp);
        using var font = new Font(FontName, px, style, GraphicsUnit.Pixel);
        var size = g.MeasureString(text ?? string.Empty, font, int.MaxValue, StringFormat.GenericTypographic);
        return new System.Drawing.Size((int)Math.Ceiling(size.Width), (int)Math.Ceiling(size.Height));
    }

    /// <summary>画带底色的文字块（左上角 x,y），返回占用区域。</summary>
    public static Rect Chip(Mat canvas, int x, int y, string text, Color bg, float px = 13f, Color? fg = null)
    {
        if (canvas == null || string.IsNullOrEmpty(text)) return default;
        const int pad = 5;
        var size = Measure(text, px);
        var w = size.Width + pad * 2;
        var h = size.Height + pad * 2;
        if (w < 8 || h < 8) return default;
        w = Math.Min(w, canvas.Width);
        h = Math.Min(h, canvas.Height);
        var px0 = Math.Clamp(x, 0, Math.Max(0, canvas.Width - w));
        var py0 = Math.Clamp(y, 0, Math.Max(0, canvas.Height - h));

        using var bmp = new Bitmap(w, h, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
        using (var g = Graphics.FromImage(bmp))
        {
            g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            g.Clear(bg);
            using var brush = new SolidBrush(fg ?? Color.White);
            using var font = new Font(FontName, px, FontStyle.Bold, GraphicsUnit.Pixel);
            g.DrawString(text, font, brush, pad, pad, StringFormat.GenericTypographic);
        }
        Blit(bmp, canvas, px0, py0);
        return new Rect(px0, py0, w, h);
    }

    /// <summary>把 CvScalar（B,G,R）转成 Color。</summary>
    public static Color ToColor(CvScalar s) =>
        Color.FromArgb(255, (int)s.Val2, (int)s.Val1, (int)s.Val0);

    /// <summary>只写文字、不加底色：先在暗底片上画，再按 alpha 合成代价高，这里用“同色底”近似。</summary>
    public static Rect Text(Mat canvas, int x, int y, string text, Color fg, float px = 13f)
    {
        return Chip(canvas, x, y, text, Color.FromArgb(180, 0, 0, 0), px, fg);
    }

    private static void Blit(Bitmap bmp, Mat canvas, int x, int y)
    {
        if (canvas.Depth() != MatType.CV_8U) return;
        var channels = canvas.Channels();
        if (channels != 1 && channels != 3 && channels != 4) return;
        var w = bmp.Width;
        var h = bmp.Height;
        var x0 = Math.Clamp(x, 0, canvas.Width);
        var y0 = Math.Clamp(y, 0, canvas.Height);
        w = Math.Min(w, canvas.Width - x0);
        h = Math.Min(h, canvas.Height - y0);
        if (w <= 0 || h <= 0) return;

        var data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
            System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
        try
        {
            var src = new byte[bmp.Width * 3];
            var dst = new byte[w * channels];
            var step = (int)canvas.Step();
            for (var r = 0; r < h; r++)
            {
                Marshal.Copy(data.Scan0 + r * data.Stride, src, 0, src.Length);
                for (var c = 0; c < w; c++)
                {
                    var b = src[c * 3];
                    var g = src[c * 3 + 1];
                    var rr = src[c * 3 + 2];
                    if (channels == 3)
                    {
                        dst[c * 3] = b;
                        dst[c * 3 + 1] = g;
                        dst[c * 3 + 2] = rr;
                    }
                    else if (channels == 4)
                    {
                        dst[c * 4] = b;
                        dst[c * 4 + 1] = g;
                        dst[c * 4 + 2] = rr;
                        dst[c * 4 + 3] = 255;
                    }
                    else
                    {
                        dst[c] = (byte)((b * 29 + g * 150 + rr * 77) >> 8);
                    }
                }
                var dstPtr = IntPtr.Add(canvas.Data, (y0 + r) * step + x0 * channels);
                Marshal.Copy(dst, 0, dstPtr, dst.Length);
            }
        }
        finally
        {
            bmp.UnlockBits(data);
        }
    }
}
