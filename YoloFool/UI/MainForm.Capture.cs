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

    private Button _btnSnapshot;
    private Button _btnRecord;
    private TableLayoutPanel _pnlMetrics;
    private bool _syncingCapGrid;

    private bool _snapshotBusy;
    private OpenCvSharp.VideoWriter _recordWriter;
    private string _recordPath = "";
    private int _recordFrames;

    // ---------------------------------------------------------------- layout

    private Control BuildCapturePage()
    {
        var root = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 4,
            BackColor = Theme.Bg,
            Padding = new Padding(2, 2, 2, 2),
        };
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

        root.Controls.Add(BuildCaptureToolbar(), 0, 0);
        root.Controls.Add(BuildVideoOptions(), 0, 1);

        _pnlAdv = BuildAdvancedPanel();
        _pnlAdv.Visible = _advExpanded;
        root.Controls.Add(_pnlAdv, 0, 2);

        root.Controls.Add(BuildCaptureBody(), 0, 3);
        return root;
    }

    private Control BuildCaptureToolbar()
    {
        var bar = new TableLayoutPanel
        {
            Dock = DockStyle.Top,
            AutoSize = true,
            ColumnCount = 11,
            RowCount = 1,
            Margin = new Padding(0, 0, 0, 4),
            BackColor = Theme.Bg,
        };
        bar.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        bar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
        for (int i = 0; i < 9; i++) bar.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

        bar.Controls.Add(FieldLabel("来源"), 0, 0);

        _tbRecImage = new TextBox
        {
            Name = "tbRecImage",
            Dock = DockStyle.Fill,
            ReadOnly = true,
            Font = Theme.Body,
            BackColor = Color.White,
            PlaceholderText = "图片路径 / 摄像头 0 / 视频文件",
            Margin = new Padding(0, 4, 8, 4),
        };
        _tbVideoPath = _tbRecImage;
        bar.Controls.Add(_tbRecImage, 1, 0);

        _btnSelectImage = Theme.Ghost("打开图片");
        _btnSelectImage.Name = "btnSelectImage";
        Stretch(_btnSelectImage, 92, 34);
        _btnSelectImage.Anchor = AnchorStyles.Left;
        _btnSelectImage.Margin = new Padding(0, 3, 4, 3);
        bar.Controls.Add(_btnSelectImage, 2, 0);

        _btnCamera = Theme.Ghost("摄像头");
        _btnCamera.Name = "btnCamera";
        Stretch(_btnCamera, 78, 34);
        _btnCamera.Anchor = AnchorStyles.Left;
        _btnCamera.Margin = new Padding(0, 3, 4, 3);
        bar.Controls.Add(_btnCamera, 3, 0);

        _btnPickVideo = Theme.Ghost("选择视频");
        _btnPickVideo.Name = "btnPickVideo";
        Stretch(_btnPickVideo, 92, 34);
        _btnPickVideo.Anchor = AnchorStyles.Left;
        _btnPickVideo.Margin = new Padding(0, 3, 4, 3);
        bar.Controls.Add(_btnPickVideo, 4, 0);

        _btnSnapshot = Theme.Ghost("拍照");
        _btnSnapshot.Name = "btnSnapshot";
        Stretch(_btnSnapshot, 68, 34);
        _btnSnapshot.Anchor = AnchorStyles.Left;
        _btnSnapshot.Margin = new Padding(0, 3, 4, 3);
        bar.Controls.Add(_btnSnapshot, 5, 0);

        _btnRecord = Theme.Ghost("● 录制");
        _btnRecord.Name = "btnRecord";
        Stretch(_btnRecord, 92, 34);
        _btnRecord.Anchor = AnchorStyles.Left;
        _btnRecord.Enabled = false;
        _btnRecord.Margin = new Padding(0, 3, 4, 3);
        bar.Controls.Add(_btnRecord, 6, 0);

        _btnStartVideo = Theme.Primary("开始识别");
        _btnStartVideo.Name = "btnStartVideo";
        Stretch(_btnStartVideo, 104, 36);
        _btnStartVideo.Anchor = AnchorStyles.Left;
        _btnStartVideo.Margin = new Padding(0, 2, 4, 3);
        bar.Controls.Add(_btnStartVideo, 7, 0);
        _btnRun = _btnStartVideo;

        _btnStopVideo = Theme.Ghost("停止");
        _btnStopVideo.Name = "btnStopVideo";
        Stretch(_btnStopVideo, 68, 34);
        _btnStopVideo.Anchor = AnchorStyles.Left;
        _btnStopVideo.Enabled = false;
        _btnStopVideo.Margin = new Padding(0, 2, 4, 3);
        bar.Controls.Add(_btnStopVideo, 8, 0);

        _btnClear = Theme.Ghost("清空");
        _btnClear.Name = "btnClear";
        Stretch(_btnClear, 62, 34);
        _btnClear.Anchor = AnchorStyles.Left;
        _btnClear.Margin = new Padding(0, 2, 4, 3);
        bar.Controls.Add(_btnClear, 9, 0);

        _btnToggleAdv = Theme.Ghost("▸ 详细设置");
        _btnToggleAdv.Name = "btnToggleAdv";
        _btnToggleAdv.Text = _advExpanded ? "▾ 收起设置" : "▸ 详细设置";
        Stretch(_btnToggleAdv, 112, 34);
        _btnToggleAdv.Anchor = AnchorStyles.Left;
        _btnToggleAdv.Margin = new Padding(0, 2, 0, 3);
        bar.Controls.Add(_btnToggleAdv, 10, 0);
        return bar;
    }

    private Control BuildCaptureBody()
    {
        var body = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 1,
            BackColor = Theme.Bg,
        };
        body.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
        body.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

        var split = new SplitContainer
        {
            Dock = DockStyle.Fill,
            Orientation = Orientation.Vertical,
            SplitterWidth = 8,
            // 先给足初始宽度：Panel2MinSize 大于当前 Width 时 WinForms 会抛 InvalidOperationException
            Size = new Size(1000, 600),
            Panel1MinSize = 300,
            Panel2MinSize = 340,
            FixedPanel = FixedPanel.Panel2,
            BackColor = Theme.Border,
            Margin = Padding.Empty,
        };
        _splitRec = split;
        _splitVideo = split;

        var left = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(8) };
        _picRec = new ImageCanvas
        {
            Name = "picRec",
            Dock = DockStyle.Fill,
            Editable = false,
            EmptyText = "① 打开图片 / 摄像头 / 视频\r\n② 点“开始识别”",
            TrackLabel = t => $"#{t.Id} {_labels.Display(t.Item.RawCategory, t.Item.ClassId)} {t.Item.Confidence:P0}",
        };
        _picVideo = _picRec;
        _picRec.CanvasMenu = BuildImageContextMenu();
        _picRec.SaveRequested += () => SaveCurrentImage(true);
        left.Controls.Add(_picRec);
        split.Panel1.Controls.Add(left);
        split.Panel2.Controls.Add(BuildCaptureSidePanel());

        body.Controls.Add(split, 0, 0);
        return body;
    }

    private Control BuildCaptureSidePanel()
    {
        var panel = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White,
            Padding = new Padding(12, 10, 10, 10),
        };
        var t = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 5,
            BackColor = Color.White,
        };
        t.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        t.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        t.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        t.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
        t.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        var head = new TableLayoutPanel
        {
            Dock = DockStyle.Top,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            ColumnCount = 1,
            RowCount = 2,
            Margin = new Padding(0, 0, 0, 2),
        };
        head.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
        head.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        head.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        head.Controls.Add(new Label
        {
            Text = "识别结果",
            Font = Theme.Section,
            ForeColor = Theme.Ink,
            AutoSize = true,
            Margin = new Padding(0, 2, 0, 0),
        }, 0, 0);

        var headTools = new FlowLayoutPanel
        {
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            WrapContents = false,
            Dock = DockStyle.Fill,
            Margin = new Padding(0, 2, 0, 0),
        };
        _lblFilterState = new Label
        {
            Text = "筛除：关",
            AutoSize = true,
            Font = Theme.Small,
            ForeColor = Theme.Muted,
            Margin = new Padding(0, 6, 8, 0),
        };
        headTools.Controls.Add(_lblFilterState);

        _btnFilter = Theme.Link("筛除…");
        _btnFilter.Margin = new Padding(0, 2, 10, 0);
        _btnFilter.Click += (_, _) => EditFilterOptions();
        headTools.Controls.Add(_btnFilter);

        _btnBlockClass = Theme.Link("屏蔽该类");
        _btnBlockClass.Margin = new Padding(0, 2, 10, 0);
        _btnBlockClass.Click += (_, _) => BlockSelectedClass();
        headTools.Controls.Add(_btnBlockClass);

        _btnDeleteResult = Theme.Link("删除");
        _btnDeleteResult.Margin = new Padding(0, 2, 0, 0);
        _btnDeleteResult.Click += (_, _) => RemoveSelectedResult();
        headTools.Controls.Add(_btnDeleteResult);

        head.Controls.Add(headTools, 0, 1);
        t.Controls.Add(head, 0, 0);
        RefreshFilterBadge();

        var status = new TableLayoutPanel
        {
            Dock = DockStyle.Top,
            AutoSize = true,
            ColumnCount = 1,
            RowCount = 2,
            Margin = new Padding(0, 0, 0, 6),
        };
        _lblResultStats = new Label
        {
            Name = "lblResultStats",
            Text = "尚未识别。",
            Font = Theme.Small,
            ForeColor = Theme.Muted,
            AutoSize = true,
            Margin = new Padding(0, 0, 0, 2),
        };
        _lblVideoStats = _lblResultStats;
        _lblVideoArea = new Label
        {
            Name = "lblVideoArea",
            Text = "",
            Font = Theme.Small,
            ForeColor = Theme.Muted,
            AutoSize = true,
            Margin = new Padding(0, 0, 0, 4),
        };
        status.Controls.Add(_lblResultStats, 0, 0);
        status.Controls.Add(_lblVideoArea, 0, 1);
        t.Controls.Add(status, 0, 1);

        _pnlMetrics = new TableLayoutPanel
        {
            Dock = DockStyle.Top,
            AutoSize = true,
            ColumnCount = 4,
            RowCount = 1,
            Margin = new Padding(0, 0, 0, 8),
            Visible = false,
        };
        for (int i = 0; i < 4; i++) _pnlMetrics.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
        _pnlMetrics.Controls.Add(Metric("活跃 ID", out _lblVideoActive), 0, 0);
        _pnlMetrics.Controls.Add(Metric("累计 ID", out _lblVideoTotal), 1, 0);
        _pnlMetrics.Controls.Add(Metric("A→B", out _lblVideoAtoB), 2, 0);
        _pnlMetrics.Controls.Add(Metric("B→A", out _lblVideoBtoA), 3, 0);
        t.Controls.Add(_pnlMetrics, 0, 2);

        _grid = new DataGridView
        {
            Name = "grid",
            Dock = DockStyle.Fill,
            Margin = Padding.Empty,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            AllowUserToResizeRows = false,
            ReadOnly = true,
            RowHeadersVisible = false,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
        };
        Theme.StyleGrid(_grid);
        AddCaptureColumn("colId", "ID", 8, 34);
        AddCaptureColumn("colName", "目标", 24, 56);
        AddCaptureColumn("colConf", "匹配度", 18, 52);
        AddCaptureColumn("colPos", "位置", 30, 78);
        AddCaptureColumn("colCenter", "中心", 20, 64);
        AddCaptureColumn("colArea", "面积", 22, 56);
        AddCaptureColumn("colPct", "占比", 16, 44);
        _gridVideo = _grid;
        t.Controls.Add(_grid, 0, 3);

        t.Controls.Add(new Label
        {
            Text = "点表格行可在图上高亮；视频模式点目标可看轨迹 ID。",
            Font = Theme.Small,
            ForeColor = Theme.Muted,
            AutoSize = false,
            Dock = DockStyle.Fill,
            Height = 30,
            TextAlign = ContentAlignment.MiddleLeft,
            Margin = new Padding(0, 4, 0, 0),
        }, 0, 4);

        panel.Controls.Add(t);
        return panel;
    }

    private void AddCaptureColumn(string name, string header, int weight, int minWidth) =>
        _grid.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = name,
            HeaderText = header,
            FillWeight = weight,
            MinimumWidth = minWidth,
            SortMode = DataGridViewColumnSortMode.NotSortable,
        });

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
    private ContextMenuStrip BuildImageContextMenu()
    {
        var menu = new ContextMenuStrip { Font = Theme.Body, ShowImageMargin = false };
        menu.Items.Add(new ToolStripMenuItem("保存图片…（原图）", null, (_, _) => SaveCurrentImage(true))
        {
            ShortcutKeyDisplayString = "Ctrl+S",
        });
        menu.Items.Add(new ToolStripMenuItem("保存画面…（带识别框）", null, (_, _) => SaveCurrentImage(false)));
        menu.Items.Add(new ToolStripSeparator());
        menu.Items.Add(new ToolStripMenuItem("复制图片", null, (_, _) => CopyCurrentImage()));
        menu.Items.Add(new ToolStripMenuItem("适应窗口", null, (_, _) => _picRec?.Fit()));
        menu.Opening += (_, e) => e.Cancel = CurrentSourceBitmap() == null;
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
    private void SaveCurrentImage(bool plain)
    {
        Bitmap shot = null;
        try
        {
            shot = plain ? CurrentSourceBitmap() : RenderCanvasBitmap();
            if (shot == null)
            {
                MessageBox.Show(this, "当前画面上还没有图片。", "保存图片",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            using var dialog = new SaveFileDialog
            {
                Title = plain ? "保存图片" : "保存画面",
                Filter = "PNG 图片|*.png|JPEG 图片|*.jpg;*.jpeg|BMP 图片|*.bmp",
                FilterIndex = 1,
                AddExtension = true,
                DefaultExt = "png",
                FileName = DefaultImageFileName(plain),
                InitialDirectory = Directory.Exists(AppPaths.CaptureDir) ? AppPaths.CaptureDir : AppPaths.Root,
            };
            if (dialog.ShowDialog(this) != DialogResult.OK) return;
            SaveBitmapTo(shot, dialog.FileName);
            if (_lblStatus != null) _lblStatus.Text = "已保存：" + Path.GetFileName(dialog.FileName);
        }
        catch (Exception ex)
        {
            Log.Error("保存图片失败", ex);
            MessageBox.Show(this, "保存失败：" + ex.Message, "保存图片",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        finally
        {
            if (!plain) shot?.Dispose();
        }
    }

    private Bitmap RenderCanvasBitmap()
    {
        if (_picRec == null || _picRec.Width <= 0 || _picRec.Height <= 0) return null;
        var bmp = new Bitmap(_picRec.Width, _picRec.Height);
        try
        {
            _picRec.DrawToBitmap(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));
            return bmp;
        }
        catch (Exception ex)
        {
            bmp.Dispose();
            Log.Error("渲染画面失败", ex);
            return null;
        }
    }

    private string DefaultImageFileName(bool plain)
    {
        var stamp = DateTime.Now.ToString("yyyyMMdd-HHmmss");
        if (plain && !string.IsNullOrEmpty(_recPath) && File.Exists(_recPath))
            return Path.GetFileNameWithoutExtension(_recPath) + "-" + stamp + Path.GetExtension(_recPath);
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

    private void CopyCurrentImage()
    {
        var src = CurrentSourceBitmap();
        if (src == null) return;
        try
        {
            using var copy = new Bitmap(src);
            Clipboard.SetImage(copy);
            if (_lblStatus != null) _lblStatus.Text = "图片已复制到剪贴板";
        }
        catch (Exception ex)
        {
            Log.Error("复制图片失败", ex);
            MessageBox.Show(this, "复制失败：" + ex.Message, "复制图片",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
