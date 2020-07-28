/*
 * Copyright 2020 Mikhail Shiryaev
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
 * Summary  : TCP connection with a device
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2020
 */

using Scada.Comm.Devices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Scada.Comm.Channels
{
    /// <summary>
    /// TCP connection with a device.
    /// <para>TCP-соединение с КП.</para>
    /// </summary>
    public class TcpConnection : Connection
    {
        /// <summary>
        /// Таймаут чтения одного байта, мс
        /// </summary>
        protected const int OneByteReadTimeout = 10;
        /// <summary>
        /// Максимальный размер считываемых строк по умолчанию
        /// </summary>
        protected const int DefaultMaxLineSize = 1000;
        /// <summary>
        /// Интервал повторного подключения по умолчанию, с
        /// </summary>
        public const int DefaultReconnectAfter = 5;
        /// <summary>
        /// Обозначение активности для строкового представления соединения
        /// </summary>
        protected static readonly string ActivityStr = 
            Localization.UseRussian ? "активность: " : "activity: ";

        /// <summary>
        /// Максимальный размер считываемых строк
        /// </summary>
        protected int maxLineSize;
        /// <summary>
        /// Дата и время неудачной попытки соединения (UTC)
        /// </summary>
        protected DateTime connFailDT;
        /// <summary>
        /// Список КП, относящихся к данному соединению
        /// </summary>
        protected List<KPLogic> relatedKPList;


        /// <summary>
        /// Конструктор, ограничивающий создание объекта без параметров
        /// </summary>
        protected TcpConnection()
        {
        }
        
        /// <summary>
        /// Конструктор
        /// </summary>
        public TcpConnection(TcpClient tcpClient)
            : base()
        {
            if (tcpClient == null)
                throw new ArgumentNullException("tcpClient");

            maxLineSize = DefaultMaxLineSize;
            connFailDT = DateTime.MinValue;
            relatedKPList = null;
            ReconnectAfter = DefaultReconnectAfter;

            InternalInit(tcpClient);
        }


        /// <summary>
        /// Получить признак, что соединение установлено
        /// </summary>
        public override bool Connected
        {
            get
            {
                return TcpClient.Connected;
            }
        }

        /// <summary>
        /// Получить клиента TCP-соединения
        /// </summary>
        public TcpClient TcpClient { get; protected set; }

        /// <summary>
        /// Получить поток данных TCP-соединения
        /// </summary>
        public NetworkStream NetStream { get; protected set; }

        /// <summary>
        /// Получить или установить интервал повторного подключения, с
        /// </summary>
        public int ReconnectAfter { get; set; }

        /// <summary>
        /// Получить или установить максимальный размер считываемых строк
        /// </summary>
        public int MaxLineSize
        {
            get
            {
                return maxLineSize;
            }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Maximum line size must be positive.", "value");
                maxLineSize = value;
            }
        }

        /// <summary>
        /// Получить или установить дату и время последней активности
        /// </summary>
        public DateTime ActivityDT { get; set; }

        /// <summary>
        /// Получить или установить признак, что соединение только что установлено и обмена данными не было
        /// </summary>
        public bool JustConnected { get; set; }

        /// <summary>
        /// Получить или установить признак, что соединение обровано и его необходимо закрыть
        /// </summary>
        public bool Broken { get; set; }

        /// <summary>
        /// Получить признак, что существует хотя бы один КП, относящийся к данному соединению
        /// </summary>
        public bool RelatedKPExists
        {
            get
            {
                if (relatedKPList == null)
                    return false;
                else 
                    lock (relatedKPList)
                        return relatedKPList.Count > 0;
            }
        }


        /// <summary>
        /// Инициализировать соединение
        /// </summary>
        protected void InternalInit(TcpClient tcpClient)
        {
            TcpClient = tcpClient;
            TakeNetStream();
            TakeAddresses();
            ActivityDT = DateTime.Now;
            JustConnected = true;
            Broken = false;
        }

        /// <summary>
        /// Обновить дату и время последней активности
        /// </summary>
        protected void UpdateActivityDT()
        {
            ActivityDT = DateTime.Now;
        }

        /// <summary>
        /// Определить поток данных TCP-соединения
        /// </summary>
        protected void TakeNetStream()
        {
            if (TcpClient.Connected)
            {
                NetStream = TcpClient.GetStream();
                NetStream.WriteTimeout = DefaultWriteTimeout;
                NetStream.ReadTimeout = DefaultReadTimeout;
            }
            else
            {
                NetStream = null;
            }
        }

        /// <summary>
        /// Определить локальный и удалённый адрес соединения
        /// </summary>
        protected void TakeAddresses()
        {
            try { LocalAddress = ((IPEndPoint)TcpClient.Client.LocalEndPoint).Address.ToString(); }
            catch { LocalAddress = ""; }

            try { RemoteAddress = ((IPEndPoint)TcpClient.Client.RemoteEndPoint).Address.ToString(); }
            catch { RemoteAddress = ""; }
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
                DateTime utcNowDT = DateTime.UtcNow;
                DateTime startDT = utcNowDT;
                DateTime stopDT = startDT.AddMilliseconds(timeout);
                NetStream.ReadTimeout = timeout; // таймаут не выдерживается, если считаны все доступные данные

                while (readCnt < count && startDT <= utcNowDT && utcNowDT <= stopDT)
                {
                    // считывание данных
                    try
                    {
                        if (NetStream.DataAvailable) // checking DataAvailable is critical in Mono
                            readCnt += NetStream.Read(buffer, readCnt + offset, count - readCnt);
                    }
                    catch (IOException) { }

                    // накопление данных во внутреннем буфере соединения
                    if (readCnt < count)
                        Thread.Sleep(DataAccumThreadDelay);

                    utcNowDT = DateTime.UtcNow;
                }

                logText = BuildReadLogText(buffer, offset, count, readCnt, logFormat);

                if (readCnt > 0)
                    UpdateActivityDT();

                return readCnt;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(CommPhrases.ReadDataError + ": " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Считать данные с условием остановки чтения
        /// </summary>
        public override int Read(byte[] buffer, int offset, int maxCount, int timeout, BinStopCondition stopCond,
            out bool stopReceived, CommUtils.ProtocolLogFormats logFormat, out string logText)
        {
            try
            {
                int readCnt = 0;
                DateTime utcNowDT = DateTime.UtcNow;
                DateTime startDT = utcNowDT;
                DateTime stopDT = startDT.AddMilliseconds(timeout);
                NetStream.ReadTimeout = OneByteReadTimeout;

                stopReceived = false;
                int curOffset = offset;

                while (readCnt < maxCount && !stopReceived && startDT <= utcNowDT && utcNowDT <= stopDT)
                {
                    // считывание одного байта данных
                    bool readOk;
                    try { readOk = NetStream.DataAvailable && NetStream.Read(buffer, curOffset, 1) > 0; }
                    catch (IOException) { readOk = false; }

                    if (readOk)
                    {
                        stopReceived = stopCond.CheckCondition(buffer, curOffset);
                        curOffset++;
                        readCnt++;
                    }
                    else
                    {
                        // накопление данных во внутреннем буфере соединения
                        Thread.Sleep(DataAccumThreadDelay);
                    }

                    utcNowDT = DateTime.UtcNow;
                }

                logText = BuildReadLogText(buffer, offset, readCnt, logFormat);

                if (readCnt > 0)
                    UpdateActivityDT();

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
                stopReceived = false;

                DateTime utcNowDT = DateTime.UtcNow;
                DateTime startDT = utcNowDT;
                DateTime stopDT = startDT.AddMilliseconds(timeout);
                NetStream.ReadTimeout = OneByteReadTimeout;

                StringBuilder sbLine = new StringBuilder(maxLineSize);
                byte[] buffer = new byte[1];

                while (!stopReceived && startDT <= utcNowDT && utcNowDT <= stopDT)
                {
                    // считывание одного байта данных
                    bool readOk;
                    try { readOk = NetStream.DataAvailable && NetStream.Read(buffer, 0, 1) > 0; }
                    catch (IOException) { readOk = false; }

                    if (readOk)
                    {
                        sbLine.Append(Encoding.Default.GetChars(buffer));
                    }
                    else
                    {
                        // накопление данных во внутреннем буфере соединения
                        Thread.Sleep(DataAccumThreadDelay);
                    }

                    bool newLineFound = StringBuilderEndsWith(sbLine, NewLine);
                    if (newLineFound || sbLine.Length == maxLineSize)
                    {
                        string line = newLineFound ?
                            sbLine.ToString(0, sbLine.Length - NewLine.Length) : 
                            sbLine.ToString();
                        lines.Add(line);
                        sbLine.Clear();
                        stopReceived = stopCond.CheckCondition(lines, line);
                    }

                    utcNowDT = DateTime.UtcNow;
                }

                logText = BuildReadLinesLogText(lines);

                if (lines.Count > 0)
                    UpdateActivityDT();

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
                NetStream.Write(buffer, offset, count);
                logText = BuildWriteLogText(buffer, offset, count, logFormat);
            }
            catch (IOException ex)
            {
                logText = CommPhrases.WriteDataError + ": " + ex.Message;
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
        /// Вернуть строковое представление соединения
        /// </summary>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            if (!string.IsNullOrEmpty(RemoteAddress))
                sb.Append(RemoteAddress).Append("; ");

            if (relatedKPList != null)
            {
                lock (relatedKPList)
                {
                    int kpCnt = relatedKPList.Count;
                    if (kpCnt > 0)
                    {
                        if (kpCnt == 1)
                        {
                            sb.Append(relatedKPList[0].Caption);
                        }
                        else
                        {
                            sb.Append(Localization.UseRussian ? "КП " : "Devices ");
                            for (int i = 0, lastInd = kpCnt - 1; i < kpCnt; i++)
                            {
                                sb.Append(relatedKPList[i].Number);
                                if (i < lastInd)
                                    sb.Append(",");
                            }
                        }
                        sb.Append("; ");
                    }
                }
            }

            sb.Append(ActivityStr).Append(ActivityDT.ToString("T", Localization.Culture));
            return sb.ToString();
        }


        /// <summary>
        /// Проверить возможность установки TCP-соединения
        /// </summary>
        public bool CanOpen(out string reason)
        {
            if ((DateTime.UtcNow - connFailDT).TotalSeconds >= ReconnectAfter)
            {
                reason = "";
                return true;
            }
            else
            {
                reason = string.Format(Localization.UseRussian ?
                    "Попытка установки TCP-соединения может быть не ранее, чем через {0} с после предыдущей." :
                    "Attempt to establish TCP connection can not be earlier than {0} seconds after the previous.",
                    ReconnectAfter);
                return false;
            }
        }

        /// <summary>
        /// Установить TCP-соединение
        /// </summary>
        public void Open(string host, int port)
        {
            if (CanOpen(out string reason))
            {
                try
                {
                    if (IPAddress.TryParse(host, out IPAddress addr))
                        TcpClient.Connect(addr, port);
                    else
                        TcpClient.Connect(host, port);

                    TakeNetStream();
                    TakeAddresses();
                    connFailDT = DateTime.MinValue;
                }
                catch (Exception ex)
                {
                    // время получено повторно, т.к. попытка соединеня может быть продолжительной
                    connFailDT = DateTime.UtcNow;
                    throw new ScadaException((Localization.UseRussian ?
                        "Ошибка при установке TCP-соединения: " :
                        "Error establishing TCP connection: ") + ex.Message, ex);
                }
            }
            else
            {
                throw new InvalidOperationException(reason);
            }
        }

        /// <summary>
        /// Закрыть соединение, обнулить ссылки на соединение для КП
        /// </summary>
        public void Close()
        {
            if (relatedKPList != null)
            {
                lock (relatedKPList)
                    foreach (KPLogic kpLogic in relatedKPList)
                        kpLogic.Connection = null;
            }

            Disconnect();
        }

        /// <summary>
        /// Разорвать соединение
        /// </summary>
        public void Disconnect()
        {
            try { NetStream.Close(); }
            catch { }

            try { TcpClient.Close(); }
            catch { }
        }

        /// <summary>
        /// Восстановить TCP-соединение
        /// </summary>
        public void Renew()
        {
            Disconnect();
            InternalInit(new TcpClient());
        }

        /// <summary>
        /// Считать данные, доступные на текущий момент
        /// </summary>
        public int ReadAvailable(byte[] buffer, int offset, CommUtils.ProtocolLogFormats logFormat, out string logText)
        {
            try
            {
                int count = TcpClient.Available;
                int readCnt = NetStream.DataAvailable ? NetStream.Read(buffer, offset, count) : 0;
                logText = BuildReadLogText(buffer, offset, count, readCnt, logFormat);

                if (readCnt > 0)
                    UpdateActivityDT();

                return readCnt;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(CommPhrases.ReadAvailableError + ": " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Очистить поток данных TCP-соединения
        /// </summary>
        public void ClearNetStream(byte[] buffer)
        {
            try
            {
                if (NetStream.DataAvailable)
                    NetStream.Read(buffer, 0, buffer.Length);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(CommPhrases.ClearDataStreamError + ": " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Добавить КП, относящийся к данному соединению, в список
        /// </summary>
        public void AddRelatedKP(KPLogic kpLogic)
        {
            if (kpLogic == null)
                throw new ArgumentNullException("kpLogic");

            if (relatedKPList == null)
                relatedKPList = new List<KPLogic>();

            lock (relatedKPList)
                relatedKPList.Add(kpLogic);
        }

        /// <summary>
        /// Очистить список КП, относящихся к данному соединению
        /// </summary>
        public void ClearRelatedKPs()
        {
            if (relatedKPList != null)
            {
                lock (relatedKPList)
                    relatedKPList.Clear();
            }
        }

        /// <summary>
        /// Получить первый в списке КП, относящийся к данному соединению
        /// </summary>
        public KPLogic GetFirstRelatedKP()
        {
            if (relatedKPList == null)
                return null;
            else
                lock (relatedKPList)
                    return relatedKPList.Count > 0 ? relatedKPList[0] : null;                        
        }
    }
}
