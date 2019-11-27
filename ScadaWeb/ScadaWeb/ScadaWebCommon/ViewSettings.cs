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
 * Module   : ScadaWebCommon
 * Summary  : View settings
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2011
 * Modified : 2019
 */

using Scada.Client;
using Scada.Data.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Xml;

namespace Scada.Web
{
    /// <summary>
    /// View settings
    /// <para>Настройки представлений</para>
    /// </summary>
    public class ViewSettings : ISettings
    {
        /// <summary>
        /// Элемент настроек, соответствующий представлению
        /// </summary>
        public class ViewItem : IComparable<ViewItem>
        {
            /// <summary>
            /// Сравнение элементов по пути
            /// </summary>
            internal class PathComparer : IComparer<ViewItem>
            {
                /// <summary>
                /// Сравнить два объекта
                /// </summary>
                public int Compare(ViewItem x, ViewItem y)
                {
                    return string.Compare(x.PathPart, y.PathPart, StringComparison.OrdinalIgnoreCase);
                }
            }

            /// <summary>
            /// Объект для сравнения элементов по пути
            /// </summary>
            internal static readonly PathComparer PathComp = new PathComparer();

            /// <summary>
            /// Конструктор
            /// </summary>
            public ViewItem()
                : this(0, "", 0)
            {
            }
            /// <summary>
            /// Конструктор
            /// </summary>
            public ViewItem(int viewID, string text, int alarmCnlNum)
            {
                ViewID = viewID;
                Text = text;
                PathPart = "";
                AlarmCnlNum = alarmCnlNum;
                Subitems = new List<ViewItem>();
            }

            /// <summary>
            /// Получить или установить идентификатор представления
            /// </summary>
            public int ViewID { get; set; }
            /// <summary>
            /// Получить или установить текст
            /// </summary>
            public string Text { get; set; }
            /// <summary>
            /// Получить или установить часть пути, соответствующую элементу
            /// </summary>
            /// <remarks>Свойство необходимо для загрузки настроек из базы конфигурации</remarks>
            internal string PathPart { get; set; }
            /// <summary>
            /// Получить или установить номер входного канала, информирующего о тревожном состоянии представления
            /// </summary>
            public int AlarmCnlNum { get; set; }
            /// <summary>
            /// Получить дочерние элементы
            /// </summary>
            public List<ViewItem> Subitems { get; protected set; }

            /// <summary>
            /// Сравнить текущий объект с другим объектом такого же типа
            /// </summary>
            public int CompareTo(ViewItem other)
            {
                int subsWeight1 = Subitems.Count > 0 ? 0 : 1;
                int subsWeight2 = other.Subitems.Count > 0 ? 0 : 1;
                int comp1 = subsWeight1.CompareTo(subsWeight2);

                if (comp1 == 0)
                {
                    int comp2 = ViewID.CompareTo(other.ViewID);
                    return comp2 == 0 ? Text.CompareTo(other.Text) : comp2;
                }
                else
                {
                    return comp1;
                }
            }
        }


        /// <summary>
        /// Имя файла настроек по умолчанию
        /// </summary>
        public const string DefFileName = "ViewSettings.xml";

        
        /// <summary>
        /// Конструктор
        /// </summary>
        public ViewSettings()
        {
            ViewItems = new List<ViewItem>();
        }


        /// <summary>
        /// Получить элементы настроек представлений
        /// </summary>
        public List<ViewItem> ViewItems { get; protected set; }


        /// <summary>
        /// Рекурсивно загрузить элемент настроек представлений
        /// </summary>
        protected void LoadViewItem(XmlElement viewItemElem, List<ViewItem> viewItems)
        {
            ViewItem viewItem = new ViewItem();
            viewItem.ViewID = viewItemElem.GetAttrAsInt("viewID");
            viewItem.Text = viewItemElem.GetAttribute("text");
            viewItem.AlarmCnlNum = viewItemElem.GetAttrAsInt("alarmCnlNum");
            viewItems.Add(viewItem);

            XmlNodeList viewItemNodes = viewItemElem.SelectNodes("ViewItem");
            foreach (XmlElement elem in viewItemNodes)
                LoadViewItem(elem, viewItem.Subitems);
        }

        /// <summary>
        /// Рекурсивно добавить элемент настроек представлений
        /// </summary>
        protected void AppendViewItem(int viewID, string text, 
            string[] pathParts, int pathPartInd, List<ViewItem> viewItems)
        {
            string pathPart = pathParts[pathPartInd];
            ViewItem newViewItem = new ViewItem();
            newViewItem.PathPart = pathPart;
            int viewItemInd = viewItems.BinarySearch(newViewItem, ViewItem.PathComp);

            if (pathPartInd < pathParts.Length - 1) // не последние части пути
            {
                if (viewItemInd >= 0)
                {
                    newViewItem = viewItems[viewItemInd];
                }
                else
                {
                    newViewItem.Text = pathPart;
                    viewItems.Insert(~viewItemInd, newViewItem);
                }

                AppendViewItem(viewID, text, pathParts, pathPartInd + 1, newViewItem.Subitems);
            }
            else // последняя часть пути
            {
                newViewItem.ViewID = Path.GetExtension(pathPart) == null ? 0 : viewID;
                newViewItem.Text = text;

                if (viewItemInd < 0)
                {
                    // добавить элемент, которого ещё не было в списке
                    viewItems.Insert(~viewItemInd, newViewItem);
                }
                else
                {
                    // добавить элемент, путь которого совпадает с уже имеющимся
                    viewItems.Insert(viewItemInd + 1, newViewItem);
                }
            }
        }

        /// <summary>
        /// Рекурсивно сортировать элементы настроек представлений
        /// </summary>
        protected void SortViewItems(List<ViewItem> viewItems)
        {
            foreach (ViewItem viewItem in viewItems)
                SortViewItems(viewItem.Subitems);
            viewItems.Sort();
        }


        /// <summary>
        /// Создать новый объект настроек
        /// </summary>
        public ISettings Create()
        {
            return new ViewSettings();
        }

        /// <summary>
        /// Определить, равны ли заданные настройки текущим настройкам
        /// </summary>
        public bool Equals(ISettings settings)
        {
            return settings == this;
        }

        /// <summary>
        /// Загрузить настройки из файла
        /// </summary>
        public bool LoadFromFile(string fileName, out string errMsg)
        {
            // установка значений по умолчанию
            ViewItems.Clear();

            // загрузка настроек
            try
            {
                if (!File.Exists(fileName))
                    throw new FileNotFoundException(string.Format(CommonPhrases.NamedFileNotFound, fileName));

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(fileName);

                XmlNodeList viewItemNodes = xmlDoc.DocumentElement.SelectNodes("ViewItem");
                foreach (XmlElement viewItemElem in viewItemNodes)
                    LoadViewItem(viewItemElem, ViewItems);

                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = WebPhrases.LoadViewSettingsError + ": " + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Сохранить настройки в файле
        /// </summary>
        public bool SaveToFile(string fileName, out string errMsg)
        {
            errMsg = WebPhrases.SaveViewSettingsError + ": Method is not implemented.";
            return false;
        }

        /// <summary>
        /// Загрузить настройки из базы конфигурации
        /// </summary>
        public bool LoadFromBase(DataAccess dataAccess, out string errMsg)
        {
            // установка значений по умолчанию
            ViewItems.Clear();

            // загрузка настроек
            try
            {
                char[] separator = { '\\', '/' };
                List<UiObjProps> viewPropsList = dataAccess.GetUiObjPropsList(UiObjProps.BaseUiTypes.View);

                foreach (UiObjProps viewProps in viewPropsList)
                {
                    if (!viewProps.IsEmpty)
                    {
                        if (viewProps.PathKind == UiObjProps.PathKinds.File)
                        {
                            // для PathKinds.File свойство Path не равно null
                            string[] pathParts = viewProps.Path.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                            if (pathParts.Length > 0)
                            {
                                string itemText = string.IsNullOrEmpty(viewProps.Title) ?
                                    pathParts[pathParts.Length - 1] : viewProps.Title;
                                AppendViewItem(viewProps.UiObjID, itemText, pathParts, 0, ViewItems);
                            }
                        }
                        else
                        {
                            string itemText = string.IsNullOrEmpty(viewProps.Title) ? 
                                viewProps.Path : viewProps.Title;
                            ViewItem viewItem = new ViewItem(viewProps.UiObjID, itemText, 0);
                            ViewItems.Add(viewItem);
                        }
                    }
                }

                SortViewItems(ViewItems);
                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = WebPhrases.LoadViewSettingsBaseError + ": " + ex.Message;
                return false;
            }
        }
    }
}
