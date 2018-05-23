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
 * Summary  : Control for selecting a connection to a remote server
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using Scada;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace ScadaAdmin.Remote
{
    /// <summary>
    /// Control for selecting a connection to a remote server
    /// <para>Элемент управления для выбора подключения к удалённому серверу</para>
    /// </summary>
    public partial class CtrlServerConn : UserControl
    {
        private ServersSettings serversSettings; // настройки взаимодействия с удалёнными серверами


        /// <summary>
        /// Конструктор
        /// </summary>
        public CtrlServerConn()
        {
            InitializeComponent();
            serversSettings = null;
        }


        /// <summary>
        /// Получить или установить настройки взаимодействия с удалёнными серверами
        /// </summary>
        public ServersSettings ServersSettings
        {
            get
            {
                return serversSettings;
            }
            set
            {
                serversSettings = value;
                FillConnList();
            }
        }

        /// <summary>
        /// Получить выбранные настройки
        /// </summary>
        public ServersSettings.ServerSettings SelectedSettings
        {
            get
            {
                return cbConnection.SelectedItem as ServersSettings.ServerSettings;
            }
        }


        /// <summary>
        /// Заполнить список подключений
        /// </summary>
        private void FillConnList()
        {
            try
            {
                cbConnection.BeginUpdate();
                cbConnection.Items.Clear();
                int selInd = 0;

                if (serversSettings != null)
                {
                    string defConnName = AppData.Settings.FormSt.ServerConn;

                    foreach (ServersSettings.ServerSettings serverSettings in serversSettings.Servers.Values)
                    {
                        int ind = cbConnection.Items.Add(serverSettings);
                        if (string.Equals(serverSettings.Connection.Name, defConnName))
                            selInd = ind;
                    }
                }

                if (cbConnection.Items.Count > 0)
                    cbConnection.SelectedIndex = selInd;
            }
            finally
            {
                cbConnection.EndUpdate();
            }
        }

        /// <summary>
        /// Добавить настройки сервера в общие настройки и выпадающий список
        /// </summary>
        private void AddToLists(ServersSettings.ServerSettings serverSettings)
        {
            // добавление в общие настройки
            string connName = serverSettings.Connection.Name;
            serversSettings.Servers.Add(connName, serverSettings);

            // добавление в выпадающий список
            int ind = serversSettings.Servers.IndexOfKey(connName);
            if (ind >= 0)
            {
                cbConnection.Items.Insert(ind, serverSettings);
                cbConnection.SelectedIndex = ind;
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
        /// Вызвать событие SelectedSettingsChanged
        /// </summary>
        private void OnSelectedSettingsChanged()
        {
            SelectedSettingsChanged?.Invoke(this, EventArgs.Empty);
        }


        /// <summary>
        /// Событие возникающее при изменении выбранного соединения
        /// </summary>
        [Category("Property Changed")]
        public event EventHandler SelectedSettingsChanged;


        private void cbConnection_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnCreateConn.Enabled = serversSettings != null;
            btnEditConn.Enabled = btnRemoveConn.Enabled = SelectedSettings != null;
            OnSelectedSettingsChanged();
        }

        private void btnCreateConn_Click(object sender, EventArgs e)
        {
            // создание новых настроек
            ServersSettings.ServerSettings serverSettings = new ServersSettings.ServerSettings();
            FrmConnSettings frmConnSettings = new FrmConnSettings()
            {
                ConnectionSettings = serverSettings.Connection,
                ExistingNames = ServersSettings.GetExistingNames()
            };

            if (frmConnSettings.ShowDialog() == DialogResult.OK)
            {
                AddToLists(serverSettings);
                SaveServersSettings();
            }
        }

        private void btnEditConn_Click(object sender, EventArgs e)
        {
            // редактирование настроек
            ServersSettings.ServerSettings serverSettings = SelectedSettings;
            string oldName = serverSettings.Connection.Name;

            FrmConnSettings frmConnSettings = new FrmConnSettings()
            {
                ConnectionSettings = serverSettings.Connection,
                ExistingNames = ServersSettings.GetExistingNames(oldName)
            };

            if (frmConnSettings.ShowDialog() == DialogResult.OK)
            {
                // обновление наименования, если оно изменилось
                if (!string.Equals(oldName, serverSettings.Connection.Name, StringComparison.Ordinal))
                {
                    serversSettings.Servers.Remove(oldName);
                    cbConnection.BeginUpdate();
                    cbConnection.Items.RemoveAt(cbConnection.SelectedIndex);
                    AddToLists(serverSettings);
                    cbConnection.EndUpdate();
                }

                // сохранение настроек
                SaveServersSettings();
            }
        }

        private void btnRemoveConn_Click(object sender, EventArgs e)
        {
            // удаление настроек
            if (MessageBox.Show(AppPhrases.DeleteConnConfirm, CommonPhrases.QuestionCaption, 
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // удаление из общих настроек
                ServersSettings.ServerSettings serverSettings = SelectedSettings;
                serversSettings.Servers.Remove(serverSettings.Connection.Name);

                // удаление из выпадающего списка
                cbConnection.BeginUpdate();
                int selInd = cbConnection.SelectedIndex;
                cbConnection.Items.RemoveAt(selInd);

                if (cbConnection.Items.Count > 0)
                {
                    cbConnection.SelectedIndex = selInd >= cbConnection.Items.Count ? 
                        cbConnection.Items.Count - 1 : selInd;
                }

                cbConnection.EndUpdate();

                // сохранение настроек
                SaveServersSettings();
            }
        }
    }
}
