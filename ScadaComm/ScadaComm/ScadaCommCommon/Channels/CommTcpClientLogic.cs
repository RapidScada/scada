/*
 * Copyright 2016 Mikhail Shiryaev
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
 * Summary  : TCP client communication channel logic
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2016
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
    /// TCP client communication channel logic
    /// <para>Логика работы канала связи TCP-клиент</para>
    /// </summary>
    public class CommTcpClientLogic : CommTcpChannelLogic
    {
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
                Host = "";
                TcpPort = 502; // порт, используемый Modbus TCP по умолчанию
                ReconnectAfter = TcpConnection.DefaultReconnectAfter;
                StayConnected = true;
                Behavior = OperatingBehaviors.Master;
                ConnMode = ConnectionModes.Individual;
            }

            /// <summary>
            /// Получить или установить удалённый хост
            /// </summary>
            public string Host { get; set; }
            /// <summary>
            /// Получить или установить удалённый TCP-порт по умолчанию
            /// </summary>
            public int TcpPort { get; set; }
            /// <summary>
            /// Получить или установить интервал повторного подключения, с
            /// </summary>
            public int ReconnectAfter { get; set; }
            /// <summary>
            /// Получить или установить признак, что необходимо оставаться на связи
            /// </summary>
            public bool StayConnected { get; set; }
            /// <summary>
            /// Получить или установить режим работы канала связи
            /// </summary>
            public OperatingBehaviors Behavior { get; set; }
            /// <summary>
            /// Получить или установить режим соединения
            /// </summary>
            public ConnectionModes ConnMode { get; set; }

            /// <summary>
            /// Инициализировать настройки на основе параметров канала связи
            /// </summary>
            public void Init(SortedList<string, string> commCnlParams, bool requireParams = true)
            {
                ConnMode = commCnlParams.GetEnumParam("ConnMode", requireParams, ConnMode);
                bool sharedConnMode = ConnMode == ConnectionModes.Shared;

                Host = commCnlParams.GetStringParam("IpAddress", false, Host); // для обратной совместимости
                if (Host == "")
                    Host = commCnlParams.GetStringParam("Host", requireParams && sharedConnMode, Host);

                TcpPort = commCnlParams.GetIntParam("TcpPort", requireParams && sharedConnMode, TcpPort);
                ReconnectAfter = commCnlParams.GetIntParam("ReconnectAfter", false, ReconnectAfter);
                StayConnected = commCnlParams.GetBoolParam("StayConnected", false, StayConnected);
                Behavior = commCnlParams.GetEnumParam("Behavior", false, Behavior);
            }

            /// <summary>
            /// Установить параметры канала связи в соответствии с настройками
            /// </summary>
            public void SetCommCnlParams(SortedList<string, string> commCnlParams)
            {
                commCnlParams["Host"] = ConnMode == ConnectionModes.Shared ? Host : "";
                commCnlParams["TcpPort"] = TcpPort.ToString();
                commCnlParams["ReconnectAfter"] = ReconnectAfter.ToString();
                commCnlParams["StayConnected"] = StayConnected.ToString();
                commCnlParams["Behavior"] = Behavior.ToString();
                commCnlParams["ConnMode"] = ConnMode.ToString();
            }
        }

        /// <summary>
        /// Наименование типа канала связи
        /// </summary>
        public const string CommCnlType = "TcpClient";
        
        /// <summary>
        /// Настройки канала связи
        /// </summary>
        protected Settings settings;
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
        public CommTcpClientLogic()
            : base()
        {
            settings = new Settings();
            tcpConnList = null;
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
        /// Цикл приёма данных по индивидуальным соединениям в режиме ведомого (метод вызывается в отдельном потоке)
        /// </summary>
        protected void ListenIndividualConn()
        {
            try
            {
                while (!terminated)
                {
                    foreach (KPLogic kpLogic in kpList)
                    {
                        if (kpLogic.Connection is TcpConnection tcpConn && tcpConn.TcpClient.Available > 0)
                        {
                            KPLogic targetKP = kpLogic;
                            if (!ExecProcUnreadIncomingReq(kpLogic, tcpConn, ref targetKP))
                                sharedTcpConn.ClearNetStream(inBuf);
                        }
                    }

                    Thread.Sleep(SlaveThreadDelay);
                }
            }
            catch (Exception ex)
            {
                // данное исключение возникать не должно
                if (Localization.UseRussian)
                {
                    WriteToLog("Ошибка при приёме данных по индивидуальным соединениям: " + ex.Message);
                    WriteToLog("Приём данных прекращён");
                }
                else
                {
                    WriteToLog("Error receiving data via individual connections: " + ex.Message);
                    WriteToLog("Data receiving is terminated");
                }
            }
        }

        /// <summary>
        /// Цикл приёма данных по общему соединению в режиме ведомого (метод вызывается в отдельном потоке)
        /// </summary>
        protected void ListenSharedConn()
        {
            try
            {
                while (!terminated)
                {
                    if (sharedTcpConn.TcpClient.Available > 0)
                    {
                        KPLogic targetKP = null;
                        if (!ExecProcUnreadIncomingReq(firstKP, sharedTcpConn, ref targetKP))
                            sharedTcpConn.ClearNetStream(inBuf);
                    }

                    Thread.Sleep(SlaveThreadDelay);
                }
            }
            catch (Exception ex)
            {
                // данное исключение возникать не должно
                if (Localization.UseRussian)
                {
                    WriteToLog("Ошибка при приёме данных по общему соединению: " + ex.Message);
                    WriteToLog("Приём данных прекращён");
                }
                else
                {
                    WriteToLog("Error receiving data via shared connection: " + ex.Message);
                    WriteToLog("Data receiving is terminated");
                }
            }
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

            // создание соединений и установка соединений КП
            if (settings.ConnMode == ConnectionModes.Shared)
            {
                // общее соединение для всех КП
                sharedTcpConn = new TcpConnection(new TcpClient()) { ReconnectAfter = settings.ReconnectAfter };

                foreach (KPLogic kpLogic in kpList)
                {
                    kpLogic.Connection = sharedTcpConn;
                }
            }
            else
            {
                // индивидуальное соединение для каждого КП или группы КП с общим позывным
                tcpConnList = new List<TcpConnection>();
                foreach (List<KPLogic> kpByCallNumList in kpCallNumDict.Values)
                {
                    foreach (KPLogic kpLogic in kpByCallNumList)
                    {
                        TcpConnection tcpConn = new TcpConnection(new TcpClient())
                        {
                            ReconnectAfter = settings.ReconnectAfter
                        };
                        tcpConnList.Add(tcpConn);
                        tcpConn.AddRelatedKP(kpLogic);
                        kpLogic.Connection = tcpConn;
                    }
                }
            }

            // проверка поддержки режима работы канала связи подключенными КП
            CheckBehaviorSupport();
        }

        /// <summary>
        /// Запустить работу канала связи
        /// </summary>
        public override void Start()
        {
            // запуск потока приёма данных в режиме ведомого
            if (settings.Behavior == OperatingBehaviors.Slave && kpListNotEmpty)
            {
                if (settings.ConnMode == ConnectionModes.Shared)
                    StartThread(new ThreadStart(ListenSharedConn));
                else
                    StartThread(new ThreadStart(ListenIndividualConn));
            }
        }

        /// <summary>
        /// Остановить работу канала связи
        /// </summary>
        public override void Stop()
        {
            try
            {
                // остановка потока приёма данных в режиме ведомого
                StopThread();
            }
            finally
            {
                if (settings.ConnMode == ConnectionModes.Shared)
                {
                    // закрытие общего соединения
                    sharedTcpConn.Close();
                    // очистка ссылки на соединение для всех КП на линии связи
                    foreach (KPLogic kpLogic in kpList)
                        kpLogic.Connection = null;
                }
                else
                {
                    // закрытие всех соединений, обнуление ссылок на соединение для КП
                    foreach (TcpConnection tcpConn in tcpConnList)
                        tcpConn.Close();
                    tcpConnList = null;

                    // вызов метода базового класса
                    base.Stop();
                }
            }
        }

        /// <summary>
        /// Выполнить действия перед сеансом опроса КП или отправкой команды
        /// </summary>
        public override void BeforeSession(KPLogic kpLogic)
        {
            // установка соединения при необходимости
            if (kpLogic.Connection is TcpConnection tcpConn && !tcpConn.Connected)
            {
                if (tcpConn.CanOpen(out string reason))
                {
                    try
                    {
                        // определение IP-адреса и TCP-порта
                        string host;
                        int port;

                        if (tcpConn == sharedTcpConn)
                        {
                            host = settings.Host;
                            port = settings.TcpPort;
                        }
                        else
                        {
                            CommUtils.ExtractHostAndPort(kpLogic.CallNum, settings.TcpPort, out host, out port);
                        }

                        // установка соединения
                        WriteToLog("");
                        WriteToLog(string.Format(Localization.UseRussian ?
                            "{0} Установка TCP-соединения с {1}:{2}" :
                            "{0} Establish a TCP connection with {1}:{2}", CommUtils.GetNowDT(), host, port));

                        if (tcpConn.NetStream != null) // соединение уже было открыто, но разорвано
                            tcpConn.Renew();

                        tcpConn.Open(host, port);
                    }
                    catch (Exception ex)
                    {
                        WriteToLog(ex.Message);
                    }
                }
                else
                {
                    WriteToLog("");
                    WriteToLog(string.Format("{0} {1}", CommUtils.GetNowDT(), reason));
                }
            }
        }

        /// <summary>
        /// Выполнить действия после сеанса опроса КП или отправки команды
        /// </summary>
        public override void AfterSession(KPLogic kpLogic)
        {
            // разрыв соединения согласно настройкам, а также если сеанс опроса КП завершён с ошибкой
            if (!settings.StayConnected || kpLogic.WorkState == KPLogic.WorkStates.Error)
            {
                if (kpLogic.Connection is TcpConnection tcpConn && tcpConn.Connected)
                {
                    WriteToLog("");
                    WriteToLog(string.Format(Localization.UseRussian ?
                        "{0} Отключение от {1}" :
                        "{0} Disconnect from {1}", CommUtils.GetNowDT(), tcpConn.RemoteAddress));
                    tcpConn.Disconnect();
                }
            }
        }

        /// <summary>
        /// Получить информацию о работе канала связи
        /// </summary>
        public override string GetInfo()
        {
            StringBuilder sbInfo = new StringBuilder(base.GetInfo());

            if (sharedTcpConn != null)
            {
                if (Localization.UseRussian)
                {
                    sbInfo
                        .Append("Соединение: ")
                        .Append(sharedTcpConn.Connected ? " установлено" : " не установлено");
                }
                else
                {
                    sbInfo
                        .Append("Connection: ")
                        .Append(sharedTcpConn.Connected ? " established" : " not established");
                }
            }

            return sbInfo.ToString();
        }
    }
}
