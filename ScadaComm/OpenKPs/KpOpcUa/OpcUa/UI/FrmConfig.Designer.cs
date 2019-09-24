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
            this.gbConnection = new System.Windows.Forms.GroupBox();
            this.btnSecurityOptions = new System.Windows.Forms.Button();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.txtServerUrl = new System.Windows.Forms.TextBox();
            this.lblServerUrl = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.tvServer = new System.Windows.Forms.TreeView();
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
            // 
            // gbDevice
            // 
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
            // tvServer
            // 
            this.tvServer.Location = new System.Drawing.Point(13, 19);
            this.tvServer.Name = "tvServer";
            this.tvServer.Size = new System.Drawing.Size(194, 305);
            this.tvServer.TabIndex = 0;
            // 
            // FrmConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
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
            this.Load += new System.EventHandler(this.FrmConfig_Load);
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
    }
}