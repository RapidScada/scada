namespace Scada.Server.Shell.Forms
{
    partial class FrmGenerator
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmGenerator));
            this.gbServerConn = new System.Windows.Forms.GroupBox();
            this.txtServerTimeout = new System.Windows.Forms.TextBox();
            this.txtServerPort = new System.Windows.Forms.TextBox();
            this.lblServerPwd = new System.Windows.Forms.Label();
            this.txtServerPwd = new System.Windows.Forms.TextBox();
            this.txtServerUser = new System.Windows.Forms.TextBox();
            this.lblServerUser = new System.Windows.Forms.Label();
            this.lblServerTimeout = new System.Windows.Forms.Label();
            this.lblServerPort = new System.Windows.Forms.Label();
            this.lblServerHost = new System.Windows.Forms.Label();
            this.txtServerHost = new System.Windows.Forms.TextBox();
            this.gbGenerator = new System.Windows.Forms.GroupBox();
            this.btnGenerateCmd = new System.Windows.Forms.Button();
            this.btnGenerateEvent = new System.Windows.Forms.Button();
            this.btnGenerateData = new System.Windows.Forms.Button();
            this.pnlWarning = new System.Windows.Forms.Panel();
            this.lblWarning = new System.Windows.Forms.Label();
            this.pbWarning = new System.Windows.Forms.PictureBox();
            this.gbServerConn.SuspendLayout();
            this.gbGenerator.SuspendLayout();
            this.pnlWarning.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbWarning)).BeginInit();
            this.SuspendLayout();
            // 
            // gbServerConn
            // 
            this.gbServerConn.Controls.Add(this.txtServerTimeout);
            this.gbServerConn.Controls.Add(this.txtServerPort);
            this.gbServerConn.Controls.Add(this.lblServerPwd);
            this.gbServerConn.Controls.Add(this.txtServerPwd);
            this.gbServerConn.Controls.Add(this.txtServerUser);
            this.gbServerConn.Controls.Add(this.lblServerUser);
            this.gbServerConn.Controls.Add(this.lblServerTimeout);
            this.gbServerConn.Controls.Add(this.lblServerPort);
            this.gbServerConn.Controls.Add(this.lblServerHost);
            this.gbServerConn.Controls.Add(this.txtServerHost);
            this.gbServerConn.Location = new System.Drawing.Point(12, 12);
            this.gbServerConn.Name = "gbServerConn";
            this.gbServerConn.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbServerConn.Size = new System.Drawing.Size(450, 104);
            this.gbServerConn.TabIndex = 0;
            this.gbServerConn.TabStop = false;
            this.gbServerConn.Text = "Server Connection";
            // 
            // txtServerTimeout
            // 
            this.txtServerTimeout.Location = new System.Drawing.Point(337, 32);
            this.txtServerTimeout.Name = "txtServerTimeout";
            this.txtServerTimeout.ReadOnly = true;
            this.txtServerTimeout.Size = new System.Drawing.Size(100, 20);
            this.txtServerTimeout.TabIndex = 5;
            // 
            // txtServerPort
            // 
            this.txtServerPort.Location = new System.Drawing.Point(231, 32);
            this.txtServerPort.Name = "txtServerPort";
            this.txtServerPort.ReadOnly = true;
            this.txtServerPort.Size = new System.Drawing.Size(100, 20);
            this.txtServerPort.TabIndex = 3;
            // 
            // lblServerPwd
            // 
            this.lblServerPwd.AutoSize = true;
            this.lblServerPwd.Location = new System.Drawing.Point(228, 55);
            this.lblServerPwd.Name = "lblServerPwd";
            this.lblServerPwd.Size = new System.Drawing.Size(53, 13);
            this.lblServerPwd.TabIndex = 8;
            this.lblServerPwd.Text = "Password";
            // 
            // txtServerPwd
            // 
            this.txtServerPwd.Location = new System.Drawing.Point(231, 71);
            this.txtServerPwd.Name = "txtServerPwd";
            this.txtServerPwd.ReadOnly = true;
            this.txtServerPwd.Size = new System.Drawing.Size(206, 20);
            this.txtServerPwd.TabIndex = 9;
            this.txtServerPwd.UseSystemPasswordChar = true;
            // 
            // txtServerUser
            // 
            this.txtServerUser.Location = new System.Drawing.Point(13, 71);
            this.txtServerUser.Name = "txtServerUser";
            this.txtServerUser.ReadOnly = true;
            this.txtServerUser.Size = new System.Drawing.Size(212, 20);
            this.txtServerUser.TabIndex = 7;
            // 
            // lblServerUser
            // 
            this.lblServerUser.AutoSize = true;
            this.lblServerUser.Location = new System.Drawing.Point(10, 55);
            this.lblServerUser.Name = "lblServerUser";
            this.lblServerUser.Size = new System.Drawing.Size(29, 13);
            this.lblServerUser.TabIndex = 6;
            this.lblServerUser.Text = "User";
            // 
            // lblServerTimeout
            // 
            this.lblServerTimeout.AutoSize = true;
            this.lblServerTimeout.Location = new System.Drawing.Point(334, 16);
            this.lblServerTimeout.Name = "lblServerTimeout";
            this.lblServerTimeout.Size = new System.Drawing.Size(45, 13);
            this.lblServerTimeout.TabIndex = 4;
            this.lblServerTimeout.Text = "Timeout";
            // 
            // lblServerPort
            // 
            this.lblServerPort.AutoSize = true;
            this.lblServerPort.Location = new System.Drawing.Point(228, 16);
            this.lblServerPort.Name = "lblServerPort";
            this.lblServerPort.Size = new System.Drawing.Size(26, 13);
            this.lblServerPort.TabIndex = 2;
            this.lblServerPort.Text = "Port";
            // 
            // lblServerHost
            // 
            this.lblServerHost.AutoSize = true;
            this.lblServerHost.Location = new System.Drawing.Point(10, 16);
            this.lblServerHost.Name = "lblServerHost";
            this.lblServerHost.Size = new System.Drawing.Size(38, 13);
            this.lblServerHost.TabIndex = 0;
            this.lblServerHost.Text = "Server";
            // 
            // txtServerHost
            // 
            this.txtServerHost.Location = new System.Drawing.Point(13, 32);
            this.txtServerHost.Name = "txtServerHost";
            this.txtServerHost.ReadOnly = true;
            this.txtServerHost.Size = new System.Drawing.Size(212, 20);
            this.txtServerHost.TabIndex = 1;
            // 
            // gbGenerator
            // 
            this.gbGenerator.Controls.Add(this.btnGenerateCmd);
            this.gbGenerator.Controls.Add(this.btnGenerateEvent);
            this.gbGenerator.Controls.Add(this.btnGenerateData);
            this.gbGenerator.Location = new System.Drawing.Point(12, 122);
            this.gbGenerator.Name = "gbGenerator";
            this.gbGenerator.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbGenerator.Size = new System.Drawing.Size(450, 55);
            this.gbGenerator.TabIndex = 1;
            this.gbGenerator.TabStop = false;
            this.gbGenerator.Text = "Generator";
            // 
            // btnGenerateCmd
            // 
            this.btnGenerateCmd.Location = new System.Drawing.Point(299, 19);
            this.btnGenerateCmd.Name = "btnGenerateCmd";
            this.btnGenerateCmd.Size = new System.Drawing.Size(138, 23);
            this.btnGenerateCmd.TabIndex = 2;
            this.btnGenerateCmd.Text = "Command";
            this.btnGenerateCmd.UseVisualStyleBackColor = true;
            this.btnGenerateCmd.Click += new System.EventHandler(this.btnGenerateCmd_Click);
            // 
            // btnGenerateEvent
            // 
            this.btnGenerateEvent.Location = new System.Drawing.Point(156, 19);
            this.btnGenerateEvent.Name = "btnGenerateEvent";
            this.btnGenerateEvent.Size = new System.Drawing.Size(137, 23);
            this.btnGenerateEvent.TabIndex = 1;
            this.btnGenerateEvent.Text = "Event";
            this.btnGenerateEvent.UseVisualStyleBackColor = true;
            this.btnGenerateEvent.Click += new System.EventHandler(this.btnGenerateEvent_Click);
            // 
            // btnGenerateData
            // 
            this.btnGenerateData.Location = new System.Drawing.Point(13, 19);
            this.btnGenerateData.Name = "btnGenerateData";
            this.btnGenerateData.Size = new System.Drawing.Size(137, 23);
            this.btnGenerateData.TabIndex = 0;
            this.btnGenerateData.Text = "Data";
            this.btnGenerateData.UseVisualStyleBackColor = true;
            this.btnGenerateData.Click += new System.EventHandler(this.btnGenerateData_Click);
            // 
            // pnlWarning
            // 
            this.pnlWarning.Controls.Add(this.lblWarning);
            this.pnlWarning.Controls.Add(this.pbWarning);
            this.pnlWarning.Location = new System.Drawing.Point(12, 183);
            this.pnlWarning.Name = "pnlWarning";
            this.pnlWarning.Size = new System.Drawing.Size(450, 19);
            this.pnlWarning.TabIndex = 3;
            this.pnlWarning.Visible = false;
            // 
            // lblWarning
            // 
            this.lblWarning.AutoSize = true;
            this.lblWarning.Location = new System.Drawing.Point(22, 3);
            this.lblWarning.Name = "lblWarning";
            this.lblWarning.Size = new System.Drawing.Size(57, 13);
            this.lblWarning.TabIndex = 1;
            this.lblWarning.Text = "lblWarning";
            // 
            // pbWarning
            // 
            this.pbWarning.Image = ((System.Drawing.Image)(resources.GetObject("pbWarning.Image")));
            this.pbWarning.Location = new System.Drawing.Point(0, 1);
            this.pbWarning.Name = "pbWarning";
            this.pbWarning.Size = new System.Drawing.Size(16, 16);
            this.pbWarning.TabIndex = 0;
            this.pbWarning.TabStop = false;
            // 
            // FrmGenerator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 411);
            this.Controls.Add(this.pnlWarning);
            this.Controls.Add(this.gbGenerator);
            this.Controls.Add(this.gbServerConn);
            this.Name = "FrmGenerator";
            this.Text = "Generator";
            this.Load += new System.EventHandler(this.FrmGenerator_Load);
            this.gbServerConn.ResumeLayout(false);
            this.gbServerConn.PerformLayout();
            this.gbGenerator.ResumeLayout(false);
            this.pnlWarning.ResumeLayout(false);
            this.pnlWarning.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbWarning)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbServerConn;
        private System.Windows.Forms.Label lblServerPwd;
        private System.Windows.Forms.TextBox txtServerPwd;
        private System.Windows.Forms.TextBox txtServerUser;
        private System.Windows.Forms.Label lblServerUser;
        private System.Windows.Forms.Label lblServerTimeout;
        private System.Windows.Forms.Label lblServerPort;
        private System.Windows.Forms.Label lblServerHost;
        private System.Windows.Forms.TextBox txtServerHost;
        private System.Windows.Forms.GroupBox gbGenerator;
        private System.Windows.Forms.Button btnGenerateCmd;
        private System.Windows.Forms.Button btnGenerateEvent;
        private System.Windows.Forms.Button btnGenerateData;
        private System.Windows.Forms.TextBox txtServerTimeout;
        private System.Windows.Forms.TextBox txtServerPort;
        private System.Windows.Forms.Panel pnlWarning;
        private System.Windows.Forms.Label lblWarning;
        private System.Windows.Forms.PictureBox pbWarning;
    }
}