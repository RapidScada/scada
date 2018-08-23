namespace Scada.Comm.Ctrl
{
    partial class FrmImport
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
            this.treeView = new System.Windows.Forms.TreeView();
            this.cmsTree = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miSelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.miDeselectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.btnImport = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblSelectData = new System.Windows.Forms.Label();
            this.cmsTree.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView
            // 
            this.treeView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeView.CheckBoxes = true;
            this.treeView.ContextMenuStrip = this.cmsTree;
            this.treeView.Location = new System.Drawing.Point(12, 25);
            this.treeView.Name = "treeView";
            this.treeView.Size = new System.Drawing.Size(270, 302);
            this.treeView.TabIndex = 0;
            this.treeView.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterCheck);
            // 
            // cmsTree
            // 
            this.cmsTree.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.cmsTree.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miSelectAll,
            this.miDeselectAll});
            this.cmsTree.Name = "cmsTree";
            this.cmsTree.Size = new System.Drawing.Size(150, 48);
            // 
            // miSelectAll
            // 
            this.miSelectAll.Name = "miSelectAll";
            this.miSelectAll.Size = new System.Drawing.Size(149, 22);
            this.miSelectAll.Text = "Выбрать все";
            this.miSelectAll.Click += new System.EventHandler(this.miSelectAll_Click);
            // 
            // miDeselectAll
            // 
            this.miDeselectAll.Name = "miDeselectAll";
            this.miDeselectAll.Size = new System.Drawing.Size(149, 22);
            this.miDeselectAll.Text = "Отменить все";
            this.miDeselectAll.Click += new System.EventHandler(this.miDeselectAll_Click);
            // 
            // btnImport
            // 
            this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImport.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnImport.Location = new System.Drawing.Point(126, 333);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(75, 23);
            this.btnImport.TabIndex = 1;
            this.btnImport.Text = "Импорт";
            this.btnImport.UseVisualStyleBackColor = true;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(207, 333);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Закрыть";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // lblSelectData
            // 
            this.lblSelectData.AutoSize = true;
            this.lblSelectData.Location = new System.Drawing.Point(9, 9);
            this.lblSelectData.Name = "lblSelectData";
            this.lblSelectData.Size = new System.Drawing.Size(183, 13);
            this.lblSelectData.TabIndex = 3;
            this.lblSelectData.Text = "Выберите импортируемые данные";
            // 
            // FrmImport
            // 
            this.AcceptButton = this.btnImport;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(294, 368);
            this.Controls.Add(this.lblSelectData);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.treeView);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(300, 400);
            this.Name = "FrmImport";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Импорт линий связи и КП";
            this.Load += new System.EventHandler(this.FrmImport_Load);
            this.cmsTree.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblSelectData;
        private System.Windows.Forms.ContextMenuStrip cmsTree;
        private System.Windows.Forms.ToolStripMenuItem miSelectAll;
        private System.Windows.Forms.ToolStripMenuItem miDeselectAll;

    }
}