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
 * Module   : ScadaSchemeCommon
 * Summary  : Editor of color for PropertyGrid
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2019
 */

#pragma warning disable 1591 // CS1591: Missing XML comment for publicly visible type or member

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Scada.Scheme.Model.PropertyGrid
{
    /// <summary>
    /// Editor of color for PropertyGrid.
    /// <para>Редактор цвета для PropertyGrid.</para>
    /// </summary>
    public class ColorEditor : UITypeEditor
    {
        /// <summary>
        /// Сравнение цветов по восприятию.
        /// </summary>
        private class ColorComparer : IComparer<object>
        {
            int IComparer<object>.Compare(object x, object y)
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

        private const int ItemCount = 12;
        private static readonly object[] ColorArr = null;

        private IWindowsFormsEditorService editorSvc = null;
        private Brush textBrush = null;


        static ColorEditor()
        {
            // заполнение и сортировка массива цветов
            List<object> colorList = new List<object>();
            Array knownColorArr = Enum.GetValues(typeof(KnownColor));

            for (int i = 0, len = knownColorArr.Length; i < len; i++)
            {
                Color color = Color.FromKnownColor((KnownColor)knownColorArr.GetValue(i));
                if (!color.IsSystemColor)
                    colorList.Add(color);
            }

            colorList.Sort(new ColorComparer());
            colorList.Insert(0, SchemeUtils.StatusColor);
            ColorArr = colorList.ToArray();
        }


        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            editorSvc = provider?.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;

            if (context != null && context.Instance != null && editorSvc != null)
            {
                // создание и заполнение выпадающего списка
                ListBox listBox = new ListBox
                {
                    BorderStyle = BorderStyle.None,
                    ItemHeight = 16,
                    IntegralHeight = false,
                    DrawMode = DrawMode.OwnerDrawFixed
                };

                listBox.Items.AddRange(ColorArr);
                listBox.Height = Math.Min(listBox.Items.Count, ItemCount) * listBox.ItemHeight;
                listBox.DrawItem += listBox_DrawItem;
                listBox.Click += listBox_Click;
                listBox.KeyDown += listBox_KeyDown;

                // выбор элемента в выпадающем списке
                if (value is string valStr && !string.IsNullOrWhiteSpace(valStr))
                {
                    valStr = valStr.Trim().ToLowerInvariant();
                    int index = 0;

                    foreach (object item in listBox.Items)
                    {
                        if (item is string s)
                        {
                            if (valStr == s.ToLowerInvariant())
                            {
                                listBox.SelectedIndex = index;
                                break;
                            }
                        }
                        else if (item is Color color)
                        {
                            if (valStr == color.Name.ToLowerInvariant())
                            {
                                listBox.SelectedIndex = index;
                                break;
                            }
                        }
                        index++;
                    }
                }

                // отображение выпадающего списка
                textBrush = new SolidBrush(listBox.ForeColor);
                editorSvc.DropDownControl(listBox);

                // установка выбранного значения
                if (listBox.SelectedItem is string strVal)
                    value = strVal;
                else if (listBox.SelectedItem is Color colorVal)
                    value = colorVal.Name;

                // очистка ресурсов выпадающего списка и кисти
                listBox.Dispose();
                textBrush.Dispose();
            }

            return value;
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        private void listBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            // отображение заднего фона элемента
            e.DrawBackground();

            // отображение значка и текста элемента
            object item = ((ListBox)sender).Items[e.Index];
            Color color;
            string text;

            if (item is Color)
            {
                color = (Color)item;
                text = color.Name;
            }
            else
            {
                color = Color.White;
                text = item.ToString();
            }

            Graphics graphics = e.Graphics;
            int x = e.Bounds.X;
            int y = e.Bounds.Y;
            int w = e.Bounds.Width;
            int h = e.Bounds.Height;

            graphics.DrawRectangle(Pens.Black, x + 2, y + 2, 21, h - 5);
            graphics.FillRectangle(new SolidBrush(color), x + 3, y + 3, 20, h - 6);
            graphics.DrawString(text, e.Font, textBrush, x + 26, y + 2);

            // отображение фокуса элемента
            e.DrawFocusRectangle();
        }

        private void listBox_Click(object sender, EventArgs e)
        {
            editorSvc?.CloseDropDown();
        }

        private void listBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape || e.KeyCode == Keys.Enter)
                editorSvc?.CloseDropDown();
        }
    }
}
