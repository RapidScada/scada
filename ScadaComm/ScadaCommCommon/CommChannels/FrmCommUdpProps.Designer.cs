namespace Scada.Comm.Channels
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
            this.pbRemoteIpAddress = new System.Windows.Forms.PictureBox();
            this.pbLocalUdpPort = new System.Windows.Forms.PictureBox();
            this.lblRemoteIpAddress = new System.Windows.Forms.Label();
            this.numLocalUdpPort = new System.Windows.Forms.NumericUpDown();
            this.pbRemoteUdpPort = new System.Windows.Forms.PictureBox();
            this.numRemoteUdpPort = new System.Windows.Forms.NumericUpDown();
            this.txtRemoteIpAddress = new System.Windows.Forms.TextBox();
            this.lblRemoteUdpPort = new System.Windows.Forms.Label();
            this.lblLocalUdpPort = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.gbMode.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbBehaviorHint)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbDevSelModeHint)).BeginInit();
            this.gbConnection.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbRemoteIpAddress)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLocalUdpPort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLocalUdpPort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbRemoteUdpPort)).BeginInit();
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
            this.toolTip.SetToolTip(this.pbBehaviorHint, "Подсказка...\r\nСтрока 2");
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
            this.toolTip.SetToolTip(this.pbDevSelModeHint, "Подсказка...\r\nСтрока 2");
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
            this.lblDevSelMode.AutoSize = true;
            this.lblDevSelMode.Location = new System.Drawing.Point(81, 50);
            this.lblDevSelMode.Name = "lblDevSelMode";
            this.lblDevSelMode.Size = new System.Drawing.Size(58, 13);
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
            this.lblBehavior.AutoSize = true;
            this.lblBehavior.Location = new System.Drawing.Point(76, 23);
            this.lblBehavior.Name = "lblBehavior";
            this.lblBehavior.Size = new System.Drawing.Size(63, 13);
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
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(267, 214);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // gbConnection
            // 
            this.gbConnection.Controls.Add(this.pbRemoteIpAddress);
            this.gbConnection.Controls.Add(this.pbLocalUdpPort);
            this.gbConnection.Controls.Add(this.lblRemoteIpAddress);
            this.gbConnection.Controls.Add(this.numLocalUdpPort);
            this.gbConnection.Controls.Add(this.pbRemoteUdpPort);
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
            // pbRemoteIpAddress
            // 
            this.pbRemoteIpAddress.Image = global::Scada.Comm.Properties.Resources.info_tooltip;
            this.pbRemoteIpAddress.Location = new System.Drawing.Point(301, 73);
            this.pbRemoteIpAddress.Name = "pbRemoteIpAddress";
            this.pbRemoteIpAddress.Size = new System.Drawing.Size(16, 16);
            this.pbRemoteIpAddress.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbRemoteIpAddress.TabIndex = 9;
            this.pbRemoteIpAddress.TabStop = false;
            // 
            // pbLocalUdpPort
            // 
            this.pbLocalUdpPort.Image = global::Scada.Comm.Properties.Resources.info_tooltip;
            this.pbLocalUdpPort.Location = new System.Drawing.Point(301, 21);
            this.pbLocalUdpPort.Name = "pbLocalUdpPort";
            this.pbLocalUdpPort.Size = new System.Drawing.Size(16, 16);
            this.pbLocalUdpPort.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbLocalUdpPort.TabIndex = 8;
            this.pbLocalUdpPort.TabStop = false;
            // 
            // lblRemoteIpAddress
            // 
            this.lblRemoteIpAddress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRemoteIpAddress.AutoSize = true;
            this.lblRemoteIpAddress.Location = new System.Drawing.Point(28, 75);
            this.lblRemoteIpAddress.Name = "lblRemoteIpAddress";
            this.lblRemoteIpAddress.Size = new System.Drawing.Size(111, 13);
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
            // pbRemoteUdpPort
            // 
            this.pbRemoteUdpPort.Image = global::Scada.Comm.Properties.Resources.info_tooltip;
            this.pbRemoteUdpPort.Location = new System.Drawing.Point(301, 47);
            this.pbRemoteUdpPort.Name = "pbRemoteUdpPort";
            this.pbRemoteUdpPort.Size = new System.Drawing.Size(16, 16);
            this.pbRemoteUdpPort.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbRemoteUdpPort.TabIndex = 5;
            this.pbRemoteUdpPort.TabStop = false;
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
            this.lblRemoteUdpPort.AutoSize = true;
            this.lblRemoteUdpPort.Location = new System.Drawing.Point(22, 49);
            this.lblRemoteUdpPort.Name = "lblRemoteUdpPort";
            this.lblRemoteUdpPort.Size = new System.Drawing.Size(117, 13);
            this.lblRemoteUdpPort.TabIndex = 2;
            this.lblRemoteUdpPort.Text = "Удалённый UDP-порт";
            this.lblRemoteUdpPort.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblLocalUdpPort
            // 
            this.lblLocalUdpPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblLocalUdpPort.AutoSize = true;
            this.lblLocalUdpPort.Location = new System.Drawing.Point(22, 23);
            this.lblLocalUdpPort.Name = "lblLocalUdpPort";
            this.lblLocalUdpPort.Size = new System.Drawing.Size(117, 13);
            this.lblLocalUdpPort.TabIndex = 0;
            this.lblLocalUdpPort.Text = "Локальный UDP-порт";
            this.lblLocalUdpPort.TextAlign = System.Drawing.ContentAlignment.TopRight;
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
            ((System.ComponentModel.ISupportInitialize)(this.pbRemoteIpAddress)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLocalUdpPort)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLocalUdpPort)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbRemoteUdpPort)).EndInit();
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
        private System.Windows.Forms.PictureBox pbRemoteUdpPort;
        private System.Windows.Forms.Label lblRemoteIpAddress;
        private System.Windows.Forms.NumericUpDown numLocalUdpPort;
        private System.Windows.Forms.PictureBox pbLocalUdpPort;
        private System.Windows.Forms.PictureBox pbRemoteIpAddress;
        private System.Windows.Forms.PictureBox pbBehaviorHint;
    }
}