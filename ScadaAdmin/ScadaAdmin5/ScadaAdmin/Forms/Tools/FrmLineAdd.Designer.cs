namespace Scada.Admin.App.Forms.Tools
{
    partial class FrmLineAdd
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
            this.gbLine = new System.Windows.Forms.GroupBox();
            this.lblLineNum = new System.Windows.Forms.Label();
            this.numLineNum = new System.Windows.Forms.NumericUpDown();
            this.lblName = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblDescr = new System.Windows.Forms.Label();
            this.txtDescr = new System.Windows.Forms.TextBox();
            this.gbComm = new System.Windows.Forms.GroupBox();
            this.chkAddToComm = new System.Windows.Forms.CheckBox();
            this.lblInstance = new System.Windows.Forms.Label();
            this.cbInstance = new System.Windows.Forms.ComboBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.gbLine.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numLineNum)).BeginInit();
            this.gbComm.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbLine
            // 
            this.gbLine.Controls.Add(this.txtDescr);
            this.gbLine.Controls.Add(this.lblDescr);
            this.gbLine.Controls.Add(this.txtName);
            this.gbLine.Controls.Add(this.lblName);
            this.gbLine.Controls.Add(this.numLineNum);
            this.gbLine.Controls.Add(this.lblLineNum);
            this.gbLine.Location = new System.Drawing.Point(12, 12);
            this.gbLine.Name = "gbLine";
            this.gbLine.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbLine.Size = new System.Drawing.Size(360, 104);
            this.gbLine.TabIndex = 0;
            this.gbLine.TabStop = false;
            this.gbLine.Text = "Communication Line";
            // 
            // lblLineNum
            // 
            this.lblLineNum.AutoSize = true;
            this.lblLineNum.Location = new System.Drawing.Point(10, 16);
            this.lblLineNum.Name = "lblLineNum";
            this.lblLineNum.Size = new System.Drawing.Size(44, 13);
            this.lblLineNum.TabIndex = 0;
            this.lblLineNum.Text = "Number";
            // 
            // numLineNum
            // 
            this.numLineNum.Location = new System.Drawing.Point(13, 32);
            this.numLineNum.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numLineNum.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numLineNum.Name = "numLineNum";
            this.numLineNum.Size = new System.Drawing.Size(75, 20);
            this.numLineNum.TabIndex = 1;
            this.numLineNum.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(91, 16);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(35, 13);
            this.lblName.TabIndex = 2;
            this.lblName.Text = "Name";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(94, 32);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(253, 20);
            this.txtName.TabIndex = 3;
            // 
            // lblDescr
            // 
            this.lblDescr.AutoSize = true;
            this.lblDescr.Location = new System.Drawing.Point(10, 55);
            this.lblDescr.Name = "lblDescr";
            this.lblDescr.Size = new System.Drawing.Size(60, 13);
            this.lblDescr.TabIndex = 4;
            this.lblDescr.Text = "Description";
            // 
            // txtDescr
            // 
            this.txtDescr.Location = new System.Drawing.Point(13, 71);
            this.txtDescr.Name = "txtDescr";
            this.txtDescr.Size = new System.Drawing.Size(334, 20);
            this.txtDescr.TabIndex = 5;
            // 
            // gbComm
            // 
            this.gbComm.Controls.Add(this.cbInstance);
            this.gbComm.Controls.Add(this.lblInstance);
            this.gbComm.Controls.Add(this.chkAddToComm);
            this.gbComm.Location = new System.Drawing.Point(12, 122);
            this.gbComm.Name = "gbComm";
            this.gbComm.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbComm.Size = new System.Drawing.Size(360, 89);
            this.gbComm.TabIndex = 1;
            this.gbComm.TabStop = false;
            this.gbComm.Text = "Communicator";
            // 
            // chkAddToComm
            // 
            this.chkAddToComm.AutoSize = true;
            this.chkAddToComm.Location = new System.Drawing.Point(13, 19);
            this.chkAddToComm.Name = "chkAddToComm";
            this.chkAddToComm.Size = new System.Drawing.Size(145, 17);
            this.chkAddToComm.TabIndex = 0;
            this.chkAddToComm.Text = "Add line in Communicator";
            this.chkAddToComm.UseVisualStyleBackColor = true;
            // 
            // lblInstance
            // 
            this.lblInstance.AutoSize = true;
            this.lblInstance.Location = new System.Drawing.Point(10, 39);
            this.lblInstance.Name = "lblInstance";
            this.lblInstance.Size = new System.Drawing.Size(48, 13);
            this.lblInstance.TabIndex = 1;
            this.lblInstance.Text = "Instance";
            // 
            // cbInstance
            // 
            this.cbInstance.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbInstance.FormattingEnabled = true;
            this.cbInstance.Location = new System.Drawing.Point(13, 55);
            this.cbInstance.Name = "cbInstance";
            this.cbInstance.Size = new System.Drawing.Size(334, 21);
            this.cbInstance.TabIndex = 2;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(216, 217);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(297, 217);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // FrmLineAdd
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(384, 252);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.gbComm);
            this.Controls.Add(this.gbLine);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmLineAdd";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add Communication Line";
            this.Load += new System.EventHandler(this.FrmLineAdd_Load);
            this.gbLine.ResumeLayout(false);
            this.gbLine.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numLineNum)).EndInit();
            this.gbComm.ResumeLayout(false);
            this.gbComm.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbLine;
        private System.Windows.Forms.Label lblLineNum;
        private System.Windows.Forms.NumericUpDown numLineNum;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox txtDescr;
        private System.Windows.Forms.Label lblDescr;
        private System.Windows.Forms.GroupBox gbComm;
        private System.Windows.Forms.CheckBox chkAddToComm;
        private System.Windows.Forms.ComboBox cbInstance;
        private System.Windows.Forms.Label lblInstance;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
    }
}