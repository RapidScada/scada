namespace Scada.Comm.Devices.Modbus.UI
{
    partial class CtrlElemGroup
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
            this.gbElemGroup = new System.Windows.Forms.GroupBox();
            this.chkGrActive = new System.Windows.Forms.CheckBox();
            this.lblGrElemCnt = new System.Windows.Forms.Label();
            this.numGrElemCnt = new System.Windows.Forms.NumericUpDown();
            this.txtGrName = new System.Windows.Forms.TextBox();
            this.lblGrName = new System.Windows.Forms.Label();
            this.numGrAddress = new System.Windows.Forms.NumericUpDown();
            this.lblGrAddress = new System.Windows.Forms.Label();
            this.lblGrTableType = new System.Windows.Forms.Label();
            this.cbGrTableType = new System.Windows.Forms.ComboBox();
            this.lblFuncCode = new System.Windows.Forms.Label();
            this.txtFuncCode = new System.Windows.Forms.TextBox();
            this.lblGrAddressHint = new System.Windows.Forms.Label();
            this.gbElemGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numGrElemCnt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numGrAddress)).BeginInit();
            this.SuspendLayout();
            // 
            // gbElemGroup
            // 
            this.gbElemGroup.Controls.Add(this.lblGrAddressHint);
            this.gbElemGroup.Controls.Add(this.txtFuncCode);
            this.gbElemGroup.Controls.Add(this.lblFuncCode);
            this.gbElemGroup.Controls.Add(this.chkGrActive);
            this.gbElemGroup.Controls.Add(this.lblGrElemCnt);
            this.gbElemGroup.Controls.Add(this.numGrElemCnt);
            this.gbElemGroup.Controls.Add(this.txtGrName);
            this.gbElemGroup.Controls.Add(this.lblGrName);
            this.gbElemGroup.Controls.Add(this.numGrAddress);
            this.gbElemGroup.Controls.Add(this.lblGrAddress);
            this.gbElemGroup.Controls.Add(this.lblGrTableType);
            this.gbElemGroup.Controls.Add(this.cbGrTableType);
            this.gbElemGroup.Location = new System.Drawing.Point(0, 0);
            this.gbElemGroup.Name = "gbElemGroup";
            this.gbElemGroup.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbElemGroup.Size = new System.Drawing.Size(280, 245);
            this.gbElemGroup.TabIndex = 0;
            this.gbElemGroup.TabStop = false;
            this.gbElemGroup.Text = "Element group parameters";
            // 
            // chkGrActive
            // 
            this.chkGrActive.AutoSize = true;
            this.chkGrActive.Location = new System.Drawing.Point(13, 19);
            this.chkGrActive.Name = "chkGrActive";
            this.chkGrActive.Size = new System.Drawing.Size(56, 17);
            this.chkGrActive.TabIndex = 0;
            this.chkGrActive.Text = "Active";
            this.chkGrActive.UseVisualStyleBackColor = true;
            this.chkGrActive.CheckedChanged += new System.EventHandler(this.chkGrActive_CheckedChanged);
            // 
            // lblGrElemCnt
            // 
            this.lblGrElemCnt.AutoSize = true;
            this.lblGrElemCnt.Location = new System.Drawing.Point(10, 196);
            this.lblGrElemCnt.Name = "lblGrElemCnt";
            this.lblGrElemCnt.Size = new System.Drawing.Size(75, 13);
            this.lblGrElemCnt.TabIndex = 10;
            this.lblGrElemCnt.Text = "Element count";
            // 
            // numGrElemCnt
            // 
            this.numGrElemCnt.Location = new System.Drawing.Point(13, 212);
            this.numGrElemCnt.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.numGrElemCnt.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numGrElemCnt.Name = "numGrElemCnt";
            this.numGrElemCnt.Size = new System.Drawing.Size(124, 20);
            this.numGrElemCnt.TabIndex = 11;
            this.numGrElemCnt.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numGrElemCnt.ValueChanged += new System.EventHandler(this.numGrElemCnt_ValueChanged);
            // 
            // txtGrName
            // 
            this.txtGrName.Location = new System.Drawing.Point(13, 55);
            this.txtGrName.Name = "txtGrName";
            this.txtGrName.Size = new System.Drawing.Size(254, 20);
            this.txtGrName.TabIndex = 2;
            this.txtGrName.TextChanged += new System.EventHandler(this.txtGrName_TextChanged);
            // 
            // lblGrName
            // 
            this.lblGrName.AutoSize = true;
            this.lblGrName.Location = new System.Drawing.Point(10, 39);
            this.lblGrName.Name = "lblGrName";
            this.lblGrName.Size = new System.Drawing.Size(35, 13);
            this.lblGrName.TabIndex = 1;
            this.lblGrName.Text = "Name";
            // 
            // numGrAddress
            // 
            this.numGrAddress.Location = new System.Drawing.Point(13, 173);
            this.numGrAddress.Maximum = new decimal(new int[] {
            65536,
            0,
            0,
            0});
            this.numGrAddress.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numGrAddress.Name = "numGrAddress";
            this.numGrAddress.Size = new System.Drawing.Size(124, 20);
            this.numGrAddress.TabIndex = 8;
            this.numGrAddress.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numGrAddress.ValueChanged += new System.EventHandler(this.numGrAddress_ValueChanged);
            // 
            // lblGrAddress
            // 
            this.lblGrAddress.AutoSize = true;
            this.lblGrAddress.Location = new System.Drawing.Point(10, 157);
            this.lblGrAddress.Name = "lblGrAddress";
            this.lblGrAddress.Size = new System.Drawing.Size(109, 13);
            this.lblGrAddress.TabIndex = 7;
            this.lblGrAddress.Text = "Start element address";
            // 
            // lblGrTableType
            // 
            this.lblGrTableType.AutoSize = true;
            this.lblGrTableType.Location = new System.Drawing.Point(10, 78);
            this.lblGrTableType.Name = "lblGrTableType";
            this.lblGrTableType.Size = new System.Drawing.Size(56, 13);
            this.lblGrTableType.TabIndex = 3;
            this.lblGrTableType.Text = "Data table";
            // 
            // cbGrTableType
            // 
            this.cbGrTableType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbGrTableType.FormattingEnabled = true;
            this.cbGrTableType.Items.AddRange(new object[] {
            "Discretes Inputs (1X)",
            "Coils (0X)",
            "Input Registers (3X)",
            "Holding Registers (4X)"});
            this.cbGrTableType.Location = new System.Drawing.Point(13, 94);
            this.cbGrTableType.Name = "cbGrTableType";
            this.cbGrTableType.Size = new System.Drawing.Size(254, 21);
            this.cbGrTableType.TabIndex = 4;
            this.cbGrTableType.SelectedIndexChanged += new System.EventHandler(this.cbGrTableType_SelectedIndexChanged);
            // 
            // lblFuncCode
            // 
            this.lblFuncCode.AutoSize = true;
            this.lblFuncCode.Location = new System.Drawing.Point(10, 118);
            this.lblFuncCode.Name = "lblFuncCode";
            this.lblFuncCode.Size = new System.Drawing.Size(75, 13);
            this.lblFuncCode.TabIndex = 5;
            this.lblFuncCode.Text = "Function code";
            // 
            // txtFuncCode
            // 
            this.txtFuncCode.Location = new System.Drawing.Point(13, 134);
            this.txtFuncCode.Name = "txtFuncCode";
            this.txtFuncCode.ReadOnly = true;
            this.txtFuncCode.Size = new System.Drawing.Size(254, 20);
            this.txtFuncCode.TabIndex = 6;
            // 
            // lblGrAddressHint
            // 
            this.lblGrAddressHint.AutoSize = true;
            this.lblGrAddressHint.Location = new System.Drawing.Point(144, 177);
            this.lblGrAddressHint.Name = "lblGrAddressHint";
            this.lblGrAddressHint.Size = new System.Drawing.Size(29, 13);
            this.lblGrAddressHint.TabIndex = 9;
            this.lblGrAddressHint.Text = "DEC";
            // 
            // CtrlElemGroup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbElemGroup);
            this.Name = "CtrlElemGroup";
            this.Size = new System.Drawing.Size(280, 245);
            this.gbElemGroup.ResumeLayout(false);
            this.gbElemGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numGrElemCnt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numGrAddress)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbElemGroup;
        private System.Windows.Forms.CheckBox chkGrActive;
        private System.Windows.Forms.Label lblGrElemCnt;
        private System.Windows.Forms.NumericUpDown numGrElemCnt;
        private System.Windows.Forms.TextBox txtGrName;
        private System.Windows.Forms.Label lblGrName;
        private System.Windows.Forms.NumericUpDown numGrAddress;
        private System.Windows.Forms.Label lblGrAddress;
        private System.Windows.Forms.Label lblGrTableType;
        private System.Windows.Forms.ComboBox cbGrTableType;
        private System.Windows.Forms.Label lblFuncCode;
        private System.Windows.Forms.TextBox txtFuncCode;
        private System.Windows.Forms.Label lblGrAddressHint;
    }
}
