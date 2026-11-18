using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace YoloFool;

/// <summary>一次自动预标注的结果（用于回显给用户）。</summary>
internal sealed record AutoLabelStats(int Scanned, int Labeled, int Skipped, int Boxes, IReadOnlyList<string> Errors)
{
    public string Summary =>
        $"检查 {Scanned} 张：写入 {Labeled} 张 / {Boxes} 个待复核框，跳过已标注 {Skipped} 张"
        + (Errors.Count > 0 ? $"，失败 {Errors.Count} 张" : "");
}

/// <summary>
/// 半自动标注：用当前模型给“还没有任何标注”的图片预生成检测框。
/// 生成的框是 Auto 状态（待复核）：人工确认前不导出标签、也不进训练集，
/// 这样模型猜错的框不会污染数据集。
/// 训练时模型一直在变，所以正确顺序是“载入新图 → 预标注 → 逐张复核 → 再训练”。
/// 内部自己新建 InferenceService，不动界面正在用的模型；调用方负责放到后台线程。
/// </summary>
internal static class AutoLabeler
{
    /// <summary>预标注置信度：低于它的框不写，避免给复核制造噪音。</summary>
    public const float DefaultConfidence = 0.45f;

    public static AutoLabelStats Run(
        DatasetStore store,
        ModelOptions options,
        IReadOnlyList<string> images,
        Action<int, int> progress = null,
        Func<bool> cancelled = null)
    {
        if (store == null) throw new ArgumentNullException(nameof(store));
        if (options == null) throw new ArgumentNullException(nameof(options));
        images ??= Array.Empty<string>();

        int scanned = 0, labeled = 0, skipped = 0, boxes = 0;
        var errors = new List<string>();

        using var svc = new InferenceService();
        svc.Load(options.With(conf: DefaultConfidence));
        svc.Warmup();

        for (int i = 0; i < images.Count; i++)
        {
            if (cancelled != null && cancelled()) break;

            var path = images[i];
            scanned++;
            progress?.Invoke(i + 1, images.Count);

            // 只补“完全没有标注”的图：已经有人工框的图不混自动框，免得复核时分不清哪来的。
            if (store.GetBoxes(path).Count > 0) { skipped++; continue; }
            if (!File.Exists(path)) { skipped++; continue; }

            try
            {
                var result = svc.Predict(path);
                var items = new List<(string DisplayName, Rectangle Bounds, float Confidence)>();
                foreach (var d in result.Items)
                {
                    if (d.Confidence < DefaultConfidence) continue;

                    var name = store.Labels.Display(d.RawCategory, d.ClassId);
                    if (string.IsNullOrWhiteSpace(name)) name = d.RawCategory;
                    if (string.IsNullOrWhiteSpace(name)) continue;

                    items.Add((name, new Rectangle(d.X, d.Y, d.W, d.H), d.Confidence));
                }

                if (items.Count == 0) continue;
                int added = store.AddAutoBoxes(path, items);
                if (added <= 0) continue;
                labeled++;
                boxes += added;
            }
            catch (Exception ex)
            {
                Log.Warn($"预标注失败：{path} → {ex.Message}");
                if (errors.Count < 20) errors.Add(Path.GetFileName(path) + "：" + ex.Message);
            }
        }

        Log.Info($"自动预标注完成：检查 {scanned} 张，写入 {labeled} 张 / {boxes} 个待复核框，跳过 {skipped} 张，失败 {errors.Count} 张");
        return new AutoLabelStats(scanned, labeled, skipped, boxes, errors);
    }
}
