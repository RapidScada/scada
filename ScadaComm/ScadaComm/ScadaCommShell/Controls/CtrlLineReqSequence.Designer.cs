namespace Scada.Comm.Shell.Controls
{
    partial class CtrlLineReqSequence
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
            this.components = new System.ComponentModel.Container();
            this.gbSelectedDevice = new System.Windows.Forms.GroupBox();
            this.btnDeviceProps = new System.Windows.Forms.Button();
            this.btnResetReqParams = new System.Windows.Forms.Button();
            this.cbDeviceDll = new System.Windows.Forms.ComboBox();
            this.txtDeviceCmdLine = new System.Windows.Forms.TextBox();
            this.dtpDevicePeriod = new System.Windows.Forms.DateTimePicker();
            this.dtpDeviceTime = new System.Windows.Forms.DateTimePicker();
            this.lblDeviceCmdLine = new System.Windows.Forms.Label();
            this.lblDevicePeriod = new System.Windows.Forms.Label();
            this.lblDeviceTime = new System.Windows.Forms.Label();
            this.numDeviceDelay = new System.Windows.Forms.NumericUpDown();
            this.lblDeviceDelay = new System.Windows.Forms.Label();
            this.lblDeviceTimeout = new System.Windows.Forms.Label();
            this.numDeviceTimeout = new System.Windows.Forms.NumericUpDown();
            this.lblDeviceCallNum = new System.Windows.Forms.Label();
            this.txtDeviceCallNum = new System.Windows.Forms.TextBox();
            this.lblDeviceAddress = new System.Windows.Forms.Label();
            this.numDeviceAddress = new System.Windows.Forms.NumericUpDown();
            this.lblDeviceDll = new System.Windows.Forms.Label();
            this.lblDeviceName = new System.Windows.Forms.Label();
            this.numDeviceNumber = new System.Windows.Forms.NumericUpDown();
            this.lblDeviceNumber = new System.Windows.Forms.Label();
            this.txtDeviceName = new System.Windows.Forms.TextBox();
            this.chkDeviceBound = new System.Windows.Forms.CheckBox();
            this.chkDeviceActive = new System.Windows.Forms.CheckBox();
            this.lvReqSequence = new System.Windows.Forms.ListView();
            this.colDeviceOrder = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDeviceActive = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDeviceBound = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDeviceNumber = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDeviceName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDeviceDll = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDeviceAddress = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDeviceCallNum = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDeviceTimeout = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDeviceDelay = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDeviceTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDevicePeriod = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDeviceCmdLine = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnAddDevice = new System.Windows.Forms.Button();
            this.btnMoveUpDevice = new System.Windows.Forms.Button();
            this.btnMoveDownDevice = new System.Windows.Forms.Button();
            this.btnDeleteDevice = new System.Windows.Forms.Button();
            this.btnPasteDevice = new System.Windows.Forms.Button();
            this.btnCopyDevice = new System.Windows.Forms.Button();
            this.btnCutDevice = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.gbSelectedDevice.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDeviceDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDeviceTimeout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDeviceAddress)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDeviceNumber)).BeginInit();
            this.SuspendLayout();
            // 
            // gbSelectedDevice
            // 
            this.gbSelectedDevice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.gbSelectedDevice.Controls.Add(this.btnDeviceProps);
            this.gbSelectedDevice.Controls.Add(this.btnResetReqParams);
            this.gbSelectedDevice.Controls.Add(this.cbDeviceDll);
            this.gbSelectedDevice.Controls.Add(this.txtDeviceCmdLine);
            this.gbSelectedDevice.Controls.Add(this.dtpDevicePeriod);
            this.gbSelectedDevice.Controls.Add(this.dtpDeviceTime);
            this.gbSelectedDevice.Controls.Add(this.lblDeviceCmdLine);
            this.gbSelectedDevice.Controls.Add(this.lblDevicePeriod);
            this.gbSelectedDevice.Controls.Add(this.lblDeviceTime);
            this.gbSelectedDevice.Controls.Add(this.numDeviceDelay);
            this.gbSelectedDevice.Controls.Add(this.lblDeviceDelay);
            this.gbSelectedDevice.Controls.Add(this.lblDeviceTimeout);
            this.gbSelectedDevice.Controls.Add(this.numDeviceTimeout);
            this.gbSelectedDevice.Controls.Add(this.lblDeviceCallNum);
            this.gbSelectedDevice.Controls.Add(this.txtDeviceCallNum);
            this.gbSelectedDevice.Controls.Add(this.lblDeviceAddress);
            this.gbSelectedDevice.Controls.Add(this.numDeviceAddress);
            this.gbSelectedDevice.Controls.Add(this.lblDeviceDll);
            this.gbSelectedDevice.Controls.Add(this.lblDeviceName);
            this.gbSelectedDevice.Controls.Add(this.numDeviceNumber);
            this.gbSelectedDevice.Controls.Add(this.lblDeviceNumber);
            this.gbSelectedDevice.Controls.Add(this.txtDeviceName);
            this.gbSelectedDevice.Controls.Add(this.chkDeviceBound);
            this.gbSelectedDevice.Controls.Add(this.chkDeviceActive);
            this.gbSelectedDevice.Location = new System.Drawing.Point(9, 204);
            this.gbSelectedDevice.Margin = new System.Windows.Forms.Padding(9, 3, 3, 12);
            this.gbSelectedDevice.Name = "gbSelectedDevice";
            this.gbSelectedDevice.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbSelectedDevice.Size = new System.Drawing.Size(450, 234);
            this.gbSelectedDevice.TabIndex = 8;
            this.gbSelectedDevice.TabStop = false;
            this.gbSelectedDevice.Text = "Selected Device";
            // 
            // btnDeviceProps
            // 
            this.btnDeviceProps.Location = new System.Drawing.Point(94, 198);
            this.btnDeviceProps.Name = "btnDeviceProps";
            this.btnDeviceProps.Size = new System.Drawing.Size(75, 23);
            this.btnDeviceProps.TabIndex = 23;
            this.btnDeviceProps.Text = "Properies";
            this.btnDeviceProps.UseVisualStyleBackColor = true;
            this.btnDeviceProps.Click += new System.EventHandler(this.btnDeviceProps_Click);
            // 
            // btnResetReqParams
            // 
            this.btnResetReqParams.Location = new System.Drawing.Point(13, 198);
            this.btnResetReqParams.Name = "btnResetReqParams";
            this.btnResetReqParams.Size = new System.Drawing.Size(75, 23);
            this.btnResetReqParams.TabIndex = 22;
            this.btnResetReqParams.Text = "Reset";
            this.toolTip.SetToolTip(this.btnResetReqParams, "Set the device request parameters to default");
            this.btnResetReqParams.UseVisualStyleBackColor = true;
            this.btnResetReqParams.Click += new System.EventHandler(this.btnResetReqParams_Click);
            // 
            // cbDeviceDll
            // 
            this.cbDeviceDll.FormattingEnabled = true;
            this.cbDeviceDll.Location = new System.Drawing.Point(317, 55);
            this.cbDeviceDll.Name = "cbDeviceDll";
            this.cbDeviceDll.Size = new System.Drawing.Size(120, 21);
            this.cbDeviceDll.TabIndex = 7;
            this.cbDeviceDll.TextChanged += new System.EventHandler(this.cbDeviceDll_TextChanged);
            // 
            // txtDeviceCmdLine
            // 
            this.txtDeviceCmdLine.Location = new System.Drawing.Point(13, 172);
            this.txtDeviceCmdLine.Name = "txtDeviceCmdLine";
            this.txtDeviceCmdLine.Size = new System.Drawing.Size(424, 20);
            this.txtDeviceCmdLine.TabIndex = 21;
            this.txtDeviceCmdLine.TextChanged += new System.EventHandler(this.txtDeviceCmdLine_TextChanged);
            // 
            // dtpDevicePeriod
            // 
            this.dtpDevicePeriod.CustomFormat = "HH:mm:ss";
            this.dtpDevicePeriod.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDevicePeriod.Location = new System.Drawing.Point(317, 133);
            this.dtpDevicePeriod.Name = "dtpDevicePeriod";
            this.dtpDevicePeriod.ShowUpDown = true;
            this.dtpDevicePeriod.Size = new System.Drawing.Size(120, 20);
            this.dtpDevicePeriod.TabIndex = 19;
            this.dtpDevicePeriod.Value = new System.DateTime(2018, 1, 1, 0, 1, 0, 0);
            this.dtpDevicePeriod.ValueChanged += new System.EventHandler(this.dtpDevicePeriod_ValueChanged);
            // 
            // dtpDeviceTime
            // 
            this.dtpDeviceTime.CustomFormat = "HH:mm:ss";
            this.dtpDeviceTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDeviceTime.Location = new System.Drawing.Point(191, 133);
            this.dtpDeviceTime.Name = "dtpDeviceTime";
            this.dtpDeviceTime.ShowUpDown = true;
            this.dtpDeviceTime.Size = new System.Drawing.Size(120, 20);
            this.dtpDeviceTime.TabIndex = 17;
            this.dtpDeviceTime.Value = new System.DateTime(2018, 1, 1, 10, 0, 0, 0);
            this.dtpDeviceTime.ValueChanged += new System.EventHandler(this.dtpDeviceTime_ValueChanged);
            // 
            // lblDeviceCmdLine
            // 
            this.lblDeviceCmdLine.AutoSize = true;
            this.lblDeviceCmdLine.Location = new System.Drawing.Point(10, 156);
            this.lblDeviceCmdLine.Name = "lblDeviceCmdLine";
            this.lblDeviceCmdLine.Size = new System.Drawing.Size(73, 13);
            this.lblDeviceCmdLine.TabIndex = 20;
            this.lblDeviceCmdLine.Text = "Command line";
            // 
            // lblDevicePeriod
            // 
            this.lblDevicePeriod.AutoSize = true;
            this.lblDevicePeriod.Location = new System.Drawing.Point(314, 117);
            this.lblDevicePeriod.Name = "lblDevicePeriod";
            this.lblDevicePeriod.Size = new System.Drawing.Size(37, 13);
            this.lblDevicePeriod.TabIndex = 18;
            this.lblDevicePeriod.Text = "Period";
            // 
            // lblDeviceTime
            // 
            this.lblDeviceTime.AutoSize = true;
            this.lblDeviceTime.Location = new System.Drawing.Point(188, 117);
            this.lblDeviceTime.Name = "lblDeviceTime";
            this.lblDeviceTime.Size = new System.Drawing.Size(30, 13);
            this.lblDeviceTime.TabIndex = 16;
            this.lblDeviceTime.Text = "Time";
            // 
            // numDeviceDelay
            // 
            this.numDeviceDelay.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numDeviceDelay.Location = new System.Drawing.Point(94, 133);
            this.numDeviceDelay.Maximum = new decimal(new int[] {
            600000,
            0,
            0,
            0});
            this.numDeviceDelay.Name = "numDeviceDelay";
            this.numDeviceDelay.Size = new System.Drawing.Size(91, 20);
            this.numDeviceDelay.TabIndex = 15;
            this.numDeviceDelay.ValueChanged += new System.EventHandler(this.numDeviceDelay_ValueChanged);
            // 
            // lblDeviceDelay
            // 
            this.lblDeviceDelay.AutoSize = true;
            this.lblDeviceDelay.Location = new System.Drawing.Point(91, 117);
            this.lblDeviceDelay.Name = "lblDeviceDelay";
            this.lblDeviceDelay.Size = new System.Drawing.Size(34, 13);
            this.lblDeviceDelay.TabIndex = 14;
            this.lblDeviceDelay.Text = "Delay";
            // 
            // lblDeviceTimeout
            // 
            this.lblDeviceTimeout.AutoSize = true;
            this.lblDeviceTimeout.Location = new System.Drawing.Point(10, 117);
            this.lblDeviceTimeout.Name = "lblDeviceTimeout";
            this.lblDeviceTimeout.Size = new System.Drawing.Size(45, 13);
            this.lblDeviceTimeout.TabIndex = 12;
            this.lblDeviceTimeout.Text = "Timeout";
            // 
            // numDeviceTimeout
            // 
            this.numDeviceTimeout.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numDeviceTimeout.Location = new System.Drawing.Point(13, 133);
            this.numDeviceTimeout.Maximum = new decimal(new int[] {
            600000,
            0,
            0,
            0});
            this.numDeviceTimeout.Name = "numDeviceTimeout";
            this.numDeviceTimeout.Size = new System.Drawing.Size(75, 20);
            this.numDeviceTimeout.TabIndex = 13;
            this.numDeviceTimeout.ValueChanged += new System.EventHandler(this.numDeviceTimeout_ValueChanged);
            // 
            // lblDeviceCallNum
            // 
            this.lblDeviceCallNum.AutoSize = true;
            this.lblDeviceCallNum.Location = new System.Drawing.Point(91, 78);
            this.lblDeviceCallNum.Name = "lblDeviceCallNum";
            this.lblDeviceCallNum.Size = new System.Drawing.Size(126, 13);
            this.lblDeviceCallNum.TabIndex = 10;
            this.lblDeviceCallNum.Text = "Call number or host name";
            // 
            // txtDeviceCallNum
            // 
            this.txtDeviceCallNum.Location = new System.Drawing.Point(94, 94);
            this.txtDeviceCallNum.Name = "txtDeviceCallNum";
            this.txtDeviceCallNum.Size = new System.Drawing.Size(343, 20);
            this.txtDeviceCallNum.TabIndex = 11;
            this.txtDeviceCallNum.TextChanged += new System.EventHandler(this.txtDeviceCallNum_TextChanged);
            // 
            // lblDeviceAddress
            // 
            this.lblDeviceAddress.AutoSize = true;
            this.lblDeviceAddress.Location = new System.Drawing.Point(10, 78);
            this.lblDeviceAddress.Name = "lblDeviceAddress";
            this.lblDeviceAddress.Size = new System.Drawing.Size(45, 13);
            this.lblDeviceAddress.TabIndex = 8;
            this.lblDeviceAddress.Text = "Address";
            // 
            // numDeviceAddress
            // 
            this.numDeviceAddress.Location = new System.Drawing.Point(13, 94);
            this.numDeviceAddress.Name = "numDeviceAddress";
            this.numDeviceAddress.Size = new System.Drawing.Size(75, 20);
            this.numDeviceAddress.TabIndex = 9;
            this.numDeviceAddress.ValueChanged += new System.EventHandler(this.numDeviceAddress_ValueChanged);
            // 
            // lblDeviceDll
            // 
            this.lblDeviceDll.AutoSize = true;
            this.lblDeviceDll.Location = new System.Drawing.Point(314, 39);
            this.lblDeviceDll.Name = "lblDeviceDll";
            this.lblDeviceDll.Size = new System.Drawing.Size(35, 13);
            this.lblDeviceDll.TabIndex = 6;
            this.lblDeviceDll.Text = "Driver";
            // 
            // lblDeviceName
            // 
            this.lblDeviceName.AutoSize = true;
            this.lblDeviceName.Location = new System.Drawing.Point(91, 39);
            this.lblDeviceName.Name = "lblDeviceName";
            this.lblDeviceName.Size = new System.Drawing.Size(35, 13);
            this.lblDeviceName.TabIndex = 4;
            this.lblDeviceName.Text = "Name";
            // 
            // numDeviceNumber
            // 
            this.numDeviceNumber.Location = new System.Drawing.Point(13, 55);
            this.numDeviceNumber.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numDeviceNumber.Name = "numDeviceNumber";
            this.numDeviceNumber.Size = new System.Drawing.Size(75, 20);
            this.numDeviceNumber.TabIndex = 3;
            this.numDeviceNumber.ValueChanged += new System.EventHandler(this.numDeviceNumber_ValueChanged);
            // 
            // lblDeviceNumber
            // 
            this.lblDeviceNumber.AutoSize = true;
            this.lblDeviceNumber.Location = new System.Drawing.Point(10, 39);
            this.lblDeviceNumber.Name = "lblDeviceNumber";
            this.lblDeviceNumber.Size = new System.Drawing.Size(44, 13);
            this.lblDeviceNumber.TabIndex = 2;
            this.lblDeviceNumber.Text = "Number";
            // 
            // txtDeviceName
            // 
            this.txtDeviceName.Location = new System.Drawing.Point(94, 55);
            this.txtDeviceName.Name = "txtDeviceName";
            this.txtDeviceName.Size = new System.Drawing.Size(217, 20);
            this.txtDeviceName.TabIndex = 5;
            this.txtDeviceName.TextChanged += new System.EventHandler(this.txtDeviceName_TextChanged);
            // 
            // chkDeviceBound
            // 
            this.chkDeviceBound.AutoSize = true;
            this.chkDeviceBound.Location = new System.Drawing.Point(94, 19);
            this.chkDeviceBound.Name = "chkDeviceBound";
            this.chkDeviceBound.Size = new System.Drawing.Size(103, 17);
            this.chkDeviceBound.TabIndex = 1;
            this.chkDeviceBound.Text = "Bound to Server";
            this.chkDeviceBound.UseVisualStyleBackColor = true;
            this.chkDeviceBound.CheckedChanged += new System.EventHandler(this.chkDeviceBound_CheckedChanged);
            // 
            // chkDeviceActive
            // 
            this.chkDeviceActive.AutoSize = true;
            this.chkDeviceActive.Location = new System.Drawing.Point(13, 19);
            this.chkDeviceActive.Name = "chkDeviceActive";
            this.chkDeviceActive.Size = new System.Drawing.Size(56, 17);
            this.chkDeviceActive.TabIndex = 0;
            this.chkDeviceActive.Text = "Active";
            this.chkDeviceActive.UseVisualStyleBackColor = true;
            this.chkDeviceActive.CheckedChanged += new System.EventHandler(this.chkDeviceActive_CheckedChanged);
            // 
            // lvReqSequence
            // 
            this.lvReqSequence.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvReqSequence.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colDeviceOrder,
            this.colDeviceActive,
            this.colDeviceBound,
            this.colDeviceNumber,
            this.colDeviceName,
            this.colDeviceDll,
            this.colDeviceAddress,
            this.colDeviceCallNum,
            this.colDeviceTimeout,
            this.colDeviceDelay,
            this.colDeviceTime,
            this.colDevicePeriod,
            this.colDeviceCmdLine});
            this.lvReqSequence.FullRowSelect = true;
            this.lvReqSequence.GridLines = true;
            this.lvReqSequence.HideSelection = false;
            this.lvReqSequence.Location = new System.Drawing.Point(9, 41);
            this.lvReqSequence.Margin = new System.Windows.Forms.Padding(9, 3, 12, 3);
            this.lvReqSequence.MultiSelect = false;
            this.lvReqSequence.Name = "lvReqSequence";
            this.lvReqSequence.ShowItemToolTips = true;
            this.lvReqSequence.Size = new System.Drawing.Size(679, 157);
            this.lvReqSequence.TabIndex = 7;
            this.lvReqSequence.UseCompatibleStateImageBehavior = false;
            this.lvReqSequence.View = System.Windows.Forms.View.Details;
            this.lvReqSequence.SelectedIndexChanged += new System.EventHandler(this.lvReqSequence_SelectedIndexChanged);
            // 
            // colDeviceOrder
            // 
            this.colDeviceOrder.Text = "#";
            this.colDeviceOrder.Width = 40;
            // 
            // colDeviceActive
            // 
            this.colDeviceActive.Text = "Active";
            this.colDeviceActive.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.colDeviceActive.Width = 50;
            // 
            // colDeviceBound
            // 
            this.colDeviceBound.Text = "Bound";
            this.colDeviceBound.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.colDeviceBound.Width = 50;
            // 
            // colDeviceNumber
            // 
            this.colDeviceNumber.Text = "Number";
            this.colDeviceNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.colDeviceNumber.Width = 50;
            // 
            // colDeviceName
            // 
            this.colDeviceName.Text = "Name";
            this.colDeviceName.Width = 90;
            // 
            // colDeviceDll
            // 
            this.colDeviceDll.Text = "DLL";
            // 
            // colDeviceAddress
            // 
            this.colDeviceAddress.Text = "Address";
            this.colDeviceAddress.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.colDeviceAddress.Width = 50;
            // 
            // colDeviceCallNum
            // 
            this.colDeviceCallNum.Text = "Call Number";
            this.colDeviceCallNum.Width = 70;
            // 
            // colDeviceTimeout
            // 
            this.colDeviceTimeout.Text = "Timeout";
            this.colDeviceTimeout.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // colDeviceDelay
            // 
            this.colDeviceDelay.Text = "Delay";
            this.colDeviceDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // colDeviceTime
            // 
            this.colDeviceTime.Text = "Time";
            this.colDeviceTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.colDeviceTime.Width = 75;
            // 
            // colDevicePeriod
            // 
            this.colDevicePeriod.Text = "Period";
            this.colDevicePeriod.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // colDeviceCmdLine
            // 
            this.colDeviceCmdLine.Text = "Command Line";
            this.colDeviceCmdLine.Width = 120;
            // 
            // btnAddDevice
            // 
            this.btnAddDevice.Location = new System.Drawing.Point(9, 12);
            this.btnAddDevice.Margin = new System.Windows.Forms.Padding(9, 12, 3, 3);
            this.btnAddDevice.Name = "btnAddDevice";
            this.btnAddDevice.Size = new System.Drawing.Size(75, 23);
            this.btnAddDevice.TabIndex = 0;
            this.btnAddDevice.Text = "Add";
            this.btnAddDevice.UseVisualStyleBackColor = true;
            this.btnAddDevice.Click += new System.EventHandler(this.btnAddDevice_Click);
            // 
            // btnMoveUpDevice
            // 
            this.btnMoveUpDevice.Location = new System.Drawing.Point(90, 12);
            this.btnMoveUpDevice.Name = "btnMoveUpDevice";
            this.btnMoveUpDevice.Size = new System.Drawing.Size(75, 23);
            this.btnMoveUpDevice.TabIndex = 1;
            this.btnMoveUpDevice.Text = "Move Up";
            this.btnMoveUpDevice.UseVisualStyleBackColor = true;
            this.btnMoveUpDevice.Click += new System.EventHandler(this.btnMoveUpDevice_Click);
            // 
            // btnMoveDownDevice
            // 
            this.btnMoveDownDevice.Location = new System.Drawing.Point(171, 12);
            this.btnMoveDownDevice.Name = "btnMoveDownDevice";
            this.btnMoveDownDevice.Size = new System.Drawing.Size(75, 23);
            this.btnMoveDownDevice.TabIndex = 2;
            this.btnMoveDownDevice.Text = "Move Down";
            this.btnMoveDownDevice.UseVisualStyleBackColor = true;
            this.btnMoveDownDevice.Click += new System.EventHandler(this.btnMoveDownDevice_Click);
            // 
            // btnDeleteDevice
            // 
            this.btnDeleteDevice.Location = new System.Drawing.Point(252, 12);
            this.btnDeleteDevice.Name = "btnDeleteDevice";
            this.btnDeleteDevice.Size = new System.Drawing.Size(75, 23);
            this.btnDeleteDevice.TabIndex = 3;
            this.btnDeleteDevice.Text = "Delete";
            this.btnDeleteDevice.UseVisualStyleBackColor = true;
            this.btnDeleteDevice.Click += new System.EventHandler(this.btnDeleteDevice_Click);
            // 
            // btnPasteDevice
            // 
            this.btnPasteDevice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPasteDevice.Location = new System.Drawing.Point(613, 12);
            this.btnPasteDevice.Margin = new System.Windows.Forms.Padding(3, 3, 12, 3);
            this.btnPasteDevice.Name = "btnPasteDevice";
            this.btnPasteDevice.Size = new System.Drawing.Size(75, 23);
            this.btnPasteDevice.TabIndex = 6;
            this.btnPasteDevice.Text = "Paste";
            this.btnPasteDevice.UseVisualStyleBackColor = true;
            this.btnPasteDevice.Click += new System.EventHandler(this.btnPasteDevice_Click);
            // 
            // btnCopyDevice
            // 
            this.btnCopyDevice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCopyDevice.Location = new System.Drawing.Point(532, 12);
            this.btnCopyDevice.Name = "btnCopyDevice";
            this.btnCopyDevice.Size = new System.Drawing.Size(75, 23);
            this.btnCopyDevice.TabIndex = 5;
            this.btnCopyDevice.Text = "Copy";
            this.btnCopyDevice.UseVisualStyleBackColor = true;
            this.btnCopyDevice.Click += new System.EventHandler(this.btnCopyDevice_Click);
            // 
            // btnCutDevice
            // 
            this.btnCutDevice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCutDevice.Location = new System.Drawing.Point(451, 12);
            this.btnCutDevice.Name = "btnCutDevice";
            this.btnCutDevice.Size = new System.Drawing.Size(75, 23);
            this.btnCutDevice.TabIndex = 4;
            this.btnCutDevice.Text = "Cut";
            this.btnCutDevice.UseVisualStyleBackColor = true;
            this.btnCutDevice.Click += new System.EventHandler(this.btnCutDevice_Click);
            // 
            // CtrlLineReqSequence
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnPasteDevice);
            this.Controls.Add(this.btnCopyDevice);
            this.Controls.Add(this.btnCutDevice);
            this.Controls.Add(this.btnDeleteDevice);
            this.Controls.Add(this.btnMoveDownDevice);
            this.Controls.Add(this.btnMoveUpDevice);
            this.Controls.Add(this.btnAddDevice);
            this.Controls.Add(this.gbSelectedDevice);
            this.Controls.Add(this.lvReqSequence);
            this.Name = "CtrlLineReqSequence";
            this.Size = new System.Drawing.Size(700, 450);
            this.Load += new System.EventHandler(this.CtrlLineReqSequence_Load);
            this.gbSelectedDevice.ResumeLayout(false);
            this.gbSelectedDevice.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDeviceDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDeviceTimeout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDeviceAddress)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDeviceNumber)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbSelectedDevice;
        private System.Windows.Forms.ComboBox cbDeviceDll;
        private System.Windows.Forms.TextBox txtDeviceCmdLine;
        private System.Windows.Forms.DateTimePicker dtpDevicePeriod;
        private System.Windows.Forms.DateTimePicker dtpDeviceTime;
        private System.Windows.Forms.Label lblDeviceCmdLine;
        private System.Windows.Forms.Label lblDevicePeriod;
        private System.Windows.Forms.Label lblDeviceTime;
        private System.Windows.Forms.NumericUpDown numDeviceDelay;
        private System.Windows.Forms.Label lblDeviceDelay;
        private System.Windows.Forms.Label lblDeviceTimeout;
        private System.Windows.Forms.NumericUpDown numDeviceTimeout;
        private System.Windows.Forms.Label lblDeviceCallNum;
        private System.Windows.Forms.TextBox txtDeviceCallNum;
        private System.Windows.Forms.Label lblDeviceAddress;
        private System.Windows.Forms.NumericUpDown numDeviceAddress;
        private System.Windows.Forms.Label lblDeviceDll;
        private System.Windows.Forms.Label lblDeviceName;
        private System.Windows.Forms.NumericUpDown numDeviceNumber;
        private System.Windows.Forms.Label lblDeviceNumber;
        private System.Windows.Forms.TextBox txtDeviceName;
        private System.Windows.Forms.CheckBox chkDeviceBound;
        private System.Windows.Forms.CheckBox chkDeviceActive;
        private System.Windows.Forms.ListView lvReqSequence;
        private System.Windows.Forms.ColumnHeader colDeviceOrder;
        private System.Windows.Forms.ColumnHeader colDeviceActive;
        private System.Windows.Forms.ColumnHeader colDeviceBound;
        private System.Windows.Forms.ColumnHeader colDeviceNumber;
        private System.Windows.Forms.ColumnHeader colDeviceName;
        private System.Windows.Forms.ColumnHeader colDeviceDll;
        private System.Windows.Forms.ColumnHeader colDeviceAddress;
        private System.Windows.Forms.ColumnHeader colDeviceCallNum;
        private System.Windows.Forms.ColumnHeader colDeviceTimeout;
        private System.Windows.Forms.ColumnHeader colDeviceDelay;
        private System.Windows.Forms.ColumnHeader colDeviceTime;
        private System.Windows.Forms.ColumnHeader colDevicePeriod;
        private System.Windows.Forms.ColumnHeader colDeviceCmdLine;
        private System.Windows.Forms.Button btnAddDevice;
        private System.Windows.Forms.Button btnMoveUpDevice;
        private System.Windows.Forms.Button btnMoveDownDevice;
        private System.Windows.Forms.Button btnDeleteDevice;
        private System.Windows.Forms.Button btnPasteDevice;
        private System.Windows.Forms.Button btnCopyDevice;
        private System.Windows.Forms.Button btnCutDevice;
        private System.Windows.Forms.Button btnDeviceProps;
        private System.Windows.Forms.Button btnResetReqParams;
        internal System.Windows.Forms.ToolTip toolTip;
    }
}
