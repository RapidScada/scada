namespace Scada.Comm.Devices.KpSnmp
{
    partial class FrmSettings
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
            this.lblReadCommunity = new System.Windows.Forms.Label();
            this.txtReadCommunity = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtWriteCommunity = new System.Windows.Forms.TextBox();
            this.lblWriteCommunity = new System.Windows.Forms.Label();
            this.lblSnmpVersion = new System.Windows.Forms.Label();
            this.cbSnmpVersion = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // lblReadCommunity
            // 
            this.lblReadCommunity.AutoSize = true;
            this.lblReadCommunity.Location = new System.Drawing.Point(9, 9);
            this.lblReadCommunity.Name = "lblReadCommunity";
            this.lblReadCommunity.Size = new System.Drawing.Size(97, 13);
            this.lblReadCommunity.TabIndex = 0;
            this.lblReadCommunity.Text = "Пароль на чтение";
            // 
            // txtReadCommunity
            // 
            this.txtReadCommunity.Location = new System.Drawing.Point(12, 25);
            this.txtReadCommunity.Name = "txtReadCommunity";
            this.txtReadCommunity.Size = new System.Drawing.Size(200, 20);
            this.txtReadCommunity.TabIndex = 1;
            this.txtReadCommunity.UseSystemPasswordChar = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(56, 130);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(137, 130);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // txtWriteCommunity
            // 
            this.txtWriteCommunity.Location = new System.Drawing.Point(12, 64);
            this.txtWriteCommunity.Name = "txtWriteCommunity";
            this.txtWriteCommunity.Size = new System.Drawing.Size(200, 20);
            this.txtWriteCommunity.TabIndex = 3;
            this.txtWriteCommunity.UseSystemPasswordChar = true;
            // 
            // lblWriteCommunity
            // 
            this.lblWriteCommunity.AutoSize = true;
            this.lblWriteCommunity.Location = new System.Drawing.Point(9, 48);
            this.lblWriteCommunity.Name = "lblWriteCommunity";
            this.lblWriteCommunity.Size = new System.Drawing.Size(99, 13);
            this.lblWriteCommunity.TabIndex = 2;
            this.lblWriteCommunity.Text = "Пароль на запись";
            // 
            // lblSnmpVersion
            // 
            this.lblSnmpVersion.AutoSize = true;
            this.lblSnmpVersion.Location = new System.Drawing.Point(9, 87);
            this.lblSnmpVersion.Name = "lblSnmpVersion";
            this.lblSnmpVersion.Size = new System.Drawing.Size(78, 13);
            this.lblSnmpVersion.TabIndex = 4;
            this.lblSnmpVersion.Text = "Версия SNMP";
            // 
            // cbSnmpVersion
            // 
            this.cbSnmpVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSnmpVersion.FormattingEnabled = true;
            this.cbSnmpVersion.Items.AddRange(new object[] {
            "v1",
            "v2c"});
            this.cbSnmpVersion.Location = new System.Drawing.Point(12, 103);
            this.cbSnmpVersion.Name = "cbSnmpVersion";
            this.cbSnmpVersion.Size = new System.Drawing.Size(200, 21);
            this.cbSnmpVersion.TabIndex = 5;
            // 
            // FrmSettings
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(224, 165);
            this.Controls.Add(this.cbSnmpVersion);
            this.Controls.Add(this.lblSnmpVersion);
            this.Controls.Add(this.txtWriteCommunity);
            this.Controls.Add(this.lblWriteCommunity);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtReadCommunity);
            this.Controls.Add(this.lblReadCommunity);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmSettings";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Настройки";
            this.Load += new System.EventHandler(this.FrmPhoneGroup_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblReadCommunity;
        private System.Windows.Forms.TextBox txtReadCommunity;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtWriteCommunity;
        private System.Windows.Forms.Label lblWriteCommunity;
        private System.Windows.Forms.Label lblSnmpVersion;
        private System.Windows.Forms.ComboBox cbSnmpVersion;
    }
}