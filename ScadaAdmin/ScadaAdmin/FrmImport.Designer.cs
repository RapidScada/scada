namespace ScadaAdmin
{
    partial class FrmImport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmImport));
            this.btnImport = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.lblFileName = new System.Windows.Forms.Label();
            this.cbTable = new System.Windows.Forms.ComboBox();
            this.lblTable = new System.Windows.Forms.Label();
            this.gbIDs = new System.Windows.Forms.GroupBox();
            this.chkNewStartID = new System.Windows.Forms.CheckBox();
            this.chkFinalID = new System.Windows.Forms.CheckBox();
            this.chkStartID = new System.Windows.Forms.CheckBox();
            this.lblNewStartID = new System.Windows.Forms.Label();
            this.numNewStartID = new System.Windows.Forms.NumericUpDown();
            this.numFinalID = new System.Windows.Forms.NumericUpDown();
            this.numStartID = new System.Windows.Forms.NumericUpDown();
            this.lblFinalID = new System.Windows.Forms.Label();
            this.lblStartID = new System.Windows.Forms.Label();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.lblDirectory = new System.Windows.Forms.Label();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.gbIDs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numNewStartID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFinalID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStartID)).BeginInit();
            this.SuspendLayout();
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(152, 162);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(75, 23);
            this.btnImport.TabIndex = 7;
            this.btnImport.Text = "Импорт";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(233, 162);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 8;
            this.btnClose.Text = "Закрыть";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // btnBrowse
            // 
            this.btnBrowse.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnBrowse.Image = ((System.Drawing.Image)(resources.GetObject("btnBrowse.Image")));
            this.btnBrowse.Location = new System.Drawing.Point(288, 65);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(20, 20);
            this.btnBrowse.TabIndex = 4;
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // txtFileName
            // 
            this.txtFileName.Location = new System.Drawing.Point(12, 65);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Size = new System.Drawing.Size(270, 20);
            this.txtFileName.TabIndex = 3;
            // 
            // lblFileName
            // 
            this.lblFileName.AutoSize = true;
            this.lblFileName.Location = new System.Drawing.Point(9, 49);
            this.lblFileName.Name = "lblFileName";
            this.lblFileName.Size = new System.Drawing.Size(36, 13);
            this.lblFileName.TabIndex = 2;
            this.lblFileName.Text = "Файл";
            // 
            // cbTable
            // 
            this.cbTable.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTable.FormattingEnabled = true;
            this.cbTable.Location = new System.Drawing.Point(12, 25);
            this.cbTable.Name = "cbTable";
            this.cbTable.Size = new System.Drawing.Size(296, 21);
            this.cbTable.TabIndex = 1;
            this.cbTable.SelectedIndexChanged += new System.EventHandler(this.cbTable_SelectedIndexChanged);
            // 
            // lblTable
            // 
            this.lblTable.AutoSize = true;
            this.lblTable.Location = new System.Drawing.Point(9, 9);
            this.lblTable.Name = "lblTable";
            this.lblTable.Size = new System.Drawing.Size(103, 13);
            this.lblTable.TabIndex = 0;
            this.lblTable.Text = "Таблица или архив";
            // 
            // gbIDs
            // 
            this.gbIDs.Controls.Add(this.chkNewStartID);
            this.gbIDs.Controls.Add(this.chkFinalID);
            this.gbIDs.Controls.Add(this.chkStartID);
            this.gbIDs.Controls.Add(this.lblNewStartID);
            this.gbIDs.Controls.Add(this.numNewStartID);
            this.gbIDs.Controls.Add(this.numFinalID);
            this.gbIDs.Controls.Add(this.numStartID);
            this.gbIDs.Controls.Add(this.lblFinalID);
            this.gbIDs.Controls.Add(this.lblStartID);
            this.gbIDs.Location = new System.Drawing.Point(12, 91);
            this.gbIDs.Name = "gbIDs";
            this.gbIDs.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbIDs.Size = new System.Drawing.Size(296, 65);
            this.gbIDs.TabIndex = 5;
            this.gbIDs.TabStop = false;
            this.gbIDs.Text = "Идентификаторы";
            // 
            // chkNewStartID
            // 
            this.chkNewStartID.AutoSize = true;
            this.chkNewStartID.Location = new System.Drawing.Point(197, 35);
            this.chkNewStartID.Name = "chkNewStartID";
            this.chkNewStartID.Size = new System.Drawing.Size(15, 14);
            this.chkNewStartID.TabIndex = 7;
            this.chkNewStartID.UseVisualStyleBackColor = true;
            this.chkNewStartID.CheckedChanged += new System.EventHandler(this.chkNewStartID_CheckedChanged);
            // 
            // chkFinalID
            // 
            this.chkFinalID.AutoSize = true;
            this.chkFinalID.Location = new System.Drawing.Point(105, 35);
            this.chkFinalID.Name = "chkFinalID";
            this.chkFinalID.Size = new System.Drawing.Size(15, 14);
            this.chkFinalID.TabIndex = 4;
            this.chkFinalID.UseVisualStyleBackColor = true;
            this.chkFinalID.CheckedChanged += new System.EventHandler(this.chkFinalID_CheckedChanged);
            // 
            // chkStartID
            // 
            this.chkStartID.AutoSize = true;
            this.chkStartID.Location = new System.Drawing.Point(13, 35);
            this.chkStartID.Name = "chkStartID";
            this.chkStartID.Size = new System.Drawing.Size(15, 14);
            this.chkStartID.TabIndex = 1;
            this.chkStartID.UseVisualStyleBackColor = true;
            this.chkStartID.CheckedChanged += new System.EventHandler(this.chkStartID_CheckedChanged);
            // 
            // lblNewStartID
            // 
            this.lblNewStartID.AutoSize = true;
            this.lblNewStartID.Location = new System.Drawing.Point(215, 16);
            this.lblNewStartID.Name = "lblNewStartID";
            this.lblNewStartID.Size = new System.Drawing.Size(64, 13);
            this.lblNewStartID.TabIndex = 6;
            this.lblNewStartID.Text = "Новый нач.";
            // 
            // numNewStartID
            // 
            this.numNewStartID.Enabled = false;
            this.numNewStartID.Location = new System.Drawing.Point(218, 32);
            this.numNewStartID.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numNewStartID.Name = "numNewStartID";
            this.numNewStartID.Size = new System.Drawing.Size(65, 20);
            this.numNewStartID.TabIndex = 8;
            // 
            // numFinalID
            // 
            this.numFinalID.Enabled = false;
            this.numFinalID.Location = new System.Drawing.Point(126, 32);
            this.numFinalID.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numFinalID.Name = "numFinalID";
            this.numFinalID.Size = new System.Drawing.Size(65, 20);
            this.numFinalID.TabIndex = 5;
            // 
            // numStartID
            // 
            this.numStartID.Enabled = false;
            this.numStartID.Location = new System.Drawing.Point(34, 32);
            this.numStartID.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numStartID.Name = "numStartID";
            this.numStartID.Size = new System.Drawing.Size(65, 20);
            this.numStartID.TabIndex = 2;
            // 
            // lblFinalID
            // 
            this.lblFinalID.AutoSize = true;
            this.lblFinalID.Location = new System.Drawing.Point(123, 16);
            this.lblFinalID.Name = "lblFinalID";
            this.lblFinalID.Size = new System.Drawing.Size(57, 13);
            this.lblFinalID.TabIndex = 3;
            this.lblFinalID.Text = "Конечный";
            // 
            // lblStartID
            // 
            this.lblStartID.AutoSize = true;
            this.lblStartID.Location = new System.Drawing.Point(31, 16);
            this.lblStartID.Name = "lblStartID";
            this.lblStartID.Size = new System.Drawing.Size(64, 13);
            this.lblStartID.TabIndex = 0;
            this.lblStartID.Text = "Начальный";
            // 
            // openFileDialog
            // 
            this.openFileDialog.CheckFileExists = false;
            this.openFileDialog.DefaultExt = "*.dat";
            this.openFileDialog.Filter = "Таблицы базы конфигурации|*.dat|Все файлы|*.*";
            this.openFileDialog.Title = "Выберите файл таблицы базы конфигурации";
            // 
            // lblDirectory
            // 
            this.lblDirectory.AutoSize = true;
            this.lblDirectory.Location = new System.Drawing.Point(51, 49);
            this.lblDirectory.Name = "lblDirectory";
            this.lblDirectory.Size = new System.Drawing.Size(69, 13);
            this.lblDirectory.TabIndex = 9;
            this.lblDirectory.Text = "Директория";
            // 
            // FrmImport
            // 
            this.AcceptButton = this.btnImport;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(320, 197);
            this.Controls.Add(this.lblDirectory);
            this.Controls.Add(this.gbIDs);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.txtFileName);
            this.Controls.Add(this.lblFileName);
            this.Controls.Add(this.cbTable);
            this.Controls.Add(this.lblTable);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmImport";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Импорт из файла";
            this.Load += new System.EventHandler(this.FrmImport_Load);
            this.gbIDs.ResumeLayout(false);
            this.gbIDs.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numNewStartID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFinalID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStartID)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox txtFileName;
        private System.Windows.Forms.Label lblFileName;
        private System.Windows.Forms.ComboBox cbTable;
        private System.Windows.Forms.Label lblTable;
        private System.Windows.Forms.GroupBox gbIDs;
        private System.Windows.Forms.NumericUpDown numNewStartID;
        private System.Windows.Forms.NumericUpDown numFinalID;
        private System.Windows.Forms.NumericUpDown numStartID;
        private System.Windows.Forms.Label lblFinalID;
        private System.Windows.Forms.Label lblStartID;
        private System.Windows.Forms.Label lblNewStartID;
        private System.Windows.Forms.CheckBox chkStartID;
        private System.Windows.Forms.CheckBox chkNewStartID;
        private System.Windows.Forms.CheckBox chkFinalID;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Label lblDirectory;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
    }
}