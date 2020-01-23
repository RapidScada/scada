namespace Scada.Admin.App.Forms.Tools
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
            this.gbPathOptions = new System.Windows.Forms.GroupBox();
            this.btnBrowseCommDir = new System.Windows.Forms.Button();
            this.txtCommDir = new System.Windows.Forms.TextBox();
            this.lblCommDir = new System.Windows.Forms.Label();
            this.btnBrowseServerDir = new System.Windows.Forms.Button();
            this.txtServerDir = new System.Windows.Forms.TextBox();
            this.lblServerDir = new System.Windows.Forms.Label();
            this.gbChannelOptions = new System.Windows.Forms.GroupBox();
            this.chkPrependDeviceName = new System.Windows.Forms.CheckBox();
            this.numCnlGap = new System.Windows.Forms.NumericUpDown();
            this.lblCnlGap = new System.Windows.Forms.Label();
            this.numCnlShift = new System.Windows.Forms.NumericUpDown();
            this.lblCnlShift = new System.Windows.Forms.Label();
            this.lblExplanation = new System.Windows.Forms.Label();
            this.numCnlMult = new System.Windows.Forms.NumericUpDown();
            this.lblCnlMult = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.lvAssociations = new System.Windows.Forms.ListView();
            this.colExt = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colPath = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnAddAssociation = new System.Windows.Forms.Button();
            this.btnEditAssociation = new System.Windows.Forms.Button();
            this.btnDeleteAssociation = new System.Windows.Forms.Button();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabGeneral = new System.Windows.Forms.TabPage();
            this.tabFileAssociations = new System.Windows.Forms.TabPage();
            this.gbPathOptions.SuspendLayout();
            this.gbChannelOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCnlGap)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCnlShift)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCnlMult)).BeginInit();
            this.tabControl.SuspendLayout();
            this.tabGeneral.SuspendLayout();
            this.tabFileAssociations.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbPathOptions
            // 
            this.gbPathOptions.Controls.Add(this.btnBrowseCommDir);
            this.gbPathOptions.Controls.Add(this.txtCommDir);
            this.gbPathOptions.Controls.Add(this.lblCommDir);
            this.gbPathOptions.Controls.Add(this.btnBrowseServerDir);
            this.gbPathOptions.Controls.Add(this.txtServerDir);
            this.gbPathOptions.Controls.Add(this.lblServerDir);
            this.gbPathOptions.Location = new System.Drawing.Point(6, 6);
            this.gbPathOptions.Name = "gbPathOptions";
            this.gbPathOptions.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbPathOptions.Size = new System.Drawing.Size(410, 106);
            this.gbPathOptions.TabIndex = 0;
            this.gbPathOptions.TabStop = false;
            this.gbPathOptions.Text = "Paths";
            // 
            // btnBrowseCommDir
            // 
            this.btnBrowseCommDir.Location = new System.Drawing.Point(322, 70);
            this.btnBrowseCommDir.Name = "btnBrowseCommDir";
            this.btnBrowseCommDir.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseCommDir.TabIndex = 5;
            this.btnBrowseCommDir.Text = "Browse...";
            this.btnBrowseCommDir.UseVisualStyleBackColor = true;
            this.btnBrowseCommDir.Click += new System.EventHandler(this.btnBrowseCommDir_Click);
            // 
            // txtCommDir
            // 
            this.txtCommDir.Location = new System.Drawing.Point(13, 71);
            this.txtCommDir.Name = "txtCommDir";
            this.txtCommDir.Size = new System.Drawing.Size(303, 20);
            this.txtCommDir.TabIndex = 4;
            this.txtCommDir.TextChanged += new System.EventHandler(this.control_Changed);
            // 
            // lblCommDir
            // 
            this.lblCommDir.AutoSize = true;
            this.lblCommDir.Location = new System.Drawing.Point(10, 55);
            this.lblCommDir.Name = "lblCommDir";
            this.lblCommDir.Size = new System.Drawing.Size(117, 13);
            this.lblCommDir.TabIndex = 3;
            this.lblCommDir.Text = "Communicator directory";
            // 
            // btnBrowseServerDir
            // 
            this.btnBrowseServerDir.Location = new System.Drawing.Point(322, 31);
            this.btnBrowseServerDir.Name = "btnBrowseServerDir";
            this.btnBrowseServerDir.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseServerDir.TabIndex = 2;
            this.btnBrowseServerDir.Text = "Browse...";
            this.btnBrowseServerDir.UseVisualStyleBackColor = true;
            this.btnBrowseServerDir.Click += new System.EventHandler(this.btnBrowseServerDir_Click);
            // 
            // txtServerDir
            // 
            this.txtServerDir.Location = new System.Drawing.Point(13, 32);
            this.txtServerDir.Name = "txtServerDir";
            this.txtServerDir.Size = new System.Drawing.Size(303, 20);
            this.txtServerDir.TabIndex = 1;
            this.txtServerDir.TextChanged += new System.EventHandler(this.control_Changed);
            // 
            // lblServerDir
            // 
            this.lblServerDir.AutoSize = true;
            this.lblServerDir.Location = new System.Drawing.Point(10, 16);
            this.lblServerDir.Name = "lblServerDir";
            this.lblServerDir.Size = new System.Drawing.Size(81, 13);
            this.lblServerDir.TabIndex = 0;
            this.lblServerDir.Text = "Server directory";
            // 
            // gbChannelOptions
            // 
            this.gbChannelOptions.Controls.Add(this.chkPrependDeviceName);
            this.gbChannelOptions.Controls.Add(this.numCnlGap);
            this.gbChannelOptions.Controls.Add(this.lblCnlGap);
            this.gbChannelOptions.Controls.Add(this.numCnlShift);
            this.gbChannelOptions.Controls.Add(this.lblCnlShift);
            this.gbChannelOptions.Controls.Add(this.lblExplanation);
            this.gbChannelOptions.Controls.Add(this.numCnlMult);
            this.gbChannelOptions.Controls.Add(this.lblCnlMult);
            this.gbChannelOptions.Location = new System.Drawing.Point(6, 118);
            this.gbChannelOptions.Name = "gbChannelOptions";
            this.gbChannelOptions.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbChannelOptions.Size = new System.Drawing.Size(410, 104);
            this.gbChannelOptions.TabIndex = 1;
            this.gbChannelOptions.TabStop = false;
            this.gbChannelOptions.Text = "Channel Generation";
            // 
            // chkPrependDeviceName
            // 
            this.chkPrependDeviceName.AutoSize = true;
            this.chkPrependDeviceName.Location = new System.Drawing.Point(158, 73);
            this.chkPrependDeviceName.Name = "chkPrependDeviceName";
            this.chkPrependDeviceName.Size = new System.Drawing.Size(130, 17);
            this.chkPrependDeviceName.TabIndex = 7;
            this.chkPrependDeviceName.Text = "Prepend device name";
            this.chkPrependDeviceName.UseVisualStyleBackColor = true;
            this.chkPrependDeviceName.CheckedChanged += new System.EventHandler(this.control_Changed);
            // 
            // numCnlGap
            // 
            this.numCnlGap.Location = new System.Drawing.Point(13, 71);
            this.numCnlGap.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numCnlGap.Name = "numCnlGap";
            this.numCnlGap.Size = new System.Drawing.Size(100, 20);
            this.numCnlGap.TabIndex = 6;
            this.numCnlGap.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numCnlGap.ValueChanged += new System.EventHandler(this.control_Changed);
            // 
            // lblCnlGap
            // 
            this.lblCnlGap.AutoSize = true;
            this.lblCnlGap.Location = new System.Drawing.Point(10, 55);
            this.lblCnlGap.Name = "lblCnlGap";
            this.lblCnlGap.Size = new System.Drawing.Size(27, 13);
            this.lblCnlGap.TabIndex = 5;
            this.lblCnlGap.Text = "Gap";
            // 
            // numCnlShift
            // 
            this.numCnlShift.Location = new System.Drawing.Point(158, 32);
            this.numCnlShift.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numCnlShift.Name = "numCnlShift";
            this.numCnlShift.Size = new System.Drawing.Size(100, 20);
            this.numCnlShift.TabIndex = 4;
            this.numCnlShift.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numCnlShift.ValueChanged += new System.EventHandler(this.control_Changed);
            // 
            // lblCnlShift
            // 
            this.lblCnlShift.AutoSize = true;
            this.lblCnlShift.Location = new System.Drawing.Point(155, 16);
            this.lblCnlShift.Name = "lblCnlShift";
            this.lblCnlShift.Size = new System.Drawing.Size(28, 13);
            this.lblCnlShift.TabIndex = 3;
            this.lblCnlShift.Text = "Shift";
            // 
            // lblExplanation
            // 
            this.lblExplanation.AutoSize = true;
            this.lblExplanation.Location = new System.Drawing.Point(119, 34);
            this.lblExplanation.Name = "lblExplanation";
            this.lblExplanation.Size = new System.Drawing.Size(33, 13);
            this.lblExplanation.TabIndex = 2;
            this.lblExplanation.Text = "× N +";
            // 
            // numCnlMult
            // 
            this.numCnlMult.Location = new System.Drawing.Point(13, 32);
            this.numCnlMult.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numCnlMult.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numCnlMult.Name = "numCnlMult";
            this.numCnlMult.Size = new System.Drawing.Size(100, 20);
            this.numCnlMult.TabIndex = 1;
            this.numCnlMult.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numCnlMult.ValueChanged += new System.EventHandler(this.control_Changed);
            // 
            // lblCnlMult
            // 
            this.lblCnlMult.AutoSize = true;
            this.lblCnlMult.Location = new System.Drawing.Point(10, 16);
            this.lblCnlMult.Name = "lblCnlMult";
            this.lblCnlMult.Size = new System.Drawing.Size(55, 13);
            this.lblCnlMult.TabIndex = 0;
            this.lblCnlMult.Text = "Multiplicity";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(266, 260);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(347, 260);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // lvAssociations
            // 
            this.lvAssociations.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colExt,
            this.colPath});
            this.lvAssociations.FullRowSelect = true;
            this.lvAssociations.GridLines = true;
            this.lvAssociations.HideSelection = false;
            this.lvAssociations.Location = new System.Drawing.Point(6, 35);
            this.lvAssociations.MultiSelect = false;
            this.lvAssociations.Name = "lvAssociations";
            this.lvAssociations.ShowItemToolTips = true;
            this.lvAssociations.Size = new System.Drawing.Size(414, 187);
            this.lvAssociations.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lvAssociations.TabIndex = 3;
            this.lvAssociations.UseCompatibleStateImageBehavior = false;
            this.lvAssociations.View = System.Windows.Forms.View.Details;
            this.lvAssociations.SelectedIndexChanged += new System.EventHandler(this.lvAssociations_SelectedIndexChanged);
            this.lvAssociations.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvAssociations_MouseDoubleClick);
            // 
            // colExt
            // 
            this.colExt.Text = "File Extenstion";
            this.colExt.Width = 90;
            // 
            // colPath
            // 
            this.colPath.Text = "Excutable Path";
            this.colPath.Width = 300;
            // 
            // btnAddAssociation
            // 
            this.btnAddAssociation.Location = new System.Drawing.Point(6, 6);
            this.btnAddAssociation.Name = "btnAddAssociation";
            this.btnAddAssociation.Size = new System.Drawing.Size(75, 23);
            this.btnAddAssociation.TabIndex = 0;
            this.btnAddAssociation.Text = "Add";
            this.btnAddAssociation.UseVisualStyleBackColor = true;
            this.btnAddAssociation.Click += new System.EventHandler(this.btnAddAssociation_Click);
            // 
            // btnEditAssociation
            // 
            this.btnEditAssociation.Location = new System.Drawing.Point(87, 6);
            this.btnEditAssociation.Name = "btnEditAssociation";
            this.btnEditAssociation.Size = new System.Drawing.Size(75, 23);
            this.btnEditAssociation.TabIndex = 1;
            this.btnEditAssociation.Text = "Edit";
            this.btnEditAssociation.UseVisualStyleBackColor = true;
            this.btnEditAssociation.Click += new System.EventHandler(this.btnEditAssociation_Click);
            // 
            // btnDeleteAssociation
            // 
            this.btnDeleteAssociation.Location = new System.Drawing.Point(168, 6);
            this.btnDeleteAssociation.Name = "btnDeleteAssociation";
            this.btnDeleteAssociation.Size = new System.Drawing.Size(75, 23);
            this.btnDeleteAssociation.TabIndex = 2;
            this.btnDeleteAssociation.Text = "Delete";
            this.btnDeleteAssociation.UseVisualStyleBackColor = true;
            this.btnDeleteAssociation.Click += new System.EventHandler(this.btnDeleteAssociation_Click);
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabGeneral);
            this.tabControl.Controls.Add(this.tabFileAssociations);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(434, 254);
            this.tabControl.TabIndex = 0;
            // 
            // tabGeneral
            // 
            this.tabGeneral.Controls.Add(this.gbPathOptions);
            this.tabGeneral.Controls.Add(this.gbChannelOptions);
            this.tabGeneral.Location = new System.Drawing.Point(4, 22);
            this.tabGeneral.Name = "tabGeneral";
            this.tabGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tabGeneral.Size = new System.Drawing.Size(426, 228);
            this.tabGeneral.TabIndex = 0;
            this.tabGeneral.Text = "General";
            this.tabGeneral.UseVisualStyleBackColor = true;
            // 
            // tabFileAssociations
            // 
            this.tabFileAssociations.Controls.Add(this.lvAssociations);
            this.tabFileAssociations.Controls.Add(this.btnDeleteAssociation);
            this.tabFileAssociations.Controls.Add(this.btnEditAssociation);
            this.tabFileAssociations.Controls.Add(this.btnAddAssociation);
            this.tabFileAssociations.Location = new System.Drawing.Point(4, 22);
            this.tabFileAssociations.Name = "tabFileAssociations";
            this.tabFileAssociations.Padding = new System.Windows.Forms.Padding(3);
            this.tabFileAssociations.Size = new System.Drawing.Size(426, 228);
            this.tabFileAssociations.TabIndex = 1;
            this.tabFileAssociations.Text = "File Associations";
            this.tabFileAssociations.UseVisualStyleBackColor = true;
            // 
            // FrmSettings
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(434, 295);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmSettings";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options";
            this.Load += new System.EventHandler(this.FrmSettings_Load);
            this.gbPathOptions.ResumeLayout(false);
            this.gbPathOptions.PerformLayout();
            this.gbChannelOptions.ResumeLayout(false);
            this.gbChannelOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCnlGap)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCnlShift)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCnlMult)).EndInit();
            this.tabControl.ResumeLayout(false);
            this.tabGeneral.ResumeLayout(false);
            this.tabFileAssociations.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbPathOptions;
        private System.Windows.Forms.Button btnBrowseServerDir;
        private System.Windows.Forms.TextBox txtServerDir;
        private System.Windows.Forms.Label lblServerDir;
        private System.Windows.Forms.Button btnBrowseCommDir;
        private System.Windows.Forms.TextBox txtCommDir;
        private System.Windows.Forms.Label lblCommDir;
        private System.Windows.Forms.GroupBox gbChannelOptions;
        private System.Windows.Forms.NumericUpDown numCnlMult;
        private System.Windows.Forms.Label lblCnlMult;
        private System.Windows.Forms.NumericUpDown numCnlShift;
        private System.Windows.Forms.Label lblExplanation;
        private System.Windows.Forms.Label lblCnlShift;
        private System.Windows.Forms.NumericUpDown numCnlGap;
        private System.Windows.Forms.Label lblCnlGap;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.CheckBox chkPrependDeviceName;
        private System.Windows.Forms.Button btnDeleteAssociation;
        private System.Windows.Forms.Button btnEditAssociation;
        private System.Windows.Forms.Button btnAddAssociation;
        private System.Windows.Forms.ListView lvAssociations;
        private System.Windows.Forms.ColumnHeader colExt;
        private System.Windows.Forms.ColumnHeader colPath;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabGeneral;
        private System.Windows.Forms.TabPage tabFileAssociations;
    }
}