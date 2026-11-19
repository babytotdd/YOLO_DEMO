using System;
using System.Threading;
using OpenCvSharp;

namespace YoloFool;

/// <summary>常驻相机：后台线程连续取帧，界面只取最新一帧的克隆。</summary>
internal sealed class FixtureCamera : IDisposable
{
    private readonly object _gate = new();
    private VideoCapture _cap;
    private Thread _thread;
    private volatile bool _running;
    private Mat _latest;
    private long _frameId;
    private string _error = "";
    private volatile int _width;
    private volatile int _height;

    public bool IsOpen => _running;
    public int Width => _width;
    public int Height => _height;
    public string LastError { get { lock (_gate) return _error; } }

    public bool Open(FixtureCameraConfig cfg, out string error)
    {
        Close();
        error = "";
        var apis = new[] { VideoCaptureAPIs.DSHOW, VideoCaptureAPIs.MSMF, VideoCaptureAPIs.ANY };
        string last = "摄像头打不开";

        foreach (var api in apis)
        {
            VideoCapture cap = null;
            try
            {
                cap = new VideoCapture(cfg.Index, api);
                if (!cap.IsOpened())
                {
                    last = $"摄像头 {cfg.Index} 打不开（可能被其它程序占用）";
                    cap.Dispose();
                    continue;
                }

                cap.Set(VideoCaptureProperties.FrameWidth, cfg.Width);
                cap.Set(VideoCaptureProperties.FrameHeight, cfg.Height);
                if (!string.IsNullOrWhiteSpace(cfg.FourCC) && cfg.FourCC.Length == 4)
                {
                    var f = cfg.FourCC.ToUpperInvariant();
                    cap.Set(VideoCaptureProperties.FourCC,
                        VideoWriter.FourCC(f[0], f[1], f[2], f[3]));
                }
                if (cfg.Exposure is double exposure) cap.Set(VideoCaptureProperties.Exposure, exposure);
                if (cfg.Gain is double gain) cap.Set(VideoCaptureProperties.Gain, gain);

                using var probe = new Mat();
                if (!cap.Read(probe) || probe.Empty())
                {
                    last = "摄像头已打开但读不到画面";
                    cap.Dispose();
                    continue;
                }

                _width = probe.Width;
                _height = probe.Height;
                _cap = cap;
                _running = true;
                _thread = new Thread(Loop)
                {
                    IsBackground = true,
                    Name = "FixtureCamera",
                };
                _thread.Start();
                return true;
            }
            catch (Exception ex)
            {
                last = ex.Message;
                try { cap?.Dispose(); } catch { }
            }
        }

        error = last;
        return false;
    }

    private void Loop()
    {
        while (_running)
        {
            try
            {
                if (_cap == null) break;
                using var frame = new Mat();
                if (!_cap.Read(frame) || frame.Empty())
                {
                    Thread.Sleep(15);
                    continue;
                }

                lock (_gate)
                {
                    _latest?.Dispose();
                    _latest = frame.Clone();
                    _frameId++;
                }
            }
            catch (Exception ex)
            {
                lock (_gate) _error = "取帧异常: " + ex.Message;
                Thread.Sleep(100);
            }
        }
    }

    public bool TryGetLatest(out Mat frame, out long frameId)
    {
        lock (_gate)
        {
            frame = null;
            frameId = _frameId;
            if (_latest == null || _latest.Empty()) return false;
            frame = _latest.Clone();
            return true;
        }
    }

    /// <summary>等到比 afterId 更新的一帧；超时返回 null。</summary>
    public Mat WaitNewFrame(long afterId, int timeoutMs) =>
        WaitNewFrame(afterId, timeoutMs, out _);

    /// <summary>同上，并带回这一帧的序号，连续确认时用它只取更新的帧。</summary>
    public Mat WaitNewFrame(long afterId, int timeoutMs, out long frameId)
    {
        var deadline = Environment.TickCount64 + timeoutMs;
        while (Environment.TickCount64 < deadline)
        {
            if (TryGetLatest(out var frame, out var id))
            {
                if (id > afterId)
                {
                    frameId = id;
                    return frame;
                }
                frame.Dispose();
            }
            Thread.Sleep(5);
        }

        frameId = afterId;
        return null;
    }

    public void Close()
    {
        _running = false;
        var t = _thread;
        _thread = null;
        try { t?.Join(1200); } catch { }
        try { _cap?.Release(); } catch { }
        try { _cap?.Dispose(); } catch { }
        _cap = null;
        lock (_gate)
        {
            _latest?.Dispose();
            _latest = null;
            _error = "";
        }
    }

    public void Dispose() => Close();
}
