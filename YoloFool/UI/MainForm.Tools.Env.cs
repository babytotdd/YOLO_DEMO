using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YoloFool;

/// <summary>
/// “首次部署 · 换电脑”卡：程序复制到另一台电脑、第一次启动时用。
/// 只做部署检查（目录 / 运行库 / 模型 / 相机）和一键补齐，与标签、数据内容无关。
/// </summary>
public sealed partial class MainForm
{
    private bool _toolsEnvBusy;


    private async void CheckToolsEnvironment(bool withCamera)
    {
        if (_toolsEnvBusy || _lvToolsEnv == null) return;
        SetToolsEnvBusy(true);
        _lblToolsEnvInfo.ForeColor = Theme.Muted;
        _lblToolsEnvInfo.Text = withCamera ? "正在检查（含相机，约几秒）…" : "正在检查…";

        try
        {
            var items = await RunEnvCheckAsync(withCamera);
            FillToolsEnv(items);
            var bad = 0;
            foreach (var i in items) if (i.State != EnvCheckState.Ok) bad++;
            _lblToolsEnvInfo.Text = bad == 0
                ? $"检查完成：{items.Count} 项全部正常，这台机器可以直接用（{DateTime.Now:HH:mm:ss}）"
                : $"检查完成：{items.Count} 项里有 {bad} 项需要注意，点“一键初始化”可自动处理（{DateTime.Now:HH:mm:ss}）";
            _lblToolsEnvInfo.ForeColor = bad == 0 ? Theme.Ok : Theme.Warn;
        }
        catch (Exception ex)
        {
            _lblToolsEnvInfo.Text = "检查失败：" + ex.Message;
            _lblToolsEnvInfo.ForeColor = Theme.Danger;
        }
        finally
        {
            SetToolsEnvBusy(false);
        }
    }

    private async void InitToolsEnvironment()
    {
        if (_toolsEnvBusy) return;

        var withTraining = _chkToolsEnvTraining?.Checked == true;
        var plan = "将要执行：\n\n  1. 补齐缺的目录（my_models、mydataset、fixtures、fixtures_v2 等）\n"
                   + (withTraining ? "  2. 缺 Python 先自动下载安装，再装 ultralytics（已就绪就跳过；不训练）\n" : "  2. 不碰训练环境（勾选框没勾）\n")
                   + "\n只补缺、不覆盖已有文件，也不动任何模型和标签。";
        var yes = MessageBox.Show(this, plan + "\n\n开始？", "首次部署 · 初始化",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (yes != DialogResult.Yes) return;

        SetToolsEnvBusy(true);
        try
        {
            var fix = EnvironmentCheck.EnsureStructure();
            var summary = fix.Created.Count == 0
                ? "目录都齐了，没有新建。"
                : "新建了 " + fix.Created.Count + " 个目录：\n" + string.Join("\n", fix.Created);
            if (fix.Failed.Count > 0)
                summary += "\n\n失败 " + fix.Failed.Count + " 个：\n" + string.Join("\n", fix.Failed);

            _lblToolsEnvInfo.ForeColor = fix.Failed.Count > 0 ? Theme.Warn : Theme.Ok;
            _lblToolsEnvInfo.Text = summary.Replace("\n", "  ");

            if (withTraining)
            {
                // 复用训练页的初始化逻辑（内部会提示结果）；期间把阶段信息显示在这张卡上，
                // 否则下载安装 Python / ultralytics 的几分钟里界面没有任何反馈。
                void Stage(string text) => BeginInvokeSafe(() =>
                {
                    _lblToolsEnvInfo.ForeColor = Theme.Ink;
                    _lblToolsEnvInfo.Text = text;
                });

                _training.Stage += Stage;
                try
                {
                    await InitEnvironmentAsync();
                }
                finally
                {
                    _training.Stage -= Stage;
                }
            }

            MessageBox.Show(this, summary, "首次部署 · 初始化",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, "初始化失败：\n" + ex.Message, "首次部署 · 初始化",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        finally
        {
            SetToolsEnvBusy(false);
        }

        // 初始化完再检查一次（不含相机）
        CheckToolsEnvironment(false);
    }

    /// <summary>后台跑一次部署体检；“模型标签”卡再训练前也调它，避免两处各写一份检查逻辑。</summary>
    private async Task<List<EnvCheckItem>> RunEnvCheckAsync(bool withCamera)
    {
        return await Task.Run(() => EnvironmentCheck.Run(withCamera));
    }

    private void FillToolsEnv(List<EnvCheckItem> items)
    {
        if (_lvToolsEnv == null) return;
        _lvToolsEnv.BeginUpdate();
        try
        {
            _lvToolsEnv.Items.Clear();
            foreach (var i in items)
            {
                var item = new ListViewItem(i.Name);
                item.SubItems.Add(i.Mark);
                item.SubItems.Add(i.Detail);
                item.ForeColor = i.State switch
                {
                    EnvCheckState.Ok => Theme.Ok,
                    EnvCheckState.Warn => Theme.Warn,
                    _ => Theme.Danger,
                };
                _lvToolsEnv.Items.Add(item);
            }

            _lvToolsEnv.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        }
        finally
        {
            _lvToolsEnv.EndUpdate();
        }
    }

    private void SetToolsEnvBusy(bool busy)
    {
        _toolsEnvBusy = busy;
        if (_btnToolsEnvCheck != null) _btnToolsEnvCheck.Enabled = !busy;
        if (_btnToolsEnvInit != null) _btnToolsEnvInit.Enabled = !busy;
        if (_btnToolsEnvCamera != null) _btnToolsEnvCamera.Enabled = !busy;
        if (_chkToolsEnvTraining != null) _chkToolsEnvTraining.Enabled = !busy;
        Cursor = busy ? Cursors.WaitCursor : Cursors.Default;
    }
}
