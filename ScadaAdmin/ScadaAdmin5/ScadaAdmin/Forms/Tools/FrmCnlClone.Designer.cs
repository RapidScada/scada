namespace Scada.Admin.App.Forms.Tools
{
    partial class FrmCnlClone
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
            this.gbCnlKind = new System.Windows.Forms.GroupBox();
            this.rbOutCnls = new System.Windows.Forms.RadioButton();
            this.rbInCnls = new System.Windows.Forms.RadioButton();
            this.gbSrcNums = new System.Windows.Forms.GroupBox();
            this.numSrcEndNum = new System.Windows.Forms.NumericUpDown();
            this.lblSrcEndNum = new System.Windows.Forms.Label();
            this.numSrcStartNum = new System.Windows.Forms.NumericUpDown();
            this.lblSrcStartNum = new System.Windows.Forms.Label();
            this.gbDestNums = new System.Windows.Forms.GroupBox();
            this.numDestEndNum = new System.Windows.Forms.NumericUpDown();
            this.lblDestEndNum = new System.Windows.Forms.Label();
            this.numDestStartNum = new System.Windows.Forms.NumericUpDown();
            this.lblDestStartNum = new System.Windows.Forms.Label();
            this.gbOptions = new System.Windows.Forms.GroupBox();
            this.chkUpdateFormulas = new System.Windows.Forms.CheckBox();
            this.cbReplaceKP = new System.Windows.Forms.ComboBox();
            this.lblReplaceKP = new System.Windows.Forms.Label();
            this.cbReplaceObj = new System.Windows.Forms.ComboBox();
            this.lblReplaceObj = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnClone = new System.Windows.Forms.Button();
            this.gbCnlKind.SuspendLayout();
            this.gbSrcNums.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSrcEndNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSrcStartNum)).BeginInit();
            this.gbDestNums.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDestEndNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDestStartNum)).BeginInit();
            this.gbOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbCnlKind
            // 
            this.gbCnlKind.Controls.Add(this.rbOutCnls);
            this.gbCnlKind.Controls.Add(this.rbInCnls);
            this.gbCnlKind.Location = new System.Drawing.Point(12, 12);
            this.gbCnlKind.Name = "gbCnlKind";
            this.gbCnlKind.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbCnlKind.Size = new System.Drawing.Size(310, 49);
            this.gbCnlKind.TabIndex = 0;
            this.gbCnlKind.TabStop = false;
            this.gbCnlKind.Text = "Channel Kind";
            // 
            // rbOutCnls
            // 
            this.rbOutCnls.AutoSize = true;
            this.rbOutCnls.Location = new System.Drawing.Point(158, 19);
            this.rbOutCnls.Name = "rbOutCnls";
            this.rbOutCnls.Size = new System.Drawing.Size(103, 17);
            this.rbOutCnls.TabIndex = 1;
            this.rbOutCnls.Text = "Output channels";
            this.rbOutCnls.UseVisualStyleBackColor = true;
            // 
            // rbInCnls
            // 
            this.rbInCnls.AutoSize = true;
            this.rbInCnls.Checked = true;
            this.rbInCnls.Location = new System.Drawing.Point(13, 19);
            this.rbInCnls.Name = "rbInCnls";
            this.rbInCnls.Size = new System.Drawing.Size(95, 17);
            this.rbInCnls.TabIndex = 0;
            this.rbInCnls.TabStop = true;
            this.rbInCnls.Text = "Input channels";
            this.rbInCnls.UseVisualStyleBackColor = true;
            // 
            // gbSrcNums
            // 
            this.gbSrcNums.Controls.Add(this.numSrcEndNum);
            this.gbSrcNums.Controls.Add(this.lblSrcEndNum);
            this.gbSrcNums.Controls.Add(this.numSrcStartNum);
            this.gbSrcNums.Controls.Add(this.lblSrcStartNum);
            this.gbSrcNums.Location = new System.Drawing.Point(12, 67);
            this.gbSrcNums.Name = "gbSrcNums";
            this.gbSrcNums.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbSrcNums.Size = new System.Drawing.Size(310, 65);
            this.gbSrcNums.TabIndex = 1;
            this.gbSrcNums.TabStop = false;
            this.gbSrcNums.Text = "Source Numbers";
            // 
            // numSrcEndNum
            // 
            this.numSrcEndNum.Location = new System.Drawing.Point(158, 32);
            this.numSrcEndNum.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numSrcEndNum.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numSrcEndNum.Name = "numSrcEndNum";
            this.numSrcEndNum.Size = new System.Drawing.Size(139, 20);
            this.numSrcEndNum.TabIndex = 5;
            this.numSrcEndNum.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numSrcEndNum.ValueChanged += new System.EventHandler(this.num_ValueChanged);
            // 
            // lblSrcEndNum
            // 
            this.lblSrcEndNum.AutoSize = true;
            this.lblSrcEndNum.Location = new System.Drawing.Point(155, 16);
            this.lblSrcEndNum.Name = "lblSrcEndNum";
            this.lblSrcEndNum.Size = new System.Drawing.Size(26, 13);
            this.lblSrcEndNum.TabIndex = 3;
            this.lblSrcEndNum.Text = "End";
            // 
            // numSrcStartNum
            // 
            this.numSrcStartNum.Location = new System.Drawing.Point(13, 32);
            this.numSrcStartNum.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numSrcStartNum.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numSrcStartNum.Name = "numSrcStartNum";
            this.numSrcStartNum.Size = new System.Drawing.Size(139, 20);
            this.numSrcStartNum.TabIndex = 2;
            this.numSrcStartNum.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numSrcStartNum.ValueChanged += new System.EventHandler(this.num_ValueChanged);
            // 
            // lblSrcStartNum
            // 
            this.lblSrcStartNum.AutoSize = true;
            this.lblSrcStartNum.Location = new System.Drawing.Point(10, 16);
            this.lblSrcStartNum.Name = "lblSrcStartNum";
            this.lblSrcStartNum.Size = new System.Drawing.Size(29, 13);
            this.lblSrcStartNum.TabIndex = 0;
            this.lblSrcStartNum.Text = "Start";
            // 
            // gbDestNums
            // 
            this.gbDestNums.Controls.Add(this.numDestEndNum);
            this.gbDestNums.Controls.Add(this.lblDestEndNum);
            this.gbDestNums.Controls.Add(this.numDestStartNum);
            this.gbDestNums.Controls.Add(this.lblDestStartNum);
            this.gbDestNums.Location = new System.Drawing.Point(12, 138);
            this.gbDestNums.Name = "gbDestNums";
            this.gbDestNums.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbDestNums.Size = new System.Drawing.Size(310, 65);
            this.gbDestNums.TabIndex = 2;
            this.gbDestNums.TabStop = false;
            this.gbDestNums.Text = "Destination Numbers";
            // 
            // numDestEndNum
            // 
            this.numDestEndNum.Increment = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numDestEndNum.Location = new System.Drawing.Point(158, 32);
            this.numDestEndNum.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.numDestEndNum.Name = "numDestEndNum";
            this.numDestEndNum.ReadOnly = true;
            this.numDestEndNum.Size = new System.Drawing.Size(139, 20);
            this.numDestEndNum.TabIndex = 4;
            // 
            // lblDestEndNum
            // 
            this.lblDestEndNum.AutoSize = true;
            this.lblDestEndNum.Location = new System.Drawing.Point(155, 16);
            this.lblDestEndNum.Name = "lblDestEndNum";
            this.lblDestEndNum.Size = new System.Drawing.Size(26, 13);
            this.lblDestEndNum.TabIndex = 3;
            this.lblDestEndNum.Text = "End";
            // 
            // numDestStartNum
            // 
            this.numDestStartNum.Location = new System.Drawing.Point(13, 32);
            this.numDestStartNum.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numDestStartNum.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numDestStartNum.Name = "numDestStartNum";
            this.numDestStartNum.Size = new System.Drawing.Size(139, 20);
            this.numDestStartNum.TabIndex = 2;
            this.numDestStartNum.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numDestStartNum.ValueChanged += new System.EventHandler(this.num_ValueChanged);
            // 
            // lblDestStartNum
            // 
            this.lblDestStartNum.AutoSize = true;
            this.lblDestStartNum.Location = new System.Drawing.Point(10, 16);
            this.lblDestStartNum.Name = "lblDestStartNum";
            this.lblDestStartNum.Size = new System.Drawing.Size(29, 13);
            this.lblDestStartNum.TabIndex = 0;
            this.lblDestStartNum.Text = "Start";
            // 
            // gbOptions
            // 
            this.gbOptions.Controls.Add(this.chkUpdateFormulas);
            this.gbOptions.Controls.Add(this.cbReplaceKP);
            this.gbOptions.Controls.Add(this.lblReplaceKP);
            this.gbOptions.Controls.Add(this.cbReplaceObj);
            this.gbOptions.Controls.Add(this.lblReplaceObj);
            this.gbOptions.Location = new System.Drawing.Point(12, 209);
            this.gbOptions.Name = "gbOptions";
            this.gbOptions.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbOptions.Size = new System.Drawing.Size(310, 94);
            this.gbOptions.TabIndex = 3;
            this.gbOptions.TabStop = false;
            this.gbOptions.Text = "Options";
            // 
            // chkUpdateFormulas
            // 
            this.chkUpdateFormulas.AutoSize = true;
            this.chkUpdateFormulas.Location = new System.Drawing.Point(13, 64);
            this.chkUpdateFormulas.Name = "chkUpdateFormulas";
            this.chkUpdateFormulas.Size = new System.Drawing.Size(198, 17);
            this.chkUpdateFormulas.TabIndex = 4;
            this.chkUpdateFormulas.Text = "Update channel numbers in formulas";
            this.chkUpdateFormulas.UseVisualStyleBackColor = true;
            // 
            // cbReplaceKP
            // 
            this.cbReplaceKP.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbReplaceKP.FormattingEnabled = true;
            this.cbReplaceKP.Location = new System.Drawing.Point(158, 32);
            this.cbReplaceKP.Name = "cbReplaceKP";
            this.cbReplaceKP.Size = new System.Drawing.Size(139, 21);
            this.cbReplaceKP.TabIndex = 3;
            // 
            // lblReplaceKP
            // 
            this.lblReplaceKP.AutoSize = true;
            this.lblReplaceKP.Location = new System.Drawing.Point(155, 16);
            this.lblReplaceKP.Name = "lblReplaceKP";
            this.lblReplaceKP.Size = new System.Drawing.Size(82, 13);
            this.lblReplaceKP.TabIndex = 2;
            this.lblReplaceKP.Text = "Replace device";
            // 
            // cbReplaceObj
            // 
            this.cbReplaceObj.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbReplaceObj.FormattingEnabled = true;
            this.cbReplaceObj.Location = new System.Drawing.Point(13, 32);
            this.cbReplaceObj.Name = "cbReplaceObj";
            this.cbReplaceObj.Size = new System.Drawing.Size(139, 21);
            this.cbReplaceObj.TabIndex = 1;
            // 
            // lblReplaceObj
            // 
            this.lblReplaceObj.AutoSize = true;
            this.lblReplaceObj.Location = new System.Drawing.Point(10, 16);
            this.lblReplaceObj.Name = "lblReplaceObj";
            this.lblReplaceObj.Size = new System.Drawing.Size(79, 13);
            this.lblReplaceObj.TabIndex = 0;
            this.lblReplaceObj.Text = "Replace object";
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(247, 309);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // btnClone
            // 
            this.btnClone.Location = new System.Drawing.Point(151, 309);
            this.btnClone.Name = "btnClone";
            this.btnClone.Size = new System.Drawing.Size(90, 23);
            this.btnClone.TabIndex = 4;
            this.btnClone.Text = "Clone";
            this.btnClone.UseVisualStyleBackColor = true;
            this.btnClone.Click += new System.EventHandler(this.btnClone_Click);
            // 
            // FrmCnlClone
            // 
            this.AcceptButton = this.btnClone;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(334, 344);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnClone);
            this.Controls.Add(this.gbOptions);
            this.Controls.Add(this.gbDestNums);
            this.Controls.Add(this.gbSrcNums);
            this.Controls.Add(this.gbCnlKind);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmCnlClone";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Clone Channels";
            this.Load += new System.EventHandler(this.FrmCnlClone_Load);
            this.gbCnlKind.ResumeLayout(false);
            this.gbCnlKind.PerformLayout();
            this.gbSrcNums.ResumeLayout(false);
            this.gbSrcNums.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSrcEndNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSrcStartNum)).EndInit();
            this.gbDestNums.ResumeLayout(false);
            this.gbDestNums.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDestEndNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDestStartNum)).EndInit();
            this.gbOptions.ResumeLayout(false);
            this.gbOptions.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbCnlKind;
        private System.Windows.Forms.RadioButton rbOutCnls;
        private System.Windows.Forms.RadioButton rbInCnls;
        private System.Windows.Forms.GroupBox gbSrcNums;
        private System.Windows.Forms.NumericUpDown numSrcEndNum;
        private System.Windows.Forms.Label lblSrcEndNum;
        private System.Windows.Forms.NumericUpDown numSrcStartNum;
        private System.Windows.Forms.Label lblSrcStartNum;
        private System.Windows.Forms.GroupBox gbDestNums;
        private System.Windows.Forms.NumericUpDown numDestEndNum;
        private System.Windows.Forms.Label lblDestEndNum;
        private System.Windows.Forms.NumericUpDown numDestStartNum;
        private System.Windows.Forms.Label lblDestStartNum;
        private System.Windows.Forms.GroupBox gbOptions;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnClone;
        private System.Windows.Forms.ComboBox cbReplaceKP;
        private System.Windows.Forms.Label lblReplaceKP;
        private System.Windows.Forms.ComboBox cbReplaceObj;
        private System.Windows.Forms.Label lblReplaceObj;
        private System.Windows.Forms.CheckBox chkUpdateFormulas;
    }
}