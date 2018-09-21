namespace Scada.Admin.App.Forms.Deployment
{
    partial class FrmInstanceStatus
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
            this.ctrlProfileSelector = new Scada.Admin.App.Controls.Deployment.CtrlProfileSelector();
            this.gbStatus = new System.Windows.Forms.GroupBox();
            this.txtUpdateTime = new System.Windows.Forms.TextBox();
            this.lblUpdateTime = new System.Windows.Forms.Label();
            this.btnRestartComm = new System.Windows.Forms.Button();
            this.txtCommStatus = new System.Windows.Forms.TextBox();
            this.lblCommStatus = new System.Windows.Forms.Label();
            this.btnRestartServer = new System.Windows.Forms.Button();
            this.txtServerStatus = new System.Windows.Forms.TextBox();
            this.lblServerStatus = new System.Windows.Forms.Label();
            this.gbAction = new System.Windows.Forms.GroupBox();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.gbStatus.SuspendLayout();
            this.gbAction.SuspendLayout();
            this.SuspendLayout();
            // 
            // ctrlProfileSelector
            // 
            this.ctrlProfileSelector.Location = new System.Drawing.Point(12, 12);
            this.ctrlProfileSelector.Name = "ctrlProfileSelector";
            this.ctrlProfileSelector.Size = new System.Drawing.Size(469, 113);
            this.ctrlProfileSelector.TabIndex = 0;
            this.ctrlProfileSelector.SelectedProfileChanged += new System.EventHandler(this.ctrlProfileSelector_SelectedProfileChanged);
            // 
            // gbStatus
            // 
            this.gbStatus.Controls.Add(this.txtUpdateTime);
            this.gbStatus.Controls.Add(this.lblUpdateTime);
            this.gbStatus.Controls.Add(this.btnRestartComm);
            this.gbStatus.Controls.Add(this.txtCommStatus);
            this.gbStatus.Controls.Add(this.lblCommStatus);
            this.gbStatus.Controls.Add(this.btnRestartServer);
            this.gbStatus.Controls.Add(this.txtServerStatus);
            this.gbStatus.Controls.Add(this.lblServerStatus);
            this.gbStatus.Location = new System.Drawing.Point(12, 192);
            this.gbStatus.Name = "gbStatus";
            this.gbStatus.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbStatus.Size = new System.Drawing.Size(469, 111);
            this.gbStatus.TabIndex = 2;
            this.gbStatus.TabStop = false;
            this.gbStatus.Text = "Status";
            // 
            // txtUpdateTime
            // 
            this.txtUpdateTime.Location = new System.Drawing.Point(150, 78);
            this.txtUpdateTime.Name = "txtUpdateTime";
            this.txtUpdateTime.ReadOnly = true;
            this.txtUpdateTime.Size = new System.Drawing.Size(200, 20);
            this.txtUpdateTime.TabIndex = 7;
            // 
            // lblUpdateTime
            // 
            this.lblUpdateTime.AutoSize = true;
            this.lblUpdateTime.Location = new System.Drawing.Point(13, 82);
            this.lblUpdateTime.Name = "lblUpdateTime";
            this.lblUpdateTime.Size = new System.Drawing.Size(64, 13);
            this.lblUpdateTime.TabIndex = 6;
            this.lblUpdateTime.Text = "Update time";
            // 
            // btnRestartComm
            // 
            this.btnRestartComm.Location = new System.Drawing.Point(356, 48);
            this.btnRestartComm.Name = "btnRestartComm";
            this.btnRestartComm.Size = new System.Drawing.Size(100, 23);
            this.btnRestartComm.TabIndex = 5;
            this.btnRestartComm.Text = "Restart";
            this.btnRestartComm.UseVisualStyleBackColor = true;
            this.btnRestartComm.Click += new System.EventHandler(this.btnRestartComm_Click);
            // 
            // txtCommStatus
            // 
            this.txtCommStatus.Location = new System.Drawing.Point(150, 49);
            this.txtCommStatus.Name = "txtCommStatus";
            this.txtCommStatus.ReadOnly = true;
            this.txtCommStatus.Size = new System.Drawing.Size(200, 20);
            this.txtCommStatus.TabIndex = 4;
            // 
            // lblCommStatus
            // 
            this.lblCommStatus.AutoSize = true;
            this.lblCommStatus.Location = new System.Drawing.Point(13, 53);
            this.lblCommStatus.Name = "lblCommStatus";
            this.lblCommStatus.Size = new System.Drawing.Size(111, 13);
            this.lblCommStatus.TabIndex = 3;
            this.lblCommStatus.Text = "Communicator service";
            // 
            // btnRestartServer
            // 
            this.btnRestartServer.Location = new System.Drawing.Point(356, 19);
            this.btnRestartServer.Name = "btnRestartServer";
            this.btnRestartServer.Size = new System.Drawing.Size(100, 23);
            this.btnRestartServer.TabIndex = 2;
            this.btnRestartServer.Text = "Restart";
            this.btnRestartServer.UseVisualStyleBackColor = true;
            this.btnRestartServer.Click += new System.EventHandler(this.btnRestartServer_Click);
            // 
            // txtServerStatus
            // 
            this.txtServerStatus.Location = new System.Drawing.Point(150, 20);
            this.txtServerStatus.Name = "txtServerStatus";
            this.txtServerStatus.ReadOnly = true;
            this.txtServerStatus.Size = new System.Drawing.Size(200, 20);
            this.txtServerStatus.TabIndex = 1;
            // 
            // lblServerStatus
            // 
            this.lblServerStatus.AutoSize = true;
            this.lblServerStatus.Location = new System.Drawing.Point(13, 24);
            this.lblServerStatus.Name = "lblServerStatus";
            this.lblServerStatus.Size = new System.Drawing.Size(75, 13);
            this.lblServerStatus.TabIndex = 0;
            this.lblServerStatus.Text = "Server service";
            // 
            // gbAction
            // 
            this.gbAction.Controls.Add(this.btnDisconnect);
            this.gbAction.Controls.Add(this.btnConnect);
            this.gbAction.Location = new System.Drawing.Point(12, 131);
            this.gbAction.Name = "gbAction";
            this.gbAction.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbAction.Size = new System.Drawing.Size(469, 55);
            this.gbAction.TabIndex = 1;
            this.gbAction.TabStop = false;
            this.gbAction.Text = "Actions";
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Location = new System.Drawing.Point(119, 19);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(100, 23);
            this.btnDisconnect.TabIndex = 1;
            this.btnDisconnect.Text = "Disconnect";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(13, 19);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(100, 23);
            this.btnConnect.TabIndex = 0;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(406, 309);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // timer
            // 
            this.timer.Interval = 1000;
            this.timer.Tick += new System.EventHandler(this.timer_TickAsync);
            // 
            // FrmInstanceStatus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(493, 344);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.gbStatus);
            this.Controls.Add(this.gbAction);
            this.Controls.Add(this.ctrlProfileSelector);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmInstanceStatus";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Instance Status";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmInstanceStatus_FormClosed);
            this.Load += new System.EventHandler(this.FrmInstanceStatus_Load);
            this.gbStatus.ResumeLayout(false);
            this.gbStatus.PerformLayout();
            this.gbAction.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.Deployment.CtrlProfileSelector ctrlProfileSelector;
        private System.Windows.Forms.GroupBox gbStatus;
        private System.Windows.Forms.TextBox txtUpdateTime;
        private System.Windows.Forms.Label lblUpdateTime;
        private System.Windows.Forms.Button btnRestartComm;
        private System.Windows.Forms.TextBox txtCommStatus;
        private System.Windows.Forms.Label lblCommStatus;
        private System.Windows.Forms.Button btnRestartServer;
        private System.Windows.Forms.TextBox txtServerStatus;
        private System.Windows.Forms.Label lblServerStatus;
        private System.Windows.Forms.GroupBox gbAction;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Timer timer;
    }
}