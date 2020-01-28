namespace Scada.Admin.App.Controls.Deployment
{
    partial class CtrlTransferSettings
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.gbOptions = new System.Windows.Forms.GroupBox();
            this.btnSelectObj = new System.Windows.Forms.Button();
            this.txtObjFilter = new System.Windows.Forms.TextBox();
            this.lblObjFilter = new System.Windows.Forms.Label();
            this.chkIgnoreWebStorage = new System.Windows.Forms.CheckBox();
            this.chkIgnoreRegKeys = new System.Windows.Forms.CheckBox();
            this.lblIgnore = new System.Windows.Forms.Label();
            this.lblInclude = new System.Windows.Forms.Label();
            this.chkIncludeWeb = new System.Windows.Forms.CheckBox();
            this.chkRestartComm = new System.Windows.Forms.CheckBox();
            this.chkIncludeComm = new System.Windows.Forms.CheckBox();
            this.chkRestartServer = new System.Windows.Forms.CheckBox();
            this.chkIncludeServer = new System.Windows.Forms.CheckBox();
            this.chkIncludeInterface = new System.Windows.Forms.CheckBox();
            this.chkIncludeBase = new System.Windows.Forms.CheckBox();
            this.gbOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbOptions
            // 
            this.gbOptions.Controls.Add(this.btnSelectObj);
            this.gbOptions.Controls.Add(this.txtObjFilter);
            this.gbOptions.Controls.Add(this.lblObjFilter);
            this.gbOptions.Controls.Add(this.chkIgnoreWebStorage);
            this.gbOptions.Controls.Add(this.chkIgnoreRegKeys);
            this.gbOptions.Controls.Add(this.lblIgnore);
            this.gbOptions.Controls.Add(this.lblInclude);
            this.gbOptions.Controls.Add(this.chkIncludeWeb);
            this.gbOptions.Controls.Add(this.chkRestartComm);
            this.gbOptions.Controls.Add(this.chkIncludeComm);
            this.gbOptions.Controls.Add(this.chkRestartServer);
            this.gbOptions.Controls.Add(this.chkIncludeServer);
            this.gbOptions.Controls.Add(this.chkIncludeInterface);
            this.gbOptions.Controls.Add(this.chkIncludeBase);
            this.gbOptions.Location = new System.Drawing.Point(0, 0);
            this.gbOptions.Name = "gbOptions";
            this.gbOptions.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbOptions.Size = new System.Drawing.Size(469, 272);
            this.gbOptions.TabIndex = 0;
            this.gbOptions.TabStop = false;
            this.gbOptions.Text = "Options";
            // 
            // btnSelectObj
            // 
            this.btnSelectObj.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectObj.Location = new System.Drawing.Point(381, 238);
            this.btnSelectObj.Name = "btnSelectObj";
            this.btnSelectObj.Size = new System.Drawing.Size(75, 23);
            this.btnSelectObj.TabIndex = 13;
            this.btnSelectObj.Text = "Select...";
            this.btnSelectObj.UseVisualStyleBackColor = true;
            this.btnSelectObj.Click += new System.EventHandler(this.btnSelectObj_Click);
            // 
            // txtObjFilter
            // 
            this.txtObjFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtObjFilter.Location = new System.Drawing.Point(13, 239);
            this.txtObjFilter.Name = "txtObjFilter";
            this.txtObjFilter.Size = new System.Drawing.Size(362, 20);
            this.txtObjFilter.TabIndex = 12;
            this.txtObjFilter.TextChanged += new System.EventHandler(this.control_Changed);
            // 
            // lblObjFilter
            // 
            this.lblObjFilter.AutoSize = true;
            this.lblObjFilter.Location = new System.Drawing.Point(10, 223);
            this.lblObjFilter.Name = "lblObjFilter";
            this.lblObjFilter.Size = new System.Drawing.Size(63, 13);
            this.lblObjFilter.TabIndex = 11;
            this.lblObjFilter.Text = "Object filter:";
            // 
            // chkIgnoreWebStorage
            // 
            this.chkIgnoreWebStorage.AutoSize = true;
            this.chkIgnoreWebStorage.Enabled = false;
            this.chkIgnoreWebStorage.Location = new System.Drawing.Point(13, 193);
            this.chkIgnoreWebStorage.Name = "chkIgnoreWebStorage";
            this.chkIgnoreWebStorage.Size = new System.Drawing.Size(118, 17);
            this.chkIgnoreWebStorage.TabIndex = 10;
            this.chkIgnoreWebStorage.Text = "Webstation storage";
            this.chkIgnoreWebStorage.UseVisualStyleBackColor = true;
            this.chkIgnoreWebStorage.CheckedChanged += new System.EventHandler(this.control_Changed);
            // 
            // chkIgnoreRegKeys
            // 
            this.chkIgnoreRegKeys.AutoSize = true;
            this.chkIgnoreRegKeys.Location = new System.Drawing.Point(13, 170);
            this.chkIgnoreRegKeys.Name = "chkIgnoreRegKeys";
            this.chkIgnoreRegKeys.Size = new System.Drawing.Size(107, 17);
            this.chkIgnoreRegKeys.TabIndex = 9;
            this.chkIgnoreRegKeys.Text = "Registration keys";
            this.chkIgnoreRegKeys.UseVisualStyleBackColor = true;
            this.chkIgnoreRegKeys.CheckedChanged += new System.EventHandler(this.control_Changed);
            // 
            // lblIgnore
            // 
            this.lblIgnore.AutoSize = true;
            this.lblIgnore.Location = new System.Drawing.Point(10, 154);
            this.lblIgnore.Name = "lblIgnore";
            this.lblIgnore.Size = new System.Drawing.Size(40, 13);
            this.lblIgnore.TabIndex = 8;
            this.lblIgnore.Text = "Ignore:";
            // 
            // lblInclude
            // 
            this.lblInclude.AutoSize = true;
            this.lblInclude.Location = new System.Drawing.Point(10, 16);
            this.lblInclude.Name = "lblInclude";
            this.lblInclude.Size = new System.Drawing.Size(45, 13);
            this.lblInclude.TabIndex = 0;
            this.lblInclude.Text = "Include:";
            // 
            // chkIncludeWeb
            // 
            this.chkIncludeWeb.AutoSize = true;
            this.chkIncludeWeb.Location = new System.Drawing.Point(13, 124);
            this.chkIncludeWeb.Name = "chkIncludeWeb";
            this.chkIncludeWeb.Size = new System.Drawing.Size(80, 17);
            this.chkIncludeWeb.TabIndex = 7;
            this.chkIncludeWeb.Text = "Webstation";
            this.chkIncludeWeb.UseVisualStyleBackColor = true;
            this.chkIncludeWeb.CheckedChanged += new System.EventHandler(this.control_Changed);
            // 
            // chkRestartComm
            // 
            this.chkRestartComm.AutoSize = true;
            this.chkRestartComm.Location = new System.Drawing.Point(150, 101);
            this.chkRestartComm.Name = "chkRestartComm";
            this.chkRestartComm.Size = new System.Drawing.Size(130, 17);
            this.chkRestartComm.TabIndex = 6;
            this.chkRestartComm.Text = "Restart Communicator";
            this.chkRestartComm.UseVisualStyleBackColor = true;
            this.chkRestartComm.CheckedChanged += new System.EventHandler(this.control_Changed);
            // 
            // chkIncludeComm
            // 
            this.chkIncludeComm.AutoSize = true;
            this.chkIncludeComm.Location = new System.Drawing.Point(13, 101);
            this.chkIncludeComm.Name = "chkIncludeComm";
            this.chkIncludeComm.Size = new System.Drawing.Size(93, 17);
            this.chkIncludeComm.TabIndex = 5;
            this.chkIncludeComm.Text = "Communicator";
            this.chkIncludeComm.UseVisualStyleBackColor = true;
            this.chkIncludeComm.CheckedChanged += new System.EventHandler(this.control_Changed);
            // 
            // chkRestartServer
            // 
            this.chkRestartServer.AutoSize = true;
            this.chkRestartServer.Location = new System.Drawing.Point(150, 78);
            this.chkRestartServer.Name = "chkRestartServer";
            this.chkRestartServer.Size = new System.Drawing.Size(94, 17);
            this.chkRestartServer.TabIndex = 4;
            this.chkRestartServer.Text = "Restart Server";
            this.chkRestartServer.UseVisualStyleBackColor = true;
            this.chkRestartServer.CheckedChanged += new System.EventHandler(this.control_Changed);
            // 
            // chkIncludeServer
            // 
            this.chkIncludeServer.AutoSize = true;
            this.chkIncludeServer.Location = new System.Drawing.Point(13, 78);
            this.chkIncludeServer.Name = "chkIncludeServer";
            this.chkIncludeServer.Size = new System.Drawing.Size(57, 17);
            this.chkIncludeServer.TabIndex = 3;
            this.chkIncludeServer.Text = "Server";
            this.chkIncludeServer.UseVisualStyleBackColor = true;
            this.chkIncludeServer.CheckedChanged += new System.EventHandler(this.control_Changed);
            // 
            // chkIncludeInterface
            // 
            this.chkIncludeInterface.AutoSize = true;
            this.chkIncludeInterface.Location = new System.Drawing.Point(13, 55);
            this.chkIncludeInterface.Name = "chkIncludeInterface";
            this.chkIncludeInterface.Size = new System.Drawing.Size(68, 17);
            this.chkIncludeInterface.TabIndex = 2;
            this.chkIncludeInterface.Text = "Interface";
            this.chkIncludeInterface.UseVisualStyleBackColor = true;
            this.chkIncludeInterface.CheckedChanged += new System.EventHandler(this.control_Changed);
            // 
            // chkIncludeBase
            // 
            this.chkIncludeBase.AutoSize = true;
            this.chkIncludeBase.Location = new System.Drawing.Point(13, 32);
            this.chkIncludeBase.Name = "chkIncludeBase";
            this.chkIncludeBase.Size = new System.Drawing.Size(135, 17);
            this.chkIncludeBase.TabIndex = 1;
            this.chkIncludeBase.Text = "Configuration database";
            this.chkIncludeBase.UseVisualStyleBackColor = true;
            this.chkIncludeBase.CheckedChanged += new System.EventHandler(this.control_Changed);
            // 
            // CtrlTransferSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbOptions);
            this.Name = "CtrlTransferSettings";
            this.Size = new System.Drawing.Size(469, 272);
            this.gbOptions.ResumeLayout(false);
            this.gbOptions.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbOptions;
        private System.Windows.Forms.CheckBox chkIgnoreWebStorage;
        private System.Windows.Forms.CheckBox chkIgnoreRegKeys;
        private System.Windows.Forms.Label lblIgnore;
        private System.Windows.Forms.Label lblInclude;
        private System.Windows.Forms.CheckBox chkIncludeWeb;
        private System.Windows.Forms.CheckBox chkIncludeComm;
        private System.Windows.Forms.CheckBox chkIncludeServer;
        private System.Windows.Forms.CheckBox chkIncludeInterface;
        private System.Windows.Forms.CheckBox chkIncludeBase;
        private System.Windows.Forms.TextBox txtObjFilter;
        private System.Windows.Forms.Label lblObjFilter;
        private System.Windows.Forms.CheckBox chkRestartComm;
        private System.Windows.Forms.CheckBox chkRestartServer;
        private System.Windows.Forms.Button btnSelectObj;
    }
}
