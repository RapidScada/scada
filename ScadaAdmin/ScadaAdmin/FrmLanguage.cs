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
 * Module   : SCADA-Administrator
 * Summary  : Choosing language form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2014
 * Modified : 2016
 */

using Scada;
using Scada.UI;
using System;
using System.Globalization;
using System.Windows.Forms;

namespace ScadaAdmin
{
    /// <summary>
    /// Choosing language form
    /// <para>Форма выбора языка</para>
    /// </summary>
    public partial class FrmLanguage : Form
    {
        /// <summary>
        /// Статический конструктор
        /// </summary>
        static FrmLanguage()
        {
            CultureName = Localization.Culture.Name;
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public FrmLanguage()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Получить или установить имя культуры для выбранного языка
        /// </summary>
        public static string CultureName { get; set; }


        private void FrmLanguage_Load(object sender, EventArgs e)
        {
            // перевод формы
            Translator.TranslateForm(this, "ScadaAdmin.FrmLanguage");

            // установка выбранного языка
            if (CultureName == "en-GB")
                cbLanguage.SelectedIndex = 0;
            else if (CultureName == "ru-RU")
                cbLanguage.SelectedIndex = 1;
            else
                cbLanguage.Text = CultureName;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            // проверка корректности выбранного или введённого языка
            if (cbLanguage.SelectedIndex >= 0)
            {
                CultureName = cbLanguage.SelectedIndex == 0 ? "en-GB" : "ru-RU";
                DialogResult = DialogResult.OK;
            }
            else
            {
                try
                {
                    string s = cbLanguage.Text.Trim();
                    if (s == "")
                        throw new Exception();
                    CultureInfo.GetCultureInfo(s);
                    CultureName = s;
                    DialogResult = DialogResult.OK;
                }
                catch
                {
                    ScadaUiUtils.ShowError(AppPhrases.IncorrectLanguage);
                }
            }
        }
    }
}
