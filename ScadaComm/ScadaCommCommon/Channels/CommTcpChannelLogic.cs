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
 * Summary  : The base class for TCP communication channel logic
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2015
 */

using Scada.Comm.Devices;
using System.Collections.Generic;
using System.Net.Sockets;

namespace Scada.Comm.Channels
{
    /// <summary>
    /// The base class for TCP communication channel logic
    /// <para>Родительский класс логики работы канала связи TCP</para>
    /// </summary>
    public abstract class CommTcpChannelLogic : CommChannelLogic
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
        /// Длина буфера принимаемых данных
        /// </summary>
        protected const int InBufLenght = 1000;

        /// <summary>
        /// Буфер принимаемых данных
        /// </summary>
        protected byte[] inBuf;
        /// <summary>
        /// Словарь КП по позывным
        /// </summary>
        protected Dictionary<string, List<KPLogic>> kpCallNumDict;


        /// <summary>
        /// Конструктор
        /// </summary>
        public CommTcpChannelLogic()
            : base()
        {
            inBuf = new byte[InBufLenght];
            kpCallNumDict = new Dictionary<string, List<KPLogic>>();
        }

        
        /// <summary>
        /// Инициализировать канал связи
        /// </summary>
        public override void Init(SortedList<string, string> commCnlParams, List<KPLogic> kpList)
        {
            // вызов метода базового класса
            base.Init(commCnlParams, kpList);

            // добавление КП в словарь по позывным
            foreach (KPLogic kpLogic in kpList)
            {
                string callNum = kpLogic.CallNum;
                if (!string.IsNullOrEmpty(callNum) && !kpCallNumDict.ContainsKey(callNum))
                {
                    List<KPLogic> kpByCallNumList;
                    if (!kpCallNumDict.TryGetValue(callNum, out kpByCallNumList))
                    {
                        kpByCallNumList = new List<KPLogic>();
                        kpCallNumDict.Add(callNum, kpByCallNumList);
                    }

                    kpByCallNumList.Add(kpLogic);
                }
            }
        }

        /// <summary>
        /// Остановить работу канала связи
        /// </summary>
        public override void Stop()
        {
            // очистка словаря КП по позывным
            kpCallNumDict.Clear();
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
    }
}
