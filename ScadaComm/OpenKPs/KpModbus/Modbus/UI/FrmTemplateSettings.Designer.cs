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
            this.rbZeroBased = new System.Windows.Forms.RadioButton();
            this.rbOneBased = new System.Windows.Forms.RadioButton();
            this.pnlBase = new System.Windows.Forms.Panel();
            this.pnlNotation = new System.Windows.Forms.Panel();
            this.rbDec = new System.Windows.Forms.RadioButton();
            this.rbHex = new System.Windows.Forms.RadioButton();
            this.lblDefByteOrderExample = new System.Windows.Forms.Label();
            this.txtDefByteOrder2 = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.gbAddressing = new System.Windows.Forms.GroupBox();
            this.gbDefByteOrder = new System.Windows.Forms.GroupBox();
            this.lblDefByteOrder8 = new System.Windows.Forms.Label();
            this.txtDefByteOrder8 = new System.Windows.Forms.TextBox();
            this.lblDefByteOrder4 = new System.Windows.Forms.Label();
            this.txtDefByteOrder4 = new System.Windows.Forms.TextBox();
            this.lblDefByteOrder2 = new System.Windows.Forms.Label();
            this.pnlBase.SuspendLayout();
            this.pnlNotation.SuspendLayout();
            this.gbAddressing.SuspendLayout();
            this.gbDefByteOrder.SuspendLayout();
            this.SuspendLayout();
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
            this.pnlBase.Location = new System.Drawing.Point(13, 19);
            this.pnlBase.Name = "pnlBase";
            this.pnlBase.Size = new System.Drawing.Size(230, 17);
            this.pnlBase.TabIndex = 0;
            // 
            // pnlNotation
            // 
            this.pnlNotation.Controls.Add(this.rbDec);
            this.pnlNotation.Controls.Add(this.rbHex);
            this.pnlNotation.Location = new System.Drawing.Point(13, 42);
            this.pnlNotation.Name = "pnlNotation";
            this.pnlNotation.Size = new System.Drawing.Size(230, 17);
            this.pnlNotation.TabIndex = 1;
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
            // lblDefByteOrderExample
            // 
            this.lblDefByteOrderExample.AutoSize = true;
            this.lblDefByteOrderExample.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblDefByteOrderExample.Location = new System.Drawing.Point(10, 133);
            this.lblDefByteOrderExample.Name = "lblDefByteOrderExample";
            this.lblDefByteOrderExample.Size = new System.Drawing.Size(118, 13);
            this.lblDefByteOrderExample.TabIndex = 6;
            this.lblDefByteOrderExample.Text = "For example, 01234567";
            // 
            // txtDefByteOrder2
            // 
            this.txtDefByteOrder2.Location = new System.Drawing.Point(13, 32);
            this.txtDefByteOrder2.Name = "txtDefByteOrder2";
            this.txtDefByteOrder2.Size = new System.Drawing.Size(234, 20);
            this.txtDefByteOrder2.TabIndex = 1;
            this.txtDefByteOrder2.TextChanged += new System.EventHandler(this.control_Changed);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(197, 252);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(116, 252);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // gbAddressing
            // 
            this.gbAddressing.Controls.Add(this.pnlBase);
            this.gbAddressing.Controls.Add(this.pnlNotation);
            this.gbAddressing.Location = new System.Drawing.Point(12, 12);
            this.gbAddressing.Name = "gbAddressing";
            this.gbAddressing.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbAddressing.Size = new System.Drawing.Size(260, 72);
            this.gbAddressing.TabIndex = 0;
            this.gbAddressing.TabStop = false;
            this.gbAddressing.Text = "Addressing";
            // 
            // gbDefByteOrder
            // 
            this.gbDefByteOrder.Controls.Add(this.lblDefByteOrder8);
            this.gbDefByteOrder.Controls.Add(this.txtDefByteOrder8);
            this.gbDefByteOrder.Controls.Add(this.lblDefByteOrder4);
            this.gbDefByteOrder.Controls.Add(this.txtDefByteOrder4);
            this.gbDefByteOrder.Controls.Add(this.lblDefByteOrder2);
            this.gbDefByteOrder.Controls.Add(this.txtDefByteOrder2);
            this.gbDefByteOrder.Controls.Add(this.lblDefByteOrderExample);
            this.gbDefByteOrder.Location = new System.Drawing.Point(12, 90);
            this.gbDefByteOrder.Name = "gbDefByteOrder";
            this.gbDefByteOrder.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbDefByteOrder.Size = new System.Drawing.Size(260, 156);
            this.gbDefByteOrder.TabIndex = 1;
            this.gbDefByteOrder.TabStop = false;
            this.gbDefByteOrder.Text = "Default byte order";
            // 
            // lblDefByteOrder8
            // 
            this.lblDefByteOrder8.AutoSize = true;
            this.lblDefByteOrder8.Location = new System.Drawing.Point(10, 94);
            this.lblDefByteOrder8.Name = "lblDefByteOrder8";
            this.lblDefByteOrder8.Size = new System.Drawing.Size(41, 13);
            this.lblDefByteOrder8.TabIndex = 4;
            this.lblDefByteOrder8.Text = "8 bytes";
            // 
            // txtDefByteOrder8
            // 
            this.txtDefByteOrder8.Location = new System.Drawing.Point(13, 110);
            this.txtDefByteOrder8.Name = "txtDefByteOrder8";
            this.txtDefByteOrder8.Size = new System.Drawing.Size(234, 20);
            this.txtDefByteOrder8.TabIndex = 5;
            this.txtDefByteOrder8.TextChanged += new System.EventHandler(this.control_Changed);
            // 
            // lblDefByteOrder4
            // 
            this.lblDefByteOrder4.AutoSize = true;
            this.lblDefByteOrder4.Location = new System.Drawing.Point(10, 55);
            this.lblDefByteOrder4.Name = "lblDefByteOrder4";
            this.lblDefByteOrder4.Size = new System.Drawing.Size(41, 13);
            this.lblDefByteOrder4.TabIndex = 2;
            this.lblDefByteOrder4.Text = "4 bytes";
            // 
            // txtDefByteOrder4
            // 
            this.txtDefByteOrder4.Location = new System.Drawing.Point(13, 71);
            this.txtDefByteOrder4.Name = "txtDefByteOrder4";
            this.txtDefByteOrder4.Size = new System.Drawing.Size(234, 20);
            this.txtDefByteOrder4.TabIndex = 3;
            this.txtDefByteOrder4.TextChanged += new System.EventHandler(this.control_Changed);
            // 
            // lblDefByteOrder2
            // 
            this.lblDefByteOrder2.AutoSize = true;
            this.lblDefByteOrder2.Location = new System.Drawing.Point(10, 16);
            this.lblDefByteOrder2.Name = "lblDefByteOrder2";
            this.lblDefByteOrder2.Size = new System.Drawing.Size(41, 13);
            this.lblDefByteOrder2.TabIndex = 0;
            this.lblDefByteOrder2.Text = "2 bytes";
            // 
            // FrmTemplateSettings
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(284, 287);
            this.Controls.Add(this.gbDefByteOrder);
            this.Controls.Add(this.gbAddressing);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
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
            this.gbAddressing.ResumeLayout(false);
            this.gbDefByteOrder.ResumeLayout(false);
            this.gbDefByteOrder.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.RadioButton rbZeroBased;
        private System.Windows.Forms.RadioButton rbOneBased;
        private System.Windows.Forms.Panel pnlBase;
        private System.Windows.Forms.Panel pnlNotation;
        private System.Windows.Forms.RadioButton rbDec;
        private System.Windows.Forms.RadioButton rbHex;
        private System.Windows.Forms.Label lblDefByteOrderExample;
        private System.Windows.Forms.TextBox txtDefByteOrder2;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.GroupBox gbAddressing;
        private System.Windows.Forms.GroupBox gbDefByteOrder;
        private System.Windows.Forms.Label lblDefByteOrder2;
        private System.Windows.Forms.Label lblDefByteOrder8;
        private System.Windows.Forms.TextBox txtDefByteOrder8;
        private System.Windows.Forms.Label lblDefByteOrder4;
        private System.Windows.Forms.TextBox txtDefByteOrder4;
    }
}