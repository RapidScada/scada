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
 * Module   : SCADA-Communicator Service
 * Summary  : Program execution management
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2006
 * Modified : 2014
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Xml;
using Scada.Client;
using Scada.Comm.KP;
using Utils;

namespace Scada.Comm.Svc
{
    /// <summary>
    /// Program execution management
    /// <para>Управление работой программы</para>
    /// </summary>
    sealed class Manager
    {
        /// <summary>
        /// Общие параметры конфигурации
        /// </summary>
        public struct CommonParams
        {
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
        }


        /// <summary>
        /// Версия программы
        /// </summary>
        private const string Version = "4.4";
        /// <summary>
        /// Имя файла конфигурации
        /// </summary>
        private const string ConfigFileName = "ScadaCommSvcConfig.xml";
        /// <summary>
        /// Имя основного Log-файла программы
        /// </summary>
        private const string LogFileName = "ScadaCommSvc.log";
        /// <summary>
        /// Имя файла информации о работе программы
        /// </summary>
        private const string InfoFileName = "ScadaCommSvc.txt";
        /// <summary>
        /// Интервал повторных попыток запуска потоков линий связи и потока обмена данными, мс
        /// </summary>
        private const int StartAttemptSpan = 10000;


        private CommonParams commonParams;   // общие параметры конфигурации
        private List<CommLine> commLines;    // список активных линий связи
        private SortedList<string, Type> kpTypes; // типы КП, полученные из подключаемых библиотек
        private CommandReader commandReader; // приём команд
        private string infoFileName;         // полное имя файла информации
        private Timer infoTimer;             // таймер для обновления файла информации о работе программы
        private Timer startTimer;            // таймер для запуска потоков линий связи и потока обмена данными
        private DateTime startDT;            // дата и время запуска программы
        private bool linesStarted;           // потоки линий связи запущены
        private int maxLineCapLen;           // максимальная длина обозначения линии связи

        private object lineLock;             // объект для синхронизации работы со списком линий связи
        private object lineCmdLock;          // объект для синхронизации выполнения команд над линиями связи
        private object infoLock;             // объект для синхронизации записи в файл информации о работе программы


        /// <summary>
        /// Статический конструктор
        /// </summary>
        static Manager()
        {
            ExeDir = ScadaUtils.NormalDir(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            ConfigDir = "";
            LangDir = "";
            LogDir = "";
            KpDir = "";
            CmdDir = "";
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public Manager()
        {
            commonParams.SetToDefault();
            commLines = new List<CommLine>();
            kpTypes = new SortedList<string, Type>();
            commandReader = null;
            infoFileName = "";
            infoTimer = new Timer(InfoTimerCallback, null, Timeout.Infinite, Timeout.Infinite);
            startTimer = null;
            startDT = DateTime.Now;
            linesStarted = false;
            maxLineCapLen = -1;

            lineLock = new object();
            lineCmdLock = new object();
            infoLock = new object();

            ServerComm = null;
            Log = new Log(Log.Formats.Full);
            Log.Encoding = Encoding.UTF8;
        }


        /// <summary>
        /// Получить директорию исполняемого файла приложения
        /// </summary>
        public static string ExeDir { get; private set; }

        /// <summary>
        /// Получить директорию конфигурации программы
        /// </summary>
        public static string ConfigDir { get; private set; }

        /// <summary>
        /// Получить директорию языковых файлов приложения
        /// </summary>
        public static string LangDir { get; private set; }

        /// <summary>
        /// Получить директорию Log-файлов программы
        /// </summary>
        public static string LogDir { get; private set; }

        /// <summary>
        /// Получить директорию подключаемых библиотек КП
        /// </summary>
        public static string KpDir { get; private set; }

        /// <summary>
        /// Получить директорию команд
        /// </summary>
        public static string CmdDir { get; private set; }


        /// <summary>
        /// Получить объект для обмена данными со SCADA-Сервером
        /// </summary>
        public ServerCommEx ServerComm { get; private set; }

        /// <summary>
        /// Получить основной журнал приложения
        /// </summary>
        public Log Log { get; private set; }


        /// <summary>
        /// Распознать общие параметры из файла конфигурации
        /// </summary>
        private bool ParseCommonParams(XmlDocument xmlDoc)
        {
            XmlNode xmlNode = xmlDoc.DocumentElement.SelectSingleNode("CommonParams");

            if (xmlNode == null)
            {
                Log.WriteAction(Localization.UseRussian ? "В файле конфигурации не найдены общие параметры" : 
                    "Common parameters not found in the configuration file", Log.ActTypes.Error);
                return false;
            }
            else
            {
                // установка общих параметров по умолчанию
                commonParams.SetToDefault();

                // загрузка общих параметров из файла конфигурации
                XmlNodeList xmlNodeList = xmlNode.SelectNodes("Param");

                foreach (XmlElement xmlElement in xmlNodeList)
                {
                    try
                    {
                        string name = xmlElement.GetAttribute("name");
                        string val = xmlElement.GetAttribute("value");

                        if (name == "ServerUse")
                            commonParams.ServerUse = bool.Parse(val.ToLower());
                        else if (name == "ServerHost")
                            commonParams.ServerHost = val;
                        else if (name == "ServerPort")
                            commonParams.ServerPort = int.Parse(val);
                        else if (name == "ServerUser")
                            commonParams.ServerUser = val;
                        else if (name == "ServerPwd")
                            commonParams.ServerPwd = val;
                        else if (name == "ServerTimeout")
                            commonParams.ServerTimeout = int.Parse(val);
                        else if (name == "WaitForStop")
                            commonParams.WaitForStop = int.Parse(val);
                        else if (name == "RefrParams")
                            commonParams.RefrParams = int.Parse(val);
                    }
                    catch (Exception ex)
                    {
                        Log.WriteAction((Localization.UseRussian ? "Ошибка при распознавании общих параметров: " : 
                            "Error parsing common parameters: ") + ex.Message, Log.ActTypes.Exception);
                    }
                }

                return true;
            }
        }

        /// <summary>
        /// Распознать настройку линий связи из файла конфигурации
        /// </summary>
        private bool ParseCommLines(XmlDocument xmlDoc)
        {
            XmlNode commLinesNode = xmlDoc.DocumentElement.SelectSingleNode("CommLines");

            if (commLinesNode == null)
            {
                Log.WriteAction(Localization.UseRussian ? "В файле конфигурации не найдены линии связи" :
                    "Communication lines not found in the configuration file", Log.ActTypes.Error);
                return false;
            }
            else
            {
                SortedList<int, object> lineNums = new SortedList<int, object>(); // номера линий связи
                XmlNodeList commLineNodes = commLinesNode.SelectNodes("CommLine");

                foreach (XmlElement commLineElem in commLineNodes)
                {
                    int lineNum = int.Parse(commLineElem.GetAttribute("number"));
                    if (lineNums.ContainsKey(lineNum))
                    {
                        Log.WriteAction(string.Format(Localization.UseRussian ? 
                            "Линия связи {0} дублируется в файле конфигурации" : 
                            "Communication line {0} is duplicated in the configuration file", lineNum), 
                            Log.ActTypes.Error);
                        continue;
                    }

                    bool lineActive;
                    CommLine commLine = ParseCommLine(lineNum, commLineElem, out lineActive);
                    if (commLine != null)
                    {
                        commLines.Add(commLine);
                        lineNums.Add(lineNum, null);
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Распознать настройку линии связи и создать соответствующую линию связи
        /// </summary>
        private CommLine ParseCommLine(int lineNum, XmlElement commLineElem, out bool lineActive)
        {
            try
            {
                lineActive = bool.Parse(commLineElem.GetAttribute("active"));
                if (!lineActive)
                    return null;

                string lineName = commLineElem.GetAttribute("name");
                bool lineBind = bool.Parse(commLineElem.GetAttribute("bind"));
                CommLine commLine = new CommLine(lineNum, lineName, lineBind);
                commLine.PassCmd = PassCmd;

                // получение настроек соединения линии связи
                XmlElement connElem = commLineElem.SelectSingleNode("Connection") as XmlElement;
                if (connElem != null)
                {
                    XmlElement connTypeElem = connElem.SelectSingleNode("ConnType") as XmlElement;
                    XmlElement commSettElem = connElem.SelectSingleNode("ComPortSettings") as XmlElement;
                    if (connTypeElem != null && connTypeElem.GetAttribute("value").ToLower() == "comport" &&
                        commSettElem != null)
                    {
                        try
                        {
                            string portName = commSettElem.GetAttribute("portName");
                            string baudRate = commSettElem.GetAttribute("baudRate");
                            string dataBits = commSettElem.GetAttribute("dataBits");
                            string parity = commSettElem.GetAttribute("parity");
                            string stopBits = commSettElem.GetAttribute("stopBits");
                            bool dtrEnable = commSettElem.GetAttribute("dtrEnable").ToLower() == "true";
                            bool rtsEnable = commSettElem.GetAttribute("rtsEnable").ToLower() == "true";

                            commLine.SerialPort = new SerialPort(portName, int.Parse(baudRate),
                                (Parity)Enum.Parse(typeof(Parity), parity, true), int.Parse(dataBits),
                                (StopBits)Enum.Parse(typeof(StopBits), stopBits, true));
                            commLine.SerialPort.DtrEnable = dtrEnable;
                            commLine.SerialPort.RtsEnable = rtsEnable;
                        }
                        catch (Exception ex)
                        {
                            commLine.ConfigError = true;
                            Log.WriteAction(string.Format(Localization.UseRussian ? 
                                "Ошибка при распознавании параметров COM-порта линии связи {0}: {1}" : 
                                "Error parsing serial port parameters of communication line {0}: {1}", 
                                lineNum, ex.Message), Log.ActTypes.Exception);
                        }
                    }
                }

                // получение параметров линии связи
                XmlElement lineParamsElem = commLineElem.SelectSingleNode("LineParams") as XmlElement;
                if (lineParamsElem != null)
                {
                    XmlNodeList paramNodes = lineParamsElem.SelectNodes("Param");
                    foreach (XmlElement paramElem in paramNodes)
                    {
                        try
                        {
                            string name = paramElem.GetAttribute("name");
                            string val = paramElem.GetAttribute("value");

                            if (name == "ReqTriesCnt")
                                commLine.ReqTriesCnt = int.Parse(val);
                            else if (name == "CycleDelay")
                                commLine.CicleDelay = int.Parse(val);
                            else if (name == "MaxCommErrCnt")
                                commLine.MaxCommErrCnt = int.Parse(val);
                            else if (name == "CmdEnabled")
                                commLine.CmdEnabled = bool.Parse(val.ToLower());
                        }
                        catch (Exception ex)
                        {
                            commLine.ConfigError = true;
                            Log.WriteAction(string.Format(Localization.UseRussian ?
                                "Ошибка при распознавании параметров линии связи {0}: {1}" :
                                "Error parsing communication line {0} parameters: {1}", 
                                lineNum, ex.Message), Log.ActTypes.Exception);
                        }
                    }
                    commLine.RefrParams = commonParams.RefrParams;
                }

                // получение пользовательских параметров линии связи
                XmlElement userParamsElem = commLineElem.SelectSingleNode("UserParams") as XmlElement;
                if (userParamsElem != null)
                {
                    XmlNodeList paramNodes = userParamsElem.SelectNodes("Param");
                    foreach (XmlElement paramElem in paramNodes)
                    {
                        string name = paramElem.GetAttribute("name");
                        string val = paramElem.GetAttribute("value");
                        if (name != "" && !commLine.UserParams.ContainsKey(name))
                            commLine.UserParams.Add(name, val);
                    }
                }

                // получение последовательности опроса линии связи
                XmlElement reqSeqElem = commLineElem.SelectSingleNode("ReqSequence") as XmlElement;
                if (reqSeqElem != null)
                {
                    XmlNodeList kpNodes = reqSeqElem.SelectNodes("KP");
                    foreach (XmlElement kpElem in kpNodes)
                    {
                        string kpActive = kpElem.GetAttribute("active").ToLower();
                        if (kpActive == "true")
                        {
                            string kpNumber = kpElem.GetAttribute("number");
                            try
                            {
                                // получение типа КП
                                string dllName = kpElem.GetAttribute("dll");
                                string typeFullName = "Scada.Comm.KP." + dllName + "Logic";
                                Type kpType;
                                if (kpTypes.ContainsKey(dllName))
                                    kpType = kpTypes[dllName];
                                else
                                {
                                    // загрузка типа из библиотеки
                                    string path = KpDir + dllName + ".dll";
                                    Log.WriteAction((Localization.UseRussian ? "Загрузка библиотеки КП: " : 
                                        "Load device library: ") + path, Log.ActTypes.Action);

                                    Assembly asm = Assembly.LoadFile(path);
                                    kpType = asm.GetType(typeFullName);
                                    kpTypes.Add(dllName, kpType);
                                }

                                // получение параметров КП
                                bool kpBind = bool.Parse(kpElem.GetAttribute("bind"));

                                KPLogic.ReqParams reqParams = new KPLogic.ReqParams();
                                reqParams.Timeout = int.Parse(kpElem.GetAttribute("timeout"));
                                reqParams.Delay = int.Parse(kpElem.GetAttribute("delay"));
                                string time = kpElem.GetAttribute("time");
                                reqParams.Time = time == "" ? DateTime.MinValue :
                                    DateTime.Parse(time, CultureInfo.CurrentCulture);
                                string period = kpElem.GetAttribute("period");
                                reqParams.Period = period == "" ? new TimeSpan(0) : TimeSpan.Parse(period);
                                string group = kpElem.GetAttribute("group");
                                reqParams.CmdLine = kpElem.GetAttribute("cmdLine");

                                // создание экземпляра класса КП
                                KPLogic kpLogic = Activator.CreateInstance(kpType, int.Parse(kpNumber)) as KPLogic;
                                kpLogic.Bind = kpBind;
                                kpLogic.Name = kpElem.GetAttribute("name");
                                string address = kpElem.GetAttribute("address");
                                kpLogic.Address = address == "" ? 0 : int.Parse(address);
                                kpLogic.CallNum = kpElem.GetAttribute("callNum");
                                kpLogic.KPReqParams = reqParams;
                                commLine.AddKP(kpLogic);
                            }
                            catch (Exception ex)
                            {
                                Log.WriteAction(string.Format(Localization.UseRussian ?
                                    "Ошибка при распознавании конфигурации КП {0}: {1}" :
                                    "Error parsing device {0} configuration: {1}",
                                    kpNumber, ex.Message), Log.ActTypes.Exception);
                            }
                        }
                    }
                }

                return commLine;
            }
            catch (Exception ex)
            {
                Log.WriteAction(string.Format(Localization.UseRussian ?
                    "Ошибка при распознавании конфигурации линии связи {0}: {1}" :
                    "Error parsing communication line {0} configuration: {1}",
                    lineNum, ex.Message), Log.ActTypes.Exception);
                lineActive = false;
                return null;
            }
        }

        /// <summary>
        /// Принять таблицу входных каналов и таблицу КП от SCADA-Сервера
        /// </summary>
        private bool ReceiveBaseTables(out DataTable tblInCnl, out DataTable tblKP)
        {
            tblInCnl = new DataTable();
            tblKP = new DataTable();

            if (ServerComm.ReceiveBaseTable("incnl.dat", tblInCnl) &&
                ServerComm.ReceiveBaseTable("kp.dat", tblKP))
            {
                return true;
            }
            else
            {
                tblInCnl = null;
                tblKP = null;
                return false;
            }
        }

        /// <summary>
        /// Настроить линию связи в соответствии с таблицами базы конфигурации
        /// </summary>
        private void TuneCommLine(CommLine commLine, DataTable tblInCnl, DataTable tblKP)
        {
            try
            {
                if (commLine.Bind)
                {
                    foreach (KPLogic kpLogic in commLine.KPList)
                    {
                        if (kpLogic.Bind)
                        {
                            // привязка параметров КП к входным каналам
                            tblInCnl.DefaultView.RowFilter = "KPNum = " + kpLogic.Number;
                            for (int i = 0; i < tblInCnl.DefaultView.Count; i++)
                            {
                                DataRowView rowView = tblInCnl.DefaultView[i];
                                if ((bool)rowView["Active"])
                                    kpLogic.BindParam((int)rowView["Signal"], (int)rowView["CnlNum"], 
                                        (int)rowView["ObjNum"], (int)rowView["ParamID"]);
                            }

                            // определение имени и адреса КП
                            tblKP.DefaultView.RowFilter = "KPNum = " + kpLogic.Number;
                            if (tblKP.DefaultView.Count > 0)
                            {
                                DataRowView rowKP = tblKP.DefaultView[0];
                                kpLogic.Name = (string)rowKP["Name"];
                                kpLogic.Address = (int)rowKP["Address"];
                                kpLogic.CallNum = (string)rowKP["CallNum"];
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                commLine.ConfigError = true;
                Log.WriteAction(string.Format(Localization.UseRussian ? "Ошибка при настройке линии связи {0}: {1}" :
                    "Error tuning communication line {0}: {1}", commLine.Number, ex.Message, Log.ActTypes.Exception));
            }
        }

        /// <summary>
        /// Найти линию связи по номеру и определить её индекс
        /// </summary>
        private CommLine FindCommLine(int lineNum, out int lineInd)
        {
            int lineCnt = commLines.Count;
            lineInd = -1;

            for (int i = 0; i < lineCnt; i++)
            {
                CommLine commLine = commLines[i];
                if (commLine.Number == lineNum)
                {
                    lineInd = i;
                    return commLine;
                }
            }

            return null;
        }
        
        /// <summary>
        /// Записать в файл информацию о работе программы
        /// </summary>
        private void WriteInfo()
        {
            Monitor.Enter(infoLock);
            StreamWriter writer = null;

            try
            {
                writer = new StreamWriter(infoFileName, false, Encoding.UTF8);

                TimeSpan workSpan = DateTime.Now - startDT;
                string workStr = workSpan.Days > 0 ? workSpan.ToString(@"d\.hh\:mm\:ss") : 
                    workSpan.ToString(@"hh\:mm\:ss");

                if (Localization.UseRussian)
                {
                    writer.WriteLine("SCADA-Коммуникатор");
                    writer.WriteLine("------------------");
                    writer.WriteLine("Запуск       : " + startDT.ToLocalizedString());
                    writer.WriteLine("Время работы : " + workStr);
                    writer.WriteLine("Версия       : " + Version);
                    string serverInfo = commonParams.ServerUse ?
                        (ServerComm == null ? "не инициализирован" : ServerComm.CommStateDescr) : "не используется";
                    writer.WriteLine("SCADA-Сервер : " + serverInfo);
                    writer.WriteLine();
                    writer.WriteLine("Активные линии связи");
                    writer.WriteLine("--------------------");
                }
                else
                {
                    writer.WriteLine("SCADA-Communicator");
                    writer.WriteLine("------------------");
                    writer.WriteLine("Started        : " + startDT.ToLocalizedString());
                    writer.WriteLine("Execution time : " + workStr);
                    writer.WriteLine("Version        : " + Version);
                    string serverInfo = commonParams.ServerUse ?
                        (ServerComm == null ? "not initialized" : ServerComm.CommStateDescr) : "not used";
                    writer.WriteLine("SCADA-Server   : " + serverInfo);
                    writer.WriteLine();
                    writer.WriteLine("Active Communication Lines");
                    writer.WriteLine("--------------------------");
                }

                lock (lineLock)
                {
                    int lineCnt = commLines.Count;
                    if (lineCnt > 0)
                    {
                        // вычисление максимальной длины обозначения линии связи
                        int ordLen = commLines.Count.ToString().Length;
                        if (maxLineCapLen < 0)
                        {
                            for (int i = 0; i < lineCnt; i++)
                            {
                                CommLine commLine = commLines[i];
                                int lineCapLen = ordLen + 2 + commLine.Caption.Length;
                                if (maxLineCapLen < lineCapLen)
                                    maxLineCapLen = lineCapLen;
                            }
                        }

                        // вывод состояний работы активных линий связи
                        for (int i = 0; i < lineCnt; i++)
                        {
                            CommLine commLine = commLines[i];
                            string lineCaption = (i + 1).ToString().PadLeft(ordLen) + ". " + commLine.Caption;
                            writer.WriteLine(lineCaption.PadRight(maxLineCapLen) + " : " + commLine.RunStateStr);
                        }
                    }
                    else
                    {
                        writer.WriteLine(Localization.UseRussian ? "Нет" : "No");
                    }
                }
            }
            catch (Exception ex)
            {
                Log.WriteAction((Localization.UseRussian ?
                    "Ошибка при записи в файл информации о работе приложения: " :
                    "Error writing application information to the file: ") + ex.Message, Log.ActTypes.Exception);
            }
            finally
            {
                if (writer != null)
                {
                    try { writer.Close(); }
                    catch { }
                }
                Monitor.Exit(infoLock);
            }
        }

        /// <summary>
        /// Обработать срабатывание таймера для обновления файла информации о работе программы
        /// </summary>
        private void InfoTimerCallback(Object state)
        {
            WriteInfo();
        }

        /// <summary>
        /// Обработать срабатывание таймера для запуска потоков линий связи и потока обмена данными
        /// </summary>
        private void StartTimerCallback(Object state)
        {
            try
            {
                // остановка таймера
                startTimer.Change(Timeout.Infinite, Timeout.Infinite);

                // установка соединения со SCADA-Сервером и приём таблиц базы конфигурации
                bool serverUse = commonParams.ServerUse && commonParams.ServerHost != "" && commonParams.ServerPort > 0;
                bool serverOk;
                DataTable tblInCnl = null;
                DataTable tblKP = null;

                if (serverUse)
                {
                    ServerComm = new ServerCommEx(commonParams, Log);
                    serverOk = ReceiveBaseTables(out tblInCnl, out tblKP);
                }
                else
                {
                    serverOk = false;
                }

                if (!serverUse || serverOk)
                {
                    // настройка линий связи
                    lock (lineLock)
                    {
                        if (serverUse)
                        {
                            foreach (CommLine commLine in commLines)
                                TuneCommLine(commLine, tblInCnl, tblKP);
                        }

                        // запуск потоков линий связи и приёма команд
                        Log.WriteAction(Localization.UseRussian ? "Запуск линий связи" : 
                            "Start communication lines", Log.ActTypes.Action);
                        foreach (CommLine commLine in commLines)
                        {
                            commLine.ServerComm = ServerComm;
                            commLine.StartThread();
                        }

                        if (ServerComm != null || CmdDir != "")
                        {
                            Log.WriteAction(Localization.UseRussian ? "Запуск приёма команд" : 
                                "Start receiving commands", Log.ActTypes.Action);
                            commandReader = new CommandReader(this);
                            commandReader.StartThread();
                        }

                        linesStarted = true;
                        startTimer = null;
                    }
                }
                else
                {
                    Log.WriteAction(Localization.UseRussian ? 
                        "Запуск линий связи невозможен из-за проблем взаимодействия со SCADA-Сервером" :
                        "Unable to start communication lines due to SCADA-Server communication error",
                        Log.ActTypes.Error);

                    // завершение работы со SCADA-Сервером
                    if (ServerComm != null)
                    {
                        ServerComm.Close();
                        ServerComm = null;
                    }
                    startTimer.Change(StartAttemptSpan, Timeout.Infinite);
                }
            }
            catch (Exception ex)
            {
                Log.WriteAction((Localization.UseRussian ? "Ошибка при запуске: " : 
                    "Start error: ") + ex.Message, Log.ActTypes.Exception);
            }
        }


        /// <summary>
        /// Инициализировать директории приложения
        /// </summary>
        public void InitAppDirs(out bool dirsExist, out bool logDirExists)
        {
            ConfigDir = ExeDir + "Config\\";
            LangDir = ExeDir + "Lang\\";
            LogDir = ExeDir + "Log\\";
            KpDir = ExeDir + "KP\\";
            CmdDir = ExeDir + "Cmd\\";

            Log.FileName = LogDir + LogFileName;
            infoFileName = LogDir + InfoFileName;
            logDirExists = Directory.Exists(LogDir);

            if (Directory.Exists(ConfigDir) && Directory.Exists(LangDir) && logDirExists &&
                Directory.Exists(KpDir) && Directory.Exists(CmdDir))
            {
                infoTimer.Change(0, 1000); // запуск таймера для обновления файла информации о работе программы
                dirsExist = true;
            }
            else
            {
                dirsExist = false;
            }
        }

        /// <summary>
        /// Распознать файл конфигурации, создать объекты линий связи и КП
        /// </summary>
        public bool ParseConfig()
        {
            try
            {
                string fileName = ConfigDir + ConfigFileName;
                Log.WriteAction((Localization.UseRussian ? "Чтение конфигурации из файла " : 
                    "Read configuration from file ") + fileName, Log.ActTypes.Action);

                XmlDocument xmlDoc = new XmlDocument(); // обрабатываемый XML-документ
                xmlDoc.Load(fileName);

                // чтение общих параметров
                if (!ParseCommonParams(xmlDoc))
                    return false;

                // чтение конфигурации линий связи
                if (!ParseCommLines(xmlDoc))
                    return false;
            }
            catch (Exception ex)
            {
                Log.WriteAction((Localization.UseRussian ? "Ошибка при чтении конфигурации из файла: " : 
                    "Error reading configuration from file: ") + ex.Message, Log.ActTypes.Exception);
                return false;
            }
            return true;
        }


        /// <summary>
        /// Запустить потоки линий связи и поток обмена данными со SCADA-Сервером
        /// </summary>
        public void StartThreads()
        {
            startTimer = new Timer(StartTimerCallback, null, 0, Timeout.Infinite);
        }

        /// <summary>
        /// Остановить потоки линий связи и поток обмена данными со SCADA-Сервером
        /// </summary>
        public void StopThreads()
        {
            try
            {
                // прерывание потока приёма команд
                if (commandReader != null && commandReader.Thread != null)
                    commandReader.Thread.Abort();

                if (commLines.Count > 0)
                {
                    Log.WriteAction(Localization.UseRussian ? "Остановка линий связи" :
                        "Stop communication lines", Log.ActTypes.Action);
                    linesStarted = false; // далее lock (lineLock) не требуется

                    // выполнение команд завершения работы линий связи
                    foreach (CommLine commLine in commLines)
                        commLine.Terminate();

                    // ожидание завершения работы линий связи
                    DateTime nowDT = DateTime.Now;
                    DateTime t0 = nowDT;
                    DateTime t1 = nowDT.AddMilliseconds(commonParams.WaitForStop);
                    bool running; // есть линия связи, продолжающая работу

                    do
                    {
                        running = false;
                        foreach (CommLine commLine in commLines)
                        {
                            if (commLine.RunState != CommLine.RunStates.Terminated)
                            {
                                running = true;
                                break;
                            }
                        }
                        if (running)
                            Thread.Sleep(200);
                        nowDT = DateTime.Now;
                    }
                    while (t0 <= nowDT && nowDT <= t1 && running);

                    // прерывание работы линий связи
                    if (running)
                    {
                        foreach (CommLine commLine in commLines)
                            if (commLine.AbortAllowed)
                                commLine.Thread.Abort();
                    }
                }

                // завершение работы со SCADA-Сервером
                if (ServerComm != null)
                {
                    ServerComm.Close();
                    ServerComm = null;
                }

                // пауза для повышения вероятности полной остановки потоков
                Thread.Sleep(500);

                // запись в файл информации о работе программы
                WriteInfo();
            }
            catch (Exception ex)
            {
                Log.WriteAction((Localization.UseRussian ? "Ошибка при остановке линий связи: " :
                    "Error stop communication lines: ") + ex.Message, Log.ActTypes.Exception);
            }
        }


        /// <summary>
        /// Запустить линию связи
        /// </summary>
        public void StartCommLine(int lineNum)
        {
            lock (lineCmdLock)
            {
                if (linesStarted)
                {
                    // поиск линии связи
                    int lineInd;
                    CommLine commLine = FindCommLine(lineNum, out lineInd);

                    if (commLine == null)
                    {
                        // считывание параметров линии связи из файла кофигурации и создание линии
                        try
                        {
                            XmlDocument xmlDoc = new XmlDocument();
                            xmlDoc.Load(ConfigDir + ConfigFileName);

                            XmlNode commLinesNode = xmlDoc.DocumentElement.SelectSingleNode("CommLines");
                            if (commLinesNode != null)
                            {
                                XmlNodeList commLineNodes = commLinesNode.SelectNodes("CommLine");
                                string lineNumStr = lineNum.ToString();
                                bool lineFound = false;
                                bool lineActive = false;

                                foreach (XmlElement commLineElem in commLineNodes)
                                {
                                    if (commLineElem.GetAttribute("number").Trim() == lineNumStr)
                                    {
                                        commLine = ParseCommLine(lineNum, commLineElem, out lineActive);
                                        lineFound = true;
                                        break;
                                    }
                                }

                                if (lineFound)
                                {
                                    if (!lineActive)
                                    {
                                        commLine = null;
                                        Log.WriteAction(string.Format(Localization.UseRussian ? 
                                            "Невозможно запустить линию связи {0}, т.к. она неактивна" : 
                                            "Unable to start communication line {0} because it is not active", 
                                            lineNumStr), Log.ActTypes.Error);
                                    }
                                }
                                else
                                {
                                    Log.WriteAction(string.Format(Localization.UseRussian ?
                                        "Невозможно запустить линию связи {0}, т.к. она не найдена в файле конфигурации" :
                                        "Unable to start communication line {0} because it is not found in the configuration file",
                                        lineNumStr), Log.ActTypes.Error);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.WriteAction((Localization.UseRussian ? 
                                "Ошибка при чтении конфигурации линии связи из файла: " : 
                                "Error reading communication line configuration from file: ") + 
                                ex.Message, Log.ActTypes.Exception);
                        }

                        // настройка линии связи
                        if (commLine != null && ServerComm != null)
                        {
                            DataTable tblInCnl = null;
                            DataTable tblKP = null;

                            if (ReceiveBaseTables(out tblInCnl, out tblKP))
                            {
                                TuneCommLine(commLine, tblInCnl, tblKP);
                            }
                            else
                            {
                                commLine = null;
                                Log.WriteAction(string.Format(Localization.UseRussian ?
                                    "Невозможно запустить линию связи {0} из-за проблем взаимодействия со SCADA-Сервером" :
                                    "Unable to start communication line {0} due to SCADA-Server communication error",
                                    lineNum), Log.ActTypes.Error);
                            }
                        }

                        // запуск линии связи
                        if (commLine != null)
                        {
                            try
                            {
                                Log.WriteAction((Localization.UseRussian ? "Запуск линии связи " : 
                                    "Start communication line ") + lineNum, Log.ActTypes.Action);
                                commLine.ServerComm = ServerComm;
                                commLine.StartThread();

                                // добавление линии связи в список
                                lock (lineLock)
                                {
                                    commLines.Add(commLine);
                                    maxLineCapLen = -1; // пересчитать максимальную длину обозначения линии связи
                                }
                            }
                            catch (Exception ex)
                            {
                                Log.WriteAction((Localization.UseRussian ? "Ошибка при запуске линии связи: " :
                                    "Error start communication line: ") + ex.Message, Log.ActTypes.Exception);
                            }
                        }
                    }
                    else
                    {
                        Log.WriteAction(string.Format(Localization.UseRussian ?
                            "Невозможно запустить линию связи {0}, т.к. она активна" :
                            "Unable to start communication line {0} because it is active", 
                            lineNum), Log.ActTypes.Error);
                    }
                }
            }
        }

        /// <summary>
        /// Остановить линию связи
        /// </summary>
        public void StopCommLine(int lineNum)
        {
            lock (lineCmdLock)
            {
                if (linesStarted)
                {
                    // поиск линии связи
                    int lineInd;
                    CommLine commLine = FindCommLine(lineNum, out lineInd);

                    if (commLine == null)
                    {
                        Log.WriteAction(string.Format(Localization.UseRussian ?
                            "Невозможно остановить линию связи {0}, т.к. она не найдена среди активных линий связи" :
                            "Unable to stop communication line {0} because it is not found among the active lines",
                            lineNum), Log.ActTypes.Error);
                    }
                    else
                    {
                        try
                        {
                            Log.WriteAction((Localization.UseRussian ? "Остановка линии связи " :
                                "Stop communication line ") + lineNum, Log.ActTypes.Action);

                            // выполнение команды завершения работы линии связи
                            commLine.Terminate();

                            // ожидание завершения работы линии связи
                            DateTime nowDT = DateTime.Now;
                            DateTime t0 = nowDT;
                            DateTime t1 = nowDT.AddMilliseconds(commonParams.WaitForStop);
                            bool running; // линия связи продолжает работу

                            do
                            {
                                running = commLine.RunState != CommLine.RunStates.Terminated;
                                if (running)
                                    Thread.Sleep(200);
                                nowDT = DateTime.Now;
                            }
                            while (t0 <= nowDT && nowDT <= t1 && running);

                            // прерывание работы линии связи
                            if (running && commLine.AbortAllowed)
                                commLine.Thread.Abort();

                            // удаление линии связи из списка
                            lock (lineLock)
                            {
                                commLines.RemoveAt(lineInd);
                                maxLineCapLen = -1; // пересчитать максимальную длину обозначения линии связи
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.WriteAction((Localization.UseRussian ? "Ошибка при остановке линии связи: " :
                                "Error stop communication line: ") + ex.Message, Log.ActTypes.Exception);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Перезапустить линию связи
        /// </summary>
        public void RestartCommLine(int lineNum)
        {
            StopCommLine(lineNum);
            StartCommLine(lineNum);
        }


        /// <summary>
        /// Передать команду КП
        /// </summary>
        public void PassCmd(KPLogic.Command cmd)
        {
            if (cmd != null && linesStarted)
            {
                try
                {
                    lock (lineLock)
                    {
                        foreach (CommLine commLine in commLines)
                            commLine.AddCmd(cmd);
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteAction((Localization.UseRussian ? "Ошибка при передаче команды менеджеру: " : 
                        "Error passing command to the manager: ") + ex.Message, Log.ActTypes.Exception);
                }
            }
        }
    }
}