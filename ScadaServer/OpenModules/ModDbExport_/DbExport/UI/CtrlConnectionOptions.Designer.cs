
namespace Scada.Server.Modules.DbExport.UI
{
    partial class CtrlConnectionOptions
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.gbConnection = new System.Windows.Forms.GroupBox();
            this.chkConnectionString = new System.Windows.Forms.CheckBox();
            this.txtConnectionString = new System.Windows.Forms.TextBox();
            this.lblDataBase = new System.Windows.Forms.Label();
            this.txtDataBase = new System.Windows.Forms.TextBox();
            this.lblDBMS = new System.Windows.Forms.Label();
            this.txtDBMS = new System.Windows.Forms.TextBox();
            this.lblServer = new System.Windows.Forms.Label();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.lblServerPwd = new System.Windows.Forms.Label();
            this.txtUserPwd = new System.Windows.Forms.TextBox();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.lblServerUser = new System.Windows.Forms.Label();
            this.gbConnection.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbConnection
            // 
            this.gbConnection.AutoSize = true;
            this.gbConnection.Controls.Add(this.chkConnectionString);
            this.gbConnection.Controls.Add(this.txtConnectionString);
            this.gbConnection.Controls.Add(this.lblDataBase);
            this.gbConnection.Controls.Add(this.txtDataBase);
            this.gbConnection.Controls.Add(this.lblDBMS);
            this.gbConnection.Controls.Add(this.txtDBMS);
            this.gbConnection.Controls.Add(this.lblServer);
            this.gbConnection.Controls.Add(this.txtServer);
            this.gbConnection.Controls.Add(this.lblServerPwd);
            this.gbConnection.Controls.Add(this.txtUserPwd);
            this.gbConnection.Controls.Add(this.txtUser);
            this.gbConnection.Controls.Add(this.lblServerUser);
            this.gbConnection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbConnection.Location = new System.Drawing.Point(0, 0);
            this.gbConnection.Name = "gbConnection";
            this.gbConnection.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbConnection.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.gbConnection.Size = new System.Drawing.Size(414, 483);
            this.gbConnection.TabIndex = 0;
            this.gbConnection.TabStop = false;
            this.gbConnection.Text = "Connection Options";
            // 
            // chkConnectionString
            // 
            this.chkConnectionString.AutoSize = true;
            this.chkConnectionString.Location = new System.Drawing.Point(13, 257);
            this.chkConnectionString.Name = "chkConnectionString";
            this.chkConnectionString.Size = new System.Drawing.Size(108, 17);
            this.chkConnectionString.TabIndex = 10;
            this.chkConnectionString.Text = "Connection string";
            this.chkConnectionString.UseVisualStyleBackColor = true;
            this.chkConnectionString.CheckedChanged += new System.EventHandler(this.chkConnectionString_CheckedChanged);
            // 
            // txtConnectionString
            // 
            this.txtConnectionString.Location = new System.Drawing.Point(13, 280);
            this.txtConnectionString.Multiline = true;
            this.txtConnectionString.Name = "txtConnectionString";
            this.txtConnectionString.Size = new System.Drawing.Size(386, 188);
            this.txtConnectionString.TabIndex = 11;
            this.txtConnectionString.TextChanged += new System.EventHandler(this.txtConnectionString_TextChanged);
            // 
            // lblDataBase
            // 
            this.lblDataBase.AutoSize = true;
            this.lblDataBase.Location = new System.Drawing.Point(10, 69);
            this.lblDataBase.Name = "lblDataBase";
            this.lblDataBase.Size = new System.Drawing.Size(53, 13);
            this.lblDataBase.TabIndex = 2;
            this.lblDataBase.Text = "Database";
            // 
            // txtDataBase
            // 
            this.txtDataBase.Location = new System.Drawing.Point(13, 85);
            this.txtDataBase.Name = "txtDataBase";
            this.txtDataBase.Size = new System.Drawing.Size(386, 20);
            this.txtDataBase.TabIndex = 3;
            this.txtDataBase.Text = "rapid scada";
            this.txtDataBase.TextChanged += new System.EventHandler(this.txtDataBase_TextChanged);
            // 
            // lblDBMS
            // 
            this.lblDBMS.AutoSize = true;
            this.lblDBMS.Location = new System.Drawing.Point(10, 22);
            this.lblDBMS.Name = "lblDBMS";
            this.lblDBMS.Size = new System.Drawing.Size(38, 13);
            this.lblDBMS.TabIndex = 0;
            this.lblDBMS.Text = "DBMS";
            // 
            // txtDBMS
            // 
            this.txtDBMS.Location = new System.Drawing.Point(13, 38);
            this.txtDBMS.Name = "txtDBMS";
            this.txtDBMS.ReadOnly = true;
            this.txtDBMS.Size = new System.Drawing.Size(386, 20);
            this.txtDBMS.TabIndex = 1;
            this.txtDBMS.TextChanged += new System.EventHandler(this.txtDBMS_TextChanged);
            // 
            // lblServer
            // 
            this.lblServer.AutoSize = true;
            this.lblServer.Location = new System.Drawing.Point(10, 116);
            this.lblServer.Name = "lblServer";
            this.lblServer.Size = new System.Drawing.Size(38, 13);
            this.lblServer.TabIndex = 4;
            this.lblServer.Text = "Server";
            // 
            // txtServer
            // 
            this.txtServer.Location = new System.Drawing.Point(13, 132);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(385, 20);
            this.txtServer.TabIndex = 5;
            this.txtServer.Text = "localhost";
            this.txtServer.TextChanged += new System.EventHandler(this.txtServer_TextChanged);
            // 
            // lblServerPwd
            // 
            this.lblServerPwd.AutoSize = true;
            this.lblServerPwd.Location = new System.Drawing.Point(10, 210);
            this.lblServerPwd.Name = "lblServerPwd";
            this.lblServerPwd.Size = new System.Drawing.Size(53, 13);
            this.lblServerPwd.TabIndex = 8;
            this.lblServerPwd.Text = "Password";
            // 
            // txtUserPwd
            // 
            this.txtUserPwd.Location = new System.Drawing.Point(13, 226);
            this.txtUserPwd.Name = "txtUserPwd";
            this.txtUserPwd.Size = new System.Drawing.Size(385, 20);
            this.txtUserPwd.TabIndex = 9;
            this.txtUserPwd.UseSystemPasswordChar = true;
            this.txtUserPwd.TextChanged += new System.EventHandler(this.txtUserPwd_TextChanged);
            // 
            // txtUser
            // 
            this.txtUser.Location = new System.Drawing.Point(13, 179);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(386, 20);
            this.txtUser.TabIndex = 7;
            this.txtUser.TextChanged += new System.EventHandler(this.txtUser_TextChanged);
            // 
            // lblServerUser
            // 
            this.lblServerUser.AutoSize = true;
            this.lblServerUser.Location = new System.Drawing.Point(10, 163);
            this.lblServerUser.Name = "lblServerUser";
            this.lblServerUser.Size = new System.Drawing.Size(29, 13);
            this.lblServerUser.TabIndex = 6;
            this.lblServerUser.Text = "User";
            // 
            // CtrlConnectionOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbConnection);
            this.Name = "CtrlConnectionOptions";
            this.Size = new System.Drawing.Size(414, 483);
            this.gbConnection.ResumeLayout(false);
            this.gbConnection.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gbConnection;
        private System.Windows.Forms.Label lblServerPwd;
        private System.Windows.Forms.TextBox txtUserPwd;
        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.Label lblServerUser;
        private System.Windows.Forms.Label lblServer;
        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.Label lblDBMS;
        private System.Windows.Forms.TextBox txtDBMS;
        private System.Windows.Forms.Label lblDataBase;
        private System.Windows.Forms.TextBox txtDataBase;
        private System.Windows.Forms.TextBox txtConnectionString;
        private System.Windows.Forms.CheckBox chkConnectionString;
    }
}
