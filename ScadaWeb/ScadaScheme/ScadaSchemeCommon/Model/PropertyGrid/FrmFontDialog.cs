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
        /// Элемент списка шрифтов
        /// </summary>
        private class FontListItem
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            public FontListItem(FontFamily fontFamily)
            {
                FontFamily = fontFamily;
            }

            /// <summary>
            /// Получить группу шрифтов
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
        /// Конструктор, ограничивающий создание объекта без параметров
        /// </summary>
        private FrmFontDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public FrmFontDialog(DataTypes.Font font)
            : this()
        {
            FontResult = font;
        }


        /// <summary>
        /// Получить шрифт в результате редактирования
        /// </summary>
        public DataTypes.Font FontResult { get; private set; }


        /// <summary>
        /// Заполнить список доступных шрифтов
        /// </summary>
        private void FillFontList()
        {
            try
            {
                cbFontName.BeginUpdate();
                foreach (FontFamily fontFamily in FontFamily.Families)
                {
                    if (fontFamily.IsStyleAvailable(FontStyle.Regular))
                        cbFontName.Items.Add(new FontListItem(fontFamily));
                }
            }
            finally
            {
                cbFontName.EndUpdate();
            }
        }


        private void FrmFontDialog_Load(object sender, EventArgs e)
        {
            // перевод формы
            Translator.TranslateForm(this, "Scada.Scheme.Model.PropertyGrid.FrmFontDialog");

            // заполнение списка шрифтов
            FillFontList();

            // создание кисти для вывода текста
            textBrush = new SolidBrush(cbFontName.ForeColor);

            // вывод шрифта
            if (FontResult != null)
            {
                cbFontName.Text = FontResult.Name;
                cbFontSize.Text = FontResult.Size.ToString();
                chkBold.Checked = FontResult.Bold;
                chkItalic.Checked = FontResult.Italic;
                chkUnderline.Checked = FontResult.Underline;
            }
        }

        private void FrmFontDialog_FormClosed(object sender, FormClosedEventArgs e)
        {
            // очистка ресурсов кисти
            textBrush.Dispose();
        }

        private void cbFontName_DrawItem(object sender, DrawItemEventArgs e)
        {
            // отображение заднего фона элемента
            e.DrawBackground();

            // отображение значка и текста элемента
            FontListItem item = (FontListItem)cbFontName.Items[e.Index];
            Font font = new Font(item.FontFamily, 14, FontStyle.Regular, GraphicsUnit.Pixel);
            e.Graphics.DrawString(item.ToString(), font, textBrush, e.Bounds.X + 2, e.Bounds.Y + 2);

            // отображение фокуса элемента
            e.DrawFocusRectangle();
        }

        private void cbFontName_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            e.ItemHeight = 20;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string fontName = cbFontName.Text.Trim();

            if (fontName == "")
            {
                FontResult = null;
                DialogResult = DialogResult.OK;
            }
            else 
            {
                int size;
                if (!int.TryParse(cbFontSize.Text, out size))
                    size = DataTypes.Font.DefaultSize;

                // создание шрифта в соответствии с выбранными параметрами
                FontResult = new DataTypes.Font();
                FontResult.Name = fontName;
                FontResult.Size = size;
                FontResult.Bold = chkBold.Checked;
                FontResult.Italic = chkItalic.Checked;
                FontResult.Underline = chkUnderline.Checked;
                DialogResult = DialogResult.OK;
            }
        }
    }
}
