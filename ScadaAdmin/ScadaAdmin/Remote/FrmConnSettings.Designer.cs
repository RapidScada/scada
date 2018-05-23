namespace ScadaAdmin.Remote
{
    partial class FrmConnSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmConnSettings));
            this.lblName = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtHost = new System.Windows.Forms.TextBox();
            this.lblHost = new System.Windows.Forms.Label();
            this.numPort = new System.Windows.Forms.NumericUpDown();
            this.lblPort = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.lblUsername = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtScadaInstance = new System.Windows.Forms.TextBox();
            this.lblScadaInstance = new System.Windows.Forms.Label();
            this.txtSecretKey = new System.Windows.Forms.TextBox();
            this.lblSecretKey = new System.Windows.Forms.Label();
            this.btnGenSecretKey = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.numPort)).BeginInit();
            this.SuspendLayout();
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(9, 9);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(83, 13);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Наименование";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(12, 25);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(250, 20);
            this.txtName.TabIndex = 1;
            // 
            // txtHost
            // 
            this.txtHost.Location = new System.Drawing.Point(12, 64);
            this.txtHost.Name = "txtHost";
            this.txtHost.Size = new System.Drawing.Size(169, 20);
            this.txtHost.TabIndex = 3;
            // 
            // lblHost
            // 
            this.lblHost.AutoSize = true;
            this.lblHost.Location = new System.Drawing.Point(9, 48);
            this.lblHost.Name = "lblHost";
            this.lblHost.Size = new System.Drawing.Size(44, 13);
            this.lblHost.TabIndex = 2;
            this.lblHost.Text = "Сервер";
            // 
            // numPort
            // 
            this.numPort.Location = new System.Drawing.Point(187, 64);
            this.numPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numPort.Name = "numPort";
            this.numPort.Size = new System.Drawing.Size(75, 20);
            this.numPort.TabIndex = 5;
            this.numPort.Value = new decimal(new int[] {
            10002,
            0,
            0,
            0});
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.Location = new System.Drawing.Point(184, 48);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(32, 13);
            this.lblPort.TabIndex = 4;
            this.lblPort.Text = "Порт";
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(12, 103);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(250, 20);
            this.txtUsername.TabIndex = 7;
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(9, 87);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(80, 13);
            this.lblUsername.TabIndex = 6;
            this.lblUsername.Text = "Пользователь";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(12, 142);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(250, 20);
            this.txtPassword.TabIndex = 9;
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(9, 126);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(45, 13);
            this.lblPassword.TabIndex = 8;
            this.lblPassword.Text = "Пароль";
            // 
            // txtScadaInstance
            // 
            this.txtScadaInstance.Location = new System.Drawing.Point(12, 181);
            this.txtScadaInstance.Name = "txtScadaInstance";
            this.txtScadaInstance.Size = new System.Drawing.Size(250, 20);
            this.txtScadaInstance.TabIndex = 11;
            // 
            // lblScadaInstance
            // 
            this.lblScadaInstance.AutoSize = true;
            this.lblScadaInstance.Location = new System.Drawing.Point(9, 165);
            this.lblScadaInstance.Name = "lblScadaInstance";
            this.lblScadaInstance.Size = new System.Drawing.Size(112, 13);
            this.lblScadaInstance.TabIndex = 10;
            this.lblScadaInstance.Text = "Экземпляр системы";
            // 
            // txtSecretKey
            // 
            this.txtSecretKey.Location = new System.Drawing.Point(12, 230);
            this.txtSecretKey.Multiline = true;
            this.txtSecretKey.Name = "txtSecretKey";
            this.txtSecretKey.Size = new System.Drawing.Size(250, 40);
            this.txtSecretKey.TabIndex = 14;
            // 
            // lblSecretKey
            // 
            this.lblSecretKey.AutoSize = true;
            this.lblSecretKey.Location = new System.Drawing.Point(9, 214);
            this.lblSecretKey.Name = "lblSecretKey";
            this.lblSecretKey.Size = new System.Drawing.Size(91, 13);
            this.lblSecretKey.TabIndex = 12;
            this.lblSecretKey.Text = "Секретный ключ";
            // 
            // btnGenSecretKey
            // 
            this.btnGenSecretKey.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnGenSecretKey.Image = ((System.Drawing.Image)(resources.GetObject("btnGenSecretKey.Image")));
            this.btnGenSecretKey.Location = new System.Drawing.Point(242, 207);
            this.btnGenSecretKey.Name = "btnGenSecretKey";
            this.btnGenSecretKey.Size = new System.Drawing.Size(20, 20);
            this.btnGenSecretKey.TabIndex = 13;
            this.toolTip.SetToolTip(this.btnGenSecretKey, "Генерировать секретный ключ");
            this.btnGenSecretKey.UseVisualStyleBackColor = true;
            this.btnGenSecretKey.Click += new System.EventHandler(this.btnGenSecretKey_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(106, 276);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 15;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(187, 276);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 16;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // FrmConnSettings
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(274, 311);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnGenSecretKey);
            this.Controls.Add(this.txtSecretKey);
            this.Controls.Add(this.lblSecretKey);
            this.Controls.Add(this.txtScadaInstance);
            this.Controls.Add(this.lblScadaInstance);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.lblUsername);
            this.Controls.Add(this.lblPort);
            this.Controls.Add(this.numPort);
            this.Controls.Add(this.txtHost);
            this.Controls.Add(this.lblHost);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmConnSettings";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Настройки соединения";
            this.Load += new System.EventHandler(this.FrmConnSettings_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numPort)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtHost;
        private System.Windows.Forms.Label lblHost;
        private System.Windows.Forms.NumericUpDown numPort;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtScadaInstance;
        private System.Windows.Forms.Label lblScadaInstance;
        private System.Windows.Forms.TextBox txtSecretKey;
        private System.Windows.Forms.Label lblSecretKey;
        private System.Windows.Forms.Button btnGenSecretKey;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ToolTip toolTip;
    }
}