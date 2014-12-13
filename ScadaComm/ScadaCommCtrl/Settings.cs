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
 * Module   : SCADA-Communicator Control
 * Summary  : SCADA-Communicator settings
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2008
 * Modified : 2014
 */

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Text;
using System.Xml;

namespace Scada.Comm.Ctrl
{
    /// <summary>
    /// SCADA-Communicator settings
    /// <para>Настройки SCADA-Коммуникатора</para>
    /// </summary>
    internal class Settings
    {
        /// <summary>
        /// Общие параметры
        /// </summary>
        public class CommonParams
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            public CommonParams()
            {
                SetToDefault();
            }

            /// <summary>
            /// Получить или установить признак использования SCADA-Сервера
            /// </summary>
            public bool ServerUse { get; set; }
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
            /// Получить или установить ожидание остановки циклов опроса линий связи, мс
            /// </summary>
            public int WaitForStop { get; set; }
            /// <summary>
            /// Получить или установить период передачи на сервер всех параметров КП для обновления, с
            /// </summary>
            public int RefrParams { get; set; }

            /// <summary>
            /// Установить значения общих параметров по умолчанию
            /// </summary>
            public void SetToDefault()
            {
                ServerUse = false;
                ServerHost = "localhost";
                ServerPort = 10000;
                ServerUser = "ScadaComm";
                ServerPwd = "12345";
                ServerTimeout = 10000;
                WaitForStop = 1000;
                RefrParams = 60;
            }
            /// <summary>
            /// Создать полную копию общих параметров
            /// </summary>
            public CommonParams Clone()
            {
                CommonParams commonParams = new CommonParams();
                commonParams.ServerUse = ServerUse;
                commonParams.ServerHost = ServerHost;
                commonParams.ServerPort = ServerPort;
                commonParams.ServerUser = ServerUser;
                commonParams.ServerPwd = ServerPwd;
                commonParams.ServerTimeout = ServerTimeout;
                commonParams.WaitForStop = WaitForStop;
                commonParams.RefrParams = RefrParams;
                return commonParams;
            }
        }

        /// <summary>
        /// Линия связи
        /// </summary>
        public class CommLine
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            public CommLine()
            {
                Active = true;
                Bind = true;
                Number = 0;
                Name = "";

                ConnType = "None";
                PortName = "COM1";
                BaudRate = 9600;
                DataBits = 8;
                Parity = Parity.None;
                StopBits = StopBits.One;
                DtrEnable = false;
                RtsEnable = false;

                ReqTriesCnt = 3;
                CycleDelay = 0;
                MaxCommErrCnt = 1;
                CmdEnabled = false;

                UserParams = new List<UserParam>();
                ReqSequence = new List<KP>();
            }

            /// <summary>
            /// Получить или установить признак активности
            /// </summary>
            public bool Active { get; set; }
            /// <summary>
            /// Получить или установить признак привязки к SCADA-Серверу
            /// </summary>
            public bool Bind { get; set; }
            /// <summary>
            /// Получить или установить номер линии связи
            /// </summary>
            public int Number { get; set; }
            /// <summary>
            /// Получить или установить наименование линии связи
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// Получить обозначение линии связи
            /// </summary>
            public string Caption
            {
                get
                {
                    return GetCaption(Number, Name);
                }
            }

            /// <summary>
            /// Получить или установить тип подключения
            /// </summary>
            public string ConnType { get; set; }
            /// <summary>
            /// Получить или установить имя последовательного порта
            /// </summary>
            public string PortName { get; set; }
            /// <summary>
            /// Получить или установить скорость обмена
            /// </summary>
            public int BaudRate { get; set; }
            /// <summary>
            /// Получить или установить биты данных
            /// </summary>
            public int DataBits { get; set; }
            /// <summary>
            /// Получить или установить контроль чётности
            /// </summary>
            public Parity Parity { get; set; }
            /// <summary>
            /// Получить или установить стоповые биты
            /// </summary>
            public StopBits StopBits { get; set; }
            /// <summary>
            /// Получить или установить использование сигнала Data Terminal Ready (DTR) 
            /// </summary>
            public bool DtrEnable { get; set; }
            /// <summary>
            /// Получить или установить использование сигнала Request to Send (RTS)
            /// </summary>
            public bool RtsEnable { get; set; }

            /// <summary>
            /// Получить или установить количество попыток перезапроса КП при ошибке
            /// </summary>
            public int ReqTriesCnt { get; set; }
            /// <summary>
            /// Получить или установить задержку после цикла опроса, мс
            /// </summary>
            public int CycleDelay { get; set; }
            /// <summary>
            /// Получить или установить количество неудачных сеансов связи до объявления КП неработающим
            /// </summary>
            public int MaxCommErrCnt { get; set; }
            /// <summary>
            /// Получить или установить признак разрешения команд ТУ
            /// </summary>
            public bool CmdEnabled { get; set; }

            /// <summary>
            /// Получить пользовательские параметры линии связи
            /// </summary>
            public List<UserParam> UserParams { get; private set; }
            /// <summary>
            /// Получить последовательность опроса КП
            /// </summary>
            public List<KP> ReqSequence { get; private set; }

            /// <summary>
            /// Создать полную копию настроек линии связи
            /// </summary>
            public CommLine Clone()
            {
                CommLine commLine = new CommLine();

                commLine.Active = Active;
                commLine.Bind = Bind;
                commLine.Number = Number;
                commLine.Name = Name;

                commLine.ConnType = ConnType;
                commLine.PortName = PortName;
                commLine.BaudRate = BaudRate;
                commLine.DataBits = DataBits;
                commLine.Parity = Parity;
                commLine.StopBits = StopBits;
                commLine.DtrEnable = DtrEnable;
                commLine.RtsEnable = RtsEnable;

                commLine.ReqTriesCnt = ReqTriesCnt;
                commLine.CycleDelay = CycleDelay;
                commLine.MaxCommErrCnt = MaxCommErrCnt;
                commLine.CmdEnabled = CmdEnabled;

                commLine.UserParams = new List<UserParam>();
                foreach (UserParam userParam in UserParams)
                    commLine.UserParams.Add(userParam.Clone());

                commLine.ReqSequence = new List<KP>();
                foreach (KP kp in ReqSequence)
                    commLine.ReqSequence.Add(kp.Clone());

                return commLine;
            }
            /// <summary>
            /// Получить обозначение линии связи
            /// </summary>
            public static string GetCaption(int number, object name)
            {
                string nameStr = name == null || name == DBNull.Value ? "" : name.ToString();
                return AppPhrases.LineCaption + " " + number + (nameStr == "" ? "" : " \"" + nameStr + "\"");
            }
        }

        /// <summary>
        /// Пользовательский параметр линии связи
        /// </summary>
        public class UserParam
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            public UserParam()
            {
                Name = "";
                Value = "";
                Descr = "";
            }

            /// <summary>
            /// Получить или установить наименование
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// Получить или установить значение
            /// </summary>
            public string Value { get; set; }
            /// <summary>
            /// Получить или установить описание
            /// </summary>
            public string Descr { get; set; }

            /// <summary>
            /// Создать полную копию пользовательского параметра
            /// </summary>
            public UserParam Clone()
            {
                UserParam userParam = new UserParam();
                userParam.Name = Name;
                userParam.Value = Value;
                userParam.Descr = Descr;
                return userParam;
            }
        }

        /// <summary>
        /// КП
        /// </summary>
        public class KP
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            public KP()
            {
                Active = true;
                Bind = true;
                Number = 0;
                Name = "";
                Dll = "";
                Address = 0;
                CallNum = "";
                Timeout = 0;
                Delay = 0;
                Time = DateTime.MinValue;
                Period = new TimeSpan(0);
                CmdLine = "";
            }

            /// <summary>
            /// Получить или установить признак активности
            /// </summary>
            public bool Active { get; set; }
            /// <summary>
            /// Получить или установить признак привязки к SCADA-Серверу
            /// </summary>
            public bool Bind { get; set; }
            /// <summary>
            /// Получить или установить номер КП
            /// </summary>
            public int Number { get; set; }
            /// <summary>
            /// Получить или установить наименование КП
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// Получить обозначение КП
            /// </summary>
            public string Caption
            {
                get
                {
                    return GetCaption(Number, Name);
                }
            }

            /// <summary>
            /// Получить или установить библиотеку КП
            /// </summary>
            public string Dll { get; set; }
            /// <summary>
            /// Получить или установить адрес
            /// </summary>
            public int Address { get; set; }
            /// <summary>
            /// Получить или установить позывной
            /// </summary>
            public string CallNum { get; set; }
            /// <summary>
            /// Получить или установить таймаут запросов, мс
            /// </summary>
            public int Timeout { get; set; }
            /// <summary>
            /// Получить или установить задержку после запроса, мс
            /// </summary>
            public int Delay { get; set; }
            /// <summary>
            /// Получить или установить время сеанса опроса
            /// </summary>
            public DateTime Time { get; set; }
            /// <summary>
            /// Получить или установить период сеансов опроса
            /// </summary>
            public TimeSpan Period { get; set; }
            /// <summary>
            /// Получить или установить командную строку
            /// </summary>
            public string CmdLine { get; set; }

            /// <summary>
            /// Создать полную копию КП
            /// </summary>
            public KP Clone()
            {
                KP kp = new KP();

                kp.Active = Active;
                kp.Bind = Bind;
                kp.Number = Number;
                kp.Name = Name;
                kp.Dll = Dll;
                kp.Address = Address;
                kp.CallNum = CallNum;
                kp.Timeout = Timeout;
                kp.Delay = Delay;
                kp.Time = Time;
                kp.Period = Period;
                kp.CmdLine = CmdLine;

                return kp;
            }
            /// <summary>
            /// Получить обозначение линии связи
            /// </summary>
            public static string GetCaption(int number, object name)
            {
                string nameStr = name == null || name == DBNull.Value ? "" : name.ToString();
                return AppPhrases.KPCaption + " " + number + (nameStr == "" ? "" : " \"" + nameStr + "\"");
            }
        }


        /// <summary>
        /// Имя файла настроек по умолчанию
        /// </summary>
        public const string DefFileName = "ScadaCommSvcConfig.xml";


        /// <summary>
        /// Конструктор
        /// </summary>
        public Settings()
        {
            Params = new CommonParams();
            CommLines = new List<CommLine>();
        }


        /// <summary>
        /// Получить настройки общих параметров
        /// </summary>
        public CommonParams Params { get; private set; }

        /// <summary>
        /// Получить настройки линий связи
        /// </summary>
        public List<CommLine> CommLines { get; private set; }


        /// <summary>
        /// Загрузить общие параметры
        /// </summary>
        private void LoadCommonParams(XmlDocument xmlDoc, StringBuilder errorBuilder)
        {
            try
            {
                XmlNode xmlNode = xmlDoc.DocumentElement.SelectSingleNode("CommonParams");
                if (xmlNode == null)
                {
                    throw new Exception(AppPhrases.NoCommonParams);
                }
                else
                {
                    XmlNodeList xmlNodeList = xmlNode.SelectNodes("Param");
                    foreach (XmlElement xmlElement in xmlNodeList)
                    {
                        string name = xmlElement.GetAttribute("name");
                        string nameL = name.ToLower();
                        string val = xmlElement.GetAttribute("value");

                        try
                        {
                            if (nameL == "serveruse")
                                Params.ServerUse = bool.Parse(val.ToLower());
                            else if (nameL == "serverhost")
                                Params.ServerHost = val;
                            else if (nameL == "serverport")
                                Params.ServerPort = int.Parse(val);
                            else if (nameL == "serveruser")
                                Params.ServerUser = val;
                            else if (nameL == "serverpwd")
                                Params.ServerPwd = val;
                            else if (nameL == "servertimeout")
                                Params.ServerTimeout = int.Parse(val);
                            else if (nameL == "waitforstop")
                                Params.WaitForStop = int.Parse(val);
                            else if (nameL == "refrparams")
                                Params.RefrParams = int.Parse(val);
                        }
                        catch
                        {
                            throw new Exception(string.Format(CommonPhrases.IncorrectXmlParamVal, name));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorBuilder.AppendLine(AppPhrases.LoadCommonParamsError + ":\r\n" + ex.Message);
            }
        }

        /// <summary>
        /// Загрузить линии связи
        /// </summary>
        private void LoadCommLines(XmlDocument xmlDoc, StringBuilder errorBuilder)
        {
            try
            {
                XmlNode xmlNode = xmlDoc.DocumentElement.SelectSingleNode("CommLines");
                if (xmlNode == null)
                {
                    throw new Exception(AppPhrases.NoCommLines);
                }
                else
                {
                    XmlNodeList listCommLine = xmlNode.SelectNodes("CommLine");
                    foreach (XmlElement elemCommLine in listCommLine)
                    {
                        string lineNumStr = elemCommLine.GetAttribute("number");
                        try
                        {
                            CommLine commLine = new CommLine();
                            commLine.Active = elemCommLine.GetAttrAsBool("active");
                            commLine.Bind = elemCommLine.GetAttrAsBool("bind");
                            commLine.Name = elemCommLine.GetAttribute("name");
                            commLine.Number = elemCommLine.GetAttrAsInt("number");
                            CommLines.Add(commLine);

                            // загрузка настроек соединения линии связи
                            XmlElement elemConn = elemCommLine.SelectSingleNode("Connection") as XmlElement;
                            if (elemConn != null)
                            {
                                XmlElement elemConnType = elemConn.SelectSingleNode("ConnType") as XmlElement;
                                if (elemConnType != null)
                                    commLine.ConnType = elemConnType.GetAttribute("value");

                                XmlElement elemCommSett = elemConn.SelectSingleNode("ComPortSettings") as XmlElement;
                                if (elemCommSett != null)
                                {
                                    try
                                    {
                                        commLine.PortName = elemCommSett.GetAttribute("portName");
                                        commLine.BaudRate = elemCommSett.GetAttrAsInt("baudRate");
                                        commLine.DataBits = elemCommSett.GetAttrAsInt("dataBits");
                                        commLine.Parity = (Parity)Enum.Parse(typeof(Parity),
                                            elemCommSett.GetAttribute("parity"), true);
                                        commLine.StopBits = (StopBits)Enum.Parse(typeof(StopBits),
                                            elemCommSett.GetAttribute("stopBits"), true);
                                        commLine.DtrEnable = elemCommSett.GetAttrAsBool("dtrEnable");
                                        commLine.RtsEnable = elemCommSett.GetAttrAsBool("rtsEnable");
                                    }
                                    catch (Exception ex)
                                    {
                                        throw new Exception(AppPhrases.IncorrectPortSettings + ":\r\n" + ex.Message);
                                    }
                                }
                            }

                            // загрузка параметров линии связи
                            XmlElement elemLineParams = elemCommLine.SelectSingleNode("LineParams") as XmlElement;
                            if (elemLineParams != null)
                            {
                                XmlNodeList listParam = elemLineParams.SelectNodes("Param");
                                foreach (XmlElement elemParam in listParam)
                                {
                                    string name = elemParam.GetAttribute("name");
                                    string nameL = name.ToLower();
                                    string val = elemParam.GetAttribute("value");
                                    
                                    try
                                    {
                                        if (nameL == "reqtriescnt")
                                            commLine.ReqTriesCnt = int.Parse(val);
                                        else if (nameL == "cycledelay")
                                            commLine.CycleDelay = int.Parse(val);
                                        else if (nameL == "maxcommerrcnt")
                                            commLine.MaxCommErrCnt = int.Parse(val);
                                        else if (nameL == "cmdenabled")
                                            commLine.CmdEnabled = bool.Parse(val);
                                    }
                                    catch
                                    {
                                        throw new Exception(AppPhrases.IncorrectCommSettings + ":\r\n" + 
                                            string.Format(CommonPhrases.IncorrectXmlParamVal, name));
                                    }
                                }
                            }

                            // загрузка пользовательских параметров линии связи
                            XmlElement elemUserParams = elemCommLine.SelectSingleNode("UserParams") as XmlElement;
                            if (elemUserParams != null)
                            {
                                XmlNodeList listParam = elemUserParams.SelectNodes("Param");
                                foreach (XmlElement elemParam in listParam)
                                {
                                    string name = elemParam.GetAttribute("name");
                                    if (name != "")
                                    {
                                        UserParam userParam = new UserParam();
                                        userParam.Name = name;
                                        userParam.Value = elemParam.GetAttribute("value");
                                        userParam.Descr = elemParam.GetAttribute("descr");
                                        commLine.UserParams.Add(userParam);
                                    }
                                }
                            }

                            // загрузка последовательности опроса линии связи
                            XmlElement elemReqSeq = elemCommLine.SelectSingleNode("ReqSequence") as XmlElement;
                            if (elemReqSeq != null)
                            {
                                XmlNodeList listKP = elemReqSeq.SelectNodes("KP");
                                foreach (XmlElement elemKP in listKP)
                                {
                                    string kpNumStr = elemKP.GetAttribute("number");
                                    try
                                    {
                                        KP kp = new KP();
                                        kp.Active = elemKP.GetAttrAsBool("active");
                                        kp.Bind = elemKP.GetAttrAsBool("bind");
                                        kp.Number = elemKP.GetAttrAsInt("number");
                                        kp.Name = elemKP.GetAttribute("name");
                                        kp.Dll = elemKP.GetAttribute("dll");
                                        kp.CallNum = elemKP.GetAttribute("callNum");
                                        kp.CmdLine = elemKP.GetAttribute("cmdLine");
                                        commLine.ReqSequence.Add(kp);

                                        string address = elemKP.GetAttribute("address");
                                        if (address != "")
                                            kp.Address = elemKP.GetAttrAsInt("address");

                                        kp.Timeout = elemKP.GetAttrAsInt("timeout");
                                        kp.Delay = elemKP.GetAttrAsInt("delay");

                                        string time = elemKP.GetAttribute("time");
                                        if (time != "")
                                            kp.Time = elemKP.GetAttrAsDateTime("time");

                                        string period = elemKP.GetAttribute("period");
                                        if (period != "")
                                            kp.Period = elemKP.GetAttrAsTimeSpan("period");
                                    }
                                    catch (Exception ex)
                                    {
                                        throw new Exception(string.Format(AppPhrases.IncorrectKPSettings, kpNumStr) +
                                            ":\r\n" + ex.Message);
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(string.Format(AppPhrases.IncorrectLineSettings, lineNumStr) +
                                ":\r\n" + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorBuilder.AppendLine(AppPhrases.LoadCommLinesError + ":\r\n" + ex.Message);
            }
        }


        /// <summary>
        /// Загрузить настройки приложения из файла
        /// </summary>
        public bool Load(string fileName, out string errMsg)
        {
            // очистка существующих настроек
            Params.SetToDefault();
            CommLines.Clear();

            // распознавание XML-документа
            XmlDocument xmlDoc = null;
            try
            {
                if (!File.Exists(fileName))
                    throw new FileNotFoundException(string.Format(CommonPhrases.NamedFileNotFound, fileName));

                xmlDoc = new XmlDocument();
                xmlDoc.Load(fileName);
            }
            catch (Exception ex)
            {
                errMsg = CommonPhrases.LoadAppSettingsError + ":\r\n" + ex.Message;
                return false;
            }

            // загрузка общих параметров
            StringBuilder errorBuilder = new StringBuilder();
            LoadCommonParams(xmlDoc, errorBuilder);

            // загрузка линий связи
            LoadCommLines(xmlDoc, errorBuilder);

            errMsg = errorBuilder.ToString();
            return errMsg == "";
        }

        /// <summary>
        /// Сохранить настройки приложения в файле
        /// </summary>
        public bool Save(string fileName, out string errMsg)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();

                XmlDeclaration xmlDecl = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
                xmlDoc.AppendChild(xmlDecl);

                XmlElement rootElem = xmlDoc.CreateElement("ScadaCommSvcConfig");
                xmlDoc.AppendChild(rootElem);

                // Общие параметры
                rootElem.AppendChild(xmlDoc.CreateComment(
                    Localization.UseRussian ? "Общие параметры" : "Common Parameters"));
                XmlElement paramsElem = xmlDoc.CreateElement("CommonParams");
                rootElem.AppendChild(paramsElem);
                paramsElem.AppendParamElem("ServerUse", Params.ServerUse,
                    "Использовать SCADA-Сервер", "Use SCADA-Server");
                paramsElem.AppendParamElem("ServerHost", Params.ServerHost,
                    "Имя компьютера или IP-адрес SCADA-Сервера", "SCADA-Server host or IP address");
                paramsElem.AppendParamElem("ServerPort", Params.ServerPort,
                    "Номер TCP-порта SCADA-Сервера", "SCADA-Server TCP port number");
                paramsElem.AppendParamElem("ServerUser", Params.ServerUser,
                    "Имя пользователя для подключения к SCADA-Серверу", 
                    "User name for the connection to SCADA-Server");
                paramsElem.AppendParamElem("ServerPwd", Params.ServerPwd,
                    "Пароль пользователя для подключения к SCADA-Серверу", 
                    "User password for the connection to SCADA-Server");
                paramsElem.AppendParamElem("ServerTimeout", Params.ServerTimeout,
                    "Таймаут ожидания ответа SCADA-Сервера, мс", "SCADA-Server response timeout, ms");
                paramsElem.AppendParamElem("WaitForStop", Params.WaitForStop,
                    "Ожидание остановки линий связи, мс", "Waiting for the communication lines temrination, ms");
                paramsElem.AppendParamElem("RefrParams", Params.RefrParams,
                    "Период передачи всех данных КП, с", "Sending all device data period, sec");

                // Линии связи
                rootElem.AppendChild(xmlDoc.CreateComment(
                    Localization.UseRussian ? "Линии связи" : "Communication Lines"));
                XmlElement linesElem = xmlDoc.CreateElement("CommLines");
                rootElem.AppendChild(linesElem);

                foreach (CommLine commLine in CommLines)
                {
                    linesElem.AppendChild(xmlDoc.CreateComment(
                        (Localization.UseRussian ? "Линия " : "Line ") + commLine.Number));

                    // соединение
                    XmlElement lineElem = xmlDoc.CreateElement("CommLine");
                    lineElem.SetAttribute("active", commLine.Active);
                    lineElem.SetAttribute("bind", commLine.Bind);
                    lineElem.SetAttribute("number", commLine.Number);
                    lineElem.SetAttribute("name", commLine.Name);
                    linesElem.AppendChild(lineElem);

                    XmlElement connElem = xmlDoc.CreateElement("Connection");
                    lineElem.AppendChild(connElem);

                    XmlElement connTypeElem = xmlDoc.CreateElement("ConnType");
                    connTypeElem.SetAttribute("value", commLine.ConnType);
                    connTypeElem.SetAttribute("descr", Localization.UseRussian ? 
                        "Тип подключения: ComPort или None" : "Connection type: ComPort or None");
                    connElem.AppendChild(connTypeElem);

                    XmlElement portElem = xmlDoc.CreateElement("ComPortSettings");
                    portElem.SetAttribute("portName", commLine.PortName);
                    portElem.SetAttribute("baudRate", commLine.BaudRate);
                    portElem.SetAttribute("dataBits", commLine.DataBits);
                    portElem.SetAttribute("parity", commLine.Parity);
                    portElem.SetAttribute("stopBits", commLine.StopBits);
                    portElem.SetAttribute("dtrEnable", commLine.DtrEnable);
                    portElem.SetAttribute("rtsEnable", commLine.RtsEnable);
                    connElem.AppendChild(portElem);

                    // параметры
                    paramsElem = xmlDoc.CreateElement("LineParams");
                    lineElem.AppendChild(paramsElem);
                    paramsElem.AppendParamElem("ReqTriesCnt", commLine.ReqTriesCnt,
                        "Количество попыток перезапроса КП при ошибке", "Device request retries count on error");
                    paramsElem.AppendParamElem("CycleDelay", commLine.CycleDelay,
                        "Задержка после цикла опроса, мс", "Delay after request cycle, ms");
                    paramsElem.AppendParamElem("MaxCommErrCnt", commLine.MaxCommErrCnt,
                        "Количество неудачных сеансов связи до объявления КП неработающим",
                        "Failed session count for setting device error state");
                    paramsElem.AppendParamElem("CmdEnabled", commLine.CmdEnabled,
                        "Команды ТУ разрешены", "Commands enabled");

                    // пользовательские параметры
                    paramsElem = xmlDoc.CreateElement("UserParams");
                    lineElem.AppendChild(paramsElem);
                    foreach (UserParam param in commLine.UserParams)
                        paramsElem.AppendParamElem(param.Name, param.Value, param.Descr);

                    // последовательность опроса
                    XmlElement reqSeqElem = xmlDoc.CreateElement("ReqSequence");
                    lineElem.AppendChild(reqSeqElem);

                    foreach (KP kp in commLine.ReqSequence)
                    {
                        XmlElement kpElem = xmlDoc.CreateElement("KP");
                        kpElem.SetAttribute("active", kp.Active);
                        kpElem.SetAttribute("bind", kp.Bind);
                        kpElem.SetAttribute("number", kp.Number);
                        kpElem.SetAttribute("name", kp.Name);
                        kpElem.SetAttribute("dll", kp.Dll);
                        kpElem.SetAttribute("address", kp.Address);
                        kpElem.SetAttribute("callNum", kp.CallNum);
                        kpElem.SetAttribute("timeout", kp.Timeout);
                        kpElem.SetAttribute("delay", kp.Delay);
                        kpElem.SetAttribute("time", kp.Time.ToString("T", DateTimeFormatInfo.InvariantInfo));
                        kpElem.SetAttribute("period", kp.Period);
                        kpElem.SetAttribute("cmdLine", kp.CmdLine);
                        reqSeqElem.AppendChild(kpElem);
                    }
                }

                // сохранение XML-документа в файле
                string bakName = fileName + ".bak";
                File.Copy(fileName, bakName, true);
                xmlDoc.Save(fileName);

                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = CommonPhrases.SaveAppSettingsError + ":\r\n" + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Создать полную копию настроек
        /// </summary>
        public Settings Clone()
        {
            Settings settings = new Settings();

            settings.Params = Params.Clone();
            settings.CommLines = new List<CommLine>();
            foreach (CommLine commLine in CommLines)
                settings.CommLines.Add(commLine.Clone());

            return settings;
        }
    }
}
