/*
 * Copyright 2018 Mikhail Shiryaev
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
 * Module   : ScadaAgentEngine
 * Summary  : Agent settings
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Scada.Agent.Engine
{
    /// <summary>
    /// Agent settings
    /// <para>Настройки агента</para>
    /// </summary>
    public class Settings
    {
        /// <summary>
        /// Имя файла настроек по умолчанию
        /// </summary>
        public const string DefFileName = "ScadaAgentConfig.xml";


        /// <summary>
        /// Конструктор
        /// </summary>
        public Settings()
        {
            SecretKey = null;
            Instances = new SortedList<string, ScadaInstanceSettings>();
        }


        /// <summary>
        /// Получить секретный ключ для шифрования паролей
        /// </summary>
        public byte[] SecretKey { get; private set; }

        /// <summary>
        /// Получить настройки экземпляров систем, ключ - наименование экземпляра
        /// </summary>
        public SortedList<string, ScadaInstanceSettings> Instances { get; private set; }


        /// <summary>
        /// Установить значения настроек по умолчанию
        /// </summary>
        private void SetToDefault()
        {
            SecretKey = null;
            Instances.Clear();
        }


        /// <summary>
        /// Загрузить настройки из файла
        /// </summary>
        public bool Load(string fileName, out string errMsg)
        {
            // установка значений по умолчанию
            SetToDefault();

            try
            {
                if (!File.Exists(fileName))
                    throw new FileNotFoundException(string.Format(CommonPhrases.NamedFileNotFound, fileName));

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(fileName);
                XmlElement rootElem = xmlDoc.DocumentElement;

                // загрузка секретного ключа
                if (ScadaUtils.HexToBytes(rootElem.GetChildAsString("SecretKey"), out byte[] secretKey) &&
                    secretKey.Length == ScadaUtils.SecretKeySize)
                {
                    SecretKey = secretKey;
                }
                else
                {
                    throw new ScadaException(string.Format(CommonPhrases.IncorrectXmlNodeVal, "SecretKey"));
                }

                // загрузка настроек экземпляров систем
                XmlNode instancesNode = rootElem.SelectSingleNode("Instances");
                if (instancesNode != null)
                {
                    XmlNodeList instanceNodeList = instancesNode.SelectNodes("Instance");
                    foreach (XmlElement instanceElem in instanceNodeList)
                    {
                        ScadaInstanceSettings instanceSettings = new ScadaInstanceSettings()
                        {
                            Name = instanceElem.GetAttribute("name"),
                            Directory = ScadaUtils.NormalDir(instanceElem.GetAttribute("directory"))
                        };

                        Instances[instanceSettings.Name] = instanceSettings;
                    }
                }

                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = CommonPhrases.LoadAppSettingsError + ":" + Environment.NewLine + ex.Message;
                return false;
            }
        }
    }
}
