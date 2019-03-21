namespace Scada.Comm.Shell.Forms
{
    partial class FrmLineParams
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
            this.lbTabs = new System.Windows.Forms.ListBox();
            this.ctrlLineReqSequence = new Scada.Comm.Shell.Controls.CtrlLineReqSequence();
            this.ctrlLineCustomParams = new Scada.Comm.Shell.Controls.CtrlLineCustomParams();
            this.ctrlLineMainParams = new Scada.Comm.Shell.Controls.CtrlLineMainParams();
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
            "Main Parameters",
            "Custom Parameters",
            "Request Sequence"});
            this.lbTabs.Location = new System.Drawing.Point(0, 0);
            this.lbTabs.Name = "lbTabs";
            this.lbTabs.Size = new System.Drawing.Size(150, 461);
            this.lbTabs.TabIndex = 1;
            this.lbTabs.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lbTabs_DrawItem);
            this.lbTabs.SelectedIndexChanged += new System.EventHandler(this.lbTabs_SelectedIndexChanged);
            // 
            // ctrlLineReqSequence
            // 
            this.ctrlLineReqSequence.CommLine = null;
            this.ctrlLineReqSequence.CustomParams = null;
            this.ctrlLineReqSequence.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlLineReqSequence.Environment = null;
            this.ctrlLineReqSequence.Location = new System.Drawing.Point(150, 0);
            this.ctrlLineReqSequence.Name = "ctrlLineReqSequence";
            this.ctrlLineReqSequence.Size = new System.Drawing.Size(584, 461);
            this.ctrlLineReqSequence.TabIndex = 4;
            this.ctrlLineReqSequence.SettingsChanged += new System.EventHandler(this.control_SettingsChanged);
            this.ctrlLineReqSequence.CustomParamsChanged += new System.EventHandler(this.ctrlLineReqSequence_CustomParamsChanged);
            // 
            // ctrlLineCustomParams
            // 
            this.ctrlLineCustomParams.CommLine = null;
            this.ctrlLineCustomParams.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlLineCustomParams.Location = new System.Drawing.Point(150, 0);
            this.ctrlLineCustomParams.Name = "ctrlLineCustomParams";
            this.ctrlLineCustomParams.Size = new System.Drawing.Size(584, 461);
            this.ctrlLineCustomParams.TabIndex = 3;
            this.ctrlLineCustomParams.SettingsChanged += new System.EventHandler(this.control_SettingsChanged);
            // 
            // ctrlLineMainParams
            // 
            this.ctrlLineMainParams.CommLine = null;
            this.ctrlLineMainParams.Location = new System.Drawing.Point(159, 12);
            this.ctrlLineMainParams.Name = "ctrlLineMainParams";
            this.ctrlLineMainParams.Size = new System.Drawing.Size(550, 450);
            this.ctrlLineMainParams.TabIndex = 2;
            this.ctrlLineMainParams.SettingsChanged += new System.EventHandler(this.control_SettingsChanged);
            // 
            // FrmLineParams
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(734, 461);
            this.Controls.Add(this.ctrlLineReqSequence);
            this.Controls.Add(this.ctrlLineCustomParams);
            this.Controls.Add(this.ctrlLineMainParams);
            this.Controls.Add(this.lbTabs);
            this.Name = "FrmLineParams";
            this.Text = "Line Parameters";
            this.Load += new System.EventHandler(this.FrmLineParams_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ListBox lbTabs;
        private Controls.CtrlLineMainParams ctrlLineMainParams;
        private Controls.CtrlLineCustomParams ctrlLineCustomParams;
        private Controls.CtrlLineReqSequence ctrlLineReqSequence;
    }
}