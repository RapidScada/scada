namespace ScadaTableEditor
{
    partial class FrmSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSettings));
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnBaseDATDir = new System.Windows.Forms.Button();
            this.txtBaseDATDir = new System.Windows.Forms.TextBox();
            this.lblBaseDATDir = new System.Windows.Forms.Label();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(126, 51);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(207, 51);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnBaseDATDir
            // 
            this.btnBaseDATDir.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnBaseDATDir.Image = ((System.Drawing.Image)(resources.GetObject("btnBaseDATDir.Image")));
            this.btnBaseDATDir.Location = new System.Drawing.Point(262, 25);
            this.btnBaseDATDir.Name = "btnBaseDATDir";
            this.btnBaseDATDir.Size = new System.Drawing.Size(20, 20);
            this.btnBaseDATDir.TabIndex = 2;
            this.btnBaseDATDir.UseVisualStyleBackColor = true;
            this.btnBaseDATDir.Click += new System.EventHandler(this.btnBaseDATDir_Click);
            // 
            // txtBaseDATDir
            // 
            this.txtBaseDATDir.Location = new System.Drawing.Point(12, 25);
            this.txtBaseDATDir.Name = "txtBaseDATDir";
            this.txtBaseDATDir.Size = new System.Drawing.Size(244, 20);
            this.txtBaseDATDir.TabIndex = 1;
            // 
            // lblBaseDATDir
            // 
            this.lblBaseDATDir.AutoSize = true;
            this.lblBaseDATDir.Location = new System.Drawing.Point(9, 9);
            this.lblBaseDATDir.Name = "lblBaseDATDir";
            this.lblBaseDATDir.Size = new System.Drawing.Size(255, 13);
            this.lblBaseDATDir.TabIndex = 0;
            this.lblBaseDATDir.Text = "Директория базы конфигурации в формате DAT";
            // 
            // folderBrowserDialog
            // 
            this.folderBrowserDialog.Description = "Выберите директорию базы конфигурации в формате DAT";
            // 
            // FrmSettings
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(294, 86);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnBaseDATDir);
            this.Controls.Add(this.txtBaseDATDir);
            this.Controls.Add(this.lblBaseDATDir);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmSettings";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Параметры";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnBaseDATDir;
        private System.Windows.Forms.TextBox txtBaseDATDir;
        private System.Windows.Forms.Label lblBaseDATDir;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
    }
}