namespace Scada.Comm.Devices.OpcUa.UI
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmConfig));
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.gbDevice = new System.Windows.Forms.GroupBox();
            this.tvDevice = new System.Windows.Forms.TreeView();
            this.ilTree = new System.Windows.Forms.ImageList(this.components);
            this.btnDeleteItem = new System.Windows.Forms.Button();
            this.btnMoveDownItem = new System.Windows.Forms.Button();
            this.btnMoveUpItem = new System.Windows.Forms.Button();
            this.btnAddSubscription = new System.Windows.Forms.Button();
            this.btnAddItem = new System.Windows.Forms.Button();
            this.gbServerBrowse = new System.Windows.Forms.GroupBox();
            this.btnViewAttrs = new System.Windows.Forms.Button();
            this.tvServer = new System.Windows.Forms.TreeView();
            this.gbConnection = new System.Windows.Forms.GroupBox();
            this.btnSecurityOptions = new System.Windows.Forms.Button();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.txtServerUrl = new System.Windows.Forms.TextBox();
            this.lblServerUrl = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.gbEmptyItem = new System.Windows.Forms.GroupBox();
            this.lblEmptyItem = new System.Windows.Forms.Label();
            this.ctrlCommand = new Scada.Comm.Devices.OpcUa.UI.CtrlCommand();
            this.ctrlItem = new Scada.Comm.Devices.OpcUa.UI.CtrlItem();
            this.ctrlSubscription = new Scada.Comm.Devices.OpcUa.UI.CtrlSubscription();
            this.gbDevice.SuspendLayout();
            this.gbServerBrowse.SuspendLayout();
            this.gbConnection.SuspendLayout();
            this.gbEmptyItem.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(647, 496);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 8;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Enabled = false;
            this.btnSave.Location = new System.Drawing.Point(566, 496);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // gbDevice
            // 
            this.gbDevice.Controls.Add(this.tvDevice);
            this.gbDevice.Controls.Add(this.btnDeleteItem);
            this.gbDevice.Controls.Add(this.btnMoveDownItem);
            this.gbDevice.Controls.Add(this.btnMoveUpItem);
            this.gbDevice.Controls.Add(this.btnAddSubscription);
            this.gbDevice.Controls.Add(this.btnAddItem);
            this.gbDevice.Location = new System.Drawing.Point(252, 83);
            this.gbDevice.Name = "gbDevice";
            this.gbDevice.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbDevice.Size = new System.Drawing.Size(234, 407);
            this.gbDevice.TabIndex = 2;
            this.gbDevice.TabStop = false;
            this.gbDevice.Text = "Device";
            // 
            // tvDevice
            // 
            this.tvDevice.HideSelection = false;
            this.tvDevice.ImageIndex = 0;
            this.tvDevice.ImageList = this.ilTree;
            this.tvDevice.Location = new System.Drawing.Point(13, 48);
            this.tvDevice.Name = "tvDevice";
            this.tvDevice.SelectedImageIndex = 0;
            this.tvDevice.Size = new System.Drawing.Size(208, 346);
            this.tvDevice.TabIndex = 5;
            this.tvDevice.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.tvDevice_AfterCollapse);
            this.tvDevice.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.tvDevice_AfterExpand);
            this.tvDevice.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvDevice_AfterSelect);
            // 
            // ilTree
            // 
            this.ilTree.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilTree.ImageStream")));
            this.ilTree.TransparentColor = System.Drawing.Color.Transparent;
            this.ilTree.Images.SetKeyName(0, "command.png");
            this.ilTree.Images.SetKeyName(1, "empty.png");
            this.ilTree.Images.SetKeyName(2, "folder_closed.png");
            this.ilTree.Images.SetKeyName(3, "folder_open.png");
            this.ilTree.Images.SetKeyName(4, "method.png");
            this.ilTree.Images.SetKeyName(5, "object.png");
            this.ilTree.Images.SetKeyName(6, "variable.png");
            // 
            // btnDeleteItem
            // 
            this.btnDeleteItem.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnDeleteItem.Image = ((System.Drawing.Image)(resources.GetObject("btnDeleteItem.Image")));
            this.btnDeleteItem.Location = new System.Drawing.Point(129, 19);
            this.btnDeleteItem.Name = "btnDeleteItem";
            this.btnDeleteItem.Size = new System.Drawing.Size(23, 23);
            this.btnDeleteItem.TabIndex = 4;
            this.toolTip.SetToolTip(this.btnDeleteItem, "Delete");
            this.btnDeleteItem.UseVisualStyleBackColor = true;
            this.btnDeleteItem.Click += new System.EventHandler(this.btnDeleteItem_Click);
            // 
            // btnMoveDownItem
            // 
            this.btnMoveDownItem.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnMoveDownItem.Image = ((System.Drawing.Image)(resources.GetObject("btnMoveDownItem.Image")));
            this.btnMoveDownItem.Location = new System.Drawing.Point(100, 19);
            this.btnMoveDownItem.Name = "btnMoveDownItem";
            this.btnMoveDownItem.Size = new System.Drawing.Size(23, 23);
            this.btnMoveDownItem.TabIndex = 3;
            this.toolTip.SetToolTip(this.btnMoveDownItem, "Move Down");
            this.btnMoveDownItem.UseVisualStyleBackColor = true;
            this.btnMoveDownItem.Click += new System.EventHandler(this.btnMoveDownItem_Click);
            // 
            // btnMoveUpItem
            // 
            this.btnMoveUpItem.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnMoveUpItem.Image = ((System.Drawing.Image)(resources.GetObject("btnMoveUpItem.Image")));
            this.btnMoveUpItem.Location = new System.Drawing.Point(71, 19);
            this.btnMoveUpItem.Name = "btnMoveUpItem";
            this.btnMoveUpItem.Size = new System.Drawing.Size(23, 23);
            this.btnMoveUpItem.TabIndex = 2;
            this.toolTip.SetToolTip(this.btnMoveUpItem, "Move Up");
            this.btnMoveUpItem.UseVisualStyleBackColor = true;
            this.btnMoveUpItem.Click += new System.EventHandler(this.btnMoveUpItem_Click);
            // 
            // btnAddSubscription
            // 
            this.btnAddSubscription.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAddSubscription.Image = ((System.Drawing.Image)(resources.GetObject("btnAddSubscription.Image")));
            this.btnAddSubscription.Location = new System.Drawing.Point(42, 19);
            this.btnAddSubscription.Name = "btnAddSubscription";
            this.btnAddSubscription.Size = new System.Drawing.Size(23, 23);
            this.btnAddSubscription.TabIndex = 1;
            this.toolTip.SetToolTip(this.btnAddSubscription, "Add Subscription");
            this.btnAddSubscription.UseVisualStyleBackColor = true;
            this.btnAddSubscription.Click += new System.EventHandler(this.btnAddSubscription_Click);
            // 
            // btnAddItem
            // 
            this.btnAddItem.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAddItem.Image = ((System.Drawing.Image)(resources.GetObject("btnAddItem.Image")));
            this.btnAddItem.Location = new System.Drawing.Point(13, 19);
            this.btnAddItem.Name = "btnAddItem";
            this.btnAddItem.Size = new System.Drawing.Size(23, 23);
            this.btnAddItem.TabIndex = 0;
            this.toolTip.SetToolTip(this.btnAddItem, "Add Selected Item");
            this.btnAddItem.UseVisualStyleBackColor = true;
            this.btnAddItem.Click += new System.EventHandler(this.btnAddItem_Click);
            // 
            // gbServerBrowse
            // 
            this.gbServerBrowse.Controls.Add(this.btnViewAttrs);
            this.gbServerBrowse.Controls.Add(this.tvServer);
            this.gbServerBrowse.Location = new System.Drawing.Point(12, 83);
            this.gbServerBrowse.Name = "gbServerBrowse";
            this.gbServerBrowse.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbServerBrowse.Size = new System.Drawing.Size(234, 407);
            this.gbServerBrowse.TabIndex = 1;
            this.gbServerBrowse.TabStop = false;
            this.gbServerBrowse.Text = "Server Browse";
            // 
            // btnViewAttrs
            // 
            this.btnViewAttrs.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnViewAttrs.Image = ((System.Drawing.Image)(resources.GetObject("btnViewAttrs.Image")));
            this.btnViewAttrs.Location = new System.Drawing.Point(13, 19);
            this.btnViewAttrs.Name = "btnViewAttrs";
            this.btnViewAttrs.Size = new System.Drawing.Size(23, 23);
            this.btnViewAttrs.TabIndex = 1;
            this.toolTip.SetToolTip(this.btnViewAttrs, "View Attributes");
            this.btnViewAttrs.UseVisualStyleBackColor = true;
            this.btnViewAttrs.Click += new System.EventHandler(this.btnViewAttrs_Click);
            // 
            // tvServer
            // 
            this.tvServer.HideSelection = false;
            this.tvServer.ImageIndex = 0;
            this.tvServer.ImageList = this.ilTree;
            this.tvServer.Location = new System.Drawing.Point(13, 48);
            this.tvServer.Name = "tvServer";
            this.tvServer.SelectedImageIndex = 0;
            this.tvServer.Size = new System.Drawing.Size(208, 346);
            this.tvServer.TabIndex = 0;
            this.tvServer.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvServer_BeforeExpand);
            this.tvServer.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvServer_AfterSelect);
            this.tvServer.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvServer_NodeMouseDoubleClick);
            this.tvServer.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tvServer_KeyDown);
            // 
            // gbConnection
            // 
            this.gbConnection.Controls.Add(this.btnSecurityOptions);
            this.gbConnection.Controls.Add(this.btnDisconnect);
            this.gbConnection.Controls.Add(this.btnConnect);
            this.gbConnection.Controls.Add(this.txtServerUrl);
            this.gbConnection.Controls.Add(this.lblServerUrl);
            this.gbConnection.Location = new System.Drawing.Point(12, 12);
            this.gbConnection.Name = "gbConnection";
            this.gbConnection.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbConnection.Size = new System.Drawing.Size(710, 65);
            this.gbConnection.TabIndex = 0;
            this.gbConnection.TabStop = false;
            this.gbConnection.Text = "Connection";
            // 
            // btnSecurityOptions
            // 
            this.btnSecurityOptions.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSecurityOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnSecurityOptions.Image")));
            this.btnSecurityOptions.Location = new System.Drawing.Point(674, 31);
            this.btnSecurityOptions.Name = "btnSecurityOptions";
            this.btnSecurityOptions.Size = new System.Drawing.Size(23, 23);
            this.btnSecurityOptions.TabIndex = 4;
            this.toolTip.SetToolTip(this.btnSecurityOptions, "Security Options");
            this.btnSecurityOptions.UseVisualStyleBackColor = true;
            this.btnSecurityOptions.Click += new System.EventHandler(this.btnSecurityOptions_Click);
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnDisconnect.Image = ((System.Drawing.Image)(resources.GetObject("btnDisconnect.Image")));
            this.btnDisconnect.Location = new System.Drawing.Point(645, 31);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(23, 23);
            this.btnDisconnect.TabIndex = 3;
            this.toolTip.SetToolTip(this.btnDisconnect, "Disconnect from Server");
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnConnect.Image = ((System.Drawing.Image)(resources.GetObject("btnConnect.Image")));
            this.btnConnect.Location = new System.Drawing.Point(616, 31);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(23, 23);
            this.btnConnect.TabIndex = 2;
            this.toolTip.SetToolTip(this.btnConnect, "Connect to Server");
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_ClickAsync);
            // 
            // txtServerUrl
            // 
            this.txtServerUrl.Location = new System.Drawing.Point(13, 32);
            this.txtServerUrl.Name = "txtServerUrl";
            this.txtServerUrl.Size = new System.Drawing.Size(597, 20);
            this.txtServerUrl.TabIndex = 1;
            this.txtServerUrl.TextChanged += new System.EventHandler(this.txtServerUrl_TextChanged);
            // 
            // lblServerUrl
            // 
            this.lblServerUrl.AutoSize = true;
            this.lblServerUrl.Location = new System.Drawing.Point(10, 16);
            this.lblServerUrl.Name = "lblServerUrl";
            this.lblServerUrl.Size = new System.Drawing.Size(63, 13);
            this.lblServerUrl.TabIndex = 0;
            this.lblServerUrl.Text = "Server URL";
            // 
            // gbEmptyItem
            // 
            this.gbEmptyItem.Controls.Add(this.lblEmptyItem);
            this.gbEmptyItem.Location = new System.Drawing.Point(492, 83);
            this.gbEmptyItem.Name = "gbEmptyItem";
            this.gbEmptyItem.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbEmptyItem.Size = new System.Drawing.Size(230, 407);
            this.gbEmptyItem.TabIndex = 3;
            this.gbEmptyItem.TabStop = false;
            this.gbEmptyItem.Text = "Item Parameters";
            // 
            // lblEmptyItem
            // 
            this.lblEmptyItem.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblEmptyItem.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblEmptyItem.Location = new System.Drawing.Point(13, 16);
            this.lblEmptyItem.Name = "lblEmptyItem";
            this.lblEmptyItem.Size = new System.Drawing.Size(204, 50);
            this.lblEmptyItem.TabIndex = 0;
            this.lblEmptyItem.Text = "Item not selected";
            this.lblEmptyItem.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ctrlCommand
            // 
            this.ctrlCommand.CommandConfig = null;
            this.ctrlCommand.Location = new System.Drawing.Point(492, 83);
            this.ctrlCommand.Name = "ctrlCommand";
            this.ctrlCommand.Size = new System.Drawing.Size(230, 407);
            this.ctrlCommand.TabIndex = 6;
            this.ctrlCommand.ObjectChanged += new Scada.UI.ObjectChangedEventHandler(this.ctrlItem_ObjectChanged);
            // 
            // ctrlItem
            // 
            this.ctrlItem.ItemConfig = null;
            this.ctrlItem.Location = new System.Drawing.Point(492, 83);
            this.ctrlItem.Name = "ctrlItem";
            this.ctrlItem.Size = new System.Drawing.Size(230, 407);
            this.ctrlItem.TabIndex = 5;
            this.ctrlItem.ObjectChanged += new Scada.UI.ObjectChangedEventHandler(this.ctrlItem_ObjectChanged);
            // 
            // ctrlSubscription
            // 
            this.ctrlSubscription.Location = new System.Drawing.Point(492, 83);
            this.ctrlSubscription.Name = "ctrlSubscription";
            this.ctrlSubscription.Size = new System.Drawing.Size(230, 407);
            this.ctrlSubscription.SubscriptionConfig = null;
            this.ctrlSubscription.TabIndex = 4;
            this.ctrlSubscription.ObjectChanged += new Scada.UI.ObjectChangedEventHandler(this.ctrlItem_ObjectChanged);
            // 
            // FrmConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(734, 531);
            this.Controls.Add(this.ctrlCommand);
            this.Controls.Add(this.ctrlItem);
            this.Controls.Add(this.ctrlSubscription);
            this.Controls.Add(this.gbEmptyItem);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.gbDevice);
            this.Controls.Add(this.gbServerBrowse);
            this.Controls.Add(this.gbConnection);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmConfig";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "OPC UA - Device {0} Properties";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmConfig_FormClosing);
            this.Load += new System.EventHandler(this.FrmConfig_Load);
            this.gbDevice.ResumeLayout(false);
            this.gbServerBrowse.ResumeLayout(false);
            this.gbConnection.ResumeLayout(false);
            this.gbConnection.PerformLayout();
            this.gbEmptyItem.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.GroupBox gbDevice;
        private System.Windows.Forms.GroupBox gbServerBrowse;
        private System.Windows.Forms.GroupBox gbConnection;
        private System.Windows.Forms.TextBox txtServerUrl;
        private System.Windows.Forms.Label lblServerUrl;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnSecurityOptions;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.TreeView tvServer;
        private System.Windows.Forms.ImageList ilTree;
        private System.Windows.Forms.Button btnAddItem;
        private System.Windows.Forms.Button btnDeleteItem;
        private System.Windows.Forms.Button btnMoveDownItem;
        private System.Windows.Forms.Button btnMoveUpItem;
        private System.Windows.Forms.Button btnAddSubscription;
        private System.Windows.Forms.TreeView tvDevice;
        private CtrlSubscription ctrlSubscription;
        private System.Windows.Forms.GroupBox gbEmptyItem;
        private System.Windows.Forms.Label lblEmptyItem;
        private CtrlItem ctrlItem;
        private CtrlCommand ctrlCommand;
        private System.Windows.Forms.Button btnViewAttrs;
    }
}