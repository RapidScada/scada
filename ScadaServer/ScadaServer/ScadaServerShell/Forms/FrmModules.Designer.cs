namespace Scada.Server.Shell.Forms
{
    partial class FrmModules
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
            this.txtModDescr = new System.Windows.Forms.TextBox();
            this.lblDescr = new System.Windows.Forms.Label();
            this.btnMoveUp = new System.Windows.Forms.Button();
            this.btnMoveDown = new System.Windows.Forms.Button();
            this.btnProperties = new System.Windows.Forms.Button();
            this.btnDeactivate = new System.Windows.Forms.Button();
            this.btnActivate = new System.Windows.Forms.Button();
            this.lnActiveModules = new System.Windows.Forms.ListBox();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.lbUnusedModules = new System.Windows.Forms.ListBox();
            this.pnlTopLeft = new System.Windows.Forms.Panel();
            this.pnlTopRight = new System.Windows.Forms.Panel();
            this.lblUnusedModules = new System.Windows.Forms.Label();
            this.lblActiveModules = new System.Windows.Forms.Label();
            this.tableLayoutPanel.SuspendLayout();
            this.pnlTopLeft.SuspendLayout();
            this.pnlTopRight.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtModDescr
            // 
            this.txtModDescr.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtModDescr.Location = new System.Drawing.Point(12, 241);
            this.txtModDescr.Multiline = true;
            this.txtModDescr.Name = "txtModDescr";
            this.txtModDescr.ReadOnly = true;
            this.txtModDescr.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtModDescr.Size = new System.Drawing.Size(710, 150);
            this.txtModDescr.TabIndex = 2;
            // 
            // lblDescr
            // 
            this.lblDescr.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblDescr.AutoSize = true;
            this.lblDescr.Location = new System.Drawing.Point(9, 225);
            this.lblDescr.Name = "lblDescr";
            this.lblDescr.Size = new System.Drawing.Size(60, 13);
            this.lblDescr.TabIndex = 1;
            this.lblDescr.Text = "Description";
            // 
            // btnMoveUp
            // 
            this.btnMoveUp.Location = new System.Drawing.Point(106, 16);
            this.btnMoveUp.Name = "btnMoveUp";
            this.btnMoveUp.Size = new System.Drawing.Size(75, 23);
            this.btnMoveUp.TabIndex = 2;
            this.btnMoveUp.Text = "Move Up";
            this.btnMoveUp.UseVisualStyleBackColor = true;
            // 
            // btnMoveDown
            // 
            this.btnMoveDown.Location = new System.Drawing.Point(187, 16);
            this.btnMoveDown.Name = "btnMoveDown";
            this.btnMoveDown.Size = new System.Drawing.Size(75, 23);
            this.btnMoveDown.TabIndex = 3;
            this.btnMoveDown.Text = "Move Down";
            this.btnMoveDown.UseVisualStyleBackColor = true;
            // 
            // btnProperties
            // 
            this.btnProperties.Location = new System.Drawing.Point(268, 16);
            this.btnProperties.Name = "btnProperties";
            this.btnProperties.Size = new System.Drawing.Size(75, 23);
            this.btnProperties.TabIndex = 4;
            this.btnProperties.Text = "Properties";
            this.btnProperties.UseVisualStyleBackColor = true;
            // 
            // btnDeactivate
            // 
            this.btnDeactivate.Location = new System.Drawing.Point(0, 16);
            this.btnDeactivate.Name = "btnDeactivate";
            this.btnDeactivate.Size = new System.Drawing.Size(100, 23);
            this.btnDeactivate.TabIndex = 1;
            this.btnDeactivate.Text = "Deactivate";
            this.btnDeactivate.UseVisualStyleBackColor = true;
            // 
            // btnActivate
            // 
            this.btnActivate.Location = new System.Drawing.Point(0, 16);
            this.btnActivate.Name = "btnActivate";
            this.btnActivate.Size = new System.Drawing.Size(100, 23);
            this.btnActivate.TabIndex = 1;
            this.btnActivate.Text = "Activate";
            this.btnActivate.UseVisualStyleBackColor = true;
            // 
            // lnActiveModules
            // 
            this.lnActiveModules.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lnActiveModules.FormattingEnabled = true;
            this.lnActiveModules.HorizontalScrollbar = true;
            this.lnActiveModules.IntegralHeight = false;
            this.lnActiveModules.Location = new System.Drawing.Point(358, 45);
            this.lnActiveModules.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.lnActiveModules.MultiColumn = true;
            this.lnActiveModules.Name = "lnActiveModules";
            this.lnActiveModules.Size = new System.Drawing.Size(352, 155);
            this.lnActiveModules.TabIndex = 3;
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.Controls.Add(this.lbUnusedModules, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.lnActiveModules, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.pnlTopLeft, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.pnlTopRight, 1, 0);
            this.tableLayoutPanel.Location = new System.Drawing.Point(12, 12);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 2;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(710, 200);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // lbUnusedModules
            // 
            this.lbUnusedModules.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbUnusedModules.FormattingEnabled = true;
            this.lbUnusedModules.HorizontalScrollbar = true;
            this.lbUnusedModules.IntegralHeight = false;
            this.lbUnusedModules.Location = new System.Drawing.Point(0, 45);
            this.lbUnusedModules.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.lbUnusedModules.MultiColumn = true;
            this.lbUnusedModules.Name = "lbUnusedModules";
            this.lbUnusedModules.Size = new System.Drawing.Size(352, 155);
            this.lbUnusedModules.TabIndex = 1;
            // 
            // pnlTopLeft
            // 
            this.pnlTopLeft.Controls.Add(this.lblUnusedModules);
            this.pnlTopLeft.Controls.Add(this.btnActivate);
            this.pnlTopLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTopLeft.Location = new System.Drawing.Point(0, 0);
            this.pnlTopLeft.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.pnlTopLeft.Name = "pnlTopLeft";
            this.pnlTopLeft.Size = new System.Drawing.Size(352, 45);
            this.pnlTopLeft.TabIndex = 0;
            // 
            // pnlTopRight
            // 
            this.pnlTopRight.Controls.Add(this.lblActiveModules);
            this.pnlTopRight.Controls.Add(this.btnDeactivate);
            this.pnlTopRight.Controls.Add(this.btnProperties);
            this.pnlTopRight.Controls.Add(this.btnMoveUp);
            this.pnlTopRight.Controls.Add(this.btnMoveDown);
            this.pnlTopRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTopRight.Location = new System.Drawing.Point(358, 0);
            this.pnlTopRight.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.pnlTopRight.Name = "pnlTopRight";
            this.pnlTopRight.Size = new System.Drawing.Size(352, 45);
            this.pnlTopRight.TabIndex = 1;
            // 
            // lblUnusedModules
            // 
            this.lblUnusedModules.AutoSize = true;
            this.lblUnusedModules.Location = new System.Drawing.Point(-3, 0);
            this.lblUnusedModules.Name = "lblUnusedModules";
            this.lblUnusedModules.Size = new System.Drawing.Size(89, 13);
            this.lblUnusedModules.TabIndex = 0;
            this.lblUnusedModules.Text = "Unused modules:";
            // 
            // lblActiveModules
            // 
            this.lblActiveModules.AutoSize = true;
            this.lblActiveModules.Location = new System.Drawing.Point(-3, 0);
            this.lblActiveModules.Name = "lblActiveModules";
            this.lblActiveModules.Size = new System.Drawing.Size(82, 13);
            this.lblActiveModules.TabIndex = 0;
            this.lblActiveModules.Text = "Active modules:";
            // 
            // FrmModules
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(734, 403);
            this.Controls.Add(this.tableLayoutPanel);
            this.Controls.Add(this.txtModDescr);
            this.Controls.Add(this.lblDescr);
            this.Name = "FrmModules";
            this.Text = "Modules";
            this.Load += new System.EventHandler(this.FrmModules_Load);
            this.tableLayoutPanel.ResumeLayout(false);
            this.pnlTopLeft.ResumeLayout(false);
            this.pnlTopLeft.PerformLayout();
            this.pnlTopRight.ResumeLayout(false);
            this.pnlTopRight.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtModDescr;
        private System.Windows.Forms.Label lblDescr;
        private System.Windows.Forms.Button btnMoveUp;
        private System.Windows.Forms.Button btnMoveDown;
        private System.Windows.Forms.Button btnProperties;
        private System.Windows.Forms.Button btnDeactivate;
        private System.Windows.Forms.Button btnActivate;
        private System.Windows.Forms.ListBox lnActiveModules;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.ListBox lbUnusedModules;
        private System.Windows.Forms.Panel pnlTopLeft;
        private System.Windows.Forms.Panel pnlTopRight;
        private System.Windows.Forms.Label lblUnusedModules;
        private System.Windows.Forms.Label lblActiveModules;
    }
}