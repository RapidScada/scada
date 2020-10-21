namespace Scada.Comm.Devices.HttpNotif.UI
{
    partial class FrmConfig
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
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnAddressBook = new System.Windows.Forms.Button();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.pageGeneral = new System.Windows.Forms.TabPage();
            this.gbParam = new System.Windows.Forms.GroupBox();
            this.txtParamEnd = new System.Windows.Forms.TextBox();
            this.lblParamEnd = new System.Windows.Forms.Label();
            this.txtParamBegin = new System.Windows.Forms.TextBox();
            this.lblParamBegin = new System.Windows.Forms.Label();
            this.chkParamEnabled = new System.Windows.Forms.CheckBox();
            this.lblUriHint = new System.Windows.Forms.Label();
            this.txtUri = new System.Windows.Forms.TextBox();
            this.lblUri = new System.Windows.Forms.Label();
            this.cbMethod = new System.Windows.Forms.ComboBox();
            this.lblMethod = new System.Windows.Forms.Label();
            this.pageHeaders = new System.Windows.Forms.TabPage();
            this.dgvHeaders = new System.Windows.Forms.DataGridView();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pageContent = new System.Windows.Forms.TabPage();
            this.lblContentHint = new System.Windows.Forms.Label();
            this.txtContent = new System.Windows.Forms.TextBox();
            this.lblContent = new System.Windows.Forms.Label();
            this.cbContentEscaping = new System.Windows.Forms.ComboBox();
            this.lblContentEscaping = new System.Windows.Forms.Label();
            this.cbContentType = new System.Windows.Forms.ComboBox();
            this.lblContentType = new System.Windows.Forms.Label();
            this.pnlBottom.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.pageGeneral.SuspendLayout();
            this.gbParam.SuspendLayout();
            this.pageHeaders.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHeaders)).BeginInit();
            this.pageContent.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.btnClose);
            this.pnlBottom.Controls.Add(this.btnSave);
            this.pnlBottom.Controls.Add(this.btnAddressBook);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 270);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(484, 41);
            this.pnlBottom.TabIndex = 1;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(397, 6);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(316, 6);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnAddressBook
            // 
            this.btnAddressBook.Location = new System.Drawing.Point(12, 6);
            this.btnAddressBook.Name = "btnAddressBook";
            this.btnAddressBook.Size = new System.Drawing.Size(130, 23);
            this.btnAddressBook.TabIndex = 0;
            this.btnAddressBook.Text = "Address book";
            this.btnAddressBook.UseVisualStyleBackColor = true;
            this.btnAddressBook.Click += new System.EventHandler(this.btnAddressBook_Click);
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.pageGeneral);
            this.tabControl.Controls.Add(this.pageHeaders);
            this.tabControl.Controls.Add(this.pageContent);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(484, 270);
            this.tabControl.TabIndex = 0;
            // 
            // pageGeneral
            // 
            this.pageGeneral.Controls.Add(this.gbParam);
            this.pageGeneral.Controls.Add(this.lblUriHint);
            this.pageGeneral.Controls.Add(this.txtUri);
            this.pageGeneral.Controls.Add(this.lblUri);
            this.pageGeneral.Controls.Add(this.cbMethod);
            this.pageGeneral.Controls.Add(this.lblMethod);
            this.pageGeneral.Location = new System.Drawing.Point(4, 22);
            this.pageGeneral.Name = "pageGeneral";
            this.pageGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.pageGeneral.Size = new System.Drawing.Size(476, 244);
            this.pageGeneral.TabIndex = 0;
            this.pageGeneral.Text = "General";
            this.pageGeneral.UseVisualStyleBackColor = true;
            // 
            // gbParam
            // 
            this.gbParam.Controls.Add(this.txtParamEnd);
            this.gbParam.Controls.Add(this.lblParamEnd);
            this.gbParam.Controls.Add(this.txtParamBegin);
            this.gbParam.Controls.Add(this.lblParamBegin);
            this.gbParam.Controls.Add(this.chkParamEnabled);
            this.gbParam.Location = new System.Drawing.Point(6, 186);
            this.gbParam.Name = "gbParam";
            this.gbParam.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbParam.Size = new System.Drawing.Size(464, 52);
            this.gbParam.TabIndex = 5;
            this.gbParam.TabStop = false;
            this.gbParam.Text = "Parameters";
            // 
            // txtParamEnd
            // 
            this.txtParamEnd.Location = new System.Drawing.Point(352, 19);
            this.txtParamEnd.MaxLength = 1;
            this.txtParamEnd.Name = "txtParamEnd";
            this.txtParamEnd.Size = new System.Drawing.Size(50, 20);
            this.txtParamEnd.TabIndex = 4;
            this.txtParamEnd.TextChanged += new System.EventHandler(this.control_Changed);
            // 
            // lblParamEnd
            // 
            this.lblParamEnd.Location = new System.Drawing.Point(276, 23);
            this.lblParamEnd.Name = "lblParamEnd";
            this.lblParamEnd.Size = new System.Drawing.Size(70, 13);
            this.lblParamEnd.TabIndex = 3;
            this.lblParamEnd.Text = "End";
            this.lblParamEnd.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtParamBegin
            // 
            this.txtParamBegin.Location = new System.Drawing.Point(220, 19);
            this.txtParamBegin.MaxLength = 1;
            this.txtParamBegin.Name = "txtParamBegin";
            this.txtParamBegin.Size = new System.Drawing.Size(50, 20);
            this.txtParamBegin.TabIndex = 2;
            this.txtParamBegin.TextChanged += new System.EventHandler(this.control_Changed);
            // 
            // lblParamBegin
            // 
            this.lblParamBegin.Location = new System.Drawing.Point(144, 23);
            this.lblParamBegin.Name = "lblParamBegin";
            this.lblParamBegin.Size = new System.Drawing.Size(70, 13);
            this.lblParamBegin.TabIndex = 1;
            this.lblParamBegin.Text = "Begin";
            this.lblParamBegin.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chkParamEnabled
            // 
            this.chkParamEnabled.AutoSize = true;
            this.chkParamEnabled.Location = new System.Drawing.Point(13, 21);
            this.chkParamEnabled.Name = "chkParamEnabled";
            this.chkParamEnabled.Size = new System.Drawing.Size(120, 17);
            this.chkParamEnabled.TabIndex = 0;
            this.chkParamEnabled.Text = "Parameters enabled";
            this.chkParamEnabled.UseVisualStyleBackColor = true;
            this.chkParamEnabled.CheckedChanged += new System.EventHandler(this.control_Changed);
            // 
            // lblUriHint
            // 
            this.lblUriHint.AutoSize = true;
            this.lblUriHint.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblUriHint.Location = new System.Drawing.Point(3, 160);
            this.lblUriHint.Name = "lblUriHint";
            this.lblUriHint.Size = new System.Drawing.Size(308, 13);
            this.lblUriHint.TabIndex = 4;
            this.lblUriHint.Text = "May contain parameters, for example {phone}, {email} and {text}";
            // 
            // txtUri
            // 
            this.txtUri.Location = new System.Drawing.Point(6, 59);
            this.txtUri.Multiline = true;
            this.txtUri.Name = "txtUri";
            this.txtUri.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtUri.Size = new System.Drawing.Size(464, 98);
            this.txtUri.TabIndex = 3;
            this.txtUri.TextChanged += new System.EventHandler(this.control_Changed);
            // 
            // lblUri
            // 
            this.lblUri.AutoSize = true;
            this.lblUri.Location = new System.Drawing.Point(3, 43);
            this.lblUri.Name = "lblUri";
            this.lblUri.Size = new System.Drawing.Size(26, 13);
            this.lblUri.TabIndex = 2;
            this.lblUri.Text = "URI";
            // 
            // cbMethod
            // 
            this.cbMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMethod.FormattingEnabled = true;
            this.cbMethod.Items.AddRange(new object[] {
            "GET",
            "POST"});
            this.cbMethod.Location = new System.Drawing.Point(6, 19);
            this.cbMethod.Name = "cbMethod";
            this.cbMethod.Size = new System.Drawing.Size(120, 21);
            this.cbMethod.TabIndex = 1;
            this.cbMethod.SelectedIndexChanged += new System.EventHandler(this.control_Changed);
            // 
            // lblMethod
            // 
            this.lblMethod.AutoSize = true;
            this.lblMethod.Location = new System.Drawing.Point(3, 3);
            this.lblMethod.Name = "lblMethod";
            this.lblMethod.Size = new System.Drawing.Size(43, 13);
            this.lblMethod.TabIndex = 0;
            this.lblMethod.Text = "Method";
            // 
            // pageHeaders
            // 
            this.pageHeaders.Controls.Add(this.dgvHeaders);
            this.pageHeaders.Location = new System.Drawing.Point(4, 22);
            this.pageHeaders.Name = "pageHeaders";
            this.pageHeaders.Padding = new System.Windows.Forms.Padding(3);
            this.pageHeaders.Size = new System.Drawing.Size(476, 244);
            this.pageHeaders.TabIndex = 1;
            this.pageHeaders.Text = "Headers";
            this.pageHeaders.UseVisualStyleBackColor = true;
            // 
            // dgvHeaders
            // 
            this.dgvHeaders.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvHeaders.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colName,
            this.colValue});
            this.dgvHeaders.Location = new System.Drawing.Point(6, 6);
            this.dgvHeaders.Name = "dgvHeaders";
            this.dgvHeaders.Size = new System.Drawing.Size(464, 232);
            this.dgvHeaders.TabIndex = 0;
            this.dgvHeaders.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvHeaders_CellValueChanged);
            this.dgvHeaders.UserDeletedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this.dgvHeaders_UserDeletedRow);
            // 
            // colName
            // 
            this.colName.DataPropertyName = "Name";
            this.colName.HeaderText = "Name";
            this.colName.Name = "colName";
            this.colName.Width = 180;
            // 
            // colValue
            // 
            this.colValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colValue.DataPropertyName = "Value";
            this.colValue.HeaderText = "Value";
            this.colValue.Name = "colValue";
            // 
            // pageContent
            // 
            this.pageContent.Controls.Add(this.lblContentHint);
            this.pageContent.Controls.Add(this.txtContent);
            this.pageContent.Controls.Add(this.lblContent);
            this.pageContent.Controls.Add(this.cbContentEscaping);
            this.pageContent.Controls.Add(this.lblContentEscaping);
            this.pageContent.Controls.Add(this.cbContentType);
            this.pageContent.Controls.Add(this.lblContentType);
            this.pageContent.Location = new System.Drawing.Point(4, 22);
            this.pageContent.Name = "pageContent";
            this.pageContent.Padding = new System.Windows.Forms.Padding(3);
            this.pageContent.Size = new System.Drawing.Size(476, 244);
            this.pageContent.TabIndex = 2;
            this.pageContent.Text = "Content";
            this.pageContent.UseVisualStyleBackColor = true;
            // 
            // lblContentHint
            // 
            this.lblContentHint.AutoSize = true;
            this.lblContentHint.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblContentHint.Location = new System.Drawing.Point(3, 228);
            this.lblContentHint.Name = "lblContentHint";
            this.lblContentHint.Size = new System.Drawing.Size(308, 13);
            this.lblContentHint.TabIndex = 6;
            this.lblContentHint.Text = "May contain parameters, for example {phone}, {email} and {text}";
            // 
            // txtContent
            // 
            this.txtContent.Location = new System.Drawing.Point(6, 59);
            this.txtContent.Multiline = true;
            this.txtContent.Name = "txtContent";
            this.txtContent.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtContent.Size = new System.Drawing.Size(464, 166);
            this.txtContent.TabIndex = 5;
            this.txtContent.TextChanged += new System.EventHandler(this.control_Changed);
            // 
            // lblContent
            // 
            this.lblContent.AutoSize = true;
            this.lblContent.Location = new System.Drawing.Point(3, 43);
            this.lblContent.Name = "lblContent";
            this.lblContent.Size = new System.Drawing.Size(31, 13);
            this.lblContent.TabIndex = 4;
            this.lblContent.Text = "Body";
            // 
            // cbContentEscaping
            // 
            this.cbContentEscaping.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbContentEscaping.FormattingEnabled = true;
            this.cbContentEscaping.Items.AddRange(new object[] {
            "None",
            "URL",
            "JSON"});
            this.cbContentEscaping.Location = new System.Drawing.Point(350, 19);
            this.cbContentEscaping.Name = "cbContentEscaping";
            this.cbContentEscaping.Size = new System.Drawing.Size(120, 21);
            this.cbContentEscaping.TabIndex = 3;
            this.cbContentEscaping.SelectedIndexChanged += new System.EventHandler(this.control_Changed);
            // 
            // lblContentEscaping
            // 
            this.lblContentEscaping.AutoSize = true;
            this.lblContentEscaping.Location = new System.Drawing.Point(347, 3);
            this.lblContentEscaping.Name = "lblContentEscaping";
            this.lblContentEscaping.Size = new System.Drawing.Size(51, 13);
            this.lblContentEscaping.TabIndex = 2;
            this.lblContentEscaping.Text = "Escaping";
            // 
            // cbContentType
            // 
            this.cbContentType.FormattingEnabled = true;
            this.cbContentType.Items.AddRange(new object[] {
            "text/plain",
            "application/json",
            "application/json-rpc"});
            this.cbContentType.Location = new System.Drawing.Point(6, 19);
            this.cbContentType.Name = "cbContentType";
            this.cbContentType.Size = new System.Drawing.Size(338, 21);
            this.cbContentType.TabIndex = 1;
            this.cbContentType.TextChanged += new System.EventHandler(this.control_Changed);
            // 
            // lblContentType
            // 
            this.lblContentType.AutoSize = true;
            this.lblContentType.Location = new System.Drawing.Point(3, 3);
            this.lblContentType.Name = "lblContentType";
            this.lblContentType.Size = new System.Drawing.Size(67, 13);
            this.lblContentType.TabIndex = 0;
            this.lblContentType.Text = "Content type";
            // 
            // FrmConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(484, 311);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.pnlBottom);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmConfig";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "HTTP Notifications - Device {0} Properties";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmConfig_FormClosing);
            this.Load += new System.EventHandler(this.FrmConfig_Load);
            this.pnlBottom.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.pageGeneral.ResumeLayout(false);
            this.pageGeneral.PerformLayout();
            this.gbParam.ResumeLayout(false);
            this.gbParam.PerformLayout();
            this.pageHeaders.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvHeaders)).EndInit();
            this.pageContent.ResumeLayout(false);
            this.pageContent.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnAddressBook;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage pageGeneral;
        private System.Windows.Forms.TabPage pageHeaders;
        private System.Windows.Forms.TabPage pageContent;
        private System.Windows.Forms.ComboBox cbMethod;
        private System.Windows.Forms.Label lblMethod;
        private System.Windows.Forms.TextBox txtUri;
        private System.Windows.Forms.Label lblUri;
        private System.Windows.Forms.TextBox txtContent;
        private System.Windows.Forms.Label lblContent;
        private System.Windows.Forms.ComboBox cbContentType;
        private System.Windows.Forms.Label lblContentType;
        private System.Windows.Forms.ComboBox cbContentEscaping;
        private System.Windows.Forms.Label lblContentEscaping;
        private System.Windows.Forms.DataGridView dgvHeaders;
        private System.Windows.Forms.Label lblUriHint;
        private System.Windows.Forms.Label lblContentHint;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colValue;
        private System.Windows.Forms.CheckBox chkParamEnabled;
        private System.Windows.Forms.TextBox txtParamBegin;
        private System.Windows.Forms.Label lblParamBegin;
        private System.Windows.Forms.TextBox txtParamEnd;
        private System.Windows.Forms.Label lblParamEnd;
        private System.Windows.Forms.GroupBox gbParam;
    }
}