using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DeploySharp.Engine;
using Cv2 = OpenCvSharp.Cv2;
using CvPoint = OpenCvSharp.Point;
using CvRect = OpenCvSharp.Rect;
using CvScalar = OpenCvSharp.Scalar;
using Hershey = OpenCvSharp.HersheyFonts;
using LineTypes = OpenCvSharp.LineTypes;
using Mat = OpenCvSharp.Mat;

namespace YoloFool;

public sealed partial class MainForm
{
    // ---------------- 状态 ----------------
    private ToolTip _anchorTip;

    private System.Windows.Forms.Timer _anchorTimer;
    private FixtureCamera _anchorCam;
    private InferenceService _anchorSvc;
    private AnchorProfile _anchorProfile;
    private AnchorVerdict _anchorLastVerdict;
    private readonly AnchorJitter _anchorJitter = new();
    private string _anchorModel = "";
    private long _anchorShownFrame = -1;
    private bool _anchorActive;
    private bool _anchorBusy;
    private bool _anchorCameraBusy;
    private bool _anchorModelLoading;
    private bool _anchorOverlayOn = true;

    // ---------------- 检测范围 / 识别目标 ----------------
    private ListView _lvAnchorTargets;
    private AnchorZoneReport _anchorZoneReport;
    private bool _anchorZonePicking;
    private bool _anchorTargetPicking;
    private bool _anchorTraining;
    private bool _anchorTrainerHooked;
    private bool _zoneUiSync;
    private TrainingService _anchorTrainer;
    private Mat _anchorRaw;

    // ---------------- 页面骨架 ----------------
    // 静态控件树已搬进 MainForm.Designer.cs（针床定位 V2 页块），这里只做运行期数据、样式与事件接线。
    private void InitializeAnchorPage()
    {
        // 相机序号 / 分辨率的候选项属运行期数据，控件本身在设计器里。
        _cboAnchorCamIndex.Items.Clear();
        for (var i = 0; i < 6; i++) _cboAnchorCamIndex.Items.Add(i.ToString());
        _cboAnchorCamIndex.SelectedIndex = 0;

        _cboAnchorRes.Items.Clear();
        _cboAnchorRes.Items.Add("1920×1080");
        _cboAnchorRes.Items.Add("1280×720");
        _cboAnchorRes.SelectedIndex = 0;

        // ---------------- 工具栏 ----------------
        _btnAnchorNewModel.Click += (_, __) => CreateAnchorModel();
        _btnAnchorCopyModel.Click += (_, __) => CopyAnchorModel();
        _chkAnchorExposure.CheckedChanged += (_, __) => _numAnchorExposure.Enabled = _chkAnchorExposure.Checked;
        _btnAnchorShot.Click += async (_, __) => await CaptureAnchorPhotoAsync();
        _btnAnchorCam.Click += async (_, __) => await ToggleAnchorCameraAsync();
        // 「判定一次」放在相机旁边（m01839）：换板后不用切页签，直接点这里。
        _btnAnchorJudge.Click += async (_, __) => await AnchorJudgeOnceAsync();

        // ---------------- 侧栏页签 ----------------
        _anchorTabs.SelectedIndexChanged += (_, __) =>
        {
            // 离开第三页就解除“基准/对比画面冻结”，否则实时预览会一直停在对比图。
            if (!ReferenceEquals(_anchorTabs.SelectedTab, _tabAnchorCalib)) _anchorCalibView = false;
        };

        // ---------------- ① 识别目标 ----------------
        _btnAnchorLearnPick.Click += (_, __) => BeginAnchorTargetPick();
        _btnAnchorLearnDel.Click += (_, __) => DeleteAnchorDraft();
        _btnAnchorLearnDelShot.Click += (_, __) => DeleteAnchorSample();
        _btnAnchorLearnLive.Click += (_, __) =>
        {
            if (_anchorEditFrame == null) { SetAnchorCamInfo("现在就是实时画面。"); return; }
            ExitAnchorEditMode();
            if (_lvAnchorSamples != null) _lvAnchorSamples.SelectedItems.Clear();
            SetAnchorCamInfo("已回到实时画面。点上方【拍照】可再拍一张。");
        };
        _lvAnchorDrafts.SelectedIndexChanged += (_, __) => SyncDraftSelectionToCanvas();
        _lvAnchorDrafts.KeyDown += (_, e) =>
        {
            if (e.KeyCode != Keys.Delete) return;
            e.Handled = true;
            DeleteAnchorDraft();
        };
        _lvAnchorSamples.SelectedIndexChanged += (_, __) =>
        {
            ShowAnchorSamplePreview();
            LoadAnchorPhotoFromSelection();
        };
        _btnAnchorTrain.Click += async (_, __) => await TrainAnchorAsync();
        _btnAnchorTrainRefresh.Click += (_, __) =>
        {
            ShowAnchorTargets();
            ShowAnchorSamples();
        };
        _hostAnchorLearn.VisibleChanged += (_, __) =>
        {
            if (!_hostAnchorLearn.Visible) return;
            ShowAnchorTargets();
            ShowAnchorSamples();
            UpdateAnchorClassLabel();
        };

        // ---------------- ② 检测范围 · 判定 ----------------
        _btnAnchorZonePick.Click += (_, __) => BeginAnchorZonePick();
        _btnAnchorZoneFromLabel.Click += (_, __) => ShowAnchorZoneClassMenu(_btnAnchorZoneFromLabel);
        _btnAnchorZoneCapture.Click += (_, __) => CaptureAnchorZoneRef();
        _btnAnchorZoneEdit.Click += (_, __) => BeginAnchorZoneEdit();
        _btnAnchorZoneRename.Click += (_, __) => RenameAnchorZone();
        _btnAnchorZoneRemove.Click += (_, __) => DeleteAnchorZone();
        _anchorTip?.SetToolTip(_btnAnchorZoneCapture, "把当前画面里这块区域的外观存成模板（采集时板必须放好）。判定时用它比对，匹配率低于下限或位置偏太多就判 NG。");
        _anchorTip?.SetToolTip(_btnAnchorZoneFromLabel, "从已经框好的标签里挑一个（例如 p1），直接用那个框的范围建检测范围。");
        _anchorTip?.SetToolTip(_btnAnchorZonePick, "在画面上拖出新范围，右键或回车确认；再点一次取消。");
        _anchorTip?.SetToolTip(_btnAnchorZoneEdit, "重新调这个范围的位置/大小/角度：拖框移动、拖角点改大小、拖顶部圆点旋转，右键保存（不保存不生效），Esc 放弃。");
        _anchorTip?.SetToolTip(_btnAnchorZoneRename, "给列表里选中的范围改名（参考图会一起改名，不会丢）。也可以右键列表或按 F2。");
        _anchorTip?.SetToolTip(_btnAnchorZoneRemove, "删除列表里选中的范围及其参考图。也可以右键列表或按 Delete。");
        _anchorTip?.SetToolTip(_cboZoneClass, "给这个范围备注它对应哪个识别目标（例如 p1）；留空只做模板匹配。");
        _lvAnchorZones.SelectedIndexChanged += (_, __) =>
        {
            LoadZoneUiFromSelected();
            RefreshZoneRefPreview();
        };
        _lvAnchorZones.KeyDown += (_, e) =>
        {
            if (e.KeyCode == Keys.Delete)
            {
                DeleteAnchorZone();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.F2)
            {
                RenameAnchorZone();
                e.Handled = true;
            }
        };
        _lvAnchorZones.MouseUp += (_, e) =>
        {
            if (e.Button != MouseButtons.Right) return;
            var hit = _lvAnchorZones.GetItemAt(e.X, e.Y);
            if (hit != null)
            {
                hit.Selected = true;
                hit.Focused = true;
            }
            ShowZoneMenu();
        };
        _chkAnchorAdv.CheckedChanged += (_, __) =>
        {
            var show = _chkAnchorAdv.Checked;
            if (_pnlAnchorTol != null) _pnlAnchorTol.Visible = show;
            _tlpAnchorZone.RowStyles[5] = new RowStyle(SizeType.Absolute, show ? 186f : 0f);
            _tlpAnchorZone.Height = 992 + (show ? 186 : 0);
        };
        _btnAnchorZoneSaveTol.Click += (_, __) => SaveAnchorTolerance();
        _btnAnchorZoneOverlay.Click += (_, __) =>
        {
            _anchorOverlayOn = !_anchorOverlayOn;
            _btnAnchorZoneOverlay.Text = _anchorOverlayOn ? "叠加：开" : "叠加：关";
            if (_anchorRaw != null && !_anchorRaw.Empty()) ShowAnchorFrame(_anchorRaw, _anchorLastVerdict);
        };
        _numZoneMatch.ValueChanged += OnZoneParamChanged;
        _numZoneSearch.ValueChanged += OnZoneParamChanged;
        _numZoneShift.ValueChanged += OnZoneParamChanged;
        _cboZoneClass.SelectedIndexChanged += OnZoneParamChanged;
        _chkZoneRequired.CheckedChanged += OnZoneParamChanged;
        _numAnchorShift.ValueChanged += (_, __) => ApplyAnchorToleranceToProfile();
        _numAnchorRot.ValueChanged += (_, __) => ApplyAnchorToleranceToProfile();
        _numAnchorScale.ValueChanged += (_, __) => ApplyAnchorToleranceToProfile();
        _numAnchorRms.ValueChanged += (_, __) => ApplyAnchorToleranceToProfile();
        _numAnchorConf.ValueChanged += (_, __) => ApplyAnchorToleranceToProfile();
        _numAnchorMin.ValueChanged += (_, __) => ApplyAnchorToleranceToProfile();
        _hostAnchorZone.VisibleChanged += (_, __) =>
        {
            if (!_hostAnchorZone.Visible) return;
            // 切到本页只做刷新：框选必须点“框选新范围”才开始，避免一拖就冒出范围。
            CancelAnchorPicking();
            RefreshZoneClassCombo();
            ShowAnchorZones();
            RefreshZoneRefPreview();
        };

        // ---------------- ③ 拍基准 · 对比 ----------------
        _btnAnchorCalib.Click += async (_, __) => await AnchorCalibrateAsync();
        _btnAnchorCalibView.Click += (_, __) => ShowAnchorCalibImage();
        _btnAnchorCalibCompare.Click += async (_, __) => await AnchorCompareCalibAsync();
        _btnAnchorCalibLive.Click += (_, __) => BackToAnchorLive();

        if (!_pageAnchor.Controls.Contains(_tlpAnchor)) _pageAnchor.Controls.Add(_tlpAnchor);
    }


    // ---------------- 事件接线 ----------------
    private void WireAnchorEvents()
    {
        _cboAnchorModel.SelectedIndexChanged += AnchorModelChanged;
        _picAnchor.BoxDrawn += OnAnchorBoxDrawn;
        _picAnchor.BoxEdited += OnAnchorBoxEdited;
        _picAnchor.SelectionChanged += OnAnchorDraftSelectionChanged;
        _picAnchor.KeyDown += AnchorCanvasKeyDown;
        _tabs.SelectedIndexChanged += (_, __) => OnAnchorTabChanged();
        FormClosing += (_, __) =>
        {
            _anchorTimer?.Stop();
            _anchorTrainer?.Cancel();
            ExitAnchorEditMode();
            CloseAnchorCamera();
            DisposeAnchorService();
            _anchorRaw?.Dispose();
            _anchorRaw = null;
        };
    }

    private void OnAnchorTabChanged()
    {
        var on = _tabs != null && ReferenceEquals(_tabs.SelectedTab, _pageAnchor);
        _anchorActive = on;
        if (on)
        {
            if (_anchorProfile == null) RefreshAnchorModels();
            else if (_anchorCam != null) _anchorTimer?.Start();
            _anchorTimer?.Start();
        }
        else
        {
            _anchorTimer?.Stop();
            ExitAnchorEditMode();
            CloseAnchorCamera();
        }
    }

    // ---------------- 机种与模型 ----------------
    private void RefreshAnchorModels()
    {
        var models = AnchorStore.ListModels().ToList();
        if (models.Count == 0)
        {
            AnchorStore.EnsureModel("B001");
            models.Add("B001");
        }

        _anchorModelLoading = true;
        try
        {
            _cboAnchorModel.Items.Clear();
            foreach (var name in models) _cboAnchorModel.Items.Add(name);
            var index = models.IndexOf(_anchorModel);
            _cboAnchorModel.SelectedIndex = index >= 0 ? index : 0;
        }
        finally
        {
            _anchorModelLoading = false;
        }

        _anchorModel = _cboAnchorModel.SelectedItem as string ?? models[0];
        _anchorProfile = AnchorStore.LoadOrCreate(_anchorModel);
        ApplyAnchorToUi();

        if (AnchorStore.LocalModelPath(_anchorModel).Length == 0)
        {
            _lblAnchorModelInfo.Text = "本机种还没有模型副本";
            _lblAnchorModelInfo.ForeColor = Theme.Warn;
            DisposeAnchorService();
            return;
        }
        _ = LoadAnchorServiceAsync();
    }

    private async void AnchorModelChanged(object sender, EventArgs e)
    {
        if (_anchorModelLoading) return;
        var model = _cboAnchorModel.SelectedItem as string;
        if (string.IsNullOrWhiteSpace(model) || string.Equals(model, _anchorModel, StringComparison.OrdinalIgnoreCase)) return;

        _anchorModel = model;
        _anchorProfile = AnchorStore.LoadOrCreate(model);
        _anchorJitter.Clear();
        ApplyAnchorToUi();
        _anchorZoneReport = null;
        RefreshZoneClassCombo();
        ShowAnchorZones();
        ShowAnchorTargets();
        DisposeAnchorService();
        await LoadAnchorServiceAsync();
    }

    private void CreateAnchorModel()
    {
        var name = "机种-" + DateTime.Now.ToString("MMdd-HHmm");
        AnchorStore.EnsureModel(name);
        _anchorModel = name;
        _anchorProfile = AnchorStore.LoadOrCreate(name);
        AnchorStore.Save(_anchorProfile);
        _anchorZoneReport = null;
        RefreshAnchorModels();
        RefreshZoneClassCombo();
        ShowAnchorZones();
        ShowAnchorTargets();
        SetAnchorCamInfo($"已新建机种 {name}，请先训练模型或点“复制当前模型”。");
    }

    private void CopyAnchorModel()
    {
        if (string.IsNullOrWhiteSpace(_anchorModel)) return;
        if (!AnchorStore.CopyFromGlobalModel(_anchorModel, out var error))
        {
            SetAnchorCamInfo(error);
            return;
        }
        SetAnchorCamInfo("模型已复制到本机种。");
        _ = LoadAnchorServiceAsync();
    }

    private async Task LoadAnchorServiceAsync()
    {
        var model = _anchorModel;
        var path = AnchorStore.LocalModelPath(model);
        if (path.Length == 0 || _anchorProfile == null) return;

        ApplyAnchorToleranceToProfile();
        var conf = (float)_anchorProfile.Infer.Conf;
        var nms = (float)_anchorProfile.Infer.Nms;
        _lblAnchorModelInfo.Text = "模型加载中…";
        _lblAnchorModelInfo.ForeColor = Theme.Muted;
        try
        {
            var svc = await Task.Run(() =>
            {
                var service = new InferenceService();
                service.Load(new ModelOptions(
                    path,
                    YoloModelKind.YOLOv8Det,
                    InferenceBackend.OnnxRuntime,
                    DeviceType.CPU,
                    OnnxRuntimeDeviceType.Default,
                    conf,
                    nms));
                service.Warmup();
                return service;
            });

            if (!string.Equals(model, _anchorModel, StringComparison.OrdinalIgnoreCase))
            {
                svc.Dispose();
                return;
            }

            DisposeAnchorService();
            _anchorSvc = svc;
            var info = svc.Info;
            var names = info?.ClassNames != null ? string.Join("、", info.ClassNames) : "?";
            _lblAnchorModelInfo.Text = $"{Path.GetFileName(path)} · {info?.Kind} · 类别 {info?.ClassNames?.Count ?? 0}：{names}";
            _lblAnchorModelInfo.ForeColor = Theme.Muted;
            RefreshZoneClassCombo();
            ShowAnchorZones();
            ShowAnchorTargets();
            UpdateAnchorClassLabel();
        }
        catch (Exception ex)
        {
            _lblAnchorModelInfo.Text = "模型加载失败：" + ex.Message;
            _lblAnchorModelInfo.ForeColor = Theme.Danger;
        }
    }

    private void DisposeAnchorService()
    {
        var svc = _anchorSvc;
        _anchorSvc = null;
        if (svc == null) return;
        try { svc.Dispose(); } catch { }
    }

    // ---------------- 相机 ----------------
    private FixtureCameraConfig ReadAnchorCameraConfig()
    {
        var cfg = new FixtureCameraConfig { FourCC = "MJPG" };
        if (_anchorProfile?.Camera != null)
        {
            cfg.Width = _anchorProfile.Camera.Width;
            cfg.Height = _anchorProfile.Camera.Height;
            cfg.Gain = _anchorProfile.Camera.Gain;
        }
        cfg.Index = Math.Max(0, _cboAnchorCamIndex.SelectedIndex);
        var res = _cboAnchorRes.SelectedItem as string ?? "";
        if (res.StartsWith("1920", StringComparison.Ordinal))
        {
            cfg.Width = 1920;
            cfg.Height = 1080;
        }
        else if (res.StartsWith("1280", StringComparison.Ordinal))
        {
            cfg.Width = 1280;
            cfg.Height = 720;
        }
        cfg.Exposure = _chkAnchorExposure.Checked ? (double)_numAnchorExposure.Value : null;
        return cfg;
    }

    private async Task<bool> ToggleAnchorCameraAsync()
    {
        if (_anchorCam != null)
        {
            CloseAnchorCamera();
            return false;
        }
        return await OpenAnchorCameraAsync();
    }

    private async Task<bool> EnsureAnchorCameraAsync()
    {
        if (_anchorCam != null) return true;
        return await OpenAnchorCameraAsync();
    }

    private async Task<bool> OpenAnchorCameraAsync()
    {
        if (_anchorCameraBusy) return false;
        _anchorCameraBusy = true;
        SetAnchorCamInfo("正在打开相机…");
        try
        {
            var cfg = ReadAnchorCameraConfig();
            var cam = new FixtureCamera();
            var error = await Task.Run(() =>
            {
                if (cam.Open(cfg, out var message)) return "";
                return string.IsNullOrWhiteSpace(message) ? "未知原因" : message;
            });
            if (error.Length > 0)
            {
                try { cam.Close(); } catch { }
                SetAnchorCamInfo("打开失败：" + error);
                return false;
            }

            _anchorCam = cam;
            _anchorShownFrame = -1;
            if (_anchorProfile != null)
            {
                cfg.Index = _anchorProfile.Camera.Index;
                _anchorProfile.Camera = cfg;
                AnchorStore.Save(_anchorProfile);
            }

            if (_anchorTimer == null)
            {
                _anchorTimer = new System.Windows.Forms.Timer { Interval = 66 };
                _anchorTimer.Tick += (_, __) => AnchorPreviewTick();
            }
            _anchorTimer.Start();
            if (_btnAnchorCam != null) _btnAnchorCam.Text = "关闭相机";
            SetAnchorCamInfo($"已连接：相机 {cfg.Index} · {cfg.Width}×{cfg.Height} · MJPG");
            return true;
        }
        catch (Exception ex)
        {
            SetAnchorCamInfo("打开失败：" + ex.Message);
            return false;
        }
        finally
        {
            _anchorCameraBusy = false;
        }
    }

    private void CloseAnchorCamera()
    {
        ExitAnchorEditMode();
        _anchorTimer?.Stop();
        var cam = _anchorCam;
        _anchorCam = null;
        if (cam != null)
        {
            try { cam.Close(); } catch { }
        }
        if (_btnAnchorCam != null) _btnAnchorCam.Text = "打开相机";
        SetAnchorCamInfo("相机已关闭");
    }

    private void AnchorPreviewTick()
    {
        if (_anchorCam == null || _anchorBusy) return;
        if (_anchorEditFrame != null || _zoneEditing) return;
        if (_anchorCalibView) return;
        if (_tabs == null || !ReferenceEquals(_tabs.SelectedTab, _pageAnchor)) return;
        if (!_anchorCam.TryGetLatest(out var frame, out var id)) return;
        try
        {
            if (id == _anchorShownFrame) return;
            _anchorShownFrame = id;
            _anchorRaw?.Dispose();
            _anchorRaw = frame.Clone();
            ShowAnchorFrame(frame, null);
        }
        finally
        {
            frame.Dispose();
        }
    }

    // ---------------- 拍基准 ----------------
    private async Task AnchorCalibrateAsync()
    {
        if (_anchorBusy) return;
        if (_anchorProfile == null)
        {
            SetAnchorCamInfo("还没有选机种。");
            return;
        }
        if (_anchorSvc == null || !_anchorSvc.IsLoaded)
        {
            SetAnchorCamInfo("模型还没加载：先点“复制当前模型”，或到训练页训练一个。");
            return;
        }
        if (!await EnsureAnchorCameraAsync()) return;

        var cam = _anchorCam;
        var svc = _anchorSvc;
        var profile = _anchorProfile;
        var model = _anchorModel;
        SetAnchorBusy(true, "正在拍基准…");
        Mat frame = null;
        long frameId = -1;
        try
        {
            frame = await Task.Run(() =>
            {
                var f = cam.WaitNewFrame(_anchorShownFrame, 1500, out var id);
                frameId = id;
                return f;
            });
            if (frame == null || frame.Empty())
            {
                SetAnchorCamInfo("1.5 秒内没有拿到新画面，检查相机连接。");
                return;
            }
            _anchorShownFrame = Math.Max(_anchorShownFrame, frameId);

            var result = await Task.Run(() => svc.Predict(frame));
            var anchors = PickAnchorItems(result, profile, out var note);
            var need = Math.Max(2, profile.Tolerance.MinAnchors);
            if (anchors.Count < need)
            {
                _lblAnchorVerdict.Text = "NG";
                _lblAnchorVerdict.ForeColor = Theme.Danger;
                _lblAnchorReason.Text = "拍基准失败：锚点不够";
                _lblAnchorReason.ForeColor = Theme.Danger;
                _lblAnchorDetail.Text = $"只检出 {anchors.Count} 个锚点，至少需要 {need} 个。{note}";
                return;
            }

            profile.Calib.Width = frame.Width;
            profile.Calib.Height = frame.Height;
            profile.Calib.At = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            profile.Calib.Anchors = anchors;
            AnchorStore.Save(profile);
            AnchorStore.SaveImage(model, AnchorStore.CalibFile, frame);

            using (var overlay = frame.Clone())
            {
                DrawAnchorBoxes(overlay, anchors.Select(a => (a.Cls, a.Ex, a.Ey, a.W, a.H)).ToList(), new CvScalar(0, 190, 60), true);
                OverlayText.Chip(overlay, 18, 10, "基准", OverlayText.ToColor(new CvScalar(0, 190, 60)), 15f);
                AnchorStore.SaveImage(model, AnchorStore.OverlayFile, overlay);
                // 拍完基准先停住画面，方便确认基准图；点【返回实时】或离开第三页再恢复预览。
                _anchorCalibView = true;
                ShowMatFrame(overlay);
            }

            _anchorJitter.Clear();
            _lblAnchorVerdict.Text = "已标定";
            _lblAnchorVerdict.ForeColor = Theme.Ok;
            _lblAnchorReason.Text = $"基准已保存：{anchors.Count} 个锚点";
            _lblAnchorReason.ForeColor = Theme.Ok;
            _lblAnchorDetail.Text = string.Join(" / ", anchors.Select(a => $"{a.Cls} @{a.Ex:0},{a.Ey:0}"))
                                    + (note.Length > 0 ? "；" + note : "");
            _lblAnchorCalibInfo.Text = $"基准：{profile.Calib.At} · {anchors.Count} 个锚点 · {profile.Calib.Width}×{profile.Calib.Height}";

            _lvAnchorHits.BeginUpdate();
            _lvAnchorHits.Items.Clear();
            foreach (var anchor in anchors)
            {
                var item = new ListViewItem(anchor.Cls);
                item.SubItems.Add($"{anchor.Ex:0},{anchor.Ey:0}");
                item.SubItems.Add("—");
                item.SubItems.Add("—");
                item.SubItems.Add($"{anchor.Conf:P0}");
                _lvAnchorHits.Items.Add(item);
            }
            _lvAnchorHits.EndUpdate();
        }
        catch (Exception ex)
        {
            SetAnchorCamInfo("拍基准出错：" + ex.Message);
        }
        finally
        {
            frame?.Dispose();
            SetAnchorBusy(false);
        }
    }

    // ---------------- 判定一次 ----------------
    private async Task AnchorJudgeOnceAsync()
    {
        if (_anchorBusy) return;
        if (_anchorProfile == null)
        {
            SetAnchorCamInfo("还没有选机种。");
            return;
        }
        if (!_anchorProfile.IsCalibrated && !_anchorProfile.HasZones)
        {
            SetAnchorCamInfo("还没有基准：先放一块确认到位的板点“拍基准”，或在“检测范围”里框一块特征。");
            return;
        }
        if ((_anchorSvc == null || !_anchorSvc.IsLoaded) && !_anchorProfile.HasZones)
        {
            SetAnchorCamInfo("模型还没加载：先点“复制当前模型”。");
            return;
        }
        if (!await EnsureAnchorCameraAsync()) return;

        ApplyAnchorToleranceToProfile();
        var cam = _anchorCam;
        var svc = _anchorSvc;
        var profile = _anchorProfile;
        var model = _anchorModel;
        SetAnchorBusy(true, "判定中…");
        Mat frame = null;
        long frameId = -1;
        try
        {
            frame = await Task.Run(() =>
            {
                var f = cam.WaitNewFrame(_anchorShownFrame, 1500, out var id);
                frameId = id;
                return f;
            });
            if (frame == null || frame.Empty())
            {
                SetAnchorCamInfo("1.5 秒内没有拿到新画面，检查相机连接。");
                return;
            }
            _anchorShownFrame = Math.Max(_anchorShownFrame, frameId);

            PredictionResult result = null;
            if (svc != null && svc.IsLoaded) result = await Task.Run(() => svc.Predict(frame));
            var verdict = JudgeAnchorFrame(frame, result);
            _anchorLastVerdict = verdict;

            if (verdict.Ok)
            {
                _anchorJitter.Add(verdict.MeanDx, verdict.MeanDy);
            }

            var shotName = $"{verdict.Time:yyyyMMdd-HHmmss}-{(verdict.Ok ? "OK" : "NG-" + verdict.ReasonCode)}.png";
            AnchorStore.SaveImage(model, Path.Combine(AnchorStore.ShotsDir, shotName), frame);
            AnchorHistory.Append(verdict, model, shotName);

            ShowAnchorVerdict(verdict, frame);
            ShowAnchorZones();
        }
        catch (Exception ex)
        {
            SetAnchorCamInfo("判定出错：" + ex.Message);
        }
        finally
        {
            frame?.Dispose();
            SetAnchorBusy(false);
        }
    }

    private void ShowAnchorVerdict(AnchorVerdict verdict, Mat frame)
    {
        if (verdict == null) return;
        _lblAnchorVerdict.Text = verdict.Ok ? "OK" : "NG";
        _lblAnchorVerdict.ForeColor = verdict.Ok ? Theme.Ok : Theme.Danger;
        _lblAnchorReason.Text = $"{verdict.ReasonName} · {verdict.ReasonCode}";
        _lblAnchorReason.ForeColor = verdict.Ok ? Theme.Ok : Theme.Danger;
        var detail = verdict.Message;
        if (_anchorZoneReport != null && _anchorZoneReport.Zones.Count > 0)
        {
            var lines = _anchorZoneReport.Zones
                .Select(r => r.Name + (r.Ok ? " ✓ " : " ✗ ") + r.ScoreText + (r.Ok ? "" : "（" + r.Code + "）"));
            var zoneLine = string.Join("  ", lines);
            detail = string.IsNullOrWhiteSpace(detail) ? zoneLine : zoneLine + "\n" + detail;
        }
        _lblAnchorDetail.Text = detail;
        _anchorTip?.SetToolTip(_lblAnchorReason, detail);

        var limit = _anchorProfile?.Tolerance?.ShiftPx ?? 12;
        _lvAnchorHits.BeginUpdate();
        _lvAnchorHits.Items.Clear();
        foreach (var hit in verdict.HitList)
        {
            var item = new ListViewItem(hit.ClsText);
            item.SubItems.Add($"{hit.Ex:0},{hit.Ey:0}");
            item.SubItems.Add($"{hit.Cx:0},{hit.Cy:0}");
            item.SubItems.Add($"{hit.Dx:+0;-0;0},{hit.Dy:+0;-0;0}");
            item.SubItems.Add($"{hit.Conf:P0}");
            item.ForeColor = hit.OffsetPx > limit ? Theme.Danger : Theme.Ink;
            _lvAnchorHits.Items.Add(item);
        }
        _lvAnchorHits.EndUpdate();

        AddAnchorLog(verdict);
        UpdateAnchorJitterLabel();
        if (frame != null) ShowAnchorFrame(frame, verdict);
    }

    private void AddAnchorLog(AnchorVerdict verdict)
    {
        if (_lvAnchorLog == null) return;
        var item = new ListViewItem(verdict.Time.ToString("HH:mm:ss"));
        item.SubItems.Add(verdict.Ok ? "OK" : "NG");
        item.SubItems.Add(verdict.ReasonCode);
        item.SubItems.Add($"{verdict.MeanDx:0.0},{verdict.MeanDy:0.0}");
        item.ForeColor = verdict.Ok ? Theme.Ok : Theme.Danger;
        _lvAnchorLog.Items.Insert(0, item);
        while (_lvAnchorLog.Items.Count > 200)
        {
            _lvAnchorLog.Items.RemoveAt(_lvAnchorLog.Items.Count - 1);
        }
    }

    private void UpdateAnchorJitterLabel()
    {
        if (_lblAnchorJitter == null) return;
        if (_anchorJitter.TrySuggest(out _, out var suggested, out var text))
        {
            var current = _anchorProfile?.Tolerance?.ShiftPx ?? 0;
            _lblAnchorJitter.Text = text + (current > suggested * 1.5 ? "（当前上限偏松）" : "");
            _lblAnchorJitter.ForeColor = current > suggested * 1.5 ? Theme.Warn : Theme.Muted;
        }
        else
        {
            _lblAnchorJitter.Text = $"连拍样本 {_anchorJitter.Count}/5，多判几次可估算抖动。";
            _lblAnchorJitter.ForeColor = Theme.Muted;
        }
    }

    // ---------------- 画面与叠加 ----------------
    private void ShowAnchorFrame(Mat frame, AnchorVerdict verdict)
    {
        if (frame == null || frame.Empty() || _picAnchor == null) return;
        using var view = BuildAnchorView(frame, verdict);
        ShowMatFrame(view);
    }

    private void ShowMatFrame(Mat frame)
    {
        if (frame == null || frame.Empty() || _picAnchor == null) return;
        var bmp = MatToBitmap(frame);
        var old = _picAnchor.Image;
        _picAnchor.SetFrame(bmp);
        old?.Dispose();
    }

    /// <summary>
    /// 把一帧画成带叠加层的显示图。showZones=false 用于样张照片编辑：照片上只保留画布自己画的草稿框。
    /// zone 判定结果只在带 verdict 的判定帧上渲染，避免上一次 NG 结果残留在实时预览或别的帧上。
    /// </summary>
    private Mat BuildAnchorView(Mat frame, AnchorVerdict verdict, bool showZones = true)
    {
        var canvas = frame.Clone();
        if (!_anchorOverlayOn) return canvas;
        if (showZones) DrawAnchorZones(canvas, verdict != null);
        if (verdict == null) return canvas;

        var color = verdict.Ok ? new CvScalar(0, 190, 60) : new CvScalar(40, 40, 220);
        DrawAnchorBoxes(canvas, verdict.HitList.Select(h => (h.ClsText, h.Ex, h.Ey, h.W, h.H)).ToList(), color, false);
        foreach (var hit in verdict.HitList)
        {
            var target = new CvPoint((int)Math.Round(hit.Ex), (int)Math.Round(hit.Ey));
            var center = new CvPoint((int)Math.Round(hit.Cx), (int)Math.Round(hit.Cy));
            Cv2.Circle(canvas, target, 8, new CvScalar(0, 200, 255), 2);
            Cv2.Line(canvas, target, center, new CvScalar(0, 200, 255), 1, LineTypes.AntiAlias);
        }
        var title = verdict.Ok ? "OK" : "NG " + verdict.ReasonCode;
        Cv2.PutText(canvas, title, new CvPoint(18, 48), Hershey.HersheyDuplex, 1.3, color, 3, LineTypes.AntiAlias);
        return canvas;
    }

    private static void DrawAnchorBoxes(Mat canvas, IReadOnlyList<(string Cls, double Ex, double Ey, double W, double H)> anchors, CvScalar color, bool marker)
    {
        foreach (var anchor in anchors)
        {
            var x = (int)Math.Round(anchor.Ex - anchor.W / 2);
            var y = (int)Math.Round(anchor.Ey - anchor.H / 2);
            var w = Math.Max(2, (int)Math.Round(anchor.W));
            var h = Math.Max(2, (int)Math.Round(anchor.H));
            Cv2.Rectangle(canvas, new CvRect(x, y, w, h), color, 2);
            var clsText = anchor.Cls ?? "?";
            if (OverlayText.IsAscii(clsText))
                Cv2.PutText(canvas, clsText, new CvPoint(x, Math.Max(16, y - 6)), Hershey.HersheySimplex, 0.55, color, 1, LineTypes.AntiAlias);
            else
                OverlayText.Chip(canvas, x, Math.Max(0, y - 24), clsText, OverlayText.ToColor(color), 12f);
            if (!marker) continue;
            var cx = (int)Math.Round(anchor.Ex);
            var cy = (int)Math.Round(anchor.Ey);
            Cv2.Line(canvas, new CvPoint(cx - 9, cy), new CvPoint(cx + 9, cy), color, 2, LineTypes.AntiAlias);
            Cv2.Line(canvas, new CvPoint(cx, cy - 9), new CvPoint(cx, cy + 9), color, 2, LineTypes.AntiAlias);
        }
    }

    // ---------------- 参数与文案 ----------------
    private void ApplyAnchorToUi()
    {
        var profile = _anchorProfile;
        if (profile == null) return;

        SetAnchorNum(_numAnchorShift, (decimal)profile.Tolerance.ShiftPx);
        SetAnchorNum(_numAnchorRot, (decimal)profile.Tolerance.RotDeg);
        SetAnchorNum(_numAnchorScale, (decimal)profile.Tolerance.ScaleTol);
        SetAnchorNum(_numAnchorRms, (decimal)profile.Tolerance.RmsPx);
        SetAnchorNum(_numAnchorConf, (decimal)profile.Infer.Conf);
        SetAnchorNum(_numAnchorMin, profile.Tolerance.MinAnchors);

        _lblAnchorCalibInfo.Text = profile.IsCalibrated
            ? $"基准：{profile.Calib.At} · {profile.Calib.Anchors.Count} 个锚点 · {profile.Calib.Width}×{profile.Calib.Height}"
            : "基准：还没有拍";

        if (_lvAnchorHits == null) return;
        _lvAnchorHits.Items.Clear();
        if (!profile.IsCalibrated) return;
        foreach (var anchor in profile.Calib.Anchors)
        {
            var item = new ListViewItem(anchor.Cls);
            item.SubItems.Add($"{anchor.Ex:0},{anchor.Ey:0}");
            item.SubItems.Add("—");
            item.SubItems.Add("—");
            item.SubItems.Add($"{anchor.Conf:P0}");
            _lvAnchorHits.Items.Add(item);
        }
    }

    private void ApplyAnchorToleranceToProfile()
    {
        var profile = _anchorProfile;
        if (profile == null) return;
        if (_numAnchorShift != null) profile.Tolerance.ShiftPx = (double)_numAnchorShift.Value;
        if (_numAnchorRot != null) profile.Tolerance.RotDeg = (double)_numAnchorRot.Value;
        if (_numAnchorScale != null) profile.Tolerance.ScaleTol = (double)_numAnchorScale.Value;
        if (_numAnchorRms != null) profile.Tolerance.RmsPx = (double)_numAnchorRms.Value;
        if (_numAnchorConf != null) profile.Infer.Conf = (double)_numAnchorConf.Value;
        if (_numAnchorMin != null) profile.Tolerance.MinAnchors = (int)_numAnchorMin.Value;
    }

    private void SaveAnchorTolerance()
    {
        if (_anchorProfile == null) return;
        ApplyAnchorToleranceToProfile();
        AnchorStore.Save(_anchorProfile);
        SetAnchorCamInfo("容差已保存。");
    }

    private static void SetAnchorNum(NumericUpDown num, decimal value)
    {
        if (num == null) return;
        if (value < num.Minimum) value = num.Minimum;
        if (value > num.Maximum) value = num.Maximum;
        num.Value = value;
    }

    private void SetAnchorBusy(bool busy, string text = null)
    {
        _anchorBusy = busy;
        if (_btnAnchorJudge != null) _btnAnchorJudge.Enabled = !busy;
        if (_btnAnchorCalib != null) _btnAnchorCalib.Enabled = !busy;
        if (busy && !string.IsNullOrEmpty(text)) SetAnchorCamInfo(text);
    }

    private void SetAnchorCamInfo(string text)
    {
        if (_lblAnchorCamInfo == null) return;
        _lblAnchorCamInfo.Text = text;
        _lblAnchorCamInfo.ForeColor = text.Contains("失败") || text.Contains("出错") ? Theme.Danger : Theme.Muted;
    }

    /// <summary>从一帧结果里挑锚点：只认 anchor* 类别，每类取最高分。</summary>
    private List<AnchorItem> PickAnchorItems(PredictionResult result, AnchorProfile profile, out string note)
    {
        note = "";
        var anchors = new List<AnchorItem>();
        var items = result?.Items ?? (IReadOnlyList<DetectionItem>)Array.Empty<DetectionItem>();
        var conf = Math.Clamp(profile?.Infer?.Conf ?? 0.35, 0.01, 0.99);
        var names = AnchorStore.AnchorClasses(_anchorSvc?.Info?.ClassNames ?? Array.Empty<string>());

        var best = new Dictionary<string, DetectionItem>(StringComparer.OrdinalIgnoreCase);
        foreach (var item in items)
        {
            if (item == null || item.Confidence < conf) continue;
            var cls = item.RawCategory ?? "";
            if (names.Count > 0 && !names.Contains(cls, StringComparer.OrdinalIgnoreCase)) continue;
            if (!best.TryGetValue(cls, out var current) || item.Confidence > current.Confidence) best[cls] = item;
        }

        foreach (var detect in best.Values.OrderBy(v => v.RawCategory, StringComparer.Ordinal))
        {
            anchors.Add(new AnchorItem
            {
                Cls = detect.RawCategory,
                Ex = detect.X + detect.W / 2.0,
                Ey = detect.Y + detect.H / 2.0,
                W = detect.W,
                H = detect.H,
                Conf = detect.Confidence,
            });
        }

        if (names.Count > 0)
        {
            var missed = names.Where(n => !best.ContainsKey(n)).ToList();
            if (missed.Count > 0) note = "本帧没检出：" + string.Join("、", missed);
        }
        return anchors;
    }
}
