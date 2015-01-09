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
 * Module   : ScadaData
 * Summary  : SCADA-Server connection settings
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2005
 * Modified : 2014
 */

using System;
using System.IO;
using System.Xml;
using Utils;

namespace Scada.Client
{
	/// <summary>
    /// SCADA-Server connection settings
    /// <para>Настройки соединения со SCADA-Сервером</para>
	/// </summary>
	public class CommSettings
	{
        /// <summary>
        /// Имя файла настроек соединения со SCADA-Сервером по умолчанию
        /// </summary>
        public const string DefFileName = "CommSettings.xml";


        /// <summary>
        /// Конструктор
        /// </summary>
		public CommSettings()
		{
            SetToDefault();
        }

        /// <summary>
        /// Конструктор с установкой параметров связи
        /// </summary>
        public CommSettings(string serverHost, int serverPort, string serverUser, string serverPwd, 
            int serverTimeout)
        {
            ServerHost = serverHost;
            ServerPort = serverPort;
            ServerUser = serverUser;
            ServerPwd = serverPwd;
            ServerTimeout = serverTimeout;
        }


        /// <summary>
        /// Получить или установить имя компьютера или IP-адрес SCADA-Сервера
        /// </summary>
        public string ServerHost { get; set; }

        /// <summary>
        /// Получить или установить номер TCP-порта SCADA-Сервера
        /// </summary>
        public int ServerPort { get; set; }

        /// <summary>
        /// Получить или установить имя пользователя для подключения к SCADA-Серверу
        /// </summary>
        public string ServerUser { get; set; }

        /// <summary>
        /// Получить или установить пароль пользователя для подключения к SCADA-Серверу
        /// </summary>
        public string ServerPwd { get; set; }

        /// <summary>
        /// Получить или установить таймаут ожидания ответа SCADA-Сервера, мс
        /// </summary>
        public int ServerTimeout { get; set; }


        /// <summary>
        /// Установить значения настроек по умолчанию
        /// </summary>
        private void SetToDefault()
        {
            ServerHost = "localhost";
            ServerPort = 10000;
            ServerUser = "";
            ServerPwd = "";
            ServerTimeout = 10000;
        }


        /// <summary>
        /// Определить, являются ли заданные параметры связи идентичными текущим параметрам
        /// </summary>
        public bool Equals(CommSettings commSettings)
        {
            return commSettings == null ? false : 
                ServerHost == commSettings.ServerHost && ServerPort == commSettings.ServerPort &&
                ServerUser == commSettings.ServerUser && ServerPwd == commSettings.ServerPwd &&
                ServerTimeout == commSettings.ServerTimeout;
        }

        /// <summary>
        /// Создать копию настроек соединения со SCADA-Сервером
        /// </summary>
        public CommSettings Clone()
        {
            return new CommSettings(ServerHost, ServerPort, ServerUser, ServerPwd, ServerTimeout);
        }

        /// <summary>
        /// Загрузить настройки соединения со SCADA-Сервером из файла
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

                XmlDocument xmlDoc = new XmlDocument(); // обрабатываемый XML-документ
                xmlDoc.Load(fileName);

                XmlNodeList xmlNodeList = xmlDoc.DocumentElement.SelectNodes("Param");
                foreach (XmlElement xmlElement in xmlNodeList)
                {
                    string name = xmlElement.GetAttribute("name").Trim();
                    string nameL = name.ToLower();
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
                        throw new Exception(string.Format(CommonPhrases.IncorrectXmlParamVal, name));
                    }
                }

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
        /// Загрузить настройки соединения со SCADA-Сервером из файла
        /// </summary>
        public void LoadFromFile(string fileName, Log log)
        {
            if (log == null)
                throw new ArgumentNullException("log");

            log.WriteAction(Localization.UseRussian ? "Загрузка настроек соединения с сервером" : 
                "Load server connection settings", Log.ActTypes.Action);
            string errMsg;
            if (!LoadFromFile(fileName, out errMsg))
                log.WriteAction(errMsg, Log.ActTypes.Exception);
        }

        /// <summary>
        /// Сохранить настройки соединения со SCADA-Сервером в файле
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

                rootElem.AppendParamElem("ServerHost", ServerHost,
                    "Имя компьютера или IP-адрес SCADA-Сервера", "SCADA-Server host or IP address");
                rootElem.AppendParamElem("ServerPort", ServerPort,
                    "Номер TCP-порта SCADA-Сервера", "SCADA-Server TCP port number");
                rootElem.AppendParamElem("ServerUser", ServerUser,
                    "Имя пользователя для подключения", "User name for the connection");
                rootElem.AppendParamElem("ServerPwd", ServerPwd,
                    "Пароль пользователя для подключения", "User password for the connection");
                rootElem.AppendParamElem("ServerTimeout", ServerTimeout,
                    "Таймаут ожидания ответа, мс", "Response timeout, ms");

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