using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DeploySharp.Data;
using DeploySharp.Engine;

namespace YoloFool;

internal enum YoloModelKind
{
    YOLOv8Det,
    YOLOv11Det,
    YOLOv8Seg,
    YOLOv26Det,
}

/// <summary>一次推理的全部配置。修改设置时必须先构造它并通过验证，不能直接改运行中的模型。</summary>
internal sealed record ModelOptions(
    string ModelPath,
    YoloModelKind Kind,
    InferenceBackend Engine,
    DeviceType Device,
    OnnxRuntimeDeviceType Provider,
    float Confidence,
    float Nms)
{
    public static IReadOnlyList<YoloModelKind> AllKinds { get; } =
        new[] { YoloModelKind.YOLOv8Det, YoloModelKind.YOLOv11Det, YoloModelKind.YOLOv8Seg, YoloModelKind.YOLOv26Det };

    public static IReadOnlyList<DeviceType> AllDevices { get; } =
        new[] { DeviceType.CPU, DeviceType.GPU0 };

    public static IReadOnlyList<OnnxRuntimeDeviceType> AllProviders { get; } =
        new[] { OnnxRuntimeDeviceType.Default, OnnxRuntimeDeviceType.DML };

    public static YoloModelKind ParseKind(string s) =>
        Enum.TryParse<YoloModelKind>(s, true, out var k) ? k : YoloModelKind.YOLOv8Det;

    public static DeviceType ParseDevice(string s) =>
        Enum.TryParse<DeviceType>(s, true, out var d) ? d : DeviceType.CPU;

    public static OnnxRuntimeDeviceType ParseProvider(string s) =>
        Enum.TryParse<OnnxRuntimeDeviceType>(s, true, out var p) ? p : OnnxRuntimeDeviceType.Default;

    public string ProviderText => Provider == OnnxRuntimeDeviceType.Default ? "CPU" : Provider.ToString();

    public string KindText => Kind switch
    {
        YoloModelKind.YOLOv8Det => "YOLOv8 检测",
        YoloModelKind.YOLOv11Det => "YOLOv11 检测",
        YoloModelKind.YOLOv8Seg => "YOLOv8 分割",
        YoloModelKind.YOLOv26Det => "YOLO26 检测（端到端）",
        _ => Kind.ToString(),
    };

    public ModelOptions With(string path = null, YoloModelKind? kind = null, DeviceType? device = null,
        OnnxRuntimeDeviceType? provider = null, float? conf = null, float? nms = null) =>
        new(path ?? ModelPath,
            kind ?? Kind,
            Engine,
            device ?? Device,
            provider ?? Provider,
            conf ?? Confidence,
            nms ?? Nms);
}

/// <summary>归一化后的检测结果（坐标是原图像素）。</summary>
internal sealed record DetectionItem(
    int ClassId,
    string RawCategory,
    float Confidence,
    int X,
    int Y,
    int W,
    int H,
    int ImageWidth,
    int ImageHeight)
{
    public double CenterXPercent => ImageWidth <= 0 ? 0 : (X + W / 2.0) / ImageWidth * 100.0;
    public double CenterYPercent => ImageHeight <= 0 ? 0 : (Y + H / 2.0) / ImageHeight * 100.0;
    public string BoundsText => $"{X},{Y},{W},{H}";
}

internal sealed record ModelRuntimeInfo(
    string ModelPath,
    YoloModelKind Kind,
    OnnxRuntimeDeviceType Provider,
    DeviceType Device,
    IReadOnlyList<string> ClassNames,
    long LoadMilliseconds)
{
    public int ClassCount => ClassNames?.Count ?? 0;
    public string FileName => Path.GetFileName(ModelPath);
}

internal sealed record PredictionResult(
    IReadOnlyList<DetectionItem> Items,
    long ElapsedMilliseconds,
    int ImageWidth,
    int ImageHeight,
    IReadOnlyList<SegMaskInfo> Masks = null)
{
    public IReadOnlyList<SegMaskInfo> MaskList => Masks ?? Array.Empty<SegMaskInfo>();
}
