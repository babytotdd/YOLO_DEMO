using DeploySharp.Data;
using DeploySharp.Engine;
using DeploySharp.Model;
using OpenCvSharp;
using Rect = DeploySharp.Data.Rect;

namespace YoloFool;

internal enum ModelKind { YOLOv8Det, YOLOv11Det, YOLOv8Seg }

internal sealed record ResultInfo(string Category, float Confidence, Rect Bounds, int ImageW, int ImageH);

internal sealed class InferenceService : IDisposable
{
    private IModel _model;
    private ModelKind _kind;

    public bool Loaded => _model != null;

    public void Load(string path, ModelKind kind, InferenceBackend engine, DeviceType device, OnnxRuntimeDeviceType provider, double conf, double nms)
    {
        if (!File.Exists(path)) throw new FileNotFoundException("模型文件不存在", path);
        Dispose();
        switch (kind)
        {
            case ModelKind.YOLOv8Det:
                {
                    var c = new Yolov8DetConfig(path, engine, device);
                    c.SetTargetOnnxRuntimeDeviceType(provider);
                    c.ConfidenceThreshold = (float)conf; c.NmsThreshold = (float)nms;
                    _model = new Yolov8DetModel(c);
                    break;
                }
            case ModelKind.YOLOv11Det:
                {
                    var c = new Yolov11DetConfig(path, engine, device);
                    c.SetTargetOnnxRuntimeDeviceType(provider);
                    c.ConfidenceThreshold = (float)conf; c.NmsThreshold = (float)nms;
                    _model = new Yolov11DetModel(c);
                    break;
                }
            default:
                {
                    var c = new Yolov8SegConfig(path, engine, device);
                    c.SetTargetOnnxRuntimeDeviceType(provider);
                    c.ConfidenceThreshold = (float)conf; c.NmsThreshold = (float)nms;
                    _model = new Yolov8SegModel(c);
                    break;
                }
        }
        _kind = kind;
    }

    public (List<ResultInfo> Items, Mat Drawn) Predict(string imagePath)
    {
        using var img = Cv2.ImRead(imagePath);
        if (img.Empty()) throw new InvalidOperationException("无法读取图片: " + imagePath);
        Result[] raw = _model.Predict(img);
        var items = new List<ResultInfo>();
        foreach (DetResult d in raw)
            items.Add(new ResultInfo(d.Category, d.Confidence, d.Bounds, img.Width, img.Height));
        Mat drawn = _kind == ModelKind.YOLOv8Seg
            ? Visualize.DrawSegResult(raw, img, new VisualizeOptions(1.0f))
            : Visualize.DrawDetResult(raw, img, new VisualizeOptions(1.0f));
        return (items, drawn);
    }

    public void Dispose()
    {
        try { (_model as IDisposable)?.Dispose(); } catch { }
        _model = null;
    }
}
