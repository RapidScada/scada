namespace Scada.Admin.App.Forms
{
    partial class FrmProjectProps
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
            this.lblName = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblDescr = new System.Windows.Forms.Label();
            this.txtDescr = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.lblVersion = new System.Windows.Forms.Label();
            this.numMajorVersion = new System.Windows.Forms.NumericUpDown();
            this.numMinorVersion = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.numMajorVersion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMinorVersion)).BeginInit();
            this.SuspendLayout();
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(9, 9);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(69, 13);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Project name";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(12, 25);
            this.txtName.Name = "txtName";
            this.txtName.ReadOnly = true;
            this.txtName.Size = new System.Drawing.Size(400, 20);
            this.txtName.TabIndex = 1;
            // 
            // lblDescr
            // 
            this.lblDescr.AutoSize = true;
            this.lblDescr.Location = new System.Drawing.Point(9, 87);
            this.lblDescr.Name = "lblDescr";
            this.lblDescr.Size = new System.Drawing.Size(60, 13);
            this.lblDescr.TabIndex = 5;
            this.lblDescr.Text = "Description";
            // 
            // txtDescr
            // 
            this.txtDescr.AcceptsReturn = true;
            this.txtDescr.Location = new System.Drawing.Point(12, 103);
            this.txtDescr.Multiline = true;
            this.txtDescr.Name = "txtDescr";
            this.txtDescr.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDescr.Size = new System.Drawing.Size(400, 100);
            this.txtDescr.TabIndex = 6;
            this.txtDescr.TextChanged += new System.EventHandler(this.control_Changed);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(337, 219);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(256, 219);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 7;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Location = new System.Drawing.Point(9, 48);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(42, 13);
            this.lblVersion.TabIndex = 2;
            this.lblVersion.Text = "Version";
            // 
            // numMajorVersion
            // 
            this.numMajorVersion.Location = new System.Drawing.Point(12, 64);
            this.numMajorVersion.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numMajorVersion.Name = "numMajorVersion";
            this.numMajorVersion.Size = new System.Drawing.Size(100, 20);
            this.numMajorVersion.TabIndex = 3;
            this.numMajorVersion.ValueChanged += new System.EventHandler(this.control_Changed);
            // 
            // numMinorVersion
            // 
            this.numMinorVersion.Location = new System.Drawing.Point(118, 64);
            this.numMinorVersion.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numMinorVersion.Name = "numMinorVersion";
            this.numMinorVersion.Size = new System.Drawing.Size(100, 20);
            this.numMinorVersion.TabIndex = 4;
            this.numMinorVersion.ValueChanged += new System.EventHandler(this.control_Changed);
            // 
            // FrmProjectProps
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(424, 254);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtDescr);
            this.Controls.Add(this.lblDescr);
            this.Controls.Add(this.numMinorVersion);
            this.Controls.Add(this.numMajorVersion);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmProjectProps";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Project Properties";
            this.Load += new System.EventHandler(this.FrmProjectProps_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numMajorVersion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMinorVersion)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblDescr;
        private System.Windows.Forms.TextBox txtDescr;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.NumericUpDown numMajorVersion;
        private System.Windows.Forms.NumericUpDown numMinorVersion;
    }
}