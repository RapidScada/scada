namespace Scada.Server.Shell.Forms
{
    partial class FrmStats
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
            this.lblState = new System.Windows.Forms.Label();
            this.rtbState = new System.Windows.Forms.RichTextBox();
            this.lblLog = new System.Windows.Forms.Label();
            this.chkPause = new System.Windows.Forms.CheckBox();
            this.rtbLog = new System.Windows.Forms.RichTextBox();
            this.pnlState = new System.Windows.Forms.Panel();
            this.pnlLog = new System.Windows.Forms.Panel();
            this.tmrRefresh = new System.Windows.Forms.Timer(this.components);
            this.pnlState.SuspendLayout();
            this.pnlLog.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblState
            // 
            this.lblState.AutoSize = true;
            this.lblState.Location = new System.Drawing.Point(9, 9);
            this.lblState.Name = "lblState";
            this.lblState.Size = new System.Drawing.Size(64, 13);
            this.lblState.TabIndex = 0;
            this.lblState.Text = "Server state";
            // 
            // rtbState
            // 
            this.rtbState.BackColor = System.Drawing.SystemColors.Window;
            this.rtbState.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbState.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbState.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rtbState.Location = new System.Drawing.Point(0, 0);
            this.rtbState.Name = "rtbState";
            this.rtbState.ReadOnly = true;
            this.rtbState.Size = new System.Drawing.Size(658, 148);
            this.rtbState.TabIndex = 0;
            this.rtbState.Text = "";
            this.rtbState.WordWrap = false;
            // 
            // lblLog
            // 
            this.lblLog.AutoSize = true;
            this.lblLog.Location = new System.Drawing.Point(9, 188);
            this.lblLog.Name = "lblLog";
            this.lblLog.Size = new System.Drawing.Size(55, 13);
            this.lblLog.TabIndex = 2;
            this.lblLog.Text = "Server log";
            // 
            // chkPause
            // 
            this.chkPause.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkPause.AutoSize = true;
            this.chkPause.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkPause.Location = new System.Drawing.Point(616, 184);
            this.chkPause.Name = "chkPause";
            this.chkPause.Size = new System.Drawing.Size(56, 17);
            this.chkPause.TabIndex = 3;
            this.chkPause.Text = "Pause";
            this.chkPause.UseVisualStyleBackColor = true;
            // 
            // rtbLog
            // 
            this.rtbLog.BackColor = System.Drawing.SystemColors.Window;
            this.rtbLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbLog.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rtbLog.Location = new System.Drawing.Point(0, 0);
            this.rtbLog.Name = "rtbLog";
            this.rtbLog.ReadOnly = true;
            this.rtbLog.Size = new System.Drawing.Size(658, 193);
            this.rtbLog.TabIndex = 0;
            this.rtbLog.Text = "";
            this.rtbLog.WordWrap = false;
            // 
            // pnlState
            // 
            this.pnlState.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlState.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlState.Controls.Add(this.rtbState);
            this.pnlState.Location = new System.Drawing.Point(12, 25);
            this.pnlState.Name = "pnlState";
            this.pnlState.Size = new System.Drawing.Size(660, 150);
            this.pnlState.TabIndex = 1;
            // 
            // pnlLog
            // 
            this.pnlLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlLog.Controls.Add(this.rtbLog);
            this.pnlLog.Location = new System.Drawing.Point(12, 204);
            this.pnlLog.Name = "pnlLog";
            this.pnlLog.Size = new System.Drawing.Size(660, 195);
            this.pnlLog.TabIndex = 4;
            // 
            // tmrRefresh
            // 
            this.tmrRefresh.Interval = 500;
            this.tmrRefresh.Tick += new System.EventHandler(this.tmrRefresh_Tick);
            // 
            // FrmStats
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 411);
            this.Controls.Add(this.lblState);
            this.Controls.Add(this.pnlState);
            this.Controls.Add(this.lblLog);
            this.Controls.Add(this.chkPause);
            this.Controls.Add(this.pnlLog);
            this.Name = "FrmStats";
            this.Text = "Stats";
            this.Load += new System.EventHandler(this.FrmStats_Load);
            this.pnlState.ResumeLayout(false);
            this.pnlLog.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblState;
        private System.Windows.Forms.RichTextBox rtbState;
        private System.Windows.Forms.Label lblLog;
        private System.Windows.Forms.CheckBox chkPause;
        private System.Windows.Forms.RichTextBox rtbLog;
        private System.Windows.Forms.Panel pnlState;
        private System.Windows.Forms.Panel pnlLog;
        private System.Windows.Forms.Timer tmrRefresh;
    }
}