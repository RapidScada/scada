namespace ScadaAdmin.Remote
{
    partial class FrmDownloadConfig
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmDownloadConfig));
            this.rbSaveToDir = new System.Windows.Forms.RadioButton();
            this.txtDestDir = new System.Windows.Forms.TextBox();
            this.btnBrowseDestDir = new System.Windows.Forms.Button();
            this.rbSaveToArc = new System.Windows.Forms.RadioButton();
            this.btnSelectDestFile = new System.Windows.Forms.Button();
            this.txtDestFile = new System.Windows.Forms.TextBox();
            this.btnDownload = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.gbOptions = new System.Windows.Forms.GroupBox();
            this.chkIncludeSpecificFiles = new System.Windows.Forms.CheckBox();
            this.chkImportBase = new System.Windows.Forms.CheckBox();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.ctrlServerConn = new ScadaAdmin.Remote.CtrlServerConn();
            this.gbOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // rbSaveToDir
            // 
            this.rbSaveToDir.AutoSize = true;
            this.rbSaveToDir.Location = new System.Drawing.Point(13, 19);
            this.rbSaveToDir.Name = "rbSaveToDir";
            this.rbSaveToDir.Size = new System.Drawing.Size(154, 17);
            this.rbSaveToDir.TabIndex = 0;
            this.rbSaveToDir.Text = "Сохранить в директорию:";
            this.rbSaveToDir.UseVisualStyleBackColor = true;
            this.rbSaveToDir.CheckedChanged += new System.EventHandler(this.rbSave_CheckedChanged);
            // 
            // txtDestDir
            // 
            this.txtDestDir.Location = new System.Drawing.Point(13, 43);
            this.txtDestDir.Name = "txtDestDir";
            this.txtDestDir.Size = new System.Drawing.Size(417, 20);
            this.txtDestDir.TabIndex = 1;
            this.txtDestDir.Text = "C:\\SCADA\\";
            this.txtDestDir.TextChanged += new System.EventHandler(this.downloadControl_Changed);
            // 
            // btnBrowseDestDir
            // 
            this.btnBrowseDestDir.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnBrowseDestDir.Image = ((System.Drawing.Image)(resources.GetObject("btnBrowseDestDir.Image")));
            this.btnBrowseDestDir.Location = new System.Drawing.Point(436, 43);
            this.btnBrowseDestDir.Name = "btnBrowseDestDir";
            this.btnBrowseDestDir.Size = new System.Drawing.Size(20, 20);
            this.btnBrowseDestDir.TabIndex = 2;
            this.btnBrowseDestDir.UseVisualStyleBackColor = true;
            this.btnBrowseDestDir.Click += new System.EventHandler(this.btnBrowseDestDir_Click);
            // 
            // rbSaveToArc
            // 
            this.rbSaveToArc.AutoSize = true;
            this.rbSaveToArc.Location = new System.Drawing.Point(13, 69);
            this.rbSaveToArc.Name = "rbSaveToArc";
            this.rbSaveToArc.Size = new System.Drawing.Size(122, 17);
            this.rbSaveToArc.TabIndex = 3;
            this.rbSaveToArc.Text = "Сохранить в архив:";
            this.rbSaveToArc.UseVisualStyleBackColor = true;
            this.rbSaveToArc.CheckedChanged += new System.EventHandler(this.rbSave_CheckedChanged);
            // 
            // btnSelectDestFile
            // 
            this.btnSelectDestFile.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSelectDestFile.Image = ((System.Drawing.Image)(resources.GetObject("btnSelectDestFile.Image")));
            this.btnSelectDestFile.Location = new System.Drawing.Point(436, 92);
            this.btnSelectDestFile.Name = "btnSelectDestFile";
            this.btnSelectDestFile.Size = new System.Drawing.Size(20, 20);
            this.btnSelectDestFile.TabIndex = 5;
            this.btnSelectDestFile.UseVisualStyleBackColor = true;
            this.btnSelectDestFile.Click += new System.EventHandler(this.btnSelectDestFile_Click);
            // 
            // txtDestFile
            // 
            this.txtDestFile.Location = new System.Drawing.Point(13, 92);
            this.txtDestFile.Name = "txtDestFile";
            this.txtDestFile.Size = new System.Drawing.Size(417, 20);
            this.txtDestFile.TabIndex = 4;
            this.txtDestFile.TextChanged += new System.EventHandler(this.downloadControl_Changed);
            // 
            // btnDownload
            // 
            this.btnDownload.Location = new System.Drawing.Point(325, 255);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(75, 23);
            this.btnDownload.TabIndex = 2;
            this.btnDownload.Text = "Скачать";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(406, 255);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Закрыть";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // gbOptions
            // 
            this.gbOptions.Controls.Add(this.chkIncludeSpecificFiles);
            this.gbOptions.Controls.Add(this.chkImportBase);
            this.gbOptions.Controls.Add(this.rbSaveToDir);
            this.gbOptions.Controls.Add(this.txtDestDir);
            this.gbOptions.Controls.Add(this.btnBrowseDestDir);
            this.gbOptions.Controls.Add(this.btnSelectDestFile);
            this.gbOptions.Controls.Add(this.rbSaveToArc);
            this.gbOptions.Controls.Add(this.txtDestFile);
            this.gbOptions.Location = new System.Drawing.Point(12, 73);
            this.gbOptions.Name = "gbOptions";
            this.gbOptions.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbOptions.Size = new System.Drawing.Size(469, 176);
            this.gbOptions.TabIndex = 1;
            this.gbOptions.TabStop = false;
            this.gbOptions.Text = "Параметры скачивания";
            // 
            // chkIncludeSpecificFiles
            // 
            this.chkIncludeSpecificFiles.AutoSize = true;
            this.chkIncludeSpecificFiles.Location = new System.Drawing.Point(13, 123);
            this.chkIncludeSpecificFiles.Name = "chkIncludeSpecificFiles";
            this.chkIncludeSpecificFiles.Size = new System.Drawing.Size(441, 17);
            this.chkIncludeSpecificFiles.TabIndex = 6;
            this.chkIncludeSpecificFiles.Text = "Включая файлы специфичные для сервера, в том числе, регистрационные ключи";
            this.chkIncludeSpecificFiles.UseVisualStyleBackColor = true;
            this.chkIncludeSpecificFiles.CheckedChanged += new System.EventHandler(this.downloadControl_Changed);
            // 
            // chkImportBase
            // 
            this.chkImportBase.AutoSize = true;
            this.chkImportBase.Location = new System.Drawing.Point(13, 146);
            this.chkImportBase.Name = "chkImportBase";
            this.chkImportBase.Size = new System.Drawing.Size(339, 17);
            this.chkImportBase.TabIndex = 7;
            this.chkImportBase.Text = "Импорт базы конфигурации в формат SDF после скачивания";
            this.chkImportBase.UseVisualStyleBackColor = true;
            this.chkImportBase.CheckedChanged += new System.EventHandler(this.downloadControl_Changed);
            // 
            // openFileDialog
            // 
            this.openFileDialog.CheckFileExists = false;
            this.openFileDialog.DefaultExt = "*.zip";
            this.openFileDialog.Filter = "Архивы конфигурации (*.zip)|*.zip|Все файлы (*.*)|*.*";
            this.openFileDialog.Title = "Выберите файл архива конфигурации";
            // 
            // folderBrowserDialog
            // 
            this.folderBrowserDialog.Description = "Выберите директорию конфигурации";
            // 
            // ctrlServerConn
            // 
            this.ctrlServerConn.Location = new System.Drawing.Point(12, 12);
            this.ctrlServerConn.Name = "ctrlServerConn";
            this.ctrlServerConn.ServersSettings = null;
            this.ctrlServerConn.Size = new System.Drawing.Size(469, 55);
            this.ctrlServerConn.TabIndex = 0;
            this.ctrlServerConn.SelectedSettingsChanged += new System.EventHandler(this.ctrlServerConn_SelectedSettingsChanged);
            // 
            // FrmDownloadConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(493, 290);
            this.Controls.Add(this.ctrlServerConn);
            this.Controls.Add(this.gbOptions);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnDownload);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmDownloadConfig";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Скачивание конфигурации";
            this.Load += new System.EventHandler(this.FrmDownloadConfig_Load);
            this.gbOptions.ResumeLayout(false);
            this.gbOptions.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.RadioButton rbSaveToDir;
        private System.Windows.Forms.TextBox txtDestDir;
        private System.Windows.Forms.Button btnBrowseDestDir;
        private System.Windows.Forms.RadioButton rbSaveToArc;
        private System.Windows.Forms.Button btnSelectDestFile;
        private System.Windows.Forms.TextBox txtDestFile;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.GroupBox gbOptions;
        private CtrlServerConn ctrlServerConn;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.CheckBox chkImportBase;
        private System.Windows.Forms.CheckBox chkIncludeSpecificFiles;
    }
}