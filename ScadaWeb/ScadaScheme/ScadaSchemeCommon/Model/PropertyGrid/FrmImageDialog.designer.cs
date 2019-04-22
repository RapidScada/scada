namespace Scada.Scheme.Model.PropertyGrid
{
    partial class FrmImageDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmImageDialog));
            this.btnSelectEmpty = new System.Windows.Forms.Button();
            this.btnSelect = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.lbImages = new System.Windows.Forms.ListBox();
            this.btnDel = new System.Windows.Forms.Button();
            this.btnOpen = new System.Windows.Forms.Button();
            this.propGrid = new System.Windows.Forms.PropertyGrid();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.lblImages = new System.Windows.Forms.Label();
            this.lblImageProps = new System.Windows.Forms.Label();
            this.lblPreview = new System.Windows.Forms.Label();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.btnSave = new System.Windows.Forms.Button();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSelectEmpty
            // 
            this.btnSelectEmpty.Location = new System.Drawing.Point(181, 350);
            this.btnSelectEmpty.Name = "btnSelectEmpty";
            this.btnSelectEmpty.Size = new System.Drawing.Size(75, 23);
            this.btnSelectEmpty.TabIndex = 8;
            this.btnSelectEmpty.Text = "Empty";
            this.btnSelectEmpty.UseVisualStyleBackColor = true;
            this.btnSelectEmpty.Click += new System.EventHandler(this.btnSelectEmpty_Click);
            // 
            // btnSelect
            // 
            this.btnSelect.Enabled = false;
            this.btnSelect.Location = new System.Drawing.Point(262, 350);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(75, 23);
            this.btnSelect.TabIndex = 9;
            this.btnSelect.Text = "Select";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(343, 350);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 10;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // lbImages
            // 
            this.lbImages.FormattingEnabled = true;
            this.lbImages.IntegralHeight = false;
            this.lbImages.Location = new System.Drawing.Point(12, 25);
            this.lbImages.Name = "lbImages";
            this.lbImages.Size = new System.Drawing.Size(170, 319);
            this.lbImages.Sorted = true;
            this.lbImages.TabIndex = 1;
            this.lbImages.SelectedIndexChanged += new System.EventHandler(this.lbImage_SelectedIndexChanged);
            this.lbImages.DoubleClick += new System.EventHandler(this.lbImage_DoubleClick);
            // 
            // btnDel
            // 
            this.btnDel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnDel.Image = ((System.Drawing.Image)(resources.GetObject("btnDel.Image")));
            this.btnDel.Location = new System.Drawing.Point(70, 350);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(23, 23);
            this.btnDel.TabIndex = 7;
            this.btnDel.UseVisualStyleBackColor = true;
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // btnOpen
            // 
            this.btnOpen.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnOpen.Image = ((System.Drawing.Image)(resources.GetObject("btnOpen.Image")));
            this.btnOpen.Location = new System.Drawing.Point(12, 350);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(23, 23);
            this.btnOpen.TabIndex = 5;
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // propGrid
            // 
            this.propGrid.HelpVisible = false;
            this.propGrid.Location = new System.Drawing.Point(188, 25);
            this.propGrid.Name = "propGrid";
            this.propGrid.Size = new System.Drawing.Size(230, 120);
            this.propGrid.TabIndex = 3;
            this.propGrid.ToolbarVisible = false;
            this.propGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propGrid_PropertyValueChanged);
            // 
            // pictureBox
            // 
            this.pictureBox.BackColor = System.Drawing.SystemColors.Window;
            this.pictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox.Location = new System.Drawing.Point(188, 164);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(230, 180);
            this.pictureBox.TabIndex = 9;
            this.pictureBox.TabStop = false;
            // 
            // lblImages
            // 
            this.lblImages.AutoSize = true;
            this.lblImages.Location = new System.Drawing.Point(9, 9);
            this.lblImages.Name = "lblImages";
            this.lblImages.Size = new System.Drawing.Size(41, 13);
            this.lblImages.TabIndex = 0;
            this.lblImages.Text = "Images";
            // 
            // lblImageProps
            // 
            this.lblImageProps.AutoSize = true;
            this.lblImageProps.Location = new System.Drawing.Point(185, 9);
            this.lblImageProps.Name = "lblImageProps";
            this.lblImageProps.Size = new System.Drawing.Size(85, 13);
            this.lblImageProps.TabIndex = 2;
            this.lblImageProps.Text = "Image properties";
            // 
            // lblPreview
            // 
            this.lblPreview.AutoSize = true;
            this.lblPreview.Location = new System.Drawing.Point(185, 148);
            this.lblPreview.Name = "lblPreview";
            this.lblPreview.Size = new System.Drawing.Size(45, 13);
            this.lblPreview.TabIndex = 4;
            this.lblPreview.Text = "Preview";
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "Images (*.jpg;*.png;*.gif;*.svg)|*.jpg;*.png;*.gif;*.svg|All Files (*.*)|*.*";
            // 
            // btnSave
            // 
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.Location = new System.Drawing.Point(41, 350);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(23, 23);
            this.btnSave.TabIndex = 6;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.Filter = "Images (*.jpg;*.png;*.gif;*.svg)|*.jpg;*.png;*.gif;*.svg|All Files (*.*)|*.*";
            // 
            // FrmImageDialog
            // 
            this.AcceptButton = this.btnSelect;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(430, 385);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.lblPreview);
            this.Controls.Add(this.lblImageProps);
            this.Controls.Add(this.lblImages);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.propGrid);
            this.Controls.Add(this.btnDel);
            this.Controls.Add(this.btnOpen);
            this.Controls.Add(this.lbImages);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSelect);
            this.Controls.Add(this.btnSelectEmpty);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmImageDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Images";
            this.Load += new System.EventHandler(this.FrmImageDialog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSelectEmpty;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ListBox lbImages;
        private System.Windows.Forms.Button btnDel;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.PropertyGrid propGrid;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Label lblImages;
        private System.Windows.Forms.Label lblImageProps;
        private System.Windows.Forms.Label lblPreview;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
    }
}