using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using OpenCvSharp;
using Point = System.Drawing.Point;

namespace YoloFool;

/// <summary>一帧里的一个跟踪目标（快照，UI 线程只读）。</summary>
internal sealed class TrackedObject
{
    public int Id { get; init; }
    public DetectionItem Item { get; init; }
    public IReadOnlyList<PointF> Trail { get; init; } = Array.Empty<PointF>();
    /// <summary>被连续匹配的帧数（新建时为 0，匹配一次 +1），用于“连续 N 帧确认”。</summary>
    public int Age { get; init; }
    public int AreaPixels { get; set; }
    public double AreaPercent { get; set; }

    public TrackedObject Snapshot() => new()
    {
        Id = Id,
        Item = Item,
        Trail = Trail.ToArray(),
        Age = Age,
        AreaPixels = AreaPixels,
        AreaPercent = AreaPercent,
    };
}

/// <summary>IoU 贪心跟踪：足够小、可解释；速度比 SORT 慢一点，但不需要额外依赖。</summary>
internal sealed class SimpleTracker
{
    private sealed class Track
    {
        public int Id;
        public DetectionItem Item;
        public int Missing;
        public int Age;
        public readonly List<PointF> Trail = new();
    }

    private readonly List<Track> _tracks = new();
    private int _nextId = 1;

    public int CreatedCount => _nextId - 1;
    public int MaxAge { get; set; } = 10;

    public void Reset()
    {
        _tracks.Clear();
        _nextId = 1;
    }

    public IReadOnlyList<TrackedObject> Update(IReadOnlyList<DetectionItem> detections)
    {
        detections ??= Array.Empty<DetectionItem>();
        int existing = _tracks.Count;
        var candidates = new List<(int Track, int Det, float Score)>();
        for (int t = 0; t < _tracks.Count; t++)
        {
            for (int d = 0; d < detections.Count; d++)
            {
                if (_tracks[t].Item.ClassId != detections[d].ClassId) continue;
                float iou = Iou(_tracks[t].Item, detections[d]);
                if (iou >= 0.2f) candidates.Add((t, d, iou));
            }
        }
        candidates.Sort((a, b) => b.Score.CompareTo(a.Score));

        var usedTrack = new bool[existing];
        var usedDet = new bool[detections.Count];
        foreach (var c in candidates)
        {
            if (usedTrack[c.Track] || usedDet[c.Det]) continue;
            usedTrack[c.Track] = true;
            usedDet[c.Det] = true;
            UpdateTrack(_tracks[c.Track], detections[c.Det]);
        }

        for (int d = 0; d < detections.Count; d++)
        {
            if (usedDet[d]) continue;
            var track = new Track { Id = _nextId++, Item = detections[d] };
            AddCenter(track);
            _tracks.Add(track);
        }

        for (int t = existing - 1; t >= 0; t--)
        {
            if (usedTrack[t]) continue;
            _tracks[t].Missing++;
            if (_tracks[t].Missing > MaxAge) _tracks.RemoveAt(t);
        }

        return _tracks.Select(t => new TrackedObject
        {
            Id = t.Id,
            Item = t.Item,
            Trail = t.Trail.ToArray(),
            Age = t.Age,
        }).ToList();
    }

    private static void UpdateTrack(Track t, DetectionItem d)
    {
        t.Item = d;
        t.Missing = 0;
        t.Age++;
        AddCenter(t);
        if (t.Trail.Count > 48) t.Trail.RemoveAt(0);
    }

    private static void AddCenter(Track t) =>
        t.Trail.Add(new PointF(t.Item.X + t.Item.W / 2f, t.Item.Y + t.Item.H / 2f));

    private static float Iou(DetectionItem a, DetectionItem b)
    {
        int x1 = Math.Max(a.X, b.X), y1 = Math.Max(a.Y, b.Y);
        int x2 = Math.Min(a.X + a.W, b.X + b.W), y2 = Math.Min(a.Y + a.H, b.Y + b.H);
        int iw = x2 - x1, ih = y2 - y1;
        if (iw <= 0 || ih <= 0) return 0;
        float intersection = iw * ih;
        float union = a.W * (float)a.H + b.W * (float)b.H - intersection;
        return union <= 0 ? 0 : intersection / union;
    }
}

/// <summary>按“轨迹中心点跨越计数线”的方向计数。A→B 与 B→A 分开累计。</summary>
internal sealed class LineCounter
{
    public int AtoB { get; private set; }
    public int BtoA { get; private set; }
    public int Total => AtoB + BtoA;

    public void Reset()
    {
        AtoB = 0;
        BtoA = 0;
    }

    public void Update(IReadOnlyList<TrackedObject> tracks, Point a, Point b)
    {
        foreach (var t in tracks)
        {
            if (t.Trail.Count < 2) continue;
            var p = t.Trail[t.Trail.Count - 2];
            var q = t.Trail[t.Trail.Count - 1];
            double s1 = Cross(a, b, p);
            double s2 = Cross(a, b, q);
            if (s1 == 0 || s2 == 0 || Math.Sign(s1) == Math.Sign(s2)) continue;
            if (!SegmentsIntersect(a, b, p, q)) continue;
            if (s2 > 0) AtoB++;
            else BtoA++;
        }
    }

    private static double Cross(Point a, Point b, PointF p) =>
        (b.X - a.X) * (p.Y - a.Y) - (b.Y - a.Y) * (p.X - a.X);

    private static double Cross(Point a, Point b, Point c, Point d) =>
        (b.X - a.X) * (c.Y - a.Y) - (b.Y - a.Y) * (c.X - a.X);

    private static bool SegmentsIntersect(Point a, Point b, PointF c, PointF d)
    {
        double d1 = Side(c, d, a);
        double d2 = Side(c, d, b);
        double d3 = Side(a, b, c);
        double d4 = Side(a, b, d);
        return ((d1 > 0 && d2 < 0) || (d1 < 0 && d2 > 0)) &&
               ((d3 > 0 && d4 < 0) || (d3 < 0 && d4 > 0));
    }

    private static double Side(PointF a, PointF b, PointF p) =>
        (b.X - a.X) * (p.Y - a.Y) - (b.Y - a.Y) * (p.X - a.X);
}

/// <summary>交给 UI 的一帧结果（Bitmap 的所有权随对象转移，UI 用完 Dispose）。</summary>
internal sealed class VideoFrame : IDisposable
{
    public VideoFrame(Bitmap bitmap, IReadOnlyList<TrackedObject> tracks, long inferenceMs,
        double fps, int frameIndex, int width, int height, long maskAreaPixels,
        double maskAreaPercent, int aToB, int bToA, int totalTracks, bool segmentation = false)
    {
        Bitmap = bitmap;
        Tracks = tracks;
        InferenceMs = inferenceMs;
        Fps = fps;
        FrameIndex = frameIndex;
        Width = width;
        Height = height;
        MaskAreaPixels = maskAreaPixels;
        MaskAreaPercent = maskAreaPercent;
        AtoB = aToB;
        BtoA = bToA;
        TotalTracks = totalTracks;
        Segmentation = segmentation;
    }

    public Bitmap Bitmap { get; }
    public IReadOnlyList<TrackedObject> Tracks { get; }
    public long InferenceMs { get; }
    public double Fps { get; }
    public int FrameIndex { get; }
    public int Width { get; }
    public int Height { get; }
    public long MaskAreaPixels { get; }
    public double MaskAreaPercent { get; }
    public int AtoB { get; }
    public int BtoA { get; }
    public int TotalTracks { get; }
    public bool Segmentation { get; }

    public void Dispose() => Bitmap?.Dispose();
}

/// <summary>
/// 视频线程：读帧 → 分割推理 → 跟踪 → 越线计数 → 掩码合成 → 发布最新帧。
/// UI 用定时器取“最新一帧”，不跟随逐帧，避免界面卡死。
/// </summary>
internal sealed class VideoRunner : IDisposable
{
    private readonly object _lineLock = new();
    private readonly SimpleTracker _tracker = new();
    private readonly LineCounter _counter = new();
    private Thread _thread;
    private CancellationTokenSource _cts;
    private VideoFrame _latest;
    private volatile bool _running;
    private Point? _lineA;
    private Point? _lineB;

    public bool IsRunning => _running;
    public int FrameStep { get; set; } = 1;
    public bool Loop { get; set; }

    /// <summary>类别白名单（中文显示名、原始类别名或类别 id）。空集合表示不过滤。</summary>
    public HashSet<string> ClassFilter { get; } = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>每帧只保留置信度最高的一个目标。</summary>
    public bool OnlyBest { get; set; }

    /// <summary>面积/长宽比/同类去重/类别屏蔽（图片页与视频页共用同一套设置）。</summary>
    public FilterOptions Filter { get; set; } = new FilterOptions();

    /// <summary>可选：把 (原始类别, 类别 id) 翻成中文显示名，用于白名单匹配。</summary>
    public Func<string, int, string> LabelOf { get; set; }

    public event Action<string> Status;
    public event Action<Exception> Error;

    public void Start(string source, ModelOptions options)
    {
        if (_running) throw new InvalidOperationException("视频已经在运行");
        _cts = new CancellationTokenSource();
        _running = true;
        var token = _cts.Token;
        _thread = new Thread(() => Run(source, options, token))
        {
            IsBackground = true,
            Name = "YoloFool.VideoRunner",
        };
        _thread.Start();
    }

    public VideoFrame TakeLatest() => Interlocked.Exchange(ref _latest, null);

    public void SetLine(Point? a, Point? b)
    {
        lock (_lineLock)
        {
            _lineA = a;
            _lineB = b;
        }
    }

    public void Stop()
    {
        try { _cts?.Cancel(); } catch { }
        var thread = _thread;
        if (thread != null && thread != Thread.CurrentThread)
        {
            try { thread.Join(3000); } catch { }
        }
        _thread = null;
        _running = false;
        var latest = Interlocked.Exchange(ref _latest, null);
        latest?.Dispose();
    }

    private void Run(string source, ModelOptions options, CancellationToken token)
    {
        InferenceService svc = null;
        VideoCapture cap = null;
        try
        {
            Status?.Invoke("正在加载模型…");
            svc = new InferenceService();
            svc.Load(options);
            svc.Warmup();

            Status?.Invoke("模型就绪，正在打开视频…");
            cap = OpenSource(source);
            if (cap == null || !cap.IsOpened())
                throw new InvalidOperationException("无法打开视频源：" + source);

            int srcW = (int)cap.FrameWidth;
            int srcH = (int)cap.FrameHeight;
            double srcFps = cap.Fps;
            Status?.Invoke($"视频已打开：{srcW}×{srcH} @ {srcFps:F1} FPS");

            int index = 0, processed = 0;
            double fps = 0;
            var sw = Stopwatch.StartNew();

            while (!token.IsCancellationRequested)
            {
                using var frame = new Mat();
                if (!cap.Read(frame) || frame.Empty())
                {
                    if (Loop)
                    {
                        cap.Set(VideoCaptureProperties.PosFrames, 0);
                        continue;
                    }
                    break;
                }

                index++;
                if (FrameStep > 1 && (index - 1) % FrameStep != 0) continue;

                var result = svc.Predict(frame);
                var items = FilterItems(result.Items);
                var updated = _tracker.Update(items);
                // 连续 N 帧确认：新目标要连续出现 ConfirmFrames 帧才算数，避免闪烁误报
                var tracks = Filter.ConfirmFrames > 1
                    ? updated.Where(t => t.Age >= Filter.ConfirmFrames - 1).ToList()
                    : updated;
                if (result.MaskList.Count > 0)
                {
                    foreach (var t in tracks)
                    {
                        var mask = MatchMask(result.MaskList, t.Item);
                        if (mask == null) continue;
                        t.AreaPixels = mask.AreaPixels;
                        t.AreaPercent = mask.AreaPercent;
                    }
                }

                var line = GetLine();
                if (line.HasValue)
                    _counter.Update(tracks, line.Value.A, line.Value.B);

                using var composed = MaskRenderer.Composite(frame, result.MaskList);
                var bitmap = ToBitmap(composed);
                processed++;
                if (sw.ElapsedMilliseconds >= 700)
                {
                    fps = processed * 1000.0 / sw.ElapsedMilliseconds;
                    processed = 0;
                    sw.Restart();
                }

                long area = result.MaskList.Sum(m => (long)m.AreaPixels);
                double areaPercent = frame.Width <= 0 || frame.Height <= 0
                    ? 0
                    : area / (double)(frame.Width * frame.Height) * 100.0;
                Publish(new VideoFrame(bitmap, tracks, result.ElapsedMilliseconds, fps, index,
                    frame.Width, frame.Height, area, areaPercent,
                    _counter.AtoB, _counter.BtoA, _tracker.CreatedCount, result.MaskList.Count > 0));
            }
        }
        catch (Exception ex)
        {
            if (!token.IsCancellationRequested) Error?.Invoke(ex);
        }
        finally
        {
            cap?.Dispose();
            svc?.Dispose();
            _running = false;
            Status?.Invoke("已停止");
        }
    }

    /// <summary>按白名单过滤，再按需只留最高分的一个框。</summary>
    private IReadOnlyList<DetectionItem> FilterItems(IReadOnlyList<DetectionItem> items)
    {
        if (items == null || items.Count == 0) return Array.Empty<DetectionItem>();
        var screened = ResultFilter.Apply(items, Filter, SafeDisplay);
        IEnumerable<DetectionItem> q = screened;
        if (ClassFilter.Count > 0) q = q.Where(i => MatchesFilter(i));
        if (OnlyBest) q = q.Take(1);
        return q.ToList();
    }

    private string SafeDisplay(string raw, int classId)
    {
        try { return LabelOf?.Invoke(raw, classId); }
        catch { return raw; }
    }

    private bool MatchesFilter(DetectionItem item)
    {
        var raw = (item.RawCategory ?? "").Trim();
        string display = null;
        try { display = LabelOf?.Invoke(item.RawCategory, item.ClassId); } catch { }
        var idText = item.ClassId.ToString();
        foreach (var f in ClassFilter)
        {
            if (string.Equals(f, raw, StringComparison.OrdinalIgnoreCase)) return true;
            if (string.Equals(f, idText, StringComparison.Ordinal)) return true;
            if (!string.IsNullOrEmpty(display) && string.Equals(f, display, StringComparison.OrdinalIgnoreCase)) return true;
        }
        return false;
    }

    private (Point A, Point B)? GetLine()
    {
        lock (_lineLock)
        {
            if (_lineA == null || _lineB == null) return null;
            return (_lineA.Value, _lineB.Value);
        }
    }

    private void Publish(VideoFrame frame)
    {
        var old = Interlocked.Exchange(ref _latest, frame);
        old?.Dispose();
    }

    private static SegMaskInfo MatchMask(IReadOnlyList<SegMaskInfo> masks, DetectionItem item)
    {
        SegMaskInfo best = null;
        float bestIou = 0.25f;
        foreach (var m in masks)
        {
            float iou = Iou(m.Item, item);
            if (iou > bestIou)
            {
                bestIou = iou;
                best = m;
            }
        }
        return best;
    }

    private static float Iou(DetectionItem a, DetectionItem b)
    {
        int x1 = Math.Max(a.X, b.X), y1 = Math.Max(a.Y, b.Y);
        int x2 = Math.Min(a.X + a.W, b.X + b.W), y2 = Math.Min(a.Y + a.H, b.Y + b.H);
        int iw = x2 - x1, ih = y2 - y1;
        if (iw <= 0 || ih <= 0) return 0;
        float inter = iw * ih;
        float union = a.W * (float)a.H + b.W * (float)b.H - inter;
        return union <= 0 ? 0 : inter / union;
    }

    private static VideoCapture OpenSource(string source)
    {
        source = (source ?? "").Trim();
        if (source.Length > 0 && source.Length <= 2 && int.TryParse(source, out int camera))
            return new VideoCapture(camera);
        if (!File.Exists(source)) throw new FileNotFoundException("视频文件不存在", source);
        var cap = new VideoCapture(source);
        cap.Set(VideoCaptureProperties.BufferSize, 1);
        return cap;
    }

    private static Bitmap ToBitmap(Mat mat)
    {
        if (mat == null || mat.Empty()) throw new InvalidOperationException("空帧");
        using var bgr = new Mat();
        if (mat.Channels() == 3) mat.CopyTo(bgr);
        else if (mat.Channels() == 4) Cv2.CvtColor(mat, bgr, ColorConversionCodes.BGRA2BGR);
        else Cv2.CvtColor(mat, bgr, ColorConversionCodes.GRAY2BGR);

        var bmp = new Bitmap(bgr.Width, bgr.Height, PixelFormat.Format24bppRgb);
        var data = bmp.LockBits(new Rectangle(0, 0, bgr.Width, bgr.Height),
            ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
        try
        {
            long step = bgr.Step();
            int rowBytes = bgr.Width * 3;
            var row = new byte[rowBytes];
            for (int y = 0; y < bgr.Height; y++)
            {
                Marshal.Copy(IntPtr.Add(bgr.Data, (int)(y * step)), row, 0, rowBytes);
                Marshal.Copy(row, 0, IntPtr.Add(data.Scan0, y * data.Stride), rowBytes);
            }
        }
        finally
        {
            bmp.UnlockBits(data);
        }
        return bmp;
    }

    public void Dispose() => Stop();
}
