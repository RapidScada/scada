namespace Scada.Server.Modules.DBExport
{
    partial class CtrlExportQuery
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
            this.txtExample = new System.Windows.Forms.TextBox();
            this.lblExample = new System.Windows.Forms.Label();
            this.txtQuery = new System.Windows.Forms.TextBox();
            this.lblQuery = new System.Windows.Forms.Label();
            this.chkExport = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // txtExample
            // 
            this.txtExample.Location = new System.Drawing.Point(0, 205);
            this.txtExample.Multiline = true;
            this.txtExample.Name = "txtExample";
            this.txtExample.ReadOnly = true;
            this.txtExample.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtExample.Size = new System.Drawing.Size(465, 150);
            this.txtExample.TabIndex = 4;
            // 
            // lblExample
            // 
            this.lblExample.AutoSize = true;
            this.lblExample.Location = new System.Drawing.Point(-3, 189);
            this.lblExample.Name = "lblExample";
            this.lblExample.Size = new System.Drawing.Size(47, 13);
            this.lblExample.TabIndex = 3;
            this.lblExample.Text = "Пример";
            // 
            // txtQuery
            // 
            this.txtQuery.Location = new System.Drawing.Point(0, 36);
            this.txtQuery.Multiline = true;
            this.txtQuery.Name = "txtQuery";
            this.txtQuery.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtQuery.Size = new System.Drawing.Size(465, 150);
            this.txtQuery.TabIndex = 2;
            // 
            // lblQuery
            // 
            this.lblQuery.AutoSize = true;
            this.lblQuery.Location = new System.Drawing.Point(-3, 20);
            this.lblQuery.Name = "lblQuery";
            this.lblQuery.Size = new System.Drawing.Size(28, 13);
            this.lblQuery.TabIndex = 1;
            this.lblQuery.Text = "SQL";
            // 
            // chkExport
            // 
            this.chkExport.AutoSize = true;
            this.chkExport.Location = new System.Drawing.Point(0, 0);
            this.chkExport.Name = "chkExport";
            this.chkExport.Size = new System.Drawing.Size(109, 17);
            this.chkExport.TabIndex = 0;
            this.chkExport.Text = "Экспортировать";
            this.chkExport.UseVisualStyleBackColor = true;
            // 
            // CtrlExportQuery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtExample);
            this.Controls.Add(this.lblExample);
            this.Controls.Add(this.txtQuery);
            this.Controls.Add(this.lblQuery);
            this.Controls.Add(this.chkExport);
            this.Name = "CtrlExportQuery";
            this.Size = new System.Drawing.Size(465, 355);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtExample;
        private System.Windows.Forms.Label lblExample;
        private System.Windows.Forms.TextBox txtQuery;
        private System.Windows.Forms.Label lblQuery;
        private System.Windows.Forms.CheckBox chkExport;
    }
}
