namespace Scada.Comm.Devices.KpEmail
{
    partial class FrmConfig
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
            this.btnEditAddressBook = new System.Windows.Forms.Button();
            this.chkEnableSsl = new System.Windows.Forms.CheckBox();
            this.txtSenderDisplayName = new System.Windows.Forms.TextBox();
            this.lblSenderDisplayName = new System.Windows.Forms.Label();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.lblUser = new System.Windows.Forms.Label();
            this.numPort = new System.Windows.Forms.NumericUpDown();
            this.lblPort = new System.Windows.Forms.Label();
            this.txtHost = new System.Windows.Forms.TextBox();
            this.lblHost = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.gbServer = new System.Windows.Forms.GroupBox();
            this.gbSender = new System.Windows.Forms.GroupBox();
            this.lblSenderAddress = new System.Windows.Forms.Label();
            this.txtSenderAddress = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.numPort)).BeginInit();
            this.gbServer.SuspendLayout();
            this.gbSender.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnEditAddressBook
            // 
            this.btnEditAddressBook.Location = new System.Drawing.Point(12, 255);
            this.btnEditAddressBook.Name = "btnEditAddressBook";
            this.btnEditAddressBook.Size = new System.Drawing.Size(130, 23);
            this.btnEditAddressBook.TabIndex = 2;
            this.btnEditAddressBook.Text = "Address Book";
            this.btnEditAddressBook.UseVisualStyleBackColor = true;
            this.btnEditAddressBook.Click += new System.EventHandler(this.btnEditAddressBook_Click);
            // 
            // chkEnableSsl
            // 
            this.chkEnableSsl.AutoSize = true;
            this.chkEnableSsl.Location = new System.Drawing.Point(13, 97);
            this.chkEnableSsl.Name = "chkEnableSsl";
            this.chkEnableSsl.Size = new System.Drawing.Size(68, 17);
            this.chkEnableSsl.TabIndex = 8;
            this.chkEnableSsl.Text = "Use SSL";
            this.chkEnableSsl.UseVisualStyleBackColor = true;
            // 
            // txtSenderDisplayName
            // 
            this.txtSenderDisplayName.Location = new System.Drawing.Point(13, 71);
            this.txtSenderDisplayName.Name = "txtSenderDisplayName";
            this.txtSenderDisplayName.Size = new System.Drawing.Size(334, 20);
            this.txtSenderDisplayName.TabIndex = 3;
            // 
            // lblSenderDisplayName
            // 
            this.lblSenderDisplayName.AutoSize = true;
            this.lblSenderDisplayName.Location = new System.Drawing.Point(10, 55);
            this.lblSenderDisplayName.Name = "lblSenderDisplayName";
            this.lblSenderDisplayName.Size = new System.Drawing.Size(70, 13);
            this.lblSenderDisplayName.TabIndex = 2;
            this.lblSenderDisplayName.Text = "Display name";
            // 
            // txtUser
            // 
            this.txtUser.Location = new System.Drawing.Point(13, 71);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(200, 20);
            this.txtUser.TabIndex = 5;
            this.txtUser.TextChanged += new System.EventHandler(this.txtUser_TextChanged);
            // 
            // lblUser
            // 
            this.lblUser.AutoSize = true;
            this.lblUser.Location = new System.Drawing.Point(10, 55);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(29, 13);
            this.lblUser.TabIndex = 4;
            this.lblUser.Text = "User";
            // 
            // numPort
            // 
            this.numPort.Location = new System.Drawing.Point(219, 32);
            this.numPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numPort.Name = "numPort";
            this.numPort.Size = new System.Drawing.Size(128, 20);
            this.numPort.TabIndex = 3;
            this.numPort.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.Location = new System.Drawing.Point(216, 16);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(26, 13);
            this.lblPort.TabIndex = 2;
            this.lblPort.Text = "Port";
            // 
            // txtHost
            // 
            this.txtHost.Location = new System.Drawing.Point(13, 32);
            this.txtHost.Name = "txtHost";
            this.txtHost.Size = new System.Drawing.Size(200, 20);
            this.txtHost.TabIndex = 1;
            // 
            // lblHost
            // 
            this.lblHost.AutoSize = true;
            this.lblHost.Location = new System.Drawing.Point(10, 16);
            this.lblHost.Name = "lblHost";
            this.lblHost.Size = new System.Drawing.Size(61, 13);
            this.lblHost.TabIndex = 0;
            this.lblHost.Text = "Server host";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(219, 71);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(128, 20);
            this.txtPassword.TabIndex = 7;
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(216, 55);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(53, 13);
            this.lblPassword.TabIndex = 6;
            this.lblPassword.Text = "Password";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(216, 255);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(297, 255);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // gbServer
            // 
            this.gbServer.Controls.Add(this.lblHost);
            this.gbServer.Controls.Add(this.txtHost);
            this.gbServer.Controls.Add(this.lblPort);
            this.gbServer.Controls.Add(this.txtPassword);
            this.gbServer.Controls.Add(this.chkEnableSsl);
            this.gbServer.Controls.Add(this.numPort);
            this.gbServer.Controls.Add(this.lblPassword);
            this.gbServer.Controls.Add(this.lblUser);
            this.gbServer.Controls.Add(this.txtUser);
            this.gbServer.Location = new System.Drawing.Point(12, 12);
            this.gbServer.Name = "gbServer";
            this.gbServer.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbServer.Size = new System.Drawing.Size(360, 127);
            this.gbServer.TabIndex = 0;
            this.gbServer.TabStop = false;
            this.gbServer.Text = "SMTP Server";
            // 
            // gbSender
            // 
            this.gbSender.Controls.Add(this.lblSenderAddress);
            this.gbSender.Controls.Add(this.txtSenderAddress);
            this.gbSender.Controls.Add(this.lblSenderDisplayName);
            this.gbSender.Controls.Add(this.txtSenderDisplayName);
            this.gbSender.Location = new System.Drawing.Point(12, 145);
            this.gbSender.Name = "gbSender";
            this.gbSender.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbSender.Size = new System.Drawing.Size(360, 104);
            this.gbSender.TabIndex = 1;
            this.gbSender.TabStop = false;
            this.gbSender.Text = "From";
            // 
            // lblSenderAddress
            // 
            this.lblSenderAddress.AutoSize = true;
            this.lblSenderAddress.Location = new System.Drawing.Point(10, 16);
            this.lblSenderAddress.Name = "lblSenderAddress";
            this.lblSenderAddress.Size = new System.Drawing.Size(81, 13);
            this.lblSenderAddress.TabIndex = 0;
            this.lblSenderAddress.Text = "Sender address";
            // 
            // txtSenderAddress
            // 
            this.txtSenderAddress.Location = new System.Drawing.Point(13, 32);
            this.txtSenderAddress.Name = "txtSenderAddress";
            this.txtSenderAddress.Size = new System.Drawing.Size(334, 20);
            this.txtSenderAddress.TabIndex = 1;
            // 
            // FrmConfig
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(384, 290);
            this.Controls.Add(this.gbSender);
            this.Controls.Add(this.gbServer);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnEditAddressBook);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmConfig";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Email - Device {0} Properties";
            this.Load += new System.EventHandler(this.FrmConfig_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numPort)).EndInit();
            this.gbServer.ResumeLayout(false);
            this.gbServer.PerformLayout();
            this.gbSender.ResumeLayout(false);
            this.gbSender.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnEditAddressBook;
        private System.Windows.Forms.CheckBox chkEnableSsl;
        private System.Windows.Forms.TextBox txtSenderDisplayName;
        private System.Windows.Forms.Label lblSenderDisplayName;
        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.NumericUpDown numPort;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.TextBox txtHost;
        private System.Windows.Forms.Label lblHost;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox gbServer;
        private System.Windows.Forms.GroupBox gbSender;
        private System.Windows.Forms.Label lblSenderAddress;
        private System.Windows.Forms.TextBox txtSenderAddress;
    }
}