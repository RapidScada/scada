namespace Scada.Server.Shell.Forms
{
    partial class FrmSnapshotTable
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSnapshotTable));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.colCnlNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStat = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bindingSource2 = new System.Windows.Forms.BindingSource(this.components);
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.pnlRight = new System.Windows.Forms.Panel();
            this.bindingNavigator2 = new System.Windows.Forms.BindingNavigator(this.components);
            this.lblCount2 = new System.Windows.Forms.ToolStripLabel();
            this.btnMoveFirst2 = new System.Windows.Forms.ToolStripButton();
            this.btnMovePrev2 = new System.Windows.Forms.ToolStripButton();
            this.sep4 = new System.Windows.Forms.ToolStripSeparator();
            this.txtPosition2 = new System.Windows.Forms.ToolStripTextBox();
            this.sep5 = new System.Windows.Forms.ToolStripSeparator();
            this.btnMoveNext2 = new System.Windows.Forms.ToolStripButton();
            this.btnMoveLast2 = new System.Windows.Forms.ToolStripButton();
            this.sep6 = new System.Windows.Forms.ToolStripSeparator();
            this.lblFilter = new System.Windows.Forms.ToolStripLabel();
            this.txtFilter = new System.Windows.Forms.ToolStripTextBox();
            this.pnlLeft = new System.Windows.Forms.Panel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bindingNavigator1 = new System.Windows.Forms.BindingNavigator(this.components);
            this.lblCount1 = new System.Windows.Forms.ToolStripLabel();
            this.btnMoveFirst1 = new System.Windows.Forms.ToolStripButton();
            this.btnMovePrev1 = new System.Windows.Forms.ToolStripButton();
            this.sep1 = new System.Windows.Forms.ToolStripSeparator();
            this.txtPosition1 = new System.Windows.Forms.ToolStripTextBox();
            this.sep2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnMoveNext1 = new System.Windows.Forms.ToolStripButton();
            this.btnMoveLast1 = new System.Windows.Forms.ToolStripButton();
            this.sep3 = new System.Windows.Forms.ToolStripSeparator();
            this.btnRefresh = new System.Windows.Forms.ToolStripButton();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.splMain = new System.Windows.Forms.Splitter();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.btnExportToCsv = new System.Windows.Forms.Button();
            this.sfdCsv = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource2)).BeginInit();
            this.pnlRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator2)).BeginInit();
            this.bindingNavigator2.SuspendLayout();
            this.pnlLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).BeginInit();
            this.bindingNavigator1.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView2
            // 
            this.dataGridView2.AllowUserToAddRows = false;
            this.dataGridView2.AllowUserToDeleteRows = false;
            this.dataGridView2.AllowUserToOrderColumns = true;
            this.dataGridView2.AutoGenerateColumns = false;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colCnlNum,
            this.colVal,
            this.colStat});
            this.dataGridView2.DataSource = this.bindingSource2;
            this.dataGridView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView2.Location = new System.Drawing.Point(0, 25);
            this.dataGridView2.Margin = new System.Windows.Forms.Padding(0);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.Size = new System.Drawing.Size(491, 446);
            this.dataGridView2.TabIndex = 1;
            this.dataGridView2.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dataGridView2_CellValidating);
            // 
            // colCnlNum
            // 
            this.colCnlNum.DataPropertyName = "CnlNum";
            this.colCnlNum.HeaderText = "CnlNum";
            this.colCnlNum.Name = "colCnlNum";
            this.colCnlNum.ReadOnly = true;
            // 
            // colVal
            // 
            this.colVal.DataPropertyName = "Val";
            this.colVal.HeaderText = "Val";
            this.colVal.Name = "colVal";
            // 
            // colStat
            // 
            this.colStat.DataPropertyName = "Stat";
            this.colStat.HeaderText = "Stat";
            this.colStat.Name = "colStat";
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(677, 6);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(596, 6);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // pnlRight
            // 
            this.pnlRight.Controls.Add(this.dataGridView2);
            this.pnlRight.Controls.Add(this.bindingNavigator2);
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRight.Location = new System.Drawing.Point(273, 0);
            this.pnlRight.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Size = new System.Drawing.Size(491, 471);
            this.pnlRight.TabIndex = 2;
            // 
            // bindingNavigator2
            // 
            this.bindingNavigator2.AddNewItem = null;
            this.bindingNavigator2.BindingSource = this.bindingSource2;
            this.bindingNavigator2.CountItem = this.lblCount2;
            this.bindingNavigator2.DeleteItem = null;
            this.bindingNavigator2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.bindingNavigator2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnMoveFirst2,
            this.btnMovePrev2,
            this.sep4,
            this.txtPosition2,
            this.lblCount2,
            this.sep5,
            this.btnMoveNext2,
            this.btnMoveLast2,
            this.sep6,
            this.lblFilter,
            this.txtFilter});
            this.bindingNavigator2.Location = new System.Drawing.Point(0, 0);
            this.bindingNavigator2.MoveFirstItem = this.btnMoveFirst2;
            this.bindingNavigator2.MoveLastItem = this.btnMoveLast2;
            this.bindingNavigator2.MoveNextItem = this.btnMoveNext2;
            this.bindingNavigator2.MovePreviousItem = this.btnMovePrev2;
            this.bindingNavigator2.Name = "bindingNavigator2";
            this.bindingNavigator2.PositionItem = this.txtPosition2;
            this.bindingNavigator2.Size = new System.Drawing.Size(491, 25);
            this.bindingNavigator2.TabIndex = 0;
            this.bindingNavigator2.Text = "bindingNavigator2";
            // 
            // lblCount2
            // 
            this.lblCount2.Name = "lblCount2";
            this.lblCount2.Size = new System.Drawing.Size(35, 22);
            this.lblCount2.Text = "of {0}";
            this.lblCount2.ToolTipText = "Total number of rows";
            // 
            // btnMoveFirst2
            // 
            this.btnMoveFirst2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnMoveFirst2.Image = ((System.Drawing.Image)(resources.GetObject("btnMoveFirst2.Image")));
            this.btnMoveFirst2.Name = "btnMoveFirst2";
            this.btnMoveFirst2.RightToLeftAutoMirrorImage = true;
            this.btnMoveFirst2.Size = new System.Drawing.Size(23, 22);
            this.btnMoveFirst2.Text = "Move first";
            // 
            // btnMovePrev2
            // 
            this.btnMovePrev2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnMovePrev2.Image = ((System.Drawing.Image)(resources.GetObject("btnMovePrev2.Image")));
            this.btnMovePrev2.Name = "btnMovePrev2";
            this.btnMovePrev2.RightToLeftAutoMirrorImage = true;
            this.btnMovePrev2.Size = new System.Drawing.Size(23, 22);
            this.btnMovePrev2.Text = "Move previous";
            // 
            // sep4
            // 
            this.sep4.Name = "sep4";
            this.sep4.Size = new System.Drawing.Size(6, 25);
            // 
            // txtPosition2
            // 
            this.txtPosition2.AccessibleName = "Position";
            this.txtPosition2.AutoSize = false;
            this.txtPosition2.Name = "txtPosition2";
            this.txtPosition2.Size = new System.Drawing.Size(50, 23);
            this.txtPosition2.Text = "0";
            this.txtPosition2.ToolTipText = "Current position";
            // 
            // sep5
            // 
            this.sep5.Name = "sep5";
            this.sep5.Size = new System.Drawing.Size(6, 25);
            // 
            // btnMoveNext2
            // 
            this.btnMoveNext2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnMoveNext2.Image = ((System.Drawing.Image)(resources.GetObject("btnMoveNext2.Image")));
            this.btnMoveNext2.Name = "btnMoveNext2";
            this.btnMoveNext2.RightToLeftAutoMirrorImage = true;
            this.btnMoveNext2.Size = new System.Drawing.Size(23, 22);
            this.btnMoveNext2.Text = "Move next";
            // 
            // btnMoveLast2
            // 
            this.btnMoveLast2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnMoveLast2.Image = ((System.Drawing.Image)(resources.GetObject("btnMoveLast2.Image")));
            this.btnMoveLast2.Name = "btnMoveLast2";
            this.btnMoveLast2.RightToLeftAutoMirrorImage = true;
            this.btnMoveLast2.Size = new System.Drawing.Size(23, 22);
            this.btnMoveLast2.Text = "Move last";
            // 
            // sep6
            // 
            this.sep6.Name = "sep6";
            this.sep6.Size = new System.Drawing.Size(6, 25);
            // 
            // lblFilter
            // 
            this.lblFilter.Name = "lblFilter";
            this.lblFilter.Size = new System.Drawing.Size(58, 22);
            this.lblFilter.Text = "SQL filter:";
            // 
            // txtFilter
            // 
            this.txtFilter.Name = "txtFilter";
            this.txtFilter.Size = new System.Drawing.Size(180, 25);
            this.txtFilter.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtFilter_KeyDown);
            // 
            // pnlLeft
            // 
            this.pnlLeft.Controls.Add(this.dataGridView1);
            this.pnlLeft.Controls.Add(this.bindingNavigator1);
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlLeft.Location = new System.Drawing.Point(0, 0);
            this.pnlLeft.Margin = new System.Windows.Forms.Padding(0);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Size = new System.Drawing.Size(270, 471);
            this.pnlLeft.TabIndex = 0;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToOrderColumns = true;
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1});
            this.dataGridView1.DataSource = this.bindingSource1;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 25);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(0);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(270, 446);
            this.dataGridView1.TabIndex = 1;
            this.dataGridView1.CurrentCellChanged += new System.EventHandler(this.dataGridView1_CurrentCellChanged);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn1.DataPropertyName = "DateTime";
            dataGridViewCellStyle1.Format = "G";
            dataGridViewCellStyle1.NullValue = null;
            this.dataGridViewTextBoxColumn1.DefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewTextBoxColumn1.HeaderText = "DateTime";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // bindingNavigator1
            // 
            this.bindingNavigator1.AddNewItem = null;
            this.bindingNavigator1.BindingSource = this.bindingSource1;
            this.bindingNavigator1.CountItem = this.lblCount1;
            this.bindingNavigator1.DeleteItem = null;
            this.bindingNavigator1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnMoveFirst1,
            this.btnMovePrev1,
            this.sep1,
            this.txtPosition1,
            this.lblCount1,
            this.sep2,
            this.btnMoveNext1,
            this.btnMoveLast1,
            this.sep3,
            this.btnRefresh});
            this.bindingNavigator1.Location = new System.Drawing.Point(0, 0);
            this.bindingNavigator1.MoveFirstItem = this.btnMoveFirst1;
            this.bindingNavigator1.MoveLastItem = this.btnMoveLast1;
            this.bindingNavigator1.MoveNextItem = this.btnMoveNext1;
            this.bindingNavigator1.MovePreviousItem = this.btnMovePrev1;
            this.bindingNavigator1.Name = "bindingNavigator1";
            this.bindingNavigator1.PositionItem = this.txtPosition1;
            this.bindingNavigator1.Size = new System.Drawing.Size(270, 25);
            this.bindingNavigator1.TabIndex = 0;
            this.bindingNavigator1.Text = "bindingNavigator1";
            // 
            // lblCount1
            // 
            this.lblCount1.Name = "lblCount1";
            this.lblCount1.Size = new System.Drawing.Size(35, 22);
            this.lblCount1.Text = "of {0}";
            this.lblCount1.ToolTipText = "Total number of rows";
            // 
            // btnMoveFirst1
            // 
            this.btnMoveFirst1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnMoveFirst1.Image = ((System.Drawing.Image)(resources.GetObject("btnMoveFirst1.Image")));
            this.btnMoveFirst1.Name = "btnMoveFirst1";
            this.btnMoveFirst1.RightToLeftAutoMirrorImage = true;
            this.btnMoveFirst1.Size = new System.Drawing.Size(23, 22);
            this.btnMoveFirst1.Text = "Move first";
            // 
            // btnMovePrev1
            // 
            this.btnMovePrev1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnMovePrev1.Image = ((System.Drawing.Image)(resources.GetObject("btnMovePrev1.Image")));
            this.btnMovePrev1.Name = "btnMovePrev1";
            this.btnMovePrev1.RightToLeftAutoMirrorImage = true;
            this.btnMovePrev1.Size = new System.Drawing.Size(23, 22);
            this.btnMovePrev1.Text = "Move previous";
            // 
            // sep1
            // 
            this.sep1.Name = "sep1";
            this.sep1.Size = new System.Drawing.Size(6, 25);
            // 
            // txtPosition1
            // 
            this.txtPosition1.AccessibleName = "Position";
            this.txtPosition1.AutoSize = false;
            this.txtPosition1.Name = "txtPosition1";
            this.txtPosition1.Size = new System.Drawing.Size(50, 23);
            this.txtPosition1.Text = "0";
            this.txtPosition1.ToolTipText = "Current position";
            // 
            // sep2
            // 
            this.sep2.Name = "sep2";
            this.sep2.Size = new System.Drawing.Size(6, 25);
            // 
            // btnMoveNext1
            // 
            this.btnMoveNext1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnMoveNext1.Image = ((System.Drawing.Image)(resources.GetObject("btnMoveNext1.Image")));
            this.btnMoveNext1.Name = "btnMoveNext1";
            this.btnMoveNext1.RightToLeftAutoMirrorImage = true;
            this.btnMoveNext1.Size = new System.Drawing.Size(23, 22);
            this.btnMoveNext1.Text = "Move next";
            // 
            // btnMoveLast1
            // 
            this.btnMoveLast1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnMoveLast1.Image = ((System.Drawing.Image)(resources.GetObject("btnMoveLast1.Image")));
            this.btnMoveLast1.Name = "btnMoveLast1";
            this.btnMoveLast1.RightToLeftAutoMirrorImage = true;
            this.btnMoveLast1.Size = new System.Drawing.Size(23, 22);
            this.btnMoveLast1.Text = "Move last";
            // 
            // sep3
            // 
            this.sep3.Name = "sep3";
            this.sep3.Size = new System.Drawing.Size(6, 25);
            // 
            // btnRefresh
            // 
            this.btnRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnRefresh.Image = ((System.Drawing.Image)(resources.GetObject("btnRefresh.Image")));
            this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(23, 22);
            this.btnRefresh.Text = "Refresh data";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.pnlRight);
            this.pnlMain.Controls.Add(this.splMain);
            this.pnlMain.Controls.Add(this.pnlLeft);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 0);
            this.pnlMain.Margin = new System.Windows.Forms.Padding(0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(764, 471);
            this.pnlMain.TabIndex = 0;
            // 
            // splMain
            // 
            this.splMain.Enabled = false;
            this.splMain.Location = new System.Drawing.Point(270, 0);
            this.splMain.Name = "splMain";
            this.splMain.Size = new System.Drawing.Size(3, 471);
            this.splMain.TabIndex = 1;
            this.splMain.TabStop = false;
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.btnExportToCsv);
            this.pnlBottom.Controls.Add(this.btnClose);
            this.pnlBottom.Controls.Add(this.btnSave);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 471);
            this.pnlBottom.Margin = new System.Windows.Forms.Padding(0);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(764, 41);
            this.pnlBottom.TabIndex = 3;
            // 
            // btnExportToCsv
            // 
            this.btnExportToCsv.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnExportToCsv.Location = new System.Drawing.Point(12, 6);
            this.btnExportToCsv.Name = "btnExportToCsv";
            this.btnExportToCsv.Size = new System.Drawing.Size(100, 23);
            this.btnExportToCsv.TabIndex = 0;
            this.btnExportToCsv.Text = "Export to CSV";
            this.btnExportToCsv.UseVisualStyleBackColor = true;
            this.btnExportToCsv.Click += new System.EventHandler(this.btnExportToCsv_Click);
            // 
            // sfdCsv
            // 
            this.sfdCsv.DefaultExt = "*.csv";
            this.sfdCsv.Filter = "Comma-separated values (*.csv)|*.csv|All Files (*.*)|*.*";
            // 
            // FrmSnapshotTable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(764, 512);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.pnlBottom);
            this.KeyPreview = true;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(780, 550);
            this.Name = "FrmSnapshotTable";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Snapshot Table";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmSrezTableEdit_FormClosing);
            this.Load += new System.EventHandler(this.FrmSrezTableEdit_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmSrezTableEdit_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource2)).EndInit();
            this.pnlRight.ResumeLayout(false);
            this.pnlRight.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator2)).EndInit();
            this.bindingNavigator2.ResumeLayout(false);
            this.bindingNavigator2.PerformLayout();
            this.pnlLeft.ResumeLayout(false);
            this.pnlLeft.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).EndInit();
            this.bindingNavigator1.ResumeLayout(false);
            this.bindingNavigator1.PerformLayout();
            this.pnlMain.ResumeLayout(false);
            this.pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Panel pnlRight;
        private System.Windows.Forms.BindingNavigator bindingNavigator2;
        private System.Windows.Forms.ToolStripLabel lblCount2;
        private System.Windows.Forms.ToolStripButton btnMoveFirst2;
        private System.Windows.Forms.ToolStripButton btnMovePrev2;
        private System.Windows.Forms.ToolStripSeparator sep4;
        private System.Windows.Forms.ToolStripTextBox txtPosition2;
        private System.Windows.Forms.ToolStripSeparator sep5;
        private System.Windows.Forms.ToolStripButton btnMoveNext2;
        private System.Windows.Forms.ToolStripButton btnMoveLast2;
        private System.Windows.Forms.ToolStripSeparator sep6;
        private System.Windows.Forms.ToolStripLabel lblFilter;
        private System.Windows.Forms.ToolStripTextBox txtFilter;
        private System.Windows.Forms.Panel pnlLeft;
        private System.Windows.Forms.BindingNavigator bindingNavigator1;
        private System.Windows.Forms.ToolStripLabel lblCount1;
        private System.Windows.Forms.ToolStripButton btnMoveFirst1;
        private System.Windows.Forms.ToolStripButton btnMovePrev1;
        private System.Windows.Forms.ToolStripSeparator sep1;
        private System.Windows.Forms.ToolStripTextBox txtPosition1;
        private System.Windows.Forms.ToolStripSeparator sep2;
        private System.Windows.Forms.ToolStripButton btnMoveNext1;
        private System.Windows.Forms.ToolStripButton btnMoveLast1;
        private System.Windows.Forms.ToolStripSeparator sep3;
        private System.Windows.Forms.ToolStripButton btnRefresh;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCnlNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVal;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStat;
        private System.Windows.Forms.BindingSource bindingSource2;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Splitter splMain;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Button btnExportToCsv;
        private System.Windows.Forms.SaveFileDialog sfdCsv;
    }
}