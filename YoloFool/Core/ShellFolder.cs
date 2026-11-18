using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;

namespace YoloFool;

/// <summary>
/// 用系统资源管理器打开文件夹。
/// 本机 UAC 关闭，用户双击即以管理员身份运行（High 完整性）。但如果 exe 位于 DSH
/// 沙箱工作区（H:\Desktop\YOLO_DEMO 及其子目录带可继承的 Low 完整性标签），进程会被
/// 降为 Low 完整性，此时 ShellExecute / explorer.exe 全部被 UIPI 拒绝，弹出
/// “Windows 无法访问指定设备、路径或文件”，所以这里先检测完整性给出明确提示。
/// 完整性正常时依次尝试：ShellExecute 打开目录 → Shell.Application COM → explorer.exe。
/// </summary>
internal static class ShellFolder
{
    private const int TokenIntegrityLevel = 25;
    private const int TokenQuery = 0x0008;

    /// <summary>打开文件夹；失败时通过 <paramref name="error"/> 返回失败原因。</summary>
    public static bool TryOpen(string dir, out string error)
    {
        if (IsLowIntegrity())
        {
            error = "程序当前以低完整性（Low IL）运行，Windows 不允许它调起资源管理器。"
                  + "请把程序复制到沙箱工作区之外（例如桌面上的 YoloFoolApp 文件夹）再运行。";
            return false;
        }

        if (TryOpenByShellExecute(dir, out error)) return true;
        var shellExecuteError = error;

        if (TryOpenByCom(dir, out error)) return true;
        var comError = error;

        if (TryOpenByExplorer(dir, out error)) return true;
        error = "ShellExecute 失败：" + shellExecuteError
              + "；Shell.Application 失败：" + comError
              + "；explorer.exe 失败：" + error;
        return false;
    }

    /// <summary>最直接的方式：等同在资源管理器里双击这个文件夹。</summary>
    private static bool TryOpenByShellExecute(string dir, out string error)
    {
        error = null;
        try
        {
            Process.Start(new ProcessStartInfo(dir) { UseShellExecute = true });
            return true;
        }
        catch (Win32Exception ex)
        {
            error = $"ShellExecute 拒绝访问（Win32={ex.NativeErrorCode}）：{ex.Message}";
            return false;
        }
        catch (Exception ex)
        {
            error = ex.GetBaseException().Message;
            return false;
        }
    }

    /// <summary>走 Shell.Application COM，由已运行的资源管理器处理，不新起进程。</summary>
    private static bool TryOpenByCom(string dir, out string error)
    {
        error = null;
        object shell = null;
        try
        {
            var type = Type.GetTypeFromProgID("Shell.Application");
            if (type == null)
            {
                error = "Shell.Application 未注册";
                return false;
            }

            shell = Activator.CreateInstance(type);
            type.InvokeMember("Explore", BindingFlags.InvokeMethod, null, shell, new object[] { dir });
            return true;
        }
        catch (Exception ex)
        {
            error = ex.GetBaseException().Message;
            return false;
        }
        finally
        {
            if (shell != null && Marshal.IsComObject(shell)) Marshal.ReleaseComObject(shell);
        }
    }

    /// <summary>退路：直接起 explorer.exe（异常环境下可能 0xc0000142，故放最后）。</summary>
    private static bool TryOpenByExplorer(string dir, out string error)
    {
        error = null;
        try
        {
            using var p = Process.Start(new ProcessStartInfo("explorer.exe", $"\"{dir}\"")
            {
                UseShellExecute = true,
            });
            if (p == null)
            {
                error = "explorer.exe 未能启动";
                return false;
            }

            // 启动成功时资源管理器通常常驻；秒退说明它初始化失败（例如 0xc0000142）。
            if (p.WaitForExit(1200) && p.ExitCode != 0)
            {
                error = $"explorer.exe 退出码=0x{p.ExitCode:X8}";
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            error = ex.GetBaseException().Message;
            return false;
        }
    }

    /// <summary>当前进程是否运行在 Low 完整性级别（S-1-16-4096）。</summary>
    private static bool IsLowIntegrity()
    {
        var token = IntPtr.Zero;
        var info = IntPtr.Zero;
        try
        {
            if (!OpenProcessToken(GetCurrentProcess(), TokenQuery, out token)) return false;

            GetTokenInformation(token, TokenIntegrityLevel, IntPtr.Zero, 0, out var needed);
            if (needed <= 0) return false;

            info = Marshal.AllocHGlobal(needed);
            if (!GetTokenInformation(token, TokenIntegrityLevel, info, needed, out _)) return false;

            // TOKEN_MANDATORY_LABEL { SID_AND_ATTRIBUTES { PSID Sid; DWORD Attributes; } }
            var sid = Marshal.ReadIntPtr(info);
            if (sid == IntPtr.Zero) return false;

            var subAuthorityCount = Marshal.ReadByte(sid, 1);
            if (subAuthorityCount == 0) return false;

            // SID: revision(1) + count(1) + authority(6) + subAuthority[count]（DWORD）
            var rid = Marshal.ReadInt32(sid, 8 + (subAuthorityCount - 1) * 4);
            return rid < 0x2000; // SECURITY_MANDATORY_MEDIUM_RID
        }
        catch
        {
            return false;
        }
        finally
        {
            if (info != IntPtr.Zero) Marshal.FreeHGlobal(info);
            if (token != IntPtr.Zero) CloseHandle(token);
        }
    }

    [DllImport("kernel32.dll")]
    private static extern IntPtr GetCurrentProcess();

    [DllImport("advapi32.dll", SetLastError = true)]
    private static extern bool OpenProcessToken(IntPtr process, uint access, out IntPtr token);

    [DllImport("advapi32.dll", SetLastError = true)]
    private static extern bool GetTokenInformation(IntPtr token, int infoClass, IntPtr info, int length, out int needed);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool CloseHandle(IntPtr handle);
}
