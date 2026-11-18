namespace YoloFool;

internal static class AppPaths
{
    public static readonly string Root = AppContext.BaseDirectory;
    public static readonly string SettingsFile = Path.Combine(Root, "settings.json");
    public static readonly string MyModelsDir = @"H:\Desktop\YOLO_DEMO\my_models";
    public static readonly string MyModelOnnx = Path.Combine(MyModelsDir, "mymodel.onnx");
    public static readonly string DatasetDir = @"H:\Desktop\YOLO_DEMO\mydataset";
    public static readonly string LabelMapFile = Path.Combine(DatasetDir, "labels_map.json");
    public static readonly string ToolsDir = @"H:\Desktop\YOLO_DEMO\tools";
    public static readonly string VenvPython = Path.Combine(ToolsDir, "yolovenv", "Scripts", "python.exe");
    public static readonly string DefaultCocoModel = @"H:\Desktop\YOLO_DEMO\demo\models\yolov8n.onnx";
}
