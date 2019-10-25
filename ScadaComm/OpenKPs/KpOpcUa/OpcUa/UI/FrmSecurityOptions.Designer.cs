namespace Scada.Comm.Devices.OpcUa.UI
{
    partial class FrmSecurityOptions
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
            this.lblSecurityMode = new System.Windows.Forms.Label();
            this.cbSecurityMode = new System.Windows.Forms.ComboBox();
            this.cbSecurityPolicy = new System.Windows.Forms.ComboBox();
            this.lblSecurityPolicy = new System.Windows.Forms.Label();
            this.cbAuthenticationMode = new System.Windows.Forms.ComboBox();
            this.lblAuthenticationMode = new System.Windows.Forms.Label();
            this.pnlUsername = new System.Windows.Forms.Panel();
            this.lblUsername = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.pnlUsername.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblSecurityMode
            // 
            this.lblSecurityMode.AutoSize = true;
            this.lblSecurityMode.Location = new System.Drawing.Point(9, 9);
            this.lblSecurityMode.Name = "lblSecurityMode";
            this.lblSecurityMode.Size = new System.Drawing.Size(74, 13);
            this.lblSecurityMode.TabIndex = 0;
            this.lblSecurityMode.Text = "Security mode";
            // 
            // cbSecurityMode
            // 
            this.cbSecurityMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSecurityMode.FormattingEnabled = true;
            this.cbSecurityMode.Location = new System.Drawing.Point(12, 25);
            this.cbSecurityMode.Name = "cbSecurityMode";
            this.cbSecurityMode.Size = new System.Drawing.Size(260, 21);
            this.cbSecurityMode.TabIndex = 1;
            this.cbSecurityMode.SelectedIndexChanged += new System.EventHandler(this.cbSecurityMode_SelectedIndexChanged);
            // 
            // cbSecurityPolicy
            // 
            this.cbSecurityPolicy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSecurityPolicy.FormattingEnabled = true;
            this.cbSecurityPolicy.Location = new System.Drawing.Point(12, 65);
            this.cbSecurityPolicy.Name = "cbSecurityPolicy";
            this.cbSecurityPolicy.Size = new System.Drawing.Size(260, 21);
            this.cbSecurityPolicy.TabIndex = 3;
            // 
            // lblSecurityPolicy
            // 
            this.lblSecurityPolicy.AutoSize = true;
            this.lblSecurityPolicy.Location = new System.Drawing.Point(9, 49);
            this.lblSecurityPolicy.Name = "lblSecurityPolicy";
            this.lblSecurityPolicy.Size = new System.Drawing.Size(75, 13);
            this.lblSecurityPolicy.TabIndex = 2;
            this.lblSecurityPolicy.Text = "Security policy";
            // 
            // cbAuthenticationMode
            // 
            this.cbAuthenticationMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAuthenticationMode.FormattingEnabled = true;
            this.cbAuthenticationMode.Location = new System.Drawing.Point(12, 105);
            this.cbAuthenticationMode.Name = "cbAuthenticationMode";
            this.cbAuthenticationMode.Size = new System.Drawing.Size(260, 21);
            this.cbAuthenticationMode.TabIndex = 5;
            this.cbAuthenticationMode.SelectedIndexChanged += new System.EventHandler(this.cbAuthenticationMode_SelectedIndexChanged);
            // 
            // lblAuthenticationMode
            // 
            this.lblAuthenticationMode.AutoSize = true;
            this.lblAuthenticationMode.Location = new System.Drawing.Point(9, 89);
            this.lblAuthenticationMode.Name = "lblAuthenticationMode";
            this.lblAuthenticationMode.Size = new System.Drawing.Size(104, 13);
            this.lblAuthenticationMode.TabIndex = 4;
            this.lblAuthenticationMode.Text = "Authentication mode";
            // 
            // pnlUsername
            // 
            this.pnlUsername.Controls.Add(this.txtPassword);
            this.pnlUsername.Controls.Add(this.lblPassword);
            this.pnlUsername.Controls.Add(this.txtUsername);
            this.pnlUsername.Controls.Add(this.lblUsername);
            this.pnlUsername.Location = new System.Drawing.Point(12, 132);
            this.pnlUsername.Name = "pnlUsername";
            this.pnlUsername.Size = new System.Drawing.Size(260, 75);
            this.pnlUsername.TabIndex = 6;
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(-3, 0);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(55, 13);
            this.lblUsername.TabIndex = 0;
            this.lblUsername.Text = "Username";
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(0, 16);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(260, 20);
            this.txtUsername.TabIndex = 1;
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(0, 55);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(260, 20);
            this.txtPassword.TabIndex = 3;
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(-3, 39);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(53, 13);
            this.lblPassword.TabIndex = 2;
            this.lblPassword.Text = "Password";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(116, 223);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 7;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(197, 223);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // FrmSecurityOptions
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(284, 258);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.pnlUsername);
            this.Controls.Add(this.cbAuthenticationMode);
            this.Controls.Add(this.lblAuthenticationMode);
            this.Controls.Add(this.cbSecurityPolicy);
            this.Controls.Add(this.lblSecurityPolicy);
            this.Controls.Add(this.cbSecurityMode);
            this.Controls.Add(this.lblSecurityMode);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmSecurityOptions";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Security Options";
            this.Load += new System.EventHandler(this.FrmSecurityOptions_Load);
            this.pnlUsername.ResumeLayout(false);
            this.pnlUsername.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblSecurityMode;
        private System.Windows.Forms.ComboBox cbSecurityMode;
        private System.Windows.Forms.ComboBox cbSecurityPolicy;
        private System.Windows.Forms.Label lblSecurityPolicy;
        private System.Windows.Forms.ComboBox cbAuthenticationMode;
        private System.Windows.Forms.Label lblAuthenticationMode;
        private System.Windows.Forms.Panel pnlUsername;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
    }
}