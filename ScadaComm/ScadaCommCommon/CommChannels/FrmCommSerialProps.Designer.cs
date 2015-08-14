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
            this.gbConnection = new System.Windows.Forms.GroupBox();
            this.cbConnType = new System.Windows.Forms.ComboBox();
            this.lblConnType = new System.Windows.Forms.Label();
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
            this.gbConnection.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbConnection
            // 
            this.gbConnection.Controls.Add(this.cbConnType);
            this.gbConnection.Controls.Add(this.lblConnType);
            this.gbConnection.Controls.Add(this.lblPortName);
            this.gbConnection.Controls.Add(this.chkRtsEnable);
            this.gbConnection.Controls.Add(this.chkDtrEnable);
            this.gbConnection.Controls.Add(this.cbStopBits);
            this.gbConnection.Controls.Add(this.lblStopBits);
            this.gbConnection.Controls.Add(this.cbParity);
            this.gbConnection.Controls.Add(this.lblParity);
            this.gbConnection.Controls.Add(this.cbDataBits);
            this.gbConnection.Controls.Add(this.lblDataBits);
            this.gbConnection.Controls.Add(this.cbBaudRate);
            this.gbConnection.Controls.Add(this.lblBaudRate);
            this.gbConnection.Controls.Add(this.cbPortName);
            this.gbConnection.Location = new System.Drawing.Point(12, 12);
            this.gbConnection.Name = "gbConnection";
            this.gbConnection.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbConnection.Size = new System.Drawing.Size(392, 106);
            this.gbConnection.TabIndex = 2;
            this.gbConnection.TabStop = false;
            this.gbConnection.Text = "Подключение";
            // 
            // cbConnType
            // 
            this.cbConnType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbConnType.FormattingEnabled = true;
            this.cbConnType.Items.AddRange(new object[] {
            "Не задан",
            "COM-порт"});
            this.cbConnType.Location = new System.Drawing.Point(13, 32);
            this.cbConnType.Name = "cbConnType";
            this.cbConnType.Size = new System.Drawing.Size(80, 21);
            this.cbConnType.TabIndex = 1;
            // 
            // lblConnType
            // 
            this.lblConnType.AutoSize = true;
            this.lblConnType.Location = new System.Drawing.Point(10, 16);
            this.lblConnType.Name = "lblConnType";
            this.lblConnType.Size = new System.Drawing.Size(26, 13);
            this.lblConnType.TabIndex = 0;
            this.lblConnType.Text = "Тип";
            // 
            // lblPortName
            // 
            this.lblPortName.AutoSize = true;
            this.lblPortName.Location = new System.Drawing.Point(98, 16);
            this.lblPortName.Name = "lblPortName";
            this.lblPortName.Size = new System.Drawing.Size(32, 13);
            this.lblPortName.TabIndex = 2;
            this.lblPortName.Text = "Порт";
            // 
            // chkRtsEnable
            // 
            this.chkRtsEnable.AutoSize = true;
            this.chkRtsEnable.Location = new System.Drawing.Point(287, 79);
            this.chkRtsEnable.Name = "chkRtsEnable";
            this.chkRtsEnable.Size = new System.Drawing.Size(48, 17);
            this.chkRtsEnable.TabIndex = 13;
            this.chkRtsEnable.Text = "RTS";
            this.chkRtsEnable.UseVisualStyleBackColor = true;
            // 
            // chkDtrEnable
            // 
            this.chkDtrEnable.AutoSize = true;
            this.chkDtrEnable.Location = new System.Drawing.Point(287, 59);
            this.chkDtrEnable.Name = "chkDtrEnable";
            this.chkDtrEnable.Size = new System.Drawing.Size(49, 17);
            this.chkDtrEnable.TabIndex = 12;
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
            this.cbStopBits.Location = new System.Drawing.Point(185, 72);
            this.cbStopBits.Name = "cbStopBits";
            this.cbStopBits.Size = new System.Drawing.Size(80, 21);
            this.cbStopBits.TabIndex = 11;
            // 
            // lblStopBits
            // 
            this.lblStopBits.AutoSize = true;
            this.lblStopBits.Location = new System.Drawing.Point(184, 56);
            this.lblStopBits.Name = "lblStopBits";
            this.lblStopBits.Size = new System.Drawing.Size(62, 13);
            this.lblStopBits.TabIndex = 10;
            this.lblStopBits.Text = "Стоп. биты";
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
            this.cbParity.Location = new System.Drawing.Point(99, 72);
            this.cbParity.Name = "cbParity";
            this.cbParity.Size = new System.Drawing.Size(80, 21);
            this.cbParity.TabIndex = 9;
            // 
            // lblParity
            // 
            this.lblParity.AutoSize = true;
            this.lblParity.Location = new System.Drawing.Point(96, 56);
            this.lblParity.Name = "lblParity";
            this.lblParity.Size = new System.Drawing.Size(55, 13);
            this.lblParity.TabIndex = 8;
            this.lblParity.Text = "Чётность";
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
            this.cbDataBits.Location = new System.Drawing.Point(271, 32);
            this.cbDataBits.Name = "cbDataBits";
            this.cbDataBits.Size = new System.Drawing.Size(80, 21);
            this.cbDataBits.TabIndex = 7;
            // 
            // lblDataBits
            // 
            this.lblDataBits.AutoSize = true;
            this.lblDataBits.Location = new System.Drawing.Point(268, 16);
            this.lblDataBits.Name = "lblDataBits";
            this.lblDataBits.Size = new System.Drawing.Size(73, 13);
            this.lblDataBits.TabIndex = 6;
            this.lblDataBits.Text = "Биты данных";
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
            this.cbBaudRate.Location = new System.Drawing.Point(185, 32);
            this.cbBaudRate.Name = "cbBaudRate";
            this.cbBaudRate.Size = new System.Drawing.Size(80, 21);
            this.cbBaudRate.TabIndex = 5;
            // 
            // lblBaudRate
            // 
            this.lblBaudRate.AutoSize = true;
            this.lblBaudRate.Location = new System.Drawing.Point(182, 16);
            this.lblBaudRate.Name = "lblBaudRate";
            this.lblBaudRate.Size = new System.Drawing.Size(55, 13);
            this.lblBaudRate.TabIndex = 4;
            this.lblBaudRate.Text = "Скорость";
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
            this.cbPortName.Location = new System.Drawing.Point(99, 32);
            this.cbPortName.Name = "cbPortName";
            this.cbPortName.Size = new System.Drawing.Size(80, 21);
            this.cbPortName.TabIndex = 3;
            this.cbPortName.Text = "COM1";
            // 
            // FrmCommSerialProps
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(419, 207);
            this.Controls.Add(this.gbConnection);
            this.Name = "FrmCommSerialProps";
            this.Text = "FrmCommSerialProps";
            this.gbConnection.ResumeLayout(false);
            this.gbConnection.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbConnection;
        private System.Windows.Forms.ComboBox cbConnType;
        private System.Windows.Forms.Label lblConnType;
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
    }
}