namespace Scada.Admin.App.Controls.Tools
{
    partial class CtrlCnlCreate3
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
            this.txtDevice = new System.Windows.Forms.TextBox();
            this.lblDevice = new System.Windows.Forms.Label();
            this.gbInCnls = new System.Windows.Forms.GroupBox();
            this.btnResetInCnls = new System.Windows.Forms.Button();
            this.numEndInCnl = new System.Windows.Forms.NumericUpDown();
            this.lblEndInCnl = new System.Windows.Forms.Label();
            this.numStartInCnl = new System.Windows.Forms.NumericUpDown();
            this.lblStartInCnl = new System.Windows.Forms.Label();
            this.gbOutCnls = new System.Windows.Forms.GroupBox();
            this.btnResetOutCnls = new System.Windows.Forms.Button();
            this.numEndOutCnl = new System.Windows.Forms.NumericUpDown();
            this.lblEndOutCnl = new System.Windows.Forms.Label();
            this.numStartOutCnl = new System.Windows.Forms.NumericUpDown();
            this.lblStartOutCnl = new System.Windows.Forms.Label();
            this.gbInCnls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numEndInCnl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStartInCnl)).BeginInit();
            this.gbOutCnls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numEndOutCnl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStartOutCnl)).BeginInit();
            this.SuspendLayout();
            // 
            // txtDevice
            // 
            this.txtDevice.Location = new System.Drawing.Point(0, 16);
            this.txtDevice.Name = "txtDevice";
            this.txtDevice.ReadOnly = true;
            this.txtDevice.Size = new System.Drawing.Size(360, 20);
            this.txtDevice.TabIndex = 1;
            // 
            // lblDevice
            // 
            this.lblDevice.AutoSize = true;
            this.lblDevice.Location = new System.Drawing.Point(-3, 0);
            this.lblDevice.Name = "lblDevice";
            this.lblDevice.Size = new System.Drawing.Size(41, 13);
            this.lblDevice.TabIndex = 0;
            this.lblDevice.Text = "Device";
            // 
            // gbInCnls
            // 
            this.gbInCnls.Controls.Add(this.btnResetInCnls);
            this.gbInCnls.Controls.Add(this.numEndInCnl);
            this.gbInCnls.Controls.Add(this.lblEndInCnl);
            this.gbInCnls.Controls.Add(this.numStartInCnl);
            this.gbInCnls.Controls.Add(this.lblStartInCnl);
            this.gbInCnls.Location = new System.Drawing.Point(0, 42);
            this.gbInCnls.Name = "gbInCnls";
            this.gbInCnls.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbInCnls.Size = new System.Drawing.Size(360, 67);
            this.gbInCnls.TabIndex = 2;
            this.gbInCnls.TabStop = false;
            this.gbInCnls.Text = "Input Channels";
            // 
            // btnResetInCnls
            // 
            this.btnResetInCnls.Location = new System.Drawing.Point(272, 31);
            this.btnResetInCnls.Name = "btnResetInCnls";
            this.btnResetInCnls.Size = new System.Drawing.Size(75, 23);
            this.btnResetInCnls.TabIndex = 4;
            this.btnResetInCnls.Text = "Reset";
            this.btnResetInCnls.UseVisualStyleBackColor = true;
            this.btnResetInCnls.Click += new System.EventHandler(this.btnResetInCnls_Click);
            // 
            // numEndInCnl
            // 
            this.numEndInCnl.Increment = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numEndInCnl.Location = new System.Drawing.Point(144, 32);
            this.numEndInCnl.Name = "numEndInCnl";
            this.numEndInCnl.ReadOnly = true;
            this.numEndInCnl.Size = new System.Drawing.Size(122, 20);
            this.numEndInCnl.TabIndex = 3;
            // 
            // lblEndInCnl
            // 
            this.lblEndInCnl.AutoSize = true;
            this.lblEndInCnl.Location = new System.Drawing.Point(141, 16);
            this.lblEndInCnl.Name = "lblEndInCnl";
            this.lblEndInCnl.Size = new System.Drawing.Size(26, 13);
            this.lblEndInCnl.TabIndex = 2;
            this.lblEndInCnl.Text = "End";
            // 
            // numStartInCnl
            // 
            this.numStartInCnl.Location = new System.Drawing.Point(16, 32);
            this.numStartInCnl.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numStartInCnl.Name = "numStartInCnl";
            this.numStartInCnl.Size = new System.Drawing.Size(122, 20);
            this.numStartInCnl.TabIndex = 1;
            this.numStartInCnl.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numStartInCnl.ValueChanged += new System.EventHandler(this.numStartInCnl_ValueChanged);
            // 
            // lblStartInCnl
            // 
            this.lblStartInCnl.AutoSize = true;
            this.lblStartInCnl.Location = new System.Drawing.Point(13, 16);
            this.lblStartInCnl.Name = "lblStartInCnl";
            this.lblStartInCnl.Size = new System.Drawing.Size(29, 13);
            this.lblStartInCnl.TabIndex = 0;
            this.lblStartInCnl.Text = "Start";
            // 
            // gbOutCnls
            // 
            this.gbOutCnls.Controls.Add(this.btnResetOutCnls);
            this.gbOutCnls.Controls.Add(this.numEndOutCnl);
            this.gbOutCnls.Controls.Add(this.lblEndOutCnl);
            this.gbOutCnls.Controls.Add(this.numStartOutCnl);
            this.gbOutCnls.Controls.Add(this.lblStartOutCnl);
            this.gbOutCnls.Location = new System.Drawing.Point(0, 115);
            this.gbOutCnls.Name = "gbOutCnls";
            this.gbOutCnls.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbOutCnls.Size = new System.Drawing.Size(360, 67);
            this.gbOutCnls.TabIndex = 3;
            this.gbOutCnls.TabStop = false;
            this.gbOutCnls.Text = "Output Channels";
            // 
            // btnResetOutCnls
            // 
            this.btnResetOutCnls.Location = new System.Drawing.Point(272, 31);
            this.btnResetOutCnls.Name = "btnResetOutCnls";
            this.btnResetOutCnls.Size = new System.Drawing.Size(75, 23);
            this.btnResetOutCnls.TabIndex = 4;
            this.btnResetOutCnls.Text = "Reset";
            this.btnResetOutCnls.UseVisualStyleBackColor = true;
            this.btnResetOutCnls.Click += new System.EventHandler(this.btnResetOutCnls_Click);
            // 
            // numEndOutCnl
            // 
            this.numEndOutCnl.Increment = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numEndOutCnl.Location = new System.Drawing.Point(144, 32);
            this.numEndOutCnl.Name = "numEndOutCnl";
            this.numEndOutCnl.ReadOnly = true;
            this.numEndOutCnl.Size = new System.Drawing.Size(122, 20);
            this.numEndOutCnl.TabIndex = 3;
            // 
            // lblEndOutCnl
            // 
            this.lblEndOutCnl.AutoSize = true;
            this.lblEndOutCnl.Location = new System.Drawing.Point(141, 16);
            this.lblEndOutCnl.Name = "lblEndOutCnl";
            this.lblEndOutCnl.Size = new System.Drawing.Size(26, 13);
            this.lblEndOutCnl.TabIndex = 2;
            this.lblEndOutCnl.Text = "End";
            // 
            // numStartOutCnl
            // 
            this.numStartOutCnl.Location = new System.Drawing.Point(16, 32);
            this.numStartOutCnl.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numStartOutCnl.Name = "numStartOutCnl";
            this.numStartOutCnl.Size = new System.Drawing.Size(122, 20);
            this.numStartOutCnl.TabIndex = 1;
            this.numStartOutCnl.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numStartOutCnl.ValueChanged += new System.EventHandler(this.numStartOutCnl_ValueChanged);
            // 
            // lblStartOutCnl
            // 
            this.lblStartOutCnl.AutoSize = true;
            this.lblStartOutCnl.Location = new System.Drawing.Point(13, 16);
            this.lblStartOutCnl.Name = "lblStartOutCnl";
            this.lblStartOutCnl.Size = new System.Drawing.Size(29, 13);
            this.lblStartOutCnl.TabIndex = 0;
            this.lblStartOutCnl.Text = "Start";
            // 
            // CtrlCnlCreate3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbOutCnls);
            this.Controls.Add(this.gbInCnls);
            this.Controls.Add(this.txtDevice);
            this.Controls.Add(this.lblDevice);
            this.Name = "CtrlCnlCreate3";
            this.Size = new System.Drawing.Size(360, 181);
            this.gbInCnls.ResumeLayout(false);
            this.gbInCnls.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numEndInCnl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStartInCnl)).EndInit();
            this.gbOutCnls.ResumeLayout(false);
            this.gbOutCnls.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numEndOutCnl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStartOutCnl)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtDevice;
        private System.Windows.Forms.Label lblDevice;
        private System.Windows.Forms.GroupBox gbInCnls;
        private System.Windows.Forms.NumericUpDown numEndInCnl;
        private System.Windows.Forms.Label lblEndInCnl;
        private System.Windows.Forms.NumericUpDown numStartInCnl;
        private System.Windows.Forms.Label lblStartInCnl;
        private System.Windows.Forms.Button btnResetInCnls;
        private System.Windows.Forms.GroupBox gbOutCnls;
        private System.Windows.Forms.Button btnResetOutCnls;
        private System.Windows.Forms.NumericUpDown numEndOutCnl;
        private System.Windows.Forms.Label lblEndOutCnl;
        private System.Windows.Forms.NumericUpDown numStartOutCnl;
        private System.Windows.Forms.Label lblStartOutCnl;
    }
}
