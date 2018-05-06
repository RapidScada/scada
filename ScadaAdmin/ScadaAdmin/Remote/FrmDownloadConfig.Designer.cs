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
            this.gbConnection = new System.Windows.Forms.GroupBox();
            this.btnRemoveConn = new System.Windows.Forms.Button();
            this.btnEditConn = new System.Windows.Forms.Button();
            this.btnCreateConn = new System.Windows.Forms.Button();
            this.cbConnection = new System.Windows.Forms.ComboBox();
            this.rbSaveToDir = new System.Windows.Forms.RadioButton();
            this.txtDestDir = new System.Windows.Forms.TextBox();
            this.btnBrowseDestDir = new System.Windows.Forms.Button();
            this.rbSaveToArc = new System.Windows.Forms.RadioButton();
            this.btnSelectDestFile = new System.Windows.Forms.Button();
            this.txtDestFile = new System.Windows.Forms.TextBox();
            this.btnDownload = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.gbOptions = new System.Windows.Forms.GroupBox();
            this.gbConnection.SuspendLayout();
            this.gbOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbConnection
            // 
            this.gbConnection.Controls.Add(this.btnRemoveConn);
            this.gbConnection.Controls.Add(this.btnEditConn);
            this.gbConnection.Controls.Add(this.btnCreateConn);
            this.gbConnection.Controls.Add(this.cbConnection);
            this.gbConnection.Location = new System.Drawing.Point(12, 12);
            this.gbConnection.Name = "gbConnection";
            this.gbConnection.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbConnection.Size = new System.Drawing.Size(469, 55);
            this.gbConnection.TabIndex = 0;
            this.gbConnection.TabStop = false;
            this.gbConnection.Text = "Подключение к серверу";
            // 
            // btnRemoveConn
            // 
            this.btnRemoveConn.Location = new System.Drawing.Point(381, 19);
            this.btnRemoveConn.Name = "btnRemoveConn";
            this.btnRemoveConn.Size = new System.Drawing.Size(75, 23);
            this.btnRemoveConn.TabIndex = 3;
            this.btnRemoveConn.Text = "Удалить";
            this.btnRemoveConn.UseVisualStyleBackColor = true;
            // 
            // btnEditConn
            // 
            this.btnEditConn.Location = new System.Drawing.Point(300, 19);
            this.btnEditConn.Name = "btnEditConn";
            this.btnEditConn.Size = new System.Drawing.Size(75, 23);
            this.btnEditConn.TabIndex = 2;
            this.btnEditConn.Text = "Настроить";
            this.btnEditConn.UseVisualStyleBackColor = true;
            // 
            // btnCreateConn
            // 
            this.btnCreateConn.Location = new System.Drawing.Point(219, 19);
            this.btnCreateConn.Name = "btnCreateConn";
            this.btnCreateConn.Size = new System.Drawing.Size(75, 23);
            this.btnCreateConn.TabIndex = 1;
            this.btnCreateConn.Text = "Создать";
            this.btnCreateConn.UseVisualStyleBackColor = true;
            // 
            // cbConnection
            // 
            this.cbConnection.FormattingEnabled = true;
            this.cbConnection.Location = new System.Drawing.Point(13, 20);
            this.cbConnection.Name = "cbConnection";
            this.cbConnection.Size = new System.Drawing.Size(200, 21);
            this.cbConnection.TabIndex = 0;
            // 
            // rbSaveToDir
            // 
            this.rbSaveToDir.AutoSize = true;
            this.rbSaveToDir.Checked = true;
            this.rbSaveToDir.Location = new System.Drawing.Point(13, 19);
            this.rbSaveToDir.Name = "rbSaveToDir";
            this.rbSaveToDir.Size = new System.Drawing.Size(154, 17);
            this.rbSaveToDir.TabIndex = 0;
            this.rbSaveToDir.TabStop = true;
            this.rbSaveToDir.Text = "Сохранить в директорию:";
            this.rbSaveToDir.UseVisualStyleBackColor = true;
            // 
            // txtDestDir
            // 
            this.txtDestDir.Location = new System.Drawing.Point(13, 43);
            this.txtDestDir.Name = "txtDestDir";
            this.txtDestDir.Size = new System.Drawing.Size(417, 20);
            this.txtDestDir.TabIndex = 1;
            this.txtDestDir.Text = "C:\\SCADA\\";
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
            // 
            // rbSaveToArc
            // 
            this.rbSaveToArc.AutoSize = true;
            this.rbSaveToArc.Location = new System.Drawing.Point(13, 69);
            this.rbSaveToArc.Name = "rbSaveToArc";
            this.rbSaveToArc.Size = new System.Drawing.Size(122, 17);
            this.rbSaveToArc.TabIndex = 3;
            this.rbSaveToArc.TabStop = true;
            this.rbSaveToArc.Text = "Сохранить в архив:";
            this.rbSaveToArc.UseVisualStyleBackColor = true;
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
            // 
            // txtDestFile
            // 
            this.txtDestFile.Location = new System.Drawing.Point(13, 92);
            this.txtDestFile.Name = "txtDestFile";
            this.txtDestFile.Size = new System.Drawing.Size(417, 20);
            this.txtDestFile.TabIndex = 4;
            // 
            // btnDownload
            // 
            this.btnDownload.Location = new System.Drawing.Point(325, 204);
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
            this.btnClose.Location = new System.Drawing.Point(406, 204);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Закрыть";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // gbOptions
            // 
            this.gbOptions.Controls.Add(this.rbSaveToDir);
            this.gbOptions.Controls.Add(this.txtDestDir);
            this.gbOptions.Controls.Add(this.btnBrowseDestDir);
            this.gbOptions.Controls.Add(this.btnSelectDestFile);
            this.gbOptions.Controls.Add(this.rbSaveToArc);
            this.gbOptions.Controls.Add(this.txtDestFile);
            this.gbOptions.Location = new System.Drawing.Point(12, 73);
            this.gbOptions.Name = "gbOptions";
            this.gbOptions.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbOptions.Size = new System.Drawing.Size(469, 125);
            this.gbOptions.TabIndex = 1;
            this.gbOptions.TabStop = false;
            this.gbOptions.Text = "Параметры скачивания";
            // 
            // FrmDownloadConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(493, 239);
            this.Controls.Add(this.gbOptions);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnDownload);
            this.Controls.Add(this.gbConnection);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmDownloadConfig";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Скачивание конфигурации";
            this.Load += new System.EventHandler(this.FrmDownloadConfig_Load);
            this.gbConnection.ResumeLayout(false);
            this.gbOptions.ResumeLayout(false);
            this.gbOptions.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbConnection;
        private System.Windows.Forms.Button btnRemoveConn;
        private System.Windows.Forms.Button btnEditConn;
        private System.Windows.Forms.Button btnCreateConn;
        private System.Windows.Forms.ComboBox cbConnection;
        private System.Windows.Forms.RadioButton rbSaveToDir;
        private System.Windows.Forms.TextBox txtDestDir;
        private System.Windows.Forms.Button btnBrowseDestDir;
        private System.Windows.Forms.RadioButton rbSaveToArc;
        private System.Windows.Forms.Button btnSelectDestFile;
        private System.Windows.Forms.TextBox txtDestFile;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.GroupBox gbOptions;
    }
}