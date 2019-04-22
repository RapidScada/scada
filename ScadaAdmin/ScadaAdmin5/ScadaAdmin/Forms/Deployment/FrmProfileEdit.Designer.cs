namespace Scada.Admin.App.Forms.Deployment
{
    partial class FrmProfileEdit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmProfileEdit));
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnGenSecretKey = new System.Windows.Forms.Button();
            this.txtSecretKey = new System.Windows.Forms.TextBox();
            this.lblSecretKey = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.lblUsername = new System.Windows.Forms.Label();
            this.lblPort = new System.Windows.Forms.Label();
            this.numPort = new System.Windows.Forms.NumericUpDown();
            this.txtHost = new System.Windows.Forms.TextBox();
            this.lblHost = new System.Windows.Forms.Label();
            this.txtProfileName = new System.Windows.Forms.TextBox();
            this.lblProfileName = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.gbConnSettings = new System.Windows.Forms.GroupBox();
            this.gbProfileProps = new System.Windows.Forms.GroupBox();
            this.lblWebUrl = new System.Windows.Forms.Label();
            this.txtWebUrl = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.numPort)).BeginInit();
            this.gbConnSettings.SuspendLayout();
            this.gbProfileProps.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(247, 340);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(166, 340);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnGenSecretKey
            // 
            this.btnGenSecretKey.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnGenSecretKey.Image = ((System.Drawing.Image)(resources.GetObject("btnGenSecretKey.Image")));
            this.btnGenSecretKey.Location = new System.Drawing.Point(277, 136);
            this.btnGenSecretKey.Name = "btnGenSecretKey";
            this.btnGenSecretKey.Size = new System.Drawing.Size(20, 20);
            this.btnGenSecretKey.TabIndex = 9;
            this.toolTip.SetToolTip(this.btnGenSecretKey, "Generate secret key");
            this.btnGenSecretKey.UseVisualStyleBackColor = true;
            this.btnGenSecretKey.Click += new System.EventHandler(this.btnGenSecretKey_Click);
            // 
            // txtSecretKey
            // 
            this.txtSecretKey.Location = new System.Drawing.Point(13, 159);
            this.txtSecretKey.Multiline = true;
            this.txtSecretKey.Name = "txtSecretKey";
            this.txtSecretKey.Size = new System.Drawing.Size(284, 40);
            this.txtSecretKey.TabIndex = 10;
            // 
            // lblSecretKey
            // 
            this.lblSecretKey.AutoSize = true;
            this.lblSecretKey.Location = new System.Drawing.Point(10, 143);
            this.lblSecretKey.Name = "lblSecretKey";
            this.lblSecretKey.Size = new System.Drawing.Size(58, 13);
            this.lblSecretKey.TabIndex = 8;
            this.lblSecretKey.Text = "Secret key";
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(10, 94);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(53, 13);
            this.lblPassword.TabIndex = 6;
            this.lblPassword.Text = "Password";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(13, 110);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(284, 20);
            this.txtPassword.TabIndex = 7;
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(13, 71);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(284, 20);
            this.txtUsername.TabIndex = 5;
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(10, 55);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(55, 13);
            this.lblUsername.TabIndex = 4;
            this.lblUsername.Text = "Username";
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.Location = new System.Drawing.Point(185, 16);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(26, 13);
            this.lblPort.TabIndex = 2;
            this.lblPort.Text = "Port";
            // 
            // numPort
            // 
            this.numPort.Location = new System.Drawing.Point(188, 32);
            this.numPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numPort.Name = "numPort";
            this.numPort.Size = new System.Drawing.Size(75, 20);
            this.numPort.TabIndex = 3;
            this.numPort.Value = new decimal(new int[] {
            10002,
            0,
            0,
            0});
            // 
            // txtHost
            // 
            this.txtHost.Location = new System.Drawing.Point(13, 32);
            this.txtHost.Name = "txtHost";
            this.txtHost.Size = new System.Drawing.Size(169, 20);
            this.txtHost.TabIndex = 1;
            // 
            // lblHost
            // 
            this.lblHost.AutoSize = true;
            this.lblHost.Location = new System.Drawing.Point(10, 16);
            this.lblHost.Name = "lblHost";
            this.lblHost.Size = new System.Drawing.Size(38, 13);
            this.lblHost.TabIndex = 0;
            this.lblHost.Text = "Server";
            // 
            // txtProfileName
            // 
            this.txtProfileName.Location = new System.Drawing.Point(13, 32);
            this.txtProfileName.Name = "txtProfileName";
            this.txtProfileName.Size = new System.Drawing.Size(284, 20);
            this.txtProfileName.TabIndex = 1;
            // 
            // lblProfileName
            // 
            this.lblProfileName.AutoSize = true;
            this.lblProfileName.Location = new System.Drawing.Point(10, 16);
            this.lblProfileName.Name = "lblProfileName";
            this.lblProfileName.Size = new System.Drawing.Size(65, 13);
            this.lblProfileName.TabIndex = 0;
            this.lblProfileName.Text = "Profile name";
            // 
            // gbConnSettings
            // 
            this.gbConnSettings.Controls.Add(this.lblHost);
            this.gbConnSettings.Controls.Add(this.txtHost);
            this.gbConnSettings.Controls.Add(this.numPort);
            this.gbConnSettings.Controls.Add(this.btnGenSecretKey);
            this.gbConnSettings.Controls.Add(this.lblPort);
            this.gbConnSettings.Controls.Add(this.txtSecretKey);
            this.gbConnSettings.Controls.Add(this.lblUsername);
            this.gbConnSettings.Controls.Add(this.lblSecretKey);
            this.gbConnSettings.Controls.Add(this.txtUsername);
            this.gbConnSettings.Controls.Add(this.lblPassword);
            this.gbConnSettings.Controls.Add(this.txtPassword);
            this.gbConnSettings.Location = new System.Drawing.Point(12, 122);
            this.gbConnSettings.Name = "gbConnSettings";
            this.gbConnSettings.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbConnSettings.Size = new System.Drawing.Size(310, 212);
            this.gbConnSettings.TabIndex = 1;
            this.gbConnSettings.TabStop = false;
            this.gbConnSettings.Text = "Connection Settings";
            // 
            // gbProfileProps
            // 
            this.gbProfileProps.Controls.Add(this.lblWebUrl);
            this.gbProfileProps.Controls.Add(this.txtWebUrl);
            this.gbProfileProps.Controls.Add(this.lblProfileName);
            this.gbProfileProps.Controls.Add(this.txtProfileName);
            this.gbProfileProps.Location = new System.Drawing.Point(12, 12);
            this.gbProfileProps.Name = "gbProfileProps";
            this.gbProfileProps.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbProfileProps.Size = new System.Drawing.Size(310, 104);
            this.gbProfileProps.TabIndex = 0;
            this.gbProfileProps.TabStop = false;
            this.gbProfileProps.Text = "Profile Properies";
            // 
            // lblWebUrl
            // 
            this.lblWebUrl.AutoSize = true;
            this.lblWebUrl.Location = new System.Drawing.Point(10, 55);
            this.lblWebUrl.Name = "lblWebUrl";
            this.lblWebUrl.Size = new System.Drawing.Size(86, 13);
            this.lblWebUrl.TabIndex = 2;
            this.lblWebUrl.Text = "Webstation URL";
            // 
            // txtWebUrl
            // 
            this.txtWebUrl.Location = new System.Drawing.Point(13, 71);
            this.txtWebUrl.Name = "txtWebUrl";
            this.txtWebUrl.Size = new System.Drawing.Size(284, 20);
            this.txtWebUrl.TabIndex = 3;
            // 
            // FrmProfileEdit
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(334, 375);
            this.Controls.Add(this.gbProfileProps);
            this.Controls.Add(this.gbConnSettings);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmProfileEdit";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit Profile";
            this.Load += new System.EventHandler(this.FrmProfileEdit_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numPort)).EndInit();
            this.gbConnSettings.ResumeLayout(false);
            this.gbConnSettings.PerformLayout();
            this.gbProfileProps.ResumeLayout(false);
            this.gbProfileProps.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnGenSecretKey;
        private System.Windows.Forms.TextBox txtSecretKey;
        private System.Windows.Forms.Label lblSecretKey;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.NumericUpDown numPort;
        private System.Windows.Forms.TextBox txtHost;
        private System.Windows.Forms.Label lblHost;
        private System.Windows.Forms.TextBox txtProfileName;
        private System.Windows.Forms.Label lblProfileName;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.GroupBox gbConnSettings;
        private System.Windows.Forms.GroupBox gbProfileProps;
        private System.Windows.Forms.Label lblWebUrl;
        private System.Windows.Forms.TextBox txtWebUrl;
    }
}