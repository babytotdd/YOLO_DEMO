using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using OpenCvSharp;

namespace YoloFool;

/// <summary>一个机种的检测配方。所有阈值都能在界面上改，不写死在判定代码里。</summary>
internal sealed class FixtureRecipe
{
    public int Schema { get; set; } = 1;
    public string Model { get; set; } = "";
    public string Name { get; set; } = "";
    public int Version { get; set; } = 1;
    public string UpdatedAt { get; set; } = "";
    public FixtureCameraConfig Camera { get; set; } = new();
    public FixtureRoi Roi { get; set; } = new();
    public FixtureCalibrationInfo Calibration { get; set; } = new();
    public FixtureTolerance Tolerance { get; set; } = new();
    public FixtureQualityConfig Quality { get; set; } = new();
    public FixtureSegment Segment { get; set; } = new();

    /// <summary>"shape"=轮廓形状比对（旧）；"zone"=安全区像素判定（不要标定）。</summary>
    public string JudgeMode { get; set; } = "shape";

    public FixtureZoneConfig Zone { get; set; } = new();
    public int ConfirmFrames { get; set; } = 1;

    public static FixtureRecipe CreateDefault(string model) => new()
    {
        Model = model,
        Name = model,
        UpdatedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
    };

    public List<string> Validate()
    {
        var errors = new List<string>();
        if (Camera.Index < 0) errors.Add("相机序号无效");

        if (string.Equals(JudgeMode, "zone", StringComparison.OrdinalIgnoreCase))
        {
            // 安全区模式不依赖 mm：不检查标定和 mm 公差。
            if (Zone == null || !Zone.Enabled || Zone.W < 16 || Zone.H < 16)
                errors.Add("还没有设置安全区，请先“按金样板生成安全区”或手动画框");
            return errors;
        }

        if (Calibration.MmPerPixel <= 0) errors.Add("未标定 mm/像素");
        if (Tolerance.DxMm <= 0 || Tolerance.DyMm <= 0) errors.Add("位移公差必须大于 0");
        if (Tolerance.AreaRatioMin >= Tolerance.AreaRatioMax) errors.Add("面积比下限必须小于上限");
        return errors;
    }
}

/// <summary>配方文件与模板图的读写，附带版本备份。</summary>
internal static class FixtureStore
{
    public const string RecipeFile = "recipe.json";
    public const string EmptyFile = "empty.png";
    public const string GoldenFile = "golden.png";
    public const string GoldenMaskFile = "golden_mask.png";

    private static readonly JsonSerializerOptions Json = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
    };

    public static string Root => AppPaths.FixturesDir;

    public static string Dir(string model) => Path.Combine(Root, Sanitize(model));

    public static string FileOf(string model, string file) => Path.Combine(Dir(model), file);

    public static string Sanitize(string model)
    {
        var name = (model ?? "").Trim();
        foreach (var c in Path.GetInvalidFileNameChars()) name = name.Replace(c, '_');
        return string.IsNullOrWhiteSpace(name) ? "B001" : name;
    }

    public static IReadOnlyList<string> ListModels()
    {
        try
        {
            if (!Directory.Exists(Root)) return Array.Empty<string>();
            return Directory.EnumerateDirectories(Root)
                .Select(Path.GetFileName)
                .Where(n => !string.IsNullOrWhiteSpace(n) && !string.Equals(n, "backup", StringComparison.OrdinalIgnoreCase))
                .OrderBy(n => n, StringComparer.OrdinalIgnoreCase)
                .ToArray();
        }
        catch (Exception ex)
        {
            Log.Warn("枚举机种失败: " + ex.Message);
            return Array.Empty<string>();
        }
    }

    public static void EnsureModel(string model) => Directory.CreateDirectory(Dir(model));

    public static FixtureRecipe LoadOrCreate(string model)
    {
        var path = FileOf(model, RecipeFile);
        if (File.Exists(path))
        {
            try
            {
                var recipe = JsonSerializer.Deserialize<FixtureRecipe>(File.ReadAllText(path), Json);
                if (recipe != null)
                {
                    // 目录名才是机种身份：复制/改名目录后必须按目录保存，否则会写回原来的机种。
                    recipe.Model = model;
                    recipe.Camera ??= new FixtureCameraConfig();
                    recipe.Roi ??= new FixtureRoi();
                    recipe.Calibration ??= new FixtureCalibrationInfo();
                    recipe.Tolerance ??= new FixtureTolerance();
                    recipe.Quality ??= new FixtureQualityConfig();
                    recipe.Segment ??= new FixtureSegment();
                    return recipe;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"读取配方失败 {path}", ex);
            }
        }

        var created = FixtureRecipe.CreateDefault(model);
        EnsureModel(model);
        Save(created);
        return created;
    }

    public static void Save(FixtureRecipe recipe)
    {
        if (recipe == null) return;
        var dir = Dir(recipe.Model);
        Directory.CreateDirectory(dir);
        var path = Path.Combine(dir, RecipeFile);

        try
        {
            if (File.Exists(path))
            {
                var backupDir = Path.Combine(dir, "backup");
                Directory.CreateDirectory(backupDir);
                var stamp = DateTime.Now.ToString("yyyyMMdd-HHmmss");
                File.Copy(path, Path.Combine(backupDir, $"recipe-{stamp}.json"), true);
                foreach (var old in Directory.EnumerateFiles(backupDir, "recipe-*.json")
                             .OrderByDescending(f => f).Skip(10))
                {
                    try { File.Delete(old); } catch { }
                }
            }

            recipe.UpdatedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            File.WriteAllText(path, JsonSerializer.Serialize(recipe, Json));
        }
        catch (Exception ex)
        {
            Log.Error("保存配方失败", ex);
            throw;
        }
    }

    /// <summary>读取模板图；不存在返回 null。</summary>
    public static Mat LoadImage(string model, string file)
    {
        var path = FileOf(model, file);
        if (!File.Exists(path)) return null;
        var mat = Cv2.ImRead(path, ImreadModes.Color);
        return mat == null || mat.Empty() ? null : mat;
    }

    public static bool SaveImage(string model, string file, Mat mat)
    {
        try
        {
            EnsureModel(model);
            return Cv2.ImWrite(FileOf(model, file), mat);
        }
        catch (Exception ex)
        {
            Log.Error($"保存模板图失败 {file}", ex);
            return false;
        }
    }

    public static bool HasImage(string model, string file) => File.Exists(FileOf(model, file));
}
