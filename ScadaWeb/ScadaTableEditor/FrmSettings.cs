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
 * Module   : SCADA-Table Editor
 * Summary  : Application settings form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2013
 * Modified : 2016
 */

using Scada;
using Scada.UI;
using System;
using System.IO;
using System.Windows.Forms;

namespace ScadaTableEditor
{
    /// <summary>
    /// Application settings form
    /// <para>Форма настроек приложения</para>
    /// </summary>
    public partial class FrmSettings : Form
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        private FrmSettings()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Отобразить форму настроек приложения
        /// </summary>
        public static DialogResult Show(ref string baseDATDir)
        {
            // создание и перевод формы
            FrmSettings frmSettings = new FrmSettings();
            Translator.TranslateForm(frmSettings, "ScadaTableEditor.FrmSettings");
            frmSettings.folderBrowserDialog.Description = CommonPhrases.ChooseBaseDATDir;

            // настройка и отображение формы
            frmSettings.txtBaseDATDir.Text = baseDATDir;
            DialogResult dlgRes = frmSettings.ShowDialog();
            if (dlgRes == DialogResult.OK)
                baseDATDir = ScadaUtils.NormalDir(frmSettings.txtBaseDATDir.Text);
            return dlgRes;
        }


        private void btnBaseDATDir_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.SelectedPath = txtBaseDATDir.Text.Trim();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                txtBaseDATDir.Text = ScadaUtils.NormalDir(folderBrowserDialog.SelectedPath);
            txtBaseDATDir.Focus();
            txtBaseDATDir.DeselectAll();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            // проверка существования директории
            if (Directory.Exists(txtBaseDATDir.Text))
                DialogResult = DialogResult.OK;
            else
                ScadaUiUtils.ShowError(CommonPhrases.DirNotExists);
        }
    }
}
