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
 * Summary  : Communication line
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2006
 * Modified : 2015
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Text;
using System.Threading;
using Utils;
using Scada.Data;
using Scada.Comm.Devices;
using Scada.Comm.Layers;
using System.Data;
using System.Reflection;

namespace Scada.Comm.Svc
{
    /// <summary>
    /// Communication line
    /// <para>Линия связи</para>
    /// </summary>
    internal sealed class CommLine : ICommLineService
    {
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
        /// Пауза перед следующей попыткой открытия COM-порта, мс
        /// </summary>
        private const int PortWaitDelay = 10000;
        /// <summary>
        /// Обозначение отсутствия действия
        /// </summary>
        private static readonly string NoAction = Localization.UseRussian ? "нет" : "no";

        private string numAndName;               // номер и нименование линии связи
        private CommLayerLogic commLayer;        // канал связи с физическими КП
        private SerialPort serialPort;           // последоватеьный порт
        private int reqTriesCnt;                 // количество попыток перезапроса КП при ошибке
        private int cycleDelay;                  // пауза после цикла опроса, мс
        private bool cmdEnabled;                 // разрешены ли команды ТУ
        private bool detailedLog;                // признак записи в журнал линии связи подробной информации
        private int sendAllDataPer;              // период передачи на сервер всех данных КП, с
        private AppDirs appDirs;                 // директории приложения
        private ServerCommEx serverComm;         // ссылка на объект обмена данными со SCADA-Сервером
        private PassCmdDelegate passCmd;         // метод передачи команды КП всем линиям связи

        private Thread thread;                   // поток работы линии связи
        private SortedList<string, object> commonProps; // общие свойства линии связи, доступные её КП
        private HashSet<int> kpNumSet;           // множество номеров КП на линии связи
        private List<Command> cmdList;           // список команд для выполнения
        private WorkStates workState;            // состояние работы линии связи
        private Log log;                         // журнал линии связи
        private string logPrefix;                // префикс имён файлов журналов, относящихся к линии связи
        private string infoFileName;             // имя файла информации о работе линии связи
        private string curAction;                // описание текущего действия
        private KPLogic curKP;                   // опрашиваемый в данный момент КП
        private string captionUnd;               // подчёркивание обозначения линии связи
        private string allCustomParams;          // все имена и значения пользовательских параметров
        private string[] kpCaptions;             // обозначения КП

        private object cmdLock;                  // объект для синхронизации доступа к списку команд
        private object infoLock;                 // объект для синхронизации записи в файл информации о работе линии связи
        private object flushLock;                // объект для синхронизации форсированной передачи архивов
        

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
            commLayer = null;
            serialPort = null;
            reqTriesCnt = 1;
            cycleDelay = 0;
            cmdEnabled = false;
            detailedLog = true;
            sendAllDataPer = 0;
            appDirs = new AppDirs();
            serverComm = null;
            passCmd = null;

            thread = null;
            commonProps = new SortedList<string, object>();
            kpNumSet = new HashSet<int>();
            cmdList = new List<Command>();
            workState = WorkStates.Idle;
            log = new Log(Log.Formats.Simple) { Encoding = Encoding.UTF8 };
            logPrefix = AppDirs.LogDir + "line" + CommUtils.AddZeros(number, 3);
            log.FileName = logPrefix + ".log";
            infoFileName = logPrefix + ".txt";
            curAction = NoAction;
            curKP = null;
            allCustomParams = null;
            kpCaptions = null;

            cmdLock = new object();
            infoLock = new object();
            flushLock = new object();

            // свойства
            Bind = bind;
            Number = number;
            Name = name;
            Caption = (Localization.UseRussian ? "Линия " : "Line ") + numAndName;
            CustomParams = new SortedList<string, string>();
            KPList = new List<KPLogic>();
            
            // ещё одно поле
            captionUnd = new string('-', Caption.Length);

            // вывод в журнал
            WriteInfo();
            log.WriteBreak();
            log.WriteAction((Localization.UseRussian ? 
                "Инициализация линии связи " : 
                "Initialize communication line ") + numAndName);
        }


        /// <summary>
        /// Получить признак привязки к SCADA-Серверу
        /// </summary>
        public bool Bind { get; private set; }

        /// <summary>
        /// Получить номер КП
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
        public CommLayerLogic CommLayerLogic
        {
            get
            {
                return commLayer;
            }
            set
            {
                CheckIdleState();
                commLayer = value;
            }
        }

        /// <summary>
        /// Получить или установить последоватеьный порт
        /// </summary>
        [Obsolete("Use Connection property")]
        public SerialPort SerialPort
        {
            get
            {
                return serialPort;
            }
            set
            {
                CheckIdleState();
                serialPort = value;
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
                CheckIdleState();
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
                CheckIdleState();
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
                CheckIdleState();
                cmdEnabled = value;
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
                CheckIdleState();
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
                CheckIdleState();
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
                CheckIdleState();
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
                CheckIdleState();
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
        /// Проверить, что линия связи бездействует
        /// </summary>
        private void CheckIdleState()
        {
            if (workState != WorkStates.Idle)
                throw new InvalidOperationException(Localization.UseRussian ?
                    "Невозможно выполнить операцию, если линия связи не бездействует." :
                    "Unable to perform the operation if the communication line is not idle.");
        }

        /// <summary>
        /// Освобождение ресурсов линии связи
        /// </summary>
        private void Clean()
        {
            try
            {
                if (serialPort != null)
                {
                    serialPort.Close();
                    serialPort = null;
                }
            }
            catch (Exception ex)
            {
                log.WriteAction(string.Format(Localization.UseRussian ? 
                    "Ошибка при освобождении ресурсов линии связи {0}: {1}" : 
                    "Error releasing communication line {0} resources: {1}", numAndName, ex.Message));
            }
        }

        /// <summary>
        /// Записать в файл информацию о работе линии связи
        /// </summary>
        private void WriteInfo()
        {
            Monitor.Enter(infoLock);

            try
            {
                StringBuilder sbInfo = new StringBuilder();
                sbInfo
                    .AppendLine(Caption)
                    .AppendLine(captionUnd);

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
                AppendCustomParams(sbInfo);
                AppendCommonProps(sbInfo);
                AppendActiveKP(sbInfo);

                // запись в файл
                using (StreamWriter writer = new StreamWriter(infoFileName, false, Encoding.UTF8))
                    writer.Write(sbInfo.ToString());
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
            finally
            {
                Monitor.Exit(infoLock);
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
                    kpInfoFileName = "_kp00" + kpNum + ".txt";
                else if (kpNum < 100) 
                    kpInfoFileName = "_kp0" + kpNum + ".txt";
                else 
                    kpInfoFileName = "_kp" + kpNum + ".txt";

                // запись информации
                using (StreamWriter writer = new StreamWriter(logPrefix + kpInfoFileName, false, Encoding.UTF8))
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
        /// Форсировано передать архивы срезов и событий SCADA-серверу
        /// </summary>
        private bool FlushArc(KPLogic kpLogic)
        {
            if (kpLogic == null)
                return false;

            Monitor.Enter(flushLock);
            bool arcOk = false; // передача архивных срезов успешна
            bool evOk = false;  // передача событий успешна

            /*try
            {
                if (serverComm == null)
                {
                    arcOk = true;
                    evOk = true;
                }
                else
                {
                    curAction = DateTime.Now.ToString("T", Localization.Culture) + (Localization.UseRussian ? 
                        " Форсированная передача архивов SCADA-серверу" : " Flushing archives to SCADA-Server");
                    WriteInfo();


                    // передача архивных срезов (без повторных попыток)
                    arcOk = true;
                    while (kpLogic.SrezList.Count > 0)
                    {
                        KPLogic.ParamSrez arcSrez = kpLogic.SrezList[0];
                        arcOk = serverComm.SendArchive(arcSrez);

                        if (arcOk)
                        {
                            kpLogic.CopySrezToBuf(0);
                            kpLogic.SrezList.RemoveAt(0);
                        }
                        else
                        {
                            log.WriteAction(Localization.UseRussian ? 
                                "Неудачная попытка форсированной передачи архивного среза SCADA-серверу" :
                                "Attempt to flush archive data to SCADA-Server failed");
                            break;
                        }
                    }

                    // передача событий (без повторных попыток)
                    if (arcOk)
                    {
                        evOk = true;
                        while (kpLogic.EventList.Count > 0)
                        {
                            KPLogic.Event ev = kpLogic.EventList[0];
                            evOk = serverComm.SendEvent(ev);

                            if (evOk)
                            {
                                kpLogic.CopyEventToBuf(0);
                                kpLogic.EventList.RemoveAt(0);
                            }
                            else
                            {
                                log.WriteAction(Localization.UseRussian ?
                                    "Неудачная попытка форсированной передачи события SCADA-серверу" :
                                    "Attempt to flush event to SCADA-Server failed");
                                break;
                            }
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
            finally
            {
                Monitor.Exit(flushLock);
            }*/

            return arcOk && evOk;
        }

        /// <summary>
        /// Вызвать метод передачи команды КП
        /// </summary>
        private void ExecPassCmd(Command cmd)
        {
            /*if (PassCmd != null)
                PassCmd(cmd);*/
        }

        /// <summary>
        /// Передать данные опрашиваемого КП SCADA-Серверу
        /// </summary>
        private void SendDataToServer(bool sendAllCurData)
        {
            /*if (serverComm != null)
            {
                curAction = DateTime.Now.ToString("T", Localization.Culture) + (Localization.UseRussian ? 
                    " Передача данных SCADA-серверу" : " Sending data to SCADA-Server");
                WriteInfo();

                // передача текущего среза
                if (sendAllCurData)
                {
                    int cnt = curKP.KPParams == null ? 0 : curKP.KPParams.Length;
                    if (cnt > 0)
                    {
                        KPLogic.ParamSrez curSrez = new KPLogic.ParamSrez(cnt);
                        for (int i = 0; i < cnt; i++)
                        {
                            curSrez.KPParams[i] = curKP.KPParams[i];
                            curSrez.Data[i] = curKP.CurData[i];
                        }
                        serverComm.SendSrez(curSrez);
                    }
                }
                else
                {
                    List<int> modifiedIndexes = curKP.GetModifiedParamIndexes();
                    int cnt = modifiedIndexes.Count;
                    if (cnt > 0)
                    {
                        KPLogic.ParamSrez curSrez = new KPLogic.ParamSrez(cnt);
                        for (int i = 0; i < cnt; i++)
                        {
                            int index = modifiedIndexes[i];
                            curSrez.KPParams[i] = curKP.KPParams[index];
                            curSrez.Data[i] = curKP.CurData[index];
                        }
                        serverComm.SendSrez(curSrez);
                    }
                }

                // передача архивных срезов
                if (curKP.SrezList.Count > 0)
                {
                    foreach (KPLogic.ParamSrez arcSrez in curKP.SrezList)
                    {
                        int errCnt = 0;
                        while (!serverComm.SendArchive(arcSrez))
                        {
                            // ожидание успешной передачи архивного среза
                            if (++errCnt == 5)
                            {
                                log.WriteAction(Localization.UseRussian ? 
                                    "Пауза перед следующей попыткой передачи архивного среза SCADA-серверу" :
                                    "Delay before the next attempt to send data to SCADA-Server");
                                Thread.Sleep(10000);
                            }
                            else
                            {
                                if (errCnt == 1)
                                    log.WriteLine();
                                log.WriteAction(Localization.UseRussian ?
                                    "Неудачная попытка передачи архивного среза SCADA-серверу" :
                                    "Attempt to send archive data to SCADA-Server failed");
                                Thread.Sleep(1000);
                            }
                        }
                    }
                }

                // передача событий
                if (curKP.EventList.Count > 0)
                {
                    foreach (KPLogic.Event ev in curKP.EventList)
                    {
                        int errCnt = 0;
                        while (!serverComm.SendEvent(ev))
                        {
                            // ожидание успешной передачи события
                            if (++errCnt == 5)
                            {
                                log.WriteAction(Localization.UseRussian ?
                                    "Пауза перед следующей попыткой передачи события SCADA-серверу" :
                                    "Delay before the next attempt to send event to SCADA-Server");
                                Thread.Sleep(10000);
                            }
                            else
                            {
                                if (errCnt == 1)
                                    log.WriteLine();
                                log.WriteAction(Localization.UseRussian ?
                                    "Неудачная попытка передачи события SCADA-серверу" :
                                    "Attempt to send event to SCADA-Server failed");
                                Thread.Sleep(1000);
                            }
                        }
                    }
                }
            }*/
        }

        /// <summary>
        /// Найти КП по номеру
        /// </summary>
        private KPLogic FindKP(int kpNum)
        {
            foreach (KPLogic kpLogic in KPList)
                if (kpLogic.Number == kpNum)
                    return kpLogic;
            return null;
        }

        /// <summary>
        /// Цикл работы линии связи
        /// </summary>
        private void WorkCycle()
        {
            bool terminateCycle = false;        // завершить цикл работы
            DateTime lastRefrDT = DateTime.Now; // время последней передачи на сервер всех параметров КП для обновления

            while (!terminateCycle)
            {
                int commCnt = 0; // количество выполненных сеансов связи (команд ТУ и опросов КП)
                int kpInd = 0;   // индекс опрашиваемого КП
                KPLogic extraKP = null; // КП для внеочередного опроса

                // определение необходимости передать на сервер все текущие данные параметров КП
                DateTime nowDT = DateTime.Now;
                bool sendAllCurData = sendAllDataPer > 0 && (nowDT - lastRefrDT).TotalSeconds >= sendAllDataPer;
                if (sendAllCurData)
                    lastRefrDT = nowDT;

                while (kpInd < KPList.Count && !terminateCycle)
                {
                    // обработка команд ТУ
                    try
                    {
                        curAction = DateTime.Now.ToLocalizedString() + 
                            (Localization.UseRussian ? " Обработка команд ТУ" : " Processing commands");
                        WriteInfo();

                        // копирование команд в буферы, чтобы минимизировать время блокировки списка команд
                        List<Command> cmdBufList = null; // буфер стандартных и бинарных команд
                        Command reqCmd = null;           // буфер команды внеочередного опроса КП

                        Monitor.Enter(cmdLock);
                        try
                        {
                            if (cmdList.Count > 0)
                            {
                                cmdBufList = new List<Command>();
                                int cmdInd = 0;

                                while (cmdInd < cmdList.Count)
                                {
                                    Command cmd = cmdList[cmdInd];
                                    bool delCmd = false;

                                    if (cmd.CmdTypeID == BaseValues.CmdTypes.Request)
                                    {
                                        if (reqCmd == null)
                                        {
                                            reqCmd = cmd;
                                            delCmd = true;
                                        }
                                    }
                                    else
                                    {
                                        cmdBufList.Add(cmd);
                                        delCmd = true;
                                    }

                                    if (delCmd)
                                        cmdList.RemoveAt(cmdInd);
                                    else
                                        cmdInd++;
                                }
                            }
                        }
                        finally
                        {
                            Monitor.Exit(cmdLock);
                        }

                        // выполнение команд ТУ
                        if (cmdBufList != null)
                        {
                            foreach (Command cmd in cmdBufList)
                            {
                                KPLogic kpLogic = FindKP(cmd.KPNum);

                                if (kpLogic != null)
                                {
                                    curAction = DateTime.Now.ToLocalizedString() + 
                                        (Localization.UseRussian ? " Команда " : " Command to ") + kpLogic.Caption;
                                    WriteInfo();

                                    try
                                    {
                                        kpLogic.SendCmd(cmd);
                                        WriteKPInfo(kpLogic);
                                        commCnt++;
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
                                            "Ошибка при отправке команды ТУ: " :
                                            "Error sending command: ") + ex.Message);
                                    }
                                }
                            }
                        }

                        // обработка команды внеочередного опроса
                        if (reqCmd != null)
                            extraKP = FindKP(reqCmd.KPNum);
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

                    // сеанс опроса КП
                    try
                    {
                        curAction = DateTime.Now.ToLocalizedString() + 
                            (Localization.UseRussian ? " Выбор КП для связи" : " Choosing device for communication");
                        WriteInfo();

                        // определение необходимости опроса КП
                        bool sessionNeeded = true; // необходимо выполнить сеанс опроса КП
                        KPLogic kpLogic;           // КП, который необходимо опросить

                        if (extraKP == null)
                        {
                            kpLogic = KPList[kpInd];
                            double reqPeriod = kpLogic.ReqParams.Period.TotalDays; // период опроса КП, дн.

                            if (kpLogic.ReqParams.Time > DateTime.MinValue || reqPeriod > 0)
                            {
                                TimeSpan nowTime = DateTime.Now.TimeOfDay;            // текущее время
                                TimeSpan reqTime = kpLogic.ReqParams.Time.TimeOfDay;  // время опроса КП
                                TimeSpan lastSessTime = kpLogic.LastSessDT.TimeOfDay; // время последнего сеанса связи с КП

                                if (reqPeriod > 0)
                                {
                                    double t = (nowTime - reqTime).TotalDays / reqPeriod;
                                    sessionNeeded = t >= 0 &&
                                        ((int)t * reqPeriod + reqTime.TotalDays > lastSessTime.TotalDays ||
                                        lastSessTime > nowTime /*новый день*/);
                                }
                                else
                                {
                                    sessionNeeded = reqTime.TotalDays <= 0 ||
                                        reqTime <= nowTime && reqTime > lastSessTime;
                                }
                            }
                        }
                        else
                        {
                            kpLogic = extraKP;
                        }

                        // установка опрашиваемого в данный момент КП
                        curKP = kpLogic;
                        curKP.Terminated = workState == WorkStates.Terminating;

                        // выполнение сеанса опроса КП
                        if (sessionNeeded)
                        {
                            curAction = DateTime.Now.ToLocalizedString() + 
                                (Localization.UseRussian ? " Сеанс связи с " : " Communication with ") + 
                                kpLogic.Caption;
                            WriteInfo();

                            curKP.Session();
                            //curKP.CopyArcToBuf();
                            WriteKPInfo(curKP);
                            commCnt++;
                        }

                        // передача данных текущего КП на сервер
                        if (sessionNeeded || sendAllCurData)
                        {
                            SendDataToServer(sendAllCurData);
                            //curKP.SrezList.Clear();
                            //curKP.EventList.Clear();
                        }

                        // определение необходимости завершить цикл работы
                        terminateCycle = workState == WorkStates.Terminating && curKP.Terminated;

                        // обнуление опрашиваемого в данный момент КП
                        curKP = null;
                    }
                    catch (ThreadAbortException)
                    {
                        // обработка данного исключения реализована в методе Execute
                    }
                    catch (Exception ex)
                    {
                        curKP = null;
                        curAction = NoAction;
                        WriteInfo();
                        log.WriteAction((Localization.UseRussian ? 
                            "Ошибка сеанса связи с КП: " : 
                            "Error communicating with the device: ") + ex.Message);
                    }

                    if (extraKP == null)
                        kpInd++;
                    else
                        extraKP = null;
                }

                // задержка после цикла опроса линии связи
                if (workState != WorkStates.Terminating)
                {
                    if (commCnt == 0 && cycleDelay == 0)
                        Thread.Sleep(200);
                    else
                        Thread.Sleep(cycleDelay);
                }
            }
        }

        /// <summary>
        /// Запуск, работа и остановка линии связи, метод вызывается в отдельном потоке
        /// </summary>
        private void Execute()
        {
            try
            {
                log.WriteAction((Localization.UseRussian ? 
                    "Запуск линии связи " : 
                    "Start communication line " ) + numAndName);
                workState = WorkStates.Running;
                allCustomParams = null;
                kpCaptions = null;
                WriteInfo();

                if (KPList.Count == 0)
                {
                    log.WriteAction(Localization.UseRussian ? 
                        "Работа линии связи невозможна из-за отсутствия КП" :
                        "Communication line execution is impossible due to device missing");
                }
                else
                {
                    // открытие порта
                    if (serialPort != null)
                    {
                        bool portOk;
                        do
                        {
                            try
                            {
                                serialPort.Open();
                                portOk = true;
                            }
                            catch (Exception ex)
                            {
                                portOk = false;
                                log.WriteAction((Localization.UseRussian ? 
                                    "Ошибка при открытии COM-порта: " :
                                    "Error opening serial port: ") + ex.Message);
                            }

                            if (!portOk)
                            {
                                // паузу нужно делать вне catch, иначе поток не может прерваться
                                log.WriteAction(Localization.UseRussian ? 
                                    "Пауза перед следующей попыткой открытия COM-порта" :
                                    "Delay before the next attempt to open serial port");
                                Thread.Sleep(PortWaitDelay);
                            }
                        }
                        while (!portOk);
                    }

                    // подготовка КП к работе
                    foreach (KPLogic kpLogic in KPList)
                    {
                        try
                        {
                            kpLogic.OnCommLineStart();
                            WriteKPInfo(kpLogic);
                        }
                        catch (Exception ex)
                        {
                            log.WriteAction(string.Format(Localization.UseRussian ? 
                                "Ошибка при выполнении действий {0} при запуске линии связи: {1}" : 
                                "Error executing actions of {0} on communication line start: {1}", 
                                kpLogic.Caption, ex.Message));
                        }
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
                    }
                    catch (Exception ex)
                    {
                        log.WriteAction(string.Format(Localization.UseRussian ?
                            "Ошибка при выполнении действий {0} при прерывании работы линии связи: {1}" :
                            "Error executing actions of {0} on communication line abort: {1}",
                            kpLogic.Caption, ex.Message));
                    }
                }

                Clean();

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
                        }
                        catch (Exception ex)
                        {
                            log.WriteAction(string.Format(Localization.UseRussian ?
                                "Ошибка при выполнении действий {0} при завершении работы линии связи: {1}" :
                                "Error executing actions of {0} on communication line terminating: {1}",
                                kpLogic.Caption, ex.Message));
                        }
                    }

                    Clean();

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
            CheckIdleState();
            kpCaptions = null;

            // настройка свойств КП, относящихся к линии связи
            kpLogic.ReqTriesCnt = ReqTriesCnt;
            //kpLogic.Connection = null;
            //kpLogic.SerialPort = null;
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
                log.WriteAction(string.Format(Localization.UseRussian ?
                    "Ошибка при выполнении действий {0} при добавлении КП на линию связи: {1}" :
                    "Error executing actions of {0} on adding device to communication line: {1}",
                    kpLogic.Caption, ex.Message));
            }

            // добавление КП в список опрашиваемых КП
            KPList.Add(kpLogic);
            kpNumSet.Add(kpLogic.Number);
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
                    lock (cmdLock)
                        cmdList.Add(cmd);
                }
                else if (kpNumSet.Contains(cmd.KPNum))
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


        KPLogic ICommLineService.FindKPLogic(int number, string callNum)
        {
            throw new NotImplementedException();
        }

        bool ICommLineService.FlushCurData(KPLogic kpLogic)
        {
            throw new NotImplementedException();
        }

        bool ICommLineService.FlushArcData(KPLogic kpLogic)
        {
            throw new NotImplementedException();
        }

        void ICommLineService.PassCmd(Command cmd)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Создать КП
        /// </summary>
        private static KPLogic CreateKPLogic(int kpNum, string dllName, 
            AppDirs appDirs, Dictionary<string, Type> kpTypes, Log log)
        {
            // получение типа КП
            Type kpType = null;
            try
            {
                if (!kpTypes.TryGetValue(dllName, out kpType))
                {
                    // загрузка типа из библиотеки
                    string path = appDirs.KPDir + dllName + ".dll";
                    log.WriteAction((Localization.UseRussian ?
                        "Загрузка библиотеки КП: " :
                        "Load device library: ") + path, Log.ActTypes.Action);

                    Assembly asm = Assembly.LoadFile(path);
                    string typeFullName = "Scada.Comm.Devices." + dllName + "Logic";
                    kpType = asm.GetType(typeFullName, true);
                    kpTypes.Add(dllName, kpType);
                }
            }
            catch (Exception ex)
            {
                throw new Exception((Localization.UseRussian ?
                    "Ошибка при получении типа КП: " :
                    "Error getting device type: ") + ex.Message);
            }

            // создание экземпляра класса КП
            try
            {
                return (KPLogic)Activator.CreateInstance(kpType, kpNum);
            }
            catch (Exception ex)
            {
                throw new Exception((Localization.UseRussian ?
                    "Ошибка при создании экземпляра класса КП: " :
                    "Error creating device class instance: ") + ex.Message);
            }
        }

        /// <summary>
        /// Создать линию связи и КП на основе настроек
        /// </summary>
        public static CommLine Create(Settings.CommLine commLineSett, Settings.CommonParams commonParams,
            AppDirs appDirs, PassCmdDelegate passCmd, Dictionary<string, Type> kpTypes, Log log)
        {
            if (!commLineSett.Active)
                return null;

            // создание и установка свойств линии связи
            CommLine commLine = new CommLine(commLineSett.Bind, commLineSett.Number, commLineSett.Name);
            commLine.ReqTriesCnt = commLineSett.ReqTriesCnt;
            commLine.CycleDelay = commLineSett.CycleDelay;
            commLine.CmdEnabled = commLineSett.CmdEnabled;
            commLine.DetailedLog = commLineSett.DetailedLog;
            commLine.SendAllDataPer = commonParams.SendAllDataPer;

            foreach (Settings.CustomParam customParam in commLineSett.CustomParams)
            {
                if (!commLine.CustomParams.ContainsKey(customParam.Name))
                    commLine.CustomParams.Add(customParam.Name, customParam.Value);
            }

            // создание КП на линии связи
            foreach (Settings.KP kpSett in commLineSett.ReqSequence)
            {
                if (kpSett.Active)
                {
                    KPLogic kpLogic = CreateKPLogic(kpSett.Number, kpSett.Dll, appDirs, kpTypes, log);
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

            // установка свойств для связи с окружением
            commLine.AppDirs = appDirs;
            commLine.ServerComm = null;
            commLine.PassCmd = passCmd;

            // создание канала связи
            commLine.CommLayerLogic = null;

            return commLine;
        }
    }
}
