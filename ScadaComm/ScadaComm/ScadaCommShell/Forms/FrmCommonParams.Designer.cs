namespace Scada.Comm.Shell.Forms
{
    partial class FrmCommonParams
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
            this.gbRuntimeOptions = new System.Windows.Forms.GroupBox();
            this.lblSendAllDataPer = new System.Windows.Forms.Label();
            this.numSendAllDataPer = new System.Windows.Forms.NumericUpDown();
            this.numWaitForStop = new System.Windows.Forms.NumericUpDown();
            this.lblWaitForStop = new System.Windows.Forms.Label();
            this.gbServerConn = new System.Windows.Forms.GroupBox();
            this.lblServerPwd = new System.Windows.Forms.Label();
            this.txtServerPwd = new System.Windows.Forms.TextBox();
            this.txtServerUser = new System.Windows.Forms.TextBox();
            this.lblServerUser = new System.Windows.Forms.Label();
            this.lblServerTimeout = new System.Windows.Forms.Label();
            this.numServerTimeout = new System.Windows.Forms.NumericUpDown();
            this.lblServerPort = new System.Windows.Forms.Label();
            this.numServerPort = new System.Windows.Forms.NumericUpDown();
            this.lblServerHost = new System.Windows.Forms.Label();
            this.chkServerUse = new System.Windows.Forms.CheckBox();
            this.txtServerHost = new System.Windows.Forms.TextBox();
            this.lblSendModData = new System.Windows.Forms.Label();
            this.chkSendModData = new System.Windows.Forms.CheckBox();
            this.gbRuntimeOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSendAllDataPer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numWaitForStop)).BeginInit();
            this.gbServerConn.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numServerTimeout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numServerPort)).BeginInit();
            this.SuspendLayout();
            // 
            // gbRuntimeOptions
            // 
            this.gbRuntimeOptions.Controls.Add(this.lblSendModData);
            this.gbRuntimeOptions.Controls.Add(this.chkSendModData);
            this.gbRuntimeOptions.Controls.Add(this.lblSendAllDataPer);
            this.gbRuntimeOptions.Controls.Add(this.numSendAllDataPer);
            this.gbRuntimeOptions.Controls.Add(this.numWaitForStop);
            this.gbRuntimeOptions.Controls.Add(this.lblWaitForStop);
            this.gbRuntimeOptions.Location = new System.Drawing.Point(12, 146);
            this.gbRuntimeOptions.Name = "gbRuntimeOptions";
            this.gbRuntimeOptions.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbRuntimeOptions.Size = new System.Drawing.Size(450, 104);
            this.gbRuntimeOptions.TabIndex = 1;
            this.gbRuntimeOptions.TabStop = false;
            this.gbRuntimeOptions.Text = "Runtime Options";
            // 
            // lblSendAllDataPer
            // 
            this.lblSendAllDataPer.AutoSize = true;
            this.lblSendAllDataPer.Location = new System.Drawing.Point(13, 75);
            this.lblSendAllDataPer.Name = "lblSendAllDataPer";
            this.lblSendAllDataPer.Size = new System.Drawing.Size(183, 13);
            this.lblSendAllDataPer.TabIndex = 4;
            this.lblSendAllDataPer.Text = "Period of sending all device tags, sec";
            // 
            // numSendAllDataPer
            // 
            this.numSendAllDataPer.Location = new System.Drawing.Point(337, 71);
            this.numSendAllDataPer.Maximum = new decimal(new int[] {
            3600,
            0,
            0,
            0});
            this.numSendAllDataPer.Name = "numSendAllDataPer";
            this.numSendAllDataPer.Size = new System.Drawing.Size(100, 20);
            this.numSendAllDataPer.TabIndex = 5;
            this.numSendAllDataPer.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.numSendAllDataPer.ValueChanged += new System.EventHandler(this.control_Changed);
            // 
            // numWaitForStop
            // 
            this.numWaitForStop.Increment = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numWaitForStop.Location = new System.Drawing.Point(337, 19);
            this.numWaitForStop.Maximum = new decimal(new int[] {
            600000,
            0,
            0,
            0});
            this.numWaitForStop.Name = "numWaitForStop";
            this.numWaitForStop.Size = new System.Drawing.Size(100, 20);
            this.numWaitForStop.TabIndex = 1;
            this.numWaitForStop.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numWaitForStop.ValueChanged += new System.EventHandler(this.control_Changed);
            // 
            // lblWaitForStop
            // 
            this.lblWaitForStop.AutoSize = true;
            this.lblWaitForStop.Location = new System.Drawing.Point(13, 23);
            this.lblWaitForStop.Name = "lblWaitForStop";
            this.lblWaitForStop.Size = new System.Drawing.Size(215, 13);
            this.lblWaitForStop.TabIndex = 0;
            this.lblWaitForStop.Text = "Wait for communication lines termination, ms";
            // 
            // gbServerConn
            // 
            this.gbServerConn.Controls.Add(this.lblServerPwd);
            this.gbServerConn.Controls.Add(this.txtServerPwd);
            this.gbServerConn.Controls.Add(this.txtServerUser);
            this.gbServerConn.Controls.Add(this.lblServerUser);
            this.gbServerConn.Controls.Add(this.lblServerTimeout);
            this.gbServerConn.Controls.Add(this.numServerTimeout);
            this.gbServerConn.Controls.Add(this.lblServerPort);
            this.gbServerConn.Controls.Add(this.numServerPort);
            this.gbServerConn.Controls.Add(this.lblServerHost);
            this.gbServerConn.Controls.Add(this.chkServerUse);
            this.gbServerConn.Controls.Add(this.txtServerHost);
            this.gbServerConn.Location = new System.Drawing.Point(12, 12);
            this.gbServerConn.Name = "gbServerConn";
            this.gbServerConn.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbServerConn.Size = new System.Drawing.Size(450, 128);
            this.gbServerConn.TabIndex = 0;
            this.gbServerConn.TabStop = false;
            this.gbServerConn.Text = "Server Connection";
            // 
            // lblServerPwd
            // 
            this.lblServerPwd.AutoSize = true;
            this.lblServerPwd.Location = new System.Drawing.Point(228, 79);
            this.lblServerPwd.Name = "lblServerPwd";
            this.lblServerPwd.Size = new System.Drawing.Size(53, 13);
            this.lblServerPwd.TabIndex = 9;
            this.lblServerPwd.Text = "Password";
            // 
            // txtServerPwd
            // 
            this.txtServerPwd.Location = new System.Drawing.Point(231, 95);
            this.txtServerPwd.Name = "txtServerPwd";
            this.txtServerPwd.Size = new System.Drawing.Size(206, 20);
            this.txtServerPwd.TabIndex = 10;
            this.txtServerPwd.Text = "12345";
            this.txtServerPwd.UseSystemPasswordChar = true;
            this.txtServerPwd.TextChanged += new System.EventHandler(this.control_Changed);
            // 
            // txtServerUser
            // 
            this.txtServerUser.Location = new System.Drawing.Point(13, 95);
            this.txtServerUser.Name = "txtServerUser";
            this.txtServerUser.Size = new System.Drawing.Size(212, 20);
            this.txtServerUser.TabIndex = 8;
            this.txtServerUser.Text = "ScadaComm";
            this.txtServerUser.TextChanged += new System.EventHandler(this.control_Changed);
            // 
            // lblServerUser
            // 
            this.lblServerUser.AutoSize = true;
            this.lblServerUser.Location = new System.Drawing.Point(10, 79);
            this.lblServerUser.Name = "lblServerUser";
            this.lblServerUser.Size = new System.Drawing.Size(29, 13);
            this.lblServerUser.TabIndex = 7;
            this.lblServerUser.Text = "User";
            // 
            // lblServerTimeout
            // 
            this.lblServerTimeout.AutoSize = true;
            this.lblServerTimeout.Location = new System.Drawing.Point(334, 39);
            this.lblServerTimeout.Name = "lblServerTimeout";
            this.lblServerTimeout.Size = new System.Drawing.Size(45, 13);
            this.lblServerTimeout.TabIndex = 5;
            this.lblServerTimeout.Text = "Timeout";
            // 
            // numServerTimeout
            // 
            this.numServerTimeout.Location = new System.Drawing.Point(337, 55);
            this.numServerTimeout.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numServerTimeout.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numServerTimeout.Name = "numServerTimeout";
            this.numServerTimeout.Size = new System.Drawing.Size(100, 20);
            this.numServerTimeout.TabIndex = 6;
            this.numServerTimeout.Value = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numServerTimeout.ValueChanged += new System.EventHandler(this.control_Changed);
            // 
            // lblServerPort
            // 
            this.lblServerPort.AutoSize = true;
            this.lblServerPort.Location = new System.Drawing.Point(228, 39);
            this.lblServerPort.Name = "lblServerPort";
            this.lblServerPort.Size = new System.Drawing.Size(26, 13);
            this.lblServerPort.TabIndex = 3;
            this.lblServerPort.Text = "Port";
            // 
            // numServerPort
            // 
            this.numServerPort.Location = new System.Drawing.Point(231, 55);
            this.numServerPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numServerPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numServerPort.Name = "numServerPort";
            this.numServerPort.Size = new System.Drawing.Size(100, 20);
            this.numServerPort.TabIndex = 4;
            this.numServerPort.Value = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numServerPort.ValueChanged += new System.EventHandler(this.control_Changed);
            // 
            // lblServerHost
            // 
            this.lblServerHost.AutoSize = true;
            this.lblServerHost.Location = new System.Drawing.Point(10, 39);
            this.lblServerHost.Name = "lblServerHost";
            this.lblServerHost.Size = new System.Drawing.Size(38, 13);
            this.lblServerHost.TabIndex = 1;
            this.lblServerHost.Text = "Server";
            // 
            // chkServerUse
            // 
            this.chkServerUse.AutoSize = true;
            this.chkServerUse.Checked = true;
            this.chkServerUse.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkServerUse.Location = new System.Drawing.Point(13, 19);
            this.chkServerUse.Name = "chkServerUse";
            this.chkServerUse.Size = new System.Drawing.Size(118, 17);
            this.chkServerUse.TabIndex = 0;
            this.chkServerUse.Text = "Interact with Server";
            this.chkServerUse.UseVisualStyleBackColor = true;
            this.chkServerUse.CheckedChanged += new System.EventHandler(this.control_Changed);
            // 
            // txtServerHost
            // 
            this.txtServerHost.Location = new System.Drawing.Point(13, 55);
            this.txtServerHost.Name = "txtServerHost";
            this.txtServerHost.Size = new System.Drawing.Size(212, 20);
            this.txtServerHost.TabIndex = 2;
            this.txtServerHost.Text = "localhost";
            this.txtServerHost.TextChanged += new System.EventHandler(this.control_Changed);
            // 
            // lblSendModData
            // 
            this.lblSendModData.AutoSize = true;
            this.lblSendModData.Location = new System.Drawing.Point(13, 49);
            this.lblSendModData.Name = "lblSendModData";
            this.lblSendModData.Size = new System.Drawing.Size(154, 13);
            this.lblSendModData.TabIndex = 2;
            this.lblSendModData.Text = "Send only modified device tags";
            // 
            // chkSendModData
            // 
            this.chkSendModData.AutoSize = true;
            this.chkSendModData.Location = new System.Drawing.Point(382, 48);
            this.chkSendModData.Name = "chkSendModData";
            this.chkSendModData.Size = new System.Drawing.Size(15, 14);
            this.chkSendModData.TabIndex = 3;
            this.chkSendModData.UseVisualStyleBackColor = true;
            this.chkSendModData.CheckedChanged += new System.EventHandler(this.chkSendModData_CheckedChanged);
            // 
            // FrmCommonParams
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 411);
            this.Controls.Add(this.gbRuntimeOptions);
            this.Controls.Add(this.gbServerConn);
            this.Name = "FrmCommonParams";
            this.Text = "Common Parameters";
            this.Load += new System.EventHandler(this.FrmCommonParams_Load);
            this.gbRuntimeOptions.ResumeLayout(false);
            this.gbRuntimeOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSendAllDataPer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numWaitForStop)).EndInit();
            this.gbServerConn.ResumeLayout(false);
            this.gbServerConn.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numServerTimeout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numServerPort)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbRuntimeOptions;
        private System.Windows.Forms.Label lblSendAllDataPer;
        private System.Windows.Forms.NumericUpDown numSendAllDataPer;
        private System.Windows.Forms.NumericUpDown numWaitForStop;
        private System.Windows.Forms.Label lblWaitForStop;
        private System.Windows.Forms.GroupBox gbServerConn;
        private System.Windows.Forms.Label lblServerPwd;
        private System.Windows.Forms.TextBox txtServerPwd;
        private System.Windows.Forms.TextBox txtServerUser;
        private System.Windows.Forms.Label lblServerUser;
        private System.Windows.Forms.Label lblServerTimeout;
        private System.Windows.Forms.NumericUpDown numServerTimeout;
        private System.Windows.Forms.Label lblServerPort;
        private System.Windows.Forms.NumericUpDown numServerPort;
        private System.Windows.Forms.Label lblServerHost;
        private System.Windows.Forms.CheckBox chkServerUse;
        private System.Windows.Forms.TextBox txtServerHost;
        private System.Windows.Forms.Label lblSendModData;
        private System.Windows.Forms.CheckBox chkSendModData;
    }
}