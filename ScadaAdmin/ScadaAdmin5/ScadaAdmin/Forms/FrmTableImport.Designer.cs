namespace Scada.Admin.App.Forms
{
    partial class FrmTableImport
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
            this.lblTable = new System.Windows.Forms.Label();
            this.cbTable = new System.Windows.Forms.ComboBox();
            this.gbSrcIDs = new System.Windows.Forms.GroupBox();
            this.chkSrcEndID = new System.Windows.Forms.CheckBox();
            this.chkSrcStartID = new System.Windows.Forms.CheckBox();
            this.numSrcEndID = new System.Windows.Forms.NumericUpDown();
            this.lblSrcEndID = new System.Windows.Forms.Label();
            this.numSrcStartID = new System.Windows.Forms.NumericUpDown();
            this.lblSrcStartID = new System.Windows.Forms.Label();
            this.lblSrcFile = new System.Windows.Forms.Label();
            this.btnImport = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.txtSrcFile = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.gbDestIDs = new System.Windows.Forms.GroupBox();
            this.chkDestStartID = new System.Windows.Forms.CheckBox();
            this.numDestEndID = new System.Windows.Forms.NumericUpDown();
            this.lblDestEndID = new System.Windows.Forms.Label();
            this.numDestStartID = new System.Windows.Forms.NumericUpDown();
            this.lblDestStartID = new System.Windows.Forms.Label();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.gbSrcIDs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSrcEndID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSrcStartID)).BeginInit();
            this.gbDestIDs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDestEndID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDestStartID)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTable
            // 
            this.lblTable.AutoSize = true;
            this.lblTable.Location = new System.Drawing.Point(9, 9);
            this.lblTable.Name = "lblTable";
            this.lblTable.Size = new System.Drawing.Size(34, 13);
            this.lblTable.TabIndex = 0;
            this.lblTable.Text = "Table";
            // 
            // cbTable
            // 
            this.cbTable.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTable.FormattingEnabled = true;
            this.cbTable.Location = new System.Drawing.Point(12, 25);
            this.cbTable.Name = "cbTable";
            this.cbTable.Size = new System.Drawing.Size(310, 21);
            this.cbTable.TabIndex = 1;
            // 
            // gbSrcIDs
            // 
            this.gbSrcIDs.Controls.Add(this.chkSrcEndID);
            this.gbSrcIDs.Controls.Add(this.chkSrcStartID);
            this.gbSrcIDs.Controls.Add(this.numSrcEndID);
            this.gbSrcIDs.Controls.Add(this.lblSrcEndID);
            this.gbSrcIDs.Controls.Add(this.numSrcStartID);
            this.gbSrcIDs.Controls.Add(this.lblSrcStartID);
            this.gbSrcIDs.Location = new System.Drawing.Point(12, 93);
            this.gbSrcIDs.Name = "gbSrcIDs";
            this.gbSrcIDs.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbSrcIDs.Size = new System.Drawing.Size(310, 65);
            this.gbSrcIDs.TabIndex = 5;
            this.gbSrcIDs.TabStop = false;
            this.gbSrcIDs.Text = "Source IDs";
            // 
            // chkSrcEndID
            // 
            this.chkSrcEndID.AutoSize = true;
            this.chkSrcEndID.Location = new System.Drawing.Point(158, 35);
            this.chkSrcEndID.Name = "chkSrcEndID";
            this.chkSrcEndID.Size = new System.Drawing.Size(15, 14);
            this.chkSrcEndID.TabIndex = 4;
            this.chkSrcEndID.UseVisualStyleBackColor = true;
            this.chkSrcEndID.CheckedChanged += new System.EventHandler(this.chkSrcEndID_CheckedChanged);
            // 
            // chkSrcStartID
            // 
            this.chkSrcStartID.AutoSize = true;
            this.chkSrcStartID.Location = new System.Drawing.Point(13, 35);
            this.chkSrcStartID.Name = "chkSrcStartID";
            this.chkSrcStartID.Size = new System.Drawing.Size(15, 14);
            this.chkSrcStartID.TabIndex = 1;
            this.chkSrcStartID.UseVisualStyleBackColor = true;
            this.chkSrcStartID.CheckedChanged += new System.EventHandler(this.chkSrcStartID_CheckedChanged);
            // 
            // numSrcEndID
            // 
            this.numSrcEndID.Enabled = false;
            this.numSrcEndID.Location = new System.Drawing.Point(179, 32);
            this.numSrcEndID.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.numSrcEndID.Name = "numSrcEndID";
            this.numSrcEndID.Size = new System.Drawing.Size(118, 20);
            this.numSrcEndID.TabIndex = 5;
            this.numSrcEndID.ValueChanged += new System.EventHandler(this.num_ValueChanged);
            // 
            // lblSrcEndID
            // 
            this.lblSrcEndID.AutoSize = true;
            this.lblSrcEndID.Location = new System.Drawing.Point(176, 16);
            this.lblSrcEndID.Name = "lblSrcEndID";
            this.lblSrcEndID.Size = new System.Drawing.Size(26, 13);
            this.lblSrcEndID.TabIndex = 3;
            this.lblSrcEndID.Text = "End";
            // 
            // numSrcStartID
            // 
            this.numSrcStartID.Enabled = false;
            this.numSrcStartID.Location = new System.Drawing.Point(34, 32);
            this.numSrcStartID.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.numSrcStartID.Name = "numSrcStartID";
            this.numSrcStartID.Size = new System.Drawing.Size(118, 20);
            this.numSrcStartID.TabIndex = 2;
            this.numSrcStartID.ValueChanged += new System.EventHandler(this.num_ValueChanged);
            // 
            // lblSrcStartID
            // 
            this.lblSrcStartID.AutoSize = true;
            this.lblSrcStartID.Location = new System.Drawing.Point(31, 16);
            this.lblSrcStartID.Name = "lblSrcStartID";
            this.lblSrcStartID.Size = new System.Drawing.Size(29, 13);
            this.lblSrcStartID.TabIndex = 0;
            this.lblSrcStartID.Text = "Start";
            // 
            // lblSrcFile
            // 
            this.lblSrcFile.AutoSize = true;
            this.lblSrcFile.Location = new System.Drawing.Point(9, 49);
            this.lblSrcFile.Name = "lblSrcFile";
            this.lblSrcFile.Size = new System.Drawing.Size(57, 13);
            this.lblSrcFile.TabIndex = 2;
            this.lblSrcFile.Text = "Source file";
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(166, 235);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(75, 23);
            this.btnImport.TabIndex = 7;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(247, 235);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 8;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // txtSrcFile
            // 
            this.txtSrcFile.Location = new System.Drawing.Point(12, 65);
            this.txtSrcFile.Name = "txtSrcFile";
            this.txtSrcFile.Size = new System.Drawing.Size(229, 20);
            this.txtSrcFile.TabIndex = 3;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(247, 64);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 4;
            this.btnBrowse.Text = "Browse...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // gbDestIDs
            // 
            this.gbDestIDs.Controls.Add(this.chkDestStartID);
            this.gbDestIDs.Controls.Add(this.numDestEndID);
            this.gbDestIDs.Controls.Add(this.lblDestEndID);
            this.gbDestIDs.Controls.Add(this.numDestStartID);
            this.gbDestIDs.Controls.Add(this.lblDestStartID);
            this.gbDestIDs.Location = new System.Drawing.Point(12, 164);
            this.gbDestIDs.Name = "gbDestIDs";
            this.gbDestIDs.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbDestIDs.Size = new System.Drawing.Size(310, 65);
            this.gbDestIDs.TabIndex = 6;
            this.gbDestIDs.TabStop = false;
            this.gbDestIDs.Text = "Destination IDs";
            // 
            // chkDestStartID
            // 
            this.chkDestStartID.AutoSize = true;
            this.chkDestStartID.Location = new System.Drawing.Point(13, 35);
            this.chkDestStartID.Name = "chkDestStartID";
            this.chkDestStartID.Size = new System.Drawing.Size(15, 14);
            this.chkDestStartID.TabIndex = 1;
            this.chkDestStartID.UseVisualStyleBackColor = true;
            this.chkDestStartID.CheckedChanged += new System.EventHandler(this.chkDestStartID_CheckedChanged);
            // 
            // numDestEndID
            // 
            this.numDestEndID.Increment = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numDestEndID.Location = new System.Drawing.Point(179, 32);
            this.numDestEndID.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.numDestEndID.Name = "numDestEndID";
            this.numDestEndID.ReadOnly = true;
            this.numDestEndID.Size = new System.Drawing.Size(118, 20);
            this.numDestEndID.TabIndex = 4;
            // 
            // lblDestEndID
            // 
            this.lblDestEndID.AutoSize = true;
            this.lblDestEndID.Location = new System.Drawing.Point(176, 16);
            this.lblDestEndID.Name = "lblDestEndID";
            this.lblDestEndID.Size = new System.Drawing.Size(26, 13);
            this.lblDestEndID.TabIndex = 3;
            this.lblDestEndID.Text = "End";
            // 
            // numDestStartID
            // 
            this.numDestStartID.Enabled = false;
            this.numDestStartID.Location = new System.Drawing.Point(34, 32);
            this.numDestStartID.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.numDestStartID.Name = "numDestStartID";
            this.numDestStartID.Size = new System.Drawing.Size(118, 20);
            this.numDestStartID.TabIndex = 2;
            this.numDestStartID.ValueChanged += new System.EventHandler(this.num_ValueChanged);
            // 
            // lblDestStartID
            // 
            this.lblDestStartID.AutoSize = true;
            this.lblDestStartID.Location = new System.Drawing.Point(31, 16);
            this.lblDestStartID.Name = "lblDestStartID";
            this.lblDestStartID.Size = new System.Drawing.Size(29, 13);
            this.lblDestStartID.TabIndex = 0;
            this.lblDestStartID.Text = "Start";
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "Tables (*.dat;*.xml)|*.dat;*.xml|All Files (*.*)|*.*";
            // 
            // FrmTableImport
            // 
            this.AcceptButton = this.btnImport;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(334, 270);
            this.Controls.Add(this.gbDestIDs);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.txtSrcFile);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.lblSrcFile);
            this.Controls.Add(this.gbSrcIDs);
            this.Controls.Add(this.cbTable);
            this.Controls.Add(this.lblTable);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmTableImport";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Import Table";
            this.Load += new System.EventHandler(this.FrmTableImport_Load);
            this.gbSrcIDs.ResumeLayout(false);
            this.gbSrcIDs.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSrcEndID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSrcStartID)).EndInit();
            this.gbDestIDs.ResumeLayout(false);
            this.gbDestIDs.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDestEndID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDestStartID)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTable;
        private System.Windows.Forms.ComboBox cbTable;
        private System.Windows.Forms.GroupBox gbSrcIDs;
        private System.Windows.Forms.Label lblSrcFile;
        private System.Windows.Forms.CheckBox chkSrcEndID;
        private System.Windows.Forms.CheckBox chkSrcStartID;
        private System.Windows.Forms.NumericUpDown numSrcEndID;
        private System.Windows.Forms.Label lblSrcEndID;
        private System.Windows.Forms.NumericUpDown numSrcStartID;
        private System.Windows.Forms.Label lblSrcStartID;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TextBox txtSrcFile;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.GroupBox gbDestIDs;
        private System.Windows.Forms.CheckBox chkDestStartID;
        private System.Windows.Forms.NumericUpDown numDestEndID;
        private System.Windows.Forms.Label lblDestEndID;
        private System.Windows.Forms.NumericUpDown numDestStartID;
        private System.Windows.Forms.Label lblDestStartID;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
    }
}