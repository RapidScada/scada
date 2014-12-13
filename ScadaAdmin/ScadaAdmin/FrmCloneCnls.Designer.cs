namespace ScadaAdmin
{
    partial class FrmCloneCnls
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
            this.gbCnlNums = new System.Windows.Forms.GroupBox();
            this.lblNewStartNum = new System.Windows.Forms.Label();
            this.numNewStartNum = new System.Windows.Forms.NumericUpDown();
            this.numFinalNum = new System.Windows.Forms.NumericUpDown();
            this.numStartNum = new System.Windows.Forms.NumericUpDown();
            this.lblFinalNum = new System.Windows.Forms.Label();
            this.lblStartNum = new System.Windows.Forms.Label();
            this.rbInCnls = new System.Windows.Forms.RadioButton();
            this.rbCtrlCnls = new System.Windows.Forms.RadioButton();
            this.gbReplace = new System.Windows.Forms.GroupBox();
            this.cbKP = new System.Windows.Forms.ComboBox();
            this.lblKP = new System.Windows.Forms.Label();
            this.lblObj = new System.Windows.Forms.Label();
            this.cbObj = new System.Windows.Forms.ComboBox();
            this.btnClone = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.gbDataType = new System.Windows.Forms.GroupBox();
            this.gbCnlNums.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numNewStartNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFinalNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStartNum)).BeginInit();
            this.gbReplace.SuspendLayout();
            this.gbDataType.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbCnlNums
            // 
            this.gbCnlNums.Controls.Add(this.lblNewStartNum);
            this.gbCnlNums.Controls.Add(this.numNewStartNum);
            this.gbCnlNums.Controls.Add(this.numFinalNum);
            this.gbCnlNums.Controls.Add(this.numStartNum);
            this.gbCnlNums.Controls.Add(this.lblFinalNum);
            this.gbCnlNums.Controls.Add(this.lblStartNum);
            this.gbCnlNums.Location = new System.Drawing.Point(12, 67);
            this.gbCnlNums.Name = "gbCnlNums";
            this.gbCnlNums.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbCnlNums.Size = new System.Drawing.Size(308, 65);
            this.gbCnlNums.TabIndex = 1;
            this.gbCnlNums.TabStop = false;
            this.gbCnlNums.Text = "Номера каналов";
            // 
            // lblNewStartNum
            // 
            this.lblNewStartNum.AutoSize = true;
            this.lblNewStartNum.Location = new System.Drawing.Point(202, 16);
            this.lblNewStartNum.Name = "lblNewStartNum";
            this.lblNewStartNum.Size = new System.Drawing.Size(64, 13);
            this.lblNewStartNum.TabIndex = 4;
            this.lblNewStartNum.Text = "Новый нач.";
            // 
            // numNewStartNum
            // 
            this.numNewStartNum.Location = new System.Drawing.Point(205, 32);
            this.numNewStartNum.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numNewStartNum.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numNewStartNum.Name = "numNewStartNum";
            this.numNewStartNum.Size = new System.Drawing.Size(90, 20);
            this.numNewStartNum.TabIndex = 5;
            this.numNewStartNum.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numFinalNum
            // 
            this.numFinalNum.Location = new System.Drawing.Point(109, 32);
            this.numFinalNum.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numFinalNum.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numFinalNum.Name = "numFinalNum";
            this.numFinalNum.Size = new System.Drawing.Size(90, 20);
            this.numFinalNum.TabIndex = 3;
            this.numFinalNum.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numStartNum
            // 
            this.numStartNum.Location = new System.Drawing.Point(13, 32);
            this.numStartNum.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numStartNum.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numStartNum.Name = "numStartNum";
            this.numStartNum.Size = new System.Drawing.Size(90, 20);
            this.numStartNum.TabIndex = 1;
            this.numStartNum.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblFinalNum
            // 
            this.lblFinalNum.AutoSize = true;
            this.lblFinalNum.Location = new System.Drawing.Point(106, 16);
            this.lblFinalNum.Name = "lblFinalNum";
            this.lblFinalNum.Size = new System.Drawing.Size(57, 13);
            this.lblFinalNum.TabIndex = 2;
            this.lblFinalNum.Text = "Конечный";
            // 
            // lblStartNum
            // 
            this.lblStartNum.AutoSize = true;
            this.lblStartNum.Location = new System.Drawing.Point(10, 16);
            this.lblStartNum.Name = "lblStartNum";
            this.lblStartNum.Size = new System.Drawing.Size(64, 13);
            this.lblStartNum.TabIndex = 0;
            this.lblStartNum.Text = "Начальный";
            // 
            // rbInCnls
            // 
            this.rbInCnls.AutoSize = true;
            this.rbInCnls.Checked = true;
            this.rbInCnls.Location = new System.Drawing.Point(13, 19);
            this.rbInCnls.Name = "rbInCnls";
            this.rbInCnls.Size = new System.Drawing.Size(110, 17);
            this.rbInCnls.TabIndex = 0;
            this.rbInCnls.TabStop = true;
            this.rbInCnls.Text = "Входные каналы";
            this.rbInCnls.UseVisualStyleBackColor = true;
            // 
            // rbCtrlCnls
            // 
            this.rbCtrlCnls.AutoSize = true;
            this.rbCtrlCnls.Location = new System.Drawing.Point(129, 19);
            this.rbCtrlCnls.Name = "rbCtrlCnls";
            this.rbCtrlCnls.Size = new System.Drawing.Size(126, 17);
            this.rbCtrlCnls.TabIndex = 1;
            this.rbCtrlCnls.TabStop = true;
            this.rbCtrlCnls.Text = "Каналы управления";
            this.rbCtrlCnls.UseVisualStyleBackColor = true;
            // 
            // gbReplace
            // 
            this.gbReplace.Controls.Add(this.cbKP);
            this.gbReplace.Controls.Add(this.lblKP);
            this.gbReplace.Controls.Add(this.lblObj);
            this.gbReplace.Controls.Add(this.cbObj);
            this.gbReplace.Location = new System.Drawing.Point(12, 138);
            this.gbReplace.Name = "gbReplace";
            this.gbReplace.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbReplace.Size = new System.Drawing.Size(308, 66);
            this.gbReplace.TabIndex = 2;
            this.gbReplace.TabStop = false;
            this.gbReplace.Text = "Замена";
            // 
            // cbKP
            // 
            this.cbKP.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbKP.FormattingEnabled = true;
            this.cbKP.Location = new System.Drawing.Point(157, 32);
            this.cbKP.Name = "cbKP";
            this.cbKP.Size = new System.Drawing.Size(138, 21);
            this.cbKP.TabIndex = 3;
            // 
            // lblKP
            // 
            this.lblKP.AutoSize = true;
            this.lblKP.Location = new System.Drawing.Point(154, 16);
            this.lblKP.Name = "lblKP";
            this.lblKP.Size = new System.Drawing.Size(22, 13);
            this.lblKP.TabIndex = 2;
            this.lblKP.Text = "КП";
            // 
            // lblObj
            // 
            this.lblObj.AutoSize = true;
            this.lblObj.Location = new System.Drawing.Point(10, 16);
            this.lblObj.Name = "lblObj";
            this.lblObj.Size = new System.Drawing.Size(45, 13);
            this.lblObj.TabIndex = 0;
            this.lblObj.Text = "Объект";
            // 
            // cbObj
            // 
            this.cbObj.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbObj.FormattingEnabled = true;
            this.cbObj.Location = new System.Drawing.Point(13, 32);
            this.cbObj.Name = "cbObj";
            this.cbObj.Size = new System.Drawing.Size(138, 21);
            this.cbObj.TabIndex = 1;
            // 
            // btnClone
            // 
            this.btnClone.Location = new System.Drawing.Point(149, 210);
            this.btnClone.Name = "btnClone";
            this.btnClone.Size = new System.Drawing.Size(90, 23);
            this.btnClone.TabIndex = 3;
            this.btnClone.Text = "Клонировать";
            this.btnClone.UseVisualStyleBackColor = true;
            this.btnClone.Click += new System.EventHandler(this.btnClone_Click);
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(245, 210);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "Закрыть";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // gbDataType
            // 
            this.gbDataType.Controls.Add(this.rbInCnls);
            this.gbDataType.Controls.Add(this.rbCtrlCnls);
            this.gbDataType.Location = new System.Drawing.Point(12, 12);
            this.gbDataType.Name = "gbDataType";
            this.gbDataType.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbDataType.Size = new System.Drawing.Size(308, 49);
            this.gbDataType.TabIndex = 0;
            this.gbDataType.TabStop = false;
            this.gbDataType.Text = "Тип данных";
            // 
            // FrmCloneCnls
            // 
            this.AcceptButton = this.btnClone;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(332, 245);
            this.Controls.Add(this.gbDataType);
            this.Controls.Add(this.btnClone);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.gbReplace);
            this.Controls.Add(this.gbCnlNums);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmCloneCnls";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Клонирование каналов";
            this.Load += new System.EventHandler(this.FrmCloneCnls_Load);
            this.Shown += new System.EventHandler(this.FrmCloneCnls_Shown);
            this.gbCnlNums.ResumeLayout(false);
            this.gbCnlNums.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numNewStartNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFinalNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStartNum)).EndInit();
            this.gbReplace.ResumeLayout(false);
            this.gbReplace.PerformLayout();
            this.gbDataType.ResumeLayout(false);
            this.gbDataType.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbCnlNums;
        private System.Windows.Forms.Label lblNewStartNum;
        private System.Windows.Forms.NumericUpDown numNewStartNum;
        private System.Windows.Forms.NumericUpDown numFinalNum;
        private System.Windows.Forms.NumericUpDown numStartNum;
        private System.Windows.Forms.Label lblFinalNum;
        private System.Windows.Forms.Label lblStartNum;
        private System.Windows.Forms.RadioButton rbInCnls;
        private System.Windows.Forms.RadioButton rbCtrlCnls;
        private System.Windows.Forms.GroupBox gbReplace;
        private System.Windows.Forms.Button btnClone;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblObj;
        private System.Windows.Forms.ComboBox cbObj;
        private System.Windows.Forms.ComboBox cbKP;
        private System.Windows.Forms.Label lblKP;
        private System.Windows.Forms.GroupBox gbDataType;
    }
}