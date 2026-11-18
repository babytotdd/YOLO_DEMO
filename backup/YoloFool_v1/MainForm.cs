using System.Diagnostics;
using System.Drawing.Imaging;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using DeploySharp.Engine;
using OpenCvSharp.Extensions;

namespace YoloFool;

public partial class MainForm : Form
{
    private sealed class BoxRecord
    {
        public string Cn { get; set; } = "";
        public string En { get; set; } = "";
        public int X { get; set; }
        public int Y { get; set; }
        public int W { get; set; }
        public int H { get; set; }
    }

    private readonly InferenceService _svc = new();
    private AppSettings _settings = new();
    private LabelMap _labels = new();
    private readonly Dictionary<string, List<BoxRecord>> _anno = new(StringComparer.OrdinalIgnoreCase);
    private bool _busy;
    private bool _advExpanded;

    private Bitmap _learnBitmap;
    private Point _rbStart, _rbCur;
    private bool _rbDown;
    private Process _trainProc;

    public MainForm()
    {
        InitializeComponent();
        cboAdvKind.Items.AddRange(new object[] { "YOLOv8Det", "YOLOv11Det", "YOLOv8Seg" });
        cboAdvEngine.Items.AddRange(new object[] { "OnnxRuntime" });
        cboAdvDevice.Items.AddRange(new object[] { "CPU", "GPU0" });
        cboAdvProvider.Items.AddRange(new object[] { "Default", "DML", "OpenVINO", "Cuda" });
        grid.Columns.Add("cName", "目标名称");
        grid.Columns.Add("cScore", "匹配度");
        grid.Columns.Add("cPos", "位置(百分比)");
        grid.Columns.Add("cPixel", "像素框(x,y,w,h)");
        lvBoxes.Columns.Add("目标", 120);
        lvBoxes.Columns.Add("存储名", 110);
        lvBoxes.Columns.Add("框(x,y,w,h)", 200);
    }

    private async void MainForm_Load(object sender, EventArgs e)
    {
        _settings = AppSettings.Load();
        _labels = LabelMap.Load();
        LoadAnnotations();
        PushSettingsToUi();
        btnRun.Enabled = false;
        Text = "YoloFool 傻瓜式视觉识别 · 模型加载中…";
        try
        {
            await Task.Run(() => LoadModelFromSettings());
            Text = "YoloFool 傻瓜式视觉识别 · 模型就绪";
        }
        catch (Exception ex) { MessageBox.Show("模型加载失败：" + ex.Message, "错误"); }
        finally { btnRun.Enabled = true; }
    }

    private void LoadModelFromSettings() =>
        _svc.Load(_settings.ModelPath, (ModelKind)Enum.Parse(typeof(ModelKind), _settings.ModelKind),
            InferenceBackend.OnnxRuntime,
            _settings.Device == "GPU0" ? DeviceType.GPU0 : DeviceType.CPU,
            (OnnxRuntimeDeviceType)Enum.Parse(typeof(OnnxRuntimeDeviceType), _settings.OnnxProvider),
            _settings.Confidence, _settings.Nms);

    private void PushSettingsToUi()
    {
        tbAdvModel.Text = _settings.ModelPath;
        cboAdvKind.SelectedItem = _settings.ModelKind; cboAdvEngine.SelectedItem = _settings.Engine;
        cboAdvDevice.SelectedItem = _settings.Device; cboAdvProvider.SelectedItem = _settings.OnnxProvider;
        numConf.Value = (decimal)_settings.Confidence; numNms.Value = (decimal)_settings.Nms;
        tbRecImage.Text = _settings.LastImagePath;
        lblAdvModelInfo.Text = "当前模型：" + Path.GetFileName(_settings.ModelPath) +
            (File.Exists(AppPaths.MyModelOnnx) ? "（我的模型存在，可自动切换）" : "（尚无我的模型，训练后自动启用）");
    }

    private void btnToggleAdv_Click(object sender, EventArgs e)
    {
        _advExpanded = !_advExpanded;
        pnlAdv.Visible = _advExpanded;
        pnlAdv.Height = _advExpanded ? 140 : 0;
        btnToggleAdv.Text = _advExpanded ? "▾ 收起设置" : "▸ 详细设置";
    }

    private void btnSelectImage_Click(object sender, EventArgs e)
    {
        using var dlg = new OpenFileDialog { InitialDirectory = @"H:\Desktop\YOLO_DEMO\demo\images", Title = "选择测试输入图片", Filter = "图片(*.png;*.jpg;*.jpeg;*.bmp)|*.png;*.jpg;*.jpeg;*.bmp" };
        if (dlg.ShowDialog() == DialogResult.OK) tbRecImage.Text = dlg.FileName;
    }

    private async void btnRun_Click(object sender, EventArgs e)
    {
        if (_busy) return;
        string path = tbRecImage.Text.Trim();
        if (!File.Exists(path)) { MessageBox.Show("请先选择有效的图片文件。", "提示"); return; }
        _busy = true; btnRun.Enabled = false;
        try
        {
            var (items, drawn) = await Task.Run(() => _svc.Predict(path));
            using (drawn)
            {
                var old = picRec.Image;
                picRec.Image = ToStandaloneBitmap(drawn);
                old?.Dispose();
            }
            grid.Rows.Clear();
            foreach (var r in items)
            {
                double cx = r.Bounds.X + r.Bounds.Width / 2.0, cy = r.Bounds.Y + r.Bounds.Height / 2.0;
                grid.Rows.Add(_labels.ToChinese(r.Category), r.Confidence.ToString("F2"),
                    $"横向 {cx / r.ImageW * 100:F1}%  纵向 {cy / r.ImageH * 100:F1}%",
                    $"{r.Bounds.X},{r.Bounds.Y},{r.Bounds.Width},{r.Bounds.Height}");
            }
            if (items.Count == 0) grid.Rows.Add("（无目标）", "-", "降低置信度阈值试试", "-");
            _settings.LastImagePath = path; _settings.Save();
        }
        catch (Exception ex) { MessageBox.Show("识别失败：" + ex.Message, "错误"); }
        finally { _busy = false; btnRun.Enabled = true; }
    }

    private static Bitmap ToStandaloneBitmap(OpenCvSharp.Mat mat)
    {
        using var ms = new MemoryStream();
        BitmapConverter.ToBitmap(mat).Save(ms, ImageFormat.Bmp);
        ms.Position = 0;
        return new Bitmap(ms);
    }

    private void btnAdvModel_Click(object sender, EventArgs e)
    {
        using var dlg = new OpenFileDialog { InitialDirectory = Path.GetDirectoryName(tbAdvModel.Text) ?? @"H:\Desktop\YOLO_DEMO", Title = "选择推理模型文件", Filter = "模型文件(*.onnx)|*.onnx" };
        if (dlg.ShowDialog() == DialogResult.OK) tbAdvModel.Text = dlg.FileName;
    }

    private async void btnSaveAdv_Click(object sender, EventArgs e)
    {
        _settings.ModelPath = tbAdvModel.Text.Trim();
        _settings.ModelKind = cboAdvKind.SelectedItem?.ToString() ?? "YOLOv8Det";
        _settings.Engine = cboAdvEngine.SelectedItem?.ToString() ?? "OnnxRuntime";
        _settings.Device = cboAdvDevice.SelectedItem?.ToString() ?? "CPU";
        _settings.OnnxProvider = cboAdvProvider.SelectedItem?.ToString() ?? "Default";
        _settings.Confidence = (double)numConf.Value; _settings.Nms = (double)numNms.Value;
        _settings.Save();
        btnRun.Enabled = false;
        try { await Task.Run(() => LoadModelFromSettings()); MessageBox.Show("设置已保存，模型已重新加载。", "提示"); }
        catch (Exception ex) { MessageBox.Show("新模型加载失败（设置已保存）：" + ex.Message, "错误"); }
        finally { btnRun.Enabled = true; PushSettingsToUi(); }
    }

    // ---------------- 学习页：框选标注 ----------------

    private void btnOpenLearnImage_Click(object sender, EventArgs e)
    {
        using var dlg = new OpenFileDialog { InitialDirectory = @"H:\Desktop\YOLO_DEMO\demo\images", Title = "打开要标注的图片", Filter = "图片(*.png;*.jpg;*.jpeg;*.bmp)|*.png;*.jpg;*.jpeg;*.bmp" };
        if (dlg.ShowDialog() != DialogResult.OK) return;
        tbLearnImage.Text = dlg.FileName;
        _learnBitmap?.Dispose();
        _learnBitmap = new Bitmap(dlg.FileName);
        picLearn.Image = _learnBitmap;
        if (!_anno.ContainsKey(dlg.FileName)) _anno[dlg.FileName] = new List<BoxRecord>();
        RefreshBoxList();
    }

    private Rectangle DisplayRect()
    {
        var img = picLearn.Image;
        if (img == null) return Rectangle.Empty;
        float scale = Math.Min((float)picLearn.Width / img.Width, (float)picLearn.Height / img.Height);
        int dw = (int)(img.Width * scale), dh = (int)(img.Height * scale);
        return new Rectangle((picLearn.Width - dw) / 2, (picLearn.Height - dh) / 2, dw, dh);
    }

    private Point ToImage(Point p)
    {
        var d = DisplayRect(); var img = picLearn.Image;
        if (img == null || d.Width == 0) return Point.Empty;
        return new Point((int)((p.X - d.X) * img.Width / (float)d.Width), (int)((p.Y - d.Y) * img.Height / (float)d.Height));
    }

    private Rectangle ToDisplay(Rectangle r)
    {
        var d = DisplayRect(); var img = picLearn.Image;
        if (img == null || d.Width == 0) return r;
        return new Rectangle(d.X + r.X * d.Width / img.Width, d.Y + r.Y * d.Height / img.Height, r.Width * d.Width / img.Width, r.Height * d.Height / img.Height);
    }

    private void picLearn_MouseDown(object sender, MouseEventArgs e)
    {
        if (e.Button != MouseButtons.Left || picLearn.Image == null) return;
        _rbDown = true; _rbStart = _rbCur = e.Location;
    }

    private void picLearn_MouseMove(object sender, MouseEventArgs e)
    {
        if (!_rbDown) return;
        _rbCur = e.Location; picLearn.Invalidate();
    }

    private void picLearn_MouseUp(object sender, MouseEventArgs e)
    {
        if (!_rbDown) return;
        _rbDown = false;
        var rb = Rectangle.FromLTRB(_rbStart.X, _rbStart.Y, e.X, e.Y);
        if (rb.Width < 6 || rb.Height < 6) { picLearn.Invalidate(); return; }
        var a = ToImage(rb.Location); var b = ToImage(new Point(rb.Right, rb.Bottom));
        var imgRect = Rectangle.FromLTRB(a.X, a.Y, b.X, b.Y);
        string path = tbLearnImage.Text.Trim();
        if (string.IsNullOrEmpty(path) || !_anno.ContainsKey(path)) { picLearn.Invalidate(); return; }
        string cn = Microsoft.VisualBasic.Interaction.InputBox("给这个目标起个名字（中文即可，存储用拼音）：", "命名目标", "");
        if (string.IsNullOrWhiteSpace(cn)) { picLearn.Invalidate(); return; }
        cn = cn.Trim();
        _anno[path].Add(new BoxRecord { Cn = cn, En = _labels.EnsureEnglish(cn), X = imgRect.X, Y = imgRect.Y, W = imgRect.Width, H = imgRect.Height });
        _labels.Save(); SaveAnnotations();
        picLearn.Invalidate(); RefreshBoxList();
    }

    private void picLearn_Paint(object sender, PaintEventArgs pe)
    {
        if (picLearn.Image == null) return;
        string path = tbLearnImage.Text.Trim();
        using var penG = new Pen(Color.Lime, 2);
        if (path != null && _anno.TryGetValue(path, out var list))
            foreach (var bx in list)
            {
                var d = ToDisplay(Rectangle.FromLTRB(bx.X, bx.Y, bx.X + bx.W, bx.Y + bx.H));
                pe.Graphics.DrawRectangle(penG, d);
                pe.Graphics.DrawString(bx.Cn, new Font("Microsoft YaHei UI", 9F, FontStyle.Bold), Brushes.Lime, d.X, d.Y - 18);
            }
        if (_rbDown)
        {
            using var penR = new Pen(Color.Red, 2) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash };
            pe.Graphics.DrawRectangle(penR, Rectangle.FromLTRB(_rbStart.X, _rbStart.Y, _rbCur.X, _rbCur.Y));
        }
    }

    private void RefreshBoxList()
    {
        lvBoxes.Items.Clear();
        string path = tbLearnImage.Text.Trim();
        if (path != null && _anno.TryGetValue(path, out var list))
            foreach (var bx in list)
                lvBoxes.Items.Add(new ListViewItem(new[] { bx.Cn, bx.En, $"{bx.X},{bx.Y},{bx.W},{bx.H}" }));
        int imgs = _anno.Count(kv => kv.Value.Count > 0), boxes = _anno.Values.Sum(v => v.Count);
        lblAnnoInfo.Text = $"已标注 {imgs} 张图 / {boxes} 个框 / {_labels.DisplayNamesByIndex().Count} 个类别";
    }

    private void btnDelBox_Click(object sender, EventArgs e)
    {
        if (lvBoxes.SelectedItems.Count == 0) return;
        string path = tbLearnImage.Text.Trim();
        if (_anno.TryGetValue(path, out var list))
        {
            int idx = lvBoxes.SelectedItems[0].Index;
            if (idx < list.Count) { list.RemoveAt(idx); SaveAnnotations(); }
        }
        RefreshBoxList(); picLearn.Invalidate();
    }

    private void btnRenameBox_Click(object sender, EventArgs e)
    {
        if (lvBoxes.SelectedItems.Count == 0) return;
        string path = tbLearnImage.Text.Trim();
        if (!_anno.TryGetValue(path, out var list)) return;
        int idx = lvBoxes.SelectedItems[0].Index;
        if (idx >= list.Count) return;
        string cn = Microsoft.VisualBasic.Interaction.InputBox("新的目标名称：", "改名", list[idx].Cn);
        if (string.IsNullOrWhiteSpace(cn)) return;
        list[idx].Cn = cn.Trim(); list[idx].En = _labels.EnsureEnglish(cn.Trim());
        _labels.Save(); SaveAnnotations(); RefreshBoxList(); picLearn.Invalidate();
    }

    private void btnSaveAnno_Click(object sender, EventArgs e)
    {
        try
        {
            string imgDir = Path.Combine(AppPaths.DatasetDir, "images");
            string labDir = Path.Combine(AppPaths.DatasetDir, "labels");
            Directory.CreateDirectory(imgDir); Directory.CreateDirectory(labDir);
            var classes = LoadClasses();
            int saved = 0;
            foreach (var kv in _anno.Where(k => k.Value.Count > 0))
            {
                string src = kv.Key;
                string baseName = Path.GetFileNameWithoutExtension(src);
                string dstImg = Path.Combine(imgDir, baseName + Path.GetExtension(src).ToLowerInvariant());
                File.Copy(src, dstImg, overwrite: true);
                var sb = new StringBuilder();
                foreach (var bx in kv.Value)
                {
                    int ci = classes.IndexOf(bx.En);
                    if (ci < 0) { ci = classes.Count; classes.Add(bx.En); }
                    double cx = (bx.X + bx.W / 2.0) / bxImgW(src, bx), cy = (bx.Y + bx.H / 2.0) / bxImgH(src, bx);
                    double w = (double)bx.W / bxImgW(src, bx), h = (double)bx.H / bxImgH(src, bx);
                    sb.AppendLine($"{ci} {cx:F6} {cy:F6} {w:F6} {h:F6}");
                }
                File.WriteAllText(Path.Combine(labDir, baseName + ".txt"), sb.ToString());
                saved++;
            }
            File.WriteAllLines(Path.Combine(AppPaths.DatasetDir, "classes.txt"), classes);
            _labels.Save(); SaveAnnotations();
            MessageBox.Show($"已导出 {saved} 张图到 mydataset（类别：{string.Join("、", _labels.DisplayNamesByIndex())}）", "保存标注");
        }
        catch (Exception ex) { MessageBox.Show("导出失败：" + ex.Message, "错误"); }
    }

    private static int bxImgW(string src, BoxRecord b) => ImageSizeOf(src).w;
    private static int bxImgH(string src, BoxRecord b) => ImageSizeOf(src).h;
    private static readonly Dictionary<string, (int w, int h)> _sizeCache = new();
    private static (int w, int h) ImageSizeOf(string src)
    {
        if (!_sizeCache.TryGetValue(src, out var s)) { using var img = Image.FromFile(src); s = (img.Width, img.Height); _sizeCache[src] = s; }
        return s;
    }

    private List<string> LoadClasses()
    {
        string f = Path.Combine(AppPaths.DatasetDir, "classes.txt");
        return File.Exists(f) ? File.ReadAllLines(f).Where(l => !string.IsNullOrWhiteSpace(l)).ToList() : new List<string>();
    }

    private void SaveAnnotations()
    {
        Directory.CreateDirectory(AppPaths.DatasetDir);
        File.WriteAllText(Path.Combine(AppPaths.DatasetDir, "annotations.json"), JsonSerializer.Serialize(_anno, new JsonSerializerOptions { WriteIndented = true }));
        RefreshBoxList();
    }

    private void LoadAnnotations()
    {
        try
        {
            string f = Path.Combine(AppPaths.DatasetDir, "annotations.json");
            if (File.Exists(f))
            {
                var d = JsonSerializer.Deserialize<Dictionary<string, List<BoxRecord>>>(File.ReadAllText(f));
                foreach (var kv in d ?? new()) _anno[kv.Key] = kv.Value ?? new();
            }
        }
        catch { }
    }

    // ---------------- 训练环境 + 后台训练 ----------------

    private static string YoloExe => Path.Combine(AppPaths.ToolsDir, "yolovenv", "Scripts", "yolo.exe");
    private bool EnvReady => File.Exists(YoloExe);

    private async void btnInitEnv_Click(object sender, EventArgs e)
    {
        btnInitEnv.Enabled = false;
        try { await Task.Run(InitTrainingEnv); }
        catch (Exception ex) { Log("环境初始化失败：" + ex.Message); }
        finally { btnInitEnv.Enabled = true; }
    }

    private void InitTrainingEnv()
    {
        Directory.CreateDirectory(AppPaths.ToolsDir);
        if (!File.Exists(AppPaths.VenvPython))
        {
            Log("创建独立 Python 环境（不影响系统 Python）…");
            RunTool("python", $"-m venv \"{AppPaths.ToolsDir}\\yolovenv\"", null);
        }
        Log("安装 ultralytics + torch(CPU)（清华源，首次约 500MB~1GB，请耐心等待）…");
        RunTool(AppPaths.VenvPython, "-m pip install --upgrade pip -i https://pypi.tuna.tsinghua.edu.cn/simple", null);
        RunTool(AppPaths.VenvPython, "-m pip install torch torchvision --index-url https://download.pytorch.org/whl/cpu -i https://pypi.tuna.tsinghua.edu.cn/simple", null);
        RunTool(AppPaths.VenvPython, "-m pip install ultralytics -i https://pypi.tuna.tsinghua.edu.cn/simple", null);
        string pt = Path.Combine(AppPaths.ToolsDir, "yolov8n.pt");
        if (!File.Exists(pt))
        {
            Log("下载预训练权重 yolov8n.pt（ghfast 镜像）…");
            using var http = new HttpClient();
            http.Timeout = TimeSpan.FromMinutes(10);
            var bytes = http.GetByteArrayAsync("https://ghfast.top/https://github.com/ultralytics/assets/releases/download/v8.3.0/yolov8n.pt").GetAwaiter().GetResult();
            File.WriteAllBytes(pt, bytes);
        }
        Log(EnvReady ? "训练环境就绪 ✔" : "警告：未找到 yolo.exe，请重试初始化");
    }

    private async void btnTrain_Click(object sender, EventArgs e)
    {
        if (!EnvReady) { MessageBox.Show("请先点\"初始化训练环境\"。", "提示"); return; }
        var classes = LoadClasses();
        int imgs = _anno.Count(kv => kv.Value.Count > 0);
        if (imgs < 3) { MessageBox.Show("至少标注 3 张图再训练（建议 30 张以上）。", "提示"); return; }
        btnTrain.Enabled = false;
        try
        {
            string yaml = Path.Combine(AppPaths.DatasetDir, "data.yaml");
            File.WriteAllText(yaml, $"path: {AppPaths.DatasetDir}\ntrain: images\nval: images\nnc: {classes.Count}\nnames: [{string.Join(", ", classes.Select(c => "'" + c + "'"))}]\n");
            int runName = 1;
            while (Directory.Exists(Path.Combine(AppPaths.DatasetDir, "runs", "train" + runName))) runName++;
            string args = $"detect train data=\"{yaml}\" model=\"{Path.Combine(AppPaths.ToolsDir, "yolov8n.pt")}\" epochs=50 device=cpu imgsz=640 batch=4 project=\"{Path.Combine(AppPaths.DatasetDir, "runs")}\" name=train{runName} exist_ok=True";
            Log("开始训练（CPU，50 轮）：" + args);
            var result = await Task.Run(() => RunTool(YoloExe, args, null));
            if (result != 0) { Log("训练进程退出码 " + result + "，未产出模型。"); return; }
            string bestPt = Path.Combine(AppPaths.DatasetDir, "runs", "train" + runName, "weights", "best.pt");
            if (!File.Exists(bestPt)) { Log("未找到 best.pt，训练可能未完成。"); return; }
            Log("训练完成，导出 ONNX…");
            int rc = RunTool(YoloExe, $"export model=\"{bestPt}\" format=onnx imgsz=640", null);
            string bestOnnx = Path.ChangeExtension(bestPt, ".onnx");
            if (rc == 0 && File.Exists(bestOnnx))
            {
                Directory.CreateDirectory(AppPaths.MyModelsDir);
                File.Copy(bestOnnx, AppPaths.MyModelOnnx, overwrite: true);
                _settings.ModelPath = AppPaths.MyModelOnnx; _settings.ModelKind = "YOLOv8Det"; _settings.Save();
                await Task.Run(() => LoadModelFromSettings());
                PushSettingsToUi();
                Log("已自动切换默认模型为\"我的模型\" ✔ 去识别页试试新目标！");
            }
            else Log("ONNX 导出失败，请查看上方日志。");
        }
        catch (Exception ex) { Log("训练失败：" + ex.Message); }
        finally { btnTrain.Enabled = true; }
    }

    private void btnCancelTrain_Click(object sender, EventArgs e)
    {
        try { _trainProc?.Kill(true); Log("已请求取消训练。"); } catch (Exception ex) { Log("取消失败：" + ex.Message); }
    }

    private void Log(string s)
    {
        if (InvokeRequired) { BeginInvoke(() => Log(s)); return; }
        txtTrainLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {s}" + Environment.NewLine);
    }

    private int RunTool(string exe, string args, string workDir)
    {
        var psi = new ProcessStartInfo(exe, args)
        {
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            WorkingDirectory = workDir ?? AppPaths.ToolsDir,
            CreateNoWindow = true,
            StandardOutputEncoding = Encoding.UTF8,
        };
        var p = new Process { StartInfo = psi };
        p.OutputDataReceived += (_, a) => { if (a.Data != null) Log(a.Data); };
        p.ErrorDataReceived += (_, a) => { if (a.Data != null) Log(a.Data); };
        p.Start();
        p.BeginOutputReadLine(); p.BeginErrorReadLine();
        _trainProc = p;
        p.WaitForExit();
        _trainProc = null;
        return p.ExitCode;
    }
}
