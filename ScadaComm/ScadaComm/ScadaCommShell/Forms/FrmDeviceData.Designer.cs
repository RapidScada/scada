namespace Scada.Comm.Shell.Forms
{
    partial class FrmDeviceData
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
            this.lbDeviceData = new System.Windows.Forms.ListBox();
            this.btnSendCommand = new System.Windows.Forms.Button();
            this.lblCommandInfo = new System.Windows.Forms.Label();
            this.tmrRefresh = new System.Windows.Forms.Timer(this.components);
            this.btnDeviceProps = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbDeviceData
            // 
            this.lbDeviceData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbDeviceData.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbDeviceData.FormattingEnabled = true;
            this.lbDeviceData.HorizontalScrollbar = true;
            this.lbDeviceData.IntegralHeight = false;
            this.lbDeviceData.Location = new System.Drawing.Point(12, 41);
            this.lbDeviceData.Name = "lbDeviceData";
            this.lbDeviceData.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbDeviceData.Size = new System.Drawing.Size(660, 358);
            this.lbDeviceData.TabIndex = 2;
            // 
            // btnSendCommand
            // 
            this.btnSendCommand.Location = new System.Drawing.Point(93, 12);
            this.btnSendCommand.Name = "btnSendCommand";
            this.btnSendCommand.Size = new System.Drawing.Size(75, 23);
            this.btnSendCommand.TabIndex = 0;
            this.btnSendCommand.Text = "Command";
            this.btnSendCommand.UseVisualStyleBackColor = true;
            this.btnSendCommand.Click += new System.EventHandler(this.btnSendCommand_Click);
            // 
            // lblCommandInfo
            // 
            this.lblCommandInfo.AutoSize = true;
            this.lblCommandInfo.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblCommandInfo.Location = new System.Drawing.Point(174, 17);
            this.lblCommandInfo.Name = "lblCommandInfo";
            this.lblCommandInfo.Size = new System.Drawing.Size(323, 13);
            this.lblCommandInfo.TabIndex = 1;
            this.lblCommandInfo.Text = "Sending direct commands to Communicator is available only locally.";
            // 
            // tmrRefresh
            // 
            this.tmrRefresh.Interval = 500;
            this.tmrRefresh.Tick += new System.EventHandler(this.tmrRefresh_Tick);
            // 
            // btnDeviceProps
            // 
            this.btnDeviceProps.Location = new System.Drawing.Point(12, 12);
            this.btnDeviceProps.Name = "btnDeviceProps";
            this.btnDeviceProps.Size = new System.Drawing.Size(75, 23);
            this.btnDeviceProps.TabIndex = 3;
            this.btnDeviceProps.Text = "Properties";
            this.btnDeviceProps.UseVisualStyleBackColor = true;
            this.btnDeviceProps.Click += new System.EventHandler(this.btnDeviceProps_Click);
            // 
            // FrmDeviceData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 411);
            this.Controls.Add(this.btnDeviceProps);
            this.Controls.Add(this.lblCommandInfo);
            this.Controls.Add(this.btnSendCommand);
            this.Controls.Add(this.lbDeviceData);
            this.Name = "FrmDeviceData";
            this.Text = "Device {0} Data";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmDeviceData_FormClosed);
            this.Load += new System.EventHandler(this.FrmDeviceData_Load);
            this.VisibleChanged += new System.EventHandler(this.FrmDeviceData_VisibleChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lbDeviceData;
        private System.Windows.Forms.Button btnSendCommand;
        private System.Windows.Forms.Label lblCommandInfo;
        private System.Windows.Forms.Timer tmrRefresh;
        private System.Windows.Forms.Button btnDeviceProps;
    }
}