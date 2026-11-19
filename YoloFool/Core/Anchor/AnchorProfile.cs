using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using OpenCvSharp;

namespace YoloFool;

/// <summary>
/// 针床定位 V2 的配置：模型、相机、基准锚点、容差。
/// 与旧“针床检测”的 recipe.json 完全独立，落在 fixtures_v2/&lt;机种&gt; 下。
/// </summary>
internal sealed class AnchorProfile
{
    public int Schema { get; set; } = 2;

    /// <summary>机种标识（= 目录名）。</summary>
    public string Model { get; set; } = "";

    public string UpdatedAt { get; set; } = "";

    public FixtureCameraConfig Camera { get; set; } = new() { Width = 1920, Height = 1080 };

    public AnchorInferConfig Infer { get; set; } = new();

    public AnchorCalib Calib { get; set; } = new();

    public AnchorTolerance Tolerance { get; set; } = new();

    /// <summary>检测范围：页内框出来的小区域 + 标签名 + 匹配率下限。</summary>
    public List<AnchorZone> Zones { get; set; } = new();

    /// <summary>只用于把像素换算成 mm 显示；判定仍以像素为准。</summary>
    public double MmPerPixel { get; set; }

    public bool IsCalibrated =>
        Calib != null && !string.IsNullOrWhiteSpace(Calib.At) &&
        Calib.Anchors != null && Calib.Anchors.Count > 0;

    public bool HasZones => Zones != null && Zones.Count > 0;

    public static AnchorProfile CreateDefault(string model) => new()
    {
        Model = model ?? "",
        UpdatedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
    };

    /// <summary>反序列化后补齐可能缺失的嵌套对象，避免 UI 到处判空。</summary>
    public void Normalize()
    {
        Camera ??= new FixtureCameraConfig { Width = 1920, Height = 1080 };
        Infer ??= new AnchorInferConfig();
        Calib ??= new AnchorCalib();
        Calib.Anchors ??= new List<AnchorItem>();
        Tolerance ??= new AnchorTolerance();
        Zones ??= new List<AnchorZone>();
        Zones.RemoveAll(z => z == null);
        foreach (var zone in Zones) zone.Normalize();
        if (Tolerance.MinAnchors < 1) Tolerance.MinAnchors = 1;
        if (Tolerance.ShiftPx <= 0) Tolerance.ShiftPx = 12;
        if (Tolerance.RmsPx <= 0) Tolerance.RmsPx = 6;
        if (Tolerance.RotDeg <= 0) Tolerance.RotDeg = 0.8;
        if (Tolerance.ScaleTol <= 0) Tolerance.ScaleTol = 0.02;
        if (Infer.Conf <= 0) Infer.Conf = 0.35;
        if (Infer.Nms <= 0) Infer.Nms = 0.5;
    }
}

internal sealed class AnchorInferConfig
{
    /// <summary>检出置信度下限（默认 0.35）。</summary>
    public double Conf { get; set; } = 0.35;

    public double Nms { get; set; } = 0.5;
}

internal sealed class AnchorCalib
{
    public int Width { get; set; }

    public int Height { get; set; }

    /// <summary>拍基准的时间，非空即视为已标定。</summary>
    public string At { get; set; } = "";

    public List<AnchorItem> Anchors { get; set; } = new();
}

/// <summary>基准帧里一个锚点的期望位置（像素，框中心）。</summary>
internal sealed class AnchorItem
{
    public string Cls { get; set; } = "";

    public double Ex { get; set; }

    public double Ey { get; set; }

    public double W { get; set; }

    public double H { get; set; }

    public double Conf { get; set; }
}

internal sealed class AnchorTolerance
{
    /// <summary>X/Y 各自允许的最大平移（像素）。</summary>
    public double ShiftPx { get; set; } = 12;

    /// <summary>最大旋转（度）。</summary>
    public double RotDeg { get; set; } = 0.8;

    /// <summary>锚点间距相对变化上限（0.02 = ±2%）。</summary>
    public double ScaleTol { get; set; } = 0.02;

    /// <summary>多个锚点相对平移后互相不一致的 RMS 上限（像素）。</summary>
    public double RmsPx { get; set; } = 6;

    /// <summary>少于该数量直接判锚点缺失。</summary>
    public int MinAnchors { get; set; } = 2;
}

/// <summary>
/// fixtures_v2/&lt;机种&gt;/ 的读写：profile.json、model.onnx、calib.png、shots/、history.csv。
/// 文件组织沿用 FixtureStore 的约定（camelCase、缩进、备份轮转）。
/// </summary>
internal static class AnchorStore
{
    public const string ProfileFile = "profile.json";
    public const string ModelFile = "model.onnx";
    public const string CalibFile = "calib.png";
    public const string OverlayFile = "calib_overlay.png";
    public const string ShotsDir = "shots";
    public const string HistoryFile = "history.csv";
    public const string BackupDir = "backup";

    /// <summary>类别名前缀：只有以它开头的类别才参与锚点标定与判定。</summary>
    public const string AnchorPrefix = "anchor";

    private static readonly JsonSerializerOptions Json = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
    };

    public static string Root => AppPaths.AnchorV2Dir;

    public static string Dir(string model) => Path.Combine(Root, Sanitize(model));

    public static string FileOf(string model, string file) => Path.Combine(Dir(model), file);

    public static string Sanitize(string model)
    {
        var name = (model ?? "").Trim();
        if (name.Length == 0) return "B001";
        var invalid = Path.GetInvalidFileNameChars();
        var chars = name.Select(c => invalid.Contains(c) ? '_' : c).ToArray();
        var clean = new string(chars).Trim();
        return clean.Length == 0 ? "B001" : clean;
    }

    public static IReadOnlyList<string> ListModels()
    {
        var list = new List<string>();
        try
        {
            if (!Directory.Exists(Root)) return list;
            foreach (var dir in Directory.GetDirectories(Root))
            {
                var name = Path.GetFileName(dir);
                if (string.Equals(name, BackupDir, StringComparison.OrdinalIgnoreCase)) continue;
                list.Add(name);
            }
        }
        catch (Exception ex)
        {
            Log.Warn("列举 V2 机种失败: " + ex.Message);
        }
        list.Sort(StringComparer.OrdinalIgnoreCase);
        return list;
    }

    public static void EnsureModel(string model) => Directory.CreateDirectory(Dir(model));

    public static AnchorProfile LoadOrCreate(string model)
    {
        var path = FileOf(model, ProfileFile);
        try
        {
            if (File.Exists(path))
            {
                var text = File.ReadAllText(path);
                var profile = JsonSerializer.Deserialize<AnchorProfile>(text, Json);
                if (profile != null)
                {
                    profile.Model = Sanitize(model);
                    profile.Normalize();
                    return profile;
                }
            }
        }
        catch (Exception ex)
        {
            Log.Warn("读取 V2 配方失败，按默认值处理: " + ex.Message);
        }
        var fresh = AnchorProfile.CreateDefault(Sanitize(model));
        fresh.Normalize();
        return fresh;
    }

    public static void Save(AnchorProfile profile)
    {
        if (profile == null) return;
        profile.Normalize();
        profile.Model = Sanitize(profile.Model);
        profile.UpdatedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        var dir = Dir(profile.Model);
        Directory.CreateDirectory(dir);
        var path = Path.Combine(dir, ProfileFile);
        try
        {
            if (File.Exists(path)) Backup(dir, path);
            File.WriteAllText(path, JsonSerializer.Serialize(profile, Json));
        }
        catch (Exception ex)
        {
            Log.Warn("保存 V2 配方失败: " + ex.Message);
        }
    }

    private static void Backup(string dir, string path)
    {
        try
        {
            var backupDir = Path.Combine(dir, BackupDir);
            Directory.CreateDirectory(backupDir);
            var stamp = DateTime.Now.ToString("yyyyMMdd-HHmmss");
            File.Copy(path, Path.Combine(backupDir, $"profile-{stamp}.json"), true);
            foreach (var old in Directory.GetFiles(backupDir, "profile-*.json")
                         .OrderByDescending(File.GetLastWriteTimeUtc).Skip(10))
            {
                try { File.Delete(old); } catch { }
            }
        }
        catch (Exception ex)
        {
            Log.Warn("备份 V2 配方失败: " + ex.Message);
        }
    }

    /// <summary>本机种专用模型副本；没有则返回空串（不回退全局模型，避免用错模型）。</summary>
    public static string LocalModelPath(string model)
    {
        var path = FileOf(model, ModelFile);
        return File.Exists(path) ? path : "";
    }

    public static bool CopyFromGlobalModel(string model, out string error)
    {
        error = "";
        try
        {
            var source = AppPaths.MyModelOnnx;
            if (!File.Exists(source))
            {
                error = "还没有训练好的模型（mymodel.onnx 不存在）。";
                return false;
            }
            EnsureModel(model);
            File.Copy(source, FileOf(model, ModelFile), true);
            return true;
        }
        catch (Exception ex)
        {
            error = "复制模型失败: " + ex.Message;
            return false;
        }
    }

    public static IReadOnlyList<string> AnchorClasses(IReadOnlyList<string> all)
    {
        var list = new List<string>();
        if (all == null) return list;
        foreach (var name in all)
        {
            if (string.IsNullOrWhiteSpace(name)) continue;
            if (name.StartsWith(AnchorPrefix, StringComparison.OrdinalIgnoreCase)) list.Add(name);
        }
        return list;
    }

    public static Mat LoadImage(string model, string file)
    {
        var path = FileOf(model, file);
        if (!File.Exists(path)) return null;
        var mat = Cv2.ImRead(path, ImreadModes.Color);
        if (mat == null || mat.Empty())
        {
            mat?.Dispose();
            return null;
        }
        return mat;
    }

    public static bool SaveImage(string model, string file, Mat mat)
    {
        if (mat == null || mat.Empty()) return false;
        try
        {
            var path = FileOf(model, file);
            var dir = Path.GetDirectoryName(path);
            if (!string.IsNullOrWhiteSpace(dir)) Directory.CreateDirectory(dir);
            return Cv2.ImWrite(path, mat);
        }
        catch (Exception ex)
        {
            Log.Warn("保存 V2 图片失败: " + ex.Message);
            return false;
        }
    }
}
