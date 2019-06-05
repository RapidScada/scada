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
            this.cbDataSourceType = new System.Windows.Forms.ComboBox();
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
            this.gbDataSourceType = new System.Windows.Forms.GroupBox();
            this.pageQuery = new System.Windows.Forms.TabPage();
            this.chkAutoTagCount = new System.Windows.Forms.CheckBox();
            this.numTagCount = new System.Windows.Forms.NumericUpDown();
            this.lblTagCount = new System.Windows.Forms.Label();
            this.txtSelectQuery = new System.Windows.Forms.TextBox();
            this.lblSelectQuery = new System.Windows.Forms.Label();
            this.pageCommands = new System.Windows.Forms.TabPage();
            this.gbCommandParams = new System.Windows.Forms.GroupBox();
            this.txtCmdQuery = new System.Windows.Forms.TextBox();
            this.lblCmdQuery = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.numCmdNum = new System.Windows.Forms.NumericUpDown();
            this.lblCmdNum = new System.Windows.Forms.Label();
            this.gbCommand = new System.Windows.Forms.GroupBox();
            this.cbCommand = new System.Windows.Forms.ComboBox();
            this.btnDeleteCommand = new System.Windows.Forms.Button();
            this.btnCreateCommand = new System.Windows.Forms.Button();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.gbConnection.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.pageDatabase.SuspendLayout();
            this.gbDataSourceType.SuspendLayout();
            this.pageQuery.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTagCount)).BeginInit();
            this.pageCommands.SuspendLayout();
            this.gbCommandParams.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCmdNum)).BeginInit();
            this.gbCommand.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbDataSourceType
            // 
            this.cbDataSourceType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDataSourceType.FormattingEnabled = true;
            this.cbDataSourceType.Items.AddRange(new object[] {
            "<Choose database type>",
            "Microsoft SQL Server",
            "Oracle",
            "PostgreSQL",
            "MySQL",
            "OLE DB"});
            this.cbDataSourceType.Location = new System.Drawing.Point(13, 19);
            this.cbDataSourceType.Name = "cbDataSourceType";
            this.cbDataSourceType.Size = new System.Drawing.Size(388, 21);
            this.cbDataSourceType.TabIndex = 0;
            this.cbDataSourceType.SelectedIndexChanged += new System.EventHandler(this.cbDataSourceType_SelectedIndexChanged);
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
            this.txtConnectionString.TextChanged += new System.EventHandler(this.txtConnectionString_TextChanged);
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
            this.txtPassword.TextChanged += new System.EventHandler(this.txtConnProp_TextChanged);
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
            this.txtUser.TextChanged += new System.EventHandler(this.txtConnProp_TextChanged);
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
            this.txtDatabase.TextChanged += new System.EventHandler(this.txtConnProp_TextChanged);
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
            this.txtServer.TextChanged += new System.EventHandler(this.txtConnProp_TextChanged);
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
            this.tabControl.Controls.Add(this.pageCommands);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(434, 309);
            this.tabControl.TabIndex = 0;
            // 
            // pageDatabase
            // 
            this.pageDatabase.Controls.Add(this.gbDataSourceType);
            this.pageDatabase.Controls.Add(this.gbConnection);
            this.pageDatabase.Location = new System.Drawing.Point(4, 22);
            this.pageDatabase.Name = "pageDatabase";
            this.pageDatabase.Padding = new System.Windows.Forms.Padding(3);
            this.pageDatabase.Size = new System.Drawing.Size(426, 283);
            this.pageDatabase.TabIndex = 0;
            this.pageDatabase.Text = "Database";
            this.pageDatabase.UseVisualStyleBackColor = true;
            // 
            // gbDataSourceType
            // 
            this.gbDataSourceType.Controls.Add(this.cbDataSourceType);
            this.gbDataSourceType.Location = new System.Drawing.Point(6, 6);
            this.gbDataSourceType.Name = "gbDataSourceType";
            this.gbDataSourceType.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbDataSourceType.Size = new System.Drawing.Size(414, 53);
            this.gbDataSourceType.TabIndex = 0;
            this.gbDataSourceType.TabStop = false;
            this.gbDataSourceType.Text = "Data Source Type";
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
            this.pageQuery.Text = "Data Retrieval";
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
            this.chkAutoTagCount.CheckedChanged += new System.EventHandler(this.chkAutoTagCount_CheckedChanged);
            // 
            // numTagCount
            // 
            this.numTagCount.Location = new System.Drawing.Point(6, 257);
            this.numTagCount.Name = "numTagCount";
            this.numTagCount.Size = new System.Drawing.Size(100, 20);
            this.numTagCount.TabIndex = 3;
            this.numTagCount.ValueChanged += new System.EventHandler(this.control_Changed);
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
            this.txtSelectQuery.AcceptsReturn = true;
            this.txtSelectQuery.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtSelectQuery.Location = new System.Drawing.Point(6, 19);
            this.txtSelectQuery.Multiline = true;
            this.txtSelectQuery.Name = "txtSelectQuery";
            this.txtSelectQuery.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtSelectQuery.Size = new System.Drawing.Size(414, 219);
            this.txtSelectQuery.TabIndex = 1;
            this.txtSelectQuery.WordWrap = false;
            this.txtSelectQuery.TextChanged += new System.EventHandler(this.control_Changed);
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
            // pageCommands
            // 
            this.pageCommands.Controls.Add(this.gbCommandParams);
            this.pageCommands.Controls.Add(this.gbCommand);
            this.pageCommands.Location = new System.Drawing.Point(4, 22);
            this.pageCommands.Name = "pageCommands";
            this.pageCommands.Padding = new System.Windows.Forms.Padding(3);
            this.pageCommands.Size = new System.Drawing.Size(426, 283);
            this.pageCommands.TabIndex = 2;
            this.pageCommands.Text = "Commands";
            this.pageCommands.UseVisualStyleBackColor = true;
            // 
            // gbCommandParams
            // 
            this.gbCommandParams.Controls.Add(this.txtCmdQuery);
            this.gbCommandParams.Controls.Add(this.lblCmdQuery);
            this.gbCommandParams.Controls.Add(this.txtName);
            this.gbCommandParams.Controls.Add(this.lblName);
            this.gbCommandParams.Controls.Add(this.numCmdNum);
            this.gbCommandParams.Controls.Add(this.lblCmdNum);
            this.gbCommandParams.Location = new System.Drawing.Point(6, 67);
            this.gbCommandParams.Name = "gbCommandParams";
            this.gbCommandParams.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbCommandParams.Size = new System.Drawing.Size(414, 210);
            this.gbCommandParams.TabIndex = 1;
            this.gbCommandParams.TabStop = false;
            this.gbCommandParams.Text = "Command Parameters";
            // 
            // txtCmdQuery
            // 
            this.txtCmdQuery.AcceptsReturn = true;
            this.txtCmdQuery.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtCmdQuery.Location = new System.Drawing.Point(13, 71);
            this.txtCmdQuery.Multiline = true;
            this.txtCmdQuery.Name = "txtCmdQuery";
            this.txtCmdQuery.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtCmdQuery.Size = new System.Drawing.Size(388, 126);
            this.txtCmdQuery.TabIndex = 5;
            this.txtCmdQuery.WordWrap = false;
            this.txtCmdQuery.TextChanged += new System.EventHandler(this.txtCmdQuery_TextChanged);
            // 
            // lblCmdQuery
            // 
            this.lblCmdQuery.AutoSize = true;
            this.lblCmdQuery.Location = new System.Drawing.Point(10, 55);
            this.lblCmdQuery.Name = "lblCmdQuery";
            this.lblCmdQuery.Size = new System.Drawing.Size(28, 13);
            this.lblCmdQuery.TabIndex = 4;
            this.lblCmdQuery.Text = "SQL";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(119, 32);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(282, 20);
            this.txtName.TabIndex = 3;
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(116, 16);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(35, 13);
            this.lblName.TabIndex = 2;
            this.lblName.Text = "Name";
            // 
            // numCmdNum
            // 
            this.numCmdNum.Location = new System.Drawing.Point(13, 32);
            this.numCmdNum.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numCmdNum.Name = "numCmdNum";
            this.numCmdNum.Size = new System.Drawing.Size(100, 20);
            this.numCmdNum.TabIndex = 1;
            this.numCmdNum.ValueChanged += new System.EventHandler(this.numCmdNum_ValueChanged);
            // 
            // lblCmdNum
            // 
            this.lblCmdNum.AutoSize = true;
            this.lblCmdNum.Location = new System.Drawing.Point(10, 16);
            this.lblCmdNum.Name = "lblCmdNum";
            this.lblCmdNum.Size = new System.Drawing.Size(92, 13);
            this.lblCmdNum.TabIndex = 0;
            this.lblCmdNum.Text = "Command number";
            // 
            // gbCommand
            // 
            this.gbCommand.Controls.Add(this.cbCommand);
            this.gbCommand.Controls.Add(this.btnDeleteCommand);
            this.gbCommand.Controls.Add(this.btnCreateCommand);
            this.gbCommand.Location = new System.Drawing.Point(6, 6);
            this.gbCommand.Name = "gbCommand";
            this.gbCommand.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbCommand.Size = new System.Drawing.Size(414, 55);
            this.gbCommand.TabIndex = 0;
            this.gbCommand.TabStop = false;
            this.gbCommand.Text = "Command";
            // 
            // cbCommand
            // 
            this.cbCommand.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCommand.FormattingEnabled = true;
            this.cbCommand.Location = new System.Drawing.Point(13, 20);
            this.cbCommand.Name = "cbCommand";
            this.cbCommand.Size = new System.Drawing.Size(226, 21);
            this.cbCommand.TabIndex = 0;
            this.cbCommand.SelectedIndexChanged += new System.EventHandler(this.cbCommand_SelectedIndexChanged);
            // 
            // btnDeleteCommand
            // 
            this.btnDeleteCommand.Location = new System.Drawing.Point(326, 19);
            this.btnDeleteCommand.Name = "btnDeleteCommand";
            this.btnDeleteCommand.Size = new System.Drawing.Size(75, 23);
            this.btnDeleteCommand.TabIndex = 2;
            this.btnDeleteCommand.Text = "Delete";
            this.btnDeleteCommand.UseVisualStyleBackColor = true;
            this.btnDeleteCommand.Click += new System.EventHandler(this.btnDeleteCommand_Click);
            // 
            // btnCreateCommand
            // 
            this.btnCreateCommand.Location = new System.Drawing.Point(245, 19);
            this.btnCreateCommand.Name = "btnCreateCommand";
            this.btnCreateCommand.Size = new System.Drawing.Size(75, 23);
            this.btnCreateCommand.TabIndex = 1;
            this.btnCreateCommand.Text = "Create";
            this.btnCreateCommand.UseVisualStyleBackColor = true;
            this.btnCreateCommand.Click += new System.EventHandler(this.btnCreateCommand_Click);
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.btnClose);
            this.pnlBottom.Controls.Add(this.btnSave);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 309);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(434, 41);
            this.pnlBottom.TabIndex = 1;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(347, 6);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(266, 6);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
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
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmConfig_FormClosing);
            this.Load += new System.EventHandler(this.FrmConfig_Load);
            this.gbConnection.ResumeLayout(false);
            this.gbConnection.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.pageDatabase.ResumeLayout(false);
            this.gbDataSourceType.ResumeLayout(false);
            this.pageQuery.ResumeLayout(false);
            this.pageQuery.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTagCount)).EndInit();
            this.pageCommands.ResumeLayout(false);
            this.gbCommandParams.ResumeLayout(false);
            this.gbCommandParams.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCmdNum)).EndInit();
            this.gbCommand.ResumeLayout(false);
            this.pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ComboBox cbDataSourceType;
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
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.GroupBox gbDataSourceType;
        private System.Windows.Forms.TextBox txtSelectQuery;
        private System.Windows.Forms.Label lblSelectQuery;
        private System.Windows.Forms.NumericUpDown numTagCount;
        private System.Windows.Forms.Label lblTagCount;
        private System.Windows.Forms.CheckBox chkAutoTagCount;
        private System.Windows.Forms.TabPage pageCommands;
        private System.Windows.Forms.ComboBox cbCommand;
        private System.Windows.Forms.GroupBox gbCommand;
        private System.Windows.Forms.Button btnDeleteCommand;
        private System.Windows.Forms.Button btnCreateCommand;
        private System.Windows.Forms.GroupBox gbCommandParams;
        private System.Windows.Forms.TextBox txtCmdQuery;
        private System.Windows.Forms.Label lblCmdQuery;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.NumericUpDown numCmdNum;
        private System.Windows.Forms.Label lblCmdNum;
    }
}