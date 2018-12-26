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
 * Summary  : Form for checking status of a remote server
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using Scada;
using Scada.UI;
using ScadaAdmin.AgentSvcRef;
using System;
using System.Windows.Forms;

namespace ScadaAdmin.Remote
{
    /// <summary>
    /// Form for checking status of a remote server
    /// <para>Форма для проверки статуса удалённого сервера</para>
    /// </summary>
    public partial class FrmServerStatus : Form
    {
        private ServersSettings serversSettings; // настройки взаимодействия с удалёнными серверами
        private AgentSvcClient client;           // клиент для взаимодействия с сервером
        private long sessionID;                  // ид. сессии взаимодействия с сервером


        /// <summary>
        /// Конструктор
        /// </summary>
        public FrmServerStatus()
        {
            InitializeComponent();
            serversSettings = new ServersSettings();
            client = null;
            sessionID = 0;
        }


        /// <summary>
        /// Разъединиться с удалённым сервером
        /// </summary>
        private void Disconnect()
        {
            timer.Stop();
            client = null;
            btnConnect.Enabled = true;
            btnDisconnect.Enabled = false;
            gbStatus.Enabled = false;
            txtServerStatus.Text = txtCommStatus.Text = txtUpdateTime.Text = "";
        }

        /// <summary>
        /// Получить строковое представление статуса службы
        /// </summary>
        private string StatusToString(ServiceStatus status)
        {
            switch (status)
            {
                case ServiceStatus.Normal:
                    return AppPhrases.NormalSvcStatus;
                case ServiceStatus.Stopped:
                    return AppPhrases.StoppedSvcStatus;
                case ServiceStatus.Error:
                    return AppPhrases.ErrorSvcStatus;
                default: // ServiceStatus.Undefined
                    return AppPhrases.UndefinedSvcStatus;
            }
        }


        private void FrmServerStatus_Load(object sender, EventArgs e)
        {
            // перевод формы
            Translator.TranslateForm(this, "ScadaAdmin.Remote.CtrlServerConn");
            Translator.TranslateForm(this, "ScadaAdmin.Remote.FrmServerStatus");

            // загрузка настроек
            if (!serversSettings.Load(AppData.AppDirs.ConfigDir + ServersSettings.DefFileName, out string errMsg))
                AppUtils.ProcError(errMsg);

            // отображение настроек
            ctrlServerConn.ServersSettings = serversSettings;
        }

        private void FrmServerStatus_FormClosed(object sender, FormClosedEventArgs e)
        {
            timer.Stop();
        }

        private void ctrlServerConn_SelectedSettingsChanged(object sender, EventArgs e)
        {
            Disconnect();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            // соединение с удалённым сервером
            if (!timer.Enabled)
            {
                btnConnect.Enabled = false;
                btnDisconnect.Enabled = true;
                gbStatus.Enabled = true;
                AppData.Settings.FormSt.ServerConn = ctrlServerConn.SelectedSettings.Connection.Name;
                timer.Start();
            }
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            // разъединение с удалённым сервером
            Disconnect();
        }

        private void btnRestartServer_Click(object sender, EventArgs e)
        {
            // перезапуск службы Сервера на удалённом сервере
            if (client != null)
            {
                if (client.ControlService(sessionID, ServiceApp.Server, ServiceCommand.Restart))
                    ScadaUiUtils.ShowInfo(AppPhrases.ServerRestarted);
                else
                    ScadaUiUtils.ShowError(AppPhrases.UnableRestartServer);
            }
        }

        private void btnRestartComm_Click(object sender, EventArgs e)
        {
            // перезапуск службы Коммуникатора на удалённом сервере
            if (client != null)
            {
                if (client.ControlService(sessionID, ServiceApp.Comm, ServiceCommand.Restart))
                    ScadaUiUtils.ShowInfo(AppPhrases.CommRestarted);
                else
                    ScadaUiUtils.ShowError(AppPhrases.UnableRestartComm);
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();

            // соединение
            if (client == null)
            {
                if (!DownloadUpload.Connect(ctrlServerConn.SelectedSettings.Connection, 
                    out client, out sessionID, out string errMsg))
                {
                    Disconnect();
                    ScadaUiUtils.ShowError(errMsg);
                }
            }

            // запрос данных
            if (client != null)
            {
                ServiceStatus status;

                try
                {
                    txtServerStatus.Text = client.GetServiceStatus(out status, sessionID, ServiceApp.Server) ?
                        StatusToString(status) : "---";
                }
                catch (Exception ex)
                {
                    txtServerStatus.Text = ex.Message;
                }

                try
                {
                    txtCommStatus.Text = client.GetServiceStatus(out status, sessionID, ServiceApp.Comm) ?
                        StatusToString(status) : "---";
                }
                catch (Exception ex)
                {
                    txtCommStatus.Text = ex.Message;
                }

                txtUpdateTime.Text = DateTime.Now.ToLocalizedString();
                timer.Start();
            }
        }
    }
}
