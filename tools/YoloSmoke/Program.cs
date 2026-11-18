using System.Drawing;
using System.Text;
using DeploySharp.Engine;
using YoloFool;

namespace YoloSmoke;

/// <summary>
/// 无界面冒烟测试：验证本机自检、COCO 推理、相对位置描述、框选自动导出。
/// 运行方式：dotnet run --project tools\YoloSmoke
/// </summary>
internal static class Program
{
    private static int _failures;

    private static void Check(bool ok, string what)
    {
        Console.WriteLine((ok ? "PASS  " : "FAIL  ") + what);
        if (!ok) _failures++;
    }

    private static int Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        AppPaths.EnsureDirectories();
        Console.WriteLine($"workspace = {AppPaths.WorkspaceRoot}");
        Console.WriteLine();

        var labels = LabelCatalog.Load();
        var settings = AppSettings.Load();

        // 1) 本机默认值自检（加载 + 样例推理 + 计时）
        SelfCheckResult check = null;
        try
        {
            check = SelfCheck.Run(settings);
        }
        catch (Exception ex)
        {
            Console.WriteLine("SelfCheck 异常：" + ex);
        }
        Check(check != null && check.Ok, "默认模型自检通过");
        if (check != null)
        {
            Console.WriteLine($"      {check.Message}");
            Console.WriteLine($"      {check.Detail}");
            Console.WriteLine($"      推理 {check.InferenceMs:F1} ms · 类别 {check.Info?.ClassNames?.Count ?? 0}");
            Check(check.Options != null && check.Options.Provider == DeploySharp.Engine.OnnxRuntimeDeviceType.Default,
                "默认后端 CPU/Default");
            check.Service?.Dispose();
        }

        // 2) 真实推理 + 相对位置（需求 5）
        try
        {
            using var svc = new InferenceService();
            svc.Load(check?.Options ?? settings.ToModelOptions());
            var pred = svc.Predict(AppPaths.SampleImage);
            Console.WriteLine($"      样例 {Path.GetFileName(AppPaths.SampleImage)}：{pred.Items.Count} 个目标，{pred.ElapsedMilliseconds:F1} ms");
            foreach (var d in pred.Items.Take(5))
            {
                Console.WriteLine($"        - {labels.Display(d.RawCategory, d.ClassId)}  " +
                                  $"{d.Confidence:P1}  {RelativePosition.Describe(d.CenterXPercent, d.CenterYPercent)}  " +
                                  $"box={d.BoundsText}");
            }
            Check(pred.Items.Count >= 1, "样例图至少识别出 1 个目标");
            Check(pred.Items.All(d => d.ImageWidth > 0 && d.ImageHeight > 0), "结果带原图尺寸（不依赖 DeploySharp 的 0x0 ImageSize）");
            Check(pred.Items.Any(d => !string.IsNullOrWhiteSpace(
                RelativePosition.Describe(d.CenterXPercent, d.CenterYPercent))), "相对位置可读");
        }
        catch (Exception ex)
        {
            Console.WriteLine("推理异常：" + ex);
            Check(false, "样例推理");
        }

        // 3) 中文命名 -> 拼音存储名（只演示，不落盘）
        var preview = LabelCatalog.Load();
        string en = preview.EnsureEnglish("公交车");
        Console.WriteLine($"      “公交车” -> 存储名 {en}");
        Check(en.All(c => c < 128), "中文目标名转换为 ASCII 存储名");

        // 4) 框选后自动导出（需求 4）：加一个框再删掉，验证 images/labels 同步
        var plane = Path.Combine(AppPaths.WorkspaceRoot, "demo", "images", "plane.png");
        if (!File.Exists(plane))
        {
            Console.WriteLine("      跳过：找不到 demo/images/plane.png");
        }
        else
        {
            var store = DatasetStore.Load(labels);
            int before = store.GetBoxes(plane).Count;
            int beforeTotal = store.BoxCount;
            string stem = "plane";
            string labelFileDir = AppPaths.LabelsDir;
            string imageDir = AppPaths.ImagesDir;
            bool hadLabelDir = Directory.Exists(labelFileDir);
            bool hadImageDir = Directory.Exists(imageDir);

            try
            {
                int idx = store.AddBox(plane, "feiji", new Rectangle(20, 20, 90, 70));
                Console.WriteLine($"      AddBox -> index {idx}, boxes={store.BoxCount}");
                Check(store.BoxCount == beforeTotal + 1, "AddBox 后总框数 +1");
                Check(File.Exists(Path.Combine(AppPaths.AnnotationsFile)), "annotations.json 已写入");

                string txt = Path.Combine(labelFileDir, stem + ".txt");
                Check(File.Exists(txt), "YOLO 标签文件已自动生成：" + txt);
                if (File.Exists(txt))
                {
                    var lines = File.ReadAllLines(txt).Where(l => l.Trim().Length > 0).ToArray();
                    Console.WriteLine("      标签内容：" + string.Join(" | ", lines));
                    Check(lines.Length == before + 1, "标签行数与该图框数一致");
                    Check(lines.All(l => l.Split(' ').Length == 5), "每行 5 列：class cx cy w h");
                }

                Check(Directory.Exists(imageDir) && Directory.GetFiles(imageDir).Length > 0, "图片已复制到 mydataset/images");

                store.RemoveBox(plane, idx);
                Check(store.BoxCount == beforeTotal, "RemoveBox 后总框数恢复");
                if (File.Exists(txt))
                    Check(File.ReadAllLines(txt).Count(l => l.Trim().Length > 0) == before, "删除后标签行数恢复");
            }
            catch (Exception ex)
            {
                Console.WriteLine("数据集异常：" + ex);
                Check(false, "框选自动导出");
            }
            finally
            {
                if (!hadLabelDir && Directory.Exists(labelFileDir)) Directory.Delete(labelFileDir, true);
                if (!hadImageDir && Directory.Exists(imageDir)) Directory.Delete(imageDir, true);
            }
        }

        // 5) 训练集视图（data.yaml + train/val 目录）
        try
        {
            var store = DatasetStore.Load(labels);
            var info = store.BuildTrainingView();
            Console.WriteLine($"      training: train={info.TrainCount} val={info.ValCount} classes={info.ClassCount} yaml={info.DataYaml}");
            Check(File.Exists(info.DataYaml), "data.yaml 已生成");
            Check(File.Exists(info.DataYaml) && File.ReadAllText(info.DataYaml).Contains("path:"), "data.yaml 含 path");
        }
        catch (Exception ex)
        {
            Console.WriteLine("训练集视图异常：" + ex);
            Check(false, "训练集视图");
        }

        // 6) 视频：分割掩码 + 跟踪 + 越线计数
        try
        {
            var opts = new ModelOptions(AppPaths.DefaultSegModel, YoloModelKind.YOLOv8Seg,
                InferenceBackend.OnnxRuntime, DeviceType.CPU, OnnxRuntimeDeviceType.Default, 0.35f, 0.5f);
            using var runner = new VideoRunner { FrameStep = 1 };
            runner.Status += msg => Console.WriteLine("      [video] " + msg);
            runner.Error += ex => Console.WriteLine("      [video-error] " + ex);
            runner.Start(Environment.GetEnvironmentVariable("YOLO_VIDEO_SOURCE") ?? AppPaths.DefaultVideo, opts);

            VideoFrame shot = null;
            int frames = 0, maxTracks = 0;
            long maxArea = 0;
            var deadline = DateTime.UtcNow.AddSeconds(35);
            while (DateTime.UtcNow < deadline && frames < 240)
            {
                var f = runner.TakeLatest();
                if (f == null) { Thread.Sleep(50); continue; }
                frames++;
                if (frames == 1)
                    runner.SetLine(new Point(f.Width / 2, (int)(f.Height * 0.15)),
                                   new Point(f.Width / 2, (int)(f.Height * 0.85)));
                maxTracks = Math.Max(maxTracks, f.Tracks.Count);
                maxArea = Math.Max(maxArea, f.MaskAreaPixels);
                if (frames % 40 == 0)
                    Console.WriteLine($"      frame {f.FrameIndex}: tracks={f.Tracks.Count} area={f.MaskAreaPixels}px ({f.MaskAreaPercent:F1}%) A->B={f.AtoB} B->A={f.BtoA} total={f.TotalTracks}");
                if (shot != null) shot.Dispose();
                shot = f;
            }
            runner.Stop();
            Check(frames > 0, "视频帧已产出");
            Check(maxTracks > 0, "跟踪到目标");
            Check(maxArea > 0, "分割掩码面积 > 0");
            if (shot != null)
            {
                Directory.CreateDirectory(AppPaths.LogDir);
                var outPath = Path.Combine(AppPaths.WorkspaceRoot, "docs", "images", "08-video-tracking.png");
                Directory.CreateDirectory(Path.GetDirectoryName(outPath));
                shot.Bitmap.Save(outPath, System.Drawing.Imaging.ImageFormat.Png);
                Console.WriteLine("      视频截图：" + outPath);
                Check(File.Exists(outPath), "视频截图已保存");
                shot.Dispose();
            }
            else Check(false, "视频截图");
        }
        catch (Exception ex)
        {
            Console.WriteLine("视频异常：" + ex);
            Check(false, "视频跟踪/掩码");
        }

        Console.WriteLine();
        Console.WriteLine(_failures == 0 ? "ALL PASS" : $"{_failures} FAILED");
        return _failures == 0 ? 0 : 1;
    }
}
