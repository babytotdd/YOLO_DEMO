using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cv2 = OpenCvSharp.Cv2;
using CvPoint = OpenCvSharp.Point;
using CvScalar = OpenCvSharp.Scalar;
using LineTypes = OpenCvSharp.LineTypes;
using Mat = OpenCvSharp.Mat;

namespace YoloFool;

/// <summary>
/// 针床定位V2 第三页「③ 拍基准 · 对比」：拍基准（绿色按钮）、查看当前基准、现状拍照与基准对比、返回实时。
/// _anchorCalibView 为 true 时实时预览不再刷新画面，保证基准图/对比图不被下一帧覆盖。
/// </summary>
public sealed partial class MainForm
{
    private bool _anchorCalibView;
    private ListView _lvAnchorCalibHits;
    private Label _lblAnchorCalibCompare;

    private Control BuildAnchorCalibTab()
    {
        var root = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 4, BackColor = Theme.Bg };
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

        var hint = new Label
        {
            AutoSize = true,
            Font = Theme.Small,
            ForeColor = Theme.Muted,
            Margin = new Padding(0, 0, 0, 6),
            Text = "① 放一块确认到位的板 → 点【拍基准】。\r\n② 换板后点【现状拍照对比】：绿框 = 基准位置，红圈 = 本次实测，黄线 = 偏差。\r\n③ 点【返回实时】回到相机实时预览。",
        };
        root.Controls.Add(hint, 0, 0);

        var bar = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, WrapContents = true, BackColor = Theme.Bg, Margin = new Padding(0, 0, 0, 6) };
        _btnAnchorCalib = Theme.Success("拍基准");
        _btnAnchorCalib.Padding = new Padding(8, 2, 8, 2);
        _btnAnchorCalib.Margin = new Padding(0, 0, 6, 0);
        _btnAnchorCalib.Click += async (_, __) => await AnchorCalibrateAsync();
        bar.Controls.Add(_btnAnchorCalib);

        var btnView = Theme.Ghost("查看当前基准");
        btnView.Padding = new Padding(8, 2, 8, 2);
        btnView.Margin = new Padding(0, 0, 6, 0);
        btnView.Click += (_, __) => ShowAnchorCalibImage();
        bar.Controls.Add(btnView);

        var btnCompare = Theme.Ghost("现状拍照对比");
        btnCompare.Padding = new Padding(8, 2, 8, 2);
        btnCompare.Margin = new Padding(0, 0, 6, 0);
        btnCompare.Click += async (_, __) => await AnchorCompareCalibAsync();
        bar.Controls.Add(btnCompare);

        var btnLive = Theme.Ghost("返回实时");
        btnLive.Padding = new Padding(8, 2, 8, 2);
        btnLive.Click += (_, __) => BackToAnchorLive();
        bar.Controls.Add(btnLive);
        root.Controls.Add(bar, 0, 1);

        _lblAnchorCalibCompare = new Label
        {
            AutoSize = true,
            Font = Theme.Body,
            ForeColor = Theme.Muted,
            Margin = new Padding(0, 0, 0, 6),
            Text = "还没有对比：拍完基准后点【现状拍照对比】。",
        };
        root.Controls.Add(_lblAnchorCalibCompare, 0, 2);

        _lvAnchorCalibHits = new ListView
        {
            Dock = DockStyle.Fill,
            View = View.Details,
            FullRowSelect = true,
            MultiSelect = false,
            HideSelection = false,
            HeaderStyle = ColumnHeaderStyle.Nonclickable,
            Font = Theme.Small,
            BackColor = Theme.Card,
            ForeColor = Theme.Ink,
            BorderStyle = BorderStyle.None,
            Margin = Padding.Empty,
        };
        _lvAnchorCalibHits.Columns.Add("锚点", 76);
        _lvAnchorCalibHits.Columns.Add("基准", 104);
        _lvAnchorCalibHits.Columns.Add("实测", 104);
        _lvAnchorCalibHits.Columns.Add("Δx", 64);
        _lvAnchorCalibHits.Columns.Add("Δy", 64);
        _lvAnchorCalibHits.Columns.Add("置信", 56);
        root.Controls.Add(AnchorCard("基准 / 实测对照", _lvAnchorCalibHits, DockStyle.Fill), 0, 3);
        return root;
    }

    /// <summary>把已保存的基准图（带绿框）显示在画布上，并停掉实时预览。</summary>
    private void ShowAnchorCalibImage()
    {
        if (string.IsNullOrEmpty(_anchorModel))
        {
            SetAnchorCamInfo("还没有选机种。");
            return;
        }
        var profile = _anchorProfile;
        if (profile == null || !profile.IsCalibrated)
        {
            SetAnchorCamInfo("还没有基准：先把确认到位的板放上针床，点【拍基准】。");
            return;
        }

        Mat img = null;
        var overlayPath = AnchorStore.FileOf(_anchorModel, AnchorStore.OverlayFile);
        if (File.Exists(overlayPath)) img = Cv2.ImRead(overlayPath, OpenCvSharp.ImreadModes.Color);
        if (img == null || img.Empty())
        {
            var calibPath = AnchorStore.FileOf(_anchorModel, AnchorStore.CalibFile);
            if (!File.Exists(calibPath))
            {
                SetAnchorCamInfo("基准图不见了：" + calibPath);
                return;
            }
            img = Cv2.ImRead(calibPath, OpenCvSharp.ImreadModes.Color);
            if (img == null || img.Empty())
            {
                SetAnchorCamInfo("基准图读不出来：" + calibPath);
                return;
            }
            DrawAnchorBoxes(img, profile.Calib.Anchors.Select(a => (a.Cls, a.Ex, a.Ey, a.W, a.H)).ToList(), new CvScalar(0, 190, 60), true);
            OverlayText.Chip(img, 18, 10, "基准", OverlayText.ToColor(new CvScalar(0, 190, 60)), 15f);
        }

        try
        {
            _anchorCalibView = true;
            ShowMatFrame(img);
        }
        finally
        {
            img.Dispose();
        }

        FillAnchorCalibBase(profile);
        _lblAnchorCalibCompare.Text = "当前基准：" + profile.Calib.At + " · " + profile.Calib.Anchors.Count + " 个锚点 · " + profile.Calib.Width + "×" + profile.Calib.Height;
        _lblAnchorCalibCompare.ForeColor = Theme.Ok;
    }

    /// <summary>现拍一张，和基准比：绿框=基准期望位置，红圈=本次实测，黄线=偏差。</summary>
    private async Task AnchorCompareCalibAsync()
    {
        if (_anchorBusy) return;
        var profile = _anchorProfile;
        if (profile == null)
        {
            SetAnchorCamInfo("还没有选机种。");
            return;
        }
        if (!profile.IsCalibrated)
        {
            SetAnchorCamInfo("还没有基准：先把确认到位的板放上针床，点【拍基准】。");
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
        SetAnchorBusy(true, "正在拍当前画面…");
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
            var verdict = AnchorJudge.Judge(result, profile, frame.Width, frame.Height);

            using (var overlay = frame.Clone())
            {
                DrawAnchorBoxes(overlay, profile.Calib.Anchors.Select(a => (a.Cls, a.Ex, a.Ey, a.W, a.H)).ToList(), new CvScalar(0, 190, 60), false);
                foreach (var hit in verdict.HitList)
                {
                    var target = new CvPoint((int)Math.Round(hit.Ex), (int)Math.Round(hit.Ey));
                    var center = new CvPoint((int)Math.Round(hit.Cx), (int)Math.Round(hit.Cy));
                    Cv2.Line(overlay, target, center, new CvScalar(0, 200, 255), 2, LineTypes.AntiAlias);
                    Cv2.Circle(overlay, center, 10, new CvScalar(40, 40, 220), 2, LineTypes.AntiAlias);
                    var delta = "dx" + hit.Dx.ToString("+0.0;-0.0;0") + " dy" + hit.Dy.ToString("+0.0;-0.0;0");
                    OverlayText.Chip(overlay, center.X + 12, center.Y + 12, delta, OverlayText.ToColor(new CvScalar(40, 40, 220)), 12f);
                }

                var color = verdict.Ok ? new CvScalar(0, 190, 60) : new CvScalar(40, 40, 220);
                var head = verdict.Ok ? "对比 OK" : "对比 NG · " + verdict.ReasonCode;
                OverlayText.Chip(overlay, 18, 10, head, OverlayText.ToColor(color), 15f);
                var shift = "Δx " + verdict.MeanDx.ToString("+0.0;-0.0;0") + "px · Δy " + verdict.MeanDy.ToString("+0.0;-0.0;0") + "px";
                OverlayText.Chip(overlay, 18, 46, shift, OverlayText.ToColor(new CvScalar(70, 70, 70)), 13f);

                _anchorCalibView = true;
                ShowMatFrame(overlay);
            }

            FillAnchorCalibHits(verdict, profile);
            var note = verdict.HitCount + "/" + verdict.NeedCount + " 个锚点 · RMS " + verdict.RmsPx.ToString("0.0") + "px";
            _lblAnchorCalibCompare.Text = verdict.Ok
                ? "对比结果：到位（" + note + "）"
                : "对比结果：不到位 · " + verdict.ReasonName + "（" + note + "）";
            _lblAnchorCalibCompare.ForeColor = verdict.Ok ? Theme.Ok : Theme.Danger;
        }
        catch (Exception ex)
        {
            SetAnchorCamInfo("对比出错：" + ex.Message);
        }
        finally
        {
            frame?.Dispose();
            SetAnchorBusy(false);
        }
    }

    /// <summary>解除基准/对比画面，回到实时预览。</summary>
    private void BackToAnchorLive()
    {
        _anchorCalibView = false;
        if (_anchorRaw != null && !_anchorRaw.Empty()) ShowAnchorFrame(_anchorRaw, _anchorLastVerdict);
        if (_lblAnchorCalibCompare != null)
        {
            _lblAnchorCalibCompare.Text = "已回到实时预览。";
            _lblAnchorCalibCompare.ForeColor = Theme.Muted;
        }
        SetAnchorCamInfo("已回到实时预览。");
    }

    private void FillAnchorCalibBase(AnchorProfile profile)
    {
        if (_lvAnchorCalibHits == null || profile == null) return;
        _lvAnchorCalibHits.BeginUpdate();
        try
        {
            _lvAnchorCalibHits.Items.Clear();
            foreach (var anchor in profile.Calib.Anchors)
            {
                var item = new ListViewItem(anchor.Cls) { ForeColor = Theme.Ink };
                item.SubItems.Add(anchor.Ex.ToString("0") + "," + anchor.Ey.ToString("0"));
                item.SubItems.Add("—");
                item.SubItems.Add("—");
                item.SubItems.Add("—");
                item.SubItems.Add(anchor.Conf.ToString("P0"));
                _lvAnchorCalibHits.Items.Add(item);
            }
        }
        finally
        {
            _lvAnchorCalibHits.EndUpdate();
        }
    }

    private void FillAnchorCalibHits(AnchorVerdict verdict, AnchorProfile profile)
    {
        if (_lvAnchorCalibHits == null) return;
        var tol = profile?.Tolerance?.ShiftPx ?? 12;
        _lvAnchorCalibHits.BeginUpdate();
        try
        {
            _lvAnchorCalibHits.Items.Clear();
            foreach (var hit in verdict?.HitList ?? Array.Empty<AnchorHit>())
            {
                var over = Math.Abs(hit.Dx) > tol || Math.Abs(hit.Dy) > tol;
                var item = new ListViewItem(hit.ClsText) { ForeColor = over ? Theme.Danger : Theme.Ink };
                item.SubItems.Add(hit.Ex.ToString("0") + "," + hit.Ey.ToString("0"));
                item.SubItems.Add(hit.Cx.ToString("0") + "," + hit.Cy.ToString("0"));
                item.SubItems.Add(hit.Dx.ToString("+0.0;-0.0;0"));
                item.SubItems.Add(hit.Dy.ToString("+0.0;-0.0;0"));
                item.SubItems.Add(hit.Conf.ToString("P0"));
                _lvAnchorCalibHits.Items.Add(item);
            }
        }
        finally
        {
            _lvAnchorCalibHits.EndUpdate();
        }
    }
}
