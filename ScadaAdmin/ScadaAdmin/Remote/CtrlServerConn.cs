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

using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace ScadaAdmin.Remote
{
    /// <summary>
    /// Control for selecting a connection to a remote server
    /// <para>Элемент управления для выбора соединения с удалённым сервером</para>
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
                FillServerList();
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
        /// Заполнить список серверов
        /// </summary>
        private void FillServerList()
        {
            try
            {
                cbConnection.BeginUpdate();
                cbConnection.Items.Clear();

                if (serversSettings != null)
                {
                    foreach (ServersSettings.ServerSettings serverSettings in serversSettings.Servers.Values)
                    {
                        cbConnection.Items.Add(serverSettings);
                    }
                }

                if (cbConnection.Items.Count > 0)
                    cbConnection.SelectedIndex = 0;
            }
            finally
            {
                cbConnection.EndUpdate();
            }
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
            OnSelectedSettingsChanged();
        }
    }
}
