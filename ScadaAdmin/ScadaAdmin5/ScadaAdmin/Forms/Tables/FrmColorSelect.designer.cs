namespace Scada.Admin.App.Forms.Tables
{
    partial class FrmColorSelect
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
            this.lbColor = new System.Windows.Forms.ListBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblSort = new System.Windows.Forms.Label();
            this.rbSortByAbc = new System.Windows.Forms.RadioButton();
            this.rbSortByColor = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // lbColor
            // 
            this.lbColor.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lbColor.FormattingEnabled = true;
            this.lbColor.IntegralHeight = false;
            this.lbColor.ItemHeight = 16;
            this.lbColor.Location = new System.Drawing.Point(12, 35);
            this.lbColor.Name = "lbColor";
            this.lbColor.Size = new System.Drawing.Size(260, 285);
            this.lbColor.TabIndex = 3;
            this.lbColor.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lbColor_DrawItem);
            this.lbColor.DoubleClick += new System.EventHandler(this.lbColor_DoubleClick);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(116, 326);
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
            this.btnCancel.Location = new System.Drawing.Point(197, 326);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // lblSort
            // 
            this.lblSort.AutoSize = true;
            this.lblSort.Location = new System.Drawing.Point(12, 14);
            this.lblSort.Name = "lblSort";
            this.lblSort.Size = new System.Drawing.Size(29, 13);
            this.lblSort.TabIndex = 0;
            this.lblSort.Text = "Sort:";
            // 
            // rbSortByAbc
            // 
            this.rbSortByAbc.AutoSize = true;
            this.rbSortByAbc.Checked = true;
            this.rbSortByAbc.Location = new System.Drawing.Point(90, 12);
            this.rbSortByAbc.Name = "rbSortByAbc";
            this.rbSortByAbc.Size = new System.Drawing.Size(90, 17);
            this.rbSortByAbc.TabIndex = 1;
            this.rbSortByAbc.TabStop = true;
            this.rbSortByAbc.Text = "Alphabetically";
            this.rbSortByAbc.UseVisualStyleBackColor = true;
            this.rbSortByAbc.CheckedChanged += new System.EventHandler(this.rbSortAbc_CheckedChanged);
            // 
            // rbSortByColor
            // 
            this.rbSortByColor.AutoSize = true;
            this.rbSortByColor.Location = new System.Drawing.Point(190, 12);
            this.rbSortByColor.Name = "rbSortByColor";
            this.rbSortByColor.Size = new System.Drawing.Size(63, 17);
            this.rbSortByColor.TabIndex = 2;
            this.rbSortByColor.Text = "By color";
            this.rbSortByColor.UseVisualStyleBackColor = true;
            this.rbSortByColor.CheckedChanged += new System.EventHandler(this.rbSortColor_CheckedChanged);
            // 
            // FrmColorSelect
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(284, 361);
            this.Controls.Add(this.rbSortByColor);
            this.Controls.Add(this.rbSortByAbc);
            this.Controls.Add(this.lblSort);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lbColor);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmColorSelect";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select Color";
            this.Load += new System.EventHandler(this.FrmColor_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lbColor;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblSort;
        private System.Windows.Forms.RadioButton rbSortByAbc;
        private System.Windows.Forms.RadioButton rbSortByColor;
    }
}