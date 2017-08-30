namespace Scada.Comm.Devices.HttpNotif.UI
{
    partial class FrmDevProps
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
            this.lblReqUrl = new System.Windows.Forms.Label();
            this.txtReqUrl = new System.Windows.Forms.TextBox();
            this.btnAddressBook = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblReqUrl
            // 
            this.lblReqUrl.AutoSize = true;
            this.lblReqUrl.Location = new System.Drawing.Point(9, 9);
            this.lblReqUrl.Name = "lblReqUrl";
            this.lblReqUrl.Size = new System.Drawing.Size(72, 13);
            this.lblReqUrl.TabIndex = 0;
            this.lblReqUrl.Text = "Request URL";
            // 
            // txtReqUrl
            // 
            this.txtReqUrl.Location = new System.Drawing.Point(12, 25);
            this.txtReqUrl.Multiline = true;
            this.txtReqUrl.Name = "txtReqUrl";
            this.txtReqUrl.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtReqUrl.Size = new System.Drawing.Size(360, 100);
            this.txtReqUrl.TabIndex = 1;
            this.txtReqUrl.TextChanged += new System.EventHandler(this.txtReqUrl_TextChanged);
            // 
            // btnAddressBook
            // 
            this.btnAddressBook.Location = new System.Drawing.Point(12, 131);
            this.btnAddressBook.Name = "btnAddressBook";
            this.btnAddressBook.Size = new System.Drawing.Size(130, 23);
            this.btnAddressBook.TabIndex = 2;
            this.btnAddressBook.Text = "Address book";
            this.btnAddressBook.UseVisualStyleBackColor = true;
            this.btnAddressBook.Click += new System.EventHandler(this.btnAddressBook_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(216, 131);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(297, 131);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // FrmDevProps
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(384, 166);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnAddressBook);
            this.Controls.Add(this.txtReqUrl);
            this.Controls.Add(this.lblReqUrl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmDevProps";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "HTTP Notifications - Device {0} Properties";
            this.Load += new System.EventHandler(this.FrmDevProps_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblReqUrl;
        private System.Windows.Forms.TextBox txtReqUrl;
        private System.Windows.Forms.Button btnAddressBook;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
    }
}