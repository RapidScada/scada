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
 * Summary  : Editing source code form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2013
 * Modified : 2016
 */

using System;
using System.Windows.Forms;
using Scada;
using Scada.UI;

namespace ScadaAdmin
{
    /// <summary>
    /// Editing source code form
    /// <para>Форма редактирования исходного кода</para>
    /// </summary>
    public partial class FrmEditSource : Form
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public FrmEditSource()
        {
            InitializeComponent();

            Source = "";
            MaxLength = 1000;
        }


        /// <summary>
        /// Получить или установить макс. длину исходного кода
        /// </summary>
        public int MaxLength { get; set; }

        /// <summary>
        /// Получить или установить исходный код
        /// </summary>
        public string Source { get; set; }


        private void FrmEditSource_Load(object sender, EventArgs e)
        {
            // перевод формы
            Translator.TranslateForm(this, "ScadaAdmin.FrmEditSource");
            // вывод исходного кода
            txtSource.MaxLength = MaxLength;
            Source = Source ?? "";
            txtSource.Text = Source.Length <= MaxLength ? Source : Source.Substring(0, MaxLength); 
        }

        private void txtSource_TextChanged(object sender, EventArgs e)
        {
            lblTextLength.Text = txtSource.Text.Length + " / " + txtSource.MaxLength;
            btnOk.Enabled = txtSource.Text != "";
        }

        private void txtSource_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.Enter)
                btnOk_Click(null, null);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Source = txtSource.Text;
            DialogResult = DialogResult.OK;
        }
    }
}
