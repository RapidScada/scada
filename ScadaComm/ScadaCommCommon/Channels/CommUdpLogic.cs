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
 * Summary  : UDP communication channel logic
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

namespace Scada.Comm.Channels
{
    /// <summary>
    /// UDP communication channel logic
    /// <para>Логика работы канала связи UDP</para>
    /// </summary>
    public class CommUdpLogic : CommChannelLogic
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
                LocalUdpPort = 1;
                RemoteUdpPort = 1;
                RemoteIpAddress = "";
                Behavior = CommChannelLogic.OperatingBehaviors.Master;
                DevSelMode = CommUdpLogic.DeviceSelectionModes.ByIPAddress;
            }

            /// <summary>
            /// Получить или установить локальный UDP-порт
            /// </summary>
            public int LocalUdpPort { get; set; }
            /// <summary>
            /// Получить или установить удалённый UDP-порт
            /// </summary>
            public int RemoteUdpPort { get; set; }
            /// <summary>
            /// Получить или установить удалённый IP-адрес по умолчанию
            /// </summary>
            public string RemoteIpAddress { get; set; }
            /// <summary>
            /// Получить или установить режим работы канала связи
            /// </summary>
            public OperatingBehaviors Behavior { get; set; }
            /// <summary>
            /// Получить или установить режим выбора КП
            /// </summary>
            public DeviceSelectionModes DevSelMode { get; set; }

            /// <summary>
            /// Инициализировать настройки на основе параметров канала связи
            /// </summary>
            public void Init(SortedList<string, string> commCnlParams, bool requireParams = true)
            {
                LocalUdpPort = commCnlParams.GetIntParam("LocalUdpPort", requireParams, LocalUdpPort);
                RemoteUdpPort = commCnlParams.GetIntParam("RemoteUdpPort", false, RemoteUdpPort);
                RemoteIpAddress = commCnlParams.GetStringParam("RemoteIpAddress", false, RemoteIpAddress);
                Behavior = commCnlParams.GetEnumParam<OperatingBehaviors>("Behavior", false, Behavior);
                DevSelMode = commCnlParams.GetEnumParam<DeviceSelectionModes>("DevSelMode", false, DevSelMode);
            }

            /// <summary>
            /// Установить параметры канала связи в соответствии с настройками
            /// </summary>
            public void SetCommCnlParams(SortedList<string, string> commCnlParams)
            {
                commCnlParams["LocalUdpPort"] = LocalUdpPort.ToString();
                commCnlParams["RemoteUdpPort"] = RemoteUdpPort.ToString();
                commCnlParams["RemoteIpAddress"] = RemoteIpAddress;
                commCnlParams["Behavior"] = Behavior.ToString();
                commCnlParams["DevSelMode"] = DevSelMode.ToString();
            }
        }

        /// <summary>
        /// Наименование типа канала связи
        /// </summary>
        public const string CommCnlType = "Udp";

        /// <summary>
        /// Настройки канала связи
        /// </summary>
        protected Settings settings;
        /// <summary>
        /// UPD-соединение
        /// </summary>
        protected UdpConnection udpConn;
        /// <summary>
        /// Словарь КП по позывным
        /// </summary>
        protected Dictionary<string, KPLogic> kpCallNumDict;


        /// <summary>
        /// Конструктор
        /// </summary>
        public CommUdpLogic()
            : base()
        {
            settings = new Settings();
            udpConn = null;
            kpCallNumDict = new Dictionary<string, KPLogic>();
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
        /// Запустить приём данных по UDP
        /// </summary>
        protected void StartUdpReceive()
        {
            try
            {
                if (!udpConn.Connected)
                    udpConn.Renew();

                udpConn.UdpClient.BeginReceive(new AsyncCallback(UdpReceiveCallback), null);
            }
            catch (Exception ex)
            {
                WriteToLog(string.Format(Localization.UseRussian ?
                    "Ошибка при запуске приёма данных: {0}" :
                    "Error starting to receive data: {0}", ex.Message));
            }
        }

        /// <summary>
        /// Обработать принятые по UDP данные
        /// </summary>
        protected void UdpReceiveCallback(IAsyncResult ar)
        {
            // приём данных, если соединение установлено
            byte[] buf = null;

            if (udpConn.Connected)
            {
                try
                {
                    IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
                    buf = udpConn.UdpClient.EndReceive(ar, ref remoteEP);
                    udpConn.RemoteAddress = remoteEP.Address.ToString();
                    udpConn.RemotePort = remoteEP.Port;

                    WriteToLog("");
                    WriteToLog(string.Format(Localization.UseRussian ?
                        "{0} Получены данные от {1}:{2}" :
                        "{0} Data received from {1}:{2}",
                        CommUtils.GetNowDT(), udpConn.RemoteAddress, udpConn.RemotePort));

                    if (buf == null)
                    {
                        WriteToLog(Localization.UseRussian ?
                            "Данные пусты" :
                            "Data is empty");
                    }
                }
                catch (Exception ex)
                {
                    WriteToLog(string.Format(Localization.UseRussian ?
                        "Ошибка при приёме данных: {0}" :
                        "Error receiving data: {0}", ex.Message));
                }
            }

            if (buf != null && kpListNotEmpty)
            {
                if (settings.DevSelMode == DeviceSelectionModes.ByIPAddress)
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
                else // DeviceSelectionModes.ByDeviceLibrary
                {
                    // обработка входящего запроса для произвольного КП
                    KPLogic targetKP = null;
                    ExecProcIncomingReq(firstKP, buf, 0, buf.Length, ref targetKP);
                }
            }

            if (!terminated)
                StartUdpReceive();
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

            // добавление КП в словарь по позывным
            foreach (KPLogic kpLogic in kpList)
            {
                string callNum = kpLogic.CallNum;
                if (!string.IsNullOrEmpty(callNum) && !kpCallNumDict.ContainsKey(callNum))
                    kpCallNumDict.Add(callNum, kpLogic);
            }

            // проверка поддержки режима работы канала связи подключенными КП
            CheckBehaviorSupport();
        }

        /// <summary>
        /// Запустить работу канала связи
        /// </summary>
        public override void Start()
        {
            // создание UDP-клиента и соединения
            UdpClient udpClient = new UdpClient(settings.LocalUdpPort);
            WriteToLog(string.Format(Localization.UseRussian ?
                "{0} Локальный UDP-порт {1} открыт" :
                "{0} Local UDP port {1} is open", CommUtils.GetNowDT(), settings.LocalUdpPort));

            terminated = false;
            udpConn = new UdpConnection(udpClient, settings.LocalUdpPort, settings.RemoteUdpPort);

            // установка соединения всем КП на линии связи
            foreach (KPLogic kpLogic in kpList)
                kpLogic.Connection = udpConn;

            // запуск приёма данных в режиме ведомого
            if (settings.Behavior == OperatingBehaviors.Slave)
            {
                WriteToLog(string.Format(Localization.UseRussian ?
                    "{0} Запуск приёма данных по UDP на порту {1}" :
                    "{0} Start receiving data via UDP on port {1}", CommUtils.GetNowDT(), settings.LocalUdpPort));
                StartUdpReceive();
            }
        }

        /// <summary>
        /// Остановить работу канала связи
        /// </summary>
        public override void Stop()
        {
            // очистка ссылки на соединение для всех КП на линии связи
            foreach (KPLogic kpLogic in kpList)
                kpLogic.Connection = null;

            // очистка словаря КП по позывным
            kpCallNumDict.Clear();

            // закрытие соединения
            terminated = true;
            if (udpConn != null)
                udpConn.Close();

            WriteToLog("");
            WriteToLog(string.Format(Localization.UseRussian ?
                "{0} Завершение приёма данных по UDP" :
                "{0} Stop receiving data via UDP", CommUtils.GetNowDT()));
        }

        /// <summary>
        /// Выполнить действия перед сеансом опроса КП или отправкой команды
        /// </summary>
        public override void BeforeSession(KPLogic kpLogic)
        {
            if (udpConn != null && Behavior == OperatingBehaviors.Master)
            {
                udpConn.RemoteAddress = string.IsNullOrEmpty(kpLogic.CallNum) ?
                    settings.RemoteIpAddress : 
                    kpLogic.CallNum;
            }
        }

        /// <summary>
        /// Выполнить действия после сеанса опроса КП или отправки команды
        /// </summary>
        public override void AfterSession(KPLogic kpLogic)
        {
            // очистка буфера неполностью считанной датаграммы, если сеанс опроса КП завершён с ошибкой
            if (kpLogic.WorkState == KPLogic.WorkStates.Error && Behavior == OperatingBehaviors.Master)
            {
                UdpConnection udpConn = kpLogic.Connection as UdpConnection;
                if (udpConn != null && udpConn.Connected)
                    udpConn.ClearDatagramBuffer();
            }
        }
    }
}
