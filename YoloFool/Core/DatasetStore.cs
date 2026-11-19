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

/// <summary>“标签管理”列表里的一行：一个类别有多少框、出现在多少张图上。</summary>
internal sealed record ClassStat(string En, string Cn, int Id, int Boxes, int Images, int Pending);

/// <summary>某个标签用到的训练内容（一张图片一条），用于弹窗里的列表 + 缩略图预览。</summary>
internal sealed record ClassImage(string ImagePath, string ExportName, int Boxes, int Pending, bool Exported)
{
    [JsonIgnore]
    public string FileName => Path.GetFileName(ImagePath);

    /// <summary>优先用已导出到 mydataset/images 的那份（训练真正用的），没有就用原图。</summary>
    [JsonIgnore]
    public string PreviewPath => Exported && ExportName.Length > 0
        ? Path.Combine(AppPaths.ImagesDir, ExportName)
        : ImagePath;

    [JsonIgnore]
    public string ExportText => Exported && ExportName.Length > 0 ? ExportName : "（未导出）";
}

/// <summary>删除标签的结果，用于给用户交代“删了多少、备份在哪”。</summary>
internal sealed record ClassRemovalResult(string En, string Cn, int Boxes, int Images, string BackupFile);

/// <summary>改标签名的结果：旧名 / 新名 / 动了多少框和图片 / 备份在哪。</summary>
internal sealed record ClassRenameResult(string OldEn, string NewEn, string Cn, int Boxes, int Images, string BackupFile);

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

    /// <summary>标签管理用：每个类别的框数 / 图片数 / 待复核框数，按类别 id 顺序排。</summary>
    public IReadOnlyList<ClassStat> ClassStats()
    {
        var boxes = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        var images = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        var pending = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        foreach (var kv in _boxes)
        {
            if (!File.Exists(kv.Key)) continue;
            foreach (var g in kv.Value.Where(b => !string.IsNullOrWhiteSpace(b.En)).GroupBy(b => b.En, StringComparer.OrdinalIgnoreCase))
            {
                boxes[g.Key] = boxes.TryGetValue(g.Key, out var n) ? n + g.Count() : g.Count();
                images[g.Key] = images.TryGetValue(g.Key, out var m) ? m + 1 : 1;
                int p = g.Count(b => b.Pending);
                if (p > 0) pending[g.Key] = pending.TryGetValue(g.Key, out var q) ? q + p : p;
            }
        }

        var list = new List<ClassStat>();
        for (int i = 0; i < _classes.Count; i++)
        {
            var en = _classes[i];
            string cn;
            try { cn = _labels.Display(en, i); } catch { cn = en; }
            list.Add(new ClassStat(
                en, cn, i,
                boxes.TryGetValue(en, out var b) ? b : 0,
                images.TryGetValue(en, out var im) ? im : 0,
                pending.TryGetValue(en, out var pd) ? pd : 0));
        }

        // classes.txt 里没有（但标注里存在）的类别也列出来，避免"看不见却删不掉"
        foreach (var en in boxes.Keys.Where(k => !_classes.Contains(k, StringComparer.OrdinalIgnoreCase)).OrderBy(x => x, StringComparer.OrdinalIgnoreCase))
        {
            string cn;
            try { cn = _labels.Display(en, -1); } catch { cn = en; }
            list.Add(new ClassStat(en, cn, -1, boxes[en], images.TryGetValue(en, out var im) ? im : 0,
                pending.TryGetValue(en, out var pd) ? pd : 0));
        }
        return list;
    }

    /// <summary>某个标签用到的所有图片（含框数与是否已导出），按文件名排序。</summary>
    public IReadOnlyList<ClassImage> ImagesOfClass(string en)
    {
        var target = (en ?? "").Trim();
        var list = new List<ClassImage>();
        if (target.Length == 0) return list;

        foreach (var kv in _boxes.OrderBy(k => Path.GetFileName(k.Key), StringComparer.OrdinalIgnoreCase))
        {
            if (!File.Exists(kv.Key)) continue;
            var mine = kv.Value.Where(b => string.Equals(b.En, target, StringComparison.OrdinalIgnoreCase)).ToList();
            if (mine.Count == 0) continue;

            _exports.TryGetValue(kv.Key, out var exported);
            var name = exported ?? "";
            bool exists = name.Length > 0 && File.Exists(Path.Combine(AppPaths.ImagesDir, name));
            list.Add(new ClassImage(kv.Key, name, mine.Count, mine.Count(b => b.Pending), exists));
        }
        return list;
    }

    /// <summary>
    /// 从数据集里彻底删掉一个标签：先备份 annotations.json / classes.txt，
    /// 再删掉这个类的所有框、把类别从 classes.txt 里去掉（后面的 id 依次前移），
    /// 最后重写所有图片的标签文件。当前模型文件不动，所以重训之前它还会报这个类。
    /// </summary>
    public ClassRemovalResult RemoveClass(string en)
    {
        var target = (en ?? "").Trim();
        int idx = _classes.FindIndex(c => string.Equals(c, target, StringComparison.OrdinalIgnoreCase));
        if (idx < 0) throw new InvalidOperationException("标签不存在：" + target);

        var cls = _classes[idx];
        string cn;
        try { cn = _labels.Display(cls, idx); } catch { cn = cls; }

        var backup = BackupAnnotationFiles(cls);

        int imgCount = _boxes.Values.Count(v => v.Any(b => string.Equals(b.En, cls, StringComparison.OrdinalIgnoreCase)));
        int boxCount = 0;
        foreach (var list in _boxes.Values)
            boxCount += list.RemoveAll(b => string.Equals(b.En, cls, StringComparison.OrdinalIgnoreCase));

        _classes.RemoveAt(idx);
        _classesDirty = true;
        Save();
        Log.Info($"删除标签：{cls}（去掉 {boxCount} 个框 / {imgCount} 张图），备份 {backup}");

        // id 变了，所有导出过的图片都要重写标签；删完没框的图直接移出数据集
        foreach (var key in _boxes.Keys.ToList())
        {
            if (_boxes[key].Any(Exportable) && File.Exists(key)) SyncImage(key);
            else RemoveExported(key);
        }
        Save();

        return new ClassRemovalResult(cls, cn, boxCount, imgCount, backup);
    }

    /// <summary>
    /// 给一个类别改名：存储名（训练用 ASCII）和显示名都可以换。
    /// 先备份 annotations.json / classes.txt，再同步 classes.txt、所有标注框的 En/Cn、labels_map.json，
    /// 最后重写带这个类的图片的 YOLO 标签。当前模型文件不动，重训后模型里的类名才会跟着变。
    /// </summary>
    public ClassRenameResult RenameClass(string en, string newEn, string newDisplayName = null)
    {
        var target = (en ?? "").Trim();
        int idx = _classes.FindIndex(c => string.Equals(c, target, StringComparison.OrdinalIgnoreCase));
        if (idx < 0) throw new InvalidOperationException("数据集里没有这个类别：" + target);

        var oldEn = _classes[idx];
        var to = (newEn ?? "").Trim();
        if (to.Length == 0) throw new ArgumentException("新的存储名不能为空", nameof(newEn));
        if (!IsStorageName(to))
            throw new InvalidOperationException("存储名只能用英文字母、数字、下划线或连字符（训练用的类名必须是 ASCII）。");

        bool same = string.Equals(oldEn, to, StringComparison.OrdinalIgnoreCase);
        if (!same)
        {
            int clash = _classes.FindIndex(c => string.Equals(c, to, StringComparison.OrdinalIgnoreCase));
            if (clash >= 0) throw new InvalidOperationException("存储名“" + to + "”已经被另一个类别占用（序号 " + clash + "）。");
            if (_labels.HasEnglish(to)) throw new InvalidOperationException("显示名映射里已经有“" + to + "”，换一个存储名。");
        }

        string cn;
        try { cn = _labels.Display(oldEn, idx); } catch { cn = oldEn; }
        if (!string.IsNullOrWhiteSpace(newDisplayName)) cn = newDisplayName.Trim();

        var backup = BackupAnnotationFiles(oldEn);

        int boxCount = 0;
        int imgCount = 0;
        foreach (var kv in _boxes)
        {
            bool touched = false;
            foreach (var b in kv.Value)
            {
                if (!string.Equals(b.En, oldEn, StringComparison.OrdinalIgnoreCase)) continue;
                b.En = to;
                b.Cn = cn;
                boxCount++;
                touched = true;
            }
            if (touched) imgCount++;
        }

        _classes[idx] = to;
        _classesDirty = true;

        _labels.RenameEnglish(oldEn, to, cn);
        _labels.Save();

        Save();

        // 类名（或 id 顺序）可能影响标签文件内容，带这个类的图片全部重写一遍
        foreach (var key in _boxes.Keys.ToList())
        {
            if (_boxes[key].Any(b => string.Equals(b.En, to, StringComparison.OrdinalIgnoreCase)) && File.Exists(key))
                SyncImage(key);
        }
        Save();

        Log.Info($"标签改名：{oldEn} → {to}（显示名 {cn}，{boxCount} 个框 / {imgCount} 张图），备份 {backup}");
        return new ClassRenameResult(oldEn, to, cn, boxCount, imgCount, backup);
    }

    /// <summary>训练用的存储名：ASCII 字母/数字/下划线/连字符。</summary>
    public static bool IsStorageName(string name)
    {
        var s = (name ?? "").Trim();
        if (s.Length == 0) return false;
        foreach (var ch in s)
        {
            if (ch >= 128) return false;
            if (!char.IsLetterOrDigit(ch) && ch != '_' && ch != '-') return false;
        }
        return true;
    }

    /// <summary>删除标签前把 annotations.json / classes.txt 复制一份到 mydataset/backup。</summary>
    private static string BackupAnnotationFiles(string cls)
    {
        var copy = "<没有可备份的文件>";
        try
        {
            Directory.CreateDirectory(AppPaths.DatasetBackupDir);
            var stamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            var safe = new StringBuilder();
            foreach (var ch in cls)
                safe.Append(char.IsLetterOrDigit(ch) && ch < 128 ? ch : '_');
            var tag = safe.ToString().Trim('_');
            if (tag.Length == 0) tag = "class";

            if (File.Exists(AppPaths.AnnotationsFile))
            {
                var dst = UniquePath(Path.Combine(AppPaths.DatasetBackupDir, $"annotations_{stamp}_{tag}.json"));
                File.Copy(AppPaths.AnnotationsFile, dst);
                copy = dst;
            }
            if (File.Exists(AppPaths.ClassesFile))
            {
                var dst = UniquePath(Path.Combine(AppPaths.DatasetBackupDir, $"classes_{stamp}_{tag}.txt"));
                File.Copy(AppPaths.ClassesFile, dst);
            }
        }
        catch (Exception ex)
        {
            Log.Error("备份标注文件失败", ex);
        }
        return copy;
    }

    private static string UniquePath(string path)
    {
        if (!File.Exists(path)) return path;
        var dir = Path.GetDirectoryName(path) ?? ".";
        var stem = Path.GetFileNameWithoutExtension(path);
        var ext = Path.GetExtension(path);
        int i = 1;
        string p;
        do { p = Path.Combine(dir, $"{stem}_{++i}{ext}"); } while (File.Exists(p));
        return p;
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
