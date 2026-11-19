using System;
using System.Windows.Forms;

namespace YoloFool;

/// <summary>
/// 不接受滚轮改值的数值框：鼠标停在参数上滚动页面时不再误改数值。
/// 需要改值时点上下箭头或直接输入。
/// </summary>
internal sealed class NoWheelNumeric : NumericUpDown
{
    protected override void OnMouseWheel(MouseEventArgs e)
    {
        // 故意吞掉：不把滚轮转成数值增减，避免调参数时误操作。
        // 也不传给父容器（父容器滚动交给页面本身）。
    }
}
