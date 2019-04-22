namespace Scada.Admin.App.Forms.Tools
{
    partial class FrmDeviceAdd
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
            this.gbDevice = new System.Windows.Forms.GroupBox();
            this.cbCommLine = new System.Windows.Forms.ComboBox();
            this.lblCommLine = new System.Windows.Forms.Label();
            this.txtCallNum = new System.Windows.Forms.TextBox();
            this.lblCallNum = new System.Windows.Forms.Label();
            this.lblAddress = new System.Windows.Forms.Label();
            this.cbKPType = new System.Windows.Forms.ComboBox();
            this.lblKPType = new System.Windows.Forms.Label();
            this.txtDescr = new System.Windows.Forms.TextBox();
            this.lblDescr = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.numKPNum = new System.Windows.Forms.NumericUpDown();
            this.lblKPNum = new System.Windows.Forms.Label();
            this.gbComm = new System.Windows.Forms.GroupBox();
            this.cbInstance = new System.Windows.Forms.ComboBox();
            this.lblInstance = new System.Windows.Forms.Label();
            this.chkAddToComm = new System.Windows.Forms.CheckBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.gbDevice.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numKPNum)).BeginInit();
            this.gbComm.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbDevice
            // 
            this.gbDevice.Controls.Add(this.cbCommLine);
            this.gbDevice.Controls.Add(this.lblCommLine);
            this.gbDevice.Controls.Add(this.txtCallNum);
            this.gbDevice.Controls.Add(this.lblCallNum);
            this.gbDevice.Controls.Add(this.txtAddress);
            this.gbDevice.Controls.Add(this.lblAddress);
            this.gbDevice.Controls.Add(this.cbKPType);
            this.gbDevice.Controls.Add(this.lblKPType);
            this.gbDevice.Controls.Add(this.txtDescr);
            this.gbDevice.Controls.Add(this.lblDescr);
            this.gbDevice.Controls.Add(this.txtName);
            this.gbDevice.Controls.Add(this.lblName);
            this.gbDevice.Controls.Add(this.numKPNum);
            this.gbDevice.Controls.Add(this.lblKPNum);
            this.gbDevice.Location = new System.Drawing.Point(12, 12);
            this.gbDevice.Name = "gbDevice";
            this.gbDevice.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbDevice.Size = new System.Drawing.Size(360, 223);
            this.gbDevice.TabIndex = 0;
            this.gbDevice.TabStop = false;
            this.gbDevice.Text = "Device";
            // 
            // cbCommLine
            // 
            this.cbCommLine.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCommLine.FormattingEnabled = true;
            this.cbCommLine.Location = new System.Drawing.Point(13, 150);
            this.cbCommLine.Name = "cbCommLine";
            this.cbCommLine.Size = new System.Drawing.Size(334, 21);
            this.cbCommLine.TabIndex = 11;
            // 
            // lblCommLine
            // 
            this.lblCommLine.AutoSize = true;
            this.lblCommLine.Location = new System.Drawing.Point(10, 134);
            this.lblCommLine.Name = "lblCommLine";
            this.lblCommLine.Size = new System.Drawing.Size(98, 13);
            this.lblCommLine.TabIndex = 10;
            this.lblCommLine.Text = "Communication line";
            // 
            // txtCallNum
            // 
            this.txtCallNum.Location = new System.Drawing.Point(94, 111);
            this.txtCallNum.Name = "txtCallNum";
            this.txtCallNum.Size = new System.Drawing.Size(253, 20);
            this.txtCallNum.TabIndex = 9;
            // 
            // lblCallNum
            // 
            this.lblCallNum.AutoSize = true;
            this.lblCallNum.Location = new System.Drawing.Point(91, 95);
            this.lblCallNum.Name = "lblCallNum";
            this.lblCallNum.Size = new System.Drawing.Size(62, 13);
            this.lblCallNum.TabIndex = 8;
            this.lblCallNum.Text = "Call number";
            // 
            // lblAddress
            // 
            this.lblAddress.AutoSize = true;
            this.lblAddress.Location = new System.Drawing.Point(10, 95);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Size = new System.Drawing.Size(45, 13);
            this.lblAddress.TabIndex = 6;
            this.lblAddress.Text = "Address";
            // 
            // cbKPType
            // 
            this.cbKPType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbKPType.FormattingEnabled = true;
            this.cbKPType.Location = new System.Drawing.Point(13, 71);
            this.cbKPType.Name = "cbKPType";
            this.cbKPType.Size = new System.Drawing.Size(334, 21);
            this.cbKPType.TabIndex = 5;
            // 
            // lblKPType
            // 
            this.lblKPType.AutoSize = true;
            this.lblKPType.Location = new System.Drawing.Point(10, 55);
            this.lblKPType.Name = "lblKPType";
            this.lblKPType.Size = new System.Drawing.Size(64, 13);
            this.lblKPType.TabIndex = 4;
            this.lblKPType.Text = "Device type";
            // 
            // txtDescr
            // 
            this.txtDescr.Location = new System.Drawing.Point(13, 190);
            this.txtDescr.Name = "txtDescr";
            this.txtDescr.Size = new System.Drawing.Size(334, 20);
            this.txtDescr.TabIndex = 13;
            // 
            // lblDescr
            // 
            this.lblDescr.AutoSize = true;
            this.lblDescr.Location = new System.Drawing.Point(10, 174);
            this.lblDescr.Name = "lblDescr";
            this.lblDescr.Size = new System.Drawing.Size(60, 13);
            this.lblDescr.TabIndex = 12;
            this.lblDescr.Text = "Description";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(94, 32);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(253, 20);
            this.txtName.TabIndex = 3;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(91, 16);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(35, 13);
            this.lblName.TabIndex = 2;
            this.lblName.Text = "Name";
            // 
            // numKPNum
            // 
            this.numKPNum.Location = new System.Drawing.Point(13, 32);
            this.numKPNum.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numKPNum.Name = "numKPNum";
            this.numKPNum.Size = new System.Drawing.Size(75, 20);
            this.numKPNum.TabIndex = 1;
            this.numKPNum.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblKPNum
            // 
            this.lblKPNum.AutoSize = true;
            this.lblKPNum.Location = new System.Drawing.Point(10, 16);
            this.lblKPNum.Name = "lblKPNum";
            this.lblKPNum.Size = new System.Drawing.Size(44, 13);
            this.lblKPNum.TabIndex = 0;
            this.lblKPNum.Text = "Number";
            // 
            // gbComm
            // 
            this.gbComm.Controls.Add(this.cbInstance);
            this.gbComm.Controls.Add(this.lblInstance);
            this.gbComm.Controls.Add(this.chkAddToComm);
            this.gbComm.Location = new System.Drawing.Point(12, 241);
            this.gbComm.Name = "gbComm";
            this.gbComm.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbComm.Size = new System.Drawing.Size(360, 89);
            this.gbComm.TabIndex = 1;
            this.gbComm.TabStop = false;
            this.gbComm.Text = "Communicator";
            // 
            // cbInstance
            // 
            this.cbInstance.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbInstance.FormattingEnabled = true;
            this.cbInstance.Location = new System.Drawing.Point(13, 55);
            this.cbInstance.Name = "cbInstance";
            this.cbInstance.Size = new System.Drawing.Size(334, 21);
            this.cbInstance.TabIndex = 2;
            // 
            // lblInstance
            // 
            this.lblInstance.AutoSize = true;
            this.lblInstance.Location = new System.Drawing.Point(10, 39);
            this.lblInstance.Name = "lblInstance";
            this.lblInstance.Size = new System.Drawing.Size(48, 13);
            this.lblInstance.TabIndex = 1;
            this.lblInstance.Text = "Instance";
            // 
            // chkAddToComm
            // 
            this.chkAddToComm.AutoSize = true;
            this.chkAddToComm.Checked = true;
            this.chkAddToComm.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAddToComm.Location = new System.Drawing.Point(13, 19);
            this.chkAddToComm.Name = "chkAddToComm";
            this.chkAddToComm.Size = new System.Drawing.Size(162, 17);
            this.chkAddToComm.TabIndex = 0;
            this.chkAddToComm.Text = "Add device to Communicator";
            this.chkAddToComm.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(297, 336);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(216, 336);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // txtAddress
            // 
            this.txtAddress.Location = new System.Drawing.Point(13, 111);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(75, 20);
            this.txtAddress.TabIndex = 7;
            // 
            // FrmDeviceAdd
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(384, 371);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.gbComm);
            this.Controls.Add(this.gbDevice);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmDeviceAdd";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add Device";
            this.Load += new System.EventHandler(this.FrmDeviceAdd_Load);
            this.gbDevice.ResumeLayout(false);
            this.gbDevice.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numKPNum)).EndInit();
            this.gbComm.ResumeLayout(false);
            this.gbComm.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbDevice;
        private System.Windows.Forms.TextBox txtDescr;
        private System.Windows.Forms.Label lblDescr;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.NumericUpDown numKPNum;
        private System.Windows.Forms.Label lblKPNum;
        private System.Windows.Forms.ComboBox cbKPType;
        private System.Windows.Forms.Label lblKPType;
        private System.Windows.Forms.Label lblAddress;
        private System.Windows.Forms.ComboBox cbCommLine;
        private System.Windows.Forms.Label lblCommLine;
        private System.Windows.Forms.TextBox txtCallNum;
        private System.Windows.Forms.Label lblCallNum;
        private System.Windows.Forms.GroupBox gbComm;
        private System.Windows.Forms.ComboBox cbInstance;
        private System.Windows.Forms.Label lblInstance;
        private System.Windows.Forms.CheckBox chkAddToComm;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.TextBox txtAddress;
    }
}