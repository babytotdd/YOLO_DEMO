using System;
using OpenCvSharp;

namespace YoloFool;

/// <summary>针床检测的判定原因码。顺序即优先级，越靠前越先判定。</summary>
internal enum FixtureReason
{
    Ok,
    CameraLost,
    LowQuality,
    NoProduct,
    MultiObject,
    OutOfZone,
    EdgeMargin,
    Shift,
    Rotation,
    AreaDefect,
    ShapeMismatch,
    CalibInvalid,
    RecipeMismatch,
    InternalError,
}

internal static class FixtureReasonText
{
    public static string Code(FixtureReason r) => r switch
    {
        FixtureReason.Ok => "OK",
        FixtureReason.CameraLost => "CAMERA_LOST",
        FixtureReason.LowQuality => "LOW_QUALITY",
        FixtureReason.NoProduct => "NO_PRODUCT",
        FixtureReason.MultiObject => "MULTI_OBJECT",
        FixtureReason.OutOfZone => "OUT_OF_ZONE",
        FixtureReason.EdgeMargin => "EDGE_MARGIN",
        FixtureReason.Shift => "SHIFT",
        FixtureReason.Rotation => "ROTATION",
        FixtureReason.AreaDefect => "AREA_DEFECT",
        FixtureReason.ShapeMismatch => "SHAPE_MISMATCH",
        FixtureReason.CalibInvalid => "CALIB_INVALID",
        FixtureReason.RecipeMismatch => "RECIPE_MISMATCH",
        _ => "INTERNAL_ERROR",
    };

    public static string Name(FixtureReason r) => r switch
    {
        FixtureReason.Ok => "放置正确",
        FixtureReason.CameraLost => "相机无画面",
        FixtureReason.LowQuality => "图像质量不合格",
        FixtureReason.NoProduct => "未检测到产品",
        FixtureReason.MultiObject => "检测到多个物体",
        FixtureReason.OutOfZone => "越出安全区",
        FixtureReason.EdgeMargin => "边距不足",
        FixtureReason.Shift => "位置偏移",
        FixtureReason.Rotation => "角度偏转",
        FixtureReason.AreaDefect => "轮廓面积异常",
        FixtureReason.ShapeMismatch => "形状不匹配",
        FixtureReason.CalibInvalid => "标定无效",
        FixtureReason.RecipeMismatch => "配方不完整",
        _ => "内部错误",
    };
}

internal sealed class FixtureResult
{
    public bool Ok { get; set; }
    public FixtureReason Reason { get; set; } = FixtureReason.Ok;
    public string Message { get; set; } = "";
    public double DxMm { get; set; }
    public double DyMm { get; set; }
    public double DThetaDeg { get; set; }
    public double AreaRatio { get; set; } = 1;
    public double IoU { get; set; } = 1;
    public double OverflowMm { get; set; }

    // 安全区（像素）模式专用：不依赖标定，直接比外沿到安全区的距离和越界面积。
    public bool ZoneMode { get; set; }
    public int ZoneLeftPx { get; set; }
    public int ZoneRightPx { get; set; }
    public int ZoneTopPx { get; set; }
    public int ZoneBottomPx { get; set; }
    public int ZoneOverflowPx { get; set; }
    public double ZoneMaxDepthPx { get; set; }
    public int ZoneMarginLimitPx { get; set; }
    public double DxPx { get; set; }
    public double DyPx { get; set; }

    public double ContourScore { get; set; } = 1;
    public double Confidence { get; set; } = 1;
    public double ElapsedMs { get; set; }
    public DateTime Time { get; set; } = DateTime.Now;
    public string Model { get; set; } = "";
    public int FrameWidth { get; set; }
    public int FrameHeight { get; set; }

    public string ReasonCode => FixtureReasonText.Code(Reason);
    public string ReasonName => FixtureReasonText.Name(Reason);
}

internal sealed class FixtureCameraConfig
{
    public int Index { get; set; }
    public int Width { get; set; } = 1280;
    public int Height { get; set; } = 720;
    public string FourCC { get; set; } = "MJPG";
    public double? Exposure { get; set; }
    public double? Gain { get; set; }
}

internal sealed class FixtureQualityConfig
{
    public double MinMeanGray { get; set; } = 35;
    public double MaxMeanGray { get; set; } = 225;
    public double MaxHighlightRatio { get; set; } = 0.08;
    public double MinContrast { get; set; } = 12;
    public double MinLaplacianVar { get; set; } = 8;
}

internal sealed class FixtureTolerance
{
    public double DxMm { get; set; } = 5;
    public double DyMm { get; set; } = 5;
    public double DThetaDeg { get; set; } = 3.5;
    public double AreaRatioMin { get; set; } = 0.90;
    public double AreaRatioMax { get; set; } = 1.10;
    public double IoUMin { get; set; } = 0.85;
    public double ZoneMarginMm { get; set; } = 5;
    public double MinContourScore { get; set; } = 0.50;

    /// <summary>允许金样板充实度向下波动的余量；判定下限 = max(MinContourScore, 金样板充实度 − 本值)。</summary>
    public double ContourScoreDrop { get; set; } = 0.10;
}

/// <summary>
/// 安全区（像素）配置：产品外沿必须留在矩形内并保持边距。
/// 与 mm 公差互斥——用这个模式时不需要标定。
/// </summary>
internal sealed class FixtureZoneConfig
{
    public bool Enabled { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public int W { get; set; }
    public int H { get; set; }

    /// <summary>产品外沿到安全区边界至少要留多少像素；0 表示只查越界。</summary>
    public int MarginPx { get; set; } = 10;

    /// <summary>允许落在安全区外的像素数；0 表示一点都不能越界。</summary>
    public int OverflowPx { get; set; }

    public bool IsValid(int frameWidth, int frameHeight) =>
        Enabled && W >= 16 && H >= 16 && X + W <= frameWidth + 1 && Y + H <= frameHeight + 1;

    public Rect ToRect(int frameWidth, int frameHeight)
    {
        int w = Math.Clamp(W, 16, frameWidth);
        int h = Math.Clamp(H, 16, frameHeight);
        int x = Math.Clamp(X, 0, frameWidth - w);
        int y = Math.Clamp(Y, 0, frameHeight - h);
        return new Rect(x, y, w, h);
    }
}

/// <summary>颜色分割参数，HSV 全范围时等价于不做色相筛选。</summary>
internal sealed class FixtureSegment
{
    public double HMin { get; set; }
    public double HMax { get; set; } = 180;
    public double SMin { get; set; }
    public double SMax { get; set; } = 255;
    public double VMin { get; set; }
    public double VMax { get; set; } = 255;
    public int MorphKernel { get; set; } = 5;

    /// <summary>空治具差分的灰度差阈值，越小越敏感；0 表示不用差分。</summary>
    public int DiffThreshold { get; set; } = 18;

    /// <summary>是否用空治具图做差分（采过空治具才生效）。</summary>
    public bool UseEmptyDiff { get; set; } = true;
}

/// <summary>归一化 ROI（0~1），与分辨率无关；默认整幅画面。</summary>
internal sealed class FixtureRoi
{
    public double X { get; set; }
    public double Y { get; set; }
    public double W { get; set; } = 1;
    public double H { get; set; } = 1;

    public bool IsFull => X <= 0.002 && Y <= 0.002 && W >= 0.998 && H >= 0.998;

    public Rect ToPixels(int width, int height)
    {
        int w = Math.Max(32, Math.Min(width, (int)Math.Round(Math.Clamp(W, 0.01, 1) * width)));
        int h = Math.Max(32, Math.Min(height, (int)Math.Round(Math.Clamp(H, 0.01, 1) * height)));
        int x = (int)Math.Round(Math.Clamp(X, 0, 0.99) * width);
        int y = (int)Math.Round(Math.Clamp(Y, 0, 0.99) * height);
        if (x + w > width) x = Math.Max(0, width - w);
        if (y + h > height) y = Math.Max(0, height - h);
        return new Rect(x, y, w, h);
    }
}

internal sealed class FixtureCalibrationInfo
{
    public double MmPerPixel { get; set; }
    public double RefLengthMm { get; set; }
    public double RefLengthPx { get; set; }

    /// <summary>标定时拾取的两点在图像里的坐标（像素）。重开程序后用它恢复标定线，便于复查。</summary>
    public int LineAX { get; set; }
    public int LineAY { get; set; }
    public int LineBX { get; set; }
    public int LineBY { get; set; }
    public string CalibratedAt { get; set; } = "";
}
