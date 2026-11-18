using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.ML.OnnxRuntime;

namespace YoloFool;

/// <summary>
/// 直接读 ONNX 文件判断它是检测模型还是分割模型。
/// 背景：训练出来的 YOLOv8 检测模型如果被当成"分割"加载，第一次推理会在
/// DeploySharp 的 seg 后处理里拿不到掩码输出，抛 IndexOutOfRangeException
/// （现场错误："视频分析失败：Index 1 out of range"）。所以以文件为准，加载前纠正。
/// </summary>
internal static class OnnxInspector
{
    private static readonly object Gate = new();
    private static readonly Dictionary<string, (DateTime Write, YoloModelKind? Kind)> Cache =
        new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// 返回模型真实类型；无法判断（非 YOLO、读不了文件、类型不认识）时返回 null，
    /// 调用方应沿用设置里的值。
    /// </summary>
    public static YoloModelKind? DetectKind(string path)
    {
        if (string.IsNullOrWhiteSpace(path) || !File.Exists(path)) return null;

        DateTime write;
        try { write = File.GetLastWriteTimeUtc(path); }
        catch { write = DateTime.MinValue; }

        lock (Gate)
        {
            if (Cache.TryGetValue(path, out var hit) && hit.Write == write) return hit.Kind;
        }

        var kind = Probe(path);
        lock (Gate) Cache[path] = (write, kind);
        return kind;
    }

    /// <summary>把探测结果翻译成给人看的中文，用于提示/日志。</summary>
    public static string Describe(YoloModelKind? kind) => kind switch
    {
        YoloModelKind.YOLOv8Seg => "分割",
        YoloModelKind.YOLOv8Det => "检测",
        YoloModelKind.YOLOv11Det => "检测",
        _ => "未知",
    };

    private static YoloModelKind? Probe(string path)
    {
        try
        {
            using var session = new InferenceSession(path);

            // Ultralytics 导出的 ONNX 会带 metadata：task=detect / segment
            var task = ReadTask(session);
            if (!string.IsNullOrEmpty(task))
            {
                switch (task.Trim().ToLowerInvariant())
                {
                    case "detect":
                    case "det":
                        return YoloModelKind.YOLOv8Det;
                    case "segment":
                    case "seg":
                        return YoloModelKind.YOLOv8Seg;
                    default:
                        Log.Warn($"模型的 ONNX 任务类型是 {task}，暂不支持，按设置加载：{path}");
                        return null;
                }
            }

            return ProbeByOutputs(session);
        }
        catch (Exception ex)
        {
            Log.Warn("读取模型信息失败（按设置加载）：" + ex.Message);
            return null;
        }
    }

    private static string ReadTask(InferenceSession session)
    {
        try
        {
            var map = session.ModelMetadata?.CustomMetadataMap;
            if (map == null) return null;
            foreach (var kv in map)
                if (string.Equals(kv.Key, "task", StringComparison.OrdinalIgnoreCase))
                    return kv.Value;
        }
        catch
        {
            // 老模型/非 Ultralytics 导出没有 metadata，走输出形状推断
        }
        return null;
    }

    /// <summary>
    /// 没有 metadata 时的退路：分割模型多一路 [1,32,H,W] 的掩码原型，
    /// 检测模型只有一路 [1,4+nc,8400]。
    /// </summary>
    private static YoloModelKind? ProbeByOutputs(InferenceSession session)
    {
        var outputs = session.OutputMetadata;
        if (outputs == null || outputs.Count == 0) return null;

        bool sawMaskPrototype = false;
        bool sawBoxOutput = false;
        foreach (var md in outputs.Values)
        {
            var dims = md?.Dimensions;
            if (dims == null) continue;
            if (dims.Length == 4)
            {
                int ch = dims[1];
                if (ch == 32 || ch == 64) sawMaskPrototype = true;
            }
            else if (dims.Length == 3)
            {
                int ch = dims[1];
                if (ch >= 5) sawBoxOutput = true;
            }
        }

        if (sawMaskPrototype) return YoloModelKind.YOLOv8Seg;
        if (sawBoxOutput) return YoloModelKind.YOLOv8Det;
        return null;
    }
}
