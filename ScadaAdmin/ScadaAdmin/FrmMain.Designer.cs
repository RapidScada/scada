namespace ScadaAdmin
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
            this.menuMain = new System.Windows.Forms.MenuStrip();
            this.miDB = new System.Windows.Forms.ToolStripMenuItem();
            this.miDbConnect = new System.Windows.Forms.ToolStripMenuItem();
            this.miDbDisconnect = new System.Windows.Forms.ToolStripMenuItem();
            this.miDbSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.miDbPassToServer = new System.Windows.Forms.ToolStripMenuItem();
            this.miDbBackup = new System.Windows.Forms.ToolStripMenuItem();
            this.miDbCompact = new System.Windows.Forms.ToolStripMenuItem();
            this.miDbSep2 = new System.Windows.Forms.ToolStripSeparator();
            this.miDbExport = new System.Windows.Forms.ToolStripMenuItem();
            this.miDbImport = new System.Windows.Forms.ToolStripMenuItem();
            this.miDbSep3 = new System.Windows.Forms.ToolStripSeparator();
            this.miDbExit = new System.Windows.Forms.ToolStripMenuItem();
            this.miEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.miEditCut = new System.Windows.Forms.ToolStripMenuItem();
            this.miEditCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.miEditPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.miEditSep = new System.Windows.Forms.ToolStripSeparator();
            this.miEditReplace = new System.Windows.Forms.ToolStripMenuItem();
            this.miView = new System.Windows.Forms.ToolStripMenuItem();
            this.miViewGroupByObj = new System.Windows.Forms.ToolStripMenuItem();
            this.miViewGroupByKP = new System.Windows.Forms.ToolStripMenuItem();
            this.miService = new System.Windows.Forms.ToolStripMenuItem();
            this.miServiceCreateCnls = new System.Windows.Forms.ToolStripMenuItem();
            this.miServiceCloneCnls = new System.Windows.Forms.ToolStripMenuItem();
            this.miServiceCnlsMap = new System.Windows.Forms.ToolStripMenuItem();
            this.miServiceSep = new System.Windows.Forms.ToolStripSeparator();
            this.miServiceRestartServer = new System.Windows.Forms.ToolStripMenuItem();
            this.miServiceRestartComm = new System.Windows.Forms.ToolStripMenuItem();
            this.miSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.miSettingsParams = new System.Windows.Forms.ToolStripMenuItem();
            this.miSettingsLanguage = new System.Windows.Forms.ToolStripMenuItem();
            this.miWindow = new System.Windows.Forms.ToolStripMenuItem();
            this.miWindowCloseActive = new System.Windows.Forms.ToolStripMenuItem();
            this.miWindowCloseAll = new System.Windows.Forms.ToolStripMenuItem();
            this.miWindowCloseAllButActive = new System.Windows.Forms.ToolStripMenuItem();
            this.miWindowPrev = new System.Windows.Forms.ToolStripMenuItem();
            this.miWindowNext = new System.Windows.Forms.ToolStripMenuItem();
            this.miHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.miHelpAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.toolMain = new System.Windows.Forms.ToolStrip();
            this.btnConnect = new System.Windows.Forms.ToolStripButton();
            this.btnDisconnect = new System.Windows.Forms.ToolStripButton();
            this.sepFirst = new System.Windows.Forms.ToolStripSeparator();
            this.btnPassToServer = new System.Windows.Forms.ToolStripButton();
            this.btnBackup = new System.Windows.Forms.ToolStripButton();
            this.sepSecond = new System.Windows.Forms.ToolStripSeparator();
            this.btnCut = new System.Windows.Forms.ToolStripButton();
            this.btnCopy = new System.Windows.Forms.ToolStripButton();
            this.btnPaste = new System.Windows.Forms.ToolStripButton();
            this.sepThird = new System.Windows.Forms.ToolStripSeparator();
            this.btnGroupByObj = new System.Windows.Forms.ToolStripButton();
            this.btnGroupByKP = new System.Windows.Forms.ToolStripButton();
            this.sepFourth = new System.Windows.Forms.ToolStripSeparator();
            this.btnRestartServer = new System.Windows.Forms.ToolStripButton();
            this.btnRestartComm = new System.Windows.Forms.ToolStripButton();
            this.sepFifth = new System.Windows.Forms.ToolStripSeparator();
            this.btnParams = new System.Windows.Forms.ToolStripButton();
            this.statusMain = new System.Windows.Forms.StatusStrip();
            this.lblBaseSdfFile = new System.Windows.Forms.ToolStripStatusLabel();
            this.pnlLeft = new System.Windows.Forms.Panel();
            this.treeView = new System.Windows.Forms.TreeView();
            this.ilTree = new System.Windows.Forms.ImageList(this.components);
            this.splitterVert = new System.Windows.Forms.Splitter();
            this.pnlRight = new System.Windows.Forms.Panel();
            this.winControl = new WinControl.WinControl();
            this.contextExpolorer = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miExplorerRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.contextInCnls = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miInCnlProps = new System.Windows.Forms.ToolStripMenuItem();
            this.miRemote = new System.Windows.Forms.ToolStripMenuItem();
            this.miRemoteDownload = new System.Windows.Forms.ToolStripMenuItem();
            this.miRemoteUpload = new System.Windows.Forms.ToolStripMenuItem();
            this.miRemoteStatus = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMain.SuspendLayout();
            this.toolMain.SuspendLayout();
            this.statusMain.SuspendLayout();
            this.pnlLeft.SuspendLayout();
            this.pnlRight.SuspendLayout();
            this.contextExpolorer.SuspendLayout();
            this.contextInCnls.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuMain
            // 
            this.menuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miDB,
            this.miEdit,
            this.miView,
            this.miService,
            this.miRemote,
            this.miSettings,
            this.miWindow,
            this.miHelp});
            this.menuMain.Location = new System.Drawing.Point(0, 0);
            this.menuMain.Name = "menuMain";
            this.menuMain.Size = new System.Drawing.Size(584, 24);
            this.menuMain.TabIndex = 0;
            // 
            // miDB
            // 
            this.miDB.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miDbConnect,
            this.miDbDisconnect,
            this.miDbSep1,
            this.miDbPassToServer,
            this.miDbBackup,
            this.miDbCompact,
            this.miDbSep2,
            this.miDbExport,
            this.miDbImport,
            this.miDbSep3,
            this.miDbExit});
            this.miDB.Name = "miDB";
            this.miDB.Size = new System.Drawing.Size(86, 20);
            this.miDB.Text = "&База данных";
            // 
            // miDbConnect
            // 
            this.miDbConnect.Image = ((System.Drawing.Image)(resources.GetObject("miDbConnect.Image")));
            this.miDbConnect.Name = "miDbConnect";
            this.miDbConnect.Size = new System.Drawing.Size(217, 22);
            this.miDbConnect.Text = "Соединиться";
            this.miDbConnect.Click += new System.EventHandler(this.miDbConnect_Click);
            // 
            // miDbDisconnect
            // 
            this.miDbDisconnect.Image = ((System.Drawing.Image)(resources.GetObject("miDbDisconnect.Image")));
            this.miDbDisconnect.Name = "miDbDisconnect";
            this.miDbDisconnect.Size = new System.Drawing.Size(217, 22);
            this.miDbDisconnect.Text = "Разъединиться";
            this.miDbDisconnect.Click += new System.EventHandler(this.miDbDisconnect_Click);
            // 
            // miDbSep1
            // 
            this.miDbSep1.Name = "miDbSep1";
            this.miDbSep1.Size = new System.Drawing.Size(214, 6);
            // 
            // miDbPassToServer
            // 
            this.miDbPassToServer.Image = ((System.Drawing.Image)(resources.GetObject("miDbPassToServer.Image")));
            this.miDbPassToServer.Name = "miDbPassToServer";
            this.miDbPassToServer.Size = new System.Drawing.Size(217, 22);
            this.miDbPassToServer.Text = "Передать SCADA-Серверу";
            this.miDbPassToServer.Click += new System.EventHandler(this.miDbPassToServer_Click);
            // 
            // miDbBackup
            // 
            this.miDbBackup.Image = ((System.Drawing.Image)(resources.GetObject("miDbBackup.Image")));
            this.miDbBackup.Name = "miDbBackup";
            this.miDbBackup.Size = new System.Drawing.Size(217, 22);
            this.miDbBackup.Text = "Резервировать";
            this.miDbBackup.Click += new System.EventHandler(this.miDbBackup_Click);
            // 
            // miDbCompact
            // 
            this.miDbCompact.Name = "miDbCompact";
            this.miDbCompact.Size = new System.Drawing.Size(217, 22);
            this.miDbCompact.Text = "Упаковать";
            this.miDbCompact.Click += new System.EventHandler(this.miDbCompact_Click);
            // 
            // miDbSep2
            // 
            this.miDbSep2.Name = "miDbSep2";
            this.miDbSep2.Size = new System.Drawing.Size(214, 6);
            // 
            // miDbExport
            // 
            this.miDbExport.Name = "miDbExport";
            this.miDbExport.Size = new System.Drawing.Size(217, 22);
            this.miDbExport.Text = "Экспорт...";
            this.miDbExport.Click += new System.EventHandler(this.miDbExport_Click);
            // 
            // miDbImport
            // 
            this.miDbImport.Name = "miDbImport";
            this.miDbImport.Size = new System.Drawing.Size(217, 22);
            this.miDbImport.Text = "Импорт...";
            this.miDbImport.Click += new System.EventHandler(this.miDbImport_Click);
            // 
            // miDbSep3
            // 
            this.miDbSep3.Name = "miDbSep3";
            this.miDbSep3.Size = new System.Drawing.Size(214, 6);
            // 
            // miDbExit
            // 
            this.miDbExit.Name = "miDbExit";
            this.miDbExit.Size = new System.Drawing.Size(217, 22);
            this.miDbExit.Text = "Выход";
            this.miDbExit.Click += new System.EventHandler(this.miDbExit_Click);
            // 
            // miEdit
            // 
            this.miEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miEditCut,
            this.miEditCopy,
            this.miEditPaste,
            this.miEditSep,
            this.miEditReplace});
            this.miEdit.Name = "miEdit";
            this.miEdit.Size = new System.Drawing.Size(59, 20);
            this.miEdit.Text = "&Правка";
            // 
            // miEditCut
            // 
            this.miEditCut.Image = ((System.Drawing.Image)(resources.GetObject("miEditCut.Image")));
            this.miEditCut.Name = "miEditCut";
            this.miEditCut.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.miEditCut.Size = new System.Drawing.Size(212, 22);
            this.miEditCut.Text = "Вырезать";
            this.miEditCut.Click += new System.EventHandler(this.miEditCut_Click);
            // 
            // miEditCopy
            // 
            this.miEditCopy.Image = ((System.Drawing.Image)(resources.GetObject("miEditCopy.Image")));
            this.miEditCopy.Name = "miEditCopy";
            this.miEditCopy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.miEditCopy.Size = new System.Drawing.Size(212, 22);
            this.miEditCopy.Text = "Копировать";
            this.miEditCopy.Click += new System.EventHandler(this.miEditCopy_Click);
            // 
            // miEditPaste
            // 
            this.miEditPaste.Image = ((System.Drawing.Image)(resources.GetObject("miEditPaste.Image")));
            this.miEditPaste.Name = "miEditPaste";
            this.miEditPaste.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.miEditPaste.Size = new System.Drawing.Size(212, 22);
            this.miEditPaste.Text = "Вставить";
            this.miEditPaste.Click += new System.EventHandler(this.miEditPaste_Click);
            // 
            // miEditSep
            // 
            this.miEditSep.Name = "miEditSep";
            this.miEditSep.Size = new System.Drawing.Size(209, 6);
            // 
            // miEditReplace
            // 
            this.miEditReplace.Name = "miEditReplace";
            this.miEditReplace.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.miEditReplace.Size = new System.Drawing.Size(212, 22);
            this.miEditReplace.Text = "Найти и заменить";
            this.miEditReplace.Click += new System.EventHandler(this.miEditReplace_Click);
            // 
            // miView
            // 
            this.miView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miViewGroupByObj,
            this.miViewGroupByKP});
            this.miView.Name = "miView";
            this.miView.Size = new System.Drawing.Size(39, 20);
            this.miView.Text = "&Вид";
            // 
            // miViewGroupByObj
            // 
            this.miViewGroupByObj.Image = ((System.Drawing.Image)(resources.GetObject("miViewGroupByObj.Image")));
            this.miViewGroupByObj.Name = "miViewGroupByObj";
            this.miViewGroupByObj.Size = new System.Drawing.Size(268, 22);
            this.miViewGroupByObj.Text = "Группировать каналы по объектам";
            this.miViewGroupByObj.Click += new System.EventHandler(this.miViewGroupByObj_Click);
            // 
            // miViewGroupByKP
            // 
            this.miViewGroupByKP.Checked = true;
            this.miViewGroupByKP.CheckState = System.Windows.Forms.CheckState.Checked;
            this.miViewGroupByKP.Image = ((System.Drawing.Image)(resources.GetObject("miViewGroupByKP.Image")));
            this.miViewGroupByKP.Name = "miViewGroupByKP";
            this.miViewGroupByKP.Size = new System.Drawing.Size(268, 22);
            this.miViewGroupByKP.Text = "Группировать каналы по КП";
            this.miViewGroupByKP.Click += new System.EventHandler(this.miViewGroupByKP_Click);
            // 
            // miService
            // 
            this.miService.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miServiceCreateCnls,
            this.miServiceCloneCnls,
            this.miServiceCnlsMap,
            this.miServiceSep,
            this.miServiceRestartServer,
            this.miServiceRestartComm});
            this.miService.Name = "miService";
            this.miService.Size = new System.Drawing.Size(59, 20);
            this.miService.Text = "С&ервис";
            // 
            // miServiceCreateCnls
            // 
            this.miServiceCreateCnls.Name = "miServiceCreateCnls";
            this.miServiceCreateCnls.Size = new System.Drawing.Size(284, 22);
            this.miServiceCreateCnls.Text = "Создание каналов...";
            this.miServiceCreateCnls.Click += new System.EventHandler(this.miServiceCreateCnls_Click);
            // 
            // miServiceCloneCnls
            // 
            this.miServiceCloneCnls.Name = "miServiceCloneCnls";
            this.miServiceCloneCnls.Size = new System.Drawing.Size(284, 22);
            this.miServiceCloneCnls.Text = "Клонирование каналов...";
            this.miServiceCloneCnls.Click += new System.EventHandler(this.miServiceCloneCnls_Click);
            // 
            // miServiceCnlsMap
            // 
            this.miServiceCnlsMap.Name = "miServiceCnlsMap";
            this.miServiceCnlsMap.Size = new System.Drawing.Size(284, 22);
            this.miServiceCnlsMap.Text = "Карта каналов...";
            this.miServiceCnlsMap.Click += new System.EventHandler(this.miServiceCnlsMap_Click);
            // 
            // miServiceSep
            // 
            this.miServiceSep.Name = "miServiceSep";
            this.miServiceSep.Size = new System.Drawing.Size(281, 6);
            // 
            // miServiceRestartServer
            // 
            this.miServiceRestartServer.Image = ((System.Drawing.Image)(resources.GetObject("miServiceRestartServer.Image")));
            this.miServiceRestartServer.Name = "miServiceRestartServer";
            this.miServiceRestartServer.Size = new System.Drawing.Size(284, 22);
            this.miServiceRestartServer.Text = "Перезапустить SCADA-Сервер";
            this.miServiceRestartServer.Click += new System.EventHandler(this.miServiceRestart_Click);
            // 
            // miServiceRestartComm
            // 
            this.miServiceRestartComm.Image = ((System.Drawing.Image)(resources.GetObject("miServiceRestartComm.Image")));
            this.miServiceRestartComm.Name = "miServiceRestartComm";
            this.miServiceRestartComm.Size = new System.Drawing.Size(284, 22);
            this.miServiceRestartComm.Text = "Перезапустить SCADA-Коммуникатор";
            this.miServiceRestartComm.Click += new System.EventHandler(this.miServiceRestart_Click);
            // 
            // miSettings
            // 
            this.miSettings.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miSettingsParams,
            this.miSettingsLanguage});
            this.miSettings.Name = "miSettings";
            this.miSettings.Size = new System.Drawing.Size(78, 20);
            this.miSettings.Text = "&Настройка";
            // 
            // miSettingsParams
            // 
            this.miSettingsParams.Image = ((System.Drawing.Image)(resources.GetObject("miSettingsParams.Image")));
            this.miSettingsParams.Name = "miSettingsParams";
            this.miSettingsParams.Size = new System.Drawing.Size(180, 22);
            this.miSettingsParams.Text = "Параметры...";
            this.miSettingsParams.Click += new System.EventHandler(this.miSettingsParams_Click);
            // 
            // miSettingsLanguage
            // 
            this.miSettingsLanguage.Name = "miSettingsLanguage";
            this.miSettingsLanguage.Size = new System.Drawing.Size(180, 22);
            this.miSettingsLanguage.Text = "Язык...";
            this.miSettingsLanguage.Click += new System.EventHandler(this.miSettingsLanguage_Click);
            // 
            // miWindow
            // 
            this.miWindow.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miWindowCloseActive,
            this.miWindowCloseAll,
            this.miWindowCloseAllButActive,
            this.miWindowPrev,
            this.miWindowNext});
            this.miWindow.Name = "miWindow";
            this.miWindow.Size = new System.Drawing.Size(48, 20);
            this.miWindow.Text = "&Окно";
            // 
            // miWindowCloseActive
            // 
            this.miWindowCloseActive.Name = "miWindowCloseActive";
            this.miWindowCloseActive.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F4)));
            this.miWindowCloseActive.Size = new System.Drawing.Size(238, 22);
            this.miWindowCloseActive.Text = "Закрыть активное";
            this.miWindowCloseActive.Click += new System.EventHandler(this.miWindowCloseActive_Click);
            // 
            // miWindowCloseAll
            // 
            this.miWindowCloseAll.Name = "miWindowCloseAll";
            this.miWindowCloseAll.Size = new System.Drawing.Size(238, 22);
            this.miWindowCloseAll.Text = "Закрыть все";
            this.miWindowCloseAll.Click += new System.EventHandler(this.miWindowCloseAll_Click);
            // 
            // miWindowCloseAllButActive
            // 
            this.miWindowCloseAllButActive.Name = "miWindowCloseAllButActive";
            this.miWindowCloseAllButActive.Size = new System.Drawing.Size(238, 22);
            this.miWindowCloseAllButActive.Text = "Закрыть все кроме активного";
            this.miWindowCloseAllButActive.Click += new System.EventHandler(this.miWindowCloseAllButActive_Click);
            // 
            // miWindowPrev
            // 
            this.miWindowPrev.Name = "miWindowPrev";
            this.miWindowPrev.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.Tab)));
            this.miWindowPrev.Size = new System.Drawing.Size(238, 22);
            this.miWindowPrev.Text = "Предыдущее";
            this.miWindowPrev.Visible = false;
            this.miWindowPrev.Click += new System.EventHandler(this.miWindowPrev_Click);
            // 
            // miWindowNext
            // 
            this.miWindowNext.Name = "miWindowNext";
            this.miWindowNext.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Tab)));
            this.miWindowNext.Size = new System.Drawing.Size(238, 22);
            this.miWindowNext.Text = "Следующее";
            this.miWindowNext.Visible = false;
            this.miWindowNext.Click += new System.EventHandler(this.miWindowNext_Click);
            // 
            // miHelp
            // 
            this.miHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miHelpAbout});
            this.miHelp.Name = "miHelp";
            this.miHelp.Size = new System.Drawing.Size(65, 20);
            this.miHelp.Text = "&Справка";
            // 
            // miHelpAbout
            // 
            this.miHelpAbout.Image = ((System.Drawing.Image)(resources.GetObject("miHelpAbout.Image")));
            this.miHelpAbout.Name = "miHelpAbout";
            this.miHelpAbout.Size = new System.Drawing.Size(158, 22);
            this.miHelpAbout.Text = "О программе...";
            this.miHelpAbout.Click += new System.EventHandler(this.miHelpAbout_Click);
            // 
            // toolMain
            // 
            this.toolMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnConnect,
            this.btnDisconnect,
            this.sepFirst,
            this.btnPassToServer,
            this.btnBackup,
            this.sepSecond,
            this.btnCut,
            this.btnCopy,
            this.btnPaste,
            this.sepThird,
            this.btnGroupByObj,
            this.btnGroupByKP,
            this.sepFourth,
            this.btnRestartServer,
            this.btnRestartComm,
            this.sepFifth,
            this.btnParams});
            this.toolMain.Location = new System.Drawing.Point(0, 24);
            this.toolMain.Name = "toolMain";
            this.toolMain.Size = new System.Drawing.Size(584, 25);
            this.toolMain.TabIndex = 1;
            this.toolMain.Text = "toolMain";
            // 
            // btnConnect
            // 
            this.btnConnect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnConnect.Image = ((System.Drawing.Image)(resources.GetObject("btnConnect.Image")));
            this.btnConnect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(23, 22);
            this.btnConnect.Text = "Соединиться";
            this.btnConnect.Click += new System.EventHandler(this.miDbConnect_Click);
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnDisconnect.Image = ((System.Drawing.Image)(resources.GetObject("btnDisconnect.Image")));
            this.btnDisconnect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(23, 22);
            this.btnDisconnect.Text = "Разъединиться";
            this.btnDisconnect.Click += new System.EventHandler(this.miDbDisconnect_Click);
            // 
            // sepFirst
            // 
            this.sepFirst.Name = "sepFirst";
            this.sepFirst.Size = new System.Drawing.Size(6, 25);
            // 
            // btnPassToServer
            // 
            this.btnPassToServer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnPassToServer.Image = ((System.Drawing.Image)(resources.GetObject("btnPassToServer.Image")));
            this.btnPassToServer.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPassToServer.Name = "btnPassToServer";
            this.btnPassToServer.Size = new System.Drawing.Size(23, 22);
            this.btnPassToServer.Text = "Передать SCADA-Серверу";
            this.btnPassToServer.Click += new System.EventHandler(this.miDbPassToServer_Click);
            // 
            // btnBackup
            // 
            this.btnBackup.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnBackup.Image = ((System.Drawing.Image)(resources.GetObject("btnBackup.Image")));
            this.btnBackup.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnBackup.Name = "btnBackup";
            this.btnBackup.Size = new System.Drawing.Size(23, 22);
            this.btnBackup.Text = "Резервировать";
            this.btnBackup.Click += new System.EventHandler(this.miDbBackup_Click);
            // 
            // sepSecond
            // 
            this.sepSecond.Name = "sepSecond";
            this.sepSecond.Size = new System.Drawing.Size(6, 25);
            // 
            // btnCut
            // 
            this.btnCut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnCut.Image = ((System.Drawing.Image)(resources.GetObject("btnCut.Image")));
            this.btnCut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCut.Name = "btnCut";
            this.btnCut.Size = new System.Drawing.Size(23, 22);
            this.btnCut.Text = "Вырезать";
            this.btnCut.Click += new System.EventHandler(this.miEditCut_Click);
            // 
            // btnCopy
            // 
            this.btnCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnCopy.Image = ((System.Drawing.Image)(resources.GetObject("btnCopy.Image")));
            this.btnCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(23, 22);
            this.btnCopy.Text = "Копировать";
            this.btnCopy.Click += new System.EventHandler(this.miEditCopy_Click);
            // 
            // btnPaste
            // 
            this.btnPaste.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnPaste.Image = ((System.Drawing.Image)(resources.GetObject("btnPaste.Image")));
            this.btnPaste.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPaste.Name = "btnPaste";
            this.btnPaste.Size = new System.Drawing.Size(23, 22);
            this.btnPaste.Text = "Вставить";
            this.btnPaste.Click += new System.EventHandler(this.miEditPaste_Click);
            // 
            // sepThird
            // 
            this.sepThird.Name = "sepThird";
            this.sepThird.Size = new System.Drawing.Size(6, 25);
            // 
            // btnGroupByObj
            // 
            this.btnGroupByObj.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnGroupByObj.Image = ((System.Drawing.Image)(resources.GetObject("btnGroupByObj.Image")));
            this.btnGroupByObj.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnGroupByObj.Name = "btnGroupByObj";
            this.btnGroupByObj.Size = new System.Drawing.Size(23, 22);
            this.btnGroupByObj.Text = "Группировать каналы по объектам";
            this.btnGroupByObj.Click += new System.EventHandler(this.miViewGroupByObj_Click);
            // 
            // btnGroupByKP
            // 
            this.btnGroupByKP.Checked = true;
            this.btnGroupByKP.CheckState = System.Windows.Forms.CheckState.Checked;
            this.btnGroupByKP.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnGroupByKP.Image = ((System.Drawing.Image)(resources.GetObject("btnGroupByKP.Image")));
            this.btnGroupByKP.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnGroupByKP.Name = "btnGroupByKP";
            this.btnGroupByKP.Size = new System.Drawing.Size(23, 22);
            this.btnGroupByKP.Text = "Группировать каналы по КП";
            this.btnGroupByKP.Click += new System.EventHandler(this.miViewGroupByKP_Click);
            // 
            // sepFourth
            // 
            this.sepFourth.Name = "sepFourth";
            this.sepFourth.Size = new System.Drawing.Size(6, 25);
            // 
            // btnRestartServer
            // 
            this.btnRestartServer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnRestartServer.Image = ((System.Drawing.Image)(resources.GetObject("btnRestartServer.Image")));
            this.btnRestartServer.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRestartServer.Name = "btnRestartServer";
            this.btnRestartServer.Size = new System.Drawing.Size(23, 22);
            this.btnRestartServer.Text = "Перезапустить службу SCADA-Сервера";
            this.btnRestartServer.Click += new System.EventHandler(this.miServiceRestart_Click);
            // 
            // btnRestartComm
            // 
            this.btnRestartComm.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnRestartComm.Image = ((System.Drawing.Image)(resources.GetObject("btnRestartComm.Image")));
            this.btnRestartComm.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRestartComm.Name = "btnRestartComm";
            this.btnRestartComm.Size = new System.Drawing.Size(23, 22);
            this.btnRestartComm.Text = "Перезапустить службу SCADA-Коммуникатора";
            this.btnRestartComm.Click += new System.EventHandler(this.miServiceRestart_Click);
            // 
            // sepFifth
            // 
            this.sepFifth.Name = "sepFifth";
            this.sepFifth.Size = new System.Drawing.Size(6, 25);
            // 
            // btnParams
            // 
            this.btnParams.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnParams.Image = ((System.Drawing.Image)(resources.GetObject("btnParams.Image")));
            this.btnParams.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnParams.Name = "btnParams";
            this.btnParams.Size = new System.Drawing.Size(23, 22);
            this.btnParams.Text = "Параметры";
            this.btnParams.Click += new System.EventHandler(this.miSettingsParams_Click);
            // 
            // statusMain
            // 
            this.statusMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblBaseSdfFile});
            this.statusMain.Location = new System.Drawing.Point(0, 390);
            this.statusMain.Name = "statusMain";
            this.statusMain.Size = new System.Drawing.Size(584, 22);
            this.statusMain.TabIndex = 2;
            // 
            // lblBaseSdfFile
            // 
            this.lblBaseSdfFile.Name = "lblBaseSdfFile";
            this.lblBaseSdfFile.Size = new System.Drawing.Size(189, 17);
            this.lblBaseSdfFile.Text = "C:\\SCADA\\BaseSDF\\ScadaBase.sdf";
            // 
            // pnlLeft
            // 
            this.pnlLeft.Controls.Add(this.treeView);
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlLeft.Location = new System.Drawing.Point(0, 49);
            this.pnlLeft.MinimumSize = new System.Drawing.Size(150, 0);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Size = new System.Drawing.Size(200, 341);
            this.pnlLeft.TabIndex = 3;
            // 
            // treeView
            // 
            this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView.ForeColor = System.Drawing.SystemColors.WindowText;
            this.treeView.HideSelection = false;
            this.treeView.ImageKey = "table.gif";
            this.treeView.ImageList = this.ilTree;
            this.treeView.Location = new System.Drawing.Point(0, 0);
            this.treeView.Name = "treeView";
            this.treeView.SelectedImageKey = "table.gif";
            this.treeView.Size = new System.Drawing.Size(200, 341);
            this.treeView.TabIndex = 1;
            this.treeView.BeforeCollapse += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView_BeforeCollapse);
            this.treeView.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView_BeforeExpand);
            this.treeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView_NodeMouseClick);
            this.treeView.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView_NodeMouseDoubleClick);
            this.treeView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.treeView_KeyDown);
            this.treeView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.treeView_MouseDown);
            // 
            // ilTree
            // 
            this.ilTree.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilTree.ImageStream")));
            this.ilTree.TransparentColor = System.Drawing.Color.Transparent;
            this.ilTree.Images.SetKeyName(0, "db.gif");
            this.ilTree.Images.SetKeyName(1, "db_gray.gif");
            this.ilTree.Images.SetKeyName(2, "folder_open.gif");
            this.ilTree.Images.SetKeyName(3, "folder_closed.gif");
            this.ilTree.Images.SetKeyName(4, "table.gif");
            this.ilTree.Images.SetKeyName(5, "object.gif");
            this.ilTree.Images.SetKeyName(6, "kp.gif");
            // 
            // splitterVert
            // 
            this.splitterVert.Location = new System.Drawing.Point(200, 49);
            this.splitterVert.MinExtra = 150;
            this.splitterVert.MinSize = 150;
            this.splitterVert.Name = "splitterVert";
            this.splitterVert.Size = new System.Drawing.Size(3, 341);
            this.splitterVert.TabIndex = 4;
            this.splitterVert.TabStop = false;
            // 
            // pnlRight
            // 
            this.pnlRight.Controls.Add(this.winControl);
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRight.Location = new System.Drawing.Point(203, 49);
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Size = new System.Drawing.Size(381, 341);
            this.pnlRight.TabIndex = 5;
            // 
            // winControl
            // 
            this.winControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.winControl.Image = null;
            this.winControl.Location = new System.Drawing.Point(0, 0);
            this.winControl.Margin = new System.Windows.Forms.Padding(7);
            this.winControl.MessageFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.winControl.MessageText = "Выберите в проводнике\r\nтаблицу для редактирования";
            this.winControl.Name = "winControl";
            this.winControl.SaveReqCancel = "Отмена";
            this.winControl.SaveReqCaption = "Сохранение изменений";
            this.winControl.SaveReqNo = "&Нет";
            this.winControl.SaveReqQuestion = "Сохранить изменения?";
            this.winControl.SaveReqYes = "&Да";
            this.winControl.Size = new System.Drawing.Size(381, 341);
            this.winControl.TabIndex = 0;
            this.winControl.ActiveFormChanged += new System.EventHandler(this.winControl_ActiveFormChanged);
            // 
            // contextExpolorer
            // 
            this.contextExpolorer.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miExplorerRefresh});
            this.contextExpolorer.Name = "contextExpolorer";
            this.contextExpolorer.Size = new System.Drawing.Size(129, 26);
            // 
            // miExplorerRefresh
            // 
            this.miExplorerRefresh.Image = ((System.Drawing.Image)(resources.GetObject("miExplorerRefresh.Image")));
            this.miExplorerRefresh.Name = "miExplorerRefresh";
            this.miExplorerRefresh.Size = new System.Drawing.Size(128, 22);
            this.miExplorerRefresh.Text = "Обновить";
            this.miExplorerRefresh.Click += new System.EventHandler(this.miExplorerRefresh_Click);
            // 
            // contextInCnls
            // 
            this.contextInCnls.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miInCnlProps});
            this.contextInCnls.Name = "contextInCnls";
            this.contextInCnls.Size = new System.Drawing.Size(167, 26);
            // 
            // miInCnlProps
            // 
            this.miInCnlProps.Image = ((System.Drawing.Image)(resources.GetObject("miInCnlProps.Image")));
            this.miInCnlProps.Name = "miInCnlProps";
            this.miInCnlProps.Size = new System.Drawing.Size(166, 22);
            this.miInCnlProps.Text = "Свойства канала";
            this.miInCnlProps.Click += new System.EventHandler(this.miInCnlProps_Click);
            // 
            // miRemote
            // 
            this.miRemote.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miRemoteDownload,
            this.miRemoteUpload,
            this.miRemoteStatus});
            this.miRemote.Name = "miRemote";
            this.miRemote.Size = new System.Drawing.Size(122, 20);
            this.miRemote.Text = "&Удаленный сервер";
            // 
            // miRemoteDownload
            // 
            this.miRemoteDownload.Name = "miRemoteDownload";
            this.miRemoteDownload.Size = new System.Drawing.Size(221, 22);
            this.miRemoteDownload.Text = "Скачать конфигурацию...";
            this.miRemoteDownload.Click += new System.EventHandler(this.miRemoteDownload_Click);
            // 
            // miRemoteUpload
            // 
            this.miRemoteUpload.Name = "miRemoteUpload";
            this.miRemoteUpload.Size = new System.Drawing.Size(221, 22);
            this.miRemoteUpload.Text = "Передать конфигурацию...";
            this.miRemoteUpload.Click += new System.EventHandler(this.miRemoteUpload_Click);
            // 
            // miRemoteStatus
            // 
            this.miRemoteStatus.Name = "miRemoteStatus";
            this.miRemoteStatus.Size = new System.Drawing.Size(221, 22);
            this.miRemoteStatus.Text = "Статус сервера...";
            this.miRemoteStatus.Click += new System.EventHandler(this.miRemoteStatus_Click);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 412);
            this.Controls.Add(this.pnlRight);
            this.Controls.Add(this.splitterVert);
            this.Controls.Add(this.pnlLeft);
            this.Controls.Add(this.statusMain);
            this.Controls.Add(this.toolMain);
            this.Controls.Add(this.menuMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuMain;
            this.MinimumSize = new System.Drawing.Size(600, 450);
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SCADA-Администратор";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMain_FormClosing);
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.Shown += new System.EventHandler(this.FrmMain_Shown);
            this.menuMain.ResumeLayout(false);
            this.menuMain.PerformLayout();
            this.toolMain.ResumeLayout(false);
            this.toolMain.PerformLayout();
            this.statusMain.ResumeLayout(false);
            this.statusMain.PerformLayout();
            this.pnlLeft.ResumeLayout(false);
            this.pnlRight.ResumeLayout(false);
            this.contextExpolorer.ResumeLayout(false);
            this.contextInCnls.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuMain;
        private System.Windows.Forms.ToolStripMenuItem miDB;
        private System.Windows.Forms.ToolStripMenuItem miDbConnect;
        private System.Windows.Forms.ToolStripMenuItem miDbDisconnect;
        private System.Windows.Forms.ToolStrip toolMain;
        private System.Windows.Forms.ToolStripButton btnConnect;
        private System.Windows.Forms.StatusStrip statusMain;
        private System.Windows.Forms.Panel pnlLeft;
        private System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.Splitter splitterVert;
        private System.Windows.Forms.Panel pnlRight;
        private System.Windows.Forms.ToolStripSeparator miDbSep1;
        private System.Windows.Forms.ToolStripMenuItem miDbExit;
        private System.Windows.Forms.ToolStripMenuItem miEdit;
        private System.Windows.Forms.ToolStripMenuItem miHelp;
        private System.Windows.Forms.ToolStripMenuItem miView;
        private System.Windows.Forms.ToolStripMenuItem miSettings;
        private System.Windows.Forms.ToolStripMenuItem miSettingsParams;
        private System.Windows.Forms.ToolStripMenuItem miService;
        private WinControl.WinControl winControl;
        private System.Windows.Forms.ImageList ilTree;
        private System.Windows.Forms.ToolStripButton btnDisconnect;
        private System.Windows.Forms.ToolStripSeparator sepFirst;
        private System.Windows.Forms.ToolStripButton btnCut;
        private System.Windows.Forms.ToolStripButton btnCopy;
        private System.Windows.Forms.ToolStripButton btnPaste;
        private System.Windows.Forms.ToolStripSeparator sepSecond;
        private System.Windows.Forms.ToolStripButton btnGroupByObj;
        private System.Windows.Forms.ToolStripButton btnGroupByKP;
        private System.Windows.Forms.ToolStripSeparator sepThird;
        private System.Windows.Forms.ToolStripButton btnParams;
        private System.Windows.Forms.ToolStripSeparator miDbSep2;
        private System.Windows.Forms.ToolStripMenuItem miDbCompact;
        private System.Windows.Forms.ToolStripMenuItem miDbPassToServer;
        private System.Windows.Forms.ToolStripStatusLabel lblBaseSdfFile;
        private System.Windows.Forms.ToolStripMenuItem miWindow;
        private System.Windows.Forms.ToolStripMenuItem miWindowCloseActive;
        private System.Windows.Forms.ToolStripMenuItem miWindowCloseAll;
        private System.Windows.Forms.ToolStripMenuItem miWindowCloseAllButActive;
        private System.Windows.Forms.ToolStripMenuItem miWindowNext;
        private System.Windows.Forms.ToolStripMenuItem miWindowPrev;
        private System.Windows.Forms.ToolStripMenuItem miEditCut;
        private System.Windows.Forms.ToolStripMenuItem miEditCopy;
        private System.Windows.Forms.ToolStripMenuItem miEditPaste;
        private System.Windows.Forms.ToolStripMenuItem miViewGroupByObj;
        private System.Windows.Forms.ToolStripMenuItem miViewGroupByKP;
        private System.Windows.Forms.ContextMenuStrip contextExpolorer;
        private System.Windows.Forms.ToolStripMenuItem miExplorerRefresh;
        private System.Windows.Forms.ToolStripMenuItem miHelpAbout;
        private System.Windows.Forms.ToolStripMenuItem miServiceCreateCnls;
        private System.Windows.Forms.ToolStripMenuItem miServiceCloneCnls;
        private System.Windows.Forms.ToolStripMenuItem miDbExport;
        private System.Windows.Forms.ToolStripMenuItem miDbImport;
        private System.Windows.Forms.ToolStripSeparator miDbSep3;
        private System.Windows.Forms.ToolStripMenuItem miDbBackup;
        private System.Windows.Forms.ToolStripButton btnPassToServer;
        private System.Windows.Forms.ToolStripButton btnBackup;
        private System.Windows.Forms.ToolStripSeparator sepFourth;
        private System.Windows.Forms.ToolStripSeparator miEditSep;
        private System.Windows.Forms.ToolStripMenuItem miEditReplace;
        private System.Windows.Forms.ContextMenuStrip contextInCnls;
        private System.Windows.Forms.ToolStripMenuItem miInCnlProps;
        private System.Windows.Forms.ToolStripMenuItem miServiceCnlsMap;
        private System.Windows.Forms.ToolStripSeparator miServiceSep;
        private System.Windows.Forms.ToolStripMenuItem miServiceRestartServer;
        private System.Windows.Forms.ToolStripMenuItem miServiceRestartComm;
        private System.Windows.Forms.ToolStripButton btnRestartServer;
        private System.Windows.Forms.ToolStripButton btnRestartComm;
        private System.Windows.Forms.ToolStripSeparator sepFifth;
        private System.Windows.Forms.ToolStripMenuItem miSettingsLanguage;
        private System.Windows.Forms.ToolStripMenuItem miRemote;
        private System.Windows.Forms.ToolStripMenuItem miRemoteDownload;
        private System.Windows.Forms.ToolStripMenuItem miRemoteUpload;
        private System.Windows.Forms.ToolStripMenuItem miRemoteStatus;
    }
}

