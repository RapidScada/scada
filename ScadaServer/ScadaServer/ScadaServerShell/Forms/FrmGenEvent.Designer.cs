namespace Scada.Server.Shell.Forms
{
    partial class FrmGenEvent
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
            this.txtData = new System.Windows.Forms.TextBox();
            this.lblData = new System.Windows.Forms.Label();
            this.txtDescr = new System.Windows.Forms.TextBox();
            this.lblDescr = new System.Windows.Forms.Label();
            this.lblNewCnlVal = new System.Windows.Forms.Label();
            this.txtNewCnlVal = new System.Windows.Forms.TextBox();
            this.numUserID = new System.Windows.Forms.NumericUpDown();
            this.lblUserID = new System.Windows.Forms.Label();
            this.numNewCnlStat = new System.Windows.Forms.NumericUpDown();
            this.lblNewCnlStat = new System.Windows.Forms.Label();
            this.numOldCnlStat = new System.Windows.Forms.NumericUpDown();
            this.lblOldCnlStat = new System.Windows.Forms.Label();
            this.lblOldCnlVal = new System.Windows.Forms.Label();
            this.txtOldCnlVal = new System.Windows.Forms.TextBox();
            this.lblCnlNum = new System.Windows.Forms.Label();
            this.numCnlNum = new System.Windows.Forms.NumericUpDown();
            this.numParamID = new System.Windows.Forms.NumericUpDown();
            this.lblParamID = new System.Windows.Forms.Label();
            this.numKPNum = new System.Windows.Forms.NumericUpDown();
            this.lblKPNum = new System.Windows.Forms.Label();
            this.numObjNum = new System.Windows.Forms.NumericUpDown();
            this.lblObjNum = new System.Windows.Forms.Label();
            this.btnSetCurTime = new System.Windows.Forms.Button();
            this.lblTime = new System.Windows.Forms.Label();
            this.lblDate = new System.Windows.Forms.Label();
            this.dtpTime = new System.Windows.Forms.DateTimePicker();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSend = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numUserID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numNewCnlStat)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numOldCnlStat)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCnlNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numParamID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numKPNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numObjNum)).BeginInit();
            this.SuspendLayout();
            // 
            // txtData
            // 
            this.txtData.Location = new System.Drawing.Point(12, 181);
            this.txtData.MaxLength = 50;
            this.txtData.Name = "txtData";
            this.txtData.Size = new System.Drawing.Size(399, 20);
            this.txtData.TabIndex = 26;
            // 
            // lblData
            // 
            this.lblData.AutoSize = true;
            this.lblData.Location = new System.Drawing.Point(9, 165);
            this.lblData.Name = "lblData";
            this.lblData.Size = new System.Drawing.Size(30, 13);
            this.lblData.TabIndex = 25;
            this.lblData.Text = "Data";
            // 
            // txtDescr
            // 
            this.txtDescr.Location = new System.Drawing.Point(12, 142);
            this.txtDescr.MaxLength = 100;
            this.txtDescr.Name = "txtDescr";
            this.txtDescr.Size = new System.Drawing.Size(399, 20);
            this.txtDescr.TabIndex = 24;
            // 
            // lblDescr
            // 
            this.lblDescr.AutoSize = true;
            this.lblDescr.Location = new System.Drawing.Point(9, 126);
            this.lblDescr.Name = "lblDescr";
            this.lblDescr.Size = new System.Drawing.Size(60, 13);
            this.lblDescr.TabIndex = 23;
            this.lblDescr.Text = "Description";
            // 
            // lblNewCnlVal
            // 
            this.lblNewCnlVal.AutoSize = true;
            this.lblNewCnlVal.Location = new System.Drawing.Point(171, 87);
            this.lblNewCnlVal.Name = "lblNewCnlVal";
            this.lblNewCnlVal.Size = new System.Drawing.Size(58, 13);
            this.lblNewCnlVal.TabIndex = 17;
            this.lblNewCnlVal.Text = "New value";
            // 
            // txtNewCnlVal
            // 
            this.txtNewCnlVal.Location = new System.Drawing.Point(174, 103);
            this.txtNewCnlVal.Name = "txtNewCnlVal";
            this.txtNewCnlVal.Size = new System.Drawing.Size(75, 20);
            this.txtNewCnlVal.TabIndex = 18;
            this.txtNewCnlVal.Text = "0";
            // 
            // numUserID
            // 
            this.numUserID.Location = new System.Drawing.Point(336, 103);
            this.numUserID.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numUserID.Name = "numUserID";
            this.numUserID.Size = new System.Drawing.Size(75, 20);
            this.numUserID.TabIndex = 22;
            // 
            // lblUserID
            // 
            this.lblUserID.AutoSize = true;
            this.lblUserID.Location = new System.Drawing.Point(333, 87);
            this.lblUserID.Name = "lblUserID";
            this.lblUserID.Size = new System.Drawing.Size(29, 13);
            this.lblUserID.TabIndex = 21;
            this.lblUserID.Text = "User";
            // 
            // numNewCnlStat
            // 
            this.numNewCnlStat.Location = new System.Drawing.Point(255, 103);
            this.numNewCnlStat.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numNewCnlStat.Name = "numNewCnlStat";
            this.numNewCnlStat.Size = new System.Drawing.Size(75, 20);
            this.numNewCnlStat.TabIndex = 20;
            // 
            // lblNewCnlStat
            // 
            this.lblNewCnlStat.AutoSize = true;
            this.lblNewCnlStat.Location = new System.Drawing.Point(252, 87);
            this.lblNewCnlStat.Name = "lblNewCnlStat";
            this.lblNewCnlStat.Size = new System.Drawing.Size(60, 13);
            this.lblNewCnlStat.TabIndex = 19;
            this.lblNewCnlStat.Text = "New status";
            // 
            // numOldCnlStat
            // 
            this.numOldCnlStat.Location = new System.Drawing.Point(93, 103);
            this.numOldCnlStat.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numOldCnlStat.Name = "numOldCnlStat";
            this.numOldCnlStat.Size = new System.Drawing.Size(75, 20);
            this.numOldCnlStat.TabIndex = 16;
            // 
            // lblOldCnlStat
            // 
            this.lblOldCnlStat.AutoSize = true;
            this.lblOldCnlStat.Location = new System.Drawing.Point(90, 87);
            this.lblOldCnlStat.Name = "lblOldCnlStat";
            this.lblOldCnlStat.Size = new System.Drawing.Size(54, 13);
            this.lblOldCnlStat.TabIndex = 15;
            this.lblOldCnlStat.Text = "Old status";
            // 
            // lblOldCnlVal
            // 
            this.lblOldCnlVal.AutoSize = true;
            this.lblOldCnlVal.Location = new System.Drawing.Point(9, 87);
            this.lblOldCnlVal.Name = "lblOldCnlVal";
            this.lblOldCnlVal.Size = new System.Drawing.Size(52, 13);
            this.lblOldCnlVal.TabIndex = 13;
            this.lblOldCnlVal.Text = "Old value";
            // 
            // txtOldCnlVal
            // 
            this.txtOldCnlVal.Location = new System.Drawing.Point(12, 103);
            this.txtOldCnlVal.Name = "txtOldCnlVal";
            this.txtOldCnlVal.Size = new System.Drawing.Size(75, 20);
            this.txtOldCnlVal.TabIndex = 14;
            this.txtOldCnlVal.Text = "0";
            // 
            // lblCnlNum
            // 
            this.lblCnlNum.AutoSize = true;
            this.lblCnlNum.Location = new System.Drawing.Point(252, 48);
            this.lblCnlNum.Name = "lblCnlNum";
            this.lblCnlNum.Size = new System.Drawing.Size(46, 13);
            this.lblCnlNum.TabIndex = 11;
            this.lblCnlNum.Text = "Channel";
            // 
            // numCnlNum
            // 
            this.numCnlNum.Location = new System.Drawing.Point(255, 64);
            this.numCnlNum.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numCnlNum.Name = "numCnlNum";
            this.numCnlNum.Size = new System.Drawing.Size(75, 20);
            this.numCnlNum.TabIndex = 12;
            // 
            // numParamID
            // 
            this.numParamID.Location = new System.Drawing.Point(174, 64);
            this.numParamID.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numParamID.Name = "numParamID";
            this.numParamID.Size = new System.Drawing.Size(75, 20);
            this.numParamID.TabIndex = 10;
            // 
            // lblParamID
            // 
            this.lblParamID.AutoSize = true;
            this.lblParamID.Location = new System.Drawing.Point(171, 48);
            this.lblParamID.Name = "lblParamID";
            this.lblParamID.Size = new System.Drawing.Size(55, 13);
            this.lblParamID.TabIndex = 9;
            this.lblParamID.Text = "Parameter";
            // 
            // numKPNum
            // 
            this.numKPNum.Location = new System.Drawing.Point(93, 64);
            this.numKPNum.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numKPNum.Name = "numKPNum";
            this.numKPNum.Size = new System.Drawing.Size(75, 20);
            this.numKPNum.TabIndex = 8;
            // 
            // lblKPNum
            // 
            this.lblKPNum.AutoSize = true;
            this.lblKPNum.Location = new System.Drawing.Point(90, 48);
            this.lblKPNum.Name = "lblKPNum";
            this.lblKPNum.Size = new System.Drawing.Size(41, 13);
            this.lblKPNum.TabIndex = 7;
            this.lblKPNum.Text = "Device";
            // 
            // numObjNum
            // 
            this.numObjNum.Location = new System.Drawing.Point(12, 64);
            this.numObjNum.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numObjNum.Name = "numObjNum";
            this.numObjNum.Size = new System.Drawing.Size(75, 20);
            this.numObjNum.TabIndex = 6;
            // 
            // lblObjNum
            // 
            this.lblObjNum.AutoSize = true;
            this.lblObjNum.Location = new System.Drawing.Point(9, 48);
            this.lblObjNum.Name = "lblObjNum";
            this.lblObjNum.Size = new System.Drawing.Size(38, 13);
            this.lblObjNum.TabIndex = 5;
            this.lblObjNum.Text = "Object";
            // 
            // btnSetCurTime
            // 
            this.btnSetCurTime.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSetCurTime.Location = new System.Drawing.Point(255, 25);
            this.btnSetCurTime.Name = "btnSetCurTime";
            this.btnSetCurTime.Size = new System.Drawing.Size(75, 20);
            this.btnSetCurTime.TabIndex = 4;
            this.btnSetCurTime.Text = "Set Current";
            this.btnSetCurTime.UseVisualStyleBackColor = true;
            this.btnSetCurTime.Click += new System.EventHandler(this.btnSetCurTime_Click);
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Location = new System.Drawing.Point(130, 9);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(30, 13);
            this.lblTime.TabIndex = 2;
            this.lblTime.Text = "Time";
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Location = new System.Drawing.Point(9, 9);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(30, 13);
            this.lblDate.TabIndex = 0;
            this.lblDate.Text = "Date";
            // 
            // dtpTime
            // 
            this.dtpTime.CustomFormat = "";
            this.dtpTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTime.Location = new System.Drawing.Point(133, 25);
            this.dtpTime.Name = "dtpTime";
            this.dtpTime.ShowUpDown = true;
            this.dtpTime.Size = new System.Drawing.Size(116, 20);
            this.dtpTime.TabIndex = 3;
            // 
            // dtpDate
            // 
            this.dtpDate.CustomFormat = "";
            this.dtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDate.Location = new System.Drawing.Point(12, 25);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Size = new System.Drawing.Size(115, 20);
            this.dtpDate.TabIndex = 1;
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(336, 217);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 28;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(255, 217);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 23);
            this.btnSend.TabIndex = 27;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // FrmGenEvent
            // 
            this.AcceptButton = this.btnSend;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(423, 252);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.txtData);
            this.Controls.Add(this.lblData);
            this.Controls.Add(this.txtDescr);
            this.Controls.Add(this.lblDescr);
            this.Controls.Add(this.lblNewCnlVal);
            this.Controls.Add(this.txtNewCnlVal);
            this.Controls.Add(this.numUserID);
            this.Controls.Add(this.lblUserID);
            this.Controls.Add(this.numNewCnlStat);
            this.Controls.Add(this.lblNewCnlStat);
            this.Controls.Add(this.numOldCnlStat);
            this.Controls.Add(this.lblOldCnlStat);
            this.Controls.Add(this.lblOldCnlVal);
            this.Controls.Add(this.txtOldCnlVal);
            this.Controls.Add(this.lblCnlNum);
            this.Controls.Add(this.numCnlNum);
            this.Controls.Add(this.numParamID);
            this.Controls.Add(this.lblParamID);
            this.Controls.Add(this.numKPNum);
            this.Controls.Add(this.lblKPNum);
            this.Controls.Add(this.numObjNum);
            this.Controls.Add(this.lblObjNum);
            this.Controls.Add(this.btnSetCurTime);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.lblDate);
            this.Controls.Add(this.dtpTime);
            this.Controls.Add(this.dtpDate);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmGenEvent";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Generate Event";
            this.Load += new System.EventHandler(this.FrmGenEvent_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numUserID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numNewCnlStat)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numOldCnlStat)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCnlNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numParamID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numKPNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numObjNum)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtData;
        private System.Windows.Forms.Label lblData;
        private System.Windows.Forms.TextBox txtDescr;
        private System.Windows.Forms.Label lblDescr;
        private System.Windows.Forms.Label lblNewCnlVal;
        private System.Windows.Forms.TextBox txtNewCnlVal;
        private System.Windows.Forms.NumericUpDown numUserID;
        private System.Windows.Forms.Label lblUserID;
        private System.Windows.Forms.NumericUpDown numNewCnlStat;
        private System.Windows.Forms.Label lblNewCnlStat;
        private System.Windows.Forms.NumericUpDown numOldCnlStat;
        private System.Windows.Forms.Label lblOldCnlStat;
        private System.Windows.Forms.Label lblOldCnlVal;
        private System.Windows.Forms.TextBox txtOldCnlVal;
        private System.Windows.Forms.Label lblCnlNum;
        private System.Windows.Forms.NumericUpDown numCnlNum;
        private System.Windows.Forms.NumericUpDown numParamID;
        private System.Windows.Forms.Label lblParamID;
        private System.Windows.Forms.NumericUpDown numKPNum;
        private System.Windows.Forms.Label lblKPNum;
        private System.Windows.Forms.NumericUpDown numObjNum;
        private System.Windows.Forms.Label lblObjNum;
        private System.Windows.Forms.Button btnSetCurTime;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.DateTimePicker dtpTime;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSend;
    }
}