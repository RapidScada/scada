namespace Scada.Comm.Channels.UI
{
    partial class FrmCommUdpProps
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
            this.components = new System.ComponentModel.Container();
            this.gbMode = new System.Windows.Forms.GroupBox();
            this.pbBehaviorHint = new System.Windows.Forms.PictureBox();
            this.pbDevSelModeHint = new System.Windows.Forms.PictureBox();
            this.cbDevSelMode = new System.Windows.Forms.ComboBox();
            this.lblDevSelMode = new System.Windows.Forms.Label();
            this.cbBehavior = new System.Windows.Forms.ComboBox();
            this.lblBehavior = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.gbConnection = new System.Windows.Forms.GroupBox();
            this.pbRemoteIpAddressHint = new System.Windows.Forms.PictureBox();
            this.pbLocalUdpPortHint = new System.Windows.Forms.PictureBox();
            this.lblRemoteIpAddress = new System.Windows.Forms.Label();
            this.numLocalUdpPort = new System.Windows.Forms.NumericUpDown();
            this.pbRemoteUdpPortHint = new System.Windows.Forms.PictureBox();
            this.numRemoteUdpPort = new System.Windows.Forms.NumericUpDown();
            this.txtRemoteIpAddress = new System.Windows.Forms.TextBox();
            this.lblRemoteUdpPort = new System.Windows.Forms.Label();
            this.lblLocalUdpPort = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.gbMode.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbBehaviorHint)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbDevSelModeHint)).BeginInit();
            this.gbConnection.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbRemoteIpAddressHint)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLocalUdpPortHint)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLocalUdpPort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbRemoteUdpPortHint)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRemoteUdpPort)).BeginInit();
            this.SuspendLayout();
            // 
            // gbMode
            // 
            this.gbMode.Controls.Add(this.pbBehaviorHint);
            this.gbMode.Controls.Add(this.pbDevSelModeHint);
            this.gbMode.Controls.Add(this.cbDevSelMode);
            this.gbMode.Controls.Add(this.lblDevSelMode);
            this.gbMode.Controls.Add(this.cbBehavior);
            this.gbMode.Controls.Add(this.lblBehavior);
            this.gbMode.Location = new System.Drawing.Point(12, 12);
            this.gbMode.Name = "gbMode";
            this.gbMode.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbMode.Size = new System.Drawing.Size(330, 80);
            this.gbMode.TabIndex = 0;
            this.gbMode.TabStop = false;
            this.gbMode.Text = "Режим работы";
            // 
            // pbBehaviorHint
            // 
            this.pbBehaviorHint.Image = global::Scada.Comm.Properties.Resources.info_tooltip;
            this.pbBehaviorHint.Location = new System.Drawing.Point(301, 21);
            this.pbBehaviorHint.Name = "pbBehaviorHint";
            this.pbBehaviorHint.Size = new System.Drawing.Size(16, 16);
            this.pbBehaviorHint.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbBehaviorHint.TabIndex = 5;
            this.pbBehaviorHint.TabStop = false;
            this.toolTip.SetToolTip(this.pbBehaviorHint, "Master - SCADA-Коммуникатор отправляет запрос устройству и получает ответ.\r\nSlave" +
        " - SCADA-Коммуникатор пассивно ожидает данные от устройства.");
            // 
            // pbDevSelModeHint
            // 
            this.pbDevSelModeHint.Image = global::Scada.Comm.Properties.Resources.info_tooltip;
            this.pbDevSelModeHint.Location = new System.Drawing.Point(301, 48);
            this.pbDevSelModeHint.Name = "pbDevSelModeHint";
            this.pbDevSelModeHint.Size = new System.Drawing.Size(16, 16);
            this.pbDevSelModeHint.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbDevSelModeHint.TabIndex = 4;
            this.pbDevSelModeHint.TabStop = false;
            this.toolTip.SetToolTip(this.pbDevSelModeHint, "Способ привязки полученных данных к КП в режиме Slave:\r\nПо IP-адресу - удалённый " +
        "IP-адрес устройства совпадает с позывным КП.\r\nОпределяется DLL - алгоритм реализ" +
        "ован в библиотеке КП.");
            // 
            // cbDevSelMode
            // 
            this.cbDevSelMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDevSelMode.FormattingEnabled = true;
            this.cbDevSelMode.Items.AddRange(new object[] {
            "По IP-адресу",
            "Определяется DLL"});
            this.cbDevSelMode.Location = new System.Drawing.Point(145, 46);
            this.cbDevSelMode.Name = "cbDevSelMode";
            this.cbDevSelMode.Size = new System.Drawing.Size(150, 21);
            this.cbDevSelMode.TabIndex = 3;
            this.cbDevSelMode.SelectedIndexChanged += new System.EventHandler(this.control_Changed);
            // 
            // lblDevSelMode
            // 
            this.lblDevSelMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDevSelMode.Location = new System.Drawing.Point(13, 50);
            this.lblDevSelMode.Name = "lblDevSelMode";
            this.lblDevSelMode.Size = new System.Drawing.Size(126, 13);
            this.lblDevSelMode.TabIndex = 2;
            this.lblDevSelMode.Text = "Выбор КП";
            this.lblDevSelMode.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // cbBehavior
            // 
            this.cbBehavior.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBehavior.FormattingEnabled = true;
            this.cbBehavior.Items.AddRange(new object[] {
            "Master",
            "Slave"});
            this.cbBehavior.Location = new System.Drawing.Point(145, 19);
            this.cbBehavior.Name = "cbBehavior";
            this.cbBehavior.Size = new System.Drawing.Size(150, 21);
            this.cbBehavior.TabIndex = 1;
            this.cbBehavior.SelectedIndexChanged += new System.EventHandler(this.cbBehavior_SelectedIndexChanged);
            // 
            // lblBehavior
            // 
            this.lblBehavior.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblBehavior.Location = new System.Drawing.Point(13, 23);
            this.lblBehavior.Name = "lblBehavior";
            this.lblBehavior.Size = new System.Drawing.Size(126, 13);
            this.lblBehavior.TabIndex = 0;
            this.lblBehavior.Text = "Поведение";
            this.lblBehavior.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(186, 214);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(267, 214);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // gbConnection
            // 
            this.gbConnection.Controls.Add(this.pbRemoteIpAddressHint);
            this.gbConnection.Controls.Add(this.pbLocalUdpPortHint);
            this.gbConnection.Controls.Add(this.lblRemoteIpAddress);
            this.gbConnection.Controls.Add(this.numLocalUdpPort);
            this.gbConnection.Controls.Add(this.pbRemoteUdpPortHint);
            this.gbConnection.Controls.Add(this.numRemoteUdpPort);
            this.gbConnection.Controls.Add(this.txtRemoteIpAddress);
            this.gbConnection.Controls.Add(this.lblRemoteUdpPort);
            this.gbConnection.Controls.Add(this.lblLocalUdpPort);
            this.gbConnection.Location = new System.Drawing.Point(12, 104);
            this.gbConnection.Name = "gbConnection";
            this.gbConnection.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbConnection.Size = new System.Drawing.Size(330, 104);
            this.gbConnection.TabIndex = 1;
            this.gbConnection.TabStop = false;
            this.gbConnection.Text = "Соединение";
            // 
            // pbRemoteIpAddressHint
            // 
            this.pbRemoteIpAddressHint.Image = global::Scada.Comm.Properties.Resources.info_tooltip;
            this.pbRemoteIpAddressHint.Location = new System.Drawing.Point(301, 73);
            this.pbRemoteIpAddressHint.Name = "pbRemoteIpAddressHint";
            this.pbRemoteIpAddressHint.Size = new System.Drawing.Size(16, 16);
            this.pbRemoteIpAddressHint.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbRemoteIpAddressHint.TabIndex = 9;
            this.pbRemoteIpAddressHint.TabStop = false;
            this.toolTip.SetToolTip(this.pbRemoteIpAddressHint, "Удалённый IP-адрес по умолчанию.\r\nМожет использоваться, например, если устройства" +
        " подключены через шлюз Ethernet - Serial.");
            // 
            // pbLocalUdpPortHint
            // 
            this.pbLocalUdpPortHint.Image = global::Scada.Comm.Properties.Resources.info_tooltip;
            this.pbLocalUdpPortHint.Location = new System.Drawing.Point(301, 21);
            this.pbLocalUdpPortHint.Name = "pbLocalUdpPortHint";
            this.pbLocalUdpPortHint.Size = new System.Drawing.Size(16, 16);
            this.pbLocalUdpPortHint.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbLocalUdpPortHint.TabIndex = 8;
            this.pbLocalUdpPortHint.TabStop = false;
            this.toolTip.SetToolTip(this.pbLocalUdpPortHint, "Локальный UDP-порт для входящих соединений.\r\nВходящие соединения должны быть разр" +
        "ешены брандмауэром.");
            // 
            // lblRemoteIpAddress
            // 
            this.lblRemoteIpAddress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRemoteIpAddress.Location = new System.Drawing.Point(13, 75);
            this.lblRemoteIpAddress.Name = "lblRemoteIpAddress";
            this.lblRemoteIpAddress.Size = new System.Drawing.Size(126, 13);
            this.lblRemoteIpAddress.TabIndex = 4;
            this.lblRemoteIpAddress.Text = "Удалённый IP-адрес";
            this.lblRemoteIpAddress.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // numLocalUdpPort
            // 
            this.numLocalUdpPort.Location = new System.Drawing.Point(145, 19);
            this.numLocalUdpPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numLocalUdpPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numLocalUdpPort.Name = "numLocalUdpPort";
            this.numLocalUdpPort.Size = new System.Drawing.Size(150, 20);
            this.numLocalUdpPort.TabIndex = 1;
            this.numLocalUdpPort.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numLocalUdpPort.ValueChanged += new System.EventHandler(this.control_Changed);
            // 
            // pbRemoteUdpPortHint
            // 
            this.pbRemoteUdpPortHint.Image = global::Scada.Comm.Properties.Resources.info_tooltip;
            this.pbRemoteUdpPortHint.Location = new System.Drawing.Point(301, 47);
            this.pbRemoteUdpPortHint.Name = "pbRemoteUdpPortHint";
            this.pbRemoteUdpPortHint.Size = new System.Drawing.Size(16, 16);
            this.pbRemoteUdpPortHint.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbRemoteUdpPortHint.TabIndex = 5;
            this.pbRemoteUdpPortHint.TabStop = false;
            this.toolTip.SetToolTip(this.pbRemoteUdpPortHint, "Удалённый UDP-порт одинаковый для всех устройств на линии связи.");
            // 
            // numRemoteUdpPort
            // 
            this.numRemoteUdpPort.Location = new System.Drawing.Point(145, 45);
            this.numRemoteUdpPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numRemoteUdpPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numRemoteUdpPort.Name = "numRemoteUdpPort";
            this.numRemoteUdpPort.Size = new System.Drawing.Size(150, 20);
            this.numRemoteUdpPort.TabIndex = 3;
            this.numRemoteUdpPort.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numRemoteUdpPort.ValueChanged += new System.EventHandler(this.control_Changed);
            // 
            // txtRemoteIpAddress
            // 
            this.txtRemoteIpAddress.Location = new System.Drawing.Point(145, 71);
            this.txtRemoteIpAddress.Name = "txtRemoteIpAddress";
            this.txtRemoteIpAddress.Size = new System.Drawing.Size(150, 20);
            this.txtRemoteIpAddress.TabIndex = 5;
            this.txtRemoteIpAddress.TextChanged += new System.EventHandler(this.control_Changed);
            // 
            // lblRemoteUdpPort
            // 
            this.lblRemoteUdpPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRemoteUdpPort.Location = new System.Drawing.Point(13, 49);
            this.lblRemoteUdpPort.Name = "lblRemoteUdpPort";
            this.lblRemoteUdpPort.Size = new System.Drawing.Size(126, 13);
            this.lblRemoteUdpPort.TabIndex = 2;
            this.lblRemoteUdpPort.Text = "Удалённый UDP-порт";
            this.lblRemoteUdpPort.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblLocalUdpPort
            // 
            this.lblLocalUdpPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblLocalUdpPort.Location = new System.Drawing.Point(13, 23);
            this.lblLocalUdpPort.Name = "lblLocalUdpPort";
            this.lblLocalUdpPort.Size = new System.Drawing.Size(126, 13);
            this.lblLocalUdpPort.TabIndex = 0;
            this.lblLocalUdpPort.Text = "Локальный UDP-порт";
            this.lblLocalUdpPort.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // toolTip
            // 
            this.toolTip.AutoPopDelay = 30000;
            this.toolTip.InitialDelay = 500;
            this.toolTip.ReshowDelay = 100;
            // 
            // FrmCommUdpProps
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(354, 249);
            this.Controls.Add(this.gbConnection);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.gbMode);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmCommUdpProps";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Свойства: UDP";
            this.Load += new System.EventHandler(this.FrmCommUdpProps_Load);
            this.gbMode.ResumeLayout(false);
            this.gbMode.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbBehaviorHint)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbDevSelModeHint)).EndInit();
            this.gbConnection.ResumeLayout(false);
            this.gbConnection.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbRemoteIpAddressHint)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLocalUdpPortHint)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLocalUdpPort)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbRemoteUdpPortHint)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRemoteUdpPort)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbMode;
        private System.Windows.Forms.ComboBox cbBehavior;
        private System.Windows.Forms.Label lblBehavior;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ComboBox cbDevSelMode;
        private System.Windows.Forms.Label lblDevSelMode;
        private System.Windows.Forms.GroupBox gbConnection;
        private System.Windows.Forms.Label lblRemoteUdpPort;
        private System.Windows.Forms.Label lblLocalUdpPort;
        private System.Windows.Forms.TextBox txtRemoteIpAddress;
        private System.Windows.Forms.NumericUpDown numRemoteUdpPort;
        private System.Windows.Forms.PictureBox pbDevSelModeHint;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.PictureBox pbRemoteUdpPortHint;
        private System.Windows.Forms.Label lblRemoteIpAddress;
        private System.Windows.Forms.NumericUpDown numLocalUdpPort;
        private System.Windows.Forms.PictureBox pbLocalUdpPortHint;
        private System.Windows.Forms.PictureBox pbRemoteIpAddressHint;
        private System.Windows.Forms.PictureBox pbBehaviorHint;
    }
}