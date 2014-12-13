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
 * Module   : ScadaWebCommon
 * Summary  : View settings
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2011
 * Modified : 2012
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Scada.Client;

namespace Scada.Web
{
    /// <summary>
    /// View settings
    /// <para>Настройки представлений</para>
    /// </summary>
    public class ViewSettings
    {
        /// <summary>
        /// Имя файла настроек представлений по умолчанию
        /// </summary>
        public const string DefFileName = "ViewSettings.xml";


        /// <summary>
        /// Информация о представлении
        /// </summary>
        public class ViewInfo
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            public ViewInfo()
            {
                Title = "";
                Type = "";
                FileName = "";
                ViewCash = null;
            }
            /// <summary>
            /// Конструктор
            /// </summary>
            public ViewInfo(string title, string type, string fileName)
            {
                Title = title;
                Type = type;
                FileName = fileName;
                ViewCash = null;
            }

            /// <summary>
            /// Получить или установить заголовок
            /// </summary>
            public string Title { get; set; }
            /// <summary>
            /// Получить или установить тип
            /// </summary>
            public string Type { get; set; }
            /// <summary>
            /// Получить или установить имя файла
            /// </summary>
            public string FileName { get; set; }
            /// <summary>
            /// Получить или установить ссылку на представление для кэширования
            /// </summary>
            public BaseView ViewCash { get; set; }

            /// <summary>
            /// Создать копию информации о представлении
            /// </summary>
            public ViewInfo Clone()
            {
                ViewInfo view = new ViewInfo(Title, Type, FileName);
                view.ViewCash = ViewCash;
                return view;
            }
        }

        /// <summary>
        /// Набор представлений
        /// </summary>
        public class ViewSet
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            public ViewSet()
            {
                Name = "";
                Directory = "";
                Items = new List<ViewInfo>();
            }
            /// <summary>
            /// Конструктор
            /// </summary>
            public ViewSet(string name, string directory)
            {
                Name = name;
                Directory = directory;
                Items = new List<ViewInfo>();
            }

            /// <summary>
            /// Получить или установить наименование
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// Получить или установить директорию набора представлений 
            /// на сервере относительно директории интерфейса
            /// </summary>
            public string Directory { get; set; }
            /// <summary>
            /// Получить список представлений набора
            /// </summary>
            public List<ViewInfo> Items { get; private set; }
            /// <summary>
            /// Получить представление из набора по индексу
            /// </summary>
            public ViewInfo this[int index]
            {
                get
                {
                    return Items[index];
                }
            }
            /// <summary>
            /// Получить количество представлений в наборе
            /// </summary>
            public int Count
            {
                get
                {
                    return Items.Count;
                }
            }

            /// <summary>
            /// Создать копию набора представлений
            /// </summary>
            public ViewSet Clone()
            {
                ViewSet viewSet = new ViewSet(Name, Directory);
                foreach (ViewInfo viewInfo in Items)
                    viewSet.Items.Add(viewInfo.Clone());
                return viewSet;
            }
        }


        /// <summary>
        /// Конструктор
        /// </summary>
        public ViewSettings()
        {
            ViewSetList = new List<ViewSet>();
        }


        /// <summary>
        /// Получить список наборов представлений
        /// </summary>
        public List<ViewSet> ViewSetList { get; private set; }


        /// <summary>
        /// Создать копию настроек представлений
        /// </summary>
        public ViewSettings Clone()
        {
            ViewSettings viewSettings = new ViewSettings();
            foreach (ViewSet viewSet in ViewSetList)
                viewSettings.ViewSetList.Add(viewSet.Clone());
            return viewSettings;
        }

        /// <summary>
        /// Загрузить настройки представлений из файла
        /// </summary>
        public bool LoadFromFile(string fileName, out string errMsg)
        {
            // установка значений по умолчанию
            ViewSetList = new List<ViewSet>();

            try
            {
                if (!File.Exists(fileName))
                    throw new FileNotFoundException(string.Format(CommonPhrases.NamedFileNotFound, fileName));

                // загрузка настроек
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(fileName);

                XmlNodeList viewSetNodeList = xmlDoc.DocumentElement.SelectNodes("ViewSet");

                foreach (XmlElement viewSetElement in viewSetNodeList)
                {
                    ViewSet viewSet = new ViewSet(viewSetElement.GetAttribute("name"),
                        ScadaUtils.NormalDir(viewSetElement.GetAttribute("directory")));
                    ViewSetList.Add(viewSet);

                    XmlNodeList viewNodeList = viewSetElement.SelectNodes("View");

                    foreach (XmlElement viewElement in viewNodeList)
                    {
                        ViewInfo viewInfo = new ViewInfo(viewElement.InnerText,
                            viewElement.GetAttribute("type"), viewElement.GetAttribute("fileName"));
                        viewSet.Items.Add(viewInfo);
                    }
                }

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
        /// Сохранить настройки представлений в файле
        /// </summary>
        public bool SaveToFile(string fileName, out string errMsg)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();

                XmlDeclaration xmlDecl = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
                xmlDoc.AppendChild(xmlDecl);

                XmlElement rootElem = xmlDoc.CreateElement("ViewSettings");
                xmlDoc.AppendChild(rootElem);

                foreach (ViewSet viewSet in ViewSetList)
                {
                    XmlElement viewSetElem = xmlDoc.CreateElement("ViewSet");
                    viewSetElem.SetAttribute("name", viewSet.Name);
                    viewSetElem.SetAttribute("directory", viewSet.Directory);
                    rootElem.AppendChild(viewSetElem);

                    foreach (ViewInfo viewInfo in viewSet.Items)
                    {
                        XmlElement viewElem = xmlDoc.CreateElement("View");
                        viewElem.SetAttribute("type", viewInfo.Type);
                        viewElem.SetAttribute("fileName", viewInfo.FileName);
                        viewElem.InnerText = viewInfo.Title;
                        viewSetElem.AppendChild(viewElem);
                    }
                }

                xmlDoc.Save(fileName);
                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = WebPhrases.SaveViewSettingsError + ":\n" + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Очистить кэш представлений
        /// </summary>
        public void ClearViewCash()
        {
            foreach (ViewSet viewSet in ViewSetList)
                foreach (ViewInfo viewInfo in viewSet.Items)
                    viewInfo.ViewCash = null;
        }
    }
}