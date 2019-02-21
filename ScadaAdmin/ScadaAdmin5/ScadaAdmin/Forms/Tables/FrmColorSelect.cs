/*
 * Copyright 2019 Mikhail Shiryaev
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
 * Module   : Administrator
 * Summary  : Form for color selection
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2010
 * Modified : 2019
 */

using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using Scada.UI;

namespace Scada.Admin.App.Forms.Tables
{
    /// <summary>
    /// Form for color selection.
    /// <para>Форма выбора цвета.</para>
    /// </summary>
    public partial class FrmColorSelect : Form
    {
        /// <summary>
        /// Compares colors alphabetically.
        /// <para>Сравнивает цвета по алфавиту.</para>
        /// </summary>
        private class ColorComparer1 : IComparer
        {
            int IComparer.Compare(object x, object y)
            {
                Color cx = (Color)x;
                Color cy = (Color)y;
                return string.Compare(cx.Name, cy.Name);
            }
        }

        /// <summary>
        /// Compare colors by perception.
        /// <para>Сравнивает цвета по восприятию.</para>
        /// </summary>
        private class ColorComparer2 : IComparer
        {
            int IComparer.Compare(object x, object y)
            {
                Color cx = (Color)x;
                Color cy = (Color)y;
                float hx = cx.GetHue();
                float hy = cy.GetHue();
                float sx = cx.GetSaturation();
                float sy = cy.GetSaturation();
                float bx = cx.GetBrightness();
                float by = cy.GetBrightness();

                if (hx < hy)
                {
                    return -1;
                }
                else if (hx > hy)
                {
                    return 1;
                }
                else if (sx < sy)
                {
                    return -1;
                }
                else if (sx > sy)
                {
                    return 1;
                }
                else
                {
                    if (bx < by) return -1;
                    else if (bx > by) return 1;
                    else return 0;
                }
            }
        }


        private static readonly Color[] ColorArr1;
        private static readonly Color[] ColorArr2;
        private readonly Brush textBrush;


        /// <summary>
        /// Initializes the class.
        /// </summary>
        static FrmColorSelect()
        {
            // initialize color arrays
            Array knownColorArr = Enum.GetValues(typeof(KnownColor));
            int colorCnt = knownColorArr.Length;

            Color[] colorArr = new Color[colorCnt];
            for (int i = 0; i < colorCnt; i++)
            {
                colorArr[i] = Color.FromKnownColor((KnownColor)knownColorArr.GetValue(i));
            }

            ColorArr1 = new Color[colorCnt];
            ColorArr2 = new Color[colorCnt];
            Array.Copy(colorArr, ColorArr1, colorCnt);
            Array.Copy(colorArr, ColorArr2, colorCnt);
            Array.Sort(ColorArr1, new ColorComparer1());
            Array.Sort(ColorArr2, new ColorComparer2());
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmColorSelect()
        {
            InitializeComponent();

            textBrush = new SolidBrush(lbColor.ForeColor);
            SelectedColor = Color.Empty;
        }


        /// <summary>
        /// Gets or sets the selected color.
        /// </summary>
        public Color SelectedColor { get; set; }


        /// <summary>
        /// Fills the color list box.
        /// </summary>
        private void FillListBox(Color[] colorArr)
        {
            try
            {
                lbColor.BeginUpdate();
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
            finally
            {
                lbColor.EndUpdate();
            }
        }


        private void FrmColor_Load(object sender, EventArgs e)
        {
            Translator.TranslateForm(this, GetType().FullName);
            FillListBox(ColorArr1);
            ActiveControl = lbColor;
        }

        private void rbSortAbc_CheckedChanged(object sender, EventArgs e)
        {
            FillListBox(ColorArr1);
        }

        private void rbSortColor_CheckedChanged(object sender, EventArgs e)
        {
            FillListBox(ColorArr2);
        }

        private void lbColor_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();

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

            e.DrawFocusRectangle();
        }

        private void lbColor_DoubleClick(object sender, EventArgs e)
        {
            btnOK_Click(null, null);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            SelectedColor = lbColor.SelectedItem == null ? Color.Empty : (Color)lbColor.SelectedItem;
            DialogResult = DialogResult.OK;
        }
    }
}
