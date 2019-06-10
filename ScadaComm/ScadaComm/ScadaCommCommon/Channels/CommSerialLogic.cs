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
 * Module   : ScadaCommCommon
 * Summary  : Serial port communication channel logic
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2019
 */

using Scada.Comm.Devices;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;
using System.Threading;

namespace Scada.Comm.Channels
{
    /// <summary>
    /// Serial port communication channel logic.
    /// <para>Логика работы канала связи через последовательный порт.</para>
    /// </summary>
    public class CommSerialLogic : CommChannelLogic
    {
        /// <summary>
        /// Настройки канала связи.
        /// </summary>
        public class Settings
        {
            /// <summary>
            /// Конструктор.
            /// </summary>
            public Settings()
            {
                // установка значений по умолчанию
                PortName = "COM1";
                BaudRate = 9600;
                Parity = Parity.None;
                DataBits = 8;
                StopBits = StopBits.One;
                DtrEnable = false;
                RtsEnable = false;
                Behavior = CommChannelLogic.OperatingBehaviors.Master;
            }

            /// <summary>
            /// Получить или установить имя последовательного порта.
            /// </summary>
            public string PortName { get; set; }
            /// <summary>
            /// Получить или установить скорость обмена.
            /// </summary>
            public int BaudRate { get; set; }
            /// <summary>
            /// Получить или установить контроль чётности.
            /// </summary>
            public Parity Parity { get; set; }
            /// <summary>
            /// Получить или установить биты данных.
            /// </summary>
            public int DataBits { get; set; }
            /// <summary>
            /// Получить или установить стоповые биты.
            /// </summary>
            public StopBits StopBits { get; set; }
            /// <summary>
            /// Получить или установить использование сигнала Data Terminal Ready (DTR) .
            /// </summary>
            public bool DtrEnable { get; set; }
            /// <summary>
            /// Получить или установить использование сигнала Request to Send (RTS).
            /// </summary>
            public bool RtsEnable { get; set; }
            /// <summary>
            /// Получить или установить режим работы канала связи.
            /// </summary>
            public OperatingBehaviors Behavior { get; set; }

            /// <summary>
            /// Инициализировать настройки на основе параметров канала связи.
            /// </summary>
            public void Init(SortedList<string, string> commCnlParams, bool requireParams = true)
            {
                PortName = commCnlParams.GetStringParam("PortName", requireParams, PortName);
                BaudRate = commCnlParams.GetIntParam("BaudRate", requireParams, BaudRate);
                Parity = commCnlParams.GetEnumParam<Parity>("Parity", false, Parity);
                DataBits = commCnlParams.GetIntParam("DataBits", false, DataBits);
                StopBits = commCnlParams.GetEnumParam<StopBits>("StopBits", false, StopBits);
                DtrEnable = commCnlParams.GetBoolParam("DtrEnable", false, DtrEnable);
                RtsEnable = commCnlParams.GetBoolParam("RtsEnable", false, RtsEnable);
                Behavior = commCnlParams.GetEnumParam<OperatingBehaviors>("Behavior", false, Behavior);
            }

            /// <summary>
            /// Установить параметры канала связи в соответствии с настройками.
            /// </summary>
            public void SetCommCnlParams(SortedList<string, string> commCnlParams)
            {
                commCnlParams["PortName"] = PortName;
                commCnlParams["BaudRate"] = BaudRate.ToString();
                commCnlParams["Parity"] = Parity.ToString();
                commCnlParams["DataBits"] = DataBits.ToString();
                commCnlParams["StopBits"] = StopBits.ToString();
                commCnlParams["DtrEnable"] = DtrEnable.ToString();
                commCnlParams["RtsEnable"] = RtsEnable.ToString();
                commCnlParams["Behavior"] = Behavior.ToString();
            }
        }

        /// <summary>
        /// The length of the input buffer in the Slave mode.
        /// </summary>
        protected const int SlaveInBufLen = 1024;
        /// <summary>
        /// The maximum time allowed to elapse before the arrival of the next byte, ms.
        /// </summary>
        protected const int ReadIntervalTimeout = 100;
        /// <summary>
        /// Наименование типа канала связи.
        /// </summary>
        public const string CommCnlType = "Serial";

        /// <summary>
        /// Настройки канала связи.
        /// </summary>
        protected Settings settings;
        /// <summary>
        /// Соединение через последовательный порт.
        /// </summary>
        protected SerialConnection serialConn;


        /// <summary>
        /// Конструктор.
        /// </summary>
        public CommSerialLogic()
            : base()
        {
            settings = new Settings();
            serialConn = null;
        }


        /// <summary>
        /// Получить наименование типа канала связи.
        /// </summary>
        public override string TypeName
        {
            get
            {
                return CommCnlType;
            }
        }

        /// <summary>
        /// Получить режим работы.
        /// </summary>
        public override OperatingBehaviors Behavior
        {
            get
            {
                return settings.Behavior;
            }
        }


        /// <summary>
        /// Открытие последовательного порта.
        /// </summary>
        protected void OpenSerialPort()
        {
            WriteToLog("");
            WriteToLog(string.Format(Localization.UseRussian ?
                "{0} Открытие последовательного порта {1}" :
                "{0} Open serial port {1}", CommUtils.GetNowDT(), serialConn.SerialPort.PortName));
            serialConn.Open();
        }

        /// <summary>
        /// Закрытие последовательного порта.
        /// </summary>
        protected void CloseSerialPort()
        {
            WriteToLog("");
            WriteToLog(string.Format(Localization.UseRussian ?
                "{0} Закрытие последовательного порта {1}" :
                "{0} Close serial port {1}", CommUtils.GetNowDT(), serialConn.SerialPort.PortName));
            serialConn.Close();
        }

        /// <summary>
        /// Listens to the serial port for incoming data.
        /// </summary>
        /// <remarks>This methods works in a separate thread. Is used in Mono.</remarks>
        protected void ListenSerialPort()
        {
            byte[] buffer = new byte[SlaveInBufLen];
            int readCnt = 0;
            int prevReadCnt = 0;

            while (!terminated)
            {
                try
                {
                    serialConn.SerialPort.ReadTimeout = 0;

                    try { readCnt += serialConn.SerialPort.Read(buffer, readCnt, SlaveInBufLen - readCnt); }
                    catch (TimeoutException) { }

                    Thread.Sleep(ReadIntervalTimeout);

                    if (prevReadCnt == readCnt && readCnt > 0 || readCnt == SlaveInBufLen)
                    {
                        KPLogic targetKP = null;
                        if (!ExecProcIncomingReq(firstKP, buffer, 0, readCnt, ref targetKP))
                            serialConn.DiscardInBuffer();
                        readCnt = 0;
                    }

                    prevReadCnt = readCnt;
                }
                catch (Exception ex)
                {
                    WriteToLog(string.Format(Localization.UseRussian ?
                        "Ошибка при прослушивании последовательного порта: {0}" :
                        "Error listening to the serial port: {0}", ex.Message));
                }
            }
        }

        /// <summary>
        /// Обработать событие приёма данных по последовательному порту.
        /// </summary>
        protected void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            KPLogic targetKP = null;
            if (!ExecProcUnreadIncomingReq(firstKP, serialConn, ref targetKP))
                serialConn.DiscardInBuffer();
        }


        /// <summary>
        /// Инициализировать канал связи.
        /// </summary>
        public override void Init(SortedList<string, string> commCnlParams, List<KPLogic> kpList)
        {
            // вызов метода базового класса
            base.Init(commCnlParams, kpList);

            // инициализация настроек канала связи
            settings.Init(commCnlParams);

            // создание клиента и соединения
            SerialPort serialPort = new SerialPort(settings.PortName, settings.BaudRate, settings.Parity, 
                settings.DataBits, settings.StopBits) { DtrEnable = settings.DtrEnable, RtsEnable = settings.RtsEnable };
            serialConn = new SerialConnection(serialPort);

            // установка соединения всем КП на линии связи
            foreach (KPLogic kpLogic in kpList)
            {
                kpLogic.Connection = serialConn;
                kpLogic.SerialPort = serialPort;
            }

            // проверка поддержки режима работы канала связи подключенными КП
            CheckBehaviorSupport();
        }

        /// <summary>
        /// Запустить работу канала связи.
        /// </summary>
        public override void Start()
        {
            // попытка открыть последовательный порт
            OpenSerialPort();

            // привязка события приёма данных в режиме ведомого
            if (settings.Behavior == OperatingBehaviors.Slave && kpList.Count > 0)
            {
                if (ScadaUtils.IsRunningOnMono)
                    StartThread(new ThreadStart(ListenSerialPort));
                else
                    serialConn.SerialPort.DataReceived += SerialPort_DataReceived;
            }
        }

        /// <summary>
        /// Остановить работу канала связи.
        /// </summary>
        public override void Stop()
        {
            // отключение приёма данных в режиме ведомого
            StopThread();
            serialConn.SerialPort.DataReceived -= SerialPort_DataReceived;

            // очистка ссылки на соединение для всех КП на линии связи
            foreach (KPLogic kpLogic in kpList)
                kpLogic.Connection = null;

            // закрытие последовательного порта
            CloseSerialPort();
        }

        /// <summary>
        /// Выполнить действия перед сеансом опроса КП или отправкой команды.
        /// </summary>
        public override void BeforeSession(KPLogic kpLogic)
        {
            // открытие последовательного порта, если он закрыт
            if (serialConn != null && !serialConn.Connected)
                OpenSerialPort();
        }

        /// <summary>
        /// Выполнить действия после сеанса опроса КП или отправки команды.
        /// </summary>
        public override void AfterSession(KPLogic kpLogic)
        {
            // закрытие последовательного порта, если произошла ошибка записи в порт
            if (serialConn != null && serialConn.Connected && serialConn.WriteError)
                CloseSerialPort();
        }

        /// <summary>
        /// Получить информацию о работе канала связи.
        /// </summary>
        public override string GetInfo()
        {
            StringBuilder sbInfo = new StringBuilder(base.GetInfo());

            if (Localization.UseRussian)
            {
                sbInfo.Append("Последовательный порт: ");                
                if (serialConn == null)
                    sbInfo.AppendLine("не определён");
                else
                    sbInfo.Append(serialConn.SerialPort.PortName)
                        .AppendLine(serialConn.SerialPort.IsOpen ? " (открыт)" : " (закрыт)");
            }
            else
            {
                sbInfo.Append("Serial port: ");
                if (serialConn == null)
                    sbInfo.AppendLine("undefined");
                else
                    sbInfo.Append(serialConn.SerialPort.PortName)
                        .AppendLine(serialConn.SerialPort.IsOpen ? " (open)" : " (closed)");
            }

            return sbInfo.ToString();
        }
    }
}
