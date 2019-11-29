namespace Scada.Scheme.Editor
{
    partial class FrmPasteSpecial
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
            this.lblInCnlOffset = new System.Windows.Forms.Label();
            this.numInCnlOffset = new System.Windows.Forms.NumericUpDown();
            this.lblCtrlCnlOffset = new System.Windows.Forms.Label();
            this.numCtrlCnlOffset = new System.Windows.Forms.NumericUpDown();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnPaste = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numInCnlOffset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCtrlCnlOffset)).BeginInit();
            this.SuspendLayout();
            // 
            // lblInCnlOffset
            // 
            this.lblInCnlOffset.AutoSize = true;
            this.lblInCnlOffset.Location = new System.Drawing.Point(12, 16);
            this.lblInCnlOffset.Name = "lblInCnlOffset";
            this.lblInCnlOffset.Size = new System.Drawing.Size(172, 13);
            this.lblInCnlOffset.TabIndex = 0;
            this.lblInCnlOffset.Text = "Increase input channel numbers by";
            // 
            // numInCnlOffset
            // 
            this.numInCnlOffset.Location = new System.Drawing.Point(242, 12);
            this.numInCnlOffset.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numInCnlOffset.Name = "numInCnlOffset";
            this.numInCnlOffset.Size = new System.Drawing.Size(100, 20);
            this.numInCnlOffset.TabIndex = 1;
            // 
            // lblCtrlCnlOffset
            // 
            this.lblCtrlCnlOffset.AutoSize = true;
            this.lblCtrlCnlOffset.Location = new System.Drawing.Point(12, 42);
            this.lblCtrlCnlOffset.Name = "lblCtrlCnlOffset";
            this.lblCtrlCnlOffset.Size = new System.Drawing.Size(179, 13);
            this.lblCtrlCnlOffset.TabIndex = 2;
            this.lblCtrlCnlOffset.Text = "Increase output channel numbers by";
            // 
            // numCtrlCnlOffset
            // 
            this.numCtrlCnlOffset.Location = new System.Drawing.Point(242, 38);
            this.numCtrlCnlOffset.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numCtrlCnlOffset.Name = "numCtrlCnlOffset";
            this.numCtrlCnlOffset.Size = new System.Drawing.Size(100, 20);
            this.numCtrlCnlOffset.TabIndex = 3;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(267, 74);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnPaste
            // 
            this.btnPaste.Location = new System.Drawing.Point(186, 74);
            this.btnPaste.Name = "btnPaste";
            this.btnPaste.Size = new System.Drawing.Size(75, 23);
            this.btnPaste.TabIndex = 4;
            this.btnPaste.Text = "Paste";
            this.btnPaste.UseVisualStyleBackColor = true;
            this.btnPaste.Click += new System.EventHandler(this.btnPaste_Click);
            // 
            // FrmPasteSpecial
            // 
            this.AcceptButton = this.btnPaste;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(354, 109);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnPaste);
            this.Controls.Add(this.numCtrlCnlOffset);
            this.Controls.Add(this.lblCtrlCnlOffset);
            this.Controls.Add(this.numInCnlOffset);
            this.Controls.Add(this.lblInCnlOffset);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmPasteSpecial";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Paste Special";
            this.Load += new System.EventHandler(this.FrmPasteSpecial_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numInCnlOffset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCtrlCnlOffset)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblInCnlOffset;
        private System.Windows.Forms.NumericUpDown numInCnlOffset;
        private System.Windows.Forms.Label lblCtrlCnlOffset;
        private System.Windows.Forms.NumericUpDown numCtrlCnlOffset;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnPaste;
    }
}