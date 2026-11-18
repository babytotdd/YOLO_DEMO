using System.Runtime.CompilerServices;

// 诊断/冒烟测试工具需要访问 internal 的 Core 类型；主程序本身仍是 internal。
[assembly: InternalsVisibleTo("UiSnap")]
[assembly: InternalsVisibleTo("YoloSmoke")]
