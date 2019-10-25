namespace Scada.Admin.App.Forms.Tables
{
    partial class FrmFind
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
            this.btnReplaceAll = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblColumn = new System.Windows.Forms.Label();
            this.cbColumn = new System.Windows.Forms.ComboBox();
            this.lblFind = new System.Windows.Forms.Label();
            this.txtFind = new System.Windows.Forms.TextBox();
            this.lblReplaceWith = new System.Windows.Forms.Label();
            this.txtReplaceWith = new System.Windows.Forms.TextBox();
            this.chkCaseSensitive = new System.Windows.Forms.CheckBox();
            this.chkWholeCellOnly = new System.Windows.Forms.CheckBox();
            this.cbFind = new System.Windows.Forms.ComboBox();
            this.cbReplaceWith = new System.Windows.Forms.ComboBox();
            this.btnFindNext = new System.Windows.Forms.Button();
            this.btnReplace = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnReplaceAll
            // 
            this.btnReplaceAll.Location = new System.Drawing.Point(242, 178);
            this.btnReplaceAll.Name = "btnReplaceAll";
            this.btnReplaceAll.Size = new System.Drawing.Size(90, 23);
            this.btnReplaceAll.TabIndex = 12;
            this.btnReplaceAll.Text = "Replace All";
            this.btnReplaceAll.UseVisualStyleBackColor = true;
            this.btnReplaceAll.Click += new System.EventHandler(this.btnReplaceAll_Click);
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(242, 207);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(90, 23);
            this.btnClose.TabIndex = 13;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblColumn
            // 
            this.lblColumn.AutoSize = true;
            this.lblColumn.Location = new System.Drawing.Point(9, 9);
            this.lblColumn.Name = "lblColumn";
            this.lblColumn.Size = new System.Drawing.Size(42, 13);
            this.lblColumn.TabIndex = 0;
            this.lblColumn.Text = "Column";
            // 
            // cbColumn
            // 
            this.cbColumn.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbColumn.FormattingEnabled = true;
            this.cbColumn.Location = new System.Drawing.Point(12, 25);
            this.cbColumn.Name = "cbColumn";
            this.cbColumn.Size = new System.Drawing.Size(320, 21);
            this.cbColumn.TabIndex = 1;
            this.cbColumn.SelectedIndexChanged += new System.EventHandler(this.cbColumn_SelectedIndexChanged);
            // 
            // lblFind
            // 
            this.lblFind.AutoSize = true;
            this.lblFind.Location = new System.Drawing.Point(9, 49);
            this.lblFind.Name = "lblFind";
            this.lblFind.Size = new System.Drawing.Size(53, 13);
            this.lblFind.TabIndex = 2;
            this.lblFind.Text = "Find what";
            // 
            // txtFind
            // 
            this.txtFind.Location = new System.Drawing.Point(12, 65);
            this.txtFind.Name = "txtFind";
            this.txtFind.Size = new System.Drawing.Size(320, 20);
            this.txtFind.TabIndex = 3;
            this.txtFind.TextChanged += new System.EventHandler(this.txtFind_TextChanged);
            // 
            // lblReplaceWith
            // 
            this.lblReplaceWith.AutoSize = true;
            this.lblReplaceWith.Location = new System.Drawing.Point(9, 89);
            this.lblReplaceWith.Name = "lblReplaceWith";
            this.lblReplaceWith.Size = new System.Drawing.Size(69, 13);
            this.lblReplaceWith.TabIndex = 5;
            this.lblReplaceWith.Text = "Replace with";
            // 
            // txtReplaceWith
            // 
            this.txtReplaceWith.Location = new System.Drawing.Point(12, 105);
            this.txtReplaceWith.Name = "txtReplaceWith";
            this.txtReplaceWith.Size = new System.Drawing.Size(320, 20);
            this.txtReplaceWith.TabIndex = 6;
            // 
            // chkCaseSensitive
            // 
            this.chkCaseSensitive.AutoSize = true;
            this.chkCaseSensitive.Location = new System.Drawing.Point(12, 132);
            this.chkCaseSensitive.Name = "chkCaseSensitive";
            this.chkCaseSensitive.Size = new System.Drawing.Size(82, 17);
            this.chkCaseSensitive.TabIndex = 8;
            this.chkCaseSensitive.Text = "Match case";
            this.chkCaseSensitive.UseVisualStyleBackColor = true;
            // 
            // chkWholeCellOnly
            // 
            this.chkWholeCellOnly.AutoSize = true;
            this.chkWholeCellOnly.Checked = true;
            this.chkWholeCellOnly.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkWholeCellOnly.Location = new System.Drawing.Point(12, 155);
            this.chkWholeCellOnly.Name = "chkWholeCellOnly";
            this.chkWholeCellOnly.Size = new System.Drawing.Size(106, 17);
            this.chkWholeCellOnly.TabIndex = 9;
            this.chkWholeCellOnly.Text = "Match whole cell";
            this.chkWholeCellOnly.UseVisualStyleBackColor = true;
            // 
            // cbFind
            // 
            this.cbFind.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFind.FormattingEnabled = true;
            this.cbFind.Location = new System.Drawing.Point(12, 75);
            this.cbFind.Name = "cbFind";
            this.cbFind.Size = new System.Drawing.Size(320, 21);
            this.cbFind.TabIndex = 4;
            this.cbFind.Visible = false;
            this.cbFind.SelectedIndexChanged += new System.EventHandler(this.cbFind_SelectedIndexChanged);
            // 
            // cbReplaceWith
            // 
            this.cbReplaceWith.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbReplaceWith.FormattingEnabled = true;
            this.cbReplaceWith.Location = new System.Drawing.Point(12, 115);
            this.cbReplaceWith.Name = "cbReplaceWith";
            this.cbReplaceWith.Size = new System.Drawing.Size(320, 21);
            this.cbReplaceWith.TabIndex = 7;
            this.cbReplaceWith.Visible = false;
            this.cbReplaceWith.SelectedIndexChanged += new System.EventHandler(this.cbReplaceWith_SelectedIndexChanged);
            // 
            // btnFindNext
            // 
            this.btnFindNext.Location = new System.Drawing.Point(50, 178);
            this.btnFindNext.Name = "btnFindNext";
            this.btnFindNext.Size = new System.Drawing.Size(90, 23);
            this.btnFindNext.TabIndex = 10;
            this.btnFindNext.Text = "Find Next";
            this.btnFindNext.UseVisualStyleBackColor = true;
            this.btnFindNext.Click += new System.EventHandler(this.btnFindNext_Click);
            // 
            // btnReplace
            // 
            this.btnReplace.Location = new System.Drawing.Point(146, 178);
            this.btnReplace.Name = "btnReplace";
            this.btnReplace.Size = new System.Drawing.Size(90, 23);
            this.btnReplace.TabIndex = 11;
            this.btnReplace.Text = "Replace";
            this.btnReplace.UseVisualStyleBackColor = true;
            this.btnReplace.Click += new System.EventHandler(this.btnReplace_Click);
            // 
            // FrmFind
            // 
            this.AcceptButton = this.btnFindNext;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(344, 242);
            this.Controls.Add(this.btnReplace);
            this.Controls.Add(this.btnFindNext);
            this.Controls.Add(this.cbReplaceWith);
            this.Controls.Add(this.cbFind);
            this.Controls.Add(this.chkWholeCellOnly);
            this.Controls.Add(this.chkCaseSensitive);
            this.Controls.Add(this.txtReplaceWith);
            this.Controls.Add(this.lblReplaceWith);
            this.Controls.Add(this.txtFind);
            this.Controls.Add(this.lblFind);
            this.Controls.Add(this.cbColumn);
            this.Controls.Add(this.lblColumn);
            this.Controls.Add(this.btnReplaceAll);
            this.Controls.Add(this.btnClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmFind";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Find and Replace";
            this.Load += new System.EventHandler(this.FrmReplace_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnReplaceAll;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblColumn;
        private System.Windows.Forms.ComboBox cbColumn;
        private System.Windows.Forms.Label lblFind;
        private System.Windows.Forms.TextBox txtFind;
        private System.Windows.Forms.Label lblReplaceWith;
        private System.Windows.Forms.TextBox txtReplaceWith;
        private System.Windows.Forms.CheckBox chkCaseSensitive;
        private System.Windows.Forms.CheckBox chkWholeCellOnly;
        private System.Windows.Forms.ComboBox cbFind;
        private System.Windows.Forms.ComboBox cbReplaceWith;
        private System.Windows.Forms.Button btnFindNext;
        private System.Windows.Forms.Button btnReplace;
    }
}