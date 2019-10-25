namespace Scada.Comm.Devices.OpcUa.UI
{
    partial class CtrlSubscription
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
            this.dbSubscription = new System.Windows.Forms.GroupBox();
            this.numPublishingInterval = new System.Windows.Forms.NumericUpDown();
            this.lblPublishingInterval = new System.Windows.Forms.Label();
            this.txtDisplayName = new System.Windows.Forms.TextBox();
            this.lblDisplayName = new System.Windows.Forms.Label();
            this.chkSubscrActive = new System.Windows.Forms.CheckBox();
            this.dbSubscription.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPublishingInterval)).BeginInit();
            this.SuspendLayout();
            // 
            // dbSubscription
            // 
            this.dbSubscription.Controls.Add(this.numPublishingInterval);
            this.dbSubscription.Controls.Add(this.lblPublishingInterval);
            this.dbSubscription.Controls.Add(this.txtDisplayName);
            this.dbSubscription.Controls.Add(this.lblDisplayName);
            this.dbSubscription.Controls.Add(this.chkSubscrActive);
            this.dbSubscription.Location = new System.Drawing.Point(0, 0);
            this.dbSubscription.Name = "dbSubscription";
            this.dbSubscription.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.dbSubscription.Size = new System.Drawing.Size(230, 407);
            this.dbSubscription.TabIndex = 0;
            this.dbSubscription.TabStop = false;
            this.dbSubscription.Text = "Subscription Parameters";
            // 
            // numPublishingInterval
            // 
            this.numPublishingInterval.Location = new System.Drawing.Point(13, 94);
            this.numPublishingInterval.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numPublishingInterval.Name = "numPublishingInterval";
            this.numPublishingInterval.Size = new System.Drawing.Size(100, 20);
            this.numPublishingInterval.TabIndex = 4;
            this.numPublishingInterval.ValueChanged += new System.EventHandler(this.numPublishingInterval_ValueChanged);
            // 
            // lblPublishingInterval
            // 
            this.lblPublishingInterval.AutoSize = true;
            this.lblPublishingInterval.Location = new System.Drawing.Point(10, 78);
            this.lblPublishingInterval.Name = "lblPublishingInterval";
            this.lblPublishingInterval.Size = new System.Drawing.Size(92, 13);
            this.lblPublishingInterval.TabIndex = 3;
            this.lblPublishingInterval.Text = "Publishing interval";
            // 
            // txtDisplayName
            // 
            this.txtDisplayName.Location = new System.Drawing.Point(13, 55);
            this.txtDisplayName.Name = "txtDisplayName";
            this.txtDisplayName.Size = new System.Drawing.Size(204, 20);
            this.txtDisplayName.TabIndex = 2;
            this.txtDisplayName.TextChanged += new System.EventHandler(this.txtDisplayName_TextChanged);
            // 
            // lblDisplayName
            // 
            this.lblDisplayName.AutoSize = true;
            this.lblDisplayName.Location = new System.Drawing.Point(10, 39);
            this.lblDisplayName.Name = "lblDisplayName";
            this.lblDisplayName.Size = new System.Drawing.Size(70, 13);
            this.lblDisplayName.TabIndex = 1;
            this.lblDisplayName.Text = "Display name";
            // 
            // chkSubscrActive
            // 
            this.chkSubscrActive.AutoSize = true;
            this.chkSubscrActive.Location = new System.Drawing.Point(13, 19);
            this.chkSubscrActive.Name = "chkSubscrActive";
            this.chkSubscrActive.Size = new System.Drawing.Size(56, 17);
            this.chkSubscrActive.TabIndex = 0;
            this.chkSubscrActive.Text = "Active";
            this.chkSubscrActive.UseVisualStyleBackColor = true;
            this.chkSubscrActive.CheckedChanged += new System.EventHandler(this.chkSubscrActive_CheckedChanged);
            // 
            // CtrlSubscription
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dbSubscription);
            this.Name = "CtrlSubscription";
            this.Size = new System.Drawing.Size(230, 407);
            this.dbSubscription.ResumeLayout(false);
            this.dbSubscription.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPublishingInterval)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox dbSubscription;
        private System.Windows.Forms.CheckBox chkSubscrActive;
        private System.Windows.Forms.NumericUpDown numPublishingInterval;
        private System.Windows.Forms.Label lblPublishingInterval;
        private System.Windows.Forms.TextBox txtDisplayName;
        private System.Windows.Forms.Label lblDisplayName;
    }
}
