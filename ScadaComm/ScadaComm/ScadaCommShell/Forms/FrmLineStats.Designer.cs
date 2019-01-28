namespace Scada.Comm.Shell.Forms
{
    partial class FrmLineStats
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
            this.lbTabs = new System.Windows.Forms.ListBox();
            this.chkPause = new System.Windows.Forms.CheckBox();
            this.tmrRefresh = new System.Windows.Forms.Timer(this.components);
            this.lbLog = new System.Windows.Forms.ListBox();
            this.lbState = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // lbTabs
            // 
            this.lbTabs.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lbTabs.Dock = System.Windows.Forms.DockStyle.Left;
            this.lbTabs.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lbTabs.FormattingEnabled = true;
            this.lbTabs.IntegralHeight = false;
            this.lbTabs.ItemHeight = 25;
            this.lbTabs.Items.AddRange(new object[] {
            "Line State",
            "Line Log"});
            this.lbTabs.Location = new System.Drawing.Point(0, 0);
            this.lbTabs.Name = "lbTabs";
            this.lbTabs.Size = new System.Drawing.Size(150, 411);
            this.lbTabs.TabIndex = 0;
            this.lbTabs.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lbTabs_DrawItem);
            this.lbTabs.SelectedIndexChanged += new System.EventHandler(this.lbTabs_SelectedIndexChanged);
            // 
            // chkPause
            // 
            this.chkPause.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkPause.AutoSize = true;
            this.chkPause.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkPause.Location = new System.Drawing.Point(616, 12);
            this.chkPause.Name = "chkPause";
            this.chkPause.Size = new System.Drawing.Size(56, 17);
            this.chkPause.TabIndex = 1;
            this.chkPause.Text = "Pause";
            this.chkPause.UseVisualStyleBackColor = true;
            // 
            // tmrRefresh
            // 
            this.tmrRefresh.Interval = 500;
            this.tmrRefresh.Tick += new System.EventHandler(this.tmrRefresh_Tick);
            // 
            // lbLog
            // 
            this.lbLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbLog.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbLog.FormattingEnabled = true;
            this.lbLog.HorizontalScrollbar = true;
            this.lbLog.IntegralHeight = false;
            this.lbLog.Location = new System.Drawing.Point(162, 35);
            this.lbLog.Margin = new System.Windows.Forms.Padding(9, 3, 3, 3);
            this.lbLog.Name = "lbLog";
            this.lbLog.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbLog.Size = new System.Drawing.Size(510, 364);
            this.lbLog.TabIndex = 3;
            // 
            // lbState
            // 
            this.lbState.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbState.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbState.FormattingEnabled = true;
            this.lbState.HorizontalScrollbar = true;
            this.lbState.IntegralHeight = false;
            this.lbState.Location = new System.Drawing.Point(162, 35);
            this.lbState.Margin = new System.Windows.Forms.Padding(9, 3, 3, 3);
            this.lbState.Name = "lbState";
            this.lbState.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbState.Size = new System.Drawing.Size(510, 364);
            this.lbState.TabIndex = 2;
            // 
            // FrmLineStats
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 411);
            this.Controls.Add(this.lbLog);
            this.Controls.Add(this.lbState);
            this.Controls.Add(this.chkPause);
            this.Controls.Add(this.lbTabs);
            this.Name = "FrmLineStats";
            this.Text = "Line Stats";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmLineStats_FormClosed);
            this.Load += new System.EventHandler(this.FrmLineStats_Load);
            this.VisibleChanged += new System.EventHandler(this.FrmLineStats_VisibleChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lbTabs;
        private System.Windows.Forms.CheckBox chkPause;
        private System.Windows.Forms.Timer tmrRefresh;
        private System.Windows.Forms.ListBox lbLog;
        private System.Windows.Forms.ListBox lbState;
    }
}