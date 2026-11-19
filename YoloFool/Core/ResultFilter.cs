using System;
using System.Collections.Generic;
using System.Linq;

namespace YoloFool;

/// <summary>结果筛除条件：匹配度下限、面积占比、长宽比、同类重叠去重、视频连续帧确认、类别屏蔽。</summary>
internal sealed class FilterOptions
{
    public bool Enabled { get; set; }
    public float MinAreaPercent { get; set; }
    public float MaxAreaPercent { get; set; } = 100f;
    public float MinAspect { get; set; }
    public float MaxAspect { get; set; }
    public float DedupIou { get; set; } = 0.6f;
    public int ConfirmFrames { get; set; } = 3;
    /// <summary>匹配度下限（百分数 0-100），0 表示不限。</summary>
    public float MinConfidencePercent { get; set; }
    public HashSet<string> Muted { get; } = new(StringComparer.OrdinalIgnoreCase);

    public bool HasGeometry => MinAreaPercent > 0 || MaxAreaPercent < 100f || MinAspect > 0 || MaxAspect > 0;

    public bool HasConfidence => MinConfidencePercent > 0;

    public static FilterOptions FromSettings(AppSettings s)
    {
        var o = new FilterOptions();
        if (s == null) return o;
        o.Enabled = s.FilterEnabled;
        o.MinAreaPercent = (float)Math.Clamp(s.FilterMinArea, 0, 100);
        o.MaxAreaPercent = (float)Math.Clamp(s.FilterMaxArea <= 0 ? 100 : s.FilterMaxArea, 0, 100);
        if (o.MaxAreaPercent < o.MinAreaPercent) (o.MinAreaPercent, o.MaxAreaPercent) = (o.MaxAreaPercent, o.MinAreaPercent);
        o.MinAspect = (float)Math.Clamp(s.FilterMinAspect, 0, 20);
        o.MaxAspect = (float)Math.Clamp(s.FilterMaxAspect, 0, 20);
        o.DedupIou = (float)Math.Clamp(s.FilterDedupIou, 0, 0.95);
        o.ConfirmFrames = Math.Clamp(s.FilterConfirmFrames, 1, 30);
        o.MinConfidencePercent = (float)Math.Clamp(s.FilterMinConfidence, 0, 100);
        foreach (var m in s.MutedClasses ?? new List<string>())
            if (!string.IsNullOrWhiteSpace(m)) o.Muted.Add(m.Trim());
        return o;
    }

    public string Summary
    {
        get
        {
            var parts = new List<string>();
            if (MinConfidencePercent > 0) parts.Add($"匹配度 ≥{MinConfidencePercent:F0}%");
            if (MinAreaPercent > 0 || MaxAreaPercent < 100f)
                parts.Add($"面积 {MinAreaPercent:F1}%–{MaxAreaPercent:F0}%");
            if (MinAspect > 0 || MaxAspect > 0)
                parts.Add($"长宽比 {MinAspect:F1}–{(MaxAspect > 0 ? MaxAspect.ToString("F1") : "∞")}");
            if (DedupIou > 0) parts.Add($"同类重叠 IoU>{DedupIou:F2} 去重");
            if (Muted.Count > 0) parts.Add($"屏蔽 {Muted.Count} 类");
            if (ConfirmFrames > 1) parts.Add($"连续 {ConfirmFrames} 帧确认");
            return parts.Count == 0 ? "未设置条件" : string.Join("，", parts);
        }
    }
}

/// <summary>纯函数的筛除逻辑，图片与视频共用。</summary>
internal static class ResultFilter
{
    public static List<DetectionItem> Apply(IReadOnlyList<DetectionItem> items, FilterOptions o,
        Func<string, int, string> display = null)
    {
        var list = items?.ToList() ?? new List<DetectionItem>();
        if (o == null || list.Count == 0) return list;

        if (o.Muted.Count > 0)
            list = list.Where(i => !IsMuted(i, o.Muted, display)).ToList();

        if (!o.Enabled) return list;

        if (o.HasConfidence)
            list = list.Where(i => (double)i.Confidence * 100.0 >= o.MinConfidencePercent).ToList();

        if (o.HasGeometry)
            list = list.Where(i => PassesGeometry(i, o)).ToList();

        if (o.DedupIou > 0)
            list = Dedup(list, o.DedupIou);

        return list;
    }

    public static bool IsMuted(DetectionItem item, ICollection<string> muted, Func<string, int, string> display = null)
    {
        if (item == null || muted == null || muted.Count == 0) return false;
        var raw = (item.RawCategory ?? "").Trim();
        var idText = item.ClassId.ToString();
        string shown = null;
        try { shown = display?.Invoke(item.RawCategory, item.ClassId); } catch { }
        foreach (var m in muted)
        {
            if (string.IsNullOrWhiteSpace(m)) continue;
            if (string.Equals(m, raw, StringComparison.OrdinalIgnoreCase)) return true;
            if (string.Equals(m, idText, StringComparison.Ordinal)) return true;
            if (!string.IsNullOrEmpty(shown) && string.Equals(m, shown, StringComparison.OrdinalIgnoreCase)) return true;
        }
        return false;
    }

    private static bool PassesGeometry(DetectionItem d, FilterOptions o)
    {
        if (d.W <= 0 || d.H <= 0) return false;
        double area = (double)d.W * d.H / Math.Max(1, (double)d.ImageWidth * d.ImageHeight) * 100.0;
        if (area < o.MinAreaPercent || area > o.MaxAreaPercent) return false;

        double aspect = (double)d.W / d.H;
        if (o.MinAspect > 0 && aspect < o.MinAspect) return false;
        if (o.MaxAspect > 0 && aspect > o.MaxAspect) return false;
        return true;
    }

    /// <summary>同类目、IoU 超过阈值时只保留分数最高的那个框。</summary>
    private static List<DetectionItem> Dedup(List<DetectionItem> items, float iouLimit)
    {
        var ordered = items.OrderByDescending(i => i.Confidence).ToList();
        var kept = new List<DetectionItem>(ordered.Count);
        foreach (var d in ordered)
        {
            bool dup = false;
            foreach (var k in kept)
            {
                if (k.ClassId != d.ClassId) continue;
                if (Iou(k, d) > iouLimit) { dup = true; break; }
            }
            if (!dup) kept.Add(d);
        }
        return kept;
    }

    public static double Iou(DetectionItem a, DetectionItem b)
    {
        int x1 = Math.Max(a.X, b.X);
        int y1 = Math.Max(a.Y, b.Y);
        int x2 = Math.Min(a.X + a.W, b.X + b.W);
        int y2 = Math.Min(a.Y + a.H, b.Y + b.H);
        int w = x2 - x1, h = y2 - y1;
        if (w <= 0 || h <= 0) return 0;
        double inter = (double)w * h;
        double union = (double)a.W * a.H + (double)b.W * b.H - inter;
        return union <= 0 ? 0 : inter / union;
    }
}
