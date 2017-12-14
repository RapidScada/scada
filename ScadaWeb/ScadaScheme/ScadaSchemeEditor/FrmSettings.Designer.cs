namespace Scada.Scheme.Editor
{
    partial class FrmSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSettings));
            this.lblWebDir = new System.Windows.Forms.Label();
            this.txtWebDir = new System.Windows.Forms.TextBox();
            this.btnWebDir = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // lblWebDir
            // 
            this.lblWebDir.AutoSize = true;
            this.lblWebDir.Location = new System.Drawing.Point(9, 9);
            this.lblWebDir.Name = "lblWebDir";
            this.lblWebDir.Size = new System.Drawing.Size(127, 13);
            this.lblWebDir.TabIndex = 0;
            this.lblWebDir.Text = "Web application directory";
            // 
            // txtWebDir
            // 
            this.txtWebDir.Location = new System.Drawing.Point(12, 25);
            this.txtWebDir.Name = "txtWebDir";
            this.txtWebDir.Size = new System.Drawing.Size(334, 20);
            this.txtWebDir.TabIndex = 1;
            // 
            // btnWebDir
            // 
            this.btnWebDir.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnWebDir.Image = ((System.Drawing.Image)(resources.GetObject("btnWebDir.Image")));
            this.btnWebDir.Location = new System.Drawing.Point(352, 25);
            this.btnWebDir.Name = "btnWebDir";
            this.btnWebDir.Size = new System.Drawing.Size(20, 20);
            this.btnWebDir.TabIndex = 2;
            this.btnWebDir.UseVisualStyleBackColor = true;
            this.btnWebDir.Click += new System.EventHandler(this.btnWebDir_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(216, 61);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(297, 61);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // FrmSettings
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(384, 96);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnWebDir);
            this.Controls.Add(this.txtWebDir);
            this.Controls.Add(this.lblWebDir);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmSettings";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.FrmSettings_Load);
            this.Shown += new System.EventHandler(this.FrmSettings_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblWebDir;
        private System.Windows.Forms.TextBox txtWebDir;
        private System.Windows.Forms.Button btnWebDir;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
    }
}