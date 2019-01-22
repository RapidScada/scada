namespace Scada.Server.Shell.Forms
{
    partial class FrmArchive
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmArchive));
            this.btnView = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.lblWarning = new System.Windows.Forms.Label();
            this.cbDataKind = new System.Windows.Forms.ComboBox();
            this.txtDir = new System.Windows.Forms.TextBox();
            this.lbFiles = new System.Windows.Forms.ListBox();
            this.pnlWarning = new System.Windows.Forms.Panel();
            this.pbWarning = new System.Windows.Forms.PictureBox();
            this.pnlWarning.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbWarning)).BeginInit();
            this.SuspendLayout();
            // 
            // btnView
            // 
            this.btnView.Location = new System.Drawing.Point(12, 12);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(100, 23);
            this.btnView.TabIndex = 0;
            this.btnView.Text = "View";
            this.btnView.UseVisualStyleBackColor = true;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(118, 12);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(100, 23);
            this.btnEdit.TabIndex = 1;
            this.btnEdit.Text = "Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // lblWarning
            // 
            this.lblWarning.AutoSize = true;
            this.lblWarning.Location = new System.Drawing.Point(22, 3);
            this.lblWarning.Name = "lblWarning";
            this.lblWarning.Size = new System.Drawing.Size(57, 13);
            this.lblWarning.TabIndex = 0;
            this.lblWarning.Text = "lblWarning";
            // 
            // cbDataKind
            // 
            this.cbDataKind.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDataKind.FormattingEnabled = true;
            this.cbDataKind.Items.AddRange(new object[] {
            "Main",
            "Copy"});
            this.cbDataKind.Location = new System.Drawing.Point(12, 41);
            this.cbDataKind.Name = "cbDataKind";
            this.cbDataKind.Size = new System.Drawing.Size(100, 21);
            this.cbDataKind.TabIndex = 3;
            this.cbDataKind.SelectedIndexChanged += new System.EventHandler(this.cbDataKind_SelectedIndexChanged);
            // 
            // txtDir
            // 
            this.txtDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDir.Location = new System.Drawing.Point(118, 41);
            this.txtDir.Name = "txtDir";
            this.txtDir.ReadOnly = true;
            this.txtDir.Size = new System.Drawing.Size(554, 20);
            this.txtDir.TabIndex = 4;
            // 
            // lbFiles
            // 
            this.lbFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbFiles.FormattingEnabled = true;
            this.lbFiles.HorizontalScrollbar = true;
            this.lbFiles.IntegralHeight = false;
            this.lbFiles.Location = new System.Drawing.Point(12, 68);
            this.lbFiles.MultiColumn = true;
            this.lbFiles.Name = "lbFiles";
            this.lbFiles.Size = new System.Drawing.Size(660, 331);
            this.lbFiles.TabIndex = 5;
            this.lbFiles.DoubleClick += new System.EventHandler(this.lbFiles_DoubleClick);
            this.lbFiles.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lbFiles_KeyDown);
            // 
            // pnlWarning
            // 
            this.pnlWarning.Controls.Add(this.pbWarning);
            this.pnlWarning.Controls.Add(this.lblWarning);
            this.pnlWarning.Location = new System.Drawing.Point(224, 14);
            this.pnlWarning.Name = "pnlWarning";
            this.pnlWarning.Size = new System.Drawing.Size(448, 19);
            this.pnlWarning.TabIndex = 2;
            this.pnlWarning.Visible = false;
            // 
            // pbWarning
            // 
            this.pbWarning.Image = ((System.Drawing.Image)(resources.GetObject("pbWarning.Image")));
            this.pbWarning.Location = new System.Drawing.Point(0, 1);
            this.pbWarning.Name = "pbWarning";
            this.pbWarning.Size = new System.Drawing.Size(16, 16);
            this.pbWarning.TabIndex = 0;
            this.pbWarning.TabStop = false;
            // 
            // FrmArchive
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 411);
            this.Controls.Add(this.pnlWarning);
            this.Controls.Add(this.lbFiles);
            this.Controls.Add(this.txtDir);
            this.Controls.Add(this.cbDataKind);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnView);
            this.Name = "FrmArchive";
            this.Text = "Archive";
            this.Load += new System.EventHandler(this.FrmArchive_Load);
            this.pnlWarning.ResumeLayout(false);
            this.pnlWarning.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbWarning)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnView;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Label lblWarning;
        private System.Windows.Forms.ComboBox cbDataKind;
        private System.Windows.Forms.TextBox txtDir;
        private System.Windows.Forms.ListBox lbFiles;
        private System.Windows.Forms.Panel pnlWarning;
        private System.Windows.Forms.PictureBox pbWarning;
    }
}