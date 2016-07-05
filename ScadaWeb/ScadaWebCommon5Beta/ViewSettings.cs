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
 * Module   : ScadaWebCommon
 * Summary  : View settings
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2011
 * Modified : 2016
 */

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
        public class ViewItem
        {
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
            /// Получить или установить номер входного канала, информирующего о тревожном состоянии представления
            /// </summary>
            public int AlarmCnlNum { get; set; }
            /// <summary>
            /// Получить дочерние элементы
            /// </summary>
            public List<ViewItem> Subitems { get; protected set; }
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
        private void LoadViewItem(XmlElement viewItemElem, List<ViewItem> viewItems)
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
            try
            {
                throw new NotImplementedException("Method is not implemented.");
            }
            catch (Exception ex)
            {
                errMsg = WebPhrases.SaveViewSettingsError + ": " + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Загрузить настройки из базы конфигурации
        /// </summary>
        public bool LoadFromBase(DataTable tblInterface, out string errMsg)
        {
            errMsg = "Method is not implemented.";
            return false;
        }
    }
}
