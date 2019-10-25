namespace Scada.Comm.Devices.OpcUa.UI
{
    partial class CtrlItem
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
            this.gbItem = new System.Windows.Forms.GroupBox();
            this.txtSignal = new System.Windows.Forms.TextBox();
            this.lblSignal = new System.Windows.Forms.Label();
            this.numCnlNum = new System.Windows.Forms.NumericUpDown();
            this.lblCnlNum = new System.Windows.Forms.Label();
            this.numArrayLen = new System.Windows.Forms.NumericUpDown();
            this.lblArrayLen = new System.Windows.Forms.Label();
            this.chkIsArray = new System.Windows.Forms.CheckBox();
            this.txtNodeID = new System.Windows.Forms.TextBox();
            this.lblNodeID = new System.Windows.Forms.Label();
            this.txtDisplayName = new System.Windows.Forms.TextBox();
            this.lblDisplayName = new System.Windows.Forms.Label();
            this.chkItemActive = new System.Windows.Forms.CheckBox();
            this.gbItem.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCnlNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numArrayLen)).BeginInit();
            this.SuspendLayout();
            // 
            // gbItem
            // 
            this.gbItem.Controls.Add(this.txtSignal);
            this.gbItem.Controls.Add(this.lblSignal);
            this.gbItem.Controls.Add(this.numCnlNum);
            this.gbItem.Controls.Add(this.lblCnlNum);
            this.gbItem.Controls.Add(this.numArrayLen);
            this.gbItem.Controls.Add(this.lblArrayLen);
            this.gbItem.Controls.Add(this.chkIsArray);
            this.gbItem.Controls.Add(this.txtNodeID);
            this.gbItem.Controls.Add(this.lblNodeID);
            this.gbItem.Controls.Add(this.txtDisplayName);
            this.gbItem.Controls.Add(this.lblDisplayName);
            this.gbItem.Controls.Add(this.chkItemActive);
            this.gbItem.Location = new System.Drawing.Point(0, 0);
            this.gbItem.Name = "gbItem";
            this.gbItem.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbItem.Size = new System.Drawing.Size(230, 407);
            this.gbItem.TabIndex = 0;
            this.gbItem.TabStop = false;
            this.gbItem.Text = "Item Parameters";
            // 
            // txtSignal
            // 
            this.txtSignal.Location = new System.Drawing.Point(13, 234);
            this.txtSignal.Name = "txtSignal";
            this.txtSignal.ReadOnly = true;
            this.txtSignal.Size = new System.Drawing.Size(100, 20);
            this.txtSignal.TabIndex = 11;
            // 
            // lblSignal
            // 
            this.lblSignal.AutoSize = true;
            this.lblSignal.Location = new System.Drawing.Point(10, 218);
            this.lblSignal.Name = "lblSignal";
            this.lblSignal.Size = new System.Drawing.Size(36, 13);
            this.lblSignal.TabIndex = 10;
            this.lblSignal.Text = "Signal";
            // 
            // numCnlNum
            // 
            this.numCnlNum.Location = new System.Drawing.Point(13, 195);
            this.numCnlNum.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numCnlNum.Name = "numCnlNum";
            this.numCnlNum.Size = new System.Drawing.Size(100, 20);
            this.numCnlNum.TabIndex = 9;
            this.numCnlNum.ValueChanged += new System.EventHandler(this.numCnlNum_ValueChanged);
            // 
            // lblCnlNum
            // 
            this.lblCnlNum.AutoSize = true;
            this.lblCnlNum.Location = new System.Drawing.Point(10, 179);
            this.lblCnlNum.Name = "lblCnlNum";
            this.lblCnlNum.Size = new System.Drawing.Size(72, 13);
            this.lblCnlNum.TabIndex = 8;
            this.lblCnlNum.Text = "Input channel";
            // 
            // numArrayLen
            // 
            this.numArrayLen.Location = new System.Drawing.Point(13, 156);
            this.numArrayLen.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numArrayLen.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numArrayLen.Name = "numArrayLen";
            this.numArrayLen.Size = new System.Drawing.Size(100, 20);
            this.numArrayLen.TabIndex = 7;
            this.numArrayLen.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numArrayLen.ValueChanged += new System.EventHandler(this.numArrayLen_ValueChanged);
            // 
            // lblArrayLen
            // 
            this.lblArrayLen.AutoSize = true;
            this.lblArrayLen.Location = new System.Drawing.Point(10, 140);
            this.lblArrayLen.Name = "lblArrayLen";
            this.lblArrayLen.Size = new System.Drawing.Size(63, 13);
            this.lblArrayLen.TabIndex = 6;
            this.lblArrayLen.Text = "Array length";
            // 
            // chkIsArray
            // 
            this.chkIsArray.AutoSize = true;
            this.chkIsArray.Location = new System.Drawing.Point(13, 120);
            this.chkIsArray.Name = "chkIsArray";
            this.chkIsArray.Size = new System.Drawing.Size(60, 17);
            this.chkIsArray.TabIndex = 5;
            this.chkIsArray.Text = "Is array";
            this.chkIsArray.UseVisualStyleBackColor = true;
            this.chkIsArray.CheckedChanged += new System.EventHandler(this.chkIsArray_CheckedChanged);
            // 
            // txtNodeID
            // 
            this.txtNodeID.Location = new System.Drawing.Point(13, 94);
            this.txtNodeID.Name = "txtNodeID";
            this.txtNodeID.ReadOnly = true;
            this.txtNodeID.Size = new System.Drawing.Size(204, 20);
            this.txtNodeID.TabIndex = 4;
            // 
            // lblNodeID
            // 
            this.lblNodeID.AutoSize = true;
            this.lblNodeID.Location = new System.Drawing.Point(10, 78);
            this.lblNodeID.Name = "lblNodeID";
            this.lblNodeID.Size = new System.Drawing.Size(47, 13);
            this.lblNodeID.TabIndex = 3;
            this.lblNodeID.Text = "Node ID";
            // 
            // txtDisplayName
            // 
            this.txtDisplayName.Location = new System.Drawing.Point(13, 55);
            this.txtDisplayName.Name = "txtDisplayName";
            this.txtDisplayName.Size = new System.Drawing.Size(204, 20);
            this.txtDisplayName.TabIndex = 2;
            this.txtDisplayName.TextChanged += new System.EventHandler(this.txtDisplayName_TextChanged);
            // 
            // lblDisplayName
            // 
            this.lblDisplayName.AutoSize = true;
            this.lblDisplayName.Location = new System.Drawing.Point(10, 39);
            this.lblDisplayName.Name = "lblDisplayName";
            this.lblDisplayName.Size = new System.Drawing.Size(70, 13);
            this.lblDisplayName.TabIndex = 1;
            this.lblDisplayName.Text = "Display name";
            // 
            // chkItemActive
            // 
            this.chkItemActive.AutoSize = true;
            this.chkItemActive.Location = new System.Drawing.Point(13, 19);
            this.chkItemActive.Name = "chkItemActive";
            this.chkItemActive.Size = new System.Drawing.Size(56, 17);
            this.chkItemActive.TabIndex = 0;
            this.chkItemActive.Text = "Active";
            this.chkItemActive.UseVisualStyleBackColor = true;
            this.chkItemActive.CheckedChanged += new System.EventHandler(this.chkItemActive_CheckedChanged);
            // 
            // CtrlItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbItem);
            this.Name = "CtrlItem";
            this.Size = new System.Drawing.Size(230, 407);
            this.gbItem.ResumeLayout(false);
            this.gbItem.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCnlNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numArrayLen)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbItem;
        private System.Windows.Forms.TextBox txtDisplayName;
        private System.Windows.Forms.Label lblDisplayName;
        private System.Windows.Forms.CheckBox chkItemActive;
        private System.Windows.Forms.TextBox txtNodeID;
        private System.Windows.Forms.Label lblNodeID;
        private System.Windows.Forms.CheckBox chkIsArray;
        private System.Windows.Forms.Label lblArrayLen;
        private System.Windows.Forms.NumericUpDown numArrayLen;
        private System.Windows.Forms.Label lblSignal;
        private System.Windows.Forms.NumericUpDown numCnlNum;
        private System.Windows.Forms.Label lblCnlNum;
        private System.Windows.Forms.TextBox txtSignal;
    }
}
