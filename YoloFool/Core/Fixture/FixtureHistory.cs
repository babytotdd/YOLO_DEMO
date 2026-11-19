using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace YoloFool;

/// <summary>把每次判定写成 CSV，便于事后统计误判率；只追加，不改判定。</summary>
internal static class FixtureHistory
{
    private static readonly object Gate = new();

    public static void Append(FixtureResult r)
    {
        if (r == null || string.IsNullOrWhiteSpace(r.Model)) return;
        try
        {
            var dir = Path.Combine(AppPaths.FixturesDir, FixtureStore.Sanitize(r.Model), "history");
            Directory.CreateDirectory(dir);
            var file = Path.Combine(dir, $"history-{r.Time:yyyyMMdd}.csv");

            var sb = new StringBuilder();
            if (!File.Exists(file))
                sb.AppendLine("time,ok,reason,dx_mm,dy_mm,dtheta_deg,area_ratio,iou,overflow_mm,contour_score,confidence,elapsed_ms,frame_w,frame_h");

            sb.AppendFormat(
                CultureInfo.InvariantCulture,
                "{0:yyyy-MM-dd HH:mm:ss.fff},{1},{2},{3:F3},{4:F3},{5:F3},{6:F4},{7:F4},{8:F3},{9:F4},{10:F4},{11:F1},{12},{13}\n",
                r.Time, r.Ok ? 1 : 0, r.ReasonCode,
                r.DxMm, r.DyMm, r.DThetaDeg, r.AreaRatio, r.IoU, r.OverflowMm,
                r.ContourScore, r.Confidence, r.ElapsedMs, r.FrameWidth, r.FrameHeight);

            lock (Gate) File.AppendAllText(file, sb.ToString(), Encoding.UTF8);
        }
        catch
        {
            // 履历写失败不能影响判定
        }
    }
}
