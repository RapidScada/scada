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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmUploadConfig));
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("BaseDAT\\");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("C:\\SCADA\\", new System.Windows.Forms.TreeNode[] {
            treeNode1});
            this.gbOptions = new System.Windows.Forms.GroupBox();
            this.chkClearSpecificFiles = new System.Windows.Forms.CheckBox();
            this.btnSelectSrcFile = new System.Windows.Forms.Button();
            this.txtSrcFile = new System.Windows.Forms.TextBox();
            this.rbGetFromArc = new System.Windows.Forms.RadioButton();
            this.rbGetFromDir = new System.Windows.Forms.RadioButton();
            this.tvFiles = new System.Windows.Forms.TreeView();
            this.lblFiles = new System.Windows.Forms.Label();
            this.txtSrcDir = new System.Windows.Forms.TextBox();
            this.btnBrowseSrcDir = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnUpload = new System.Windows.Forms.Button();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.ilTree = new System.Windows.Forms.ImageList(this.components);
            this.ctrlServerConn = new ScadaAdmin.Remote.CtrlServerConn();
            this.gbOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbOptions
            // 
            this.gbOptions.Controls.Add(this.chkClearSpecificFiles);
            this.gbOptions.Controls.Add(this.btnSelectSrcFile);
            this.gbOptions.Controls.Add(this.txtSrcFile);
            this.gbOptions.Controls.Add(this.rbGetFromArc);
            this.gbOptions.Controls.Add(this.rbGetFromDir);
            this.gbOptions.Controls.Add(this.tvFiles);
            this.gbOptions.Controls.Add(this.lblFiles);
            this.gbOptions.Controls.Add(this.txtSrcDir);
            this.gbOptions.Controls.Add(this.btnBrowseSrcDir);
            this.gbOptions.Location = new System.Drawing.Point(12, 73);
            this.gbOptions.Name = "gbOptions";
            this.gbOptions.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbOptions.Size = new System.Drawing.Size(469, 408);
            this.gbOptions.TabIndex = 1;
            this.gbOptions.TabStop = false;
            this.gbOptions.Text = "Параметры передачи";
            // 
            // chkClearSpecificFiles
            // 
            this.chkClearSpecificFiles.AutoSize = true;
            this.chkClearSpecificFiles.Location = new System.Drawing.Point(13, 378);
            this.chkClearSpecificFiles.Name = "chkClearSpecificFiles";
            this.chkClearSpecificFiles.Size = new System.Drawing.Size(444, 17);
            this.chkClearSpecificFiles.TabIndex = 8;
            this.chkClearSpecificFiles.Text = "Очистить файлы специфичные для сервера, в том числе, регистрационные ключи";
            this.chkClearSpecificFiles.UseVisualStyleBackColor = true;
            this.chkClearSpecificFiles.CheckedChanged += new System.EventHandler(this.uploadControl_Changed);
            // 
            // btnSelectSrcFile
            // 
            this.btnSelectSrcFile.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSelectSrcFile.Image = ((System.Drawing.Image)(resources.GetObject("btnSelectSrcFile.Image")));
            this.btnSelectSrcFile.Location = new System.Drawing.Point(436, 347);
            this.btnSelectSrcFile.Name = "btnSelectSrcFile";
            this.btnSelectSrcFile.Size = new System.Drawing.Size(20, 20);
            this.btnSelectSrcFile.TabIndex = 7;
            this.btnSelectSrcFile.UseVisualStyleBackColor = true;
            this.btnSelectSrcFile.Click += new System.EventHandler(this.btnSelectSrcFile_Click);
            // 
            // txtSrcFile
            // 
            this.txtSrcFile.Location = new System.Drawing.Point(13, 347);
            this.txtSrcFile.Name = "txtSrcFile";
            this.txtSrcFile.Size = new System.Drawing.Size(417, 20);
            this.txtSrcFile.TabIndex = 6;
            this.txtSrcFile.TextChanged += new System.EventHandler(this.uploadControl_Changed);
            // 
            // rbGetFromArc
            // 
            this.rbGetFromArc.AutoSize = true;
            this.rbGetFromArc.Location = new System.Drawing.Point(13, 324);
            this.rbGetFromArc.Name = "rbGetFromArc";
            this.rbGetFromArc.Size = new System.Drawing.Size(170, 17);
            this.rbGetFromArc.TabIndex = 5;
            this.rbGetFromArc.Text = "Файл архива конфигурации:";
            this.rbGetFromArc.UseVisualStyleBackColor = true;
            this.rbGetFromArc.CheckedChanged += new System.EventHandler(this.rbGet_CheckedChanged);
            // 
            // rbGetFromDir
            // 
            this.rbGetFromDir.AutoSize = true;
            this.rbGetFromDir.Location = new System.Drawing.Point(13, 19);
            this.rbGetFromDir.Name = "rbGetFromDir";
            this.rbGetFromDir.Size = new System.Drawing.Size(165, 17);
            this.rbGetFromDir.TabIndex = 0;
            this.rbGetFromDir.Text = "Директория конфигурации:";
            this.rbGetFromDir.UseVisualStyleBackColor = true;
            this.rbGetFromDir.CheckedChanged += new System.EventHandler(this.rbGet_CheckedChanged);
            // 
            // tvFiles
            // 
            this.tvFiles.CheckBoxes = true;
            this.tvFiles.ImageIndex = 0;
            this.tvFiles.ImageList = this.ilTree;
            this.tvFiles.Location = new System.Drawing.Point(13, 81);
            this.tvFiles.Name = "tvFiles";
            treeNode1.Name = "Node1";
            treeNode1.Text = "BaseDAT\\";
            treeNode2.Name = "Node0";
            treeNode2.Text = "C:\\SCADA\\";
            this.tvFiles.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode2});
            this.tvFiles.SelectedImageIndex = 0;
            this.tvFiles.Size = new System.Drawing.Size(443, 237);
            this.tvFiles.TabIndex = 4;
            this.tvFiles.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.tvFiles_AfterCheck);
            this.tvFiles.BeforeCollapse += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvFiles_BeforeCollapse);
            this.tvFiles.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvFiles_BeforeExpand);
            // 
            // lblFiles
            // 
            this.lblFiles.AutoSize = true;
            this.lblFiles.Location = new System.Drawing.Point(10, 65);
            this.lblFiles.Name = "lblFiles";
            this.lblFiles.Size = new System.Drawing.Size(115, 13);
            this.lblFiles.TabIndex = 3;
            this.lblFiles.Text = "Файлы для передачи";
            // 
            // txtSrcDir
            // 
            this.txtSrcDir.Location = new System.Drawing.Point(13, 42);
            this.txtSrcDir.Name = "txtSrcDir";
            this.txtSrcDir.ReadOnly = true;
            this.txtSrcDir.Size = new System.Drawing.Size(417, 20);
            this.txtSrcDir.TabIndex = 1;
            this.txtSrcDir.Text = "C:\\SCADA\\";
            this.txtSrcDir.TextChanged += new System.EventHandler(this.uploadControl_Changed);
            // 
            // btnBrowseSrcDir
            // 
            this.btnBrowseSrcDir.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnBrowseSrcDir.Image = ((System.Drawing.Image)(resources.GetObject("btnBrowseSrcDir.Image")));
            this.btnBrowseSrcDir.Location = new System.Drawing.Point(436, 42);
            this.btnBrowseSrcDir.Name = "btnBrowseSrcDir";
            this.btnBrowseSrcDir.Size = new System.Drawing.Size(20, 20);
            this.btnBrowseSrcDir.TabIndex = 2;
            this.btnBrowseSrcDir.UseVisualStyleBackColor = true;
            this.btnBrowseSrcDir.Click += new System.EventHandler(this.btnBrowseSrcDir_Click);
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(406, 487);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Закрыть";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // btnUpload
            // 
            this.btnUpload.Location = new System.Drawing.Point(325, 487);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(75, 23);
            this.btnUpload.TabIndex = 2;
            this.btnUpload.Text = "Передать";
            this.btnUpload.UseVisualStyleBackColor = true;
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // folderBrowserDialog
            // 
            this.folderBrowserDialog.Description = "Выберите директорию конфигурации";
            // 
            // openFileDialog
            // 
            this.openFileDialog.CheckFileExists = false;
            this.openFileDialog.DefaultExt = "*.zip";
            this.openFileDialog.Filter = "Архивы конфигурации (*.zip)|*.zip|Все файлы (*.*)|*.*";
            this.openFileDialog.Title = "Выберите файл архива конфигурации";
            // 
            // ilTree
            // 
            this.ilTree.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilTree.ImageStream")));
            this.ilTree.TransparentColor = System.Drawing.Color.Transparent;
            this.ilTree.Images.SetKeyName(0, "folder_closed2.png");
            this.ilTree.Images.SetKeyName(1, "folder_open2.png");
            this.ilTree.Images.SetKeyName(2, "file.png");
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
            // FrmUploadConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(493, 522);
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
        private System.Windows.Forms.TextBox txtSrcDir;
        private System.Windows.Forms.Button btnBrowseSrcDir;
        private System.Windows.Forms.Label lblFiles;
        private System.Windows.Forms.TreeView tvFiles;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.RadioButton rbGetFromDir;
        private System.Windows.Forms.RadioButton rbGetFromArc;
        private System.Windows.Forms.Button btnSelectSrcFile;
        private System.Windows.Forms.TextBox txtSrcFile;
        private System.Windows.Forms.CheckBox chkClearSpecificFiles;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.ImageList ilTree;
    }
}