namespace Scada.Admin.App.Controls.Deployment
{
    partial class CtrlProfileSelector
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
            this.gbProfile = new System.Windows.Forms.GroupBox();
            this.btnEditProfile = new System.Windows.Forms.Button();
            this.btnDeleteProfile = new System.Windows.Forms.Button();
            this.btnCreateProfile = new System.Windows.Forms.Button();
            this.cbProfile = new System.Windows.Forms.ComboBox();
            this.gbInstance = new System.Windows.Forms.GroupBox();
            this.txtInstanceName = new System.Windows.Forms.TextBox();
            this.gbProfile.SuspendLayout();
            this.gbInstance.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbProfile
            // 
            this.gbProfile.Controls.Add(this.btnEditProfile);
            this.gbProfile.Controls.Add(this.btnDeleteProfile);
            this.gbProfile.Controls.Add(this.btnCreateProfile);
            this.gbProfile.Controls.Add(this.cbProfile);
            this.gbProfile.Location = new System.Drawing.Point(0, 58);
            this.gbProfile.Name = "gbProfile";
            this.gbProfile.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbProfile.Size = new System.Drawing.Size(469, 55);
            this.gbProfile.TabIndex = 1;
            this.gbProfile.TabStop = false;
            this.gbProfile.Text = "Profile";
            // 
            // btnEditProfile
            // 
            this.btnEditProfile.Location = new System.Drawing.Point(300, 19);
            this.btnEditProfile.Name = "btnEditProfile";
            this.btnEditProfile.Size = new System.Drawing.Size(75, 23);
            this.btnEditProfile.TabIndex = 2;
            this.btnEditProfile.Text = "Edit";
            this.btnEditProfile.UseVisualStyleBackColor = true;
            this.btnEditProfile.Click += new System.EventHandler(this.btnEditProfile_Click);
            // 
            // btnDeleteProfile
            // 
            this.btnDeleteProfile.Location = new System.Drawing.Point(381, 19);
            this.btnDeleteProfile.Name = "btnDeleteProfile";
            this.btnDeleteProfile.Size = new System.Drawing.Size(75, 23);
            this.btnDeleteProfile.TabIndex = 3;
            this.btnDeleteProfile.Text = "Delete";
            this.btnDeleteProfile.UseVisualStyleBackColor = true;
            this.btnDeleteProfile.Click += new System.EventHandler(this.btnDeleteProfile_Click);
            // 
            // btnCreateProfile
            // 
            this.btnCreateProfile.Location = new System.Drawing.Point(219, 19);
            this.btnCreateProfile.Name = "btnCreateProfile";
            this.btnCreateProfile.Size = new System.Drawing.Size(75, 23);
            this.btnCreateProfile.TabIndex = 1;
            this.btnCreateProfile.Text = "Create";
            this.btnCreateProfile.UseVisualStyleBackColor = true;
            this.btnCreateProfile.Click += new System.EventHandler(this.btnCreateProfile_Click);
            // 
            // cbProfile
            // 
            this.cbProfile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbProfile.FormattingEnabled = true;
            this.cbProfile.Location = new System.Drawing.Point(13, 20);
            this.cbProfile.Name = "cbProfile";
            this.cbProfile.Size = new System.Drawing.Size(200, 21);
            this.cbProfile.TabIndex = 0;
            this.cbProfile.SelectedIndexChanged += new System.EventHandler(this.cbProfile_SelectedIndexChanged);
            // 
            // gbInstance
            // 
            this.gbInstance.Controls.Add(this.txtInstanceName);
            this.gbInstance.Location = new System.Drawing.Point(0, 0);
            this.gbInstance.Name = "gbInstance";
            this.gbInstance.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbInstance.Size = new System.Drawing.Size(469, 52);
            this.gbInstance.TabIndex = 0;
            this.gbInstance.TabStop = false;
            this.gbInstance.Text = "Instance";
            // 
            // txtInstanceName
            // 
            this.txtInstanceName.Location = new System.Drawing.Point(13, 19);
            this.txtInstanceName.Name = "txtInstanceName";
            this.txtInstanceName.ReadOnly = true;
            this.txtInstanceName.Size = new System.Drawing.Size(443, 20);
            this.txtInstanceName.TabIndex = 0;
            // 
            // CtrlProfileSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbInstance);
            this.Controls.Add(this.gbProfile);
            this.Name = "CtrlProfileSelector";
            this.Size = new System.Drawing.Size(469, 113);
            this.gbProfile.ResumeLayout(false);
            this.gbInstance.ResumeLayout(false);
            this.gbInstance.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbProfile;
        private System.Windows.Forms.Button btnEditProfile;
        private System.Windows.Forms.Button btnDeleteProfile;
        private System.Windows.Forms.Button btnCreateProfile;
        private System.Windows.Forms.ComboBox cbProfile;
        private System.Windows.Forms.GroupBox gbInstance;
        private System.Windows.Forms.TextBox txtInstanceName;
    }
}
