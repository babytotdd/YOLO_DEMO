using DeploySharp.Data;
using DeploySharp.Engine;
using DeploySharp.Model;
using OpenCvSharp;

string baseDir = @"H:\Desktop\YOLO_DEMO\assets";
string modelPath = Path.Combine(baseDir, "yolov8n.onnx");
string imagePath = Path.Combine(baseDir, "bus.jpg");
string outPath = Path.Combine(baseDir, "result.jpg");

Console.WriteLine("model exists: " + File.Exists(modelPath) + " | image exists: " + File.Exists(imagePath));

var config = new Yolov8DetConfig(modelPath);
config.SetTargetInferenceBackend(InferenceBackend.OnnxRuntime);
using var model = new Yolov8DetModel(config);

using Mat img = Cv2.ImRead(imagePath);
Console.WriteLine("image size: " + img.Width + "x" + img.Height);

Result[] result = model.Predict(img);
Console.WriteLine("detections: " + result.Length);
foreach (var r in result)
{
    var det = (DetResult)r;
    Console.WriteLine($"  category={det.Category}  confidence={det.Confidence:F3}  box=({det.Bounds.X},{det.Bounds.Y},{det.Bounds.Width},{det.Bounds.Height})");
}

using Mat resultImg = Visualize.DrawDetResult(result, img, new VisualizeOptions(1.0f));
Cv2.ImWrite(outPath, resultImg);
Console.WriteLine("saved: " + outPath);
model.ModelInferenceProfiler.PrintStatistics();
