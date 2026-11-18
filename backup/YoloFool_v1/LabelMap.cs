using System.Text.Json;
using ToolGood.Words;

namespace YoloFool;

internal sealed class LabelMap
{
    private readonly Dictionary<string, string> _cnToEn = new(StringComparer.Ordinal);

    public static LabelMap Load()
    {
        var m = new LabelMap();
        try
        {
            if (File.Exists(AppPaths.LabelMapFile))
            {
                var raw = JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(AppPaths.LabelMapFile));
                foreach (var kv in raw ?? new Dictionary<string, string>()) m._cnToEn[kv.Key] = kv.Value;
            }
        }
        catch { }
        return m;
    }

    public void Save()
    {
        Directory.CreateDirectory(AppPaths.DatasetDir);
        File.WriteAllText(AppPaths.LabelMapFile, JsonSerializer.Serialize(EnglishToChinese(), new JsonSerializerOptions { WriteIndented = true }));
    }

    // 显示名(中文) -> 存储名(拼音/英文)，重名自动加序号
    public string EnsureEnglish(string displayName)
    {
        displayName = displayName.Trim();
        if (_cnToEn.TryGetValue(displayName, out var known)) return known;
        string baseEn = ToAscii(displayName);
        string en = baseEn;
        int i = 1;
        var used = new HashSet<string>(_cnToEn.Values, StringComparer.OrdinalIgnoreCase);
        while (used.Contains(en)) en = baseEn + "_" + (++i);
        _cnToEn[displayName] = en;
        return en;
    }

    public string ToChinese(string stored)
    {
        foreach (var kv in _cnToEn)
            if (string.Equals(kv.Value, stored, StringComparison.OrdinalIgnoreCase)) return kv.Key;
        return stored;
    }

    public Dictionary<string, string> EnglishToChinese()
    {
        var d = new Dictionary<string, string>(StringComparer.Ordinal);
        foreach (var kv in _cnToEn) d[kv.Value] = kv.Key;
        return d;
    }

    public List<string> DisplayNamesByIndex() =>
        _cnToEn.Values.OrderBy(v => v, StringComparer.Ordinal).Select(ToChinese).ToList();

    private static string ToAscii(string name)
    {
        string py = name;
        try { py = WordsHelper.GetFirstPinyin(name); } catch { }
        var sb = new System.Text.StringBuilder();
        foreach (char c in py)
            if (char.IsLetterOrDigit(c) && c < 128) sb.Append(char.ToLowerInvariant(c));
            else if (c == ' ' || c == '_') sb.Append('_');
        string s = sb.ToString().Trim('_');
        return string.IsNullOrEmpty(s) ? "duixiang" : s;
    }
}
