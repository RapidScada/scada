namespace Scada.Comm.Devices.Modbus.UI
{
    partial class FrmTemplateSettings
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
            this.lblAddressing = new System.Windows.Forms.Label();
            this.rbZeroBased = new System.Windows.Forms.RadioButton();
            this.rbOneBased = new System.Windows.Forms.RadioButton();
            this.pnlBase = new System.Windows.Forms.Panel();
            this.pnlNotation = new System.Windows.Forms.Panel();
            this.rbDec = new System.Windows.Forms.RadioButton();
            this.rbHex = new System.Windows.Forms.RadioButton();
            this.lblDefByteOrder = new System.Windows.Forms.Label();
            this.lblDefByteOrderExample = new System.Windows.Forms.Label();
            this.txtDefByteOrder = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.pnlBase.SuspendLayout();
            this.pnlNotation.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblAddressing
            // 
            this.lblAddressing.AutoSize = true;
            this.lblAddressing.Location = new System.Drawing.Point(9, 9);
            this.lblAddressing.Name = "lblAddressing";
            this.lblAddressing.Size = new System.Drawing.Size(62, 13);
            this.lblAddressing.TabIndex = 0;
            this.lblAddressing.Text = "Addressing:";
            // 
            // rbZeroBased
            // 
            this.rbZeroBased.AutoSize = true;
            this.rbZeroBased.Location = new System.Drawing.Point(0, 0);
            this.rbZeroBased.Name = "rbZeroBased";
            this.rbZeroBased.Size = new System.Drawing.Size(79, 17);
            this.rbZeroBased.TabIndex = 0;
            this.rbZeroBased.TabStop = true;
            this.rbZeroBased.Text = "Zero-based";
            this.rbZeroBased.UseVisualStyleBackColor = true;
            this.rbZeroBased.CheckedChanged += new System.EventHandler(this.control_Changed);
            // 
            // rbOneBased
            // 
            this.rbOneBased.AutoSize = true;
            this.rbOneBased.Location = new System.Drawing.Point(120, 0);
            this.rbOneBased.Name = "rbOneBased";
            this.rbOneBased.Size = new System.Drawing.Size(77, 17);
            this.rbOneBased.TabIndex = 1;
            this.rbOneBased.TabStop = true;
            this.rbOneBased.Text = "One-based";
            this.rbOneBased.UseVisualStyleBackColor = true;
            // 
            // pnlBase
            // 
            this.pnlBase.Controls.Add(this.rbZeroBased);
            this.pnlBase.Controls.Add(this.rbOneBased);
            this.pnlBase.Location = new System.Drawing.Point(12, 25);
            this.pnlBase.Name = "pnlBase";
            this.pnlBase.Size = new System.Drawing.Size(260, 17);
            this.pnlBase.TabIndex = 1;
            // 
            // pnlNotation
            // 
            this.pnlNotation.Controls.Add(this.rbDec);
            this.pnlNotation.Controls.Add(this.rbHex);
            this.pnlNotation.Location = new System.Drawing.Point(12, 48);
            this.pnlNotation.Name = "pnlNotation";
            this.pnlNotation.Size = new System.Drawing.Size(260, 17);
            this.pnlNotation.TabIndex = 2;
            // 
            // rbDec
            // 
            this.rbDec.AutoSize = true;
            this.rbDec.Location = new System.Drawing.Point(0, 0);
            this.rbDec.Name = "rbDec";
            this.rbDec.Size = new System.Drawing.Size(63, 17);
            this.rbDec.TabIndex = 0;
            this.rbDec.TabStop = true;
            this.rbDec.Text = "Decimal";
            this.rbDec.UseVisualStyleBackColor = true;
            this.rbDec.CheckedChanged += new System.EventHandler(this.control_Changed);
            // 
            // rbHex
            // 
            this.rbHex.AutoSize = true;
            this.rbHex.Location = new System.Drawing.Point(120, 0);
            this.rbHex.Name = "rbHex";
            this.rbHex.Size = new System.Drawing.Size(86, 17);
            this.rbHex.TabIndex = 1;
            this.rbHex.TabStop = true;
            this.rbHex.Text = "Hexadecimal";
            this.rbHex.UseVisualStyleBackColor = true;
            // 
            // lblDefByteOrder
            // 
            this.lblDefByteOrder.AutoSize = true;
            this.lblDefByteOrder.Location = new System.Drawing.Point(9, 78);
            this.lblDefByteOrder.Name = "lblDefByteOrder";
            this.lblDefByteOrder.Size = new System.Drawing.Size(91, 13);
            this.lblDefByteOrder.TabIndex = 3;
            this.lblDefByteOrder.Text = "Default byte order";
            // 
            // lblDefByteOrderExample
            // 
            this.lblDefByteOrderExample.AutoSize = true;
            this.lblDefByteOrderExample.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblDefByteOrderExample.Location = new System.Drawing.Point(9, 117);
            this.lblDefByteOrderExample.Name = "lblDefByteOrderExample";
            this.lblDefByteOrderExample.Size = new System.Drawing.Size(118, 13);
            this.lblDefByteOrderExample.TabIndex = 5;
            this.lblDefByteOrderExample.Text = "For example, 01234567";
            // 
            // txtDefByteOrder
            // 
            this.txtDefByteOrder.Location = new System.Drawing.Point(12, 94);
            this.txtDefByteOrder.Name = "txtDefByteOrder";
            this.txtDefByteOrder.Size = new System.Drawing.Size(260, 20);
            this.txtDefByteOrder.TabIndex = 4;
            this.txtDefByteOrder.TextChanged += new System.EventHandler(this.control_Changed);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(197, 138);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(116, 138);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // FrmTemplateSettings
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(284, 173);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lblDefByteOrderExample);
            this.Controls.Add(this.txtDefByteOrder);
            this.Controls.Add(this.lblDefByteOrder);
            this.Controls.Add(this.pnlNotation);
            this.Controls.Add(this.pnlBase);
            this.Controls.Add(this.lblAddressing);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmTemplateSettings";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Template Settings";
            this.Load += new System.EventHandler(this.FrmTemplateSettings_Load);
            this.pnlBase.ResumeLayout(false);
            this.pnlBase.PerformLayout();
            this.pnlNotation.ResumeLayout(false);
            this.pnlNotation.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblAddressing;
        private System.Windows.Forms.RadioButton rbZeroBased;
        private System.Windows.Forms.RadioButton rbOneBased;
        private System.Windows.Forms.Panel pnlBase;
        private System.Windows.Forms.Panel pnlNotation;
        private System.Windows.Forms.RadioButton rbDec;
        private System.Windows.Forms.RadioButton rbHex;
        private System.Windows.Forms.Label lblDefByteOrder;
        private System.Windows.Forms.Label lblDefByteOrderExample;
        private System.Windows.Forms.TextBox txtDefByteOrder;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
    }
}