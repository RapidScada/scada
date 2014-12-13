namespace Scada.Comm.KP
{
    partial class FrmDevTemplate
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Группы элементов");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Команды");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmDevTemplate));
            this.treeView = new System.Windows.Forms.TreeView();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.btnNew = new System.Windows.Forms.ToolStripButton();
            this.btnOpen = new System.Windows.Forms.ToolStripButton();
            this.btnSave = new System.Windows.Forms.ToolStripButton();
            this.btnSaveAs = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.btnAddElemGroup = new System.Windows.Forms.ToolStripButton();
            this.btnAddElem = new System.Windows.Forms.ToolStripButton();
            this.btnAddCmd = new System.Windows.Forms.ToolStripButton();
            this.btnMoveUp = new System.Windows.Forms.ToolStripButton();
            this.btnMoveDown = new System.Windows.Forms.ToolStripButton();
            this.btnDelete = new System.Windows.Forms.ToolStripButton();
            this.gbDevTemplate = new System.Windows.Forms.GroupBox();
            this.gbElemGroup = new System.Windows.Forms.GroupBox();
            this.lblGrElemCnt = new System.Windows.Forms.Label();
            this.numGrElemCnt = new System.Windows.Forms.NumericUpDown();
            this.txtGrName = new System.Windows.Forms.TextBox();
            this.lblGrName = new System.Windows.Forms.Label();
            this.numGrAddress = new System.Windows.Forms.NumericUpDown();
            this.lblGrAddress = new System.Windows.Forms.Label();
            this.lblGrTableType = new System.Windows.Forms.Label();
            this.cbGrTableType = new System.Windows.Forms.ComboBox();
            this.gbElem = new System.Windows.Forms.GroupBox();
            this.rbBool = new System.Windows.Forms.RadioButton();
            this.rbFloat = new System.Windows.Forms.RadioButton();
            this.rbInt = new System.Windows.Forms.RadioButton();
            this.rbUInt = new System.Windows.Forms.RadioButton();
            this.rbShort = new System.Windows.Forms.RadioButton();
            this.lblElemType = new System.Windows.Forms.Label();
            this.rbUShort = new System.Windows.Forms.RadioButton();
            this.txtElemAddress = new System.Windows.Forms.TextBox();
            this.lblElemAddress = new System.Windows.Forms.Label();
            this.txtElemSignal = new System.Windows.Forms.TextBox();
            this.lblElemSignal = new System.Windows.Forms.Label();
            this.txtElemName = new System.Windows.Forms.TextBox();
            this.lblElemName = new System.Windows.Forms.Label();
            this.gbCmd = new System.Windows.Forms.GroupBox();
            this.chkCmdMultiple = new System.Windows.Forms.CheckBox();
            this.lblCmdElemCnt = new System.Windows.Forms.Label();
            this.numCmdElemCnt = new System.Windows.Forms.NumericUpDown();
            this.txtCmdName = new System.Windows.Forms.TextBox();
            this.lblCmdName = new System.Windows.Forms.Label();
            this.lblCmdNum = new System.Windows.Forms.Label();
            this.numCmdNum = new System.Windows.Forms.NumericUpDown();
            this.numCmdAddress = new System.Windows.Forms.NumericUpDown();
            this.lblCmdAddress = new System.Windows.Forms.Label();
            this.lblCmdTableType = new System.Windows.Forms.Label();
            this.cbCmdTableType = new System.Windows.Forms.ComboBox();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.toolStrip.SuspendLayout();
            this.gbDevTemplate.SuspendLayout();
            this.gbElemGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numGrElemCnt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numGrAddress)).BeginInit();
            this.gbElem.SuspendLayout();
            this.gbCmd.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCmdElemCnt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCmdNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCmdAddress)).BeginInit();
            this.SuspendLayout();
            // 
            // treeView
            // 
            this.treeView.HideSelection = false;
            this.treeView.ImageIndex = 0;
            this.treeView.ImageList = this.imageList;
            this.treeView.Location = new System.Drawing.Point(13, 19);
            this.treeView.Name = "treeView";
            treeNode1.ImageKey = "group.png";
            treeNode1.Name = "grsNode";
            treeNode1.SelectedImageKey = "group.png";
            treeNode1.Text = "Группы элементов";
            treeNode2.ImageIndex = 2;
            treeNode2.Name = "cmdsNode";
            treeNode2.SelectedImageKey = "cmds.png";
            treeNode2.Text = "Команды";
            this.treeView.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2});
            this.treeView.SelectedImageIndex = 0;
            this.treeView.ShowRootLines = false;
            this.treeView.Size = new System.Drawing.Size(254, 372);
            this.treeView.TabIndex = 0;
            this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterSelect);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "group.png");
            this.imageList.Images.SetKeyName(1, "elem.png");
            this.imageList.Images.SetKeyName(2, "cmds.png");
            this.imageList.Images.SetKeyName(3, "cmd.png");
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnNew,
            this.btnOpen,
            this.btnSave,
            this.btnSaveAs,
            this.toolStripSeparator,
            this.btnAddElemGroup,
            this.btnAddElem,
            this.btnAddCmd,
            this.btnMoveUp,
            this.btnMoveDown,
            this.btnDelete});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(590, 25);
            this.toolStrip.TabIndex = 0;
            this.toolStrip.Text = "toolStrip1";
            // 
            // btnNew
            // 
            this.btnNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnNew.Image = ((System.Drawing.Image)(resources.GetObject("btnNew.Image")));
            this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(23, 22);
            this.btnNew.ToolTipText = "Создать новый шаблон";
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnOpen
            // 
            this.btnOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnOpen.Image = ((System.Drawing.Image)(resources.GetObject("btnOpen.Image")));
            this.btnOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(23, 22);
            this.btnOpen.ToolTipText = "Открыть шаблон";
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // btnSave
            // 
            this.btnSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(23, 22);
            this.btnSave.ToolTipText = "Сохранить шаблон";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnSaveAs
            // 
            this.btnSaveAs.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSaveAs.Image = ((System.Drawing.Image)(resources.GetObject("btnSaveAs.Image")));
            this.btnSaveAs.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSaveAs.Name = "btnSaveAs";
            this.btnSaveAs.Size = new System.Drawing.Size(23, 22);
            this.btnSaveAs.ToolTipText = "Сохранить шаблон как";
            this.btnSaveAs.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            this.toolStripSeparator.Size = new System.Drawing.Size(6, 25);
            // 
            // btnAddElemGroup
            // 
            this.btnAddElemGroup.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnAddElemGroup.Image = ((System.Drawing.Image)(resources.GetObject("btnAddElemGroup.Image")));
            this.btnAddElemGroup.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddElemGroup.Name = "btnAddElemGroup";
            this.btnAddElemGroup.Size = new System.Drawing.Size(23, 22);
            this.btnAddElemGroup.ToolTipText = "Добавить группу элементов";
            this.btnAddElemGroup.Click += new System.EventHandler(this.btnAddElemGroup_Click);
            // 
            // btnAddElem
            // 
            this.btnAddElem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnAddElem.Image = ((System.Drawing.Image)(resources.GetObject("btnAddElem.Image")));
            this.btnAddElem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddElem.Name = "btnAddElem";
            this.btnAddElem.Size = new System.Drawing.Size(23, 22);
            this.btnAddElem.ToolTipText = "Добавить элемент";
            this.btnAddElem.Click += new System.EventHandler(this.btnAddElem_Click);
            // 
            // btnAddCmd
            // 
            this.btnAddCmd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnAddCmd.Image = ((System.Drawing.Image)(resources.GetObject("btnAddCmd.Image")));
            this.btnAddCmd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddCmd.Name = "btnAddCmd";
            this.btnAddCmd.Size = new System.Drawing.Size(23, 22);
            this.btnAddCmd.ToolTipText = "Добавить команду";
            this.btnAddCmd.Click += new System.EventHandler(this.btnAddCmd_Click);
            // 
            // btnMoveUp
            // 
            this.btnMoveUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnMoveUp.Image = ((System.Drawing.Image)(resources.GetObject("btnMoveUp.Image")));
            this.btnMoveUp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnMoveUp.Name = "btnMoveUp";
            this.btnMoveUp.Size = new System.Drawing.Size(23, 22);
            this.btnMoveUp.ToolTipText = "Переместить вверх";
            this.btnMoveUp.Click += new System.EventHandler(this.btnMoveUp_Click);
            // 
            // btnMoveDown
            // 
            this.btnMoveDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnMoveDown.Image = ((System.Drawing.Image)(resources.GetObject("btnMoveDown.Image")));
            this.btnMoveDown.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnMoveDown.Name = "btnMoveDown";
            this.btnMoveDown.Size = new System.Drawing.Size(23, 22);
            this.btnMoveDown.ToolTipText = "Переместить вниз";
            this.btnMoveDown.Click += new System.EventHandler(this.btnMoveDown_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnDelete.Image = ((System.Drawing.Image)(resources.GetObject("btnDelete.Image")));
            this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(23, 22);
            this.btnDelete.ToolTipText = "Удалить";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // gbDevTemplate
            // 
            this.gbDevTemplate.Controls.Add(this.treeView);
            this.gbDevTemplate.Location = new System.Drawing.Point(12, 28);
            this.gbDevTemplate.Name = "gbDevTemplate";
            this.gbDevTemplate.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbDevTemplate.Size = new System.Drawing.Size(280, 404);
            this.gbDevTemplate.TabIndex = 1;
            this.gbDevTemplate.TabStop = false;
            this.gbDevTemplate.Text = "Шаблон устройства";
            // 
            // gbElemGroup
            // 
            this.gbElemGroup.Controls.Add(this.lblGrElemCnt);
            this.gbElemGroup.Controls.Add(this.numGrElemCnt);
            this.gbElemGroup.Controls.Add(this.txtGrName);
            this.gbElemGroup.Controls.Add(this.lblGrName);
            this.gbElemGroup.Controls.Add(this.numGrAddress);
            this.gbElemGroup.Controls.Add(this.lblGrAddress);
            this.gbElemGroup.Controls.Add(this.lblGrTableType);
            this.gbElemGroup.Controls.Add(this.cbGrTableType);
            this.gbElemGroup.Location = new System.Drawing.Point(298, 28);
            this.gbElemGroup.Name = "gbElemGroup";
            this.gbElemGroup.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbElemGroup.Size = new System.Drawing.Size(280, 144);
            this.gbElemGroup.TabIndex = 2;
            this.gbElemGroup.TabStop = false;
            this.gbElemGroup.Text = "Параметры группы элементов";
            // 
            // lblGrElemCnt
            // 
            this.lblGrElemCnt.AutoSize = true;
            this.lblGrElemCnt.Location = new System.Drawing.Point(140, 95);
            this.lblGrElemCnt.Name = "lblGrElemCnt";
            this.lblGrElemCnt.Size = new System.Drawing.Size(124, 13);
            this.lblGrElemCnt.TabIndex = 6;
            this.lblGrElemCnt.Text = "Количество элементов";
            // 
            // numGrElemCnt
            // 
            this.numGrElemCnt.Location = new System.Drawing.Point(143, 111);
            this.numGrElemCnt.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.numGrElemCnt.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numGrElemCnt.Name = "numGrElemCnt";
            this.numGrElemCnt.Size = new System.Drawing.Size(124, 20);
            this.numGrElemCnt.TabIndex = 7;
            this.numGrElemCnt.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numGrElemCnt.ValueChanged += new System.EventHandler(this.numGrElemCnt_ValueChanged);
            // 
            // txtGrName
            // 
            this.txtGrName.Location = new System.Drawing.Point(13, 32);
            this.txtGrName.Name = "txtGrName";
            this.txtGrName.Size = new System.Drawing.Size(254, 20);
            this.txtGrName.TabIndex = 1;
            this.txtGrName.TextChanged += new System.EventHandler(this.txtGrName_TextChanged);
            // 
            // lblGrName
            // 
            this.lblGrName.AutoSize = true;
            this.lblGrName.Location = new System.Drawing.Point(10, 16);
            this.lblGrName.Name = "lblGrName";
            this.lblGrName.Size = new System.Drawing.Size(83, 13);
            this.lblGrName.TabIndex = 0;
            this.lblGrName.Text = "Наименование";
            // 
            // numGrAddress
            // 
            this.numGrAddress.Location = new System.Drawing.Point(13, 111);
            this.numGrAddress.Maximum = new decimal(new int[] {
            65536,
            0,
            0,
            0});
            this.numGrAddress.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numGrAddress.Name = "numGrAddress";
            this.numGrAddress.Size = new System.Drawing.Size(124, 20);
            this.numGrAddress.TabIndex = 5;
            this.numGrAddress.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numGrAddress.ValueChanged += new System.EventHandler(this.numGrAddress_ValueChanged);
            // 
            // lblGrAddress
            // 
            this.lblGrAddress.AutoSize = true;
            this.lblGrAddress.Location = new System.Drawing.Point(10, 95);
            this.lblGrAddress.Name = "lblGrAddress";
            this.lblGrAddress.Size = new System.Drawing.Size(113, 13);
            this.lblGrAddress.TabIndex = 4;
            this.lblGrAddress.Text = "Адрес нач. элемента";
            // 
            // lblGrTableType
            // 
            this.lblGrTableType.AutoSize = true;
            this.lblGrTableType.Location = new System.Drawing.Point(10, 55);
            this.lblGrTableType.Name = "lblGrTableType";
            this.lblGrTableType.Size = new System.Drawing.Size(90, 13);
            this.lblGrTableType.TabIndex = 2;
            this.lblGrTableType.Text = "Таблица данных";
            // 
            // cbGrTableType
            // 
            this.cbGrTableType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbGrTableType.FormattingEnabled = true;
            this.cbGrTableType.Items.AddRange(new object[] {
            "Discrete Inputs (Дискретные входы, 1X)",
            "Coils (Флаги, 0X)",
            "Input Registers (Входные регистры, 3X)",
            "Holding Registers (Регистры хранения, 4X)"});
            this.cbGrTableType.Location = new System.Drawing.Point(13, 71);
            this.cbGrTableType.Name = "cbGrTableType";
            this.cbGrTableType.Size = new System.Drawing.Size(254, 21);
            this.cbGrTableType.TabIndex = 3;
            this.cbGrTableType.SelectedIndexChanged += new System.EventHandler(this.cbGrTableType_SelectedIndexChanged);
            // 
            // gbElem
            // 
            this.gbElem.Controls.Add(this.rbBool);
            this.gbElem.Controls.Add(this.rbFloat);
            this.gbElem.Controls.Add(this.rbInt);
            this.gbElem.Controls.Add(this.rbUInt);
            this.gbElem.Controls.Add(this.rbShort);
            this.gbElem.Controls.Add(this.lblElemType);
            this.gbElem.Controls.Add(this.rbUShort);
            this.gbElem.Controls.Add(this.txtElemAddress);
            this.gbElem.Controls.Add(this.lblElemAddress);
            this.gbElem.Controls.Add(this.txtElemSignal);
            this.gbElem.Controls.Add(this.lblElemSignal);
            this.gbElem.Controls.Add(this.txtElemName);
            this.gbElem.Controls.Add(this.lblElemName);
            this.gbElem.Location = new System.Drawing.Point(298, 91);
            this.gbElem.Name = "gbElem";
            this.gbElem.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbElem.Size = new System.Drawing.Size(280, 186);
            this.gbElem.TabIndex = 3;
            this.gbElem.TabStop = false;
            this.gbElem.Text = "Параметры элемента";
            // 
            // rbBool
            // 
            this.rbBool.AutoSize = true;
            this.rbBool.Location = new System.Drawing.Point(143, 156);
            this.rbBool.Name = "rbBool";
            this.rbBool.Size = new System.Drawing.Size(80, 17);
            this.rbBool.TabIndex = 12;
            this.rbBool.TabStop = true;
            this.rbBool.Text = "bool (1 бит)";
            this.rbBool.UseVisualStyleBackColor = true;
            this.rbBool.CheckedChanged += new System.EventHandler(this.rbType_CheckedChanged);
            // 
            // rbFloat
            // 
            this.rbFloat.AutoSize = true;
            this.rbFloat.Location = new System.Drawing.Point(13, 156);
            this.rbFloat.Name = "rbFloat";
            this.rbFloat.Size = new System.Drawing.Size(92, 17);
            this.rbFloat.TabIndex = 11;
            this.rbFloat.TabStop = true;
            this.rbFloat.Text = "float (4 байта)";
            this.rbFloat.UseVisualStyleBackColor = true;
            this.rbFloat.CheckedChanged += new System.EventHandler(this.rbType_CheckedChanged);
            // 
            // rbInt
            // 
            this.rbInt.AutoSize = true;
            this.rbInt.Location = new System.Drawing.Point(143, 133);
            this.rbInt.Name = "rbInt";
            this.rbInt.Size = new System.Drawing.Size(83, 17);
            this.rbInt.TabIndex = 10;
            this.rbInt.TabStop = true;
            this.rbInt.Text = "int (4 байта)";
            this.rbInt.UseVisualStyleBackColor = true;
            this.rbInt.CheckedChanged += new System.EventHandler(this.rbType_CheckedChanged);
            // 
            // rbUInt
            // 
            this.rbUInt.AutoSize = true;
            this.rbUInt.Location = new System.Drawing.Point(13, 133);
            this.rbUInt.Name = "rbUInt";
            this.rbUInt.Size = new System.Drawing.Size(89, 17);
            this.rbUInt.TabIndex = 9;
            this.rbUInt.TabStop = true;
            this.rbUInt.Text = "uint (4 байта)";
            this.rbUInt.UseVisualStyleBackColor = true;
            this.rbUInt.CheckedChanged += new System.EventHandler(this.rbType_CheckedChanged);
            // 
            // rbShort
            // 
            this.rbShort.AutoSize = true;
            this.rbShort.Location = new System.Drawing.Point(143, 110);
            this.rbShort.Name = "rbShort";
            this.rbShort.Size = new System.Drawing.Size(95, 17);
            this.rbShort.TabIndex = 8;
            this.rbShort.TabStop = true;
            this.rbShort.Text = "short (2 байта)";
            this.rbShort.UseVisualStyleBackColor = true;
            this.rbShort.CheckedChanged += new System.EventHandler(this.rbType_CheckedChanged);
            // 
            // lblElemType
            // 
            this.lblElemType.AutoSize = true;
            this.lblElemType.Location = new System.Drawing.Point(10, 94);
            this.lblElemType.Name = "lblElemType";
            this.lblElemType.Size = new System.Drawing.Size(29, 13);
            this.lblElemType.TabIndex = 6;
            this.lblElemType.Text = "Тип:";
            // 
            // rbUShort
            // 
            this.rbUShort.AutoSize = true;
            this.rbUShort.Location = new System.Drawing.Point(13, 110);
            this.rbUShort.Name = "rbUShort";
            this.rbUShort.Size = new System.Drawing.Size(101, 17);
            this.rbUShort.TabIndex = 7;
            this.rbUShort.TabStop = true;
            this.rbUShort.Text = "ushort (2 байта)";
            this.rbUShort.UseVisualStyleBackColor = true;
            this.rbUShort.CheckedChanged += new System.EventHandler(this.rbType_CheckedChanged);
            // 
            // txtElemAddress
            // 
            this.txtElemAddress.Location = new System.Drawing.Point(13, 71);
            this.txtElemAddress.Name = "txtElemAddress";
            this.txtElemAddress.ReadOnly = true;
            this.txtElemAddress.Size = new System.Drawing.Size(124, 20);
            this.txtElemAddress.TabIndex = 3;
            // 
            // lblElemAddress
            // 
            this.lblElemAddress.AutoSize = true;
            this.lblElemAddress.Location = new System.Drawing.Point(10, 55);
            this.lblElemAddress.Name = "lblElemAddress";
            this.lblElemAddress.Size = new System.Drawing.Size(38, 13);
            this.lblElemAddress.TabIndex = 2;
            this.lblElemAddress.Text = "Адрес";
            // 
            // txtElemSignal
            // 
            this.txtElemSignal.Location = new System.Drawing.Point(143, 71);
            this.txtElemSignal.Name = "txtElemSignal";
            this.txtElemSignal.ReadOnly = true;
            this.txtElemSignal.Size = new System.Drawing.Size(124, 20);
            this.txtElemSignal.TabIndex = 5;
            // 
            // lblElemSignal
            // 
            this.lblElemSignal.AutoSize = true;
            this.lblElemSignal.Location = new System.Drawing.Point(140, 55);
            this.lblElemSignal.Name = "lblElemSignal";
            this.lblElemSignal.Size = new System.Drawing.Size(61, 13);
            this.lblElemSignal.TabIndex = 4;
            this.lblElemSignal.Text = "Сигнал КП";
            // 
            // txtElemName
            // 
            this.txtElemName.Location = new System.Drawing.Point(13, 32);
            this.txtElemName.Name = "txtElemName";
            this.txtElemName.Size = new System.Drawing.Size(254, 20);
            this.txtElemName.TabIndex = 1;
            this.txtElemName.TextChanged += new System.EventHandler(this.txtElemName_TextChanged);
            // 
            // lblElemName
            // 
            this.lblElemName.AutoSize = true;
            this.lblElemName.Location = new System.Drawing.Point(10, 16);
            this.lblElemName.Name = "lblElemName";
            this.lblElemName.Size = new System.Drawing.Size(83, 13);
            this.lblElemName.TabIndex = 0;
            this.lblElemName.Text = "Наименование";
            // 
            // gbCmd
            // 
            this.gbCmd.Controls.Add(this.chkCmdMultiple);
            this.gbCmd.Controls.Add(this.lblCmdElemCnt);
            this.gbCmd.Controls.Add(this.numCmdElemCnt);
            this.gbCmd.Controls.Add(this.txtCmdName);
            this.gbCmd.Controls.Add(this.lblCmdName);
            this.gbCmd.Controls.Add(this.lblCmdNum);
            this.gbCmd.Controls.Add(this.numCmdNum);
            this.gbCmd.Controls.Add(this.numCmdAddress);
            this.gbCmd.Controls.Add(this.lblCmdAddress);
            this.gbCmd.Controls.Add(this.lblCmdTableType);
            this.gbCmd.Controls.Add(this.cbCmdTableType);
            this.gbCmd.Location = new System.Drawing.Point(298, 226);
            this.gbCmd.Name = "gbCmd";
            this.gbCmd.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbCmd.Size = new System.Drawing.Size(280, 206);
            this.gbCmd.TabIndex = 4;
            this.gbCmd.TabStop = false;
            this.gbCmd.Text = "Параметры команды";
            // 
            // chkCmdMultiple
            // 
            this.chkCmdMultiple.AutoSize = true;
            this.chkCmdMultiple.Location = new System.Drawing.Point(13, 98);
            this.chkCmdMultiple.Name = "chkCmdMultiple";
            this.chkCmdMultiple.Size = new System.Drawing.Size(155, 17);
            this.chkCmdMultiple.TabIndex = 4;
            this.chkCmdMultiple.Text = "Множественная команда";
            this.chkCmdMultiple.UseVisualStyleBackColor = true;
            this.chkCmdMultiple.CheckedChanged += new System.EventHandler(this.chkCmdMultiple_CheckedChanged);
            // 
            // lblCmdElemCnt
            // 
            this.lblCmdElemCnt.AutoSize = true;
            this.lblCmdElemCnt.Location = new System.Drawing.Point(140, 118);
            this.lblCmdElemCnt.Name = "lblCmdElemCnt";
            this.lblCmdElemCnt.Size = new System.Drawing.Size(124, 13);
            this.lblCmdElemCnt.TabIndex = 7;
            this.lblCmdElemCnt.Text = "Количество элементов";
            // 
            // numCmdElemCnt
            // 
            this.numCmdElemCnt.Location = new System.Drawing.Point(143, 134);
            this.numCmdElemCnt.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.numCmdElemCnt.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numCmdElemCnt.Name = "numCmdElemCnt";
            this.numCmdElemCnt.Size = new System.Drawing.Size(124, 20);
            this.numCmdElemCnt.TabIndex = 8;
            this.numCmdElemCnt.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numCmdElemCnt.ValueChanged += new System.EventHandler(this.numCmdElemCnt_ValueChanged);
            // 
            // txtCmdName
            // 
            this.txtCmdName.Location = new System.Drawing.Point(13, 32);
            this.txtCmdName.Name = "txtCmdName";
            this.txtCmdName.Size = new System.Drawing.Size(254, 20);
            this.txtCmdName.TabIndex = 1;
            this.txtCmdName.TextChanged += new System.EventHandler(this.txtCmdName_TextChanged);
            // 
            // lblCmdName
            // 
            this.lblCmdName.AutoSize = true;
            this.lblCmdName.Location = new System.Drawing.Point(10, 16);
            this.lblCmdName.Name = "lblCmdName";
            this.lblCmdName.Size = new System.Drawing.Size(83, 13);
            this.lblCmdName.TabIndex = 0;
            this.lblCmdName.Text = "Наименование";
            // 
            // lblCmdNum
            // 
            this.lblCmdNum.AutoSize = true;
            this.lblCmdNum.Location = new System.Drawing.Point(10, 157);
            this.lblCmdNum.Name = "lblCmdNum";
            this.lblCmdNum.Size = new System.Drawing.Size(108, 13);
            this.lblCmdNum.TabIndex = 9;
            this.lblCmdNum.Text = "Номер команды КП";
            // 
            // numCmdNum
            // 
            this.numCmdNum.Location = new System.Drawing.Point(13, 173);
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
            this.numCmdNum.Size = new System.Drawing.Size(124, 20);
            this.numCmdNum.TabIndex = 10;
            this.numCmdNum.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numCmdNum.ValueChanged += new System.EventHandler(this.numCmdNum_ValueChanged);
            // 
            // numCmdAddress
            // 
            this.numCmdAddress.Location = new System.Drawing.Point(13, 134);
            this.numCmdAddress.Maximum = new decimal(new int[] {
            65536,
            0,
            0,
            0});
            this.numCmdAddress.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numCmdAddress.Name = "numCmdAddress";
            this.numCmdAddress.Size = new System.Drawing.Size(124, 20);
            this.numCmdAddress.TabIndex = 6;
            this.numCmdAddress.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numCmdAddress.ValueChanged += new System.EventHandler(this.numCmdAddress_ValueChanged);
            // 
            // lblCmdAddress
            // 
            this.lblCmdAddress.AutoSize = true;
            this.lblCmdAddress.Location = new System.Drawing.Point(10, 118);
            this.lblCmdAddress.Name = "lblCmdAddress";
            this.lblCmdAddress.Size = new System.Drawing.Size(90, 13);
            this.lblCmdAddress.TabIndex = 5;
            this.lblCmdAddress.Text = "Адрес элемента";
            // 
            // lblCmdTableType
            // 
            this.lblCmdTableType.AutoSize = true;
            this.lblCmdTableType.Location = new System.Drawing.Point(10, 55);
            this.lblCmdTableType.Name = "lblCmdTableType";
            this.lblCmdTableType.Size = new System.Drawing.Size(90, 13);
            this.lblCmdTableType.TabIndex = 2;
            this.lblCmdTableType.Text = "Таблица данных";
            // 
            // cbCmdTableType
            // 
            this.cbCmdTableType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCmdTableType.FormattingEnabled = true;
            this.cbCmdTableType.Items.AddRange(new object[] {
            "Coils (Флаги, 0X)",
            "Holding Registers (Регистры хранения, 4X)"});
            this.cbCmdTableType.Location = new System.Drawing.Point(13, 71);
            this.cbCmdTableType.Name = "cbCmdTableType";
            this.cbCmdTableType.Size = new System.Drawing.Size(254, 21);
            this.cbCmdTableType.TabIndex = 3;
            this.cbCmdTableType.SelectedIndexChanged += new System.EventHandler(this.cbCmdTableType_SelectedIndexChanged);
            // 
            // openFileDialog
            // 
            this.openFileDialog.DefaultExt = "*.xml";
            this.openFileDialog.Filter = "Файлы шаблонов (*.xml)|*.xml|Все файлы (*.*)|*.*";
            this.openFileDialog.FilterIndex = 0;
            this.openFileDialog.Title = "Открыть файл";
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.DefaultExt = "*.xml";
            this.saveFileDialog.Filter = "Файлы шаблонов (*.xml)|*.xml|Все файлы (*.*)|*.*";
            this.saveFileDialog.FilterIndex = 0;
            this.saveFileDialog.Title = "Сохранить файл";
            // 
            // FrmDevTemplate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(590, 444);
            this.Controls.Add(this.gbCmd);
            this.Controls.Add(this.gbElem);
            this.Controls.Add(this.gbElemGroup);
            this.Controls.Add(this.gbDevTemplate);
            this.Controls.Add(this.toolStrip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmDevTemplate";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "MODBUS. Редактор шаблонов устройств";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmDevTemplate_FormClosing);
            this.Load += new System.EventHandler(this.FrmDevTemplate_Load);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.gbDevTemplate.ResumeLayout(false);
            this.gbElemGroup.ResumeLayout(false);
            this.gbElemGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numGrElemCnt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numGrAddress)).EndInit();
            this.gbElem.ResumeLayout(false);
            this.gbElem.PerformLayout();
            this.gbCmd.ResumeLayout(false);
            this.gbCmd.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCmdElemCnt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCmdNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCmdAddress)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton btnSave;
        private System.Windows.Forms.ToolStripButton btnOpen;
        private System.Windows.Forms.ToolStripButton btnNew;
        private System.Windows.Forms.ToolStripButton btnAddElemGroup;
        private System.Windows.Forms.ToolStripButton btnAddCmd;
        private System.Windows.Forms.ToolStripButton btnMoveUp;
        private System.Windows.Forms.ToolStripButton btnMoveDown;
        private System.Windows.Forms.ToolStripButton btnDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.GroupBox gbDevTemplate;
        private System.Windows.Forms.GroupBox gbElemGroup;
        private System.Windows.Forms.GroupBox gbElem;
        private System.Windows.Forms.GroupBox gbCmd;
        private System.Windows.Forms.Label lblGrTableType;
        private System.Windows.Forms.ComboBox cbGrTableType;
        private System.Windows.Forms.Label lblGrAddress;
        private System.Windows.Forms.NumericUpDown numGrAddress;
        private System.Windows.Forms.TextBox txtGrName;
        private System.Windows.Forms.Label lblGrName;
        private System.Windows.Forms.NumericUpDown numGrElemCnt;
        private System.Windows.Forms.Label lblGrElemCnt;
        private System.Windows.Forms.TextBox txtElemName;
        private System.Windows.Forms.Label lblElemName;
        private System.Windows.Forms.TextBox txtElemSignal;
        private System.Windows.Forms.Label lblElemSignal;
        private System.Windows.Forms.TextBox txtElemAddress;
        private System.Windows.Forms.Label lblElemAddress;
        private System.Windows.Forms.NumericUpDown numCmdAddress;
        private System.Windows.Forms.Label lblCmdAddress;
        private System.Windows.Forms.Label lblCmdTableType;
        private System.Windows.Forms.ComboBox cbCmdTableType;
        private System.Windows.Forms.Label lblCmdNum;
        private System.Windows.Forms.NumericUpDown numCmdNum;
        private System.Windows.Forms.TextBox txtCmdName;
        private System.Windows.Forms.Label lblCmdName;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.ToolStripButton btnSaveAs;
        private System.Windows.Forms.Label lblElemType;
        private System.Windows.Forms.RadioButton rbUShort;
        private System.Windows.Forms.RadioButton rbShort;
        private System.Windows.Forms.RadioButton rbInt;
        private System.Windows.Forms.RadioButton rbUInt;
        private System.Windows.Forms.RadioButton rbFloat;
        private System.Windows.Forms.RadioButton rbBool;
        private System.Windows.Forms.ToolStripButton btnAddElem;
        private System.Windows.Forms.Label lblCmdElemCnt;
        private System.Windows.Forms.NumericUpDown numCmdElemCnt;
        private System.Windows.Forms.CheckBox chkCmdMultiple;
    }
}