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
 * Summary  : Selecting color form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2010
 * Modified : 2016
 */

using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using Scada;
using Scada.UI;

namespace ScadaAdmin
{
    /// <summary>
    /// Selecting color form
    /// <para>Форма выбора цвета</para>
    /// </summary>
    public partial class FrmSelectColor : Form
    {
        /// <summary>
        /// Сравнение цветов по алфавиту
        /// </summary>
        private class ColorComparer1 : IComparer
        {
            int IComparer.Compare(Object x, Object y)
            {
                Color cx = (Color)x;
                Color cy = (Color)y;
                return string.Compare(cx.Name, cy.Name);
            }
        }

        /// <summary>
        /// Сравнение цветов по восприятию
        /// </summary>
        private class ColorComparer2 : IComparer
        {
            int IComparer.Compare(Object x, Object y)
            {
                Color cx = (Color)x;
                Color cy = (Color)y;
                float hx = cx.GetHue();
                float hy = cy.GetHue();
                float sx = cx.GetSaturation();
                float sy = cy.GetSaturation();
                float bx = cx.GetBrightness();
                float by = cy.GetBrightness();

                if (hx < hy) return -1;
                else if (hx > hy) return 1;
                else
                {
                    if (sx < sy) return -1;
                    else if (sx > sy) return 1;
                    else
                    {
                        if (bx < by) return -1;
                        else if (bx > by) return 1;
                        else return 0;
                    }
                }
            }
        }


        private static Color[] colorArr1 = null;
        private static Color[] colorArr2 = null;
        private Brush textBrush;


        /// <summary>
        /// Конструктор
        /// </summary>
        public FrmSelectColor()
        {
            InitializeComponent();
            SelectedColor = Color.Empty;
        }


        /// <summary>
        /// Заполненить список цветов
        /// </summary>
        private void FillListBox(Color[] colorArr)
        {
            int selArgb = SelectedColor.ToArgb();
            int selInd = 0;
            lbColor.Items.Clear();

            foreach (Color color in colorArr)
            {
                if (!color.IsSystemColor && color != Color.Transparent)
                {
                    int ind = lbColor.Items.Add(color);
                    if (color.ToArgb() == selArgb)
                        selInd = ind;
                }
            }

            lbColor.SelectedIndex = selInd;
        }


        /// <summary>
        /// Получить или установить выбранный цвет
        /// </summary>
        public Color SelectedColor { get; set; }

        /// <summary>
        /// Получить наименование (строковую запись) выбранного цвета
        /// </summary>
        public string SelectedColorName
        {
            get
            {
                return SelectedColor.Name;
            }
        }


        private void FrmColor_Load(object sender, EventArgs e)
        {
            // перевод формы
            Translator.TranslateForm(this, "ScadaAdmin.FrmSelectColor");

            // заполнение массивов цветов
            if (colorArr1 == null)
            {
                Array knownColorArr = Enum.GetValues(typeof(KnownColor));
                int len = knownColorArr.Length;

                Color[] colorArr = new Color[len];
                for (int i = 0; i < len; i++)
                    colorArr[i] = Color.FromKnownColor((KnownColor)knownColorArr.GetValue(i));

                colorArr1 = new Color[len];
                colorArr2 = new Color[len];
                Array.Copy(colorArr, colorArr1, len);
                Array.Copy(colorArr, colorArr2, len);
                Array.Sort(colorArr1, new ColorComparer1());
                Array.Sort(colorArr2, new ColorComparer2());
            }

            // определение цвета текста элементов списка
            textBrush = new SolidBrush(lbColor.ForeColor);

            // заполнение списка цветов
            FillListBox(colorArr1);
            ActiveControl = lbColor;
        }

        private void rbSortAbc_CheckedChanged(object sender, EventArgs e)
        {
            FillListBox(colorArr1);
        }

        private void rbSortColor_CheckedChanged(object sender, EventArgs e)
        {
            FillListBox(colorArr2);
        }

        private void lbColor_DrawItem(object sender, DrawItemEventArgs e)
        {
            // отображение заднего фона элемента
            e.DrawBackground();

            // отображение значка и текста элемента
            Color color = (Color)lbColor.Items[e.Index];
            Brush brush = new SolidBrush(color);

            Graphics graphics = e.Graphics;
            int x = e.Bounds.X;
            int y = e.Bounds.Y;
            int w = e.Bounds.Width;
            int h = e.Bounds.Height;

            graphics.DrawRectangle(Pens.Black, x + 2, y + 2, 21, h - 5);
            graphics.FillRectangle(brush, x + 3, y + 3, 20, h - 6);
            graphics.DrawString(color.Name, e.Font, textBrush, x + 26, y + 2);

            // отображение фокуса элемента
            e.DrawFocusRectangle();
        }

        private void lbColor_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedColor = lbColor.SelectedItem == null ? Color.Empty : (Color)lbColor.SelectedItem;
        }

        private void lbColor_DoubleClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
