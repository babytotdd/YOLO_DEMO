using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mat = OpenCvSharp.Mat;
using Cv2 = OpenCvSharp.Cv2;
using CvPoint = OpenCvSharp.Point;
using CvScalar = OpenCvSharp.Scalar;
using CvMatType = OpenCvSharp.MatType;
using CvRect = OpenCvSharp.Rect;
using Timer = System.Windows.Forms.Timer;

namespace YoloFool;

/// <summary>
/// 针床检测页：固定相机看产品有没有放正。
/// 调试模式配相机/标定/模板/公差，执行模式只留一个大按钮和 OK/NG。
/// </summary>
public sealed partial class MainForm
{

    private ToolTip _fixTip;
    private bool _syncingFixTol;

    private bool _fixZonePicking;

    // 【分割参数】页：参数一改就作用到实时画面，叠加掩码看产品边缘。
    private bool _fixSegPreview;
    private bool _fixSegStatic;
    private bool _fixSegSyncing;

    // 安全区/检测区叠加只在这个时间点之前画：工具做完就恢复干净画面。
    private DateTime _fixOverlayUntil = DateTime.MinValue;

    private Timer _fixTimer;
    private bool _fixPreviewHeld;   // 保存对话框期间预览被临时停掉，关掉后要记得恢复
    private FixtureCamera _fixCam;
    private Mat _fixSnap;
    private bool _fixFrozen;
    private bool _fixOpening;
    private FixtureRecipe _fixRecipe;
    private Mat _fixGolden;
    private Mat _fixMask;
    private Mat _fixEmpty;
    private string _fixModel = "";
    private bool _fixDebug = true;
    private bool _fixModelSync;
    private bool _fixJudging;
    private bool _fixRoiPicking;
    private bool _fixCalibPicking;
    private bool _fixtureActive;
    private long _fixShownFrame = -1;
    private double _fixCalibPx;
    private Point? _fixCalibLineA;
    private Point? _fixCalibLineB;
    private bool _fixHold;
    private bool _fixTemplateView;
    private bool _fixCapturing;
    private long _fixFrameWatermark;
    private long _fixSnapFrameId;
    private PictureBox _fixThumbEmpty;
    private PictureBox _fixThumbGolden;
    private DateTime _fixFreezeUntil = DateTime.MinValue;

    private static readonly (string Text, int W, int H)[] FixResolutions =
    {
        ("640×480", 640, 480),
        ("1280×720", 1280, 720),
        ("1920×1080", 1920, 1080),
    };

    // ---------------------------------------------------------------- layout

    /// <summary>
    /// 「针床检测」页的静态控件树已经搬进 MainForm.Designer.cs（可直接在设计器里改）。
    /// 这里只做运行期的事：挂事件、建缩略图、配右键菜单与提示。
    /// </summary>
    private void InitializeFixturePage()
    {
        _picFixture.CanvasMenu = BuildImageContextMenu(_picFixture, () => _picFixture.Image,
            text => SetFixCameraInfo(text), null, SetFixPreviewBusy);
        _picFixture.LineLabel = "标定线";
        _picFixture.SaveRequested += () =>
            SaveImageAs(_picFixture, () => _picFixture.Image, true, text => SetFixCameraInfo(text), null,
                SetFixPreviewBusy);

        _flpFixThumbs.Controls.Add(MakeFixThumb("空治具", false));
        _flpFixThumbs.Controls.Add(MakeFixThumb("金样板", true));
        _pnlFixExec.Visible = false;

        _btnFixCamApply.Click += async (_, _) => await OpenFixCameraAsync(false);
        _btnFixCamOpenApply.Click += async (_, _) => await OpenFixCameraAsync(false);
        _btnFixCamDriver.Click += (_, _) => ShowCameraPropertyPage();
        _btnFixOverlay.Click += (_, _) => ToggleFixOverlay();
        _tcFixDebug.SelectedIndexChanged += (_, _) => OnFixDebugTabChanged();

        _fixTip ??= new ToolTip { AutoPopDelay = 9000, InitialDelay = 350, ReshowDelay = 100, ShowAlways = true };
        _fixTip.SetToolTip(_trkFixPos, "位置允许偏移：产品相对金样板允许的最大平移（X/Y 同一值）。调大=更容易放行轻微歪放；调小=更严格。");
        _fixTip.SetToolTip(_numFixPos, "位置允许偏移（mm）：拖滑块或直接输入，两个方向共用。");
        _fixTip.SetToolTip(_trkFixAngle, "角度允许偏转：产品相对金样板允许的最大旋转，压损边界约 7°。");
        _fixTip.SetToolTip(_numFixAngle, "角度允许偏转（度）：拖滑块或直接输入。");
        _fixTip.SetToolTip(_cboFixShape, "形状严格度：宽松 IoU≥0.80 / 标准 0.85 / 严格 0.90；越严格越容易被反光、遮挡判成 NG。");
        _fixTip.SetToolTip(_numFixConfirm, "连续多少帧都 NG 才报警，用来压掉单帧抖动。");
        _fixTip.SetToolTip(_btnFixSaveTol, "把当前阈值写进机种配方；不点保存不会生效。");
        _fixTip.SetToolTip(_btnFixAdvToggle, "展开/收起不常用的数值参数（X/Y 公差、面积比、质量闸门等）。");
        _fixTip.SetToolTip(_numFixDx, "X 方向允许偏移（mm），高级用法；常用层会同时改 X/Y。");
        _fixTip.SetToolTip(_numFixDy, "Y 方向允许偏移（mm），高级用法。");
        _fixTip.SetToolTip(_numFixIou, "与金样板的重叠度下限 IoU；标准 0.85。");
        _fixTip.SetToolTip(_numFixAreaMin, "产品面积相对金样板的下限比例。");
        _fixTip.SetToolTip(_numFixAreaMax, "产品面积相对金样板的 上限比例。");
        _fixTip.SetToolTip(_numFixZone, "金样板外形再向外扩多少 mm 仍算安全；超出即判压损风险。");
        _fixTip.SetToolTip(_numFixContour, "轮廓充实度绝对下限；实际用 max(该值, 金样板分-允许下降)。");
        _fixTip.SetToolTip(_numFixDrop, "允许现场帧比金样板低多少充实度；调大更耐反光遮挡。");
        _fixTip.SetToolTip(_numFixMeanGray, "画面过暗门限：均值低于它直接判图像质量不合格。");
        _fixTip.SetToolTip(_numFixContrast, "画面对比度过低门限。");
        _fixTip.SetToolTip(_numFixHighlight, "高光过曝像素比例上限。");
    }

    private Control MakeFixThumb(string caption, bool golden)
    {
        var panel = new TableLayoutPanel
        {
            ColumnCount = 1,
            RowCount = 2,
            Width = 116,
            Height = 68,
            BackColor = Color.White,
            Margin = new Padding(0, 0, 6, 0),
            Cursor = Cursors.Hand,
        };
        panel.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
        panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 16f));

        var pic = new PictureBox
        {
            Dock = DockStyle.Fill,
            SizeMode = PictureBoxSizeMode.Zoom,
            BackColor = Color.FromArgb(245, 246, 248),
            Cursor = Cursors.Hand,
            Margin = new Padding(0),
        };
        var text = new Label
        {
            Dock = DockStyle.Fill,
            Text = caption,
            Font = Theme.Small,
            ForeColor = Theme.Muted,
            TextAlign = ContentAlignment.MiddleCenter,
            Cursor = Cursors.Hand,
            Margin = new Padding(0),
        };

        panel.Controls.Add(pic, 0, 0);
        panel.Controls.Add(text, 0, 1);
        if (golden) _fixThumbGolden = pic; else _fixThumbEmpty = pic;

        EventHandler click = (_, _) => ShowTemplate(golden);
        panel.Click += click;
        pic.Click += click;
        text.Click += click;
        return panel;
    }

    private void UpdateFixThumbs()
    {
        SetFixThumb(_fixThumbEmpty, _fixEmpty);
        SetFixThumb(_fixThumbGolden, _fixGolden);
    }

    private void SetFixThumb(PictureBox pic, Mat mat)
    {
        if (pic == null) return;
        var old = pic.Image;
        pic.Image = mat == null || mat.Empty() ? null : MatToBitmap(mat);
        old?.Dispose();
    }

    /// <summary>点缩略图：把保存的模板显示在左侧，并显示标定线，方便核对机位有没有动。</summary>
    private void ShowTemplate(bool golden)
    {
        var mat = golden ? _fixGolden : _fixEmpty;
        if (mat == null || mat.Empty())
        {
            SetFixCameraInfo(golden ? "还没有采集金样板" : "还没有采集空治具");
            return;
        }

        _fixSnap?.Dispose();
        _fixSnap = null;
        _fixFrozen = false;
        _fixSnapFrameId = 0;

        ShowFixFrame(mat);
        _fixHold = true;
        _fixTemplateView = true;
        ShowCalibLine(true);
        UpdateFixSnapButtons();
        SetFixVerdict(null);
        SetFixCameraInfo($"正在查看{(golden ? "金样板" : "空治具")}模板；点“恢复预览”回到实时画面");
    }

    /// <summary>把配方里的分割参数回填到【分割参数】页。</summary>
    private void LoadSegParamsToUi()
    {
        if (_fixRecipe == null || _numFixSegHMin == null) return;
        _fixSegSyncing = true;
        try
        {
            var s = _fixRecipe.Segment ??= new FixtureSegment();
            _numFixSegHMin.Value = Clamp((decimal)s.HMin, _numFixSegHMin);
            _numFixSegHMax.Value = Clamp((decimal)s.HMax, _numFixSegHMax);
            _numFixSegSMin.Value = Clamp((decimal)s.SMin, _numFixSegSMin);
            _numFixSegSMax.Value = Clamp((decimal)s.SMax, _numFixSegSMax);
            _numFixSegVMin.Value = Clamp((decimal)s.VMin, _numFixSegVMin);
            _numFixSegVMax.Value = Clamp((decimal)s.VMax, _numFixSegVMax);
            _numFixSegMorph.Value = Clamp(Math.Clamp(s.MorphKernel, 1, 31), _numFixSegMorph);
            _numFixSegDiff.Value = Clamp(Math.Clamp(s.DiffThreshold, 0, 255), _numFixSegDiff);
            _chkFixSegUseEmpty.Checked = s.UseEmptyDiff;
        }
        finally
        {
            _fixSegSyncing = false;
        }

        UpdateFixSegInfo();
    }

    /// <summary>页面上当前参数 → 配方（只在内存里，点“应用并保存”才落盘）。</summary>
    private void ApplySegParamsToRecipe()
    {
        if (_fixRecipe == null || _numFixSegHMin == null) return;
        var s = _fixRecipe.Segment ??= new FixtureSegment();
        s.HMin = (double)_numFixSegHMin.Value;
        s.HMax = (double)_numFixSegHMax.Value;
        s.SMin = (double)_numFixSegSMin.Value;
        s.SMax = (double)_numFixSegSMax.Value;
        s.VMin = (double)_numFixSegVMin.Value;
        s.VMax = (double)_numFixSegVMax.Value;
        s.MorphKernel = (int)_numFixSegMorph.Value | 1;
        s.DiffThreshold = (int)_numFixSegDiff.Value;
        s.UseEmptyDiff = _chkFixSegUseEmpty.Checked;
    }

    private void OnFixSegParamChanged()
    {
        if (_fixSegSyncing || _fixRecipe == null) return;
        ApplySegParamsToRecipe();
        UpdateFixSegInfo();
        // 定格/相机没开时没有实时帧进来，参数改完要主动重画一次静态预览。
        if (_fixSegPreview && _fixHold) ShowSegStaticPreview();
    }

    private void UpdateFixSegInfo()
    {
        if (_lblFixSegInfo == null || _fixRecipe == null) return;
        var s = _fixRecipe.Segment;
        if (s == null)
        {
            _lblFixSegInfo.Text = "还没有分割参数。";
            return;
        }

        string diff = !s.UseEmptyDiff || s.DiffThreshold <= 0
            ? "差分：关闭"
            : _fixEmpty == null ? "差分：无空治具，未生效" : $"差分阈值：{s.DiffThreshold}";
        _lblFixSegInfo.Text =
            $"H {s.HMin:F0}–{s.HMax:F0} / S {s.SMin:F0}–{s.SMax:F0} / V {s.VMin:F0}–{s.VMax:F0}\r\n" +
            $"形态核 {Math.Max(1, s.MorphKernel | 1)} px / {diff}";
    }

    /// <summary>切到/离开【分割参数】页：进来开实时掩码叠加，离开恢复普通画面。</summary>
    private void OnFixDebugTabChanged()
    {
        if (_tcFixDebug == null || _fixSegTab == null) return;
        bool on = _tcFixDebug.SelectedTab == _fixSegTab;
        if (on == _fixSegPreview) return;
        _fixSegPreview = on;

        if (on)
        {
            // 分割页自己画掩码/安全区，别再叠独立的安全区框。
            _fixOverlayUntil = DateTime.MinValue;
            _picFixture.Boxes = Array.Empty<BoxRecord>();
            LoadSegParamsToUi();
            if (_fixCam == null || _fixHold) ShowSegStaticPreview();
            else _fixShownFrame = -1;
            SetFixCameraInfo(_fixCam == null
                ? "分割参数：打开相机即可实时看掩码；相机没开时用金样板/照片做静态预览"
                : "分割参数：实时画面叠加掩码，改参数立刻看效果");
            return;
        }

        if (_fixSegStatic)
        {
            _fixSegStatic = false;
            if (_fixHold) ResumeFixPreview();
        }

        _fixShownFrame = -1;
        ApplyFixRoiOverlay();
    }

    /// <summary>没有实时帧时（相机没开或画面定格），拿金样板/照片做一次掩码预览。</summary>
    private void ShowSegStaticPreview()
    {
        Mat src = null;
        if (_fixSnap != null && !_fixSnap.Empty()) src = _fixSnap;
        else if (_fixGolden != null && !_fixGolden.Empty()) src = _fixGolden;
        if (src == null)
        {
            UpdateFixSegInfo();
            return;
        }

        using var view = BuildSegPreview(src, out var px, out var over);
        if (view == null) return;
        ShowFixFrame(view);
        _fixHold = true;
        _fixTemplateView = false;
        _fixSegStatic = true;
        UpdateFixSnapButtons();
        SetFixCameraInfo($"分割参数：{(ReferenceEquals(src, _fixSnap) ? "照片" : "金样板")}静态预览，掩码 {px}px，越界 {over}px");
    }

    /// <summary>把掩码叠到画面上：青色=被选中的像素，红边=外沿，橙红=越出安全区。调用方负责释放返回值。</summary>
    private Mat BuildSegPreview(Mat frame, out int maskPx, out int overflowPx)
    {
        maskPx = 0;
        overflowPx = 0;
        if (frame == null || frame.Empty() || _fixRecipe == null) return null;

        using var mask = FixtureAnalyzer.BuildMask(frame, _fixRecipe, _fixEmpty);
        maskPx = Cv2.CountNonZero(mask);

        var view = frame.Clone();
        try
        {
            using (var tint = new Mat(frame.Size(), CvMatType.CV_8UC3, new CvScalar(255, 200, 60)))
            using (var blend = new Mat())
            {
                Cv2.AddWeighted(view, 0.45, tint, 0.55, 0, blend);
                blend.CopyTo(view, mask);
            }

            var zone = _fixRecipe.Zone;
            bool zoneOn = zone != null && zone.Enabled && zone.W >= 16 && zone.H >= 16;
            if (zoneOn)
            {
                var z = zone.ToRect(view.Width, view.Height);
                using var inside = new Mat(mask.Size(), CvMatType.CV_8UC1, CvScalar.All(0));
                Cv2.Rectangle(inside, z, CvScalar.All(255), -1);
                using var outside = new Mat();
                Cv2.BitwiseNot(inside, outside);
                using var over = new Mat();
                Cv2.BitwiseAnd(mask, outside, over);
                overflowPx = Cv2.CountNonZero(over);
                if (overflowPx > 0)
                {
                    using var warn = new Mat(frame.Size(), CvMatType.CV_8UC3, new CvScalar(0, 0, 255));
                    using var blend2 = new Mat();
                    Cv2.AddWeighted(view, 0.35, warn, 0.65, 0, blend2);
                    blend2.CopyTo(view, over);
                }

                Cv2.Rectangle(view, z, new CvScalar(0, 255, 0), 1);
            }

            Cv2.FindContours(mask, out CvPoint[][] contours, out _,
                OpenCvSharp.RetrievalModes.External, OpenCvSharp.ContourApproximationModes.ApproxSimple);
            if (contours.Length > 0) Cv2.DrawContours(view, contours, -1, new CvScalar(0, 0, 255), 2);

            string text = zoneOn
                ? $"mask {maskPx}px  out {overflowPx}px"
                : $"mask {maskPx}px";
            Cv2.PutText(view, text, new CvPoint(12, view.Height - 14),
                OpenCvSharp.HersheyFonts.HersheySimplex, 0.7, new CvScalar(0, 255, 255), 2);

            if (_lblFixSegInfo != null)
            {
                string txt = zoneOn
                    ? $"掩码 {maskPx}px（占检测区 {maskPx * 100.0 / Math.Max(1, zone.W * zone.H):F2}%）· 越界 {overflowPx}px"
                    : $"掩码 {maskPx}px · 还没有安全区，无法算越界";
                if (_lblFixSegInfo.Text != txt) _lblFixSegInfo.Text = txt;
            }

            return view;
        }
        catch
        {
            view.Dispose();
            throw;
        }
    }

    /// <summary>重新估算分割参数（用金样板颜色分布），并同步到分割页。</summary>
    private async Task EstimateSegmentFromTabAsync()
    {
        if (_fixRecipe == null || _fixGolden == null || _fixGolden.Empty())
        {
            SetFixCameraInfo("先采集金样板，才能重新估算分割参数");
            return;
        }

        var recipe = _fixRecipe;
        var golden = _fixGolden;
        try
        {
            var seg = await Task.Run(() => FixtureAnalyzer.EstimateSegment(golden, recipe));
            recipe.Segment = seg;
            LoadSegParamsToUi();
            if (_fixHold) ShowSegStaticPreview();
            _fixShownFrame = -1;
            SetFixCameraInfo("已按金样板重新估算分割参数（还没落盘，满意后点“应用并保存”）");
        }
        catch (Exception ex)
        {
            Log.Error("重新估算分割参数失败", ex);
            SetFixCameraInfo("重新估算失败：" + ex.Message);
        }
    }

    /// <summary>按分割页当前参数重建金样板掩码并保存。</summary>
    private async Task ApplySegmentFromTabAsync()
    {
        if (_fixRecipe == null || _fixGolden == null || _fixGolden.Empty())
        {
            SetFixCameraInfo("先采集金样板，才能保存分割参数");
            return;
        }

        ApplySegParamsToRecipe();
        var recipe = _fixRecipe;
        var golden = _fixGolden;
        var model = _fixModel;
        try
        {
            await Task.Run(() =>
            {
                using var mask = FixtureAnalyzer.BuildMask(golden, recipe, _fixEmpty);
                FixtureStore.SaveImage(model, FixtureStore.GoldenMaskFile, mask);
                recipe.Version++;
                recipe.UpdatedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                FixtureStore.Save(recipe);
            });

            _fixMask?.Dispose();
            _fixMask = FixtureStore.LoadImage(model, FixtureStore.GoldenMaskFile);
            UpdateFixGoldenScore();
            UpdateFixTemplateInfo();
            if (_fixHold) ShowSegStaticPreview();
            SetFixCameraInfo("分割参数已保存，金样板掩码已重建；安全区不会自动变化，回【模板】页确认");
        }
        catch (Exception ex)
        {
            Log.Error("保存分割参数失败", ex);
            SetFixCameraInfo("保存失败：" + ex.Message);
        }
    }

    /// <summary>安全区/检测区叠加的开关：默认只在工具操作后短暂显示。</summary>
    private void ToggleFixOverlay()
    {
        bool show = _fixOverlayUntil != DateTime.MaxValue;
        _fixOverlayUntil = show ? DateTime.MaxValue : DateTime.MinValue;
        if (_btnFixOverlay != null) _btnFixOverlay.Text = show ? "隐藏叠加" : "显示叠加";
        _fixShownFrame = -1;
        if (!show) _picFixture.Boxes = Array.Empty<BoxRecord>();
        else ApplyFixRoiOverlay();
    }

    // ---------------------------------------------------------------- wiring

    private void WireFixtureEvents()
    {
        _fixTimer = new Timer { Interval = 66 };
        _fixTimer.Tick += (_, _) => FixPreviewTick();

        _cboFixModel.SelectedIndexChanged += (_, _) => OnFixtureModelChanged();
        _btnFixNewModel.Click += (_, _) => CreateFixtureModel();
        _btnFixMode.Click += (_, _) => ToggleFixtureMode();
        _btnFixCamClose.Click += (_, _) =>
        {
            CloseFixCamera();
            SetFixCameraInfo("相机已关闭");
        };
        _btnFixSnapshot.Click += async (_, _) => await TakeFixSnapshotAsync();
        _btnFixLive.Click += (_, _) => ResumeFixPreview();
        _btnFixRoi.Click += (_, _) => BeginRoiPick();
        _picFixture.BoxDrawn += OnFixtureBoxDrawn;
        _picFixture.LineDrawn += OnFixtureLineDrawn;
        _btnFixCalibPick.Click += (_, _) => BeginCalibPick();
        _btnFixCalibSave.Click += (_, _) => SaveCalibration();
        _btnFixCaptureEmpty.Click += async (_, _) => await CaptureTemplateAsync(false);
        _btnFixCaptureGolden.Click += async (_, _) => await CaptureTemplateAsync(true);
        _btnFixRebuildSeg.Click += async (_, _) => await RebuildSegmentAsync();
        _btnFixZoneAuto.Click += (_, _) => AutoGenerateZone();
        _btnFixZoneDraw.Click += (_, _) => BeginZonePick();
        _btnFixZoneClear.Click += (_, _) => ClearZone();
        _cboFixJudgeMode.SelectedIndexChanged += (_, _) => OnFixJudgeModeChanged();
        _btnFixTest.Click += async (_, _) => await JudgeTestAsync();
        _btnFixReplay.Click += async (_, _) => await ReplayAsync();
        _btnFixJudge.Click += async (_, _) =>
        {
            ResumeFixPreview();
            await JudgeAsync(null);
        };
        _btnFixReplay2.Click += async (_, _) => await ReplayAsync();
        _btnFixSaveTol.Click += (_, _) => SaveTolerance();
        _btnFixSegEstimate.Click += async (_, _) => await EstimateSegmentFromTabAsync();
        _btnFixSegApply.Click += async (_, _) => await ApplySegmentFromTabAsync();
        foreach (var num in new[] { _numFixSegHMin, _numFixSegHMax, _numFixSegSMin, _numFixSegSMax, _numFixSegVMin, _numFixSegVMax, _numFixSegMorph, _numFixSegDiff })
            num.ValueChanged += (_, _) => OnFixSegParamChanged();
        _chkFixSegUseEmpty.CheckedChanged += (_, _) => OnFixSegParamChanged();
        WireFixThresholdEvents();
        _tabs.SelectedIndexChanged += (_, _) => OnFixtureTabChanged();
        FormClosing += (_, _) => CloseFixCamera();
    }

    private void WireFixThresholdEvents()
    {
        _trkFixPos.ValueChanged += (_, _) => OnFixPosSliderChanged();
        _numFixPos.ValueChanged += (_, _) => OnFixPosNumberChanged();
        _trkFixAngle.ValueChanged += (_, _) => OnFixAngleSliderChanged();
        _numFixAngle.ValueChanged += (_, _) => OnFixAngleNumberChanged();
        _numFixTheta.ValueChanged += (_, _) =>
        {
            if (_syncingFixTol) return;
            _syncingFixTol = true;
            try
            {
                _numFixAngle.Value = Clamp(_numFixTheta.Value, _numFixAngle);
                _trkFixAngle.Value = ClampTrack(_trkFixAngle, _numFixAngle.Value * 2);
            }
            finally
            {
                _syncingFixTol = false;
            }
            UpdateFixTolSummary();
        };
        _cboFixShape.SelectedIndexChanged += (_, _) => ApplyFixShapePreset(_cboFixShape.SelectedIndex);
        _btnFixPresetLoose.Click += (_, _) => ApplyFixPreset(8m, 5m, 0);
        _btnFixPresetStd.Click += (_, _) => ApplyFixPreset(5m, 3.5m, 1);
        _btnFixPresetStrict.Click += (_, _) => ApplyFixPreset(3m, 2m, 2);
        _btnFixAdvToggle.Click += (_, _) =>
        {
            _pnlFixAdv.Visible = !_pnlFixAdv.Visible;
            _btnFixAdvToggle.Text = _pnlFixAdv.Visible ? "收起高级参数 ▴" : "高级参数 ▾";
            if (_pnlFixAdv.Visible && _pnlFixAdv.Parent is ScrollableControl sc)
                sc.AutoScrollPosition = new Point(0, sc.VerticalScroll.Maximum);
        };
        _numFixIou.ValueChanged += (_, _) =>
        {
            if (_syncingFixTol) return;
            UpdateFixShapeSelection();
            UpdateFixTolSummary();
        };
        _numFixDrop.ValueChanged += (_, _) =>
        {
            if (_syncingFixTol) return;
            UpdateFixShapeSelection();
            UpdateFixTolSummary();
        };
    }

    private void OnFixPosSliderChanged()
    {
        if (_syncingFixTol) return;
        _syncingFixTol = true;
        try
        {
            _numFixPos.Value = _trkFixPos.Value / 2m;
            _numFixDx.Value = _numFixPos.Value;
            _numFixDy.Value = _numFixPos.Value;
        }
        finally
        {
            _syncingFixTol = false;
        }
        UpdateFixTolSummary();
    }

    private void OnFixPosNumberChanged()
    {
        if (_syncingFixTol) return;
        _syncingFixTol = true;
        try
        {
            _trkFixPos.Value = ClampTrack(_trkFixPos, _numFixPos.Value * 2);
            _numFixDx.Value = _numFixPos.Value;
            _numFixDy.Value = _numFixPos.Value;
        }
        finally
        {
            _syncingFixTol = false;
        }
        UpdateFixTolSummary();
    }

    private void OnFixAngleSliderChanged()
    {
        if (_syncingFixTol) return;
        _syncingFixTol = true;
        try
        {
            _numFixAngle.Value = _trkFixAngle.Value / 2m;
            _numFixTheta.Value = _numFixAngle.Value;
        }
        finally
        {
            _syncingFixTol = false;
        }
        UpdateFixTolSummary();
    }

    private void OnFixAngleNumberChanged()
    {
        if (_syncingFixTol) return;
        _syncingFixTol = true;
        try
        {
            _trkFixAngle.Value = ClampTrack(_trkFixAngle, _numFixAngle.Value * 2);
            _numFixTheta.Value = _numFixAngle.Value;
        }
        finally
        {
            _syncingFixTol = false;
        }
        UpdateFixTolSummary();
    }

    private static int ClampTrack(TrackBar bar, decimal value) =>
        Math.Clamp((int)Math.Round(value), bar.Minimum, bar.Maximum);

    private void ApplyFixShapePreset(int index)
    {
        if (_syncingFixTol) return;
        if (index < 0 || index > 2)
        {
            UpdateFixTolSummary();
            return;
        }
        _syncingFixTol = true;
        try
        {
            switch (index)
            {
                case 0:
                    _numFixIou.Value = 0.80m;
                    _numFixDrop.Value = 0.15m;
                    break;
                case 1:
                    _numFixIou.Value = 0.85m;
                    _numFixDrop.Value = 0.10m;
                    break;
                default:
                    _numFixIou.Value = 0.90m;
                    _numFixDrop.Value = 0.05m;
                    break;
            }
        }
        finally
        {
            _syncingFixTol = false;
        }
        UpdateFixTolSummary();
    }

    private void ApplyFixPreset(decimal posMm, decimal angleDeg, int shapeIndex)
    {
        _syncingFixTol = true;
        try
        {
            _numFixPos.Value = posMm;
            _numFixAngle.Value = angleDeg;
            _numFixTheta.Value = Math.Clamp(angleDeg, _numFixTheta.Minimum, _numFixTheta.Maximum);
            _trkFixPos.Value = ClampTrack(_trkFixPos, posMm * 2);
            _trkFixAngle.Value = ClampTrack(_trkFixAngle, angleDeg * 2);
            _numFixDx.Value = posMm;
            _numFixDy.Value = posMm;
        }
        finally
        {
            _syncingFixTol = false;
        }
        _cboFixShape.SelectedIndex = shapeIndex;
        UpdateFixTolSummary();
    }

    private static bool Near(decimal a, decimal b) => Math.Abs(a - b) < 0.0001m;

    private void UpdateFixShapeSelection()
    {
        int index = 3;
        if (Near(_numFixIou.Value, 0.80m) && Near(_numFixDrop.Value, 0.15m)) index = 0;
        else if (Near(_numFixIou.Value, 0.85m) && Near(_numFixDrop.Value, 0.10m)) index = 1;
        else if (Near(_numFixIou.Value, 0.90m) && Near(_numFixDrop.Value, 0.05m)) index = 2;

        bool was = _syncingFixTol;
        _syncingFixTol = true;
        try
        {
            _cboFixShape.SelectedIndex = index;
        }
        finally
        {
            _syncingFixTol = was;
        }
    }

    private void UpdateFixTolSummary()
    {
        if (_lblFixTolSummary == null || _numFixPos == null) return;
        string confirm = _numFixConfirm.Value <= 1
            ? "NG 即报警"
            : $"连续 {_numFixConfirm.Value} 帧 NG 才报警";
        _lblFixTolSummary.Text = _cboFixJudgeMode != null && _cboFixJudgeMode.SelectedIndex == 1
            ? $"当前：安全区模式 / 边距 ≥{_numFixMarginPx.Value:0} px / 允许越界 {_numFixOverflowPx.Value:0} px / {confirm}"
            : $"当前：位置 ±{_numFixPos.Value:0.#} mm / 角度 ±{_numFixTheta.Value:0.#}° / IoU ≥{_numFixIou.Value:0.00} / {confirm}";
    }

    // ---------------------------------------------------------------- lifecycle

    private void OnFixtureTabChanged()
    {
        if (_tabs == null || _pageFixture == null) return;
        bool on = ReferenceEquals(_tabs.SelectedTab, _pageFixture);
        if (on == _fixtureActive) return;
        _fixtureActive = on;
        if (on)
        {
            if (_cboFixModel.Items.Count == 0) LoadFixModels();
            // 默认不开相机：等用户点“打开相机”，避免一进页就占住设备。
        }
        else
        {
            CloseFixCamera();
        }
    }

    private void LoadFixModels()
    {
        var models = FixtureStore.ListModels();
        if (models.Count == 0)
        {
            var def = FixtureRecipe.CreateDefault("B001");
            FixtureStore.EnsureModel("B001");
            FixtureStore.Save(def);
            models = new[] { "B001" };
        }

        _fixModelSync = true;
        try
        {
            _cboFixModel.Items.Clear();
            foreach (var m in models) _cboFixModel.Items.Add(m);
            _cboFixModel.SelectedIndex = 0;
        }
        finally
        {
            _fixModelSync = false;
        }

        var model = _cboFixModel.SelectedItem as string;
        if (!string.IsNullOrWhiteSpace(model)) LoadFixRecipe(model);
    }

    private void OnFixtureModelChanged()
    {
        if (_fixModelSync) return;
        var model = _cboFixModel.SelectedItem as string;
        if (!string.IsNullOrWhiteSpace(model)) LoadFixRecipe(model);
    }

    private void LoadFixRecipe(string model)
    {
        _fixModel = model;
        _fixRecipe = FixtureStore.LoadOrCreate(model);

        _fixGolden?.Dispose();
        _fixGolden = FixtureStore.LoadImage(model, FixtureStore.GoldenFile);
        _fixMask?.Dispose();
        _fixMask = FixtureStore.LoadImage(model, FixtureStore.GoldenMaskFile);
        _fixEmpty?.Dispose();
        _fixEmpty = FixtureStore.LoadImage(model, FixtureStore.EmptyFile);

        SyncFixUiFromRecipe();
        LoadSegParamsToUi();
        RestoreCalibrationLine();
        UpdateFixTemplateInfo();
        UpdateFixZoneInfo();
        UpdateFixThumbs();
        UpdateFixCalibInfo();
        UpdateFixRoiInfo();
        SetFixVerdict(null);
        ApplyFixRoiOverlay();
    }

    private void SyncFixUiFromRecipe()
    {
        var r = _fixRecipe;
        if (r == null) return;

        int camIndex = Math.Clamp(r.Camera.Index, 0, 5);
        _cboFixCamIndex.SelectedIndex = camIndex;

        int resIndex = Array.FindIndex(FixResolutions, x => x.W == r.Camera.Width && x.H == r.Camera.Height);
        _cboFixRes.SelectedIndex = resIndex >= 0 ? resIndex : 1;

        var fourcc = (r.Camera.FourCC ?? "MJPG").ToUpperInvariant();
        _cboFixFourCC.SelectedIndex = fourcc == "YUYV" ? 1 : 0;

        _chkFixExposure.Checked = r.Camera.Exposure.HasValue;
        if (r.Camera.Exposure is double exp)
            _numFixExposure.Value = Math.Clamp((decimal)exp, _numFixExposure.Minimum, _numFixExposure.Maximum);
        if (r.Camera.Gain is double gain)
            _numFixGain.Value = Math.Clamp((decimal)gain, _numFixGain.Minimum, _numFixGain.Maximum);

        _numFixDx.Value = Clamp((decimal)r.Tolerance.DxMm, _numFixDx);
        _numFixDy.Value = Clamp((decimal)r.Tolerance.DyMm, _numFixDy);
        _numFixTheta.Value = Clamp((decimal)r.Tolerance.DThetaDeg, _numFixTheta);
        _numFixIou.Value = Clamp((decimal)r.Tolerance.IoUMin, _numFixIou);
        _numFixAreaMin.Value = Clamp((decimal)r.Tolerance.AreaRatioMin, _numFixAreaMin);
        _numFixAreaMax.Value = Clamp((decimal)r.Tolerance.AreaRatioMax, _numFixAreaMax);
        _numFixZone.Value = Clamp((decimal)r.Tolerance.ZoneMarginMm, _numFixZone);
        _numFixConfirm.Value = Clamp(r.ConfirmFrames, _numFixConfirm);
        _numFixMeanGray.Value = Clamp((decimal)r.Quality.MinMeanGray, _numFixMeanGray);
        _numFixContrast.Value = Clamp((decimal)r.Quality.MinContrast, _numFixContrast);
        _numFixHighlight.Value = Clamp((decimal)r.Quality.MaxHighlightRatio, _numFixHighlight);
        _numFixContour.Value = Clamp((decimal)r.Tolerance.MinContourScore, _numFixContour);
        _numFixDrop.Value = Clamp((decimal)r.Tolerance.ContourScoreDrop, _numFixDrop);
        _numFixMarginPx.Value = Clamp(r.Zone?.MarginPx ?? 10, _numFixMarginPx);
        _numFixOverflowPx.Value = Clamp(r.Zone?.OverflowPx ?? 0, _numFixOverflowPx);
        _cboFixJudgeMode.SelectedIndex = string.Equals(r.JudgeMode, "zone", StringComparison.OrdinalIgnoreCase) ? 1 : 0;

        _syncingFixTol = true;
        try
        {
            decimal pos = (decimal)Math.Min(r.Tolerance.DxMm, r.Tolerance.DyMm);
            _numFixPos.Value = Clamp(pos, _numFixPos);
            _trkFixPos.Value = ClampTrack(_trkFixPos, _numFixPos.Value * 2);
            _numFixAngle.Value = Clamp((decimal)r.Tolerance.DThetaDeg, _numFixAngle);
            _trkFixAngle.Value = ClampTrack(_trkFixAngle, _numFixAngle.Value * 2);
            UpdateFixShapeSelection();
        }
        finally
        {
            _syncingFixTol = false;
        }

        UpdateFixTolSummary();
        UpdateFixGoldenScore();
    }

    private static decimal Clamp(decimal value, NumericUpDown box) =>
        Math.Clamp(value, box.Minimum, box.Maximum);

    private void ToggleFixtureMode()
    {
        _fixDebug = !_fixDebug;
        _pnlFixExec.Visible = !_fixDebug;
        _pnlFixDebug.Visible = _fixDebug;
        _btnFixMode.Text = _fixDebug ? "切到执行模式" : "切到调试模式";
    }

    // ---------------------------------------------------------------- camera

    private FixtureCameraConfig BuildCameraConfig()
    {
        var cfg = new FixtureCameraConfig
        {
            Index = int.TryParse(_cboFixCamIndex.SelectedItem as string, out var idx) ? idx : 0,
            FourCC = _cboFixFourCC.SelectedItem as string ?? "MJPG",
        };

        int res = _cboFixRes.SelectedIndex;
        if (res >= 0 && res < FixResolutions.Length)
        {
            cfg.Width = FixResolutions[res].W;
            cfg.Height = FixResolutions[res].H;
        }

        if (_chkFixExposure.Checked) cfg.Exposure = (double)_numFixExposure.Value;
        if (_numFixGain.Value > 0) cfg.Gain = (double)_numFixGain.Value;
        return cfg;
    }

    private async Task<bool> OpenFixCameraAsync(bool quiet)
    {
        if (_fixOpening)
        {
            SetFixCameraInfo("正在打开相机，请稍候…");
            return false;
        }

        if (_capSource == CaptureSource.Camera || _capSource == CaptureSource.Video)
        {
            SetFixCameraInfo("识别测试页正在占用摄像头，请先停止");
            return false;
        }

        CloseFixCamera();

        var cfg = BuildCameraConfig();
        _fixOpening = true;
        if (_btnFixCamApply != null) _btnFixCamApply.Enabled = false;
        UseWaitCursor = true;
        SetFixCameraInfo($"正在打开相机 {cfg.Index}…");
        try
        {
            var opened = await Task.Run(() =>
            {
                var cam = new FixtureCamera();
                if (!cam.Open(cfg, out var error))
                {
                    cam.Dispose();
                    return (Cam: (FixtureCamera)null, Error: error, Frame: (Mat)null);
                }

                return (Cam: cam, Error: "", Frame: cam.WaitNewFrame(0, 2500));
            });

            if (opened.Cam == null)
            {
                SetFixCameraInfo("打开失败：" + opened.Error);
                if (!quiet) Log.Warn("针床检测相机打开失败: " + opened.Error);
                return false;
            }

            _fixCam = opened.Cam;
            _fixSnap?.Dispose();
            _fixSnap = null;
            _fixSnapFrameId = 0;
            _fixFrameWatermark = 0;
            _fixShownFrame = -1;
            _fixFrozen = false;
            _fixHold = false;
            _fixTemplateView = false;
            _fixFreezeUntil = DateTime.MinValue;
            ShowCalibLine(false);
            UpdateFixSnapButtons();

            var frame = opened.Frame;
            if (frame == null)
            {
                SetFixCameraInfo($"相机 {cfg.Index} 已打开，但 2.5 秒内没有画面");
            }
            else
            {
                try
                {
                    _fixShownFrame = 1;
                    ShowFixFrame(frame);
                    SetFixCameraInfo($"相机 {cfg.Index} 已打开 {opened.Cam.Width}×{opened.Cam.Height}");
                }
                finally
                {
                    frame.Dispose();
                }
            }

            _fixTimer?.Start();
            PersistCameraConfig(cfg);
            return true;
        }
        catch (Exception ex)
        {
            Log.Error("打开针床相机失败", ex);
            SetFixCameraInfo("打开失败：" + ex.Message);
            return false;
        }
        finally
        {
            _fixOpening = false;
            UseWaitCursor = false;
            if (_btnFixCamApply != null) _btnFixCamApply.Enabled = true;
        }
    }

    /// <summary>只有相机参数真的变了才写配方，避免每次打开就刷一份备份。</summary>
    private void PersistCameraConfig(FixtureCameraConfig cfg)
    {
        if (_fixRecipe == null) return;
        var cur = _fixRecipe.Camera;
        if (cur.Index == cfg.Index && cur.Width == cfg.Width && cur.Height == cfg.Height &&
            string.Equals(cur.FourCC, cfg.FourCC, StringComparison.OrdinalIgnoreCase) &&
            Nullable.Equals(cur.Exposure, cfg.Exposure) && Nullable.Equals(cur.Gain, cfg.Gain))
        {
            return;
        }

        _fixRecipe.Camera = cfg;
        TrySaveQuiet();
    }

    private void CloseFixCamera()
    {
        _fixTimer?.Stop();
        _fixCam?.Dispose();
        _fixCam = null;
        _fixShownFrame = -1;
        _fixSnapFrameId = 0;
        _fixFrameWatermark = 0;
        _fixFreezeUntil = DateTime.MinValue;
        _fixFrozen = false;
        _fixHold = false;
        _fixTemplateView = false;
        _fixRoiPicking = false;
        if (_picFixture != null) _picFixture.Editable = false;
        _fixSnap?.Dispose();
        _fixSnap = null;
        ShowCalibLine(false);
        UpdateFixSnapButtons();
    }

    /// <summary>拍照：把当前画面定格在界面上（只进内存，不落盘），单张测试判定这张照片。</summary>
    private async Task TakeFixSnapshotAsync()
    {
        if (_fixJudging)
        {
            SetFixCameraInfo("正在检测，完成后再拍照");
            return;
        }

        if (_fixCam == null && !await OpenFixCameraAsync(false)) return;

        var cam = _fixCam;
        if (cam == null) return;

        // 上一次的定格先清掉：否则“重拍”会拿同一帧，看起来像没反应。
        _fixSnap?.Dispose();
        _fixSnap = null;
        _fixSnapFrameId = 0;
        _fixFrozen = false;
        _fixHold = false;
        _fixTemplateView = false;
        _fixFreezeUntil = DateTime.MinValue;
        UpdateFixSnapButtons();

        SetFixCameraInfo("正在拍照…");
        long frameId = 0;
        var frame = await Task.Run(() => WaitFreshFrame(cam, 1500, out frameId));
        if (frame == null)
        {
            // 1.5 秒没等到新帧时退一步用最新缓存帧，至少不会让按钮看起来失效；
            // 连缓存帧都没有才是真的没画面。
            frame = await Task.Run(() => cam.TryGetLatest(out var latest, out frameId) ? latest : null);
        }

        if (frame == null)
        {
            SetFixCameraInfo("拍照失败：没有取到画面，检查相机是否被其它程序占用");
            return;
        }

        try
        {
            _fixSnap = frame.Clone();
            _fixSnapFrameId = frameId;
            _fixFrameWatermark = Math.Max(_fixFrameWatermark, frameId);
            ShowFixFrame(frame);
            _fixFrozen = true;
            _fixHold = true;
            _fixFreezeUntil = DateTime.MaxValue;
            UpdateFixSnapButtons();
            SetFixCameraInfo($"已拍照（仅内存，未保存）{frame.Width}×{frame.Height}；单张测试判定这张照片，点“恢复预览”回到实时画面");
        }
        finally
        {
            frame.Dispose();
        }
    }

    /// <summary>只返回开始等待之后产生的新帧，避免“拍到的是上一张缓存”。</summary>
    private static Mat WaitFreshFrame(FixtureCamera cam, int timeoutMs, out long frameId)
    {
        frameId = 0;
        if (cam == null) return null;
        var frame = cam.WaitNewFrame(0, timeoutMs, out var id);
        frameId = id;
        return frame;
    }

    /// <summary>从定格照片回到实时预览，并丢弃内存里的照片。</summary>
    private void ResumeFixPreview()
    {
        _fixSnap?.Dispose();
        _fixSnap = null;
        _fixSnapFrameId = 0;
        _fixFrozen = false;
        _fixHold = false;
        _fixTemplateView = false;
        _fixFreezeUntil = DateTime.MinValue;
        ShowCalibLine(false);
        UpdateFixSnapButtons();
        SetFixVerdict("待检测");
        SetFixCameraInfo(_fixCam == null ? "相机未打开" : "已恢复实时预览");
    }

    /// <summary>
    /// 另存为对话框是模态的，但模态循环照样派发定时器消息：针床页 66ms 一跳的实时预览
    /// （换帧 + 释放旧帧 + 可能的分割预览）会一直占着 UI 线程，对话框画不出来就成了"整窗发白 + 未响应"。
    /// 保存期间把预览停掉，关掉对话框再按原状态恢复。
    /// </summary>
    private void SetFixPreviewBusy(bool busy)
    {
        if (_fixTimer == null) return;
        if (busy)
        {
            _fixPreviewHeld = _fixTimer.Enabled;
            _fixTimer.Stop();
            Log.Info($"保存对话框打开：暂停针床预览（相机={_fixCam != null}，分割预览={_fixSegPreview}，定格={_fixFrozen}）");
        }
        else
        {
            if (_fixPreviewHeld) _fixTimer.Start();
            _fixPreviewHeld = false;
            Log.Info("保存对话框关闭：恢复针床预览");
        }
    }

    private void UpdateFixSnapButtons()
    {
        if (_btnFixLive != null) _btnFixLive.Enabled = _fixFrozen || _fixHold;
        if (_btnFixSnapshot != null) _btnFixSnapshot.Text = (_fixFrozen || _fixHold) ? "重拍" : "拍照";
    }

    /// <summary>单张测试：有定格照片就判照片，否则判一帧相机画面。</summary>
    private async Task JudgeTestAsync()
    {
        if (_fixSnap != null)
        {
            using var frame = _fixSnap.Clone();
            await RunJudgeAsync(frame);
            return;
        }

        // 正在看模板时点单张测试，先回到实时画面，避免判的是模板却显示实时结果。
        if (_fixTemplateView || _fixHold) ResumeFixPreview();
        await JudgeAsync(null);
    }

    private void SetFixCameraInfo(string text)
    {
        if (_lblFixCamInfo == null) return;
        _lblFixCamInfo.Text = text;
        _lblFixCamInfo.ForeColor = text.Contains("失败") || text.Contains("占用") ? Theme.Danger : Theme.Muted;
    }

    private void FixPreviewTick()
    {
        if (_fixCam == null || _fixHold) return;
        if (_fixFrozen || DateTime.Now < _fixFreezeUntil) return;
        if (!_fixCam.TryGetLatest(out var frame, out var id)) return;
        try
        {
            if (id == _fixShownFrame) return;
            _fixShownFrame = id;
            _fixFrameWatermark = Math.Max(_fixFrameWatermark, id);
            ShowLiveFixFrame(frame);
        }
        finally
        {
            frame.Dispose();
        }
    }

    private void ShowFixFrame(Mat frame)
    {
        if (frame == null || frame.Empty()) return;
        var bmp = MatToBitmap(frame);
        var old = _picFixture.Image;
        _picFixture.SetFrame(bmp);
        old?.Dispose();
        if (!_fixRoiPicking) ApplyFixRoiOverlay();
    }

    /// <summary>实时画面：不要画标定线（标定线会让预览看起来像故障），静帧才画。</summary>
    private void ShowLiveFixFrame(Mat frame)
    {
        ShowCalibLine(false);
        if (_fixSegPreview && _fixRecipe != null)
        {
            using var view = BuildSegPreview(frame, out _, out _);
            if (view != null)
            {
                ShowFixFrame(view);
                return;
            }
        }

        ShowFixFrame(frame);
    }

    /// <summary>把当前 ROI 和安全区画在画布上；都没有时清空叠加框。</summary>
    private void ApplyFixRoiOverlay()
    {
        if (_picFixture?.Image == null || _fixRecipe == null) return;
        if (_fixRoiPicking || _fixZonePicking) return;

        // 叠加框只在工具操作后短暂保留：过点就撤掉，实时画面恢复干净。要常看就点工具栏“显示叠加”。
        if (_fixSegPreview || DateTime.Now > _fixOverlayUntil)
        {
            if (_picFixture.Boxes.Count > 0) _picFixture.Boxes = Array.Empty<BoxRecord>();
            return;
        }

        int fw = _picFixture.Image.Width;
        int fh = _picFixture.Image.Height;
        var boxes = new List<BoxRecord>();

        var zone = _fixRecipe.Zone;
        bool zoneOn = zone != null && zone.Enabled && zone.W >= 16 && zone.H >= 16;

        // 安全区模式只显示安全区，避免两个框的标签叠在一起。
        var roi = _fixRecipe.Roi ?? new FixtureRoi();
        if (!roi.IsFull && !zoneOn)
        {
            var r = roi.ToPixels(fw, fh);
            boxes.Add(new BoxRecord
            {
                Cn = "检测区", En = "roi", X = r.X, Y = r.Y, W = r.Width, H = r.Height,
                Angle = 0, Confirmed = true, Conf = 1,
            });
        }

        if (zoneOn)
        {
            var z = zone.ToRect(fw, fh);
            boxes.Add(new BoxRecord
            {
                Cn = "安全区", En = "zone", X = z.X, Y = z.Y, W = z.Width, H = z.Height,
                Angle = 0, Confirmed = true, Conf = 1,
            });
        }

        _picFixture.Boxes = boxes.ToArray();
    }

    private void UpdateFixRoiInfo()
    {
        if (_lblFixRoi == null || _fixRecipe == null) return;
        var roi = _fixRecipe.Roi ?? new FixtureRoi();
        _lblFixRoi.Text = roi.IsFull
            ? "当前 ROI：整幅画面（建议框到针床区域，排除台面杂物）"
            : $"当前 ROI：X {roi.X:P0} Y {roi.Y:P0} W {roi.W:P0} H {roi.H:P0}";
    }

    private void ShowCameraPropertyPage()
    {
        if (_fixCam == null)
        {
            SetFixCameraInfo("先打开相机，再进驱动属性页");
            return;
        }

        MessageBox.Show(this,
            "普通 USB 相机没有统一的属性页接口。请用相机厂商工具关闭自动曝光/自动增益，" + Environment.NewLine +
            "或换一台支持 UVC 属性页的相机（如海康、Basler、大恒）。" + Environment.NewLine + Environment.NewLine +
            "本程序的\u201c手动曝光\u201d只是尽力写入，写不进去不代表生效。",
            "锁曝光", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    // ---------------------------------------------------------------- calibration

    /// <summary>显示/隐藏标定线。实时预览时隐藏，只有静帧、模板和拾取时才画。</summary>
    private void ShowCalibLine(bool show)
    {
        if (!show || _fixCalibLineA == null || _fixCalibLineB == null)
        {
            _picFixture.LineA = null;
            _picFixture.LineB = null;
            return;
        }

        _picFixture.LineA = _fixCalibLineA;
        _picFixture.LineB = _fixCalibLineB;
    }

    /// <summary>从配方恢复标定线坐标和参考像素长度，重启后还能用“重算并保存”。</summary>
    private void RestoreCalibrationLine()
    {
        var c = _fixRecipe?.Calibration;
        _fixCalibPx = c?.RefLengthPx ?? 0;
        if (c != null && c.RefLengthPx > 0 && c.LineBX > 0 && c.LineBY > 0)
        {
            _fixCalibLineA = new Point(c.LineAX, c.LineAY);
            _fixCalibLineB = new Point(c.LineBX, c.LineBY);
        }
        else
        {
            _fixCalibLineA = null;
            _fixCalibLineB = null;
        }

        ShowCalibLine(false);
    }

    private void BeginCalibPick()
    {
        if (_picFixture.Image == null)
        {
            SetFixCameraInfo("先打开相机或回放一张图片，再拾取标尺");
            return;
        }

        _fixCalibPicking = true;
        _fixHold = false;
        _fixTemplateView = false;
        ShowCalibLine(false);
        _picFixture.LineEditable = true;
        _picFixture.Focus();
        SetFixCameraInfo("请在画面里点直尺两端（第一下起点，第二下终点）");
    }

    private void OnFixtureLineDrawn(Point a, Point b)
    {
        if (!_fixCalibPicking) return;
        _fixCalibPicking = false;
        _picFixture.LineEditable = false;
        _fixCalibLineA = a;
        _fixCalibLineB = b;
        ShowCalibLine(true);

        _fixCalibPx = Math.Sqrt(Math.Pow(b.X - a.X, 2) + Math.Pow(b.Y - a.Y, 2));
        if (_fixCalibPx < 5)
        {
            SetFixCameraInfo("两点太近，重新拾取");
            return;
        }

        var mm = PromptNumber("标定", "这两点之间的真实长度（mm）",
            _numFixCalibMm.Value, 1, 2000, 1);
        if (mm == null)
        {
            ShowCalibLine(false);
            SetFixCameraInfo("标定未保存，已收起标定线");
            return;
        }

        _numFixCalibMm.Value = mm.Value;
        SaveCalibration();
    }

    private void SaveCalibration()
    {
        if (_fixRecipe == null)
        {
            SetFixCameraInfo("先选择机种");
            return;
        }

        double px = _fixCalibPx;
        if (px <= 0) px = _fixRecipe.Calibration.RefLengthPx;
        if (px <= 0)
        {
            SetFixCameraInfo("还没有拾取两点，无法换算 mm/像素");
            return;
        }

        double mm = (double)_numFixCalibMm.Value;
        var c = _fixRecipe.Calibration;
        c.MmPerPixel = mm / px;
        c.RefLengthMm = mm;
        c.RefLengthPx = px;
        c.CalibratedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        if (_fixCalibLineA is Point la && _fixCalibLineB is Point lb)
        {
            c.LineAX = la.X;
            c.LineAY = la.Y;
            c.LineBX = lb.X;
            c.LineBY = lb.Y;
        }

        TrySaveQuiet();
        // 标定线只用于拾取/核对；保存后撤掉，工具做完就回干净画面。
        ShowCalibLine(false);
        UpdateFixCalibInfo();
        SetFixCameraInfo($"标定已保存：{c.MmPerPixel:F4} mm/像素（参考 {c.RefLengthMm:F1} mm）；已收起标定线");
    }

    private void UpdateFixCalibInfo()
    {
        if (_lblFixCalib == null || _fixRecipe == null) return;
        var c = _fixRecipe.Calibration;
        _lblFixCalib.Text = c.MmPerPixel > 0
            ? $"已标定：{c.MmPerPixel:F4} mm/像素（参考 {c.RefLengthMm:F1} mm / {c.RefLengthPx:F1} px）\r\n" +
              $"在 1280 宽画面里，1 像素约 {c.MmPerPixel:F3} mm。"
            : "尚未标定。没有 mm/像素 时，位移和越界只能用像素判断。";
    }

    // ---------------------------------------------------------------- templates

    private async Task CaptureTemplateAsync(bool golden)
    {
        if (_fixRecipe == null) return;
        if (_fixCapturing) return;

        _fixCapturing = true;
        _btnFixCaptureEmpty.Enabled = false;
        _btnFixCaptureGolden.Enabled = false;
        try
        {
            // 采模板前先退出“定格/看模板”状态，保证拿到的是当前实时画面。
            ResumeFixPreview();

            var frame = await GrabFixFrameAsync(1500);
            if (frame == null)
            {
                SetFixCameraInfo("取不到画面，先打开相机");
                return;
            }

            string model = _fixModel;
            bool hasGolden = false;
            bool hasEmpty = false;
            try
            {
                await Task.Run(() =>
                {
                    if (golden)
                        FixtureStore.SaveImage(model, FixtureStore.GoldenFile, frame);
                    else
                        FixtureStore.SaveImage(model, FixtureStore.EmptyFile, frame);

                    // 两张模板谁先谁后都行：每次采完都按磁盘上的两张重建一次掩码。
                    // 先采金样板时还没有空治具，掩码只按颜色分割；补采空治具后这里会自动补上差分。
                    using var g = FixtureStore.LoadImage(model, FixtureStore.GoldenFile);
                    using var e = FixtureStore.LoadImage(model, FixtureStore.EmptyFile);
                    if (g != null)
                    {
                        // 分割参数只跟金样板有关，只有采金样板时才重新估计。
                        if (golden) _fixRecipe.Segment = FixtureAnalyzer.EstimateSegment(g, _fixRecipe);
                        using var mask = FixtureAnalyzer.BuildMask(g, _fixRecipe, e);
                        FixtureStore.SaveImage(model, FixtureStore.GoldenMaskFile, mask);
                    }

                    _fixRecipe.Version++;
                    FixtureStore.Save(_fixRecipe);
                    hasGolden = g != null;
                    hasEmpty = e != null;
                });

                _fixEmpty?.Dispose();
                _fixEmpty = FixtureStore.LoadImage(model, FixtureStore.EmptyFile);
                _fixGolden?.Dispose();
                _fixGolden = FixtureStore.LoadImage(model, FixtureStore.GoldenFile);
                _fixMask?.Dispose();
                _fixMask = FixtureStore.LoadImage(model, FixtureStore.GoldenMaskFile);
                UpdateFixGoldenScore();

                // 工具做完就回实时画面：模板和标定线不留在画面上，要回看点顶部缩略图。
                ResumeFixPreview();
                UpdateFixTemplateInfo();
                UpdateFixThumbs();
                SetFixVerdict(null);
                string missing = !hasGolden
                    ? "还缺金样板：放正产品后采金样板，掩码才会生成"
                    : !hasEmpty
                        ? "还缺空治具：补采空治具后掩码会自动重建"
                        : "两张模板齐了，掩码已按当前两张重建";
                SetFixCameraInfo($"{(golden ? "金样板" : "空治具")}已保存；{missing}；已回到实时画面，点顶部缩略图可回看模板");
            }
            finally
            {
                frame.Dispose();
            }
        }
        finally
        {
            _fixCapturing = false;
            _btnFixCaptureEmpty.Enabled = true;
            _btnFixCaptureGolden.Enabled = true;
        }
    }

    /// <summary>采集模板期间把提示写在大标签下，右侧日志保留历史。</summary>
    private void FeedbackFixTemplate(string text)
    {
        if (_lblFixReason != null) _lblFixReason.Text = text;
    }

    private async Task RebuildSegmentAsync()
    {
        if (_fixRecipe == null || _fixGolden == null)
        {
            SetFixCameraInfo("先采集金样板");
            return;
        }

        var recipe = _fixRecipe;
        var golden = _fixGolden;
        await Task.Run(() =>
        {
            var seg = FixtureAnalyzer.EstimateSegment(golden, recipe);
            recipe.Segment = seg;
            using var mask = FixtureAnalyzer.BuildMask(golden, recipe, _fixEmpty);
            FixtureStore.SaveImage(recipe.Model, FixtureStore.GoldenMaskFile, mask);
            FixtureStore.Save(recipe);
        });

        _fixMask?.Dispose();
        _fixMask = FixtureStore.LoadImage(_fixModel, FixtureStore.GoldenMaskFile);
        UpdateFixGoldenScore();
        UpdateFixTemplateInfo();
        LoadSegParamsToUi();
        if (_fixHold || _fixCam == null) ShowSegStaticPreview();
        SetFixCameraInfo("已按金样板重建分割参数，已同步到【分割参数】页");
    }

    private void UpdateFixGoldenScore()
    {
        if (_lblFixGoldenScore == null) return;
        double score = _fixMask == null ? 0 : FixtureAnalyzer.GoldenContourScore(_fixMask);
        _lblFixGoldenScore.Text = score > 0 ? $"金样板 {score:F3}" : "金样板未采集";
        _lblFixGoldenScore.ForeColor = Theme.Muted;
    }

    private void UpdateFixTemplateInfo()
    {
        if (_lblFixTemplate == null) return;
        bool empty = _fixEmpty != null;
        bool golden = _fixGolden != null;
        bool mask = _fixMask != null;
        string maskText = !mask
            ? "未生成（先采金样板）"
            : empty ? "已生成（含空图差分）" : "已生成（暂无空图，只按颜色）";
        _lblFixTemplate.Text =
            $"空治具：{(empty ? "已采集" : "未采集")}\r\n" +
            $"金样板：{(golden ? "已采集" : "未采集")}\r\n" +
            $"分割掩码：{maskText}\r\n" +
            $"配方版本：v{_fixRecipe?.Version ?? 0}";
    }

    // ---------------------------------------------------------------- zone

    /// <summary>按金样板轮廓自动生成一版安全区：金样板外框四周各留一点余量，之后可手调。</summary>
    private void AutoGenerateZone()
    {
        if (_fixRecipe == null)
        {
            SetFixCameraInfo("请先选择机种");
            return;
        }

        if (_fixMask == null || _fixMask.Empty())
        {
            SetFixCameraInfo("先采集金样板，才能按它生成安全区");
            return;
        }

        using var bin = new Mat();
        using var gray = new Mat();
        if (_fixMask.Channels() == 1) _fixMask.CopyTo(gray);
        else OpenCvSharp.Cv2.CvtColor(_fixMask, gray, OpenCvSharp.ColorConversionCodes.BGR2GRAY);
        OpenCvSharp.Cv2.Threshold(gray, bin, 127, 255, OpenCvSharp.ThresholdTypes.Binary);
        OpenCvSharp.Cv2.FindContours(bin, out OpenCvSharp.Point[][] contours, out _,
            OpenCvSharp.RetrievalModes.External, OpenCvSharp.ContourApproximationModes.ApproxSimple);
        if (contours.Length == 0)
        {
            SetFixCameraInfo("金样板掩码里没有轮廓，先重建分割参数");
            return;
        }

        var largest = contours.OrderByDescending(c => OpenCvSharp.Cv2.ContourArea(c)).First();
        var box = OpenCvSharp.Cv2.BoundingRect(largest);
        // 分割边缘本身有 ±几像素抖动，外扩太小会让“同一块板”也判成越界。
        int pad = Math.Max(24, (int)Math.Round(Math.Max(box.Width, box.Height) * 0.035));
        int fw = _fixMask.Width, fh = _fixMask.Height;
        int x = Math.Max(0, box.X - pad);
        int y = Math.Max(0, box.Y - pad);
        int w = Math.Min(fw - x, box.Width + pad * 2);
        int h = Math.Min(fh - y, box.Height + pad * 2);

        _fixRecipe.Zone = new FixtureZoneConfig
        {
            Enabled = true,
            X = x,
            Y = y,
            W = w,
            H = h,
            MarginPx = (int)_numFixMarginPx.Value,
            OverflowPx = (int)_numFixOverflowPx.Value,
        };
        _fixRecipe.JudgeMode = "zone";
        if (_cboFixJudgeMode.SelectedIndex != 1) _cboFixJudgeMode.SelectedIndex = 1;
        TrySaveQuiet();
        UpdateFixZoneInfo();
        _fixOverlayUntil = DateTime.Now.AddSeconds(4);
        ApplyFixRoiOverlay();
        SetFixCameraInfo($"安全区已按金样板生成：{w}×{h} px（四周外扩 {pad}px）；可直接用，也可点“框选安全区”手调");
    }

    private void BeginZonePick()
    {
        if (_fixRecipe == null)
        {
            SetFixCameraInfo("请先选择机种");
            return;
        }

        if (_picFixture.Image == null)
        {
            SetFixCameraInfo("先打开相机或拍一张照，再画安全区");
            return;
        }

        _picFixture.Boxes = Array.Empty<BoxRecord>();
        _picFixture.ClearPendingBox();
        _fixOverlayUntil = DateTime.MaxValue;
        _fixZonePicking = true;
        _picFixture.Editable = true;
        _picFixture.Focus();
        SetFixCameraInfo("请按住左键拖出安全区（产品允许出现的最大范围），松开即保存");
    }

    private void ClearZone()
    {
        if (_fixRecipe == null) return;
        _fixRecipe.Zone = new FixtureZoneConfig();
        TrySaveQuiet();
        UpdateFixZoneInfo();
        _fixOverlayUntil = DateTime.Now.AddSeconds(4);
        ApplyFixRoiOverlay();
        SetFixCameraInfo("安全区已清除");
    }

    private void OnFixJudgeModeChanged()
    {
        if (_fixRecipe == null || _numFixMarginPx == null) return;
        bool zone = _cboFixJudgeMode.SelectedIndex == 1;
        _numFixMarginPx.Enabled = zone;
        _numFixOverflowPx.Enabled = zone;
        UpdateFixTolSummary();
    }

    private void UpdateFixZoneInfo()
    {
        if (_lblFixZone == null) return;
        var z = _fixRecipe?.Zone;
        _lblFixZone.Text = z != null && z.Enabled && z.W >= 16
            ? $"安全区：X {z.X} Y {z.Y} {z.W}×{z.H} px / 边距 ≥{z.MarginPx}px / 越界 ≤{z.OverflowPx}px"
            : "安全区：未设置（安全区模式必须先生成或手画）";
    }

    // ---------------------------------------------------------------- roi

    private void BeginRoiPick()
    {
        if (_fixRecipe == null)
        {
            SetFixCameraInfo("请先选择机种");
            return;
        }

        if (_picFixture.Image == null)
        {
            SetFixCameraInfo("先打开相机或拍一张照，再框选检测区");
            return;
        }

        // 框选期间先撤掉画布上的旧 ROI 框：否则从旧框里起手会被画布当成“编辑已有框”，
        // 页面不处理 BoxEdited，看起来就是框选没反应。
        _picFixture.Boxes = Array.Empty<BoxRecord>();
        _picFixture.ClearPendingBox();
        _fixOverlayUntil = DateTime.MaxValue;
        _fixRoiPicking = true;
        _picFixture.Editable = true;
        _picFixture.Focus();
        SetFixCameraInfo("请按住左键拖出检测区（只框针床和产品），松开即生效；画完后到“模板”页重建掩码");
    }

    private void OnFixtureBoxDrawn(object sender, BoxDraft draft)
    {
        if (_fixRecipe == null) return;
        if (!_fixRoiPicking && !_fixZonePicking) return;
        var image = _picFixture.Image;
        if (image == null) return;

        if (_fixZonePicking)
        {
            _fixZonePicking = false;
            _picFixture.Editable = false;
            _picFixture.ClearPendingBox();

            var zb = draft.Bounds;
            if (zb.Width < 16 || zb.Height < 16)
            {
                SetFixCameraInfo("安全区太小，重新框选");
                return;
            }

            _fixRecipe.Zone = new FixtureZoneConfig
            {
                Enabled = true,
                X = Math.Clamp(zb.X, 0, image.Width - 16),
                Y = Math.Clamp(zb.Y, 0, image.Height - 16),
                W = Math.Min(zb.Width, image.Width - Math.Clamp(zb.X, 0, image.Width - 16)),
                H = Math.Min(zb.Height, image.Height - Math.Clamp(zb.Y, 0, image.Height - 16)),
                MarginPx = (int)_numFixMarginPx.Value,
                OverflowPx = (int)_numFixOverflowPx.Value,
            };
            _fixRecipe.JudgeMode = "zone";
            if (_cboFixJudgeMode.SelectedIndex != 1) _cboFixJudgeMode.SelectedIndex = 1;
            TrySaveQuiet();
            UpdateFixZoneInfo();
            _fixOverlayUntil = DateTime.Now.AddSeconds(4);
            ApplyFixRoiOverlay();
            SetFixCameraInfo($"安全区已保存：{_fixRecipe.Zone.W}×{_fixRecipe.Zone.H} px，判定方式已切到安全区模式");
            return;
        }

        _fixRoiPicking = false;
        _picFixture.Editable = false;
        _picFixture.ClearPendingBox();

        var bounds = draft.Bounds;
        if (bounds.Width < 16 || bounds.Height < 16)
        {
            SetFixCameraInfo("ROI 太小，重新框选");
            return;
        }

        _fixRecipe.Roi = new FixtureRoi
        {
            X = bounds.X / (double)image.Width,
            Y = bounds.Y / (double)image.Height,
            W = bounds.Width / (double)image.Width,
            H = bounds.Height / (double)image.Height,
        };

        TrySaveQuiet();
        UpdateFixRoiInfo();
        _fixOverlayUntil = DateTime.Now.AddSeconds(4);
        ApplyFixRoiOverlay();
        SetFixCameraInfo("检测区已保存；金样板掩码还是按旧 ROI 算的，请到“模板”页点“用金样板重建分割参数”");
    }

    // ---------------------------------------------------------------- judge

    private async Task JudgeAsync(Mat provided)
    {
        if (_fixJudging) return;
        if (_fixRecipe == null)
        {
            SetFixCameraInfo("请先选择机种");
            return;
        }

        var errors = _fixRecipe.Validate();
        if (errors.Count > 0)
        {
            ShowFixResult(new FixtureResult
            {
                Ok = false,
                Reason = FixtureReason.CalibInvalid,
                Message = string.Join("；", errors),
                Model = _fixRecipe.Model,
            }, null);
            return;
        }

        if (provided != null)
        {
            await RunJudgeAsync(provided);
            return;
        }

        if (_fixCam == null && !await OpenFixCameraAsync(true))
        {
            ReportCameraLost("相机没有打开");
            return;
        }

        await JudgeFromCameraAsync();
    }

    /// <summary>单张判定（回放图片或调试用），不负责释放 frame。</summary>
    private async Task RunJudgeAsync(Mat frame)
    {
        var recipe = _fixRecipe;
        var golden = _fixGolden;
        var mask = _fixMask;
        var empty = _fixEmpty;

        SetFixBusy(true);
        try
        {
            var outcome = await Task.Run(() => FixtureAnalyzer.Analyze(frame, recipe, golden, mask, empty));
            ShowFixResult(outcome.Result, outcome.Overlay);
        }
        catch (Exception ex)
        {
            Log.Error("针床检测失败", ex);
            ShowFixResult(new FixtureResult
            {
                Ok = false, Reason = FixtureReason.InternalError,
                Message = "检测失败：" + ex.Message, Model = _fixModel,
            }, null);
        }
        finally
        {
            SetFixBusy(false);
        }
    }

    /// <summary>按 ConfirmFrames 连续取帧：全部 OK 才 OK，中间任何一帧 NG 立即按 NG 报。</summary>
    private async Task JudgeFromCameraAsync()
    {
        var cam = _fixCam;
        var recipe = _fixRecipe;
        var golden = _fixGolden;
        var mask = _fixMask;
        var empty = _fixEmpty;
        int need = Math.Clamp(recipe.ConfirmFrames, 1, 10);
        long lastId = _fixShownFrame;
        FixtureOutcome last = null;

        SetFixBusy(true);
        try
        {
            for (int i = 0; i < need; i++)
            {
                long after = lastId;
                var got = await Task.Run(() =>
                {
                    var frame = cam.WaitNewFrame(after, 900, out var fid);
                    return (Frame: frame, Id: fid);
                });

                if (got.Frame == null)
                {
                    last?.Overlay?.Dispose();
                    ReportCameraLost($"900ms 内没有取到新画面（第 {i + 1}/{need} 帧）");
                    return;
                }

                lastId = got.Id;
                var frame = got.Frame;
                FixtureOutcome outcome;
                try
                {
                    outcome = await Task.Run(() => FixtureAnalyzer.Analyze(frame, recipe, golden, mask, empty));
                }
                finally
                {
                    frame.Dispose();
                }

                if (!outcome.Result.Ok)
                {
                    last?.Overlay?.Dispose();
                    ShowFixResult(outcome.Result, outcome.Overlay);
                    return;
                }

                last?.Overlay?.Dispose();
                last = outcome;
            }

            if (last != null) ShowFixResult(last.Result, last.Overlay);
            else ReportCameraLost("没有取到可用画面");
        }
        catch (Exception ex)
        {
            Log.Error("针床检测失败", ex);
            last?.Overlay?.Dispose();
            ShowFixResult(new FixtureResult
            {
                Ok = false, Reason = FixtureReason.InternalError,
                Message = "检测失败：" + ex.Message, Model = _fixModel,
            }, null);
        }
        finally
        {
            SetFixBusy(false);
        }
    }

    private void ReportCameraLost(string message)
    {
        ShowFixResult(new FixtureResult
        {
            Ok = false, Reason = FixtureReason.CameraLost,
            Message = message, Model = _fixModel,
        }, null);
    }

    private void SetFixBusy(bool busy)
    {
        _fixJudging = busy;
        _btnFixJudge.Enabled = !busy;
        _btnFixTest.Enabled = !busy;
        _btnFixReplay.Enabled = !busy;
        _btnFixReplay2.Enabled = !busy;
    }

    private async Task ReplayAsync()
    {
        using var dlg = new OpenFileDialog
        {
            Title = "选择一张现场照片",
            Filter = "图片|*.png;*.jpg;*.jpeg;*.bmp|所有文件|*.*",
            InitialDirectory = AppPaths.CaptureDir,
        };
        if (dlg.ShowDialog(this) != DialogResult.OK) return;

        Mat frame = null;
        try
        {
            frame = OpenCvSharp.Cv2.ImRead(dlg.FileName, OpenCvSharp.ImreadModes.Color);
            if (frame == null || frame.Empty())
            {
                SetFixCameraInfo("这张图片读不出来");
                return;
            }

            ShowFixFrame(frame);
            await JudgeAsync(frame);
        }
        finally
        {
            frame?.Dispose();
        }
    }

    private void ShowFixResult(FixtureResult result, Mat overlay)
    {
        if (result == null) return;

        _lblFixVerdict.Text = result.Ok ? "OK" : "NG";
        _lblFixVerdict.ForeColor = result.Ok ? Theme.Ok : Theme.Danger;
        var reasonText = $"{result.ReasonName} · {result.ReasonCode}\r\n{result.Message}";
        _lblFixReason.Text = reasonText;
        // 标签最多显示两行，具体数值（边距/越界像素等）鼠标停留可看全文。
        _fixTip?.SetToolTip(_lblFixReason, reasonText);

        if (_lblFixTestInfo != null)
        {
            _lblFixTestInfo.Text = result.ZoneMode
                ? $"安全区模式：边距 左{result.ZoneLeftPx} 右{result.ZoneRightPx} 上{result.ZoneTopPx} 下{result.ZoneBottomPx} px（下限 {result.ZoneMarginLimitPx}px）\r\n" +
                  $"越界 {result.ZoneOverflowPx}px / 最大越界深度 {result.ZoneMaxDepthPx:F0}px / 一致度 {result.Confidence:P0}\r\n" +
                  $"中心偏移 X {result.DxPx:F0} / Y {result.DyPx:F0} px / 耗时 {result.ElapsedMs:F0} ms / {result.FrameWidth}×{result.FrameHeight}"
                : $"X {result.DxMm:F2} mm / Y {result.DyMm:F2} mm / 角度 {result.DThetaDeg:F2}°\r\n" +
                  $"IoU {result.IoU:F3} / 面积比 {result.AreaRatio:F3} / 越界 {result.OverflowMm:F2} mm\r\n" +
                  $"一致度 {result.Confidence:P0} / 耗时 {result.ElapsedMs:F0} ms / {result.FrameWidth}×{result.FrameHeight}";
        }

        AddFixLog(result);
        FixtureHistory.Append(result);

        _fixHold = false;
        _fixTemplateView = false;
        ShowCalibLine(false);
        UpdateFixSnapButtons();

        if (overlay != null)
        {
            try
            {
                var bmp = MatToBitmap(overlay);
                var old = _picFixture.Image;
                _picFixture.SetFrame(bmp);
                old?.Dispose();
            }
            finally
            {
                overlay.Dispose();
            }
        }

        _fixFreezeUntil = DateTime.Now.AddSeconds(2.5);
        if (!result.Ok)
            Log.Warn($"针床检测 NG {result.Model}: {result.ReasonCode} {result.Message}");
    }

    private void AddFixLog(FixtureResult r)
    {
        if (_lvFixLog == null) return;
        var item = new ListViewItem(r.Time.ToString("HH:mm:ss")) { ForeColor = r.Ok ? Theme.Ok : Theme.Danger };
        item.SubItems.Add(r.Ok ? "OK" : "NG");
        item.SubItems.Add($"{r.ReasonName} {r.ReasonCode}");
        item.SubItems.Add(r.ZoneMode
            ? $"边距 {Math.Min(Math.Min(r.ZoneLeftPx, r.ZoneRightPx), Math.Min(r.ZoneTopPx, r.ZoneBottomPx))}px / 越界 {r.ZoneOverflowPx}px"
            : $"X {r.DxMm:F1} / Y {r.DyMm:F1}");
        item.SubItems.Add($"{r.ElapsedMs:F0} ms");
        _lvFixLog.Items.Insert(0, item);
        while (_lvFixLog.Items.Count > 30) _lvFixLog.Items.RemoveAt(_lvFixLog.Items.Count - 1);
    }

    private void SetFixVerdict(string text)
    {
        if (_lblFixVerdict == null) return;
        _lblFixVerdict.Text = string.IsNullOrWhiteSpace(text) ? "待检测" : text;
        _lblFixVerdict.ForeColor = Theme.Muted;
    }

    // ---------------------------------------------------------------- misc

    private void SaveTolerance()
    {
        if (_fixRecipe == null) return;
        _fixRecipe.Tolerance.DxMm = (double)_numFixDx.Value;
        _fixRecipe.Tolerance.DyMm = (double)_numFixDy.Value;
        _fixRecipe.Tolerance.DThetaDeg = (double)_numFixTheta.Value;
        _fixRecipe.Tolerance.IoUMin = (double)_numFixIou.Value;
        _fixRecipe.Tolerance.AreaRatioMin = (double)_numFixAreaMin.Value;
        _fixRecipe.Tolerance.AreaRatioMax = (double)_numFixAreaMax.Value;
        _fixRecipe.Tolerance.ZoneMarginMm = (double)_numFixZone.Value;
        _fixRecipe.ConfirmFrames = (int)_numFixConfirm.Value;
        _fixRecipe.Quality.MinMeanGray = (double)_numFixMeanGray.Value;
        _fixRecipe.Quality.MinContrast = (double)_numFixContrast.Value;
        _fixRecipe.Quality.MaxHighlightRatio = (double)_numFixHighlight.Value;
        _fixRecipe.Tolerance.MinContourScore = (double)_numFixContour.Value;
        _fixRecipe.Tolerance.ContourScoreDrop = (double)_numFixDrop.Value;
        _fixRecipe.JudgeMode = _cboFixJudgeMode.SelectedIndex == 1 ? "zone" : "shape";
        _fixRecipe.Zone ??= new FixtureZoneConfig();
        _fixRecipe.Zone.MarginPx = (int)_numFixMarginPx.Value;
        _fixRecipe.Zone.OverflowPx = (int)_numFixOverflowPx.Value;
        TrySaveQuiet();
        SetFixCameraInfo("阈值已保存");
    }

    private void CreateFixtureModel()
    {
        var name = PromptText("新建机种", "机种编号（例如 B002）", "");
        if (string.IsNullOrWhiteSpace(name)) return;
        var model = FixtureStore.Sanitize(name);

        var recipe = FixtureRecipe.CreateDefault(model);
        FixtureStore.EnsureModel(model);
        FixtureStore.Save(recipe);

        _fixModelSync = true;
        try
        {
            if (!_cboFixModel.Items.Contains(model)) _cboFixModel.Items.Add(model);
            _cboFixModel.SelectedItem = model;
        }
        finally
        {
            _fixModelSync = false;
        }

        LoadFixRecipe(model);
    }

    private void TrySaveQuiet()
    {
        try
        {
            FixtureStore.Save(_fixRecipe);
        }
        catch (Exception ex)
        {
            SetFixCameraInfo("保存配方失败：" + ex.Message);
        }
    }

    private async Task<Mat> GrabFixFrameAsync(int timeoutMs)
    {
        if (_fixCam == null) await OpenFixCameraAsync(true);
        var cam = _fixCam;
        if (cam == null) return null;

        long after = Math.Max(_fixFrameWatermark, _fixSnapFrameId);
        var got = await Task.Run(() =>
        {
            var frame = cam.WaitNewFrame(after, timeoutMs, out var id);
            return (Frame: frame, Id: id);
        });

        long frameId = got.Id;
        var result = got.Frame;
        if (result == null)
        {
            result = cam.TryGetLatest(out var latest, out frameId) ? latest : null;
        }

        if (result != null) _fixFrameWatermark = Math.Max(_fixFrameWatermark, frameId);
        return result;
    }

    private decimal? PromptNumber(string title, string label, decimal value,
        decimal min, decimal max, int decimals)
    {
        using var dlg = new Form
        {
            Text = title,
            FormBorderStyle = FormBorderStyle.FixedDialog,
            StartPosition = FormStartPosition.CenterParent,
            ClientSize = new Size(340, 150),
            Font = Theme.Body,
            BackColor = Color.White,
            MinimizeBox = false,
            MaximizeBox = false,
            ShowInTaskbar = false,
        };

        var lbl = new Label { Text = label, AutoSize = true, Location = new Point(16, 18), ForeColor = Theme.Ink };
        var num = new NumericUpDown
        {
            Minimum = min, Maximum = max, Value = Math.Clamp(value, min, max),
            DecimalPlaces = decimals, Increment = 1,
            Location = new Point(16, 48), Width = 220,
        };
        var ok = Theme.Primary("确定");
        ok.Location = new Point(156, 96);
        ok.DialogResult = DialogResult.OK;
        var cancel = Theme.Ghost("取消");
        cancel.Location = new Point(248, 96);
        cancel.DialogResult = DialogResult.Cancel;

        dlg.Controls.AddRange(new Control[] { lbl, num, ok, cancel });
        dlg.AcceptButton = ok;
        dlg.CancelButton = cancel;

        return dlg.ShowDialog(this) == DialogResult.OK ? num.Value : null;
    }

    private string PromptText(string title, string label, string value)
    {
        using var dlg = new Form
        {
            Text = title,
            FormBorderStyle = FormBorderStyle.FixedDialog,
            StartPosition = FormStartPosition.CenterParent,
            ClientSize = new Size(340, 150),
            Font = Theme.Body,
            BackColor = Color.White,
            MinimizeBox = false,
            MaximizeBox = false,
            ShowInTaskbar = false,
        };

        var lbl = new Label { Text = label, AutoSize = true, Location = new Point(16, 18), ForeColor = Theme.Ink };
        var tb = new TextBox { Text = value, Location = new Point(16, 48), Width = 300, Font = Theme.Body };
        var ok = Theme.Primary("确定");
        ok.Location = new Point(156, 96);
        ok.DialogResult = DialogResult.OK;
        var cancel = Theme.Ghost("取消");
        cancel.Location = new Point(248, 96);
        cancel.DialogResult = DialogResult.Cancel;

        dlg.Controls.AddRange(new Control[] { lbl, tb, ok, cancel });
        dlg.AcceptButton = ok;
        dlg.CancelButton = cancel;

        return dlg.ShowDialog(this) == DialogResult.OK ? tb.Text.Trim() : null;
    }
}
