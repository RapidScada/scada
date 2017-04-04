namespace Scada.Scheme.Editor
{
    partial class FrmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.btnFileNew = new System.Windows.Forms.ToolStripButton();
            this.btnFileOpen = new System.Windows.Forms.ToolStripButton();
            this.btnFileSave = new System.Windows.Forms.ToolStripSplitButton();
            this.miFileSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.sep1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnEditCut = new System.Windows.Forms.ToolStripButton();
            this.btnEditCopy = new System.Windows.Forms.ToolStripButton();
            this.btnEditPaste = new System.Windows.Forms.ToolStripButton();
            this.sep2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnSchemePointer = new System.Windows.Forms.ToolStripButton();
            this.btnSchemeDelete = new System.Windows.Forms.ToolStripButton();
            this.sep3 = new System.Windows.Forms.ToolStripSeparator();
            this.btnHelpAbout = new System.Windows.Forms.ToolStripButton();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.btnFileOpenBrowser = new System.Windows.Forms.ToolStripButton();
            this.toolStrip.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnFileNew,
            this.btnFileOpen,
            this.btnFileSave,
            this.btnFileOpenBrowser,
            this.sep1,
            this.btnEditCut,
            this.btnEditCopy,
            this.btnEditPaste,
            this.sep2,
            this.btnSchemePointer,
            this.btnSchemeDelete,
            this.sep3,
            this.btnHelpAbout});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(284, 25);
            this.toolStrip.TabIndex = 0;
            this.toolStrip.MouseMove += new System.Windows.Forms.MouseEventHandler(this.FrmMain_MouseMove);
            // 
            // btnFileNew
            // 
            this.btnFileNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnFileNew.Image = ((System.Drawing.Image)(resources.GetObject("btnFileNew.Image")));
            this.btnFileNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnFileNew.Name = "btnFileNew";
            this.btnFileNew.Size = new System.Drawing.Size(23, 22);
            this.btnFileNew.ToolTipText = "Create new scheme";
            this.btnFileNew.Click += new System.EventHandler(this.btnFileNew_Click);
            // 
            // btnFileOpen
            // 
            this.btnFileOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnFileOpen.Image = ((System.Drawing.Image)(resources.GetObject("btnFileOpen.Image")));
            this.btnFileOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnFileOpen.Name = "btnFileOpen";
            this.btnFileOpen.Size = new System.Drawing.Size(23, 22);
            this.btnFileOpen.ToolTipText = "Open scheme";
            this.btnFileOpen.Click += new System.EventHandler(this.btnFileOpen_Click);
            // 
            // btnFileSave
            // 
            this.btnFileSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnFileSave.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miFileSaveAs});
            this.btnFileSave.Image = ((System.Drawing.Image)(resources.GetObject("btnFileSave.Image")));
            this.btnFileSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnFileSave.Name = "btnFileSave";
            this.btnFileSave.Size = new System.Drawing.Size(32, 22);
            this.btnFileSave.ToolTipText = "Save scheme";
            this.btnFileSave.ButtonClick += new System.EventHandler(this.btnFileSave_ButtonClick);
            // 
            // miFileSaveAs
            // 
            this.miFileSaveAs.Image = ((System.Drawing.Image)(resources.GetObject("miFileSaveAs.Image")));
            this.miFileSaveAs.Name = "miFileSaveAs";
            this.miFileSaveAs.Size = new System.Drawing.Size(152, 22);
            this.miFileSaveAs.Text = "Save As...";
            this.miFileSaveAs.Click += new System.EventHandler(this.miFileSaveAs_Click);
            // 
            // sep1
            // 
            this.sep1.Name = "sep1";
            this.sep1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnEditCut
            // 
            this.btnEditCut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnEditCut.Image = ((System.Drawing.Image)(resources.GetObject("btnEditCut.Image")));
            this.btnEditCut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnEditCut.Name = "btnEditCut";
            this.btnEditCut.Size = new System.Drawing.Size(23, 22);
            this.btnEditCut.ToolTipText = "Cut scheme component";
            this.btnEditCut.Click += new System.EventHandler(this.btnEditCut_Click);
            // 
            // btnEditCopy
            // 
            this.btnEditCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnEditCopy.Image = ((System.Drawing.Image)(resources.GetObject("btnEditCopy.Image")));
            this.btnEditCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnEditCopy.Name = "btnEditCopy";
            this.btnEditCopy.Size = new System.Drawing.Size(23, 22);
            this.btnEditCopy.Text = "toolStripButton5";
            this.btnEditCopy.ToolTipText = "Copy scheme component";
            this.btnEditCopy.Click += new System.EventHandler(this.btnEditCopy_Click);
            // 
            // btnEditPaste
            // 
            this.btnEditPaste.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnEditPaste.Image = ((System.Drawing.Image)(resources.GetObject("btnEditPaste.Image")));
            this.btnEditPaste.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnEditPaste.Name = "btnEditPaste";
            this.btnEditPaste.Size = new System.Drawing.Size(23, 22);
            this.btnEditPaste.Text = "toolStripButton1";
            this.btnEditPaste.ToolTipText = "Paste scheme component";
            this.btnEditPaste.Click += new System.EventHandler(this.btnEditPaste_Click);
            // 
            // sep2
            // 
            this.sep2.Name = "sep2";
            this.sep2.Size = new System.Drawing.Size(6, 25);
            // 
            // btnSchemePointer
            // 
            this.btnSchemePointer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSchemePointer.Image = ((System.Drawing.Image)(resources.GetObject("btnSchemePointer.Image")));
            this.btnSchemePointer.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSchemePointer.Name = "btnSchemePointer";
            this.btnSchemePointer.Size = new System.Drawing.Size(23, 22);
            this.btnSchemePointer.ToolTipText = "Cancel adding component";
            this.btnSchemePointer.Click += new System.EventHandler(this.btnSchemePointer_Click);
            // 
            // btnSchemeDelete
            // 
            this.btnSchemeDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSchemeDelete.Image = ((System.Drawing.Image)(resources.GetObject("btnSchemeDelete.Image")));
            this.btnSchemeDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSchemeDelete.Name = "btnSchemeDelete";
            this.btnSchemeDelete.Size = new System.Drawing.Size(23, 22);
            this.btnSchemeDelete.ToolTipText = "Delete selected component";
            this.btnSchemeDelete.Click += new System.EventHandler(this.btnSchemeDelete_Click);
            // 
            // sep3
            // 
            this.sep3.Name = "sep3";
            this.sep3.Size = new System.Drawing.Size(6, 25);
            // 
            // btnHelpAbout
            // 
            this.btnHelpAbout.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnHelpAbout.Image = ((System.Drawing.Image)(resources.GetObject("btnHelpAbout.Image")));
            this.btnHelpAbout.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnHelpAbout.Name = "btnHelpAbout";
            this.btnHelpAbout.Size = new System.Drawing.Size(23, 22);
            this.btnHelpAbout.ToolTipText = "About";
            this.btnHelpAbout.Click += new System.EventHandler(this.btnHelpAbout_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
            this.statusStrip.Location = new System.Drawing.Point(0, 439);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(284, 22);
            this.statusStrip.TabIndex = 2;
            this.statusStrip.MouseMove += new System.Windows.Forms.MouseEventHandler(this.FrmMain_MouseMove);
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(52, 17);
            this.lblStatus.Text = "lblStatus";
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPage3);
            this.tabControl.Controls.Add(this.tabPage1);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 25);
            this.tabControl.Multiline = true;
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(284, 414);
            this.tabControl.TabIndex = 1;
            this.tabControl.MouseMove += new System.Windows.Forms.MouseEventHandler(this.FrmMain_MouseMove);
            // 
            // tabPage3
            // 
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(276, 388);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Components";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.propertyGrid1);
            this.tabPage1.Controls.Add(this.comboBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(276, 388);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Properties";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid1.Location = new System.Drawing.Point(3, 24);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(270, 361);
            this.propertyGrid1.TabIndex = 1;
            // 
            // comboBox1
            // 
            this.comboBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(3, 3);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(270, 21);
            this.comboBox1.TabIndex = 0;
            // 
            // btnFileOpenBrowser
            // 
            this.btnFileOpenBrowser.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnFileOpenBrowser.Image = ((System.Drawing.Image)(resources.GetObject("btnFileOpenBrowser.Image")));
            this.btnFileOpenBrowser.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnFileOpenBrowser.Name = "btnFileOpenBrowser";
            this.btnFileOpenBrowser.Size = new System.Drawing.Size(23, 22);
            this.btnFileOpenBrowser.ToolTipText = "Open new browser tab";
            this.btnFileOpenBrowser.Click += new System.EventHandler(this.btnFileOpenBrowser_Click);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 461);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.statusStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(200, 300);
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Scheme Editor";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMain_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmMain_FormClosed);
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.FrmMain_MouseMove);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton btnFileNew;
        private System.Windows.Forms.ToolStripButton btnFileOpen;
        private System.Windows.Forms.ToolStripButton btnEditCut;
        private System.Windows.Forms.ToolStripButton btnEditCopy;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.ToolStripSeparator sep1;
        private System.Windows.Forms.ToolStripSplitButton btnFileSave;
        private System.Windows.Forms.ToolStripMenuItem miFileSaveAs;
        private System.Windows.Forms.ToolStripButton btnEditPaste;
        private System.Windows.Forms.ToolStripSeparator sep2;
        private System.Windows.Forms.ToolStripButton btnSchemePointer;
        private System.Windows.Forms.ToolStripButton btnSchemeDelete;
        private System.Windows.Forms.ToolStripSeparator sep3;
        private System.Windows.Forms.ToolStripButton btnHelpAbout;
        private System.Windows.Forms.ToolStripButton btnFileOpenBrowser;
    }
}

