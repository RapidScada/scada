namespace Scada.Comm.Devices.Modbus.UI
{
    partial class CtrlCmd
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.gbCmd = new System.Windows.Forms.GroupBox();
            this.lblCmdElemType = new System.Windows.Forms.Label();
            this.cbCmdElemType = new System.Windows.Forms.ComboBox();
            this.lblCmdAddressHint = new System.Windows.Forms.Label();
            this.lblCmdByteOrderExample = new System.Windows.Forms.Label();
            this.txtCmdByteOrder = new System.Windows.Forms.TextBox();
            this.lblCmdByteOrder = new System.Windows.Forms.Label();
            this.txtCmdFuncCode = new System.Windows.Forms.TextBox();
            this.lblCmdFuncCode = new System.Windows.Forms.Label();
            this.chkCmdMultiple = new System.Windows.Forms.CheckBox();
            this.lblCmdElemCnt = new System.Windows.Forms.Label();
            this.numCmdElemCnt = new System.Windows.Forms.NumericUpDown();
            this.txtCmdName = new System.Windows.Forms.TextBox();
            this.lblCmdName = new System.Windows.Forms.Label();
            this.lblCmdNum = new System.Windows.Forms.Label();
            this.numCmdNum = new System.Windows.Forms.NumericUpDown();
            this.numCmdAddress = new System.Windows.Forms.NumericUpDown();
            this.lblCmdAddress = new System.Windows.Forms.Label();
            this.lblCmdTableType = new System.Windows.Forms.Label();
            this.cbCmdTableType = new System.Windows.Forms.ComboBox();
            this.gbCmd.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCmdElemCnt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCmdNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCmdAddress)).BeginInit();
            this.SuspendLayout();
            // 
            // gbCmd
            // 
            this.gbCmd.Controls.Add(this.lblCmdElemType);
            this.gbCmd.Controls.Add(this.cbCmdElemType);
            this.gbCmd.Controls.Add(this.lblCmdAddressHint);
            this.gbCmd.Controls.Add(this.lblCmdByteOrderExample);
            this.gbCmd.Controls.Add(this.txtCmdByteOrder);
            this.gbCmd.Controls.Add(this.lblCmdByteOrder);
            this.gbCmd.Controls.Add(this.txtCmdFuncCode);
            this.gbCmd.Controls.Add(this.lblCmdFuncCode);
            this.gbCmd.Controls.Add(this.chkCmdMultiple);
            this.gbCmd.Controls.Add(this.lblCmdElemCnt);
            this.gbCmd.Controls.Add(this.numCmdElemCnt);
            this.gbCmd.Controls.Add(this.txtCmdName);
            this.gbCmd.Controls.Add(this.lblCmdName);
            this.gbCmd.Controls.Add(this.lblCmdNum);
            this.gbCmd.Controls.Add(this.numCmdNum);
            this.gbCmd.Controls.Add(this.numCmdAddress);
            this.gbCmd.Controls.Add(this.lblCmdAddress);
            this.gbCmd.Controls.Add(this.lblCmdTableType);
            this.gbCmd.Controls.Add(this.cbCmdTableType);
            this.gbCmd.Location = new System.Drawing.Point(0, 0);
            this.gbCmd.Name = "gbCmd";
            this.gbCmd.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbCmd.Size = new System.Drawing.Size(280, 324);
            this.gbCmd.TabIndex = 0;
            this.gbCmd.TabStop = false;
            this.gbCmd.Text = "Command parameters";
            // 
            // lblCmdElemType
            // 
            this.lblCmdElemType.AutoSize = true;
            this.lblCmdElemType.Location = new System.Drawing.Point(10, 196);
            this.lblCmdElemType.Name = "lblCmdElemType";
            this.lblCmdElemType.Size = new System.Drawing.Size(68, 13);
            this.lblCmdElemType.TabIndex = 10;
            this.lblCmdElemType.Text = "Element type";
            // 
            // cbCmdElemType
            // 
            this.cbCmdElemType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCmdElemType.FormattingEnabled = true;
            this.cbCmdElemType.Items.AddRange(new object[] {
            "Undefined",
            "ushort (2 bytes)",
            "short (2 bytes)",
            "uint (4 bytes)",
            "int (4 bytes)",
            "ulong (8 bytes)",
            "long (8 bytes)",
            "float (4 bytes)",
            "double (8 bytes)",
            "bool (1 bit)"});
            this.cbCmdElemType.Location = new System.Drawing.Point(13, 212);
            this.cbCmdElemType.Name = "cbCmdElemType";
            this.cbCmdElemType.Size = new System.Drawing.Size(124, 21);
            this.cbCmdElemType.TabIndex = 11;
            this.cbCmdElemType.SelectedIndexChanged += new System.EventHandler(this.cbCmdElemType_SelectedIndexChanged);
            // 
            // lblCmdAddressHint
            // 
            this.lblCmdAddressHint.AutoSize = true;
            this.lblCmdAddressHint.Location = new System.Drawing.Point(140, 177);
            this.lblCmdAddressHint.Name = "lblCmdAddressHint";
            this.lblCmdAddressHint.Size = new System.Drawing.Size(29, 13);
            this.lblCmdAddressHint.TabIndex = 9;
            this.lblCmdAddressHint.Text = "DEC";
            // 
            // lblCmdByteOrderExample
            // 
            this.lblCmdByteOrderExample.AutoSize = true;
            this.lblCmdByteOrderExample.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblCmdByteOrderExample.Location = new System.Drawing.Point(143, 256);
            this.lblCmdByteOrderExample.Name = "lblCmdByteOrderExample";
            this.lblCmdByteOrderExample.Size = new System.Drawing.Size(118, 13);
            this.lblCmdByteOrderExample.TabIndex = 16;
            this.lblCmdByteOrderExample.Text = "For example, 01234567";
            // 
            // txtCmdByteOrder
            // 
            this.txtCmdByteOrder.Location = new System.Drawing.Point(13, 252);
            this.txtCmdByteOrder.Name = "txtCmdByteOrder";
            this.txtCmdByteOrder.Size = new System.Drawing.Size(124, 20);
            this.txtCmdByteOrder.TabIndex = 15;
            this.txtCmdByteOrder.TextChanged += new System.EventHandler(this.txtCmdByteOrder_TextChanged);
            // 
            // lblCmdByteOrder
            // 
            this.lblCmdByteOrder.AutoSize = true;
            this.lblCmdByteOrder.Location = new System.Drawing.Point(10, 236);
            this.lblCmdByteOrder.Name = "lblCmdByteOrder";
            this.lblCmdByteOrder.Size = new System.Drawing.Size(55, 13);
            this.lblCmdByteOrder.TabIndex = 14;
            this.lblCmdByteOrder.Text = "Byte order";
            // 
            // txtCmdFuncCode
            // 
            this.txtCmdFuncCode.Location = new System.Drawing.Point(13, 134);
            this.txtCmdFuncCode.Name = "txtCmdFuncCode";
            this.txtCmdFuncCode.ReadOnly = true;
            this.txtCmdFuncCode.Size = new System.Drawing.Size(124, 20);
            this.txtCmdFuncCode.TabIndex = 6;
            // 
            // lblCmdFuncCode
            // 
            this.lblCmdFuncCode.AutoSize = true;
            this.lblCmdFuncCode.Location = new System.Drawing.Point(10, 118);
            this.lblCmdFuncCode.Name = "lblCmdFuncCode";
            this.lblCmdFuncCode.Size = new System.Drawing.Size(75, 13);
            this.lblCmdFuncCode.TabIndex = 5;
            this.lblCmdFuncCode.Text = "Function code";
            // 
            // chkCmdMultiple
            // 
            this.chkCmdMultiple.AutoSize = true;
            this.chkCmdMultiple.Location = new System.Drawing.Point(13, 98);
            this.chkCmdMultiple.Name = "chkCmdMultiple";
            this.chkCmdMultiple.Size = new System.Drawing.Size(62, 17);
            this.chkCmdMultiple.TabIndex = 4;
            this.chkCmdMultiple.Text = "Multiple";
            this.chkCmdMultiple.UseVisualStyleBackColor = true;
            this.chkCmdMultiple.CheckedChanged += new System.EventHandler(this.chkCmdMultiple_CheckedChanged);
            // 
            // lblCmdElemCnt
            // 
            this.lblCmdElemCnt.AutoSize = true;
            this.lblCmdElemCnt.Location = new System.Drawing.Point(140, 196);
            this.lblCmdElemCnt.Name = "lblCmdElemCnt";
            this.lblCmdElemCnt.Size = new System.Drawing.Size(75, 13);
            this.lblCmdElemCnt.TabIndex = 12;
            this.lblCmdElemCnt.Text = "Element count";
            // 
            // numCmdElemCnt
            // 
            this.numCmdElemCnt.Location = new System.Drawing.Point(143, 212);
            this.numCmdElemCnt.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.numCmdElemCnt.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numCmdElemCnt.Name = "numCmdElemCnt";
            this.numCmdElemCnt.Size = new System.Drawing.Size(124, 20);
            this.numCmdElemCnt.TabIndex = 13;
            this.numCmdElemCnt.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numCmdElemCnt.ValueChanged += new System.EventHandler(this.numCmdElemCnt_ValueChanged);
            // 
            // txtCmdName
            // 
            this.txtCmdName.Location = new System.Drawing.Point(13, 32);
            this.txtCmdName.Name = "txtCmdName";
            this.txtCmdName.Size = new System.Drawing.Size(254, 20);
            this.txtCmdName.TabIndex = 1;
            this.txtCmdName.TextChanged += new System.EventHandler(this.txtCmdName_TextChanged);
            // 
            // lblCmdName
            // 
            this.lblCmdName.AutoSize = true;
            this.lblCmdName.Location = new System.Drawing.Point(10, 16);
            this.lblCmdName.Name = "lblCmdName";
            this.lblCmdName.Size = new System.Drawing.Size(35, 13);
            this.lblCmdName.TabIndex = 0;
            this.lblCmdName.Text = "Name";
            // 
            // lblCmdNum
            // 
            this.lblCmdNum.AutoSize = true;
            this.lblCmdNum.Location = new System.Drawing.Point(10, 275);
            this.lblCmdNum.Name = "lblCmdNum";
            this.lblCmdNum.Size = new System.Drawing.Size(92, 13);
            this.lblCmdNum.TabIndex = 17;
            this.lblCmdNum.Text = "Command number";
            // 
            // numCmdNum
            // 
            this.numCmdNum.Location = new System.Drawing.Point(13, 291);
            this.numCmdNum.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numCmdNum.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numCmdNum.Name = "numCmdNum";
            this.numCmdNum.Size = new System.Drawing.Size(124, 20);
            this.numCmdNum.TabIndex = 18;
            this.numCmdNum.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numCmdNum.ValueChanged += new System.EventHandler(this.numCmdNum_ValueChanged);
            // 
            // numCmdAddress
            // 
            this.numCmdAddress.Location = new System.Drawing.Point(13, 173);
            this.numCmdAddress.Maximum = new decimal(new int[] {
            65536,
            0,
            0,
            0});
            this.numCmdAddress.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numCmdAddress.Name = "numCmdAddress";
            this.numCmdAddress.Size = new System.Drawing.Size(124, 20);
            this.numCmdAddress.TabIndex = 8;
            this.numCmdAddress.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numCmdAddress.ValueChanged += new System.EventHandler(this.numCmdAddress_ValueChanged);
            // 
            // lblCmdAddress
            // 
            this.lblCmdAddress.AutoSize = true;
            this.lblCmdAddress.Location = new System.Drawing.Point(10, 157);
            this.lblCmdAddress.Name = "lblCmdAddress";
            this.lblCmdAddress.Size = new System.Drawing.Size(85, 13);
            this.lblCmdAddress.TabIndex = 7;
            this.lblCmdAddress.Text = "Element address";
            // 
            // lblCmdTableType
            // 
            this.lblCmdTableType.AutoSize = true;
            this.lblCmdTableType.Location = new System.Drawing.Point(10, 55);
            this.lblCmdTableType.Name = "lblCmdTableType";
            this.lblCmdTableType.Size = new System.Drawing.Size(56, 13);
            this.lblCmdTableType.TabIndex = 2;
            this.lblCmdTableType.Text = "Data table";
            // 
            // cbCmdTableType
            // 
            this.cbCmdTableType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCmdTableType.FormattingEnabled = true;
            this.cbCmdTableType.Items.AddRange(new object[] {
            "Coils (0X)",
            "Holding Registers (4X)"});
            this.cbCmdTableType.Location = new System.Drawing.Point(13, 71);
            this.cbCmdTableType.Name = "cbCmdTableType";
            this.cbCmdTableType.Size = new System.Drawing.Size(254, 21);
            this.cbCmdTableType.TabIndex = 3;
            this.cbCmdTableType.SelectedIndexChanged += new System.EventHandler(this.cbCmdTableType_SelectedIndexChanged);
            // 
            // CtrlCmd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbCmd);
            this.Name = "CtrlCmd";
            this.Size = new System.Drawing.Size(280, 324);
            this.gbCmd.ResumeLayout(false);
            this.gbCmd.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCmdElemCnt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCmdNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCmdAddress)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbCmd;
        private System.Windows.Forms.CheckBox chkCmdMultiple;
        private System.Windows.Forms.Label lblCmdElemCnt;
        private System.Windows.Forms.NumericUpDown numCmdElemCnt;
        private System.Windows.Forms.TextBox txtCmdName;
        private System.Windows.Forms.Label lblCmdName;
        private System.Windows.Forms.Label lblCmdNum;
        private System.Windows.Forms.NumericUpDown numCmdNum;
        private System.Windows.Forms.NumericUpDown numCmdAddress;
        private System.Windows.Forms.Label lblCmdAddress;
        private System.Windows.Forms.Label lblCmdTableType;
        private System.Windows.Forms.ComboBox cbCmdTableType;
        private System.Windows.Forms.TextBox txtCmdFuncCode;
        private System.Windows.Forms.Label lblCmdFuncCode;
        private System.Windows.Forms.Label lblCmdByteOrderExample;
        private System.Windows.Forms.TextBox txtCmdByteOrder;
        private System.Windows.Forms.Label lblCmdByteOrder;
        private System.Windows.Forms.Label lblCmdAddressHint;
        private System.Windows.Forms.Label lblCmdElemType;
        private System.Windows.Forms.ComboBox cbCmdElemType;
    }
}
