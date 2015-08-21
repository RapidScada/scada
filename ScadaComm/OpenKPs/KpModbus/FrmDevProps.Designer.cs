namespace Scada.Comm.Devices.KpModbus
{
    partial class FrmDevProps
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
            this.gbDevice = new System.Windows.Forms.GroupBox();
            this.lblDevTemplate = new System.Windows.Forms.Label();
            this.txtDevTemplate = new System.Windows.Forms.TextBox();
            this.btnCreateDevTemplate = new System.Windows.Forms.Button();
            this.btnBrowseDevTemplate = new System.Windows.Forms.Button();
            this.btnEditDevTemplate = new System.Windows.Forms.Button();
            this.gbCommLine = new System.Windows.Forms.GroupBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblTransMode = new System.Windows.Forms.Label();
            this.cbTransMode = new System.Windows.Forms.ComboBox();
            this.gbDevice.SuspendLayout();
            this.gbCommLine.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbDevice
            // 
            this.gbDevice.Controls.Add(this.btnEditDevTemplate);
            this.gbDevice.Controls.Add(this.btnBrowseDevTemplate);
            this.gbDevice.Controls.Add(this.btnCreateDevTemplate);
            this.gbDevice.Controls.Add(this.txtDevTemplate);
            this.gbDevice.Controls.Add(this.lblDevTemplate);
            this.gbDevice.Location = new System.Drawing.Point(12, 84);
            this.gbDevice.Name = "gbDevice";
            this.gbDevice.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbDevice.Size = new System.Drawing.Size(288, 94);
            this.gbDevice.TabIndex = 1;
            this.gbDevice.TabStop = false;
            this.gbDevice.Text = "КП";
            // 
            // lblDevTemplate
            // 
            this.lblDevTemplate.AutoSize = true;
            this.lblDevTemplate.Location = new System.Drawing.Point(10, 16);
            this.lblDevTemplate.Name = "lblDevTemplate";
            this.lblDevTemplate.Size = new System.Drawing.Size(106, 13);
            this.lblDevTemplate.TabIndex = 0;
            this.lblDevTemplate.Text = "Шаблон устройства";
            // 
            // txtDevTemplate
            // 
            this.txtDevTemplate.Location = new System.Drawing.Point(13, 32);
            this.txtDevTemplate.Name = "txtDevTemplate";
            this.txtDevTemplate.Size = new System.Drawing.Size(262, 20);
            this.txtDevTemplate.TabIndex = 1;
            // 
            // btnCreateDevTemplate
            // 
            this.btnCreateDevTemplate.Location = new System.Drawing.Point(13, 58);
            this.btnCreateDevTemplate.Name = "btnCreateDevTemplate";
            this.btnCreateDevTemplate.Size = new System.Drawing.Size(75, 23);
            this.btnCreateDevTemplate.TabIndex = 2;
            this.btnCreateDevTemplate.Text = "Создать";
            this.btnCreateDevTemplate.UseVisualStyleBackColor = true;
            // 
            // btnBrowseDevTemplate
            // 
            this.btnBrowseDevTemplate.Location = new System.Drawing.Point(94, 58);
            this.btnBrowseDevTemplate.Name = "btnBrowseDevTemplate";
            this.btnBrowseDevTemplate.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseDevTemplate.TabIndex = 3;
            this.btnBrowseDevTemplate.Text = "Выбрать";
            this.btnBrowseDevTemplate.UseVisualStyleBackColor = true;
            // 
            // btnEditDevTemplate
            // 
            this.btnEditDevTemplate.Location = new System.Drawing.Point(175, 58);
            this.btnEditDevTemplate.Name = "btnEditDevTemplate";
            this.btnEditDevTemplate.Size = new System.Drawing.Size(100, 23);
            this.btnEditDevTemplate.TabIndex = 4;
            this.btnEditDevTemplate.Text = "Редактировать";
            this.btnEditDevTemplate.UseVisualStyleBackColor = true;
            // 
            // gbCommLine
            // 
            this.gbCommLine.Controls.Add(this.cbTransMode);
            this.gbCommLine.Controls.Add(this.lblTransMode);
            this.gbCommLine.Location = new System.Drawing.Point(12, 12);
            this.gbCommLine.Name = "gbCommLine";
            this.gbCommLine.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbCommLine.Size = new System.Drawing.Size(288, 66);
            this.gbCommLine.TabIndex = 0;
            this.gbCommLine.TabStop = false;
            this.gbCommLine.Text = "Линия связи";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(144, 184);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(225, 184);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // lblTransMode
            // 
            this.lblTransMode.AutoSize = true;
            this.lblTransMode.Location = new System.Drawing.Point(10, 16);
            this.lblTransMode.Name = "lblTransMode";
            this.lblTransMode.Size = new System.Drawing.Size(56, 13);
            this.lblTransMode.TabIndex = 0;
            this.lblTransMode.Text = "Протокол";
            // 
            // cbTransMode
            // 
            this.cbTransMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTransMode.FormattingEnabled = true;
            this.cbTransMode.Location = new System.Drawing.Point(13, 32);
            this.cbTransMode.Name = "cbTransMode";
            this.cbTransMode.Size = new System.Drawing.Size(262, 21);
            this.cbTransMode.TabIndex = 1;
            // 
            // FrmDevProps
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(312, 219);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.gbCommLine);
            this.Controls.Add(this.gbDevice);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmDevProps";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Свойства КП";
            this.gbDevice.ResumeLayout(false);
            this.gbDevice.PerformLayout();
            this.gbCommLine.ResumeLayout(false);
            this.gbCommLine.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbDevice;
        private System.Windows.Forms.Button btnEditDevTemplate;
        private System.Windows.Forms.Button btnBrowseDevTemplate;
        private System.Windows.Forms.Button btnCreateDevTemplate;
        private System.Windows.Forms.TextBox txtDevTemplate;
        private System.Windows.Forms.Label lblDevTemplate;
        private System.Windows.Forms.GroupBox gbCommLine;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ComboBox cbTransMode;
        private System.Windows.Forms.Label lblTransMode;
    }
}