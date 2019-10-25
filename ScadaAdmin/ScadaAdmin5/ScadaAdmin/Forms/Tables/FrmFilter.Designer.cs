namespace Scada.Admin.App.Forms.Tables
{
    partial class FrmFilter
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
            this.btnClearFilter = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblColumn = new System.Windows.Forms.Label();
            this.cbColumn = new System.Windows.Forms.ComboBox();
            this.lblOperation = new System.Windows.Forms.Label();
            this.cbStringOperation = new System.Windows.Forms.ComboBox();
            this.lblValue = new System.Windows.Forms.Label();
            this.txtValue = new System.Windows.Forms.TextBox();
            this.cbMathOperation = new System.Windows.Forms.ComboBox();
            this.cbValue = new System.Windows.Forms.ComboBox();
            this.cbBoolean = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // btnClearFilter
            // 
            this.btnClearFilter.Location = new System.Drawing.Point(12, 102);
            this.btnClearFilter.Name = "btnClearFilter";
            this.btnClearFilter.Size = new System.Drawing.Size(75, 23);
            this.btnClearFilter.TabIndex = 9;
            this.btnClearFilter.Text = "Clear Filter";
            this.btnClearFilter.UseVisualStyleBackColor = true;
            this.btnClearFilter.Click += new System.EventHandler(this.btnClearFilter_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(176, 102);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 10;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(257, 102);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // lblColumn
            // 
            this.lblColumn.AutoSize = true;
            this.lblColumn.Location = new System.Drawing.Point(9, 9);
            this.lblColumn.Name = "lblColumn";
            this.lblColumn.Size = new System.Drawing.Size(42, 13);
            this.lblColumn.TabIndex = 0;
            this.lblColumn.Text = "Column";
            // 
            // cbColumn
            // 
            this.cbColumn.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbColumn.FormattingEnabled = true;
            this.cbColumn.Location = new System.Drawing.Point(12, 25);
            this.cbColumn.Name = "cbColumn";
            this.cbColumn.Size = new System.Drawing.Size(320, 21);
            this.cbColumn.TabIndex = 1;
            this.cbColumn.SelectedIndexChanged += new System.EventHandler(this.cbColumn_SelectedIndexChanged);
            // 
            // lblOperation
            // 
            this.lblOperation.AutoSize = true;
            this.lblOperation.Location = new System.Drawing.Point(9, 49);
            this.lblOperation.Name = "lblOperation";
            this.lblOperation.Size = new System.Drawing.Size(53, 13);
            this.lblOperation.TabIndex = 2;
            this.lblOperation.Text = "Operation";
            // 
            // cbStringOperation
            // 
            this.cbStringOperation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbStringOperation.FormattingEnabled = true;
            this.cbStringOperation.Items.AddRange(new object[] {
            "Equals",
            "Contains"});
            this.cbStringOperation.Location = new System.Drawing.Point(12, 65);
            this.cbStringOperation.Name = "cbStringOperation";
            this.cbStringOperation.Size = new System.Drawing.Size(114, 21);
            this.cbStringOperation.TabIndex = 3;
            // 
            // lblValue
            // 
            this.lblValue.AutoSize = true;
            this.lblValue.Location = new System.Drawing.Point(129, 49);
            this.lblValue.Name = "lblValue";
            this.lblValue.Size = new System.Drawing.Size(34, 13);
            this.lblValue.TabIndex = 5;
            this.lblValue.Text = "Value";
            // 
            // txtValue
            // 
            this.txtValue.Location = new System.Drawing.Point(132, 65);
            this.txtValue.Name = "txtValue";
            this.txtValue.Size = new System.Drawing.Size(200, 20);
            this.txtValue.TabIndex = 6;
            // 
            // cbMathOperation
            // 
            this.cbMathOperation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMathOperation.FormattingEnabled = true;
            this.cbMathOperation.Items.AddRange(new object[] {
            "=",
            "<>",
            "<",
            "<=",
            ">",
            ">="});
            this.cbMathOperation.Location = new System.Drawing.Point(12, 75);
            this.cbMathOperation.Name = "cbMathOperation";
            this.cbMathOperation.Size = new System.Drawing.Size(114, 21);
            this.cbMathOperation.TabIndex = 4;
            // 
            // cbValue
            // 
            this.cbValue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbValue.FormattingEnabled = true;
            this.cbValue.Location = new System.Drawing.Point(132, 75);
            this.cbValue.Name = "cbValue";
            this.cbValue.Size = new System.Drawing.Size(200, 21);
            this.cbValue.TabIndex = 7;
            // 
            // cbBoolean
            // 
            this.cbBoolean.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBoolean.FormattingEnabled = true;
            this.cbBoolean.Items.AddRange(new object[] {
            "False",
            "True"});
            this.cbBoolean.Location = new System.Drawing.Point(132, 85);
            this.cbBoolean.Name = "cbBoolean";
            this.cbBoolean.Size = new System.Drawing.Size(200, 21);
            this.cbBoolean.TabIndex = 8;
            // 
            // FrmFilter
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(344, 137);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnClearFilter);
            this.Controls.Add(this.cbBoolean);
            this.Controls.Add(this.cbValue);
            this.Controls.Add(this.txtValue);
            this.Controls.Add(this.lblValue);
            this.Controls.Add(this.cbMathOperation);
            this.Controls.Add(this.cbStringOperation);
            this.Controls.Add(this.lblOperation);
            this.Controls.Add(this.cbColumn);
            this.Controls.Add(this.lblColumn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmFilter";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Filter";
            this.Load += new System.EventHandler(this.FrmFilter_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnClearFilter;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblColumn;
        private System.Windows.Forms.ComboBox cbColumn;
        private System.Windows.Forms.Label lblOperation;
        private System.Windows.Forms.ComboBox cbStringOperation;
        private System.Windows.Forms.Label lblValue;
        private System.Windows.Forms.TextBox txtValue;
        private System.Windows.Forms.ComboBox cbMathOperation;
        private System.Windows.Forms.ComboBox cbValue;
        private System.Windows.Forms.ComboBox cbBoolean;
    }
}