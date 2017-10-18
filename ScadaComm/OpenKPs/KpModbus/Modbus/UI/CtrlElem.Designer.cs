namespace Scada.Comm.Devices.Modbus.UI
{
    partial class CtrlElem
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
            this.gbElem = new System.Windows.Forms.GroupBox();
            this.lblElemByteOrderExample = new System.Windows.Forms.Label();
            this.txtElemByteOrder = new System.Windows.Forms.TextBox();
            this.lblElemByteOrder = new System.Windows.Forms.Label();
            this.rbDouble = new System.Windows.Forms.RadioButton();
            this.rbLong = new System.Windows.Forms.RadioButton();
            this.rbULong = new System.Windows.Forms.RadioButton();
            this.rbBool = new System.Windows.Forms.RadioButton();
            this.rbFloat = new System.Windows.Forms.RadioButton();
            this.rbInt = new System.Windows.Forms.RadioButton();
            this.rbUInt = new System.Windows.Forms.RadioButton();
            this.rbShort = new System.Windows.Forms.RadioButton();
            this.lblElemType = new System.Windows.Forms.Label();
            this.rbUShort = new System.Windows.Forms.RadioButton();
            this.txtElemAddress = new System.Windows.Forms.TextBox();
            this.lblElemAddress = new System.Windows.Forms.Label();
            this.txtElemSignal = new System.Windows.Forms.TextBox();
            this.lblElemSignal = new System.Windows.Forms.Label();
            this.txtElemName = new System.Windows.Forms.TextBox();
            this.lblElemName = new System.Windows.Forms.Label();
            this.gbElem.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbElem
            // 
            this.gbElem.Controls.Add(this.lblElemByteOrderExample);
            this.gbElem.Controls.Add(this.txtElemByteOrder);
            this.gbElem.Controls.Add(this.lblElemByteOrder);
            this.gbElem.Controls.Add(this.rbDouble);
            this.gbElem.Controls.Add(this.rbLong);
            this.gbElem.Controls.Add(this.rbULong);
            this.gbElem.Controls.Add(this.rbBool);
            this.gbElem.Controls.Add(this.rbFloat);
            this.gbElem.Controls.Add(this.rbInt);
            this.gbElem.Controls.Add(this.rbUInt);
            this.gbElem.Controls.Add(this.rbShort);
            this.gbElem.Controls.Add(this.lblElemType);
            this.gbElem.Controls.Add(this.rbUShort);
            this.gbElem.Controls.Add(this.txtElemAddress);
            this.gbElem.Controls.Add(this.lblElemAddress);
            this.gbElem.Controls.Add(this.txtElemSignal);
            this.gbElem.Controls.Add(this.lblElemSignal);
            this.gbElem.Controls.Add(this.txtElemName);
            this.gbElem.Controls.Add(this.lblElemName);
            this.gbElem.Location = new System.Drawing.Point(0, 0);
            this.gbElem.Name = "gbElem";
            this.gbElem.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbElem.Size = new System.Drawing.Size(280, 271);
            this.gbElem.TabIndex = 0;
            this.gbElem.TabStop = false;
            this.gbElem.Text = "Element parameters";
            // 
            // lblElemByteOrderExample
            // 
            this.lblElemByteOrderExample.AutoSize = true;
            this.lblElemByteOrderExample.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblElemByteOrderExample.Location = new System.Drawing.Point(143, 242);
            this.lblElemByteOrderExample.Name = "lblElemByteOrderExample";
            this.lblElemByteOrderExample.Size = new System.Drawing.Size(118, 13);
            this.lblElemByteOrderExample.TabIndex = 18;
            this.lblElemByteOrderExample.Text = "For example, 01234567";
            // 
            // txtElemByteOrder
            // 
            this.txtElemByteOrder.Location = new System.Drawing.Point(13, 238);
            this.txtElemByteOrder.Name = "txtElemByteOrder";
            this.txtElemByteOrder.Size = new System.Drawing.Size(124, 20);
            this.txtElemByteOrder.TabIndex = 17;
            this.txtElemByteOrder.TextChanged += new System.EventHandler(this.txtByteOrder_TextChanged);
            // 
            // lblElemByteOrder
            // 
            this.lblElemByteOrder.AutoSize = true;
            this.lblElemByteOrder.Location = new System.Drawing.Point(10, 222);
            this.lblElemByteOrder.Name = "lblElemByteOrder";
            this.lblElemByteOrder.Size = new System.Drawing.Size(55, 13);
            this.lblElemByteOrder.TabIndex = 16;
            this.lblElemByteOrder.Text = "Byte order";
            // 
            // rbDouble
            // 
            this.rbDouble.AutoSize = true;
            this.rbDouble.Location = new System.Drawing.Point(143, 179);
            this.rbDouble.Name = "rbDouble";
            this.rbDouble.Size = new System.Drawing.Size(100, 17);
            this.rbDouble.TabIndex = 14;
            this.rbDouble.TabStop = true;
            this.rbDouble.Text = "double (8 bytes)";
            this.rbDouble.UseVisualStyleBackColor = true;
            this.rbDouble.CheckedChanged += new System.EventHandler(this.rbType_CheckedChanged);
            // 
            // rbLong
            // 
            this.rbLong.AutoSize = true;
            this.rbLong.Location = new System.Drawing.Point(143, 156);
            this.rbLong.Name = "rbLong";
            this.rbLong.Size = new System.Drawing.Size(88, 17);
            this.rbLong.TabIndex = 12;
            this.rbLong.TabStop = true;
            this.rbLong.Text = "long (8 bytes)";
            this.rbLong.UseVisualStyleBackColor = true;
            this.rbLong.CheckedChanged += new System.EventHandler(this.rbType_CheckedChanged);
            // 
            // rbULong
            // 
            this.rbULong.AutoSize = true;
            this.rbULong.Location = new System.Drawing.Point(13, 156);
            this.rbULong.Name = "rbULong";
            this.rbULong.Size = new System.Drawing.Size(94, 17);
            this.rbULong.TabIndex = 11;
            this.rbULong.TabStop = true;
            this.rbULong.Text = "ulong (8 bytes)";
            this.rbULong.UseVisualStyleBackColor = true;
            this.rbULong.CheckedChanged += new System.EventHandler(this.rbType_CheckedChanged);
            // 
            // rbBool
            // 
            this.rbBool.AutoSize = true;
            this.rbBool.Location = new System.Drawing.Point(13, 202);
            this.rbBool.Name = "rbBool";
            this.rbBool.Size = new System.Drawing.Size(74, 17);
            this.rbBool.TabIndex = 15;
            this.rbBool.TabStop = true;
            this.rbBool.Text = "bool (1 bit)";
            this.rbBool.UseVisualStyleBackColor = true;
            this.rbBool.CheckedChanged += new System.EventHandler(this.rbType_CheckedChanged);
            // 
            // rbFloat
            // 
            this.rbFloat.AutoSize = true;
            this.rbFloat.Location = new System.Drawing.Point(13, 179);
            this.rbFloat.Name = "rbFloat";
            this.rbFloat.Size = new System.Drawing.Size(88, 17);
            this.rbFloat.TabIndex = 13;
            this.rbFloat.TabStop = true;
            this.rbFloat.Text = "float (4 bytes)";
            this.rbFloat.UseVisualStyleBackColor = true;
            this.rbFloat.CheckedChanged += new System.EventHandler(this.rbType_CheckedChanged);
            // 
            // rbInt
            // 
            this.rbInt.AutoSize = true;
            this.rbInt.Location = new System.Drawing.Point(143, 133);
            this.rbInt.Name = "rbInt";
            this.rbInt.Size = new System.Drawing.Size(79, 17);
            this.rbInt.TabIndex = 10;
            this.rbInt.TabStop = true;
            this.rbInt.Text = "int (4 bytes)";
            this.rbInt.UseVisualStyleBackColor = true;
            this.rbInt.CheckedChanged += new System.EventHandler(this.rbType_CheckedChanged);
            // 
            // rbUInt
            // 
            this.rbUInt.AutoSize = true;
            this.rbUInt.Location = new System.Drawing.Point(13, 133);
            this.rbUInt.Name = "rbUInt";
            this.rbUInt.Size = new System.Drawing.Size(85, 17);
            this.rbUInt.TabIndex = 9;
            this.rbUInt.TabStop = true;
            this.rbUInt.Text = "uint (4 bytes)";
            this.rbUInt.UseVisualStyleBackColor = true;
            this.rbUInt.CheckedChanged += new System.EventHandler(this.rbType_CheckedChanged);
            // 
            // rbShort
            // 
            this.rbShort.AutoSize = true;
            this.rbShort.Location = new System.Drawing.Point(143, 110);
            this.rbShort.Name = "rbShort";
            this.rbShort.Size = new System.Drawing.Size(91, 17);
            this.rbShort.TabIndex = 8;
            this.rbShort.TabStop = true;
            this.rbShort.Text = "short (2 bytes)";
            this.rbShort.UseVisualStyleBackColor = true;
            this.rbShort.CheckedChanged += new System.EventHandler(this.rbType_CheckedChanged);
            // 
            // lblElemType
            // 
            this.lblElemType.AutoSize = true;
            this.lblElemType.Location = new System.Drawing.Point(10, 94);
            this.lblElemType.Name = "lblElemType";
            this.lblElemType.Size = new System.Drawing.Size(34, 13);
            this.lblElemType.TabIndex = 6;
            this.lblElemType.Text = "Type:";
            // 
            // rbUShort
            // 
            this.rbUShort.AutoSize = true;
            this.rbUShort.Location = new System.Drawing.Point(13, 110);
            this.rbUShort.Name = "rbUShort";
            this.rbUShort.Size = new System.Drawing.Size(97, 17);
            this.rbUShort.TabIndex = 7;
            this.rbUShort.TabStop = true;
            this.rbUShort.Text = "ushort (2 bytes)";
            this.rbUShort.UseVisualStyleBackColor = true;
            this.rbUShort.CheckedChanged += new System.EventHandler(this.rbType_CheckedChanged);
            // 
            // txtElemAddress
            // 
            this.txtElemAddress.Location = new System.Drawing.Point(13, 71);
            this.txtElemAddress.Name = "txtElemAddress";
            this.txtElemAddress.ReadOnly = true;
            this.txtElemAddress.Size = new System.Drawing.Size(124, 20);
            this.txtElemAddress.TabIndex = 3;
            // 
            // lblElemAddress
            // 
            this.lblElemAddress.AutoSize = true;
            this.lblElemAddress.Location = new System.Drawing.Point(10, 55);
            this.lblElemAddress.Name = "lblElemAddress";
            this.lblElemAddress.Size = new System.Drawing.Size(45, 13);
            this.lblElemAddress.TabIndex = 2;
            this.lblElemAddress.Text = "Address";
            // 
            // txtElemSignal
            // 
            this.txtElemSignal.Location = new System.Drawing.Point(143, 71);
            this.txtElemSignal.Name = "txtElemSignal";
            this.txtElemSignal.ReadOnly = true;
            this.txtElemSignal.Size = new System.Drawing.Size(124, 20);
            this.txtElemSignal.TabIndex = 5;
            // 
            // lblElemSignal
            // 
            this.lblElemSignal.AutoSize = true;
            this.lblElemSignal.Location = new System.Drawing.Point(140, 55);
            this.lblElemSignal.Name = "lblElemSignal";
            this.lblElemSignal.Size = new System.Drawing.Size(36, 13);
            this.lblElemSignal.TabIndex = 4;
            this.lblElemSignal.Text = "Signal";
            // 
            // txtElemName
            // 
            this.txtElemName.Location = new System.Drawing.Point(13, 32);
            this.txtElemName.Name = "txtElemName";
            this.txtElemName.Size = new System.Drawing.Size(254, 20);
            this.txtElemName.TabIndex = 1;
            this.txtElemName.TextChanged += new System.EventHandler(this.txtElemName_TextChanged);
            // 
            // lblElemName
            // 
            this.lblElemName.AutoSize = true;
            this.lblElemName.Location = new System.Drawing.Point(10, 16);
            this.lblElemName.Name = "lblElemName";
            this.lblElemName.Size = new System.Drawing.Size(35, 13);
            this.lblElemName.TabIndex = 0;
            this.lblElemName.Text = "Name";
            // 
            // CtrlElem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbElem);
            this.Name = "CtrlElem";
            this.Size = new System.Drawing.Size(280, 271);
            this.gbElem.ResumeLayout(false);
            this.gbElem.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbElem;
        private System.Windows.Forms.Label lblElemByteOrderExample;
        private System.Windows.Forms.TextBox txtElemByteOrder;
        private System.Windows.Forms.Label lblElemByteOrder;
        private System.Windows.Forms.RadioButton rbDouble;
        private System.Windows.Forms.RadioButton rbLong;
        private System.Windows.Forms.RadioButton rbULong;
        private System.Windows.Forms.RadioButton rbBool;
        private System.Windows.Forms.RadioButton rbFloat;
        private System.Windows.Forms.RadioButton rbInt;
        private System.Windows.Forms.RadioButton rbUInt;
        private System.Windows.Forms.RadioButton rbShort;
        private System.Windows.Forms.Label lblElemType;
        private System.Windows.Forms.RadioButton rbUShort;
        private System.Windows.Forms.TextBox txtElemAddress;
        private System.Windows.Forms.Label lblElemAddress;
        private System.Windows.Forms.TextBox txtElemSignal;
        private System.Windows.Forms.Label lblElemSignal;
        private System.Windows.Forms.TextBox txtElemName;
        private System.Windows.Forms.Label lblElemName;
    }
}
