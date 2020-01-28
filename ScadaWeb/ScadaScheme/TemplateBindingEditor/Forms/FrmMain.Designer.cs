namespace Scada.Scheme.TemplateBindingEditor.Forms
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.btnFileNew = new System.Windows.Forms.ToolStripButton();
            this.btnFileOpen = new System.Windows.Forms.ToolStripButton();
            this.btnFileSave = new System.Windows.Forms.ToolStripButton();
            this.btnFileSaveAs = new System.Windows.Forms.ToolStripButton();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabGeneral = new System.Windows.Forms.TabPage();
            this.cbTitleComponent = new System.Windows.Forms.ComboBox();
            this.lblTitleComponent = new System.Windows.Forms.Label();
            this.btnReloadTemplate = new System.Windows.Forms.Button();
            this.btnBrowseTemplate = new System.Windows.Forms.Button();
            this.txtTemplateFileName = new System.Windows.Forms.TextBox();
            this.lblTemplateFileName = new System.Windows.Forms.Label();
            this.tabBindings = new System.Windows.Forms.TabPage();
            this.dgvBindings = new System.Windows.Forms.DataGridView();
            this.colComponent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCnlNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCtrlCnlNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bsBindings = new System.Windows.Forms.BindingSource(this.components);
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.lblInterfaceDir = new System.Windows.Forms.ToolStripStatusLabel();
            this.ofdBindings = new System.Windows.Forms.OpenFileDialog();
            this.sfdBindings = new System.Windows.Forms.SaveFileDialog();
            this.ofdScheme = new System.Windows.Forms.OpenFileDialog();
            this.toolStrip.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabGeneral.SuspendLayout();
            this.tabBindings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBindings)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsBindings)).BeginInit();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnFileNew,
            this.btnFileOpen,
            this.btnFileSave,
            this.btnFileSaveAs});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(734, 25);
            this.toolStrip.TabIndex = 0;
            this.toolStrip.Text = "toolStrip1";
            // 
            // btnFileNew
            // 
            this.btnFileNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnFileNew.Image = ((System.Drawing.Image)(resources.GetObject("btnFileNew.Image")));
            this.btnFileNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnFileNew.Name = "btnFileNew";
            this.btnFileNew.Size = new System.Drawing.Size(23, 22);
            this.btnFileNew.ToolTipText = "New Bindings (Ctrl + N)";
            this.btnFileNew.Click += new System.EventHandler(this.btnFileNew_Click);
            // 
            // btnFileOpen
            // 
            this.btnFileOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnFileOpen.Image = ((System.Drawing.Image)(resources.GetObject("btnFileOpen.Image")));
            this.btnFileOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnFileOpen.Name = "btnFileOpen";
            this.btnFileOpen.Size = new System.Drawing.Size(23, 22);
            this.btnFileOpen.ToolTipText = "Open Bindings (Ctrl + O)";
            this.btnFileOpen.Click += new System.EventHandler(this.btnFileOpen_Click);
            // 
            // btnFileSave
            // 
            this.btnFileSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnFileSave.Image = ((System.Drawing.Image)(resources.GetObject("btnFileSave.Image")));
            this.btnFileSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnFileSave.Name = "btnFileSave";
            this.btnFileSave.Size = new System.Drawing.Size(23, 22);
            this.btnFileSave.ToolTipText = "Save Bindings (Ctrl + S)";
            this.btnFileSave.Click += new System.EventHandler(this.btnFileSave_Click);
            // 
            // btnFileSaveAs
            // 
            this.btnFileSaveAs.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnFileSaveAs.Image = ((System.Drawing.Image)(resources.GetObject("btnFileSaveAs.Image")));
            this.btnFileSaveAs.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnFileSaveAs.Name = "btnFileSaveAs";
            this.btnFileSaveAs.Size = new System.Drawing.Size(23, 22);
            this.btnFileSaveAs.Text = "toolStripButton1";
            this.btnFileSaveAs.ToolTipText = "Save Bindings As";
            this.btnFileSaveAs.Click += new System.EventHandler(this.btnFileSaveAs_Click);
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabGeneral);
            this.tabControl.Controls.Add(this.tabBindings);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 25);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(734, 464);
            this.tabControl.TabIndex = 1;
            // 
            // tabGeneral
            // 
            this.tabGeneral.Controls.Add(this.cbTitleComponent);
            this.tabGeneral.Controls.Add(this.lblTitleComponent);
            this.tabGeneral.Controls.Add(this.btnReloadTemplate);
            this.tabGeneral.Controls.Add(this.btnBrowseTemplate);
            this.tabGeneral.Controls.Add(this.txtTemplateFileName);
            this.tabGeneral.Controls.Add(this.lblTemplateFileName);
            this.tabGeneral.Location = new System.Drawing.Point(4, 22);
            this.tabGeneral.Name = "tabGeneral";
            this.tabGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tabGeneral.Size = new System.Drawing.Size(726, 438);
            this.tabGeneral.TabIndex = 0;
            this.tabGeneral.Text = "General Parameters";
            this.tabGeneral.UseVisualStyleBackColor = true;
            // 
            // cbTitleComponent
            // 
            this.cbTitleComponent.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTitleComponent.FormattingEnabled = true;
            this.cbTitleComponent.Location = new System.Drawing.Point(6, 58);
            this.cbTitleComponent.Name = "cbTitleComponent";
            this.cbTitleComponent.Size = new System.Drawing.Size(400, 21);
            this.cbTitleComponent.TabIndex = 5;
            this.cbTitleComponent.SelectedIndexChanged += new System.EventHandler(this.cbTitleComponent_SelectedIndexChanged);
            // 
            // lblTitleComponent
            // 
            this.lblTitleComponent.AutoSize = true;
            this.lblTitleComponent.Location = new System.Drawing.Point(3, 42);
            this.lblTitleComponent.Name = "lblTitleComponent";
            this.lblTitleComponent.Size = new System.Drawing.Size(83, 13);
            this.lblTitleComponent.TabIndex = 4;
            this.lblTitleComponent.Text = "Title component";
            // 
            // btnReloadTemplate
            // 
            this.btnReloadTemplate.Location = new System.Drawing.Point(493, 18);
            this.btnReloadTemplate.Name = "btnReloadTemplate";
            this.btnReloadTemplate.Size = new System.Drawing.Size(75, 23);
            this.btnReloadTemplate.TabIndex = 3;
            this.btnReloadTemplate.Text = "Reload";
            this.btnReloadTemplate.UseVisualStyleBackColor = true;
            this.btnReloadTemplate.Click += new System.EventHandler(this.btnReloadTemplate_Click);
            // 
            // btnBrowseTemplate
            // 
            this.btnBrowseTemplate.Location = new System.Drawing.Point(412, 18);
            this.btnBrowseTemplate.Name = "btnBrowseTemplate";
            this.btnBrowseTemplate.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseTemplate.TabIndex = 2;
            this.btnBrowseTemplate.Text = "Browse...";
            this.btnBrowseTemplate.UseVisualStyleBackColor = true;
            this.btnBrowseTemplate.Click += new System.EventHandler(this.btnBrowseTemplate_Click);
            // 
            // txtTemplateFileName
            // 
            this.txtTemplateFileName.Location = new System.Drawing.Point(6, 19);
            this.txtTemplateFileName.Name = "txtTemplateFileName";
            this.txtTemplateFileName.Size = new System.Drawing.Size(400, 20);
            this.txtTemplateFileName.TabIndex = 1;
            this.txtTemplateFileName.TextChanged += new System.EventHandler(this.txtTemplateFileName_TextChanged);
            // 
            // lblTemplateFileName
            // 
            this.lblTemplateFileName.AutoSize = true;
            this.lblTemplateFileName.Location = new System.Drawing.Point(3, 3);
            this.lblTemplateFileName.Name = "lblTemplateFileName";
            this.lblTemplateFileName.Size = new System.Drawing.Size(89, 13);
            this.lblTemplateFileName.TabIndex = 0;
            this.lblTemplateFileName.Text = "Scheme template";
            // 
            // tabBindings
            // 
            this.tabBindings.Controls.Add(this.dgvBindings);
            this.tabBindings.Location = new System.Drawing.Point(4, 22);
            this.tabBindings.Name = "tabBindings";
            this.tabBindings.Padding = new System.Windows.Forms.Padding(3);
            this.tabBindings.Size = new System.Drawing.Size(726, 438);
            this.tabBindings.TabIndex = 1;
            this.tabBindings.Text = "Component Bindings";
            this.tabBindings.UseVisualStyleBackColor = true;
            // 
            // dgvBindings
            // 
            this.dgvBindings.AllowUserToAddRows = false;
            this.dgvBindings.AllowUserToDeleteRows = false;
            this.dgvBindings.AllowUserToResizeRows = false;
            this.dgvBindings.AutoGenerateColumns = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvBindings.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvBindings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBindings.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colComponent,
            this.colCnlNum,
            this.colCtrlCnlNum});
            this.dgvBindings.DataSource = this.bsBindings;
            this.dgvBindings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvBindings.Location = new System.Drawing.Point(3, 3);
            this.dgvBindings.Name = "dgvBindings";
            this.dgvBindings.Size = new System.Drawing.Size(720, 432);
            this.dgvBindings.TabIndex = 0;
            // 
            // colComponent
            // 
            this.colComponent.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colComponent.DataPropertyName = "CompDisplayName";
            this.colComponent.HeaderText = "Component";
            this.colComponent.Name = "colComponent";
            this.colComponent.ReadOnly = true;
            this.colComponent.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colCnlNum
            // 
            this.colCnlNum.DataPropertyName = "InCnlNum";
            this.colCnlNum.HeaderText = "Input Channel";
            this.colCnlNum.Name = "colCnlNum";
            this.colCnlNum.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colCnlNum.Width = 120;
            // 
            // colCtrlCnlNum
            // 
            this.colCtrlCnlNum.DataPropertyName = "CtrlCnlNum";
            this.colCtrlCnlNum.HeaderText = "Output Channel";
            this.colCtrlCnlNum.Name = "colCtrlCnlNum";
            this.colCtrlCnlNum.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colCtrlCnlNum.Width = 120;
            // 
            // bsBindings
            // 
            this.bsBindings.AllowNew = false;
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblInterfaceDir});
            this.statusStrip.Location = new System.Drawing.Point(0, 489);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.ShowItemToolTips = true;
            this.statusStrip.Size = new System.Drawing.Size(734, 22);
            this.statusStrip.TabIndex = 2;
            this.statusStrip.Text = "statusStrip1";
            // 
            // lblInterfaceDir
            // 
            this.lblInterfaceDir.Image = ((System.Drawing.Image)(resources.GetObject("lblInterfaceDir.Image")));
            this.lblInterfaceDir.Name = "lblInterfaceDir";
            this.lblInterfaceDir.Size = new System.Drawing.Size(97, 17);
            this.lblInterfaceDir.Text = "lblInterfaceDir";
            this.lblInterfaceDir.ToolTipText = "The interface directory of the project";
            // 
            // ofdBindings
            // 
            this.ofdBindings.DefaultExt = "*.stb";
            this.ofdBindings.Filter = "Template Bindings (*.stb)|*.stb|All Files (*.*)|*.*";
            // 
            // sfdBindings
            // 
            this.sfdBindings.DefaultExt = "*.stb";
            this.sfdBindings.Filter = "Template Bindings (*.stb)|*.stb|All Files (*.*)|*.*";
            // 
            // ofdScheme
            // 
            this.ofdScheme.DefaultExt = "*.sch";
            this.ofdScheme.Filter = "Schemes (*.sch)|*.sch|All Files (*.*)|*.*";
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(734, 511);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.toolStrip);
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Template Binding Editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMain_FormClosing);
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.tabGeneral.ResumeLayout(false);
            this.tabGeneral.PerformLayout();
            this.tabBindings.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBindings)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsBindings)).EndInit();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton btnFileNew;
        private System.Windows.Forms.ToolStripButton btnFileOpen;
        private System.Windows.Forms.ToolStripButton btnFileSave;
        private System.Windows.Forms.ToolStripButton btnFileSaveAs;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabGeneral;
        private System.Windows.Forms.TabPage tabBindings;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.Label lblTemplateFileName;
        private System.Windows.Forms.TextBox txtTemplateFileName;
        private System.Windows.Forms.Button btnBrowseTemplate;
        private System.Windows.Forms.ComboBox cbTitleComponent;
        private System.Windows.Forms.Label lblTitleComponent;
        private System.Windows.Forms.Button btnReloadTemplate;
        private System.Windows.Forms.DataGridView dgvBindings;
        private System.Windows.Forms.ToolStripStatusLabel lblInterfaceDir;
        private System.Windows.Forms.OpenFileDialog ofdBindings;
        private System.Windows.Forms.SaveFileDialog sfdBindings;
        private System.Windows.Forms.OpenFileDialog ofdScheme;
        private System.Windows.Forms.BindingSource bsBindings;
        private System.Windows.Forms.DataGridViewTextBoxColumn colComponent;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCnlNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCtrlCnlNum;
    }
}

