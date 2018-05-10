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
 * Summary  : Download configuration form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using Scada;
using Scada.UI;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace ScadaAdmin.Remote
{
    /// <summary>
    /// Download configuration form
    /// <para>Форма скачивания конфигурации</para>
    /// </summary>
    public partial class FrmDownloadConfig : Form
    {
        private ServersSettings serversSettings; // настройки взаимодействия с удалёнными серверами
        private bool downloadSettingsModified;   // последние выбранные настройки скачивания были изменены


        /// <summary>
        /// Конструктор
        /// </summary>
        public FrmDownloadConfig()
        {
            InitializeComponent();
            serversSettings = new ServersSettings();
            downloadSettingsModified = false;
        }


        /// <summary>
        /// Отобразить настройки скачивания конфигурации
        /// </summary>
        private void ShowDownloadSettings(ServersSettings.DownloadSettings downloadSettings)
        {
            if (downloadSettings == null)
            {
                gbOptions.Enabled = false;
                rbSaveToDir.Checked = true;
                txtDestDir.Text = txtDestFile.Text = "";
                chkImportBase.Checked = false;
                btnDownload.Enabled = false;
            }
            else
            {
                gbOptions.Enabled = true;
                btnDownload.Enabled = true;

                if (downloadSettings.SaveToDir)
                {
                    rbSaveToDir.Checked = true;
                    txtDestDir.Enabled = true;
                    btnBrowseDestDir.Enabled = true;
                    txtDestFile.Enabled = false;
                    btnSelectDestFile.Enabled = false;
                }
                else
                {
                    rbSaveToArc.Checked = true;
                    txtDestDir.Enabled = false;
                    btnBrowseDestDir.Enabled = false;
                    txtDestFile.Enabled = true;
                    btnSelectDestFile.Enabled = true;
                }

                txtDestDir.Text = downloadSettings.DestDir;
                txtDestFile.Text = downloadSettings.DestFile;
                chkImportBase.Checked = downloadSettings.ImportBase;
            }
        }

        /// <summary>
        /// Проверить настройки скачивания конфигурации
        /// </summary>
        private bool ValidateDownloadSettings()
        {
            if (rbSaveToDir.Checked)
            {
                if (string.IsNullOrWhiteSpace(txtDestDir.Text))
                {
                    ScadaUiUtils.ShowError(AppPhrases.DestDirRequired);
                    return false;
                }
            }
            else if (string.IsNullOrWhiteSpace(txtDestFile.Text))
            {
                ScadaUiUtils.ShowError(AppPhrases.DestFileRequired);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Применить настройки скачивания конфигурации
        /// </summary>
        private void ApplyDownloadSettings(ServersSettings.DownloadSettings downloadSettings)
        {
            downloadSettings.SaveToDir = rbSaveToDir.Checked;
            downloadSettings.DestDir = txtDestDir.Text;
            downloadSettings.DestFile = txtDestFile.Text;
            downloadSettings.ImportBase = chkImportBase.Checked;
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
        /// Скачать конфигурацию
        /// </summary>
        private void DownloadConfig(ServersSettings.ServerSettings serverSettings)
        {
            // скачивание
            string logFileName = AppData.AppDirs.LogDir + "ScadaAdminDownload.txt";
            bool downloadOK = DownloadUpload.DownloadConfig(serverSettings,
                logFileName, out bool logCreated, out string msg);

            // отображение сообщения о результате
            if (downloadOK)
            {
                ScadaUiUtils.ShowInfo(msg);

                // запуск импорта
                ServersSettings.DownloadSettings downloadSettings = serverSettings.Download;
                if (downloadSettings.ImportBase)
                {
                    FrmImport frmImport = new FrmImport();
                    if (downloadSettings.SaveToDir)
                    {
                        frmImport.DefaultSelection = FrmImport.SelectedItem.AllTables;
                        frmImport.DefaultBaseDATDir = Path.Combine(downloadSettings.DestDir, "BaseDAT");
                    }
                    else
                    {
                        frmImport.DefaultSelection = FrmImport.SelectedItem.Archive;
                        frmImport.DefaultArcFileName = downloadSettings.DestFile;
                        frmImport.DefaultBaseDATDir = AppData.Settings.AppSett.BaseDATDir;
                    }
                    frmImport.ShowDialog();
                }
            }
            else
            {
                AppUtils.ProcError(msg);

                // отображение журнала в блокноте
                if (logCreated)
                    Process.Start(logFileName);
            }
        }


        private void FrmDownloadConfig_Load(object sender, EventArgs e)
        {
            // перевод формы
            Translator.TranslateForm(this, "ScadaAdmin.Remote.CtrlServerConn");
            Translator.TranslateForm(this, "ScadaAdmin.Remote.FrmDownloadConfig");
            openFileDialog.Title = AppPhrases.ChooseArchiveFile;
            openFileDialog.Filter = AppPhrases.ArchiveFileFilter;
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
            ShowDownloadSettings(ctrlServerConn.SelectedSettings?.Download);
            downloadSettingsModified = false;
        }

        private void rbSaveToDir_CheckedChanged(object sender, EventArgs e)
        {
            txtDestDir.Enabled = rbSaveToDir.Checked;
            btnBrowseDestDir.Enabled = rbSaveToDir.Checked;
            downloadSettingsModified = true;
        }

        private void rbSaveToArc_CheckedChanged(object sender, EventArgs e)
        {
            txtDestFile.Enabled = rbSaveToArc.Checked;
            btnSelectDestFile.Enabled = rbSaveToArc.Checked;
            downloadSettingsModified = true;
        }

        private void downloadControl_Changed(object sender, EventArgs e)
        {
            downloadSettingsModified = true;
        }

        private void btnBrowseDestDir_Click(object sender, EventArgs e)
        {
            // выбор директории для сохранения конфигурации
            folderBrowserDialog.SelectedPath = txtDestDir.Text.Trim();

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                txtDestDir.Text = ScadaUtils.NormalDir(folderBrowserDialog.SelectedPath);

            txtDestDir.Focus();
            txtDestDir.DeselectAll();
        }

        private void btnSelectDestFile_Click(object sender, EventArgs e)
        {
            // выбор файла архива для сохранения конфигурации
            string fileName = txtDestFile.Text.Trim();
            openFileDialog.FileName = fileName;

            if (fileName != "")
                openFileDialog.InitialDirectory = Path.GetDirectoryName(fileName);

            if (openFileDialog.ShowDialog() == DialogResult.OK)
                txtDestFile.Text = openFileDialog.FileName;

            txtDestFile.Focus();
            txtDestFile.DeselectAll();
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            // проверка настроек и скачивание конфигурации
            ServersSettings.ServerSettings serverSettings = ctrlServerConn.SelectedSettings;

            if (serverSettings != null && ValidateDownloadSettings())
            {
                if (downloadSettingsModified)
                {
                    ApplyDownloadSettings(serverSettings.Download);
                    SaveServersSettings();
                }

                DownloadConfig(serverSettings);
            }
        }
    }
}
