namespace Scada.Comm.Ctrl
{
    partial class FrmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Общие параметры");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Библиотеки КП");
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("Линии связи");
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("Статистика");
            this.cmsLine = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miAddLine = new System.Windows.Forms.ToolStripMenuItem();
            this.miMoveUpLine = new System.Windows.Forms.ToolStripMenuItem();
            this.miMoveDownLine = new System.Windows.Forms.ToolStripMenuItem();
            this.miDelLine = new System.Windows.Forms.ToolStripMenuItem();
            this.miImportLines = new System.Windows.Forms.ToolStripMenuItem();
            this.sepLine = new System.Windows.Forms.ToolStripSeparator();
            this.miStartLine = new System.Windows.Forms.ToolStripMenuItem();
            this.miStopLine = new System.Windows.Forms.ToolStripMenuItem();
            this.miRestartLine = new System.Windows.Forms.ToolStripMenuItem();
            this.ilMain = new System.Windows.Forms.ImageList(this.components);
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.cmsNotify = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miNotifyOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.miNotifyStart = new System.Windows.Forms.ToolStripMenuItem();
            this.miNotifyStop = new System.Windows.Forms.ToolStripMenuItem();
            this.miNotifyRestart = new System.Windows.Forms.ToolStripMenuItem();
            this.miNotifySep = new System.Windows.Forms.ToolStripSeparator();
            this.miNotifyExit = new System.Windows.Forms.ToolStripMenuItem();
            this.tmrRefr = new System.Windows.Forms.Timer(this.components);
            this.treeView = new System.Windows.Forms.TreeView();
            this.ilTree = new System.Windows.Forms.ImageList(this.components);
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.lblCurDate = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblCurTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblServiceState = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.btnServiceStart = new System.Windows.Forms.ToolStripButton();
            this.btnServiceStop = new System.Windows.Forms.ToolStripButton();
            this.btnServiceRestart = new System.Windows.Forms.ToolStripButton();
            this.sepMain1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnSettingsApply = new System.Windows.Forms.ToolStripButton();
            this.btnSettingsCancel = new System.Windows.Forms.ToolStripButton();
            this.btnSettingsUpdate = new System.Windows.Forms.ToolStripButton();
            this.sepMain2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnAbout = new System.Windows.Forms.ToolStripButton();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.pageCommonParams = new System.Windows.Forms.TabPage();
            this.gbPerformance = new System.Windows.Forms.GroupBox();
            this.lblSendAllDataPer = new System.Windows.Forms.Label();
            this.numSendAllDataPer = new System.Windows.Forms.NumericUpDown();
            this.numWaitForStop = new System.Windows.Forms.NumericUpDown();
            this.lblWaitForStop = new System.Windows.Forms.Label();
            this.gbServer = new System.Windows.Forms.GroupBox();
            this.lblServerPwd = new System.Windows.Forms.Label();
            this.txtServerPwd = new System.Windows.Forms.TextBox();
            this.txtServerUser = new System.Windows.Forms.TextBox();
            this.lblServerUser = new System.Windows.Forms.Label();
            this.lblServerTimeout = new System.Windows.Forms.Label();
            this.numServerTimeout = new System.Windows.Forms.NumericUpDown();
            this.lblServerPort = new System.Windows.Forms.Label();
            this.numServerPort = new System.Windows.Forms.NumericUpDown();
            this.lblServerHost = new System.Windows.Forms.Label();
            this.chkServerUse = new System.Windows.Forms.CheckBox();
            this.txtServerHost = new System.Windows.Forms.TextBox();
            this.pageKpDlls = new System.Windows.Forms.TabPage();
            this.btnKpDllProps = new System.Windows.Forms.Button();
            this.txtKpDllDescr = new System.Windows.Forms.TextBox();
            this.lblKpDllDescr = new System.Windows.Forms.Label();
            this.lbKpDll = new System.Windows.Forms.ListBox();
            this.pageLineParams = new System.Windows.Forms.TabPage();
            this.gbCommLine = new System.Windows.Forms.GroupBox();
            this.chkLineBind = new System.Windows.Forms.CheckBox();
            this.numLineNumber = new System.Windows.Forms.NumericUpDown();
            this.txtLineName = new System.Windows.Forms.TextBox();
            this.lnlLineName = new System.Windows.Forms.Label();
            this.lblLineNumber = new System.Windows.Forms.Label();
            this.chkLineActive = new System.Windows.Forms.CheckBox();
            this.gbLineParams = new System.Windows.Forms.GroupBox();
            this.chkReqAfterCmd = new System.Windows.Forms.CheckBox();
            this.lblReqAfterCmd = new System.Windows.Forms.Label();
            this.lblDetailedLog = new System.Windows.Forms.Label();
            this.chkDetailedLog = new System.Windows.Forms.CheckBox();
            this.lblCmdEnabled = new System.Windows.Forms.Label();
            this.chkCmdEnabled = new System.Windows.Forms.CheckBox();
            this.lblCycleDelay = new System.Windows.Forms.Label();
            this.numCycleDelay = new System.Windows.Forms.NumericUpDown();
            this.numReqTriesCnt = new System.Windows.Forms.NumericUpDown();
            this.lblReqTriesCnt = new System.Windows.Forms.Label();
            this.gbCommChannel = new System.Windows.Forms.GroupBox();
            this.txtCommCnlParams = new System.Windows.Forms.TextBox();
            this.lblCommCnlParams = new System.Windows.Forms.Label();
            this.btnCommCnlProps = new System.Windows.Forms.Button();
            this.cbCommCnlType = new System.Windows.Forms.ComboBox();
            this.lblCommCnlType = new System.Windows.Forms.Label();
            this.pageCustomParams = new System.Windows.Forms.TabPage();
            this.btnDelParam = new System.Windows.Forms.Button();
            this.gbSelectedParam = new System.Windows.Forms.GroupBox();
            this.lblParamValue = new System.Windows.Forms.Label();
            this.txtParamValue = new System.Windows.Forms.TextBox();
            this.txtParamName = new System.Windows.Forms.TextBox();
            this.lblParamName = new System.Windows.Forms.Label();
            this.lvCustomParams = new System.Windows.Forms.ListView();
            this.colParamName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colParamValue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnAddParam = new System.Windows.Forms.Button();
            this.pageReqSequence = new System.Windows.Forms.TabPage();
            this.btnCutKP = new System.Windows.Forms.Button();
            this.btnImportKP = new System.Windows.Forms.Button();
            this.gbSelectedKP = new System.Windows.Forms.GroupBox();
            this.cbKpDll = new System.Windows.Forms.ComboBox();
            this.btnKpProps = new System.Windows.Forms.Button();
            this.txtCmdLine = new System.Windows.Forms.TextBox();
            this.timeKpPeriod = new System.Windows.Forms.DateTimePicker();
            this.timeKpTime = new System.Windows.Forms.DateTimePicker();
            this.lblCmdLine = new System.Windows.Forms.Label();
            this.lblKpPeriod = new System.Windows.Forms.Label();
            this.lblKpTime = new System.Windows.Forms.Label();
            this.numKpDelay = new System.Windows.Forms.NumericUpDown();
            this.lblKpDelay = new System.Windows.Forms.Label();
            this.lblKpTimeout = new System.Windows.Forms.Label();
            this.numKpTimeout = new System.Windows.Forms.NumericUpDown();
            this.lblCallNum = new System.Windows.Forms.Label();
            this.txtCallNum = new System.Windows.Forms.TextBox();
            this.lblKpAddress = new System.Windows.Forms.Label();
            this.numKpAddress = new System.Windows.Forms.NumericUpDown();
            this.lblKpDll = new System.Windows.Forms.Label();
            this.btnResetReqParams = new System.Windows.Forms.Button();
            this.lblKpName = new System.Windows.Forms.Label();
            this.numKpNumber = new System.Windows.Forms.NumericUpDown();
            this.lblKpNum = new System.Windows.Forms.Label();
            this.txtKpName = new System.Windows.Forms.TextBox();
            this.chkKpBind = new System.Windows.Forms.CheckBox();
            this.chkKpActive = new System.Windows.Forms.CheckBox();
            this.btnPasteKP = new System.Windows.Forms.Button();
            this.btnCopyKP = new System.Windows.Forms.Button();
            this.btnDelKP = new System.Windows.Forms.Button();
            this.btnMoveDownKP = new System.Windows.Forms.Button();
            this.btnMoveUpKP = new System.Windows.Forms.Button();
            this.lvReqSequence = new System.Windows.Forms.ListView();
            this.colKpOrder = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colKpActive = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colKpBind = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colKpNumber = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colKpName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colKpDll = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colKpAddress = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colCallNum = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colKpTimeout = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colKpDelay = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colKpTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colKpPeriod = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colKpCmdLine = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnAddKP = new System.Windows.Forms.Button();
            this.pageLineState = new System.Windows.Forms.TabPage();
            this.lbLineState = new System.Windows.Forms.ListBox();
            this.pageLineLog = new System.Windows.Forms.TabPage();
            this.lbLineLog = new System.Windows.Forms.ListBox();
            this.chkLineLogPause = new System.Windows.Forms.CheckBox();
            this.pageKpData = new System.Windows.Forms.TabPage();
            this.lbKpData = new System.Windows.Forms.ListBox();
            this.pageKpCmd = new System.Windows.Forms.TabPage();
            this.gbKpCmd = new System.Windows.Forms.GroupBox();
            this.btnSendCmd = new System.Windows.Forms.Button();
            this.pnlCmdData = new System.Windows.Forms.Panel();
            this.txtCmdData = new System.Windows.Forms.TextBox();
            this.rbCmdHex = new System.Windows.Forms.RadioButton();
            this.rbCmdStr = new System.Windows.Forms.RadioButton();
            this.pnlCmdVal = new System.Windows.Forms.Panel();
            this.btnCmdValOn = new System.Windows.Forms.Button();
            this.btnCmdValOff = new System.Windows.Forms.Button();
            this.txtCmdVal = new System.Windows.Forms.TextBox();
            this.lblCmdVal = new System.Windows.Forms.Label();
            this.rbCmdReq = new System.Windows.Forms.RadioButton();
            this.rbCmdBin = new System.Windows.Forms.RadioButton();
            this.rbCmdStand = new System.Windows.Forms.RadioButton();
            this.numCmdNum = new System.Windows.Forms.NumericUpDown();
            this.lblCmdNum = new System.Windows.Forms.Label();
            this.gbCmdPwd = new System.Windows.Forms.GroupBox();
            this.txtCmdPwd = new System.Windows.Forms.TextBox();
            this.pageStats = new System.Windows.Forms.TabPage();
            this.lbAppState = new System.Windows.Forms.ListBox();
            this.lbAppLog = new System.Windows.Forms.ListBox();
            this.lblAppLog = new System.Windows.Forms.Label();
            this.lblAppState = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.cmsKP = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miKpProps = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.lblSendModData = new System.Windows.Forms.Label();
            this.chkSendModData = new System.Windows.Forms.CheckBox();
            this.cmsLine.SuspendLayout();
            this.cmsNotify.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.pageCommonParams.SuspendLayout();
            this.gbPerformance.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSendAllDataPer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numWaitForStop)).BeginInit();
            this.gbServer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numServerTimeout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numServerPort)).BeginInit();
            this.pageKpDlls.SuspendLayout();
            this.pageLineParams.SuspendLayout();
            this.gbCommLine.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numLineNumber)).BeginInit();
            this.gbLineParams.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCycleDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numReqTriesCnt)).BeginInit();
            this.gbCommChannel.SuspendLayout();
            this.pageCustomParams.SuspendLayout();
            this.gbSelectedParam.SuspendLayout();
            this.pageReqSequence.SuspendLayout();
            this.gbSelectedKP.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numKpDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numKpTimeout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numKpAddress)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numKpNumber)).BeginInit();
            this.pageLineState.SuspendLayout();
            this.pageLineLog.SuspendLayout();
            this.pageKpData.SuspendLayout();
            this.pageKpCmd.SuspendLayout();
            this.gbKpCmd.SuspendLayout();
            this.pnlCmdData.SuspendLayout();
            this.pnlCmdVal.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCmdNum)).BeginInit();
            this.gbCmdPwd.SuspendLayout();
            this.pageStats.SuspendLayout();
            this.cmsKP.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmsLine
            // 
            this.cmsLine.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miAddLine,
            this.miMoveUpLine,
            this.miMoveDownLine,
            this.miDelLine,
            this.miImportLines,
            this.sepLine,
            this.miStartLine,
            this.miStopLine,
            this.miRestartLine});
            this.cmsLine.Name = "cmsLine";
            this.cmsLine.Size = new System.Drawing.Size(230, 186);
            this.cmsLine.Opened += new System.EventHandler(this.cmsLine_Opened);
            // 
            // miAddLine
            // 
            this.miAddLine.Image = ((System.Drawing.Image)(resources.GetObject("miAddLine.Image")));
            this.miAddLine.Name = "miAddLine";
            this.miAddLine.Size = new System.Drawing.Size(229, 22);
            this.miAddLine.Text = "Добавить линию связи";
            this.miAddLine.Click += new System.EventHandler(this.miAddLine_Click);
            // 
            // miMoveUpLine
            // 
            this.miMoveUpLine.Image = ((System.Drawing.Image)(resources.GetObject("miMoveUpLine.Image")));
            this.miMoveUpLine.Name = "miMoveUpLine";
            this.miMoveUpLine.Size = new System.Drawing.Size(229, 22);
            this.miMoveUpLine.Text = "Переместить вверх";
            this.miMoveUpLine.Click += new System.EventHandler(this.miMoveUpLine_Click);
            // 
            // miMoveDownLine
            // 
            this.miMoveDownLine.Image = ((System.Drawing.Image)(resources.GetObject("miMoveDownLine.Image")));
            this.miMoveDownLine.Name = "miMoveDownLine";
            this.miMoveDownLine.Size = new System.Drawing.Size(229, 22);
            this.miMoveDownLine.Text = "Переместить вниз";
            this.miMoveDownLine.Click += new System.EventHandler(this.miMoveDownLine_Click);
            // 
            // miDelLine
            // 
            this.miDelLine.Image = ((System.Drawing.Image)(resources.GetObject("miDelLine.Image")));
            this.miDelLine.Name = "miDelLine";
            this.miDelLine.Size = new System.Drawing.Size(229, 22);
            this.miDelLine.Text = "Удалить линию связи";
            this.miDelLine.Click += new System.EventHandler(this.miDelLine_Click);
            // 
            // miImportLines
            // 
            this.miImportLines.Image = ((System.Drawing.Image)(resources.GetObject("miImportLines.Image")));
            this.miImportLines.Name = "miImportLines";
            this.miImportLines.Size = new System.Drawing.Size(229, 22);
            this.miImportLines.Text = "Импорт линий связи и КП";
            this.miImportLines.Click += new System.EventHandler(this.miImportLines_Click);
            // 
            // sepLine
            // 
            this.sepLine.Name = "sepLine";
            this.sepLine.Size = new System.Drawing.Size(226, 6);
            // 
            // miStartLine
            // 
            this.miStartLine.Image = ((System.Drawing.Image)(resources.GetObject("miStartLine.Image")));
            this.miStartLine.ImageTransparentColor = System.Drawing.Color.White;
            this.miStartLine.Name = "miStartLine";
            this.miStartLine.Size = new System.Drawing.Size(229, 22);
            this.miStartLine.Tag = "StartLine";
            this.miStartLine.Text = "Запустить линию связи";
            this.miStartLine.Click += new System.EventHandler(this.miStartStopLine_Click);
            // 
            // miStopLine
            // 
            this.miStopLine.Image = ((System.Drawing.Image)(resources.GetObject("miStopLine.Image")));
            this.miStopLine.ImageTransparentColor = System.Drawing.Color.White;
            this.miStopLine.Name = "miStopLine";
            this.miStopLine.Size = new System.Drawing.Size(229, 22);
            this.miStopLine.Tag = "StopLine";
            this.miStopLine.Text = "Остановить линию связи";
            this.miStopLine.Click += new System.EventHandler(this.miStartStopLine_Click);
            // 
            // miRestartLine
            // 
            this.miRestartLine.Image = ((System.Drawing.Image)(resources.GetObject("miRestartLine.Image")));
            this.miRestartLine.Name = "miRestartLine";
            this.miRestartLine.Size = new System.Drawing.Size(229, 22);
            this.miRestartLine.Tag = "RestartLine";
            this.miRestartLine.Text = "Перезапустить линию связи";
            this.miRestartLine.Click += new System.EventHandler(this.miStartStopLine_Click);
            // 
            // ilMain
            // 
            this.ilMain.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilMain.ImageStream")));
            this.ilMain.TransparentColor = System.Drawing.Color.Gray;
            this.ilMain.Images.SetKeyName(0, "star_on.ico");
            this.ilMain.Images.SetKeyName(1, "star_off.ico");
            // 
            // notifyIcon
            // 
            this.notifyIcon.ContextMenuStrip = this.cmsNotify;
            this.notifyIcon.Text = "SCADA-Коммуникатор";
            this.notifyIcon.Visible = true;
            this.notifyIcon.MouseUp += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseUp);
            // 
            // cmsNotify
            // 
            this.cmsNotify.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miNotifyOpen,
            this.miNotifyStart,
            this.miNotifyStop,
            this.miNotifyRestart,
            this.miNotifySep,
            this.miNotifyExit});
            this.cmsNotify.Name = "contextMenuStrip";
            this.cmsNotify.Size = new System.Drawing.Size(156, 120);
            // 
            // miNotifyOpen
            // 
            this.miNotifyOpen.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.miNotifyOpen.Image = ((System.Drawing.Image)(resources.GetObject("miNotifyOpen.Image")));
            this.miNotifyOpen.ImageTransparentColor = System.Drawing.Color.Gray;
            this.miNotifyOpen.Name = "miNotifyOpen";
            this.miNotifyOpen.Size = new System.Drawing.Size(155, 22);
            this.miNotifyOpen.Text = "Открыть";
            this.miNotifyOpen.Click += new System.EventHandler(this.miNotifyOpen_Click);
            // 
            // miNotifyStart
            // 
            this.miNotifyStart.Image = ((System.Drawing.Image)(resources.GetObject("miNotifyStart.Image")));
            this.miNotifyStart.ImageTransparentColor = System.Drawing.Color.Gray;
            this.miNotifyStart.Name = "miNotifyStart";
            this.miNotifyStart.Size = new System.Drawing.Size(155, 22);
            this.miNotifyStart.Text = "Запустить";
            this.miNotifyStart.Click += new System.EventHandler(this.btnServiceStart_Click);
            // 
            // miNotifyStop
            // 
            this.miNotifyStop.Image = ((System.Drawing.Image)(resources.GetObject("miNotifyStop.Image")));
            this.miNotifyStop.ImageTransparentColor = System.Drawing.Color.White;
            this.miNotifyStop.Name = "miNotifyStop";
            this.miNotifyStop.Size = new System.Drawing.Size(155, 22);
            this.miNotifyStop.Text = "Остановить";
            this.miNotifyStop.Click += new System.EventHandler(this.btnServiceStop_Click);
            // 
            // miNotifyRestart
            // 
            this.miNotifyRestart.Image = ((System.Drawing.Image)(resources.GetObject("miNotifyRestart.Image")));
            this.miNotifyRestart.Name = "miNotifyRestart";
            this.miNotifyRestart.Size = new System.Drawing.Size(155, 22);
            this.miNotifyRestart.Text = "Перезапустить";
            this.miNotifyRestart.Click += new System.EventHandler(this.btnServiceRestart_Click);
            // 
            // miNotifySep
            // 
            this.miNotifySep.Name = "miNotifySep";
            this.miNotifySep.Size = new System.Drawing.Size(152, 6);
            // 
            // miNotifyExit
            // 
            this.miNotifyExit.Name = "miNotifyExit";
            this.miNotifyExit.Size = new System.Drawing.Size(155, 22);
            this.miNotifyExit.Text = "Выход";
            this.miNotifyExit.Click += new System.EventHandler(this.miNotifyExit_Click);
            // 
            // tmrRefr
            // 
            this.tmrRefr.Interval = 200;
            this.tmrRefr.Tick += new System.EventHandler(this.tmrRefr_Tick);
            // 
            // treeView
            // 
            this.treeView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.treeView.HideSelection = false;
            this.treeView.ImageKey = "params.png";
            this.treeView.ImageList = this.ilTree;
            this.treeView.Location = new System.Drawing.Point(0, 0);
            this.treeView.Margin = new System.Windows.Forms.Padding(0);
            this.treeView.Name = "treeView";
            treeNode5.ImageKey = "params.png";
            treeNode5.Name = "nodeCommonParams";
            treeNode5.SelectedImageKey = "params.png";
            treeNode5.Text = "Общие параметры";
            treeNode6.ImageKey = "kpdll.png";
            treeNode6.Name = "nodeKpDlls";
            treeNode6.SelectedImageKey = "kpdll.png";
            treeNode6.Text = "Библиотеки КП";
            treeNode7.ContextMenuStrip = this.cmsLine;
            treeNode7.ImageKey = "commlines.png";
            treeNode7.Name = "nodeLines";
            treeNode7.SelectedImageKey = "commlines.png";
            treeNode7.Text = "Линии связи";
            treeNode8.ImageKey = "stats.png";
            treeNode8.Name = "nodeStats";
            treeNode8.SelectedImageKey = "stats.png";
            treeNode8.Text = "Статистика";
            this.treeView.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode5,
            treeNode6,
            treeNode7,
            treeNode8});
            this.treeView.SelectedImageKey = "params.png";
            this.treeView.ShowRootLines = false;
            this.treeView.Size = new System.Drawing.Size(240, 464);
            this.treeView.TabIndex = 0;
            this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterSelect);
            this.treeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView_NodeMouseClick);
            this.treeView.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView_NodeMouseDoubleClick);
            // 
            // ilTree
            // 
            this.ilTree.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilTree.ImageStream")));
            this.ilTree.TransparentColor = System.Drawing.Color.Transparent;
            this.ilTree.Images.SetKeyName(0, "params.png");
            this.ilTree.Images.SetKeyName(1, "kpdll.png");
            this.ilTree.Images.SetKeyName(2, "commlines.png");
            this.ilTree.Images.SetKeyName(3, "commline.png");
            this.ilTree.Images.SetKeyName(4, "stats.png");
            this.ilTree.Images.SetKeyName(5, "kp.png");
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblCurDate,
            this.lblCurTime,
            this.lblServiceState});
            this.statusStrip.Location = new System.Drawing.Point(0, 490);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(654, 22);
            this.statusStrip.SizingGrip = false;
            this.statusStrip.TabIndex = 2;
            // 
            // lblCurDate
            // 
            this.lblCurDate.AutoSize = false;
            this.lblCurDate.Name = "lblCurDate";
            this.lblCurDate.Size = new System.Drawing.Size(75, 17);
            this.lblCurDate.Text = "15.02.2008";
            // 
            // lblCurTime
            // 
            this.lblCurTime.AutoSize = false;
            this.lblCurTime.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)));
            this.lblCurTime.Name = "lblCurTime";
            this.lblCurTime.Size = new System.Drawing.Size(75, 17);
            this.lblCurTime.Text = "19:20:00";
            // 
            // lblServiceState
            // 
            this.lblServiceState.Name = "lblServiceState";
            this.lblServiceState.Size = new System.Drawing.Size(324, 17);
            this.lblServiceState.Text = "Состояние службы SCADA-Коммуникатора: остановлена";
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnServiceStart,
            this.btnServiceStop,
            this.btnServiceRestart,
            this.sepMain1,
            this.btnSettingsApply,
            this.btnSettingsCancel,
            this.btnSettingsUpdate,
            this.sepMain2,
            this.btnAbout});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(654, 25);
            this.toolStrip.TabIndex = 0;
            // 
            // btnServiceStart
            // 
            this.btnServiceStart.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnServiceStart.Image = ((System.Drawing.Image)(resources.GetObject("btnServiceStart.Image")));
            this.btnServiceStart.ImageTransparentColor = System.Drawing.Color.White;
            this.btnServiceStart.Name = "btnServiceStart";
            this.btnServiceStart.Size = new System.Drawing.Size(23, 22);
            this.btnServiceStart.ToolTipText = "Запустить службу SCADA-Коммуникатора";
            this.btnServiceStart.Click += new System.EventHandler(this.btnServiceStart_Click);
            // 
            // btnServiceStop
            // 
            this.btnServiceStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnServiceStop.Image = ((System.Drawing.Image)(resources.GetObject("btnServiceStop.Image")));
            this.btnServiceStop.ImageTransparentColor = System.Drawing.Color.White;
            this.btnServiceStop.Name = "btnServiceStop";
            this.btnServiceStop.Size = new System.Drawing.Size(23, 22);
            this.btnServiceStop.ToolTipText = "Остановить службу SCADA-Коммуникатора";
            this.btnServiceStop.Click += new System.EventHandler(this.btnServiceStop_Click);
            // 
            // btnServiceRestart
            // 
            this.btnServiceRestart.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnServiceRestart.Image = ((System.Drawing.Image)(resources.GetObject("btnServiceRestart.Image")));
            this.btnServiceRestart.ImageTransparentColor = System.Drawing.Color.White;
            this.btnServiceRestart.Name = "btnServiceRestart";
            this.btnServiceRestart.Size = new System.Drawing.Size(23, 22);
            this.btnServiceRestart.ToolTipText = "Перезапустить службу SCADA-Коммуникатора";
            this.btnServiceRestart.Click += new System.EventHandler(this.btnServiceRestart_Click);
            // 
            // sepMain1
            // 
            this.sepMain1.Name = "sepMain1";
            this.sepMain1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnSettingsApply
            // 
            this.btnSettingsApply.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSettingsApply.Enabled = false;
            this.btnSettingsApply.Image = ((System.Drawing.Image)(resources.GetObject("btnSettingsApply.Image")));
            this.btnSettingsApply.ImageTransparentColor = System.Drawing.Color.White;
            this.btnSettingsApply.Name = "btnSettingsApply";
            this.btnSettingsApply.Size = new System.Drawing.Size(23, 22);
            this.btnSettingsApply.ToolTipText = "Применить изменения настроек";
            this.btnSettingsApply.Click += new System.EventHandler(this.btnSettingsApply_Click);
            // 
            // btnSettingsCancel
            // 
            this.btnSettingsCancel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSettingsCancel.Enabled = false;
            this.btnSettingsCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnSettingsCancel.Image")));
            this.btnSettingsCancel.ImageTransparentColor = System.Drawing.Color.White;
            this.btnSettingsCancel.Name = "btnSettingsCancel";
            this.btnSettingsCancel.Size = new System.Drawing.Size(23, 22);
            this.btnSettingsCancel.ToolTipText = "Отменить изменения настроек";
            this.btnSettingsCancel.Click += new System.EventHandler(this.btnSettingsCancel_Click);
            // 
            // btnSettingsUpdate
            // 
            this.btnSettingsUpdate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSettingsUpdate.Image = ((System.Drawing.Image)(resources.GetObject("btnSettingsUpdate.Image")));
            this.btnSettingsUpdate.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.btnSettingsUpdate.Name = "btnSettingsUpdate";
            this.btnSettingsUpdate.Size = new System.Drawing.Size(23, 22);
            this.btnSettingsUpdate.ToolTipText = "Обновить настройки по базе конфигурации";
            this.btnSettingsUpdate.Click += new System.EventHandler(this.btnSettingsUpdate_Click);
            // 
            // sepMain2
            // 
            this.sepMain2.Name = "sepMain2";
            this.sepMain2.Size = new System.Drawing.Size(6, 25);
            // 
            // btnAbout
            // 
            this.btnAbout.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnAbout.Image = ((System.Drawing.Image)(resources.GetObject("btnAbout.Image")));
            this.btnAbout.ImageTransparentColor = System.Drawing.Color.Silver;
            this.btnAbout.Name = "btnAbout";
            this.btnAbout.Size = new System.Drawing.Size(23, 22);
            this.btnAbout.ToolTipText = "О программе";
            this.btnAbout.Click += new System.EventHandler(this.btnAbout_Click);
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.pageCommonParams);
            this.tabControl.Controls.Add(this.pageKpDlls);
            this.tabControl.Controls.Add(this.pageLineParams);
            this.tabControl.Controls.Add(this.pageCustomParams);
            this.tabControl.Controls.Add(this.pageReqSequence);
            this.tabControl.Controls.Add(this.pageLineState);
            this.tabControl.Controls.Add(this.pageLineLog);
            this.tabControl.Controls.Add(this.pageKpData);
            this.tabControl.Controls.Add(this.pageKpCmd);
            this.tabControl.Controls.Add(this.pageStats);
            this.tabControl.Location = new System.Drawing.Point(240, 0);
            this.tabControl.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(414, 465);
            this.tabControl.TabIndex = 1;
            this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
            // 
            // pageCommonParams
            // 
            this.pageCommonParams.BackColor = System.Drawing.Color.Transparent;
            this.pageCommonParams.Controls.Add(this.gbPerformance);
            this.pageCommonParams.Controls.Add(this.gbServer);
            this.pageCommonParams.Location = new System.Drawing.Point(4, 22);
            this.pageCommonParams.Name = "pageCommonParams";
            this.pageCommonParams.Padding = new System.Windows.Forms.Padding(3);
            this.pageCommonParams.Size = new System.Drawing.Size(406, 439);
            this.pageCommonParams.TabIndex = 0;
            this.pageCommonParams.Text = "Общие параметры";
            this.pageCommonParams.UseVisualStyleBackColor = true;
            // 
            // gbPerformance
            // 
            this.gbPerformance.Controls.Add(this.lblSendModData);
            this.gbPerformance.Controls.Add(this.chkSendModData);
            this.gbPerformance.Controls.Add(this.lblSendAllDataPer);
            this.gbPerformance.Controls.Add(this.numSendAllDataPer);
            this.gbPerformance.Controls.Add(this.numWaitForStop);
            this.gbPerformance.Controls.Add(this.lblWaitForStop);
            this.gbPerformance.Location = new System.Drawing.Point(6, 140);
            this.gbPerformance.Name = "gbPerformance";
            this.gbPerformance.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbPerformance.Size = new System.Drawing.Size(392, 104);
            this.gbPerformance.TabIndex = 1;
            this.gbPerformance.TabStop = false;
            this.gbPerformance.Text = "Выполнение";
            // 
            // lblSendAllDataPer
            // 
            this.lblSendAllDataPer.AutoSize = true;
            this.lblSendAllDataPer.Location = new System.Drawing.Point(13, 75);
            this.lblSendAllDataPer.Name = "lblSendAllDataPer";
            this.lblSendAllDataPer.Size = new System.Drawing.Size(227, 13);
            this.lblSendAllDataPer.TabIndex = 4;
            this.lblSendAllDataPer.Text = "Период передачи Серверу всех тегов КП, с";
            // 
            // numSendAllDataPer
            // 
            this.numSendAllDataPer.Location = new System.Drawing.Point(319, 71);
            this.numSendAllDataPer.Maximum = new decimal(new int[] {
            3600,
            0,
            0,
            0});
            this.numSendAllDataPer.Name = "numSendAllDataPer";
            this.numSendAllDataPer.Size = new System.Drawing.Size(60, 20);
            this.numSendAllDataPer.TabIndex = 5;
            this.numSendAllDataPer.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.numSendAllDataPer.ValueChanged += new System.EventHandler(this.numSendAllDataPer_ValueChanged);
            // 
            // numWaitForStop
            // 
            this.numWaitForStop.Increment = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numWaitForStop.Location = new System.Drawing.Point(319, 19);
            this.numWaitForStop.Maximum = new decimal(new int[] {
            600000,
            0,
            0,
            0});
            this.numWaitForStop.Name = "numWaitForStop";
            this.numWaitForStop.Size = new System.Drawing.Size(60, 20);
            this.numWaitForStop.TabIndex = 1;
            this.numWaitForStop.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numWaitForStop.ValueChanged += new System.EventHandler(this.numWaitForStop_ValueChanged);
            // 
            // lblWaitForStop
            // 
            this.lblWaitForStop.AutoSize = true;
            this.lblWaitForStop.Location = new System.Drawing.Point(13, 23);
            this.lblWaitForStop.Name = "lblWaitForStop";
            this.lblWaitForStop.Size = new System.Drawing.Size(201, 13);
            this.lblWaitForStop.TabIndex = 0;
            this.lblWaitForStop.Text = "Ожидание остановки линий связи, мс";
            // 
            // gbServer
            // 
            this.gbServer.Controls.Add(this.lblServerPwd);
            this.gbServer.Controls.Add(this.txtServerPwd);
            this.gbServer.Controls.Add(this.txtServerUser);
            this.gbServer.Controls.Add(this.lblServerUser);
            this.gbServer.Controls.Add(this.lblServerTimeout);
            this.gbServer.Controls.Add(this.numServerTimeout);
            this.gbServer.Controls.Add(this.lblServerPort);
            this.gbServer.Controls.Add(this.numServerPort);
            this.gbServer.Controls.Add(this.lblServerHost);
            this.gbServer.Controls.Add(this.chkServerUse);
            this.gbServer.Controls.Add(this.txtServerHost);
            this.gbServer.Location = new System.Drawing.Point(6, 6);
            this.gbServer.Name = "gbServer";
            this.gbServer.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbServer.Size = new System.Drawing.Size(392, 128);
            this.gbServer.TabIndex = 0;
            this.gbServer.TabStop = false;
            this.gbServer.Text = "Связь со SCADA-Сервером";
            // 
            // lblServerPwd
            // 
            this.lblServerPwd.AutoSize = true;
            this.lblServerPwd.Location = new System.Drawing.Point(170, 78);
            this.lblServerPwd.Name = "lblServerPwd";
            this.lblServerPwd.Size = new System.Drawing.Size(45, 13);
            this.lblServerPwd.TabIndex = 9;
            this.lblServerPwd.Text = "Пароль";
            // 
            // txtServerPwd
            // 
            this.txtServerPwd.Location = new System.Drawing.Point(173, 95);
            this.txtServerPwd.Name = "txtServerPwd";
            this.txtServerPwd.Size = new System.Drawing.Size(100, 20);
            this.txtServerPwd.TabIndex = 10;
            this.txtServerPwd.Text = "12345";
            this.txtServerPwd.UseSystemPasswordChar = true;
            this.txtServerPwd.TextChanged += new System.EventHandler(this.txtServerPwd_TextChanged);
            // 
            // txtServerUser
            // 
            this.txtServerUser.Location = new System.Drawing.Point(13, 95);
            this.txtServerUser.Name = "txtServerUser";
            this.txtServerUser.Size = new System.Drawing.Size(154, 20);
            this.txtServerUser.TabIndex = 8;
            this.txtServerUser.Text = "ScadaComm";
            this.txtServerUser.TextChanged += new System.EventHandler(this.txtServerUser_TextChanged);
            // 
            // lblServerUser
            // 
            this.lblServerUser.AutoSize = true;
            this.lblServerUser.Location = new System.Drawing.Point(10, 79);
            this.lblServerUser.Name = "lblServerUser";
            this.lblServerUser.Size = new System.Drawing.Size(80, 13);
            this.lblServerUser.TabIndex = 7;
            this.lblServerUser.Text = "Пользователь";
            // 
            // lblServerTimeout
            // 
            this.lblServerTimeout.AutoSize = true;
            this.lblServerTimeout.Location = new System.Drawing.Point(276, 39);
            this.lblServerTimeout.Name = "lblServerTimeout";
            this.lblServerTimeout.Size = new System.Drawing.Size(50, 13);
            this.lblServerTimeout.TabIndex = 5;
            this.lblServerTimeout.Text = "Таймаут";
            // 
            // numServerTimeout
            // 
            this.numServerTimeout.Location = new System.Drawing.Point(279, 55);
            this.numServerTimeout.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numServerTimeout.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numServerTimeout.Name = "numServerTimeout";
            this.numServerTimeout.Size = new System.Drawing.Size(100, 20);
            this.numServerTimeout.TabIndex = 6;
            this.numServerTimeout.Value = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numServerTimeout.ValueChanged += new System.EventHandler(this.numServerTimeout_ValueChanged);
            // 
            // lblServerPort
            // 
            this.lblServerPort.AutoSize = true;
            this.lblServerPort.Location = new System.Drawing.Point(170, 39);
            this.lblServerPort.Name = "lblServerPort";
            this.lblServerPort.Size = new System.Drawing.Size(32, 13);
            this.lblServerPort.TabIndex = 3;
            this.lblServerPort.Text = "Порт";
            // 
            // numServerPort
            // 
            this.numServerPort.Location = new System.Drawing.Point(173, 55);
            this.numServerPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numServerPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numServerPort.Name = "numServerPort";
            this.numServerPort.Size = new System.Drawing.Size(100, 20);
            this.numServerPort.TabIndex = 4;
            this.numServerPort.Value = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numServerPort.ValueChanged += new System.EventHandler(this.numServerPort_ValueChanged);
            // 
            // lblServerHost
            // 
            this.lblServerHost.AutoSize = true;
            this.lblServerHost.Location = new System.Drawing.Point(10, 39);
            this.lblServerHost.Name = "lblServerHost";
            this.lblServerHost.Size = new System.Drawing.Size(44, 13);
            this.lblServerHost.TabIndex = 1;
            this.lblServerHost.Text = "Сервер";
            // 
            // chkServerUse
            // 
            this.chkServerUse.AutoSize = true;
            this.chkServerUse.Checked = true;
            this.chkServerUse.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkServerUse.Location = new System.Drawing.Point(13, 19);
            this.chkServerUse.Name = "chkServerUse";
            this.chkServerUse.Size = new System.Drawing.Size(178, 17);
            this.chkServerUse.TabIndex = 0;
            this.chkServerUse.Text = "Использовать SCADA-Сервер";
            this.chkServerUse.UseVisualStyleBackColor = true;
            this.chkServerUse.CheckedChanged += new System.EventHandler(this.chkServerUse_CheckedChanged);
            // 
            // txtServerHost
            // 
            this.txtServerHost.Location = new System.Drawing.Point(13, 55);
            this.txtServerHost.Name = "txtServerHost";
            this.txtServerHost.Size = new System.Drawing.Size(154, 20);
            this.txtServerHost.TabIndex = 2;
            this.txtServerHost.Text = "localhost";
            this.txtServerHost.TextChanged += new System.EventHandler(this.txtServerHost_TextChanged);
            // 
            // pageKpDlls
            // 
            this.pageKpDlls.BackColor = System.Drawing.Color.Transparent;
            this.pageKpDlls.Controls.Add(this.btnKpDllProps);
            this.pageKpDlls.Controls.Add(this.txtKpDllDescr);
            this.pageKpDlls.Controls.Add(this.lblKpDllDescr);
            this.pageKpDlls.Controls.Add(this.lbKpDll);
            this.pageKpDlls.Location = new System.Drawing.Point(4, 22);
            this.pageKpDlls.Name = "pageKpDlls";
            this.pageKpDlls.Padding = new System.Windows.Forms.Padding(3);
            this.pageKpDlls.Size = new System.Drawing.Size(406, 439);
            this.pageKpDlls.TabIndex = 2;
            this.pageKpDlls.Text = "Библиотеки КП";
            this.pageKpDlls.UseVisualStyleBackColor = true;
            // 
            // btnKpDllProps
            // 
            this.btnKpDllProps.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnKpDllProps.Enabled = false;
            this.btnKpDllProps.Location = new System.Drawing.Point(6, 406);
            this.btnKpDllProps.Name = "btnKpDllProps";
            this.btnKpDllProps.Size = new System.Drawing.Size(90, 23);
            this.btnKpDllProps.TabIndex = 3;
            this.btnKpDllProps.Text = "Свойства";
            this.btnKpDllProps.UseVisualStyleBackColor = true;
            this.btnKpDllProps.Click += new System.EventHandler(this.btnKpTypeProps_Click);
            // 
            // txtKpDllDescr
            // 
            this.txtKpDllDescr.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtKpDllDescr.Location = new System.Drawing.Point(6, 198);
            this.txtKpDllDescr.Multiline = true;
            this.txtKpDllDescr.Name = "txtKpDllDescr";
            this.txtKpDllDescr.ReadOnly = true;
            this.txtKpDllDescr.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtKpDllDescr.Size = new System.Drawing.Size(392, 202);
            this.txtKpDllDescr.TabIndex = 2;
            // 
            // lblKpDllDescr
            // 
            this.lblKpDllDescr.AutoSize = true;
            this.lblKpDllDescr.Location = new System.Drawing.Point(3, 182);
            this.lblKpDllDescr.Name = "lblKpDllDescr";
            this.lblKpDllDescr.Size = new System.Drawing.Size(57, 13);
            this.lblKpDllDescr.TabIndex = 1;
            this.lblKpDllDescr.Text = "Описание";
            // 
            // lbKpDll
            // 
            this.lbKpDll.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbKpDll.FormattingEnabled = true;
            this.lbKpDll.HorizontalScrollbar = true;
            this.lbKpDll.Location = new System.Drawing.Point(6, 6);
            this.lbKpDll.MultiColumn = true;
            this.lbKpDll.Name = "lbKpDll";
            this.lbKpDll.Size = new System.Drawing.Size(392, 173);
            this.lbKpDll.TabIndex = 0;
            this.lbKpDll.SelectedIndexChanged += new System.EventHandler(this.lbKpDll_SelectedIndexChanged);
            this.lbKpDll.DoubleClick += new System.EventHandler(this.btnKpTypeProps_Click);
            // 
            // pageLineParams
            // 
            this.pageLineParams.BackColor = System.Drawing.Color.Transparent;
            this.pageLineParams.Controls.Add(this.gbCommLine);
            this.pageLineParams.Controls.Add(this.gbLineParams);
            this.pageLineParams.Controls.Add(this.gbCommChannel);
            this.pageLineParams.Location = new System.Drawing.Point(4, 22);
            this.pageLineParams.Name = "pageLineParams";
            this.pageLineParams.Padding = new System.Windows.Forms.Padding(3);
            this.pageLineParams.Size = new System.Drawing.Size(406, 439);
            this.pageLineParams.TabIndex = 1;
            this.pageLineParams.Text = "Параметры линии связи";
            this.pageLineParams.UseVisualStyleBackColor = true;
            // 
            // gbCommLine
            // 
            this.gbCommLine.Controls.Add(this.chkLineBind);
            this.gbCommLine.Controls.Add(this.numLineNumber);
            this.gbCommLine.Controls.Add(this.txtLineName);
            this.gbCommLine.Controls.Add(this.lnlLineName);
            this.gbCommLine.Controls.Add(this.lblLineNumber);
            this.gbCommLine.Controls.Add(this.chkLineActive);
            this.gbCommLine.Location = new System.Drawing.Point(6, 6);
            this.gbCommLine.Name = "gbCommLine";
            this.gbCommLine.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbCommLine.Size = new System.Drawing.Size(392, 65);
            this.gbCommLine.TabIndex = 0;
            this.gbCommLine.TabStop = false;
            this.gbCommLine.Text = "Линия связи";
            // 
            // chkLineBind
            // 
            this.chkLineBind.AutoSize = true;
            this.chkLineBind.Location = new System.Drawing.Point(13, 38);
            this.chkLineBind.Name = "chkLineBind";
            this.chkLineBind.Size = new System.Drawing.Size(135, 17);
            this.chkLineBind.TabIndex = 1;
            this.chkLineBind.Text = "Привязана к серверу";
            this.chkLineBind.UseVisualStyleBackColor = true;
            this.chkLineBind.CheckedChanged += new System.EventHandler(this.chkLineBind_CheckedChanged);
            // 
            // numLineNumber
            // 
            this.numLineNumber.Location = new System.Drawing.Point(148, 32);
            this.numLineNumber.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numLineNumber.Name = "numLineNumber";
            this.numLineNumber.Size = new System.Drawing.Size(60, 20);
            this.numLineNumber.TabIndex = 3;
            this.numLineNumber.ValueChanged += new System.EventHandler(this.numLineNumber_ValueChanged);
            // 
            // txtLineName
            // 
            this.txtLineName.Location = new System.Drawing.Point(214, 32);
            this.txtLineName.Name = "txtLineName";
            this.txtLineName.Size = new System.Drawing.Size(165, 20);
            this.txtLineName.TabIndex = 5;
            this.txtLineName.TextChanged += new System.EventHandler(this.txtLineName_TextChanged);
            // 
            // lnlLineName
            // 
            this.lnlLineName.AutoSize = true;
            this.lnlLineName.Location = new System.Drawing.Point(211, 16);
            this.lnlLineName.Name = "lnlLineName";
            this.lnlLineName.Size = new System.Drawing.Size(83, 13);
            this.lnlLineName.TabIndex = 4;
            this.lnlLineName.Text = "Наименование";
            // 
            // lblLineNumber
            // 
            this.lblLineNumber.AutoSize = true;
            this.lblLineNumber.Location = new System.Drawing.Point(145, 16);
            this.lblLineNumber.Name = "lblLineNumber";
            this.lblLineNumber.Size = new System.Drawing.Size(41, 13);
            this.lblLineNumber.TabIndex = 2;
            this.lblLineNumber.Text = "Номер";
            // 
            // chkLineActive
            // 
            this.chkLineActive.AutoSize = true;
            this.chkLineActive.Location = new System.Drawing.Point(13, 19);
            this.chkLineActive.Name = "chkLineActive";
            this.chkLineActive.Size = new System.Drawing.Size(68, 17);
            this.chkLineActive.TabIndex = 0;
            this.chkLineActive.Text = "Активна";
            this.chkLineActive.UseVisualStyleBackColor = true;
            this.chkLineActive.CheckedChanged += new System.EventHandler(this.chkLineActive_CheckedChanged);
            // 
            // gbLineParams
            // 
            this.gbLineParams.Controls.Add(this.chkReqAfterCmd);
            this.gbLineParams.Controls.Add(this.lblReqAfterCmd);
            this.gbLineParams.Controls.Add(this.lblDetailedLog);
            this.gbLineParams.Controls.Add(this.chkDetailedLog);
            this.gbLineParams.Controls.Add(this.lblCmdEnabled);
            this.gbLineParams.Controls.Add(this.chkCmdEnabled);
            this.gbLineParams.Controls.Add(this.lblCycleDelay);
            this.gbLineParams.Controls.Add(this.numCycleDelay);
            this.gbLineParams.Controls.Add(this.numReqTriesCnt);
            this.gbLineParams.Controls.Add(this.lblReqTriesCnt);
            this.gbLineParams.Location = new System.Drawing.Point(6, 279);
            this.gbLineParams.Name = "gbLineParams";
            this.gbLineParams.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbLineParams.Size = new System.Drawing.Size(392, 154);
            this.gbLineParams.TabIndex = 2;
            this.gbLineParams.TabStop = false;
            this.gbLineParams.Text = "Параметры связи";
            // 
            // chkReqAfterCmd
            // 
            this.chkReqAfterCmd.AutoSize = true;
            this.chkReqAfterCmd.Location = new System.Drawing.Point(342, 101);
            this.chkReqAfterCmd.Name = "chkReqAfterCmd";
            this.chkReqAfterCmd.Size = new System.Drawing.Size(15, 14);
            this.chkReqAfterCmd.TabIndex = 7;
            this.chkReqAfterCmd.UseVisualStyleBackColor = true;
            this.chkReqAfterCmd.CheckedChanged += new System.EventHandler(this.chkReqAfterCmd_CheckedChanged);
            // 
            // lblReqAfterCmd
            // 
            this.lblReqAfterCmd.AutoSize = true;
            this.lblReqAfterCmd.Location = new System.Drawing.Point(13, 101);
            this.lblReqAfterCmd.Name = "lblReqAfterCmd";
            this.lblReqAfterCmd.Size = new System.Drawing.Size(157, 13);
            this.lblReqAfterCmd.TabIndex = 6;
            this.lblReqAfterCmd.Text = "Опрос КП после команды ТУ";
            this.lblReqAfterCmd.Click += new System.EventHandler(this.lblReqAfterCmd_Click);
            // 
            // lblDetailedLog
            // 
            this.lblDetailedLog.AutoSize = true;
            this.lblDetailedLog.Location = new System.Drawing.Point(13, 127);
            this.lblDetailedLog.Name = "lblDetailedLog";
            this.lblDetailedLog.Size = new System.Drawing.Size(105, 13);
            this.lblDetailedLog.TabIndex = 8;
            this.lblDetailedLog.Text = "Подробный журнал";
            this.lblDetailedLog.Click += new System.EventHandler(this.lblDetailedLog_Click);
            // 
            // chkDetailedLog
            // 
            this.chkDetailedLog.AutoSize = true;
            this.chkDetailedLog.Location = new System.Drawing.Point(342, 127);
            this.chkDetailedLog.Name = "chkDetailedLog";
            this.chkDetailedLog.Size = new System.Drawing.Size(15, 14);
            this.chkDetailedLog.TabIndex = 9;
            this.chkDetailedLog.UseVisualStyleBackColor = true;
            this.chkDetailedLog.CheckedChanged += new System.EventHandler(this.chkDetailedLog_CheckedChanged);
            // 
            // lblCmdEnabled
            // 
            this.lblCmdEnabled.AutoSize = true;
            this.lblCmdEnabled.Location = new System.Drawing.Point(13, 75);
            this.lblCmdEnabled.Name = "lblCmdEnabled";
            this.lblCmdEnabled.Size = new System.Drawing.Size(133, 13);
            this.lblCmdEnabled.TabIndex = 4;
            this.lblCmdEnabled.Text = "Команды ТУ разрешены";
            this.lblCmdEnabled.Click += new System.EventHandler(this.lblCmdEnabled_Click);
            // 
            // chkCmdEnabled
            // 
            this.chkCmdEnabled.AutoSize = true;
            this.chkCmdEnabled.Location = new System.Drawing.Point(342, 75);
            this.chkCmdEnabled.Name = "chkCmdEnabled";
            this.chkCmdEnabled.Size = new System.Drawing.Size(15, 14);
            this.chkCmdEnabled.TabIndex = 5;
            this.chkCmdEnabled.UseVisualStyleBackColor = true;
            this.chkCmdEnabled.CheckedChanged += new System.EventHandler(this.chkCmdEnabled_CheckedChanged);
            // 
            // lblCycleDelay
            // 
            this.lblCycleDelay.AutoSize = true;
            this.lblCycleDelay.Location = new System.Drawing.Point(13, 49);
            this.lblCycleDelay.Name = "lblCycleDelay";
            this.lblCycleDelay.Size = new System.Drawing.Size(183, 13);
            this.lblCycleDelay.TabIndex = 2;
            this.lblCycleDelay.Text = "Задержка после цикла опроса, мс";
            // 
            // numCycleDelay
            // 
            this.numCycleDelay.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numCycleDelay.Location = new System.Drawing.Point(319, 45);
            this.numCycleDelay.Maximum = new decimal(new int[] {
            600000,
            0,
            0,
            0});
            this.numCycleDelay.Name = "numCycleDelay";
            this.numCycleDelay.Size = new System.Drawing.Size(60, 20);
            this.numCycleDelay.TabIndex = 3;
            this.numCycleDelay.ValueChanged += new System.EventHandler(this.numCycleDelay_ValueChanged);
            // 
            // numReqTriesCnt
            // 
            this.numReqTriesCnt.Location = new System.Drawing.Point(319, 19);
            this.numReqTriesCnt.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numReqTriesCnt.Name = "numReqTriesCnt";
            this.numReqTriesCnt.Size = new System.Drawing.Size(60, 20);
            this.numReqTriesCnt.TabIndex = 1;
            this.numReqTriesCnt.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.numReqTriesCnt.ValueChanged += new System.EventHandler(this.numReqTriesCnt_ValueChanged);
            // 
            // lblReqTriesCnt
            // 
            this.lblReqTriesCnt.AutoSize = true;
            this.lblReqTriesCnt.Location = new System.Drawing.Point(13, 23);
            this.lblReqTriesCnt.Name = "lblReqTriesCnt";
            this.lblReqTriesCnt.Size = new System.Drawing.Size(261, 13);
            this.lblReqTriesCnt.TabIndex = 0;
            this.lblReqTriesCnt.Text = "Количество попыток перезапроса КП при ошибке";
            // 
            // gbCommChannel
            // 
            this.gbCommChannel.Controls.Add(this.txtCommCnlParams);
            this.gbCommChannel.Controls.Add(this.lblCommCnlParams);
            this.gbCommChannel.Controls.Add(this.btnCommCnlProps);
            this.gbCommChannel.Controls.Add(this.cbCommCnlType);
            this.gbCommChannel.Controls.Add(this.lblCommCnlType);
            this.gbCommChannel.Location = new System.Drawing.Point(6, 77);
            this.gbCommChannel.Name = "gbCommChannel";
            this.gbCommChannel.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbCommChannel.Size = new System.Drawing.Size(392, 196);
            this.gbCommChannel.TabIndex = 1;
            this.gbCommChannel.TabStop = false;
            this.gbCommChannel.Text = "Канал связи";
            // 
            // txtCommCnlParams
            // 
            this.txtCommCnlParams.Location = new System.Drawing.Point(13, 72);
            this.txtCommCnlParams.Multiline = true;
            this.txtCommCnlParams.Name = "txtCommCnlParams";
            this.txtCommCnlParams.ReadOnly = true;
            this.txtCommCnlParams.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtCommCnlParams.Size = new System.Drawing.Size(366, 111);
            this.txtCommCnlParams.TabIndex = 4;
            // 
            // lblCommCnlParams
            // 
            this.lblCommCnlParams.AutoSize = true;
            this.lblCommCnlParams.Location = new System.Drawing.Point(10, 56);
            this.lblCommCnlParams.Name = "lblCommCnlParams";
            this.lblCommCnlParams.Size = new System.Drawing.Size(66, 13);
            this.lblCommCnlParams.TabIndex = 3;
            this.lblCommCnlParams.Text = "Параметры";
            // 
            // btnCommCnlProps
            // 
            this.btnCommCnlProps.Location = new System.Drawing.Point(289, 31);
            this.btnCommCnlProps.Name = "btnCommCnlProps";
            this.btnCommCnlProps.Size = new System.Drawing.Size(90, 23);
            this.btnCommCnlProps.TabIndex = 2;
            this.btnCommCnlProps.Text = "Свойства";
            this.btnCommCnlProps.UseVisualStyleBackColor = true;
            this.btnCommCnlProps.Click += new System.EventHandler(this.btnCommCnlProps_Click);
            // 
            // cbCommCnlType
            // 
            this.cbCommCnlType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCommCnlType.FormattingEnabled = true;
            this.cbCommCnlType.Items.AddRange(new object[] {
            "Не задан"});
            this.cbCommCnlType.Location = new System.Drawing.Point(13, 32);
            this.cbCommCnlType.Name = "cbCommCnlType";
            this.cbCommCnlType.Size = new System.Drawing.Size(270, 21);
            this.cbCommCnlType.TabIndex = 1;
            this.cbCommCnlType.SelectedIndexChanged += new System.EventHandler(this.cbCommCnlType_SelectedIndexChanged);
            // 
            // lblCommCnlType
            // 
            this.lblCommCnlType.AutoSize = true;
            this.lblCommCnlType.Location = new System.Drawing.Point(10, 16);
            this.lblCommCnlType.Name = "lblCommCnlType";
            this.lblCommCnlType.Size = new System.Drawing.Size(26, 13);
            this.lblCommCnlType.TabIndex = 0;
            this.lblCommCnlType.Text = "Тип";
            // 
            // pageCustomParams
            // 
            this.pageCustomParams.BackColor = System.Drawing.Color.Transparent;
            this.pageCustomParams.Controls.Add(this.btnDelParam);
            this.pageCustomParams.Controls.Add(this.gbSelectedParam);
            this.pageCustomParams.Controls.Add(this.lvCustomParams);
            this.pageCustomParams.Controls.Add(this.btnAddParam);
            this.pageCustomParams.Location = new System.Drawing.Point(4, 22);
            this.pageCustomParams.Name = "pageCustomParams";
            this.pageCustomParams.Padding = new System.Windows.Forms.Padding(3);
            this.pageCustomParams.Size = new System.Drawing.Size(406, 439);
            this.pageCustomParams.TabIndex = 3;
            this.pageCustomParams.Text = "Пользовательские параметры";
            this.pageCustomParams.UseVisualStyleBackColor = true;
            // 
            // btnDelParam
            // 
            this.btnDelParam.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnDelParam.Image = ((System.Drawing.Image)(resources.GetObject("btnDelParam.Image")));
            this.btnDelParam.Location = new System.Drawing.Point(35, 6);
            this.btnDelParam.Name = "btnDelParam";
            this.btnDelParam.Size = new System.Drawing.Size(23, 22);
            this.btnDelParam.TabIndex = 1;
            this.toolTip.SetToolTip(this.btnDelParam, "Удалить выбранный параметр");
            this.btnDelParam.UseVisualStyleBackColor = true;
            this.btnDelParam.Click += new System.EventHandler(this.btnDelParam_Click);
            // 
            // gbSelectedParam
            // 
            this.gbSelectedParam.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.gbSelectedParam.Controls.Add(this.lblParamValue);
            this.gbSelectedParam.Controls.Add(this.txtParamValue);
            this.gbSelectedParam.Controls.Add(this.txtParamName);
            this.gbSelectedParam.Controls.Add(this.lblParamName);
            this.gbSelectedParam.Location = new System.Drawing.Point(6, 364);
            this.gbSelectedParam.Name = "gbSelectedParam";
            this.gbSelectedParam.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbSelectedParam.Size = new System.Drawing.Size(392, 65);
            this.gbSelectedParam.TabIndex = 3;
            this.gbSelectedParam.TabStop = false;
            this.gbSelectedParam.Text = "Выбранный параметр";
            // 
            // lblParamValue
            // 
            this.lblParamValue.AutoSize = true;
            this.lblParamValue.Location = new System.Drawing.Point(146, 16);
            this.lblParamValue.Name = "lblParamValue";
            this.lblParamValue.Size = new System.Drawing.Size(55, 13);
            this.lblParamValue.TabIndex = 2;
            this.lblParamValue.Text = "Значение";
            // 
            // txtParamValue
            // 
            this.txtParamValue.Location = new System.Drawing.Point(149, 32);
            this.txtParamValue.Name = "txtParamValue";
            this.txtParamValue.Size = new System.Drawing.Size(230, 20);
            this.txtParamValue.TabIndex = 3;
            this.txtParamValue.TextChanged += new System.EventHandler(this.txtParamValue_TextChanged);
            // 
            // txtParamName
            // 
            this.txtParamName.Location = new System.Drawing.Point(13, 32);
            this.txtParamName.Name = "txtParamName";
            this.txtParamName.Size = new System.Drawing.Size(130, 20);
            this.txtParamName.TabIndex = 1;
            this.txtParamName.TextChanged += new System.EventHandler(this.txtParamName_TextChanged);
            this.txtParamName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtParamName_KeyDown);
            this.txtParamName.Leave += new System.EventHandler(this.txtParamName_Leave);
            // 
            // lblParamName
            // 
            this.lblParamName.AutoSize = true;
            this.lblParamName.Location = new System.Drawing.Point(10, 16);
            this.lblParamName.Name = "lblParamName";
            this.lblParamName.Size = new System.Drawing.Size(83, 13);
            this.lblParamName.TabIndex = 0;
            this.lblParamName.Text = "Наименование";
            // 
            // lvCustomParams
            // 
            this.lvCustomParams.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvCustomParams.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colParamName,
            this.colParamValue});
            this.lvCustomParams.FullRowSelect = true;
            this.lvCustomParams.GridLines = true;
            this.lvCustomParams.HideSelection = false;
            this.lvCustomParams.Location = new System.Drawing.Point(6, 34);
            this.lvCustomParams.MultiSelect = false;
            this.lvCustomParams.Name = "lvCustomParams";
            this.lvCustomParams.ShowItemToolTips = true;
            this.lvCustomParams.Size = new System.Drawing.Size(392, 324);
            this.lvCustomParams.TabIndex = 2;
            this.lvCustomParams.UseCompatibleStateImageBehavior = false;
            this.lvCustomParams.View = System.Windows.Forms.View.Details;
            this.lvCustomParams.SelectedIndexChanged += new System.EventHandler(this.lvCustomParams_SelectedIndexChanged);
            // 
            // colParamName
            // 
            this.colParamName.Text = "Наименование";
            this.colParamName.Width = 150;
            // 
            // colParamValue
            // 
            this.colParamValue.Text = "Значение";
            this.colParamValue.Width = 221;
            // 
            // btnAddParam
            // 
            this.btnAddParam.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAddParam.Image = ((System.Drawing.Image)(resources.GetObject("btnAddParam.Image")));
            this.btnAddParam.Location = new System.Drawing.Point(6, 6);
            this.btnAddParam.Name = "btnAddParam";
            this.btnAddParam.Size = new System.Drawing.Size(23, 22);
            this.btnAddParam.TabIndex = 0;
            this.toolTip.SetToolTip(this.btnAddParam, "Добавить параметр");
            this.btnAddParam.UseVisualStyleBackColor = true;
            this.btnAddParam.Click += new System.EventHandler(this.btnAddParam_Click);
            // 
            // pageReqSequence
            // 
            this.pageReqSequence.BackColor = System.Drawing.Color.Transparent;
            this.pageReqSequence.Controls.Add(this.btnCutKP);
            this.pageReqSequence.Controls.Add(this.btnImportKP);
            this.pageReqSequence.Controls.Add(this.gbSelectedKP);
            this.pageReqSequence.Controls.Add(this.btnPasteKP);
            this.pageReqSequence.Controls.Add(this.btnCopyKP);
            this.pageReqSequence.Controls.Add(this.btnDelKP);
            this.pageReqSequence.Controls.Add(this.btnMoveDownKP);
            this.pageReqSequence.Controls.Add(this.btnMoveUpKP);
            this.pageReqSequence.Controls.Add(this.lvReqSequence);
            this.pageReqSequence.Controls.Add(this.btnAddKP);
            this.pageReqSequence.Location = new System.Drawing.Point(4, 22);
            this.pageReqSequence.Name = "pageReqSequence";
            this.pageReqSequence.Padding = new System.Windows.Forms.Padding(3);
            this.pageReqSequence.Size = new System.Drawing.Size(406, 439);
            this.pageReqSequence.TabIndex = 4;
            this.pageReqSequence.Text = "Опрос КП";
            this.pageReqSequence.UseVisualStyleBackColor = true;
            // 
            // btnCutKP
            // 
            this.btnCutKP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCutKP.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCutKP.Image = ((System.Drawing.Image)(resources.GetObject("btnCutKP.Image")));
            this.btnCutKP.Location = new System.Drawing.Point(317, 6);
            this.btnCutKP.Name = "btnCutKP";
            this.btnCutKP.Size = new System.Drawing.Size(23, 22);
            this.btnCutKP.TabIndex = 5;
            this.toolTip.SetToolTip(this.btnCutKP, "Вырезать выбранный КП");
            this.btnCutKP.UseVisualStyleBackColor = true;
            this.btnCutKP.Click += new System.EventHandler(this.btnCutKP_Click);
            // 
            // btnImportKP
            // 
            this.btnImportKP.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnImportKP.Image = ((System.Drawing.Image)(resources.GetObject("btnImportKP.Image")));
            this.btnImportKP.Location = new System.Drawing.Point(122, 6);
            this.btnImportKP.Name = "btnImportKP";
            this.btnImportKP.Size = new System.Drawing.Size(23, 22);
            this.btnImportKP.TabIndex = 4;
            this.toolTip.SetToolTip(this.btnImportKP, "Импорт КП из базы конфигурации");
            this.btnImportKP.UseVisualStyleBackColor = true;
            this.btnImportKP.Click += new System.EventHandler(this.btnImportKP_Click);
            // 
            // gbSelectedKP
            // 
            this.gbSelectedKP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.gbSelectedKP.Controls.Add(this.cbKpDll);
            this.gbSelectedKP.Controls.Add(this.btnKpProps);
            this.gbSelectedKP.Controls.Add(this.txtCmdLine);
            this.gbSelectedKP.Controls.Add(this.timeKpPeriod);
            this.gbSelectedKP.Controls.Add(this.timeKpTime);
            this.gbSelectedKP.Controls.Add(this.lblCmdLine);
            this.gbSelectedKP.Controls.Add(this.lblKpPeriod);
            this.gbSelectedKP.Controls.Add(this.lblKpTime);
            this.gbSelectedKP.Controls.Add(this.numKpDelay);
            this.gbSelectedKP.Controls.Add(this.lblKpDelay);
            this.gbSelectedKP.Controls.Add(this.lblKpTimeout);
            this.gbSelectedKP.Controls.Add(this.numKpTimeout);
            this.gbSelectedKP.Controls.Add(this.lblCallNum);
            this.gbSelectedKP.Controls.Add(this.txtCallNum);
            this.gbSelectedKP.Controls.Add(this.lblKpAddress);
            this.gbSelectedKP.Controls.Add(this.numKpAddress);
            this.gbSelectedKP.Controls.Add(this.lblKpDll);
            this.gbSelectedKP.Controls.Add(this.btnResetReqParams);
            this.gbSelectedKP.Controls.Add(this.lblKpName);
            this.gbSelectedKP.Controls.Add(this.numKpNumber);
            this.gbSelectedKP.Controls.Add(this.lblKpNum);
            this.gbSelectedKP.Controls.Add(this.txtKpName);
            this.gbSelectedKP.Controls.Add(this.chkKpBind);
            this.gbSelectedKP.Controls.Add(this.chkKpActive);
            this.gbSelectedKP.Location = new System.Drawing.Point(6, 284);
            this.gbSelectedKP.Name = "gbSelectedKP";
            this.gbSelectedKP.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbSelectedKP.Size = new System.Drawing.Size(392, 145);
            this.gbSelectedKP.TabIndex = 9;
            this.gbSelectedKP.TabStop = false;
            this.gbSelectedKP.Text = "Выбранный КП";
            // 
            // cbKpDll
            // 
            this.cbKpDll.FormattingEnabled = true;
            this.cbKpDll.Location = new System.Drawing.Point(13, 72);
            this.cbKpDll.Name = "cbKpDll";
            this.cbKpDll.Size = new System.Drawing.Size(85, 21);
            this.cbKpDll.TabIndex = 7;
            this.cbKpDll.TextChanged += new System.EventHandler(this.cbKpType_TextChanged);
            // 
            // btnKpProps
            // 
            this.btnKpProps.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnKpProps.Image = ((System.Drawing.Image)(resources.GetObject("btnKpProps.Image")));
            this.btnKpProps.Location = new System.Drawing.Point(359, 112);
            this.btnKpProps.Name = "btnKpProps";
            this.btnKpProps.Size = new System.Drawing.Size(20, 20);
            this.btnKpProps.TabIndex = 24;
            this.toolTip.SetToolTip(this.btnKpProps, "Свойства КП");
            this.btnKpProps.UseVisualStyleBackColor = true;
            this.btnKpProps.Click += new System.EventHandler(this.btnKpProps_Click);
            // 
            // txtCmdLine
            // 
            this.txtCmdLine.Location = new System.Drawing.Point(175, 112);
            this.txtCmdLine.Name = "txtCmdLine";
            this.txtCmdLine.Size = new System.Drawing.Size(152, 20);
            this.txtCmdLine.TabIndex = 22;
            this.txtCmdLine.TextChanged += new System.EventHandler(this.txtCmdLine_TextChanged);
            // 
            // timeKpPeriod
            // 
            this.timeKpPeriod.CustomFormat = "HH:mm:ss";
            this.timeKpPeriod.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.timeKpPeriod.Location = new System.Drawing.Point(104, 112);
            this.timeKpPeriod.Name = "timeKpPeriod";
            this.timeKpPeriod.ShowUpDown = true;
            this.timeKpPeriod.Size = new System.Drawing.Size(65, 20);
            this.timeKpPeriod.TabIndex = 20;
            this.timeKpPeriod.ValueChanged += new System.EventHandler(this.timeKpPeriod_ValueChanged);
            // 
            // timeKpTime
            // 
            this.timeKpTime.CustomFormat = "HH:mm:ss";
            this.timeKpTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.timeKpTime.Location = new System.Drawing.Point(13, 112);
            this.timeKpTime.Name = "timeKpTime";
            this.timeKpTime.ShowUpDown = true;
            this.timeKpTime.Size = new System.Drawing.Size(85, 20);
            this.timeKpTime.TabIndex = 18;
            this.timeKpTime.Value = new System.DateTime(2010, 7, 9, 21, 51, 0, 0);
            this.timeKpTime.ValueChanged += new System.EventHandler(this.timeKpTime_ValueChanged);
            // 
            // lblCmdLine
            // 
            this.lblCmdLine.AutoSize = true;
            this.lblCmdLine.Location = new System.Drawing.Point(172, 95);
            this.lblCmdLine.Name = "lblCmdLine";
            this.lblCmdLine.Size = new System.Drawing.Size(102, 13);
            this.lblCmdLine.TabIndex = 21;
            this.lblCmdLine.Text = "Командная строка";
            // 
            // lblKpPeriod
            // 
            this.lblKpPeriod.AutoSize = true;
            this.lblKpPeriod.Location = new System.Drawing.Point(101, 96);
            this.lblKpPeriod.Name = "lblKpPeriod";
            this.lblKpPeriod.Size = new System.Drawing.Size(45, 13);
            this.lblKpPeriod.TabIndex = 19;
            this.lblKpPeriod.Text = "Период";
            // 
            // lblKpTime
            // 
            this.lblKpTime.AutoSize = true;
            this.lblKpTime.Location = new System.Drawing.Point(10, 96);
            this.lblKpTime.Name = "lblKpTime";
            this.lblKpTime.Size = new System.Drawing.Size(40, 13);
            this.lblKpTime.TabIndex = 17;
            this.lblKpTime.Text = "Время";
            // 
            // numKpDelay
            // 
            this.numKpDelay.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numKpDelay.Location = new System.Drawing.Point(324, 72);
            this.numKpDelay.Maximum = new decimal(new int[] {
            600000,
            0,
            0,
            0});
            this.numKpDelay.Name = "numKpDelay";
            this.numKpDelay.Size = new System.Drawing.Size(55, 20);
            this.numKpDelay.TabIndex = 16;
            this.numKpDelay.ValueChanged += new System.EventHandler(this.numKpDelay_ValueChanged);
            // 
            // lblKpDelay
            // 
            this.lblKpDelay.AutoSize = true;
            this.lblKpDelay.Location = new System.Drawing.Point(321, 56);
            this.lblKpDelay.Name = "lblKpDelay";
            this.lblKpDelay.Size = new System.Drawing.Size(38, 13);
            this.lblKpDelay.TabIndex = 15;
            this.lblKpDelay.Text = "Пауза";
            // 
            // lblKpTimeout
            // 
            this.lblKpTimeout.AutoSize = true;
            this.lblKpTimeout.Location = new System.Drawing.Point(260, 56);
            this.lblKpTimeout.Name = "lblKpTimeout";
            this.lblKpTimeout.Size = new System.Drawing.Size(50, 13);
            this.lblKpTimeout.TabIndex = 12;
            this.lblKpTimeout.Text = "Таймаут";
            // 
            // numKpTimeout
            // 
            this.numKpTimeout.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numKpTimeout.Location = new System.Drawing.Point(263, 72);
            this.numKpTimeout.Maximum = new decimal(new int[] {
            600000,
            0,
            0,
            0});
            this.numKpTimeout.Name = "numKpTimeout";
            this.numKpTimeout.Size = new System.Drawing.Size(55, 20);
            this.numKpTimeout.TabIndex = 13;
            this.numKpTimeout.ValueChanged += new System.EventHandler(this.numKpTimeout_ValueChanged);
            // 
            // lblCallNum
            // 
            this.lblCallNum.AutoSize = true;
            this.lblCallNum.Location = new System.Drawing.Point(172, 56);
            this.lblCallNum.Name = "lblCallNum";
            this.lblCallNum.Size = new System.Drawing.Size(59, 13);
            this.lblCallNum.TabIndex = 10;
            this.lblCallNum.Text = "Позывной";
            // 
            // txtCallNum
            // 
            this.txtCallNum.Location = new System.Drawing.Point(175, 72);
            this.txtCallNum.Name = "txtCallNum";
            this.txtCallNum.Size = new System.Drawing.Size(82, 20);
            this.txtCallNum.TabIndex = 11;
            this.txtCallNum.TextChanged += new System.EventHandler(this.txtCallNum_TextChanged);
            // 
            // lblKpAddress
            // 
            this.lblKpAddress.AutoSize = true;
            this.lblKpAddress.Location = new System.Drawing.Point(101, 56);
            this.lblKpAddress.Name = "lblKpAddress";
            this.lblKpAddress.Size = new System.Drawing.Size(38, 13);
            this.lblKpAddress.TabIndex = 8;
            this.lblKpAddress.Text = "Адрес";
            // 
            // numKpAddress
            // 
            this.numKpAddress.Location = new System.Drawing.Point(104, 72);
            this.numKpAddress.Name = "numKpAddress";
            this.numKpAddress.Size = new System.Drawing.Size(65, 20);
            this.numKpAddress.TabIndex = 9;
            this.numKpAddress.ValueChanged += new System.EventHandler(this.numKpAddress_ValueChanged);
            // 
            // lblKpDll
            // 
            this.lblKpDll.AutoSize = true;
            this.lblKpDll.Location = new System.Drawing.Point(10, 56);
            this.lblKpDll.Name = "lblKpDll";
            this.lblKpDll.Size = new System.Drawing.Size(27, 13);
            this.lblKpDll.TabIndex = 6;
            this.lblKpDll.Text = "DLL";
            // 
            // btnResetReqParams
            // 
            this.btnResetReqParams.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnResetReqParams.Image = ((System.Drawing.Image)(resources.GetObject("btnResetReqParams.Image")));
            this.btnResetReqParams.Location = new System.Drawing.Point(333, 112);
            this.btnResetReqParams.Name = "btnResetReqParams";
            this.btnResetReqParams.Size = new System.Drawing.Size(20, 20);
            this.btnResetReqParams.TabIndex = 23;
            this.toolTip.SetToolTip(this.btnResetReqParams, "Установить параметры опроса КП по умолчанию");
            this.btnResetReqParams.UseVisualStyleBackColor = true;
            this.btnResetReqParams.Click += new System.EventHandler(this.btnResetReqParams_Click);
            // 
            // lblKpName
            // 
            this.lblKpName.AutoSize = true;
            this.lblKpName.Location = new System.Drawing.Point(172, 16);
            this.lblKpName.Name = "lblKpName";
            this.lblKpName.Size = new System.Drawing.Size(83, 13);
            this.lblKpName.TabIndex = 4;
            this.lblKpName.Text = "Наименование";
            // 
            // numKpNumber
            // 
            this.numKpNumber.Location = new System.Drawing.Point(104, 32);
            this.numKpNumber.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numKpNumber.Name = "numKpNumber";
            this.numKpNumber.Size = new System.Drawing.Size(65, 20);
            this.numKpNumber.TabIndex = 3;
            this.numKpNumber.ValueChanged += new System.EventHandler(this.numKpNumber_ValueChanged);
            // 
            // lblKpNum
            // 
            this.lblKpNum.AutoSize = true;
            this.lblKpNum.Location = new System.Drawing.Point(101, 16);
            this.lblKpNum.Name = "lblKpNum";
            this.lblKpNum.Size = new System.Drawing.Size(41, 13);
            this.lblKpNum.TabIndex = 2;
            this.lblKpNum.Text = "Номер";
            // 
            // txtKpName
            // 
            this.txtKpName.Location = new System.Drawing.Point(175, 32);
            this.txtKpName.Name = "txtKpName";
            this.txtKpName.Size = new System.Drawing.Size(204, 20);
            this.txtKpName.TabIndex = 5;
            this.txtKpName.TextChanged += new System.EventHandler(this.txtKpName_TextChanged);
            // 
            // chkKpBind
            // 
            this.chkKpBind.AutoSize = true;
            this.chkKpBind.Location = new System.Drawing.Point(13, 38);
            this.chkKpBind.Name = "chkKpBind";
            this.chkKpBind.Size = new System.Drawing.Size(76, 17);
            this.chkKpBind.TabIndex = 1;
            this.chkKpBind.Text = "Привязка";
            this.chkKpBind.UseVisualStyleBackColor = true;
            this.chkKpBind.CheckedChanged += new System.EventHandler(this.chkKpBind_CheckedChanged);
            // 
            // chkKpActive
            // 
            this.chkKpActive.AutoSize = true;
            this.chkKpActive.Location = new System.Drawing.Point(13, 19);
            this.chkKpActive.Name = "chkKpActive";
            this.chkKpActive.Size = new System.Drawing.Size(68, 17);
            this.chkKpActive.TabIndex = 0;
            this.chkKpActive.Text = "Активен";
            this.chkKpActive.UseVisualStyleBackColor = true;
            this.chkKpActive.CheckedChanged += new System.EventHandler(this.chkKpActive_CheckedChanged);
            // 
            // btnPasteKP
            // 
            this.btnPasteKP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPasteKP.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnPasteKP.Image = ((System.Drawing.Image)(resources.GetObject("btnPasteKP.Image")));
            this.btnPasteKP.Location = new System.Drawing.Point(375, 6);
            this.btnPasteKP.Name = "btnPasteKP";
            this.btnPasteKP.Size = new System.Drawing.Size(23, 22);
            this.btnPasteKP.TabIndex = 7;
            this.toolTip.SetToolTip(this.btnPasteKP, "Вставить КП");
            this.btnPasteKP.UseVisualStyleBackColor = true;
            this.btnPasteKP.Click += new System.EventHandler(this.btnPasteKP_Click);
            // 
            // btnCopyKP
            // 
            this.btnCopyKP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCopyKP.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCopyKP.Image = ((System.Drawing.Image)(resources.GetObject("btnCopyKP.Image")));
            this.btnCopyKP.Location = new System.Drawing.Point(346, 6);
            this.btnCopyKP.Name = "btnCopyKP";
            this.btnCopyKP.Size = new System.Drawing.Size(23, 22);
            this.btnCopyKP.TabIndex = 6;
            this.toolTip.SetToolTip(this.btnCopyKP, "Копировать выбранный КП");
            this.btnCopyKP.UseVisualStyleBackColor = true;
            this.btnCopyKP.Click += new System.EventHandler(this.btnCopyKP_Click);
            // 
            // btnDelKP
            // 
            this.btnDelKP.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnDelKP.Image = ((System.Drawing.Image)(resources.GetObject("btnDelKP.Image")));
            this.btnDelKP.Location = new System.Drawing.Point(93, 6);
            this.btnDelKP.Name = "btnDelKP";
            this.btnDelKP.Size = new System.Drawing.Size(23, 22);
            this.btnDelKP.TabIndex = 3;
            this.toolTip.SetToolTip(this.btnDelKP, "Удалить выбранный КП");
            this.btnDelKP.UseVisualStyleBackColor = true;
            this.btnDelKP.Click += new System.EventHandler(this.btnDelKP_Click);
            // 
            // btnMoveDownKP
            // 
            this.btnMoveDownKP.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnMoveDownKP.Image = ((System.Drawing.Image)(resources.GetObject("btnMoveDownKP.Image")));
            this.btnMoveDownKP.Location = new System.Drawing.Point(64, 6);
            this.btnMoveDownKP.Name = "btnMoveDownKP";
            this.btnMoveDownKP.Size = new System.Drawing.Size(23, 22);
            this.btnMoveDownKP.TabIndex = 2;
            this.toolTip.SetToolTip(this.btnMoveDownKP, "Переместить выбранный КП вниз");
            this.btnMoveDownKP.UseVisualStyleBackColor = true;
            this.btnMoveDownKP.Click += new System.EventHandler(this.btnMoveDownKP_Click);
            // 
            // btnMoveUpKP
            // 
            this.btnMoveUpKP.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnMoveUpKP.Image = ((System.Drawing.Image)(resources.GetObject("btnMoveUpKP.Image")));
            this.btnMoveUpKP.Location = new System.Drawing.Point(35, 6);
            this.btnMoveUpKP.Name = "btnMoveUpKP";
            this.btnMoveUpKP.Size = new System.Drawing.Size(23, 22);
            this.btnMoveUpKP.TabIndex = 1;
            this.toolTip.SetToolTip(this.btnMoveUpKP, "Переместить выбранный КП вверх");
            this.btnMoveUpKP.UseVisualStyleBackColor = true;
            this.btnMoveUpKP.Click += new System.EventHandler(this.btnMoveUpKP_Click);
            // 
            // lvReqSequence
            // 
            this.lvReqSequence.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvReqSequence.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colKpOrder,
            this.colKpActive,
            this.colKpBind,
            this.colKpNumber,
            this.colKpName,
            this.colKpDll,
            this.colKpAddress,
            this.colCallNum,
            this.colKpTimeout,
            this.colKpDelay,
            this.colKpTime,
            this.colKpPeriod,
            this.colKpCmdLine});
            this.lvReqSequence.FullRowSelect = true;
            this.lvReqSequence.GridLines = true;
            this.lvReqSequence.HideSelection = false;
            this.lvReqSequence.Location = new System.Drawing.Point(6, 34);
            this.lvReqSequence.MultiSelect = false;
            this.lvReqSequence.Name = "lvReqSequence";
            this.lvReqSequence.ShowItemToolTips = true;
            this.lvReqSequence.Size = new System.Drawing.Size(392, 244);
            this.lvReqSequence.TabIndex = 8;
            this.lvReqSequence.UseCompatibleStateImageBehavior = false;
            this.lvReqSequence.View = System.Windows.Forms.View.Details;
            this.lvReqSequence.SelectedIndexChanged += new System.EventHandler(this.lvReqSequence_SelectedIndexChanged);
            // 
            // colKpOrder
            // 
            this.colKpOrder.Text = "№";
            this.colKpOrder.Width = 40;
            // 
            // colKpActive
            // 
            this.colKpActive.Text = "Актив.";
            this.colKpActive.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.colKpActive.Width = 50;
            // 
            // colKpBind
            // 
            this.colKpBind.Text = "Прив.";
            this.colKpBind.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.colKpBind.Width = 50;
            // 
            // colKpNumber
            // 
            this.colKpNumber.Text = "Номер";
            this.colKpNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.colKpNumber.Width = 50;
            // 
            // colKpName
            // 
            this.colKpName.Text = "Наименование";
            this.colKpName.Width = 90;
            // 
            // colKpDll
            // 
            this.colKpDll.Text = "DLL";
            // 
            // colKpAddress
            // 
            this.colKpAddress.Text = "Адрес";
            this.colKpAddress.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.colKpAddress.Width = 50;
            // 
            // colCallNum
            // 
            this.colCallNum.Text = "Позывной";
            this.colCallNum.Width = 70;
            // 
            // colKpTimeout
            // 
            this.colKpTimeout.Text = "Таймаут";
            this.colKpTimeout.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // colKpDelay
            // 
            this.colKpDelay.Text = "Пауза";
            this.colKpDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // colKpTime
            // 
            this.colKpTime.Text = "Время";
            this.colKpTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.colKpTime.Width = 75;
            // 
            // colKpPeriod
            // 
            this.colKpPeriod.Text = "Период";
            this.colKpPeriod.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // colKpCmdLine
            // 
            this.colKpCmdLine.Text = "Командная строка";
            this.colKpCmdLine.Width = 120;
            // 
            // btnAddKP
            // 
            this.btnAddKP.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAddKP.Image = ((System.Drawing.Image)(resources.GetObject("btnAddKP.Image")));
            this.btnAddKP.Location = new System.Drawing.Point(6, 6);
            this.btnAddKP.Name = "btnAddKP";
            this.btnAddKP.Size = new System.Drawing.Size(23, 22);
            this.btnAddKP.TabIndex = 0;
            this.toolTip.SetToolTip(this.btnAddKP, "Добавить КП");
            this.btnAddKP.UseVisualStyleBackColor = true;
            this.btnAddKP.Click += new System.EventHandler(this.btnAddKP_Click);
            // 
            // pageLineState
            // 
            this.pageLineState.BackColor = System.Drawing.Color.Transparent;
            this.pageLineState.Controls.Add(this.lbLineState);
            this.pageLineState.Location = new System.Drawing.Point(4, 22);
            this.pageLineState.Name = "pageLineState";
            this.pageLineState.Padding = new System.Windows.Forms.Padding(3);
            this.pageLineState.Size = new System.Drawing.Size(406, 439);
            this.pageLineState.TabIndex = 5;
            this.pageLineState.Text = "Состояние линии связи";
            this.pageLineState.UseVisualStyleBackColor = true;
            // 
            // lbLineState
            // 
            this.lbLineState.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbLineState.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbLineState.FormattingEnabled = true;
            this.lbLineState.HorizontalScrollbar = true;
            this.lbLineState.IntegralHeight = false;
            this.lbLineState.ItemHeight = 11;
            this.lbLineState.Location = new System.Drawing.Point(6, 6);
            this.lbLineState.Name = "lbLineState";
            this.lbLineState.Size = new System.Drawing.Size(392, 423);
            this.lbLineState.TabIndex = 1;
            this.lbLineState.KeyDown += new System.Windows.Forms.KeyEventHandler(this.LogListBox_KeyDown);
            // 
            // pageLineLog
            // 
            this.pageLineLog.BackColor = System.Drawing.Color.Transparent;
            this.pageLineLog.Controls.Add(this.lbLineLog);
            this.pageLineLog.Controls.Add(this.chkLineLogPause);
            this.pageLineLog.Location = new System.Drawing.Point(4, 22);
            this.pageLineLog.Name = "pageLineLog";
            this.pageLineLog.Padding = new System.Windows.Forms.Padding(3);
            this.pageLineLog.Size = new System.Drawing.Size(406, 439);
            this.pageLineLog.TabIndex = 6;
            this.pageLineLog.Text = "Журнал линии связи";
            this.pageLineLog.UseVisualStyleBackColor = true;
            // 
            // lbLineLog
            // 
            this.lbLineLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbLineLog.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbLineLog.FormattingEnabled = true;
            this.lbLineLog.HorizontalScrollbar = true;
            this.lbLineLog.IntegralHeight = false;
            this.lbLineLog.ItemHeight = 11;
            this.lbLineLog.Location = new System.Drawing.Point(6, 22);
            this.lbLineLog.Name = "lbLineLog";
            this.lbLineLog.Size = new System.Drawing.Size(392, 407);
            this.lbLineLog.TabIndex = 2;
            this.lbLineLog.KeyDown += new System.Windows.Forms.KeyEventHandler(this.LogListBox_KeyDown);
            // 
            // chkLineLogPause
            // 
            this.chkLineLogPause.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkLineLogPause.AutoSize = true;
            this.chkLineLogPause.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkLineLogPause.Location = new System.Drawing.Point(341, 3);
            this.chkLineLogPause.Name = "chkLineLogPause";
            this.chkLineLogPause.Size = new System.Drawing.Size(57, 17);
            this.chkLineLogPause.TabIndex = 1;
            this.chkLineLogPause.Text = "Пауза";
            this.chkLineLogPause.UseVisualStyleBackColor = true;
            // 
            // pageKpData
            // 
            this.pageKpData.BackColor = System.Drawing.Color.Transparent;
            this.pageKpData.Controls.Add(this.lbKpData);
            this.pageKpData.Location = new System.Drawing.Point(4, 22);
            this.pageKpData.Name = "pageKpData";
            this.pageKpData.Padding = new System.Windows.Forms.Padding(3);
            this.pageKpData.Size = new System.Drawing.Size(406, 439);
            this.pageKpData.TabIndex = 7;
            this.pageKpData.Text = "Данные КП";
            this.pageKpData.UseVisualStyleBackColor = true;
            // 
            // lbKpData
            // 
            this.lbKpData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbKpData.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbKpData.FormattingEnabled = true;
            this.lbKpData.HorizontalScrollbar = true;
            this.lbKpData.IntegralHeight = false;
            this.lbKpData.ItemHeight = 11;
            this.lbKpData.Location = new System.Drawing.Point(6, 6);
            this.lbKpData.Name = "lbKpData";
            this.lbKpData.Size = new System.Drawing.Size(392, 423);
            this.lbKpData.TabIndex = 1;
            this.lbKpData.KeyDown += new System.Windows.Forms.KeyEventHandler(this.LogListBox_KeyDown);
            // 
            // pageKpCmd
            // 
            this.pageKpCmd.Controls.Add(this.gbKpCmd);
            this.pageKpCmd.Controls.Add(this.gbCmdPwd);
            this.pageKpCmd.Location = new System.Drawing.Point(4, 22);
            this.pageKpCmd.Name = "pageKpCmd";
            this.pageKpCmd.Padding = new System.Windows.Forms.Padding(3);
            this.pageKpCmd.Size = new System.Drawing.Size(406, 439);
            this.pageKpCmd.TabIndex = 10;
            this.pageKpCmd.Text = "Команды";
            this.pageKpCmd.UseVisualStyleBackColor = true;
            // 
            // gbKpCmd
            // 
            this.gbKpCmd.Controls.Add(this.btnSendCmd);
            this.gbKpCmd.Controls.Add(this.pnlCmdData);
            this.gbKpCmd.Controls.Add(this.pnlCmdVal);
            this.gbKpCmd.Controls.Add(this.rbCmdReq);
            this.gbKpCmd.Controls.Add(this.rbCmdBin);
            this.gbKpCmd.Controls.Add(this.rbCmdStand);
            this.gbKpCmd.Controls.Add(this.numCmdNum);
            this.gbKpCmd.Controls.Add(this.lblCmdNum);
            this.gbKpCmd.Location = new System.Drawing.Point(6, 64);
            this.gbKpCmd.Name = "gbKpCmd";
            this.gbKpCmd.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbKpCmd.Size = new System.Drawing.Size(392, 263);
            this.gbKpCmd.TabIndex = 1;
            this.gbKpCmd.TabStop = false;
            this.gbKpCmd.Text = "Команда ТУ";
            this.gbKpCmd.Visible = false;
            // 
            // btnSendCmd
            // 
            this.btnSendCmd.Location = new System.Drawing.Point(13, 227);
            this.btnSendCmd.Name = "btnSendCmd";
            this.btnSendCmd.Size = new System.Drawing.Size(90, 23);
            this.btnSendCmd.TabIndex = 9;
            this.btnSendCmd.Text = "Отправить";
            this.btnSendCmd.UseVisualStyleBackColor = true;
            this.btnSendCmd.Click += new System.EventHandler(this.btnSendCmd_Click);
            // 
            // pnlCmdData
            // 
            this.pnlCmdData.Controls.Add(this.txtCmdData);
            this.pnlCmdData.Controls.Add(this.rbCmdHex);
            this.pnlCmdData.Controls.Add(this.rbCmdStr);
            this.pnlCmdData.Location = new System.Drawing.Point(13, 100);
            this.pnlCmdData.Name = "pnlCmdData";
            this.pnlCmdData.Size = new System.Drawing.Size(366, 121);
            this.pnlCmdData.TabIndex = 8;
            // 
            // txtCmdData
            // 
            this.txtCmdData.Location = new System.Drawing.Point(0, 23);
            this.txtCmdData.Multiline = true;
            this.txtCmdData.Name = "txtCmdData";
            this.txtCmdData.Size = new System.Drawing.Size(366, 98);
            this.txtCmdData.TabIndex = 2;
            // 
            // rbCmdHex
            // 
            this.rbCmdHex.AutoSize = true;
            this.rbCmdHex.Location = new System.Drawing.Point(67, 0);
            this.rbCmdHex.Name = "rbCmdHex";
            this.rbCmdHex.Size = new System.Drawing.Size(118, 17);
            this.rbCmdHex.TabIndex = 1;
            this.rbCmdHex.Text = "16-ричные данные";
            this.rbCmdHex.UseVisualStyleBackColor = true;
            // 
            // rbCmdStr
            // 
            this.rbCmdStr.AutoSize = true;
            this.rbCmdStr.Checked = true;
            this.rbCmdStr.Location = new System.Drawing.Point(0, 0);
            this.rbCmdStr.Name = "rbCmdStr";
            this.rbCmdStr.Size = new System.Drawing.Size(61, 17);
            this.rbCmdStr.TabIndex = 0;
            this.rbCmdStr.TabStop = true;
            this.rbCmdStr.Text = "Строка";
            this.rbCmdStr.UseVisualStyleBackColor = true;
            // 
            // pnlCmdVal
            // 
            this.pnlCmdVal.Controls.Add(this.btnCmdValOn);
            this.pnlCmdVal.Controls.Add(this.btnCmdValOff);
            this.pnlCmdVal.Controls.Add(this.txtCmdVal);
            this.pnlCmdVal.Controls.Add(this.lblCmdVal);
            this.pnlCmdVal.Location = new System.Drawing.Point(13, 58);
            this.pnlCmdVal.Name = "pnlCmdVal";
            this.pnlCmdVal.Size = new System.Drawing.Size(366, 36);
            this.pnlCmdVal.TabIndex = 7;
            // 
            // btnCmdValOn
            // 
            this.btnCmdValOn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCmdValOn.Location = new System.Drawing.Point(207, 16);
            this.btnCmdValOn.Name = "btnCmdValOn";
            this.btnCmdValOn.Size = new System.Drawing.Size(45, 20);
            this.btnCmdValOn.TabIndex = 3;
            this.btnCmdValOn.Text = "Вкл.";
            this.btnCmdValOn.UseVisualStyleBackColor = true;
            this.btnCmdValOn.Click += new System.EventHandler(this.btnCmdVal_Click);
            // 
            // btnCmdValOff
            // 
            this.btnCmdValOff.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCmdValOff.Location = new System.Drawing.Point(156, 16);
            this.btnCmdValOff.Name = "btnCmdValOff";
            this.btnCmdValOff.Size = new System.Drawing.Size(45, 20);
            this.btnCmdValOff.TabIndex = 2;
            this.btnCmdValOff.Text = "Откл.";
            this.btnCmdValOff.UseVisualStyleBackColor = true;
            this.btnCmdValOff.Click += new System.EventHandler(this.btnCmdVal_Click);
            // 
            // txtCmdVal
            // 
            this.txtCmdVal.Location = new System.Drawing.Point(0, 16);
            this.txtCmdVal.Name = "txtCmdVal";
            this.txtCmdVal.Size = new System.Drawing.Size(150, 20);
            this.txtCmdVal.TabIndex = 1;
            this.txtCmdVal.Text = "0";
            // 
            // lblCmdVal
            // 
            this.lblCmdVal.AutoSize = true;
            this.lblCmdVal.Location = new System.Drawing.Point(-3, 0);
            this.lblCmdVal.Name = "lblCmdVal";
            this.lblCmdVal.Size = new System.Drawing.Size(55, 13);
            this.lblCmdVal.TabIndex = 0;
            this.lblCmdVal.Text = "Значение";
            // 
            // rbCmdReq
            // 
            this.rbCmdReq.AutoSize = true;
            this.rbCmdReq.Location = new System.Drawing.Point(299, 34);
            this.rbCmdReq.Name = "rbCmdReq";
            this.rbCmdReq.Size = new System.Drawing.Size(75, 17);
            this.rbCmdReq.TabIndex = 6;
            this.rbCmdReq.Text = "Опрос КП";
            this.rbCmdReq.UseVisualStyleBackColor = true;
            this.rbCmdReq.CheckedChanged += new System.EventHandler(this.rbCmdType_CheckedChanged);
            // 
            // rbCmdBin
            // 
            this.rbCmdBin.AutoSize = true;
            this.rbCmdBin.Location = new System.Drawing.Point(204, 34);
            this.rbCmdBin.Name = "rbCmdBin";
            this.rbCmdBin.Size = new System.Drawing.Size(74, 17);
            this.rbCmdBin.TabIndex = 5;
            this.rbCmdBin.Text = "Бинарная";
            this.rbCmdBin.UseVisualStyleBackColor = true;
            this.rbCmdBin.CheckedChanged += new System.EventHandler(this.rbCmdType_CheckedChanged);
            // 
            // rbCmdStand
            // 
            this.rbCmdStand.AutoSize = true;
            this.rbCmdStand.Checked = true;
            this.rbCmdStand.Location = new System.Drawing.Point(109, 34);
            this.rbCmdStand.Name = "rbCmdStand";
            this.rbCmdStand.Size = new System.Drawing.Size(90, 17);
            this.rbCmdStand.TabIndex = 4;
            this.rbCmdStand.TabStop = true;
            this.rbCmdStand.Text = "Стандартная";
            this.rbCmdStand.UseVisualStyleBackColor = true;
            this.rbCmdStand.CheckedChanged += new System.EventHandler(this.rbCmdType_CheckedChanged);
            // 
            // numCmdNum
            // 
            this.numCmdNum.Location = new System.Drawing.Point(13, 32);
            this.numCmdNum.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numCmdNum.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numCmdNum.Name = "numCmdNum";
            this.numCmdNum.Size = new System.Drawing.Size(90, 20);
            this.numCmdNum.TabIndex = 1;
            this.numCmdNum.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblCmdNum
            // 
            this.lblCmdNum.AutoSize = true;
            this.lblCmdNum.Location = new System.Drawing.Point(10, 16);
            this.lblCmdNum.Name = "lblCmdNum";
            this.lblCmdNum.Size = new System.Drawing.Size(90, 13);
            this.lblCmdNum.TabIndex = 0;
            this.lblCmdNum.Text = "Номер команды";
            // 
            // gbCmdPwd
            // 
            this.gbCmdPwd.Controls.Add(this.txtCmdPwd);
            this.gbCmdPwd.Location = new System.Drawing.Point(6, 6);
            this.gbCmdPwd.Name = "gbCmdPwd";
            this.gbCmdPwd.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbCmdPwd.Size = new System.Drawing.Size(392, 52);
            this.gbCmdPwd.TabIndex = 0;
            this.gbCmdPwd.TabStop = false;
            this.gbCmdPwd.Text = "Пароль";
            // 
            // txtCmdPwd
            // 
            this.txtCmdPwd.Location = new System.Drawing.Point(13, 19);
            this.txtCmdPwd.Name = "txtCmdPwd";
            this.txtCmdPwd.Size = new System.Drawing.Size(150, 20);
            this.txtCmdPwd.TabIndex = 1;
            this.txtCmdPwd.UseSystemPasswordChar = true;
            this.txtCmdPwd.TextChanged += new System.EventHandler(this.txtCmdPwd_TextChanged);
            // 
            // pageStats
            // 
            this.pageStats.BackColor = System.Drawing.Color.Transparent;
            this.pageStats.Controls.Add(this.lbAppState);
            this.pageStats.Controls.Add(this.lbAppLog);
            this.pageStats.Controls.Add(this.lblAppLog);
            this.pageStats.Controls.Add(this.lblAppState);
            this.pageStats.Location = new System.Drawing.Point(4, 22);
            this.pageStats.Name = "pageStats";
            this.pageStats.Padding = new System.Windows.Forms.Padding(3);
            this.pageStats.Size = new System.Drawing.Size(406, 439);
            this.pageStats.TabIndex = 8;
            this.pageStats.Text = "Статистика";
            this.pageStats.UseVisualStyleBackColor = true;
            // 
            // lbAppState
            // 
            this.lbAppState.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbAppState.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbAppState.FormattingEnabled = true;
            this.lbAppState.HorizontalScrollbar = true;
            this.lbAppState.IntegralHeight = false;
            this.lbAppState.ItemHeight = 11;
            this.lbAppState.Location = new System.Drawing.Point(6, 22);
            this.lbAppState.Name = "lbAppState";
            this.lbAppState.Size = new System.Drawing.Size(392, 140);
            this.lbAppState.TabIndex = 1;
            this.lbAppState.KeyDown += new System.Windows.Forms.KeyEventHandler(this.LogListBox_KeyDown);
            // 
            // lbAppLog
            // 
            this.lbAppLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbAppLog.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbAppLog.FormattingEnabled = true;
            this.lbAppLog.HorizontalScrollbar = true;
            this.lbAppLog.IntegralHeight = false;
            this.lbAppLog.ItemHeight = 11;
            this.lbAppLog.Location = new System.Drawing.Point(6, 181);
            this.lbAppLog.Name = "lbAppLog";
            this.lbAppLog.Size = new System.Drawing.Size(392, 248);
            this.lbAppLog.TabIndex = 3;
            this.lbAppLog.KeyDown += new System.Windows.Forms.KeyEventHandler(this.LogListBox_KeyDown);
            // 
            // lblAppLog
            // 
            this.lblAppLog.AutoSize = true;
            this.lblAppLog.Location = new System.Drawing.Point(3, 165);
            this.lblAppLog.Name = "lblAppLog";
            this.lblAppLog.Size = new System.Drawing.Size(47, 13);
            this.lblAppLog.TabIndex = 2;
            this.lblAppLog.Text = "Журнал";
            // 
            // lblAppState
            // 
            this.lblAppState.AutoSize = true;
            this.lblAppState.Location = new System.Drawing.Point(3, 6);
            this.lblAppState.Name = "lblAppState";
            this.lblAppState.Size = new System.Drawing.Size(61, 13);
            this.lblAppState.TabIndex = 0;
            this.lblAppState.Text = "Состояние";
            // 
            // cmsKP
            // 
            this.cmsKP.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miKpProps});
            this.cmsKP.Name = "cmsKP";
            this.cmsKP.Size = new System.Drawing.Size(128, 26);
            this.cmsKP.Opened += new System.EventHandler(this.cmsKP_Opened);
            // 
            // miKpProps
            // 
            this.miKpProps.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.miKpProps.Image = ((System.Drawing.Image)(resources.GetObject("miKpProps.Image")));
            this.miKpProps.Name = "miKpProps";
            this.miKpProps.Size = new System.Drawing.Size(127, 22);
            this.miKpProps.Text = "Свойства";
            this.miKpProps.Click += new System.EventHandler(this.miKpProps_Click);
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.tabControl);
            this.pnlMain.Controls.Add(this.treeView);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 25);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(654, 465);
            this.pnlMain.TabIndex = 1;
            // 
            // lblSendModData
            // 
            this.lblSendModData.AutoSize = true;
            this.lblSendModData.Location = new System.Drawing.Point(13, 49);
            this.lblSendModData.Name = "lblSendModData";
            this.lblSendModData.Size = new System.Drawing.Size(228, 13);
            this.lblSendModData.TabIndex = 2;
            this.lblSendModData.Text = "Передавать только изменившиеся теги КП";
            // 
            // chkSendModData
            // 
            this.chkSendModData.AutoSize = true;
            this.chkSendModData.Location = new System.Drawing.Point(342, 48);
            this.chkSendModData.Name = "chkSendModData";
            this.chkSendModData.Size = new System.Drawing.Size(15, 14);
            this.chkSendModData.TabIndex = 3;
            this.chkSendModData.UseVisualStyleBackColor = true;
            this.chkSendModData.CheckedChanged += new System.EventHandler(this.chkSendModData_CheckedChanged);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(654, 512);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.statusStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1200, 1000);
            this.MinimumSize = new System.Drawing.Size(670, 550);
            this.Name = "FrmMain";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SCADA-Коммуникатор";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.Deactivate += new System.EventHandler(this.FrmMain_Deactivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMain_FormClosing);
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.cmsLine.ResumeLayout(false);
            this.cmsNotify.ResumeLayout(false);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.pageCommonParams.ResumeLayout(false);
            this.gbPerformance.ResumeLayout(false);
            this.gbPerformance.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSendAllDataPer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numWaitForStop)).EndInit();
            this.gbServer.ResumeLayout(false);
            this.gbServer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numServerTimeout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numServerPort)).EndInit();
            this.pageKpDlls.ResumeLayout(false);
            this.pageKpDlls.PerformLayout();
            this.pageLineParams.ResumeLayout(false);
            this.gbCommLine.ResumeLayout(false);
            this.gbCommLine.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numLineNumber)).EndInit();
            this.gbLineParams.ResumeLayout(false);
            this.gbLineParams.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCycleDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numReqTriesCnt)).EndInit();
            this.gbCommChannel.ResumeLayout(false);
            this.gbCommChannel.PerformLayout();
            this.pageCustomParams.ResumeLayout(false);
            this.gbSelectedParam.ResumeLayout(false);
            this.gbSelectedParam.PerformLayout();
            this.pageReqSequence.ResumeLayout(false);
            this.gbSelectedKP.ResumeLayout(false);
            this.gbSelectedKP.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numKpDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numKpTimeout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numKpAddress)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numKpNumber)).EndInit();
            this.pageLineState.ResumeLayout(false);
            this.pageLineLog.ResumeLayout(false);
            this.pageLineLog.PerformLayout();
            this.pageKpData.ResumeLayout(false);
            this.pageKpCmd.ResumeLayout(false);
            this.gbKpCmd.ResumeLayout(false);
            this.gbKpCmd.PerformLayout();
            this.pnlCmdData.ResumeLayout(false);
            this.pnlCmdData.PerformLayout();
            this.pnlCmdVal.ResumeLayout(false);
            this.pnlCmdVal.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCmdNum)).EndInit();
            this.gbCmdPwd.ResumeLayout(false);
            this.gbCmdPwd.PerformLayout();
            this.pageStats.ResumeLayout(false);
            this.pageStats.PerformLayout();
            this.cmsKP.ResumeLayout(false);
            this.pnlMain.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ImageList ilMain;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.Timer tmrRefr;
        private System.Windows.Forms.ContextMenuStrip cmsNotify;
        private System.Windows.Forms.ToolStripMenuItem miNotifyOpen;
        private System.Windows.Forms.ToolStripMenuItem miNotifyStart;
        private System.Windows.Forms.ToolStripMenuItem miNotifyStop;
        private System.Windows.Forms.ToolStripMenuItem miNotifyExit;
        private System.Windows.Forms.ToolStripSeparator miNotifySep;
        private System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripStatusLabel lblCurDate;
        private System.Windows.Forms.ToolStripStatusLabel lblCurTime;
        private System.Windows.Forms.ToolStripStatusLabel lblServiceState;
        private System.Windows.Forms.ToolStripButton btnServiceStart;
        private System.Windows.Forms.ToolStripButton btnServiceStop;
        private System.Windows.Forms.ToolStripButton btnServiceRestart;
        private System.Windows.Forms.ToolStripSeparator sepMain1;
        private System.Windows.Forms.ToolStripButton btnSettingsApply;
        private System.Windows.Forms.ToolStripButton btnSettingsCancel;
        private System.Windows.Forms.ToolStripButton btnAbout;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage pageCommonParams;
        private System.Windows.Forms.TabPage pageLineParams;
        private System.Windows.Forms.TabPage pageKpDlls;
        private System.Windows.Forms.GroupBox gbServer;
        private System.Windows.Forms.CheckBox chkServerUse;
        private System.Windows.Forms.TextBox txtServerHost;
        private System.Windows.Forms.Label lblServerHost;
        private System.Windows.Forms.Label lblServerPort;
        private System.Windows.Forms.NumericUpDown numServerPort;
        private System.Windows.Forms.NumericUpDown numServerTimeout;
        private System.Windows.Forms.Label lblServerTimeout;
        private System.Windows.Forms.TextBox txtServerUser;
        private System.Windows.Forms.Label lblServerUser;
        private System.Windows.Forms.TextBox txtServerPwd;
        private System.Windows.Forms.Label lblServerPwd;
        private System.Windows.Forms.GroupBox gbCommChannel;
        private System.Windows.Forms.GroupBox gbLineParams;
        private System.Windows.Forms.Label lblReqTriesCnt;
        private System.Windows.Forms.NumericUpDown numReqTriesCnt;
        private System.Windows.Forms.Label lblCycleDelay;
        private System.Windows.Forms.NumericUpDown numCycleDelay;
        private System.Windows.Forms.CheckBox chkCmdEnabled;
        private System.Windows.Forms.Label lblCmdEnabled;
        private System.Windows.Forms.ComboBox cbCommCnlType;
        private System.Windows.Forms.Label lblCommCnlType;
        private System.Windows.Forms.GroupBox gbCommLine;
        private System.Windows.Forms.CheckBox chkLineBind;
        private System.Windows.Forms.NumericUpDown numLineNumber;
        private System.Windows.Forms.TextBox txtLineName;
        private System.Windows.Forms.Label lnlLineName;
        private System.Windows.Forms.Label lblLineNumber;
        private System.Windows.Forms.CheckBox chkLineActive;
        private System.Windows.Forms.TabPage pageCustomParams;
        private System.Windows.Forms.Button btnAddParam;
        private System.Windows.Forms.ListView lvCustomParams;
        private System.Windows.Forms.ColumnHeader colParamName;
        private System.Windows.Forms.ColumnHeader colParamValue;
        private System.Windows.Forms.GroupBox gbSelectedParam;
        private System.Windows.Forms.TextBox txtParamName;
        private System.Windows.Forms.Label lblParamName;
        private System.Windows.Forms.Label lblParamValue;
        private System.Windows.Forms.TextBox txtParamValue;
        private System.Windows.Forms.Button btnDelParam;
        private System.Windows.Forms.ListBox lbKpDll;
        private System.Windows.Forms.Label lblKpDllDescr;
        private System.Windows.Forms.TextBox txtKpDllDescr;
        private System.Windows.Forms.Button btnKpDllProps;
        private System.Windows.Forms.TabPage pageReqSequence;
        private System.Windows.Forms.Button btnDelKP;
        private System.Windows.Forms.Button btnMoveDownKP;
        private System.Windows.Forms.Button btnMoveUpKP;
        private System.Windows.Forms.ListView lvReqSequence;
        private System.Windows.Forms.ColumnHeader colKpOrder;
        private System.Windows.Forms.ColumnHeader colKpNumber;
        private System.Windows.Forms.ColumnHeader colKpBind;
        private System.Windows.Forms.ColumnHeader colKpName;
        private System.Windows.Forms.Button btnAddKP;
        private System.Windows.Forms.Button btnPasteKP;
        private System.Windows.Forms.Button btnCopyKP;
        private System.Windows.Forms.ColumnHeader colKpDll;
        private System.Windows.Forms.ColumnHeader colKpAddress;
        private System.Windows.Forms.ColumnHeader colCallNum;
        private System.Windows.Forms.ColumnHeader colKpTimeout;
        private System.Windows.Forms.ColumnHeader colKpDelay;
        private System.Windows.Forms.ColumnHeader colKpTime;
        private System.Windows.Forms.ColumnHeader colKpPeriod;
        private System.Windows.Forms.ColumnHeader colKpCmdLine;
        private System.Windows.Forms.GroupBox gbSelectedKP;
        private System.Windows.Forms.ColumnHeader colKpActive;
        private System.Windows.Forms.CheckBox chkKpActive;
        private System.Windows.Forms.CheckBox chkKpBind;
        private System.Windows.Forms.Label lblKpNum;
        private System.Windows.Forms.TextBox txtKpName;
        private System.Windows.Forms.Label lblKpName;
        private System.Windows.Forms.NumericUpDown numKpNumber;
        private System.Windows.Forms.Button btnResetReqParams;
        private System.Windows.Forms.Label lblKpDll;
        private System.Windows.Forms.Label lblCallNum;
        private System.Windows.Forms.TextBox txtCallNum;
        private System.Windows.Forms.Label lblKpAddress;
        private System.Windows.Forms.NumericUpDown numKpAddress;
        private System.Windows.Forms.Label lblKpTimeout;
        private System.Windows.Forms.NumericUpDown numKpTimeout;
        private System.Windows.Forms.NumericUpDown numKpDelay;
        private System.Windows.Forms.Label lblKpDelay;
        private System.Windows.Forms.Label lblKpPeriod;
        private System.Windows.Forms.Label lblKpTime;
        private System.Windows.Forms.Label lblCmdLine;
        private System.Windows.Forms.DateTimePicker timeKpTime;
        private System.Windows.Forms.TextBox txtCmdLine;
        private System.Windows.Forms.DateTimePicker timeKpPeriod;
        private System.Windows.Forms.TabPage pageLineState;
        private System.Windows.Forms.TabPage pageLineLog;
        private System.Windows.Forms.TabPage pageKpData;
        private System.Windows.Forms.TabPage pageStats;
        private System.Windows.Forms.Label lblAppState;
        private System.Windows.Forms.Label lblAppLog;
        private System.Windows.Forms.Button btnKpProps;
        private System.Windows.Forms.ContextMenuStrip cmsLine;
        private System.Windows.Forms.ToolStripMenuItem miAddLine;
        private System.Windows.Forms.ToolStripMenuItem miMoveUpLine;
        private System.Windows.Forms.ToolStripMenuItem miMoveDownLine;
        private System.Windows.Forms.ToolStripMenuItem miDelLine;
        private System.Windows.Forms.ListBox lbAppLog;
        private System.Windows.Forms.ListBox lbAppState;
        private System.Windows.Forms.ListBox lbKpData;
        private System.Windows.Forms.ListBox lbLineLog;
        private System.Windows.Forms.ListBox lbLineState;
        private System.Windows.Forms.ComboBox cbKpDll;
        private System.Windows.Forms.ToolStripButton btnSettingsUpdate;
        private System.Windows.Forms.ToolStripSeparator sepMain2;
        private System.Windows.Forms.CheckBox chkLineLogPause;
        private System.Windows.Forms.GroupBox gbPerformance;
        private System.Windows.Forms.Label lblSendAllDataPer;
        private System.Windows.Forms.NumericUpDown numSendAllDataPer;
        private System.Windows.Forms.NumericUpDown numWaitForStop;
        private System.Windows.Forms.Label lblWaitForStop;
        private System.Windows.Forms.TabPage pageKpCmd;
        private System.Windows.Forms.GroupBox gbCmdPwd;
        private System.Windows.Forms.TextBox txtCmdPwd;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Button btnImportKP;
        private System.Windows.Forms.ToolStripMenuItem miImportLines;
        private System.Windows.Forms.ContextMenuStrip cmsKP;
        private System.Windows.Forms.ToolStripMenuItem miKpProps;
        private System.Windows.Forms.ToolStripSeparator sepLine;
        private System.Windows.Forms.ToolStripMenuItem miStartLine;
        private System.Windows.Forms.ToolStripMenuItem miStopLine;
        private System.Windows.Forms.ToolStripMenuItem miNotifyRestart;
        private System.Windows.Forms.ToolStripMenuItem miRestartLine;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.ImageList ilTree;
        private System.Windows.Forms.Button btnCutKP;
        private System.Windows.Forms.GroupBox gbKpCmd;
        private System.Windows.Forms.Button btnSendCmd;
        private System.Windows.Forms.Panel pnlCmdData;
        private System.Windows.Forms.TextBox txtCmdData;
        private System.Windows.Forms.RadioButton rbCmdHex;
        private System.Windows.Forms.RadioButton rbCmdStr;
        private System.Windows.Forms.Panel pnlCmdVal;
        private System.Windows.Forms.Button btnCmdValOn;
        private System.Windows.Forms.Button btnCmdValOff;
        private System.Windows.Forms.TextBox txtCmdVal;
        private System.Windows.Forms.Label lblCmdVal;
        private System.Windows.Forms.RadioButton rbCmdReq;
        private System.Windows.Forms.RadioButton rbCmdBin;
        private System.Windows.Forms.RadioButton rbCmdStand;
        private System.Windows.Forms.NumericUpDown numCmdNum;
        private System.Windows.Forms.Label lblCmdNum;
        private System.Windows.Forms.Label lblDetailedLog;
        private System.Windows.Forms.CheckBox chkDetailedLog;
        private System.Windows.Forms.Button btnCommCnlProps;
        private System.Windows.Forms.TextBox txtCommCnlParams;
        private System.Windows.Forms.Label lblCommCnlParams;
        private System.Windows.Forms.Label lblReqAfterCmd;
        private System.Windows.Forms.CheckBox chkReqAfterCmd;
        private System.Windows.Forms.Label lblSendModData;
        private System.Windows.Forms.CheckBox chkSendModData;
    }
}

