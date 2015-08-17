namespace Scada.Comm.Channels
{
    partial class FrmCommSerialProps
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.gbPort = new System.Windows.Forms.GroupBox();
            this.lblPortName = new System.Windows.Forms.Label();
            this.chkRtsEnable = new System.Windows.Forms.CheckBox();
            this.chkDtrEnable = new System.Windows.Forms.CheckBox();
            this.cbStopBits = new System.Windows.Forms.ComboBox();
            this.lblStopBits = new System.Windows.Forms.Label();
            this.cbParity = new System.Windows.Forms.ComboBox();
            this.lblParity = new System.Windows.Forms.Label();
            this.cbDataBits = new System.Windows.Forms.ComboBox();
            this.lblDataBits = new System.Windows.Forms.Label();
            this.cbBaudRate = new System.Windows.Forms.ComboBox();
            this.lblBaudRate = new System.Windows.Forms.Label();
            this.cbPortName = new System.Windows.Forms.ComboBox();
            this.gbOperating = new System.Windows.Forms.GroupBox();
            this.cbBehavior = new System.Windows.Forms.ComboBox();
            this.lblBehavior = new System.Windows.Forms.Label();
            this.gbPort.SuspendLayout();
            this.gbOperating.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(201, 284);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(120, 284);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // gbPort
            // 
            this.gbPort.Controls.Add(this.lblPortName);
            this.gbPort.Controls.Add(this.chkRtsEnable);
            this.gbPort.Controls.Add(this.chkDtrEnable);
            this.gbPort.Controls.Add(this.cbStopBits);
            this.gbPort.Controls.Add(this.lblStopBits);
            this.gbPort.Controls.Add(this.cbParity);
            this.gbPort.Controls.Add(this.lblParity);
            this.gbPort.Controls.Add(this.cbDataBits);
            this.gbPort.Controls.Add(this.lblDataBits);
            this.gbPort.Controls.Add(this.cbBaudRate);
            this.gbPort.Controls.Add(this.lblBaudRate);
            this.gbPort.Controls.Add(this.cbPortName);
            this.gbPort.Location = new System.Drawing.Point(12, 12);
            this.gbPort.Name = "gbPort";
            this.gbPort.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbPort.Size = new System.Drawing.Size(264, 207);
            this.gbPort.TabIndex = 0;
            this.gbPort.TabStop = false;
            this.gbPort.Text = "Порт";
            // 
            // lblPortName
            // 
            this.lblPortName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPortName.AutoSize = true;
            this.lblPortName.Location = new System.Drawing.Point(34, 23);
            this.lblPortName.Name = "lblPortName";
            this.lblPortName.Size = new System.Drawing.Size(61, 13);
            this.lblPortName.TabIndex = 0;
            this.lblPortName.Text = "Имя порта";
            this.lblPortName.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // chkRtsEnable
            // 
            this.chkRtsEnable.AutoSize = true;
            this.chkRtsEnable.Location = new System.Drawing.Point(67, 177);
            this.chkRtsEnable.Name = "chkRtsEnable";
            this.chkRtsEnable.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chkRtsEnable.Size = new System.Drawing.Size(48, 17);
            this.chkRtsEnable.TabIndex = 11;
            this.chkRtsEnable.Text = "RTS";
            this.chkRtsEnable.UseVisualStyleBackColor = true;
            // 
            // chkDtrEnable
            // 
            this.chkDtrEnable.AutoSize = true;
            this.chkDtrEnable.Location = new System.Drawing.Point(66, 154);
            this.chkDtrEnable.Name = "chkDtrEnable";
            this.chkDtrEnable.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chkDtrEnable.Size = new System.Drawing.Size(49, 17);
            this.chkDtrEnable.TabIndex = 10;
            this.chkDtrEnable.Text = "DTR";
            this.chkDtrEnable.UseVisualStyleBackColor = true;
            // 
            // cbStopBits
            // 
            this.cbStopBits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbStopBits.FormattingEnabled = true;
            this.cbStopBits.Items.AddRange(new object[] {
            "1",
            "1,5",
            "2"});
            this.cbStopBits.Location = new System.Drawing.Point(101, 127);
            this.cbStopBits.Name = "cbStopBits";
            this.cbStopBits.Size = new System.Drawing.Size(150, 21);
            this.cbStopBits.TabIndex = 9;
            // 
            // lblStopBits
            // 
            this.lblStopBits.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStopBits.AutoSize = true;
            this.lblStopBits.Location = new System.Drawing.Point(33, 130);
            this.lblStopBits.Name = "lblStopBits";
            this.lblStopBits.Size = new System.Drawing.Size(62, 13);
            this.lblStopBits.TabIndex = 8;
            this.lblStopBits.Text = "Стоп. биты";
            this.lblStopBits.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // cbParity
            // 
            this.cbParity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbParity.FormattingEnabled = true;
            this.cbParity.Items.AddRange(new object[] {
            "Чёт",
            "Нечёт",
            "Нет",
            "Маркер (1)",
            "Пробел (0)"});
            this.cbParity.Location = new System.Drawing.Point(101, 100);
            this.cbParity.Name = "cbParity";
            this.cbParity.Size = new System.Drawing.Size(150, 21);
            this.cbParity.TabIndex = 7;
            // 
            // lblParity
            // 
            this.lblParity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblParity.AutoSize = true;
            this.lblParity.Location = new System.Drawing.Point(40, 104);
            this.lblParity.Name = "lblParity";
            this.lblParity.Size = new System.Drawing.Size(55, 13);
            this.lblParity.TabIndex = 6;
            this.lblParity.Text = "Чётность";
            this.lblParity.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // cbDataBits
            // 
            this.cbDataBits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDataBits.FormattingEnabled = true;
            this.cbDataBits.Items.AddRange(new object[] {
            "5",
            "6",
            "7",
            "8"});
            this.cbDataBits.Location = new System.Drawing.Point(101, 73);
            this.cbDataBits.Name = "cbDataBits";
            this.cbDataBits.Size = new System.Drawing.Size(150, 21);
            this.cbDataBits.TabIndex = 5;
            // 
            // lblDataBits
            // 
            this.lblDataBits.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDataBits.AutoSize = true;
            this.lblDataBits.Location = new System.Drawing.Point(22, 77);
            this.lblDataBits.Name = "lblDataBits";
            this.lblDataBits.Size = new System.Drawing.Size(73, 13);
            this.lblDataBits.TabIndex = 4;
            this.lblDataBits.Text = "Биты данных";
            this.lblDataBits.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // cbBaudRate
            // 
            this.cbBaudRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBaudRate.FormattingEnabled = true;
            this.cbBaudRate.Items.AddRange(new object[] {
            "110",
            "300",
            "1200",
            "2400",
            "4800",
            "9600",
            "19200",
            "38400",
            "57600",
            "115200",
            "230400",
            "460800",
            "921600"});
            this.cbBaudRate.Location = new System.Drawing.Point(101, 46);
            this.cbBaudRate.Name = "cbBaudRate";
            this.cbBaudRate.Size = new System.Drawing.Size(150, 21);
            this.cbBaudRate.TabIndex = 3;
            // 
            // lblBaudRate
            // 
            this.lblBaudRate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblBaudRate.AutoSize = true;
            this.lblBaudRate.Location = new System.Drawing.Point(40, 50);
            this.lblBaudRate.Name = "lblBaudRate";
            this.lblBaudRate.Size = new System.Drawing.Size(55, 13);
            this.lblBaudRate.TabIndex = 2;
            this.lblBaudRate.Text = "Скорость";
            this.lblBaudRate.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // cbPortName
            // 
            this.cbPortName.FormattingEnabled = true;
            this.cbPortName.Items.AddRange(new object[] {
            "COM1",
            "COM2",
            "COM3",
            "COM4",
            "COM5",
            "COM6",
            "COM7",
            "COM8",
            "COM9",
            "COM10"});
            this.cbPortName.Location = new System.Drawing.Point(101, 19);
            this.cbPortName.Name = "cbPortName";
            this.cbPortName.Size = new System.Drawing.Size(150, 21);
            this.cbPortName.TabIndex = 1;
            this.cbPortName.Text = "COM1";
            // 
            // gbOperating
            // 
            this.gbOperating.Controls.Add(this.cbBehavior);
            this.gbOperating.Controls.Add(this.lblBehavior);
            this.gbOperating.Location = new System.Drawing.Point(12, 225);
            this.gbOperating.Name = "gbOperating";
            this.gbOperating.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbOperating.Size = new System.Drawing.Size(264, 53);
            this.gbOperating.TabIndex = 1;
            this.gbOperating.TabStop = false;
            this.gbOperating.Text = "Работа";
            // 
            // cbBehavior
            // 
            this.cbBehavior.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBehavior.FormattingEnabled = true;
            this.cbBehavior.Items.AddRange(new object[] {
            "Master",
            "Slave"});
            this.cbBehavior.Location = new System.Drawing.Point(101, 19);
            this.cbBehavior.Name = "cbBehavior";
            this.cbBehavior.Size = new System.Drawing.Size(150, 21);
            this.cbBehavior.TabIndex = 1;
            // 
            // lblBehavior
            // 
            this.lblBehavior.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblBehavior.AutoSize = true;
            this.lblBehavior.Location = new System.Drawing.Point(13, 23);
            this.lblBehavior.Name = "lblBehavior";
            this.lblBehavior.Size = new System.Drawing.Size(82, 13);
            this.lblBehavior.TabIndex = 0;
            this.lblBehavior.Text = "Режим работы";
            this.lblBehavior.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // FrmCommSerialProps
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(288, 319);
            this.Controls.Add(this.gbOperating);
            this.Controls.Add(this.gbPort);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmCommSerialProps";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Свойства: Последовательный порт";
            this.Load += new System.EventHandler(this.FrmCommSerialProps_Load);
            this.gbPort.ResumeLayout(false);
            this.gbPort.PerformLayout();
            this.gbOperating.ResumeLayout(false);
            this.gbOperating.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.GroupBox gbPort;
        private System.Windows.Forms.Label lblPortName;
        private System.Windows.Forms.CheckBox chkRtsEnable;
        private System.Windows.Forms.CheckBox chkDtrEnable;
        private System.Windows.Forms.ComboBox cbStopBits;
        private System.Windows.Forms.Label lblStopBits;
        private System.Windows.Forms.ComboBox cbParity;
        private System.Windows.Forms.Label lblParity;
        private System.Windows.Forms.ComboBox cbDataBits;
        private System.Windows.Forms.Label lblDataBits;
        private System.Windows.Forms.ComboBox cbBaudRate;
        private System.Windows.Forms.Label lblBaudRate;
        private System.Windows.Forms.ComboBox cbPortName;
        private System.Windows.Forms.GroupBox gbOperating;
        private System.Windows.Forms.Label lblBehavior;
        private System.Windows.Forms.ComboBox cbBehavior;

    }
}