namespace Scada.Comm.Shell.Controls
{
    partial class CtrlLineCustomParams
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
            this.gbSelectedParam = new System.Windows.Forms.GroupBox();
            this.lblParamValue = new System.Windows.Forms.Label();
            this.txtParamValue = new System.Windows.Forms.TextBox();
            this.txtParamName = new System.Windows.Forms.TextBox();
            this.lblParamName = new System.Windows.Forms.Label();
            this.lvCustomParams = new System.Windows.Forms.ListView();
            this.colParamName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colParamValue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnAddCustomParam = new System.Windows.Forms.Button();
            this.btnDeleteCustomParam = new System.Windows.Forms.Button();
            this.gbSelectedParam.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbSelectedParam
            // 
            this.gbSelectedParam.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.gbSelectedParam.Controls.Add(this.lblParamValue);
            this.gbSelectedParam.Controls.Add(this.txtParamValue);
            this.gbSelectedParam.Controls.Add(this.txtParamName);
            this.gbSelectedParam.Controls.Add(this.lblParamName);
            this.gbSelectedParam.Location = new System.Drawing.Point(9, 373);
            this.gbSelectedParam.Margin = new System.Windows.Forms.Padding(9, 3, 3, 12);
            this.gbSelectedParam.Name = "gbSelectedParam";
            this.gbSelectedParam.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbSelectedParam.Size = new System.Drawing.Size(450, 65);
            this.gbSelectedParam.TabIndex = 3;
            this.gbSelectedParam.TabStop = false;
            this.gbSelectedParam.Text = "Selected Parameter";
            // 
            // lblParamValue
            // 
            this.lblParamValue.AutoSize = true;
            this.lblParamValue.Location = new System.Drawing.Point(166, 16);
            this.lblParamValue.Name = "lblParamValue";
            this.lblParamValue.Size = new System.Drawing.Size(34, 13);
            this.lblParamValue.TabIndex = 2;
            this.lblParamValue.Text = "Value";
            // 
            // txtParamValue
            // 
            this.txtParamValue.Location = new System.Drawing.Point(169, 32);
            this.txtParamValue.Name = "txtParamValue";
            this.txtParamValue.Size = new System.Drawing.Size(268, 20);
            this.txtParamValue.TabIndex = 3;
            this.txtParamValue.TextChanged += new System.EventHandler(this.txtParamValue_TextChanged);
            // 
            // txtParamName
            // 
            this.txtParamName.Location = new System.Drawing.Point(13, 32);
            this.txtParamName.Name = "txtParamName";
            this.txtParamName.Size = new System.Drawing.Size(150, 20);
            this.txtParamName.TabIndex = 1;
            this.txtParamName.TextChanged += new System.EventHandler(this.txtParamName_TextChanged);
            // 
            // lblParamName
            // 
            this.lblParamName.AutoSize = true;
            this.lblParamName.Location = new System.Drawing.Point(10, 16);
            this.lblParamName.Name = "lblParamName";
            this.lblParamName.Size = new System.Drawing.Size(35, 13);
            this.lblParamName.TabIndex = 0;
            this.lblParamName.Text = "Name";
            // 
            // lvCustomParams
            // 
            this.lvCustomParams.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvCustomParams.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colParamName,
            this.colParamValue});
            this.lvCustomParams.FullRowSelect = true;
            this.lvCustomParams.GridLines = true;
            this.lvCustomParams.HideSelection = false;
            this.lvCustomParams.Location = new System.Drawing.Point(9, 41);
            this.lvCustomParams.Margin = new System.Windows.Forms.Padding(9, 3, 12, 3);
            this.lvCustomParams.MultiSelect = false;
            this.lvCustomParams.Name = "lvCustomParams";
            this.lvCustomParams.ShowItemToolTips = true;
            this.lvCustomParams.Size = new System.Drawing.Size(479, 326);
            this.lvCustomParams.TabIndex = 2;
            this.lvCustomParams.UseCompatibleStateImageBehavior = false;
            this.lvCustomParams.View = System.Windows.Forms.View.Details;
            this.lvCustomParams.SelectedIndexChanged += new System.EventHandler(this.lvCustomParams_SelectedIndexChanged);
            // 
            // colParamName
            // 
            this.colParamName.Text = "Name";
            this.colParamName.Width = 200;
            // 
            // colParamValue
            // 
            this.colParamValue.Text = "Value";
            this.colParamValue.Width = 300;
            // 
            // btnAddCustomParam
            // 
            this.btnAddCustomParam.Location = new System.Drawing.Point(9, 12);
            this.btnAddCustomParam.Margin = new System.Windows.Forms.Padding(9, 12, 3, 3);
            this.btnAddCustomParam.Name = "btnAddCustomParam";
            this.btnAddCustomParam.Size = new System.Drawing.Size(75, 23);
            this.btnAddCustomParam.TabIndex = 0;
            this.btnAddCustomParam.Text = "Add";
            this.btnAddCustomParam.UseVisualStyleBackColor = true;
            this.btnAddCustomParam.Click += new System.EventHandler(this.btnAddCustomParam_Click);
            // 
            // btnDeleteCustomParam
            // 
            this.btnDeleteCustomParam.Location = new System.Drawing.Point(90, 12);
            this.btnDeleteCustomParam.Name = "btnDeleteCustomParam";
            this.btnDeleteCustomParam.Size = new System.Drawing.Size(75, 23);
            this.btnDeleteCustomParam.TabIndex = 1;
            this.btnDeleteCustomParam.Text = "Delete";
            this.btnDeleteCustomParam.UseVisualStyleBackColor = true;
            this.btnDeleteCustomParam.Click += new System.EventHandler(this.btnDeleteCustomParam_Click);
            // 
            // CtrlLineCustomParams
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnDeleteCustomParam);
            this.Controls.Add(this.btnAddCustomParam);
            this.Controls.Add(this.gbSelectedParam);
            this.Controls.Add(this.lvCustomParams);
            this.Name = "CtrlLineCustomParams";
            this.Size = new System.Drawing.Size(500, 450);
            this.Load += new System.EventHandler(this.CtrlLineCustomParams_Load);
            this.gbSelectedParam.ResumeLayout(false);
            this.gbSelectedParam.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbSelectedParam;
        private System.Windows.Forms.Label lblParamValue;
        private System.Windows.Forms.TextBox txtParamValue;
        private System.Windows.Forms.TextBox txtParamName;
        private System.Windows.Forms.Label lblParamName;
        private System.Windows.Forms.ListView lvCustomParams;
        private System.Windows.Forms.ColumnHeader colParamName;
        private System.Windows.Forms.ColumnHeader colParamValue;
        private System.Windows.Forms.Button btnAddCustomParam;
        private System.Windows.Forms.Button btnDeleteCustomParam;
    }
}
