using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DeploySharp.Engine;

namespace YoloFool;

/// <summary>
/// 采集 + 识别合并页：同一个画布上既能看图片，也能看摄像头/视频，并支持拍照与录像。
/// 旧的“视频分析”页签已并入这里，标记逻辑仍复用 MainForm.Video.cs。
/// </summary>
public sealed partial class MainForm
{
    private enum CaptureSource { None, Image, Camera, Video }

    private CaptureSource _capSource = CaptureSource.None;


    private bool _snapshotBusy;
    private OpenCvSharp.VideoWriter _recordWriter;
    private string _recordPath = "";
    private int _recordFrames;

    // ---------------------------------------------------------------- layout

    /// <summary>
    /// 识别测试页：工具栏、视频选项、详细设置三块已经在 MainForm.Designer.cs 里建好，
    /// 这里只做运行期绑定（别名、展开状态）；画布 + 结果表下一步再搬进设计器。
    /// </summary>
    private void InitializeCapturePage()
    {
        _btnRun = _btnStartVideo;
        _tbVideoPath = _tbRecImage;
        _btnToggleAdv.Text = _advExpanded ? "▾ 收起设置" : "▸ 详细设置";
        _pnlAdv.Visible = _advExpanded;
        InitializeCaptureBody();
    }

    /// <summary>
    /// 画布 + 结果侧栏已由 MainForm.Designer.cs 建好，这里只做运行期绑定：
    /// 别名（视频页复用同一套控件）、右键菜单、链接按钮的点击。
    /// </summary>
    private void InitializeCaptureBody()
    {
        _picVideo = _picRec;
        _splitVideo = _splitRec;
        _gridVideo = _grid;
        _lblVideoStats = _lblResultStats;

        _picRec.TrackLabel = t => $"#{t.Id} {_labels.Display(t.Item.RawCategory, t.Item.ClassId)} {t.Item.Confidence:P0}";
        _picRec.CanvasMenu = BuildImageContextMenu();
        _picRec.SaveRequested += () => SaveCurrentImage(true);

        Theme.StyleGrid(_grid);
        _btnFilter.Click += (_, _) => EditFilterOptions();
        _btnBlockClass.Click += (_, _) => BlockSelectedClass();
        _btnDeleteResult.Click += (_, _) => RemoveSelectedResult();
        RefreshFilterBadge();
    }


    // ---------------------------------------------------------------- mode

    private bool IsVideoMode =>
        _capSource == CaptureSource.Camera || _capSource == CaptureSource.Video;

    /// <summary>按来源切换列与统计区的可见性，避免图片模式出现“活跃 ID”这种噪声。</summary>
    private void ApplyCaptureLayout()
    {
        if (_grid == null || _grid.Columns.Count == 0) return;

        bool video = IsVideoMode;
        SetColumnVisible("colId", video);
        SetColumnVisible("colArea", video);
        SetColumnVisible("colPct", video);
        SetColumnVisible("colPos", !video);
        SetColumnVisible("colCenter", !video);

        if (_pnlMetrics != null) _pnlMetrics.Visible = video;
        if (_lblVideoArea != null && !video) _lblVideoArea.Text = "";

        if (_btnStartVideo != null)
            _btnStartVideo.Text = video ? "开始分析" : "开始识别";
        if (_btnRecord != null && !video && _recordWriter == null) _btnRecord.Enabled = false;
    }

    private void SetColumnVisible(string name, bool visible)
    {
        if (_grid.Columns[name] is { } col) col.Visible = visible;
    }

    private void SetCaptureSource(CaptureSource source)
    {
        _capSource = source;
        ApplyCaptureLayout();
    }

    // ---------------------------------------------------------------- actions

    /// <summary>
    /// 拍照：像手机相机一样“按下快门就立刻定格在画面上”，全程不写文件。
    /// ① 正在看摄像头/视频：复制当前帧 → 停止采集 → 这一帧直接显示成图片，可接着“开始识别”；
    /// ② 画面上已经是图片/照片：不重复拍，只提示已定格；
    /// ③ 什么都还没有：自己开摄像头抓一张（不需要先点“摄像头”），抓完立刻关掉设备。
    /// </summary>
    private void TakeSnapshot()
    {
        if (_snapshotBusy) return;

        var bitmap = _picRec?.Image as Bitmap;
        bool running = _video != null && _video.IsRunning;

        if (running)
        {
            if (bitmap == null)
            {
                _lblStatus.Text = "画面还没出来，等一秒再拍。";
                return;
            }

            // 必须先复制一帧：StopVideo 会释放画布上的位图
            Bitmap shot;
            try { shot = new Bitmap(bitmap); }
            catch (Exception ex)
            {
                Log.Error("拍照失败", ex);
                _lblStatus.Text = "拍照失败：" + ex.Message;
                return;
            }

            StopVideo(false);
            LoadRecognitionBitmap(shot,
                $"已拍照并定格（{shot.Width}×{shot.Height}），没有保存到本地，点“开始识别”直接识别这张照片。");
            return;
        }

        // 没在跑摄像头/视频时，“拍照”=“现在开摄像头拍一张”，即使画面上已有图片也重新拍，
        // 否则用户会以为按钮没反应（旧版在这里只改一行状态栏文字就返回了）。
        TakeSnapshotFromCameraAsync();
    }

    /// <summary>OpenCV Mat → GDI+ Bitmap（统一转 BGR 后逐行拷贝，兼容非连续 stride）。</summary>
    private static Bitmap MatToBitmap(OpenCvSharp.Mat mat)
    {
        using var bgr = new OpenCvSharp.Mat();
        switch (mat.Channels())
        {
            case 4:
                OpenCvSharp.Cv2.CvtColor(mat, bgr, OpenCvSharp.ColorConversionCodes.BGRA2BGR);
                break;
            case 3:
                mat.CopyTo(bgr);
                break;
            default:
                OpenCvSharp.Cv2.CvtColor(mat, bgr, OpenCvSharp.ColorConversionCodes.GRAY2BGR);
                break;
        }

        int width = bgr.Width;
        int height = bgr.Height;
        var bmp = new Bitmap(width, height, PixelFormat.Format24bppRgb);
        var data = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
        try
        {
            var row = new byte[width * 3];
            for (int y = 0; y < height; y++)
            {
                System.Runtime.InteropServices.Marshal.Copy(bgr.Ptr(y), row, 0, row.Length);
                System.Runtime.InteropServices.Marshal.Copy(row, 0, data.Scan0 + y * data.Stride, row.Length);
            }
        }
        finally
        {
            bmp.UnlockBits(data);
        }
        return bmp;
    }

    /// <summary>没画面时直接开摄像头抓一帧（只进内存），抓完就关；不加载模型，所以很快。</summary>
    private static (Bitmap Shot, string Error) GrabCameraSnapshot()
    {
        try
        {
            var apis = new[]
            {
                OpenCvSharp.VideoCaptureAPIs.DSHOW,
                OpenCvSharp.VideoCaptureAPIs.MSMF,
                OpenCvSharp.VideoCaptureAPIs.ANY,
            };

            string lastError = null;
            foreach (var api in apis)
            {
                OpenCvSharp.VideoCapture cap = null;
                try
                {
                    cap = new OpenCvSharp.VideoCapture(0, api);
                    if (!cap.IsOpened())
                    {
                        lastError = "摄像头打不开（可能被其它程序占用）";
                        continue;
                    }

                    using var frame = new OpenCvSharp.Mat();
                    var deadline = DateTime.UtcNow.AddSeconds(3);
                    while (DateTime.UtcNow < deadline)
                    {
                        if (cap.Read(frame) && !frame.Empty())
                            return (MatToBitmap(frame), null);
                        Thread.Sleep(60);
                    }
                    lastError = "摄像头没有返回画面";
                }
                catch (Exception ex)
                {
                    lastError = ex.Message;
                }
                finally
                {
                    try { cap?.Release(); } catch { }
                    try { cap?.Dispose(); } catch { }
                }
            }
            return (null, lastError ?? "没有找到可用摄像头");
        }
        catch (Exception ex)
        {
            return (null, ex.Message);
        }
    }

    private async void TakeSnapshotFromCameraAsync()
    {
        _snapshotBusy = true;
        string oldText = _btnSnapshot?.Text ?? "拍照";
        if (_btnSnapshot != null)
        {
            _btnSnapshot.Enabled = false;
            _btnSnapshot.Text = "拍照中…";
        }
        UseWaitCursor = true;
        // 抓拍期间独占摄像头，避免同时再点“摄像头/开始”抢设备
        if (_btnCamera != null) _btnCamera.Enabled = false;
        if (_btnStartVideo != null) _btnStartVideo.Enabled = false;
        _lblStatus.Text = "正在打开摄像头拍照…";
        try
        {
            var (shot, error) = await Task.Run(GrabCameraSnapshot);
            if (shot == null)
            {
                _lblStatus.Text = "拍照失败：" + (error ?? "没有可用画面");
                MessageBox.Show(this,
                    "没有可拍的画面：" + (error ?? "摄像头不可用") +
                    "\n\n可以先用“打开图片”或“选择视频”，再点“拍照”。",
                    "拍照", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            StopVideo(false);
            LoadRecognitionBitmap(shot,
                $"已从摄像头拍照并定格（{shot.Width}×{shot.Height}），没有保存到本地，点“开始识别”识别这张照片。");
        }
        catch (Exception ex)
        {
            Log.Error("拍照失败", ex);
            _lblStatus.Text = "拍照失败：" + ex.Message;
        }
        finally
        {
            _snapshotBusy = false;
            UseWaitCursor = false;
            if (_btnSnapshot != null)
            {
                _btnSnapshot.Enabled = true;
                _btnSnapshot.Text = oldText;
            }
            if (_btnCamera != null) _btnCamera.Enabled = true;
            if (_btnStartVideo != null) _btnStartVideo.Enabled = !(_video != null && _video.IsRunning);
        }
    }

    /// <summary>开始/停止录像。录的是画面上看到的帧（已带框/掩码），第一帧到达时才真正打开文件。</summary>
    private void ToggleRecording()
    {
        if (_recordWriter != null || _recordPath.Length > 0)
        {
            StopRecording(true);
            return;
        }
        if (_video == null || !_video.IsRunning)
        {
            _lblStatus.Text = "先开始摄像头或视频，再录制。";
            return;
        }

        var dir = AppPaths.CaptureDir;
        Directory.CreateDirectory(dir);
        _recordPath = Path.Combine(dir, $"rec-{DateTime.Now:yyyyMMdd-HHmmss}.avi");
        _recordFrames = 0;
        _btnRecord.Text = "■ 停止录制";
        _lblStatus.Text = "正在录制…";
    }

    private void WriteRecordFrame(Bitmap bitmap)
    {
        if (string.IsNullOrEmpty(_recordPath) || bitmap == null) return;
        try
        {
            if (_recordWriter == null)
            {
                _recordWriter = new OpenCvSharp.VideoWriter(
                    _recordPath, OpenCvSharp.FourCC.MJPG, 25.0,
                    new OpenCvSharp.Size(bitmap.Width, bitmap.Height));
            }
            using var mat = ToMat(bitmap);
            _recordWriter.Write(mat);
            _recordFrames++;
        }
        catch (Exception ex)
        {
            Log.Error("录像写入失败（已停止录制）", ex);
            StopRecording(false);
            _lblStatus.Text = "录像失败：" + ex.Message;
        }
    }

    private void StopRecording(bool announce)
    {
        var writer = _recordWriter;
        _recordWriter = null;
        var path = _recordPath;
        _recordPath = "";
        try
        {
            writer?.Release();
            writer?.Dispose();
        }
        catch { }

        if (_btnRecord != null) _btnRecord.Text = "● 录制";
        if (announce)
        {
            _lblStatus.Text = string.IsNullOrEmpty(path)
                ? "已停止录制"
                : $"录像已保存：{Path.GetFileName(path)}（{_recordFrames} 帧）";
        }
        _recordFrames = 0;
    }

    private static OpenCvSharp.Mat ToMat(Bitmap bitmap)
    {
        var rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
        var data = bitmap.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
        try
        {
            using var view = OpenCvSharp.Mat.FromPixelData(
                bitmap.Height, bitmap.Width, OpenCvSharp.MatType.CV_8UC3, data.Scan0, data.Stride);
            return view.Clone();
        }
        finally
        {
            bitmap.UnlockBits(data);
        }
    }

    /// <summary>主按钮：图片模式=识别，摄像头/视频模式=开始分析。</summary>
    private async void StartPrimaryAction()
    {
        if (_capSource == CaptureSource.Image || (!IsVideoMode && !string.IsNullOrEmpty(_recPath)))
        {
            await RunRecognitionAsync();
            return;
        }
        if (!IsVideoMode && string.IsNullOrEmpty(_tbRecImage.Text))
        {
            SelectRecognizeImage();
            if (string.IsNullOrEmpty(_recPath)) return;
            await RunRecognitionAsync();
            return;
        }
        StartVideo(null);
    }

    /// <summary>画布右键菜单：保存原图 / 保存画面（带识别框）/ 复制 / 适应窗口。</summary>
    private ContextMenuStrip BuildImageContextMenu() =>
        BuildImageContextMenu(_picRec, CurrentSourceBitmap, text =>
        {
            if (_lblStatus != null) _lblStatus.Text = text;
        }, () => _recPath);

    /// <summary>同一套右键菜单，换一个画布也能用（识别测试页 / 针床检测页）。</summary>
    /// <param name="busy">保存对话框开/关时回调：针床页用来暂停实时预览。</param>
    private ContextMenuStrip BuildImageContextMenu(ImageCanvas canvas, Func<Bitmap> source,
        Action<string> status, Func<string> pathProvider = null, Action<bool> busy = null)
    {
        var menu = new ContextMenuStrip { Font = Theme.Body, ShowImageMargin = false };
        menu.Items.Add(new ToolStripMenuItem("保存图片…（原图）", null,
            (_, _) => SaveImageAs(canvas, source, true, status, pathProvider?.Invoke(), busy))
        {
            ShortcutKeyDisplayString = "Ctrl+S",
        });
        menu.Items.Add(new ToolStripMenuItem("保存画面…（带识别框）", null,
            (_, _) => SaveImageAs(canvas, null, false, status, null, busy)));
        menu.Items.Add(new ToolStripSeparator());
        menu.Items.Add(new ToolStripMenuItem("复制图片", null,
            (_, _) => CopyImageAs(source?.Invoke(), status)));
        menu.Items.Add(new ToolStripMenuItem("适应窗口", null, (_, _) => canvas?.Fit()));
        menu.Opening += (_, e) => e.Cancel = source?.Invoke() == null && canvas?.Image == null;
        return menu;
    }

    /// <summary>当前画布对应的源图：优先内存位图（拍照/识别结果），其次画布上显示的帧。</summary>
    private Bitmap CurrentSourceBitmap()
    {
        if (Usable(_recBitmap)) return _recBitmap;
        var shown = _picRec?.Image;
        return Usable(shown) ? shown : null;
    }

    /// <summary>Bitmap 没有 IsDisposed，访问已释放对象的 Width 会抛异常，这里兜住。</summary>
    private static bool Usable(Bitmap bitmap)
    {
        if (bitmap == null) return false;
        try { return bitmap.Width > 0 && bitmap.Height > 0; }
        catch { return false; }
    }

    /// <summary>plain=true 存原图；false 存画布所见（带框/掩码/标签）。</summary>
    private void SaveCurrentImage(bool plain) =>
        SaveImageAs(_picRec, CurrentSourceBitmap, plain, text =>
        {
            if (_lblStatus != null) _lblStatus.Text = text;
        }, _recPath);

    /// <summary>同一套保存逻辑，给不同的画布用（识别测试页 / 针床检测页）。</summary>
    /// <param name="busy">对话框打开/关闭时回调：针床页用来暂停实时预览。</param>
    private void SaveImageAs(ImageCanvas canvas, Func<Bitmap> source, bool plain, Action<string> status,
        string sourcePath = null, Action<bool> busy = null)
    {
        Bitmap shot = null;
        try
        {
            Log.Info($"保存图片：进入 page={canvas?.Name ?? "null"} plain={plain}");
            // 源图可能就是画布上正在显示的那一张：针床页实时预览每 66ms 换帧并释放旧帧，
            // 而模态对话框期间定时器照跳，所以必须先克隆一份自己持有，否则会被换帧释放掉。
            var src = plain ? source?.Invoke() : RenderCanvasBitmap(canvas);
            if (src == null)
            {
                Log.Info("保存图片：没有可用图片");
                MessageBox.Show(this, "当前画面上还没有图片。", "保存图片",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            shot = CloneBitmap(src);
            if (!plain) src.Dispose();
            if (shot == null)
            {
                Log.Warn("保存图片：位图克隆失败，源图可能已被释放");
                MessageBox.Show(this, "保存失败：画面图片已失效，请稍后再试。", "保存图片",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            Log.Info($"保存图片：已取图 {shot.Width}×{shot.Height}");

            using var dialog = new SaveFileDialog
            {
                Title = plain ? "保存图片" : "保存画面",
                Filter = "PNG 图片|*.png|JPEG 图片|*.jpg;*.jpeg|BMP 图片|*.bmp",
                FilterIndex = 1,
                AddExtension = true,
                DefaultExt = "png",
                FileName = DefaultImageFileName(plain, sourcePath),
                InitialDirectory = Directory.Exists(AppPaths.CaptureDir) ? AppPaths.CaptureDir : AppPaths.Root,
            };
            Log.Info("保存图片：打开另存为对话框");
            DialogResult result;
            if (busy != null) busy(true);
            try
            {
                result = dialog.ShowDialog(this);
            }
            finally
            {
                if (busy != null) busy(false);
            }
            Log.Info($"保存图片：对话框返回 {result}");
            if (result != DialogResult.OK) return;
            SaveBitmapTo(shot, dialog.FileName);
            Log.Info("保存图片：已写盘 " + dialog.FileName);
            status?.Invoke("已保存：" + Path.GetFileName(dialog.FileName));
        }
        catch (Exception ex)
        {
            Log.Error("保存图片失败", ex);
            MessageBox.Show(this, "保存失败：" + ex.Message, "保存图片",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        finally
        {
            shot?.Dispose();
        }
    }

    /// <summary>独立克隆一张位图，和源图彻底解耦；源图已释放时返回 null 而不是抛异常。</summary>
    private static Bitmap CloneBitmap(Bitmap bitmap)
    {
        if (bitmap == null) return null;
        try
        {
            return new Bitmap(bitmap);
        }
        catch (Exception ex)
        {
            Log.Error("克隆图片失败", ex);
            return null;
        }
    }

    private Bitmap RenderCanvasBitmap(ImageCanvas canvas)
    {
        if (canvas == null || canvas.Width <= 0 || canvas.Height <= 0) return null;
        var bmp = new Bitmap(canvas.Width, canvas.Height);
        try
        {
            canvas.DrawToBitmap(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));
            return bmp;
        }
        catch (Exception ex)
        {
            bmp.Dispose();
            Log.Error("渲染画面失败", ex);
            return null;
        }
    }

    private string DefaultImageFileName(bool plain, string path)
    {
        var stamp = DateTime.Now.ToString("yyyyMMdd-HHmmss");
        if (plain && !string.IsNullOrEmpty(path) && File.Exists(path))
            return Path.GetFileNameWithoutExtension(path) + "-" + stamp + Path.GetExtension(path);
        return (plain ? "图片-" : "画面-") + stamp + ".png";
    }

    private static void SaveBitmapTo(Bitmap bitmap, string path)
    {
        var ext = (Path.GetExtension(path) ?? "").ToLowerInvariant();
        var format = ext == ".jpg" || ext == ".jpeg" ? ImageFormat.Jpeg
            : ext == ".bmp" ? ImageFormat.Bmp
            : ImageFormat.Png;
        using var copy = new Bitmap(bitmap);
        copy.Save(path, format);
    }

    private void CopyCurrentImage() =>
        CopyImageAs(CurrentSourceBitmap(), text =>
        {
            if (_lblStatus != null) _lblStatus.Text = text;
        });

    private void CopyImageAs(Bitmap src, Action<string> status)
    {
        if (src == null) return;
        try
        {
            using var copy = new Bitmap(src);
            Clipboard.SetImage(copy);
            status?.Invoke("图片已复制到剪贴板");
        }
        catch (Exception ex)
        {
            Log.Error("复制图片失败", ex);
            MessageBox.Show(this, "复制失败：" + ex.Message, "复制图片",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
