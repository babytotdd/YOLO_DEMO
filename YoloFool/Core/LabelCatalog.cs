using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using ToolGood.Words;

namespace YoloFool;

/// <summary>
/// 类别中文名管理。
/// - labels_map.json 的格式固定为 { "英文/拼音存储名": "中文显示名" }；
/// - 内置 COCO 80 类中文名，开箱即用；
/// - 新目标命名时把中文转拼音作为训练用的 ASCII 类名（YOLO 的 classes 必须是 ASCII）。
/// </summary>
internal sealed class LabelCatalog
{
    private readonly Dictionary<string, string> _enToCn = new(StringComparer.OrdinalIgnoreCase);
    private bool _dirty;

    public static IReadOnlyDictionary<string, string> CocoZh { get; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
    {
        ["person"] = "人", ["bicycle"] = "自行车", ["car"] = "汽车", ["motorcycle"] = "摩托车",
        ["airplane"] = "飞机", ["bus"] = "公交车", ["train"] = "火车", ["truck"] = "卡车",
        ["boat"] = "船", ["traffic light"] = "交通灯", ["fire hydrant"] = "消防栓", ["stop sign"] = "停车标志",
        ["parking meter"] = "停车计时器", ["bench"] = "长椅", ["bird"] = "鸟", ["cat"] = "猫",
        ["dog"] = "狗", ["horse"] = "马", ["sheep"] = "羊", ["cow"] = "牛",
        ["elephant"] = "大象", ["bear"] = "熊", ["zebra"] = "斑马", ["giraffe"] = "长颈鹿",
        ["backpack"] = "背包", ["umbrella"] = "雨伞", ["handbag"] = "手提包", ["tie"] = "领带",
        ["suitcase"] = "行李箱", ["frisbee"] = "飞盘", ["skis"] = "双板滑雪板", ["snowboard"] = "单板滑雪板",
        ["sports ball"] = "球", ["kite"] = "风筝", ["baseball bat"] = "棒球棒", ["baseball glove"] = "棒球手套",
        ["skateboard"] = "滑板", ["surfboard"] = "冲浪板", ["tennis racket"] = "网球拍", ["bottle"] = "瓶子",
        ["wine glass"] = "酒杯", ["cup"] = "杯子", ["fork"] = "叉子", ["knife"] = "刀",
        ["spoon"] = "勺子", ["bowl"] = "碗", ["banana"] = "香蕉", ["apple"] = "苹果",
        ["sandwich"] = "三明治", ["orange"] = "橙子", ["broccoli"] = "西兰花", ["carrot"] = "胡萝卜",
        ["hot dog"] = "热狗", ["pizza"] = "披萨", ["donut"] = "甜甜圈", ["cake"] = "蛋糕",
        ["chair"] = "椅子", ["couch"] = "沙发", ["potted plant"] = "盆栽", ["bed"] = "床",
        ["dining table"] = "餐桌", ["toilet"] = "马桶", ["tv"] = "电视", ["laptop"] = "笔记本电脑",
        ["mouse"] = "鼠标", ["remote"] = "遥控器", ["keyboard"] = "键盘", ["cell phone"] = "手机",
        ["microwave"] = "微波炉", ["oven"] = "烤箱", ["toaster"] = "烤面包机", ["sink"] = "水槽",
        ["refrigerator"] = "冰箱", ["book"] = "书", ["clock"] = "时钟", ["vase"] = "花瓶",
        ["scissors"] = "剪刀", ["teddy bear"] = "泰迪熊", ["hair drier"] = "吹风机", ["toothbrush"] = "牙刷",
    };

    public static LabelCatalog Load()
    {
        var c = new LabelCatalog();
        foreach (var kv in CocoZh) c._enToCn[kv.Key] = kv.Value;
        try
        {
            if (File.Exists(AppPaths.LabelsMapFile))
            {
                var raw = JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(AppPaths.LabelsMapFile));
                foreach (var kv in raw ?? new Dictionary<string, string>())
                {
                    if (string.IsNullOrWhiteSpace(kv.Key)) continue;
                    c._enToCn[kv.Key.Trim()] = string.IsNullOrWhiteSpace(kv.Value) ? kv.Key.Trim() : kv.Value.Trim();
                }
            }
        }
        catch (Exception ex)
        {
            Log.Error("读取 labels_map.json 失败", ex);
        }
        return c;
    }

    public string Display(string rawCategory, int classId)
    {
        var raw = (rawCategory ?? "").Trim();
        if (_enToCn.TryGetValue(raw, out var cn) && !string.IsNullOrWhiteSpace(cn)) return cn;
        if (raw.Length == 0 || int.TryParse(raw, out _)) return $"类别 {classId}";
        return raw;
    }

    public string English(string rawCategory, int classId)
    {
        var raw = (rawCategory ?? "").Trim();
        return raw.Length == 0 || int.TryParse(raw, out _) ? $"class_{classId}" : raw;
    }

    /// <summary>中文显示名 → 存储用 ASCII 类名（幂等，重名自动加序号）。</summary>
    public string EnsureEnglish(string displayName)
    {
        var cn = (displayName ?? "").Trim();
        if (cn.Length == 0) throw new ArgumentException("目标名称不能为空", nameof(displayName));

        foreach (var kv in _enToCn)
            if (string.Equals(kv.Value, cn, StringComparison.Ordinal)) return kv.Key;

        var baseEn = ToAscii(cn);
        if (_enToCn.TryGetValue(baseEn, out var existing) &&
            string.Equals(existing, baseEn, StringComparison.OrdinalIgnoreCase))
        {
            // 之前是“未命名”（显示名就是拼音），现在补上中文显示名，不新增类别
            _enToCn[baseEn] = cn;
            _dirty = true;
            return baseEn;
        }

        var en = baseEn;
        int i = 1;
        while (_enToCn.ContainsKey(en)) en = $"{baseEn}_{++i}";
        _enToCn[en] = cn;
        _dirty = true;
        return en;
    }

    /// <summary>只预览存储名，不写入映射。</summary>
    public string PreviewEnglish(string displayName)
    {
        var cn = (displayName ?? "").Trim();
        if (cn.Length == 0) return "";
        foreach (var kv in _enToCn)
            if (string.Equals(kv.Value, cn, StringComparison.Ordinal)) return kv.Key;

        var baseEn = ToAscii(cn);
        if (_enToCn.TryGetValue(baseEn, out var existing) && string.Equals(existing, baseEn, StringComparison.OrdinalIgnoreCase))
            return baseEn;
        var en = baseEn;
        int i = 1;
        while (_enToCn.ContainsKey(en) && !string.Equals(_enToCn[en], cn, StringComparison.Ordinal)) en = $"{baseEn}_{++i}";
        return en;
    }

    public IEnumerable<string> KnownDisplayNames =>
        _enToCn.Values.Where(x => !string.IsNullOrWhiteSpace(x)).Distinct().OrderBy(x => x);

    public bool IsDirty => _dirty;

    public void Save(bool force = false)
    {
        if (!_dirty && !force) return;
        try
        {
            Directory.CreateDirectory(AppPaths.DatasetDir);
            var map = new SortedDictionary<string, string>(StringComparer.Ordinal);
            foreach (var kv in _enToCn.OrderBy(x => x.Key, StringComparer.OrdinalIgnoreCase)) map[kv.Key] = kv.Value;
            File.WriteAllText(AppPaths.LabelsMapFile,
                JsonSerializer.Serialize(map, new JsonSerializerOptions { WriteIndented = true }), Encoding.UTF8);
            _dirty = false;
        }
        catch (Exception ex)
        {
            Log.Error("保存 labels_map.json 失败", ex);
        }
    }

    private static string ToAscii(string name)
    {
        string py = name;
        try { py = WordsHelper.GetFirstPinyin(name); } catch { }
        var sb = new StringBuilder();
        foreach (var ch in py)
        {
            if (ch < 128 && char.IsLetterOrDigit(ch)) sb.Append(char.ToLowerInvariant(ch));
            else if (ch is ' ' or '-' or '_') sb.Append('_');
        }
        var s = sb.ToString().Trim('_');
        while (s.Contains("__")) s = s.Replace("__", "_");
        return string.IsNullOrEmpty(s) ? "object" : s;
    }
}
