namespace Scada.Admin.App.Controls.Tools
{
    partial class CtrlCnlCreate1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CtrlCnlCreate1));
            this.lblCommLine = new System.Windows.Forms.Label();
            this.cbCommLine = new System.Windows.Forms.ComboBox();
            this.lblDevice = new System.Windows.Forms.Label();
            this.cbDevice = new System.Windows.Forms.ComboBox();
            this.txtInfo = new System.Windows.Forms.TextBox();
            this.pbStatus = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbStatus)).BeginInit();
            this.SuspendLayout();
            // 
            // lblCommLine
            // 
            this.lblCommLine.AutoSize = true;
            this.lblCommLine.Location = new System.Drawing.Point(-3, 0);
            this.lblCommLine.Name = "lblCommLine";
            this.lblCommLine.Size = new System.Drawing.Size(98, 13);
            this.lblCommLine.TabIndex = 0;
            this.lblCommLine.Text = "Communication line";
            // 
            // cbCommLine
            // 
            this.cbCommLine.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCommLine.FormattingEnabled = true;
            this.cbCommLine.Location = new System.Drawing.Point(0, 16);
            this.cbCommLine.Name = "cbCommLine";
            this.cbCommLine.Size = new System.Drawing.Size(360, 21);
            this.cbCommLine.TabIndex = 1;
            this.cbCommLine.SelectedIndexChanged += new System.EventHandler(this.cbCommLine_SelectedIndexChanged);
            // 
            // lblDevice
            // 
            this.lblDevice.AutoSize = true;
            this.lblDevice.Location = new System.Drawing.Point(-3, 40);
            this.lblDevice.Name = "lblDevice";
            this.lblDevice.Size = new System.Drawing.Size(41, 13);
            this.lblDevice.TabIndex = 2;
            this.lblDevice.Text = "Device";
            // 
            // cbDevice
            // 
            this.cbDevice.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDevice.FormattingEnabled = true;
            this.cbDevice.Location = new System.Drawing.Point(0, 56);
            this.cbDevice.Name = "cbDevice";
            this.cbDevice.Size = new System.Drawing.Size(360, 21);
            this.cbDevice.TabIndex = 3;
            this.cbDevice.SelectedIndexChanged += new System.EventHandler(this.cbDevice_SelectedIndexChanged);
            // 
            // txtInfo
            // 
            this.txtInfo.Location = new System.Drawing.Point(0, 93);
            this.txtInfo.Multiline = true;
            this.txtInfo.Name = "txtInfo";
            this.txtInfo.ReadOnly = true;
            this.txtInfo.Size = new System.Drawing.Size(338, 88);
            this.txtInfo.TabIndex = 4;
            // 
            // pbStatus
            // 
            this.pbStatus.Image = ((System.Drawing.Image)(resources.GetObject("pbStatus.Image")));
            this.pbStatus.Location = new System.Drawing.Point(344, 93);
            this.pbStatus.Name = "pbStatus";
            this.pbStatus.Size = new System.Drawing.Size(16, 16);
            this.pbStatus.TabIndex = 5;
            this.pbStatus.TabStop = false;
            // 
            // CtrlCnlCreate1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pbStatus);
            this.Controls.Add(this.txtInfo);
            this.Controls.Add(this.cbDevice);
            this.Controls.Add(this.lblDevice);
            this.Controls.Add(this.cbCommLine);
            this.Controls.Add(this.lblCommLine);
            this.Name = "CtrlCnlCreate1";
            this.Size = new System.Drawing.Size(360, 181);
            ((System.ComponentModel.ISupportInitialize)(this.pbStatus)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblCommLine;
        private System.Windows.Forms.ComboBox cbCommLine;
        private System.Windows.Forms.Label lblDevice;
        private System.Windows.Forms.ComboBox cbDevice;
        private System.Windows.Forms.TextBox txtInfo;
        private System.Windows.Forms.PictureBox pbStatus;
    }
}
