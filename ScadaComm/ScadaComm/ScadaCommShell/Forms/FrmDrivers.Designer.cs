namespace Scada.Comm.Shell.Forms
{
    partial class FrmDrivers
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
            this.txtDescr = new System.Windows.Forms.TextBox();
            this.lblDescr = new System.Windows.Forms.Label();
            this.lbDrivers = new System.Windows.Forms.ListBox();
            this.lblDrivers = new System.Windows.Forms.Label();
            this.btnProperties = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtDescr
            // 
            this.txtDescr.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDescr.Location = new System.Drawing.Point(12, 249);
            this.txtDescr.Multiline = true;
            this.txtDescr.Name = "txtDescr";
            this.txtDescr.ReadOnly = true;
            this.txtDescr.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDescr.Size = new System.Drawing.Size(660, 150);
            this.txtDescr.TabIndex = 4;
            // 
            // lblDescr
            // 
            this.lblDescr.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblDescr.AutoSize = true;
            this.lblDescr.Location = new System.Drawing.Point(9, 233);
            this.lblDescr.Name = "lblDescr";
            this.lblDescr.Size = new System.Drawing.Size(60, 13);
            this.lblDescr.TabIndex = 3;
            this.lblDescr.Text = "Description";
            // 
            // lbDrivers
            // 
            this.lbDrivers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbDrivers.HorizontalScrollbar = true;
            this.lbDrivers.IntegralHeight = false;
            this.lbDrivers.Location = new System.Drawing.Point(12, 54);
            this.lbDrivers.MultiColumn = true;
            this.lbDrivers.Name = "lbDrivers";
            this.lbDrivers.Size = new System.Drawing.Size(660, 166);
            this.lbDrivers.TabIndex = 2;
            this.lbDrivers.SelectedIndexChanged += new System.EventHandler(this.lbDrivers_SelectedIndexChanged);
            this.lbDrivers.DoubleClick += new System.EventHandler(this.lbDrivers_DoubleClick);
            // 
            // lblDrivers
            // 
            this.lblDrivers.AutoSize = true;
            this.lblDrivers.Location = new System.Drawing.Point(9, 9);
            this.lblDrivers.Name = "lblDrivers";
            this.lblDrivers.Size = new System.Drawing.Size(87, 13);
            this.lblDrivers.TabIndex = 0;
            this.lblDrivers.Text = "Available drivers:";
            // 
            // btnProperties
            // 
            this.btnProperties.Location = new System.Drawing.Point(12, 25);
            this.btnProperties.Name = "btnProperties";
            this.btnProperties.Size = new System.Drawing.Size(100, 23);
            this.btnProperties.TabIndex = 1;
            this.btnProperties.Text = "Properties";
            this.btnProperties.UseVisualStyleBackColor = true;
            this.btnProperties.Click += new System.EventHandler(this.btnProperties_Click);
            // 
            // FrmDrivers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 411);
            this.Controls.Add(this.lbDrivers);
            this.Controls.Add(this.lblDrivers);
            this.Controls.Add(this.btnProperties);
            this.Controls.Add(this.txtDescr);
            this.Controls.Add(this.lblDescr);
            this.Name = "FrmDrivers";
            this.Text = "Drivers";
            this.Load += new System.EventHandler(this.FrmDrivers_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtDescr;
        private System.Windows.Forms.Label lblDescr;
        private System.Windows.Forms.ListBox lbDrivers;
        private System.Windows.Forms.Label lblDrivers;
        private System.Windows.Forms.Button btnProperties;
    }
}