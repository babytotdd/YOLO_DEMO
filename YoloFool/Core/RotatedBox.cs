using System;
using System.Drawing;

namespace YoloFool;

/// <summary>
/// 旋转框的几何工具。BoxRecord 只存“未旋转的基框 + 角度”，
/// 显示、命中测试和导出时再算出四个角点或水平外接框。
/// </summary>
internal static class RotatedBox
{
    public static PointF Rotate(float cx, float cy, float x, float y, float angleDeg)
    {
        if (Math.Abs(angleDeg) < 0.001f) return new PointF(x, y);
        double rad = angleDeg * Math.PI / 180.0;
        float cos = (float)Math.Cos(rad);
        float sin = (float)Math.Sin(rad);
        float dx = x - cx, dy = y - cy;
        return new PointF(cx + dx * cos - dy * sin, cy + dx * sin + dy * cos);
    }

    /// <summary>按顺时针角度返回 左上、右上、右下、左下 四个角点（图像坐标）。</summary>
    public static PointF[] Corners(float x, float y, float w, float h, float angleDeg)
    {
        if (Math.Abs(angleDeg) < 0.001f)
            return new[]
            {
                new PointF(x, y), new PointF(x + w, y),
                new PointF(x + w, y + h), new PointF(x, y + h),
            };

        float cx = x + w / 2f, cy = y + h / 2f;
        return new[]
        {
            Rotate(cx, cy, x, y, angleDeg),
            Rotate(cx, cy, x + w, y, angleDeg),
            Rotate(cx, cy, x + w, y + h, angleDeg),
            Rotate(cx, cy, x, y + h, angleDeg),
        };
    }

    /// <summary>旋转框的水平外接框。YOLO 检测标签只能用水平框，导出时用这个。</summary>
    public static RectangleF AxisAligned(float x, float y, float w, float h, float angleDeg)
    {
        if (Math.Abs(angleDeg) < 0.001f) return new RectangleF(x, y, w, h);
        var pts = Corners(x, y, w, h, angleDeg);
        float minX = float.MaxValue, minY = float.MaxValue;
        float maxX = float.MinValue, maxY = float.MinValue;
        foreach (var p in pts)
        {
            if (p.X < minX) minX = p.X;
            if (p.Y < minY) minY = p.Y;
            if (p.X > maxX) maxX = p.X;
            if (p.Y > maxY) maxY = p.Y;
        }
        return RectangleF.FromLTRB(minX, minY, maxX, maxY);
    }

    /// <summary>命中测试：把点反向旋转回基框坐标系再判断。</summary>
    public static bool Contains(float x, float y, float w, float h, float angleDeg, PointF p)
    {
        if (Math.Abs(angleDeg) < 0.001f) return new RectangleF(x, y, w, h).Contains(p);
        float cx = x + w / 2f, cy = y + h / 2f;
        double rad = -angleDeg * Math.PI / 180.0;
        float cos = (float)Math.Cos(rad);
        float sin = (float)Math.Sin(rad);
        float dx = p.X - cx, dy = p.Y - cy;
        float lx = cx + dx * cos - dy * sin;
        float ly = cy + dx * sin + dy * cos;
        return lx >= x && lx <= x + w && ly >= y && ly <= y + h;
    }
}
