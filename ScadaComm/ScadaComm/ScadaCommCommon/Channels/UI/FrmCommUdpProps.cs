/*
 * Copyright 2015 Mikhail Shiryaev
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
 * Module   : ScadaCommCommon
 * Summary  : UDP communication channel properties
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2015
 */

using Scada.UI;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Scada.Comm.Channels.UI
{
    /// <summary>
    /// UDP communication channel properties
    /// <para>Свойства канала связи UDP</para>
    /// </summary>
    internal partial class FrmCommUdpProps : Form
    {
        private SortedList<string, string> commCnlParams; // параметры канала связи
        private bool modified;                            // признак изменения параметров
        private CommUdpLogic.Settings settings;           // настройки канала связи

        /// <summary>
        /// Конструктор, ограничивающий создание формы без параметров
        /// </summary>
        private FrmCommUdpProps()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Отобразить форму модально
        /// </summary>
        public static void ShowDialog(SortedList<string, string> commCnlParams, out bool modified)
        {
            if (commCnlParams == null)
                throw new ArgumentNullException("commCnlParams");

            FrmCommUdpProps form = new FrmCommUdpProps();
            form.commCnlParams = commCnlParams;
            form.modified = false;
            form.ShowDialog();
            modified = form.modified;
        }


        private void FrmCommUdpProps_Load(object sender, EventArgs e)
        {
            // перевод формы
            Translator.TranslateForm(this, "Scada.Comm.Channels.FrmCommUdpProps", toolTip);

            // инициализация настроек канала связи
            settings = new CommUdpLogic.Settings();
            settings.Init(commCnlParams, false);

            // установка элементов управления в соответствии с параметрами канала связи
            cbBehavior.Text = settings.Behavior.ToString();
            cbDevSelMode.SetSelectedItem(settings.DevSelMode, 
                new Dictionary<string, int>() { { "ByIPAddress", 0 }, { "ByDeviceLibrary", 1 } });
            numLocalUdpPort.SetValue(settings.LocalUdpPort);
            numRemoteUdpPort.SetValue(settings.RemoteUdpPort);
            txtRemoteIpAddress.Text = settings.RemoteIpAddress;

            modified = false;
        }

        private void control_Changed(object sender, EventArgs e)
        {
            modified = true;
        }

        private void cbBehavior_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbDevSelMode.Enabled = cbBehavior.SelectedIndex == 1; // Slave
            modified = true;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            // изменение настроек в соответствии с элементами управления
            if (modified)
            {
                settings.Behavior = cbBehavior.ParseText<CommChannelLogic.OperatingBehaviors>();
                settings.DevSelMode = (CommUdpLogic.DeviceSelectionModes)cbDevSelMode.GetSelectedItem(
                    new Dictionary<int, object>() { 
                        { 0, CommUdpLogic.DeviceSelectionModes.ByIPAddress }, 
                        { 1, CommUdpLogic.DeviceSelectionModes.ByDeviceLibrary } });
                settings.LocalUdpPort = Convert.ToInt32(numLocalUdpPort.Value);
                settings.RemoteUdpPort = Convert.ToInt32(numRemoteUdpPort.Value);
                settings.RemoteIpAddress = txtRemoteIpAddress.Text;

                settings.SetCommCnlParams(commCnlParams);
            }

            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            modified = false;
            DialogResult = DialogResult.Cancel;
        }
    }
}
