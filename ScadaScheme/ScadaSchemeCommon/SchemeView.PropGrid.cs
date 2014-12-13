/*
 * Copyright 2014 Mikhail Shiryaev
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
 * Summary  : Scheme view. Type converters and type editors for PropertyGrid component
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2012
 * Modified : 2014
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Scada.Scheme
{
    partial class SchemeView
	{
        /// <summary>
        /// Преобразователь структур для PropertyGrid
        /// </summary>
        private class StructConverter : TypeConverter
        {
            public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
            {
                return true;
            }

            public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
            {
                Type propType = context.PropertyDescriptor.PropertyType;
                if (propType == typeof(Point))
                    return new Point((int)propertyValues["X"], (int)propertyValues["Y"]);
                else if (propType == typeof(Size))
                    return new Size((int)propertyValues["Width"], (int)propertyValues["Height"]);
                else
                    return null;
            }

            public override bool GetPropertiesSupported(ITypeDescriptorContext context)
            {
                return true;
            }

            public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
            {
                PropertyDescriptorCollection props = TypeDescriptor.GetProperties(value, attributes);
                return value is Size ? props.Sort(new string[] { "Width", "Height" }) : props;
            }

            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
            {
                if (destinationType == typeof(string))
                {
                    if (value is Point)
                        return ((Point)value).X + "; " + ((Point)value).Y;
                    else if (value is Size)
                        return ((Size)value).Width + "; " + ((Size)value).Height;
                    else
                        return "";
                }
                else
                {
                    return base.ConvertTo(context, culture, value, destinationType);
                }
            }

            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
            }

            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                try
                {
                    if (value is string)
                    {
                        string[] parts = ((string)value).Split(Separator, StringSplitOptions.RemoveEmptyEntries);
                        int part0 = Convert.ToInt32(parts[0]);
                        int part1 = Convert.ToInt32(parts[1]); 
                        Type propType = context.PropertyDescriptor.PropertyType;
                        
                        if (propType == typeof(Size))
                            return new Size(part0, part1);
                        else if (propType == typeof(Point))
                            return new Point(part0, part1);
                        else
                            return base.ConvertFrom(context, culture, value);
                    }
                    else
                    {
                        return base.ConvertFrom(context, culture, value);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(SchemePhrases.StringConvertError + ": " + ex.Message);
                }
            }
        }

        /// <summary>
        /// Преобразователь перечислений для PropertyGrid
        /// </summary>
        private class EnumConverterEx : EnumConverter
        {
            private Type enumType;

            public EnumConverterEx(Type type)
                : base(type)
            {
                enumType = type;
            }

            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
            {
                if (destinationType == typeof(string))
                {
                    FieldInfo fi = enumType.GetField(Enum.GetName(enumType, value));
                    DescriptionAttribute da = (DescriptionAttribute)Attribute.GetCustomAttribute(fi, typeof(DescriptionAttribute));
                    return da == null ? value.ToString() : da.Description;
                }
                else
                {
                    return base.ConvertTo(context, culture, value, destinationType);
                }
            }

            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
            }

            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                if (value is string)
                {
                    foreach (FieldInfo fi in enumType.GetFields())
                    {
                        DescriptionAttribute da = (DescriptionAttribute)Attribute.GetCustomAttribute(fi, typeof(DescriptionAttribute));
                        if (da != null && (string)value == da.Description)
                            return Enum.Parse(enumType, fi.Name);
                    }

                    return Enum.Parse(enumType, (string)value);
                }
                else
                {
                    return base.ConvertFrom(context, culture, value);
                }
            }
        }

        /// <summary>
        /// Преобразователь логических значений для PropertyGrid
        /// </summary>
        private class BooleanConverterEx : BooleanConverter
        {
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
            {
                if (destinationType == typeof(string))
                    return (bool)value ? 
                        (Localization.UseRussian ? "Да" : "Yes") : 
                        (Localization.UseRussian ? "Нет" : "No");
                else
                    return base.ConvertTo(context, culture, value, destinationType);
            }

            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                if (value is string)
                {
                    string val = ((string)value).ToLowerInvariant();
                    return val == "да" || val == "yes";
                }
                else
                {
                    return base.ConvertFrom(context, culture, value);
                }
            }
        }

        /// <summary>
        /// Преобразователь коллекций для PropertyGrid
        /// </summary>
        private class CollectionConverter : TypeConverter
        {
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
            {
                if (destinationType == typeof(string))
                {
                    ICollection coll = value as ICollection;
                    return coll == null || coll.Count == 0 ? 
                        (Localization.UseRussian ? "(Нет)" : "(None)") : 
                        (Localization.UseRussian ? "(Коллекция)" : "(Collection)");
                }
                else
                {
                    return base.ConvertTo(context, culture, value, destinationType);
                }
            }
        }

        /// <summary>
        /// Преобразователь шрифтов для PropertyGrid
        /// </summary>
        private class FontConverter : TypeConverter
        {
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
            {
                if (destinationType == typeof(string))
                {
                    SchemeView.Font font = value as SchemeView.Font;

                    if (font == null)
                    {
                        return "";
                    }
                    else
                    {
                        StringBuilder sb = new StringBuilder();
                        if (!string.IsNullOrEmpty(font.Name))
                            sb.Append(font.Name);
                        if (sb.Length > 0)
                            sb.Append("; ");
                        sb.Append(font.Size);
                        if (font.Bold)
                            sb.Append(Localization.UseRussian ? "; Ж" : "; B");
                        if (font.Italic)
                            sb.Append(Localization.UseRussian ? "; К" : "; I");
                        if (font.Underline)
                            sb.Append(Localization.UseRussian ? "; П" : "; U");
                        return sb.ToString();
                    }
                }
                else
                {
                    return base.ConvertTo(context, culture, value, destinationType);
                }
            }
        }

        /// <summary>
        /// Преобразователь изображений для PropertyGrid
        /// </summary>
        private class ImageConverter : TypeConverter
        {
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
            {
                if (destinationType == typeof(string))
                {
                    SchemeView.Image image = value as SchemeView.Image;
                    return image == null ? 
                        (Localization.UseRussian ? "(Нет)" : "(None)") : 
                        image.Name;
                }
                else
                {
                    return base.ConvertTo(context, culture, value, destinationType);
                }
            }
        }

        /// <summary>
        /// Преобразователь наименований для PropertyGrid
        /// </summary>
        internal class NameConverter : StringConverter
        {
            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                string name = value as string;
                if (name != null)
                {
                    name = name.Trim();
                    value = name;
                    ICheckNameUnique itfCheckNameUnique = context.Instance as ICheckNameUnique;
                    if (itfCheckNameUnique != null && !itfCheckNameUnique.NameIsUnique(name))
                        throw new Exception(SchemePhrases.NameUniqueError);
                }
                return base.ConvertFrom(context, culture, value);
            }
        }

        /// <summary>
        /// Преобразователь фильтра по каналам для PropertyGrid
        /// </summary>
        private class CnlsFilterConverter : BooleanConverter
        {
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
            {
                if (destinationType == typeof(string))
                {
                    List<int> cnlFilter = value as List<int>;
                    return cnlFilter == null || cnlFilter.Count == 0 ? "" : string.Join<int>(" ", cnlFilter);
                }
                else
                {
                    return base.ConvertTo(context, culture, value, destinationType);
                }
            }
        }

        /// <summary>
        /// Редактор цветов для PropertyGrid
        /// </summary>
        private class ColorEditor : UITypeEditor
        {
            /// <summary>
            /// Сравнение цветов по восприятию
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
            private static object[] colorArr = null;
            private IWindowsFormsEditorService editorSvc = null;
            private ListBox listBox = null;
            private Brush textBrush = null;

            static ColorEditor()
            {
                // заполнениеи сортировка массива цветов
                List<object> colorList = new List<object>();
                Array knownColorArr = Enum.GetValues(typeof(KnownColor));
                int len = knownColorArr.Length;

                for (int i = 0; i < len; i++)
                {
                    Color color = Color.FromKnownColor((KnownColor)knownColorArr.GetValue(i));
                    if (!color.IsSystemColor)
                        colorList.Add(color);
                }

                colorList.Sort(new ColorComparer());
                colorList.Insert(0, "Status");
                colorArr = colorList.ToArray();
            }

            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                editorSvc = provider == null ? null : 
                    (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

                if (context != null && context.Instance != null && editorSvc != null)
                {
                    // создание и заполнение выпадающего списка
                    listBox = new ListBox();
                    textBrush = new SolidBrush(listBox.ForeColor);

                    listBox.BorderStyle = BorderStyle.None;
                    listBox.ItemHeight = 16;
                    listBox.IntegralHeight = false;
                    listBox.DrawMode = DrawMode.OwnerDrawFixed;
                    listBox.Items.AddRange(colorArr);

                    Type instanceType = context.Instance.GetType();
                    if (instanceType == typeof(SchemeView.Scheme) && context.PropertyDescriptor.Name != "ForeColor" ||
                        instanceType == typeof(SchemeView.StaticText) || 
                        instanceType == typeof(SchemeView.StaticPicture))
                        listBox.Items.RemoveAt(0); // запрет цвета, который определяется статусом входного канала

                    int n = listBox.Items.Count <= ItemCount ? listBox.Items.Count : ItemCount;
                    listBox.Height = n * listBox.ItemHeight;

                    listBox.DrawItem += listBox_DrawItem;
                    listBox.Click += listBox_Click;
                    listBox.KeyDown += listBox_KeyDown;

                    // выбор элемента в выпадающем списке
                    string selColor = value is string ? ((string)value).Trim().ToLower() : "";

                    if (selColor != "")
                    {
                        int cnt = listBox.Items.Count;
                        for (int i = 0; i < cnt; i++)
                        {
                            object item = listBox.Items[i];
                            if (item is string)
                            {
                                if (selColor == ((string)item).ToLower())
                                {
                                    listBox.SelectedIndex = i;
                                    break;
                                }
                            }
                            else if (item is Color)
                            {
                                if (selColor == ((Color)item).Name.ToLower())
                                {
                                    listBox.SelectedIndex = i;
                                    break;
                                }
                            }
                        }
                    }

                    // отображение выпадающего списка
                    editorSvc.DropDownControl(listBox);

                    // установка выбранного значения
                    object selItem = listBox.SelectedItem;
                    if (selItem is string)
                        value = selItem;
                    else if (selItem is Color)
                        value = ((Color)selItem).Name;
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
                object item = listBox.Items[e.Index];
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
                if (editorSvc != null)
                    editorSvc.CloseDropDown();
            }

            private void listBox_KeyDown(object sender, KeyEventArgs e)
            {
                if ((e.KeyCode == Keys.Escape || e.KeyCode == Keys.Enter) && editorSvc != null)
                    editorSvc.CloseDropDown();
            }
        }

        /// <summary>
        /// Редактор шрифтов для PropertyGrid
        /// </summary>
        private class FontEditor : UITypeEditor
        {
            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                IWindowsFormsEditorService editorSvc = provider == null ? null :
                    (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

                if (context != null && context.Instance != null && editorSvc != null)
                {
                    FrmFontDialog frmFontDialog = new FrmFontDialog();
                    frmFontDialog.SelectedFont = value as SchemeView.Font;

                    if (frmFontDialog.ShowDialog() == DialogResult.OK)
                        value = frmFontDialog.SelectedFont;
                }

                return value;
            }

            public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
            {
                return UITypeEditorEditStyle.Modal;
            }
        }

        /// <summary>
        /// Редактор условий для PropertyGrid
        /// </summary>
        private class CondEditor : UITypeEditor
        {
            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                IWindowsFormsEditorService editorSvc = provider == null ? null :
                    (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

                if (context != null && context.Instance != null && editorSvc != null)
                {
                    FrmCondDialog frmCondDialog = new FrmCondDialog();
                    frmCondDialog.SelectedConds = value as List<SchemeView.Condition>;

                    if (frmCondDialog.ShowDialog() == DialogResult.OK)
                        value = frmCondDialog.SelectedConds;
                }

                return value;
            }

            public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
            {
                return UITypeEditorEditStyle.Modal;
            }
        }

        /// <summary>
        /// Редактор изображений для PropertyGrid
        /// </summary>
        private class ImageEditor : UITypeEditor
        {
            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                IWindowsFormsEditorService editorSvc = provider == null ? null :
                    (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

                if (context != null && context.Instance != null && editorSvc != null)
                {
                    if (context.PropertyDescriptor.PropertyType == typeof(SchemeView.Image))
                    {
                        FrmImageDialog frmImageDialog = new FrmImageDialog();
                        frmImageDialog.SelectedImage = value as SchemeView.Image;

                        if (frmImageDialog.ShowDialog() == DialogResult.OK)
                            value = frmImageDialog.SelectedImage;
                    }
                    else
                    {
                        FrmImageDialog frmImageDialog = new FrmImageDialog();
                        frmImageDialog.CanSelectImage = false;
                        frmImageDialog.ShowDialog();
                    }
                }

                return value;
            }

            public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
            {
                return UITypeEditorEditStyle.Modal;
            }
        }

        /// <summary>
        /// Редактор фильтра по каналам для PropertyGrid
        /// </summary>
        private class CnlsFilterEditor : UITypeEditor
        {
            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                IWindowsFormsEditorService editorSvc = provider == null ? null :
                    (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

                if (context != null && context.Instance != null && editorSvc != null)
                    (new FrmCnlsFilterDialog()).ShowDialog();

                return value;
            }

            public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
            {
                return UITypeEditorEditStyle.Modal;
            }
        }
    }
}
