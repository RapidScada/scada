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
 * Summary  : Connection with a device via serial port
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2020
 */

using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;
using System.Threading;

namespace Scada.Comm.Channels
{
    /// <summary>
    /// Connection with a device via serial port
    /// <para>Соединение с КП через последовательный порт</para>
    /// </summary>
    public class SerialConnection : Connection
    {
        /// <summary>
        /// Интервал повторного открытия порта, с
        /// </summary>
        protected const int ReopenAfter = 5;

        /// <summary>
        /// Дата и время неудачной попытки открытия порта (UTC)
        /// </summary>
        protected DateTime openFailDT;


        /// <summary>
        /// Конструктор, ограничивающий создание объекта без параметров
        /// </summary>
        protected SerialConnection()
        {
        }
        
        /// <summary>
        /// Конструктор
        /// </summary>
        public SerialConnection(SerialPort serialPort)
            : base()
        {
            if (serialPort == null)
                throw new ArgumentNullException("serialPort");

            openFailDT = DateTime.MinValue;

            SerialPort = serialPort;
            SerialPort.WriteTimeout = DefaultWriteTimeout;
            SerialPort.NewLine = NewLine;
            WriteError = false;
        }


        /// <summary>
        /// Получить признак, что соединение установлено
        /// </summary>
        public override bool Connected
        {
            get
            {
                return SerialPort.IsOpen;
            }
        }

        /// <summary>
        /// Получить или установить окончание строки для методов считывания и записи строк
        /// </summary>
        public override string NewLine
        {
            get
            {
                return base.NewLine;
            }
            set
            {
                base.NewLine = value;
                SerialPort.NewLine = value;
            }
        }

        /// <summary>
        /// Получить последовательный порт
        /// </summary>
        public SerialPort SerialPort { get; protected set; }

        /// <summary>
        /// Ошибка записи в порт
        /// </summary>
        /// <remarks>Сигнализирует о нарушении связи с сетевым конвертером</remarks>
        public bool WriteError { get; protected set; }


        /// <summary>
        /// Считать данные
        /// </summary>
        public override int Read(byte[] buffer, int offset, int count, int timeout, 
            CommUtils.ProtocolLogFormats logFormat, out string logText)
        {
            try
            {
                // данный способ чтения данных необходим для избежания исключения 
                // System.ObjectDisposedException при прерывании потока линии связи
                int readCnt = 0;
                DateTime utcNowDT = DateTime.UtcNow;
                DateTime startDT = utcNowDT;
                DateTime stopDT = startDT.AddMilliseconds(timeout);
                SerialPort.ReadTimeout = 0;

                while (readCnt < count && startDT <= utcNowDT && utcNowDT <= stopDT)
                {
                    try { readCnt += SerialPort.Read(buffer, offset + readCnt, count - readCnt); }
                    catch (TimeoutException) { }

                    // накопление входных данных в буфере порта
                    if (readCnt < count)
                        Thread.Sleep(DataAccumThreadDelay);

                    utcNowDT = DateTime.UtcNow;
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
                DateTime utcNowDT = DateTime.UtcNow;
                DateTime startDT = utcNowDT;
                DateTime stopDT = startDT.AddMilliseconds(timeout);

                stopReceived = false;
                int curInd = offset;
                SerialPort.ReadTimeout = 0;

                while (readCnt < maxCount && !stopReceived && startDT <= utcNowDT && utcNowDT <= stopDT)
                {
                    bool readOk;
                    try { readOk = SerialPort.Read(buffer, curInd, 1) > 0; }
                    catch (TimeoutException) { readOk = false; }

                    if (readOk)
                    {
                        stopReceived = stopCond.CheckCondition(buffer, curInd);
                        curInd++;
                        readCnt++;
                    }
                    else
                    {
                        // накопление входных данных в буфере порта
                        Thread.Sleep(DataAccumThreadDelay);
                    }

                    utcNowDT = DateTime.UtcNow;
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
                stopReceived = false;

                DateTime utcNowDT = DateTime.UtcNow;
                DateTime startDT = utcNowDT;
                DateTime stopDT = startDT.AddMilliseconds(timeout);
                SerialPort.ReadTimeout = ScadaUtils.IsRunningOnMono ? timeout : 0;

                while (!stopReceived && startDT <= utcNowDT && utcNowDT <= stopDT)
                {
                    string line;
                    try { line = SerialPort.ReadLine().Trim(); }
                    catch (TimeoutException) { line = ""; }

                    if (line != "")
                    {
                        lines.Add(line);
                        stopReceived = stopCond.CheckCondition(lines, line);
                    }

                    // накопление входных данных в буфере порта
                    if (!stopReceived)
                        Thread.Sleep(DataAccumThreadDelay);

                    utcNowDT = DateTime.UtcNow;
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
                SerialPort.DiscardInBuffer();
                SerialPort.DiscardOutBuffer();

                try
                {
                    SerialPort.Write(buffer, offset, count);
                    logText = BuildWriteLogText(buffer, offset, count, logFormat);
                    WriteError = false;
                }
                catch (TimeoutException ex)
                {
                    logText = CommPhrases.WriteDataError + ": " + ex.Message;
                }
            }
            catch (Exception ex)
            {
                WriteError = true;
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
                SerialPort.DiscardInBuffer();
                SerialPort.DiscardOutBuffer();

                try
                {
                    SerialPort.WriteLine(text);
                    logText = CommPhrases.SendNotation + ": " + text;
                    WriteError = false;
                }
                catch (TimeoutException ex)
                {
                    logText = CommPhrases.WriteDataError + ": " + ex.Message;
                }
            }
            catch (Exception ex)
            {
                WriteError = true;
                throw new InvalidOperationException(CommPhrases.WriteLineError + ": " + ex.Message, ex);
            }
        }


        /// <summary>
        /// Открыть соединение
        /// </summary>
        public void Open()
        {
            WriteError = false;

            if ((DateTime.UtcNow - openFailDT).TotalSeconds >= ReopenAfter)
            {
                try
                {
                    SerialPort.Open();
                    openFailDT = DateTime.MinValue;
                }
                catch (Exception ex)
                {
                    // время получено повторно, т.к. попытка открытия порта может быть продолжительной
                    openFailDT = DateTime.UtcNow;
                    throw new ScadaException((Localization.UseRussian ?
                        "Ошибка при открытии последовательного порта: " :
                        "Error opening serial port: ") + ex.Message, ex);
                }
            }
            else
            {
                throw new InvalidOperationException(string.Format(Localization.UseRussian ?
                    "Попытка открытия последовательного порта может быть не ранее, чем через {0} с после предыдущей." :
                    "Attempt to open serial port can not be earlier than {0} seconds after the previous.",
                    ReopenAfter));
            }
        }

        /// <summary>
        /// Закрыть соединение
        /// </summary>
        public void Close()
        {
            try { SerialPort.Close(); }
            catch { }
        }

        /// <summary>
        /// Очистить буфер входных данных порта
        /// </summary>
        public void DiscardInBuffer()
        {
            try
            {
                SerialPort.DiscardInBuffer();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(CommPhrases.ClearDataStreamError + ": " + ex.Message, ex);
            }
        }
    }
}
