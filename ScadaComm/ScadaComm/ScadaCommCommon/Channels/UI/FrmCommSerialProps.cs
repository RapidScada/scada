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
 * Summary  : Serial port communication channel properties
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2015
 */

using Scada.UI;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Windows.Forms;

namespace Scada.Comm.Channels.UI
{
    /// <summary>
    /// Serial port communication channel properties
    /// <para>Свойства канала связи через последовательный порт</para>
    /// </summary>
    internal partial class FrmCommSerialProps : Form
    {
        private SortedList<string, string> commCnlParams; // параметры канала связи
        private bool modified;                            // признак изменения параметров
        private CommSerialLogic.Settings settings;        // настройки канала связи


        /// <summary>
        /// Конструктор, ограничивающий создание формы без параметров
        /// </summary>
        private FrmCommSerialProps()
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

            FrmCommSerialProps form = new FrmCommSerialProps();
            form.commCnlParams = commCnlParams;
            form.modified = false;
            form.ShowDialog();
            modified = form.modified;
        }


        private void FrmCommSerialProps_Load(object sender, EventArgs e)
        {
            // перевод формы
            Translator.TranslateForm(this, "Scada.Comm.Channels.FrmCommSerialProps");

            // инициализация настроек канала связи
            settings = new CommSerialLogic.Settings();
            settings.Init(commCnlParams, false);

            // установка элементов управления в соответствии с параметрами канала связи
            cbPortName.Text = settings.PortName;
            cbBaudRate.Text = settings.BaudRate.ToString();
            cbDataBits.Text = settings.DataBits.ToString();
            cbParity.SetSelectedItem(settings.Parity, new Dictionary<string, int>() 
                { { "Even", 0 }, { "Odd", 1 }, { "None", 2 }, { "Mark", 3 }, { "Space", 4 } });
            cbStopBits.SetSelectedItem(settings.StopBits, new Dictionary<string, int>() 
                { { "One", 0 }, { "OnePointFive", 1 }, { "Two", 2 } });
            chkDtrEnable.Checked = settings.DtrEnable;
            chkRtsEnable.Checked = settings.RtsEnable;
            cbBehavior.Text = settings.Behavior.ToString();

            modified = false;
        }

        private void control_Changed(object sender, EventArgs e)
        {
            modified = true;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            // изменение настроек в соответствии с элементами управления
            if (modified)
            {
                settings.PortName = cbPortName.Text;
                settings.BaudRate = int.Parse(cbBaudRate.Text);
                settings.DataBits = int.Parse(cbDataBits.Text);
                settings.Parity = (Parity)cbParity.GetSelectedItem(new Dictionary<int, object>() 
                    { { 0, Parity.Even }, { 1, Parity.Odd }, { 2, Parity.None }, { 3, Parity.Mark }, { 4, Parity.Space } });
                settings.StopBits = (StopBits)cbStopBits.GetSelectedItem(new Dictionary<int, object>() 
                    { { 0, StopBits.One }, { 1, StopBits.OnePointFive }, { 2, StopBits.Two } });
                settings.DtrEnable = chkDtrEnable.Checked;
                settings.RtsEnable = chkRtsEnable.Checked;
                settings.Behavior = cbBehavior.ParseText<CommChannelLogic.OperatingBehaviors>();

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
