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
 * Module   : ScadaCommCommon
 * Summary  : TCP client communication layer logic
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2015
 */

using Scada.Comm.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace Scada.Comm.Layers
{
    /// <summary>
    /// TCP client communication layer logic
    /// <para>Логика работы слоя связи TCP-клиент</para>
    /// </summary>
    public class CommTcpClientLogic : CommLayerLogic
    {
        /// <summary>
        /// Режимы соединения
        /// </summary>
        public enum ConnectionModes
        {
            /// <summary>
            /// Индивидуальное соединение для каждого КП
            /// </summary>
            Individual,
            /// <summary>
            /// Общее соединение для всех КП линии связи
            /// </summary>
            Shared
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
                IpAddress = "";
                TcpPort = 0;
                Behavior = CommLayerLogic.OperatingBehaviors.Master;
                ConnMode = ConnectionModes.Individual;
            }

            /// <summary>
            /// Получить или установить удалённый IP-адрес
            /// </summary>
            public string IpAddress { get; set; }
            /// <summary>
            /// Получить или установить удалённый TCP-порт по умолчанию
            /// </summary>
            public int TcpPort { get; set; }
            /// <summary>
            /// Получить или установить режим работы слоя связи
            /// </summary>
            public OperatingBehaviors Behavior { get; set; }
            /// <summary>
            /// Получить или установить режим соединения
            /// </summary>
            public ConnectionModes ConnMode { get; set; }
        }

        
        /// <summary>
        /// Настройки слоя связи
        /// </summary>
        protected Settings settings;
        /// <summary>
        /// Используется общее соединение для всех КП линии связи
        /// </summary>
        protected bool sharedConnMode;


        /// <summary>
        /// Конструктор
        /// </summary>
        public CommTcpClientLogic()
            : base()
        {
            settings = new Settings();
            sharedConnMode = false;
        }


        /// <summary>
        /// Получить наименование слоя связи
        /// </summary>
        public override string InternalName
        {
            get
            {
                return "CommTcpClient";
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
        /// Инициализировать слой связи
        /// </summary>
        public override void Init(Dictionary<string, string> layerParams, List<KPLogic> kpList)
        {
            // вызов метода базового класса
            base.Init(layerParams, kpList);

            // получение настроек слоя связи
            settings.ConnMode = GetEnumLayerParam<ConnectionModes>(layerParams, "ConnMode", true, settings.ConnMode);
            sharedConnMode = settings.ConnMode == ConnectionModes.Shared;
            settings.IpAddress = GetStringLayerParam(layerParams, "IPAddress", sharedConnMode, settings.IpAddress);
            int tcpPort = GetIntLayerParam(layerParams, "TcpPort", sharedConnMode, settings.TcpPort);
            settings.TcpPort = tcpPort;
            settings.Behavior = GetEnumLayerParam<OperatingBehaviors>(layerParams, "Behavior", 
                false, settings.Behavior);

            // создание соединений и установка соединений КП
            if (sharedConnMode)
            {
                // общее соединение для всех КП
                TcpClient tcpClient = new TcpClient(settings.IpAddress, tcpPort);
                TcpConnection tcpConn = new TcpConnection(tcpClient);
                foreach (KPLogic kpLogic in kpList)
                    kpLogic.Connection = tcpConn;
            }
            else
            {
                // индивидуальное соединение для каждого КП
                foreach (KPLogic kpLogic in kpList)
                {
                    string ipAddr;
                    int port;
                    CommUtils.ExtractAddrAndPort(kpLogic.CallNum, tcpPort, out ipAddr, out port);
                    TcpClient tcpClient = new TcpClient(ipAddr, port);
                    TcpConnection tcpConn = new TcpConnection(tcpClient);
                    kpLogic.Connection = tcpConn;
                }
            }

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
            throw new NotImplementedException();
        }

        /// <summary>
        /// Остановить работу слоя связи
        /// </summary>
        public override void Stop()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Получить информацию о работе слоя связи
        /// </summary>
        public override string GetInfo()
        {
            throw new NotImplementedException();
        }
    }
}
