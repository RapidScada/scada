namespace ScadaAdmin
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
            this.lblBaseSDFFile = new System.Windows.Forms.Label();
            this.txtBaseSDFFile = new System.Windows.Forms.TextBox();
            this.lblBaseDATDir = new System.Windows.Forms.Label();
            this.txtBaseDATDir = new System.Windows.Forms.TextBox();
            this.lblBackupDir = new System.Windows.Forms.Label();
            this.txtBackupDir = new System.Windows.Forms.TextBox();
            this.lblCommDir = new System.Windows.Forms.Label();
            this.txtCommDir = new System.Windows.Forms.TextBox();
            this.btnBaseSDFFile = new System.Windows.Forms.Button();
            this.btnBaseDATDir = new System.Windows.Forms.Button();
            this.btnBackupDir = new System.Windows.Forms.Button();
            this.btnCommDir = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.chkAutoBackupBase = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // lblBaseSDFFile
            // 
            this.lblBaseSDFFile.AutoSize = true;
            this.lblBaseSDFFile.Location = new System.Drawing.Point(9, 9);
            this.lblBaseSDFFile.Name = "lblBaseSDFFile";
            this.lblBaseSDFFile.Size = new System.Drawing.Size(221, 13);
            this.lblBaseSDFFile.TabIndex = 0;
            this.lblBaseSDFFile.Text = "Файл базы конфигурации в формате SDF";
            // 
            // txtBaseSDFFile
            // 
            this.txtBaseSDFFile.Location = new System.Drawing.Point(12, 25);
            this.txtBaseSDFFile.Name = "txtBaseSDFFile";
            this.txtBaseSDFFile.Size = new System.Drawing.Size(344, 20);
            this.txtBaseSDFFile.TabIndex = 1;
            // 
            // lblBaseDATDir
            // 
            this.lblBaseDATDir.AutoSize = true;
            this.lblBaseDATDir.Location = new System.Drawing.Point(9, 48);
            this.lblBaseDATDir.Name = "lblBaseDATDir";
            this.lblBaseDATDir.Size = new System.Drawing.Size(255, 13);
            this.lblBaseDATDir.TabIndex = 3;
            this.lblBaseDATDir.Text = "Директория базы конфигурации в формате DAT";
            // 
            // txtBaseDATDir
            // 
            this.txtBaseDATDir.Location = new System.Drawing.Point(12, 64);
            this.txtBaseDATDir.Name = "txtBaseDATDir";
            this.txtBaseDATDir.Size = new System.Drawing.Size(344, 20);
            this.txtBaseDATDir.TabIndex = 4;
            // 
            // lblBackupDir
            // 
            this.lblBackupDir.AutoSize = true;
            this.lblBackupDir.Location = new System.Drawing.Point(9, 87);
            this.lblBackupDir.Name = "lblBackupDir";
            this.lblBackupDir.Size = new System.Drawing.Size(304, 13);
            this.lblBackupDir.TabIndex = 6;
            this.lblBackupDir.Text = "Директория резервного копирования базы конфигурации";
            // 
            // txtBackupDir
            // 
            this.txtBackupDir.Location = new System.Drawing.Point(12, 103);
            this.txtBackupDir.Name = "txtBackupDir";
            this.txtBackupDir.Size = new System.Drawing.Size(344, 20);
            this.txtBackupDir.TabIndex = 7;
            // 
            // lblCommDir
            // 
            this.lblCommDir.AutoSize = true;
            this.lblCommDir.Location = new System.Drawing.Point(9, 126);
            this.lblCommDir.Name = "lblCommDir";
            this.lblCommDir.Size = new System.Drawing.Size(192, 13);
            this.lblCommDir.TabIndex = 9;
            this.lblCommDir.Text = "Директория SCADA-Коммуникатора";
            // 
            // txtCommDir
            // 
            this.txtCommDir.Location = new System.Drawing.Point(12, 142);
            this.txtCommDir.Name = "txtCommDir";
            this.txtCommDir.Size = new System.Drawing.Size(344, 20);
            this.txtCommDir.TabIndex = 10;
            // 
            // btnBaseSDFFile
            // 
            this.btnBaseSDFFile.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnBaseSDFFile.Image = ((System.Drawing.Image)(resources.GetObject("btnBaseSDFFile.Image")));
            this.btnBaseSDFFile.Location = new System.Drawing.Point(362, 25);
            this.btnBaseSDFFile.Name = "btnBaseSDFFile";
            this.btnBaseSDFFile.Size = new System.Drawing.Size(20, 20);
            this.btnBaseSDFFile.TabIndex = 2;
            this.btnBaseSDFFile.UseVisualStyleBackColor = true;
            this.btnBaseSDFFile.Click += new System.EventHandler(this.btnBaseSDFFile_Click);
            // 
            // btnBaseDATDir
            // 
            this.btnBaseDATDir.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnBaseDATDir.Image = ((System.Drawing.Image)(resources.GetObject("btnBaseDATDir.Image")));
            this.btnBaseDATDir.Location = new System.Drawing.Point(362, 64);
            this.btnBaseDATDir.Name = "btnBaseDATDir";
            this.btnBaseDATDir.Size = new System.Drawing.Size(20, 20);
            this.btnBaseDATDir.TabIndex = 5;
            this.btnBaseDATDir.UseVisualStyleBackColor = true;
            this.btnBaseDATDir.Click += new System.EventHandler(this.btnBaseDATDir_Click);
            // 
            // btnBackupDir
            // 
            this.btnBackupDir.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnBackupDir.Image = ((System.Drawing.Image)(resources.GetObject("btnBackupDir.Image")));
            this.btnBackupDir.Location = new System.Drawing.Point(362, 102);
            this.btnBackupDir.Name = "btnBackupDir";
            this.btnBackupDir.Size = new System.Drawing.Size(20, 20);
            this.btnBackupDir.TabIndex = 8;
            this.btnBackupDir.UseVisualStyleBackColor = true;
            this.btnBackupDir.Click += new System.EventHandler(this.btnBackupDir_Click);
            // 
            // btnCommDir
            // 
            this.btnCommDir.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCommDir.Image = ((System.Drawing.Image)(resources.GetObject("btnCommDir.Image")));
            this.btnCommDir.Location = new System.Drawing.Point(362, 142);
            this.btnCommDir.Name = "btnCommDir";
            this.btnCommDir.Size = new System.Drawing.Size(20, 20);
            this.btnCommDir.TabIndex = 11;
            this.btnCommDir.UseVisualStyleBackColor = true;
            this.btnCommDir.Click += new System.EventHandler(this.btnCommDir_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(307, 191);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 14;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(226, 191);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 13;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.DefaultExt = "*.sdf";
            this.openFileDialog.FileName = "ScadaBase.sdf";
            this.openFileDialog.Filter = "Базы конфигурации|*.sdf|Все файлы|*.*";
            this.openFileDialog.Title = "Выберите файл базы конфигурации SCADA-Администратора в формате SDF";
            // 
            // chkAutoBackupBase
            // 
            this.chkAutoBackupBase.AutoSize = true;
            this.chkAutoBackupBase.Location = new System.Drawing.Point(12, 168);
            this.chkAutoBackupBase.Name = "chkAutoBackupBase";
            this.chkAutoBackupBase.Size = new System.Drawing.Size(285, 17);
            this.chkAutoBackupBase.TabIndex = 12;
            this.chkAutoBackupBase.Text = "Автоматически резервировать базу конфигурации";
            this.chkAutoBackupBase.UseVisualStyleBackColor = true;
            // 
            // FrmSettings
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(394, 226);
            this.Controls.Add(this.chkAutoBackupBase);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnCommDir);
            this.Controls.Add(this.btnBackupDir);
            this.Controls.Add(this.btnBaseDATDir);
            this.Controls.Add(this.btnBaseSDFFile);
            this.Controls.Add(this.txtCommDir);
            this.Controls.Add(this.lblCommDir);
            this.Controls.Add(this.txtBackupDir);
            this.Controls.Add(this.lblBackupDir);
            this.Controls.Add(this.txtBaseDATDir);
            this.Controls.Add(this.lblBaseDATDir);
            this.Controls.Add(this.txtBaseSDFFile);
            this.Controls.Add(this.lblBaseSDFFile);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmSettings";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Параметры";
            this.Load += new System.EventHandler(this.FrmSettings_Load);
            this.Shown += new System.EventHandler(this.FrmSettings_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblBaseSDFFile;
        private System.Windows.Forms.TextBox txtBaseSDFFile;
        private System.Windows.Forms.Label lblBaseDATDir;
        private System.Windows.Forms.TextBox txtBaseDATDir;
        private System.Windows.Forms.Label lblBackupDir;
        private System.Windows.Forms.TextBox txtBackupDir;
        private System.Windows.Forms.Label lblCommDir;
        private System.Windows.Forms.TextBox txtCommDir;
        private System.Windows.Forms.Button btnBaseSDFFile;
        private System.Windows.Forms.Button btnBaseDATDir;
        private System.Windows.Forms.Button btnBackupDir;
        private System.Windows.Forms.Button btnCommDir;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.CheckBox chkAutoBackupBase;
    }
}