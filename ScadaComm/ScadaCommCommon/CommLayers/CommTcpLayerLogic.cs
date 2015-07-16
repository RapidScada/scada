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
 * Summary  : The base class for TCP communication layer logic
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2015
 */

using System.Net.Sockets;

namespace Scada.Comm.Layers
{
    /// <summary>
    /// The base class for TCP communication layer logic
    /// <para>Родительский класс логики работы слоя связи TCP</para>
    /// </summary>
    public abstract class CommTcpLayerLogic : CommLayerLogic
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
        /// Таймаут отправки данных по TCP, мс
        /// </summary>
        protected const int TcpSendTimeout = 1000;
        /// <summary>
        /// Таймаут приёма данных по TCP, мс
        /// </summary>
        protected const int TcpReceiveTimeout = 5000;
        /// <summary>
        /// Длина буфера принимаемых данных
        /// </summary>
        protected const int InBufLenght = 1000;

        /// <summary>
        /// Буфер принимаемых данных
        /// </summary>
        protected byte[] inBuf;


        /// <summary>
        /// Конструктор
        /// </summary>
        public CommTcpLayerLogic()
            : base()
        {
            inBuf = new byte[InBufLenght];
        }

        
        /// <summary>
        /// Настроить TCP-клиент
        /// </summary>
        protected TcpClient TuneTcpClient(TcpClient tcpClient, 
            int sendTimeout = TcpSendTimeout, int receiveTimeout = TcpReceiveTimeout)
        {
            tcpClient.NoDelay = true;
            tcpClient.SendTimeout = sendTimeout;
            tcpClient.ReceiveTimeout = receiveTimeout;
            return tcpClient;
        }
    }
}
