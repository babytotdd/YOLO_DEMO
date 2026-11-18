using System.Diagnostics;
using System.Drawing.Imaging;
using System.Text;
using YoloFool;

namespace UiSnap;

/// <summary>
/// 界面体检工具：把现有 WinForm 在多个窗口尺寸下离屏渲染成截图，
/// 并输出每个控件的实际边界，用来定位"按钮被遮挡/超出可视区"的问题。
/// 不修改主程序，仅用于诊断。
/// </summary>
internal static class Program
{
    private static readonly string OutDir =
        Path.Combine(AppContext.BaseDirectory, "snapshots");

    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();
        Directory.CreateDirectory(OutDir);
        // 每个用例都从“默认设置”开始，避免上一个用例把 AdvancedExpanded 持久化后串状态。
        var settingsFile = Path.Combine(AppContext.BaseDirectory, "settings.json");
        try { File.Delete(settingsFile); } catch { }
        File.WriteAllText(Path.Combine(OutDir, "environment.txt"),
            $"Screen={Screen.PrimaryScreen.Bounds} Working={Screen.PrimaryScreen.WorkingArea}\r\n" +
            $"DeviceDpi={GetDpiForSystem()}\r\n", Encoding.UTF8);

        if (Environment.GetEnvironmentVariable("YOLO_UI_CAMERA") == "1")
        {
            SmokeCamera();
            return;
        }

        var cases = new (int w, int h, string tag)[]
        {
            (1384, 821, "default"),
            (1100, 700, "minimum"),
            (1680, 1010, "maximized"),
            (1366, 768, "laptop"),
        };

        // 先做识别冒烟：此时进程里只有一个 MainForm，排除反复建窗导致句柄耗尽。
        SmokeRecognize();
        SmokeVideo();

        foreach (var c in cases)
            Snap(c.w, c.h, c.tag, false);

        Snap(1100, 700, "minimum_adv", true);
        Snap(1366, 768, "laptop_adv", true);
    }

    /// <summary>端到端：点“摄像头”，确认真实摄像头能出帧、能跟踪并截图。</summary>
    private static void SmokeCamera()
    {
        var f = new MainForm
        {
            StartPosition = FormStartPosition.Manual,
            Location = new Point(-5000, -5000),
            ShowInTaskbar = false,
            WindowState = FormWindowState.Normal,
        };
        f.ClientSize = new Size(1384, 821);

        var timer = new System.Windows.Forms.Timer { Interval = 300 };
        var sw = Stopwatch.StartNew();
        int phase = 0;
        timer.Tick += (_, _) =>
        {
            try
            {
                if (phase == 0)
                {
                    if (!f.Text.Contains("就绪") && !f.Text.Contains("失败")) return;
                    var tabs = Find(f, "tabMain") as TabControl;
                    if (tabs != null && tabs.TabPages.ContainsKey("tabVideo"))
                        tabs.SelectedTab = tabs.TabPages["tabVideo"];
                    var camera = Find(f, "btnCamera") as Button;
                    if (camera == null || !camera.Enabled) return;
                    camera.PerformClick();
                    phase = 1;
                    sw.Restart();
                    return;
                }

                var stats = Find(f, "lblVideoStats") as Label;
                var grid = Find(f, "gridVideo") as DataGridView;
                var info = Find(f, "lblVideoInfo") as Label;
                bool warm = sw.ElapsedMilliseconds > 8000;
                bool got = (warm && stats?.Text?.Contains("FPS") == true && grid != null && grid.Rows.Count > 0)
                           || (info?.Text?.StartsWith("失败") == true);
                bool timeout = sw.ElapsedMilliseconds > 15000;
                if (!got && !timeout) return;

                File.WriteAllText(Path.Combine(OutDir, "smoke_camera.txt"),
                    $"got={got} timeout={timeout}\nstats={stats?.Text}\ninfo={info?.Text}\nrows={grid?.Rows.Count}\n" +
                    string.Join("\n", grid?.Rows.Cast<DataGridViewRow>()
                        .Select(r => string.Join(" | ", r.Cells.Cast<DataGridViewCell>().Select(c => c.Value))) ?? Array.Empty<string>()),
                    Encoding.UTF8);

                using var bmp = new Bitmap(f.Width, f.Height);
                f.DrawToBitmap(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));
                bmp.Save(Path.Combine(OutDir, "smoke_camera.png"), ImageFormat.Png);
                timer.Stop();
                f.Close();
            }
            catch (Exception ex)
            {
                File.WriteAllText(Path.Combine(OutDir, "smoke_camera.txt"), "EX: " + ex, Encoding.UTF8);
                timer.Stop();
                f.Close();
            }
        };
        f.Shown += (_, _) => timer.Start();
        Application.Run(f);
        timer.Dispose();
    }

    /// <summary>端到端：在真实消息循环里点“开始识别”，确认结果表格与统计都更新并截图。</summary>
    private static void SmokeRecognize()
    {
        var f = new MainForm
        {
            StartPosition = FormStartPosition.Manual,
            Location = new Point(-5000, -5000),
            ShowInTaskbar = false,
            WindowState = FormWindowState.Normal,
        };
        f.ClientSize = new Size(1384, 821);

        var timer = new System.Windows.Forms.Timer { Interval = 250 };
        var sw = System.Diagnostics.Stopwatch.StartNew();
        int phase = 0; // 0=等就绪，1=已点识别，2=完成
        timer.Tick += (_, _) =>
        {
            try
            {
                if (phase == 0)
                {
                    if (!f.Text.Contains("就绪") && !f.Text.Contains("失败")) return;
                    var run = Find(f, "btnRun") as Button;
                    if (run == null || !run.Enabled) return;
                    run.PerformClick();
                    phase = 1;
                    sw.Restart();
                    return;
                }

                if (phase != 1) return;
                var grid = Find(f, "grid") as DataGridView;
                var stats = Find(f, "lblResultStats") as Label;
                bool done = grid != null && grid.Rows.Count > 0 &&
                            (stats?.Text?.Contains("个目标") ?? false);
                bool timeout = sw.ElapsedMilliseconds > 15000;

                if (!done && !timeout) return;

                File.WriteAllText(Path.Combine(OutDir, "smoke_result.txt"),
                    $"done={done} timeout={timeout}\n" +
                    $"rows={grid?.Rows.Count ?? -1}\n" +
                    $"stats={stats?.Text}\n" +
                    $"status={f.Text}\n" +
                    string.Join("\n", grid?.Rows.Cast<DataGridViewRow>()
                        .Select(r => string.Join(" | ", r.Cells.Cast<DataGridViewCell>().Select(c => c.Value))) ?? Array.Empty<string>()),
                    Encoding.UTF8);

                using var bmp = new Bitmap(f.Width, f.Height);
                f.DrawToBitmap(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));
                bmp.Save(Path.Combine(OutDir, "smoke_recognize.png"), ImageFormat.Png);
                phase = 2;
                timer.Stop();
                f.Close();
            }
            catch (Exception ex)
            {
                File.WriteAllText(Path.Combine(OutDir, "smoke_result.txt"), "EX: " + ex, Encoding.UTF8);
                timer.Stop();
                f.Close();
            }
        };
        f.Shown += (_, _) => timer.Start();
        Application.Run(f);
        timer.Dispose();
    }

    /// <summary>端到端：视频页启动分割模型，跑出 FPS/跟踪/面积并截图。</summary>
    private static void SmokeVideo()
    {
        var f = new MainForm
        {
            StartPosition = FormStartPosition.Manual,
            Location = new Point(-5000, -5000),
            ShowInTaskbar = false,
            WindowState = FormWindowState.Normal,
        };
        f.ClientSize = new Size(1384, 821);

        var timer = new System.Windows.Forms.Timer { Interval = 400 };
        var sw = Stopwatch.StartNew();
        int phase = 0;
        timer.Tick += (_, _) =>
        {
            try
            {
                if (phase == 0)
                {
                    if (!f.Text.Contains("就绪") && !f.Text.Contains("失败")) return;
                    var tabs = Find(f, "tabMain") as TabControl;
                    if (tabs != null && tabs.TabPages.ContainsKey("tabVideo"))
                        tabs.SelectedTab = tabs.TabPages["tabVideo"];
                    var start = Find(f, "btnStartVideo") as Button;
                    if (start == null || !start.Enabled) return;
                    start.PerformClick();
                    phase = 1;
                    sw.Restart();
                    return;
                }

                var stats = Find(f, "lblVideoStats") as Label;
                var area = Find(f, "lblVideoArea") as Label;
                var grid = Find(f, "gridVideo") as DataGridView;
                var atob = Find(f, "lblVideoAtoB") as Label;
                var btoa = Find(f, "lblVideoBtoA") as Label;
                bool counted = (atob?.Text ?? "0") != "0" || (btoa?.Text ?? "0") != "0";
                bool got = stats?.Text?.Contains("FPS") == true && grid != null && grid.Rows.Count > 0 && counted;
                bool timeout = sw.ElapsedMilliseconds > 40000;
                if (!got && !timeout) return;

                File.WriteAllText(Path.Combine(OutDir, "smoke_video.txt"),
                    $"got={got} timeout={timeout}\n" +
                    $"stats={stats?.Text}\narea={area?.Text}\nrows={grid?.Rows.Count}\n" +
                    $"AtoB={atob?.Text} BtoA={btoa?.Text}\n" +
                    string.Join("\n", grid?.Rows.Cast<DataGridViewRow>()
                        .Select(r => string.Join(" | ", r.Cells.Cast<DataGridViewCell>().Select(c => c.Value))) ?? Array.Empty<string>()),
                    Encoding.UTF8);

                using var bmp = new Bitmap(f.Width, f.Height);
                f.DrawToBitmap(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));
                bmp.Save(Path.Combine(OutDir, "smoke_video.png"), ImageFormat.Png);
                timer.Stop();
                f.Close();
            }
            catch (Exception ex)
            {
                File.WriteAllText(Path.Combine(OutDir, "smoke_video.txt"), "EX: " + ex, Encoding.UTF8);
                timer.Stop();
                f.Close();
            }
        };
        f.Shown += (_, _) => timer.Start();
        Application.Run(f);
        timer.Dispose();
    }

    private static void Snap(int w, int h, string tag, bool expandAdv)
    {
        using var f = new MainForm
        {
            StartPosition = FormStartPosition.Manual,
            Location = new Point(-5000, -5000),
            ShowInTaskbar = false,
            WindowState = FormWindowState.Normal,
        };
        f.ClientSize = new Size(w, h);
        f.Show();
        WaitForIdle(f, 15000);

        if (expandAdv)
        {
            var btn = Find(f, "btnToggleAdv") as Button;
            btn?.PerformClick();
            // 设置可能被上一次运行持久化成“已展开”，此时第一次点击反而是收起；再点一次确保展开。
            if (btn != null && btn.Text.Contains("▸")) btn.PerformClick();
            WaitForIdle(f, 1000);
        }

        foreach (var page in new[] { "tabRecognize", "tabLearn", "tabVideo" })
        {
            var tab = Find(f, "tabMain") as TabControl;
            if (tab == null) continue;
            tab.SelectedTab = tab.TabPages[page];
            WaitForIdle(f, 500);

            string file = Path.Combine(OutDir, $"current_{tag}_{page}.png");
            using (var bmp = new Bitmap(f.Width, f.Height))
            {
                f.DrawToBitmap(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));
                bmp.Save(file, ImageFormat.Png);
            }

            var report = new StringBuilder();
            report.AppendLine($"FORM client={f.ClientSize} dpi={GetDpiForSystem()} screen={Screen.PrimaryScreen.WorkingArea}");
            Walk(f, f.ClientSize, report, 0);
            File.WriteAllText(Path.Combine(OutDir, $"bounds_{tag}_{page}.txt"), report.ToString(), Encoding.UTF8);
        }
        f.Close();
        Application.DoEvents();
    }

    private static void WaitForIdle(Form f, int timeoutMs)
    {
        var sw = System.Diagnostics.Stopwatch.StartNew();
        while (sw.ElapsedMilliseconds < timeoutMs)
        {
            Application.DoEvents();
            Thread.Sleep(80);
            if (f.IsHandleCreated && (f.Text.Contains("就绪") || f.Text.Contains("失败"))) break;
        }
        Application.DoEvents();
    }

    private static Control Find(Control root, string name)
    {
        if (root.Name == name) return root;
        foreach (Control c in root.Controls)
        {
            var hit = Find(c, name);
            if (hit != null) return hit;
        }
        return null;
    }

    private static void Walk(Control parent, Size formClient, StringBuilder sb, int depth)
    {
        foreach (Control c in parent.Controls)
        {
            if (!c.Visible) { sb.AppendLine($"{new string(' ', depth * 2)}[hidden] {c.GetType().Name} {c.Name}"); continue; }
            var b = c.Bounds;
            bool overflowX = b.Right > parent.ClientSize.Width + 1;
            bool overflowY = b.Bottom > parent.ClientSize.Height + 1;
            string flags = (overflowX ? " OVERFLOW-X" : "") + (overflowY ? " OVERFLOW-Y" : "");
            sb.AppendLine($"{new string(' ', depth * 2)}{c.GetType().Name,-14} {c.Name,-18} bounds={b} parent={parent.Name}({parent.ClientSize.Width}x{parent.ClientSize.Height}){flags} text=\"{Trim(c.Text)}\"");
            if (c.Controls.Count > 0) Walk(c, formClient, sb, depth + 1);
        }
    }

    private static string Trim(string s) =>
        string.IsNullOrEmpty(s) ? "" : (s.Length > 24 ? s[..24] + "…" : s.Replace("\r", " ").Replace("\n", " "));

    [System.Runtime.InteropServices.DllImport("user32.dll")]
    private static extern uint GetDpiForSystem();
}
