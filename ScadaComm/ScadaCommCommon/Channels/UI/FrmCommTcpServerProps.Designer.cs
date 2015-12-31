namespace Scada.Comm.Channels.UI
{
    partial class FrmCommTcpServerProps
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmCommTcpServerProps));
            this.gbMode = new System.Windows.Forms.GroupBox();
            this.pbBehaviorHint = new System.Windows.Forms.PictureBox();
            this.pbDevSelModeHint = new System.Windows.Forms.PictureBox();
            this.cbDevSelMode = new System.Windows.Forms.ComboBox();
            this.lblDevSelMode = new System.Windows.Forms.Label();
            this.pbConnModeHint = new System.Windows.Forms.PictureBox();
            this.cbConnMode = new System.Windows.Forms.ComboBox();
            this.lblConnMode = new System.Windows.Forms.Label();
            this.cbBehavior = new System.Windows.Forms.ComboBox();
            this.lblBehavior = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.gbConnection = new System.Windows.Forms.GroupBox();
            this.pbInactiveTimeHint = new System.Windows.Forms.PictureBox();
            this.pbTcpPortHint = new System.Windows.Forms.PictureBox();
            this.numTcpPort = new System.Windows.Forms.NumericUpDown();
            this.numInactiveTime = new System.Windows.Forms.NumericUpDown();
            this.lblTcpPort = new System.Windows.Forms.Label();
            this.lblInactiveTime = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.gbMode.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbBehaviorHint)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbDevSelModeHint)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbConnModeHint)).BeginInit();
            this.gbConnection.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbInactiveTimeHint)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbTcpPortHint)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTcpPort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numInactiveTime)).BeginInit();
            this.SuspendLayout();
            // 
            // gbMode
            // 
            this.gbMode.Controls.Add(this.pbBehaviorHint);
            this.gbMode.Controls.Add(this.pbDevSelModeHint);
            this.gbMode.Controls.Add(this.cbDevSelMode);
            this.gbMode.Controls.Add(this.lblDevSelMode);
            this.gbMode.Controls.Add(this.pbConnModeHint);
            this.gbMode.Controls.Add(this.cbConnMode);
            this.gbMode.Controls.Add(this.lblConnMode);
            this.gbMode.Controls.Add(this.cbBehavior);
            this.gbMode.Controls.Add(this.lblBehavior);
            this.gbMode.Location = new System.Drawing.Point(12, 12);
            this.gbMode.Name = "gbMode";
            this.gbMode.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbMode.Size = new System.Drawing.Size(330, 107);
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
            this.pbBehaviorHint.TabIndex = 8;
            this.pbBehaviorHint.TabStop = false;
            this.toolTip.SetToolTip(this.pbBehaviorHint, "Master - после установки соединения SCADA-Коммуникатор отправляет запрос устройст" +
        "ву и получает ответ.\r\nSlave - SCADA-Коммуникатор пассивно ожидает данные от устр" +
        "ойства.");
            // 
            // pbDevSelModeHint
            // 
            this.pbDevSelModeHint.Image = global::Scada.Comm.Properties.Resources.info_tooltip;
            this.pbDevSelModeHint.Location = new System.Drawing.Point(301, 75);
            this.pbDevSelModeHint.Name = "pbDevSelModeHint";
            this.pbDevSelModeHint.Size = new System.Drawing.Size(16, 16);
            this.pbDevSelModeHint.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbDevSelModeHint.TabIndex = 7;
            this.pbDevSelModeHint.TabStop = false;
            this.toolTip.SetToolTip(this.pbDevSelModeHint, resources.GetString("pbDevSelModeHint.ToolTip"));
            // 
            // cbDevSelMode
            // 
            this.cbDevSelMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDevSelMode.FormattingEnabled = true;
            this.cbDevSelMode.Items.AddRange(new object[] {
            "По IP-адресу",
            "По первому пакету",
            "Определяется DLL"});
            this.cbDevSelMode.Location = new System.Drawing.Point(145, 73);
            this.cbDevSelMode.Name = "cbDevSelMode";
            this.cbDevSelMode.Size = new System.Drawing.Size(150, 21);
            this.cbDevSelMode.TabIndex = 6;
            this.cbDevSelMode.SelectedIndexChanged += new System.EventHandler(this.control_Changed);
            // 
            // lblDevSelMode
            // 
            this.lblDevSelMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDevSelMode.Location = new System.Drawing.Point(13, 77);
            this.lblDevSelMode.Name = "lblDevSelMode";
            this.lblDevSelMode.Size = new System.Drawing.Size(126, 13);
            this.lblDevSelMode.TabIndex = 5;
            this.lblDevSelMode.Text = "Выбор КП";
            this.lblDevSelMode.TextAlign = System.Drawing.ContentAlignment.TopRight;
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
            this.btnOK.Location = new System.Drawing.Point(186, 209);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(267, 209);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // gbConnection
            // 
            this.gbConnection.Controls.Add(this.pbInactiveTimeHint);
            this.gbConnection.Controls.Add(this.pbTcpPortHint);
            this.gbConnection.Controls.Add(this.numTcpPort);
            this.gbConnection.Controls.Add(this.numInactiveTime);
            this.gbConnection.Controls.Add(this.lblTcpPort);
            this.gbConnection.Controls.Add(this.lblInactiveTime);
            this.gbConnection.Location = new System.Drawing.Point(12, 125);
            this.gbConnection.Name = "gbConnection";
            this.gbConnection.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbConnection.Size = new System.Drawing.Size(330, 78);
            this.gbConnection.TabIndex = 1;
            this.gbConnection.TabStop = false;
            this.gbConnection.Text = "Соединение";
            // 
            // pbInactiveTimeHint
            // 
            this.pbInactiveTimeHint.Image = global::Scada.Comm.Properties.Resources.info_tooltip;
            this.pbInactiveTimeHint.Location = new System.Drawing.Point(301, 47);
            this.pbInactiveTimeHint.Name = "pbInactiveTimeHint";
            this.pbInactiveTimeHint.Size = new System.Drawing.Size(16, 16);
            this.pbInactiveTimeHint.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbInactiveTimeHint.TabIndex = 9;
            this.pbInactiveTimeHint.TabStop = false;
            this.toolTip.SetToolTip(this.pbInactiveTimeHint, "Время неактивности соединения до отключения.");
            // 
            // pbTcpPortHint
            // 
            this.pbTcpPortHint.Image = global::Scada.Comm.Properties.Resources.info_tooltip;
            this.pbTcpPortHint.Location = new System.Drawing.Point(301, 21);
            this.pbTcpPortHint.Name = "pbTcpPortHint";
            this.pbTcpPortHint.Size = new System.Drawing.Size(16, 16);
            this.pbTcpPortHint.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbTcpPortHint.TabIndex = 8;
            this.pbTcpPortHint.TabStop = false;
            this.toolTip.SetToolTip(this.pbTcpPortHint, "Локальный TCP-порт для входящих соединений.\r\nВходящие соединения должны быть разр" +
        "ешены брандмауэром.");
            // 
            // numTcpPort
            // 
            this.numTcpPort.Location = new System.Drawing.Point(145, 19);
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
            this.numTcpPort.TabIndex = 1;
            this.numTcpPort.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numTcpPort.ValueChanged += new System.EventHandler(this.control_Changed);
            // 
            // numInactiveTime
            // 
            this.numInactiveTime.Location = new System.Drawing.Point(145, 45);
            this.numInactiveTime.Maximum = new decimal(new int[] {
            3600,
            0,
            0,
            0});
            this.numInactiveTime.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numInactiveTime.Name = "numInactiveTime";
            this.numInactiveTime.Size = new System.Drawing.Size(150, 20);
            this.numInactiveTime.TabIndex = 3;
            this.numInactiveTime.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numInactiveTime.ValueChanged += new System.EventHandler(this.control_Changed);
            // 
            // lblTcpPort
            // 
            this.lblTcpPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTcpPort.Location = new System.Drawing.Point(13, 23);
            this.lblTcpPort.Name = "lblTcpPort";
            this.lblTcpPort.Size = new System.Drawing.Size(126, 13);
            this.lblTcpPort.TabIndex = 0;
            this.lblTcpPort.Text = "Локальный TCP-порт";
            this.lblTcpPort.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblInactiveTime
            // 
            this.lblInactiveTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblInactiveTime.Location = new System.Drawing.Point(13, 49);
            this.lblInactiveTime.Name = "lblInactiveTime";
            this.lblInactiveTime.Size = new System.Drawing.Size(126, 13);
            this.lblInactiveTime.TabIndex = 2;
            this.lblInactiveTime.Text = "Неактивность, с";
            this.lblInactiveTime.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // toolTip
            // 
            this.toolTip.AutoPopDelay = 30000;
            this.toolTip.InitialDelay = 500;
            this.toolTip.ReshowDelay = 100;
            // 
            // FrmCommTcpServerProps
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(354, 244);
            this.Controls.Add(this.gbConnection);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.gbMode);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmCommTcpServerProps";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Свойства: TCP-сервер";
            this.Load += new System.EventHandler(this.FrmCommTcpServerProps_Load);
            this.gbMode.ResumeLayout(false);
            this.gbMode.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbBehaviorHint)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbDevSelModeHint)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbConnModeHint)).EndInit();
            this.gbConnection.ResumeLayout(false);
            this.gbConnection.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbInactiveTimeHint)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbTcpPortHint)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTcpPort)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numInactiveTime)).EndInit();
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
        private System.Windows.Forms.Label lblInactiveTime;
        private System.Windows.Forms.NumericUpDown numInactiveTime;
        private System.Windows.Forms.PictureBox pbConnModeHint;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.PictureBox pbDevSelModeHint;
        private System.Windows.Forms.ComboBox cbDevSelMode;
        private System.Windows.Forms.Label lblDevSelMode;
        private System.Windows.Forms.NumericUpDown numTcpPort;
        private System.Windows.Forms.PictureBox pbBehaviorHint;
        private System.Windows.Forms.PictureBox pbInactiveTimeHint;
        private System.Windows.Forms.PictureBox pbTcpPortHint;
    }
}