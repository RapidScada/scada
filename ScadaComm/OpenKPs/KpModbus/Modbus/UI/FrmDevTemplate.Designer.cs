namespace Scada.Comm.Devices.Modbus.UI
{
    partial class FrmDevTemplate
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Element groups");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Commands");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmDevTemplate));
            this.treeView = new System.Windows.Forms.TreeView();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.btnNew = new System.Windows.Forms.ToolStripButton();
            this.btnOpen = new System.Windows.Forms.ToolStripButton();
            this.btnSave = new System.Windows.Forms.ToolStripButton();
            this.btnSaveAs = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnAddElemGroup = new System.Windows.Forms.ToolStripButton();
            this.btnAddElem = new System.Windows.Forms.ToolStripButton();
            this.btnAddCmd = new System.Windows.Forms.ToolStripButton();
            this.btnMoveUp = new System.Windows.Forms.ToolStripButton();
            this.btnMoveDown = new System.Windows.Forms.ToolStripButton();
            this.btnDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnEditSettings = new System.Windows.Forms.ToolStripButton();
            this.btnEditSettingsExt = new System.Windows.Forms.ToolStripButton();
            this.gbDevTemplate = new System.Windows.Forms.GroupBox();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.ctrlElemGroup = new Scada.Comm.Devices.Modbus.UI.CtrlElemGroup();
            this.ctrlElem = new Scada.Comm.Devices.Modbus.UI.CtrlElem();
            this.ctrlCmd = new Scada.Comm.Devices.Modbus.UI.CtrlCmd();
            this.toolStrip.SuspendLayout();
            this.gbDevTemplate.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView
            // 
            this.treeView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeView.HideSelection = false;
            this.treeView.ImageIndex = 0;
            this.treeView.ImageList = this.imageList;
            this.treeView.Location = new System.Drawing.Point(13, 19);
            this.treeView.Name = "treeView";
            treeNode1.ImageKey = "group.png";
            treeNode1.Name = "grsNode";
            treeNode1.SelectedImageKey = "group.png";
            treeNode1.Text = "Element groups";
            treeNode2.ImageIndex = 3;
            treeNode2.Name = "cmdsNode";
            treeNode2.SelectedImageKey = "cmds.png";
            treeNode2.Text = "Commands";
            this.treeView.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2});
            this.treeView.SelectedImageIndex = 0;
            this.treeView.ShowRootLines = false;
            this.treeView.Size = new System.Drawing.Size(254, 469);
            this.treeView.TabIndex = 0;
            this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterSelect);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "group.png");
            this.imageList.Images.SetKeyName(1, "group_inactive.png");
            this.imageList.Images.SetKeyName(2, "elem.png");
            this.imageList.Images.SetKeyName(3, "cmds.png");
            this.imageList.Images.SetKeyName(4, "cmd.png");
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnNew,
            this.btnOpen,
            this.btnSave,
            this.btnSaveAs,
            this.toolStripSeparator1,
            this.btnAddElemGroup,
            this.btnAddElem,
            this.btnAddCmd,
            this.btnMoveUp,
            this.btnMoveDown,
            this.btnDelete,
            this.toolStripSeparator2,
            this.btnEditSettings,
            this.btnEditSettingsExt});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(590, 25);
            this.toolStrip.TabIndex = 0;
            this.toolStrip.Text = "toolStrip1";
            // 
            // btnNew
            // 
            this.btnNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnNew.Image = ((System.Drawing.Image)(resources.GetObject("btnNew.Image")));
            this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(23, 22);
            this.btnNew.ToolTipText = "Create new template";
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnOpen
            // 
            this.btnOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnOpen.Image = ((System.Drawing.Image)(resources.GetObject("btnOpen.Image")));
            this.btnOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(23, 22);
            this.btnOpen.ToolTipText = "Open template";
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // btnSave
            // 
            this.btnSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(23, 22);
            this.btnSave.ToolTipText = "Save template";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnSaveAs
            // 
            this.btnSaveAs.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSaveAs.Image = ((System.Drawing.Image)(resources.GetObject("btnSaveAs.Image")));
            this.btnSaveAs.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSaveAs.Name = "btnSaveAs";
            this.btnSaveAs.Size = new System.Drawing.Size(23, 22);
            this.btnSaveAs.ToolTipText = "Save template as";
            this.btnSaveAs.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnAddElemGroup
            // 
            this.btnAddElemGroup.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnAddElemGroup.Image = ((System.Drawing.Image)(resources.GetObject("btnAddElemGroup.Image")));
            this.btnAddElemGroup.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddElemGroup.Name = "btnAddElemGroup";
            this.btnAddElemGroup.Size = new System.Drawing.Size(23, 22);
            this.btnAddElemGroup.ToolTipText = "Add element group";
            this.btnAddElemGroup.Click += new System.EventHandler(this.btnAddElemGroup_Click);
            // 
            // btnAddElem
            // 
            this.btnAddElem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnAddElem.Image = ((System.Drawing.Image)(resources.GetObject("btnAddElem.Image")));
            this.btnAddElem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddElem.Name = "btnAddElem";
            this.btnAddElem.Size = new System.Drawing.Size(23, 22);
            this.btnAddElem.ToolTipText = "Add element";
            this.btnAddElem.Click += new System.EventHandler(this.btnAddElem_Click);
            // 
            // btnAddCmd
            // 
            this.btnAddCmd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnAddCmd.Image = ((System.Drawing.Image)(resources.GetObject("btnAddCmd.Image")));
            this.btnAddCmd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddCmd.Name = "btnAddCmd";
            this.btnAddCmd.Size = new System.Drawing.Size(23, 22);
            this.btnAddCmd.ToolTipText = "Add command";
            this.btnAddCmd.Click += new System.EventHandler(this.btnAddCmd_Click);
            // 
            // btnMoveUp
            // 
            this.btnMoveUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnMoveUp.Image = ((System.Drawing.Image)(resources.GetObject("btnMoveUp.Image")));
            this.btnMoveUp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnMoveUp.Name = "btnMoveUp";
            this.btnMoveUp.Size = new System.Drawing.Size(23, 22);
            this.btnMoveUp.ToolTipText = "Move up";
            this.btnMoveUp.Click += new System.EventHandler(this.btnMoveUp_Click);
            // 
            // btnMoveDown
            // 
            this.btnMoveDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnMoveDown.Image = ((System.Drawing.Image)(resources.GetObject("btnMoveDown.Image")));
            this.btnMoveDown.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnMoveDown.Name = "btnMoveDown";
            this.btnMoveDown.Size = new System.Drawing.Size(23, 22);
            this.btnMoveDown.ToolTipText = "Move down";
            this.btnMoveDown.Click += new System.EventHandler(this.btnMoveDown_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnDelete.Image = ((System.Drawing.Image)(resources.GetObject("btnDelete.Image")));
            this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(23, 22);
            this.btnDelete.ToolTipText = "Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // btnEditSettings
            // 
            this.btnEditSettings.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnEditSettings.Image = ((System.Drawing.Image)(resources.GetObject("btnEditSettings.Image")));
            this.btnEditSettings.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnEditSettings.Name = "btnEditSettings";
            this.btnEditSettings.Size = new System.Drawing.Size(23, 22);
            this.btnEditSettings.ToolTipText = "Edit template settings";
            this.btnEditSettings.Click += new System.EventHandler(this.btnEditSettings_Click);
            // 
            // btnEditSettingsExt
            // 
            this.btnEditSettingsExt.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnEditSettingsExt.Image = ((System.Drawing.Image)(resources.GetObject("btnEditSettingsExt.Image")));
            this.btnEditSettingsExt.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnEditSettingsExt.Name = "btnEditSettingsExt";
            this.btnEditSettingsExt.Size = new System.Drawing.Size(23, 22);
            this.btnEditSettingsExt.ToolTipText = "Edit extended settings";
            this.btnEditSettingsExt.Click += new System.EventHandler(this.btnEditSettingsExt_Click);
            // 
            // gbDevTemplate
            // 
            this.gbDevTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbDevTemplate.Controls.Add(this.treeView);
            this.gbDevTemplate.Location = new System.Drawing.Point(12, 28);
            this.gbDevTemplate.Name = "gbDevTemplate";
            this.gbDevTemplate.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbDevTemplate.Size = new System.Drawing.Size(280, 501);
            this.gbDevTemplate.TabIndex = 1;
            this.gbDevTemplate.TabStop = false;
            this.gbDevTemplate.Text = "Device template";
            // 
            // openFileDialog
            // 
            this.openFileDialog.DefaultExt = "*.xml";
            this.openFileDialog.Filter = "Template Files (*.xml)|*.xml|All Files (*.*)|*.*";
            this.openFileDialog.FilterIndex = 0;
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.DefaultExt = "*.xml";
            this.saveFileDialog.Filter = "Template Files (*.xml)|*.xml|All Files (*.*)|*.*";
            this.saveFileDialog.FilterIndex = 0;
            // 
            // ctrlElemGroup
            // 
            this.ctrlElemGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ctrlElemGroup.ElemGroup = null;
            this.ctrlElemGroup.Location = new System.Drawing.Point(298, 28);
            this.ctrlElemGroup.Name = "ctrlElemGroup";
            this.ctrlElemGroup.Settings = null;
            this.ctrlElemGroup.Size = new System.Drawing.Size(280, 245);
            this.ctrlElemGroup.TabIndex = 2;
            this.ctrlElemGroup.ObjectChanged += new Scada.UI.ObjectChangedEventHandler(this.ctrlElemGroup_ObjectChanged);
            // 
            // ctrlElem
            // 
            this.ctrlElem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ctrlElem.ElemInfo = null;
            this.ctrlElem.Location = new System.Drawing.Point(298, 201);
            this.ctrlElem.Name = "ctrlElem";
            this.ctrlElem.Size = new System.Drawing.Size(280, 271);
            this.ctrlElem.TabIndex = 3;
            this.ctrlElem.ObjectChanged += new Scada.UI.ObjectChangedEventHandler(this.ctrlElem_ObjectChanged);
            // 
            // ctrlCmd
            // 
            this.ctrlCmd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ctrlCmd.Location = new System.Drawing.Point(298, 205);
            this.ctrlCmd.ModbusCmd = null;
            this.ctrlCmd.Name = "ctrlCmd";
            this.ctrlCmd.Settings = null;
            this.ctrlCmd.Size = new System.Drawing.Size(280, 324);
            this.ctrlCmd.TabIndex = 4;
            this.ctrlCmd.ObjectChanged += new Scada.UI.ObjectChangedEventHandler(this.ctrlCmd_ObjectChanged);
            // 
            // FrmDevTemplate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(590, 541);
            this.Controls.Add(this.ctrlElemGroup);
            this.Controls.Add(this.ctrlElem);
            this.Controls.Add(this.ctrlCmd);
            this.Controls.Add(this.gbDevTemplate);
            this.Controls.Add(this.toolStrip);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(606, 500);
            this.Name = "FrmDevTemplate";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "MODBUS. Device Template Editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmDevTemplate_FormClosing);
            this.Load += new System.EventHandler(this.FrmDevTemplate_Load);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.gbDevTemplate.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton btnSave;
        private System.Windows.Forms.ToolStripButton btnOpen;
        private System.Windows.Forms.ToolStripButton btnNew;
        private System.Windows.Forms.ToolStripButton btnAddElemGroup;
        private System.Windows.Forms.ToolStripButton btnAddCmd;
        private System.Windows.Forms.ToolStripButton btnMoveUp;
        private System.Windows.Forms.ToolStripButton btnMoveDown;
        private System.Windows.Forms.ToolStripButton btnDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.GroupBox gbDevTemplate;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.ToolStripButton btnSaveAs;
        private System.Windows.Forms.ToolStripButton btnAddElem;
        private CtrlCmd ctrlCmd;
        private CtrlElem ctrlElem;
        private CtrlElemGroup ctrlElemGroup;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton btnEditSettings;
        private System.Windows.Forms.ToolStripButton btnEditSettingsExt;
    }
}