
namespace Scada.Server.Modules.DbExport.UI
{
    partial class CtrlArcUploadOptions
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.gbArcUploadOptions = new System.Windows.Forms.GroupBox();
            this.lblDelay = new System.Windows.Forms.Label();
            this.lblMaxAge = new System.Windows.Forms.Label();
            this.chkEnabled = new System.Windows.Forms.CheckBox();
            this.numMaxAge = new System.Windows.Forms.NumericUpDown();
            this.numDelay = new System.Windows.Forms.NumericUpDown();
            this.cbSnapshotType = new System.Windows.Forms.ComboBox();
            this.lblSnapshotType = new System.Windows.Forms.Label();
            this.gbArcUploadOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxAge)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDelay)).BeginInit();
            this.SuspendLayout();
            // 
            // gbArcUploadOptions
            // 
            this.gbArcUploadOptions.Controls.Add(this.lblDelay);
            this.gbArcUploadOptions.Controls.Add(this.lblMaxAge);
            this.gbArcUploadOptions.Controls.Add(this.chkEnabled);
            this.gbArcUploadOptions.Controls.Add(this.numMaxAge);
            this.gbArcUploadOptions.Controls.Add(this.numDelay);
            this.gbArcUploadOptions.Controls.Add(this.cbSnapshotType);
            this.gbArcUploadOptions.Controls.Add(this.lblSnapshotType);
            this.gbArcUploadOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbArcUploadOptions.Location = new System.Drawing.Point(0, 0);
            this.gbArcUploadOptions.Name = "gbArcUploadOptions";
            this.gbArcUploadOptions.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbArcUploadOptions.Size = new System.Drawing.Size(414, 388);
            this.gbArcUploadOptions.TabIndex = 0;
            this.gbArcUploadOptions.TabStop = false;
            this.gbArcUploadOptions.Text = "Archive Upload Options";
            // 
            // lblDelay
            // 
            this.lblDelay.AutoSize = true;
            this.lblDelay.Location = new System.Drawing.Point(10, 99);
            this.lblDelay.Name = "lblDelay";
            this.lblDelay.Size = new System.Drawing.Size(168, 13);
            this.lblDelay.TabIndex = 3;
            this.lblDelay.Text = "Delay before sending archive, sec";
            // 
            // lblMaxAge
            // 
            this.lblMaxAge.AutoSize = true;
            this.lblMaxAge.Location = new System.Drawing.Point(10, 148);
            this.lblMaxAge.Name = "lblMaxAge";
            this.lblMaxAge.Size = new System.Drawing.Size(150, 13);
            this.lblMaxAge.TabIndex = 5;
            this.lblMaxAge.Text = "Maximum age of archive, days";
            // 
            // chkEnabled
            // 
            this.chkEnabled.AutoSize = true;
            this.chkEnabled.Location = new System.Drawing.Point(13, 19);
            this.chkEnabled.Name = "chkEnabled";
            this.chkEnabled.Size = new System.Drawing.Size(65, 17);
            this.chkEnabled.TabIndex = 0;
            this.chkEnabled.Text = "Enabled";
            this.chkEnabled.UseVisualStyleBackColor = true;
            this.chkEnabled.CheckedChanged += new System.EventHandler(this.chkEnabled_CheckedChanged);
            // 
            // numMaxAge
            // 
            this.numMaxAge.Location = new System.Drawing.Point(13, 164);
            this.numMaxAge.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.numMaxAge.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numMaxAge.Name = "numMaxAge";
            this.numMaxAge.Size = new System.Drawing.Size(120, 20);
            this.numMaxAge.TabIndex = 6;
            this.numMaxAge.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numMaxAge.ValueChanged += new System.EventHandler(this.numMaxAge_ValueChanged);
            // 
            // numDelay
            // 
            this.numDelay.Location = new System.Drawing.Point(13, 115);
            this.numDelay.Maximum = new decimal(new int[] {
            3600000,
            0,
            0,
            0});
            this.numDelay.Name = "numDelay";
            this.numDelay.Size = new System.Drawing.Size(120, 20);
            this.numDelay.TabIndex = 4;
            this.numDelay.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numDelay.ValueChanged += new System.EventHandler(this.numDelay_ValueChanged);
            // 
            // cbSnapshotType
            // 
            this.cbSnapshotType.FormattingEnabled = true;
            this.cbSnapshotType.Location = new System.Drawing.Point(13, 65);
            this.cbSnapshotType.Name = "cbSnapshotType";
            this.cbSnapshotType.Size = new System.Drawing.Size(120, 21);
            this.cbSnapshotType.TabIndex = 2;
            this.cbSnapshotType.SelectedIndexChanged += new System.EventHandler(this.cbSnapshotType_SelectedIndexChanged);
            // 
            // lblSnapshotType
            // 
            this.lblSnapshotType.AutoSize = true;
            this.lblSnapshotType.Location = new System.Drawing.Point(10, 49);
            this.lblSnapshotType.Name = "lblSnapshotType";
            this.lblSnapshotType.Size = new System.Drawing.Size(79, 13);
            this.lblSnapshotType.TabIndex = 1;
            this.lblSnapshotType.Text = "Snapshot code";
            // 
            // CtrlArcUploadOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbArcUploadOptions);
            this.Name = "CtrlArcUploadOptions";
            this.Size = new System.Drawing.Size(414, 388);
            this.gbArcUploadOptions.ResumeLayout(false);
            this.gbArcUploadOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxAge)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDelay)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbArcUploadOptions;
        private System.Windows.Forms.Label lblDelay;
        private System.Windows.Forms.Label lblMaxAge;
        private System.Windows.Forms.CheckBox chkEnabled;
        private System.Windows.Forms.NumericUpDown numMaxAge;
        private System.Windows.Forms.NumericUpDown numDelay;
        private System.Windows.Forms.ComboBox cbSnapshotType;
        private System.Windows.Forms.Label lblSnapshotType;
    }
}
