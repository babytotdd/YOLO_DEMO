using System.Text.Json;
using DeploySharp.Engine;

namespace YoloFool;

internal sealed class AppSettings
{
    public string ModelPath { get; set; } = "";
    public string ModelKind { get; set; } = "YOLOv8Det";
    public string Engine { get; set; } = nameof(InferenceBackend.OnnxRuntime);
    public string Device { get; set; } = nameof(DeviceType.CPU);
    public string OnnxProvider { get; set; } = "Default";
    public double Confidence { get; set; } = 0.5;
    public double Nms { get; set; } = 0.5;
    public string LastImagePath { get; set; } = "";

    public static AppSettings Load()
    {
        AppSettings s = null;
        try
        {
            if (File.Exists(AppPaths.SettingsFile))
                s = JsonSerializer.Deserialize<AppSettings>(File.ReadAllText(AppPaths.SettingsFile));
        }
        catch { }
        if (s == null) s = new AppSettings();
        if (string.IsNullOrEmpty(s.ModelPath) || !File.Exists(s.ModelPath))
        {
            // 本机验证过的默认：有"我的模型"优先，否则 COCO yolov8n
            s.ModelPath = File.Exists(AppPaths.MyModelOnnx) ? AppPaths.MyModelOnnx : AppPaths.DefaultCocoModel;
            s.ModelKind = "YOLOv8Det";
        }
        return s;
    }

    public void Save() =>
        File.WriteAllText(AppPaths.SettingsFile, JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true }));
}
