namespace Scada.Comm.Devices.DbImport.UI
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
            this.cbDbType = new System.Windows.Forms.ComboBox();
            this.gbConnection = new System.Windows.Forms.GroupBox();
            this.txtConnectionString = new System.Windows.Forms.TextBox();
            this.lblConnectionString = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.lblUser = new System.Windows.Forms.Label();
            this.txtDatabase = new System.Windows.Forms.TextBox();
            this.lblDatabase = new System.Windows.Forms.Label();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.lblServer = new System.Windows.Forms.Label();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.pageDatabase = new System.Windows.Forms.TabPage();
            this.gbDbType = new System.Windows.Forms.GroupBox();
            this.pageQuery = new System.Windows.Forms.TabPage();
            this.chkAutoTagCount = new System.Windows.Forms.CheckBox();
            this.numTagCount = new System.Windows.Forms.NumericUpDown();
            this.lblTagCount = new System.Windows.Forms.Label();
            this.txtSelectQuery = new System.Windows.Forms.TextBox();
            this.lblSelectQuery = new System.Windows.Forms.Label();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.gbConnection.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.pageDatabase.SuspendLayout();
            this.gbDbType.SuspendLayout();
            this.pageQuery.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTagCount)).BeginInit();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbDbType
            // 
            this.cbDbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDbType.FormattingEnabled = true;
            this.cbDbType.Items.AddRange(new object[] {
            "<Choose database type>",
            "Microsoft SQL Server",
            "Oracle",
            "PostgreSQL",
            "MySQL",
            "OLE DB"});
            this.cbDbType.Location = new System.Drawing.Point(13, 19);
            this.cbDbType.Name = "cbDbType";
            this.cbDbType.Size = new System.Drawing.Size(388, 21);
            this.cbDbType.TabIndex = 0;
            // 
            // gbConnection
            // 
            this.gbConnection.Controls.Add(this.txtConnectionString);
            this.gbConnection.Controls.Add(this.lblConnectionString);
            this.gbConnection.Controls.Add(this.txtPassword);
            this.gbConnection.Controls.Add(this.lblPassword);
            this.gbConnection.Controls.Add(this.txtUser);
            this.gbConnection.Controls.Add(this.lblUser);
            this.gbConnection.Controls.Add(this.txtDatabase);
            this.gbConnection.Controls.Add(this.lblDatabase);
            this.gbConnection.Controls.Add(this.txtServer);
            this.gbConnection.Controls.Add(this.lblServer);
            this.gbConnection.Location = new System.Drawing.Point(6, 65);
            this.gbConnection.Name = "gbConnection";
            this.gbConnection.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbConnection.Size = new System.Drawing.Size(414, 212);
            this.gbConnection.TabIndex = 1;
            this.gbConnection.TabStop = false;
            this.gbConnection.Text = "Connection";
            // 
            // txtConnectionString
            // 
            this.txtConnectionString.Location = new System.Drawing.Point(13, 149);
            this.txtConnectionString.Multiline = true;
            this.txtConnectionString.Name = "txtConnectionString";
            this.txtConnectionString.Size = new System.Drawing.Size(388, 50);
            this.txtConnectionString.TabIndex = 9;
            // 
            // lblConnectionString
            // 
            this.lblConnectionString.AutoSize = true;
            this.lblConnectionString.Location = new System.Drawing.Point(10, 133);
            this.lblConnectionString.Name = "lblConnectionString";
            this.lblConnectionString.Size = new System.Drawing.Size(89, 13);
            this.lblConnectionString.TabIndex = 8;
            this.lblConnectionString.Text = "Connection string";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(210, 110);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(191, 20);
            this.txtPassword.TabIndex = 7;
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(207, 94);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(53, 13);
            this.lblPassword.TabIndex = 6;
            this.lblPassword.Text = "Password";
            // 
            // txtUser
            // 
            this.txtUser.Location = new System.Drawing.Point(13, 110);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(191, 20);
            this.txtUser.TabIndex = 5;
            // 
            // lblUser
            // 
            this.lblUser.AutoSize = true;
            this.lblUser.Location = new System.Drawing.Point(10, 94);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(29, 13);
            this.lblUser.TabIndex = 4;
            this.lblUser.Text = "User";
            // 
            // txtDatabase
            // 
            this.txtDatabase.Location = new System.Drawing.Point(13, 71);
            this.txtDatabase.Name = "txtDatabase";
            this.txtDatabase.Size = new System.Drawing.Size(388, 20);
            this.txtDatabase.TabIndex = 3;
            // 
            // lblDatabase
            // 
            this.lblDatabase.AutoSize = true;
            this.lblDatabase.Location = new System.Drawing.Point(10, 55);
            this.lblDatabase.Name = "lblDatabase";
            this.lblDatabase.Size = new System.Drawing.Size(53, 13);
            this.lblDatabase.TabIndex = 2;
            this.lblDatabase.Text = "Database";
            // 
            // txtServer
            // 
            this.txtServer.Location = new System.Drawing.Point(13, 32);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(388, 20);
            this.txtServer.TabIndex = 1;
            // 
            // lblServer
            // 
            this.lblServer.AutoSize = true;
            this.lblServer.Location = new System.Drawing.Point(10, 16);
            this.lblServer.Name = "lblServer";
            this.lblServer.Size = new System.Drawing.Size(38, 13);
            this.lblServer.TabIndex = 0;
            this.lblServer.Text = "Server";
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.pageDatabase);
            this.tabControl.Controls.Add(this.pageQuery);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(434, 309);
            this.tabControl.TabIndex = 0;
            // 
            // pageDatabase
            // 
            this.pageDatabase.Controls.Add(this.gbDbType);
            this.pageDatabase.Controls.Add(this.gbConnection);
            this.pageDatabase.Location = new System.Drawing.Point(4, 22);
            this.pageDatabase.Name = "pageDatabase";
            this.pageDatabase.Padding = new System.Windows.Forms.Padding(3);
            this.pageDatabase.Size = new System.Drawing.Size(426, 283);
            this.pageDatabase.TabIndex = 0;
            this.pageDatabase.Text = "Database";
            this.pageDatabase.UseVisualStyleBackColor = true;
            // 
            // gbDbType
            // 
            this.gbDbType.Controls.Add(this.cbDbType);
            this.gbDbType.Location = new System.Drawing.Point(6, 6);
            this.gbDbType.Name = "gbDbType";
            this.gbDbType.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbDbType.Size = new System.Drawing.Size(414, 53);
            this.gbDbType.TabIndex = 0;
            this.gbDbType.TabStop = false;
            this.gbDbType.Text = "Database Type";
            // 
            // pageQuery
            // 
            this.pageQuery.Controls.Add(this.chkAutoTagCount);
            this.pageQuery.Controls.Add(this.numTagCount);
            this.pageQuery.Controls.Add(this.lblTagCount);
            this.pageQuery.Controls.Add(this.txtSelectQuery);
            this.pageQuery.Controls.Add(this.lblSelectQuery);
            this.pageQuery.Location = new System.Drawing.Point(4, 22);
            this.pageQuery.Name = "pageQuery";
            this.pageQuery.Padding = new System.Windows.Forms.Padding(3);
            this.pageQuery.Size = new System.Drawing.Size(426, 283);
            this.pageQuery.TabIndex = 1;
            this.pageQuery.Text = "Query";
            this.pageQuery.UseVisualStyleBackColor = true;
            // 
            // chkAutoTagCount
            // 
            this.chkAutoTagCount.AutoSize = true;
            this.chkAutoTagCount.Location = new System.Drawing.Point(112, 259);
            this.chkAutoTagCount.Name = "chkAutoTagCount";
            this.chkAutoTagCount.Size = new System.Drawing.Size(48, 17);
            this.chkAutoTagCount.TabIndex = 4;
            this.chkAutoTagCount.Text = "Auto";
            this.chkAutoTagCount.UseVisualStyleBackColor = true;
            // 
            // numTagCount
            // 
            this.numTagCount.Location = new System.Drawing.Point(6, 257);
            this.numTagCount.Name = "numTagCount";
            this.numTagCount.Size = new System.Drawing.Size(100, 20);
            this.numTagCount.TabIndex = 3;
            // 
            // lblTagCount
            // 
            this.lblTagCount.AutoSize = true;
            this.lblTagCount.Location = new System.Drawing.Point(3, 241);
            this.lblTagCount.Name = "lblTagCount";
            this.lblTagCount.Size = new System.Drawing.Size(56, 13);
            this.lblTagCount.TabIndex = 2;
            this.lblTagCount.Text = "Tag count";
            // 
            // txtSelectQuery
            // 
            this.txtSelectQuery.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtSelectQuery.Location = new System.Drawing.Point(6, 19);
            this.txtSelectQuery.Multiline = true;
            this.txtSelectQuery.Name = "txtSelectQuery";
            this.txtSelectQuery.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtSelectQuery.Size = new System.Drawing.Size(414, 219);
            this.txtSelectQuery.TabIndex = 1;
            this.txtSelectQuery.WordWrap = false;
            // 
            // lblSelectQuery
            // 
            this.lblSelectQuery.AutoSize = true;
            this.lblSelectQuery.Location = new System.Drawing.Point(3, 3);
            this.lblSelectQuery.Name = "lblSelectQuery";
            this.lblSelectQuery.Size = new System.Drawing.Size(28, 13);
            this.lblSelectQuery.TabIndex = 0;
            this.lblSelectQuery.Text = "SQL";
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.btnCancel);
            this.pnlBottom.Controls.Add(this.btnClose);
            this.pnlBottom.Controls.Add(this.btnSave);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 309);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(434, 41);
            this.pnlBottom.TabIndex = 1;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Enabled = false;
            this.btnCancel.Location = new System.Drawing.Point(266, 6);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(347, 6);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Enabled = false;
            this.btnSave.Location = new System.Drawing.Point(185, 6);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // FrmConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(434, 350);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.pnlBottom);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmConfig";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "DB Import - Device {0} Properties";
            this.gbConnection.ResumeLayout(false);
            this.gbConnection.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.pageDatabase.ResumeLayout(false);
            this.gbDbType.ResumeLayout(false);
            this.pageQuery.ResumeLayout(false);
            this.pageQuery.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTagCount)).EndInit();
            this.pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ComboBox cbDbType;
        private System.Windows.Forms.GroupBox gbConnection;
        private System.Windows.Forms.TextBox txtDatabase;
        private System.Windows.Forms.Label lblDatabase;
        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.Label lblServer;
        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtConnectionString;
        private System.Windows.Forms.Label lblConnectionString;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage pageDatabase;
        private System.Windows.Forms.TabPage pageQuery;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.GroupBox gbDbType;
        private System.Windows.Forms.TextBox txtSelectQuery;
        private System.Windows.Forms.Label lblSelectQuery;
        private System.Windows.Forms.NumericUpDown numTagCount;
        private System.Windows.Forms.Label lblTagCount;
        private System.Windows.Forms.CheckBox chkAutoTagCount;
    }
}