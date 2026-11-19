using System;
using System.Collections.Generic;
using System.Linq;

namespace YoloFool;

internal enum AnchorReason
{
    Ok,
    NoCalib,
    Config,
    NoProduct,
    MissingAnchor,
    Shift,
    Rotate,
    Scale,
    Deform,
    Zone,
    Internal,
}

internal static class AnchorReasonText
{
    public static string Code(AnchorReason reason) => reason switch
    {
        AnchorReason.Ok => "OK",
        AnchorReason.NoCalib => "NO_CALIB",
        AnchorReason.Config => "CONFIG",
        AnchorReason.NoProduct => "NO_PRODUCT",
        AnchorReason.MissingAnchor => "MISSING_ANCHOR",
        AnchorReason.Shift => "SHIFT",
        AnchorReason.Rotate => "ROTATE",
        AnchorReason.Scale => "SCALE",
        AnchorReason.Deform => "DEFORM",
        AnchorReason.Zone => "ZONE",
        _ => "INTERNAL",
    };

    public static string Name(AnchorReason reason) => reason switch
    {
        AnchorReason.Ok => "到位",
        AnchorReason.NoCalib => "还没标定",
        AnchorReason.Config => "配置不匹配",
        AnchorReason.NoProduct => "没有产品",
        AnchorReason.MissingAnchor => "锚点缺失",
        AnchorReason.Shift => "整体偏移",
        AnchorReason.Rotate => "角度偏转",
        AnchorReason.Scale => "尺度变化",
        AnchorReason.Deform => "锚点不一致",
        AnchorReason.Zone => "区域不匹配",
        _ => "内部错误",
    };
}

/// <summary>一个锚点：检出位置（框中心）与基准期望位置。</summary>
internal sealed record AnchorHit(
    string Cls,
    double Cx,
    double Cy,
    double Ex,
    double Ey,
    double Conf,
    double W,
    double H)
{
    public string ClsText => string.IsNullOrWhiteSpace(Cls) ? "?" : Cls;

    /// <summary>实际 − 期望（像素，右/下为正）。</summary>
    public double Dx => Cx - Ex;

    public double Dy => Cy - Ey;

    public double OffsetPx => Math.Sqrt(Dx * Dx + Dy * Dy);
}

internal sealed record AnchorVerdict(
    bool Ok,
    AnchorReason Reason,
    string Message,
    double MeanDx,
    double MeanDy,
    double RotDeg,
    double Scale,
    double RmsPx,
    int HitCount,
    int NeedCount,
    int FrameWidth,
    int FrameHeight,
    long ElapsedMs,
    DateTime Time,
    IReadOnlyList<AnchorHit> Hits)
{
    public string ReasonCode => AnchorReasonText.Code(Reason);

    public string ReasonName => AnchorReasonText.Name(Reason);

    /// <summary>X/Y 里绝对值更大的那个，用作“整体偏移”口径。</summary>
    public double MaxShiftPx => Math.Max(Math.Abs(MeanDx), Math.Abs(MeanDy));

    public IReadOnlyList<AnchorHit> HitList => Hits ?? Array.Empty<AnchorHit>();
}

/// <summary>
/// 锚点判定内核：只解单平移（2 参数）+ 残差，旋转/尺度用基准间距最大的一对锚点另算。
/// 两锚点的相似变换恰定、残差恒为 0，所以残差只在“平移模型”下才有意义。
/// </summary>
internal static class AnchorJudge
{
    public static AnchorVerdict Judge(PredictionResult result, AnchorProfile profile, int frameWidth, int frameHeight)
    {
        var items = result?.Items ?? (IReadOnlyList<DetectionItem>)Array.Empty<DetectionItem>();
        var now = DateTime.Now;
        var elapsed = result?.ElapsedMilliseconds ?? 0;

        if (profile == null || !profile.IsCalibrated)
        {
            return Fail(AnchorReason.NoCalib, "还没有拍基准：先放一块确认到位的板，点“拍基准”。", frameWidth, frameHeight, elapsed, now);
        }

        var calib = profile.Calib;
        if (calib.Width > 0 && (calib.Width != frameWidth || calib.Height != frameHeight))
        {
            return Fail(AnchorReason.Config,
                $"画面尺寸变了（基准 {calib.Width}×{calib.Height}，当前 {frameWidth}×{frameHeight}），请重新拍基准。",
                frameWidth, frameHeight, elapsed, now);
        }

        var tol = profile.Tolerance ?? new AnchorTolerance();
        var need = Math.Max(1, tol.MinAnchors);
        var conf = Math.Clamp(profile.Infer?.Conf ?? 0.35, 0.01, 0.99);

        // 每个类别只留分数最高的一个，避免同类多框把均值带偏。
        var best = new Dictionary<string, DetectionItem>(StringComparer.OrdinalIgnoreCase);
        foreach (var item in items)
        {
            if (item == null || item.Confidence < conf) continue;
            var key = item.RawCategory ?? "";
            if (!best.TryGetValue(key, out var current) || item.Confidence > current.Confidence) best[key] = item;
        }

        var hits = new List<AnchorHit>();
        var missing = new List<string>();
        foreach (var anchor in calib.Anchors)
        {
            if (anchor == null) continue;
            if (!best.TryGetValue(anchor.Cls ?? "", out var detect))
            {
                missing.Add(string.IsNullOrWhiteSpace(anchor.Cls) ? "?" : anchor.Cls);
                continue;
            }
            hits.Add(new AnchorHit(
                anchor.Cls,
                detect.X + detect.W / 2.0,
                detect.Y + detect.H / 2.0,
                anchor.Ex,
                anchor.Ey,
                detect.Confidence,
                detect.W,
                detect.H));
        }

        if (hits.Count == 0)
        {
            return Fail(AnchorReason.NoProduct, "一个锚点都没检出：治具上可能没有放产品。", frameWidth, frameHeight, elapsed, now, hits, need);
        }

        var meanX = hits.Average(h => h.Dx);
        var meanY = hits.Average(h => h.Dy);
        var rms = hits.Count > 1
            ? Math.Sqrt(hits.Average(h => (h.Dx - meanX) * (h.Dx - meanX) + (h.Dy - meanY) * (h.Dy - meanY)))
            : 0;

        var rotDeg = 0.0;
        var scale = 1.0;
        if (hits.Count >= 2) (rotDeg, scale) = PairRotationScale(hits);

        var missText = missing.Count > 0 ? $"（本次未检出：{string.Join("、", missing)}）" : "";

        AnchorVerdict Build(AnchorReason reason, string message) => new(
            reason == AnchorReason.Ok,
            reason,
            message + missText,
            meanX,
            meanY,
            rotDeg,
            scale,
            rms,
            hits.Count,
            need,
            frameWidth,
            frameHeight,
            elapsed,
            now,
            hits);

        if (hits.Count < need)
        {
            return Build(AnchorReason.MissingAnchor, $"只检出 {hits.Count} 个锚点，至少需要 {need} 个。");
        }

        if (Math.Max(Math.Abs(meanX), Math.Abs(meanY)) > tol.ShiftPx)
        {
            return Build(AnchorReason.Shift,
                $"整体偏移 X {meanX:0.0} / Y {meanY:0.0} px，超过 ±{tol.ShiftPx:0.#} px。");
        }

        if (Math.Abs(rotDeg) > tol.RotDeg)
        {
            return Build(AnchorReason.Rotate, $"角度偏转 {rotDeg:0.00}°，超过 ±{tol.RotDeg:0.##}°。");
        }

        if (Math.Abs(scale - 1) > tol.ScaleTol)
        {
            return Build(AnchorReason.Scale,
                $"锚点间距变化 {(scale - 1) * 100:0.0}%，超过 ±{tol.ScaleTol * 100:0.##}%。");
        }

        if (hits.Count >= 2 && rms > tol.RmsPx)
        {
            return Build(AnchorReason.Deform, $"各锚点互不一致，RMS {rms:0.0} px，超过 {tol.RmsPx:0.#} px。");
        }

        var detail = hits.Count >= 2
            ? $"平移 X {meanX:0.0} / Y {meanY:0.0} px，旋转 {rotDeg:0.00}°，间距 {(scale - 1) * 100:0.0}%，RMS {rms:0.0} px。"
            : $"平移 X {meanX:0.0} / Y {meanY:0.0} px（只有一个锚点，不判旋转/尺度）。";
        return Build(AnchorReason.Ok, detail);
    }

    /// <summary>取基准间距最大的一对锚点算旋转与尺度，抗单框抖动。</summary>
    private static (double RotDeg, double Scale) PairRotationScale(IReadOnlyList<AnchorHit> hits)
    {
        var bi = 0;
        var bj = 1;
        var bestLen = -1.0;
        for (var i = 0; i < hits.Count; i++)
        {
            for (var j = i + 1; j < hits.Count; j++)
            {
                var length = Distance(hits[i].Ex, hits[i].Ey, hits[j].Ex, hits[j].Ey);
                if (length > bestLen)
                {
                    bestLen = length;
                    bi = i;
                    bj = j;
                }
            }
        }

        var a = hits[bi];
        var b = hits[bj];
        var ex = b.Ex - a.Ex;
        var ey = b.Ey - a.Ey;
        var cx = b.Cx - a.Cx;
        var cy = b.Cy - a.Cy;
        var lenExpected = Math.Sqrt(ex * ex + ey * ey);
        var lenCurrent = Math.Sqrt(cx * cx + cy * cy);
        var scale = lenExpected > 1e-6 ? lenCurrent / lenExpected : 1.0;
        var rot = NormalizeDeg(AngleDeg(cx, cy) - AngleDeg(ex, ey));
        if (!double.IsFinite(scale) || scale <= 0) scale = 1.0;
        if (!double.IsFinite(rot)) rot = 0;
        return (rot, scale);
    }

    private static double Distance(double x1, double y1, double x2, double y2)
    {
        var dx = x2 - x1;
        var dy = y2 - y1;
        return Math.Sqrt(dx * dx + dy * dy);
    }

    private static double AngleDeg(double dx, double dy) => Math.Atan2(dy, dx) * 180.0 / Math.PI;

    private static double NormalizeDeg(double deg)
    {
        var d = (deg + 180.0) % 360.0;
        if (d < 0) d += 360.0;
        return d - 180.0;
    }

    private static AnchorVerdict Fail(
        AnchorReason reason,
        string message,
        int frameWidth,
        int frameHeight,
        long elapsedMs,
        DateTime time,
        IReadOnlyList<AnchorHit> hits = null,
        int need = 0) => new(
        false,
        reason,
        message,
        0,
        0,
        0,
        1,
        0,
        hits?.Count ?? 0,
        need,
        frameWidth,
        frameHeight,
        elapsedMs,
        time,
        hits ?? Array.Empty<AnchorHit>());
}

/// <summary>
/// 同一块板连拍多次的抖动统计：用来给“平移上限”一个现场可用的建议值。
/// </summary>
internal sealed class AnchorJitter
{
    private readonly Queue<(double Dx, double Dy)> _samples = new();
    private const int Capacity = 20;

    public int Count => _samples.Count;

    public void Add(double dx, double dy)
    {
        _samples.Enqueue((dx, dy));
        while (_samples.Count > Capacity) _samples.Dequeue();
    }

    public void Clear() => _samples.Clear();

    /// <summary>样本够（≥5）时给出建议上限 = max(3, 4σ + 2)，σ 取两轴较大者。</summary>
    public bool TrySuggest(out double sigma, out double suggested, out string text)
    {
        sigma = 0;
        suggested = 0;
        text = "";
        var n = _samples.Count;
        if (n < 5) return false;

        var meanX = _samples.Average(s => s.Dx);
        var meanY = _samples.Average(s => s.Dy);
        var varX = _samples.Average(s => (s.Dx - meanX) * (s.Dx - meanX));
        var varY = _samples.Average(s => (s.Dy - meanY) * (s.Dy - meanY));
        sigma = Math.Sqrt(Math.Max(varX, varY));
        suggested = Math.Max(3.0, Math.Round(4 * sigma + 2, 0));
        text = $"连拍 {n} 次抖动 σ≈{sigma:0.0} px → 建议平移上限 ≈{suggested:0} px";
        return true;
    }
}
