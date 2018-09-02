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
 * Module   : ScadaServerCommon
 * Summary  : SCADA-Server settings
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2013
 * Modified : 2018
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Scada.Server
{
    /// <summary>
    /// SCADA-Server settings
    /// <para>Настройки SCADA-Сервера</para>
    /// </summary>
    public class Settings
    {
        /// <summary>
        /// Имя файла настроек по умолчанию
        /// </summary>
        public const string DefFileName = "ScadaServerSvcConfig.xml";
        /// <summary>
        /// Номер TCP-порта по умолчанию
        /// </summary>
        public const int DefTcpPort = 10000;

        
        /// <summary>
        /// Конструкотр
        /// </summary>
        public Settings()
        {
            ModuleFileNames = new List<string>();
            CreateBakFile = true;
            SetToDefault();
        }


        /// <summary>
        /// Получить или установить номер TCP-порта
        /// </summary>
        public int TcpPort { get; set; }

        /// <summary>
        /// Получить или установить признак использования Active Directory для аутентификации пользователей
        /// </summary>
        public bool UseAD { get; set; }

        /// <summary>
        /// Получить или установить путь к серверу контроллера домена
        /// </summary>
        public string LdapPath { get; set; }

        /// <summary>
        /// Получить или установить признак записи в журнал приложения подробной информации
        /// </summary>
        public bool DetailedLog { get; set; }


        /// <summary>
        /// Получить или установить директорию базы конфигурации в формате DAT
        /// </summary>
        public string BaseDATDir { get; set; }

        /// <summary>
        /// Получить или установить директорию интерфейса
        /// </summary>
        public string ItfDir { get; set; }

        /// <summary>
        /// Получить или установить директорию архива в формате DAT
        /// </summary>
        public string ArcDir { get; set; }

        /// <summary>
        /// Получить или установить директорию копии архива в формате DAT
        /// </summary>
        public string ArcCopyDir { get; set; }


        /// <summary>
        /// Получить или установить период записи текущего среза, с
        /// </summary>
        /// <remarks>Если значение меньше или равно 0, то выполняется запись по изменению</remarks>
        public int WriteCurPer { get; set; }

        /// <summary>
        /// Получить или установить время установки недостоверности при неактивности, мин.
        /// </summary>
        public int InactUnrelTime { get; set; }

        /// <summary>
        /// Получить или установить признак записи текущего среза
        /// </summary>
        public bool WriteCur { get; set; }

        /// <summary>
        /// Получить или установить признак записи копии текущего среза
        /// </summary>
        public bool WriteCurCopy { get; set; }

        /// <summary>
        /// Получить или установить период записи минутных срезов, с
        /// </summary>
        public int WriteMinPer { get; set; }

        /// <summary>
        /// Получить или установить период хранения минутных срезов, дн.
        /// </summary>
        public int StoreMinPer { get; set; }

        /// <summary>
        /// Получить или установить признак записи минутных срезов
        /// </summary>
        public bool WriteMin { get; set; }

        /// <summary>
        /// Получить или установить признак записи копий минутных срезов
        /// </summary>
        public bool WriteMinCopy { get; set; }

        /// <summary>
        /// Получить или установить период записи часовых срезов, с
        /// </summary>
        public int WriteHrPer { get; set; }

        /// <summary>
        /// Получить или установить период хранения часовых срезов, дн.
        /// </summary>
        public int StoreHrPer { get; set; }

        /// <summary>
        /// Получить или установить признак записи часовых срезов
        /// </summary>
        public bool WriteHr { get; set; }

        /// <summary>
        /// Получить или установить признак записи копий часовых срезов
        /// </summary>
        public bool WriteHrCopy { get; set; }

        /// <summary>
        /// Получить или установить период хранения событий, дн.
        /// </summary>
        public int StoreEvPer { get; set; }

        /// <summary>
        /// Получить или установить признак записи событий
        /// </summary>
        public bool WriteEv { get; set; }

        /// <summary>
        /// Получить или установить признак записи копий событий
        /// </summary>
        public bool WriteEvCopy { get; set; }


        /// <summary>
        /// Получить список имён файлов модулей
        /// </summary>
        public List<string> ModuleFileNames { get; private set; }

        /// <summary>
        /// Получить или установить признак создания резервного файла при сохранении настроек
        /// </summary>
        public bool CreateBakFile { get; set; }


        /// <summary>
        /// Установить значения настроек по умолчанию
        /// </summary>
        private void SetToDefault()
        {
            TcpPort = DefTcpPort;
            UseAD = false;
            LdapPath = "";
            DetailedLog = false;

            BaseDATDir = @"C:\SCADA\BaseDAT\";
            ItfDir = @"C:\SCADA\Interface\";
            ArcDir = @"C:\SCADA\ArchiveDAT\";
            ArcCopyDir = @"C:\SCADA\ArchiveDATCopy\";

            WriteCurPer = 5;
            InactUnrelTime = 5;
            WriteCur = true;
            WriteCurCopy = true;
            WriteMinPer = 60;
            StoreMinPer = 365;
            WriteMin = true;
            WriteMinCopy = true;
            WriteHrPer = 60;
            StoreHrPer = 365;
            WriteHr = true;
            WriteHrCopy = true;
            StoreEvPer = 365;
            WriteEv = true;
            WriteEvCopy = true;

            ModuleFileNames.Clear();
        }
        

        /// <summary>
        /// Загрузить настройки приложения из файла
        /// </summary>
        public bool Load(string fileName, out string errMsg)
        {
            // установка значений по умолчанию
            SetToDefault();

            // загрузка настроек
            try
            {
                if (!File.Exists(fileName))
                    throw new FileNotFoundException(string.Format(CommonPhrases.NamedFileNotFound, fileName));

                XmlDocument xmlDoc = new XmlDocument(); // обрабатываемый XML-документ
                xmlDoc.Load(fileName);
                XmlElement rootElem = xmlDoc.DocumentElement;

                // загрузка общих параметров
                XmlNode paramsNode = rootElem.SelectSingleNode("CommonParams");
                if (paramsNode != null)
                {
                    XmlNodeList paramNodeList = paramsNode.SelectNodes("Param");
                    foreach (XmlElement paramElem in paramNodeList)
                    {
                        string name = paramElem.GetAttribute("name").Trim();
                        string nameL = name.ToLowerInvariant();
                        string val = paramElem.GetAttribute("value");

                        try
                        {
                            if (nameL == "tcpport")
                                TcpPort = int.Parse(val);
                            else if (nameL == "usead")
                                UseAD = bool.Parse(val);
                            else if (nameL == "ldappath")
                                LdapPath = val;
                            else if (nameL == "detailedlog")
                                DetailedLog = bool.Parse(val);
                        }
                        catch
                        {
                            throw new Exception(string.Format(CommonPhrases.IncorrectXmlParamVal, name));
                        }
                    }
                }

                // загрузка директорий
                paramsNode = rootElem.SelectSingleNode("Directories");
                if (paramsNode != null)
                {
                    XmlNodeList paramNodeList = paramsNode.SelectNodes("Param");
                    foreach (XmlElement paramElem in paramNodeList)
                    {
                        string nameL = paramElem.GetAttribute("name").Trim().ToLowerInvariant();
                        string val = ScadaUtils.NormalDir(paramElem.GetAttribute("value"));

                        if (nameL == "basedatdir")
                            BaseDATDir = val;
                        else if (nameL == "itfdir")
                            ItfDir = val;
                        else if (nameL == "arcdir")
                            ArcDir = val;
                        else if (nameL == "arccopydir")
                            ArcCopyDir = val;
                    }
                }

                // загрузка параметров записи данных
                paramsNode = rootElem.SelectSingleNode("SaveParams");
                if (paramsNode != null)
                {
                    XmlNodeList paramNodeList = paramsNode.SelectNodes("Param");
                    foreach (XmlElement paramElem in paramNodeList)
                    {
                        string name = paramElem.GetAttribute("name").Trim();
                        string nameL = name.ToLowerInvariant();
                        string val = paramElem.GetAttribute("value");

                        try
                        {
                            if (nameL == "writecurper")
                                WriteCurPer = int.Parse(val);
                            else if (nameL == "inactunreltime")
                                InactUnrelTime = int.Parse(val);
                            else if (nameL == "writecur")
                                WriteCur = bool.Parse(val);
                            else if (nameL == "writecurcopy")
                                WriteCurCopy = bool.Parse(val);
                            else if (nameL == "writeminper")
                                WriteMinPer = int.Parse(val);
                            else if (nameL == "storeminper")
                                StoreMinPer = int.Parse(val);
                            else if (nameL == "writemin")
                                WriteMin = bool.Parse(val);
                            else if (nameL == "writemincopy")
                                WriteMinCopy = bool.Parse(val);
                            else if (nameL == "writehrper")
                                WriteHrPer = int.Parse(val) * 60;
                            else if (nameL == "storehrper")
                                StoreHrPer = int.Parse(val);
                            else if (nameL == "writehr")
                                WriteHr = bool.Parse(val);
                            else if (nameL == "writehrcopy")
                                WriteHrCopy = bool.Parse(val);
                            else if (nameL == "storeevper")
                                StoreEvPer = int.Parse(val);
                            else if (nameL == "writeev")
                                WriteEv = bool.Parse(val);
                            else if (nameL == "writeevcopy")
                                WriteEvCopy = bool.Parse(val);
                        }
                        catch
                        {
                            throw new Exception(string.Format(CommonPhrases.IncorrectXmlParamVal, name));
                        }
                    }
                }

                // загрузка имён файлов модулей
                XmlNode modulesNode = rootElem.SelectSingleNode("Modules");
                if (modulesNode != null)
                {
                    XmlNodeList moduleNodeList = modulesNode.SelectNodes("Module");
                    foreach (XmlElement moduleElem in moduleNodeList)
                        ModuleFileNames.Add(moduleElem.GetAttribute("fileName"));
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

        /// <summary>
        /// Сохранить настройки приложения в файле
        /// </summary>
        public bool Save(string fileName, out string errMsg)
        {
            try
            {
                // формирование XML-документа
                XmlDocument xmlDoc = new XmlDocument();

                XmlDeclaration xmlDecl = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
                xmlDoc.AppendChild(xmlDecl);

                XmlElement rootElem = xmlDoc.CreateElement("ScadaServerSvcConfig");
                xmlDoc.AppendChild(rootElem);

                rootElem.AppendChild(xmlDoc.CreateComment(
                    Localization.UseRussian ? "Общие параметры" : "Common Parameters"));
                XmlElement paramsElem = xmlDoc.CreateElement("CommonParams");
                rootElem.AppendChild(paramsElem);
                paramsElem.AppendParamElem("TcpPort", TcpPort, 
                    "Номер TCP-порта", "TCP port number");
                paramsElem.AppendParamElem("UseAD", UseAD,
                    "Использовать Active Directory для аутентификации пользователей", 
                    "Use Active Directory for users authentication");
                paramsElem.AppendParamElem("LdapPath", LdapPath, 
                    "Путь к серверу контроллера домена", "Domain controller server path");
                paramsElem.AppendParamElem("DetailedLog", DetailedLog,
                    "Записывать в журнал приложения подробную информацию", "Write detailed information to the log");

                rootElem.AppendChild(xmlDoc.CreateComment(Localization.UseRussian ? "Директории" : "Directories"));
                paramsElem = xmlDoc.CreateElement("Directories");
                rootElem.AppendChild(paramsElem);
                paramsElem.AppendParamElem("BaseDATDir", BaseDATDir,
                    "Директория базы конфигурации в формате DAT", 
                    "The configuration database in DAT format directory");
                paramsElem.AppendParamElem("ItfDir", ItfDir,
                    "Директория интерфейса", "The interface directory");
                paramsElem.AppendParamElem("ArcDir", ArcDir,
                    "Директория архива в формате DAT", "The archive in DAT format directory");
                paramsElem.AppendParamElem("ArcCopyDir", ArcCopyDir,
                    "Директория копии архива в формате DAT", "The archive copy in DAT format directory");

                rootElem.AppendChild(xmlDoc.CreateComment(
                    Localization.UseRussian ? "Параметры записи" : "Saving Parameters"));
                paramsElem = xmlDoc.CreateElement("SaveParams");
                rootElem.AppendChild(paramsElem);
                paramsElem.AppendParamElem("WriteCurPer", WriteCurPer,
                    "Период записи текущего среза, с", "Current data writing period, sec");
                paramsElem.AppendParamElem("InactUnrelTime", InactUnrelTime,
                    "Время установки недостоверности при неактивности, мин.", "Unreliable on inactivity, min");
                paramsElem.AppendParamElem("WriteCur", WriteCur,
                    "Записывать текущий срез", "Write current data");
                paramsElem.AppendParamElem("WriteCurCopy", WriteCurCopy,
                    "Записывать копию текущего среза", "Write current data copy");
                paramsElem.AppendParamElem("WriteMinPer", WriteMinPer,
                    "Период записи минутных срезов, с", "Minute data writing period, sec");
                paramsElem.AppendParamElem("StoreMinPer", StoreMinPer,
                    "Период хранения минутных срезов, дн.", "Minute data storing period, days");
                paramsElem.AppendParamElem("WriteMin", WriteMin,
                    "Записывать минутные срезы", "Write minute data");
                paramsElem.AppendParamElem("WriteMinCopy", WriteMinCopy,
                    "Записывать копии минутных срезов", "Write minute data copy");
                paramsElem.AppendParamElem("WriteHrPer", WriteHrPer / 60,
                    "Период записи часовых срезов, мин.", "Hourly data writing period, min");
                paramsElem.AppendParamElem("StoreHrPer", StoreHrPer,
                    "Период хранения часовых срезов, дн.", "Hourly data storing period, days");
                paramsElem.AppendParamElem("WriteHr", WriteHr,
                    "Записывать часовые срезы", "Write hourly data");
                paramsElem.AppendParamElem("WriteHrCopy", WriteHrCopy,
                    "Записывать копии часовых срезов", "Write hourly data copy");
                paramsElem.AppendParamElem("StoreEvPer", StoreEvPer,
                    "Период хранения событий, дн.", "Events storing period, days");
                paramsElem.AppendParamElem("WriteEv", WriteEv,
                    "Записывать события", "Write events");
                paramsElem.AppendParamElem("WriteEvCopy", WriteEvCopy,
                    "Записывать копии событий", "Write events copy");

                rootElem.AppendChild(xmlDoc.CreateComment(Localization.UseRussian ? "Модули" : "Modules"));
                XmlElement modulesElem = xmlDoc.CreateElement("Modules");
                rootElem.AppendChild(modulesElem);

                foreach (string moduleFileName in ModuleFileNames)
                {
                    XmlElement moduleElem = xmlDoc.CreateElement("Module");
                    moduleElem.SetAttribute("fileName", moduleFileName);
                    modulesElem.AppendChild(moduleElem);
                }

                // сохранение XML-документа в файле
                if (CreateBakFile)
                {
                    string bakName = fileName + ".bak";
                    File.Copy(fileName, bakName, true);
                }

                xmlDoc.Save(fileName);

                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = CommonPhrases.SaveAppSettingsError + ":" + Environment.NewLine + ex.Message;
                return false;
            }
        }
    }
}
