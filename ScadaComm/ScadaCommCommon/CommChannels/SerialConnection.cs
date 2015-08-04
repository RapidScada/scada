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
 * Summary  : Connection with a device via serial port
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2015
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
        /// Конструктор, ограничивающий создание объекта без параметров
        /// </summary>
        protected SerialConnection()
        {
        }
        
        /// <summary>
        /// Конструктор
        /// </summary>
        public SerialConnection(SerialPort serialPort)
        {
            if (serialPort == null)
                throw new ArgumentNullException("serialPort");
            
            SerialPort = serialPort;
            SerialPort.NewLine = NewLine;
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
        /// Получить последовательный порт
        /// </summary>
        public SerialPort SerialPort { get; protected set; }


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
                DateTime nowDT = DateTime.Now;
                DateTime startDT = nowDT;
                DateTime stopDT = startDT.AddMilliseconds(timeout);
                SerialPort.ReadTimeout = 0;

                while (readCnt < count && startDT <= nowDT && nowDT <= stopDT)
                {
                    try { readCnt += SerialPort.Read(buffer, offset + readCnt, count - readCnt); }
                    catch (TimeoutException) { }

                    // накопление входных данных в буфере порта
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
        public override int Read(byte[] buffer, int offset, int maxCount, int timeout, Connection.BinStopCondition stopCond, 
            out bool stopReceived, CommUtils.ProtocolLogFormats logFormat, out string logText)
        {
            try
            {
                int readCnt = 0;
                DateTime nowDT = DateTime.Now;
                DateTime startDT = nowDT;
                DateTime stopDT = startDT.AddMilliseconds(timeout);

                stopReceived = false;
                byte stopCode = stopCond.StopCode;
                int curInd = offset;
                SerialPort.ReadTimeout = 0;

                while (readCnt <= maxCount && !stopReceived && startDT <= nowDT && nowDT <= stopDT)
                {
                    bool readOk;
                    try { readOk = SerialPort.Read(buffer, curInd, 1) > 0; }
                    catch (TimeoutException) { readOk = false; }

                    if (readOk)
                    {
                        stopReceived = buffer[curInd] == stopCode;
                        curInd++;
                        readCnt++;
                    }
                    else
                    {
                        // накопление входных данных в буфере порта
                        Thread.Sleep(DataAccumThreadDelay);
                    }

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
        public override List<string> ReadLines(int timeout, Connection.TextStopCondition stopCond, 
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
                SerialPort.ReadTimeout = 0;

                while (!stopReceived && startDT <= nowDT && nowDT <= stopDT)
                {
                    string line;
                    try { line = SerialPort.ReadLine().Trim(); }
                    catch (TimeoutException) { line = ""; }

                    if (line != "")
                    {
                        lines.Add(line);
                        for (int i = 0; i < endingsLen && !stopReceived; i++)
                            stopReceived = line.EndsWith(stopEndings[i], StringComparison.OrdinalIgnoreCase);
                    }

                    // накопление входных данных в буфере порта
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
                SerialPort.DiscardInBuffer();
                SerialPort.DiscardOutBuffer();
                SerialPort.Write(buffer, offset, count);
                logText = BuildWriteLogText(buffer, offset, count, logFormat);
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
                SerialPort.DiscardInBuffer();
                SerialPort.DiscardOutBuffer();
                SerialPort.WriteLine(text);
                logText = CommPhrases.SendNotation + ": " + text;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(CommPhrases.WriteLineError + ": " + ex.Message, ex);
            }
        }


        /// <summary>
        /// Открыть соединение
        /// </summary>
        public void Open()
        {
            SerialPort.Open();
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
