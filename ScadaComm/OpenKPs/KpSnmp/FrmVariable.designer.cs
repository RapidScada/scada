namespace Scada.Comm.Devices.KpSnmp
{
    partial class FrmVariable
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
            this.lblName = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnChange = new System.Windows.Forms.Button();
            this.txtOID = new System.Windows.Forms.TextBox();
            this.lblOID = new System.Windows.Forms.Label();
            this.txtSignal = new System.Windows.Forms.TextBox();
            this.lblSignal = new System.Windows.Forms.Label();
            this.chkBits = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(9, 9);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(83, 13);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Наименование";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(12, 25);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(210, 20);
            this.txtName.TabIndex = 1;
            this.txtName.TextChanged += new System.EventHandler(this.txtNameOrOID_TextChanged);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(66, 152);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 7;
            this.btnAdd.Text = "Добавить";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Visible = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(147, 152);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnChange
            // 
            this.btnChange.Location = new System.Drawing.Point(104, 152);
            this.btnChange.Name = "btnChange";
            this.btnChange.Size = new System.Drawing.Size(75, 23);
            this.btnChange.TabIndex = 8;
            this.btnChange.Text = "Изменить";
            this.btnChange.UseVisualStyleBackColor = true;
            this.btnChange.Visible = false;
            this.btnChange.Click += new System.EventHandler(this.btnChange_Click);
            // 
            // txtOID
            // 
            this.txtOID.Location = new System.Drawing.Point(12, 64);
            this.txtOID.Name = "txtOID";
            this.txtOID.Size = new System.Drawing.Size(210, 20);
            this.txtOID.TabIndex = 3;
            this.txtOID.TextChanged += new System.EventHandler(this.txtNameOrOID_TextChanged);
            // 
            // lblOID
            // 
            this.lblOID.AutoSize = true;
            this.lblOID.Location = new System.Drawing.Point(9, 48);
            this.lblOID.Name = "lblOID";
            this.lblOID.Size = new System.Drawing.Size(26, 13);
            this.lblOID.TabIndex = 2;
            this.lblOID.Text = "OID";
            // 
            // txtSignal
            // 
            this.txtSignal.Location = new System.Drawing.Point(12, 126);
            this.txtSignal.Name = "txtSignal";
            this.txtSignal.ReadOnly = true;
            this.txtSignal.Size = new System.Drawing.Size(210, 20);
            this.txtSignal.TabIndex = 6;
            this.txtSignal.TabStop = false;
            // 
            // lblSignal
            // 
            this.lblSignal.AutoSize = true;
            this.lblSignal.Location = new System.Drawing.Point(9, 110);
            this.lblSignal.Name = "lblSignal";
            this.lblSignal.Size = new System.Drawing.Size(43, 13);
            this.lblSignal.TabIndex = 5;
            this.lblSignal.Text = "Сигнал";
            // 
            // chkBits
            // 
            this.chkBits.AutoSize = true;
            this.chkBits.Location = new System.Drawing.Point(12, 90);
            this.chkBits.Name = "chkBits";
            this.chkBits.Size = new System.Drawing.Size(177, 17);
            this.chkBits.TabIndex = 4;
            this.chkBits.Text = "BITS (расширение OctetString)";
            this.chkBits.UseVisualStyleBackColor = true;
            // 
            // FrmVariable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(234, 187);
            this.Controls.Add(this.chkBits);
            this.Controls.Add(this.txtSignal);
            this.Controls.Add(this.lblSignal);
            this.Controls.Add(this.txtOID);
            this.Controls.Add(this.lblOID);
            this.Controls.Add(this.btnChange);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmVariable";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Переменная";
            this.Load += new System.EventHandler(this.FrmPhoneGroup_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnChange;
        private System.Windows.Forms.TextBox txtOID;
        private System.Windows.Forms.Label lblOID;
        private System.Windows.Forms.TextBox txtSignal;
        private System.Windows.Forms.Label lblSignal;
        private System.Windows.Forms.CheckBox chkBits;
    }
}