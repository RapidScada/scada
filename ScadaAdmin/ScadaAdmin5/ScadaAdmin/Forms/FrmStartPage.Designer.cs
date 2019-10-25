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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmStartPage));
            this.pnlContent = new System.Windows.Forms.Panel();
            this.lblNoRecentProjects = new System.Windows.Forms.Label();
            this.lbRecentProjects = new System.Windows.Forms.ListBox();
            this.btnOpenProject = new System.Windows.Forms.Button();
            this.btnNewProject = new System.Windows.Forms.Button();
            this.lblRecentProjects = new System.Windows.Forms.Label();
            this.cmsProjectList = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miRemoveFromList = new System.Windows.Forms.ToolStripMenuItem();
            this.miCopyPath = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlContent.SuspendLayout();
            this.cmsProjectList.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlContent
            // 
            this.pnlContent.BackColor = System.Drawing.SystemColors.Window;
            this.pnlContent.Controls.Add(this.lblNoRecentProjects);
            this.pnlContent.Controls.Add(this.lbRecentProjects);
            this.pnlContent.Controls.Add(this.btnOpenProject);
            this.pnlContent.Controls.Add(this.btnNewProject);
            this.pnlContent.Controls.Add(this.lblRecentProjects);
            this.pnlContent.Location = new System.Drawing.Point(0, 0);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Size = new System.Drawing.Size(800, 400);
            this.pnlContent.TabIndex = 0;
            // 
            // lblNoRecentProjects
            // 
            this.lblNoRecentProjects.AutoSize = true;
            this.lblNoRecentProjects.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblNoRecentProjects.Location = new System.Drawing.Point(28, 59);
            this.lblNoRecentProjects.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
            this.lblNoRecentProjects.Name = "lblNoRecentProjects";
            this.lblNoRecentProjects.Size = new System.Drawing.Size(132, 16);
            this.lblNoRecentProjects.TabIndex = 1;
            this.lblNoRecentProjects.Text = "No recent projects";
            // 
            // lbRecentProjects
            // 
            this.lbRecentProjects.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lbRecentProjects.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lbRecentProjects.ContextMenuStrip = this.cmsProjectList;
            this.lbRecentProjects.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lbRecentProjects.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbRecentProjects.FormattingEnabled = true;
            this.lbRecentProjects.IntegralHeight = false;
            this.lbRecentProjects.ItemHeight = 50;
            this.lbRecentProjects.Location = new System.Drawing.Point(25, 59);
            this.lbRecentProjects.Margin = new System.Windows.Forms.Padding(25, 10, 10, 10);
            this.lbRecentProjects.Name = "lbRecentProjects";
            this.lbRecentProjects.Size = new System.Drawing.Size(535, 331);
            this.lbRecentProjects.TabIndex = 2;
            this.lbRecentProjects.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lbRecentProjects_MouseClick);
            this.lbRecentProjects.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lbRecentProjects_DrawItem);
            this.lbRecentProjects.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lbRecentProjects_KeyDown);
            this.lbRecentProjects.MouseLeave += new System.EventHandler(this.lbRecentProjects_MouseLeave);
            this.lbRecentProjects.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lbRecentProjects_MouseMove);
            // 
            // btnOpenProject
            // 
            this.btnOpenProject.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOpenProject.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenProject.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnOpenProject.ForeColor = System.Drawing.SystemColors.Highlight;
            this.btnOpenProject.Location = new System.Drawing.Point(580, 80);
            this.btnOpenProject.Margin = new System.Windows.Forms.Padding(10, 10, 20, 10);
            this.btnOpenProject.Name = "btnOpenProject";
            this.btnOpenProject.Size = new System.Drawing.Size(200, 40);
            this.btnOpenProject.TabIndex = 4;
            this.btnOpenProject.Text = "Open Project";
            this.btnOpenProject.UseVisualStyleBackColor = true;
            this.btnOpenProject.Click += new System.EventHandler(this.btnOpenProject_Click);
            // 
            // btnNewProject
            // 
            this.btnNewProject.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNewProject.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNewProject.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnNewProject.ForeColor = System.Drawing.SystemColors.Highlight;
            this.btnNewProject.Location = new System.Drawing.Point(580, 20);
            this.btnNewProject.Margin = new System.Windows.Forms.Padding(10, 20, 20, 10);
            this.btnNewProject.Name = "btnNewProject";
            this.btnNewProject.Size = new System.Drawing.Size(200, 40);
            this.btnNewProject.TabIndex = 3;
            this.btnNewProject.Text = "New Project";
            this.btnNewProject.UseVisualStyleBackColor = true;
            this.btnNewProject.Click += new System.EventHandler(this.btnNewProject_Click);
            // 
            // lblRecentProjects
            // 
            this.lblRecentProjects.AutoSize = true;
            this.lblRecentProjects.Font = new System.Drawing.Font("Verdana", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblRecentProjects.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblRecentProjects.Location = new System.Drawing.Point(25, 20);
            this.lblRecentProjects.Name = "lblRecentProjects";
            this.lblRecentProjects.Size = new System.Drawing.Size(198, 29);
            this.lblRecentProjects.TabIndex = 0;
            this.lblRecentProjects.Text = "Recent Projects";
            // 
            // cmsProjectList
            // 
            this.cmsProjectList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miRemoveFromList,
            this.miCopyPath});
            this.cmsProjectList.Name = "cmsProjectList";
            this.cmsProjectList.Size = new System.Drawing.Size(181, 70);
            this.cmsProjectList.Opening += new System.ComponentModel.CancelEventHandler(this.cmsProjectList_Opening);
            // 
            // miRemoveFromList
            // 
            this.miRemoveFromList.Image = ((System.Drawing.Image)(resources.GetObject("miRemoveFromList.Image")));
            this.miRemoveFromList.Name = "miRemoveFromList";
            this.miRemoveFromList.Size = new System.Drawing.Size(180, 22);
            this.miRemoveFromList.Text = "Remove From List";
            this.miRemoveFromList.Click += new System.EventHandler(this.miRemoveFromList_Click);
            // 
            // miCopyPath
            // 
            this.miCopyPath.Image = ((System.Drawing.Image)(resources.GetObject("miCopyPath.Image")));
            this.miCopyPath.Name = "miCopyPath";
            this.miCopyPath.Size = new System.Drawing.Size(180, 22);
            this.miCopyPath.Text = "Copy Path";
            this.miCopyPath.Click += new System.EventHandler(this.miCopyPath_Click);
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
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmStartPage_FormClosed);
            this.Load += new System.EventHandler(this.FrmStartPage_Load);
            this.Resize += new System.EventHandler(this.FrmStartPage_Resize);
            this.pnlContent.ResumeLayout(false);
            this.pnlContent.PerformLayout();
            this.cmsProjectList.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.Label lblRecentProjects;
        private System.Windows.Forms.Button btnNewProject;
        private System.Windows.Forms.Button btnOpenProject;
        private System.Windows.Forms.ListBox lbRecentProjects;
        private System.Windows.Forms.Label lblNoRecentProjects;
        private System.Windows.Forms.ContextMenuStrip cmsProjectList;
        private System.Windows.Forms.ToolStripMenuItem miRemoveFromList;
        private System.Windows.Forms.ToolStripMenuItem miCopyPath;
    }
}