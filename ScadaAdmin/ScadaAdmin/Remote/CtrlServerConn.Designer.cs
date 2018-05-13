namespace ScadaAdmin.Remote
{
    partial class CtrlServerConn
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
            this.gbConnection = new System.Windows.Forms.GroupBox();
            this.btnRemoveConn = new System.Windows.Forms.Button();
            this.btnEditConn = new System.Windows.Forms.Button();
            this.btnCreateConn = new System.Windows.Forms.Button();
            this.cbConnection = new System.Windows.Forms.ComboBox();
            this.gbConnection.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbConnection
            // 
            this.gbConnection.Controls.Add(this.btnRemoveConn);
            this.gbConnection.Controls.Add(this.btnEditConn);
            this.gbConnection.Controls.Add(this.btnCreateConn);
            this.gbConnection.Controls.Add(this.cbConnection);
            this.gbConnection.Location = new System.Drawing.Point(0, 0);
            this.gbConnection.Name = "gbConnection";
            this.gbConnection.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbConnection.Size = new System.Drawing.Size(469, 55);
            this.gbConnection.TabIndex = 1;
            this.gbConnection.TabStop = false;
            this.gbConnection.Text = "Подключение к серверу";
            // 
            // btnRemoveConn
            // 
            this.btnRemoveConn.Location = new System.Drawing.Point(381, 19);
            this.btnRemoveConn.Name = "btnRemoveConn";
            this.btnRemoveConn.Size = new System.Drawing.Size(75, 23);
            this.btnRemoveConn.TabIndex = 3;
            this.btnRemoveConn.Text = "Удалить";
            this.btnRemoveConn.UseVisualStyleBackColor = true;
            this.btnRemoveConn.Click += new System.EventHandler(this.btnRemoveConn_Click);
            // 
            // btnEditConn
            // 
            this.btnEditConn.Location = new System.Drawing.Point(300, 19);
            this.btnEditConn.Name = "btnEditConn";
            this.btnEditConn.Size = new System.Drawing.Size(75, 23);
            this.btnEditConn.TabIndex = 2;
            this.btnEditConn.Text = "Настроить";
            this.btnEditConn.UseVisualStyleBackColor = true;
            this.btnEditConn.Click += new System.EventHandler(this.btnEditConn_Click);
            // 
            // btnCreateConn
            // 
            this.btnCreateConn.Location = new System.Drawing.Point(219, 19);
            this.btnCreateConn.Name = "btnCreateConn";
            this.btnCreateConn.Size = new System.Drawing.Size(75, 23);
            this.btnCreateConn.TabIndex = 1;
            this.btnCreateConn.Text = "Создать";
            this.btnCreateConn.UseVisualStyleBackColor = true;
            this.btnCreateConn.Click += new System.EventHandler(this.btnCreateConn_Click);
            // 
            // cbConnection
            // 
            this.cbConnection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbConnection.FormattingEnabled = true;
            this.cbConnection.Location = new System.Drawing.Point(13, 20);
            this.cbConnection.Name = "cbConnection";
            this.cbConnection.Size = new System.Drawing.Size(200, 21);
            this.cbConnection.TabIndex = 0;
            this.cbConnection.SelectedIndexChanged += new System.EventHandler(this.cbConnection_SelectedIndexChanged);
            // 
            // CtrlServerConn
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbConnection);
            this.Name = "CtrlServerConn";
            this.Size = new System.Drawing.Size(469, 55);
            this.gbConnection.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbConnection;
        private System.Windows.Forms.Button btnRemoveConn;
        private System.Windows.Forms.Button btnEditConn;
        private System.Windows.Forms.Button btnCreateConn;
        private System.Windows.Forms.ComboBox cbConnection;
    }
}
