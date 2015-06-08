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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmDBExportConfig));
            this.treeView = new System.Windows.Forms.TreeView();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabConnection = new System.Windows.Forms.TabPage();
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
            this.tabExportCurDataQuery = new System.Windows.Forms.TabPage();
            this.ctrlExportCurDataQuery = new Scada.Server.Modules.DBExport.CtrlExportQuery();
            this.tabExportArcDataQuery = new System.Windows.Forms.TabPage();
            this.ctrlExportArcDataQuery = new Scada.Server.Modules.DBExport.CtrlExportQuery();
            this.tabExportEventQuery = new System.Windows.Forms.TabPage();
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
            this.tabControl.SuspendLayout();
            this.tabConnection.SuspendLayout();
            this.tabExportCurDataQuery.SuspendLayout();
            this.tabExportArcDataQuery.SuspendLayout();
            this.tabExportEventQuery.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView
            // 
            this.treeView.Location = new System.Drawing.Point(0, 28);
            this.treeView.Name = "treeView";
            this.treeView.Size = new System.Drawing.Size(200, 392);
            this.treeView.TabIndex = 1;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(435, 427);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Сохранить";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(516, 427);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
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
            this.tabControl.Controls.Add(this.tabConnection);
            this.tabControl.Controls.Add(this.tabExportCurDataQuery);
            this.tabControl.Controls.Add(this.tabExportArcDataQuery);
            this.tabControl.Controls.Add(this.tabExportEventQuery);
            this.tabControl.Location = new System.Drawing.Point(200, 28);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(485, 393);
            this.tabControl.TabIndex = 3;
            // 
            // tabConnection
            // 
            this.tabConnection.Controls.Add(this.txtConnectionString);
            this.tabConnection.Controls.Add(this.lblConnectionString);
            this.tabConnection.Controls.Add(this.lblPassword);
            this.tabConnection.Controls.Add(this.txtPassword);
            this.tabConnection.Controls.Add(this.txtUser);
            this.tabConnection.Controls.Add(this.lblUser);
            this.tabConnection.Controls.Add(this.txtDatabase);
            this.tabConnection.Controls.Add(this.lblDatabase);
            this.tabConnection.Controls.Add(this.txtServer);
            this.tabConnection.Controls.Add(this.lblServer);
            this.tabConnection.Location = new System.Drawing.Point(4, 22);
            this.tabConnection.Name = "tabConnection";
            this.tabConnection.Padding = new System.Windows.Forms.Padding(3);
            this.tabConnection.Size = new System.Drawing.Size(477, 367);
            this.tabConnection.TabIndex = 3;
            this.tabConnection.Text = "Соединение";
            this.tabConnection.UseVisualStyleBackColor = true;
            // 
            // txtConnectionString
            // 
            this.txtConnectionString.Location = new System.Drawing.Point(6, 136);
            this.txtConnectionString.Multiline = true;
            this.txtConnectionString.Name = "txtConnectionString";
            this.txtConnectionString.Size = new System.Drawing.Size(465, 50);
            this.txtConnectionString.TabIndex = 9;
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
            // 
            // txtUser
            // 
            this.txtUser.Location = new System.Drawing.Point(6, 97);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(230, 20);
            this.txtUser.TabIndex = 5;
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
            // tabExportCurDataQuery
            // 
            this.tabExportCurDataQuery.Controls.Add(this.ctrlExportCurDataQuery);
            this.tabExportCurDataQuery.Location = new System.Drawing.Point(4, 22);
            this.tabExportCurDataQuery.Name = "tabExportCurDataQuery";
            this.tabExportCurDataQuery.Padding = new System.Windows.Forms.Padding(3);
            this.tabExportCurDataQuery.Size = new System.Drawing.Size(477, 367);
            this.tabExportCurDataQuery.TabIndex = 0;
            this.tabExportCurDataQuery.Text = "Текущие данные";
            this.tabExportCurDataQuery.UseVisualStyleBackColor = true;
            // 
            // ctrlExportCurDataQuery
            // 
            this.ctrlExportCurDataQuery.Example = "";
            this.ctrlExportCurDataQuery.Export = false;
            this.ctrlExportCurDataQuery.Location = new System.Drawing.Point(6, 6);
            this.ctrlExportCurDataQuery.Name = "ctrlExportCurDataQuery";
            this.ctrlExportCurDataQuery.Query = "";
            this.ctrlExportCurDataQuery.Size = new System.Drawing.Size(465, 355);
            this.ctrlExportCurDataQuery.TabIndex = 0;
            // 
            // tabExportArcDataQuery
            // 
            this.tabExportArcDataQuery.Controls.Add(this.ctrlExportArcDataQuery);
            this.tabExportArcDataQuery.Location = new System.Drawing.Point(4, 22);
            this.tabExportArcDataQuery.Name = "tabExportArcDataQuery";
            this.tabExportArcDataQuery.Padding = new System.Windows.Forms.Padding(3);
            this.tabExportArcDataQuery.Size = new System.Drawing.Size(477, 367);
            this.tabExportArcDataQuery.TabIndex = 1;
            this.tabExportArcDataQuery.Text = "Архивные данные";
            this.tabExportArcDataQuery.UseVisualStyleBackColor = true;
            // 
            // ctrlExportArcDataQuery
            // 
            this.ctrlExportArcDataQuery.Example = "";
            this.ctrlExportArcDataQuery.Export = false;
            this.ctrlExportArcDataQuery.Location = new System.Drawing.Point(6, 6);
            this.ctrlExportArcDataQuery.Name = "ctrlExportArcDataQuery";
            this.ctrlExportArcDataQuery.Query = "";
            this.ctrlExportArcDataQuery.Size = new System.Drawing.Size(465, 355);
            this.ctrlExportArcDataQuery.TabIndex = 1;
            // 
            // tabExportEventQuery
            // 
            this.tabExportEventQuery.Controls.Add(this.ctrlExportEventQuery);
            this.tabExportEventQuery.Location = new System.Drawing.Point(4, 22);
            this.tabExportEventQuery.Name = "tabExportEventQuery";
            this.tabExportEventQuery.Padding = new System.Windows.Forms.Padding(3);
            this.tabExportEventQuery.Size = new System.Drawing.Size(477, 367);
            this.tabExportEventQuery.TabIndex = 2;
            this.tabExportEventQuery.Text = "События";
            this.tabExportEventQuery.UseVisualStyleBackColor = true;
            // 
            // ctrlExportEventQuery
            // 
            this.ctrlExportEventQuery.Example = "";
            this.ctrlExportEventQuery.Export = false;
            this.ctrlExportEventQuery.Location = new System.Drawing.Point(6, 6);
            this.ctrlExportEventQuery.Name = "ctrlExportEventQuery";
            this.ctrlExportEventQuery.Query = "";
            this.ctrlExportEventQuery.Size = new System.Drawing.Size(465, 355);
            this.ctrlExportEventQuery.TabIndex = 2;
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ddbAddDataSource,
            this.btnDelDataSource});
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
            // 
            // miAddOraDataSource
            // 
            this.miAddOraDataSource.Image = ((System.Drawing.Image)(resources.GetObject("miAddOraDataSource.Image")));
            this.miAddOraDataSource.Name = "miAddOraDataSource";
            this.miAddOraDataSource.Size = new System.Drawing.Size(184, 22);
            this.miAddOraDataSource.Text = "Oracle";
            // 
            // miAddPgSqlDataSource
            // 
            this.miAddPgSqlDataSource.Image = ((System.Drawing.Image)(resources.GetObject("miAddPgSqlDataSource.Image")));
            this.miAddPgSqlDataSource.Name = "miAddPgSqlDataSource";
            this.miAddPgSqlDataSource.Size = new System.Drawing.Size(184, 22);
            this.miAddPgSqlDataSource.Text = "PostgreSQL";
            // 
            // miAddMySqlDataSource
            // 
            this.miAddMySqlDataSource.Image = ((System.Drawing.Image)(resources.GetObject("miAddMySqlDataSource.Image")));
            this.miAddMySqlDataSource.Name = "miAddMySqlDataSource";
            this.miAddMySqlDataSource.Size = new System.Drawing.Size(184, 22);
            this.miAddMySqlDataSource.Text = "MySQL";
            // 
            // miAddOleDbDataSource
            // 
            this.miAddOleDbDataSource.Image = ((System.Drawing.Image)(resources.GetObject("miAddOleDbDataSource.Image")));
            this.miAddOleDbDataSource.Name = "miAddOleDbDataSource";
            this.miAddOleDbDataSource.Size = new System.Drawing.Size(184, 22);
            this.miAddOleDbDataSource.Text = "OLE DB";
            // 
            // btnDelDataSource
            // 
            this.btnDelDataSource.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnDelDataSource.Image = ((System.Drawing.Image)(resources.GetObject("btnDelDataSource.Image")));
            this.btnDelDataSource.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDelDataSource.Name = "btnDelDataSource";
            this.btnDelDataSource.Size = new System.Drawing.Size(23, 22);
            this.btnDelDataSource.ToolTipText = "Удалить источник данных";
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
            this.Load += new System.EventHandler(this.FrmDBExportConfig_Load);
            this.tabControl.ResumeLayout(false);
            this.tabConnection.ResumeLayout(false);
            this.tabConnection.PerformLayout();
            this.tabExportCurDataQuery.ResumeLayout(false);
            this.tabExportArcDataQuery.ResumeLayout(false);
            this.tabExportEventQuery.ResumeLayout(false);
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
        private System.Windows.Forms.TabPage tabExportCurDataQuery;
        private System.Windows.Forms.TabPage tabExportArcDataQuery;
        private System.Windows.Forms.TabPage tabExportEventQuery;
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
        private System.Windows.Forms.TabPage tabConnection;
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
    }
}