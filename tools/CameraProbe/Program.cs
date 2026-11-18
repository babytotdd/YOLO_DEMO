using OpenCvSharp;

Console.WriteLine("OpenCV " + Cv2.GetVersionString());
foreach (var api in new[] { VideoCaptureAPIs.ANY, VideoCaptureAPIs.DSHOW, VideoCaptureAPIs.MSMF })
{
    try
    {
        using var cap = new VideoCapture(0, api);
        bool opened = cap.IsOpened();
        string info = "";
        if (opened)
        {
            cap.Set(VideoCaptureProperties.FrameWidth, 1280);
            cap.Set(VideoCaptureProperties.FrameHeight, 720);
            using var frame = new Mat();
            if (cap.Read(frame) && !frame.Empty())
            {
                info = $"{frame.Width}x{frame.Height} fps={cap.Fps:F1}";
                Directory.CreateDirectory(@"H:\Desktop\YOLO_DEMO\samples\camera");
                Cv2.ImWrite(@"H:\Desktop\YOLO_DEMO\samples\camera\csharp_cam0.jpg", frame);
            }
            else info = "opened but read failed";
        }
        Console.WriteLine($"{api}: opened={opened} {info}");
    }
    catch (Exception ex) { Console.WriteLine($"{api}: EX {ex.GetType().Name} {ex.Message}"); }
}
