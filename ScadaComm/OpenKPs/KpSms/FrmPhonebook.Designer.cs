namespace Scada.Comm.Devices.KpSms
{
    partial class FrmPhonebook
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPhonebook));
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Справочник");
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.btnCreateGroup = new System.Windows.Forms.ToolStripButton();
            this.btnCreateNumber = new System.Windows.Forms.ToolStripButton();
            this.btnEdit = new System.Windows.Forms.ToolStripButton();
            this.btnDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnCutNumber = new System.Windows.Forms.ToolStripButton();
            this.btnCopyNumber = new System.Windows.Forms.ToolStripButton();
            this.btnPasteNumber = new System.Windows.Forms.ToolStripButton();
            this.tvPhonebook = new System.Windows.Forms.TreeView();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.pnlBottom.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.btnClose);
            this.pnlBottom.Controls.Add(this.btnSave);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 421);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(334, 41);
            this.pnlBottom.TabIndex = 1;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(247, 6);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Закрыть";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(166, 6);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Сохранить";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnCreateGroup,
            this.btnCreateNumber,
            this.btnEdit,
            this.btnDelete,
            this.toolStripSeparator1,
            this.btnCutNumber,
            this.btnCopyNumber,
            this.btnPasteNumber});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(334, 25);
            this.toolStrip.TabIndex = 2;
            this.toolStrip.Text = "toolStrip1";
            // 
            // btnCreateGroup
            // 
            this.btnCreateGroup.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnCreateGroup.Image = ((System.Drawing.Image)(resources.GetObject("btnCreateGroup.Image")));
            this.btnCreateGroup.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCreateGroup.Name = "btnCreateGroup";
            this.btnCreateGroup.Size = new System.Drawing.Size(23, 22);
            this.btnCreateGroup.ToolTipText = "Создать группу";
            this.btnCreateGroup.Click += new System.EventHandler(this.btnCreateGroup_Click);
            // 
            // btnCreateNumber
            // 
            this.btnCreateNumber.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnCreateNumber.Image = ((System.Drawing.Image)(resources.GetObject("btnCreateNumber.Image")));
            this.btnCreateNumber.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCreateNumber.Name = "btnCreateNumber";
            this.btnCreateNumber.Size = new System.Drawing.Size(23, 22);
            this.btnCreateNumber.ToolTipText = "Создать номер";
            this.btnCreateNumber.Click += new System.EventHandler(this.btnCreateNumber_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnEdit.Image = ((System.Drawing.Image)(resources.GetObject("btnEdit.Image")));
            this.btnEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(23, 22);
            this.btnEdit.ToolTipText = "Редактировать";
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnDelete.Image = ((System.Drawing.Image)(resources.GetObject("btnDelete.Image")));
            this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(23, 22);
            this.btnDelete.ToolTipText = "Удалить";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnCutNumber
            // 
            this.btnCutNumber.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnCutNumber.Image = ((System.Drawing.Image)(resources.GetObject("btnCutNumber.Image")));
            this.btnCutNumber.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCutNumber.Name = "btnCutNumber";
            this.btnCutNumber.Size = new System.Drawing.Size(23, 22);
            this.btnCutNumber.ToolTipText = "Вырезать номер";
            this.btnCutNumber.Click += new System.EventHandler(this.btnCutNumber_Click);
            // 
            // btnCopyNumber
            // 
            this.btnCopyNumber.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnCopyNumber.Image = ((System.Drawing.Image)(resources.GetObject("btnCopyNumber.Image")));
            this.btnCopyNumber.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCopyNumber.Name = "btnCopyNumber";
            this.btnCopyNumber.Size = new System.Drawing.Size(23, 22);
            this.btnCopyNumber.ToolTipText = "Копировать номер";
            this.btnCopyNumber.Click += new System.EventHandler(this.btnCopyNumber_Click);
            // 
            // btnPasteNumber
            // 
            this.btnPasteNumber.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnPasteNumber.Image = ((System.Drawing.Image)(resources.GetObject("btnPasteNumber.Image")));
            this.btnPasteNumber.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPasteNumber.Name = "btnPasteNumber";
            this.btnPasteNumber.Size = new System.Drawing.Size(23, 22);
            this.btnPasteNumber.ToolTipText = "Вставить номер";
            this.btnPasteNumber.Click += new System.EventHandler(this.btnPasteNumber_Click);
            // 
            // tvPhonebook
            // 
            this.tvPhonebook.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvPhonebook.HideSelection = false;
            this.tvPhonebook.ImageIndex = 0;
            this.tvPhonebook.ImageList = this.imageList;
            this.tvPhonebook.Location = new System.Drawing.Point(0, 25);
            this.tvPhonebook.Name = "tvPhonebook";
            treeNode1.ImageKey = "book.png";
            treeNode1.Name = "nodePhonebook";
            treeNode1.SelectedImageKey = "book.png";
            treeNode1.Text = "Справочник";
            this.tvPhonebook.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1});
            this.tvPhonebook.SelectedImageIndex = 0;
            this.tvPhonebook.ShowRootLines = false;
            this.tvPhonebook.Size = new System.Drawing.Size(334, 396);
            this.tvPhonebook.TabIndex = 3;
            this.tvPhonebook.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvPhonebook_AfterSelect);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "folder_closed.png");
            this.imageList.Images.SetKeyName(1, "folder_open.png");
            this.imageList.Images.SetKeyName(2, "book.png");
            this.imageList.Images.SetKeyName(3, "phone.png");
            // 
            // FrmPhonebook
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(334, 462);
            this.Controls.Add(this.tvPhonebook);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.pnlBottom);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(250, 300);
            this.Name = "FrmPhonebook";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "SMS";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmPhonebook_FormClosing);
            this.Load += new System.EventHandler(this.FrmPhonebook_Load);
            this.pnlBottom.ResumeLayout(false);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.TreeView tvPhonebook;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.ToolStripButton btnCreateGroup;
        private System.Windows.Forms.ToolStripButton btnDelete;
        private System.Windows.Forms.ToolStripButton btnEdit;
        private System.Windows.Forms.ToolStripButton btnCreateNumber;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnCutNumber;
        private System.Windows.Forms.ToolStripButton btnCopyNumber;
        private System.Windows.Forms.ToolStripButton btnPasteNumber;
    }
}