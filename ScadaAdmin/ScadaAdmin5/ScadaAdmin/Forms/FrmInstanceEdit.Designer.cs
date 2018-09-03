namespace Scada.Admin.App.Forms
{
    partial class FrmInstanceEdit
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
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.gbApplications = new System.Windows.Forms.GroupBox();
            this.chkWebApp = new System.Windows.Forms.CheckBox();
            this.chkCommApp = new System.Windows.Forms.CheckBox();
            this.chkServerApp = new System.Windows.Forms.CheckBox();
            this.gbApplications.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(237, 152);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(156, 152);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(12, 25);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(300, 20);
            this.txtName.TabIndex = 1;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(9, 9);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(77, 13);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Instance name";
            // 
            // gbApplications
            // 
            this.gbApplications.Controls.Add(this.chkWebApp);
            this.gbApplications.Controls.Add(this.chkCommApp);
            this.gbApplications.Controls.Add(this.chkServerApp);
            this.gbApplications.Location = new System.Drawing.Point(12, 51);
            this.gbApplications.Name = "gbApplications";
            this.gbApplications.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbApplications.Size = new System.Drawing.Size(300, 95);
            this.gbApplications.TabIndex = 2;
            this.gbApplications.TabStop = false;
            this.gbApplications.Text = "Applications";
            // 
            // chkWebApp
            // 
            this.chkWebApp.AutoSize = true;
            this.chkWebApp.Checked = true;
            this.chkWebApp.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkWebApp.Location = new System.Drawing.Point(13, 65);
            this.chkWebApp.Name = "chkWebApp";
            this.chkWebApp.Size = new System.Drawing.Size(80, 17);
            this.chkWebApp.TabIndex = 2;
            this.chkWebApp.Text = "Webstation";
            this.chkWebApp.UseVisualStyleBackColor = true;
            // 
            // chkCommApp
            // 
            this.chkCommApp.AutoSize = true;
            this.chkCommApp.Checked = true;
            this.chkCommApp.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCommApp.Location = new System.Drawing.Point(13, 42);
            this.chkCommApp.Name = "chkCommApp";
            this.chkCommApp.Size = new System.Drawing.Size(93, 17);
            this.chkCommApp.TabIndex = 1;
            this.chkCommApp.Text = "Communicator";
            this.chkCommApp.UseVisualStyleBackColor = true;
            // 
            // chkServerApp
            // 
            this.chkServerApp.AutoSize = true;
            this.chkServerApp.Checked = true;
            this.chkServerApp.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkServerApp.Location = new System.Drawing.Point(13, 19);
            this.chkServerApp.Name = "chkServerApp";
            this.chkServerApp.Size = new System.Drawing.Size(57, 17);
            this.chkServerApp.TabIndex = 0;
            this.chkServerApp.Text = "Server";
            this.chkServerApp.UseVisualStyleBackColor = true;
            // 
            // FrmInstanceEdit
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(324, 187);
            this.Controls.Add(this.gbApplications);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmInstanceEdit";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit Instance";
            this.Load += new System.EventHandler(this.FrmInstanceEdit_Load);
            this.gbApplications.ResumeLayout(false);
            this.gbApplications.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.GroupBox gbApplications;
        private System.Windows.Forms.CheckBox chkWebApp;
        private System.Windows.Forms.CheckBox chkCommApp;
        private System.Windows.Forms.CheckBox chkServerApp;
    }
}