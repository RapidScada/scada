namespace ScadaAdmin
{
    partial class FrmAbout
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmAbout));
            this.pbAboutRu = new System.Windows.Forms.PictureBox();
            this.lblWebsite = new System.Windows.Forms.Label();
            this.lblVersionRu = new System.Windows.Forms.Label();
            this.pbAboutEn = new System.Windows.Forms.PictureBox();
            this.lblVersionEn = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbAboutRu)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbAboutEn)).BeginInit();
            this.SuspendLayout();
            // 
            // pbAboutRu
            // 
            this.pbAboutRu.Enabled = false;
            this.pbAboutRu.Image = ((System.Drawing.Image)(resources.GetObject("pbAboutRu.Image")));
            this.pbAboutRu.Location = new System.Drawing.Point(0, 0);
            this.pbAboutRu.Name = "pbAboutRu";
            this.pbAboutRu.Size = new System.Drawing.Size(424, 222);
            this.pbAboutRu.TabIndex = 0;
            this.pbAboutRu.TabStop = false;
            // 
            // lblWebsite
            // 
            this.lblWebsite.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblWebsite.Location = new System.Drawing.Point(232, 174);
            this.lblWebsite.Name = "lblWebsite";
            this.lblWebsite.Size = new System.Drawing.Size(95, 23);
            this.lblWebsite.TabIndex = 2;
            this.lblWebsite.Click += new System.EventHandler(this.lblLink_Click);
            // 
            // lblVersionRu
            // 
            this.lblVersionRu.BackColor = System.Drawing.Color.White;
            this.lblVersionRu.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(204)));
            this.lblVersionRu.ForeColor = System.Drawing.Color.Black;
            this.lblVersionRu.Location = new System.Drawing.Point(289, 77);
            this.lblVersionRu.Margin = new System.Windows.Forms.Padding(0);
            this.lblVersionRu.Name = "lblVersionRu";
            this.lblVersionRu.Size = new System.Drawing.Size(80, 12);
            this.lblVersionRu.TabIndex = 0;
            this.lblVersionRu.Text = "Версия 5.0.0.0";
            this.lblVersionRu.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblVersionRu.Click += new System.EventHandler(this.FrmAbout_Click);
            // 
            // pbAboutEn
            // 
            this.pbAboutEn.Enabled = false;
            this.pbAboutEn.Image = ((System.Drawing.Image)(resources.GetObject("pbAboutEn.Image")));
            this.pbAboutEn.Location = new System.Drawing.Point(0, 0);
            this.pbAboutEn.Name = "pbAboutEn";
            this.pbAboutEn.Size = new System.Drawing.Size(424, 222);
            this.pbAboutEn.TabIndex = 3;
            this.pbAboutEn.TabStop = false;
            // 
            // lblVersionEn
            // 
            this.lblVersionEn.BackColor = System.Drawing.Color.White;
            this.lblVersionEn.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(204)));
            this.lblVersionEn.ForeColor = System.Drawing.Color.Black;
            this.lblVersionEn.Location = new System.Drawing.Point(315, 77);
            this.lblVersionEn.Margin = new System.Windows.Forms.Padding(0);
            this.lblVersionEn.Name = "lblVersionEn";
            this.lblVersionEn.Size = new System.Drawing.Size(80, 12);
            this.lblVersionEn.TabIndex = 1;
            this.lblVersionEn.Text = "Version 5.0.0.0";
            this.lblVersionEn.Click += new System.EventHandler(this.FrmAbout_Click);
            // 
            // FrmAbout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(424, 222);
            this.Controls.Add(this.lblVersionRu);
            this.Controls.Add(this.pbAboutRu);
            this.Controls.Add(this.lblVersionEn);
            this.Controls.Add(this.pbAboutEn);
            this.Controls.Add(this.lblWebsite);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmAbout";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "О программе";
            this.Click += new System.EventHandler(this.FrmAbout_Click);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FrmAbout_KeyPress);
            ((System.ComponentModel.ISupportInitialize)(this.pbAboutRu)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbAboutEn)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbAboutRu;
        private System.Windows.Forms.Label lblWebsite;
        private System.Windows.Forms.Label lblVersionRu;
        private System.Windows.Forms.PictureBox pbAboutEn;
        private System.Windows.Forms.Label lblVersionEn;
    }
}