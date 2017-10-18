/*
 * Copyright 2017 Mikhail Shiryaev
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
 * Module   : KpModbus
 * Summary  : Editing template settings form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2017
 */

using Scada.Comm.Devices.Modbus.Protocol;
using Scada.UI;
using System;
using System.Windows.Forms;

namespace Scada.Comm.Devices.Modbus.UI
{
    /// <summary>
    /// Editing template settings form
    /// <para>Форма редактирования настроек шаблона</para>
    /// </summary>
    public partial class FrmTemplateSettings : Form
    {
        private bool modified; // настройки изменены


        /// <summary>
        /// Конструктор, ограничивающий создание формы без параметров
        /// </summary>
        private FrmTemplateSettings()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Установить значения элементов управления в соответствии с настройками
        /// </summary>
        private void SettingsToControls(DeviceTemplate.Settings settings)
        {
            if (settings.ZeroAddr)
                rbZeroBased.Checked = true;
            else
                rbOneBased.Checked = true;

            if (settings.DecAddr)
                rbDec.Checked = true;
            else
                rbHex.Checked = true;

            txtDefByteOrder2.Text = settings.DefByteOrder2;
            txtDefByteOrder4.Text = settings.DefByteOrder4;
            txtDefByteOrder8.Text = settings.DefByteOrder8;
            modified = false;
        }

        /// <summary>
        /// Установить конфигурацию в соответствии с элементами управления 
        /// </summary>
        private void ControlsToSettings(DeviceTemplate.Settings settings)
        {
            settings.ZeroAddr = rbZeroBased.Checked;
            settings.DecAddr = rbDec.Checked;
            settings.DefByteOrder2 = txtDefByteOrder2.Text;
            settings.DefByteOrder4 = txtDefByteOrder4.Text;
            settings.DefByteOrder8 = txtDefByteOrder8.Text;
        }


        /// <summary>
        /// Отобразить форму модально
        /// </summary>
        /// <returns>Возвращает true, если настройки были изменены</returns>
        public static bool ShowDialog(DeviceTemplate.Settings settings)
        {
            if (settings == null)
                throw new ArgumentNullException("settings");

            FrmTemplateSettings form = new FrmTemplateSettings();
            form.SettingsToControls(settings);

            if (form.ShowDialog() == DialogResult.OK && form.modified)
            {
                form.ControlsToSettings(settings);
                return true;
            }
            else
            {
                return false;
            }
        }


        private void FrmTemplateSettings_Load(object sender, EventArgs e)
        {
            // перевод формы
            Translator.TranslateForm(this, "Scada.Comm.Devices.Modbus.UI.FrmTemplateSettings");
        }

        private void control_Changed(object sender, EventArgs e)
        {
            modified = true;
        }
    }
}
