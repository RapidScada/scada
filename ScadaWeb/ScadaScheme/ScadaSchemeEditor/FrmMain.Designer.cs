namespace Scada.Scheme.Editor
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
            System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("Standard", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewItem listViewItem6 = new System.Windows.Forms.ListViewItem("Pointer", "(none)");
            System.Windows.Forms.ListViewItem listViewItem7 = new System.Windows.Forms.ListViewItem("Static Text", "(none)");
            System.Windows.Forms.ListViewItem listViewItem8 = new System.Windows.Forms.ListViewItem("Dynamic Text", "(none)");
            System.Windows.Forms.ListViewItem listViewItem9 = new System.Windows.Forms.ListViewItem("Static Picture", "(none)");
            System.Windows.Forms.ListViewItem listViewItem10 = new System.Windows.Forms.ListViewItem("Dynamic Picture", "(none)");
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.btnFileNew = new System.Windows.Forms.ToolStripButton();
            this.btnFileOpen = new System.Windows.Forms.ToolStripButton();
            this.btnFileSave = new System.Windows.Forms.ToolStripSplitButton();
            this.miFileSaveAs2 = new System.Windows.Forms.ToolStripMenuItem();
            this.btnFileOpenBrowser = new System.Windows.Forms.ToolStripButton();
            this.sep1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnEditCut = new System.Windows.Forms.ToolStripButton();
            this.btnEditCopy = new System.Windows.Forms.ToolStripButton();
            this.btnEditPaste = new System.Windows.Forms.ToolStripButton();
            this.sep2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnEditUndo = new System.Windows.Forms.ToolStripButton();
            this.btnEditRedo = new System.Windows.Forms.ToolStripButton();
            this.sep3 = new System.Windows.Forms.ToolStripSeparator();
            this.btnEditPointer = new System.Windows.Forms.ToolStripButton();
            this.btnEditDelete = new System.Windows.Forms.ToolStripButton();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.pageComponents = new System.Windows.Forms.TabPage();
            this.lvCompTypes = new System.Windows.Forms.ListView();
            this.colCompName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ilCompTypes = new System.Windows.Forms.ImageList(this.components);
            this.pageProperties = new System.Windows.Forms.TabPage();
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.cbSchComp = new System.Windows.Forms.ComboBox();
            this.ofdScheme = new System.Windows.Forms.OpenFileDialog();
            this.sfdScheme = new System.Windows.Forms.SaveFileDialog();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.miFile = new System.Windows.Forms.ToolStripMenuItem();
            this.miFileNew = new System.Windows.Forms.ToolStripMenuItem();
            this.miFileOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.miFileSave = new System.Windows.Forms.ToolStripMenuItem();
            this.miFileSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.miFileOpenBrowser = new System.Windows.Forms.ToolStripMenuItem();
            this.miFileSep = new System.Windows.Forms.ToolStripSeparator();
            this.miFileExit = new System.Windows.Forms.ToolStripMenuItem();
            this.miEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.miEditCut = new System.Windows.Forms.ToolStripMenuItem();
            this.miEditCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.miEditPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.miEditSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.miEditUndo = new System.Windows.Forms.ToolStripMenuItem();
            this.miEditRedo = new System.Windows.Forms.ToolStripMenuItem();
            this.miEditSep2 = new System.Windows.Forms.ToolStripSeparator();
            this.miEditPointer = new System.Windows.Forms.ToolStripMenuItem();
            this.miEditDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.miTools = new System.Windows.Forms.ToolStripMenuItem();
            this.miToolsOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.miHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.miHelpAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.miEditPasteSpecial = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.pageComponents.SuspendLayout();
            this.pageProperties.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnFileNew,
            this.btnFileOpen,
            this.btnFileSave,
            this.btnFileOpenBrowser,
            this.sep1,
            this.btnEditCut,
            this.btnEditCopy,
            this.btnEditPaste,
            this.sep2,
            this.btnEditUndo,
            this.btnEditRedo,
            this.sep3,
            this.btnEditPointer,
            this.btnEditDelete});
            this.toolStrip.Location = new System.Drawing.Point(0, 24);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(309, 25);
            this.toolStrip.TabIndex = 0;
            this.toolStrip.MouseEnter += new System.EventHandler(this.FrmMain_MouseEnter);
            // 
            // btnFileNew
            // 
            this.btnFileNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnFileNew.Image = ((System.Drawing.Image)(resources.GetObject("btnFileNew.Image")));
            this.btnFileNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnFileNew.Name = "btnFileNew";
            this.btnFileNew.Size = new System.Drawing.Size(23, 22);
            this.btnFileNew.ToolTipText = "New scheme (Ctrl+N)";
            this.btnFileNew.Click += new System.EventHandler(this.miFileNew_Click);
            // 
            // btnFileOpen
            // 
            this.btnFileOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnFileOpen.Image = ((System.Drawing.Image)(resources.GetObject("btnFileOpen.Image")));
            this.btnFileOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnFileOpen.Name = "btnFileOpen";
            this.btnFileOpen.Size = new System.Drawing.Size(23, 22);
            this.btnFileOpen.ToolTipText = "Open scheme (Ctrl+O)";
            this.btnFileOpen.Click += new System.EventHandler(this.miFileOpen_Click);
            // 
            // btnFileSave
            // 
            this.btnFileSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnFileSave.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miFileSaveAs2});
            this.btnFileSave.Image = ((System.Drawing.Image)(resources.GetObject("btnFileSave.Image")));
            this.btnFileSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnFileSave.Name = "btnFileSave";
            this.btnFileSave.Size = new System.Drawing.Size(32, 22);
            this.btnFileSave.ToolTipText = "Save scheme (Ctrl+S)";
            this.btnFileSave.ButtonClick += new System.EventHandler(this.miFileSave_Click);
            // 
            // miFileSaveAs2
            // 
            this.miFileSaveAs2.Image = ((System.Drawing.Image)(resources.GetObject("miFileSaveAs2.Image")));
            this.miFileSaveAs2.Name = "miFileSaveAs2";
            this.miFileSaveAs2.Size = new System.Drawing.Size(123, 22);
            this.miFileSaveAs2.Text = "Save As...";
            this.miFileSaveAs2.Click += new System.EventHandler(this.miFileSaveAs_Click);
            // 
            // btnFileOpenBrowser
            // 
            this.btnFileOpenBrowser.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnFileOpenBrowser.Image = ((System.Drawing.Image)(resources.GetObject("btnFileOpenBrowser.Image")));
            this.btnFileOpenBrowser.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnFileOpenBrowser.Name = "btnFileOpenBrowser";
            this.btnFileOpenBrowser.Size = new System.Drawing.Size(23, 22);
            this.btnFileOpenBrowser.ToolTipText = "Open new browser tab";
            this.btnFileOpenBrowser.Click += new System.EventHandler(this.miFileOpenBrowser_Click);
            // 
            // sep1
            // 
            this.sep1.Name = "sep1";
            this.sep1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnEditCut
            // 
            this.btnEditCut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnEditCut.Image = ((System.Drawing.Image)(resources.GetObject("btnEditCut.Image")));
            this.btnEditCut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnEditCut.Name = "btnEditCut";
            this.btnEditCut.Size = new System.Drawing.Size(23, 22);
            this.btnEditCut.ToolTipText = "Cut scheme components (Ctrl+X)";
            this.btnEditCut.Click += new System.EventHandler(this.miEditCut_Click);
            // 
            // btnEditCopy
            // 
            this.btnEditCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnEditCopy.Image = ((System.Drawing.Image)(resources.GetObject("btnEditCopy.Image")));
            this.btnEditCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnEditCopy.Name = "btnEditCopy";
            this.btnEditCopy.Size = new System.Drawing.Size(23, 22);
            this.btnEditCopy.ToolTipText = "Copy scheme components (Ctrl+C)";
            this.btnEditCopy.Click += new System.EventHandler(this.miEditCopy_Click);
            // 
            // btnEditPaste
            // 
            this.btnEditPaste.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnEditPaste.Image = ((System.Drawing.Image)(resources.GetObject("btnEditPaste.Image")));
            this.btnEditPaste.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnEditPaste.Name = "btnEditPaste";
            this.btnEditPaste.Size = new System.Drawing.Size(23, 22);
            this.btnEditPaste.ToolTipText = "Paste scheme components (Ctrl+V)";
            this.btnEditPaste.Click += new System.EventHandler(this.miEditPaste_Click);
            // 
            // sep2
            // 
            this.sep2.Name = "sep2";
            this.sep2.Size = new System.Drawing.Size(6, 25);
            // 
            // btnEditUndo
            // 
            this.btnEditUndo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnEditUndo.Image = ((System.Drawing.Image)(resources.GetObject("btnEditUndo.Image")));
            this.btnEditUndo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnEditUndo.Name = "btnEditUndo";
            this.btnEditUndo.Size = new System.Drawing.Size(23, 22);
            this.btnEditUndo.ToolTipText = "Undo (Ctrl+Z)";
            this.btnEditUndo.Click += new System.EventHandler(this.miEditUndo_Click);
            // 
            // btnEditRedo
            // 
            this.btnEditRedo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnEditRedo.Image = ((System.Drawing.Image)(resources.GetObject("btnEditRedo.Image")));
            this.btnEditRedo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnEditRedo.Name = "btnEditRedo";
            this.btnEditRedo.Size = new System.Drawing.Size(23, 22);
            this.btnEditRedo.ToolTipText = "Redo (Ctrl+Y)";
            this.btnEditRedo.Click += new System.EventHandler(this.miEditRedo_Click);
            // 
            // sep3
            // 
            this.sep3.Name = "sep3";
            this.sep3.Size = new System.Drawing.Size(6, 25);
            // 
            // btnEditPointer
            // 
            this.btnEditPointer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnEditPointer.Image = ((System.Drawing.Image)(resources.GetObject("btnEditPointer.Image")));
            this.btnEditPointer.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnEditPointer.Name = "btnEditPointer";
            this.btnEditPointer.Size = new System.Drawing.Size(23, 22);
            this.btnEditPointer.ToolTipText = "Cancel adding component (Esc)";
            this.btnEditPointer.Click += new System.EventHandler(this.miEditPointer_Click);
            // 
            // btnEditDelete
            // 
            this.btnEditDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnEditDelete.Image = ((System.Drawing.Image)(resources.GetObject("btnEditDelete.Image")));
            this.btnEditDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnEditDelete.Name = "btnEditDelete";
            this.btnEditDelete.Size = new System.Drawing.Size(23, 22);
            this.btnEditDelete.ToolTipText = "Delete selected components (Del)";
            this.btnEditDelete.Click += new System.EventHandler(this.miEditDelete_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
            this.statusStrip.Location = new System.Drawing.Point(0, 489);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(309, 22);
            this.statusStrip.TabIndex = 2;
            this.statusStrip.MouseEnter += new System.EventHandler(this.FrmMain_MouseEnter);
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(52, 17);
            this.lblStatus.Text = "lblStatus";
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.pageComponents);
            this.tabControl.Controls.Add(this.pageProperties);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 49);
            this.tabControl.Multiline = true;
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(309, 440);
            this.tabControl.TabIndex = 1;
            this.tabControl.MouseEnter += new System.EventHandler(this.FrmMain_MouseEnter);
            // 
            // pageComponents
            // 
            this.pageComponents.Controls.Add(this.lvCompTypes);
            this.pageComponents.Location = new System.Drawing.Point(4, 22);
            this.pageComponents.Name = "pageComponents";
            this.pageComponents.Padding = new System.Windows.Forms.Padding(3);
            this.pageComponents.Size = new System.Drawing.Size(301, 414);
            this.pageComponents.TabIndex = 2;
            this.pageComponents.Text = "Components";
            this.pageComponents.UseVisualStyleBackColor = true;
            // 
            // lvCompTypes
            // 
            this.lvCompTypes.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colCompName});
            this.lvCompTypes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvCompTypes.FullRowSelect = true;
            listViewGroup2.Header = "Standard";
            listViewGroup2.Name = "lvgStandard";
            this.lvCompTypes.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup2});
            this.lvCompTypes.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvCompTypes.HideSelection = false;
            listViewItem6.Group = listViewGroup2;
            listViewItem6.IndentCount = 1;
            listViewItem7.Group = listViewGroup2;
            listViewItem7.IndentCount = 1;
            listViewItem7.Tag = "Scada.Scheme.Model.StaticText";
            listViewItem8.Group = listViewGroup2;
            listViewItem8.IndentCount = 1;
            listViewItem8.Tag = "Scada.Scheme.Model.DynamicText";
            listViewItem9.Group = listViewGroup2;
            listViewItem9.IndentCount = 1;
            listViewItem9.Tag = "Scada.Scheme.Model.StaticPicture";
            listViewItem10.Group = listViewGroup2;
            listViewItem10.IndentCount = 1;
            listViewItem10.Tag = "Scada.Scheme.Model.DynamicPicture";
            this.lvCompTypes.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem6,
            listViewItem7,
            listViewItem8,
            listViewItem9,
            listViewItem10});
            this.lvCompTypes.LabelWrap = false;
            this.lvCompTypes.Location = new System.Drawing.Point(3, 3);
            this.lvCompTypes.MultiSelect = false;
            this.lvCompTypes.Name = "lvCompTypes";
            this.lvCompTypes.Size = new System.Drawing.Size(295, 408);
            this.lvCompTypes.SmallImageList = this.ilCompTypes;
            this.lvCompTypes.TabIndex = 0;
            this.lvCompTypes.UseCompatibleStateImageBehavior = false;
            this.lvCompTypes.View = System.Windows.Forms.View.Details;
            this.lvCompTypes.SelectedIndexChanged += new System.EventHandler(this.lvCompTypes_SelectedIndexChanged);
            // 
            // colCompName
            // 
            this.colCompName.Width = 250;
            // 
            // ilCompTypes
            // 
            this.ilCompTypes.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.ilCompTypes.ImageSize = new System.Drawing.Size(16, 16);
            this.ilCompTypes.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // pageProperties
            // 
            this.pageProperties.Controls.Add(this.propertyGrid);
            this.pageProperties.Controls.Add(this.cbSchComp);
            this.pageProperties.Location = new System.Drawing.Point(4, 22);
            this.pageProperties.Name = "pageProperties";
            this.pageProperties.Padding = new System.Windows.Forms.Padding(3);
            this.pageProperties.Size = new System.Drawing.Size(301, 414);
            this.pageProperties.TabIndex = 0;
            this.pageProperties.Text = "Properties";
            this.pageProperties.UseVisualStyleBackColor = true;
            // 
            // propertyGrid
            // 
            this.propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid.LineColor = System.Drawing.SystemColors.ControlDark;
            this.propertyGrid.Location = new System.Drawing.Point(3, 24);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new System.Drawing.Size(295, 387);
            this.propertyGrid.TabIndex = 1;
            this.propertyGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid_PropertyValueChanged);
            // 
            // cbSchComp
            // 
            this.cbSchComp.Dock = System.Windows.Forms.DockStyle.Top;
            this.cbSchComp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSchComp.FormattingEnabled = true;
            this.cbSchComp.Location = new System.Drawing.Point(3, 3);
            this.cbSchComp.Name = "cbSchComp";
            this.cbSchComp.Size = new System.Drawing.Size(295, 21);
            this.cbSchComp.TabIndex = 0;
            this.cbSchComp.SelectedIndexChanged += new System.EventHandler(this.cbSchComp_SelectedIndexChanged);
            // 
            // ofdScheme
            // 
            this.ofdScheme.DefaultExt = "*.sch";
            this.ofdScheme.Filter = "Schemes (*.sch)|*.sch|All Files (*.*)|*.*";
            // 
            // sfdScheme
            // 
            this.sfdScheme.DefaultExt = "*.sch";
            this.sfdScheme.Filter = "Schemes (*.sch)|*.sch|All Files (*.*)|*.*";
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miFile,
            this.miEdit,
            this.miTools,
            this.miHelp});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(309, 24);
            this.menuStrip.TabIndex = 3;
            // 
            // miFile
            // 
            this.miFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miFileNew,
            this.miFileOpen,
            this.miFileSave,
            this.miFileSaveAs,
            this.miFileOpenBrowser,
            this.miFileSep,
            this.miFileExit});
            this.miFile.Name = "miFile";
            this.miFile.Size = new System.Drawing.Size(37, 20);
            this.miFile.Text = "&File";
            // 
            // miFileNew
            // 
            this.miFileNew.Image = ((System.Drawing.Image)(resources.GetObject("miFileNew.Image")));
            this.miFileNew.Name = "miFileNew";
            this.miFileNew.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.miFileNew.Size = new System.Drawing.Size(155, 22);
            this.miFileNew.Text = "New";
            this.miFileNew.Click += new System.EventHandler(this.miFileNew_Click);
            // 
            // miFileOpen
            // 
            this.miFileOpen.Image = ((System.Drawing.Image)(resources.GetObject("miFileOpen.Image")));
            this.miFileOpen.Name = "miFileOpen";
            this.miFileOpen.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.miFileOpen.Size = new System.Drawing.Size(155, 22);
            this.miFileOpen.Text = "Open...";
            this.miFileOpen.Click += new System.EventHandler(this.miFileOpen_Click);
            // 
            // miFileSave
            // 
            this.miFileSave.Image = ((System.Drawing.Image)(resources.GetObject("miFileSave.Image")));
            this.miFileSave.Name = "miFileSave";
            this.miFileSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.miFileSave.Size = new System.Drawing.Size(155, 22);
            this.miFileSave.Text = "Save";
            this.miFileSave.Click += new System.EventHandler(this.miFileSave_Click);
            // 
            // miFileSaveAs
            // 
            this.miFileSaveAs.Image = ((System.Drawing.Image)(resources.GetObject("miFileSaveAs.Image")));
            this.miFileSaveAs.Name = "miFileSaveAs";
            this.miFileSaveAs.Size = new System.Drawing.Size(180, 22);
            this.miFileSaveAs.Text = "Save As...";
            this.miFileSaveAs.Click += new System.EventHandler(this.miFileSaveAs_Click);
            // 
            // miFileOpenBrowser
            // 
            this.miFileOpenBrowser.Image = ((System.Drawing.Image)(resources.GetObject("miFileOpenBrowser.Image")));
            this.miFileOpenBrowser.Name = "miFileOpenBrowser";
            this.miFileOpenBrowser.Size = new System.Drawing.Size(180, 22);
            this.miFileOpenBrowser.Text = "Open Browser";
            this.miFileOpenBrowser.Click += new System.EventHandler(this.miFileOpenBrowser_Click);
            // 
            // miFileSep
            // 
            this.miFileSep.Name = "miFileSep";
            this.miFileSep.Size = new System.Drawing.Size(152, 6);
            // 
            // miFileExit
            // 
            this.miFileExit.Image = ((System.Drawing.Image)(resources.GetObject("miFileExit.Image")));
            this.miFileExit.Name = "miFileExit";
            this.miFileExit.Size = new System.Drawing.Size(155, 22);
            this.miFileExit.Text = "Exit";
            this.miFileExit.Click += new System.EventHandler(this.miFileExit_Click);
            // 
            // miEdit
            // 
            this.miEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miEditCut,
            this.miEditCopy,
            this.miEditPaste,
            this.miEditPasteSpecial,
            this.miEditSep1,
            this.miEditUndo,
            this.miEditRedo,
            this.miEditSep2,
            this.miEditPointer,
            this.miEditDelete});
            this.miEdit.Name = "miEdit";
            this.miEdit.Size = new System.Drawing.Size(39, 20);
            this.miEdit.Text = "&Edit";
            // 
            // miEditCut
            // 
            this.miEditCut.Image = ((System.Drawing.Image)(resources.GetObject("miEditCut.Image")));
            this.miEditCut.Name = "miEditCut";
            this.miEditCut.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.miEditCut.Size = new System.Drawing.Size(180, 22);
            this.miEditCut.Text = "Cut";
            this.miEditCut.Click += new System.EventHandler(this.miEditCut_Click);
            // 
            // miEditCopy
            // 
            this.miEditCopy.Image = ((System.Drawing.Image)(resources.GetObject("miEditCopy.Image")));
            this.miEditCopy.Name = "miEditCopy";
            this.miEditCopy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.miEditCopy.Size = new System.Drawing.Size(180, 22);
            this.miEditCopy.Text = "Copy";
            this.miEditCopy.Click += new System.EventHandler(this.miEditCopy_Click);
            // 
            // miEditPaste
            // 
            this.miEditPaste.Image = ((System.Drawing.Image)(resources.GetObject("miEditPaste.Image")));
            this.miEditPaste.Name = "miEditPaste";
            this.miEditPaste.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.miEditPaste.Size = new System.Drawing.Size(180, 22);
            this.miEditPaste.Text = "Paste";
            this.miEditPaste.Click += new System.EventHandler(this.miEditPaste_Click);
            // 
            // miEditSep1
            // 
            this.miEditSep1.Name = "miEditSep1";
            this.miEditSep1.Size = new System.Drawing.Size(177, 6);
            // 
            // miEditUndo
            // 
            this.miEditUndo.Image = ((System.Drawing.Image)(resources.GetObject("miEditUndo.Image")));
            this.miEditUndo.Name = "miEditUndo";
            this.miEditUndo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.miEditUndo.Size = new System.Drawing.Size(180, 22);
            this.miEditUndo.Text = "Undo";
            this.miEditUndo.Click += new System.EventHandler(this.miEditUndo_Click);
            // 
            // miEditRedo
            // 
            this.miEditRedo.Image = ((System.Drawing.Image)(resources.GetObject("miEditRedo.Image")));
            this.miEditRedo.Name = "miEditRedo";
            this.miEditRedo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
            this.miEditRedo.Size = new System.Drawing.Size(180, 22);
            this.miEditRedo.Text = "Redo";
            this.miEditRedo.Click += new System.EventHandler(this.miEditRedo_Click);
            // 
            // miEditSep2
            // 
            this.miEditSep2.Name = "miEditSep2";
            this.miEditSep2.Size = new System.Drawing.Size(177, 6);
            // 
            // miEditPointer
            // 
            this.miEditPointer.Image = ((System.Drawing.Image)(resources.GetObject("miEditPointer.Image")));
            this.miEditPointer.Name = "miEditPointer";
            this.miEditPointer.Size = new System.Drawing.Size(180, 22);
            this.miEditPointer.Text = "Pointer";
            this.miEditPointer.Click += new System.EventHandler(this.miEditPointer_Click);
            // 
            // miEditDelete
            // 
            this.miEditDelete.Image = ((System.Drawing.Image)(resources.GetObject("miEditDelete.Image")));
            this.miEditDelete.Name = "miEditDelete";
            this.miEditDelete.Size = new System.Drawing.Size(180, 22);
            this.miEditDelete.Text = "Delete";
            this.miEditDelete.Click += new System.EventHandler(this.miEditDelete_Click);
            // 
            // miTools
            // 
            this.miTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miToolsOptions});
            this.miTools.Name = "miTools";
            this.miTools.Size = new System.Drawing.Size(46, 20);
            this.miTools.Text = "&Tools";
            // 
            // miToolsOptions
            // 
            this.miToolsOptions.Image = ((System.Drawing.Image)(resources.GetObject("miToolsOptions.Image")));
            this.miToolsOptions.Name = "miToolsOptions";
            this.miToolsOptions.Size = new System.Drawing.Size(180, 22);
            this.miToolsOptions.Text = "Options...";
            this.miToolsOptions.Click += new System.EventHandler(this.miToolsOptions_Click);
            // 
            // miHelp
            // 
            this.miHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miHelpAbout});
            this.miHelp.Name = "miHelp";
            this.miHelp.Size = new System.Drawing.Size(44, 20);
            this.miHelp.Text = "&Help";
            // 
            // miHelpAbout
            // 
            this.miHelpAbout.Image = ((System.Drawing.Image)(resources.GetObject("miHelpAbout.Image")));
            this.miHelpAbout.Name = "miHelpAbout";
            this.miHelpAbout.Size = new System.Drawing.Size(107, 22);
            this.miHelpAbout.Text = "About";
            this.miHelpAbout.Click += new System.EventHandler(this.miHelpAbout_Click);
            // 
            // miEditPasteSpecial
            // 
            this.miEditPasteSpecial.Name = "miEditPasteSpecial";
            this.miEditPasteSpecial.Size = new System.Drawing.Size(180, 22);
            this.miEditPasteSpecial.Text = "Paste Special...";
            this.miEditPasteSpecial.Click += new System.EventHandler(this.miEditPasteSpecial_Click);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(309, 511);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.menuStrip);
            this.Controls.Add(this.statusStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(200, 300);
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Scheme Editor";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMain_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmMain_FormClosed);
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmMain_KeyDown);
            this.MouseEnter += new System.EventHandler(this.FrmMain_MouseEnter);
            this.Move += new System.EventHandler(this.FrmMain_Move);
            this.Resize += new System.EventHandler(this.FrmMain_Resize);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.pageComponents.ResumeLayout(false);
            this.pageProperties.ResumeLayout(false);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton btnFileNew;
        private System.Windows.Forms.ToolStripButton btnFileOpen;
        private System.Windows.Forms.ToolStripButton btnEditCut;
        private System.Windows.Forms.ToolStripButton btnEditCopy;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage pageProperties;
        private System.Windows.Forms.PropertyGrid propertyGrid;
        private System.Windows.Forms.ComboBox cbSchComp;
        private System.Windows.Forms.TabPage pageComponents;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.ToolStripSeparator sep1;
        private System.Windows.Forms.ToolStripSplitButton btnFileSave;
        private System.Windows.Forms.ToolStripMenuItem miFileSaveAs2;
        private System.Windows.Forms.ToolStripButton btnEditPaste;
        private System.Windows.Forms.ToolStripSeparator sep2;
        private System.Windows.Forms.ToolStripButton btnEditPointer;
        private System.Windows.Forms.ToolStripButton btnEditDelete;
        private System.Windows.Forms.ToolStripSeparator sep3;
        private System.Windows.Forms.ToolStripButton btnFileOpenBrowser;
        private System.Windows.Forms.ToolStripButton btnEditUndo;
        private System.Windows.Forms.ToolStripButton btnEditRedo;
        private System.Windows.Forms.OpenFileDialog ofdScheme;
        private System.Windows.Forms.SaveFileDialog sfdScheme;
        private System.Windows.Forms.ListView lvCompTypes;
        private System.Windows.Forms.ImageList ilCompTypes;
        private System.Windows.Forms.ColumnHeader colCompName;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem miFile;
        private System.Windows.Forms.ToolStripMenuItem miFileNew;
        private System.Windows.Forms.ToolStripMenuItem miFileOpen;
        private System.Windows.Forms.ToolStripMenuItem miFileSave;
        private System.Windows.Forms.ToolStripMenuItem miFileSaveAs;
        private System.Windows.Forms.ToolStripSeparator miFileSep;
        private System.Windows.Forms.ToolStripMenuItem miFileExit;
        private System.Windows.Forms.ToolStripMenuItem miEdit;
        private System.Windows.Forms.ToolStripMenuItem miTools;
        private System.Windows.Forms.ToolStripMenuItem miHelp;
        private System.Windows.Forms.ToolStripMenuItem miEditCut;
        private System.Windows.Forms.ToolStripMenuItem miEditCopy;
        private System.Windows.Forms.ToolStripMenuItem miEditPaste;
        private System.Windows.Forms.ToolStripSeparator miEditSep1;
        private System.Windows.Forms.ToolStripMenuItem miEditUndo;
        private System.Windows.Forms.ToolStripMenuItem miEditRedo;
        private System.Windows.Forms.ToolStripSeparator miEditSep2;
        private System.Windows.Forms.ToolStripMenuItem miEditPointer;
        private System.Windows.Forms.ToolStripMenuItem miEditDelete;
        private System.Windows.Forms.ToolStripMenuItem miToolsOptions;
        private System.Windows.Forms.ToolStripMenuItem miHelpAbout;
        private System.Windows.Forms.ToolStripMenuItem miFileOpenBrowser;
        private System.Windows.Forms.ToolStripMenuItem miEditPasteSpecial;
    }
}

