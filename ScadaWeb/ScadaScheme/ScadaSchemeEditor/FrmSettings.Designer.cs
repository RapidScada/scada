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
            this.gbBrowser = new System.Windows.Forms.GroupBox();
            this.rbFirefox = new System.Windows.Forms.RadioButton();
            this.rbChrome = new System.Windows.Forms.RadioButton();
            this.rbDefault = new System.Windows.Forms.RadioButton();
            this.gbBrowser.SuspendLayout();
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
            this.txtWebDir.TextChanged += new System.EventHandler(this.control_Changed);
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
            this.btnOK.Location = new System.Drawing.Point(216, 152);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(297, 152);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // gbBrowser
            // 
            this.gbBrowser.Controls.Add(this.rbFirefox);
            this.gbBrowser.Controls.Add(this.rbChrome);
            this.gbBrowser.Controls.Add(this.rbDefault);
            this.gbBrowser.Location = new System.Drawing.Point(12, 51);
            this.gbBrowser.Name = "gbBrowser";
            this.gbBrowser.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbBrowser.Size = new System.Drawing.Size(360, 95);
            this.gbBrowser.TabIndex = 3;
            this.gbBrowser.TabStop = false;
            this.gbBrowser.Text = "Browser";
            // 
            // rbFirefox
            // 
            this.rbFirefox.AutoSize = true;
            this.rbFirefox.Location = new System.Drawing.Point(13, 65);
            this.rbFirefox.Name = "rbFirefox";
            this.rbFirefox.Size = new System.Drawing.Size(56, 17);
            this.rbFirefox.TabIndex = 2;
            this.rbFirefox.Tag = "";
            this.rbFirefox.Text = "Firefox";
            this.rbFirefox.UseVisualStyleBackColor = true;
            this.rbFirefox.CheckedChanged += new System.EventHandler(this.control_Changed);
            // 
            // rbChrome
            // 
            this.rbChrome.AutoSize = true;
            this.rbChrome.Location = new System.Drawing.Point(13, 42);
            this.rbChrome.Name = "rbChrome";
            this.rbChrome.Size = new System.Drawing.Size(61, 17);
            this.rbChrome.TabIndex = 1;
            this.rbChrome.Tag = "";
            this.rbChrome.Text = "Chrome";
            this.rbChrome.UseVisualStyleBackColor = true;
            this.rbChrome.CheckedChanged += new System.EventHandler(this.control_Changed);
            // 
            // rbDefault
            // 
            this.rbDefault.AutoSize = true;
            this.rbDefault.Checked = true;
            this.rbDefault.Location = new System.Drawing.Point(13, 19);
            this.rbDefault.Name = "rbDefault";
            this.rbDefault.Size = new System.Drawing.Size(59, 17);
            this.rbDefault.TabIndex = 0;
            this.rbDefault.TabStop = true;
            this.rbDefault.Tag = "";
            this.rbDefault.Text = "Default";
            this.rbDefault.UseVisualStyleBackColor = true;
            this.rbDefault.CheckedChanged += new System.EventHandler(this.control_Changed);
            // 
            // FrmSettings
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(384, 187);
            this.Controls.Add(this.gbBrowser);
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
            this.gbBrowser.ResumeLayout(false);
            this.gbBrowser.PerformLayout();
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
        private System.Windows.Forms.GroupBox gbBrowser;
        private System.Windows.Forms.RadioButton rbFirefox;
        private System.Windows.Forms.RadioButton rbChrome;
        private System.Windows.Forms.RadioButton rbDefault;
    }
}