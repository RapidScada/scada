namespace ScadaWebConfig
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
            this.lblConfigDir = new System.Windows.Forms.Label();
            this.txtConfigDir = new System.Windows.Forms.TextBox();
            this.btnConfigDir = new System.Windows.Forms.Button();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabComm = new System.Windows.Forms.TabPage();
            this.lblServerPwd = new System.Windows.Forms.Label();
            this.txtServerPwd = new System.Windows.Forms.TextBox();
            this.txtServerUser = new System.Windows.Forms.TextBox();
            this.lblServerUser = new System.Windows.Forms.Label();
            this.lblServerTimeout = new System.Windows.Forms.Label();
            this.numServerTimeout = new System.Windows.Forms.NumericUpDown();
            this.lblServerPort = new System.Windows.Forms.Label();
            this.numServerPort = new System.Windows.Forms.NumericUpDown();
            this.lblServerHost = new System.Windows.Forms.Label();
            this.txtServerHost = new System.Windows.Forms.TextBox();
            this.tabWeb = new System.Windows.Forms.TabPage();
            this.chkSimpleCmd = new System.Windows.Forms.CheckBox();
            this.chkRemEnabled = new System.Windows.Forms.CheckBox();
            this.chkCmdEnabled = new System.Windows.Forms.CheckBox();
            this.numDiagBreak = new System.Windows.Forms.NumericUpDown();
            this.numEventRefrFreq = new System.Windows.Forms.NumericUpDown();
            this.numEventCnt = new System.Windows.Forms.NumericUpDown();
            this.numSrezRefrFreq = new System.Windows.Forms.NumericUpDown();
            this.lblSrezRefrFreq = new System.Windows.Forms.Label();
            this.lblDiagBreak = new System.Windows.Forms.Label();
            this.lblEventCnt = new System.Windows.Forms.Label();
            this.lblEventRefrFreq = new System.Windows.Forms.Label();
            this.chkEventFltr = new System.Windows.Forms.CheckBox();
            this.tabView = new System.Windows.Forms.TabPage();
            this.lblFile = new System.Windows.Forms.Label();
            this.cbType = new System.Windows.Forms.ComboBox();
            this.lblType = new System.Windows.Forms.Label();
            this.btnSelectView = new System.Windows.Forms.Button();
            this.btnUp = new System.Windows.Forms.Button();
            this.btnDown = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnAddView = new System.Windows.Forms.Button();
            this.btnAddViewSet = new System.Windows.Forms.Button();
            this.txtDirOrFile = new System.Windows.Forms.TextBox();
            this.lblDir = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.tvTableSets = new System.Windows.Forms.TreeView();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.btnCancel = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.tabControl.SuspendLayout();
            this.tabComm.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numServerTimeout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numServerPort)).BeginInit();
            this.tabWeb.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDiagBreak)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numEventRefrFreq)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numEventCnt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSrezRefrFreq)).BeginInit();
            this.tabView.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblConfigDir
            // 
            this.lblConfigDir.AutoSize = true;
            this.lblConfigDir.Location = new System.Drawing.Point(9, 9);
            this.lblConfigDir.Name = "lblConfigDir";
            this.lblConfigDir.Size = new System.Drawing.Size(144, 13);
            this.lblConfigDir.TabIndex = 0;
            this.lblConfigDir.Text = "Директория конфигурации";
            // 
            // txtConfigDir
            // 
            this.txtConfigDir.Location = new System.Drawing.Point(12, 25);
            this.txtConfigDir.Name = "txtConfigDir";
            this.txtConfigDir.ReadOnly = true;
            this.txtConfigDir.Size = new System.Drawing.Size(440, 20);
            this.txtConfigDir.TabIndex = 1;
            this.txtConfigDir.Text = "C:\\SCADA\\ScadaWeb\\config\\";
            // 
            // btnConfigDir
            // 
            this.btnConfigDir.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnConfigDir.Image = ((System.Drawing.Image)(resources.GetObject("btnConfigDir.Image")));
            this.btnConfigDir.Location = new System.Drawing.Point(458, 25);
            this.btnConfigDir.Name = "btnConfigDir";
            this.btnConfigDir.Size = new System.Drawing.Size(20, 20);
            this.btnConfigDir.TabIndex = 2;
            this.btnConfigDir.UseVisualStyleBackColor = true;
            this.btnConfigDir.Click += new System.EventHandler(this.btnConfigDir_Click);
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabComm);
            this.tabControl.Controls.Add(this.tabWeb);
            this.tabControl.Controls.Add(this.tabView);
            this.tabControl.Location = new System.Drawing.Point(12, 51);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(470, 330);
            this.tabControl.TabIndex = 3;
            // 
            // tabComm
            // 
            this.tabComm.Controls.Add(this.lblServerPwd);
            this.tabComm.Controls.Add(this.txtServerPwd);
            this.tabComm.Controls.Add(this.txtServerUser);
            this.tabComm.Controls.Add(this.lblServerUser);
            this.tabComm.Controls.Add(this.lblServerTimeout);
            this.tabComm.Controls.Add(this.numServerTimeout);
            this.tabComm.Controls.Add(this.lblServerPort);
            this.tabComm.Controls.Add(this.numServerPort);
            this.tabComm.Controls.Add(this.lblServerHost);
            this.tabComm.Controls.Add(this.txtServerHost);
            this.tabComm.Location = new System.Drawing.Point(4, 22);
            this.tabComm.Name = "tabComm";
            this.tabComm.Padding = new System.Windows.Forms.Padding(3);
            this.tabComm.Size = new System.Drawing.Size(462, 304);
            this.tabComm.TabIndex = 0;
            this.tabComm.Text = "Соединение";
            this.tabComm.UseVisualStyleBackColor = true;
            // 
            // lblServerPwd
            // 
            this.lblServerPwd.AutoSize = true;
            this.lblServerPwd.Location = new System.Drawing.Point(110, 46);
            this.lblServerPwd.Name = "lblServerPwd";
            this.lblServerPwd.Size = new System.Drawing.Size(45, 13);
            this.lblServerPwd.TabIndex = 8;
            this.lblServerPwd.Text = "Пароль";
            // 
            // txtServerPwd
            // 
            this.txtServerPwd.Location = new System.Drawing.Point(113, 62);
            this.txtServerPwd.Name = "txtServerPwd";
            this.txtServerPwd.Size = new System.Drawing.Size(100, 20);
            this.txtServerPwd.TabIndex = 9;
            this.txtServerPwd.Text = "12345";
            this.txtServerPwd.UseSystemPasswordChar = true;
            this.txtServerPwd.TextChanged += new System.EventHandler(this.dataSettings_Changed);
            // 
            // txtServerUser
            // 
            this.txtServerUser.Location = new System.Drawing.Point(6, 62);
            this.txtServerUser.Name = "txtServerUser";
            this.txtServerUser.Size = new System.Drawing.Size(100, 20);
            this.txtServerUser.TabIndex = 7;
            this.txtServerUser.Text = "ScadaWeb";
            this.txtServerUser.TextChanged += new System.EventHandler(this.dataSettings_Changed);
            // 
            // lblServerUser
            // 
            this.lblServerUser.AutoSize = true;
            this.lblServerUser.Location = new System.Drawing.Point(3, 46);
            this.lblServerUser.Name = "lblServerUser";
            this.lblServerUser.Size = new System.Drawing.Size(80, 13);
            this.lblServerUser.TabIndex = 6;
            this.lblServerUser.Text = "Пользователь";
            // 
            // lblServerTimeout
            // 
            this.lblServerTimeout.AutoSize = true;
            this.lblServerTimeout.Location = new System.Drawing.Point(216, 6);
            this.lblServerTimeout.Name = "lblServerTimeout";
            this.lblServerTimeout.Size = new System.Drawing.Size(50, 13);
            this.lblServerTimeout.TabIndex = 4;
            this.lblServerTimeout.Text = "Таймаут";
            // 
            // numServerTimeout
            // 
            this.numServerTimeout.Location = new System.Drawing.Point(219, 22);
            this.numServerTimeout.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numServerTimeout.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numServerTimeout.Name = "numServerTimeout";
            this.numServerTimeout.Size = new System.Drawing.Size(100, 20);
            this.numServerTimeout.TabIndex = 5;
            this.numServerTimeout.Value = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numServerTimeout.ValueChanged += new System.EventHandler(this.dataSettings_Changed);
            // 
            // lblServerPort
            // 
            this.lblServerPort.AutoSize = true;
            this.lblServerPort.Location = new System.Drawing.Point(110, 6);
            this.lblServerPort.Name = "lblServerPort";
            this.lblServerPort.Size = new System.Drawing.Size(32, 13);
            this.lblServerPort.TabIndex = 2;
            this.lblServerPort.Text = "Порт";
            // 
            // numServerPort
            // 
            this.numServerPort.Location = new System.Drawing.Point(113, 22);
            this.numServerPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numServerPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numServerPort.Name = "numServerPort";
            this.numServerPort.Size = new System.Drawing.Size(100, 20);
            this.numServerPort.TabIndex = 3;
            this.numServerPort.Value = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numServerPort.ValueChanged += new System.EventHandler(this.dataSettings_Changed);
            // 
            // lblServerHost
            // 
            this.lblServerHost.AutoSize = true;
            this.lblServerHost.Location = new System.Drawing.Point(3, 6);
            this.lblServerHost.Name = "lblServerHost";
            this.lblServerHost.Size = new System.Drawing.Size(44, 13);
            this.lblServerHost.TabIndex = 0;
            this.lblServerHost.Text = "Сервер";
            // 
            // txtServerHost
            // 
            this.txtServerHost.Location = new System.Drawing.Point(6, 22);
            this.txtServerHost.Name = "txtServerHost";
            this.txtServerHost.Size = new System.Drawing.Size(100, 20);
            this.txtServerHost.TabIndex = 1;
            this.txtServerHost.Text = "localhost";
            this.txtServerHost.TextChanged += new System.EventHandler(this.dataSettings_Changed);
            // 
            // tabWeb
            // 
            this.tabWeb.Controls.Add(this.chkSimpleCmd);
            this.tabWeb.Controls.Add(this.chkRemEnabled);
            this.tabWeb.Controls.Add(this.chkCmdEnabled);
            this.tabWeb.Controls.Add(this.numDiagBreak);
            this.tabWeb.Controls.Add(this.numEventRefrFreq);
            this.tabWeb.Controls.Add(this.numEventCnt);
            this.tabWeb.Controls.Add(this.numSrezRefrFreq);
            this.tabWeb.Controls.Add(this.lblSrezRefrFreq);
            this.tabWeb.Controls.Add(this.lblDiagBreak);
            this.tabWeb.Controls.Add(this.lblEventCnt);
            this.tabWeb.Controls.Add(this.lblEventRefrFreq);
            this.tabWeb.Controls.Add(this.chkEventFltr);
            this.tabWeb.Location = new System.Drawing.Point(4, 22);
            this.tabWeb.Name = "tabWeb";
            this.tabWeb.Padding = new System.Windows.Forms.Padding(3);
            this.tabWeb.Size = new System.Drawing.Size(462, 304);
            this.tabWeb.TabIndex = 1;
            this.tabWeb.Text = "Отображение";
            this.tabWeb.UseVisualStyleBackColor = true;
            // 
            // chkSimpleCmd
            // 
            this.chkSimpleCmd.AutoSize = true;
            this.chkSimpleCmd.Location = new System.Drawing.Point(9, 166);
            this.chkSimpleCmd.Name = "chkSimpleCmd";
            this.chkSimpleCmd.Size = new System.Drawing.Size(222, 17);
            this.chkSimpleCmd.TabIndex = 10;
            this.chkSimpleCmd.Text = "Простая отправка команд управления";
            this.chkSimpleCmd.UseVisualStyleBackColor = true;
            this.chkSimpleCmd.CheckedChanged += new System.EventHandler(this.webSettings_Changed);
            // 
            // chkRemEnabled
            // 
            this.chkRemEnabled.AutoSize = true;
            this.chkRemEnabled.Location = new System.Drawing.Point(9, 189);
            this.chkRemEnabled.Name = "chkRemEnabled";
            this.chkRemEnabled.Size = new System.Drawing.Size(233, 17);
            this.chkRemEnabled.TabIndex = 11;
            this.chkRemEnabled.Text = "Разрешение запоминать вход в систему";
            this.chkRemEnabled.UseVisualStyleBackColor = true;
            this.chkRemEnabled.CheckedChanged += new System.EventHandler(this.webSettings_Changed);
            // 
            // chkCmdEnabled
            // 
            this.chkCmdEnabled.AutoSize = true;
            this.chkCmdEnabled.Checked = true;
            this.chkCmdEnabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCmdEnabled.Location = new System.Drawing.Point(9, 143);
            this.chkCmdEnabled.Name = "chkCmdEnabled";
            this.chkCmdEnabled.Size = new System.Drawing.Size(192, 17);
            this.chkCmdEnabled.TabIndex = 9;
            this.chkCmdEnabled.Text = "Разрешение команд управления";
            this.chkCmdEnabled.UseVisualStyleBackColor = true;
            this.chkCmdEnabled.CheckedChanged += new System.EventHandler(this.webSettings_Changed);
            // 
            // numDiagBreak
            // 
            this.numDiagBreak.Location = new System.Drawing.Point(203, 112);
            this.numDiagBreak.Maximum = new decimal(new int[] {
            3600,
            0,
            0,
            0});
            this.numDiagBreak.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numDiagBreak.Name = "numDiagBreak";
            this.numDiagBreak.Size = new System.Drawing.Size(60, 20);
            this.numDiagBreak.TabIndex = 8;
            this.numDiagBreak.Value = new decimal(new int[] {
            90,
            0,
            0,
            0});
            this.numDiagBreak.ValueChanged += new System.EventHandler(this.webSettings_Changed);
            // 
            // numEventRefrFreq
            // 
            this.numEventRefrFreq.Location = new System.Drawing.Point(203, 32);
            this.numEventRefrFreq.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.numEventRefrFreq.Name = "numEventRefrFreq";
            this.numEventRefrFreq.Size = new System.Drawing.Size(60, 20);
            this.numEventRefrFreq.TabIndex = 3;
            this.numEventRefrFreq.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numEventRefrFreq.ValueChanged += new System.EventHandler(this.webSettings_Changed);
            // 
            // numEventCnt
            // 
            this.numEventCnt.Location = new System.Drawing.Point(203, 58);
            this.numEventCnt.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numEventCnt.Name = "numEventCnt";
            this.numEventCnt.Size = new System.Drawing.Size(60, 20);
            this.numEventCnt.TabIndex = 5;
            this.numEventCnt.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numEventCnt.ValueChanged += new System.EventHandler(this.webSettings_Changed);
            // 
            // numSrezRefrFreq
            // 
            this.numSrezRefrFreq.Location = new System.Drawing.Point(203, 6);
            this.numSrezRefrFreq.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.numSrezRefrFreq.Name = "numSrezRefrFreq";
            this.numSrezRefrFreq.Size = new System.Drawing.Size(60, 20);
            this.numSrezRefrFreq.TabIndex = 1;
            this.numSrezRefrFreq.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numSrezRefrFreq.ValueChanged += new System.EventHandler(this.webSettings_Changed);
            // 
            // lblSrezRefrFreq
            // 
            this.lblSrezRefrFreq.AutoSize = true;
            this.lblSrezRefrFreq.Location = new System.Drawing.Point(6, 10);
            this.lblSrezRefrFreq.Name = "lblSrezRefrFreq";
            this.lblSrezRefrFreq.Size = new System.Drawing.Size(163, 13);
            this.lblSrezRefrFreq.TabIndex = 0;
            this.lblSrezRefrFreq.Text = "Частота обновления срезов, с";
            // 
            // lblDiagBreak
            // 
            this.lblDiagBreak.AutoSize = true;
            this.lblDiagBreak.Location = new System.Drawing.Point(6, 109);
            this.lblDiagBreak.Name = "lblDiagBreak";
            this.lblDiagBreak.Size = new System.Drawing.Size(179, 26);
            this.lblDiagBreak.TabIndex = 7;
            this.lblDiagBreak.Text = "Разрыв между точками графика, \r\nкоторые соединяются линией, c";
            // 
            // lblEventCnt
            // 
            this.lblEventCnt.AutoSize = true;
            this.lblEventCnt.Location = new System.Drawing.Point(6, 62);
            this.lblEventCnt.Name = "lblEventCnt";
            this.lblEventCnt.Size = new System.Drawing.Size(191, 13);
            this.lblEventCnt.TabIndex = 4;
            this.lblEventCnt.Text = "Количество отображаемых событий";
            // 
            // lblEventRefrFreq
            // 
            this.lblEventRefrFreq.AutoSize = true;
            this.lblEventRefrFreq.Location = new System.Drawing.Point(6, 36);
            this.lblEventRefrFreq.Name = "lblEventRefrFreq";
            this.lblEventRefrFreq.Size = new System.Drawing.Size(170, 13);
            this.lblEventRefrFreq.TabIndex = 2;
            this.lblEventRefrFreq.Text = "Частота обновления событий, с";
            // 
            // chkEventFltr
            // 
            this.chkEventFltr.AutoSize = true;
            this.chkEventFltr.Checked = true;
            this.chkEventFltr.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkEventFltr.Location = new System.Drawing.Point(9, 84);
            this.chkEventFltr.Name = "chkEventFltr";
            this.chkEventFltr.Size = new System.Drawing.Size(344, 17);
            this.chkEventFltr.TabIndex = 6;
            this.chkEventFltr.Text = "Установка фильтра событий по представлению по умолчанию";
            this.chkEventFltr.UseVisualStyleBackColor = true;
            this.chkEventFltr.CheckedChanged += new System.EventHandler(this.webSettings_Changed);
            // 
            // tabView
            // 
            this.tabView.Controls.Add(this.lblFile);
            this.tabView.Controls.Add(this.cbType);
            this.tabView.Controls.Add(this.lblType);
            this.tabView.Controls.Add(this.btnSelectView);
            this.tabView.Controls.Add(this.btnUp);
            this.tabView.Controls.Add(this.btnDown);
            this.tabView.Controls.Add(this.btnDelete);
            this.tabView.Controls.Add(this.btnAddView);
            this.tabView.Controls.Add(this.btnAddViewSet);
            this.tabView.Controls.Add(this.txtDirOrFile);
            this.tabView.Controls.Add(this.lblDir);
            this.tabView.Controls.Add(this.txtName);
            this.tabView.Controls.Add(this.lblName);
            this.tabView.Controls.Add(this.tvTableSets);
            this.tabView.Location = new System.Drawing.Point(4, 22);
            this.tabView.Name = "tabView";
            this.tabView.Padding = new System.Windows.Forms.Padding(3);
            this.tabView.Size = new System.Drawing.Size(462, 304);
            this.tabView.TabIndex = 3;
            this.tabView.Text = "Представления";
            this.tabView.UseVisualStyleBackColor = true;
            // 
            // lblFile
            // 
            this.lblFile.AutoSize = true;
            this.lblFile.Location = new System.Drawing.Point(235, 74);
            this.lblFile.Name = "lblFile";
            this.lblFile.Size = new System.Drawing.Size(36, 13);
            this.lblFile.TabIndex = 9;
            this.lblFile.Text = "Файл";
            this.lblFile.Visible = false;
            // 
            // cbType
            // 
            this.cbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbType.FormattingEnabled = true;
            this.cbType.Items.AddRange(new object[] {
            "Таблица",
            "Схема",
            "Лица",
            "Веб-страница"});
            this.cbType.Location = new System.Drawing.Point(238, 129);
            this.cbType.Name = "cbType";
            this.cbType.Size = new System.Drawing.Size(192, 21);
            this.cbType.TabIndex = 13;
            this.cbType.SelectedIndexChanged += new System.EventHandler(this.cbType_SelectedIndexChanged);
            // 
            // lblType
            // 
            this.lblType.AutoSize = true;
            this.lblType.Location = new System.Drawing.Point(235, 113);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(26, 13);
            this.lblType.TabIndex = 12;
            this.lblType.Text = "Тип";
            // 
            // btnSelectView
            // 
            this.btnSelectView.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSelectView.Image = ((System.Drawing.Image)(resources.GetObject("btnSelectView.Image")));
            this.btnSelectView.Location = new System.Drawing.Point(436, 90);
            this.btnSelectView.Name = "btnSelectView";
            this.btnSelectView.Size = new System.Drawing.Size(20, 20);
            this.btnSelectView.TabIndex = 11;
            this.btnSelectView.UseVisualStyleBackColor = true;
            this.btnSelectView.Click += new System.EventHandler(this.btnSelectView_Click);
            // 
            // btnUp
            // 
            this.btnUp.Location = new System.Drawing.Point(304, 6);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(60, 23);
            this.btnUp.TabIndex = 3;
            this.btnUp.Text = "Вверх";
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // btnDown
            // 
            this.btnDown.Location = new System.Drawing.Point(370, 6);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(60, 23);
            this.btnDown.TabIndex = 4;
            this.btnDown.Text = "Вниз";
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(238, 6);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(60, 23);
            this.btnDelete.TabIndex = 2;
            this.btnDelete.Text = "Удалить";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnAddView
            // 
            this.btnAddView.Location = new System.Drawing.Point(122, 6);
            this.btnAddView.Name = "btnAddView";
            this.btnAddView.Size = new System.Drawing.Size(110, 23);
            this.btnAddView.TabIndex = 1;
            this.btnAddView.Text = "Добавить предст.";
            this.btnAddView.UseVisualStyleBackColor = true;
            this.btnAddView.Click += new System.EventHandler(this.btnAddView_Click);
            // 
            // btnAddViewSet
            // 
            this.btnAddViewSet.Location = new System.Drawing.Point(6, 6);
            this.btnAddViewSet.Name = "btnAddViewSet";
            this.btnAddViewSet.Size = new System.Drawing.Size(110, 23);
            this.btnAddViewSet.TabIndex = 0;
            this.btnAddViewSet.Text = "Добавить набор";
            this.btnAddViewSet.UseVisualStyleBackColor = true;
            this.btnAddViewSet.Click += new System.EventHandler(this.btnAddViewSet_Click);
            // 
            // txtDirOrFile
            // 
            this.txtDirOrFile.Location = new System.Drawing.Point(238, 90);
            this.txtDirOrFile.Name = "txtDirOrFile";
            this.txtDirOrFile.Size = new System.Drawing.Size(192, 20);
            this.txtDirOrFile.TabIndex = 10;
            this.txtDirOrFile.TextChanged += new System.EventHandler(this.txtDirOrFile_TextChanged);
            // 
            // lblDir
            // 
            this.lblDir.AutoSize = true;
            this.lblDir.Location = new System.Drawing.Point(235, 74);
            this.lblDir.Name = "lblDir";
            this.lblDir.Size = new System.Drawing.Size(69, 13);
            this.lblDir.TabIndex = 8;
            this.lblDir.Text = "Директория";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(238, 51);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(192, 20);
            this.txtName.TabIndex = 7;
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(235, 35);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(83, 13);
            this.lblName.TabIndex = 6;
            this.lblName.Text = "Наименование";
            // 
            // tvTableSets
            // 
            this.tvTableSets.HideSelection = false;
            this.tvTableSets.Location = new System.Drawing.Point(6, 35);
            this.tvTableSets.Name = "tvTableSets";
            this.tvTableSets.Size = new System.Drawing.Size(226, 262);
            this.tvTableSets.TabIndex = 5;
            this.tvTableSets.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvTableSets_AfterSelect);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(245, 387);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Сохранить";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(407, 387);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "Закрыть";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // folderBrowserDialog
            // 
            this.folderBrowserDialog.Description = "Выбрать директорию конфигурации";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(326, 387);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "Представления (*.tbl;*.ofm;*.sch;*.fcs)|*.tbl;*.ofm;*.sch;*.fcs|Веб-страницы (*.a" +
    "spx;*.htm;*.html)|*.aspx;*.htm;*.html|Все файлы (*.*)|*.*";
            this.openFileDialog.Title = "Выберите файл представления";
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(494, 422);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.btnConfigDir);
            this.Controls.Add(this.txtConfigDir);
            this.Controls.Add(this.lblConfigDir);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SCADA-Web конфигуратор";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMain_FormClosing);
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.Shown += new System.EventHandler(this.FrmMain_Shown);
            this.tabControl.ResumeLayout(false);
            this.tabComm.ResumeLayout(false);
            this.tabComm.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numServerTimeout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numServerPort)).EndInit();
            this.tabWeb.ResumeLayout(false);
            this.tabWeb.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDiagBreak)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numEventRefrFreq)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numEventCnt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSrezRefrFreq)).EndInit();
            this.tabView.ResumeLayout(false);
            this.tabView.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblConfigDir;
        private System.Windows.Forms.TextBox txtConfigDir;
        private System.Windows.Forms.Button btnConfigDir;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabComm;
        private System.Windows.Forms.TabPage tabWeb;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblServerPwd;
        private System.Windows.Forms.TextBox txtServerPwd;
        private System.Windows.Forms.TextBox txtServerUser;
        private System.Windows.Forms.Label lblServerUser;
        private System.Windows.Forms.Label lblServerTimeout;
        private System.Windows.Forms.NumericUpDown numServerTimeout;
        private System.Windows.Forms.Label lblServerPort;
        private System.Windows.Forms.NumericUpDown numServerPort;
        private System.Windows.Forms.Label lblServerHost;
        private System.Windows.Forms.TextBox txtServerHost;
        private System.Windows.Forms.NumericUpDown numDiagBreak;
        private System.Windows.Forms.NumericUpDown numEventRefrFreq;
        private System.Windows.Forms.NumericUpDown numEventCnt;
        private System.Windows.Forms.NumericUpDown numSrezRefrFreq;
        private System.Windows.Forms.Label lblSrezRefrFreq;
        private System.Windows.Forms.Label lblDiagBreak;
        private System.Windows.Forms.Label lblEventCnt;
        private System.Windows.Forms.Label lblEventRefrFreq;
        private System.Windows.Forms.TabPage tabView;
        private System.Windows.Forms.Button btnAddView;
        private System.Windows.Forms.Button btnAddViewSet;
        private System.Windows.Forms.TextBox txtDirOrFile;
        private System.Windows.Forms.Label lblDir;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TreeView tvTableSets;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.CheckBox chkCmdEnabled;
        private System.Windows.Forms.CheckBox chkEventFltr;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Button btnSelectView;
        private System.Windows.Forms.ComboBox cbType;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.CheckBox chkRemEnabled;
        private System.Windows.Forms.CheckBox chkSimpleCmd;
        private System.Windows.Forms.Label lblFile;
    }
}

