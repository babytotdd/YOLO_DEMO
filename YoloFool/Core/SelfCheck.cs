using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using DeploySharp.Engine;
using OpenCvSharp;

namespace YoloFool;

internal sealed record ProviderProbe(OnnxRuntimeDeviceType Provider, bool Ok, string Detail, long Milliseconds);

internal sealed class SelfCheckResult
{
    public bool Ok { get; init; }
    public ModelOptions Options { get; init; }
    public InferenceService Service { get; init; }
    public ModelRuntimeInfo Info { get; init; }
    public long InferenceMs { get; init; }
    public string Message { get; init; } = "";
    public string Detail { get; init; } = "";
    public IReadOnlyList<ProviderProbe> Probes { get; init; } = Array.Empty<ProviderProbe>();
}

/// <summary>
/// 本机自检：选模型 → 加载 → 样例推理 → 计时；失败自动回退 CPU/Default。
/// 傻瓜版默认值必须来自这里，而不是写死的猜测。
/// </summary>
internal static class SelfCheck
{
    public static string ResolveModelPath(AppSettings s)
    {
        if (!string.IsNullOrWhiteSpace(s.ModelPath) && File.Exists(s.ModelPath)) return s.ModelPath;
        if (File.Exists(AppPaths.MyModelOnnx)) return AppPaths.MyModelOnnx;
        if (File.Exists(AppPaths.DefaultCocoModel)) return AppPaths.DefaultCocoModel;
        if (Directory.Exists(AppPaths.ModelsDir))
        {
            var any = Directory.GetFiles(AppPaths.ModelsDir, "*.onnx", SearchOption.TopDirectoryOnly)
                .OrderBy(p => p.Contains("yolov8n") ? 0 : 1).ThenBy(p => p).FirstOrDefault();
            if (!string.IsNullOrEmpty(any)) return any;
        }
        return "";
    }

    public static SelfCheckResult Run(AppSettings settings, Action<string> progress = null)
    {
        var path = ResolveModelPath(settings);
        if (path.Length == 0)
        {
            return new SelfCheckResult
            {
                Ok = false,
                Message = "没有找到可用的 ONNX 模型。",
                Detail = $"请把模型放到 {AppPaths.ModelsDir}，或准备内置模型 {AppPaths.DefaultCocoModel}。",
            };
        }

        var requested = settings.ToModelOptions() with
        {
            ModelPath = path,
            Kind = IsBuiltinCoco(path) ? YoloModelKind.YOLOv8Det : ModelOptions.ParseKind(settings.ModelKind),
        };

        var candidates = new List<ModelOptions> { requested };
        if (requested.Provider != OnnxRuntimeDeviceType.Default)
            candidates.Add(requested.With(provider: OnnxRuntimeDeviceType.Default, device: DeviceType.CPU));
        else if (requested.Device != DeviceType.CPU)
            candidates.Add(requested.With(device: DeviceType.CPU));

        var errors = new List<string>();
        foreach (var candidate in candidates)
        {
            progress?.Invoke($"正在验证 {candidate.ProviderText} 推理…");
            InferenceService svc = null;
            try
            {
                svc = new InferenceService();
                var info = svc.Load(candidate);
                svc.Warmup();
                var ms = Measure(svc);
                var message = $"{candidate.ProviderText} · {ms} ms/张 · {info.ClassCount} 类";
                var detail = candidate.Provider == requested.Provider
                    ? ""
                    : $"首选 {requested.ProviderText} 不可用，已自动回退到 {candidate.ProviderText}。";
                Log.Info($"自检通过：{candidate.ModelPath} / {candidate.Kind} / {message}");
                return new SelfCheckResult
                {
                    Ok = true,
                    Options = candidate,
                    Service = svc,
                    Info = info,
                    InferenceMs = ms,
                    Message = message,
                    Detail = detail,
                };
            }
            catch (Exception ex)
            {
                svc?.Dispose();
                var line = $"{candidate.ProviderText}：{ex.Message}";
                errors.Add(line);
                Log.Error($"自检失败（{candidate.ProviderText}）", ex);
            }
        }

        var aggregate = string.Join(Environment.NewLine, errors);
        return new SelfCheckResult
        {
            Ok = false,
            Options = requested,
            Message = "模型加载失败，已保留上一次可用设置。",
            Detail = aggregate,
        };
    }

    /// <summary>后台探测各执行后端，用于“详细设置”里的能力提示。</summary>
    public static IReadOnlyList<ProviderProbe> ProbeProviders(string modelPath, YoloModelKind kind)
    {
        var list = new List<ProviderProbe>();
        foreach (var provider in new[] { OnnxRuntimeDeviceType.Default, OnnxRuntimeDeviceType.DML })
        {
            var sw = Stopwatch.StartNew();
            try
            {
                using var svc = new InferenceService();
                svc.Load(new ModelOptions(modelPath, kind, InferenceBackend.OnnxRuntime, DeviceType.CPU, provider, 0.25f, 0.5f));
                svc.Warmup();
                sw.Stop();
                list.Add(new ProviderProbe(provider, true, $"{sw.ElapsedMilliseconds} ms", sw.ElapsedMilliseconds));
            }
            catch (Exception ex)
            {
                sw.Stop();
                list.Add(new ProviderProbe(provider, false, ex.Message, sw.ElapsedMilliseconds));
            }
        }
        return list;
    }

    private static long Measure(InferenceService svc)
    {
        if (File.Exists(AppPaths.SampleImage))
        {
            var times = new List<long>();
            for (int i = 0; i < 3; i++) times.Add(svc.Predict(AppPaths.SampleImage).ElapsedMilliseconds);
            times.Sort();
            return times[times.Count / 2];
        }
        using var mat = new Mat(640, 640, MatType.CV_8UC3, Scalar.All(114));
        return svc.Predict(mat).ElapsedMilliseconds;
    }

    private static bool IsBuiltinCoco(string path) =>
        string.Equals(Path.GetFullPath(path), Path.GetFullPath(AppPaths.DefaultCocoModel), StringComparison.OrdinalIgnoreCase);
}
