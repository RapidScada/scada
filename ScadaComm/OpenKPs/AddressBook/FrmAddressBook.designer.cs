namespace Scada.Comm.Devices.AB
{
    partial class FrmAddressBook
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmAddressBook));
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Адресная книга");
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.btnAddContactGroup = new System.Windows.Forms.ToolStripButton();
            this.btnAddContact = new System.Windows.Forms.ToolStripButton();
            this.btnAddPhoneNumber = new System.Windows.Forms.ToolStripButton();
            this.btnAddEmail = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnEdit = new System.Windows.Forms.ToolStripButton();
            this.btnDelete = new System.Windows.Forms.ToolStripButton();
            this.treeView = new System.Windows.Forms.TreeView();
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
            this.btnAddContactGroup,
            this.btnAddContact,
            this.btnAddPhoneNumber,
            this.btnAddEmail,
            this.toolStripSeparator1,
            this.btnEdit,
            this.btnDelete});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(334, 25);
            this.toolStrip.TabIndex = 2;
            this.toolStrip.Text = "toolStrip1";
            // 
            // btnAddContactGroup
            // 
            this.btnAddContactGroup.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnAddContactGroup.Image = ((System.Drawing.Image)(resources.GetObject("btnAddContactGroup.Image")));
            this.btnAddContactGroup.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddContactGroup.Name = "btnAddContactGroup";
            this.btnAddContactGroup.Size = new System.Drawing.Size(23, 22);
            this.btnAddContactGroup.ToolTipText = "Добавить группу контактов";
            this.btnAddContactGroup.Click += new System.EventHandler(this.btnAddContactGroup_Click);
            // 
            // btnAddContact
            // 
            this.btnAddContact.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnAddContact.Image = ((System.Drawing.Image)(resources.GetObject("btnAddContact.Image")));
            this.btnAddContact.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddContact.Name = "btnAddContact";
            this.btnAddContact.Size = new System.Drawing.Size(23, 22);
            this.btnAddContact.ToolTipText = "Добавить контакт";
            this.btnAddContact.Click += new System.EventHandler(this.btnAddContact_Click);
            // 
            // btnAddPhoneNumber
            // 
            this.btnAddPhoneNumber.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnAddPhoneNumber.Image = ((System.Drawing.Image)(resources.GetObject("btnAddPhoneNumber.Image")));
            this.btnAddPhoneNumber.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddPhoneNumber.Name = "btnAddPhoneNumber";
            this.btnAddPhoneNumber.Size = new System.Drawing.Size(23, 22);
            this.btnAddPhoneNumber.ToolTipText = "Добавить телефонный номер";
            this.btnAddPhoneNumber.Click += new System.EventHandler(this.btnAddPhoneNumber_Click);
            // 
            // btnAddEmail
            // 
            this.btnAddEmail.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnAddEmail.Image = ((System.Drawing.Image)(resources.GetObject("btnAddEmail.Image")));
            this.btnAddEmail.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddEmail.Name = "btnAddEmail";
            this.btnAddEmail.Size = new System.Drawing.Size(23, 22);
            this.btnAddEmail.ToolTipText = "Добавить адрес электронной почты";
            this.btnAddEmail.Click += new System.EventHandler(this.btnAddEmail_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
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
            // treeView
            // 
            this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView.HideSelection = false;
            this.treeView.ImageIndex = 0;
            this.treeView.ImageList = this.imageList;
            this.treeView.LabelEdit = true;
            this.treeView.Location = new System.Drawing.Point(0, 25);
            this.treeView.Name = "treeView";
            treeNode1.ImageKey = "book.png";
            treeNode1.Name = "rootNode";
            treeNode1.SelectedImageKey = "book.png";
            treeNode1.Text = "Адресная книга";
            this.treeView.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1});
            this.treeView.SelectedImageIndex = 0;
            this.treeView.ShowRootLines = false;
            this.treeView.Size = new System.Drawing.Size(334, 396);
            this.treeView.TabIndex = 3;
            this.treeView.BeforeLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.treeView_BeforeLabelEdit);
            this.treeView.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.treeView_AfterLabelEdit);
            this.treeView.BeforeCollapse += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView_BeforeCollapse);
            this.treeView.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView_BeforeExpand);
            this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterSelect);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "book.png");
            this.imageList.Images.SetKeyName(1, "folder_closed.png");
            this.imageList.Images.SetKeyName(2, "folder_open.png");
            this.imageList.Images.SetKeyName(3, "contact.png");
            this.imageList.Images.SetKeyName(4, "phone.png");
            this.imageList.Images.SetKeyName(5, "email.png");
            // 
            // FrmAddressBook
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(334, 462);
            this.Controls.Add(this.treeView);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.pnlBottom);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(250, 300);
            this.Name = "FrmAddressBook";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Адресная книга";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmAddressBook_FormClosing);
            this.Load += new System.EventHandler(this.FrmAddressBook_Load);
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
        private System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.ToolStripButton btnAddContactGroup;
        private System.Windows.Forms.ToolStripButton btnDelete;
        private System.Windows.Forms.ToolStripButton btnEdit;
        private System.Windows.Forms.ToolStripButton btnAddPhoneNumber;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnAddContact;
        private System.Windows.Forms.ToolStripButton btnAddEmail;
    }
}