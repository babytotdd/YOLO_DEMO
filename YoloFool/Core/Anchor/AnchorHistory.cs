using System;
using System.IO;
using System.Text;

namespace YoloFool;

/// <summary>V2 判定留档：fixtures_v2/&lt;机种&gt;/history.csv（UTF-8 带 BOM，Excel 直接打开）。</summary>
internal static class AnchorHistory
{
    public static void Append(AnchorVerdict verdict, string model, string shotFile)
    {
        if (verdict == null) return;
        try
        {
            var dir = AnchorStore.Dir(model);
            Directory.CreateDirectory(dir);
            var path = Path.Combine(dir, AnchorStore.HistoryFile);
            var sb = new StringBuilder();
            if (!File.Exists(path))
            {
                sb.AppendLine("时间,机种,结论,原因码,原因,dx,dy,旋转deg,尺度,RMSpx,耗时ms,留档");
            }
            sb.AppendLine(string.Join(",",
                verdict.Time.ToString("yyyy-MM-dd HH:mm:ss"),
                Escape(model),
                verdict.Ok ? "OK" : "NG",
                verdict.ReasonCode,
                Escape(verdict.ReasonName),
                verdict.MeanDx.ToString("0.0"),
                verdict.MeanDy.ToString("0.0"),
                verdict.RotDeg.ToString("0.00"),
                verdict.Scale.ToString("0.0000"),
                verdict.RmsPx.ToString("0.0"),
                verdict.ElapsedMs.ToString(),
                Escape(shotFile ?? "")));
            File.AppendAllText(path, sb.ToString(), new UTF8Encoding(true));
        }
        catch (Exception ex)
        {
            Log.Warn("写 V2 判定历史失败: " + ex.Message);
        }
    }

    private static string Escape(string text)
    {
        if (string.IsNullOrEmpty(text)) return "";
        return text.Contains(',') || text.Contains('"')
            ? "\"" + text.Replace("\"", "\"\"") + "\""
            : text;
    }
}
