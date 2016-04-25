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
 * Module   : ScadaData.Svc
 * Summary  : Windows service properties
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

using System;
using System.IO;
using System.Reflection;
using System.Xml;

namespace Scada.Svc
{
    /// <summary>
    /// Windows service properties
    /// <para>Свойства службы Windows</para>
    /// </summary>
    public class SvcProps
    {
        /// <summary>
        /// Имя файла, содержащего свойства службы
        /// </summary>
        public const string SvcPropsFileName = "svc_config.xml";
        /// <summary>
        /// Сообщение об ошибке, что имя службы пустое
        /// </summary>
        public static readonly string ServiceNameEmptyError = Localization.UseRussian ?
            "Имя службы не должно быть пустым." :
            "Service name must not be empty.";


        /// <summary>
        /// Конструктор
        /// </summary>
        public SvcProps()
            : this("", "")
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public SvcProps(string serviceName, string description)
        {
            ServiceName = serviceName;
            Description = description;
        }


        /// <summary>
        /// Получить или установить имя службы
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// Получить или установить описание
        /// </summary>
        public string Description { get; set; }


        /// <summary>
        /// Загрузить свойства службы
        /// </summary>
        public bool LoadFromFile(string fileName, out string errMsg)
        {
            ServiceName = "";
            Description = "";

            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(fileName);

                XmlNode node = xmlDoc.DocumentElement.SelectSingleNode("ServiceName");
                ServiceName = node == null ? "" : node.InnerText;

                if (string.IsNullOrEmpty(ServiceName))
                    throw new Exception(ServiceNameEmptyError);

                node = xmlDoc.DocumentElement.SelectSingleNode("Description");
                Description = node == null ? "" : node.InnerText;

                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = (Localization.UseRussian ?
                    "Ошибка при загрузке свойств службы: " :
                    "Error loading service properties: ") + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Загрузить свойства службы
        /// </summary>
        public bool LoadFromFile()
        {
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string fileName = path + Path.DirectorySeparatorChar + SvcPropsFileName;

            if (File.Exists(fileName))
            {
                string errMsg;
                if (LoadFromFile(fileName, out errMsg))
                    return true;
                else
                    throw new ScadaException(errMsg);
            }
            else
            {
                return false;
            }
        }
    }
}
