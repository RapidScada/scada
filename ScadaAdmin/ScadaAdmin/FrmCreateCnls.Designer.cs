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
            this.gbCnlsNumOptions = new System.Windows.Forms.GroupBox();
            this.lblCtrlCnls = new System.Windows.Forms.Label();
            this.lblInCnls = new System.Windows.Forms.Label();
            this.numCtrlCnlsSpace = new System.Windows.Forms.NumericUpDown();
            this.numInCnlsSpace = new System.Windows.Forms.NumericUpDown();
            this.lblInCnlsSpace = new System.Windows.Forms.Label();
            this.lblInCnlsStart = new System.Windows.Forms.Label();
            this.numCtrlCnlsShift = new System.Windows.Forms.NumericUpDown();
            this.numInCnlsShift = new System.Windows.Forms.NumericUpDown();
            this.numCtrlCnlsMultiple = new System.Windows.Forms.NumericUpDown();
            this.numInCnlsMultiple = new System.Windows.Forms.NumericUpDown();
            this.numCtrlCnlsStart = new System.Windows.Forms.NumericUpDown();
            this.numInCnlsStart = new System.Windows.Forms.NumericUpDown();
            this.lblInCnlsShift = new System.Windows.Forms.Label();
            this.lblInCnlsMult = new System.Windows.Forms.Label();
            this.btnCalc = new System.Windows.Forms.Button();
            this.btnCreate = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.gvKPSel = new System.Windows.Forms.DataGridView();
            this.colSelected = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colKPName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colObjNum = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colDllFileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colInCnls = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCtrlCnls = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chkInsertKPName = new System.Windows.Forms.CheckBox();
            this.gbAdditionalOptions = new System.Windows.Forms.GroupBox();
            this.cbKPFilter = new System.Windows.Forms.ComboBox();
            this.gbCnlsNumOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCtrlCnlsSpace)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numInCnlsSpace)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCtrlCnlsShift)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numInCnlsShift)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCtrlCnlsMultiple)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numInCnlsMultiple)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCtrlCnlsStart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numInCnlsStart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvKPSel)).BeginInit();
            this.gbAdditionalOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Location = new System.Drawing.Point(416, 24);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(90, 23);
            this.btnSelectAll.TabIndex = 2;
            this.btnSelectAll.Text = "Выбрать все";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // btnDeselectAll
            // 
            this.btnDeselectAll.Location = new System.Drawing.Point(512, 24);
            this.btnDeselectAll.Name = "btnDeselectAll";
            this.btnDeselectAll.Size = new System.Drawing.Size(90, 23);
            this.btnDeselectAll.TabIndex = 3;
            this.btnDeselectAll.Text = "Отменить все";
            this.btnDeselectAll.UseVisualStyleBackColor = true;
            this.btnDeselectAll.Click += new System.EventHandler(this.btnDeselectAll_Click);
            // 
            // lblKPSel
            // 
            this.lblKPSel.AutoSize = true;
            this.lblKPSel.Location = new System.Drawing.Point(9, 9);
            this.lblKPSel.Name = "lblKPSel";
            this.lblKPSel.Size = new System.Drawing.Size(192, 13);
            this.lblKPSel.TabIndex = 0;
            this.lblKPSel.Text = "Выберите КП для создания каналов";
            // 
            // gbCnlsNumOptions
            // 
            this.gbCnlsNumOptions.Controls.Add(this.lblCtrlCnls);
            this.gbCnlsNumOptions.Controls.Add(this.lblInCnls);
            this.gbCnlsNumOptions.Controls.Add(this.numCtrlCnlsSpace);
            this.gbCnlsNumOptions.Controls.Add(this.numInCnlsSpace);
            this.gbCnlsNumOptions.Controls.Add(this.lblInCnlsSpace);
            this.gbCnlsNumOptions.Controls.Add(this.lblInCnlsStart);
            this.gbCnlsNumOptions.Controls.Add(this.numCtrlCnlsShift);
            this.gbCnlsNumOptions.Controls.Add(this.numInCnlsShift);
            this.gbCnlsNumOptions.Controls.Add(this.numCtrlCnlsMultiple);
            this.gbCnlsNumOptions.Controls.Add(this.numInCnlsMultiple);
            this.gbCnlsNumOptions.Controls.Add(this.numCtrlCnlsStart);
            this.gbCnlsNumOptions.Controls.Add(this.numInCnlsStart);
            this.gbCnlsNumOptions.Controls.Add(this.lblInCnlsShift);
            this.gbCnlsNumOptions.Controls.Add(this.lblInCnlsMult);
            this.gbCnlsNumOptions.Location = new System.Drawing.Point(12, 338);
            this.gbCnlsNumOptions.Name = "gbCnlsNumOptions";
            this.gbCnlsNumOptions.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbCnlsNumOptions.Size = new System.Drawing.Size(352, 143);
            this.gbCnlsNumOptions.TabIndex = 5;
            this.gbCnlsNumOptions.TabStop = false;
            this.gbCnlsNumOptions.Text = "Нумерация каналов";
            // 
            // lblCtrlCnls
            // 
            this.lblCtrlCnls.AutoSize = true;
            this.lblCtrlCnls.Location = new System.Drawing.Point(286, 16);
            this.lblCtrlCnls.Name = "lblCtrlCnls";
            this.lblCtrlCnls.Size = new System.Drawing.Size(52, 13);
            this.lblCtrlCnls.TabIndex = 1;
            this.lblCtrlCnls.Text = "Кан. упр.";
            // 
            // lblInCnls
            // 
            this.lblInCnls.AutoSize = true;
            this.lblInCnls.Location = new System.Drawing.Point(230, 16);
            this.lblInCnls.Name = "lblInCnls";
            this.lblInCnls.Size = new System.Drawing.Size(46, 13);
            this.lblInCnls.TabIndex = 0;
            this.lblInCnls.Text = "Вх. кан.";
            // 
            // numCtrlCnlsSpace
            // 
            this.numCtrlCnlsSpace.Location = new System.Drawing.Point(289, 110);
            this.numCtrlCnlsSpace.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numCtrlCnlsSpace.Name = "numCtrlCnlsSpace";
            this.numCtrlCnlsSpace.Size = new System.Drawing.Size(50, 20);
            this.numCtrlCnlsSpace.TabIndex = 13;
            this.numCtrlCnlsSpace.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numCtrlCnlsSpace.ValueChanged += new System.EventHandler(this.numCnls_ValueChanged);
            // 
            // numInCnlsSpace
            // 
            this.numInCnlsSpace.Location = new System.Drawing.Point(233, 110);
            this.numInCnlsSpace.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numInCnlsSpace.Name = "numInCnlsSpace";
            this.numInCnlsSpace.Size = new System.Drawing.Size(50, 20);
            this.numInCnlsSpace.TabIndex = 12;
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
            this.lblInCnlsSpace.Location = new System.Drawing.Point(13, 114);
            this.lblInCnlsSpace.Name = "lblInCnlsSpace";
            this.lblInCnlsSpace.Size = new System.Drawing.Size(214, 13);
            this.lblInCnlsSpace.TabIndex = 11;
            this.lblInCnlsSpace.Text = "Свободных номеров между КП не менее";
            // 
            // lblInCnlsStart
            // 
            this.lblInCnlsStart.AutoSize = true;
            this.lblInCnlsStart.Location = new System.Drawing.Point(13, 36);
            this.lblInCnlsStart.Name = "lblInCnlsStart";
            this.lblInCnlsStart.Size = new System.Drawing.Size(128, 13);
            this.lblInCnlsStart.TabIndex = 2;
            this.lblInCnlsStart.Text = "Нумеровать, начиная с ";
            // 
            // numCtrlCnlsShift
            // 
            this.numCtrlCnlsShift.Location = new System.Drawing.Point(290, 84);
            this.numCtrlCnlsShift.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numCtrlCnlsShift.Name = "numCtrlCnlsShift";
            this.numCtrlCnlsShift.Size = new System.Drawing.Size(50, 20);
            this.numCtrlCnlsShift.TabIndex = 10;
            this.numCtrlCnlsShift.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numCtrlCnlsShift.ValueChanged += new System.EventHandler(this.numCnls_ValueChanged);
            // 
            // numInCnlsShift
            // 
            this.numInCnlsShift.Location = new System.Drawing.Point(233, 84);
            this.numInCnlsShift.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numInCnlsShift.Name = "numInCnlsShift";
            this.numInCnlsShift.Size = new System.Drawing.Size(50, 20);
            this.numInCnlsShift.TabIndex = 9;
            this.numInCnlsShift.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numInCnlsShift.ValueChanged += new System.EventHandler(this.numCnls_ValueChanged);
            // 
            // numCtrlCnlsMultiple
            // 
            this.numCtrlCnlsMultiple.Location = new System.Drawing.Point(290, 58);
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
            this.numCtrlCnlsMultiple.TabIndex = 7;
            this.numCtrlCnlsMultiple.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numCtrlCnlsMultiple.ValueChanged += new System.EventHandler(this.numCnls_ValueChanged);
            // 
            // numInCnlsMultiple
            // 
            this.numInCnlsMultiple.Location = new System.Drawing.Point(233, 58);
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
            this.numInCnlsMultiple.TabIndex = 6;
            this.numInCnlsMultiple.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numInCnlsMultiple.ValueChanged += new System.EventHandler(this.numCnls_ValueChanged);
            // 
            // numCtrlCnlsStart
            // 
            this.numCtrlCnlsStart.Location = new System.Drawing.Point(289, 32);
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
            this.numCtrlCnlsStart.TabIndex = 4;
            this.numCtrlCnlsStart.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numCtrlCnlsStart.ValueChanged += new System.EventHandler(this.numCnls_ValueChanged);
            // 
            // numInCnlsStart
            // 
            this.numInCnlsStart.Location = new System.Drawing.Point(233, 32);
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
            this.numInCnlsStart.TabIndex = 3;
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
            this.lblInCnlsShift.Location = new System.Drawing.Point(13, 88);
            this.lblInCnlsShift.Name = "lblInCnlsShift";
            this.lblInCnlsShift.Size = new System.Drawing.Size(183, 13);
            this.lblInCnlsShift.TabIndex = 8;
            this.lblInCnlsShift.Text = "Смещение первого канала для КП";
            // 
            // lblInCnlsMult
            // 
            this.lblInCnlsMult.AutoSize = true;
            this.lblInCnlsMult.Location = new System.Drawing.Point(13, 62);
            this.lblInCnlsMult.Name = "lblInCnlsMult";
            this.lblInCnlsMult.Size = new System.Drawing.Size(182, 13);
            this.lblInCnlsMult.TabIndex = 5;
            this.lblInCnlsMult.Text = "Кратность первого канала для КП";
            // 
            // btnCalc
            // 
            this.btnCalc.Location = new System.Drawing.Point(12, 487);
            this.btnCalc.Name = "btnCalc";
            this.btnCalc.Size = new System.Drawing.Size(170, 23);
            this.btnCalc.TabIndex = 7;
            this.btnCalc.Text = "Рассчитать номера каналов";
            this.btnCalc.UseVisualStyleBackColor = true;
            this.btnCalc.Click += new System.EventHandler(this.btnCalc_Click);
            // 
            // btnCreate
            // 
            this.btnCreate.Location = new System.Drawing.Point(446, 487);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(75, 23);
            this.btnCreate.TabIndex = 8;
            this.btnCreate.Text = "Создать";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(527, 487);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 9;
            this.btnClose.Text = "Закрыть";
            this.btnClose.UseVisualStyleBackColor = true;
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
            this.colInCnls,
            this.colCtrlCnls});
            this.gvKPSel.Location = new System.Drawing.Point(12, 52);
            this.gvKPSel.MultiSelect = false;
            this.gvKPSel.Name = "gvKPSel";
            this.gvKPSel.RowHeadersVisible = false;
            this.gvKPSel.Size = new System.Drawing.Size(590, 280);
            this.gvKPSel.TabIndex = 4;
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
            this.colKPName.Width = 125;
            // 
            // colObjNum
            // 
            this.colObjNum.DataPropertyName = "ObjNum";
            this.colObjNum.DisplayStyleForCurrentCellOnly = true;
            this.colObjNum.HeaderText = "Объект";
            this.colObjNum.Name = "colObjNum";
            this.colObjNum.Width = 125;
            // 
            // colDllFileName
            // 
            this.colDllFileName.DataPropertyName = "DllWithState";
            this.colDllFileName.HeaderText = "DLL";
            this.colDllFileName.Name = "colDllFileName";
            this.colDllFileName.ReadOnly = true;
            this.colDllFileName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colDllFileName.Width = 125;
            // 
            // colInCnls
            // 
            this.colInCnls.DataPropertyName = "InCnls";
            this.colInCnls.HeaderText = "Вх. каналы";
            this.colInCnls.Name = "colInCnls";
            this.colInCnls.ReadOnly = true;
            this.colInCnls.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colInCnls.Width = 85;
            // 
            // colCtrlCnls
            // 
            this.colCtrlCnls.DataPropertyName = "CtrlCnls";
            this.colCtrlCnls.HeaderText = "Каналы упр.";
            this.colCtrlCnls.Name = "colCtrlCnls";
            this.colCtrlCnls.ReadOnly = true;
            this.colCtrlCnls.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colCtrlCnls.Width = 85;
            // 
            // chkInsertKPName
            // 
            this.chkInsertKPName.AutoSize = true;
            this.chkInsertKPName.Checked = true;
            this.chkInsertKPName.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkInsertKPName.Location = new System.Drawing.Point(13, 32);
            this.chkInsertKPName.Name = "chkInsertKPName";
            this.chkInsertKPName.Size = new System.Drawing.Size(185, 17);
            this.chkInsertKPName.TabIndex = 0;
            this.chkInsertKPName.Text = "Вставить имя КП в имя канала";
            this.chkInsertKPName.UseVisualStyleBackColor = true;
            // 
            // gbAdditionalOptions
            // 
            this.gbAdditionalOptions.Controls.Add(this.chkInsertKPName);
            this.gbAdditionalOptions.Location = new System.Drawing.Point(370, 338);
            this.gbAdditionalOptions.Name = "gbAdditionalOptions";
            this.gbAdditionalOptions.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbAdditionalOptions.Size = new System.Drawing.Size(232, 143);
            this.gbAdditionalOptions.TabIndex = 6;
            this.gbAdditionalOptions.TabStop = false;
            this.gbAdditionalOptions.Text = "Дополнительные параметры";
            // 
            // cbKPFilter
            // 
            this.cbKPFilter.DisplayMember = "Name";
            this.cbKPFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbKPFilter.FormattingEnabled = true;
            this.cbKPFilter.Items.AddRange(new object[] {
            "<Фильтр КП по линии связи>"});
            this.cbKPFilter.Location = new System.Drawing.Point(12, 25);
            this.cbKPFilter.Name = "cbKPFilter";
            this.cbKPFilter.Size = new System.Drawing.Size(398, 21);
            this.cbKPFilter.TabIndex = 1;
            this.cbKPFilter.ValueMember = "CommLineNum";
            // 
            // FrmCreateCnls
            // 
            this.AcceptButton = this.btnCreate;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(614, 522);
            this.Controls.Add(this.cbKPFilter);
            this.Controls.Add(this.gbAdditionalOptions);
            this.Controls.Add(this.btnCalc);
            this.Controls.Add(this.btnCreate);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.gbCnlsNumOptions);
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
            this.gbCnlsNumOptions.ResumeLayout(false);
            this.gbCnlsNumOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCtrlCnlsSpace)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numInCnlsSpace)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCtrlCnlsShift)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numInCnlsShift)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCtrlCnlsMultiple)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numInCnlsMultiple)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCtrlCnlsStart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numInCnlsStart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvKPSel)).EndInit();
            this.gbAdditionalOptions.ResumeLayout(false);
            this.gbAdditionalOptions.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.Button btnDeselectAll;
        private System.Windows.Forms.Label lblKPSel;
        private System.Windows.Forms.GroupBox gbCnlsNumOptions;
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
        private System.Windows.Forms.NumericUpDown numCtrlCnlsSpace;
        private System.Windows.Forms.NumericUpDown numCtrlCnlsShift;
        private System.Windows.Forms.NumericUpDown numCtrlCnlsMultiple;
        private System.Windows.Forms.NumericUpDown numCtrlCnlsStart;
        private System.Windows.Forms.DataGridView gvKPSel;
        private System.Windows.Forms.CheckBox chkInsertKPName;
        private System.Windows.Forms.Label lblInCnls;
        private System.Windows.Forms.Label lblCtrlCnls;
        private System.Windows.Forms.GroupBox gbAdditionalOptions;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colSelected;
        private System.Windows.Forms.DataGridViewTextBoxColumn colKPName;
        private System.Windows.Forms.DataGridViewComboBoxColumn colObjNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDllFileName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInCnls;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCtrlCnls;
        private System.Windows.Forms.ComboBox cbKPFilter;
    }
}