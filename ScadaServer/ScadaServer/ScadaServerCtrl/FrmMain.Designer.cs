namespace Scada.Server.Ctrl
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Общие параметры");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Параметры записи");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("База конфигурации");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Текущий срез");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Минутные срезы");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Часовые срезы");
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("События");
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("Файлы", new System.Windows.Forms.TreeNode[] {
            treeNode3,
            treeNode4,
            treeNode5,
            treeNode6,
            treeNode7});
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("Модули");
            System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("Генератор");
            System.Windows.Forms.TreeNode treeNode11 = new System.Windows.Forms.TreeNode("Статистика");
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.btnServiceStart = new System.Windows.Forms.ToolStripButton();
            this.btnServiceStop = new System.Windows.Forms.ToolStripButton();
            this.btnServiceRestart = new System.Windows.Forms.ToolStripButton();
            this.sepMain1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnSettingsApply = new System.Windows.Forms.ToolStripButton();
            this.btnSettingsCancel = new System.Windows.Forms.ToolStripButton();
            this.sepMain2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnAbout = new System.Windows.Forms.ToolStripButton();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.lblCurDate = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblCurTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblServiceState = new System.Windows.Forms.ToolStripStatusLabel();
            this.treeView = new System.Windows.Forms.TreeView();
            this.ilTree = new System.Windows.Forms.ImageList(this.components);
            this.tabControl = new System.Windows.Forms.TabControl();
            this.pageCommonParams = new System.Windows.Forms.TabPage();
            this.gbDirs = new System.Windows.Forms.GroupBox();
            this.btnArcCopyDir = new System.Windows.Forms.Button();
            this.txtArcCopyDir = new System.Windows.Forms.TextBox();
            this.lblArcCopyDir = new System.Windows.Forms.Label();
            this.btnArcDir = new System.Windows.Forms.Button();
            this.btnItfDir = new System.Windows.Forms.Button();
            this.btnBaseDATDir = new System.Windows.Forms.Button();
            this.txtArcDir = new System.Windows.Forms.TextBox();
            this.lblArcDir = new System.Windows.Forms.Label();
            this.txtItfDir = new System.Windows.Forms.TextBox();
            this.lblItfDir = new System.Windows.Forms.Label();
            this.txtBaseDATDir = new System.Windows.Forms.TextBox();
            this.lblBaseDATDir = new System.Windows.Forms.Label();
            this.gbConn = new System.Windows.Forms.GroupBox();
            this.lblTcpPort = new System.Windows.Forms.Label();
            this.numTcpPort = new System.Windows.Forms.NumericUpDown();
            this.lblLdapPath = new System.Windows.Forms.Label();
            this.chkUseAD = new System.Windows.Forms.CheckBox();
            this.txtLdapPath = new System.Windows.Forms.TextBox();
            this.pageSaveParams = new System.Windows.Forms.TabPage();
            this.gbSaveParamsEv = new System.Windows.Forms.GroupBox();
            this.chkWriteEvCopy = new System.Windows.Forms.CheckBox();
            this.chkWriteEv = new System.Windows.Forms.CheckBox();
            this.numStoreEvPer = new System.Windows.Forms.NumericUpDown();
            this.lblStoreEvPer = new System.Windows.Forms.Label();
            this.lblWriteEvPer = new System.Windows.Forms.Label();
            this.cbWriteEvPer = new System.Windows.Forms.ComboBox();
            this.gbSaveParamsHr = new System.Windows.Forms.GroupBox();
            this.chkWriteHrCopy = new System.Windows.Forms.CheckBox();
            this.chkWriteHr = new System.Windows.Forms.CheckBox();
            this.numStoreHrPer = new System.Windows.Forms.NumericUpDown();
            this.lblStoreHrPer = new System.Windows.Forms.Label();
            this.lblWriteHrPer = new System.Windows.Forms.Label();
            this.cbWriteHrPer = new System.Windows.Forms.ComboBox();
            this.gbSaveParamsMin = new System.Windows.Forms.GroupBox();
            this.chkWriteMinCopy = new System.Windows.Forms.CheckBox();
            this.chkWriteMin = new System.Windows.Forms.CheckBox();
            this.numStoreMinPer = new System.Windows.Forms.NumericUpDown();
            this.lblStoreMinPer = new System.Windows.Forms.Label();
            this.lblWriteMinPer = new System.Windows.Forms.Label();
            this.cbWriteMinPer = new System.Windows.Forms.ComboBox();
            this.gbSaveParamsCur = new System.Windows.Forms.GroupBox();
            this.cbInactUnrelTime = new System.Windows.Forms.ComboBox();
            this.chkWriteCurCopy = new System.Windows.Forms.CheckBox();
            this.chkWriteCur = new System.Windows.Forms.CheckBox();
            this.lblInactUnrelTime = new System.Windows.Forms.Label();
            this.lblWriteCurPer = new System.Windows.Forms.Label();
            this.cbWriteCurPer = new System.Windows.Forms.ComboBox();
            this.pageFiles = new System.Windows.Forms.TabPage();
            this.txtDataDir = new System.Windows.Forms.TextBox();
            this.rbFilesCopy = new System.Windows.Forms.RadioButton();
            this.rbFilesMain = new System.Windows.Forms.RadioButton();
            this.btnEditFile = new System.Windows.Forms.Button();
            this.btnViewFile = new System.Windows.Forms.Button();
            this.lbFiles = new System.Windows.Forms.ListBox();
            this.pageModules = new System.Windows.Forms.TabPage();
            this.btnDelMod = new System.Windows.Forms.Button();
            this.btnMoveDownMod = new System.Windows.Forms.Button();
            this.btnMoveUpMod = new System.Windows.Forms.Button();
            this.btnAddMod = new System.Windows.Forms.Button();
            this.btnModCommSettings = new System.Windows.Forms.Button();
            this.btnModProps = new System.Windows.Forms.Button();
            this.txtModDescr = new System.Windows.Forms.TextBox();
            this.lblModDescr = new System.Windows.Forms.Label();
            this.lbModDll = new System.Windows.Forms.ListBox();
            this.pageGenSrez = new System.Windows.Forms.TabPage();
            this.gbGenCommSettings1 = new System.Windows.Forms.GroupBox();
            this.btnGenCommSettings1 = new System.Windows.Forms.Button();
            this.gbGenPwd1 = new System.Windows.Forms.GroupBox();
            this.txtGenPwd1 = new System.Windows.Forms.TextBox();
            this.gbGenSrez = new System.Windows.Forms.GroupBox();
            this.btnSendSrez = new System.Windows.Forms.Button();
            this.numSrezCnlStat = new System.Windows.Forms.NumericUpDown();
            this.btnSrezCnlStatOne = new System.Windows.Forms.Button();
            this.btnSrezCnlStatZero = new System.Windows.Forms.Button();
            this.lblSrezCnlStat = new System.Windows.Forms.Label();
            this.btnSrezValPlusOne = new System.Windows.Forms.Button();
            this.btnSrezValMinusOne = new System.Windows.Forms.Button();
            this.lblSrezCnlVal = new System.Windows.Forms.Label();
            this.txtSrezCnlVal = new System.Windows.Forms.TextBox();
            this.cbSrezCnlNum = new System.Windows.Forms.ComboBox();
            this.lblSrezCnlNum = new System.Windows.Forms.Label();
            this.dtpSrezTime = new System.Windows.Forms.DateTimePicker();
            this.dtpSrezDate = new System.Windows.Forms.DateTimePicker();
            this.rbArcSrez = new System.Windows.Forms.RadioButton();
            this.rbCurSrez = new System.Windows.Forms.RadioButton();
            this.pageGenEv = new System.Windows.Forms.TabPage();
            this.gbGenCommSettings2 = new System.Windows.Forms.GroupBox();
            this.btnGenCommSettings2 = new System.Windows.Forms.Button();
            this.gbGenPwd2 = new System.Windows.Forms.GroupBox();
            this.txtGenPwd2 = new System.Windows.Forms.TextBox();
            this.gbCheckEv = new System.Windows.Forms.GroupBox();
            this.btnCheckEvent = new System.Windows.Forms.Button();
            this.lblEvUserID2 = new System.Windows.Forms.Label();
            this.numEvUserID2 = new System.Windows.Forms.NumericUpDown();
            this.lblEvDate2 = new System.Windows.Forms.Label();
            this.dtpEvDate2 = new System.Windows.Forms.DateTimePicker();
            this.numEvNum = new System.Windows.Forms.NumericUpDown();
            this.lblEvNum = new System.Windows.Forms.Label();
            this.gbGenEv = new System.Windows.Forms.GroupBox();
            this.btnSendEvent = new System.Windows.Forms.Button();
            this.txtEvData = new System.Windows.Forms.TextBox();
            this.lblEvData = new System.Windows.Forms.Label();
            this.txtEvDescr = new System.Windows.Forms.TextBox();
            this.lblEvDescr = new System.Windows.Forms.Label();
            this.lblEvNewCnlVal = new System.Windows.Forms.Label();
            this.txtEvNewCnlVal = new System.Windows.Forms.TextBox();
            this.numEvUserID1 = new System.Windows.Forms.NumericUpDown();
            this.lblEvUserID1 = new System.Windows.Forms.Label();
            this.numEvNewCnlStat = new System.Windows.Forms.NumericUpDown();
            this.lblEvNewCnlStat = new System.Windows.Forms.Label();
            this.numEvOldCnlStat = new System.Windows.Forms.NumericUpDown();
            this.lblEvOldCnlStat = new System.Windows.Forms.Label();
            this.lblEvOldCnlVal = new System.Windows.Forms.Label();
            this.txtEvOldCnlVal = new System.Windows.Forms.TextBox();
            this.lblEvCnlNum = new System.Windows.Forms.Label();
            this.numEvCnlNum = new System.Windows.Forms.NumericUpDown();
            this.numEvParamID = new System.Windows.Forms.NumericUpDown();
            this.lblEvParamID = new System.Windows.Forms.Label();
            this.numEvKPNum = new System.Windows.Forms.NumericUpDown();
            this.lblEvKPNum = new System.Windows.Forms.Label();
            this.numEvObjNum = new System.Windows.Forms.NumericUpDown();
            this.lblEvObjNum = new System.Windows.Forms.Label();
            this.btnSetEvDT = new System.Windows.Forms.Button();
            this.lblEvTime = new System.Windows.Forms.Label();
            this.lblEvDate1 = new System.Windows.Forms.Label();
            this.dtpEvTime = new System.Windows.Forms.DateTimePicker();
            this.dtpEvDate1 = new System.Windows.Forms.DateTimePicker();
            this.pageGenCmd = new System.Windows.Forms.TabPage();
            this.gbGenCmd = new System.Windows.Forms.GroupBox();
            this.pnlCmdKP = new System.Windows.Forms.Panel();
            this.numCmdKPNum = new System.Windows.Forms.NumericUpDown();
            this.lblCmdKPNum = new System.Windows.Forms.Label();
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
            this.numCmdCtrlCnlNum = new System.Windows.Forms.NumericUpDown();
            this.lblCmdCtrlCnlNum = new System.Windows.Forms.Label();
            this.numCmdUserID = new System.Windows.Forms.NumericUpDown();
            this.lblCmdUserID = new System.Windows.Forms.Label();
            this.gbGenCommSettings3 = new System.Windows.Forms.GroupBox();
            this.btnGenCommSettings3 = new System.Windows.Forms.Button();
            this.gbGenPwd3 = new System.Windows.Forms.GroupBox();
            this.txtGenPwd3 = new System.Windows.Forms.TextBox();
            this.pageStats = new System.Windows.Forms.TabPage();
            this.chkDetailedLog = new System.Windows.Forms.CheckBox();
            this.lbAppState = new System.Windows.Forms.ListBox();
            this.lbAppLog = new System.Windows.Forms.ListBox();
            this.lblAppLog = new System.Windows.Forms.Label();
            this.lblAppState = new System.Windows.Forms.Label();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.cmsNotify = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miNotifyOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.miNotifyStart = new System.Windows.Forms.ToolStripMenuItem();
            this.miNotifyStop = new System.Windows.Forms.ToolStripMenuItem();
            this.miNotifyRestart = new System.Windows.Forms.ToolStripMenuItem();
            this.miNotifySep = new System.Windows.Forms.ToolStripSeparator();
            this.miNotifyExit = new System.Windows.Forms.ToolStripMenuItem();
            this.tmrRefr = new System.Windows.Forms.Timer(this.components);
            this.ilNotify = new System.Windows.Forms.ImageList(this.components);
            this.dlgDir = new System.Windows.Forms.FolderBrowserDialog();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.dlgMod = new System.Windows.Forms.OpenFileDialog();
            this.toolStrip.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.pageCommonParams.SuspendLayout();
            this.gbDirs.SuspendLayout();
            this.gbConn.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTcpPort)).BeginInit();
            this.pageSaveParams.SuspendLayout();
            this.gbSaveParamsEv.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numStoreEvPer)).BeginInit();
            this.gbSaveParamsHr.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numStoreHrPer)).BeginInit();
            this.gbSaveParamsMin.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numStoreMinPer)).BeginInit();
            this.gbSaveParamsCur.SuspendLayout();
            this.pageFiles.SuspendLayout();
            this.pageModules.SuspendLayout();
            this.pageGenSrez.SuspendLayout();
            this.gbGenCommSettings1.SuspendLayout();
            this.gbGenPwd1.SuspendLayout();
            this.gbGenSrez.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSrezCnlStat)).BeginInit();
            this.pageGenEv.SuspendLayout();
            this.gbGenCommSettings2.SuspendLayout();
            this.gbGenPwd2.SuspendLayout();
            this.gbCheckEv.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numEvUserID2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numEvNum)).BeginInit();
            this.gbGenEv.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numEvUserID1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numEvNewCnlStat)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numEvOldCnlStat)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numEvCnlNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numEvParamID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numEvKPNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numEvObjNum)).BeginInit();
            this.pageGenCmd.SuspendLayout();
            this.gbGenCmd.SuspendLayout();
            this.pnlCmdKP.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCmdKPNum)).BeginInit();
            this.pnlCmdData.SuspendLayout();
            this.pnlCmdVal.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCmdCtrlCnlNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCmdUserID)).BeginInit();
            this.gbGenCommSettings3.SuspendLayout();
            this.gbGenPwd3.SuspendLayout();
            this.pageStats.SuspendLayout();
            this.cmsNotify.SuspendLayout();
            this.SuspendLayout();
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
            this.sepMain2,
            this.btnAbout});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(594, 25);
            this.toolStrip.TabIndex = 0;
            // 
            // btnServiceStart
            // 
            this.btnServiceStart.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnServiceStart.Image = ((System.Drawing.Image)(resources.GetObject("btnServiceStart.Image")));
            this.btnServiceStart.ImageTransparentColor = System.Drawing.Color.White;
            this.btnServiceStart.Name = "btnServiceStart";
            this.btnServiceStart.Size = new System.Drawing.Size(23, 22);
            this.btnServiceStart.ToolTipText = "Запустить службу SCADA-Сервера";
            this.btnServiceStart.Click += new System.EventHandler(this.btnServiceStart_Click);
            // 
            // btnServiceStop
            // 
            this.btnServiceStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnServiceStop.Image = ((System.Drawing.Image)(resources.GetObject("btnServiceStop.Image")));
            this.btnServiceStop.ImageTransparentColor = System.Drawing.Color.White;
            this.btnServiceStop.Name = "btnServiceStop";
            this.btnServiceStop.Size = new System.Drawing.Size(23, 22);
            this.btnServiceStop.ToolTipText = "Остановить службу SCADA-Сервера";
            this.btnServiceStop.Click += new System.EventHandler(this.btnServiceStop_Click);
            // 
            // btnServiceRestart
            // 
            this.btnServiceRestart.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnServiceRestart.Image = ((System.Drawing.Image)(resources.GetObject("btnServiceRestart.Image")));
            this.btnServiceRestart.ImageTransparentColor = System.Drawing.Color.White;
            this.btnServiceRestart.Name = "btnServiceRestart";
            this.btnServiceRestart.Size = new System.Drawing.Size(23, 22);
            this.btnServiceRestart.ToolTipText = "Перезапустить службу SCADA-Сервера";
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
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblCurDate,
            this.lblCurTime,
            this.lblServiceState});
            this.statusStrip.Location = new System.Drawing.Point(0, 430);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(594, 22);
            this.statusStrip.SizingGrip = false;
            this.statusStrip.TabIndex = 3;
            // 
            // lblCurDate
            // 
            this.lblCurDate.AutoSize = false;
            this.lblCurDate.Name = "lblCurDate";
            this.lblCurDate.Size = new System.Drawing.Size(75, 17);
            this.lblCurDate.Text = "07.08.2013";
            // 
            // lblCurTime
            // 
            this.lblCurTime.AutoSize = false;
            this.lblCurTime.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)));
            this.lblCurTime.Name = "lblCurTime";
            this.lblCurTime.Size = new System.Drawing.Size(75, 17);
            this.lblCurTime.Text = "17:25:00";
            // 
            // lblServiceState
            // 
            this.lblServiceState.Name = "lblServiceState";
            this.lblServiceState.Size = new System.Drawing.Size(281, 17);
            this.lblServiceState.Text = "Состояние службы SCADA-Сервера: остановлена";
            // 
            // treeView
            // 
            this.treeView.HideSelection = false;
            this.treeView.ImageKey = "empty.png";
            this.treeView.ImageList = this.ilTree;
            this.treeView.Location = new System.Drawing.Point(0, 25);
            this.treeView.Margin = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.treeView.Name = "treeView";
            treeNode1.ImageKey = "params.png";
            treeNode1.Name = "nodeCommonParams";
            treeNode1.SelectedImageKey = "params.png";
            treeNode1.Text = "Общие параметры";
            treeNode2.ImageKey = "save.png";
            treeNode2.Name = "nodeSaveParams";
            treeNode2.SelectedImageKey = "save.png";
            treeNode2.Text = "Параметры записи";
            treeNode3.ImageKey = "base_table.png";
            treeNode3.Name = "nodeBase";
            treeNode3.SelectedImageKey = "base_table.png";
            treeNode3.Text = "База конфигурации";
            treeNode4.ImageKey = "srez_table.png";
            treeNode4.Name = "nodeCurSrez";
            treeNode4.SelectedImageKey = "srez_table.png";
            treeNode4.Text = "Текущий срез";
            treeNode5.ImageKey = "srez_table.png";
            treeNode5.Name = "nodeMinSrez";
            treeNode5.SelectedImageKey = "srez_table.png";
            treeNode5.Text = "Минутные срезы";
            treeNode6.ImageKey = "srez_table.png";
            treeNode6.Name = "nodeHrSrez";
            treeNode6.SelectedImageKey = "srez_table.png";
            treeNode6.Text = "Часовые срезы";
            treeNode7.ImageKey = "event.png";
            treeNode7.Name = "nodeEvents";
            treeNode7.SelectedImageKey = "event.png";
            treeNode7.Text = "События";
            treeNode8.ImageKey = "db.png";
            treeNode8.Name = "nodeFiles";
            treeNode8.SelectedImageKey = "db.png";
            treeNode8.Text = "Файлы";
            treeNode9.ImageKey = "module.png";
            treeNode9.Name = "nodeModules";
            treeNode9.SelectedImageKey = "module.png";
            treeNode9.Text = "Модули";
            treeNode10.ImageKey = "hand.png";
            treeNode10.Name = "nodeGenerator";
            treeNode10.SelectedImageKey = "hand.png";
            treeNode10.Text = "Генератор";
            treeNode11.ImageKey = "stats.png";
            treeNode11.Name = "nodeStats";
            treeNode11.SelectedImageKey = "stats.png";
            treeNode11.Text = "Статистика";
            this.treeView.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode8,
            treeNode9,
            treeNode10,
            treeNode11});
            this.treeView.SelectedImageKey = "empty.png";
            this.treeView.ShowRootLines = false;
            this.treeView.Size = new System.Drawing.Size(160, 403);
            this.treeView.TabIndex = 1;
            this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterSelect);
            this.treeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView_NodeMouseClick);
            // 
            // ilTree
            // 
            this.ilTree.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilTree.ImageStream")));
            this.ilTree.TransparentColor = System.Drawing.Color.Transparent;
            this.ilTree.Images.SetKeyName(0, "empty.png");
            this.ilTree.Images.SetKeyName(1, "stats.png");
            this.ilTree.Images.SetKeyName(2, "hand.png");
            this.ilTree.Images.SetKeyName(3, "module.png");
            this.ilTree.Images.SetKeyName(4, "params.png");
            this.ilTree.Images.SetKeyName(5, "event.png");
            this.ilTree.Images.SetKeyName(6, "save.png");
            this.ilTree.Images.SetKeyName(7, "db.png");
            this.ilTree.Images.SetKeyName(8, "base_table.png");
            this.ilTree.Images.SetKeyName(9, "srez_table.png");
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.pageCommonParams);
            this.tabControl.Controls.Add(this.pageSaveParams);
            this.tabControl.Controls.Add(this.pageFiles);
            this.tabControl.Controls.Add(this.pageModules);
            this.tabControl.Controls.Add(this.pageGenSrez);
            this.tabControl.Controls.Add(this.pageGenEv);
            this.tabControl.Controls.Add(this.pageGenCmd);
            this.tabControl.Controls.Add(this.pageStats);
            this.tabControl.Location = new System.Drawing.Point(160, 25);
            this.tabControl.Margin = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(434, 404);
            this.tabControl.TabIndex = 2;
            // 
            // pageCommonParams
            // 
            this.pageCommonParams.BackColor = System.Drawing.Color.Transparent;
            this.pageCommonParams.Controls.Add(this.gbDirs);
            this.pageCommonParams.Controls.Add(this.gbConn);
            this.pageCommonParams.Location = new System.Drawing.Point(4, 22);
            this.pageCommonParams.Name = "pageCommonParams";
            this.pageCommonParams.Padding = new System.Windows.Forms.Padding(3);
            this.pageCommonParams.Size = new System.Drawing.Size(426, 378);
            this.pageCommonParams.TabIndex = 2;
            this.pageCommonParams.Text = "Общие параметры";
            this.pageCommonParams.UseVisualStyleBackColor = true;
            // 
            // gbDirs
            // 
            this.gbDirs.Controls.Add(this.btnArcCopyDir);
            this.gbDirs.Controls.Add(this.txtArcCopyDir);
            this.gbDirs.Controls.Add(this.lblArcCopyDir);
            this.gbDirs.Controls.Add(this.btnArcDir);
            this.gbDirs.Controls.Add(this.btnItfDir);
            this.gbDirs.Controls.Add(this.btnBaseDATDir);
            this.gbDirs.Controls.Add(this.txtArcDir);
            this.gbDirs.Controls.Add(this.lblArcDir);
            this.gbDirs.Controls.Add(this.txtItfDir);
            this.gbDirs.Controls.Add(this.lblItfDir);
            this.gbDirs.Controls.Add(this.txtBaseDATDir);
            this.gbDirs.Controls.Add(this.lblBaseDATDir);
            this.gbDirs.Location = new System.Drawing.Point(6, 77);
            this.gbDirs.Name = "gbDirs";
            this.gbDirs.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbDirs.Size = new System.Drawing.Size(414, 182);
            this.gbDirs.TabIndex = 1;
            this.gbDirs.TabStop = false;
            this.gbDirs.Text = "Директории";
            // 
            // btnArcCopyDir
            // 
            this.btnArcCopyDir.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnArcCopyDir.Image = ((System.Drawing.Image)(resources.GetObject("btnArcCopyDir.Image")));
            this.btnArcCopyDir.Location = new System.Drawing.Point(381, 149);
            this.btnArcCopyDir.Name = "btnArcCopyDir";
            this.btnArcCopyDir.Size = new System.Drawing.Size(20, 20);
            this.btnArcCopyDir.TabIndex = 11;
            this.btnArcCopyDir.UseVisualStyleBackColor = true;
            this.btnArcCopyDir.Click += new System.EventHandler(this.btnDir_Click);
            // 
            // txtArcCopyDir
            // 
            this.txtArcCopyDir.Location = new System.Drawing.Point(13, 149);
            this.txtArcCopyDir.Name = "txtArcCopyDir";
            this.txtArcCopyDir.Size = new System.Drawing.Size(362, 20);
            this.txtArcCopyDir.TabIndex = 10;
            this.txtArcCopyDir.TextChanged += new System.EventHandler(this.control_Changed);
            // 
            // lblArcCopyDir
            // 
            this.lblArcCopyDir.AutoSize = true;
            this.lblArcCopyDir.Location = new System.Drawing.Point(10, 133);
            this.lblArcCopyDir.Name = "lblArcCopyDir";
            this.lblArcCopyDir.Size = new System.Drawing.Size(158, 13);
            this.lblArcCopyDir.TabIndex = 9;
            this.lblArcCopyDir.Text = "Копия архива в формате DAT";
            // 
            // btnArcDir
            // 
            this.btnArcDir.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnArcDir.Image = ((System.Drawing.Image)(resources.GetObject("btnArcDir.Image")));
            this.btnArcDir.Location = new System.Drawing.Point(381, 110);
            this.btnArcDir.Name = "btnArcDir";
            this.btnArcDir.Size = new System.Drawing.Size(20, 20);
            this.btnArcDir.TabIndex = 8;
            this.btnArcDir.UseVisualStyleBackColor = true;
            this.btnArcDir.Click += new System.EventHandler(this.btnDir_Click);
            // 
            // btnItfDir
            // 
            this.btnItfDir.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnItfDir.Image = ((System.Drawing.Image)(resources.GetObject("btnItfDir.Image")));
            this.btnItfDir.Location = new System.Drawing.Point(381, 71);
            this.btnItfDir.Name = "btnItfDir";
            this.btnItfDir.Size = new System.Drawing.Size(20, 20);
            this.btnItfDir.TabIndex = 5;
            this.btnItfDir.UseVisualStyleBackColor = true;
            this.btnItfDir.Click += new System.EventHandler(this.btnDir_Click);
            // 
            // btnBaseDATDir
            // 
            this.btnBaseDATDir.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnBaseDATDir.Image = ((System.Drawing.Image)(resources.GetObject("btnBaseDATDir.Image")));
            this.btnBaseDATDir.Location = new System.Drawing.Point(381, 32);
            this.btnBaseDATDir.Name = "btnBaseDATDir";
            this.btnBaseDATDir.Size = new System.Drawing.Size(20, 20);
            this.btnBaseDATDir.TabIndex = 2;
            this.btnBaseDATDir.UseVisualStyleBackColor = true;
            this.btnBaseDATDir.Click += new System.EventHandler(this.btnDir_Click);
            // 
            // txtArcDir
            // 
            this.txtArcDir.Location = new System.Drawing.Point(13, 110);
            this.txtArcDir.Name = "txtArcDir";
            this.txtArcDir.Size = new System.Drawing.Size(362, 20);
            this.txtArcDir.TabIndex = 7;
            this.txtArcDir.TextChanged += new System.EventHandler(this.control_Changed);
            // 
            // lblArcDir
            // 
            this.lblArcDir.AutoSize = true;
            this.lblArcDir.Location = new System.Drawing.Point(10, 94);
            this.lblArcDir.Name = "lblArcDir";
            this.lblArcDir.Size = new System.Drawing.Size(119, 13);
            this.lblArcDir.TabIndex = 6;
            this.lblArcDir.Text = "Архив в формате DAT";
            // 
            // txtItfDir
            // 
            this.txtItfDir.Location = new System.Drawing.Point(13, 71);
            this.txtItfDir.Name = "txtItfDir";
            this.txtItfDir.Size = new System.Drawing.Size(362, 20);
            this.txtItfDir.TabIndex = 4;
            this.txtItfDir.TextChanged += new System.EventHandler(this.control_Changed);
            // 
            // lblItfDir
            // 
            this.lblItfDir.AutoSize = true;
            this.lblItfDir.Location = new System.Drawing.Point(10, 55);
            this.lblItfDir.Name = "lblItfDir";
            this.lblItfDir.Size = new System.Drawing.Size(64, 13);
            this.lblItfDir.TabIndex = 3;
            this.lblItfDir.Text = "Интерфейс";
            // 
            // txtBaseDATDir
            // 
            this.txtBaseDATDir.Location = new System.Drawing.Point(13, 32);
            this.txtBaseDATDir.Name = "txtBaseDATDir";
            this.txtBaseDATDir.Size = new System.Drawing.Size(362, 20);
            this.txtBaseDATDir.TabIndex = 1;
            this.txtBaseDATDir.TextChanged += new System.EventHandler(this.control_Changed);
            // 
            // lblBaseDATDir
            // 
            this.lblBaseDATDir.AutoSize = true;
            this.lblBaseDATDir.Location = new System.Drawing.Point(10, 16);
            this.lblBaseDATDir.Name = "lblBaseDATDir";
            this.lblBaseDATDir.Size = new System.Drawing.Size(189, 13);
            this.lblBaseDATDir.TabIndex = 0;
            this.lblBaseDATDir.Text = "База конфигурации в формате DAT";
            // 
            // gbConn
            // 
            this.gbConn.Controls.Add(this.lblTcpPort);
            this.gbConn.Controls.Add(this.numTcpPort);
            this.gbConn.Controls.Add(this.lblLdapPath);
            this.gbConn.Controls.Add(this.chkUseAD);
            this.gbConn.Controls.Add(this.txtLdapPath);
            this.gbConn.Location = new System.Drawing.Point(6, 6);
            this.gbConn.Name = "gbConn";
            this.gbConn.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbConn.Size = new System.Drawing.Size(414, 65);
            this.gbConn.TabIndex = 0;
            this.gbConn.TabStop = false;
            this.gbConn.Text = "Соединение";
            // 
            // lblTcpPort
            // 
            this.lblTcpPort.AutoSize = true;
            this.lblTcpPort.Location = new System.Drawing.Point(10, 16);
            this.lblTcpPort.Name = "lblTcpPort";
            this.lblTcpPort.Size = new System.Drawing.Size(54, 13);
            this.lblTcpPort.TabIndex = 0;
            this.lblTcpPort.Text = "TCP-порт";
            // 
            // numTcpPort
            // 
            this.numTcpPort.Location = new System.Drawing.Point(13, 32);
            this.numTcpPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numTcpPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numTcpPort.Name = "numTcpPort";
            this.numTcpPort.Size = new System.Drawing.Size(100, 20);
            this.numTcpPort.TabIndex = 1;
            this.numTcpPort.Value = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numTcpPort.ValueChanged += new System.EventHandler(this.control_Changed);
            // 
            // lblLdapPath
            // 
            this.lblLdapPath.AutoSize = true;
            this.lblLdapPath.Location = new System.Drawing.Point(137, 16);
            this.lblLdapPath.Name = "lblLdapPath";
            this.lblLdapPath.Size = new System.Drawing.Size(108, 13);
            this.lblLdapPath.TabIndex = 3;
            this.lblLdapPath.Text = "Контроллер домена";
            // 
            // chkUseAD
            // 
            this.chkUseAD.AutoSize = true;
            this.chkUseAD.Location = new System.Drawing.Point(120, 35);
            this.chkUseAD.Name = "chkUseAD";
            this.chkUseAD.Size = new System.Drawing.Size(15, 14);
            this.chkUseAD.TabIndex = 2;
            this.chkUseAD.UseVisualStyleBackColor = true;
            this.chkUseAD.CheckedChanged += new System.EventHandler(this.control_Changed);
            // 
            // txtLdapPath
            // 
            this.txtLdapPath.Location = new System.Drawing.Point(140, 32);
            this.txtLdapPath.Name = "txtLdapPath";
            this.txtLdapPath.Size = new System.Drawing.Size(261, 20);
            this.txtLdapPath.TabIndex = 4;
            this.txtLdapPath.TextChanged += new System.EventHandler(this.control_Changed);
            // 
            // pageSaveParams
            // 
            this.pageSaveParams.Controls.Add(this.gbSaveParamsEv);
            this.pageSaveParams.Controls.Add(this.gbSaveParamsHr);
            this.pageSaveParams.Controls.Add(this.gbSaveParamsMin);
            this.pageSaveParams.Controls.Add(this.gbSaveParamsCur);
            this.pageSaveParams.Location = new System.Drawing.Point(4, 22);
            this.pageSaveParams.Name = "pageSaveParams";
            this.pageSaveParams.Padding = new System.Windows.Forms.Padding(3);
            this.pageSaveParams.Size = new System.Drawing.Size(426, 378);
            this.pageSaveParams.TabIndex = 3;
            this.pageSaveParams.Text = "Параметры записи";
            this.pageSaveParams.UseVisualStyleBackColor = true;
            // 
            // gbSaveParamsEv
            // 
            this.gbSaveParamsEv.Controls.Add(this.chkWriteEvCopy);
            this.gbSaveParamsEv.Controls.Add(this.chkWriteEv);
            this.gbSaveParamsEv.Controls.Add(this.numStoreEvPer);
            this.gbSaveParamsEv.Controls.Add(this.lblStoreEvPer);
            this.gbSaveParamsEv.Controls.Add(this.lblWriteEvPer);
            this.gbSaveParamsEv.Controls.Add(this.cbWriteEvPer);
            this.gbSaveParamsEv.Location = new System.Drawing.Point(216, 163);
            this.gbSaveParamsEv.Name = "gbSaveParamsEv";
            this.gbSaveParamsEv.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbSaveParamsEv.Size = new System.Drawing.Size(204, 152);
            this.gbSaveParamsEv.TabIndex = 3;
            this.gbSaveParamsEv.TabStop = false;
            this.gbSaveParamsEv.Text = "События";
            // 
            // chkWriteEvCopy
            // 
            this.chkWriteEvCopy.AutoSize = true;
            this.chkWriteEvCopy.Checked = true;
            this.chkWriteEvCopy.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkWriteEvCopy.Location = new System.Drawing.Point(13, 122);
            this.chkWriteEvCopy.Name = "chkWriteEvCopy";
            this.chkWriteEvCopy.Size = new System.Drawing.Size(136, 17);
            this.chkWriteEvCopy.TabIndex = 5;
            this.chkWriteEvCopy.Text = "Запись копии данных";
            this.chkWriteEvCopy.UseVisualStyleBackColor = true;
            this.chkWriteEvCopy.CheckedChanged += new System.EventHandler(this.control_Changed);
            // 
            // chkWriteEv
            // 
            this.chkWriteEv.AutoSize = true;
            this.chkWriteEv.Checked = true;
            this.chkWriteEv.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkWriteEv.Location = new System.Drawing.Point(13, 99);
            this.chkWriteEv.Name = "chkWriteEv";
            this.chkWriteEv.Size = new System.Drawing.Size(103, 17);
            this.chkWriteEv.TabIndex = 4;
            this.chkWriteEv.Text = "Запись данных";
            this.chkWriteEv.UseVisualStyleBackColor = true;
            this.chkWriteEv.CheckedChanged += new System.EventHandler(this.control_Changed);
            // 
            // numStoreEvPer
            // 
            this.numStoreEvPer.Location = new System.Drawing.Point(13, 72);
            this.numStoreEvPer.Maximum = new decimal(new int[] {
            3650,
            0,
            0,
            0});
            this.numStoreEvPer.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numStoreEvPer.Name = "numStoreEvPer";
            this.numStoreEvPer.Size = new System.Drawing.Size(178, 20);
            this.numStoreEvPer.TabIndex = 3;
            this.numStoreEvPer.Value = new decimal(new int[] {
            365,
            0,
            0,
            0});
            this.numStoreEvPer.ValueChanged += new System.EventHandler(this.control_Changed);
            // 
            // lblStoreEvPer
            // 
            this.lblStoreEvPer.AutoSize = true;
            this.lblStoreEvPer.Location = new System.Drawing.Point(10, 56);
            this.lblStoreEvPer.Name = "lblStoreEvPer";
            this.lblStoreEvPer.Size = new System.Drawing.Size(125, 13);
            this.lblStoreEvPer.TabIndex = 2;
            this.lblStoreEvPer.Text = "Период хранения, дней";
            // 
            // lblWriteEvPer
            // 
            this.lblWriteEvPer.AutoSize = true;
            this.lblWriteEvPer.Location = new System.Drawing.Point(10, 16);
            this.lblWriteEvPer.Name = "lblWriteEvPer";
            this.lblWriteEvPer.Size = new System.Drawing.Size(84, 13);
            this.lblWriteEvPer.TabIndex = 0;
            this.lblWriteEvPer.Text = "Период записи";
            // 
            // cbWriteEvPer
            // 
            this.cbWriteEvPer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWriteEvPer.Enabled = false;
            this.cbWriteEvPer.FormattingEnabled = true;
            this.cbWriteEvPer.Items.AddRange(new object[] {
            "По изменению"});
            this.cbWriteEvPer.Location = new System.Drawing.Point(13, 32);
            this.cbWriteEvPer.Name = "cbWriteEvPer";
            this.cbWriteEvPer.Size = new System.Drawing.Size(178, 21);
            this.cbWriteEvPer.TabIndex = 1;
            // 
            // gbSaveParamsHr
            // 
            this.gbSaveParamsHr.Controls.Add(this.chkWriteHrCopy);
            this.gbSaveParamsHr.Controls.Add(this.chkWriteHr);
            this.gbSaveParamsHr.Controls.Add(this.numStoreHrPer);
            this.gbSaveParamsHr.Controls.Add(this.lblStoreHrPer);
            this.gbSaveParamsHr.Controls.Add(this.lblWriteHrPer);
            this.gbSaveParamsHr.Controls.Add(this.cbWriteHrPer);
            this.gbSaveParamsHr.Location = new System.Drawing.Point(6, 163);
            this.gbSaveParamsHr.Name = "gbSaveParamsHr";
            this.gbSaveParamsHr.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbSaveParamsHr.Size = new System.Drawing.Size(204, 152);
            this.gbSaveParamsHr.TabIndex = 2;
            this.gbSaveParamsHr.TabStop = false;
            this.gbSaveParamsHr.Text = "Часовые срезы";
            // 
            // chkWriteHrCopy
            // 
            this.chkWriteHrCopy.AutoSize = true;
            this.chkWriteHrCopy.Checked = true;
            this.chkWriteHrCopy.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkWriteHrCopy.Location = new System.Drawing.Point(13, 122);
            this.chkWriteHrCopy.Name = "chkWriteHrCopy";
            this.chkWriteHrCopy.Size = new System.Drawing.Size(136, 17);
            this.chkWriteHrCopy.TabIndex = 5;
            this.chkWriteHrCopy.Text = "Запись копии данных";
            this.chkWriteHrCopy.UseVisualStyleBackColor = true;
            this.chkWriteHrCopy.CheckedChanged += new System.EventHandler(this.control_Changed);
            // 
            // chkWriteHr
            // 
            this.chkWriteHr.AutoSize = true;
            this.chkWriteHr.Checked = true;
            this.chkWriteHr.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkWriteHr.Location = new System.Drawing.Point(13, 99);
            this.chkWriteHr.Name = "chkWriteHr";
            this.chkWriteHr.Size = new System.Drawing.Size(103, 17);
            this.chkWriteHr.TabIndex = 4;
            this.chkWriteHr.Text = "Запись данных";
            this.chkWriteHr.UseVisualStyleBackColor = true;
            this.chkWriteHr.CheckedChanged += new System.EventHandler(this.control_Changed);
            // 
            // numStoreHrPer
            // 
            this.numStoreHrPer.Location = new System.Drawing.Point(13, 72);
            this.numStoreHrPer.Maximum = new decimal(new int[] {
            3650,
            0,
            0,
            0});
            this.numStoreHrPer.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numStoreHrPer.Name = "numStoreHrPer";
            this.numStoreHrPer.Size = new System.Drawing.Size(178, 20);
            this.numStoreHrPer.TabIndex = 3;
            this.numStoreHrPer.Value = new decimal(new int[] {
            365,
            0,
            0,
            0});
            this.numStoreHrPer.ValueChanged += new System.EventHandler(this.control_Changed);
            // 
            // lblStoreHrPer
            // 
            this.lblStoreHrPer.AutoSize = true;
            this.lblStoreHrPer.Location = new System.Drawing.Point(10, 56);
            this.lblStoreHrPer.Name = "lblStoreHrPer";
            this.lblStoreHrPer.Size = new System.Drawing.Size(125, 13);
            this.lblStoreHrPer.TabIndex = 2;
            this.lblStoreHrPer.Text = "Период хранения, дней";
            // 
            // lblWriteHrPer
            // 
            this.lblWriteHrPer.AutoSize = true;
            this.lblWriteHrPer.Location = new System.Drawing.Point(10, 16);
            this.lblWriteHrPer.Name = "lblWriteHrPer";
            this.lblWriteHrPer.Size = new System.Drawing.Size(84, 13);
            this.lblWriteHrPer.TabIndex = 0;
            this.lblWriteHrPer.Text = "Период записи";
            // 
            // cbWriteHrPer
            // 
            this.cbWriteHrPer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWriteHrPer.FormattingEnabled = true;
            this.cbWriteHrPer.Items.AddRange(new object[] {
            "30 минут",
            "1 час"});
            this.cbWriteHrPer.Location = new System.Drawing.Point(13, 32);
            this.cbWriteHrPer.Name = "cbWriteHrPer";
            this.cbWriteHrPer.Size = new System.Drawing.Size(178, 21);
            this.cbWriteHrPer.TabIndex = 1;
            this.cbWriteHrPer.SelectedIndexChanged += new System.EventHandler(this.control_Changed);
            // 
            // gbSaveParamsMin
            // 
            this.gbSaveParamsMin.Controls.Add(this.chkWriteMinCopy);
            this.gbSaveParamsMin.Controls.Add(this.chkWriteMin);
            this.gbSaveParamsMin.Controls.Add(this.numStoreMinPer);
            this.gbSaveParamsMin.Controls.Add(this.lblStoreMinPer);
            this.gbSaveParamsMin.Controls.Add(this.lblWriteMinPer);
            this.gbSaveParamsMin.Controls.Add(this.cbWriteMinPer);
            this.gbSaveParamsMin.Location = new System.Drawing.Point(216, 6);
            this.gbSaveParamsMin.Name = "gbSaveParamsMin";
            this.gbSaveParamsMin.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbSaveParamsMin.Size = new System.Drawing.Size(204, 152);
            this.gbSaveParamsMin.TabIndex = 1;
            this.gbSaveParamsMin.TabStop = false;
            this.gbSaveParamsMin.Text = "Минутные срезы";
            // 
            // chkWriteMinCopy
            // 
            this.chkWriteMinCopy.AutoSize = true;
            this.chkWriteMinCopy.Checked = true;
            this.chkWriteMinCopy.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkWriteMinCopy.Location = new System.Drawing.Point(13, 122);
            this.chkWriteMinCopy.Name = "chkWriteMinCopy";
            this.chkWriteMinCopy.Size = new System.Drawing.Size(136, 17);
            this.chkWriteMinCopy.TabIndex = 5;
            this.chkWriteMinCopy.Text = "Запись копии данных";
            this.chkWriteMinCopy.UseVisualStyleBackColor = true;
            this.chkWriteMinCopy.CheckedChanged += new System.EventHandler(this.control_Changed);
            // 
            // chkWriteMin
            // 
            this.chkWriteMin.AutoSize = true;
            this.chkWriteMin.Checked = true;
            this.chkWriteMin.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkWriteMin.Location = new System.Drawing.Point(13, 99);
            this.chkWriteMin.Name = "chkWriteMin";
            this.chkWriteMin.Size = new System.Drawing.Size(103, 17);
            this.chkWriteMin.TabIndex = 4;
            this.chkWriteMin.Text = "Запись данных";
            this.chkWriteMin.UseVisualStyleBackColor = true;
            this.chkWriteMin.CheckedChanged += new System.EventHandler(this.control_Changed);
            // 
            // numStoreMinPer
            // 
            this.numStoreMinPer.Location = new System.Drawing.Point(13, 72);
            this.numStoreMinPer.Maximum = new decimal(new int[] {
            3650,
            0,
            0,
            0});
            this.numStoreMinPer.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numStoreMinPer.Name = "numStoreMinPer";
            this.numStoreMinPer.Size = new System.Drawing.Size(178, 20);
            this.numStoreMinPer.TabIndex = 3;
            this.numStoreMinPer.Value = new decimal(new int[] {
            365,
            0,
            0,
            0});
            this.numStoreMinPer.ValueChanged += new System.EventHandler(this.control_Changed);
            // 
            // lblStoreMinPer
            // 
            this.lblStoreMinPer.AutoSize = true;
            this.lblStoreMinPer.Location = new System.Drawing.Point(10, 56);
            this.lblStoreMinPer.Name = "lblStoreMinPer";
            this.lblStoreMinPer.Size = new System.Drawing.Size(125, 13);
            this.lblStoreMinPer.TabIndex = 2;
            this.lblStoreMinPer.Text = "Период хранения, дней";
            // 
            // lblWriteMinPer
            // 
            this.lblWriteMinPer.AutoSize = true;
            this.lblWriteMinPer.Location = new System.Drawing.Point(10, 16);
            this.lblWriteMinPer.Name = "lblWriteMinPer";
            this.lblWriteMinPer.Size = new System.Drawing.Size(84, 13);
            this.lblWriteMinPer.TabIndex = 0;
            this.lblWriteMinPer.Text = "Период записи";
            // 
            // cbWriteMinPer
            // 
            this.cbWriteMinPer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWriteMinPer.FormattingEnabled = true;
            this.cbWriteMinPer.Items.AddRange(new object[] {
            "30 секунд",
            "1 минута",
            "2 минуты",
            "3 минуты",
            "4 минуты",
            "5 минут",
            "10 минут"});
            this.cbWriteMinPer.Location = new System.Drawing.Point(13, 32);
            this.cbWriteMinPer.Name = "cbWriteMinPer";
            this.cbWriteMinPer.Size = new System.Drawing.Size(178, 21);
            this.cbWriteMinPer.TabIndex = 1;
            this.cbWriteMinPer.SelectedIndexChanged += new System.EventHandler(this.control_Changed);
            // 
            // gbSaveParamsCur
            // 
            this.gbSaveParamsCur.Controls.Add(this.cbInactUnrelTime);
            this.gbSaveParamsCur.Controls.Add(this.chkWriteCurCopy);
            this.gbSaveParamsCur.Controls.Add(this.chkWriteCur);
            this.gbSaveParamsCur.Controls.Add(this.lblInactUnrelTime);
            this.gbSaveParamsCur.Controls.Add(this.lblWriteCurPer);
            this.gbSaveParamsCur.Controls.Add(this.cbWriteCurPer);
            this.gbSaveParamsCur.Location = new System.Drawing.Point(6, 6);
            this.gbSaveParamsCur.Name = "gbSaveParamsCur";
            this.gbSaveParamsCur.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbSaveParamsCur.Size = new System.Drawing.Size(204, 152);
            this.gbSaveParamsCur.TabIndex = 0;
            this.gbSaveParamsCur.TabStop = false;
            this.gbSaveParamsCur.Text = "Текущий срез";
            // 
            // cbInactUnrelTime
            // 
            this.cbInactUnrelTime.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbInactUnrelTime.FormattingEnabled = true;
            this.cbInactUnrelTime.Items.AddRange(new object[] {
            "Нет",
            "1 минута",
            "2 минуты",
            "3 минуты",
            "4 минуты",
            "5 минут",
            "10 минут",
            "20 минут",
            "30 минут",
            "1 час"});
            this.cbInactUnrelTime.Location = new System.Drawing.Point(13, 72);
            this.cbInactUnrelTime.Name = "cbInactUnrelTime";
            this.cbInactUnrelTime.Size = new System.Drawing.Size(178, 21);
            this.cbInactUnrelTime.TabIndex = 3;
            this.cbInactUnrelTime.SelectedIndexChanged += new System.EventHandler(this.control_Changed);
            // 
            // chkWriteCurCopy
            // 
            this.chkWriteCurCopy.AutoSize = true;
            this.chkWriteCurCopy.Checked = true;
            this.chkWriteCurCopy.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkWriteCurCopy.Location = new System.Drawing.Point(13, 122);
            this.chkWriteCurCopy.Name = "chkWriteCurCopy";
            this.chkWriteCurCopy.Size = new System.Drawing.Size(136, 17);
            this.chkWriteCurCopy.TabIndex = 5;
            this.chkWriteCurCopy.Text = "Запись копии данных";
            this.chkWriteCurCopy.UseVisualStyleBackColor = true;
            this.chkWriteCurCopy.CheckedChanged += new System.EventHandler(this.control_Changed);
            // 
            // chkWriteCur
            // 
            this.chkWriteCur.AutoSize = true;
            this.chkWriteCur.Checked = true;
            this.chkWriteCur.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkWriteCur.Location = new System.Drawing.Point(13, 99);
            this.chkWriteCur.Name = "chkWriteCur";
            this.chkWriteCur.Size = new System.Drawing.Size(103, 17);
            this.chkWriteCur.TabIndex = 4;
            this.chkWriteCur.Text = "Запись данных";
            this.chkWriteCur.UseVisualStyleBackColor = true;
            this.chkWriteCur.CheckedChanged += new System.EventHandler(this.control_Changed);
            // 
            // lblInactUnrelTime
            // 
            this.lblInactUnrelTime.AutoSize = true;
            this.lblInactUnrelTime.Location = new System.Drawing.Point(10, 56);
            this.lblInactUnrelTime.Name = "lblInactUnrelTime";
            this.lblInactUnrelTime.Size = new System.Drawing.Size(153, 13);
            this.lblInactUnrelTime.TabIndex = 2;
            this.lblInactUnrelTime.Text = "Недостов. при неактивности";
            // 
            // lblWriteCurPer
            // 
            this.lblWriteCurPer.AutoSize = true;
            this.lblWriteCurPer.Location = new System.Drawing.Point(10, 16);
            this.lblWriteCurPer.Name = "lblWriteCurPer";
            this.lblWriteCurPer.Size = new System.Drawing.Size(84, 13);
            this.lblWriteCurPer.TabIndex = 0;
            this.lblWriteCurPer.Text = "Период записи";
            // 
            // cbWriteCurPer
            // 
            this.cbWriteCurPer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWriteCurPer.FormattingEnabled = true;
            this.cbWriteCurPer.Items.AddRange(new object[] {
            "По изменению",
            "1 секунда",
            "2 секунды",
            "3 секунды",
            "4 секунды",
            "5 секунд",
            "10 секунд",
            "20 секунд",
            "30 секунд",
            "1 минута"});
            this.cbWriteCurPer.Location = new System.Drawing.Point(13, 32);
            this.cbWriteCurPer.Name = "cbWriteCurPer";
            this.cbWriteCurPer.Size = new System.Drawing.Size(178, 21);
            this.cbWriteCurPer.TabIndex = 1;
            this.cbWriteCurPer.SelectedIndexChanged += new System.EventHandler(this.control_Changed);
            // 
            // pageFiles
            // 
            this.pageFiles.Controls.Add(this.txtDataDir);
            this.pageFiles.Controls.Add(this.rbFilesCopy);
            this.pageFiles.Controls.Add(this.rbFilesMain);
            this.pageFiles.Controls.Add(this.btnEditFile);
            this.pageFiles.Controls.Add(this.btnViewFile);
            this.pageFiles.Controls.Add(this.lbFiles);
            this.pageFiles.Location = new System.Drawing.Point(4, 22);
            this.pageFiles.Name = "pageFiles";
            this.pageFiles.Padding = new System.Windows.Forms.Padding(3);
            this.pageFiles.Size = new System.Drawing.Size(426, 378);
            this.pageFiles.TabIndex = 4;
            this.pageFiles.Text = "Файлы";
            this.pageFiles.UseVisualStyleBackColor = true;
            // 
            // txtDataDir
            // 
            this.txtDataDir.Location = new System.Drawing.Point(6, 29);
            this.txtDataDir.Name = "txtDataDir";
            this.txtDataDir.ReadOnly = true;
            this.txtDataDir.Size = new System.Drawing.Size(414, 20);
            this.txtDataDir.TabIndex = 2;
            // 
            // rbFilesCopy
            // 
            this.rbFilesCopy.AutoSize = true;
            this.rbFilesCopy.Location = new System.Drawing.Point(130, 6);
            this.rbFilesCopy.Name = "rbFilesCopy";
            this.rbFilesCopy.Size = new System.Drawing.Size(96, 17);
            this.rbFilesCopy.TabIndex = 1;
            this.rbFilesCopy.Text = "Копии данных";
            this.rbFilesCopy.UseVisualStyleBackColor = true;
            // 
            // rbFilesMain
            // 
            this.rbFilesMain.AutoSize = true;
            this.rbFilesMain.Checked = true;
            this.rbFilesMain.Location = new System.Drawing.Point(6, 6);
            this.rbFilesMain.Name = "rbFilesMain";
            this.rbFilesMain.Size = new System.Drawing.Size(118, 17);
            this.rbFilesMain.TabIndex = 0;
            this.rbFilesMain.TabStop = true;
            this.rbFilesMain.Text = "Основные данные";
            this.rbFilesMain.UseVisualStyleBackColor = true;
            this.rbFilesMain.CheckedChanged += new System.EventHandler(this.rbFiles_CheckedChanged);
            // 
            // btnEditFile
            // 
            this.btnEditFile.Location = new System.Drawing.Point(132, 349);
            this.btnEditFile.Name = "btnEditFile";
            this.btnEditFile.Size = new System.Drawing.Size(120, 23);
            this.btnEditFile.TabIndex = 5;
            this.btnEditFile.Text = "Редактирование";
            this.btnEditFile.UseVisualStyleBackColor = true;
            this.btnEditFile.Click += new System.EventHandler(this.btnEditFile_Click);
            // 
            // btnViewFile
            // 
            this.btnViewFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnViewFile.Location = new System.Drawing.Point(6, 349);
            this.btnViewFile.Name = "btnViewFile";
            this.btnViewFile.Size = new System.Drawing.Size(120, 23);
            this.btnViewFile.TabIndex = 4;
            this.btnViewFile.Text = "Просмотр";
            this.btnViewFile.UseVisualStyleBackColor = true;
            this.btnViewFile.Click += new System.EventHandler(this.btnViewFile_Click);
            // 
            // lbFiles
            // 
            this.lbFiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbFiles.FormattingEnabled = true;
            this.lbFiles.HorizontalScrollbar = true;
            this.lbFiles.IntegralHeight = false;
            this.lbFiles.Location = new System.Drawing.Point(6, 55);
            this.lbFiles.MultiColumn = true;
            this.lbFiles.Name = "lbFiles";
            this.lbFiles.Size = new System.Drawing.Size(414, 288);
            this.lbFiles.TabIndex = 3;
            this.lbFiles.DoubleClick += new System.EventHandler(this.lbFiles_DoubleClick);
            this.lbFiles.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lbFiles_KeyDown);
            // 
            // pageModules
            // 
            this.pageModules.Controls.Add(this.btnDelMod);
            this.pageModules.Controls.Add(this.btnMoveDownMod);
            this.pageModules.Controls.Add(this.btnMoveUpMod);
            this.pageModules.Controls.Add(this.btnAddMod);
            this.pageModules.Controls.Add(this.btnModCommSettings);
            this.pageModules.Controls.Add(this.btnModProps);
            this.pageModules.Controls.Add(this.txtModDescr);
            this.pageModules.Controls.Add(this.lblModDescr);
            this.pageModules.Controls.Add(this.lbModDll);
            this.pageModules.Location = new System.Drawing.Point(4, 22);
            this.pageModules.Name = "pageModules";
            this.pageModules.Padding = new System.Windows.Forms.Padding(3);
            this.pageModules.Size = new System.Drawing.Size(426, 378);
            this.pageModules.TabIndex = 5;
            this.pageModules.Text = "Модули";
            this.pageModules.UseVisualStyleBackColor = true;
            // 
            // btnDelMod
            // 
            this.btnDelMod.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnDelMod.Image = ((System.Drawing.Image)(resources.GetObject("btnDelMod.Image")));
            this.btnDelMod.Location = new System.Drawing.Point(93, 6);
            this.btnDelMod.Name = "btnDelMod";
            this.btnDelMod.Size = new System.Drawing.Size(23, 22);
            this.btnDelMod.TabIndex = 3;
            this.toolTip.SetToolTip(this.btnDelMod, "Удалить выбранный модуль");
            this.btnDelMod.UseVisualStyleBackColor = true;
            this.btnDelMod.Click += new System.EventHandler(this.btnDelMod_Click);
            // 
            // btnMoveDownMod
            // 
            this.btnMoveDownMod.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnMoveDownMod.Image = ((System.Drawing.Image)(resources.GetObject("btnMoveDownMod.Image")));
            this.btnMoveDownMod.Location = new System.Drawing.Point(64, 6);
            this.btnMoveDownMod.Name = "btnMoveDownMod";
            this.btnMoveDownMod.Size = new System.Drawing.Size(23, 22);
            this.btnMoveDownMod.TabIndex = 2;
            this.toolTip.SetToolTip(this.btnMoveDownMod, "Переместить выбранный модуль вниз");
            this.btnMoveDownMod.UseVisualStyleBackColor = true;
            this.btnMoveDownMod.Click += new System.EventHandler(this.btnMoveDownMod_Click);
            // 
            // btnMoveUpMod
            // 
            this.btnMoveUpMod.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnMoveUpMod.Image = ((System.Drawing.Image)(resources.GetObject("btnMoveUpMod.Image")));
            this.btnMoveUpMod.Location = new System.Drawing.Point(35, 6);
            this.btnMoveUpMod.Name = "btnMoveUpMod";
            this.btnMoveUpMod.Size = new System.Drawing.Size(23, 22);
            this.btnMoveUpMod.TabIndex = 1;
            this.toolTip.SetToolTip(this.btnMoveUpMod, "Переместить выбранный модуль вверх");
            this.btnMoveUpMod.UseVisualStyleBackColor = true;
            this.btnMoveUpMod.Click += new System.EventHandler(this.btnMoveUpMod_Click);
            // 
            // btnAddMod
            // 
            this.btnAddMod.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAddMod.Image = ((System.Drawing.Image)(resources.GetObject("btnAddMod.Image")));
            this.btnAddMod.Location = new System.Drawing.Point(6, 6);
            this.btnAddMod.Name = "btnAddMod";
            this.btnAddMod.Size = new System.Drawing.Size(23, 22);
            this.btnAddMod.TabIndex = 0;
            this.toolTip.SetToolTip(this.btnAddMod, "Добавить модуль");
            this.btnAddMod.UseVisualStyleBackColor = true;
            this.btnAddMod.Click += new System.EventHandler(this.btnAddMod_Click);
            // 
            // btnModCommSettings
            // 
            this.btnModCommSettings.Location = new System.Drawing.Point(102, 349);
            this.btnModCommSettings.Name = "btnModCommSettings";
            this.btnModCommSettings.Size = new System.Drawing.Size(140, 23);
            this.btnModCommSettings.TabIndex = 8;
            this.btnModCommSettings.Text = "Настроить соединение";
            this.btnModCommSettings.UseVisualStyleBackColor = true;
            this.btnModCommSettings.Click += new System.EventHandler(this.btnCommSettings_Click);
            // 
            // btnModProps
            // 
            this.btnModProps.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnModProps.Enabled = false;
            this.btnModProps.Location = new System.Drawing.Point(6, 349);
            this.btnModProps.Name = "btnModProps";
            this.btnModProps.Size = new System.Drawing.Size(90, 23);
            this.btnModProps.TabIndex = 7;
            this.btnModProps.Text = "Свойства";
            this.btnModProps.UseVisualStyleBackColor = true;
            this.btnModProps.Click += new System.EventHandler(this.btnModProps_Click);
            // 
            // txtModDescr
            // 
            this.txtModDescr.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtModDescr.Location = new System.Drawing.Point(6, 226);
            this.txtModDescr.Multiline = true;
            this.txtModDescr.Name = "txtModDescr";
            this.txtModDescr.ReadOnly = true;
            this.txtModDescr.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtModDescr.Size = new System.Drawing.Size(412, 117);
            this.txtModDescr.TabIndex = 6;
            // 
            // lblModDescr
            // 
            this.lblModDescr.AutoSize = true;
            this.lblModDescr.Location = new System.Drawing.Point(3, 210);
            this.lblModDescr.Name = "lblModDescr";
            this.lblModDescr.Size = new System.Drawing.Size(57, 13);
            this.lblModDescr.TabIndex = 5;
            this.lblModDescr.Text = "Описание";
            // 
            // lbModDll
            // 
            this.lbModDll.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbModDll.FormattingEnabled = true;
            this.lbModDll.HorizontalScrollbar = true;
            this.lbModDll.Location = new System.Drawing.Point(6, 34);
            this.lbModDll.MultiColumn = true;
            this.lbModDll.Name = "lbModDll";
            this.lbModDll.Size = new System.Drawing.Size(412, 173);
            this.lbModDll.TabIndex = 4;
            this.lbModDll.SelectedIndexChanged += new System.EventHandler(this.lbModDll_SelectedIndexChanged);
            this.lbModDll.DoubleClick += new System.EventHandler(this.lbModDll_DoubleClick);
            // 
            // pageGenSrez
            // 
            this.pageGenSrez.Controls.Add(this.gbGenCommSettings1);
            this.pageGenSrez.Controls.Add(this.gbGenPwd1);
            this.pageGenSrez.Controls.Add(this.gbGenSrez);
            this.pageGenSrez.Location = new System.Drawing.Point(4, 22);
            this.pageGenSrez.Name = "pageGenSrez";
            this.pageGenSrez.Padding = new System.Windows.Forms.Padding(3);
            this.pageGenSrez.Size = new System.Drawing.Size(426, 378);
            this.pageGenSrez.TabIndex = 6;
            this.pageGenSrez.Text = "Данные";
            this.pageGenSrez.UseVisualStyleBackColor = true;
            // 
            // gbGenCommSettings1
            // 
            this.gbGenCommSettings1.Controls.Add(this.btnGenCommSettings1);
            this.gbGenCommSettings1.Location = new System.Drawing.Point(138, 6);
            this.gbGenCommSettings1.Name = "gbGenCommSettings1";
            this.gbGenCommSettings1.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbGenCommSettings1.Size = new System.Drawing.Size(282, 55);
            this.gbGenCommSettings1.TabIndex = 1;
            this.gbGenCommSettings1.TabStop = false;
            this.gbGenCommSettings1.Text = "Соединение";
            // 
            // btnGenCommSettings1
            // 
            this.btnGenCommSettings1.Location = new System.Drawing.Point(13, 19);
            this.btnGenCommSettings1.Name = "btnGenCommSettings1";
            this.btnGenCommSettings1.Size = new System.Drawing.Size(140, 23);
            this.btnGenCommSettings1.TabIndex = 0;
            this.btnGenCommSettings1.Text = "Настроить соединение";
            this.btnGenCommSettings1.UseVisualStyleBackColor = true;
            this.btnGenCommSettings1.Click += new System.EventHandler(this.btnCommSettings_Click);
            // 
            // gbGenPwd1
            // 
            this.gbGenPwd1.Controls.Add(this.txtGenPwd1);
            this.gbGenPwd1.Location = new System.Drawing.Point(6, 6);
            this.gbGenPwd1.Name = "gbGenPwd1";
            this.gbGenPwd1.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbGenPwd1.Size = new System.Drawing.Size(126, 55);
            this.gbGenPwd1.TabIndex = 0;
            this.gbGenPwd1.TabStop = false;
            this.gbGenPwd1.Text = "Пароль";
            // 
            // txtGenPwd1
            // 
            this.txtGenPwd1.Location = new System.Drawing.Point(13, 20);
            this.txtGenPwd1.Name = "txtGenPwd1";
            this.txtGenPwd1.Size = new System.Drawing.Size(100, 20);
            this.txtGenPwd1.TabIndex = 0;
            this.txtGenPwd1.UseSystemPasswordChar = true;
            this.txtGenPwd1.TextChanged += new System.EventHandler(this.txtGenPwd_TextChanged);
            // 
            // gbGenSrez
            // 
            this.gbGenSrez.Controls.Add(this.btnSendSrez);
            this.gbGenSrez.Controls.Add(this.numSrezCnlStat);
            this.gbGenSrez.Controls.Add(this.btnSrezCnlStatOne);
            this.gbGenSrez.Controls.Add(this.btnSrezCnlStatZero);
            this.gbGenSrez.Controls.Add(this.lblSrezCnlStat);
            this.gbGenSrez.Controls.Add(this.btnSrezValPlusOne);
            this.gbGenSrez.Controls.Add(this.btnSrezValMinusOne);
            this.gbGenSrez.Controls.Add(this.lblSrezCnlVal);
            this.gbGenSrez.Controls.Add(this.txtSrezCnlVal);
            this.gbGenSrez.Controls.Add(this.cbSrezCnlNum);
            this.gbGenSrez.Controls.Add(this.lblSrezCnlNum);
            this.gbGenSrez.Controls.Add(this.dtpSrezTime);
            this.gbGenSrez.Controls.Add(this.dtpSrezDate);
            this.gbGenSrez.Controls.Add(this.rbArcSrez);
            this.gbGenSrez.Controls.Add(this.rbCurSrez);
            this.gbGenSrez.Location = new System.Drawing.Point(6, 67);
            this.gbGenSrez.Name = "gbGenSrez";
            this.gbGenSrez.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbGenSrez.Size = new System.Drawing.Size(414, 144);
            this.gbGenSrez.TabIndex = 2;
            this.gbGenSrez.TabStop = false;
            this.gbGenSrez.Text = "Генерация данных";
            this.gbGenSrez.Visible = false;
            // 
            // btnSendSrez
            // 
            this.btnSendSrez.Location = new System.Drawing.Point(13, 108);
            this.btnSendSrez.Name = "btnSendSrez";
            this.btnSendSrez.Size = new System.Drawing.Size(82, 23);
            this.btnSendSrez.TabIndex = 14;
            this.btnSendSrez.Text = "Отправить";
            this.btnSendSrez.UseVisualStyleBackColor = true;
            this.btnSendSrez.Click += new System.EventHandler(this.btnSendSrez_Click);
            // 
            // numSrezCnlStat
            // 
            this.numSrezCnlStat.Location = new System.Drawing.Point(254, 81);
            this.numSrezCnlStat.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numSrezCnlStat.Name = "numSrezCnlStat";
            this.numSrezCnlStat.Size = new System.Drawing.Size(75, 20);
            this.numSrezCnlStat.TabIndex = 11;
            // 
            // btnSrezCnlStatOne
            // 
            this.btnSrezCnlStatOne.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSrezCnlStatOne.Location = new System.Drawing.Point(371, 81);
            this.btnSrezCnlStatOne.Name = "btnSrezCnlStatOne";
            this.btnSrezCnlStatOne.Size = new System.Drawing.Size(30, 20);
            this.btnSrezCnlStatOne.TabIndex = 13;
            this.btnSrezCnlStatOne.Text = "1";
            this.btnSrezCnlStatOne.UseVisualStyleBackColor = true;
            this.btnSrezCnlStatOne.Click += new System.EventHandler(this.btnSrezCnlStat_Click);
            // 
            // btnSrezCnlStatZero
            // 
            this.btnSrezCnlStatZero.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSrezCnlStatZero.Location = new System.Drawing.Point(335, 81);
            this.btnSrezCnlStatZero.Name = "btnSrezCnlStatZero";
            this.btnSrezCnlStatZero.Size = new System.Drawing.Size(30, 20);
            this.btnSrezCnlStatZero.TabIndex = 12;
            this.btnSrezCnlStatZero.Text = "0";
            this.btnSrezCnlStatZero.UseVisualStyleBackColor = true;
            this.btnSrezCnlStatZero.Click += new System.EventHandler(this.btnSrezCnlStat_Click);
            // 
            // lblSrezCnlStat
            // 
            this.lblSrezCnlStat.AutoSize = true;
            this.lblSrezCnlStat.Location = new System.Drawing.Point(251, 65);
            this.lblSrezCnlStat.Name = "lblSrezCnlStat";
            this.lblSrezCnlStat.Size = new System.Drawing.Size(41, 13);
            this.lblSrezCnlStat.TabIndex = 10;
            this.lblSrezCnlStat.Text = "Статус";
            // 
            // btnSrezValPlusOne
            // 
            this.btnSrezValPlusOne.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSrezValPlusOne.Location = new System.Drawing.Point(218, 81);
            this.btnSrezValPlusOne.Name = "btnSrezValPlusOne";
            this.btnSrezValPlusOne.Size = new System.Drawing.Size(30, 20);
            this.btnSrezValPlusOne.TabIndex = 9;
            this.btnSrezValPlusOne.Text = "+1";
            this.btnSrezValPlusOne.UseVisualStyleBackColor = true;
            this.btnSrezValPlusOne.Click += new System.EventHandler(this.btnSrezVal_Click);
            // 
            // btnSrezValMinusOne
            // 
            this.btnSrezValMinusOne.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSrezValMinusOne.Location = new System.Drawing.Point(182, 81);
            this.btnSrezValMinusOne.Name = "btnSrezValMinusOne";
            this.btnSrezValMinusOne.Size = new System.Drawing.Size(30, 20);
            this.btnSrezValMinusOne.TabIndex = 8;
            this.btnSrezValMinusOne.Text = "-1";
            this.btnSrezValMinusOne.UseVisualStyleBackColor = true;
            this.btnSrezValMinusOne.Click += new System.EventHandler(this.btnSrezVal_Click);
            // 
            // lblSrezCnlVal
            // 
            this.lblSrezCnlVal.AutoSize = true;
            this.lblSrezCnlVal.Location = new System.Drawing.Point(98, 65);
            this.lblSrezCnlVal.Name = "lblSrezCnlVal";
            this.lblSrezCnlVal.Size = new System.Drawing.Size(55, 13);
            this.lblSrezCnlVal.TabIndex = 6;
            this.lblSrezCnlVal.Text = "Значение";
            // 
            // txtSrezCnlVal
            // 
            this.txtSrezCnlVal.Location = new System.Drawing.Point(101, 81);
            this.txtSrezCnlVal.Name = "txtSrezCnlVal";
            this.txtSrezCnlVal.Size = new System.Drawing.Size(75, 20);
            this.txtSrezCnlVal.TabIndex = 7;
            this.txtSrezCnlVal.Text = "0";
            // 
            // cbSrezCnlNum
            // 
            this.cbSrezCnlNum.FormattingEnabled = true;
            this.cbSrezCnlNum.Location = new System.Drawing.Point(13, 81);
            this.cbSrezCnlNum.Name = "cbSrezCnlNum";
            this.cbSrezCnlNum.Size = new System.Drawing.Size(82, 21);
            this.cbSrezCnlNum.TabIndex = 5;
            // 
            // lblSrezCnlNum
            // 
            this.lblSrezCnlNum.AutoSize = true;
            this.lblSrezCnlNum.Location = new System.Drawing.Point(10, 65);
            this.lblSrezCnlNum.Name = "lblSrezCnlNum";
            this.lblSrezCnlNum.Size = new System.Drawing.Size(38, 13);
            this.lblSrezCnlNum.TabIndex = 4;
            this.lblSrezCnlNum.Text = "Канал";
            // 
            // dtpSrezTime
            // 
            this.dtpSrezTime.CustomFormat = "";
            this.dtpSrezTime.Enabled = false;
            this.dtpSrezTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpSrezTime.Location = new System.Drawing.Point(229, 42);
            this.dtpSrezTime.Name = "dtpSrezTime";
            this.dtpSrezTime.ShowUpDown = true;
            this.dtpSrezTime.Size = new System.Drawing.Size(100, 20);
            this.dtpSrezTime.TabIndex = 3;
            // 
            // dtpSrezDate
            // 
            this.dtpSrezDate.CustomFormat = "";
            this.dtpSrezDate.Enabled = false;
            this.dtpSrezDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpSrezDate.Location = new System.Drawing.Point(123, 42);
            this.dtpSrezDate.Name = "dtpSrezDate";
            this.dtpSrezDate.Size = new System.Drawing.Size(100, 20);
            this.dtpSrezDate.TabIndex = 2;
            // 
            // rbArcSrez
            // 
            this.rbArcSrez.AutoSize = true;
            this.rbArcSrez.Location = new System.Drawing.Point(13, 44);
            this.rbArcSrez.Name = "rbArcSrez";
            this.rbArcSrez.Size = new System.Drawing.Size(102, 17);
            this.rbArcSrez.TabIndex = 1;
            this.rbArcSrez.TabStop = true;
            this.rbArcSrez.Text = "Архивный срез";
            this.rbArcSrez.UseVisualStyleBackColor = true;
            this.rbArcSrez.CheckedChanged += new System.EventHandler(this.rbArcSrez_CheckedChanged);
            // 
            // rbCurSrez
            // 
            this.rbCurSrez.AutoSize = true;
            this.rbCurSrez.Checked = true;
            this.rbCurSrez.Location = new System.Drawing.Point(13, 21);
            this.rbCurSrez.Name = "rbCurSrez";
            this.rbCurSrez.Size = new System.Drawing.Size(97, 17);
            this.rbCurSrez.TabIndex = 0;
            this.rbCurSrez.TabStop = true;
            this.rbCurSrez.Text = "Текущий срез";
            this.rbCurSrez.UseVisualStyleBackColor = true;
            // 
            // pageGenEv
            // 
            this.pageGenEv.Controls.Add(this.gbGenCommSettings2);
            this.pageGenEv.Controls.Add(this.gbGenPwd2);
            this.pageGenEv.Controls.Add(this.gbCheckEv);
            this.pageGenEv.Controls.Add(this.gbGenEv);
            this.pageGenEv.Location = new System.Drawing.Point(4, 22);
            this.pageGenEv.Name = "pageGenEv";
            this.pageGenEv.Padding = new System.Windows.Forms.Padding(3);
            this.pageGenEv.Size = new System.Drawing.Size(426, 378);
            this.pageGenEv.TabIndex = 10;
            this.pageGenEv.Text = "События";
            this.pageGenEv.UseVisualStyleBackColor = true;
            // 
            // gbGenCommSettings2
            // 
            this.gbGenCommSettings2.Controls.Add(this.btnGenCommSettings2);
            this.gbGenCommSettings2.Location = new System.Drawing.Point(138, 6);
            this.gbGenCommSettings2.Name = "gbGenCommSettings2";
            this.gbGenCommSettings2.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbGenCommSettings2.Size = new System.Drawing.Size(282, 55);
            this.gbGenCommSettings2.TabIndex = 1;
            this.gbGenCommSettings2.TabStop = false;
            this.gbGenCommSettings2.Text = "Соединение";
            // 
            // btnGenCommSettings2
            // 
            this.btnGenCommSettings2.Location = new System.Drawing.Point(13, 19);
            this.btnGenCommSettings2.Name = "btnGenCommSettings2";
            this.btnGenCommSettings2.Size = new System.Drawing.Size(140, 23);
            this.btnGenCommSettings2.TabIndex = 0;
            this.btnGenCommSettings2.Text = "Настроить соединение";
            this.btnGenCommSettings2.UseVisualStyleBackColor = true;
            this.btnGenCommSettings2.Click += new System.EventHandler(this.btnCommSettings_Click);
            // 
            // gbGenPwd2
            // 
            this.gbGenPwd2.Controls.Add(this.txtGenPwd2);
            this.gbGenPwd2.Location = new System.Drawing.Point(6, 6);
            this.gbGenPwd2.Name = "gbGenPwd2";
            this.gbGenPwd2.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbGenPwd2.Size = new System.Drawing.Size(126, 55);
            this.gbGenPwd2.TabIndex = 0;
            this.gbGenPwd2.TabStop = false;
            this.gbGenPwd2.Text = "Пароль";
            // 
            // txtGenPwd2
            // 
            this.txtGenPwd2.Location = new System.Drawing.Point(13, 20);
            this.txtGenPwd2.Name = "txtGenPwd2";
            this.txtGenPwd2.Size = new System.Drawing.Size(100, 20);
            this.txtGenPwd2.TabIndex = 0;
            this.txtGenPwd2.UseSystemPasswordChar = true;
            this.txtGenPwd2.TextChanged += new System.EventHandler(this.txtGenPwd_TextChanged);
            // 
            // gbCheckEv
            // 
            this.gbCheckEv.Controls.Add(this.btnCheckEvent);
            this.gbCheckEv.Controls.Add(this.lblEvUserID2);
            this.gbCheckEv.Controls.Add(this.numEvUserID2);
            this.gbCheckEv.Controls.Add(this.lblEvDate2);
            this.gbCheckEv.Controls.Add(this.dtpEvDate2);
            this.gbCheckEv.Controls.Add(this.numEvNum);
            this.gbCheckEv.Controls.Add(this.lblEvNum);
            this.gbCheckEv.Location = new System.Drawing.Point(6, 257);
            this.gbCheckEv.Name = "gbCheckEv";
            this.gbCheckEv.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbCheckEv.Size = new System.Drawing.Size(414, 67);
            this.gbCheckEv.TabIndex = 3;
            this.gbCheckEv.TabStop = false;
            this.gbCheckEv.Text = "Квитирование события";
            this.gbCheckEv.Visible = false;
            // 
            // btnCheckEvent
            // 
            this.btnCheckEvent.Location = new System.Drawing.Point(311, 31);
            this.btnCheckEvent.Name = "btnCheckEvent";
            this.btnCheckEvent.Size = new System.Drawing.Size(90, 23);
            this.btnCheckEvent.TabIndex = 6;
            this.btnCheckEvent.Text = "Отправить";
            this.btnCheckEvent.UseVisualStyleBackColor = true;
            this.btnCheckEvent.Click += new System.EventHandler(this.btnCheckEvent_Click);
            // 
            // lblEvUserID2
            // 
            this.lblEvUserID2.AutoSize = true;
            this.lblEvUserID2.Location = new System.Drawing.Point(212, 16);
            this.lblEvUserID2.Name = "lblEvUserID2";
            this.lblEvUserID2.Size = new System.Drawing.Size(80, 13);
            this.lblEvUserID2.TabIndex = 4;
            this.lblEvUserID2.Text = "Пользователь";
            // 
            // numEvUserID2
            // 
            this.numEvUserID2.Location = new System.Drawing.Point(215, 32);
            this.numEvUserID2.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numEvUserID2.Name = "numEvUserID2";
            this.numEvUserID2.Size = new System.Drawing.Size(90, 20);
            this.numEvUserID2.TabIndex = 5;
            // 
            // lblEvDate2
            // 
            this.lblEvDate2.AutoSize = true;
            this.lblEvDate2.Location = new System.Drawing.Point(10, 16);
            this.lblEvDate2.Name = "lblEvDate2";
            this.lblEvDate2.Size = new System.Drawing.Size(33, 13);
            this.lblEvDate2.TabIndex = 0;
            this.lblEvDate2.Text = "Дата";
            // 
            // dtpEvDate2
            // 
            this.dtpEvDate2.CustomFormat = "";
            this.dtpEvDate2.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpEvDate2.Location = new System.Drawing.Point(13, 32);
            this.dtpEvDate2.Name = "dtpEvDate2";
            this.dtpEvDate2.Size = new System.Drawing.Size(100, 20);
            this.dtpEvDate2.TabIndex = 1;
            // 
            // numEvNum
            // 
            this.numEvNum.Location = new System.Drawing.Point(119, 32);
            this.numEvNum.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numEvNum.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numEvNum.Name = "numEvNum";
            this.numEvNum.Size = new System.Drawing.Size(90, 20);
            this.numEvNum.TabIndex = 3;
            this.numEvNum.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblEvNum
            // 
            this.lblEvNum.AutoSize = true;
            this.lblEvNum.Location = new System.Drawing.Point(116, 16);
            this.lblEvNum.Name = "lblEvNum";
            this.lblEvNum.Size = new System.Drawing.Size(87, 13);
            this.lblEvNum.TabIndex = 2;
            this.lblEvNum.Text = "Номер события";
            // 
            // gbGenEv
            // 
            this.gbGenEv.Controls.Add(this.btnSendEvent);
            this.gbGenEv.Controls.Add(this.txtEvData);
            this.gbGenEv.Controls.Add(this.lblEvData);
            this.gbGenEv.Controls.Add(this.txtEvDescr);
            this.gbGenEv.Controls.Add(this.lblEvDescr);
            this.gbGenEv.Controls.Add(this.lblEvNewCnlVal);
            this.gbGenEv.Controls.Add(this.txtEvNewCnlVal);
            this.gbGenEv.Controls.Add(this.numEvUserID1);
            this.gbGenEv.Controls.Add(this.lblEvUserID1);
            this.gbGenEv.Controls.Add(this.numEvNewCnlStat);
            this.gbGenEv.Controls.Add(this.lblEvNewCnlStat);
            this.gbGenEv.Controls.Add(this.numEvOldCnlStat);
            this.gbGenEv.Controls.Add(this.lblEvOldCnlStat);
            this.gbGenEv.Controls.Add(this.lblEvOldCnlVal);
            this.gbGenEv.Controls.Add(this.txtEvOldCnlVal);
            this.gbGenEv.Controls.Add(this.lblEvCnlNum);
            this.gbGenEv.Controls.Add(this.numEvCnlNum);
            this.gbGenEv.Controls.Add(this.numEvParamID);
            this.gbGenEv.Controls.Add(this.lblEvParamID);
            this.gbGenEv.Controls.Add(this.numEvKPNum);
            this.gbGenEv.Controls.Add(this.lblEvKPNum);
            this.gbGenEv.Controls.Add(this.numEvObjNum);
            this.gbGenEv.Controls.Add(this.lblEvObjNum);
            this.gbGenEv.Controls.Add(this.btnSetEvDT);
            this.gbGenEv.Controls.Add(this.lblEvTime);
            this.gbGenEv.Controls.Add(this.lblEvDate1);
            this.gbGenEv.Controls.Add(this.dtpEvTime);
            this.gbGenEv.Controls.Add(this.dtpEvDate1);
            this.gbGenEv.Location = new System.Drawing.Point(6, 67);
            this.gbGenEv.Name = "gbGenEv";
            this.gbGenEv.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbGenEv.Size = new System.Drawing.Size(414, 184);
            this.gbGenEv.TabIndex = 2;
            this.gbGenEv.TabStop = false;
            this.gbGenEv.Text = "Генерация события";
            this.gbGenEv.Visible = false;
            // 
            // btnSendEvent
            // 
            this.btnSendEvent.Location = new System.Drawing.Point(311, 148);
            this.btnSendEvent.Name = "btnSendEvent";
            this.btnSendEvent.Size = new System.Drawing.Size(90, 23);
            this.btnSendEvent.TabIndex = 27;
            this.btnSendEvent.Text = "Отправить";
            this.btnSendEvent.UseVisualStyleBackColor = true;
            this.btnSendEvent.Click += new System.EventHandler(this.btnSendEvent_Click);
            // 
            // txtEvData
            // 
            this.txtEvData.Location = new System.Drawing.Point(171, 149);
            this.txtEvData.MaxLength = 50;
            this.txtEvData.Name = "txtEvData";
            this.txtEvData.Size = new System.Drawing.Size(134, 20);
            this.txtEvData.TabIndex = 26;
            // 
            // lblEvData
            // 
            this.lblEvData.AutoSize = true;
            this.lblEvData.Location = new System.Drawing.Point(168, 133);
            this.lblEvData.Name = "lblEvData";
            this.lblEvData.Size = new System.Drawing.Size(48, 13);
            this.lblEvData.TabIndex = 25;
            this.lblEvData.Text = "Данные";
            // 
            // txtEvDescr
            // 
            this.txtEvDescr.Location = new System.Drawing.Point(13, 149);
            this.txtEvDescr.MaxLength = 100;
            this.txtEvDescr.Name = "txtEvDescr";
            this.txtEvDescr.Size = new System.Drawing.Size(151, 20);
            this.txtEvDescr.TabIndex = 24;
            // 
            // lblEvDescr
            // 
            this.lblEvDescr.AutoSize = true;
            this.lblEvDescr.Location = new System.Drawing.Point(10, 133);
            this.lblEvDescr.Name = "lblEvDescr";
            this.lblEvDescr.Size = new System.Drawing.Size(57, 13);
            this.lblEvDescr.TabIndex = 23;
            this.lblEvDescr.Text = "Описание";
            // 
            // lblEvNewCnlVal
            // 
            this.lblEvNewCnlVal.AutoSize = true;
            this.lblEvNewCnlVal.Location = new System.Drawing.Point(168, 94);
            this.lblEvNewCnlVal.Name = "lblEvNewCnlVal";
            this.lblEvNewCnlVal.Size = new System.Drawing.Size(56, 13);
            this.lblEvNewCnlVal.TabIndex = 17;
            this.lblEvNewCnlVal.Text = "Зн. новое";
            // 
            // txtEvNewCnlVal
            // 
            this.txtEvNewCnlVal.Location = new System.Drawing.Point(171, 110);
            this.txtEvNewCnlVal.Name = "txtEvNewCnlVal";
            this.txtEvNewCnlVal.Size = new System.Drawing.Size(72, 20);
            this.txtEvNewCnlVal.TabIndex = 18;
            this.txtEvNewCnlVal.Text = "0";
            // 
            // numEvUserID1
            // 
            this.numEvUserID1.Location = new System.Drawing.Point(329, 110);
            this.numEvUserID1.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numEvUserID1.Name = "numEvUserID1";
            this.numEvUserID1.Size = new System.Drawing.Size(72, 20);
            this.numEvUserID1.TabIndex = 22;
            // 
            // lblEvUserID1
            // 
            this.lblEvUserID1.AutoSize = true;
            this.lblEvUserID1.Location = new System.Drawing.Point(326, 94);
            this.lblEvUserID1.Name = "lblEvUserID1";
            this.lblEvUserID1.Size = new System.Drawing.Size(42, 13);
            this.lblEvUserID1.TabIndex = 21;
            this.lblEvUserID1.Text = "Польз.";
            // 
            // numEvNewCnlStat
            // 
            this.numEvNewCnlStat.Location = new System.Drawing.Point(250, 110);
            this.numEvNewCnlStat.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numEvNewCnlStat.Name = "numEvNewCnlStat";
            this.numEvNewCnlStat.Size = new System.Drawing.Size(72, 20);
            this.numEvNewCnlStat.TabIndex = 20;
            // 
            // lblEvNewCnlStat
            // 
            this.lblEvNewCnlStat.AutoSize = true;
            this.lblEvNewCnlStat.Location = new System.Drawing.Point(247, 94);
            this.lblEvNewCnlStat.Name = "lblEvNewCnlStat";
            this.lblEvNewCnlStat.Size = new System.Drawing.Size(57, 13);
            this.lblEvNewCnlStat.TabIndex = 19;
            this.lblEvNewCnlStat.Text = "Ст. новый";
            // 
            // numEvOldCnlStat
            // 
            this.numEvOldCnlStat.Location = new System.Drawing.Point(92, 110);
            this.numEvOldCnlStat.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numEvOldCnlStat.Name = "numEvOldCnlStat";
            this.numEvOldCnlStat.Size = new System.Drawing.Size(72, 20);
            this.numEvOldCnlStat.TabIndex = 16;
            // 
            // lblEvOldCnlStat
            // 
            this.lblEvOldCnlStat.AutoSize = true;
            this.lblEvOldCnlStat.Location = new System.Drawing.Point(89, 94);
            this.lblEvOldCnlStat.Name = "lblEvOldCnlStat";
            this.lblEvOldCnlStat.Size = new System.Drawing.Size(62, 13);
            this.lblEvOldCnlStat.TabIndex = 15;
            this.lblEvOldCnlStat.Text = "Ст. старый";
            // 
            // lblEvOldCnlVal
            // 
            this.lblEvOldCnlVal.AutoSize = true;
            this.lblEvOldCnlVal.Location = new System.Drawing.Point(10, 94);
            this.lblEvOldCnlVal.Name = "lblEvOldCnlVal";
            this.lblEvOldCnlVal.Size = new System.Drawing.Size(61, 13);
            this.lblEvOldCnlVal.TabIndex = 13;
            this.lblEvOldCnlVal.Text = "Зн. старое";
            // 
            // txtEvOldCnlVal
            // 
            this.txtEvOldCnlVal.Location = new System.Drawing.Point(13, 110);
            this.txtEvOldCnlVal.Name = "txtEvOldCnlVal";
            this.txtEvOldCnlVal.Size = new System.Drawing.Size(72, 20);
            this.txtEvOldCnlVal.TabIndex = 14;
            this.txtEvOldCnlVal.Text = "0";
            // 
            // lblEvCnlNum
            // 
            this.lblEvCnlNum.AutoSize = true;
            this.lblEvCnlNum.Location = new System.Drawing.Point(247, 55);
            this.lblEvCnlNum.Name = "lblEvCnlNum";
            this.lblEvCnlNum.Size = new System.Drawing.Size(38, 13);
            this.lblEvCnlNum.TabIndex = 11;
            this.lblEvCnlNum.Text = "Канал";
            // 
            // numEvCnlNum
            // 
            this.numEvCnlNum.Location = new System.Drawing.Point(250, 71);
            this.numEvCnlNum.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numEvCnlNum.Name = "numEvCnlNum";
            this.numEvCnlNum.Size = new System.Drawing.Size(72, 20);
            this.numEvCnlNum.TabIndex = 12;
            // 
            // numEvParamID
            // 
            this.numEvParamID.Location = new System.Drawing.Point(171, 71);
            this.numEvParamID.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numEvParamID.Name = "numEvParamID";
            this.numEvParamID.Size = new System.Drawing.Size(72, 20);
            this.numEvParamID.TabIndex = 10;
            // 
            // lblEvParamID
            // 
            this.lblEvParamID.AutoSize = true;
            this.lblEvParamID.Location = new System.Drawing.Point(168, 55);
            this.lblEvParamID.Name = "lblEvParamID";
            this.lblEvParamID.Size = new System.Drawing.Size(58, 13);
            this.lblEvParamID.TabIndex = 9;
            this.lblEvParamID.Text = "Параметр";
            // 
            // numEvKPNum
            // 
            this.numEvKPNum.Location = new System.Drawing.Point(92, 71);
            this.numEvKPNum.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numEvKPNum.Name = "numEvKPNum";
            this.numEvKPNum.Size = new System.Drawing.Size(72, 20);
            this.numEvKPNum.TabIndex = 8;
            // 
            // lblEvKPNum
            // 
            this.lblEvKPNum.AutoSize = true;
            this.lblEvKPNum.Location = new System.Drawing.Point(89, 55);
            this.lblEvKPNum.Name = "lblEvKPNum";
            this.lblEvKPNum.Size = new System.Drawing.Size(22, 13);
            this.lblEvKPNum.TabIndex = 7;
            this.lblEvKPNum.Text = "КП";
            // 
            // numEvObjNum
            // 
            this.numEvObjNum.Location = new System.Drawing.Point(13, 71);
            this.numEvObjNum.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numEvObjNum.Name = "numEvObjNum";
            this.numEvObjNum.Size = new System.Drawing.Size(72, 20);
            this.numEvObjNum.TabIndex = 6;
            // 
            // lblEvObjNum
            // 
            this.lblEvObjNum.AutoSize = true;
            this.lblEvObjNum.Location = new System.Drawing.Point(10, 55);
            this.lblEvObjNum.Name = "lblEvObjNum";
            this.lblEvObjNum.Size = new System.Drawing.Size(45, 13);
            this.lblEvObjNum.TabIndex = 5;
            this.lblEvObjNum.Text = "Объект";
            // 
            // btnSetEvDT
            // 
            this.btnSetEvDT.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSetEvDT.Location = new System.Drawing.Point(225, 32);
            this.btnSetEvDT.Name = "btnSetEvDT";
            this.btnSetEvDT.Size = new System.Drawing.Size(40, 20);
            this.btnSetEvDT.TabIndex = 4;
            this.btnSetEvDT.Text = "Тек.";
            this.btnSetEvDT.UseVisualStyleBackColor = true;
            this.btnSetEvDT.Click += new System.EventHandler(this.btnSetEvDT_Click);
            // 
            // lblEvTime
            // 
            this.lblEvTime.AutoSize = true;
            this.lblEvTime.Location = new System.Drawing.Point(116, 16);
            this.lblEvTime.Name = "lblEvTime";
            this.lblEvTime.Size = new System.Drawing.Size(40, 13);
            this.lblEvTime.TabIndex = 2;
            this.lblEvTime.Text = "Время";
            // 
            // lblEvDate1
            // 
            this.lblEvDate1.AutoSize = true;
            this.lblEvDate1.Location = new System.Drawing.Point(10, 16);
            this.lblEvDate1.Name = "lblEvDate1";
            this.lblEvDate1.Size = new System.Drawing.Size(33, 13);
            this.lblEvDate1.TabIndex = 0;
            this.lblEvDate1.Text = "Дата";
            // 
            // dtpEvTime
            // 
            this.dtpEvTime.CustomFormat = "";
            this.dtpEvTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpEvTime.Location = new System.Drawing.Point(119, 32);
            this.dtpEvTime.Name = "dtpEvTime";
            this.dtpEvTime.ShowUpDown = true;
            this.dtpEvTime.Size = new System.Drawing.Size(100, 20);
            this.dtpEvTime.TabIndex = 3;
            // 
            // dtpEvDate1
            // 
            this.dtpEvDate1.CustomFormat = "";
            this.dtpEvDate1.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpEvDate1.Location = new System.Drawing.Point(13, 32);
            this.dtpEvDate1.Name = "dtpEvDate1";
            this.dtpEvDate1.Size = new System.Drawing.Size(100, 20);
            this.dtpEvDate1.TabIndex = 1;
            // 
            // pageGenCmd
            // 
            this.pageGenCmd.Controls.Add(this.gbGenCmd);
            this.pageGenCmd.Controls.Add(this.gbGenCommSettings3);
            this.pageGenCmd.Controls.Add(this.gbGenPwd3);
            this.pageGenCmd.Location = new System.Drawing.Point(4, 22);
            this.pageGenCmd.Name = "pageGenCmd";
            this.pageGenCmd.Padding = new System.Windows.Forms.Padding(3);
            this.pageGenCmd.Size = new System.Drawing.Size(426, 378);
            this.pageGenCmd.TabIndex = 8;
            this.pageGenCmd.Text = "Команды";
            this.pageGenCmd.UseVisualStyleBackColor = true;
            // 
            // gbGenCmd
            // 
            this.gbGenCmd.Controls.Add(this.pnlCmdKP);
            this.gbGenCmd.Controls.Add(this.btnSendCmd);
            this.gbGenCmd.Controls.Add(this.pnlCmdData);
            this.gbGenCmd.Controls.Add(this.pnlCmdVal);
            this.gbGenCmd.Controls.Add(this.rbCmdReq);
            this.gbGenCmd.Controls.Add(this.rbCmdBin);
            this.gbGenCmd.Controls.Add(this.rbCmdStand);
            this.gbGenCmd.Controls.Add(this.numCmdCtrlCnlNum);
            this.gbGenCmd.Controls.Add(this.lblCmdCtrlCnlNum);
            this.gbGenCmd.Controls.Add(this.numCmdUserID);
            this.gbGenCmd.Controls.Add(this.lblCmdUserID);
            this.gbGenCmd.Location = new System.Drawing.Point(6, 67);
            this.gbGenCmd.Name = "gbGenCmd";
            this.gbGenCmd.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbGenCmd.Size = new System.Drawing.Size(412, 305);
            this.gbGenCmd.TabIndex = 2;
            this.gbGenCmd.TabStop = false;
            this.gbGenCmd.Text = "Команда ТУ";
            this.gbGenCmd.Visible = false;
            // 
            // pnlCmdKP
            // 
            this.pnlCmdKP.Controls.Add(this.numCmdKPNum);
            this.pnlCmdKP.Controls.Add(this.lblCmdKPNum);
            this.pnlCmdKP.Location = new System.Drawing.Point(13, 227);
            this.pnlCmdKP.Name = "pnlCmdKP";
            this.pnlCmdKP.Size = new System.Drawing.Size(386, 36);
            this.pnlCmdKP.TabIndex = 9;
            // 
            // numCmdKPNum
            // 
            this.numCmdKPNum.Location = new System.Drawing.Point(0, 16);
            this.numCmdKPNum.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numCmdKPNum.Name = "numCmdKPNum";
            this.numCmdKPNum.Size = new System.Drawing.Size(60, 20);
            this.numCmdKPNum.TabIndex = 1;
            // 
            // lblCmdKPNum
            // 
            this.lblCmdKPNum.AutoSize = true;
            this.lblCmdKPNum.Location = new System.Drawing.Point(-3, 0);
            this.lblCmdKPNum.Name = "lblCmdKPNum";
            this.lblCmdKPNum.Size = new System.Drawing.Size(22, 13);
            this.lblCmdKPNum.TabIndex = 0;
            this.lblCmdKPNum.Text = "КП";
            // 
            // btnSendCmd
            // 
            this.btnSendCmd.Location = new System.Drawing.Point(13, 269);
            this.btnSendCmd.Name = "btnSendCmd";
            this.btnSendCmd.Size = new System.Drawing.Size(90, 23);
            this.btnSendCmd.TabIndex = 10;
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
            this.pnlCmdData.Size = new System.Drawing.Size(386, 121);
            this.pnlCmdData.TabIndex = 8;
            // 
            // txtCmdData
            // 
            this.txtCmdData.Location = new System.Drawing.Point(0, 23);
            this.txtCmdData.Multiline = true;
            this.txtCmdData.Name = "txtCmdData";
            this.txtCmdData.Size = new System.Drawing.Size(386, 98);
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
            this.pnlCmdVal.Size = new System.Drawing.Size(386, 36);
            this.pnlCmdVal.TabIndex = 7;
            // 
            // btnCmdValOn
            // 
            this.btnCmdValOn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCmdValOn.Location = new System.Drawing.Point(183, 16);
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
            this.btnCmdValOff.Location = new System.Drawing.Point(132, 16);
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
            this.txtCmdVal.Size = new System.Drawing.Size(126, 20);
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
            this.rbCmdReq.Location = new System.Drawing.Point(330, 34);
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
            this.rbCmdBin.Location = new System.Drawing.Point(240, 34);
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
            this.rbCmdStand.Location = new System.Drawing.Point(145, 34);
            this.rbCmdStand.Name = "rbCmdStand";
            this.rbCmdStand.Size = new System.Drawing.Size(90, 17);
            this.rbCmdStand.TabIndex = 4;
            this.rbCmdStand.TabStop = true;
            this.rbCmdStand.Text = "Стандартная";
            this.rbCmdStand.UseVisualStyleBackColor = true;
            this.rbCmdStand.CheckedChanged += new System.EventHandler(this.rbCmdType_CheckedChanged);
            // 
            // numCmdCtrlCnlNum
            // 
            this.numCmdCtrlCnlNum.Location = new System.Drawing.Point(13, 32);
            this.numCmdCtrlCnlNum.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numCmdCtrlCnlNum.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numCmdCtrlCnlNum.Name = "numCmdCtrlCnlNum";
            this.numCmdCtrlCnlNum.Size = new System.Drawing.Size(60, 20);
            this.numCmdCtrlCnlNum.TabIndex = 1;
            this.numCmdCtrlCnlNum.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblCmdCtrlCnlNum
            // 
            this.lblCmdCtrlCnlNum.AutoSize = true;
            this.lblCmdCtrlCnlNum.Location = new System.Drawing.Point(10, 16);
            this.lblCmdCtrlCnlNum.Name = "lblCmdCtrlCnlNum";
            this.lblCmdCtrlCnlNum.Size = new System.Drawing.Size(61, 13);
            this.lblCmdCtrlCnlNum.TabIndex = 0;
            this.lblCmdCtrlCnlNum.Text = "Канал упр.";
            // 
            // numCmdUserID
            // 
            this.numCmdUserID.Location = new System.Drawing.Point(79, 32);
            this.numCmdUserID.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numCmdUserID.Name = "numCmdUserID";
            this.numCmdUserID.Size = new System.Drawing.Size(60, 20);
            this.numCmdUserID.TabIndex = 3;
            // 
            // lblCmdUserID
            // 
            this.lblCmdUserID.AutoSize = true;
            this.lblCmdUserID.Location = new System.Drawing.Point(76, 16);
            this.lblCmdUserID.Name = "lblCmdUserID";
            this.lblCmdUserID.Size = new System.Drawing.Size(42, 13);
            this.lblCmdUserID.TabIndex = 2;
            this.lblCmdUserID.Text = "Польз.";
            // 
            // gbGenCommSettings3
            // 
            this.gbGenCommSettings3.Controls.Add(this.btnGenCommSettings3);
            this.gbGenCommSettings3.Location = new System.Drawing.Point(138, 6);
            this.gbGenCommSettings3.Name = "gbGenCommSettings3";
            this.gbGenCommSettings3.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbGenCommSettings3.Size = new System.Drawing.Size(282, 55);
            this.gbGenCommSettings3.TabIndex = 1;
            this.gbGenCommSettings3.TabStop = false;
            this.gbGenCommSettings3.Text = "Соединение";
            // 
            // btnGenCommSettings3
            // 
            this.btnGenCommSettings3.Location = new System.Drawing.Point(13, 19);
            this.btnGenCommSettings3.Name = "btnGenCommSettings3";
            this.btnGenCommSettings3.Size = new System.Drawing.Size(140, 23);
            this.btnGenCommSettings3.TabIndex = 0;
            this.btnGenCommSettings3.Text = "Настроить соединение";
            this.btnGenCommSettings3.UseVisualStyleBackColor = true;
            this.btnGenCommSettings3.Click += new System.EventHandler(this.btnCommSettings_Click);
            // 
            // gbGenPwd3
            // 
            this.gbGenPwd3.Controls.Add(this.txtGenPwd3);
            this.gbGenPwd3.Location = new System.Drawing.Point(6, 6);
            this.gbGenPwd3.Name = "gbGenPwd3";
            this.gbGenPwd3.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbGenPwd3.Size = new System.Drawing.Size(126, 55);
            this.gbGenPwd3.TabIndex = 0;
            this.gbGenPwd3.TabStop = false;
            this.gbGenPwd3.Text = "Пароль";
            // 
            // txtGenPwd3
            // 
            this.txtGenPwd3.Location = new System.Drawing.Point(13, 20);
            this.txtGenPwd3.Name = "txtGenPwd3";
            this.txtGenPwd3.Size = new System.Drawing.Size(100, 20);
            this.txtGenPwd3.TabIndex = 0;
            this.txtGenPwd3.UseSystemPasswordChar = true;
            this.txtGenPwd3.TextChanged += new System.EventHandler(this.txtGenPwd_TextChanged);
            // 
            // pageStats
            // 
            this.pageStats.Controls.Add(this.chkDetailedLog);
            this.pageStats.Controls.Add(this.lbAppState);
            this.pageStats.Controls.Add(this.lbAppLog);
            this.pageStats.Controls.Add(this.lblAppLog);
            this.pageStats.Controls.Add(this.lblAppState);
            this.pageStats.Location = new System.Drawing.Point(4, 22);
            this.pageStats.Name = "pageStats";
            this.pageStats.Padding = new System.Windows.Forms.Padding(3);
            this.pageStats.Size = new System.Drawing.Size(426, 378);
            this.pageStats.TabIndex = 9;
            this.pageStats.Text = "Статистика";
            this.pageStats.UseVisualStyleBackColor = true;
            // 
            // chkDetailedLog
            // 
            this.chkDetailedLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkDetailedLog.AutoSize = true;
            this.chkDetailedLog.Location = new System.Drawing.Point(296, 165);
            this.chkDetailedLog.Name = "chkDetailedLog";
            this.chkDetailedLog.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chkDetailedLog.Size = new System.Drawing.Size(124, 17);
            this.chkDetailedLog.TabIndex = 3;
            this.chkDetailedLog.Text = "Подробный журнал";
            this.chkDetailedLog.UseVisualStyleBackColor = true;
            this.chkDetailedLog.CheckedChanged += new System.EventHandler(this.control_Changed);
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
            this.lbAppState.Location = new System.Drawing.Point(6, 19);
            this.lbAppState.Name = "lbAppState";
            this.lbAppState.Size = new System.Drawing.Size(414, 140);
            this.lbAppState.TabIndex = 1;
            this.lbAppState.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lbLog_KeyDown);
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
            this.lbAppLog.Location = new System.Drawing.Point(6, 183);
            this.lbAppLog.Name = "lbAppLog";
            this.lbAppLog.Size = new System.Drawing.Size(414, 189);
            this.lbAppLog.TabIndex = 4;
            this.lbAppLog.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lbLog_KeyDown);
            // 
            // lblAppLog
            // 
            this.lblAppLog.AutoSize = true;
            this.lblAppLog.Location = new System.Drawing.Point(3, 167);
            this.lblAppLog.Name = "lblAppLog";
            this.lblAppLog.Size = new System.Drawing.Size(47, 13);
            this.lblAppLog.TabIndex = 2;
            this.lblAppLog.Text = "Журнал";
            // 
            // lblAppState
            // 
            this.lblAppState.AutoSize = true;
            this.lblAppState.Location = new System.Drawing.Point(3, 3);
            this.lblAppState.Name = "lblAppState";
            this.lblAppState.Size = new System.Drawing.Size(61, 13);
            this.lblAppState.TabIndex = 0;
            this.lblAppState.Text = "Состояние";
            // 
            // notifyIcon
            // 
            this.notifyIcon.ContextMenuStrip = this.cmsNotify;
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "SCADA-Сервер";
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
            this.tmrRefr.Tick += new System.EventHandler(this.tmrRefr_Tick);
            // 
            // ilNotify
            // 
            this.ilNotify.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilNotify.ImageStream")));
            this.ilNotify.TransparentColor = System.Drawing.Color.Transparent;
            this.ilNotify.Images.SetKeyName(0, "globe01.ico");
            this.ilNotify.Images.SetKeyName(1, "globe02.ico");
            this.ilNotify.Images.SetKeyName(2, "globe03.ico");
            this.ilNotify.Images.SetKeyName(3, "globe04.ico");
            this.ilNotify.Images.SetKeyName(4, "globe05.ico");
            this.ilNotify.Images.SetKeyName(5, "globe06.ico");
            this.ilNotify.Images.SetKeyName(6, "globe07.ico");
            this.ilNotify.Images.SetKeyName(7, "globe08.ico");
            this.ilNotify.Images.SetKeyName(8, "globe09.ico");
            this.ilNotify.Images.SetKeyName(9, "globe10.ico");
            this.ilNotify.Images.SetKeyName(10, "globe11.ico");
            this.ilNotify.Images.SetKeyName(11, "globe12.ico");
            this.ilNotify.Images.SetKeyName(12, "globe13.ico");
            this.ilNotify.Images.SetKeyName(13, "globe14.ico");
            this.ilNotify.Images.SetKeyName(14, "globe15.ico");
            this.ilNotify.Images.SetKeyName(15, "globe16.ico");
            // 
            // dlgDir
            // 
            this.dlgDir.RootFolder = System.Environment.SpecialFolder.MyComputer;
            // 
            // dlgMod
            // 
            this.dlgMod.DefaultExt = "*.dll";
            this.dlgMod.Filter = "Модули (*.dll)|*.dll|Все файлы (*.*)|*.*";
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(594, 452);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.treeView);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.toolStrip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FrmMain";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SCADA-Сервер";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.Deactivate += new System.EventHandler(this.FrmMain_Deactivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMain_FormClosing);
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.pageCommonParams.ResumeLayout(false);
            this.gbDirs.ResumeLayout(false);
            this.gbDirs.PerformLayout();
            this.gbConn.ResumeLayout(false);
            this.gbConn.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTcpPort)).EndInit();
            this.pageSaveParams.ResumeLayout(false);
            this.gbSaveParamsEv.ResumeLayout(false);
            this.gbSaveParamsEv.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numStoreEvPer)).EndInit();
            this.gbSaveParamsHr.ResumeLayout(false);
            this.gbSaveParamsHr.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numStoreHrPer)).EndInit();
            this.gbSaveParamsMin.ResumeLayout(false);
            this.gbSaveParamsMin.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numStoreMinPer)).EndInit();
            this.gbSaveParamsCur.ResumeLayout(false);
            this.gbSaveParamsCur.PerformLayout();
            this.pageFiles.ResumeLayout(false);
            this.pageFiles.PerformLayout();
            this.pageModules.ResumeLayout(false);
            this.pageModules.PerformLayout();
            this.pageGenSrez.ResumeLayout(false);
            this.gbGenCommSettings1.ResumeLayout(false);
            this.gbGenPwd1.ResumeLayout(false);
            this.gbGenPwd1.PerformLayout();
            this.gbGenSrez.ResumeLayout(false);
            this.gbGenSrez.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSrezCnlStat)).EndInit();
            this.pageGenEv.ResumeLayout(false);
            this.gbGenCommSettings2.ResumeLayout(false);
            this.gbGenPwd2.ResumeLayout(false);
            this.gbGenPwd2.PerformLayout();
            this.gbCheckEv.ResumeLayout(false);
            this.gbCheckEv.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numEvUserID2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numEvNum)).EndInit();
            this.gbGenEv.ResumeLayout(false);
            this.gbGenEv.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numEvUserID1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numEvNewCnlStat)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numEvOldCnlStat)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numEvCnlNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numEvParamID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numEvKPNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numEvObjNum)).EndInit();
            this.pageGenCmd.ResumeLayout(false);
            this.gbGenCmd.ResumeLayout(false);
            this.gbGenCmd.PerformLayout();
            this.pnlCmdKP.ResumeLayout(false);
            this.pnlCmdKP.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCmdKPNum)).EndInit();
            this.pnlCmdData.ResumeLayout(false);
            this.pnlCmdData.PerformLayout();
            this.pnlCmdVal.ResumeLayout(false);
            this.pnlCmdVal.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCmdCtrlCnlNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCmdUserID)).EndInit();
            this.gbGenCommSettings3.ResumeLayout(false);
            this.gbGenPwd3.ResumeLayout(false);
            this.gbGenPwd3.PerformLayout();
            this.pageStats.ResumeLayout(false);
            this.pageStats.PerformLayout();
            this.cmsNotify.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton btnServiceStart;
        private System.Windows.Forms.ToolStripButton btnServiceStop;
        private System.Windows.Forms.ToolStripButton btnServiceRestart;
        private System.Windows.Forms.ToolStripSeparator sepMain1;
        private System.Windows.Forms.ToolStripButton btnSettingsApply;
        private System.Windows.Forms.ToolStripButton btnSettingsCancel;
        private System.Windows.Forms.ToolStripSeparator sepMain2;
        private System.Windows.Forms.ToolStripButton btnAbout;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel lblCurDate;
        private System.Windows.Forms.ToolStripStatusLabel lblCurTime;
        private System.Windows.Forms.ToolStripStatusLabel lblServiceState;
        private System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.ImageList ilTree;
        private System.Windows.Forms.TabPage pageCommonParams;
        private System.Windows.Forms.GroupBox gbConn;
        private System.Windows.Forms.Label lblTcpPort;
        private System.Windows.Forms.NumericUpDown numTcpPort;
        private System.Windows.Forms.Label lblLdapPath;
        private System.Windows.Forms.CheckBox chkUseAD;
        private System.Windows.Forms.TextBox txtLdapPath;
        private System.Windows.Forms.TabPage pageSaveParams;
        private System.Windows.Forms.GroupBox gbSaveParamsCur;
        private System.Windows.Forms.GroupBox gbSaveParamsMin;
        private System.Windows.Forms.ComboBox cbWriteCurPer;
        private System.Windows.Forms.Label lblWriteCurPer;
        private System.Windows.Forms.Label lblInactUnrelTime;
        private System.Windows.Forms.CheckBox chkWriteCurCopy;
        private System.Windows.Forms.CheckBox chkWriteCur;
        private System.Windows.Forms.GroupBox gbSaveParamsEv;
        private System.Windows.Forms.GroupBox gbSaveParamsHr;
        private System.Windows.Forms.CheckBox chkWriteMinCopy;
        private System.Windows.Forms.CheckBox chkWriteMin;
        private System.Windows.Forms.NumericUpDown numStoreMinPer;
        private System.Windows.Forms.Label lblStoreMinPer;
        private System.Windows.Forms.Label lblWriteMinPer;
        private System.Windows.Forms.ComboBox cbWriteMinPer;
        private System.Windows.Forms.CheckBox chkWriteHrCopy;
        private System.Windows.Forms.CheckBox chkWriteHr;
        private System.Windows.Forms.NumericUpDown numStoreHrPer;
        private System.Windows.Forms.Label lblStoreHrPer;
        private System.Windows.Forms.Label lblWriteHrPer;
        private System.Windows.Forms.ComboBox cbWriteHrPer;
        private System.Windows.Forms.CheckBox chkWriteEvCopy;
        private System.Windows.Forms.CheckBox chkWriteEv;
        private System.Windows.Forms.NumericUpDown numStoreEvPer;
        private System.Windows.Forms.Label lblStoreEvPer;
        private System.Windows.Forms.Label lblWriteEvPer;
        private System.Windows.Forms.ComboBox cbWriteEvPer;
        private System.Windows.Forms.ComboBox cbInactUnrelTime;
        private System.Windows.Forms.TabPage pageFiles;
        private System.Windows.Forms.Button btnEditFile;
        private System.Windows.Forms.Button btnViewFile;
        private System.Windows.Forms.ListBox lbFiles;
        private System.Windows.Forms.RadioButton rbFilesMain;
        private System.Windows.Forms.RadioButton rbFilesCopy;
        private System.Windows.Forms.TabPage pageModules;
        private System.Windows.Forms.Button btnModProps;
        private System.Windows.Forms.TextBox txtModDescr;
        private System.Windows.Forms.Label lblModDescr;
        private System.Windows.Forms.ListBox lbModDll;
        private System.Windows.Forms.Button btnModCommSettings;
        private System.Windows.Forms.TabPage pageGenSrez;
        private System.Windows.Forms.TabPage pageGenCmd;
        private System.Windows.Forms.TabPage pageStats;
        private System.Windows.Forms.ListBox lbAppState;
        private System.Windows.Forms.ListBox lbAppLog;
        private System.Windows.Forms.Label lblAppLog;
        private System.Windows.Forms.Label lblAppState;
        private System.Windows.Forms.CheckBox chkDetailedLog;
        private System.Windows.Forms.GroupBox gbGenSrez;
        private System.Windows.Forms.RadioButton rbArcSrez;
        private System.Windows.Forms.RadioButton rbCurSrez;
        private System.Windows.Forms.DateTimePicker dtpSrezDate;
        private System.Windows.Forms.DateTimePicker dtpSrezTime;
        private System.Windows.Forms.ComboBox cbSrezCnlNum;
        private System.Windows.Forms.Label lblSrezCnlNum;
        private System.Windows.Forms.Label lblSrezCnlVal;
        private System.Windows.Forms.TextBox txtSrezCnlVal;
        private System.Windows.Forms.Button btnSrezValMinusOne;
        private System.Windows.Forms.Button btnSrezValPlusOne;
        private System.Windows.Forms.Button btnSrezCnlStatOne;
        private System.Windows.Forms.Button btnSrezCnlStatZero;
        private System.Windows.Forms.Label lblSrezCnlStat;
        private System.Windows.Forms.NumericUpDown numSrezCnlStat;
        private System.Windows.Forms.Button btnSendSrez;
        private System.Windows.Forms.TabPage pageGenEv;
        private System.Windows.Forms.GroupBox gbCheckEv;
        private System.Windows.Forms.Button btnCheckEvent;
        private System.Windows.Forms.Label lblEvUserID2;
        private System.Windows.Forms.NumericUpDown numEvUserID2;
        private System.Windows.Forms.Label lblEvDate2;
        private System.Windows.Forms.DateTimePicker dtpEvDate2;
        private System.Windows.Forms.NumericUpDown numEvNum;
        private System.Windows.Forms.Label lblEvNum;
        private System.Windows.Forms.GroupBox gbGenEv;
        private System.Windows.Forms.Label lblEvData;
        private System.Windows.Forms.TextBox txtEvDescr;
        private System.Windows.Forms.Label lblEvDescr;
        private System.Windows.Forms.Label lblEvNewCnlVal;
        private System.Windows.Forms.TextBox txtEvNewCnlVal;
        private System.Windows.Forms.NumericUpDown numEvUserID1;
        private System.Windows.Forms.Label lblEvUserID1;
        private System.Windows.Forms.NumericUpDown numEvNewCnlStat;
        private System.Windows.Forms.Label lblEvNewCnlStat;
        private System.Windows.Forms.NumericUpDown numEvOldCnlStat;
        private System.Windows.Forms.Label lblEvOldCnlStat;
        private System.Windows.Forms.Label lblEvOldCnlVal;
        private System.Windows.Forms.TextBox txtEvOldCnlVal;
        private System.Windows.Forms.Label lblEvCnlNum;
        private System.Windows.Forms.NumericUpDown numEvCnlNum;
        private System.Windows.Forms.NumericUpDown numEvParamID;
        private System.Windows.Forms.Label lblEvParamID;
        private System.Windows.Forms.NumericUpDown numEvKPNum;
        private System.Windows.Forms.Label lblEvKPNum;
        private System.Windows.Forms.NumericUpDown numEvObjNum;
        private System.Windows.Forms.Label lblEvObjNum;
        private System.Windows.Forms.Button btnSetEvDT;
        private System.Windows.Forms.Label lblEvTime;
        private System.Windows.Forms.Label lblEvDate1;
        private System.Windows.Forms.DateTimePicker dtpEvTime;
        private System.Windows.Forms.DateTimePicker dtpEvDate1;
        private System.Windows.Forms.Button btnSendEvent;
        private System.Windows.Forms.TextBox txtEvData;
        private System.Windows.Forms.GroupBox gbGenPwd1;
        private System.Windows.Forms.TextBox txtGenPwd1;
        private System.Windows.Forms.Button btnGenCommSettings1;
        private System.Windows.Forms.GroupBox gbGenCommSettings1;
        private System.Windows.Forms.GroupBox gbGenCommSettings2;
        private System.Windows.Forms.Button btnGenCommSettings2;
        private System.Windows.Forms.GroupBox gbGenPwd2;
        private System.Windows.Forms.TextBox txtGenPwd2;
        private System.Windows.Forms.GroupBox gbGenCommSettings3;
        private System.Windows.Forms.Button btnGenCommSettings3;
        private System.Windows.Forms.GroupBox gbGenPwd3;
        private System.Windows.Forms.TextBox txtGenPwd3;
        private System.Windows.Forms.GroupBox gbGenCmd;
        private System.Windows.Forms.NumericUpDown numCmdUserID;
        private System.Windows.Forms.Label lblCmdUserID;
        private System.Windows.Forms.NumericUpDown numCmdCtrlCnlNum;
        private System.Windows.Forms.Label lblCmdCtrlCnlNum;
        private System.Windows.Forms.RadioButton rbCmdBin;
        private System.Windows.Forms.RadioButton rbCmdStand;
        private System.Windows.Forms.RadioButton rbCmdReq;
        private System.Windows.Forms.Panel pnlCmdVal;
        private System.Windows.Forms.Label lblCmdVal;
        private System.Windows.Forms.TextBox txtCmdVal;
        private System.Windows.Forms.Button btnCmdValOn;
        private System.Windows.Forms.Button btnCmdValOff;
        private System.Windows.Forms.Panel pnlCmdData;
        private System.Windows.Forms.RadioButton rbCmdStr;
        private System.Windows.Forms.RadioButton rbCmdHex;
        private System.Windows.Forms.TextBox txtCmdData;
        private System.Windows.Forms.Button btnSendCmd;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ContextMenuStrip cmsNotify;
        private System.Windows.Forms.ToolStripMenuItem miNotifyOpen;
        private System.Windows.Forms.ToolStripMenuItem miNotifyStart;
        private System.Windows.Forms.ToolStripMenuItem miNotifyStop;
        private System.Windows.Forms.ToolStripSeparator miNotifySep;
        private System.Windows.Forms.ToolStripMenuItem miNotifyExit;
        private System.Windows.Forms.Timer tmrRefr;
        private System.Windows.Forms.ToolStripMenuItem miNotifyRestart;
        private System.Windows.Forms.ImageList ilNotify;
        private System.Windows.Forms.FolderBrowserDialog dlgDir;
        private System.Windows.Forms.Button btnDelMod;
        private System.Windows.Forms.Button btnMoveDownMod;
        private System.Windows.Forms.Button btnMoveUpMod;
        private System.Windows.Forms.Button btnAddMod;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.OpenFileDialog dlgMod;
        private System.Windows.Forms.Panel pnlCmdKP;
        private System.Windows.Forms.Label lblCmdKPNum;
        private System.Windows.Forms.NumericUpDown numCmdKPNum;
        private System.Windows.Forms.TextBox txtDataDir;
        private System.Windows.Forms.GroupBox gbDirs;
        private System.Windows.Forms.Button btnArcCopyDir;
        private System.Windows.Forms.TextBox txtArcCopyDir;
        private System.Windows.Forms.Label lblArcCopyDir;
        private System.Windows.Forms.Button btnArcDir;
        private System.Windows.Forms.Button btnItfDir;
        private System.Windows.Forms.Button btnBaseDATDir;
        private System.Windows.Forms.TextBox txtArcDir;
        private System.Windows.Forms.Label lblArcDir;
        private System.Windows.Forms.TextBox txtItfDir;
        private System.Windows.Forms.Label lblItfDir;
        private System.Windows.Forms.TextBox txtBaseDATDir;
        private System.Windows.Forms.Label lblBaseDATDir;
    }
}

