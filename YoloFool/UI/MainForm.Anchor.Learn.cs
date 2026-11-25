using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using OpenCvSharp;
using Mat = OpenCvSharp.Mat;

namespace YoloFool;

/// <summary>“识别目标”子页：冻结当前画面 → 在冻结帧上多框标注（可拖动/缩放/旋转/删除）→ 保存成样本。</summary>
public sealed partial class MainForm
{
    private sealed class AnchorDraft
    {
        public string Name { get; init; } = "";
        public int Id { get; init; }
        public Rectangle Rect { get; set; }
        public float Angle { get; set; }
    }

    private readonly List<AnchorDraft> _anchorDrafts = new();
    private Mat _anchorEditFrame;
    private bool _anchorPhotoSync;
    private bool _anchorDraftSync;
    private string _anchorSampleStem;
    // 刚【拍照】拍到、还没被框选确认的照片：先留在内存里，确认后才写进样本库。
    private Mat _anchorPendingShot;
    private string _anchorPendingStem;

    // ---------------- 拍照与照片载入 ----------------
    private async Task CaptureAnchorPhotoAsync()
    {
        if (_anchorProfile == null) return;
        if (!await EnsureAnchorCameraAsync())
        {
            SetAnchorCamInfo("相机没打开：先点“打开相机”。");
            return;
        }
        var cam = _anchorCam;
        if (cam == null) return;
        SetAnchorCamInfo("正在拍照…");
        Mat frame = null;
        try
        {
            long id = -1;
            frame = await Task.Run(() => cam.WaitNewFrame(_anchorShownFrame, 1500, out id));
            if (frame == null || frame.Empty())
            {
                SetAnchorCamInfo("1.5 秒内没有拿到新画面，再点一次【拍照】。");
                return;
            }
            _anchorShownFrame = Math.Max(_anchorShownFrame, id);
            var store = new AnchorSampleStore(_anchorModel);
            var stem = store.NextFileStem();
            // frame 在 finally 里会 Dispose，先留一份自己的。
            var shot = frame.Clone();
            ExitAnchorEditMode();
            _anchorPendingShot = shot;
            _anchorPendingStem = stem;
            _anchorSampleStem = stem;
            _anchorEditFrame = shot.Clone();
            _anchorDrafts.Clear();
            _anchorTargetPicking = true;
            _anchorZonePicking = false;
            ShowFrozenAnchorFrame();
            if (_picAnchor != null) _picAnchor.Editable = true;
            SyncDraftBoxes(-1);
            ShowAnchorDrafts();
            if (_lvAnchorSamples != null) _lvAnchorSamples.SelectedItems.Clear();
            if (_anchorTabs != null) _anchorTabs.SelectedIndex = 0;
            SetAnchorCamInfo("已拍下 " + stem + "（还没入队）：在照片上拖动框选，右键或回车确认后才算一张样本。");
        }
        catch (Exception ex)
        {
            SetAnchorCamInfo("拍照失败：" + ex.Message);
        }
        finally
        {
            frame?.Dispose();
        }
    }

    /// <summary>把一张已保存的照片载入画布（连同它的标注），之后加框/改框都会自动写回。</summary>
    private void LoadAnchorPhoto(string stem)
    {
        if (string.IsNullOrEmpty(stem) || string.IsNullOrEmpty(_anchorModel)) return;
        var store = new AnchorSampleStore(_anchorModel);
        var path = Path.Combine(store.ImagesDir, stem + ".jpg");
        if (!File.Exists(path))
        {
            SetAnchorCamInfo("照片不存在：" + stem);
            return;
        }
        try
        {
            var img = Cv2.ImRead(path, ImreadModes.Color);
            if (img == null || img.Empty())
            {
                SetAnchorCamInfo("照片读不出来：" + path);
                return;
            }
            ExitAnchorEditMode();
            _anchorEditFrame = img;
            _anchorDrafts.Clear();
            foreach (var b in store.ReadLabels(stem, img.Width, img.Height))
                _anchorDrafts.Add(new AnchorDraft { Name = b.ClassName, Id = b.ClassId, Rect = b.Rect, Angle = b.Angle });
            _anchorSampleStem = stem;
            _anchorTargetPicking = true;
            _anchorZonePicking = false;
            ShowFrozenAnchorFrame();
            if (_picAnchor != null) _picAnchor.Editable = true;
            SyncDraftBoxes(-1);
            ShowAnchorDrafts();
            if (_lblAnchorSampleInfo != null)
                _lblAnchorSampleInfo.Text = "正在编辑 " + stem + "：" + _anchorDrafts.Count + " 个框（自动保存）。";
        }
        catch (Exception ex)
        {
            SetAnchorCamInfo("载入照片失败：" + ex.Message);
        }
    }

    private void LoadAnchorPhotoFromSelection()
    {
        if (_anchorPhotoSync) return;
        var entry = _lvAnchorSamples != null && _lvAnchorSamples.SelectedItems.Count > 0
            ? _lvAnchorSamples.SelectedItems[0].Tag as AnchorSampleEntry
            : null;
        if (entry == null) return;
        if (string.Equals(entry.Stem, _anchorSampleStem, StringComparison.OrdinalIgnoreCase)) return;
        LoadAnchorPhoto(entry.Stem);
    }

    /// <summary>把当前照片的框写回磁盘（照片本身已存过，这里只更新标注）。</summary>
    private bool PersistAnchorPhoto()
    {
        if (_anchorEditFrame == null || _anchorEditFrame.Empty() || string.IsNullOrEmpty(_anchorSampleStem)) return false;
        var boxes = _anchorDrafts.Select(d => new AnchorLabelBox(d.Id, d.Name, d.Rect, d.Angle)).ToList();
        var store = new AnchorSampleStore(_anchorModel);
        try
        {
            // 这一张是刚拍的、还在内存里：等第一个框确认了再把整帧写进样本库。
            if (_anchorPendingShot != null && string.Equals(_anchorPendingStem, _anchorSampleStem, StringComparison.OrdinalIgnoreCase))
            {
                if (boxes.Count == 0) return false;
                if (!store.WritePhoto(_anchorSampleStem, _anchorPendingShot)) return false;
                _anchorPendingShot.Dispose();
                _anchorPendingShot = null;
                _anchorPendingStem = null;
            }
            store.UpdateFrame(_anchorSampleStem, _anchorEditFrame.Width, _anchorEditFrame.Height, boxes);
            return true;
        }
        catch (Exception ex)
        {
            SetAnchorCamInfo("保存标注失败：" + ex.Message);
            return false;
        }
    }

    // ---------------- 冻结帧与多框标注 ----------------
    private bool EnsureAnchorEditFrame()
    {
        if (_anchorEditFrame != null && !_anchorEditFrame.Empty()) return true;
        if (_anchorRaw == null || _anchorRaw.Empty()) return false;
        _anchorEditFrame = _anchorRaw.Clone();
        ShowFrozenAnchorFrame();
        return true;
    }

    private void ShowFrozenAnchorFrame()
    {
        if (_anchorEditFrame == null || _anchorEditFrame.Empty()) return;
        using var view = BuildAnchorView(_anchorEditFrame, null, showZones: false);
        ShowMatFrame(view);
    }

    private Rectangle ClampToDraftFrame(Rectangle rect)
    {
        var w = _anchorEditFrame?.Width ?? 0;
        var h = _anchorEditFrame?.Height ?? 0;
        if (w <= 0 || h <= 0) return rect;
        return Rectangle.Intersect(rect, new Rectangle(0, 0, w, h));
    }

    /// <summary>框选回调：命名后把框加成可继续编辑的草稿。</summary>
    private void AddAnchorTargetDraft(BoxDraft draft)
    {
        if (draft == null || _anchorProfile == null) return;
        if (_anchorEditFrame == null || _anchorEditFrame.Empty() || string.IsNullOrEmpty(_anchorSampleStem))
        {
            SetAnchorCamInfo("先点上方【拍照】存一张照片，再在照片上框选。");
            return;
        }
        var rect = ClampToDraftFrame(draft.Bounds);
        if (rect.Width < 8 || rect.Height < 8)
        {
            SetAnchorCamInfo("框太小了：至少 8×8 像素。");
            return;
        }

        var store = new AnchorSampleStore(_anchorModel);
        var name = AnchorNameDialog.Ask(this, "新建识别目标", "给框里的特征起个名字（丝印字或器件名，例如 wifi、SN、logo）：", store.ReadClasses());
        if (string.IsNullOrWhiteSpace(name))
        {
            SetAnchorCamInfo("没有命名，这个框没有加入。");
            return;
        }
        int id;
        try { id = store.EnsureClass(name); }
        catch (Exception ex)
        {
            SetAnchorCamInfo("命名失败：" + ex.Message);
            return;
        }

        _anchorDrafts.Add(new AnchorDraft { Name = name.Trim(), Id = id, Rect = rect, Angle = draft.Angle });
        // 不自动选中：选中态会画填充和手柄，且下一次按下会变成“拖旧框”，
        // 导致框第二个目标时旧框跟着鼠标跑、看起来像残留遮挡。
        SyncDraftBoxes(-1);
        ShowAnchorDrafts();
        PersistAnchorPhoto();
        ShowAnchorSamples();
        ShowAnchorTargets();
        RefreshZoneClassCombo();
        SetAnchorCamInfo("已加框“" + name + "”并保存。可继续框下一个，或点上方【拍照】拍下一张。");
    }

    /// <summary>把草稿同步到画布（画布只在 Editable 时允许拖动/缩放/旋转）。</summary>
    private void SyncDraftBoxes(int select)
    {
        if (_picAnchor == null) return;
        _anchorDraftSync = true;
        try
        {
            _picAnchor.Boxes = _anchorDrafts.Select(d => new BoxRecord
            {
                Cn = d.Name,
                En = d.Name,
                X = d.Rect.X,
                Y = d.Rect.Y,
                W = d.Rect.Width,
                H = d.Rect.Height,
                Angle = d.Angle,
            }).ToArray();
            _picAnchor.SelectedIndex = select;
        }
        finally
        {
            _anchorDraftSync = false;
        }
    }

    private void OnAnchorBoxEdited(object sender, BoxEdit edit)
    {
        if (_zoneEditing)
        {
            ApplyZoneEditBox(edit);
            return;
        }
        if (_anchorDrafts.Count == 0 || edit == null) return;
        if (edit.Index < 0 || edit.Index >= _anchorDrafts.Count) return;
        var draft = _anchorDrafts[edit.Index];
        draft.Rect = ClampToDraftFrame(edit.Bounds);
        draft.Angle = edit.Angle;
        SyncDraftBoxes(edit.Index);
        ShowAnchorDrafts();
        PersistAnchorPhoto();
        ShowAnchorSamples();
        var angle = Math.Abs(draft.Angle) > 0.01f ? "，旋转 " + draft.Angle.ToString("0.0") + "°" : "";
        SetAnchorCamInfo("已调整“" + draft.Name + "”并保存：" + draft.Rect.Width + "×" + draft.Rect.Height + "px" + angle + "。");
    }

    private void OnAnchorDraftSelectionChanged(object sender, int index)
    {
        if (_anchorDraftSync || _lvAnchorDrafts == null) return;
        if (index < 0 || index >= _lvAnchorDrafts.Items.Count) return;
        _anchorDraftSync = true;
        try
        {
            _lvAnchorDrafts.SelectedIndices.Clear();
            _lvAnchorDrafts.Items[index].Selected = true;
        }
        finally
        {
            _anchorDraftSync = false;
        }
        var draft = _anchorDrafts[index];
        SetAnchorCamInfo("选中“" + draft.Name + "”：" + draft.Rect.Width + "×" + draft.Rect.Height + "px。拖角点改大小，拖顶部圆点旋转，Delete 删除。");
    }

    private void SyncDraftSelectionToCanvas()
    {
        if (_anchorDraftSync || _picAnchor == null) return;
        var index = _lvAnchorDrafts != null && _lvAnchorDrafts.SelectedIndices.Count > 0 ? _lvAnchorDrafts.SelectedIndices[0] : -1;
        _anchorDraftSync = true;
        try { _picAnchor.SelectedIndex = index; }
        finally { _anchorDraftSync = false; }
    }

/// <summary>把选中的样本重新载入画布：改大小/旋转/删框后保存回同一张样本。</summary>
    private void EditAnchorSample()
    {
        var entry = _lvAnchorSamples != null && _lvAnchorSamples.SelectedItems.Count > 0 ? _lvAnchorSamples.SelectedItems[0].Tag as AnchorSampleEntry : null;
        if (entry == null)
        {
            SetAnchorCamInfo("先在样本列表里选中一条，再点“编辑选中”。");
            return;
        }
        try
        {
            var img = Cv2.ImRead(entry.ImagePath, ImreadModes.Color);
            if (img == null || img.Empty())
            {
                SetAnchorCamInfo("样本图读不出来：" + entry.ImagePath);
                return;
            }
            ExitAnchorEditMode();
            _anchorEditFrame = img;
            var boxes = new AnchorSampleStore(_anchorModel).ReadLabels(entry.Stem, img.Width, img.Height);
            _anchorDrafts.Clear();
            foreach (var b in boxes)
                _anchorDrafts.Add(new AnchorDraft { Name = b.ClassName, Id = b.ClassId, Rect = b.Rect, Angle = b.Angle });
            _anchorSampleStem = entry.Stem;
            _anchorTargetPicking = true;
            _anchorZonePicking = false;
            ShowFrozenAnchorFrame();
            if (_picAnchor != null) _picAnchor.Editable = true;
            SyncDraftBoxes(-1);
            ShowAnchorDrafts();
            SetAnchorCamInfo("正在编辑样本 " + entry.Stem + "：" + _anchorDrafts.Count + " 个框，改完点“保存本帧”写回。");
        }
        catch (Exception ex)
        {
            SetAnchorCamInfo("载入样本失败：" + ex.Message);
        }
    }

    private void SaveAnchorDraftFrame()
    {
        if (_anchorEditFrame == null || _anchorEditFrame.Empty())
        {
            SetAnchorCamInfo("先点“框选新目标”，画面会冻结在这一帧。");
            return;
        }
        if (_anchorDrafts.Count == 0)
        {
            SetAnchorCamInfo("本帧还没有框：先框一个再保存。");
            return;
        }
        var boxes = _anchorDrafts.Select(d => new AnchorLabelBox(d.Id, d.Name, d.Rect, d.Angle)).ToList();
        var store = new AnchorSampleStore(_anchorModel);
        var editing = _anchorSampleStem;
        string stem;
        try
        {
            if (!string.IsNullOrEmpty(editing))
            {
                store.UpdateFrame(editing, _anchorEditFrame.Width, _anchorEditFrame.Height, boxes);
                stem = editing + "（已写回）";
            }
            else
            {
                stem = store.SaveFrame(_anchorEditFrame, boxes);
            }
        }
        catch (Exception ex)
        {
            SetAnchorCamInfo("保存失败：" + ex.Message);
            return;
        }
        var count = _anchorDrafts.Count;
        ExitAnchorEditMode();
        ShowAnchorTargets();
        ShowAnchorSamples();
        RefreshZoneClassCombo();
        SetAnchorCamInfo("已保存 " + stem + "：" + count + " 个框。下方样本列表可点选查看标注效果。");
    }

    private void DeleteAnchorDraft()
    {
        var index = _picAnchor != null ? _picAnchor.SelectedIndex : -1;
        if (index < 0 && _lvAnchorDrafts != null && _lvAnchorDrafts.SelectedIndices.Count > 0) index = _lvAnchorDrafts.SelectedIndices[0];
        if (index < 0 || index >= _anchorDrafts.Count)
        {
            SetAnchorCamInfo("先在画布或列表里点中一个框，再点“删除选中”。");
            return;
        }
        var draft = _anchorDrafts[index];
        if (MessageBox.Show(this, "删除这个框“" + draft.Name + "”？", "识别目标", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK) return;
        _anchorDrafts.RemoveAt(index);
        SyncDraftBoxes(_anchorDrafts.Count == 0 ? -1 : Math.Min(index, _anchorDrafts.Count - 1));
        ShowAnchorDrafts();
        PersistAnchorPhoto();
        ShowAnchorSamples();
        ShowAnchorTargets();
        SetAnchorCamInfo(_anchorDrafts.Count == 0
            ? "已删掉本张最后一个框；照片还在，可重新框，或点“删除本张”。"
            : "已删除 1 个框，本张还剩 " + _anchorDrafts.Count + " 个。");
    }

    private void DeleteAnchorSample()
    {
        var entry = _lvAnchorSamples != null && _lvAnchorSamples.SelectedItems.Count > 0
            ? _lvAnchorSamples.SelectedItems[0].Tag as AnchorSampleEntry
            : null;
        var stem = _anchorSampleStem ?? entry?.Stem;
        if (string.IsNullOrEmpty(stem))
        {
            SetAnchorCamInfo("先在照片列表里选中一张。");
            return;
        }
        var count = string.Equals(stem, _anchorSampleStem, StringComparison.OrdinalIgnoreCase) ? _anchorDrafts.Count : entry?.BoxCount ?? 0;
        if (MessageBox.Show(this, "删除照片 " + stem + "（含 " + count + " 个标注）？", "识别目标", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != DialogResult.OK) return;
        try { new AnchorSampleStore(_anchorModel).DeleteSample(stem); }
        catch (Exception ex) { SetAnchorCamInfo("删除失败：" + ex.Message); return; }
        if (string.Equals(stem, _anchorSampleStem, StringComparison.OrdinalIgnoreCase)) ExitAnchorEditMode();
        ShowAnchorSamples();
        ShowAnchorTargets();
        SetAnchorCamInfo("已删除照片 " + stem + "。");
    }

    private void CancelAnchorDraftFrame()
    {
        if (_anchorEditFrame == null && _anchorDrafts.Count == 0)
        {
            SetAnchorCamInfo("当前没有正在编辑的帧。");
            return;
        }
        if (_anchorDrafts.Count > 0 && MessageBox.Show(this, "放弃本帧的 " + _anchorDrafts.Count + " 个框？", "识别目标", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK) return;
        ExitAnchorEditMode();
        SetAnchorCamInfo("已取消本帧标注，预览恢复实时画面。");
    }

    private void ExitAnchorEditMode()
    {
        // 画布上还没确认的框也要清掉，否则拍下一张时旧方框会留在新画面上。
        _picAnchor?.CancelPending();
        _anchorDrafts.Clear();
        if (_picAnchor != null)
        {
            _anchorDraftSync = true;
            try
            {
                _picAnchor.Boxes = Array.Empty<BoxRecord>();
                _picAnchor.SelectedIndex = -1;
            }
            finally { _anchorDraftSync = false; }
            _picAnchor.Editable = false;
        }
        _anchorTargetPicking = false;
        _anchorSampleStem = null;
        _anchorPendingStem = null;
        if (_anchorPendingShot != null)
        {
            _anchorPendingShot.Dispose();
            _anchorPendingShot = null;
        }
        if (_anchorEditFrame != null)
        {
            _anchorEditFrame.Dispose();
            _anchorEditFrame = null;
        }
        _anchorShownFrame = -1;
        _anchorCalibView = false;
        ShowAnchorDrafts();
    }

    private void ShowAnchorDrafts()
    {
        if (_lvAnchorDrafts == null) return;
        _lvAnchorDrafts.BeginUpdate();
        try
        {
            _lvAnchorDrafts.Items.Clear();
            foreach (var d in _anchorDrafts)
            {
                var item = new ListViewItem(d.Name);
                item.SubItems.Add(d.Rect.X + "," + d.Rect.Y + "  " + d.Rect.Width + "×" + d.Rect.Height);
                item.SubItems.Add(Math.Abs(d.Angle) < 0.01f ? "0°" : d.Angle.ToString("0.0") + "°");
                _lvAnchorDrafts.Items.Add(item);
            }
        }
        finally { _lvAnchorDrafts.EndUpdate(); }
        if (_lblAnchorLearnInfo != null)
        {
            _lblAnchorLearnInfo.Text = _anchorEditFrame == null
                ? "① 点上方【拍照】存一张照片 ② 点“框选目标” ③ 拖动框选后右键/回车确认"
                : "正在标注 " + (_anchorSampleStem ?? "") + "：" + _anchorDrafts.Count + " 个框"
                  + (_anchorPendingShot != null ? "（第一次框选右键确认后入队）。可拖动/缩放/旋转，Delete 删除。" : "（自动保存）。可拖动/缩放/旋转，Delete 删除。");
        }
    }

    /// <summary>把模型里的类别名和每个类的样本框数显示出来，解决“看不到模型里有什么标签”。</summary>
    private void UpdateAnchorClassLabel()
    {
        if (_lblAnchorClasses == null) return;
        var names = _anchorSvc?.Info?.ClassNames?.Where(n => !string.IsNullOrWhiteSpace(n)).ToList() ?? new List<string>();
        List<AnchorSampleStat> stats = null;
        if (!string.IsNullOrEmpty(_anchorModel))
        {
            try { stats = new AnchorSampleStore(_anchorModel).Stats(); } catch { }
        }
        if (names.Count == 0)
        {
            _lblAnchorClasses.Text = "模型里还没有标签：先在照片上框选目标并训练，或点上方“复制当前模型”放一个已训练模型。";
            _lblAnchorClasses.ForeColor = Theme.Warn;
            return;
        }
        var counts = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        if (stats != null)
            foreach (var s in stats) counts[s.Name] = s.Count;
        var parts = names.Select(n => counts.TryGetValue(n, out var c) ? n + "×" + c : n + "(无样本)");
        _lblAnchorClasses.Text = "模型标签（" + names.Count + "）：" + string.Join("、", parts);
        _lblAnchorClasses.ForeColor = Theme.Muted;
    }

    private void ShowAnchorSamples()
    {
        if (_lvAnchorSamples == null) return;
        var keep = _anchorSampleStem;
        if (string.IsNullOrEmpty(keep) && _lvAnchorSamples.SelectedItems.Count > 0)
            keep = (_lvAnchorSamples.SelectedItems[0].Tag as AnchorSampleEntry)?.Stem;
        List<AnchorSampleEntry> list;
        try { list = string.IsNullOrEmpty(_anchorModel) ? new List<AnchorSampleEntry>() : new AnchorSampleStore(_anchorModel).ListSamples(300); }
        catch (Exception ex)
        {
            if (_lblAnchorSampleInfo != null) _lblAnchorSampleInfo.Text = "读取照片失败：" + ex.Message;
            return;
        }
        _anchorPhotoSync = true;
        _lvAnchorSamples.BeginUpdate();
        try
        {
            _lvAnchorSamples.Items.Clear();
            foreach (var s in list)
            {
                var item = new ListViewItem(s.Stem) { Tag = s };
                item.SubItems.Add(s.Classes);
                item.SubItems.Add(s.BoxCount.ToString());
                item.SubItems.Add(s.Time.ToString("MM-dd HH:mm"));
                _lvAnchorSamples.Items.Add(item);
                if (!string.IsNullOrEmpty(keep) && string.Equals(keep, s.Stem, StringComparison.OrdinalIgnoreCase))
                {
                    item.Selected = true;
                    item.EnsureVisible();
                }
            }
        }
        finally
        {
            _lvAnchorSamples.EndUpdate();
            _anchorPhotoSync = false;
        }
        if (_lblAnchorSampleInfo != null)
            _lblAnchorSampleInfo.Text = "照片 " + list.Count + " 张（" + list.Count(s => s.BoxCount > 0) + " 张已标注）；点一条就把那张照片和它的框载入左侧继续改。";
        ShowAnchorSamplePreview();
    }

    private void ShowAnchorSamplePreview()
    {
        var entry = _lvAnchorSamples != null && _lvAnchorSamples.SelectedItems.Count > 0 ? _lvAnchorSamples.SelectedItems[0].Tag as AnchorSampleEntry : null;
        if (entry == null || _picAnchorSample == null) return;
        try
        {
            using var img = Cv2.ImRead(entry.ImagePath, ImreadModes.Color);
            if (img == null || img.Empty())
            {
                if (_lblAnchorSampleInfo != null) _lblAnchorSampleInfo.Text = "样本图读不出来：" + entry.ImagePath;
                return;
            }
            var boxes = new AnchorSampleStore(_anchorModel).ReadLabels(entry.Stem, img.Width, img.Height);
            foreach (var b in boxes)
            {
                var corners = RotatedBox.Corners(b.Rect.X, b.Rect.Y, b.Rect.Width, b.Rect.Height, b.Angle);
                var pts = new OpenCvSharp.Point[corners.Length];
                for (var i = 0; i < corners.Length; i++) pts[i] = new OpenCvSharp.Point((int)Math.Round(corners[i].X), (int)Math.Round(corners[i].Y));
                Cv2.Polylines(img, new[] { pts }, true, new Scalar(80, 220, 120), 2);
                var tx = pts[0].X;
                var ty = Math.Max(18, pts[0].Y - 6);
                if (OverlayText.IsAscii(b.ClassName))
                    Cv2.PutText(img, b.ClassName, new OpenCvSharp.Point(tx, ty), HersheyFonts.HersheySimplex, 0.6, new Scalar(80, 220, 120), 2, LineTypes.AntiAlias);
                else
                    OverlayText.Chip(img, tx, Math.Max(0, ty - 20), b.ClassName, Color.FromArgb(80, 220, 120), 12f, Color.FromArgb(20, 20, 20));
            }
            var bmp = MatToBitmap(img);
            var old = _picAnchorSample.Image;
            _picAnchorSample.SetFrame(bmp);
            old?.Dispose();
            if (_lblAnchorSampleInfo != null) _lblAnchorSampleInfo.Text = entry.Stem + " · " + boxes.Count + " 个框 · " + entry.Time.ToString("MM-dd HH:mm");
        }
        catch (Exception ex)
        {
            if (_lblAnchorSampleInfo != null) _lblAnchorSampleInfo.Text = "预览失败：" + ex.Message;
        }
    }

    private void AnchorCanvasKeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Escape)
        {
            if (_zoneEditing)
            {
                CancelAnchorZoneEdit("已取消调整，范围没有改动。");
                e.Handled = true;
                e.SuppressKeyPress = true;
                return;
            }
            if (_anchorTargetPicking)
            {
                CancelAnchorPicking();
                if (_anchorEditFrame != null) ShowFrozenAnchorFrame();
                else if (_anchorRaw != null && !_anchorRaw.Empty()) ShowAnchorFrame(_anchorRaw, _anchorLastVerdict);
                if (_lblAnchorSampleInfo != null) _lblAnchorSampleInfo.Text = "已退出框选，本张照片没有变化。";
                e.Handled = true;
                e.SuppressKeyPress = true;
                return;
            }
        }
        if (_zoneEditing && e.KeyCode == Keys.Enter)
        {
            SaveZoneEdit();
            e.Handled = true;
            e.SuppressKeyPress = true;
            return;
        }
        if (e.KeyCode != Keys.Delete && e.KeyCode != Keys.Back) return;
        var hasDraft = _anchorDrafts.Count > 0 && _picAnchor != null && _picAnchor.SelectedIndex >= 0;
        var hasSample = _lvAnchorSamples != null && _lvAnchorSamples.SelectedItems.Count > 0;
        if (!hasDraft && !hasSample) return;
        DeleteAnchorDraft();
        e.Handled = true;
        e.SuppressKeyPress = true;
    }
}