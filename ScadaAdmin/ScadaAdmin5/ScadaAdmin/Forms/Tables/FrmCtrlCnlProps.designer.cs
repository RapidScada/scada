namespace Scada.Admin.App.Forms.Tables
{
    partial class FrmCtrlCnlProps
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
            this.lblCtrlCnlNum = new System.Windows.Forms.Label();
            this.chkActive = new System.Windows.Forms.CheckBox();
            this.cbCmdType = new System.Windows.Forms.ComboBox();
            this.lblCmdType = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.lblObj = new System.Windows.Forms.Label();
            this.txtObjNum = new System.Windows.Forms.TextBox();
            this.cbObj = new System.Windows.Forms.ComboBox();
            this.cbKP = new System.Windows.Forms.ComboBox();
            this.txtKPNum = new System.Windows.Forms.TextBox();
            this.lblKP = new System.Windows.Forms.Label();
            this.lblFormula = new System.Windows.Forms.Label();
            this.chkFormulaUsed = new System.Windows.Forms.CheckBox();
            this.txtFormula = new System.Windows.Forms.TextBox();
            this.lblCmd = new System.Windows.Forms.Label();
            this.cbCmdVal = new System.Windows.Forms.ComboBox();
            this.chkEvEnabled = new System.Windows.Forms.CheckBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtCtrlCnlNum = new System.Windows.Forms.TextBox();
            this.txtCmdNum = new System.Windows.Forms.TextBox();
            this.lblCmdNum = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblCtrlCnlNum
            // 
            this.lblCtrlCnlNum.AutoSize = true;
            this.lblCtrlCnlNum.Location = new System.Drawing.Point(9, 32);
            this.lblCtrlCnlNum.Name = "lblCtrlCnlNum";
            this.lblCtrlCnlNum.Size = new System.Drawing.Size(44, 13);
            this.lblCtrlCnlNum.TabIndex = 1;
            this.lblCtrlCnlNum.Text = "Number";
            // 
            // chkActive
            // 
            this.chkActive.AutoSize = true;
            this.chkActive.Location = new System.Drawing.Point(12, 12);
            this.chkActive.Name = "chkActive";
            this.chkActive.Size = new System.Drawing.Size(56, 17);
            this.chkActive.TabIndex = 0;
            this.chkActive.Text = "Active";
            this.chkActive.UseVisualStyleBackColor = true;
            // 
            // cbCmdType
            // 
            this.cbCmdType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCmdType.FormattingEnabled = true;
            this.cbCmdType.Location = new System.Drawing.Point(68, 87);
            this.cbCmdType.Name = "cbCmdType";
            this.cbCmdType.Size = new System.Drawing.Size(334, 21);
            this.cbCmdType.TabIndex = 6;
            // 
            // lblCmdType
            // 
            this.lblCmdType.AutoSize = true;
            this.lblCmdType.Location = new System.Drawing.Point(65, 71);
            this.lblCmdType.Name = "lblCmdType";
            this.lblCmdType.Size = new System.Drawing.Size(77, 13);
            this.lblCmdType.TabIndex = 5;
            this.lblCmdType.Text = "Command type";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(68, 48);
            this.txtName.MaxLength = 100;
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(334, 20);
            this.txtName.TabIndex = 4;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(65, 32);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(35, 13);
            this.lblName.TabIndex = 3;
            this.lblName.Text = "Name";
            // 
            // lblObj
            // 
            this.lblObj.AutoSize = true;
            this.lblObj.Location = new System.Drawing.Point(65, 111);
            this.lblObj.Name = "lblObj";
            this.lblObj.Size = new System.Drawing.Size(38, 13);
            this.lblObj.TabIndex = 7;
            this.lblObj.Text = "Object";
            // 
            // txtObjNum
            // 
            this.txtObjNum.Location = new System.Drawing.Point(12, 127);
            this.txtObjNum.Name = "txtObjNum";
            this.txtObjNum.ReadOnly = true;
            this.txtObjNum.Size = new System.Drawing.Size(50, 20);
            this.txtObjNum.TabIndex = 8;
            this.txtObjNum.TabStop = false;
            // 
            // cbObj
            // 
            this.cbObj.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbObj.FormattingEnabled = true;
            this.cbObj.Location = new System.Drawing.Point(68, 127);
            this.cbObj.Name = "cbObj";
            this.cbObj.Size = new System.Drawing.Size(334, 21);
            this.cbObj.TabIndex = 9;
            this.cbObj.SelectedIndexChanged += new System.EventHandler(this.cbObj_SelectedIndexChanged);
            // 
            // cbKP
            // 
            this.cbKP.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbKP.FormattingEnabled = true;
            this.cbKP.Location = new System.Drawing.Point(68, 167);
            this.cbKP.Name = "cbKP";
            this.cbKP.Size = new System.Drawing.Size(334, 21);
            this.cbKP.TabIndex = 12;
            this.cbKP.SelectedIndexChanged += new System.EventHandler(this.cbKP_SelectedIndexChanged);
            // 
            // txtKPNum
            // 
            this.txtKPNum.Location = new System.Drawing.Point(12, 167);
            this.txtKPNum.Name = "txtKPNum";
            this.txtKPNum.ReadOnly = true;
            this.txtKPNum.Size = new System.Drawing.Size(50, 20);
            this.txtKPNum.TabIndex = 11;
            this.txtKPNum.TabStop = false;
            // 
            // lblKP
            // 
            this.lblKP.AutoSize = true;
            this.lblKP.Location = new System.Drawing.Point(65, 151);
            this.lblKP.Name = "lblKP";
            this.lblKP.Size = new System.Drawing.Size(41, 13);
            this.lblKP.TabIndex = 10;
            this.lblKP.Text = "Device";
            // 
            // lblFormula
            // 
            this.lblFormula.AutoSize = true;
            this.lblFormula.Location = new System.Drawing.Point(65, 231);
            this.lblFormula.Name = "lblFormula";
            this.lblFormula.Size = new System.Drawing.Size(44, 13);
            this.lblFormula.TabIndex = 17;
            this.lblFormula.Text = "Formula";
            // 
            // chkFormulaUsed
            // 
            this.chkFormulaUsed.AutoSize = true;
            this.chkFormulaUsed.Location = new System.Drawing.Point(47, 250);
            this.chkFormulaUsed.Name = "chkFormulaUsed";
            this.chkFormulaUsed.Size = new System.Drawing.Size(15, 14);
            this.chkFormulaUsed.TabIndex = 18;
            this.chkFormulaUsed.UseVisualStyleBackColor = true;
            // 
            // txtFormula
            // 
            this.txtFormula.Location = new System.Drawing.Point(68, 247);
            this.txtFormula.MaxLength = 100;
            this.txtFormula.Name = "txtFormula";
            this.txtFormula.Size = new System.Drawing.Size(334, 20);
            this.txtFormula.TabIndex = 19;
            // 
            // lblCmd
            // 
            this.lblCmd.AutoSize = true;
            this.lblCmd.Location = new System.Drawing.Point(65, 191);
            this.lblCmd.Name = "lblCmd";
            this.lblCmd.Size = new System.Drawing.Size(54, 13);
            this.lblCmd.TabIndex = 15;
            this.lblCmd.Text = "Command";
            // 
            // cbCmdVal
            // 
            this.cbCmdVal.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCmdVal.FormattingEnabled = true;
            this.cbCmdVal.Location = new System.Drawing.Point(68, 207);
            this.cbCmdVal.Name = "cbCmdVal";
            this.cbCmdVal.Size = new System.Drawing.Size(334, 21);
            this.cbCmdVal.TabIndex = 16;
            // 
            // chkEvEnabled
            // 
            this.chkEvEnabled.AutoSize = true;
            this.chkEvEnabled.Location = new System.Drawing.Point(47, 278);
            this.chkEvEnabled.Name = "chkEvEnabled";
            this.chkEvEnabled.Size = new System.Drawing.Size(86, 17);
            this.chkEvEnabled.TabIndex = 20;
            this.chkEvEnabled.Text = "Write events";
            this.chkEvEnabled.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(246, 306);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 21;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(327, 306);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 22;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // txtCtrlCnlNum
            // 
            this.txtCtrlCnlNum.Location = new System.Drawing.Point(12, 48);
            this.txtCtrlCnlNum.Name = "txtCtrlCnlNum";
            this.txtCtrlCnlNum.Size = new System.Drawing.Size(50, 20);
            this.txtCtrlCnlNum.TabIndex = 2;
            // 
            // txtCmdNum
            // 
            this.txtCmdNum.Location = new System.Drawing.Point(12, 207);
            this.txtCmdNum.Name = "txtCmdNum";
            this.txtCmdNum.Size = new System.Drawing.Size(50, 20);
            this.txtCmdNum.TabIndex = 14;
            // 
            // lblCmdNum
            // 
            this.lblCmdNum.AutoSize = true;
            this.lblCmdNum.Location = new System.Drawing.Point(9, 191);
            this.lblCmdNum.Name = "lblCmdNum";
            this.lblCmdNum.Size = new System.Drawing.Size(92, 13);
            this.lblCmdNum.TabIndex = 13;
            this.lblCmdNum.Text = "Command number";
            this.lblCmdNum.Visible = false;
            // 
            // FrmCtrlCnlProps
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(414, 341);
            this.Controls.Add(this.lblCmd);
            this.Controls.Add(this.lblCmdNum);
            this.Controls.Add(this.chkEvEnabled);
            this.Controls.Add(this.txtCmdNum);
            this.Controls.Add(this.txtCtrlCnlNum);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.cbCmdVal);
            this.Controls.Add(this.txtFormula);
            this.Controls.Add(this.chkFormulaUsed);
            this.Controls.Add(this.lblFormula);
            this.Controls.Add(this.cbKP);
            this.Controls.Add(this.txtKPNum);
            this.Controls.Add(this.lblKP);
            this.Controls.Add(this.cbObj);
            this.Controls.Add(this.txtObjNum);
            this.Controls.Add(this.lblObj);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblCmdType);
            this.Controls.Add(this.cbCmdType);
            this.Controls.Add(this.chkActive);
            this.Controls.Add(this.lblCtrlCnlNum);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmCtrlCnlProps";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Output Channel Properties";
            this.Load += new System.EventHandler(this.FrmInCnlProps_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblCtrlCnlNum;
        private System.Windows.Forms.CheckBox chkActive;
        private System.Windows.Forms.ComboBox cbCmdType;
        private System.Windows.Forms.Label lblCmdType;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblObj;
        private System.Windows.Forms.TextBox txtObjNum;
        private System.Windows.Forms.ComboBox cbObj;
        private System.Windows.Forms.ComboBox cbKP;
        private System.Windows.Forms.TextBox txtKPNum;
        private System.Windows.Forms.Label lblKP;
        private System.Windows.Forms.Label lblFormula;
        private System.Windows.Forms.CheckBox chkFormulaUsed;
        private System.Windows.Forms.TextBox txtFormula;
        private System.Windows.Forms.Label lblCmd;
        private System.Windows.Forms.ComboBox cbCmdVal;
        private System.Windows.Forms.CheckBox chkEvEnabled;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtCtrlCnlNum;
        private System.Windows.Forms.TextBox txtCmdNum;
        private System.Windows.Forms.Label lblCmdNum;
    }
}