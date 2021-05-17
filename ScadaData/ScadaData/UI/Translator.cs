/*
 * Copyright 2021 Mikhail Shiryaev
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
 * Module   : ScadaData
 * Summary  : User interface translation
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2021
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using WinForms = System.Windows.Forms;

namespace Scada.UI
{
    /// <summary>
    /// User interface translation.
    /// <para>Перевод пользовательского интерфейса.</para>
    /// </summary>
    public static class Translator
    {
        /// <summary>
        /// Информация об элементе управления
        /// </summary>
        private class ControlInfo
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            public ControlInfo()
            {
                Text = null;
                ToolTip = null;
                Props = null;
                Items = null;
            }

            /// <summary>
            /// Получить или установить текст
            /// </summary>
            public string Text { get; set; }
            /// <summary>
            /// Получить или установить всплывающую подсказку
            /// </summary>
            public string ToolTip { get; set; }
            /// <summary>
            /// Получить словарь свойств, исключая текст и подсказку
            /// </summary>
            public Dictionary<string, string> Props { get; private set; }
            /// <summary>
            /// Получить список элементов
            /// </summary>
            public List<string> Items { get; private set; }

            /// <summary>
            /// Установить значение свойства, инициализировав словарь при необходимости
            /// </summary>
            public void SetProp(string name, string val)
            {
                if (Props == null)
                    Props = new Dictionary<string, string>();
                Props[name] = val;
            }
            /// <summary>
            /// Установить значение элемента списка, инициализировав список при необходимости
            /// </summary>
            public void SetItem(int index, string val)
            {
                if (Items == null)
                    Items = new List<string>();

                if (index < Items.Count)
                {
                    Items[index] = val;
                }
                else
                {
                    while (Items.Count < index)
                        Items.Add(null);
                    Items.Add(val);
                }
            }
        }


        /// <summary>
        /// Gets or sets a value indicating whether to append the product name to a form title.
        /// </summary>
        public static bool AppendProductName { get; set; } = false;


        /// <summary>
        /// Получить информацию об элементах управления из словаря
        /// </summary>
        private static Dictionary<string, ControlInfo> GetControlInfoDict(Localization.Dict dict)
        {
            Dictionary<string, ControlInfo> controlInfoDict = new Dictionary<string, ControlInfo>();

            foreach (string phraseKey in dict.Phrases.Keys)
            {
                string phraseVal = dict.Phrases[phraseKey];
                int dotPos = phraseKey.IndexOf('.');

                if (dotPos < 0)
                {
                    // если точки в ключе фразы нет, то присваивается свойство текст
                    if (controlInfoDict.ContainsKey(phraseKey))
                        controlInfoDict[phraseKey].Text = phraseVal;
                    else
                        controlInfoDict[phraseKey] = new ControlInfo() { Text = phraseVal };
                }
                else if (0 < dotPos && dotPos < phraseKey.Length - 1)
                {
                    // если точка в середине ключа фразы, то слева от точки - имя элемента, справа - свойство
                    string ctrlName = phraseKey.Substring(0, dotPos);
                    string ctrlProp = phraseKey.Substring(dotPos + 1);
                    bool propAssigned = true;

                    ControlInfo controlInfo;
                    if (!controlInfoDict.TryGetValue(ctrlName, out controlInfo))
                        controlInfo = new ControlInfo();

                    if (ctrlProp == "Text")
                    {
                        controlInfo.Text = phraseVal;
                    }
                    else if (ctrlProp == "ToolTip")
                    {
                        controlInfo.ToolTip = phraseVal;
                    }
                    else if (ctrlProp.StartsWith("Items["))
                    {
                        int pos = ctrlProp.IndexOf(']');
                        int ind;
                        if (pos >= 0 && int.TryParse(ctrlProp.Substring(6, pos - 6), out ind))
                            controlInfo.SetItem(ind, phraseVal);
                    }
                    else if (ctrlProp != "")
                    {
                        controlInfo.SetProp(ctrlProp, phraseVal);
                    }
                    else
                    {
                        propAssigned = false;
                    }

                    if (propAssigned)
                        controlInfoDict[ctrlName] = controlInfo;
                }
            }

            return controlInfoDict;
        }

        /// <summary>
        /// Рекурсивно перевести элементы управления Windows-формы
        /// </summary>
        private static void TranslateWinControls(ICollection controls, WinForms.ToolTip toolTip,
            Dictionary<string, ControlInfo> controlInfoDict)
        {
            if (controls == null)
                return;

            foreach (object elem in controls)
            {
                ControlInfo controlInfo;

                if (elem is WinForms.Control control)
                {
                    // обработка обычного элемента управления
                    if (!string.IsNullOrEmpty(control.Name) /*например, поле ввода и кнопки NumericUpDown*/ &&
                        controlInfoDict.TryGetValue(control.Name, out controlInfo))
                    {
                        if (controlInfo.Text != null)
                            control.Text = controlInfo.Text;

                        if (controlInfo.ToolTip != null && toolTip != null)
                            toolTip.SetToolTip(control, controlInfo.ToolTip);

                        if (controlInfo.Items != null)
                        {
                            int itemCnt = controlInfo.Items.Count;

                            if (elem is WinForms.ComboBox comboBox)
                            {
                                for (int i = 0, cnt = Math.Min(comboBox.Items.Count, itemCnt); i < cnt; i++)
                                {
                                    string itemText = controlInfo.Items[i];
                                    if (itemText != null)
                                        comboBox.Items[i] = itemText;
                                }
                            }
                            else if (elem is WinForms.ListBox listBox)
                            {
                                for (int i = 0, cnt = Math.Min(listBox.Items.Count, itemCnt); i < cnt; i++)
                                {
                                    string itemText = controlInfo.Items[i];
                                    if (itemText != null)
                                        listBox.Items[i] = itemText;
                                }
                            }
                            else if (elem is WinForms.ListView listView)
                            {
                                for (int i = 0, cnt = Math.Min(listView.Items.Count, itemCnt); i < cnt; i++)
                                {
                                    string itemText = controlInfo.Items[i];
                                    if (itemText != null)
                                        listView.Items[i].Text = itemText;
                                }
                            }
                        }
                    }

                    // запуск обработки вложенных элементов
                    if (elem is WinForms.MenuStrip menuStrip)
                    {
                        // запуск обработки элементов меню
                        TranslateWinControls(menuStrip.Items, toolTip, controlInfoDict);
                    }
                    else if (elem is WinForms.ToolStrip toolStrip)
                    {
                        // запуск обработки элементов панели инструментов
                        TranslateWinControls(toolStrip.Items, toolTip, controlInfoDict);
                    }
                    else if (elem is WinForms.DataGridView dataGridView)
                    {
                        // запуск обработки столбцов таблицы
                        TranslateWinControls(dataGridView.Columns, toolTip, controlInfoDict);
                    }
                    else if (elem is WinForms.ListView listView)
                    {
                        // запуск обработки столбцов и групп списка
                        TranslateWinControls(listView.Columns, toolTip, controlInfoDict);
                        TranslateWinControls(listView.Groups, toolTip, controlInfoDict);
                    }

                    // запуск обработки дочерних элементов
                    if (control.HasChildren)
                        TranslateWinControls(control.Controls, toolTip, controlInfoDict);
                }
                else if (elem is WinForms.ToolStripItem toolStripItem)
                {
                    // обработка элемента меню или элемента панели инструментов
                    if (controlInfoDict.TryGetValue(toolStripItem.Name, out controlInfo))
                    {
                        if (controlInfo.Text != null)
                            toolStripItem.Text = controlInfo.Text;
                        if (controlInfo.ToolTip != null)
                            toolStripItem.ToolTipText = controlInfo.ToolTip;
                    }

                    // запуск обработки элементов подменю
                    if (elem is WinForms.ToolStripDropDownItem dropDownItem && dropDownItem.HasDropDownItems)
                        TranslateWinControls(dropDownItem.DropDownItems, toolTip, controlInfoDict);
                }
                else if (elem is WinForms.DataGridViewColumn column)
                {
                    // обработка столбца таблицы
                    if (controlInfoDict.TryGetValue(column.Name, out controlInfo) && controlInfo.Text != null)
                        column.HeaderText = controlInfo.Text;
                }
                else if (elem is WinForms.ColumnHeader columnHeader)
                {
                    // обработка столбца списка
                    if (controlInfoDict.TryGetValue(columnHeader.Name, out controlInfo) && controlInfo.Text != null)
                        columnHeader.Text = controlInfo.Text;
                }
                else if (elem is WinForms.ListViewGroup listViewGroup)
                {
                    // обработка группы списка
                    if (controlInfoDict.TryGetValue(listViewGroup.Name, out controlInfo) && controlInfo.Text != null)
                        listViewGroup.Header = controlInfo.Text;
                }
            }
        }

        /// <summary>
        /// Рекурсивно перевести элементы управления веб-формы
        /// </summary>
        private static void TranslateWebControls(ControlCollection controls,
            Dictionary<string, ControlInfo> controlInfoDict)
        {
            if (controls == null)
                return;

            foreach (Control control in controls)
            {
                if (!string.IsNullOrEmpty(control.ID) && 
                    controlInfoDict.TryGetValue(control.ID, out ControlInfo controlInfo))
                {
                    if (control is Label label)
                    {
                        if (controlInfo.Text != null)
                            label.Text = controlInfo.Text;
                        if (controlInfo.ToolTip != null)
                            label.ToolTip = controlInfo.ToolTip;
                    }
                    else if (control is TextBox textBox)
                    {
                        if (controlInfo.Text != null)
                            textBox.Text = controlInfo.Text;
                        if (controlInfo.ToolTip != null)
                            textBox.ToolTip = controlInfo.ToolTip;
                    }
                    else if (control is CheckBox checkBox)
                    {
                        if (controlInfo.Text != null)
                            checkBox.Text = controlInfo.Text;
                        if (controlInfo.ToolTip != null)
                            checkBox.ToolTip = controlInfo.ToolTip;
                    }
                    else if (control is HyperLink hyperLink)
                    {
                        if (controlInfo.Text != null)
                            hyperLink.Text = controlInfo.Text;
                        if (controlInfo.Props != null &&
                            controlInfo.Props.TryGetValue("NavigateUrl", out string navigateUrl))
                            hyperLink.NavigateUrl = navigateUrl;
                    }
                    else if (control is Button button)
                    {
                        if (controlInfo.Text != null)
                            button.Text = controlInfo.Text;
                        if (controlInfo.ToolTip != null)
                            button.ToolTip = controlInfo.ToolTip;
                    }
                    else if (control is LinkButton linkButton)
                    {
                        if (controlInfo.Text != null)
                            linkButton.Text = controlInfo.Text;
                        if (controlInfo.ToolTip != null)
                            linkButton.ToolTip = controlInfo.ToolTip;
                    }
                    else if (control is Image image)
                    {
                        if (controlInfo.ToolTip != null)
                            image.ToolTip = controlInfo.ToolTip;
                    }
                    else if (control is Panel panel)
                    {
                        if (controlInfo.ToolTip != null)
                            panel.ToolTip = controlInfo.ToolTip;
                    }
                    else if (control is HiddenField hiddenField)
                    {
                        if (controlInfo.Text != null)
                            hiddenField.Value = controlInfo.Text;
                    }
                    else if (control is GridView gridView)
                    {
                        if (controlInfo.Items != null)
                        {
                            for (int i = 0, cnt = Math.Min(gridView.Columns.Count, controlInfo.Items.Count); 
                                i < cnt; i++)
                            {
                                string itemText = controlInfo.Items[i];
                                if (itemText != null)
                                    gridView.Columns[i].HeaderText = itemText;
                            }
                        }
                    }
                }

                // запуск обработки дочерних элементов
                TranslateWebControls(control.Controls, controlInfoDict);
            }
        }

        /// <summary>
        /// Gets the form title, optionally adding the product name.
        /// </summary>
        private static string GetFormTitle(string s)
        {
            if (AppendProductName)
            {
                string suffix = " - " + CommonPhrases.ProductName;
                return s.EndsWith(suffix) ? s : s + suffix;
            }
            else
            {
                return s;
            }
        }


        /// <summary>
        /// Перевести форму, используя заданный словарь
        /// </summary>
        public static void TranslateForm(WinForms.Form form, string dictName,
            WinForms.ToolTip toolTip = null, params WinForms.ContextMenuStrip[] contextMenus)
        {
            Localization.Dict dict;
            if (form != null && Localization.Dictionaries.TryGetValue(dictName, out dict))
            {
                Dictionary<string, ControlInfo> controlInfoDict = GetControlInfoDict(dict);

                // перевод заголовка формы
                if (controlInfoDict.TryGetValue("this", out ControlInfo controlInfo) && controlInfo.Text != null)
                    form.Text = GetFormTitle(controlInfo.Text);

                // перевод элементов управления
                TranslateWinControls(form.Controls, toolTip, controlInfoDict);

                // перевод контекстных меню
                if (contextMenus != null)
                    TranslateWinControls(contextMenus, null, controlInfoDict);
            }
        }

        /// <summary>
        /// Перевести веб-страницу, используя заданный словарь
        /// </summary>
        public static void TranslatePage(Page page, string dictName)
        {

            Localization.Dict dict;
            if (page != null && Localization.Dictionaries.TryGetValue(dictName, out dict))
            {
                Dictionary<string, ControlInfo> controlInfoDict = GetControlInfoDict(dict);

                // перевод заголовка страницы
                if (controlInfoDict.TryGetValue("this", out ControlInfo controlInfo) && controlInfo.Text != null)
                    page.Title = GetFormTitle(controlInfo.Text);

                // перевод элементов управления
                TranslateWebControls(page.Controls, controlInfoDict);
            }
        }
    }
}
