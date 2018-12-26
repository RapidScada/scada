namespace Scada.Server.Shell.Forms
{
    partial class FrmSaveParams
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
            this.gbEvents = new System.Windows.Forms.GroupBox();
            this.chkWriteEvCopy = new System.Windows.Forms.CheckBox();
            this.chkWriteEv = new System.Windows.Forms.CheckBox();
            this.numStoreEvPer = new System.Windows.Forms.NumericUpDown();
            this.lblStoreEvPer = new System.Windows.Forms.Label();
            this.lblWriteEvPer = new System.Windows.Forms.Label();
            this.cbWriteEvPer = new System.Windows.Forms.ComboBox();
            this.gbHourData = new System.Windows.Forms.GroupBox();
            this.chkWriteHrCopy = new System.Windows.Forms.CheckBox();
            this.chkWriteHr = new System.Windows.Forms.CheckBox();
            this.numStoreHrPer = new System.Windows.Forms.NumericUpDown();
            this.lblStoreHrPer = new System.Windows.Forms.Label();
            this.lblWriteHrPer = new System.Windows.Forms.Label();
            this.cbWriteHrPer = new System.Windows.Forms.ComboBox();
            this.gbMinData = new System.Windows.Forms.GroupBox();
            this.chkWriteMinCopy = new System.Windows.Forms.CheckBox();
            this.chkWriteMin = new System.Windows.Forms.CheckBox();
            this.numStoreMinPer = new System.Windows.Forms.NumericUpDown();
            this.lblStoreMinPer = new System.Windows.Forms.Label();
            this.lblWriteMinPer = new System.Windows.Forms.Label();
            this.cbWriteMinPer = new System.Windows.Forms.ComboBox();
            this.gbCurData = new System.Windows.Forms.GroupBox();
            this.cbInactUnrelTime = new System.Windows.Forms.ComboBox();
            this.chkWriteCurCopy = new System.Windows.Forms.CheckBox();
            this.chkWriteCur = new System.Windows.Forms.CheckBox();
            this.lblInactUnrelTime = new System.Windows.Forms.Label();
            this.lblWriteCurPer = new System.Windows.Forms.Label();
            this.cbWriteCurPer = new System.Windows.Forms.ComboBox();
            this.gbEvents.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numStoreEvPer)).BeginInit();
            this.gbHourData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numStoreHrPer)).BeginInit();
            this.gbMinData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numStoreMinPer)).BeginInit();
            this.gbCurData.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbEvents
            // 
            this.gbEvents.Controls.Add(this.chkWriteEvCopy);
            this.gbEvents.Controls.Add(this.chkWriteEv);
            this.gbEvents.Controls.Add(this.numStoreEvPer);
            this.gbEvents.Controls.Add(this.lblStoreEvPer);
            this.gbEvents.Controls.Add(this.lblWriteEvPer);
            this.gbEvents.Controls.Add(this.cbWriteEvPer);
            this.gbEvents.Location = new System.Drawing.Point(265, 169);
            this.gbEvents.Name = "gbEvents";
            this.gbEvents.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbEvents.Size = new System.Drawing.Size(247, 152);
            this.gbEvents.TabIndex = 7;
            this.gbEvents.TabStop = false;
            this.gbEvents.Text = "Events";
            // 
            // chkWriteEvCopy
            // 
            this.chkWriteEvCopy.AutoSize = true;
            this.chkWriteEvCopy.Checked = true;
            this.chkWriteEvCopy.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkWriteEvCopy.Location = new System.Drawing.Point(13, 122);
            this.chkWriteEvCopy.Name = "chkWriteEvCopy";
            this.chkWriteEvCopy.Size = new System.Drawing.Size(101, 17);
            this.chkWriteEvCopy.TabIndex = 5;
            this.chkWriteEvCopy.Text = "Write data copy";
            this.chkWriteEvCopy.UseVisualStyleBackColor = true;
            this.chkWriteEvCopy.CheckedChanged += new System.EventHandler(this.control_Changed);
            // 
            // chkWriteEv
            // 
            this.chkWriteEv.AutoSize = true;
            this.chkWriteEv.Checked = true;
            this.chkWriteEv.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkWriteEv.Location = new System.Drawing.Point(13, 99);
            this.chkWriteEv.Name = "chkWriteEv";
            this.chkWriteEv.Size = new System.Drawing.Size(75, 17);
            this.chkWriteEv.TabIndex = 4;
            this.chkWriteEv.Text = "Write data";
            this.chkWriteEv.UseVisualStyleBackColor = true;
            this.chkWriteEv.CheckedChanged += new System.EventHandler(this.control_Changed);
            // 
            // numStoreEvPer
            // 
            this.numStoreEvPer.Location = new System.Drawing.Point(13, 72);
            this.numStoreEvPer.Maximum = new decimal(new int[] {
            3650,
            0,
            0,
            0});
            this.numStoreEvPer.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numStoreEvPer.Name = "numStoreEvPer";
            this.numStoreEvPer.Size = new System.Drawing.Size(221, 20);
            this.numStoreEvPer.TabIndex = 3;
            this.numStoreEvPer.Value = new decimal(new int[] {
            365,
            0,
            0,
            0});
            this.numStoreEvPer.ValueChanged += new System.EventHandler(this.control_Changed);
            // 
            // lblStoreEvPer
            // 
            this.lblStoreEvPer.AutoSize = true;
            this.lblStoreEvPer.Location = new System.Drawing.Point(10, 56);
            this.lblStoreEvPer.Name = "lblStoreEvPer";
            this.lblStoreEvPer.Size = new System.Drawing.Size(100, 13);
            this.lblStoreEvPer.TabIndex = 2;
            this.lblStoreEvPer.Text = "Storing period, days";
            // 
            // lblWriteEvPer
            // 
            this.lblWriteEvPer.AutoSize = true;
            this.lblWriteEvPer.Location = new System.Drawing.Point(10, 16);
            this.lblWriteEvPer.Name = "lblWriteEvPer";
            this.lblWriteEvPer.Size = new System.Drawing.Size(72, 13);
            this.lblWriteEvPer.TabIndex = 0;
            this.lblWriteEvPer.Text = "Writing period";
            // 
            // cbWriteEvPer
            // 
            this.cbWriteEvPer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWriteEvPer.Enabled = false;
            this.cbWriteEvPer.FormattingEnabled = true;
            this.cbWriteEvPer.Items.AddRange(new object[] {
            "On change"});
            this.cbWriteEvPer.Location = new System.Drawing.Point(13, 32);
            this.cbWriteEvPer.Name = "cbWriteEvPer";
            this.cbWriteEvPer.Size = new System.Drawing.Size(221, 21);
            this.cbWriteEvPer.TabIndex = 1;
            // 
            // gbHourData
            // 
            this.gbHourData.Controls.Add(this.chkWriteHrCopy);
            this.gbHourData.Controls.Add(this.chkWriteHr);
            this.gbHourData.Controls.Add(this.numStoreHrPer);
            this.gbHourData.Controls.Add(this.lblStoreHrPer);
            this.gbHourData.Controls.Add(this.lblWriteHrPer);
            this.gbHourData.Controls.Add(this.cbWriteHrPer);
            this.gbHourData.Location = new System.Drawing.Point(12, 169);
            this.gbHourData.Name = "gbHourData";
            this.gbHourData.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbHourData.Size = new System.Drawing.Size(247, 152);
            this.gbHourData.TabIndex = 6;
            this.gbHourData.TabStop = false;
            this.gbHourData.Text = "Hourly Data";
            // 
            // chkWriteHrCopy
            // 
            this.chkWriteHrCopy.AutoSize = true;
            this.chkWriteHrCopy.Checked = true;
            this.chkWriteHrCopy.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkWriteHrCopy.Location = new System.Drawing.Point(13, 122);
            this.chkWriteHrCopy.Name = "chkWriteHrCopy";
            this.chkWriteHrCopy.Size = new System.Drawing.Size(101, 17);
            this.chkWriteHrCopy.TabIndex = 5;
            this.chkWriteHrCopy.Text = "Write data copy";
            this.chkWriteHrCopy.UseVisualStyleBackColor = true;
            this.chkWriteHrCopy.CheckedChanged += new System.EventHandler(this.control_Changed);
            // 
            // chkWriteHr
            // 
            this.chkWriteHr.AutoSize = true;
            this.chkWriteHr.Checked = true;
            this.chkWriteHr.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkWriteHr.Location = new System.Drawing.Point(13, 99);
            this.chkWriteHr.Name = "chkWriteHr";
            this.chkWriteHr.Size = new System.Drawing.Size(75, 17);
            this.chkWriteHr.TabIndex = 4;
            this.chkWriteHr.Text = "Write data";
            this.chkWriteHr.UseVisualStyleBackColor = true;
            this.chkWriteHr.CheckedChanged += new System.EventHandler(this.control_Changed);
            // 
            // numStoreHrPer
            // 
            this.numStoreHrPer.Location = new System.Drawing.Point(13, 72);
            this.numStoreHrPer.Maximum = new decimal(new int[] {
            3650,
            0,
            0,
            0});
            this.numStoreHrPer.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numStoreHrPer.Name = "numStoreHrPer";
            this.numStoreHrPer.Size = new System.Drawing.Size(221, 20);
            this.numStoreHrPer.TabIndex = 3;
            this.numStoreHrPer.Value = new decimal(new int[] {
            365,
            0,
            0,
            0});
            this.numStoreHrPer.ValueChanged += new System.EventHandler(this.control_Changed);
            // 
            // lblStoreHrPer
            // 
            this.lblStoreHrPer.AutoSize = true;
            this.lblStoreHrPer.Location = new System.Drawing.Point(10, 56);
            this.lblStoreHrPer.Name = "lblStoreHrPer";
            this.lblStoreHrPer.Size = new System.Drawing.Size(100, 13);
            this.lblStoreHrPer.TabIndex = 2;
            this.lblStoreHrPer.Text = "Storing period, days";
            // 
            // lblWriteHrPer
            // 
            this.lblWriteHrPer.AutoSize = true;
            this.lblWriteHrPer.Location = new System.Drawing.Point(10, 16);
            this.lblWriteHrPer.Name = "lblWriteHrPer";
            this.lblWriteHrPer.Size = new System.Drawing.Size(72, 13);
            this.lblWriteHrPer.TabIndex = 0;
            this.lblWriteHrPer.Text = "Writing period";
            // 
            // cbWriteHrPer
            // 
            this.cbWriteHrPer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWriteHrPer.FormattingEnabled = true;
            this.cbWriteHrPer.Items.AddRange(new object[] {
            "30 minutes",
            "1 hour"});
            this.cbWriteHrPer.Location = new System.Drawing.Point(13, 32);
            this.cbWriteHrPer.Name = "cbWriteHrPer";
            this.cbWriteHrPer.Size = new System.Drawing.Size(221, 21);
            this.cbWriteHrPer.TabIndex = 1;
            this.cbWriteHrPer.SelectedIndexChanged += new System.EventHandler(this.control_Changed);
            // 
            // gbMinData
            // 
            this.gbMinData.Controls.Add(this.chkWriteMinCopy);
            this.gbMinData.Controls.Add(this.chkWriteMin);
            this.gbMinData.Controls.Add(this.numStoreMinPer);
            this.gbMinData.Controls.Add(this.lblStoreMinPer);
            this.gbMinData.Controls.Add(this.lblWriteMinPer);
            this.gbMinData.Controls.Add(this.cbWriteMinPer);
            this.gbMinData.Location = new System.Drawing.Point(265, 12);
            this.gbMinData.Name = "gbMinData";
            this.gbMinData.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbMinData.Size = new System.Drawing.Size(247, 152);
            this.gbMinData.TabIndex = 5;
            this.gbMinData.TabStop = false;
            this.gbMinData.Text = "Minute Data";
            // 
            // chkWriteMinCopy
            // 
            this.chkWriteMinCopy.AutoSize = true;
            this.chkWriteMinCopy.Checked = true;
            this.chkWriteMinCopy.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkWriteMinCopy.Location = new System.Drawing.Point(13, 122);
            this.chkWriteMinCopy.Name = "chkWriteMinCopy";
            this.chkWriteMinCopy.Size = new System.Drawing.Size(101, 17);
            this.chkWriteMinCopy.TabIndex = 5;
            this.chkWriteMinCopy.Text = "Write data copy";
            this.chkWriteMinCopy.UseVisualStyleBackColor = true;
            this.chkWriteMinCopy.CheckedChanged += new System.EventHandler(this.control_Changed);
            // 
            // chkWriteMin
            // 
            this.chkWriteMin.AutoSize = true;
            this.chkWriteMin.Checked = true;
            this.chkWriteMin.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkWriteMin.Location = new System.Drawing.Point(13, 99);
            this.chkWriteMin.Name = "chkWriteMin";
            this.chkWriteMin.Size = new System.Drawing.Size(75, 17);
            this.chkWriteMin.TabIndex = 4;
            this.chkWriteMin.Text = "Write data";
            this.chkWriteMin.UseVisualStyleBackColor = true;
            this.chkWriteMin.CheckedChanged += new System.EventHandler(this.control_Changed);
            // 
            // numStoreMinPer
            // 
            this.numStoreMinPer.Location = new System.Drawing.Point(13, 72);
            this.numStoreMinPer.Maximum = new decimal(new int[] {
            3650,
            0,
            0,
            0});
            this.numStoreMinPer.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numStoreMinPer.Name = "numStoreMinPer";
            this.numStoreMinPer.Size = new System.Drawing.Size(221, 20);
            this.numStoreMinPer.TabIndex = 3;
            this.numStoreMinPer.Value = new decimal(new int[] {
            365,
            0,
            0,
            0});
            this.numStoreMinPer.ValueChanged += new System.EventHandler(this.control_Changed);
            // 
            // lblStoreMinPer
            // 
            this.lblStoreMinPer.AutoSize = true;
            this.lblStoreMinPer.Location = new System.Drawing.Point(10, 56);
            this.lblStoreMinPer.Name = "lblStoreMinPer";
            this.lblStoreMinPer.Size = new System.Drawing.Size(100, 13);
            this.lblStoreMinPer.TabIndex = 2;
            this.lblStoreMinPer.Text = "Storing period, days";
            // 
            // lblWriteMinPer
            // 
            this.lblWriteMinPer.AutoSize = true;
            this.lblWriteMinPer.Location = new System.Drawing.Point(10, 16);
            this.lblWriteMinPer.Name = "lblWriteMinPer";
            this.lblWriteMinPer.Size = new System.Drawing.Size(72, 13);
            this.lblWriteMinPer.TabIndex = 0;
            this.lblWriteMinPer.Text = "Writing period";
            // 
            // cbWriteMinPer
            // 
            this.cbWriteMinPer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWriteMinPer.FormattingEnabled = true;
            this.cbWriteMinPer.Items.AddRange(new object[] {
            "30 seconds",
            "1 minute",
            "2 minutes",
            "3 minutes",
            "4 minutes",
            "5 minutes",
            "10 minutes"});
            this.cbWriteMinPer.Location = new System.Drawing.Point(13, 32);
            this.cbWriteMinPer.Name = "cbWriteMinPer";
            this.cbWriteMinPer.Size = new System.Drawing.Size(221, 21);
            this.cbWriteMinPer.TabIndex = 1;
            this.cbWriteMinPer.SelectedIndexChanged += new System.EventHandler(this.control_Changed);
            // 
            // gbCurData
            // 
            this.gbCurData.Controls.Add(this.cbInactUnrelTime);
            this.gbCurData.Controls.Add(this.chkWriteCurCopy);
            this.gbCurData.Controls.Add(this.chkWriteCur);
            this.gbCurData.Controls.Add(this.lblInactUnrelTime);
            this.gbCurData.Controls.Add(this.lblWriteCurPer);
            this.gbCurData.Controls.Add(this.cbWriteCurPer);
            this.gbCurData.Location = new System.Drawing.Point(12, 12);
            this.gbCurData.Name = "gbCurData";
            this.gbCurData.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbCurData.Size = new System.Drawing.Size(247, 152);
            this.gbCurData.TabIndex = 4;
            this.gbCurData.TabStop = false;
            this.gbCurData.Text = "Current Data";
            // 
            // cbInactUnrelTime
            // 
            this.cbInactUnrelTime.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbInactUnrelTime.FormattingEnabled = true;
            this.cbInactUnrelTime.Items.AddRange(new object[] {
            "Disabled",
            "1 minute",
            "2 minutes",
            "3 minutes",
            "4 minutes",
            "5 minutes",
            "10 minutes",
            "20 minutes",
            "30 minutes",
            "1 hour"});
            this.cbInactUnrelTime.Location = new System.Drawing.Point(13, 72);
            this.cbInactUnrelTime.Name = "cbInactUnrelTime";
            this.cbInactUnrelTime.Size = new System.Drawing.Size(221, 21);
            this.cbInactUnrelTime.TabIndex = 3;
            this.cbInactUnrelTime.SelectedIndexChanged += new System.EventHandler(this.control_Changed);
            // 
            // chkWriteCurCopy
            // 
            this.chkWriteCurCopy.AutoSize = true;
            this.chkWriteCurCopy.Checked = true;
            this.chkWriteCurCopy.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkWriteCurCopy.Location = new System.Drawing.Point(13, 122);
            this.chkWriteCurCopy.Name = "chkWriteCurCopy";
            this.chkWriteCurCopy.Size = new System.Drawing.Size(101, 17);
            this.chkWriteCurCopy.TabIndex = 5;
            this.chkWriteCurCopy.Text = "Write data copy";
            this.chkWriteCurCopy.UseVisualStyleBackColor = true;
            this.chkWriteCurCopy.CheckedChanged += new System.EventHandler(this.control_Changed);
            // 
            // chkWriteCur
            // 
            this.chkWriteCur.AutoSize = true;
            this.chkWriteCur.Checked = true;
            this.chkWriteCur.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkWriteCur.Location = new System.Drawing.Point(13, 99);
            this.chkWriteCur.Name = "chkWriteCur";
            this.chkWriteCur.Size = new System.Drawing.Size(75, 17);
            this.chkWriteCur.TabIndex = 4;
            this.chkWriteCur.Text = "Write data";
            this.chkWriteCur.UseVisualStyleBackColor = true;
            this.chkWriteCur.CheckedChanged += new System.EventHandler(this.control_Changed);
            // 
            // lblInactUnrelTime
            // 
            this.lblInactUnrelTime.AutoSize = true;
            this.lblInactUnrelTime.Location = new System.Drawing.Point(10, 56);
            this.lblInactUnrelTime.Name = "lblInactUnrelTime";
            this.lblInactUnrelTime.Size = new System.Drawing.Size(113, 13);
            this.lblInactUnrelTime.TabIndex = 2;
            this.lblInactUnrelTime.Text = "Unreliable on inactivity";
            // 
            // lblWriteCurPer
            // 
            this.lblWriteCurPer.AutoSize = true;
            this.lblWriteCurPer.Location = new System.Drawing.Point(10, 16);
            this.lblWriteCurPer.Name = "lblWriteCurPer";
            this.lblWriteCurPer.Size = new System.Drawing.Size(72, 13);
            this.lblWriteCurPer.TabIndex = 0;
            this.lblWriteCurPer.Text = "Writing period";
            // 
            // cbWriteCurPer
            // 
            this.cbWriteCurPer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWriteCurPer.FormattingEnabled = true;
            this.cbWriteCurPer.Items.AddRange(new object[] {
            "On change",
            "1 second",
            "2 seconds",
            "3 seconds",
            "4 seconds",
            "5 seconds",
            "10 seconds",
            "20 seconds",
            "30 seconds",
            "1 minute"});
            this.cbWriteCurPer.Location = new System.Drawing.Point(13, 32);
            this.cbWriteCurPer.Name = "cbWriteCurPer";
            this.cbWriteCurPer.Size = new System.Drawing.Size(221, 21);
            this.cbWriteCurPer.TabIndex = 1;
            this.cbWriteCurPer.SelectedIndexChanged += new System.EventHandler(this.control_Changed);
            // 
            // FrmSaveParams
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 411);
            this.Controls.Add(this.gbEvents);
            this.Controls.Add(this.gbHourData);
            this.Controls.Add(this.gbMinData);
            this.Controls.Add(this.gbCurData);
            this.Name = "FrmSaveParams";
            this.Text = "Saving Parameters";
            this.Load += new System.EventHandler(this.FrmSaveParams_Load);
            this.gbEvents.ResumeLayout(false);
            this.gbEvents.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numStoreEvPer)).EndInit();
            this.gbHourData.ResumeLayout(false);
            this.gbHourData.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numStoreHrPer)).EndInit();
            this.gbMinData.ResumeLayout(false);
            this.gbMinData.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numStoreMinPer)).EndInit();
            this.gbCurData.ResumeLayout(false);
            this.gbCurData.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbEvents;
        private System.Windows.Forms.CheckBox chkWriteEvCopy;
        private System.Windows.Forms.CheckBox chkWriteEv;
        private System.Windows.Forms.NumericUpDown numStoreEvPer;
        private System.Windows.Forms.Label lblStoreEvPer;
        private System.Windows.Forms.Label lblWriteEvPer;
        private System.Windows.Forms.ComboBox cbWriteEvPer;
        private System.Windows.Forms.GroupBox gbHourData;
        private System.Windows.Forms.CheckBox chkWriteHrCopy;
        private System.Windows.Forms.CheckBox chkWriteHr;
        private System.Windows.Forms.NumericUpDown numStoreHrPer;
        private System.Windows.Forms.Label lblStoreHrPer;
        private System.Windows.Forms.Label lblWriteHrPer;
        private System.Windows.Forms.ComboBox cbWriteHrPer;
        private System.Windows.Forms.GroupBox gbMinData;
        private System.Windows.Forms.CheckBox chkWriteMinCopy;
        private System.Windows.Forms.CheckBox chkWriteMin;
        private System.Windows.Forms.NumericUpDown numStoreMinPer;
        private System.Windows.Forms.Label lblStoreMinPer;
        private System.Windows.Forms.Label lblWriteMinPer;
        private System.Windows.Forms.ComboBox cbWriteMinPer;
        private System.Windows.Forms.GroupBox gbCurData;
        private System.Windows.Forms.ComboBox cbInactUnrelTime;
        private System.Windows.Forms.CheckBox chkWriteCurCopy;
        private System.Windows.Forms.CheckBox chkWriteCur;
        private System.Windows.Forms.Label lblInactUnrelTime;
        private System.Windows.Forms.Label lblWriteCurPer;
        private System.Windows.Forms.ComboBox cbWriteCurPer;
    }
}