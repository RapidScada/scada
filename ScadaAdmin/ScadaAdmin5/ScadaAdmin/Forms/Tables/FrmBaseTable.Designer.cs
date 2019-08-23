namespace Scada.Admin.App.Forms.Tables
{
    partial class FrmBaseTable
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmBaseTable));
            this.bindingNavigator = new System.Windows.Forms.BindingNavigator(this.components);
            this.lblCount = new System.Windows.Forms.ToolStripLabel();
            this.sep1 = new System.Windows.Forms.ToolStripSeparator();
            this.txtPosition = new System.Windows.Forms.ToolStripTextBox();
            this.sep2 = new System.Windows.Forms.ToolStripSeparator();
            this.sep3 = new System.Windows.Forms.ToolStripSeparator();
            this.sep4 = new System.Windows.Forms.ToolStripSeparator();
            this.sep5 = new System.Windows.Forms.ToolStripSeparator();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.pnlError = new System.Windows.Forms.Panel();
            this.btnCloseError = new System.Windows.Forms.Button();
            this.lblError = new System.Windows.Forms.Label();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.cmsTable = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.bindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.btnAddNew = new System.Windows.Forms.ToolStripButton();
            this.btnMoveFirst = new System.Windows.Forms.ToolStripButton();
            this.btnMovePrevious = new System.Windows.Forms.ToolStripButton();
            this.btnMoveNext = new System.Windows.Forms.ToolStripButton();
            this.btnMoveLast = new System.Windows.Forms.ToolStripButton();
            this.btnApplyEdit = new System.Windows.Forms.ToolStripButton();
            this.btnCancelEdit = new System.Windows.Forms.ToolStripButton();
            this.btnRefresh = new System.Windows.Forms.ToolStripButton();
            this.btnDelete = new System.Windows.Forms.ToolStripButton();
            this.btnClear = new System.Windows.Forms.ToolStripButton();
            this.btnCut = new System.Windows.Forms.ToolStripButton();
            this.btnCopy = new System.Windows.Forms.ToolStripButton();
            this.btnPaste = new System.Windows.Forms.ToolStripButton();
            this.btnFind = new System.Windows.Forms.ToolStripButton();
            this.btnAutoSizeColumns = new System.Windows.Forms.ToolStripButton();
            this.btnProperties = new System.Windows.Forms.ToolStripButton();
            this.miProperties = new System.Windows.Forms.ToolStripMenuItem();
            this.btnFilter = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator)).BeginInit();
            this.bindingNavigator.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.pnlError.SuspendLayout();
            this.cmsTable.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // bindingNavigator
            // 
            this.bindingNavigator.AddNewItem = this.btnAddNew;
            this.bindingNavigator.BindingSource = this.bindingSource;
            this.bindingNavigator.CountItem = this.lblCount;
            this.bindingNavigator.DeleteItem = null;
            this.bindingNavigator.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnMoveFirst,
            this.btnMovePrevious,
            this.sep1,
            this.txtPosition,
            this.lblCount,
            this.sep2,
            this.btnMoveNext,
            this.btnMoveLast,
            this.sep3,
            this.btnApplyEdit,
            this.btnCancelEdit,
            this.btnRefresh,
            this.btnAddNew,
            this.btnDelete,
            this.btnClear,
            this.sep4,
            this.btnCut,
            this.btnCopy,
            this.btnPaste,
            this.sep5,
            this.btnFind,
            this.btnFilter,
            this.btnAutoSizeColumns,
            this.btnProperties});
            this.bindingNavigator.Location = new System.Drawing.Point(0, 40);
            this.bindingNavigator.MoveFirstItem = this.btnMoveFirst;
            this.bindingNavigator.MoveLastItem = this.btnMoveLast;
            this.bindingNavigator.MoveNextItem = this.btnMoveNext;
            this.bindingNavigator.MovePreviousItem = this.btnMovePrevious;
            this.bindingNavigator.Name = "bindingNavigator";
            this.bindingNavigator.PositionItem = this.txtPosition;
            this.bindingNavigator.Size = new System.Drawing.Size(584, 25);
            this.bindingNavigator.TabIndex = 1;
            // 
            // lblCount
            // 
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(35, 22);
            this.lblCount.Text = "of {0}";
            this.lblCount.ToolTipText = "Total Number of Items";
            // 
            // sep1
            // 
            this.sep1.Name = "sep1";
            this.sep1.Size = new System.Drawing.Size(6, 25);
            // 
            // txtPosition
            // 
            this.txtPosition.AccessibleName = "Position";
            this.txtPosition.AutoSize = false;
            this.txtPosition.Name = "txtPosition";
            this.txtPosition.Size = new System.Drawing.Size(50, 23);
            this.txtPosition.Text = "0";
            this.txtPosition.ToolTipText = "Current Position";
            // 
            // sep2
            // 
            this.sep2.Name = "sep2";
            this.sep2.Size = new System.Drawing.Size(6, 25);
            // 
            // sep3
            // 
            this.sep3.Name = "sep3";
            this.sep3.Size = new System.Drawing.Size(6, 25);
            // 
            // sep4
            // 
            this.sep4.Name = "sep4";
            this.sep4.Size = new System.Drawing.Size(6, 25);
            // 
            // sep5
            // 
            this.sep5.Name = "sep5";
            this.sep5.Size = new System.Drawing.Size(6, 25);
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToOrderColumns = true;
            this.dataGridView.AutoGenerateColumns = false;
            this.dataGridView.DataSource = this.bindingSource;
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView.Location = new System.Drawing.Point(0, 65);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.Size = new System.Drawing.Size(584, 296);
            this.dataGridView.TabIndex = 2;
            this.dataGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_CellClick);
            this.dataGridView.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridView_CellFormatting);
            this.dataGridView.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView_CellMouseClick);
            this.dataGridView.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dataGridView_CellValidating);
            this.dataGridView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridView_DataError);
            this.dataGridView.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dataGridView_EditingControlShowing);
            this.dataGridView.RowValidating += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridView_RowValidating);
            // 
            // pnlError
            // 
            this.pnlError.AutoSize = true;
            this.pnlError.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(222)))), ((int)(((byte)(222)))));
            this.pnlError.Controls.Add(this.btnCloseError);
            this.pnlError.Controls.Add(this.lblError);
            this.pnlError.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlError.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(68)))), ((int)(((byte)(66)))));
            this.pnlError.Location = new System.Drawing.Point(0, 0);
            this.pnlError.Name = "pnlError";
            this.pnlError.Size = new System.Drawing.Size(584, 40);
            this.pnlError.TabIndex = 0;
            this.pnlError.Visible = false;
            // 
            // btnCloseError
            // 
            this.btnCloseError.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCloseError.Location = new System.Drawing.Point(497, 8);
            this.btnCloseError.Name = "btnCloseError";
            this.btnCloseError.Size = new System.Drawing.Size(75, 23);
            this.btnCloseError.TabIndex = 1;
            this.btnCloseError.Text = "Close";
            this.btnCloseError.UseVisualStyleBackColor = true;
            this.btnCloseError.Click += new System.EventHandler(this.btnCloseError_Click);
            // 
            // lblError
            // 
            this.lblError.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblError.Location = new System.Drawing.Point(0, 0);
            this.lblError.Name = "lblError";
            this.lblError.Padding = new System.Windows.Forms.Padding(5);
            this.lblError.Size = new System.Drawing.Size(479, 40);
            this.lblError.TabIndex = 0;
            this.lblError.Text = "Error message";
            this.lblError.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmsTable
            // 
            this.cmsTable.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miProperties});
            this.cmsTable.Name = "cmsTable";
            this.cmsTable.Size = new System.Drawing.Size(128, 26);
            // 
            // btnAddNew
            // 
            this.btnAddNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnAddNew.Image = ((System.Drawing.Image)(resources.GetObject("btnAddNew.Image")));
            this.btnAddNew.Name = "btnAddNew";
            this.btnAddNew.RightToLeftAutoMirrorImage = true;
            this.btnAddNew.Size = new System.Drawing.Size(23, 22);
            this.btnAddNew.Text = "Add New";
            // 
            // btnMoveFirst
            // 
            this.btnMoveFirst.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnMoveFirst.Image = ((System.Drawing.Image)(resources.GetObject("btnMoveFirst.Image")));
            this.btnMoveFirst.Name = "btnMoveFirst";
            this.btnMoveFirst.RightToLeftAutoMirrorImage = true;
            this.btnMoveFirst.Size = new System.Drawing.Size(23, 22);
            this.btnMoveFirst.Text = "Move First";
            // 
            // btnMovePrevious
            // 
            this.btnMovePrevious.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnMovePrevious.Image = ((System.Drawing.Image)(resources.GetObject("btnMovePrevious.Image")));
            this.btnMovePrevious.Name = "btnMovePrevious";
            this.btnMovePrevious.RightToLeftAutoMirrorImage = true;
            this.btnMovePrevious.Size = new System.Drawing.Size(23, 22);
            this.btnMovePrevious.Text = "Move Previous";
            // 
            // btnMoveNext
            // 
            this.btnMoveNext.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnMoveNext.Image = ((System.Drawing.Image)(resources.GetObject("btnMoveNext.Image")));
            this.btnMoveNext.Name = "btnMoveNext";
            this.btnMoveNext.RightToLeftAutoMirrorImage = true;
            this.btnMoveNext.Size = new System.Drawing.Size(23, 22);
            this.btnMoveNext.Text = "Move Next";
            // 
            // btnMoveLast
            // 
            this.btnMoveLast.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnMoveLast.Image = ((System.Drawing.Image)(resources.GetObject("btnMoveLast.Image")));
            this.btnMoveLast.Name = "btnMoveLast";
            this.btnMoveLast.RightToLeftAutoMirrorImage = true;
            this.btnMoveLast.Size = new System.Drawing.Size(23, 22);
            this.btnMoveLast.Text = "Move Last";
            // 
            // btnApplyEdit
            // 
            this.btnApplyEdit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnApplyEdit.Image = ((System.Drawing.Image)(resources.GetObject("btnApplyEdit.Image")));
            this.btnApplyEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnApplyEdit.Name = "btnApplyEdit";
            this.btnApplyEdit.Size = new System.Drawing.Size(23, 22);
            this.btnApplyEdit.Text = "Apply Edit Operation";
            this.btnApplyEdit.Click += new System.EventHandler(this.btnApplyEdit_Click);
            // 
            // btnCancelEdit
            // 
            this.btnCancelEdit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnCancelEdit.Image = ((System.Drawing.Image)(resources.GetObject("btnCancelEdit.Image")));
            this.btnCancelEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCancelEdit.Name = "btnCancelEdit";
            this.btnCancelEdit.Size = new System.Drawing.Size(23, 22);
            this.btnCancelEdit.Text = "Cancel Edit Operation";
            this.btnCancelEdit.Click += new System.EventHandler(this.btnCancelEdit_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnRefresh.Image = ((System.Drawing.Image)(resources.GetObject("btnRefresh.Image")));
            this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(23, 22);
            this.btnRefresh.Text = "Refresh Data";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnDelete.Image = ((System.Drawing.Image)(resources.GetObject("btnDelete.Image")));
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.RightToLeftAutoMirrorImage = true;
            this.btnDelete.Size = new System.Drawing.Size(23, 22);
            this.btnDelete.Text = "Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnClear
            // 
            this.btnClear.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnClear.Image = ((System.Drawing.Image)(resources.GetObject("btnClear.Image")));
            this.btnClear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(23, 22);
            this.btnClear.Text = "Clear Table";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnCut
            // 
            this.btnCut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnCut.Image = ((System.Drawing.Image)(resources.GetObject("btnCut.Image")));
            this.btnCut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCut.Name = "btnCut";
            this.btnCut.Size = new System.Drawing.Size(23, 22);
            this.btnCut.Text = "Cut (Ctrl+X)";
            this.btnCut.Click += new System.EventHandler(this.btnCut_Click);
            // 
            // btnCopy
            // 
            this.btnCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnCopy.Image = ((System.Drawing.Image)(resources.GetObject("btnCopy.Image")));
            this.btnCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(23, 22);
            this.btnCopy.Text = "Copy (Ctrl+C)";
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // btnPaste
            // 
            this.btnPaste.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnPaste.Image = ((System.Drawing.Image)(resources.GetObject("btnPaste.Image")));
            this.btnPaste.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPaste.Name = "btnPaste";
            this.btnPaste.Size = new System.Drawing.Size(23, 22);
            this.btnPaste.Text = "Paste (Ctrl+V)";
            this.btnPaste.Click += new System.EventHandler(this.btnPaste_Click);
            // 
            // btnFind
            // 
            this.btnFind.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnFind.Image = ((System.Drawing.Image)(resources.GetObject("btnFind.Image")));
            this.btnFind.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(23, 22);
            this.btnFind.Text = "Find and Replace (Ctrl+F)";
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // btnAutoSizeColumns
            // 
            this.btnAutoSizeColumns.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnAutoSizeColumns.Image = ((System.Drawing.Image)(resources.GetObject("btnAutoSizeColumns.Image")));
            this.btnAutoSizeColumns.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAutoSizeColumns.Name = "btnAutoSizeColumns";
            this.btnAutoSizeColumns.Size = new System.Drawing.Size(23, 22);
            this.btnAutoSizeColumns.Text = "Autofit Column Widths";
            this.btnAutoSizeColumns.Click += new System.EventHandler(this.btnAutoSizeColumns_Click);
            // 
            // btnProperties
            // 
            this.btnProperties.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnProperties.Image = ((System.Drawing.Image)(resources.GetObject("btnProperties.Image")));
            this.btnProperties.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnProperties.Name = "btnProperties";
            this.btnProperties.Size = new System.Drawing.Size(23, 22);
            this.btnProperties.Text = "Item Properties";
            this.btnProperties.Click += new System.EventHandler(this.btnProperties_Click);
            // 
            // miProperties
            // 
            this.miProperties.Image = ((System.Drawing.Image)(resources.GetObject("miProperties.Image")));
            this.miProperties.Name = "miProperties";
            this.miProperties.Size = new System.Drawing.Size(127, 22);
            this.miProperties.Text = "Properties";
            this.miProperties.Click += new System.EventHandler(this.btnProperties_Click);
            // 
            // btnFilter
            // 
            this.btnFilter.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnFilter.Image = ((System.Drawing.Image)(resources.GetObject("btnFilter.Image")));
            this.btnFilter.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.Size = new System.Drawing.Size(23, 22);
            this.btnFilter.Text = "Filter";
            this.btnFilter.Click += new System.EventHandler(this.btnFilter_Click);
            // 
            // FrmBaseTable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 361);
            this.Controls.Add(this.dataGridView);
            this.Controls.Add(this.bindingNavigator);
            this.Controls.Add(this.pnlError);
            this.KeyPreview = true;
            this.Name = "FrmBaseTable";
            this.Text = "FrmBaseTable";
            this.Load += new System.EventHandler(this.FrmBaseTable_Load);
            this.Shown += new System.EventHandler(this.FrmBaseTable_Shown);
            this.VisibleChanged += new System.EventHandler(this.FrmBaseTable_VisibleChanged);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmBaseTable_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator)).EndInit();
            this.bindingNavigator.ResumeLayout(false);
            this.bindingNavigator.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.pnlError.ResumeLayout(false);
            this.cmsTable.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.BindingNavigator bindingNavigator;
        private System.Windows.Forms.ToolStripButton btnAddNew;
        private System.Windows.Forms.ToolStripLabel lblCount;
        private System.Windows.Forms.ToolStripButton btnDelete;
        private System.Windows.Forms.ToolStripButton btnMoveFirst;
        private System.Windows.Forms.ToolStripButton btnMovePrevious;
        private System.Windows.Forms.ToolStripSeparator sep1;
        private System.Windows.Forms.ToolStripTextBox txtPosition;
        private System.Windows.Forms.ToolStripSeparator sep2;
        private System.Windows.Forms.ToolStripButton btnMoveNext;
        private System.Windows.Forms.ToolStripButton btnMoveLast;
        private System.Windows.Forms.ToolStripSeparator sep3;
        private System.Windows.Forms.Panel pnlError;
        private System.Windows.Forms.Label lblError;
        private System.Windows.Forms.Button btnCloseError;
        private System.Windows.Forms.ToolStripButton btnRefresh;
        private System.Windows.Forms.ToolStripButton btnClear;
        private System.Windows.Forms.ToolStripSeparator sep4;
        private System.Windows.Forms.ToolStripButton btnAutoSizeColumns;
        private System.Windows.Forms.ToolStripButton btnApplyEdit;
        private System.Windows.Forms.ToolStripButton btnCancelEdit;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.ToolStripButton btnCut;
        private System.Windows.Forms.ToolStripButton btnCopy;
        private System.Windows.Forms.ToolStripButton btnPaste;
        private System.Windows.Forms.ToolStripSeparator sep5;
        private System.Windows.Forms.ToolStripButton btnFind;
        private System.Windows.Forms.ToolStripButton btnProperties;
        private System.Windows.Forms.BindingSource bindingSource;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.ContextMenuStrip cmsTable;
        private System.Windows.Forms.ToolStripMenuItem miProperties;
        private System.Windows.Forms.ToolStripButton btnFilter;
    }
}