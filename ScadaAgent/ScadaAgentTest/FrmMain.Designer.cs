namespace Scada.Agent.Test
{
    partial class FrmMain
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
            this.btnCreateSession = new System.Windows.Forms.Button();
            this.btnDownload = new System.Windows.Forms.Button();
            this.btnUpload = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnCreateSession
            // 
            this.btnCreateSession.Location = new System.Drawing.Point(12, 12);
            this.btnCreateSession.Name = "btnCreateSession";
            this.btnCreateSession.Size = new System.Drawing.Size(100, 23);
            this.btnCreateSession.TabIndex = 0;
            this.btnCreateSession.Text = "Create Session";
            this.btnCreateSession.UseVisualStyleBackColor = true;
            this.btnCreateSession.Click += new System.EventHandler(this.btnCreateSession_Click);
            // 
            // btnDownload
            // 
            this.btnDownload.Location = new System.Drawing.Point(12, 41);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(100, 23);
            this.btnDownload.TabIndex = 1;
            this.btnDownload.Text = "Download";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // btnUpload
            // 
            this.btnUpload.Location = new System.Drawing.Point(12, 70);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(100, 23);
            this.btnUpload.TabIndex = 2;
            this.btnUpload.Text = "Upload";
            this.btnUpload.UseVisualStyleBackColor = true;
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 311);
            this.Controls.Add(this.btnUpload);
            this.Controls.Add(this.btnDownload);
            this.Controls.Add(this.btnCreateSession);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Agent Test";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCreateSession;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.Button btnUpload;
    }
}

