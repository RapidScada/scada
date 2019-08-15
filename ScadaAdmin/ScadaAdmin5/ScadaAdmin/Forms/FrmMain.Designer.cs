namespace Scada.Admin.App.Forms
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
            this.msMain = new System.Windows.Forms.MenuStrip();
            this.miFile = new System.Windows.Forms.ToolStripMenuItem();
            this.miFileNewProject = new System.Windows.Forms.ToolStripMenuItem();
            this.miFileOpenProject = new System.Windows.Forms.ToolStripMenuItem();
            this.miFileShowStartPage = new System.Windows.Forms.ToolStripMenuItem();
            this.miFileSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.miFileSave = new System.Windows.Forms.ToolStripMenuItem();
            this.miFileSaveAll = new System.Windows.Forms.ToolStripMenuItem();
            this.miFileSep2 = new System.Windows.Forms.ToolStripSeparator();
            this.miFileImportTable = new System.Windows.Forms.ToolStripMenuItem();
            this.miFileExportTable = new System.Windows.Forms.ToolStripMenuItem();
            this.miFileSep3 = new System.Windows.Forms.ToolStripSeparator();
            this.miFileClose = new System.Windows.Forms.ToolStripMenuItem();
            this.miFileCloseProject = new System.Windows.Forms.ToolStripMenuItem();
            this.miFileSep4 = new System.Windows.Forms.ToolStripSeparator();
            this.miFileExit = new System.Windows.Forms.ToolStripMenuItem();
            this.miDeploy = new System.Windows.Forms.ToolStripMenuItem();
            this.miDeployInstanceProfile = new System.Windows.Forms.ToolStripMenuItem();
            this.miDeployDownloadConfig = new System.Windows.Forms.ToolStripMenuItem();
            this.miDeployUploadConfig = new System.Windows.Forms.ToolStripMenuItem();
            this.miDeployInstanceStatus = new System.Windows.Forms.ToolStripMenuItem();
            this.miTools = new System.Windows.Forms.ToolStripMenuItem();
            this.miToolsAddLine = new System.Windows.Forms.ToolStripMenuItem();
            this.miToolsAddDevice = new System.Windows.Forms.ToolStripMenuItem();
            this.miToolsCreateCnls = new System.Windows.Forms.ToolStripMenuItem();
            this.miToolsSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.miToolsCloneCnls = new System.Windows.Forms.ToolStripMenuItem();
            this.miToolsCnlMap = new System.Windows.Forms.ToolStripMenuItem();
            this.miToolsCheckIntegrity = new System.Windows.Forms.ToolStripMenuItem();
            this.miToolsSep2 = new System.Windows.Forms.ToolStripSeparator();
            this.miToolsOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.miToolsCulture = new System.Windows.Forms.ToolStripMenuItem();
            this.miWindow = new System.Windows.Forms.ToolStripMenuItem();
            this.miWindowCloseActive = new System.Windows.Forms.ToolStripMenuItem();
            this.miWindowCloseAll = new System.Windows.Forms.ToolStripMenuItem();
            this.miWindowCloseAllButActive = new System.Windows.Forms.ToolStripMenuItem();
            this.miHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.miHelpDoc = new System.Windows.Forms.ToolStripMenuItem();
            this.miHelpSupport = new System.Windows.Forms.ToolStripMenuItem();
            this.miHelpSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.miHelpAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.tsMain = new System.Windows.Forms.ToolStrip();
            this.btnFileNewProject = new System.Windows.Forms.ToolStripButton();
            this.btnFileOpenProject = new System.Windows.Forms.ToolStripButton();
            this.btnFileSave = new System.Windows.Forms.ToolStripButton();
            this.btnFileSaveAll = new System.Windows.Forms.ToolStripButton();
            this.toolSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnDeployInstanceProfile = new System.Windows.Forms.ToolStripButton();
            this.btnDeployDownloadConfig = new System.Windows.Forms.ToolStripButton();
            this.btnDeployUploadConfig = new System.Windows.Forms.ToolStripButton();
            this.btnDeployInstanceStatus = new System.Windows.Forms.ToolStripButton();
            this.toolSep2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnToolsAddLine = new System.Windows.Forms.ToolStripButton();
            this.btnToolsAddDevice = new System.Windows.Forms.ToolStripButton();
            this.btnToolsCreateCnls = new System.Windows.Forms.ToolStripButton();
            this.ssMain = new System.Windows.Forms.StatusStrip();
            this.lblSelectedInstance = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblSelectedProfile = new System.Windows.Forms.ToolStripStatusLabel();
            this.pnlLeft = new System.Windows.Forms.Panel();
            this.tvExplorer = new System.Windows.Forms.TreeView();
            this.ilExplorer = new System.Windows.Forms.ImageList(this.components);
            this.splVert = new System.Windows.Forms.Splitter();
            this.pnlRight = new System.Windows.Forms.Panel();
            this.wctrlMain = new WinControl.WinControl();
            this.ofdProject = new System.Windows.Forms.OpenFileDialog();
            this.cmsCommLine = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miCommLineImport = new System.Windows.Forms.ToolStripMenuItem();
            this.miCommLineSync = new System.Windows.Forms.ToolStripMenuItem();
            this.miCommLineSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.miCommLineAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.miCommLineMoveUp = new System.Windows.Forms.ToolStripMenuItem();
            this.miCommLineMoveDown = new System.Windows.Forms.ToolStripMenuItem();
            this.miCommLineDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.miCommLineSep2 = new System.Windows.Forms.ToolStripSeparator();
            this.miCommLineStart = new System.Windows.Forms.ToolStripMenuItem();
            this.miCommLineStop = new System.Windows.Forms.ToolStripMenuItem();
            this.miCommLineRestart = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsInstance = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miInstanceAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.miInstanceMoveUp = new System.Windows.Forms.ToolStripMenuItem();
            this.miInstanceMoveDown = new System.Windows.Forms.ToolStripMenuItem();
            this.miInstanceDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.miInstanceSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.miInstanceProfile = new System.Windows.Forms.ToolStripMenuItem();
            this.miInstanceDownloadConfig = new System.Windows.Forms.ToolStripMenuItem();
            this.miInstanceUploadConfig = new System.Windows.Forms.ToolStripMenuItem();
            this.miInstanceStatus = new System.Windows.Forms.ToolStripMenuItem();
            this.miInstanceSep2 = new System.Windows.Forms.ToolStripSeparator();
            this.miInstanceOpenInExplorer = new System.Windows.Forms.ToolStripMenuItem();
            this.miInstanceOpenInBrowser = new System.Windows.Forms.ToolStripMenuItem();
            this.miInstanceRename = new System.Windows.Forms.ToolStripMenuItem();
            this.miInstanceProperties = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsProject = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miProjectOpenInExplorer = new System.Windows.Forms.ToolStripMenuItem();
            this.miProjectRename = new System.Windows.Forms.ToolStripMenuItem();
            this.miProjectProperties = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsDirectory = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miDirectoryNewFile = new System.Windows.Forms.ToolStripMenuItem();
            this.miDirectoryNewFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.miDirectorySep1 = new System.Windows.Forms.ToolStripSeparator();
            this.miDirectoryDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.miDirectoryRename = new System.Windows.Forms.ToolStripMenuItem();
            this.miDirectorySep2 = new System.Windows.Forms.ToolStripSeparator();
            this.miDirectoryOpenInExplorer = new System.Windows.Forms.ToolStripMenuItem();
            this.miDirectoryRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsFileItem = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miFileItemOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.miFileItemOpenLocation = new System.Windows.Forms.ToolStripMenuItem();
            this.miFileItemSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.miFileItemDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.miFileItemRename = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsServer = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miServerOpenInExplorer = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsComm = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miCommOpenInExplorer = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsDevice = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miDeviceCommand = new System.Windows.Forms.ToolStripMenuItem();
            this.miDeviceProperties = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsCnlTable = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miCnlTableComm = new System.Windows.Forms.ToolStripMenuItem();
            this.miCnlTableRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.msMain.SuspendLayout();
            this.tsMain.SuspendLayout();
            this.ssMain.SuspendLayout();
            this.pnlLeft.SuspendLayout();
            this.pnlRight.SuspendLayout();
            this.cmsCommLine.SuspendLayout();
            this.cmsInstance.SuspendLayout();
            this.cmsProject.SuspendLayout();
            this.cmsDirectory.SuspendLayout();
            this.cmsFileItem.SuspendLayout();
            this.cmsServer.SuspendLayout();
            this.cmsComm.SuspendLayout();
            this.cmsDevice.SuspendLayout();
            this.cmsCnlTable.SuspendLayout();
            this.SuspendLayout();
            // 
            // msMain
            // 
            this.msMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miFile,
            this.miDeploy,
            this.miTools,
            this.miWindow,
            this.miHelp});
            this.msMain.Location = new System.Drawing.Point(0, 0);
            this.msMain.Name = "msMain";
            this.msMain.Size = new System.Drawing.Size(684, 24);
            this.msMain.TabIndex = 0;
            // 
            // miFile
            // 
            this.miFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miFileNewProject,
            this.miFileOpenProject,
            this.miFileShowStartPage,
            this.miFileSep1,
            this.miFileSave,
            this.miFileSaveAll,
            this.miFileSep2,
            this.miFileImportTable,
            this.miFileExportTable,
            this.miFileSep3,
            this.miFileClose,
            this.miFileCloseProject,
            this.miFileSep4,
            this.miFileExit});
            this.miFile.Name = "miFile";
            this.miFile.Size = new System.Drawing.Size(37, 20);
            this.miFile.Text = "&File";
            this.miFile.DropDownOpening += new System.EventHandler(this.miFile_DropDownOpening);
            // 
            // miFileNewProject
            // 
            this.miFileNewProject.Image = ((System.Drawing.Image)(resources.GetObject("miFileNewProject.Image")));
            this.miFileNewProject.Name = "miFileNewProject";
            this.miFileNewProject.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.miFileNewProject.Size = new System.Drawing.Size(195, 22);
            this.miFileNewProject.Text = "New Project...";
            this.miFileNewProject.Click += new System.EventHandler(this.miFileNewProject_Click);
            // 
            // miFileOpenProject
            // 
            this.miFileOpenProject.Image = ((System.Drawing.Image)(resources.GetObject("miFileOpenProject.Image")));
            this.miFileOpenProject.Name = "miFileOpenProject";
            this.miFileOpenProject.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.miFileOpenProject.Size = new System.Drawing.Size(195, 22);
            this.miFileOpenProject.Text = "Open Project...";
            this.miFileOpenProject.Click += new System.EventHandler(this.miFileOpenProject_Click);
            // 
            // miFileShowStartPage
            // 
            this.miFileShowStartPage.Image = ((System.Drawing.Image)(resources.GetObject("miFileShowStartPage.Image")));
            this.miFileShowStartPage.Name = "miFileShowStartPage";
            this.miFileShowStartPage.Size = new System.Drawing.Size(195, 22);
            this.miFileShowStartPage.Text = "Start Page";
            this.miFileShowStartPage.Click += new System.EventHandler(this.miFileShowStartPage_Click);
            // 
            // miFileSep1
            // 
            this.miFileSep1.Name = "miFileSep1";
            this.miFileSep1.Size = new System.Drawing.Size(192, 6);
            // 
            // miFileSave
            // 
            this.miFileSave.Image = ((System.Drawing.Image)(resources.GetObject("miFileSave.Image")));
            this.miFileSave.Name = "miFileSave";
            this.miFileSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.miFileSave.Size = new System.Drawing.Size(195, 22);
            this.miFileSave.Text = "Save";
            this.miFileSave.Click += new System.EventHandler(this.miFileSave_Click);
            // 
            // miFileSaveAll
            // 
            this.miFileSaveAll.Image = ((System.Drawing.Image)(resources.GetObject("miFileSaveAll.Image")));
            this.miFileSaveAll.Name = "miFileSaveAll";
            this.miFileSaveAll.Size = new System.Drawing.Size(195, 22);
            this.miFileSaveAll.Text = "Save All";
            this.miFileSaveAll.Click += new System.EventHandler(this.miFileSaveAll_Click);
            // 
            // miFileSep2
            // 
            this.miFileSep2.Name = "miFileSep2";
            this.miFileSep2.Size = new System.Drawing.Size(192, 6);
            // 
            // miFileImportTable
            // 
            this.miFileImportTable.Name = "miFileImportTable";
            this.miFileImportTable.Size = new System.Drawing.Size(195, 22);
            this.miFileImportTable.Text = "Import Table...";
            this.miFileImportTable.Click += new System.EventHandler(this.miFileImportTable_Click);
            // 
            // miFileExportTable
            // 
            this.miFileExportTable.Name = "miFileExportTable";
            this.miFileExportTable.Size = new System.Drawing.Size(195, 22);
            this.miFileExportTable.Text = "Export Table...";
            this.miFileExportTable.Click += new System.EventHandler(this.miFileExportTable_Click);
            // 
            // miFileSep3
            // 
            this.miFileSep3.Name = "miFileSep3";
            this.miFileSep3.Size = new System.Drawing.Size(192, 6);
            // 
            // miFileClose
            // 
            this.miFileClose.Name = "miFileClose";
            this.miFileClose.Size = new System.Drawing.Size(195, 22);
            this.miFileClose.Text = "Close";
            this.miFileClose.Click += new System.EventHandler(this.miFileClose_Click);
            // 
            // miFileCloseProject
            // 
            this.miFileCloseProject.Name = "miFileCloseProject";
            this.miFileCloseProject.Size = new System.Drawing.Size(195, 22);
            this.miFileCloseProject.Text = "Close Project";
            this.miFileCloseProject.Click += new System.EventHandler(this.miFileCloseProject_Click);
            // 
            // miFileSep4
            // 
            this.miFileSep4.Name = "miFileSep4";
            this.miFileSep4.Size = new System.Drawing.Size(192, 6);
            // 
            // miFileExit
            // 
            this.miFileExit.Image = ((System.Drawing.Image)(resources.GetObject("miFileExit.Image")));
            this.miFileExit.Name = "miFileExit";
            this.miFileExit.Size = new System.Drawing.Size(195, 22);
            this.miFileExit.Text = "Exit";
            this.miFileExit.Click += new System.EventHandler(this.miFileExit_Click);
            // 
            // miDeploy
            // 
            this.miDeploy.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miDeployInstanceProfile,
            this.miDeployDownloadConfig,
            this.miDeployUploadConfig,
            this.miDeployInstanceStatus});
            this.miDeploy.Name = "miDeploy";
            this.miDeploy.Size = new System.Drawing.Size(56, 20);
            this.miDeploy.Text = "&Deploy";
            // 
            // miDeployInstanceProfile
            // 
            this.miDeployInstanceProfile.Image = ((System.Drawing.Image)(resources.GetObject("miDeployInstanceProfile.Image")));
            this.miDeployInstanceProfile.Name = "miDeployInstanceProfile";
            this.miDeployInstanceProfile.Size = new System.Drawing.Size(214, 22);
            this.miDeployInstanceProfile.Text = "Deployment Profile...";
            this.miDeployInstanceProfile.Click += new System.EventHandler(this.miDeployInstanceProfile_Click);
            // 
            // miDeployDownloadConfig
            // 
            this.miDeployDownloadConfig.Image = ((System.Drawing.Image)(resources.GetObject("miDeployDownloadConfig.Image")));
            this.miDeployDownloadConfig.Name = "miDeployDownloadConfig";
            this.miDeployDownloadConfig.Size = new System.Drawing.Size(214, 22);
            this.miDeployDownloadConfig.Text = "Download Configuration...";
            this.miDeployDownloadConfig.Click += new System.EventHandler(this.miDeployDownloadConfig_Click);
            // 
            // miDeployUploadConfig
            // 
            this.miDeployUploadConfig.Image = ((System.Drawing.Image)(resources.GetObject("miDeployUploadConfig.Image")));
            this.miDeployUploadConfig.Name = "miDeployUploadConfig";
            this.miDeployUploadConfig.Size = new System.Drawing.Size(214, 22);
            this.miDeployUploadConfig.Text = "Upload Configuration...";
            this.miDeployUploadConfig.Click += new System.EventHandler(this.miDeployUploadConfig_Click);
            // 
            // miDeployInstanceStatus
            // 
            this.miDeployInstanceStatus.Image = ((System.Drawing.Image)(resources.GetObject("miDeployInstanceStatus.Image")));
            this.miDeployInstanceStatus.Name = "miDeployInstanceStatus";
            this.miDeployInstanceStatus.Size = new System.Drawing.Size(214, 22);
            this.miDeployInstanceStatus.Text = "Instance Status...";
            this.miDeployInstanceStatus.Click += new System.EventHandler(this.miDeployInstanceStatus_Click);
            // 
            // miTools
            // 
            this.miTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miToolsAddLine,
            this.miToolsAddDevice,
            this.miToolsCreateCnls,
            this.miToolsSep1,
            this.miToolsCloneCnls,
            this.miToolsCnlMap,
            this.miToolsCheckIntegrity,
            this.miToolsSep2,
            this.miToolsOptions,
            this.miToolsCulture});
            this.miTools.Name = "miTools";
            this.miTools.Size = new System.Drawing.Size(46, 20);
            this.miTools.Text = "&Tools";
            // 
            // miToolsAddLine
            // 
            this.miToolsAddLine.Image = ((System.Drawing.Image)(resources.GetObject("miToolsAddLine.Image")));
            this.miToolsAddLine.Name = "miToolsAddLine";
            this.miToolsAddLine.Size = new System.Drawing.Size(169, 22);
            this.miToolsAddLine.Text = "Add Line...";
            this.miToolsAddLine.Click += new System.EventHandler(this.miToolsAddLine_Click);
            // 
            // miToolsAddDevice
            // 
            this.miToolsAddDevice.Image = ((System.Drawing.Image)(resources.GetObject("miToolsAddDevice.Image")));
            this.miToolsAddDevice.Name = "miToolsAddDevice";
            this.miToolsAddDevice.Size = new System.Drawing.Size(169, 22);
            this.miToolsAddDevice.Text = "Add Device...";
            this.miToolsAddDevice.Click += new System.EventHandler(this.miToolsAddDevice_Click);
            // 
            // miToolsCreateCnls
            // 
            this.miToolsCreateCnls.Image = ((System.Drawing.Image)(resources.GetObject("miToolsCreateCnls.Image")));
            this.miToolsCreateCnls.Name = "miToolsCreateCnls";
            this.miToolsCreateCnls.Size = new System.Drawing.Size(169, 22);
            this.miToolsCreateCnls.Text = "Create Channels...";
            this.miToolsCreateCnls.Click += new System.EventHandler(this.miToolsCreateCnls_Click);
            // 
            // miToolsSep1
            // 
            this.miToolsSep1.Name = "miToolsSep1";
            this.miToolsSep1.Size = new System.Drawing.Size(166, 6);
            // 
            // miToolsCloneCnls
            // 
            this.miToolsCloneCnls.Name = "miToolsCloneCnls";
            this.miToolsCloneCnls.Size = new System.Drawing.Size(169, 22);
            this.miToolsCloneCnls.Text = "Clone Channels...";
            this.miToolsCloneCnls.Click += new System.EventHandler(this.miToolsCloneCnls_Click);
            // 
            // miToolsCnlMap
            // 
            this.miToolsCnlMap.Name = "miToolsCnlMap";
            this.miToolsCnlMap.Size = new System.Drawing.Size(169, 22);
            this.miToolsCnlMap.Text = "Channel Map...";
            this.miToolsCnlMap.Click += new System.EventHandler(this.miToolsCnlMap_Click);
            // 
            // miToolsCheckIntegrity
            // 
            this.miToolsCheckIntegrity.Name = "miToolsCheckIntegrity";
            this.miToolsCheckIntegrity.Size = new System.Drawing.Size(169, 22);
            this.miToolsCheckIntegrity.Text = "Check Integrity";
            this.miToolsCheckIntegrity.Click += new System.EventHandler(this.miToolsCheckIntegrity_Click);
            // 
            // miToolsSep2
            // 
            this.miToolsSep2.Name = "miToolsSep2";
            this.miToolsSep2.Size = new System.Drawing.Size(166, 6);
            // 
            // miToolsOptions
            // 
            this.miToolsOptions.Image = ((System.Drawing.Image)(resources.GetObject("miToolsOptions.Image")));
            this.miToolsOptions.Name = "miToolsOptions";
            this.miToolsOptions.Size = new System.Drawing.Size(169, 22);
            this.miToolsOptions.Text = "Options...";
            this.miToolsOptions.Click += new System.EventHandler(this.miToolsOptions_Click);
            // 
            // miToolsCulture
            // 
            this.miToolsCulture.Name = "miToolsCulture";
            this.miToolsCulture.Size = new System.Drawing.Size(169, 22);
            this.miToolsCulture.Text = "Language...";
            this.miToolsCulture.Click += new System.EventHandler(this.miToolsCulture_Click);
            // 
            // miWindow
            // 
            this.miWindow.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miWindowCloseActive,
            this.miWindowCloseAll,
            this.miWindowCloseAllButActive});
            this.miWindow.Name = "miWindow";
            this.miWindow.Size = new System.Drawing.Size(63, 20);
            this.miWindow.Text = "&Window";
            this.miWindow.DropDownOpening += new System.EventHandler(this.miWindow_DropDownOpening);
            // 
            // miWindowCloseActive
            // 
            this.miWindowCloseActive.Name = "miWindowCloseActive";
            this.miWindowCloseActive.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F4)));
            this.miWindowCloseActive.Size = new System.Drawing.Size(185, 22);
            this.miWindowCloseActive.Text = "Close Active";
            this.miWindowCloseActive.Click += new System.EventHandler(this.miWindowCloseActive_Click);
            // 
            // miWindowCloseAll
            // 
            this.miWindowCloseAll.Name = "miWindowCloseAll";
            this.miWindowCloseAll.Size = new System.Drawing.Size(185, 22);
            this.miWindowCloseAll.Text = "Close All";
            this.miWindowCloseAll.Click += new System.EventHandler(this.miWindowCloseAll_Click);
            // 
            // miWindowCloseAllButActive
            // 
            this.miWindowCloseAllButActive.Name = "miWindowCloseAllButActive";
            this.miWindowCloseAllButActive.Size = new System.Drawing.Size(185, 22);
            this.miWindowCloseAllButActive.Text = "Close All But Active";
            this.miWindowCloseAllButActive.Click += new System.EventHandler(this.miWindowCloseAllButActive_Click);
            // 
            // miHelp
            // 
            this.miHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miHelpDoc,
            this.miHelpSupport,
            this.miHelpSep1,
            this.miHelpAbout});
            this.miHelp.Name = "miHelp";
            this.miHelp.Size = new System.Drawing.Size(44, 20);
            this.miHelp.Text = "&Help";
            // 
            // miHelpDoc
            // 
            this.miHelpDoc.Image = ((System.Drawing.Image)(resources.GetObject("miHelpDoc.Image")));
            this.miHelpDoc.Name = "miHelpDoc";
            this.miHelpDoc.Size = new System.Drawing.Size(168, 22);
            this.miHelpDoc.Text = "Documentation";
            this.miHelpDoc.Click += new System.EventHandler(this.miHelpDoc_Click);
            // 
            // miHelpSupport
            // 
            this.miHelpSupport.Image = ((System.Drawing.Image)(resources.GetObject("miHelpSupport.Image")));
            this.miHelpSupport.Name = "miHelpSupport";
            this.miHelpSupport.Size = new System.Drawing.Size(168, 22);
            this.miHelpSupport.Text = "Technical Support";
            this.miHelpSupport.Click += new System.EventHandler(this.miHelpSupport_Click);
            // 
            // miHelpSep1
            // 
            this.miHelpSep1.Name = "miHelpSep1";
            this.miHelpSep1.Size = new System.Drawing.Size(165, 6);
            // 
            // miHelpAbout
            // 
            this.miHelpAbout.Image = ((System.Drawing.Image)(resources.GetObject("miHelpAbout.Image")));
            this.miHelpAbout.Name = "miHelpAbout";
            this.miHelpAbout.Size = new System.Drawing.Size(168, 22);
            this.miHelpAbout.Text = "About";
            this.miHelpAbout.Click += new System.EventHandler(this.miHelpAbout_Click);
            // 
            // tsMain
            // 
            this.tsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnFileNewProject,
            this.btnFileOpenProject,
            this.btnFileSave,
            this.btnFileSaveAll,
            this.toolSep1,
            this.btnDeployInstanceProfile,
            this.btnDeployDownloadConfig,
            this.btnDeployUploadConfig,
            this.btnDeployInstanceStatus,
            this.toolSep2,
            this.btnToolsAddLine,
            this.btnToolsAddDevice,
            this.btnToolsCreateCnls});
            this.tsMain.Location = new System.Drawing.Point(0, 24);
            this.tsMain.Name = "tsMain";
            this.tsMain.Size = new System.Drawing.Size(684, 25);
            this.tsMain.TabIndex = 1;
            // 
            // btnFileNewProject
            // 
            this.btnFileNewProject.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnFileNewProject.Image = ((System.Drawing.Image)(resources.GetObject("btnFileNewProject.Image")));
            this.btnFileNewProject.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnFileNewProject.Name = "btnFileNewProject";
            this.btnFileNewProject.Size = new System.Drawing.Size(23, 22);
            this.btnFileNewProject.ToolTipText = "New Project (Ctrl+N)";
            this.btnFileNewProject.Click += new System.EventHandler(this.miFileNewProject_Click);
            // 
            // btnFileOpenProject
            // 
            this.btnFileOpenProject.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnFileOpenProject.Image = ((System.Drawing.Image)(resources.GetObject("btnFileOpenProject.Image")));
            this.btnFileOpenProject.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnFileOpenProject.Name = "btnFileOpenProject";
            this.btnFileOpenProject.Size = new System.Drawing.Size(23, 22);
            this.btnFileOpenProject.ToolTipText = "Open Project (Ctrl+O)";
            this.btnFileOpenProject.Click += new System.EventHandler(this.miFileOpenProject_Click);
            // 
            // btnFileSave
            // 
            this.btnFileSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnFileSave.Image = ((System.Drawing.Image)(resources.GetObject("btnFileSave.Image")));
            this.btnFileSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnFileSave.Name = "btnFileSave";
            this.btnFileSave.Size = new System.Drawing.Size(23, 22);
            this.btnFileSave.ToolTipText = "Save (Ctrl+S)";
            this.btnFileSave.Click += new System.EventHandler(this.miFileSave_Click);
            // 
            // btnFileSaveAll
            // 
            this.btnFileSaveAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnFileSaveAll.Image = ((System.Drawing.Image)(resources.GetObject("btnFileSaveAll.Image")));
            this.btnFileSaveAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnFileSaveAll.Name = "btnFileSaveAll";
            this.btnFileSaveAll.Size = new System.Drawing.Size(23, 22);
            this.btnFileSaveAll.ToolTipText = "Save All";
            this.btnFileSaveAll.Click += new System.EventHandler(this.miFileSaveAll_Click);
            // 
            // toolSep1
            // 
            this.toolSep1.Name = "toolSep1";
            this.toolSep1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnDeployInstanceProfile
            // 
            this.btnDeployInstanceProfile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnDeployInstanceProfile.Image = ((System.Drawing.Image)(resources.GetObject("btnDeployInstanceProfile.Image")));
            this.btnDeployInstanceProfile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDeployInstanceProfile.Name = "btnDeployInstanceProfile";
            this.btnDeployInstanceProfile.Size = new System.Drawing.Size(23, 22);
            this.btnDeployInstanceProfile.ToolTipText = "Deployment Profile";
            this.btnDeployInstanceProfile.Click += new System.EventHandler(this.miDeployInstanceProfile_Click);
            // 
            // btnDeployDownloadConfig
            // 
            this.btnDeployDownloadConfig.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnDeployDownloadConfig.Image = ((System.Drawing.Image)(resources.GetObject("btnDeployDownloadConfig.Image")));
            this.btnDeployDownloadConfig.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDeployDownloadConfig.Name = "btnDeployDownloadConfig";
            this.btnDeployDownloadConfig.Size = new System.Drawing.Size(23, 22);
            this.btnDeployDownloadConfig.ToolTipText = "Download Configuration";
            this.btnDeployDownloadConfig.Click += new System.EventHandler(this.miDeployDownloadConfig_Click);
            // 
            // btnDeployUploadConfig
            // 
            this.btnDeployUploadConfig.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnDeployUploadConfig.Image = ((System.Drawing.Image)(resources.GetObject("btnDeployUploadConfig.Image")));
            this.btnDeployUploadConfig.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDeployUploadConfig.Name = "btnDeployUploadConfig";
            this.btnDeployUploadConfig.Size = new System.Drawing.Size(23, 22);
            this.btnDeployUploadConfig.ToolTipText = "Upload Configuration";
            this.btnDeployUploadConfig.Click += new System.EventHandler(this.miDeployUploadConfig_Click);
            // 
            // btnDeployInstanceStatus
            // 
            this.btnDeployInstanceStatus.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnDeployInstanceStatus.Image = ((System.Drawing.Image)(resources.GetObject("btnDeployInstanceStatus.Image")));
            this.btnDeployInstanceStatus.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDeployInstanceStatus.Name = "btnDeployInstanceStatus";
            this.btnDeployInstanceStatus.Size = new System.Drawing.Size(23, 22);
            this.btnDeployInstanceStatus.ToolTipText = "Instance Status";
            this.btnDeployInstanceStatus.Click += new System.EventHandler(this.miDeployInstanceStatus_Click);
            // 
            // toolSep2
            // 
            this.toolSep2.Name = "toolSep2";
            this.toolSep2.Size = new System.Drawing.Size(6, 25);
            // 
            // btnToolsAddLine
            // 
            this.btnToolsAddLine.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnToolsAddLine.Image = ((System.Drawing.Image)(resources.GetObject("btnToolsAddLine.Image")));
            this.btnToolsAddLine.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnToolsAddLine.Name = "btnToolsAddLine";
            this.btnToolsAddLine.Size = new System.Drawing.Size(23, 22);
            this.btnToolsAddLine.ToolTipText = "Add Line";
            this.btnToolsAddLine.Click += new System.EventHandler(this.miToolsAddLine_Click);
            // 
            // btnToolsAddDevice
            // 
            this.btnToolsAddDevice.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnToolsAddDevice.Image = ((System.Drawing.Image)(resources.GetObject("btnToolsAddDevice.Image")));
            this.btnToolsAddDevice.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnToolsAddDevice.Name = "btnToolsAddDevice";
            this.btnToolsAddDevice.Size = new System.Drawing.Size(23, 22);
            this.btnToolsAddDevice.ToolTipText = "Add Device";
            this.btnToolsAddDevice.Click += new System.EventHandler(this.miToolsAddDevice_Click);
            // 
            // btnToolsCreateCnls
            // 
            this.btnToolsCreateCnls.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnToolsCreateCnls.Image = ((System.Drawing.Image)(resources.GetObject("btnToolsCreateCnls.Image")));
            this.btnToolsCreateCnls.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnToolsCreateCnls.Name = "btnToolsCreateCnls";
            this.btnToolsCreateCnls.Size = new System.Drawing.Size(23, 22);
            this.btnToolsCreateCnls.ToolTipText = "Create Channels";
            this.btnToolsCreateCnls.Click += new System.EventHandler(this.miToolsCreateCnls_Click);
            // 
            // ssMain
            // 
            this.ssMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblSelectedInstance,
            this.lblSelectedProfile});
            this.ssMain.Location = new System.Drawing.Point(0, 387);
            this.ssMain.Name = "ssMain";
            this.ssMain.Size = new System.Drawing.Size(684, 24);
            this.ssMain.TabIndex = 2;
            // 
            // lblSelectedInstance
            // 
            this.lblSelectedInstance.Name = "lblSelectedInstance";
            this.lblSelectedInstance.Size = new System.Drawing.Size(108, 19);
            this.lblSelectedInstance.Text = "lblSelectedInstance";
            // 
            // lblSelectedProfile
            // 
            this.lblSelectedProfile.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.lblSelectedProfile.Name = "lblSelectedProfile";
            this.lblSelectedProfile.Size = new System.Drawing.Size(102, 19);
            this.lblSelectedProfile.Text = "lblSelectedProfile";
            // 
            // pnlLeft
            // 
            this.pnlLeft.Controls.Add(this.tvExplorer);
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlLeft.Location = new System.Drawing.Point(0, 49);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Size = new System.Drawing.Size(250, 338);
            this.pnlLeft.TabIndex = 3;
            // 
            // tvExplorer
            // 
            this.tvExplorer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvExplorer.HideSelection = false;
            this.tvExplorer.ImageIndex = 0;
            this.tvExplorer.ImageList = this.ilExplorer;
            this.tvExplorer.Location = new System.Drawing.Point(0, 0);
            this.tvExplorer.Name = "tvExplorer";
            this.tvExplorer.SelectedImageIndex = 0;
            this.tvExplorer.ShowRootLines = false;
            this.tvExplorer.Size = new System.Drawing.Size(250, 338);
            this.tvExplorer.TabIndex = 0;
            this.tvExplorer.BeforeCollapse += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvExplorer_BeforeCollapse);
            this.tvExplorer.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.tvExplorer_AfterCollapse);
            this.tvExplorer.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvExplorer_BeforeExpand);
            this.tvExplorer.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.tvExplorer_AfterExpand);
            this.tvExplorer.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvExplorer_AfterSelect);
            this.tvExplorer.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvExplorer_NodeMouseClick);
            this.tvExplorer.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvExplorer_NodeMouseDoubleClick);
            this.tvExplorer.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tvExplorer_KeyDown);
            this.tvExplorer.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tvExplorer_MouseDown);
            // 
            // ilExplorer
            // 
            this.ilExplorer.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.ilExplorer.ImageSize = new System.Drawing.Size(16, 16);
            this.ilExplorer.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // splVert
            // 
            this.splVert.Location = new System.Drawing.Point(250, 49);
            this.splVert.MinExtra = 100;
            this.splVert.MinSize = 100;
            this.splVert.Name = "splVert";
            this.splVert.Size = new System.Drawing.Size(3, 338);
            this.splVert.TabIndex = 4;
            this.splVert.TabStop = false;
            // 
            // pnlRight
            // 
            this.pnlRight.Controls.Add(this.wctrlMain);
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRight.Location = new System.Drawing.Point(253, 49);
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Size = new System.Drawing.Size(431, 338);
            this.pnlRight.TabIndex = 5;
            // 
            // wctrlMain
            // 
            this.wctrlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wctrlMain.Image = null;
            this.wctrlMain.Location = new System.Drawing.Point(0, 0);
            this.wctrlMain.MessageFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.wctrlMain.Name = "wctrlMain";
            this.wctrlMain.SaveReqCancel = "Cancel";
            this.wctrlMain.SaveReqCaption = "Save changes";
            this.wctrlMain.SaveReqNo = "&No";
            this.wctrlMain.SaveReqQuestion = "Save changes to the following items?";
            this.wctrlMain.SaveReqYes = "&Yes";
            this.wctrlMain.Size = new System.Drawing.Size(431, 338);
            this.wctrlMain.TabIndex = 0;
            this.wctrlMain.ActiveFormChanged += new System.EventHandler(this.wctrlMain_ActiveFormChanged);
            this.wctrlMain.ChildFormClosed += new System.EventHandler<WinControl.ChildFormClosedEventArgs>(this.wctrlMain_ChildFormClosed);
            this.wctrlMain.ChildFormMessage += new System.EventHandler<WinControl.FormMessageEventArgs>(this.wctrlMain_ChildFormMessage);
            this.wctrlMain.ChildFormModifiedChanged += new System.EventHandler<WinControl.ChildFormEventArgs>(this.wctrlMain_ChildFormModifiedChanged);
            // 
            // ofdProject
            // 
            this.ofdProject.DefaultExt = "*.rsproj";
            this.ofdProject.Filter = "Projects (*.rsproj)|*.rsproj|All Files (*.*)|*.*";
            // 
            // cmsCommLine
            // 
            this.cmsCommLine.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miCommLineImport,
            this.miCommLineSync,
            this.miCommLineSep1,
            this.miCommLineAdd,
            this.miCommLineMoveUp,
            this.miCommLineMoveDown,
            this.miCommLineDelete,
            this.miCommLineSep2,
            this.miCommLineStart,
            this.miCommLineStop,
            this.miCommLineRestart});
            this.cmsCommLine.Name = "cmsCommLine";
            this.cmsCommLine.Size = new System.Drawing.Size(164, 214);
            this.cmsCommLine.Opening += new System.ComponentModel.CancelEventHandler(this.cmsCommLine_Opening);
            // 
            // miCommLineImport
            // 
            this.miCommLineImport.Image = ((System.Drawing.Image)(resources.GetObject("miCommLineImport.Image")));
            this.miCommLineImport.Name = "miCommLineImport";
            this.miCommLineImport.Size = new System.Drawing.Size(163, 22);
            this.miCommLineImport.Text = "Import...";
            this.miCommLineImport.Click += new System.EventHandler(this.miCommLineImport_Click);
            // 
            // miCommLineSync
            // 
            this.miCommLineSync.Image = ((System.Drawing.Image)(resources.GetObject("miCommLineSync.Image")));
            this.miCommLineSync.Name = "miCommLineSync";
            this.miCommLineSync.Size = new System.Drawing.Size(163, 22);
            this.miCommLineSync.Text = "Synchronize...";
            this.miCommLineSync.Click += new System.EventHandler(this.miCommLineSync_Click);
            // 
            // miCommLineSep1
            // 
            this.miCommLineSep1.Name = "miCommLineSep1";
            this.miCommLineSep1.Size = new System.Drawing.Size(160, 6);
            // 
            // miCommLineAdd
            // 
            this.miCommLineAdd.Image = ((System.Drawing.Image)(resources.GetObject("miCommLineAdd.Image")));
            this.miCommLineAdd.Name = "miCommLineAdd";
            this.miCommLineAdd.Size = new System.Drawing.Size(163, 22);
            this.miCommLineAdd.Text = "Add Line";
            this.miCommLineAdd.Click += new System.EventHandler(this.miCommLineAdd_Click);
            // 
            // miCommLineMoveUp
            // 
            this.miCommLineMoveUp.Image = ((System.Drawing.Image)(resources.GetObject("miCommLineMoveUp.Image")));
            this.miCommLineMoveUp.Name = "miCommLineMoveUp";
            this.miCommLineMoveUp.Size = new System.Drawing.Size(163, 22);
            this.miCommLineMoveUp.Text = "Move Line Up";
            this.miCommLineMoveUp.Click += new System.EventHandler(this.miCommLineMoveUp_Click);
            // 
            // miCommLineMoveDown
            // 
            this.miCommLineMoveDown.Image = ((System.Drawing.Image)(resources.GetObject("miCommLineMoveDown.Image")));
            this.miCommLineMoveDown.Name = "miCommLineMoveDown";
            this.miCommLineMoveDown.Size = new System.Drawing.Size(163, 22);
            this.miCommLineMoveDown.Text = "Move Line Down";
            this.miCommLineMoveDown.Click += new System.EventHandler(this.miCommLineMoveDown_Click);
            // 
            // miCommLineDelete
            // 
            this.miCommLineDelete.Image = ((System.Drawing.Image)(resources.GetObject("miCommLineDelete.Image")));
            this.miCommLineDelete.Name = "miCommLineDelete";
            this.miCommLineDelete.Size = new System.Drawing.Size(163, 22);
            this.miCommLineDelete.Text = "Delete Line";
            this.miCommLineDelete.Click += new System.EventHandler(this.miCommLineDelete_Click);
            // 
            // miCommLineSep2
            // 
            this.miCommLineSep2.Name = "miCommLineSep2";
            this.miCommLineSep2.Size = new System.Drawing.Size(160, 6);
            // 
            // miCommLineStart
            // 
            this.miCommLineStart.Image = ((System.Drawing.Image)(resources.GetObject("miCommLineStart.Image")));
            this.miCommLineStart.Name = "miCommLineStart";
            this.miCommLineStart.Size = new System.Drawing.Size(163, 22);
            this.miCommLineStart.Text = "Start Line";
            this.miCommLineStart.Click += new System.EventHandler(this.miCommLineStartStop_Click);
            // 
            // miCommLineStop
            // 
            this.miCommLineStop.Image = ((System.Drawing.Image)(resources.GetObject("miCommLineStop.Image")));
            this.miCommLineStop.Name = "miCommLineStop";
            this.miCommLineStop.Size = new System.Drawing.Size(163, 22);
            this.miCommLineStop.Text = "Stop Line";
            this.miCommLineStop.Click += new System.EventHandler(this.miCommLineStartStop_Click);
            // 
            // miCommLineRestart
            // 
            this.miCommLineRestart.Image = ((System.Drawing.Image)(resources.GetObject("miCommLineRestart.Image")));
            this.miCommLineRestart.Name = "miCommLineRestart";
            this.miCommLineRestart.Size = new System.Drawing.Size(163, 22);
            this.miCommLineRestart.Text = "Restart Line";
            this.miCommLineRestart.Click += new System.EventHandler(this.miCommLineStartStop_Click);
            // 
            // cmsInstance
            // 
            this.cmsInstance.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miInstanceAdd,
            this.miInstanceMoveUp,
            this.miInstanceMoveDown,
            this.miInstanceDelete,
            this.miInstanceSep1,
            this.miInstanceProfile,
            this.miInstanceDownloadConfig,
            this.miInstanceUploadConfig,
            this.miInstanceStatus,
            this.miInstanceSep2,
            this.miInstanceOpenInExplorer,
            this.miInstanceOpenInBrowser,
            this.miInstanceRename,
            this.miInstanceProperties});
            this.cmsInstance.Name = "cmsCommLine";
            this.cmsInstance.Size = new System.Drawing.Size(220, 280);
            this.cmsInstance.Opening += new System.ComponentModel.CancelEventHandler(this.cmsInstance_Opening);
            // 
            // miInstanceAdd
            // 
            this.miInstanceAdd.Image = ((System.Drawing.Image)(resources.GetObject("miInstanceAdd.Image")));
            this.miInstanceAdd.Name = "miInstanceAdd";
            this.miInstanceAdd.Size = new System.Drawing.Size(219, 22);
            this.miInstanceAdd.Text = "Add Instance...";
            this.miInstanceAdd.Click += new System.EventHandler(this.miInstanceAdd_Click);
            // 
            // miInstanceMoveUp
            // 
            this.miInstanceMoveUp.Image = ((System.Drawing.Image)(resources.GetObject("miInstanceMoveUp.Image")));
            this.miInstanceMoveUp.Name = "miInstanceMoveUp";
            this.miInstanceMoveUp.Size = new System.Drawing.Size(219, 22);
            this.miInstanceMoveUp.Text = "Move Instance Up";
            this.miInstanceMoveUp.Click += new System.EventHandler(this.miInstanceMoveUp_Click);
            // 
            // miInstanceMoveDown
            // 
            this.miInstanceMoveDown.Image = ((System.Drawing.Image)(resources.GetObject("miInstanceMoveDown.Image")));
            this.miInstanceMoveDown.Name = "miInstanceMoveDown";
            this.miInstanceMoveDown.Size = new System.Drawing.Size(219, 22);
            this.miInstanceMoveDown.Text = "Move Instance Down";
            this.miInstanceMoveDown.Click += new System.EventHandler(this.miInstanceMoveDown_Click);
            // 
            // miInstanceDelete
            // 
            this.miInstanceDelete.Image = ((System.Drawing.Image)(resources.GetObject("miInstanceDelete.Image")));
            this.miInstanceDelete.Name = "miInstanceDelete";
            this.miInstanceDelete.Size = new System.Drawing.Size(219, 22);
            this.miInstanceDelete.Text = "Delete Instance";
            this.miInstanceDelete.Click += new System.EventHandler(this.miInstanceDelete_Click);
            // 
            // miInstanceSep1
            // 
            this.miInstanceSep1.Name = "miInstanceSep1";
            this.miInstanceSep1.Size = new System.Drawing.Size(216, 6);
            // 
            // miInstanceProfile
            // 
            this.miInstanceProfile.Image = ((System.Drawing.Image)(resources.GetObject("miInstanceProfile.Image")));
            this.miInstanceProfile.Name = "miInstanceProfile";
            this.miInstanceProfile.Size = new System.Drawing.Size(219, 22);
            this.miInstanceProfile.Text = "Deployment Profile...";
            this.miInstanceProfile.Click += new System.EventHandler(this.miDeployInstanceProfile_Click);
            // 
            // miInstanceDownloadConfig
            // 
            this.miInstanceDownloadConfig.Image = ((System.Drawing.Image)(resources.GetObject("miInstanceDownloadConfig.Image")));
            this.miInstanceDownloadConfig.Name = "miInstanceDownloadConfig";
            this.miInstanceDownloadConfig.Size = new System.Drawing.Size(219, 22);
            this.miInstanceDownloadConfig.Text = "Download Configuration...";
            this.miInstanceDownloadConfig.Click += new System.EventHandler(this.miDeployDownloadConfig_Click);
            // 
            // miInstanceUploadConfig
            // 
            this.miInstanceUploadConfig.Image = ((System.Drawing.Image)(resources.GetObject("miInstanceUploadConfig.Image")));
            this.miInstanceUploadConfig.Name = "miInstanceUploadConfig";
            this.miInstanceUploadConfig.Size = new System.Drawing.Size(219, 22);
            this.miInstanceUploadConfig.Text = "Upload Configuration...";
            this.miInstanceUploadConfig.Click += new System.EventHandler(this.miDeployUploadConfig_Click);
            // 
            // miInstanceStatus
            // 
            this.miInstanceStatus.Image = ((System.Drawing.Image)(resources.GetObject("miInstanceStatus.Image")));
            this.miInstanceStatus.Name = "miInstanceStatus";
            this.miInstanceStatus.Size = new System.Drawing.Size(219, 22);
            this.miInstanceStatus.Text = "Instance Status...";
            this.miInstanceStatus.Click += new System.EventHandler(this.miDeployInstanceStatus_Click);
            // 
            // miInstanceSep2
            // 
            this.miInstanceSep2.Name = "miInstanceSep2";
            this.miInstanceSep2.Size = new System.Drawing.Size(216, 6);
            // 
            // miInstanceOpenInExplorer
            // 
            this.miInstanceOpenInExplorer.Image = ((System.Drawing.Image)(resources.GetObject("miInstanceOpenInExplorer.Image")));
            this.miInstanceOpenInExplorer.Name = "miInstanceOpenInExplorer";
            this.miInstanceOpenInExplorer.Size = new System.Drawing.Size(219, 22);
            this.miInstanceOpenInExplorer.Text = "Open Folder in File Explorer";
            this.miInstanceOpenInExplorer.Click += new System.EventHandler(this.miDirectoryOpenInExplorer_Click);
            // 
            // miInstanceOpenInBrowser
            // 
            this.miInstanceOpenInBrowser.Image = ((System.Drawing.Image)(resources.GetObject("miInstanceOpenInBrowser.Image")));
            this.miInstanceOpenInBrowser.Name = "miInstanceOpenInBrowser";
            this.miInstanceOpenInBrowser.Size = new System.Drawing.Size(219, 22);
            this.miInstanceOpenInBrowser.Text = "Open in Web Browser";
            this.miInstanceOpenInBrowser.Click += new System.EventHandler(this.miInstanceOpenInBrowser_Click);
            // 
            // miInstanceRename
            // 
            this.miInstanceRename.Image = ((System.Drawing.Image)(resources.GetObject("miInstanceRename.Image")));
            this.miInstanceRename.Name = "miInstanceRename";
            this.miInstanceRename.Size = new System.Drawing.Size(219, 22);
            this.miInstanceRename.Text = "Rename Instance";
            this.miInstanceRename.Click += new System.EventHandler(this.miInstanceRename_Click);
            // 
            // miInstanceProperties
            // 
            this.miInstanceProperties.Image = ((System.Drawing.Image)(resources.GetObject("miInstanceProperties.Image")));
            this.miInstanceProperties.Name = "miInstanceProperties";
            this.miInstanceProperties.Size = new System.Drawing.Size(219, 22);
            this.miInstanceProperties.Text = "Properties";
            this.miInstanceProperties.Click += new System.EventHandler(this.miInstanceProperties_Click);
            // 
            // cmsProject
            // 
            this.cmsProject.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miProjectOpenInExplorer,
            this.miProjectRename,
            this.miProjectProperties});
            this.cmsProject.Name = "cmsCommLine";
            this.cmsProject.Size = new System.Drawing.Size(220, 70);
            // 
            // miProjectOpenInExplorer
            // 
            this.miProjectOpenInExplorer.Image = ((System.Drawing.Image)(resources.GetObject("miProjectOpenInExplorer.Image")));
            this.miProjectOpenInExplorer.Name = "miProjectOpenInExplorer";
            this.miProjectOpenInExplorer.Size = new System.Drawing.Size(219, 22);
            this.miProjectOpenInExplorer.Text = "Open Folder in File Explorer";
            this.miProjectOpenInExplorer.Click += new System.EventHandler(this.miDirectoryOpenInExplorer_Click);
            // 
            // miProjectRename
            // 
            this.miProjectRename.Image = ((System.Drawing.Image)(resources.GetObject("miProjectRename.Image")));
            this.miProjectRename.Name = "miProjectRename";
            this.miProjectRename.Size = new System.Drawing.Size(219, 22);
            this.miProjectRename.Text = "Rename Project";
            this.miProjectRename.Click += new System.EventHandler(this.miProjectRename_Click);
            // 
            // miProjectProperties
            // 
            this.miProjectProperties.Image = ((System.Drawing.Image)(resources.GetObject("miProjectProperties.Image")));
            this.miProjectProperties.Name = "miProjectProperties";
            this.miProjectProperties.Size = new System.Drawing.Size(219, 22);
            this.miProjectProperties.Text = "Properties";
            this.miProjectProperties.Click += new System.EventHandler(this.miProjectProperties_Click);
            // 
            // cmsDirectory
            // 
            this.cmsDirectory.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miDirectoryNewFile,
            this.miDirectoryNewFolder,
            this.miDirectorySep1,
            this.miDirectoryDelete,
            this.miDirectoryRename,
            this.miDirectorySep2,
            this.miDirectoryOpenInExplorer,
            this.miDirectoryRefresh});
            this.cmsDirectory.Name = "cmsDirectory";
            this.cmsDirectory.Size = new System.Drawing.Size(220, 148);
            this.cmsDirectory.Opening += new System.ComponentModel.CancelEventHandler(this.cmsDirectory_Opening);
            // 
            // miDirectoryNewFile
            // 
            this.miDirectoryNewFile.Image = ((System.Drawing.Image)(resources.GetObject("miDirectoryNewFile.Image")));
            this.miDirectoryNewFile.Name = "miDirectoryNewFile";
            this.miDirectoryNewFile.Size = new System.Drawing.Size(219, 22);
            this.miDirectoryNewFile.Text = "New File...";
            this.miDirectoryNewFile.Click += new System.EventHandler(this.miDirectoryNewFile_Click);
            // 
            // miDirectoryNewFolder
            // 
            this.miDirectoryNewFolder.Image = ((System.Drawing.Image)(resources.GetObject("miDirectoryNewFolder.Image")));
            this.miDirectoryNewFolder.Name = "miDirectoryNewFolder";
            this.miDirectoryNewFolder.Size = new System.Drawing.Size(219, 22);
            this.miDirectoryNewFolder.Text = "New Folder...";
            this.miDirectoryNewFolder.Click += new System.EventHandler(this.miDirectoryNewFolder_Click);
            // 
            // miDirectorySep1
            // 
            this.miDirectorySep1.Name = "miDirectorySep1";
            this.miDirectorySep1.Size = new System.Drawing.Size(216, 6);
            // 
            // miDirectoryDelete
            // 
            this.miDirectoryDelete.Image = ((System.Drawing.Image)(resources.GetObject("miDirectoryDelete.Image")));
            this.miDirectoryDelete.Name = "miDirectoryDelete";
            this.miDirectoryDelete.Size = new System.Drawing.Size(219, 22);
            this.miDirectoryDelete.Text = "Delete";
            this.miDirectoryDelete.Click += new System.EventHandler(this.miDirectoryDelete_Click);
            // 
            // miDirectoryRename
            // 
            this.miDirectoryRename.Image = ((System.Drawing.Image)(resources.GetObject("miDirectoryRename.Image")));
            this.miDirectoryRename.Name = "miDirectoryRename";
            this.miDirectoryRename.Size = new System.Drawing.Size(219, 22);
            this.miDirectoryRename.Text = "Rename";
            this.miDirectoryRename.Click += new System.EventHandler(this.miDirectoryRename_Click);
            // 
            // miDirectorySep2
            // 
            this.miDirectorySep2.Name = "miDirectorySep2";
            this.miDirectorySep2.Size = new System.Drawing.Size(216, 6);
            // 
            // miDirectoryOpenInExplorer
            // 
            this.miDirectoryOpenInExplorer.Image = ((System.Drawing.Image)(resources.GetObject("miDirectoryOpenInExplorer.Image")));
            this.miDirectoryOpenInExplorer.Name = "miDirectoryOpenInExplorer";
            this.miDirectoryOpenInExplorer.Size = new System.Drawing.Size(219, 22);
            this.miDirectoryOpenInExplorer.Text = "Open Folder in File Explorer";
            this.miDirectoryOpenInExplorer.Click += new System.EventHandler(this.miDirectoryOpenInExplorer_Click);
            // 
            // miDirectoryRefresh
            // 
            this.miDirectoryRefresh.Image = ((System.Drawing.Image)(resources.GetObject("miDirectoryRefresh.Image")));
            this.miDirectoryRefresh.Name = "miDirectoryRefresh";
            this.miDirectoryRefresh.Size = new System.Drawing.Size(219, 22);
            this.miDirectoryRefresh.Text = "Refresh";
            this.miDirectoryRefresh.Click += new System.EventHandler(this.miDirectoryRefresh_Click);
            // 
            // cmsFileItem
            // 
            this.cmsFileItem.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miFileItemOpen,
            this.miFileItemOpenLocation,
            this.miFileItemSep1,
            this.miFileItemDelete,
            this.miFileItemRename});
            this.cmsFileItem.Name = "cmsFileItem";
            this.cmsFileItem.Size = new System.Drawing.Size(233, 98);
            this.cmsFileItem.Opening += new System.ComponentModel.CancelEventHandler(this.cmsFileItem_Opening);
            // 
            // miFileItemOpen
            // 
            this.miFileItemOpen.Image = ((System.Drawing.Image)(resources.GetObject("miFileItemOpen.Image")));
            this.miFileItemOpen.Name = "miFileItemOpen";
            this.miFileItemOpen.Size = new System.Drawing.Size(232, 22);
            this.miFileItemOpen.Text = "Open";
            this.miFileItemOpen.Click += new System.EventHandler(this.miFileItemOpen_Click);
            // 
            // miFileItemOpenLocation
            // 
            this.miFileItemOpenLocation.Image = ((System.Drawing.Image)(resources.GetObject("miFileItemOpenLocation.Image")));
            this.miFileItemOpenLocation.Name = "miFileItemOpenLocation";
            this.miFileItemOpenLocation.Size = new System.Drawing.Size(232, 22);
            this.miFileItemOpenLocation.Text = "Open Location in File Explorer";
            this.miFileItemOpenLocation.Click += new System.EventHandler(this.miFileItemOpenLocation_Click);
            // 
            // miFileItemSep1
            // 
            this.miFileItemSep1.Name = "miFileItemSep1";
            this.miFileItemSep1.Size = new System.Drawing.Size(229, 6);
            // 
            // miFileItemDelete
            // 
            this.miFileItemDelete.Image = ((System.Drawing.Image)(resources.GetObject("miFileItemDelete.Image")));
            this.miFileItemDelete.Name = "miFileItemDelete";
            this.miFileItemDelete.Size = new System.Drawing.Size(232, 22);
            this.miFileItemDelete.Text = "Delete";
            this.miFileItemDelete.Click += new System.EventHandler(this.miFileItemDelete_Click);
            // 
            // miFileItemRename
            // 
            this.miFileItemRename.Image = ((System.Drawing.Image)(resources.GetObject("miFileItemRename.Image")));
            this.miFileItemRename.Name = "miFileItemRename";
            this.miFileItemRename.Size = new System.Drawing.Size(232, 22);
            this.miFileItemRename.Text = "Rename";
            this.miFileItemRename.Click += new System.EventHandler(this.miFileItemRename_Click);
            // 
            // cmsServer
            // 
            this.cmsServer.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miServerOpenInExplorer});
            this.cmsServer.Name = "cmsServer";
            this.cmsServer.Size = new System.Drawing.Size(220, 26);
            // 
            // miServerOpenInExplorer
            // 
            this.miServerOpenInExplorer.Image = ((System.Drawing.Image)(resources.GetObject("miServerOpenInExplorer.Image")));
            this.miServerOpenInExplorer.Name = "miServerOpenInExplorer";
            this.miServerOpenInExplorer.Size = new System.Drawing.Size(219, 22);
            this.miServerOpenInExplorer.Text = "Open Folder in File Explorer";
            this.miServerOpenInExplorer.Click += new System.EventHandler(this.miDirectoryOpenInExplorer_Click);
            // 
            // cmsComm
            // 
            this.cmsComm.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miCommOpenInExplorer});
            this.cmsComm.Name = "cmsServer";
            this.cmsComm.Size = new System.Drawing.Size(220, 26);
            // 
            // miCommOpenInExplorer
            // 
            this.miCommOpenInExplorer.Image = ((System.Drawing.Image)(resources.GetObject("miCommOpenInExplorer.Image")));
            this.miCommOpenInExplorer.Name = "miCommOpenInExplorer";
            this.miCommOpenInExplorer.Size = new System.Drawing.Size(219, 22);
            this.miCommOpenInExplorer.Text = "Open Folder in File Explorer";
            this.miCommOpenInExplorer.Click += new System.EventHandler(this.miDirectoryOpenInExplorer_Click);
            // 
            // cmsDevice
            // 
            this.cmsDevice.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miDeviceCommand,
            this.miDeviceProperties});
            this.cmsDevice.Name = "cmsDevice";
            this.cmsDevice.Size = new System.Drawing.Size(170, 48);
            this.cmsDevice.Opening += new System.ComponentModel.CancelEventHandler(this.cmsDevice_Opening);
            // 
            // miDeviceCommand
            // 
            this.miDeviceCommand.Image = ((System.Drawing.Image)(resources.GetObject("miDeviceCommand.Image")));
            this.miDeviceCommand.Name = "miDeviceCommand";
            this.miDeviceCommand.Size = new System.Drawing.Size(169, 22);
            this.miDeviceCommand.Text = "Send Command...";
            this.miDeviceCommand.Click += new System.EventHandler(this.miDeviceCommand_Click);
            // 
            // miDeviceProperties
            // 
            this.miDeviceProperties.Image = ((System.Drawing.Image)(resources.GetObject("miDeviceProperties.Image")));
            this.miDeviceProperties.Name = "miDeviceProperties";
            this.miDeviceProperties.Size = new System.Drawing.Size(169, 22);
            this.miDeviceProperties.Text = "Properies";
            this.miDeviceProperties.Click += new System.EventHandler(this.miDeviceProperties_Click);
            // 
            // cmsCnlTable
            // 
            this.cmsCnlTable.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miCnlTableComm,
            this.miCnlTableRefresh});
            this.cmsCnlTable.Name = "cmsChannels";
            this.cmsCnlTable.Size = new System.Drawing.Size(188, 48);
            this.cmsCnlTable.Opening += new System.ComponentModel.CancelEventHandler(this.cmsCnlTable_Opening);
            // 
            // miCnlTableComm
            // 
            this.miCnlTableComm.Image = ((System.Drawing.Image)(resources.GetObject("miCnlTableComm.Image")));
            this.miCnlTableComm.Name = "miCnlTableComm";
            this.miCnlTableComm.Size = new System.Drawing.Size(187, 22);
            this.miCnlTableComm.Text = "Go to Communicator";
            this.miCnlTableComm.Click += new System.EventHandler(this.miCnlTableComm_Click);
            // 
            // miCnlTableRefresh
            // 
            this.miCnlTableRefresh.Image = ((System.Drawing.Image)(resources.GetObject("miCnlTableRefresh.Image")));
            this.miCnlTableRefresh.Name = "miCnlTableRefresh";
            this.miCnlTableRefresh.Size = new System.Drawing.Size(187, 22);
            this.miCnlTableRefresh.Text = "Refresh";
            this.miCnlTableRefresh.Click += new System.EventHandler(this.miCnlTableRefresh_Click);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 411);
            this.Controls.Add(this.pnlRight);
            this.Controls.Add(this.splVert);
            this.Controls.Add(this.pnlLeft);
            this.Controls.Add(this.ssMain);
            this.Controls.Add(this.tsMain);
            this.Controls.Add(this.msMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.msMain;
            this.MinimumSize = new System.Drawing.Size(300, 200);
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Administrator";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMain_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmMain_FormClosed);
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.msMain.ResumeLayout(false);
            this.msMain.PerformLayout();
            this.tsMain.ResumeLayout(false);
            this.tsMain.PerformLayout();
            this.ssMain.ResumeLayout(false);
            this.ssMain.PerformLayout();
            this.pnlLeft.ResumeLayout(false);
            this.pnlRight.ResumeLayout(false);
            this.cmsCommLine.ResumeLayout(false);
            this.cmsInstance.ResumeLayout(false);
            this.cmsProject.ResumeLayout(false);
            this.cmsDirectory.ResumeLayout(false);
            this.cmsFileItem.ResumeLayout(false);
            this.cmsServer.ResumeLayout(false);
            this.cmsComm.ResumeLayout(false);
            this.cmsDevice.ResumeLayout(false);
            this.cmsCnlTable.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip msMain;
        private System.Windows.Forms.ToolStrip tsMain;
        private System.Windows.Forms.StatusStrip ssMain;
        private System.Windows.Forms.Panel pnlLeft;
        private System.Windows.Forms.Splitter splVert;
        private System.Windows.Forms.Panel pnlRight;
        private System.Windows.Forms.ToolStripMenuItem miFile;
        private System.Windows.Forms.ToolStripMenuItem miFileNewProject;
        private System.Windows.Forms.ToolStripMenuItem miFileOpenProject;
        private System.Windows.Forms.ToolStripMenuItem miFileSave;
        private System.Windows.Forms.ToolStripMenuItem miFileSaveAll;
        private System.Windows.Forms.ToolStripSeparator miFileSep1;
        private System.Windows.Forms.ToolStripMenuItem miFileExit;
        private System.Windows.Forms.ToolStripMenuItem miTools;
        private System.Windows.Forms.ToolStripMenuItem miToolsOptions;
        private System.Windows.Forms.ToolStripMenuItem miHelp;
        private System.Windows.Forms.ToolStripMenuItem miHelpAbout;
        private System.Windows.Forms.ToolStripMenuItem miHelpDoc;
        private System.Windows.Forms.ToolStripMenuItem miHelpSupport;
        private System.Windows.Forms.ToolStripSeparator miHelpSep1;
        private System.Windows.Forms.ToolStripButton btnFileNewProject;
        private System.Windows.Forms.TreeView tvExplorer;
        private WinControl.WinControl wctrlMain;
        private System.Windows.Forms.ImageList ilExplorer;
        private System.Windows.Forms.OpenFileDialog ofdProject;
        private System.Windows.Forms.ContextMenuStrip cmsCommLine;
        private System.Windows.Forms.ToolStripMenuItem miCommLineAdd;
        private System.Windows.Forms.ToolStripMenuItem miCommLineMoveUp;
        private System.Windows.Forms.ToolStripMenuItem miCommLineMoveDown;
        private System.Windows.Forms.ToolStripMenuItem miCommLineDelete;
        private System.Windows.Forms.ContextMenuStrip cmsInstance;
        private System.Windows.Forms.ToolStripMenuItem miInstanceAdd;
        private System.Windows.Forms.ToolStripMenuItem miInstanceMoveUp;
        private System.Windows.Forms.ToolStripMenuItem miInstanceMoveDown;
        private System.Windows.Forms.ToolStripMenuItem miInstanceDelete;
        private System.Windows.Forms.ToolStripSeparator miInstanceSep1;
        private System.Windows.Forms.ToolStripMenuItem miInstanceRename;
        private System.Windows.Forms.ToolStripMenuItem miInstanceProperties;
        private System.Windows.Forms.ContextMenuStrip cmsProject;
        private System.Windows.Forms.ToolStripMenuItem miProjectRename;
        private System.Windows.Forms.ToolStripMenuItem miProjectProperties;
        private System.Windows.Forms.ContextMenuStrip cmsDirectory;
        private System.Windows.Forms.ToolStripMenuItem miDirectoryNewFile;
        private System.Windows.Forms.ToolStripMenuItem miDirectoryNewFolder;
        private System.Windows.Forms.ToolStripSeparator miDirectorySep1;
        private System.Windows.Forms.ToolStripMenuItem miDirectoryDelete;
        private System.Windows.Forms.ToolStripMenuItem miDirectoryRename;
        private System.Windows.Forms.ToolStripSeparator miDirectorySep2;
        private System.Windows.Forms.ToolStripMenuItem miDirectoryOpenInExplorer;
        private System.Windows.Forms.ContextMenuStrip cmsFileItem;
        private System.Windows.Forms.ToolStripMenuItem miFileItemOpen;
        private System.Windows.Forms.ToolStripMenuItem miFileItemOpenLocation;
        private System.Windows.Forms.ToolStripSeparator miFileItemSep1;
        private System.Windows.Forms.ToolStripMenuItem miFileItemDelete;
        private System.Windows.Forms.ToolStripMenuItem miFileItemRename;
        private System.Windows.Forms.ToolStripMenuItem miDirectoryRefresh;
        private System.Windows.Forms.ToolStripMenuItem miFileClose;
        private System.Windows.Forms.ToolStripMenuItem miFileCloseProject;
        private System.Windows.Forms.ToolStripSeparator miFileSep2;
        private System.Windows.Forms.ToolStripButton btnFileOpenProject;
        private System.Windows.Forms.ToolStripButton btnFileSave;
        private System.Windows.Forms.ToolStripButton btnFileSaveAll;
        private System.Windows.Forms.ToolStripSeparator toolSep1;
        private System.Windows.Forms.ToolStripMenuItem miDeploy;
        private System.Windows.Forms.ToolStripMenuItem miDeployDownloadConfig;
        private System.Windows.Forms.ToolStripMenuItem miDeployUploadConfig;
        private System.Windows.Forms.ToolStripMenuItem miDeployInstanceStatus;
        private System.Windows.Forms.ToolStripSeparator miInstanceSep2;
        private System.Windows.Forms.ToolStripMenuItem miInstanceDownloadConfig;
        private System.Windows.Forms.ToolStripMenuItem miInstanceUploadConfig;
        private System.Windows.Forms.ToolStripMenuItem miInstanceStatus;
        private System.Windows.Forms.ToolStripMenuItem miInstanceProfile;
        private System.Windows.Forms.ToolStripMenuItem miDeployInstanceProfile;
        private System.Windows.Forms.ToolStripMenuItem miProjectOpenInExplorer;
        private System.Windows.Forms.ToolStripMenuItem miInstanceOpenInExplorer;
        private System.Windows.Forms.ContextMenuStrip cmsServer;
        private System.Windows.Forms.ToolStripMenuItem miServerOpenInExplorer;
        private System.Windows.Forms.ContextMenuStrip cmsComm;
        private System.Windows.Forms.ToolStripMenuItem miCommOpenInExplorer;
        private System.Windows.Forms.ToolStripSeparator miCommLineSep2;
        private System.Windows.Forms.ToolStripMenuItem miCommLineStart;
        private System.Windows.Forms.ToolStripMenuItem miCommLineStop;
        private System.Windows.Forms.ToolStripMenuItem miCommLineRestart;
        private System.Windows.Forms.ContextMenuStrip cmsDevice;
        private System.Windows.Forms.ToolStripMenuItem miDeviceCommand;
        private System.Windows.Forms.ToolStripMenuItem miDeviceProperties;
        private System.Windows.Forms.ToolStripMenuItem miToolsCulture;
        private System.Windows.Forms.ToolStripMenuItem miToolsCnlMap;
        private System.Windows.Forms.ToolStripSeparator miToolsSep1;
        private System.Windows.Forms.ToolStripMenuItem miWindow;
        private System.Windows.Forms.ToolStripMenuItem miWindowCloseActive;
        private System.Windows.Forms.ToolStripMenuItem miWindowCloseAll;
        private System.Windows.Forms.ToolStripMenuItem miWindowCloseAllButActive;
        private System.Windows.Forms.ToolStripMenuItem miToolsCheckIntegrity;
        private System.Windows.Forms.ToolStripMenuItem miFileImportTable;
        private System.Windows.Forms.ToolStripMenuItem miFileExportTable;
        private System.Windows.Forms.ToolStripSeparator miFileSep3;
        private System.Windows.Forms.ToolStripMenuItem miToolsCloneCnls;
        private System.Windows.Forms.ToolStripMenuItem miCommLineImport;
        private System.Windows.Forms.ToolStripMenuItem miCommLineSync;
        private System.Windows.Forms.ToolStripSeparator miCommLineSep1;
        private System.Windows.Forms.ToolStripMenuItem miFileShowStartPage;
        private System.Windows.Forms.ToolStripSeparator miFileSep4;
        private System.Windows.Forms.ToolStripButton btnDeployInstanceProfile;
        private System.Windows.Forms.ToolStripButton btnDeployDownloadConfig;
        private System.Windows.Forms.ToolStripButton btnDeployUploadConfig;
        private System.Windows.Forms.ToolStripButton btnDeployInstanceStatus;
        private System.Windows.Forms.ToolStripSeparator toolSep2;
        private System.Windows.Forms.ToolStripMenuItem miToolsAddLine;
        private System.Windows.Forms.ToolStripMenuItem miToolsAddDevice;
        private System.Windows.Forms.ToolStripMenuItem miToolsCreateCnls;
        private System.Windows.Forms.ToolStripSeparator miToolsSep2;
        private System.Windows.Forms.ToolStripButton btnToolsAddLine;
        private System.Windows.Forms.ToolStripButton btnToolsAddDevice;
        private System.Windows.Forms.ToolStripButton btnToolsCreateCnls;
        private System.Windows.Forms.ContextMenuStrip cmsCnlTable;
        private System.Windows.Forms.ToolStripMenuItem miCnlTableRefresh;
        private System.Windows.Forms.ToolStripMenuItem miCnlTableComm;
        private System.Windows.Forms.ToolStripMenuItem miInstanceOpenInBrowser;
        private System.Windows.Forms.ToolStripStatusLabel lblSelectedInstance;
        private System.Windows.Forms.ToolStripStatusLabel lblSelectedProfile;
    }
}

