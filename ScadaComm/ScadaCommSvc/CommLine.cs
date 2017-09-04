/*
 * Copyright 2017 Mikhail Shiryaev
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
 * Summary  : Communication line
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2006
 * Modified : 2017
 */

using Scada.Comm.Channels;
using Scada.Comm.Devices;
using Scada.Data.Configuration;
using Scada.Data.Models;
using Scada.Data.Tables;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Threading;
using Utils;

namespace Scada.Comm.Svc
{
    /// <summary>
    /// Communication line
    /// <para>Линия связи</para>
    /// </summary>
    internal sealed class CommLine : ICommLineService
    {
        /// <summary>
        /// Полный адрес КП
        /// </summary>
        private class KPFullAddr : IComparer<KPFullAddr>
        {
            /// <summary>
            /// Числовой адрес
            /// </summary>
            public int Address;
            /// <summary>
            /// Позывной
            /// </summary>
            public string CallNum;

            /// <summary>
            /// Конструктор
            /// </summary>
            public KPFullAddr(int address, string callNum)
            {
                Address = address;
                CallNum = callNum;
            }
            /// <summary>
            /// Performs a comparison of two objects of the same type
            /// </summary>
            public int Compare(KPFullAddr x, KPFullAddr y)
            {
                int comp = x.Address.CompareTo(y.Address);
                return comp == 0 ? string.Compare(x.CallNum, y.CallNum, StringComparison.OrdinalIgnoreCase) : comp;
            }
        }

        /// <summary>
        /// Состояния работы линии связи
        /// </summary>
        private enum WorkStates
        {
            /// <summary>
            /// Бездействие
            /// </summary>
            Idle,
            /// <summary>
            /// Цикл работы
            /// </summary>
            Running,
            /// <summary>
            /// Завершение работы
            /// </summary>
            Terminating,
            /// <summary>
            /// Работа нормально завершена
            /// </summary>
            Terminated,
            /// <summary>
            /// Работа прервана
            /// </summary>
            Aborted
        }

        /// <summary> 
        /// Делегат передачи команды КП 
        /// </summary> 
        public delegate void PassCmdDelegate(Command cmd); 


        /// <summary>
        /// Задержка перед повторными попытками, мс
        /// </summary>
        private const int RetryDelay = 10000;
        /// <summary>
        /// Минимальная задержка после цикла опроса, мс
        /// </summary>
        private const int MinCycleDelay = 10;
        /// <summary>
        /// Задержка после пустого цикла опроса, мс
        /// </summary>
        private const int EmptyCycleDelay = 200;
        /// <summary>
        /// Количество попыток передачи данных серверу без задержки
        /// </summary>
        private const int QuickAttemptCnt = 5;
        /// <summary>
        /// Обозначение отсутствия действия
        /// </summary>
        private static readonly string NoAction = Localization.UseRussian ? "нет" : "no";

        private string numAndName;        // номер и нименование линии связи
        private CommChannelLogic commCnl; // канал связи с физическими КП
        private int reqTriesCnt;          // количество попыток перезапроса КП при ошибке
        private int cycleDelay;           // пауза после цикла опроса, мс
        private bool cmdEnabled;          // разрешены ли команды ТУ
        private bool reqAfterCmd;         // опрос КП после команды ТУ
        private bool detailedLog;         // признак записи в журнал линии связи подробной информации
        private int sendAllDataPer;       // период передачи на сервер всех данных КП, с
        private AppDirs appDirs;          // директории приложения
        private ServerCommEx serverComm;  // ссылка на объект обмена данными со SCADA-Сервером
        private PassCmdDelegate passCmd;  // метод передачи команды КП всем линиям связи

        private Thread thread;                                  // поток работы линии связи
        private SortedList<string, object> commonProps;         // общие свойства линии связи, доступные её КП
        private Dictionary<int, KPLogic> kpNumDict;             // словарь КП по номерам
        private Dictionary<int, KPLogic> kpAddrDict;            // словарь КП по адресам
        private Dictionary<string, KPLogic> kpCallNumDict;      // словарь КП по позывным
        private Dictionary<KPFullAddr, KPLogic> kpFullAddrDict; // словарь КП по полным адресам
        private List<Command> cmdList;                          // список команд для выполнения
        private List<KPLogic.TagSrez> unsentSrezList;           // список неотправленных архивных срезов КП
        private List<KPLogic.KPEvent> unsentEventList;          // список неотправленных событий КП
        private WorkStates workState;     // состояние работы линии связи
        private Log log;                  // журнал линии связи
        private string infoFileName;      // имя файла информации о работе линии связи
        private string curAction;         // описание текущего действия
        private string captionUnderline;  // подчёркивание обозначения линии связи
        private string allCustomParams;   // все имена и значения пользовательских параметров
        private string[] kpCaptions;      // обозначения КП

        private object infoLock;          // объект для синхронизации записи в файл информации о работе линии связи
        private object serverLock;        // объект для синхронизации связи с сервером в рамках линии связи
        

        /// <summary>
        /// Конструктор, ограничивающий создание объекта без параметров
        /// </summary>
        private CommLine()
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public CommLine(bool bind, int number, string name)
        {
            // поля
            numAndName = number + (string.IsNullOrEmpty(name) ? "" : " \"" + name + "\"");
            commCnl = null;
            reqTriesCnt = 1;
            cycleDelay = 0;
            cmdEnabled = false;
            reqAfterCmd = false;
            detailedLog = true;
            sendAllDataPer = 0;
            appDirs = new AppDirs();
            serverComm = null;
            passCmd = null;

            thread = null;
            commonProps = new SortedList<string, object>();
            kpNumDict = new Dictionary<int, KPLogic>();
            kpAddrDict = new Dictionary<int, KPLogic>();
            kpCallNumDict = new Dictionary<string, KPLogic>();
            kpFullAddrDict = new Dictionary<KPFullAddr, KPLogic>();
            cmdList = new List<Command>();
            unsentSrezList = new List<KPLogic.TagSrez>();
            unsentEventList = new List<KPLogic.KPEvent>();
            workState = WorkStates.Idle;
            log = new Log(Log.Formats.Simple) { Encoding = Encoding.UTF8 };
            infoFileName = "";
            curAction = NoAction;
            allCustomParams = null;
            kpCaptions = null;

            infoLock = new object();
            serverLock = new object();

            // свойства
            Bind = bind;
            Number = number;
            Name = name;
            Caption = (Localization.UseRussian ? "Линия " : "Line ") + numAndName;
            CustomParams = new SortedList<string, string>();
            KPList = new List<KPLogic>();
            
            // ещё одно поле
            captionUnderline = new string('-', Caption.Length);
        }


        /// <summary>
        /// Получить признак привязки к SCADA-Серверу
        /// </summary>
        public bool Bind { get; private set; }

        /// <summary>
        /// Получить номер линии связи
        /// </summary>
        public int Number { get; private set; }

        /// <summary>
        /// Получить имя КП
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Получить обозначение линии связи
        /// </summary>
        public string Caption { get; private set; }

        /// <summary>
        /// Получить или установить канал связи с физическими КП
        /// </summary>
        public CommChannelLogic CommChannelLogic
        {
            get
            {
                return commCnl;
            }
            set
            {
                CheckChangesAllowed();
                commCnl = value;
            }
        }

        /// <summary>
        /// Получить или установить количество попыток перезапроса КП при ошибке
        /// </summary>
        public int ReqTriesCnt
        {
            get
            {
                return reqTriesCnt;
            }
            set
            {
                CheckChangesAllowed();
                reqTriesCnt = value;
            }
        }

        /// <summary>
        /// Получить или установить паузу после цикла опроса, мс
        /// </summary>
        public int CycleDelay
        {
            get
            {
                return cycleDelay;
            }
            set
            {
                CheckChangesAllowed();
                cycleDelay = value;
            }
        }

        /// <summary>
        /// Получить или установить признак, разрешены ли команды ТУ
        /// </summary>
        public bool CmdEnabled
        {
            get
            {
                return cmdEnabled;
            }
            set
            {
                CheckChangesAllowed();
                cmdEnabled = value;
            }
        }

        /// <summary>
        /// Получить или установить необходимость опроса КП после команды ТУ
        /// </summary>
        public bool ReqAfterCmd
        {
            get
            {
                return reqAfterCmd;
            }
            set
            {
                CheckChangesAllowed();
                reqAfterCmd = value;
            }
        }

        /// <summary>
        /// Получить или установить признак записи в журнал линии связи подробной информации
        /// </summary>
        public bool DetailedLog
        {
            get
            {
                return detailedLog;
            }
            set
            {
                CheckChangesAllowed();
                detailedLog = value;
            }
        }

        /// <summary>
        /// Получить или установить период передачи на сервер всех данных КП, с
        /// </summary>
        public int SendAllDataPer
        {
            get
            {
                return sendAllDataPer;
            }
            set
            {
                CheckChangesAllowed();
                sendAllDataPer = value;
            }
        }

        /// <summary>
        /// Получить пользовательские параметры линии связи
        /// </summary>
        public SortedList<string, string> CustomParams { get; private set; }

        /// <summary>
        /// Получить список опрашиваемых КП
        /// </summary>
        public List<KPLogic> KPList { get; private set; }

        /// <summary>
        /// Получить или установить директории приложения
        /// </summary>
        public AppDirs AppDirs
        {
            get
            {
                return appDirs;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                appDirs = value;
                string fileName = appDirs.LogDir + "line" + CommUtils.AddZeros(Number, 3);
                log.FileName = fileName + ".log";
                infoFileName = fileName + ".txt";
            }
        }

        /// <summary>
        /// Получить или установить ссылку на объект обмена данными со SCADA-Сервером
        /// </summary>
        public ServerCommEx ServerComm
        {
            get
            {
                return serverComm;
            }
            set
            {
                CheckChangesAllowed();
                serverComm = Bind ? value : null;
            }
        }

        /// <summary>
        /// Получить или установить метод передачи команды КП всем линиям связи
        /// </summary>
        public PassCmdDelegate PassCmd
        {
            get
            {
                return passCmd;
            }
            set
            {
                CheckChangesAllowed();
                passCmd = value;
            }
        }

        /// <summary>
        /// Получить строковое представление состояния работы линии связи
        /// </summary>
        public string WorkStateStr
        {
            get
            {
                switch (workState)
                {
                    case WorkStates.Idle:
                        return Localization.UseRussian ? "бездействие" : "idle";
                    case WorkStates.Running:
                        return Localization.UseRussian ? "цикл работы" : "running";
                    case WorkStates.Terminating:
                        return Localization.UseRussian ? "завершение работы" : "terminating";
                    case WorkStates.Terminated:
                        return Localization.UseRussian ? "работа завершена" : "terminated";
                    case WorkStates.Aborted:
                        return Localization.UseRussian ? "работа прервана" : "aborted";
                    default:
                        return "";
                }
            }
        }

        /// <summary>
        /// Получить признак, что работа линии связи полностью завершена
        /// </summary>
        public bool Terminated
        {
            get
            {
                return workState == WorkStates.Terminated;
            }
        }


        /// <summary>
        /// Проверить, что изменения конфигурации линии связи разрешены
        /// </summary>
        private void CheckChangesAllowed()
        {
            if (workState == WorkStates.Running || workState == WorkStates.Terminating)
                throw new InvalidOperationException(Localization.UseRussian ?
                    "Невозможно изменить конфигурацию линии связи, если линия работает." :
                    "Unable to change the communication line configuration if the line is operating.");
        }

        /// <summary>
        /// Записать в файл информацию о работе линии связи
        /// </summary>
        private void WriteInfo()
        {
            try
            {
                StringBuilder sbInfo = new StringBuilder();
                sbInfo
                    .AppendLine(Caption)
                    .AppendLine(captionUnderline);

                if (Localization.UseRussian)
                {
                    sbInfo
                        .Append("Состояние : ").AppendLine(WorkStateStr)
                        .Append("Действие  : ").AppendLine(curAction);
                }
                else
                {
                    sbInfo
                        .Append("State  : ").AppendLine(WorkStateStr)
                        .Append("Action : ").AppendLine(curAction);
                }

                sbInfo.AppendLine();
                if (commCnl != null)
                    sbInfo.Append(commCnl.GetInfo()).AppendLine();
                AppendCustomParams(sbInfo);
                AppendCommonProps(sbInfo);
                AppendActiveKP(sbInfo);

                // запись в файл
                lock (infoLock)
                {
                    using (StreamWriter writer = new StreamWriter(infoFileName, false, Encoding.UTF8))
                        writer.Write(sbInfo.ToString());
                }
            }
            catch (ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                log.WriteAction((Localization.UseRussian ? 
                    "Ошибка при записи в файл информации о работе линии связи: " :
                    "Error writing communication line information to the file: ") + ex.Message);
            }
        }

        /// <summary>
        /// Добавить в конструктор строки пользовательские параметры линии связи
        /// </summary>
        private void AppendCustomParams(StringBuilder sb)
        {
            if (Localization.UseRussian)
            {
                sb.AppendLine("Пользовательские параметры");
                sb.AppendLine("--------------------------");
            }
            else
            {
                sb.AppendLine("Custom Parameters");
                sb.AppendLine("-----------------");
            }

            if (allCustomParams == null)
            {
                // формирование строки всех пользовательских параметров
                StringBuilder sbCustomParams = new StringBuilder();
                if (CustomParams.Count > 0)
                {
                    foreach (KeyValuePair<string, string> pair in CustomParams)
                        sbCustomParams.Append(pair.Key).Append(" = ").AppendLine(pair.Value);
                    allCustomParams = sbCustomParams.ToString();
                }
                else
                {
                    allCustomParams = (Localization.UseRussian ? "Нет" : "No") + Environment.NewLine;
                }
            }

            sb.AppendLine(allCustomParams);
        }

        /// <summary>
        /// Добавить в конструктор строки общие свойства линии связи
        /// </summary>
        private void AppendCommonProps(StringBuilder sb)
        {
            if (Localization.UseRussian)
            {
                sb.AppendLine("Общие свойства");
                sb.AppendLine("--------------");
            }
            else
            {
                sb.AppendLine("Common Properties");
                sb.AppendLine("-----------------");
            }

            if (commonProps.Count > 0)
            {
                foreach (KeyValuePair<string, object> pair in commonProps)
                    sb.Append(pair.Key).Append(" = ").AppendLine(pair.Value == null ? "null" : pair.Value.ToString());
            }
            else
            {
                sb.AppendLine(Localization.UseRussian ? "Нет" : "No");
            }

            sb.AppendLine();
        }

        /// <summary>
        /// Добавить в конструктор строки активые КП линии связи
        /// </summary>
        private void AppendActiveKP(StringBuilder sb)
        {
            if (Localization.UseRussian)
            {
                sb.AppendLine("Активные КП");
                sb.AppendLine("-----------");
            }
            else
            {
                sb.AppendLine("Active Devices");
                sb.AppendLine("--------------");
            }

            int kpCnt = KPList.Count;

            if (kpCaptions == null)
            {
                if (kpCnt > 0)
                {
                    // вычисление максимальной длины обозначения КП
                    int maxKPCapLen = 0;
                    int ordLen = kpCnt.ToString().Length;
                    foreach (KPLogic kpLogic in KPList)
                    {
                        int kpCapLen = ordLen + 2 + kpLogic.Caption.Length;
                        if (maxKPCapLen < kpCapLen)
                            maxKPCapLen = kpCapLen;
                    }

                    // формирование обозначений активных КП
                    kpCaptions = new string[kpCnt];
                    for (int i = 0; i < kpCnt; i++)
                    {
                        string s = (i + 1).ToString().PadLeft(ordLen) + ". " + KPList[i].Caption;
                        kpCaptions[i] = s.PadRight(maxKPCapLen) + " : ";
                    }
                }
                else
                {
                    kpCaptions = new string[0];
                }
            }

            if (kpCnt > 0)
            {
                // вывод состояний работы активных КП
                for (int i = 0; i < kpCnt; i++)
                    sb.Append(kpCaptions[i]).AppendLine(KPList[i].WorkStateStr);
            }
            else
            {
                sb.AppendLine(Localization.UseRussian ? "Нет" : "No");
            }
        }

        /// <summary>
        /// Записать в файл информацию о работе КП
        /// </summary>
        private void WriteKPInfo(KPLogic kpLogic)
        {
            try
            {
                // определение имени файла
                string kpInfoFileName;
                int kpNum = kpLogic.Number;
                if (kpNum < 10) 
                    kpInfoFileName = "kp00" + kpNum + ".txt";
                else if (kpNum < 100) 
                    kpInfoFileName = "kp0" + kpNum + ".txt";
                else 
                    kpInfoFileName = "kp" + kpNum + ".txt";

                // запись информации
                using (StreamWriter writer = new StreamWriter(appDirs.LogDir + kpInfoFileName, false, Encoding.UTF8))
                    writer.Write(kpLogic.GetInfo());
            }
            catch (ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                log.WriteAction((Localization.UseRussian ?
                    "Ошибка при записи в файл информации о работе КП: " :
                    "Error writing device information to the file: ") + ex.Message);
            }
        }

        /// <summary>
        /// Вывести начальную информацию о линии связи
        /// </summary>
        private void WriteInitialInfo()
        {
            WriteInfo();
            log.WriteBreak();
            log.WriteAction((Localization.UseRussian ?
                "Инициализация линии связи " :
                "Initialize communication line ") + numAndName);
        }

        /// <summary>
        /// Запуск, работа и остановка линии связи, метод вызывается в отдельном потоке
        /// </summary>
        private void Execute()
        {
            try
            {
                workState = WorkStates.Running;
                allCustomParams = null;
                kpCaptions = null;

                // вывод в журнал и в файлы информации о работе
                log.WriteAction((Localization.UseRussian ? 
                    "Запуск линии связи " : 
                    "Start communication line ") + numAndName);

                WriteInfo();
                foreach (KPLogic kpLogic in KPList)
                    WriteKPInfo(kpLogic);

                if (KPList.Count <= 0)
                {
                    log.WriteAction(Localization.UseRussian ? 
                        "Работа линии связи невозможна из-за отсутствия КП" :
                        "Communication line execution is impossible due to device missing");
                }
                else
                {
                    // запуск канала связи
                    if (commCnl != null)
                    {
                        bool commCnlStarted = false;
                        while (!commCnlStarted)
                        {
                            commCnlStarted = StartCommChannel();
                            if (!commCnlStarted)
                            {
                                // паузу нужно делать вне catch, иначе поток не может прерваться
                                log.WriteAction(CommPhrases.RetryDelay);
                                Thread.Sleep(RetryDelay);
                            }
                        }
                    }

                    // подготовка КП к работе
                    foreach (KPLogic kpLogic in KPList)
                    {
                        try
                        {
                            kpLogic.OnCommLineStart();
                        }
                        catch (Exception ex)
                        {
                            kpLogic.WorkState = KPLogic.WorkStates.Error;
                            log.WriteAction(string.Format(Localization.UseRussian ? 
                                "Ошибка при выполнении действий {0} при запуске линии связи: {1}" : 
                                "Error executing actions of {0} on communication line start: {1}", 
                                kpLogic.Caption, ex.Message));
                        }
                        WriteKPInfo(kpLogic);
                    }

                    if (!detailedLog)
                    {
                        log.WriteAction(Localization.UseRussian ?
                            "Вывод информации КП в журнал отключен" :
                            "Device output to the log is disabled");
                    }

                    // цикл работы линии связи
                    WorkCycle();
                }
            }
            catch (ThreadAbortException)
            {
                // Clean-up code can go here.
                // If there is no Finally clause, ThreadAbortException is
                // re-thrown by the system at the end of the Catch clause. 
                foreach (KPLogic kpLogic in KPList)
                {
                    try
                    {
                        kpLogic.OnCommLineAbort();
                        kpLogic.WorkState = KPLogic.WorkStates.Undefined;
                    }
                    catch (Exception ex)
                    {
                        kpLogic.WorkState = KPLogic.WorkStates.Error;
                        log.WriteAction(string.Format(Localization.UseRussian ?
                            "Ошибка при выполнении действий {0} при прерывании работы линии связи: {1}" :
                            "Error executing actions of {0} on communication line abort: {1}",
                            kpLogic.Caption, ex.Message));
                    }
                    WriteKPInfo(kpLogic);
                }

                StopCommChannel();

                log.WriteLine();
                log.WriteAction((Localization.UseRussian ? 
                    "Прерывание работы линии связи " : 
                    "Abort communication line ") + numAndName);
                log.WriteBreak();

                workState = WorkStates.Aborted;
                curAction = NoAction;
                WriteInfo();
            }
            catch (Exception ex)
            {
                log.WriteAction(string.Format(Localization.UseRussian ? 
                    "Ошибка при работе линии связи {0}: {1}" : 
                    "Error communication line {0} execution: {1}", numAndName, ex.Message));
            }
            finally
            {
                if (workState != WorkStates.Aborted)
                {
                    foreach (KPLogic kpLogic in KPList)
                    {
                        try
                        {
                            kpLogic.OnCommLineTerminate();
                            kpLogic.WorkState = KPLogic.WorkStates.Undefined;
                        }
                        catch (Exception ex)
                        {
                            kpLogic.WorkState = KPLogic.WorkStates.Error;
                            log.WriteAction(string.Format(Localization.UseRussian ?
                                "Ошибка при выполнении действий {0} при завершении работы линии связи: {1}" :
                                "Error executing actions of {0} on communication line terminating: {1}",
                                kpLogic.Caption, ex.Message));
                        }
                        WriteKPInfo(kpLogic);
                    }

                    StopCommChannel();

                    log.WriteLine();
                    log.WriteAction((Localization.UseRussian ? 
                        "Завершение работы линии связи " : 
                        "Stop communication line ") + numAndName);
                    log.WriteBreak();

                    workState = WorkStates.Terminated;
                    curAction = NoAction;
                    WriteInfo();
                }

                thread = null;
            }

            // Do not put clean-up code here, because the exception 
            // is rethrown at the end of the Finally clause.
        }

        /// <summary>
        /// Цикл работы линии связи
        /// </summary>
        private void WorkCycle()
        {
            bool terminateCycle = false;           // завершить цикл работы
            DateTime sendAllDataDT = DateTime.Now; // время передачи на сервер всех текущих данных КП
            int kpCnt = KPList.Count;              // количество КП

            while (!terminateCycle)
            {
                int commCnt = 0; // количество выполненных сеансов связи (команд ТУ и опросов КП) за итерацию
                int kpInd = 0;   // индекс опрашиваемого КП

                // определение необходимости передать на сервер все текущие данные КП
                DateTime nowDT = DateTime.Now;
                bool sendAllData = sendAllDataPer > 0 && (nowDT - sendAllDataDT).TotalSeconds >= sendAllDataPer;
                if (sendAllData)
                    sendAllDataDT = nowDT;

                while (kpInd < kpCnt && !terminateCycle)
                {
                    // обработка команд ТУ
                    List<KPLogic> extraKPs; // список КП для внеочередного опроса
                    ProcCommands(ref commCnt, out extraKPs);

                    // внеочередной опрос КП
                    if (extraKPs != null)
                    {
                        for (int ind = 0, cnt = extraKPs.Count; ind < cnt && !terminateCycle; ind++)
                        {
                            KPLogic extraKP = extraKPs[ind];
                            InteractKP(extraKP, true, sendAllData, ref commCnt, out terminateCycle);
                        }
                    }

                    // опрос очередного КП
                    if (!terminateCycle)
                    {
                        curAction = DateTime.Now.ToLocalizedString() + (Localization.UseRussian ?
                            " Выбор КП для связи" :
                            " Choosing device for communication");
                        WriteInfo();

                        KPLogic kpLogic = KPList[kpInd];
                        bool sessionNeeded = CheckSessionNeeded(kpLogic);
                        InteractKP(kpLogic, sessionNeeded, sendAllData, ref commCnt, out terminateCycle);
                        kpInd++;
                    }
                }

                // задержка после цикла опроса линии связи
                if (workState != WorkStates.Terminating)
                {
                    if (commCnt == 0)
                        Thread.Sleep(EmptyCycleDelay);
                    else
                        Thread.Sleep(cycleDelay);
                }
            }
        }

        /// <summary>
        /// Выполнить взаимодействие с КП
        /// </summary>
        private void InteractKP(KPLogic kpLogic, bool sessionNeeded, bool sendAllData, 
            ref int commCnt, out bool terminateCycle)
        {
            terminateCycle = false;

            try
            {
                // установка признака завершения работы для опрашиваемого КП
                kpLogic.Terminated = workState == WorkStates.Terminating;

                // выполнение сеанса опроса КП
                if (sessionNeeded)
                {
                    curAction = DateTime.Now.ToLocalizedString() + (Localization.UseRussian ?
                    " Сеанс связи с " :
                    " Communication with ") + kpLogic.Caption;
                    WriteInfo();

                    CommCnlBeforeSession(kpLogic);

                    if (kpLogic.ConnRequired && (kpLogic.Connection == null || !kpLogic.Connection.Connected))
                    {
                        KPInvalidateCurData(kpLogic);
                        log.WriteLine();
                        log.WriteAction(string.Format(Localization.UseRussian ?
                            "Невозможно выполнить сеанс связи с {0}, т.к. соединение не установлено" :
                            "Unable to communicate with {0} because connection is not established",
                            kpLogic.Caption));
                    }
                    else if (KPSession(kpLogic))
                    {
                        commCnt++;
                    }

                    WriteKPInfo(kpLogic);
                    CommCnlAfterSession(kpLogic);
                }

                // передача данных КП на сервер
                if (sessionNeeded || sendAllData)
                    SendDataToServer(kpLogic, sendAllData);

                // определение необходимости завершить цикл работы
                terminateCycle = workState == WorkStates.Terminating && kpLogic.Terminated;
            }
            catch (ThreadAbortException)
            {
                // обработка данного исключения реализована в методе Execute
            }
            catch (Exception ex)
            {
                curAction = NoAction;
                WriteInfo();
                log.WriteAction(string.Format(Localization.UseRussian ?
                    "Ошибка при взаимодействии с {0}: " :
                    "Error interacting with {0}: ", kpLogic.Caption, ex.Message));
            }
        }

        /// <summary>
        /// Запустить канал связи
        /// </summary>
        private bool StartCommChannel()
        {
            try
            {
                commCnl.Start();
                return true;
            }
            catch (Exception ex)
            {
                log.WriteAction((Localization.UseRussian ?
                    "Ошибка при запуске канала связи: " :
                    "Error starting communication channel: ") + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Остановить канал связи
        /// </summary>
        private void StopCommChannel()
        {
            try
            {
                if (commCnl != null)
                    commCnl.Stop();
            }
            catch (Exception ex)
            {
                log.WriteAction((Localization.UseRussian ?
                    "Ошибка при остановке канала связи: " :
                    "Error stopping communication channel: ") + ex.Message);
            }
        }

        /// <summary>
        /// Выполнить действия канала связи перед сеансом опроса КП
        /// </summary>
        private void CommCnlBeforeSession(KPLogic kpLogic)
        {
            try
            {
                if (commCnl != null)
                    commCnl.BeforeSession(kpLogic);
            }
            catch (Exception ex)
            {
                log.WriteAction((Localization.UseRussian ?
                    "Ошибка при выполнении действий канала связи перед сеансом опроса КП: " :
                    "Error executing the communication channel actions before session with a device: ") + ex.Message);
            }
        }

        /// <summary>
        /// Выполнить действия канала связи после сеанса опроса КП
        /// </summary>
        private void CommCnlAfterSession(KPLogic kpLogic)
        {
            try
            {
                if (commCnl != null)
                    commCnl.AfterSession(kpLogic);
            }
            catch (Exception ex)
            {
                log.WriteAction((Localization.UseRussian ?
                    "Ошибка при выполнении действий канала связи после сеанса опроса КП: " :
                    "Error executing the communication channel actions after session with a device: ") + ex.Message);
            }
        }

        /// <summary>
        /// Выполнить сеанс опроса КП
        /// </summary>
        private bool KPSession(KPLogic kpLogic)
        {
            try
            {
                kpLogic.Session();
                return true;
            }
            catch (Exception ex)
            {
                kpLogic.WorkState = KPLogic.WorkStates.Error;
                log.WriteAction((Localization.UseRussian ?
                    "Ошибка при выполнении сеанса опроса КП: " :
                    "Error communicating with the device: ") + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Установить текущие данные КП как недостоверные
        /// </summary>
        private void KPInvalidateCurData(KPLogic kpLogic)
        {
            try
            {
                kpLogic.InvalidateCurData();
            }
            catch (Exception ex)
            {
                kpLogic.WorkState = KPLogic.WorkStates.Error;
                log.WriteAction((Localization.UseRussian ?
                    "Ошибка при установке текущих данных КП как недостоверных: " :
                    "Error invalidating the device data: ") + ex.Message);
            }
        }

        /// <summary>
        /// Отправить команду ТУ
        /// </summary>
        private bool KPSendCmd(KPLogic kpLogic, Command cmd)
        {
            try
            {
                kpLogic.SendCmd(cmd);
                return true;
            }
            catch (Exception ex)
            {
                kpLogic.WorkState = KPLogic.WorkStates.Error;
                log.WriteAction((Localization.UseRussian ?
                    "Ошибка при отправке команды ТУ: " :
                    "Error sending command: ") + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Обработать команды ТУ в цикле линии связи
        /// </summary>
        private void ProcCommands(ref int commCnt, out List<KPLogic> reqKPs)
        {
            reqKPs = null;

            try
            {
                curAction = DateTime.Now.ToLocalizedString() + (Localization.UseRussian ? 
                    " Обработка команд ТУ" : 
                    " Processing commands");
                WriteInfo();

                // копирование команд в буфер, чтобы минимизировать время блокировки списка команд
                List<Command> cmdBufList = null; // буфер стандартных и бинарных команд
                HashSet<int> reqKPNums = null;   // номера КП для внеочередного опроса

                lock (cmdList)
                {
                    if (cmdList.Count > 0)
                    {
                        cmdBufList = new List<Command>();

                        while (cmdList.Count > 0)
                        {
                            Command cmd = cmdList[0];

                            if (cmd.CmdTypeID == BaseValues.CmdTypes.Request || reqAfterCmd)
                            {
                                if (reqKPs == null)
                                {
                                    reqKPs = new List<KPLogic>();
                                    reqKPNums = new HashSet<int>();
                                }

                                KPLogic reqKP = FindKP(cmd.KPNum);

                                if (reqKP != null && !reqKPNums.Contains(reqKP.Number))
                                {
                                    reqKPs.Add(reqKP);
                                    reqKPNums.Add(reqKP.Number);
                                }
                            }

                            if (cmd.CmdTypeID != BaseValues.CmdTypes.Request)
                            {
                                cmdBufList.Add(cmd);
                            }

                            cmdList.RemoveAt(0);
                        }
                    }
                }

                // отправка команд ТУ
                if (cmdBufList != null)
                {
                    foreach (Command cmd in cmdBufList)
                    {
                        KPLogic kpLogic = FindKP(cmd.KPNum);

                        if (kpLogic != null)
                        {
                            curAction = DateTime.Now.ToLocalizedString() + (Localization.UseRussian ? 
                                " Команда " : " Command to ") + kpLogic.Caption;
                            WriteInfo();

                            CommCnlBeforeSession(kpLogic);
                            if (kpLogic.ConnRequired && (kpLogic.Connection == null || !kpLogic.Connection.Connected))
                            {
                                log.WriteAction(string.Format(Localization.UseRussian ?
                                    "Невозможно отправить команду ТУ для {0}, т.к. соединение не установлено: " :
                                    "Unable to send command to {0} because connection is not established",
                                    kpLogic.Caption));
                            }
                            else
                            {
                                if (KPSendCmd(kpLogic, cmd))
                                    commCnt++;
                                WriteKPInfo(kpLogic);
                            }
                            CommCnlAfterSession(kpLogic);
                        }
                    }
                }
            }
            catch (ThreadAbortException)
            {
                // обработка данного исключения реализована в методе Execute
            }
            catch (Exception ex)
            {
                curAction = NoAction;
                WriteInfo();
                log.WriteAction((Localization.UseRussian ?
                    "Ошибка при обработке команд ТУ: " :
                    "Error processing commands: ") + ex.Message);
            }
        }

        /// <summary>
        /// Проверить, что необходимо выполнить сеанс опроса КП в соответствии с его параметрами опроса
        /// </summary>
        private bool CheckSessionNeeded(KPLogic kpLogic)
        {
            double reqPeriod = kpLogic.ReqParams.Period.TotalDays; // период опроса КП, дн.
            if (kpLogic.ReqParams.Time > DateTime.MinValue || reqPeriod > 0)
            {
                TimeSpan nowTime = DateTime.Now.TimeOfDay;            // текущее время
                TimeSpan reqTime = kpLogic.ReqParams.Time.TimeOfDay;  // время опроса КП
                TimeSpan lastSessTime = kpLogic.LastSessDT.TimeOfDay; // время последнего сеанса связи с КП

                if (reqPeriod > 0)
                {
                    double t = (nowTime - reqTime).TotalDays / reqPeriod;
                    return t >= 0 &&
                        ((int)t * reqPeriod + reqTime.TotalDays > lastSessTime.TotalDays ||
                        lastSessTime > nowTime /*новый день*/);
                }
                else
                {
                    return reqTime.TotalDays <= 0 ||
                        reqTime <= nowTime && reqTime > lastSessTime;
                }
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Передать данные КП SCADA-Серверу
        /// </summary>
        private void SendDataToServer(KPLogic kpLogic, bool sendAllData)
        {
            if (serverComm != null)
            {
                lock (serverLock)
                {
                    curAction = DateTime.Now.ToLocalizedString() + (Localization.UseRussian ? 
                        " Передача данных SCADA-серверу" : 
                        " Sending data to SCADA-Server");
                    WriteInfo();

                    // передача текущих данных
                    KPLogic.TagSrez curSrez = GetKPCurData(kpLogic, sendAllData);
                    if (curSrez != null)
                        serverComm.SendSrez(curSrez);

                    // передача архивных срезов до тех пор, пока все срезы не будут переданы
                    kpLogic.MoveArcSrez(unsentSrezList);

                    foreach (KPLogic.TagSrez tagSrez in unsentSrezList)
                    {
                        int attemptNum = 0;
                        while (!serverComm.SendArchive(tagSrez))
                        {
                            if (++attemptNum < QuickAttemptCnt)
                            {
                                if (attemptNum == 1)
                                    log.WriteLine();
                                log.WriteAction(Localization.UseRussian ?
                                    "Неудачная попытка передачи архивного среза SCADA-серверу" :
                                    "Attempt to send archive data to SCADA-Server failed");
                                Thread.Sleep(ScadaUtils.ThreadDelay);
                            }
                            else
                            {
                                // задержка перед следующей попыткой
                                log.WriteAction(CommPhrases.RetryDelay);
                                Thread.Sleep(RetryDelay);
                            }
                        }
                    }

                    unsentSrezList.Clear();

                    // передача событий до тех пор, пока все события не будут переданы
                    kpLogic.MoveEvents(unsentEventList);

                    foreach (KPLogic.KPEvent kpEvent in unsentEventList)
                    {
                        int attemptNum = 0;
                        while (!serverComm.SendEvent(kpEvent))
                        {
                            if (++attemptNum < QuickAttemptCnt)
                            {
                                if (attemptNum == 1)
                                    log.WriteLine();
                                log.WriteAction(Localization.UseRussian ?
                                    "Неудачная попытка передачи события SCADA-серверу" :
                                    "Attempt to send event to SCADA-Server failed");
                                Thread.Sleep(ScadaUtils.ThreadDelay);
                            }
                            else
                            {
                                // задержка перед следующей попыткой
                                log.WriteAction(CommPhrases.RetryDelay);
                                Thread.Sleep(RetryDelay);
                            }
                        }
                    }

                    unsentEventList.Clear();
                }
            }
        }

        /// <summary>
        /// Получить текущие данные КП
        /// </summary>
        private KPLogic.TagSrez GetKPCurData(KPLogic kpLogic, bool allData)
        {
            // получение текущих данных КП
            int tagCnt = kpLogic.KPTags.Length;
            SrezTableLight.CnlData[] curData = new SrezTableLight.CnlData[tagCnt];
            bool[] curDataMod = new bool[tagCnt];
            kpLogic.CopyCurData(curData, curDataMod);

            // создание среза передаваемых данных
            KPLogic.TagSrez curSrez = null;
            if (allData)
            {
                if (tagCnt > 0)
                {
                    curSrez = new KPLogic.TagSrez(tagCnt);
                    for (int i = 0; i < tagCnt; i++)
                    {
                        curSrez.KPTags[i] = kpLogic.KPTags[i];
                        curSrez.TagData[i] = curData[i];
                    }
                    serverComm.SendSrez(curSrez);
                }
            }
            else
            {
                int modTagCnt = 0; // количество изменившихся тегов
                for (int i = 0; i < tagCnt; i++)
                    if (curDataMod[i])
                        modTagCnt++;

                if (modTagCnt > 0)
                {
                    curSrez = new KPLogic.TagSrez(modTagCnt);
                    for (int i = 0, j = 0; i < tagCnt; i++)
                    {
                        if (curDataMod[i])
                        {
                            curSrez.KPTags[j] = kpLogic.KPTags[i];
                            curSrez.TagData[j] = curData[i];
                            j++;
                        }
                    }
                }
            }

            return curSrez;
        }

        /// <summary>
        /// Найти КП по номеру
        /// </summary>
        private KPLogic FindKP(int kpNum)
        {
            KPLogic kpLogic;
            return kpNumDict.TryGetValue(kpNum, out kpLogic) ? kpLogic : null;
        }


        /// <summary>
        /// Настроить линию связи в соответствии с таблицами базы конфигурации
        /// </summary>
        public void Tune(DataTable tblInCnl, DataTable tblKP)
        {
            try
            {
                if (Bind)
                {
                    foreach (KPLogic kpLogic in KPList)
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
                log.WriteAction(string.Format(Localization.UseRussian ?
                    "Ошибка при настройке линии связи {0}: {1}" :
                    "Error tuning communication line {0}: {1}", Number, ex.Message), 
                    Log.ActTypes.Exception);
            }
        }

        /// <summary>
        /// Запустить работу линии связи
        /// </summary>
        public void Start()
        {
            if (thread == null)
            {
                thread = new Thread(new ThreadStart(Execute));
                thread.Start();
            }
        }

        /// <summary>
        /// Завершить работу линии связи
        /// </summary>
        public void Terminate()
        {
            workState = thread == null ? WorkStates.Terminated : WorkStates.Terminating;
            foreach (KPLogic kpLogic in KPList)
                kpLogic.Terminated = true;
            WriteInfo();
        }

        /// <summary>
        /// Прервать работу линии связи
        /// </summary>
        public void Abort()
        {
            if (thread != null && thread.ThreadState != ThreadState.Stopped)
            {
                thread.Abort();
                thread = null;
            }
        }
        
        /// <summary>
        /// Добавить КП в последовательность опроса линии связи
        /// </summary>
        public void AddKP(KPLogic kpLogic)
        {
            CheckChangesAllowed();
            kpCaptions = null;

            // настройка свойств КП, относящихся к линии связи
            kpLogic.ReqTriesCnt = ReqTriesCnt;
            kpLogic.CustomParams = CustomParams;
            kpLogic.CommonProps = commonProps;
            kpLogic.AppDirs = appDirs;
            if (detailedLog)
                kpLogic.WriteToLog = log.WriteLine;
            kpLogic.CommLineSvc = this;

            // вызов метода обработки добавления КП
            try
            {
                kpLogic.OnAddedToCommLine();
            }
            catch (Exception ex)
            {
                kpLogic.WorkState = KPLogic.WorkStates.Error;
                log.WriteAction(string.Format(Localization.UseRussian ?
                    "Ошибка при выполнении действий {0} при добавлении КП на линию связи: {1}" :
                    "Error executing actions of {0} on adding device to communication line: {1}",
                    kpLogic.Caption, ex.Message));
            }

            // добавление КП в список опрашиваемых КП
            KPList.Add(kpLogic);

            // добавление КП в словари для быстрого поиска
            if (!kpNumDict.ContainsKey(kpLogic.Number))
                kpNumDict.Add(kpLogic.Number, kpLogic);

            int address = kpLogic.Address;
            string callNum = kpLogic.CallNum;

            if (!kpAddrDict.ContainsKey(address))
                kpAddrDict.Add(address, kpLogic);

            if (callNum != null && !kpCallNumDict.ContainsKey(callNum))
                kpCallNumDict.Add(callNum, kpLogic);

            KPFullAddr kpFullAddr = new KPFullAddr(address, callNum);
            if (!kpFullAddrDict.ContainsKey(kpFullAddr))
                kpFullAddrDict.Add(kpFullAddr, kpLogic);
        }

        /// <summary>
        /// Добавить команду в очередь команд линии связи, если команды ТУ разрешены
        /// </summary>
        public void EnqueueCmd(Command cmd)
        {
            try
            {
                if (cmdEnabled)
                {
                    lock (cmdList)
                        cmdList.Add(cmd);
                }
                else if (kpNumDict.ContainsKey(cmd.KPNum))
                {
                    log.WriteLine();
                    log.WriteAction(Localization.UseRussian ? 
                        "Выполнение команды ТУ не разрешено параметрами линии связи" :
                        "Command execution is disabled by the communication line parameters");
                }
            }
            catch (Exception ex)
            {
                log.WriteAction((Localization.UseRussian ? 
                    "Ошибка при добавлении команды в очередь: " : 
                    "Error adding command to the queue: ") + ex.Message);
            }
        }


        /// <summary>
        /// Найти КП на линии связи по адресу и позывному
        /// </summary>
        /// <remarks>Если address меньше 0, то он не учитывается при поиске.
        /// Если позывной равен null, то он не учитывается при поиске.</remarks>
        KPLogic ICommLineService.FindKPLogic(int address, string callNum)
        {
            KPLogic kpLogic;

            if (address < 0 && callNum == null)
            {
                return null;
            }
            else if (address < 0)
            {
                lock (kpCallNumDict)
                    return kpCallNumDict.TryGetValue(callNum, out kpLogic) ? kpLogic : null;
            }
            else if (callNum == null)
            {
                lock (kpAddrDict)
                    return kpAddrDict.TryGetValue(address, out kpLogic) ? kpLogic : null;
            }
            else
            {
                lock (kpFullAddrDict)
                    return kpFullAddrDict.TryGetValue(new KPFullAddr(address, callNum), out kpLogic) ? kpLogic : null;
            }
        }

        /// <summary>
        /// Форсированно передать текущие данные SCADA-Серверу
        /// </summary>
        bool ICommLineService.FlushCurData(KPLogic kpLogic)
        {
            if (kpLogic == null)
                return false;

            if (serverComm == null)
                return true;

            try
            {
                lock (serverLock)
                {
                    curAction = DateTime.Now.ToLocalizedString() + (Localization.UseRussian ?
                        " Форсированная передача текущих данных SCADA-серверу" :
                        " Flushing current data to SCADA-Server");
                    WriteInfo();

                    KPLogic.TagSrez curSrez = GetKPCurData(kpLogic, false);
                    return serverComm.SendSrez(curSrez);
                }
            }
            catch (Exception ex)
            {
                log.WriteAction((Localization.UseRussian ?
                    "Ошибка при форсированной передаче текущих данных SCADA-серверу: " :
                    "Error flushing current data to SCADA-Server") + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Форсированно передать архивные данные и события SCADA-Серверу
        /// </summary>
        bool ICommLineService.FlushArcData(KPLogic kpLogic)
        {
            if (kpLogic == null)
                return false;

            if (serverComm == null)
                return true;

            bool arcOK = true; // передача архивных срезов успешна
            bool evOK = true;  // передача событий успешна

            try
            {
                lock (serverLock)
                {
                    curAction = DateTime.Now.ToLocalizedString() + (Localization.UseRussian ?
                        " Форсированная передача архивов SCADA-серверу" :
                        " Flushing archives to SCADA-Server");
                    WriteInfo();

                    // передача архивных срезов до передачи всех срезов или возникновения ошибки
                    kpLogic.MoveArcSrez(unsentSrezList);

                    while (unsentSrezList.Count > 0 && arcOK)
                    {
                        if (serverComm.SendArchive(unsentSrezList[0]))
                        {
                            unsentSrezList.RemoveAt(0);
                        }
                        else
                        {
                            log.WriteAction(Localization.UseRussian ?
                                "Неудачная попытка форсированной передачи архивного среза SCADA-серверу" :
                                "Attempt to flush archive data to SCADA-Server failed");
                            arcOK = false;
                        }
                    }

                    // передача событий до передачи всех событий или возникновения ошибки
                    kpLogic.MoveEvents(unsentEventList);

                    while (unsentEventList.Count > 0 && evOK)
                    {
                        if (serverComm.SendEvent(unsentEventList[0]))
                        {
                            unsentEventList.RemoveAt(0);
                        }
                        else
                        {
                            log.WriteAction(Localization.UseRussian ?
                                "Неудачная попытка форсированной передачи события SCADA-серверу" :
                                "Attempt to flush event to SCADA-Server failed");
                            evOK = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.WriteAction((Localization.UseRussian ?
                    "Ошибка при форсированной передаче архивов SCADA-серверу: " : 
                    "Error flushing archives to SCADA-Server") + ex.Message);
            }

            return arcOK && evOK;
        }

        /// <summary>
        /// Передать команду ТУ, адресованную КП на данной линии связи
        /// </summary>
        void ICommLineService.PassCmd(Command cmd)
        {
            if (PassCmd != null)
                PassCmd(cmd);
        }


        /// <summary>
        /// Создать КП
        /// </summary>
        private static KPLogic CreateKPLogic(int kpNum, string dllName, 
            AppDirs appDirs, Dictionary<string, Type> kpTypes, Log appLog)
        {
            Type kpType;
            if (kpTypes.TryGetValue(dllName, out kpType))
            {
                return KPFactory.GetKPLogic(kpType, kpNum);
            }
            else
            {
                appLog.WriteAction((Localization.UseRussian ?
                    "Загрузка библиотеки КП: " :
                    "Load device library: ") + dllName, Log.ActTypes.Action);

                KPLogic kpLogic = KPFactory.GetKPLogic(appDirs.KPDir, dllName, kpNum);
                kpTypes.Add(dllName, kpLogic.GetType());
                return kpLogic;
            }
        }

        /// <summary>
        /// Создать линию связи и КП на основе настроек
        /// </summary>
        public static CommLine Create(Settings.CommLine commLineSett, Settings.CommonParams commonParams,
            AppDirs appDirs, PassCmdDelegate passCmd, Dictionary<string, Type> kpTypes, Log appLog)
        {
            if (!commLineSett.Active)
                return null;

            // создание линии связи
            CommLine commLine = new CommLine(commLineSett.Bind, commLineSett.Number, commLineSett.Name);

            // установка свойств для связи с окружением
            commLine.AppDirs = appDirs;
            commLine.ServerComm = null;
            commLine.PassCmd = passCmd;

            // вывод начальной информации о линии связи
            commLine.WriteInitialInfo();

            // установка параметров работы линии связи
            commLine.ReqTriesCnt = commLineSett.ReqTriesCnt;
            commLine.CycleDelay = Math.Max(commLineSett.CycleDelay, MinCycleDelay);
            commLine.CmdEnabled = commLineSett.CmdEnabled;
            commLine.ReqAfterCmd = commLineSett.ReqAfterCmd;
            commLine.DetailedLog = commLineSett.DetailedLog;
            commLine.SendAllDataPer = commonParams.SendAllDataPer;

            foreach (KeyValuePair<string, string> customParam in commLineSett.CustomParams)
                commLine.CustomParams.Add(customParam.Key, customParam.Value);

            // создание КП на линии связи
            foreach (Settings.KP kpSett in commLineSett.ReqSequence)
            {
                if (kpSett.Active)
                {
                    KPLogic kpLogic = CreateKPLogic(kpSett.Number, kpSett.Dll, appDirs, kpTypes, appLog);
                    kpLogic.Bind = kpSett.Bind;
                    kpLogic.Name = kpSett.Name;
                    kpLogic.Address = kpSett.Address;
                    kpLogic.CallNum = kpSett.CallNum;
                    kpLogic.ReqParams = new KPReqParams()
                    {
                        Timeout = kpSett.Timeout,
                        Delay = kpSett.Delay,
                        Time = kpSett.Time,
                        Period = kpSett.Period,
                        CmdLine = kpSett.CmdLine
                    };
                    commLine.AddKP(kpLogic);
                }
            }

            // создание канала связи
            if (!string.IsNullOrEmpty(commLineSett.CommCnlType))
            {
                commLine.CommChannelLogic = CommChannelFactory.GetCommChannel(commLineSett.CommCnlType);
                commLine.CommChannelLogic.WriteToLog = commLine.log.WriteLine;

                try
                {
                    commLine.CommChannelLogic.Init(commLineSett.CommCnlParams, commLine.KPList);
                }
                catch (Exception ex)
                {
                    string errMsg = (Localization.UseRussian ?
                        "Ошибка при инициализации канала связи: " :
                        "Error initializing communication channel: ") + ex.Message;
                    commLine.log.WriteAction(errMsg);
                    throw new Exception(errMsg);
                }
            }

            return commLine;
        }
    }
}