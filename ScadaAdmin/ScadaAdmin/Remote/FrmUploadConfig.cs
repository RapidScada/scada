/*
 * Copyright 2018 Mikhail Shiryaev
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
 * Module   : SCADA-Administrator
 * Summary  : Upload configuration form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using Scada;
using Scada.UI;
using ScadaAdmin.AgentSvcRef;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace ScadaAdmin.Remote
{
    /// <summary>
    /// Upload configuration form
    /// <para>Форма передачи конфигурации</para>
    /// </summary>
    public partial class FrmUploadConfig : Form
    {
        /// <summary>
        /// Информация узла дерева
        /// </summary>
        private class NodeInfo
        {
            /// <summary>
            /// Получить или установить путь относительно директории конфигурации
            /// </summary>
            public string Path { get; set; }
            /// <summary>
            /// Получить или установить признак, что узел соответствует директории
            /// </summary>
            public bool IsDirectory { get; set; }
        }

        private ServersSettings serversSettings; // настройки взаимодействия с удалёнными серверами
        private bool uploadSettingsModified;     // последние выбранные настройки передачи были изменены


        /// <summary>
        /// Конструктор
        /// </summary>
        public FrmUploadConfig()
        {
            InitializeComponent();
            serversSettings = new ServersSettings();
            uploadSettingsModified = false;
        }


        /// <summary>
        /// Отобразить настройки передачи конфигурации
        /// </summary>
        private void ShowUploadSettings(ServersSettings.UploadSettings uploadSettings)
        {
            if (uploadSettings == null)
            {
                gbOptions.Enabled = false;
                rbGetFromDir.Checked = true;
                txtSrcDir.Text = txtSrcFile.Text = "";
                tvFiles.Nodes.Clear();
                chkClearSpecificFiles.Checked = false;
                btnUpload.Enabled = false;
            }
            else
            {
                gbOptions.Enabled = true;
                txtSrcDir.Text = uploadSettings.SrcDir;
                FillTreeView(uploadSettings);
                txtSrcFile.Text = uploadSettings.SrcFile;
                chkClearSpecificFiles.Checked = uploadSettings.ClearSpecificFiles;
                btnUpload.Enabled = true;

                if (uploadSettings.GetFromDir)
                    rbGetFromDir.Checked = true;
                else
                    rbGetFromArc.Checked = true;
            }
        }

        /// <summary>
        /// Заполнить дерево выбранных файлов
        /// </summary>
        private void FillTreeView(ServersSettings.UploadSettings uploadSettings)
        {
            try
            {
                tvFiles.BeginUpdate();
                tvFiles.Nodes.Clear();

                string srcDir = ScadaUtils.NormalDir(uploadSettings.SrcDir);
                int srcDirLen = srcDir.Length;
                TreeNode rootNode = tvFiles.Nodes.Add(srcDir);

                // добавление узла базы конфигурации
                HashSet<string> selectedFiles = new HashSet<string>(uploadSettings.SelectedFiles);
                AddDirToTreeView(rootNode, srcDir + "BaseDAT\\", srcDirLen, selectedFiles);

                // добавление узла интерфейса
                AddDirToTreeView(rootNode, srcDir + "Interface\\", srcDirLen, selectedFiles);

                // добавление узла Коммуникатора
                string commDir = srcDir + "ScadaComm\\";
                if (Directory.Exists(commDir))
                {
                    TreeNode commNode = rootNode.Nodes.Add("ScadaComm\\");
                    AddDirToTreeView(commNode, commDir + "Config\\", srcDirLen, selectedFiles);
                }

                // добавление узла Сервера
                string serverDir = srcDir + "ScadaServer\\";
                if (Directory.Exists(serverDir))
                {
                    TreeNode serverNode = rootNode.Nodes.Add("ScadaServer\\");
                    AddDirToTreeView(serverNode, serverDir + "Config\\", srcDirLen, selectedFiles);
                }

                // добавление узла Вебстанции
                string webDir = srcDir + "ScadaWeb\\";
                if (Directory.Exists(webDir))
                {
                    TreeNode serverNode = rootNode.Nodes.Add("ScadaWeb\\");
                    AddDirToTreeView(serverNode, webDir + "config\\", srcDirLen, selectedFiles);
                    AddDirToTreeView(serverNode, webDir + "storage\\", srcDirLen, selectedFiles);
                }

                rootNode.Expand();
            }
            finally
            {
                tvFiles.EndUpdate();
            }
        }
        
        /// <summary>
        /// Добавить директорию в дерево
        /// </summary>
        private void AddDirToTreeView(TreeNode parentNode, string dir, int rootDirLen, HashSet<string> selectedFiles)
        {
            AddDirToTreeView(parentNode, new DirectoryInfo(dir), rootDirLen, selectedFiles);
        }

        /// <summary>
        /// Добавить директорию в дерево
        /// </summary>
        private void AddDirToTreeView(TreeNode parentNode, DirectoryInfo dirInfo, 
            int rootDirLen, HashSet<string> selectedFiles)
        {
            if (dirInfo.Exists)
            {
                TreeNode dirNode = parentNode.Nodes.Add(dirInfo.Name + "\\");
                string dirNodePath = ScadaUtils.NormalDir(dirInfo.FullName.Substring(rootDirLen));
                dirNode.Tag = new NodeInfo() { Path = dirNodePath, IsDirectory = true };

                // добавление поддиректорий
                DirectoryInfo[] subdirInfoArr = dirInfo.GetDirectories("*", SearchOption.TopDirectoryOnly);

                foreach (DirectoryInfo subdirInfo in subdirInfoArr)
                {
                    AddDirToTreeView(dirNode, subdirInfo, rootDirLen, selectedFiles);
                }

                // добавление файлов
                FileInfo[] fileInfoArr = dirInfo.GetFiles("*", SearchOption.TopDirectoryOnly);

                foreach (FileInfo fileInfo in fileInfoArr)
                {
                    if (fileInfo.Extension != ".bak")
                    {
                        TreeNode fileNode = dirNode.Nodes.Add(fileInfo.Name);
                        string fileNodePath = fileInfo.FullName.Substring(rootDirLen);
                        fileNode.Tag = new NodeInfo() { Path = fileNodePath, IsDirectory = false };

                        if (selectedFiles.Contains(fileNodePath))
                            fileNode.Checked = true;
                    }
                }

                // выбор узла и его дочерних узлов после того, как они добавлены в дерево
                if (selectedFiles.Contains(dirNodePath))
                {
                    dirNode.Checked = true;
                    CheckAllChildNodes(dirNode, true);
                }
            }
        }

        /// <summary>
        /// Установить или снять выбор дочерних узлов дерева
        /// </summary>
        private void CheckAllChildNodes(TreeNode treeNode, bool nodeChecked)
        {
            foreach (TreeNode node in treeNode.Nodes)
            {
                node.Checked = nodeChecked;
                CheckAllChildNodes(node, nodeChecked);
            }
        }

        /// <summary>
        /// Проверить настройки передачи конфигурации
        /// </summary>
        private bool ValidateUploadSettings()
        {
            return true;
        }

        /// <summary>
        /// Применить настройки передачи конфигурации
        /// </summary>
        private void ApplyUploadSettings(ServersSettings.UploadSettings uploadSettings)
        {
            uploadSettings.GetFromDir = rbGetFromDir.Checked;
            uploadSettings.SrcDir = txtSrcDir.Text;
            uploadSettings.SrcFile = txtSrcFile.Text;
            uploadSettings.ClearSpecificFiles = chkClearSpecificFiles.Checked;

            uploadSettings.SelectedFiles.Clear();
            TraverseNodes(tvFiles.Nodes[0]);

            // Получить выбранные файлы на основе выбранных узлов дерева
            void TraverseNodes(TreeNode node)
            {
                if (node.Tag is NodeInfo nodeInfo && node.Checked)
                {
                    uploadSettings.SelectedFiles.Add(nodeInfo.Path);
                }
                else
                {
                    foreach (TreeNode childNode in node.Nodes)
                    {
                        TraverseNodes(childNode);
                    }
                }
            }
        }

        /// <summary>
        /// Сохранить настройки взаимодействия с удалёнными серверами
        /// </summary>
        private void SaveServersSettings()
        {
            if (!serversSettings.Save(AppData.AppDirs.ConfigDir + ServersSettings.DefFileName, out string errMsg))
                AppUtils.ProcError(errMsg);
        }

        /// <summary>
        /// Конвертировать базу конфигурации в формат DAT, если необходимо
        /// </summary>
        private void ConvertBaseToDAT(string srcDir)
        {
            Settings.AppSettings appSettings = AppData.Settings.AppSett;
            string workBaseDATDir = ScadaUtils.NormalDir(appSettings.BaseDATDir);
            string srcBaseDATDir = Path.Combine(srcDir, "BaseDAT\\");

            if (string.Equals(workBaseDATDir, srcBaseDATDir, StringComparison.OrdinalIgnoreCase))
            {
                // резервное копирование
                if (appSettings.AutoBackupBase &&
                    !ImportExport.BackupSDF(appSettings.BaseSDFFile, appSettings.BackupDir, out string msg))
                    AppUtils.ProcError(msg);

                // конвертирование
                if (!ImportExport.PassBase(Tables.TableInfoList, workBaseDATDir, out msg))
                    AppUtils.ProcError(msg);
            }
        }

        /// <summary>
        /// Передать конфигурацию
        /// </summary>
        private void UploadConfig(ServersSettings.ServerSettings serverSettings)
        {
            // передача
            Cursor = Cursors.WaitCursor;
            string logFileName = AppData.AppDirs.LogDir + "ScadaAdminUpload.txt";
            bool uploadOK = DownloadUpload.UploadConfig(serverSettings,
                logFileName, out bool logCreated, out string msg);
            Cursor = Cursors.Default;

            // отображение сообщения о результате
            if (uploadOK)
            {
                ScadaUiUtils.ShowInfo(msg);
            }
            else
            {
                AppUtils.ProcError(msg);

                // отображение журнала в блокноте
                if (logCreated)
                    Process.Start(logFileName);
            }
        }


        private void FrmUploadConfig_Load(object sender, EventArgs e)
        {
            // перевод формы
            Translator.TranslateForm(this, "ScadaAdmin.Remote.CtrlServerConn");
            Translator.TranslateForm(this, "ScadaAdmin.Remote.FrmUploadConfig");
            folderBrowserDialog.Description = AppPhrases.ChooseConfigDir;

            // загрузка настроек
            if (!serversSettings.Load(AppData.AppDirs.ConfigDir + ServersSettings.DefFileName, out string errMsg))
            {
                AppData.ErrLog.WriteError(errMsg);
                ScadaUiUtils.ShowError(errMsg);
            }

            // отображение настроек
            ctrlServerConn.ServersSettings = serversSettings;
        }

        private void ctrlServerConn_SelectedSettingsChanged(object sender, EventArgs e)
        {
            ShowUploadSettings(ctrlServerConn.SelectedSettings?.Upload);
            uploadSettingsModified = false;
        }

        private void rbGet_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked) // чтобы исключить двойное срабатывание
            {
                bool getFromDir = rbGetFromDir.Checked;
                txtSrcDir.Enabled = getFromDir;
                btnBrowseSrcDir.Enabled = getFromDir;
                tvFiles.Enabled = getFromDir;
                txtSrcFile.Enabled = !getFromDir;
                btnSelectSrcFile.Enabled = !getFromDir;
                uploadSettingsModified = true;
            }
        }

        private void uploadControl_Changed(object sender, EventArgs e)
        {
            uploadSettingsModified = true;
        }

        private void tvFiles_AfterCheck(object sender, TreeViewEventArgs e)
        {
            // установка выбора для дочерних узлов
            if (e.Action != TreeViewAction.Unknown)
                CheckAllChildNodes(e.Node, e.Node.Checked);

            uploadSettingsModified = true;
        }

        private void btnBrowseSrcDir_Click(object sender, EventArgs e)
        {
            // выбор директории конфигурации
            folderBrowserDialog.SelectedPath = txtSrcDir.Text.Trim();

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                txtSrcDir.Text = ScadaUtils.NormalDir(folderBrowserDialog.SelectedPath);

            txtSrcDir.Focus();
            txtSrcDir.DeselectAll();
        }

        private void btnSelectSrcFile_Click(object sender, EventArgs e)
        {
            // выбор файла архива конфигурации
            string fileName = txtSrcFile.Text.Trim();
            openFileDialog.FileName = fileName;

            if (fileName != "")
                openFileDialog.InitialDirectory = Path.GetDirectoryName(fileName);

            if (openFileDialog.ShowDialog() == DialogResult.OK)
                txtSrcFile.Text = openFileDialog.FileName;

            txtSrcFile.Focus();
            txtSrcFile.DeselectAll();
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            // проверка настроек и передача конфигурации
            ServersSettings.ServerSettings serverSettings = ctrlServerConn.SelectedSettings;

            if (serverSettings != null && ValidateUploadSettings())
            {
                if (uploadSettingsModified)
                {
                    ApplyUploadSettings(serverSettings.Upload);
                    SaveServersSettings();
                }

                if (serverSettings.Upload.GetFromDir)
                    ConvertBaseToDAT(serverSettings.Upload.SrcDir);

                UploadConfig(serverSettings);
            }
        }
    }
}
