namespace ScadaAdmin.Remote
{
    partial class FrmUploadConfig
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmUploadConfig));
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("BaseDAT\\");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("C:\\SCADA\\", new System.Windows.Forms.TreeNode[] {
            treeNode3});
            this.ctrlServerConn = new ScadaAdmin.Remote.CtrlServerConn();
            this.gbOptions = new System.Windows.Forms.GroupBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnUpload = new System.Windows.Forms.Button();
            this.lblSrcDir = new System.Windows.Forms.Label();
            this.txtSrcDir = new System.Windows.Forms.TextBox();
            this.btnBrowseSrcDir = new System.Windows.Forms.Button();
            this.lblFiles = new System.Windows.Forms.Label();
            this.tvFiles = new System.Windows.Forms.TreeView();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.gbOptions.SuspendLayout();
            this.SuspendLayout();
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
            // gbOptions
            // 
            this.gbOptions.Controls.Add(this.tvFiles);
            this.gbOptions.Controls.Add(this.lblFiles);
            this.gbOptions.Controls.Add(this.txtSrcDir);
            this.gbOptions.Controls.Add(this.btnBrowseSrcDir);
            this.gbOptions.Controls.Add(this.lblSrcDir);
            this.gbOptions.Location = new System.Drawing.Point(12, 73);
            this.gbOptions.Name = "gbOptions";
            this.gbOptions.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbOptions.Size = new System.Drawing.Size(469, 334);
            this.gbOptions.TabIndex = 1;
            this.gbOptions.TabStop = false;
            this.gbOptions.Text = "Параметры передачи";
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(406, 413);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "Закрыть";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // btnUpload
            // 
            this.btnUpload.Location = new System.Drawing.Point(325, 413);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(75, 23);
            this.btnUpload.TabIndex = 4;
            this.btnUpload.Text = "Передать";
            this.btnUpload.UseVisualStyleBackColor = true;
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // lblSrcDir
            // 
            this.lblSrcDir.AutoSize = true;
            this.lblSrcDir.Location = new System.Drawing.Point(10, 16);
            this.lblSrcDir.Name = "lblSrcDir";
            this.lblSrcDir.Size = new System.Drawing.Size(144, 13);
            this.lblSrcDir.TabIndex = 0;
            this.lblSrcDir.Text = "Директория конфигурации";
            // 
            // txtSrcDir
            // 
            this.txtSrcDir.Location = new System.Drawing.Point(13, 32);
            this.txtSrcDir.Name = "txtSrcDir";
            this.txtSrcDir.ReadOnly = true;
            this.txtSrcDir.Size = new System.Drawing.Size(417, 20);
            this.txtSrcDir.TabIndex = 3;
            this.txtSrcDir.Text = "C:\\SCADA\\";
            // 
            // btnBrowseSrcDir
            // 
            this.btnBrowseSrcDir.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnBrowseSrcDir.Image = ((System.Drawing.Image)(resources.GetObject("btnBrowseSrcDir.Image")));
            this.btnBrowseSrcDir.Location = new System.Drawing.Point(436, 32);
            this.btnBrowseSrcDir.Name = "btnBrowseSrcDir";
            this.btnBrowseSrcDir.Size = new System.Drawing.Size(20, 20);
            this.btnBrowseSrcDir.TabIndex = 4;
            this.btnBrowseSrcDir.UseVisualStyleBackColor = true;
            this.btnBrowseSrcDir.Click += new System.EventHandler(this.btnBrowseSrcDir_Click);
            // 
            // lblFiles
            // 
            this.lblFiles.AutoSize = true;
            this.lblFiles.Location = new System.Drawing.Point(10, 55);
            this.lblFiles.Name = "lblFiles";
            this.lblFiles.Size = new System.Drawing.Size(115, 13);
            this.lblFiles.TabIndex = 5;
            this.lblFiles.Text = "Файлы для передачи";
            // 
            // tvFiles
            // 
            this.tvFiles.CheckBoxes = true;
            this.tvFiles.Location = new System.Drawing.Point(13, 71);
            this.tvFiles.Name = "tvFiles";
            treeNode3.Name = "Node1";
            treeNode3.Text = "BaseDAT\\";
            treeNode4.Name = "Node0";
            treeNode4.Text = "C:\\SCADA\\";
            this.tvFiles.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode4});
            this.tvFiles.Size = new System.Drawing.Size(443, 250);
            this.tvFiles.TabIndex = 6;
            // 
            // folderBrowserDialog
            // 
            this.folderBrowserDialog.Description = "Выберите директорию конфигурации";
            // 
            // FrmUploadConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(493, 448);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnUpload);
            this.Controls.Add(this.gbOptions);
            this.Controls.Add(this.ctrlServerConn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmUploadConfig";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Передача конфигурации";
            this.Load += new System.EventHandler(this.FrmUploadConfig_Load);
            this.gbOptions.ResumeLayout(false);
            this.gbOptions.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private CtrlServerConn ctrlServerConn;
        private System.Windows.Forms.GroupBox gbOptions;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnUpload;
        private System.Windows.Forms.Label lblSrcDir;
        private System.Windows.Forms.TextBox txtSrcDir;
        private System.Windows.Forms.Button btnBrowseSrcDir;
        private System.Windows.Forms.Label lblFiles;
        private System.Windows.Forms.TreeView tvFiles;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
    }
}