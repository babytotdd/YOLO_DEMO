using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using DeploySharp.Engine;

namespace YoloFool;

/// <summary>
/// 用户设置。傻瓜版的原则：
/// 1) 首次启动由 SelfCheck 选出“本机验证通过”的默认值并写回；
/// 2) 详细设置里修改后，必须先验证成功才允许保存；
/// 3) 保存失败/模型损坏时，沿用上一次可用配置。
/// </summary>
internal sealed class AppSettings
{
    public int SchemaVersion { get; set; } = 2;

    public string ModelPath { get; set; } = "";
    public string ModelKind { get; set; } = nameof(YoloModelKind.YOLOv8Det);
    public string Engine { get; set; } = nameof(InferenceBackend.OnnxRuntime);
    public string Device { get; set; } = nameof(DeviceType.CPU);
    public string OnnxProvider { get; set; } = nameof(OnnxRuntimeDeviceType.Default);

    public double Confidence { get; set; } = 0.5;
    public double Nms { get; set; } = 0.5;

    public string LastImagePath { get; set; } = "";
    public string LastLearnImagePath { get; set; } = "";

    public string LastVideoPath { get; set; } = "";
    public string VideoModelPath { get; set; } = "";
    public string VideoDetModelPath { get; set; } = "";

    /// <summary>视频页任务类型：seg=分割（面积统计），det=检测（只数目标）。</summary>
    public string VideoTask { get; set; } = "seg";
    public double VideoConfidence { get; set; } = 0.35;
    public bool VideoOnlyBest { get; set; }
    public string VideoClassFilter { get; set; } = "";

    // 结果筛除（P2）：面积/长宽比/同类去重/视频连续帧确认/类别屏蔽
    public bool FilterEnabled { get; set; }
    public double FilterMinArea { get; set; }
    public double FilterMaxArea { get; set; } = 100;
    public double FilterMinAspect { get; set; }
    public double FilterMaxAspect { get; set; }
    public double FilterDedupIou { get; set; } = 0.6;
    public int FilterConfirmFrames { get; set; } = 3;
    public List<string> MutedClasses { get; set; } = new();

    public bool VideoLoop { get; set; }
    public int VideoFrameStep { get; set; } = 1;

    public bool AdvancedExpanded { get; set; }

    public int TrainEpochs { get; set; } = 50;
    public int TrainImageSize { get; set; } = 640;
    public int TrainBatch { get; set; } = 4;

    public string SelfCheckAt { get; set; } = "";
    public string SelfCheckText { get; set; } = "";

    [JsonIgnore]
    public string FilePath => AppPaths.SettingsFile;

    public static AppSettings Load()
    {
        AppSettings s = null;
        try
        {
            if (File.Exists(AppPaths.SettingsFile))
                s = JsonSerializer.Deserialize<AppSettings>(File.ReadAllText(AppPaths.SettingsFile));
        }
        catch (Exception ex)
        {
            Log.Error("读取 settings.json 失败，使用内置默认值", ex);
        }

        s ??= new AppSettings();
        s.Normalize();
        return s;
    }

    public void Normalize()
    {
        if (string.IsNullOrWhiteSpace(Engine)) Engine = nameof(InferenceBackend.OnnxRuntime);
        if (string.IsNullOrWhiteSpace(ModelKind)) ModelKind = nameof(YoloModelKind.YOLOv8Det);
        if (string.IsNullOrWhiteSpace(Device)) Device = nameof(DeviceType.CPU);
        if (string.IsNullOrWhiteSpace(OnnxProvider)) OnnxProvider = nameof(OnnxRuntimeDeviceType.Default);

        Confidence = Math.Clamp(Confidence, 0.01, 0.99);
        Nms = Math.Clamp(Nms, 0.01, 0.99);

        VideoFrameStep = Math.Clamp(VideoFrameStep, 1, 12);
        VideoTask = string.Equals(VideoTask, "det", StringComparison.OrdinalIgnoreCase) ? "det" : "seg";
        VideoConfidence = Math.Clamp(VideoConfidence, 0.01, 0.99);
        VideoClassFilter = (VideoClassFilter ?? "").Trim();
        FilterMinArea = Math.Clamp(FilterMinArea, 0, 100);
        FilterMaxArea = Math.Clamp(FilterMaxArea <= 0 ? 100 : FilterMaxArea, 0, 100);
        if (FilterMaxArea < FilterMinArea) (FilterMinArea, FilterMaxArea) = (FilterMaxArea, FilterMinArea);
        FilterMinAspect = Math.Clamp(FilterMinAspect, 0, 20);
        FilterMaxAspect = Math.Clamp(FilterMaxAspect, 0, 20);
        FilterDedupIou = Math.Clamp(FilterDedupIou, 0, 0.95);
        FilterConfirmFrames = Math.Clamp(FilterConfirmFrames, 1, 30);
        MutedClasses ??= new List<string>();
        MutedClasses = MutedClasses.Where(m => !string.IsNullOrWhiteSpace(m)).Select(m => m.Trim()).Distinct(StringComparer.OrdinalIgnoreCase).ToList();
        TrainEpochs = Math.Clamp(TrainEpochs, 1, 500);
        TrainBatch = Math.Clamp(TrainBatch, 1, 64);
        if (TrainImageSize < 320 || TrainImageSize > 1280) TrainImageSize = 640;
    }

    public ModelOptions ToModelOptions() => new(
        ModelPath,
        ModelOptions.ParseKind(ModelKind),
        InferenceBackend.OnnxRuntime,
        ModelOptions.ParseDevice(Device),
        ModelOptions.ParseProvider(OnnxProvider),
        (float)Confidence,
        (float)Nms);

    public void Apply(ModelOptions o)
    {
        ModelPath = o.ModelPath;
        ModelKind = o.Kind.ToString();
        Engine = nameof(InferenceBackend.OnnxRuntime);
        Device = o.Device.ToString();
        OnnxProvider = o.Provider.ToString();
        Confidence = Math.Round(o.Confidence, 2);
        Nms = Math.Round(o.Nms, 2);
    }

    public void Save()
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(AppPaths.SettingsFile)!);
            var json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
            var tmp = AppPaths.SettingsFile + ".tmp";
            File.WriteAllText(tmp, json);
            File.Move(tmp, AppPaths.SettingsFile, true);
        }
        catch (Exception ex)
        {
            Log.Error("保存 settings.json 失败", ex);
        }
    }
}
