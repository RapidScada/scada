namespace Scada.Server.Ctrl
{
    partial class FrmCommSettings
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
            this.lblServerPwd = new System.Windows.Forms.Label();
            this.txtServerPwd = new System.Windows.Forms.TextBox();
            this.txtServerUser = new System.Windows.Forms.TextBox();
            this.lblServerUser = new System.Windows.Forms.Label();
            this.lblServerTimeout = new System.Windows.Forms.Label();
            this.numServerTimeout = new System.Windows.Forms.NumericUpDown();
            this.lblServerPort = new System.Windows.Forms.Label();
            this.numServerPort = new System.Windows.Forms.NumericUpDown();
            this.lblServerHost = new System.Windows.Forms.Label();
            this.txtServerHost = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numServerTimeout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numServerPort)).BeginInit();
            this.SuspendLayout();
            // 
            // lblServerPwd
            // 
            this.lblServerPwd.AutoSize = true;
            this.lblServerPwd.Location = new System.Drawing.Point(105, 48);
            this.lblServerPwd.Name = "lblServerPwd";
            this.lblServerPwd.Size = new System.Drawing.Size(45, 13);
            this.lblServerPwd.TabIndex = 8;
            this.lblServerPwd.Text = "Пароль";
            // 
            // txtServerPwd
            // 
            this.txtServerPwd.Location = new System.Drawing.Point(108, 64);
            this.txtServerPwd.Name = "txtServerPwd";
            this.txtServerPwd.Size = new System.Drawing.Size(90, 20);
            this.txtServerPwd.TabIndex = 9;
            this.txtServerPwd.Text = "12345";
            this.txtServerPwd.UseSystemPasswordChar = true;
            // 
            // txtServerUser
            // 
            this.txtServerUser.Location = new System.Drawing.Point(12, 64);
            this.txtServerUser.Name = "txtServerUser";
            this.txtServerUser.Size = new System.Drawing.Size(90, 20);
            this.txtServerUser.TabIndex = 7;
            this.txtServerUser.Text = "ScadaComm";
            // 
            // lblServerUser
            // 
            this.lblServerUser.AutoSize = true;
            this.lblServerUser.Location = new System.Drawing.Point(9, 48);
            this.lblServerUser.Name = "lblServerUser";
            this.lblServerUser.Size = new System.Drawing.Size(80, 13);
            this.lblServerUser.TabIndex = 6;
            this.lblServerUser.Text = "Пользователь";
            // 
            // lblServerTimeout
            // 
            this.lblServerTimeout.AutoSize = true;
            this.lblServerTimeout.Location = new System.Drawing.Point(201, 9);
            this.lblServerTimeout.Name = "lblServerTimeout";
            this.lblServerTimeout.Size = new System.Drawing.Size(50, 13);
            this.lblServerTimeout.TabIndex = 4;
            this.lblServerTimeout.Text = "Таймаут";
            // 
            // numServerTimeout
            // 
            this.numServerTimeout.Location = new System.Drawing.Point(204, 25);
            this.numServerTimeout.Maximum = new decimal(new int[] {
            60000,
            0,
            0,
            0});
            this.numServerTimeout.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numServerTimeout.Name = "numServerTimeout";
            this.numServerTimeout.Size = new System.Drawing.Size(90, 20);
            this.numServerTimeout.TabIndex = 5;
            this.numServerTimeout.Value = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            // 
            // lblServerPort
            // 
            this.lblServerPort.AutoSize = true;
            this.lblServerPort.Location = new System.Drawing.Point(105, 10);
            this.lblServerPort.Name = "lblServerPort";
            this.lblServerPort.Size = new System.Drawing.Size(32, 13);
            this.lblServerPort.TabIndex = 2;
            this.lblServerPort.Text = "Порт";
            // 
            // numServerPort
            // 
            this.numServerPort.Location = new System.Drawing.Point(108, 26);
            this.numServerPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numServerPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numServerPort.Name = "numServerPort";
            this.numServerPort.Size = new System.Drawing.Size(90, 20);
            this.numServerPort.TabIndex = 3;
            this.numServerPort.Value = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            // 
            // lblServerHost
            // 
            this.lblServerHost.AutoSize = true;
            this.lblServerHost.Location = new System.Drawing.Point(9, 9);
            this.lblServerHost.Name = "lblServerHost";
            this.lblServerHost.Size = new System.Drawing.Size(44, 13);
            this.lblServerHost.TabIndex = 0;
            this.lblServerHost.Text = "Сервер";
            // 
            // txtServerHost
            // 
            this.txtServerHost.Location = new System.Drawing.Point(12, 25);
            this.txtServerHost.Name = "txtServerHost";
            this.txtServerHost.Size = new System.Drawing.Size(90, 20);
            this.txtServerHost.TabIndex = 1;
            this.txtServerHost.Text = "localhost";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(138, 90);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 10;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(219, 90);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // FrmCommSettings
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(306, 125);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lblServerPwd);
            this.Controls.Add(this.txtServerPwd);
            this.Controls.Add(this.txtServerUser);
            this.Controls.Add(this.lblServerUser);
            this.Controls.Add(this.lblServerTimeout);
            this.Controls.Add(this.numServerTimeout);
            this.Controls.Add(this.lblServerPort);
            this.Controls.Add(this.numServerPort);
            this.Controls.Add(this.lblServerHost);
            this.Controls.Add(this.txtServerHost);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmCommSettings";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Настройки соединения";
            this.Load += new System.EventHandler(this.FrmCommSettings_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numServerTimeout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numServerPort)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblServerPwd;
        private System.Windows.Forms.TextBox txtServerPwd;
        private System.Windows.Forms.TextBox txtServerUser;
        private System.Windows.Forms.Label lblServerUser;
        private System.Windows.Forms.Label lblServerTimeout;
        private System.Windows.Forms.NumericUpDown numServerTimeout;
        private System.Windows.Forms.Label lblServerPort;
        private System.Windows.Forms.NumericUpDown numServerPort;
        private System.Windows.Forms.Label lblServerHost;
        private System.Windows.Forms.TextBox txtServerHost;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
    }
}