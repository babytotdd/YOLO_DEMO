using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace YoloFool;

/// <summary>
/// 最近命名的目标名（最新在前，最多保留 8 个；命名弹窗只展示前 3 个）。
/// 存在 mydataset\recent_names.json，删掉即可清空。
/// </summary>
internal static class RecentNames
{
    private const int Keep = 8;
    private const int Show = 3;
    private static List<string> _items;

    private static string FilePath => Path.Combine(AppPaths.DatasetDir, "recent_names.json");

    /// <summary>命名弹窗里要展示的最近 3 个名字（最新在前）。</summary>
    public static IReadOnlyList<string> Items
    {
        get { EnsureLoaded(); return _items.Take(Show).ToList(); }
    }

    public static void Push(string name)
    {
        var n = (name ?? "").Trim();
        if (n.Length == 0) return;
        EnsureLoaded();
        _items.RemoveAll(x => string.Equals(x, n, StringComparison.OrdinalIgnoreCase));
        _items.Insert(0, n);
        if (_items.Count > Keep) _items.RemoveRange(Keep, _items.Count - Keep);
        Save();
    }

    private static void EnsureLoaded()
    {
        if (_items != null) return;
        _items = new List<string>();
        try
        {
            if (!File.Exists(FilePath)) return;
            var raw = JsonSerializer.Deserialize<List<string>>(File.ReadAllText(FilePath));
            if (raw == null) return;
            foreach (var s in raw)
            {
                var v = (s ?? "").Trim();
                if (v.Length == 0 || _items.Contains(v, StringComparer.OrdinalIgnoreCase)) continue;
                _items.Add(v);
            }
        }
        catch (Exception ex)
        {
            Log.Error("读取 recent_names.json 失败", ex);
        }
    }

    private static void Save()
    {
        try
        {
            Directory.CreateDirectory(AppPaths.DatasetDir);
            var tmp = FilePath + ".tmp";
            File.WriteAllText(tmp, JsonSerializer.Serialize(_items, new JsonSerializerOptions { WriteIndented = true }),
                new UTF8Encoding(false));
            File.Move(tmp, FilePath, true);
        }
        catch (Exception ex)
        {
            Log.Error("保存 recent_names.json 失败", ex);
        }
    }
}
