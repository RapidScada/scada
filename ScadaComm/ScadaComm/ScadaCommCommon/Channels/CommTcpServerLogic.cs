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
 * Module   : ScadaCommCommon
 * Summary  : TCP server communication channel logic
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2017
 */

using Scada.Comm.Devices;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Scada.Comm.Channels
{
    /// <summary>
    /// TCP server communication channel logic
    /// <para>Логика работы канала связи TCP-сервер</para>
    /// </summary>
    public class CommTcpServerLogic : CommTcpChannelLogic
    {
        /// <summary>
        /// Режимы выбора КП для обработки входящих запросов
        /// </summary>
        public enum DeviceSelectionModes
        {
            /// <summary>
            /// По IP-адресу
            /// </summary>
            ByIPAddress,
            /// <summary>
            /// По первому пакету данных
            /// </summary>
            ByFirstPackage,
            /// <summary>
            /// Используя библиотеку КП
            /// </summary>
            ByDeviceLibrary
        }

        /// <summary>
        /// Настройки канала связи
        /// </summary>
        public class Settings
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            public Settings()
            {
                // установка значений по умолчанию
                TcpPort = 1;
                InactiveTime = 60;
                Behavior = CommChannelLogic.OperatingBehaviors.Master;
                ConnMode = ConnectionModes.Individual;
                DevSelMode = CommTcpServerLogic.DeviceSelectionModes.ByIPAddress;
            }

            /// <summary>
            /// Получить или установить TCP-порт для входящих соединений
            /// </summary>
            public int TcpPort { get; set; }
            /// <summary>
            /// Получить или установить время неактивности TCP-соединения до его отключения, с
            /// </summary>
            public int InactiveTime { get; set; }
            /// <summary>
            /// Получить или установить режим работы канала связи
            /// </summary>
            public OperatingBehaviors Behavior { get; set; }
            /// <summary>
            /// Получить или установить режим соединения
            /// </summary>
            public ConnectionModes ConnMode { get; set; }
            /// <summary>
            /// Получить или установить режим выбора КП
            /// </summary>
            public DeviceSelectionModes DevSelMode { get; set; }

            /// <summary>
            /// Инициализировать настройки на основе параметров канала связи
            /// </summary>
            public void Init(SortedList<string, string> commCnlParams, bool requireParams = true)
            {
                TcpPort = commCnlParams.GetIntParam("TcpPort", requireParams, TcpPort);
                InactiveTime = commCnlParams.GetIntParam("InactiveTime", false, InactiveTime);
                Behavior = commCnlParams.GetEnumParam("Behavior", false, Behavior);
                ConnMode = commCnlParams.GetEnumParam("ConnMode", false, ConnMode);
                DevSelMode = commCnlParams.GetEnumParam("DevSelMode", false, DevSelMode);
            }

            /// <summary>
            /// Установить параметры канала связи в соответствии с настройками
            /// </summary>
            public void SetCommCnlParams(SortedList<string, string> commCnlParams)
            {
                commCnlParams["TcpPort"] = TcpPort.ToString();
                commCnlParams["InactiveTime"] = InactiveTime.ToString();
                commCnlParams["Behavior"] = Behavior.ToString();
                commCnlParams["ConnMode"] = ConnMode.ToString();
                commCnlParams["DevSelMode"] = DevSelMode.ToString();
            }
        }

        /// <summary>
        /// Наименование типа канала связи
        /// </summary>
        public const string CommCnlType = "TcpServer";

        // переменные для постоянно используемых значений
        private bool slaveBehavior;
        private bool sharedConnMode;
        private bool devSelByFirstPackage;
        private bool devSelByDeviceLibrary;

        /// <summary>
        /// Настройки канала связи
        /// </summary>
        protected Settings settings;
        /// <summary>
        /// Прослушиватель TCP-соединений
        /// </summary>
        protected TcpListener tcpListener;
        /// <summary>
        /// Список соединений
        /// </summary>
        protected List<TcpConnection> tcpConnList;
        /// <summary>
        /// Общее соединение для всех КП линии связи
        /// </summary>
        protected TcpConnection sharedTcpConn;


        /// <summary>
        /// Конструктор
        /// </summary>
        public CommTcpServerLogic()
            : base()
        {
            slaveBehavior = false;
            sharedConnMode = false;
            devSelByFirstPackage = false;
            devSelByDeviceLibrary = false;

            settings = new Settings();
            tcpListener = null;
            tcpConnList = new List<TcpConnection>();
            sharedTcpConn = null;
        }


        /// <summary>
        /// Получить наименование типа канала связи
        /// </summary>
        public override string TypeName
        {
            get
            {
                return CommCnlType;
            }
        }

        /// <summary>
        /// Получить режим работы
        /// </summary>
        public override OperatingBehaviors Behavior
        {
            get
            {
                return settings.Behavior;
            }
        }


        /// <summary>
        /// Цикл взаимодействия с TCP-клиентами (метод вызывается в отдельном потоке)
        /// </summary>
        protected void Execute()
        {
            // сохранение в локальных переменных постоянно используемых значений
            int inactiveTime = settings.InactiveTime;
            bool devSelByIPAddress = settings.DevSelMode == DeviceSelectionModes.ByIPAddress;
            int threadDelay = slaveBehavior ? SlaveThreadDelay : MasterThreadDelay;

            // выбор метода обработки доступных данных
            Action<TcpConnection> procAvailableData;
            if (sharedConnMode)
                procAvailableData = ProcAvailableDataShared;
            else
                procAvailableData = ProcAvailableDataIndiv;

            // цикл взаимодействия с TCP-клиентами
            while (!terminated)
            {
                TcpConnection tcpConn = null;

                try
                {
                    lock (tcpConnList)
                    {
                        // открытие запрашиваемых соединений
                        while (tcpListener.Pending() && !terminated)
                        {
                            TcpClient tcpClient = tcpListener.AcceptTcpClient();
                            tcpConn = new TcpConnection(tcpClient);
                            tcpConn.WriteToLog = WriteToLog;
                            WriteToLog(string.Format(Localization.UseRussian ? 
                                "{0} Соединение с клиентом {1}" : 
                                "{0} Connect to the client {1}", 
                                CommUtils.GetNowDT(), tcpConn.RemoteAddress));
                            tcpConnList.Add(tcpConn);

                            // установка соединения всем КП
                            if (sharedConnMode)
                                SetConnectionToAllKPs(tcpConn);
                            // привязка соединения к КП по IP-адресу
                            else if (devSelByIPAddress && !BindConnByIP(tcpConn))
                                tcpConn.Broken = true;
                        }

                        // работа с открытыми соединениями
                        DateTime nowDT = DateTime.Now;
                        int connInd = 0;

                        while (connInd < tcpConnList.Count && !terminated)
                        {
                            tcpConn = tcpConnList[connInd];

                            // приём и обработка данных от TCP-клиента
                            if (tcpConn.TcpClient.Available > 0)
                                procAvailableData(tcpConn);

                            // закрытие соединения, если оно неактивно
                            if ((nowDT - tcpConn.ActivityDT).TotalSeconds > inactiveTime || tcpConn.Broken)
                            {
                                WriteToLog(string.Format(Localization.UseRussian ? 
                                    "{0} Отключение клиента {1}" : 
                                    "{0} Disconnect the client {1}", 
                                    nowDT.ToString(CommUtils.CommLineDTFormat), tcpConn.RemoteAddress));
                                tcpConn.Close();
                                tcpConnList.RemoveAt(connInd);
                            }
                            else
                            {
                                connInd++;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (tcpConn == null)
                        WriteToLog(string.Format(Localization.UseRussian ?
                            "Ошибка при взаимодействии с клиентами: {0}" :
                            "Error communicating with clients: {0}", ex.Message));
                    else
                        WriteToLog(string.Format(Localization.UseRussian ?
                            "Ошибка при взаимодействии с клиентом {0}: {1}" :
                            "Error communicating with the client {0}: {1}", tcpConn.RemoteAddress, ex.Message));
                }

                Thread.Sleep(threadDelay);
            }
        }

        /// <summary>
        /// Обработать доступные данные в режиме соединения Individual
        /// </summary>
        protected void ProcAvailableDataIndiv(TcpConnection tcpConn)
        {
            if (tcpConn.RelatedKPExists)
            {
                // обработка входящего запроса в режиме ведомого для первого КП из группы с одинаковым позывным
                if (slaveBehavior)
                {
                    KPLogic targetKP = tcpConn.GetFirstRelatedKP();
                    if (!ExecProcUnreadIncomingReq(targetKP, tcpConn, ref targetKP))
                        tcpConn.ClearNetStream(inBuf);
                }
            }
            else
            {
                // привязка соединения к КП по первому пакету данных
                if (devSelByFirstPackage)
                {
                    if (tcpConn.JustConnected)
                    {
                        string firstPackage = ReceiveFirstPackage(tcpConn);
                        if (!BindConnByFirstPackage(tcpConn, firstPackage))
                            tcpConn.Broken = true;
                    }
                }
                else if (devSelByDeviceLibrary)
                {
                    // привязка соединения к КП, используя произвольную библиотеку КП
                    if (kpListNotEmpty)
                    {
                        KPLogic targetKP = null;
                        if (!ExecProcUnreadIncomingReq(firstKP, tcpConn, ref targetKP))
                            tcpConn.ClearNetStream(inBuf);
                        BindConnByDeviceLibrary(tcpConn, targetKP);
                    }
                }
            }

            tcpConn.JustConnected = false;
        }

        /// <summary>
        /// Обработать доступные данные в режиме соединения Shared
        /// </summary>
        protected void ProcAvailableDataShared(TcpConnection tcpConn)
        {
            // обработка входящего запроса в режиме ведомого для произвольного КП
            if (tcpConn == sharedTcpConn && slaveBehavior && firstKP != null)
            {
                KPLogic targetKP = null;
                if (!ExecProcUnreadIncomingReq(firstKP, tcpConn, ref targetKP))
                    tcpConn.ClearNetStream(inBuf);
            }
        }

        /// <summary>
        /// Установить соединение для КП
        /// </summary>
        protected void SetConnection(KPLogic kpLogic, TcpConnection tcpConn)
        {
            TcpConnection existingTcpConn = kpLogic.Connection as TcpConnection;
            if (existingTcpConn != null)
            {
                existingTcpConn.Broken = true;
                existingTcpConn.ClearRelatedKPs();
            }

            kpLogic.Connection = tcpConn;
            tcpConn.AddRelatedKP(kpLogic);
        }

        /// <summary>
        /// Установить соединение всем КП на линии связи
        /// </summary>
        protected void SetConnectionToAllKPs(TcpConnection tcpConn)
        {
            if (sharedTcpConn != null)
                sharedTcpConn.Broken = true;

            sharedTcpConn = tcpConn;
            foreach (KPLogic kpLogic in kpList)
                kpLogic.Connection = sharedTcpConn;
        }

        /// <summary>
        /// Привязать соединение к КП по IP-адресу 
        /// </summary>
        protected bool BindConnByIP(TcpConnection tcpConn)
        {
            List<KPLogic> kpByCallNumList; // список КП с общим позывным
            string nowDTStr = CommUtils.GetNowDT();

            if (kpCallNumDict.TryGetValue(tcpConn.RemoteAddress, out kpByCallNumList))
            {
                foreach (KPLogic kpLogic in kpByCallNumList)
                {
                    WriteToLog(string.Format(Localization.UseRussian ?
                        "{0} Клиент {1} привязан к {2} по IP-адресу" :
                        "{0} The client {1} is bound to the {2} by IP address",
                        nowDTStr, tcpConn.RemoteAddress, kpLogic.Caption));
                    SetConnection(kpLogic, tcpConn);
                }
                return true;
            }
            else
            {
                WriteToLog(string.Format(Localization.UseRussian ?
                    "{0} Не удалось привязать клиента {1} к КП по IP-адресу" :
                    "{0} Unable to bind the client {1} to a device by IP address",
                    nowDTStr, tcpConn.RemoteAddress));
                return false;
            }
        }

        /// <summary>
        /// Привязать соединение к КП по позывному
        /// </summary>
        protected bool BindConnByFirstPackage(TcpConnection tcpConn, string firstPackage)
        {
            List<KPLogic> kpByCallNumList; // список КП с одинаковым позывным
            string nowDTStr = CommUtils.GetNowDT();

            if (kpCallNumDict.TryGetValue(firstPackage, out kpByCallNumList))
            {
                foreach (KPLogic kpLogic in kpByCallNumList)
                {
                    WriteToLog(string.Format(Localization.UseRussian ?
                        "{0} Клиент {1} привязан к {2} по первому пакету данных" :
                        "{0} The client {1} is bound to the {2} by first data package",
                        nowDTStr, tcpConn.RemoteAddress, kpLogic.Caption));
                    SetConnection(kpLogic, tcpConn);
                }
                return true;
            }
            else
            {
                WriteToLog(string.Format(Localization.UseRussian ?
                    "{0} Не удалось привязать клиента {1} к КП по первому пакету данных" :
                    "{0} Unable to bind the client {1} to a device by first data package",
                    nowDTStr, tcpConn.RemoteAddress));
                return false;
            }
        }

        /// <summary>
        /// Привязать соединение к КП, используя библиотеку КП
        /// </summary>
        protected void BindConnByDeviceLibrary(TcpConnection tcpConn, KPLogic kpLogic)
        {
            if (kpLogic != null)
            {
                WriteToLog(string.Format(Localization.UseRussian ?
                    "{0} Клиент {1} привязан к {2}, используя библиотеку КП" :
                    "{0} The client {1} is bound to the {2} using a device library", 
                    CommUtils.GetNowDT(), tcpConn.RemoteAddress, kpLogic.Caption));
                SetConnection(kpLogic, tcpConn);
            }
            else
            {
                WriteToLog(string.Format(Localization.UseRussian ?
                    "{0} Не удалось привязать клиента {1} к КП, используя библиотеку КП" :
                    "{0} Unable to bind the client {1} to a device using a device library",
                    CommUtils.GetNowDT(), tcpConn.RemoteAddress));
            }
        }

        /// <summary>
        /// Принять первый пакет данных, содержащий позывной
        /// </summary>
        protected string ReceiveFirstPackage(TcpConnection tcpConn)
        {
            WriteToLog(string.Format(Localization.UseRussian ? 
                "{0} Приём первого пакета данных от клиента {1}" : 
                "{0} Receive the first data package from the client {1}",
                CommUtils.GetNowDT(), tcpConn.RemoteAddress));

            string logText;
            int readCnt = tcpConn.ReadAvailable(inBuf, 0, CommUtils.ProtocolLogFormats.String, out logText);
            WriteToLog(logText);

            return readCnt > 0 ? Encoding.Default.GetString(inBuf, 0, readCnt) : "";
        }


        /// <summary>
        /// Инициализировать канал связи
        /// </summary>
        public override void Init(SortedList<string, string> commCnlParams, List<KPLogic> kpList)
        {
            // вызов метода базового класса
            base.Init(commCnlParams, kpList);

            // инициализация настроек канала связи
            settings.Init(commCnlParams);

            // сохранение постоянно используемых значений
            slaveBehavior = settings.Behavior == OperatingBehaviors.Slave;
            sharedConnMode = settings.ConnMode == ConnectionModes.Shared;
            devSelByFirstPackage = settings.DevSelMode == DeviceSelectionModes.ByFirstPackage;
            devSelByDeviceLibrary = settings.DevSelMode == DeviceSelectionModes.ByDeviceLibrary;

            // создание прослушивателя соединений
            tcpListener = new TcpListener(IPAddress.Any, settings.TcpPort);

            // проверка поддержки режима работы канала связи подключенными КП
            CheckBehaviorSupport();
        }

        /// <summary>
        /// Запустить работу канала связи
        /// </summary>
        public override void Start()
        {
            // проверка и запуск прослушивателя соединений
            if (tcpListener == null)
                throw new InvalidOperationException(string.Format(Localization.UseRussian ?
                    "{0} Прослушиватель соединений не инициализирован." : 
                    "{0} Connection listener is not initialized.", CommUtils.GetNowDT()));

            tcpListener.Start();
            WriteToLog(string.Format(Localization.UseRussian ? 
                "{0} Прослушиватель соединений на порту {1} запущен" :
                "{0} Connection listener on port {1} is started", CommUtils.GetNowDT(), settings.TcpPort));

            // запуск потока взаимодействия с клиентами
            StartThread(new ThreadStart(Execute));
        }

        /// <summary>
        /// Остановить работу канала связи
        /// </summary>
        public override void Stop()
        {
            try
            {
                // остановка потока взаимодействия с клиентами
                StopThread();
            }
            finally
            {
                try
                {
                    // остановка прослушивателя соединений
                    if (tcpListener != null)
                    {
                        tcpListener.Stop();
                        tcpListener = null;
                        WriteToLog("");
                        WriteToLog(string.Format(Localization.UseRussian ? 
                            "{0} Прослушиватель соединений остановлен" :
                            "{0} Connection listener is stopped", CommUtils.GetNowDT()));
                    }
                }
                finally
                {
                    // отключение всех клиентов
                    lock (tcpConnList)
                    {
                        foreach (TcpConnection tcpConn in tcpConnList)
                            tcpConn.Close();
                        tcpConnList.Clear();
                        sharedTcpConn = null;
                    }

                    // вызов метода базового класса
                    base.Stop();
                }
            }
        }

        /// <summary>
        /// Выполнить действия после сеанса опроса КП или отправки команды
        /// </summary>
        public override void AfterSession(KPLogic kpLogic)
        {
            // очистка потока данных, если сеанс опроса КП завершён с ошибкой
            if (kpLogic.WorkState == KPLogic.WorkStates.Error && Behavior == OperatingBehaviors.Master)
            {
                TcpConnection tcpConn = kpLogic.Connection as TcpConnection;
                if (tcpConn != null && tcpConn.Connected)
                    tcpConn.ClearNetStream(inBuf);
            }
        }

        /// <summary>
        /// Получить информацию о работе канала связи
        /// </summary>
        public override string GetInfo()
        {
            lock (tcpConnList)
            {
                StringBuilder sbInfo = new StringBuilder(base.GetInfo());
                sbInfo.Append(Localization.UseRussian ? "Подключенные клиенты" : "Connected clients");

                int cnt = tcpConnList.Count;
                if (cnt > 0)
                {
                    sbInfo.Append(" (").Append(cnt).AppendLine("):");
                    for (int i = 0; i < cnt; i++)
                        sbInfo.Append((i + 1).ToString()).Append(". ")
                            .AppendLine(tcpConnList[i].ToString());
                }
                else
                {
                    sbInfo.AppendLine(Localization.UseRussian ? ": Нет" : ": No");
                }

                return sbInfo.ToString();
            }
        }
    }
}
