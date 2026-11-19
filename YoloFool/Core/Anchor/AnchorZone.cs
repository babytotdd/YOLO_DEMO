using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using OpenCvSharp;
using CvPoint = OpenCvSharp.Point;

namespace YoloFool;

/// <summary>
/// 检测范围：画面里的一小块区域 + 一个标签名 + 匹配率下限。
/// 与锚点的分工：锚点回答“整块板有没有放到位”，检测范围回答“这个特征在不在、偏没偏”。
/// </summary>
internal sealed class AnchorZone
{
    public string Name { get; set; } = "";

    /// <summary>绑定到识别目标的标签名（例如 wifi）；留空表示只做模板匹配。</summary>
    public string TargetClass { get; set; } = "";

    public int X { get; set; }

    public int Y { get; set; }

    public int W { get; set; }

    public int H { get; set; }

    /// <summary>匹配率下限（0~1）。模板匹配得分低于它判不通过。</summary>
    public double MinMatch { get; set; } = 0.90;

    /// <summary>在区域四周外扩多少像素的窗口里找模板，用来容忍小幅位移。</summary>
    public int SearchPx { get; set; } = 24;

    /// <summary>找到的最佳位置与基准位置的最大偏差（像素）。</summary>
    public int MaxShiftPx { get; set; } = 12;

    /// <summary>绑定标签时的检出置信度下限。</summary>
    public double MinConf { get; set; } = 0.35;

    /// <summary>false 表示只提示不参与结论。</summary>
    public bool Required { get; set; } = true;

    /// <summary>采集参考图的时间，非空即可比对。</summary>
    public string RefAt { get; set; } = "";

    /// <summary>范围相对画面的旋转角（度）。板斜着放时也能贴着特征框，参考图会摆正后再存。</summary>
    public double Angle { get; set; }

    public void Normalize()
    {
        if (string.IsNullOrWhiteSpace(Name)) Name = "区域";
        TargetClass ??= "";
        if (W < 8) W = 8;
        if (H < 8) H = 8;
        MinMatch = MinMatch <= 0 ? 0.9 : Math.Clamp(MinMatch, 0.3, 0.999);
        SearchPx = Math.Clamp(SearchPx, 0, 160);
        MaxShiftPx = MaxShiftPx <= 0 ? 12 : Math.Clamp(MaxShiftPx, 0, 240);
        MinConf = MinConf <= 0 ? 0.35 : Math.Clamp(MinConf, 0.05, 0.99);
        if (double.IsNaN(Angle) || double.IsInfinity(Angle)) Angle = 0;
        Angle = Math.Clamp(Angle, -180.0, 180.0);
    }
}

internal sealed record AnchorZoneResult(
    string Name,
    bool Ok,
    string Code,
    string Text,
    double Score,
    int Dx,
    int Dy,
    double Conf,
    bool HasTarget,
    bool Required,
    int X,
    int Y,
    int W,
    int H,
    double Angle)
{
    public string ScoreText => Score > 0 ? (Score * 100).ToString("0.0") + "%" : "-";
}

internal sealed record AnchorZoneReport(
    bool Ok,
    string Message,
    int ElapsedMs,
    IReadOnlyList<AnchorZoneResult> Zones)
{
    public int PassCount => Zones.Count(z => z.Ok);

    public int FailCount => Zones.Count(z => !z.Ok);
}

internal static class AnchorZoneJudge
{
    public static AnchorZoneReport Judge(
        Mat frame,
        string model,
        IReadOnlyList<AnchorZone> zones,
        PredictionResult pred,
        IReadOnlyList<string> classes)
    {
        var sw = Stopwatch.StartNew();
        var results = new List<AnchorZoneResult>();
        var list = zones ?? Array.Empty<AnchorZone>();
        foreach (var zone in list)
        {
            try
            {
                results.Add(Evaluate(frame, model, zone, pred, classes));
            }
            catch (Exception ex)
            {
                results.Add(new AnchorZoneResult(zone?.Name ?? "区域", false, "ERROR", "比对出错：" + ex.Message, 0, 0, 0, 0, false, true, 0, 0, 0, 0, 0));
            }
        }

        sw.Stop();
        var fails = results.Where(r => r.Required && !r.Ok).ToList();
        var ok = fails.Count == 0;
        string message;
        if (list.Count == 0) message = "还没有检测范围";
        else if (ok) message = results.Count + " 个检测范围全部通过";
        else message = "不通过 " + fails.Count + " 个：" + string.Join("、", fails.Take(3).Select(f => f.Name + " " + f.Text));
        return new AnchorZoneReport(ok, message, (int)sw.ElapsedMilliseconds, results);
    }

    private static AnchorZoneResult Evaluate(Mat frame, string model, AnchorZone zone, PredictionResult pred, IReadOnlyList<string> classes)
    {
        zone.Normalize();
        var x = Math.Clamp(zone.X, 0, Math.Max(0, frame.Width - 8));
        var y = Math.Clamp(zone.Y, 0, Math.Max(0, frame.Height - 8));
        var w = Math.Clamp(zone.W, 8, frame.Width - x);
        var h = Math.Clamp(zone.H, 8, frame.Height - y);
        var hasTarget = !string.IsNullOrWhiteSpace(zone.TargetClass);

        AnchorZoneResult Fail(string code, string text, double score = 0, int dx = 0, int dy = 0, double conf = 0) =>
            new(zone.Name, false, code, text, score, dx, dy, conf, hasTarget, zone.Required, x, y, w, h, zone.Angle);

        using var template = AnchorZoneStore.LoadTemplate(model, zone.Name);
        if (template == null || template.Empty())
            return Fail("NO_REF", "还没采集参考");

        var pad = Math.Clamp(zone.SearchPx, 0, 160);
        var wx = Math.Max(0, x - pad);
        var wy = Math.Max(0, y - pad);
        var ww = Math.Min(frame.Width - wx, w + pad * 2);
        var wh = Math.Min(frame.Height - wy, h + pad * 2);
        if (ww < template.Width || wh < template.Height)
            return Fail("TINY", "区域太小放不下参考图");

        double score = 0;
        int dx = 0;
        int dy = 0;
        using (var gray = ToGray(frame))
        using (var tpl = ToGray(template))
        using (var win = new Mat(gray, new Rect(wx, wy, ww, wh)))
        using (var aligned = AnchorZoneStore.AlignWindow(win, wx, wy, x, y, w, h, zone.Angle, ww, wh))
        using (var res = new Mat())
        {
            // 范围带角度时先把窗口按同角度摆正，模板是摆正后裁的，比较才有意义。
            Cv2.MatchTemplate(aligned, tpl, res, TemplateMatchModes.CCoeffNormed);
            Cv2.MinMaxLoc(res, out double _, out double maxVal, out CvPoint _, out CvPoint maxLoc);
            if (!double.IsNaN(maxVal) && !double.IsInfinity(maxVal))
            {
                score = Math.Clamp(maxVal, 0.0, 1.0);
                AnchorZoneStore.Unrotate(zone.Angle, maxLoc.X, maxLoc.Y, out var fdx, out var fdy);
                dx = (int)Math.Round(fdx);
                dy = (int)Math.Round(fdy);
            }
        }

        var code = "OK";
        var text = score.ToString("0.0%");
        if (score < zone.MinMatch)
        {
            code = "LOW_MATCH";
            text = score.ToString("0.0%") + " < " + zone.MinMatch.ToString("0.0%");
        }
        else if (Math.Abs(dx) > zone.MaxShiftPx || Math.Abs(dy) > zone.MaxShiftPx)
        {
            code = "SHIFT";
            text = "位置偏 (" + dx + "," + dy + ")";
        }

        double conf = 0;
        if (hasTarget)
        {
            var idx = ClassIndex(classes, zone.TargetClass);
            if (pred == null || pred.Items == null)
                return Fail("NO_MODEL", "模型没加载，无法校验 " + zone.TargetClass, score, dx, dy);
            if (idx < 0)
                return Fail("NO_CLASS", "模型里没有类别 " + zone.TargetClass, score, dx, dy);

            var best = pred.Items
                .Where(i => i.ClassId == idx && i.Confidence >= zone.MinConf)
                .Where(i => Inside(i, wx, wy, ww, wh))
                .OrderByDescending(i => i.Confidence)
                .FirstOrDefault();
            if (best == null)
                return Fail("NO_TARGET", "没检测到 " + zone.TargetClass, score, dx, dy);

            conf = best.Confidence;
            var mx = x + dx + template.Width / 2.0;
            var my = y + dy + template.Height / 2.0;
            var ox = best.X + best.W / 2.0 - mx;
            var oy = best.Y + best.H / 2.0 - my;
            if (Math.Abs(ox) > zone.MaxShiftPx || Math.Abs(oy) > zone.MaxShiftPx)
            {
                code = "SHIFT";
                text = zone.TargetClass + " 偏 (" + (int)Math.Round(ox) + "," + (int)Math.Round(oy) + ")";
            }
            else if (code == "OK")
            {
                text = score.ToString("0.0%") + " / " + zone.TargetClass + " " + (conf * 100).ToString("0") + "%";
            }
        }

        return new AnchorZoneResult(zone.Name, code == "OK", code, text, score, dx, dy, conf, hasTarget, zone.Required, x, y, w, h, zone.Angle);
    }

    private static bool Inside(DetectionItem item, int x, int y, int w, int h)
    {
        var cx = item.X + item.W / 2.0;
        var cy = item.Y + item.H / 2.0;
        return cx >= x && cx <= x + w && cy >= y && cy <= y + h;
    }

    private static int ClassIndex(IReadOnlyList<string> classes, string name)
    {
        if (classes == null || string.IsNullOrWhiteSpace(name)) return -1;
        for (var i = 0; i < classes.Count; i++)
            if (string.Equals(classes[i], name, StringComparison.OrdinalIgnoreCase)) return i;
        return -1;
    }

    private static Mat ToGray(Mat src)
    {
        var gray = new Mat();
        if (src.Channels() == 4) Cv2.CvtColor(src, gray, ColorConversionCodes.BGRA2GRAY);
        else if (src.Channels() == 3) Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);
        else src.CopyTo(gray);
        return gray;
    }
}

/// <summary>检测范围的参考图，落盘在 fixtures_v2/&lt;机种&gt;/zones/&lt;区域名&gt;.png。</summary>
internal static class AnchorZoneStore
{
    public static string ZonesDir(string model) => Path.Combine(AnchorStore.Dir(model), "zones");

    public static string Safe(string name)
    {
        var text = string.IsNullOrWhiteSpace(name) ? "zone" : name.Trim();
        foreach (var ch in Path.GetInvalidFileNameChars())
            text = text.Replace(ch, '_');
        if (text.Length > 40) text = text[..40];
        return text;
    }

    public static string TemplateFile(string model, string name) =>
        Path.Combine(ZonesDir(model), Safe(name) + ".png");

    public static bool HasTemplate(string model, string name) => File.Exists(TemplateFile(model, name));

    public static bool SaveTemplate(string model, string name, Mat frame, int x, int y, int w, int h, double angle = 0)
    {
        if (frame == null || frame.Empty()) return false;
        x = Math.Clamp(x, 0, frame.Width - 2);
        y = Math.Clamp(y, 0, frame.Height - 2);
        w = Math.Clamp(w, 2, frame.Width - x);
        h = Math.Clamp(h, 2, frame.Height - y);
        Directory.CreateDirectory(ZonesDir(model));

        if (Math.Abs(angle) < 0.05)
        {
            using var crop = new Mat(frame, new Rect(x, y, w, h));
            return Cv2.ImWrite(TemplateFile(model, name), crop);
        }

        // 带角度：按框的四个角做仿射，把斜着的特征摆正后裁成 w×h 模板。
        using var m = AffineOfCorners(x, y, w, h, angle, 0, 0, w, h);
        using var rotated = new Mat();
        Cv2.WarpAffine(frame, rotated, m, new OpenCvSharp.Size(w, h), InterpolationFlags.Linear, BorderTypes.Replicate);
        return Cv2.ImWrite(TemplateFile(model, name), rotated);
    }

    /// <summary>把带角度的框摆正：把 frame 坐标映射到“框左上角为 (0,0)”的坐标。</summary>
    private static Mat AffineOfCorners(int x, int y, int w, int h, double angle, int offX, int offY, int dstW, int dstH)
    {
        var c = RotatedBox.Corners(x, y, w, h, (float)angle);
        var src = new[]
        {
            new Point2f((float)c[0].X - offX, (float)c[0].Y - offY),
            new Point2f((float)c[1].X - offX, (float)c[1].Y - offY),
            new Point2f((float)c[3].X - offX, (float)c[3].Y - offY),
        };
        var dst = new[] { new Point2f(0, 0), new Point2f(dstW, 0), new Point2f(0, dstH) };
        return Cv2.GetAffineTransform(src, dst);
    }

    /// <summary>判定时把搜索窗口按同一角度摆正，模板直接在上面滑窗。</summary>
    public static Mat AlignWindow(Mat win, int wx, int wy, int x, int y, int w, int h, double angle, int ww, int wh)
    {
        using var m = AffineOfCorners(x, y, w, h, angle, wx, wy, w, h);
        var aligned = new Mat();
        Cv2.WarpAffine(win, aligned, m, new OpenCvSharp.Size(ww, wh), InterpolationFlags.Linear, BorderTypes.Replicate);
        return aligned;
    }

    /// <summary>把“摆正坐标系里的偏移”换回画面偏移（逆旋转）。</summary>
    public static void Unrotate(double angle, int dx, int dy, out double fdx, out double fdy)
    {
        if (Math.Abs(angle) < 0.05)
        {
            fdx = dx;
            fdy = dy;
            return;
        }
        var rad = angle * Math.PI / 180.0;
        var cos = Math.Cos(rad);
        var sin = Math.Sin(rad);
        fdx = dx * cos - dy * sin;
        fdy = dx * sin + dy * cos;
    }

    public static Mat LoadTemplate(string model, string name)
    {
        var file = TemplateFile(model, name);
        if (!File.Exists(file)) return null;
        var mat = Cv2.ImRead(file, ImreadModes.Unchanged);
        if (mat != null && !mat.Empty()) return mat;
        mat?.Dispose();
        return null;
    }

    public static void DeleteTemplate(string model, string name)
    {
        var file = TemplateFile(model, name);
        if (File.Exists(file)) File.Delete(file);
    }

    /// <summary>改名时把参考模板一起改，避免改名后“缺参考”。</summary>
    public static void RenameTemplate(string model, string oldName, string newName)
    {
        var from = TemplateFile(model, oldName);
        var to = TemplateFile(model, newName);
        if (!File.Exists(from) || string.Equals(from, to, StringComparison.OrdinalIgnoreCase)) return;
        Directory.CreateDirectory(ZonesDir(model));
        File.Move(from, to, true);
    }
}
