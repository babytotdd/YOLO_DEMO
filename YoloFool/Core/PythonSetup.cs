using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace YoloFool;

/// <summary>
/// 训练环境依赖的 Python：探测 3.10+、缺了就静默下载安装（仅当前用户，不需要管理员）。
/// 只在“装训练环境”时用到；识别、针床、数据集管理完全不依赖 Python。
/// </summary>
internal static class PythonSetup
{
    public const string InstallVersion = "3.12.10";
    public const int MinimumVersion = 310;

    private static readonly string[] Mirrors =
    {
        "https://www.python.org/ftp/python/" + InstallVersion + "/python-" + InstallVersion + "-amd64.exe",
        "https://mirrors.huaweicloud.com/python/" + InstallVersion + "/python-" + InstallVersion + "-amd64.exe",
        "https://registry.npmmirror.com/-/binary/python/" + InstallVersion + "/python-" + InstallVersion + "-amd64.exe",
    };

    /// <summary>安装包落盘位置（tools\downloads，换电脑时可以不拷）。</summary>
    public static string InstallerFile =>
        Path.Combine(AppPaths.ToolsDir, "downloads", "python-" + InstallVersion + "-amd64.exe");

    private static string LocalDir =>
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

    /// <summary>找到 3.10+ 解释器；没有返回 null。</summary>
    public static string Find() => Find(out _);

    public static string Find(out int version)
    {
        version = 0;
        foreach (var exe in Candidates())
        {
            if (exe.Contains('\\') && !File.Exists(exe)) continue;
            var v = Probe(exe);
            if (v >= MinimumVersion)
            {
                version = v;
                return exe;
            }
        }

        return null;
    }

    private static IEnumerable<string> Candidates()
    {
        foreach (var minor in new[] { 13, 12, 11, 10 })
            yield return Path.Combine(LocalDir, @"Programs\Python\Python3" + minor, "python.exe");
        foreach (var minor in new[] { 13, 12, 11, 10 })
            yield return @"C:\Python3" + minor + @"\python.exe";
        yield return "python.exe";
    }

    /// <summary>跑一次 python 取主次版本号（312 表示 3.12）；失败返回 0。</summary>
    public static int Probe(string exe)
    {
        try
        {
            var psi = new ProcessStartInfo(exe)
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
            };
            psi.ArgumentList.Add("-c");
            psi.ArgumentList.Add("import sys;print(sys.version_info[0]*100+sys.version_info[1])");

            using var p = new Process { StartInfo = psi };
            p.Start();
            var text = p.StandardOutput.ReadToEnd().Trim();
            if (!p.WaitForExit(8000))
            {
                try { p.Kill(true); } catch { }
                return 0;
            }

            return int.TryParse(text, out var v) ? v : 0;
        }
        catch
        {
            return 0;
        }
    }

    /// <summary>
    /// 确保有可用的 Python：有就直接返回；没有就下载官方安装包并静默安装到当前用户，
    /// 然后重新查找。返回 Ok=false 时 Message 里是给人看的原因。
    /// </summary>
    public static async Task<(bool Ok, string Path, string Message)> EnsureAsync(Action<string> stage)
    {
        var found = Find(out _);
        if (found != null) return (true, found, "Python 已就绪 · " + found);

        Directory.CreateDirectory(Path.GetDirectoryName(InstallerFile)!);
        var needDownload = !File.Exists(InstallerFile) || new FileInfo(InstallerFile).Length < 20_000_000;
        if (needDownload)
        {
            var error = "";
            var ok = false;
            using var http = new HttpClient { Timeout = TimeSpan.FromMinutes(15) };
            http.DefaultRequestHeaders.UserAgent.ParseAdd("YoloFool/1.0");

            foreach (var url in Mirrors)
            {
                try
                {
                    stage?.Invoke("正在下载 Python " + InstallVersion + "（" + new Uri(url).Host + "，约 27MB）…");
                    var tmp = InstallerFile + ".part";
                    using (var resp = await http.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
                    {
                        resp.EnsureSuccessStatusCode();
                        using var fs = File.Create(tmp);
                        await resp.Content.CopyToAsync(fs);
                    }

                    if (new FileInfo(tmp).Length < 20_000_000) throw new IOException("安装包不完整");
                    File.Move(tmp, InstallerFile, true);
                    ok = true;
                    break;
                }
                catch (Exception ex)
                {
                    error = ex.Message;
                }
            }

            if (!ok)
                return (false, null, "下载 Python 安装包失败：" + error + "\n可手动安装 Python 3.10+ 后再试。");
        }

        stage?.Invoke("正在静默安装 Python（仅当前用户，约 1 分钟）…");
        try
        {
            var code = await Task.Run(() => RunInstaller(InstallerFile));
            if (code != 0 && code != 3010)
                return (false, null, "Python 安装程序返回错误码 " + code + "，请手动安装 Python 3.10+。");
        }
        catch (Exception ex)
        {
            return (false, null, "运行 Python 安装程序失败：" + ex.Message);
        }

        for (var i = 0; i < 8; i++)
        {
            found = Find(out _);
            if (found != null) return (true, found, "Python 已安装 · " + found);
            await Task.Delay(1500);
        }

        return (false, null, "Python 安装完成但没有找到解释器，重启程序后再点一次“一键初始化”。");
    }

    private static int RunInstaller(string installer)
    {
        var psi = new ProcessStartInfo(installer)
        {
            UseShellExecute = false,
            CreateNoWindow = true,
        };
        foreach (var arg in new[]
                 {
                     "/quiet", "InstallAllUsers=0", "PrependPath=1", "Include_pip=1", "Include_test=0",
                 })
            psi.ArgumentList.Add(arg);

        using var p = Process.Start(psi);
        if (p == null) return -1;
        if (!p.WaitForExit(10 * 60 * 1000))
        {
            try { p.Kill(true); } catch { }
            return -2;
        }

        return p.ExitCode;
    }
}
