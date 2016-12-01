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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.menuMain = new System.Windows.Forms.MenuStrip();
            this.miFile = new System.Windows.Forms.ToolStripMenuItem();
            this.miFileNew = new System.Windows.Forms.ToolStripMenuItem();
            this.miFileOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.miFileSave = new System.Windows.Forms.ToolStripMenuItem();
            this.miFileSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.miFileSep = new System.Windows.Forms.ToolStripSeparator();
            this.miFileExit = new System.Windows.Forms.ToolStripMenuItem();
            this.miEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.miEditCut = new System.Windows.Forms.ToolStripMenuItem();
            this.miEditCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.miEditPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.miScheme = new System.Windows.Forms.ToolStripMenuItem();
            this.miSchemeAddStText = new System.Windows.Forms.ToolStripMenuItem();
            this.miSchemeAddDynText = new System.Windows.Forms.ToolStripMenuItem();
            this.miSchemeAddStPic = new System.Windows.Forms.ToolStripMenuItem();
            this.miSchemeAddDynPic = new System.Windows.Forms.ToolStripMenuItem();
            this.miSchemeCancelAddElem = new System.Windows.Forms.ToolStripMenuItem();
            this.miSchemeDelElem = new System.Windows.Forms.ToolStripMenuItem();
            this.miHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.miHelpAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.toolMain = new System.Windows.Forms.ToolStrip();
            this.btnFileNew = new System.Windows.Forms.ToolStripButton();
            this.btnFileOpen = new System.Windows.Forms.ToolStripButton();
            this.btnFileSave = new System.Windows.Forms.ToolStripButton();
            this.sep1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnEditCut = new System.Windows.Forms.ToolStripButton();
            this.btnEditCopy = new System.Windows.Forms.ToolStripButton();
            this.btnEditPaste = new System.Windows.Forms.ToolStripButton();
            this.sep2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnSchemeAddStText = new System.Windows.Forms.ToolStripButton();
            this.btnSchemeAddDynText = new System.Windows.Forms.ToolStripButton();
            this.btnSchemeAddStPic = new System.Windows.Forms.ToolStripButton();
            this.btnSchemeAddDynPic = new System.Windows.Forms.ToolStripButton();
            this.btnSchemeCancelAddElem = new System.Windows.Forms.ToolStripButton();
            this.btnSchemeDelElem = new System.Windows.Forms.ToolStripButton();
            this.propGrid = new System.Windows.Forms.PropertyGrid();
            this.pnlLeft = new System.Windows.Forms.Panel();
            this.lblNoSelObj = new System.Windows.Forms.Label();
            this.splitterVert = new System.Windows.Forms.Splitter();
            this.pnlRight = new System.Windows.Forms.Panel();
            this.webBrowser = new System.Windows.Forms.WebBrowser();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuMain.SuspendLayout();
            this.toolMain.SuspendLayout();
            this.pnlLeft.SuspendLayout();
            this.pnlRight.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuMain
            // 
            this.menuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miFile,
            this.miEdit,
            this.miScheme,
            this.miHelp});
            this.menuMain.Location = new System.Drawing.Point(0, 0);
            this.menuMain.Name = "menuMain";
            this.menuMain.Size = new System.Drawing.Size(584, 24);
            this.menuMain.TabIndex = 0;
            // 
            // miFile
            // 
            this.miFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miFileNew,
            this.miFileOpen,
            this.miFileSave,
            this.miFileSaveAs,
            this.miFileSep,
            this.miFileExit});
            this.miFile.Name = "miFile";
            this.miFile.Size = new System.Drawing.Size(48, 20);
            this.miFile.Text = "&Файл";
            // 
            // miFileNew
            // 
            this.miFileNew.Image = ((System.Drawing.Image)(resources.GetObject("miFileNew.Image")));
            this.miFileNew.Name = "miFileNew";
            this.miFileNew.Size = new System.Drawing.Size(162, 22);
            this.miFileNew.Text = "Создать";
            this.miFileNew.Click += new System.EventHandler(this.miFileNew_Click);
            // 
            // miFileOpen
            // 
            this.miFileOpen.Image = ((System.Drawing.Image)(resources.GetObject("miFileOpen.Image")));
            this.miFileOpen.Name = "miFileOpen";
            this.miFileOpen.Size = new System.Drawing.Size(162, 22);
            this.miFileOpen.Text = "Открыть...";
            this.miFileOpen.Click += new System.EventHandler(this.miFileOpen_Click);
            // 
            // miFileSave
            // 
            this.miFileSave.Image = ((System.Drawing.Image)(resources.GetObject("miFileSave.Image")));
            this.miFileSave.Name = "miFileSave";
            this.miFileSave.Size = new System.Drawing.Size(162, 22);
            this.miFileSave.Text = "Сохранить";
            this.miFileSave.Click += new System.EventHandler(this.miFileSave_Click);
            // 
            // miFileSaveAs
            // 
            this.miFileSaveAs.Name = "miFileSaveAs";
            this.miFileSaveAs.Size = new System.Drawing.Size(162, 22);
            this.miFileSaveAs.Text = "Сохранить как...";
            this.miFileSaveAs.Click += new System.EventHandler(this.miEditSaveAs_Click);
            // 
            // miFileSep
            // 
            this.miFileSep.Name = "miFileSep";
            this.miFileSep.Size = new System.Drawing.Size(159, 6);
            // 
            // miFileExit
            // 
            this.miFileExit.Name = "miFileExit";
            this.miFileExit.Size = new System.Drawing.Size(162, 22);
            this.miFileExit.Text = "Выход";
            this.miFileExit.Click += new System.EventHandler(this.miFileExit_Click);
            // 
            // miEdit
            // 
            this.miEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miEditCut,
            this.miEditCopy,
            this.miEditPaste});
            this.miEdit.Name = "miEdit";
            this.miEdit.Size = new System.Drawing.Size(59, 20);
            this.miEdit.Text = "&Правка";
            // 
            // miEditCut
            // 
            this.miEditCut.Image = ((System.Drawing.Image)(resources.GetObject("miEditCut.Image")));
            this.miEditCut.Name = "miEditCut";
            this.miEditCut.Size = new System.Drawing.Size(139, 22);
            this.miEditCut.Text = "Вырезать";
            this.miEditCut.Click += new System.EventHandler(this.miEditCut_Click);
            // 
            // miEditCopy
            // 
            this.miEditCopy.Image = ((System.Drawing.Image)(resources.GetObject("miEditCopy.Image")));
            this.miEditCopy.Name = "miEditCopy";
            this.miEditCopy.Size = new System.Drawing.Size(139, 22);
            this.miEditCopy.Text = "Копировать";
            this.miEditCopy.Click += new System.EventHandler(this.miEditCopy_Click);
            // 
            // miEditPaste
            // 
            this.miEditPaste.Image = ((System.Drawing.Image)(resources.GetObject("miEditPaste.Image")));
            this.miEditPaste.Name = "miEditPaste";
            this.miEditPaste.Size = new System.Drawing.Size(139, 22);
            this.miEditPaste.Text = "Вставить";
            this.miEditPaste.Click += new System.EventHandler(this.miEditPaste_Click);
            // 
            // miScheme
            // 
            this.miScheme.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miSchemeAddStText,
            this.miSchemeAddDynText,
            this.miSchemeAddStPic,
            this.miSchemeAddDynPic,
            this.miSchemeCancelAddElem,
            this.miSchemeDelElem});
            this.miScheme.Name = "miScheme";
            this.miScheme.Size = new System.Drawing.Size(53, 20);
            this.miScheme.Text = "С&хема";
            // 
            // miSchemeAddStText
            // 
            this.miSchemeAddStText.Image = ((System.Drawing.Image)(resources.GetObject("miSchemeAddStText.Image")));
            this.miSchemeAddStText.Name = "miSchemeAddStText";
            this.miSchemeAddStText.Size = new System.Drawing.Size(260, 22);
            this.miSchemeAddStText.Text = "Добавить статическую надпись";
            this.miSchemeAddStText.Click += new System.EventHandler(this.miSchemeAddElem_Click);
            // 
            // miSchemeAddDynText
            // 
            this.miSchemeAddDynText.Image = ((System.Drawing.Image)(resources.GetObject("miSchemeAddDynText.Image")));
            this.miSchemeAddDynText.Name = "miSchemeAddDynText";
            this.miSchemeAddDynText.Size = new System.Drawing.Size(260, 22);
            this.miSchemeAddDynText.Text = "Добавить динамическую надпись";
            this.miSchemeAddDynText.Click += new System.EventHandler(this.miSchemeAddElem_Click);
            // 
            // miSchemeAddStPic
            // 
            this.miSchemeAddStPic.Image = ((System.Drawing.Image)(resources.GetObject("miSchemeAddStPic.Image")));
            this.miSchemeAddStPic.Name = "miSchemeAddStPic";
            this.miSchemeAddStPic.Size = new System.Drawing.Size(260, 22);
            this.miSchemeAddStPic.Text = "Добавить статический рисунок";
            this.miSchemeAddStPic.Click += new System.EventHandler(this.miSchemeAddElem_Click);
            // 
            // miSchemeAddDynPic
            // 
            this.miSchemeAddDynPic.Image = ((System.Drawing.Image)(resources.GetObject("miSchemeAddDynPic.Image")));
            this.miSchemeAddDynPic.Name = "miSchemeAddDynPic";
            this.miSchemeAddDynPic.Size = new System.Drawing.Size(260, 22);
            this.miSchemeAddDynPic.Text = "Добавить динамический рисунок";
            this.miSchemeAddDynPic.Click += new System.EventHandler(this.miSchemeAddElem_Click);
            // 
            // miSchemeCancelAddElem
            // 
            this.miSchemeCancelAddElem.Image = ((System.Drawing.Image)(resources.GetObject("miSchemeCancelAddElem.Image")));
            this.miSchemeCancelAddElem.Name = "miSchemeCancelAddElem";
            this.miSchemeCancelAddElem.Size = new System.Drawing.Size(260, 22);
            this.miSchemeCancelAddElem.Text = "Отменить добавление элемента";
            this.miSchemeCancelAddElem.Click += new System.EventHandler(this.miSchemeCancelAddElem_Click);
            // 
            // miSchemeDelElem
            // 
            this.miSchemeDelElem.Image = ((System.Drawing.Image)(resources.GetObject("miSchemeDelElem.Image")));
            this.miSchemeDelElem.Name = "miSchemeDelElem";
            this.miSchemeDelElem.Size = new System.Drawing.Size(260, 22);
            this.miSchemeDelElem.Text = "Удалить выбранный элемент";
            this.miSchemeDelElem.Click += new System.EventHandler(this.miSchemeDelElem_Click);
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
            this.miHelpAbout.Name = "miHelpAbout";
            this.miHelpAbout.Size = new System.Drawing.Size(149, 22);
            this.miHelpAbout.Text = "О программе";
            this.miHelpAbout.Click += new System.EventHandler(this.miHelpAbout_Click);
            // 
            // toolMain
            // 
            this.toolMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnFileNew,
            this.btnFileOpen,
            this.btnFileSave,
            this.sep1,
            this.btnEditCut,
            this.btnEditCopy,
            this.btnEditPaste,
            this.sep2,
            this.btnSchemeAddStText,
            this.btnSchemeAddDynText,
            this.btnSchemeAddStPic,
            this.btnSchemeAddDynPic,
            this.btnSchemeCancelAddElem,
            this.btnSchemeDelElem});
            this.toolMain.Location = new System.Drawing.Point(0, 24);
            this.toolMain.Name = "toolMain";
            this.toolMain.Size = new System.Drawing.Size(584, 25);
            this.toolMain.TabIndex = 1;
            // 
            // btnFileNew
            // 
            this.btnFileNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnFileNew.Image = ((System.Drawing.Image)(resources.GetObject("btnFileNew.Image")));
            this.btnFileNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnFileNew.Name = "btnFileNew";
            this.btnFileNew.Size = new System.Drawing.Size(23, 22);
            this.btnFileNew.ToolTipText = "Создать новую схему";
            this.btnFileNew.Click += new System.EventHandler(this.miFileNew_Click);
            // 
            // btnFileOpen
            // 
            this.btnFileOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnFileOpen.Image = ((System.Drawing.Image)(resources.GetObject("btnFileOpen.Image")));
            this.btnFileOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnFileOpen.Name = "btnFileOpen";
            this.btnFileOpen.Size = new System.Drawing.Size(23, 22);
            this.btnFileOpen.ToolTipText = "Открыть схему";
            this.btnFileOpen.Click += new System.EventHandler(this.miFileOpen_Click);
            // 
            // btnFileSave
            // 
            this.btnFileSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnFileSave.Image = ((System.Drawing.Image)(resources.GetObject("btnFileSave.Image")));
            this.btnFileSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnFileSave.Name = "btnFileSave";
            this.btnFileSave.Size = new System.Drawing.Size(23, 22);
            this.btnFileSave.ToolTipText = "Сохранить схему";
            this.btnFileSave.Click += new System.EventHandler(this.miFileSave_Click);
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
            this.btnEditCut.ToolTipText = "Вырезать элемент схемы";
            this.btnEditCut.Click += new System.EventHandler(this.miEditCut_Click);
            // 
            // btnEditCopy
            // 
            this.btnEditCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnEditCopy.Image = ((System.Drawing.Image)(resources.GetObject("btnEditCopy.Image")));
            this.btnEditCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnEditCopy.Name = "btnEditCopy";
            this.btnEditCopy.Size = new System.Drawing.Size(23, 22);
            this.btnEditCopy.ToolTipText = "Копировать элемент схемы";
            this.btnEditCopy.Click += new System.EventHandler(this.miEditCopy_Click);
            // 
            // btnEditPaste
            // 
            this.btnEditPaste.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnEditPaste.Image = ((System.Drawing.Image)(resources.GetObject("btnEditPaste.Image")));
            this.btnEditPaste.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnEditPaste.Name = "btnEditPaste";
            this.btnEditPaste.Size = new System.Drawing.Size(23, 22);
            this.btnEditPaste.ToolTipText = "Вставить элемент схемы";
            this.btnEditPaste.Click += new System.EventHandler(this.miEditPaste_Click);
            // 
            // sep2
            // 
            this.sep2.Name = "sep2";
            this.sep2.Size = new System.Drawing.Size(6, 25);
            // 
            // btnSchemeAddStText
            // 
            this.btnSchemeAddStText.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSchemeAddStText.Image = ((System.Drawing.Image)(resources.GetObject("btnSchemeAddStText.Image")));
            this.btnSchemeAddStText.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSchemeAddStText.Name = "btnSchemeAddStText";
            this.btnSchemeAddStText.Size = new System.Drawing.Size(23, 22);
            this.btnSchemeAddStText.ToolTipText = "Добавить статическую надпись";
            this.btnSchemeAddStText.Click += new System.EventHandler(this.miSchemeAddElem_Click);
            // 
            // btnSchemeAddDynText
            // 
            this.btnSchemeAddDynText.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSchemeAddDynText.Image = ((System.Drawing.Image)(resources.GetObject("btnSchemeAddDynText.Image")));
            this.btnSchemeAddDynText.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSchemeAddDynText.Name = "btnSchemeAddDynText";
            this.btnSchemeAddDynText.Size = new System.Drawing.Size(23, 22);
            this.btnSchemeAddDynText.ToolTipText = "Добавить динамическую надпись";
            this.btnSchemeAddDynText.Click += new System.EventHandler(this.miSchemeAddElem_Click);
            // 
            // btnSchemeAddStPic
            // 
            this.btnSchemeAddStPic.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSchemeAddStPic.Image = ((System.Drawing.Image)(resources.GetObject("btnSchemeAddStPic.Image")));
            this.btnSchemeAddStPic.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSchemeAddStPic.Name = "btnSchemeAddStPic";
            this.btnSchemeAddStPic.Size = new System.Drawing.Size(23, 22);
            this.btnSchemeAddStPic.ToolTipText = "Добавить статический рисунок";
            this.btnSchemeAddStPic.Click += new System.EventHandler(this.miSchemeAddElem_Click);
            // 
            // btnSchemeAddDynPic
            // 
            this.btnSchemeAddDynPic.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSchemeAddDynPic.Image = ((System.Drawing.Image)(resources.GetObject("btnSchemeAddDynPic.Image")));
            this.btnSchemeAddDynPic.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSchemeAddDynPic.Name = "btnSchemeAddDynPic";
            this.btnSchemeAddDynPic.Size = new System.Drawing.Size(23, 22);
            this.btnSchemeAddDynPic.ToolTipText = "Добавить динамический рисунок";
            this.btnSchemeAddDynPic.Click += new System.EventHandler(this.miSchemeAddElem_Click);
            // 
            // btnSchemeCancelAddElem
            // 
            this.btnSchemeCancelAddElem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSchemeCancelAddElem.Image = ((System.Drawing.Image)(resources.GetObject("btnSchemeCancelAddElem.Image")));
            this.btnSchemeCancelAddElem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSchemeCancelAddElem.Name = "btnSchemeCancelAddElem";
            this.btnSchemeCancelAddElem.Size = new System.Drawing.Size(23, 22);
            this.btnSchemeCancelAddElem.ToolTipText = "Отменить добавление элемента";
            this.btnSchemeCancelAddElem.Click += new System.EventHandler(this.miSchemeCancelAddElem_Click);
            // 
            // btnSchemeDelElem
            // 
            this.btnSchemeDelElem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSchemeDelElem.Image = ((System.Drawing.Image)(resources.GetObject("btnSchemeDelElem.Image")));
            this.btnSchemeDelElem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSchemeDelElem.Name = "btnSchemeDelElem";
            this.btnSchemeDelElem.Size = new System.Drawing.Size(23, 22);
            this.btnSchemeDelElem.ToolTipText = "Удалить выбранный элемент";
            this.btnSchemeDelElem.Click += new System.EventHandler(this.miSchemeDelElem_Click);
            // 
            // propGrid
            // 
            this.propGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propGrid.Location = new System.Drawing.Point(0, 0);
            this.propGrid.Name = "propGrid";
            this.propGrid.Size = new System.Drawing.Size(230, 341);
            this.propGrid.TabIndex = 0;
            this.propGrid.ToolbarVisible = false;
            this.propGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propGrid_PropertyValueChanged);
            // 
            // pnlLeft
            // 
            this.pnlLeft.Controls.Add(this.lblNoSelObj);
            this.pnlLeft.Controls.Add(this.propGrid);
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlLeft.Location = new System.Drawing.Point(0, 49);
            this.pnlLeft.MinimumSize = new System.Drawing.Size(150, 0);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Size = new System.Drawing.Size(230, 341);
            this.pnlLeft.TabIndex = 2;
            // 
            // lblNoSelObj
            // 
            this.lblNoSelObj.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblNoSelObj.BackColor = System.Drawing.SystemColors.Window;
            this.lblNoSelObj.ForeColor = System.Drawing.Color.DimGray;
            this.lblNoSelObj.Location = new System.Drawing.Point(12, 10);
            this.lblNoSelObj.Name = "lblNoSelObj";
            this.lblNoSelObj.Size = new System.Drawing.Size(212, 40);
            this.lblNoSelObj.TabIndex = 1;
            this.lblNoSelObj.Text = "Выберите элемент для редактирования его свойств";
            // 
            // splitterVert
            // 
            this.splitterVert.Location = new System.Drawing.Point(230, 49);
            this.splitterVert.MinExtra = 150;
            this.splitterVert.MinSize = 150;
            this.splitterVert.Name = "splitterVert";
            this.splitterVert.Size = new System.Drawing.Size(3, 341);
            this.splitterVert.TabIndex = 3;
            this.splitterVert.TabStop = false;
            // 
            // pnlRight
            // 
            this.pnlRight.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.pnlRight.Controls.Add(this.webBrowser);
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRight.ForeColor = System.Drawing.SystemColors.ControlText;
            this.pnlRight.Location = new System.Drawing.Point(233, 49);
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Padding = new System.Windows.Forms.Padding(1);
            this.pnlRight.Size = new System.Drawing.Size(351, 341);
            this.pnlRight.TabIndex = 4;
            // 
            // webBrowser
            // 
            this.webBrowser.AllowWebBrowserDrop = false;
            this.webBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser.IsWebBrowserContextMenuEnabled = false;
            this.webBrowser.Location = new System.Drawing.Point(1, 1);
            this.webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser.Name = "webBrowser";
            this.webBrowser.Size = new System.Drawing.Size(349, 339);
            this.webBrowser.TabIndex = 0;
            this.webBrowser.WebBrowserShortcutsEnabled = false;
            this.webBrowser.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(this.webBrowser_Navigating);
            this.webBrowser.NewWindow += new System.ComponentModel.CancelEventHandler(this.webBrowser_NewWindow);
            // 
            // openFileDialog
            // 
            this.openFileDialog.DefaultExt = "*.sch";
            this.openFileDialog.Filter = "Схемы (*.sch)|*.sch|Все файлы (*.*)|*.*";
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.DefaultExt = "*.sch";
            this.saveFileDialog.Filter = "Схемы (*.sch)|*.sch|Все файлы (*.*)|*.*";
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
            this.statusStrip.Location = new System.Drawing.Point(0, 390);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(584, 22);
            this.statusStrip.TabIndex = 5;
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(52, 17);
            this.lblStatus.Text = "lblStatus";
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 412);
            this.Controls.Add(this.pnlRight);
            this.Controls.Add(this.splitterVert);
            this.Controls.Add(this.pnlLeft);
            this.Controls.Add(this.toolMain);
            this.Controls.Add(this.menuMain);
            this.Controls.Add(this.statusStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuMain;
            this.MinimumSize = new System.Drawing.Size(600, 450);
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SCADA-Редактор схем";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMain_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmMain_FormClosed);
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.menuMain.ResumeLayout(false);
            this.menuMain.PerformLayout();
            this.toolMain.ResumeLayout(false);
            this.toolMain.PerformLayout();
            this.pnlLeft.ResumeLayout(false);
            this.pnlRight.ResumeLayout(false);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuMain;
        private System.Windows.Forms.ToolStripMenuItem miFile;
        private System.Windows.Forms.ToolStrip toolMain;
        private System.Windows.Forms.PropertyGrid propGrid;
        private System.Windows.Forms.Panel pnlLeft;
        private System.Windows.Forms.Splitter splitterVert;
        private System.Windows.Forms.Panel pnlRight;
        private System.Windows.Forms.WebBrowser webBrowser;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.ToolStripMenuItem miFileOpen;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.ToolStripMenuItem miFileSave;
        private System.Windows.Forms.ToolStripMenuItem miFileSaveAs;
        private System.Windows.Forms.ToolStripSeparator miFileSep;
        private System.Windows.Forms.ToolStripMenuItem miFileExit;
        private System.Windows.Forms.ToolStripButton btnFileOpen;
        private System.Windows.Forms.ToolStripButton btnFileSave;
        private System.Windows.Forms.ToolStripSeparator sep1;
        private System.Windows.Forms.ToolStripMenuItem miEdit;
        private System.Windows.Forms.ToolStripMenuItem miEditCut;
        private System.Windows.Forms.ToolStripMenuItem miEditCopy;
        private System.Windows.Forms.ToolStripMenuItem miEditPaste;
        private System.Windows.Forms.ToolStripButton btnEditCut;
        private System.Windows.Forms.ToolStripButton btnEditCopy;
        private System.Windows.Forms.ToolStripButton btnEditPaste;
        private System.Windows.Forms.ToolStripSeparator sep2;
        private System.Windows.Forms.ToolStripMenuItem miScheme;
        private System.Windows.Forms.ToolStripMenuItem miSchemeAddStText;
        private System.Windows.Forms.ToolStripMenuItem miSchemeAddDynText;
        private System.Windows.Forms.ToolStripMenuItem miSchemeAddStPic;
        private System.Windows.Forms.ToolStripMenuItem miSchemeAddDynPic;
        private System.Windows.Forms.ToolStripMenuItem miSchemeCancelAddElem;
        private System.Windows.Forms.ToolStripMenuItem miSchemeDelElem;
        private System.Windows.Forms.ToolStripButton btnSchemeAddStText;
        private System.Windows.Forms.ToolStripButton btnSchemeAddDynText;
        private System.Windows.Forms.ToolStripButton btnSchemeAddStPic;
        private System.Windows.Forms.ToolStripButton btnSchemeAddDynPic;
        private System.Windows.Forms.ToolStripButton btnSchemeCancelAddElem;
        private System.Windows.Forms.ToolStripButton btnSchemeDelElem;
        private System.Windows.Forms.ToolStripMenuItem miFileNew;
        private System.Windows.Forms.ToolStripButton btnFileNew;
        private System.Windows.Forms.Label lblNoSelObj;
        private System.Windows.Forms.ToolStripMenuItem miHelp;
        private System.Windows.Forms.ToolStripMenuItem miHelpAbout;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
    }
}

