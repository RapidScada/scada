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
            this.gbServerBrowse = new System.Windows.Forms.GroupBox();
            this.tvServer = new System.Windows.Forms.TreeView();
            this.ilServer = new System.Windows.Forms.ImageList(this.components);
            this.gbConnection = new System.Windows.Forms.GroupBox();
            this.btnSecurityOptions = new System.Windows.Forms.Button();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.txtServerUrl = new System.Windows.Forms.TextBox();
            this.lblServerUrl = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.btnAddItem = new System.Windows.Forms.Button();
            this.btnAddSubscription = new System.Windows.Forms.Button();
            this.btnMoveUpItem = new System.Windows.Forms.Button();
            this.btnMoveDownItem = new System.Windows.Forms.Button();
            this.btnDeleteItem = new System.Windows.Forms.Button();
            this.tvDevice = new System.Windows.Forms.TreeView();
            this.ilDevice = new System.Windows.Forms.ImageList(this.components);
            this.gbDevice.SuspendLayout();
            this.gbServerBrowse.SuspendLayout();
            this.gbConnection.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(607, 426);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Enabled = false;
            this.btnSave.Location = new System.Drawing.Point(526, 426);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 4;
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
            this.gbDevice.Location = new System.Drawing.Point(238, 83);
            this.gbDevice.Name = "gbDevice";
            this.gbDevice.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbDevice.Size = new System.Drawing.Size(220, 337);
            this.gbDevice.TabIndex = 2;
            this.gbDevice.TabStop = false;
            this.gbDevice.Text = "Device";
            // 
            // gbServerBrowse
            // 
            this.gbServerBrowse.Controls.Add(this.tvServer);
            this.gbServerBrowse.Location = new System.Drawing.Point(12, 83);
            this.gbServerBrowse.Name = "gbServerBrowse";
            this.gbServerBrowse.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbServerBrowse.Size = new System.Drawing.Size(220, 337);
            this.gbServerBrowse.TabIndex = 1;
            this.gbServerBrowse.TabStop = false;
            this.gbServerBrowse.Text = "Server Browse";
            // 
            // tvServer
            // 
            this.tvServer.ImageIndex = 0;
            this.tvServer.ImageList = this.ilServer;
            this.tvServer.Location = new System.Drawing.Point(13, 19);
            this.tvServer.Name = "tvServer";
            this.tvServer.SelectedImageIndex = 0;
            this.tvServer.Size = new System.Drawing.Size(194, 305);
            this.tvServer.TabIndex = 0;
            this.tvServer.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvServer_BeforeExpand);
            // 
            // ilServer
            // 
            this.ilServer.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilServer.ImageStream")));
            this.ilServer.TransparentColor = System.Drawing.Color.Transparent;
            this.ilServer.Images.SetKeyName(0, "empty.png");
            this.ilServer.Images.SetKeyName(1, "method.png");
            this.ilServer.Images.SetKeyName(2, "object.png");
            this.ilServer.Images.SetKeyName(3, "variable.png");
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
            this.gbConnection.Size = new System.Drawing.Size(670, 65);
            this.gbConnection.TabIndex = 0;
            this.gbConnection.TabStop = false;
            this.gbConnection.Text = "Connection";
            // 
            // btnSecurityOptions
            // 
            this.btnSecurityOptions.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSecurityOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnSecurityOptions.Image")));
            this.btnSecurityOptions.Location = new System.Drawing.Point(634, 31);
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
            this.btnDisconnect.Location = new System.Drawing.Point(605, 31);
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
            this.btnConnect.Location = new System.Drawing.Point(576, 31);
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
            this.txtServerUrl.Size = new System.Drawing.Size(557, 20);
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
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(464, 83);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.groupBox1.Size = new System.Drawing.Size(218, 337);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Parameters";
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
            // tvDevice
            // 
            this.tvDevice.ImageIndex = 0;
            this.tvDevice.ImageList = this.ilDevice;
            this.tvDevice.Location = new System.Drawing.Point(13, 48);
            this.tvDevice.Name = "tvDevice";
            this.tvDevice.SelectedImageIndex = 0;
            this.tvDevice.Size = new System.Drawing.Size(194, 276);
            this.tvDevice.TabIndex = 5;
            this.tvDevice.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.tvDevice_AfterCollapse);
            this.tvDevice.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.tvDevice_AfterExpand);
            this.tvDevice.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvDevice_AfterSelect);
            // 
            // ilDevice
            // 
            this.ilDevice.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilDevice.ImageStream")));
            this.ilDevice.TransparentColor = System.Drawing.Color.Transparent;
            this.ilDevice.Images.SetKeyName(0, "command.png");
            this.ilDevice.Images.SetKeyName(1, "folder_closed.png");
            this.ilDevice.Images.SetKeyName(2, "folder_open.png");
            this.ilDevice.Images.SetKeyName(3, "variable.png");
            // 
            // FrmConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(694, 461);
            this.Controls.Add(this.groupBox1);
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
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.TreeView tvServer;
        private System.Windows.Forms.ImageList ilServer;
        private System.Windows.Forms.Button btnAddItem;
        private System.Windows.Forms.Button btnDeleteItem;
        private System.Windows.Forms.Button btnMoveDownItem;
        private System.Windows.Forms.Button btnMoveUpItem;
        private System.Windows.Forms.Button btnAddSubscription;
        private System.Windows.Forms.TreeView tvDevice;
        private System.Windows.Forms.ImageList ilDevice;
    }
}