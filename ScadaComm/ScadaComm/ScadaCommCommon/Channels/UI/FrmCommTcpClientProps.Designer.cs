namespace Scada.Comm.Channels.UI
{
    partial class FrmCommTcpClientProps
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmCommTcpClientProps));
            this.gbMode = new System.Windows.Forms.GroupBox();
            this.pbBehaviorHint = new System.Windows.Forms.PictureBox();
            this.pbConnModeHint = new System.Windows.Forms.PictureBox();
            this.cbConnMode = new System.Windows.Forms.ComboBox();
            this.lblConnMode = new System.Windows.Forms.Label();
            this.cbBehavior = new System.Windows.Forms.ComboBox();
            this.lblBehavior = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.gbConnection = new System.Windows.Forms.GroupBox();
            this.pbStayConnectedHint = new System.Windows.Forms.PictureBox();
            this.chkStayConnected = new System.Windows.Forms.CheckBox();
            this.pbReconnectAfterHint = new System.Windows.Forms.PictureBox();
            this.lblReconnectAfter = new System.Windows.Forms.Label();
            this.numReconnectAfter = new System.Windows.Forms.NumericUpDown();
            this.pbHostHint = new System.Windows.Forms.PictureBox();
            this.pbTcpPortHint = new System.Windows.Forms.PictureBox();
            this.numTcpPort = new System.Windows.Forms.NumericUpDown();
            this.txtHost = new System.Windows.Forms.TextBox();
            this.lblTcpPort = new System.Windows.Forms.Label();
            this.lblHost = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.gbMode.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbBehaviorHint)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbConnModeHint)).BeginInit();
            this.gbConnection.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbStayConnectedHint)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbReconnectAfterHint)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numReconnectAfter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbHostHint)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbTcpPortHint)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTcpPort)).BeginInit();
            this.SuspendLayout();
            // 
            // gbMode
            // 
            this.gbMode.Controls.Add(this.pbBehaviorHint);
            this.gbMode.Controls.Add(this.pbConnModeHint);
            this.gbMode.Controls.Add(this.cbConnMode);
            this.gbMode.Controls.Add(this.lblConnMode);
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
            // pbConnModeHint
            // 
            this.pbConnModeHint.Image = global::Scada.Comm.Properties.Resources.info_tooltip;
            this.pbConnModeHint.Location = new System.Drawing.Point(301, 48);
            this.pbConnModeHint.Name = "pbConnModeHint";
            this.pbConnModeHint.Size = new System.Drawing.Size(16, 16);
            this.pbConnModeHint.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbConnModeHint.TabIndex = 4;
            this.pbConnModeHint.TabStop = false;
            this.toolTip.SetToolTip(this.pbConnModeHint, resources.GetString("pbConnModeHint.ToolTip"));
            // 
            // cbConnMode
            // 
            this.cbConnMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbConnMode.FormattingEnabled = true;
            this.cbConnMode.Items.AddRange(new object[] {
            "Индивидуальное",
            "Общее"});
            this.cbConnMode.Location = new System.Drawing.Point(145, 46);
            this.cbConnMode.Name = "cbConnMode";
            this.cbConnMode.Size = new System.Drawing.Size(150, 21);
            this.cbConnMode.TabIndex = 3;
            this.cbConnMode.SelectedIndexChanged += new System.EventHandler(this.cbConnMode_SelectedIndexChanged);
            // 
            // lblConnMode
            // 
            this.lblConnMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblConnMode.Location = new System.Drawing.Point(13, 50);
            this.lblConnMode.Name = "lblConnMode";
            this.lblConnMode.Size = new System.Drawing.Size(126, 13);
            this.lblConnMode.TabIndex = 2;
            this.lblConnMode.Text = "Соединение";
            this.lblConnMode.TextAlign = System.Drawing.ContentAlignment.TopRight;
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
            this.cbBehavior.SelectedIndexChanged += new System.EventHandler(this.control_Changed);
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
            this.btnOK.Location = new System.Drawing.Point(186, 238);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(267, 238);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // gbConnection
            // 
            this.gbConnection.Controls.Add(this.pbStayConnectedHint);
            this.gbConnection.Controls.Add(this.chkStayConnected);
            this.gbConnection.Controls.Add(this.pbReconnectAfterHint);
            this.gbConnection.Controls.Add(this.lblReconnectAfter);
            this.gbConnection.Controls.Add(this.numReconnectAfter);
            this.gbConnection.Controls.Add(this.pbHostHint);
            this.gbConnection.Controls.Add(this.pbTcpPortHint);
            this.gbConnection.Controls.Add(this.numTcpPort);
            this.gbConnection.Controls.Add(this.txtHost);
            this.gbConnection.Controls.Add(this.lblTcpPort);
            this.gbConnection.Controls.Add(this.lblHost);
            this.gbConnection.Location = new System.Drawing.Point(12, 104);
            this.gbConnection.Name = "gbConnection";
            this.gbConnection.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbConnection.Size = new System.Drawing.Size(330, 128);
            this.gbConnection.TabIndex = 1;
            this.gbConnection.TabStop = false;
            this.gbConnection.Text = "Соединение";
            // 
            // pbStayConnectedHint
            // 
            this.pbStayConnectedHint.Image = global::Scada.Comm.Properties.Resources.info_tooltip;
            this.pbStayConnectedHint.Location = new System.Drawing.Point(301, 99);
            this.pbStayConnectedHint.Name = "pbStayConnectedHint";
            this.pbStayConnectedHint.Size = new System.Drawing.Size(16, 16);
            this.pbStayConnectedHint.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbStayConnectedHint.TabIndex = 11;
            this.pbStayConnectedHint.TabStop = false;
            this.toolTip.SetToolTip(this.pbStayConnectedHint, "Если параметр установлен, то соединение между сеансами связи сохраняется.\r\nВ прот" +
        "ивном случае соединение разрывается после каждого сеанса.");
            // 
            // chkStayConnected
            // 
            this.chkStayConnected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkStayConnected.AutoSize = true;
            this.chkStayConnected.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkStayConnected.Location = new System.Drawing.Point(25, 99);
            this.chkStayConnected.Name = "chkStayConnected";
            this.chkStayConnected.Size = new System.Drawing.Size(134, 17);
            this.chkStayConnected.TabIndex = 6;
            this.chkStayConnected.Text = "Оставаться на связи";
            this.chkStayConnected.UseVisualStyleBackColor = true;
            this.chkStayConnected.CheckedChanged += new System.EventHandler(this.control_Changed);
            // 
            // pbReconnectAfterHint
            // 
            this.pbReconnectAfterHint.Image = global::Scada.Comm.Properties.Resources.info_tooltip;
            this.pbReconnectAfterHint.Location = new System.Drawing.Point(301, 73);
            this.pbReconnectAfterHint.Name = "pbReconnectAfterHint";
            this.pbReconnectAfterHint.Size = new System.Drawing.Size(16, 16);
            this.pbReconnectAfterHint.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbReconnectAfterHint.TabIndex = 9;
            this.pbReconnectAfterHint.TabStop = false;
            this.toolTip.SetToolTip(this.pbReconnectAfterHint, "Задаёт интервал времени для повторного подключения после неудачной попытки.");
            // 
            // lblReconnectAfter
            // 
            this.lblReconnectAfter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblReconnectAfter.Location = new System.Drawing.Point(13, 75);
            this.lblReconnectAfter.Name = "lblReconnectAfter";
            this.lblReconnectAfter.Size = new System.Drawing.Size(126, 13);
            this.lblReconnectAfter.TabIndex = 4;
            this.lblReconnectAfter.Text = "Переподключение, с";
            this.lblReconnectAfter.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // numReconnectAfter
            // 
            this.numReconnectAfter.Location = new System.Drawing.Point(145, 71);
            this.numReconnectAfter.Maximum = new decimal(new int[] {
            3600,
            0,
            0,
            0});
            this.numReconnectAfter.Name = "numReconnectAfter";
            this.numReconnectAfter.Size = new System.Drawing.Size(150, 20);
            this.numReconnectAfter.TabIndex = 5;
            this.numReconnectAfter.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.numReconnectAfter.ValueChanged += new System.EventHandler(this.control_Changed);
            // 
            // pbHostHint
            // 
            this.pbHostHint.Image = global::Scada.Comm.Properties.Resources.info_tooltip;
            this.pbHostHint.Location = new System.Drawing.Point(301, 21);
            this.pbHostHint.Name = "pbHostHint";
            this.pbHostHint.Size = new System.Drawing.Size(16, 16);
            this.pbHostHint.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbHostHint.TabIndex = 6;
            this.pbHostHint.TabStop = false;
            this.toolTip.SetToolTip(this.pbHostHint, "Удалённое DNS-имя или IP-адрес при использовании общего соединения.");
            // 
            // pbTcpPortHint
            // 
            this.pbTcpPortHint.Image = global::Scada.Comm.Properties.Resources.info_tooltip;
            this.pbTcpPortHint.Location = new System.Drawing.Point(301, 47);
            this.pbTcpPortHint.Name = "pbTcpPortHint";
            this.pbTcpPortHint.Size = new System.Drawing.Size(16, 16);
            this.pbTcpPortHint.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbTcpPortHint.TabIndex = 5;
            this.pbTcpPortHint.TabStop = false;
            this.toolTip.SetToolTip(this.pbTcpPortHint, resources.GetString("pbTcpPortHint.ToolTip"));
            // 
            // numTcpPort
            // 
            this.numTcpPort.Location = new System.Drawing.Point(145, 45);
            this.numTcpPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numTcpPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numTcpPort.Name = "numTcpPort";
            this.numTcpPort.Size = new System.Drawing.Size(150, 20);
            this.numTcpPort.TabIndex = 3;
            this.numTcpPort.Value = new decimal(new int[] {
            502,
            0,
            0,
            0});
            this.numTcpPort.ValueChanged += new System.EventHandler(this.control_Changed);
            // 
            // txtHost
            // 
            this.txtHost.Location = new System.Drawing.Point(145, 19);
            this.txtHost.Name = "txtHost";
            this.txtHost.Size = new System.Drawing.Size(150, 20);
            this.txtHost.TabIndex = 1;
            this.txtHost.TextChanged += new System.EventHandler(this.control_Changed);
            // 
            // lblTcpPort
            // 
            this.lblTcpPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTcpPort.Location = new System.Drawing.Point(13, 49);
            this.lblTcpPort.Name = "lblTcpPort";
            this.lblTcpPort.Size = new System.Drawing.Size(126, 13);
            this.lblTcpPort.TabIndex = 2;
            this.lblTcpPort.Text = "Удалённый TCP-порт";
            this.lblTcpPort.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblHost
            // 
            this.lblHost.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblHost.Location = new System.Drawing.Point(13, 23);
            this.lblHost.Name = "lblHost";
            this.lblHost.Size = new System.Drawing.Size(126, 13);
            this.lblHost.TabIndex = 0;
            this.lblHost.Text = "Удалённый хост";
            this.lblHost.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // toolTip
            // 
            this.toolTip.AutoPopDelay = 30000;
            this.toolTip.InitialDelay = 500;
            this.toolTip.ReshowDelay = 100;
            // 
            // FrmCommTcpClientProps
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(354, 273);
            this.Controls.Add(this.gbConnection);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.gbMode);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmCommTcpClientProps";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Свойства: TCP-клиент";
            this.Load += new System.EventHandler(this.FrmCommTcpClientProps_Load);
            this.gbMode.ResumeLayout(false);
            this.gbMode.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbBehaviorHint)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbConnModeHint)).EndInit();
            this.gbConnection.ResumeLayout(false);
            this.gbConnection.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbStayConnectedHint)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbReconnectAfterHint)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numReconnectAfter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbHostHint)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbTcpPortHint)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTcpPort)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbMode;
        private System.Windows.Forms.ComboBox cbBehavior;
        private System.Windows.Forms.Label lblBehavior;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ComboBox cbConnMode;
        private System.Windows.Forms.Label lblConnMode;
        private System.Windows.Forms.GroupBox gbConnection;
        private System.Windows.Forms.Label lblTcpPort;
        private System.Windows.Forms.Label lblHost;
        private System.Windows.Forms.TextBox txtHost;
        private System.Windows.Forms.NumericUpDown numTcpPort;
        private System.Windows.Forms.PictureBox pbConnModeHint;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.PictureBox pbTcpPortHint;
        private System.Windows.Forms.PictureBox pbBehaviorHint;
        private System.Windows.Forms.PictureBox pbHostHint;
        private System.Windows.Forms.NumericUpDown numReconnectAfter;
        private System.Windows.Forms.PictureBox pbReconnectAfterHint;
        private System.Windows.Forms.Label lblReconnectAfter;
        private System.Windows.Forms.CheckBox chkStayConnected;
        private System.Windows.Forms.PictureBox pbStayConnectedHint;
    }
}