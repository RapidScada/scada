/*
 * Copyright 2020 Mikhail Shiryaev
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
 * Summary  : SCADA-Server connection settings
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2005
 * Modified : 2020
 */

using System;
using System.IO;
using System.Xml;
using Utils;

namespace Scada.Client
{
    /// <summary>
    /// SCADA-Server connection settings.
    /// <para>Настройки соединения со SCADA-Сервером.</para>
    /// </summary>
    [Serializable]
    public class CommSettings : ISettings
	{
        /// <summary>
        /// Формат сохранения и загрузки из XML.
        /// </summary>
        public enum XmlFormat
        {
            /// <summary>
            /// Retrieve properties from XML attributes.
            /// </summary>
            Attr,
            /// <summary>
            /// Retrieve properties from XML elements.
            /// </summary>
            Elem
        }

        /// <summary>
        /// Имя файла настроек по умолчанию.
        /// </summary>
        public const string DefFileName = "CommSettings.xml";


        /// <summary>
        /// Конструктор.
        /// </summary>
		public CommSettings()
		{
            SetToDefault();
            Format = XmlFormat.Attr;
        }

        /// <summary>
        /// Конструктор с установкой параметров связи.
        /// </summary>
        public CommSettings(string serverHost, int serverPort, string serverUser, string serverPwd, 
            int serverTimeout)
        {
            ServerHost = serverHost;
            ServerPort = serverPort;
            ServerUser = serverUser;
            ServerPwd = serverPwd;
            ServerTimeout = serverTimeout;
            Format = XmlFormat.Attr;
        }


        /// <summary>
        /// Получить или установить имя компьютера или IP-адрес SCADA-Сервера.
        /// </summary>
        public string ServerHost { get; set; }

        /// <summary>
        /// Получить или установить номер TCP-порта SCADA-Сервера.
        /// </summary>
        public int ServerPort { get; set; }

        /// <summary>
        /// Получить или установить имя пользователя для подключения к SCADA-Серверу.
        /// </summary>
        public string ServerUser { get; set; }

        /// <summary>
        /// Получить или установить пароль пользователя для подключения к SCADA-Серверу.
        /// </summary>
        public string ServerPwd { get; set; }

        /// <summary>
        /// Получить или установить таймаут ожидания ответа SCADA-Сервера, мс.
        /// </summary>
        public int ServerTimeout { get; set; }

        /// <summary>
        /// Получить или установить формат сохранения и загрузки из XML.
        /// </summary>
        public XmlFormat Format { get; set; }


        /// <summary>
        /// Создать новый объект настроек.
        /// </summary>
        public ISettings Create()
        {
            return new CommSettings();
        }

        /// <summary>
        /// Определить, равны ли заданные настройки текущим настройкам.
        /// </summary>
        public bool Equals(ISettings settings)
        {
            return Equals(settings as CommSettings);
        }

        /// <summary>
        /// Определить, равны ли заданные настройки текущим настройкам.
        /// </summary>
        public bool Equals(CommSettings commSettings)
        {
            return commSettings == null ? false :
                commSettings == this ? true :
                ServerHost == commSettings.ServerHost && ServerPort == commSettings.ServerPort &&
                ServerUser == commSettings.ServerUser && ServerPwd == commSettings.ServerPwd &&
                ServerTimeout == commSettings.ServerTimeout;
        }

        /// <summary>
        /// Установить значения настроек по умолчанию.
        /// </summary>
        public void SetToDefault()
        {
            ServerHost = "localhost";
            ServerPort = 10000;
            ServerUser = "";
            ServerPwd = "12345";
            ServerTimeout = 10000;
        }

        /// <summary>
        /// Создать копию настроек.
        /// </summary>
        public CommSettings Clone()
        {
            return new CommSettings(ServerHost, ServerPort, ServerUser, ServerPwd, ServerTimeout);
        }

        /// <summary>
        /// Загрузить настройки из заданного XML-узла.
        /// </summary>
        public void LoadFromXml(XmlNode xmlNode)
        {
            if (xmlNode == null)
                throw new ArgumentNullException("xmlNode");

            if (Format == XmlFormat.Attr)
            {
                XmlNodeList xmlNodeList = xmlNode.SelectNodes("Param");

                foreach (XmlElement xmlElement in xmlNodeList)
                {
                    string name = xmlElement.GetAttribute("name").Trim();
                    string nameL = name.ToLowerInvariant();
                    string val = xmlElement.GetAttribute("value");

                    try
                    {
                        if (nameL == "serverhost")
                            ServerHost = val;
                        else if (nameL == "serverport")
                            ServerPort = int.Parse(val);
                        else if (nameL == "serveruser")
                            ServerUser = val;
                        else if (nameL == "serverpwd")
                            ServerPwd = val;
                        else if (nameL == "servertimeout")
                            ServerTimeout = int.Parse(val);
                    }
                    catch
                    {
                        throw new ScadaException(string.Format(CommonPhrases.IncorrectXmlParamVal, name));
                    }
                }
            }
            else
            {
                ServerHost = xmlNode.GetChildAsString("Host");
                ServerPort = xmlNode.GetChildAsInt("Port");
                ServerUser = xmlNode.GetChildAsString("User");
                ServerPwd = ScadaUtils.Decrypt(xmlNode.GetChildAsString("Password"));
                ServerTimeout = xmlNode.GetChildAsInt("Timeout");
            }
        }

        /// <summary>
        /// Загрузить настройки из файла.
        /// </summary>
        public bool LoadFromFile(string fileName, out string errMsg)
        {
            // установка значений по умолчанию
            SetToDefault();

            // загрузка настроек соединения со SCADA-Сервером
            try
            {
                if (!File.Exists(fileName))
                    throw new FileNotFoundException(string.Format(CommonPhrases.NamedFileNotFound, fileName));

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(fileName);
                LoadFromXml(xmlDoc.DocumentElement);

                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = CommonPhrases.LoadCommSettingsError + ": " + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Сохранить настройки в заданный XML-элемент.
        /// </summary>
        public void SaveToXml(XmlElement xmlElem)
        {
            if (xmlElem == null)
                throw new ArgumentNullException("xmlElem");

            if (Format == XmlFormat.Attr)
            {
                xmlElem.AppendParamElem("ServerHost", ServerHost,
                    "Имя компьютера или IP-адрес SCADA-Сервера", "SCADA-Server host or IP address");
                xmlElem.AppendParamElem("ServerPort", ServerPort,
                    "Номер TCP-порта SCADA-Сервера", "SCADA-Server TCP port number");
                xmlElem.AppendParamElem("ServerUser", ServerUser,
                    "Имя пользователя для подключения", "User name for the connection");
                xmlElem.AppendParamElem("ServerPwd", ServerPwd,
                    "Пароль пользователя для подключения", "User password for the connection");
                xmlElem.AppendParamElem("ServerTimeout", ServerTimeout,
                    "Таймаут ожидания ответа, мс", "Response timeout, ms");
            }
            else
            {
                xmlElem.AppendElem("Host", ServerHost);
                xmlElem.AppendElem("Port", ServerPort);
                xmlElem.AppendElem("User", ServerUser);
                xmlElem.AppendElem("Password", ScadaUtils.Encrypt(ServerPwd));
                xmlElem.AppendElem("Timeout", ServerTimeout);
            }
        }

        /// <summary>
        /// Сохранить настройки в файле.
        /// </summary>
        public bool SaveToFile(string fileName, out string errMsg)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();

                XmlDeclaration xmlDecl = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
                xmlDoc.AppendChild(xmlDecl);

                XmlElement rootElem = xmlDoc.CreateElement("CommSettings");
                xmlDoc.AppendChild(rootElem);
                SaveToXml(rootElem);

                xmlDoc.Save(fileName);
                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = CommonPhrases.SaveCommSettingsError + ":\n" + ex.Message;
                return false;
            }
        }
    }
}