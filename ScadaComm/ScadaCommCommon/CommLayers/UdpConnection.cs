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
 * Summary  : UDP connection with a device
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2015
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
    /// UDP connection with a device
    /// <para>UDP-соединение с КП</para>
    /// </summary>
    /// <remarks>In fact, UDP does not create a connection object
    /// <para>На самом деле протокол UDP не создаёт объект соединения</para></remarks>
    public class UdpConnection : Connection
    {
        /// <summary>
        /// Конструктор, ограничивающий создание объекта без параметров
        /// </summary>
        protected UdpConnection()
        {
        }
        
        /// <summary>
        /// Конструктор
        /// </summary>
        public UdpConnection(UdpClient udpClient, int localPort, int remotePort)
        {
            if (udpClient == null)
                throw new ArgumentNullException("udpClient");

            UdpClient = udpClient;
            LocalPort = localPort;
            RemotePort = remotePort;
        }


        /// <summary>
        /// Получить UDP-клиента
        /// </summary>
        public UdpClient UdpClient { get; protected set; }

        /// <summary>
        /// Получить или установить локальный порт
        /// </summary>
        public int LocalPort { get; set; }

        /// <summary>
        /// Получить или установить удалённый порт
        /// </summary>
        public int RemotePort { get; set; }


        /// <summary>
        /// Создать объект IPEndPoint, описывающий удалённый хост
        /// </summary>
        public IPEndPoint CreateIPEndPoint()
        {
            return new IPEndPoint(IPAddress.Parse(RemoteAddress), RemotePort);
        }


        /// <summary>
        /// Считать данные
        /// </summary>
        public override int Read(byte[] buffer, int offset, int count, int timeout, 
            CommUtils.ProtocolLogFormats logFormat, out string logText)
        {
            try
            {
                int readCnt = 0;
                DateTime nowDT = DateTime.Now;
                DateTime startDT = nowDT;
                DateTime stopDT = startDT.AddMilliseconds(timeout);
                IPEndPoint endPoint = CreateIPEndPoint();

                while (readCnt < count && startDT <= nowDT && nowDT <= stopDT)
                {
                    // считывание данных
                    byte[] datagram = UdpClient.Receive(ref endPoint);

                    // копирование полученных данных в заданный буфер
                    if (datagram.Length > 0)
                    {
                        int requiredCnt = count - readCnt;
                        int copyCnt = Math.Min(datagram.Length, requiredCnt);
                        Array.Copy(datagram, 0, buffer, readCnt, copyCnt);
                        readCnt += copyCnt;
                    }

                    // накопление данных во внутреннем буфере соединения
                    if (readCnt < count)
                        Thread.Sleep(DataAccumThreadDelay);

                    nowDT = DateTime.Now;
                }

                logText = BuildReadLogText(buffer, offset, count, readCnt, logFormat);

                return readCnt;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(CommPhrases.ReadDataError + ": " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Считать данные с условиями остановки чтения
        /// </summary>
        public override int Read(byte[] buffer, int offset, int maxCount, int timeout, BinStopCondition stopCond, 
            out bool stopReceived, CommUtils.ProtocolLogFormats logFormat, out string logText)
        {
            try
            {
                int readCnt = 0;
                DateTime nowDT = DateTime.Now;
                DateTime startDT = nowDT;
                DateTime stopDT = startDT.AddMilliseconds(timeout);
                IPEndPoint endPoint = CreateIPEndPoint();

                stopReceived = false;
                int curOffset = offset;
                byte stopCode = stopCond.StopCode;

                while (readCnt < maxCount && !stopReceived && startDT <= nowDT && nowDT <= stopDT)
                {
                    // считывание данных
                    byte[] datagram = UdpClient.Receive(ref endPoint);

                    if (datagram.Length > 0)
                    {
                        // поиск кода остановки в считанных данных
                        int datagramLen = datagram.Length;
                        int stopCodeInd = -1;
                        for (int i = 0; i < datagramLen && !stopReceived; i++)
                        {
                            if (datagram[i] == stopCode)
                            {
                                stopCodeInd = i;
                                stopReceived = true;
                            }
                        }

                        // копирование полученных данных в заданный буфер
                        int requiredCnt = stopReceived ? stopCodeInd - readCnt + 1 : maxCount - readCnt;
                        int copyCnt = Math.Min(datagram.Length, requiredCnt);
                        Array.Copy(datagram, 0, buffer, readCnt, copyCnt);
                        readCnt += copyCnt;
                    }

                    // накопление данных во внутреннем буфере соединения
                    if (readCnt < maxCount && !stopReceived)
                        Thread.Sleep(DataAccumThreadDelay);

                    nowDT = DateTime.Now;
                }

                logText = BuildReadLogText(buffer, offset, readCnt, logFormat);

                return readCnt;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(CommPhrases.ReadDataWithStopCondError + ": " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Считать строки
        /// </summary>
        public override List<string> ReadLines(int timeout, TextStopCondition stopCond, 
            out bool stopReceived, out string logText)
        {
            try
            {
                List<string> lines = new List<string>();
                string[] stopEndings = stopCond.StopEndings;
                int endingsLen = stopEndings == null ? 0 : stopEndings.Length;
                stopReceived = false;

                DateTime nowDT = DateTime.Now;
                DateTime startDT = nowDT;
                DateTime stopDT = startDT.AddMilliseconds(timeout);
                IPEndPoint endPoint = CreateIPEndPoint();

                while (!stopReceived && startDT <= nowDT && nowDT <= stopDT)
                {
                    // считывание данных
                    byte[] datagram = UdpClient.Receive(ref endPoint);

                    if (datagram.Length > 0)
                    {
                        // получение строки из считанных данных
                        string line = Encoding.Default.GetString(datagram);
                        int newLineInd = line.IndexOf(NewLine);
                        if (newLineInd >= 0)
                            line = line.Substring(0, newLineInd);

                        lines.Add(line);
                        for (int i = 0; i < endingsLen && !stopReceived; i++)
                            stopReceived = line.EndsWith(stopEndings[i], StringComparison.OrdinalIgnoreCase);
                    }

                    // накопление данных во внутреннем буфере соединения
                    if (!stopReceived)
                        Thread.Sleep(DataAccumThreadDelay);

                    nowDT = DateTime.Now;
                }

                logText = BuildReadLinesLogText(lines);
                return lines;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(CommPhrases.ReadLinesError + ": " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Записать данные
        /// </summary>
        public override void Write(byte[] buffer, int offset, int count, 
            CommUtils.ProtocolLogFormats logFormat, out string logText)
        {
            try
            {
                if (string.IsNullOrEmpty(RemoteAddress))
                    throw new InvalidOperationException("RemoteAddress is undefined.");
                if (RemotePort <= 0)
                    throw new InvalidOperationException("RemotePort is undefined.");

                byte[] datagram;
                if (offset > 0)
                {
                    datagram = new byte[count];
                    Array.Copy(buffer, offset, datagram, 0, count);
                }
                else
                {
                    datagram = buffer;
                }

                int sentCnt = UdpClient.Send(datagram, count, RemoteAddress, RemotePort);
                logText = BuildWriteLogText(datagram, 0, count, logFormat);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(CommPhrases.WriteDataError + ": " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Записать строку
        /// </summary>
        public override void WriteLine(string text, out string logText)
        {
            try
            {
                byte[] buffer = Encoding.Default.GetBytes(text + NewLine);
                Write(buffer, 0, buffer.Length, CommUtils.ProtocolLogFormats.String, out logText);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(CommPhrases.WriteLineError + ": " + ex.Message, ex);
            }
        }


        /// <summary>
        /// Закрыть соединение
        /// </summary>
        public void Close()
        {
            try { UdpClient.Close(); }
            catch { }
        }
    }
}
