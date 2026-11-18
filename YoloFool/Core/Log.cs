using System;
using System.IO;
using System.Text;

namespace YoloFool;

/// <summary>极简文件日志：出问题时用户可以把 logs 目录发回来定位。</summary>
internal static class Log
{
    private static readonly object Gate = new();

    public static void Info(string message) => Write("INFO", message);

    public static void Warn(string message) => Write("WARN", message);

    public static void Error(string message, Exception ex = null) =>
        Write("ERROR", ex == null ? message : $"{message} | {ex}");

    private static void Write(string level, string message)
    {
        try
        {
            Directory.CreateDirectory(AppPaths.LogDir);
            var line = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} [{level}] {message}{Environment.NewLine}";
            lock (Gate) File.AppendAllText(Path.Combine(AppPaths.LogDir, $"app-{DateTime.Now:yyyyMMdd}.log"), line, Encoding.UTF8);
        }
        catch
        {
            // 日志失败不能影响主流程
        }
    }
}
