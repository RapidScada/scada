namespace Scada.Server.Shell.Forms
{
    partial class FrmGenData
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
            this.numCnlStat = new System.Windows.Forms.NumericUpDown();
            this.btnStatOne = new System.Windows.Forms.Button();
            this.btnStatZero = new System.Windows.Forms.Button();
            this.lblCnlStat = new System.Windows.Forms.Label();
            this.btnValOn = new System.Windows.Forms.Button();
            this.btnValOff = new System.Windows.Forms.Button();
            this.lblCnlVal = new System.Windows.Forms.Label();
            this.txtCnlVal = new System.Windows.Forms.TextBox();
            this.cbCnlNum = new System.Windows.Forms.ComboBox();
            this.lblCnlNum = new System.Windows.Forms.Label();
            this.dtpArcTime = new System.Windows.Forms.DateTimePicker();
            this.dtpArcDate = new System.Windows.Forms.DateTimePicker();
            this.rbArcData = new System.Windows.Forms.RadioButton();
            this.rbCurData = new System.Windows.Forms.RadioButton();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSend = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numCnlStat)).BeginInit();
            this.SuspendLayout();
            // 
            // numCnlStat
            // 
            this.numCnlStat.Location = new System.Drawing.Point(230, 112);
            this.numCnlStat.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numCnlStat.Name = "numCnlStat";
            this.numCnlStat.Size = new System.Drawing.Size(75, 20);
            this.numCnlStat.TabIndex = 11;
            // 
            // btnStatOne
            // 
            this.btnStatOne.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnStatOne.Location = new System.Drawing.Point(362, 112);
            this.btnStatOne.Name = "btnStatOne";
            this.btnStatOne.Size = new System.Drawing.Size(45, 20);
            this.btnStatOne.TabIndex = 13;
            this.btnStatOne.Text = "1";
            this.btnStatOne.UseVisualStyleBackColor = true;
            this.btnStatOne.Click += new System.EventHandler(this.btnStatOne_Click);
            // 
            // btnStatZero
            // 
            this.btnStatZero.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnStatZero.Location = new System.Drawing.Point(311, 112);
            this.btnStatZero.Name = "btnStatZero";
            this.btnStatZero.Size = new System.Drawing.Size(45, 20);
            this.btnStatZero.TabIndex = 12;
            this.btnStatZero.Text = "0";
            this.btnStatZero.UseVisualStyleBackColor = true;
            this.btnStatZero.Click += new System.EventHandler(this.btnStatZero_Click);
            // 
            // lblCnlStat
            // 
            this.lblCnlStat.AutoSize = true;
            this.lblCnlStat.Location = new System.Drawing.Point(227, 96);
            this.lblCnlStat.Name = "lblCnlStat";
            this.lblCnlStat.Size = new System.Drawing.Size(37, 13);
            this.lblCnlStat.TabIndex = 10;
            this.lblCnlStat.Text = "Status";
            // 
            // btnValOn
            // 
            this.btnValOn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnValOn.Location = new System.Drawing.Point(179, 112);
            this.btnValOn.Name = "btnValOn";
            this.btnValOn.Size = new System.Drawing.Size(45, 20);
            this.btnValOn.TabIndex = 9;
            this.btnValOn.Text = "On";
            this.btnValOn.UseVisualStyleBackColor = true;
            this.btnValOn.Click += new System.EventHandler(this.btnValOn_Click);
            // 
            // btnValOff
            // 
            this.btnValOff.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnValOff.Location = new System.Drawing.Point(128, 112);
            this.btnValOff.Name = "btnValOff";
            this.btnValOff.Size = new System.Drawing.Size(45, 20);
            this.btnValOff.TabIndex = 8;
            this.btnValOff.Text = "Off";
            this.btnValOff.UseVisualStyleBackColor = true;
            this.btnValOff.Click += new System.EventHandler(this.btnValOff_Click);
            // 
            // lblCnlVal
            // 
            this.lblCnlVal.AutoSize = true;
            this.lblCnlVal.Location = new System.Drawing.Point(9, 96);
            this.lblCnlVal.Name = "lblCnlVal";
            this.lblCnlVal.Size = new System.Drawing.Size(34, 13);
            this.lblCnlVal.TabIndex = 6;
            this.lblCnlVal.Text = "Value";
            // 
            // txtCnlVal
            // 
            this.txtCnlVal.Location = new System.Drawing.Point(12, 112);
            this.txtCnlVal.Name = "txtCnlVal";
            this.txtCnlVal.Size = new System.Drawing.Size(110, 20);
            this.txtCnlVal.TabIndex = 7;
            this.txtCnlVal.Text = "0";
            // 
            // cbCnlNum
            // 
            this.cbCnlNum.FormattingEnabled = true;
            this.cbCnlNum.Location = new System.Drawing.Point(12, 72);
            this.cbCnlNum.Name = "cbCnlNum";
            this.cbCnlNum.Size = new System.Drawing.Size(110, 21);
            this.cbCnlNum.TabIndex = 5;
            // 
            // lblCnlNum
            // 
            this.lblCnlNum.AutoSize = true;
            this.lblCnlNum.Location = new System.Drawing.Point(9, 55);
            this.lblCnlNum.Name = "lblCnlNum";
            this.lblCnlNum.Size = new System.Drawing.Size(46, 13);
            this.lblCnlNum.TabIndex = 4;
            this.lblCnlNum.Text = "Channel";
            // 
            // dtpArcTime
            // 
            this.dtpArcTime.Checked = false;
            this.dtpArcTime.CustomFormat = "";
            this.dtpArcTime.Enabled = false;
            this.dtpArcTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpArcTime.Location = new System.Drawing.Point(230, 33);
            this.dtpArcTime.Name = "dtpArcTime";
            this.dtpArcTime.ShowUpDown = true;
            this.dtpArcTime.Size = new System.Drawing.Size(75, 20);
            this.dtpArcTime.TabIndex = 3;
            // 
            // dtpArcDate
            // 
            this.dtpArcDate.CustomFormat = "";
            this.dtpArcDate.Enabled = false;
            this.dtpArcDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpArcDate.Location = new System.Drawing.Point(128, 33);
            this.dtpArcDate.Name = "dtpArcDate";
            this.dtpArcDate.Size = new System.Drawing.Size(96, 20);
            this.dtpArcDate.TabIndex = 2;
            // 
            // rbArcData
            // 
            this.rbArcData.AutoSize = true;
            this.rbArcData.Location = new System.Drawing.Point(12, 35);
            this.rbArcData.Name = "rbArcData";
            this.rbArcData.Size = new System.Drawing.Size(85, 17);
            this.rbArcData.TabIndex = 1;
            this.rbArcData.TabStop = true;
            this.rbArcData.Text = "Archive data";
            this.rbArcData.UseVisualStyleBackColor = true;
            this.rbArcData.CheckedChanged += new System.EventHandler(this.rbArcData_CheckedChanged);
            // 
            // rbCurData
            // 
            this.rbCurData.AutoSize = true;
            this.rbCurData.Checked = true;
            this.rbCurData.Location = new System.Drawing.Point(12, 12);
            this.rbCurData.Name = "rbCurData";
            this.rbCurData.Size = new System.Drawing.Size(83, 17);
            this.rbCurData.TabIndex = 0;
            this.rbCurData.TabStop = true;
            this.rbCurData.Text = "Current data";
            this.rbCurData.UseVisualStyleBackColor = true;
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(332, 148);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 15;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(251, 148);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 23);
            this.btnSend.TabIndex = 14;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // FrmGenData
            // 
            this.AcceptButton = this.btnSend;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(419, 183);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.numCnlStat);
            this.Controls.Add(this.btnStatOne);
            this.Controls.Add(this.btnStatZero);
            this.Controls.Add(this.lblCnlStat);
            this.Controls.Add(this.btnValOn);
            this.Controls.Add(this.btnValOff);
            this.Controls.Add(this.lblCnlVal);
            this.Controls.Add(this.txtCnlVal);
            this.Controls.Add(this.cbCnlNum);
            this.Controls.Add(this.lblCnlNum);
            this.Controls.Add(this.dtpArcTime);
            this.Controls.Add(this.dtpArcDate);
            this.Controls.Add(this.rbArcData);
            this.Controls.Add(this.rbCurData);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmGenData";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Generate Data";
            this.Load += new System.EventHandler(this.FrmGenData_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numCnlStat)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown numCnlStat;
        private System.Windows.Forms.Button btnStatOne;
        private System.Windows.Forms.Button btnStatZero;
        private System.Windows.Forms.Label lblCnlStat;
        private System.Windows.Forms.Button btnValOn;
        private System.Windows.Forms.Button btnValOff;
        private System.Windows.Forms.Label lblCnlVal;
        private System.Windows.Forms.TextBox txtCnlVal;
        private System.Windows.Forms.ComboBox cbCnlNum;
        private System.Windows.Forms.Label lblCnlNum;
        private System.Windows.Forms.DateTimePicker dtpArcTime;
        private System.Windows.Forms.DateTimePicker dtpArcDate;
        private System.Windows.Forms.RadioButton rbArcData;
        private System.Windows.Forms.RadioButton rbCurData;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSend;
    }
}