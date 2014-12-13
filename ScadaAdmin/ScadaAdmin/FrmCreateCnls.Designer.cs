namespace ScadaAdmin
{
    partial class FrmCreateCnls
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
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.btnDeselectAll = new System.Windows.Forms.Button();
            this.lblKPSel = new System.Windows.Forms.Label();
            this.gbInCnlsNumbering = new System.Windows.Forms.GroupBox();
            this.numInCnlsSpace = new System.Windows.Forms.NumericUpDown();
            this.lblInCnlsSpace = new System.Windows.Forms.Label();
            this.lblInCnlsStart = new System.Windows.Forms.Label();
            this.numInCnlsShift = new System.Windows.Forms.NumericUpDown();
            this.numInCnlsMultiple = new System.Windows.Forms.NumericUpDown();
            this.numInCnlsStart = new System.Windows.Forms.NumericUpDown();
            this.lblInCnlsShift = new System.Windows.Forms.Label();
            this.lblInCnlsMult = new System.Windows.Forms.Label();
            this.btnCalc = new System.Windows.Forms.Button();
            this.btnCreate = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.gbCtrlCnlsNumbering = new System.Windows.Forms.GroupBox();
            this.numCtrlCnlsSpace = new System.Windows.Forms.NumericUpDown();
            this.lblCtrlCnlsSpace = new System.Windows.Forms.Label();
            this.lblCtrlCnlsStart = new System.Windows.Forms.Label();
            this.numCtrlCnlsShift = new System.Windows.Forms.NumericUpDown();
            this.numCtrlCnlsMultiple = new System.Windows.Forms.NumericUpDown();
            this.numCtrlCnlsStart = new System.Windows.Forms.NumericUpDown();
            this.lblCtrlCnlsShift = new System.Windows.Forms.Label();
            this.lblCtrlCnlsMult = new System.Windows.Forms.Label();
            this.gvKPSel = new System.Windows.Forms.DataGridView();
            this.colSelected = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colKPName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colObjNum = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colDllFileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDllState = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colInCnls = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCtrlCnls = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gbInCnlsNumbering.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numInCnlsSpace)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numInCnlsShift)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numInCnlsMultiple)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numInCnlsStart)).BeginInit();
            this.gbCtrlCnlsNumbering.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCtrlCnlsSpace)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCtrlCnlsShift)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCtrlCnlsMultiple)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCtrlCnlsStart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvKPSel)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Location = new System.Drawing.Point(441, 12);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(90, 23);
            this.btnSelectAll.TabIndex = 1;
            this.btnSelectAll.Text = "Выбрать все";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // btnDeselectAll
            // 
            this.btnDeselectAll.Location = new System.Drawing.Point(537, 12);
            this.btnDeselectAll.Name = "btnDeselectAll";
            this.btnDeselectAll.Size = new System.Drawing.Size(90, 23);
            this.btnDeselectAll.TabIndex = 2;
            this.btnDeselectAll.Text = "Отменить все";
            this.btnDeselectAll.UseVisualStyleBackColor = true;
            this.btnDeselectAll.Click += new System.EventHandler(this.btnDeselectAll_Click);
            // 
            // lblKPSel
            // 
            this.lblKPSel.AutoSize = true;
            this.lblKPSel.Location = new System.Drawing.Point(12, 17);
            this.lblKPSel.Name = "lblKPSel";
            this.lblKPSel.Size = new System.Drawing.Size(178, 13);
            this.lblKPSel.TabIndex = 0;
            this.lblKPSel.Text = "Выбор КП для создания каналов:";
            // 
            // gbInCnlsNumbering
            // 
            this.gbInCnlsNumbering.Controls.Add(this.numInCnlsSpace);
            this.gbInCnlsNumbering.Controls.Add(this.lblInCnlsSpace);
            this.gbInCnlsNumbering.Controls.Add(this.lblInCnlsStart);
            this.gbInCnlsNumbering.Controls.Add(this.numInCnlsShift);
            this.gbInCnlsNumbering.Controls.Add(this.numInCnlsMultiple);
            this.gbInCnlsNumbering.Controls.Add(this.numInCnlsStart);
            this.gbInCnlsNumbering.Controls.Add(this.lblInCnlsShift);
            this.gbInCnlsNumbering.Controls.Add(this.lblInCnlsMult);
            this.gbInCnlsNumbering.Location = new System.Drawing.Point(12, 351);
            this.gbInCnlsNumbering.Name = "gbInCnlsNumbering";
            this.gbInCnlsNumbering.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbInCnlsNumbering.Size = new System.Drawing.Size(307, 130);
            this.gbInCnlsNumbering.TabIndex = 4;
            this.gbInCnlsNumbering.TabStop = false;
            this.gbInCnlsNumbering.Text = "Нумерация входных каналов";
            // 
            // numInCnlsSpace
            // 
            this.numInCnlsSpace.Location = new System.Drawing.Point(233, 97);
            this.numInCnlsSpace.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numInCnlsSpace.Name = "numInCnlsSpace";
            this.numInCnlsSpace.Size = new System.Drawing.Size(50, 20);
            this.numInCnlsSpace.TabIndex = 7;
            this.numInCnlsSpace.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numInCnlsSpace.ValueChanged += new System.EventHandler(this.numCnls_ValueChanged);
            // 
            // lblInCnlsSpace
            // 
            this.lblInCnlsSpace.AutoSize = true;
            this.lblInCnlsSpace.Location = new System.Drawing.Point(13, 101);
            this.lblInCnlsSpace.Name = "lblInCnlsSpace";
            this.lblInCnlsSpace.Size = new System.Drawing.Size(214, 13);
            this.lblInCnlsSpace.TabIndex = 6;
            this.lblInCnlsSpace.Text = "Свободных номеров между КП не менее";
            // 
            // lblInCnlsStart
            // 
            this.lblInCnlsStart.AutoSize = true;
            this.lblInCnlsStart.Location = new System.Drawing.Point(13, 23);
            this.lblInCnlsStart.Name = "lblInCnlsStart";
            this.lblInCnlsStart.Size = new System.Drawing.Size(128, 13);
            this.lblInCnlsStart.TabIndex = 0;
            this.lblInCnlsStart.Text = "Нумеровать, начиная с ";
            // 
            // numInCnlsShift
            // 
            this.numInCnlsShift.Location = new System.Drawing.Point(233, 71);
            this.numInCnlsShift.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numInCnlsShift.Name = "numInCnlsShift";
            this.numInCnlsShift.Size = new System.Drawing.Size(50, 20);
            this.numInCnlsShift.TabIndex = 5;
            this.numInCnlsShift.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numInCnlsShift.ValueChanged += new System.EventHandler(this.numCnls_ValueChanged);
            // 
            // numInCnlsMultiple
            // 
            this.numInCnlsMultiple.Location = new System.Drawing.Point(233, 45);
            this.numInCnlsMultiple.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numInCnlsMultiple.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numInCnlsMultiple.Name = "numInCnlsMultiple";
            this.numInCnlsMultiple.Size = new System.Drawing.Size(50, 20);
            this.numInCnlsMultiple.TabIndex = 3;
            this.numInCnlsMultiple.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numInCnlsMultiple.ValueChanged += new System.EventHandler(this.numCnls_ValueChanged);
            // 
            // numInCnlsStart
            // 
            this.numInCnlsStart.Location = new System.Drawing.Point(233, 19);
            this.numInCnlsStart.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numInCnlsStart.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numInCnlsStart.Name = "numInCnlsStart";
            this.numInCnlsStart.Size = new System.Drawing.Size(50, 20);
            this.numInCnlsStart.TabIndex = 1;
            this.numInCnlsStart.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numInCnlsStart.ValueChanged += new System.EventHandler(this.numCnls_ValueChanged);
            // 
            // lblInCnlsShift
            // 
            this.lblInCnlsShift.AutoSize = true;
            this.lblInCnlsShift.Location = new System.Drawing.Point(13, 75);
            this.lblInCnlsShift.Name = "lblInCnlsShift";
            this.lblInCnlsShift.Size = new System.Drawing.Size(183, 13);
            this.lblInCnlsShift.TabIndex = 4;
            this.lblInCnlsShift.Text = "Смещение первого канала для КП";
            // 
            // lblInCnlsMult
            // 
            this.lblInCnlsMult.AutoSize = true;
            this.lblInCnlsMult.Location = new System.Drawing.Point(13, 49);
            this.lblInCnlsMult.Name = "lblInCnlsMult";
            this.lblInCnlsMult.Size = new System.Drawing.Size(182, 13);
            this.lblInCnlsMult.TabIndex = 2;
            this.lblInCnlsMult.Text = "Кратность первого канала для КП";
            // 
            // btnCalc
            // 
            this.btnCalc.Location = new System.Drawing.Point(12, 487);
            this.btnCalc.Name = "btnCalc";
            this.btnCalc.Size = new System.Drawing.Size(160, 23);
            this.btnCalc.TabIndex = 6;
            this.btnCalc.Text = "Расчитать номера каналов";
            this.btnCalc.UseVisualStyleBackColor = true;
            this.btnCalc.Click += new System.EventHandler(this.btnCalc_Click);
            // 
            // btnCreate
            // 
            this.btnCreate.Location = new System.Drawing.Point(476, 487);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(75, 23);
            this.btnCreate.TabIndex = 7;
            this.btnCreate.Text = "Создать";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(557, 487);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 8;
            this.btnClose.Text = "Закрыть";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // gbCtrlCnlsNumbering
            // 
            this.gbCtrlCnlsNumbering.Controls.Add(this.numCtrlCnlsSpace);
            this.gbCtrlCnlsNumbering.Controls.Add(this.lblCtrlCnlsSpace);
            this.gbCtrlCnlsNumbering.Controls.Add(this.lblCtrlCnlsStart);
            this.gbCtrlCnlsNumbering.Controls.Add(this.numCtrlCnlsShift);
            this.gbCtrlCnlsNumbering.Controls.Add(this.numCtrlCnlsMultiple);
            this.gbCtrlCnlsNumbering.Controls.Add(this.numCtrlCnlsStart);
            this.gbCtrlCnlsNumbering.Controls.Add(this.lblCtrlCnlsShift);
            this.gbCtrlCnlsNumbering.Controls.Add(this.lblCtrlCnlsMult);
            this.gbCtrlCnlsNumbering.Location = new System.Drawing.Point(325, 351);
            this.gbCtrlCnlsNumbering.Name = "gbCtrlCnlsNumbering";
            this.gbCtrlCnlsNumbering.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbCtrlCnlsNumbering.Size = new System.Drawing.Size(307, 130);
            this.gbCtrlCnlsNumbering.TabIndex = 5;
            this.gbCtrlCnlsNumbering.TabStop = false;
            this.gbCtrlCnlsNumbering.Text = "Нумерация каналов управления";
            // 
            // numCtrlCnlsSpace
            // 
            this.numCtrlCnlsSpace.Location = new System.Drawing.Point(233, 97);
            this.numCtrlCnlsSpace.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numCtrlCnlsSpace.Name = "numCtrlCnlsSpace";
            this.numCtrlCnlsSpace.Size = new System.Drawing.Size(50, 20);
            this.numCtrlCnlsSpace.TabIndex = 7;
            this.numCtrlCnlsSpace.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numCtrlCnlsSpace.ValueChanged += new System.EventHandler(this.numCnls_ValueChanged);
            // 
            // lblCtrlCnlsSpace
            // 
            this.lblCtrlCnlsSpace.AutoSize = true;
            this.lblCtrlCnlsSpace.Location = new System.Drawing.Point(13, 101);
            this.lblCtrlCnlsSpace.Name = "lblCtrlCnlsSpace";
            this.lblCtrlCnlsSpace.Size = new System.Drawing.Size(214, 13);
            this.lblCtrlCnlsSpace.TabIndex = 6;
            this.lblCtrlCnlsSpace.Text = "Свободных номеров между КП не менее";
            // 
            // lblCtrlCnlsStart
            // 
            this.lblCtrlCnlsStart.AutoSize = true;
            this.lblCtrlCnlsStart.Location = new System.Drawing.Point(13, 23);
            this.lblCtrlCnlsStart.Name = "lblCtrlCnlsStart";
            this.lblCtrlCnlsStart.Size = new System.Drawing.Size(128, 13);
            this.lblCtrlCnlsStart.TabIndex = 0;
            this.lblCtrlCnlsStart.Text = "Нумеровать, начиная с ";
            // 
            // numCtrlCnlsShift
            // 
            this.numCtrlCnlsShift.Location = new System.Drawing.Point(233, 71);
            this.numCtrlCnlsShift.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numCtrlCnlsShift.Name = "numCtrlCnlsShift";
            this.numCtrlCnlsShift.Size = new System.Drawing.Size(50, 20);
            this.numCtrlCnlsShift.TabIndex = 5;
            this.numCtrlCnlsShift.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numCtrlCnlsShift.ValueChanged += new System.EventHandler(this.numCnls_ValueChanged);
            // 
            // numCtrlCnlsMultiple
            // 
            this.numCtrlCnlsMultiple.Location = new System.Drawing.Point(233, 45);
            this.numCtrlCnlsMultiple.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numCtrlCnlsMultiple.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numCtrlCnlsMultiple.Name = "numCtrlCnlsMultiple";
            this.numCtrlCnlsMultiple.Size = new System.Drawing.Size(50, 20);
            this.numCtrlCnlsMultiple.TabIndex = 3;
            this.numCtrlCnlsMultiple.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numCtrlCnlsMultiple.ValueChanged += new System.EventHandler(this.numCnls_ValueChanged);
            // 
            // numCtrlCnlsStart
            // 
            this.numCtrlCnlsStart.Location = new System.Drawing.Point(232, 19);
            this.numCtrlCnlsStart.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numCtrlCnlsStart.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numCtrlCnlsStart.Name = "numCtrlCnlsStart";
            this.numCtrlCnlsStart.Size = new System.Drawing.Size(50, 20);
            this.numCtrlCnlsStart.TabIndex = 1;
            this.numCtrlCnlsStart.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numCtrlCnlsStart.ValueChanged += new System.EventHandler(this.numCnls_ValueChanged);
            // 
            // lblCtrlCnlsShift
            // 
            this.lblCtrlCnlsShift.AutoSize = true;
            this.lblCtrlCnlsShift.Location = new System.Drawing.Point(13, 75);
            this.lblCtrlCnlsShift.Name = "lblCtrlCnlsShift";
            this.lblCtrlCnlsShift.Size = new System.Drawing.Size(183, 13);
            this.lblCtrlCnlsShift.TabIndex = 4;
            this.lblCtrlCnlsShift.Text = "Смещение первого канала для КП";
            // 
            // lblCtrlCnlsMult
            // 
            this.lblCtrlCnlsMult.AutoSize = true;
            this.lblCtrlCnlsMult.Location = new System.Drawing.Point(13, 49);
            this.lblCtrlCnlsMult.Name = "lblCtrlCnlsMult";
            this.lblCtrlCnlsMult.Size = new System.Drawing.Size(182, 13);
            this.lblCtrlCnlsMult.TabIndex = 2;
            this.lblCtrlCnlsMult.Text = "Кратность первого канала для КП";
            // 
            // gvKPSel
            // 
            this.gvKPSel.AllowUserToAddRows = false;
            this.gvKPSel.AllowUserToDeleteRows = false;
            this.gvKPSel.AllowUserToResizeRows = false;
            this.gvKPSel.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvKPSel.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSelected,
            this.colKPName,
            this.colObjNum,
            this.colDllFileName,
            this.colDllState,
            this.colInCnls,
            this.colCtrlCnls});
            this.gvKPSel.Location = new System.Drawing.Point(12, 41);
            this.gvKPSel.MultiSelect = false;
            this.gvKPSel.Name = "gvKPSel";
            this.gvKPSel.RowHeadersVisible = false;
            this.gvKPSel.Size = new System.Drawing.Size(620, 304);
            this.gvKPSel.TabIndex = 3;
            this.gvKPSel.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.gvKPSel_CellBeginEdit);
            this.gvKPSel.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.gvKPSel_CellFormatting);
            // 
            // colSelected
            // 
            this.colSelected.DataPropertyName = "Selected";
            this.colSelected.HeaderText = "";
            this.colSelected.Name = "colSelected";
            this.colSelected.Width = 25;
            // 
            // colKPName
            // 
            this.colKPName.DataPropertyName = "KPName";
            this.colKPName.HeaderText = "КП";
            this.colKPName.Name = "colKPName";
            this.colKPName.ReadOnly = true;
            this.colKPName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colKPName.Width = 110;
            // 
            // colObjNum
            // 
            this.colObjNum.DataPropertyName = "ObjNum";
            this.colObjNum.DisplayStyleForCurrentCellOnly = true;
            this.colObjNum.HeaderText = "Объект";
            this.colObjNum.Name = "colObjNum";
            this.colObjNum.Width = 115;
            // 
            // colDllFileName
            // 
            this.colDllFileName.DataPropertyName = "DllFileName";
            this.colDllFileName.HeaderText = "Имя файла DLL";
            this.colDllFileName.Name = "colDllFileName";
            this.colDllFileName.ReadOnly = true;
            this.colDllFileName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colDllFileName.Width = 95;
            // 
            // colDllState
            // 
            this.colDllState.DataPropertyName = "DllState";
            this.colDllState.HeaderText = "Состояние DLL";
            this.colDllState.Name = "colDllState";
            this.colDllState.ReadOnly = true;
            this.colDllState.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colDllState.Width = 95;
            // 
            // colInCnls
            // 
            this.colInCnls.DataPropertyName = "InCnls";
            this.colInCnls.HeaderText = "Вх. каналы";
            this.colInCnls.Name = "colInCnls";
            this.colInCnls.ReadOnly = true;
            this.colInCnls.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colInCnls.Width = 80;
            // 
            // colCtrlCnls
            // 
            this.colCtrlCnls.DataPropertyName = "CtrlCnls";
            this.colCtrlCnls.HeaderText = "Каналы упр.";
            this.colCtrlCnls.Name = "colCtrlCnls";
            this.colCtrlCnls.ReadOnly = true;
            this.colCtrlCnls.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colCtrlCnls.Width = 80;
            // 
            // FrmCreateCnls
            // 
            this.AcceptButton = this.btnCreate;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(644, 522);
            this.Controls.Add(this.gbCtrlCnlsNumbering);
            this.Controls.Add(this.btnCalc);
            this.Controls.Add(this.btnCreate);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.gbInCnlsNumbering);
            this.Controls.Add(this.lblKPSel);
            this.Controls.Add(this.btnDeselectAll);
            this.Controls.Add(this.btnSelectAll);
            this.Controls.Add(this.gvKPSel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmCreateCnls";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Создание каналов";
            this.Load += new System.EventHandler(this.FrmCreateCnls_Load);
            this.Shown += new System.EventHandler(this.FrmCreateCnls_Shown);
            this.gbInCnlsNumbering.ResumeLayout(false);
            this.gbInCnlsNumbering.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numInCnlsSpace)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numInCnlsShift)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numInCnlsMultiple)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numInCnlsStart)).EndInit();
            this.gbCtrlCnlsNumbering.ResumeLayout(false);
            this.gbCtrlCnlsNumbering.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCtrlCnlsSpace)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCtrlCnlsShift)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCtrlCnlsMultiple)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCtrlCnlsStart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvKPSel)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.Button btnDeselectAll;
        private System.Windows.Forms.Label lblKPSel;
        private System.Windows.Forms.GroupBox gbInCnlsNumbering;
        private System.Windows.Forms.Label lblInCnlsShift;
        private System.Windows.Forms.Label lblInCnlsMult;
        private System.Windows.Forms.NumericUpDown numInCnlsMultiple;
        private System.Windows.Forms.NumericUpDown numInCnlsStart;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblInCnlsStart;
        private System.Windows.Forms.NumericUpDown numInCnlsShift;
        private System.Windows.Forms.Button btnCalc;
        private System.Windows.Forms.NumericUpDown numInCnlsSpace;
        private System.Windows.Forms.Label lblInCnlsSpace;
        private System.Windows.Forms.GroupBox gbCtrlCnlsNumbering;
        private System.Windows.Forms.NumericUpDown numCtrlCnlsSpace;
        private System.Windows.Forms.Label lblCtrlCnlsSpace;
        private System.Windows.Forms.Label lblCtrlCnlsStart;
        private System.Windows.Forms.NumericUpDown numCtrlCnlsShift;
        private System.Windows.Forms.NumericUpDown numCtrlCnlsMultiple;
        private System.Windows.Forms.NumericUpDown numCtrlCnlsStart;
        private System.Windows.Forms.Label lblCtrlCnlsShift;
        private System.Windows.Forms.Label lblCtrlCnlsMult;
        private System.Windows.Forms.DataGridView gvKPSel;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colSelected;
        private System.Windows.Forms.DataGridViewTextBoxColumn colKPName;
        private System.Windows.Forms.DataGridViewComboBoxColumn colObjNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDllFileName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDllState;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInCnls;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCtrlCnls;
    }
}