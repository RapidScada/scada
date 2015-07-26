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

namespace Scada.Comm.Svc
{
    /// <summary>
    /// Communication line
    /// <para>Линия связи</para>
    /// </summary>
    internal sealed class CommLine
    {
        /// <summary>
        /// Состояния работы линии связи
        /// </summary>
        private enum RunStates
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
        /// Пауза перед следующей попыткой открытия COM-порта, мс
        /// </summary>
        private const int PortWaitDelay = 10000;
        /// <summary>
        /// Обозначение отсутствия действия
        /// </summary>
        private static readonly string NoAction = Localization.UseRussian ? "нет" : "no";

        private string numAndName;               // номер и нименование линии связи
        private bool configError;                // ошибка при конфигурировании линии связи
        private ServerCommEx serverComm;         // ссылка на объект обмена данными со SCADA-Сервером
        private SerialPort serialPort;           // последоватеьный порт
        //private KPLogic.PassCmdDelegate passCmd; // метод передачи команды КП
        private int reqTriesCnt;                 // количество попыток перезапроса КП при ошибке
        private int cicleDelay;                  // пауза после цикла опроса, мс
        private int maxCommErrCnt;               // количество неудачных сеансов связи до объявления КП неработающим
        private bool cmdEnabled;                 // разрешены ли команды ТУ
        private int refrParams;                  // период передачи на сервер всех параметров КП для обновления, с

        private SortedList<string, object> commonProps; // общие свойства линии связи, доступные относящимся к ней КП
        private HashSet<int> kpNumSet;           // множество номеров КП на линии связи
        private List<Command> cmdList;           // список команд
        private KPLogic curKP;                   // опрашиваемое в данный момент КП

        private Thread thread;                   // поток работы линии связи
        private RunStates runState;              // состояние работы линии связи
        private Log log;                         // журнал линии связи
        private string infoFileName;             // имя файла информации о работе линии связи
        private string curAction;                // описание текущего действия
        private int maxKPCapLen;                 // максимальная длина обозначения КП

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
            configError = false;
            serverComm = null;
            serialPort = null;
            //passCmd = null;
            reqTriesCnt = 1;
            cicleDelay = 0;
            maxCommErrCnt = 1;
            cmdEnabled = false;

            commonProps = new SortedList<string, object>();
            kpNumSet = new HashSet<int>();
            cmdList = new List<Command>();
            curKP = null;

            thread = null;
            runState = RunStates.Idle;
            log = new Log(Log.Formats.Simple) { Encoding = Encoding.UTF8 };
            string fileName = AppDirs.LogDir + "line" + CommUtils.AddZeros(number, 3);
            log.FileName = fileName + ".log";
            infoFileName = fileName + ".txt";
            curAction = NoAction;
            maxKPCapLen = -1;

            cmdLock = new object();
            infoLock = new object();
            flushLock = new object();

            // свойства
            Number = number;
            Name = name;
            Bind = bind;
            Caption = (Localization.UseRussian ? "Линия " : "Line ") + numAndName;

            CustomParams = new SortedList<string, string>();
            KPList = new List<KPLogic>();

            AppDirs = null;

            // вывод в журнал
            WriteInfo();
            log.WriteBreak();
            log.WriteAction((Localization.UseRussian ? "Инициализация линии связи " : 
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
        /// Получить пользовательские параметры линии связи
        /// </summary>
        public SortedList<string, string> CustomParams { get; private set; }

        /// <summary>
        /// Получить список опрашиваемых КП
        /// </summary>
        public List<KPLogic> KPList { get; private set; }

        /// <summary>
        /// Получить строковое представление состояния работы линии связи
        /// </summary>
        public string RunStateStr
        {
            get
            {
                switch (runState)
                {
                    case RunStates.Idle:
                        return Localization.UseRussian ? "бездействие" : "idle";
                    case RunStates.Running:
                        return Localization.UseRussian ? "цикл работы" : "running";
                    case RunStates.Terminating:
                        return Localization.UseRussian ? "завершение работы" : "terminating";
                    case RunStates.Terminated:
                        return Localization.UseRussian ? "работа завершена" : "terminated";
                    case RunStates.Aborted:
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
                return runState == RunStates.Terminated;
            }
        }


        /// <summary>
        /// Получить или установить признак ошибки при конфигурировании линии связи
        /// </summary>
        public bool ConfigError
        {
            get
            {
                return configError;
            }
            set
            {
                CheckIdleState();
                configError = value;
            }
        }

        /// <summary>
        /// Получить или установить директории приложения
        /// </summary>
        public AppDirs AppDirs { get; set; }

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
                serverComm = value;
            }
        }

        /// <summary>
        /// Получить или установить последоватеьный порт
        /// </summary>
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
        /// Получить или установить метод передачи команды КП
        /// </summary>
        /*public KPLogic.PassCmdDelegate PassCmd
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
        }*/

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
        public int CicleDelay
        {
            get
            {
                return cicleDelay;
            }
            set
            {
                CheckIdleState();
                cicleDelay = value;
            }
        }

        /// <summary>
        /// Получить или установить количество неудачных сеансов связи до объявления КП неработающим
        /// </summary>
        public int MaxCommErrCnt
        {
            get
            {
                return maxCommErrCnt;
            }
            set
            {
                CheckIdleState();
                maxCommErrCnt = value;
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
        /// Получить или установить период передачи на сервер всех параметров КП для обновления, с
        /// </summary>
        public int RefrParams
        {
            get
            {
                return refrParams;
            }
            set
            {
                CheckIdleState();
                refrParams = value;
            }
        }


        /// <summary>
        /// Проверить, что линия связи бездействует
        /// </summary>
        private void CheckIdleState()
        {
            if (runState != RunStates.Idle)
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
            StreamWriter writer = null;

            try
            {
                writer = new StreamWriter(infoFileName, false, Encoding.UTF8);
                writer.WriteLine(Caption);
                writer.WriteLine(new string('-', Caption.Length));

                if (Localization.UseRussian)
                {
                    writer.WriteLine("Состояние : " + RunStateStr);
                    writer.WriteLine("Действие  : " + curAction);
                    writer.WriteLine();

                    writer.WriteLine("Пользовательские параметры");
                    writer.WriteLine("--------------------------");
                    if (!WriteUserParams(writer))
                        writer.WriteLine("Нет");
                    writer.WriteLine();

                    writer.WriteLine("Общие свойства");
                    writer.WriteLine("--------------");
                    if (!WriteCommonProps(writer))
                        writer.WriteLine("Нет");
                    writer.WriteLine();

                    writer.WriteLine("Активные КП");
                    writer.WriteLine("-----------");
                    if (!WriteActiveKP(writer))
                        writer.WriteLine("Нет");
                }
                else
                {
                    writer.WriteLine("State  : " + RunStateStr);
                    writer.WriteLine("Action : " + curAction);
                    writer.WriteLine();

                    writer.WriteLine("Custom Parameters");
                    writer.WriteLine("-----------------");
                    if (!WriteUserParams(writer))
                        writer.WriteLine("No");
                    writer.WriteLine();

                    writer.WriteLine("Common Properties");
                    writer.WriteLine("-----------------");
                    if (!WriteCommonProps(writer))
                        writer.WriteLine("No");
                    writer.WriteLine();

                    writer.WriteLine("Active Devices");
                    writer.WriteLine("--------------");
                    if (!WriteActiveKP(writer))
                        writer.WriteLine("No");
                }
            }
            catch (Exception ex)
            {
                log.WriteAction((Localization.UseRussian ? 
                    "Ошибка при записи в файл информации о работе линии связи: " :
                    "Error writing communication line information to the file: ") + ex.Message);
            }
            finally
            {
                if (writer != null)
                    try { writer.Close(); }
                    catch { }
                Monitor.Exit(infoLock);
            }
        }

        /// <summary>
        /// Записать общие свойства линии связи
        /// </summary>
        private bool WriteUserParams(StreamWriter writer)
        {
            int cnt = CustomParams.Count;
            if (cnt > 0)
            {
                for (int i = 0; i < cnt; i++)
                    writer.WriteLine((i + 1).ToString() + ". " + CustomParams.Keys[i] + " = " + CustomParams.Values[i]);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Записать пользовательские параметры линии связи
        /// </summary>
        private bool WriteCommonProps(StreamWriter writer)
        {
            int cnt = commonProps.Count;
            if (cnt > 0)
            {
                for (int i = 0; i < cnt; i++)
                {
                    object val = commonProps.Values[i];
                    string valStr = val == null ? "null" : val.ToString();
                    writer.WriteLine((i + 1).ToString() + ". " + commonProps.Keys[i] + " = " + valStr);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Записать активые КП линии связи
        /// </summary>
        private bool WriteActiveKP(StreamWriter writer)
        {
            int cnt = KPList.Count;
            if (cnt > 0)
            {
                // вычисление максимальной длины обозначения КП
                int ordLen = KPList.Count.ToString().Length;
                if (maxKPCapLen < 0)
                {
                    for (int i = 0; i < cnt; i++)
                    {
                        KPLogic kpLogic = KPList[i];
                        int kpCapLen = ordLen + 2 + kpLogic.Caption.Length;
                        if (maxKPCapLen < kpCapLen)
                            maxKPCapLen = kpCapLen;
                    }
                }

                // вывод состояний работы активных КП
                for (int i = 0; i < cnt; i++)
                {
                    KPLogic kpLogic = KPList[i];
                    string kpCaption = (i + 1).ToString().PadLeft(ordLen) + ". " + kpLogic.Caption;
                    writer.WriteLine(kpCaption.PadRight(maxKPCapLen) + " : " + kpLogic.WorkStateStr);
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Записать в файл информацию о работе КП
        /// </summary>
        private void WriteKPInfo(KPLogic kpLogic)
        {
            StreamWriter writer = null;

            try
            {
                // определение имени файла
                string kpInfoFileName;
                int kpNum = kpLogic.Number;
                if (kpNum < 10) kpInfoFileName = "kp00" + kpNum + ".txt";
                else if (kpNum < 100) kpInfoFileName = "kp0" + kpNum + ".txt";
                else kpInfoFileName = "kp" + kpNum + ".txt";

                // запись информации
                writer = new StreamWriter(AppDirs.LogDir + kpInfoFileName, false, Encoding.UTF8);
                writer.Write(kpLogic.GetInfo());
            }
            catch (Exception ex)
            {
                log.WriteAction((Localization.UseRussian ?
                    "Ошибка при записи в файл информации о работе КП: " :
                    "Error writing device information to the file: ") + ex.Message);
            }
            finally
            {
                if (writer != null)
                {
                    try { writer.Close(); }
                    catch { }
                }
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
                bool sendAllCurData = refrParams > 0 && (nowDT - lastRefrDT).TotalSeconds >= refrParams;
                if (sendAllCurData)
                    lastRefrDT = nowDT;

                while (kpInd < KPList.Count && !terminateCycle)
                {
                    // обработка команд ТУ
                    try
                    {
                        curAction = DateTime.Now.ToString("T", Localization.Culture) + (Localization.UseRussian ? 
                            " Обработка команд ТУ" : " Processing commands");
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
                                    curAction = DateTime.Now.ToString("T", Localization.Culture) + 
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
                                        log.WriteAction((Localization.UseRussian ? "Ошибка при отправке команды ТУ: " :
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
                        log.WriteAction((Localization.UseRussian ? "Ошибка при обработке команд ТУ: " : 
                            "Error processing commands: ") + ex.Message);
                    }

                    // сеанс опроса КП
                    try
                    {
                        curAction = DateTime.Now.ToString("T", Localization.Culture) + (Localization.UseRussian ? 
                            " Выбор КП для связи" : " Choosing device for communication");
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
                        curKP.Terminated = runState == RunStates.Terminating;

                        // выполнение сеанса опроса КП
                        if (sessionNeeded)
                        {
                            curAction = DateTime.Now.ToString("T", Localization.Culture) + (Localization.UseRussian ? 
                                " Сеанс связи с " : " Communication with ") + kpLogic.Caption;
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
                        terminateCycle = runState == RunStates.Terminating && curKP.Terminated;

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
                        log.WriteAction((Localization.UseRussian ? "Ошибка сеанса связи с КП: " : 
                            "Error communicating with the device: ") + ex.Message);
                    }

                    if (extraKP == null)
                        kpInd++;
                    else
                        extraKP = null;
                }

                // задержка после цикла опроса линии связи
                if (runState != RunStates.Terminating)
                {
                    if (commCnt == 0 && cicleDelay == 0)
                        Thread.Sleep(200);
                    else
                        Thread.Sleep(cicleDelay);
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
                log.WriteAction((Localization.UseRussian ? "Запуск линии связи " : 
                    "Start communication line " ) + numAndName);
                runState = RunStates.Running;
                WriteInfo();

                if (configError)
                {
                    log.WriteAction(Localization.UseRussian ? 
                        "Работа линии связи невозможна из-за ошибки конфигурации" :
                        "Communication line execution is impossible due to configuration error");
                }
                else if (KPList.Count == 0)
                {
                    log.WriteAction(Localization.UseRussian ? "Работа линии связи невозможна из-за отсутствия КП" :
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
                                log.WriteAction((Localization.UseRussian ? "Ошибка при открытии COM-порта: " :
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
                log.WriteAction((Localization.UseRussian ? "Прерывание работы линии связи " : 
                    "Abort communication line ") + numAndName);
                log.WriteBreak();

                runState = RunStates.Aborted;
                curAction = NoAction;
                WriteInfo();
            }
            catch (Exception ex)
            {
                log.WriteAction(string.Format(Localization.UseRussian ? "Ошибка при работе линии связи {0}: {1}" : 
                    "Error communication line {0} execution: {1}", numAndName, ex.Message));
            }
            finally
            {
                if (runState != RunStates.Aborted)
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
                    log.WriteAction((Localization.UseRussian ? "Завершение работы линии связи " : 
                        "Stop communication line ") + numAndName);
                    log.WriteBreak();

                    runState = RunStates.Terminated;
                    curAction = NoAction;
                    WriteInfo();
                }

                thread = null;
            }

            // Do not put clean-up code here, because the exception 
            // is rethrown at the end of the Finally clause.
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
            runState = thread == null ? RunStates.Terminated : RunStates.Terminating;
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

            // настройка свойств КП
            /*kpLogic.SerialPort = serialPort;
            kpLogic.CommLineParams = new KPLogic.LineParams(reqTriesCnt, maxCommErrCnt);
            kpLogic.UserParams = UserParams;
            kpLogic.CommonProps = commonProps;
            kpLogic.ConfigDir = AppDirs.ConfigDir;
            kpLogic.LangDir = AppDirs.LangDir;
            kpLogic.LogDir = AppDirs.LogDir;
            kpLogic.CmdDir = AppDirs.CmdDir;
            kpLogic.FlushArc = FlushArc;
            kpLogic.WriteToLog = log.WriteLine;
            kpLogic.PassCmd = ExecPassCmd;*/

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
    }
}
