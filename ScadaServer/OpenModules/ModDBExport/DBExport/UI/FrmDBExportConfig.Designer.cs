namespace Scada.Server.Modules.DBExport
{
    partial class FrmDBExportConfig
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmDBExportConfig));
            this.treeView = new System.Windows.Forms.TreeView();
            this.ilTree = new System.Windows.Forms.ImageList(this.components);
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.pageConnection = new System.Windows.Forms.TabPage();
            this.txtConnectionString = new System.Windows.Forms.TextBox();
            this.lblConnectionString = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.lblUser = new System.Windows.Forms.Label();
            this.txtDatabase = new System.Windows.Forms.TextBox();
            this.lblDatabase = new System.Windows.Forms.Label();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.lblServer = new System.Windows.Forms.Label();
            this.pageExportCurDataQuery = new System.Windows.Forms.TabPage();
            this.ctrlExportCurDataQuery = new Scada.Server.Modules.DBExport.CtrlExportQuery();
            this.pageExportArcDataQuery = new System.Windows.Forms.TabPage();
            this.ctrlExportArcDataQuery = new Scada.Server.Modules.DBExport.CtrlExportQuery();
            this.pageExportEventQuery = new System.Windows.Forms.TabPage();
            this.ctrlExportEventQuery = new Scada.Server.Modules.DBExport.CtrlExportQuery();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.ddbAddDataSource = new System.Windows.Forms.ToolStripDropDownButton();
            this.miAddSqlDataSource = new System.Windows.Forms.ToolStripMenuItem();
            this.miAddOraDataSource = new System.Windows.Forms.ToolStripMenuItem();
            this.miAddPgSqlDataSource = new System.Windows.Forms.ToolStripMenuItem();
            this.miAddMySqlDataSource = new System.Windows.Forms.ToolStripMenuItem();
            this.miAddOleDbDataSource = new System.Windows.Forms.ToolStripMenuItem();
            this.btnDelDataSource = new System.Windows.Forms.ToolStripButton();
            this.lblInstruction = new System.Windows.Forms.Label();
            this.sep1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnManualExport = new System.Windows.Forms.ToolStripButton();
            this.tabControl.SuspendLayout();
            this.pageConnection.SuspendLayout();
            this.pageExportCurDataQuery.SuspendLayout();
            this.pageExportArcDataQuery.SuspendLayout();
            this.pageExportEventQuery.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView
            // 
            this.treeView.HideSelection = false;
            this.treeView.ImageIndex = 0;
            this.treeView.ImageList = this.ilTree;
            this.treeView.Location = new System.Drawing.Point(0, 28);
            this.treeView.Name = "treeView";
            this.treeView.SelectedImageIndex = 0;
            this.treeView.ShowRootLines = false;
            this.treeView.Size = new System.Drawing.Size(200, 392);
            this.treeView.TabIndex = 1;
            this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterSelect);
            // 
            // ilTree
            // 
            this.ilTree.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilTree.ImageStream")));
            this.ilTree.TransparentColor = System.Drawing.Color.Transparent;
            this.ilTree.Images.SetKeyName(0, "mssql.png");
            this.ilTree.Images.SetKeyName(1, "oracle.png");
            this.ilTree.Images.SetKeyName(2, "postgresql.png");
            this.ilTree.Images.SetKeyName(3, "mysql.png");
            this.ilTree.Images.SetKeyName(4, "oledb.png");
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(435, 427);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Сохранить";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(516, 427);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(597, 427);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "Закрыть";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.pageConnection);
            this.tabControl.Controls.Add(this.pageExportCurDataQuery);
            this.tabControl.Controls.Add(this.pageExportArcDataQuery);
            this.tabControl.Controls.Add(this.pageExportEventQuery);
            this.tabControl.Location = new System.Drawing.Point(200, 28);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(485, 393);
            this.tabControl.TabIndex = 3;
            // 
            // pageConnection
            // 
            this.pageConnection.Controls.Add(this.txtConnectionString);
            this.pageConnection.Controls.Add(this.lblConnectionString);
            this.pageConnection.Controls.Add(this.lblPassword);
            this.pageConnection.Controls.Add(this.txtPassword);
            this.pageConnection.Controls.Add(this.txtUser);
            this.pageConnection.Controls.Add(this.lblUser);
            this.pageConnection.Controls.Add(this.txtDatabase);
            this.pageConnection.Controls.Add(this.lblDatabase);
            this.pageConnection.Controls.Add(this.txtServer);
            this.pageConnection.Controls.Add(this.lblServer);
            this.pageConnection.Location = new System.Drawing.Point(4, 22);
            this.pageConnection.Name = "pageConnection";
            this.pageConnection.Padding = new System.Windows.Forms.Padding(3);
            this.pageConnection.Size = new System.Drawing.Size(477, 367);
            this.pageConnection.TabIndex = 3;
            this.pageConnection.Text = "Соединение";
            this.pageConnection.UseVisualStyleBackColor = true;
            // 
            // txtConnectionString
            // 
            this.txtConnectionString.Location = new System.Drawing.Point(6, 136);
            this.txtConnectionString.Multiline = true;
            this.txtConnectionString.Name = "txtConnectionString";
            this.txtConnectionString.Size = new System.Drawing.Size(465, 50);
            this.txtConnectionString.TabIndex = 9;
            this.txtConnectionString.TextChanged += new System.EventHandler(this.txtConnectionString_TextChanged);
            // 
            // lblConnectionString
            // 
            this.lblConnectionString.AutoSize = true;
            this.lblConnectionString.Location = new System.Drawing.Point(3, 120);
            this.lblConnectionString.Name = "lblConnectionString";
            this.lblConnectionString.Size = new System.Drawing.Size(106, 13);
            this.lblConnectionString.TabIndex = 8;
            this.lblConnectionString.Text = "Строка соединения";
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(239, 81);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(45, 13);
            this.lblPassword.TabIndex = 6;
            this.lblPassword.Text = "Пароль";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(242, 97);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(229, 20);
            this.txtPassword.TabIndex = 7;
            this.txtPassword.UseSystemPasswordChar = true;
            this.txtPassword.TextChanged += new System.EventHandler(this.txtPassword_TextChanged);
            // 
            // txtUser
            // 
            this.txtUser.Location = new System.Drawing.Point(6, 97);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(230, 20);
            this.txtUser.TabIndex = 5;
            this.txtUser.TextChanged += new System.EventHandler(this.txtUser_TextChanged);
            // 
            // lblUser
            // 
            this.lblUser.AutoSize = true;
            this.lblUser.Location = new System.Drawing.Point(3, 81);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(80, 13);
            this.lblUser.TabIndex = 4;
            this.lblUser.Text = "Пользователь";
            // 
            // txtDatabase
            // 
            this.txtDatabase.Location = new System.Drawing.Point(6, 58);
            this.txtDatabase.Name = "txtDatabase";
            this.txtDatabase.Size = new System.Drawing.Size(465, 20);
            this.txtDatabase.TabIndex = 3;
            this.txtDatabase.TextChanged += new System.EventHandler(this.txtDatabase_TextChanged);
            // 
            // lblDatabase
            // 
            this.lblDatabase.AutoSize = true;
            this.lblDatabase.Location = new System.Drawing.Point(3, 42);
            this.lblDatabase.Name = "lblDatabase";
            this.lblDatabase.Size = new System.Drawing.Size(72, 13);
            this.lblDatabase.TabIndex = 2;
            this.lblDatabase.Text = "База данных";
            // 
            // txtServer
            // 
            this.txtServer.Location = new System.Drawing.Point(6, 19);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(465, 20);
            this.txtServer.TabIndex = 1;
            this.txtServer.TextChanged += new System.EventHandler(this.txtServer_TextChanged);
            // 
            // lblServer
            // 
            this.lblServer.AutoSize = true;
            this.lblServer.Location = new System.Drawing.Point(3, 3);
            this.lblServer.Name = "lblServer";
            this.lblServer.Size = new System.Drawing.Size(44, 13);
            this.lblServer.TabIndex = 0;
            this.lblServer.Text = "Сервер";
            // 
            // pageExportCurDataQuery
            // 
            this.pageExportCurDataQuery.Controls.Add(this.ctrlExportCurDataQuery);
            this.pageExportCurDataQuery.Location = new System.Drawing.Point(4, 22);
            this.pageExportCurDataQuery.Name = "pageExportCurDataQuery";
            this.pageExportCurDataQuery.Padding = new System.Windows.Forms.Padding(3);
            this.pageExportCurDataQuery.Size = new System.Drawing.Size(477, 367);
            this.pageExportCurDataQuery.TabIndex = 0;
            this.pageExportCurDataQuery.Text = "Текущие данные";
            this.pageExportCurDataQuery.UseVisualStyleBackColor = true;
            // 
            // ctrlExportCurDataQuery
            // 
            this.ctrlExportCurDataQuery.Export = false;
            this.ctrlExportCurDataQuery.Location = new System.Drawing.Point(6, 6);
            this.ctrlExportCurDataQuery.Name = "ctrlExportCurDataQuery";
            this.ctrlExportCurDataQuery.Query = "";
            this.ctrlExportCurDataQuery.Size = new System.Drawing.Size(465, 355);
            this.ctrlExportCurDataQuery.TabIndex = 0;
            this.ctrlExportCurDataQuery.PropChanged += new System.EventHandler(this.ctrlExportCurDataQuery_PropChanged);
            // 
            // pageExportArcDataQuery
            // 
            this.pageExportArcDataQuery.Controls.Add(this.ctrlExportArcDataQuery);
            this.pageExportArcDataQuery.Location = new System.Drawing.Point(4, 22);
            this.pageExportArcDataQuery.Name = "pageExportArcDataQuery";
            this.pageExportArcDataQuery.Padding = new System.Windows.Forms.Padding(3);
            this.pageExportArcDataQuery.Size = new System.Drawing.Size(477, 367);
            this.pageExportArcDataQuery.TabIndex = 1;
            this.pageExportArcDataQuery.Text = "Архивные данные";
            this.pageExportArcDataQuery.UseVisualStyleBackColor = true;
            // 
            // ctrlExportArcDataQuery
            // 
            this.ctrlExportArcDataQuery.Export = false;
            this.ctrlExportArcDataQuery.Location = new System.Drawing.Point(6, 6);
            this.ctrlExportArcDataQuery.Name = "ctrlExportArcDataQuery";
            this.ctrlExportArcDataQuery.Query = "";
            this.ctrlExportArcDataQuery.Size = new System.Drawing.Size(465, 355);
            this.ctrlExportArcDataQuery.TabIndex = 1;
            this.ctrlExportArcDataQuery.PropChanged += new System.EventHandler(this.ctrlExportArcDataQuery_PropChanged);
            // 
            // pageExportEventQuery
            // 
            this.pageExportEventQuery.Controls.Add(this.ctrlExportEventQuery);
            this.pageExportEventQuery.Location = new System.Drawing.Point(4, 22);
            this.pageExportEventQuery.Name = "pageExportEventQuery";
            this.pageExportEventQuery.Padding = new System.Windows.Forms.Padding(3);
            this.pageExportEventQuery.Size = new System.Drawing.Size(477, 367);
            this.pageExportEventQuery.TabIndex = 2;
            this.pageExportEventQuery.Text = "События";
            this.pageExportEventQuery.UseVisualStyleBackColor = true;
            // 
            // ctrlExportEventQuery
            // 
            this.ctrlExportEventQuery.Export = false;
            this.ctrlExportEventQuery.Location = new System.Drawing.Point(6, 6);
            this.ctrlExportEventQuery.Name = "ctrlExportEventQuery";
            this.ctrlExportEventQuery.Query = "";
            this.ctrlExportEventQuery.Size = new System.Drawing.Size(465, 355);
            this.ctrlExportEventQuery.TabIndex = 2;
            this.ctrlExportEventQuery.PropChanged += new System.EventHandler(this.ctrlExportEventQuery_PropChanged);
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ddbAddDataSource,
            this.btnDelDataSource,
            this.sep1,
            this.btnManualExport});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(684, 25);
            this.toolStrip.TabIndex = 0;
            this.toolStrip.Text = "toolStrip1";
            // 
            // ddbAddDataSource
            // 
            this.ddbAddDataSource.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ddbAddDataSource.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miAddSqlDataSource,
            this.miAddOraDataSource,
            this.miAddPgSqlDataSource,
            this.miAddMySqlDataSource,
            this.miAddOleDbDataSource});
            this.ddbAddDataSource.Image = ((System.Drawing.Image)(resources.GetObject("ddbAddDataSource.Image")));
            this.ddbAddDataSource.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ddbAddDataSource.Name = "ddbAddDataSource";
            this.ddbAddDataSource.Size = new System.Drawing.Size(29, 22);
            this.ddbAddDataSource.ToolTipText = "Добавить источник данных";
            // 
            // miAddSqlDataSource
            // 
            this.miAddSqlDataSource.Image = ((System.Drawing.Image)(resources.GetObject("miAddSqlDataSource.Image")));
            this.miAddSqlDataSource.Name = "miAddSqlDataSource";
            this.miAddSqlDataSource.Size = new System.Drawing.Size(184, 22);
            this.miAddSqlDataSource.Text = "Microsoft SQL Server";
            this.miAddSqlDataSource.Click += new System.EventHandler(this.miAddDataSource_Click);
            // 
            // miAddOraDataSource
            // 
            this.miAddOraDataSource.Image = ((System.Drawing.Image)(resources.GetObject("miAddOraDataSource.Image")));
            this.miAddOraDataSource.Name = "miAddOraDataSource";
            this.miAddOraDataSource.Size = new System.Drawing.Size(184, 22);
            this.miAddOraDataSource.Text = "Oracle";
            this.miAddOraDataSource.Click += new System.EventHandler(this.miAddDataSource_Click);
            // 
            // miAddPgSqlDataSource
            // 
            this.miAddPgSqlDataSource.Image = ((System.Drawing.Image)(resources.GetObject("miAddPgSqlDataSource.Image")));
            this.miAddPgSqlDataSource.Name = "miAddPgSqlDataSource";
            this.miAddPgSqlDataSource.Size = new System.Drawing.Size(184, 22);
            this.miAddPgSqlDataSource.Text = "PostgreSQL";
            this.miAddPgSqlDataSource.Click += new System.EventHandler(this.miAddDataSource_Click);
            // 
            // miAddMySqlDataSource
            // 
            this.miAddMySqlDataSource.Image = ((System.Drawing.Image)(resources.GetObject("miAddMySqlDataSource.Image")));
            this.miAddMySqlDataSource.Name = "miAddMySqlDataSource";
            this.miAddMySqlDataSource.Size = new System.Drawing.Size(184, 22);
            this.miAddMySqlDataSource.Text = "MySQL";
            this.miAddMySqlDataSource.Click += new System.EventHandler(this.miAddDataSource_Click);
            // 
            // miAddOleDbDataSource
            // 
            this.miAddOleDbDataSource.Image = ((System.Drawing.Image)(resources.GetObject("miAddOleDbDataSource.Image")));
            this.miAddOleDbDataSource.Name = "miAddOleDbDataSource";
            this.miAddOleDbDataSource.Size = new System.Drawing.Size(184, 22);
            this.miAddOleDbDataSource.Text = "OLE DB";
            this.miAddOleDbDataSource.Click += new System.EventHandler(this.miAddDataSource_Click);
            // 
            // btnDelDataSource
            // 
            this.btnDelDataSource.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnDelDataSource.Image = ((System.Drawing.Image)(resources.GetObject("btnDelDataSource.Image")));
            this.btnDelDataSource.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDelDataSource.Name = "btnDelDataSource";
            this.btnDelDataSource.Size = new System.Drawing.Size(23, 22);
            this.btnDelDataSource.ToolTipText = "Удалить источник данных";
            this.btnDelDataSource.Click += new System.EventHandler(this.btnDelDataSource_Click);
            // 
            // lblInstruction
            // 
            this.lblInstruction.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblInstruction.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblInstruction.Location = new System.Drawing.Point(200, 250);
            this.lblInstruction.Name = "lblInstruction";
            this.lblInstruction.Size = new System.Drawing.Size(485, 23);
            this.lblInstruction.TabIndex = 2;
            this.lblInstruction.Text = "Добавьте источники данных";
            this.lblInstruction.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // sep1
            // 
            this.sep1.Name = "sep1";
            this.sep1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnManualExport
            // 
            this.btnManualExport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnManualExport.Image = ((System.Drawing.Image)(resources.GetObject("btnManualExport.Image")));
            this.btnManualExport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnManualExport.Name = "btnManualExport";
            this.btnManualExport.Size = new System.Drawing.Size(23, 22);
            this.btnManualExport.ToolTipText = "Экспорт в ручном режиме";
            this.btnManualExport.Click += new System.EventHandler(this.btnManualExport_Click);
            // 
            // FrmDBExportConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(684, 462);
            this.Controls.Add(this.lblInstruction);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.treeView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmDBExportConfig";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Экспорт в БД";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmDBExportConfig_FormClosing);
            this.Load += new System.EventHandler(this.FrmDBExportConfig_Load);
            this.tabControl.ResumeLayout(false);
            this.pageConnection.ResumeLayout(false);
            this.pageConnection.PerformLayout();
            this.pageExportCurDataQuery.ResumeLayout(false);
            this.pageExportArcDataQuery.ResumeLayout(false);
            this.pageExportEventQuery.ResumeLayout(false);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage pageExportCurDataQuery;
        private System.Windows.Forms.TabPage pageExportArcDataQuery;
        private System.Windows.Forms.TabPage pageExportEventQuery;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripDropDownButton ddbAddDataSource;
        private System.Windows.Forms.ToolStripMenuItem miAddSqlDataSource;
        private System.Windows.Forms.ToolStripButton btnDelDataSource;
        private System.Windows.Forms.Label lblInstruction;
        private CtrlExportQuery ctrlExportCurDataQuery;
        private CtrlExportQuery ctrlExportArcDataQuery;
        private CtrlExportQuery ctrlExportEventQuery;
        private System.Windows.Forms.ToolStripMenuItem miAddOraDataSource;
        private System.Windows.Forms.ToolStripMenuItem miAddPgSqlDataSource;
        private System.Windows.Forms.ToolStripMenuItem miAddMySqlDataSource;
        private System.Windows.Forms.ToolStripMenuItem miAddOleDbDataSource;
        private System.Windows.Forms.TabPage pageConnection;
        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.Label lblServer;
        private System.Windows.Forms.TextBox txtDatabase;
        private System.Windows.Forms.Label lblDatabase;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Label lblConnectionString;
        private System.Windows.Forms.TextBox txtConnectionString;
        private System.Windows.Forms.ImageList ilTree;
        private System.Windows.Forms.ToolStripSeparator sep1;
        private System.Windows.Forms.ToolStripButton btnManualExport;
    }
}