/*
 * Copyright 2015 Mikhail Shiryaev
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
 * Modified : 2015
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
    /// User interface translation
    /// <para>Перевод пользовательского интерфейса</para>
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
            /// Получить или установить список элементов
            /// </summary>
            public List<string> Items { get; set; }

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
        private static void TranslateWinControls(IList controls, WinForms.ToolTip toolTip,
            Dictionary<string, ControlInfo> controlInfoDict)
        {
            if (controls == null)
                return;

            foreach (object elem in controls)
            {
                ControlInfo controlInfo;

                if (elem is WinForms.Control)
                {
                    // обработка обычного элемента управления
                    WinForms.Control control = (WinForms.Control)elem;
                    if (!string.IsNullOrEmpty(control.Name) /*например, поле ввода и кнопки NumericUpDown*/ &&
                        controlInfoDict.TryGetValue(control.Name, out controlInfo))
                    {
                        if (controlInfo.Text != null)
                            control.Text = controlInfo.Text;

                        if (controlInfo.ToolTip != null && toolTip != null)
                            toolTip.SetToolTip(control, controlInfo.ToolTip);

                        if (controlInfo.Items != null && elem is WinForms.ComboBox)
                        {
                            WinForms.ComboBox comboBox = (WinForms.ComboBox)elem;
                            int cnt = Math.Min(comboBox.Items.Count, controlInfo.Items.Count);
                            for (int i = 0; i < cnt; i++)
                            {
                                string itemText = controlInfo.Items[i];
                                if (itemText != null)
                                    comboBox.Items[i] = itemText;
                            }
                        }
                    }

                    if (elem is WinForms.MenuStrip)
                    {
                        // запуск обработки элементов меню
                        WinForms.MenuStrip menuStrip = (WinForms.MenuStrip)elem;
                        TranslateWinControls(menuStrip.Items, toolTip, controlInfoDict);
                    }
                    else if (elem is WinForms.ToolStrip)
                    {
                        // запуск обработки элементов панели инструментов
                        WinForms.ToolStrip toolStrip = (WinForms.ToolStrip)elem;
                        TranslateWinControls(toolStrip.Items, toolTip, controlInfoDict);
                    }
                    else if (elem is WinForms.DataGridView)
                    {
                        // запуск обработки столбцов таблицы
                        WinForms.DataGridView dataGridView = (WinForms.DataGridView)elem;
                        TranslateWinControls(dataGridView.Columns, toolTip, controlInfoDict);
                    }
                    else if (elem is WinForms.ListView)
                    {
                        // запуск обработки столбцов списка
                        WinForms.ListView listView = (WinForms.ListView)elem;
                        TranslateWinControls(listView.Columns, toolTip, controlInfoDict);
                    }

                    // запуск обработки дочерних элементов
                    if (control.HasChildren)
                        TranslateWinControls(control.Controls, toolTip, controlInfoDict);
                }
                else if (elem is WinForms.ToolStripItem)
                {
                    // обработка элемента меню или элемента панели инструментов
                    WinForms.ToolStripItem item = (WinForms.ToolStripItem)elem;
                    if (controlInfoDict.TryGetValue(item.Name, out controlInfo))
                    {
                        if (controlInfo.Text != null)
                            item.Text = controlInfo.Text;
                        if (controlInfo.ToolTip != null)
                            item.ToolTipText = controlInfo.ToolTip;
                    }

                    if (elem is WinForms.ToolStripMenuItem)
                    {
                        // запуск обработки элементов подменю
                        WinForms.ToolStripMenuItem menuItem = (WinForms.ToolStripMenuItem)elem;
                        if (menuItem.HasDropDownItems)
                            TranslateWinControls(menuItem.DropDownItems, toolTip, controlInfoDict);
                    }
                }
                else if (elem is WinForms.DataGridViewColumn)
                {
                    // обработка столбца таблицы
                    WinForms.DataGridViewColumn column = (WinForms.DataGridViewColumn)elem;
                    if (controlInfoDict.TryGetValue(column.Name, out controlInfo) && controlInfo.Text != null)
                        column.HeaderText = controlInfo.Text;
                }
                else if (elem is WinForms.ColumnHeader)
                {
                    // обработка столбца списка
                    WinForms.ColumnHeader columnHeader = (WinForms.ColumnHeader)elem;
                    if (controlInfoDict.TryGetValue(columnHeader.Name, out controlInfo) && controlInfo.Text != null)
                        columnHeader.Text = controlInfo.Text;
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
                ControlInfo controlInfo;

                if (!string.IsNullOrEmpty(control.ID) && controlInfoDict.TryGetValue(control.ID, out controlInfo))
                {
                    if (control is Label)
                    {
                        Label label = (Label)control;
                        if (controlInfo.Text != null)
                            label.Text = controlInfo.Text;
                    }
                    else if (control is CheckBox)
                    {
                        CheckBox checkBox = (CheckBox)control;
                        if (controlInfo.Text != null)
                            checkBox.Text = controlInfo.Text;
                    }
                    else if (control is Button)
                    {
                        Button button = (Button)control;
                        if (controlInfo.Text != null)
                            button.Text = controlInfo.Text;
                    }
                    else if (control is Image)
                    {
                        Image image = (Image)control;
                        if (controlInfo.ToolTip != null)
                            image.ToolTip = controlInfo.ToolTip;
                    }
                    else if (control is Panel)
                    {
                        Panel panel = (Panel)control;
                        if (controlInfo.ToolTip != null)
                            panel.ToolTip = controlInfo.ToolTip;
                    }
                }

                // запуск обработки дочерних элементов
                TranslateWebControls(control.Controls, controlInfoDict);
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
                ControlInfo controlInfo;
                if (controlInfoDict.TryGetValue("this", out controlInfo) && controlInfo.Text != null)
                    form.Text = controlInfo.Text;

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
                ControlInfo controlInfo;
                if (controlInfoDict.TryGetValue("this", out controlInfo) && controlInfo.Text != null)
                    page.Title = controlInfo.Text;

                // перевод элементов управления
                TranslateWebControls(page.Controls, controlInfoDict);
            }
        }
    }
}
