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
 * Summary  : Device and communication line properties form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2017
 */

using Scada.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Scada.Comm.Devices.Modbus.UI
{
    /// <summary>
    /// Device and communication line properties form
    /// <para>Форма настройки свойств КП и линии связи</para>
    /// </summary>
    public partial class FrmDevProps : Form
    {
        private int kpNum;                   // номер КП
        private KPView.KPProperties kpProps; // свойства КП, сохраняемые SCADA-Коммуникатором
        private AppDirs appDirs;             // директории приложения


        /// <summary>
        /// Конструктор, ограничивающий создание формы без параметров
        /// </summary>
        private FrmDevProps()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Редактировать шаблон устройства
        /// </summary>
        private void EditDevTemplate(string fileName)
        {
            FrmDevTemplate.ShowDialog(appDirs, true, ref fileName);

            if (!string.IsNullOrEmpty(fileName))
                txtDevTemplate.Text = Path.GetFileName(fileName);
        }

        /// <summary>
        /// Отобразить форму модально
        /// </summary>
        public static void ShowDialog(int kpNum, KPView.KPProperties kpProps, AppDirs appDirs)
        {
            if (kpProps == null)
                throw new ArgumentNullException("kpProps");
            if (appDirs == null)
                throw new ArgumentNullException("appDirs");

            FrmDevProps frmDevProps = new FrmDevProps();
            frmDevProps.kpNum = kpNum;
            frmDevProps.kpProps = kpProps;
            frmDevProps.appDirs = appDirs;
            frmDevProps.ShowDialog();
        }


        private void FrmDevProps_Load(object sender, EventArgs e)
        {
            // перевод формы
            Translator.TranslateForm(this, "Scada.Comm.Devices.KpModbus.FrmDevProps", toolTip);

            // вывод заголовка
            Text = string.Format(Text, kpNum);

            // установка элементов управления в соответствии со свойствами КП
            string transMode = kpProps.CustomParams.GetStringParam("TransMode", false, "RTU");
            cbTransMode.SetSelectedItem(transMode, new Dictionary<string, int>() 
                { { "RTU", 0 }, { "ASCII", 1 }, { "TCP", 2 } }, 0);
            txtDevTemplate.Text = kpProps.CmdLine;
            kpProps.Modified = false;

            // настройка элементов управления
            openFileDialog.InitialDirectory = appDirs.ConfigDir;
        }

        private void control_Changed(object sender, EventArgs e)
        {
            kpProps.Modified = true;
        }

        private void txtDevTemplate_TextChanged(object sender, EventArgs e)
        {
            kpProps.Modified = true;
            btnEditDevTemplate.Enabled = txtDevTemplate.Text.Trim() != "";
        }

        private void btnBrowseDevTemplate_Click(object sender, EventArgs e)
        {
            openFileDialog.FileName = "";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
                txtDevTemplate.Text = Path.GetFileName(openFileDialog.FileName);
            txtDevTemplate.Select();
        }

        private void btnCreateDevTemplate_Click(object sender, EventArgs e)
        {
            EditDevTemplate("");
            txtDevTemplate.Select();
        }

        private void btnEditDevTemplate_Click(object sender, EventArgs e)
        {
            EditDevTemplate(appDirs.ConfigDir + txtDevTemplate.Text);
            txtDevTemplate.Select();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            // проверка существования файла шаблона устройства
            if (!File.Exists(appDirs.ConfigDir + txtDevTemplate.Text))
            {
                ScadaUiUtils.ShowError(KpPhrases.TemplNotExists);
                return;
            }

            // изменение свойств КП в соответствии с элементами управления
            if (kpProps.Modified)
            {
                kpProps.CustomParams["TransMode"] = (string)cbTransMode.GetSelectedItem(
                    new Dictionary<int, object>() { { 0, "RTU" }, { 1, "ASCII" }, { 2, "TCP" } });
                kpProps.CmdLine = txtDevTemplate.Text;
            }

            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            kpProps.Modified = false;
            DialogResult = DialogResult.Cancel;
        }
    }
}
