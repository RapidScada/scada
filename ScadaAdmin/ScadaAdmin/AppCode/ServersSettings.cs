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
 * Module   : SCADA-Administrator
 * Summary  : Settings of interaction with remote servers
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using Scada;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace ScadaAdmin
{
    /// <summary>
    /// Settings of interaction with remote servers
    /// <para>Настройки взаимодействия с удалёнными серверами</para>
    /// </summary>
    public class ServersSettings
    {
        /// <summary>
        /// Remote server connection settings
        /// <para>Настройки подключения к удалённому серверу</para>
        /// </summary>
        public class ConnectionSettings
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            public ConnectionSettings()
            {
                SetToDefault();
            }

            /// <summary>
            /// Получить или установить наименование
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// Получить или установить имя компьютера или IP-адрес
            /// </summary>
            public string Host { get; set; }
            /// <summary>
            /// Получить или установить TCP-порт
            /// </summary>
            public int Port { get; set; }
            /// <summary>
            /// Получить или установить имя пользователя
            /// </summary>
            public string Username { get; set; }
            /// <summary>
            /// Получить или установить пароль пользователя
            /// </summary>
            public string Password { get; set; }
            /// <summary>
            /// Получить или установить наименования экземпляра системы
            /// </summary>
            public string ScadaInstance { get; set; }
            /// <summary>
            /// Получить или установить секретный ключ
            /// </summary>
            public byte[] SecretKey { get; set; }

            /// <summary>
            /// Установить настройки по умолчанию
            /// </summary>
            private void SetToDefault()
            {
                Name = "";
                Host = "";
                Port = 10002;
                Username = "admin";
                Password = "";
                ScadaInstance = "Default";
                SecretKey = new byte[0];
            }
            /// <summary>
            /// Загрузить настройки из XML-узла
            /// </summary>
            public void LoadFromXml(XmlNode xmlNode)
            {
                if (xmlNode == null)
                    throw new ArgumentNullException("xmlNode");

                Name = xmlNode.GetChildAsString("Name");
                Host = xmlNode.GetChildAsString("Host");
                Port = xmlNode.GetChildAsInt("Port", 10002);
                Username = xmlNode.GetChildAsString("Username", "admin");
                Password = xmlNode.GetChildAsString("Password");
                ScadaInstance = xmlNode.GetChildAsString("ScadaInstance");
                SecretKey = ScadaUtils.HexToBytes(xmlNode.GetChildAsString("SecretKey"));
            }
            /// <summary>
            /// Сохранить настройки в XML-узле
            /// </summary>
            public void SaveToXml(XmlElement xmlElem)
            {
                if (xmlElem == null)
                    throw new ArgumentNullException("xmlElem");

                xmlElem.AppendElem("Name", Name);
                xmlElem.AppendElem("Host", Host);
                xmlElem.AppendElem("Port", Port);
                xmlElem.AppendElem("Username", Username);
                xmlElem.AppendElem("Password", Password);
                xmlElem.AppendElem("ScadaInstance", ScadaInstance);
                xmlElem.AppendElem("SecretKey", ScadaUtils.BytesToHex(SecretKey));
            }
        }

        /// <summary>
        /// Settings of downloading configuration
        /// <para>Настройки скачивания конфигурации</para>
        /// </summary>
        public class DownloadSettings
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            public DownloadSettings()
            {
                SetToDefault();
            }

            /// <summary>
            /// Получить или установить признак сохранения в директорию
            /// </summary>
            public bool SaveToDir { get; set; }
            /// <summary>
            /// Получить или установить директорию для сохранения конфигурации
            /// </summary>
            public string DestDir { get; set; }
            /// <summary>
            /// Получить или установить имя файла архива для сохранения конфигурации
            /// </summary>
            public string DestFile { get; set; }
            /// <summary>
            /// Получить или установить признак запуска импорта базы конфигурации после скачивания
            /// </summary>
            public bool ImportBase { get; set; }

            /// <summary>
            /// Установить настройки по умолчанию
            /// </summary>
            private void SetToDefault()
            {
                SaveToDir = true;
                DestDir = @"C:\SCADA\";
                DestFile = "";
                ImportBase = true;
            }
            /// <summary>
            /// Загрузить настройки из XML-узла
            /// </summary>
            public void LoadFromXml(XmlNode xmlNode)
            {
                if (xmlNode == null)
                    throw new ArgumentNullException("xmlNode");

                SaveToDir = xmlNode.GetChildAsBool("SaveToDir", true);
                DestDir = ScadaUtils.NormalDir(xmlNode.GetChildAsString("DestDir", @"C:\SCADA\"));
                DestFile = xmlNode.GetChildAsString("DestFile", @"C:\SCADA\config.zip");
                ImportBase = xmlNode.GetChildAsBool("ImportBase", true);
            }
            /// <summary>
            /// Сохранить настройки в XML-узле
            /// </summary>
            public void SaveToXml(XmlElement xmlElem)
            {
                if (xmlElem == null)
                    throw new ArgumentNullException("xmlElem");

                xmlElem.AppendElem("SaveToDir", SaveToDir);
                xmlElem.AppendElem("DestDir", DestDir);
                xmlElem.AppendElem("DestFile", DestFile);
                xmlElem.AppendElem("ImportBase", ImportBase);
            }
        }

        /// <summary>
        /// Settings of uploading configuration
        /// <para>Настройки передачи конфигурации</para>
        /// </summary>
        public class UploadSettings
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            public UploadSettings()
            {
                SelectedFiles = new List<string>();
                SetToDefault();
            }

            /// <summary>
            /// Получить или установить директорию конфигурации
            /// </summary>
            public string SrcDir { get; set; }
            /// <summary>
            /// Получить выбранные для передачи файлы конфигурации
            /// </summary>
            public List<string> SelectedFiles { get; private set; }

            /// <summary>
            /// Установить настройки по умолчанию
            /// </summary>
            private void SetToDefault()
            {
                SrcDir = @"C:\SCADA\";
                SelectedFiles.Clear();
            }
            /// <summary>
            /// Загрузить настройки из XML-узла
            /// </summary>
            public void LoadFromXml(XmlNode xmlNode)
            {
                if (xmlNode == null)
                    throw new ArgumentNullException("xmlNode");

                SrcDir = ScadaUtils.NormalDir(xmlNode.GetChildAsString("SrcDir", @"C:\SCADA\"));

                SelectedFiles.Clear();
                XmlNode selectedFilesNode = xmlNode.SelectSingleNode("SelectedFiles");
                if (selectedFilesNode != null)
                {
                    XmlNodeList pathNodeList = xmlNode.SelectNodes("Path");
                    foreach (XmlNode pathNode in pathNodeList)
                    {
                        SelectedFiles.Add(pathNode.InnerText);
                    }
                }
            }
            /// <summary>
            /// Сохранить настройки в XML-узле
            /// </summary>
            public void SaveToXml(XmlElement xmlElem)
            {
                if (xmlElem == null)
                    throw new ArgumentNullException("xmlElem");

                xmlElem.AppendElem("SrcDir", SrcDir);

                XmlElement selectedFilesElem = xmlElem.AppendElem("SelectedFiles");
                foreach (string path in SelectedFiles)
                {
                    selectedFilesElem.AppendElem("Path", path);
                }
            }
        }

        /// <summary>
        /// Settings of interaction with a remote server
        /// <para>Настройки взаимодействия с удалённым сервером</para>
        /// </summary>
        public class ServerSettings
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            public ServerSettings()
            {
                Connection = new ConnectionSettings();
                Download = new DownloadSettings();
                Upload = new UploadSettings();
            }

            /// <summary>
            /// Получить настройки подключения к серверу
            /// </summary>
            public ConnectionSettings Connection { get; private set; }
            /// <summary>
            /// Получить настройки скачивания конфигурации
            /// </summary>
            public DownloadSettings Download { get; private set; }
            /// <summary>
            /// Получить настройки передачи конфигурации
            /// </summary>
            public UploadSettings Upload { get; private set; }

            /// <summary>
            /// Загрузить настройки из XML-узла
            /// </summary>
            public void LoadFromXml(XmlNode xmlNode)
            {
                if (xmlNode == null)
                    throw new ArgumentNullException("xmlNode");

                XmlNode connectionNode = xmlNode.SelectSingleNode("Connection");
                if (connectionNode != null)
                    Connection.LoadFromXml(connectionNode);

                XmlNode downloadNode = xmlNode.SelectSingleNode("Download");
                if (downloadNode != null)
                    Download.LoadFromXml(downloadNode);

                XmlNode uploadNode = xmlNode.SelectSingleNode("Upload");
                if (uploadNode != null)
                    Upload.LoadFromXml(uploadNode);
            }
            /// <summary>
            /// Сохранить настройки в XML-узле
            /// </summary>
            public void SaveToXml(XmlElement xmlElem)
            {
                if (xmlElem == null)
                    throw new ArgumentNullException("xmlElem");

                Connection.SaveToXml(xmlElem.AppendElem("Connection"));
                Download.SaveToXml(xmlElem.AppendElem("Download"));
                Upload.SaveToXml(xmlElem.AppendElem("Upload"));
            }
            /// <summary>
            /// Получить строковое представление объекта
            /// </summary>
            public override string ToString()
            {
                return Connection.Name;
            }
        }


        /// <summary>
        /// Имя файла настроек по умолчанию
        /// </summary>
        public const string DefFileName = "RemoteServers.xml";


        /// <summary>
        /// Конструктор
        /// </summary>
        public ServersSettings()
        {
            Servers = new SortedList<string, ServerSettings>();
        }


        /// <summary>
        /// Получить список настроек взаимодействия с серверами, ключ - наименование подключения
        /// </summary>
        public SortedList<string, ServerSettings> Servers { get; private set; }


        /// <summary>
        /// Загрузить настройки из файла
        /// </summary>
        public bool Load(string fileName, out string errMsg)
        {
            // установка значений по умолчанию
            Servers.Clear();

            try
            {
                if (!File.Exists(fileName))
                    throw new FileNotFoundException(string.Format(CommonPhrases.NamedFileNotFound, fileName));

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(fileName);

                XmlNodeList remoteServerNodeList = xmlDoc.DocumentElement.SelectNodes("RemoteServer");
                foreach (XmlNode remoteServerNode in remoteServerNodeList)
                {
                    ServerSettings serverSettings = new ServerSettings();
                    serverSettings.LoadFromXml(remoteServerNode);
                    Servers[serverSettings.Connection.Name] = serverSettings;
                }

                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = AppPhrases.LoadServersSettingsError + ":" + Environment.NewLine + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Сохранить настройки в файле
        /// </summary>
        public bool Save(string fileName, out string errMsg)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();

                XmlDeclaration xmlDecl = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
                xmlDoc.AppendChild(xmlDecl);

                XmlElement rootElem = xmlDoc.CreateElement("RemoteServers");
                xmlDoc.AppendChild(rootElem);

                foreach (ServerSettings serverSettings in Servers.Values)
                {
                    serverSettings.SaveToXml(rootElem.AppendElem("RemoteServer"));
                }

                string bakName = fileName + ".bak";
                File.Copy(fileName, bakName, true);
                xmlDoc.Save(fileName);

                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = AppPhrases.SaveServersSettingsError + ":" + Environment.NewLine + ex.Message;
                return false;
            }
        }
    }
}
