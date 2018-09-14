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
            this.miFileSave = new System.Windows.Forms.ToolStripMenuItem();
            this.miFileSaveAll = new System.Windows.Forms.ToolStripMenuItem();
            this.miFileSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.miFileClose = new System.Windows.Forms.ToolStripMenuItem();
            this.miFileCloseProject = new System.Windows.Forms.ToolStripMenuItem();
            this.miFileSep2 = new System.Windows.Forms.ToolStripSeparator();
            this.miFileExit = new System.Windows.Forms.ToolStripMenuItem();
            this.miEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.miEditCut = new System.Windows.Forms.ToolStripMenuItem();
            this.miEditCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.miEditPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.miTools = new System.Windows.Forms.ToolStripMenuItem();
            this.miToolsOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.miHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.miHelpDoc = new System.Windows.Forms.ToolStripMenuItem();
            this.miHelpSupport = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.miHelpAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.tsMain = new System.Windows.Forms.ToolStrip();
            this.btnFileNewProject = new System.Windows.Forms.ToolStripButton();
            this.btnFileOpenProject = new System.Windows.Forms.ToolStripButton();
            this.btnFileSave = new System.Windows.Forms.ToolStripButton();
            this.btnFileSaveAll = new System.Windows.Forms.ToolStripButton();
            this.toolSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnEditCut = new System.Windows.Forms.ToolStripButton();
            this.btnEditCopy = new System.Windows.Forms.ToolStripButton();
            this.btnEditPaste = new System.Windows.Forms.ToolStripButton();
            this.ssMain = new System.Windows.Forms.StatusStrip();
            this.pnlLeft = new System.Windows.Forms.Panel();
            this.tvExplorer = new System.Windows.Forms.TreeView();
            this.ilExplorer = new System.Windows.Forms.ImageList(this.components);
            this.splVert = new System.Windows.Forms.Splitter();
            this.pnlRight = new System.Windows.Forms.Panel();
            this.wctrlMain = new WinControl.WinControl();
            this.ofdProject = new System.Windows.Forms.OpenFileDialog();
            this.cmsCommLine = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miCommLineAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.miCommLineMoveUp = new System.Windows.Forms.ToolStripMenuItem();
            this.miCommLineMoveDown = new System.Windows.Forms.ToolStripMenuItem();
            this.miCommLineDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsInstance = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miInstanceAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.miInstanceMoveUp = new System.Windows.Forms.ToolStripMenuItem();
            this.miInstanceMoveDown = new System.Windows.Forms.ToolStripMenuItem();
            this.miInstanceDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.miInstanceSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.miInstanceRename = new System.Windows.Forms.ToolStripMenuItem();
            this.miInstanceProperties = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsProject = new System.Windows.Forms.ContextMenuStrip(this.components);
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
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.miFileItemDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.miFileItemRename = new System.Windows.Forms.ToolStripMenuItem();
            this.miDeploy = new System.Windows.Forms.ToolStripMenuItem();
            this.miDeployDownloadConfig = new System.Windows.Forms.ToolStripMenuItem();
            this.miDeployUploadConfig = new System.Windows.Forms.ToolStripMenuItem();
            this.miDeployInstanceStatus = new System.Windows.Forms.ToolStripMenuItem();
            this.miInstanceSep2 = new System.Windows.Forms.ToolStripSeparator();
            this.miInstanceDownloadConfig = new System.Windows.Forms.ToolStripMenuItem();
            this.miInstanceUploadConfig = new System.Windows.Forms.ToolStripMenuItem();
            this.miInstanceStatus = new System.Windows.Forms.ToolStripMenuItem();
            this.msMain.SuspendLayout();
            this.tsMain.SuspendLayout();
            this.pnlLeft.SuspendLayout();
            this.pnlRight.SuspendLayout();
            this.cmsCommLine.SuspendLayout();
            this.cmsInstance.SuspendLayout();
            this.cmsProject.SuspendLayout();
            this.cmsDirectory.SuspendLayout();
            this.cmsFileItem.SuspendLayout();
            this.SuspendLayout();
            // 
            // msMain
            // 
            this.msMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miFile,
            this.miEdit,
            this.miDeploy,
            this.miTools,
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
            this.miFileSave,
            this.miFileSaveAll,
            this.miFileSep1,
            this.miFileClose,
            this.miFileCloseProject,
            this.miFileSep2,
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
            // miFileSep1
            // 
            this.miFileSep1.Name = "miFileSep1";
            this.miFileSep1.Size = new System.Drawing.Size(192, 6);
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
            // miFileSep2
            // 
            this.miFileSep2.Name = "miFileSep2";
            this.miFileSep2.Size = new System.Drawing.Size(192, 6);
            // 
            // miFileExit
            // 
            this.miFileExit.Image = ((System.Drawing.Image)(resources.GetObject("miFileExit.Image")));
            this.miFileExit.Name = "miFileExit";
            this.miFileExit.Size = new System.Drawing.Size(195, 22);
            this.miFileExit.Text = "Exit";
            this.miFileExit.Click += new System.EventHandler(this.miFileExit_Click);
            // 
            // miEdit
            // 
            this.miEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miEditCut,
            this.miEditCopy,
            this.miEditPaste});
            this.miEdit.Name = "miEdit";
            this.miEdit.Size = new System.Drawing.Size(39, 20);
            this.miEdit.Text = "&Edit";
            // 
            // miEditCut
            // 
            this.miEditCut.Image = ((System.Drawing.Image)(resources.GetObject("miEditCut.Image")));
            this.miEditCut.Name = "miEditCut";
            this.miEditCut.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.miEditCut.Size = new System.Drawing.Size(144, 22);
            this.miEditCut.Text = "Cut";
            this.miEditCut.Click += new System.EventHandler(this.miEditCut_Click);
            // 
            // miEditCopy
            // 
            this.miEditCopy.Image = ((System.Drawing.Image)(resources.GetObject("miEditCopy.Image")));
            this.miEditCopy.Name = "miEditCopy";
            this.miEditCopy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.miEditCopy.Size = new System.Drawing.Size(144, 22);
            this.miEditCopy.Text = "Copy";
            this.miEditCopy.Click += new System.EventHandler(this.miEditCopy_Click);
            // 
            // miEditPaste
            // 
            this.miEditPaste.Image = ((System.Drawing.Image)(resources.GetObject("miEditPaste.Image")));
            this.miEditPaste.Name = "miEditPaste";
            this.miEditPaste.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.miEditPaste.Size = new System.Drawing.Size(144, 22);
            this.miEditPaste.Text = "Paste";
            this.miEditPaste.Click += new System.EventHandler(this.miEditPaste_Click);
            // 
            // miTools
            // 
            this.miTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miToolsOptions});
            this.miTools.Name = "miTools";
            this.miTools.Size = new System.Drawing.Size(47, 20);
            this.miTools.Text = "&Tools";
            // 
            // miToolsOptions
            // 
            this.miToolsOptions.Image = ((System.Drawing.Image)(resources.GetObject("miToolsOptions.Image")));
            this.miToolsOptions.Name = "miToolsOptions";
            this.miToolsOptions.Size = new System.Drawing.Size(125, 22);
            this.miToolsOptions.Text = "Options...";
            this.miToolsOptions.Click += new System.EventHandler(this.miToolsOptions_Click);
            // 
            // miHelp
            // 
            this.miHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miHelpDoc,
            this.miHelpSupport,
            this.toolStripMenuItem1,
            this.miHelpAbout});
            this.miHelp.Name = "miHelp";
            this.miHelp.Size = new System.Drawing.Size(44, 20);
            this.miHelp.Text = "&Help";
            // 
            // miHelpDoc
            // 
            this.miHelpDoc.Image = ((System.Drawing.Image)(resources.GetObject("miHelpDoc.Image")));
            this.miHelpDoc.Name = "miHelpDoc";
            this.miHelpDoc.Size = new System.Drawing.Size(180, 22);
            this.miHelpDoc.Text = "Documentation";
            this.miHelpDoc.Click += new System.EventHandler(this.miHelpDoc_Click);
            // 
            // miHelpSupport
            // 
            this.miHelpSupport.Image = ((System.Drawing.Image)(resources.GetObject("miHelpSupport.Image")));
            this.miHelpSupport.Name = "miHelpSupport";
            this.miHelpSupport.Size = new System.Drawing.Size(180, 22);
            this.miHelpSupport.Text = "Technical Support";
            this.miHelpSupport.Click += new System.EventHandler(this.miHelpSupport_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(177, 6);
            // 
            // miHelpAbout
            // 
            this.miHelpAbout.Image = ((System.Drawing.Image)(resources.GetObject("miHelpAbout.Image")));
            this.miHelpAbout.Name = "miHelpAbout";
            this.miHelpAbout.Size = new System.Drawing.Size(180, 22);
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
            this.btnEditCut,
            this.btnEditCopy,
            this.btnEditPaste});
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
            // btnEditCut
            // 
            this.btnEditCut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnEditCut.Image = ((System.Drawing.Image)(resources.GetObject("btnEditCut.Image")));
            this.btnEditCut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnEditCut.Name = "btnEditCut";
            this.btnEditCut.Size = new System.Drawing.Size(23, 22);
            this.btnEditCut.ToolTipText = "Cut (Ctrl+X)";
            this.btnEditCut.Click += new System.EventHandler(this.miEditCut_Click);
            // 
            // btnEditCopy
            // 
            this.btnEditCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnEditCopy.Image = ((System.Drawing.Image)(resources.GetObject("btnEditCopy.Image")));
            this.btnEditCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnEditCopy.Name = "btnEditCopy";
            this.btnEditCopy.Size = new System.Drawing.Size(23, 22);
            this.btnEditCopy.ToolTipText = "Copy (Ctrl+C)";
            this.btnEditCopy.Click += new System.EventHandler(this.miEditCopy_Click);
            // 
            // btnEditPaste
            // 
            this.btnEditPaste.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnEditPaste.Image = ((System.Drawing.Image)(resources.GetObject("btnEditPaste.Image")));
            this.btnEditPaste.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnEditPaste.Name = "btnEditPaste";
            this.btnEditPaste.Size = new System.Drawing.Size(23, 22);
            this.btnEditPaste.ToolTipText = "Paste (Ctrl+V)";
            this.btnEditPaste.Click += new System.EventHandler(this.miEditPaste_Click);
            // 
            // ssMain
            // 
            this.ssMain.Location = new System.Drawing.Point(0, 389);
            this.ssMain.Name = "ssMain";
            this.ssMain.Size = new System.Drawing.Size(684, 22);
            this.ssMain.TabIndex = 2;
            // 
            // pnlLeft
            // 
            this.pnlLeft.Controls.Add(this.tvExplorer);
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlLeft.Location = new System.Drawing.Point(0, 49);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Size = new System.Drawing.Size(250, 340);
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
            this.tvExplorer.Size = new System.Drawing.Size(250, 340);
            this.tvExplorer.TabIndex = 0;
            this.tvExplorer.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.tvExplorer_AfterCollapse);
            this.tvExplorer.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvExplorer_BeforeExpand);
            this.tvExplorer.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.tvExplorer_AfterExpand);
            this.tvExplorer.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvExplorer_NodeMouseClick);
            this.tvExplorer.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvExplorer_NodeMouseDoubleClick);
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
            this.splVert.Size = new System.Drawing.Size(3, 340);
            this.splVert.TabIndex = 4;
            this.splVert.TabStop = false;
            // 
            // pnlRight
            // 
            this.pnlRight.Controls.Add(this.wctrlMain);
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRight.Location = new System.Drawing.Point(253, 49);
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Size = new System.Drawing.Size(431, 340);
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
            this.wctrlMain.Size = new System.Drawing.Size(431, 340);
            this.wctrlMain.TabIndex = 0;
            this.wctrlMain.ActiveFormChanged += new System.EventHandler(this.wctrlMain_ActiveFormChanged);
            this.wctrlMain.ChildFormClosed += new System.EventHandler<WinControl.ChildFormClosedEventArgs>(this.wctrlMain_ChildFormClosed);
            this.wctrlMain.ChildFormMessage += new System.EventHandler<WinControl.FormMessageEventArgs>(this.wctrlMain_ChildFormMessage);
            this.wctrlMain.ChildFormModifiedChanged += new System.EventHandler<WinControl.ChildFormEventArgs>(this.wctrlMain_ChildFormModifiedChanged);
            // 
            // cmsCommLine
            // 
            this.cmsCommLine.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miCommLineAdd,
            this.miCommLineMoveUp,
            this.miCommLineMoveDown,
            this.miCommLineDelete});
            this.cmsCommLine.Name = "cmsCommLine";
            this.cmsCommLine.Size = new System.Drawing.Size(164, 92);
            this.cmsCommLine.Opening += new System.ComponentModel.CancelEventHandler(this.cmsCommLine_Opening);
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
            // cmsInstance
            // 
            this.cmsInstance.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miInstanceAdd,
            this.miInstanceMoveUp,
            this.miInstanceMoveDown,
            this.miInstanceDelete,
            this.miInstanceSep1,
            this.miInstanceDownloadConfig,
            this.miInstanceUploadConfig,
            this.miInstanceStatus,
            this.miInstanceSep2,
            this.miInstanceRename,
            this.miInstanceProperties});
            this.cmsInstance.Name = "cmsCommLine";
            this.cmsInstance.Size = new System.Drawing.Size(215, 236);
            this.cmsInstance.Opening += new System.ComponentModel.CancelEventHandler(this.cmsInstance_Opening);
            // 
            // miInstanceAdd
            // 
            this.miInstanceAdd.Image = ((System.Drawing.Image)(resources.GetObject("miInstanceAdd.Image")));
            this.miInstanceAdd.Name = "miInstanceAdd";
            this.miInstanceAdd.Size = new System.Drawing.Size(214, 22);
            this.miInstanceAdd.Text = "Add Instance...";
            this.miInstanceAdd.Click += new System.EventHandler(this.miInstanceAdd_Click);
            // 
            // miInstanceMoveUp
            // 
            this.miInstanceMoveUp.Image = ((System.Drawing.Image)(resources.GetObject("miInstanceMoveUp.Image")));
            this.miInstanceMoveUp.Name = "miInstanceMoveUp";
            this.miInstanceMoveUp.Size = new System.Drawing.Size(214, 22);
            this.miInstanceMoveUp.Text = "Move Instance Up";
            this.miInstanceMoveUp.Click += new System.EventHandler(this.miInstanceMoveUp_Click);
            // 
            // miInstanceMoveDown
            // 
            this.miInstanceMoveDown.Image = ((System.Drawing.Image)(resources.GetObject("miInstanceMoveDown.Image")));
            this.miInstanceMoveDown.Name = "miInstanceMoveDown";
            this.miInstanceMoveDown.Size = new System.Drawing.Size(214, 22);
            this.miInstanceMoveDown.Text = "Move Instance Down";
            this.miInstanceMoveDown.Click += new System.EventHandler(this.miInstanceMoveDown_Click);
            // 
            // miInstanceDelete
            // 
            this.miInstanceDelete.Image = ((System.Drawing.Image)(resources.GetObject("miInstanceDelete.Image")));
            this.miInstanceDelete.Name = "miInstanceDelete";
            this.miInstanceDelete.Size = new System.Drawing.Size(214, 22);
            this.miInstanceDelete.Text = "Delete Instance";
            this.miInstanceDelete.Click += new System.EventHandler(this.miInstanceDelete_Click);
            // 
            // miInstanceSep1
            // 
            this.miInstanceSep1.Name = "miInstanceSep1";
            this.miInstanceSep1.Size = new System.Drawing.Size(211, 6);
            // 
            // miInstanceRename
            // 
            this.miInstanceRename.Image = ((System.Drawing.Image)(resources.GetObject("miInstanceRename.Image")));
            this.miInstanceRename.Name = "miInstanceRename";
            this.miInstanceRename.Size = new System.Drawing.Size(214, 22);
            this.miInstanceRename.Text = "Rename Instance";
            this.miInstanceRename.Click += new System.EventHandler(this.miInstanceRename_Click);
            // 
            // miInstanceProperties
            // 
            this.miInstanceProperties.Image = ((System.Drawing.Image)(resources.GetObject("miInstanceProperties.Image")));
            this.miInstanceProperties.Name = "miInstanceProperties";
            this.miInstanceProperties.Size = new System.Drawing.Size(214, 22);
            this.miInstanceProperties.Text = "Properties";
            this.miInstanceProperties.Click += new System.EventHandler(this.miInstanceProperties_Click);
            // 
            // cmsProject
            // 
            this.cmsProject.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miProjectRename,
            this.miProjectProperties});
            this.cmsProject.Name = "cmsCommLine";
            this.cmsProject.Size = new System.Drawing.Size(158, 48);
            // 
            // miProjectRename
            // 
            this.miProjectRename.Image = ((System.Drawing.Image)(resources.GetObject("miProjectRename.Image")));
            this.miProjectRename.Name = "miProjectRename";
            this.miProjectRename.Size = new System.Drawing.Size(157, 22);
            this.miProjectRename.Text = "Rename Project";
            this.miProjectRename.Click += new System.EventHandler(this.miProjectRename_Click);
            // 
            // miProjectProperties
            // 
            this.miProjectProperties.Image = ((System.Drawing.Image)(resources.GetObject("miProjectProperties.Image")));
            this.miProjectProperties.Name = "miProjectProperties";
            this.miProjectProperties.Size = new System.Drawing.Size(157, 22);
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
            this.cmsDirectory.Size = new System.Drawing.Size(219, 148);
            this.cmsDirectory.Opening += new System.ComponentModel.CancelEventHandler(this.cmsDirectory_Opening);
            // 
            // miDirectoryNewFile
            // 
            this.miDirectoryNewFile.Image = ((System.Drawing.Image)(resources.GetObject("miDirectoryNewFile.Image")));
            this.miDirectoryNewFile.Name = "miDirectoryNewFile";
            this.miDirectoryNewFile.Size = new System.Drawing.Size(218, 22);
            this.miDirectoryNewFile.Text = "New File...";
            this.miDirectoryNewFile.Click += new System.EventHandler(this.miDirectoryNewFile_Click);
            // 
            // miDirectoryNewFolder
            // 
            this.miDirectoryNewFolder.Image = ((System.Drawing.Image)(resources.GetObject("miDirectoryNewFolder.Image")));
            this.miDirectoryNewFolder.Name = "miDirectoryNewFolder";
            this.miDirectoryNewFolder.Size = new System.Drawing.Size(218, 22);
            this.miDirectoryNewFolder.Text = "New Folder...";
            this.miDirectoryNewFolder.Click += new System.EventHandler(this.miDirectoryNewFolder_Click);
            // 
            // miDirectorySep1
            // 
            this.miDirectorySep1.Name = "miDirectorySep1";
            this.miDirectorySep1.Size = new System.Drawing.Size(215, 6);
            // 
            // miDirectoryDelete
            // 
            this.miDirectoryDelete.Image = ((System.Drawing.Image)(resources.GetObject("miDirectoryDelete.Image")));
            this.miDirectoryDelete.Name = "miDirectoryDelete";
            this.miDirectoryDelete.Size = new System.Drawing.Size(218, 22);
            this.miDirectoryDelete.Text = "Delete";
            this.miDirectoryDelete.Click += new System.EventHandler(this.miDirectoryDelete_Click);
            // 
            // miDirectoryRename
            // 
            this.miDirectoryRename.Image = ((System.Drawing.Image)(resources.GetObject("miDirectoryRename.Image")));
            this.miDirectoryRename.Name = "miDirectoryRename";
            this.miDirectoryRename.Size = new System.Drawing.Size(218, 22);
            this.miDirectoryRename.Text = "Rename";
            this.miDirectoryRename.Click += new System.EventHandler(this.miDirectoryRename_Click);
            // 
            // miDirectorySep2
            // 
            this.miDirectorySep2.Name = "miDirectorySep2";
            this.miDirectorySep2.Size = new System.Drawing.Size(215, 6);
            // 
            // miDirectoryOpenInExplorer
            // 
            this.miDirectoryOpenInExplorer.Image = ((System.Drawing.Image)(resources.GetObject("miDirectoryOpenInExplorer.Image")));
            this.miDirectoryOpenInExplorer.Name = "miDirectoryOpenInExplorer";
            this.miDirectoryOpenInExplorer.Size = new System.Drawing.Size(218, 22);
            this.miDirectoryOpenInExplorer.Text = "Open Folder in File Explorer";
            this.miDirectoryOpenInExplorer.Click += new System.EventHandler(this.miDirectoryOpenInExplorer_Click);
            // 
            // miDirectoryRefresh
            // 
            this.miDirectoryRefresh.Image = ((System.Drawing.Image)(resources.GetObject("miDirectoryRefresh.Image")));
            this.miDirectoryRefresh.Name = "miDirectoryRefresh";
            this.miDirectoryRefresh.Size = new System.Drawing.Size(218, 22);
            this.miDirectoryRefresh.Text = "Refresh";
            this.miDirectoryRefresh.Click += new System.EventHandler(this.miDirectoryRefresh_Click);
            // 
            // cmsFileItem
            // 
            this.cmsFileItem.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miFileItemOpen,
            this.miFileItemOpenLocation,
            this.toolStripMenuItem4,
            this.miFileItemDelete,
            this.miFileItemRename});
            this.cmsFileItem.Name = "cmsFileItem";
            this.cmsFileItem.Size = new System.Drawing.Size(232, 98);
            this.cmsFileItem.Opening += new System.ComponentModel.CancelEventHandler(this.cmsFileItem_Opening);
            // 
            // miFileItemOpen
            // 
            this.miFileItemOpen.Image = ((System.Drawing.Image)(resources.GetObject("miFileItemOpen.Image")));
            this.miFileItemOpen.Name = "miFileItemOpen";
            this.miFileItemOpen.Size = new System.Drawing.Size(231, 22);
            this.miFileItemOpen.Text = "Open";
            this.miFileItemOpen.Click += new System.EventHandler(this.miFileItemOpen_Click);
            // 
            // miFileItemOpenLocation
            // 
            this.miFileItemOpenLocation.Image = ((System.Drawing.Image)(resources.GetObject("miFileItemOpenLocation.Image")));
            this.miFileItemOpenLocation.Name = "miFileItemOpenLocation";
            this.miFileItemOpenLocation.Size = new System.Drawing.Size(231, 22);
            this.miFileItemOpenLocation.Text = "Open Location in File Explorer";
            this.miFileItemOpenLocation.Click += new System.EventHandler(this.miFileItemOpenLocation_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(228, 6);
            // 
            // miFileItemDelete
            // 
            this.miFileItemDelete.Image = ((System.Drawing.Image)(resources.GetObject("miFileItemDelete.Image")));
            this.miFileItemDelete.Name = "miFileItemDelete";
            this.miFileItemDelete.Size = new System.Drawing.Size(231, 22);
            this.miFileItemDelete.Text = "Delete";
            this.miFileItemDelete.Click += new System.EventHandler(this.miFileItemDelete_Click);
            // 
            // miFileItemRename
            // 
            this.miFileItemRename.Image = ((System.Drawing.Image)(resources.GetObject("miFileItemRename.Image")));
            this.miFileItemRename.Name = "miFileItemRename";
            this.miFileItemRename.Size = new System.Drawing.Size(231, 22);
            this.miFileItemRename.Text = "Rename";
            this.miFileItemRename.Click += new System.EventHandler(this.miFileItemRename_Click);
            // 
            // miDeploy
            // 
            this.miDeploy.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miDeployDownloadConfig,
            this.miDeployUploadConfig,
            this.miDeployInstanceStatus});
            this.miDeploy.Name = "miDeploy";
            this.miDeploy.Size = new System.Drawing.Size(56, 20);
            this.miDeploy.Text = "&Deploy";
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
            // miInstanceSep2
            // 
            this.miInstanceSep2.Name = "miInstanceSep2";
            this.miInstanceSep2.Size = new System.Drawing.Size(211, 6);
            // 
            // miInstanceDownloadConfig
            // 
            this.miInstanceDownloadConfig.Image = ((System.Drawing.Image)(resources.GetObject("miInstanceDownloadConfig.Image")));
            this.miInstanceDownloadConfig.Name = "miInstanceDownloadConfig";
            this.miInstanceDownloadConfig.Size = new System.Drawing.Size(214, 22);
            this.miInstanceDownloadConfig.Text = "Download Configuration...";
            this.miInstanceDownloadConfig.Click += new System.EventHandler(this.miDeployDownloadConfig_Click);
            // 
            // miInstanceUploadConfig
            // 
            this.miInstanceUploadConfig.Image = ((System.Drawing.Image)(resources.GetObject("miInstanceUploadConfig.Image")));
            this.miInstanceUploadConfig.Name = "miInstanceUploadConfig";
            this.miInstanceUploadConfig.Size = new System.Drawing.Size(214, 22);
            this.miInstanceUploadConfig.Text = "Upload Configuration...";
            this.miInstanceUploadConfig.Click += new System.EventHandler(this.miDeployUploadConfig_Click);
            // 
            // miInstanceStatus
            // 
            this.miInstanceStatus.Image = ((System.Drawing.Image)(resources.GetObject("miInstanceStatus.Image")));
            this.miInstanceStatus.Name = "miInstanceStatus";
            this.miInstanceStatus.Size = new System.Drawing.Size(214, 22);
            this.miInstanceStatus.Text = "Instance Status...";
            this.miInstanceStatus.Click += new System.EventHandler(this.miDeployInstanceStatus_Click);
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
            this.MainMenuStrip = this.msMain;
            this.MinimumSize = new System.Drawing.Size(300, 200);
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Administrator";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMain_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmMain_FormClosed);
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.msMain.ResumeLayout(false);
            this.msMain.PerformLayout();
            this.tsMain.ResumeLayout(false);
            this.tsMain.PerformLayout();
            this.pnlLeft.ResumeLayout(false);
            this.pnlRight.ResumeLayout(false);
            this.cmsCommLine.ResumeLayout(false);
            this.cmsInstance.ResumeLayout(false);
            this.cmsProject.ResumeLayout(false);
            this.cmsDirectory.ResumeLayout(false);
            this.cmsFileItem.ResumeLayout(false);
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
        private System.Windows.Forms.ToolStripMenuItem miEdit;
        private System.Windows.Forms.ToolStripMenuItem miFileNewProject;
        private System.Windows.Forms.ToolStripMenuItem miFileOpenProject;
        private System.Windows.Forms.ToolStripMenuItem miFileSave;
        private System.Windows.Forms.ToolStripMenuItem miFileSaveAll;
        private System.Windows.Forms.ToolStripSeparator miFileSep1;
        private System.Windows.Forms.ToolStripMenuItem miFileExit;
        private System.Windows.Forms.ToolStripMenuItem miEditCut;
        private System.Windows.Forms.ToolStripMenuItem miEditCopy;
        private System.Windows.Forms.ToolStripMenuItem miEditPaste;
        private System.Windows.Forms.ToolStripMenuItem miTools;
        private System.Windows.Forms.ToolStripMenuItem miToolsOptions;
        private System.Windows.Forms.ToolStripMenuItem miHelp;
        private System.Windows.Forms.ToolStripMenuItem miHelpAbout;
        private System.Windows.Forms.ToolStripMenuItem miHelpDoc;
        private System.Windows.Forms.ToolStripMenuItem miHelpSupport;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
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
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
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
        private System.Windows.Forms.ToolStripButton btnEditCut;
        private System.Windows.Forms.ToolStripButton btnEditCopy;
        private System.Windows.Forms.ToolStripButton btnEditPaste;
        private System.Windows.Forms.ToolStripMenuItem miDeploy;
        private System.Windows.Forms.ToolStripMenuItem miDeployDownloadConfig;
        private System.Windows.Forms.ToolStripMenuItem miDeployUploadConfig;
        private System.Windows.Forms.ToolStripMenuItem miDeployInstanceStatus;
        private System.Windows.Forms.ToolStripSeparator miInstanceSep2;
        private System.Windows.Forms.ToolStripMenuItem miInstanceDownloadConfig;
        private System.Windows.Forms.ToolStripMenuItem miInstanceUploadConfig;
        private System.Windows.Forms.ToolStripMenuItem miInstanceStatus;
    }
}

