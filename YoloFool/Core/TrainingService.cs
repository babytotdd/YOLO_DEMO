using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DeploySharp.Engine;

namespace YoloFool;

internal sealed record TrainingOutcome(bool Ok, string Message, string OnnxPath = "", string RunDir = "");

/// <summary>
/// 训练编排：ultralytics 命令行 → best.pt → ONNX → 验证 → 备份 → 切换 mymodel.onnx。
/// 所有输出都通过事件回传给界面，异常不会让程序退出。
/// </summary>
internal sealed class TrainingService
{
    private static readonly Regex EpochRegex = new(@"^\s*(\d+)\s*/\s*(\d+)\s+\S", RegexOptions.Compiled);
    private readonly object _gate = new();
    private Process _proc;
    private bool _cancelled;

    public event Action<string> LogLine;
    public event Action<int> Progress;
    public event Action<string> Stage;

    public bool IsRunning
    {
        get { lock (_gate) return _proc != null && !_proc.HasExited; }
    }

    public void Cancel()
    {
        _cancelled = true;
        try
        {
            lock (_gate)
            {
                if (_proc != null && !_proc.HasExited) _proc.Kill(entireProcessTree: true);
            }
        }
        catch (Exception ex)
        {
            Log.Warn("取消训练失败：" + ex.Message);
        }
    }

    public async Task<TrainingOutcome> TrainAsync(TrainingSetInfo set, int epochs, int imgsz, int batch)
    {
        _cancelled = false;
        if (!File.Exists(AppPaths.YoloExe))
            return new TrainingOutcome(false, $"没有找到训练环境：{AppPaths.YoloExe}\n请先点“初始化训练环境”。");
        if (!File.Exists(AppPaths.BaseWeight))
            return new TrainingOutcome(false, $"没有找到基础权重：{AppPaths.BaseWeight}");
        if (!File.Exists(set.DataYaml))
            return new TrainingOutcome(false, "data.yaml 不存在，请重新生成训练集。");

        var runName = "train_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
        var runDir = Path.Combine(AppPaths.RunsDir, runName);
        Directory.CreateDirectory(runDir);

        Stage?.Invoke("正在训练（CPU）…");
        var args = new List<string>
        {
            "detect", "train",
            $"data={set.DataYaml}",
            $"model={AppPaths.BaseWeight}",
            $"epochs={epochs}",
            $"imgsz={imgsz}",
            $"batch={batch}",
            "device=cpu",
            "workers=0",
            $"project={AppPaths.RunsDir}",
            $"name={runName}",
            "exist_ok=True",
            "patience=20",
            "plots=False",
            "verbose=True",
        };
        var code = await RunAsync(AppPaths.YoloExe, args, runDir);
        if (_cancelled) return new TrainingOutcome(false, "训练已取消。", RunDir: runDir);
        if (code != 0) return new TrainingOutcome(false, $"训练进程退出码 {code}，请查看下方日志。", RunDir: runDir);

        var bestPt = FindBestPt(runDir);
        if (bestPt == null) return new TrainingOutcome(false, "训练结束但没有找到 weights\\best.pt。", RunDir: runDir);

        Stage?.Invoke("正在导出 ONNX…");
        var exportArgs = new List<string>
        {
            // nms=False：YOLO26 导出成端到端 ONNX（[1,300,6]，框已解码、无 NMS 后处理）；
            // 用 yolov8 基础权重训练时该参数会被忽略，仍导出 [1,84,8400]。
            "export", $"model={bestPt}", "format=onnx", $"imgsz={imgsz}", "opset=12", "simplify=False", "nms=False",
        };
        code = await RunAsync(AppPaths.YoloExe, exportArgs, Path.GetDirectoryName(bestPt));
        if (_cancelled) return new TrainingOutcome(false, "训练已取消。", RunDir: runDir);
        if (code != 0) return new TrainingOutcome(false, $"导出 ONNX 失败，退出码 {code}。", RunDir: runDir);

        var onnx = Path.ChangeExtension(bestPt, ".onnx");
        if (!File.Exists(onnx))
            onnx = Directory.GetFiles(Path.GetDirectoryName(bestPt)!, "*.onnx").FirstOrDefault();
        if (onnx == null || !File.Exists(onnx))
            return new TrainingOutcome(false, "导出命令结束但没有找到 ONNX 文件。", RunDir: runDir);

        Stage?.Invoke("正在验证新模型…");
        try
        {
            using var svc = new InferenceService();
            // 以导出文件为准：YOLO26 端到端是 [1,300,6]，必须用 YOLO26 分支验证，否则会静默出一堆假框。
            var kind = OnnxInspector.DetectKind(onnx) ?? YoloModelKind.YOLOv8Det;
            svc.Load(new ModelOptions(onnx, kind, InferenceBackend.OnnxRuntime, DeviceType.CPU,
                OnnxRuntimeDeviceType.Default, 0.25f, 0.5f));
            svc.Warmup();
        }
        catch (Exception ex)
        {
            return new TrainingOutcome(false, "新模型验证失败，未替换当前模型：\n" + ex.Message, onnx, runDir);
        }

        Stage?.Invoke("正在保存并切换模型…");
        try
        {
            var deployed = Deploy(onnx, runName, epochs, set.ClassNames);
            Progress?.Invoke(100);
            return new TrainingOutcome(true, "训练完成，已切换为新模型。", deployed, runDir);
        }
        catch (Exception ex)
        {
            return new TrainingOutcome(false, "保存新模型失败：" + ex.Message, onnx, runDir);
        }
    }

    public async Task<string> InitEnvironmentAsync()
    {
        if (File.Exists(AppPaths.YoloExe)) return "训练环境已就绪。";

        Directory.CreateDirectory(AppPaths.ToolsDir);

        var python = PythonSetup.Find();
        if (python == null)
        {
            Stage?.Invoke("本机没有 Python，正在自动下载安装（首次约 27MB）…");
            var parsed = await PythonSetup.EnsureAsync(s => Stage?.Invoke(s));
            if (!parsed.Ok) return parsed.Message;
            python = parsed.Path;
        }

        Stage?.Invoke("正在创建 Python 虚拟环境…");
        var code = await RunAsync(python, new List<string> { "-m", "venv", AppPaths.VenvDir }, AppPaths.ToolsDir);
        if (code != 0 || !File.Exists(AppPaths.VenvPython))
            return "创建虚拟环境失败：确认 Python 能正常运行（" + python + "）。";

        Stage?.Invoke("正在安装 ultralytics（CPU 版，首次较慢）…");
        code = await RunAsync(AppPaths.VenvPython, new List<string>
        {
            "-m", "pip", "install", "--upgrade", "pip", "-i", "https://pypi.tuna.tsinghua.edu.cn/simple",
        }, AppPaths.ToolsDir);
        code = await RunAsync(AppPaths.VenvPython, new List<string>
        {
            "-m", "pip", "install", "ultralytics", "onnx", "onnxslim",
            "--index-url", "https://pypi.tuna.tsinghua.edu.cn/simple",
        }, AppPaths.ToolsDir);
        if (code != 0 || !File.Exists(AppPaths.YoloExe)) return "安装 ultralytics 失败，请查看日志。";

        if (!File.Exists(AppPaths.BaseWeight))
        {
            Stage?.Invoke("正在下载基础权重 yolov8n.pt…");
            try
            {
                using var http = new System.Net.Http.HttpClient { Timeout = TimeSpan.FromMinutes(5) };
                var bytes = await http.GetByteArrayAsync("https://ghfast.top/https://github.com/ultralytics/assets/releases/download/v8.3.0/yolov8n.pt");
                await File.WriteAllBytesAsync(AppPaths.BaseWeight, bytes);
            }
            catch (Exception ex)
            {
                return "训练环境已装好，但下载 yolov8n.pt 失败：" + ex.Message;
            }
        }
        if (!File.Exists(AppPaths.BaseWeight26))
        {
            // 顺带补一份 YOLO26 基础权重：训练优先用它，导出端到端模型（推理更快）。失败不影响已有环境。
            Stage?.Invoke("正在下载基础权重 yolo26n.pt…");
            try
            {
                using var http = new System.Net.Http.HttpClient { Timeout = TimeSpan.FromMinutes(5) };
                var bytes = await http.GetByteArrayAsync("https://ghfast.top/https://github.com/ultralytics/assets/releases/download/v8.4.0/yolo26n.pt");
                await File.WriteAllBytesAsync(AppPaths.BaseWeight26, bytes);
            }
            catch (Exception ex)
            {
                Log.Warn("下载 yolo26n.pt 失败（继续用 yolov8n.pt）：" + ex.Message);
            }
        }
        return "训练环境已就绪。";
    }

    private async Task<int> RunAsync(string exe, List<string> args, string workDir)
    {
        var psi = new ProcessStartInfo(exe)
        {
            WorkingDirectory = string.IsNullOrWhiteSpace(workDir) ? AppPaths.WorkspaceRoot : workDir,
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            StandardOutputEncoding = Encoding.UTF8,
            StandardErrorEncoding = Encoding.UTF8,
        };
        foreach (var a in args) psi.ArgumentList.Add(a);

        LogLine?.Invoke("> " + exe + " " + string.Join(" ", args));
        var proc = new Process { StartInfo = psi, EnableRaisingEvents = true };
        try
        {
            if (!proc.Start()) return -1;
        }
        catch (Exception ex)
        {
            LogLine?.Invoke("启动失败：" + ex.Message);
            return -1;
        }

        lock (_gate) _proc = proc;
        try
        {
            var stdout = PumpAsync(proc.StandardOutput, false);
            var stderr = PumpAsync(proc.StandardError, true);
            await Task.WhenAll(stdout, stderr, proc.WaitForExitAsync());
            return proc.ExitCode;
        }
        catch (Exception ex)
        {
            LogLine?.Invoke("进程异常：" + ex.Message);
            return -1;
        }
        finally
        {
            lock (_gate) _proc = null;
            proc.Dispose();
        }
    }

    private async Task PumpAsync(StreamReader reader, bool isError)
    {
        string line;
        while ((line = await reader.ReadLineAsync()) != null)
        {
            LogLine?.Invoke(isError ? "[stderr] " + line : line);
            var m = EpochRegex.Match(line);
            if (m.Success && int.TryParse(m.Groups[1].Value, out var cur) && int.TryParse(m.Groups[2].Value, out var total) && total > 0)
                Progress?.Invoke((int)Math.Round(cur * 100.0 / total));
        }
    }

    private static string FindBestPt(string runDir)
    {
        var direct = Path.Combine(runDir, "weights", "best.pt");
        if (File.Exists(direct)) return direct;
        return Directory.Exists(runDir)
            ? Directory.GetFiles(runDir, "best.pt", SearchOption.AllDirectories).FirstOrDefault()
            : null;
    }

    private static string Deploy(string onnx, string runName, int epochs, IReadOnlyList<string> classes)
    {
        Directory.CreateDirectory(AppPaths.ModelsDir);
        Directory.CreateDirectory(AppPaths.ModelBackupDir);
        if (File.Exists(AppPaths.MyModelOnnx))
        {
            var bak = Path.Combine(AppPaths.ModelBackupDir, $"mymodel_{DateTime.Now:yyyyMMdd_HHmmss}.onnx");
            File.Copy(AppPaths.MyModelOnnx, bak, true);
        }
        File.Copy(onnx, AppPaths.MyModelOnnx, true);

        var info = new
        {
            created = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            source = runName,
            epochs,
            classes,
            file = AppPaths.MyModelOnnx,
        };
        File.WriteAllText(AppPaths.ModelInfoFile,
            JsonSerializer.Serialize(info, new JsonSerializerOptions { WriteIndented = true, Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping }),
            new UTF8Encoding(false));
        Log.Info($"新模型已部署：{AppPaths.MyModelOnnx}（备份目录 {AppPaths.ModelBackupDir}）");
        return AppPaths.MyModelOnnx;
    }
}
