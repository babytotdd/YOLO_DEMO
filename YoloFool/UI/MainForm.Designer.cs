namespace YoloFool
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// 该方法内容由设计器维护：主窗体外壳（顶栏 / 页签 / 状态栏）。
        /// 各页签内部的卡片与数据控件仍在 MainForm.*.cs 中按运行期数据生成，
        /// 设计器里只显示页签框架，方便调整窗口尺寸、页签顺序与顶栏布局。
        /// </summary>
        private void InitializeComponent()
        {
            tlpRoot = new TableLayoutPanel();
            pnlHeader = new TableLayoutPanel();
            tlpTitle = new TableLayoutPanel();
            lblTitle = new Label();
            lblSubtitle = new Label();
            tlpModel = new TableLayoutPanel();
            _lblModelStatus = new Label();
            _lblModelSub = new Label();
            _btnSelfCheck = new Button();
            _tabs = new TabControl();
            _pageRec = new TabPage();
            tlpCap = new TableLayoutPanel();
            tlpCapBody = new TableLayoutPanel();
            tlpCapBar = new TableLayoutPanel();
            lblCapSource = new Label();
            _tbRecImage = new TextBox();
            _btnSelectImage = new Button();
            _btnCamera = new Button();
            _btnPickVideo = new Button();
            _btnSnapshot = new Button();
            _btnRecord = new Button();
            _btnStartVideo = new Button();
            _btnStopVideo = new Button();
            _btnClear = new Button();
            _btnToggleAdv = new Button();
            tlpVideoOpt = new TableLayoutPanel();
            flowVideo1 = new FlowLayoutPanel();
            _chkVideoLoop = new CheckBox();
            lblVideoStep = new Label();
            _numVideoStep = new NumericUpDown();
            _btnDrawLine = new Button();
            _btnClearLine = new Button();
            _lblVideoLine = new Label();
            flowVideo2 = new FlowLayoutPanel();
            lblVideoTask = new Label();
            _cboVideoTask = new ComboBox();
            lblVideoConf = new Label();
            _numVideoConf = new NumericUpDown();
            lblVideoClasses = new Label();
            _tbVideoClasses = new TextBox();
            _chkVideoBest = new CheckBox();
            _btnVideoModel = new Button();
            _lblVideoModel = new Label();
            _lblVideoInfo = new Label();
            _pnlAdv = new Panel();
            tlpAdvGrid = new TableLayoutPanel();
            lblAdvModel = new Label();
            tlpAdvPick = new TableLayoutPanel();
            _tbAdvModel = new TextBox();
            _btnAdvModel = new Button();
            lblAdvKind = new Label();
            _cboKind = new ComboBox();
            lblAdvDevice = new Label();
            _cboDevice = new ComboBox();
            lblAdvProvider = new Label();
            _cboProvider = new ComboBox();
            lblAdvThr = new Label();
            flowAdvThr = new FlowLayoutPanel();
            _numConf = new NumericUpDown();
            lblAdvNms = new Label();
            _numNms = new NumericUpDown();
            _lblAdvInfo = new Label();
            tlpAdvFooter = new TableLayoutPanel();
            lblAdvFooterHint = new Label();
            flowAdvFooter = new FlowLayoutPanel();
            _btnVerifySave = new Button();
            _btnResetAdv = new Button();
            _lblAdvProbe = new Label();
            _pageLearn = new TabPage();
            _tlpLearn = new TableLayoutPanel();
            _tlpLearnBar = new TableLayoutPanel();
            _lblLearnSource = new Label();
            _tbLearnImage = new TextBox();
            _btnOpenLearn = new Button();
            _btnOpenFolder = new Button();
            _btnClearLearn = new Button();
            _lblLearnHint = new Label();
            _splitLearn = new SplitContainer();
            _pnlLearnLeft = new Panel();
            _picLearn = new ImageCanvas();
            _pnlLearnSide = new Panel();
            _tlpLearnStack = new TableLayoutPanel();
            _cardImgList = new Panel();
            _cardImgListBody = new TableLayoutPanel();
            _lblImgListTitle = new Label();
            _lvImages = new ListView();
            colLvImage = new ColumnHeader();
            colLvAnno = new ColumnHeader();
            colLvStatus = new ColumnHeader();
            _flowImgList = new FlowLayoutPanel();
            _btnPrevImage = new Button();
            _btnNextImage = new Button();
            _flowAuto = new FlowLayoutPanel();
            _btnAutoLabel = new Button();
            _btnReview = new Button();
            _btnClearAuto = new Button();
            _lblImageListInfo = new Label();
            _lblReviewHint = new Label();
            _cardBoxes = new Panel();
            _cardBoxesBody = new TableLayoutPanel();
            _lblBoxesTitle = new Label();
            _lvBoxes = new ListView();
            colBoxName = new ColumnHeader();
            colBoxEn = new ColumnHeader();
            colBoxRect = new ColumnHeader();
            _flowBoxOps = new FlowLayoutPanel();
            _btnDelBox = new Button();
            _btnRenameBox = new Button();
            _btnReexport = new Button();
            _lblAnnoInfo = new Label();
            _cardDataset = new Panel();
            _cardDatasetBody = new TableLayoutPanel();
            _lblDatasetTitle = new Label();
            _lblDatasetStats = new Label();
            _lnkOpenDataset = new Button();
            _cardTrain = new Panel();
            _cardTrainBody = new TableLayoutPanel();
            _lblTrainTitle = new Label();
            _tlpTrainGrid = new TableLayoutPanel();
            _tlpTrainLeft = new TableLayoutPanel();
            _lblEpochs = new Label();
            _numEpochs = new NumericUpDown();
            _lblImgSize = new Label();
            _cboImgSize = new ComboBox();
            _lblBatch = new Label();
            _numBatch = new NumericUpDown();
            _flowTrainBtns = new FlowLayoutPanel();
            _btnInitEnv = new Button();
            _btnTrain = new Button();
            _btnCancelTrain = new Button();
            _tlpTrainRight = new TableLayoutPanel();
            _lblTrainStage = new Label();
            _pbTrain = new ProgressBar();
            _txtTrainLog = new TextBox();
            _pageTools = new TabPage();
            _tlpTools = new TableLayoutPanel();
            _tlpToolsBar = new TableLayoutPanel();
            _lblToolsTitle = new Label();
            _lblToolsHint = new Label();
            _btnToolsRefresh = new Button();
            _pnlToolsScroll = new Panel();
            _tlpToolsStack = new TableLayoutPanel();
            _cardToolsFirstRun = new Panel();
            _bodyToolsFirstRun = new TableLayoutPanel();
            _lblToolsFirstRunTitle = new Label();
            _lblToolsFirstRunInfo = new Label();
            _lvToolsEnv = new ListView();
            _chkToolsEnvTraining = new CheckBox();
            _flowToolsEnv = new FlowLayoutPanel();
            _btnToolsEnvCheck = new Button();
            _btnToolsEnvInit = new Button();
            _btnToolsEnvCamera = new Button();
            _lblToolsEnvInfo = new Label();
            _cardToolsLib = new Panel();
            _bodyToolsLib = new TableLayoutPanel();
            _lblToolsLibTitle = new Label();
            _lblToolsLibInfo = new Label();
            _tlpToolsLibPick = new TableLayoutPanel();
            _cboToolsLibCategory = new ComboBox();
            _btnToolsLibFolderOpen = new Button();
            _lvToolsLibModels = new ListView();
            _flowToolsLib = new FlowLayoutPanel();
            _btnToolsLibUse = new Button();
            _btnToolsLibOpen = new Button();
            _lblToolsLibDir = new Label();
            _cardToolsModel = new Panel();
            _bodyToolsModel = new TableLayoutPanel();
            _lblToolsModelTitle = new Label();
            _lblToolsModelMain = new Label();
            _lblToolsModelMeta = new Label();
            _lblToolsModelClasses = new Label();
            _lblToolsModelTrain = new Label();
            _flowToolsModel = new FlowLayoutPanel();
            _btnToolsUseModel = new Button();
            _btnToolsModelBackup = new Button();
            _btnToolsModelFolder = new Button();
            _cardToolsModelClasses = new Panel();
            _bodyToolsModelClasses = new TableLayoutPanel();
            _lblToolsMcTitle = new Label();
            _lblToolsMcInfo = new Label();
            _gridToolsMc = new DataGridView();
            dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn4 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn5 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn6 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn7 = new DataGridViewTextBoxColumn();
            _lblToolsMcHint = new Label();
            _flowToolsMc = new FlowLayoutPanel();
            _btnToolsMcRename = new Button();
            _btnToolsMcDelete = new Button();
            _btnToolsMcMute = new Button();
            _btnToolsMcTrain = new Button();
            _cardToolsAnchorLib = new Panel();
            _bodyToolsAnchorLib = new TableLayoutPanel();
            _lblToolsAnchorLibTitle = new Label();
            _tlpToolsAnchorPick = new TableLayoutPanel();
            _cboToolsAnchorModel = new ComboBox();
            _lblToolsAnchorStats = new Label();
            _lblToolsAnchorModelInfo = new Label();
            _lblToolsAnchorCalibInfo = new Label();
            _flowToolsAnchorLib = new FlowLayoutPanel();
            _btnToolsAnchorNew = new Button();
            _btnToolsAnchorCopy = new Button();
            _btnToolsAnchorTrain = new Button();
            _btnToolsAnchorFolder = new Button();
            _btnToolsAnchorArchive = new Button();
            _cardToolsMachine = new Panel();
            _bodyToolsMachine = new TableLayoutPanel();
            _lblToolsMachineTitle = new Label();
            _tlpToolsMachinePick = new TableLayoutPanel();
            _cboToolsMachineKind = new ComboBox();
            _lblToolsMachineInfo = new Label();
            _lvToolsMachines = new ListView();
            _flowToolsMachine = new FlowLayoutPanel();
            _btnToolsMachineNew = new Button();
            _btnToolsMachineRename = new Button();
            _btnToolsMachineArchive = new Button();
            _btnToolsMachineFolder = new Button();
            _cardToolsDataset = new Panel();
            _bodyToolsDataset = new TableLayoutPanel();
            _lblToolsDatasetTitle = new Label();
            _lblToolsDatasetStats = new Label();
            _lblToolsDatasetClasses = new Label();
            _lblToolsDatasetExtra = new Label();
            _flowToolsDataset = new FlowLayoutPanel();
            _btnToolsDatasetRebuild = new Button();
            _btnToolsDatasetFolder = new Button();
            _cardToolsLabels = new Panel();
            _bodyToolsLabels = new TableLayoutPanel();
            _lblToolsLabelsTitle = new Label();
            _lblToolsLabelsInfo = new Label();
            _gridToolsLabels = new DataGridView();
            dataGridViewTextBoxColumn8 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn9 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn10 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn11 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn12 = new DataGridViewTextBoxColumn();
            _lblToolsLabelHint = new Label();
            _flowToolsLabels = new FlowLayoutPanel();
            _btnToolsLabelView = new Button();
            _btnToolsLabelDelete = new Button();
            _cardToolsBackup = new Panel();
            _bodyToolsBackup = new TableLayoutPanel();
            _lblToolsBackupTitle = new Label();
            _lvToolsBackups = new ListBox();
            _lblToolsBackupInfo = new Label();
            _flowToolsBackup = new FlowLayoutPanel();
            _btnToolsBackupRestore = new Button();
            _btnToolsBackupDelete = new Button();
            _btnToolsBackupFolder = new Button();
            _cardToolsMisdetect = new Panel();
            _bodyToolsMisdetect = new TableLayoutPanel();
            _lblToolsMisdetectTitle = new Label();
            _lblToolsMisdetect = new Label();
            _flowToolsMisdetect = new FlowLayoutPanel();
            _btnToolsMisdetectFolder = new Button();
            _btnToolsMisdetectClear = new Button();
            _pageFixture = new TabPage();
            _tlpFixture = new TableLayoutPanel();
            _tlpFixBar = new TableLayoutPanel();
            _flpFixBar = new FlowLayoutPanel();
            _lblFixModel = new Label();
            _cboFixModel = new ComboBox();
            _btnFixNewModel = new Button();
            _btnFixMode = new Button();
            _btnFixCamApply = new Button();
            _btnFixCamClose = new Button();
            _btnFixSnapshot = new Button();
            _btnFixLive = new Button();
            _btnFixOverlay = new Button();
            _lblFixCamInfo = new Label();
            _tlpFixBody = new TableLayoutPanel();
            _picFixture = new ImageCanvas();
            _tlpFixSide = new TableLayoutPanel();
            _pnlFixVerdict = new Panel();
            _lblFixVerdict = new Label();
            _lblFixReason = new Label();
            _pnlFixHost = new Panel();
            _pnlFixExec = new Panel();
            _tlpFixExec = new TableLayoutPanel();
            _btnFixJudge = new Button();
            _lblFixExecHint = new Label();
            _btnFixReplay2 = new Button();
            _pnlFixDebug = new Panel();
            _tlpFixDebugHost = new TableLayoutPanel();
            _flpFixThumbs = new FlowLayoutPanel();
            _tcFixDebug = new TabControl();
            _fixTabCamera = new TabPage();
            _flpFixColCamera = new FlowLayoutPanel();
            _flpFixRowCam1 = new FlowLayoutPanel();
            _lblFixCamIndex = new Label();
            _cboFixCamIndex = new ComboBox();
            _lblFixCamRes = new Label();
            _cboFixRes = new ComboBox();
            _flpFixRowCam2 = new FlowLayoutPanel();
            _lblFixCamFormat = new Label();
            _cboFixFourCC = new ComboBox();
            _flpFixRowCam3 = new FlowLayoutPanel();
            _chkFixExposure = new CheckBox();
            _lblFixExposure = new Label();
            _numFixExposure = new NumericUpDown();
            _lblFixGain = new Label();
            _numFixGain = new NumericUpDown();
            _lblFixExposureHint = new Label();
            _flpFixRowCam4 = new FlowLayoutPanel();
            _btnFixCamOpenApply = new Button();
            _btnFixCamDriver = new Button();
            _btnFixRoi = new Button();
            _lblFixRoi = new Label();
            _fixTabCalib = new TabPage();
            _flpFixColCalib = new FlowLayoutPanel();
            _lblFixCalibHint = new Label();
            _flpFixRowCalib1 = new FlowLayoutPanel();
            _btnFixCalibPick = new Button();
            _lblFixCalibMm = new Label();
            _numFixCalibMm = new NumericUpDown();
            _flpFixRowCalib2 = new FlowLayoutPanel();
            _btnFixCalibSave = new Button();
            _lblFixCalib = new Label();
            _fixTabTemplate = new TabPage();
            _flpFixColTemplate = new FlowLayoutPanel();
            _lblFixTemplateHint = new Label();
            _flpFixRowTpl1 = new FlowLayoutPanel();
            _btnFixCaptureEmpty = new Button();
            _btnFixCaptureGolden = new Button();
            _flpFixRowTpl2 = new FlowLayoutPanel();
            _btnFixRebuildSeg = new Button();
            _flpFixRowTpl3 = new FlowLayoutPanel();
            _btnFixZoneAuto = new Button();
            _btnFixZoneDraw = new Button();
            _btnFixZoneClear = new Button();
            _lblFixZone = new Label();
            _lblFixTemplate = new Label();
            _fixTabThreshold = new TabPage();
            _flpFixColThreshold = new FlowLayoutPanel();
            _flpFixRowJudgeMode = new FlowLayoutPanel();
            _lblFixJudgeMode = new Label();
            _cboFixJudgeMode = new ComboBox();
            _lblFixJudgeModeHint = new Label();
            _flpFixRowMargin = new FlowLayoutPanel();
            _lblFixMargin = new Label();
            _numFixMarginPx = new NumericUpDown();
            _lblFixMarginUnit = new Label();
            _flpFixRowOverflow = new FlowLayoutPanel();
            _lblFixOverflow = new Label();
            _numFixOverflowPx = new NumericUpDown();
            _lblFixOverflowUnit = new Label();
            _flpFixRowPos = new FlowLayoutPanel();
            _lblFixPos = new Label();
            _numFixPos = new NumericUpDown();
            _lblFixPosUnit = new Label();
            _flpFixRowPosTrack = new FlowLayoutPanel();
            _trkFixPos = new TrackBar();
            _lblFixPosHint = new Label();
            _flpFixRowAngle = new FlowLayoutPanel();
            _lblFixAngleNum = new Label();
            _numFixAngle = new NumericUpDown();
            _lblFixAngleUnit = new Label();
            _flpFixRowAngleTrack = new FlowLayoutPanel();
            _trkFixAngle = new TrackBar();
            _lblFixAngleHint = new Label();
            _flpFixRowShape = new FlowLayoutPanel();
            _lblFixShape = new Label();
            _cboFixShape = new ComboBox();
            _lblFixShapeHint = new Label();
            _flpFixRowConfirm = new FlowLayoutPanel();
            _lblFixConfirm = new Label();
            _numFixConfirm = new NumericUpDown();
            _lblFixConfirmUnit = new Label();
            _lblFixConfirmHint = new Label();
            _flpFixRowPreset = new FlowLayoutPanel();
            _lblFixPreset = new Label();
            _btnFixPresetLoose = new Button();
            _btnFixPresetStd = new Button();
            _btnFixPresetStrict = new Button();
            _lblFixTolSummary = new Label();
            _flpFixRowSaveTol = new FlowLayoutPanel();
            _btnFixSaveTol = new Button();
            _btnFixAdvToggle = new Button();
            _pnlFixAdv = new FlowLayoutPanel();
            _flpFixRowDxDy = new FlowLayoutPanel();
            _lblFixDx = new Label();
            _numFixDx = new NumericUpDown();
            _lblFixDy = new Label();
            _numFixDy = new NumericUpDown();
            _flpFixRowIouZone = new FlowLayoutPanel();
            _lblFixIou = new Label();
            _numFixIou = new NumericUpDown();
            _lblFixZoneMm = new Label();
            _numFixZone = new NumericUpDown();
            _lblFixTheta = new Label();
            _numFixTheta = new NumericUpDown();
            _flpFixRowArea = new FlowLayoutPanel();
            _lblFixAreaMin = new Label();
            _numFixAreaMin = new NumericUpDown();
            _lblFixAreaMax = new Label();
            _numFixAreaMax = new NumericUpDown();
            _flpFixRowQuality = new FlowLayoutPanel();
            _lblFixMeanGray = new Label();
            _numFixMeanGray = new NumericUpDown();
            _lblFixContrast = new Label();
            _numFixContrast = new NumericUpDown();
            _flpFixRowQuality2 = new FlowLayoutPanel();
            _lblFixHighlight = new Label();
            _numFixHighlight = new NumericUpDown();
            _lblFixContour = new Label();
            _numFixContour = new NumericUpDown();
            _flpFixRowDrop = new FlowLayoutPanel();
            _lblFixDrop = new Label();
            _numFixDrop = new NumericUpDown();
            _lblFixGoldenScore = new Label();
            _lblFixAdvHint = new Label();
            _fixTabTest = new TabPage();
            _flpFixColTest = new FlowLayoutPanel();
            _flpFixRowTest1 = new FlowLayoutPanel();
            _btnFixTest = new Button();
            _btnFixReplay = new Button();
            _lblFixTestInfo = new Label();
            _fixSegTab = new TabPage();
            _flpFixColSeg = new FlowLayoutPanel();
            _lblFixSegHint1 = new Label();
            _flpFixRowSegH = new FlowLayoutPanel();
            _lblFixSegH = new Label();
            _numFixSegHMin = new NumericUpDown();
            _lblFixSegHTilde = new Label();
            _numFixSegHMax = new NumericUpDown();
            _flpFixRowSegS = new FlowLayoutPanel();
            _lblFixSegS = new Label();
            _numFixSegSMin = new NumericUpDown();
            _lblFixSegSTilde = new Label();
            _numFixSegSMax = new NumericUpDown();
            _flpFixRowSegV = new FlowLayoutPanel();
            _lblFixSegV = new Label();
            _numFixSegVMin = new NumericUpDown();
            _lblFixSegVTilde = new Label();
            _numFixSegVMax = new NumericUpDown();
            _lblFixSegHint2 = new Label();
            _flpFixRowSegMorph = new FlowLayoutPanel();
            _lblFixSegMorph = new Label();
            _numFixSegMorph = new NumericUpDown();
            _lblFixSegMorphUnit = new Label();
            _lblFixSegHint3 = new Label();
            _flpFixRowSegDiff = new FlowLayoutPanel();
            _chkFixSegUseEmpty = new CheckBox();
            _lblFixSegDiff = new Label();
            _numFixSegDiff = new NumericUpDown();
            _lblFixSegHint4 = new Label();
            _flpFixRowSegSave = new FlowLayoutPanel();
            _btnFixSegEstimate = new Button();
            _btnFixSegApply = new Button();
            _lblFixSegInfo = new Label();
            _lblFixSegHint5 = new Label();
            _lvFixLog = new ListView();
            colFixLogTime = new ColumnHeader();
            colFixLogResult = new ColumnHeader();
            colFixLogReason = new ColumnHeader();
            colFixLogOffset = new ColumnHeader();
            colFixLogCost = new ColumnHeader();
            _pageAnchor = new TabPage();
            _tlpAnchor = new TableLayoutPanel();
            _tlpAnchorTop = new TableLayoutPanel();
            _tlpAnchorBar = new TableLayoutPanel();
            _flowAnchorBarModel = new FlowLayoutPanel();
            _lblAnchorBarModelCap = new Label();
            _cboAnchorModel = new ComboBox();
            _btnAnchorNewModel = new Button();
            _btnAnchorCopyModel = new Button();
            _lblAnchorModelInfo = new Label();
            _flowAnchorBarCam = new FlowLayoutPanel();
            _lblAnchorBarCamCap = new Label();
            _cboAnchorCamIndex = new ComboBox();
            _cboAnchorRes = new ComboBox();
            _chkAnchorExposure = new CheckBox();
            _numAnchorExposure = new NumericUpDown();
            _btnAnchorShot = new Button();
            _btnAnchorCam = new Button();
            _btnAnchorJudge = new Button();
            _lblAnchorCamInfo = new Label();
            _pnlAnchorVerdict = new Panel();
            _tlpAnchorVerdict = new TableLayoutPanel();
            _lblAnchorVerdict = new Label();
            _lblAnchorReason = new Label();
            _lblAnchorDetail = new Label();
            _tlpAnchorBody = new TableLayoutPanel();
            _picAnchor = new ImageCanvas();
            _tlpAnchorSide = new TableLayoutPanel();
            _anchorTabs = new TabControl();
            _tabAnchorTarget = new TabPage();
            _hostAnchorLearn = new Panel();
            _tlpAnchorLearn = new TableLayoutPanel();
            _flowAnchorLearnBar = new FlowLayoutPanel();
            _btnAnchorLearnPick = new Button();
            _btnAnchorLearnDel = new Button();
            _btnAnchorLearnDelShot = new Button();
            _btnAnchorLearnLive = new Button();
            _lblAnchorLearnInfo = new Label();
            _lvAnchorDrafts = new ListView();
            colAnchorDraft = new ColumnHeader();
            colAnchorDraftPos = new ColumnHeader();
            colAnchorDraftAngle = new ColumnHeader();
            _lvAnchorSamples = new ListView();
            colAnchorSample = new ColumnHeader();
            colAnchorSampleTarget = new ColumnHeader();
            colAnchorSampleBoxes = new ColumnHeader();
            colAnchorSampleTime = new ColumnHeader();
            _lblAnchorSampleInfo = new Label();
            _picAnchorSample = new ImageCanvas();
            _tlpAnchorTrainBar = new TableLayoutPanel();
            _flowAnchorTrainParams = new FlowLayoutPanel();
            _lblAnchorEpochsCap = new Label();
            _numTrainEpochs = new NoWheelNumeric();
            _lblAnchorImgszCap = new Label();
            _numTrainImgsz = new NoWheelNumeric();
            _flowAnchorTrainBtns = new FlowLayoutPanel();
            _btnAnchorTrain = new Button();
            _btnAnchorTrainRefresh = new Button();
            _txtAnchorTrainLog = new TextBox();
            _lblAnchorClasses = new Label();
            _lblAnchorTargetInfo = new Label();
            _tabAnchorZone = new TabPage();
            _hostAnchorZone = new Panel();
            _tlpAnchorZone = new TableLayoutPanel();
            _flowAnchorZoneBar = new FlowLayoutPanel();
            _btnAnchorZonePick = new Button();
            _btnAnchorZoneFromLabel = new Button();
            _btnAnchorZoneCapture = new Button();
            _btnAnchorZoneEdit = new Button();
            _btnAnchorZoneRename = new Button();
            _btnAnchorZoneRemove = new Button();
            _lvAnchorZones = new ListView();
            colAnchorZoneName = new ColumnHeader();
            colAnchorZoneLabel = new ColumnHeader();
            colAnchorZoneMatch = new ColumnHeader();
            colAnchorZoneShift = new ColumnHeader();
            colAnchorZoneState = new ColumnHeader();
            _tlpAnchorZoneRef = new TableLayoutPanel();
            _picZoneRef = new PictureBox();
            _lblZoneRefInfo = new Label();
            _tlpAnchorZoneParams = new TableLayoutPanel();
            _lblZoneMatchCap = new Label();
            _numZoneMatch = new NoWheelNumeric();
            _lblZoneMatchTip = new Label();
            _lblZoneSearchCap = new Label();
            _numZoneSearch = new NoWheelNumeric();
            _lblZoneSearchTip = new Label();
            _lblZoneShiftCap = new Label();
            _numZoneShift = new NoWheelNumeric();
            _lblZoneShiftTip = new Label();
            _lblZoneClassCap = new Label();
            _cboZoneClass = new ComboBox();
            _lblZoneClassTip = new Label();
            _chkZoneRequired = new CheckBox();
            _chkAnchorAdv = new CheckBox();
            _pnlAnchorTol = new Panel();
            _tlpAnchorTol = new TableLayoutPanel();
            _lblAnchorTolShift = new Label();
            _numAnchorShift = new NoWheelNumeric();
            _lblAnchorTolRot = new Label();
            _numAnchorRot = new NoWheelNumeric();
            _lblAnchorTolScale = new Label();
            _numAnchorScale = new NoWheelNumeric();
            _lblAnchorTolRms = new Label();
            _numAnchorRms = new NoWheelNumeric();
            _lblAnchorTolConf = new Label();
            _numAnchorConf = new NoWheelNumeric();
            _lblAnchorTolMin = new Label();
            _numAnchorMin = new NoWheelNumeric();
            _flowAnchorZoneJudge = new FlowLayoutPanel();
            _btnAnchorZoneSaveTol = new Button();
            _btnAnchorZoneOverlay = new Button();
            _pnlAnchorHitsCard = new Panel();
            _tlpAnchorHitsCard = new TableLayoutPanel();
            _lblAnchorHitsTitle = new Label();
            _lvAnchorHits = new ListView();
            colAnchorHitName = new ColumnHeader();
            colAnchorHitBase = new ColumnHeader();
            colAnchorHitActual = new ColumnHeader();
            colAnchorHitDelta = new ColumnHeader();
            colAnchorHitConf = new ColumnHeader();
            _pnlAnchorLogCard = new Panel();
            _tlpAnchorLogCard = new TableLayoutPanel();
            _lblAnchorLogTitle = new Label();
            _lvAnchorLog = new ListView();
            colAnchorLogTime = new ColumnHeader();
            colAnchorLogVerdict = new ColumnHeader();
            colAnchorLogReason = new ColumnHeader();
            colAnchorLogDelta = new ColumnHeader();
            _tlpAnchorZoneHints = new TableLayoutPanel();
            _lblAnchorZoneInfo = new Label();
            _lblAnchorCalibInfo = new Label();
            _lblAnchorJitter = new Label();
            _tabAnchorCalib = new TabPage();
            _tlpAnchorCalib = new TableLayoutPanel();
            _lblAnchorCalibHint = new Label();
            _flowAnchorCalibBar = new FlowLayoutPanel();
            _btnAnchorCalib = new Button();
            _btnAnchorCalibView = new Button();
            _btnAnchorCalibCompare = new Button();
            _btnAnchorCalibLive = new Button();
            _lblAnchorCalibCompare = new Label();
            _pnlAnchorCalibCard = new Panel();
            _tlpAnchorCalibCard = new TableLayoutPanel();
            _lblAnchorCalibHitsTitle = new Label();
            _lvAnchorCalibHits = new ListView();
            colAnchorCalibName = new ColumnHeader();
            colAnchorCalibBase = new ColumnHeader();
            colAnchorCalibActual = new ColumnHeader();
            colAnchorCalibDx = new ColumnHeader();
            colAnchorCalibDy = new ColumnHeader();
            colAnchorCalibConf = new ColumnHeader();
            _pageMultiCam = new TabPage();
            _tlpMultiCam = new TableLayoutPanel();
            _mcWall = new FlowLayoutPanel();
            _tlpMcSide = new TableLayoutPanel();
            _lblMcTitle = new Label();
            _mcGrid = new DataGridView();
            dataGridViewTextBoxColumn13 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn14 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn15 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn16 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn17 = new DataGridViewTextBoxColumn();
            _flowMcOpen = new FlowLayoutPanel();
            _btnMcOpen = new Button();
            _btnMcClose = new Button();
            _flowMcAll = new FlowLayoutPanel();
            _btnMcAllOn = new Button();
            _btnMcAllOff = new Button();
            _flowMcIndex = new FlowLayoutPanel();
            _lblMcIndexHint = new Label();
            _mcNumIndex = new NumericUpDown();
            _btnMcAdd = new Button();
            _btnMcRemove = new Button();
            _mcSelInfo = new Label();
            _pnlMcSettings = new Panel();
            _tlpMcSettings = new TableLayoutPanel();
            _lblMcSettingsTitle = new Label();
            _lblMcRes = new Label();
            _flowMcRes = new FlowLayoutPanel();
            _mcCmbRes = new ComboBox();
            _mcNumW = new NumericUpDown();
            _lblMcTimes = new Label();
            _mcNumH = new NumericUpDown();
            _lblMcFps = new Label();
            _flowMcFps = new FlowLayoutPanel();
            _mcNumFps = new NumericUpDown();
            _mcCmbFmt = new ComboBox();
            _lblMcExp = new Label();
            _flowMcExp = new FlowLayoutPanel();
            _mcChkAutoExp = new CheckBox();
            _mcNumExp = new NumericUpDown();
            _lblMcWb = new Label();
            _flowMcWb = new FlowLayoutPanel();
            _mcChkAutoWb = new CheckBox();
            _mcNumWb = new NumericUpDown();
            _lblMcGain = new Label();
            _mcNumGain = new NumericUpDown();
            _lblMcBright = new Label();
            _mcNumBright = new NumericUpDown();
            _lblMcContrast = new Label();
            _mcNumContrast = new NumericUpDown();
            _lblMcSat = new Label();
            _mcNumSat = new NumericUpDown();
            _flowMcApply = new FlowLayoutPanel();
            _mcBtnApply = new Button();
            _mcLblActual = new Label();
            _lblMcSettingsNote = new Label();
            _lblMcProcTitle = new Label();
            _pnlMcBottom = new Panel();
            _lblMcHint = new Label();
            _mcProcInfo = new Label();
            pnlStatus = new TableLayoutPanel();
            _lblStatus = new Label();
            _lblStatusRight = new Label();
            _splitRec = new SplitContainer();
            pnlRecLeft = new Panel();
            _picRec = new ImageCanvas();
            pnlRecSide = new Panel();
            tlpSide = new TableLayoutPanel();
            tlpSideHead = new TableLayoutPanel();
            _lblResultTitle = new Label();
            flowResultTools = new FlowLayoutPanel();
            _lblFilterState = new Label();
            _btnFilter = new Button();
            _btnBlockClass = new Button();
            _btnDeleteResult = new Button();
            tlpSideStatus = new TableLayoutPanel();
            _lblResultStats = new Label();
            _lblVideoArea = new Label();
            _pnlMetrics = new TableLayoutPanel();
            tlpMetricActive = new TableLayoutPanel();
            lblMetricActiveTitle = new Label();
            _lblVideoActive = new Label();
            tlpMetricTotal = new TableLayoutPanel();
            lblMetricTotalTitle = new Label();
            _lblVideoTotal = new Label();
            tlpMetricAB = new TableLayoutPanel();
            lblMetricABTitle = new Label();
            _lblVideoAtoB = new Label();
            tlpMetricBA = new TableLayoutPanel();
            lblMetricBATitle = new Label();
            _lblVideoBtoA = new Label();
            _grid = new DataGridView();
            colId = new DataGridViewTextBoxColumn();
            colName = new DataGridViewTextBoxColumn();
            colConf = new DataGridViewTextBoxColumn();
            colPos = new DataGridViewTextBoxColumn();
            colCenter = new DataGridViewTextBoxColumn();
            colArea = new DataGridViewTextBoxColumn();
            colPct = new DataGridViewTextBoxColumn();
            lblGridHint = new Label();
            tlpRoot.SuspendLayout();
            pnlHeader.SuspendLayout();
            tlpTitle.SuspendLayout();
            tlpModel.SuspendLayout();
            _tabs.SuspendLayout();
            _pageRec.SuspendLayout();
            tlpCap.SuspendLayout();
            tlpCapBar.SuspendLayout();
            tlpVideoOpt.SuspendLayout();
            flowVideo1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_numVideoStep).BeginInit();
            flowVideo2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_numVideoConf).BeginInit();
            _pnlAdv.SuspendLayout();
            tlpAdvGrid.SuspendLayout();
            tlpAdvPick.SuspendLayout();
            flowAdvThr.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_numConf).BeginInit();
            ((System.ComponentModel.ISupportInitialize)_numNms).BeginInit();
            tlpAdvFooter.SuspendLayout();
            flowAdvFooter.SuspendLayout();
            _pageLearn.SuspendLayout();
            _tlpLearn.SuspendLayout();
            _tlpLearnBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_splitLearn).BeginInit();
            _splitLearn.Panel1.SuspendLayout();
            _splitLearn.Panel2.SuspendLayout();
            _splitLearn.SuspendLayout();
            _pnlLearnLeft.SuspendLayout();
            _pnlLearnSide.SuspendLayout();
            _tlpLearnStack.SuspendLayout();
            _cardImgList.SuspendLayout();
            _cardImgListBody.SuspendLayout();
            _flowImgList.SuspendLayout();
            _flowAuto.SuspendLayout();
            _cardBoxes.SuspendLayout();
            _cardBoxesBody.SuspendLayout();
            _flowBoxOps.SuspendLayout();
            _cardDataset.SuspendLayout();
            _cardDatasetBody.SuspendLayout();
            _cardTrain.SuspendLayout();
            _cardTrainBody.SuspendLayout();
            _tlpTrainGrid.SuspendLayout();
            _tlpTrainLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_numEpochs).BeginInit();
            ((System.ComponentModel.ISupportInitialize)_numBatch).BeginInit();
            _flowTrainBtns.SuspendLayout();
            _tlpTrainRight.SuspendLayout();
            _pageTools.SuspendLayout();
            _tlpTools.SuspendLayout();
            _tlpToolsBar.SuspendLayout();
            _pnlToolsScroll.SuspendLayout();
            _tlpToolsStack.SuspendLayout();
            _cardToolsFirstRun.SuspendLayout();
            _bodyToolsFirstRun.SuspendLayout();
            _flowToolsEnv.SuspendLayout();
            _cardToolsLib.SuspendLayout();
            _bodyToolsLib.SuspendLayout();
            _tlpToolsLibPick.SuspendLayout();
            _flowToolsLib.SuspendLayout();
            _cardToolsModel.SuspendLayout();
            _bodyToolsModel.SuspendLayout();
            _flowToolsModel.SuspendLayout();
            _cardToolsModelClasses.SuspendLayout();
            _bodyToolsModelClasses.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_gridToolsMc).BeginInit();
            _flowToolsMc.SuspendLayout();
            _cardToolsAnchorLib.SuspendLayout();
            _bodyToolsAnchorLib.SuspendLayout();
            _tlpToolsAnchorPick.SuspendLayout();
            _flowToolsAnchorLib.SuspendLayout();
            _cardToolsMachine.SuspendLayout();
            _bodyToolsMachine.SuspendLayout();
            _tlpToolsMachinePick.SuspendLayout();
            _flowToolsMachine.SuspendLayout();
            _cardToolsDataset.SuspendLayout();
            _bodyToolsDataset.SuspendLayout();
            _flowToolsDataset.SuspendLayout();
            _cardToolsLabels.SuspendLayout();
            _bodyToolsLabels.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_gridToolsLabels).BeginInit();
            _flowToolsLabels.SuspendLayout();
            _cardToolsBackup.SuspendLayout();
            _bodyToolsBackup.SuspendLayout();
            _flowToolsBackup.SuspendLayout();
            _cardToolsMisdetect.SuspendLayout();
            _bodyToolsMisdetect.SuspendLayout();
            _flowToolsMisdetect.SuspendLayout();
            _pageFixture.SuspendLayout();
            _tlpFixture.SuspendLayout();
            _tlpFixBar.SuspendLayout();
            _flpFixBar.SuspendLayout();
            _tlpFixBody.SuspendLayout();
            _tlpFixSide.SuspendLayout();
            _pnlFixVerdict.SuspendLayout();
            _pnlFixHost.SuspendLayout();
            _pnlFixExec.SuspendLayout();
            _tlpFixExec.SuspendLayout();
            _pnlFixDebug.SuspendLayout();
            _tlpFixDebugHost.SuspendLayout();
            _tcFixDebug.SuspendLayout();
            _fixTabCamera.SuspendLayout();
            _flpFixColCamera.SuspendLayout();
            _flpFixRowCam1.SuspendLayout();
            _flpFixRowCam2.SuspendLayout();
            _flpFixRowCam3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_numFixExposure).BeginInit();
            ((System.ComponentModel.ISupportInitialize)_numFixGain).BeginInit();
            _flpFixRowCam4.SuspendLayout();
            _fixTabCalib.SuspendLayout();
            _flpFixColCalib.SuspendLayout();
            _flpFixRowCalib1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_numFixCalibMm).BeginInit();
            _flpFixRowCalib2.SuspendLayout();
            _fixTabTemplate.SuspendLayout();
            _flpFixColTemplate.SuspendLayout();
            _flpFixRowTpl1.SuspendLayout();
            _flpFixRowTpl2.SuspendLayout();
            _flpFixRowTpl3.SuspendLayout();
            _fixTabThreshold.SuspendLayout();
            _flpFixColThreshold.SuspendLayout();
            _flpFixRowJudgeMode.SuspendLayout();
            _flpFixRowMargin.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_numFixMarginPx).BeginInit();
            _flpFixRowOverflow.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_numFixOverflowPx).BeginInit();
            _flpFixRowPos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_numFixPos).BeginInit();
            _flpFixRowPosTrack.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_trkFixPos).BeginInit();
            _flpFixRowAngle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_numFixAngle).BeginInit();
            _flpFixRowAngleTrack.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_trkFixAngle).BeginInit();
            _flpFixRowShape.SuspendLayout();
            _flpFixRowConfirm.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_numFixConfirm).BeginInit();
            _flpFixRowPreset.SuspendLayout();
            _flpFixRowSaveTol.SuspendLayout();
            _pnlFixAdv.SuspendLayout();
            _flpFixRowDxDy.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_numFixDx).BeginInit();
            ((System.ComponentModel.ISupportInitialize)_numFixDy).BeginInit();
            _flpFixRowIouZone.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_numFixIou).BeginInit();
            ((System.ComponentModel.ISupportInitialize)_numFixZone).BeginInit();
            ((System.ComponentModel.ISupportInitialize)_numFixTheta).BeginInit();
            _flpFixRowArea.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_numFixAreaMin).BeginInit();
            ((System.ComponentModel.ISupportInitialize)_numFixAreaMax).BeginInit();
            _flpFixRowQuality.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_numFixMeanGray).BeginInit();
            ((System.ComponentModel.ISupportInitialize)_numFixContrast).BeginInit();
            _flpFixRowQuality2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_numFixHighlight).BeginInit();
            ((System.ComponentModel.ISupportInitialize)_numFixContour).BeginInit();
            _flpFixRowDrop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_numFixDrop).BeginInit();
            _fixTabTest.SuspendLayout();
            _flpFixColTest.SuspendLayout();
            _flpFixRowTest1.SuspendLayout();
            _fixSegTab.SuspendLayout();
            _flpFixColSeg.SuspendLayout();
            _flpFixRowSegH.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_numFixSegHMin).BeginInit();
            ((System.ComponentModel.ISupportInitialize)_numFixSegHMax).BeginInit();
            _flpFixRowSegS.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_numFixSegSMin).BeginInit();
            ((System.ComponentModel.ISupportInitialize)_numFixSegSMax).BeginInit();
            _flpFixRowSegV.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_numFixSegVMin).BeginInit();
            ((System.ComponentModel.ISupportInitialize)_numFixSegVMax).BeginInit();
            _flpFixRowSegMorph.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_numFixSegMorph).BeginInit();
            _flpFixRowSegDiff.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_numFixSegDiff).BeginInit();
            _flpFixRowSegSave.SuspendLayout();
            _pageAnchor.SuspendLayout();
            _tlpAnchor.SuspendLayout();
            _tlpAnchorTop.SuspendLayout();
            _tlpAnchorBar.SuspendLayout();
            _flowAnchorBarModel.SuspendLayout();
            _flowAnchorBarCam.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_numAnchorExposure).BeginInit();
            _pnlAnchorVerdict.SuspendLayout();
            _tlpAnchorVerdict.SuspendLayout();
            _tlpAnchorBody.SuspendLayout();
            _tlpAnchorSide.SuspendLayout();
            _anchorTabs.SuspendLayout();
            _tabAnchorTarget.SuspendLayout();
            _hostAnchorLearn.SuspendLayout();
            _tlpAnchorLearn.SuspendLayout();
            _flowAnchorLearnBar.SuspendLayout();
            _tlpAnchorTrainBar.SuspendLayout();
            _flowAnchorTrainParams.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_numTrainEpochs).BeginInit();
            ((System.ComponentModel.ISupportInitialize)_numTrainImgsz).BeginInit();
            _flowAnchorTrainBtns.SuspendLayout();
            _tabAnchorZone.SuspendLayout();
            _hostAnchorZone.SuspendLayout();
            _tlpAnchorZone.SuspendLayout();
            _flowAnchorZoneBar.SuspendLayout();
            _tlpAnchorZoneRef.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_picZoneRef).BeginInit();
            _tlpAnchorZoneParams.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_numZoneMatch).BeginInit();
            ((System.ComponentModel.ISupportInitialize)_numZoneSearch).BeginInit();
            ((System.ComponentModel.ISupportInitialize)_numZoneShift).BeginInit();
            _pnlAnchorTol.SuspendLayout();
            _tlpAnchorTol.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_numAnchorShift).BeginInit();
            ((System.ComponentModel.ISupportInitialize)_numAnchorRot).BeginInit();
            ((System.ComponentModel.ISupportInitialize)_numAnchorScale).BeginInit();
            ((System.ComponentModel.ISupportInitialize)_numAnchorRms).BeginInit();
            ((System.ComponentModel.ISupportInitialize)_numAnchorConf).BeginInit();
            ((System.ComponentModel.ISupportInitialize)_numAnchorMin).BeginInit();
            _flowAnchorZoneJudge.SuspendLayout();
            _pnlAnchorHitsCard.SuspendLayout();
            _tlpAnchorHitsCard.SuspendLayout();
            _pnlAnchorLogCard.SuspendLayout();
            _tlpAnchorLogCard.SuspendLayout();
            _tlpAnchorZoneHints.SuspendLayout();
            _tabAnchorCalib.SuspendLayout();
            _tlpAnchorCalib.SuspendLayout();
            _flowAnchorCalibBar.SuspendLayout();
            _pnlAnchorCalibCard.SuspendLayout();
            _tlpAnchorCalibCard.SuspendLayout();
            _pageMultiCam.SuspendLayout();
            _tlpMultiCam.SuspendLayout();
            _tlpMcSide.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_mcGrid).BeginInit();
            _flowMcOpen.SuspendLayout();
            _flowMcAll.SuspendLayout();
            _flowMcIndex.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_mcNumIndex).BeginInit();
            _pnlMcSettings.SuspendLayout();
            _tlpMcSettings.SuspendLayout();
            _flowMcRes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_mcNumW).BeginInit();
            ((System.ComponentModel.ISupportInitialize)_mcNumH).BeginInit();
            _flowMcFps.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_mcNumFps).BeginInit();
            _flowMcExp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_mcNumExp).BeginInit();
            _flowMcWb.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_mcNumWb).BeginInit();
            ((System.ComponentModel.ISupportInitialize)_mcNumGain).BeginInit();
            ((System.ComponentModel.ISupportInitialize)_mcNumBright).BeginInit();
            ((System.ComponentModel.ISupportInitialize)_mcNumContrast).BeginInit();
            ((System.ComponentModel.ISupportInitialize)_mcNumSat).BeginInit();
            _flowMcApply.SuspendLayout();
            _pnlMcBottom.SuspendLayout();
            pnlStatus.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_splitRec).BeginInit();
            _splitRec.Panel1.SuspendLayout();
            _splitRec.Panel2.SuspendLayout();
            _splitRec.SuspendLayout();
            pnlRecLeft.SuspendLayout();
            pnlRecSide.SuspendLayout();
            tlpSide.SuspendLayout();
            tlpSideHead.SuspendLayout();
            flowResultTools.SuspendLayout();
            tlpSideStatus.SuspendLayout();
            _pnlMetrics.SuspendLayout();
            tlpMetricActive.SuspendLayout();
            tlpMetricTotal.SuspendLayout();
            tlpMetricAB.SuspendLayout();
            tlpMetricBA.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_grid).BeginInit();
            this.SuspendLayout();
            // 
            // tlpRoot
            // 
            tlpRoot.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            tlpRoot.ColumnCount = 1;
            tlpRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpRoot.Controls.Add(pnlHeader, 0, 0);
            tlpRoot.Controls.Add(_tabs, 0, 1);
            tlpRoot.Controls.Add(pnlStatus, 0, 2);
            tlpRoot.Dock = DockStyle.Fill;
            tlpRoot.Location = new Point(0, 0);
            tlpRoot.Margin = new Padding(0);
            tlpRoot.Name = "tlpRoot";
            tlpRoot.RowCount = 3;
            tlpRoot.RowStyles.Add(new RowStyle());
            tlpRoot.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tlpRoot.Size = new Size(1284, 821);
            tlpRoot.TabIndex = 0;
            // 
            // pnlHeader
            // 
            pnlHeader.AutoSize = true;
            pnlHeader.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            pnlHeader.BackColor = Color.White;
            pnlHeader.ColumnCount = 3;
            pnlHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            pnlHeader.ColumnStyles.Add(new ColumnStyle());
            pnlHeader.ColumnStyles.Add(new ColumnStyle());
            pnlHeader.Controls.Add(tlpTitle, 0, 0);
            pnlHeader.Controls.Add(tlpModel, 1, 0);
            pnlHeader.Controls.Add(_btnSelfCheck, 2, 0);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Margin = new Padding(0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Padding = new Padding(20, 10, 20, 10);
            pnlHeader.RowCount = 1;
            pnlHeader.RowStyles.Add(new RowStyle());
            pnlHeader.Size = new Size(1284, 70);
            pnlHeader.TabIndex = 0;
            pnlHeader.Paint += (this.pnlHeader_Paint);
            // 
            // tlpTitle
            // 
            tlpTitle.AutoSize = true;
            tlpTitle.ColumnCount = 1;
            tlpTitle.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpTitle.Controls.Add(lblTitle, 0, 0);
            tlpTitle.Controls.Add(lblSubtitle, 0, 1);
            tlpTitle.Dock = DockStyle.Fill;
            tlpTitle.Location = new Point(20, 10);
            tlpTitle.Margin = new Padding(0);
            tlpTitle.Name = "tlpTitle";
            tlpTitle.RowCount = 2;
            tlpTitle.RowStyles.Add(new RowStyle());
            tlpTitle.RowStyles.Add(new RowStyle());
            tlpTitle.Size = new Size(759, 50);
            tlpTitle.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Microsoft YaHei UI", 15F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            lblTitle.Location = new Point(0, 0);
            lblTitle.Margin = new Padding(0, 0, 0, 2);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(154, 27);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "YOLO 傻瓜识别";
            // 
            // lblSubtitle
            // 
            lblSubtitle.AutoSize = true;
            lblSubtitle.Font = new Font("Microsoft YaHei UI", 8.5F);
            lblSubtitle.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            lblSubtitle.Location = new Point(0, 31);
            lblSubtitle.Margin = new Padding(0, 2, 0, 2);
            lblSubtitle.Name = "lblSubtitle";
            lblSubtitle.Size = new Size(282, 17);
            lblSubtitle.TabIndex = 1;
            lblSubtitle.Text = "默认值按本机自动验证；需要时再展开“详细设置”。";
            // 
            // tlpModel
            // 
            tlpModel.Anchor = (AnchorStyles)(AnchorStyles.Top) | (AnchorStyles.Right);
            tlpModel.AutoSize = true;
            tlpModel.ColumnCount = 1;
            tlpModel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpModel.Controls.Add(_lblModelStatus, 0, 0);
            tlpModel.Controls.Add(_lblModelSub, 0, 1);
            tlpModel.Location = new Point(779, 12);
            tlpModel.Margin = new Padding(0, 2, 12, 0);
            tlpModel.Name = "tlpModel";
            tlpModel.RowCount = 2;
            tlpModel.RowStyles.Add(new RowStyle());
            tlpModel.RowStyles.Add(new RowStyle());
            tlpModel.Size = new Size(380, 38);
            tlpModel.TabIndex = 1;
            // 
            // _lblModelStatus
            // 
            _lblModelStatus.Anchor = (AnchorStyles)(AnchorStyles.Top) | (AnchorStyles.Right);
            _lblModelStatus.AutoSize = true;
            _lblModelStatus.Font = new Font("Microsoft YaHei UI", 9.5F, FontStyle.Bold);
            _lblModelStatus.ForeColor = Color.FromArgb((int)(byte)190, (int)(byte)118, (int)(byte)18);
            _lblModelStatus.Location = new Point(294, 0);
            _lblModelStatus.Margin = new Padding(0);
            _lblModelStatus.Name = "_lblModelStatus";
            _lblModelStatus.Size = new Size(86, 19);
            _lblModelStatus.TabIndex = 0;
            _lblModelStatus.Text = "● 正在自检…";
            _lblModelStatus.TextAlign = ContentAlignment.MiddleRight;
            // 
            // _lblModelSub
            // 
            _lblModelSub.Anchor = (AnchorStyles)(AnchorStyles.Top) | (AnchorStyles.Right);
            _lblModelSub.AutoEllipsis = true;
            _lblModelSub.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lblModelSub.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblModelSub.Location = new Point(0, 20);
            _lblModelSub.Margin = new Padding(0, 1, 0, 0);
            _lblModelSub.Name = "_lblModelSub";
            _lblModelSub.Size = new Size(380, 18);
            _lblModelSub.TabIndex = 1;
            _lblModelSub.TextAlign = ContentAlignment.MiddleRight;
            // 
            // _btnSelfCheck
            // 
            _btnSelfCheck.Anchor = (AnchorStyles)(AnchorStyles.Top) | (AnchorStyles.Right);
            _btnSelfCheck.AutoSize = true;
            _btnSelfCheck.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnSelfCheck.BackColor = Color.White;
            _btnSelfCheck.Cursor = Cursors.Hand;
            _btnSelfCheck.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnSelfCheck.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnSelfCheck.FlatStyle = FlatStyle.Flat;
            _btnSelfCheck.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnSelfCheck.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnSelfCheck.Location = new Point(1171, 15);
            _btnSelfCheck.Margin = new Padding(0, 5, 0, 0);
            _btnSelfCheck.MinimumSize = new Size(84, 34);
            _btnSelfCheck.Name = "_btnSelfCheck";
            _btnSelfCheck.Padding = new Padding(10, 0, 10, 0);
            _btnSelfCheck.Size = new Size(93, 34);
            _btnSelfCheck.TabIndex = 2;
            _btnSelfCheck.Text = "重新自检";
            _btnSelfCheck.UseVisualStyleBackColor = false;
            // 
            // _tabs
            // 
            _tabs.Controls.Add(_pageRec);
            _tabs.Controls.Add(_pageLearn);
            _tabs.Controls.Add(_pageTools);
            _tabs.Controls.Add(_pageFixture);
            _tabs.Controls.Add(_pageAnchor);
            _tabs.Controls.Add(_pageMultiCam);
            _tabs.Dock = DockStyle.Fill;
            _tabs.Font = new Font("Microsoft YaHei UI", 9.5F);
            _tabs.Location = new Point(10, 78);
            _tabs.Margin = new Padding(10, 8, 10, 6);
            _tabs.Name = "_tabs";
            _tabs.Padding = new Point(14, 6);
            _tabs.SelectedIndex = 0;
            _tabs.Size = new Size(1264, 707);
            _tabs.TabIndex = 1;
            // 
            // _pageRec
            // 
            _pageRec.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _pageRec.Controls.Add(tlpCap);
            _pageRec.Location = new Point(4, 34);
            _pageRec.Name = "_pageRec";
            _pageRec.Padding = new Padding(12, 10, 12, 10);
            _pageRec.Size = new Size(1256, 669);
            _pageRec.TabIndex = 0;
            _pageRec.Text = "识别测试";
            // 
            // tlpCap
            // 
            tlpCap.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            tlpCap.ColumnCount = 1;
            tlpCap.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpCap.Controls.Add(tlpCapBody, 0, 3);
            tlpCap.Controls.Add(tlpCapBar, 0, 0);
            tlpCap.Controls.Add(tlpVideoOpt, 0, 1);
            tlpCap.Controls.Add(_pnlAdv, 0, 2);
            tlpCap.Dock = DockStyle.Fill;
            tlpCap.Location = new Point(12, 10);
            tlpCap.Name = "tlpCap";
            tlpCap.Padding = new Padding(2);
            tlpCap.RowCount = 4;
            tlpCap.RowStyles.Add(new RowStyle());
            tlpCap.RowStyles.Add(new RowStyle());
            tlpCap.RowStyles.Add(new RowStyle());
            tlpCap.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpCap.Size = new Size(1232, 649);
            tlpCap.TabIndex = 0;
            // 
            // tlpCapBody
            // 
            tlpCapBody.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            tlpCapBody.ColumnCount = 1;
            tlpCapBody.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpCapBody.Dock = DockStyle.Fill;
            tlpCapBody.Location = new Point(2, 363);
            tlpCapBody.Margin = new Padding(0);
            tlpCapBody.Name = "tlpCapBody";
            tlpCapBody.RowCount = 1;
            tlpCapBody.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpCapBody.Size = new Size(1228, 284);
            tlpCapBody.TabIndex = 3;
            // 
            // tlpCapBar
            // 
            tlpCapBar.AutoSize = true;
            tlpCapBar.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            tlpCapBar.ColumnCount = 11;
            tlpCapBar.ColumnStyles.Add(new ColumnStyle());
            tlpCapBar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpCapBar.ColumnStyles.Add(new ColumnStyle());
            tlpCapBar.ColumnStyles.Add(new ColumnStyle());
            tlpCapBar.ColumnStyles.Add(new ColumnStyle());
            tlpCapBar.ColumnStyles.Add(new ColumnStyle());
            tlpCapBar.ColumnStyles.Add(new ColumnStyle());
            tlpCapBar.ColumnStyles.Add(new ColumnStyle());
            tlpCapBar.ColumnStyles.Add(new ColumnStyle());
            tlpCapBar.ColumnStyles.Add(new ColumnStyle());
            tlpCapBar.ColumnStyles.Add(new ColumnStyle());
            tlpCapBar.Controls.Add(lblCapSource, 0, 0);
            tlpCapBar.Controls.Add(_tbRecImage, 1, 0);
            tlpCapBar.Controls.Add(_btnSelectImage, 2, 0);
            tlpCapBar.Controls.Add(_btnCamera, 3, 0);
            tlpCapBar.Controls.Add(_btnPickVideo, 4, 0);
            tlpCapBar.Controls.Add(_btnSnapshot, 5, 0);
            tlpCapBar.Controls.Add(_btnRecord, 6, 0);
            tlpCapBar.Controls.Add(_btnStartVideo, 7, 0);
            tlpCapBar.Controls.Add(_btnStopVideo, 8, 0);
            tlpCapBar.Controls.Add(_btnClear, 9, 0);
            tlpCapBar.Controls.Add(_btnToggleAdv, 10, 0);
            tlpCapBar.Dock = DockStyle.Top;
            tlpCapBar.Location = new Point(2, 2);
            tlpCapBar.Margin = new Padding(0, 0, 0, 4);
            tlpCapBar.Name = "tlpCapBar";
            tlpCapBar.RowCount = 1;
            tlpCapBar.RowStyles.Add(new RowStyle());
            tlpCapBar.Size = new Size(1228, 41);
            tlpCapBar.TabIndex = 4;
            // 
            // lblCapSource
            // 
            lblCapSource.Anchor = AnchorStyles.Left;
            lblCapSource.AutoSize = true;
            lblCapSource.Font = new Font("Microsoft YaHei UI", 9.5F);
            lblCapSource.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            lblCapSource.Location = new Point(0, 11);
            lblCapSource.Margin = new Padding(0, 7, 8, 6);
            lblCapSource.Name = "lblCapSource";
            lblCapSource.Size = new Size(35, 19);
            lblCapSource.TabIndex = 0;
            lblCapSource.Text = "来源";
            // 
            // _tbRecImage
            // 
            _tbRecImage.BackColor = Color.White;
            _tbRecImage.Dock = DockStyle.Fill;
            _tbRecImage.Font = new Font("Microsoft YaHei UI", 9.5F);
            _tbRecImage.Location = new Point(43, 4);
            _tbRecImage.Margin = new Padding(0, 4, 8, 4);
            _tbRecImage.Name = "_tbRecImage";
            _tbRecImage.PlaceholderText = "图片路径 / 摄像头 0 / 视频文件";
            _tbRecImage.ReadOnly = true;
            _tbRecImage.Size = new Size(377, 24);
            _tbRecImage.TabIndex = 1;
            // 
            // _btnSelectImage
            // 
            _btnSelectImage.BackColor = Color.White;
            _btnSelectImage.Cursor = Cursors.Hand;
            _btnSelectImage.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnSelectImage.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnSelectImage.FlatStyle = FlatStyle.Flat;
            _btnSelectImage.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnSelectImage.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnSelectImage.Location = new Point(428, 3);
            _btnSelectImage.Margin = new Padding(0, 3, 4, 3);
            _btnSelectImage.MinimumSize = new Size(92, 34);
            _btnSelectImage.Name = "_btnSelectImage";
            _btnSelectImage.Padding = new Padding(10, 0, 10, 0);
            _btnSelectImage.Size = new Size(92, 34);
            _btnSelectImage.TabIndex = 2;
            _btnSelectImage.Text = "打开图片";
            _btnSelectImage.UseVisualStyleBackColor = false;
            // 
            // _btnCamera
            // 
            _btnCamera.BackColor = Color.White;
            _btnCamera.Cursor = Cursors.Hand;
            _btnCamera.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnCamera.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnCamera.FlatStyle = FlatStyle.Flat;
            _btnCamera.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnCamera.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnCamera.Location = new Point(524, 3);
            _btnCamera.Margin = new Padding(0, 3, 4, 3);
            _btnCamera.MinimumSize = new Size(78, 34);
            _btnCamera.Name = "_btnCamera";
            _btnCamera.Padding = new Padding(10, 0, 10, 0);
            _btnCamera.Size = new Size(78, 34);
            _btnCamera.TabIndex = 3;
            _btnCamera.Text = "摄像头";
            _btnCamera.UseVisualStyleBackColor = false;
            // 
            // _btnPickVideo
            // 
            _btnPickVideo.BackColor = Color.White;
            _btnPickVideo.Cursor = Cursors.Hand;
            _btnPickVideo.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnPickVideo.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnPickVideo.FlatStyle = FlatStyle.Flat;
            _btnPickVideo.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnPickVideo.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnPickVideo.Location = new Point(606, 3);
            _btnPickVideo.Margin = new Padding(0, 3, 4, 3);
            _btnPickVideo.MinimumSize = new Size(92, 34);
            _btnPickVideo.Name = "_btnPickVideo";
            _btnPickVideo.Padding = new Padding(10, 0, 10, 0);
            _btnPickVideo.Size = new Size(92, 34);
            _btnPickVideo.TabIndex = 4;
            _btnPickVideo.Text = "选择视频";
            _btnPickVideo.UseVisualStyleBackColor = false;
            // 
            // _btnSnapshot
            // 
            _btnSnapshot.BackColor = Color.White;
            _btnSnapshot.Cursor = Cursors.Hand;
            _btnSnapshot.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnSnapshot.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnSnapshot.FlatStyle = FlatStyle.Flat;
            _btnSnapshot.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnSnapshot.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnSnapshot.Location = new Point(702, 3);
            _btnSnapshot.Margin = new Padding(0, 3, 4, 3);
            _btnSnapshot.MinimumSize = new Size(68, 34);
            _btnSnapshot.Name = "_btnSnapshot";
            _btnSnapshot.Padding = new Padding(10, 0, 10, 0);
            _btnSnapshot.Size = new Size(68, 34);
            _btnSnapshot.TabIndex = 5;
            _btnSnapshot.Text = "拍照";
            _btnSnapshot.UseVisualStyleBackColor = false;
            // 
            // _btnRecord
            // 
            _btnRecord.BackColor = Color.White;
            _btnRecord.Cursor = Cursors.Hand;
            _btnRecord.Enabled = false;
            _btnRecord.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnRecord.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnRecord.FlatStyle = FlatStyle.Flat;
            _btnRecord.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnRecord.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnRecord.Location = new Point(774, 3);
            _btnRecord.Margin = new Padding(0, 3, 4, 3);
            _btnRecord.MinimumSize = new Size(92, 34);
            _btnRecord.Name = "_btnRecord";
            _btnRecord.Padding = new Padding(10, 0, 10, 0);
            _btnRecord.Size = new Size(92, 34);
            _btnRecord.TabIndex = 6;
            _btnRecord.Text = "● 录制";
            _btnRecord.UseVisualStyleBackColor = false;
            // 
            // _btnStartVideo
            // 
            _btnStartVideo.BackColor = Color.FromArgb((int)(byte)36, (int)(byte)112, (int)(byte)210);
            _btnStartVideo.Cursor = Cursors.Hand;
            _btnStartVideo.FlatAppearance.BorderSize = 0;
            _btnStartVideo.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)28, (int)(byte)96, (int)(byte)186);
            _btnStartVideo.FlatStyle = FlatStyle.Flat;
            _btnStartVideo.Font = new Font("Microsoft YaHei UI", 9.5F, FontStyle.Bold);
            _btnStartVideo.ForeColor = Color.White;
            _btnStartVideo.Location = new Point(870, 2);
            _btnStartVideo.Margin = new Padding(0, 2, 4, 3);
            _btnStartVideo.MinimumSize = new Size(104, 36);
            _btnStartVideo.Name = "_btnStartVideo";
            _btnStartVideo.Padding = new Padding(10, 0, 10, 0);
            _btnStartVideo.Size = new Size(104, 36);
            _btnStartVideo.TabIndex = 7;
            _btnStartVideo.Text = "开始识别";
            _btnStartVideo.UseVisualStyleBackColor = false;
            // 
            // _btnStopVideo
            // 
            _btnStopVideo.BackColor = Color.White;
            _btnStopVideo.Cursor = Cursors.Hand;
            _btnStopVideo.Enabled = false;
            _btnStopVideo.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnStopVideo.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnStopVideo.FlatStyle = FlatStyle.Flat;
            _btnStopVideo.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnStopVideo.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnStopVideo.Location = new Point(978, 2);
            _btnStopVideo.Margin = new Padding(0, 2, 4, 3);
            _btnStopVideo.MinimumSize = new Size(68, 34);
            _btnStopVideo.Name = "_btnStopVideo";
            _btnStopVideo.Padding = new Padding(10, 0, 10, 0);
            _btnStopVideo.Size = new Size(68, 34);
            _btnStopVideo.TabIndex = 8;
            _btnStopVideo.Text = "停止";
            _btnStopVideo.UseVisualStyleBackColor = false;
            // 
            // _btnClear
            // 
            _btnClear.BackColor = Color.White;
            _btnClear.Cursor = Cursors.Hand;
            _btnClear.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnClear.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnClear.FlatStyle = FlatStyle.Flat;
            _btnClear.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnClear.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnClear.Location = new Point(1050, 2);
            _btnClear.Margin = new Padding(0, 2, 4, 3);
            _btnClear.MinimumSize = new Size(62, 34);
            _btnClear.Name = "_btnClear";
            _btnClear.Padding = new Padding(10, 0, 10, 0);
            _btnClear.Size = new Size(62, 34);
            _btnClear.TabIndex = 9;
            _btnClear.Text = "清空";
            _btnClear.UseVisualStyleBackColor = false;
            // 
            // _btnToggleAdv
            // 
            _btnToggleAdv.BackColor = Color.White;
            _btnToggleAdv.Cursor = Cursors.Hand;
            _btnToggleAdv.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnToggleAdv.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnToggleAdv.FlatStyle = FlatStyle.Flat;
            _btnToggleAdv.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnToggleAdv.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnToggleAdv.Location = new Point(1116, 2);
            _btnToggleAdv.Margin = new Padding(0, 2, 0, 3);
            _btnToggleAdv.MinimumSize = new Size(112, 34);
            _btnToggleAdv.Name = "_btnToggleAdv";
            _btnToggleAdv.Padding = new Padding(10, 0, 10, 0);
            _btnToggleAdv.Size = new Size(112, 34);
            _btnToggleAdv.TabIndex = 10;
            _btnToggleAdv.Text = "▸ 详细设置";
            _btnToggleAdv.UseVisualStyleBackColor = false;
            // 
            // tlpVideoOpt
            // 
            tlpVideoOpt.AutoSize = true;
            tlpVideoOpt.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tlpVideoOpt.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            tlpVideoOpt.ColumnCount = 1;
            tlpVideoOpt.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpVideoOpt.Controls.Add(flowVideo1, 0, 0);
            tlpVideoOpt.Controls.Add(flowVideo2, 0, 1);
            tlpVideoOpt.Dock = DockStyle.Top;
            tlpVideoOpt.Location = new Point(2, 47);
            tlpVideoOpt.Margin = new Padding(0, 0, 0, 6);
            tlpVideoOpt.Name = "tlpVideoOpt";
            tlpVideoOpt.RowCount = 2;
            tlpVideoOpt.RowStyles.Add(new RowStyle());
            tlpVideoOpt.RowStyles.Add(new RowStyle());
            tlpVideoOpt.Size = new Size(1228, 80);
            tlpVideoOpt.TabIndex = 5;
            // 
            // flowVideo1
            // 
            flowVideo1.AutoSize = true;
            flowVideo1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flowVideo1.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            flowVideo1.Controls.Add(_chkVideoLoop);
            flowVideo1.Controls.Add(lblVideoStep);
            flowVideo1.Controls.Add(_numVideoStep);
            flowVideo1.Controls.Add(_btnDrawLine);
            flowVideo1.Controls.Add(_btnClearLine);
            flowVideo1.Controls.Add(_lblVideoLine);
            flowVideo1.Dock = DockStyle.Fill;
            flowVideo1.Location = new Point(0, 0);
            flowVideo1.Margin = new Padding(0);
            flowVideo1.Name = "flowVideo1";
            flowVideo1.Padding = new Padding(0, 0, 0, 2);
            flowVideo1.Size = new Size(1228, 38);
            flowVideo1.TabIndex = 0;
            flowVideo1.WrapContents = false;
            // 
            // _chkVideoLoop
            // 
            _chkVideoLoop.AutoSize = true;
            _chkVideoLoop.Font = new Font("Microsoft YaHei UI", 9.5F);
            _chkVideoLoop.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _chkVideoLoop.Location = new Point(2, 8);
            _chkVideoLoop.Margin = new Padding(2, 8, 12, 4);
            _chkVideoLoop.Name = "_chkVideoLoop";
            _chkVideoLoop.Size = new Size(80, 23);
            _chkVideoLoop.TabIndex = 0;
            _chkVideoLoop.Text = "循环播放";
            // 
            // lblVideoStep
            // 
            lblVideoStep.Anchor = AnchorStyles.Left;
            lblVideoStep.AutoSize = true;
            lblVideoStep.Font = new Font("Microsoft YaHei UI", 9.5F);
            lblVideoStep.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            lblVideoStep.Location = new Point(94, 9);
            lblVideoStep.Margin = new Padding(0, 7, 8, 6);
            lblVideoStep.Name = "lblVideoStep";
            lblVideoStep.Size = new Size(61, 19);
            lblVideoStep.TabIndex = 1;
            lblVideoStep.Text = "跳帧间隔";
            // 
            // _numVideoStep
            // 
            _numVideoStep.Font = new Font("Microsoft YaHei UI", 9.5F);
            _numVideoStep.Location = new Point(163, 5);
            _numVideoStep.Margin = new Padding(0, 5, 14, 4);
            _numVideoStep.Maximum = new decimal(new int[] { 12, 0, 0, 0 });
            _numVideoStep.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            _numVideoStep.Name = "_numVideoStep";
            _numVideoStep.Size = new Size(54, 24);
            _numVideoStep.TabIndex = 2;
            _numVideoStep.TextAlign = HorizontalAlignment.Right;
            _numVideoStep.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // _btnDrawLine
            // 
            _btnDrawLine.BackColor = Color.White;
            _btnDrawLine.Cursor = Cursors.Hand;
            _btnDrawLine.Enabled = false;
            _btnDrawLine.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnDrawLine.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnDrawLine.FlatStyle = FlatStyle.Flat;
            _btnDrawLine.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnDrawLine.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnDrawLine.Location = new Point(231, 2);
            _btnDrawLine.Margin = new Padding(0, 2, 4, 2);
            _btnDrawLine.MinimumSize = new Size(92, 32);
            _btnDrawLine.Name = "_btnDrawLine";
            _btnDrawLine.Padding = new Padding(10, 0, 10, 0);
            _btnDrawLine.Size = new Size(92, 32);
            _btnDrawLine.TabIndex = 3;
            _btnDrawLine.Text = "画计数线";
            _btnDrawLine.UseVisualStyleBackColor = false;
            // 
            // _btnClearLine
            // 
            _btnClearLine.BackColor = Color.White;
            _btnClearLine.Cursor = Cursors.Hand;
            _btnClearLine.Enabled = false;
            _btnClearLine.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnClearLine.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnClearLine.FlatStyle = FlatStyle.Flat;
            _btnClearLine.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnClearLine.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnClearLine.Location = new Point(327, 2);
            _btnClearLine.Margin = new Padding(0, 2, 12, 2);
            _btnClearLine.MinimumSize = new Size(76, 32);
            _btnClearLine.Name = "_btnClearLine";
            _btnClearLine.Padding = new Padding(10, 0, 10, 0);
            _btnClearLine.Size = new Size(76, 32);
            _btnClearLine.TabIndex = 4;
            _btnClearLine.Text = "清除线";
            _btnClearLine.UseVisualStyleBackColor = false;
            // 
            // _lblVideoLine
            // 
            _lblVideoLine.AutoSize = true;
            _lblVideoLine.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lblVideoLine.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblVideoLine.Location = new Point(415, 11);
            _lblVideoLine.Margin = new Padding(0, 11, 12, 4);
            _lblVideoLine.Name = "_lblVideoLine";
            _lblVideoLine.Size = new Size(128, 17);
            _lblVideoLine.TabIndex = 5;
            _lblVideoLine.Text = "计数线：默认画面中线";
            // 
            // flowVideo2
            // 
            flowVideo2.AutoSize = true;
            flowVideo2.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flowVideo2.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            flowVideo2.Controls.Add(lblVideoTask);
            flowVideo2.Controls.Add(_cboVideoTask);
            flowVideo2.Controls.Add(lblVideoConf);
            flowVideo2.Controls.Add(_numVideoConf);
            flowVideo2.Controls.Add(lblVideoClasses);
            flowVideo2.Controls.Add(_tbVideoClasses);
            flowVideo2.Controls.Add(_chkVideoBest);
            flowVideo2.Controls.Add(_btnVideoModel);
            flowVideo2.Controls.Add(_lblVideoModel);
            flowVideo2.Controls.Add(_lblVideoInfo);
            flowVideo2.Dock = DockStyle.Fill;
            flowVideo2.Location = new Point(0, 38);
            flowVideo2.Margin = new Padding(0);
            flowVideo2.Name = "flowVideo2";
            flowVideo2.Size = new Size(1228, 42);
            flowVideo2.TabIndex = 1;
            flowVideo2.WrapContents = false;
            // 
            // lblVideoTask
            // 
            lblVideoTask.Anchor = AnchorStyles.Left;
            lblVideoTask.AutoSize = true;
            lblVideoTask.Font = new Font("Microsoft YaHei UI", 9.5F);
            lblVideoTask.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            lblVideoTask.Location = new Point(0, 12);
            lblVideoTask.Margin = new Padding(0, 7, 8, 6);
            lblVideoTask.Name = "lblVideoTask";
            lblVideoTask.Size = new Size(61, 19);
            lblVideoTask.TabIndex = 0;
            lblVideoTask.Text = "模型类型";
            // 
            // _cboVideoTask
            // 
            _cboVideoTask.DropDownStyle = ComboBoxStyle.DropDownList;
            _cboVideoTask.Font = new Font("Microsoft YaHei UI", 9.5F);
            _cboVideoTask.Items.AddRange(new object[] { "分割（掩码）", "检测（方框）" });
            _cboVideoTask.Location = new Point(69, 6);
            _cboVideoTask.Margin = new Padding(0, 6, 12, 4);
            _cboVideoTask.Name = "_cboVideoTask";
            _cboVideoTask.Size = new Size(96, 27);
            _cboVideoTask.TabIndex = 1;
            // 
            // lblVideoConf
            // 
            lblVideoConf.Anchor = AnchorStyles.Left;
            lblVideoConf.AutoSize = true;
            lblVideoConf.Font = new Font("Microsoft YaHei UI", 9.5F);
            lblVideoConf.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            lblVideoConf.Location = new Point(177, 12);
            lblVideoConf.Margin = new Padding(0, 7, 8, 6);
            lblVideoConf.Name = "lblVideoConf";
            lblVideoConf.Size = new Size(48, 19);
            lblVideoConf.TabIndex = 2;
            lblVideoConf.Text = "置信度";
            // 
            // _numVideoConf
            // 
            _numVideoConf.DecimalPlaces = 2;
            _numVideoConf.Font = new Font("Microsoft YaHei UI", 9.5F);
            _numVideoConf.Increment = new decimal(new int[] { 5, 0, 0, 131072 });
            _numVideoConf.Location = new Point(233, 6);
            _numVideoConf.Margin = new Padding(0, 6, 12, 4);
            _numVideoConf.Maximum = new decimal(new int[] { 95, 0, 0, 131072 });
            _numVideoConf.Minimum = new decimal(new int[] { 5, 0, 0, 131072 });
            _numVideoConf.Name = "_numVideoConf";
            _numVideoConf.Size = new Size(62, 24);
            _numVideoConf.TabIndex = 3;
            _numVideoConf.TextAlign = HorizontalAlignment.Right;
            _numVideoConf.Value = new decimal(new int[] { 5, 0, 0, 65536 });
            // 
            // lblVideoClasses
            // 
            lblVideoClasses.Anchor = AnchorStyles.Left;
            lblVideoClasses.AutoSize = true;
            lblVideoClasses.Font = new Font("Microsoft YaHei UI", 9.5F);
            lblVideoClasses.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            lblVideoClasses.Location = new Point(307, 12);
            lblVideoClasses.Margin = new Padding(0, 7, 8, 6);
            lblVideoClasses.Name = "lblVideoClasses";
            lblVideoClasses.Size = new Size(35, 19);
            lblVideoClasses.TabIndex = 4;
            lblVideoClasses.Text = "类别";
            // 
            // _tbVideoClasses
            // 
            _tbVideoClasses.Font = new Font("Microsoft YaHei UI", 9.5F);
            _tbVideoClasses.Location = new Point(350, 7);
            _tbVideoClasses.Margin = new Padding(0, 7, 12, 4);
            _tbVideoClasses.Name = "_tbVideoClasses";
            _tbVideoClasses.PlaceholderText = "留空=全部";
            _tbVideoClasses.Size = new Size(128, 24);
            _tbVideoClasses.TabIndex = 5;
            // 
            // _chkVideoBest
            // 
            _chkVideoBest.AutoSize = true;
            _chkVideoBest.Font = new Font("Microsoft YaHei UI", 9.5F);
            _chkVideoBest.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _chkVideoBest.Location = new Point(490, 9);
            _chkVideoBest.Margin = new Padding(0, 9, 12, 4);
            _chkVideoBest.Name = "_chkVideoBest";
            _chkVideoBest.Size = new Size(93, 23);
            _chkVideoBest.TabIndex = 6;
            _chkVideoBest.Text = "只留最高分";
            // 
            // _btnVideoModel
            // 
            _btnVideoModel.AutoSize = true;
            _btnVideoModel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnVideoModel.BackColor = Color.Transparent;
            _btnVideoModel.Cursor = Cursors.Hand;
            _btnVideoModel.FlatAppearance.BorderSize = 0;
            _btnVideoModel.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)232, (int)(byte)240, (int)(byte)252);
            _btnVideoModel.FlatStyle = FlatStyle.Flat;
            _btnVideoModel.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnVideoModel.ForeColor = Color.FromArgb((int)(byte)36, (int)(byte)112, (int)(byte)210);
            _btnVideoModel.Location = new Point(595, 6);
            _btnVideoModel.Margin = new Padding(0, 6, 4, 2);
            _btnVideoModel.MinimumSize = new Size(84, 34);
            _btnVideoModel.Name = "_btnVideoModel";
            _btnVideoModel.Padding = new Padding(10, 0, 10, 0);
            _btnVideoModel.Size = new Size(91, 34);
            _btnVideoModel.TabIndex = 7;
            _btnVideoModel.Text = "更换模型";
            _btnVideoModel.UseVisualStyleBackColor = false;
            // 
            // _lblVideoModel
            // 
            _lblVideoModel.AutoSize = true;
            _lblVideoModel.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lblVideoModel.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblVideoModel.Location = new Point(690, 12);
            _lblVideoModel.Margin = new Padding(0, 12, 12, 4);
            _lblVideoModel.Name = "_lblVideoModel";
            _lblVideoModel.Size = new Size(68, 17);
            _lblVideoModel.TabIndex = 8;
            _lblVideoModel.Text = "分割模型：";
            // 
            // _lblVideoInfo
            // 
            _lblVideoInfo.AutoEllipsis = true;
            _lblVideoInfo.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lblVideoInfo.ForeColor = Color.FromArgb((int)(byte)36, (int)(byte)112, (int)(byte)210);
            _lblVideoInfo.Location = new Point(770, 12);
            _lblVideoInfo.Margin = new Padding(0, 12, 0, 4);
            _lblVideoInfo.Name = "_lblVideoInfo";
            _lblVideoInfo.Size = new Size(210, 23);
            _lblVideoInfo.TabIndex = 9;
            _lblVideoInfo.Text = "就绪";
            // 
            // _pnlAdv
            // 
            _pnlAdv.AutoSize = true;
            _pnlAdv.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _pnlAdv.BackColor = Color.White;
            _pnlAdv.Controls.Add(tlpAdvGrid);
            _pnlAdv.Dock = DockStyle.Top;
            _pnlAdv.Location = new Point(2, 133);
            _pnlAdv.Margin = new Padding(0, 0, 0, 8);
            _pnlAdv.Name = "_pnlAdv";
            _pnlAdv.Padding = new Padding(14, 12, 14, 12);
            _pnlAdv.Size = new Size(1228, 222);
            _pnlAdv.TabIndex = 6;
            _pnlAdv.Paint += (this.pnlAdv_Paint);
            // 
            // tlpAdvGrid
            // 
            tlpAdvGrid.AutoSize = true;
            tlpAdvGrid.BackColor = Color.White;
            tlpAdvGrid.ColumnCount = 4;
            tlpAdvGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 82F));
            tlpAdvGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpAdvGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 104F));
            tlpAdvGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpAdvGrid.Controls.Add(lblAdvModel, 0, 0);
            tlpAdvGrid.Controls.Add(tlpAdvPick, 1, 0);
            tlpAdvGrid.Controls.Add(lblAdvKind, 0, 1);
            tlpAdvGrid.Controls.Add(_cboKind, 1, 1);
            tlpAdvGrid.Controls.Add(lblAdvDevice, 2, 1);
            tlpAdvGrid.Controls.Add(_cboDevice, 3, 1);
            tlpAdvGrid.Controls.Add(lblAdvProvider, 0, 2);
            tlpAdvGrid.Controls.Add(_cboProvider, 1, 2);
            tlpAdvGrid.Controls.Add(lblAdvThr, 2, 2);
            tlpAdvGrid.Controls.Add(flowAdvThr, 3, 2);
            tlpAdvGrid.Controls.Add(_lblAdvInfo, 0, 3);
            tlpAdvGrid.Controls.Add(tlpAdvFooter, 0, 4);
            tlpAdvGrid.Controls.Add(_lblAdvProbe, 0, 5);
            tlpAdvGrid.Dock = DockStyle.Top;
            tlpAdvGrid.Location = new Point(14, 12);
            tlpAdvGrid.Margin = new Padding(0);
            tlpAdvGrid.Name = "tlpAdvGrid";
            tlpAdvGrid.RowCount = 6;
            tlpAdvGrid.RowStyles.Add(new RowStyle());
            tlpAdvGrid.RowStyles.Add(new RowStyle());
            tlpAdvGrid.RowStyles.Add(new RowStyle());
            tlpAdvGrid.RowStyles.Add(new RowStyle());
            tlpAdvGrid.RowStyles.Add(new RowStyle());
            tlpAdvGrid.RowStyles.Add(new RowStyle());
            tlpAdvGrid.Size = new Size(1200, 198);
            tlpAdvGrid.TabIndex = 0;
            // 
            // lblAdvModel
            // 
            lblAdvModel.Anchor = AnchorStyles.Left;
            lblAdvModel.AutoSize = true;
            lblAdvModel.Font = new Font("Microsoft YaHei UI", 9.5F);
            lblAdvModel.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            lblAdvModel.Location = new Point(0, 9);
            lblAdvModel.Margin = new Padding(0, 7, 8, 6);
            lblAdvModel.Name = "lblAdvModel";
            lblAdvModel.Size = new Size(61, 19);
            lblAdvModel.TabIndex = 0;
            lblAdvModel.Text = "模型文件";
            // 
            // tlpAdvPick
            // 
            tlpAdvPick.AutoSize = true;
            tlpAdvPick.ColumnCount = 2;
            tlpAdvGrid.SetColumnSpan(tlpAdvPick, 3);
            tlpAdvPick.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpAdvPick.ColumnStyles.Add(new ColumnStyle());
            tlpAdvPick.Controls.Add(_tbAdvModel, 0, 0);
            tlpAdvPick.Controls.Add(_btnAdvModel, 1, 0);
            tlpAdvPick.Dock = DockStyle.Fill;
            tlpAdvPick.Location = new Point(82, 3);
            tlpAdvPick.Margin = new Padding(0, 3, 0, 3);
            tlpAdvPick.Name = "tlpAdvPick";
            tlpAdvPick.RowCount = 1;
            tlpAdvPick.RowStyles.Add(new RowStyle());
            tlpAdvPick.Size = new Size(1118, 30);
            tlpAdvPick.TabIndex = 1;
            // 
            // _tbAdvModel
            // 
            _tbAdvModel.Dock = DockStyle.Fill;
            _tbAdvModel.Font = new Font("Microsoft YaHei UI", 9.5F);
            _tbAdvModel.Location = new Point(0, 2);
            _tbAdvModel.Margin = new Padding(0, 2, 6, 2);
            _tbAdvModel.Name = "_tbAdvModel";
            _tbAdvModel.Size = new Size(1028, 24);
            _tbAdvModel.TabIndex = 0;
            // 
            // _btnAdvModel
            // 
            _btnAdvModel.BackColor = Color.White;
            _btnAdvModel.Cursor = Cursors.Hand;
            _btnAdvModel.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnAdvModel.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnAdvModel.FlatStyle = FlatStyle.Flat;
            _btnAdvModel.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnAdvModel.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnAdvModel.Location = new Point(1034, 0);
            _btnAdvModel.Margin = new Padding(0);
            _btnAdvModel.MinimumSize = new Size(84, 30);
            _btnAdvModel.Name = "_btnAdvModel";
            _btnAdvModel.Padding = new Padding(10, 0, 10, 0);
            _btnAdvModel.Size = new Size(84, 30);
            _btnAdvModel.TabIndex = 1;
            _btnAdvModel.Text = "选择…";
            _btnAdvModel.UseVisualStyleBackColor = false;
            // 
            // lblAdvKind
            // 
            lblAdvKind.Anchor = AnchorStyles.Left;
            lblAdvKind.AutoSize = true;
            lblAdvKind.Font = new Font("Microsoft YaHei UI", 9.5F);
            lblAdvKind.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            lblAdvKind.Location = new Point(0, 44);
            lblAdvKind.Margin = new Padding(0, 7, 8, 6);
            lblAdvKind.Name = "lblAdvKind";
            lblAdvKind.Size = new Size(61, 19);
            lblAdvKind.TabIndex = 2;
            lblAdvKind.Text = "模型类型";
            // 
            // _cboKind
            // 
            _cboKind.Dock = DockStyle.Fill;
            _cboKind.DropDownStyle = ComboBoxStyle.DropDownList;
            _cboKind.Font = new Font("Microsoft YaHei UI", 9.5F);
            _cboKind.Items.AddRange(new object[] { "YOLOv8 检测（推荐）", "YOLOv11 检测", "YOLOv8 分割", "YOLO26 检测（端到端）" });
            _cboKind.Location = new Point(82, 40);
            _cboKind.Margin = new Padding(0, 4, 12, 4);
            _cboKind.Name = "_cboKind";
            _cboKind.Size = new Size(495, 27);
            _cboKind.TabIndex = 3;
            // 
            // lblAdvDevice
            // 
            lblAdvDevice.Anchor = AnchorStyles.Left;
            lblAdvDevice.AutoSize = true;
            lblAdvDevice.Font = new Font("Microsoft YaHei UI", 9.5F);
            lblAdvDevice.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            lblAdvDevice.Location = new Point(589, 44);
            lblAdvDevice.Margin = new Padding(0, 7, 8, 6);
            lblAdvDevice.Name = "lblAdvDevice";
            lblAdvDevice.Size = new Size(61, 19);
            lblAdvDevice.TabIndex = 4;
            lblAdvDevice.Text = "推理设备";
            // 
            // _cboDevice
            // 
            _cboDevice.Dock = DockStyle.Fill;
            _cboDevice.DropDownStyle = ComboBoxStyle.DropDownList;
            _cboDevice.Font = new Font("Microsoft YaHei UI", 9.5F);
            _cboDevice.Items.AddRange(new object[] { "CPU", "GPU 0" });
            _cboDevice.Location = new Point(693, 40);
            _cboDevice.Margin = new Padding(0, 4, 12, 4);
            _cboDevice.Name = "_cboDevice";
            _cboDevice.Size = new Size(495, 27);
            _cboDevice.TabIndex = 5;
            // 
            // lblAdvProvider
            // 
            lblAdvProvider.Anchor = AnchorStyles.Left;
            lblAdvProvider.AutoSize = true;
            lblAdvProvider.Font = new Font("Microsoft YaHei UI", 9.5F);
            lblAdvProvider.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            lblAdvProvider.Location = new Point(0, 79);
            lblAdvProvider.Margin = new Padding(0, 7, 8, 6);
            lblAdvProvider.Name = "lblAdvProvider";
            lblAdvProvider.Size = new Size(61, 19);
            lblAdvProvider.TabIndex = 6;
            lblAdvProvider.Text = "加速后端";
            // 
            // _cboProvider
            // 
            _cboProvider.Dock = DockStyle.Fill;
            _cboProvider.DropDownStyle = ComboBoxStyle.DropDownList;
            _cboProvider.Font = new Font("Microsoft YaHei UI", 9.5F);
            _cboProvider.Items.AddRange(new object[] { "CPU（本机已验证）", "DirectML（GPU）" });
            _cboProvider.Location = new Point(82, 75);
            _cboProvider.Margin = new Padding(0, 4, 12, 4);
            _cboProvider.Name = "_cboProvider";
            _cboProvider.Size = new Size(495, 27);
            _cboProvider.TabIndex = 7;
            // 
            // lblAdvThr
            // 
            lblAdvThr.Anchor = AnchorStyles.Left;
            lblAdvThr.AutoSize = true;
            lblAdvThr.Font = new Font("Microsoft YaHei UI", 9.5F);
            lblAdvThr.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            lblAdvThr.Location = new Point(589, 79);
            lblAdvThr.Margin = new Padding(0, 7, 8, 6);
            lblAdvThr.Name = "lblAdvThr";
            lblAdvThr.Size = new Size(94, 19);
            lblAdvThr.TabIndex = 8;
            lblAdvThr.Text = "置信度 / NMS";
            // 
            // flowAdvThr
            // 
            flowAdvThr.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            flowAdvThr.Controls.Add(_numConf);
            flowAdvThr.Controls.Add(lblAdvNms);
            flowAdvThr.Controls.Add(_numNms);
            flowAdvThr.Dock = DockStyle.Fill;
            flowAdvThr.Location = new Point(693, 73);
            flowAdvThr.Margin = new Padding(0, 2, 0, 2);
            flowAdvThr.Name = "flowAdvThr";
            flowAdvThr.Size = new Size(507, 31);
            flowAdvThr.TabIndex = 9;
            flowAdvThr.WrapContents = false;
            // 
            // _numConf
            // 
            _numConf.DecimalPlaces = 2;
            _numConf.Font = new Font("Microsoft YaHei UI", 9.5F);
            _numConf.Increment = new decimal(new int[] { 5, 0, 0, 131072 });
            _numConf.Location = new Point(0, 2);
            _numConf.Margin = new Padding(0, 2, 0, 2);
            _numConf.Maximum = new decimal(new int[] { 99, 0, 0, 131072 });
            _numConf.Minimum = new decimal(new int[] { 1, 0, 0, 131072 });
            _numConf.Name = "_numConf";
            _numConf.Size = new Size(64, 24);
            _numConf.TabIndex = 0;
            _numConf.TextAlign = HorizontalAlignment.Right;
            _numConf.Value = new decimal(new int[] { 5, 0, 0, 65536 });
            // 
            // lblAdvNms
            // 
            lblAdvNms.AutoSize = true;
            lblAdvNms.Font = new Font("Microsoft YaHei UI", 8.5F);
            lblAdvNms.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            lblAdvNms.Location = new Point(72, 8);
            lblAdvNms.Margin = new Padding(8, 8, 6, 0);
            lblAdvNms.Name = "lblAdvNms";
            lblAdvNms.Size = new Size(37, 17);
            lblAdvNms.TabIndex = 1;
            lblAdvNms.Text = "NMS";
            // 
            // _numNms
            // 
            _numNms.DecimalPlaces = 2;
            _numNms.Font = new Font("Microsoft YaHei UI", 9.5F);
            _numNms.Increment = new decimal(new int[] { 5, 0, 0, 131072 });
            _numNms.Location = new Point(115, 2);
            _numNms.Margin = new Padding(0, 2, 0, 2);
            _numNms.Maximum = new decimal(new int[] { 99, 0, 0, 131072 });
            _numNms.Minimum = new decimal(new int[] { 1, 0, 0, 131072 });
            _numNms.Name = "_numNms";
            _numNms.Size = new Size(64, 24);
            _numNms.TabIndex = 2;
            _numNms.TextAlign = HorizontalAlignment.Right;
            _numNms.Value = new decimal(new int[] { 5, 0, 0, 65536 });
            // 
            // _lblAdvInfo
            // 
            _lblAdvInfo.AutoSize = true;
            tlpAdvGrid.SetColumnSpan(_lblAdvInfo, 4);
            _lblAdvInfo.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lblAdvInfo.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblAdvInfo.Location = new Point(0, 112);
            _lblAdvInfo.Margin = new Padding(0, 6, 0, 0);
            _lblAdvInfo.Name = "_lblAdvInfo";
            _lblAdvInfo.Size = new Size(104, 17);
            _lblAdvInfo.TabIndex = 10;
            _lblAdvInfo.Text = "当前：未加载模型";
            // 
            // tlpAdvFooter
            // 
            tlpAdvFooter.AutoSize = true;
            tlpAdvFooter.ColumnCount = 2;
            tlpAdvGrid.SetColumnSpan(tlpAdvFooter, 4);
            tlpAdvFooter.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpAdvFooter.ColumnStyles.Add(new ColumnStyle());
            tlpAdvFooter.Controls.Add(lblAdvFooterHint, 0, 0);
            tlpAdvFooter.Controls.Add(flowAdvFooter, 1, 0);
            tlpAdvFooter.Dock = DockStyle.Fill;
            tlpAdvFooter.Location = new Point(0, 135);
            tlpAdvFooter.Margin = new Padding(0, 6, 0, 0);
            tlpAdvFooter.Name = "tlpAdvFooter";
            tlpAdvFooter.RowCount = 1;
            tlpAdvFooter.RowStyles.Add(new RowStyle());
            tlpAdvFooter.Size = new Size(1200, 42);
            tlpAdvFooter.TabIndex = 11;
            // 
            // lblAdvFooterHint
            // 
            lblAdvFooterHint.Dock = DockStyle.Fill;
            lblAdvFooterHint.Font = new Font("Microsoft YaHei UI", 8.5F);
            lblAdvFooterHint.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            lblAdvFooterHint.Location = new Point(3, 0);
            lblAdvFooterHint.Name = "lblAdvFooterHint";
            lblAdvFooterHint.Size = new Size(970, 42);
            lblAdvFooterHint.TabIndex = 0;
            lblAdvFooterHint.Text = "改动会先验证：通过后才保存并切换，失败则保留当前模型。";
            lblAdvFooterHint.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // flowAdvFooter
            // 
            flowAdvFooter.AutoSize = true;
            flowAdvFooter.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flowAdvFooter.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            flowAdvFooter.Controls.Add(_btnVerifySave);
            flowAdvFooter.Controls.Add(_btnResetAdv);
            flowAdvFooter.Dock = DockStyle.Fill;
            flowAdvFooter.FlowDirection = FlowDirection.RightToLeft;
            flowAdvFooter.Location = new Point(976, 0);
            flowAdvFooter.Margin = new Padding(0);
            flowAdvFooter.Name = "flowAdvFooter";
            flowAdvFooter.Size = new Size(224, 42);
            flowAdvFooter.TabIndex = 1;
            flowAdvFooter.WrapContents = false;
            // 
            // _btnVerifySave
            // 
            _btnVerifySave.BackColor = Color.FromArgb((int)(byte)36, (int)(byte)112, (int)(byte)210);
            _btnVerifySave.Cursor = Cursors.Hand;
            _btnVerifySave.FlatAppearance.BorderSize = 0;
            _btnVerifySave.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)28, (int)(byte)96, (int)(byte)186);
            _btnVerifySave.FlatStyle = FlatStyle.Flat;
            _btnVerifySave.Font = new Font("Microsoft YaHei UI", 9.5F, FontStyle.Bold);
            _btnVerifySave.ForeColor = Color.White;
            _btnVerifySave.Location = new Point(108, 0);
            _btnVerifySave.Margin = new Padding(8, 0, 0, 0);
            _btnVerifySave.MinimumSize = new Size(116, 36);
            _btnVerifySave.Name = "_btnVerifySave";
            _btnVerifySave.Padding = new Padding(10, 0, 10, 0);
            _btnVerifySave.Size = new Size(116, 36);
            _btnVerifySave.TabIndex = 0;
            _btnVerifySave.Text = "验证并保存";
            _btnVerifySave.UseVisualStyleBackColor = false;
            // 
            // _btnResetAdv
            // 
            _btnResetAdv.BackColor = Color.White;
            _btnResetAdv.Cursor = Cursors.Hand;
            _btnResetAdv.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnResetAdv.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnResetAdv.FlatStyle = FlatStyle.Flat;
            _btnResetAdv.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnResetAdv.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnResetAdv.Location = new Point(3, 3);
            _btnResetAdv.MinimumSize = new Size(94, 36);
            _btnResetAdv.Name = "_btnResetAdv";
            _btnResetAdv.Padding = new Padding(10, 0, 10, 0);
            _btnResetAdv.Size = new Size(94, 36);
            _btnResetAdv.TabIndex = 1;
            _btnResetAdv.Text = "恢复默认";
            _btnResetAdv.UseVisualStyleBackColor = false;
            // 
            // _lblAdvProbe
            // 
            _lblAdvProbe.AutoSize = true;
            tlpAdvGrid.SetColumnSpan(_lblAdvProbe, 4);
            _lblAdvProbe.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lblAdvProbe.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblAdvProbe.Location = new Point(0, 181);
            _lblAdvProbe.Margin = new Padding(0, 4, 0, 0);
            _lblAdvProbe.Name = "_lblAdvProbe";
            _lblAdvProbe.Size = new Size(0, 17);
            _lblAdvProbe.TabIndex = 12;
            // 
            // _pageLearn
            // 
            _pageLearn.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _pageLearn.Controls.Add(_tlpLearn);
            _pageLearn.Location = new Point(4, 34);
            _pageLearn.Name = "_pageLearn";
            _pageLearn.Padding = new Padding(12, 10, 12, 10);
            _pageLearn.Size = new Size(1256, 669);
            _pageLearn.TabIndex = 1;
            _pageLearn.Text = "学习训练";
            // 
            // _tlpLearn
            // 
            _tlpLearn.AutoSize = true;
            _tlpLearn.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _tlpLearn.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _tlpLearn.ColumnCount = 1;
            _tlpLearn.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            _tlpLearn.Controls.Add(_tlpLearnBar, 0, 0);
            _tlpLearn.Controls.Add(_splitLearn, 0, 1);
            _tlpLearn.Controls.Add(_cardTrain, 0, 2);
            _tlpLearn.Dock = DockStyle.Fill;
            _tlpLearn.Location = new Point(12, 10);
            _tlpLearn.Margin = new Padding(0);
            _tlpLearn.Name = "_tlpLearn";
            _tlpLearn.RowCount = 3;
            _tlpLearn.RowStyles.Add(new RowStyle());
            _tlpLearn.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            _tlpLearn.RowStyles.Add(new RowStyle(SizeType.Absolute, 216F));
            _tlpLearn.Size = new Size(1232, 649);
            _tlpLearn.TabIndex = 0;
            // 
            // _tlpLearnBar
            // 
            _tlpLearnBar.AutoSize = true;
            _tlpLearnBar.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _tlpLearnBar.ColumnCount = 5;
            _tlpLearnBar.ColumnStyles.Add(new ColumnStyle());
            _tlpLearnBar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            _tlpLearnBar.ColumnStyles.Add(new ColumnStyle());
            _tlpLearnBar.ColumnStyles.Add(new ColumnStyle());
            _tlpLearnBar.ColumnStyles.Add(new ColumnStyle());
            _tlpLearnBar.Controls.Add(_lblLearnSource, 0, 0);
            _tlpLearnBar.Controls.Add(_tbLearnImage, 1, 0);
            _tlpLearnBar.Controls.Add(_btnOpenLearn, 2, 0);
            _tlpLearnBar.Controls.Add(_btnOpenFolder, 3, 0);
            _tlpLearnBar.Controls.Add(_btnClearLearn, 4, 0);
            _tlpLearnBar.Controls.Add(_lblLearnHint, 0, 1);
            _tlpLearnBar.Dock = DockStyle.Fill;
            _tlpLearnBar.Location = new Point(0, 0);
            _tlpLearnBar.Margin = new Padding(0, 0, 0, 8);
            _tlpLearnBar.Name = "_tlpLearnBar";
            _tlpLearnBar.RowCount = 2;
            _tlpLearnBar.RowStyles.Add(new RowStyle());
            _tlpLearnBar.RowStyles.Add(new RowStyle());
            _tlpLearnBar.Size = new Size(1232, 80);
            _tlpLearnBar.TabIndex = 0;
            // 
            // _lblLearnSource
            // 
            _lblLearnSource.Anchor = AnchorStyles.Left;
            _lblLearnSource.AutoSize = true;
            _lblLearnSource.Font = new Font("Microsoft YaHei UI", 9.5F, FontStyle.Bold);
            _lblLearnSource.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _lblLearnSource.Location = new Point(0, 11);
            _lblLearnSource.Margin = new Padding(0, 0, 8, 0);
            _lblLearnSource.Name = "_lblLearnSource";
            _lblLearnSource.Size = new Size(35, 19);
            _lblLearnSource.TabIndex = 0;
            _lblLearnSource.Text = "图片";
            // 
            // _tbLearnImage
            // 
            _tbLearnImage.BackColor = Color.White;
            _tbLearnImage.Dock = DockStyle.Fill;
            _tbLearnImage.Font = new Font("Microsoft YaHei UI", 9.5F);
            _tbLearnImage.Location = new Point(43, 6);
            _tbLearnImage.Margin = new Padding(0, 6, 8, 6);
            _tbLearnImage.Name = "_tbLearnImage";
            _tbLearnImage.ReadOnly = true;
            _tbLearnImage.Size = new Size(891, 24);
            _tbLearnImage.TabIndex = 1;
            // 
            // _btnOpenLearn
            // 
            _btnOpenLearn.Anchor = AnchorStyles.Left;
            _btnOpenLearn.BackColor = Color.White;
            _btnOpenLearn.Cursor = Cursors.Hand;
            _btnOpenLearn.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnOpenLearn.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnOpenLearn.FlatStyle = FlatStyle.Flat;
            _btnOpenLearn.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnOpenLearn.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnOpenLearn.Location = new Point(942, 4);
            _btnOpenLearn.Margin = new Padding(0, 4, 8, 4);
            _btnOpenLearn.MinimumSize = new Size(96, 34);
            _btnOpenLearn.Name = "_btnOpenLearn";
            _btnOpenLearn.Padding = new Padding(10, 0, 10, 0);
            _btnOpenLearn.Size = new Size(96, 34);
            _btnOpenLearn.TabIndex = 2;
            _btnOpenLearn.Text = "打开图片…";
            _btnOpenLearn.UseVisualStyleBackColor = false;
            // 
            // _btnOpenFolder
            // 
            _btnOpenFolder.Anchor = AnchorStyles.Left;
            _btnOpenFolder.BackColor = Color.White;
            _btnOpenFolder.Cursor = Cursors.Hand;
            _btnOpenFolder.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnOpenFolder.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnOpenFolder.FlatStyle = FlatStyle.Flat;
            _btnOpenFolder.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnOpenFolder.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnOpenFolder.Location = new Point(1046, 4);
            _btnOpenFolder.Margin = new Padding(0, 4, 8, 4);
            _btnOpenFolder.MinimumSize = new Size(116, 34);
            _btnOpenFolder.Name = "_btnOpenFolder";
            _btnOpenFolder.Padding = new Padding(10, 0, 10, 0);
            _btnOpenFolder.Size = new Size(116, 34);
            _btnOpenFolder.TabIndex = 3;
            _btnOpenFolder.Text = "打开文件夹…";
            _btnOpenFolder.UseVisualStyleBackColor = false;
            // 
            // _btnClearLearn
            // 
            _btnClearLearn.Anchor = AnchorStyles.Left;
            _btnClearLearn.BackColor = Color.White;
            _btnClearLearn.Cursor = Cursors.Hand;
            _btnClearLearn.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnClearLearn.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnClearLearn.FlatStyle = FlatStyle.Flat;
            _btnClearLearn.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnClearLearn.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnClearLearn.Location = new Point(1170, 4);
            _btnClearLearn.Margin = new Padding(0, 4, 0, 4);
            _btnClearLearn.MinimumSize = new Size(62, 34);
            _btnClearLearn.Name = "_btnClearLearn";
            _btnClearLearn.Padding = new Padding(10, 0, 10, 0);
            _btnClearLearn.Size = new Size(62, 34);
            _btnClearLearn.TabIndex = 4;
            _btnClearLearn.Text = "清空";
            _btnClearLearn.UseVisualStyleBackColor = false;
            // 
            // _lblLearnHint
            // 
            _lblLearnHint.AutoSize = true;
            _tlpLearnBar.SetColumnSpan(_lblLearnHint, 5);
            _lblLearnHint.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lblLearnHint.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblLearnHint.Location = new Point(0, 44);
            _lblLearnHint.Margin = new Padding(0, 2, 0, 2);
            _lblLearnHint.Name = "_lblLearnHint";
            _lblLearnHint.Size = new Size(1229, 34);
            _lblLearnHint.TabIndex = 5;
            _lblLearnHint.Text = "① 打开图片  →  ② 拖动框选  →  ③ 拖边角调大小、框内拖动平移、拖圆点旋转（Shift 吸附 15°）→  ④ 回车 / 右键确认并命名；重新框选或 Esc 取消。已保存的框：单击选中后同样能拖手柄改大小/旋转，或点框内拖动。标注立即导出到 mydataset。";
            // 
            // _splitLearn
            // 
            _splitLearn.BackColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _splitLearn.Dock = DockStyle.Fill;
            _splitLearn.FixedPanel = FixedPanel.Panel2;
            _splitLearn.Location = new Point(0, 88);
            _splitLearn.Margin = new Padding(0);
            _splitLearn.Name = "_splitLearn";
            // 
            // _splitLearn.Panel1
            // 
            _splitLearn.Panel1.Controls.Add(_pnlLearnLeft);
            _splitLearn.Panel1MinSize = 300;
            // 
            // _splitLearn.Panel2
            // 
            _splitLearn.Panel2.Controls.Add(_pnlLearnSide);
            _splitLearn.Panel2MinSize = 380;
            _splitLearn.Size = new Size(1232, 345);
            _splitLearn.SplitterDistance = 553;
            _splitLearn.SplitterWidth = 8;
            _splitLearn.TabIndex = 1;
            // 
            // _pnlLearnLeft
            // 
            _pnlLearnLeft.BackColor = Color.White;
            _pnlLearnLeft.Controls.Add(_picLearn);
            _pnlLearnLeft.Dock = DockStyle.Fill;
            _pnlLearnLeft.Location = new Point(0, 0);
            _pnlLearnLeft.Name = "_pnlLearnLeft";
            _pnlLearnLeft.Padding = new Padding(8);
            _pnlLearnLeft.Size = new Size(553, 345);
            _pnlLearnLeft.TabIndex = 0;
            // 
            // _picLearn
            // 
            _picLearn.BackColor = Color.FromArgb((int)(byte)36, (int)(byte)40, (int)(byte)47);
            _picLearn.BoxLabel = null;
            _picLearn.BoxPending = null;
            _picLearn.CanvasMenu = null;
            _picLearn.DetectionLabel = null;
            _picLearn.Dock = DockStyle.Fill;
            _picLearn.Editable = true;
            _picLearn.EmptyText = "① 打开图片\r\n② 在图上拖动框选目标";
            _picLearn.Font = new Font("Microsoft YaHei UI", 9.5F);
            _picLearn.Image = null;
            _picLearn.LineA = null;
            _picLearn.LineB = null;
            _picLearn.LineEditable = false;
            _picLearn.LineLabel = "计数线";
            _picLearn.Location = new Point(8, 8);
            _picLearn.Name = "_picLearn";
            _picLearn.RightClickAction = null;
            _picLearn.SelectedIndex = -1;
            _picLearn.ShowZoomInfo = true;
            _picLearn.Size = new Size(537, 329);
            _picLearn.TabIndex = 0;
            _picLearn.TrackLabel = null;
            // 
            // _pnlLearnSide
            // 
            _pnlLearnSide.AutoScroll = true;
            _pnlLearnSide.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _pnlLearnSide.Controls.Add(_tlpLearnStack);
            _pnlLearnSide.Dock = DockStyle.Fill;
            _pnlLearnSide.Location = new Point(0, 0);
            _pnlLearnSide.Name = "_pnlLearnSide";
            _pnlLearnSide.Padding = new Padding(8, 0, 0, 0);
            _pnlLearnSide.Size = new Size(671, 345);
            _pnlLearnSide.TabIndex = 0;
            // 
            // _tlpLearnStack
            // 
            _tlpLearnStack.AutoSize = true;
            _tlpLearnStack.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _tlpLearnStack.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _tlpLearnStack.ColumnCount = 1;
            _tlpLearnStack.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            _tlpLearnStack.Controls.Add(_cardImgList);
            _tlpLearnStack.Controls.Add(_cardBoxes);
            _tlpLearnStack.Controls.Add(_cardDataset);
            _tlpLearnStack.Dock = DockStyle.Top;
            _tlpLearnStack.Location = new Point(8, 0);
            _tlpLearnStack.Margin = new Padding(0);
            _tlpLearnStack.Name = "_tlpLearnStack";
            _tlpLearnStack.RowCount = 3;
            _tlpLearnStack.RowStyles.Add(new RowStyle());
            _tlpLearnStack.RowStyles.Add(new RowStyle());
            _tlpLearnStack.RowStyles.Add(new RowStyle());
            _tlpLearnStack.Size = new Size(646, 738);
            _tlpLearnStack.TabIndex = 0;
            // 
            // _cardImgList
            // 
            _cardImgList.AutoSize = true;
            _cardImgList.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _cardImgList.BackColor = Color.White;
            _cardImgList.Controls.Add(_cardImgListBody);
            _cardImgList.Dock = DockStyle.Top;
            _cardImgList.Location = new Point(0, 0);
            _cardImgList.Margin = new Padding(0, 0, 0, 10);
            _cardImgList.Name = "_cardImgList";
            _cardImgList.Padding = new Padding(14, 12, 14, 12);
            _cardImgList.Size = new Size(646, 355);
            _cardImgList.TabIndex = 0;
            _cardImgList.Paint += (this.pnlCard_Paint);
            // 
            // _cardImgListBody
            // 
            _cardImgListBody.AutoSize = true;
            _cardImgListBody.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _cardImgListBody.BackColor = Color.White;
            _cardImgListBody.ColumnCount = 1;
            _cardImgListBody.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            _cardImgListBody.Controls.Add(_lblImgListTitle, 0, 0);
            _cardImgListBody.Controls.Add(_lvImages, 0, 1);
            _cardImgListBody.Controls.Add(_flowImgList, 0, 2);
            _cardImgListBody.Controls.Add(_flowAuto, 0, 3);
            _cardImgListBody.Controls.Add(_lblImageListInfo, 0, 4);
            _cardImgListBody.Controls.Add(_lblReviewHint, 0, 5);
            _cardImgListBody.Dock = DockStyle.Top;
            _cardImgListBody.Location = new Point(14, 12);
            _cardImgListBody.Margin = new Padding(0);
            _cardImgListBody.Name = "_cardImgListBody";
            _cardImgListBody.RowCount = 6;
            _cardImgListBody.RowStyles.Add(new RowStyle());
            _cardImgListBody.RowStyles.Add(new RowStyle());
            _cardImgListBody.RowStyles.Add(new RowStyle());
            _cardImgListBody.RowStyles.Add(new RowStyle());
            _cardImgListBody.RowStyles.Add(new RowStyle());
            _cardImgListBody.RowStyles.Add(new RowStyle());
            _cardImgListBody.Size = new Size(618, 331);
            _cardImgListBody.TabIndex = 0;
            // 
            // _lblImgListTitle
            // 
            _lblImgListTitle.AutoSize = true;
            _lblImgListTitle.Font = new Font("Microsoft YaHei UI", 10.5F, FontStyle.Bold);
            _lblImgListTitle.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _lblImgListTitle.Location = new Point(0, 0);
            _lblImgListTitle.Margin = new Padding(0, 0, 0, 8);
            _lblImgListTitle.Name = "_lblImgListTitle";
            _lblImgListTitle.Size = new Size(65, 19);
            _lblImgListTitle.TabIndex = 0;
            _lblImgListTitle.Text = "图片列表";
            // 
            // _lvImages
            // 
            _lvImages.BorderStyle = BorderStyle.FixedSingle;
            _lvImages.Columns.AddRange(new ColumnHeader[] { colLvImage, colLvAnno, colLvStatus });
            _lvImages.Dock = DockStyle.Top;
            _lvImages.Font = new Font("Microsoft YaHei UI", 9.5F);
            _lvImages.FullRowSelect = true;
            _lvImages.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            _lvImages.Location = new Point(0, 27);
            _lvImages.Margin = new Padding(0, 0, 0, 6);
            _lvImages.MultiSelect = false;
            _lvImages.Name = "_lvImages";
            _lvImages.Size = new Size(618, 168);
            _lvImages.TabIndex = 1;
            _lvImages.UseCompatibleStateImageBehavior = false;
            _lvImages.View = View.Details;
            // 
            // colLvImage
            // 
            colLvImage.Name = "colLvImage";
            colLvImage.Text = "图片";
            colLvImage.Width = 186;
            // 
            // colLvAnno
            // 
            colLvAnno.Name = "colLvAnno";
            colLvAnno.Text = "标注";
            colLvAnno.TextAlign = HorizontalAlignment.Right;
            colLvAnno.Width = 54;
            // 
            // colLvStatus
            // 
            colLvStatus.Name = "colLvStatus";
            colLvStatus.Text = "状态";
            colLvStatus.Width = 76;
            // 
            // _flowImgList
            // 
            _flowImgList.AutoSize = true;
            _flowImgList.Controls.Add(_btnPrevImage);
            _flowImgList.Controls.Add(_btnNextImage);
            _flowImgList.Dock = DockStyle.Top;
            _flowImgList.Location = new Point(0, 201);
            _flowImgList.Margin = new Padding(0, 0, 0, 4);
            _flowImgList.Name = "_flowImgList";
            _flowImgList.Size = new Size(618, 40);
            _flowImgList.TabIndex = 2;
            _flowImgList.WrapContents = false;
            // 
            // _btnPrevImage
            // 
            _btnPrevImage.BackColor = Color.White;
            _btnPrevImage.Cursor = Cursors.Hand;
            _btnPrevImage.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnPrevImage.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnPrevImage.FlatStyle = FlatStyle.Flat;
            _btnPrevImage.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnPrevImage.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnPrevImage.Location = new Point(4, 4);
            _btnPrevImage.Margin = new Padding(4);
            _btnPrevImage.MinimumSize = new Size(74, 32);
            _btnPrevImage.Name = "_btnPrevImage";
            _btnPrevImage.Padding = new Padding(10, 0, 10, 0);
            _btnPrevImage.Size = new Size(74, 32);
            _btnPrevImage.TabIndex = 0;
            _btnPrevImage.Text = "上一张";
            _btnPrevImage.UseVisualStyleBackColor = false;
            // 
            // _btnNextImage
            // 
            _btnNextImage.BackColor = Color.White;
            _btnNextImage.Cursor = Cursors.Hand;
            _btnNextImage.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnNextImage.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnNextImage.FlatStyle = FlatStyle.Flat;
            _btnNextImage.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnNextImage.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnNextImage.Location = new Point(86, 4);
            _btnNextImage.Margin = new Padding(4);
            _btnNextImage.MinimumSize = new Size(74, 32);
            _btnNextImage.Name = "_btnNextImage";
            _btnNextImage.Padding = new Padding(10, 0, 10, 0);
            _btnNextImage.Size = new Size(74, 32);
            _btnNextImage.TabIndex = 1;
            _btnNextImage.Text = "下一张";
            _btnNextImage.UseVisualStyleBackColor = false;
            // 
            // _flowAuto
            // 
            _flowAuto.AutoSize = true;
            _flowAuto.Controls.Add(_btnAutoLabel);
            _flowAuto.Controls.Add(_btnReview);
            _flowAuto.Controls.Add(_btnClearAuto);
            _flowAuto.Dock = DockStyle.Top;
            _flowAuto.Location = new Point(0, 245);
            _flowAuto.Margin = new Padding(0, 0, 0, 4);
            _flowAuto.Name = "_flowAuto";
            _flowAuto.Size = new Size(618, 40);
            _flowAuto.TabIndex = 3;
            _flowAuto.WrapContents = false;
            // 
            // _btnAutoLabel
            // 
            _btnAutoLabel.BackColor = Color.FromArgb((int)(byte)22, (int)(byte)138, (int)(byte)88);
            _btnAutoLabel.Cursor = Cursors.Hand;
            _btnAutoLabel.FlatAppearance.BorderSize = 0;
            _btnAutoLabel.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)18, (int)(byte)118, (int)(byte)74);
            _btnAutoLabel.FlatStyle = FlatStyle.Flat;
            _btnAutoLabel.Font = new Font("Microsoft YaHei UI", 9.5F, FontStyle.Bold);
            _btnAutoLabel.ForeColor = Color.White;
            _btnAutoLabel.Location = new Point(4, 4);
            _btnAutoLabel.Margin = new Padding(4);
            _btnAutoLabel.MinimumSize = new Size(116, 32);
            _btnAutoLabel.Name = "_btnAutoLabel";
            _btnAutoLabel.Padding = new Padding(10, 0, 10, 0);
            _btnAutoLabel.Size = new Size(116, 32);
            _btnAutoLabel.TabIndex = 0;
            _btnAutoLabel.Text = "⚡ 自动预标注";
            _btnAutoLabel.UseVisualStyleBackColor = false;
            // 
            // _btnReview
            // 
            _btnReview.BackColor = Color.White;
            _btnReview.Cursor = Cursors.Hand;
            _btnReview.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnReview.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnReview.FlatStyle = FlatStyle.Flat;
            _btnReview.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnReview.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnReview.Location = new Point(128, 4);
            _btnReview.Margin = new Padding(4);
            _btnReview.MinimumSize = new Size(58, 32);
            _btnReview.Name = "_btnReview";
            _btnReview.Padding = new Padding(10, 0, 10, 0);
            _btnReview.Size = new Size(58, 32);
            _btnReview.TabIndex = 1;
            _btnReview.Text = "复核";
            _btnReview.UseVisualStyleBackColor = false;
            // 
            // _btnClearAuto
            // 
            _btnClearAuto.BackColor = Color.White;
            _btnClearAuto.Cursor = Cursors.Hand;
            _btnClearAuto.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnClearAuto.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnClearAuto.FlatStyle = FlatStyle.Flat;
            _btnClearAuto.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnClearAuto.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnClearAuto.Location = new Point(194, 4);
            _btnClearAuto.Margin = new Padding(4);
            _btnClearAuto.MinimumSize = new Size(92, 32);
            _btnClearAuto.Name = "_btnClearAuto";
            _btnClearAuto.Padding = new Padding(10, 0, 10, 0);
            _btnClearAuto.Size = new Size(92, 32);
            _btnClearAuto.TabIndex = 2;
            _btnClearAuto.Text = "清空自动框";
            _btnClearAuto.UseVisualStyleBackColor = false;
            // 
            // _lblImageListInfo
            // 
            _lblImageListInfo.AutoSize = true;
            _lblImageListInfo.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lblImageListInfo.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblImageListInfo.Location = new Point(0, 291);
            _lblImageListInfo.Margin = new Padding(0, 2, 0, 2);
            _lblImageListInfo.Name = "_lblImageListInfo";
            _lblImageListInfo.Size = new Size(342, 17);
            _lblImageListInfo.TabIndex = 4;
            _lblImageListInfo.Text = "点“打开文件夹”一次载入整个文件夹；选中列表即可切换图片。";
            // 
            // _lblReviewHint
            // 
            _lblReviewHint.AutoSize = true;
            _lblReviewHint.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lblReviewHint.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblReviewHint.Location = new Point(0, 312);
            _lblReviewHint.Margin = new Padding(0, 2, 0, 2);
            _lblReviewHint.Name = "_lblReviewHint";
            _lblReviewHint.Size = new Size(428, 17);
            _lblReviewHint.TabIndex = 5;
            _lblReviewHint.Text = "复核中：Enter 确认并下一张 · D 丢弃本图自动框 · Delete 删选中框 · Esc 退出";
            _lblReviewHint.Visible = false;
            // 
            // _cardBoxes
            // 
            _cardBoxes.AutoSize = true;
            _cardBoxes.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _cardBoxes.BackColor = Color.White;
            _cardBoxes.Controls.Add(_cardBoxesBody);
            _cardBoxes.Dock = DockStyle.Top;
            _cardBoxes.Location = new Point(0, 365);
            _cardBoxes.Margin = new Padding(0, 0, 0, 10);
            _cardBoxes.Name = "_cardBoxes";
            _cardBoxes.Padding = new Padding(14, 12, 14, 12);
            _cardBoxes.Size = new Size(646, 246);
            _cardBoxes.TabIndex = 1;
            _cardBoxes.Paint += (this.pnlCard_Paint);
            // 
            // _cardBoxesBody
            // 
            _cardBoxesBody.AutoSize = true;
            _cardBoxesBody.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _cardBoxesBody.BackColor = Color.White;
            _cardBoxesBody.ColumnCount = 1;
            _cardBoxesBody.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            _cardBoxesBody.Controls.Add(_lblBoxesTitle, 0, 0);
            _cardBoxesBody.Controls.Add(_lvBoxes, 0, 1);
            _cardBoxesBody.Controls.Add(_flowBoxOps, 0, 2);
            _cardBoxesBody.Controls.Add(_lblAnnoInfo, 0, 3);
            _cardBoxesBody.Dock = DockStyle.Top;
            _cardBoxesBody.Location = new Point(14, 12);
            _cardBoxesBody.Margin = new Padding(0);
            _cardBoxesBody.Name = "_cardBoxesBody";
            _cardBoxesBody.RowCount = 4;
            _cardBoxesBody.RowStyles.Add(new RowStyle());
            _cardBoxesBody.RowStyles.Add(new RowStyle());
            _cardBoxesBody.RowStyles.Add(new RowStyle());
            _cardBoxesBody.RowStyles.Add(new RowStyle());
            _cardBoxesBody.Size = new Size(618, 222);
            _cardBoxesBody.TabIndex = 0;
            // 
            // _lblBoxesTitle
            // 
            _lblBoxesTitle.AutoSize = true;
            _lblBoxesTitle.Font = new Font("Microsoft YaHei UI", 10.5F, FontStyle.Bold);
            _lblBoxesTitle.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _lblBoxesTitle.Location = new Point(0, 0);
            _lblBoxesTitle.Margin = new Padding(0, 0, 0, 8);
            _lblBoxesTitle.Name = "_lblBoxesTitle";
            _lblBoxesTitle.Size = new Size(65, 19);
            _lblBoxesTitle.TabIndex = 0;
            _lblBoxesTitle.Text = "本图标注";
            // 
            // _lvBoxes
            // 
            _lvBoxes.BorderStyle = BorderStyle.FixedSingle;
            _lvBoxes.Columns.AddRange(new ColumnHeader[] { colBoxName, colBoxEn, colBoxRect });
            _lvBoxes.Dock = DockStyle.Top;
            _lvBoxes.Font = new Font("Microsoft YaHei UI", 9.5F);
            _lvBoxes.FullRowSelect = true;
            _lvBoxes.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            _lvBoxes.Location = new Point(0, 27);
            _lvBoxes.Margin = new Padding(0, 0, 0, 8);
            _lvBoxes.MultiSelect = false;
            _lvBoxes.Name = "_lvBoxes";
            _lvBoxes.Size = new Size(618, 120);
            _lvBoxes.TabIndex = 1;
            _lvBoxes.UseCompatibleStateImageBehavior = false;
            _lvBoxes.View = View.Details;
            // 
            // colBoxName
            // 
            colBoxName.Name = "colBoxName";
            colBoxName.Text = "目标";
            colBoxName.Width = 92;
            // 
            // colBoxEn
            // 
            colBoxEn.Name = "colBoxEn";
            colBoxEn.Text = "存储名";
            colBoxEn.Width = 78;
            // 
            // colBoxRect
            // 
            colBoxRect.Name = "colBoxRect";
            colBoxRect.Text = "框(x,y,w,h)";
            colBoxRect.Width = 152;
            // 
            // _flowBoxOps
            // 
            _flowBoxOps.AutoSize = true;
            _flowBoxOps.Controls.Add(_btnDelBox);
            _flowBoxOps.Controls.Add(_btnRenameBox);
            _flowBoxOps.Controls.Add(_btnReexport);
            _flowBoxOps.Dock = DockStyle.Top;
            _flowBoxOps.Location = new Point(0, 155);
            _flowBoxOps.Margin = new Padding(0, 0, 0, 6);
            _flowBoxOps.Name = "_flowBoxOps";
            _flowBoxOps.Size = new Size(618, 40);
            _flowBoxOps.TabIndex = 2;
            _flowBoxOps.WrapContents = false;
            // 
            // _btnDelBox
            // 
            _btnDelBox.BackColor = Color.White;
            _btnDelBox.Cursor = Cursors.Hand;
            _btnDelBox.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnDelBox.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnDelBox.FlatStyle = FlatStyle.Flat;
            _btnDelBox.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnDelBox.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnDelBox.Location = new Point(4, 4);
            _btnDelBox.Margin = new Padding(4);
            _btnDelBox.MinimumSize = new Size(68, 32);
            _btnDelBox.Name = "_btnDelBox";
            _btnDelBox.Padding = new Padding(10, 0, 10, 0);
            _btnDelBox.Size = new Size(68, 32);
            _btnDelBox.TabIndex = 0;
            _btnDelBox.Text = "删除";
            _btnDelBox.UseVisualStyleBackColor = false;
            // 
            // _btnRenameBox
            // 
            _btnRenameBox.BackColor = Color.White;
            _btnRenameBox.Cursor = Cursors.Hand;
            _btnRenameBox.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnRenameBox.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnRenameBox.FlatStyle = FlatStyle.Flat;
            _btnRenameBox.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnRenameBox.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnRenameBox.Location = new Point(80, 4);
            _btnRenameBox.Margin = new Padding(4);
            _btnRenameBox.MinimumSize = new Size(68, 32);
            _btnRenameBox.Name = "_btnRenameBox";
            _btnRenameBox.Padding = new Padding(10, 0, 10, 0);
            _btnRenameBox.Size = new Size(68, 32);
            _btnRenameBox.TabIndex = 1;
            _btnRenameBox.Text = "改名";
            _btnRenameBox.UseVisualStyleBackColor = false;
            // 
            // _btnReexport
            // 
            _btnReexport.BackColor = Color.White;
            _btnReexport.Cursor = Cursors.Hand;
            _btnReexport.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnReexport.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnReexport.FlatStyle = FlatStyle.Flat;
            _btnReexport.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnReexport.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnReexport.Location = new Point(156, 4);
            _btnReexport.Margin = new Padding(4);
            _btnReexport.MinimumSize = new Size(92, 32);
            _btnReexport.Name = "_btnReexport";
            _btnReexport.Padding = new Padding(10, 0, 10, 0);
            _btnReexport.Size = new Size(92, 32);
            _btnReexport.TabIndex = 2;
            _btnReexport.Text = "重新导出";
            _btnReexport.UseVisualStyleBackColor = false;
            // 
            // _lblAnnoInfo
            // 
            _lblAnnoInfo.AutoSize = true;
            _lblAnnoInfo.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lblAnnoInfo.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblAnnoInfo.Location = new Point(0, 203);
            _lblAnnoInfo.Margin = new Padding(0, 2, 0, 2);
            _lblAnnoInfo.Name = "_lblAnnoInfo";
            _lblAnnoInfo.Size = new Size(284, 17);
            _lblAnnoInfo.TabIndex = 3;
            _lblAnnoInfo.Text = "本图还没有标注：在左侧图片上拖动鼠标框选目标。";
            // 
            // _cardDataset
            // 
            _cardDataset.AutoSize = true;
            _cardDataset.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _cardDataset.BackColor = Color.White;
            _cardDataset.Controls.Add(_cardDatasetBody);
            _cardDataset.Dock = DockStyle.Top;
            _cardDataset.Location = new Point(0, 621);
            _cardDataset.Margin = new Padding(0, 0, 0, 10);
            _cardDataset.Name = "_cardDataset";
            _cardDataset.Padding = new Padding(14, 12, 14, 12);
            _cardDataset.Size = new Size(646, 107);
            _cardDataset.TabIndex = 2;
            _cardDataset.Paint += (this.pnlCard_Paint);
            // 
            // _cardDatasetBody
            // 
            _cardDatasetBody.AutoSize = true;
            _cardDatasetBody.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _cardDatasetBody.BackColor = Color.White;
            _cardDatasetBody.ColumnCount = 1;
            _cardDatasetBody.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            _cardDatasetBody.Controls.Add(_lblDatasetTitle, 0, 0);
            _cardDatasetBody.Controls.Add(_lblDatasetStats, 0, 1);
            _cardDatasetBody.Controls.Add(_lnkOpenDataset, 0, 2);
            _cardDatasetBody.Dock = DockStyle.Top;
            _cardDatasetBody.Location = new Point(14, 12);
            _cardDatasetBody.Margin = new Padding(0);
            _cardDatasetBody.Name = "_cardDatasetBody";
            _cardDatasetBody.RowCount = 3;
            _cardDatasetBody.RowStyles.Add(new RowStyle());
            _cardDatasetBody.RowStyles.Add(new RowStyle());
            _cardDatasetBody.RowStyles.Add(new RowStyle());
            _cardDatasetBody.Size = new Size(618, 83);
            _cardDatasetBody.TabIndex = 0;
            // 
            // _lblDatasetTitle
            // 
            _lblDatasetTitle.AutoSize = true;
            _lblDatasetTitle.Font = new Font("Microsoft YaHei UI", 10.5F, FontStyle.Bold);
            _lblDatasetTitle.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _lblDatasetTitle.Location = new Point(0, 0);
            _lblDatasetTitle.Margin = new Padding(0, 0, 0, 8);
            _lblDatasetTitle.Name = "_lblDatasetTitle";
            _lblDatasetTitle.Size = new Size(51, 19);
            _lblDatasetTitle.TabIndex = 0;
            _lblDatasetTitle.Text = "数据集";
            // 
            // _lblDatasetStats
            // 
            _lblDatasetStats.AutoSize = true;
            _lblDatasetStats.Font = new Font("Microsoft YaHei UI", 9.5F);
            _lblDatasetStats.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _lblDatasetStats.Location = new Point(0, 27);
            _lblDatasetStats.Margin = new Padding(0, 0, 0, 6);
            _lblDatasetStats.Name = "_lblDatasetStats";
            _lblDatasetStats.Size = new Size(145, 19);
            _lblDatasetStats.TabIndex = 1;
            _lblDatasetStats.Text = "0 张图片 · 0 个框 · 0 类";
            // 
            // _lnkOpenDataset
            // 
            _lnkOpenDataset.AutoSize = true;
            _lnkOpenDataset.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _lnkOpenDataset.BackColor = Color.Transparent;
            _lnkOpenDataset.Cursor = Cursors.Hand;
            _lnkOpenDataset.FlatAppearance.BorderSize = 0;
            _lnkOpenDataset.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)232, (int)(byte)240, (int)(byte)252);
            _lnkOpenDataset.FlatStyle = FlatStyle.Flat;
            _lnkOpenDataset.Font = new Font("Microsoft YaHei UI", 9.5F);
            _lnkOpenDataset.ForeColor = Color.FromArgb((int)(byte)36, (int)(byte)112, (int)(byte)210);
            _lnkOpenDataset.Location = new Point(0, 52);
            _lnkOpenDataset.Margin = new Padding(0, 0, 0, 2);
            _lnkOpenDataset.Name = "_lnkOpenDataset";
            _lnkOpenDataset.Padding = new Padding(4, 0, 4, 0);
            _lnkOpenDataset.Size = new Size(131, 29);
            _lnkOpenDataset.TabIndex = 2;
            _lnkOpenDataset.Text = "打开数据集文件夹";
            _lnkOpenDataset.UseVisualStyleBackColor = false;
            // 
            // _cardTrain
            // 
            _cardTrain.BackColor = Color.White;
            _cardTrain.Controls.Add(_cardTrainBody);
            _cardTrain.Dock = DockStyle.Fill;
            _cardTrain.Location = new Point(0, 441);
            _cardTrain.Margin = new Padding(0, 8, 0, 0);
            _cardTrain.Name = "_cardTrain";
            _cardTrain.Padding = new Padding(14, 12, 14, 12);
            _cardTrain.Size = new Size(1232, 208);
            _cardTrain.TabIndex = 2;
            _cardTrain.Paint += (this.pnlCard_Paint);
            // 
            // _cardTrainBody
            // 
            _cardTrainBody.AutoSize = true;
            _cardTrainBody.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _cardTrainBody.BackColor = Color.White;
            _cardTrainBody.ColumnCount = 1;
            _cardTrainBody.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            _cardTrainBody.Controls.Add(_lblTrainTitle, 0, 0);
            _cardTrainBody.Controls.Add(_tlpTrainGrid, 0, 1);
            _cardTrainBody.Dock = DockStyle.Top;
            _cardTrainBody.Location = new Point(14, 12);
            _cardTrainBody.Margin = new Padding(0);
            _cardTrainBody.Name = "_cardTrainBody";
            _cardTrainBody.RowCount = 2;
            _cardTrainBody.RowStyles.Add(new RowStyle());
            _cardTrainBody.RowStyles.Add(new RowStyle());
            _cardTrainBody.Size = new Size(1204, 162);
            _cardTrainBody.TabIndex = 0;
            // 
            // _lblTrainTitle
            // 
            _lblTrainTitle.AutoSize = true;
            _lblTrainTitle.Font = new Font("Microsoft YaHei UI", 10.5F, FontStyle.Bold);
            _lblTrainTitle.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _lblTrainTitle.Location = new Point(0, 0);
            _lblTrainTitle.Margin = new Padding(0, 0, 0, 8);
            _lblTrainTitle.Name = "_lblTrainTitle";
            _lblTrainTitle.Size = new Size(65, 19);
            _lblTrainTitle.TabIndex = 0;
            _lblTrainTitle.Text = "训练模型";
            // 
            // _tlpTrainGrid
            // 
            _tlpTrainGrid.AutoSize = true;
            _tlpTrainGrid.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _tlpTrainGrid.ColumnCount = 2;
            _tlpTrainGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55F));
            _tlpTrainGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45F));
            _tlpTrainGrid.Controls.Add(_tlpTrainLeft, 0, 0);
            _tlpTrainGrid.Controls.Add(_tlpTrainRight, 1, 0);
            _tlpTrainGrid.Location = new Point(0, 27);
            _tlpTrainGrid.Margin = new Padding(0);
            _tlpTrainGrid.Name = "_tlpTrainGrid";
            _tlpTrainGrid.RowCount = 1;
            _tlpTrainGrid.RowStyles.Add(new RowStyle());
            _tlpTrainGrid.Size = new Size(1131, 135);
            _tlpTrainGrid.TabIndex = 1;
            // 
            // _tlpTrainLeft
            // 
            _tlpTrainLeft.AutoSize = true;
            _tlpTrainLeft.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _tlpTrainLeft.ColumnCount = 4;
            _tlpTrainLeft.ColumnStyles.Add(new ColumnStyle());
            _tlpTrainLeft.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 26F));
            _tlpTrainLeft.ColumnStyles.Add(new ColumnStyle());
            _tlpTrainLeft.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 74F));
            _tlpTrainLeft.Controls.Add(_lblEpochs, 0, 0);
            _tlpTrainLeft.Controls.Add(_numEpochs, 1, 0);
            _tlpTrainLeft.Controls.Add(_lblImgSize, 2, 0);
            _tlpTrainLeft.Controls.Add(_cboImgSize, 3, 0);
            _tlpTrainLeft.Controls.Add(_lblBatch, 0, 1);
            _tlpTrainLeft.Controls.Add(_numBatch, 1, 1);
            _tlpTrainLeft.Controls.Add(_flowTrainBtns, 0, 2);
            _tlpTrainLeft.Location = new Point(0, 0);
            _tlpTrainLeft.Margin = new Padding(0, 0, 12, 0);
            _tlpTrainLeft.Name = "_tlpTrainLeft";
            _tlpTrainLeft.RowCount = 3;
            _tlpTrainLeft.RowStyles.Add(new RowStyle());
            _tlpTrainLeft.RowStyles.Add(new RowStyle());
            _tlpTrainLeft.RowStyles.Add(new RowStyle());
            _tlpTrainLeft.Size = new Size(607, 112);
            _tlpTrainLeft.TabIndex = 0;
            // 
            // _lblEpochs
            // 
            _lblEpochs.Anchor = AnchorStyles.Left;
            _lblEpochs.AutoSize = true;
            _lblEpochs.Font = new Font("Microsoft YaHei UI", 9.5F);
            _lblEpochs.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblEpochs.Location = new Point(0, 7);
            _lblEpochs.Margin = new Padding(0, 7, 8, 6);
            _lblEpochs.Name = "_lblEpochs";
            _lblEpochs.Size = new Size(35, 19);
            _lblEpochs.TabIndex = 0;
            _lblEpochs.Text = "轮数";
            // 
            // _numEpochs
            // 
            _numEpochs.Dock = DockStyle.Fill;
            _numEpochs.Font = new Font("Microsoft YaHei UI", 9.5F);
            _numEpochs.Increment = new decimal(new int[] { 5, 0, 0, 0 });
            _numEpochs.Location = new Point(56, 2);
            _numEpochs.Margin = new Padding(0, 2, 12, 2);
            _numEpochs.Maximum = new decimal(new int[] { 300, 0, 0, 0 });
            _numEpochs.Minimum = new decimal(new int[] { 5, 0, 0, 0 });
            _numEpochs.Name = "_numEpochs";
            _numEpochs.Size = new Size(120, 24);
            _numEpochs.TabIndex = 1;
            _numEpochs.Value = new decimal(new int[] { 5, 0, 0, 0 });
            // 
            // _lblImgSize
            // 
            _lblImgSize.Anchor = AnchorStyles.Left;
            _lblImgSize.AutoSize = true;
            _lblImgSize.Font = new Font("Microsoft YaHei UI", 9.5F);
            _lblImgSize.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblImgSize.Location = new Point(188, 7);
            _lblImgSize.Margin = new Padding(0, 7, 8, 6);
            _lblImgSize.Name = "_lblImgSize";
            _lblImgSize.Size = new Size(35, 19);
            _lblImgSize.TabIndex = 2;
            _lblImgSize.Text = "尺寸";
            // 
            // _cboImgSize
            // 
            _cboImgSize.Dock = DockStyle.Fill;
            _cboImgSize.DropDownStyle = ComboBoxStyle.DropDownList;
            _cboImgSize.Font = new Font("Microsoft YaHei UI", 9.5F);
            _cboImgSize.Items.AddRange(new object[] { "320", "416", "512", "640" });
            _cboImgSize.Location = new Point(231, 2);
            _cboImgSize.Margin = new Padding(0, 2, 0, 2);
            _cboImgSize.Name = "_cboImgSize";
            _cboImgSize.Size = new Size(376, 27);
            _cboImgSize.TabIndex = 3;
            // 
            // _lblBatch
            // 
            _lblBatch.Anchor = AnchorStyles.Left;
            _lblBatch.AutoSize = true;
            _lblBatch.Font = new Font("Microsoft YaHei UI", 9.5F);
            _lblBatch.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblBatch.Location = new Point(0, 39);
            _lblBatch.Margin = new Padding(0, 7, 8, 6);
            _lblBatch.Name = "_lblBatch";
            _lblBatch.Size = new Size(48, 19);
            _lblBatch.TabIndex = 4;
            _lblBatch.Text = "批大小";
            // 
            // _numBatch
            // 
            _numBatch.Dock = DockStyle.Fill;
            _numBatch.Font = new Font("Microsoft YaHei UI", 9.5F);
            _numBatch.Location = new Point(56, 34);
            _numBatch.Margin = new Padding(0, 2, 12, 2);
            _numBatch.Maximum = new decimal(new int[] { 16, 0, 0, 0 });
            _numBatch.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            _numBatch.Name = "_numBatch";
            _numBatch.Size = new Size(120, 24);
            _numBatch.TabIndex = 5;
            _numBatch.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // _flowTrainBtns
            // 
            _flowTrainBtns.AutoSize = true;
            _tlpTrainLeft.SetColumnSpan(_flowTrainBtns, 4);
            _flowTrainBtns.Controls.Add(_btnInitEnv);
            _flowTrainBtns.Controls.Add(_btnTrain);
            _flowTrainBtns.Controls.Add(_btnCancelTrain);
            _flowTrainBtns.Dock = DockStyle.Top;
            _flowTrainBtns.Location = new Point(0, 68);
            _flowTrainBtns.Margin = new Padding(0, 4, 0, 2);
            _flowTrainBtns.Name = "_flowTrainBtns";
            _flowTrainBtns.Size = new Size(607, 42);
            _flowTrainBtns.TabIndex = 6;
            _flowTrainBtns.WrapContents = false;
            // 
            // _btnInitEnv
            // 
            _btnInitEnv.BackColor = Color.White;
            _btnInitEnv.Cursor = Cursors.Hand;
            _btnInitEnv.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnInitEnv.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnInitEnv.FlatStyle = FlatStyle.Flat;
            _btnInitEnv.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnInitEnv.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnInitEnv.Location = new Point(4, 4);
            _btnInitEnv.Margin = new Padding(4);
            _btnInitEnv.MinimumSize = new Size(108, 34);
            _btnInitEnv.Name = "_btnInitEnv";
            _btnInitEnv.Padding = new Padding(10, 0, 10, 0);
            _btnInitEnv.Size = new Size(108, 34);
            _btnInitEnv.TabIndex = 0;
            _btnInitEnv.Text = "初始化环境";
            _btnInitEnv.UseVisualStyleBackColor = false;
            // 
            // _btnTrain
            // 
            _btnTrain.BackColor = Color.FromArgb((int)(byte)22, (int)(byte)138, (int)(byte)88);
            _btnTrain.Cursor = Cursors.Hand;
            _btnTrain.FlatAppearance.BorderSize = 0;
            _btnTrain.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)18, (int)(byte)118, (int)(byte)74);
            _btnTrain.FlatStyle = FlatStyle.Flat;
            _btnTrain.Font = new Font("Microsoft YaHei UI", 9.5F, FontStyle.Bold);
            _btnTrain.ForeColor = Color.White;
            _btnTrain.Location = new Point(120, 4);
            _btnTrain.Margin = new Padding(4);
            _btnTrain.MinimumSize = new Size(96, 34);
            _btnTrain.Name = "_btnTrain";
            _btnTrain.Padding = new Padding(10, 0, 10, 0);
            _btnTrain.Size = new Size(96, 34);
            _btnTrain.TabIndex = 1;
            _btnTrain.Text = "开始训练";
            _btnTrain.UseVisualStyleBackColor = false;
            // 
            // _btnCancelTrain
            // 
            _btnCancelTrain.BackColor = Color.White;
            _btnCancelTrain.Cursor = Cursors.Hand;
            _btnCancelTrain.Enabled = false;
            _btnCancelTrain.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnCancelTrain.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnCancelTrain.FlatStyle = FlatStyle.Flat;
            _btnCancelTrain.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnCancelTrain.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnCancelTrain.Location = new Point(224, 4);
            _btnCancelTrain.Margin = new Padding(4);
            _btnCancelTrain.MinimumSize = new Size(62, 34);
            _btnCancelTrain.Name = "_btnCancelTrain";
            _btnCancelTrain.Padding = new Padding(10, 0, 10, 0);
            _btnCancelTrain.Size = new Size(62, 34);
            _btnCancelTrain.TabIndex = 2;
            _btnCancelTrain.Text = "取消";
            _btnCancelTrain.UseVisualStyleBackColor = false;
            // 
            // _tlpTrainRight
            // 
            _tlpTrainRight.AutoSize = true;
            _tlpTrainRight.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _tlpTrainRight.ColumnCount = 1;
            _tlpTrainRight.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            _tlpTrainRight.Controls.Add(_lblTrainStage, 0, 0);
            _tlpTrainRight.Controls.Add(_pbTrain, 0, 1);
            _tlpTrainRight.Controls.Add(_txtTrainLog, 0, 2);
            _tlpTrainRight.Dock = DockStyle.Fill;
            _tlpTrainRight.Location = new Point(622, 0);
            _tlpTrainRight.Margin = new Padding(0);
            _tlpTrainRight.Name = "_tlpTrainRight";
            _tlpTrainRight.RowCount = 3;
            _tlpTrainRight.RowStyles.Add(new RowStyle());
            _tlpTrainRight.RowStyles.Add(new RowStyle());
            _tlpTrainRight.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            _tlpTrainRight.Size = new Size(509, 135);
            _tlpTrainRight.TabIndex = 1;
            // 
            // _lblTrainStage
            // 
            _lblTrainStage.AutoSize = true;
            _lblTrainStage.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lblTrainStage.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblTrainStage.Location = new Point(0, 2);
            _lblTrainStage.Margin = new Padding(0, 2, 0, 2);
            _lblTrainStage.Name = "_lblTrainStage";
            _lblTrainStage.Size = new Size(280, 17);
            _lblTrainStage.TabIndex = 0;
            _lblTrainStage.Text = "≥3 个标注框即可训练；CPU 50 轮约 5~15 分钟。";
            // 
            // _pbTrain
            // 
            _pbTrain.Dock = DockStyle.Top;
            _pbTrain.Location = new Point(0, 23);
            _pbTrain.Margin = new Padding(0, 2, 0, 6);
            _pbTrain.Name = "_pbTrain";
            _pbTrain.Size = new Size(509, 8);
            _pbTrain.TabIndex = 1;
            // 
            // _txtTrainLog
            // 
            _txtTrainLog.BackColor = Color.FromArgb((int)(byte)28, (int)(byte)32, (int)(byte)39);
            _txtTrainLog.BorderStyle = BorderStyle.FixedSingle;
            _txtTrainLog.Dock = DockStyle.Fill;
            _txtTrainLog.Font = new Font("Consolas", 8.5F);
            _txtTrainLog.ForeColor = Color.FromArgb((int)(byte)214, (int)(byte)221, (int)(byte)232);
            _txtTrainLog.Location = new Point(0, 39);
            _txtTrainLog.Margin = new Padding(0, 2, 0, 0);
            _txtTrainLog.MinimumSize = new Size(0, 96);
            _txtTrainLog.Multiline = true;
            _txtTrainLog.Name = "_txtTrainLog";
            _txtTrainLog.ReadOnly = true;
            _txtTrainLog.ScrollBars = ScrollBars.Vertical;
            _txtTrainLog.Size = new Size(509, 96);
            _txtTrainLog.TabIndex = 2;
            _txtTrainLog.Text = "训练日志会显示在这里。";
            _txtTrainLog.WordWrap = false;
            // 
            // _pageTools
            // 
            _pageTools.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _pageTools.Controls.Add(_tlpTools);
            _pageTools.Location = new Point(4, 34);
            _pageTools.Name = "_pageTools";
            _pageTools.Padding = new Padding(12, 10, 12, 10);
            _pageTools.Size = new Size(1256, 669);
            _pageTools.TabIndex = 2;
            _pageTools.Text = "工具 · 我的模型";
            // 
            // _tlpTools
            // 
            _tlpTools.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _tlpTools.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _tlpTools.ColumnCount = 1;
            _tlpTools.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            _tlpTools.Controls.Add(_tlpToolsBar, 0, 0);
            _tlpTools.Controls.Add(_pnlToolsScroll, 0, 1);
            _tlpTools.Dock = DockStyle.Fill;
            _tlpTools.Location = new Point(12, 10);
            _tlpTools.Margin = new Padding(0);
            _tlpTools.Name = "_tlpTools";
            _tlpTools.RowCount = 2;
            _tlpTools.RowStyles.Add(new RowStyle());
            _tlpTools.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            _tlpTools.Size = new Size(1232, 649);
            _tlpTools.TabIndex = 0;
            // 
            // _tlpToolsBar
            // 
            _tlpToolsBar.AutoSize = true;
            _tlpToolsBar.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _tlpToolsBar.ColumnCount = 3;
            _tlpToolsBar.ColumnStyles.Add(new ColumnStyle());
            _tlpToolsBar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            _tlpToolsBar.ColumnStyles.Add(new ColumnStyle());
            _tlpToolsBar.Controls.Add(_lblToolsTitle, 0, 0);
            _tlpToolsBar.Controls.Add(_lblToolsHint, 1, 0);
            _tlpToolsBar.Controls.Add(_btnToolsRefresh, 2, 0);
            _tlpToolsBar.Dock = DockStyle.Fill;
            _tlpToolsBar.Location = new Point(0, 0);
            _tlpToolsBar.Margin = new Padding(0, 0, 0, 8);
            _tlpToolsBar.Name = "_tlpToolsBar";
            _tlpToolsBar.RowCount = 1;
            _tlpToolsBar.RowStyles.Add(new RowStyle());
            _tlpToolsBar.Size = new Size(1232, 36);
            _tlpToolsBar.TabIndex = 0;
            // 
            // _lblToolsTitle
            // 
            _lblToolsTitle.Anchor = AnchorStyles.Left;
            _lblToolsTitle.AutoSize = true;
            _lblToolsTitle.Font = new Font("Microsoft YaHei UI", 10.5F, FontStyle.Bold);
            _lblToolsTitle.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _lblToolsTitle.Location = new Point(0, 8);
            _lblToolsTitle.Margin = new Padding(0, 4, 12, 4);
            _lblToolsTitle.Name = "_lblToolsTitle";
            _lblToolsTitle.Size = new Size(65, 19);
            _lblToolsTitle.TabIndex = 0;
            _lblToolsTitle.Text = "我的模型";
            // 
            // _lblToolsHint
            // 
            _lblToolsHint.Anchor = AnchorStyles.Left;
            _lblToolsHint.AutoSize = true;
            _lblToolsHint.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lblToolsHint.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblToolsHint.Location = new Point(77, 11);
            _lblToolsHint.Margin = new Padding(0, 7, 12, 4);
            _lblToolsHint.Name = "_lblToolsHint";
            _lblToolsHint.Size = new Size(356, 17);
            _lblToolsHint.TabIndex = 1;
            _lblToolsHint.Text = "训练好的模型、数据集、备份和误检样本都在这里，不用记路径。";
            // 
            // _btnToolsRefresh
            // 
            _btnToolsRefresh.Anchor = AnchorStyles.Right;
            _btnToolsRefresh.AutoSize = true;
            _btnToolsRefresh.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnToolsRefresh.BackColor = Color.White;
            _btnToolsRefresh.Cursor = Cursors.Hand;
            _btnToolsRefresh.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnToolsRefresh.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnToolsRefresh.FlatStyle = FlatStyle.Flat;
            _btnToolsRefresh.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnToolsRefresh.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnToolsRefresh.Location = new Point(1148, 2);
            _btnToolsRefresh.Margin = new Padding(0, 2, 0, 2);
            _btnToolsRefresh.MinimumSize = new Size(84, 32);
            _btnToolsRefresh.Name = "_btnToolsRefresh";
            _btnToolsRefresh.Padding = new Padding(10, 0, 10, 0);
            _btnToolsRefresh.Size = new Size(84, 32);
            _btnToolsRefresh.TabIndex = 2;
            _btnToolsRefresh.Text = "刷新";
            _btnToolsRefresh.UseVisualStyleBackColor = false;
            // 
            // _pnlToolsScroll
            // 
            _pnlToolsScroll.AutoScroll = true;
            _pnlToolsScroll.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _pnlToolsScroll.Controls.Add(_tlpToolsStack);
            _pnlToolsScroll.Dock = DockStyle.Fill;
            _pnlToolsScroll.Location = new Point(0, 44);
            _pnlToolsScroll.Margin = new Padding(0);
            _pnlToolsScroll.Name = "_pnlToolsScroll";
            _pnlToolsScroll.Size = new Size(1232, 605);
            _pnlToolsScroll.TabIndex = 1;
            // 
            // _tlpToolsStack
            // 
            _tlpToolsStack.AutoSize = true;
            _tlpToolsStack.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _tlpToolsStack.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _tlpToolsStack.ColumnCount = 1;
            _tlpToolsStack.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            _tlpToolsStack.Controls.Add(_cardToolsFirstRun, 0, 0);
            _tlpToolsStack.Controls.Add(_cardToolsLib, 0, 1);
            _tlpToolsStack.Controls.Add(_cardToolsModel, 0, 2);
            _tlpToolsStack.Controls.Add(_cardToolsModelClasses, 0, 3);
            _tlpToolsStack.Controls.Add(_cardToolsAnchorLib, 0, 4);
            _tlpToolsStack.Controls.Add(_cardToolsMachine, 0, 5);
            _tlpToolsStack.Controls.Add(_cardToolsDataset, 0, 6);
            _tlpToolsStack.Controls.Add(_cardToolsLabels, 0, 7);
            _tlpToolsStack.Controls.Add(_cardToolsBackup, 0, 8);
            _tlpToolsStack.Controls.Add(_cardToolsMisdetect, 0, 9);
            _tlpToolsStack.Dock = DockStyle.Top;
            _tlpToolsStack.Location = new Point(0, 0);
            _tlpToolsStack.Margin = new Padding(0);
            _tlpToolsStack.Name = "_tlpToolsStack";
            _tlpToolsStack.RowCount = 10;
            _tlpToolsStack.RowStyles.Add(new RowStyle());
            _tlpToolsStack.RowStyles.Add(new RowStyle());
            _tlpToolsStack.RowStyles.Add(new RowStyle());
            _tlpToolsStack.RowStyles.Add(new RowStyle());
            _tlpToolsStack.RowStyles.Add(new RowStyle());
            _tlpToolsStack.RowStyles.Add(new RowStyle());
            _tlpToolsStack.RowStyles.Add(new RowStyle());
            _tlpToolsStack.RowStyles.Add(new RowStyle());
            _tlpToolsStack.RowStyles.Add(new RowStyle());
            _tlpToolsStack.RowStyles.Add(new RowStyle());
            _tlpToolsStack.Size = new Size(1215, 2362);
            _tlpToolsStack.TabIndex = 0;
            // 
            // _cardToolsFirstRun
            // 
            _cardToolsFirstRun.AutoSize = true;
            _cardToolsFirstRun.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _cardToolsFirstRun.BackColor = Color.White;
            _cardToolsFirstRun.Controls.Add(_bodyToolsFirstRun);
            _cardToolsFirstRun.Dock = DockStyle.Top;
            _cardToolsFirstRun.Location = new Point(0, 0);
            _cardToolsFirstRun.Margin = new Padding(0, 0, 0, 10);
            _cardToolsFirstRun.Name = "_cardToolsFirstRun";
            _cardToolsFirstRun.Padding = new Padding(14, 12, 14, 12);
            _cardToolsFirstRun.Size = new Size(1215, 341);
            _cardToolsFirstRun.TabIndex = 0;
            _cardToolsFirstRun.Paint += (this.pnlCard_Paint);
            // 
            // _bodyToolsFirstRun
            // 
            _bodyToolsFirstRun.AutoSize = true;
            _bodyToolsFirstRun.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _bodyToolsFirstRun.BackColor = Color.White;
            _bodyToolsFirstRun.ColumnCount = 1;
            _bodyToolsFirstRun.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            _bodyToolsFirstRun.Controls.Add(_lblToolsFirstRunTitle, 0, 0);
            _bodyToolsFirstRun.Controls.Add(_lblToolsFirstRunInfo, 0, 1);
            _bodyToolsFirstRun.Controls.Add(_lvToolsEnv, 0, 2);
            _bodyToolsFirstRun.Controls.Add(_chkToolsEnvTraining, 0, 3);
            _bodyToolsFirstRun.Controls.Add(_flowToolsEnv, 0, 4);
            _bodyToolsFirstRun.Controls.Add(_lblToolsEnvInfo, 0, 5);
            _bodyToolsFirstRun.Dock = DockStyle.Top;
            _bodyToolsFirstRun.Location = new Point(14, 12);
            _bodyToolsFirstRun.Margin = new Padding(0);
            _bodyToolsFirstRun.Name = "_bodyToolsFirstRun";
            _bodyToolsFirstRun.RowCount = 5;
            _bodyToolsFirstRun.RowStyles.Add(new RowStyle());
            _bodyToolsFirstRun.RowStyles.Add(new RowStyle());
            _bodyToolsFirstRun.RowStyles.Add(new RowStyle());
            _bodyToolsFirstRun.RowStyles.Add(new RowStyle());
            _bodyToolsFirstRun.RowStyles.Add(new RowStyle());
            _bodyToolsFirstRun.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            _bodyToolsFirstRun.Size = new Size(1187, 317);
            _bodyToolsFirstRun.TabIndex = 0;
            // 
            // _lblToolsFirstRunTitle
            // 
            _lblToolsFirstRunTitle.AutoSize = true;
            _lblToolsFirstRunTitle.Font = new Font("Microsoft YaHei UI", 10.5F, FontStyle.Bold);
            _lblToolsFirstRunTitle.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _lblToolsFirstRunTitle.Location = new Point(0, 0);
            _lblToolsFirstRunTitle.Margin = new Padding(0, 0, 0, 6);
            _lblToolsFirstRunTitle.Name = "_lblToolsFirstRunTitle";
            _lblToolsFirstRunTitle.Size = new Size(121, 19);
            _lblToolsFirstRunTitle.TabIndex = 0;
            _lblToolsFirstRunTitle.Text = "首次部署 · 换电脑";
            // 
            // _lblToolsFirstRunInfo
            // 
            _lblToolsFirstRunInfo.AutoSize = true;
            _lblToolsFirstRunInfo.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lblToolsFirstRunInfo.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblToolsFirstRunInfo.Location = new Point(0, 25);
            _lblToolsFirstRunInfo.Margin = new Padding(0, 0, 0, 6);
            _lblToolsFirstRunInfo.Name = "_lblToolsFirstRunInfo";
            _lblToolsFirstRunInfo.Size = new Size(680, 17);
            _lblToolsFirstRunInfo.TabIndex = 1;
            _lblToolsFirstRunInfo.Text = "换电脑或第一次启动时用这张卡：检查运行库、目录、模型文件，缺什么一键补齐。日常使用不用管，也不影响标签和数据集。";
            // 
            // _lvToolsEnv
            // 
            _lvToolsEnv.BackColor = Color.White;
            _lvToolsEnv.BorderStyle = BorderStyle.FixedSingle;
            _lvToolsEnv.Dock = DockStyle.Top;
            _lvToolsEnv.Font = new Font("Microsoft YaHei UI", 9.5F);
            _lvToolsEnv.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _lvToolsEnv.FullRowSelect = true;
            _lvToolsEnv.Location = new Point(0, 48);
            _lvToolsEnv.Margin = new Padding(0, 0, 0, 8);
            _lvToolsEnv.MultiSelect = false;
            _lvToolsEnv.Name = "_lvToolsEnv";
            _lvToolsEnv.Size = new Size(1187, 168);
            _lvToolsEnv.TabIndex = 2;
            _lvToolsEnv.UseCompatibleStateImageBehavior = false;
            _lvToolsEnv.View = View.Details;
            // 
            // _chkToolsEnvTraining
            // 
            _chkToolsEnvTraining.AutoSize = true;
            _chkToolsEnvTraining.Checked = true;
            _chkToolsEnvTraining.CheckState = CheckState.Checked;
            _chkToolsEnvTraining.Font = new Font("Microsoft YaHei UI", 9.5F);
            _chkToolsEnvTraining.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _chkToolsEnvTraining.Location = new Point(0, 224);
            _chkToolsEnvTraining.Margin = new Padding(0, 0, 0, 6);
            _chkToolsEnvTraining.Name = "_chkToolsEnvTraining";
            _chkToolsEnvTraining.Size = new Size(611, 23);
            _chkToolsEnvTraining.TabIndex = 3;
            _chkToolsEnvTraining.Text = "初始化时顺带装训练环境（缺 Python 会自动下载安装，再装 ultralytics，首次几分钟；只装不训练）";
            // 
            // _flowToolsEnv
            // 
            _flowToolsEnv.AutoSize = true;
            _flowToolsEnv.BackColor = Color.White;
            _flowToolsEnv.Controls.Add(_btnToolsEnvCheck);
            _flowToolsEnv.Controls.Add(_btnToolsEnvInit);
            _flowToolsEnv.Controls.Add(_btnToolsEnvCamera);
            _flowToolsEnv.Dock = DockStyle.Top;
            _flowToolsEnv.Location = new Point(0, 253);
            _flowToolsEnv.Margin = new Padding(0, 0, 0, 6);
            _flowToolsEnv.Name = "_flowToolsEnv";
            _flowToolsEnv.Size = new Size(1187, 38);
            _flowToolsEnv.TabIndex = 4;
            // 
            // _btnToolsEnvCheck
            // 
            _btnToolsEnvCheck.AutoSize = true;
            _btnToolsEnvCheck.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnToolsEnvCheck.BackColor = Color.FromArgb((int)(byte)36, (int)(byte)112, (int)(byte)210);
            _btnToolsEnvCheck.Cursor = Cursors.Hand;
            _btnToolsEnvCheck.FlatAppearance.BorderSize = 0;
            _btnToolsEnvCheck.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)28, (int)(byte)96, (int)(byte)186);
            _btnToolsEnvCheck.FlatStyle = FlatStyle.Flat;
            _btnToolsEnvCheck.Font = new Font("Microsoft YaHei UI", 9.5F, FontStyle.Bold);
            _btnToolsEnvCheck.ForeColor = Color.White;
            _btnToolsEnvCheck.Location = new Point(0, 2);
            _btnToolsEnvCheck.Margin = new Padding(0, 2, 8, 2);
            _btnToolsEnvCheck.MinimumSize = new Size(136, 34);
            _btnToolsEnvCheck.Name = "_btnToolsEnvCheck";
            _btnToolsEnvCheck.Padding = new Padding(10, 0, 10, 0);
            _btnToolsEnvCheck.Size = new Size(136, 34);
            _btnToolsEnvCheck.TabIndex = 0;
            _btnToolsEnvCheck.Text = "一键检查环境";
            _btnToolsEnvCheck.UseVisualStyleBackColor = false;
            // 
            // _btnToolsEnvInit
            // 
            _btnToolsEnvInit.AutoSize = true;
            _btnToolsEnvInit.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnToolsEnvInit.BackColor = Color.White;
            _btnToolsEnvInit.Cursor = Cursors.Hand;
            _btnToolsEnvInit.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnToolsEnvInit.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnToolsEnvInit.FlatStyle = FlatStyle.Flat;
            _btnToolsEnvInit.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnToolsEnvInit.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnToolsEnvInit.Location = new Point(144, 2);
            _btnToolsEnvInit.Margin = new Padding(0, 2, 8, 2);
            _btnToolsEnvInit.MinimumSize = new Size(136, 34);
            _btnToolsEnvInit.Name = "_btnToolsEnvInit";
            _btnToolsEnvInit.Padding = new Padding(10, 0, 10, 0);
            _btnToolsEnvInit.Size = new Size(136, 34);
            _btnToolsEnvInit.TabIndex = 1;
            _btnToolsEnvInit.Text = "一键初始化";
            _btnToolsEnvInit.UseVisualStyleBackColor = false;
            // 
            // _btnToolsEnvCamera
            // 
            _btnToolsEnvCamera.AutoSize = true;
            _btnToolsEnvCamera.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnToolsEnvCamera.BackColor = Color.White;
            _btnToolsEnvCamera.Cursor = Cursors.Hand;
            _btnToolsEnvCamera.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnToolsEnvCamera.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnToolsEnvCamera.FlatStyle = FlatStyle.Flat;
            _btnToolsEnvCamera.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnToolsEnvCamera.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnToolsEnvCamera.Location = new Point(288, 2);
            _btnToolsEnvCamera.Margin = new Padding(0, 2, 8, 2);
            _btnToolsEnvCamera.MinimumSize = new Size(128, 34);
            _btnToolsEnvCamera.Name = "_btnToolsEnvCamera";
            _btnToolsEnvCamera.Padding = new Padding(10, 0, 10, 0);
            _btnToolsEnvCamera.Size = new Size(128, 34);
            _btnToolsEnvCamera.TabIndex = 2;
            _btnToolsEnvCamera.Text = "同时探测相机";
            _btnToolsEnvCamera.UseVisualStyleBackColor = false;
            // 
            // _lblToolsEnvInfo
            // 
            _lblToolsEnvInfo.AutoSize = true;
            _lblToolsEnvInfo.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lblToolsEnvInfo.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblToolsEnvInfo.Location = new Point(0, 297);
            _lblToolsEnvInfo.Margin = new Padding(0, 0, 0, 2);
            _lblToolsEnvInfo.Name = "_lblToolsEnvInfo";
            _lblToolsEnvInfo.Size = new Size(364, 17);
            _lblToolsEnvInfo.TabIndex = 5;
            _lblToolsEnvInfo.Text = "还没检查。换电脑后先点“一键检查环境”，缺项的点“一键初始化”。";
            // 
            // _cardToolsLib
            // 
            _cardToolsLib.AutoSize = true;
            _cardToolsLib.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _cardToolsLib.BackColor = Color.White;
            _cardToolsLib.Controls.Add(_bodyToolsLib);
            _cardToolsLib.Dock = DockStyle.Top;
            _cardToolsLib.Location = new Point(0, 351);
            _cardToolsLib.Margin = new Padding(0, 0, 0, 10);
            _cardToolsLib.Name = "_cardToolsLib";
            _cardToolsLib.Padding = new Padding(14, 12, 14, 12);
            _cardToolsLib.Size = new Size(1215, 336);
            _cardToolsLib.TabIndex = 1;
            _cardToolsLib.Paint += (this.pnlCard_Paint);
            // 
            // _bodyToolsLib
            // 
            _bodyToolsLib.AutoSize = true;
            _bodyToolsLib.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _bodyToolsLib.BackColor = Color.White;
            _bodyToolsLib.ColumnCount = 1;
            _bodyToolsLib.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            _bodyToolsLib.Controls.Add(_lblToolsLibTitle, 0, 0);
            _bodyToolsLib.Controls.Add(_lblToolsLibInfo, 0, 1);
            _bodyToolsLib.Controls.Add(_tlpToolsLibPick, 0, 2);
            _bodyToolsLib.Controls.Add(_lvToolsLibModels, 0, 3);
            _bodyToolsLib.Controls.Add(_flowToolsLib, 0, 4);
            _bodyToolsLib.Controls.Add(_lblToolsLibDir, 0, 5);
            _bodyToolsLib.Dock = DockStyle.Top;
            _bodyToolsLib.Location = new Point(14, 12);
            _bodyToolsLib.Margin = new Padding(0);
            _bodyToolsLib.Name = "_bodyToolsLib";
            _bodyToolsLib.RowCount = 5;
            _bodyToolsLib.RowStyles.Add(new RowStyle());
            _bodyToolsLib.RowStyles.Add(new RowStyle());
            _bodyToolsLib.RowStyles.Add(new RowStyle());
            _bodyToolsLib.RowStyles.Add(new RowStyle());
            _bodyToolsLib.RowStyles.Add(new RowStyle());
            _bodyToolsLib.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            _bodyToolsLib.Size = new Size(1187, 312);
            _bodyToolsLib.TabIndex = 0;
            // 
            // _lblToolsLibTitle
            // 
            _lblToolsLibTitle.AutoSize = true;
            _lblToolsLibTitle.Font = new Font("Microsoft YaHei UI", 10.5F, FontStyle.Bold);
            _lblToolsLibTitle.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _lblToolsLibTitle.Location = new Point(0, 0);
            _lblToolsLibTitle.Margin = new Padding(0, 0, 0, 6);
            _lblToolsLibTitle.Name = "_lblToolsLibTitle";
            _lblToolsLibTitle.Size = new Size(205, 19);
            _lblToolsLibTitle.TabIndex = 0;
            _lblToolsLibTitle.Text = "模型库（磁盘上现有的都在这）";
            // 
            // _lblToolsLibInfo
            // 
            _lblToolsLibInfo.AutoSize = true;
            _lblToolsLibInfo.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lblToolsLibInfo.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblToolsLibInfo.Location = new Point(0, 25);
            _lblToolsLibInfo.Margin = new Padding(0, 0, 0, 6);
            _lblToolsLibInfo.Name = "_lblToolsLibInfo";
            _lblToolsLibInfo.Size = new Size(477, 17);
            _lblToolsLibInfo.TabIndex = 1;
            _lblToolsLibInfo.Text = "选一个类别，看这一类的模型 / 学习库现在有哪些。选中的 onnx 可以直接切过去识别。";
            // 
            // _tlpToolsLibPick
            // 
            _tlpToolsLibPick.AutoSize = true;
            _tlpToolsLibPick.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _tlpToolsLibPick.ColumnCount = 2;
            _tlpToolsLibPick.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            _tlpToolsLibPick.ColumnStyles.Add(new ColumnStyle());
            _tlpToolsLibPick.Controls.Add(_cboToolsLibCategory, 0, 0);
            _tlpToolsLibPick.Controls.Add(_btnToolsLibFolderOpen, 1, 0);
            _tlpToolsLibPick.Dock = DockStyle.Top;
            _tlpToolsLibPick.Location = new Point(0, 48);
            _tlpToolsLibPick.Margin = new Padding(0, 0, 0, 6);
            _tlpToolsLibPick.Name = "_tlpToolsLibPick";
            _tlpToolsLibPick.RowCount = 1;
            _tlpToolsLibPick.RowStyles.Add(new RowStyle());
            _tlpToolsLibPick.Size = new Size(1187, 36);
            _tlpToolsLibPick.TabIndex = 2;
            // 
            // _cboToolsLibCategory
            // 
            _cboToolsLibCategory.Dock = DockStyle.Fill;
            _cboToolsLibCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            _cboToolsLibCategory.Font = new Font("Microsoft YaHei UI", 9.5F);
            _cboToolsLibCategory.Location = new Point(0, 2);
            _cboToolsLibCategory.Margin = new Padding(0, 2, 8, 2);
            _cboToolsLibCategory.Name = "_cboToolsLibCategory";
            _cboToolsLibCategory.Size = new Size(1043, 27);
            _cboToolsLibCategory.TabIndex = 0;
            // 
            // _btnToolsLibFolderOpen
            // 
            _btnToolsLibFolderOpen.AutoSize = true;
            _btnToolsLibFolderOpen.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnToolsLibFolderOpen.BackColor = Color.White;
            _btnToolsLibFolderOpen.Cursor = Cursors.Hand;
            _btnToolsLibFolderOpen.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnToolsLibFolderOpen.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnToolsLibFolderOpen.FlatStyle = FlatStyle.Flat;
            _btnToolsLibFolderOpen.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnToolsLibFolderOpen.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnToolsLibFolderOpen.Location = new Point(1051, 2);
            _btnToolsLibFolderOpen.Margin = new Padding(0, 2, 8, 2);
            _btnToolsLibFolderOpen.MinimumSize = new Size(128, 32);
            _btnToolsLibFolderOpen.Name = "_btnToolsLibFolderOpen";
            _btnToolsLibFolderOpen.Padding = new Padding(10, 0, 10, 0);
            _btnToolsLibFolderOpen.Size = new Size(128, 32);
            _btnToolsLibFolderOpen.TabIndex = 1;
            _btnToolsLibFolderOpen.Text = "打开所在目录";
            _btnToolsLibFolderOpen.UseVisualStyleBackColor = false;
            // 
            // _lvToolsLibModels
            // 
            _lvToolsLibModels.BackColor = Color.White;
            _lvToolsLibModels.BorderStyle = BorderStyle.FixedSingle;
            _lvToolsLibModels.Dock = DockStyle.Top;
            _lvToolsLibModels.Font = new Font("Microsoft YaHei UI", 9.5F);
            _lvToolsLibModels.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _lvToolsLibModels.FullRowSelect = true;
            _lvToolsLibModels.Location = new Point(0, 90);
            _lvToolsLibModels.Margin = new Padding(0, 0, 0, 8);
            _lvToolsLibModels.MultiSelect = false;
            _lvToolsLibModels.Name = "_lvToolsLibModels";
            _lvToolsLibModels.Size = new Size(1187, 150);
            _lvToolsLibModels.TabIndex = 3;
            _lvToolsLibModels.UseCompatibleStateImageBehavior = false;
            _lvToolsLibModels.View = View.Details;
            // 
            // _flowToolsLib
            // 
            _flowToolsLib.AutoSize = true;
            _flowToolsLib.BackColor = Color.White;
            _flowToolsLib.Controls.Add(_btnToolsLibUse);
            _flowToolsLib.Controls.Add(_btnToolsLibOpen);
            _flowToolsLib.Dock = DockStyle.Top;
            _flowToolsLib.Location = new Point(0, 248);
            _flowToolsLib.Margin = new Padding(0, 0, 0, 6);
            _flowToolsLib.Name = "_flowToolsLib";
            _flowToolsLib.Size = new Size(1187, 38);
            _flowToolsLib.TabIndex = 4;
            // 
            // _btnToolsLibUse
            // 
            _btnToolsLibUse.AutoSize = true;
            _btnToolsLibUse.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnToolsLibUse.BackColor = Color.FromArgb((int)(byte)36, (int)(byte)112, (int)(byte)210);
            _btnToolsLibUse.Cursor = Cursors.Hand;
            _btnToolsLibUse.FlatAppearance.BorderSize = 0;
            _btnToolsLibUse.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)28, (int)(byte)96, (int)(byte)186);
            _btnToolsLibUse.FlatStyle = FlatStyle.Flat;
            _btnToolsLibUse.Font = new Font("Microsoft YaHei UI", 9.5F, FontStyle.Bold);
            _btnToolsLibUse.ForeColor = Color.White;
            _btnToolsLibUse.Location = new Point(0, 2);
            _btnToolsLibUse.Margin = new Padding(0, 2, 8, 2);
            _btnToolsLibUse.MinimumSize = new Size(150, 34);
            _btnToolsLibUse.Name = "_btnToolsLibUse";
            _btnToolsLibUse.Padding = new Padding(10, 0, 10, 0);
            _btnToolsLibUse.Size = new Size(154, 34);
            _btnToolsLibUse.TabIndex = 0;
            _btnToolsLibUse.Text = "用这个 ONNX 识别";
            _btnToolsLibUse.UseVisualStyleBackColor = false;
            // 
            // _btnToolsLibOpen
            // 
            _btnToolsLibOpen.AutoSize = true;
            _btnToolsLibOpen.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnToolsLibOpen.BackColor = Color.White;
            _btnToolsLibOpen.Cursor = Cursors.Hand;
            _btnToolsLibOpen.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnToolsLibOpen.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnToolsLibOpen.FlatStyle = FlatStyle.Flat;
            _btnToolsLibOpen.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnToolsLibOpen.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnToolsLibOpen.Location = new Point(162, 2);
            _btnToolsLibOpen.Margin = new Padding(0, 2, 8, 2);
            _btnToolsLibOpen.MinimumSize = new Size(128, 34);
            _btnToolsLibOpen.Name = "_btnToolsLibOpen";
            _btnToolsLibOpen.Padding = new Padding(10, 0, 10, 0);
            _btnToolsLibOpen.Size = new Size(128, 34);
            _btnToolsLibOpen.TabIndex = 1;
            _btnToolsLibOpen.Text = "打开所在目录";
            _btnToolsLibOpen.UseVisualStyleBackColor = false;
            // 
            // _lblToolsLibDir
            // 
            _lblToolsLibDir.AutoSize = true;
            _lblToolsLibDir.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lblToolsLibDir.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblToolsLibDir.Location = new Point(0, 292);
            _lblToolsLibDir.Margin = new Padding(0, 0, 0, 2);
            _lblToolsLibDir.Name = "_lblToolsLibDir";
            _lblToolsLibDir.Size = new Size(0, 17);
            _lblToolsLibDir.TabIndex = 5;
            // 
            // _cardToolsModel
            // 
            _cardToolsModel.AutoSize = true;
            _cardToolsModel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _cardToolsModel.BackColor = Color.White;
            _cardToolsModel.Controls.Add(_bodyToolsModel);
            _cardToolsModel.Dock = DockStyle.Top;
            _cardToolsModel.Location = new Point(0, 697);
            _cardToolsModel.Margin = new Padding(0, 0, 0, 10);
            _cardToolsModel.Name = "_cardToolsModel";
            _cardToolsModel.Padding = new Padding(14, 12, 14, 12);
            _cardToolsModel.Size = new Size(1215, 157);
            _cardToolsModel.TabIndex = 2;
            _cardToolsModel.Paint += (this.pnlCard_Paint);
            // 
            // _bodyToolsModel
            // 
            _bodyToolsModel.AutoSize = true;
            _bodyToolsModel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _bodyToolsModel.BackColor = Color.White;
            _bodyToolsModel.ColumnCount = 1;
            _bodyToolsModel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            _bodyToolsModel.Controls.Add(_lblToolsModelTitle, 0, 0);
            _bodyToolsModel.Controls.Add(_lblToolsModelMain, 0, 1);
            _bodyToolsModel.Controls.Add(_lblToolsModelMeta, 0, 2);
            _bodyToolsModel.Controls.Add(_lblToolsModelClasses, 0, 3);
            _bodyToolsModel.Controls.Add(_lblToolsModelTrain, 0, 4);
            _bodyToolsModel.Controls.Add(_flowToolsModel, 0, 5);
            _bodyToolsModel.Dock = DockStyle.Top;
            _bodyToolsModel.Location = new Point(14, 12);
            _bodyToolsModel.Margin = new Padding(0);
            _bodyToolsModel.Name = "_bodyToolsModel";
            _bodyToolsModel.RowCount = 5;
            _bodyToolsModel.RowStyles.Add(new RowStyle());
            _bodyToolsModel.RowStyles.Add(new RowStyle());
            _bodyToolsModel.RowStyles.Add(new RowStyle());
            _bodyToolsModel.RowStyles.Add(new RowStyle());
            _bodyToolsModel.RowStyles.Add(new RowStyle());
            _bodyToolsModel.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            _bodyToolsModel.Size = new Size(1187, 133);
            _bodyToolsModel.TabIndex = 0;
            // 
            // _lblToolsModelTitle
            // 
            _lblToolsModelTitle.AutoSize = true;
            _lblToolsModelTitle.Font = new Font("Microsoft YaHei UI", 10.5F, FontStyle.Bold);
            _lblToolsModelTitle.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _lblToolsModelTitle.Location = new Point(0, 0);
            _lblToolsModelTitle.Margin = new Padding(0, 0, 0, 6);
            _lblToolsModelTitle.Name = "_lblToolsModelTitle";
            _lblToolsModelTitle.Size = new Size(65, 19);
            _lblToolsModelTitle.TabIndex = 0;
            _lblToolsModelTitle.Text = "训练模型";
            // 
            // _lblToolsModelMain
            // 
            _lblToolsModelMain.AutoSize = true;
            _lblToolsModelMain.Font = new Font("Microsoft YaHei UI", 9.5F, FontStyle.Bold);
            _lblToolsModelMain.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _lblToolsModelMain.Location = new Point(0, 25);
            _lblToolsModelMain.Margin = new Padding(0, 0, 0, 6);
            _lblToolsModelMain.Name = "_lblToolsModelMain";
            _lblToolsModelMain.Size = new Size(87, 19);
            _lblToolsModelMain.TabIndex = 1;
            _lblToolsModelMain.Text = "尚未训练模型";
            // 
            // _lblToolsModelMeta
            // 
            _lblToolsModelMeta.AutoSize = true;
            _lblToolsModelMeta.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lblToolsModelMeta.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblToolsModelMeta.Location = new Point(0, 52);
            _lblToolsModelMeta.Margin = new Padding(0, 2, 0, 2);
            _lblToolsModelMeta.Name = "_lblToolsModelMeta";
            _lblToolsModelMeta.Size = new Size(0, 17);
            _lblToolsModelMeta.TabIndex = 2;
            // 
            // _lblToolsModelClasses
            // 
            _lblToolsModelClasses.AutoSize = true;
            _lblToolsModelClasses.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lblToolsModelClasses.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblToolsModelClasses.Location = new Point(0, 73);
            _lblToolsModelClasses.Margin = new Padding(0, 2, 0, 2);
            _lblToolsModelClasses.Name = "_lblToolsModelClasses";
            _lblToolsModelClasses.Size = new Size(0, 17);
            _lblToolsModelClasses.TabIndex = 3;
            // 
            // _lblToolsModelTrain
            // 
            _lblToolsModelTrain.AutoSize = true;
            _lblToolsModelTrain.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lblToolsModelTrain.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblToolsModelTrain.Location = new Point(0, 94);
            _lblToolsModelTrain.Margin = new Padding(0, 2, 0, 2);
            _lblToolsModelTrain.Name = "_lblToolsModelTrain";
            _lblToolsModelTrain.Size = new Size(0, 17);
            _lblToolsModelTrain.TabIndex = 4;
            // 
            // _flowToolsModel
            // 
            _flowToolsModel.AutoSize = true;
            _flowToolsModel.BackColor = Color.White;
            _flowToolsModel.Controls.Add(_btnToolsUseModel);
            _flowToolsModel.Controls.Add(_btnToolsModelBackup);
            _flowToolsModel.Controls.Add(_btnToolsModelFolder);
            _flowToolsModel.Dock = DockStyle.Top;
            _flowToolsModel.Location = new Point(0, 121);
            _flowToolsModel.Margin = new Padding(0, 8, 0, 0);
            _flowToolsModel.Name = "_flowToolsModel";
            _flowToolsModel.Size = new Size(1187, 12);
            _flowToolsModel.TabIndex = 5;
            _flowToolsModel.WrapContents = false;
            // 
            // _btnToolsUseModel
            // 
            _btnToolsUseModel.AutoSize = true;
            _btnToolsUseModel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnToolsUseModel.BackColor = Color.FromArgb((int)(byte)36, (int)(byte)112, (int)(byte)210);
            _btnToolsUseModel.Cursor = Cursors.Hand;
            _btnToolsUseModel.FlatAppearance.BorderSize = 0;
            _btnToolsUseModel.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)28, (int)(byte)96, (int)(byte)186);
            _btnToolsUseModel.FlatStyle = FlatStyle.Flat;
            _btnToolsUseModel.Font = new Font("Microsoft YaHei UI", 9.5F, FontStyle.Bold);
            _btnToolsUseModel.ForeColor = Color.White;
            _btnToolsUseModel.Location = new Point(0, 2);
            _btnToolsUseModel.Margin = new Padding(0, 2, 8, 2);
            _btnToolsUseModel.MinimumSize = new Size(140, 34);
            _btnToolsUseModel.Name = "_btnToolsUseModel";
            _btnToolsUseModel.Padding = new Padding(10, 0, 10, 0);
            _btnToolsUseModel.Size = new Size(140, 34);
            _btnToolsUseModel.TabIndex = 0;
            _btnToolsUseModel.Text = "用这个模型识别";
            _btnToolsUseModel.UseVisualStyleBackColor = false;
            // 
            // _btnToolsModelBackup
            // 
            _btnToolsModelBackup.AutoSize = true;
            _btnToolsModelBackup.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnToolsModelBackup.BackColor = Color.White;
            _btnToolsModelBackup.Cursor = Cursors.Hand;
            _btnToolsModelBackup.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnToolsModelBackup.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnToolsModelBackup.FlatStyle = FlatStyle.Flat;
            _btnToolsModelBackup.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnToolsModelBackup.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnToolsModelBackup.Location = new Point(148, 2);
            _btnToolsModelBackup.Margin = new Padding(0, 2, 8, 2);
            _btnToolsModelBackup.MinimumSize = new Size(120, 34);
            _btnToolsModelBackup.Name = "_btnToolsModelBackup";
            _btnToolsModelBackup.Padding = new Padding(10, 0, 10, 0);
            _btnToolsModelBackup.Size = new Size(120, 34);
            _btnToolsModelBackup.TabIndex = 1;
            _btnToolsModelBackup.Text = "备份当前模型";
            _btnToolsModelBackup.UseVisualStyleBackColor = false;
            // 
            // _btnToolsModelFolder
            // 
            _btnToolsModelFolder.AutoSize = true;
            _btnToolsModelFolder.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnToolsModelFolder.BackColor = Color.White;
            _btnToolsModelFolder.Cursor = Cursors.Hand;
            _btnToolsModelFolder.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnToolsModelFolder.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnToolsModelFolder.FlatStyle = FlatStyle.Flat;
            _btnToolsModelFolder.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnToolsModelFolder.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnToolsModelFolder.Location = new Point(276, 2);
            _btnToolsModelFolder.Margin = new Padding(0, 2, 8, 2);
            _btnToolsModelFolder.MinimumSize = new Size(130, 34);
            _btnToolsModelFolder.Name = "_btnToolsModelFolder";
            _btnToolsModelFolder.Padding = new Padding(10, 0, 10, 0);
            _btnToolsModelFolder.Size = new Size(132, 34);
            _btnToolsModelFolder.TabIndex = 2;
            _btnToolsModelFolder.Text = "打开模型文件夹";
            _btnToolsModelFolder.UseVisualStyleBackColor = false;
            // 
            // _cardToolsModelClasses
            // 
            _cardToolsModelClasses.AutoSize = true;
            _cardToolsModelClasses.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _cardToolsModelClasses.BackColor = Color.White;
            _cardToolsModelClasses.Controls.Add(_bodyToolsModelClasses);
            _cardToolsModelClasses.Dock = DockStyle.Top;
            _cardToolsModelClasses.Location = new Point(0, 864);
            _cardToolsModelClasses.Margin = new Padding(0, 0, 0, 10);
            _cardToolsModelClasses.Name = "_cardToolsModelClasses";
            _cardToolsModelClasses.Padding = new Padding(14, 12, 14, 12);
            _cardToolsModelClasses.Size = new Size(1215, 315);
            _cardToolsModelClasses.TabIndex = 3;
            _cardToolsModelClasses.Paint += (this.pnlCard_Paint);
            // 
            // _bodyToolsModelClasses
            // 
            _bodyToolsModelClasses.AutoSize = true;
            _bodyToolsModelClasses.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _bodyToolsModelClasses.BackColor = Color.White;
            _bodyToolsModelClasses.ColumnCount = 1;
            _bodyToolsModelClasses.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            _bodyToolsModelClasses.Controls.Add(_lblToolsMcTitle, 0, 0);
            _bodyToolsModelClasses.Controls.Add(_lblToolsMcInfo, 0, 1);
            _bodyToolsModelClasses.Controls.Add(_gridToolsMc, 0, 2);
            _bodyToolsModelClasses.Controls.Add(_lblToolsMcHint, 0, 3);
            _bodyToolsModelClasses.Controls.Add(_flowToolsMc, 0, 4);
            _bodyToolsModelClasses.Dock = DockStyle.Top;
            _bodyToolsModelClasses.Location = new Point(14, 12);
            _bodyToolsModelClasses.Margin = new Padding(0);
            _bodyToolsModelClasses.Name = "_bodyToolsModelClasses";
            _bodyToolsModelClasses.RowCount = 4;
            _bodyToolsModelClasses.RowStyles.Add(new RowStyle());
            _bodyToolsModelClasses.RowStyles.Add(new RowStyle());
            _bodyToolsModelClasses.RowStyles.Add(new RowStyle());
            _bodyToolsModelClasses.RowStyles.Add(new RowStyle());
            _bodyToolsModelClasses.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            _bodyToolsModelClasses.Size = new Size(1187, 291);
            _bodyToolsModelClasses.TabIndex = 0;
            // 
            // _lblToolsMcTitle
            // 
            _lblToolsMcTitle.AutoSize = true;
            _lblToolsMcTitle.Font = new Font("Microsoft YaHei UI", 10.5F, FontStyle.Bold);
            _lblToolsMcTitle.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _lblToolsMcTitle.Location = new Point(0, 0);
            _lblToolsMcTitle.Margin = new Padding(0, 0, 0, 6);
            _lblToolsMcTitle.Name = "_lblToolsMcTitle";
            _lblToolsMcTitle.Size = new Size(135, 19);
            _lblToolsMcTitle.TabIndex = 0;
            _lblToolsMcTitle.Text = "模型标签（识别用）";
            // 
            // _lblToolsMcInfo
            // 
            _lblToolsMcInfo.AutoSize = true;
            _lblToolsMcInfo.Font = new Font("Microsoft YaHei UI", 9.5F);
            _lblToolsMcInfo.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _lblToolsMcInfo.Location = new Point(0, 25);
            _lblToolsMcInfo.Margin = new Padding(0, 0, 0, 4);
            _lblToolsMcInfo.Name = "_lblToolsMcInfo";
            _lblToolsMcInfo.Size = new Size(139, 19);
            _lblToolsMcInfo.TabIndex = 1;
            _lblToolsMcInfo.Text = "还没有模型类别信息。";
            // 
            // _gridToolsMc
            // 
            _gridToolsMc.Columns.AddRange(new DataGridViewColumn[] { dataGridViewTextBoxColumn1, dataGridViewTextBoxColumn2, dataGridViewTextBoxColumn3, dataGridViewTextBoxColumn4, dataGridViewTextBoxColumn5, dataGridViewTextBoxColumn6, dataGridViewTextBoxColumn7 });
            _gridToolsMc.Dock = DockStyle.Top;
            _gridToolsMc.Location = new Point(0, 48);
            _gridToolsMc.Margin = new Padding(0, 0, 0, 6);
            _gridToolsMc.Name = "_gridToolsMc";
            _gridToolsMc.Size = new Size(1187, 196);
            _gridToolsMc.TabIndex = 2;
            // 
            // dataGridViewTextBoxColumn1
            // 
            dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // dataGridViewTextBoxColumn2
            // 
            dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // dataGridViewTextBoxColumn3
            // 
            dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            // 
            // dataGridViewTextBoxColumn4
            // 
            dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            // 
            // dataGridViewTextBoxColumn5
            // 
            dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            // 
            // dataGridViewTextBoxColumn6
            // 
            dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            // 
            // dataGridViewTextBoxColumn7
            // 
            dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            // 
            // _lblToolsMcHint
            // 
            _lblToolsMcHint.AutoSize = true;
            _lblToolsMcHint.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lblToolsMcHint.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblToolsMcHint.Location = new Point(0, 252);
            _lblToolsMcHint.Margin = new Padding(0, 2, 0, 2);
            _lblToolsMcHint.Name = "_lblToolsMcHint";
            _lblToolsMcHint.Size = new Size(700, 17);
            _lblToolsMcHint.TabIndex = 3;
            _lblToolsMcHint.Text = "显示名只影响界面；存储名会改 classes.txt 和该类的所有标注框。模型文件不会自动变——改完要重新训练，模型里才认新名字。";
            // 
            // _flowToolsMc
            // 
            _flowToolsMc.AutoSize = true;
            _flowToolsMc.BackColor = Color.White;
            _flowToolsMc.Controls.Add(_btnToolsMcRename);
            _flowToolsMc.Controls.Add(_btnToolsMcDelete);
            _flowToolsMc.Controls.Add(_btnToolsMcMute);
            _flowToolsMc.Controls.Add(_btnToolsMcTrain);
            _flowToolsMc.Dock = DockStyle.Top;
            _flowToolsMc.Location = new Point(0, 277);
            _flowToolsMc.Margin = new Padding(0, 6, 0, 0);
            _flowToolsMc.Name = "_flowToolsMc";
            _flowToolsMc.Size = new Size(1187, 14);
            _flowToolsMc.TabIndex = 4;
            // 
            // _btnToolsMcRename
            // 
            _btnToolsMcRename.AutoSize = true;
            _btnToolsMcRename.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnToolsMcRename.BackColor = Color.FromArgb((int)(byte)36, (int)(byte)112, (int)(byte)210);
            _btnToolsMcRename.Cursor = Cursors.Hand;
            _btnToolsMcRename.FlatAppearance.BorderSize = 0;
            _btnToolsMcRename.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)28, (int)(byte)96, (int)(byte)186);
            _btnToolsMcRename.FlatStyle = FlatStyle.Flat;
            _btnToolsMcRename.Font = new Font("Microsoft YaHei UI", 9.5F, FontStyle.Bold);
            _btnToolsMcRename.ForeColor = Color.White;
            _btnToolsMcRename.Location = new Point(0, 2);
            _btnToolsMcRename.Margin = new Padding(0, 2, 8, 2);
            _btnToolsMcRename.MinimumSize = new Size(132, 34);
            _btnToolsMcRename.Name = "_btnToolsMcRename";
            _btnToolsMcRename.Padding = new Padding(10, 0, 10, 0);
            _btnToolsMcRename.Size = new Size(132, 34);
            _btnToolsMcRename.TabIndex = 0;
            _btnToolsMcRename.Text = "修改标签名";
            _btnToolsMcRename.UseVisualStyleBackColor = false;
            // 
            // _btnToolsMcDelete
            // 
            _btnToolsMcDelete.AutoSize = true;
            _btnToolsMcDelete.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnToolsMcDelete.BackColor = Color.White;
            _btnToolsMcDelete.Cursor = Cursors.Hand;
            _btnToolsMcDelete.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnToolsMcDelete.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnToolsMcDelete.FlatStyle = FlatStyle.Flat;
            _btnToolsMcDelete.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnToolsMcDelete.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnToolsMcDelete.Location = new Point(140, 2);
            _btnToolsMcDelete.Margin = new Padding(0, 2, 8, 2);
            _btnToolsMcDelete.MinimumSize = new Size(130, 34);
            _btnToolsMcDelete.Name = "_btnToolsMcDelete";
            _btnToolsMcDelete.Padding = new Padding(10, 0, 10, 0);
            _btnToolsMcDelete.Size = new Size(130, 34);
            _btnToolsMcDelete.TabIndex = 1;
            _btnToolsMcDelete.Text = "从数据集删除";
            _btnToolsMcDelete.UseVisualStyleBackColor = false;
            // 
            // _btnToolsMcMute
            // 
            _btnToolsMcMute.AutoSize = true;
            _btnToolsMcMute.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnToolsMcMute.BackColor = Color.White;
            _btnToolsMcMute.Cursor = Cursors.Hand;
            _btnToolsMcMute.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnToolsMcMute.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnToolsMcMute.FlatStyle = FlatStyle.Flat;
            _btnToolsMcMute.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnToolsMcMute.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnToolsMcMute.Location = new Point(278, 2);
            _btnToolsMcMute.Margin = new Padding(0, 2, 8, 2);
            _btnToolsMcMute.MinimumSize = new Size(110, 34);
            _btnToolsMcMute.Name = "_btnToolsMcMute";
            _btnToolsMcMute.Padding = new Padding(10, 0, 10, 0);
            _btnToolsMcMute.Size = new Size(110, 34);
            _btnToolsMcMute.TabIndex = 2;
            _btnToolsMcMute.Text = "加入屏蔽";
            _btnToolsMcMute.UseVisualStyleBackColor = false;
            // 
            // _btnToolsMcTrain
            // 
            _btnToolsMcTrain.AutoSize = true;
            _btnToolsMcTrain.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnToolsMcTrain.BackColor = Color.White;
            _btnToolsMcTrain.Cursor = Cursors.Hand;
            _btnToolsMcTrain.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnToolsMcTrain.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnToolsMcTrain.FlatStyle = FlatStyle.Flat;
            _btnToolsMcTrain.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnToolsMcTrain.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnToolsMcTrain.Location = new Point(396, 2);
            _btnToolsMcTrain.Margin = new Padding(0, 2, 8, 2);
            _btnToolsMcTrain.MinimumSize = new Size(140, 34);
            _btnToolsMcTrain.Name = "_btnToolsMcTrain";
            _btnToolsMcTrain.Padding = new Padding(10, 0, 10, 0);
            _btnToolsMcTrain.Size = new Size(140, 34);
            _btnToolsMcTrain.TabIndex = 3;
            _btnToolsMcTrain.Text = "再训练并部署";
            _btnToolsMcTrain.UseVisualStyleBackColor = false;
            // 
            // _cardToolsAnchorLib
            // 
            _cardToolsAnchorLib.AutoSize = true;
            _cardToolsAnchorLib.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _cardToolsAnchorLib.BackColor = Color.White;
            _cardToolsAnchorLib.Controls.Add(_bodyToolsAnchorLib);
            _cardToolsAnchorLib.Dock = DockStyle.Top;
            _cardToolsAnchorLib.Location = new Point(0, 1189);
            _cardToolsAnchorLib.Margin = new Padding(0, 0, 0, 10);
            _cardToolsAnchorLib.Name = "_cardToolsAnchorLib";
            _cardToolsAnchorLib.Padding = new Padding(14, 12, 14, 12);
            _cardToolsAnchorLib.Size = new Size(1215, 148);
            _cardToolsAnchorLib.TabIndex = 4;
            _cardToolsAnchorLib.Paint += (this.pnlCard_Paint);
            // 
            // _bodyToolsAnchorLib
            // 
            _bodyToolsAnchorLib.AutoSize = true;
            _bodyToolsAnchorLib.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _bodyToolsAnchorLib.BackColor = Color.White;
            _bodyToolsAnchorLib.ColumnCount = 1;
            _bodyToolsAnchorLib.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            _bodyToolsAnchorLib.Controls.Add(_lblToolsAnchorLibTitle, 0, 0);
            _bodyToolsAnchorLib.Controls.Add(_tlpToolsAnchorPick, 0, 1);
            _bodyToolsAnchorLib.Controls.Add(_lblToolsAnchorModelInfo, 0, 2);
            _bodyToolsAnchorLib.Controls.Add(_lblToolsAnchorCalibInfo, 0, 3);
            _bodyToolsAnchorLib.Controls.Add(_flowToolsAnchorLib, 0, 4);
            _bodyToolsAnchorLib.Dock = DockStyle.Top;
            _bodyToolsAnchorLib.Location = new Point(14, 12);
            _bodyToolsAnchorLib.Margin = new Padding(0);
            _bodyToolsAnchorLib.Name = "_bodyToolsAnchorLib";
            _bodyToolsAnchorLib.RowCount = 4;
            _bodyToolsAnchorLib.RowStyles.Add(new RowStyle());
            _bodyToolsAnchorLib.RowStyles.Add(new RowStyle());
            _bodyToolsAnchorLib.RowStyles.Add(new RowStyle());
            _bodyToolsAnchorLib.RowStyles.Add(new RowStyle());
            _bodyToolsAnchorLib.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            _bodyToolsAnchorLib.Size = new Size(1187, 124);
            _bodyToolsAnchorLib.TabIndex = 0;
            // 
            // _lblToolsAnchorLibTitle
            // 
            _lblToolsAnchorLibTitle.AutoSize = true;
            _lblToolsAnchorLibTitle.Font = new Font("Microsoft YaHei UI", 10.5F, FontStyle.Bold);
            _lblToolsAnchorLibTitle.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _lblToolsAnchorLibTitle.Location = new Point(0, 0);
            _lblToolsAnchorLibTitle.Margin = new Padding(0, 0, 0, 6);
            _lblToolsAnchorLibTitle.Name = "_lblToolsAnchorLibTitle";
            _lblToolsAnchorLibTitle.Size = new Size(168, 19);
            _lblToolsAnchorLibTitle.TabIndex = 0;
            _lblToolsAnchorLibTitle.Text = "针床定位V2 · 学习库管理";
            // 
            // _tlpToolsAnchorPick
            // 
            _tlpToolsAnchorPick.AutoSize = true;
            _tlpToolsAnchorPick.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _tlpToolsAnchorPick.ColumnCount = 2;
            _tlpToolsAnchorPick.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 220F));
            _tlpToolsAnchorPick.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            _tlpToolsAnchorPick.Controls.Add(_cboToolsAnchorModel, 0, 0);
            _tlpToolsAnchorPick.Controls.Add(_lblToolsAnchorStats, 1, 0);
            _tlpToolsAnchorPick.Dock = DockStyle.Top;
            _tlpToolsAnchorPick.Location = new Point(0, 25);
            _tlpToolsAnchorPick.Margin = new Padding(0, 0, 0, 6);
            _tlpToolsAnchorPick.Name = "_tlpToolsAnchorPick";
            _tlpToolsAnchorPick.RowCount = 1;
            _tlpToolsAnchorPick.RowStyles.Add(new RowStyle());
            _tlpToolsAnchorPick.Size = new Size(1187, 31);
            _tlpToolsAnchorPick.TabIndex = 1;
            // 
            // _cboToolsAnchorModel
            // 
            _cboToolsAnchorModel.Dock = DockStyle.Fill;
            _cboToolsAnchorModel.DropDownStyle = ComboBoxStyle.DropDownList;
            _cboToolsAnchorModel.Font = new Font("Microsoft YaHei UI", 9.5F);
            _cboToolsAnchorModel.Location = new Point(0, 2);
            _cboToolsAnchorModel.Margin = new Padding(0, 2, 12, 2);
            _cboToolsAnchorModel.Name = "_cboToolsAnchorModel";
            _cboToolsAnchorModel.Size = new Size(208, 27);
            _cboToolsAnchorModel.TabIndex = 0;
            // 
            // _lblToolsAnchorStats
            // 
            _lblToolsAnchorStats.Anchor = AnchorStyles.Left;
            _lblToolsAnchorStats.AutoSize = true;
            _lblToolsAnchorStats.Font = new Font("Microsoft YaHei UI", 9.5F);
            _lblToolsAnchorStats.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _lblToolsAnchorStats.Location = new Point(220, 7);
            _lblToolsAnchorStats.Margin = new Padding(0, 7, 0, 4);
            _lblToolsAnchorStats.Name = "_lblToolsAnchorStats";
            _lblToolsAnchorStats.Size = new Size(0, 19);
            _lblToolsAnchorStats.TabIndex = 1;
            // 
            // _lblToolsAnchorModelInfo
            // 
            _lblToolsAnchorModelInfo.AutoSize = true;
            _lblToolsAnchorModelInfo.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lblToolsAnchorModelInfo.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblToolsAnchorModelInfo.Location = new Point(0, 62);
            _lblToolsAnchorModelInfo.Margin = new Padding(0, 0, 0, 2);
            _lblToolsAnchorModelInfo.Name = "_lblToolsAnchorModelInfo";
            _lblToolsAnchorModelInfo.Size = new Size(0, 17);
            _lblToolsAnchorModelInfo.TabIndex = 2;
            // 
            // _lblToolsAnchorCalibInfo
            // 
            _lblToolsAnchorCalibInfo.AutoSize = true;
            _lblToolsAnchorCalibInfo.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lblToolsAnchorCalibInfo.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblToolsAnchorCalibInfo.Location = new Point(0, 81);
            _lblToolsAnchorCalibInfo.Margin = new Padding(0, 0, 0, 6);
            _lblToolsAnchorCalibInfo.Name = "_lblToolsAnchorCalibInfo";
            _lblToolsAnchorCalibInfo.Size = new Size(0, 17);
            _lblToolsAnchorCalibInfo.TabIndex = 3;
            // 
            // _flowToolsAnchorLib
            // 
            _flowToolsAnchorLib.AutoSize = true;
            _flowToolsAnchorLib.BackColor = Color.White;
            _flowToolsAnchorLib.Controls.Add(_btnToolsAnchorNew);
            _flowToolsAnchorLib.Controls.Add(_btnToolsAnchorCopy);
            _flowToolsAnchorLib.Controls.Add(_btnToolsAnchorTrain);
            _flowToolsAnchorLib.Controls.Add(_btnToolsAnchorFolder);
            _flowToolsAnchorLib.Controls.Add(_btnToolsAnchorArchive);
            _flowToolsAnchorLib.Dock = DockStyle.Top;
            _flowToolsAnchorLib.Location = new Point(0, 104);
            _flowToolsAnchorLib.Margin = new Padding(0, 0, 0, 6);
            _flowToolsAnchorLib.Name = "_flowToolsAnchorLib";
            _flowToolsAnchorLib.Size = new Size(1187, 14);
            _flowToolsAnchorLib.TabIndex = 4;
            // 
            // _btnToolsAnchorNew
            // 
            _btnToolsAnchorNew.AutoSize = true;
            _btnToolsAnchorNew.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnToolsAnchorNew.BackColor = Color.White;
            _btnToolsAnchorNew.Cursor = Cursors.Hand;
            _btnToolsAnchorNew.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnToolsAnchorNew.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnToolsAnchorNew.FlatStyle = FlatStyle.Flat;
            _btnToolsAnchorNew.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnToolsAnchorNew.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnToolsAnchorNew.Location = new Point(0, 2);
            _btnToolsAnchorNew.Margin = new Padding(0, 2, 8, 2);
            _btnToolsAnchorNew.MinimumSize = new Size(112, 34);
            _btnToolsAnchorNew.Name = "_btnToolsAnchorNew";
            _btnToolsAnchorNew.Padding = new Padding(10, 0, 10, 0);
            _btnToolsAnchorNew.Size = new Size(112, 34);
            _btnToolsAnchorNew.TabIndex = 0;
            _btnToolsAnchorNew.Text = "新建学习库";
            _btnToolsAnchorNew.UseVisualStyleBackColor = false;
            // 
            // _btnToolsAnchorCopy
            // 
            _btnToolsAnchorCopy.AutoSize = true;
            _btnToolsAnchorCopy.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnToolsAnchorCopy.BackColor = Color.White;
            _btnToolsAnchorCopy.Cursor = Cursors.Hand;
            _btnToolsAnchorCopy.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnToolsAnchorCopy.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnToolsAnchorCopy.FlatStyle = FlatStyle.Flat;
            _btnToolsAnchorCopy.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnToolsAnchorCopy.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnToolsAnchorCopy.Location = new Point(120, 2);
            _btnToolsAnchorCopy.Margin = new Padding(0, 2, 8, 2);
            _btnToolsAnchorCopy.MinimumSize = new Size(150, 34);
            _btnToolsAnchorCopy.Name = "_btnToolsAnchorCopy";
            _btnToolsAnchorCopy.Padding = new Padding(10, 0, 10, 0);
            _btnToolsAnchorCopy.Size = new Size(150, 34);
            _btnToolsAnchorCopy.TabIndex = 1;
            _btnToolsAnchorCopy.Text = "复制当前训练模型";
            _btnToolsAnchorCopy.UseVisualStyleBackColor = false;
            // 
            // _btnToolsAnchorTrain
            // 
            _btnToolsAnchorTrain.AutoSize = true;
            _btnToolsAnchorTrain.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnToolsAnchorTrain.BackColor = Color.FromArgb((int)(byte)36, (int)(byte)112, (int)(byte)210);
            _btnToolsAnchorTrain.Cursor = Cursors.Hand;
            _btnToolsAnchorTrain.FlatAppearance.BorderSize = 0;
            _btnToolsAnchorTrain.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)28, (int)(byte)96, (int)(byte)186);
            _btnToolsAnchorTrain.FlatStyle = FlatStyle.Flat;
            _btnToolsAnchorTrain.Font = new Font("Microsoft YaHei UI", 9.5F, FontStyle.Bold);
            _btnToolsAnchorTrain.ForeColor = Color.White;
            _btnToolsAnchorTrain.Location = new Point(278, 2);
            _btnToolsAnchorTrain.Margin = new Padding(0, 2, 8, 2);
            _btnToolsAnchorTrain.MinimumSize = new Size(116, 34);
            _btnToolsAnchorTrain.Name = "_btnToolsAnchorTrain";
            _btnToolsAnchorTrain.Padding = new Padding(10, 0, 10, 0);
            _btnToolsAnchorTrain.Size = new Size(116, 34);
            _btnToolsAnchorTrain.TabIndex = 2;
            _btnToolsAnchorTrain.Text = "训练并部署";
            _btnToolsAnchorTrain.UseVisualStyleBackColor = false;
            // 
            // _btnToolsAnchorFolder
            // 
            _btnToolsAnchorFolder.AutoSize = true;
            _btnToolsAnchorFolder.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnToolsAnchorFolder.BackColor = Color.White;
            _btnToolsAnchorFolder.Cursor = Cursors.Hand;
            _btnToolsAnchorFolder.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnToolsAnchorFolder.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnToolsAnchorFolder.FlatStyle = FlatStyle.Flat;
            _btnToolsAnchorFolder.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnToolsAnchorFolder.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnToolsAnchorFolder.Location = new Point(402, 2);
            _btnToolsAnchorFolder.Margin = new Padding(0, 2, 8, 2);
            _btnToolsAnchorFolder.MinimumSize = new Size(96, 34);
            _btnToolsAnchorFolder.Name = "_btnToolsAnchorFolder";
            _btnToolsAnchorFolder.Padding = new Padding(10, 0, 10, 0);
            _btnToolsAnchorFolder.Size = new Size(96, 34);
            _btnToolsAnchorFolder.TabIndex = 3;
            _btnToolsAnchorFolder.Text = "打开目录";
            _btnToolsAnchorFolder.UseVisualStyleBackColor = false;
            // 
            // _btnToolsAnchorArchive
            // 
            _btnToolsAnchorArchive.AutoSize = true;
            _btnToolsAnchorArchive.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnToolsAnchorArchive.BackColor = Color.White;
            _btnToolsAnchorArchive.Cursor = Cursors.Hand;
            _btnToolsAnchorArchive.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnToolsAnchorArchive.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnToolsAnchorArchive.FlatStyle = FlatStyle.Flat;
            _btnToolsAnchorArchive.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnToolsAnchorArchive.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnToolsAnchorArchive.Location = new Point(506, 2);
            _btnToolsAnchorArchive.Margin = new Padding(0, 2, 8, 2);
            _btnToolsAnchorArchive.MinimumSize = new Size(96, 34);
            _btnToolsAnchorArchive.Name = "_btnToolsAnchorArchive";
            _btnToolsAnchorArchive.Padding = new Padding(10, 0, 10, 0);
            _btnToolsAnchorArchive.Size = new Size(96, 34);
            _btnToolsAnchorArchive.TabIndex = 4;
            _btnToolsAnchorArchive.Text = "压缩归档";
            _btnToolsAnchorArchive.UseVisualStyleBackColor = false;
            // 
            // _cardToolsMachine
            // 
            _cardToolsMachine.AutoSize = true;
            _cardToolsMachine.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _cardToolsMachine.BackColor = Color.White;
            _cardToolsMachine.Controls.Add(_bodyToolsMachine);
            _cardToolsMachine.Dock = DockStyle.Top;
            _cardToolsMachine.Location = new Point(0, 1347);
            _cardToolsMachine.Margin = new Padding(0, 0, 0, 10);
            _cardToolsMachine.Name = "_cardToolsMachine";
            _cardToolsMachine.Padding = new Padding(14, 12, 14, 12);
            _cardToolsMachine.Size = new Size(1215, 264);
            _cardToolsMachine.TabIndex = 5;
            _cardToolsMachine.Paint += (this.pnlCard_Paint);
            // 
            // _bodyToolsMachine
            // 
            _bodyToolsMachine.AutoSize = true;
            _bodyToolsMachine.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _bodyToolsMachine.BackColor = Color.White;
            _bodyToolsMachine.ColumnCount = 1;
            _bodyToolsMachine.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            _bodyToolsMachine.Controls.Add(_lblToolsMachineTitle, 0, 0);
            _bodyToolsMachine.Controls.Add(_tlpToolsMachinePick, 0, 1);
            _bodyToolsMachine.Controls.Add(_lvToolsMachines, 0, 2);
            _bodyToolsMachine.Controls.Add(_flowToolsMachine, 0, 3);
            _bodyToolsMachine.Dock = DockStyle.Top;
            _bodyToolsMachine.Location = new Point(14, 12);
            _bodyToolsMachine.Margin = new Padding(0);
            _bodyToolsMachine.Name = "_bodyToolsMachine";
            _bodyToolsMachine.RowCount = 3;
            _bodyToolsMachine.RowStyles.Add(new RowStyle());
            _bodyToolsMachine.RowStyles.Add(new RowStyle());
            _bodyToolsMachine.RowStyles.Add(new RowStyle());
            _bodyToolsMachine.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            _bodyToolsMachine.Size = new Size(1187, 240);
            _bodyToolsMachine.TabIndex = 0;
            // 
            // _lblToolsMachineTitle
            // 
            _lblToolsMachineTitle.AutoSize = true;
            _lblToolsMachineTitle.Font = new Font("Microsoft YaHei UI", 10.5F, FontStyle.Bold);
            _lblToolsMachineTitle.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _lblToolsMachineTitle.Location = new Point(0, 0);
            _lblToolsMachineTitle.Margin = new Padding(0, 0, 0, 6);
            _lblToolsMachineTitle.Name = "_lblToolsMachineTitle";
            _lblToolsMachineTitle.Size = new Size(65, 19);
            _lblToolsMachineTitle.TabIndex = 0;
            _lblToolsMachineTitle.Text = "机种管理";
            // 
            // _tlpToolsMachinePick
            // 
            _tlpToolsMachinePick.AutoSize = true;
            _tlpToolsMachinePick.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _tlpToolsMachinePick.ColumnCount = 2;
            _tlpToolsMachinePick.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 260F));
            _tlpToolsMachinePick.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            _tlpToolsMachinePick.Controls.Add(_cboToolsMachineKind, 0, 0);
            _tlpToolsMachinePick.Controls.Add(_lblToolsMachineInfo, 1, 0);
            _tlpToolsMachinePick.Dock = DockStyle.Top;
            _tlpToolsMachinePick.Location = new Point(0, 25);
            _tlpToolsMachinePick.Margin = new Padding(0, 0, 0, 6);
            _tlpToolsMachinePick.Name = "_tlpToolsMachinePick";
            _tlpToolsMachinePick.RowCount = 1;
            _tlpToolsMachinePick.RowStyles.Add(new RowStyle());
            _tlpToolsMachinePick.Size = new Size(1187, 31);
            _tlpToolsMachinePick.TabIndex = 1;
            // 
            // _cboToolsMachineKind
            // 
            _cboToolsMachineKind.Dock = DockStyle.Fill;
            _cboToolsMachineKind.DropDownStyle = ComboBoxStyle.DropDownList;
            _cboToolsMachineKind.Font = new Font("Microsoft YaHei UI", 9.5F);
            _cboToolsMachineKind.Location = new Point(0, 2);
            _cboToolsMachineKind.Margin = new Padding(0, 2, 12, 2);
            _cboToolsMachineKind.Name = "_cboToolsMachineKind";
            _cboToolsMachineKind.Size = new Size(248, 27);
            _cboToolsMachineKind.TabIndex = 0;
            // 
            // _lblToolsMachineInfo
            // 
            _lblToolsMachineInfo.Anchor = AnchorStyles.Left;
            _lblToolsMachineInfo.AutoSize = true;
            _lblToolsMachineInfo.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lblToolsMachineInfo.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblToolsMachineInfo.Location = new Point(260, 8);
            _lblToolsMachineInfo.Margin = new Padding(0, 7, 0, 4);
            _lblToolsMachineInfo.Name = "_lblToolsMachineInfo";
            _lblToolsMachineInfo.Size = new Size(0, 17);
            _lblToolsMachineInfo.TabIndex = 1;
            // 
            // _lvToolsMachines
            // 
            _lvToolsMachines.BackColor = Color.White;
            _lvToolsMachines.BorderStyle = BorderStyle.FixedSingle;
            _lvToolsMachines.Dock = DockStyle.Top;
            _lvToolsMachines.Font = new Font("Microsoft YaHei UI", 9.5F);
            _lvToolsMachines.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _lvToolsMachines.FullRowSelect = true;
            _lvToolsMachines.Location = new Point(0, 62);
            _lvToolsMachines.Margin = new Padding(0, 0, 0, 8);
            _lvToolsMachines.MultiSelect = false;
            _lvToolsMachines.Name = "_lvToolsMachines";
            _lvToolsMachines.Size = new Size(1187, 150);
            _lvToolsMachines.TabIndex = 2;
            _lvToolsMachines.UseCompatibleStateImageBehavior = false;
            _lvToolsMachines.View = View.Details;
            // 
            // _flowToolsMachine
            // 
            _flowToolsMachine.AutoSize = true;
            _flowToolsMachine.BackColor = Color.White;
            _flowToolsMachine.Controls.Add(_btnToolsMachineNew);
            _flowToolsMachine.Controls.Add(_btnToolsMachineRename);
            _flowToolsMachine.Controls.Add(_btnToolsMachineArchive);
            _flowToolsMachine.Controls.Add(_btnToolsMachineFolder);
            _flowToolsMachine.Dock = DockStyle.Top;
            _flowToolsMachine.Location = new Point(0, 220);
            _flowToolsMachine.Margin = new Padding(0, 0, 0, 6);
            _flowToolsMachine.Name = "_flowToolsMachine";
            _flowToolsMachine.Size = new Size(1187, 14);
            _flowToolsMachine.TabIndex = 3;
            // 
            // _btnToolsMachineNew
            // 
            _btnToolsMachineNew.AutoSize = true;
            _btnToolsMachineNew.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnToolsMachineNew.BackColor = Color.FromArgb((int)(byte)36, (int)(byte)112, (int)(byte)210);
            _btnToolsMachineNew.Cursor = Cursors.Hand;
            _btnToolsMachineNew.FlatAppearance.BorderSize = 0;
            _btnToolsMachineNew.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)28, (int)(byte)96, (int)(byte)186);
            _btnToolsMachineNew.FlatStyle = FlatStyle.Flat;
            _btnToolsMachineNew.Font = new Font("Microsoft YaHei UI", 9.5F, FontStyle.Bold);
            _btnToolsMachineNew.ForeColor = Color.White;
            _btnToolsMachineNew.Location = new Point(0, 2);
            _btnToolsMachineNew.Margin = new Padding(0, 2, 8, 2);
            _btnToolsMachineNew.MinimumSize = new Size(110, 34);
            _btnToolsMachineNew.Name = "_btnToolsMachineNew";
            _btnToolsMachineNew.Padding = new Padding(10, 0, 10, 0);
            _btnToolsMachineNew.Size = new Size(110, 34);
            _btnToolsMachineNew.TabIndex = 0;
            _btnToolsMachineNew.Text = "新建机种";
            _btnToolsMachineNew.UseVisualStyleBackColor = false;
            // 
            // _btnToolsMachineRename
            // 
            _btnToolsMachineRename.AutoSize = true;
            _btnToolsMachineRename.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnToolsMachineRename.BackColor = Color.White;
            _btnToolsMachineRename.Cursor = Cursors.Hand;
            _btnToolsMachineRename.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnToolsMachineRename.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnToolsMachineRename.FlatStyle = FlatStyle.Flat;
            _btnToolsMachineRename.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnToolsMachineRename.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnToolsMachineRename.Location = new Point(118, 2);
            _btnToolsMachineRename.Margin = new Padding(0, 2, 8, 2);
            _btnToolsMachineRename.MinimumSize = new Size(96, 34);
            _btnToolsMachineRename.Name = "_btnToolsMachineRename";
            _btnToolsMachineRename.Padding = new Padding(10, 0, 10, 0);
            _btnToolsMachineRename.Size = new Size(96, 34);
            _btnToolsMachineRename.TabIndex = 1;
            _btnToolsMachineRename.Text = "重命名";
            _btnToolsMachineRename.UseVisualStyleBackColor = false;
            // 
            // _btnToolsMachineArchive
            // 
            _btnToolsMachineArchive.AutoSize = true;
            _btnToolsMachineArchive.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnToolsMachineArchive.BackColor = Color.White;
            _btnToolsMachineArchive.Cursor = Cursors.Hand;
            _btnToolsMachineArchive.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnToolsMachineArchive.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnToolsMachineArchive.FlatStyle = FlatStyle.Flat;
            _btnToolsMachineArchive.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnToolsMachineArchive.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnToolsMachineArchive.Location = new Point(222, 2);
            _btnToolsMachineArchive.Margin = new Padding(0, 2, 8, 2);
            _btnToolsMachineArchive.MinimumSize = new Size(156, 34);
            _btnToolsMachineArchive.Name = "_btnToolsMachineArchive";
            _btnToolsMachineArchive.Padding = new Padding(10, 0, 10, 0);
            _btnToolsMachineArchive.Size = new Size(156, 34);
            _btnToolsMachineArchive.TabIndex = 2;
            _btnToolsMachineArchive.Text = "压缩归档（删除）";
            _btnToolsMachineArchive.UseVisualStyleBackColor = false;
            // 
            // _btnToolsMachineFolder
            // 
            _btnToolsMachineFolder.AutoSize = true;
            _btnToolsMachineFolder.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnToolsMachineFolder.BackColor = Color.White;
            _btnToolsMachineFolder.Cursor = Cursors.Hand;
            _btnToolsMachineFolder.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnToolsMachineFolder.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnToolsMachineFolder.FlatStyle = FlatStyle.Flat;
            _btnToolsMachineFolder.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnToolsMachineFolder.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnToolsMachineFolder.Location = new Point(386, 2);
            _btnToolsMachineFolder.Margin = new Padding(0, 2, 8, 2);
            _btnToolsMachineFolder.MinimumSize = new Size(96, 34);
            _btnToolsMachineFolder.Name = "_btnToolsMachineFolder";
            _btnToolsMachineFolder.Padding = new Padding(10, 0, 10, 0);
            _btnToolsMachineFolder.Size = new Size(96, 34);
            _btnToolsMachineFolder.TabIndex = 3;
            _btnToolsMachineFolder.Text = "打开目录";
            _btnToolsMachineFolder.UseVisualStyleBackColor = false;
            // 
            // _cardToolsDataset
            // 
            _cardToolsDataset.AutoSize = true;
            _cardToolsDataset.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _cardToolsDataset.BackColor = Color.White;
            _cardToolsDataset.Controls.Add(_bodyToolsDataset);
            _cardToolsDataset.Dock = DockStyle.Top;
            _cardToolsDataset.Location = new Point(0, 1621);
            _cardToolsDataset.Margin = new Padding(0, 0, 0, 10);
            _cardToolsDataset.Name = "_cardToolsDataset";
            _cardToolsDataset.Padding = new Padding(14, 12, 14, 12);
            _cardToolsDataset.Size = new Size(1215, 136);
            _cardToolsDataset.TabIndex = 6;
            _cardToolsDataset.Paint += (this.pnlCard_Paint);
            // 
            // _bodyToolsDataset
            // 
            _bodyToolsDataset.AutoSize = true;
            _bodyToolsDataset.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _bodyToolsDataset.BackColor = Color.White;
            _bodyToolsDataset.ColumnCount = 1;
            _bodyToolsDataset.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            _bodyToolsDataset.Controls.Add(_lblToolsDatasetTitle, 0, 0);
            _bodyToolsDataset.Controls.Add(_lblToolsDatasetStats, 0, 1);
            _bodyToolsDataset.Controls.Add(_lblToolsDatasetClasses, 0, 2);
            _bodyToolsDataset.Controls.Add(_lblToolsDatasetExtra, 0, 3);
            _bodyToolsDataset.Controls.Add(_flowToolsDataset, 0, 4);
            _bodyToolsDataset.Dock = DockStyle.Top;
            _bodyToolsDataset.Location = new Point(14, 12);
            _bodyToolsDataset.Margin = new Padding(0);
            _bodyToolsDataset.Name = "_bodyToolsDataset";
            _bodyToolsDataset.RowCount = 4;
            _bodyToolsDataset.RowStyles.Add(new RowStyle());
            _bodyToolsDataset.RowStyles.Add(new RowStyle());
            _bodyToolsDataset.RowStyles.Add(new RowStyle());
            _bodyToolsDataset.RowStyles.Add(new RowStyle());
            _bodyToolsDataset.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            _bodyToolsDataset.Size = new Size(1187, 112);
            _bodyToolsDataset.TabIndex = 0;
            // 
            // _lblToolsDatasetTitle
            // 
            _lblToolsDatasetTitle.AutoSize = true;
            _lblToolsDatasetTitle.Font = new Font("Microsoft YaHei UI", 10.5F, FontStyle.Bold);
            _lblToolsDatasetTitle.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _lblToolsDatasetTitle.Location = new Point(0, 0);
            _lblToolsDatasetTitle.Margin = new Padding(0, 0, 0, 6);
            _lblToolsDatasetTitle.Name = "_lblToolsDatasetTitle";
            _lblToolsDatasetTitle.Size = new Size(51, 19);
            _lblToolsDatasetTitle.TabIndex = 0;
            _lblToolsDatasetTitle.Text = "数据集";
            // 
            // _lblToolsDatasetStats
            // 
            _lblToolsDatasetStats.AutoSize = true;
            _lblToolsDatasetStats.Font = new Font("Microsoft YaHei UI", 9.5F);
            _lblToolsDatasetStats.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _lblToolsDatasetStats.Location = new Point(0, 25);
            _lblToolsDatasetStats.Margin = new Padding(0, 0, 0, 6);
            _lblToolsDatasetStats.Name = "_lblToolsDatasetStats";
            _lblToolsDatasetStats.Size = new Size(197, 19);
            _lblToolsDatasetStats.TabIndex = 1;
            _lblToolsDatasetStats.Text = "数据集：0 张图片 · 0 个框 · 0 类";
            // 
            // _lblToolsDatasetClasses
            // 
            _lblToolsDatasetClasses.AutoSize = true;
            _lblToolsDatasetClasses.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lblToolsDatasetClasses.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblToolsDatasetClasses.Location = new Point(0, 52);
            _lblToolsDatasetClasses.Margin = new Padding(0, 2, 0, 2);
            _lblToolsDatasetClasses.Name = "_lblToolsDatasetClasses";
            _lblToolsDatasetClasses.Size = new Size(0, 17);
            _lblToolsDatasetClasses.TabIndex = 2;
            // 
            // _lblToolsDatasetExtra
            // 
            _lblToolsDatasetExtra.AutoSize = true;
            _lblToolsDatasetExtra.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lblToolsDatasetExtra.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblToolsDatasetExtra.Location = new Point(0, 73);
            _lblToolsDatasetExtra.Margin = new Padding(0, 2, 0, 2);
            _lblToolsDatasetExtra.Name = "_lblToolsDatasetExtra";
            _lblToolsDatasetExtra.Size = new Size(0, 17);
            _lblToolsDatasetExtra.TabIndex = 3;
            // 
            // _flowToolsDataset
            // 
            _flowToolsDataset.AutoSize = true;
            _flowToolsDataset.BackColor = Color.White;
            _flowToolsDataset.Controls.Add(_btnToolsDatasetRebuild);
            _flowToolsDataset.Controls.Add(_btnToolsDatasetFolder);
            _flowToolsDataset.Dock = DockStyle.Top;
            _flowToolsDataset.Location = new Point(0, 100);
            _flowToolsDataset.Margin = new Padding(0, 8, 0, 0);
            _flowToolsDataset.Name = "_flowToolsDataset";
            _flowToolsDataset.Size = new Size(1187, 12);
            _flowToolsDataset.TabIndex = 4;
            _flowToolsDataset.WrapContents = false;
            // 
            // _btnToolsDatasetRebuild
            // 
            _btnToolsDatasetRebuild.AutoSize = true;
            _btnToolsDatasetRebuild.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnToolsDatasetRebuild.BackColor = Color.White;
            _btnToolsDatasetRebuild.Cursor = Cursors.Hand;
            _btnToolsDatasetRebuild.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnToolsDatasetRebuild.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnToolsDatasetRebuild.FlatStyle = FlatStyle.Flat;
            _btnToolsDatasetRebuild.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnToolsDatasetRebuild.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnToolsDatasetRebuild.Location = new Point(0, 2);
            _btnToolsDatasetRebuild.Margin = new Padding(0, 2, 8, 2);
            _btnToolsDatasetRebuild.MinimumSize = new Size(120, 34);
            _btnToolsDatasetRebuild.Name = "_btnToolsDatasetRebuild";
            _btnToolsDatasetRebuild.Padding = new Padding(10, 0, 10, 0);
            _btnToolsDatasetRebuild.Size = new Size(120, 34);
            _btnToolsDatasetRebuild.TabIndex = 0;
            _btnToolsDatasetRebuild.Text = "重建训练集";
            _btnToolsDatasetRebuild.UseVisualStyleBackColor = false;
            // 
            // _btnToolsDatasetFolder
            // 
            _btnToolsDatasetFolder.AutoSize = true;
            _btnToolsDatasetFolder.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnToolsDatasetFolder.BackColor = Color.White;
            _btnToolsDatasetFolder.Cursor = Cursors.Hand;
            _btnToolsDatasetFolder.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnToolsDatasetFolder.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnToolsDatasetFolder.FlatStyle = FlatStyle.Flat;
            _btnToolsDatasetFolder.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnToolsDatasetFolder.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnToolsDatasetFolder.Location = new Point(128, 2);
            _btnToolsDatasetFolder.Margin = new Padding(0, 2, 8, 2);
            _btnToolsDatasetFolder.MinimumSize = new Size(140, 34);
            _btnToolsDatasetFolder.Name = "_btnToolsDatasetFolder";
            _btnToolsDatasetFolder.Padding = new Padding(10, 0, 10, 0);
            _btnToolsDatasetFolder.Size = new Size(145, 34);
            _btnToolsDatasetFolder.TabIndex = 1;
            _btnToolsDatasetFolder.Text = "打开数据集文件夹";
            _btnToolsDatasetFolder.UseVisualStyleBackColor = false;
            // 
            // _cardToolsLabels
            // 
            _cardToolsLabels.AutoSize = true;
            _cardToolsLabels.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _cardToolsLabels.BackColor = Color.White;
            _cardToolsLabels.Controls.Add(_bodyToolsLabels);
            _cardToolsLabels.Dock = DockStyle.Top;
            _cardToolsLabels.Location = new Point(0, 1767);
            _cardToolsLabels.Margin = new Padding(0, 0, 0, 10);
            _cardToolsLabels.Name = "_cardToolsLabels";
            _cardToolsLabels.Padding = new Padding(14, 12, 14, 12);
            _cardToolsLabels.Size = new Size(1215, 269);
            _cardToolsLabels.TabIndex = 7;
            _cardToolsLabels.Paint += (this.pnlCard_Paint);
            // 
            // _bodyToolsLabels
            // 
            _bodyToolsLabels.AutoSize = true;
            _bodyToolsLabels.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _bodyToolsLabels.BackColor = Color.White;
            _bodyToolsLabels.ColumnCount = 1;
            _bodyToolsLabels.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            _bodyToolsLabels.Controls.Add(_lblToolsLabelsTitle, 0, 0);
            _bodyToolsLabels.Controls.Add(_lblToolsLabelsInfo, 0, 1);
            _bodyToolsLabels.Controls.Add(_gridToolsLabels, 0, 2);
            _bodyToolsLabels.Controls.Add(_lblToolsLabelHint, 0, 3);
            _bodyToolsLabels.Controls.Add(_flowToolsLabels, 0, 4);
            _bodyToolsLabels.Dock = DockStyle.Top;
            _bodyToolsLabels.Location = new Point(14, 12);
            _bodyToolsLabels.Margin = new Padding(0);
            _bodyToolsLabels.Name = "_bodyToolsLabels";
            _bodyToolsLabels.RowCount = 4;
            _bodyToolsLabels.RowStyles.Add(new RowStyle());
            _bodyToolsLabels.RowStyles.Add(new RowStyle());
            _bodyToolsLabels.RowStyles.Add(new RowStyle());
            _bodyToolsLabels.RowStyles.Add(new RowStyle());
            _bodyToolsLabels.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            _bodyToolsLabels.Size = new Size(1187, 245);
            _bodyToolsLabels.TabIndex = 0;
            // 
            // _lblToolsLabelsTitle
            // 
            _lblToolsLabelsTitle.AutoSize = true;
            _lblToolsLabelsTitle.Font = new Font("Microsoft YaHei UI", 10.5F, FontStyle.Bold);
            _lblToolsLabelsTitle.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _lblToolsLabelsTitle.Location = new Point(0, 0);
            _lblToolsLabelsTitle.Margin = new Padding(0, 0, 0, 6);
            _lblToolsLabelsTitle.Name = "_lblToolsLabelsTitle";
            _lblToolsLabelsTitle.Size = new Size(65, 19);
            _lblToolsLabelsTitle.TabIndex = 0;
            _lblToolsLabelsTitle.Text = "标签管理";
            // 
            // _lblToolsLabelsInfo
            // 
            _lblToolsLabelsInfo.AutoSize = true;
            _lblToolsLabelsInfo.Font = new Font("Microsoft YaHei UI", 9.5F);
            _lblToolsLabelsInfo.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _lblToolsLabelsInfo.Location = new Point(0, 25);
            _lblToolsLabelsInfo.Margin = new Padding(0, 0, 0, 6);
            _lblToolsLabelsInfo.Name = "_lblToolsLabelsInfo";
            _lblToolsLabelsInfo.Size = new Size(113, 19);
            _lblToolsLabelsInfo.TabIndex = 1;
            _lblToolsLabelsInfo.Text = "标签：还没有类别";
            // 
            // _gridToolsLabels
            // 
            _gridToolsLabels.Columns.AddRange(new DataGridViewColumn[] { dataGridViewTextBoxColumn8, dataGridViewTextBoxColumn9, dataGridViewTextBoxColumn10, dataGridViewTextBoxColumn11, dataGridViewTextBoxColumn12 });
            _gridToolsLabels.Dock = DockStyle.Top;
            _gridToolsLabels.Location = new Point(0, 50);
            _gridToolsLabels.Margin = new Padding(0, 0, 0, 6);
            _gridToolsLabels.Name = "_gridToolsLabels";
            _gridToolsLabels.Size = new Size(1187, 148);
            _gridToolsLabels.TabIndex = 2;
            // 
            // dataGridViewTextBoxColumn8
            // 
            dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            // 
            // dataGridViewTextBoxColumn9
            // 
            dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            // 
            // dataGridViewTextBoxColumn10
            // 
            dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            // 
            // dataGridViewTextBoxColumn11
            // 
            dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
            // 
            // dataGridViewTextBoxColumn12
            // 
            dataGridViewTextBoxColumn12.Name = "dataGridViewTextBoxColumn12";
            // 
            // _lblToolsLabelHint
            // 
            _lblToolsLabelHint.AutoSize = true;
            _lblToolsLabelHint.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lblToolsLabelHint.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblToolsLabelHint.Location = new Point(0, 206);
            _lblToolsLabelHint.Margin = new Padding(0, 2, 0, 2);
            _lblToolsLabelHint.Name = "_lblToolsLabelHint";
            _lblToolsLabelHint.Size = new Size(860, 17);
            _lblToolsLabelHint.TabIndex = 3;
            _lblToolsLabelHint.Text = "删标签会把对应的标注一起从数据集里删掉（删前自动备份 annotations.json / classes.txt），当前模型不变；想让它立刻不再显示，在弹窗里勾选“同时屏蔽”。";
            // 
            // _flowToolsLabels
            // 
            _flowToolsLabels.AutoSize = true;
            _flowToolsLabels.BackColor = Color.White;
            _flowToolsLabels.Controls.Add(_btnToolsLabelView);
            _flowToolsLabels.Controls.Add(_btnToolsLabelDelete);
            _flowToolsLabels.Dock = DockStyle.Top;
            _flowToolsLabels.Location = new Point(0, 225);
            _flowToolsLabels.Margin = new Padding(0, 0, 0, 6);
            _flowToolsLabels.Name = "_flowToolsLabels";
            _flowToolsLabels.Size = new Size(1187, 14);
            _flowToolsLabels.TabIndex = 4;
            // 
            // _btnToolsLabelView
            // 
            _btnToolsLabelView.AutoSize = true;
            _btnToolsLabelView.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnToolsLabelView.BackColor = Color.FromArgb((int)(byte)36, (int)(byte)112, (int)(byte)210);
            _btnToolsLabelView.Cursor = Cursors.Hand;
            _btnToolsLabelView.FlatAppearance.BorderSize = 0;
            _btnToolsLabelView.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)28, (int)(byte)96, (int)(byte)186);
            _btnToolsLabelView.FlatStyle = FlatStyle.Flat;
            _btnToolsLabelView.Font = new Font("Microsoft YaHei UI", 9.5F, FontStyle.Bold);
            _btnToolsLabelView.ForeColor = Color.White;
            _btnToolsLabelView.Location = new Point(0, 2);
            _btnToolsLabelView.Margin = new Padding(0, 2, 8, 2);
            _btnToolsLabelView.MinimumSize = new Size(140, 34);
            _btnToolsLabelView.Name = "_btnToolsLabelView";
            _btnToolsLabelView.Padding = new Padding(10, 0, 10, 0);
            _btnToolsLabelView.Size = new Size(140, 34);
            _btnToolsLabelView.TabIndex = 0;
            _btnToolsLabelView.Text = "查看训练内容";
            _btnToolsLabelView.UseVisualStyleBackColor = false;
            // 
            // _btnToolsLabelDelete
            // 
            _btnToolsLabelDelete.AutoSize = true;
            _btnToolsLabelDelete.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnToolsLabelDelete.BackColor = Color.White;
            _btnToolsLabelDelete.Cursor = Cursors.Hand;
            _btnToolsLabelDelete.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnToolsLabelDelete.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnToolsLabelDelete.FlatStyle = FlatStyle.Flat;
            _btnToolsLabelDelete.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnToolsLabelDelete.ForeColor = Color.FromArgb((int)(byte)190, (int)(byte)58, (int)(byte)58);
            _btnToolsLabelDelete.Location = new Point(148, 2);
            _btnToolsLabelDelete.Margin = new Padding(0, 2, 8, 2);
            _btnToolsLabelDelete.MinimumSize = new Size(132, 34);
            _btnToolsLabelDelete.Name = "_btnToolsLabelDelete";
            _btnToolsLabelDelete.Padding = new Padding(10, 0, 10, 0);
            _btnToolsLabelDelete.Size = new Size(132, 34);
            _btnToolsLabelDelete.TabIndex = 1;
            _btnToolsLabelDelete.Text = "删除这个标签";
            _btnToolsLabelDelete.UseVisualStyleBackColor = false;
            // 
            // _cardToolsBackup
            // 
            _cardToolsBackup.AutoSize = true;
            _cardToolsBackup.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _cardToolsBackup.BackColor = Color.White;
            _cardToolsBackup.Controls.Add(_bodyToolsBackup);
            _cardToolsBackup.Dock = DockStyle.Top;
            _cardToolsBackup.Location = new Point(0, 2046);
            _cardToolsBackup.Margin = new Padding(0, 0, 0, 10);
            _cardToolsBackup.Name = "_cardToolsBackup";
            _cardToolsBackup.Padding = new Padding(14, 12, 14, 12);
            _cardToolsBackup.Size = new Size(1215, 206);
            _cardToolsBackup.TabIndex = 8;
            _cardToolsBackup.Paint += (this.pnlCard_Paint);
            // 
            // _bodyToolsBackup
            // 
            _bodyToolsBackup.AutoSize = true;
            _bodyToolsBackup.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _bodyToolsBackup.BackColor = Color.White;
            _bodyToolsBackup.ColumnCount = 1;
            _bodyToolsBackup.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            _bodyToolsBackup.Controls.Add(_lblToolsBackupTitle, 0, 0);
            _bodyToolsBackup.Controls.Add(_lvToolsBackups, 0, 1);
            _bodyToolsBackup.Controls.Add(_lblToolsBackupInfo, 0, 2);
            _bodyToolsBackup.Controls.Add(_flowToolsBackup, 0, 3);
            _bodyToolsBackup.Dock = DockStyle.Top;
            _bodyToolsBackup.Location = new Point(14, 12);
            _bodyToolsBackup.Margin = new Padding(0);
            _bodyToolsBackup.Name = "_bodyToolsBackup";
            _bodyToolsBackup.RowCount = 3;
            _bodyToolsBackup.RowStyles.Add(new RowStyle());
            _bodyToolsBackup.RowStyles.Add(new RowStyle());
            _bodyToolsBackup.RowStyles.Add(new RowStyle());
            _bodyToolsBackup.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            _bodyToolsBackup.Size = new Size(1187, 182);
            _bodyToolsBackup.TabIndex = 0;
            // 
            // _lblToolsBackupTitle
            // 
            _lblToolsBackupTitle.AutoSize = true;
            _lblToolsBackupTitle.Font = new Font("Microsoft YaHei UI", 10.5F, FontStyle.Bold);
            _lblToolsBackupTitle.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _lblToolsBackupTitle.Location = new Point(0, 0);
            _lblToolsBackupTitle.Margin = new Padding(0, 0, 0, 6);
            _lblToolsBackupTitle.Name = "_lblToolsBackupTitle";
            _lblToolsBackupTitle.Size = new Size(65, 19);
            _lblToolsBackupTitle.TabIndex = 0;
            _lblToolsBackupTitle.Text = "模型备份";
            // 
            // _lvToolsBackups
            // 
            _lvToolsBackups.BorderStyle = BorderStyle.FixedSingle;
            _lvToolsBackups.Dock = DockStyle.Top;
            _lvToolsBackups.Font = new Font("Consolas", 8.5F);
            _lvToolsBackups.IntegralHeight = false;
            _lvToolsBackups.ItemHeight = 13;
            _lvToolsBackups.Location = new Point(0, 25);
            _lvToolsBackups.Margin = new Padding(0, 0, 0, 6);
            _lvToolsBackups.Name = "_lvToolsBackups";
            _lvToolsBackups.Size = new Size(1187, 110);
            _lvToolsBackups.TabIndex = 1;
            // 
            // _lblToolsBackupInfo
            // 
            _lblToolsBackupInfo.AutoSize = true;
            _lblToolsBackupInfo.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lblToolsBackupInfo.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblToolsBackupInfo.Location = new Point(0, 143);
            _lblToolsBackupInfo.Margin = new Padding(0, 2, 0, 2);
            _lblToolsBackupInfo.Name = "_lblToolsBackupInfo";
            _lblToolsBackupInfo.Size = new Size(0, 17);
            _lblToolsBackupInfo.TabIndex = 2;
            // 
            // _flowToolsBackup
            // 
            _flowToolsBackup.AutoSize = true;
            _flowToolsBackup.BackColor = Color.White;
            _flowToolsBackup.Controls.Add(_btnToolsBackupRestore);
            _flowToolsBackup.Controls.Add(_btnToolsBackupDelete);
            _flowToolsBackup.Controls.Add(_btnToolsBackupFolder);
            _flowToolsBackup.Dock = DockStyle.Top;
            _flowToolsBackup.Location = new Point(0, 162);
            _flowToolsBackup.Margin = new Padding(0, 0, 0, 6);
            _flowToolsBackup.Name = "_flowToolsBackup";
            _flowToolsBackup.Size = new Size(1187, 14);
            _flowToolsBackup.TabIndex = 3;
            // 
            // _btnToolsBackupRestore
            // 
            _btnToolsBackupRestore.AutoSize = true;
            _btnToolsBackupRestore.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnToolsBackupRestore.BackColor = Color.White;
            _btnToolsBackupRestore.Cursor = Cursors.Hand;
            _btnToolsBackupRestore.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnToolsBackupRestore.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnToolsBackupRestore.FlatStyle = FlatStyle.Flat;
            _btnToolsBackupRestore.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnToolsBackupRestore.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnToolsBackupRestore.Location = new Point(0, 2);
            _btnToolsBackupRestore.Margin = new Padding(0, 2, 8, 2);
            _btnToolsBackupRestore.MinimumSize = new Size(130, 34);
            _btnToolsBackupRestore.Name = "_btnToolsBackupRestore";
            _btnToolsBackupRestore.Padding = new Padding(10, 0, 10, 0);
            _btnToolsBackupRestore.Size = new Size(130, 34);
            _btnToolsBackupRestore.TabIndex = 0;
            _btnToolsBackupRestore.Text = "恢复选中备份";
            _btnToolsBackupRestore.UseVisualStyleBackColor = false;
            // 
            // _btnToolsBackupDelete
            // 
            _btnToolsBackupDelete.AutoSize = true;
            _btnToolsBackupDelete.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnToolsBackupDelete.BackColor = Color.White;
            _btnToolsBackupDelete.Cursor = Cursors.Hand;
            _btnToolsBackupDelete.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnToolsBackupDelete.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnToolsBackupDelete.FlatStyle = FlatStyle.Flat;
            _btnToolsBackupDelete.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnToolsBackupDelete.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnToolsBackupDelete.Location = new Point(138, 2);
            _btnToolsBackupDelete.Margin = new Padding(0, 2, 8, 2);
            _btnToolsBackupDelete.MinimumSize = new Size(130, 34);
            _btnToolsBackupDelete.Name = "_btnToolsBackupDelete";
            _btnToolsBackupDelete.Padding = new Padding(10, 0, 10, 0);
            _btnToolsBackupDelete.Size = new Size(130, 34);
            _btnToolsBackupDelete.TabIndex = 1;
            _btnToolsBackupDelete.Text = "删除选中备份";
            _btnToolsBackupDelete.UseVisualStyleBackColor = false;
            // 
            // _btnToolsBackupFolder
            // 
            _btnToolsBackupFolder.AutoSize = true;
            _btnToolsBackupFolder.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnToolsBackupFolder.BackColor = Color.White;
            _btnToolsBackupFolder.Cursor = Cursors.Hand;
            _btnToolsBackupFolder.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnToolsBackupFolder.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnToolsBackupFolder.FlatStyle = FlatStyle.Flat;
            _btnToolsBackupFolder.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnToolsBackupFolder.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnToolsBackupFolder.Location = new Point(276, 2);
            _btnToolsBackupFolder.Margin = new Padding(0, 2, 8, 2);
            _btnToolsBackupFolder.MinimumSize = new Size(140, 34);
            _btnToolsBackupFolder.Name = "_btnToolsBackupFolder";
            _btnToolsBackupFolder.Padding = new Padding(10, 0, 10, 0);
            _btnToolsBackupFolder.Size = new Size(140, 34);
            _btnToolsBackupFolder.TabIndex = 2;
            _btnToolsBackupFolder.Text = "打开备份文件夹";
            _btnToolsBackupFolder.UseVisualStyleBackColor = false;
            // 
            // _cardToolsMisdetect
            // 
            _cardToolsMisdetect.AutoSize = true;
            _cardToolsMisdetect.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _cardToolsMisdetect.BackColor = Color.White;
            _cardToolsMisdetect.Controls.Add(_bodyToolsMisdetect);
            _cardToolsMisdetect.Dock = DockStyle.Top;
            _cardToolsMisdetect.Location = new Point(0, 2262);
            _cardToolsMisdetect.Margin = new Padding(0, 0, 0, 10);
            _cardToolsMisdetect.Name = "_cardToolsMisdetect";
            _cardToolsMisdetect.Padding = new Padding(14, 12, 14, 12);
            _cardToolsMisdetect.Size = new Size(1215, 90);
            _cardToolsMisdetect.TabIndex = 9;
            _cardToolsMisdetect.Paint += (this.pnlCard_Paint);
            // 
            // _bodyToolsMisdetect
            // 
            _bodyToolsMisdetect.AutoSize = true;
            _bodyToolsMisdetect.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _bodyToolsMisdetect.BackColor = Color.White;
            _bodyToolsMisdetect.ColumnCount = 1;
            _bodyToolsMisdetect.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            _bodyToolsMisdetect.Controls.Add(_lblToolsMisdetectTitle, 0, 0);
            _bodyToolsMisdetect.Controls.Add(_lblToolsMisdetect, 0, 1);
            _bodyToolsMisdetect.Controls.Add(_flowToolsMisdetect, 0, 2);
            _bodyToolsMisdetect.Dock = DockStyle.Top;
            _bodyToolsMisdetect.Location = new Point(14, 12);
            _bodyToolsMisdetect.Margin = new Padding(0);
            _bodyToolsMisdetect.Name = "_bodyToolsMisdetect";
            _bodyToolsMisdetect.RowCount = 2;
            _bodyToolsMisdetect.RowStyles.Add(new RowStyle());
            _bodyToolsMisdetect.RowStyles.Add(new RowStyle());
            _bodyToolsMisdetect.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            _bodyToolsMisdetect.Size = new Size(1187, 66);
            _bodyToolsMisdetect.TabIndex = 0;
            // 
            // _lblToolsMisdetectTitle
            // 
            _lblToolsMisdetectTitle.AutoSize = true;
            _lblToolsMisdetectTitle.Font = new Font("Microsoft YaHei UI", 10.5F, FontStyle.Bold);
            _lblToolsMisdetectTitle.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _lblToolsMisdetectTitle.Location = new Point(0, 0);
            _lblToolsMisdetectTitle.Margin = new Padding(0, 0, 0, 6);
            _lblToolsMisdetectTitle.Name = "_lblToolsMisdetectTitle";
            _lblToolsMisdetectTitle.Size = new Size(65, 19);
            _lblToolsMisdetectTitle.TabIndex = 0;
            _lblToolsMisdetectTitle.Text = "误检样本";
            // 
            // _lblToolsMisdetect
            // 
            _lblToolsMisdetect.AutoSize = true;
            _lblToolsMisdetect.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lblToolsMisdetect.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblToolsMisdetect.Location = new Point(0, 27);
            _lblToolsMisdetect.Margin = new Padding(0, 2, 0, 2);
            _lblToolsMisdetect.Name = "_lblToolsMisdetect";
            _lblToolsMisdetect.Size = new Size(0, 17);
            _lblToolsMisdetect.TabIndex = 1;
            // 
            // _flowToolsMisdetect
            // 
            _flowToolsMisdetect.AutoSize = true;
            _flowToolsMisdetect.BackColor = Color.White;
            _flowToolsMisdetect.Controls.Add(_btnToolsMisdetectFolder);
            _flowToolsMisdetect.Controls.Add(_btnToolsMisdetectClear);
            _flowToolsMisdetect.Dock = DockStyle.Top;
            _flowToolsMisdetect.Location = new Point(0, 46);
            _flowToolsMisdetect.Margin = new Padding(0, 0, 0, 6);
            _flowToolsMisdetect.Name = "_flowToolsMisdetect";
            _flowToolsMisdetect.Size = new Size(1187, 14);
            _flowToolsMisdetect.TabIndex = 2;
            // 
            // _btnToolsMisdetectFolder
            // 
            _btnToolsMisdetectFolder.AutoSize = true;
            _btnToolsMisdetectFolder.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnToolsMisdetectFolder.BackColor = Color.White;
            _btnToolsMisdetectFolder.Cursor = Cursors.Hand;
            _btnToolsMisdetectFolder.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnToolsMisdetectFolder.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnToolsMisdetectFolder.FlatStyle = FlatStyle.Flat;
            _btnToolsMisdetectFolder.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnToolsMisdetectFolder.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnToolsMisdetectFolder.Location = new Point(0, 2);
            _btnToolsMisdetectFolder.Margin = new Padding(0, 2, 8, 2);
            _btnToolsMisdetectFolder.MinimumSize = new Size(140, 34);
            _btnToolsMisdetectFolder.Name = "_btnToolsMisdetectFolder";
            _btnToolsMisdetectFolder.Padding = new Padding(10, 0, 10, 0);
            _btnToolsMisdetectFolder.Size = new Size(140, 34);
            _btnToolsMisdetectFolder.TabIndex = 0;
            _btnToolsMisdetectFolder.Text = "打开误检文件夹";
            _btnToolsMisdetectFolder.UseVisualStyleBackColor = false;
            // 
            // _btnToolsMisdetectClear
            // 
            _btnToolsMisdetectClear.AutoSize = true;
            _btnToolsMisdetectClear.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnToolsMisdetectClear.BackColor = Color.White;
            _btnToolsMisdetectClear.Cursor = Cursors.Hand;
            _btnToolsMisdetectClear.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnToolsMisdetectClear.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnToolsMisdetectClear.FlatStyle = FlatStyle.Flat;
            _btnToolsMisdetectClear.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnToolsMisdetectClear.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnToolsMisdetectClear.Location = new Point(148, 2);
            _btnToolsMisdetectClear.Margin = new Padding(0, 2, 8, 2);
            _btnToolsMisdetectClear.MinimumSize = new Size(130, 34);
            _btnToolsMisdetectClear.Name = "_btnToolsMisdetectClear";
            _btnToolsMisdetectClear.Padding = new Padding(10, 0, 10, 0);
            _btnToolsMisdetectClear.Size = new Size(130, 34);
            _btnToolsMisdetectClear.TabIndex = 1;
            _btnToolsMisdetectClear.Text = "清空误检样本";
            _btnToolsMisdetectClear.UseVisualStyleBackColor = false;
            // 
            // _pageFixture
            // 
            _pageFixture.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _pageFixture.Controls.Add(_tlpFixture);
            _pageFixture.Location = new Point(4, 34);
            _pageFixture.Name = "_pageFixture";
            _pageFixture.Padding = new Padding(12, 10, 12, 10);
            _pageFixture.Size = new Size(1256, 669);
            _pageFixture.TabIndex = 3;
            _pageFixture.Text = "针床检测";
            // 
            // _tlpFixture
            // 
            _tlpFixture.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _tlpFixture.ColumnCount = 1;
            _tlpFixture.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            _tlpFixture.Controls.Add(_tlpFixBar, 0, 0);
            _tlpFixture.Controls.Add(_tlpFixBody, 0, 1);
            _tlpFixture.Dock = DockStyle.Fill;
            _tlpFixture.Location = new Point(12, 10);
            _tlpFixture.Name = "_tlpFixture";
            _tlpFixture.Padding = new Padding(2);
            _tlpFixture.RowCount = 2;
            _tlpFixture.RowStyles.Add(new RowStyle());
            _tlpFixture.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            _tlpFixture.Size = new Size(1232, 649);
            _tlpFixture.TabIndex = 0;
            // 
            // _tlpFixBar
            // 
            _tlpFixBar.AutoSize = true;
            _tlpFixBar.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _tlpFixBar.ColumnCount = 2;
            _tlpFixBar.ColumnStyles.Add(new ColumnStyle());
            _tlpFixBar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            _tlpFixBar.Controls.Add(_flpFixBar, 0, 0);
            _tlpFixBar.Controls.Add(_lblFixCamInfo, 1, 0);
            _tlpFixBar.Dock = DockStyle.Top;
            _tlpFixBar.Location = new Point(2, 2);
            _tlpFixBar.Margin = new Padding(0, 0, 0, 6);
            _tlpFixBar.Name = "_tlpFixBar";
            _tlpFixBar.RowCount = 1;
            _tlpFixBar.RowStyles.Add(new RowStyle());
            _tlpFixBar.Size = new Size(1228, 39);
            _tlpFixBar.TabIndex = 0;
            // 
            // _flpFixBar
            // 
            _flpFixBar.AutoSize = true;
            _flpFixBar.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _flpFixBar.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _flpFixBar.Controls.Add(_lblFixModel);
            _flpFixBar.Controls.Add(_cboFixModel);
            _flpFixBar.Controls.Add(_btnFixNewModel);
            _flpFixBar.Controls.Add(_btnFixMode);
            _flpFixBar.Controls.Add(_btnFixCamApply);
            _flpFixBar.Controls.Add(_btnFixCamClose);
            _flpFixBar.Controls.Add(_btnFixSnapshot);
            _flpFixBar.Controls.Add(_btnFixLive);
            _flpFixBar.Controls.Add(_btnFixOverlay);
            _flpFixBar.Location = new Point(0, 0);
            _flpFixBar.Margin = new Padding(0);
            _flpFixBar.Name = "_flpFixBar";
            _flpFixBar.Size = new Size(875, 39);
            _flpFixBar.TabIndex = 0;
            _flpFixBar.WrapContents = false;
            // 
            // _lblFixModel
            // 
            _lblFixModel.AutoSize = true;
            _lblFixModel.Font = new Font("Microsoft YaHei UI", 9.5F, FontStyle.Bold);
            _lblFixModel.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _lblFixModel.Location = new Point(0, 11);
            _lblFixModel.Margin = new Padding(0, 11, 6, 0);
            _lblFixModel.Name = "_lblFixModel";
            _lblFixModel.Size = new Size(35, 19);
            _lblFixModel.TabIndex = 0;
            _lblFixModel.Text = "机种";
            // 
            // _cboFixModel
            // 
            _cboFixModel.DropDownStyle = ComboBoxStyle.DropDownList;
            _cboFixModel.Font = new Font("Microsoft YaHei UI", 9.5F);
            _cboFixModel.Location = new Point(41, 7);
            _cboFixModel.Margin = new Padding(0, 7, 4, 0);
            _cboFixModel.Name = "_cboFixModel";
            _cboFixModel.Size = new Size(110, 27);
            _cboFixModel.TabIndex = 1;
            // 
            // _btnFixNewModel
            // 
            _btnFixNewModel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnFixNewModel.BackColor = Color.White;
            _btnFixNewModel.Cursor = Cursors.Hand;
            _btnFixNewModel.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnFixNewModel.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnFixNewModel.FlatStyle = FlatStyle.Flat;
            _btnFixNewModel.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnFixNewModel.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnFixNewModel.Location = new Point(155, 5);
            _btnFixNewModel.Margin = new Padding(0, 5, 12, 0);
            _btnFixNewModel.MinimumSize = new Size(92, 34);
            _btnFixNewModel.Name = "_btnFixNewModel";
            _btnFixNewModel.Padding = new Padding(10, 0, 10, 0);
            _btnFixNewModel.Size = new Size(92, 34);
            _btnFixNewModel.TabIndex = 2;
            _btnFixNewModel.Text = "新建机种";
            _btnFixNewModel.UseVisualStyleBackColor = false;
            // 
            // _btnFixMode
            // 
            _btnFixMode.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnFixMode.BackColor = Color.White;
            _btnFixMode.Cursor = Cursors.Hand;
            _btnFixMode.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnFixMode.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnFixMode.FlatStyle = FlatStyle.Flat;
            _btnFixMode.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnFixMode.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnFixMode.Location = new Point(259, 5);
            _btnFixMode.Margin = new Padding(0, 5, 12, 0);
            _btnFixMode.MinimumSize = new Size(112, 34);
            _btnFixMode.Name = "_btnFixMode";
            _btnFixMode.Padding = new Padding(10, 0, 10, 0);
            _btnFixMode.Size = new Size(112, 34);
            _btnFixMode.TabIndex = 3;
            _btnFixMode.Text = "切到执行模式";
            _btnFixMode.UseVisualStyleBackColor = false;
            // 
            // _btnFixCamApply
            // 
            _btnFixCamApply.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnFixCamApply.BackColor = Color.White;
            _btnFixCamApply.Cursor = Cursors.Hand;
            _btnFixCamApply.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnFixCamApply.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnFixCamApply.FlatStyle = FlatStyle.Flat;
            _btnFixCamApply.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnFixCamApply.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnFixCamApply.Location = new Point(383, 5);
            _btnFixCamApply.Margin = new Padding(0, 5, 4, 0);
            _btnFixCamApply.MinimumSize = new Size(92, 34);
            _btnFixCamApply.Name = "_btnFixCamApply";
            _btnFixCamApply.Padding = new Padding(10, 0, 10, 0);
            _btnFixCamApply.Size = new Size(92, 34);
            _btnFixCamApply.TabIndex = 4;
            _btnFixCamApply.Text = "打开相机";
            _btnFixCamApply.UseVisualStyleBackColor = false;
            // 
            // _btnFixCamClose
            // 
            _btnFixCamClose.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnFixCamClose.BackColor = Color.White;
            _btnFixCamClose.Cursor = Cursors.Hand;
            _btnFixCamClose.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnFixCamClose.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnFixCamClose.FlatStyle = FlatStyle.Flat;
            _btnFixCamClose.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnFixCamClose.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnFixCamClose.Location = new Point(479, 5);
            _btnFixCamClose.Margin = new Padding(0, 5, 4, 0);
            _btnFixCamClose.MinimumSize = new Size(92, 34);
            _btnFixCamClose.Name = "_btnFixCamClose";
            _btnFixCamClose.Padding = new Padding(10, 0, 10, 0);
            _btnFixCamClose.Size = new Size(92, 34);
            _btnFixCamClose.TabIndex = 5;
            _btnFixCamClose.Text = "关闭相机";
            _btnFixCamClose.UseVisualStyleBackColor = false;
            // 
            // _btnFixSnapshot
            // 
            _btnFixSnapshot.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnFixSnapshot.BackColor = Color.White;
            _btnFixSnapshot.Cursor = Cursors.Hand;
            _btnFixSnapshot.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnFixSnapshot.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnFixSnapshot.FlatStyle = FlatStyle.Flat;
            _btnFixSnapshot.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnFixSnapshot.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnFixSnapshot.Location = new Point(575, 5);
            _btnFixSnapshot.Margin = new Padding(0, 5, 4, 0);
            _btnFixSnapshot.MinimumSize = new Size(84, 34);
            _btnFixSnapshot.Name = "_btnFixSnapshot";
            _btnFixSnapshot.Padding = new Padding(10, 0, 10, 0);
            _btnFixSnapshot.Size = new Size(84, 34);
            _btnFixSnapshot.TabIndex = 6;
            _btnFixSnapshot.Text = "拍照";
            _btnFixSnapshot.UseVisualStyleBackColor = false;
            // 
            // _btnFixLive
            // 
            _btnFixLive.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnFixLive.BackColor = Color.White;
            _btnFixLive.Cursor = Cursors.Hand;
            _btnFixLive.Enabled = false;
            _btnFixLive.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnFixLive.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnFixLive.FlatStyle = FlatStyle.Flat;
            _btnFixLive.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnFixLive.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnFixLive.Location = new Point(663, 5);
            _btnFixLive.Margin = new Padding(0, 5, 12, 0);
            _btnFixLive.MinimumSize = new Size(96, 34);
            _btnFixLive.Name = "_btnFixLive";
            _btnFixLive.Padding = new Padding(10, 0, 10, 0);
            _btnFixLive.Size = new Size(96, 34);
            _btnFixLive.TabIndex = 7;
            _btnFixLive.Text = "恢复预览";
            _btnFixLive.UseVisualStyleBackColor = false;
            // 
            // _btnFixOverlay
            // 
            _btnFixOverlay.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnFixOverlay.BackColor = Color.White;
            _btnFixOverlay.Cursor = Cursors.Hand;
            _btnFixOverlay.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnFixOverlay.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnFixOverlay.FlatStyle = FlatStyle.Flat;
            _btnFixOverlay.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnFixOverlay.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnFixOverlay.Location = new Point(771, 5);
            _btnFixOverlay.Margin = new Padding(0, 5, 12, 0);
            _btnFixOverlay.MinimumSize = new Size(92, 34);
            _btnFixOverlay.Name = "_btnFixOverlay";
            _btnFixOverlay.Padding = new Padding(10, 0, 10, 0);
            _btnFixOverlay.Size = new Size(92, 34);
            _btnFixOverlay.TabIndex = 8;
            _btnFixOverlay.Text = "显示叠加";
            _btnFixOverlay.UseVisualStyleBackColor = false;
            // 
            // _lblFixCamInfo
            // 
            _lblFixCamInfo.AutoEllipsis = true;
            _lblFixCamInfo.Dock = DockStyle.Fill;
            _lblFixCamInfo.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lblFixCamInfo.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblFixCamInfo.Location = new Point(875, 5);
            _lblFixCamInfo.Margin = new Padding(0, 5, 0, 0);
            _lblFixCamInfo.Name = "_lblFixCamInfo";
            _lblFixCamInfo.Size = new Size(353, 34);
            _lblFixCamInfo.TabIndex = 1;
            _lblFixCamInfo.Text = "相机未打开";
            _lblFixCamInfo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // _tlpFixBody
            // 
            _tlpFixBody.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _tlpFixBody.ColumnCount = 2;
            _tlpFixBody.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            _tlpFixBody.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 440F));
            _tlpFixBody.Controls.Add(_picFixture, 0, 0);
            _tlpFixBody.Controls.Add(_tlpFixSide, 1, 0);
            _tlpFixBody.Dock = DockStyle.Fill;
            _tlpFixBody.Location = new Point(5, 50);
            _tlpFixBody.Name = "_tlpFixBody";
            _tlpFixBody.RowCount = 1;
            _tlpFixBody.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            _tlpFixBody.Size = new Size(1222, 594);
            _tlpFixBody.TabIndex = 1;
            // 
            // _picFixture
            // 
            _picFixture.BackColor = Color.FromArgb((int)(byte)36, (int)(byte)40, (int)(byte)47);
            _picFixture.BoxLabel = null;
            _picFixture.BoxPending = null;
            _picFixture.CanvasMenu = null;
            _picFixture.DetectionLabel = null;
            _picFixture.Dock = DockStyle.Fill;
            _picFixture.Editable = false;
            _picFixture.EmptyText = "① 打开相机\r\n② 采集空治具与金样板\r\n③ 点“开始检测”";
            _picFixture.Font = new Font("Microsoft YaHei UI", 9.5F);
            _picFixture.Image = null;
            _picFixture.LineA = null;
            _picFixture.LineB = null;
            _picFixture.LineEditable = false;
            _picFixture.LineLabel = "计数线";
            _picFixture.Location = new Point(0, 0);
            _picFixture.Margin = new Padding(0, 0, 8, 0);
            _picFixture.Name = "_picFixture";
            _picFixture.RightClickAction = null;
            _picFixture.SelectedIndex = -1;
            _picFixture.ShowZoomInfo = true;
            _picFixture.Size = new Size(774, 594);
            _picFixture.TabIndex = 0;
            _picFixture.TrackLabel = null;
            // 
            // _tlpFixSide
            // 
            _tlpFixSide.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _tlpFixSide.ColumnCount = 1;
            _tlpFixSide.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            _tlpFixSide.Controls.Add(_pnlFixVerdict, 0, 0);
            _tlpFixSide.Controls.Add(_lblFixReason, 0, 1);
            _tlpFixSide.Controls.Add(_pnlFixHost, 0, 2);
            _tlpFixSide.Controls.Add(_lvFixLog, 0, 3);
            _tlpFixSide.Dock = DockStyle.Fill;
            _tlpFixSide.Location = new Point(785, 3);
            _tlpFixSide.Name = "_tlpFixSide";
            _tlpFixSide.RowCount = 4;
            _tlpFixSide.RowStyles.Add(new RowStyle(SizeType.Absolute, 80F));
            _tlpFixSide.RowStyles.Add(new RowStyle());
            _tlpFixSide.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            _tlpFixSide.RowStyles.Add(new RowStyle(SizeType.Absolute, 116F));
            _tlpFixSide.Size = new Size(434, 588);
            _tlpFixSide.TabIndex = 1;
            // 
            // _pnlFixVerdict
            // 
            _pnlFixVerdict.BackColor = Color.White;
            _pnlFixVerdict.BorderStyle = BorderStyle.FixedSingle;
            _pnlFixVerdict.Controls.Add(_lblFixVerdict);
            _pnlFixVerdict.Dock = DockStyle.Fill;
            _pnlFixVerdict.Location = new Point(0, 0);
            _pnlFixVerdict.Margin = new Padding(0, 0, 0, 6);
            _pnlFixVerdict.Name = "_pnlFixVerdict";
            _pnlFixVerdict.Size = new Size(434, 74);
            _pnlFixVerdict.TabIndex = 0;
            // 
            // _lblFixVerdict
            // 
            _lblFixVerdict.AutoSize = true;
            _lblFixVerdict.Dock = DockStyle.Fill;
            _lblFixVerdict.Font = new Font("Microsoft YaHei UI", 26F, FontStyle.Bold);
            _lblFixVerdict.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblFixVerdict.Location = new Point(0, 0);
            _lblFixVerdict.Name = "_lblFixVerdict";
            _lblFixVerdict.Size = new Size(125, 46);
            _lblFixVerdict.TabIndex = 0;
            _lblFixVerdict.Text = "待检测";
            _lblFixVerdict.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // _lblFixReason
            // 
            _lblFixReason.AutoSize = true;
            _lblFixReason.Dock = DockStyle.Top;
            _lblFixReason.Font = new Font("Microsoft YaHei UI", 9.5F);
            _lblFixReason.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _lblFixReason.Location = new Point(2, 80);
            _lblFixReason.Margin = new Padding(2, 0, 2, 8);
            _lblFixReason.MaximumSize = new Size(340, 40);
            _lblFixReason.Name = "_lblFixReason";
            _lblFixReason.Size = new Size(340, 19);
            _lblFixReason.TabIndex = 1;
            _lblFixReason.Text = "选择机种后，先采集空治具和金样板。";
            // 
            // _pnlFixHost
            // 
            _pnlFixHost.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _pnlFixHost.Controls.Add(_pnlFixExec);
            _pnlFixHost.Controls.Add(_pnlFixDebug);
            _pnlFixHost.Dock = DockStyle.Fill;
            _pnlFixHost.Location = new Point(3, 110);
            _pnlFixHost.Name = "_pnlFixHost";
            _pnlFixHost.Size = new Size(428, 359);
            _pnlFixHost.TabIndex = 2;
            // 
            // _pnlFixExec
            // 
            _pnlFixExec.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _pnlFixExec.Controls.Add(_tlpFixExec);
            _pnlFixExec.Dock = DockStyle.Fill;
            _pnlFixExec.Location = new Point(0, 0);
            _pnlFixExec.Name = "_pnlFixExec";
            _pnlFixExec.Size = new Size(428, 359);
            _pnlFixExec.TabIndex = 0;
            // 
            // _tlpFixExec
            // 
            _tlpFixExec.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _tlpFixExec.ColumnCount = 1;
            _tlpFixExec.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            _tlpFixExec.Controls.Add(_btnFixJudge, 0, 0);
            _tlpFixExec.Controls.Add(_lblFixExecHint, 0, 1);
            _tlpFixExec.Controls.Add(_btnFixReplay2, 0, 2);
            _tlpFixExec.Dock = DockStyle.Fill;
            _tlpFixExec.Location = new Point(0, 0);
            _tlpFixExec.Name = "_tlpFixExec";
            _tlpFixExec.RowCount = 4;
            _tlpFixExec.RowStyles.Add(new RowStyle());
            _tlpFixExec.RowStyles.Add(new RowStyle());
            _tlpFixExec.RowStyles.Add(new RowStyle());
            _tlpFixExec.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            _tlpFixExec.Size = new Size(428, 359);
            _tlpFixExec.TabIndex = 0;
            // 
            // _btnFixJudge
            // 
            _btnFixJudge.AutoSize = true;
            _btnFixJudge.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnFixJudge.BackColor = Color.FromArgb((int)(byte)36, (int)(byte)112, (int)(byte)210);
            _btnFixJudge.Cursor = Cursors.Hand;
            _btnFixJudge.Dock = DockStyle.Top;
            _btnFixJudge.FlatAppearance.BorderSize = 0;
            _btnFixJudge.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)28, (int)(byte)96, (int)(byte)186);
            _btnFixJudge.FlatStyle = FlatStyle.Flat;
            _btnFixJudge.Font = new Font("Microsoft YaHei UI", 9.5F, FontStyle.Bold);
            _btnFixJudge.ForeColor = Color.White;
            _btnFixJudge.Location = new Point(0, 4);
            _btnFixJudge.Margin = new Padding(0, 4, 0, 8);
            _btnFixJudge.MinimumSize = new Size(84, 58);
            _btnFixJudge.Name = "_btnFixJudge";
            _btnFixJudge.Padding = new Padding(10, 0, 10, 0);
            _btnFixJudge.Size = new Size(428, 58);
            _btnFixJudge.TabIndex = 0;
            _btnFixJudge.Text = "开始检测";
            _btnFixJudge.UseVisualStyleBackColor = false;
            // 
            // _lblFixExecHint
            // 
            _lblFixExecHint.AutoSize = true;
            _lblFixExecHint.Dock = DockStyle.Top;
            _lblFixExecHint.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lblFixExecHint.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblFixExecHint.Location = new Point(2, 70);
            _lblFixExecHint.Margin = new Padding(2, 0, 2, 8);
            _lblFixExecHint.MaximumSize = new Size(340, 0);
            _lblFixExecHint.Name = "_lblFixExecHint";
            _lblFixExecHint.Size = new Size(340, 17);
            _lblFixExecHint.TabIndex = 1;
            _lblFixExecHint.Text = "单次触发：点一次检测一次。相机常开，不影响节拍。";
            // 
            // _btnFixReplay2
            // 
            _btnFixReplay2.AutoSize = true;
            _btnFixReplay2.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnFixReplay2.BackColor = Color.White;
            _btnFixReplay2.Cursor = Cursors.Hand;
            _btnFixReplay2.Dock = DockStyle.Top;
            _btnFixReplay2.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnFixReplay2.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnFixReplay2.FlatStyle = FlatStyle.Flat;
            _btnFixReplay2.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnFixReplay2.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnFixReplay2.Location = new Point(0, 95);
            _btnFixReplay2.Margin = new Padding(0, 0, 0, 4);
            _btnFixReplay2.MinimumSize = new Size(84, 34);
            _btnFixReplay2.Name = "_btnFixReplay2";
            _btnFixReplay2.Padding = new Padding(10, 0, 10, 0);
            _btnFixReplay2.Size = new Size(428, 34);
            _btnFixReplay2.TabIndex = 2;
            _btnFixReplay2.Text = "回放图片…";
            _btnFixReplay2.UseVisualStyleBackColor = false;
            // 
            // _pnlFixDebug
            // 
            _pnlFixDebug.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _pnlFixDebug.Controls.Add(_tlpFixDebugHost);
            _pnlFixDebug.Dock = DockStyle.Fill;
            _pnlFixDebug.Location = new Point(0, 0);
            _pnlFixDebug.Name = "_pnlFixDebug";
            _pnlFixDebug.Size = new Size(428, 359);
            _pnlFixDebug.TabIndex = 1;
            // 
            // _tlpFixDebugHost
            // 
            _tlpFixDebugHost.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _tlpFixDebugHost.ColumnCount = 1;
            _tlpFixDebugHost.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            _tlpFixDebugHost.Controls.Add(_flpFixThumbs, 0, 0);
            _tlpFixDebugHost.Controls.Add(_tcFixDebug, 0, 1);
            _tlpFixDebugHost.Dock = DockStyle.Fill;
            _tlpFixDebugHost.Location = new Point(0, 0);
            _tlpFixDebugHost.Name = "_tlpFixDebugHost";
            _tlpFixDebugHost.RowCount = 2;
            _tlpFixDebugHost.RowStyles.Add(new RowStyle(SizeType.Absolute, 72F));
            _tlpFixDebugHost.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            _tlpFixDebugHost.Size = new Size(428, 359);
            _tlpFixDebugHost.TabIndex = 0;
            // 
            // _flpFixThumbs
            // 
            _flpFixThumbs.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _flpFixThumbs.Dock = DockStyle.Fill;
            _flpFixThumbs.Location = new Point(0, 0);
            _flpFixThumbs.Margin = new Padding(0);
            _flpFixThumbs.Name = "_flpFixThumbs";
            _flpFixThumbs.Padding = new Padding(0, 0, 0, 4);
            _flpFixThumbs.Size = new Size(428, 72);
            _flpFixThumbs.TabIndex = 0;
            _flpFixThumbs.WrapContents = false;
            // 
            // _tcFixDebug
            // 
            _tcFixDebug.Controls.Add(_fixTabCamera);
            _tcFixDebug.Controls.Add(_fixTabCalib);
            _tcFixDebug.Controls.Add(_fixTabTemplate);
            _tcFixDebug.Controls.Add(_fixTabThreshold);
            _tcFixDebug.Controls.Add(_fixTabTest);
            _tcFixDebug.Controls.Add(_fixSegTab);
            _tcFixDebug.Dock = DockStyle.Fill;
            _tcFixDebug.Font = new Font("Microsoft YaHei UI", 9.5F);
            _tcFixDebug.Location = new Point(3, 75);
            _tcFixDebug.Name = "_tcFixDebug";
            _tcFixDebug.Padding = new Point(12, 5);
            _tcFixDebug.SelectedIndex = 0;
            _tcFixDebug.Size = new Size(422, 281);
            _tcFixDebug.TabIndex = 1;
            // 
            // _fixTabCamera
            // 
            _fixTabCamera.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _fixTabCamera.Controls.Add(_flpFixColCamera);
            _fixTabCamera.Location = new Point(4, 32);
            _fixTabCamera.Name = "_fixTabCamera";
            _fixTabCamera.Padding = new Padding(6);
            _fixTabCamera.Size = new Size(414, 245);
            _fixTabCamera.TabIndex = 0;
            _fixTabCamera.Text = "相机";
            // 
            // _flpFixColCamera
            // 
            _flpFixColCamera.AutoScroll = true;
            _flpFixColCamera.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _flpFixColCamera.Controls.Add(_flpFixRowCam1);
            _flpFixColCamera.Controls.Add(_flpFixRowCam2);
            _flpFixColCamera.Controls.Add(_flpFixRowCam3);
            _flpFixColCamera.Controls.Add(_lblFixExposureHint);
            _flpFixColCamera.Controls.Add(_flpFixRowCam4);
            _flpFixColCamera.Controls.Add(_lblFixRoi);
            _flpFixColCamera.Dock = DockStyle.Fill;
            _flpFixColCamera.FlowDirection = FlowDirection.TopDown;
            _flpFixColCamera.Location = new Point(6, 6);
            _flpFixColCamera.Margin = new Padding(0);
            _flpFixColCamera.Name = "_flpFixColCamera";
            _flpFixColCamera.Padding = new Padding(4);
            _flpFixColCamera.Size = new Size(402, 233);
            _flpFixColCamera.TabIndex = 0;
            _flpFixColCamera.WrapContents = false;
            // 
            // _flpFixRowCam1
            // 
            _flpFixRowCam1.AutoSize = true;
            _flpFixRowCam1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _flpFixRowCam1.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _flpFixRowCam1.Controls.Add(_lblFixCamIndex);
            _flpFixRowCam1.Controls.Add(_cboFixCamIndex);
            _flpFixRowCam1.Controls.Add(_lblFixCamRes);
            _flpFixRowCam1.Controls.Add(_cboFixRes);
            _flpFixRowCam1.Location = new Point(4, 4);
            _flpFixRowCam1.Margin = new Padding(0, 0, 0, 6);
            _flpFixRowCam1.Name = "_flpFixRowCam1";
            _flpFixRowCam1.Size = new Size(275, 33);
            _flpFixRowCam1.TabIndex = 0;
            _flpFixRowCam1.WrapContents = false;
            // 
            // _lblFixCamIndex
            // 
            _lblFixCamIndex.AutoSize = true;
            _lblFixCamIndex.Font = new Font("Microsoft YaHei UI", 9.5F);
            _lblFixCamIndex.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _lblFixCamIndex.Location = new Point(3, 0);
            _lblFixCamIndex.Name = "_lblFixCamIndex";
            _lblFixCamIndex.Size = new Size(35, 19);
            _lblFixCamIndex.TabIndex = 0;
            _lblFixCamIndex.Text = "序号";
            // 
            // _cboFixCamIndex
            // 
            _cboFixCamIndex.DropDownStyle = ComboBoxStyle.DropDownList;
            _cboFixCamIndex.Font = new Font("Microsoft YaHei UI", 9.5F);
            _cboFixCamIndex.Items.AddRange(new object[] { "0", "1", "2", "3", "4", "5" });
            _cboFixCamIndex.Location = new Point(41, 6);
            _cboFixCamIndex.Margin = new Padding(0, 6, 8, 0);
            _cboFixCamIndex.Name = "_cboFixCamIndex";
            _cboFixCamIndex.Size = new Size(60, 27);
            _cboFixCamIndex.TabIndex = 1;
            // 
            // _lblFixCamRes
            // 
            _lblFixCamRes.AutoSize = true;
            _lblFixCamRes.Font = new Font("Microsoft YaHei UI", 9.5F);
            _lblFixCamRes.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _lblFixCamRes.Location = new Point(112, 0);
            _lblFixCamRes.Name = "_lblFixCamRes";
            _lblFixCamRes.Size = new Size(48, 19);
            _lblFixCamRes.TabIndex = 2;
            _lblFixCamRes.Text = "分辨率";
            // 
            // _cboFixRes
            // 
            _cboFixRes.DropDownStyle = ComboBoxStyle.DropDownList;
            _cboFixRes.Font = new Font("Microsoft YaHei UI", 9.5F);
            _cboFixRes.Items.AddRange(new object[] { "640×480", "1280×720", "1920×1080" });
            _cboFixRes.Location = new Point(163, 6);
            _cboFixRes.Margin = new Padding(0, 6, 8, 0);
            _cboFixRes.Name = "_cboFixRes";
            _cboFixRes.Size = new Size(104, 27);
            _cboFixRes.TabIndex = 3;
            // 
            // _flpFixRowCam2
            // 
            _flpFixRowCam2.AutoSize = true;
            _flpFixRowCam2.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _flpFixRowCam2.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _flpFixRowCam2.Controls.Add(_lblFixCamFormat);
            _flpFixRowCam2.Controls.Add(_cboFixFourCC);
            _flpFixRowCam2.Location = new Point(4, 43);
            _flpFixRowCam2.Margin = new Padding(0, 0, 0, 6);
            _flpFixRowCam2.Name = "_flpFixRowCam2";
            _flpFixRowCam2.Size = new Size(113, 33);
            _flpFixRowCam2.TabIndex = 1;
            _flpFixRowCam2.WrapContents = false;
            // 
            // _lblFixCamFormat
            // 
            _lblFixCamFormat.AutoSize = true;
            _lblFixCamFormat.Font = new Font("Microsoft YaHei UI", 9.5F);
            _lblFixCamFormat.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _lblFixCamFormat.Location = new Point(3, 0);
            _lblFixCamFormat.Name = "_lblFixCamFormat";
            _lblFixCamFormat.Size = new Size(35, 19);
            _lblFixCamFormat.TabIndex = 0;
            _lblFixCamFormat.Text = "格式";
            // 
            // _cboFixFourCC
            // 
            _cboFixFourCC.DropDownStyle = ComboBoxStyle.DropDownList;
            _cboFixFourCC.Font = new Font("Microsoft YaHei UI", 9.5F);
            _cboFixFourCC.Items.AddRange(new object[] { "MJPG", "YUYV" });
            _cboFixFourCC.Location = new Point(41, 6);
            _cboFixFourCC.Margin = new Padding(0, 6, 8, 0);
            _cboFixFourCC.Name = "_cboFixFourCC";
            _cboFixFourCC.Size = new Size(64, 27);
            _cboFixFourCC.TabIndex = 1;
            // 
            // _flpFixRowCam3
            // 
            _flpFixRowCam3.AutoSize = true;
            _flpFixRowCam3.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _flpFixRowCam3.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _flpFixRowCam3.Controls.Add(_chkFixExposure);
            _flpFixRowCam3.Controls.Add(_lblFixExposure);
            _flpFixRowCam3.Controls.Add(_numFixExposure);
            _flpFixRowCam3.Controls.Add(_lblFixGain);
            _flpFixRowCam3.Controls.Add(_numFixGain);
            _flpFixRowCam3.Location = new Point(4, 82);
            _flpFixRowCam3.Margin = new Padding(0, 0, 0, 6);
            _flpFixRowCam3.Name = "_flpFixRowCam3";
            _flpFixRowCam3.Size = new Size(296, 32);
            _flpFixRowCam3.TabIndex = 2;
            _flpFixRowCam3.WrapContents = false;
            // 
            // _chkFixExposure
            // 
            _chkFixExposure.AutoSize = true;
            _chkFixExposure.Font = new Font("Microsoft YaHei UI", 9.5F);
            _chkFixExposure.Location = new Point(0, 9);
            _chkFixExposure.Margin = new Padding(0, 9, 6, 0);
            _chkFixExposure.Name = "_chkFixExposure";
            _chkFixExposure.Size = new Size(80, 23);
            _chkFixExposure.TabIndex = 0;
            _chkFixExposure.Text = "手动曝光";
            // 
            // _lblFixExposure
            // 
            _lblFixExposure.AutoSize = true;
            _lblFixExposure.Font = new Font("Microsoft YaHei UI", 9.5F);
            _lblFixExposure.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _lblFixExposure.Location = new Point(89, 0);
            _lblFixExposure.Name = "_lblFixExposure";
            _lblFixExposure.Size = new Size(35, 19);
            _lblFixExposure.TabIndex = 1;
            _lblFixExposure.Text = "曝光";
            // 
            // _numFixExposure
            // 
            _numFixExposure.Font = new Font("Microsoft YaHei UI", 9.5F);
            _numFixExposure.Location = new Point(127, 6);
            _numFixExposure.Margin = new Padding(0, 6, 8, 0);
            _numFixExposure.Maximum = new decimal(new int[] { 0, 0, 0, 0 });
            _numFixExposure.Minimum = new decimal(new int[] { 13, 0, 0, global::System.Int32.MinValue });
            _numFixExposure.Name = "_numFixExposure";
            _numFixExposure.Size = new Size(56, 24);
            _numFixExposure.TabIndex = 2;
            _numFixExposure.Value = new decimal(new int[] { 5, 0, 0, global::System.Int32.MinValue });
            // 
            // _lblFixGain
            // 
            _lblFixGain.AutoSize = true;
            _lblFixGain.Font = new Font("Microsoft YaHei UI", 9.5F);
            _lblFixGain.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _lblFixGain.Location = new Point(194, 0);
            _lblFixGain.Name = "_lblFixGain";
            _lblFixGain.Size = new Size(35, 19);
            _lblFixGain.TabIndex = 3;
            _lblFixGain.Text = "增益";
            // 
            // _numFixGain
            // 
            _numFixGain.Font = new Font("Microsoft YaHei UI", 9.5F);
            _numFixGain.Location = new Point(232, 6);
            _numFixGain.Margin = new Padding(0, 6, 8, 0);
            _numFixGain.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            _numFixGain.Name = "_numFixGain";
            _numFixGain.Size = new Size(56, 24);
            _numFixGain.TabIndex = 4;
            // 
            // _lblFixExposureHint
            // 
            _lblFixExposureHint.AutoSize = true;
            _lblFixExposureHint.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lblFixExposureHint.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblFixExposureHint.Location = new Point(4, 120);
            _lblFixExposureHint.Margin = new Padding(0, 0, 0, 8);
            _lblFixExposureHint.MaximumSize = new Size(340, 0);
            _lblFixExposureHint.Name = "_lblFixExposureHint";
            _lblFixExposureHint.Size = new Size(338, 34);
            _lblFixExposureHint.TabIndex = 3;
            _lblFixExposureHint.Text = "普通 USB 相机常锁不住曝光：写入失败时用“驱动属性页”关自动曝光，再固定光源。";
            // 
            // _flpFixRowCam4
            // 
            _flpFixRowCam4.AutoSize = true;
            _flpFixRowCam4.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _flpFixRowCam4.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _flpFixRowCam4.Controls.Add(_btnFixCamOpenApply);
            _flpFixRowCam4.Controls.Add(_btnFixCamDriver);
            _flpFixRowCam4.Controls.Add(_btnFixRoi);
            _flpFixRowCam4.Location = new Point(4, 162);
            _flpFixRowCam4.Margin = new Padding(0, 0, 0, 6);
            _flpFixRowCam4.Name = "_flpFixRowCam4";
            _flpFixRowCam4.Size = new Size(374, 36);
            _flpFixRowCam4.TabIndex = 4;
            _flpFixRowCam4.WrapContents = false;
            // 
            // _btnFixCamOpenApply
            // 
            _btnFixCamOpenApply.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnFixCamOpenApply.BackColor = Color.White;
            _btnFixCamOpenApply.Cursor = Cursors.Hand;
            _btnFixCamOpenApply.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnFixCamOpenApply.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnFixCamOpenApply.FlatStyle = FlatStyle.Flat;
            _btnFixCamOpenApply.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnFixCamOpenApply.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnFixCamOpenApply.Location = new Point(0, 2);
            _btnFixCamOpenApply.Margin = new Padding(0, 2, 6, 0);
            _btnFixCamOpenApply.MinimumSize = new Size(104, 34);
            _btnFixCamOpenApply.Name = "_btnFixCamOpenApply";
            _btnFixCamOpenApply.Padding = new Padding(10, 0, 10, 0);
            _btnFixCamOpenApply.Size = new Size(104, 34);
            _btnFixCamOpenApply.TabIndex = 0;
            _btnFixCamOpenApply.Text = "应用并打开";
            _btnFixCamOpenApply.UseVisualStyleBackColor = false;
            // 
            // _btnFixCamDriver
            // 
            _btnFixCamDriver.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnFixCamDriver.BackColor = Color.White;
            _btnFixCamDriver.Cursor = Cursors.Hand;
            _btnFixCamDriver.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnFixCamDriver.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnFixCamDriver.FlatStyle = FlatStyle.Flat;
            _btnFixCamDriver.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnFixCamDriver.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnFixCamDriver.Location = new Point(110, 2);
            _btnFixCamDriver.Margin = new Padding(0, 2, 6, 0);
            _btnFixCamDriver.MinimumSize = new Size(104, 34);
            _btnFixCamDriver.Name = "_btnFixCamDriver";
            _btnFixCamDriver.Padding = new Padding(10, 0, 10, 0);
            _btnFixCamDriver.Size = new Size(104, 34);
            _btnFixCamDriver.TabIndex = 1;
            _btnFixCamDriver.Text = "驱动属性页";
            _btnFixCamDriver.UseVisualStyleBackColor = false;
            // 
            // _btnFixRoi
            // 
            _btnFixRoi.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnFixRoi.BackColor = Color.White;
            _btnFixRoi.Cursor = Cursors.Hand;
            _btnFixRoi.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnFixRoi.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnFixRoi.FlatStyle = FlatStyle.Flat;
            _btnFixRoi.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnFixRoi.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnFixRoi.Location = new Point(220, 2);
            _btnFixRoi.Margin = new Padding(0, 2, 6, 0);
            _btnFixRoi.MinimumSize = new Size(148, 34);
            _btnFixRoi.Name = "_btnFixRoi";
            _btnFixRoi.Padding = new Padding(10, 0, 10, 0);
            _btnFixRoi.Size = new Size(148, 34);
            _btnFixRoi.TabIndex = 2;
            _btnFixRoi.Text = "框选检测区 ROI";
            _btnFixRoi.UseVisualStyleBackColor = false;
            // 
            // _lblFixRoi
            // 
            _lblFixRoi.AutoSize = true;
            _lblFixRoi.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lblFixRoi.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblFixRoi.Location = new Point(4, 204);
            _lblFixRoi.Margin = new Padding(0, 0, 0, 8);
            _lblFixRoi.MaximumSize = new Size(340, 0);
            _lblFixRoi.Name = "_lblFixRoi";
            _lblFixRoi.Size = new Size(118, 17);
            _lblFixRoi.TabIndex = 5;
            _lblFixRoi.Text = "当前 ROI：整幅画面";
            // 
            // _fixTabCalib
            // 
            _fixTabCalib.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _fixTabCalib.Controls.Add(_flpFixColCalib);
            _fixTabCalib.Location = new Point(4, 32);
            _fixTabCalib.Name = "_fixTabCalib";
            _fixTabCalib.Padding = new Padding(6);
            _fixTabCalib.Size = new Size(414, 245);
            _fixTabCalib.TabIndex = 1;
            _fixTabCalib.Text = "标定";
            // 
            // _flpFixColCalib
            // 
            _flpFixColCalib.AutoScroll = true;
            _flpFixColCalib.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _flpFixColCalib.Controls.Add(_lblFixCalibHint);
            _flpFixColCalib.Controls.Add(_flpFixRowCalib1);
            _flpFixColCalib.Controls.Add(_flpFixRowCalib2);
            _flpFixColCalib.Controls.Add(_lblFixCalib);
            _flpFixColCalib.Dock = DockStyle.Fill;
            _flpFixColCalib.FlowDirection = FlowDirection.TopDown;
            _flpFixColCalib.Location = new Point(6, 6);
            _flpFixColCalib.Margin = new Padding(0);
            _flpFixColCalib.Name = "_flpFixColCalib";
            _flpFixColCalib.Padding = new Padding(4);
            _flpFixColCalib.Size = new Size(402, 233);
            _flpFixColCalib.TabIndex = 0;
            _flpFixColCalib.WrapContents = false;
            // 
            // _lblFixCalibHint
            // 
            _lblFixCalibHint.AutoSize = true;
            _lblFixCalibHint.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lblFixCalibHint.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblFixCalibHint.Location = new Point(4, 4);
            _lblFixCalibHint.Margin = new Padding(0, 0, 0, 8);
            _lblFixCalibHint.MaximumSize = new Size(372, 0);
            _lblFixCalibHint.Name = "_lblFixCalibHint";
            _lblFixCalibHint.Size = new Size(366, 34);
            _lblFixCalibHint.TabIndex = 0;
            _lblFixCalibHint.Text = "把直尺或标定块放到针床平面，点“拾取两点”，在画面里点直尺的两端。得到 mm/像素 后，所有公差才有物理意义。";
            // 
            // _flpFixRowCalib1
            // 
            _flpFixRowCalib1.AutoSize = true;
            _flpFixRowCalib1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _flpFixRowCalib1.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _flpFixRowCalib1.Controls.Add(_btnFixCalibPick);
            _flpFixRowCalib1.Controls.Add(_lblFixCalibMm);
            _flpFixRowCalib1.Controls.Add(_numFixCalibMm);
            _flpFixRowCalib1.Location = new Point(4, 46);
            _flpFixRowCalib1.Margin = new Padding(0, 0, 0, 6);
            _flpFixRowCalib1.Name = "_flpFixRowCalib1";
            _flpFixRowCalib1.Size = new Size(297, 36);
            _flpFixRowCalib1.TabIndex = 1;
            _flpFixRowCalib1.WrapContents = false;
            // 
            // _btnFixCalibPick
            // 
            _btnFixCalibPick.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnFixCalibPick.BackColor = Color.White;
            _btnFixCalibPick.Cursor = Cursors.Hand;
            _btnFixCalibPick.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnFixCalibPick.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnFixCalibPick.FlatStyle = FlatStyle.Flat;
            _btnFixCalibPick.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnFixCalibPick.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnFixCalibPick.Location = new Point(0, 2);
            _btnFixCalibPick.Margin = new Padding(0, 2, 8, 0);
            _btnFixCalibPick.MinimumSize = new Size(96, 34);
            _btnFixCalibPick.Name = "_btnFixCalibPick";
            _btnFixCalibPick.Padding = new Padding(10, 0, 10, 0);
            _btnFixCalibPick.Size = new Size(96, 34);
            _btnFixCalibPick.TabIndex = 0;
            _btnFixCalibPick.Text = "拾取两点";
            _btnFixCalibPick.UseVisualStyleBackColor = false;
            // 
            // _lblFixCalibMm
            // 
            _lblFixCalibMm.AutoSize = true;
            _lblFixCalibMm.Font = new Font("Microsoft YaHei UI", 9.5F);
            _lblFixCalibMm.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _lblFixCalibMm.Location = new Point(107, 0);
            _lblFixCalibMm.Name = "_lblFixCalibMm";
            _lblFixCalibMm.Size = new Size(89, 19);
            _lblFixCalibMm.TabIndex = 1;
            _lblFixCalibMm.Text = "实际长度 mm";
            // 
            // _numFixCalibMm
            // 
            _numFixCalibMm.DecimalPlaces = 1;
            _numFixCalibMm.Font = new Font("Microsoft YaHei UI", 9.5F);
            _numFixCalibMm.Location = new Point(199, 6);
            _numFixCalibMm.Margin = new Padding(0, 6, 8, 0);
            _numFixCalibMm.Maximum = new decimal(new int[] { 2000, 0, 0, 0 });
            _numFixCalibMm.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            _numFixCalibMm.Name = "_numFixCalibMm";
            _numFixCalibMm.Size = new Size(90, 24);
            _numFixCalibMm.TabIndex = 2;
            _numFixCalibMm.Value = new decimal(new int[] { 100, 0, 0, 0 });
            // 
            // _flpFixRowCalib2
            // 
            _flpFixRowCalib2.AutoSize = true;
            _flpFixRowCalib2.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _flpFixRowCalib2.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _flpFixRowCalib2.Controls.Add(_btnFixCalibSave);
            _flpFixRowCalib2.Location = new Point(4, 88);
            _flpFixRowCalib2.Margin = new Padding(0, 0, 0, 6);
            _flpFixRowCalib2.Name = "_flpFixRowCalib2";
            _flpFixRowCalib2.Size = new Size(112, 36);
            _flpFixRowCalib2.TabIndex = 2;
            _flpFixRowCalib2.WrapContents = false;
            // 
            // _btnFixCalibSave
            // 
            _btnFixCalibSave.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnFixCalibSave.BackColor = Color.White;
            _btnFixCalibSave.Cursor = Cursors.Hand;
            _btnFixCalibSave.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnFixCalibSave.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnFixCalibSave.FlatStyle = FlatStyle.Flat;
            _btnFixCalibSave.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnFixCalibSave.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnFixCalibSave.Location = new Point(0, 2);
            _btnFixCalibSave.Margin = new Padding(0, 2, 8, 0);
            _btnFixCalibSave.MinimumSize = new Size(104, 34);
            _btnFixCalibSave.Name = "_btnFixCalibSave";
            _btnFixCalibSave.Padding = new Padding(10, 0, 10, 0);
            _btnFixCalibSave.Size = new Size(104, 34);
            _btnFixCalibSave.TabIndex = 0;
            _btnFixCalibSave.Text = "重算并保存";
            _btnFixCalibSave.UseVisualStyleBackColor = false;
            // 
            // _lblFixCalib
            // 
            _lblFixCalib.AutoSize = true;
            _lblFixCalib.Font = new Font("Microsoft YaHei UI", 9.5F);
            _lblFixCalib.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _lblFixCalib.Location = new Point(4, 134);
            _lblFixCalib.Margin = new Padding(0, 4, 0, 8);
            _lblFixCalib.MaximumSize = new Size(372, 0);
            _lblFixCalib.Name = "_lblFixCalib";
            _lblFixCalib.Size = new Size(74, 19);
            _lblFixCalib.TabIndex = 3;
            _lblFixCalib.Text = "尚未标定。";
            // 
            // _fixTabTemplate
            // 
            _fixTabTemplate.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _fixTabTemplate.Controls.Add(_flpFixColTemplate);
            _fixTabTemplate.Location = new Point(4, 32);
            _fixTabTemplate.Name = "_fixTabTemplate";
            _fixTabTemplate.Padding = new Padding(6);
            _fixTabTemplate.Size = new Size(414, 245);
            _fixTabTemplate.TabIndex = 2;
            _fixTabTemplate.Text = "模板";
            // 
            // _flpFixColTemplate
            // 
            _flpFixColTemplate.AutoScroll = true;
            _flpFixColTemplate.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _flpFixColTemplate.Controls.Add(_lblFixTemplateHint);
            _flpFixColTemplate.Controls.Add(_flpFixRowTpl1);
            _flpFixColTemplate.Controls.Add(_flpFixRowTpl2);
            _flpFixColTemplate.Controls.Add(_flpFixRowTpl3);
            _flpFixColTemplate.Controls.Add(_lblFixZone);
            _flpFixColTemplate.Controls.Add(_lblFixTemplate);
            _flpFixColTemplate.Dock = DockStyle.Fill;
            _flpFixColTemplate.FlowDirection = FlowDirection.TopDown;
            _flpFixColTemplate.Location = new Point(6, 6);
            _flpFixColTemplate.Margin = new Padding(0);
            _flpFixColTemplate.Name = "_flpFixColTemplate";
            _flpFixColTemplate.Padding = new Padding(4);
            _flpFixColTemplate.Size = new Size(402, 233);
            _flpFixColTemplate.TabIndex = 0;
            _flpFixColTemplate.WrapContents = false;
            // 
            // _lblFixTemplateHint
            // 
            _lblFixTemplateHint.AutoSize = true;
            _lblFixTemplateHint.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lblFixTemplateHint.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblFixTemplateHint.Location = new Point(4, 4);
            _lblFixTemplateHint.Margin = new Padding(0, 0, 0, 8);
            _lblFixTemplateHint.MaximumSize = new Size(340, 0);
            _lblFixTemplateHint.Name = "_lblFixTemplateHint";
            _lblFixTemplateHint.Size = new Size(330, 34);
            _lblFixTemplateHint.TabIndex = 0;
            _lblFixTemplateHint.Text = "空治具用于扣除针床背景；金样板是“放对了”的基准。两者都必须和正式检测时的机位、光源一致。";
            // 
            // _flpFixRowTpl1
            // 
            _flpFixRowTpl1.AutoSize = true;
            _flpFixRowTpl1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _flpFixRowTpl1.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _flpFixRowTpl1.Controls.Add(_btnFixCaptureEmpty);
            _flpFixRowTpl1.Controls.Add(_btnFixCaptureGolden);
            _flpFixRowTpl1.Location = new Point(4, 46);
            _flpFixRowTpl1.Margin = new Padding(0, 0, 0, 6);
            _flpFixRowTpl1.Name = "_flpFixRowTpl1";
            _flpFixRowTpl1.Size = new Size(240, 36);
            _flpFixRowTpl1.TabIndex = 1;
            _flpFixRowTpl1.WrapContents = false;
            // 
            // _btnFixCaptureEmpty
            // 
            _btnFixCaptureEmpty.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnFixCaptureEmpty.BackColor = Color.White;
            _btnFixCaptureEmpty.Cursor = Cursors.Hand;
            _btnFixCaptureEmpty.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnFixCaptureEmpty.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnFixCaptureEmpty.FlatStyle = FlatStyle.Flat;
            _btnFixCaptureEmpty.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnFixCaptureEmpty.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnFixCaptureEmpty.Location = new Point(0, 2);
            _btnFixCaptureEmpty.Margin = new Padding(0, 2, 8, 0);
            _btnFixCaptureEmpty.MinimumSize = new Size(112, 34);
            _btnFixCaptureEmpty.Name = "_btnFixCaptureEmpty";
            _btnFixCaptureEmpty.Padding = new Padding(10, 0, 10, 0);
            _btnFixCaptureEmpty.Size = new Size(112, 34);
            _btnFixCaptureEmpty.TabIndex = 0;
            _btnFixCaptureEmpty.Text = "采集空治具";
            _btnFixCaptureEmpty.UseVisualStyleBackColor = false;
            // 
            // _btnFixCaptureGolden
            // 
            _btnFixCaptureGolden.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnFixCaptureGolden.BackColor = Color.FromArgb((int)(byte)22, (int)(byte)138, (int)(byte)88);
            _btnFixCaptureGolden.Cursor = Cursors.Hand;
            _btnFixCaptureGolden.FlatAppearance.BorderSize = 0;
            _btnFixCaptureGolden.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)18, (int)(byte)118, (int)(byte)74);
            _btnFixCaptureGolden.FlatStyle = FlatStyle.Flat;
            _btnFixCaptureGolden.Font = new Font("Microsoft YaHei UI", 9.5F, FontStyle.Bold);
            _btnFixCaptureGolden.ForeColor = Color.White;
            _btnFixCaptureGolden.Location = new Point(120, 2);
            _btnFixCaptureGolden.Margin = new Padding(0, 2, 8, 0);
            _btnFixCaptureGolden.MinimumSize = new Size(112, 34);
            _btnFixCaptureGolden.Name = "_btnFixCaptureGolden";
            _btnFixCaptureGolden.Padding = new Padding(10, 0, 10, 0);
            _btnFixCaptureGolden.Size = new Size(112, 34);
            _btnFixCaptureGolden.TabIndex = 1;
            _btnFixCaptureGolden.Text = "采集金样板";
            _btnFixCaptureGolden.UseVisualStyleBackColor = false;
            // 
            // _flpFixRowTpl2
            // 
            _flpFixRowTpl2.AutoSize = true;
            _flpFixRowTpl2.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _flpFixRowTpl2.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _flpFixRowTpl2.Controls.Add(_btnFixRebuildSeg);
            _flpFixRowTpl2.Location = new Point(4, 88);
            _flpFixRowTpl2.Margin = new Padding(0, 0, 0, 6);
            _flpFixRowTpl2.Name = "_flpFixRowTpl2";
            _flpFixRowTpl2.Size = new Size(180, 36);
            _flpFixRowTpl2.TabIndex = 2;
            _flpFixRowTpl2.WrapContents = false;
            // 
            // _btnFixRebuildSeg
            // 
            _btnFixRebuildSeg.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnFixRebuildSeg.BackColor = Color.White;
            _btnFixRebuildSeg.Cursor = Cursors.Hand;
            _btnFixRebuildSeg.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnFixRebuildSeg.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnFixRebuildSeg.FlatStyle = FlatStyle.Flat;
            _btnFixRebuildSeg.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnFixRebuildSeg.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnFixRebuildSeg.Location = new Point(0, 2);
            _btnFixRebuildSeg.Margin = new Padding(0, 2, 8, 0);
            _btnFixRebuildSeg.MinimumSize = new Size(172, 34);
            _btnFixRebuildSeg.Name = "_btnFixRebuildSeg";
            _btnFixRebuildSeg.Padding = new Padding(10, 0, 10, 0);
            _btnFixRebuildSeg.Size = new Size(172, 34);
            _btnFixRebuildSeg.TabIndex = 0;
            _btnFixRebuildSeg.Text = "用金样板重建分割参数";
            _btnFixRebuildSeg.UseVisualStyleBackColor = false;
            // 
            // _flpFixRowTpl3
            // 
            _flpFixRowTpl3.AutoSize = true;
            _flpFixRowTpl3.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _flpFixRowTpl3.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _flpFixRowTpl3.Controls.Add(_btnFixZoneAuto);
            _flpFixRowTpl3.Controls.Add(_btnFixZoneDraw);
            _flpFixRowTpl3.Controls.Add(_btnFixZoneClear);
            _flpFixRowTpl3.Location = new Point(4, 130);
            _flpFixRowTpl3.Margin = new Padding(0, 0, 0, 6);
            _flpFixRowTpl3.Name = "_flpFixRowTpl3";
            _flpFixRowTpl3.Size = new Size(364, 42);
            _flpFixRowTpl3.TabIndex = 3;
            _flpFixRowTpl3.WrapContents = false;
            // 
            // _btnFixZoneAuto
            // 
            _btnFixZoneAuto.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnFixZoneAuto.BackColor = Color.White;
            _btnFixZoneAuto.Cursor = Cursors.Hand;
            _btnFixZoneAuto.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnFixZoneAuto.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnFixZoneAuto.FlatStyle = FlatStyle.Flat;
            _btnFixZoneAuto.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnFixZoneAuto.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnFixZoneAuto.Location = new Point(0, 2);
            _btnFixZoneAuto.Margin = new Padding(0, 2, 8, 0);
            _btnFixZoneAuto.MinimumSize = new Size(172, 34);
            _btnFixZoneAuto.Name = "_btnFixZoneAuto";
            _btnFixZoneAuto.Padding = new Padding(10, 0, 10, 0);
            _btnFixZoneAuto.Size = new Size(172, 34);
            _btnFixZoneAuto.TabIndex = 0;
            _btnFixZoneAuto.Text = "按金样板生成安全区";
            _btnFixZoneAuto.UseVisualStyleBackColor = false;
            // 
            // _btnFixZoneDraw
            // 
            _btnFixZoneDraw.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnFixZoneDraw.BackColor = Color.White;
            _btnFixZoneDraw.Cursor = Cursors.Hand;
            _btnFixZoneDraw.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnFixZoneDraw.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnFixZoneDraw.FlatStyle = FlatStyle.Flat;
            _btnFixZoneDraw.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnFixZoneDraw.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnFixZoneDraw.Location = new Point(180, 2);
            _btnFixZoneDraw.Margin = new Padding(0, 2, 8, 0);
            _btnFixZoneDraw.MinimumSize = new Size(104, 34);
            _btnFixZoneDraw.Name = "_btnFixZoneDraw";
            _btnFixZoneDraw.Padding = new Padding(10, 0, 10, 0);
            _btnFixZoneDraw.Size = new Size(104, 34);
            _btnFixZoneDraw.TabIndex = 1;
            _btnFixZoneDraw.Text = "框选安全区";
            _btnFixZoneDraw.UseVisualStyleBackColor = false;
            // 
            // _btnFixZoneClear
            // 
            _btnFixZoneClear.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnFixZoneClear.BackColor = Color.White;
            _btnFixZoneClear.Cursor = Cursors.Hand;
            _btnFixZoneClear.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnFixZoneClear.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnFixZoneClear.FlatStyle = FlatStyle.Flat;
            _btnFixZoneClear.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnFixZoneClear.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnFixZoneClear.Location = new Point(296, 4);
            _btnFixZoneClear.Margin = new Padding(4);
            _btnFixZoneClear.MinimumSize = new Size(64, 34);
            _btnFixZoneClear.Name = "_btnFixZoneClear";
            _btnFixZoneClear.Padding = new Padding(10, 0, 10, 0);
            _btnFixZoneClear.Size = new Size(64, 34);
            _btnFixZoneClear.TabIndex = 2;
            _btnFixZoneClear.Text = "清除";
            _btnFixZoneClear.UseVisualStyleBackColor = false;
            // 
            // _lblFixZone
            // 
            _lblFixZone.AutoSize = true;
            _lblFixZone.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lblFixZone.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblFixZone.Location = new Point(4, 180);
            _lblFixZone.Margin = new Padding(0, 2, 0, 8);
            _lblFixZone.MaximumSize = new Size(340, 0);
            _lblFixZone.Name = "_lblFixZone";
            _lblFixZone.Size = new Size(92, 17);
            _lblFixZone.TabIndex = 4;
            _lblFixZone.Text = "安全区：未设置";
            // 
            // _lblFixTemplate
            // 
            _lblFixTemplate.AutoSize = true;
            _lblFixTemplate.Font = new Font("Microsoft YaHei UI", 9.5F);
            _lblFixTemplate.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _lblFixTemplate.Location = new Point(4, 209);
            _lblFixTemplate.Margin = new Padding(0, 4, 0, 8);
            _lblFixTemplate.MaximumSize = new Size(340, 0);
            _lblFixTemplate.Name = "_lblFixTemplate";
            _lblFixTemplate.Size = new Size(100, 19);
            _lblFixTemplate.TabIndex = 5;
            _lblFixTemplate.Text = "尚未采集模板。";
            // 
            // _fixTabThreshold
            // 
            _fixTabThreshold.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _fixTabThreshold.Controls.Add(_flpFixColThreshold);
            _fixTabThreshold.Location = new Point(4, 32);
            _fixTabThreshold.Name = "_fixTabThreshold";
            _fixTabThreshold.Padding = new Padding(6);
            _fixTabThreshold.Size = new Size(414, 245);
            _fixTabThreshold.TabIndex = 3;
            _fixTabThreshold.Text = "阈值";
            // 
            // _flpFixColThreshold
            // 
            _flpFixColThreshold.AutoScroll = true;
            _flpFixColThreshold.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _flpFixColThreshold.Controls.Add(_flpFixRowJudgeMode);
            _flpFixColThreshold.Controls.Add(_lblFixJudgeModeHint);
            _flpFixColThreshold.Controls.Add(_flpFixRowMargin);
            _flpFixColThreshold.Controls.Add(_flpFixRowOverflow);
            _flpFixColThreshold.Controls.Add(_flpFixRowPos);
            _flpFixColThreshold.Controls.Add(_flpFixRowPosTrack);
            _flpFixColThreshold.Controls.Add(_lblFixPosHint);
            _flpFixColThreshold.Controls.Add(_flpFixRowAngle);
            _flpFixColThreshold.Controls.Add(_flpFixRowAngleTrack);
            _flpFixColThreshold.Controls.Add(_lblFixAngleHint);
            _flpFixColThreshold.Controls.Add(_flpFixRowShape);
            _flpFixColThreshold.Controls.Add(_lblFixShapeHint);
            _flpFixColThreshold.Controls.Add(_flpFixRowConfirm);
            _flpFixColThreshold.Controls.Add(_lblFixConfirmHint);
            _flpFixColThreshold.Controls.Add(_flpFixRowPreset);
            _flpFixColThreshold.Controls.Add(_lblFixTolSummary);
            _flpFixColThreshold.Controls.Add(_flpFixRowSaveTol);
            _flpFixColThreshold.Controls.Add(_pnlFixAdv);
            _flpFixColThreshold.Dock = DockStyle.Fill;
            _flpFixColThreshold.FlowDirection = FlowDirection.TopDown;
            _flpFixColThreshold.Location = new Point(6, 6);
            _flpFixColThreshold.Margin = new Padding(0);
            _flpFixColThreshold.Name = "_flpFixColThreshold";
            _flpFixColThreshold.Padding = new Padding(4);
            _flpFixColThreshold.Size = new Size(402, 233);
            _flpFixColThreshold.TabIndex = 0;
            _flpFixColThreshold.WrapContents = false;
            // 
            // _flpFixRowJudgeMode
            // 
            _flpFixRowJudgeMode.AutoSize = true;
            _flpFixRowJudgeMode.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _flpFixRowJudgeMode.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _flpFixRowJudgeMode.Controls.Add(_lblFixJudgeMode);
            _flpFixRowJudgeMode.Controls.Add(_cboFixJudgeMode);
            _flpFixRowJudgeMode.Location = new Point(4, 4);
            _flpFixRowJudgeMode.Margin = new Padding(0, 0, 0, 6);
            _flpFixRowJudgeMode.Name = "_flpFixRowJudgeMode";
            _flpFixRowJudgeMode.Size = new Size(207, 33);
            _flpFixRowJudgeMode.TabIndex = 0;
            _flpFixRowJudgeMode.WrapContents = false;
            // 
            // _lblFixJudgeMode
            // 
            _lblFixJudgeMode.AutoSize = true;
            _lblFixJudgeMode.Font = new Font("Microsoft YaHei UI", 9.5F);
            _lblFixJudgeMode.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _lblFixJudgeMode.Location = new Point(3, 0);
            _lblFixJudgeMode.Name = "_lblFixJudgeMode";
            _lblFixJudgeMode.Size = new Size(61, 19);
            _lblFixJudgeMode.TabIndex = 0;
            _lblFixJudgeMode.Text = "判定方式";
            // 
            // _cboFixJudgeMode
            // 
            _cboFixJudgeMode.DropDownStyle = ComboBoxStyle.DropDownList;
            _cboFixJudgeMode.Font = new Font("Microsoft YaHei UI", 9.5F);
            _cboFixJudgeMode.Items.AddRange(new object[] { "形状匹配（旧）", "安全区（像素）" });
            _cboFixJudgeMode.Location = new Point(67, 6);
            _cboFixJudgeMode.Margin = new Padding(0, 6, 8, 0);
            _cboFixJudgeMode.Name = "_cboFixJudgeMode";
            _cboFixJudgeMode.Size = new Size(132, 27);
            _cboFixJudgeMode.TabIndex = 1;
            // 
            // _lblFixJudgeModeHint
            // 
            _lblFixJudgeModeHint.AutoSize = true;
            _lblFixJudgeModeHint.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lblFixJudgeModeHint.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblFixJudgeModeHint.Location = new Point(4, 43);
            _lblFixJudgeModeHint.Margin = new Padding(0, 0, 0, 8);
            _lblFixJudgeModeHint.MaximumSize = new Size(352, 0);
            _lblFixJudgeModeHint.Name = "_lblFixJudgeModeHint";
            _lblFixJudgeModeHint.Size = new Size(346, 34);
            _lblFixJudgeModeHint.TabIndex = 1;
            _lblFixJudgeModeHint.Text = "安全区模式不需要标定、不用 mm：只看产品外沿有没有越出安全区、边距够不够。";
            // 
            // _flpFixRowMargin
            // 
            _flpFixRowMargin.AutoSize = true;
            _flpFixRowMargin.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _flpFixRowMargin.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _flpFixRowMargin.Controls.Add(_lblFixMargin);
            _flpFixRowMargin.Controls.Add(_numFixMarginPx);
            _flpFixRowMargin.Controls.Add(_lblFixMarginUnit);
            _flpFixRowMargin.Location = new Point(4, 85);
            _flpFixRowMargin.Margin = new Padding(0, 0, 0, 6);
            _flpFixRowMargin.Name = "_flpFixRowMargin";
            _flpFixRowMargin.Size = new Size(169, 30);
            _flpFixRowMargin.TabIndex = 2;
            _flpFixRowMargin.WrapContents = false;
            // 
            // _lblFixMargin
            // 
            _lblFixMargin.AutoSize = true;
            _lblFixMargin.Font = new Font("Microsoft YaHei UI", 9.5F);
            _lblFixMargin.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _lblFixMargin.Location = new Point(3, 0);
            _lblFixMargin.Name = "_lblFixMargin";
            _lblFixMargin.Size = new Size(61, 19);
            _lblFixMargin.TabIndex = 0;
            _lblFixMargin.Text = "边距下限";
            // 
            // _numFixMarginPx
            // 
            _numFixMarginPx.Font = new Font("Microsoft YaHei UI", 9.5F);
            _numFixMarginPx.Location = new Point(67, 6);
            _numFixMarginPx.Margin = new Padding(0, 6, 8, 0);
            _numFixMarginPx.Maximum = new decimal(new int[] { 200, 0, 0, 0 });
            _numFixMarginPx.Name = "_numFixMarginPx";
            _numFixMarginPx.Size = new Size(64, 24);
            _numFixMarginPx.TabIndex = 1;
            _numFixMarginPx.Value = new decimal(new int[] { 10, 0, 0, 0 });
            // 
            // _lblFixMarginUnit
            // 
            _lblFixMarginUnit.AutoSize = true;
            _lblFixMarginUnit.Font = new Font("Microsoft YaHei UI", 9.5F);
            _lblFixMarginUnit.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _lblFixMarginUnit.Location = new Point(142, 0);
            _lblFixMarginUnit.Name = "_lblFixMarginUnit";
            _lblFixMarginUnit.Size = new Size(24, 19);
            _lblFixMarginUnit.TabIndex = 2;
            _lblFixMarginUnit.Text = "px";
            // 
            // _flpFixRowOverflow
            // 
            _flpFixRowOverflow.AutoSize = true;
            _flpFixRowOverflow.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _flpFixRowOverflow.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _flpFixRowOverflow.Controls.Add(_lblFixOverflow);
            _flpFixRowOverflow.Controls.Add(_numFixOverflowPx);
            _flpFixRowOverflow.Controls.Add(_lblFixOverflowUnit);
            _flpFixRowOverflow.Location = new Point(4, 121);
            _flpFixRowOverflow.Margin = new Padding(0, 0, 0, 6);
            _flpFixRowOverflow.Name = "_flpFixRowOverflow";
            _flpFixRowOverflow.Size = new Size(175, 30);
            _flpFixRowOverflow.TabIndex = 3;
            _flpFixRowOverflow.WrapContents = false;
            // 
            // _lblFixOverflow
            // 
            _lblFixOverflow.AutoSize = true;
            _lblFixOverflow.Font = new Font("Microsoft YaHei UI", 9.5F);
            _lblFixOverflow.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _lblFixOverflow.Location = new Point(3, 0);
            _lblFixOverflow.Name = "_lblFixOverflow";
            _lblFixOverflow.Size = new Size(61, 19);
            _lblFixOverflow.TabIndex = 0;
            _lblFixOverflow.Text = "允许越界";
            // 
            // _numFixOverflowPx
            // 
            _numFixOverflowPx.Font = new Font("Microsoft YaHei UI", 9.5F);
            _numFixOverflowPx.Increment = new decimal(new int[] { 10, 0, 0, 0 });
            _numFixOverflowPx.Location = new Point(67, 6);
            _numFixOverflowPx.Margin = new Padding(0, 6, 8, 0);
            _numFixOverflowPx.Maximum = new decimal(new int[] { 5000, 0, 0, 0 });
            _numFixOverflowPx.Name = "_numFixOverflowPx";
            _numFixOverflowPx.Size = new Size(70, 24);
            _numFixOverflowPx.TabIndex = 1;
            // 
            // _lblFixOverflowUnit
            // 
            _lblFixOverflowUnit.AutoSize = true;
            _lblFixOverflowUnit.Font = new Font("Microsoft YaHei UI", 9.5F);
            _lblFixOverflowUnit.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _lblFixOverflowUnit.Location = new Point(148, 0);
            _lblFixOverflowUnit.Name = "_lblFixOverflowUnit";
            _lblFixOverflowUnit.Size = new Size(24, 19);
            _lblFixOverflowUnit.TabIndex = 2;
            _lblFixOverflowUnit.Text = "px";
            // 
            // _flpFixRowPos
            // 
            _flpFixRowPos.AutoSize = true;
            _flpFixRowPos.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _flpFixRowPos.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _flpFixRowPos.Controls.Add(_lblFixPos);
            _flpFixRowPos.Controls.Add(_numFixPos);
            _flpFixRowPos.Controls.Add(_lblFixPosUnit);
            _flpFixRowPos.Location = new Point(4, 157);
            _flpFixRowPos.Margin = new Padding(0, 0, 0, 6);
            _flpFixRowPos.Name = "_flpFixRowPos";
            _flpFixRowPos.Size = new Size(204, 30);
            _flpFixRowPos.TabIndex = 4;
            _flpFixRowPos.WrapContents = false;
            // 
            // _lblFixPos
            // 
            _lblFixPos.AutoSize = true;
            _lblFixPos.Font = new Font("Microsoft YaHei UI", 9.5F);
            _lblFixPos.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _lblFixPos.Location = new Point(3, 0);
            _lblFixPos.Name = "_lblFixPos";
            _lblFixPos.Size = new Size(87, 19);
            _lblFixPos.TabIndex = 0;
            _lblFixPos.Text = "位置允许偏移";
            // 
            // _numFixPos
            // 
            _numFixPos.DecimalPlaces = 1;
            _numFixPos.Font = new Font("Microsoft YaHei UI", 9.5F);
            _numFixPos.Increment = new decimal(new int[] { 5, 0, 0, 65536 });
            _numFixPos.Location = new Point(93, 6);
            _numFixPos.Margin = new Padding(0, 6, 8, 0);
            _numFixPos.Maximum = new decimal(new int[] { 15, 0, 0, 0 });
            _numFixPos.Minimum = new decimal(new int[] { 5, 0, 0, 65536 });
            _numFixPos.Name = "_numFixPos";
            _numFixPos.Size = new Size(64, 24);
            _numFixPos.TabIndex = 1;
            _numFixPos.Value = new decimal(new int[] { 5, 0, 0, 0 });
            // 
            // _lblFixPosUnit
            // 
            _lblFixPosUnit.AutoSize = true;
            _lblFixPosUnit.Font = new Font("Microsoft YaHei UI", 9.5F);
            _lblFixPosUnit.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _lblFixPosUnit.Location = new Point(168, 0);
            _lblFixPosUnit.Name = "_lblFixPosUnit";
            _lblFixPosUnit.Size = new Size(33, 19);
            _lblFixPosUnit.TabIndex = 2;
            _lblFixPosUnit.Text = "mm";
            // 
            // _flpFixRowPosTrack
            // 
            _flpFixRowPosTrack.AutoSize = true;
            _flpFixRowPosTrack.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _flpFixRowPosTrack.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _flpFixRowPosTrack.Controls.Add(_trkFixPos);
            _flpFixRowPosTrack.Location = new Point(4, 193);
            _flpFixRowPosTrack.Margin = new Padding(0, 0, 0, 6);
            _flpFixRowPosTrack.Name = "_flpFixRowPosTrack";
            _flpFixRowPosTrack.Size = new Size(252, 36);
            _flpFixRowPosTrack.TabIndex = 5;
            _flpFixRowPosTrack.WrapContents = false;
            // 
            // _trkFixPos
            // 
            _trkFixPos.AutoSize = false;
            _trkFixPos.LargeChange = 2;
            _trkFixPos.Location = new Point(0, 2);
            _trkFixPos.Margin = new Padding(0, 2, 0, 4);
            _trkFixPos.Maximum = 30;
            _trkFixPos.Minimum = 1;
            _trkFixPos.Name = "_trkFixPos";
            _trkFixPos.Size = new Size(252, 30);
            _trkFixPos.TabIndex = 0;
            _trkFixPos.TickStyle = TickStyle.None;
            _trkFixPos.Value = 10;
            // 
            // _lblFixPosHint
            // 
            _lblFixPosHint.AutoSize = true;
            _lblFixPosHint.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lblFixPosHint.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblFixPosHint.Location = new Point(4, 235);
            _lblFixPosHint.Margin = new Padding(0, 0, 0, 8);
            _lblFixPosHint.MaximumSize = new Size(352, 0);
            _lblFixPosHint.Name = "_lblFixPosHint";
            _lblFixPosHint.Size = new Size(340, 34);
            _lblFixPosHint.TabIndex = 6;
            _lblFixPosHint.Text = "调大：轻微歪放也放行；调小更严。压损边界约 10mm，建议 3–8mm。";
            // 
            // _flpFixRowAngle
            // 
            _flpFixRowAngle.AutoSize = true;
            _flpFixRowAngle.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _flpFixRowAngle.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _flpFixRowAngle.Controls.Add(_lblFixAngleNum);
            _flpFixRowAngle.Controls.Add(_numFixAngle);
            _flpFixRowAngle.Controls.Add(_lblFixAngleUnit);
            _flpFixRowAngle.Location = new Point(4, 277);
            _flpFixRowAngle.Margin = new Padding(0, 0, 0, 6);
            _flpFixRowAngle.Name = "_flpFixRowAngle";
            _flpFixRowAngle.Size = new Size(185, 30);
            _flpFixRowAngle.TabIndex = 7;
            _flpFixRowAngle.WrapContents = false;
            // 
            // _lblFixAngleNum
            // 
            _lblFixAngleNum.AutoSize = true;
            _lblFixAngleNum.Font = new Font("Microsoft YaHei UI", 9.5F);
            _lblFixAngleNum.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _lblFixAngleNum.Location = new Point(3, 0);
            _lblFixAngleNum.Name = "_lblFixAngleNum";
            _lblFixAngleNum.Size = new Size(87, 19);
            _lblFixAngleNum.TabIndex = 0;
            _lblFixAngleNum.Text = "角度允许偏转";
            // 
            // _numFixAngle
            // 
            _numFixAngle.DecimalPlaces = 1;
            _numFixAngle.Font = new Font("Microsoft YaHei UI", 9.5F);
            _numFixAngle.Increment = new decimal(new int[] { 5, 0, 0, 65536 });
            _numFixAngle.Location = new Point(93, 6);
            _numFixAngle.Margin = new Padding(0, 6, 8, 0);
            _numFixAngle.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
            _numFixAngle.Minimum = new decimal(new int[] { 5, 0, 0, 65536 });
            _numFixAngle.Name = "_numFixAngle";
            _numFixAngle.Size = new Size(64, 24);
            _numFixAngle.TabIndex = 1;
            _numFixAngle.Value = new decimal(new int[] { 35, 0, 0, 65536 });
            // 
            // _lblFixAngleUnit
            // 
            _lblFixAngleUnit.AutoSize = true;
            _lblFixAngleUnit.Font = new Font("Microsoft YaHei UI", 9.5F);
            _lblFixAngleUnit.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _lblFixAngleUnit.Location = new Point(168, 0);
            _lblFixAngleUnit.Name = "_lblFixAngleUnit";
            _lblFixAngleUnit.Size = new Size(14, 19);
            _lblFixAngleUnit.TabIndex = 2;
            _lblFixAngleUnit.Text = "°";
            // 
            // _flpFixRowAngleTrack
            // 
            _flpFixRowAngleTrack.AutoSize = true;
            _flpFixRowAngleTrack.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _flpFixRowAngleTrack.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _flpFixRowAngleTrack.Controls.Add(_trkFixAngle);
            _flpFixRowAngleTrack.Location = new Point(4, 313);
            _flpFixRowAngleTrack.Margin = new Padding(0, 0, 0, 6);
            _flpFixRowAngleTrack.Name = "_flpFixRowAngleTrack";
            _flpFixRowAngleTrack.Size = new Size(252, 36);
            _flpFixRowAngleTrack.TabIndex = 8;
            _flpFixRowAngleTrack.WrapContents = false;
            // 
            // _trkFixAngle
            // 
            _trkFixAngle.AutoSize = false;
            _trkFixAngle.LargeChange = 2;
            _trkFixAngle.Location = new Point(0, 2);
            _trkFixAngle.Margin = new Padding(0, 2, 0, 4);
            _trkFixAngle.Maximum = 20;
            _trkFixAngle.Minimum = 1;
            _trkFixAngle.Name = "_trkFixAngle";
            _trkFixAngle.Size = new Size(252, 30);
            _trkFixAngle.TabIndex = 0;
            _trkFixAngle.TickStyle = TickStyle.None;
            _trkFixAngle.Value = 7;
            // 
            // _lblFixAngleHint
            // 
            _lblFixAngleHint.AutoSize = true;
            _lblFixAngleHint.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lblFixAngleHint.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblFixAngleHint.Location = new Point(4, 355);
            _lblFixAngleHint.Margin = new Padding(0, 0, 0, 8);
            _lblFixAngleHint.MaximumSize = new Size(352, 0);
            _lblFixAngleHint.Name = "_lblFixAngleHint";
            _lblFixAngleHint.Size = new Size(341, 17);
            _lblFixAngleHint.TabIndex = 9;
            _lblFixAngleHint.Text = "调大：允许放歪一点；调小更严。压损边界约 7°，建议 2–5°。";
            // 
            // _flpFixRowShape
            // 
            _flpFixRowShape.AutoSize = true;
            _flpFixRowShape.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _flpFixRowShape.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _flpFixRowShape.Controls.Add(_lblFixShape);
            _flpFixRowShape.Controls.Add(_cboFixShape);
            _flpFixRowShape.Location = new Point(4, 380);
            _flpFixRowShape.Margin = new Padding(0, 0, 0, 6);
            _flpFixRowShape.Name = "_flpFixRowShape";
            _flpFixRowShape.Size = new Size(220, 33);
            _flpFixRowShape.TabIndex = 10;
            _flpFixRowShape.WrapContents = false;
            // 
            // _lblFixShape
            // 
            _lblFixShape.AutoSize = true;
            _lblFixShape.Font = new Font("Microsoft YaHei UI", 9.5F);
            _lblFixShape.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _lblFixShape.Location = new Point(3, 0);
            _lblFixShape.Name = "_lblFixShape";
            _lblFixShape.Size = new Size(74, 19);
            _lblFixShape.TabIndex = 0;
            _lblFixShape.Text = "形状严格度";
            // 
            // _cboFixShape
            // 
            _cboFixShape.DropDownStyle = ComboBoxStyle.DropDownList;
            _cboFixShape.Font = new Font("Microsoft YaHei UI", 9.5F);
            _cboFixShape.Items.AddRange(new object[] { "宽松（0.80）", "标准（0.85）", "严格（0.90）", "自定义" });
            _cboFixShape.Location = new Point(80, 6);
            _cboFixShape.Margin = new Padding(0, 6, 8, 0);
            _cboFixShape.Name = "_cboFixShape";
            _cboFixShape.Size = new Size(132, 27);
            _cboFixShape.TabIndex = 1;
            // 
            // _lblFixShapeHint
            // 
            _lblFixShapeHint.AutoSize = true;
            _lblFixShapeHint.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lblFixShapeHint.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblFixShapeHint.Location = new Point(4, 419);
            _lblFixShapeHint.Margin = new Padding(0, 0, 0, 8);
            _lblFixShapeHint.MaximumSize = new Size(352, 0);
            _lblFixShapeHint.Name = "_lblFixShapeHint";
            _lblFixShapeHint.Size = new Size(349, 34);
            _lblFixShapeHint.TabIndex = 11;
            _lblFixShapeHint.Text = "宽松/标准/严格对应 IoU 0.80 / 0.85 / 0.90；充实度按金样板自动留余量。";
            // 
            // _flpFixRowConfirm
            // 
            _flpFixRowConfirm.AutoSize = true;
            _flpFixRowConfirm.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _flpFixRowConfirm.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _flpFixRowConfirm.Controls.Add(_lblFixConfirm);
            _flpFixRowConfirm.Controls.Add(_numFixConfirm);
            _flpFixRowConfirm.Controls.Add(_lblFixConfirmUnit);
            _flpFixRowConfirm.Location = new Point(4, 461);
            _flpFixRowConfirm.Margin = new Padding(0, 0, 0, 6);
            _flpFixRowConfirm.Name = "_flpFixRowConfirm";
            _flpFixRowConfirm.Size = new Size(159, 30);
            _flpFixRowConfirm.TabIndex = 12;
            _flpFixRowConfirm.WrapContents = false;
            // 
            // _lblFixConfirm
            // 
            _lblFixConfirm.AutoSize = true;
            _lblFixConfirm.Font = new Font("Microsoft YaHei UI", 9.5F);
            _lblFixConfirm.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _lblFixConfirm.Location = new Point(3, 0);
            _lblFixConfirm.Name = "_lblFixConfirm";
            _lblFixConfirm.Size = new Size(61, 19);
            _lblFixConfirm.TabIndex = 0;
            _lblFixConfirm.Text = "确认帧数";
            // 
            // _numFixConfirm
            // 
            _numFixConfirm.Font = new Font("Microsoft YaHei UI", 9.5F);
            _numFixConfirm.Location = new Point(67, 6);
            _numFixConfirm.Margin = new Padding(0, 6, 8, 0);
            _numFixConfirm.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
            _numFixConfirm.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            _numFixConfirm.Name = "_numFixConfirm";
            _numFixConfirm.Size = new Size(56, 24);
            _numFixConfirm.TabIndex = 1;
            _numFixConfirm.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // _lblFixConfirmUnit
            // 
            _lblFixConfirmUnit.AutoSize = true;
            _lblFixConfirmUnit.Font = new Font("Microsoft YaHei UI", 9.5F);
            _lblFixConfirmUnit.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _lblFixConfirmUnit.Location = new Point(134, 0);
            _lblFixConfirmUnit.Name = "_lblFixConfirmUnit";
            _lblFixConfirmUnit.Size = new Size(22, 19);
            _lblFixConfirmUnit.TabIndex = 2;
            _lblFixConfirmUnit.Text = "帧";
            // 
            // _lblFixConfirmHint
            // 
            _lblFixConfirmHint.AutoSize = true;
            _lblFixConfirmHint.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lblFixConfirmHint.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblFixConfirmHint.Location = new Point(4, 497);
            _lblFixConfirmHint.Margin = new Padding(0, 0, 0, 8);
            _lblFixConfirmHint.MaximumSize = new Size(352, 0);
            _lblFixConfirmHint.Name = "_lblFixConfirmHint";
            _lblFixConfirmHint.Size = new Size(274, 17);
            _lblFixConfirmHint.TabIndex = 13;
            _lblFixConfirmHint.Text = "连续几帧都 NG 才报警，压掉单帧抖动。建议 2。";
            // 
            // _flpFixRowPreset
            // 
            _flpFixRowPreset.AutoSize = true;
            _flpFixRowPreset.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _flpFixRowPreset.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _flpFixRowPreset.Controls.Add(_lblFixPreset);
            _flpFixRowPreset.Controls.Add(_btnFixPresetLoose);
            _flpFixRowPreset.Controls.Add(_btnFixPresetStd);
            _flpFixRowPreset.Controls.Add(_btnFixPresetStrict);
            _flpFixRowPreset.Location = new Point(4, 522);
            _flpFixRowPreset.Margin = new Padding(0, 0, 0, 6);
            _flpFixRowPreset.Name = "_flpFixRowPreset";
            _flpFixRowPreset.Size = new Size(299, 40);
            _flpFixRowPreset.TabIndex = 14;
            _flpFixRowPreset.WrapContents = false;
            // 
            // _lblFixPreset
            // 
            _lblFixPreset.AutoSize = true;
            _lblFixPreset.Font = new Font("Microsoft YaHei UI", 9.5F);
            _lblFixPreset.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _lblFixPreset.Location = new Point(3, 0);
            _lblFixPreset.Name = "_lblFixPreset";
            _lblFixPreset.Size = new Size(35, 19);
            _lblFixPreset.TabIndex = 0;
            _lblFixPreset.Text = "预设";
            // 
            // _btnFixPresetLoose
            // 
            _btnFixPresetLoose.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnFixPresetLoose.BackColor = Color.White;
            _btnFixPresetLoose.Cursor = Cursors.Hand;
            _btnFixPresetLoose.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnFixPresetLoose.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnFixPresetLoose.FlatStyle = FlatStyle.Flat;
            _btnFixPresetLoose.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnFixPresetLoose.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnFixPresetLoose.Location = new Point(41, 2);
            _btnFixPresetLoose.Margin = new Padding(0, 2, 8, 0);
            _btnFixPresetLoose.MinimumSize = new Size(78, 32);
            _btnFixPresetLoose.Name = "_btnFixPresetLoose";
            _btnFixPresetLoose.Padding = new Padding(10, 0, 10, 0);
            _btnFixPresetLoose.Size = new Size(78, 32);
            _btnFixPresetLoose.TabIndex = 1;
            _btnFixPresetLoose.Text = "宽松";
            _btnFixPresetLoose.UseVisualStyleBackColor = false;
            // 
            // _btnFixPresetStd
            // 
            _btnFixPresetStd.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnFixPresetStd.BackColor = Color.White;
            _btnFixPresetStd.Cursor = Cursors.Hand;
            _btnFixPresetStd.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnFixPresetStd.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnFixPresetStd.FlatStyle = FlatStyle.Flat;
            _btnFixPresetStd.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnFixPresetStd.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnFixPresetStd.Location = new Point(127, 2);
            _btnFixPresetStd.Margin = new Padding(0, 2, 8, 0);
            _btnFixPresetStd.MinimumSize = new Size(78, 32);
            _btnFixPresetStd.Name = "_btnFixPresetStd";
            _btnFixPresetStd.Padding = new Padding(10, 0, 10, 0);
            _btnFixPresetStd.Size = new Size(78, 32);
            _btnFixPresetStd.TabIndex = 2;
            _btnFixPresetStd.Text = "标准";
            _btnFixPresetStd.UseVisualStyleBackColor = false;
            // 
            // _btnFixPresetStrict
            // 
            _btnFixPresetStrict.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnFixPresetStrict.BackColor = Color.White;
            _btnFixPresetStrict.Cursor = Cursors.Hand;
            _btnFixPresetStrict.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnFixPresetStrict.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnFixPresetStrict.FlatStyle = FlatStyle.Flat;
            _btnFixPresetStrict.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnFixPresetStrict.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnFixPresetStrict.Location = new Point(217, 4);
            _btnFixPresetStrict.Margin = new Padding(4);
            _btnFixPresetStrict.MinimumSize = new Size(78, 32);
            _btnFixPresetStrict.Name = "_btnFixPresetStrict";
            _btnFixPresetStrict.Padding = new Padding(10, 0, 10, 0);
            _btnFixPresetStrict.Size = new Size(78, 32);
            _btnFixPresetStrict.TabIndex = 3;
            _btnFixPresetStrict.Text = "严格";
            _btnFixPresetStrict.UseVisualStyleBackColor = false;
            // 
            // _lblFixTolSummary
            // 
            _lblFixTolSummary.AutoSize = true;
            _lblFixTolSummary.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lblFixTolSummary.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblFixTolSummary.Location = new Point(4, 570);
            _lblFixTolSummary.Margin = new Padding(0, 2, 0, 6);
            _lblFixTolSummary.MaximumSize = new Size(360, 0);
            _lblFixTolSummary.Name = "_lblFixTolSummary";
            _lblFixTolSummary.Size = new Size(57, 17);
            _lblFixTolSummary.TabIndex = 15;
            _lblFixTolSummary.Text = "当前：—";
            // 
            // _flpFixRowSaveTol
            // 
            _flpFixRowSaveTol.AutoSize = true;
            _flpFixRowSaveTol.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _flpFixRowSaveTol.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _flpFixRowSaveTol.Controls.Add(_btnFixSaveTol);
            _flpFixRowSaveTol.Controls.Add(_btnFixAdvToggle);
            _flpFixRowSaveTol.Location = new Point(4, 593);
            _flpFixRowSaveTol.Margin = new Padding(0, 0, 0, 6);
            _flpFixRowSaveTol.Name = "_flpFixRowSaveTol";
            _flpFixRowSaveTol.Size = new Size(248, 38);
            _flpFixRowSaveTol.TabIndex = 16;
            _flpFixRowSaveTol.WrapContents = false;
            // 
            // _btnFixSaveTol
            // 
            _btnFixSaveTol.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnFixSaveTol.BackColor = Color.FromArgb((int)(byte)36, (int)(byte)112, (int)(byte)210);
            _btnFixSaveTol.Cursor = Cursors.Hand;
            _btnFixSaveTol.FlatAppearance.BorderSize = 0;
            _btnFixSaveTol.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)28, (int)(byte)96, (int)(byte)186);
            _btnFixSaveTol.FlatStyle = FlatStyle.Flat;
            _btnFixSaveTol.Font = new Font("Microsoft YaHei UI", 9.5F, FontStyle.Bold);
            _btnFixSaveTol.ForeColor = Color.White;
            _btnFixSaveTol.Location = new Point(0, 4);
            _btnFixSaveTol.Margin = new Padding(0, 4, 8, 0);
            _btnFixSaveTol.MinimumSize = new Size(104, 34);
            _btnFixSaveTol.Name = "_btnFixSaveTol";
            _btnFixSaveTol.Padding = new Padding(10, 0, 10, 0);
            _btnFixSaveTol.Size = new Size(104, 34);
            _btnFixSaveTol.TabIndex = 0;
            _btnFixSaveTol.Text = "保存阈值";
            _btnFixSaveTol.UseVisualStyleBackColor = false;
            // 
            // _btnFixAdvToggle
            // 
            _btnFixAdvToggle.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnFixAdvToggle.BackColor = Color.White;
            _btnFixAdvToggle.Cursor = Cursors.Hand;
            _btnFixAdvToggle.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnFixAdvToggle.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnFixAdvToggle.FlatStyle = FlatStyle.Flat;
            _btnFixAdvToggle.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnFixAdvToggle.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnFixAdvToggle.Location = new Point(112, 4);
            _btnFixAdvToggle.Margin = new Padding(0, 4, 8, 0);
            _btnFixAdvToggle.MinimumSize = new Size(128, 34);
            _btnFixAdvToggle.Name = "_btnFixAdvToggle";
            _btnFixAdvToggle.Padding = new Padding(10, 0, 10, 0);
            _btnFixAdvToggle.Size = new Size(128, 34);
            _btnFixAdvToggle.TabIndex = 1;
            _btnFixAdvToggle.Text = "高级参数 ▾";
            _btnFixAdvToggle.UseVisualStyleBackColor = false;
            // 
            // _pnlFixAdv
            // 
            _pnlFixAdv.AutoSize = true;
            _pnlFixAdv.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _pnlFixAdv.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _pnlFixAdv.Controls.Add(_flpFixRowDxDy);
            _pnlFixAdv.Controls.Add(_flpFixRowIouZone);
            _pnlFixAdv.Controls.Add(_flpFixRowArea);
            _pnlFixAdv.Controls.Add(_flpFixRowQuality);
            _pnlFixAdv.Controls.Add(_flpFixRowQuality2);
            _pnlFixAdv.Controls.Add(_flpFixRowDrop);
            _pnlFixAdv.Controls.Add(_lblFixAdvHint);
            _pnlFixAdv.FlowDirection = FlowDirection.TopDown;
            _pnlFixAdv.Location = new Point(4, 641);
            _pnlFixAdv.Margin = new Padding(0, 4, 0, 0);
            _pnlFixAdv.Name = "_pnlFixAdv";
            _pnlFixAdv.Size = new Size(497, 241);
            _pnlFixAdv.TabIndex = 17;
            _pnlFixAdv.Visible = false;
            _pnlFixAdv.WrapContents = false;
            // 
            // _flpFixRowDxDy
            // 
            _flpFixRowDxDy.AutoSize = true;
            _flpFixRowDxDy.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _flpFixRowDxDy.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _flpFixRowDxDy.Controls.Add(_lblFixDx);
            _flpFixRowDxDy.Controls.Add(_numFixDx);
            _flpFixRowDxDy.Controls.Add(_lblFixDy);
            _flpFixRowDxDy.Controls.Add(_numFixDy);
            _flpFixRowDxDy.Location = new Point(0, 0);
            _flpFixRowDxDy.Margin = new Padding(0, 0, 0, 6);
            _flpFixRowDxDy.Name = "_flpFixRowDxDy";
            _flpFixRowDxDy.Size = new Size(326, 30);
            _flpFixRowDxDy.TabIndex = 0;
            _flpFixRowDxDy.WrapContents = false;
            // 
            // _lblFixDx
            // 
            _lblFixDx.AutoSize = true;
            _lblFixDx.Font = new Font("Microsoft YaHei UI", 9.5F);
            _lblFixDx.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _lblFixDx.Location = new Point(3, 0);
            _lblFixDx.Name = "_lblFixDx";
            _lblFixDx.Size = new Size(75, 19);
            _lblFixDx.TabIndex = 0;
            _lblFixDx.Text = "X 公差 mm";
            // 
            // _numFixDx
            // 
            _numFixDx.DecimalPlaces = 2;
            _numFixDx.Font = new Font("Microsoft YaHei UI", 9.5F);
            _numFixDx.Increment = new decimal(new int[] { 5, 0, 0, 65536 });
            _numFixDx.Location = new Point(81, 6);
            _numFixDx.Margin = new Padding(0, 6, 8, 0);
            _numFixDx.Minimum = new decimal(new int[] { 1, 0, 0, 65536 });
            _numFixDx.Name = "_numFixDx";
            _numFixDx.Size = new Size(74, 24);
            _numFixDx.TabIndex = 1;
            _numFixDx.Value = new decimal(new int[] { 5, 0, 0, 0 });
            // 
            // _lblFixDy
            // 
            _lblFixDy.AutoSize = true;
            _lblFixDy.Font = new Font("Microsoft YaHei UI", 9.5F);
            _lblFixDy.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _lblFixDy.Location = new Point(166, 0);
            _lblFixDy.Name = "_lblFixDy";
            _lblFixDy.Size = new Size(75, 19);
            _lblFixDy.TabIndex = 2;
            _lblFixDy.Text = "Y 公差 mm";
            // 
            // _numFixDy
            // 
            _numFixDy.DecimalPlaces = 2;
            _numFixDy.Font = new Font("Microsoft YaHei UI", 9.5F);
            _numFixDy.Increment = new decimal(new int[] { 5, 0, 0, 65536 });
            _numFixDy.Location = new Point(244, 6);
            _numFixDy.Margin = new Padding(0, 6, 8, 0);
            _numFixDy.Minimum = new decimal(new int[] { 1, 0, 0, 65536 });
            _numFixDy.Name = "_numFixDy";
            _numFixDy.Size = new Size(74, 24);
            _numFixDy.TabIndex = 3;
            _numFixDy.Value = new decimal(new int[] { 5, 0, 0, 0 });
            // 
            // _flpFixRowIouZone
            // 
            _flpFixRowIouZone.AutoSize = true;
            _flpFixRowIouZone.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _flpFixRowIouZone.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _flpFixRowIouZone.Controls.Add(_lblFixIou);
            _flpFixRowIouZone.Controls.Add(_numFixIou);
            _flpFixRowIouZone.Controls.Add(_lblFixZoneMm);
            _flpFixRowIouZone.Controls.Add(_numFixZone);
            _flpFixRowIouZone.Controls.Add(_lblFixTheta);
            _flpFixRowIouZone.Controls.Add(_numFixTheta);
            _flpFixRowIouZone.Location = new Point(0, 36);
            _flpFixRowIouZone.Margin = new Padding(0, 0, 0, 6);
            _flpFixRowIouZone.Name = "_flpFixRowIouZone";
            _flpFixRowIouZone.Size = new Size(497, 30);
            _flpFixRowIouZone.TabIndex = 1;
            _flpFixRowIouZone.WrapContents = false;
            // 
            // _lblFixIou
            // 
            _lblFixIou.AutoSize = true;
            _lblFixIou.Font = new Font("Microsoft YaHei UI", 9.5F);
            _lblFixIou.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _lblFixIou.Location = new Point(3, 0);
            _lblFixIou.Name = "_lblFixIou";
            _lblFixIou.Size = new Size(61, 19);
            _lblFixIou.TabIndex = 0;
            _lblFixIou.Text = "IoU 下限";
            // 
            // _numFixIou
            // 
            _numFixIou.DecimalPlaces = 3;
            _numFixIou.Font = new Font("Microsoft YaHei UI", 9.5F);
            _numFixIou.Increment = new decimal(new int[] { 1, 0, 0, 131072 });
            _numFixIou.Location = new Point(67, 6);
            _numFixIou.Margin = new Padding(0, 6, 8, 0);
            _numFixIou.Maximum = new decimal(new int[] { 999, 0, 0, 196608 });
            _numFixIou.Minimum = new decimal(new int[] { 3, 0, 0, 65536 });
            _numFixIou.Name = "_numFixIou";
            _numFixIou.Size = new Size(70, 24);
            _numFixIou.TabIndex = 1;
            _numFixIou.Value = new decimal(new int[] { 85, 0, 0, 131072 });
            // 
            // _lblFixZoneMm
            // 
            _lblFixZoneMm.AutoSize = true;
            _lblFixZoneMm.Font = new Font("Microsoft YaHei UI", 9.5F);
            _lblFixZoneMm.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _lblFixZoneMm.Location = new Point(148, 0);
            _lblFixZoneMm.Name = "_lblFixZoneMm";
            _lblFixZoneMm.Size = new Size(102, 19);
            _lblFixZoneMm.TabIndex = 2;
            _lblFixZoneMm.Text = "安全区外扩 mm";
            // 
            // _numFixZone
            // 
            _numFixZone.DecimalPlaces = 2;
            _numFixZone.Font = new Font("Microsoft YaHei UI", 9.5F);
            _numFixZone.Increment = new decimal(new int[] { 5, 0, 0, 65536 });
            _numFixZone.Location = new Point(253, 6);
            _numFixZone.Margin = new Padding(0, 6, 8, 0);
            _numFixZone.Maximum = new decimal(new int[] { 50, 0, 0, 0 });
            _numFixZone.Name = "_numFixZone";
            _numFixZone.Size = new Size(74, 24);
            _numFixZone.TabIndex = 3;
            _numFixZone.Value = new decimal(new int[] { 5, 0, 0, 0 });
            // 
            // _lblFixTheta
            // 
            _lblFixTheta.AutoSize = true;
            _lblFixTheta.Font = new Font("Microsoft YaHei UI", 9.5F);
            _lblFixTheta.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _lblFixTheta.Location = new Point(338, 0);
            _lblFixTheta.Name = "_lblFixTheta";
            _lblFixTheta.Size = new Size(74, 19);
            _lblFixTheta.TabIndex = 4;
            _lblFixTheta.Text = "角度 公差 °";
            // 
            // _numFixTheta
            // 
            _numFixTheta.DecimalPlaces = 2;
            _numFixTheta.Font = new Font("Microsoft YaHei UI", 9.5F);
            _numFixTheta.Increment = new decimal(new int[] { 5, 0, 0, 65536 });
            _numFixTheta.Location = new Point(415, 6);
            _numFixTheta.Margin = new Padding(0, 6, 8, 0);
            _numFixTheta.Maximum = new decimal(new int[] { 30, 0, 0, 0 });
            _numFixTheta.Minimum = new decimal(new int[] { 1, 0, 0, 65536 });
            _numFixTheta.Name = "_numFixTheta";
            _numFixTheta.Size = new Size(74, 24);
            _numFixTheta.TabIndex = 5;
            _numFixTheta.Value = new decimal(new int[] { 35, 0, 0, 65536 });
            // 
            // _flpFixRowArea
            // 
            _flpFixRowArea.AutoSize = true;
            _flpFixRowArea.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _flpFixRowArea.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _flpFixRowArea.Controls.Add(_lblFixAreaMin);
            _flpFixRowArea.Controls.Add(_numFixAreaMin);
            _flpFixRowArea.Controls.Add(_lblFixAreaMax);
            _flpFixRowArea.Controls.Add(_numFixAreaMax);
            _flpFixRowArea.Location = new Point(0, 72);
            _flpFixRowArea.Margin = new Padding(0, 0, 0, 6);
            _flpFixRowArea.Name = "_flpFixRowArea";
            _flpFixRowArea.Size = new Size(277, 30);
            _flpFixRowArea.TabIndex = 2;
            _flpFixRowArea.WrapContents = false;
            // 
            // _lblFixAreaMin
            // 
            _lblFixAreaMin.AutoSize = true;
            _lblFixAreaMin.Font = new Font("Microsoft YaHei UI", 9.5F);
            _lblFixAreaMin.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _lblFixAreaMin.Location = new Point(3, 0);
            _lblFixAreaMin.Name = "_lblFixAreaMin";
            _lblFixAreaMin.Size = new Size(74, 19);
            _lblFixAreaMin.TabIndex = 0;
            _lblFixAreaMin.Text = "面积比下限";
            // 
            // _numFixAreaMin
            // 
            _numFixAreaMin.DecimalPlaces = 3;
            _numFixAreaMin.Font = new Font("Microsoft YaHei UI", 9.5F);
            _numFixAreaMin.Increment = new decimal(new int[] { 1, 0, 0, 131072 });
            _numFixAreaMin.Location = new Point(80, 6);
            _numFixAreaMin.Margin = new Padding(0, 6, 8, 0);
            _numFixAreaMin.Maximum = new decimal(new int[] { 15, 0, 0, 65536 });
            _numFixAreaMin.Minimum = new decimal(new int[] { 3, 0, 0, 65536 });
            _numFixAreaMin.Name = "_numFixAreaMin";
            _numFixAreaMin.Size = new Size(70, 24);
            _numFixAreaMin.TabIndex = 1;
            _numFixAreaMin.Value = new decimal(new int[] { 9, 0, 0, 65536 });
            // 
            // _lblFixAreaMax
            // 
            _lblFixAreaMax.AutoSize = true;
            _lblFixAreaMax.Font = new Font("Microsoft YaHei UI", 9.5F);
            _lblFixAreaMax.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _lblFixAreaMax.Location = new Point(161, 0);
            _lblFixAreaMax.Name = "_lblFixAreaMax";
            _lblFixAreaMax.Size = new Size(35, 19);
            _lblFixAreaMax.TabIndex = 2;
            _lblFixAreaMax.Text = "上限";
            // 
            // _numFixAreaMax
            // 
            _numFixAreaMax.DecimalPlaces = 3;
            _numFixAreaMax.Font = new Font("Microsoft YaHei UI", 9.5F);
            _numFixAreaMax.Increment = new decimal(new int[] { 1, 0, 0, 131072 });
            _numFixAreaMax.Location = new Point(199, 6);
            _numFixAreaMax.Margin = new Padding(0, 6, 8, 0);
            _numFixAreaMax.Maximum = new decimal(new int[] { 15, 0, 0, 65536 });
            _numFixAreaMax.Minimum = new decimal(new int[] { 3, 0, 0, 65536 });
            _numFixAreaMax.Name = "_numFixAreaMax";
            _numFixAreaMax.Size = new Size(70, 24);
            _numFixAreaMax.TabIndex = 3;
            _numFixAreaMax.Value = new decimal(new int[] { 11, 0, 0, 65536 });
            // 
            // _flpFixRowQuality
            // 
            _flpFixRowQuality.AutoSize = true;
            _flpFixRowQuality.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _flpFixRowQuality.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _flpFixRowQuality.Controls.Add(_lblFixMeanGray);
            _flpFixRowQuality.Controls.Add(_numFixMeanGray);
            _flpFixRowQuality.Controls.Add(_lblFixContrast);
            _flpFixRowQuality.Controls.Add(_numFixContrast);
            _flpFixRowQuality.Location = new Point(0, 108);
            _flpFixRowQuality.Margin = new Padding(0, 0, 0, 6);
            _flpFixRowQuality.Name = "_flpFixRowQuality";
            _flpFixRowQuality.Size = new Size(311, 30);
            _flpFixRowQuality.TabIndex = 3;
            _flpFixRowQuality.WrapContents = false;
            // 
            // _lblFixMeanGray
            // 
            _lblFixMeanGray.AutoSize = true;
            _lblFixMeanGray.Font = new Font("Microsoft YaHei UI", 9.5F);
            _lblFixMeanGray.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _lblFixMeanGray.Location = new Point(3, 0);
            _lblFixMeanGray.Name = "_lblFixMeanGray";
            _lblFixMeanGray.Size = new Size(61, 19);
            _lblFixMeanGray.TabIndex = 0;
            _lblFixMeanGray.Text = "最暗均值";
            // 
            // _numFixMeanGray
            // 
            _numFixMeanGray.Font = new Font("Microsoft YaHei UI", 9.5F);
            _numFixMeanGray.Location = new Point(67, 6);
            _numFixMeanGray.Margin = new Padding(0, 6, 8, 0);
            _numFixMeanGray.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            _numFixMeanGray.Name = "_numFixMeanGray";
            _numFixMeanGray.Size = new Size(74, 24);
            _numFixMeanGray.TabIndex = 1;
            _numFixMeanGray.Value = new decimal(new int[] { 35, 0, 0, 0 });
            // 
            // _lblFixContrast
            // 
            _lblFixContrast.AutoSize = true;
            _lblFixContrast.Font = new Font("Microsoft YaHei UI", 9.5F);
            _lblFixContrast.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _lblFixContrast.Location = new Point(152, 0);
            _lblFixContrast.Name = "_lblFixContrast";
            _lblFixContrast.Size = new Size(74, 19);
            _lblFixContrast.TabIndex = 2;
            _lblFixContrast.Text = "对比度下限";
            // 
            // _numFixContrast
            // 
            _numFixContrast.Font = new Font("Microsoft YaHei UI", 9.5F);
            _numFixContrast.Location = new Point(229, 6);
            _numFixContrast.Margin = new Padding(0, 6, 8, 0);
            _numFixContrast.Maximum = new decimal(new int[] { 120, 0, 0, 0 });
            _numFixContrast.Name = "_numFixContrast";
            _numFixContrast.Size = new Size(74, 24);
            _numFixContrast.TabIndex = 3;
            _numFixContrast.Value = new decimal(new int[] { 12, 0, 0, 0 });
            // 
            // _flpFixRowQuality2
            // 
            _flpFixRowQuality2.AutoSize = true;
            _flpFixRowQuality2.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _flpFixRowQuality2.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _flpFixRowQuality2.Controls.Add(_lblFixHighlight);
            _flpFixRowQuality2.Controls.Add(_numFixHighlight);
            _flpFixRowQuality2.Controls.Add(_lblFixContour);
            _flpFixRowQuality2.Controls.Add(_numFixContour);
            _flpFixRowQuality2.Location = new Point(0, 144);
            _flpFixRowQuality2.Margin = new Padding(0, 0, 0, 6);
            _flpFixRowQuality2.Name = "_flpFixRowQuality2";
            _flpFixRowQuality2.Size = new Size(329, 30);
            _flpFixRowQuality2.TabIndex = 4;
            _flpFixRowQuality2.WrapContents = false;
            // 
            // _lblFixHighlight
            // 
            _lblFixHighlight.AutoSize = true;
            _lblFixHighlight.Font = new Font("Microsoft YaHei UI", 9.5F);
            _lblFixHighlight.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _lblFixHighlight.Location = new Point(3, 0);
            _lblFixHighlight.Name = "_lblFixHighlight";
            _lblFixHighlight.Size = new Size(87, 19);
            _lblFixHighlight.TabIndex = 0;
            _lblFixHighlight.Text = "高光比例上限";
            // 
            // _numFixHighlight
            // 
            _numFixHighlight.DecimalPlaces = 2;
            _numFixHighlight.Font = new Font("Microsoft YaHei UI", 9.5F);
            _numFixHighlight.Increment = new decimal(new int[] { 1, 0, 0, 131072 });
            _numFixHighlight.Location = new Point(93, 6);
            _numFixHighlight.Margin = new Padding(0, 6, 8, 0);
            _numFixHighlight.Maximum = new decimal(new int[] { 1, 0, 0, 0 });
            _numFixHighlight.Name = "_numFixHighlight";
            _numFixHighlight.Size = new Size(70, 24);
            _numFixHighlight.TabIndex = 1;
            _numFixHighlight.Value = new decimal(new int[] { 8, 0, 0, 131072 });
            // 
            // _lblFixContour
            // 
            _lblFixContour.AutoSize = true;
            _lblFixContour.Font = new Font("Microsoft YaHei UI", 9.5F);
            _lblFixContour.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _lblFixContour.Location = new Point(174, 0);
            _lblFixContour.Name = "_lblFixContour";
            _lblFixContour.Size = new Size(74, 19);
            _lblFixContour.TabIndex = 2;
            _lblFixContour.Text = "轮廓分下限";
            // 
            // _numFixContour
            // 
            _numFixContour.DecimalPlaces = 2;
            _numFixContour.Font = new Font("Microsoft YaHei UI", 9.5F);
            _numFixContour.Increment = new decimal(new int[] { 1, 0, 0, 131072 });
            _numFixContour.Location = new Point(251, 6);
            _numFixContour.Margin = new Padding(0, 6, 8, 0);
            _numFixContour.Maximum = new decimal(new int[] { 1, 0, 0, 0 });
            _numFixContour.Minimum = new decimal(new int[] { 3, 0, 0, 65536 });
            _numFixContour.Name = "_numFixContour";
            _numFixContour.Size = new Size(70, 24);
            _numFixContour.TabIndex = 3;
            _numFixContour.Value = new decimal(new int[] { 6, 0, 0, 65536 });
            // 
            // _flpFixRowDrop
            // 
            _flpFixRowDrop.AutoSize = true;
            _flpFixRowDrop.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _flpFixRowDrop.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _flpFixRowDrop.Controls.Add(_lblFixDrop);
            _flpFixRowDrop.Controls.Add(_numFixDrop);
            _flpFixRowDrop.Controls.Add(_lblFixGoldenScore);
            _flpFixRowDrop.Location = new Point(0, 180);
            _flpFixRowDrop.Margin = new Padding(0, 0, 0, 6);
            _flpFixRowDrop.Name = "_flpFixRowDrop";
            _flpFixRowDrop.Size = new Size(272, 30);
            _flpFixRowDrop.TabIndex = 5;
            _flpFixRowDrop.WrapContents = false;
            // 
            // _lblFixDrop
            // 
            _lblFixDrop.AutoSize = true;
            _lblFixDrop.Font = new Font("Microsoft YaHei UI", 9.5F);
            _lblFixDrop.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _lblFixDrop.Location = new Point(3, 0);
            _lblFixDrop.Name = "_lblFixDrop";
            _lblFixDrop.Size = new Size(100, 19);
            _lblFixDrop.TabIndex = 0;
            _lblFixDrop.Text = "充实度允许下降";
            // 
            // _numFixDrop
            // 
            _numFixDrop.DecimalPlaces = 2;
            _numFixDrop.Font = new Font("Microsoft YaHei UI", 9.5F);
            _numFixDrop.Increment = new decimal(new int[] { 1, 0, 0, 131072 });
            _numFixDrop.Location = new Point(106, 6);
            _numFixDrop.Margin = new Padding(0, 6, 8, 0);
            _numFixDrop.Maximum = new decimal(new int[] { 3, 0, 0, 65536 });
            _numFixDrop.Name = "_numFixDrop";
            _numFixDrop.Size = new Size(70, 24);
            _numFixDrop.TabIndex = 1;
            _numFixDrop.Value = new decimal(new int[] { 1, 0, 0, 65536 });
            // 
            // _lblFixGoldenScore
            // 
            _lblFixGoldenScore.AutoSize = true;
            _lblFixGoldenScore.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lblFixGoldenScore.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblFixGoldenScore.Location = new Point(192, 6);
            _lblFixGoldenScore.Margin = new Padding(8, 6, 0, 0);
            _lblFixGoldenScore.Name = "_lblFixGoldenScore";
            _lblFixGoldenScore.Size = new Size(80, 17);
            _lblFixGoldenScore.TabIndex = 2;
            _lblFixGoldenScore.Text = "金样板未采集";
            // 
            // _lblFixAdvHint
            // 
            _lblFixAdvHint.AutoSize = true;
            _lblFixAdvHint.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lblFixAdvHint.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblFixAdvHint.Location = new Point(0, 216);
            _lblFixAdvHint.Margin = new Padding(0, 0, 0, 8);
            _lblFixAdvHint.MaximumSize = new Size(352, 0);
            _lblFixAdvHint.Name = "_lblFixAdvHint";
            _lblFixAdvHint.Size = new Size(345, 17);
            _lblFixAdvHint.TabIndex = 6;
            _lblFixAdvHint.Text = "高级项一般不用动；改 IoU/面积比前先看 CSV 履历里的分布。";
            // 
            // _fixTabTest
            // 
            _fixTabTest.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _fixTabTest.Controls.Add(_flpFixColTest);
            _fixTabTest.Location = new Point(4, 32);
            _fixTabTest.Name = "_fixTabTest";
            _fixTabTest.Padding = new Padding(6);
            _fixTabTest.Size = new Size(414, 245);
            _fixTabTest.TabIndex = 4;
            _fixTabTest.Text = "测试";
            // 
            // _flpFixColTest
            // 
            _flpFixColTest.AutoScroll = true;
            _flpFixColTest.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _flpFixColTest.Controls.Add(_flpFixRowTest1);
            _flpFixColTest.Controls.Add(_lblFixTestInfo);
            _flpFixColTest.Dock = DockStyle.Fill;
            _flpFixColTest.FlowDirection = FlowDirection.TopDown;
            _flpFixColTest.Location = new Point(6, 6);
            _flpFixColTest.Margin = new Padding(0);
            _flpFixColTest.Name = "_flpFixColTest";
            _flpFixColTest.Padding = new Padding(4);
            _flpFixColTest.Size = new Size(402, 233);
            _flpFixColTest.TabIndex = 0;
            _flpFixColTest.WrapContents = false;
            // 
            // _flpFixRowTest1
            // 
            _flpFixRowTest1.AutoSize = true;
            _flpFixRowTest1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _flpFixRowTest1.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _flpFixRowTest1.Controls.Add(_btnFixTest);
            _flpFixRowTest1.Controls.Add(_btnFixReplay);
            _flpFixRowTest1.Location = new Point(4, 4);
            _flpFixRowTest1.Margin = new Padding(0, 0, 0, 6);
            _flpFixRowTest1.Name = "_flpFixRowTest1";
            _flpFixRowTest1.Size = new Size(216, 36);
            _flpFixRowTest1.TabIndex = 0;
            _flpFixRowTest1.WrapContents = false;
            // 
            // _btnFixTest
            // 
            _btnFixTest.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnFixTest.BackColor = Color.FromArgb((int)(byte)36, (int)(byte)112, (int)(byte)210);
            _btnFixTest.Cursor = Cursors.Hand;
            _btnFixTest.FlatAppearance.BorderSize = 0;
            _btnFixTest.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)28, (int)(byte)96, (int)(byte)186);
            _btnFixTest.FlatStyle = FlatStyle.Flat;
            _btnFixTest.Font = new Font("Microsoft YaHei UI", 9.5F, FontStyle.Bold);
            _btnFixTest.ForeColor = Color.White;
            _btnFixTest.Location = new Point(0, 2);
            _btnFixTest.Margin = new Padding(0, 2, 8, 0);
            _btnFixTest.MinimumSize = new Size(96, 34);
            _btnFixTest.Name = "_btnFixTest";
            _btnFixTest.Padding = new Padding(10, 0, 10, 0);
            _btnFixTest.Size = new Size(96, 34);
            _btnFixTest.TabIndex = 0;
            _btnFixTest.Text = "单张测试";
            _btnFixTest.UseVisualStyleBackColor = false;
            // 
            // _btnFixReplay
            // 
            _btnFixReplay.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnFixReplay.BackColor = Color.White;
            _btnFixReplay.Cursor = Cursors.Hand;
            _btnFixReplay.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnFixReplay.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnFixReplay.FlatStyle = FlatStyle.Flat;
            _btnFixReplay.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnFixReplay.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnFixReplay.Location = new Point(104, 2);
            _btnFixReplay.Margin = new Padding(0, 2, 8, 0);
            _btnFixReplay.MinimumSize = new Size(104, 34);
            _btnFixReplay.Name = "_btnFixReplay";
            _btnFixReplay.Padding = new Padding(10, 0, 10, 0);
            _btnFixReplay.Size = new Size(104, 34);
            _btnFixReplay.TabIndex = 1;
            _btnFixReplay.Text = "回放图片…";
            _btnFixReplay.UseVisualStyleBackColor = false;
            // 
            // _lblFixTestInfo
            // 
            _lblFixTestInfo.AutoSize = true;
            _lblFixTestInfo.Font = new Font("Microsoft YaHei UI", 9.5F);
            _lblFixTestInfo.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _lblFixTestInfo.Location = new Point(4, 50);
            _lblFixTestInfo.Margin = new Padding(0, 4, 0, 8);
            _lblFixTestInfo.MaximumSize = new Size(340, 0);
            _lblFixTestInfo.Name = "_lblFixTestInfo";
            _lblFixTestInfo.Size = new Size(113, 19);
            _lblFixTestInfo.TabIndex = 1;
            _lblFixTestInfo.Text = "还没有检测记录。";
            // 
            // _fixSegTab
            // 
            _fixSegTab.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _fixSegTab.Controls.Add(_flpFixColSeg);
            _fixSegTab.Location = new Point(4, 32);
            _fixSegTab.Name = "_fixSegTab";
            _fixSegTab.Padding = new Padding(6);
            _fixSegTab.Size = new Size(414, 245);
            _fixSegTab.TabIndex = 5;
            _fixSegTab.Text = "分割参数";
            // 
            // _flpFixColSeg
            // 
            _flpFixColSeg.AutoScroll = true;
            _flpFixColSeg.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _flpFixColSeg.Controls.Add(_lblFixSegHint1);
            _flpFixColSeg.Controls.Add(_flpFixRowSegH);
            _flpFixColSeg.Controls.Add(_flpFixRowSegS);
            _flpFixColSeg.Controls.Add(_flpFixRowSegV);
            _flpFixColSeg.Controls.Add(_lblFixSegHint2);
            _flpFixColSeg.Controls.Add(_flpFixRowSegMorph);
            _flpFixColSeg.Controls.Add(_lblFixSegHint3);
            _flpFixColSeg.Controls.Add(_flpFixRowSegDiff);
            _flpFixColSeg.Controls.Add(_lblFixSegHint4);
            _flpFixColSeg.Controls.Add(_flpFixRowSegSave);
            _flpFixColSeg.Controls.Add(_lblFixSegInfo);
            _flpFixColSeg.Controls.Add(_lblFixSegHint5);
            _flpFixColSeg.Dock = DockStyle.Fill;
            _flpFixColSeg.FlowDirection = FlowDirection.TopDown;
            _flpFixColSeg.Location = new Point(6, 6);
            _flpFixColSeg.Margin = new Padding(0);
            _flpFixColSeg.Name = "_flpFixColSeg";
            _flpFixColSeg.Padding = new Padding(4);
            _flpFixColSeg.Size = new Size(402, 233);
            _flpFixColSeg.TabIndex = 0;
            _flpFixColSeg.WrapContents = false;
            // 
            // _lblFixSegHint1
            // 
            _lblFixSegHint1.AutoSize = true;
            _lblFixSegHint1.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lblFixSegHint1.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblFixSegHint1.Location = new Point(4, 4);
            _lblFixSegHint1.Margin = new Padding(0, 0, 0, 8);
            _lblFixSegHint1.MaximumSize = new Size(352, 0);
            _lblFixSegHint1.Name = "_lblFixSegHint1";
            _lblFixSegHint1.Size = new Size(344, 51);
            _lblFixSegHint1.TabIndex = 0;
            _lblFixSegHint1.Text = "直接看实时画面调：青色是被选中的产品像素，红边是它的外沿，橙红块是越出安全区的像素。边缘毛刺会算成越界，先把这里调贴合，再回头定安全区/允许越界。";
            // 
            // _flpFixRowSegH
            // 
            _flpFixRowSegH.AutoSize = true;
            _flpFixRowSegH.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _flpFixRowSegH.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _flpFixRowSegH.Controls.Add(_lblFixSegH);
            _flpFixRowSegH.Controls.Add(_numFixSegHMin);
            _flpFixRowSegH.Controls.Add(_lblFixSegHTilde);
            _flpFixRowSegH.Controls.Add(_numFixSegHMax);
            _flpFixRowSegH.Location = new Point(4, 63);
            _flpFixRowSegH.Margin = new Padding(0, 0, 0, 6);
            _flpFixRowSegH.Name = "_flpFixRowSegH";
            _flpFixRowSegH.Size = new Size(224, 30);
            _flpFixRowSegH.TabIndex = 1;
            _flpFixRowSegH.WrapContents = false;
            // 
            // _lblFixSegH
            // 
            _lblFixSegH.AutoSize = true;
            _lblFixSegH.Font = new Font("Microsoft YaHei UI", 9.5F);
            _lblFixSegH.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _lblFixSegH.Location = new Point(3, 0);
            _lblFixSegH.Name = "_lblFixSegH";
            _lblFixSegH.Size = new Size(49, 19);
            _lblFixSegH.TabIndex = 0;
            _lblFixSegH.Text = "色相 H";
            // 
            // _numFixSegHMin
            // 
            _numFixSegHMin.Font = new Font("Microsoft YaHei UI", 9.5F);
            _numFixSegHMin.Location = new Point(55, 6);
            _numFixSegHMin.Margin = new Padding(0, 6, 8, 0);
            _numFixSegHMin.Maximum = new decimal(new int[] { 180, 0, 0, 0 });
            _numFixSegHMin.Name = "_numFixSegHMin";
            _numFixSegHMin.Size = new Size(64, 24);
            _numFixSegHMin.TabIndex = 1;
            // 
            // _lblFixSegHTilde
            // 
            _lblFixSegHTilde.AutoSize = true;
            _lblFixSegHTilde.Font = new Font("Microsoft YaHei UI", 9.5F);
            _lblFixSegHTilde.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _lblFixSegHTilde.Location = new Point(130, 0);
            _lblFixSegHTilde.Name = "_lblFixSegHTilde";
            _lblFixSegHTilde.Size = new Size(19, 19);
            _lblFixSegHTilde.TabIndex = 2;
            _lblFixSegHTilde.Text = "~";
            // 
            // _numFixSegHMax
            // 
            _numFixSegHMax.Font = new Font("Microsoft YaHei UI", 9.5F);
            _numFixSegHMax.Location = new Point(152, 6);
            _numFixSegHMax.Margin = new Padding(0, 6, 8, 0);
            _numFixSegHMax.Maximum = new decimal(new int[] { 180, 0, 0, 0 });
            _numFixSegHMax.Name = "_numFixSegHMax";
            _numFixSegHMax.Size = new Size(64, 24);
            _numFixSegHMax.TabIndex = 3;
            _numFixSegHMax.Value = new decimal(new int[] { 180, 0, 0, 0 });
            // 
            // _flpFixRowSegS
            // 
            _flpFixRowSegS.AutoSize = true;
            _flpFixRowSegS.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _flpFixRowSegS.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _flpFixRowSegS.Controls.Add(_lblFixSegS);
            _flpFixRowSegS.Controls.Add(_numFixSegSMin);
            _flpFixRowSegS.Controls.Add(_lblFixSegSTilde);
            _flpFixRowSegS.Controls.Add(_numFixSegSMax);
            _flpFixRowSegS.Location = new Point(4, 99);
            _flpFixRowSegS.Margin = new Padding(0, 0, 0, 6);
            _flpFixRowSegS.Name = "_flpFixRowSegS";
            _flpFixRowSegS.Size = new Size(222, 30);
            _flpFixRowSegS.TabIndex = 2;
            _flpFixRowSegS.WrapContents = false;
            // 
            // _lblFixSegS
            // 
            _lblFixSegS.AutoSize = true;
            _lblFixSegS.Font = new Font("Microsoft YaHei UI", 9.5F);
            _lblFixSegS.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _lblFixSegS.Location = new Point(3, 0);
            _lblFixSegS.Name = "_lblFixSegS";
            _lblFixSegS.Size = new Size(47, 19);
            _lblFixSegS.TabIndex = 0;
            _lblFixSegS.Text = "饱和 S";
            // 
            // _numFixSegSMin
            // 
            _numFixSegSMin.Font = new Font("Microsoft YaHei UI", 9.5F);
            _numFixSegSMin.Location = new Point(53, 6);
            _numFixSegSMin.Margin = new Padding(0, 6, 8, 0);
            _numFixSegSMin.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            _numFixSegSMin.Name = "_numFixSegSMin";
            _numFixSegSMin.Size = new Size(64, 24);
            _numFixSegSMin.TabIndex = 1;
            // 
            // _lblFixSegSTilde
            // 
            _lblFixSegSTilde.AutoSize = true;
            _lblFixSegSTilde.Font = new Font("Microsoft YaHei UI", 9.5F);
            _lblFixSegSTilde.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _lblFixSegSTilde.Location = new Point(128, 0);
            _lblFixSegSTilde.Name = "_lblFixSegSTilde";
            _lblFixSegSTilde.Size = new Size(19, 19);
            _lblFixSegSTilde.TabIndex = 2;
            _lblFixSegSTilde.Text = "~";
            // 
            // _numFixSegSMax
            // 
            _numFixSegSMax.Font = new Font("Microsoft YaHei UI", 9.5F);
            _numFixSegSMax.Location = new Point(150, 6);
            _numFixSegSMax.Margin = new Padding(0, 6, 8, 0);
            _numFixSegSMax.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            _numFixSegSMax.Name = "_numFixSegSMax";
            _numFixSegSMax.Size = new Size(64, 24);
            _numFixSegSMax.TabIndex = 3;
            _numFixSegSMax.Value = new decimal(new int[] { 255, 0, 0, 0 });
            // 
            // _flpFixRowSegV
            // 
            _flpFixRowSegV.AutoSize = true;
            _flpFixRowSegV.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _flpFixRowSegV.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _flpFixRowSegV.Controls.Add(_lblFixSegV);
            _flpFixRowSegV.Controls.Add(_numFixSegVMin);
            _flpFixRowSegV.Controls.Add(_lblFixSegVTilde);
            _flpFixRowSegV.Controls.Add(_numFixSegVMax);
            _flpFixRowSegV.Location = new Point(4, 135);
            _flpFixRowSegV.Margin = new Padding(0, 0, 0, 6);
            _flpFixRowSegV.Name = "_flpFixRowSegV";
            _flpFixRowSegV.Size = new Size(223, 30);
            _flpFixRowSegV.TabIndex = 3;
            _flpFixRowSegV.WrapContents = false;
            // 
            // _lblFixSegV
            // 
            _lblFixSegV.AutoSize = true;
            _lblFixSegV.Font = new Font("Microsoft YaHei UI", 9.5F);
            _lblFixSegV.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _lblFixSegV.Location = new Point(3, 0);
            _lblFixSegV.Name = "_lblFixSegV";
            _lblFixSegV.Size = new Size(48, 19);
            _lblFixSegV.TabIndex = 0;
            _lblFixSegV.Text = "亮度 V";
            // 
            // _numFixSegVMin
            // 
            _numFixSegVMin.Font = new Font("Microsoft YaHei UI", 9.5F);
            _numFixSegVMin.Location = new Point(54, 6);
            _numFixSegVMin.Margin = new Padding(0, 6, 8, 0);
            _numFixSegVMin.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            _numFixSegVMin.Name = "_numFixSegVMin";
            _numFixSegVMin.Size = new Size(64, 24);
            _numFixSegVMin.TabIndex = 1;
            // 
            // _lblFixSegVTilde
            // 
            _lblFixSegVTilde.AutoSize = true;
            _lblFixSegVTilde.Font = new Font("Microsoft YaHei UI", 9.5F);
            _lblFixSegVTilde.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _lblFixSegVTilde.Location = new Point(129, 0);
            _lblFixSegVTilde.Name = "_lblFixSegVTilde";
            _lblFixSegVTilde.Size = new Size(19, 19);
            _lblFixSegVTilde.TabIndex = 2;
            _lblFixSegVTilde.Text = "~";
            // 
            // _numFixSegVMax
            // 
            _numFixSegVMax.Font = new Font("Microsoft YaHei UI", 9.5F);
            _numFixSegVMax.Location = new Point(151, 6);
            _numFixSegVMax.Margin = new Padding(0, 6, 8, 0);
            _numFixSegVMax.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            _numFixSegVMax.Name = "_numFixSegVMax";
            _numFixSegVMax.Size = new Size(64, 24);
            _numFixSegVMax.TabIndex = 3;
            _numFixSegVMax.Value = new decimal(new int[] { 255, 0, 0, 0 });
            // 
            // _lblFixSegHint2
            // 
            _lblFixSegHint2.AutoSize = true;
            _lblFixSegHint2.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lblFixSegHint2.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblFixSegHint2.Location = new Point(4, 171);
            _lblFixSegHint2.Margin = new Padding(0, 0, 0, 8);
            _lblFixSegHint2.MaximumSize = new Size(352, 0);
            _lblFixSegHint2.Name = "_lblFixSegHint2";
            _lblFixSegHint2.Size = new Size(344, 34);
            _lblFixSegHint2.TabIndex = 4;
            _lblFixSegHint2.Text = "H 取 0–180：先用 H 框住产品颜色，再用 S/V 去掉台面、阴影和反光。";
            // 
            // _flpFixRowSegMorph
            // 
            _flpFixRowSegMorph.AutoSize = true;
            _flpFixRowSegMorph.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _flpFixRowSegMorph.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _flpFixRowSegMorph.Controls.Add(_lblFixSegMorph);
            _flpFixRowSegMorph.Controls.Add(_numFixSegMorph);
            _flpFixRowSegMorph.Controls.Add(_lblFixSegMorphUnit);
            _flpFixRowSegMorph.Location = new Point(4, 213);
            _flpFixRowSegMorph.Margin = new Padding(0, 0, 0, 6);
            _flpFixRowSegMorph.Name = "_flpFixRowSegMorph";
            _flpFixRowSegMorph.Size = new Size(208, 30);
            _flpFixRowSegMorph.TabIndex = 5;
            _flpFixRowSegMorph.WrapContents = false;
            // 
            // _lblFixSegMorph
            // 
            _lblFixSegMorph.AutoSize = true;
            _lblFixSegMorph.Font = new Font("Microsoft YaHei UI", 9.5F);
            _lblFixSegMorph.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _lblFixSegMorph.Location = new Point(3, 0);
            _lblFixSegMorph.Name = "_lblFixSegMorph";
            _lblFixSegMorph.Size = new Size(48, 19);
            _lblFixSegMorph.TabIndex = 0;
            _lblFixSegMorph.Text = "形态核";
            // 
            // _numFixSegMorph
            // 
            _numFixSegMorph.Font = new Font("Microsoft YaHei UI", 9.5F);
            _numFixSegMorph.Increment = new decimal(new int[] { 2, 0, 0, 0 });
            _numFixSegMorph.Location = new Point(54, 6);
            _numFixSegMorph.Margin = new Padding(0, 6, 8, 0);
            _numFixSegMorph.Maximum = new decimal(new int[] { 31, 0, 0, 0 });
            _numFixSegMorph.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            _numFixSegMorph.Name = "_numFixSegMorph";
            _numFixSegMorph.Size = new Size(64, 24);
            _numFixSegMorph.TabIndex = 1;
            _numFixSegMorph.Value = new decimal(new int[] { 5, 0, 0, 0 });
            // 
            // _lblFixSegMorphUnit
            // 
            _lblFixSegMorphUnit.AutoSize = true;
            _lblFixSegMorphUnit.Font = new Font("Microsoft YaHei UI", 9.5F);
            _lblFixSegMorphUnit.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _lblFixSegMorphUnit.Location = new Point(129, 0);
            _lblFixSegMorphUnit.Name = "_lblFixSegMorphUnit";
            _lblFixSegMorphUnit.Size = new Size(76, 19);
            _lblFixSegMorphUnit.TabIndex = 2;
            _lblFixSegMorphUnit.Text = "px（奇数）";
            // 
            // _lblFixSegHint3
            // 
            _lblFixSegHint3.AutoSize = true;
            _lblFixSegHint3.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lblFixSegHint3.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblFixSegHint3.Location = new Point(4, 249);
            _lblFixSegHint3.Margin = new Padding(0, 0, 0, 8);
            _lblFixSegHint3.MaximumSize = new Size(352, 0);
            _lblFixSegHint3.Name = "_lblFixSegHint3";
            _lblFixSegHint3.Size = new Size(325, 17);
            _lblFixSegHint3.TabIndex = 6;
            _lblFixSegHint3.Text = "开/闭运算核越大越能吃毛刺，但会把细边磨圆；先试 3–7。";
            // 
            // _flpFixRowSegDiff
            // 
            _flpFixRowSegDiff.AutoSize = true;
            _flpFixRowSegDiff.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _flpFixRowSegDiff.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _flpFixRowSegDiff.Controls.Add(_chkFixSegUseEmpty);
            _flpFixRowSegDiff.Controls.Add(_lblFixSegDiff);
            _flpFixRowSegDiff.Controls.Add(_numFixSegDiff);
            _flpFixRowSegDiff.Location = new Point(4, 274);
            _flpFixRowSegDiff.Margin = new Padding(0, 0, 0, 6);
            _flpFixRowSegDiff.Name = "_flpFixRowSegDiff";
            _flpFixRowSegDiff.Size = new Size(255, 32);
            _flpFixRowSegDiff.TabIndex = 7;
            _flpFixRowSegDiff.WrapContents = false;
            // 
            // _chkFixSegUseEmpty
            // 
            _chkFixSegUseEmpty.AutoSize = true;
            _chkFixSegUseEmpty.Font = new Font("Microsoft YaHei UI", 9.5F);
            _chkFixSegUseEmpty.Location = new Point(0, 9);
            _chkFixSegUseEmpty.Margin = new Padding(0, 9, 10, 0);
            _chkFixSegUseEmpty.Name = "_chkFixSegUseEmpty";
            _chkFixSegUseEmpty.Size = new Size(106, 23);
            _chkFixSegUseEmpty.TabIndex = 0;
            _chkFixSegUseEmpty.Text = "用空治具差分";
            // 
            // _lblFixSegDiff
            // 
            _lblFixSegDiff.AutoSize = true;
            _lblFixSegDiff.Font = new Font("Microsoft YaHei UI", 9.5F);
            _lblFixSegDiff.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _lblFixSegDiff.Location = new Point(119, 0);
            _lblFixSegDiff.Name = "_lblFixSegDiff";
            _lblFixSegDiff.Size = new Size(61, 19);
            _lblFixSegDiff.TabIndex = 1;
            _lblFixSegDiff.Text = "差分阈值";
            // 
            // _numFixSegDiff
            // 
            _numFixSegDiff.Font = new Font("Microsoft YaHei UI", 9.5F);
            _numFixSegDiff.Location = new Point(183, 6);
            _numFixSegDiff.Margin = new Padding(0, 6, 8, 0);
            _numFixSegDiff.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            _numFixSegDiff.Name = "_numFixSegDiff";
            _numFixSegDiff.Size = new Size(64, 24);
            _numFixSegDiff.TabIndex = 2;
            _numFixSegDiff.Value = new decimal(new int[] { 18, 0, 0, 0 });
            // 
            // _lblFixSegHint4
            // 
            _lblFixSegHint4.AutoSize = true;
            _lblFixSegHint4.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lblFixSegHint4.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblFixSegHint4.Location = new Point(4, 312);
            _lblFixSegHint4.Margin = new Padding(0, 0, 0, 8);
            _lblFixSegHint4.MaximumSize = new Size(352, 0);
            _lblFixSegHint4.Name = "_lblFixSegHint4";
            _lblFixSegHint4.Size = new Size(332, 34);
            _lblFixSegHint4.TabIndex = 8;
            _lblFixSegHint4.Text = "只有采过空治具才生效：与空图灰度差超过阈值的像素才算产品。空图和现场亮度不一致时，关掉差分反而更稳。";
            // 
            // _flpFixRowSegSave
            // 
            _flpFixRowSegSave.AutoSize = true;
            _flpFixRowSegSave.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _flpFixRowSegSave.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _flpFixRowSegSave.Controls.Add(_btnFixSegEstimate);
            _flpFixRowSegSave.Controls.Add(_btnFixSegApply);
            _flpFixRowSegSave.Location = new Point(4, 354);
            _flpFixRowSegSave.Margin = new Padding(0, 0, 0, 6);
            _flpFixRowSegSave.Name = "_flpFixRowSegSave";
            _flpFixRowSegSave.Size = new Size(216, 38);
            _flpFixRowSegSave.TabIndex = 9;
            _flpFixRowSegSave.WrapContents = false;
            // 
            // _btnFixSegEstimate
            // 
            _btnFixSegEstimate.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnFixSegEstimate.BackColor = Color.White;
            _btnFixSegEstimate.Cursor = Cursors.Hand;
            _btnFixSegEstimate.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnFixSegEstimate.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnFixSegEstimate.FlatStyle = FlatStyle.Flat;
            _btnFixSegEstimate.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnFixSegEstimate.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnFixSegEstimate.Location = new Point(0, 4);
            _btnFixSegEstimate.Margin = new Padding(0, 4, 8, 0);
            _btnFixSegEstimate.MinimumSize = new Size(96, 34);
            _btnFixSegEstimate.Name = "_btnFixSegEstimate";
            _btnFixSegEstimate.Padding = new Padding(10, 0, 10, 0);
            _btnFixSegEstimate.Size = new Size(96, 34);
            _btnFixSegEstimate.TabIndex = 0;
            _btnFixSegEstimate.Text = "重新估算";
            _btnFixSegEstimate.UseVisualStyleBackColor = false;
            // 
            // _btnFixSegApply
            // 
            _btnFixSegApply.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnFixSegApply.BackColor = Color.FromArgb((int)(byte)36, (int)(byte)112, (int)(byte)210);
            _btnFixSegApply.Cursor = Cursors.Hand;
            _btnFixSegApply.FlatAppearance.BorderSize = 0;
            _btnFixSegApply.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)28, (int)(byte)96, (int)(byte)186);
            _btnFixSegApply.FlatStyle = FlatStyle.Flat;
            _btnFixSegApply.Font = new Font("Microsoft YaHei UI", 9.5F, FontStyle.Bold);
            _btnFixSegApply.ForeColor = Color.White;
            _btnFixSegApply.Location = new Point(104, 4);
            _btnFixSegApply.Margin = new Padding(0, 4, 8, 0);
            _btnFixSegApply.MinimumSize = new Size(104, 34);
            _btnFixSegApply.Name = "_btnFixSegApply";
            _btnFixSegApply.Padding = new Padding(10, 0, 10, 0);
            _btnFixSegApply.Size = new Size(104, 34);
            _btnFixSegApply.TabIndex = 1;
            _btnFixSegApply.Text = "应用并保存";
            _btnFixSegApply.UseVisualStyleBackColor = false;
            // 
            // _lblFixSegInfo
            // 
            _lblFixSegInfo.AutoSize = true;
            _lblFixSegInfo.Font = new Font("Microsoft YaHei UI", 9.5F);
            _lblFixSegInfo.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _lblFixSegInfo.Location = new Point(4, 402);
            _lblFixSegInfo.Margin = new Padding(0, 4, 0, 8);
            _lblFixSegInfo.MaximumSize = new Size(352, 0);
            _lblFixSegInfo.Name = "_lblFixSegInfo";
            _lblFixSegInfo.Size = new Size(113, 19);
            _lblFixSegInfo.TabIndex = 10;
            _lblFixSegInfo.Text = "还没有分割参数。";
            // 
            // _lblFixSegHint5
            // 
            _lblFixSegHint5.AutoSize = true;
            _lblFixSegHint5.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lblFixSegHint5.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblFixSegHint5.Location = new Point(4, 429);
            _lblFixSegHint5.Margin = new Padding(0, 0, 0, 8);
            _lblFixSegHint5.MaximumSize = new Size(352, 0);
            _lblFixSegHint5.Name = "_lblFixSegHint5";
            _lblFixSegHint5.Size = new Size(344, 51);
            _lblFixSegHint5.TabIndex = 11;
            _lblFixSegHint5.Text = "“应用并保存”会按当前参数重画 golden_mask.png 并写进配方；安全区不会自动跟着改，改完回【模板】页确认，必要时重新生成安全区。";
            // 
            // _lvFixLog
            // 
            _lvFixLog.BackColor = Color.White;
            _lvFixLog.BorderStyle = BorderStyle.FixedSingle;
            _lvFixLog.Columns.AddRange(new ColumnHeader[] { colFixLogTime, colFixLogResult, colFixLogReason, colFixLogOffset, colFixLogCost });
            _lvFixLog.Dock = DockStyle.Fill;
            _lvFixLog.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lvFixLog.FullRowSelect = true;
            _lvFixLog.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            _lvFixLog.Location = new Point(0, 478);
            _lvFixLog.Margin = new Padding(0, 6, 0, 0);
            _lvFixLog.Name = "_lvFixLog";
            _lvFixLog.Size = new Size(434, 110);
            _lvFixLog.TabIndex = 3;
            _lvFixLog.UseCompatibleStateImageBehavior = false;
            _lvFixLog.View = View.Details;
            // 
            // colFixLogTime
            // 
            colFixLogTime.Name = "colFixLogTime";
            colFixLogTime.Text = "时间";
            colFixLogTime.Width = 62;
            // 
            // colFixLogResult
            // 
            colFixLogResult.Name = "colFixLogResult";
            colFixLogResult.Text = "结论";
            colFixLogResult.Width = 46;
            // 
            // colFixLogReason
            // 
            colFixLogReason.Name = "colFixLogReason";
            colFixLogReason.Text = "原因";
            colFixLogReason.Width = 138;
            // 
            // colFixLogOffset
            // 
            colFixLogOffset.Name = "colFixLogOffset";
            colFixLogOffset.Text = "偏移";
            colFixLogOffset.Width = 98;
            // 
            // colFixLogCost
            // 
            colFixLogCost.Name = "colFixLogCost";
            colFixLogCost.Text = "耗时";
            colFixLogCost.Width = 56;
            // 
            // _pageAnchor
            // 
            _pageAnchor.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _pageAnchor.Controls.Add(_tlpAnchor);
            _pageAnchor.Location = new Point(4, 34);
            _pageAnchor.Name = "_pageAnchor";
            _pageAnchor.Padding = new Padding(8);
            _pageAnchor.Size = new Size(1256, 669);
            _pageAnchor.TabIndex = 4;
            _pageAnchor.Text = "针床定位 V2";
            // 
            // _tlpAnchor
            // 
            _tlpAnchor.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _tlpAnchor.ColumnCount = 1;
            _tlpAnchor.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            _tlpAnchor.Controls.Add(_tlpAnchorTop, 0, 0);
            _tlpAnchor.Controls.Add(_tlpAnchorBody, 0, 1);
            _tlpAnchor.Dock = DockStyle.Fill;
            _tlpAnchor.Location = new Point(8, 8);
            _tlpAnchor.Margin = new Padding(0);
            _tlpAnchor.Name = "_tlpAnchor";
            _tlpAnchor.RowCount = 2;
            _tlpAnchor.RowStyles.Add(new RowStyle());
            _tlpAnchor.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            _tlpAnchor.Size = new Size(1240, 653);
            _tlpAnchor.TabIndex = 0;
            // 
            // _tlpAnchorTop
            // 
            _tlpAnchorTop.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _tlpAnchorTop.ColumnCount = 2;
            _tlpAnchorTop.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            _tlpAnchorTop.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 412F));
            _tlpAnchorTop.Controls.Add(_tlpAnchorBar, 0, 0);
            _tlpAnchorTop.Controls.Add(_pnlAnchorVerdict, 1, 0);
            _tlpAnchorTop.Dock = DockStyle.Fill;
            _tlpAnchorTop.Location = new Point(0, 0);
            _tlpAnchorTop.Margin = new Padding(0);
            _tlpAnchorTop.Name = "_tlpAnchorTop";
            _tlpAnchorTop.RowCount = 1;
            _tlpAnchorTop.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            _tlpAnchorTop.Size = new Size(1240, 100);
            _tlpAnchorTop.TabIndex = 0;
            // 
            // _tlpAnchorBar
            // 
            _tlpAnchorBar.AutoSize = true;
            _tlpAnchorBar.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _tlpAnchorBar.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _tlpAnchorBar.ColumnCount = 1;
            _tlpAnchorBar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            _tlpAnchorBar.Controls.Add(_flowAnchorBarModel, 0, 0);
            _tlpAnchorBar.Controls.Add(_flowAnchorBarCam, 0, 1);
            _tlpAnchorBar.Dock = DockStyle.Top;
            _tlpAnchorBar.Location = new Point(0, 0);
            _tlpAnchorBar.Margin = new Padding(0, 0, 0, 6);
            _tlpAnchorBar.Name = "_tlpAnchorBar";
            _tlpAnchorBar.RowCount = 2;
            _tlpAnchorBar.RowStyles.Add(new RowStyle());
            _tlpAnchorBar.RowStyles.Add(new RowStyle());
            _tlpAnchorBar.Size = new Size(828, 78);
            _tlpAnchorBar.TabIndex = 0;
            // 
            // _flowAnchorBarModel
            // 
            _flowAnchorBarModel.AutoSize = true;
            _flowAnchorBarModel.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _flowAnchorBarModel.Controls.Add(_lblAnchorBarModelCap);
            _flowAnchorBarModel.Controls.Add(_cboAnchorModel);
            _flowAnchorBarModel.Controls.Add(_btnAnchorNewModel);
            _flowAnchorBarModel.Controls.Add(_btnAnchorCopyModel);
            _flowAnchorBarModel.Controls.Add(_lblAnchorModelInfo);
            _flowAnchorBarModel.Dock = DockStyle.Fill;
            _flowAnchorBarModel.Location = new Point(0, 0);
            _flowAnchorBarModel.Margin = new Padding(0);
            _flowAnchorBarModel.Name = "_flowAnchorBarModel";
            _flowAnchorBarModel.Size = new Size(828, 39);
            _flowAnchorBarModel.TabIndex = 0;
            _flowAnchorBarModel.WrapContents = false;
            // 
            // _lblAnchorBarModelCap
            // 
            _lblAnchorBarModelCap.AutoSize = true;
            _lblAnchorBarModelCap.Font = new Font("Microsoft YaHei UI", 9.5F);
            _lblAnchorBarModelCap.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblAnchorBarModelCap.Location = new Point(0, 9);
            _lblAnchorBarModelCap.Margin = new Padding(0, 9, 6, 0);
            _lblAnchorBarModelCap.Name = "_lblAnchorBarModelCap";
            _lblAnchorBarModelCap.Size = new Size(35, 19);
            _lblAnchorBarModelCap.TabIndex = 0;
            _lblAnchorBarModelCap.Text = "机种";
            // 
            // _cboAnchorModel
            // 
            _cboAnchorModel.DropDownStyle = ComboBoxStyle.DropDownList;
            _cboAnchorModel.Font = new Font("Microsoft YaHei UI", 9.5F);
            _cboAnchorModel.Location = new Point(41, 5);
            _cboAnchorModel.Margin = new Padding(0, 5, 6, 0);
            _cboAnchorModel.Name = "_cboAnchorModel";
            _cboAnchorModel.Size = new Size(150, 27);
            _cboAnchorModel.TabIndex = 1;
            // 
            // _btnAnchorNewModel
            // 
            _btnAnchorNewModel.AutoSize = true;
            _btnAnchorNewModel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnAnchorNewModel.BackColor = Color.White;
            _btnAnchorNewModel.Cursor = Cursors.Hand;
            _btnAnchorNewModel.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnAnchorNewModel.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnAnchorNewModel.FlatStyle = FlatStyle.Flat;
            _btnAnchorNewModel.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnAnchorNewModel.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnAnchorNewModel.Location = new Point(197, 5);
            _btnAnchorNewModel.Margin = new Padding(0, 5, 6, 0);
            _btnAnchorNewModel.MinimumSize = new Size(84, 34);
            _btnAnchorNewModel.Name = "_btnAnchorNewModel";
            _btnAnchorNewModel.Padding = new Padding(10, 0, 10, 0);
            _btnAnchorNewModel.Size = new Size(93, 34);
            _btnAnchorNewModel.TabIndex = 2;
            _btnAnchorNewModel.Text = "新建机种";
            _btnAnchorNewModel.UseVisualStyleBackColor = false;
            // 
            // _btnAnchorCopyModel
            // 
            _btnAnchorCopyModel.AutoSize = true;
            _btnAnchorCopyModel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnAnchorCopyModel.BackColor = Color.White;
            _btnAnchorCopyModel.Cursor = Cursors.Hand;
            _btnAnchorCopyModel.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnAnchorCopyModel.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnAnchorCopyModel.FlatStyle = FlatStyle.Flat;
            _btnAnchorCopyModel.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnAnchorCopyModel.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnAnchorCopyModel.Location = new Point(296, 5);
            _btnAnchorCopyModel.Margin = new Padding(0, 5, 10, 0);
            _btnAnchorCopyModel.MinimumSize = new Size(84, 34);
            _btnAnchorCopyModel.Name = "_btnAnchorCopyModel";
            _btnAnchorCopyModel.Padding = new Padding(10, 0, 10, 0);
            _btnAnchorCopyModel.Size = new Size(119, 34);
            _btnAnchorCopyModel.TabIndex = 3;
            _btnAnchorCopyModel.Text = "复制当前模型";
            _btnAnchorCopyModel.UseVisualStyleBackColor = false;
            // 
            // _lblAnchorModelInfo
            // 
            _lblAnchorModelInfo.AutoSize = true;
            _lblAnchorModelInfo.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lblAnchorModelInfo.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblAnchorModelInfo.Location = new Point(425, 10);
            _lblAnchorModelInfo.Margin = new Padding(0, 10, 0, 0);
            _lblAnchorModelInfo.Name = "_lblAnchorModelInfo";
            _lblAnchorModelInfo.Size = new Size(80, 17);
            _lblAnchorModelInfo.TabIndex = 4;
            _lblAnchorModelInfo.Text = "模型：未加载";
            // 
            // _flowAnchorBarCam
            // 
            _flowAnchorBarCam.AutoSize = true;
            _flowAnchorBarCam.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _flowAnchorBarCam.Controls.Add(_lblAnchorBarCamCap);
            _flowAnchorBarCam.Controls.Add(_cboAnchorCamIndex);
            _flowAnchorBarCam.Controls.Add(_cboAnchorRes);
            _flowAnchorBarCam.Controls.Add(_chkAnchorExposure);
            _flowAnchorBarCam.Controls.Add(_numAnchorExposure);
            _flowAnchorBarCam.Controls.Add(_btnAnchorShot);
            _flowAnchorBarCam.Controls.Add(_btnAnchorCam);
            _flowAnchorBarCam.Controls.Add(_btnAnchorJudge);
            _flowAnchorBarCam.Controls.Add(_lblAnchorCamInfo);
            _flowAnchorBarCam.Dock = DockStyle.Fill;
            _flowAnchorBarCam.Location = new Point(0, 39);
            _flowAnchorBarCam.Margin = new Padding(0);
            _flowAnchorBarCam.Name = "_flowAnchorBarCam";
            _flowAnchorBarCam.Size = new Size(828, 39);
            _flowAnchorBarCam.TabIndex = 1;
            _flowAnchorBarCam.WrapContents = false;
            // 
            // _lblAnchorBarCamCap
            // 
            _lblAnchorBarCamCap.AutoSize = true;
            _lblAnchorBarCamCap.Font = new Font("Microsoft YaHei UI", 9.5F);
            _lblAnchorBarCamCap.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblAnchorBarCamCap.Location = new Point(0, 9);
            _lblAnchorBarCamCap.Margin = new Padding(0, 9, 6, 0);
            _lblAnchorBarCamCap.Name = "_lblAnchorBarCamCap";
            _lblAnchorBarCamCap.Size = new Size(35, 19);
            _lblAnchorBarCamCap.TabIndex = 0;
            _lblAnchorBarCamCap.Text = "相机";
            // 
            // _cboAnchorCamIndex
            // 
            _cboAnchorCamIndex.DropDownStyle = ComboBoxStyle.DropDownList;
            _cboAnchorCamIndex.Font = new Font("Microsoft YaHei UI", 9.5F);
            _cboAnchorCamIndex.Location = new Point(41, 5);
            _cboAnchorCamIndex.Margin = new Padding(0, 5, 6, 0);
            _cboAnchorCamIndex.Name = "_cboAnchorCamIndex";
            _cboAnchorCamIndex.Size = new Size(52, 27);
            _cboAnchorCamIndex.TabIndex = 1;
            // 
            // _cboAnchorRes
            // 
            _cboAnchorRes.DropDownStyle = ComboBoxStyle.DropDownList;
            _cboAnchorRes.Font = new Font("Microsoft YaHei UI", 9.5F);
            _cboAnchorRes.Location = new Point(99, 5);
            _cboAnchorRes.Margin = new Padding(0, 5, 10, 0);
            _cboAnchorRes.Name = "_cboAnchorRes";
            _cboAnchorRes.Size = new Size(108, 27);
            _cboAnchorRes.TabIndex = 2;
            // 
            // _chkAnchorExposure
            // 
            _chkAnchorExposure.AutoSize = true;
            _chkAnchorExposure.Font = new Font("Microsoft YaHei UI", 9.5F);
            _chkAnchorExposure.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _chkAnchorExposure.Location = new Point(217, 8);
            _chkAnchorExposure.Margin = new Padding(0, 8, 4, 0);
            _chkAnchorExposure.Name = "_chkAnchorExposure";
            _chkAnchorExposure.Size = new Size(54, 23);
            _chkAnchorExposure.TabIndex = 3;
            _chkAnchorExposure.Text = "曝光";
            // 
            // _numAnchorExposure
            // 
            _numAnchorExposure.Enabled = false;
            _numAnchorExposure.Font = new Font("Microsoft YaHei UI", 9.5F);
            _numAnchorExposure.Location = new Point(275, 5);
            _numAnchorExposure.Margin = new Padding(0, 5, 10, 0);
            _numAnchorExposure.Maximum = new decimal(new int[] { 0, 0, 0, 0 });
            _numAnchorExposure.Minimum = new decimal(new int[] { 13, 0, 0, global::System.Int32.MinValue });
            _numAnchorExposure.Name = "_numAnchorExposure";
            _numAnchorExposure.Size = new Size(58, 24);
            _numAnchorExposure.TabIndex = 4;
            _numAnchorExposure.TextAlign = HorizontalAlignment.Right;
            _numAnchorExposure.Value = new decimal(new int[] { 5, 0, 0, global::System.Int32.MinValue });
            // 
            // _btnAnchorShot
            // 
            _btnAnchorShot.AutoSize = true;
            _btnAnchorShot.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnAnchorShot.BackColor = Color.FromArgb((int)(byte)22, (int)(byte)138, (int)(byte)88);
            _btnAnchorShot.Cursor = Cursors.Hand;
            _btnAnchorShot.FlatAppearance.BorderSize = 0;
            _btnAnchorShot.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)18, (int)(byte)118, (int)(byte)74);
            _btnAnchorShot.FlatStyle = FlatStyle.Flat;
            _btnAnchorShot.Font = new Font("Microsoft YaHei UI", 9.5F, FontStyle.Bold);
            _btnAnchorShot.ForeColor = Color.White;
            _btnAnchorShot.Location = new Point(343, 5);
            _btnAnchorShot.Margin = new Padding(0, 5, 8, 0);
            _btnAnchorShot.MinimumSize = new Size(84, 34);
            _btnAnchorShot.Name = "_btnAnchorShot";
            _btnAnchorShot.Padding = new Padding(10, 0, 10, 0);
            _btnAnchorShot.Size = new Size(84, 34);
            _btnAnchorShot.TabIndex = 5;
            _btnAnchorShot.Text = "拍照";
            _btnAnchorShot.UseVisualStyleBackColor = false;
            // 
            // _btnAnchorCam
            // 
            _btnAnchorCam.AutoSize = true;
            _btnAnchorCam.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnAnchorCam.BackColor = Color.FromArgb((int)(byte)36, (int)(byte)112, (int)(byte)210);
            _btnAnchorCam.Cursor = Cursors.Hand;
            _btnAnchorCam.FlatAppearance.BorderSize = 0;
            _btnAnchorCam.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)28, (int)(byte)96, (int)(byte)186);
            _btnAnchorCam.FlatStyle = FlatStyle.Flat;
            _btnAnchorCam.Font = new Font("Microsoft YaHei UI", 9.5F, FontStyle.Bold);
            _btnAnchorCam.ForeColor = Color.White;
            _btnAnchorCam.Location = new Point(435, 5);
            _btnAnchorCam.Margin = new Padding(0, 5, 8, 0);
            _btnAnchorCam.MinimumSize = new Size(84, 34);
            _btnAnchorCam.Name = "_btnAnchorCam";
            _btnAnchorCam.Padding = new Padding(10, 0, 10, 0);
            _btnAnchorCam.Size = new Size(91, 34);
            _btnAnchorCam.TabIndex = 6;
            _btnAnchorCam.Text = "打开相机";
            _btnAnchorCam.UseVisualStyleBackColor = false;
            // 
            // _btnAnchorJudge
            // 
            _btnAnchorJudge.AutoSize = true;
            _btnAnchorJudge.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnAnchorJudge.BackColor = Color.FromArgb((int)(byte)36, (int)(byte)112, (int)(byte)210);
            _btnAnchorJudge.Cursor = Cursors.Hand;
            _btnAnchorJudge.FlatAppearance.BorderSize = 0;
            _btnAnchorJudge.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)28, (int)(byte)96, (int)(byte)186);
            _btnAnchorJudge.FlatStyle = FlatStyle.Flat;
            _btnAnchorJudge.Font = new Font("Microsoft YaHei UI", 9.5F, FontStyle.Bold);
            _btnAnchorJudge.ForeColor = Color.White;
            _btnAnchorJudge.Location = new Point(534, 5);
            _btnAnchorJudge.Margin = new Padding(0, 5, 8, 0);
            _btnAnchorJudge.MinimumSize = new Size(84, 34);
            _btnAnchorJudge.Name = "_btnAnchorJudge";
            _btnAnchorJudge.Padding = new Padding(10, 0, 10, 0);
            _btnAnchorJudge.Size = new Size(91, 34);
            _btnAnchorJudge.TabIndex = 7;
            _btnAnchorJudge.Text = "判定一次";
            _btnAnchorJudge.UseVisualStyleBackColor = false;
            // 
            // _lblAnchorCamInfo
            // 
            _lblAnchorCamInfo.AutoSize = true;
            _lblAnchorCamInfo.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lblAnchorCamInfo.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblAnchorCamInfo.Location = new Point(633, 10);
            _lblAnchorCamInfo.Margin = new Padding(0, 10, 0, 0);
            _lblAnchorCamInfo.Name = "_lblAnchorCamInfo";
            _lblAnchorCamInfo.Size = new Size(68, 17);
            _lblAnchorCamInfo.TabIndex = 8;
            _lblAnchorCamInfo.Text = "相机未打开";
            // 
            // _pnlAnchorVerdict
            // 
            _pnlAnchorVerdict.BackColor = Color.White;
            _pnlAnchorVerdict.Controls.Add(_tlpAnchorVerdict);
            _pnlAnchorVerdict.Dock = DockStyle.Fill;
            _pnlAnchorVerdict.Location = new Point(836, 0);
            _pnlAnchorVerdict.Margin = new Padding(8, 0, 0, 6);
            _pnlAnchorVerdict.MinimumSize = new Size(0, 96);
            _pnlAnchorVerdict.Name = "_pnlAnchorVerdict";
            _pnlAnchorVerdict.Padding = new Padding(10, 6, 10, 6);
            _pnlAnchorVerdict.Size = new Size(404, 96);
            _pnlAnchorVerdict.TabIndex = 1;
            // 
            // _tlpAnchorVerdict
            // 
            _tlpAnchorVerdict.BackColor = Color.White;
            _tlpAnchorVerdict.ColumnCount = 2;
            _tlpAnchorVerdict.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 96F));
            _tlpAnchorVerdict.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            _tlpAnchorVerdict.Controls.Add(_lblAnchorVerdict, 0, 0);
            _tlpAnchorVerdict.Controls.Add(_lblAnchorReason, 1, 0);
            _tlpAnchorVerdict.Controls.Add(_lblAnchorDetail, 0, 1);
            _tlpAnchorVerdict.Dock = DockStyle.Fill;
            _tlpAnchorVerdict.Location = new Point(10, 6);
            _tlpAnchorVerdict.Margin = new Padding(0);
            _tlpAnchorVerdict.Name = "_tlpAnchorVerdict";
            _tlpAnchorVerdict.RowCount = 2;
            _tlpAnchorVerdict.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
            _tlpAnchorVerdict.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            _tlpAnchorVerdict.Size = new Size(384, 84);
            _tlpAnchorVerdict.TabIndex = 0;
            // 
            // _lblAnchorVerdict
            // 
            _lblAnchorVerdict.Dock = DockStyle.Fill;
            _lblAnchorVerdict.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            _lblAnchorVerdict.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblAnchorVerdict.Location = new Point(0, 0);
            _lblAnchorVerdict.Margin = new Padding(0);
            _lblAnchorVerdict.Name = "_lblAnchorVerdict";
            _lblAnchorVerdict.Size = new Size(96, 44);
            _lblAnchorVerdict.TabIndex = 0;
            _lblAnchorVerdict.Text = "—";
            _lblAnchorVerdict.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // _lblAnchorReason
            // 
            _lblAnchorReason.Dock = DockStyle.Fill;
            _lblAnchorReason.Font = new Font("Microsoft YaHei UI", 9.5F, FontStyle.Bold);
            _lblAnchorReason.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblAnchorReason.Location = new Point(104, 0);
            _lblAnchorReason.Margin = new Padding(8, 0, 0, 0);
            _lblAnchorReason.Name = "_lblAnchorReason";
            _lblAnchorReason.Size = new Size(280, 44);
            _lblAnchorReason.TabIndex = 1;
            _lblAnchorReason.Text = "还没有判定";
            _lblAnchorReason.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // _lblAnchorDetail
            // 
            _tlpAnchorVerdict.SetColumnSpan(_lblAnchorDetail, 2);
            _lblAnchorDetail.Dock = DockStyle.Fill;
            _lblAnchorDetail.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lblAnchorDetail.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblAnchorDetail.Location = new Point(0, 48);
            _lblAnchorDetail.Margin = new Padding(0, 4, 0, 0);
            _lblAnchorDetail.Name = "_lblAnchorDetail";
            _lblAnchorDetail.Size = new Size(384, 36);
            _lblAnchorDetail.TabIndex = 2;
            _lblAnchorDetail.Text = "放好板 → 到「② 检测范围 · 判定」点“判定一次”。";
            // 
            // _tlpAnchorBody
            // 
            _tlpAnchorBody.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _tlpAnchorBody.ColumnCount = 2;
            _tlpAnchorBody.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            _tlpAnchorBody.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 412F));
            _tlpAnchorBody.Controls.Add(_picAnchor, 0, 0);
            _tlpAnchorBody.Controls.Add(_tlpAnchorSide, 1, 0);
            _tlpAnchorBody.Dock = DockStyle.Fill;
            _tlpAnchorBody.Location = new Point(3, 103);
            _tlpAnchorBody.Name = "_tlpAnchorBody";
            _tlpAnchorBody.RowCount = 1;
            _tlpAnchorBody.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            _tlpAnchorBody.Size = new Size(1234, 547);
            _tlpAnchorBody.TabIndex = 1;
            // 
            // _picAnchor
            // 
            _picAnchor.BackColor = Color.FromArgb((int)(byte)36, (int)(byte)40, (int)(byte)47);
            _picAnchor.BoxLabel = null;
            _picAnchor.BoxPending = null;
            _picAnchor.CanvasMenu = null;
            _picAnchor.DetectionLabel = null;
            _picAnchor.Dock = DockStyle.Fill;
            _picAnchor.Editable = false;
            _picAnchor.EmptyText = "① 选机种，把板放上针床\r\n② 打开相机 → 点上方【拍照】\r\n③ 在「① 识别目标」里框选特征（右键确认后才入队）\r\n④ 在「② 检测范围 · 判定」里框范围、采集参考、设匹配率\r\n⑤ 换板后点“判定一次”，结果显示在右上角";
            _picAnchor.Font = new Font("Microsoft YaHei UI", 9.5F);
            _picAnchor.Image = null;
            _picAnchor.LineA = null;
            _picAnchor.LineB = null;
            _picAnchor.LineEditable = false;
            _picAnchor.LineLabel = "计数线";
            _picAnchor.Location = new Point(0, 0);
            _picAnchor.Margin = new Padding(0, 0, 10, 0);
            _picAnchor.Name = "_picAnchor";
            _picAnchor.RightClickAction = null;
            _picAnchor.SelectedIndex = -1;
            _picAnchor.ShowZoomInfo = true;
            _picAnchor.Size = new Size(812, 547);
            _picAnchor.TabIndex = 0;
            _picAnchor.TrackLabel = null;
            // 
            // _tlpAnchorSide
            // 
            _tlpAnchorSide.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _tlpAnchorSide.ColumnCount = 1;
            _tlpAnchorSide.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            _tlpAnchorSide.Controls.Add(_anchorTabs, 0, 0);
            _tlpAnchorSide.Dock = DockStyle.Fill;
            _tlpAnchorSide.Location = new Point(825, 3);
            _tlpAnchorSide.Name = "_tlpAnchorSide";
            _tlpAnchorSide.RowCount = 1;
            _tlpAnchorSide.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            _tlpAnchorSide.Size = new Size(406, 541);
            _tlpAnchorSide.TabIndex = 1;
            // 
            // _anchorTabs
            // 
            _anchorTabs.Controls.Add(_tabAnchorTarget);
            _anchorTabs.Controls.Add(_tabAnchorZone);
            _anchorTabs.Controls.Add(_tabAnchorCalib);
            _anchorTabs.Dock = DockStyle.Fill;
            _anchorTabs.Font = new Font("Microsoft YaHei UI", 9.5F);
            _anchorTabs.Location = new Point(3, 3);
            _anchorTabs.Name = "_anchorTabs";
            _anchorTabs.SelectedIndex = 0;
            _anchorTabs.Size = new Size(400, 535);
            _anchorTabs.TabIndex = 0;
            // 
            // _tabAnchorTarget
            // 
            _tabAnchorTarget.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _tabAnchorTarget.Controls.Add(_hostAnchorLearn);
            _tabAnchorTarget.Location = new Point(4, 28);
            _tabAnchorTarget.Name = "_tabAnchorTarget";
            _tabAnchorTarget.Padding = new Padding(4);
            _tabAnchorTarget.Size = new Size(392, 503);
            _tabAnchorTarget.TabIndex = 0;
            _tabAnchorTarget.Text = "① 识别目标";
            // 
            // _hostAnchorLearn
            // 
            _hostAnchorLearn.AutoScroll = true;
            _hostAnchorLearn.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _hostAnchorLearn.Controls.Add(_tlpAnchorLearn);
            _hostAnchorLearn.Dock = DockStyle.Fill;
            _hostAnchorLearn.Location = new Point(4, 4);
            _hostAnchorLearn.Name = "_hostAnchorLearn";
            _hostAnchorLearn.Size = new Size(384, 495);
            _hostAnchorLearn.TabIndex = 0;
            // 
            // _tlpAnchorLearn
            // 
            _tlpAnchorLearn.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _tlpAnchorLearn.ColumnCount = 1;
            _tlpAnchorLearn.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            _tlpAnchorLearn.Controls.Add(_flowAnchorLearnBar, 0, 0);
            _tlpAnchorLearn.Controls.Add(_lblAnchorLearnInfo, 0, 1);
            _tlpAnchorLearn.Controls.Add(_lvAnchorDrafts, 0, 2);
            _tlpAnchorLearn.Controls.Add(_lvAnchorSamples, 0, 3);
            _tlpAnchorLearn.Controls.Add(_lblAnchorSampleInfo, 0, 4);
            _tlpAnchorLearn.Controls.Add(_picAnchorSample, 0, 5);
            _tlpAnchorLearn.Controls.Add(_tlpAnchorTrainBar, 0, 6);
            _tlpAnchorLearn.Controls.Add(_txtAnchorTrainLog, 0, 7);
            _tlpAnchorLearn.Controls.Add(_lblAnchorClasses, 0, 8);
            _tlpAnchorLearn.Controls.Add(_lblAnchorTargetInfo, 0, 9);
            _tlpAnchorLearn.Dock = DockStyle.Top;
            _tlpAnchorLearn.Location = new Point(0, 0);
            _tlpAnchorLearn.Name = "_tlpAnchorLearn";
            _tlpAnchorLearn.RowCount = 10;
            _tlpAnchorLearn.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
            _tlpAnchorLearn.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            _tlpAnchorLearn.RowStyles.Add(new RowStyle(SizeType.Absolute, 86F));
            _tlpAnchorLearn.RowStyles.Add(new RowStyle(SizeType.Absolute, 132F));
            _tlpAnchorLearn.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
            _tlpAnchorLearn.RowStyles.Add(new RowStyle(SizeType.Absolute, 132F));
            _tlpAnchorLearn.RowStyles.Add(new RowStyle(SizeType.Absolute, 96F));
            _tlpAnchorLearn.RowStyles.Add(new RowStyle(SizeType.Absolute, 108F));
            _tlpAnchorLearn.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
            _tlpAnchorLearn.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));
            _tlpAnchorLearn.Size = new Size(367, 758);
            _tlpAnchorLearn.TabIndex = 0;
            // 
            // _flowAnchorLearnBar
            // 
            _flowAnchorLearnBar.AutoSize = true;
            _flowAnchorLearnBar.Controls.Add(_btnAnchorLearnPick);
            _flowAnchorLearnBar.Controls.Add(_btnAnchorLearnDel);
            _flowAnchorLearnBar.Controls.Add(_btnAnchorLearnDelShot);
            _flowAnchorLearnBar.Controls.Add(_btnAnchorLearnLive);
            _flowAnchorLearnBar.Dock = DockStyle.Fill;
            _flowAnchorLearnBar.Location = new Point(0, 0);
            _flowAnchorLearnBar.Margin = new Padding(0, 0, 0, 4);
            _flowAnchorLearnBar.Name = "_flowAnchorLearnBar";
            _flowAnchorLearnBar.Size = new Size(367, 40);
            _flowAnchorLearnBar.TabIndex = 0;
            // 
            // _btnAnchorLearnPick
            // 
            _btnAnchorLearnPick.AutoSize = true;
            _btnAnchorLearnPick.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnAnchorLearnPick.BackColor = Color.FromArgb((int)(byte)36, (int)(byte)112, (int)(byte)210);
            _btnAnchorLearnPick.Cursor = Cursors.Hand;
            _btnAnchorLearnPick.FlatAppearance.BorderSize = 0;
            _btnAnchorLearnPick.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)28, (int)(byte)96, (int)(byte)186);
            _btnAnchorLearnPick.FlatStyle = FlatStyle.Flat;
            _btnAnchorLearnPick.Font = new Font("Microsoft YaHei UI", 9.5F, FontStyle.Bold);
            _btnAnchorLearnPick.ForeColor = Color.White;
            _btnAnchorLearnPick.Location = new Point(4, 4);
            _btnAnchorLearnPick.Margin = new Padding(4);
            _btnAnchorLearnPick.MinimumSize = new Size(84, 34);
            _btnAnchorLearnPick.Name = "_btnAnchorLearnPick";
            _btnAnchorLearnPick.Padding = new Padding(6, 2, 6, 2);
            _btnAnchorLearnPick.Size = new Size(84, 34);
            _btnAnchorLearnPick.TabIndex = 0;
            _btnAnchorLearnPick.Text = "框选目标";
            _btnAnchorLearnPick.UseVisualStyleBackColor = false;
            // 
            // _btnAnchorLearnDel
            // 
            _btnAnchorLearnDel.AutoSize = true;
            _btnAnchorLearnDel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnAnchorLearnDel.BackColor = Color.White;
            _btnAnchorLearnDel.Cursor = Cursors.Hand;
            _btnAnchorLearnDel.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnAnchorLearnDel.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnAnchorLearnDel.FlatStyle = FlatStyle.Flat;
            _btnAnchorLearnDel.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnAnchorLearnDel.ForeColor = Color.FromArgb((int)(byte)190, (int)(byte)58, (int)(byte)58);
            _btnAnchorLearnDel.Location = new Point(96, 4);
            _btnAnchorLearnDel.Margin = new Padding(4);
            _btnAnchorLearnDel.MinimumSize = new Size(84, 34);
            _btnAnchorLearnDel.Name = "_btnAnchorLearnDel";
            _btnAnchorLearnDel.Padding = new Padding(6, 2, 6, 2);
            _btnAnchorLearnDel.Size = new Size(85, 35);
            _btnAnchorLearnDel.TabIndex = 1;
            _btnAnchorLearnDel.Text = "删除选中";
            _btnAnchorLearnDel.UseVisualStyleBackColor = false;
            // 
            // _btnAnchorLearnDelShot
            // 
            _btnAnchorLearnDelShot.AutoSize = true;
            _btnAnchorLearnDelShot.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnAnchorLearnDelShot.BackColor = Color.White;
            _btnAnchorLearnDelShot.Cursor = Cursors.Hand;
            _btnAnchorLearnDelShot.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnAnchorLearnDelShot.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnAnchorLearnDelShot.FlatStyle = FlatStyle.Flat;
            _btnAnchorLearnDelShot.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnAnchorLearnDelShot.ForeColor = Color.FromArgb((int)(byte)190, (int)(byte)58, (int)(byte)58);
            _btnAnchorLearnDelShot.Location = new Point(189, 4);
            _btnAnchorLearnDelShot.Margin = new Padding(4);
            _btnAnchorLearnDelShot.MinimumSize = new Size(84, 34);
            _btnAnchorLearnDelShot.Name = "_btnAnchorLearnDelShot";
            _btnAnchorLearnDelShot.Padding = new Padding(6, 2, 6, 2);
            _btnAnchorLearnDelShot.Size = new Size(85, 35);
            _btnAnchorLearnDelShot.TabIndex = 2;
            _btnAnchorLearnDelShot.Text = "删除本张";
            _btnAnchorLearnDelShot.UseVisualStyleBackColor = false;
            // 
            // _btnAnchorLearnLive
            // 
            _btnAnchorLearnLive.AutoSize = true;
            _btnAnchorLearnLive.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnAnchorLearnLive.BackColor = Color.White;
            _btnAnchorLearnLive.Cursor = Cursors.Hand;
            _btnAnchorLearnLive.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnAnchorLearnLive.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnAnchorLearnLive.FlatStyle = FlatStyle.Flat;
            _btnAnchorLearnLive.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnAnchorLearnLive.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnAnchorLearnLive.Location = new Point(4, 47);
            _btnAnchorLearnLive.Margin = new Padding(4);
            _btnAnchorLearnLive.MinimumSize = new Size(84, 34);
            _btnAnchorLearnLive.Name = "_btnAnchorLearnLive";
            _btnAnchorLearnLive.Padding = new Padding(6, 2, 6, 2);
            _btnAnchorLearnLive.Size = new Size(85, 35);
            _btnAnchorLearnLive.TabIndex = 3;
            _btnAnchorLearnLive.Text = "回到实时";
            _btnAnchorLearnLive.UseVisualStyleBackColor = false;
            // 
            // _lblAnchorLearnInfo
            // 
            _lblAnchorLearnInfo.Dock = DockStyle.Fill;
            _lblAnchorLearnInfo.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lblAnchorLearnInfo.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblAnchorLearnInfo.Location = new Point(0, 44);
            _lblAnchorLearnInfo.Margin = new Padding(0, 0, 0, 4);
            _lblAnchorLearnInfo.Name = "_lblAnchorLearnInfo";
            _lblAnchorLearnInfo.Size = new Size(367, 44);
            _lblAnchorLearnInfo.TabIndex = 1;
            _lblAnchorLearnInfo.Text = "① 点上方【拍照】存一张照片 ② 在照片上拖动框选，右键/回车确认 ③ 一张照片可框多个目标，可继续拍照 ④ 选中框可拉伸、旋转，Delete 删除";
            // 
            // _lvAnchorDrafts
            // 
            _lvAnchorDrafts.BackColor = Color.White;
            _lvAnchorDrafts.BorderStyle = BorderStyle.FixedSingle;
            _lvAnchorDrafts.Columns.AddRange(new ColumnHeader[] { colAnchorDraft, colAnchorDraftPos, colAnchorDraftAngle });
            _lvAnchorDrafts.Dock = DockStyle.Fill;
            _lvAnchorDrafts.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lvAnchorDrafts.FullRowSelect = true;
            _lvAnchorDrafts.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            _lvAnchorDrafts.Location = new Point(0, 92);
            _lvAnchorDrafts.Margin = new Padding(0, 0, 0, 4);
            _lvAnchorDrafts.MultiSelect = false;
            _lvAnchorDrafts.Name = "_lvAnchorDrafts";
            _lvAnchorDrafts.Size = new Size(367, 82);
            _lvAnchorDrafts.TabIndex = 2;
            _lvAnchorDrafts.UseCompatibleStateImageBehavior = false;
            _lvAnchorDrafts.View = View.Details;
            // 
            // colAnchorDraft
            // 
            colAnchorDraft.Name = "colAnchorDraft";
            colAnchorDraft.Text = "目标";
            colAnchorDraft.Width = 96;
            // 
            // colAnchorDraftPos
            // 
            colAnchorDraftPos.Name = "colAnchorDraftPos";
            colAnchorDraftPos.Text = "位置";
            colAnchorDraftPos.Width = 128;
            // 
            // colAnchorDraftAngle
            // 
            colAnchorDraftAngle.Name = "colAnchorDraftAngle";
            colAnchorDraftAngle.Text = "角度";
            colAnchorDraftAngle.Width = 54;
            // 
            // _lvAnchorSamples
            // 
            _lvAnchorSamples.BackColor = Color.White;
            _lvAnchorSamples.BorderStyle = BorderStyle.FixedSingle;
            _lvAnchorSamples.Columns.AddRange(new ColumnHeader[] { colAnchorSample, colAnchorSampleTarget, colAnchorSampleBoxes, colAnchorSampleTime });
            _lvAnchorSamples.Dock = DockStyle.Fill;
            _lvAnchorSamples.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lvAnchorSamples.FullRowSelect = true;
            _lvAnchorSamples.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            _lvAnchorSamples.Location = new Point(0, 178);
            _lvAnchorSamples.Margin = new Padding(0, 0, 0, 4);
            _lvAnchorSamples.MultiSelect = false;
            _lvAnchorSamples.Name = "_lvAnchorSamples";
            _lvAnchorSamples.Size = new Size(367, 128);
            _lvAnchorSamples.TabIndex = 3;
            _lvAnchorSamples.UseCompatibleStateImageBehavior = false;
            _lvAnchorSamples.View = View.Details;
            // 
            // colAnchorSample
            // 
            colAnchorSample.Name = "colAnchorSample";
            colAnchorSample.Text = "照片";
            colAnchorSample.Width = 74;
            // 
            // colAnchorSampleTarget
            // 
            colAnchorSampleTarget.Name = "colAnchorSampleTarget";
            colAnchorSampleTarget.Text = "目标";
            colAnchorSampleTarget.Width = 128;
            // 
            // colAnchorSampleBoxes
            // 
            colAnchorSampleBoxes.Name = "colAnchorSampleBoxes";
            colAnchorSampleBoxes.Text = "框数";
            colAnchorSampleBoxes.Width = 42;
            // 
            // colAnchorSampleTime
            // 
            colAnchorSampleTime.Name = "colAnchorSampleTime";
            colAnchorSampleTime.Text = "时间";
            colAnchorSampleTime.Width = 92;
            // 
            // _lblAnchorSampleInfo
            // 
            _lblAnchorSampleInfo.Dock = DockStyle.Fill;
            _lblAnchorSampleInfo.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lblAnchorSampleInfo.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblAnchorSampleInfo.Location = new Point(3, 310);
            _lblAnchorSampleInfo.Name = "_lblAnchorSampleInfo";
            _lblAnchorSampleInfo.Size = new Size(361, 24);
            _lblAnchorSampleInfo.TabIndex = 4;
            _lblAnchorSampleInfo.Text = "选中一条照片就会载入左侧画布，可继续加框或调整。";
            // 
            // _picAnchorSample
            // 
            _picAnchorSample.BackColor = Color.White;
            _picAnchorSample.BoxLabel = null;
            _picAnchorSample.BoxPending = null;
            _picAnchorSample.CanvasMenu = null;
            _picAnchorSample.DetectionLabel = null;
            _picAnchorSample.Dock = DockStyle.Fill;
            _picAnchorSample.Editable = false;
            _picAnchorSample.EmptyText = "选中上面的样本，这里显示标注效果";
            _picAnchorSample.Font = new Font("Microsoft YaHei UI", 9.5F);
            _picAnchorSample.Image = null;
            _picAnchorSample.LineA = null;
            _picAnchorSample.LineB = null;
            _picAnchorSample.LineEditable = false;
            _picAnchorSample.LineLabel = "计数线";
            _picAnchorSample.Location = new Point(0, 334);
            _picAnchorSample.Margin = new Padding(0, 0, 0, 4);
            _picAnchorSample.Name = "_picAnchorSample";
            _picAnchorSample.RightClickAction = null;
            _picAnchorSample.SelectedIndex = -1;
            _picAnchorSample.ShowZoomInfo = true;
            _picAnchorSample.Size = new Size(367, 128);
            _picAnchorSample.TabIndex = 5;
            _picAnchorSample.TrackLabel = null;
            // 
            // _tlpAnchorTrainBar
            // 
            _tlpAnchorTrainBar.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _tlpAnchorTrainBar.ColumnCount = 1;
            _tlpAnchorTrainBar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            _tlpAnchorTrainBar.Controls.Add(_flowAnchorTrainParams, 0, 0);
            _tlpAnchorTrainBar.Controls.Add(_flowAnchorTrainBtns, 0, 1);
            _tlpAnchorTrainBar.Dock = DockStyle.Fill;
            _tlpAnchorTrainBar.Location = new Point(0, 466);
            _tlpAnchorTrainBar.Margin = new Padding(0, 0, 0, 4);
            _tlpAnchorTrainBar.Name = "_tlpAnchorTrainBar";
            _tlpAnchorTrainBar.RowCount = 2;
            _tlpAnchorTrainBar.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            _tlpAnchorTrainBar.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            _tlpAnchorTrainBar.Size = new Size(367, 92);
            _tlpAnchorTrainBar.TabIndex = 6;
            // 
            // _flowAnchorTrainParams
            // 
            _flowAnchorTrainParams.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _flowAnchorTrainParams.Controls.Add(_lblAnchorEpochsCap);
            _flowAnchorTrainParams.Controls.Add(_numTrainEpochs);
            _flowAnchorTrainParams.Controls.Add(_lblAnchorImgszCap);
            _flowAnchorTrainParams.Controls.Add(_numTrainImgsz);
            _flowAnchorTrainParams.Dock = DockStyle.Fill;
            _flowAnchorTrainParams.Location = new Point(0, 0);
            _flowAnchorTrainParams.Margin = new Padding(0);
            _flowAnchorTrainParams.Name = "_flowAnchorTrainParams";
            _flowAnchorTrainParams.Size = new Size(367, 32);
            _flowAnchorTrainParams.TabIndex = 0;
            _flowAnchorTrainParams.WrapContents = false;
            // 
            // _lblAnchorEpochsCap
            // 
            _lblAnchorEpochsCap.AutoSize = true;
            _lblAnchorEpochsCap.Font = new Font("Microsoft YaHei UI", 9.5F);
            _lblAnchorEpochsCap.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblAnchorEpochsCap.Location = new Point(0, 6);
            _lblAnchorEpochsCap.Margin = new Padding(0, 6, 4, 0);
            _lblAnchorEpochsCap.Name = "_lblAnchorEpochsCap";
            _lblAnchorEpochsCap.Size = new Size(35, 19);
            _lblAnchorEpochsCap.TabIndex = 0;
            _lblAnchorEpochsCap.Text = "轮数";
            // 
            // _numTrainEpochs
            // 
            _numTrainEpochs.Font = new Font("Microsoft YaHei UI", 9.5F);
            _numTrainEpochs.Increment = new decimal(new int[] { 5, 0, 0, 0 });
            _numTrainEpochs.Location = new Point(39, 4);
            _numTrainEpochs.Margin = new Padding(0, 4, 0, 4);
            _numTrainEpochs.Maximum = new decimal(new int[] { 300, 0, 0, 0 });
            _numTrainEpochs.Minimum = new decimal(new int[] { 5, 0, 0, 0 });
            _numTrainEpochs.Name = "_numTrainEpochs";
            _numTrainEpochs.Size = new Size(60, 24);
            _numTrainEpochs.TabIndex = 1;
            _numTrainEpochs.TextAlign = HorizontalAlignment.Right;
            _numTrainEpochs.Value = new decimal(new int[] { 60, 0, 0, 0 });
            // 
            // _lblAnchorImgszCap
            // 
            _lblAnchorImgszCap.AutoSize = true;
            _lblAnchorImgszCap.Font = new Font("Microsoft YaHei UI", 9.5F);
            _lblAnchorImgszCap.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblAnchorImgszCap.Location = new Point(113, 6);
            _lblAnchorImgszCap.Margin = new Padding(14, 6, 4, 0);
            _lblAnchorImgszCap.Name = "_lblAnchorImgszCap";
            _lblAnchorImgszCap.Size = new Size(35, 19);
            _lblAnchorImgszCap.TabIndex = 2;
            _lblAnchorImgszCap.Text = "尺寸";
            // 
            // _numTrainImgsz
            // 
            _numTrainImgsz.Font = new Font("Microsoft YaHei UI", 9.5F);
            _numTrainImgsz.Increment = new decimal(new int[] { 64, 0, 0, 0 });
            _numTrainImgsz.Location = new Point(152, 4);
            _numTrainImgsz.Margin = new Padding(0, 4, 0, 4);
            _numTrainImgsz.Maximum = new decimal(new int[] { 1280, 0, 0, 0 });
            _numTrainImgsz.Minimum = new decimal(new int[] { 320, 0, 0, 0 });
            _numTrainImgsz.Name = "_numTrainImgsz";
            _numTrainImgsz.Size = new Size(66, 24);
            _numTrainImgsz.TabIndex = 3;
            _numTrainImgsz.TextAlign = HorizontalAlignment.Right;
            _numTrainImgsz.Value = new decimal(new int[] { 640, 0, 0, 0 });
            // 
            // _flowAnchorTrainBtns
            // 
            _flowAnchorTrainBtns.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _flowAnchorTrainBtns.Controls.Add(_btnAnchorTrain);
            _flowAnchorTrainBtns.Controls.Add(_btnAnchorTrainRefresh);
            _flowAnchorTrainBtns.Dock = DockStyle.Fill;
            _flowAnchorTrainBtns.Location = new Point(0, 32);
            _flowAnchorTrainBtns.Margin = new Padding(0);
            _flowAnchorTrainBtns.Name = "_flowAnchorTrainBtns";
            _flowAnchorTrainBtns.Size = new Size(367, 60);
            _flowAnchorTrainBtns.TabIndex = 1;
            _flowAnchorTrainBtns.WrapContents = false;
            // 
            // _btnAnchorTrain
            // 
            _btnAnchorTrain.AutoSize = true;
            _btnAnchorTrain.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnAnchorTrain.BackColor = Color.FromArgb((int)(byte)22, (int)(byte)138, (int)(byte)88);
            _btnAnchorTrain.Cursor = Cursors.Hand;
            _btnAnchorTrain.FlatAppearance.BorderSize = 0;
            _btnAnchorTrain.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)18, (int)(byte)118, (int)(byte)74);
            _btnAnchorTrain.FlatStyle = FlatStyle.Flat;
            _btnAnchorTrain.Font = new Font("Microsoft YaHei UI", 9.5F, FontStyle.Bold);
            _btnAnchorTrain.ForeColor = Color.White;
            _btnAnchorTrain.Location = new Point(0, 0);
            _btnAnchorTrain.Margin = new Padding(0, 0, 8, 0);
            _btnAnchorTrain.MinimumSize = new Size(84, 34);
            _btnAnchorTrain.Name = "_btnAnchorTrain";
            _btnAnchorTrain.Padding = new Padding(8, 4, 8, 4);
            _btnAnchorTrain.Size = new Size(100, 37);
            _btnAnchorTrain.TabIndex = 0;
            _btnAnchorTrain.Text = "训练并部署";
            _btnAnchorTrain.UseVisualStyleBackColor = false;
            // 
            // _btnAnchorTrainRefresh
            // 
            _btnAnchorTrainRefresh.AutoSize = true;
            _btnAnchorTrainRefresh.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnAnchorTrainRefresh.BackColor = Color.White;
            _btnAnchorTrainRefresh.Cursor = Cursors.Hand;
            _btnAnchorTrainRefresh.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnAnchorTrainRefresh.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnAnchorTrainRefresh.FlatStyle = FlatStyle.Flat;
            _btnAnchorTrainRefresh.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnAnchorTrainRefresh.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnAnchorTrainRefresh.Location = new Point(108, 0);
            _btnAnchorTrainRefresh.Margin = new Padding(0, 0, 4, 0);
            _btnAnchorTrainRefresh.MinimumSize = new Size(84, 34);
            _btnAnchorTrainRefresh.Name = "_btnAnchorTrainRefresh";
            _btnAnchorTrainRefresh.Padding = new Padding(8, 4, 8, 4);
            _btnAnchorTrainRefresh.Size = new Size(84, 39);
            _btnAnchorTrainRefresh.TabIndex = 1;
            _btnAnchorTrainRefresh.Text = "刷新";
            _btnAnchorTrainRefresh.UseVisualStyleBackColor = false;
            // 
            // _txtAnchorTrainLog
            // 
            _txtAnchorTrainLog.BackColor = Color.White;
            _txtAnchorTrainLog.BorderStyle = BorderStyle.FixedSingle;
            _txtAnchorTrainLog.Dock = DockStyle.Fill;
            _txtAnchorTrainLog.Font = new Font("Consolas", 8.5F);
            _txtAnchorTrainLog.Location = new Point(0, 562);
            _txtAnchorTrainLog.Margin = new Padding(0, 0, 0, 4);
            _txtAnchorTrainLog.Multiline = true;
            _txtAnchorTrainLog.Name = "_txtAnchorTrainLog";
            _txtAnchorTrainLog.ReadOnly = true;
            _txtAnchorTrainLog.ScrollBars = ScrollBars.Vertical;
            _txtAnchorTrainLog.Size = new Size(367, 104);
            _txtAnchorTrainLog.TabIndex = 7;
            _txtAnchorTrainLog.WordWrap = false;
            // 
            // _lblAnchorClasses
            // 
            _lblAnchorClasses.AutoEllipsis = true;
            _lblAnchorClasses.Dock = DockStyle.Fill;
            _lblAnchorClasses.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lblAnchorClasses.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblAnchorClasses.Location = new Point(0, 670);
            _lblAnchorClasses.Margin = new Padding(0, 0, 0, 2);
            _lblAnchorClasses.Name = "_lblAnchorClasses";
            _lblAnchorClasses.Size = new Size(367, 34);
            _lblAnchorClasses.TabIndex = 8;
            _lblAnchorClasses.Text = "模型标签：加载模型后显示。";
            // 
            // _lblAnchorTargetInfo
            // 
            _lblAnchorTargetInfo.Dock = DockStyle.Fill;
            _lblAnchorTargetInfo.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lblAnchorTargetInfo.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblAnchorTargetInfo.Location = new Point(3, 706);
            _lblAnchorTargetInfo.Name = "_lblAnchorTargetInfo";
            _lblAnchorTargetInfo.Size = new Size(361, 52);
            _lblAnchorTargetInfo.TabIndex = 9;
            _lblAnchorTargetInfo.Text = "每换一次位置/光照拍一张，同一目标 60 张以上再训练。没有框的照片不计入训练。";
            // 
            // _tabAnchorZone
            // 
            _tabAnchorZone.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _tabAnchorZone.Controls.Add(_hostAnchorZone);
            _tabAnchorZone.Location = new Point(4, 28);
            _tabAnchorZone.Name = "_tabAnchorZone";
            _tabAnchorZone.Padding = new Padding(4);
            _tabAnchorZone.Size = new Size(392, 503);
            _tabAnchorZone.TabIndex = 1;
            _tabAnchorZone.Text = "② 检测范围 · 判定";
            // 
            // _hostAnchorZone
            // 
            _hostAnchorZone.AutoScroll = true;
            _hostAnchorZone.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _hostAnchorZone.Controls.Add(_tlpAnchorZone);
            _hostAnchorZone.Dock = DockStyle.Fill;
            _hostAnchorZone.Location = new Point(4, 4);
            _hostAnchorZone.Name = "_hostAnchorZone";
            _hostAnchorZone.Size = new Size(384, 495);
            _hostAnchorZone.TabIndex = 0;
            // 
            // _tlpAnchorZone
            // 
            _tlpAnchorZone.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _tlpAnchorZone.ColumnCount = 1;
            _tlpAnchorZone.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            _tlpAnchorZone.Controls.Add(_flowAnchorZoneBar, 0, 0);
            _tlpAnchorZone.Controls.Add(_lvAnchorZones, 0, 1);
            _tlpAnchorZone.Controls.Add(_tlpAnchorZoneRef, 0, 2);
            _tlpAnchorZone.Controls.Add(_tlpAnchorZoneParams, 0, 3);
            _tlpAnchorZone.Controls.Add(_chkAnchorAdv, 0, 4);
            _tlpAnchorZone.Controls.Add(_pnlAnchorTol, 0, 5);
            _tlpAnchorZone.Controls.Add(_flowAnchorZoneJudge, 0, 6);
            _tlpAnchorZone.Controls.Add(_pnlAnchorHitsCard, 0, 7);
            _tlpAnchorZone.Controls.Add(_pnlAnchorLogCard, 0, 8);
            _tlpAnchorZone.Controls.Add(_tlpAnchorZoneHints, 0, 9);
            _tlpAnchorZone.Dock = DockStyle.Top;
            _tlpAnchorZone.Location = new Point(0, 0);
            _tlpAnchorZone.Name = "_tlpAnchorZone";
            _tlpAnchorZone.RowCount = 10;
            _tlpAnchorZone.RowStyles.Add(new RowStyle(SizeType.Absolute, 88F));
            _tlpAnchorZone.RowStyles.Add(new RowStyle(SizeType.Absolute, 132F));
            _tlpAnchorZone.RowStyles.Add(new RowStyle(SizeType.Absolute, 88F));
            _tlpAnchorZone.RowStyles.Add(new RowStyle(SizeType.Absolute, 214F));
            _tlpAnchorZone.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            _tlpAnchorZone.RowStyles.Add(new RowStyle(SizeType.Absolute, 0F));
            _tlpAnchorZone.RowStyles.Add(new RowStyle(SizeType.Absolute, 88F));
            _tlpAnchorZone.RowStyles.Add(new RowStyle(SizeType.Absolute, 160F));
            _tlpAnchorZone.RowStyles.Add(new RowStyle(SizeType.Absolute, 112F));
            _tlpAnchorZone.RowStyles.Add(new RowStyle(SizeType.Absolute, 84F));
            _tlpAnchorZone.Size = new Size(367, 992);
            _tlpAnchorZone.TabIndex = 0;
            // 
            // _flowAnchorZoneBar
            // 
            _flowAnchorZoneBar.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _flowAnchorZoneBar.Controls.Add(_btnAnchorZonePick);
            _flowAnchorZoneBar.Controls.Add(_btnAnchorZoneFromLabel);
            _flowAnchorZoneBar.Controls.Add(_btnAnchorZoneCapture);
            _flowAnchorZoneBar.Controls.Add(_btnAnchorZoneEdit);
            _flowAnchorZoneBar.Controls.Add(_btnAnchorZoneRename);
            _flowAnchorZoneBar.Controls.Add(_btnAnchorZoneRemove);
            _flowAnchorZoneBar.Dock = DockStyle.Fill;
            _flowAnchorZoneBar.Location = new Point(0, 0);
            _flowAnchorZoneBar.Margin = new Padding(0, 0, 0, 6);
            _flowAnchorZoneBar.Name = "_flowAnchorZoneBar";
            _flowAnchorZoneBar.Size = new Size(367, 82);
            _flowAnchorZoneBar.TabIndex = 0;
            // 
            // _btnAnchorZonePick
            // 
            _btnAnchorZonePick.AutoSize = true;
            _btnAnchorZonePick.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnAnchorZonePick.BackColor = Color.FromArgb((int)(byte)36, (int)(byte)112, (int)(byte)210);
            _btnAnchorZonePick.Cursor = Cursors.Hand;
            _btnAnchorZonePick.FlatAppearance.BorderSize = 0;
            _btnAnchorZonePick.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)28, (int)(byte)96, (int)(byte)186);
            _btnAnchorZonePick.FlatStyle = FlatStyle.Flat;
            _btnAnchorZonePick.Font = new Font("Microsoft YaHei UI", 9.5F, FontStyle.Bold);
            _btnAnchorZonePick.ForeColor = Color.White;
            _btnAnchorZonePick.Location = new Point(4, 4);
            _btnAnchorZonePick.Margin = new Padding(4);
            _btnAnchorZonePick.MinimumSize = new Size(84, 34);
            _btnAnchorZonePick.Name = "_btnAnchorZonePick";
            _btnAnchorZonePick.Padding = new Padding(8, 2, 8, 2);
            _btnAnchorZonePick.Size = new Size(100, 34);
            _btnAnchorZonePick.TabIndex = 0;
            _btnAnchorZonePick.Text = "框选新范围";
            _btnAnchorZonePick.UseVisualStyleBackColor = false;
            // 
            // _btnAnchorZoneFromLabel
            // 
            _btnAnchorZoneFromLabel.AutoSize = true;
            _btnAnchorZoneFromLabel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnAnchorZoneFromLabel.BackColor = Color.FromArgb((int)(byte)22, (int)(byte)138, (int)(byte)88);
            _btnAnchorZoneFromLabel.Cursor = Cursors.Hand;
            _btnAnchorZoneFromLabel.FlatAppearance.BorderSize = 0;
            _btnAnchorZoneFromLabel.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)18, (int)(byte)118, (int)(byte)74);
            _btnAnchorZoneFromLabel.FlatStyle = FlatStyle.Flat;
            _btnAnchorZoneFromLabel.Font = new Font("Microsoft YaHei UI", 9.5F, FontStyle.Bold);
            _btnAnchorZoneFromLabel.ForeColor = Color.White;
            _btnAnchorZoneFromLabel.Location = new Point(112, 4);
            _btnAnchorZoneFromLabel.Margin = new Padding(4);
            _btnAnchorZoneFromLabel.MinimumSize = new Size(84, 34);
            _btnAnchorZoneFromLabel.Name = "_btnAnchorZoneFromLabel";
            _btnAnchorZoneFromLabel.Padding = new Padding(8, 2, 8, 2);
            _btnAnchorZoneFromLabel.Size = new Size(100, 34);
            _btnAnchorZoneFromLabel.TabIndex = 1;
            _btnAnchorZoneFromLabel.Text = "按标签新建";
            _btnAnchorZoneFromLabel.UseVisualStyleBackColor = false;
            // 
            // _btnAnchorZoneCapture
            // 
            _btnAnchorZoneCapture.AutoSize = true;
            _btnAnchorZoneCapture.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnAnchorZoneCapture.BackColor = Color.White;
            _btnAnchorZoneCapture.Cursor = Cursors.Hand;
            _btnAnchorZoneCapture.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnAnchorZoneCapture.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnAnchorZoneCapture.FlatStyle = FlatStyle.Flat;
            _btnAnchorZoneCapture.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnAnchorZoneCapture.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnAnchorZoneCapture.Location = new Point(220, 4);
            _btnAnchorZoneCapture.Margin = new Padding(4);
            _btnAnchorZoneCapture.MinimumSize = new Size(84, 34);
            _btnAnchorZoneCapture.Name = "_btnAnchorZoneCapture";
            _btnAnchorZoneCapture.Padding = new Padding(8, 2, 8, 2);
            _btnAnchorZoneCapture.Size = new Size(89, 35);
            _btnAnchorZoneCapture.TabIndex = 2;
            _btnAnchorZoneCapture.Text = "采集参考";
            _btnAnchorZoneCapture.UseVisualStyleBackColor = false;
            // 
            // _btnAnchorZoneEdit
            // 
            _btnAnchorZoneEdit.AutoSize = true;
            _btnAnchorZoneEdit.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnAnchorZoneEdit.BackColor = Color.White;
            _btnAnchorZoneEdit.Cursor = Cursors.Hand;
            _btnAnchorZoneEdit.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnAnchorZoneEdit.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnAnchorZoneEdit.FlatStyle = FlatStyle.Flat;
            _btnAnchorZoneEdit.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnAnchorZoneEdit.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnAnchorZoneEdit.Location = new Point(4, 47);
            _btnAnchorZoneEdit.Margin = new Padding(4);
            _btnAnchorZoneEdit.MinimumSize = new Size(84, 34);
            _btnAnchorZoneEdit.Name = "_btnAnchorZoneEdit";
            _btnAnchorZoneEdit.Padding = new Padding(8, 2, 8, 2);
            _btnAnchorZoneEdit.Size = new Size(89, 35);
            _btnAnchorZoneEdit.TabIndex = 3;
            _btnAnchorZoneEdit.Text = "调整范围";
            _btnAnchorZoneEdit.UseVisualStyleBackColor = false;
            // 
            // _btnAnchorZoneRename
            // 
            _btnAnchorZoneRename.AutoSize = true;
            _btnAnchorZoneRename.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnAnchorZoneRename.BackColor = Color.White;
            _btnAnchorZoneRename.Cursor = Cursors.Hand;
            _btnAnchorZoneRename.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnAnchorZoneRename.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnAnchorZoneRename.FlatStyle = FlatStyle.Flat;
            _btnAnchorZoneRename.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnAnchorZoneRename.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnAnchorZoneRename.Location = new Point(101, 47);
            _btnAnchorZoneRename.Margin = new Padding(4);
            _btnAnchorZoneRename.MinimumSize = new Size(84, 34);
            _btnAnchorZoneRename.Name = "_btnAnchorZoneRename";
            _btnAnchorZoneRename.Padding = new Padding(8, 2, 8, 2);
            _btnAnchorZoneRename.Size = new Size(84, 35);
            _btnAnchorZoneRename.TabIndex = 4;
            _btnAnchorZoneRename.Text = "改名";
            _btnAnchorZoneRename.UseVisualStyleBackColor = false;
            // 
            // _btnAnchorZoneRemove
            // 
            _btnAnchorZoneRemove.AutoSize = true;
            _btnAnchorZoneRemove.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnAnchorZoneRemove.BackColor = Color.White;
            _btnAnchorZoneRemove.Cursor = Cursors.Hand;
            _btnAnchorZoneRemove.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnAnchorZoneRemove.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnAnchorZoneRemove.FlatStyle = FlatStyle.Flat;
            _btnAnchorZoneRemove.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnAnchorZoneRemove.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnAnchorZoneRemove.Location = new Point(193, 47);
            _btnAnchorZoneRemove.Margin = new Padding(4);
            _btnAnchorZoneRemove.MinimumSize = new Size(84, 34);
            _btnAnchorZoneRemove.Name = "_btnAnchorZoneRemove";
            _btnAnchorZoneRemove.Padding = new Padding(8, 2, 8, 2);
            _btnAnchorZoneRemove.Size = new Size(84, 35);
            _btnAnchorZoneRemove.TabIndex = 5;
            _btnAnchorZoneRemove.Text = "删除";
            _btnAnchorZoneRemove.UseVisualStyleBackColor = false;
            // 
            // _lvAnchorZones
            // 
            _lvAnchorZones.BackColor = Color.White;
            _lvAnchorZones.BorderStyle = BorderStyle.FixedSingle;
            _lvAnchorZones.Columns.AddRange(new ColumnHeader[] { colAnchorZoneName, colAnchorZoneLabel, colAnchorZoneMatch, colAnchorZoneShift, colAnchorZoneState });
            _lvAnchorZones.Dock = DockStyle.Fill;
            _lvAnchorZones.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lvAnchorZones.FullRowSelect = true;
            _lvAnchorZones.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            _lvAnchorZones.Location = new Point(0, 88);
            _lvAnchorZones.Margin = new Padding(0, 0, 0, 6);
            _lvAnchorZones.MultiSelect = false;
            _lvAnchorZones.Name = "_lvAnchorZones";
            _lvAnchorZones.Size = new Size(367, 126);
            _lvAnchorZones.TabIndex = 1;
            _lvAnchorZones.UseCompatibleStateImageBehavior = false;
            _lvAnchorZones.View = View.Details;
            // 
            // colAnchorZoneName
            // 
            colAnchorZoneName.Name = "colAnchorZoneName";
            colAnchorZoneName.Text = "范围";
            colAnchorZoneName.Width = 98;
            // 
            // colAnchorZoneLabel
            // 
            colAnchorZoneLabel.Name = "colAnchorZoneLabel";
            colAnchorZoneLabel.Text = "标签";
            colAnchorZoneLabel.Width = 68;
            // 
            // colAnchorZoneMatch
            // 
            colAnchorZoneMatch.Name = "colAnchorZoneMatch";
            colAnchorZoneMatch.Text = "匹配率";
            colAnchorZoneMatch.Width = 62;
            // 
            // colAnchorZoneShift
            // 
            colAnchorZoneShift.Name = "colAnchorZoneShift";
            colAnchorZoneShift.Text = "容许";
            colAnchorZoneShift.Width = 52;
            // 
            // colAnchorZoneState
            // 
            colAnchorZoneState.Name = "colAnchorZoneState";
            colAnchorZoneState.Text = "状态";
            colAnchorZoneState.Width = 84;
            // 
            // _tlpAnchorZoneRef
            // 
            _tlpAnchorZoneRef.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _tlpAnchorZoneRef.ColumnCount = 2;
            _tlpAnchorZoneRef.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 104F));
            _tlpAnchorZoneRef.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            _tlpAnchorZoneRef.Controls.Add(_picZoneRef, 0, 0);
            _tlpAnchorZoneRef.Controls.Add(_lblZoneRefInfo, 1, 0);
            _tlpAnchorZoneRef.Dock = DockStyle.Fill;
            _tlpAnchorZoneRef.Location = new Point(0, 220);
            _tlpAnchorZoneRef.Margin = new Padding(0, 0, 0, 6);
            _tlpAnchorZoneRef.Name = "_tlpAnchorZoneRef";
            _tlpAnchorZoneRef.RowCount = 1;
            _tlpAnchorZoneRef.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            _tlpAnchorZoneRef.Size = new Size(367, 82);
            _tlpAnchorZoneRef.TabIndex = 2;
            // 
            // _picZoneRef
            // 
            _picZoneRef.BackColor = Color.White;
            _picZoneRef.BorderStyle = BorderStyle.FixedSingle;
            _picZoneRef.Dock = DockStyle.Fill;
            _picZoneRef.Location = new Point(0, 0);
            _picZoneRef.Margin = new Padding(0, 0, 8, 0);
            _picZoneRef.Name = "_picZoneRef";
            _picZoneRef.Size = new Size(96, 82);
            _picZoneRef.SizeMode = PictureBoxSizeMode.Zoom;
            _picZoneRef.TabIndex = 0;
            _picZoneRef.TabStop = false;
            // 
            // _lblZoneRefInfo
            // 
            _lblZoneRefInfo.Dock = DockStyle.Fill;
            _lblZoneRefInfo.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lblZoneRefInfo.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblZoneRefInfo.Location = new Point(104, 0);
            _lblZoneRefInfo.Margin = new Padding(0);
            _lblZoneRefInfo.Name = "_lblZoneRefInfo";
            _lblZoneRefInfo.Size = new Size(263, 82);
            _lblZoneRefInfo.TabIndex = 1;
            _lblZoneRefInfo.Text = "参考图：选中一个检测范围后显示";
            _lblZoneRefInfo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // _tlpAnchorZoneParams
            // 
            _tlpAnchorZoneParams.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _tlpAnchorZoneParams.ColumnCount = 2;
            _tlpAnchorZoneParams.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 96F));
            _tlpAnchorZoneParams.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            _tlpAnchorZoneParams.Controls.Add(_lblZoneMatchCap, 0, 0);
            _tlpAnchorZoneParams.Controls.Add(_numZoneMatch, 1, 0);
            _tlpAnchorZoneParams.Controls.Add(_lblZoneMatchTip, 0, 1);
            _tlpAnchorZoneParams.Controls.Add(_lblZoneSearchCap, 0, 2);
            _tlpAnchorZoneParams.Controls.Add(_numZoneSearch, 1, 2);
            _tlpAnchorZoneParams.Controls.Add(_lblZoneSearchTip, 0, 3);
            _tlpAnchorZoneParams.Controls.Add(_lblZoneShiftCap, 0, 4);
            _tlpAnchorZoneParams.Controls.Add(_numZoneShift, 1, 4);
            _tlpAnchorZoneParams.Controls.Add(_lblZoneShiftTip, 0, 5);
            _tlpAnchorZoneParams.Controls.Add(_lblZoneClassCap, 0, 6);
            _tlpAnchorZoneParams.Controls.Add(_cboZoneClass, 1, 6);
            _tlpAnchorZoneParams.Controls.Add(_lblZoneClassTip, 0, 7);
            _tlpAnchorZoneParams.Controls.Add(_chkZoneRequired, 1, 8);
            _tlpAnchorZoneParams.Dock = DockStyle.Fill;
            _tlpAnchorZoneParams.Location = new Point(0, 308);
            _tlpAnchorZoneParams.Margin = new Padding(0, 0, 0, 6);
            _tlpAnchorZoneParams.Name = "_tlpAnchorZoneParams";
            _tlpAnchorZoneParams.RowCount = 9;
            _tlpAnchorZoneParams.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
            _tlpAnchorZoneParams.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            _tlpAnchorZoneParams.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
            _tlpAnchorZoneParams.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            _tlpAnchorZoneParams.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
            _tlpAnchorZoneParams.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            _tlpAnchorZoneParams.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
            _tlpAnchorZoneParams.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            _tlpAnchorZoneParams.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            _tlpAnchorZoneParams.Size = new Size(367, 208);
            _tlpAnchorZoneParams.TabIndex = 3;
            // 
            // _lblZoneMatchCap
            // 
            _lblZoneMatchCap.AutoEllipsis = true;
            _lblZoneMatchCap.Dock = DockStyle.Fill;
            _lblZoneMatchCap.Font = new Font("Microsoft YaHei UI", 9.5F);
            _lblZoneMatchCap.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblZoneMatchCap.Location = new Point(0, 0);
            _lblZoneMatchCap.Margin = new Padding(0);
            _lblZoneMatchCap.Name = "_lblZoneMatchCap";
            _lblZoneMatchCap.Size = new Size(96, 26);
            _lblZoneMatchCap.TabIndex = 0;
            _lblZoneMatchCap.Text = "匹配率≥";
            _lblZoneMatchCap.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // _numZoneMatch
            // 
            _numZoneMatch.DecimalPlaces = 1;
            _numZoneMatch.Font = new Font("Microsoft YaHei UI", 9.5F);
            _numZoneMatch.Increment = new decimal(new int[] { 5, 0, 0, 65536 });
            _numZoneMatch.Location = new Point(96, 1);
            _numZoneMatch.Margin = new Padding(0, 1, 0, 1);
            _numZoneMatch.Maximum = new decimal(new int[] { 999, 0, 0, 65536 });
            _numZoneMatch.Minimum = new decimal(new int[] { 50, 0, 0, 0 });
            _numZoneMatch.Name = "_numZoneMatch";
            _numZoneMatch.Size = new Size(96, 24);
            _numZoneMatch.TabIndex = 1;
            _numZoneMatch.TextAlign = HorizontalAlignment.Right;
            _numZoneMatch.Value = new decimal(new int[] { 90, 0, 0, 0 });
            // 
            // _lblZoneMatchTip
            // 
            _lblZoneMatchTip.AutoEllipsis = true;
            _tlpAnchorZoneParams.SetColumnSpan(_lblZoneMatchTip, 2);
            _lblZoneMatchTip.Dock = DockStyle.Fill;
            _lblZoneMatchTip.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lblZoneMatchTip.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblZoneMatchTip.Location = new Point(0, 26);
            _lblZoneMatchTip.Margin = new Padding(0);
            _lblZoneMatchTip.Name = "_lblZoneMatchTip";
            _lblZoneMatchTip.Size = new Size(367, 20);
            _lblZoneMatchTip.TabIndex = 2;
            _lblZoneMatchTip.Text = "和参考图要多像才算通过：调高更严，调低更松。";
            // 
            // _lblZoneSearchCap
            // 
            _lblZoneSearchCap.AutoEllipsis = true;
            _lblZoneSearchCap.Dock = DockStyle.Fill;
            _lblZoneSearchCap.Font = new Font("Microsoft YaHei UI", 9.5F);
            _lblZoneSearchCap.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblZoneSearchCap.Location = new Point(0, 46);
            _lblZoneSearchCap.Margin = new Padding(0);
            _lblZoneSearchCap.Name = "_lblZoneSearchCap";
            _lblZoneSearchCap.Size = new Size(96, 26);
            _lblZoneSearchCap.TabIndex = 3;
            _lblZoneSearchCap.Text = "搜索范围";
            _lblZoneSearchCap.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // _numZoneSearch
            // 
            _numZoneSearch.Font = new Font("Microsoft YaHei UI", 9.5F);
            _numZoneSearch.Increment = new decimal(new int[] { 2, 0, 0, 0 });
            _numZoneSearch.Location = new Point(96, 47);
            _numZoneSearch.Margin = new Padding(0, 1, 0, 1);
            _numZoneSearch.Maximum = new decimal(new int[] { 160, 0, 0, 0 });
            _numZoneSearch.Name = "_numZoneSearch";
            _numZoneSearch.Size = new Size(96, 24);
            _numZoneSearch.TabIndex = 4;
            _numZoneSearch.TextAlign = HorizontalAlignment.Right;
            _numZoneSearch.Value = new decimal(new int[] { 24, 0, 0, 0 });
            // 
            // _lblZoneSearchTip
            // 
            _lblZoneSearchTip.AutoEllipsis = true;
            _tlpAnchorZoneParams.SetColumnSpan(_lblZoneSearchTip, 2);
            _lblZoneSearchTip.Dock = DockStyle.Fill;
            _lblZoneSearchTip.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lblZoneSearchTip.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblZoneSearchTip.Location = new Point(0, 72);
            _lblZoneSearchTip.Margin = new Padding(0);
            _lblZoneSearchTip.Name = "_lblZoneSearchTip";
            _lblZoneSearchTip.Size = new Size(367, 20);
            _lblZoneSearchTip.TabIndex = 5;
            _lblZoneSearchTip.Text = "在区域四周找参考的余量（px），太小易 NG。";
            // 
            // _lblZoneShiftCap
            // 
            _lblZoneShiftCap.AutoEllipsis = true;
            _lblZoneShiftCap.Dock = DockStyle.Fill;
            _lblZoneShiftCap.Font = new Font("Microsoft YaHei UI", 9.5F);
            _lblZoneShiftCap.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblZoneShiftCap.Location = new Point(0, 92);
            _lblZoneShiftCap.Margin = new Padding(0);
            _lblZoneShiftCap.Name = "_lblZoneShiftCap";
            _lblZoneShiftCap.Size = new Size(96, 26);
            _lblZoneShiftCap.TabIndex = 6;
            _lblZoneShiftCap.Text = "容许位移";
            _lblZoneShiftCap.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // _numZoneShift
            // 
            _numZoneShift.Font = new Font("Microsoft YaHei UI", 9.5F);
            _numZoneShift.Location = new Point(96, 93);
            _numZoneShift.Margin = new Padding(0, 1, 0, 1);
            _numZoneShift.Maximum = new decimal(new int[] { 120, 0, 0, 0 });
            _numZoneShift.Name = "_numZoneShift";
            _numZoneShift.Size = new Size(96, 24);
            _numZoneShift.TabIndex = 7;
            _numZoneShift.TextAlign = HorizontalAlignment.Right;
            _numZoneShift.Value = new decimal(new int[] { 12, 0, 0, 0 });
            // 
            // _lblZoneShiftTip
            // 
            _lblZoneShiftTip.AutoEllipsis = true;
            _tlpAnchorZoneParams.SetColumnSpan(_lblZoneShiftTip, 2);
            _lblZoneShiftTip.Dock = DockStyle.Fill;
            _lblZoneShiftTip.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lblZoneShiftTip.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblZoneShiftTip.Location = new Point(0, 118);
            _lblZoneShiftTip.Margin = new Padding(0);
            _lblZoneShiftTip.Name = "_lblZoneShiftTip";
            _lblZoneShiftTip.Size = new Size(367, 20);
            _lblZoneShiftTip.TabIndex = 8;
            _lblZoneShiftTip.Text = "找到的位置最多允许偏多少像素。";
            // 
            // _lblZoneClassCap
            // 
            _lblZoneClassCap.AutoEllipsis = true;
            _lblZoneClassCap.Dock = DockStyle.Fill;
            _lblZoneClassCap.Font = new Font("Microsoft YaHei UI", 9.5F);
            _lblZoneClassCap.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblZoneClassCap.Location = new Point(0, 138);
            _lblZoneClassCap.Margin = new Padding(0);
            _lblZoneClassCap.Name = "_lblZoneClassCap";
            _lblZoneClassCap.Size = new Size(96, 26);
            _lblZoneClassCap.TabIndex = 9;
            _lblZoneClassCap.Text = "绑定标签";
            _lblZoneClassCap.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // _cboZoneClass
            // 
            _cboZoneClass.Dock = DockStyle.Fill;
            _cboZoneClass.DropDownStyle = ComboBoxStyle.DropDownList;
            _cboZoneClass.Font = new Font("Microsoft YaHei UI", 9.5F);
            _cboZoneClass.Location = new Point(96, 139);
            _cboZoneClass.Margin = new Padding(0, 1, 0, 1);
            _cboZoneClass.Name = "_cboZoneClass";
            _cboZoneClass.Size = new Size(271, 27);
            _cboZoneClass.TabIndex = 10;
            // 
            // _lblZoneClassTip
            // 
            _lblZoneClassTip.AutoEllipsis = true;
            _tlpAnchorZoneParams.SetColumnSpan(_lblZoneClassTip, 2);
            _lblZoneClassTip.Dock = DockStyle.Fill;
            _lblZoneClassTip.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lblZoneClassTip.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblZoneClassTip.Location = new Point(0, 164);
            _lblZoneClassTip.Margin = new Padding(0);
            _lblZoneClassTip.Name = "_lblZoneClassTip";
            _lblZoneClassTip.Size = new Size(367, 20);
            _lblZoneClassTip.TabIndex = 11;
            _lblZoneClassTip.Text = "可选：这里必须检出某个目标（如 p1）。";
            // 
            // _chkZoneRequired
            // 
            _chkZoneRequired.AutoSize = true;
            _chkZoneRequired.Checked = true;
            _chkZoneRequired.CheckState = CheckState.Checked;
            _chkZoneRequired.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _chkZoneRequired.Location = new Point(99, 187);
            _chkZoneRequired.Name = "_chkZoneRequired";
            _chkZoneRequired.Size = new Size(196, 23);
            _chkZoneRequired.TabIndex = 12;
            _chkZoneRequired.Text = "必须通过（不通过即判 NG）";
            // 
            // _chkAnchorAdv
            // 
            _chkAnchorAdv.AutoSize = true;
            _chkAnchorAdv.Font = new Font("Microsoft YaHei UI", 9.5F);
            _chkAnchorAdv.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _chkAnchorAdv.Location = new Point(0, 526);
            _chkAnchorAdv.Margin = new Padding(0, 4, 0, 4);
            _chkAnchorAdv.Name = "_chkAnchorAdv";
            _chkAnchorAdv.Size = new Size(158, 23);
            _chkAnchorAdv.TabIndex = 4;
            _chkAnchorAdv.Text = "详细参数（判定容差）";
            // 
            // _pnlAnchorTol
            // 
            _pnlAnchorTol.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _pnlAnchorTol.Controls.Add(_tlpAnchorTol);
            _pnlAnchorTol.Dock = DockStyle.Fill;
            _pnlAnchorTol.Location = new Point(0, 554);
            _pnlAnchorTol.Margin = new Padding(0, 0, 0, 6);
            _pnlAnchorTol.Name = "_pnlAnchorTol";
            _pnlAnchorTol.Size = new Size(367, 1);
            _pnlAnchorTol.TabIndex = 5;
            _pnlAnchorTol.Visible = false;
            // 
            // _tlpAnchorTol
            // 
            _tlpAnchorTol.AutoSize = true;
            _tlpAnchorTol.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _tlpAnchorTol.BackColor = Color.White;
            _tlpAnchorTol.ColumnCount = 2;
            _tlpAnchorTol.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            _tlpAnchorTol.ColumnStyles.Add(new ColumnStyle());
            _tlpAnchorTol.Controls.Add(_lblAnchorTolShift, 0, 0);
            _tlpAnchorTol.Controls.Add(_numAnchorShift, 1, 0);
            _tlpAnchorTol.Controls.Add(_lblAnchorTolRot, 0, 1);
            _tlpAnchorTol.Controls.Add(_numAnchorRot, 1, 1);
            _tlpAnchorTol.Controls.Add(_lblAnchorTolScale, 0, 2);
            _tlpAnchorTol.Controls.Add(_numAnchorScale, 1, 2);
            _tlpAnchorTol.Controls.Add(_lblAnchorTolRms, 0, 3);
            _tlpAnchorTol.Controls.Add(_numAnchorRms, 1, 3);
            _tlpAnchorTol.Controls.Add(_lblAnchorTolConf, 0, 4);
            _tlpAnchorTol.Controls.Add(_numAnchorConf, 1, 4);
            _tlpAnchorTol.Controls.Add(_lblAnchorTolMin, 0, 5);
            _tlpAnchorTol.Controls.Add(_numAnchorMin, 1, 5);
            _tlpAnchorTol.Dock = DockStyle.Fill;
            _tlpAnchorTol.Location = new Point(0, 0);
            _tlpAnchorTol.Name = "_tlpAnchorTol";
            _tlpAnchorTol.RowCount = 6;
            _tlpAnchorTol.RowStyles.Add(new RowStyle());
            _tlpAnchorTol.RowStyles.Add(new RowStyle());
            _tlpAnchorTol.RowStyles.Add(new RowStyle());
            _tlpAnchorTol.RowStyles.Add(new RowStyle());
            _tlpAnchorTol.RowStyles.Add(new RowStyle());
            _tlpAnchorTol.RowStyles.Add(new RowStyle());
            _tlpAnchorTol.Size = new Size(367, 1);
            _tlpAnchorTol.TabIndex = 0;
            // 
            // _lblAnchorTolShift
            // 
            _lblAnchorTolShift.Anchor = AnchorStyles.Left;
            _lblAnchorTolShift.AutoSize = true;
            _lblAnchorTolShift.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lblAnchorTolShift.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblAnchorTolShift.Location = new Point(0, 7);
            _lblAnchorTolShift.Margin = new Padding(0, 6, 6, 2);
            _lblAnchorTolShift.Name = "_lblAnchorTolShift";
            _lblAnchorTolShift.Size = new Size(56, 17);
            _lblAnchorTolShift.TabIndex = 0;
            _lblAnchorTolShift.Text = "平移上限";
            // 
            // _numAnchorShift
            // 
            _numAnchorShift.Font = new Font("Microsoft YaHei UI", 9.5F);
            _numAnchorShift.Location = new Point(283, 2);
            _numAnchorShift.Margin = new Padding(0, 2, 0, 2);
            _numAnchorShift.Maximum = new decimal(new int[] { 200, 0, 0, 0 });
            _numAnchorShift.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            _numAnchorShift.Name = "_numAnchorShift";
            _numAnchorShift.Size = new Size(84, 24);
            _numAnchorShift.TabIndex = 1;
            _numAnchorShift.TextAlign = HorizontalAlignment.Right;
            _numAnchorShift.Value = new decimal(new int[] { 12, 0, 0, 0 });
            // 
            // _lblAnchorTolRot
            // 
            _lblAnchorTolRot.Anchor = AnchorStyles.Left;
            _lblAnchorTolRot.AutoSize = true;
            _lblAnchorTolRot.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lblAnchorTolRot.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblAnchorTolRot.Location = new Point(0, 35);
            _lblAnchorTolRot.Margin = new Padding(0, 6, 6, 2);
            _lblAnchorTolRot.Name = "_lblAnchorTolRot";
            _lblAnchorTolRot.Size = new Size(73, 17);
            _lblAnchorTolRot.TabIndex = 2;
            _lblAnchorTolRot.Text = "旋转上限 (°)";
            // 
            // _numAnchorRot
            // 
            _numAnchorRot.DecimalPlaces = 2;
            _numAnchorRot.Font = new Font("Microsoft YaHei UI", 9.5F);
            _numAnchorRot.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
            _numAnchorRot.Location = new Point(283, 30);
            _numAnchorRot.Margin = new Padding(0, 2, 0, 2);
            _numAnchorRot.Maximum = new decimal(new int[] { 30, 0, 0, 0 });
            _numAnchorRot.Minimum = new decimal(new int[] { 1, 0, 0, 65536 });
            _numAnchorRot.Name = "_numAnchorRot";
            _numAnchorRot.Size = new Size(84, 24);
            _numAnchorRot.TabIndex = 3;
            _numAnchorRot.TextAlign = HorizontalAlignment.Right;
            _numAnchorRot.Value = new decimal(new int[] { 8, 0, 0, 65536 });
            // 
            // _lblAnchorTolScale
            // 
            _lblAnchorTolScale.Anchor = AnchorStyles.Left;
            _lblAnchorTolScale.AutoSize = true;
            _lblAnchorTolScale.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lblAnchorTolScale.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblAnchorTolScale.Location = new Point(0, 63);
            _lblAnchorTolScale.Margin = new Padding(0, 6, 6, 2);
            _lblAnchorTolScale.Name = "_lblAnchorTolScale";
            _lblAnchorTolScale.Size = new Size(80, 17);
            _lblAnchorTolScale.TabIndex = 4;
            _lblAnchorTolScale.Text = "间距变化上限";
            // 
            // _numAnchorScale
            // 
            _numAnchorScale.DecimalPlaces = 3;
            _numAnchorScale.Font = new Font("Microsoft YaHei UI", 9.5F);
            _numAnchorScale.Increment = new decimal(new int[] { 5, 0, 0, 196608 });
            _numAnchorScale.Location = new Point(283, 58);
            _numAnchorScale.Margin = new Padding(0, 2, 0, 2);
            _numAnchorScale.Maximum = new decimal(new int[] { 2, 0, 0, 65536 });
            _numAnchorScale.Minimum = new decimal(new int[] { 5, 0, 0, 196608 });
            _numAnchorScale.Name = "_numAnchorScale";
            _numAnchorScale.Size = new Size(84, 24);
            _numAnchorScale.TabIndex = 5;
            _numAnchorScale.TextAlign = HorizontalAlignment.Right;
            _numAnchorScale.Value = new decimal(new int[] { 2, 0, 0, 131072 });
            // 
            // _lblAnchorTolRms
            // 
            _lblAnchorTolRms.Anchor = AnchorStyles.Left;
            _lblAnchorTolRms.AutoSize = true;
            _lblAnchorTolRms.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lblAnchorTolRms.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblAnchorTolRms.Location = new Point(0, 91);
            _lblAnchorTolRms.Margin = new Padding(0, 6, 6, 2);
            _lblAnchorTolRms.Name = "_lblAnchorTolRms";
            _lblAnchorTolRms.Size = new Size(87, 17);
            _lblAnchorTolRms.TabIndex = 6;
            _lblAnchorTolRms.Text = "多点残差 RMS";
            // 
            // _numAnchorRms
            // 
            _numAnchorRms.Font = new Font("Microsoft YaHei UI", 9.5F);
            _numAnchorRms.Location = new Point(283, 86);
            _numAnchorRms.Margin = new Padding(0, 2, 0, 2);
            _numAnchorRms.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            _numAnchorRms.Name = "_numAnchorRms";
            _numAnchorRms.Size = new Size(84, 24);
            _numAnchorRms.TabIndex = 7;
            _numAnchorRms.TextAlign = HorizontalAlignment.Right;
            _numAnchorRms.Value = new decimal(new int[] { 6, 0, 0, 0 });
            // 
            // _lblAnchorTolConf
            // 
            _lblAnchorTolConf.Anchor = AnchorStyles.Left;
            _lblAnchorTolConf.AutoSize = true;
            _lblAnchorTolConf.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lblAnchorTolConf.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblAnchorTolConf.Location = new Point(0, 119);
            _lblAnchorTolConf.Margin = new Padding(0, 6, 6, 2);
            _lblAnchorTolConf.Name = "_lblAnchorTolConf";
            _lblAnchorTolConf.Size = new Size(68, 17);
            _lblAnchorTolConf.TabIndex = 8;
            _lblAnchorTolConf.Text = "检出置信度";
            // 
            // _numAnchorConf
            // 
            _numAnchorConf.DecimalPlaces = 2;
            _numAnchorConf.Font = new Font("Microsoft YaHei UI", 9.5F);
            _numAnchorConf.Increment = new decimal(new int[] { 5, 0, 0, 131072 });
            _numAnchorConf.Location = new Point(283, 114);
            _numAnchorConf.Margin = new Padding(0, 2, 0, 2);
            _numAnchorConf.Maximum = new decimal(new int[] { 95, 0, 0, 131072 });
            _numAnchorConf.Minimum = new decimal(new int[] { 5, 0, 0, 131072 });
            _numAnchorConf.Name = "_numAnchorConf";
            _numAnchorConf.Size = new Size(84, 24);
            _numAnchorConf.TabIndex = 9;
            _numAnchorConf.TextAlign = HorizontalAlignment.Right;
            _numAnchorConf.Value = new decimal(new int[] { 35, 0, 0, 131072 });
            // 
            // _lblAnchorTolMin
            // 
            _lblAnchorTolMin.Anchor = AnchorStyles.Left;
            _lblAnchorTolMin.AutoSize = true;
            _lblAnchorTolMin.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lblAnchorTolMin.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblAnchorTolMin.Location = new Point(0, 147);
            _lblAnchorTolMin.Margin = new Padding(0, 6, 6, 2);
            _lblAnchorTolMin.Name = "_lblAnchorTolMin";
            _lblAnchorTolMin.Size = new Size(68, 17);
            _lblAnchorTolMin.TabIndex = 10;
            _lblAnchorTolMin.Text = "最少锚点数";
            // 
            // _numAnchorMin
            // 
            _numAnchorMin.Font = new Font("Microsoft YaHei UI", 9.5F);
            _numAnchorMin.Location = new Point(283, 142);
            _numAnchorMin.Margin = new Padding(0, 2, 0, 2);
            _numAnchorMin.Maximum = new decimal(new int[] { 6, 0, 0, 0 });
            _numAnchorMin.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            _numAnchorMin.Name = "_numAnchorMin";
            _numAnchorMin.Size = new Size(84, 24);
            _numAnchorMin.TabIndex = 11;
            _numAnchorMin.TextAlign = HorizontalAlignment.Right;
            _numAnchorMin.Value = new decimal(new int[] { 2, 0, 0, 0 });
            // 
            // _flowAnchorZoneJudge
            // 
            _flowAnchorZoneJudge.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _flowAnchorZoneJudge.Controls.Add(_btnAnchorZoneSaveTol);
            _flowAnchorZoneJudge.Controls.Add(_btnAnchorZoneOverlay);
            _flowAnchorZoneJudge.Dock = DockStyle.Fill;
            _flowAnchorZoneJudge.Location = new Point(0, 554);
            _flowAnchorZoneJudge.Margin = new Padding(0, 0, 0, 6);
            _flowAnchorZoneJudge.Name = "_flowAnchorZoneJudge";
            _flowAnchorZoneJudge.Size = new Size(367, 82);
            _flowAnchorZoneJudge.TabIndex = 6;
            // 
            // _btnAnchorZoneSaveTol
            // 
            _btnAnchorZoneSaveTol.AutoSize = true;
            _btnAnchorZoneSaveTol.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnAnchorZoneSaveTol.BackColor = Color.White;
            _btnAnchorZoneSaveTol.Cursor = Cursors.Hand;
            _btnAnchorZoneSaveTol.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnAnchorZoneSaveTol.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnAnchorZoneSaveTol.FlatStyle = FlatStyle.Flat;
            _btnAnchorZoneSaveTol.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnAnchorZoneSaveTol.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnAnchorZoneSaveTol.Location = new Point(4, 4);
            _btnAnchorZoneSaveTol.Margin = new Padding(4);
            _btnAnchorZoneSaveTol.MinimumSize = new Size(84, 34);
            _btnAnchorZoneSaveTol.Name = "_btnAnchorZoneSaveTol";
            _btnAnchorZoneSaveTol.Padding = new Padding(8, 2, 8, 2);
            _btnAnchorZoneSaveTol.Size = new Size(89, 35);
            _btnAnchorZoneSaveTol.TabIndex = 0;
            _btnAnchorZoneSaveTol.Text = "保存容差";
            _btnAnchorZoneSaveTol.UseVisualStyleBackColor = false;
            // 
            // _btnAnchorZoneOverlay
            // 
            _btnAnchorZoneOverlay.AutoSize = true;
            _btnAnchorZoneOverlay.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnAnchorZoneOverlay.BackColor = Color.White;
            _btnAnchorZoneOverlay.Cursor = Cursors.Hand;
            _btnAnchorZoneOverlay.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnAnchorZoneOverlay.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnAnchorZoneOverlay.FlatStyle = FlatStyle.Flat;
            _btnAnchorZoneOverlay.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnAnchorZoneOverlay.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnAnchorZoneOverlay.Location = new Point(101, 4);
            _btnAnchorZoneOverlay.Margin = new Padding(4);
            _btnAnchorZoneOverlay.MinimumSize = new Size(84, 34);
            _btnAnchorZoneOverlay.Name = "_btnAnchorZoneOverlay";
            _btnAnchorZoneOverlay.Padding = new Padding(8, 2, 8, 2);
            _btnAnchorZoneOverlay.Size = new Size(89, 35);
            _btnAnchorZoneOverlay.TabIndex = 1;
            _btnAnchorZoneOverlay.Text = "叠加：开";
            _btnAnchorZoneOverlay.UseVisualStyleBackColor = false;
            // 
            // _pnlAnchorHitsCard
            // 
            _pnlAnchorHitsCard.BackColor = Color.White;
            _pnlAnchorHitsCard.Controls.Add(_tlpAnchorHitsCard);
            _pnlAnchorHitsCard.Dock = DockStyle.Fill;
            _pnlAnchorHitsCard.Location = new Point(0, 642);
            _pnlAnchorHitsCard.Margin = new Padding(0, 0, 0, 8);
            _pnlAnchorHitsCard.Name = "_pnlAnchorHitsCard";
            _pnlAnchorHitsCard.Padding = new Padding(10, 8, 10, 8);
            _pnlAnchorHitsCard.Size = new Size(367, 152);
            _pnlAnchorHitsCard.TabIndex = 7;
            // 
            // _tlpAnchorHitsCard
            // 
            _tlpAnchorHitsCard.BackColor = Color.White;
            _tlpAnchorHitsCard.ColumnCount = 1;
            _tlpAnchorHitsCard.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            _tlpAnchorHitsCard.Controls.Add(_lblAnchorHitsTitle, 0, 0);
            _tlpAnchorHitsCard.Controls.Add(_lvAnchorHits, 0, 1);
            _tlpAnchorHitsCard.Dock = DockStyle.Fill;
            _tlpAnchorHitsCard.Location = new Point(10, 8);
            _tlpAnchorHitsCard.Margin = new Padding(0);
            _tlpAnchorHitsCard.Name = "_tlpAnchorHitsCard";
            _tlpAnchorHitsCard.RowCount = 2;
            _tlpAnchorHitsCard.RowStyles.Add(new RowStyle());
            _tlpAnchorHitsCard.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            _tlpAnchorHitsCard.Size = new Size(347, 136);
            _tlpAnchorHitsCard.TabIndex = 0;
            // 
            // _lblAnchorHitsTitle
            // 
            _lblAnchorHitsTitle.AutoSize = true;
            _lblAnchorHitsTitle.Font = new Font("Microsoft YaHei UI", 9.5F, FontStyle.Bold);
            _lblAnchorHitsTitle.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblAnchorHitsTitle.Location = new Point(0, 0);
            _lblAnchorHitsTitle.Margin = new Padding(0, 0, 0, 6);
            _lblAnchorHitsTitle.Name = "_lblAnchorHitsTitle";
            _lblAnchorHitsTitle.Size = new Size(165, 19);
            _lblAnchorHitsTitle.TabIndex = 0;
            _lblAnchorHitsTitle.Text = "锚点对照（拍基准后才有）";
            // 
            // _lvAnchorHits
            // 
            _lvAnchorHits.BackColor = Color.White;
            _lvAnchorHits.BorderStyle = BorderStyle.None;
            _lvAnchorHits.Columns.AddRange(new ColumnHeader[] { colAnchorHitName, colAnchorHitBase, colAnchorHitActual, colAnchorHitDelta, colAnchorHitConf });
            _lvAnchorHits.Dock = DockStyle.Fill;
            _lvAnchorHits.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lvAnchorHits.FullRowSelect = true;
            _lvAnchorHits.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            _lvAnchorHits.Location = new Point(0, 25);
            _lvAnchorHits.Margin = new Padding(0);
            _lvAnchorHits.MultiSelect = false;
            _lvAnchorHits.Name = "_lvAnchorHits";
            _lvAnchorHits.Size = new Size(347, 111);
            _lvAnchorHits.TabIndex = 1;
            _lvAnchorHits.UseCompatibleStateImageBehavior = false;
            _lvAnchorHits.View = View.Details;
            // 
            // colAnchorHitName
            // 
            colAnchorHitName.Name = "colAnchorHitName";
            colAnchorHitName.Text = "锚点";
            colAnchorHitName.Width = 66;
            // 
            // colAnchorHitBase
            // 
            colAnchorHitBase.Name = "colAnchorHitBase";
            colAnchorHitBase.Text = "基准";
            colAnchorHitBase.Width = 76;
            // 
            // colAnchorHitActual
            // 
            colAnchorHitActual.Name = "colAnchorHitActual";
            colAnchorHitActual.Text = "实测";
            colAnchorHitActual.Width = 76;
            // 
            // colAnchorHitDelta
            // 
            colAnchorHitDelta.Name = "colAnchorHitDelta";
            colAnchorHitDelta.Text = "偏差";
            colAnchorHitDelta.Width = 78;
            // 
            // colAnchorHitConf
            // 
            colAnchorHitConf.Name = "colAnchorHitConf";
            colAnchorHitConf.Text = "置信";
            colAnchorHitConf.Width = 50;
            // 
            // _pnlAnchorLogCard
            // 
            _pnlAnchorLogCard.BackColor = Color.White;
            _pnlAnchorLogCard.Controls.Add(_tlpAnchorLogCard);
            _pnlAnchorLogCard.Dock = DockStyle.Fill;
            _pnlAnchorLogCard.Location = new Point(0, 802);
            _pnlAnchorLogCard.Margin = new Padding(0, 0, 0, 8);
            _pnlAnchorLogCard.Name = "_pnlAnchorLogCard";
            _pnlAnchorLogCard.Padding = new Padding(10, 8, 10, 8);
            _pnlAnchorLogCard.Size = new Size(367, 104);
            _pnlAnchorLogCard.TabIndex = 8;
            // 
            // _tlpAnchorLogCard
            // 
            _tlpAnchorLogCard.BackColor = Color.White;
            _tlpAnchorLogCard.ColumnCount = 1;
            _tlpAnchorLogCard.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            _tlpAnchorLogCard.Controls.Add(_lblAnchorLogTitle, 0, 0);
            _tlpAnchorLogCard.Controls.Add(_lvAnchorLog, 0, 1);
            _tlpAnchorLogCard.Dock = DockStyle.Fill;
            _tlpAnchorLogCard.Location = new Point(10, 8);
            _tlpAnchorLogCard.Margin = new Padding(0);
            _tlpAnchorLogCard.Name = "_tlpAnchorLogCard";
            _tlpAnchorLogCard.RowCount = 2;
            _tlpAnchorLogCard.RowStyles.Add(new RowStyle());
            _tlpAnchorLogCard.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            _tlpAnchorLogCard.Size = new Size(347, 88);
            _tlpAnchorLogCard.TabIndex = 0;
            // 
            // _lblAnchorLogTitle
            // 
            _lblAnchorLogTitle.AutoSize = true;
            _lblAnchorLogTitle.Font = new Font("Microsoft YaHei UI", 9.5F, FontStyle.Bold);
            _lblAnchorLogTitle.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblAnchorLogTitle.Location = new Point(0, 0);
            _lblAnchorLogTitle.Margin = new Padding(0, 0, 0, 6);
            _lblAnchorLogTitle.Name = "_lblAnchorLogTitle";
            _lblAnchorLogTitle.Size = new Size(61, 19);
            _lblAnchorLogTitle.TabIndex = 0;
            _lblAnchorLogTitle.Text = "最近判定";
            // 
            // _lvAnchorLog
            // 
            _lvAnchorLog.BackColor = Color.White;
            _lvAnchorLog.BorderStyle = BorderStyle.None;
            _lvAnchorLog.Columns.AddRange(new ColumnHeader[] { colAnchorLogTime, colAnchorLogVerdict, colAnchorLogReason, colAnchorLogDelta });
            _lvAnchorLog.Dock = DockStyle.Fill;
            _lvAnchorLog.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lvAnchorLog.FullRowSelect = true;
            _lvAnchorLog.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            _lvAnchorLog.Location = new Point(0, 25);
            _lvAnchorLog.Margin = new Padding(0);
            _lvAnchorLog.MultiSelect = false;
            _lvAnchorLog.Name = "_lvAnchorLog";
            _lvAnchorLog.Size = new Size(347, 63);
            _lvAnchorLog.TabIndex = 1;
            _lvAnchorLog.UseCompatibleStateImageBehavior = false;
            _lvAnchorLog.View = View.Details;
            // 
            // colAnchorLogTime
            // 
            colAnchorLogTime.Name = "colAnchorLogTime";
            colAnchorLogTime.Text = "时间";
            colAnchorLogTime.Width = 66;
            // 
            // colAnchorLogVerdict
            // 
            colAnchorLogVerdict.Name = "colAnchorLogVerdict";
            colAnchorLogVerdict.Text = "结论";
            colAnchorLogVerdict.Width = 52;
            // 
            // colAnchorLogReason
            // 
            colAnchorLogReason.Name = "colAnchorLogReason";
            colAnchorLogReason.Text = "原因";
            colAnchorLogReason.Width = 108;
            // 
            // colAnchorLogDelta
            // 
            colAnchorLogDelta.Name = "colAnchorLogDelta";
            colAnchorLogDelta.Text = "偏差";
            colAnchorLogDelta.Width = 84;
            // 
            // _tlpAnchorZoneHints
            // 
            _tlpAnchorZoneHints.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _tlpAnchorZoneHints.ColumnCount = 1;
            _tlpAnchorZoneHints.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            _tlpAnchorZoneHints.Controls.Add(_lblAnchorZoneInfo, 0, 0);
            _tlpAnchorZoneHints.Controls.Add(_lblAnchorCalibInfo, 0, 1);
            _tlpAnchorZoneHints.Controls.Add(_lblAnchorJitter, 0, 2);
            _tlpAnchorZoneHints.Dock = DockStyle.Fill;
            _tlpAnchorZoneHints.Location = new Point(0, 914);
            _tlpAnchorZoneHints.Margin = new Padding(0);
            _tlpAnchorZoneHints.Name = "_tlpAnchorZoneHints";
            _tlpAnchorZoneHints.RowCount = 3;
            _tlpAnchorZoneHints.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            _tlpAnchorZoneHints.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
            _tlpAnchorZoneHints.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
            _tlpAnchorZoneHints.Size = new Size(367, 84);
            _tlpAnchorZoneHints.TabIndex = 9;
            // 
            // _lblAnchorZoneInfo
            // 
            _lblAnchorZoneInfo.Dock = DockStyle.Fill;
            _lblAnchorZoneInfo.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lblAnchorZoneInfo.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblAnchorZoneInfo.Location = new Point(0, 0);
            _lblAnchorZoneInfo.Margin = new Padding(0);
            _lblAnchorZoneInfo.Name = "_lblAnchorZoneInfo";
            _lblAnchorZoneInfo.Size = new Size(367, 30);
            _lblAnchorZoneInfo.TabIndex = 0;
            _lblAnchorZoneInfo.Text = "还没有检测范围";
            // 
            // _lblAnchorCalibInfo
            // 
            _lblAnchorCalibInfo.Dock = DockStyle.Fill;
            _lblAnchorCalibInfo.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lblAnchorCalibInfo.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblAnchorCalibInfo.Location = new Point(0, 30);
            _lblAnchorCalibInfo.Margin = new Padding(0);
            _lblAnchorCalibInfo.Name = "_lblAnchorCalibInfo";
            _lblAnchorCalibInfo.Size = new Size(367, 26);
            _lblAnchorCalibInfo.TabIndex = 1;
            _lblAnchorCalibInfo.Text = "基准：还没有拍";
            // 
            // _lblAnchorJitter
            // 
            _lblAnchorJitter.Dock = DockStyle.Fill;
            _lblAnchorJitter.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lblAnchorJitter.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblAnchorJitter.Location = new Point(0, 56);
            _lblAnchorJitter.Margin = new Padding(0);
            _lblAnchorJitter.Name = "_lblAnchorJitter";
            _lblAnchorJitter.Size = new Size(367, 28);
            _lblAnchorJitter.TabIndex = 2;
            // 
            // _tabAnchorCalib
            // 
            _tabAnchorCalib.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _tabAnchorCalib.Controls.Add(_tlpAnchorCalib);
            _tabAnchorCalib.Location = new Point(4, 28);
            _tabAnchorCalib.Name = "_tabAnchorCalib";
            _tabAnchorCalib.Padding = new Padding(4);
            _tabAnchorCalib.Size = new Size(392, 503);
            _tabAnchorCalib.TabIndex = 2;
            _tabAnchorCalib.Text = "③ 拍基准 · 对比";
            // 
            // _tlpAnchorCalib
            // 
            _tlpAnchorCalib.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _tlpAnchorCalib.ColumnCount = 1;
            _tlpAnchorCalib.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            _tlpAnchorCalib.Controls.Add(_lblAnchorCalibHint, 0, 0);
            _tlpAnchorCalib.Controls.Add(_flowAnchorCalibBar, 0, 1);
            _tlpAnchorCalib.Controls.Add(_lblAnchorCalibCompare, 0, 2);
            _tlpAnchorCalib.Controls.Add(_pnlAnchorCalibCard, 0, 3);
            _tlpAnchorCalib.Dock = DockStyle.Fill;
            _tlpAnchorCalib.Location = new Point(4, 4);
            _tlpAnchorCalib.Name = "_tlpAnchorCalib";
            _tlpAnchorCalib.RowCount = 4;
            _tlpAnchorCalib.RowStyles.Add(new RowStyle());
            _tlpAnchorCalib.RowStyles.Add(new RowStyle());
            _tlpAnchorCalib.RowStyles.Add(new RowStyle());
            _tlpAnchorCalib.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            _tlpAnchorCalib.Size = new Size(384, 495);
            _tlpAnchorCalib.TabIndex = 0;
            // 
            // _lblAnchorCalibHint
            // 
            _lblAnchorCalibHint.AutoSize = true;
            _lblAnchorCalibHint.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lblAnchorCalibHint.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblAnchorCalibHint.Location = new Point(0, 0);
            _lblAnchorCalibHint.Margin = new Padding(0, 0, 0, 6);
            _lblAnchorCalibHint.Name = "_lblAnchorCalibHint";
            _lblAnchorCalibHint.Size = new Size(382, 68);
            _lblAnchorCalibHint.TabIndex = 0;
            _lblAnchorCalibHint.Text = "① 放一块确认到位的板 → 点【拍基准】。\r\n② 换板后点【现状拍照对比】：绿框 = 基准位置，红圈 = 本次实测，黄线 = 偏差。\r\n③ 点【返回实时】回到相机实时预览。";
            // 
            // _flowAnchorCalibBar
            // 
            _flowAnchorCalibBar.AutoSize = true;
            _flowAnchorCalibBar.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _flowAnchorCalibBar.Controls.Add(_btnAnchorCalib);
            _flowAnchorCalibBar.Controls.Add(_btnAnchorCalibView);
            _flowAnchorCalibBar.Controls.Add(_btnAnchorCalibCompare);
            _flowAnchorCalibBar.Controls.Add(_btnAnchorCalibLive);
            _flowAnchorCalibBar.Dock = DockStyle.Fill;
            _flowAnchorCalibBar.Location = new Point(0, 74);
            _flowAnchorCalibBar.Margin = new Padding(0, 0, 0, 6);
            _flowAnchorCalibBar.Name = "_flowAnchorCalibBar";
            _flowAnchorCalibBar.Size = new Size(384, 78);
            _flowAnchorCalibBar.TabIndex = 1;
            // 
            // _btnAnchorCalib
            // 
            _btnAnchorCalib.AutoSize = true;
            _btnAnchorCalib.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnAnchorCalib.BackColor = Color.FromArgb((int)(byte)22, (int)(byte)138, (int)(byte)88);
            _btnAnchorCalib.Cursor = Cursors.Hand;
            _btnAnchorCalib.FlatAppearance.BorderSize = 0;
            _btnAnchorCalib.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)18, (int)(byte)118, (int)(byte)74);
            _btnAnchorCalib.FlatStyle = FlatStyle.Flat;
            _btnAnchorCalib.Font = new Font("Microsoft YaHei UI", 9.5F, FontStyle.Bold);
            _btnAnchorCalib.ForeColor = Color.White;
            _btnAnchorCalib.Location = new Point(0, 0);
            _btnAnchorCalib.Margin = new Padding(0, 0, 6, 0);
            _btnAnchorCalib.MinimumSize = new Size(84, 34);
            _btnAnchorCalib.Name = "_btnAnchorCalib";
            _btnAnchorCalib.Padding = new Padding(8, 2, 8, 2);
            _btnAnchorCalib.Size = new Size(84, 34);
            _btnAnchorCalib.TabIndex = 0;
            _btnAnchorCalib.Text = "拍基准";
            _btnAnchorCalib.UseVisualStyleBackColor = false;
            // 
            // _btnAnchorCalibView
            // 
            _btnAnchorCalibView.AutoSize = true;
            _btnAnchorCalibView.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnAnchorCalibView.BackColor = Color.White;
            _btnAnchorCalibView.Cursor = Cursors.Hand;
            _btnAnchorCalibView.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnAnchorCalibView.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnAnchorCalibView.FlatStyle = FlatStyle.Flat;
            _btnAnchorCalibView.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnAnchorCalibView.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnAnchorCalibView.Location = new Point(90, 0);
            _btnAnchorCalibView.Margin = new Padding(0, 0, 6, 0);
            _btnAnchorCalibView.MinimumSize = new Size(84, 34);
            _btnAnchorCalibView.Name = "_btnAnchorCalibView";
            _btnAnchorCalibView.Padding = new Padding(8, 2, 8, 2);
            _btnAnchorCalibView.Size = new Size(115, 35);
            _btnAnchorCalibView.TabIndex = 1;
            _btnAnchorCalibView.Text = "查看当前基准";
            _btnAnchorCalibView.UseVisualStyleBackColor = false;
            // 
            // _btnAnchorCalibCompare
            // 
            _btnAnchorCalibCompare.AutoSize = true;
            _btnAnchorCalibCompare.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnAnchorCalibCompare.BackColor = Color.White;
            _btnAnchorCalibCompare.Cursor = Cursors.Hand;
            _btnAnchorCalibCompare.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnAnchorCalibCompare.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnAnchorCalibCompare.FlatStyle = FlatStyle.Flat;
            _btnAnchorCalibCompare.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnAnchorCalibCompare.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnAnchorCalibCompare.Location = new Point(211, 0);
            _btnAnchorCalibCompare.Margin = new Padding(0, 0, 6, 0);
            _btnAnchorCalibCompare.MinimumSize = new Size(84, 34);
            _btnAnchorCalibCompare.Name = "_btnAnchorCalibCompare";
            _btnAnchorCalibCompare.Padding = new Padding(8, 2, 8, 2);
            _btnAnchorCalibCompare.Size = new Size(115, 35);
            _btnAnchorCalibCompare.TabIndex = 2;
            _btnAnchorCalibCompare.Text = "现状拍照对比";
            _btnAnchorCalibCompare.UseVisualStyleBackColor = false;
            // 
            // _btnAnchorCalibLive
            // 
            _btnAnchorCalibLive.AutoSize = true;
            _btnAnchorCalibLive.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnAnchorCalibLive.BackColor = Color.White;
            _btnAnchorCalibLive.Cursor = Cursors.Hand;
            _btnAnchorCalibLive.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnAnchorCalibLive.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnAnchorCalibLive.FlatStyle = FlatStyle.Flat;
            _btnAnchorCalibLive.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnAnchorCalibLive.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnAnchorCalibLive.Location = new Point(4, 39);
            _btnAnchorCalibLive.Margin = new Padding(4);
            _btnAnchorCalibLive.MinimumSize = new Size(84, 34);
            _btnAnchorCalibLive.Name = "_btnAnchorCalibLive";
            _btnAnchorCalibLive.Padding = new Padding(8, 2, 8, 2);
            _btnAnchorCalibLive.Size = new Size(89, 35);
            _btnAnchorCalibLive.TabIndex = 3;
            _btnAnchorCalibLive.Text = "返回实时";
            _btnAnchorCalibLive.UseVisualStyleBackColor = false;
            // 
            // _lblAnchorCalibCompare
            // 
            _lblAnchorCalibCompare.AutoSize = true;
            _lblAnchorCalibCompare.Font = new Font("Microsoft YaHei UI", 9.5F);
            _lblAnchorCalibCompare.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblAnchorCalibCompare.Location = new Point(0, 158);
            _lblAnchorCalibCompare.Margin = new Padding(0, 0, 0, 6);
            _lblAnchorCalibCompare.Name = "_lblAnchorCalibCompare";
            _lblAnchorCalibCompare.Size = new Size(282, 19);
            _lblAnchorCalibCompare.TabIndex = 2;
            _lblAnchorCalibCompare.Text = "还没有对比：拍完基准后点【现状拍照对比】。";
            // 
            // _pnlAnchorCalibCard
            // 
            _pnlAnchorCalibCard.BackColor = Color.White;
            _pnlAnchorCalibCard.Controls.Add(_tlpAnchorCalibCard);
            _pnlAnchorCalibCard.Dock = DockStyle.Fill;
            _pnlAnchorCalibCard.Location = new Point(0, 183);
            _pnlAnchorCalibCard.Margin = new Padding(0, 0, 0, 8);
            _pnlAnchorCalibCard.Name = "_pnlAnchorCalibCard";
            _pnlAnchorCalibCard.Padding = new Padding(10, 8, 10, 8);
            _pnlAnchorCalibCard.Size = new Size(384, 304);
            _pnlAnchorCalibCard.TabIndex = 3;
            // 
            // _tlpAnchorCalibCard
            // 
            _tlpAnchorCalibCard.BackColor = Color.White;
            _tlpAnchorCalibCard.ColumnCount = 1;
            _tlpAnchorCalibCard.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            _tlpAnchorCalibCard.Controls.Add(_lblAnchorCalibHitsTitle, 0, 0);
            _tlpAnchorCalibCard.Controls.Add(_lvAnchorCalibHits, 0, 1);
            _tlpAnchorCalibCard.Dock = DockStyle.Fill;
            _tlpAnchorCalibCard.Location = new Point(10, 8);
            _tlpAnchorCalibCard.Margin = new Padding(0);
            _tlpAnchorCalibCard.Name = "_tlpAnchorCalibCard";
            _tlpAnchorCalibCard.RowCount = 2;
            _tlpAnchorCalibCard.RowStyles.Add(new RowStyle());
            _tlpAnchorCalibCard.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            _tlpAnchorCalibCard.Size = new Size(364, 288);
            _tlpAnchorCalibCard.TabIndex = 0;
            // 
            // _lblAnchorCalibHitsTitle
            // 
            _lblAnchorCalibHitsTitle.AutoSize = true;
            _lblAnchorCalibHitsTitle.Font = new Font("Microsoft YaHei UI", 9.5F, FontStyle.Bold);
            _lblAnchorCalibHitsTitle.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblAnchorCalibHitsTitle.Location = new Point(0, 0);
            _lblAnchorCalibHitsTitle.Margin = new Padding(0, 0, 0, 6);
            _lblAnchorCalibHitsTitle.Name = "_lblAnchorCalibHitsTitle";
            _lblAnchorCalibHitsTitle.Size = new Size(101, 19);
            _lblAnchorCalibHitsTitle.TabIndex = 0;
            _lblAnchorCalibHitsTitle.Text = "基准 / 实测对照";
            // 
            // _lvAnchorCalibHits
            // 
            _lvAnchorCalibHits.BackColor = Color.White;
            _lvAnchorCalibHits.BorderStyle = BorderStyle.None;
            _lvAnchorCalibHits.Columns.AddRange(new ColumnHeader[] { colAnchorCalibName, colAnchorCalibBase, colAnchorCalibActual, colAnchorCalibDx, colAnchorCalibDy, colAnchorCalibConf });
            _lvAnchorCalibHits.Dock = DockStyle.Fill;
            _lvAnchorCalibHits.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lvAnchorCalibHits.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _lvAnchorCalibHits.FullRowSelect = true;
            _lvAnchorCalibHits.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            _lvAnchorCalibHits.Location = new Point(0, 25);
            _lvAnchorCalibHits.Margin = new Padding(0);
            _lvAnchorCalibHits.MultiSelect = false;
            _lvAnchorCalibHits.Name = "_lvAnchorCalibHits";
            _lvAnchorCalibHits.Size = new Size(364, 263);
            _lvAnchorCalibHits.TabIndex = 1;
            _lvAnchorCalibHits.UseCompatibleStateImageBehavior = false;
            _lvAnchorCalibHits.View = View.Details;
            // 
            // colAnchorCalibName
            // 
            colAnchorCalibName.Name = "colAnchorCalibName";
            colAnchorCalibName.Text = "锚点";
            colAnchorCalibName.Width = 76;
            // 
            // colAnchorCalibBase
            // 
            colAnchorCalibBase.Name = "colAnchorCalibBase";
            colAnchorCalibBase.Text = "基准";
            colAnchorCalibBase.Width = 104;
            // 
            // colAnchorCalibActual
            // 
            colAnchorCalibActual.Name = "colAnchorCalibActual";
            colAnchorCalibActual.Text = "实测";
            colAnchorCalibActual.Width = 104;
            // 
            // colAnchorCalibDx
            // 
            colAnchorCalibDx.Name = "colAnchorCalibDx";
            colAnchorCalibDx.Text = "Δx";
            colAnchorCalibDx.Width = 64;
            // 
            // colAnchorCalibDy
            // 
            colAnchorCalibDy.Name = "colAnchorCalibDy";
            colAnchorCalibDy.Text = "Δy";
            colAnchorCalibDy.Width = 64;
            // 
            // colAnchorCalibConf
            // 
            colAnchorCalibConf.Name = "colAnchorCalibConf";
            colAnchorCalibConf.Text = "置信";
            colAnchorCalibConf.Width = 56;
            // 
            // _pageMultiCam
            // 
            _pageMultiCam.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _pageMultiCam.Controls.Add(_tlpMultiCam);
            _pageMultiCam.Location = new Point(4, 34);
            _pageMultiCam.Name = "_pageMultiCam";
            _pageMultiCam.Padding = new Padding(12, 10, 12, 10);
            _pageMultiCam.Size = new Size(1256, 669);
            _pageMultiCam.TabIndex = 5;
            _pageMultiCam.Text = "多相机 · 资源";
            // 
            // _tlpMultiCam
            // 
            _tlpMultiCam.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _tlpMultiCam.ColumnCount = 2;
            _tlpMultiCam.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            _tlpMultiCam.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 360F));
            _tlpMultiCam.Controls.Add(_mcWall, 0, 0);
            _tlpMultiCam.Controls.Add(_tlpMcSide, 1, 0);
            _tlpMultiCam.Dock = DockStyle.Fill;
            _tlpMultiCam.Location = new Point(12, 10);
            _tlpMultiCam.Name = "_tlpMultiCam";
            _tlpMultiCam.RowCount = 1;
            _tlpMultiCam.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            _tlpMultiCam.Size = new Size(1232, 649);
            _tlpMultiCam.TabIndex = 0;
            // 
            // _mcWall
            // 
            _mcWall.AutoScroll = true;
            _mcWall.BackColor = Color.FromArgb((int)(byte)24, (int)(byte)27, (int)(byte)33);
            _mcWall.Dock = DockStyle.Fill;
            _mcWall.Location = new Point(0, 0);
            _mcWall.Margin = new Padding(0);
            _mcWall.Name = "_mcWall";
            _mcWall.Padding = new Padding(6);
            _mcWall.Size = new Size(872, 649);
            _mcWall.TabIndex = 0;
            // 
            // _tlpMcSide
            // 
            _tlpMcSide.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _tlpMcSide.ColumnCount = 1;
            _tlpMcSide.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            _tlpMcSide.Controls.Add(_lblMcTitle, 0, 0);
            _tlpMcSide.Controls.Add(_mcGrid, 0, 1);
            _tlpMcSide.Controls.Add(_flowMcOpen, 0, 2);
            _tlpMcSide.Controls.Add(_flowMcAll, 0, 3);
            _tlpMcSide.Controls.Add(_flowMcIndex, 0, 4);
            _tlpMcSide.Controls.Add(_mcSelInfo, 0, 5);
            _tlpMcSide.Controls.Add(_pnlMcSettings, 0, 6);
            _tlpMcSide.Controls.Add(_lblMcProcTitle, 0, 7);
            _tlpMcSide.Controls.Add(_pnlMcBottom, 0, 8);
            _tlpMcSide.Dock = DockStyle.Fill;
            _tlpMcSide.Location = new Point(872, 0);
            _tlpMcSide.Margin = new Padding(0);
            _tlpMcSide.Name = "_tlpMcSide";
            _tlpMcSide.Padding = new Padding(10, 6, 4, 6);
            _tlpMcSide.RowCount = 9;
            _tlpMcSide.RowStyles.Add(new RowStyle());
            _tlpMcSide.RowStyles.Add(new RowStyle(SizeType.Percent, 26F));
            _tlpMcSide.RowStyles.Add(new RowStyle());
            _tlpMcSide.RowStyles.Add(new RowStyle());
            _tlpMcSide.RowStyles.Add(new RowStyle());
            _tlpMcSide.RowStyles.Add(new RowStyle());
            _tlpMcSide.RowStyles.Add(new RowStyle(SizeType.Percent, 46F));
            _tlpMcSide.RowStyles.Add(new RowStyle());
            _tlpMcSide.RowStyles.Add(new RowStyle(SizeType.Percent, 28F));
            _tlpMcSide.Size = new Size(360, 649);
            _tlpMcSide.TabIndex = 1;
            // 
            // _lblMcTitle
            // 
            _lblMcTitle.AutoSize = true;
            _lblMcTitle.Font = new Font("Microsoft YaHei UI", 10.5F, FontStyle.Bold);
            _lblMcTitle.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _lblMcTitle.Location = new Point(10, 6);
            _lblMcTitle.Margin = new Padding(0, 0, 0, 4);
            _lblMcTitle.Name = "_lblMcTitle";
            _lblMcTitle.Size = new Size(37, 19);
            _lblMcTitle.TabIndex = 0;
            _lblMcTitle.Text = "相机";
            // 
            // _mcGrid
            // 
            _mcGrid.Columns.AddRange(new DataGridViewColumn[] { dataGridViewTextBoxColumn13, dataGridViewTextBoxColumn14, dataGridViewTextBoxColumn15, dataGridViewTextBoxColumn16, dataGridViewTextBoxColumn17 });
            _mcGrid.Dock = DockStyle.Fill;
            _mcGrid.Location = new Point(10, 29);
            _mcGrid.Margin = new Padding(0, 0, 0, 6);
            _mcGrid.Name = "_mcGrid";
            _mcGrid.Size = new Size(346, 106);
            _mcGrid.TabIndex = 1;
            // 
            // dataGridViewTextBoxColumn13
            // 
            dataGridViewTextBoxColumn13.Name = "dataGridViewTextBoxColumn13";
            // 
            // dataGridViewTextBoxColumn14
            // 
            dataGridViewTextBoxColumn14.Name = "dataGridViewTextBoxColumn14";
            // 
            // dataGridViewTextBoxColumn15
            // 
            dataGridViewTextBoxColumn15.Name = "dataGridViewTextBoxColumn15";
            // 
            // dataGridViewTextBoxColumn16
            // 
            dataGridViewTextBoxColumn16.Name = "dataGridViewTextBoxColumn16";
            // 
            // dataGridViewTextBoxColumn17
            // 
            dataGridViewTextBoxColumn17.Name = "dataGridViewTextBoxColumn17";
            // 
            // _flowMcOpen
            // 
            _flowMcOpen.AutoSize = true;
            _flowMcOpen.Controls.Add(_btnMcOpen);
            _flowMcOpen.Controls.Add(_btnMcClose);
            _flowMcOpen.Dock = DockStyle.Top;
            _flowMcOpen.Location = new Point(10, 141);
            _flowMcOpen.Margin = new Padding(0, 0, 0, 2);
            _flowMcOpen.Name = "_flowMcOpen";
            _flowMcOpen.Size = new Size(346, 42);
            _flowMcOpen.TabIndex = 2;
            // 
            // _btnMcOpen
            // 
            _btnMcOpen.AutoSize = true;
            _btnMcOpen.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnMcOpen.BackColor = Color.FromArgb((int)(byte)36, (int)(byte)112, (int)(byte)210);
            _btnMcOpen.Cursor = Cursors.Hand;
            _btnMcOpen.FlatAppearance.BorderSize = 0;
            _btnMcOpen.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)28, (int)(byte)96, (int)(byte)186);
            _btnMcOpen.FlatStyle = FlatStyle.Flat;
            _btnMcOpen.Font = new Font("Microsoft YaHei UI", 9.5F, FontStyle.Bold);
            _btnMcOpen.ForeColor = Color.White;
            _btnMcOpen.Location = new Point(4, 4);
            _btnMcOpen.Margin = new Padding(4);
            _btnMcOpen.MinimumSize = new Size(84, 34);
            _btnMcOpen.Name = "_btnMcOpen";
            _btnMcOpen.Padding = new Padding(10, 0, 10, 0);
            _btnMcOpen.Size = new Size(91, 34);
            _btnMcOpen.TabIndex = 0;
            _btnMcOpen.Text = "打开选中";
            _btnMcOpen.UseVisualStyleBackColor = false;
            // 
            // _btnMcClose
            // 
            _btnMcClose.AutoSize = true;
            _btnMcClose.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnMcClose.BackColor = Color.White;
            _btnMcClose.Cursor = Cursors.Hand;
            _btnMcClose.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnMcClose.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnMcClose.FlatStyle = FlatStyle.Flat;
            _btnMcClose.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnMcClose.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnMcClose.Location = new Point(103, 4);
            _btnMcClose.Margin = new Padding(4);
            _btnMcClose.MinimumSize = new Size(84, 34);
            _btnMcClose.Name = "_btnMcClose";
            _btnMcClose.Padding = new Padding(10, 0, 10, 0);
            _btnMcClose.Size = new Size(93, 34);
            _btnMcClose.TabIndex = 1;
            _btnMcClose.Text = "关闭选中";
            _btnMcClose.UseVisualStyleBackColor = false;
            // 
            // _flowMcAll
            // 
            _flowMcAll.AutoSize = true;
            _flowMcAll.Controls.Add(_btnMcAllOn);
            _flowMcAll.Controls.Add(_btnMcAllOff);
            _flowMcAll.Dock = DockStyle.Top;
            _flowMcAll.Location = new Point(10, 185);
            _flowMcAll.Margin = new Padding(0, 0, 0, 2);
            _flowMcAll.Name = "_flowMcAll";
            _flowMcAll.Size = new Size(346, 42);
            _flowMcAll.TabIndex = 3;
            // 
            // _btnMcAllOn
            // 
            _btnMcAllOn.AutoSize = true;
            _btnMcAllOn.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnMcAllOn.BackColor = Color.FromArgb((int)(byte)22, (int)(byte)138, (int)(byte)88);
            _btnMcAllOn.Cursor = Cursors.Hand;
            _btnMcAllOn.FlatAppearance.BorderSize = 0;
            _btnMcAllOn.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)18, (int)(byte)118, (int)(byte)74);
            _btnMcAllOn.FlatStyle = FlatStyle.Flat;
            _btnMcAllOn.Font = new Font("Microsoft YaHei UI", 9.5F, FontStyle.Bold);
            _btnMcAllOn.ForeColor = Color.White;
            _btnMcAllOn.Location = new Point(4, 4);
            _btnMcAllOn.Margin = new Padding(4);
            _btnMcAllOn.MinimumSize = new Size(84, 34);
            _btnMcAllOn.Name = "_btnMcAllOn";
            _btnMcAllOn.Padding = new Padding(10, 0, 10, 0);
            _btnMcAllOn.Size = new Size(91, 34);
            _btnMcAllOn.TabIndex = 0;
            _btnMcAllOn.Text = "全部打开";
            _btnMcAllOn.UseVisualStyleBackColor = false;
            // 
            // _btnMcAllOff
            // 
            _btnMcAllOff.AutoSize = true;
            _btnMcAllOff.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnMcAllOff.BackColor = Color.White;
            _btnMcAllOff.Cursor = Cursors.Hand;
            _btnMcAllOff.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnMcAllOff.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnMcAllOff.FlatStyle = FlatStyle.Flat;
            _btnMcAllOff.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnMcAllOff.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnMcAllOff.Location = new Point(103, 4);
            _btnMcAllOff.Margin = new Padding(4);
            _btnMcAllOff.MinimumSize = new Size(84, 34);
            _btnMcAllOff.Name = "_btnMcAllOff";
            _btnMcAllOff.Padding = new Padding(10, 0, 10, 0);
            _btnMcAllOff.Size = new Size(93, 34);
            _btnMcAllOff.TabIndex = 1;
            _btnMcAllOff.Text = "全部关闭";
            _btnMcAllOff.UseVisualStyleBackColor = false;
            // 
            // _flowMcIndex
            // 
            _flowMcIndex.AutoSize = true;
            _flowMcIndex.Controls.Add(_lblMcIndexHint);
            _flowMcIndex.Controls.Add(_mcNumIndex);
            _flowMcIndex.Controls.Add(_btnMcAdd);
            _flowMcIndex.Controls.Add(_btnMcRemove);
            _flowMcIndex.Dock = DockStyle.Top;
            _flowMcIndex.Location = new Point(10, 229);
            _flowMcIndex.Margin = new Padding(0, 0, 0, 2);
            _flowMcIndex.Name = "_flowMcIndex";
            _flowMcIndex.Size = new Size(346, 42);
            _flowMcIndex.TabIndex = 4;
            // 
            // _lblMcIndexHint
            // 
            _lblMcIndexHint.AutoSize = true;
            _lblMcIndexHint.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lblMcIndexHint.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblMcIndexHint.Location = new Point(0, 9);
            _lblMcIndexHint.Margin = new Padding(0, 9, 4, 0);
            _lblMcIndexHint.Name = "_lblMcIndexHint";
            _lblMcIndexHint.Size = new Size(39, 17);
            _lblMcIndexHint.TabIndex = 0;
            _lblMcIndexHint.Text = "index";
            // 
            // _mcNumIndex
            // 
            _mcNumIndex.Font = new Font("Microsoft YaHei UI", 9.5F);
            _mcNumIndex.Location = new Point(43, 6);
            _mcNumIndex.Margin = new Padding(0, 6, 6, 0);
            _mcNumIndex.Maximum = new decimal(new int[] { 15, 0, 0, 0 });
            _mcNumIndex.Name = "_mcNumIndex";
            _mcNumIndex.Size = new Size(54, 24);
            _mcNumIndex.TabIndex = 1;
            // 
            // _btnMcAdd
            // 
            _btnMcAdd.AutoSize = true;
            _btnMcAdd.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnMcAdd.BackColor = Color.White;
            _btnMcAdd.Cursor = Cursors.Hand;
            _btnMcAdd.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnMcAdd.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnMcAdd.FlatStyle = FlatStyle.Flat;
            _btnMcAdd.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnMcAdd.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnMcAdd.Location = new Point(107, 4);
            _btnMcAdd.Margin = new Padding(4);
            _btnMcAdd.MinimumSize = new Size(84, 34);
            _btnMcAdd.Name = "_btnMcAdd";
            _btnMcAdd.Padding = new Padding(10, 0, 10, 0);
            _btnMcAdd.Size = new Size(84, 34);
            _btnMcAdd.TabIndex = 2;
            _btnMcAdd.Text = "＋ 添加";
            _btnMcAdd.UseVisualStyleBackColor = false;
            // 
            // _btnMcRemove
            // 
            _btnMcRemove.AutoSize = true;
            _btnMcRemove.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnMcRemove.BackColor = Color.White;
            _btnMcRemove.Cursor = Cursors.Hand;
            _btnMcRemove.FlatAppearance.BorderColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _btnMcRemove.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)238, (int)(byte)242, (int)(byte)248);
            _btnMcRemove.FlatStyle = FlatStyle.Flat;
            _btnMcRemove.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnMcRemove.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _btnMcRemove.Location = new Point(199, 4);
            _btnMcRemove.Margin = new Padding(4);
            _btnMcRemove.MinimumSize = new Size(84, 34);
            _btnMcRemove.Name = "_btnMcRemove";
            _btnMcRemove.Padding = new Padding(10, 0, 10, 0);
            _btnMcRemove.Size = new Size(84, 34);
            _btnMcRemove.TabIndex = 3;
            _btnMcRemove.Text = "－ 移除";
            _btnMcRemove.UseVisualStyleBackColor = false;
            // 
            // _mcSelInfo
            // 
            _mcSelInfo.AutoSize = true;
            _mcSelInfo.Font = new Font("Microsoft YaHei UI", 8.5F);
            _mcSelInfo.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _mcSelInfo.Location = new Point(10, 275);
            _mcSelInfo.Margin = new Padding(0, 2, 0, 4);
            _mcSelInfo.Name = "_mcSelInfo";
            _mcSelInfo.Size = new Size(0, 17);
            _mcSelInfo.TabIndex = 5;
            // 
            // _pnlMcSettings
            // 
            _pnlMcSettings.AutoScroll = true;
            _pnlMcSettings.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _pnlMcSettings.Controls.Add(_tlpMcSettings);
            _pnlMcSettings.Dock = DockStyle.Fill;
            _pnlMcSettings.Location = new Point(10, 298);
            _pnlMcSettings.Margin = new Padding(0, 2, 0, 2);
            _pnlMcSettings.Name = "_pnlMcSettings";
            _pnlMcSettings.Size = new Size(346, 195);
            _pnlMcSettings.TabIndex = 6;
            // 
            // _tlpMcSettings
            // 
            _tlpMcSettings.AutoSize = true;
            _tlpMcSettings.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _tlpMcSettings.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _tlpMcSettings.ColumnCount = 2;
            _tlpMcSettings.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 58F));
            _tlpMcSettings.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            _tlpMcSettings.Controls.Add(_lblMcSettingsTitle, 0, 0);
            _tlpMcSettings.Controls.Add(_lblMcRes, 0, 1);
            _tlpMcSettings.Controls.Add(_flowMcRes, 1, 1);
            _tlpMcSettings.Controls.Add(_lblMcFps, 0, 2);
            _tlpMcSettings.Controls.Add(_flowMcFps, 1, 2);
            _tlpMcSettings.Controls.Add(_lblMcExp, 0, 3);
            _tlpMcSettings.Controls.Add(_flowMcExp, 1, 3);
            _tlpMcSettings.Controls.Add(_lblMcWb, 0, 4);
            _tlpMcSettings.Controls.Add(_flowMcWb, 1, 4);
            _tlpMcSettings.Controls.Add(_lblMcGain, 0, 5);
            _tlpMcSettings.Controls.Add(_mcNumGain, 1, 5);
            _tlpMcSettings.Controls.Add(_lblMcBright, 0, 6);
            _tlpMcSettings.Controls.Add(_mcNumBright, 1, 6);
            _tlpMcSettings.Controls.Add(_lblMcContrast, 0, 7);
            _tlpMcSettings.Controls.Add(_mcNumContrast, 1, 7);
            _tlpMcSettings.Controls.Add(_lblMcSat, 0, 8);
            _tlpMcSettings.Controls.Add(_mcNumSat, 1, 8);
            _tlpMcSettings.Controls.Add(_flowMcApply, 0, 9);
            _tlpMcSettings.Controls.Add(_mcLblActual, 0, 10);
            _tlpMcSettings.Controls.Add(_lblMcSettingsNote, 0, 11);
            _tlpMcSettings.Dock = DockStyle.Top;
            _tlpMcSettings.Location = new Point(0, 0);
            _tlpMcSettings.Margin = new Padding(0);
            _tlpMcSettings.Name = "_tlpMcSettings";
            _tlpMcSettings.RowCount = 12;
            _tlpMcSettings.RowStyles.Add(new RowStyle());
            _tlpMcSettings.RowStyles.Add(new RowStyle());
            _tlpMcSettings.RowStyles.Add(new RowStyle());
            _tlpMcSettings.RowStyles.Add(new RowStyle());
            _tlpMcSettings.RowStyles.Add(new RowStyle());
            _tlpMcSettings.RowStyles.Add(new RowStyle());
            _tlpMcSettings.RowStyles.Add(new RowStyle());
            _tlpMcSettings.RowStyles.Add(new RowStyle());
            _tlpMcSettings.RowStyles.Add(new RowStyle());
            _tlpMcSettings.RowStyles.Add(new RowStyle());
            _tlpMcSettings.RowStyles.Add(new RowStyle());
            _tlpMcSettings.RowStyles.Add(new RowStyle());
            _tlpMcSettings.Size = new Size(329, 443);
            _tlpMcSettings.TabIndex = 0;
            // 
            // _lblMcSettingsTitle
            // 
            _lblMcSettingsTitle.AutoSize = true;
            _tlpMcSettings.SetColumnSpan(_lblMcSettingsTitle, 2);
            _lblMcSettingsTitle.Font = new Font("Microsoft YaHei UI", 10.5F, FontStyle.Bold);
            _lblMcSettingsTitle.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _lblMcSettingsTitle.Location = new Point(0, 2);
            _lblMcSettingsTitle.Margin = new Padding(0, 2, 0, 4);
            _lblMcSettingsTitle.Name = "_lblMcSettingsTitle";
            _lblMcSettingsTitle.Size = new Size(149, 19);
            _lblMcSettingsTitle.TabIndex = 0;
            _lblMcSettingsTitle.Text = "采集设置（选中相机）";
            // 
            // _lblMcRes
            // 
            _lblMcRes.AutoSize = true;
            _lblMcRes.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lblMcRes.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblMcRes.Location = new Point(0, 32);
            _lblMcRes.Margin = new Padding(0, 7, 4, 4);
            _lblMcRes.Name = "_lblMcRes";
            _lblMcRes.Size = new Size(44, 17);
            _lblMcRes.TabIndex = 1;
            _lblMcRes.Text = "分辨率";
            // 
            // _flowMcRes
            // 
            _flowMcRes.AutoSize = true;
            _flowMcRes.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _flowMcRes.Controls.Add(_mcCmbRes);
            _flowMcRes.Controls.Add(_mcNumW);
            _flowMcRes.Controls.Add(_lblMcTimes);
            _flowMcRes.Controls.Add(_mcNumH);
            _flowMcRes.Dock = DockStyle.Left;
            _flowMcRes.Location = new Point(58, 29);
            _flowMcRes.Margin = new Padding(0, 4, 0, 4);
            _flowMcRes.Name = "_flowMcRes";
            _flowMcRes.Size = new Size(265, 35);
            _flowMcRes.TabIndex = 2;
            // 
            // _mcCmbRes
            // 
            _mcCmbRes.DropDownStyle = ComboBoxStyle.DropDownList;
            _mcCmbRes.Font = new Font("Microsoft YaHei UI", 9.5F);
            _mcCmbRes.Items.AddRange(new object[] { "640 × 480", "800 × 600", "1280 × 720", "1920 × 1080", "自定义" });
            _mcCmbRes.Location = new Point(0, 4);
            _mcCmbRes.Margin = new Padding(0, 4, 6, 4);
            _mcCmbRes.Name = "_mcCmbRes";
            _mcCmbRes.Size = new Size(108, 27);
            _mcCmbRes.TabIndex = 0;
            // 
            // _mcNumW
            // 
            _mcNumW.Font = new Font("Microsoft YaHei UI", 9.5F);
            _mcNumW.Increment = new decimal(new int[] { 2, 0, 0, 0 });
            _mcNumW.Location = new Point(114, 4);
            _mcNumW.Margin = new Padding(0, 4, 0, 4);
            _mcNumW.Maximum = new decimal(new int[] { 3840, 0, 0, 0 });
            _mcNumW.Minimum = new decimal(new int[] { 160, 0, 0, 0 });
            _mcNumW.Name = "_mcNumW";
            _mcNumW.Size = new Size(64, 24);
            _mcNumW.TabIndex = 1;
            _mcNumW.Value = new decimal(new int[] { 1280, 0, 0, 0 });
            // 
            // _lblMcTimes
            // 
            _lblMcTimes.AutoSize = true;
            _lblMcTimes.Font = new Font("Microsoft YaHei UI", 9.5F);
            _lblMcTimes.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblMcTimes.Location = new Point(180, 8);
            _lblMcTimes.Margin = new Padding(2, 8, 2, 0);
            _lblMcTimes.Name = "_lblMcTimes";
            _lblMcTimes.Size = new Size(19, 19);
            _lblMcTimes.TabIndex = 2;
            _lblMcTimes.Text = "×";
            // 
            // _mcNumH
            // 
            _mcNumH.Font = new Font("Microsoft YaHei UI", 9.5F);
            _mcNumH.Increment = new decimal(new int[] { 2, 0, 0, 0 });
            _mcNumH.Location = new Point(201, 4);
            _mcNumH.Margin = new Padding(0, 4, 0, 4);
            _mcNumH.Maximum = new decimal(new int[] { 2160, 0, 0, 0 });
            _mcNumH.Minimum = new decimal(new int[] { 120, 0, 0, 0 });
            _mcNumH.Name = "_mcNumH";
            _mcNumH.Size = new Size(64, 24);
            _mcNumH.TabIndex = 3;
            _mcNumH.Value = new decimal(new int[] { 720, 0, 0, 0 });
            // 
            // _lblMcFps
            // 
            _lblMcFps.AutoSize = true;
            _lblMcFps.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lblMcFps.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblMcFps.Location = new Point(0, 75);
            _lblMcFps.Margin = new Padding(0, 7, 4, 4);
            _lblMcFps.Name = "_lblMcFps";
            _lblMcFps.Size = new Size(49, 34);
            _lblMcFps.TabIndex = 3;
            _lblMcFps.Text = "帧率/格式";
            // 
            // _flowMcFps
            // 
            _flowMcFps.AutoSize = true;
            _flowMcFps.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _flowMcFps.Controls.Add(_mcNumFps);
            _flowMcFps.Controls.Add(_mcCmbFmt);
            _flowMcFps.Dock = DockStyle.Left;
            _flowMcFps.Location = new Point(58, 72);
            _flowMcFps.Margin = new Padding(0, 4, 0, 4);
            _flowMcFps.Name = "_flowMcFps";
            _flowMcFps.Size = new Size(140, 37);
            _flowMcFps.TabIndex = 4;
            // 
            // _mcNumFps
            // 
            _mcNumFps.Font = new Font("Microsoft YaHei UI", 9.5F);
            _mcNumFps.Location = new Point(0, 4);
            _mcNumFps.Margin = new Padding(0, 4, 0, 4);
            _mcNumFps.Maximum = new decimal(new int[] { 120, 0, 0, 0 });
            _mcNumFps.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            _mcNumFps.Name = "_mcNumFps";
            _mcNumFps.Size = new Size(58, 24);
            _mcNumFps.TabIndex = 0;
            _mcNumFps.Value = new decimal(new int[] { 30, 0, 0, 0 });
            // 
            // _mcCmbFmt
            // 
            _mcCmbFmt.DropDownStyle = ComboBoxStyle.DropDownList;
            _mcCmbFmt.Font = new Font("Microsoft YaHei UI", 9.5F);
            _mcCmbFmt.Items.AddRange(new object[] { "自动", "MJPG", "YUY2" });
            _mcCmbFmt.Location = new Point(64, 4);
            _mcCmbFmt.Margin = new Padding(6, 4, 0, 4);
            _mcCmbFmt.Name = "_mcCmbFmt";
            _mcCmbFmt.Size = new Size(76, 27);
            _mcCmbFmt.TabIndex = 1;
            // 
            // _lblMcExp
            // 
            _lblMcExp.AutoSize = true;
            _lblMcExp.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lblMcExp.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblMcExp.Location = new Point(0, 120);
            _lblMcExp.Margin = new Padding(0, 7, 4, 4);
            _lblMcExp.Name = "_lblMcExp";
            _lblMcExp.Size = new Size(32, 17);
            _lblMcExp.TabIndex = 5;
            _lblMcExp.Text = "曝光";
            // 
            // _flowMcExp
            // 
            _flowMcExp.AutoSize = true;
            _flowMcExp.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _flowMcExp.Controls.Add(_mcChkAutoExp);
            _flowMcExp.Controls.Add(_mcNumExp);
            _flowMcExp.Dock = DockStyle.Left;
            _flowMcExp.Location = new Point(58, 117);
            _flowMcExp.Margin = new Padding(0, 4, 0, 4);
            _flowMcExp.Name = "_flowMcExp";
            _flowMcExp.Size = new Size(115, 32);
            _flowMcExp.TabIndex = 6;
            // 
            // _mcChkAutoExp
            // 
            _mcChkAutoExp.AutoSize = true;
            _mcChkAutoExp.Checked = true;
            _mcChkAutoExp.CheckState = CheckState.Checked;
            _mcChkAutoExp.Font = new Font("Microsoft YaHei UI", 8.5F);
            _mcChkAutoExp.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _mcChkAutoExp.Location = new Point(0, 9);
            _mcChkAutoExp.Margin = new Padding(0, 9, 6, 0);
            _mcChkAutoExp.Name = "_mcChkAutoExp";
            _mcChkAutoExp.Size = new Size(51, 21);
            _mcChkAutoExp.TabIndex = 0;
            _mcChkAutoExp.Text = "自动";
            // 
            // _mcNumExp
            // 
            _mcNumExp.Font = new Font("Microsoft YaHei UI", 9.5F);
            _mcNumExp.Location = new Point(57, 4);
            _mcNumExp.Margin = new Padding(0, 4, 0, 4);
            _mcNumExp.Maximum = new decimal(new int[] { 0, 0, 0, 0 });
            _mcNumExp.Minimum = new decimal(new int[] { 13, 0, 0, global::System.Int32.MinValue });
            _mcNumExp.Name = "_mcNumExp";
            _mcNumExp.Size = new Size(58, 24);
            _mcNumExp.TabIndex = 1;
            _mcNumExp.Value = new decimal(new int[] { 6, 0, 0, global::System.Int32.MinValue });
            // 
            // _lblMcWb
            // 
            _lblMcWb.AutoSize = true;
            _lblMcWb.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lblMcWb.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblMcWb.Location = new Point(0, 160);
            _lblMcWb.Margin = new Padding(0, 7, 4, 4);
            _lblMcWb.Name = "_lblMcWb";
            _lblMcWb.Size = new Size(44, 17);
            _lblMcWb.TabIndex = 7;
            _lblMcWb.Text = "白平衡";
            // 
            // _flowMcWb
            // 
            _flowMcWb.AutoSize = true;
            _flowMcWb.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _flowMcWb.Controls.Add(_mcChkAutoWb);
            _flowMcWb.Controls.Add(_mcNumWb);
            _flowMcWb.Dock = DockStyle.Left;
            _flowMcWb.Location = new Point(58, 157);
            _flowMcWb.Margin = new Padding(0, 4, 0, 4);
            _flowMcWb.Name = "_flowMcWb";
            _flowMcWb.Size = new Size(127, 32);
            _flowMcWb.TabIndex = 8;
            // 
            // _mcChkAutoWb
            // 
            _mcChkAutoWb.AutoSize = true;
            _mcChkAutoWb.Checked = true;
            _mcChkAutoWb.CheckState = CheckState.Checked;
            _mcChkAutoWb.Font = new Font("Microsoft YaHei UI", 8.5F);
            _mcChkAutoWb.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _mcChkAutoWb.Location = new Point(0, 9);
            _mcChkAutoWb.Margin = new Padding(0, 9, 6, 0);
            _mcChkAutoWb.Name = "_mcChkAutoWb";
            _mcChkAutoWb.Size = new Size(51, 21);
            _mcChkAutoWb.TabIndex = 0;
            _mcChkAutoWb.Text = "自动";
            // 
            // _mcNumWb
            // 
            _mcNumWb.Font = new Font("Microsoft YaHei UI", 9.5F);
            _mcNumWb.Increment = new decimal(new int[] { 100, 0, 0, 0 });
            _mcNumWb.Location = new Point(57, 4);
            _mcNumWb.Margin = new Padding(0, 4, 0, 4);
            _mcNumWb.Maximum = new decimal(new int[] { 6500, 0, 0, 0 });
            _mcNumWb.Minimum = new decimal(new int[] { 2800, 0, 0, 0 });
            _mcNumWb.Name = "_mcNumWb";
            _mcNumWb.Size = new Size(70, 24);
            _mcNumWb.TabIndex = 1;
            _mcNumWb.Value = new decimal(new int[] { 4600, 0, 0, 0 });
            // 
            // _lblMcGain
            // 
            _lblMcGain.AutoSize = true;
            _lblMcGain.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lblMcGain.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblMcGain.Location = new Point(0, 200);
            _lblMcGain.Margin = new Padding(0, 7, 4, 4);
            _lblMcGain.Name = "_lblMcGain";
            _lblMcGain.Size = new Size(32, 17);
            _lblMcGain.TabIndex = 9;
            _lblMcGain.Text = "增益";
            // 
            // _mcNumGain
            // 
            _mcNumGain.Dock = DockStyle.Left;
            _mcNumGain.Font = new Font("Microsoft YaHei UI", 9.5F);
            _mcNumGain.Location = new Point(58, 197);
            _mcNumGain.Margin = new Padding(0, 4, 0, 4);
            _mcNumGain.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            _mcNumGain.Name = "_mcNumGain";
            _mcNumGain.Size = new Size(58, 24);
            _mcNumGain.TabIndex = 10;
            // 
            // _lblMcBright
            // 
            _lblMcBright.AutoSize = true;
            _lblMcBright.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lblMcBright.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblMcBright.Location = new Point(0, 232);
            _lblMcBright.Margin = new Padding(0, 7, 4, 4);
            _lblMcBright.Name = "_lblMcBright";
            _lblMcBright.Size = new Size(32, 17);
            _lblMcBright.TabIndex = 11;
            _lblMcBright.Text = "亮度";
            // 
            // _mcNumBright
            // 
            _mcNumBright.Dock = DockStyle.Left;
            _mcNumBright.Font = new Font("Microsoft YaHei UI", 9.5F);
            _mcNumBright.Location = new Point(58, 229);
            _mcNumBright.Margin = new Padding(0, 4, 0, 4);
            _mcNumBright.Maximum = new decimal(new int[] { 64, 0, 0, 0 });
            _mcNumBright.Minimum = new decimal(new int[] { 64, 0, 0, global::System.Int32.MinValue });
            _mcNumBright.Name = "_mcNumBright";
            _mcNumBright.Size = new Size(58, 24);
            _mcNumBright.TabIndex = 12;
            // 
            // _lblMcContrast
            // 
            _lblMcContrast.AutoSize = true;
            _lblMcContrast.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lblMcContrast.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblMcContrast.Location = new Point(0, 264);
            _lblMcContrast.Margin = new Padding(0, 7, 4, 4);
            _lblMcContrast.Name = "_lblMcContrast";
            _lblMcContrast.Size = new Size(44, 17);
            _lblMcContrast.TabIndex = 13;
            _lblMcContrast.Text = "对比度";
            // 
            // _mcNumContrast
            // 
            _mcNumContrast.Dock = DockStyle.Left;
            _mcNumContrast.Font = new Font("Microsoft YaHei UI", 9.5F);
            _mcNumContrast.Location = new Point(58, 261);
            _mcNumContrast.Margin = new Padding(0, 4, 0, 4);
            _mcNumContrast.Maximum = new decimal(new int[] { 64, 0, 0, 0 });
            _mcNumContrast.Name = "_mcNumContrast";
            _mcNumContrast.Size = new Size(58, 24);
            _mcNumContrast.TabIndex = 14;
            _mcNumContrast.Value = new decimal(new int[] { 32, 0, 0, 0 });
            // 
            // _lblMcSat
            // 
            _lblMcSat.AutoSize = true;
            _lblMcSat.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lblMcSat.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblMcSat.Location = new Point(0, 296);
            _lblMcSat.Margin = new Padding(0, 7, 4, 4);
            _lblMcSat.Name = "_lblMcSat";
            _lblMcSat.Size = new Size(44, 17);
            _lblMcSat.TabIndex = 15;
            _lblMcSat.Text = "饱和度";
            // 
            // _mcNumSat
            // 
            _mcNumSat.Dock = DockStyle.Left;
            _mcNumSat.Font = new Font("Microsoft YaHei UI", 9.5F);
            _mcNumSat.Location = new Point(58, 293);
            _mcNumSat.Margin = new Padding(0, 4, 0, 4);
            _mcNumSat.Maximum = new decimal(new int[] { 128, 0, 0, 0 });
            _mcNumSat.Name = "_mcNumSat";
            _mcNumSat.Size = new Size(58, 24);
            _mcNumSat.TabIndex = 16;
            _mcNumSat.Value = new decimal(new int[] { 64, 0, 0, 0 });
            // 
            // _flowMcApply
            // 
            _flowMcApply.AutoSize = true;
            _flowMcApply.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            _tlpMcSettings.SetColumnSpan(_flowMcApply, 2);
            _flowMcApply.Controls.Add(_mcBtnApply);
            _flowMcApply.Location = new Point(0, 321);
            _flowMcApply.Margin = new Padding(0);
            _flowMcApply.Name = "_flowMcApply";
            _flowMcApply.Size = new Size(112, 42);
            _flowMcApply.TabIndex = 17;
            // 
            // _mcBtnApply
            // 
            _mcBtnApply.AutoSize = true;
            _mcBtnApply.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _mcBtnApply.BackColor = Color.FromArgb((int)(byte)36, (int)(byte)112, (int)(byte)210);
            _mcBtnApply.Cursor = Cursors.Hand;
            _mcBtnApply.FlatAppearance.BorderSize = 0;
            _mcBtnApply.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)28, (int)(byte)96, (int)(byte)186);
            _mcBtnApply.FlatStyle = FlatStyle.Flat;
            _mcBtnApply.Font = new Font("Microsoft YaHei UI", 9.5F, FontStyle.Bold);
            _mcBtnApply.ForeColor = Color.White;
            _mcBtnApply.Location = new Point(4, 4);
            _mcBtnApply.Margin = new Padding(4);
            _mcBtnApply.MinimumSize = new Size(84, 34);
            _mcBtnApply.Name = "_mcBtnApply";
            _mcBtnApply.Padding = new Padding(10, 0, 10, 0);
            _mcBtnApply.Size = new Size(104, 34);
            _mcBtnApply.TabIndex = 0;
            _mcBtnApply.Text = "应用并重开";
            _mcBtnApply.UseVisualStyleBackColor = false;
            // 
            // _mcLblActual
            // 
            _mcLblActual.AutoSize = true;
            _tlpMcSettings.SetColumnSpan(_mcLblActual, 2);
            _mcLblActual.Font = new Font("Microsoft YaHei UI", 8.5F);
            _mcLblActual.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _mcLblActual.Location = new Point(0, 365);
            _mcLblActual.Margin = new Padding(0, 2, 0, 6);
            _mcLblActual.Name = "_mcLblActual";
            _mcLblActual.Size = new Size(57, 17);
            _mcLblActual.TabIndex = 18;
            _mcLblActual.Text = "实际：—";
            // 
            // _lblMcSettingsNote
            // 
            _lblMcSettingsNote.AutoSize = true;
            _tlpMcSettings.SetColumnSpan(_lblMcSettingsNote, 2);
            _lblMcSettingsNote.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lblMcSettingsNote.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblMcSettingsNote.Location = new Point(0, 388);
            _lblMcSettingsNote.Margin = new Padding(0, 0, 0, 4);
            _lblMcSettingsNote.MaximumSize = new Size(315, 0);
            _lblMcSettingsNote.Name = "_lblMcSettingsNote";
            _lblMcSettingsNote.Size = new Size(313, 51);
            _lblMcSettingsNote.TabIndex = 19;
            _lblMcSettingsNote.Text = "USB2 相机线速约 18–20MB/s：720p 约 10fps、1080p 约 5fps，CPU 本身很低；驱动不接受的参数会被它自己改掉，看“实际”。";
            // 
            // _lblMcProcTitle
            // 
            _lblMcProcTitle.AutoSize = true;
            _lblMcProcTitle.Font = new Font("Microsoft YaHei UI", 10.5F, FontStyle.Bold);
            _lblMcProcTitle.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _lblMcProcTitle.Location = new Point(10, 499);
            _lblMcProcTitle.Margin = new Padding(0, 4, 0, 2);
            _lblMcProcTitle.Name = "_lblMcProcTitle";
            _lblMcProcTitle.Size = new Size(79, 19);
            _lblMcProcTitle.TabIndex = 7;
            _lblMcProcTitle.Text = "本进程资源";
            // 
            // _pnlMcBottom
            // 
            _pnlMcBottom.AutoScroll = true;
            _pnlMcBottom.Controls.Add(_lblMcHint);
            _pnlMcBottom.Controls.Add(_mcProcInfo);
            _pnlMcBottom.Dock = DockStyle.Fill;
            _pnlMcBottom.Location = new Point(13, 523);
            _pnlMcBottom.Name = "_pnlMcBottom";
            _pnlMcBottom.Size = new Size(340, 117);
            _pnlMcBottom.TabIndex = 8;
            // 
            // _lblMcHint
            // 
            _lblMcHint.AutoSize = true;
            _lblMcHint.Dock = DockStyle.Top;
            _lblMcHint.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lblMcHint.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblMcHint.Location = new Point(0, 17);
            _lblMcHint.MaximumSize = new Size(320, 0);
            _lblMcHint.Name = "_lblMcHint";
            _lblMcHint.Size = new Size(319, 85);
            _lblMcHint.TabIndex = 0;
            _lblMcHint.Text = "CPU 的两种口径：表格里是“本路采集线程”（只含缩放+拷贝，所以很小），驱动内部的解码/传输入口线程不计在内；“整进程 CPU”才是整个程序真实占用，评估用后者。点左侧任意一格选中一路，右侧“采集设置”只改这一路，改完重新打开才生效。";
            // 
            // _mcProcInfo
            // 
            _mcProcInfo.AutoSize = true;
            _mcProcInfo.Dock = DockStyle.Top;
            _mcProcInfo.Font = new Font("Microsoft YaHei UI", 8.5F);
            _mcProcInfo.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _mcProcInfo.Location = new Point(0, 0);
            _mcProcInfo.Margin = new Padding(0);
            _mcProcInfo.Name = "_mcProcInfo";
            _mcProcInfo.Size = new Size(0, 17);
            _mcProcInfo.TabIndex = 1;
            // 
            // pnlStatus
            // 
            pnlStatus.BackColor = Color.White;
            pnlStatus.ColumnCount = 2;
            pnlStatus.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            pnlStatus.ColumnStyles.Add(new ColumnStyle());
            pnlStatus.Controls.Add(_lblStatus, 0, 0);
            pnlStatus.Controls.Add(_lblStatusRight, 1, 0);
            pnlStatus.Dock = DockStyle.Fill;
            pnlStatus.Location = new Point(0, 791);
            pnlStatus.Margin = new Padding(0);
            pnlStatus.Name = "pnlStatus";
            pnlStatus.Padding = new Padding(14, 0, 14, 0);
            pnlStatus.RowCount = 1;
            pnlStatus.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            pnlStatus.Size = new Size(1284, 30);
            pnlStatus.TabIndex = 2;
            pnlStatus.Paint += (this.pnlStatus_Paint);
            // 
            // _lblStatus
            // 
            _lblStatus.AutoEllipsis = true;
            _lblStatus.Dock = DockStyle.Fill;
            _lblStatus.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lblStatus.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblStatus.Location = new Point(14, 0);
            _lblStatus.Margin = new Padding(0);
            _lblStatus.Name = "_lblStatus";
            _lblStatus.Size = new Size(1119, 30);
            _lblStatus.TabIndex = 0;
            _lblStatus.Text = "正在启动…";
            _lblStatus.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // _lblStatusRight
            // 
            _lblStatusRight.Anchor = (AnchorStyles)(AnchorStyles.Top) | (AnchorStyles.Right);
            _lblStatusRight.AutoSize = true;
            _lblStatusRight.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lblStatusRight.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblStatusRight.Location = new Point(1133, 0);
            _lblStatusRight.Margin = new Padding(0);
            _lblStatusRight.Name = "_lblStatusRight";
            _lblStatusRight.Size = new Size(137, 17);
            _lblStatusRight.TabIndex = 1;
            _lblStatusRight.Text = "数据集：0 张图 · 0 个框";
            _lblStatusRight.TextAlign = ContentAlignment.MiddleRight;
            // 
            // _splitRec
            // 
            _splitRec.BackColor = Color.FromArgb((int)(byte)219, (int)(byte)224, (int)(byte)232);
            _splitRec.Dock = DockStyle.Fill;
            _splitRec.FixedPanel = FixedPanel.Panel2;
            _splitRec.Location = new Point(0, 0);
            _splitRec.Margin = new Padding(0);
            _splitRec.Name = "_splitRec";
            // 
            // _splitRec.Panel1
            // 
            _splitRec.Panel1.Controls.Add(pnlRecLeft);
            _splitRec.Panel1MinSize = 300;
            // 
            // _splitRec.Panel2
            // 
            _splitRec.Panel2.Controls.Add(pnlRecSide);
            _splitRec.Panel2MinSize = 340;
            _splitRec.Size = new Size(1000, 600);
            _splitRec.SplitterDistance = 300;
            _splitRec.SplitterWidth = 8;
            _splitRec.TabIndex = 0;
            // 
            // pnlRecLeft
            // 
            pnlRecLeft.BackColor = Color.White;
            pnlRecLeft.Controls.Add(_picRec);
            pnlRecLeft.Dock = DockStyle.Fill;
            pnlRecLeft.Location = new Point(0, 0);
            pnlRecLeft.Name = "pnlRecLeft";
            pnlRecLeft.Padding = new Padding(8);
            pnlRecLeft.Size = new Size(300, 600);
            pnlRecLeft.TabIndex = 0;
            // 
            // _picRec
            // 
            _picRec.BackColor = Color.FromArgb((int)(byte)36, (int)(byte)40, (int)(byte)47);
            _picRec.BoxLabel = null;
            _picRec.BoxPending = null;
            _picRec.CanvasMenu = null;
            _picRec.DetectionLabel = null;
            _picRec.Dock = DockStyle.Fill;
            _picRec.Editable = false;
            _picRec.EmptyText = "① 打开图片 / 摄像头 / 视频\r\n② 点“开始识别”";
            _picRec.Font = new Font("Microsoft YaHei UI", 9.5F);
            _picRec.Image = null;
            _picRec.LineA = null;
            _picRec.LineB = null;
            _picRec.LineEditable = false;
            _picRec.LineLabel = "计数线";
            _picRec.Location = new Point(8, 8);
            _picRec.Name = "_picRec";
            _picRec.RightClickAction = null;
            _picRec.SelectedIndex = -1;
            _picRec.ShowZoomInfo = true;
            _picRec.Size = new Size(284, 584);
            _picRec.TabIndex = 0;
            _picRec.TrackLabel = null;
            // 
            // pnlRecSide
            // 
            pnlRecSide.BackColor = Color.White;
            pnlRecSide.Controls.Add(tlpSide);
            pnlRecSide.Dock = DockStyle.Fill;
            pnlRecSide.Location = new Point(0, 0);
            pnlRecSide.Name = "pnlRecSide";
            pnlRecSide.Padding = new Padding(12, 10, 10, 10);
            pnlRecSide.Size = new Size(696, 600);
            pnlRecSide.TabIndex = 0;
            // 
            // tlpSide
            // 
            tlpSide.BackColor = Color.White;
            tlpSide.ColumnCount = 1;
            tlpSide.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpSide.Controls.Add(tlpSideHead, 0, 0);
            tlpSide.Controls.Add(tlpSideStatus, 0, 1);
            tlpSide.Controls.Add(_pnlMetrics, 0, 2);
            tlpSide.Controls.Add(_grid, 0, 3);
            tlpSide.Controls.Add(lblGridHint, 0, 4);
            tlpSide.Dock = DockStyle.Fill;
            tlpSide.Location = new Point(12, 10);
            tlpSide.Name = "tlpSide";
            tlpSide.RowCount = 5;
            tlpSide.RowStyles.Add(new RowStyle());
            tlpSide.RowStyles.Add(new RowStyle());
            tlpSide.RowStyles.Add(new RowStyle());
            tlpSide.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpSide.RowStyles.Add(new RowStyle());
            tlpSide.Size = new Size(674, 580);
            tlpSide.TabIndex = 0;
            // 
            // tlpSideHead
            // 
            tlpSideHead.AutoSize = true;
            tlpSideHead.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tlpSideHead.ColumnCount = 1;
            tlpSideHead.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpSideHead.Controls.Add(_lblResultTitle, 0, 0);
            tlpSideHead.Controls.Add(flowResultTools, 0, 1);
            tlpSideHead.Dock = DockStyle.Top;
            tlpSideHead.Location = new Point(0, 0);
            tlpSideHead.Margin = new Padding(0, 0, 0, 2);
            tlpSideHead.Name = "tlpSideHead";
            tlpSideHead.RowCount = 2;
            tlpSideHead.RowStyles.Add(new RowStyle());
            tlpSideHead.RowStyles.Add(new RowStyle());
            tlpSideHead.Size = new Size(674, 59);
            tlpSideHead.TabIndex = 0;
            // 
            // _lblResultTitle
            // 
            _lblResultTitle.AutoSize = true;
            _lblResultTitle.Font = new Font("Microsoft YaHei UI", 10.5F, FontStyle.Bold);
            _lblResultTitle.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _lblResultTitle.Location = new Point(0, 2);
            _lblResultTitle.Margin = new Padding(0, 2, 0, 0);
            _lblResultTitle.Name = "_lblResultTitle";
            _lblResultTitle.Size = new Size(65, 19);
            _lblResultTitle.TabIndex = 0;
            _lblResultTitle.Text = "识别结果";
            // 
            // flowResultTools
            // 
            flowResultTools.AutoSize = true;
            flowResultTools.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flowResultTools.Controls.Add(_lblFilterState);
            flowResultTools.Controls.Add(_btnFilter);
            flowResultTools.Controls.Add(_btnBlockClass);
            flowResultTools.Controls.Add(_btnDeleteResult);
            flowResultTools.Dock = DockStyle.Fill;
            flowResultTools.Location = new Point(0, 23);
            flowResultTools.Margin = new Padding(0, 2, 0, 0);
            flowResultTools.Name = "flowResultTools";
            flowResultTools.Size = new Size(674, 36);
            flowResultTools.TabIndex = 1;
            flowResultTools.WrapContents = false;
            // 
            // _lblFilterState
            // 
            _lblFilterState.AutoSize = true;
            _lblFilterState.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lblFilterState.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblFilterState.Location = new Point(0, 6);
            _lblFilterState.Margin = new Padding(0, 6, 8, 0);
            _lblFilterState.Name = "_lblFilterState";
            _lblFilterState.Size = new Size(56, 17);
            _lblFilterState.TabIndex = 0;
            _lblFilterState.Text = "筛除：关";
            // 
            // _btnFilter
            // 
            _btnFilter.AutoSize = true;
            _btnFilter.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnFilter.BackColor = Color.Transparent;
            _btnFilter.Cursor = Cursors.Hand;
            _btnFilter.FlatAppearance.BorderSize = 0;
            _btnFilter.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)232, (int)(byte)240, (int)(byte)252);
            _btnFilter.FlatStyle = FlatStyle.Flat;
            _btnFilter.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnFilter.ForeColor = Color.FromArgb((int)(byte)36, (int)(byte)112, (int)(byte)210);
            _btnFilter.Location = new Point(64, 2);
            _btnFilter.Margin = new Padding(0, 2, 10, 0);
            _btnFilter.MinimumSize = new Size(84, 34);
            _btnFilter.Name = "_btnFilter";
            _btnFilter.Padding = new Padding(10, 0, 10, 0);
            _btnFilter.Size = new Size(84, 34);
            _btnFilter.TabIndex = 1;
            _btnFilter.Text = "筛除…";
            _btnFilter.UseVisualStyleBackColor = false;
            // 
            // _btnBlockClass
            // 
            _btnBlockClass.AutoSize = true;
            _btnBlockClass.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnBlockClass.BackColor = Color.Transparent;
            _btnBlockClass.Cursor = Cursors.Hand;
            _btnBlockClass.FlatAppearance.BorderSize = 0;
            _btnBlockClass.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)232, (int)(byte)240, (int)(byte)252);
            _btnBlockClass.FlatStyle = FlatStyle.Flat;
            _btnBlockClass.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnBlockClass.ForeColor = Color.FromArgb((int)(byte)36, (int)(byte)112, (int)(byte)210);
            _btnBlockClass.Location = new Point(158, 2);
            _btnBlockClass.Margin = new Padding(0, 2, 10, 0);
            _btnBlockClass.MinimumSize = new Size(84, 34);
            _btnBlockClass.Name = "_btnBlockClass";
            _btnBlockClass.Padding = new Padding(10, 0, 10, 0);
            _btnBlockClass.Size = new Size(91, 34);
            _btnBlockClass.TabIndex = 2;
            _btnBlockClass.Text = "屏蔽该类";
            _btnBlockClass.UseVisualStyleBackColor = false;
            // 
            // _btnDeleteResult
            // 
            _btnDeleteResult.AutoSize = true;
            _btnDeleteResult.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnDeleteResult.BackColor = Color.Transparent;
            _btnDeleteResult.Cursor = Cursors.Hand;
            _btnDeleteResult.FlatAppearance.BorderSize = 0;
            _btnDeleteResult.FlatAppearance.MouseOverBackColor = Color.FromArgb((int)(byte)232, (int)(byte)240, (int)(byte)252);
            _btnDeleteResult.FlatStyle = FlatStyle.Flat;
            _btnDeleteResult.Font = new Font("Microsoft YaHei UI", 9.5F);
            _btnDeleteResult.ForeColor = Color.FromArgb((int)(byte)36, (int)(byte)112, (int)(byte)210);
            _btnDeleteResult.Location = new Point(259, 2);
            _btnDeleteResult.Margin = new Padding(0, 2, 0, 0);
            _btnDeleteResult.MinimumSize = new Size(84, 34);
            _btnDeleteResult.Name = "_btnDeleteResult";
            _btnDeleteResult.Padding = new Padding(10, 0, 10, 0);
            _btnDeleteResult.Size = new Size(84, 34);
            _btnDeleteResult.TabIndex = 3;
            _btnDeleteResult.Text = "删除";
            _btnDeleteResult.UseVisualStyleBackColor = false;
            // 
            // tlpSideStatus
            // 
            tlpSideStatus.AutoSize = true;
            tlpSideStatus.ColumnCount = 1;
            tlpSideStatus.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpSideStatus.Controls.Add(_lblResultStats, 0, 0);
            tlpSideStatus.Controls.Add(_lblVideoArea, 0, 1);
            tlpSideStatus.Dock = DockStyle.Top;
            tlpSideStatus.Location = new Point(0, 61);
            tlpSideStatus.Margin = new Padding(0, 0, 0, 6);
            tlpSideStatus.Name = "tlpSideStatus";
            tlpSideStatus.RowCount = 2;
            tlpSideStatus.RowStyles.Add(new RowStyle());
            tlpSideStatus.RowStyles.Add(new RowStyle());
            tlpSideStatus.Size = new Size(674, 40);
            tlpSideStatus.TabIndex = 1;
            // 
            // _lblResultStats
            // 
            _lblResultStats.AutoSize = true;
            _lblResultStats.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lblResultStats.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblResultStats.Location = new Point(0, 0);
            _lblResultStats.Margin = new Padding(0, 0, 0, 2);
            _lblResultStats.Name = "_lblResultStats";
            _lblResultStats.Size = new Size(68, 17);
            _lblResultStats.TabIndex = 0;
            _lblResultStats.Text = "尚未识别。";
            // 
            // _lblVideoArea
            // 
            _lblVideoArea.AutoSize = true;
            _lblVideoArea.Font = new Font("Microsoft YaHei UI", 8.5F);
            _lblVideoArea.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            _lblVideoArea.Location = new Point(0, 19);
            _lblVideoArea.Margin = new Padding(0, 0, 0, 4);
            _lblVideoArea.Name = "_lblVideoArea";
            _lblVideoArea.Size = new Size(0, 17);
            _lblVideoArea.TabIndex = 1;
            // 
            // _pnlMetrics
            // 
            _pnlMetrics.AutoSize = true;
            _pnlMetrics.ColumnCount = 4;
            _pnlMetrics.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            _pnlMetrics.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            _pnlMetrics.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            _pnlMetrics.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            _pnlMetrics.Controls.Add(tlpMetricActive, 0, 0);
            _pnlMetrics.Controls.Add(tlpMetricTotal, 1, 0);
            _pnlMetrics.Controls.Add(tlpMetricAB, 2, 0);
            _pnlMetrics.Controls.Add(tlpMetricBA, 3, 0);
            _pnlMetrics.Dock = DockStyle.Top;
            _pnlMetrics.Location = new Point(0, 107);
            _pnlMetrics.Margin = new Padding(0, 0, 0, 8);
            _pnlMetrics.Name = "_pnlMetrics";
            _pnlMetrics.RowCount = 1;
            _pnlMetrics.RowStyles.Add(new RowStyle());
            _pnlMetrics.Size = new Size(674, 46);
            _pnlMetrics.TabIndex = 2;
            _pnlMetrics.Visible = false;
            // 
            // tlpMetricActive
            // 
            tlpMetricActive.AutoSize = true;
            tlpMetricActive.BackColor = Color.FromArgb((int)(byte)232, (int)(byte)240, (int)(byte)252);
            tlpMetricActive.ColumnCount = 1;
            tlpMetricActive.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpMetricActive.Controls.Add(lblMetricActiveTitle, 0, 0);
            tlpMetricActive.Controls.Add(_lblVideoActive, 0, 1);
            tlpMetricActive.Dock = DockStyle.Fill;
            tlpMetricActive.Location = new Point(0, 0);
            tlpMetricActive.Margin = new Padding(0, 0, 6, 0);
            tlpMetricActive.Name = "tlpMetricActive";
            tlpMetricActive.Padding = new Padding(7, 4, 7, 5);
            tlpMetricActive.RowCount = 2;
            tlpMetricActive.RowStyles.Add(new RowStyle());
            tlpMetricActive.RowStyles.Add(new RowStyle());
            tlpMetricActive.Size = new Size(162, 46);
            tlpMetricActive.TabIndex = 0;
            // 
            // lblMetricActiveTitle
            // 
            lblMetricActiveTitle.AutoSize = true;
            lblMetricActiveTitle.Font = new Font("Microsoft YaHei UI", 8.5F);
            lblMetricActiveTitle.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            lblMetricActiveTitle.Location = new Point(7, 4);
            lblMetricActiveTitle.Margin = new Padding(0, 0, 0, 1);
            lblMetricActiveTitle.Name = "lblMetricActiveTitle";
            lblMetricActiveTitle.Size = new Size(49, 17);
            lblMetricActiveTitle.TabIndex = 0;
            lblMetricActiveTitle.Text = "活跃 ID";
            // 
            // _lblVideoActive
            // 
            _lblVideoActive.AutoSize = true;
            _lblVideoActive.Font = new Font("Microsoft YaHei UI", 10.5F, FontStyle.Bold);
            _lblVideoActive.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _lblVideoActive.Location = new Point(7, 22);
            _lblVideoActive.Margin = new Padding(0);
            _lblVideoActive.Name = "_lblVideoActive";
            _lblVideoActive.Size = new Size(18, 19);
            _lblVideoActive.TabIndex = 1;
            _lblVideoActive.Text = "0";
            // 
            // tlpMetricTotal
            // 
            tlpMetricTotal.AutoSize = true;
            tlpMetricTotal.BackColor = Color.FromArgb((int)(byte)232, (int)(byte)240, (int)(byte)252);
            tlpMetricTotal.ColumnCount = 1;
            tlpMetricTotal.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpMetricTotal.Controls.Add(lblMetricTotalTitle, 0, 0);
            tlpMetricTotal.Controls.Add(_lblVideoTotal, 0, 1);
            tlpMetricTotal.Dock = DockStyle.Fill;
            tlpMetricTotal.Location = new Point(168, 0);
            tlpMetricTotal.Margin = new Padding(0, 0, 6, 0);
            tlpMetricTotal.Name = "tlpMetricTotal";
            tlpMetricTotal.Padding = new Padding(7, 4, 7, 5);
            tlpMetricTotal.RowCount = 2;
            tlpMetricTotal.RowStyles.Add(new RowStyle());
            tlpMetricTotal.RowStyles.Add(new RowStyle());
            tlpMetricTotal.Size = new Size(162, 46);
            tlpMetricTotal.TabIndex = 1;
            // 
            // lblMetricTotalTitle
            // 
            lblMetricTotalTitle.AutoSize = true;
            lblMetricTotalTitle.Font = new Font("Microsoft YaHei UI", 8.5F);
            lblMetricTotalTitle.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            lblMetricTotalTitle.Location = new Point(7, 4);
            lblMetricTotalTitle.Margin = new Padding(0, 0, 0, 1);
            lblMetricTotalTitle.Name = "lblMetricTotalTitle";
            lblMetricTotalTitle.Size = new Size(49, 17);
            lblMetricTotalTitle.TabIndex = 0;
            lblMetricTotalTitle.Text = "累计 ID";
            // 
            // _lblVideoTotal
            // 
            _lblVideoTotal.AutoSize = true;
            _lblVideoTotal.Font = new Font("Microsoft YaHei UI", 10.5F, FontStyle.Bold);
            _lblVideoTotal.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _lblVideoTotal.Location = new Point(7, 22);
            _lblVideoTotal.Margin = new Padding(0);
            _lblVideoTotal.Name = "_lblVideoTotal";
            _lblVideoTotal.Size = new Size(18, 19);
            _lblVideoTotal.TabIndex = 1;
            _lblVideoTotal.Text = "0";
            // 
            // tlpMetricAB
            // 
            tlpMetricAB.AutoSize = true;
            tlpMetricAB.BackColor = Color.FromArgb((int)(byte)232, (int)(byte)240, (int)(byte)252);
            tlpMetricAB.ColumnCount = 1;
            tlpMetricAB.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpMetricAB.Controls.Add(lblMetricABTitle, 0, 0);
            tlpMetricAB.Controls.Add(_lblVideoAtoB, 0, 1);
            tlpMetricAB.Dock = DockStyle.Fill;
            tlpMetricAB.Location = new Point(336, 0);
            tlpMetricAB.Margin = new Padding(0, 0, 6, 0);
            tlpMetricAB.Name = "tlpMetricAB";
            tlpMetricAB.Padding = new Padding(7, 4, 7, 5);
            tlpMetricAB.RowCount = 2;
            tlpMetricAB.RowStyles.Add(new RowStyle());
            tlpMetricAB.RowStyles.Add(new RowStyle());
            tlpMetricAB.Size = new Size(162, 46);
            tlpMetricAB.TabIndex = 2;
            // 
            // lblMetricABTitle
            // 
            lblMetricABTitle.AutoSize = true;
            lblMetricABTitle.Font = new Font("Microsoft YaHei UI", 8.5F);
            lblMetricABTitle.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            lblMetricABTitle.Location = new Point(7, 4);
            lblMetricABTitle.Margin = new Padding(0, 0, 0, 1);
            lblMetricABTitle.Name = "lblMetricABTitle";
            lblMetricABTitle.Size = new Size(36, 17);
            lblMetricABTitle.TabIndex = 0;
            lblMetricABTitle.Text = "A→B";
            // 
            // _lblVideoAtoB
            // 
            _lblVideoAtoB.AutoSize = true;
            _lblVideoAtoB.Font = new Font("Microsoft YaHei UI", 10.5F, FontStyle.Bold);
            _lblVideoAtoB.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _lblVideoAtoB.Location = new Point(7, 22);
            _lblVideoAtoB.Margin = new Padding(0);
            _lblVideoAtoB.Name = "_lblVideoAtoB";
            _lblVideoAtoB.Size = new Size(18, 19);
            _lblVideoAtoB.TabIndex = 1;
            _lblVideoAtoB.Text = "0";
            // 
            // tlpMetricBA
            // 
            tlpMetricBA.AutoSize = true;
            tlpMetricBA.BackColor = Color.FromArgb((int)(byte)232, (int)(byte)240, (int)(byte)252);
            tlpMetricBA.ColumnCount = 1;
            tlpMetricBA.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpMetricBA.Controls.Add(lblMetricBATitle, 0, 0);
            tlpMetricBA.Controls.Add(_lblVideoBtoA, 0, 1);
            tlpMetricBA.Dock = DockStyle.Fill;
            tlpMetricBA.Location = new Point(504, 0);
            tlpMetricBA.Margin = new Padding(0);
            tlpMetricBA.Name = "tlpMetricBA";
            tlpMetricBA.Padding = new Padding(7, 4, 7, 5);
            tlpMetricBA.RowCount = 2;
            tlpMetricBA.RowStyles.Add(new RowStyle());
            tlpMetricBA.RowStyles.Add(new RowStyle());
            tlpMetricBA.Size = new Size(170, 46);
            tlpMetricBA.TabIndex = 3;
            // 
            // lblMetricBATitle
            // 
            lblMetricBATitle.AutoSize = true;
            lblMetricBATitle.Font = new Font("Microsoft YaHei UI", 8.5F);
            lblMetricBATitle.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            lblMetricBATitle.Location = new Point(7, 4);
            lblMetricBATitle.Margin = new Padding(0, 0, 0, 1);
            lblMetricBATitle.Name = "lblMetricBATitle";
            lblMetricBATitle.Size = new Size(36, 17);
            lblMetricBATitle.TabIndex = 0;
            lblMetricBATitle.Text = "B→A";
            // 
            // _lblVideoBtoA
            // 
            _lblVideoBtoA.AutoSize = true;
            _lblVideoBtoA.Font = new Font("Microsoft YaHei UI", 10.5F, FontStyle.Bold);
            _lblVideoBtoA.ForeColor = Color.FromArgb((int)(byte)30, (int)(byte)35, (int)(byte)44);
            _lblVideoBtoA.Location = new Point(7, 22);
            _lblVideoBtoA.Margin = new Padding(0);
            _lblVideoBtoA.Name = "_lblVideoBtoA";
            _lblVideoBtoA.Size = new Size(18, 19);
            _lblVideoBtoA.TabIndex = 1;
            _lblVideoBtoA.Text = "0";
            // 
            // _grid
            // 
            _grid.AllowUserToAddRows = false;
            _grid.AllowUserToDeleteRows = false;
            _grid.AllowUserToResizeRows = false;
            _grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            _grid.ColumnHeadersHeight = 34;
            _grid.Columns.AddRange(new DataGridViewColumn[] { colId, colName, colConf, colPos, colCenter, colArea, colPct });
            _grid.Dock = DockStyle.Fill;
            _grid.Location = new Point(0, 161);
            _grid.Margin = new Padding(0);
            _grid.MultiSelect = false;
            _grid.Name = "_grid";
            _grid.ReadOnly = true;
            _grid.RowHeadersVisible = false;
            _grid.RowTemplate.Height = 30;
            _grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            _grid.Size = new Size(674, 385);
            _grid.TabIndex = 3;
            // 
            // colId
            // 
            colId.FillWeight = 8F;
            colId.HeaderText = "ID";
            colId.MinimumWidth = 34;
            colId.Name = "colId";
            colId.ReadOnly = true;
            colId.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // colName
            // 
            colName.FillWeight = 24F;
            colName.HeaderText = "目标";
            colName.MinimumWidth = 56;
            colName.Name = "colName";
            colName.ReadOnly = true;
            colName.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // colConf
            // 
            colConf.FillWeight = 18F;
            colConf.HeaderText = "匹配度";
            colConf.MinimumWidth = 52;
            colConf.Name = "colConf";
            colConf.ReadOnly = true;
            colConf.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // colPos
            // 
            colPos.FillWeight = 30F;
            colPos.HeaderText = "位置";
            colPos.MinimumWidth = 78;
            colPos.Name = "colPos";
            colPos.ReadOnly = true;
            colPos.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // colCenter
            // 
            colCenter.FillWeight = 20F;
            colCenter.HeaderText = "中心";
            colCenter.MinimumWidth = 64;
            colCenter.Name = "colCenter";
            colCenter.ReadOnly = true;
            colCenter.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // colArea
            // 
            colArea.FillWeight = 22F;
            colArea.HeaderText = "面积";
            colArea.MinimumWidth = 56;
            colArea.Name = "colArea";
            colArea.ReadOnly = true;
            colArea.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // colPct
            // 
            colPct.FillWeight = 16F;
            colPct.HeaderText = "占比";
            colPct.MinimumWidth = 44;
            colPct.Name = "colPct";
            colPct.ReadOnly = true;
            colPct.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // lblGridHint
            // 
            lblGridHint.Dock = DockStyle.Fill;
            lblGridHint.Font = new Font("Microsoft YaHei UI", 8.5F);
            lblGridHint.ForeColor = Color.FromArgb((int)(byte)108, (int)(byte)116, (int)(byte)130);
            lblGridHint.Location = new Point(0, 550);
            lblGridHint.Margin = new Padding(0, 4, 0, 0);
            lblGridHint.Name = "lblGridHint";
            lblGridHint.Size = new Size(674, 30);
            lblGridHint.TabIndex = 4;
            lblGridHint.Text = "点表格行可在图上高亮；视频模式点目标可看轨迹 ID。";
            lblGridHint.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new SizeF(96F, 96F);
            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.BackColor = Color.FromArgb((int)(byte)244, (int)(byte)246, (int)(byte)250);
            this.ClientSize = new Size(1284, 821);
            this.Controls.Add(tlpRoot);
            this.DoubleBuffered = true;
            this.Font = new Font("Microsoft YaHei UI", 9.5F);
            this.MinimumSize = new Size(1000, 620);
            this.Name = "MainForm";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "YoloFool · 傻瓜式视觉识别";
            tlpRoot.ResumeLayout(false);
            tlpRoot.PerformLayout();
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            tlpTitle.ResumeLayout(false);
            tlpTitle.PerformLayout();
            tlpModel.ResumeLayout(false);
            tlpModel.PerformLayout();
            _tabs.ResumeLayout(false);
            _pageRec.ResumeLayout(false);
            tlpCap.ResumeLayout(false);
            tlpCap.PerformLayout();
            tlpCapBar.ResumeLayout(false);
            tlpCapBar.PerformLayout();
            tlpVideoOpt.ResumeLayout(false);
            tlpVideoOpt.PerformLayout();
            flowVideo1.ResumeLayout(false);
            flowVideo1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)_numVideoStep).EndInit();
            flowVideo2.ResumeLayout(false);
            flowVideo2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)_numVideoConf).EndInit();
            _pnlAdv.ResumeLayout(false);
            _pnlAdv.PerformLayout();
            tlpAdvGrid.ResumeLayout(false);
            tlpAdvGrid.PerformLayout();
            tlpAdvPick.ResumeLayout(false);
            tlpAdvPick.PerformLayout();
            flowAdvThr.ResumeLayout(false);
            flowAdvThr.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)_numConf).EndInit();
            ((System.ComponentModel.ISupportInitialize)_numNms).EndInit();
            tlpAdvFooter.ResumeLayout(false);
            tlpAdvFooter.PerformLayout();
            flowAdvFooter.ResumeLayout(false);
            _pageLearn.ResumeLayout(false);
            _pageLearn.PerformLayout();
            _tlpLearn.ResumeLayout(false);
            _tlpLearn.PerformLayout();
            _tlpLearnBar.ResumeLayout(false);
            _tlpLearnBar.PerformLayout();
            _splitLearn.Panel1.ResumeLayout(false);
            _splitLearn.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)_splitLearn).EndInit();
            _splitLearn.ResumeLayout(false);
            _pnlLearnLeft.ResumeLayout(false);
            _pnlLearnSide.ResumeLayout(false);
            _pnlLearnSide.PerformLayout();
            _tlpLearnStack.ResumeLayout(false);
            _tlpLearnStack.PerformLayout();
            _cardImgList.ResumeLayout(false);
            _cardImgList.PerformLayout();
            _cardImgListBody.ResumeLayout(false);
            _cardImgListBody.PerformLayout();
            _flowImgList.ResumeLayout(false);
            _flowAuto.ResumeLayout(false);
            _cardBoxes.ResumeLayout(false);
            _cardBoxes.PerformLayout();
            _cardBoxesBody.ResumeLayout(false);
            _cardBoxesBody.PerformLayout();
            _flowBoxOps.ResumeLayout(false);
            _cardDataset.ResumeLayout(false);
            _cardDataset.PerformLayout();
            _cardDatasetBody.ResumeLayout(false);
            _cardDatasetBody.PerformLayout();
            _cardTrain.ResumeLayout(false);
            _cardTrain.PerformLayout();
            _cardTrainBody.ResumeLayout(false);
            _cardTrainBody.PerformLayout();
            _tlpTrainGrid.ResumeLayout(false);
            _tlpTrainGrid.PerformLayout();
            _tlpTrainLeft.ResumeLayout(false);
            _tlpTrainLeft.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)_numEpochs).EndInit();
            ((System.ComponentModel.ISupportInitialize)_numBatch).EndInit();
            _flowTrainBtns.ResumeLayout(false);
            _tlpTrainRight.ResumeLayout(false);
            _tlpTrainRight.PerformLayout();
            _pageTools.ResumeLayout(false);
            _tlpTools.ResumeLayout(false);
            _tlpTools.PerformLayout();
            _tlpToolsBar.ResumeLayout(false);
            _tlpToolsBar.PerformLayout();
            _pnlToolsScroll.ResumeLayout(false);
            _pnlToolsScroll.PerformLayout();
            _tlpToolsStack.ResumeLayout(false);
            _tlpToolsStack.PerformLayout();
            _cardToolsFirstRun.ResumeLayout(false);
            _cardToolsFirstRun.PerformLayout();
            _bodyToolsFirstRun.ResumeLayout(false);
            _bodyToolsFirstRun.PerformLayout();
            _flowToolsEnv.ResumeLayout(false);
            _flowToolsEnv.PerformLayout();
            _cardToolsLib.ResumeLayout(false);
            _cardToolsLib.PerformLayout();
            _bodyToolsLib.ResumeLayout(false);
            _bodyToolsLib.PerformLayout();
            _tlpToolsLibPick.ResumeLayout(false);
            _tlpToolsLibPick.PerformLayout();
            _flowToolsLib.ResumeLayout(false);
            _flowToolsLib.PerformLayout();
            _cardToolsModel.ResumeLayout(false);
            _cardToolsModel.PerformLayout();
            _bodyToolsModel.ResumeLayout(false);
            _bodyToolsModel.PerformLayout();
            _flowToolsModel.ResumeLayout(false);
            _flowToolsModel.PerformLayout();
            _cardToolsModelClasses.ResumeLayout(false);
            _cardToolsModelClasses.PerformLayout();
            _bodyToolsModelClasses.ResumeLayout(false);
            _bodyToolsModelClasses.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)_gridToolsMc).EndInit();
            _flowToolsMc.ResumeLayout(false);
            _flowToolsMc.PerformLayout();
            _cardToolsAnchorLib.ResumeLayout(false);
            _cardToolsAnchorLib.PerformLayout();
            _bodyToolsAnchorLib.ResumeLayout(false);
            _bodyToolsAnchorLib.PerformLayout();
            _tlpToolsAnchorPick.ResumeLayout(false);
            _tlpToolsAnchorPick.PerformLayout();
            _flowToolsAnchorLib.ResumeLayout(false);
            _flowToolsAnchorLib.PerformLayout();
            _cardToolsMachine.ResumeLayout(false);
            _cardToolsMachine.PerformLayout();
            _bodyToolsMachine.ResumeLayout(false);
            _bodyToolsMachine.PerformLayout();
            _tlpToolsMachinePick.ResumeLayout(false);
            _tlpToolsMachinePick.PerformLayout();
            _flowToolsMachine.ResumeLayout(false);
            _flowToolsMachine.PerformLayout();
            _cardToolsDataset.ResumeLayout(false);
            _cardToolsDataset.PerformLayout();
            _bodyToolsDataset.ResumeLayout(false);
            _bodyToolsDataset.PerformLayout();
            _flowToolsDataset.ResumeLayout(false);
            _flowToolsDataset.PerformLayout();
            _cardToolsLabels.ResumeLayout(false);
            _cardToolsLabels.PerformLayout();
            _bodyToolsLabels.ResumeLayout(false);
            _bodyToolsLabels.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)_gridToolsLabels).EndInit();
            _flowToolsLabels.ResumeLayout(false);
            _flowToolsLabels.PerformLayout();
            _cardToolsBackup.ResumeLayout(false);
            _cardToolsBackup.PerformLayout();
            _bodyToolsBackup.ResumeLayout(false);
            _bodyToolsBackup.PerformLayout();
            _flowToolsBackup.ResumeLayout(false);
            _flowToolsBackup.PerformLayout();
            _cardToolsMisdetect.ResumeLayout(false);
            _cardToolsMisdetect.PerformLayout();
            _bodyToolsMisdetect.ResumeLayout(false);
            _bodyToolsMisdetect.PerformLayout();
            _flowToolsMisdetect.ResumeLayout(false);
            _flowToolsMisdetect.PerformLayout();
            _pageFixture.ResumeLayout(false);
            _tlpFixture.ResumeLayout(false);
            _tlpFixture.PerformLayout();
            _tlpFixBar.ResumeLayout(false);
            _tlpFixBar.PerformLayout();
            _flpFixBar.ResumeLayout(false);
            _flpFixBar.PerformLayout();
            _tlpFixBody.ResumeLayout(false);
            _tlpFixSide.ResumeLayout(false);
            _tlpFixSide.PerformLayout();
            _pnlFixVerdict.ResumeLayout(false);
            _pnlFixVerdict.PerformLayout();
            _pnlFixHost.ResumeLayout(false);
            _pnlFixExec.ResumeLayout(false);
            _tlpFixExec.ResumeLayout(false);
            _tlpFixExec.PerformLayout();
            _pnlFixDebug.ResumeLayout(false);
            _tlpFixDebugHost.ResumeLayout(false);
            _tcFixDebug.ResumeLayout(false);
            _fixTabCamera.ResumeLayout(false);
            _flpFixColCamera.ResumeLayout(false);
            _flpFixColCamera.PerformLayout();
            _flpFixRowCam1.ResumeLayout(false);
            _flpFixRowCam1.PerformLayout();
            _flpFixRowCam2.ResumeLayout(false);
            _flpFixRowCam2.PerformLayout();
            _flpFixRowCam3.ResumeLayout(false);
            _flpFixRowCam3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)_numFixExposure).EndInit();
            ((System.ComponentModel.ISupportInitialize)_numFixGain).EndInit();
            _flpFixRowCam4.ResumeLayout(false);
            _fixTabCalib.ResumeLayout(false);
            _flpFixColCalib.ResumeLayout(false);
            _flpFixColCalib.PerformLayout();
            _flpFixRowCalib1.ResumeLayout(false);
            _flpFixRowCalib1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)_numFixCalibMm).EndInit();
            _flpFixRowCalib2.ResumeLayout(false);
            _fixTabTemplate.ResumeLayout(false);
            _flpFixColTemplate.ResumeLayout(false);
            _flpFixColTemplate.PerformLayout();
            _flpFixRowTpl1.ResumeLayout(false);
            _flpFixRowTpl2.ResumeLayout(false);
            _flpFixRowTpl3.ResumeLayout(false);
            _fixTabThreshold.ResumeLayout(false);
            _flpFixColThreshold.ResumeLayout(false);
            _flpFixColThreshold.PerformLayout();
            _flpFixRowJudgeMode.ResumeLayout(false);
            _flpFixRowJudgeMode.PerformLayout();
            _flpFixRowMargin.ResumeLayout(false);
            _flpFixRowMargin.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)_numFixMarginPx).EndInit();
            _flpFixRowOverflow.ResumeLayout(false);
            _flpFixRowOverflow.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)_numFixOverflowPx).EndInit();
            _flpFixRowPos.ResumeLayout(false);
            _flpFixRowPos.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)_numFixPos).EndInit();
            _flpFixRowPosTrack.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)_trkFixPos).EndInit();
            _flpFixRowAngle.ResumeLayout(false);
            _flpFixRowAngle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)_numFixAngle).EndInit();
            _flpFixRowAngleTrack.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)_trkFixAngle).EndInit();
            _flpFixRowShape.ResumeLayout(false);
            _flpFixRowShape.PerformLayout();
            _flpFixRowConfirm.ResumeLayout(false);
            _flpFixRowConfirm.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)_numFixConfirm).EndInit();
            _flpFixRowPreset.ResumeLayout(false);
            _flpFixRowPreset.PerformLayout();
            _flpFixRowSaveTol.ResumeLayout(false);
            _pnlFixAdv.ResumeLayout(false);
            _pnlFixAdv.PerformLayout();
            _flpFixRowDxDy.ResumeLayout(false);
            _flpFixRowDxDy.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)_numFixDx).EndInit();
            ((System.ComponentModel.ISupportInitialize)_numFixDy).EndInit();
            _flpFixRowIouZone.ResumeLayout(false);
            _flpFixRowIouZone.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)_numFixIou).EndInit();
            ((System.ComponentModel.ISupportInitialize)_numFixZone).EndInit();
            ((System.ComponentModel.ISupportInitialize)_numFixTheta).EndInit();
            _flpFixRowArea.ResumeLayout(false);
            _flpFixRowArea.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)_numFixAreaMin).EndInit();
            ((System.ComponentModel.ISupportInitialize)_numFixAreaMax).EndInit();
            _flpFixRowQuality.ResumeLayout(false);
            _flpFixRowQuality.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)_numFixMeanGray).EndInit();
            ((System.ComponentModel.ISupportInitialize)_numFixContrast).EndInit();
            _flpFixRowQuality2.ResumeLayout(false);
            _flpFixRowQuality2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)_numFixHighlight).EndInit();
            ((System.ComponentModel.ISupportInitialize)_numFixContour).EndInit();
            _flpFixRowDrop.ResumeLayout(false);
            _flpFixRowDrop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)_numFixDrop).EndInit();
            _fixTabTest.ResumeLayout(false);
            _flpFixColTest.ResumeLayout(false);
            _flpFixColTest.PerformLayout();
            _flpFixRowTest1.ResumeLayout(false);
            _fixSegTab.ResumeLayout(false);
            _flpFixColSeg.ResumeLayout(false);
            _flpFixColSeg.PerformLayout();
            _flpFixRowSegH.ResumeLayout(false);
            _flpFixRowSegH.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)_numFixSegHMin).EndInit();
            ((System.ComponentModel.ISupportInitialize)_numFixSegHMax).EndInit();
            _flpFixRowSegS.ResumeLayout(false);
            _flpFixRowSegS.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)_numFixSegSMin).EndInit();
            ((System.ComponentModel.ISupportInitialize)_numFixSegSMax).EndInit();
            _flpFixRowSegV.ResumeLayout(false);
            _flpFixRowSegV.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)_numFixSegVMin).EndInit();
            ((System.ComponentModel.ISupportInitialize)_numFixSegVMax).EndInit();
            _flpFixRowSegMorph.ResumeLayout(false);
            _flpFixRowSegMorph.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)_numFixSegMorph).EndInit();
            _flpFixRowSegDiff.ResumeLayout(false);
            _flpFixRowSegDiff.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)_numFixSegDiff).EndInit();
            _flpFixRowSegSave.ResumeLayout(false);
            _pageAnchor.ResumeLayout(false);
            _tlpAnchor.ResumeLayout(false);
            _tlpAnchorTop.ResumeLayout(false);
            _tlpAnchorTop.PerformLayout();
            _tlpAnchorBar.ResumeLayout(false);
            _tlpAnchorBar.PerformLayout();
            _flowAnchorBarModel.ResumeLayout(false);
            _flowAnchorBarModel.PerformLayout();
            _flowAnchorBarCam.ResumeLayout(false);
            _flowAnchorBarCam.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)_numAnchorExposure).EndInit();
            _pnlAnchorVerdict.ResumeLayout(false);
            _tlpAnchorVerdict.ResumeLayout(false);
            _tlpAnchorBody.ResumeLayout(false);
            _tlpAnchorSide.ResumeLayout(false);
            _anchorTabs.ResumeLayout(false);
            _tabAnchorTarget.ResumeLayout(false);
            _hostAnchorLearn.ResumeLayout(false);
            _tlpAnchorLearn.ResumeLayout(false);
            _tlpAnchorLearn.PerformLayout();
            _flowAnchorLearnBar.ResumeLayout(false);
            _flowAnchorLearnBar.PerformLayout();
            _tlpAnchorTrainBar.ResumeLayout(false);
            _flowAnchorTrainParams.ResumeLayout(false);
            _flowAnchorTrainParams.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)_numTrainEpochs).EndInit();
            ((System.ComponentModel.ISupportInitialize)_numTrainImgsz).EndInit();
            _flowAnchorTrainBtns.ResumeLayout(false);
            _flowAnchorTrainBtns.PerformLayout();
            _tabAnchorZone.ResumeLayout(false);
            _hostAnchorZone.ResumeLayout(false);
            _tlpAnchorZone.ResumeLayout(false);
            _tlpAnchorZone.PerformLayout();
            _flowAnchorZoneBar.ResumeLayout(false);
            _flowAnchorZoneBar.PerformLayout();
            _tlpAnchorZoneRef.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)_picZoneRef).EndInit();
            _tlpAnchorZoneParams.ResumeLayout(false);
            _tlpAnchorZoneParams.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)_numZoneMatch).EndInit();
            ((System.ComponentModel.ISupportInitialize)_numZoneSearch).EndInit();
            ((System.ComponentModel.ISupportInitialize)_numZoneShift).EndInit();
            _pnlAnchorTol.ResumeLayout(false);
            _pnlAnchorTol.PerformLayout();
            _tlpAnchorTol.ResumeLayout(false);
            _tlpAnchorTol.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)_numAnchorShift).EndInit();
            ((System.ComponentModel.ISupportInitialize)_numAnchorRot).EndInit();
            ((System.ComponentModel.ISupportInitialize)_numAnchorScale).EndInit();
            ((System.ComponentModel.ISupportInitialize)_numAnchorRms).EndInit();
            ((System.ComponentModel.ISupportInitialize)_numAnchorConf).EndInit();
            ((System.ComponentModel.ISupportInitialize)_numAnchorMin).EndInit();
            _flowAnchorZoneJudge.ResumeLayout(false);
            _flowAnchorZoneJudge.PerformLayout();
            _pnlAnchorHitsCard.ResumeLayout(false);
            _tlpAnchorHitsCard.ResumeLayout(false);
            _tlpAnchorHitsCard.PerformLayout();
            _pnlAnchorLogCard.ResumeLayout(false);
            _tlpAnchorLogCard.ResumeLayout(false);
            _tlpAnchorLogCard.PerformLayout();
            _tlpAnchorZoneHints.ResumeLayout(false);
            _tabAnchorCalib.ResumeLayout(false);
            _tlpAnchorCalib.ResumeLayout(false);
            _tlpAnchorCalib.PerformLayout();
            _flowAnchorCalibBar.ResumeLayout(false);
            _flowAnchorCalibBar.PerformLayout();
            _pnlAnchorCalibCard.ResumeLayout(false);
            _tlpAnchorCalibCard.ResumeLayout(false);
            _tlpAnchorCalibCard.PerformLayout();
            _pageMultiCam.ResumeLayout(false);
            _tlpMultiCam.ResumeLayout(false);
            _tlpMcSide.ResumeLayout(false);
            _tlpMcSide.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)_mcGrid).EndInit();
            _flowMcOpen.ResumeLayout(false);
            _flowMcOpen.PerformLayout();
            _flowMcAll.ResumeLayout(false);
            _flowMcAll.PerformLayout();
            _flowMcIndex.ResumeLayout(false);
            _flowMcIndex.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)_mcNumIndex).EndInit();
            _pnlMcSettings.ResumeLayout(false);
            _pnlMcSettings.PerformLayout();
            _tlpMcSettings.ResumeLayout(false);
            _tlpMcSettings.PerformLayout();
            _flowMcRes.ResumeLayout(false);
            _flowMcRes.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)_mcNumW).EndInit();
            ((System.ComponentModel.ISupportInitialize)_mcNumH).EndInit();
            _flowMcFps.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)_mcNumFps).EndInit();
            _flowMcExp.ResumeLayout(false);
            _flowMcExp.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)_mcNumExp).EndInit();
            _flowMcWb.ResumeLayout(false);
            _flowMcWb.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)_mcNumWb).EndInit();
            ((System.ComponentModel.ISupportInitialize)_mcNumGain).EndInit();
            ((System.ComponentModel.ISupportInitialize)_mcNumBright).EndInit();
            ((System.ComponentModel.ISupportInitialize)_mcNumContrast).EndInit();
            ((System.ComponentModel.ISupportInitialize)_mcNumSat).EndInit();
            _flowMcApply.ResumeLayout(false);
            _flowMcApply.PerformLayout();
            _pnlMcBottom.ResumeLayout(false);
            _pnlMcBottom.PerformLayout();
            pnlStatus.ResumeLayout(false);
            pnlStatus.PerformLayout();
            _splitRec.Panel1.ResumeLayout(false);
            _splitRec.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)_splitRec).EndInit();
            _splitRec.ResumeLayout(false);
            pnlRecLeft.ResumeLayout(false);
            pnlRecSide.ResumeLayout(false);
            tlpSide.ResumeLayout(false);
            tlpSide.PerformLayout();
            tlpSideHead.ResumeLayout(false);
            tlpSideHead.PerformLayout();
            flowResultTools.ResumeLayout(false);
            flowResultTools.PerformLayout();
            tlpSideStatus.ResumeLayout(false);
            tlpSideStatus.PerformLayout();
            _pnlMetrics.ResumeLayout(false);
            _pnlMetrics.PerformLayout();
            tlpMetricActive.ResumeLayout(false);
            tlpMetricActive.PerformLayout();
            tlpMetricTotal.ResumeLayout(false);
            tlpMetricTotal.PerformLayout();
            tlpMetricAB.ResumeLayout(false);
            tlpMetricAB.PerformLayout();
            tlpMetricBA.ResumeLayout(false);
            tlpMetricBA.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)_grid).EndInit();
            this.ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tlpRoot;
        private System.Windows.Forms.TableLayoutPanel pnlHeader;

        private System.Windows.Forms.TableLayoutPanel tlpTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubtitle;
        private System.Windows.Forms.TableLayoutPanel tlpModel;
        private System.Windows.Forms.TabControl _tabs;
        private System.Windows.Forms.TabPage _pageRec;
        private System.Windows.Forms.TableLayoutPanel _tlpLearn;
        private System.Windows.Forms.TableLayoutPanel _tlpLearnBar;
        private System.Windows.Forms.TableLayoutPanel _tlpLearnStack;
        private System.Windows.Forms.Label _lblLearnSource;
        private System.Windows.Forms.TextBox _tbLearnImage;
        private System.Windows.Forms.Button _btnOpenLearn;
        private System.Windows.Forms.Button _btnOpenFolder;
        private System.Windows.Forms.Button _btnClearLearn;
        private System.Windows.Forms.Label _lblLearnHint;
        private System.Windows.Forms.SplitContainer _splitLearn;
        private System.Windows.Forms.Panel _pnlLearnLeft;
        private YoloFool.ImageCanvas _picLearn;
        private System.Windows.Forms.Panel _pnlLearnSide;
        private System.Windows.Forms.Panel _cardImgList;
        private System.Windows.Forms.TableLayoutPanel _cardImgListBody;
        private System.Windows.Forms.Label _lblImgListTitle;
        private System.Windows.Forms.Panel _cardBoxes;
        private System.Windows.Forms.TableLayoutPanel _cardBoxesBody;
        private System.Windows.Forms.Label _lblBoxesTitle;
        private System.Windows.Forms.Panel _cardDataset;
        private System.Windows.Forms.TableLayoutPanel _cardDatasetBody;
        private System.Windows.Forms.Label _lblDatasetTitle;
        private System.Windows.Forms.ListView _lvImages;
        private System.Windows.Forms.ColumnHeader _colLvImage;
        private System.Windows.Forms.ColumnHeader _colLvAnno;
        private System.Windows.Forms.ColumnHeader _colLvStatus;
        private System.Windows.Forms.FlowLayoutPanel _flowImgList;
        private System.Windows.Forms.Button _btnPrevImage;
        private System.Windows.Forms.Button _btnNextImage;
        private System.Windows.Forms.FlowLayoutPanel _flowAuto;
        private System.Windows.Forms.Button _btnAutoLabel;
        private System.Windows.Forms.Button _btnReview;
        private System.Windows.Forms.Button _btnClearAuto;
        private System.Windows.Forms.Label _lblImageListInfo;
        private System.Windows.Forms.Label _lblReviewHint;
        private System.Windows.Forms.ListView _lvBoxes;
        private System.Windows.Forms.ColumnHeader _colBoxName;
        private System.Windows.Forms.ColumnHeader _colBoxEn;
        private System.Windows.Forms.ColumnHeader _colBoxRect;
        private System.Windows.Forms.FlowLayoutPanel _flowBoxOps;
        private System.Windows.Forms.Button _btnDelBox;
        private System.Windows.Forms.Button _btnRenameBox;
        private System.Windows.Forms.Button _btnReexport;
        private System.Windows.Forms.Label _lblAnnoInfo;
        private System.Windows.Forms.Label _lblDatasetStats;
        private System.Windows.Forms.Button _lnkOpenDataset;
        private System.Windows.Forms.Panel _cardTrain;
        private System.Windows.Forms.TableLayoutPanel _cardTrainBody;
        private System.Windows.Forms.Label _lblTrainTitle;
        private System.Windows.Forms.TableLayoutPanel _tlpTrainGrid;
        private System.Windows.Forms.TableLayoutPanel _tlpTrainLeft;
        private System.Windows.Forms.Label _lblEpochs;
        private System.Windows.Forms.NumericUpDown _numEpochs;
        private System.Windows.Forms.Label _lblImgSize;
        private System.Windows.Forms.ComboBox _cboImgSize;
        private System.Windows.Forms.Label _lblBatch;
        private System.Windows.Forms.NumericUpDown _numBatch;
        private System.Windows.Forms.FlowLayoutPanel _flowTrainBtns;
        private System.Windows.Forms.Button _btnInitEnv;
        private System.Windows.Forms.Button _btnTrain;
        private System.Windows.Forms.Button _btnCancelTrain;
        private System.Windows.Forms.TableLayoutPanel _tlpTrainRight;
        private System.Windows.Forms.Label _lblTrainStage;
        private System.Windows.Forms.ProgressBar _pbTrain;
        private System.Windows.Forms.TextBox _txtTrainLog;
        private System.Windows.Forms.TableLayoutPanel tlpCapBody;
        private System.Windows.Forms.SplitContainer _splitRec;
        private System.Windows.Forms.Panel pnlRecLeft;
        private YoloFool.ImageCanvas _picRec;
        private System.Windows.Forms.Panel pnlRecSide;
        private System.Windows.Forms.TableLayoutPanel tlpSide;
        private System.Windows.Forms.TableLayoutPanel tlpSideHead;
        private System.Windows.Forms.Label _lblResultTitle;
        private System.Windows.Forms.FlowLayoutPanel flowResultTools;
        private System.Windows.Forms.Label _lblFilterState;
        private System.Windows.Forms.Button _btnFilter;
        private System.Windows.Forms.Button _btnBlockClass;
        private System.Windows.Forms.Button _btnDeleteResult;
        private System.Windows.Forms.TableLayoutPanel tlpSideStatus;
        private System.Windows.Forms.Label _lblResultStats;
        private System.Windows.Forms.Label _lblVideoArea;
        private System.Windows.Forms.TableLayoutPanel _pnlMetrics;
        private System.Windows.Forms.TableLayoutPanel tlpMetricActive;
        private System.Windows.Forms.TableLayoutPanel tlpMetricTotal;
        private System.Windows.Forms.TableLayoutPanel tlpMetricAB;
        private System.Windows.Forms.TableLayoutPanel tlpMetricBA;
        private System.Windows.Forms.Label lblMetricActiveTitle;
        private System.Windows.Forms.Label lblMetricTotalTitle;
        private System.Windows.Forms.Label lblMetricABTitle;
        private System.Windows.Forms.Label lblMetricBATitle;
        private System.Windows.Forms.Label _lblVideoActive;
        private System.Windows.Forms.Label _lblVideoTotal;
        private System.Windows.Forms.Label _lblVideoAtoB;
        private System.Windows.Forms.Label _lblVideoBtoA;
        private System.Windows.Forms.DataGridView _grid;
        private System.Windows.Forms.DataGridViewTextBoxColumn colId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colConf;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPos;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCenter;
        private System.Windows.Forms.DataGridViewTextBoxColumn colArea;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPct;
        private System.Windows.Forms.Label lblGridHint;
        private System.Windows.Forms.TableLayoutPanel tlpCap;
        private System.Windows.Forms.TableLayoutPanel tlpCapBar;
        private System.Windows.Forms.Label lblCapSource;
        private System.Windows.Forms.TableLayoutPanel tlpVideoOpt;
        private System.Windows.Forms.FlowLayoutPanel flowVideo1;
        private System.Windows.Forms.FlowLayoutPanel flowVideo2;
        private System.Windows.Forms.Label lblVideoStep;
        private System.Windows.Forms.Label lblVideoTask;
        private System.Windows.Forms.Label lblVideoConf;
        private System.Windows.Forms.Label lblVideoClasses;
        private System.Windows.Forms.Label _lblVideoLine;
        private System.Windows.Forms.Panel _pnlAdv;
        private System.Windows.Forms.TableLayoutPanel tlpAdvGrid;
        private System.Windows.Forms.TableLayoutPanel tlpAdvPick;
        private System.Windows.Forms.Label lblAdvModel;
        private System.Windows.Forms.Label lblAdvKind;
        private System.Windows.Forms.Label lblAdvDevice;
        private System.Windows.Forms.Label lblAdvProvider;
        private System.Windows.Forms.Label lblAdvThr;
        private System.Windows.Forms.Label lblAdvNms;
        private System.Windows.Forms.Label _lblAdvInfo;
        private System.Windows.Forms.Label _lblAdvProbe;
        private System.Windows.Forms.Label lblAdvFooterHint;
        private System.Windows.Forms.FlowLayoutPanel flowAdvThr;
        private System.Windows.Forms.TableLayoutPanel tlpAdvFooter;
        private System.Windows.Forms.FlowLayoutPanel flowAdvFooter;
        private System.Windows.Forms.TextBox _tbRecImage;
        private System.Windows.Forms.Button _btnSelectImage;
        private System.Windows.Forms.Button _btnCamera;
        private System.Windows.Forms.Button _btnPickVideo;
        private System.Windows.Forms.Button _btnSnapshot;
        private System.Windows.Forms.Button _btnRecord;
        private System.Windows.Forms.Button _btnStartVideo;
        private System.Windows.Forms.Button _btnStopVideo;
        private System.Windows.Forms.Button _btnClear;
        private System.Windows.Forms.Button _btnToggleAdv;
        private System.Windows.Forms.CheckBox _chkVideoLoop;
        private System.Windows.Forms.NumericUpDown _numVideoStep;
        private System.Windows.Forms.Button _btnDrawLine;
        private System.Windows.Forms.Button _btnClearLine;
        private System.Windows.Forms.ComboBox _cboVideoTask;
        private System.Windows.Forms.NumericUpDown _numVideoConf;
        private System.Windows.Forms.TextBox _tbVideoClasses;
        private System.Windows.Forms.CheckBox _chkVideoBest;
        private System.Windows.Forms.Button _btnVideoModel;
        private System.Windows.Forms.Label _lblVideoModel;
        private System.Windows.Forms.Label _lblVideoInfo;
        private System.Windows.Forms.TextBox _tbAdvModel;
        private System.Windows.Forms.Button _btnAdvModel;
        private System.Windows.Forms.ComboBox _cboKind;
        private System.Windows.Forms.ComboBox _cboDevice;
        private System.Windows.Forms.ComboBox _cboProvider;
        private System.Windows.Forms.NumericUpDown _numConf;
        private System.Windows.Forms.NumericUpDown _numNms;
        private System.Windows.Forms.Button _btnVerifySave;
        private System.Windows.Forms.Button _btnResetAdv;
        private System.Windows.Forms.TabPage _pageLearn;
        private System.Windows.Forms.TabPage _pageTools;
        private System.Windows.Forms.TabPage _pageFixture;
        private System.Windows.Forms.TabPage _pageAnchor;
        private System.Windows.Forms.TabPage _pageMultiCam;
        private System.Windows.Forms.TableLayoutPanel pnlStatus;
        private System.Windows.Forms.Label _lblModelStatus;
        private System.Windows.Forms.Label _lblModelSub;
        private System.Windows.Forms.Button _btnSelfCheck;
        private System.Windows.Forms.Label _lblStatus;
        private System.Windows.Forms.Label _lblStatusRight;
        private System.Windows.Forms.TableLayoutPanel _tlpTools;
        private System.Windows.Forms.TableLayoutPanel _tlpToolsBar;
        private System.Windows.Forms.Label _lblToolsTitle;
        private System.Windows.Forms.Label _lblToolsHint;
        private System.Windows.Forms.Button _btnToolsRefresh;
        private System.Windows.Forms.Panel _pnlToolsScroll;
        private System.Windows.Forms.TableLayoutPanel _tlpToolsStack;
        private System.Windows.Forms.Panel _cardToolsFirstRun;
        private System.Windows.Forms.TableLayoutPanel _bodyToolsFirstRun;
        private System.Windows.Forms.Label _lblToolsFirstRunTitle;
        private System.Windows.Forms.Label _lblToolsFirstRunInfo;
        private System.Windows.Forms.ListView _lvToolsEnv;
        private System.Windows.Forms.CheckBox _chkToolsEnvTraining;
        private System.Windows.Forms.FlowLayoutPanel _flowToolsEnv;
        private System.Windows.Forms.Button _btnToolsEnvCheck;
        private System.Windows.Forms.Button _btnToolsEnvInit;
        private System.Windows.Forms.Button _btnToolsEnvCamera;
        private System.Windows.Forms.Label _lblToolsEnvInfo;
        private System.Windows.Forms.Panel _cardToolsLib;
        private System.Windows.Forms.TableLayoutPanel _bodyToolsLib;
        private System.Windows.Forms.Label _lblToolsLibTitle;
        private System.Windows.Forms.Label _lblToolsLibInfo;
        private System.Windows.Forms.TableLayoutPanel _tlpToolsLibPick;
        private System.Windows.Forms.ComboBox _cboToolsLibCategory;
        private System.Windows.Forms.Button _btnToolsLibFolderOpen;
        private System.Windows.Forms.ListView _lvToolsLibModels;
        private System.Windows.Forms.FlowLayoutPanel _flowToolsLib;
        private System.Windows.Forms.Button _btnToolsLibUse;
        private System.Windows.Forms.Button _btnToolsLibOpen;
        private System.Windows.Forms.Label _lblToolsLibDir;
        private System.Windows.Forms.Panel _cardToolsModel;
        private System.Windows.Forms.TableLayoutPanel _bodyToolsModel;
        private System.Windows.Forms.Label _lblToolsModelTitle;
        private System.Windows.Forms.Label _lblToolsModelMain;
        private System.Windows.Forms.Label _lblToolsModelMeta;
        private System.Windows.Forms.Label _lblToolsModelClasses;
        private System.Windows.Forms.Label _lblToolsModelTrain;
        private System.Windows.Forms.FlowLayoutPanel _flowToolsModel;
        private System.Windows.Forms.Button _btnToolsUseModel;
        private System.Windows.Forms.Button _btnToolsModelBackup;
        private System.Windows.Forms.Button _btnToolsModelFolder;
        private System.Windows.Forms.Panel _cardToolsModelClasses;
        private System.Windows.Forms.TableLayoutPanel _bodyToolsModelClasses;
        private System.Windows.Forms.Label _lblToolsMcTitle;
        private System.Windows.Forms.Label _lblToolsMcInfo;
        private System.Windows.Forms.DataGridView _gridToolsMc;
        private System.Windows.Forms.Label _lblToolsMcHint;
        private System.Windows.Forms.FlowLayoutPanel _flowToolsMc;
        private System.Windows.Forms.Button _btnToolsMcRename;
        private System.Windows.Forms.Button _btnToolsMcDelete;
        private System.Windows.Forms.Button _btnToolsMcMute;
        private System.Windows.Forms.Button _btnToolsMcTrain;
        private System.Windows.Forms.Panel _cardToolsAnchorLib;
        private System.Windows.Forms.TableLayoutPanel _bodyToolsAnchorLib;
        private System.Windows.Forms.Label _lblToolsAnchorLibTitle;
        private System.Windows.Forms.TableLayoutPanel _tlpToolsAnchorPick;
        private System.Windows.Forms.ComboBox _cboToolsAnchorModel;
        private System.Windows.Forms.Label _lblToolsAnchorStats;
        private System.Windows.Forms.Label _lblToolsAnchorModelInfo;
        private System.Windows.Forms.Label _lblToolsAnchorCalibInfo;
        private System.Windows.Forms.FlowLayoutPanel _flowToolsAnchorLib;
        private System.Windows.Forms.Button _btnToolsAnchorNew;
        private System.Windows.Forms.Button _btnToolsAnchorCopy;
        private System.Windows.Forms.Button _btnToolsAnchorTrain;
        private System.Windows.Forms.Button _btnToolsAnchorFolder;
        private System.Windows.Forms.Button _btnToolsAnchorArchive;
        private System.Windows.Forms.Panel _cardToolsMachine;
        private System.Windows.Forms.TableLayoutPanel _bodyToolsMachine;
        private System.Windows.Forms.Label _lblToolsMachineTitle;
        private System.Windows.Forms.TableLayoutPanel _tlpToolsMachinePick;
        private System.Windows.Forms.ComboBox _cboToolsMachineKind;
        private System.Windows.Forms.Label _lblToolsMachineInfo;
        private System.Windows.Forms.ListView _lvToolsMachines;
        private System.Windows.Forms.FlowLayoutPanel _flowToolsMachine;
        private System.Windows.Forms.Button _btnToolsMachineNew;
        private System.Windows.Forms.Button _btnToolsMachineRename;
        private System.Windows.Forms.Button _btnToolsMachineArchive;
        private System.Windows.Forms.Button _btnToolsMachineFolder;
        private System.Windows.Forms.Panel _cardToolsDataset;
        private System.Windows.Forms.TableLayoutPanel _bodyToolsDataset;
        private System.Windows.Forms.Label _lblToolsDatasetTitle;
        private System.Windows.Forms.Label _lblToolsDatasetStats;
        private System.Windows.Forms.Label _lblToolsDatasetClasses;
        private System.Windows.Forms.Label _lblToolsDatasetExtra;
        private System.Windows.Forms.FlowLayoutPanel _flowToolsDataset;
        private System.Windows.Forms.Button _btnToolsDatasetRebuild;
        private System.Windows.Forms.Button _btnToolsDatasetFolder;
        private System.Windows.Forms.Panel _cardToolsLabels;
        private System.Windows.Forms.TableLayoutPanel _bodyToolsLabels;
        private System.Windows.Forms.Label _lblToolsLabelsTitle;
        private System.Windows.Forms.Label _lblToolsLabelsInfo;
        private System.Windows.Forms.DataGridView _gridToolsLabels;
        private System.Windows.Forms.Label _lblToolsLabelHint;
        private System.Windows.Forms.FlowLayoutPanel _flowToolsLabels;
        private System.Windows.Forms.Button _btnToolsLabelView;
        private System.Windows.Forms.Button _btnToolsLabelDelete;
        private System.Windows.Forms.Panel _cardToolsBackup;
        private System.Windows.Forms.TableLayoutPanel _bodyToolsBackup;
        private System.Windows.Forms.Label _lblToolsBackupTitle;
        private System.Windows.Forms.ListBox _lvToolsBackups;
        private System.Windows.Forms.Label _lblToolsBackupInfo;
        private System.Windows.Forms.FlowLayoutPanel _flowToolsBackup;
        private System.Windows.Forms.Button _btnToolsBackupRestore;
        private System.Windows.Forms.Button _btnToolsBackupDelete;
        private System.Windows.Forms.Button _btnToolsBackupFolder;
        private System.Windows.Forms.Panel _cardToolsMisdetect;
        private System.Windows.Forms.TableLayoutPanel _bodyToolsMisdetect;
        private System.Windows.Forms.Label _lblToolsMisdetectTitle;
        private System.Windows.Forms.Label _lblToolsMisdetect;
        private System.Windows.Forms.FlowLayoutPanel _flowToolsMisdetect;
        private System.Windows.Forms.Button _btnToolsMisdetectFolder;
        private System.Windows.Forms.Button _btnToolsMisdetectClear;
        private System.Windows.Forms.TableLayoutPanel _tlpMultiCam;
        private System.Windows.Forms.FlowLayoutPanel _mcWall;
        private System.Windows.Forms.TableLayoutPanel _tlpMcSide;
        private System.Windows.Forms.Label _lblMcTitle;
        private System.Windows.Forms.DataGridView _mcGrid;
        private System.Windows.Forms.FlowLayoutPanel _flowMcOpen;
        private System.Windows.Forms.Button _btnMcOpen;
        private System.Windows.Forms.Button _btnMcClose;
        private System.Windows.Forms.FlowLayoutPanel _flowMcAll;
        private System.Windows.Forms.Button _btnMcAllOn;
        private System.Windows.Forms.Button _btnMcAllOff;
        private System.Windows.Forms.FlowLayoutPanel _flowMcIndex;
        private System.Windows.Forms.Label _lblMcIndexHint;
        private System.Windows.Forms.NumericUpDown _mcNumIndex;
        private System.Windows.Forms.Button _btnMcAdd;
        private System.Windows.Forms.Button _btnMcRemove;
        private System.Windows.Forms.Label _mcSelInfo;
        private System.Windows.Forms.Panel _pnlMcSettings;
        private System.Windows.Forms.TableLayoutPanel _tlpMcSettings;
        private System.Windows.Forms.Label _lblMcSettingsTitle;
        private System.Windows.Forms.Label _lblMcRes;
        private System.Windows.Forms.ComboBox _mcCmbRes;
        private System.Windows.Forms.NumericUpDown _mcNumW;
        private System.Windows.Forms.Label _lblMcTimes;
        private System.Windows.Forms.NumericUpDown _mcNumH;
        private System.Windows.Forms.FlowLayoutPanel _flowMcRes;
        private System.Windows.Forms.Label _lblMcFps;
        private System.Windows.Forms.NumericUpDown _mcNumFps;
        private System.Windows.Forms.ComboBox _mcCmbFmt;
        private System.Windows.Forms.FlowLayoutPanel _flowMcFps;
        private System.Windows.Forms.Label _lblMcExp;
        private System.Windows.Forms.CheckBox _mcChkAutoExp;
        private System.Windows.Forms.NumericUpDown _mcNumExp;
        private System.Windows.Forms.FlowLayoutPanel _flowMcExp;
        private System.Windows.Forms.Label _lblMcWb;
        private System.Windows.Forms.CheckBox _mcChkAutoWb;
        private System.Windows.Forms.NumericUpDown _mcNumWb;
        private System.Windows.Forms.FlowLayoutPanel _flowMcWb;
        private System.Windows.Forms.Label _lblMcGain;
        private System.Windows.Forms.NumericUpDown _mcNumGain;
        private System.Windows.Forms.Label _lblMcBright;
        private System.Windows.Forms.NumericUpDown _mcNumBright;
        private System.Windows.Forms.Label _lblMcContrast;
        private System.Windows.Forms.NumericUpDown _mcNumContrast;
        private System.Windows.Forms.Label _lblMcSat;
        private System.Windows.Forms.NumericUpDown _mcNumSat;
        private System.Windows.Forms.FlowLayoutPanel _flowMcApply;
        private System.Windows.Forms.Button _mcBtnApply;
        private System.Windows.Forms.Label _mcLblActual;
        private System.Windows.Forms.Label _lblMcSettingsNote;
        private System.Windows.Forms.Label _lblMcProcTitle;
        private System.Windows.Forms.Panel _pnlMcBottom;
        private System.Windows.Forms.Label _lblMcHint;
        private System.Windows.Forms.Label _mcProcInfo;
// ================= 针床定位 V2 页（tlpAnchor）字段 =================
        private System.Windows.Forms.TableLayoutPanel _tlpAnchor;
        private System.Windows.Forms.TableLayoutPanel _tlpAnchorTop;
        private System.Windows.Forms.TableLayoutPanel _tlpAnchorBar;
        private System.Windows.Forms.FlowLayoutPanel _flowAnchorBarModel;
        private System.Windows.Forms.Label _lblAnchorBarModelCap;
        private System.Windows.Forms.ComboBox _cboAnchorModel;
        private System.Windows.Forms.Button _btnAnchorNewModel;
        private System.Windows.Forms.Button _btnAnchorCopyModel;
        private System.Windows.Forms.Label _lblAnchorModelInfo;
        private System.Windows.Forms.FlowLayoutPanel _flowAnchorBarCam;
        private System.Windows.Forms.Label _lblAnchorBarCamCap;
        private System.Windows.Forms.ComboBox _cboAnchorCamIndex;
        private System.Windows.Forms.ComboBox _cboAnchorRes;
        private System.Windows.Forms.CheckBox _chkAnchorExposure;
        private System.Windows.Forms.NumericUpDown _numAnchorExposure;
        private System.Windows.Forms.Button _btnAnchorShot;
        private System.Windows.Forms.Button _btnAnchorCam;
        private System.Windows.Forms.Button _btnAnchorJudge;
        private System.Windows.Forms.Label _lblAnchorCamInfo;
        private System.Windows.Forms.Panel _pnlAnchorVerdict;
        private System.Windows.Forms.TableLayoutPanel _tlpAnchorVerdict;
        private System.Windows.Forms.Label _lblAnchorVerdict;
        private System.Windows.Forms.Label _lblAnchorReason;
        private System.Windows.Forms.Label _lblAnchorDetail;
        private System.Windows.Forms.TableLayoutPanel _tlpAnchorBody;
        private YoloFool.ImageCanvas _picAnchor;
        private System.Windows.Forms.TableLayoutPanel _tlpAnchorSide;
        private System.Windows.Forms.TabControl _anchorTabs;
        private System.Windows.Forms.TabPage _tabAnchorTarget;
        private System.Windows.Forms.TabPage _tabAnchorZone;
        private System.Windows.Forms.TabPage _tabAnchorCalib;
        private System.Windows.Forms.Panel _hostAnchorLearn;
        private System.Windows.Forms.TableLayoutPanel _tlpAnchorLearn;
        private System.Windows.Forms.FlowLayoutPanel _flowAnchorLearnBar;
        private System.Windows.Forms.Button _btnAnchorLearnPick;
        private System.Windows.Forms.Button _btnAnchorLearnDel;
        private System.Windows.Forms.Button _btnAnchorLearnDelShot;
        private System.Windows.Forms.Button _btnAnchorLearnLive;
        private System.Windows.Forms.Label _lblAnchorLearnInfo;
        private System.Windows.Forms.ListView _lvAnchorDrafts;
        private System.Windows.Forms.ColumnHeader _colAnchorDraft;
        private System.Windows.Forms.ColumnHeader _colAnchorDraftPos;
        private System.Windows.Forms.ColumnHeader _colAnchorDraftAngle;
        private System.Windows.Forms.ListView _lvAnchorSamples;
        private System.Windows.Forms.ColumnHeader _colAnchorSample;
        private System.Windows.Forms.ColumnHeader _colAnchorSampleTarget;
        private System.Windows.Forms.ColumnHeader _colAnchorSampleBoxes;
        private System.Windows.Forms.ColumnHeader _colAnchorSampleTime;
        private System.Windows.Forms.Label _lblAnchorSampleInfo;
        private YoloFool.ImageCanvas _picAnchorSample;
        private System.Windows.Forms.TableLayoutPanel _tlpAnchorTrainBar;
        private System.Windows.Forms.FlowLayoutPanel _flowAnchorTrainParams;
        private System.Windows.Forms.Label _lblAnchorEpochsCap;
        private YoloFool.NoWheelNumeric _numTrainEpochs;
        private System.Windows.Forms.Label _lblAnchorImgszCap;
        private YoloFool.NoWheelNumeric _numTrainImgsz;
        private System.Windows.Forms.FlowLayoutPanel _flowAnchorTrainBtns;
        private System.Windows.Forms.Button _btnAnchorTrain;
        private System.Windows.Forms.Button _btnAnchorTrainRefresh;
        private System.Windows.Forms.TextBox _txtAnchorTrainLog;
        private System.Windows.Forms.Label _lblAnchorClasses;
        private System.Windows.Forms.Label _lblAnchorTargetInfo;
        private System.Windows.Forms.Panel _hostAnchorZone;
        private System.Windows.Forms.TableLayoutPanel _tlpAnchorZone;
        private System.Windows.Forms.FlowLayoutPanel _flowAnchorZoneBar;
        private System.Windows.Forms.Button _btnAnchorZonePick;
        private System.Windows.Forms.Button _btnAnchorZoneFromLabel;
        private System.Windows.Forms.Button _btnAnchorZoneCapture;
        private System.Windows.Forms.Button _btnAnchorZoneEdit;
        private System.Windows.Forms.Button _btnAnchorZoneRename;
        private System.Windows.Forms.Button _btnAnchorZoneRemove;
        private System.Windows.Forms.ListView _lvAnchorZones;
        private System.Windows.Forms.ColumnHeader _colAnchorZoneName;
        private System.Windows.Forms.ColumnHeader _colAnchorZoneLabel;
        private System.Windows.Forms.ColumnHeader _colAnchorZoneMatch;
        private System.Windows.Forms.ColumnHeader _colAnchorZoneShift;
        private System.Windows.Forms.ColumnHeader _colAnchorZoneState;
        private System.Windows.Forms.TableLayoutPanel _tlpAnchorZoneRef;
        private System.Windows.Forms.PictureBox _picZoneRef;
        private System.Windows.Forms.Label _lblZoneRefInfo;
        private System.Windows.Forms.TableLayoutPanel _tlpAnchorZoneParams;
        private System.Windows.Forms.Label _lblZoneMatchCap;
        private YoloFool.NoWheelNumeric _numZoneMatch;
        private System.Windows.Forms.Label _lblZoneMatchTip;
        private System.Windows.Forms.Label _lblZoneSearchCap;
        private YoloFool.NoWheelNumeric _numZoneSearch;
        private System.Windows.Forms.Label _lblZoneSearchTip;
        private System.Windows.Forms.Label _lblZoneShiftCap;
        private YoloFool.NoWheelNumeric _numZoneShift;
        private System.Windows.Forms.Label _lblZoneShiftTip;
        private System.Windows.Forms.Label _lblZoneClassCap;
        private System.Windows.Forms.ComboBox _cboZoneClass;
        private System.Windows.Forms.Label _lblZoneClassTip;
        private System.Windows.Forms.CheckBox _chkZoneRequired;
        private System.Windows.Forms.CheckBox _chkAnchorAdv;
        private System.Windows.Forms.Panel _pnlAnchorTol;
        private System.Windows.Forms.TableLayoutPanel _tlpAnchorTol;
        private System.Windows.Forms.Label _lblAnchorTolShift;
        private YoloFool.NoWheelNumeric _numAnchorShift;
        private System.Windows.Forms.Label _lblAnchorTolRot;
        private YoloFool.NoWheelNumeric _numAnchorRot;
        private System.Windows.Forms.Label _lblAnchorTolScale;
        private YoloFool.NoWheelNumeric _numAnchorScale;
        private System.Windows.Forms.Label _lblAnchorTolRms;
        private YoloFool.NoWheelNumeric _numAnchorRms;
        private System.Windows.Forms.Label _lblAnchorTolConf;
        private YoloFool.NoWheelNumeric _numAnchorConf;
        private System.Windows.Forms.Label _lblAnchorTolMin;
        private YoloFool.NoWheelNumeric _numAnchorMin;
        private System.Windows.Forms.FlowLayoutPanel _flowAnchorZoneJudge;
        private System.Windows.Forms.Button _btnAnchorZoneSaveTol;
        private System.Windows.Forms.Button _btnAnchorZoneOverlay;
        private System.Windows.Forms.Panel _pnlAnchorHitsCard;
        private System.Windows.Forms.TableLayoutPanel _tlpAnchorHitsCard;
        private System.Windows.Forms.Label _lblAnchorHitsTitle;
        private System.Windows.Forms.ListView _lvAnchorHits;
        private System.Windows.Forms.ColumnHeader _colAnchorHitName;
        private System.Windows.Forms.ColumnHeader _colAnchorHitBase;
        private System.Windows.Forms.ColumnHeader _colAnchorHitActual;
        private System.Windows.Forms.ColumnHeader _colAnchorHitDelta;
        private System.Windows.Forms.ColumnHeader _colAnchorHitConf;
        private System.Windows.Forms.Panel _pnlAnchorLogCard;
        private System.Windows.Forms.TableLayoutPanel _tlpAnchorLogCard;
        private System.Windows.Forms.Label _lblAnchorLogTitle;
        private System.Windows.Forms.ListView _lvAnchorLog;
        private System.Windows.Forms.ColumnHeader _colAnchorLogTime;
        private System.Windows.Forms.ColumnHeader _colAnchorLogVerdict;
        private System.Windows.Forms.ColumnHeader _colAnchorLogReason;
        private System.Windows.Forms.ColumnHeader _colAnchorLogDelta;
        private System.Windows.Forms.TableLayoutPanel _tlpAnchorZoneHints;
        private System.Windows.Forms.Label _lblAnchorZoneInfo;
        private System.Windows.Forms.Label _lblAnchorCalibInfo;
        private System.Windows.Forms.Label _lblAnchorJitter;
        private System.Windows.Forms.TableLayoutPanel _tlpAnchorCalib;
        private System.Windows.Forms.Label _lblAnchorCalibHint;
        private System.Windows.Forms.FlowLayoutPanel _flowAnchorCalibBar;
        private System.Windows.Forms.Button _btnAnchorCalib;
        private System.Windows.Forms.Button _btnAnchorCalibView;
        private System.Windows.Forms.Button _btnAnchorCalibCompare;
        private System.Windows.Forms.Button _btnAnchorCalibLive;
        private System.Windows.Forms.Label _lblAnchorCalibCompare;
        private System.Windows.Forms.Panel _pnlAnchorCalibCard;
        private System.Windows.Forms.TableLayoutPanel _tlpAnchorCalibCard;
        private System.Windows.Forms.Label _lblAnchorCalibHitsTitle;
        private System.Windows.Forms.ListView _lvAnchorCalibHits;
        private System.Windows.Forms.ColumnHeader _colAnchorCalibName;
        private System.Windows.Forms.ColumnHeader _colAnchorCalibBase;
        private System.Windows.Forms.ColumnHeader _colAnchorCalibActual;
        private System.Windows.Forms.ColumnHeader _colAnchorCalibDx;
        private System.Windows.Forms.ColumnHeader _colAnchorCalibDy;
        private System.Windows.Forms.ColumnHeader _colAnchorCalibConf;
// ================= end 针床定位 V2 页（tlpAnchor）字段 =================
// ================= 针床检测页（tlpFixture）字段 =================
        private System.Windows.Forms.TableLayoutPanel _tlpFixture;
        private System.Windows.Forms.TableLayoutPanel _tlpFixBar;
        private System.Windows.Forms.FlowLayoutPanel _flpFixBar;
        private System.Windows.Forms.Label _lblFixModel;
        private System.Windows.Forms.ComboBox _cboFixModel;
        private System.Windows.Forms.Button _btnFixNewModel;
        private System.Windows.Forms.Button _btnFixMode;
        private System.Windows.Forms.Button _btnFixCamApply;
        private System.Windows.Forms.Button _btnFixCamClose;
        private System.Windows.Forms.Button _btnFixSnapshot;
        private System.Windows.Forms.Button _btnFixLive;
        private System.Windows.Forms.Button _btnFixOverlay;
        private System.Windows.Forms.Label _lblFixCamInfo;
        private System.Windows.Forms.TableLayoutPanel _tlpFixBody;
        private YoloFool.ImageCanvas _picFixture;
        private System.Windows.Forms.TableLayoutPanel _tlpFixSide;
        private System.Windows.Forms.Panel _pnlFixVerdict;
        private System.Windows.Forms.Label _lblFixVerdict;
        private System.Windows.Forms.Label _lblFixReason;
        private System.Windows.Forms.Panel _pnlFixHost;
        private System.Windows.Forms.Panel _pnlFixExec;
        private System.Windows.Forms.Panel _pnlFixDebug;
        private System.Windows.Forms.TableLayoutPanel _tlpFixExec;
        private System.Windows.Forms.Button _btnFixJudge;
        private System.Windows.Forms.Label _lblFixExecHint;
        private System.Windows.Forms.Button _btnFixReplay2;
        private System.Windows.Forms.TableLayoutPanel _tlpFixDebugHost;
        private System.Windows.Forms.FlowLayoutPanel _flpFixThumbs;
        private System.Windows.Forms.TabControl _tcFixDebug;
        private System.Windows.Forms.ListView _lvFixLog;
        private System.Windows.Forms.ColumnHeader _colFixLogTime;
        private System.Windows.Forms.ColumnHeader _colFixLogResult;
        private System.Windows.Forms.ColumnHeader _colFixLogReason;
        private System.Windows.Forms.ColumnHeader _colFixLogOffset;
        private System.Windows.Forms.ColumnHeader _colFixLogCost;
        private System.Windows.Forms.TabPage _fixTabCamera;
        private System.Windows.Forms.FlowLayoutPanel _flpFixColCamera;
        private System.Windows.Forms.ComboBox _cboFixCamIndex;
        private System.Windows.Forms.ComboBox _cboFixRes;
        private System.Windows.Forms.ComboBox _cboFixFourCC;
        private System.Windows.Forms.Label _lblFixCamIndex;
        private System.Windows.Forms.Label _lblFixCamRes;
        private System.Windows.Forms.FlowLayoutPanel _flpFixRowCam1;
        private System.Windows.Forms.CheckBox _chkFixExposure;
        private System.Windows.Forms.NumericUpDown _numFixExposure;
        private System.Windows.Forms.NumericUpDown _numFixGain;
        private System.Windows.Forms.Label _lblFixCamFormat;
        private System.Windows.Forms.FlowLayoutPanel _flpFixRowCam2;
        private System.Windows.Forms.Label _lblFixExposure;
        private System.Windows.Forms.Label _lblFixGain;
        private System.Windows.Forms.FlowLayoutPanel _flpFixRowCam3;
        private System.Windows.Forms.Label _lblFixExposureHint;
        private System.Windows.Forms.Button _btnFixCamOpenApply;
        private System.Windows.Forms.Button _btnFixCamDriver;
        private System.Windows.Forms.Button _btnFixRoi;
        private System.Windows.Forms.FlowLayoutPanel _flpFixRowCam4;
        private System.Windows.Forms.Label _lblFixRoi;
        private System.Windows.Forms.TabPage _fixTabCalib;
        private System.Windows.Forms.FlowLayoutPanel _flpFixColCalib;
        private System.Windows.Forms.Label _lblFixCalibHint;
        private System.Windows.Forms.Button _btnFixCalibPick;
        private System.Windows.Forms.NumericUpDown _numFixCalibMm;
        private System.Windows.Forms.Label _lblFixCalibMm;
        private System.Windows.Forms.FlowLayoutPanel _flpFixRowCalib1;
        private System.Windows.Forms.Button _btnFixCalibSave;
        private System.Windows.Forms.FlowLayoutPanel _flpFixRowCalib2;
        private System.Windows.Forms.Label _lblFixCalib;
        private System.Windows.Forms.TabPage _fixTabTemplate;
        private System.Windows.Forms.FlowLayoutPanel _flpFixColTemplate;
        private System.Windows.Forms.Label _lblFixTemplateHint;
        private System.Windows.Forms.Button _btnFixCaptureEmpty;
        private System.Windows.Forms.Button _btnFixCaptureGolden;
        private System.Windows.Forms.FlowLayoutPanel _flpFixRowTpl1;
        private System.Windows.Forms.Button _btnFixRebuildSeg;
        private System.Windows.Forms.FlowLayoutPanel _flpFixRowTpl2;
        private System.Windows.Forms.Button _btnFixZoneAuto;
        private System.Windows.Forms.Button _btnFixZoneDraw;
        private System.Windows.Forms.Button _btnFixZoneClear;
        private System.Windows.Forms.FlowLayoutPanel _flpFixRowTpl3;
        private System.Windows.Forms.Label _lblFixZone;
        private System.Windows.Forms.Label _lblFixTemplate;
        private System.Windows.Forms.TabPage _fixTabThreshold;
        private System.Windows.Forms.FlowLayoutPanel _flpFixColThreshold;
        private System.Windows.Forms.ComboBox _cboFixJudgeMode;
        private System.Windows.Forms.Label _lblFixJudgeMode;
        private System.Windows.Forms.FlowLayoutPanel _flpFixRowJudgeMode;
        private System.Windows.Forms.Label _lblFixJudgeModeHint;
        private System.Windows.Forms.NumericUpDown _numFixMarginPx;
        private System.Windows.Forms.NumericUpDown _numFixOverflowPx;
        private System.Windows.Forms.Label _lblFixMargin;
        private System.Windows.Forms.Label _lblFixMarginUnit;
        private System.Windows.Forms.FlowLayoutPanel _flpFixRowMargin;
        private System.Windows.Forms.Label _lblFixOverflow;
        private System.Windows.Forms.Label _lblFixOverflowUnit;
        private System.Windows.Forms.FlowLayoutPanel _flpFixRowOverflow;
        private System.Windows.Forms.NumericUpDown _numFixPos;
        private System.Windows.Forms.Label _lblFixPos;
        private System.Windows.Forms.Label _lblFixPosUnit;
        private System.Windows.Forms.FlowLayoutPanel _flpFixRowPos;
        private System.Windows.Forms.TrackBar _trkFixPos;
        private System.Windows.Forms.FlowLayoutPanel _flpFixRowPosTrack;
        private System.Windows.Forms.Label _lblFixPosHint;
        private System.Windows.Forms.NumericUpDown _numFixAngle;
        private System.Windows.Forms.Label _lblFixAngleNum;
        private System.Windows.Forms.Label _lblFixAngleUnit;
        private System.Windows.Forms.FlowLayoutPanel _flpFixRowAngle;
        private System.Windows.Forms.TrackBar _trkFixAngle;
        private System.Windows.Forms.FlowLayoutPanel _flpFixRowAngleTrack;
        private System.Windows.Forms.Label _lblFixAngleHint;
        private System.Windows.Forms.ComboBox _cboFixShape;
        private System.Windows.Forms.Label _lblFixShape;
        private System.Windows.Forms.FlowLayoutPanel _flpFixRowShape;
        private System.Windows.Forms.Label _lblFixShapeHint;
        private System.Windows.Forms.NumericUpDown _numFixConfirm;
        private System.Windows.Forms.Label _lblFixConfirm;
        private System.Windows.Forms.Label _lblFixConfirmUnit;
        private System.Windows.Forms.FlowLayoutPanel _flpFixRowConfirm;
        private System.Windows.Forms.Label _lblFixConfirmHint;
        private System.Windows.Forms.Button _btnFixPresetLoose;
        private System.Windows.Forms.Button _btnFixPresetStd;
        private System.Windows.Forms.Button _btnFixPresetStrict;
        private System.Windows.Forms.Label _lblFixPreset;
        private System.Windows.Forms.FlowLayoutPanel _flpFixRowPreset;
        private System.Windows.Forms.Label _lblFixTolSummary;
        private System.Windows.Forms.Button _btnFixSaveTol;
        private System.Windows.Forms.Button _btnFixAdvToggle;
        private System.Windows.Forms.FlowLayoutPanel _flpFixRowSaveTol;
        private System.Windows.Forms.FlowLayoutPanel _pnlFixAdv;
        private System.Windows.Forms.NumericUpDown _numFixDx;
        private System.Windows.Forms.NumericUpDown _numFixDy;
        private System.Windows.Forms.NumericUpDown _numFixIou;
        private System.Windows.Forms.NumericUpDown _numFixZone;
        private System.Windows.Forms.NumericUpDown _numFixAreaMin;
        private System.Windows.Forms.NumericUpDown _numFixAreaMax;
        private System.Windows.Forms.NumericUpDown _numFixMeanGray;
        private System.Windows.Forms.NumericUpDown _numFixContrast;
        private System.Windows.Forms.NumericUpDown _numFixHighlight;
        private System.Windows.Forms.NumericUpDown _numFixContour;
        private System.Windows.Forms.NumericUpDown _numFixDrop;
        private System.Windows.Forms.NumericUpDown _numFixTheta;
        private System.Windows.Forms.Label _lblFixDx;
        private System.Windows.Forms.Label _lblFixDy;
        private System.Windows.Forms.Label _lblFixIou;
        private System.Windows.Forms.Label _lblFixZoneMm;
        private System.Windows.Forms.Label _lblFixTheta;
        private System.Windows.Forms.Label _lblFixAreaMin;
        private System.Windows.Forms.Label _lblFixAreaMax;
        private System.Windows.Forms.Label _lblFixMeanGray;
        private System.Windows.Forms.Label _lblFixContrast;
        private System.Windows.Forms.Label _lblFixHighlight;
        private System.Windows.Forms.Label _lblFixContour;
        private System.Windows.Forms.Label _lblFixDrop;
        private System.Windows.Forms.Label _lblFixGoldenScore;
        private System.Windows.Forms.FlowLayoutPanel _flpFixRowDxDy;
        private System.Windows.Forms.FlowLayoutPanel _flpFixRowIouZone;
        private System.Windows.Forms.FlowLayoutPanel _flpFixRowArea;
        private System.Windows.Forms.FlowLayoutPanel _flpFixRowQuality;
        private System.Windows.Forms.FlowLayoutPanel _flpFixRowQuality2;
        private System.Windows.Forms.FlowLayoutPanel _flpFixRowDrop;
        private System.Windows.Forms.Label _lblFixAdvHint;
        private System.Windows.Forms.TabPage _fixTabTest;
        private System.Windows.Forms.FlowLayoutPanel _flpFixColTest;
        private System.Windows.Forms.Button _btnFixTest;
        private System.Windows.Forms.Button _btnFixReplay;
        private System.Windows.Forms.FlowLayoutPanel _flpFixRowTest1;
        private System.Windows.Forms.Label _lblFixTestInfo;
        private System.Windows.Forms.TabPage _fixSegTab;
        private System.Windows.Forms.FlowLayoutPanel _flpFixColSeg;
        private System.Windows.Forms.Label _lblFixSegHint1;
        private System.Windows.Forms.NumericUpDown _numFixSegHMin;
        private System.Windows.Forms.NumericUpDown _numFixSegHMax;
        private System.Windows.Forms.NumericUpDown _numFixSegSMin;
        private System.Windows.Forms.NumericUpDown _numFixSegSMax;
        private System.Windows.Forms.NumericUpDown _numFixSegVMin;
        private System.Windows.Forms.NumericUpDown _numFixSegVMax;
        private System.Windows.Forms.NumericUpDown _numFixSegMorph;
        private System.Windows.Forms.NumericUpDown _numFixSegDiff;
        private System.Windows.Forms.Label _lblFixSegH;
        private System.Windows.Forms.Label _lblFixSegHTilde;
        private System.Windows.Forms.Label _lblFixSegS;
        private System.Windows.Forms.Label _lblFixSegSTilde;
        private System.Windows.Forms.Label _lblFixSegV;
        private System.Windows.Forms.Label _lblFixSegVTilde;
        private System.Windows.Forms.Label _lblFixSegMorph;
        private System.Windows.Forms.Label _lblFixSegMorphUnit;
        private System.Windows.Forms.Label _lblFixSegDiff;
        private System.Windows.Forms.FlowLayoutPanel _flpFixRowSegH;
        private System.Windows.Forms.FlowLayoutPanel _flpFixRowSegS;
        private System.Windows.Forms.FlowLayoutPanel _flpFixRowSegV;
        private System.Windows.Forms.Label _lblFixSegHint2;
        private System.Windows.Forms.FlowLayoutPanel _flpFixRowSegMorph;
        private System.Windows.Forms.Label _lblFixSegHint3;
        private System.Windows.Forms.CheckBox _chkFixSegUseEmpty;
        private System.Windows.Forms.FlowLayoutPanel _flpFixRowSegDiff;
        private System.Windows.Forms.Label _lblFixSegHint4;
        private System.Windows.Forms.Button _btnFixSegEstimate;
        private System.Windows.Forms.Button _btnFixSegApply;
        private System.Windows.Forms.FlowLayoutPanel _flpFixRowSegSave;
        private System.Windows.Forms.Label _lblFixSegInfo;
        private System.Windows.Forms.Label _lblFixSegHint5;
        private ColumnHeader colLvImage;
        private ColumnHeader colLvAnno;
        private ColumnHeader colLvStatus;
        private ColumnHeader colBoxName;
        private ColumnHeader colBoxEn;
        private ColumnHeader colBoxRect;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn12;
        private ColumnHeader colFixLogTime;
        private ColumnHeader colFixLogResult;
        private ColumnHeader colFixLogReason;
        private ColumnHeader colFixLogOffset;
        private ColumnHeader colFixLogCost;
        private ColumnHeader colAnchorDraft;
        private ColumnHeader colAnchorDraftPos;
        private ColumnHeader colAnchorDraftAngle;
        private ColumnHeader colAnchorSample;
        private ColumnHeader colAnchorSampleTarget;
        private ColumnHeader colAnchorSampleBoxes;
        private ColumnHeader colAnchorSampleTime;
        private ColumnHeader colAnchorZoneName;
        private ColumnHeader colAnchorZoneLabel;
        private ColumnHeader colAnchorZoneMatch;
        private ColumnHeader colAnchorZoneShift;
        private ColumnHeader colAnchorZoneState;
        private ColumnHeader colAnchorHitName;
        private ColumnHeader colAnchorHitBase;
        private ColumnHeader colAnchorHitActual;
        private ColumnHeader colAnchorHitDelta;
        private ColumnHeader colAnchorHitConf;
        private ColumnHeader colAnchorLogTime;
        private ColumnHeader colAnchorLogVerdict;
        private ColumnHeader colAnchorLogReason;
        private ColumnHeader colAnchorLogDelta;
        private ColumnHeader colAnchorCalibName;
        private ColumnHeader colAnchorCalibBase;
        private ColumnHeader colAnchorCalibActual;
        private ColumnHeader colAnchorCalibDx;
        private ColumnHeader colAnchorCalibDy;
        private ColumnHeader colAnchorCalibConf;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn13;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn14;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn15;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn16;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn17;
        // ================= end 针床检测页（tlpFixture）字段 =================

    }
}
