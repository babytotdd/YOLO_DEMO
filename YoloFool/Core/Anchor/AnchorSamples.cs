using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Text.Json;
using OpenCvSharp;

namespace YoloFool;

internal sealed record AnchorSampleStat(string Name, int Id, int Count);

/// <summary>样本里的一个框：Rect 是未旋转的基准框，Angle 是绕中心旋转的角度（用于显示与侧车，训练标签取水平外接框）。</summary>
internal sealed record AnchorLabelBox(int ClassId, string ClassName, Rectangle Rect, float Angle);

/// <summary>样本列表里的一行。</summary>
internal sealed record AnchorSampleEntry(string Stem, string ImagePath, string Classes, int BoxCount, DateTime Time);

/// <summary>
/// 页内“识别目标”的样本库：在画面上框一次，就把这块小图存一张、再写一行 YOLO 标注。
/// 训练时把 samples 拼成 dataset（images/train、images/val、labels/...、data.yaml），
/// 交给现有 TrainingService，训练完把 mymodel.onnx 复制成 fixtures_v2/&lt;机种&gt;/model.onnx。
/// </summary>
internal sealed class AnchorSampleStore
{
    public AnchorSampleStore(string model)
    {
        Model = model ?? "";
        Root = Path.Combine(AnchorStore.Dir(Model), "samples");
    }

    public string Model { get; }

    public string Root { get; }

    public string ImagesDir => Path.Combine(Root, "images");

    public string LabelsDir => Path.Combine(Root, "labels");

    public string ClassesFile => Path.Combine(Root, "classes.txt");

    public string DatasetDir => Path.Combine(Root, "dataset");

    /// <summary>标签表：一行一个名字，行号就是类别 id。</summary>
    public List<string> ReadClasses()
    {
        var list = new List<string>();
        if (!File.Exists(ClassesFile)) return list;
        foreach (var line in File.ReadAllLines(ClassesFile, Encoding.UTF8))
        {
            var text = (line ?? "").Trim();
            if (text.Length > 0) list.Add(text);
        }
        return list;
    }

    private void WriteClasses(IReadOnlyList<string> classes)
    {
        Directory.CreateDirectory(Root);
        File.WriteAllLines(ClassesFile, classes, new UTF8Encoding(false));
    }

    public int ClassId(string name)
    {
        var classes = ReadClasses();
        for (var i = 0; i < classes.Count; i++)
            if (string.Equals(classes[i], name, StringComparison.OrdinalIgnoreCase)) return i;
        return -1;
    }

    /// <summary>取类别 id，没有就追加到标签表末尾。</summary>
    public int EnsureClass(string name)
    {
        var text = (name ?? "").Trim();
        if (text.Length == 0) throw new ArgumentException("类别名不能为空", nameof(name));
        var classes = ReadClasses();
        for (var i = 0; i < classes.Count; i++)
            if (string.Equals(classes[i], text, StringComparison.OrdinalIgnoreCase)) return i;
        classes.Add(text);
        WriteClasses(classes);
        return classes.Count - 1;
    }

    public string NextFileStem()
    {
        Directory.CreateDirectory(ImagesDir);
        var index = 1;
        while (File.Exists(Path.Combine(ImagesDir, "sam_" + index.ToString("0000") + ".jpg"))) index++;
        return "sam_" + index.ToString("0000");
    }

    /// <summary>把画面里的一块区域存成样本（jpg + YOLO 归一化标注），返回图片路径。</summary>
    public string SaveSample(Mat frame, Rect rect, int classId)
    {
        if (frame == null || frame.Empty()) throw new InvalidOperationException("没有可用画面");
        var x = Math.Clamp(rect.X, 0, Math.Max(0, frame.Width - 4));
        var y = Math.Clamp(rect.Y, 0, Math.Max(0, frame.Height - 4));
        var w = Math.Clamp(rect.Width, 4, frame.Width - x);
        var h = Math.Clamp(rect.Height, 4, frame.Height - y);

        Directory.CreateDirectory(ImagesDir);
        Directory.CreateDirectory(LabelsDir);

        var stem = NextFileStem();
        var imgFile = Path.Combine(ImagesDir, stem + ".jpg");
        using (var crop = new Mat(frame, new Rect(x, y, w, h)))
        {
            Cv2.ImWrite(imgFile, crop);
        }

        var cx = (x + w / 2.0) / frame.Width;
        var cy = (y + h / 2.0) / frame.Height;
        var nw = w / (double)frame.Width;
        var nh = h / (double)frame.Height;
        var line = string.Format(
            CultureInfo.InvariantCulture,
            "{0} {1:0.######} {2:0.######} {3:0.######} {4:0.######}",
            classId, cx, cy, nw, nh);
        File.WriteAllText(Path.Combine(LabelsDir, stem + ".txt"), line + Environment.NewLine, new UTF8Encoding(false));
        return imgFile;
    }

    public IReadOnlyList<string> LabeledImages()
    {
        var list = new List<string>();
        if (!Directory.Exists(ImagesDir)) return list;
        foreach (var img in Directory.GetFiles(ImagesDir, "*.jpg").OrderBy(f => f, StringComparer.Ordinal))
        {
            var label = Path.Combine(LabelsDir, Path.GetFileNameWithoutExtension(img) + ".txt");
            if (File.Exists(label)) list.Add(img);
        }
        return list;
    }

    public List<AnchorSampleStat> Stats()
    {
        var classes = ReadClasses();
        var counts = new int[classes.Count];
        foreach (var label in Directory.Exists(LabelsDir) ? Directory.GetFiles(LabelsDir, "*.txt") : Array.Empty<string>())
        {
            foreach (var line in File.ReadLines(label))
            {
                var parts = (line ?? "").Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length < 5 || !int.TryParse(parts[0], out var id)) continue;
                if (id >= 0 && id < counts.Length) counts[id]++;
            }
        }

        var result = new List<AnchorSampleStat>();
        for (var i = 0; i < classes.Count; i++) result.Add(new AnchorSampleStat(classes[i], i, counts[i]));
        return result;
    }

    /// <summary>拼数据集。只有 1 张也能拼（同一张同时进 train 与 val），样本少只影响效果、不再拦训练。</summary>
    public TrainingSetInfo BuildDataset()
    {
        var classes = ReadClasses();
        if (classes.Count == 0 || classes.All(c => string.IsNullOrWhiteSpace(c)))
            throw new InvalidOperationException("还没有识别目标：先在画面上框一块特征并命名（例如 wifi）。");

        var pairs = LabeledImages();
        if (pairs.Count == 0)
            throw new InvalidOperationException("还没有带框的照片：先点上方【拍照】，再在照片上框选目标。");

        var imgTrain = Path.Combine(DatasetDir, "images", "train");
        var imgVal = Path.Combine(DatasetDir, "images", "val");
        var lblTrain = Path.Combine(DatasetDir, "labels", "train");
        var lblVal = Path.Combine(DatasetDir, "labels", "val");
        foreach (var dir in new[] { imgTrain, imgVal, lblTrain, lblVal })
        {
            if (Directory.Exists(dir)) Directory.Delete(dir, true);
            Directory.CreateDirectory(dir);
        }

        var trainCount = 0;
        var valCount = 0;
        if (pairs.Count == 1)
        {
            // 单张样本：同一张同时作为训练和验证，保证 YOLO 的 val 目录不为空。
            var only = Path.GetFileName(pairs[0]);
            var onlyLabel = Path.Combine(LabelsDir, Path.GetFileNameWithoutExtension(only) + ".txt");
            File.Copy(pairs[0], Path.Combine(imgTrain, only), true);
            File.Copy(onlyLabel, Path.Combine(lblTrain, only.Replace(".jpg", ".txt")), true);
            File.Copy(pairs[0], Path.Combine(imgVal, only), true);
            File.Copy(onlyLabel, Path.Combine(lblVal, only.Replace(".jpg", ".txt")), true);
            trainCount = 1;
            valCount = 1;
        }
        var valEvery = pairs.Count >= 12 ? 6 : 2;
        for (var i = 0; i < pairs.Count && pairs.Count > 1; i++)
        {
            var isVal = i % valEvery == valEvery - 1;
            var name = Path.GetFileName(pairs[i]);
            var label = Path.Combine(LabelsDir, Path.GetFileNameWithoutExtension(name) + ".txt");
            File.Copy(pairs[i], Path.Combine(isVal ? imgVal : imgTrain, name), true);
            File.Copy(label, Path.Combine(isVal ? lblVal : lblTrain, name.Replace(".jpg", ".txt")), true);
            if (isVal) valCount++;
            else trainCount++;
        }

        var yaml = Path.Combine(DatasetDir, "data.yaml");
        var sb = new StringBuilder();
        sb.AppendLine("path: " + DatasetDir.Replace('\\', '/'));
        sb.AppendLine("train: images/train");
        sb.AppendLine("val: images/val");
        sb.AppendLine("nc: " + classes.Count);
        sb.AppendLine("names: [" + string.Join(", ", classes.Select(Quote)) + "]");
        File.WriteAllText(yaml, sb.ToString(), new UTF8Encoding(false));
        return new TrainingSetInfo(yaml, trainCount, valCount, classes.Count, classes);
    }

    /// <summary>把整帧画面连同多个框一起存成一张样本，返回样本编号（sam_0001）。</summary>
    public string SaveFrame(Mat frame, IReadOnlyList<AnchorLabelBox> boxes)
    {
        if (frame == null || frame.Empty()) throw new InvalidOperationException("没有可用画面");
        if (boxes == null || boxes.Count == 0) throw new InvalidOperationException("还没有框：先在画面上框一个目标");
        Directory.CreateDirectory(ImagesDir);
        Directory.CreateDirectory(LabelsDir);
        var stem = NextFileStem();
        Cv2.ImWrite(Path.Combine(ImagesDir, stem + ".jpg"), frame);
        WriteLabels(stem, frame.Width, frame.Height, boxes);
        return stem;
    }

    /// <summary>只把整帧画面存成一张照片样本（还没有框），框选后再用 UpdateFrame 写标注。</summary>
    public string SavePhoto(Mat frame)
    {
        if (frame == null || frame.Empty()) throw new InvalidOperationException("没有可用画面");
        Directory.CreateDirectory(ImagesDir);
        Directory.CreateDirectory(LabelsDir);
        var stem = NextFileStem();
        Cv2.ImWrite(Path.Combine(ImagesDir, stem + ".jpg"), frame);
        return stem;
    }

    /// <summary>按指定编号把整帧存成照片（拍照还没确认时先占号，框好再真正写盘）。</summary>
    public bool WritePhoto(string stem, Mat frame)
    {
        if (string.IsNullOrEmpty(stem)) return false;
        if (frame == null || frame.Empty()) return false;
        Directory.CreateDirectory(ImagesDir);
        Directory.CreateDirectory(LabelsDir);
        return Cv2.ImWrite(Path.Combine(ImagesDir, stem + ".jpg"), frame);
    }

    /// <summary>改写已有样本的框（画面不换）；框全删掉时只清标注，照片留着。</summary>
    public void UpdateFrame(string stem, int width, int height, IReadOnlyList<AnchorLabelBox> boxes)
    {
        if (string.IsNullOrEmpty(stem)) throw new InvalidOperationException("没有选中样本");
        if (!File.Exists(Path.Combine(ImagesDir, stem + ".jpg"))) throw new InvalidOperationException("样本图不存在：" + stem);
        if (boxes == null || boxes.Count == 0)
        {
            foreach (var file in new[] { Path.Combine(LabelsDir, stem + ".txt"), Path.Combine(LabelsDir, stem + ".json") })
            {
                if (File.Exists(file)) File.Delete(file);
            }
            return;
        }
        WriteLabels(stem, width, height, boxes);
    }

    private void WriteLabels(string stem, int width, int height, IReadOnlyList<AnchorLabelBox> boxes)
    {
        var sb = new StringBuilder();
        var side = new List<object>();
        foreach (var box in boxes)
        {
            var r = box.Rect;
            if (Math.Abs(box.Angle) > 0.01f)
            {
                var aabb = RotatedBox.AxisAligned(r.X, r.Y, r.Width, r.Height, box.Angle);
                r = Rectangle.Round(aabb);
            }
            r = Rectangle.Intersect(r, new Rectangle(0, 0, Math.Max(1, width), Math.Max(1, height)));
            if (r.Width < 4 || r.Height < 4) continue;
            double cx = (r.X + r.Width / 2.0) / width;
            double cy = (r.Y + r.Height / 2.0) / height;
            double nw = r.Width / (double)width;
            double nh = r.Height / (double)height;
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "{0} {1:0.######} {2:0.######} {3:0.######} {4:0.######}", box.ClassId, cx, cy, nw, nh));
            side.Add(new { id = box.ClassId, name = box.ClassName, x = box.Rect.X, y = box.Rect.Y, w = box.Rect.Width, h = box.Rect.Height, a = box.Angle });
        }
        if (sb.Length == 0) throw new InvalidOperationException("框都在画面外：重新框一次");
        File.WriteAllText(Path.Combine(LabelsDir, stem + ".txt"), sb.ToString(), new UTF8Encoding(false));
        File.WriteAllText(Path.Combine(LabelsDir, stem + ".json"), JsonSerializer.Serialize(side), new UTF8Encoding(false));
    }
    /// <summary>读回某张样本的框（归一化坐标按给定尺寸还原成像素）。</summary>
    public List<AnchorLabelBox> ReadLabels(string stem, int width, int height)
    {
        var list = new List<AnchorLabelBox>();
        var txt = Path.Combine(LabelsDir, stem + ".txt");
        if (!File.Exists(txt) || width <= 0 || height <= 0) return list;
        var classes = ReadClasses();
        var angles = ReadSidecar(stem);
        var index = 0;
        foreach (var line in File.ReadAllLines(txt, Encoding.UTF8))
        {
            var parts = (line ?? "").Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 5 || !int.TryParse(parts[0], out var id)) continue;
            if (!double.TryParse(parts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out var cx) ||
                !double.TryParse(parts[2], NumberStyles.Float, CultureInfo.InvariantCulture, out var cy) ||
                !double.TryParse(parts[3], NumberStyles.Float, CultureInfo.InvariantCulture, out var nw) ||
                !double.TryParse(parts[4], NumberStyles.Float, CultureInfo.InvariantCulture, out var nh)) continue;
            var w = Math.Max(2, (int)Math.Round(nw * width));
            var h = Math.Max(2, (int)Math.Round(nh * height));
            var x = (int)Math.Round(cx * width - w / 2.0);
            var y = (int)Math.Round(cy * height - h / 2.0);
            var name = id >= 0 && id < classes.Count ? classes[id] : "?" + id;
            float angle = 0f;
            if (index < angles.Count) angle = angles[index];
            list.Add(new AnchorLabelBox(id, name, new Rectangle(x, y, w, h), angle));
            index++;
        }
        return list;
    }

    private List<float> ReadSidecar(string stem)
    {
        var list = new List<float>();
        var json = Path.Combine(LabelsDir, stem + ".json");
        if (!File.Exists(json)) return list;
        try
        {
            using var doc = JsonDocument.Parse(File.ReadAllText(json));
            if (doc.RootElement.ValueKind != JsonValueKind.Array) return list;
            foreach (var item in doc.RootElement.EnumerateArray())
            {
                list.Add(item.TryGetProperty("a", out var a) && a.TryGetSingle(out var value) ? value : 0f);
            }
        }
        catch { }
        return list;
    }
    /// <summary>最近的样本（按编号倒序），用于列表浏览与预览。</summary>
    public List<AnchorSampleEntry> ListSamples(int max = 200)
    {
        var result = new List<AnchorSampleEntry>();
        if (!Directory.Exists(ImagesDir)) return result;
        var classes = ReadClasses();
        var files = Directory.GetFiles(ImagesDir, "*.jpg").OrderByDescending(f => f, StringComparer.Ordinal).Take(Math.Max(1, max));
        foreach (var img in files)
        {
            var stem = Path.GetFileNameWithoutExtension(img);
            var txt = Path.Combine(LabelsDir, stem + ".txt");
            var names = new List<string>();
            var count = 0;
            if (File.Exists(txt))
            {
                foreach (var line in File.ReadLines(txt))
                {
                    var parts = (line ?? "").Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length < 5 || !int.TryParse(parts[0], out var id)) continue;
                    count++;
                    var name = id >= 0 && id < classes.Count ? classes[id] : "?" + id;
                    if (!names.Contains(name)) names.Add(name);
                }
            }
            DateTime time = File.GetCreationTime(img);
            result.Add(new AnchorSampleEntry(stem, img, count == 0 ? "（未标注）" : string.Join("、", names), count, time));
        }
        return result;
    }

    /// <summary>删掉一张样本（图片 + 标注 + 侧车）。</summary>
    public bool DeleteSample(string stem)
    {
        if (string.IsNullOrWhiteSpace(stem)) return false;
        var ok = false;
        foreach (var file in new[]
        {
            Path.Combine(ImagesDir, stem + ".jpg"),
            Path.Combine(LabelsDir, stem + ".txt"),
            Path.Combine(LabelsDir, stem + ".json"),
        })
        {
            try
            {
                if (!File.Exists(file)) continue;
                File.Delete(file);
                ok = true;
            }
            catch { }
        }
        return ok;
    }
    private static string Quote(string text)
    {
        var value = (text ?? "").Replace("\\", "\\\\").Replace("\"", "\\\"");
        return "\"" + value + "\"";
    }
}
