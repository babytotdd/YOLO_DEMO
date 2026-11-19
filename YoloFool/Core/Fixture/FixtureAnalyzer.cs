using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using OpenCvSharp;
using Point = OpenCvSharp.Point;
using Size = OpenCvSharp.Size;

namespace YoloFool;

internal sealed class FixtureOutcome
{
    public FixtureResult Result { get; init; }
    public Mat Overlay { get; init; }
}

/// <summary>
/// 固定相机 + 金样板几何比对。
/// 不做“图像相似度百分比”，而是把产品轮廓相对金样板的平移、旋转、面积、IoU、
/// 安全区越界量都换算成毫米/度，再逐项和配方公差比较。
/// </summary>
internal static class FixtureAnalyzer
{
    public static Rect RoiPixels(FixtureRecipe recipe, int width, int height) =>
        (recipe.Roi ?? new FixtureRoi()).ToPixels(width, height);

    /// <summary>用于采集金样板掩码：与在线判定完全相同的分割路径。</summary>
    public static Mat BuildMask(Mat frame, FixtureRecipe recipe, Mat empty)
    {
        using var bgr = ToBgr(frame);
        var roi = RoiPixels(recipe, bgr.Width, bgr.Height);
        return BuildMaskFrom(bgr, recipe, roi, FitSizeOrNull(empty, bgr.Size()));
    }

    public static FixtureOutcome Analyze(Mat frame, FixtureRecipe recipe, Mat golden, Mat goldenMask, Mat empty)
    {
        var sw = Stopwatch.StartNew();
        var result = new FixtureResult
        {
            Time = DateTime.Now,
            Model = recipe.Model,
            FrameWidth = frame.Width,
            FrameHeight = frame.Height,
        };

        using var bgr = ToBgr(frame);
        var roi = RoiPixels(recipe, bgr.Width, bgr.Height);
        Mat goldenUse = null, maskUse = null, emptyUse = null, productMask = null, goldenBin = null;

        try
        {
            // 1) 图像质量闸门：全黑、过曝、失焦时任何几何量都不可信。
            using (var roiBgr = new Mat(bgr, roi))
            using (var gray = new Mat())
            {
                Cv2.CvtColor(roiBgr, gray, ColorConversionCodes.BGR2GRAY);
                var bad = CheckQuality(gray, recipe.Quality);
                if (bad != null) return FinishBad(bgr, roi, result, FixtureReason.LowQuality, bad, sw);
            }

            if (golden == null || golden.Empty() || goldenMask == null || goldenMask.Empty())
                return FinishBad(bgr, roi, result, FixtureReason.RecipeMismatch,
                    "缺少金样板或模板掩码，请先在“模板”页采集金样板", sw);

            goldenUse = FitSize(golden, bgr.Size(), false);
            maskUse = FitSize(goldenMask, bgr.Size(), true);
            emptyUse = FitSizeOrNull(empty, bgr.Size());

            goldenBin = ToBinaryMask(maskUse);
            var gContours = FindContours(goldenBin);
            if (gContours.Length == 0)
                return FinishBad(bgr, roi, result, FixtureReason.RecipeMismatch, "金样板掩码是空的，请重新采集金样板", sw);

            var gLargest = gContours.OrderByDescending(c => Cv2.ContourArea(c)).First();
            double gArea = Cv2.ContourArea(gLargest);
            if (gArea < 100)
                return FinishBad(bgr, roi, result, FixtureReason.RecipeMismatch, "金样板轮廓太小，请检查分割参数", sw);

            var gRect = Cv2.MinAreaRect(gLargest);
            var gCenter = gRect.Center;
            double gAngle = RectAngle(gRect);

            double gScore = gRect.Size.Width * gRect.Size.Height > 0
                ? Math.Clamp(gArea / (gRect.Size.Width * gRect.Size.Height), 0, 1)
                : 1;

            double mmPerPixel = recipe.Calibration.MmPerPixel;
            if (mmPerPixel <= 0)
                return FinishBad(bgr, roi, result, FixtureReason.CalibInvalid, "还没有标定 mm/像素，请先在“标定”页标定", sw);

            // 2) 产品分割
            productMask = BuildMaskFrom(bgr, recipe, roi, emptyUse);
            var pContours = FindContours(productMask);
            if (pContours.Length == 0)
                return FinishBad(bgr, roi, result, FixtureReason.NoProduct, "画面里没有找到产品轮廓", sw);

            var ordered = pContours.OrderByDescending(c => Cv2.ContourArea(c)).ToArray();
            var pLargest = ordered[0];
            double pArea = Cv2.ContourArea(pLargest);
            if (pArea < 50)
                return FinishBad(bgr, roi, result, FixtureReason.NoProduct, "产品轮廓面积过小", sw);

            var pRect = Cv2.MinAreaRect(pLargest);
            var pCenter = pRect.Center;
            double pAngle = RectAngle(pRect);
            double pScore = pRect.Size.Width * pRect.Size.Height > 0
                ? Math.Clamp(pArea / (pRect.Size.Width * pRect.Size.Height), 0, 1)
                : 1;

            // 安全区（像素）模式：不做 mm 换算，只看产品外沿与安全区的关系。
            if (string.Equals(recipe.JudgeMode, "zone", StringComparison.OrdinalIgnoreCase))
                return AnalyzeZone(bgr, roi, recipe, result, productMask, pLargest, pCenter, gCenter,
                    ordered, pArea, sw);

            double dx = (pCenter.X - gCenter.X) * mmPerPixel;
            double dy = (pCenter.Y - gCenter.Y) * mmPerPixel;
            double dTheta = NormalizeDeg(pAngle - gAngle);
            double areaRatio = pArea / gArea;
            double iou = Math.Max(
                ComputeIoU(productMask, goldenBin, pCenter, pRect, gAngle, gCenter, mmPerPixel),
                ComputeIoU(productMask, goldenBin, pCenter, pRect, gAngle, gCenter, mmPerPixel, true));

            // 3) 安全区越界：金样板外扩 ZoneMarginMm，产品超出部分的最大穿透深度。
            double overflowMm = ComputeOverflow(productMask, goldenBin, gArea, recipe.Tolerance.ZoneMarginMm, mmPerPixel);

            bool multi = ordered.Length > 1 && Cv2.ContourArea(ordered[1]) > pArea * 0.15;

            var tol = recipe.Tolerance;
            // 充实度对遮挡/反光很敏感：以金样板自身为基线留余量，绝对下限只兜底。
            double contourLimit = Math.Max(tol.MinContourScore, gScore - Math.Max(0, tol.ContourScoreDrop));
            double devX = Math.Abs(dx) / Math.Max(tol.DxMm, 1e-6);
            double devY = Math.Abs(dy) / Math.Max(tol.DyMm, 1e-6);
            double devT = Math.Abs(dTheta) / Math.Max(tol.DThetaDeg, 1e-6);
            double devA = Math.Abs(areaRatio - 1) / Math.Max(tol.AreaRatioMax - 1, 1e-3);
            double devS = (1 - iou) / Math.Max(1 - tol.IoUMin, 1e-3);
            double confidence = Math.Clamp(1 - new[] { devX, devY, devT, devA, devS }.Max(), 0, 1);

            FixtureReason reason;
            string message;
            if (multi)
            {
                reason = FixtureReason.MultiObject;
                message = $"画面里有 {ordered.Length} 个物体，只允许一个产品";
            }
            else if (overflowMm > 0.05)
            {
                reason = FixtureReason.OutOfZone;
                message = $"越出安全区 {overflowMm:F2}mm（允许 0）";
            }
            else if (Math.Abs(dx) > tol.DxMm || Math.Abs(dy) > tol.DyMm)
            {
                reason = FixtureReason.Shift;
                message = $"位置偏移 X {dx:F2}mm / Y {dy:F2}mm（允许 ±{tol.DxMm:F1}/{tol.DyMm:F1}mm）";
            }
            else if (Math.Abs(dTheta) > tol.DThetaDeg)
            {
                reason = FixtureReason.Rotation;
                message = $"角度偏转 {dTheta:F2}°（允许 ±{tol.DThetaDeg:F1}°）";
            }
            else if (areaRatio < tol.AreaRatioMin || areaRatio > tol.AreaRatioMax)
            {
                reason = FixtureReason.AreaDefect;
                message = $"轮廓面积比 {areaRatio:F3}（允许 {tol.AreaRatioMin:F2}–{tol.AreaRatioMax:F2}）";
            }
            else if (iou < tol.IoUMin || pScore < contourLimit)
            {
                reason = FixtureReason.ShapeMismatch;
                message = $"形状不匹配 IoU {iou:F3} / 充实度 {pScore:F3}（下限 {contourLimit:F3}）";
            }
            else
            {
                reason = FixtureReason.Ok;
                message = "放置正确";
            }

            result.Ok = reason == FixtureReason.Ok;
            result.Reason = reason;
            result.Message = message;
            result.DxMm = dx;
            result.DyMm = dy;
            result.DThetaDeg = dTheta;
            result.AreaRatio = areaRatio;
            result.IoU = iou;
            result.OverflowMm = overflowMm;
            result.ContourScore = pScore;
            result.Confidence = confidence;
            result.ElapsedMs = sw.Elapsed.TotalMilliseconds;

            var overlay = bgr.Clone();
            Cv2.Rectangle(overlay, roi, new Scalar(140, 140, 140), 1);
            DrawRotRect(overlay, gRect, new Scalar(200, 200, 60), 1);
            DrawRotRect(overlay, pRect, result.Ok ? new Scalar(70, 190, 70) : new Scalar(60, 60, 220), 2);
            Cv2.Circle(overlay, new Point((int)gCenter.X, (int)gCenter.Y), 4, new Scalar(200, 200, 60), -1);
            Cv2.Circle(overlay, new Point((int)pCenter.X, (int)pCenter.Y), 4, new Scalar(60, 220, 240), -1);
            Cv2.Line(overlay,
                new Point((int)gCenter.X, (int)gCenter.Y),
                new Point((int)pCenter.X, (int)pCenter.Y),
                new Scalar(60, 220, 240), 1);
            DrawBanner(overlay, (result.Ok ? "OK  " : "NG  ") + result.ReasonCode,
                result.Ok ? new Scalar(70, 190, 70) : new Scalar(60, 60, 220));
            Cv2.PutText(overlay,
                $"dX {dx:F2}mm  dY {dy:F2}mm  dA {dTheta:F2}deg  IoU {iou:F3}  {result.ElapsedMs:F0}ms",
                new Point(10, 54), HersheyFonts.HersheySimplex, 0.55, new Scalar(30, 30, 30), 3, LineTypes.AntiAlias);
            Cv2.PutText(overlay,
                $"dX {dx:F2}mm  dY {dy:F2}mm  dA {dTheta:F2}deg  IoU {iou:F3}  {result.ElapsedMs:F0}ms",
                new Point(10, 54), HersheyFonts.HersheySimplex, 0.55, new Scalar(245, 245, 245), 1, LineTypes.AntiAlias);

            return new FixtureOutcome { Result = result, Overlay = overlay };
        }
        catch (Exception ex)
        {
            Log.Error("针床检测异常", ex);
            result.Ok = false;
            result.Reason = FixtureReason.InternalError;
            result.Message = "检测异常: " + ex.Message;
            result.ElapsedMs = sw.Elapsed.TotalMilliseconds;
            var overlay = bgr.Clone();
            Cv2.Rectangle(overlay, roi, new Scalar(140, 140, 140), 1);
            DrawBanner(overlay, "NG  INTERNAL_ERROR", new Scalar(60, 60, 220));
            return new FixtureOutcome { Result = result, Overlay = overlay };
        }
        finally
        {
            goldenUse?.Dispose();
            maskUse?.Dispose();
            emptyUse?.Dispose();
            productMask?.Dispose();
            goldenBin?.Dispose();
        }
    }

    private static FixtureOutcome FinishBad(Mat bgr, Rect roi, FixtureResult result,
        FixtureReason reason, string message, Stopwatch sw)
    {
        result.Ok = false;
        result.Reason = reason;
        result.Message = message;
        result.ElapsedMs = sw.Elapsed.TotalMilliseconds;
        var overlay = bgr.Clone();
        Cv2.Rectangle(overlay, roi, new Scalar(140, 140, 140), 1);
        DrawBanner(overlay, "NG  " + FixtureReasonText.Code(reason), new Scalar(60, 60, 220));
        // Cv2.PutText 不支持中文，message 是中文说明时改用 GDI+ 叠加，避免抛 "Cannot marshal"。
        if (OverlayText.IsAscii(message))
            Cv2.PutText(overlay, message, new Point(10, 54), HersheyFonts.HersheySimplex, 0.5,
                new Scalar(60, 60, 220), 1, LineTypes.AntiAlias);
        else
            OverlayText.Chip(overlay, 10, 34, message, Color.FromArgb(220, 60, 60), 12f);
        return new FixtureOutcome { Result = result, Overlay = overlay };
    }

    /// <summary>安全区（像素）判定：产品外沿必须留在矩形内，并留出边距；不依赖标定。</summary>
    private static FixtureOutcome AnalyzeZone(Mat bgr, Rect roi, FixtureRecipe recipe, FixtureResult result,
        Mat productMask, Point[] contour, Point2f pCenter, Point2f gCenter,
        Point[][] orderedContours, double pArea, Stopwatch sw)
    {
        var zone = recipe.Zone;
        var zr = zone.ToRect(bgr.Width, bgr.Height);
        var (left, right, top, bottom) = RobustExtents(contour);

        int overflow;
        double maxDepth = 0;
        int mL, mR, mT, mB;
        bool multi = orderedContours.Length > 1 && Cv2.ContourArea(orderedContours[1]) > pArea * 0.15;

        using (var zoneMask = new Mat(bgr.Size(), MatType.CV_8UC1, Scalar.All(0)))
        {
            Cv2.Rectangle(zoneMask, zr, Scalar.All(255), -1);
            using var outside = new Mat();
            Cv2.BitwiseNot(zoneMask, outside);
            using var overflowMask = new Mat();
            Cv2.BitwiseAnd(productMask, outside, overflowMask);
            overflow = Cv2.CountNonZero(overflowMask);

            if (overflow > 0)
            {
                // 越界深度：安全区外每个像素到安全区最近的像素距离；只统计越界像素。
                using var depthSrc = new Mat(bgr.Size(), MatType.CV_8UC1, Scalar.All(255));
                Cv2.Rectangle(depthSrc, zr, Scalar.All(0), -1);
                using var dist = new Mat();
                Cv2.DistanceTransform(depthSrc, dist, DistanceTypes.L2, DistanceTransformMasks.Mask3);
                using var masked = new Mat();
                dist.CopyTo(masked, overflowMask);
                Cv2.MinMaxLoc(masked, out _, out maxDepth, out _, out _);
            }

            mL = left - zr.X;
            mR = zr.Right - right;
            mT = top - zr.Y;
            mB = zr.Bottom - bottom;
            int minMargin = Math.Min(Math.Min(mL, mR), Math.Min(mT, mB));
            string side = minMargin == mL ? "左" : minMargin == mR ? "右" : minMargin == mT ? "上" : "下";

            double devO = overflow <= zone.OverflowPx
                ? 0
                : (overflow - zone.OverflowPx) / Math.Max(1.0, pArea * 0.02);
            double devM = minMargin >= zone.MarginPx
                ? 0
                : (zone.MarginPx - minMargin) / Math.Max(1.0, zone.MarginPx);
            double confidence = Math.Clamp(1 - Math.Max(devO, devM), 0, 1);

            FixtureReason reason;
            string message;
            if (multi)
            {
                reason = FixtureReason.MultiObject;
                message = $"画面里有 {orderedContours.Length} 个物体，只允许一个产品";
            }
            else if (overflow > zone.OverflowPx)
            {
                reason = FixtureReason.OutOfZone;
                message = $"产品越出安全区 {overflow}px（允许 {zone.OverflowPx}px，最大越界 {maxDepth:F0}px）";
            }
            else if (minMargin < zone.MarginPx)
            {
                reason = FixtureReason.EdgeMargin;
                message = $"产品离{side}边太近：最小边距 {minMargin}px（要求 ≥{zone.MarginPx}px）";
            }
            else
            {
                reason = FixtureReason.Ok;
                message = $"放置正确（最小边距 {minMargin}px）";
            }

            result.Ok = reason == FixtureReason.Ok;
            result.Reason = reason;
            result.Message = message;
            result.ZoneMode = true;
            result.ZoneLeftPx = mL;
            result.ZoneRightPx = mR;
            result.ZoneTopPx = mT;
            result.ZoneBottomPx = mB;
            result.ZoneOverflowPx = overflow;
            result.ZoneMaxDepthPx = maxDepth;
            result.ZoneMarginLimitPx = zone.MarginPx;
            result.DxPx = pCenter.X - gCenter.X;
            result.DyPx = pCenter.Y - gCenter.Y;
            result.Confidence = confidence;
            result.ElapsedMs = sw.Elapsed.TotalMilliseconds;

            var overlay = bgr.Clone();
            Cv2.Rectangle(overlay, roi, new Scalar(140, 140, 140), 1);
            DrawDashedRect(overlay, zr, new Scalar(0, 200, 255), 2);
            var color = result.Ok ? new Scalar(70, 190, 70) : new Scalar(60, 60, 220);
            Cv2.Polylines(overlay, new[] { contour }, false, color, 2);
            if (overflow > 0)
            {
                using var red = new Mat(overlay.Size(), MatType.CV_8UC3, new Scalar(0, 0, 220));
                red.CopyTo(overlay, overflowMask);
            }

            DrawBanner(overlay, result.Ok ? "OK" : "NG  " + result.ReasonCode, color);
            // OpenCV 的 Hershey 字体画不了中文，画面上的叠加文字一律用 ASCII；中文说明放在右侧面板。
            var text = $"L {mL}  R {mR}  T {mT}  B {mB} px   min {zone.MarginPx} px   out {overflow} px   {result.ElapsedMs:F0} ms";
            Cv2.PutText(overlay, text, new Point(10, 54), HersheyFonts.HersheySimplex, 0.55,
                new Scalar(30, 30, 30), 3, LineTypes.AntiAlias);
            Cv2.PutText(overlay, text, new Point(10, 54), HersheyFonts.HersheySimplex, 0.55,
                new Scalar(245, 245, 245), 1, LineTypes.AntiAlias);

            return new FixtureOutcome { Result = result, Overlay = overlay };
        }
    }

    /// <summary>轮廓的鲁棒外沿：取 1%/99% 分位，避免个别反光噪点把包围框拉大。</summary>
    private static (int Left, int Right, int Top, int Bottom) RobustExtents(Point[] contour)
    {
        if (contour == null || contour.Length == 0) return (0, 0, 0, 0);
        var xs = contour.Select(p => p.X).OrderBy(v => v).ToArray();
        var ys = contour.Select(p => p.Y).OrderBy(v => v).ToArray();
        return (Percentile(xs, 0.01), Percentile(xs, 0.99), Percentile(ys, 0.01), Percentile(ys, 0.99));
    }

    private static int Percentile(int[] sorted, double p)
    {
        if (sorted.Length == 0) return 0;
        int idx = (int)Math.Round((sorted.Length - 1) * p);
        return sorted[Math.Clamp(idx, 0, sorted.Length - 1)];
    }

    private static void DrawDashedRect(Mat img, Rect r, Scalar color, int thickness)
    {
        const int dash = 10;
        for (int x = r.X; x < r.Right; x += dash * 2)
            Cv2.Line(img, new Point(x, r.Y), new Point(Math.Min(x + dash, r.Right), r.Y), color, thickness);
        for (int x = r.X; x < r.Right; x += dash * 2)
            Cv2.Line(img, new Point(x, r.Bottom), new Point(Math.Min(x + dash, r.Right), r.Bottom), color, thickness);
        for (int y = r.Y; y < r.Bottom; y += dash * 2)
            Cv2.Line(img, new Point(r.X, y), new Point(r.X, Math.Min(y + dash, r.Bottom)), color, thickness);
        for (int y = r.Y; y < r.Bottom; y += dash * 2)
            Cv2.Line(img, new Point(r.Right, y), new Point(r.Right, Math.Min(y + dash, r.Bottom)), color, thickness);
    }

    // ------------------------------------------------------------ 分割

    private static Mat BuildMaskFrom(Mat bgr, FixtureRecipe recipe, Rect roi, Mat empty)
    {
        var full = new Mat(bgr.Size(), MatType.CV_8UC1, Scalar.All(0));
        try
        {
            using var roiMask = BuildRoiMask(bgr, recipe.Segment, roi);
            using (var dst = new Mat(full, roi)) roiMask.CopyTo(dst);

            var seg = recipe.Segment ?? new FixtureSegment();
            if (seg.UseEmptyDiff && seg.DiffThreshold > 0 && empty != null && !empty.Empty())
            {
                using var eg = new Mat();
                using var fg = new Mat();
                Cv2.CvtColor(empty, eg, ColorConversionCodes.BGR2GRAY);
                Cv2.CvtColor(bgr, fg, ColorConversionCodes.BGR2GRAY);
                using var diff = new Mat();
                Cv2.Absdiff(fg, eg, diff);
                using var changed = new Mat();
                Cv2.Threshold(diff, changed, Math.Clamp(seg.DiffThreshold, 1, 255), 255, ThresholdTypes.Binary);
                using var k = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(5, 5));
                Cv2.Dilate(changed, changed, k);
                Cv2.BitwiseAnd(full, changed, full);
            }

            return full;
        }
        catch
        {
            full.Dispose();
            throw;
        }
    }

    private static Mat BuildRoiMask(Mat bgr, FixtureSegment seg, Rect roi)
    {
        seg ??= new FixtureSegment();
        using var hsv = new Mat();
        using (var roiBgr = new Mat(bgr, roi))
            Cv2.CvtColor(roiBgr, hsv, ColorConversionCodes.BGR2HSV);

        var mask = new Mat(roi.Size, MatType.CV_8UC1, Scalar.All(0));
        try
        {
            var lo = new Scalar(seg.HMin, seg.SMin, seg.VMin);
            var hi = new Scalar(seg.HMax, seg.SMax, seg.VMax);
            if (seg.HMin <= seg.HMax)
            {
                Cv2.InRange(hsv, lo, hi, mask);
            }
            else
            {
                // 红色等跨 0 的色相：拆成两段再或。
                using var a = new Mat();
                using var b = new Mat();
                Cv2.InRange(hsv, new Scalar(0, seg.SMin, seg.VMin), new Scalar(seg.HMax, seg.SMax, seg.VMax), a);
                Cv2.InRange(hsv, new Scalar(seg.HMin, seg.SMin, seg.VMin), new Scalar(180, seg.SMax, seg.VMax), b);
                Cv2.BitwiseOr(a, b, mask);
            }

            int k = Math.Max(1, seg.MorphKernel | 1);
            if (k > 1)
            {
                using var kernel = Cv2.GetStructuringElement(MorphShapes.Ellipse, new Size(k, k));
                Cv2.MorphologyEx(mask, mask, MorphTypes.Open, kernel);
                Cv2.MorphologyEx(mask, mask, MorphTypes.Close, kernel);
            }

            return mask;
        }
        catch
        {
            mask.Dispose();
            throw;
        }
    }

    /// <summary>用金样板中心区域的颜色分布估计 HSV 范围，再用最大轮廓内部做一次修正。</summary>
    public static FixtureSegment EstimateSegment(Mat frame, FixtureRecipe recipe)
    {
        using var bgr = ToBgr(frame);
        var roi = RoiPixels(recipe, bgr.Width, bgr.Height);
        var seg = new FixtureSegment();

        var center = CenterRect(roi, 0.5);
        ApplyStats(bgr, center, seg);

        using var mask = BuildRoiMask(bgr, seg, roi);
        var contours = FindContours(mask);
        if (contours.Length > 0)
        {
            var largest = contours.OrderByDescending(c => Cv2.ContourArea(c)).First();
            if (Cv2.ContourArea(largest) > roi.Width * roi.Height * 0.02)
            {
                var r = Cv2.BoundingRect(largest);
                r.X += roi.X;
                r.Y += roi.Y;
                ApplyStats(bgr, CenterRect(r, 0.6), seg);
            }
        }

        seg.MorphKernel = 5;
        return seg;
    }

    private static void ApplyStats(Mat bgr, Rect region, FixtureSegment seg)
    {
        region = ClampRect(region, bgr.Width, bgr.Height);
        if (region.Width < 4 || region.Height < 4) return;

        using var hsv = new Mat();
        using (var roiBgr = new Mat(bgr, region))
            Cv2.CvtColor(roiBgr, hsv, ColorConversionCodes.BGR2HSV);
        Cv2.Split(hsv, out var ch);
        try
        {
            var (h1, h2) = Percentile(ch[0], 0.02, 0.98);
            var (s1, s2) = Percentile(ch[1], 0.02, 0.98);
            var (v1, v2) = Percentile(ch[2], 0.02, 0.98);

            seg.HMin = Math.Max(0, h1 - 4);
            seg.HMax = Math.Min(180, h2 + 4);
            seg.SMin = Math.Max(20, s1 - 20);
            seg.SMax = Math.Min(255, s2 + 20);
            seg.VMin = Math.Max(20, v1 - 20);
            seg.VMax = Math.Min(255, v2 + 20);
        }
        finally
        {
            foreach (var c in ch) c.Dispose();
        }
    }

    private static (int Lo, int Hi) Percentile(Mat channel, double loP, double hiP)
    {
        using var hist = new Mat();
        Cv2.CalcHist(new[] { channel }, new[] { 0 }, null, hist, 1, new[] { 256 },
            new[] { new Rangef(0, 256) });

        double total = channel.Rows * channel.Cols;
        double lo = total * loP, hi = total * hiP, acc = 0;
        int loV = 0, hiV = 255;
        bool loSet = false;
        for (int i = 0; i < 256; i++)
        {
            acc += hist.At<float>(i, 0);
            if (!loSet && acc >= lo) { loV = i; loSet = true; }
            if (acc >= hi) { hiV = i; break; }
        }
        if (hiV < loV) hiV = loV;
        return (loV, hiV);
    }

    // ------------------------------------------------------------ 几何

    private static double ComputeIoU(Mat productMask, Mat goldenBin, Point2f pCenter,
        RotatedRect pRect, double gAngle, Point2f gCenter, double mmPerPixel, bool flip = false)
    {
        double angle = NormalizeDeg(gAngle - RectAngle(pRect));
        if (flip) angle = -angle;

        using var m = Cv2.GetRotationMatrix2D(pCenter, angle, 1.0);
        m.Set(0, 2, m.At<double>(0, 2) + (gCenter.X - pCenter.X));
        m.Set(1, 2, m.At<double>(1, 2) + (gCenter.Y - pCenter.Y));

        using var warped = new Mat();
        Cv2.WarpAffine(productMask, warped, m, goldenBin.Size(),
            InterpolationFlags.Nearest, BorderTypes.Constant, Scalar.All(0));

        using var inter = new Mat();
        using var union = new Mat();
        Cv2.BitwiseAnd(warped, goldenBin, inter);
        Cv2.BitwiseOr(warped, goldenBin, union);
        double u = Cv2.CountNonZero(union);
        return u <= 0 ? 0 : Cv2.CountNonZero(inter) / u;
    }

    private static double ComputeOverflow(Mat productMask, Mat goldenBin, double goldenArea,
        double marginMm, double mmPerPixel)
    {
        int marginPx = Math.Max(2, (int)Math.Round(marginMm / Math.Max(mmPerPixel, 1e-6)));
        int k = Math.Min(301, marginPx * 2 + 1);

        using var kernel = Cv2.GetStructuringElement(MorphShapes.Ellipse, new Size(k, k));
        using var safe = new Mat();
        Cv2.Dilate(goldenBin, safe, kernel);

        using var outside = new Mat();
        Cv2.Subtract(productMask, safe, outside);
        int count = Cv2.CountNonZero(outside);
        if (count <= Math.Max(20, goldenArea * 0.002)) return 0;

        using var inv = new Mat();
        Cv2.BitwiseNot(safe, inv);
        using var dist = new Mat();
        Cv2.DistanceTransform(inv, dist, DistanceTypes.L2, DistanceTransformMasks.Mask3);
        Cv2.MinMaxLoc(dist, out _, out double maxDist, out _, out _, outside);
        return maxDist * mmPerPixel;
    }

    private static double RectAngle(RotatedRect r)
    {
        double a = r.Angle;
        if (r.Size.Width < r.Size.Height) a += 90;
        a %= 90;
        if (a > 45) a -= 90;
        if (a < -45) a += 90;
        return a;
    }

    private static double NormalizeDeg(double a)
    {
        a %= 360;
        if (a > 180) a -= 360;
        if (a < -180) a += 360;
        return a;
    }

    // ------------------------------------------------------------ 质量

    /// <summary>返回 null 表示合格，否则返回中文原因。</summary>
    public static string CheckQuality(Mat gray, FixtureQualityConfig q)
    {
        q ??= new FixtureQualityConfig();
        double mean = Cv2.Mean(gray).Val0;
        Cv2.MeanStdDev(gray, out _, out var std);
        double contrast = std.Val0;

        using var high = new Mat();
        Cv2.Threshold(gray, high, 245, 255, ThresholdTypes.Binary);
        double highlight = Cv2.CountNonZero(high) / (double)Math.Max(1, gray.Rows * gray.Cols);

        using var lap = new Mat();
        Cv2.Laplacian(gray, lap, MatType.CV_64F);
        Cv2.MeanStdDev(lap, out _, out var lapStd);
        double lapVar = lapStd.Val0 * lapStd.Val0;

        if (mean < q.MinMeanGray) return $"画面过暗（亮度 {mean:F0}，下限 {q.MinMeanGray:F0}）";
        if (mean > q.MaxMeanGray) return $"画面过亮（亮度 {mean:F0}，上限 {q.MaxMeanGray:F0}）";
        if (highlight > q.MaxHighlightRatio)
            return $"高光过曝面积 {highlight:P1}（上限 {q.MaxHighlightRatio:P0}），检查光源反射";
        if (contrast < q.MinContrast) return $"对比度过低（{contrast:F1}，下限 {q.MinContrast:F1}）";
        if (lapVar < q.MinLaplacianVar) return $"画面模糊或失焦（清晰度 {lapVar:F1}，下限 {q.MinLaplacianVar:F1}）";
        return null;
    }

    // ------------------------------------------------------------ 工具

    /// <summary>金样板自身的充实度（轮廓面积 / 最小外接矩形面积），用于相对判定与界面显示。</summary>
    public static double GoldenContourScore(Mat goldenMask)
    {
        if (goldenMask == null || goldenMask.Empty()) return 0;
        using var bin = ToBinaryMask(goldenMask);
        var contours = FindContours(bin);
        if (contours.Length == 0) return 0;
        var largest = contours.OrderByDescending(c => Cv2.ContourArea(c)).First();
        double area = Cv2.ContourArea(largest);
        var rect = Cv2.MinAreaRect(largest);
        double box = rect.Size.Width * rect.Size.Height;
        return box > 0 ? Math.Clamp(area / box, 0, 1) : 0;
    }

    private static Point[][] FindContours(Mat mask)
    {
        Cv2.FindContours(mask, out Point[][] contours, out _,
            RetrievalModes.External, ContourApproximationModes.ApproxSimple);
        return contours;
    }

    private static Mat ToBgr(Mat src)
    {
        var bgr = new Mat();
        switch (src.Channels())
        {
            case 4:
                Cv2.CvtColor(src, bgr, ColorConversionCodes.BGRA2BGR);
                break;
            case 3:
                src.CopyTo(bgr);
                break;
            default:
                Cv2.CvtColor(src, bgr, ColorConversionCodes.GRAY2BGR);
                break;
        }
        return bgr;
    }

    private static Mat ToBinaryMask(Mat mask)
    {
        var bin = new Mat();
        if (mask.Channels() == 1)
        {
            Cv2.Threshold(mask, bin, 127, 255, ThresholdTypes.Binary);
        }
        else
        {
            using var gray = new Mat();
            Cv2.CvtColor(mask, gray, ColorConversionCodes.BGR2GRAY);
            Cv2.Threshold(gray, bin, 127, 255, ThresholdTypes.Binary);
        }
        return bin;
    }

    private static Mat FitSize(Mat src, OpenCvSharp.Size size, bool nearest)
    {
        if (src.Size() == size)
        {
            var copy = new Mat();
            src.CopyTo(copy);
            return copy;
        }
        var dst = new Mat();
        Cv2.Resize(src, dst, size, 0, 0,
            nearest ? InterpolationFlags.Nearest : InterpolationFlags.Linear);
        return dst;
    }

    private static Mat FitSizeOrNull(Mat src, OpenCvSharp.Size size)
        => src == null || src.Empty() ? null : FitSize(src, size, false);

    private static Rect CenterRect(Rect r, double ratio)
    {
        int w = Math.Max(4, (int)Math.Round(r.Width * ratio));
        int h = Math.Max(4, (int)Math.Round(r.Height * ratio));
        int x = r.X + (r.Width - w) / 2;
        int y = r.Y + (r.Height - h) / 2;
        return new Rect(x, y, w, h);
    }

    private static Rect ClampRect(Rect r, int w, int h)
    {
        int x = Math.Clamp(r.X, 0, Math.Max(0, w - 1));
        int y = Math.Clamp(r.Y, 0, Math.Max(0, h - 1));
        int rw = Math.Clamp(r.Width, 1, w - x);
        int rh = Math.Clamp(r.Height, 1, h - y);
        return new Rect(x, y, rw, rh);
    }

    private static void DrawRotRect(Mat img, RotatedRect rect, Scalar color, int thickness)
    {
        var pts = rect.Points().Select(p => new Point((int)Math.Round(p.X), (int)Math.Round(p.Y))).ToArray();
        Cv2.Polylines(img, new[] { pts }, true, color, thickness, LineTypes.AntiAlias);
    }

    private static void DrawBanner(Mat img, string text, Scalar color)
    {
        var org = new Point(10, 30);
        Cv2.PutText(img, text, org, HersheyFonts.HersheyDuplex, 0.9, new Scalar(0, 0, 0), 4, LineTypes.AntiAlias);
        Cv2.PutText(img, text, org, HersheyFonts.HersheyDuplex, 0.9, color, 2, LineTypes.AntiAlias);
    }
}
