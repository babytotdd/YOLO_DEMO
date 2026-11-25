using System;
using System.IO;
using System.Linq;

namespace YoloFool;

/// <summary>
/// 统一管理程序用到的所有路径。
/// 旧版本把 H:\Desktop\YOLO_DEMO 写死在代码里，换一台电脑就全部失效；
/// 这里改为从程序目录向上寻找“工作区”（含 demo / mydataset 的目录），
/// 找不到时再回退到 H:\Desktop\YOLO_DEMO，仍找不到就用程序目录。
/// </summary>
internal static class AppPaths
{
    public static string Root { get; } = AppContext.BaseDirectory.TrimEnd(Path.DirectorySeparatorChar);
    public static string WorkspaceRoot { get; } = ResolveWorkspaceRoot();

    public static string SettingsFile => Path.Combine(Root, "settings.json");

    public static string ModelsDir => Path.Combine(WorkspaceRoot, "my_models");
    public static string MyModelOnnx => Path.Combine(ModelsDir, "mymodel.onnx");
    public static string ModelInfoFile => Path.Combine(ModelsDir, "model_info.json");
    public static string ModelBackupDir => Path.Combine(ModelsDir, "backup");

    public static string DatasetDir => Path.Combine(WorkspaceRoot, "mydataset");
    public static string ImagesDir => Path.Combine(DatasetDir, "images");
    public static string LabelsDir => Path.Combine(DatasetDir, "labels");
    public static string ClassesFile => Path.Combine(DatasetDir, "classes.txt");
    public static string AnnotationsFile => Path.Combine(DatasetDir, "annotations.json");
    public static string LabelsMapFile => Path.Combine(DatasetDir, "labels_map.json");
    public static string ExportMapFile => Path.Combine(DatasetDir, "export_map.json");
    public static string DataYamlFile => Path.Combine(DatasetDir, "data.yaml");
    public static string TrainingDir => Path.Combine(DatasetDir, "training");

    /// <summary>删除标签等破坏性操作前的自动备份目录（annotations.json / classes.txt）。</summary>
    public static string DatasetBackupDir => Path.Combine(DatasetDir, "backup");
    public static string RunsDir => Path.Combine(DatasetDir, "runs");

    public static string ToolsDir => Path.Combine(WorkspaceRoot, "tools");
    public static string VenvDir => Path.Combine(ToolsDir, "yolovenv");
    public static string VenvPython => Path.Combine(VenvDir, "Scripts", "python.exe");
    public static string YoloExe => Path.Combine(VenvDir, "Scripts", "yolo.exe");
    /// <summary>YOLO26 基础权重：存在时优先用于训练，导出的 ONNX 才是端到端（[1,300,6]），推理更快。</summary>
    public static string BaseWeight26 => Path.Combine(ToolsDir, "yolo26n.pt");

    public static string BaseWeight => File.Exists(BaseWeight26) ? BaseWeight26 : Path.Combine(ToolsDir, "yolov8n.pt");

    /// <summary>NuGet 包自带的 COCO 预训练模型（只做“开箱即用”的默认模型）。</summary>
    public static string DefaultCocoModel
    {
        get
        {
            foreach (var p in new[]
                     {
                         Path.Combine(WorkspaceRoot, "demo", "models", "yolov8n.onnx"),
                         Path.Combine(WorkspaceRoot, "assets", "yolov8n.onnx"),
                     })
            {
                if (File.Exists(p)) return p;
            }
            var dir = Path.Combine(WorkspaceRoot, "demo", "models");
            return Directory.Exists(dir)
                ? Directory.EnumerateFiles(dir, "*.onnx").OrderBy(x => x).FirstOrDefault() ?? ""
                : "";
        }
    }

    /// <summary>分割模型（视频分析默认用它，同时提供框和掩码）。</summary>
    public static string DefaultSegModel
    {
        get
        {
            foreach (var p in new[]
                     {
                         Path.Combine(WorkspaceRoot, "demo", "models", "yolov8n-seg.onnx"),
                         Path.Combine(WorkspaceRoot, "assets", "yolov8n-seg.onnx"),
                     })
            {
                if (File.Exists(p)) return p;
            }
            return "";
        }
    }

    public static string VideoSamplesDir => Path.Combine(WorkspaceRoot, "samples", "video");

    /// <summary>拍照与录像的落盘目录。</summary>
    public static string CaptureDir => Path.Combine(WorkspaceRoot, "captures");

    /// <summary>针床检测的配方与模板图（fixtures/&lt;机种&gt;/）。</summary>
    public static string FixturesDir => Path.Combine(WorkspaceRoot, "fixtures");

    /// <summary>针床定位 V2 的模型、基准与容差（fixtures_v2/机种/）。</summary>
    public static string AnchorV2Dir => Path.Combine(WorkspaceRoot, "fixtures_v2");

    /// <summary>手工删除的误检样本（图片副本 + misdetect.jsonl 记录），供后续难例回收。</summary>
    public static string MisdetectDir => Path.Combine(ModelsDir, "misdetect");

    public static string DefaultVideo
    {
        get
        {
            foreach (var name in new[] { "people-detection.mp4", "car-detection.mp4", "store-aisle-detection.mp4" })
            {
                var p = Path.Combine(VideoSamplesDir, name);
                if (File.Exists(p)) return p;
            }
            var dir = VideoSamplesDir;
            return Directory.Exists(dir)
                ? Directory.EnumerateFiles(dir, "*.mp4").OrderBy(x => x).FirstOrDefault() ?? ""
                : "";
        }
    }

    public static string SampleImage
    {
        get
        {
            foreach (var p in new[]
                     {
                         Path.Combine(WorkspaceRoot, "assets", "bus.jpg"),
                         Path.Combine(WorkspaceRoot, "demo", "images", "bus.jpg"),
                         Path.Combine(WorkspaceRoot, "demo", "images", "plane.png"),
                     })
            {
                if (File.Exists(p)) return p;
            }
            var dir = Path.Combine(WorkspaceRoot, "demo", "images");
            return Directory.Exists(dir)
                ? Directory.EnumerateFiles(dir, "*.*").FirstOrDefault(x =>
                    x.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                    x.EndsWith(".png", StringComparison.OrdinalIgnoreCase)) ?? ""
                : "";
        }
    }

    public static void EnsureDirectories()
    {
        try
        {
            Directory.CreateDirectory(DatasetDir);
            Directory.CreateDirectory(ImagesDir);
            Directory.CreateDirectory(LabelsDir);
            Directory.CreateDirectory(ModelsDir);
            Directory.CreateDirectory(DatasetBackupDir);
            Directory.CreateDirectory(FixturesDir);
            Directory.CreateDirectory(LogDir);
        }
        catch (Exception ex)
        {
            Log.Error("创建目录失败", ex);
        }
    }

    public static string LogDir => Path.Combine(Root, "logs");

    private static string ResolveWorkspaceRoot()
    {
        var env = Environment.GetEnvironmentVariable("YOLO_DEMO_HOME");
        if (!string.IsNullOrWhiteSpace(env) && Directory.Exists(env)) return Path.GetFullPath(env);

        var dir = new DirectoryInfo(Root);
        for (int i = 0; i < 8 && dir != null; i++, dir = dir.Parent)
        {
            if (Directory.Exists(Path.Combine(dir.FullName, "demo")) ||
                Directory.Exists(Path.Combine(dir.FullName, "mydataset")))
                return dir.FullName;
        }

        const string fallback = @"H:\Desktop\YOLO_DEMO";
        return Directory.Exists(fallback) ? fallback : Root;
    }
}
