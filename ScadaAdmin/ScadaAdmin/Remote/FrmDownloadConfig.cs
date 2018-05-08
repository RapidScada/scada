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
using ScadaAdmin.AgentSvcRef;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
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
        /// Создать вектор инициализации на освнове ид. сессии
        /// </summary>
        private static byte[] CreateIV(long sessionID)
        {
            byte[] iv = new byte[ScadaUtils.IVSize];
            byte[] sessBuf = BitConverter.GetBytes(sessionID);
            int sessBufLen = sessBuf.Length;

            for (int i = 0; i < ScadaUtils.IVSize; i++)
            {
                iv[i] = sessBuf[i % sessBufLen];
            }

            return iv;
        }

        /// <summary>
        /// Зашифровать пароль
        /// </summary>
        private static string EncryptPassword(string password, long sessionID, byte[] secretKey)
        {
            return ScadaUtils.Encrypt(password, secretKey, CreateIV(sessionID));
        }

        /// <summary>
        /// Отобразить настройки скачивания конфигурации
        /// </summary>
        private void ShowDownloadSettings(ServersSettings.DownloadSettings downloadSettings)
        {
            if (downloadSettings == null)
            {
                gbOptions.Enabled = false;
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
            }
        }

        /// <summary>
        /// Применить настройки скачивания конфигурации
        /// </summary>
        private void ApplyDownloadSettings(ServersSettings.DownloadSettings downloadSettings)
        {
            downloadSettings.SaveToDir = rbSaveToDir.Checked;
            downloadSettings.DestDir = txtDestDir.Text;
            downloadSettings.DestFile = txtDestFile.Text;
        }

        /// <summary>
        /// Сохранить настройки взаимодействия с удалёнными серверами
        /// </summary>
        private void SaveServersSettings()
        {
            if (!serversSettings.Save(AppData.AppDirs.ConfigDir + ServersSettings.DefFileName, out string errMsg))
            {
                AppData.ErrLog.WriteError(errMsg);
                ScadaUiUtils.ShowError(errMsg);
            }
        }

        /// <summary>
        /// Скачать конфигурацию
        /// </summary>
        private void DownloadConfig(ServersSettings.ServerSettings serverSettings)
        {
            AgentSvcClient client = new AgentSvcClient();

            try
            {
                client.CreateSession(out long sessionID);
                MessageBox.Show("Session ID = " + sessionID);

                byte[] secretKey = ScadaUtils.HexToBytes("5ABF5A7FD01752A2F1DFD21370B96EA462B0AE5C66A64F8901C9E1E2A06E40F1");
                string encryptedPassword = EncryptPassword("12345", sessionID, secretKey);

                if (client.Login(out string errMsg, sessionID, "admin", encryptedPassword, "Default"))
                    MessageBox.Show("Login OK");
                else
                    MessageBox.Show(errMsg);

                Stream stream = client.DownloadConfig(sessionID,
                    new ConfigOptions() { ConfigParts = ConfigParts.All });

                if (stream == null)
                {
                    MessageBox.Show("Stream is null.");
                }
                else
                {
                    DateTime t0 = DateTime.UtcNow;
                    byte[] buf = new byte[1024];
                    Stream saver = File.Create(@"C:\SCADA\config.zip");
                    int cnt;

                    while ((cnt = stream.Read(buf, 0, buf.Length)) > 0)
                    {
                        saver.Write(buf, 0, cnt);
                    }

                    saver.Close();
                    MessageBox.Show("Done in " + (int)(DateTime.UtcNow - t0).TotalMilliseconds + " ms");
                }
            }
            finally
            {
                client.Close();
            }
        }


        private void FrmDownloadConfig_Load(object sender, EventArgs e)
        {
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

        private void txtDestDir_TextChanged(object sender, EventArgs e)
        {
            downloadSettingsModified = true;
        }

        private void txtDestFile_TextChanged(object sender, EventArgs e)
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

            if (serverSettings != null)
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
