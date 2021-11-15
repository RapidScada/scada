
namespace Scada.Server.Modules.DbExport.UI
{
    partial class FrmDbExportConfig
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmDbExportConfig));
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Element groups");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Triggers");
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.tlStBottom = new System.Windows.Forms.ToolStrip();
            this.ddbAdd = new System.Windows.Forms.ToolStripDropDownButton();
            this.btnSqlServer = new System.Windows.Forms.ToolStripMenuItem();
            this.btnOracle = new System.Windows.Forms.ToolStripMenuItem();
            this.btnPostgreSql = new System.Windows.Forms.ToolStripMenuItem();
            this.btnMySql = new System.Windows.Forms.ToolStripMenuItem();
            this.btnOleDb = new System.Windows.Forms.ToolStripMenuItem();
            this.btnAddCurTrigger = new System.Windows.Forms.ToolStripButton();
            this.btnAddArcTrigger = new System.Windows.Forms.ToolStripButton();
            this.btnAddEventTrigger = new System.Windows.Forms.ToolStripButton();
            this.btnDelete = new System.Windows.Forms.ToolStripButton();
            this.btnMoveUp = new System.Windows.Forms.ToolStripButton();
            this.btnMoveDown = new System.Windows.Forms.ToolStripButton();
            this.pnlGeneral = new System.Windows.Forms.Panel();
            this.ctrlConnectionOptions = new Scada.Server.Modules.DbExport.UI.CtrlConnectionOptions();
            this.ctrlGeneralOptions = new Scada.Server.Modules.DbExport.UI.CtrlGeneralOptions();
            this.gbTarget = new System.Windows.Forms.GroupBox();
            this.tvTargets = new System.Windows.Forms.TreeView();
            this.cmsTree = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miExpandAll = new System.Windows.Forms.ToolStripMenuItem();
            this.miCollapseAll = new System.Windows.Forms.ToolStripMenuItem();
            this.ilTree = new System.Windows.Forms.ImageList(this.components);
            this.ctrlEventTrigger = new Scada.Server.Modules.DbExport.UI.CtrlEventTrigger();
            this.ctrlTrigger = new Scada.Server.Modules.DbExport.UI.CtrlTrigger();
            this.ctrlArcUploadOptions = new Scada.Server.Modules.DbExport.UI.CtrlArcUploadOptions();
            this.pnlBottom.SuspendLayout();
            this.tlStBottom.SuspendLayout();
            this.pnlGeneral.SuspendLayout();
            this.gbTarget.SuspendLayout();
            this.cmsTree.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.btnCancel);
            this.pnlBottom.Controls.Add(this.btnSave);
            this.pnlBottom.Controls.Add(this.btnClose);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 518);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(714, 41);
            this.pnlBottom.TabIndex = 2;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Enabled = false;
            this.btnCancel.Location = new System.Drawing.Point(546, 6);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(465, 6);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(627, 6);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // tlStBottom
            // 
            this.tlStBottom.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ddbAdd,
            this.btnAddCurTrigger,
            this.btnAddArcTrigger,
            this.btnAddEventTrigger,
            this.btnDelete,
            this.btnMoveUp,
            this.btnMoveDown});
            this.tlStBottom.Location = new System.Drawing.Point(0, 0);
            this.tlStBottom.Name = "tlStBottom";
            this.tlStBottom.Size = new System.Drawing.Size(714, 25);
            this.tlStBottom.TabIndex = 0;
            this.tlStBottom.Text = " ";
            // 
            // ddbAdd
            // 
            this.ddbAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ddbAdd.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnSqlServer,
            this.btnOracle,
            this.btnPostgreSql,
            this.btnMySql,
            this.btnOleDb});
            this.ddbAdd.Image = ((System.Drawing.Image)(resources.GetObject("ddbAdd.Image")));
            this.ddbAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ddbAdd.Name = "ddbAdd";
            this.ddbAdd.Size = new System.Drawing.Size(29, 22);
            this.ddbAdd.Text = "Add";
            this.ddbAdd.ToolTipText = "Add export target";
            // 
            // btnSqlServer
            // 
            this.btnSqlServer.Image = ((System.Drawing.Image)(resources.GetObject("btnSqlServer.Image")));
            this.btnSqlServer.Name = "btnSqlServer";
            this.btnSqlServer.Size = new System.Drawing.Size(184, 22);
            this.btnSqlServer.Text = "Microsoft SQL Server";
            this.btnSqlServer.Click += new System.EventHandler(this.btnAddTagret_Click);
            // 
            // btnOracle
            // 
            this.btnOracle.Image = ((System.Drawing.Image)(resources.GetObject("btnOracle.Image")));
            this.btnOracle.Name = "btnOracle";
            this.btnOracle.Size = new System.Drawing.Size(184, 22);
            this.btnOracle.Text = "Oracle";
            this.btnOracle.Click += new System.EventHandler(this.btnAddTagret_Click);
            // 
            // btnPostgreSql
            // 
            this.btnPostgreSql.Image = ((System.Drawing.Image)(resources.GetObject("btnPostgreSql.Image")));
            this.btnPostgreSql.Name = "btnPostgreSql";
            this.btnPostgreSql.Size = new System.Drawing.Size(184, 22);
            this.btnPostgreSql.Text = "PostgreSQL";
            this.btnPostgreSql.Click += new System.EventHandler(this.btnAddTagret_Click);
            // 
            // btnMySql
            // 
            this.btnMySql.Image = ((System.Drawing.Image)(resources.GetObject("btnMySql.Image")));
            this.btnMySql.Name = "btnMySql";
            this.btnMySql.Size = new System.Drawing.Size(184, 22);
            this.btnMySql.Text = "MySQL";
            this.btnMySql.Click += new System.EventHandler(this.btnAddTagret_Click);
            // 
            // btnOleDb
            // 
            this.btnOleDb.Image = ((System.Drawing.Image)(resources.GetObject("btnOleDb.Image")));
            this.btnOleDb.Name = "btnOleDb";
            this.btnOleDb.Size = new System.Drawing.Size(184, 22);
            this.btnOleDb.Text = "OLE DB";
            this.btnOleDb.Click += new System.EventHandler(this.btnAddTagret_Click);
            // 
            // btnAddCurTrigger
            // 
            this.btnAddCurTrigger.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnAddCurTrigger.Image = ((System.Drawing.Image)(resources.GetObject("btnAddCurTrigger.Image")));
            this.btnAddCurTrigger.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddCurTrigger.Name = "btnAddCurTrigger";
            this.btnAddCurTrigger.Size = new System.Drawing.Size(23, 22);
            this.btnAddCurTrigger.Text = "Add current data trigger";
            this.btnAddCurTrigger.ToolTipText = "Add current data trigger";
            this.btnAddCurTrigger.Click += new System.EventHandler(this.btnAddTrigger_Click);
            // 
            // btnAddArcTrigger
            // 
            this.btnAddArcTrigger.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnAddArcTrigger.Image = ((System.Drawing.Image)(resources.GetObject("btnAddArcTrigger.Image")));
            this.btnAddArcTrigger.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddArcTrigger.Name = "btnAddArcTrigger";
            this.btnAddArcTrigger.Size = new System.Drawing.Size(23, 22);
            this.btnAddArcTrigger.Text = "tlStBtnAddArcTrigger";
            this.btnAddArcTrigger.ToolTipText = "Add archive data trigger";
            this.btnAddArcTrigger.Click += new System.EventHandler(this.btnAddTrigger_Click);
            // 
            // btnAddEventTrigger
            // 
            this.btnAddEventTrigger.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnAddEventTrigger.Image = ((System.Drawing.Image)(resources.GetObject("btnAddEventTrigger.Image")));
            this.btnAddEventTrigger.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddEventTrigger.Name = "btnAddEventTrigger";
            this.btnAddEventTrigger.Size = new System.Drawing.Size(23, 22);
            this.btnAddEventTrigger.Text = "tlStBtnAddEventTrigger";
            this.btnAddEventTrigger.ToolTipText = "Add event trigger";
            this.btnAddEventTrigger.Click += new System.EventHandler(this.btnAddTrigger_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnDelete.Image = ((System.Drawing.Image)(resources.GetObject("btnDelete.Image")));
            this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(23, 22);
            this.btnDelete.Text = "Delete";
            this.btnDelete.ToolTipText = "Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnMoveUp
            // 
            this.btnMoveUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnMoveUp.Image = ((System.Drawing.Image)(resources.GetObject("btnMoveUp.Image")));
            this.btnMoveUp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnMoveUp.Name = "btnMoveUp";
            this.btnMoveUp.Size = new System.Drawing.Size(23, 22);
            this.btnMoveUp.Text = "Move up";
            this.btnMoveUp.ToolTipText = "Move up";
            this.btnMoveUp.Click += new System.EventHandler(this.btnMoveUpDown_Click);
            // 
            // btnMoveDown
            // 
            this.btnMoveDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnMoveDown.Image = ((System.Drawing.Image)(resources.GetObject("btnMoveDown.Image")));
            this.btnMoveDown.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnMoveDown.Name = "btnMoveDown";
            this.btnMoveDown.Size = new System.Drawing.Size(23, 22);
            this.btnMoveDown.Text = "Move down";
            this.btnMoveDown.ToolTipText = "Move down";
            this.btnMoveDown.Click += new System.EventHandler(this.btnMoveUpDown_Click);
            // 
            // pnlGeneral
            // 
            this.pnlGeneral.Controls.Add(this.ctrlConnectionOptions);
            this.pnlGeneral.Controls.Add(this.ctrlGeneralOptions);
            this.pnlGeneral.Controls.Add(this.gbTarget);
            this.pnlGeneral.Controls.Add(this.ctrlEventTrigger);
            this.pnlGeneral.Controls.Add(this.ctrlTrigger);
            this.pnlGeneral.Controls.Add(this.ctrlArcUploadOptions);
            this.pnlGeneral.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGeneral.Location = new System.Drawing.Point(0, 25);
            this.pnlGeneral.Name = "pnlGeneral";
            this.pnlGeneral.Size = new System.Drawing.Size(714, 493);
            this.pnlGeneral.TabIndex = 1;
            // 
            // ctrlConnectionOptions
            // 
            this.ctrlConnectionOptions.ConnectionOptions = null;
            this.ctrlConnectionOptions.Location = new System.Drawing.Point(288, 5);
            this.ctrlConnectionOptions.Name = "ctrlConnectionOptions";
            this.ctrlConnectionOptions.Size = new System.Drawing.Size(412, 481);
            this.ctrlConnectionOptions.TabIndex = 1;
            this.ctrlConnectionOptions.ConnectChanged += new Scada.UI.ObjectChangedEventHandler(this.ctrlConnectionOptions_ConnectChanged);
            // 
            // ctrlGeneralOptions
            // 
            this.ctrlGeneralOptions.Location = new System.Drawing.Point(288, 5);
            this.ctrlGeneralOptions.Name = "ctrlGeneralOptions";
            this.ctrlGeneralOptions.Size = new System.Drawing.Size(412, 481);
            this.ctrlGeneralOptions.TabIndex = 6;
            this.ctrlGeneralOptions.GeneralOptionsChanged += new Scada.UI.ObjectChangedEventHandler(this.ctrlGeneralOptions_GeneralOptionsChanged);
            // 
            // gbTarget
            // 
            this.gbTarget.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.gbTarget.Controls.Add(this.tvTargets);
            this.gbTarget.Location = new System.Drawing.Point(6, 5);
            this.gbTarget.Name = "gbTarget";
            this.gbTarget.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbTarget.Size = new System.Drawing.Size(276, 481);
            this.gbTarget.TabIndex = 0;
            this.gbTarget.TabStop = false;
            this.gbTarget.Text = "Export Targets";
            // 
            // tvTargets
            // 
            this.tvTargets.ContextMenuStrip = this.cmsTree;
            this.tvTargets.HideSelection = false;
            this.tvTargets.ImageIndex = 0;
            this.tvTargets.ImageList = this.ilTree;
            this.tvTargets.Location = new System.Drawing.Point(13, 19);
            this.tvTargets.Name = "tvTargets";
            treeNode1.ImageKey = "group.png";
            treeNode1.Name = "grsNode";
            treeNode1.SelectedImageKey = "group.png";
            treeNode1.Text = "Element groups";
            treeNode2.ImageIndex = 2;
            treeNode2.Name = "triggersNode";
            treeNode2.SelectedImageKey = "cmds.png";
            treeNode2.Text = "Triggers";
            this.tvTargets.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2});
            this.tvTargets.SelectedImageIndex = 0;
            this.tvTargets.Size = new System.Drawing.Size(250, 449);
            this.tvTargets.TabIndex = 0;
            this.tvTargets.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.tvTargets_AfterCollapse);
            this.tvTargets.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.tvTargets_AfterExpand);
            this.tvTargets.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvTargets_AfterSelect);
            // 
            // cmsTree
            // 
            this.cmsTree.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miExpandAll,
            this.miCollapseAll});
            this.cmsTree.Name = "cmsTree";
            this.cmsTree.Size = new System.Drawing.Size(137, 48);
            // 
            // miExpandAll
            // 
            this.miExpandAll.Name = "miExpandAll";
            this.miExpandAll.Size = new System.Drawing.Size(136, 22);
            this.miExpandAll.Text = "Expand All";
            this.miExpandAll.Click += new System.EventHandler(this.miExpandAll_Click);
            // 
            // miCollapseAll
            // 
            this.miCollapseAll.Name = "miCollapseAll";
            this.miCollapseAll.Size = new System.Drawing.Size(136, 22);
            this.miCollapseAll.Text = "Collapse All";
            this.miCollapseAll.Click += new System.EventHandler(this.miCollapseAll_Click);
            // 
            // ilTree
            // 
            this.ilTree.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilTree.ImageStream")));
            this.ilTree.TransparentColor = System.Drawing.Color.Transparent;
            this.ilTree.Images.SetKeyName(0, "arc_upload.png");
            this.ilTree.Images.SetKeyName(1, "connect.png");
            this.ilTree.Images.SetKeyName(2, "db_mssql.png");
            this.ilTree.Images.SetKeyName(3, "db_mysql.png");
            this.ilTree.Images.SetKeyName(4, "db_oledb.png");
            this.ilTree.Images.SetKeyName(5, "db_oracle.png");
            this.ilTree.Images.SetKeyName(6, "db_postgresql.png");
            this.ilTree.Images.SetKeyName(7, "tr_arc_data.png");
            this.ilTree.Images.SetKeyName(8, "tr_cur_data.png");
            this.ilTree.Images.SetKeyName(9, "tr_data_inactive.png");
            this.ilTree.Images.SetKeyName(10, "tr_event.png");
            this.ilTree.Images.SetKeyName(11, "tr_event_inactive.png");
            this.ilTree.Images.SetKeyName(12, "db_mssql_inactive.png");
            this.ilTree.Images.SetKeyName(13, "db_mysql_inactive.png");
            this.ilTree.Images.SetKeyName(14, "db_oledb_inactive.png");
            this.ilTree.Images.SetKeyName(15, "db_oracle_inactive.png");
            this.ilTree.Images.SetKeyName(16, "db_postgresql_inactive.png");
            this.ilTree.Images.SetKeyName(17, "folder_closed.png");
            this.ilTree.Images.SetKeyName(18, "folder_open.png");
            // 
            // ctrlEventTrigger
            // 
            this.ctrlEventTrigger.Location = new System.Drawing.Point(288, 5);
            this.ctrlEventTrigger.Name = "ctrlEventTrigger";
            this.ctrlEventTrigger.Size = new System.Drawing.Size(412, 481);
            this.ctrlEventTrigger.TabIndex = 24;
            this.ctrlEventTrigger.TriggerEventOptionsChanged += new Scada.UI.ObjectChangedEventHandler(this.ctrlEventTriggers_TriggerEventOptionsChanged);
            // 
            // ctrlTrigger
            // 
            this.ctrlTrigger.Location = new System.Drawing.Point(288, 5);
            this.ctrlTrigger.Name = "ctrlTrigger";
            this.ctrlTrigger.Size = new System.Drawing.Size(412, 481);
            this.ctrlTrigger.TabIndex = 23;
            this.ctrlTrigger.TriggerOptionsChanged += new Scada.UI.ObjectChangedEventHandler(this.ctrlTriggers_TriggerOptionsChanged);
            // 
            // ctrlArcUploadOptions
            // 
            this.ctrlArcUploadOptions.Location = new System.Drawing.Point(288, 5);
            this.ctrlArcUploadOptions.Name = "ctrlArcUploadOptions";
            this.ctrlArcUploadOptions.Size = new System.Drawing.Size(412, 481);
            this.ctrlArcUploadOptions.TabIndex = 5;
            this.ctrlArcUploadOptions.ArcUploadOptionsChanged += new Scada.UI.ObjectChangedEventHandler(this.ctrlArcUploadOptions_ArcUploadOptionsChanged);
            // 
            // FrmDbExportConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(714, 559);
            this.Controls.Add(this.pnlGeneral);
            this.Controls.Add(this.tlStBottom);
            this.Controls.Add(this.pnlBottom);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmDbExportConfig";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Export to DB";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmConfig_FormClosing);
            this.Load += new System.EventHandler(this.FrmConfig_Load);
            this.pnlBottom.ResumeLayout(false);
            this.tlStBottom.ResumeLayout(false);
            this.tlStBottom.PerformLayout();
            this.pnlGeneral.ResumeLayout(false);
            this.gbTarget.ResumeLayout(false);
            this.cmsTree.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ToolStrip tlStBottom;
        private System.Windows.Forms.ToolStripButton btnDelete;
        private System.Windows.Forms.ToolStripButton btnMoveUp;
        private System.Windows.Forms.ToolStripButton btnMoveDown;
        private System.Windows.Forms.Panel pnlGeneral;
        private System.Windows.Forms.GroupBox gbTarget;
        private System.Windows.Forms.TreeView tvTargets;
        private CtrlGeneralOptions ctrlGeneralOptions;
        private CtrlConnectionOptions ctrlConnectionOptions;
        private CtrlTrigger ctrlTrigger;
        private CtrlArcUploadOptions ctrlArcUploadOptions;
        private System.Windows.Forms.ContextMenuStrip cmsTree;
        private System.Windows.Forms.ToolStripMenuItem miExpandAll;
        private System.Windows.Forms.ToolStripMenuItem miCollapseAll;
        private CtrlEventTrigger ctrlEventTrigger;
        private System.Windows.Forms.ToolStripButton btnAddCurTrigger;
        private System.Windows.Forms.ToolStripButton btnAddArcTrigger;
        private System.Windows.Forms.ToolStripButton btnAddEventTrigger;
        private System.Windows.Forms.ImageList ilTree;
        private System.Windows.Forms.ToolStripDropDownButton ddbAdd;
        private System.Windows.Forms.ToolStripMenuItem btnSqlServer;
        private System.Windows.Forms.ToolStripMenuItem btnOracle;
        private System.Windows.Forms.ToolStripMenuItem btnPostgreSql;
        private System.Windows.Forms.ToolStripMenuItem btnMySql;
        private System.Windows.Forms.ToolStripMenuItem btnOleDb;
    }
}