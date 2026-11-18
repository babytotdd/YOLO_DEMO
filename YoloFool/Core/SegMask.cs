using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using OpenCvSharp;
using Size = OpenCvSharp.Size;

namespace YoloFool;

/// <summary>一个检测框对应的分割掩码。掩码是 0/255 的单通道图，尺寸可能等于原图，也可能等于框尺寸。</summary>
internal sealed class SegMaskInfo
{
    public SegMaskInfo(DetectionItem item, byte[] mask, int maskWidth, int maskHeight)
    {
        Item = item;
        Mask = mask;
        MaskWidth = Math.Max(1, maskWidth);
        MaskHeight = Math.Max(1, maskHeight);
        IsFullImage = MaskWidth == item.ImageWidth && MaskHeight == item.ImageHeight;

        long count = 0;
        for (int i = 0; i < mask.Length; i++)
            if (mask[i] > 127) count++;

        double scale = IsFullImage
            ? 1.0
            : (item.W / (double)MaskWidth) * (item.H / (double)MaskHeight);
        long area = (long)Math.Round(count * scale);
        long maxArea = Math.Max(1L, (long)item.ImageWidth * item.ImageHeight);
        AreaPixels = (int)Math.Min(maxArea, Math.Max(0L, area));
        AreaPercent = item.ImageWidth <= 0 || item.ImageHeight <= 0
            ? 0
            : AreaPixels / (double)(item.ImageWidth * item.ImageHeight) * 100.0;
    }

    public DetectionItem Item { get; }
    public byte[] Mask { get; }
    public int MaskWidth { get; }
    public int MaskHeight { get; }
    public bool IsFullImage { get; }
    public int AreaPixels { get; }
    public double AreaPercent { get; }
}

/// <summary>把分割掩码以半透明颜色合成到 BGR 帧上，输出给界面直接显示。</summary>
internal static class MaskRenderer
{
    private static readonly Scalar[] Palette =
    {
        new(40, 150, 255),
        new(80, 200, 90),
        new(255, 120, 70),
        new(190, 90, 255),
        new(0, 200, 200),
        new(255, 190, 0),
    };

    public static Mat Composite(Mat frame, IReadOnlyList<SegMaskInfo> masks)
    {
        if (frame == null || frame.Empty() || masks == null || masks.Count == 0)
            return frame?.Clone() ?? new Mat();

        var canvas = frame.Clone();
        for (int index = 0; index < masks.Count; index++)
        {
            var m = masks[index];
            if (m.Mask == null || m.Mask.Length == 0 || m.MaskWidth <= 0 || m.MaskHeight <= 0)
                continue;

            int x = m.IsFullImage ? 0 : m.Item.X;
            int y = m.IsFullImage ? 0 : m.Item.Y;
            int w = m.IsFullImage ? canvas.Width : m.Item.W;
            int h = m.IsFullImage ? canvas.Height : m.Item.H;
            if (x < 0) { w += x; x = 0; }
            if (y < 0) { h += y; y = 0; }
            w = Math.Min(w, canvas.Width - x);
            h = Math.Min(h, canvas.Height - y);
            if (w <= 0 || h <= 0) continue;

            using var src = new Mat(m.MaskHeight, m.MaskWidth, MatType.CV_8UC1);
            Marshal.Copy(m.Mask, 0, src.Data, m.Mask.Length);
            using var resized = new Mat();
            var mask = src;
            if (w != m.MaskWidth || h != m.MaskHeight)
            {
                Cv2.Resize(src, resized, new Size(w, h), 0, 0, InterpolationFlags.Nearest);
                mask = resized;
            }

            using var roi = new Mat(canvas, new Rect(x, y, w, h));
            using var color = new Mat(h, w, MatType.CV_8UC3, Palette[index % Palette.Length]);
            using var blended = new Mat();
            Cv2.AddWeighted(roi, 0.68, color, 0.32, 0, blended);
            using var binary = new Mat();
            Cv2.Threshold(mask, binary, 127, 255, ThresholdTypes.Binary);
            blended.CopyTo(roi, binary);
        }
        return canvas;
    }
}
