namespace Scada.Comm.Shell.Controls
{
    partial class CtrlLineMainParams
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
            this.gbCommLine = new System.Windows.Forms.GroupBox();
            this.chkLineBound = new System.Windows.Forms.CheckBox();
            this.numLineNumber = new System.Windows.Forms.NumericUpDown();
            this.txtLineName = new System.Windows.Forms.TextBox();
            this.lnlLineName = new System.Windows.Forms.Label();
            this.lblLineNumber = new System.Windows.Forms.Label();
            this.chkLineActive = new System.Windows.Forms.CheckBox();
            this.gbLineParams = new System.Windows.Forms.GroupBox();
            this.chkReqAfterCmd = new System.Windows.Forms.CheckBox();
            this.lblReqAfterCmd = new System.Windows.Forms.Label();
            this.lblDetailedLog = new System.Windows.Forms.Label();
            this.chkDetailedLog = new System.Windows.Forms.CheckBox();
            this.lblCmdEnabled = new System.Windows.Forms.Label();
            this.chkCmdEnabled = new System.Windows.Forms.CheckBox();
            this.lblCycleDelay = new System.Windows.Forms.Label();
            this.numCycleDelay = new System.Windows.Forms.NumericUpDown();
            this.numReqTriesCnt = new System.Windows.Forms.NumericUpDown();
            this.lblReqTriesCnt = new System.Windows.Forms.Label();
            this.gbCommChannel = new System.Windows.Forms.GroupBox();
            this.txtCommCnlParams = new System.Windows.Forms.TextBox();
            this.lblCommCnlParams = new System.Windows.Forms.Label();
            this.btnCommCnlProps = new System.Windows.Forms.Button();
            this.cbCommCnlType = new System.Windows.Forms.ComboBox();
            this.lblCommCnlType = new System.Windows.Forms.Label();
            this.gbCommLine.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numLineNumber)).BeginInit();
            this.gbLineParams.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCycleDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numReqTriesCnt)).BeginInit();
            this.gbCommChannel.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbCommLine
            // 
            this.gbCommLine.Controls.Add(this.chkLineBound);
            this.gbCommLine.Controls.Add(this.numLineNumber);
            this.gbCommLine.Controls.Add(this.txtLineName);
            this.gbCommLine.Controls.Add(this.lnlLineName);
            this.gbCommLine.Controls.Add(this.lblLineNumber);
            this.gbCommLine.Controls.Add(this.chkLineActive);
            this.gbCommLine.Location = new System.Drawing.Point(0, 0);
            this.gbCommLine.Name = "gbCommLine";
            this.gbCommLine.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbCommLine.Size = new System.Drawing.Size(450, 88);
            this.gbCommLine.TabIndex = 0;
            this.gbCommLine.TabStop = false;
            this.gbCommLine.Text = "Communication Line";
            // 
            // chkLineBound
            // 
            this.chkLineBound.AutoSize = true;
            this.chkLineBound.Location = new System.Drawing.Point(94, 19);
            this.chkLineBound.Name = "chkLineBound";
            this.chkLineBound.Size = new System.Drawing.Size(103, 17);
            this.chkLineBound.TabIndex = 1;
            this.chkLineBound.Text = "Bound to Server";
            this.chkLineBound.UseVisualStyleBackColor = true;
            this.chkLineBound.CheckedChanged += new System.EventHandler(this.control_Changed);
            // 
            // numLineNumber
            // 
            this.numLineNumber.Location = new System.Drawing.Point(13, 55);
            this.numLineNumber.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numLineNumber.Name = "numLineNumber";
            this.numLineNumber.Size = new System.Drawing.Size(75, 20);
            this.numLineNumber.TabIndex = 3;
            this.numLineNumber.ValueChanged += new System.EventHandler(this.control_Changed);
            // 
            // txtLineName
            // 
            this.txtLineName.Location = new System.Drawing.Point(94, 55);
            this.txtLineName.Name = "txtLineName";
            this.txtLineName.Size = new System.Drawing.Size(343, 20);
            this.txtLineName.TabIndex = 5;
            this.txtLineName.TextChanged += new System.EventHandler(this.control_Changed);
            // 
            // lnlLineName
            // 
            this.lnlLineName.AutoSize = true;
            this.lnlLineName.Location = new System.Drawing.Point(91, 39);
            this.lnlLineName.Name = "lnlLineName";
            this.lnlLineName.Size = new System.Drawing.Size(35, 13);
            this.lnlLineName.TabIndex = 4;
            this.lnlLineName.Text = "Name";
            // 
            // lblLineNumber
            // 
            this.lblLineNumber.AutoSize = true;
            this.lblLineNumber.Location = new System.Drawing.Point(10, 39);
            this.lblLineNumber.Name = "lblLineNumber";
            this.lblLineNumber.Size = new System.Drawing.Size(44, 13);
            this.lblLineNumber.TabIndex = 2;
            this.lblLineNumber.Text = "Number";
            // 
            // chkLineActive
            // 
            this.chkLineActive.AutoSize = true;
            this.chkLineActive.Location = new System.Drawing.Point(13, 19);
            this.chkLineActive.Name = "chkLineActive";
            this.chkLineActive.Size = new System.Drawing.Size(56, 17);
            this.chkLineActive.TabIndex = 0;
            this.chkLineActive.Text = "Active";
            this.chkLineActive.UseVisualStyleBackColor = true;
            this.chkLineActive.CheckedChanged += new System.EventHandler(this.control_Changed);
            // 
            // gbLineParams
            // 
            this.gbLineParams.Controls.Add(this.chkReqAfterCmd);
            this.gbLineParams.Controls.Add(this.lblReqAfterCmd);
            this.gbLineParams.Controls.Add(this.lblDetailedLog);
            this.gbLineParams.Controls.Add(this.chkDetailedLog);
            this.gbLineParams.Controls.Add(this.lblCmdEnabled);
            this.gbLineParams.Controls.Add(this.chkCmdEnabled);
            this.gbLineParams.Controls.Add(this.lblCycleDelay);
            this.gbLineParams.Controls.Add(this.numCycleDelay);
            this.gbLineParams.Controls.Add(this.numReqTriesCnt);
            this.gbLineParams.Controls.Add(this.lblReqTriesCnt);
            this.gbLineParams.Location = new System.Drawing.Point(0, 285);
            this.gbLineParams.Name = "gbLineParams";
            this.gbLineParams.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbLineParams.Size = new System.Drawing.Size(450, 154);
            this.gbLineParams.TabIndex = 2;
            this.gbLineParams.TabStop = false;
            this.gbLineParams.Text = "Communication Parameters";
            // 
            // chkReqAfterCmd
            // 
            this.chkReqAfterCmd.AutoSize = true;
            this.chkReqAfterCmd.Location = new System.Drawing.Point(400, 101);
            this.chkReqAfterCmd.Name = "chkReqAfterCmd";
            this.chkReqAfterCmd.Size = new System.Drawing.Size(15, 14);
            this.chkReqAfterCmd.TabIndex = 7;
            this.chkReqAfterCmd.UseVisualStyleBackColor = true;
            this.chkReqAfterCmd.CheckedChanged += new System.EventHandler(this.control_Changed);
            // 
            // lblReqAfterCmd
            // 
            this.lblReqAfterCmd.AutoSize = true;
            this.lblReqAfterCmd.Location = new System.Drawing.Point(13, 101);
            this.lblReqAfterCmd.Name = "lblReqAfterCmd";
            this.lblReqAfterCmd.Size = new System.Drawing.Size(120, 13);
            this.lblReqAfterCmd.TabIndex = 6;
            this.lblReqAfterCmd.Text = "Request after command";
            // 
            // lblDetailedLog
            // 
            this.lblDetailedLog.AutoSize = true;
            this.lblDetailedLog.Location = new System.Drawing.Point(13, 127);
            this.lblDetailedLog.Name = "lblDetailedLog";
            this.lblDetailedLog.Size = new System.Drawing.Size(63, 13);
            this.lblDetailedLog.TabIndex = 8;
            this.lblDetailedLog.Text = "Detailed log";
            // 
            // chkDetailedLog
            // 
            this.chkDetailedLog.AutoSize = true;
            this.chkDetailedLog.Location = new System.Drawing.Point(400, 127);
            this.chkDetailedLog.Name = "chkDetailedLog";
            this.chkDetailedLog.Size = new System.Drawing.Size(15, 14);
            this.chkDetailedLog.TabIndex = 9;
            this.chkDetailedLog.UseVisualStyleBackColor = true;
            this.chkDetailedLog.CheckedChanged += new System.EventHandler(this.control_Changed);
            // 
            // lblCmdEnabled
            // 
            this.lblCmdEnabled.AutoSize = true;
            this.lblCmdEnabled.Location = new System.Drawing.Point(13, 75);
            this.lblCmdEnabled.Name = "lblCmdEnabled";
            this.lblCmdEnabled.Size = new System.Drawing.Size(100, 13);
            this.lblCmdEnabled.TabIndex = 4;
            this.lblCmdEnabled.Text = "Commands enabled";
            // 
            // chkCmdEnabled
            // 
            this.chkCmdEnabled.AutoSize = true;
            this.chkCmdEnabled.Location = new System.Drawing.Point(400, 75);
            this.chkCmdEnabled.Name = "chkCmdEnabled";
            this.chkCmdEnabled.Size = new System.Drawing.Size(15, 14);
            this.chkCmdEnabled.TabIndex = 5;
            this.chkCmdEnabled.UseVisualStyleBackColor = true;
            this.chkCmdEnabled.CheckedChanged += new System.EventHandler(this.control_Changed);
            // 
            // lblCycleDelay
            // 
            this.lblCycleDelay.AutoSize = true;
            this.lblCycleDelay.Location = new System.Drawing.Point(13, 49);
            this.lblCycleDelay.Name = "lblCycleDelay";
            this.lblCycleDelay.Size = new System.Drawing.Size(143, 13);
            this.lblCycleDelay.TabIndex = 2;
            this.lblCycleDelay.Text = "Delay after request cycle, ms";
            // 
            // numCycleDelay
            // 
            this.numCycleDelay.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numCycleDelay.Location = new System.Drawing.Point(377, 45);
            this.numCycleDelay.Maximum = new decimal(new int[] {
            600000,
            0,
            0,
            0});
            this.numCycleDelay.Name = "numCycleDelay";
            this.numCycleDelay.Size = new System.Drawing.Size(60, 20);
            this.numCycleDelay.TabIndex = 3;
            this.numCycleDelay.ValueChanged += new System.EventHandler(this.control_Changed);
            // 
            // numReqTriesCnt
            // 
            this.numReqTriesCnt.Location = new System.Drawing.Point(377, 19);
            this.numReqTriesCnt.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numReqTriesCnt.Name = "numReqTriesCnt";
            this.numReqTriesCnt.Size = new System.Drawing.Size(60, 20);
            this.numReqTriesCnt.TabIndex = 1;
            this.numReqTriesCnt.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.numReqTriesCnt.ValueChanged += new System.EventHandler(this.control_Changed);
            // 
            // lblReqTriesCnt
            // 
            this.lblReqTriesCnt.AutoSize = true;
            this.lblReqTriesCnt.Location = new System.Drawing.Point(13, 23);
            this.lblReqTriesCnt.Name = "lblReqTriesCnt";
            this.lblReqTriesCnt.Size = new System.Drawing.Size(164, 13);
            this.lblReqTriesCnt.TabIndex = 0;
            this.lblReqTriesCnt.Text = "Number of request retries on error";
            // 
            // gbCommChannel
            // 
            this.gbCommChannel.Controls.Add(this.txtCommCnlParams);
            this.gbCommChannel.Controls.Add(this.lblCommCnlParams);
            this.gbCommChannel.Controls.Add(this.btnCommCnlProps);
            this.gbCommChannel.Controls.Add(this.cbCommCnlType);
            this.gbCommChannel.Controls.Add(this.lblCommCnlType);
            this.gbCommChannel.Location = new System.Drawing.Point(0, 94);
            this.gbCommChannel.Name = "gbCommChannel";
            this.gbCommChannel.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbCommChannel.Size = new System.Drawing.Size(450, 185);
            this.gbCommChannel.TabIndex = 1;
            this.gbCommChannel.TabStop = false;
            this.gbCommChannel.Text = "Communication Channel";
            // 
            // txtCommCnlParams
            // 
            this.txtCommCnlParams.Location = new System.Drawing.Point(13, 72);
            this.txtCommCnlParams.Multiline = true;
            this.txtCommCnlParams.Name = "txtCommCnlParams";
            this.txtCommCnlParams.ReadOnly = true;
            this.txtCommCnlParams.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtCommCnlParams.Size = new System.Drawing.Size(424, 100);
            this.txtCommCnlParams.TabIndex = 4;
            // 
            // lblCommCnlParams
            // 
            this.lblCommCnlParams.AutoSize = true;
            this.lblCommCnlParams.Location = new System.Drawing.Point(10, 56);
            this.lblCommCnlParams.Name = "lblCommCnlParams";
            this.lblCommCnlParams.Size = new System.Drawing.Size(60, 13);
            this.lblCommCnlParams.TabIndex = 3;
            this.lblCommCnlParams.Text = "Parameters";
            // 
            // btnCommCnlProps
            // 
            this.btnCommCnlProps.Location = new System.Drawing.Point(347, 31);
            this.btnCommCnlProps.Name = "btnCommCnlProps";
            this.btnCommCnlProps.Size = new System.Drawing.Size(90, 23);
            this.btnCommCnlProps.TabIndex = 2;
            this.btnCommCnlProps.Text = "Properties";
            this.btnCommCnlProps.UseVisualStyleBackColor = true;
            this.btnCommCnlProps.Click += new System.EventHandler(this.btnCommCnlProps_Click);
            // 
            // cbCommCnlType
            // 
            this.cbCommCnlType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCommCnlType.FormattingEnabled = true;
            this.cbCommCnlType.Items.AddRange(new object[] {
            "Undefined"});
            this.cbCommCnlType.Location = new System.Drawing.Point(13, 32);
            this.cbCommCnlType.Name = "cbCommCnlType";
            this.cbCommCnlType.Size = new System.Drawing.Size(328, 21);
            this.cbCommCnlType.TabIndex = 1;
            this.cbCommCnlType.SelectedIndexChanged += new System.EventHandler(this.cbCommCnlType_SelectedIndexChanged);
            // 
            // lblCommCnlType
            // 
            this.lblCommCnlType.AutoSize = true;
            this.lblCommCnlType.Location = new System.Drawing.Point(10, 16);
            this.lblCommCnlType.Name = "lblCommCnlType";
            this.lblCommCnlType.Size = new System.Drawing.Size(31, 13);
            this.lblCommCnlType.TabIndex = 0;
            this.lblCommCnlType.Text = "Type";
            // 
            // CtrlLineMainParams
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbCommLine);
            this.Controls.Add(this.gbLineParams);
            this.Controls.Add(this.gbCommChannel);
            this.Name = "CtrlLineMainParams";
            this.Size = new System.Drawing.Size(500, 450);
            this.Load += new System.EventHandler(this.CtrlLineMainParams_Load);
            this.gbCommLine.ResumeLayout(false);
            this.gbCommLine.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numLineNumber)).EndInit();
            this.gbLineParams.ResumeLayout(false);
            this.gbLineParams.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCycleDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numReqTriesCnt)).EndInit();
            this.gbCommChannel.ResumeLayout(false);
            this.gbCommChannel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbCommLine;
        private System.Windows.Forms.CheckBox chkLineBound;
        private System.Windows.Forms.NumericUpDown numLineNumber;
        private System.Windows.Forms.TextBox txtLineName;
        private System.Windows.Forms.Label lnlLineName;
        private System.Windows.Forms.Label lblLineNumber;
        private System.Windows.Forms.CheckBox chkLineActive;
        private System.Windows.Forms.GroupBox gbLineParams;
        private System.Windows.Forms.CheckBox chkReqAfterCmd;
        private System.Windows.Forms.Label lblReqAfterCmd;
        private System.Windows.Forms.Label lblDetailedLog;
        private System.Windows.Forms.CheckBox chkDetailedLog;
        private System.Windows.Forms.Label lblCmdEnabled;
        private System.Windows.Forms.CheckBox chkCmdEnabled;
        private System.Windows.Forms.Label lblCycleDelay;
        private System.Windows.Forms.NumericUpDown numCycleDelay;
        private System.Windows.Forms.NumericUpDown numReqTriesCnt;
        private System.Windows.Forms.Label lblReqTriesCnt;
        private System.Windows.Forms.GroupBox gbCommChannel;
        private System.Windows.Forms.TextBox txtCommCnlParams;
        private System.Windows.Forms.Label lblCommCnlParams;
        private System.Windows.Forms.Button btnCommCnlProps;
        private System.Windows.Forms.ComboBox cbCommCnlType;
        private System.Windows.Forms.Label lblCommCnlType;
    }
}
