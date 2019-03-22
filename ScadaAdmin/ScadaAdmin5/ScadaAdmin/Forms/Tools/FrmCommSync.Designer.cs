namespace Scada.Admin.App.Forms.Tools
{
    partial class FrmCommSync
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
            this.lblCommLine = new System.Windows.Forms.Label();
            this.cbCommLine = new System.Windows.Forms.ComboBox();
            this.gbParams = new System.Windows.Forms.GroupBox();
            this.chkCommLineName = new System.Windows.Forms.CheckBox();
            this.chkDeviceName = new System.Windows.Forms.CheckBox();
            this.chkDeviceDriver = new System.Windows.Forms.CheckBox();
            this.chkDeviceAddress = new System.Windows.Forms.CheckBox();
            this.chkDeviceCallNum = new System.Windows.Forms.CheckBox();
            this.btnSync = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.gbParams.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblCommLine
            // 
            this.lblCommLine.AutoSize = true;
            this.lblCommLine.Location = new System.Drawing.Point(9, 9);
            this.lblCommLine.Name = "lblCommLine";
            this.lblCommLine.Size = new System.Drawing.Size(98, 13);
            this.lblCommLine.TabIndex = 0;
            this.lblCommLine.Text = "Communication line";
            // 
            // cbCommLine
            // 
            this.cbCommLine.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCommLine.FormattingEnabled = true;
            this.cbCommLine.Location = new System.Drawing.Point(12, 25);
            this.cbCommLine.Name = "cbCommLine";
            this.cbCommLine.Size = new System.Drawing.Size(360, 21);
            this.cbCommLine.TabIndex = 1;
            // 
            // gbParams
            // 
            this.gbParams.Controls.Add(this.chkDeviceCallNum);
            this.gbParams.Controls.Add(this.chkDeviceAddress);
            this.gbParams.Controls.Add(this.chkDeviceDriver);
            this.gbParams.Controls.Add(this.chkDeviceName);
            this.gbParams.Controls.Add(this.chkCommLineName);
            this.gbParams.Location = new System.Drawing.Point(12, 52);
            this.gbParams.Name = "gbParams";
            this.gbParams.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbParams.Size = new System.Drawing.Size(360, 141);
            this.gbParams.TabIndex = 2;
            this.gbParams.TabStop = false;
            this.gbParams.Text = "Parameters to Update";
            // 
            // chkCommLineName
            // 
            this.chkCommLineName.AutoSize = true;
            this.chkCommLineName.Checked = true;
            this.chkCommLineName.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCommLineName.Enabled = false;
            this.chkCommLineName.Location = new System.Drawing.Point(13, 19);
            this.chkCommLineName.Name = "chkCommLineName";
            this.chkCommLineName.Size = new System.Drawing.Size(146, 17);
            this.chkCommLineName.TabIndex = 0;
            this.chkCommLineName.Text = "Communication line name";
            this.chkCommLineName.UseVisualStyleBackColor = true;
            // 
            // chkDeviceName
            // 
            this.chkDeviceName.AutoSize = true;
            this.chkDeviceName.Checked = true;
            this.chkDeviceName.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDeviceName.Enabled = false;
            this.chkDeviceName.Location = new System.Drawing.Point(13, 42);
            this.chkDeviceName.Name = "chkDeviceName";
            this.chkDeviceName.Size = new System.Drawing.Size(89, 17);
            this.chkDeviceName.TabIndex = 1;
            this.chkDeviceName.Text = "Device name";
            this.chkDeviceName.UseVisualStyleBackColor = true;
            // 
            // chkDeviceDriver
            // 
            this.chkDeviceDriver.AutoSize = true;
            this.chkDeviceDriver.Checked = true;
            this.chkDeviceDriver.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDeviceDriver.Enabled = false;
            this.chkDeviceDriver.Location = new System.Drawing.Point(13, 65);
            this.chkDeviceDriver.Name = "chkDeviceDriver";
            this.chkDeviceDriver.Size = new System.Drawing.Size(89, 17);
            this.chkDeviceDriver.TabIndex = 2;
            this.chkDeviceDriver.Text = "Device driver";
            this.chkDeviceDriver.UseVisualStyleBackColor = true;
            // 
            // chkDeviceAddress
            // 
            this.chkDeviceAddress.AutoSize = true;
            this.chkDeviceAddress.Checked = true;
            this.chkDeviceAddress.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDeviceAddress.Enabled = false;
            this.chkDeviceAddress.Location = new System.Drawing.Point(13, 88);
            this.chkDeviceAddress.Name = "chkDeviceAddress";
            this.chkDeviceAddress.Size = new System.Drawing.Size(100, 17);
            this.chkDeviceAddress.TabIndex = 3;
            this.chkDeviceAddress.Text = "Device address";
            this.chkDeviceAddress.UseVisualStyleBackColor = true;
            // 
            // chkDeviceCallNum
            // 
            this.chkDeviceCallNum.AutoSize = true;
            this.chkDeviceCallNum.Checked = true;
            this.chkDeviceCallNum.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDeviceCallNum.Enabled = false;
            this.chkDeviceCallNum.Location = new System.Drawing.Point(13, 111);
            this.chkDeviceCallNum.Name = "chkDeviceCallNum";
            this.chkDeviceCallNum.Size = new System.Drawing.Size(117, 17);
            this.chkDeviceCallNum.TabIndex = 4;
            this.chkDeviceCallNum.Text = "Device call number";
            this.chkDeviceCallNum.UseVisualStyleBackColor = true;
            // 
            // btnSync
            // 
            this.btnSync.Location = new System.Drawing.Point(216, 199);
            this.btnSync.Name = "btnSync";
            this.btnSync.Size = new System.Drawing.Size(75, 23);
            this.btnSync.TabIndex = 3;
            this.btnSync.Text = "Sync";
            this.btnSync.UseVisualStyleBackColor = true;
            this.btnSync.Click += new System.EventHandler(this.btnSync_Click);
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(297, 199);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // FrmCommSync
            // 
            this.AcceptButton = this.btnSync;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(384, 234);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSync);
            this.Controls.Add(this.gbParams);
            this.Controls.Add(this.cbCommLine);
            this.Controls.Add(this.lblCommLine);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmCommSync";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Sync Communicator settings";
            this.Load += new System.EventHandler(this.FrmCommSync_Load);
            this.gbParams.ResumeLayout(false);
            this.gbParams.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblCommLine;
        private System.Windows.Forms.ComboBox cbCommLine;
        private System.Windows.Forms.GroupBox gbParams;
        private System.Windows.Forms.CheckBox chkCommLineName;
        private System.Windows.Forms.CheckBox chkDeviceCallNum;
        private System.Windows.Forms.CheckBox chkDeviceAddress;
        private System.Windows.Forms.CheckBox chkDeviceDriver;
        private System.Windows.Forms.CheckBox chkDeviceName;
        private System.Windows.Forms.Button btnSync;
        private System.Windows.Forms.Button btnClose;
    }
}