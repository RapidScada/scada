namespace ScadaAdmin
{
    partial class FrmCnlMap
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
            this.btnCreate = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.gbDataType = new System.Windows.Forms.GroupBox();
            this.rbCtrlCnls = new System.Windows.Forms.RadioButton();
            this.rbInCnls = new System.Windows.Forms.RadioButton();
            this.gbGroupBy = new System.Windows.Forms.GroupBox();
            this.rbGroupByKP = new System.Windows.Forms.RadioButton();
            this.rbGroupByObj = new System.Windows.Forms.RadioButton();
            this.gbDataType.SuspendLayout();
            this.gbGroupBy.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCreate
            // 
            this.btnCreate.Location = new System.Drawing.Point(126, 122);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(75, 23);
            this.btnCreate.TabIndex = 2;
            this.btnCreate.Text = "Создать";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(207, 122);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Закрыть";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // gbDataType
            // 
            this.gbDataType.Controls.Add(this.rbCtrlCnls);
            this.gbDataType.Controls.Add(this.rbInCnls);
            this.gbDataType.Location = new System.Drawing.Point(12, 12);
            this.gbDataType.Name = "gbDataType";
            this.gbDataType.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbDataType.Size = new System.Drawing.Size(270, 49);
            this.gbDataType.TabIndex = 0;
            this.gbDataType.TabStop = false;
            this.gbDataType.Text = "Тип данных";
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
            // gbGroupBy
            // 
            this.gbGroupBy.Controls.Add(this.rbGroupByKP);
            this.gbGroupBy.Controls.Add(this.rbGroupByObj);
            this.gbGroupBy.Location = new System.Drawing.Point(12, 67);
            this.gbGroupBy.Name = "gbGroupBy";
            this.gbGroupBy.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbGroupBy.Size = new System.Drawing.Size(270, 49);
            this.gbGroupBy.TabIndex = 1;
            this.gbGroupBy.TabStop = false;
            this.gbGroupBy.Text = "Группировать";
            // 
            // rbGroupByKP
            // 
            this.rbGroupByKP.AutoSize = true;
            this.rbGroupByKP.Location = new System.Drawing.Point(129, 19);
            this.rbGroupByKP.Name = "rbGroupByKP";
            this.rbGroupByKP.Size = new System.Drawing.Size(57, 17);
            this.rbGroupByKP.TabIndex = 1;
            this.rbGroupByKP.TabStop = true;
            this.rbGroupByKP.Text = "По КП";
            this.rbGroupByKP.UseVisualStyleBackColor = true;
            // 
            // rbGroupByObj
            // 
            this.rbGroupByObj.AutoSize = true;
            this.rbGroupByObj.Checked = true;
            this.rbGroupByObj.Location = new System.Drawing.Point(13, 19);
            this.rbGroupByObj.Name = "rbGroupByObj";
            this.rbGroupByObj.Size = new System.Drawing.Size(92, 17);
            this.rbGroupByObj.TabIndex = 0;
            this.rbGroupByObj.TabStop = true;
            this.rbGroupByObj.Text = "По объектам";
            this.rbGroupByObj.UseVisualStyleBackColor = true;
            // 
            // FrmCnlsMap
            // 
            this.AcceptButton = this.btnCreate;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(294, 157);
            this.Controls.Add(this.gbGroupBy);
            this.Controls.Add(this.gbDataType);
            this.Controls.Add(this.btnCreate);
            this.Controls.Add(this.btnClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmCnlsMap";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Карта каналов";
            this.Load += new System.EventHandler(this.FrmCnlMap_Load);
            this.gbDataType.ResumeLayout(false);
            this.gbDataType.PerformLayout();
            this.gbGroupBy.ResumeLayout(false);
            this.gbGroupBy.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.GroupBox gbDataType;
        private System.Windows.Forms.RadioButton rbCtrlCnls;
        private System.Windows.Forms.RadioButton rbInCnls;
        private System.Windows.Forms.GroupBox gbGroupBy;
        private System.Windows.Forms.RadioButton rbGroupByKP;
        private System.Windows.Forms.RadioButton rbGroupByObj;
    }
}