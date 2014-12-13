namespace ScadaAdmin
{
    partial class FrmInCnlProps
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
            this.lblCnlNum = new System.Windows.Forms.Label();
            this.chkActive = new System.Windows.Forms.CheckBox();
            this.cbCnlType = new System.Windows.Forms.ComboBox();
            this.lblCnlType = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.lblObj = new System.Windows.Forms.Label();
            this.txtObjNum = new System.Windows.Forms.TextBox();
            this.cbObj = new System.Windows.Forms.ComboBox();
            this.cbKP = new System.Windows.Forms.ComboBox();
            this.txtKPNum = new System.Windows.Forms.TextBox();
            this.lblKP = new System.Windows.Forms.Label();
            this.lblSignal = new System.Windows.Forms.Label();
            this.lblFormula = new System.Windows.Forms.Label();
            this.chkFormulaUsed = new System.Windows.Forms.CheckBox();
            this.txtFormula = new System.Windows.Forms.TextBox();
            this.lblParam = new System.Windows.Forms.Label();
            this.lblFormat = new System.Windows.Forms.Label();
            this.lblUnit = new System.Windows.Forms.Label();
            this.cbParam = new System.Windows.Forms.ComboBox();
            this.cbFormat = new System.Windows.Forms.ComboBox();
            this.cbUnit = new System.Windows.Forms.ComboBox();
            this.lblCtrlCnlNum = new System.Windows.Forms.Label();
            this.txtCtrlCnlName = new System.Windows.Forms.TextBox();
            this.chkEvEnabled = new System.Windows.Forms.CheckBox();
            this.chkEvSound = new System.Windows.Forms.CheckBox();
            this.chkEvOnChange = new System.Windows.Forms.CheckBox();
            this.chkEvOnUndef = new System.Windows.Forms.CheckBox();
            this.lblLimLowCrash = new System.Windows.Forms.Label();
            this.lblLimLow = new System.Windows.Forms.Label();
            this.lblLimHighCrash = new System.Windows.Forms.Label();
            this.lblLimHigh = new System.Windows.Forms.Label();
            this.txtLimLowCrash = new System.Windows.Forms.TextBox();
            this.txtLimLow = new System.Windows.Forms.TextBox();
            this.txtLimHighCrash = new System.Windows.Forms.TextBox();
            this.txtLimHigh = new System.Windows.Forms.TextBox();
            this.lblModifiedDT = new System.Windows.Forms.Label();
            this.txtModifiedDT = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.gbLimits = new System.Windows.Forms.GroupBox();
            this.gbEvents = new System.Windows.Forms.GroupBox();
            this.chkAveraging = new System.Windows.Forms.CheckBox();
            this.txtCnlNum = new System.Windows.Forms.TextBox();
            this.txtSignal = new System.Windows.Forms.TextBox();
            this.txtCtrlCnlNum = new System.Windows.Forms.TextBox();
            this.gbLimits.SuspendLayout();
            this.gbEvents.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblCnlNum
            // 
            this.lblCnlNum.AutoSize = true;
            this.lblCnlNum.Location = new System.Drawing.Point(9, 35);
            this.lblCnlNum.Name = "lblCnlNum";
            this.lblCnlNum.Size = new System.Drawing.Size(41, 13);
            this.lblCnlNum.TabIndex = 3;
            this.lblCnlNum.Text = "Номер";
            // 
            // chkActive
            // 
            this.chkActive.AutoSize = true;
            this.chkActive.Location = new System.Drawing.Point(12, 14);
            this.chkActive.Name = "chkActive";
            this.chkActive.Size = new System.Drawing.Size(76, 17);
            this.chkActive.TabIndex = 0;
            this.chkActive.Text = "Активный";
            this.chkActive.UseVisualStyleBackColor = true;
            // 
            // cbCnlType
            // 
            this.cbCnlType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCnlType.FormattingEnabled = true;
            this.cbCnlType.Location = new System.Drawing.Point(68, 90);
            this.cbCnlType.Name = "cbCnlType";
            this.cbCnlType.Size = new System.Drawing.Size(334, 21);
            this.cbCnlType.TabIndex = 8;
            // 
            // lblCnlType
            // 
            this.lblCnlType.AutoSize = true;
            this.lblCnlType.Location = new System.Drawing.Point(65, 74);
            this.lblCnlType.Name = "lblCnlType";
            this.lblCnlType.Size = new System.Drawing.Size(65, 13);
            this.lblCnlType.TabIndex = 7;
            this.lblCnlType.Text = "Тип канала";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(68, 51);
            this.txtName.MaxLength = 50;
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(334, 20);
            this.txtName.TabIndex = 6;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(65, 35);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(83, 13);
            this.lblName.TabIndex = 5;
            this.lblName.Text = "Наименование";
            // 
            // lblObj
            // 
            this.lblObj.AutoSize = true;
            this.lblObj.Location = new System.Drawing.Point(65, 114);
            this.lblObj.Name = "lblObj";
            this.lblObj.Size = new System.Drawing.Size(45, 13);
            this.lblObj.TabIndex = 9;
            this.lblObj.Text = "Объект";
            // 
            // txtObjNum
            // 
            this.txtObjNum.Location = new System.Drawing.Point(12, 130);
            this.txtObjNum.Name = "txtObjNum";
            this.txtObjNum.ReadOnly = true;
            this.txtObjNum.Size = new System.Drawing.Size(50, 20);
            this.txtObjNum.TabIndex = 10;
            this.txtObjNum.TabStop = false;
            // 
            // cbObj
            // 
            this.cbObj.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbObj.FormattingEnabled = true;
            this.cbObj.Location = new System.Drawing.Point(68, 130);
            this.cbObj.Name = "cbObj";
            this.cbObj.Size = new System.Drawing.Size(334, 21);
            this.cbObj.TabIndex = 11;
            // 
            // cbKP
            // 
            this.cbKP.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbKP.FormattingEnabled = true;
            this.cbKP.Location = new System.Drawing.Point(68, 170);
            this.cbKP.Name = "cbKP";
            this.cbKP.Size = new System.Drawing.Size(278, 21);
            this.cbKP.TabIndex = 14;
            // 
            // txtKPNum
            // 
            this.txtKPNum.Location = new System.Drawing.Point(12, 170);
            this.txtKPNum.Name = "txtKPNum";
            this.txtKPNum.ReadOnly = true;
            this.txtKPNum.Size = new System.Drawing.Size(50, 20);
            this.txtKPNum.TabIndex = 13;
            this.txtKPNum.TabStop = false;
            // 
            // lblKP
            // 
            this.lblKP.AutoSize = true;
            this.lblKP.Location = new System.Drawing.Point(65, 154);
            this.lblKP.Name = "lblKP";
            this.lblKP.Size = new System.Drawing.Size(22, 13);
            this.lblKP.TabIndex = 12;
            this.lblKP.Text = "КП";
            // 
            // lblSignal
            // 
            this.lblSignal.AutoSize = true;
            this.lblSignal.Location = new System.Drawing.Point(349, 154);
            this.lblSignal.Name = "lblSignal";
            this.lblSignal.Size = new System.Drawing.Size(43, 13);
            this.lblSignal.TabIndex = 15;
            this.lblSignal.Text = "Сигнал";
            // 
            // lblFormula
            // 
            this.lblFormula.AutoSize = true;
            this.lblFormula.Location = new System.Drawing.Point(65, 194);
            this.lblFormula.Name = "lblFormula";
            this.lblFormula.Size = new System.Drawing.Size(55, 13);
            this.lblFormula.TabIndex = 17;
            this.lblFormula.Text = "Формула";
            // 
            // chkFormulaUsed
            // 
            this.chkFormulaUsed.AutoSize = true;
            this.chkFormulaUsed.Location = new System.Drawing.Point(47, 213);
            this.chkFormulaUsed.Name = "chkFormulaUsed";
            this.chkFormulaUsed.Size = new System.Drawing.Size(15, 14);
            this.chkFormulaUsed.TabIndex = 18;
            this.chkFormulaUsed.UseVisualStyleBackColor = true;
            // 
            // txtFormula
            // 
            this.txtFormula.Location = new System.Drawing.Point(68, 210);
            this.txtFormula.MaxLength = 100;
            this.txtFormula.Name = "txtFormula";
            this.txtFormula.Size = new System.Drawing.Size(220, 20);
            this.txtFormula.TabIndex = 19;
            // 
            // lblParam
            // 
            this.lblParam.AutoSize = true;
            this.lblParam.Location = new System.Drawing.Point(9, 233);
            this.lblParam.Name = "lblParam";
            this.lblParam.Size = new System.Drawing.Size(58, 13);
            this.lblParam.TabIndex = 21;
            this.lblParam.Text = "Величина";
            // 
            // lblFormat
            // 
            this.lblFormat.AutoSize = true;
            this.lblFormat.Location = new System.Drawing.Point(175, 233);
            this.lblFormat.Name = "lblFormat";
            this.lblFormat.Size = new System.Drawing.Size(49, 13);
            this.lblFormat.TabIndex = 23;
            this.lblFormat.Text = "Формат";
            // 
            // lblUnit
            // 
            this.lblUnit.AutoSize = true;
            this.lblUnit.Location = new System.Drawing.Point(290, 233);
            this.lblUnit.Name = "lblUnit";
            this.lblUnit.Size = new System.Drawing.Size(75, 13);
            this.lblUnit.TabIndex = 25;
            this.lblUnit.Text = "Размерность";
            // 
            // cbParam
            // 
            this.cbParam.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbParam.FormattingEnabled = true;
            this.cbParam.Location = new System.Drawing.Point(12, 249);
            this.cbParam.Name = "cbParam";
            this.cbParam.Size = new System.Drawing.Size(160, 21);
            this.cbParam.TabIndex = 22;
            // 
            // cbFormat
            // 
            this.cbFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFormat.FormattingEnabled = true;
            this.cbFormat.Location = new System.Drawing.Point(178, 249);
            this.cbFormat.Name = "cbFormat";
            this.cbFormat.Size = new System.Drawing.Size(109, 21);
            this.cbFormat.TabIndex = 24;
            // 
            // cbUnit
            // 
            this.cbUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbUnit.FormattingEnabled = true;
            this.cbUnit.Location = new System.Drawing.Point(293, 249);
            this.cbUnit.Name = "cbUnit";
            this.cbUnit.Size = new System.Drawing.Size(109, 21);
            this.cbUnit.TabIndex = 26;
            // 
            // lblCtrlCnlNum
            // 
            this.lblCtrlCnlNum.AutoSize = true;
            this.lblCtrlCnlNum.Location = new System.Drawing.Point(9, 273);
            this.lblCtrlCnlNum.Name = "lblCtrlCnlNum";
            this.lblCtrlCnlNum.Size = new System.Drawing.Size(100, 13);
            this.lblCtrlCnlNum.TabIndex = 27;
            this.lblCtrlCnlNum.Text = "Канал управления";
            // 
            // txtCtrlCnlName
            // 
            this.txtCtrlCnlName.Location = new System.Drawing.Point(68, 289);
            this.txtCtrlCnlName.Name = "txtCtrlCnlName";
            this.txtCtrlCnlName.ReadOnly = true;
            this.txtCtrlCnlName.Size = new System.Drawing.Size(334, 20);
            this.txtCtrlCnlName.TabIndex = 29;
            this.txtCtrlCnlName.TabStop = false;
            // 
            // chkEvEnabled
            // 
            this.chkEvEnabled.AutoSize = true;
            this.chkEvEnabled.Location = new System.Drawing.Point(13, 19);
            this.chkEvEnabled.Name = "chkEvEnabled";
            this.chkEvEnabled.Size = new System.Drawing.Size(109, 17);
            this.chkEvEnabled.TabIndex = 0;
            this.chkEvEnabled.Text = "Запись событий";
            this.chkEvEnabled.UseVisualStyleBackColor = true;
            // 
            // chkEvSound
            // 
            this.chkEvSound.AutoSize = true;
            this.chkEvSound.Location = new System.Drawing.Point(187, 19);
            this.chkEvSound.Name = "chkEvSound";
            this.chkEvSound.Size = new System.Drawing.Size(96, 17);
            this.chkEvSound.TabIndex = 1;
            this.chkEvSound.Text = "Звук события";
            this.chkEvSound.UseVisualStyleBackColor = true;
            // 
            // chkEvOnChange
            // 
            this.chkEvOnChange.AutoSize = true;
            this.chkEvOnChange.Location = new System.Drawing.Point(13, 42);
            this.chkEvOnChange.Name = "chkEvOnChange";
            this.chkEvOnChange.Size = new System.Drawing.Size(101, 17);
            this.chkEvOnChange.TabIndex = 2;
            this.chkEvOnChange.Text = "По изменению";
            this.chkEvOnChange.UseVisualStyleBackColor = true;
            // 
            // chkEvOnUndef
            // 
            this.chkEvOnUndef.AutoSize = true;
            this.chkEvOnUndef.Location = new System.Drawing.Point(187, 42);
            this.chkEvOnUndef.Name = "chkEvOnUndef";
            this.chkEvOnUndef.Size = new System.Drawing.Size(192, 17);
            this.chkEvOnUndef.TabIndex = 3;
            this.chkEvOnUndef.Text = "По неопределённому состоянию";
            this.chkEvOnUndef.UseVisualStyleBackColor = true;
            // 
            // lblLimLowCrash
            // 
            this.lblLimLowCrash.AutoSize = true;
            this.lblLimLowCrash.Location = new System.Drawing.Point(10, 16);
            this.lblLimLowCrash.Name = "lblLimLowCrash";
            this.lblLimLowCrash.Size = new System.Drawing.Size(62, 13);
            this.lblLimLowCrash.TabIndex = 0;
            this.lblLimLowCrash.Text = "Ниж. авар.";
            // 
            // lblLimLow
            // 
            this.lblLimLow.AutoSize = true;
            this.lblLimLow.Location = new System.Drawing.Point(102, 16);
            this.lblLimLow.Name = "lblLimLow";
            this.lblLimLow.Size = new System.Drawing.Size(41, 13);
            this.lblLimLow.TabIndex = 2;
            this.lblLimLow.Text = "Нижяя";
            // 
            // lblLimHighCrash
            // 
            this.lblLimHighCrash.AutoSize = true;
            this.lblLimHighCrash.Location = new System.Drawing.Point(286, 16);
            this.lblLimHighCrash.Name = "lblLimHighCrash";
            this.lblLimHighCrash.Size = new System.Drawing.Size(64, 13);
            this.lblLimHighCrash.TabIndex = 6;
            this.lblLimHighCrash.Text = "Верх. авар.";
            // 
            // lblLimHigh
            // 
            this.lblLimHigh.AutoSize = true;
            this.lblLimHigh.Location = new System.Drawing.Point(194, 16);
            this.lblLimHigh.Name = "lblLimHigh";
            this.lblLimHigh.Size = new System.Drawing.Size(49, 13);
            this.lblLimHigh.TabIndex = 4;
            this.lblLimHigh.Text = "Верхняя";
            // 
            // txtLimLowCrash
            // 
            this.txtLimLowCrash.Location = new System.Drawing.Point(13, 32);
            this.txtLimLowCrash.Name = "txtLimLowCrash";
            this.txtLimLowCrash.Size = new System.Drawing.Size(86, 20);
            this.txtLimLowCrash.TabIndex = 1;
            // 
            // txtLimLow
            // 
            this.txtLimLow.Location = new System.Drawing.Point(105, 32);
            this.txtLimLow.Name = "txtLimLow";
            this.txtLimLow.Size = new System.Drawing.Size(86, 20);
            this.txtLimLow.TabIndex = 3;
            // 
            // txtLimHighCrash
            // 
            this.txtLimHighCrash.Location = new System.Drawing.Point(289, 32);
            this.txtLimHighCrash.Name = "txtLimHighCrash";
            this.txtLimHighCrash.Size = new System.Drawing.Size(88, 20);
            this.txtLimHighCrash.TabIndex = 7;
            // 
            // txtLimHigh
            // 
            this.txtLimHigh.Location = new System.Drawing.Point(197, 32);
            this.txtLimHigh.Name = "txtLimHigh";
            this.txtLimHigh.Size = new System.Drawing.Size(86, 20);
            this.txtLimHigh.TabIndex = 5;
            // 
            // lblModifiedDT
            // 
            this.lblModifiedDT.Location = new System.Drawing.Point(146, 15);
            this.lblModifiedDT.Name = "lblModifiedDT";
            this.lblModifiedDT.Size = new System.Drawing.Size(150, 13);
            this.lblModifiedDT.TabIndex = 1;
            this.lblModifiedDT.Text = "Время изменения";
            this.lblModifiedDT.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtModifiedDT
            // 
            this.txtModifiedDT.Location = new System.Drawing.Point(302, 12);
            this.txtModifiedDT.Name = "txtModifiedDT";
            this.txtModifiedDT.ReadOnly = true;
            this.txtModifiedDT.Size = new System.Drawing.Size(100, 20);
            this.txtModifiedDT.TabIndex = 2;
            this.txtModifiedDT.TabStop = false;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(246, 464);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 32;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(327, 464);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 33;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // gbLimits
            // 
            this.gbLimits.Controls.Add(this.txtLimHighCrash);
            this.gbLimits.Controls.Add(this.lblLimLowCrash);
            this.gbLimits.Controls.Add(this.lblLimLow);
            this.gbLimits.Controls.Add(this.lblLimHighCrash);
            this.gbLimits.Controls.Add(this.lblLimHigh);
            this.gbLimits.Controls.Add(this.txtLimHigh);
            this.gbLimits.Controls.Add(this.txtLimLowCrash);
            this.gbLimits.Controls.Add(this.txtLimLow);
            this.gbLimits.Location = new System.Drawing.Point(12, 393);
            this.gbLimits.Name = "gbLimits";
            this.gbLimits.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbLimits.Size = new System.Drawing.Size(390, 65);
            this.gbLimits.TabIndex = 31;
            this.gbLimits.TabStop = false;
            this.gbLimits.Text = "Границы";
            // 
            // gbEvents
            // 
            this.gbEvents.Controls.Add(this.chkEvEnabled);
            this.gbEvents.Controls.Add(this.chkEvSound);
            this.gbEvents.Controls.Add(this.chkEvOnChange);
            this.gbEvents.Controls.Add(this.chkEvOnUndef);
            this.gbEvents.Location = new System.Drawing.Point(12, 315);
            this.gbEvents.Name = "gbEvents";
            this.gbEvents.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbEvents.Size = new System.Drawing.Size(390, 72);
            this.gbEvents.TabIndex = 30;
            this.gbEvents.TabStop = false;
            this.gbEvents.Text = "События";
            // 
            // chkAveraging
            // 
            this.chkAveraging.AutoSize = true;
            this.chkAveraging.Location = new System.Drawing.Point(296, 212);
            this.chkAveraging.Name = "chkAveraging";
            this.chkAveraging.Size = new System.Drawing.Size(88, 17);
            this.chkAveraging.TabIndex = 20;
            this.chkAveraging.Text = "Усреднение";
            this.chkAveraging.UseVisualStyleBackColor = true;
            // 
            // txtCnlNum
            // 
            this.txtCnlNum.Location = new System.Drawing.Point(12, 51);
            this.txtCnlNum.Name = "txtCnlNum";
            this.txtCnlNum.Size = new System.Drawing.Size(50, 20);
            this.txtCnlNum.TabIndex = 4;
            // 
            // txtSignal
            // 
            this.txtSignal.Location = new System.Drawing.Point(352, 171);
            this.txtSignal.Name = "txtSignal";
            this.txtSignal.Size = new System.Drawing.Size(50, 20);
            this.txtSignal.TabIndex = 16;
            // 
            // txtCtrlCnlNum
            // 
            this.txtCtrlCnlNum.Location = new System.Drawing.Point(12, 289);
            this.txtCtrlCnlNum.Name = "txtCtrlCnlNum";
            this.txtCtrlCnlNum.Size = new System.Drawing.Size(50, 20);
            this.txtCtrlCnlNum.TabIndex = 28;
            this.txtCtrlCnlNum.TextChanged += new System.EventHandler(this.txtCtrlCnlNum_TextChanged);
            // 
            // FrmInCnlProps
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(414, 499);
            this.Controls.Add(this.txtCtrlCnlNum);
            this.Controls.Add(this.txtSignal);
            this.Controls.Add(this.txtCnlNum);
            this.Controls.Add(this.chkAveraging);
            this.Controls.Add(this.gbEvents);
            this.Controls.Add(this.gbLimits);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtModifiedDT);
            this.Controls.Add(this.lblModifiedDT);
            this.Controls.Add(this.txtCtrlCnlName);
            this.Controls.Add(this.lblCtrlCnlNum);
            this.Controls.Add(this.cbUnit);
            this.Controls.Add(this.cbFormat);
            this.Controls.Add(this.cbParam);
            this.Controls.Add(this.lblUnit);
            this.Controls.Add(this.lblFormat);
            this.Controls.Add(this.lblParam);
            this.Controls.Add(this.txtFormula);
            this.Controls.Add(this.chkFormulaUsed);
            this.Controls.Add(this.lblFormula);
            this.Controls.Add(this.lblSignal);
            this.Controls.Add(this.cbKP);
            this.Controls.Add(this.txtKPNum);
            this.Controls.Add(this.lblKP);
            this.Controls.Add(this.cbObj);
            this.Controls.Add(this.txtObjNum);
            this.Controls.Add(this.lblObj);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblCnlType);
            this.Controls.Add(this.cbCnlType);
            this.Controls.Add(this.chkActive);
            this.Controls.Add(this.lblCnlNum);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmInCnlProps";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Свойства входного канала";
            this.Load += new System.EventHandler(this.FrmInCnlProps_Load);
            this.gbLimits.ResumeLayout(false);
            this.gbLimits.PerformLayout();
            this.gbEvents.ResumeLayout(false);
            this.gbEvents.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblCnlNum;
        private System.Windows.Forms.CheckBox chkActive;
        private System.Windows.Forms.ComboBox cbCnlType;
        private System.Windows.Forms.Label lblCnlType;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblObj;
        private System.Windows.Forms.TextBox txtObjNum;
        private System.Windows.Forms.ComboBox cbObj;
        private System.Windows.Forms.ComboBox cbKP;
        private System.Windows.Forms.TextBox txtKPNum;
        private System.Windows.Forms.Label lblKP;
        private System.Windows.Forms.Label lblSignal;
        private System.Windows.Forms.Label lblFormula;
        private System.Windows.Forms.CheckBox chkFormulaUsed;
        private System.Windows.Forms.TextBox txtFormula;
        private System.Windows.Forms.Label lblParam;
        private System.Windows.Forms.Label lblFormat;
        private System.Windows.Forms.Label lblUnit;
        private System.Windows.Forms.ComboBox cbParam;
        private System.Windows.Forms.ComboBox cbFormat;
        private System.Windows.Forms.ComboBox cbUnit;
        private System.Windows.Forms.Label lblCtrlCnlNum;
        private System.Windows.Forms.TextBox txtCtrlCnlName;
        private System.Windows.Forms.CheckBox chkEvEnabled;
        private System.Windows.Forms.CheckBox chkEvSound;
        private System.Windows.Forms.CheckBox chkEvOnChange;
        private System.Windows.Forms.CheckBox chkEvOnUndef;
        private System.Windows.Forms.Label lblLimLowCrash;
        private System.Windows.Forms.Label lblLimLow;
        private System.Windows.Forms.Label lblLimHighCrash;
        private System.Windows.Forms.Label lblLimHigh;
        private System.Windows.Forms.TextBox txtLimLowCrash;
        private System.Windows.Forms.TextBox txtLimLow;
        private System.Windows.Forms.TextBox txtLimHighCrash;
        private System.Windows.Forms.TextBox txtLimHigh;
        private System.Windows.Forms.Label lblModifiedDT;
        private System.Windows.Forms.TextBox txtModifiedDT;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox gbLimits;
        private System.Windows.Forms.GroupBox gbEvents;
        private System.Windows.Forms.CheckBox chkAveraging;
        private System.Windows.Forms.TextBox txtCnlNum;
        private System.Windows.Forms.TextBox txtSignal;
        private System.Windows.Forms.TextBox txtCtrlCnlNum;
    }
}