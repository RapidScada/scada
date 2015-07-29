/*
 * Copyright 2015 Mikhail Shiryaev
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
 * Modified : 2015
 */

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Xml;

namespace Scada.Comm
{
    /// <summary>
    /// SCADA-Communicator settings
    /// <para>Настройки SCADA-Коммуникатора</para>
    /// </summary>
    public class Settings
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
            /// Получить или установить период передачи на сервер всех данных КП, с
            /// </summary>
            public int SendAllDataPer { get; set; }

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
                SendAllDataPer = 60;
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
                commonParams.SendAllDataPer = SendAllDataPer;
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
                CmdEnabled = false;
                DetailedLog = true;

                CustomParams = new List<CustomParam>();
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
            /// Получить или установить признак разрешения команд ТУ
            /// </summary>
            public bool CmdEnabled { get; set; }
            /// <summary>
            /// Получить или установить признак записи в журнал линии связи подробной информации
            /// </summary>
            public bool DetailedLog { get; set; }

            /// <summary>
            /// Получить пользовательские параметры линии связи
            /// </summary>
            public List<CustomParam> CustomParams { get; private set; }
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
                commLine.CmdEnabled = CmdEnabled;
                commLine.DetailedLog = DetailedLog;

                commLine.CustomParams = new List<CustomParam>();
                foreach (CustomParam userParam in CustomParams)
                    commLine.CustomParams.Add(userParam.Clone());

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
                return CommPhrases.LineCaption + " " + number + (nameStr == "" ? "" : " \"" + nameStr + "\"");
            }
        }

        /// <summary>
        /// Пользовательский параметр линии связи
        /// </summary>
        public class CustomParam
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            public CustomParam()
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
            public CustomParam Clone()
            {
                return new CustomParam()
                {
                    Name = this.Name,
                    Value = this.Value,
                    Descr = this.Descr
                };
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
                return new KP()
                {
                    Active = this.Active,
                    Bind = this.Bind,
                    Number = this.Number,
                    Name = this.Name,
                    Dll = this.Dll,
                    Address = this.Address,
                    CallNum = this.CallNum,
                    Timeout = this.Timeout,
                    Delay = this.Delay,
                    Time = this.Time,
                    Period = this.Period,
                    CmdLine = this.CmdLine
                };
            }
            /// <summary>
            /// Получить обозначение линии связи
            /// </summary>
            public static string GetCaption(int number, object name)
            {
                string nameStr = name == null || name == DBNull.Value ? "" : name.ToString();
                return CommPhrases.KPCaption + " " + number + (nameStr == "" ? "" : " \"" + nameStr + "\"");
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
        private void LoadCommonParams(XmlDocument xmlDoc)
        {
            XmlNode commonParamsNode = xmlDoc.DocumentElement.SelectSingleNode("CommonParams");
            if (commonParamsNode != null)
            {
                XmlNodeList paramNodes = commonParamsNode.SelectNodes("Param");
                foreach (XmlElement paramElem in paramNodes)
                {
                    string name = paramElem.GetAttribute("name");
                    string nameL = name.ToLowerInvariant();
                    string val = paramElem.GetAttribute("value");

                    try
                    {
                        if (nameL == "serveruse")
                            Params.ServerUse = bool.Parse(val);
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
                        else if (nameL == "updatealldataper")
                            Params.SendAllDataPer = int.Parse(val);
                    }
                    catch
                    {
                        throw new Exception(string.Format(CommonPhrases.IncorrectXmlParamVal, name));
                    }
                }
            }
        }

        /// <summary>
        /// Загрузить линии связи
        /// </summary>
        private void LoadCommLines(XmlDocument xmlDoc)
        {
            XmlNode commLinesNode = xmlDoc.DocumentElement.SelectSingleNode("CommLines");
            if (commLinesNode != null)
            {
                XmlNodeList commLineNodes = commLinesNode.SelectNodes("CommLine");
                foreach (XmlElement commLineElem in commLineNodes)
                {
                    string lineNumStr = commLineElem.GetAttribute("number");
                    try
                    {
                        CommLine commLine = LoadCommLine(commLineElem);
                        CommLines.Add(commLine);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(string.Format(CommPhrases.IncorrectLineSettings, lineNumStr) +
                            ":" + Environment.NewLine + ex.Message);
                    }
                }
            }
        }

        /// <summary>
        /// Загрузить одну линию связи
        /// </summary>
        private CommLine LoadCommLine(XmlElement commLineElem)
        {
            CommLine commLine = new CommLine();
            commLine.Active = commLineElem.GetAttrAsBool("active");
            commLine.Bind = commLineElem.GetAttrAsBool("bind");
            commLine.Name = commLineElem.GetAttribute("name");
            commLine.Number = commLineElem.GetAttrAsInt("number");

            // загрузка настроек соединения линии связи
            XmlElement connElem = commLineElem.SelectSingleNode("Connection") as XmlElement;
            if (connElem != null)
            {
                XmlElement connTypeElem = connElem.SelectSingleNode("ConnType") as XmlElement;
                if (connTypeElem != null)
                    commLine.ConnType = connTypeElem.GetAttribute("value");

                XmlElement commSettElem = connElem.SelectSingleNode("ComPortSettings") as XmlElement;
                if (commSettElem != null)
                {
                    commLine.PortName = commSettElem.GetAttribute("portName");
                    commLine.BaudRate = commSettElem.GetAttrAsInt("baudRate");
                    commLine.DataBits = commSettElem.GetAttrAsInt("dataBits");
                    commLine.Parity = (Parity)Enum.Parse(typeof(Parity),
                        commSettElem.GetAttribute("parity"), true);
                    commLine.StopBits = (StopBits)Enum.Parse(typeof(StopBits),
                        commSettElem.GetAttribute("stopBits"), true);
                    commLine.DtrEnable = commSettElem.GetAttrAsBool("dtrEnable");
                    commLine.RtsEnable = commSettElem.GetAttrAsBool("rtsEnable");
                }
            }

            // загрузка параметров связи
            XmlElement lineParamsElem = commLineElem.SelectSingleNode("LineParams") as XmlElement;
            if (lineParamsElem != null)
            {
                XmlNodeList paramNodes = lineParamsElem.SelectNodes("Param");
                foreach (XmlElement paramElem in paramNodes)
                {
                    string name = paramElem.GetAttribute("name");
                    string nameL = name.ToLowerInvariant();
                    string val = paramElem.GetAttribute("value");

                    try
                    {
                        if (nameL == "reqtriescnt")
                            commLine.ReqTriesCnt = int.Parse(val);
                        else if (nameL == "cycledelay")
                            commLine.CycleDelay = int.Parse(val);
                        else if (nameL == "cmdenabled")
                            commLine.CmdEnabled = bool.Parse(val);
                        else if (nameL == "detailedlog")
                            commLine.DetailedLog = bool.Parse(val);
                    }
                    catch
                    {
                        throw new Exception(string.Format(CommonPhrases.IncorrectXmlParamVal, name));
                    }
                }
            }

            // загрузка пользовательских параметров линии связи
            XmlElement customParamsElem = commLineElem.SelectSingleNode("CustomParams") as XmlElement;
            if (customParamsElem == null)
                customParamsElem = commLineElem.SelectSingleNode("UserParams") as XmlElement; // обратная совместимость

            if (customParamsElem != null)
            {
                XmlNodeList paramNodes = customParamsElem.SelectNodes("Param");
                foreach (XmlElement paramElem in paramNodes)
                {
                    string name = paramElem.GetAttribute("name");
                    if (name != "")
                    {
                        CustomParam customParam = new CustomParam();
                        customParam.Name = name;
                        customParam.Value = paramElem.GetAttribute("value");
                        customParam.Descr = paramElem.GetAttribute("descr");
                        commLine.CustomParams.Add(customParam);
                    }
                }
            }

            // загрузка последовательности опроса линии связи
            XmlElement reqSeqElem = commLineElem.SelectSingleNode("ReqSequence") as XmlElement;
            if (reqSeqElem != null)
            {
                XmlNodeList kpNodes = reqSeqElem.SelectNodes("KP");
                foreach (XmlElement kpElem in kpNodes)
                {
                    string kpNumStr = kpElem.GetAttribute("number");
                    try
                    {
                        KP kp = new KP();
                        kp.Active = kpElem.GetAttrAsBool("active");
                        kp.Bind = kpElem.GetAttrAsBool("bind");
                        kp.Number = kpElem.GetAttrAsInt("number");
                        kp.Name = kpElem.GetAttribute("name");
                        kp.Dll = kpElem.GetAttribute("dll");
                        kp.CallNum = kpElem.GetAttribute("callNum");
                        kp.CmdLine = kpElem.GetAttribute("cmdLine");
                        commLine.ReqSequence.Add(kp);

                        string address = kpElem.GetAttribute("address");
                        if (address != "")
                            kp.Address = kpElem.GetAttrAsInt("address");

                        kp.Timeout = kpElem.GetAttrAsInt("timeout");
                        kp.Delay = kpElem.GetAttrAsInt("delay");

                        string time = kpElem.GetAttribute("time");
                        if (time != "")
                            kp.Time = kpElem.GetAttrAsDateTime("time");

                        string period = kpElem.GetAttribute("period");
                        if (period != "")
                            kp.Period = kpElem.GetAttrAsTimeSpan("period");
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(string.Format(CommPhrases.IncorrectKPSettings, kpNumStr) +
                            ":" + Environment.NewLine + ex.Message);
                    }
                }
            }

            return commLine;
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

                // загрузка общих параметров
                LoadCommonParams(xmlDoc);
                // загрузка линий связи
                LoadCommLines(xmlDoc);

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
        /// Загрузить линию связи из файла настроек
        /// </summary>
        public bool LoadCommLine(string fileName, int lineNum, out CommLine commLine, out string errMsg)
        {
            commLine = null;
            errMsg = "";
            XmlDocument xmlDoc = null;

            try
            {
                if (!File.Exists(fileName))
                    throw new FileNotFoundException(string.Format(CommonPhrases.NamedFileNotFound, fileName));

                xmlDoc = new XmlDocument();
                xmlDoc.Load(fileName);

                XmlNode commLinesNode = xmlDoc.DocumentElement.SelectSingleNode("CommLines");
                if (commLinesNode != null)
                {
                    XmlNodeList commLineNodes = commLinesNode.SelectNodes("CommLine");
                    string lineNumStr = lineNum.ToString();

                    foreach (XmlElement commLineElem in commLineNodes)
                    {
                        if (commLineElem.GetAttribute("number").Trim() == lineNumStr)
                        {
                            commLine = LoadCommLine(commLineElem);
                            break;
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                errMsg = string.Format(Localization.UseRussian ? 
                    "Ошибка при загрузке конфигурации линии связи {0} из файла: {1}" : 
                    "Error loding communication line {0} configuration from file: {1}", lineNum, ex.Message);
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
                paramsElem.AppendParamElem("UpdateAllDataPer", Params.SendAllDataPer,
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
                    paramsElem.AppendParamElem("CmdEnabled", commLine.CmdEnabled,
                        "Команды ТУ разрешены", "Commands enabled");
                    paramsElem.AppendParamElem("DetailedLog", commLine.DetailedLog,
                        "Записывать в журнал подробную информацию", "Write detailed information to the log");

                    // пользовательские параметры
                    paramsElem = xmlDoc.CreateElement("UserParams"); // CustomParams
                    lineElem.AppendChild(paramsElem);
                    foreach (CustomParam param in commLine.CustomParams)
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
                errMsg = CommonPhrases.SaveAppSettingsError + ":" + Environment.NewLine + ex.Message;
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
