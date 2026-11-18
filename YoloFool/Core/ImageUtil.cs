using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace YoloFool;

internal static class ImageUtil
{
    private static readonly string[] Supported = { ".jpg", ".jpeg", ".png", ".bmp", ".webp" };

    public static bool IsSupported(string path) =>
        !string.IsNullOrWhiteSpace(path) &&
        Supported.Contains(Path.GetExtension(path), StringComparer.OrdinalIgnoreCase);

    /// <summary>读取图片但不锁文件（选择图片后用户可能想改名/删除）。</summary>
    public static Bitmap LoadBitmap(string path)
    {
        using var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        using var tmp = Image.FromStream(fs, false, false);
        var bmp = new Bitmap(tmp.Width, tmp.Height, PixelFormat.Format32bppArgb);
        using (var g = Graphics.FromImage(bmp))
        {
            g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
            g.DrawImage(tmp, new Rectangle(0, 0, bmp.Width, bmp.Height));
        }
        bmp.SetResolution(96, 96);
        return bmp;
    }

    public static Size GetSize(string path)
    {
        using var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        using var img = Image.FromStream(fs, false, false);
        return img.Size;
    }
}
