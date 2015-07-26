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
 * Module   : SCADA-Communicator Service
 * Summary  : Program execution management
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2006
 * Modified : 2015
 */

using Scada.Client;
using Scada.Comm;
using Scada.Comm.Devices;
using Scada.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Xml;
using Utils;

namespace Scada.Comm.Svc
{
    /// <summary>
    /// Program execution management
    /// <para>Управление работой программы</para>
    /// </summary>
    internal sealed class Manager
    {
        /// <summary>
        /// Версия приложения
        /// </summary>
        private const string AppVersion = "4.5.0.0";
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
        /// Задержка потока записи информации о работе приложения, мс
        /// </summary>
        private const int WriteInfoDelay = 1000;
        /// <summary>
        /// Интервал повторных попыток запуска потоков
        /// </summary>
        private static readonly TimeSpan StartAttemptSpan = TimeSpan.FromSeconds(10);

        private Dictionary<string, Type> kpTypes; // типы КП, полученные из подключаемых библиотек
        private List<CommLine> commLines;         // список активных линий связи
        private CommandReader commandReader;      // сервис приёма команд
        private Thread infoThread;                // поток записи информации о работе приложения
        private string infoFileName;              // полное имя файла информации
        private Thread startThread;               // поток повторения попыток запуска
        private DateTime startDT;                 // дата и время запуска работы
        private bool linesStarted;                // потоки линий связи запущены
        private string[] lineCaptions;            // обозначения линий связи

        private object lineLock;    // объект для синхронизации работы со списком линий связи
        private object lineCmdLock; // объект для синхронизации выполнения команд над линиями связи
        private object infoLock;    // объект для синхронизации записи информации о работе приложения


        /// <summary>
        /// Конструктор
        /// </summary>
        public Manager()
        {
            kpTypes = new Dictionary<string, Type>();
            commLines = new List<CommLine>();
            commandReader = null;
            infoThread = null;
            infoFileName = "";
            startThread = null;
            startDT = DateTime.MinValue;
            linesStarted = false;
            lineCaptions = null;

            lineLock = new object();
            lineCmdLock = new object();
            infoLock = new object();

            AppDirs = new AppDirs();
            Settings = new Settings();
            ServerComm = null;
            AppLog = new Log(Log.Formats.Full) { Encoding = Encoding.UTF8 };

            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
        }


        /// <summary>
        /// Получить директории приложения
        /// </summary>
        public AppDirs AppDirs { get; private set; }

        /// <summary>
        /// Получить настройки приложения
        /// </summary>
        public Settings Settings { get; private set; }

        /// <summary>
        /// Получить объект для обмена данными со SCADA-Сервером
        /// </summary>
        public ServerCommEx ServerComm { get; private set; }

        /// <summary>
        /// Получить основной журнал приложения
        /// </summary>
        public Log AppLog { get; private set; }


        /// <summary>
        /// Вывести информацию о необработанном исключении в журнал
        /// </summary>
        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs args)
        {
            Exception ex = args.ExceptionObject as Exception;
            AppLog.WriteAction(string.Format(Localization.UseRussian ? "Необработанное исключение{0}" :
                "Unhandled exception{0}", ex == null ? "" : ": " + ex.ToString()), Log.ActTypes.Exception);
            AppLog.WriteBreak();
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
        /// Распознать настройку линии связи и создать соответствующую линию связи
        /// </summary>
        private CommLine ParseCommLine(int lineNum, XmlElement commLineElem, out bool lineActive)
        {
            lineActive = false;
            return null;

            /*try
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
                            AppLog.WriteAction(string.Format(Localization.UseRussian ? 
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
                            AppLog.WriteAction(string.Format(Localization.UseRussian ?
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
                                {
                                    kpType = kpTypes[dllName];
                                }
                                else
                                {
                                    // загрузка типа из библиотеки
                                    string path = AppDirs.KPDir + dllName + ".dll";
                                    AppLog.WriteAction((Localization.UseRussian ? "Загрузка библиотеки КП: " :
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
                                KPLogic kpLogic;
                                try
                                {
                                    kpLogic = (KPLogic)Activator.CreateInstance(kpType, int.Parse(kpNumber));
                                }
                                catch (Exception ex)
                                {
                                    kpLogic = null;
                                    throw new Exception((Localization.UseRussian ? 
                                        "Ошибка при создании экземпляра класса КП: " : 
                                        "Error creating device class instance: ") + ex.Message);
                                }
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
                                AppLog.WriteAction(string.Format(Localization.UseRussian ?
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
                AppLog.WriteAction(string.Format(Localization.UseRussian ?
                    "Ошибка при распознавании конфигурации линии связи {0}: {1}" :
                    "Error parsing communication line {0} configuration: {1}",
                    lineNum, ex.Message), Log.ActTypes.Exception);
                lineActive = false;
                return null;
            }*/
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
                            // привязка тегов КП к входным каналам
                            tblInCnl.DefaultView.RowFilter = "KPNum = " + kpLogic.Number;
                            for (int i = 0; i < tblInCnl.DefaultView.Count; i++)
                            {
                                DataRowView rowView = tblInCnl.DefaultView[i];
                                kpLogic.BindTag((int)rowView["Signal"], (int)rowView["CnlNum"], 
                                    (int)rowView["ObjNum"], (int)rowView["ParamID"]);
                            }

                            // перезапись имени, адреса и позывного КП
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
                AppLog.WriteAction(string.Format(Localization.UseRussian ? 
                    "Ошибка при настройке линии связи {0}: {1}" :
                    "Error tuning communication line {0}: {1}", 
                    commLine.Number, ex.Message, Log.ActTypes.Exception));
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
        /// Инициализировать директории приложения
        /// </summary>
        private void InitAppDirs(out bool dirsExist, out bool logDirExists)
        {
            AppDirs.Init(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));

            AppLog.FileName = AppDirs.LogDir + LogFileName;
            infoFileName = AppDirs.LogDir + InfoFileName;
            logDirExists = Directory.Exists(AppDirs.LogDir);

            dirsExist = Directory.Exists(AppDirs.ConfigDir) && Directory.Exists(AppDirs.LangDir) && logDirExists &&
                Directory.Exists(AppDirs.KPDir) && Directory.Exists(AppDirs.CmdDir);
        }

        /// <summary>
        /// Распознать файл конфигурации, создать объекты линий связи и КП
        /// </summary>
        private bool ParseConfig()
        {
            string errMsg;
            if (Settings.Load(AppDirs.ConfigDir + Settings.DefFileName, out errMsg))
            {
                // создание линий связи и КП на основе загруженной конфигурации
                HashSet<int> lineNums = new HashSet<int>();
                foreach (Settings.CommLine commLineSett in Settings.CommLines)
                {
                    if (lineNums.Contains(commLineSett.Number))
                    {
                        AppLog.WriteAction(string.Format(Localization.UseRussian ?
                            "Линия связи {0} дублируется в файле конфигурации" :
                            "Communication line {0} is duplicated in the configuration file", commLineSett.Number),
                            Log.ActTypes.Error);
                    }
                    else if (commLineSett.Active)
                    {
                        CommLine commLine = CreateCommLine(commLineSett);
                        commLines.Add(commLine);
                        lineNums.Add(commLineSett.Number);
                    }
                }

                return true;
            }
            else
            {
                AppLog.WriteAction(errMsg, Log.ActTypes.Error);
                return false;
            }
        }

        /// <summary>
        /// Создать линию связи и КП на основе настроек
        /// </summary>
        private CommLine CreateCommLine(Settings.CommLine commLineSett)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Циклически записывать информацию о работе приложения, метод вызывается в отдельном потоке
        /// </summary>
        private void WriteInfoExecute()
        {
            while (true)
            {
                WriteInfo();
                Thread.Sleep(WriteInfoDelay);
            }
        }

        /// <summary>
        /// Записать в файл информацию о работе приложения
        /// </summary>
        private void WriteInfo()
        {
            Monitor.Enter(infoLock);

            try
            {
                StringBuilder sbInfo = new StringBuilder();

                TimeSpan workSpan = DateTime.Now - startDT;
                string workStr = workSpan.Days > 0 ? workSpan.ToString(@"d\.hh\:mm\:ss") :
                    workSpan.ToString(@"hh\:mm\:ss");

                if (Localization.UseRussian)
                {
                    sbInfo
                        .AppendLine("SCADA-Коммуникатор")
                        .AppendLine("------------------")
                        .Append("Запуск       : ").AppendLine(startDT.ToLocalizedString())
                        .Append("Время работы : ").AppendLine(workStr)
                        .Append("Версия       : ").AppendLine(AppVersion)
                        .Append("SCADA-Сервер : ").AppendLine(Settings.Params.ServerUse ?
                            (ServerComm == null ? "не инициализирован" : ServerComm.CommStateDescr) :
                            "не используется")
                        .AppendLine()
                        .AppendLine("Активные линии связи")
                        .AppendLine("--------------------");
                }
                else
                {
                    sbInfo
                        .AppendLine("SCADA-Communicator")
                        .AppendLine("------------------")
                        .Append("Started       : ").AppendLine(startDT.ToLocalizedString())
                        .Append("Execution time : ").AppendLine(workStr)
                        .Append("Version       : ").AppendLine(AppVersion)
                        .Append("SCADA-Server : ").AppendLine(Settings.Params.ServerUse ?
                            (ServerComm == null ? "not initialized" : ServerComm.CommStateDescr) :
                            "not used")
                        .AppendLine()
                        .AppendLine("Active Communication Lines")
                        .AppendLine("--------------------------");
                }

                lock (lineLock)
                {
                    int lineCnt = commLines.Count;
                    if (lineCnt > 0)
                    {
                        // инициализация обозначений линий связи
                        if (lineCaptions == null)
                            InitLineCaptions();

                        // вывод состояний работы активных линий связи
                        for (int i = 0; i < lineCnt; i++)
                            sbInfo.Append(lineCaptions[i]).AppendLine(commLines[i].RunStateStr);
                    }
                    else
                    {
                        sbInfo.AppendLine(Localization.UseRussian ? "Нет" : "No");
                    }
                }

                // запись в файл
                using (StreamWriter writer = new StreamWriter(infoFileName, false, Encoding.UTF8))
                    writer.Write(sbInfo.ToString());
            }
            catch (ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                AppLog.WriteAction((Localization.UseRussian ?
                    "Ошибка при записи в файл информации о работе приложения: " :
                    "Error writing application information to the file: ") + ex.Message, Log.ActTypes.Exception);
            }
            finally
            {
                Monitor.Exit(infoLock);
            }
        }

        /// <summary>
        /// Инициализировать обозначения линий связи
        /// </summary>
        private void InitLineCaptions()
        {
            // определение максимальной длины обозначения
            int maxCapLen = 0;
            int ordLen = commLines.Count.ToString().Length + 2 /*точка и пробел*/;
            foreach (CommLine commLine in commLines)
            {
                int lineCapLen = ordLen + commLine.Caption.Length;
                if (maxCapLen < lineCapLen)
                    maxCapLen = lineCapLen;
            }

            // заполение массива обозначений
            lineCaptions = new string[commLines.Count];
            int i = 0;
            foreach (CommLine commLine in commLines)
            {
                string lineCaption = (i + 1).ToString().PadLeft(ordLen) + ". " + commLine.Caption;
                lineCaptions[i] = lineCaption.PadRight(maxCapLen) + " : ";
                i++;
            }
        }


        /// <summary>
        /// Начать работу
        /// </summary>
        private bool StartOperation()
        {
            startDT = DateTime.Now;

            if (ParseConfig())
            {
                startThread = new Thread(new ThreadStart(TryToStartThreads));
                startThread.Start();
                return true;
            }
            else
            {
                WriteInfo();
                return false;
            }
        }

        /// <summary>
        /// Остановить работу
        /// </summary>
        private void StopOperation()
        {
            StopTryingToStart();
            StopThreads();
        }

        /// <summary>
        /// Попытаться запустить потоки, метод вызывается в отдельном потоке
        /// </summary>
        private void TryToStartThreads()
        {
            DateTime nowDT = DateTime.Now;
            while (!StartThreads())
            {
                DateTime retryDT = startDT + StartAttemptSpan;
                Thread.Sleep(retryDT - DateTime.Now);
            }
        }

        /// <summary>
        /// Остановить попытки запустить потоки
        /// </summary>
        private void StopTryingToStart()
        {
            if (startThread != null)
            {
                startThread.Abort();
                startThread = null;
            }
        }

        /// <summary>
        /// Запустить потоки линий связи и поток обмена данными со SCADA-Сервером
        /// </summary>
        private bool StartThreads()
        {
            // остановка потоков
            StopThreads();

            try
            {
                // приём необходимых таблиц базы конфигурации от SCADA-Сервера
                bool fatalError = false;
                DataTable tblInCnl;
                DataTable tblKP;

                if (Settings.Params.ServerUse)
                {
                    ServerComm = new ServerCommEx(Settings.Params, AppLog);
                    if (!ReceiveBaseTables(out tblInCnl, out tblKP))
                    {
                        fatalError = true;
                        AppLog.WriteAction(string.Format(Localization.UseRussian ?
                            "Запуск работы невозможен из-за проблем взаимодействия со SCADA-Сервером.{0}" + 
                            "Пауза перед следующей попыткой" :
                            "Unable to start operation due to SCADA-Server communication error.{0}" +
                            "Pause before trying again", Environment.NewLine), Log.ActTypes.Error);
                        ServerComm.Close();
                        ServerComm = null;
                    }
                }
                else
                {
                    tblInCnl = null;
                    tblKP = null;
                }

                if (!fatalError)
                {
                    // настройка линий связи по базе конфигурации
                    if (Settings.Params.ServerUse)
                    {
                        foreach (CommLine commLine in commLines)
                            TuneCommLine(commLine, tblInCnl, tblKP);
                    }

                    if (commLines.Count > 0)
                    {
                        // запуск потоков линий связи
                        AppLog.WriteAction(Localization.UseRussian ? 
                            "Запуск линий связи" : 
                            "Start communication lines", Log.ActTypes.Action);

                        lock (lineLock)
                        {
                            foreach (CommLine commLine in commLines)
                            {
                                commLine.ServerComm = ServerComm;
                                commLine.Start();
                            }
                            linesStarted = true;
                        }

                        // запуск приёма команд
                        AppLog.WriteAction(Localization.UseRussian ? 
                            "Запуск приёма команд" :
                            "Start receiving commands", Log.ActTypes.Action);
                        commandReader = new CommandReader(this);
                        commandReader.StartThread();
                    }
                    else
                    {
                        AppLog.WriteAction(Localization.UseRussian ?
                            "Отсутствуют активные линии связи" :
                            "No active communication lines", Log.ActTypes.Error);
                    }
                }

                // запуск потока записи информации о работе приложения
                if (linesStarted)
                {
                    infoThread = new Thread(new ThreadStart(WriteInfoExecute));
                    infoThread.Start();
                }

                return linesStarted;
            }
            catch (Exception ex)
            {
                AppLog.WriteAction((Localization.UseRussian ? "Ошибка при запуске работы: " :
                    "Error starting operation: ") + ex.Message, Log.ActTypes.Exception);
                return false;
            }
            finally
            {
                if (infoThread == null)
                    WriteInfo();
            }
        }

        /// <summary>
        /// Остановить потоки линий связи и поток обмена данными со SCADA-Сервером
        /// </summary>
        private void StopThreads()
        {
            try
            {
                // остановка потока записи информации о работе приложения
                if (infoThread != null)
                {
                    infoThread.Abort();
                    infoThread = null;
                }

                // остановка потока приёма команд
                if (commandReader != null)
                    commandReader.StopThread();

                if (commLines.Count > 0)
                {
                    AppLog.WriteAction(Localization.UseRussian ? "Остановка линий связи" :
                        "Stop communication lines", Log.ActTypes.Action);
                    linesStarted = false; // далее lock (lineLock) не требуется

                    // выполнение команд завершения работы линий связи
                    foreach (CommLine commLine in commLines)
                        commLine.Terminate();

                    // ожидание завершения работы линий связи
                    DateTime nowDT = DateTime.Now;
                    DateTime t0 = nowDT;
                    DateTime t1 = nowDT.AddMilliseconds(Settings.Params.WaitForStop);
                    bool running; // есть линия связи, продолжающая работу

                    do
                    {
                        running = false;
                        foreach (CommLine commLine in commLines)
                        {
                            if (!commLine.Terminated)
                            {
                                running = true;
                                Thread.Sleep(ScadaUtils.ThreadDelay);
                                break;
                            }
                        }
                        nowDT = DateTime.Now;
                    }
                    while (t0 <= nowDT && nowDT <= t1 && running);

                    // прерывание работы линий связи
                    if (running)
                    {
                        foreach (CommLine commLine in commLines)
                            commLine.Abort();
                    }
                }

                // завершение работы со SCADA-Сервером
                if (ServerComm != null)
                {
                    ServerComm.Close();
                    ServerComm = null;
                }

                // пауза для повышения вероятности полной остановки потоков
                Thread.Sleep(ScadaUtils.ThreadDelay);

                // запись информации о работе программы
                WriteInfo();
            }
            catch (Exception ex)
            {
                AppLog.WriteAction((Localization.UseRussian ? 
                    "Ошибка при остановке линий связи: " :
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
                        // загрузка линии связи из файла кофигурации
                        string errMsg;
                        Settings.CommLine commLineSett; 
                        if (Settings.LoadCommLine(AppDirs.ConfigDir + ConfigFileName, lineNum, 
                            out commLineSett, out errMsg))
                        {
                            if (commLineSett == null)
                            {
                                AppLog.WriteAction(string.Format(Localization.UseRussian ?
                                    "Невозможно запустить линию связи {0}, т.к. она не найдена в файле конфигурации" :
                                    "Unable to start communication line {0} because it is not found in the configuration file",
                                    lineNum), Log.ActTypes.Error);
                            }
                            else if (commLineSett.Active)
                            {
                                // создание линии связи
                                commLine = CreateCommLine(commLineSett);
                            }
                            else
                            {
                                AppLog.WriteAction(string.Format(Localization.UseRussian ?
                                    "Невозможно запустить линию связи {0}, т.к. она неактивна" :
                                    "Unable to start communication line {0} because it is not active",
                                    lineNum), Log.ActTypes.Error);
                            }
                        }
                        else
                        {
                            AppLog.WriteAction(errMsg);
                        }

                        // настройка линии связи
                        if (commLine != null && ServerComm != null)
                        {
                            DataTable tblInCnl;
                            DataTable tblKP;

                            if (ReceiveBaseTables(out tblInCnl, out tblKP))
                            {
                                TuneCommLine(commLine, tblInCnl, tblKP);
                            }
                            else
                            {
                                commLine = null;
                                AppLog.WriteAction(string.Format(Localization.UseRussian ?
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
                                AppLog.WriteAction((Localization.UseRussian ? 
                                    "Запуск линии связи " : 
                                    "Start communication line ") + lineNum, Log.ActTypes.Action);
                                commLine.ServerComm = ServerComm;
                                commLine.Start();

                                // добавление линии связи в список
                                lock (lineLock)
                                {
                                    commLines.Add(commLine);
                                    lineCaptions = null; // заново сформировать обозначения линии связи
                                }
                            }
                            catch (Exception ex)
                            {
                                AppLog.WriteAction((Localization.UseRussian ? 
                                    "Ошибка при запуске линии связи: " :
                                    "Error start communication line: ") + ex.Message, Log.ActTypes.Exception);
                            }
                        }
                    }
                    else
                    {
                        AppLog.WriteAction(string.Format(Localization.UseRussian ?
                            "Невозможно запустить линию связи {0}, т.к. она уже активна" :
                            "Unable to start communication line {0} because it is already active", 
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
                        AppLog.WriteAction(string.Format(Localization.UseRussian ?
                            "Невозможно остановить линию связи {0}, т.к. она не найдена среди активных линий связи" :
                            "Unable to stop communication line {0} because it is not found among the active lines",
                            lineNum), Log.ActTypes.Error);
                    }
                    else
                    {
                        try
                        {
                            AppLog.WriteAction((Localization.UseRussian ? 
                                "Остановка линии связи " :
                                "Stop communication line ") + lineNum, Log.ActTypes.Action);

                            // выполнение команды завершения работы линии связи
                            commLine.Terminate();

                            // ожидание завершения работы линии связи
                            DateTime nowDT = DateTime.Now;
                            DateTime t0 = nowDT;
                            DateTime t1 = nowDT.AddMilliseconds(Settings.Params.WaitForStop);

                            do
                            {
                                if (!commLine.Terminated)
                                    Thread.Sleep(ScadaUtils.ThreadDelay);
                                nowDT = DateTime.Now;
                            }
                            while (t0 <= nowDT && nowDT <= t1 && !commLine.Terminated);

                            // прерывание работы линии связи
                            if (!commLine.Terminated)
                                commLine.Abort();

                            // удаление линии связи из списка
                            lock (lineLock)
                            {
                                commLines.RemoveAt(lineInd);
                                lineCaptions = null; // заново сформировать обозначения линии связи
                            }
                        }
                        catch (Exception ex)
                        {
                            AppLog.WriteAction((Localization.UseRussian ? 
                                "Ошибка при остановке линии связи: " :
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
        public void PassCmd(Command cmd)
        {
            if (cmd != null && linesStarted)
            {
                try
                {
                    lock (lineLock)
                    {
                        // передача команды линиям связи
                        foreach (CommLine commLine in commLines)
                            commLine.AddCmd(cmd);
                    }
                }
                catch (Exception ex)
                {
                    AppLog.WriteAction((Localization.UseRussian ? 
                        "Ошибка при передаче команды менеджеру: " : 
                        "Error passing command to the manager: ") + ex.Message, Log.ActTypes.Exception);
                }
            }
        }


        /// <summary>
        /// Запустить службу
        /// </summary>
        public void StartService()
        {
            // инициализация необходимых директорий
            bool dirsExist;    // необходимые директории существуют
            bool logDirExists; // директория log-файлов существует
            InitAppDirs(out dirsExist, out logDirExists);

            if (logDirExists)
            {
                AppLog.WriteBreak();
                AppLog.WriteAction(Localization.UseRussian ? 
                    "Служба ScadaCommService запущена" :
                    "ScadaCommService is started", Log.ActTypes.Action);
            }

            if (dirsExist)
            {
                // локализация ScadaData.dll
                if (!Localization.UseRussian)
                {
                    string errMsg;
                    if (Localization.LoadDictionaries(AppDirs.LangDir, "ScadaData", out errMsg))
                        CommonPhrases.Init();
                    else
                        AppLog.WriteAction(errMsg, Log.ActTypes.Error);
                }

                // запуск работы
                if (!StartOperation())
                    AppLog.WriteAction(Localization.UseRussian ? 
                        "Нормальная работа программы невозможна." :
                        "Normal program execution is impossible.", Log.ActTypes.Error);
            }
            else
            {
                string errMsg = string.Format(Localization.UseRussian ?
                    "Не существуют необходимые директории:{0}{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}" +
                    "Нормальная работа программы невозможна." :
                    "Required directories are not exist:{0}{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}" +
                    "Normal program execution is impossible.",
                    Environment.NewLine, AppDirs.ConfigDir, AppDirs.LangDir, AppDirs.LogDir, 
                    AppDirs.KPDir, AppDirs.CmdDir);

                try
                {
                    if (EventLog.SourceExists("ScadaCommService"))
                        EventLog.WriteEvent("ScadaCommService",
                            new EventInstance(0, 0, EventLogEntryType.Error), errMsg);
                }
                catch { }

                if (logDirExists)
                    AppLog.WriteAction(errMsg, Log.ActTypes.Error);
            }
        }

        /// <summary>
        /// Остановить службу
        /// </summary>
        public void StopService()
        {
            StopOperation();
            AppLog.WriteAction(Localization.UseRussian ? 
                "Служба ScadaCommService остановлена" :
                "ScadaCommService is stopped", Log.ActTypes.Action);
            AppLog.WriteBreak();
        }

        /// <summary>
        /// Отключить службу немедленно при выключении компьютера
        /// </summary>
        public void ShutdownService()
        {
            StopOperation();
            AppLog.WriteAction(Localization.UseRussian ? 
                "Служба ScadaCommService отключена" :
                "ScadaCommService is shutdown", Log.ActTypes.Action);
            AppLog.WriteBreak();
        }
    }
}