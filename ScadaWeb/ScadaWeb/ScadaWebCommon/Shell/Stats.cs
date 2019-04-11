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
 * Module   : ScadaWebCommon
 * Summary  : Provides sharing depersonalized stats with the developer team
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2018
 */

using System;
using System.IO;
using System.Text;
using System.Web;
using System.Xml;
using Utils;

namespace Scada.Web.Shell
{
    /// <summary>
    /// Provides sharing depersonalized stats with the developer team
    /// <para>Обеспечивает передачу обезличенной статистики команде разработчиков</para>
    /// </summary>
    /// <remarks>The object must be a singleton
    /// <para>Объект должен являться синглтоном</para></remarks>
    public class Stats
    {
        /// <summary>
        /// Объект для работы с хранилищем приложения
        /// </summary>
        protected readonly Storage storage;
        /// <summary>
        /// Журнал
        /// </summary>
        protected readonly Log log;
        /// <summary>
        /// Объект для синхронизации достапа к файлу параметров статистики
        /// </summary>
        protected readonly object fileLock;

        /// <summary>
        /// Ид. сервера для передачи статистики
        /// </summary>
        protected string serverID;


        /// <summary>
        /// Конструктор, ограничивающий создание объекта без параметров
        /// </summary>
        protected Stats()
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public Stats(Storage storage, Log log)
        {
            if (storage == null)
                throw new ArgumentNullException("storage");
            if (log == null)
                throw new ArgumentNullException("log");

            this.storage = storage;
            this.log = log;
            fileLock = new object();
        }


        /// <summary>
        /// Получить ид. сервера для передачи статистики, 
        /// при первом получении загрузив его из хранилица или сгенерировав новый
        /// </summary>
        protected string GetServerID()
        {
            try
            {
                if (string.IsNullOrEmpty(serverID))
                {
                    lock (fileLock)
                    {
                        string allUsersAppDir = storage.GetAllUsersAppDir();
                        Storage.ForceDir(allUsersAppDir);
                        string statsParamsFileName = allUsersAppDir + "Stats.xml";

                        if (File.Exists(statsParamsFileName))
                        {
                            serverID = LoadServerID(statsParamsFileName);
                        }
                        else
                        {
                            serverID = GenerateServerID();
                            SaveServerID(statsParamsFileName, serverID);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                serverID = "";
                log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при получении ид. сервера для передачи статистики" :
                    "Error getting server ID for sharing stats");
            }

            return serverID;
        }

        /// <summary>
        /// Генерировать ид. сервера
        /// </summary>
        protected string GenerateServerID()
        {
            // используется GUID с заменой первой группы чисел на дату в 16-ричном формате
            Guid guid = Guid.NewGuid();
            byte[] guidBuf = guid.ToByteArray();
            DateTime utcNowDT = DateTime.UtcNow;
            guidBuf[0] = (byte)utcNowDT.Day;
            guidBuf[1] = (byte)utcNowDT.Month;
            guidBuf[2] = (byte)(utcNowDT.Year % 256);
            guidBuf[3] = (byte)(utcNowDT.Year / 256);
            guidBuf[7] &= 0x0F; // установка нестандартной версии GUID
            return new Guid(guidBuf).ToString();
        }

        /// <summary>
        /// Загрузить ид. сервера из файла
        /// </summary>
        protected string LoadServerID(string fileName)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(fileName);
            return xmlDoc.DocumentElement.GetChildAsString("ServerID");
        }

        /// <summary>
        /// Сохранить ид. сервера в файле
        /// </summary>
        protected void SaveServerID(string fileName, string serverID)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlDeclaration xmlDecl = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
            xmlDoc.AppendChild(xmlDecl);

            XmlElement rootElem = xmlDoc.CreateElement("Stats");
            xmlDoc.AppendChild(rootElem);
            rootElem.AppendElem("ServerID", serverID);

            xmlDoc.Save(fileName);
        }


        /// <summary>
        /// Генерировать HTML-код для передачи статистики
        /// </summary>
        public string GenerateHtml(bool shareStats, bool useHttps, string customData = "")
        {
            if (shareStats)
            {
                StringBuilder sbHtml = new StringBuilder();
                sbHtml
                    .Append("<iframe id='frameStats' src='")
                    .Append(useHttps ? "https://" : "http://")
                    .AppendFormat(UrlTemplates.Stats, GetServerID());

                if (!string.IsNullOrEmpty(customData))
                    sbHtml.Append("#").Append(HttpUtility.UrlEncode(customData));

                sbHtml.Append("'></iframe>");
                return sbHtml.ToString();
            }
            else
            {
                return "";
            }
        }
    }
}
