namespace ScadaTableEditor
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.menuMain = new System.Windows.Forms.MenuStrip();
            this.miFile = new System.Windows.Forms.ToolStripMenuItem();
            this.miFileNew = new System.Windows.Forms.ToolStripMenuItem();
            this.miFileOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.miFileSave = new System.Windows.Forms.ToolStripMenuItem();
            this.miFileSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.miFileSep = new System.Windows.Forms.ToolStripSeparator();
            this.miFileExit = new System.Windows.Forms.ToolStripMenuItem();
            this.miSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.miSettingsParams = new System.Windows.Forms.ToolStripMenuItem();
            this.miHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.miHelpAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.toolMain = new System.Windows.Forms.ToolStrip();
            this.btnFileNew = new System.Windows.Forms.ToolStripButton();
            this.btnFileOpen = new System.Windows.Forms.ToolStripButton();
            this.btnFileSave = new System.Windows.Forms.ToolStripButton();
            this.sep1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnSettingsParams = new System.Windows.Forms.ToolStripButton();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.statusMain = new System.Windows.Forms.StatusStrip();
            this.lblBaseDATDir = new System.Windows.Forms.ToolStripStatusLabel();
            this.pnlLeft = new System.Windows.Forms.Panel();
            this.gbBase = new System.Windows.Forms.GroupBox();
            this.btnRefreshBase = new System.Windows.Forms.Button();
            this.btnFilterCnlsByObj = new System.Windows.Forms.Button();
            this.btnFilterCnlsByKP = new System.Windows.Forms.Button();
            this.rbCtrlCnls = new System.Windows.Forms.RadioButton();
            this.rbInCnls = new System.Windows.Forms.RadioButton();
            this.dgvCnls = new System.Windows.Forms.DataGridView();
            this.colCnlNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCnlName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bsCnls = new System.Windows.Forms.BindingSource(this.components);
            this.cbCnlFilter = new System.Windows.Forms.ComboBox();
            this.lblCnlFilterByKP = new System.Windows.Forms.Label();
            this.lblCnlFilterByObj = new System.Windows.Forms.Label();
            this.splVert = new System.Windows.Forms.Splitter();
            this.pnlRight = new System.Windows.Forms.Panel();
            this.gbTableView = new System.Windows.Forms.GroupBox();
            this.btnAddItem = new System.Windows.Forms.Button();
            this.btnShowItemInfo = new System.Windows.Forms.Button();
            this.dgvItems = new System.Windows.Forms.DataGridView();
            this.colItemCnlNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colItemCtrlCnlNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colItemCaption = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colItemHidden = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.bsItems = new System.Windows.Forms.BindingSource(this.components);
            this.lblTableItems = new System.Windows.Forms.Label();
            this.btnDeleteItem = new System.Windows.Forms.Button();
            this.lblTableTitle = new System.Windows.Forms.Label();
            this.btnMoveDownItem = new System.Windows.Forms.Button();
            this.txtTableTitle = new System.Windows.Forms.TextBox();
            this.btnMoveUpItem = new System.Windows.Forms.Button();
            this.btnAddEmptyItem = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.menuMain.SuspendLayout();
            this.toolMain.SuspendLayout();
            this.statusMain.SuspendLayout();
            this.pnlLeft.SuspendLayout();
            this.gbBase.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCnls)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsCnls)).BeginInit();
            this.pnlRight.SuspendLayout();
            this.gbTableView.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItems)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsItems)).BeginInit();
            this.SuspendLayout();
            // 
            // menuMain
            // 
            this.menuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miFile,
            this.miSettings,
            this.miHelp});
            this.menuMain.Location = new System.Drawing.Point(0, 0);
            this.menuMain.Name = "menuMain";
            this.menuMain.Size = new System.Drawing.Size(764, 24);
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
            this.miFileNew.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.miFileNew.Size = new System.Drawing.Size(173, 22);
            this.miFileNew.Text = "Создать";
            this.miFileNew.Click += new System.EventHandler(this.miFileNew_Click);
            // 
            // miFileOpen
            // 
            this.miFileOpen.Image = ((System.Drawing.Image)(resources.GetObject("miFileOpen.Image")));
            this.miFileOpen.Name = "miFileOpen";
            this.miFileOpen.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.miFileOpen.Size = new System.Drawing.Size(173, 22);
            this.miFileOpen.Text = "Открыть...";
            this.miFileOpen.Click += new System.EventHandler(this.miFileOpen_Click);
            // 
            // miFileSave
            // 
            this.miFileSave.Image = ((System.Drawing.Image)(resources.GetObject("miFileSave.Image")));
            this.miFileSave.Name = "miFileSave";
            this.miFileSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.miFileSave.Size = new System.Drawing.Size(173, 22);
            this.miFileSave.Text = "Сохранить";
            this.miFileSave.Click += new System.EventHandler(this.miFileSave_Click);
            // 
            // miFileSaveAs
            // 
            this.miFileSaveAs.Name = "miFileSaveAs";
            this.miFileSaveAs.Size = new System.Drawing.Size(173, 22);
            this.miFileSaveAs.Text = "Сохранить как...";
            this.miFileSaveAs.Click += new System.EventHandler(this.miFileSaveAs_Click);
            // 
            // miFileSep
            // 
            this.miFileSep.Name = "miFileSep";
            this.miFileSep.Size = new System.Drawing.Size(170, 6);
            // 
            // miFileExit
            // 
            this.miFileExit.Name = "miFileExit";
            this.miFileExit.Size = new System.Drawing.Size(173, 22);
            this.miFileExit.Text = "Выход";
            this.miFileExit.Click += new System.EventHandler(this.miFileExit_Click);
            // 
            // miSettings
            // 
            this.miSettings.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miSettingsParams});
            this.miSettings.Name = "miSettings";
            this.miSettings.Size = new System.Drawing.Size(78, 20);
            this.miSettings.Text = "&Настройка";
            // 
            // miSettingsParams
            // 
            this.miSettingsParams.Image = ((System.Drawing.Image)(resources.GetObject("miSettingsParams.Image")));
            this.miSettingsParams.Name = "miSettingsParams";
            this.miSettingsParams.Size = new System.Drawing.Size(147, 22);
            this.miSettingsParams.Text = "Параметры...";
            this.miSettingsParams.Click += new System.EventHandler(this.miSettingsParams_Click);
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
            this.btnSettingsParams});
            this.toolMain.Location = new System.Drawing.Point(0, 24);
            this.toolMain.Name = "toolMain";
            this.toolMain.Size = new System.Drawing.Size(764, 25);
            this.toolMain.TabIndex = 1;
            // 
            // btnFileNew
            // 
            this.btnFileNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnFileNew.Image = ((System.Drawing.Image)(resources.GetObject("btnFileNew.Image")));
            this.btnFileNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnFileNew.Name = "btnFileNew";
            this.btnFileNew.Size = new System.Drawing.Size(23, 22);
            this.btnFileNew.ToolTipText = "Создать новое табличное представление";
            this.btnFileNew.Click += new System.EventHandler(this.miFileNew_Click);
            // 
            // btnFileOpen
            // 
            this.btnFileOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnFileOpen.Image = ((System.Drawing.Image)(resources.GetObject("btnFileOpen.Image")));
            this.btnFileOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnFileOpen.Name = "btnFileOpen";
            this.btnFileOpen.Size = new System.Drawing.Size(23, 22);
            this.btnFileOpen.ToolTipText = "Открыть табличное представление";
            this.btnFileOpen.Click += new System.EventHandler(this.miFileOpen_Click);
            // 
            // btnFileSave
            // 
            this.btnFileSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnFileSave.Image = ((System.Drawing.Image)(resources.GetObject("btnFileSave.Image")));
            this.btnFileSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnFileSave.Name = "btnFileSave";
            this.btnFileSave.Size = new System.Drawing.Size(23, 22);
            this.btnFileSave.ToolTipText = "Сохранить табличное представление";
            this.btnFileSave.Click += new System.EventHandler(this.miFileSave_Click);
            // 
            // sep1
            // 
            this.sep1.Name = "sep1";
            this.sep1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnSettingsParams
            // 
            this.btnSettingsParams.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSettingsParams.Image = ((System.Drawing.Image)(resources.GetObject("btnSettingsParams.Image")));
            this.btnSettingsParams.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSettingsParams.Name = "btnSettingsParams";
            this.btnSettingsParams.Size = new System.Drawing.Size(23, 22);
            this.btnSettingsParams.ToolTipText = "Параметры";
            this.btnSettingsParams.Click += new System.EventHandler(this.miSettingsParams_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.DefaultExt = "*.tbl";
            this.openFileDialog.Filter = "Табличные представления (*.tbl; *.ofm)|*.tbl;*.ofm|Все файлы (*.*)|*.*";
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.DefaultExt = "*.tbl";
            this.saveFileDialog.Filter = "Табличные представления (*.tbl)|*.tbl|Все файлы (*.*)|*.*";
            // 
            // statusMain
            // 
            this.statusMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblBaseDATDir});
            this.statusMain.Location = new System.Drawing.Point(0, 520);
            this.statusMain.Name = "statusMain";
            this.statusMain.Size = new System.Drawing.Size(764, 22);
            this.statusMain.TabIndex = 5;
            // 
            // lblBaseDATDir
            // 
            this.lblBaseDATDir.Name = "lblBaseDATDir";
            this.lblBaseDATDir.Size = new System.Drawing.Size(117, 17);
            this.lblBaseDATDir.Text = "C:\\SCADA\\BaseDAT\\";
            // 
            // pnlLeft
            // 
            this.pnlLeft.Controls.Add(this.gbBase);
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlLeft.Location = new System.Drawing.Point(0, 49);
            this.pnlLeft.Margin = new System.Windows.Forms.Padding(0);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Size = new System.Drawing.Size(320, 471);
            this.pnlLeft.TabIndex = 2;
            // 
            // gbBase
            // 
            this.gbBase.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbBase.Controls.Add(this.btnRefreshBase);
            this.gbBase.Controls.Add(this.btnFilterCnlsByObj);
            this.gbBase.Controls.Add(this.btnFilterCnlsByKP);
            this.gbBase.Controls.Add(this.rbCtrlCnls);
            this.gbBase.Controls.Add(this.rbInCnls);
            this.gbBase.Controls.Add(this.dgvCnls);
            this.gbBase.Controls.Add(this.cbCnlFilter);
            this.gbBase.Controls.Add(this.lblCnlFilterByKP);
            this.gbBase.Controls.Add(this.lblCnlFilterByObj);
            this.gbBase.Location = new System.Drawing.Point(3, 0);
            this.gbBase.Name = "gbBase";
            this.gbBase.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbBase.Size = new System.Drawing.Size(317, 468);
            this.gbBase.TabIndex = 0;
            this.gbBase.TabStop = false;
            this.gbBase.Text = "База конфигурации";
            // 
            // btnRefreshBase
            // 
            this.btnRefreshBase.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefreshBase.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnRefreshBase.Image = ((System.Drawing.Image)(resources.GetObject("btnRefreshBase.Image")));
            this.btnRefreshBase.Location = new System.Drawing.Point(281, 32);
            this.btnRefreshBase.Name = "btnRefreshBase";
            this.btnRefreshBase.Size = new System.Drawing.Size(23, 22);
            this.btnRefreshBase.TabIndex = 5;
            this.toolTip.SetToolTip(this.btnRefreshBase, "Обновить данные базы конфигурации");
            this.btnRefreshBase.UseVisualStyleBackColor = true;
            this.btnRefreshBase.Click += new System.EventHandler(this.btnRefreshBase_Click);
            // 
            // btnFilterCnlsByObj
            // 
            this.btnFilterCnlsByObj.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFilterCnlsByObj.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnFilterCnlsByObj.Image = ((System.Drawing.Image)(resources.GetObject("btnFilterCnlsByObj.Image")));
            this.btnFilterCnlsByObj.Location = new System.Drawing.Point(223, 32);
            this.btnFilterCnlsByObj.Name = "btnFilterCnlsByObj";
            this.btnFilterCnlsByObj.Size = new System.Drawing.Size(23, 22);
            this.btnFilterCnlsByObj.TabIndex = 3;
            this.toolTip.SetToolTip(this.btnFilterCnlsByObj, "Фильтровать каналы по объектам");
            this.btnFilterCnlsByObj.UseVisualStyleBackColor = true;
            this.btnFilterCnlsByObj.Click += new System.EventHandler(this.btnGroupCnlsByObj_Click);
            // 
            // btnFilterCnlsByKP
            // 
            this.btnFilterCnlsByKP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFilterCnlsByKP.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnFilterCnlsByKP.Image = ((System.Drawing.Image)(resources.GetObject("btnFilterCnlsByKP.Image")));
            this.btnFilterCnlsByKP.Location = new System.Drawing.Point(252, 32);
            this.btnFilterCnlsByKP.Name = "btnFilterCnlsByKP";
            this.btnFilterCnlsByKP.Size = new System.Drawing.Size(23, 22);
            this.btnFilterCnlsByKP.TabIndex = 4;
            this.toolTip.SetToolTip(this.btnFilterCnlsByKP, "Фильтровать каналы по КП");
            this.btnFilterCnlsByKP.UseVisualStyleBackColor = true;
            this.btnFilterCnlsByKP.Click += new System.EventHandler(this.btnGroupCnlsByKP_Click);
            // 
            // rbCtrlCnls
            // 
            this.rbCtrlCnls.AutoSize = true;
            this.rbCtrlCnls.Location = new System.Drawing.Point(129, 65);
            this.rbCtrlCnls.Name = "rbCtrlCnls";
            this.rbCtrlCnls.Size = new System.Drawing.Size(126, 17);
            this.rbCtrlCnls.TabIndex = 7;
            this.rbCtrlCnls.TabStop = true;
            this.rbCtrlCnls.Text = "Каналы управления";
            this.rbCtrlCnls.UseVisualStyleBackColor = true;
            this.rbCtrlCnls.CheckedChanged += new System.EventHandler(this.rbCnls_CheckedChanged);
            // 
            // rbInCnls
            // 
            this.rbInCnls.AutoSize = true;
            this.rbInCnls.Checked = true;
            this.rbInCnls.Location = new System.Drawing.Point(13, 65);
            this.rbInCnls.Name = "rbInCnls";
            this.rbInCnls.Size = new System.Drawing.Size(110, 17);
            this.rbInCnls.TabIndex = 6;
            this.rbInCnls.TabStop = true;
            this.rbInCnls.Text = "Входные каналы";
            this.rbInCnls.UseVisualStyleBackColor = true;
            this.rbInCnls.CheckedChanged += new System.EventHandler(this.rbCnls_CheckedChanged);
            // 
            // dgvCnls
            // 
            this.dgvCnls.AllowUserToResizeRows = false;
            this.dgvCnls.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvCnls.AutoGenerateColumns = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvCnls.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvCnls.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCnls.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colCnlNum,
            this.colCnlName});
            this.dgvCnls.DataSource = this.bsCnls;
            this.dgvCnls.Location = new System.Drawing.Point(13, 86);
            this.dgvCnls.Margin = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.dgvCnls.Name = "dgvCnls";
            this.dgvCnls.ReadOnly = true;
            this.dgvCnls.RowHeadersVisible = false;
            this.dgvCnls.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCnls.Size = new System.Drawing.Size(291, 369);
            this.dgvCnls.TabIndex = 8;
            this.dgvCnls.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCnls_CellDoubleClick);
            this.dgvCnls.SelectionChanged += new System.EventHandler(this.dgvCnls_SelectionChanged);
            this.dgvCnls.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvCnls_KeyDown);
            // 
            // colCnlNum
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colCnlNum.DefaultCellStyle = dataGridViewCellStyle2;
            this.colCnlNum.HeaderText = "№";
            this.colCnlNum.Name = "colCnlNum";
            this.colCnlNum.ReadOnly = true;
            this.colCnlNum.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colCnlNum.Width = 50;
            // 
            // colCnlName
            // 
            this.colCnlName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colCnlName.DataPropertyName = "Name";
            this.colCnlName.HeaderText = "Наименование";
            this.colCnlName.Name = "colCnlName";
            this.colCnlName.ReadOnly = true;
            this.colCnlName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // bsCnls
            // 
            this.bsCnls.AllowNew = false;
            // 
            // cbCnlFilter
            // 
            this.cbCnlFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbCnlFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCnlFilter.FormattingEnabled = true;
            this.cbCnlFilter.Location = new System.Drawing.Point(13, 32);
            this.cbCnlFilter.Name = "cbCnlFilter";
            this.cbCnlFilter.Size = new System.Drawing.Size(204, 21);
            this.cbCnlFilter.TabIndex = 2;
            this.cbCnlFilter.SelectedIndexChanged += new System.EventHandler(this.cbCnlFilter_SelectedIndexChanged);
            // 
            // lblCnlFilterByKP
            // 
            this.lblCnlFilterByKP.AutoSize = true;
            this.lblCnlFilterByKP.Location = new System.Drawing.Point(10, 16);
            this.lblCnlFilterByKP.Name = "lblCnlFilterByKP";
            this.lblCnlFilterByKP.Size = new System.Drawing.Size(22, 13);
            this.lblCnlFilterByKP.TabIndex = 0;
            this.lblCnlFilterByKP.Text = "КП";
            // 
            // lblCnlFilterByObj
            // 
            this.lblCnlFilterByObj.AutoSize = true;
            this.lblCnlFilterByObj.Location = new System.Drawing.Point(10, 16);
            this.lblCnlFilterByObj.Name = "lblCnlFilterByObj";
            this.lblCnlFilterByObj.Size = new System.Drawing.Size(45, 13);
            this.lblCnlFilterByObj.TabIndex = 1;
            this.lblCnlFilterByObj.Text = "Объект";
            this.lblCnlFilterByObj.Visible = false;
            // 
            // splVert
            // 
            this.splVert.Location = new System.Drawing.Point(320, 49);
            this.splVert.MinExtra = 350;
            this.splVert.MinSize = 280;
            this.splVert.Name = "splVert";
            this.splVert.Size = new System.Drawing.Size(3, 471);
            this.splVert.TabIndex = 3;
            this.splVert.TabStop = false;
            // 
            // pnlRight
            // 
            this.pnlRight.Controls.Add(this.gbTableView);
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRight.Location = new System.Drawing.Point(323, 49);
            this.pnlRight.Margin = new System.Windows.Forms.Padding(0);
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Size = new System.Drawing.Size(441, 471);
            this.pnlRight.TabIndex = 4;
            // 
            // gbTableView
            // 
            this.gbTableView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbTableView.Controls.Add(this.btnAddItem);
            this.gbTableView.Controls.Add(this.btnShowItemInfo);
            this.gbTableView.Controls.Add(this.dgvItems);
            this.gbTableView.Controls.Add(this.lblTableItems);
            this.gbTableView.Controls.Add(this.btnDeleteItem);
            this.gbTableView.Controls.Add(this.lblTableTitle);
            this.gbTableView.Controls.Add(this.btnMoveDownItem);
            this.gbTableView.Controls.Add(this.txtTableTitle);
            this.gbTableView.Controls.Add(this.btnMoveUpItem);
            this.gbTableView.Controls.Add(this.btnAddEmptyItem);
            this.gbTableView.Location = new System.Drawing.Point(0, 0);
            this.gbTableView.Name = "gbTableView";
            this.gbTableView.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbTableView.Size = new System.Drawing.Size(438, 468);
            this.gbTableView.TabIndex = 0;
            this.gbTableView.TabStop = false;
            this.gbTableView.Text = "Табличное представление";
            // 
            // btnAddItem
            // 
            this.btnAddItem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddItem.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAddItem.Image = ((System.Drawing.Image)(resources.GetObject("btnAddItem.Image")));
            this.btnAddItem.Location = new System.Drawing.Point(257, 58);
            this.btnAddItem.Name = "btnAddItem";
            this.btnAddItem.Size = new System.Drawing.Size(23, 22);
            this.btnAddItem.TabIndex = 3;
            this.toolTip.SetToolTip(this.btnAddItem, "Добавить элемент из базы конфигурации");
            this.btnAddItem.UseVisualStyleBackColor = true;
            this.btnAddItem.Click += new System.EventHandler(this.btnAddItem_Click);
            // 
            // btnShowItemInfo
            // 
            this.btnShowItemInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnShowItemInfo.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnShowItemInfo.Image = ((System.Drawing.Image)(resources.GetObject("btnShowItemInfo.Image")));
            this.btnShowItemInfo.Location = new System.Drawing.Point(402, 58);
            this.btnShowItemInfo.Name = "btnShowItemInfo";
            this.btnShowItemInfo.Size = new System.Drawing.Size(23, 22);
            this.btnShowItemInfo.TabIndex = 8;
            this.toolTip.SetToolTip(this.btnShowItemInfo, "Информация о выбранном элементе");
            this.btnShowItemInfo.UseVisualStyleBackColor = true;
            this.btnShowItemInfo.Click += new System.EventHandler(this.btnShowItemInfo_Click);
            // 
            // dgvItems
            // 
            this.dgvItems.AllowUserToResizeRows = false;
            this.dgvItems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvItems.AutoGenerateColumns = false;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvItems.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvItems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvItems.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colItemCnlNum,
            this.colItemCtrlCnlNum,
            this.colItemCaption,
            this.colItemHidden});
            this.dgvItems.DataSource = this.bsItems;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvItems.DefaultCellStyle = dataGridViewCellStyle5;
            this.dgvItems.Location = new System.Drawing.Point(13, 86);
            this.dgvItems.Margin = new System.Windows.Forms.Padding(0, 3, 3, 0);
            this.dgvItems.Name = "dgvItems";
            this.dgvItems.Size = new System.Drawing.Size(412, 369);
            this.dgvItems.TabIndex = 9;
            this.dgvItems.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvItems_CellFormatting);
            this.dgvItems.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dgvItems_CellValidating);
            this.dgvItems.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvItems_CellValueChanged);
            this.dgvItems.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvItems_DataError);
            this.dgvItems.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.dgvItems_RowsRemoved);
            this.dgvItems.SelectionChanged += new System.EventHandler(this.dgvItems_SelectionChanged);
            // 
            // colItemCnlNum
            // 
            this.colItemCnlNum.DataPropertyName = "CnlNum";
            this.colItemCnlNum.HeaderText = "№ вх. кан.";
            this.colItemCnlNum.Name = "colItemCnlNum";
            this.colItemCnlNum.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colItemCnlNum.Width = 65;
            // 
            // colItemCtrlCnlNum
            // 
            this.colItemCtrlCnlNum.DataPropertyName = "CtrlCnlNum";
            this.colItemCtrlCnlNum.HeaderText = "№ кан. упр.";
            this.colItemCtrlCnlNum.Name = "colItemCtrlCnlNum";
            this.colItemCtrlCnlNum.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colItemCtrlCnlNum.Width = 75;
            // 
            // colItemCaption
            // 
            this.colItemCaption.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colItemCaption.DataPropertyName = "Caption";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.colItemCaption.DefaultCellStyle = dataGridViewCellStyle4;
            this.colItemCaption.HeaderText = "Обозначение";
            this.colItemCaption.Name = "colItemCaption";
            this.colItemCaption.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colItemHidden
            // 
            this.colItemHidden.DataPropertyName = "Hidden";
            this.colItemHidden.HeaderText = "Скрыть";
            this.colItemHidden.Name = "colItemHidden";
            this.colItemHidden.Width = 60;
            // 
            // bsItems
            // 
            this.bsItems.AllowNew = false;
            // 
            // lblTableItems
            // 
            this.lblTableItems.AutoSize = true;
            this.lblTableItems.Location = new System.Drawing.Point(10, 67);
            this.lblTableItems.Name = "lblTableItems";
            this.lblTableItems.Size = new System.Drawing.Size(59, 13);
            this.lblTableItems.TabIndex = 2;
            this.lblTableItems.Text = "Элементы";
            // 
            // btnDeleteItem
            // 
            this.btnDeleteItem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeleteItem.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnDeleteItem.Image = ((System.Drawing.Image)(resources.GetObject("btnDeleteItem.Image")));
            this.btnDeleteItem.Location = new System.Drawing.Point(373, 58);
            this.btnDeleteItem.Name = "btnDeleteItem";
            this.btnDeleteItem.Size = new System.Drawing.Size(23, 22);
            this.btnDeleteItem.TabIndex = 7;
            this.toolTip.SetToolTip(this.btnDeleteItem, "Удалить выбранные элементы");
            this.btnDeleteItem.UseVisualStyleBackColor = true;
            this.btnDeleteItem.Click += new System.EventHandler(this.btnDeleteItem_Click);
            // 
            // lblTableTitle
            // 
            this.lblTableTitle.AutoSize = true;
            this.lblTableTitle.Location = new System.Drawing.Point(10, 16);
            this.lblTableTitle.Name = "lblTableTitle";
            this.lblTableTitle.Size = new System.Drawing.Size(83, 13);
            this.lblTableTitle.TabIndex = 0;
            this.lblTableTitle.Text = "Наименование";
            // 
            // btnMoveDownItem
            // 
            this.btnMoveDownItem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMoveDownItem.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnMoveDownItem.Image = ((System.Drawing.Image)(resources.GetObject("btnMoveDownItem.Image")));
            this.btnMoveDownItem.Location = new System.Drawing.Point(344, 58);
            this.btnMoveDownItem.Name = "btnMoveDownItem";
            this.btnMoveDownItem.Size = new System.Drawing.Size(23, 22);
            this.btnMoveDownItem.TabIndex = 6;
            this.toolTip.SetToolTip(this.btnMoveDownItem, "Переместить выбранный элемент вниз");
            this.btnMoveDownItem.UseVisualStyleBackColor = true;
            this.btnMoveDownItem.Click += new System.EventHandler(this.btnMoveUpDownItem_Click);
            // 
            // txtTableTitle
            // 
            this.txtTableTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTableTitle.Location = new System.Drawing.Point(13, 32);
            this.txtTableTitle.Name = "txtTableTitle";
            this.txtTableTitle.Size = new System.Drawing.Size(412, 20);
            this.txtTableTitle.TabIndex = 1;
            this.txtTableTitle.TextChanged += new System.EventHandler(this.txtTableTitle_TextChanged);
            // 
            // btnMoveUpItem
            // 
            this.btnMoveUpItem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMoveUpItem.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnMoveUpItem.Image = ((System.Drawing.Image)(resources.GetObject("btnMoveUpItem.Image")));
            this.btnMoveUpItem.Location = new System.Drawing.Point(315, 58);
            this.btnMoveUpItem.Name = "btnMoveUpItem";
            this.btnMoveUpItem.Size = new System.Drawing.Size(23, 22);
            this.btnMoveUpItem.TabIndex = 5;
            this.toolTip.SetToolTip(this.btnMoveUpItem, "Переместить выбранный элемент вверх");
            this.btnMoveUpItem.UseVisualStyleBackColor = true;
            this.btnMoveUpItem.Click += new System.EventHandler(this.btnMoveUpDownItem_Click);
            // 
            // btnAddEmptyItem
            // 
            this.btnAddEmptyItem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddEmptyItem.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAddEmptyItem.Image = ((System.Drawing.Image)(resources.GetObject("btnAddEmptyItem.Image")));
            this.btnAddEmptyItem.Location = new System.Drawing.Point(286, 58);
            this.btnAddEmptyItem.Name = "btnAddEmptyItem";
            this.btnAddEmptyItem.Size = new System.Drawing.Size(23, 22);
            this.btnAddEmptyItem.TabIndex = 4;
            this.toolTip.SetToolTip(this.btnAddEmptyItem, "Добавить пустой элемент");
            this.btnAddEmptyItem.UseVisualStyleBackColor = true;
            this.btnAddEmptyItem.Click += new System.EventHandler(this.btnAddEmptyItem_Click);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(764, 542);
            this.Controls.Add(this.pnlRight);
            this.Controls.Add(this.splVert);
            this.Controls.Add(this.pnlLeft);
            this.Controls.Add(this.statusMain);
            this.Controls.Add(this.toolMain);
            this.Controls.Add(this.menuMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(700, 500);
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SCADA-Редактор таблиц";
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
            this.gbBase.ResumeLayout(false);
            this.gbBase.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCnls)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsCnls)).EndInit();
            this.pnlRight.ResumeLayout(false);
            this.gbTableView.ResumeLayout(false);
            this.gbTableView.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItems)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsItems)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuMain;
        private System.Windows.Forms.ToolStripMenuItem miFile;
        private System.Windows.Forms.ToolStripMenuItem miFileNew;
        private System.Windows.Forms.ToolStripMenuItem miFileOpen;
        private System.Windows.Forms.ToolStripMenuItem miFileSave;
        private System.Windows.Forms.ToolStripMenuItem miFileSaveAs;
        private System.Windows.Forms.ToolStripSeparator miFileSep;
        private System.Windows.Forms.ToolStripMenuItem miFileExit;
        private System.Windows.Forms.ToolStrip toolMain;
        private System.Windows.Forms.ToolStripButton btnFileNew;
        private System.Windows.Forms.ToolStripButton btnFileOpen;
        private System.Windows.Forms.ToolStripButton btnFileSave;
        private System.Windows.Forms.ToolStripSeparator sep1;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.StatusStrip statusMain;
        private System.Windows.Forms.ToolStripStatusLabel lblBaseDATDir;
        private System.Windows.Forms.Panel pnlLeft;
        private System.Windows.Forms.Splitter splVert;
        private System.Windows.Forms.DataGridView dgvCnls;
        private System.Windows.Forms.Label lblCnlFilterByKP;
        private System.Windows.Forms.ComboBox cbCnlFilter;
        private System.Windows.Forms.Panel pnlRight;
        private System.Windows.Forms.Label lblTableTitle;
        private System.Windows.Forms.TextBox txtTableTitle;
        private System.Windows.Forms.Label lblTableItems;
        private System.Windows.Forms.Button btnDeleteItem;
        private System.Windows.Forms.Button btnMoveDownItem;
        private System.Windows.Forms.Button btnMoveUpItem;
        private System.Windows.Forms.Button btnAddEmptyItem;
        private System.Windows.Forms.DataGridView dgvItems;
        private System.Windows.Forms.GroupBox gbBase;
        private System.Windows.Forms.GroupBox gbTableView;
        private System.Windows.Forms.Button btnShowItemInfo;
        private System.Windows.Forms.RadioButton rbInCnls;
        private System.Windows.Forms.RadioButton rbCtrlCnls;
        private System.Windows.Forms.Button btnAddItem;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ToolStripButton btnSettingsParams;
        private System.Windows.Forms.ToolStripMenuItem miSettings;
        private System.Windows.Forms.ToolStripMenuItem miHelp;
        private System.Windows.Forms.ToolStripMenuItem miHelpAbout;
        private System.Windows.Forms.Button btnFilterCnlsByObj;
        private System.Windows.Forms.Button btnFilterCnlsByKP;
        private System.Windows.Forms.Button btnRefreshBase;
        private System.Windows.Forms.ToolStripMenuItem miSettingsParams;
        private System.Windows.Forms.BindingSource bsItems;
        private System.Windows.Forms.BindingSource bsCnls;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCnlNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCnlName;
        private System.Windows.Forms.Label lblCnlFilterByObj;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItemCnlNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItemCtrlCnlNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItemCaption;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colItemHidden;
    }
}

