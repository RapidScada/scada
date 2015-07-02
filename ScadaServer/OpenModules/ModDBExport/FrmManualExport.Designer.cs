namespace Scada.Server.Modules.DBExport
{
    partial class FrmManualExport
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
            this.cbDataSource = new System.Windows.Forms.ComboBox();
            this.lblDataSource = new System.Windows.Forms.Label();
            this.gbCurData = new System.Windows.Forms.GroupBox();
            this.numCurDataCtrlCnlNum = new System.Windows.Forms.NumericUpDown();
            this.lblCurDataCtrlCnlNum = new System.Windows.Forms.Label();
            this.btnExportCurData = new System.Windows.Forms.Button();
            this.gbArcData = new System.Windows.Forms.GroupBox();
            this.lblArcDataDateTime = new System.Windows.Forms.Label();
            this.dtpArcDataTime = new System.Windows.Forms.DateTimePicker();
            this.dtpArcDataDate = new System.Windows.Forms.DateTimePicker();
            this.numArcDataCtrlCnlNum = new System.Windows.Forms.NumericUpDown();
            this.lblArcDataCtrlCnlNum = new System.Windows.Forms.Label();
            this.btnExportArcData = new System.Windows.Forms.Button();
            this.gbEvents = new System.Windows.Forms.GroupBox();
            this.lblEventsDate = new System.Windows.Forms.Label();
            this.dtpEventsDate = new System.Windows.Forms.DateTimePicker();
            this.numEventsCtrlCnlNum = new System.Windows.Forms.NumericUpDown();
            this.lblEventsCtrlCnlNum = new System.Windows.Forms.Label();
            this.btnExportEvents = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.gbCurData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCurDataCtrlCnlNum)).BeginInit();
            this.gbArcData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numArcDataCtrlCnlNum)).BeginInit();
            this.gbEvents.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numEventsCtrlCnlNum)).BeginInit();
            this.SuspendLayout();
            // 
            // cbDataSource
            // 
            this.cbDataSource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDataSource.FormattingEnabled = true;
            this.cbDataSource.Location = new System.Drawing.Point(12, 25);
            this.cbDataSource.Name = "cbDataSource";
            this.cbDataSource.Size = new System.Drawing.Size(374, 21);
            this.cbDataSource.TabIndex = 1;
            // 
            // lblDataSource
            // 
            this.lblDataSource.AutoSize = true;
            this.lblDataSource.Location = new System.Drawing.Point(9, 9);
            this.lblDataSource.Name = "lblDataSource";
            this.lblDataSource.Size = new System.Drawing.Size(95, 13);
            this.lblDataSource.TabIndex = 0;
            this.lblDataSource.Text = "Источник данных";
            // 
            // gbCurData
            // 
            this.gbCurData.Controls.Add(this.numCurDataCtrlCnlNum);
            this.gbCurData.Controls.Add(this.lblCurDataCtrlCnlNum);
            this.gbCurData.Controls.Add(this.btnExportCurData);
            this.gbCurData.Location = new System.Drawing.Point(12, 52);
            this.gbCurData.Name = "gbCurData";
            this.gbCurData.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbCurData.Size = new System.Drawing.Size(374, 67);
            this.gbCurData.TabIndex = 2;
            this.gbCurData.TabStop = false;
            this.gbCurData.Text = "Текущие данные";
            // 
            // numCurDataCtrlCnlNum
            // 
            this.numCurDataCtrlCnlNum.Location = new System.Drawing.Point(13, 32);
            this.numCurDataCtrlCnlNum.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numCurDataCtrlCnlNum.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numCurDataCtrlCnlNum.Name = "numCurDataCtrlCnlNum";
            this.numCurDataCtrlCnlNum.Size = new System.Drawing.Size(70, 20);
            this.numCurDataCtrlCnlNum.TabIndex = 1;
            this.numCurDataCtrlCnlNum.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblCurDataCtrlCnlNum
            // 
            this.lblCurDataCtrlCnlNum.AutoSize = true;
            this.lblCurDataCtrlCnlNum.Location = new System.Drawing.Point(10, 16);
            this.lblCurDataCtrlCnlNum.Name = "lblCurDataCtrlCnlNum";
            this.lblCurDataCtrlCnlNum.Size = new System.Drawing.Size(61, 13);
            this.lblCurDataCtrlCnlNum.TabIndex = 0;
            this.lblCurDataCtrlCnlNum.Text = "Канал упр.";
            // 
            // btnExportCurData
            // 
            this.btnExportCurData.Location = new System.Drawing.Point(89, 31);
            this.btnExportCurData.Name = "btnExportCurData";
            this.btnExportCurData.Size = new System.Drawing.Size(75, 23);
            this.btnExportCurData.TabIndex = 2;
            this.btnExportCurData.Text = "Экспорт";
            this.btnExportCurData.UseVisualStyleBackColor = true;
            this.btnExportCurData.Click += new System.EventHandler(this.btnExportCurData_Click);
            // 
            // gbArcData
            // 
            this.gbArcData.Controls.Add(this.lblArcDataDateTime);
            this.gbArcData.Controls.Add(this.dtpArcDataTime);
            this.gbArcData.Controls.Add(this.dtpArcDataDate);
            this.gbArcData.Controls.Add(this.numArcDataCtrlCnlNum);
            this.gbArcData.Controls.Add(this.lblArcDataCtrlCnlNum);
            this.gbArcData.Controls.Add(this.btnExportArcData);
            this.gbArcData.Location = new System.Drawing.Point(12, 125);
            this.gbArcData.Name = "gbArcData";
            this.gbArcData.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbArcData.Size = new System.Drawing.Size(374, 67);
            this.gbArcData.TabIndex = 3;
            this.gbArcData.TabStop = false;
            this.gbArcData.Text = "Архивные данные";
            // 
            // lblArcDataDateTime
            // 
            this.lblArcDataDateTime.AutoSize = true;
            this.lblArcDataDateTime.Location = new System.Drawing.Point(86, 16);
            this.lblArcDataDateTime.Name = "lblArcDataDateTime";
            this.lblArcDataDateTime.Size = new System.Drawing.Size(77, 13);
            this.lblArcDataDateTime.TabIndex = 2;
            this.lblArcDataDateTime.Text = "Дата и время";
            // 
            // dtpArcDataTime
            // 
            this.dtpArcDataTime.CustomFormat = "HH:mm:ss";
            this.dtpArcDataTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpArcDataTime.Location = new System.Drawing.Point(195, 32);
            this.dtpArcDataTime.Name = "dtpArcDataTime";
            this.dtpArcDataTime.ShowUpDown = true;
            this.dtpArcDataTime.Size = new System.Drawing.Size(85, 20);
            this.dtpArcDataTime.TabIndex = 4;
            // 
            // dtpArcDataDate
            // 
            this.dtpArcDataDate.CustomFormat = "dd.MM.yyyy";
            this.dtpArcDataDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpArcDataDate.Location = new System.Drawing.Point(89, 32);
            this.dtpArcDataDate.Name = "dtpArcDataDate";
            this.dtpArcDataDate.Size = new System.Drawing.Size(100, 20);
            this.dtpArcDataDate.TabIndex = 3;
            // 
            // numArcDataCtrlCnlNum
            // 
            this.numArcDataCtrlCnlNum.Location = new System.Drawing.Point(13, 32);
            this.numArcDataCtrlCnlNum.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numArcDataCtrlCnlNum.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numArcDataCtrlCnlNum.Name = "numArcDataCtrlCnlNum";
            this.numArcDataCtrlCnlNum.Size = new System.Drawing.Size(70, 20);
            this.numArcDataCtrlCnlNum.TabIndex = 1;
            this.numArcDataCtrlCnlNum.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // lblArcDataCtrlCnlNum
            // 
            this.lblArcDataCtrlCnlNum.AutoSize = true;
            this.lblArcDataCtrlCnlNum.Location = new System.Drawing.Point(10, 16);
            this.lblArcDataCtrlCnlNum.Name = "lblArcDataCtrlCnlNum";
            this.lblArcDataCtrlCnlNum.Size = new System.Drawing.Size(61, 13);
            this.lblArcDataCtrlCnlNum.TabIndex = 0;
            this.lblArcDataCtrlCnlNum.Text = "Канал упр.";
            // 
            // btnExportArcData
            // 
            this.btnExportArcData.Location = new System.Drawing.Point(286, 31);
            this.btnExportArcData.Name = "btnExportArcData";
            this.btnExportArcData.Size = new System.Drawing.Size(75, 23);
            this.btnExportArcData.TabIndex = 5;
            this.btnExportArcData.Text = "Экспорт";
            this.btnExportArcData.UseVisualStyleBackColor = true;
            this.btnExportArcData.Click += new System.EventHandler(this.btnExportArcData_Click);
            // 
            // gbEvents
            // 
            this.gbEvents.Controls.Add(this.lblEventsDate);
            this.gbEvents.Controls.Add(this.dtpEventsDate);
            this.gbEvents.Controls.Add(this.numEventsCtrlCnlNum);
            this.gbEvents.Controls.Add(this.lblEventsCtrlCnlNum);
            this.gbEvents.Controls.Add(this.btnExportEvents);
            this.gbEvents.Location = new System.Drawing.Point(12, 198);
            this.gbEvents.Name = "gbEvents";
            this.gbEvents.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbEvents.Size = new System.Drawing.Size(374, 67);
            this.gbEvents.TabIndex = 4;
            this.gbEvents.TabStop = false;
            this.gbEvents.Text = "События";
            // 
            // lblEventsDate
            // 
            this.lblEventsDate.AutoSize = true;
            this.lblEventsDate.Location = new System.Drawing.Point(86, 16);
            this.lblEventsDate.Name = "lblEventsDate";
            this.lblEventsDate.Size = new System.Drawing.Size(33, 13);
            this.lblEventsDate.TabIndex = 2;
            this.lblEventsDate.Text = "Дата";
            // 
            // dtpEventsDate
            // 
            this.dtpEventsDate.CustomFormat = "dd.MM.yyyy";
            this.dtpEventsDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpEventsDate.Location = new System.Drawing.Point(89, 32);
            this.dtpEventsDate.Name = "dtpEventsDate";
            this.dtpEventsDate.Size = new System.Drawing.Size(100, 20);
            this.dtpEventsDate.TabIndex = 3;
            // 
            // numEventsCtrlCnlNum
            // 
            this.numEventsCtrlCnlNum.Location = new System.Drawing.Point(13, 32);
            this.numEventsCtrlCnlNum.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numEventsCtrlCnlNum.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numEventsCtrlCnlNum.Name = "numEventsCtrlCnlNum";
            this.numEventsCtrlCnlNum.Size = new System.Drawing.Size(70, 20);
            this.numEventsCtrlCnlNum.TabIndex = 1;
            this.numEventsCtrlCnlNum.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // lblEventsCtrlCnlNum
            // 
            this.lblEventsCtrlCnlNum.AutoSize = true;
            this.lblEventsCtrlCnlNum.Location = new System.Drawing.Point(10, 16);
            this.lblEventsCtrlCnlNum.Name = "lblEventsCtrlCnlNum";
            this.lblEventsCtrlCnlNum.Size = new System.Drawing.Size(61, 13);
            this.lblEventsCtrlCnlNum.TabIndex = 0;
            this.lblEventsCtrlCnlNum.Text = "Канал упр.";
            // 
            // btnExportEvents
            // 
            this.btnExportEvents.Location = new System.Drawing.Point(195, 31);
            this.btnExportEvents.Name = "btnExportEvents";
            this.btnExportEvents.Size = new System.Drawing.Size(75, 23);
            this.btnExportEvents.TabIndex = 4;
            this.btnExportEvents.Text = "Экспорт";
            this.btnExportEvents.UseVisualStyleBackColor = true;
            this.btnExportEvents.Click += new System.EventHandler(this.btnExportEvents_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(311, 271);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(230, 271);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // FrmManualExport
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(398, 306);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.gbEvents);
            this.Controls.Add(this.gbArcData);
            this.Controls.Add(this.gbCurData);
            this.Controls.Add(this.lblDataSource);
            this.Controls.Add(this.cbDataSource);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmManualExport";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Экспорт в ручном режиме";
            this.gbCurData.ResumeLayout(false);
            this.gbCurData.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCurDataCtrlCnlNum)).EndInit();
            this.gbArcData.ResumeLayout(false);
            this.gbArcData.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numArcDataCtrlCnlNum)).EndInit();
            this.gbEvents.ResumeLayout(false);
            this.gbEvents.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numEventsCtrlCnlNum)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbDataSource;
        private System.Windows.Forms.Label lblDataSource;
        private System.Windows.Forms.GroupBox gbCurData;
        private System.Windows.Forms.Button btnExportCurData;
        private System.Windows.Forms.NumericUpDown numCurDataCtrlCnlNum;
        private System.Windows.Forms.Label lblCurDataCtrlCnlNum;
        private System.Windows.Forms.GroupBox gbArcData;
        private System.Windows.Forms.NumericUpDown numArcDataCtrlCnlNum;
        private System.Windows.Forms.Label lblArcDataCtrlCnlNum;
        private System.Windows.Forms.Button btnExportArcData;
        private System.Windows.Forms.DateTimePicker dtpArcDataTime;
        private System.Windows.Forms.DateTimePicker dtpArcDataDate;
        private System.Windows.Forms.Label lblArcDataDateTime;
        private System.Windows.Forms.GroupBox gbEvents;
        private System.Windows.Forms.DateTimePicker dtpEventsDate;
        private System.Windows.Forms.NumericUpDown numEventsCtrlCnlNum;
        private System.Windows.Forms.Label lblEventsCtrlCnlNum;
        private System.Windows.Forms.Button btnExportEvents;
        private System.Windows.Forms.Label lblEventsDate;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
    }
}