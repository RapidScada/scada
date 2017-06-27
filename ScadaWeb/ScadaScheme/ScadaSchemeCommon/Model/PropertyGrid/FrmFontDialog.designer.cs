namespace Scada.Scheme.Model.PropertyGrid
{
    partial class FrmFontDialog
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
            this.lblFontName = new System.Windows.Forms.Label();
            this.lblFontSize = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.cbFontSize = new System.Windows.Forms.ComboBox();
            this.gbStyle = new System.Windows.Forms.GroupBox();
            this.chkUnderline = new System.Windows.Forms.CheckBox();
            this.chkItalic = new System.Windows.Forms.CheckBox();
            this.chkBold = new System.Windows.Forms.CheckBox();
            this.cbFontName = new System.Windows.Forms.ComboBox();
            this.gbStyle.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblFontName
            // 
            this.lblFontName.AutoSize = true;
            this.lblFontName.Location = new System.Drawing.Point(9, 9);
            this.lblFontName.Name = "lblFontName";
            this.lblFontName.Size = new System.Drawing.Size(28, 13);
            this.lblFontName.TabIndex = 0;
            this.lblFontName.Text = "Font";
            // 
            // lblFontSize
            // 
            this.lblFontSize.AutoSize = true;
            this.lblFontSize.Location = new System.Drawing.Point(165, 9);
            this.lblFontSize.Name = "lblFontSize";
            this.lblFontSize.Size = new System.Drawing.Size(27, 13);
            this.lblFontSize.TabIndex = 2;
            this.lblFontSize.Text = "Size";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(193, 153);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(112, 153);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // cbFontSize
            // 
            this.cbFontSize.FormattingEnabled = true;
            this.cbFontSize.Items.AddRange(new object[] {
            "8",
            "9",
            "10",
            "11",
            "12",
            "14",
            "16",
            "18",
            "20",
            "22",
            "24",
            "26",
            "28",
            "36",
            "48",
            "72"});
            this.cbFontSize.Location = new System.Drawing.Point(168, 25);
            this.cbFontSize.Name = "cbFontSize";
            this.cbFontSize.Size = new System.Drawing.Size(100, 21);
            this.cbFontSize.TabIndex = 3;
            // 
            // gbStyle
            // 
            this.gbStyle.Controls.Add(this.chkUnderline);
            this.gbStyle.Controls.Add(this.chkItalic);
            this.gbStyle.Controls.Add(this.chkBold);
            this.gbStyle.Location = new System.Drawing.Point(12, 52);
            this.gbStyle.Name = "gbStyle";
            this.gbStyle.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbStyle.Size = new System.Drawing.Size(256, 95);
            this.gbStyle.TabIndex = 4;
            this.gbStyle.TabStop = false;
            this.gbStyle.Text = "Style";
            // 
            // chkUnderline
            // 
            this.chkUnderline.AutoSize = true;
            this.chkUnderline.Location = new System.Drawing.Point(13, 65);
            this.chkUnderline.Name = "chkUnderline";
            this.chkUnderline.Size = new System.Drawing.Size(71, 17);
            this.chkUnderline.TabIndex = 2;
            this.chkUnderline.Text = "Underline";
            this.chkUnderline.UseVisualStyleBackColor = true;
            // 
            // chkItalic
            // 
            this.chkItalic.AutoSize = true;
            this.chkItalic.Location = new System.Drawing.Point(13, 42);
            this.chkItalic.Name = "chkItalic";
            this.chkItalic.Size = new System.Drawing.Size(48, 17);
            this.chkItalic.TabIndex = 1;
            this.chkItalic.Text = "Italic";
            this.chkItalic.UseVisualStyleBackColor = true;
            // 
            // chkBold
            // 
            this.chkBold.AutoSize = true;
            this.chkBold.Location = new System.Drawing.Point(13, 19);
            this.chkBold.Name = "chkBold";
            this.chkBold.Size = new System.Drawing.Size(47, 17);
            this.chkBold.TabIndex = 0;
            this.chkBold.Text = "Bold";
            this.chkBold.UseVisualStyleBackColor = true;
            // 
            // cbFontName
            // 
            this.cbFontName.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cbFontName.FormattingEnabled = true;
            this.cbFontName.Location = new System.Drawing.Point(12, 25);
            this.cbFontName.Name = "cbFontName";
            this.cbFontName.Size = new System.Drawing.Size(150, 21);
            this.cbFontName.TabIndex = 1;
            this.cbFontName.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cbFontName_DrawItem);
            this.cbFontName.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(this.cbFontName_MeasureItem);
            // 
            // FrmFontDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(281, 188);
            this.Controls.Add(this.cbFontName);
            this.Controls.Add(this.gbStyle);
            this.Controls.Add(this.cbFontSize);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblFontSize);
            this.Controls.Add(this.lblFontName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmFontDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Font";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmFontDialog_FormClosed);
            this.Load += new System.EventHandler(this.FrmFontDialog_Load);
            this.gbStyle.ResumeLayout(false);
            this.gbStyle.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblFontName;
        private System.Windows.Forms.Label lblFontSize;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.ComboBox cbFontSize;
        private System.Windows.Forms.GroupBox gbStyle;
        private System.Windows.Forms.CheckBox chkUnderline;
        private System.Windows.Forms.CheckBox chkItalic;
        private System.Windows.Forms.CheckBox chkBold;
        private System.Windows.Forms.ComboBox cbFontName;
    }
}