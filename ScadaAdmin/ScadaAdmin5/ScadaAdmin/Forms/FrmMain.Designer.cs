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
            this.miFileSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.miFileSep = new System.Windows.Forms.ToolStripSeparator();
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
            this.btnFileNew = new System.Windows.Forms.ToolStripButton();
            this.ssMain = new System.Windows.Forms.StatusStrip();
            this.pnlLeft = new System.Windows.Forms.Panel();
            this.tvExplorer = new System.Windows.Forms.TreeView();
            this.ilExplorer = new System.Windows.Forms.ImageList(this.components);
            this.splVert = new System.Windows.Forms.Splitter();
            this.pnlRight = new System.Windows.Forms.Panel();
            this.wctrlMain = new WinControl.WinControl();
            this.msMain.SuspendLayout();
            this.tsMain.SuspendLayout();
            this.pnlLeft.SuspendLayout();
            this.pnlRight.SuspendLayout();
            this.SuspendLayout();
            // 
            // msMain
            // 
            this.msMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miFile,
            this.miEdit,
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
            this.miFileSaveAs,
            this.miFileSep,
            this.miFileExit});
            this.miFile.Name = "miFile";
            this.miFile.Size = new System.Drawing.Size(37, 20);
            this.miFile.Text = "&File";
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
            this.miFileSave.Size = new System.Drawing.Size(190, 22);
            this.miFileSave.Text = "Save";
            this.miFileSave.Click += new System.EventHandler(this.miFileSave_Click);
            // 
            // miFileSaveAs
            // 
            this.miFileSaveAs.Image = ((System.Drawing.Image)(resources.GetObject("miFileSaveAs.Image")));
            this.miFileSaveAs.Name = "miFileSaveAs";
            this.miFileSaveAs.Size = new System.Drawing.Size(190, 22);
            this.miFileSaveAs.Text = "Save As...";
            this.miFileSaveAs.Click += new System.EventHandler(this.miFileSaveAs_Click);
            // 
            // miFileSep
            // 
            this.miFileSep.Name = "miFileSep";
            this.miFileSep.Size = new System.Drawing.Size(187, 6);
            // 
            // miFileExit
            // 
            this.miFileExit.Image = ((System.Drawing.Image)(resources.GetObject("miFileExit.Image")));
            this.miFileExit.Name = "miFileExit";
            this.miFileExit.Size = new System.Drawing.Size(190, 22);
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
            this.miHelpDoc.Size = new System.Drawing.Size(169, 22);
            this.miHelpDoc.Text = "Documentation";
            this.miHelpDoc.Click += new System.EventHandler(this.miHelpDoc_Click);
            // 
            // miHelpSupport
            // 
            this.miHelpSupport.Image = ((System.Drawing.Image)(resources.GetObject("miHelpSupport.Image")));
            this.miHelpSupport.Name = "miHelpSupport";
            this.miHelpSupport.Size = new System.Drawing.Size(169, 22);
            this.miHelpSupport.Text = "Technical Support";
            this.miHelpSupport.Click += new System.EventHandler(this.miHelpSupport_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(166, 6);
            // 
            // miHelpAbout
            // 
            this.miHelpAbout.Image = ((System.Drawing.Image)(resources.GetObject("miHelpAbout.Image")));
            this.miHelpAbout.Name = "miHelpAbout";
            this.miHelpAbout.Size = new System.Drawing.Size(169, 22);
            this.miHelpAbout.Text = "About";
            this.miHelpAbout.Click += new System.EventHandler(this.miHelpAbout_Click);
            // 
            // tsMain
            // 
            this.tsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnFileNew});
            this.tsMain.Location = new System.Drawing.Point(0, 24);
            this.tsMain.Name = "tsMain";
            this.tsMain.Size = new System.Drawing.Size(684, 25);
            this.tsMain.TabIndex = 1;
            // 
            // btnFileNew
            // 
            this.btnFileNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnFileNew.Image = ((System.Drawing.Image)(resources.GetObject("btnFileNew.Image")));
            this.btnFileNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnFileNew.Name = "btnFileNew";
            this.btnFileNew.Size = new System.Drawing.Size(23, 22);
            this.btnFileNew.ToolTipText = "Create new project (Ctrl+N)";
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
            this.tvExplorer.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvExplorer_NodeMouseClick);
            this.tvExplorer.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvExplorer_NodeMouseDoubleClick);
            // 
            // ilExplorer
            // 
            this.ilExplorer.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilExplorer.ImageStream")));
            this.ilExplorer.TransparentColor = System.Drawing.Color.Transparent;
            this.ilExplorer.Images.SetKeyName(0, "comm.png");
            this.ilExplorer.Images.SetKeyName(1, "database.png");
            this.ilExplorer.Images.SetKeyName(2, "folder_closed.png");
            this.ilExplorer.Images.SetKeyName(3, "folder_open.png");
            this.ilExplorer.Images.SetKeyName(4, "instance.png");
            this.ilExplorer.Images.SetKeyName(5, "instances.png");
            this.ilExplorer.Images.SetKeyName(6, "interface.png");
            this.ilExplorer.Images.SetKeyName(7, "project.png");
            this.ilExplorer.Images.SetKeyName(8, "server.png");
            this.ilExplorer.Images.SetKeyName(9, "table.png");
            this.ilExplorer.Images.SetKeyName(10, "webstation.png");
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
            this.wctrlMain.MessageFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.wctrlMain.Name = "wctrlMain";
            this.wctrlMain.SaveReqCancel = "Cancel";
            this.wctrlMain.SaveReqCaption = "Save changes";
            this.wctrlMain.SaveReqNo = "&No";
            this.wctrlMain.SaveReqQuestion = "Save changes to the following items?";
            this.wctrlMain.SaveReqYes = "&Yes";
            this.wctrlMain.Size = new System.Drawing.Size(431, 340);
            this.wctrlMain.TabIndex = 0;
            this.wctrlMain.ChildFormClosed += new System.EventHandler<WinControl.ChildFormClosedEventArgs>(this.wctrlMain_ChildFormClosed);
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
            this.pnlLeft.ResumeLayout(false);
            this.pnlRight.ResumeLayout(false);
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
        private System.Windows.Forms.ToolStripMenuItem miFileSaveAs;
        private System.Windows.Forms.ToolStripSeparator miFileSep;
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
        private System.Windows.Forms.ToolStripButton btnFileNew;
        private System.Windows.Forms.TreeView tvExplorer;
        private WinControl.WinControl wctrlMain;
        private System.Windows.Forms.ImageList ilExplorer;
    }
}

