/*
 * Библиотека слоёв связи
 * Логика работы слоя связи TCP-сервер
 * 
 * Разработчик:
 * 2015, Ширяев Михаил
 */

using Scada.Comm.Devices;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Scada.Comm.Layers
{
    /// <summary>
    /// Логика работы слоя связи TCP-сервер
    /// </summary>
    public class CommTcpServerLogic : CommLayerLogic
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
        /// Настройки слоя связи
        /// </summary>
        public class Settings
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            public Settings()
            {
                // установка значений по умолчанию
                TcpPort = 0;
                InactiveTime = 60;
                Behavior = CommLayerLogic.OperatingBehaviors.Master;
                DevSelMode = CommTcpServerLogic.DeviceSelectionModes.ByIPAddress;
            }

            /// <summary>
            /// Используемый TCP-порт
            /// </summary>
            public int TcpPort;
            /// <summary>
            /// Время неактивности TCP-соединения до его отключения, с
            /// </summary>
            public int InactiveTime;
            /// <summary>
            /// Режим работы слоя связи
            /// </summary>
            public OperatingBehaviors Behavior;
            /// <summary>
            /// Режим выбора КП
            /// </summary>
            public DeviceSelectionModes DevSelMode;
        }

        /// <summary>
        /// Таймаут отправки данных по TCP, мс
        /// </summary>
        private const int TcpSendTimeout = 1000;
        /// <summary>
        /// Таймаут приёма данных по TCP, мс
        /// </summary>
        private const int TcpReceiveTimeout = 5000;
        /// <summary>
        /// Длина буфера принимаемых данных
        /// </summary>
        private const int InBufLenght = 1000;

        private Settings settings;                  // настройки слоя связи
        private TcpListener tcpListener;            // прослушиватель TCP-соединений
        private List<TcpConnection> connList;       // список соединений
        private byte[] inBuf;                       // буфер принимаемых данных


        /// <summary>
        /// Конструктор
        /// </summary>
        public CommTcpServerLogic()
            : base()
        {
            settings = new Settings();
            tcpListener = null;
            connList = new List<TcpConnection>();
            inBuf = new byte[InBufLenght];
        }


        /// <summary>
        /// Получить наименование слоя связи
        /// </summary>
        public override string InternalName
        {
            get
            {
                return "CommTcpServer";
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
        private void Execute()
        {
            // сохранение в локальных переменных постоянно используемых значений
            int inactiveTime = settings.InactiveTime;
            bool slaveBehavior = settings.Behavior == OperatingBehaviors.Slave;
            bool devSelByIPAddress = settings.DevSelMode == DeviceSelectionModes.ByIPAddress;
            bool devSelByFirstPackage = settings.DevSelMode == DeviceSelectionModes.ByFirstPackage;
            bool devSelByDeviceLibrary = settings.DevSelMode == DeviceSelectionModes.ByDeviceLibrary;
            int threadDelay = settings.Behavior == OperatingBehaviors.Master ? MasterThreadDelay : SlaveThreadDelay;

            while (!terminated)
            {
                TcpConnection tcpConn = null;

                try
                {
                    lock (connList)
                    {
                        // открытие запрашиваемых соединений
                        while (tcpListener.Pending() && !terminated)
                        {
                            TcpClient tcpClient = tcpListener.AcceptTcpClient();
                            TuneTcpClient(tcpClient);

                            tcpConn = new TcpConnection(tcpClient);
                            tcpConn.WriteToLog = WriteToLog;
                            WriteToLog(string.Format(Localization.UseRussian ? 
                                "{0} Соединение с клиентом {1}" : "{0} Connect to the client {1}",
                                CommUtils.GetNowDT(), tcpConn.RemoteAddress));
                            connList.Add(tcpConn);

                            // привязка соединения к КП по IP-адресу 
                            if (devSelByIPAddress)
                                if (!BindConnByIP(tcpConn))
                                    tcpConn.Broken = true;
                        }

                        // работа с открытыми соединениями
                        DateTime nowDT = DateTime.Now;
                        int connInd = 0;

                        while (connInd < connList.Count && !terminated)
                        {
                            tcpConn = connList[connInd];

                            // приём и обработка данных от TCP-клиента
                            if (tcpConn.TcpClient.Available > 0)
                            {
                                if (tcpConn.RelatedKP == null)
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
                                        if (kpList.Count > 0)
                                        {
                                            KPLogic targetKP = null;
                                            if (!ExecProcUnreadIncomingReq(kpList[0], tcpConn, ref targetKP))
                                                tcpConn.ClearNetStream(inBuf);
                                            BindConnByDeviceLibrary(tcpConn, targetKP);
                                        }
                                    }
                                }
                                else
                                {
                                    // обработка входящего запроса в режиме ведомого для определённого КП
                                    if (slaveBehavior)
                                    {
                                        KPLogic targetKP = tcpConn.RelatedKP;
                                        if (!ExecProcUnreadIncomingReq(targetKP, tcpConn, ref targetKP))
                                            tcpConn.ClearNetStream(inBuf);
                                    }
                                }

                                tcpConn.JustConnected = false;
                            }

                            // закрытие соединения, если оно неактивно
                            if ((nowDT - tcpConn.ActivityDT).TotalSeconds > inactiveTime || tcpConn.Broken)
                            {
                                WriteToLog(string.Format(Localization.UseRussian ? 
                                    "{0} Отключение клиента {1}" : "{0} Disconnect the client {1}",
                                    nowDT.ToString(CommUtils.CommLineDTFormat), tcpConn.RemoteAddress));
                                ReleaseConnection(tcpConn);
                                connList.RemoveAt(connInd);
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
        /// Настроить TCP-клиент
        /// </summary>
        private void TuneTcpClient(TcpClient tcpClient)
        {
            tcpClient.NoDelay = true;
            tcpClient.SendTimeout = TcpSendTimeout;
            tcpClient.ReceiveTimeout = TcpReceiveTimeout;
        }

        /// <summary>
        /// Установить соединение для КП
        /// </summary>
        private void SetConnection(KPLogic kpLogic, TcpConnection tcpConn)
        {
            TcpConnection existingTcpConn = kpLogic.Connection as TcpConnection;
            if (existingTcpConn != null)
            {
                existingTcpConn.Broken = true;
                existingTcpConn.RelatedKP = null;
            }

            kpLogic.Connection = tcpConn;
            tcpConn.RelatedKP = kpLogic;
        }

        /// <summary>
        /// Прекратить использование соединения
        /// </summary>
        private void ReleaseConnection(TcpConnection tcpConn)
        {
            if (tcpConn.RelatedKP != null)
                tcpConn.RelatedKP.Connection = null;
            tcpConn.Close();
        }

        /// <summary>
        /// Привязать соединение к КП по IP-адресу 
        /// </summary>
        private bool BindConnByIP(TcpConnection tcpConn)
        {
            KPLogic kpLogic;
            if (kpCallNumDict.TryGetValue(tcpConn.RemoteAddress, out kpLogic))
            {
                WriteToLog(string.Format(Localization.UseRussian ?
                    "{0} Клиент {1} привязан к {2} по IP-адресу" :
                    "{0} The client {1} is bound to the {2} by IP address", 
                    CommUtils.GetNowDT(), tcpConn.RemoteAddress, kpLogic.Caption));
                SetConnection(kpLogic, tcpConn);
                return true;
            }
            else
            {
                WriteToLog(string.Format(Localization.UseRussian ?
                    "{0} Не удалось привязать клиента {1} к КП по IP-адресу" :
                    "{0} Unable to bind the client {1} to a device by IP address",
                    CommUtils.GetNowDT(), tcpConn.RemoteAddress));
                return false;
            }
        }

        /// <summary>
        /// Привязать соединение к КП по позывному
        /// </summary>
        private bool BindConnByFirstPackage(TcpConnection tcpConn, string firstPackage)
        {
            KPLogic kpLogic;
            if (kpCallNumDict.TryGetValue(firstPackage, out kpLogic))
            {
                WriteToLog(string.Format(Localization.UseRussian ?
                    "{0} Клиент {1} привязан к {2} по первому пакету данных" :
                    "{0} The client {1} is bound to the {2} by first data package", 
                    CommUtils.GetNowDT(), tcpConn.RemoteAddress, kpLogic.Caption));
                SetConnection(kpLogic, tcpConn);
                return true;
            }
            else
            {
                WriteToLog(string.Format(Localization.UseRussian ?
                    "{0} Не удалось привязать клиента {1} к КП по первому пакету данных" :
                    "{0} Unable to bind the client {1} to a device by first data package",
                    CommUtils.GetNowDT(), tcpConn.RemoteAddress));
                return false;
            }
        }

        /// <summary>
        /// Привязать соединение к КП, используя библиотеку КП
        /// </summary>
        private void BindConnByDeviceLibrary(TcpConnection tcpConn, KPLogic kpLogic)
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
        private string ReceiveFirstPackage(TcpConnection tcpConn)
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
        /// Инициализировать слой связи
        /// </summary>
        public override void Init(Dictionary<string, string> layerParams, List<KPLogic> kpList)
        {
            // вызов метода базового класса
            base.Init(layerParams, kpList);

            // получение настроек слоя связи
            settings.TcpPort = GetIntLayerParam(layerParams, "TcpPort", true, settings.TcpPort);
            settings.InactiveTime = GetIntLayerParam(layerParams, "InactiveTime", true, settings.InactiveTime);
            settings.Behavior = GetEnumLayerParam<OperatingBehaviors>(layerParams, "Behavior", 
                false, settings.Behavior);
            settings.DevSelMode = GetEnumLayerParam<DeviceSelectionModes>(layerParams, "DevSelMode", 
                false, settings.DevSelMode);

            // создание прослушивателя соединений
            tcpListener = new TcpListener(IPAddress.Any, settings.TcpPort);

            // проверка библиотек КП в режиме ведомого
            string warnMsg;
            if (settings.Behavior == OperatingBehaviors.Slave && !AreDllsEqual(out warnMsg))
                WriteToLog(warnMsg);
        }

        /// <summary>
        /// Запустить работу слоя связи
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
        /// Остановить работу слоя связи
        /// </summary>
        public override void Stop()
        {
            try
            {
                // остановка потока работы слоя связи
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
                        WriteToLog(Localization.UseRussian ? "Прослушиватель соединений остановлен" :
                            "Connection listener is stopped");
                    }
                }
                finally
                {
                    // отключение всех клиентов
                    lock (connList)
                    {
                        foreach (TcpConnection tcpConn in connList)
                            ReleaseConnection(tcpConn);
                        connList.Clear();
                    }
                }
            }
        }

        /// <summary>
        /// Получить информацию о работе слоя связи
        /// </summary>
        public override string GetInfo()
        {
            lock (connList)
            {
                StringBuilder sbInfo = new StringBuilder();

                if (Localization.UseRussian)
                {
                    string title = "Слой связи";
                    sbInfo.AppendLine(title)
                        .AppendLine(new string('-', title.Length))
                        .AppendLine("Наименование: " + InternalName)
                        .Append("Подключенные клиенты");
                }
                else
                {
                    string title = "Connection Layer";
                    sbInfo.AppendLine(title)
                        .AppendLine(new string('-', title.Length))
                        .AppendLine("Name: " + InternalName)
                        .Append("Connected clients");
                }

                int cnt = connList.Count;
                if (cnt > 0)
                {
                    sbInfo.Append(" (").Append(cnt).AppendLine("):");
                    for (int i = 0; i < cnt; i++)
                        sbInfo.Append((i + 1).ToString()).Append(". ")
                            .AppendLine(connList[i].ToString());
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
