
namespace Scada.Server.Modules.DbExport.UI
{
    partial class CtrlGeneralOptions
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
            this.gbGeneral = new System.Windows.Forms.GroupBox();
            this.numOutCnlNum = new System.Windows.Forms.NumericUpDown();
            this.lblOutCnlNum = new System.Windows.Forms.Label();
            this.numDataLifetime = new System.Windows.Forms.NumericUpDown();
            this.lblDataLifetime = new System.Windows.Forms.Label();
            this.numMaxQueueSize = new System.Windows.Forms.NumericUpDown();
            this.lblMaxQueueSize = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.lblID = new System.Windows.Forms.Label();
            this.chkActive = new System.Windows.Forms.CheckBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtTargetID = new System.Windows.Forms.TextBox();
            this.gbGeneral.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numOutCnlNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDataLifetime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxQueueSize)).BeginInit();
            this.SuspendLayout();
            // 
            // gbGeneral
            // 
            this.gbGeneral.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbGeneral.Controls.Add(this.numOutCnlNum);
            this.gbGeneral.Controls.Add(this.lblOutCnlNum);
            this.gbGeneral.Controls.Add(this.numDataLifetime);
            this.gbGeneral.Controls.Add(this.lblDataLifetime);
            this.gbGeneral.Controls.Add(this.numMaxQueueSize);
            this.gbGeneral.Controls.Add(this.lblMaxQueueSize);
            this.gbGeneral.Controls.Add(this.lblName);
            this.gbGeneral.Controls.Add(this.lblID);
            this.gbGeneral.Controls.Add(this.chkActive);
            this.gbGeneral.Controls.Add(this.txtName);
            this.gbGeneral.Controls.Add(this.txtTargetID);
            this.gbGeneral.Location = new System.Drawing.Point(0, 0);
            this.gbGeneral.Name = "gbGeneral";
            this.gbGeneral.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbGeneral.Size = new System.Drawing.Size(414, 388);
            this.gbGeneral.TabIndex = 0;
            this.gbGeneral.TabStop = false;
            this.gbGeneral.Text = "General";
            // 
            // numOutCnlNum
            // 
            this.numOutCnlNum.Location = new System.Drawing.Point(13, 211);
            this.numOutCnlNum.Maximum = new decimal(new int[] {
            65635,
            0,
            0,
            0});
            this.numOutCnlNum.Name = "numOutCnlNum";
            this.numOutCnlNum.Size = new System.Drawing.Size(120, 20);
            this.numOutCnlNum.TabIndex = 10;
            this.numOutCnlNum.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numOutCnlNum.ValueChanged += new System.EventHandler(this.numOutCnlNum_ValueChanged);
            // 
            // lblOutCnlNum
            // 
            this.lblOutCnlNum.AutoSize = true;
            this.lblOutCnlNum.Location = new System.Drawing.Point(10, 195);
            this.lblOutCnlNum.Name = "lblOutCnlNum";
            this.lblOutCnlNum.Size = new System.Drawing.Size(118, 13);
            this.lblOutCnlNum.TabIndex = 9;
            this.lblOutCnlNum.Text = "Output channel number";
            // 
            // numDataLifetime
            // 
            this.numDataLifetime.Location = new System.Drawing.Point(13, 163);
            this.numDataLifetime.Maximum = new decimal(new int[] {
            3600,
            0,
            0,
            0});
            this.numDataLifetime.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numDataLifetime.Name = "numDataLifetime";
            this.numDataLifetime.Size = new System.Drawing.Size(120, 20);
            this.numDataLifetime.TabIndex = 8;
            this.numDataLifetime.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.numDataLifetime.ValueChanged += new System.EventHandler(this.numDataLifetime_ValueChanged);
            // 
            // lblDataLifetime
            // 
            this.lblDataLifetime.AutoSize = true;
            this.lblDataLifetime.Location = new System.Drawing.Point(10, 147);
            this.lblDataLifetime.Name = "lblDataLifetime";
            this.lblDataLifetime.Size = new System.Drawing.Size(91, 13);
            this.lblDataLifetime.TabIndex = 7;
            this.lblDataLifetime.Text = "Data life time, sec";
            // 
            // numMaxQueueSize
            // 
            this.numMaxQueueSize.Location = new System.Drawing.Point(13, 110);
            this.numMaxQueueSize.Maximum = new decimal(new int[] {
            86400,
            0,
            0,
            0});
            this.numMaxQueueSize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numMaxQueueSize.Name = "numMaxQueueSize";
            this.numMaxQueueSize.Size = new System.Drawing.Size(120, 20);
            this.numMaxQueueSize.TabIndex = 6;
            this.numMaxQueueSize.Value = new decimal(new int[] {
            3600,
            0,
            0,
            0});
            this.numMaxQueueSize.ValueChanged += new System.EventHandler(this.numMaxQueueSize_ValueChanged);
            // 
            // lblMaxQueueSize
            // 
            this.lblMaxQueueSize.AutoSize = true;
            this.lblMaxQueueSize.Location = new System.Drawing.Point(10, 94);
            this.lblMaxQueueSize.Name = "lblMaxQueueSize";
            this.lblMaxQueueSize.Size = new System.Drawing.Size(105, 13);
            this.lblMaxQueueSize.TabIndex = 5;
            this.lblMaxQueueSize.Text = "Maximum queue size";
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(74, 43);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(35, 13);
            this.lblName.TabIndex = 3;
            this.lblName.Text = "Name";
            // 
            // lblID
            // 
            this.lblID.AutoSize = true;
            this.lblID.Location = new System.Drawing.Point(10, 43);
            this.lblID.Name = "lblID";
            this.lblID.Size = new System.Drawing.Size(52, 13);
            this.lblID.TabIndex = 1;
            this.lblID.Text = "Target ID";
            // 
            // chkActive
            // 
            this.chkActive.AutoSize = true;
            this.chkActive.Location = new System.Drawing.Point(13, 19);
            this.chkActive.Name = "chkActive";
            this.chkActive.Size = new System.Drawing.Size(56, 17);
            this.chkActive.TabIndex = 0;
            this.chkActive.Text = "Active";
            this.chkActive.UseVisualStyleBackColor = true;
            this.chkActive.CheckedChanged += new System.EventHandler(this.chkActive_CheckedChanged);
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(77, 59);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(324, 20);
            this.txtName.TabIndex = 4;
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            // 
            // txtTargetID
            // 
            this.txtTargetID.Location = new System.Drawing.Point(13, 59);
            this.txtTargetID.Name = "txtTargetID";
            this.txtTargetID.ReadOnly = true;
            this.txtTargetID.Size = new System.Drawing.Size(58, 20);
            this.txtTargetID.TabIndex = 2;
            this.txtTargetID.TabStop = false;
            // 
            // CtrlGeneralOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbGeneral);
            this.Name = "CtrlGeneralOptions";
            this.Size = new System.Drawing.Size(414, 388);
            this.gbGeneral.ResumeLayout(false);
            this.gbGeneral.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numOutCnlNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDataLifetime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxQueueSize)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbGeneral;
        private System.Windows.Forms.CheckBox chkActive;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtTargetID;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblID;
        private System.Windows.Forms.NumericUpDown numOutCnlNum;
        private System.Windows.Forms.Label lblOutCnlNum;
        private System.Windows.Forms.NumericUpDown numDataLifetime;
        private System.Windows.Forms.Label lblDataLifetime;
        private System.Windows.Forms.NumericUpDown numMaxQueueSize;
        private System.Windows.Forms.Label lblMaxQueueSize;
    }
}
