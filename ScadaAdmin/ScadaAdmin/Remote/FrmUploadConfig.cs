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
                txtSrcDir.Text = "";
                tvFiles.Nodes.Clear();
                btnUpload.Enabled = false;
            }
            else
            {
                gbOptions.Enabled = true;
                txtSrcDir.Text = uploadSettings.SrcDir;
                FillTreeView(uploadSettings);
                btnUpload.Enabled = true;
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
                TreeNode rootNode = tvFiles.Nodes.Add(srcDir);

                // добавление узла базы конфигурации
                AddDirToTreeView(rootNode, srcDir + "BaseDAT\\");

                // добавление узла интерфейса
                AddDirToTreeView(rootNode, srcDir + "Interface\\");

                // добавление узла Коммуникатора
                string commDir = srcDir + "ScadaComm\\";
                if (Directory.Exists(commDir))
                {
                    TreeNode commNode = rootNode.Nodes.Add("ScadaComm\\");
                    AddDirToTreeView(commNode, commDir + "Config\\");
                }

                // добавление узла Сервера
                string serverDir = srcDir + "ScadaServer\\";
                if (Directory.Exists(serverDir))
                {
                    TreeNode serverNode = rootNode.Nodes.Add("ScadaServer\\");
                    AddDirToTreeView(serverNode, serverDir + "Config\\");
                }

                // добавление узла Вебстанции
                string webDir = srcDir + "ScadaWeb\\";
                if (Directory.Exists(webDir))
                {
                    TreeNode serverNode = rootNode.Nodes.Add("ScadaWeb\\");
                    AddDirToTreeView(serverNode, webDir + "config\\");
                    AddDirToTreeView(serverNode, webDir + "storage\\");
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
        private void AddDirToTreeView(TreeNode parentNode, string dir)
        {
            AddDirToTreeView(parentNode, new DirectoryInfo(dir));
        }

        /// <summary>
        /// Добавить директорию в дерево
        /// </summary>
        private void AddDirToTreeView(TreeNode parentNode, DirectoryInfo dirInfo)
        {
            if (dirInfo.Exists)
            {
                TreeNode dirNode = parentNode.Nodes.Add(dirInfo.Name + "\\");

                // добавление поддиректорий
                DirectoryInfo[] subdirInfoArr = dirInfo.GetDirectories("*", SearchOption.TopDirectoryOnly);

                foreach (DirectoryInfo subdirInfo in subdirInfoArr)
                {
                    AddDirToTreeView(dirNode, subdirInfo);
                }

                // добавление файлов
                FileInfo[] fileInfoArr = dirInfo.GetFiles("*", SearchOption.TopDirectoryOnly);

                foreach (FileInfo fileInfo in fileInfoArr)
                {
                    if (fileInfo.Extension != ".bak")
                        dirNode.Nodes.Add(fileInfo.Name);
                }
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
            uploadSettings.SrcDir = txtSrcDir.Text;
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
        /// Извлечь параметры передачи конфигурации
        /// </summary>
        public void RetrieveUploadOptions(out List<string> fileNames, out ConfigParts configParts)
        {
            fileNames = new List<string>();
            configParts = ConfigParts.All;
        }

        /// <summary>
        /// Передать конфигурацию
        /// </summary>
        private void UploadConfig(ServersSettings.ConnectionSettings connectionSettings,
            List<string> fileNames, ConfigParts configParts)
        {
            // передача
            string logFileName = AppData.AppDirs.LogDir + "ScadaAdminUpload.txt";
            bool uploadOK = DownloadUpload.UploadConfig(connectionSettings,
                fileNames, configParts, logFileName, out bool logCreated, out string msg);

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

        private void btnBrowseSrcDir_Click(object sender, EventArgs e)
        {

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

                RetrieveUploadOptions(out List<string> fileNames, out ConfigParts configParts);
                UploadConfig(serverSettings.Connection, fileNames, configParts);
            }
        }
    }
}
