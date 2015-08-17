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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Scada.Comm.Channels
{
    /// <summary>
    /// Serial port communication channel properties
    /// <para>Свойства канала связи через последовательный порт</para>
    /// </summary>
    internal partial class FrmCommSerialProps : Form
    {
        private SortedList<string, string> commCnlParams; // параметры канала связи
        private bool modified;                            // признак изменения параметров


        /// <summary>
        /// Конструктор, ограничивающий создание формы без параметров
        /// </summary>
        private FrmCommSerialProps()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Выбрать элемент выпадающего списка, используя карту соответствия значений и индексов элементов списка
        /// </summary>
        private static void SelectComboBoxItem(ComboBox comboBox, object value, Dictionary<string, int> valueToItemIndex)
        {
            string valStr = value.ToString();
            if (valueToItemIndex.ContainsKey(valStr))
                comboBox.SelectedIndex = valueToItemIndex[valStr];
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
            // инициализация настроек канала связи
            CommSerialLogic.Settings settings = new CommSerialLogic.Settings();
            settings.Init(commCnlParams, false);

            // установка элементов управления в соответствии с параметрами канала связи
            cbPortName.Text = settings.PortName;
            cbBaudRate.Text = settings.BaudRate.ToString();
            cbDataBits.Text = settings.DataBits.ToString();
            SelectComboBoxItem(cbParity, settings.Parity, new Dictionary<string, int>() 
                { { "Even", 0 }, { "Odd", 1 }, { "None", 2 }, { "Mark", 3 }, { "Space", 4 } });
            SelectComboBoxItem(cbStopBits, settings.StopBits, new Dictionary<string, int>() 
                { { "One", 0 }, { "OnePointFive", 1 }, { "Two", 2 } });
            chkDtrEnable.Checked = settings.DtrEnable;
            chkRtsEnable.Checked = settings.RtsEnable;
            cbBehavior.Text = settings.Behavior.ToString();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
