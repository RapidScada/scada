/*
 * Библиотека слоёв связи
 * Логика работы слоя связи UDP
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
    /// Логика работы слоя связи UDP
    /// </summary>
    public class CommUdpLogic : CommLayerLogic
    {
        /// <summary>
        /// Режимы выбора КП для обработки входящих запросов
        /// </summary>
        public enum DeviceSelectionMode
        {
            /// <summary>
            /// По IP-адресу
            /// </summary>
            ByIPAddress,
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
                LocalUdpPort = 0;
                RemoteUdpPort = 0;
                Behavior = CommLayerLogic.OperatingBehavior.Master;
                DevSelMode = CommUdpLogic.DeviceSelectionMode.ByIPAddress;
            }

            /// <summary>
            /// Локальный UDP-порт
            /// </summary>
            public int LocalUdpPort;
            /// <summary>
            /// Удалённый UDP-порт
            /// </summary>
            public int RemoteUdpPort;
            /// <summary>
            /// Режим работы слоя связи
            /// </summary>
            public OperatingBehavior Behavior;
            /// <summary>
            /// Режим выбора КП
            /// </summary>
            public DeviceSelectionMode DevSelMode;
        }


        /// <summary>
        /// Настройки слоя связи
        /// </summary>
        protected Settings settings;
        /// <summary>
        /// UPD-соединение
        /// </summary>
        protected UdpConnection udpConn;


        /// <summary>
        /// Конструктор
        /// </summary>
        public CommUdpLogic()
            : base()
        {
            settings = new Settings();
            udpConn = null;
        }


        /// <summary>
        /// Получить наименование слоя связи
        /// </summary>
        public override string InternalName
        {
            get
            {
                return "CommUdpServer";
            }
        }

        /// <summary>
        /// Получить режим работы
        /// </summary>
        public override OperatingBehavior Behavior
        {
            get
            {
                return settings.Behavior;
            }
        }


        /// <summary>
        /// Запустить приём данных по UDP
        /// </summary>
        protected void StartUdpReceive()
        {
            udpConn.UdpClient.BeginReceive(new AsyncCallback(UdpReceiveCallback), null);
        }

        /// <summary>
        /// Обработать принятые по UDP данные
        /// </summary>
        protected void UdpReceiveCallback(IAsyncResult ar)
        {
            // приём данных
            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
            byte[] buf = udpConn.UdpClient.EndReceive(ar, ref remoteEP);
            udpConn.RemoteAddress = remoteEP.Address.ToString();
            udpConn.RemotePort = remoteEP.Port;
            WriteToLog(string.Format(Localization.UseRussian ? "{0} Получены данные от {1}:{2}" :
                "{0} Data received from {1}:{2}", CommUtils.GetNowDT(), udpConn.RemoteAddress, udpConn.RemotePort));

            if (buf == null)
            {
                WriteToLog(Localization.UseRussian ? "Данные пусты" : "Data is empty");
            }
            else if (kpList.Count > 0)
            {
                if (settings.DevSelMode == DeviceSelectionMode.ByIPAddress)
                {
                    KPLogic kpLogic;
                    if (kpCallNumDict.TryGetValue(udpConn.RemoteAddress, out kpLogic))
                    {
                        // обработка входящего запроса для определённого КП
                        ExecProcIncomingReq(kpLogic, buf, 0, buf.Length, ref kpLogic);
                    }
                    else
                    {
                        WriteToLog(string.Format(Localization.UseRussian ?
                            "{0} Не удалось найти КП по IP-адресу {1}" :
                            "{0} Unable to find device by IP address {1}",
                            CommUtils.GetNowDT(), udpConn.RemoteAddress));
                    }
                }
                else if (settings.DevSelMode == DeviceSelectionMode.ByDeviceLibrary)
                {
                    // обработка входящего запроса для произвольного КП
                    KPLogic targetKP = null;
                    ExecProcIncomingReq(kpList[0], buf, 0, buf.Length, ref targetKP);
                }
            }

            StartUdpReceive();
        }


        /// <summary>
        /// Инициализировать слой связи
        /// </summary>
        public override void Init(Dictionary<string, string> layerParams, List<KPLogic> kpList)
        {
            // вызов метода базового класса
            base.Init(layerParams, kpList);

            // получение настроек слоя связи
            settings.LocalUdpPort = GetIntLayerParam(layerParams, "LocalUdpPort", true, settings.LocalUdpPort);
            settings.RemoteUdpPort = GetIntLayerParam(layerParams, "RemoteUdpPort", false, settings.RemoteUdpPort);
            settings.Behavior = GetEnumLayerParam<OperatingBehavior>(layerParams, "Behavior", 
                false, settings.Behavior);
            settings.DevSelMode = GetEnumLayerParam<DeviceSelectionMode>(layerParams, "DevSelMode", 
                false, settings.DevSelMode);

            // создание клиента и соединения
            UdpClient udpClient = new UdpClient(settings.LocalUdpPort);
            udpConn = new UdpConnection(udpClient, settings.LocalUdpPort, settings.RemoteUdpPort);

            // установка соединения всем КП на линии связи
            foreach (KPLogic kpLogic in kpList)
                kpLogic.Connection = udpConn;

            // проверка библиотек КП в режиме ведомого
            string warnMsg;
            if (settings.Behavior == OperatingBehavior.Slave && !AreDllsEqual(out warnMsg))
                WriteToLog(warnMsg);
        }

        /// <summary>
        /// Запустить работу слоя связи
        /// </summary>
        public override void Start()
        {
            if (settings.Behavior == OperatingBehavior.Slave)
            {
                WriteToLog(string.Format(Localization.UseRussian ?
                    "{0} Запуск приёма данных по UDP на порту {1}" :
                    "{0} Start receiving data via UDP on port {1}", CommUtils.GetNowDT(), settings.LocalUdpPort));
                StartUdpReceive();
            }
        }

        /// <summary>
        /// Остановить работу слоя связи
        /// </summary>
        public override void Stop()
        {
            foreach (KPLogic kpLogic in kpList)
                kpLogic.Connection = null;
        
            udpConn.Close();
            udpConn = null;
        }

        /// <summary>
        /// Выполнить действия перед сеансом опроса КП или отправкой команды
        /// </summary>
        public override void BeforeSession(KPLogic kpLogic)
        {
            if (udpConn != null)
                udpConn.RemoteAddress = kpLogic.CallNum;
        }

        /// <summary>
        /// Получить информацию о работе слоя связи
        /// </summary>
        public override string GetInfo()
        {
            StringBuilder sbInfo = new StringBuilder();

            if (Localization.UseRussian)
            {
                string title = "Слой связи";
                sbInfo.AppendLine(title)
                    .AppendLine(new string('-', title.Length))
                    .AppendLine("Наименование: " + InternalName)
                    .Append("Локальный порт: ")
                    .Append(udpConn == null ? "не определён" : udpConn.LocalPort.ToString());
            }
            else
            {
                string title = "Connection Layer";
                sbInfo.AppendLine(title)
                    .AppendLine(new string('-', title.Length))
                    .AppendLine("Name: " + InternalName)
                    .Append("Local port: ")
                    .Append(udpConn == null ? "undefined" : udpConn.LocalPort.ToString());
            }

            return sbInfo.ToString();
        }
    }
}
