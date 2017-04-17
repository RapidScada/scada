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
 * Module   : ScadaSchemeCommon
 * Summary  : Form for editing font
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2012
 * Modified : 2017
 */

using Scada.UI;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Scada.Scheme.Model.PropertyGrid
{
    /// <summary>
    /// Form for editing font
    /// <para>Форма редактирования шрифта</para>
    /// </summary>
    internal partial class FrmFontDialog : Form
    {
        /// <summary>
        /// Информация группе шрифтов
        /// </summary>
        private class FontFamilyInfo
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            public FontFamilyInfo(FontFamily fontFamily)
            {
                FontFamily = fontFamily;
            }
            /// <summary>
            /// Получить или установить группу шрифтов
            /// </summary>
            public FontFamily FontFamily { get; private set; }
            /// <summary>
            /// Получить строковое представление объекта
            /// </summary>
            public override string ToString()
            {
                return FontFamily.Name;
            }
        }


        private Brush textBrush; // кисть для вывода текста


        /// <summary>
        /// Конструктор
        /// </summary>
        public FrmFontDialog()
        {
            InitializeComponent();
            SelectedFont = null;
        }


        /// <summary>
        /// Получить или установить выбранный шрифт
        /// </summary>
        public DataTypes.Font SelectedFont { get; set; }


        private void FrmFontDialog_Load(object sender, EventArgs e)
        {
            // перевод формы
            Translator.TranslateForm(this, "Scada.Scheme.Model.PropertyGrid");
            
            // заполнение списка шрифтов
            cbFontName.BeginUpdate();
            foreach (FontFamily fontFamily in FontFamily.Families)
            {
                if (fontFamily.IsStyleAvailable(FontStyle.Regular))
                    cbFontName.Items.Add(new FontFamilyInfo(fontFamily));
            }
            cbFontName.EndUpdate();

            // создание кисти для вывода текста
            textBrush = new SolidBrush(cbFontName.ForeColor);

            // вывод выбранного шрифта
            if (SelectedFont != null)
            {
                cbFontName.Text = SelectedFont.Name;
                cbFontSize.Text = SelectedFont.Size.ToString();
                chkBold.Checked = SelectedFont.Bold;
                chkItalic.Checked = SelectedFont.Italic;
                chkUnderline.Checked = SelectedFont.Underline;
            }
        }

        private void cbFontName_DrawItem(object sender, DrawItemEventArgs e)
        {
            // отображение заднего фона элемента
            e.DrawBackground();

            // отображение значка и текста элемента
            FontFamilyInfo fontFamilyInfo = (FontFamilyInfo)cbFontName.Items[e.Index];
            Font font = new Font(fontFamilyInfo.FontFamily, 14, FontStyle.Regular, GraphicsUnit.Pixel);
            e.Graphics.DrawString(fontFamilyInfo.ToString(), font, textBrush, e.Bounds.X + 2, e.Bounds.Y + 2);

            // отображение фокуса элемента
            e.DrawFocusRectangle();
        }

        private void cbFontName_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            e.ItemHeight = 20;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            // создание выбранного шрифта
            string name = cbFontName.Text.Trim();
            int size;

            if (name == "")
            {
                SelectedFont = null;
                DialogResult = DialogResult.OK;
            }
            else if (int.TryParse(cbFontSize.Text, out size))
            {
                SelectedFont = new DataTypes.Font();
                SelectedFont.Name = name;
                SelectedFont.Size = size;
                SelectedFont.Bold = chkBold.Checked;
                SelectedFont.Italic = chkItalic.Checked;
                SelectedFont.Underline = chkUnderline.Checked;
                DialogResult = DialogResult.OK;
            }
            else
            {
                ScadaUiUtils.ShowError("!!!"/*SchemePhrases.SizeInteger*/); // TODO
            }
        }
    }
}
