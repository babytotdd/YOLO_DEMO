using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using DeploySharp.Data;
using DeploySharp.Engine;
using DeploySharp.Model;
using OpenCvSharp;

namespace YoloFool;

/// <summary>
/// DeploySharp 推理封装。
/// 关键改动：新模型先构造成功，再替换旧模型；构造失败不会破坏当前可用模型。
/// </summary>
internal sealed class InferenceService : IDisposable
{
    private IModel _model;
    private ModelRuntimeInfo _info;
    private float _confidence = 0.5f;

    public bool IsLoaded => _model != null;
    public ModelRuntimeInfo Info => _info;

    public ModelRuntimeInfo Load(ModelOptions o)
    {
        if (string.IsNullOrWhiteSpace(o.ModelPath) || !File.Exists(o.ModelPath))
            throw new FileNotFoundException("模型文件不存在", o.ModelPath);

        o = ReconcileKind(o);

        var sw = Stopwatch.StartNew();
        var cfg = BuildConfig(o);
        IModel next = o.Kind switch
        {
            YoloModelKind.YOLOv11Det => new Yolov11DetModel((Yolov11DetConfig)cfg),
            YoloModelKind.YOLOv8Seg => new Yolov8SegModel((Yolov8SegConfig)cfg),
            _ => new Yolov8DetModel((Yolov8DetConfig)cfg),
        };
        sw.Stop();

        var previous = _model;
        _model = next;
        _confidence = o.Confidence;
        _info = new ModelRuntimeInfo(o.ModelPath, o.Kind, o.Provider, o.Device,
            ReadClassNames((YoloConfig)cfg), sw.ElapsedMilliseconds);
        try { (previous as IDisposable)?.Dispose(); } catch { }
        Log.Info($"模型加载成功：{o.ModelPath}（{o.Kind}/{o.Device}/{o.Provider}，{_info.ClassCount} 类，{sw.ElapsedMilliseconds}ms）");
        return _info;
    }

    public void Warmup()
    {
        if (_model == null) return;
        using var gray = new Mat(640, 640, MatType.CV_8UC3, Scalar.All(114));
        _model.Predict(gray);
    }

    public PredictionResult Predict(string imagePath)
    {
        using var img = Cv2.ImRead(imagePath, ImreadModes.Color);
        if (img.Empty()) throw new InvalidOperationException("无法读取图片：" + imagePath);
        return Predict(img);
    }

    public PredictionResult Predict(Mat img)
    {
        if (_model == null) throw new InvalidOperationException("模型尚未加载");
        if (img == null || img.Empty()) throw new InvalidOperationException("图像为空");

        var sw = Stopwatch.StartNew();
        Result[] raw = _model.Predict(img);
        sw.Stop();

        var items = new List<DetectionItem>();
        var masks = new List<SegMaskInfo>();
        foreach (var r in raw)
        {
            if (r is not DetResult d) continue;
            if (d.Confidence < _confidence) continue;
            var b = d.Bounds;
            var item = new DetectionItem(d.Id, d.Category ?? "", d.Confidence,
                b.X, b.Y, b.Width, b.Height, img.Width, img.Height);
            items.Add(item);

            if (r is SegResult seg && seg.Mask != null)
            {
                var bytes = ToMaskBytes(seg.Mask);
                if (bytes != null)
                    masks.Add(new SegMaskInfo(item, bytes, seg.Mask.Width, seg.Mask.Height));
            }
        }
        items.Sort((a, b) => b.Confidence.CompareTo(a.Confidence));
        return new PredictionResult(items, sw.ElapsedMilliseconds, img.Width, img.Height, masks);
    }

    private static byte[] ToMaskBytes(ImageDataF mask)
    {
        try
        {
            int w = mask.Width, h = mask.Height;
            if (w <= 0 || h <= 0) return null;
            var data = mask.GetRawFloatData();
            if (data == null || data.Length < w * h) return null;
            var bytes = new byte[w * h];
            for (int i = 0; i < bytes.Length; i++)
                bytes[i] = data[i] > 0.5f ? (byte)255 : (byte)0;
            return bytes;
        }
        catch (Exception ex)
        {
            Log.Warn("转换分割掩码失败：" + ex.Message);
            return null;
        }
    }

    /// <summary>
    /// 以 ONNX 文件为准纠正模型类型：检测模型被当成分割加载时，seg 后处理拿不到掩码输出，
    /// 会在第一次推理抛 IndexOutOfRangeException（视频页表现为"Index 1 out of range"）。
    /// </summary>
    private static ModelOptions ReconcileKind(ModelOptions o)
    {
        var actual = OnnxInspector.DetectKind(o.ModelPath);
        if (actual == null || actual.Value == o.Kind) return o;

        Log.Warn($"模型实际类型是{OnnxInspector.Describe(actual)}，与设置 {o.Kind} 不一致，已按文件自动纠正：{o.ModelPath}");
        return o.With(kind: actual.Value);
    }

    private static YoloConfig BuildConfig(ModelOptions o)
    {
        YoloConfig cfg = o.Kind switch
        {
            YoloModelKind.YOLOv11Det => new Yolov11DetConfig(o.ModelPath, InferenceBackend.OnnxRuntime, o.Device, o.Confidence, o.Nms),
            YoloModelKind.YOLOv8Seg => new Yolov8SegConfig(o.ModelPath, InferenceBackend.OnnxRuntime, o.Device, o.Confidence, o.Nms),
            _ => new Yolov8DetConfig(o.ModelPath, InferenceBackend.OnnxRuntime, o.Device, o.Confidence, o.Nms),
        };
        cfg.SetTargetOnnxRuntimeDeviceType(o.Provider);
        return cfg;
    }

    private static IReadOnlyList<string> ReadClassNames(YoloConfig cfg)
    {
        try
        {
            var dict = cfg.CategoryDict;
            if (dict == null || dict.Count == 0) return Array.Empty<string>();

            // CategoryDict 的 key 在 0.0.8.1 中是类别号（int）；用 Convert 兼容 int/string 两种形态，
            // 再按 id 直接落到数组下标，避免字符串排序把 "10" 排到 "2" 前面。
            var pairs = dict.Select(kv => (Id: Convert.ToInt32(kv.Key), Name: kv.Value)).ToList();
            int max = pairs.Max(p => p.Id);
            if (max < 0) return Array.Empty<string>();
            var names = new string[max + 1];
            foreach (var p in pairs)
                if (p.Id >= 0 && p.Id < names.Length) names[p.Id] = p.Name;
            return names;
        }
        catch
        {
            return Array.Empty<string>();
        }
    }

    public void Dispose()
    {
        try { (_model as IDisposable)?.Dispose(); } catch { }
        _model = null;
        _info = null;
    }
}
