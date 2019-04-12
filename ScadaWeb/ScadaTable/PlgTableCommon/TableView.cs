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
 * Module   : PlgTableCommon
 * Summary  : Table view
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2011
 * Modified : 2019
 */

using Scada.Client;
using Scada.Data.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace Scada.Table
{
    /// <summary>
    /// Table view.
    /// <para>Табличное представление.</para>
    /// </summary>
    public class TableView : BaseView
    {
        /// <summary>
        /// Элемент представления
        /// </summary>
        public class Item
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            public Item()
                : this(0, 0, false, "")
            {
            }
            /// <summary>
            /// Конструктор
            /// </summary>
            public Item(int cnlNum, int ctrlCnlNum, bool hidden, string caption)
            {
                CnlNum = cnlNum;
                CtrlCnlNum = ctrlCnlNum;
                Hidden = hidden;
                Caption = caption;
                CnlProps = null;
                CtrlCnlProps = null;
            }

            /// <summary>
            /// Получить или установить номер входного канала
            /// </summary>
            public int CnlNum { get; set; }            
            /// <summary>
            /// Получить или установить номер канала управления
            /// </summary>
            public int CtrlCnlNum { get; set; }
            /// <summary>
            /// Получить или установить признак, что элемент является скрытым
            /// </summary>
            public bool Hidden { get; set; }
            /// <summary>
            /// Получить или установить обозначение элемента
            /// </summary>
            public string Caption { get; set; }
            /// <summary>
            /// Получить или установить свойства входного канала элемента
            /// </summary>
            public InCnlProps CnlProps { get; set; }
            /// <summary>
            /// Получить или установить свойства канала управления элемента
            /// </summary>
            public CtrlCnlProps CtrlCnlProps { get; set; }
        }


        /// <summary>
        /// Конструктор
        /// </summary>
        public TableView()
            : base()
        {
            Items = new List<Item>();
            VisibleItems = new List<Item>();
        }


        /// <summary>
        /// Получить список элементов представления
        /// </summary>
        public List<Item> Items { get; protected set; }

        /// <summary>
        /// Получить список видимых элементов представления
        /// </summary>
        /// <remarks>Список заполняется при загрузке таблицы</remarks>
        public List<Item> VisibleItems { get; protected set; }

        /// <summary>
        /// Получить количество элементов представления
        /// </summary>
        public int Count
        {
            get
            {
                return Items.Count;
            }
        }

        /// <summary>
        /// Получить количество видимых элементов представления
        /// </summary>
        public int VisibleCount
        {
            get
            {
                return VisibleItems.Count;
            }
        }


        /// <summary>
        /// Добавить элемент в списки элементов
        /// </summary>
        private void AddItem(Item item)
        {
            Items.Add(item);
            if (!item.Hidden)
                VisibleItems.Add(item);
        }

        /// <summary>
        /// Загрузить представление из потока
        /// </summary>
        public override void LoadFromStream(Stream stream)
        {
            // очистка представления
            Clear();

            // загрузка представления в новом формате
            XmlDocument xmlDoc;
            bool xmlDocLoaded; // XML-документ успешно загружен

            try
            {
                xmlDoc = new XmlDocument();
                xmlDoc.Load(stream);
                xmlDocLoaded = true;
            }
            catch
            {
                xmlDoc = null;
                xmlDocLoaded = false;
                stream.Seek(0, SeekOrigin.Begin);
            }

            if (xmlDocLoaded)
            {
                Title = xmlDoc.DocumentElement.GetAttribute("title");
                XmlNodeList paramNodeList = xmlDoc.DocumentElement.SelectNodes("Param");

                foreach (XmlElement paramElem in paramNodeList)
                {
                    int cnlNum;
                    int ctrlCnlNum;
                    bool hidden;

                    int.TryParse(paramElem.GetAttribute("cnlNum"), out cnlNum);
                    int.TryParse(paramElem.GetAttribute("ctrlCnlNum"), out ctrlCnlNum);
                    bool.TryParse(paramElem.GetAttribute("hidden"), out hidden);

                    Item item = new Item(cnlNum, ctrlCnlNum, hidden, paramElem.InnerText);
                    AddItem(item);
                    AddCnlNum(cnlNum);
                    AddCtrlCnlNum(ctrlCnlNum);
                }
            }
            else
            {
                // загрузка представления в старом формате
                const int IdentLen = 10; // длина идентификатора в файле таблицы
                StreamReader reader = null;

                try
                {
                    reader = new StreamReader(stream, Encoding.Default);

                    // считывание заголовка таблицы
                    string titLn = reader.ReadLine();
                    if (titLn != null && titLn.Length > IdentLen)
                        Title = titLn.Substring(IdentLen);

                    // считывание элементов
                    string cnlLn;
                    string capLn;
                    do
                    {
                        cnlLn = reader.ReadLine();
                        capLn = reader.ReadLine();
                        if (cnlLn != null && capLn != null)
                        {
                            int cnlNum = 0;
                            if (cnlLn.Length > IdentLen)
                            {
                                try { cnlNum = int.Parse(cnlLn.Substring(IdentLen)); }
                                catch { }
                            }

                            string caption = capLn.Length > IdentLen ? capLn.Substring(IdentLen) : "";

                            AddItem(new Item(cnlNum, 0, false, caption));
                            AddCnlNum(cnlNum);
                        }
                    } while (cnlLn != null && capLn != null);
                }
                finally
                {
                    if (reader != null)
                        reader.Close();
                }
            }
        }

        /// <summary>
        /// Загрузить представление из файла
        /// </summary>
        public bool LoadFromFile(string fileName, out string errMsg)
        {
            try
            {
                using (FileStream fileStream = 
                    new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    LoadFromStream(fileStream);
                }

                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = TablePhrases.LoadTableViewError + ": " + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Сохранить представление в файле
        /// </summary>
        public bool SaveToFile(string fileName, out string errMsg)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                XmlDeclaration xmlDecl = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
                xmlDoc.AppendChild(xmlDecl);

                XmlElement rootElem = xmlDoc.CreateElement("TableView");
                rootElem.SetAttribute("title", Title);
                xmlDoc.AppendChild(rootElem);

                foreach (Item item in Items)
                {
                    XmlElement paramElem = xmlDoc.CreateElement("Param");
                    paramElem.InnerText = item.Caption;
                    if (item.CnlNum > 0)
                        paramElem.SetAttribute("cnlNum", item.CnlNum.ToString());
                    if (item.CtrlCnlNum > 0)
                        paramElem.SetAttribute("ctrlCnlNum", item.CtrlCnlNum.ToString());
                    if (item.Hidden)
                        paramElem.SetAttribute("hidden", "true");
                    paramElem.IsEmpty = string.IsNullOrEmpty(item.Caption) && 
                        item.CnlNum <= 0 && item.CtrlCnlNum <= 0 && !item.Hidden;
                    rootElem.AppendChild(paramElem);
                }

                xmlDoc.Save(fileName);
                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = TablePhrases.SaveTableViewError + ": " + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Привязать свойства входных каналов к элементам представления
        /// </summary>
        public override void BindCnlProps(InCnlProps[] cnlPropsArr)
        {
            base.BindCnlProps(cnlPropsArr);

            if (cnlPropsArr != null)
            {
                foreach (Item item in Items)
                {
                    int ind = item.CnlNum > 0 ? 
                        Array.BinarySearch(cnlPropsArr, item.CnlNum, InCnlProps.IntComp) : -1;

                    if (ind >= 0)
                    {
                        InCnlProps cnlProps = cnlPropsArr[ind];
                        item.CnlProps = cnlProps;
                        if (cnlProps.CtrlCnlNum > 0)
                        {
                            item.CtrlCnlNum = cnlProps.CtrlCnlNum;
                            AddCtrlCnlNum(cnlProps.CtrlCnlNum);
                        }
                    }
                    else
                    {
                        item.CnlProps = null;
                    }
                }
            }
        }

        /// <summary>
        /// Привязать свойства каналов управления к элементам представления
        /// </summary>
        public override void BindCtrlCnlProps(CtrlCnlProps[] ctrlCnlPropsArr)
        {
            base.BindCtrlCnlProps(ctrlCnlPropsArr);

            if (ctrlCnlPropsArr != null)
            {
                foreach (Item item in Items)
                {
                    int ind = item.CtrlCnlNum > 0 ?
                        Array.BinarySearch(ctrlCnlPropsArr, item.CtrlCnlNum, CtrlCnlProps.IntComp) : -1;
                    item.CtrlCnlProps = ind >= 0 ? ctrlCnlPropsArr[ind] : null;
                }
            }
        }

        /// <summary>
        /// Очистить представление
        /// </summary>
        public override void Clear()
        {
            base.Clear();
            Title = "";
            Items.Clear();
            VisibleItems.Clear();
        }
    }
}
