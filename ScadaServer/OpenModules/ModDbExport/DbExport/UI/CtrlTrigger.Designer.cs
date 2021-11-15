
namespace Scada.Server.Modules.DbExport.UI
{
    partial class CtrlTrigger
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CtrlTrigger));
            this.gbTrigger = new System.Windows.Forms.GroupBox();
            this.lblTriggerType = new System.Windows.Forms.Label();
            this.txtTriggerType = new System.Windows.Forms.TextBox();
            this.btnEditDeviceNum = new System.Windows.Forms.Button();
            this.btnEditCnlNum = new System.Windows.Forms.Button();
            this.lblSeparatorQuery = new System.Windows.Forms.Label();
            this.lblSeparatorFilter = new System.Windows.Forms.Label();
            this.txtDeviceNum = new System.Windows.Forms.TextBox();
            this.lblDeviceNum = new System.Windows.Forms.Label();
            this.lvParametrs = new System.Windows.Forms.ListView();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblParametrs = new System.Windows.Forms.Label();
            this.txtSql = new System.Windows.Forms.TextBox();
            this.lblTriggerName = new System.Windows.Forms.Label();
            this.lblSql = new System.Windows.Forms.Label();
            this.txtCnlNum = new System.Windows.Forms.TextBox();
            this.chkSingleQuery = new System.Windows.Forms.CheckBox();
            this.chkActive = new System.Windows.Forms.CheckBox();
            this.lblCnlNum = new System.Windows.Forms.Label();
            this.lblSeparator1 = new System.Windows.Forms.Label();
            this.lblSeparator = new System.Windows.Forms.Label();
            this.gbTrigger.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbTrigger
            // 
            this.gbTrigger.Controls.Add(this.lblTriggerType);
            this.gbTrigger.Controls.Add(this.txtTriggerType);
            this.gbTrigger.Controls.Add(this.btnEditDeviceNum);
            this.gbTrigger.Controls.Add(this.btnEditCnlNum);
            this.gbTrigger.Controls.Add(this.lblSeparatorQuery);
            this.gbTrigger.Controls.Add(this.lblSeparatorFilter);
            this.gbTrigger.Controls.Add(this.txtDeviceNum);
            this.gbTrigger.Controls.Add(this.lblDeviceNum);
            this.gbTrigger.Controls.Add(this.lvParametrs);
            this.gbTrigger.Controls.Add(this.txtName);
            this.gbTrigger.Controls.Add(this.lblParametrs);
            this.gbTrigger.Controls.Add(this.txtSql);
            this.gbTrigger.Controls.Add(this.lblTriggerName);
            this.gbTrigger.Controls.Add(this.lblSql);
            this.gbTrigger.Controls.Add(this.txtCnlNum);
            this.gbTrigger.Controls.Add(this.chkSingleQuery);
            this.gbTrigger.Controls.Add(this.chkActive);
            this.gbTrigger.Controls.Add(this.lblCnlNum);
            this.gbTrigger.Controls.Add(this.lblSeparator1);
            this.gbTrigger.Controls.Add(this.lblSeparator);
            this.gbTrigger.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbTrigger.Location = new System.Drawing.Point(0, 0);
            this.gbTrigger.Name = "gbTrigger";
            this.gbTrigger.Size = new System.Drawing.Size(414, 480);
            this.gbTrigger.TabIndex = 0;
            this.gbTrigger.TabStop = false;
            this.gbTrigger.Text = "Triggers";
            // 
            // lblTriggerType
            // 
            this.lblTriggerType.AutoSize = true;
            this.lblTriggerType.Location = new System.Drawing.Point(217, 38);
            this.lblTriggerType.Name = "lblTriggerType";
            this.lblTriggerType.Size = new System.Drawing.Size(63, 13);
            this.lblTriggerType.TabIndex = 3;
            this.lblTriggerType.Text = "Trigger type";
            // 
            // txtTriggerType
            // 
            this.txtTriggerType.Location = new System.Drawing.Point(220, 54);
            this.txtTriggerType.Name = "txtTriggerType";
            this.txtTriggerType.ReadOnly = true;
            this.txtTriggerType.Size = new System.Drawing.Size(177, 20);
            this.txtTriggerType.TabIndex = 4;
            this.txtTriggerType.TabStop = false;
            // 
            // btnEditDeviceNum
            // 
            this.btnEditDeviceNum.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnEditDeviceNum.Image = ((System.Drawing.Image)(resources.GetObject("btnEditDeviceNum.Image")));
            this.btnEditDeviceNum.Location = new System.Drawing.Point(377, 171);
            this.btnEditDeviceNum.Name = "btnEditDeviceNum";
            this.btnEditDeviceNum.Size = new System.Drawing.Size(20, 20);
            this.btnEditDeviceNum.TabIndex = 12;
            this.btnEditDeviceNum.UseVisualStyleBackColor = true;
            this.btnEditDeviceNum.Click += new System.EventHandler(this.btnEditDeviceNum_Click);
            // 
            // btnEditCnlNum
            // 
            this.btnEditCnlNum.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnEditCnlNum.Image = ((System.Drawing.Image)(resources.GetObject("btnEditCnlNum.Image")));
            this.btnEditCnlNum.Location = new System.Drawing.Point(377, 129);
            this.btnEditCnlNum.Name = "btnEditCnlNum";
            this.btnEditCnlNum.Size = new System.Drawing.Size(20, 20);
            this.btnEditCnlNum.TabIndex = 9;
            this.btnEditCnlNum.UseVisualStyleBackColor = true;
            this.btnEditCnlNum.Click += new System.EventHandler(this.btnEditCnlNum_Click);
            // 
            // lblSeparatorQuery
            // 
            this.lblSeparatorQuery.AutoSize = true;
            this.lblSeparatorQuery.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblSeparatorQuery.Location = new System.Drawing.Point(10, 208);
            this.lblSeparatorQuery.Name = "lblSeparatorQuery";
            this.lblSeparatorQuery.Size = new System.Drawing.Size(35, 13);
            this.lblSeparatorQuery.TabIndex = 13;
            this.lblSeparatorQuery.Text = "Query";
            // 
            // lblSeparatorFilter
            // 
            this.lblSeparatorFilter.AutoSize = true;
            this.lblSeparatorFilter.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblSeparatorFilter.Location = new System.Drawing.Point(10, 91);
            this.lblSeparatorFilter.Name = "lblSeparatorFilter";
            this.lblSeparatorFilter.Size = new System.Drawing.Size(34, 13);
            this.lblSeparatorFilter.TabIndex = 5;
            this.lblSeparatorFilter.Text = "Filtres";
            // 
            // txtDeviceNum
            // 
            this.txtDeviceNum.Location = new System.Drawing.Point(13, 171);
            this.txtDeviceNum.Name = "txtDeviceNum";
            this.txtDeviceNum.Size = new System.Drawing.Size(358, 20);
            this.txtDeviceNum.TabIndex = 11;
            this.txtDeviceNum.TextChanged += new System.EventHandler(this.txtDeviceNum_TextChanged);
            this.txtDeviceNum.Enter += new System.EventHandler(this.txtDeviceNum_Enter);
            this.txtDeviceNum.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtDeviceNum_KeyDown);
            this.txtDeviceNum.Validating += new System.ComponentModel.CancelEventHandler(this.txtDeviceNum_Validating);
            // 
            // lblDeviceNum
            // 
            this.lblDeviceNum.AutoSize = true;
            this.lblDeviceNum.Location = new System.Drawing.Point(10, 155);
            this.lblDeviceNum.Name = "lblDeviceNum";
            this.lblDeviceNum.Size = new System.Drawing.Size(84, 13);
            this.lblDeviceNum.TabIndex = 10;
            this.lblDeviceNum.Text = "Device numbers";
            // 
            // lvParametrs
            // 
            this.lvParametrs.BackColor = System.Drawing.SystemColors.MenuBar;
            this.lvParametrs.HideSelection = false;
            this.lvParametrs.Location = new System.Drawing.Point(13, 388);
            this.lvParametrs.MultiSelect = false;
            this.lvParametrs.Name = "lvParametrs";
            this.lvParametrs.Size = new System.Drawing.Size(384, 75);
            this.lvParametrs.TabIndex = 19;
            this.lvParametrs.UseCompatibleStateImageBehavior = false;
            this.lvParametrs.View = System.Windows.Forms.View.List;
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(13, 54);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(202, 20);
            this.txtName.TabIndex = 2;
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            // 
            // lblParametrs
            // 
            this.lblParametrs.AutoSize = true;
            this.lblParametrs.Location = new System.Drawing.Point(10, 372);
            this.lblParametrs.Name = "lblParametrs";
            this.lblParametrs.Size = new System.Drawing.Size(99, 13);
            this.lblParametrs.TabIndex = 18;
            this.lblParametrs.Text = "Available parametrs";
            // 
            // txtSql
            // 
            this.txtSql.Location = new System.Drawing.Point(13, 266);
            this.txtSql.Multiline = true;
            this.txtSql.Name = "txtSql";
            this.txtSql.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtSql.Size = new System.Drawing.Size(384, 98);
            this.txtSql.TabIndex = 17;
            this.txtSql.TextChanged += new System.EventHandler(this.txtSql_TextChanged);
            // 
            // lblTriggerName
            // 
            this.lblTriggerName.AutoSize = true;
            this.lblTriggerName.Location = new System.Drawing.Point(10, 38);
            this.lblTriggerName.Name = "lblTriggerName";
            this.lblTriggerName.Size = new System.Drawing.Size(69, 13);
            this.lblTriggerName.TabIndex = 1;
            this.lblTriggerName.Text = "Trigger name";
            // 
            // lblSql
            // 
            this.lblSql.AutoSize = true;
            this.lblSql.Location = new System.Drawing.Point(10, 250);
            this.lblSql.Name = "lblSql";
            this.lblSql.Size = new System.Drawing.Size(28, 13);
            this.lblSql.TabIndex = 16;
            this.lblSql.Text = "SQL";
            // 
            // txtCnlNum
            // 
            this.txtCnlNum.Location = new System.Drawing.Point(13, 129);
            this.txtCnlNum.Name = "txtCnlNum";
            this.txtCnlNum.Size = new System.Drawing.Size(358, 20);
            this.txtCnlNum.TabIndex = 8;
            this.txtCnlNum.TextChanged += new System.EventHandler(this.txtCnlNum_TextChanged);
            this.txtCnlNum.Enter += new System.EventHandler(this.txtCnlNum_Enter);
            this.txtCnlNum.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCnlNum_KeyDown);
            this.txtCnlNum.Validating += new System.ComponentModel.CancelEventHandler(this.txtCnlNum_Validating);
            // 
            // chkSingleQuery
            // 
            this.chkSingleQuery.AutoSize = true;
            this.chkSingleQuery.Location = new System.Drawing.Point(13, 230);
            this.chkSingleQuery.Name = "chkSingleQuery";
            this.chkSingleQuery.Size = new System.Drawing.Size(203, 17);
            this.chkSingleQuery.TabIndex = 15;
            this.chkSingleQuery.Text = "Single query (input channels required)";
            this.chkSingleQuery.UseVisualStyleBackColor = true;
            this.chkSingleQuery.CheckedChanged += new System.EventHandler(this.chkSingleQuery_CheckedChanged);
            // 
            // chkActive
            // 
            this.chkActive.AutoSize = true;
            this.chkActive.Location = new System.Drawing.Point(13, 19);
            this.chkActive.Name = "chkActive";
            this.chkActive.Size = new System.Drawing.Size(56, 17);
            this.chkActive.TabIndex = 0;
            this.chkActive.Text = "Active";
            this.chkActive.UseVisualStyleBackColor = true;
            this.chkActive.CheckedChanged += new System.EventHandler(this.chkActive_CheckedChanged);
            // 
            // lblCnlNum
            // 
            this.lblCnlNum.AutoSize = true;
            this.lblCnlNum.Location = new System.Drawing.Point(10, 113);
            this.lblCnlNum.Name = "lblCnlNum";
            this.lblCnlNum.Size = new System.Drawing.Size(77, 13);
            this.lblCnlNum.TabIndex = 7;
            this.lblCnlNum.Text = "Input channels";
            // 
            // lblSeparator1
            // 
            this.lblSeparator1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblSeparator1.Location = new System.Drawing.Point(13, 215);
            this.lblSeparator1.Name = "lblSeparator1";
            this.lblSeparator1.Size = new System.Drawing.Size(385, 2);
            this.lblSeparator1.TabIndex = 14;
            // 
            // lblSeparator
            // 
            this.lblSeparator.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblSeparator.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblSeparator.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblSeparator.Location = new System.Drawing.Point(13, 98);
            this.lblSeparator.Name = "lblSeparator";
            this.lblSeparator.Size = new System.Drawing.Size(385, 2);
            this.lblSeparator.TabIndex = 6;
            // 
            // CtrlTrigger
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbTrigger);
            this.Name = "CtrlTrigger";
            this.Size = new System.Drawing.Size(414, 480);
            this.gbTrigger.ResumeLayout(false);
            this.gbTrigger.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbTrigger;
        private System.Windows.Forms.CheckBox chkActive;
        private System.Windows.Forms.Label lblCnlNum;
        private System.Windows.Forms.TextBox txtCnlNum;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblTriggerName;
        private System.Windows.Forms.ListView lvParametrs;
        private System.Windows.Forms.Label lblParametrs;
        private System.Windows.Forms.TextBox txtSql;
        private System.Windows.Forms.Label lblSql;
        private System.Windows.Forms.CheckBox chkSingleQuery;
        private System.Windows.Forms.TextBox txtDeviceNum;
        private System.Windows.Forms.Label lblDeviceNum;
        private System.Windows.Forms.Label lblSeparatorQuery;
        private System.Windows.Forms.Label lblSeparatorFilter;
        private System.Windows.Forms.Button btnEditCnlNum;
        private System.Windows.Forms.Button btnEditDeviceNum;
        private System.Windows.Forms.Label lblTriggerType;
        private System.Windows.Forms.TextBox txtTriggerType;
        private System.Windows.Forms.Label lblSeparator1;
        private System.Windows.Forms.Label lblSeparator;
    }
}
