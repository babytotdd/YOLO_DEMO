using System;

namespace YoloFool;

/// <summary>把“框中心在图像中的百分比”翻译成人能读懂的方位描述。</summary>
internal static class RelativePosition
{
    public static string Horizontal(double percent)
    {
        if (percent < 15) return "最左侧";
        if (percent < 35) return "左侧";
        if (percent < 45) return "中间偏左";
        if (percent <= 55) return "正中";
        if (percent <= 65) return "中间偏右";
        if (percent <= 85) return "右侧";
        return "最右侧";
    }

    public static string Vertical(double percent)
    {
        if (percent < 15) return "最上方";
        if (percent < 35) return "偏上";
        if (percent < 45) return "中上";
        if (percent <= 55) return "垂直居中";
        if (percent <= 65) return "中下";
        if (percent <= 85) return "偏下";
        return "最下方";
    }

    /// <summary>例：“位于画面左侧、偏上（横向 22.4% / 纵向 31.0%）”。</summary>
    public static string Describe(double cxPercent, double cyPercent)
    {
        var h = Horizontal(cxPercent);
        var v = Vertical(cyPercent);
        var where = h == "正中" && v == "垂直居中"
            ? "位于画面正中心"
            : $"位于画面{h}、{v}";
        return $"{where}（横向 {cxPercent:F1}% / 纵向 {cyPercent:F1}%）";
    }

    /// <summary>表格用短描述，例如“左侧 / 偏上”。</summary>
    public static string Short(double cxPercent, double cyPercent) =>
        $"{Horizontal(cxPercent)} / {Vertical(cyPercent)}";
}
