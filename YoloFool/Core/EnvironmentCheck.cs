using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OpenCvSharp;

namespace YoloFool;

internal enum EnvCheckState
{
    Ok = 0,
    Warn = 1,
    Fail = 2,
}

internal sealed class EnvCheckItem
{
    public string Name = "";
    public EnvCheckState State;
    public string Detail = "";

    public string Mark => State switch
    {
        EnvCheckState.Ok => "✓",
        EnvCheckState.Warn => "!",
        _ => "✗",
    };
}

/// <summary>“运行环境”卡的体检逻辑：只读文件与依赖，不加载 ONNX 模型、不弹窗。</summary>
internal static class EnvironmentCheck
{
    internal sealed class StructureFix
    {
        public readonly List<string> Created = new();
        public readonly List<string> Failed = new();
    }

    private static readonly (string Label, Func<string> Dir)[] RequiredDirs =
    {
        ("模型目录", () => AppPaths.ModelsDir),
        ("模型备份", () => AppPaths.ModelBackupDir),
        ("数据集", () => AppPaths.DatasetDir),
        ("数据集图片", () => AppPaths.ImagesDir),
        ("数据集标签", () => AppPaths.LabelsDir),
        ("针床检测机种", () => AppPaths.FixturesDir),
        ("定位V2学习库", () => AppPaths.AnchorV2Dir),
        ("误检样本", () => AppPaths.MisdetectDir),
        ("日志", () => AppPaths.LogDir),
    };

    public static List<EnvCheckItem> Run(bool includeCamera)
    {
        var items = new List<EnvCheckItem>();

        // 1. 工作区可读写
        items.Add(CheckWritable());

        // 2. 目录结构
        var missing = RequiredDirs.Where(d => !Directory.Exists(d.Dir())).Select(d => d.Label).ToList();
        items.Add(missing.Count == 0
            ? Ok("目录结构", $"{RequiredDirs.Length} 个目录都在")
            : new EnvCheckItem
            {
                Name = "目录结构",
                State = EnvCheckState.Warn,
                Detail = "缺 " + string.Join("、", missing) + "（可点“运行环境初始化”补齐）",
            });

        // 3. 模型文件
        items.Add(CheckModel());

        // 4. 推理运行库
        items.Add(CheckRuntime());

        // 5. 训练环境（含 Python 解释器）
        items.Add(CheckTraining());
        items.Add(CheckPython());

        // 6. 磁盘空间
        items.Add(CheckDisk());

        // 7. 相机
        if (includeCamera) items.Add(CheckCamera());

        return items;
    }

    private static EnvCheckItem Ok(string name, string detail) =>
        new() { Name = name, State = EnvCheckState.Ok, Detail = detail };

    private static EnvCheckItem CheckWritable()
    {
        var root = AppPaths.WorkspaceRoot;
        var probe = Path.Combine(root, ".env_probe_" + Guid.NewGuid().ToString("N") + ".tmp");
        try
        {
            Directory.CreateDirectory(root);
            File.WriteAllText(probe, "ok");
            File.Delete(probe);
            return Ok("工作区目录", "可读写 · " + root);
        }
        catch (Exception ex)
        {
            return new EnvCheckItem { Name = "工作区目录", State = EnvCheckState.Fail, Detail = root + " · " + ex.Message };
        }
    }

    private static EnvCheckItem CheckModel()
    {
        var path = AppPaths.MyModelOnnx;
        if (!File.Exists(path))
            return new EnvCheckItem
            {
                Name = "识别模型",
                State = EnvCheckState.Warn,
                Detail = "还没有 my_models/mymodel.onnx，可先加载预置示例模型",
            };

        var fi = new FileInfo(path);
        var kind = "";
        try { kind = OnnxInspector.Describe(OnnxInspector.DetectKind(path)); } catch { }
        return Ok("识别模型", $"{fi.Name} · {ToolsLibrary.FormatSize(fi.Length)} · {fi.LastWriteTime:MM-dd HH:mm}"
                              + (string.IsNullOrEmpty(kind) ? "" : " · " + kind));
    }

    private static EnvCheckItem CheckRuntime()
    {
        try
        {
            using var mat = new Mat(8, 8, MatType.CV_8UC3, Scalar.All(0));
            var opencv = typeof(Mat).Assembly.GetName().Version?.ToString() ?? "?";
            var onnx = typeof(Microsoft.ML.OnnxRuntime.InferenceSession).Assembly.GetName().Version?.ToString() ?? "?";
            return Ok("推理运行库", $"OpenCV {opencv} · ONNX Runtime {onnx} 都能加载");
        }
        catch (Exception ex)
        {
            return new EnvCheckItem { Name = "推理运行库", State = EnvCheckState.Fail, Detail = ex.Message };
        }
    }

    private static EnvCheckItem CheckTraining()
    {
        if (File.Exists(AppPaths.YoloExe))
            return Ok("训练环境", "ultralytics 已就绪 · " + AppPaths.YoloExe);
        if (Directory.Exists(AppPaths.VenvDir))
            return new EnvCheckItem
            {
                Name = "训练环境",
                State = EnvCheckState.Warn,
                Detail = "venv 在，但缺 Scripts/yolo.exe（可点初始化补装）",
            };
        return new EnvCheckItem
        {
            Name = "训练环境",
            State = EnvCheckState.Warn,
            Detail = "未初始化（训练/自动标注前需要，可点“运行环境初始化”）",
        };
    }

    private static EnvCheckItem CheckPython()
    {
        var path = PythonSetup.Find(out var version);
        if (path != null)
            return Ok("Python（训练用）", version / 100 + "." + version % 100 + " · " + path);

        return new EnvCheckItem
        {
            Name = "Python（训练用）",
            State = EnvCheckState.Warn,
            Detail = "没装 Python 3.10+；勾选“顺带装训练环境”后一键初始化会自动下载安装",
        };
    }

    private static EnvCheckItem CheckDisk()
    {
        try
        {
            var root = Path.GetPathRoot(AppPaths.WorkspaceRoot) ?? "C:\\";
            var drive = new DriveInfo(root);
            var free = drive.AvailableFreeSpace;
            var state = free < 2L * 1024 * 1024 * 1024 ? EnvCheckState.Warn : EnvCheckState.Ok;
            return new EnvCheckItem
            {
                Name = "磁盘空间",
                State = state,
                Detail = $"{drive.Name.TrimEnd('\\')} 剩余 {ToolsLibrary.FormatSize(free)}",
            };
        }
        catch (Exception ex)
        {
            return new EnvCheckItem { Name = "磁盘空间", State = EnvCheckState.Warn, Detail = ex.Message };
        }
    }

    private static EnvCheckItem CheckCamera()
    {
        var found = new List<int>();
        for (var i = 0; i < 4; i++)
        {
            try
            {
                using var cap = new VideoCapture(i, VideoCaptureAPIs.DSHOW);
                if (cap.IsOpened() && cap.Read(new Mat()) is var ok && ok)
                    found.Add(i);
            }
            catch
            {
                // 打不开就当没有
            }
        }

        return found.Count > 0
            ? Ok("相机", "找到 " + string.Join(" / ", found.Select(i => "#" + i)))
            : new EnvCheckItem { Name = "相机", State = EnvCheckState.Warn, Detail = "没探测到相机（不接相机也能看照片）" };
    }

    /// <summary>补齐缺失目录，不动任何已有文件。</summary>
    public static StructureFix EnsureStructure()
    {
        var fix = new StructureFix();
        foreach (var (label, dir) in RequiredDirs)
        {
            var path = dir();
            if (Directory.Exists(path)) continue;
            try
            {
                Directory.CreateDirectory(path);
                fix.Created.Add(label + "：" + path);
            }
            catch (Exception ex)
            {
                fix.Failed.Add(label + "：" + ex.Message);
            }
        }

        try { AppPaths.EnsureDirectories(); } catch { }
        return fix;
    }
}
