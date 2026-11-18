using System.Diagnostics;
using DeploySharp.Data;
using DeploySharp.Engine;
using DeploySharp.Model;
using OpenCvSharp;

internal static class Program
{
    private static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        var modelPath = args.Length > 0 ? args[0] : @"H:\Desktop\YOLO_DEMO\demo\models\yolov8n.onnx";
        var imgPath = @"H:\Desktop\YOLO_DEMO\demo\images\bus.jpg";
        foreach (var (name, dev, prov) in new[]
        {
            ("CPU/Default", DeviceType.CPU, OnnxRuntimeDeviceType.Default),
            ("GPU0/DML", DeviceType.GPU0, OnnxRuntimeDeviceType.DML),
            ("CPU/Dnnl", DeviceType.CPU, OnnxRuntimeDeviceType.Dnnl),
        })
        {
            try
            {
                var cfg = new Yolov8DetConfig(modelPath, InferenceBackend.OnnxRuntime, dev);
                cfg.SetTargetOnnxRuntimeDeviceType(prov);
                cfg.ConfidenceThreshold = 0.5f;
                cfg.NmsThreshold = 0.5f;
                using var model = new Yolov8DetModel(cfg);
                using var img = Cv2.ImRead(imgPath);
                var first = Stopwatch.StartNew();
                var res = model.Predict(img);
                first.Stop();
                var times = new List<long>();
                for (int i = 0; i < 5; i++)
                {
                    var sw = Stopwatch.StartNew();
                    model.Predict(img);
                    sw.Stop();
                    times.Add(sw.ElapsedMilliseconds);
                }
                Console.WriteLine($"RESULT {name}: n={res.Length} first={first.ElapsedMilliseconds}ms warm=[{string.Join(",", times)}] avg={times.Average():F1}");
                foreach (var r in res.OrderByDescending(x => x.Confidence).Take(4))
                    Console.WriteLine($"   {r.Category} {r.Confidence:F3} ({r.Bounds.X},{r.Bounds.Y},{r.Bounds.Width},{r.Bounds.Height})");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"RESULT {name}: FAILED {ex.GetType().Name}: {ex.Message}");
            }
        }
    }
}
