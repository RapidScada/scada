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
 * Module   : Scheme Editor
 * Summary  : Application settings form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2017
 */

using Scada.UI;
using System;
using System.IO;
using System.Windows.Forms;

namespace Scada.Scheme.Editor
{
    /// <summary>
    /// Application settings form
    /// <para>Форма настройки приложения</para>
    /// </summary>
    internal partial class FrmSettings : Form
    {
        private Settings settings; // настройки приложения


        /// <summary>
        /// Конструктор
        /// </summary>
        public FrmSettings()
        {
            InitializeComponent();
            settings = null;
        }


        /// <summary>
        /// Отобразить форму модально
        /// </summary>
        /// <returns>Возвращает true, если настройки были изменена</returns>
        public static bool ShowDialog(Settings settings)
        {
            if (settings == null)
                throw new ArgumentNullException("settings");

            string oldWebDir = settings.WebDir;
            FrmSettings frmSettings = new FrmSettings();
            frmSettings.settings = settings;

            return frmSettings.ShowDialog() == DialogResult.OK && oldWebDir != settings.WebDir;
        }


        private void FrmSettings_Load(object sender, EventArgs e)
        {
            // перевод формы
            Translator.TranslateForm(this, "Scada.Scheme.Editor.FrmSettings");

            // настройка элементов управления
            txtWebDir.Text = settings.WebDir;
        }

        private void FrmSettings_Shown(object sender, EventArgs e)
        {
            txtWebDir.Focus();
            txtWebDir.DeselectAll();
        }

        private void btnWebDir_Click(object sender, EventArgs e)
        {
            // выбор директории веб-приложения
            folderBrowserDialog.SelectedPath = txtWebDir.Text.Trim();
            folderBrowserDialog.Description = AppPhrases.ChooseWebDir;

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                txtWebDir.Text = ScadaUtils.NormalDir(folderBrowserDialog.SelectedPath);

            txtWebDir.Focus();
            txtWebDir.DeselectAll();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(txtWebDir.Text))
                ScadaUiUtils.ShowWarning(AppPhrases.WebDirNotExists);

            settings.WebDir = txtWebDir.Text;
            DialogResult = DialogResult.OK;
        }
    }
}
