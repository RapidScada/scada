namespace Scada.Admin.App.Controls.Tools
{
    partial class CtrlCnlCreate2
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
            this.lblDevice = new System.Windows.Forms.Label();
            this.txtDevice = new System.Windows.Forms.TextBox();
            this.lblObj = new System.Windows.Forms.Label();
            this.cbObj = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // lblDevice
            // 
            this.lblDevice.AutoSize = true;
            this.lblDevice.Location = new System.Drawing.Point(-3, 0);
            this.lblDevice.Name = "lblDevice";
            this.lblDevice.Size = new System.Drawing.Size(41, 13);
            this.lblDevice.TabIndex = 0;
            this.lblDevice.Text = "Device";
            // 
            // txtDevice
            // 
            this.txtDevice.Location = new System.Drawing.Point(0, 16);
            this.txtDevice.Name = "txtDevice";
            this.txtDevice.ReadOnly = true;
            this.txtDevice.Size = new System.Drawing.Size(360, 20);
            this.txtDevice.TabIndex = 1;
            // 
            // lblObj
            // 
            this.lblObj.AutoSize = true;
            this.lblObj.Location = new System.Drawing.Point(-3, 39);
            this.lblObj.Name = "lblObj";
            this.lblObj.Size = new System.Drawing.Size(38, 13);
            this.lblObj.TabIndex = 2;
            this.lblObj.Text = "Object";
            // 
            // cbObj
            // 
            this.cbObj.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbObj.FormattingEnabled = true;
            this.cbObj.Location = new System.Drawing.Point(0, 55);
            this.cbObj.Name = "cbObj";
            this.cbObj.Size = new System.Drawing.Size(360, 21);
            this.cbObj.TabIndex = 3;
            // 
            // CtrlCnlCreate2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cbObj);
            this.Controls.Add(this.lblObj);
            this.Controls.Add(this.txtDevice);
            this.Controls.Add(this.lblDevice);
            this.Name = "CtrlCnlCreate2";
            this.Size = new System.Drawing.Size(360, 100);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblDevice;
        private System.Windows.Forms.TextBox txtDevice;
        private System.Windows.Forms.Label lblObj;
        private System.Windows.Forms.ComboBox cbObj;
    }
}
