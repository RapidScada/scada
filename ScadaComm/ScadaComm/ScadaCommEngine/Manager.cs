/*
 * Copyright 2019 Mikhail Shiryaev
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
 * Module   : ScadaCommEngine
 * Summary  : Program execution management
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2006
 * Modified : 2019
 */

using Scada.Data.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using Utils;

namespace Scada.Comm.Engine
{
    /// <summary>
    /// Program execution management.
    /// <para>Управление работой программы.</para>
    /// </summary>
    public sealed class Manager
    {
        /// <summary>
        /// Names of work states.
        /// <para>Наименования состояний работы.</para>
        /// </summary>
        private static class WorkStateNames
        {
            static WorkStateNames()
            {
                if (Localization.UseRussian)
                {
                    Normal = "норма";
                    Stopped = "остановлен";
                    Error = "ошибка";
                }
                else
                {
                    Normal = "normal";
                    Stopped = "stopped";
                    Error = "error";
                }
            }

            public static readonly string Normal;
            public static readonly string Stopped;
            public static readonly string Error;
        }

        /// <summary>
        /// Задержка потока записи информации о работе приложения, мс
        /// </summary>
        private const int WriteInfoDelay = 1000;
        /// <summary>
        /// Задержка перед повторной попыткой запуска потоков, мс
        /// </summary>
        private const int StartRetryDelay = 10000;

        private readonly Dictionary<string, Type> kpTypes; // типы КП, полученные из подключаемых библиотек
        private readonly List<CommLine> commLines; // список активных линий связи
        private readonly object lineCmdLock;       // объект для синхронизации выполнения команд над линиями связи

        private CommandReader commandReader; // сервис приёма команд
        private Thread infoThread;           // поток записи информации о работе приложения
        private string infoFileName;         // полное имя файла информации
        private Thread startThread;          // поток повторения попыток запуска
        private DateTime startDT;            // дата и время запуска работы
        private string workState;            // состояние работы
        private bool linesStarted;           // потоки линий связи запущены
        private string[] lineCaptions;       // обозначения линий связи


        /// <summary>
        /// Конструктор
        /// </summary>
        public Manager()
        {
            kpTypes = new Dictionary<string, Type>();
            commLines = new List<CommLine>();
            lineCmdLock = new object();

            commandReader = null;
            infoThread = null;
            infoFileName = "";
            startThread = null;
            startDT = DateTime.MinValue;
            workState = "";
            linesStarted = false;
            lineCaptions = null;

            AppDirs = new AppDirs();
            Settings = new Settings();
            ServerComm = null;
            AppLog = new Log(Log.Formats.Full) { Encoding = Encoding.UTF8 };

            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
        }


        /// <summary>
        /// Получить директории приложения
        /// </summary>
        internal AppDirs AppDirs { get; private set; }

        /// <summary>
        /// Получить настройки приложения
        /// </summary>
        internal Settings Settings { get; private set; }

        /// <summary>
        /// Получить объект для обмена данными со SCADA-Сервером
        /// </summary>
        internal ServerCommEx ServerComm { get; private set; }

        /// <summary>
        /// Получить основной журнал приложения
        /// </summary>
        internal Log AppLog { get; private set; }


        /// <summary>
        /// Вывести информацию о необработанном исключении в журнал
        /// </summary>
        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs args)
        {
            Exception ex = args.ExceptionObject as Exception;
            AppLog.WriteException(ex, string.Format(Localization.UseRussian ? 
                "Необработанное исключение" :
                "Unhandled exception"));
        }

        /// <summary>
        /// Receives the configuration database subset from Server.
        /// </summary>
        private bool ReceiveConfigBase(out ConfigBaseSubset configBase)
        {
            configBase = new ConfigBaseSubset();

            if (ServerComm.ReceiveBaseTable("incnl.dat", configBase.InCnlTable) &&
                ServerComm.ReceiveBaseTable("kp.dat", configBase.KPTable))
            {
                return true;
            }
            else
            {
                configBase = null;
                return false;
            }
        }

        /// <summary>
        /// Инициализировать директории и журнал приложения
        /// </summary>
        private void InitAppDirs()
        {
            AppDirs.Init(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            AppLog.FileName = AppDirs.LogDir + CommUtils.AppLogFileName;
            infoFileName = AppDirs.LogDir + CommUtils.AppStateFileName;
        }

        /// <summary>
        /// Распознать файл конфигурации, создать объекты линий связи и КП
        /// </summary>
        private bool ParseConfig()
        {
            if (Settings.Load(AppDirs.ConfigDir + Settings.DefFileName, out string errMsg))
            {
                // создание линий связи и КП на основе загруженной конфигурации
                try
                {
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
                            if (commLine != null)
                            {
                                commLines.Add(commLine);
                                lineNums.Add(commLineSett.Number);
                            }
                        }
                    }

                    if (commLines.Count > 0)
                    {
                        return true;
                    }
                    else
                    {
                        AppLog.WriteError(Localization.UseRussian ?
                            "Отсутствуют активные линии связи" :
                            "No active communication lines");
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    AppLog.WriteException(ex, Localization.UseRussian ?
                        "Ошибка при создании линий связи" :
                        "Error creating communication lines");
                    return false;
                }
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
            try
            {
                return CommLine.Create(commLineSett, Settings.Params, AppDirs, PassCmd, kpTypes, AppLog);
            }
            catch (Exception ex)
            {
                AppLog.WriteException(ex, string.Format(Localization.UseRussian ? 
                    "Ошибка при создании линии связи {0}" :
                    "Error creating communication line {0}", commLineSett.Number));
                return null;
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
            try
            {
                StringBuilder sbInfo = new StringBuilder();

                TimeSpan workSpan = DateTime.Now - startDT;
                string workDur = workSpan.Days > 0 ? workSpan.ToString(@"d\.hh\:mm\:ss") :
                    workSpan.ToString(@"hh\:mm\:ss");

                if (Localization.UseRussian)
                {
                    sbInfo
                        .AppendLine("SCADA-Коммуникатор")
                        .AppendLine("------------------")
                        .Append("Запуск       : ").AppendLine(startDT.ToLocalizedString())
                        .Append("Время работы : ").AppendLine(workDur)
                        .Append("Состояние    : ").AppendLine(workState)
                        .Append("Версия       : ").AppendLine(CommUtils.AppVersion)
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
                        .Append("Started        : ").AppendLine(startDT.ToLocalizedString())
                        .Append("Execution time : ").AppendLine(workDur)
                        .Append("State          : ").AppendLine(workState)
                        .Append("Version        : ").AppendLine(CommUtils.AppVersion)
                        .Append("SCADA-Server   : ").AppendLine(Settings.Params.ServerUse ?
                            (ServerComm == null ? "not initialized" : ServerComm.CommStateDescr) :
                            "not used")
                        .AppendLine()
                        .AppendLine("Active Communication Lines")
                        .AppendLine("--------------------------");
                }

                lock (commLines)
                {
                    int lineCnt = commLines.Count;
                    if (lineCnt > 0)
                    {
                        // инициализация обозначений линий связи
                        if (lineCaptions == null)
                            InitLineCaptions();

                        // вывод состояний работы активных линий связи
                        for (int i = 0; i < lineCnt; i++)
                            sbInfo.Append(lineCaptions[i]).AppendLine(commLines[i].WorkStateStr);
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
                AppLog.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при записи в файл информации о работе приложения" :
                    "Error writing application information to the file");
            }
        }

        /// <summary>
        /// Инициализировать обозначения линий связи
        /// </summary>
        private void InitLineCaptions()
        {
            // определение максимальной длины обозначения
            int maxCapLen = 0;
            foreach (CommLine commLine in commLines)
            {
                int lineCapLen = commLine.Caption.Length;
                if (maxCapLen < lineCapLen)
                    maxCapLen = lineCapLen;
            }

            int ordLen = commLines.Count.ToString().Length;
            maxCapLen += ordLen + 2 /*точка и пробел*/;

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
                workState = WorkStateNames.Error;
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
            while (!StartThreads())
            {
                Thread.Sleep(StartRetryDelay);
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
                ConfigBaseSubset configBase = null;

                if (Settings.Params.ServerUse)
                {
                    ServerComm = new ServerCommEx(Settings.Params, AppLog);
                    if (!ReceiveConfigBase(out configBase))
                    {
                        fatalError = true;
                        AppLog.WriteError(string.Format(Localization.UseRussian ?
                            "Запуск работы невозможен из-за проблем взаимодействия со SCADA-Сервером.{0}{1}" :
                            "Unable to start operation due to SCADA-Server communication error.{0}{1}",
                            Environment.NewLine, CommPhrases.RetryDelay));
                        ServerComm.Close();
                        ServerComm = null;
                    }
                }

                if (!fatalError)
                {
                    // настройка линий связи по базе конфигурации
                    if (Settings.Params.ServerUse)
                    {
                        foreach (CommLine commLine in commLines)
                        {
                            commLine.Tune(configBase);
                        }
                    }

                    // запуск потоков линий связи
                    AppLog.WriteAction(Localization.UseRussian ? 
                        "Запуск линий связи" : 
                        "Start communication lines");

                    lock (commLines)
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
                        "Start receiving commands");
                    commandReader = new CommandReader(this);
                    commandReader.StartThread();
                }

                if (linesStarted)
                {
                    // запуск потока записи информации о работе приложения
                    infoThread = new Thread(new ThreadStart(WriteInfoExecute));
                    infoThread.Start();

                    workState = WorkStateNames.Normal;
                    return true;
                }
                else
                {
                    workState = WorkStateNames.Error;
                    return false;
                }
            }
            catch (Exception ex)
            {
                AppLog.WriteException(ex, Localization.UseRussian ? 
                    "Ошибка при запуске работы" :
                    "Error starting operation");
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

                if (linesStarted)
                {
                    AppLog.WriteAction(Localization.UseRussian ? 
                        "Остановка линий связи" :
                        "Stop communication lines");
                    linesStarted = false; // далее lock (commLines) не требуется

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
                workState = WorkStateNames.Stopped;
                WriteInfo();
            }
            catch (Exception ex)
            {
                AppLog.WriteException(ex, Localization.UseRussian ? 
                    "Ошибка при остановке линий связи" :
                    "Error stop communication lines");
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
                    CommLine commLine = FindCommLine(lineNum, out int lineInd);

                    if (commLine == null)
                    {
                        // загрузка линии связи из файла кофигурации
                        if (Settings.LoadCommLine(AppDirs.ConfigDir + Settings.DefFileName, lineNum,
                            out Settings.CommLine commLineSett, out string errMsg))
                        {
                            if (commLineSett == null)
                            {
                                AppLog.WriteError(string.Format(Localization.UseRussian ?
                                    "Невозможно запустить линию связи {0}, т.к. она не найдена в файле конфигурации" :
                                    "Unable to start communication line {0} because it is not found in the configuration file",
                                    lineNum));
                            }
                            else if (commLineSett.Active)
                            {
                                // создание линии связи
                                commLine = CreateCommLine(commLineSett);
                            }
                            else
                            {
                                AppLog.WriteError(string.Format(Localization.UseRussian ?
                                    "Невозможно запустить линию связи {0}, т.к. она неактивна" :
                                    "Unable to start communication line {0} because it is not active",
                                    lineNum));
                            }
                        }
                        else
                        {
                            AppLog.WriteAction(errMsg);
                        }

                        // настройка линии связи
                        if (commLine != null && ServerComm != null)
                        {
                            if (ReceiveConfigBase(out ConfigBaseSubset configBase))
                            {
                                commLine.Tune(configBase);
                            }
                            else
                            {
                                commLine = null;
                                AppLog.WriteError(string.Format(Localization.UseRussian ?
                                    "Невозможно запустить линию связи {0} из-за проблем взаимодействия с Сервером" :
                                    "Unable to start communication line {0} due to Server communication error", 
                                    lineNum));
                            }
                        }

                        // запуск линии связи
                        if (commLine != null)
                        {
                            try
                            {
                                AppLog.WriteAction((Localization.UseRussian ? 
                                    "Запуск линии связи " : 
                                    "Start communication line ") + lineNum);
                                commLine.ServerComm = ServerComm;
                                commLine.Start();

                                // добавление линии связи в список
                                lock (commLines)
                                {
                                    commLines.Add(commLine);
                                    lineCaptions = null; // заново сформировать обозначения линии связи
                                }
                            }
                            catch (Exception ex)
                            {
                                AppLog.WriteException(ex, Localization.UseRussian ? 
                                    "Ошибка при запуске линии связи" :
                                    "Error start communication line");
                            }
                        }
                    }
                    else
                    {
                        AppLog.WriteError(string.Format(Localization.UseRussian ?
                            "Невозможно запустить линию связи {0}, т.к. она уже активна" :
                            "Unable to start communication line {0} because it is already active", 
                            lineNum));
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
                        AppLog.WriteError(string.Format(Localization.UseRussian ?
                            "Невозможно остановить линию связи {0}, т.к. она не найдена среди активных линий связи" :
                            "Unable to stop communication line {0} because it is not found among the active lines",
                            lineNum));
                    }
                    else
                    {
                        try
                        {
                            AppLog.WriteAction((Localization.UseRussian ? 
                                "Остановка линии связи " :
                                "Stop communication line ") + lineNum);

                            // выполнение команды завершения работы линии связи
                            commLine.Terminate();

                            // ожидание завершения работы линии связи
                            DateTime utcNowDT = DateTime.UtcNow;
                            DateTime t0 = utcNowDT;
                            DateTime t1 = utcNowDT.AddMilliseconds(Settings.Params.WaitForStop);

                            do
                            {
                                if (!commLine.Terminated)
                                    Thread.Sleep(ScadaUtils.ThreadDelay);
                                utcNowDT = DateTime.UtcNow;
                            }
                            while (t0 <= utcNowDT && utcNowDT <= t1 && !commLine.Terminated);

                            // прерывание работы линии связи
                            if (!commLine.Terminated)
                                commLine.Abort();

                            // удаление линии связи из списка
                            lock (commLines)
                            {
                                commLines.RemoveAt(lineInd);
                                lineCaptions = null; // заново сформировать обозначения линии связи
                            }
                        }
                        catch (Exception ex)
                        {
                            AppLog.WriteException(ex, Localization.UseRussian ? 
                                "Ошибка при остановке линии связи" :
                                "Error stop communication line");
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
                    lock (commLines)
                    {
                        // передача команды линиям связи
                        foreach (CommLine commLine in commLines)
                            commLine.EnqueueCmd(cmd);
                    }
                }
                catch (Exception ex)
                {
                    AppLog.WriteException(ex, Localization.UseRussian ? 
                        "Ошибка при передаче команды менеджеру" : 
                        "Error passing command to the manager");
                }
            }
        }


        /// <summary>
        /// Запустить службу
        /// </summary>
        public void StartService()
        {
            // инициализация необходимых директорий
            InitAppDirs();

            AppLog.WriteBreak();
            AppLog.WriteAction(string.Format(Localization.UseRussian ? 
                "Служба ScadaCommService {0} запущена" :
                "ScadaCommService {0} is started", CommUtils.AppVersion));

            if (AppDirs.Exist)
            {
                // локализация
                if (Localization.LoadDictionaries(AppDirs.LangDir, "ScadaData", out string errMsg))
                    CommonPhrases.Init();
                else
                    AppLog.WriteAction(errMsg, Log.ActTypes.Error);

                if (Localization.LoadDictionaries(AppDirs.LangDir, "ScadaComm", out errMsg))
                    CommPhrases.InitFromDictionaries();
                else
                    AppLog.WriteError(errMsg);

                // запуск работы
                if (StartOperation())
                    return;
            }
            else
            {
                AppLog.WriteError(string.Format(Localization.UseRussian ?
                    "Необходимые директории не существуют:{0}{1}" :
                    "The required directories do not exist:{0}{1}",
                    Environment.NewLine, string.Join(Environment.NewLine, AppDirs.GetRequiredDirs())));
            }

            AppLog.WriteError(Localization.UseRussian ?
                "Нормальная работа программы невозможна" :
                "Normal program execution is impossible");
        }

        /// <summary>
        /// Остановить службу
        /// </summary>
        public void StopService()
        {
            StopOperation();
            AppLog.WriteAction(Localization.UseRussian ? 
                "Служба ScadaCommService остановлена" :
                "ScadaCommService is stopped");
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
                "ScadaCommService is shutdown");
            AppLog.WriteBreak();
        }
    }
}
