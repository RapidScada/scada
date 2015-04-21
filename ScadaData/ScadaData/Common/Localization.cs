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
 * Summary  : Localization mechanism
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2014
 * Modified : 2015
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using Microsoft.Win32;
using WinForms = System.Windows.Forms;

namespace Scada
{
    /// <summary>
    /// Localization mechanism
    /// <para>Механизм локализации</para>
    /// </summary>
    public static class Localization
    {
        /// <summary>
        /// Словарь
        /// </summary>
        public class Dict
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            private Dict()
            {

            }
            /// <summary>
            /// Конструктор
            /// </summary>
            public Dict(string key)
            {
                Key = key;
                Phrases = new Dictionary<string, string>();
            }

            /// <summary>
            /// Получить ключ словаря
            /// </summary>
            public string Key { get; private set; }
            /// <summary>
            /// Получить фразы, содержащиеся в словаре, по их ключам
            /// </summary>
            public Dictionary<string, string> Phrases { get; private set; }

            /// <summary>
            /// Получить фразу из словаря по ключу или значение по умолчанию при её отсутствии
            /// </summary>
            public string GetPhrase(string key, string defaultVal)
            {
                return Phrases.ContainsKey(key) ? Phrases[key] : defaultVal;
            }
        }

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
        /// Конструктор
        /// </summary>
        static Localization()
        {
            ReadCulture();
            Dictionaries = new Dictionary<string, Dict>();
        }


        /// <summary>
        /// Получить информацию о культуре всех приложений SCADA
        /// </summary>
        public static CultureInfo Culture { get; private set; }

        /// <summary>
        /// Получить признак использования русской локализации
        /// </summary>
        public static bool UseRussian { get; private set; }

        /// <summary>
        /// Получить загруженные словари для локализации
        /// </summary>
        public static Dictionary<string, Dict> Dictionaries { get; private set; }


        /// <summary>
        /// Получить информацию об элементах управления из словаря
        /// </summary>
        private static Dictionary<string, ControlInfo> GetControlInfoDict(Dict dict)
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
        /// Считать информацию о культуре из реестра
        /// </summary>
        private static void ReadCulture()
        {
            try
            {
                using (RegistryKey key = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).
                    OpenSubKey("Software\\SCADA", false))
                    Culture = CultureInfo.GetCultureInfo(key.GetValue("Culture").ToString());
            }
            catch
            {
                Culture = CultureIsRussian(CultureInfo.CurrentCulture) ? 
                    CultureInfo.GetCultureInfo("ru-RU") : CultureInfo.GetCultureInfo("en-GB");
            }
            finally
            {
                UseRussian = CultureIsRussian(Culture);
            }
        }

        /// <summary>
        /// Проверить, что имя культуры соответствует русской культуре
        /// </summary>
        private static bool CultureIsRussian(CultureInfo cultureInfo)
        {
            return cultureInfo.Name == "ru" || cultureInfo.Name.StartsWith("ru-", StringComparison.OrdinalIgnoreCase);
        }


        /// <summary>
        /// Записать информацию о культуре в реестр
        /// </summary>
        public static bool WriteCulture(string cultureName, out string errMsg)
        {
            try
            {
                using (RegistryKey key = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).
                    CreateSubKey("Software\\SCADA"))
                    key.SetValue("Culture", cultureName);
                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = (UseRussian ? "Ошибка при записи информации о культуре в реестр: " : 
                    "Error writing culture info to the registry: ") + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Загрузить словари для считанной культуры
        /// </summary>
        /// <remarks>Если ключ загружаемого словаря совпадает с ключом уже загруженного, то словари сливаются.
        /// Если совпадают ключи фраз, то новое значение фразы записывается поверх старого</remarks>
        public static bool LoadDictionaries(string directory, string fileNamePrefix, out string errMsg)
        {
            string fileName = GetDictionaryFileName(directory, fileNamePrefix);
            return LoadDictionaries(fileName, out errMsg);
        }

        /// <summary>
        /// Загрузить словари для считанной культуры
        /// </summary>
        public static bool LoadDictionaries(string fileName, out string errMsg)
        {
            if (File.Exists(fileName))
            {
                try
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(fileName);

                    XmlNodeList dictNodeList = xmlDoc.DocumentElement.SelectNodes("Dictionary");
                    foreach (XmlElement dictElem in dictNodeList)
                    {
                        Dict dict;
                        string dictKey = dictElem.GetAttribute("key");

                        if (!Dictionaries.TryGetValue(dictKey, out dict))
                        {
                            dict = new Dict(dictKey);
                            Dictionaries.Add(dictKey, dict);
                        }

                        XmlNodeList phraseNodeList = dictElem.SelectNodes("Phrase");
                        foreach (XmlElement phraseElem in phraseNodeList)
                        {
                            string phraseKey = phraseElem.GetAttribute("key");
                            dict.Phrases[phraseKey] = phraseElem.InnerText;
                        }
                    }

                    errMsg = "";
                    return true;
                }
                catch (Exception ex)
                {
                    errMsg = (UseRussian ? "Ошибка при загрузке словарей: " : 
                        "Error loading dictionaries: ") + ex.Message;
                    return false;
                }
            }
            else
            {
                errMsg = (UseRussian ? "Не найден файл словарей " : "File with dictionaries not found ") + fileName;
                return false;
            }
        }

        /// <summary>
        /// Обновить словарь, если он изменился
        /// </summary>
        public static bool RefreshDictionary(string directory, string fileNamePrefix, ref DateTime fileAge, 
            out bool updated, out string errMsg)
        {
            string fileName = Localization.GetDictionaryFileName(directory, fileNamePrefix);
            DateTime newFileAge = ScadaUtils.GetLastWriteTime(fileName);

            if (fileAge == newFileAge)
            {
                updated = false;
                errMsg = "";
                return true;
            }
            else if (Localization.LoadDictionaries(fileName, out errMsg))
            {
                fileAge = newFileAge;
                updated = true;
                return true;
            }
            else
            {
                updated = false;
                return false;
            }
        }

        /// <summary>
        /// Получить имя файла словаря
        /// </summary>
        public static string GetDictionaryFileName(string directory, string fileNamePrefix)
        {
            return directory + fileNamePrefix +
                (string.IsNullOrEmpty(Culture.Name) ? "" : "." + Culture.Name) + ".xml";
        }


        /// <summary>
        /// Перевести форму, используя заданный словарь
        /// </summary>
        public static void TranslateForm(WinForms.Form form, string dictName, 
            WinForms.ToolTip toolTip = null, params WinForms.ContextMenuStrip[] contextMenus)
        {
            Dict dict;
            if (form != null && Dictionaries.TryGetValue(dictName, out dict))
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

            Dict dict;
            if (page != null && Dictionaries.TryGetValue(dictName, out dict))
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

        /// <summary>
        /// Преобразовать дату и время в строку с использованием информации о культуре
        /// </summary>
        public static string ToLocalizedString(this DateTime dateTime)
        {
            return dateTime.ToString("d", Localization.Culture) + " " + dateTime.ToString("T", Localization.Culture);
        }
    }
}