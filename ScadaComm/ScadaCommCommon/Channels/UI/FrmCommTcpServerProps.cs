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
 * Summary  : TCP server communication channel properties
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
    /// TCP server communication channel properties
    /// <para>Свойства канала связи TCP-сервер</para>
    /// </summary>
    internal partial class FrmCommTcpServerProps : Form
    {
        private SortedList<string, string> commCnlParams; // параметры канала связи
        private bool modified;                            // признак изменения параметров
        private CommTcpServerLogic.Settings settings;     // настройки канала связи

        /// <summary>
        /// Конструктор, ограничивающий создание формы без параметров
        /// </summary>
        private FrmCommTcpServerProps()
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

            FrmCommTcpServerProps form = new FrmCommTcpServerProps();
            form.commCnlParams = commCnlParams;
            form.modified = false;
            form.ShowDialog();
            modified = form.modified;
        }


        private void FrmCommTcpServerProps_Load(object sender, EventArgs e)
        {
            // перевод формы
            Translator.TranslateForm(this, "Scada.Comm.Channels.FrmCommTcpServerProps", toolTip);

            // инициализация настроек канала связи
            settings = new CommTcpServerLogic.Settings();
            settings.Init(commCnlParams, false);

            // установка элементов управления в соответствии с параметрами канала связи
            cbBehavior.Text = settings.Behavior.ToString();
            cbConnMode.SetSelectedItem(settings.ConnMode, new Dictionary<string, int>() 
                { { "Individual", 0 }, { "Shared", 1 } });
            cbDevSelMode.SetSelectedItem(settings.DevSelMode, new Dictionary<string, int>() 
                { { "ByIPAddress", 0 }, { "ByFirstPackage", 1 }, { "ByDeviceLibrary", 2 } });
            numTcpPort.SetValue(settings.TcpPort);
            numInactiveTime.SetValue(settings.InactiveTime);

            modified = false;
        }

        private void control_Changed(object sender, EventArgs e)
        {
            modified = true;
        }

        private void cbConnMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbDevSelMode.Enabled = cbConnMode.SelectedIndex == 0; // Individual
            modified = true;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            // изменение настроек в соответствии с элементами управления
            if (modified)
            {
                settings.Behavior = cbBehavior.ParseText<CommChannelLogic.OperatingBehaviors>();
                settings.ConnMode = (CommTcpChannelLogic.ConnectionModes)cbConnMode.GetSelectedItem(
                    new Dictionary<int, object>() { 
                        { 0, CommTcpChannelLogic.ConnectionModes.Individual }, 
                        { 1, CommTcpChannelLogic.ConnectionModes.Shared } });
                settings.DevSelMode = (CommTcpServerLogic.DeviceSelectionModes)cbDevSelMode.GetSelectedItem(
                    new Dictionary<int, object>() { 
                        { 0, CommTcpServerLogic.DeviceSelectionModes.ByIPAddress }, 
                        { 1, CommTcpServerLogic.DeviceSelectionModes.ByFirstPackage }, 
                        { 2, CommTcpServerLogic.DeviceSelectionModes.ByDeviceLibrary } });
                settings.TcpPort = Convert.ToInt32(numTcpPort.Value);
                settings.InactiveTime = Convert.ToInt32(numInactiveTime.Value);

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
