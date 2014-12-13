namespace ScadaAdmin
{
    partial class FrmReplace
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
            this.lblTable = new System.Windows.Forms.Label();
            this.btnReplaceAll = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.txtTable = new System.Windows.Forms.TextBox();
            this.lblColumn = new System.Windows.Forms.Label();
            this.cbTableColumn = new System.Windows.Forms.ComboBox();
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
            // lblTable
            // 
            this.lblTable.AutoSize = true;
            this.lblTable.Location = new System.Drawing.Point(9, 9);
            this.lblTable.Name = "lblTable";
            this.lblTable.Size = new System.Drawing.Size(50, 13);
            this.lblTable.TabIndex = 0;
            this.lblTable.Text = "Таблица";
            // 
            // btnReplaceAll
            // 
            this.btnReplaceAll.Location = new System.Drawing.Point(242, 215);
            this.btnReplaceAll.Name = "btnReplaceAll";
            this.btnReplaceAll.Size = new System.Drawing.Size(90, 23);
            this.btnReplaceAll.TabIndex = 14;
            this.btnReplaceAll.Text = "Заменить все";
            this.btnReplaceAll.UseVisualStyleBackColor = true;
            this.btnReplaceAll.Click += new System.EventHandler(this.btnReplaceAll_Click);
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(242, 244);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(90, 23);
            this.btnClose.TabIndex = 15;
            this.btnClose.Text = "Закрыть";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // txtTable
            // 
            this.txtTable.Location = new System.Drawing.Point(12, 25);
            this.txtTable.Name = "txtTable";
            this.txtTable.ReadOnly = true;
            this.txtTable.Size = new System.Drawing.Size(320, 20);
            this.txtTable.TabIndex = 1;
            this.txtTable.TabStop = false;
            // 
            // lblColumn
            // 
            this.lblColumn.AutoSize = true;
            this.lblColumn.Location = new System.Drawing.Point(9, 48);
            this.lblColumn.Name = "lblColumn";
            this.lblColumn.Size = new System.Drawing.Size(49, 13);
            this.lblColumn.TabIndex = 2;
            this.lblColumn.Text = "Столбец";
            // 
            // cbTableColumn
            // 
            this.cbTableColumn.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTableColumn.FormattingEnabled = true;
            this.cbTableColumn.Location = new System.Drawing.Point(12, 64);
            this.cbTableColumn.Name = "cbTableColumn";
            this.cbTableColumn.Size = new System.Drawing.Size(320, 21);
            this.cbTableColumn.TabIndex = 3;
            this.cbTableColumn.SelectedIndexChanged += new System.EventHandler(this.cbColumn_SelectedIndexChanged);
            // 
            // lblFind
            // 
            this.lblFind.AutoSize = true;
            this.lblFind.Location = new System.Drawing.Point(9, 88);
            this.lblFind.Name = "lblFind";
            this.lblFind.Size = new System.Drawing.Size(38, 13);
            this.lblFind.TabIndex = 4;
            this.lblFind.Text = "Найти";
            // 
            // txtFind
            // 
            this.txtFind.Location = new System.Drawing.Point(12, 104);
            this.txtFind.Name = "txtFind";
            this.txtFind.Size = new System.Drawing.Size(320, 20);
            this.txtFind.TabIndex = 5;
            this.txtFind.TextChanged += new System.EventHandler(this.txtFind_TextChanged);
            // 
            // lblReplaceWith
            // 
            this.lblReplaceWith.AutoSize = true;
            this.lblReplaceWith.Location = new System.Drawing.Point(9, 126);
            this.lblReplaceWith.Name = "lblReplaceWith";
            this.lblReplaceWith.Size = new System.Drawing.Size(72, 13);
            this.lblReplaceWith.TabIndex = 7;
            this.lblReplaceWith.Text = "Заменить на";
            // 
            // txtReplaceWith
            // 
            this.txtReplaceWith.Location = new System.Drawing.Point(12, 143);
            this.txtReplaceWith.Name = "txtReplaceWith";
            this.txtReplaceWith.Size = new System.Drawing.Size(320, 20);
            this.txtReplaceWith.TabIndex = 8;
            // 
            // chkCaseSensitive
            // 
            this.chkCaseSensitive.AutoSize = true;
            this.chkCaseSensitive.Location = new System.Drawing.Point(12, 169);
            this.chkCaseSensitive.Name = "chkCaseSensitive";
            this.chkCaseSensitive.Size = new System.Drawing.Size(124, 17);
            this.chkCaseSensitive.TabIndex = 10;
            this.chkCaseSensitive.Text = "Учитывать регистр";
            this.chkCaseSensitive.UseVisualStyleBackColor = true;
            // 
            // chkWholeCellOnly
            // 
            this.chkWholeCellOnly.AutoSize = true;
            this.chkWholeCellOnly.Checked = true;
            this.chkWholeCellOnly.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkWholeCellOnly.Location = new System.Drawing.Point(12, 192);
            this.chkWholeCellOnly.Name = "chkWholeCellOnly";
            this.chkWholeCellOnly.Size = new System.Drawing.Size(147, 17);
            this.chkWholeCellOnly.TabIndex = 11;
            this.chkWholeCellOnly.Text = "Только ячейку целиком";
            this.chkWholeCellOnly.UseVisualStyleBackColor = true;
            // 
            // cbFind
            // 
            this.cbFind.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFind.FormattingEnabled = true;
            this.cbFind.Location = new System.Drawing.Point(12, 104);
            this.cbFind.Name = "cbFind";
            this.cbFind.Size = new System.Drawing.Size(320, 21);
            this.cbFind.TabIndex = 6;
            this.cbFind.Visible = false;
            this.cbFind.SelectedIndexChanged += new System.EventHandler(this.cbFind_SelectedIndexChanged);
            // 
            // cbReplaceWith
            // 
            this.cbReplaceWith.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbReplaceWith.FormattingEnabled = true;
            this.cbReplaceWith.Location = new System.Drawing.Point(12, 142);
            this.cbReplaceWith.Name = "cbReplaceWith";
            this.cbReplaceWith.Size = new System.Drawing.Size(320, 21);
            this.cbReplaceWith.TabIndex = 9;
            this.cbReplaceWith.Visible = false;
            // 
            // btnFindNext
            // 
            this.btnFindNext.Location = new System.Drawing.Point(50, 215);
            this.btnFindNext.Name = "btnFindNext";
            this.btnFindNext.Size = new System.Drawing.Size(90, 23);
            this.btnFindNext.TabIndex = 12;
            this.btnFindNext.Text = "Найти далее";
            this.btnFindNext.UseVisualStyleBackColor = true;
            this.btnFindNext.Click += new System.EventHandler(this.btnFindNext_Click);
            // 
            // btnReplace
            // 
            this.btnReplace.Location = new System.Drawing.Point(146, 215);
            this.btnReplace.Name = "btnReplace";
            this.btnReplace.Size = new System.Drawing.Size(90, 23);
            this.btnReplace.TabIndex = 13;
            this.btnReplace.Text = "Заменить";
            this.btnReplace.UseVisualStyleBackColor = true;
            this.btnReplace.Click += new System.EventHandler(this.btnReplace_Click);
            // 
            // FrmReplace
            // 
            this.AcceptButton = this.btnFindNext;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(344, 279);
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
            this.Controls.Add(this.cbTableColumn);
            this.Controls.Add(this.lblColumn);
            this.Controls.Add(this.txtTable);
            this.Controls.Add(this.btnReplaceAll);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lblTable);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmReplace";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Найти и заменить";
            this.Load += new System.EventHandler(this.FrmReplace_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTable;
        private System.Windows.Forms.Button btnReplaceAll;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TextBox txtTable;
        private System.Windows.Forms.Label lblColumn;
        private System.Windows.Forms.ComboBox cbTableColumn;
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