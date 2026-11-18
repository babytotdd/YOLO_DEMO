using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace YoloFool;

internal sealed class BoxRecord
{
    public string Cn { get; set; } = "";
    public string En { get; set; } = "";
    public int X { get; set; }
    public int Y { get; set; }
    public int W { get; set; }
    public int H { get; set; }

    /// <summary>旋转角（度，顺时针，绕框中心）。0 表示普通水平框。</summary>
    public float Angle { get; set; }

    /// <summary>自动预标注生成的框：人工复核确认前不导出标签、不参与训练。</summary>
    public bool Auto { get; set; }

    /// <summary>自动框是否已被人工复核确认。</summary>
    public bool Confirmed { get; set; }

    /// <summary>自动框的模型置信度（0~1，仅 Auto 时有意义，用于复核时判断）。</summary>
    public float Conf { get; set; }

    /// <summary>自动框还没复核：既不导出标签，也不进训练集。</summary>
    [JsonIgnore]
    public bool Pending => Auto && !Confirmed;

    /// <summary>列表里显示的状态：手工 / 待复核 / 已确认。</summary>
    [JsonIgnore]
    public string StateText => !Auto ? "手工" : (Confirmed ? "已确认" : "待复核");

    [JsonIgnore]
    public bool Rotated => Math.Abs(Angle) > 0.01f;

    [JsonIgnore]
    public string BoundsText => Rotated ? $"{X},{Y},{W},{H} ∠{Angle:F0}°" : $"{X},{Y},{W},{H}";
}

internal sealed record TrainingSetInfo(string DataYaml, int TrainCount, int ValCount, int ClassCount, IReadOnlyList<string> ClassNames);

/// <summary>
/// 数据集仓库（最核心的“学习”能力）：
/// - 框选命名后立即把图片复制到 mydataset\images，并重写该图的 YOLO txt（归一化 cx cy w h）；
/// - classes.txt 只追加不重排，保证类别 id 永远稳定（旧程序最大的坑）；
/// - 训练前生成 training\{images,labels}\{train,val} 和 data.yaml。
/// </summary>
internal sealed class DatasetStore
{
    private static readonly JsonSerializerOptions JsonOpts = new()
    {
        WriteIndented = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
    };

    private readonly LabelCatalog _labels;
    private readonly Dictionary<string, List<BoxRecord>> _boxes = new(StringComparer.OrdinalIgnoreCase);
    private readonly List<string> _classes = new();
    private readonly Dictionary<string, string> _exports = new(StringComparer.OrdinalIgnoreCase);
    private bool _classesDirty;
    private bool _exportsDirty;

    private DatasetStore(LabelCatalog labels) => _labels = labels;

    public LabelCatalog Labels => _labels;
    public IReadOnlyList<string> Classes => _classes;

    public int ImageCount => _boxes.Count(kv => kv.Value.Count > 0 && File.Exists(kv.Key));
    public int BoxCount => _boxes.Values.Sum(v => v.Count);

    public static DatasetStore Load(LabelCatalog labels)
    {
        var store = new DatasetStore(labels);
        store.LoadAnnotations();
        store.LoadClasses();
        store.LoadExports();
        return store;
    }

    public IReadOnlyList<BoxRecord> GetBoxes(string imagePath)
    {
        var key = Key(imagePath);
        return _boxes.TryGetValue(key, out var list) ? list : Array.Empty<BoxRecord>();
    }

    public int AddBox(string imagePath, string displayName, Rectangle rect) => AddBox(imagePath, displayName, rect, 0f);

    public int AddBox(string imagePath, string displayName, Rectangle rect, float angleDeg)
    {
        var key = Key(imagePath);
        var name = (displayName ?? "").Trim();
        if (name.Length == 0) throw new ArgumentException("目标名称不能为空", nameof(displayName));
        var en = _labels.EnsureEnglish(name);
        _labels.Save();
        EnsureClass(en);

        if (!_boxes.TryGetValue(key, out var list)) _boxes[key] = list = new List<BoxRecord>();
        list.Add(new BoxRecord
        {
            Cn = name,
            En = en,
            X = rect.X,
            Y = rect.Y,
            W = rect.Width,
            H = rect.Height,
            Angle = Math.Abs(angleDeg) < 0.01f ? 0f : (float)Math.Round(angleDeg, 1),
        });
        Save();
        SyncImage(key);
        return list.Count - 1;
    }

    /// <summary>拖动已保存框后写回几何（位置/大小/角度），并重新导出本图标签。</summary>
    public void UpdateBox(string imagePath, int index, Rectangle rect, float angleDeg)
    {
        var key = Key(imagePath);
        if (!_boxes.TryGetValue(key, out var list) || index < 0 || index >= list.Count) return;
        var b = list[index];
        b.X = rect.X;
        b.Y = rect.Y;
        b.W = rect.Width;
        b.H = rect.Height;
        b.Angle = Math.Abs(angleDeg) < 0.01f ? 0f : (float)Math.Round(angleDeg, 1);
        Save();
        SyncImage(key);
    }

    public void RenameBox(string imagePath, int index, string displayName)
    {
        var key = Key(imagePath);
        if (!_boxes.TryGetValue(key, out var list) || index < 0 || index >= list.Count) return;
        var name = (displayName ?? "").Trim();
        if (name.Length == 0) return;
        list[index].Cn = name;
        list[index].En = _labels.EnsureEnglish(name);
        _labels.Save();
        EnsureClass(list[index].En);
        Save();
        SyncImage(key);
    }

    public void RemoveBox(string imagePath, int index)
    {
        var key = Key(imagePath);
        if (!_boxes.TryGetValue(key, out var list) || index < 0 || index >= list.Count) return;
        list.RemoveAt(index);
        Save();
        SyncImage(key);
    }

    /// <summary>这个框会不会导出到训练集：手工框和已确认的自动框会，待复核的不会。</summary>
    public static bool Exportable(BoxRecord b) => b != null && (!b.Auto || b.Confirmed);

    /// <summary>参与训练导出的框（不含待复核的自动框）。</summary>
    public IReadOnlyList<BoxRecord> GetExportableBoxes(string imagePath) =>
        GetBoxes(imagePath).Where(Exportable).ToList();

    public int PendingCount(string imagePath) => GetBoxes(imagePath).Count(b => b.Pending);

    public int PendingBoxCount => _boxes.Values.Sum(v => v.Count(b => b.Pending));

    public bool HasPending => PendingBoxCount > 0;

    /// <summary>
    /// 批量写入自动预标注的框：一次保存 + 一次导出，避免几十张图几百次重写 annotations.json。
    /// 这些框标记为 Auto，人工复核确认前不会写进 mydataset/labels，也不进训练集。
    /// </summary>
    public int AddAutoBoxes(string imagePath, IEnumerable<(string DisplayName, Rectangle Bounds, float Confidence)> items)
    {
        if (items == null) return 0;
        var key = Key(imagePath);
        int added = 0;
        foreach (var it in items)
        {
            var name = (it.DisplayName ?? "").Trim();
            if (name.Length == 0) continue;
            if (it.Bounds.Width < 6 || it.Bounds.Height < 6) continue;

            var en = _labels.EnsureEnglish(name);
            if (string.IsNullOrWhiteSpace(en)) continue;
            EnsureClass(en);

            if (!_boxes.TryGetValue(key, out var list)) _boxes[key] = list = new List<BoxRecord>();
            list.Add(new BoxRecord
            {
                Cn = name,
                En = en,
                X = it.Bounds.X,
                Y = it.Bounds.Y,
                W = it.Bounds.Width,
                H = it.Bounds.Height,
                Auto = true,
                Confirmed = false,
                Conf = Math.Clamp(it.Confidence, 0f, 1f),
            });
            added++;
        }

        if (added > 0)
        {
            _labels.Save();
            Save();
            SyncImage(key);
        }
        return added;
    }

    /// <summary>把本图所有（或指定序号）待复核的自动框标为“已确认”，随后导出并进入训练集。</summary>
    public int ConfirmAutoBoxes(string imagePath, int index = -1)
    {
        var key = Key(imagePath);
        if (!_boxes.TryGetValue(key, out var list)) return 0;

        int n = 0;
        for (int i = 0; i < list.Count; i++)
        {
            if (index >= 0 && i != index) continue;
            if (!list[i].Pending) continue;
            list[i].Confirmed = true;
            n++;
        }
        if (n > 0)
        {
            Save();
            SyncImage(key);
        }
        return n;
    }

    /// <summary>丢掉本图还没确认的自动框；人工框和已确认框保留。includeConfirmed 时连已确认的一起丢。</summary>
    public int RemoveAutoBoxes(string imagePath, bool includeConfirmed = false)
    {
        var key = Key(imagePath);
        if (!_boxes.TryGetValue(key, out var list)) return 0;

        int n = list.RemoveAll(b => b.Auto && (includeConfirmed || !b.Confirmed));
        if (n > 0)
        {
            Save();
            SyncImage(key);
        }
        return n;
    }

    /// <summary>一次清空所有图片的自动框（默认只清待复核的），只写一次 annotations.json。</summary>
    public int RemoveAllAutoBoxes(bool includeConfirmed = false)
    {
        int total = 0;
        var touched = new List<string>();
        foreach (var kv in _boxes)
        {
            int n = kv.Value.RemoveAll(b => b.Auto && (includeConfirmed || !b.Confirmed));
            if (n <= 0) continue;
            total += n;
            touched.Add(kv.Key);
        }
        if (total > 0)
        {
            Save();
            foreach (var key in touched) SyncImage(key);
        }
        return total;
    }

    public IReadOnlyDictionary<string, int> ClassCounts()
    {
        var d = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        foreach (var b in _boxes.Values.SelectMany(v => v))
        {
            if (string.IsNullOrWhiteSpace(b.En)) continue;
            d[b.En] = d.TryGetValue(b.En, out var n) ? n + 1 : 1;
        }
        return d;
    }

    /// <summary>把当前标注整理成 YOLO 训练目录，返回 data.yaml 等信息。</summary>
    public TrainingSetInfo BuildTrainingView()
    {
        // 只有会导出标签的图（有手工框或已确认的自动框）才能进训练集。
        var entries = _boxes.Where(kv => File.Exists(kv.Key) && kv.Value.Any(Exportable)).ToList();
        if (entries.Count == 0)
            throw new InvalidOperationException(_boxes.Values.Any(v => v.Any(b => b.Pending))
                ? "当前只有待复核的自动框：请先复核确认，再开始训练。"
                : "还没有任何标注，请先在“学习训练”里框选目标。");

        foreach (var kv in entries) SyncImage(kv.Key);

        if (Directory.Exists(AppPaths.TrainingDir)) Directory.Delete(AppPaths.TrainingDir, true);
        var trainImg = Path.Combine(AppPaths.TrainingDir, "images", "train");
        var valImg = Path.Combine(AppPaths.TrainingDir, "images", "val");
        var trainLbl = Path.Combine(AppPaths.TrainingDir, "labels", "train");
        var valLbl = Path.Combine(AppPaths.TrainingDir, "labels", "val");
        foreach (var d in new[] { trainImg, valImg, trainLbl, valLbl }) Directory.CreateDirectory(d);

        var items = entries.Select(kv => (Image: Path.Combine(AppPaths.ImagesDir, ExportNameFor(kv.Key)), Key: kv.Key)).ToList();
        var rnd = new Random(20260501);
        items = items.OrderBy(_ => rnd.Next()).ToList();
        int valCount = items.Count >= 5 ? Math.Max(1, items.Count / 5) : 0;

        for (int i = 0; i < items.Count; i++)
        {
            bool isVal = i < valCount;
            var imgDest = Path.Combine(isVal ? valImg : trainImg, Path.GetFileName(items[i].Image));
            var lblDest = Path.Combine(isVal ? valLbl : trainLbl, Path.GetFileNameWithoutExtension(items[i].Image) + ".txt");
            File.Copy(items[i].Image, imgDest, true);
            var lblSrc = LabelPathFor(items[i].Image);
            if (File.Exists(lblSrc)) File.Copy(lblSrc, lblDest, true);
        }

        var names = string.Join(", ", _classes.Select(c => $"'{c.Replace("'", "''")}'"));
        var yaml = new StringBuilder();
        yaml.AppendLine("# 由 YoloFool 自动生成，请勿手工修改");
        yaml.AppendLine($"path: {AppPaths.TrainingDir.Replace('\\', '/')}");
        yaml.AppendLine("train: images/train");
        yaml.AppendLine($"val: images/{(valCount > 0 ? "val" : "train")}");
        yaml.AppendLine($"nc: {_classes.Count}");
        yaml.AppendLine($"names: [{names}]");
        File.WriteAllText(AppPaths.DataYamlFile, yaml.ToString(), new UTF8Encoding(false));
        Log.Info($"训练集已生成：{items.Count - valCount} 训练 / {valCount} 验证，{_classes.Count} 类 → {AppPaths.DataYamlFile}");

        return new TrainingSetInfo(AppPaths.DataYamlFile, items.Count - valCount, valCount, _classes.Count, _classes.ToList());
    }

    /// <summary>删除某张图的标注后，同步删除导出的图片和标签。</summary>
    public void SyncImage(string imagePath)
    {
        var key = Key(imagePath);
        if (!_boxes.TryGetValue(key, out var list)) return;

        // 待复核的自动框不导出：既不写标签文件，也不把图片放进 mydataset/images。
        var exportable = list.Where(Exportable).ToList();
        if (exportable.Count == 0 || !File.Exists(key))
        {
            RemoveExported(key);
            return;
        }

        try
        {
            var name = ExportNameFor(key);
            Directory.CreateDirectory(AppPaths.ImagesDir);
            Directory.CreateDirectory(AppPaths.LabelsDir);
            File.Copy(key, Path.Combine(AppPaths.ImagesDir, name), true);

            var size = ImageUtil.GetSize(key);
            var sb = new StringBuilder();
            foreach (var b in exportable)
            {
                var en = string.IsNullOrWhiteSpace(b.En) ? _labels.EnsureEnglish(b.Cn) : b.En;
                b.En = en;
                EnsureClass(en);
                int id = _classes.IndexOf(en);
                if (id < 0) { EnsureClass(en); id = _classes.IndexOf(en); }

                // YOLO 检测标签只有水平框：旋转框导出时取水平外接框
                var box = b.Rotated
                    ? Rectangle.Round(RotatedBox.AxisAligned(b.X, b.Y, b.W, b.H, b.Angle))
                    : new Rectangle(b.X, b.Y, b.W, b.H);
                box = Rectangle.Intersect(box, new Rectangle(0, 0, Math.Max(1, size.Width), Math.Max(1, size.Height)));
                if (box.Width <= 0 || box.Height <= 0) continue;

                double cx = (box.X + box.Width / 2.0) / size.Width;
                double cy = (box.Y + box.Height / 2.0) / size.Height;
                double nw = (double)box.Width / size.Width;
                double nh = (double)box.Height / size.Height;
                cx = Math.Clamp(cx, 0, 1); cy = Math.Clamp(cy, 0, 1);
                nw = Math.Clamp(nw, 0, 1); nh = Math.Clamp(nh, 0, 1);
                sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "{0} {1:F6} {2:F6} {3:F6} {4:F6}", id, cx, cy, nw, nh));
            }
            File.WriteAllText(LabelPathFor(Path.Combine(AppPaths.ImagesDir, name)), sb.ToString(), new UTF8Encoding(false));
        }
        catch (Exception ex)
        {
            Log.Error($"同步数据集失败：{key}", ex);
            throw;
        }
    }

    private string ExportNameFor(string key)
    {
        if (_exports.TryGetValue(key, out var existing)) return existing;

        var ext = Path.GetExtension(key).ToLowerInvariant();
        if (ext.Length == 0) ext = ".jpg";
        var stem = Path.GetFileNameWithoutExtension(key);
        var safe = new StringBuilder();
        foreach (var ch in stem)
        {
            if (ch < 128 && char.IsLetterOrDigit(ch)) safe.Append(ch);
            else if (ch is '-' or '_') safe.Append(ch);
            else if (ch == ' ') safe.Append('_');
        }
        var baseName = safe.ToString().Trim('_');
        if (baseName.Length == 0) baseName = "img";

        var name = baseName + ext;
        int i = 1;
        while (_exports.Any(kv => string.Equals(kv.Value, name, StringComparison.OrdinalIgnoreCase) && !string.Equals(kv.Key, key, StringComparison.OrdinalIgnoreCase))
               || (File.Exists(Path.Combine(AppPaths.ImagesDir, name)) && !_exports.ContainsKey(key)))
        {
            name = $"{baseName}_{++i}{ext}";
        }

        _exports[key] = name;
        _exportsDirty = true;
        SaveExports();
        return name;
    }

    private void RemoveExported(string key)
    {
        if (!_exports.TryGetValue(key, out var name)) return;
        try
        {
            var img = Path.Combine(AppPaths.ImagesDir, name);
            var lbl = Path.Combine(AppPaths.LabelsDir, Path.GetFileNameWithoutExtension(name) + ".txt");
            if (File.Exists(img)) File.Delete(img);
            if (File.Exists(lbl)) File.Delete(lbl);
        }
        catch (Exception ex)
        {
            Log.Error($"删除导出文件失败：{key}", ex);
        }
    }

    private string LabelPathFor(string exportedImagePath) =>
        Path.Combine(AppPaths.LabelsDir, Path.GetFileNameWithoutExtension(exportedImagePath) + ".txt");

    private void EnsureClass(string en)
    {
        if (string.IsNullOrWhiteSpace(en)) return;
        if (_classes.Contains(en, StringComparer.OrdinalIgnoreCase)) return;
        _classes.Add(en);
        _classesDirty = true;
        SaveClasses();
        Log.Info($"新增类别 id={_classes.Count - 1}：{en}");
    }

    private void Save()
    {
        SaveAnnotations();
        if (_classesDirty) SaveClasses();
        if (_exportsDirty) SaveExports();
    }

    private void SaveAnnotations()
    {
        try
        {
            Directory.CreateDirectory(AppPaths.DatasetDir);
            var map = _boxes.OrderBy(kv => kv.Key, StringComparer.OrdinalIgnoreCase)
                            .ToDictionary(kv => kv.Key, kv => kv.Value);
            AtomicWrite(AppPaths.AnnotationsFile, JsonSerializer.Serialize(map, JsonOpts));
        }
        catch (Exception ex)
        {
            Log.Error("保存 annotations.json 失败", ex);
        }
    }

    private void SaveClasses()
    {
        try
        {
            Directory.CreateDirectory(AppPaths.DatasetDir);
            AtomicWrite(AppPaths.ClassesFile, string.Join(Environment.NewLine, _classes) + (_classes.Count > 0 ? Environment.NewLine : ""));
            _classesDirty = false;
        }
        catch (Exception ex)
        {
            Log.Error("保存 classes.txt 失败", ex);
        }
    }

    private void SaveExports()
    {
        try
        {
            Directory.CreateDirectory(AppPaths.DatasetDir);
            AtomicWrite(AppPaths.ExportMapFile, JsonSerializer.Serialize(_exports, JsonOpts));
            _exportsDirty = false;
        }
        catch (Exception ex)
        {
            Log.Error("保存 export_map.json 失败", ex);
        }
    }

    private void LoadAnnotations()
    {
        try
        {
            if (!File.Exists(AppPaths.AnnotationsFile)) return;
            var raw = JsonSerializer.Deserialize<Dictionary<string, List<BoxRecord>>>(File.ReadAllText(AppPaths.AnnotationsFile));
            foreach (var kv in raw ?? new Dictionary<string, List<BoxRecord>>())
            {
                if (string.IsNullOrWhiteSpace(kv.Key)) continue;
                var key = NormalizeKey(kv.Key);
                _boxes[key] = (kv.Value ?? new List<BoxRecord>()).Where(b => b != null).ToList();
            }
        }
        catch (Exception ex)
        {
            Log.Error("读取 annotations.json 失败", ex);
        }
    }

    private void LoadClasses()
    {
        try
        {
            if (!File.Exists(AppPaths.ClassesFile)) return;
            foreach (var line in File.ReadAllLines(AppPaths.ClassesFile))
            {
                var s = line.Trim();
                if (s.Length > 0 && !_classes.Contains(s, StringComparer.OrdinalIgnoreCase)) _classes.Add(s);
            }
        }
        catch (Exception ex)
        {
            Log.Error("读取 classes.txt 失败", ex);
        }
    }

    private void LoadExports()
    {
        try
        {
            if (!File.Exists(AppPaths.ExportMapFile)) return;
            var raw = JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(AppPaths.ExportMapFile));
            foreach (var kv in raw ?? new Dictionary<string, string>())
                if (!string.IsNullOrWhiteSpace(kv.Key) && !string.IsNullOrWhiteSpace(kv.Value))
                    _exports[NormalizeKey(kv.Key)] = kv.Value;
        }
        catch (Exception ex)
        {
            Log.Error("读取 export_map.json 失败", ex);
        }
    }

    private static string Key(string path) => NormalizeKey(path);

    private static string NormalizeKey(string path)
    {
        try { return Path.GetFullPath(path); }
        catch { return path; }
    }

    private static void AtomicWrite(string path, string content)
    {
        var tmp = path + ".tmp";
        File.WriteAllText(tmp, content, new UTF8Encoding(false));
        File.Move(tmp, path, true);
    }
}
