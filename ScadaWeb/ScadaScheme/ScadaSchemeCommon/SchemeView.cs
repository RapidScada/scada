/*
 * Copyright 2017 Mikhail Shiryaev
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
 * Summary  : Scheme view
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2017
 */

using Scada.Client;
using Scada.Scheme.Model;
using Scada.Scheme.Model.DataTypes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Scada.Scheme
{
    /// <summary>
    /// Scheme view
    /// <para>Представление схемы</para>
    /// </summary>
    public class SchemeView : BaseView
    {
        /// <summary>
        /// Максимальный идентификатор компонентов схемы
        /// </summary>
        protected int maxComponentID;

        
        /// <summary>
        /// Конструктор
        /// </summary>
        public SchemeView()
            : base()
        {
            maxComponentID = 0;
            SchemeDoc = new SchemeDocument();
            Components = new SortedList<int, BaseComponent>();
            LoadErrors = new List<string>();
        }


        /// <summary>
        /// Получить свойства документа схемы
        /// </summary>
        public SchemeDocument SchemeDoc { get; protected set; }

        /// <summary>
        /// Получить компоненты схемы, ключ - идентификатор компонента
        /// </summary>
        public SortedList<int, BaseComponent> Components { get; protected set; }

        /// <summary>
        /// Получить ошибки при загрузке схемы
        /// </summary>
        /// <remarks>Необходимо для контроля загрузки библиотек и компонентов</remarks>
        public List<string> LoadErrors { get; protected set; }


        /// <summary>
        /// Загрузить представление из потока
        /// </summary>
        public override void LoadFromStream(Stream stream)
        {
            // очистка представления
            Clear();

            // загрузка XML-документа
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(stream);

            // проверка формата файла (потока)
            XmlElement rootElem = xmlDoc.DocumentElement;
            if (!rootElem.Name.Equals("SchemeView", StringComparison.OrdinalIgnoreCase))
                throw new ScadaException(SchemePhrases.IncorrectFileFormat);

            // загрузка документа схемы
            XmlNode documentNode = rootElem.SelectSingleNode("Document") ?? rootElem.SelectSingleNode("Scheme");
            if (documentNode != null)
            {
                SchemeDoc.LoadFromXml(documentNode);

                // загрузка заголовка схемы для старого формата
                if (SchemeDoc.Title == "")
                    SchemeDoc.Title = rootElem.GetAttribute("title");

                // установка заголовка представления
                Title = SchemeDoc.Title;

                // загрузка фильтра по входным каналам для старого формата
                XmlNode cnlsFilterNode = rootElem.SelectSingleNode("CnlsFilter");
                if (cnlsFilterNode != null)
                    SchemeDoc.CnlFilter.ParseCnlFilter(cnlsFilterNode.InnerText);

                // добавление входных каналов представления
                foreach (int cnlNum in SchemeDoc.CnlFilter)
                    AddCnlNum(cnlNum);
            }

            // загрузка компонентов схемы
            XmlNode componentsNode = rootElem.SelectSingleNode("Components") ?? rootElem.SelectSingleNode("Elements");
            if (componentsNode != null)
            {
                HashSet<string> errNodeNames = new HashSet<string>(); // имена узлов незагруженных компонентов
                CompManager compManager = CompManager.GetInstance();
                LoadErrors.AddRange(compManager.LoadErrors);

                foreach (XmlNode compNode in componentsNode.ChildNodes)
                {
                    // создание компонента
                    string errMsg;
                    BaseComponent component = compManager.CreateComponent(compNode, out errMsg);

                    if (component == null)
                    {
                        if (errNodeNames.Add(compNode.Name))
                            LoadErrors.Add(errMsg);
                    }
                    else
                    {
                        // загрузка компонента и добавление его в представление
                        component.SchemeDoc = SchemeDoc;
                        component.LoadFromXml(compNode);
                        Components[component.ID] = component;

                        // добавление входных каналов представления
                        if (component is IDynamicComponent)
                        {
                            IDynamicComponent dynamicComponent = (IDynamicComponent)component;
                            AddCnlNum(dynamicComponent.InCnlNum);
                            AddCtrlCnlNum(dynamicComponent.CtrlCnlNum);
                        }

                        // определение макс. идентификатора компонентов
                        if (component.ID > maxComponentID)
                            maxComponentID = component.ID;
                    }
                }
            }

            // загрузка изображений схемы
            XmlNode imagesNode = rootElem.SelectSingleNode("Images");
            if (imagesNode != null)
            {
                Dictionary<string, Image> images = SchemeDoc.Images;
                XmlNodeList imageNodes = imagesNode.SelectNodes("Image");
                foreach (XmlNode imageNode in imageNodes)
                {
                    Image image = new Image();
                    image.LoadFromXml(imageNode);
                    if (!string.IsNullOrEmpty(image.Name))
                        images[image.Name] = image;
                }
            }
        }

        /// <summary>
        /// Загрузить схему из файла
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
                errMsg = SchemePhrases.LoadSchemeViewError + ": " + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Сохранить схему в файле
        /// </summary>
        public bool SaveToFile(string fileName, out string errMsg)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                XmlDeclaration xmlDecl = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
                xmlDoc.AppendChild(xmlDecl);

                // запись заголовка представления
                XmlElement rootElem = xmlDoc.CreateElement("SchemeView");
                rootElem.SetAttribute("title", SchemeDoc.Title);
                xmlDoc.AppendChild(rootElem);

                // пока используется старый формат файла
                // запись документа схемы
                XmlElement documentElem = xmlDoc.CreateElement("Scheme");
                rootElem.AppendChild(documentElem);
                SchemeDoc.SaveToXml(documentElem);

                // запись компонентов схемы
                CompManager compManager = CompManager.GetInstance();
                HashSet<string> prefixes = new HashSet<string>();
                XmlElement componentsElem = xmlDoc.CreateElement("Components");
                rootElem.AppendChild(componentsElem);

                foreach (BaseComponent component in Components.Values)
                {
                    Type compType = component.GetType();
                    CompLibSpec compLibSpec = compManager.GetSpecByType(compType);

                    // добавление пространства имён
                    if (compLibSpec != null && !prefixes.Contains(compLibSpec.XmlPrefix))
                    {
                        rootElem.SetAttribute("xmlns:" + compLibSpec.XmlPrefix, compLibSpec.XmlNs);
                        prefixes.Add(compLibSpec.XmlPrefix);
                    }

                    // создание компонента
                    XmlElement componentElem = compLibSpec == null ?
                        xmlDoc.CreateElement(compType.Name) /*стандартный компонент*/ :
                        xmlDoc.CreateElement(compLibSpec.XmlPrefix, compType.Name, compLibSpec.XmlNs);

                    componentsElem.AppendChild(componentElem);
                    component.SaveToXml(componentElem);
                }

                // запись изображений схемы
                XmlElement imagesElem = xmlDoc.CreateElement("Images");
                rootElem.AppendChild(imagesElem);

                foreach (Image image in SchemeDoc.Images.Values)
                {
                    XmlElement imageElem = xmlDoc.CreateElement("Image");
                    imagesElem.AppendChild(imageElem);
                    image.SaveToXml(imageElem);
                }

                // запись фильтра по входным каналам
                XmlElement cnlsFilterElem = xmlDoc.CreateElement("CnlsFilter");
                cnlsFilterElem.InnerText = SchemeDoc.CnlFilter.CnlFilterToString();
                rootElem.AppendChild(cnlsFilterElem);

                xmlDoc.Save(fileName);
                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = SchemePhrases.SaveSchemeViewError + ": " + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Очистить представление
        /// </summary>
        public override void Clear()
        {
            base.Clear();
            maxComponentID = 0;
            SchemeDoc.SetToDefault();
            Components.Clear();
            LoadErrors.Clear();
        }

        /// <summary>
        /// Получить следующий идентификатор компонента схемы
        /// </summary>
        public int GetNextComponentID()
        {
            return ++maxComponentID;
        }
    }
}
