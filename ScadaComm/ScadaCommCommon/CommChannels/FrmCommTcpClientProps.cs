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
 * Summary  : TCP client communication channel properties
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2015
 */

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Scada.Comm.Channels
{
    /// <summary>
    /// TCP client communication channel properties
    /// <para>Свойства канала связи TCP-клиент</para>
    /// </summary>
    internal partial class FrmCommTcpClientProps : Form
    {
        private SortedList<string, string> commCnlParams; // параметры канала связи
        private bool modified;                            // признак изменения параметров
        private CommTcpClientLogic.Settings settings;     // настройки канала связи

        /// <summary>
        /// Конструктор, ограничивающий создание формы без параметров
        /// </summary>
        private FrmCommTcpClientProps()
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

            FrmCommTcpClientProps form = new FrmCommTcpClientProps();
            form.commCnlParams = commCnlParams;
            form.modified = false;
            form.ShowDialog();
            modified = form.modified;
        }


        private void FrmCommTcpClientProps_Load(object sender, EventArgs e)
        {
            // перевод формы
            Localization.TranslateForm(this, "Scada.Comm.Channels.FrmCommTcpClientProps", toolTip);

            // инициализация настроек канала связи
            settings = new CommTcpClientLogic.Settings();
            settings.Init(commCnlParams, false);

            // установка элементов управления в соответствии с параметрами канала связи
            cbBehavior.Text = settings.Behavior.ToString();
            cbConnMode.SelectItem(settings.ConnMode, new Dictionary<string, int>() { { "Individual", 0 }, { "Shared", 1 } });
            txtIpAddress.Text = settings.IpAddress;
            numTcpPort.SetNumericValue(settings.TcpPort);

            modified = false;
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
                settings.IpAddress = txtIpAddress.Text;
                settings.TcpPort = Convert.ToInt32(numTcpPort.Value);

                settings.SetCommCnlParams(commCnlParams);
            }

            // проверка настроек
            if (settings.ConnMode == CommTcpChannelLogic.ConnectionModes.Shared &&
                string.IsNullOrWhiteSpace(settings.IpAddress))
                ScadaUtils.ShowError(CommPhrases.IpAddressRequired);
            else
                DialogResult = DialogResult.OK;
        }

        private void control_Changed(object sender, EventArgs e)
        {
            modified = true;
        }

        private void cbConnMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtIpAddress.Enabled = cbConnMode.SelectedIndex == 1; // Shared
            modified = true;
        }
    }
}
