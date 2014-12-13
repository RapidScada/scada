/*
 * Copyright 2014 Mikhail Shiryaev
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 * 
 * Product  : Rapid SCADA
 * Module   : Report Execution
 * Summary  : Main form of the application
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2005
 * Modified : 2014
 * 
 * Description :
 * The application is designed to execute and test reports.
 */

using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Utils.Report.Exec
{
    /// <summary>
    /// Main form of the application
    /// <para>Главная форма приложения</para>
    /// </summary>
    public class FrmMain : System.Windows.Forms.Form
    {
        #region Windows Form Designer generated code
        private System.Windows.Forms.Button btnExecute;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnMake;
        private System.Windows.Forms.ColumnHeader colHdrFile;
        private System.Windows.Forms.ColumnHeader colHdrName;
        private System.Windows.Forms.ColumnHeader colHdrDescr;
        private System.Windows.Forms.ListView lvRep;
        private System.Windows.Forms.Label lblTemplDir;
        private System.Windows.Forms.Label lblOutDir;
        private System.Windows.Forms.TextBox txtTemplDir;
        private System.Windows.Forms.TextBox txtOutDir;
        private System.Windows.Forms.Button btnBrowseOutDir;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.Label lblDllDir;
        private System.Windows.Forms.TextBox txtDllDir;
        private System.Windows.Forms.Button btnBrowseDllDir;
        private System.Windows.Forms.Button btnBrowseTemplDir;
        private System.Windows.Forms.Button btnReload;
        private System.Windows.Forms.CheckBox chkShowErr;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnExecute = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnMake = new System.Windows.Forms.Button();
            this.lvRep = new System.Windows.Forms.ListView();
            this.colHdrFile = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colHdrName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colHdrDescr = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.txtTemplDir = new System.Windows.Forms.TextBox();
            this.txtOutDir = new System.Windows.Forms.TextBox();
            this.lblTemplDir = new System.Windows.Forms.Label();
            this.lblOutDir = new System.Windows.Forms.Label();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.btnBrowseOutDir = new System.Windows.Forms.Button();
            this.btnBrowseDllDir = new System.Windows.Forms.Button();
            this.lblDllDir = new System.Windows.Forms.Label();
            this.txtDllDir = new System.Windows.Forms.TextBox();
            this.btnBrowseTemplDir = new System.Windows.Forms.Button();
            this.btnReload = new System.Windows.Forms.Button();
            this.chkShowErr = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnExecute
            // 
            this.btnExecute.Location = new System.Drawing.Point(8, 424);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(160, 23);
            this.btnExecute.TabIndex = 17;
            this.btnExecute.Text = "Tune up and execute report";
            this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(432, 424);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 20;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnMake
            // 
            this.btnMake.Location = new System.Drawing.Point(176, 424);
            this.btnMake.Name = "btnMake";
            this.btnMake.Size = new System.Drawing.Size(160, 23);
            this.btnMake.TabIndex = 18;
            this.btnMake.Text = "Make report";
            this.btnMake.Click += new System.EventHandler(this.btnMake_Click);
            // 
            // lvRep
            // 
            this.lvRep.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colHdrFile,
            this.colHdrName,
            this.colHdrDescr});
            this.lvRep.FullRowSelect = true;
            this.lvRep.HideSelection = false;
            this.lvRep.Location = new System.Drawing.Point(8, 12);
            this.lvRep.MultiSelect = false;
            this.lvRep.Name = "lvRep";
            this.lvRep.Size = new System.Drawing.Size(504, 296);
            this.lvRep.TabIndex = 1;
            this.lvRep.UseCompatibleStateImageBehavior = false;
            this.lvRep.View = System.Windows.Forms.View.Details;
            // 
            // colHdrFile
            // 
            this.colHdrFile.Text = "Filename";
            this.colHdrFile.Width = 100;
            // 
            // colHdrName
            // 
            this.colHdrName.Text = "Report name";
            this.colHdrName.Width = 200;
            // 
            // colHdrDescr
            // 
            this.colHdrDescr.Text = "Report description";
            this.colHdrDescr.Width = 200;
            // 
            // txtTemplDir
            // 
            this.txtTemplDir.Location = new System.Drawing.Point(122, 344);
            this.txtTemplDir.Name = "txtTemplDir";
            this.txtTemplDir.Size = new System.Drawing.Size(356, 20);
            this.txtTemplDir.TabIndex = 11;
            this.txtTemplDir.Text = "txtTemplDir";
            // 
            // txtOutDir
            // 
            this.txtOutDir.Location = new System.Drawing.Point(122, 368);
            this.txtOutDir.Name = "txtOutDir";
            this.txtOutDir.Size = new System.Drawing.Size(356, 20);
            this.txtOutDir.TabIndex = 14;
            this.txtOutDir.Text = "txtOutDir";
            // 
            // lblTemplDir
            // 
            this.lblTemplDir.Location = new System.Drawing.Point(16, 346);
            this.lblTemplDir.Name = "lblTemplDir";
            this.lblTemplDir.Size = new System.Drawing.Size(100, 16);
            this.lblTemplDir.TabIndex = 10;
            this.lblTemplDir.Text = "Template directory";
            this.lblTemplDir.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblOutDir
            // 
            this.lblOutDir.Location = new System.Drawing.Point(16, 370);
            this.lblOutDir.Name = "lblOutDir";
            this.lblOutDir.Size = new System.Drawing.Size(100, 16);
            this.lblOutDir.TabIndex = 13;
            this.lblOutDir.Text = "Output directory";
            this.lblOutDir.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnBrowseOutDir
            // 
            this.btnBrowseOutDir.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnBrowseOutDir.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnBrowseOutDir.Location = new System.Drawing.Point(484, 368);
            this.btnBrowseOutDir.Name = "btnBrowseOutDir";
            this.btnBrowseOutDir.Size = new System.Drawing.Size(24, 20);
            this.btnBrowseOutDir.TabIndex = 15;
            this.btnBrowseOutDir.Text = "...";
            this.btnBrowseOutDir.Click += new System.EventHandler(this.btnBrowseOutDir_Click);
            // 
            // btnBrowseDllDir
            // 
            this.btnBrowseDllDir.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnBrowseDllDir.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnBrowseDllDir.Location = new System.Drawing.Point(484, 320);
            this.btnBrowseDllDir.Name = "btnBrowseDllDir";
            this.btnBrowseDllDir.Size = new System.Drawing.Size(23, 20);
            this.btnBrowseDllDir.TabIndex = 9;
            this.btnBrowseDllDir.Text = "...";
            this.btnBrowseDllDir.Click += new System.EventHandler(this.btnBrowseDllDir_Click);
            // 
            // lblDllDir
            // 
            this.lblDllDir.Location = new System.Drawing.Point(16, 322);
            this.lblDllDir.Name = "lblDllDir";
            this.lblDllDir.Size = new System.Drawing.Size(100, 16);
            this.lblDllDir.TabIndex = 7;
            this.lblDllDir.Text = "DLL directory";
            this.lblDllDir.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtDllDir
            // 
            this.txtDllDir.Location = new System.Drawing.Point(122, 320);
            this.txtDllDir.Name = "txtDllDir";
            this.txtDllDir.Size = new System.Drawing.Size(356, 20);
            this.txtDllDir.TabIndex = 8;
            this.txtDllDir.Text = "txtDllDir";
            // 
            // btnBrowseTemplDir
            // 
            this.btnBrowseTemplDir.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnBrowseTemplDir.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnBrowseTemplDir.Location = new System.Drawing.Point(484, 344);
            this.btnBrowseTemplDir.Name = "btnBrowseTemplDir";
            this.btnBrowseTemplDir.Size = new System.Drawing.Size(24, 20);
            this.btnBrowseTemplDir.TabIndex = 12;
            this.btnBrowseTemplDir.Text = "...";
            this.btnBrowseTemplDir.Click += new System.EventHandler(this.btnBrowseTemplDir_Click);
            // 
            // btnReload
            // 
            this.btnReload.Location = new System.Drawing.Point(352, 424);
            this.btnReload.Name = "btnReload";
            this.btnReload.Size = new System.Drawing.Size(75, 23);
            this.btnReload.TabIndex = 19;
            this.btnReload.Text = "Reload";
            this.btnReload.Click += new System.EventHandler(this.btnReload_Click);
            // 
            // chkShowErr
            // 
            this.chkShowErr.AutoSize = true;
            this.chkShowErr.Location = new System.Drawing.Point(122, 394);
            this.chkShowErr.Name = "chkShowErr";
            this.chkShowErr.Size = new System.Drawing.Size(179, 17);
            this.chkShowErr.TabIndex = 16;
            this.chkShowErr.Text = "Show errors during loading DLLs";
            // 
            // FrmMain
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(522, 464);
            this.Controls.Add(this.txtDllDir);
            this.Controls.Add(this.txtOutDir);
            this.Controls.Add(this.txtTemplDir);
            this.Controls.Add(this.chkShowErr);
            this.Controls.Add(this.btnReload);
            this.Controls.Add(this.btnBrowseTemplDir);
            this.Controls.Add(this.btnBrowseDllDir);
            this.Controls.Add(this.lblDllDir);
            this.Controls.Add(this.btnBrowseOutDir);
            this.Controls.Add(this.lblOutDir);
            this.Controls.Add(this.lblTemplDir);
            this.Controls.Add(this.lvRep);
            this.Controls.Add(this.btnMake);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnExecute);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Report Execution";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.FrmMain_Closing);
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion


        /// <summary>
        /// Пространство имён отчётов
        /// </summary>
        private const string ReportNamespace = "Scada.Report.";

        private string repDllDir;   // директория библиотек отчётов
        private string repTemplDir; // директория шаблонов отчётов
        private string repOutDir;   // директория результатов отчётов

        /// <summary>
        /// Конструктор
        /// </summary>
        public FrmMain()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new FrmMain());
        }


        /// <summary>
        /// Добавить '\' к имени директории, если необходимо
        /// </summary>
        private static string NormalDir(string dir)
        {
            if (dir[dir.Length - 1] != '\\') dir += @"\";
            return dir;
        }

        /// <summary>
        /// Считать из реестра значение типа string
        /// </summary>
        private static string GetStringValue(RegistryKey key, string name, string defaultValue)
        {
            return key.GetValue(name, defaultValue).ToString();
        }

        /// <summary>
        /// Перезагрузить библиотеки
        /// </summary>
        private void Reload()
        {
            // определение директорий отчётов
            repDllDir = NormalDir(txtDllDir.Text);
            repTemplDir = NormalDir(txtTemplDir.Text);
            repOutDir = NormalDir(txtOutDir.Text);
            DirectoryInfo repDllDirInfo = new DirectoryInfo(repDllDir);

            // заполнение списка отчётов
            lvRep.Items.Clear();
            if (repDllDirInfo.Exists)
            {
                FileInfo[] fileInfoAr = repDllDirInfo.GetFiles("*.dll");
                foreach (FileInfo fileInfo in fileInfoAr)
                {
                    // загрузка библиотеки (сборки)
                    string asmName = repDllDir + fileInfo.Name; // имя файла библиотеки
                    Assembly asm = null; // библиотека
                    try
                    {
                        asm = Assembly.LoadFile(asmName);
                    }
                    catch (Exception ex)
                    {
                        if (chkShowErr.Checked)
                            MessageBox.Show("Error loading library\n" + asmName + "\n" + ex.Message);
                        continue;
                    }

                    // определение имени типа в загруженной библиотеке
                    string typeName = ReportNamespace +
                        fileInfo.Name.Substring(0, fileInfo.Name.Length - 4);
                    if (typeName == "Utils.Report.RepBuilder" /*базовый абстрактный класс*/)
                        continue;

                    // получение типа из загруженной библиотеки
                    Type repType = null;
                    try
                    {
                        repType = asm.GetType(typeName);
                        if (repType == null && chkShowErr.Checked)
                            MessageBox.Show("Getting the type " + typeName + " failed. Library:\n" + asmName);
                    }
                    catch (Exception ex)
                    {
                        if (chkShowErr.Checked)
                            MessageBox.Show("Getting the type " + typeName + " failed. Library:\n" + asmName +
                                "\n" + ex.Message);
                    }

                    if (repType != null)
                    {
                        try
                        {
                            // создание экземпляра класса отчёта
                            RepBuilder rep = Activator.CreateInstance(repType) as RepBuilder;

                            // заполнение элемента списка отчётов
                            ListViewItem item =
                                new ListViewItem(new string[] { fileInfo.Name, rep.RepName, rep.RepDescr });
                            item.Tag = rep;
                            lvRep.Items.Add(item);
                        }
                        catch (Exception ex)
                        {
                            if (chkShowErr.Checked)
                                MessageBox.Show("Error creating instance of the class " + repType +
                                    ". Library:\n" + asmName + "\n" + ex.Message);
                        }
                    }
                }
                if (lvRep.Items.Count > 0)
                    lvRep.Items[0].Selected = true;
            }
            else
            {
                MessageBox.Show("DLL directory does not exist:\n" + repDllDir);
            }
        }


        private void FrmMain_Load(object sender, System.EventArgs e)
        {
            // считывание настроек из реестра
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\SCADA\RepExec", false))
                {
                    DirectoryInfo repDllDirInfo = new DirectoryInfo(@"Report\");
                    repDllDir = GetStringValue(key, "DllDir", repDllDirInfo.FullName);
                    repTemplDir = GetStringValue(key, "TemplDir", repDllDir + "Templates");
                    repOutDir = GetStringValue(key, "OutDir", repDllDir + "Out");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error reading settings from the registry:\n" + ex.Message);
            }
            txtDllDir.Text = repDllDir;
            txtTemplDir.Text = repTemplDir;
            txtOutDir.Text = repOutDir;

            // загрузка библиотек
            Reload();
        }

        private void FrmMain_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // запись настроек в реестр
            try
            {
                using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\SCADA\RepExec"))
                {
                    key.SetValue("DllDir", txtDllDir.Text);
                    key.SetValue("TemplDir", txtTemplDir.Text);
                    key.SetValue("OutDir", txtOutDir.Text);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error writing settings to the registry:\n" + ex.Message);
            }
        }

        private void btnExecute_Click(object sender, System.EventArgs e)
        {
            // определение директорий отчётов
            repTemplDir = NormalDir(txtTemplDir.Text);
            repOutDir = NormalDir(txtOutDir.Text);

            ListView.SelectedListViewItemCollection items = lvRep.SelectedItems;
            if (items.Count > 0)
            {
                try
                {
                    // запуск отчёта
                    ListViewItem item = items[0];
                    string dllFileName = item.SubItems[0].Text;
                    string repFileName = dllFileName.Substring(0, dllFileName.Length - 4) + "Out.xml";

                    RepBuilder rep = item.Tag as RepBuilder;
                    if (rep.WinForm == null || rep.WinForm.ShowDialog() == DialogResult.OK)
                    {
                        DateTime startTm = DateTime.Now;
                        rep.Make(repOutDir + repFileName, repTemplDir);

                        MessageBox.Show("Report has been generated.\n" +
                            "Execution time: " + DateTime.Now.Subtract(startTm).ToString() +
                            "\nOutput file: " + repOutDir + repFileName);
                    }
                    else
                    {
                        MessageBox.Show("Report generation has been canceled.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error executing the report:\n" + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Select report.");
            }
        }

        private void btnMake_Click(object sender, System.EventArgs e)
        {
            // определение директорий отчётов
            repTemplDir = NormalDir(txtTemplDir.Text);
            repOutDir = NormalDir(txtOutDir.Text);

            ListView.SelectedListViewItemCollection items = lvRep.SelectedItems;
            if (items.Count > 0)
            {
                try
                {
                    // генерация отчёта
                    ListViewItem item = items[0];
                    string dllFileName = item.SubItems[0].Text;
                    string repFileName = dllFileName.Substring(0, dllFileName.Length - 4) + "Out.xml";

                    RepBuilder rep = item.Tag as RepBuilder;
                    DateTime startTm = DateTime.Now;
                    rep.Make(repOutDir + repFileName, repTemplDir);

                    MessageBox.Show("Report has been generated.\n" +
                        "Execution time: " + DateTime.Now.Subtract(startTm).ToString() +
                        "\nOutput file: " + repOutDir + repFileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error making the report:\n" + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Select report.");
            }
        }

        private void btnReload_Click(object sender, System.EventArgs e)
        {
            Reload();
        }

        private void btnClose_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        private void btnBrowseDllDir_Click(object sender, System.EventArgs e)
        {
            txtDllDir.Focus();
            folderBrowserDialog.SelectedPath = txtDllDir.Text;
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                txtDllDir.Text = folderBrowserDialog.SelectedPath;
        }

        private void btnBrowseTemplDir_Click(object sender, System.EventArgs e)
        {
            txtTemplDir.Focus();
            folderBrowserDialog.SelectedPath = txtTemplDir.Text;
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                txtTemplDir.Text = folderBrowserDialog.SelectedPath;
        }

        private void btnBrowseOutDir_Click(object sender, System.EventArgs e)
        {
            txtOutDir.Focus();
            folderBrowserDialog.SelectedPath = txtOutDir.Text;
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                txtOutDir.Text = folderBrowserDialog.SelectedPath;
        }
    }
}
