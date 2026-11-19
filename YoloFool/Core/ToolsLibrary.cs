using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace YoloFool;

/// <summary>“工具 · 我的模型”页里的模型类别。</summary>
internal enum ToolLibCategory
{
    Trained = 0,   // 学习训练模型 my_models
    Demo = 1,      // 预置示例模型 demo/models
    Fixture = 2,   // 针床检测机种 fixtures
    AnchorV2 = 3,  // 针床定位V2学习库 fixtures_v2
}

/// <summary>模型库清单里的一行。</summary>
internal sealed class ToolModelEntry
{
    public string Name = "";
    public string Path = "";
    public string Dir = "";
    public long Bytes;
    public DateTime Modified;
    public string Summary = "";
    public bool IsMachine;
    public bool IsBackup;
}

/// <summary>机种管理清单里的一行。</summary>
internal sealed class ToolMachineEntry
{
    public string Name = "";
    public string Dir = "";
    public DateTime Modified;
    public string Summary = "";
    public bool IsAnchorV2;
}

internal static class ToolsLibrary
{
    /// <summary>每个类别 backup/ 目录的压缩包上限，超过就删最旧的。</summary>
    public const long ArchiveCapBytes = 512L * 1024 * 1024;

    public static string Name(ToolLibCategory c) => c switch
    {
        ToolLibCategory.Trained => "学习训练模型",
        ToolLibCategory.Demo => "预置示例模型",
        ToolLibCategory.Fixture => "针床检测机种",
        ToolLibCategory.AnchorV2 => "针床定位V2学习库",
        _ => c.ToString(),
    };

    public static string Root(ToolLibCategory c) => c switch
    {
        ToolLibCategory.Trained => AppPaths.ModelsDir,
        ToolLibCategory.Demo => Path.Combine(AppPaths.WorkspaceRoot, "demo", "models"),
        ToolLibCategory.Fixture => FixtureStore.Root,
        ToolLibCategory.AnchorV2 => AnchorStore.Root,
        _ => AppPaths.ModelsDir,
    };

    public static string ArchiveRoot(ToolLibCategory c) => Path.Combine(Root(c), "backup");

    public static bool IsMachine(ToolLibCategory c) =>
        c == ToolLibCategory.Fixture || c == ToolLibCategory.AnchorV2;

    public static string Stamp() => DateTime.Now.ToString("yyyyMMdd-HHmmss");

    public static string FormatSize(long bytes) => bytes >= 1024L * 1024 * 1024
        ? $"{bytes / 1024.0 / 1024 / 1024:F2} GB"
        : bytes >= 1024 * 1024
            ? $"{bytes / 1024.0 / 1024:F1} MB"
            : $"{bytes / 1024.0:F0} KB";

    // ------------------------------------------------------------ 清单

    public static List<ToolModelEntry> ScanModels(ToolLibCategory c)
    {
        var list = new List<ToolModelEntry>();
        switch (c)
        {
            case ToolLibCategory.Trained:
                AddFile(list, AppPaths.MyModelOnnx, "当前使用的训练模型", false);
                if (Directory.Exists(AppPaths.ModelBackupDir))
                {
                    foreach (var f in new DirectoryInfo(AppPaths.ModelBackupDir)
                                 .GetFiles("*.onnx")
                                 .OrderByDescending(f => f.LastWriteTime))
                        AddFile(list, f.FullName, "备份", true);
                }
                break;

            case ToolLibCategory.Demo:
            {
                var dir = Root(c);
                if (Directory.Exists(dir))
                {
                    foreach (var f in new DirectoryInfo(dir).GetFiles("*.onnx")
                                 .OrderBy(f => f.Name))
                        AddFile(list, f.FullName, "预置示例模型（只读）", false);
                }
                break;
            }

            case ToolLibCategory.Fixture:
                foreach (var model in FixtureStore.ListModels())
                {
                    var dir = FixtureStore.Dir(model);
                    var recipe = FixtureStore.FileOf(model, FixtureStore.RecipeFile);
                    var entry = new ToolModelEntry
                    {
                        Name = model,
                        Dir = dir,
                        IsMachine = true,
                        Modified = File.Exists(recipe) ? File.GetLastWriteTime(recipe)
                            : Directory.GetLastWriteTime(dir),
                        Summary = "配方 " + (File.Exists(recipe) ? "有" : "无")
                                  + " · 金样板 " + (File.Exists(FixtureStore.FileOf(model, FixtureStore.GoldenFile)) ? "有" : "无")
                                  + " · 空板 " + (File.Exists(FixtureStore.FileOf(model, FixtureStore.EmptyFile)) ? "有" : "无"),
                    };
                    list.Add(entry);
                }
                break;

            case ToolLibCategory.AnchorV2:
                foreach (var model in AnchorStore.ListModels())
                {
                    var dir = AnchorStore.Dir(model);
                    var profile = AnchorStore.FileOf(model, AnchorStore.ProfileFile);
                    var store = new AnchorSampleStore(model);
                    var stats = store.Stats();
                    var modelPath = AnchorStore.LocalModelPath(model);
                    var entry = new ToolModelEntry
                    {
                        Name = model,
                        Dir = dir,
                        IsMachine = true,
                        Modified = File.Exists(profile) ? File.GetLastWriteTime(profile)
                            : Directory.GetLastWriteTime(dir),
                        Summary = $"样本 {stats.Sum(s => s.Count)} 张 · {stats.Count} 类"
                                  + (string.IsNullOrEmpty(modelPath)
                                      ? " · 无模型"
                                      : " · 模型 " + FormatSize(new FileInfo(modelPath).Length))
                                  + (File.Exists(AnchorStore.FileOf(model, AnchorStore.CalibFile)) ? " · 有基准" : " · 未拍基准"),
                    };
                    list.Add(entry);
                }
                break;
        }

        return list;
    }

    private static void AddFile(List<ToolModelEntry> list, string path, string summary, bool backup)
    {
        if (!File.Exists(path)) return;
        var fi = new FileInfo(path);
        list.Add(new ToolModelEntry
        {
            Name = fi.Name,
            Path = fi.FullName,
            Dir = fi.DirectoryName ?? "",
            Bytes = fi.Length,
            Modified = fi.LastWriteTime,
            Summary = summary,
            IsBackup = backup,
        });
    }

    public static List<ToolMachineEntry> ScanMachines(ToolLibCategory c)
    {
        var list = new List<ToolMachineEntry>();
        if (c == ToolLibCategory.Fixture)
        {
            foreach (var model in FixtureStore.ListModels())
            {
                var dir = FixtureStore.Dir(model);
                var recipe = FixtureStore.FileOf(model, FixtureStore.RecipeFile);
                list.Add(new ToolMachineEntry
                {
                    Name = model,
                    Dir = dir,
                    Modified = File.Exists(recipe) ? File.GetLastWriteTime(recipe) : Directory.GetLastWriteTime(dir),
                    Summary = "金样板 " + (File.Exists(FixtureStore.FileOf(model, FixtureStore.GoldenFile)) ? "有" : "无")
                              + " · 空板 " + (File.Exists(FixtureStore.FileOf(model, FixtureStore.EmptyFile)) ? "有" : "无"),
                });
            }
        }
        else if (c == ToolLibCategory.AnchorV2)
        {
            foreach (var model in AnchorStore.ListModels())
            {
                var dir = AnchorStore.Dir(model);
                var stats = new AnchorSampleStore(model).Stats();
                list.Add(new ToolMachineEntry
                {
                    Name = model,
                    Dir = dir,
                    IsAnchorV2 = true,
                    Modified = File.Exists(AnchorStore.FileOf(model, AnchorStore.ProfileFile))
                        ? File.GetLastWriteTime(AnchorStore.FileOf(model, AnchorStore.ProfileFile))
                        : Directory.GetLastWriteTime(dir),
                    Summary = $"样本 {stats.Sum(s => s.Count)} 张 · {stats.Count} 类"
                              + (File.Exists(AnchorStore.FileOf(model, AnchorStore.CalibFile)) ? " · 有基准" : " · 未拍基准"),
                });
            }
        }

        return list;
    }

    public static long DirSize(string dir)
    {
        if (!Directory.Exists(dir)) return 0;
        long total = 0;
        try
        {
            foreach (var f in Directory.EnumerateFiles(dir, "*", SearchOption.AllDirectories))
            {
                try { total += new FileInfo(f).Length; } catch { }
            }
        }
        catch { }
        return total;
    }

    // ------------------------------------------------------------ 压缩 / 清理

    /// <summary>把机种目录整体压成 zip 放进该类别 backup/，压缩成功后删掉原目录。</summary>
    public static bool ArchiveMachine(ToolLibCategory c, string name, out string archive, out string error)
    {
        archive = "";
        error = "";
        var dir = MachineDir(c, name);
        if (string.IsNullOrEmpty(dir) || !Directory.Exists(dir))
        {
            error = "机种目录不存在：" + dir;
            return false;
        }

        try
        {
            var root = ArchiveRoot(c);
            Directory.CreateDirectory(root);
            var zip = Path.Combine(root, $"{SanitizeName(name)}_{Stamp()}.zip");
            var n = 1;
            while (File.Exists(zip))
                zip = Path.Combine(root, $"{SanitizeName(name)}_{Stamp()}_{n++}.zip");

            ZipFile.CreateFromDirectory(dir, zip, CompressionLevel.SmallestSize, false);
            if (!File.Exists(zip))
            {
                error = "压缩没有生成文件。";
                return false;
            }

            Directory.Delete(dir, true);
            archive = zip;
            Log.Info($"归档机种 {name} → {zip}");
            return true;
        }
        catch (Exception ex)
        {
            error = ex.Message;
            return false;
        }
    }

    /// <summary>backup/ 里压缩包超过上限时，从不最新删到够。</summary>
    public static int PurgeArchives(ToolLibCategory c, long cap = ArchiveCapBytes)
    {
        var root = ArchiveRoot(c);
        if (!Directory.Exists(root)) return 0;
        try
        {
            var files = new DirectoryInfo(root).GetFiles("*.zip")
                .OrderBy(f => f.LastWriteTime)
                .ToList();
            long total = files.Sum(f => f.Length);
            var removed = 0;
            foreach (var f in files)
            {
                if (total <= cap) break;
                total -= f.Length;
                try { f.Delete(); removed++; } catch { }
            }

            if (removed > 0) Log.Info($"{Name(c)} 备份超 {cap / 1024 / 1024}MB，自动删了 {removed} 个最旧压缩包。");
            return removed;
        }
        catch
        {
            return 0;
        }
    }

    public static string MachineDir(ToolLibCategory c, string name) => c switch
    {
        ToolLibCategory.Fixture => FixtureStore.Dir(name),
        ToolLibCategory.AnchorV2 => AnchorStore.Dir(name),
        _ => "",
    };

    public static string SanitizeName(string name) => Path.GetInvalidFileNameChars()
        .Aggregate((name ?? "").Trim(), (s, ch) => s.Replace(ch, '_'));
}
