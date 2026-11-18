namespace YoloFool;

partial class MainForm
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null)) components.Dispose();
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private void InitializeComponent()
    {
        tabMain = new TabControl();
        tabRecognize = new TabPage();
        tabLearn = new TabPage();
        splitRec = new SplitContainer();
        splitLearn = new SplitContainer();
        pnlLearnTop = new Panel();
        pnlTop = new Panel();
        lblImagePath = new Label();
        tbRecImage = new TextBox();
        btnSelectImage = new Button();
        btnRun = new Button();
        btnToggleAdv = new Button();
        pnlAdv = new Panel();
        lblAdvModel = new Label();
        tbAdvModel = new TextBox();
        btnAdvModel = new Button();
        lblAdvKind = new Label();
        cboAdvKind = new ComboBox();
        lblAdvEngine = new Label();
        cboAdvEngine = new ComboBox();
        lblAdvDevice = new Label();
        cboAdvDevice = new ComboBox();
        lblAdvProvider = new Label();
        cboAdvProvider = new ComboBox();
        lblAdvConf = new Label();
        numConf = new NumericUpDown();
        lblAdvNms = new Label();
        numNms = new NumericUpDown();
        btnSaveAdv = new Button();
        lblAdvModelInfo = new Label();
        grid = new DataGridView();
        picRec = new PictureBox();
        lblLearnImage = new Label();
        tbLearnImage = new TextBox();
        btnOpenLearnImage = new Button();
        picLearn = new PictureBox();
        lblBoxes = new Label();
        lvBoxes = new ListView();
        btnDelBox = new Button();
        btnRenameBox = new Button();
        btnSaveAnno = new Button();
        lblAnnoInfo = new Label();
        btnInitEnv = new Button();
        btnTrain = new Button();
        btnCancelTrain = new Button();
        txtTrainLog = new TextBox();
        tabMain.SuspendLayout();
        tabRecognize.SuspendLayout();
        tabLearn.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)splitRec).BeginInit();
        splitRec.Panel1.SuspendLayout();
        splitRec.Panel2.SuspendLayout();
        splitRec.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)splitLearn).BeginInit();
        splitLearn.Panel1.SuspendLayout();
        splitLearn.Panel2.SuspendLayout();
        splitLearn.SuspendLayout();
        pnlLearnTop.SuspendLayout();
        pnlTop.SuspendLayout();
        pnlAdv.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)numConf).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numNms).BeginInit();
        SuspendLayout();
        //
        // tabMain
        //
        tabMain.Dock = DockStyle.Fill;
        tabMain.Controls.Add(tabRecognize);
        tabMain.Controls.Add(tabLearn);
        tabMain.Location = new Point(0, 0);
        tabMain.Name = "tabMain";
        tabMain.TabIndex = 0;
        //
        // tabRecognize
        //
        tabRecognize.Text = "识别";
        tabRecognize.Name = "tabRecognize";
        tabRecognize.Padding = new Padding(0, 4, 0, 0);
        tabRecognize.Controls.Add(splitRec);
        tabRecognize.Controls.Add(pnlAdv);
        tabRecognize.Controls.Add(pnlTop);
        //
        // pnlTop
        //
        pnlTop.Name = "pnlTop";
        pnlTop.Location = new Point(0, 0);
        pnlTop.Size = new Size(1368, 52);
        pnlTop.Dock = DockStyle.Top;
        pnlTop.Controls.Add(lblImagePath);
        pnlTop.Controls.Add(tbRecImage);
        pnlTop.Controls.Add(btnSelectImage);
        pnlTop.Controls.Add(btnRun);
        pnlTop.Controls.Add(btnToggleAdv);
        //
        // lblImagePath
        //
        lblImagePath.Text = "图片：";
        lblImagePath.Name = "lblImagePath";
        lblImagePath.AutoSize = true;
        lblImagePath.Location = new Point(10, 17);
        //
        // tbRecImage
        //
        tbRecImage.Name = "tbRecImage";
        tbRecImage.Location = new Point(60, 14);
        tbRecImage.Width = 560;
        tbRecImage.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        //
        // btnSelectImage
        //
        btnSelectImage.Text = "选择";
        btnSelectImage.Name = "btnSelectImage";
        btnSelectImage.Location = new Point(628, 12);
        btnSelectImage.Size = new Size(70, 30);
        btnSelectImage.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnSelectImage.Click += btnSelectImage_Click;
        //
        // btnRun
        //
        btnRun.Text = "开 始 识 别";
        btnRun.Name = "btnRun";
        btnRun.Location = new Point(712, 8);
        btnRun.Size = new Size(160, 38);
        btnRun.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnRun.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Bold);
        btnRun.BackColor = Color.FromArgb(36, 112, 210);
        btnRun.ForeColor = Color.White;
        btnRun.FlatStyle = FlatStyle.Flat;
        btnRun.Click += btnRun_Click;
        //
        // btnToggleAdv
        //
        btnToggleAdv.Text = "▸ 详细设置";
        btnToggleAdv.Name = "btnToggleAdv";
        btnToggleAdv.Location = new Point(884, 12);
        btnToggleAdv.Size = new Size(120, 30);
        btnToggleAdv.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnToggleAdv.Click += btnToggleAdv_Click;
        //
        // pnlAdv
        //
        pnlAdv.Name = "pnlAdv";
        pnlAdv.Location = new Point(0, 0);
        pnlAdv.Size = new Size(1368, 140);
        pnlAdv.Dock = DockStyle.Top;
        pnlAdv.Height = 0;
        pnlAdv.Visible = false;
        pnlAdv.BackColor = Color.FromArgb(240, 244, 250);
        pnlAdv.Controls.Add(lblAdvModel);
        pnlAdv.Controls.Add(tbAdvModel);
        pnlAdv.Controls.Add(btnAdvModel);
        pnlAdv.Controls.Add(lblAdvKind);
        pnlAdv.Controls.Add(cboAdvKind);
        pnlAdv.Controls.Add(lblAdvEngine);
        pnlAdv.Controls.Add(cboAdvEngine);
        pnlAdv.Controls.Add(lblAdvDevice);
        pnlAdv.Controls.Add(cboAdvDevice);
        pnlAdv.Controls.Add(lblAdvProvider);
        pnlAdv.Controls.Add(cboAdvProvider);
        pnlAdv.Controls.Add(lblAdvConf);
        pnlAdv.Controls.Add(numConf);
        pnlAdv.Controls.Add(lblAdvNms);
        pnlAdv.Controls.Add(numNms);
        pnlAdv.Controls.Add(btnSaveAdv);
        pnlAdv.Controls.Add(lblAdvModelInfo);
        //
        // lblAdvModel
        //
        lblAdvModel.Text = "模型文件：";
        lblAdvModel.Name = "lblAdvModel";
        lblAdvModel.AutoSize = true;
        lblAdvModel.Location = new Point(12, 14);
        //
        // tbAdvModel
        //
        tbAdvModel.Name = "tbAdvModel";
        tbAdvModel.Location = new Point(90, 11);
        tbAdvModel.Width = 560;
        tbAdvModel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        //
        // btnAdvModel
        //
        btnAdvModel.Text = "选择";
        btnAdvModel.Name = "btnAdvModel";
        btnAdvModel.Location = new Point(658, 9);
        btnAdvModel.Size = new Size(60, 28);
        btnAdvModel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnAdvModel.Click += btnAdvModel_Click;
        //
        // lblAdvKind
        //
        lblAdvKind.Text = "模型类型：";
        lblAdvKind.Name = "lblAdvKind";
        lblAdvKind.AutoSize = true;
        lblAdvKind.Location = new Point(12, 48);
        //
        // cboAdvKind
        //
        cboAdvKind.Name = "cboAdvKind";
        cboAdvKind.DropDownStyle = ComboBoxStyle.DropDownList;
        cboAdvKind.Location = new Point(90, 44);
        cboAdvKind.Width = 140;
        //
        // lblAdvEngine
        //
        lblAdvEngine.Text = "推理引擎：";
        lblAdvEngine.Name = "lblAdvEngine";
        lblAdvEngine.AutoSize = true;
        lblAdvEngine.Location = new Point(250, 48);
        //
        // cboAdvEngine
        //
        cboAdvEngine.Name = "cboAdvEngine";
        cboAdvEngine.DropDownStyle = ComboBoxStyle.DropDownList;
        cboAdvEngine.Location = new Point(330, 44);
        cboAdvEngine.Width = 140;
        //
        // lblAdvDevice
        //
        lblAdvDevice.Text = "设备：";
        lblAdvDevice.Name = "lblAdvDevice";
        lblAdvDevice.AutoSize = true;
        lblAdvDevice.Location = new Point(490, 48);
        //
        // cboAdvDevice
        //
        cboAdvDevice.Name = "cboAdvDevice";
        cboAdvDevice.DropDownStyle = ComboBoxStyle.DropDownList;
        cboAdvDevice.Location = new Point(540, 44);
        cboAdvDevice.Width = 120;
        //
        // lblAdvProvider
        //
        lblAdvProvider.Text = "加速后端：";
        lblAdvProvider.Name = "lblAdvProvider";
        lblAdvProvider.AutoSize = true;
        lblAdvProvider.Location = new Point(12, 82);
        //
        // cboAdvProvider
        //
        cboAdvProvider.Name = "cboAdvProvider";
        cboAdvProvider.DropDownStyle = ComboBoxStyle.DropDownList;
        cboAdvProvider.Location = new Point(90, 78);
        cboAdvProvider.Width = 140;
        //
        // lblAdvConf
        //
        lblAdvConf.Text = "置信度：";
        lblAdvConf.Name = "lblAdvConf";
        lblAdvConf.AutoSize = true;
        lblAdvConf.Location = new Point(250, 82);
        //
        // numConf
        //
        numConf.Name = "numConf";
        numConf.DecimalPlaces = 2;
        numConf.Minimum = 0.05M;
        numConf.Maximum = 0.95M;
        numConf.Increment = 0.05M;
        numConf.Value = 0.5M;
        numConf.Location = new Point(330, 78);
        numConf.Width = 70;
        //
        // lblAdvNms
        //
        lblAdvNms.Text = "NMS：";
        lblAdvNms.Name = "lblAdvNms";
        lblAdvNms.AutoSize = true;
        lblAdvNms.Location = new Point(420, 82);
        //
        // numNms
        //
        numNms.Name = "numNms";
        numNms.DecimalPlaces = 2;
        numNms.Minimum = 0.1M;
        numNms.Maximum = 0.95M;
        numNms.Increment = 0.05M;
        numNms.Value = 0.5M;
        numNms.Location = new Point(480, 78);
        numNms.Width = 70;
        //
        // btnSaveAdv
        //
        btnSaveAdv.Text = "保存设置";
        btnSaveAdv.Name = "btnSaveAdv";
        btnSaveAdv.Location = new Point(560, 76);
        btnSaveAdv.Size = new Size(100, 32);
        btnSaveAdv.Click += btnSaveAdv_Click;
        //
        // lblAdvModelInfo
        //
        lblAdvModelInfo.Name = "lblAdvModelInfo";
        lblAdvModelInfo.AutoSize = true;
        lblAdvModelInfo.ForeColor = Color.Gray;
        lblAdvModelInfo.Location = new Point(12, 116);
        lblAdvModelInfo.Text = "默认值已按本机验证配置；修改后点\"保存设置\"写入 settings.json。";
        //
        // splitRec
        //
        splitRec.Name = "splitRec";
        splitRec.Location = new Point(0, 0);
        splitRec.Size = new Size(1360, 700);
        splitRec.Dock = DockStyle.Fill;
        splitRec.Orientation = Orientation.Vertical;
        splitRec.Panel1MinSize = 200;
        splitRec.Panel2MinSize = 200;
        splitRec.SplitterWidth = 6;
        splitRec.SplitterDistance = 580;
        splitRec.Panel1.Controls.Add(grid);
        splitRec.Panel2.Controls.Add(picRec);
        //
        // grid
        //
        grid.Name = "grid";
        grid.Dock = DockStyle.Fill;
        grid.AllowUserToAddRows = false;
        grid.ReadOnly = true;
        grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        grid.RowHeadersVisible = false;
        grid.ColumnHeadersVisible = true;
        grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
        grid.ColumnHeadersHeight = 40;
        grid.RowTemplate.Height = 32;
        grid.BackgroundColor = Color.White;
        //
        // picRec
        //
        picRec.Name = "picRec";
        picRec.Dock = DockStyle.Fill;
        picRec.SizeMode = PictureBoxSizeMode.Zoom;
        picRec.BackColor = Color.FromArgb(232, 232, 232);
        //
        // tabLearn
        //
        tabLearn.Text = "学习";
        tabLearn.Name = "tabLearn";
        tabLearn.Padding = new Padding(0, 4, 0, 0);
        tabLearn.Controls.Add(splitLearn);
        tabLearn.Controls.Add(pnlLearnTop);
        //
        // pnlLearnTop
        //
        pnlLearnTop.Name = "pnlLearnTop";
        pnlLearnTop.Location = new Point(0, 0);
        pnlLearnTop.Size = new Size(1368, 52);
        pnlLearnTop.Dock = DockStyle.Top;
        pnlLearnTop.Controls.Add(lblLearnImage);
        pnlLearnTop.Controls.Add(tbLearnImage);
        pnlLearnTop.Controls.Add(btnOpenLearnImage);
        //
        // lblLearnImage
        //
        lblLearnImage.Text = "图片：";
        lblLearnImage.Name = "lblLearnImage";
        lblLearnImage.AutoSize = true;
        lblLearnImage.Location = new Point(10, 17);
        //
        // tbLearnImage
        //
        tbLearnImage.Name = "tbLearnImage";
        tbLearnImage.Location = new Point(60, 14);
        tbLearnImage.Width = 560;
        tbLearnImage.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        //
        // btnOpenLearnImage
        //
        btnOpenLearnImage.Text = "打开";
        btnOpenLearnImage.Name = "btnOpenLearnImage";
        btnOpenLearnImage.Location = new Point(628, 12);
        btnOpenLearnImage.Size = new Size(70, 30);
        btnOpenLearnImage.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnOpenLearnImage.Click += btnOpenLearnImage_Click;
        //
        // splitLearn
        //
        splitLearn.Name = "splitLearn";
        splitLearn.Location = new Point(0, 0);
        splitLearn.Size = new Size(1360, 700);
        splitLearn.Dock = DockStyle.Fill;
        splitLearn.Orientation = Orientation.Vertical;
        splitLearn.Panel1MinSize = 300;
        splitLearn.Panel2MinSize = 420;
        splitLearn.SplitterWidth = 6;
        splitLearn.SplitterDistance = 900;
        splitLearn.Panel1.Controls.Add(picLearn);
        splitLearn.Panel2.Controls.Add(lblBoxes);
        splitLearn.Panel2.Controls.Add(lvBoxes);
        splitLearn.Panel2.Controls.Add(btnDelBox);
        splitLearn.Panel2.Controls.Add(btnRenameBox);
        splitLearn.Panel2.Controls.Add(btnSaveAnno);
        splitLearn.Panel2.Controls.Add(lblAnnoInfo);
        splitLearn.Panel2.Controls.Add(btnInitEnv);
        splitLearn.Panel2.Controls.Add(btnTrain);
        splitLearn.Panel2.Controls.Add(btnCancelTrain);
        splitLearn.Panel2.Controls.Add(txtTrainLog);
        //
        // picLearn
        //
        picLearn.Name = "picLearn";
        picLearn.Dock = DockStyle.Fill;
        picLearn.SizeMode = PictureBoxSizeMode.Zoom;
        picLearn.BackColor = Color.FromArgb(232, 232, 232);
        picLearn.MouseDown += picLearn_MouseDown;
        picLearn.MouseMove += picLearn_MouseMove;
        picLearn.MouseUp += picLearn_MouseUp;
        picLearn.Paint += picLearn_Paint;
        //
        // lblBoxes
        //
        lblBoxes.Text = "本图标注框：";
        lblBoxes.Name = "lblBoxes";
        lblBoxes.AutoSize = true;
        lblBoxes.Location = new Point(8, 8);
        //
        // lvBoxes
        //
        lvBoxes.Name = "lvBoxes";
        lvBoxes.View = View.Details;
        lvBoxes.FullRowSelect = true;
        lvBoxes.Location = new Point(8, 32);
        lvBoxes.Size = new Size(438, 240);
        lvBoxes.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        //
        // btnDelBox
        //
        btnDelBox.Text = "删除框";
        btnDelBox.Name = "btnDelBox";
        btnDelBox.Location = new Point(8, 280);
        btnDelBox.Size = new Size(100, 30);
        btnDelBox.Click += btnDelBox_Click;
        //
        // btnRenameBox
        //
        btnRenameBox.Text = "改名";
        btnRenameBox.Name = "btnRenameBox";
        btnRenameBox.Location = new Point(114, 280);
        btnRenameBox.Size = new Size(90, 30);
        btnRenameBox.Click += btnRenameBox_Click;
        //
        // btnSaveAnno
        //
        btnSaveAnno.Text = "保存标注到数据集";
        btnSaveAnno.Name = "btnSaveAnno";
        btnSaveAnno.Location = new Point(210, 280);
        btnSaveAnno.Size = new Size(236, 30);
        btnSaveAnno.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        btnSaveAnno.Click += btnSaveAnno_Click;
        //
        // lblAnnoInfo
        //
        lblAnnoInfo.Name = "lblAnnoInfo";
        lblAnnoInfo.AutoSize = true;
        lblAnnoInfo.ForeColor = Color.Gray;
        lblAnnoInfo.Location = new Point(8, 318);
        lblAnnoInfo.Text = "尚未标注";
        //
        // btnInitEnv
        //
        btnInitEnv.Text = "初始化训练环境";
        btnInitEnv.Name = "btnInitEnv";
        btnInitEnv.Location = new Point(8, 348);
        btnInitEnv.Size = new Size(150, 32);
        btnInitEnv.Click += btnInitEnv_Click;
        //
        // btnTrain
        //
        btnTrain.Text = "开始训练";
        btnTrain.Name = "btnTrain";
        btnTrain.Location = new Point(164, 348);
        btnTrain.Size = new Size(110, 32);
        btnTrain.BackColor = Color.FromArgb(36, 160, 90);
        btnTrain.ForeColor = Color.White;
        btnTrain.FlatStyle = FlatStyle.Flat;
        btnTrain.Click += btnTrain_Click;
        //
        // btnCancelTrain
        //
        btnCancelTrain.Text = "取消训练";
        btnCancelTrain.Name = "btnCancelTrain";
        btnCancelTrain.Location = new Point(280, 348);
        btnCancelTrain.Size = new Size(100, 32);
        btnCancelTrain.Click += btnCancelTrain_Click;
        //
        // txtTrainLog
        //
        txtTrainLog.Name = "txtTrainLog";
        txtTrainLog.Multiline = true;
        txtTrainLog.ReadOnly = true;
        txtTrainLog.ScrollBars = ScrollBars.Vertical;
        txtTrainLog.Location = new Point(8, 388);
        txtTrainLog.Size = new Size(438, 260);
        txtTrainLog.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
        //
        // MainForm
        //
        AutoScaleDimensions = new SizeF(7F, 17F);
        AutoScaleMode = AutoScaleMode.Font;
        Text = "YoloFool 傻瓜式视觉识别";
        Name = "MainForm";
        ClientSize = new Size(1384, 821);
        MinimumSize = new Size(1100, 700);
        StartPosition = FormStartPosition.CenterScreen;
        Font = new Font("Microsoft YaHei UI", 10F);
        Controls.Add(tabMain);
        Load += MainForm_Load;
        tabMain.ResumeLayout(false);
        tabRecognize.ResumeLayout(false);
        tabLearn.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)splitRec).EndInit();
        splitRec.Panel1.ResumeLayout(false);
        splitRec.Panel2.ResumeLayout(false);
        splitRec.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)splitLearn).EndInit();
        splitLearn.Panel1.ResumeLayout(false);
        splitLearn.Panel2.ResumeLayout(false);
        splitLearn.ResumeLayout(false);
        pnlLearnTop.ResumeLayout(false);
        pnlLearnTop.PerformLayout();
        pnlTop.ResumeLayout(false);
        pnlTop.PerformLayout();
        pnlAdv.ResumeLayout(false);
        pnlAdv.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)numConf).EndInit();
        ((System.ComponentModel.ISupportInitialize)numNms).EndInit();
        ResumeLayout(false);
    }

    #endregion

    private TabControl tabMain;
    private TabPage tabRecognize;
    private TabPage tabLearn;
    private SplitContainer splitRec;
    private SplitContainer splitLearn;
    private Panel pnlLearnTop;
    private Panel pnlTop;
    private Label lblImagePath;
    private TextBox tbRecImage;
    private Button btnSelectImage;
    private Button btnRun;
    private Button btnToggleAdv;
    private Panel pnlAdv;
    private Label lblAdvModel;
    private TextBox tbAdvModel;
    private Button btnAdvModel;
    private Label lblAdvKind;
    private ComboBox cboAdvKind;
    private Label lblAdvEngine;
    private ComboBox cboAdvEngine;
    private Label lblAdvDevice;
    private ComboBox cboAdvDevice;
    private Label lblAdvProvider;
    private ComboBox cboAdvProvider;
    private Label lblAdvConf;
    private NumericUpDown numConf;
    private Label lblAdvNms;
    private NumericUpDown numNms;
    private Button btnSaveAdv;
    private Label lblAdvModelInfo;
    private DataGridView grid;
    private PictureBox picRec;
    private Label lblLearnImage;
    private TextBox tbLearnImage;
    private Button btnOpenLearnImage;
    private PictureBox picLearn;
    private Label lblBoxes;
    private ListView lvBoxes;
    private Button btnDelBox;
    private Button btnRenameBox;
    private Button btnSaveAnno;
    private Label lblAnnoInfo;
    private Button btnInitEnv;
    private Button btnTrain;
    private Button btnCancelTrain;
    private TextBox txtTrainLog;
}
