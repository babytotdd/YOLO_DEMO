using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using OpenCvSharp;

namespace YoloFool.Core.MultiCam;

/// <summary>一路相机的状态。</summary>
internal enum CameraState
{
    Idle = 0,
    Opening = 1,
    Running = 2,
    Failed = 3,
}

/// <summary>UI 每帧/每秒读一次的指标快照（值类型，读的时候不用再持锁）。</summary>
internal readonly struct CameraMetrics
{
    public readonly CameraState State;
    public readonly string Error;
    public readonly int SrcWidth;
    public readonly int SrcHeight;
    public readonly int ReqWidth;
    public readonly int ReqHeight;
    public readonly string Api;
    public readonly string PixelFormat;
    public readonly double Fps;
    public readonly double FrameMs;
    public readonly double CpuPercent;
    public readonly double CpuAvgPercent;
    public readonly double PeakCpuPercent;
    public readonly double AllocMbPerSec;
    public readonly double BufferMb;
    public readonly long Frames;
    public readonly double ElapsedSec;
    public readonly double OpenMs;

    public CameraMetrics(CameraState state, string error, int sw, int sh, int reqW, int reqH, string api,
        string pixelFormat, double fps, double frameMs, double cpu, double cpuAvg, double peakCpu, double allocMb,
        double bufferMb, long frames, double elapsed, double openMs)
    {
        State = state;
        Error = error ?? "";
        SrcWidth = sw;
        SrcHeight = sh;
        ReqWidth = reqW;
        ReqHeight = reqH;
        Api = api ?? "";
        PixelFormat = pixelFormat ?? "";
        Fps = fps;
        FrameMs = frameMs;
        CpuPercent = cpu;
        CpuAvgPercent = cpuAvg;
        PeakCpuPercent = peakCpu;
        AllocMbPerSec = allocMb;
        BufferMb = bufferMb;
        Frames = frames;
        ElapsedSec = elapsed;
        OpenMs = openMs;
    }

    /// <summary>请求的分辨率是否被驱动接受（部分相机只能给固定档位）。</summary>
    public bool SizeHonored => SrcWidth == ReqWidth && SrcHeight == ReqHeight;
}

/// <summary>一路相机的采集参数。改完要重新打开才生效（驱动只在打开时协商格式）。</summary>
internal sealed class CameraSettings
{
    public int Width = 1280;
    public int Height = 720;
    public int Fps = 30;
    /// <summary>自动 / MJPG / YUY2。多数 USB 相机实际只给 YUY2。</summary>
    public string PixelFormat = "自动";
    public bool AutoExposure = true;
    /// <summary>手动曝光档位（DSHOW 一般是 -13~-1，越小越暗，驱动自己会截断）。</summary>
    public double Exposure = -6;
    public double Gain;
    public double Brightness;
    public double Contrast;
    public double Saturation;
    public bool AutoWhiteBalance = true;
    public double WhiteBalance = 4600;

    public CameraSettings Clone() => (CameraSettings)MemberwiseClone();
}

/// <summary>取当前线程的 CPU 时间（GetThreadTimes 的伪句柄只能在本线程内用）。</summary>
internal static class ThreadCpu
{
    [StructLayout(LayoutKind.Sequential)]
    private struct FileTime
    {
        public uint Low;
        public uint High;
    }

    [DllImport("kernel32.dll")]
    private static extern IntPtr GetCurrentThread();

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool GetThreadTimes(IntPtr handle, out FileTime creation, out FileTime exit,
        out FileTime kernel, out FileTime user);

    public static double Seconds()
    {
        if (!GetThreadTimes(GetCurrentThread(), out _, out _, out var k, out var u)) return 0;
        return (((ulong)k.High << 32) | k.Low) / 1e7 + (((ulong)u.High << 32) | u.Low) / 1e7;
    }
}

/// <summary>
/// 一路常开相机：独立 STA 线程负责“打开 → 连续取帧 → 缩放到预览尺寸”，
/// 并在这条线程上就地测出自己的 CPU 占用、帧率和内存分配速率。
///
/// 为什么必须是 STA 线程：这台机器上 DSHOW 走 COM，普通后台线程（MTA）里
/// 打开 VideoCapture 会直接失败，STA 线程则正常（都实测过）。
/// 为什么缩放放在采集线程：缩放属于“这一路相机的成本”，放这里才能被准确归因。
/// </summary>
internal sealed class CameraStream : IDisposable
{
    private readonly object _gate = new();
    private readonly ManualResetEventSlim _stop = new(false);

    private Thread _thread;
    private VideoCapture _cap;
    private readonly Mat _frame = new();
    private readonly Mat _small = new();

    private CameraState _state = CameraState.Idle;
    private string _error = "";

    private int _srcW;
    private int _srcH;
    private int _reqW = 1280;
    private int _reqH = 720;
    private string _api = "";
    private string _pixelFormat = "";
    private CameraSettings _settings = new();
    private int _tileW = 320;
    private int _tileH = 180;

    private byte[] _front;
    private byte[] _write;
    private int _pubW;
    private int _pubH;
    private long _publishId;

    private double _fps;
    private double _frameMs;
    private double _cpuPercent;
    private double _cpuAvgPercent;
    private double _peakCpuPercent;
    private double _allocMbPerSec;
    private double _openMs;
    private long _frames;

    private readonly Stopwatch _life = new();

    public CameraStream(int deviceIndex)
    {
        DeviceIndex = deviceIndex;
    }

    public int DeviceIndex { get; }

    /// <summary>当前采集参数（拷贝一份，改它不会影响正在跑的这一路）。</summary>
    public CameraSettings Settings => _settings.Clone();

    /// <summary>换采集参数；正在跑的话由调用方先 Stop 再 Start。</summary>
    public void SetSettings(CameraSettings settings)
    {
        if (settings == null) return;
        lock (_gate) _settings = settings.Clone();
    }

    /// <summary>采集线程是否还活着（关掉后可能还在读当前这一帧）。</summary>
    public bool IsBusy => _thread != null && _thread.IsAlive;

    public void SetTargetSize(int width, int height)
    {
        width = Math.Max(80, width);
        height = Math.Max(45, height);
        lock (_gate)
        {
            if (_tileW == width && _tileH == height) return;
            _tileW = width;
            _tileH = height;
            // 尺寸变了，旧缓冲直接作废，由采集线程按新尺寸重建。
            _front = null;
            _write = null;
        }
    }

    public CameraMetrics Snapshot()
    {
        lock (_gate)
        {
            return new CameraMetrics(_state, _error, _srcW, _srcH, _reqW, _reqH, _api, _pixelFormat,
                _fps, _frameMs, _cpuPercent, _cpuAvgPercent, _peakCpuPercent, _allocMbPerSec,
                _pubW * (double)_pubH * 3 / (1024 * 1024), _frames, _life.Elapsed.TotalSeconds, _openMs);
        }
    }

    /// <summary>把最新一帧拷进 target（BGR24）。没有新帧返回 false。</summary>
    public bool TryCopyFrame(ref byte[] target, out int width, out int height, ref long lastId)
    {
        lock (_gate)
        {
            width = _pubW;
            height = _pubH;
            if (_front == null || _publishId == lastId) return false;
            int need = _front.Length;
            if (target == null || target.Length != need) target = new byte[need];
            Buffer.BlockCopy(_front, 0, target, 0, need);
            lastId = _publishId;
            return true;
        }
    }

    public void Start()
    {
        if (IsBusy) return;
        lock (_gate)
        {
            _state = CameraState.Opening;
            _error = "";
            _frames = 0;
            _fps = 0;
            _frameMs = 0;
            _cpuPercent = 0;
            _cpuAvgPercent = 0;
            _peakCpuPercent = 0;
            _allocMbPerSec = 0;
            _openMs = 0;
        }

        _stop.Reset();
        _life.Restart();
        _thread = new Thread(Run)
        {
            IsBackground = true,
            Name = "cam-" + DeviceIndex,
        };
        _thread.SetApartmentState(ApartmentState.STA);
        _thread.Start();
    }

    public void Stop()
    {
        _stop.Set();
        var t = _thread;
        if (t == null) return;
        if (!t.Join(3000))
            Log.Warn($"相机 {DeviceIndex} 采集线程 3s 内没退出（可能还在打开），先放着，等它自己结束");
        _thread = null;
        lock (_gate)
        {
            if (_state != CameraState.Failed) _state = CameraState.Idle;
            _fps = 0;
            _cpuPercent = 0;
            _cpuAvgPercent = 0;
        }
    }

    public void Dispose()
    {
        Stop();
        lock (_gate)
        {
            _front = null;
            _write = null;
        }
        _stop.Dispose();
        _frame.Dispose();
        _small.Dispose();
    }

    private void Run()
    {
        try
        {
            if (!OpenCamera()) return;
            Loop();
        }
        catch (Exception ex)
        {
            Fail(ex.Message);
            Log.Error($"相机 {DeviceIndex} 采集线程异常", ex);
        }
        finally
        {
            try
            {
                _cap?.Release();
                _cap?.Dispose();
            }
            catch
            {
                // 关相机失败不该影响退出
            }
            _cap = null;
        }
    }

    private bool OpenCamera()
    {
        var sw = Stopwatch.StartNew();
        string lastError = "";
        foreach (var api in new[] { VideoCaptureAPIs.DSHOW, VideoCaptureAPIs.MSMF })
        {
            if (_stop.IsSet)
            {
                Fail("已取消");
                return false;
            }

            VideoCapture cap = null;
            try
            {
                cap = new VideoCapture(DeviceIndex, api);
                if (!cap.IsOpened())
                {
                    lastError = $"{api} 打不开 index {DeviceIndex}";
                    cap.Dispose();
                    Log.Warn($"相机 {DeviceIndex}: {lastError}");
                    continue;
                }

                CameraSettings cfg;
                lock (_gate) cfg = _settings;
                ApplyCaptureSettings(cap, cfg);

                using var probe = new Mat();
                if (!cap.Read(probe) || probe.Empty())
                {
                    lastError = $"{api} 读不到画面";
                    cap.Dispose();
                    Log.Warn($"相机 {DeviceIndex}: {lastError}");
                    continue;
                }

                _cap = cap;
                string fourcc = FourCCName(cap.Get(VideoCaptureProperties.FourCC));
                lock (_gate)
                {
                    _srcW = probe.Width;
                    _srcH = probe.Height;
                    _reqW = cfg.Width;
                    _reqH = cfg.Height;
                    _api = api.ToString();
                    _pixelFormat = fourcc;
                    _openMs = sw.Elapsed.TotalMilliseconds;
                    _state = CameraState.Running;
                    _error = "";
                }
                Log.Info($"相机 {DeviceIndex} 打开成功 api={api} 请求 {cfg.Width}x{cfg.Height}@{cfg.Fps} " +
                         $"实际 {probe.Width}x{probe.Height} {fourcc} 用时 {sw.ElapsedMilliseconds}ms");
                return true;
            }
            catch (Exception ex)
            {
                lastError = ex.Message;
                try
                {
                    cap?.Dispose();
                }
                catch
                {
                }
                Log.Warn($"相机 {DeviceIndex} 打开异常 api={api}: {ex.Message}");
            }
        }

        Fail(string.IsNullOrEmpty(lastError) ? "打开失败" : lastError);
        return false;
    }

    /// <summary>
    /// 按“先格式、再分辨率、再帧率、最后图像参数”的顺序下发；实测这个相机如果
    /// 把 FourCC/Fps 放在分辨率之后，驱动会把分辨率悄悄退回默认 1280x720。
    /// </summary>
    private static void ApplyCaptureSettings(VideoCapture cap, CameraSettings cfg)
    {
        if (!string.Equals(cfg.PixelFormat, "自动", StringComparison.OrdinalIgnoreCase))
        {
            string fcc = cfg.PixelFormat.ToUpperInvariant();
            if (fcc.Length == 4)
                cap.Set(VideoCaptureProperties.FourCC, VideoWriter.FourCC(fcc[0], fcc[1], fcc[2], fcc[3]));
        }

        cap.Set(VideoCaptureProperties.FrameWidth, cfg.Width);
        cap.Set(VideoCaptureProperties.FrameHeight, cfg.Height);
        cap.Set(VideoCaptureProperties.Fps, cfg.Fps);
        // 再压一次尺寸：个别驱动在改帧率时会把尺寸重置
        cap.Set(VideoCaptureProperties.FrameWidth, cfg.Width);
        cap.Set(VideoCaptureProperties.FrameHeight, cfg.Height);

        cap.Set(VideoCaptureProperties.Brightness, cfg.Brightness);
        cap.Set(VideoCaptureProperties.Contrast, cfg.Contrast);
        cap.Set(VideoCaptureProperties.Saturation, cfg.Saturation);
        cap.Set(VideoCaptureProperties.Gain, cfg.Gain);

        // DSHOW: 0.75 自动曝光 / 0.25 手动；别的后端回 False 就当没设置
        cap.Set(VideoCaptureProperties.AutoExposure, cfg.AutoExposure ? 0.75 : 0.25);
        if (!cfg.AutoExposure)
            cap.Set(VideoCaptureProperties.Exposure, cfg.Exposure);
        cap.Set(VideoCaptureProperties.AutoWB, cfg.AutoWhiteBalance ? 1 : 0);
        if (!cfg.AutoWhiteBalance)
            // OpenCvSharp 这个版本没有 WhiteBalance 成员，直接给 CAP_PROP_WHITE_BALANCE_BLUE_U(17)
            cap.Set((VideoCaptureProperties)17, cfg.WhiteBalance);
    }

    private static string FourCCName(double fourcc)
    {
        int v = (int)fourcc;
        if (v == 0) return "未知";
        var chars = new char[4];
        for (int i = 0; i < 4; i++)
        {
            int c = (v >> (8 * i)) & 0xFF;
            chars[i] = c >= 32 && c < 127 ? (char)c : '?';
        }
        return new string(chars).Trim();
    }

    private void Loop()
    {
        var sw = Stopwatch.StartNew();
        double lastCpu = ThreadCpu.Seconds();
        // 打开阶段的 CPU 也记在这条线程上，但 life 从 Loop 开始算，不减基线会把均值抬高
        double baseCpu = lastCpu;
        long lastAlloc = GC.GetAllocatedBytesForCurrentThread();
        long lastFrames = 0;
        double lastReport = 0;
        double frameMsSum = 0;
        int failStreak = 0;

        while (!_stop.IsSet)
        {
            long t0 = sw.ElapsedTicks;
            bool ok;
            try
            {
                ok = _cap != null && _cap.Read(_frame) && !_frame.Empty();
            }
            catch (Exception ex)
            {
                Fail("读帧异常: " + ex.Message);
                return;
            }

            if (!ok)
            {
                if (++failStreak > 20)
                {
                    Fail("连续 20 帧读取失败（相机被占用或已拔出）");
                    return;
                }
                Thread.Sleep(10);
                continue;
            }
            failStreak = 0;
            double readMs = (sw.ElapsedTicks - t0) * 1000.0 / Stopwatch.Frequency;

            int tileW, tileH;
            byte[] target;
            lock (_gate)
            {
                tileW = _tileW;
                tileH = _tileH;
                if (_write == null || _write.Length != tileW * tileH * 3)
                    _write = new byte[tileW * tileH * 3];
                target = _write;
            }

            try
            {
                Cv2.Resize(_frame, _small, new OpenCvSharp.Size(tileW, tileH), 0, 0, InterpolationFlags.Linear);
                if (_small.Total() * _small.ElemSize() == tileW * tileH * 3)
                    Marshal.Copy(_small.Data, target, 0, target.Length);
            }
            catch (Exception ex)
            {
                Fail("缩放异常: " + ex.Message);
                return;
            }

            lock (_gate)
            {
                var tmp = _front;
                _front = target;
                _write = tmp;
                _pubW = tileW;
                _pubH = tileH;
                _publishId++;
                _frames++;
            }

            frameMsSum += readMs;

            double now = sw.Elapsed.TotalSeconds;
            if (now - lastReport >= 0.5)
            {
                double dt = now - lastReport;
                double cpuNow = ThreadCpu.Seconds();
                long allocNow = GC.GetAllocatedBytesForCurrentThread();
                long framesNow;
                lock (_gate) framesNow = _frames;
                long dFrames = framesNow - lastFrames;

                lock (_gate)
                {
                    _fps = dFrames / dt;
                    _cpuPercent = (cpuNow - lastCpu) / dt * 100;
                    double life = Math.Max(0.5, sw.Elapsed.TotalSeconds);
                    _cpuAvgPercent = Math.Max(0, cpuNow - baseCpu) / life * 100;
                    if (_cpuPercent > _peakCpuPercent) _peakCpuPercent = _cpuPercent;
                    _allocMbPerSec = (allocNow - lastAlloc) / dt / 1024.0 / 1024.0;
                    _frameMs = dFrames > 0 ? frameMsSum / dFrames : 0;
                }

                lastReport = now;
                lastCpu = cpuNow;
                lastAlloc = allocNow;
                lastFrames = framesNow;
                frameMsSum = 0;
            }
        }
    }

    private void Fail(string message)
    {
        lock (_gate)
        {
            _state = CameraState.Failed;
            _error = message ?? "未知错误";
            _fps = 0;
            _cpuPercent = 0;
        }
    }
}
