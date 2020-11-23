/*
 * Copyright 2016 Mikhail Shiryaev
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
 * Module   : KpSnmp
 * Summary  : Additional settings form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

using Scada.UI;
using System;
using System.Windows.Forms;

namespace Scada.Comm.Devices.KpSnmp
{
    /// <summary>
    /// Additional settings form
    /// <para>Форма дополнительных настроек</para>
    /// </summary>
    internal partial class FrmSettings : Form
    {
        private KpConfig config; // конфигурация связи с КП


        /// <summary>
        /// Конструктор, ограничивающий создание формы без параметров
        /// </summary>
        private FrmSettings()
        {
            InitializeComponent();
            config = null;
        }


        /// <summary>
        /// Отобразить форму
        /// </summary>
        /// <returns>Возвращает true, если конфигурация была изменена</returns>
        public static bool Show(KpConfig config)
        {
            if (config == null)
                throw new ArgumentNullException("config");

            string oldReadCommunity = config.ReadCommunity;
            string oldWriteCommunity = config.WriteCommunity;
            int oldSnmpVersion = config.SnmpVersion;

            FrmSettings frmSettings = new FrmSettings();
            frmSettings.config = config;

            return frmSettings.ShowDialog() == DialogResult.OK &&
                !(oldReadCommunity == config.ReadCommunity && 
                oldWriteCommunity == config.WriteCommunity && 
                oldSnmpVersion == config.SnmpVersion);
        }


        private void FrmPhoneGroup_Load(object sender, EventArgs e)
        {
            // перевод формы
            Translator.TranslateForm(this, "Scada.Comm.Devices.KpSnmp.FrmSettings");

            // настройка элементов управления
            txtReadCommunity.Text = config.ReadCommunity;
            txtWriteCommunity.Text = config.WriteCommunity;
            cbSnmpVersion.SelectedIndex = config.SnmpVersion == 1 ? 0 : 1;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            config.ReadCommunity = txtReadCommunity.Text;
            config.WriteCommunity = txtWriteCommunity.Text;
            config.SnmpVersion = cbSnmpVersion.SelectedIndex == 0 ? 1 : 2;
            DialogResult = DialogResult.OK;
        }
    }
}
