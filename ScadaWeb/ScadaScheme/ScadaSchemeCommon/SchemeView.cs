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
        /// Конструктор
        /// </summary>
        public SchemeView()
            : base()
        {
            SchemeDoc = new SchemeDocument();
            Components = new List<BaseComponent>();
        }


        /// <summary>
        /// Получить свойства документа схемы
        /// </summary>
        public SchemeDocument SchemeDoc { get; protected set; }

        /// <summary>
        /// Получить компоненты схемы
        /// </summary>
        public List<BaseComponent> Components { get; protected set; }


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

            // загрузка параметров схемы
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
            }

            // загрузка компонентов схемы
            XmlNode componentsNode = rootElem.SelectSingleNode("Components") ?? rootElem.SelectSingleNode("Elements");
            if (componentsNode != null)
            {
                foreach (XmlNode componentNode in componentsNode.ChildNodes)
                {
                    string nodeName = componentNode.Name.ToLowerInvariant();
                    BaseComponent component = null;

                    if (nodeName == "statictext")
                        component = new StaticText();
                    else if (nodeName == "dynamictext")
                        component = new DynamicText();
                    else if (nodeName == "staticpicture")
                        component = new StaticPicture();
                    else if (nodeName == "dynamicpicture")
                        component = new DynamicPicture();

                    if (component != null)
                    {
                        component.SchemeDoc = SchemeDoc;
                        component.LoadFromXml(componentNode);
                        Components.Add(component);
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
            SchemeDoc.SetToDefault();
            Components.Clear();
        }
    }
}
