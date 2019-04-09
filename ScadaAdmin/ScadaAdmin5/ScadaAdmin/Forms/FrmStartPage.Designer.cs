namespace Scada.Admin.App.Forms
{
    partial class FrmStartPage
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
            this.pnlContent = new System.Windows.Forms.Panel();
            this.lblRecentProjects = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.lbRecentProjects = new System.Windows.Forms.ListBox();
            this.pnlContent.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlContent
            // 
            this.pnlContent.BackColor = System.Drawing.SystemColors.Window;
            this.pnlContent.Controls.Add(this.lbRecentProjects);
            this.pnlContent.Controls.Add(this.button2);
            this.pnlContent.Controls.Add(this.button1);
            this.pnlContent.Controls.Add(this.lblRecentProjects);
            this.pnlContent.Location = new System.Drawing.Point(0, 0);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Size = new System.Drawing.Size(800, 400);
            this.pnlContent.TabIndex = 0;
            // 
            // lblRecentProjects
            // 
            this.lblRecentProjects.AutoSize = true;
            this.lblRecentProjects.Font = new System.Drawing.Font("Verdana", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblRecentProjects.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblRecentProjects.Location = new System.Drawing.Point(15, 9);
            this.lblRecentProjects.Name = "lblRecentProjects";
            this.lblRecentProjects.Size = new System.Drawing.Size(198, 29);
            this.lblRecentProjects.TabIndex = 0;
            this.lblRecentProjects.Text = "Recent Projects";
            // 
            // button1
            // 
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button1.ForeColor = System.Drawing.SystemColors.Highlight;
            this.button1.Location = new System.Drawing.Point(580, 20);
            this.button1.Margin = new System.Windows.Forms.Padding(10, 20, 20, 10);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(200, 40);
            this.button1.TabIndex = 2;
            this.button1.Text = "New Project";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button2.ForeColor = System.Drawing.SystemColors.Highlight;
            this.button2.Location = new System.Drawing.Point(580, 80);
            this.button2.Margin = new System.Windows.Forms.Padding(10, 10, 20, 10);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(200, 40);
            this.button2.TabIndex = 3;
            this.button2.Text = "Open Project";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // lbRecentProjects
            // 
            this.lbRecentProjects.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lbRecentProjects.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lbRecentProjects.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lbRecentProjects.FormattingEnabled = true;
            this.lbRecentProjects.IntegralHeight = false;
            this.lbRecentProjects.ItemHeight = 40;
            this.lbRecentProjects.Location = new System.Drawing.Point(20, 48);
            this.lbRecentProjects.Margin = new System.Windows.Forms.Padding(20, 10, 10, 10);
            this.lbRecentProjects.Name = "lbRecentProjects";
            this.lbRecentProjects.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.lbRecentProjects.Size = new System.Drawing.Size(540, 342);
            this.lbRecentProjects.TabIndex = 1;
            // 
            // FrmStartPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.ClientSize = new System.Drawing.Size(834, 461);
            this.Controls.Add(this.pnlContent);
            this.Name = "FrmStartPage";
            this.Text = "Start Page";
            this.Load += new System.EventHandler(this.FrmStartPage_Load);
            this.Resize += new System.EventHandler(this.FrmStartPage_Resize);
            this.pnlContent.ResumeLayout(false);
            this.pnlContent.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.Label lblRecentProjects;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ListBox lbRecentProjects;
    }
}