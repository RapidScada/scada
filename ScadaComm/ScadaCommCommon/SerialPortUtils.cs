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
 * Summary  : Serial port utilities
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2015
 */

using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;

namespace Scada.Comm
{
    /// <summary>
    /// Serial port utilities
    /// <para>Вспомогательные методы для работы с последовательным портом</para>
    /// </summary>
    [Obsolete("Use communication layers")]
    public static class SerialPortUtils
    {
        /// <summary>
        /// Сообщение о невозможности отправки данных
        /// </summary>
        public static readonly string WriteDataImpossible;
        /// <summary>
        /// Сообщение о невозможности отправки строк
        /// </summary>
        public static readonly string WriteLineImpossible;
        /// <summary>
        /// Сообщение о невозможности приёма данных
        /// </summary>
        public static readonly string ReadDataImpossible;
        /// <summary>
        /// Сообщение о невозможности приёма строк
        /// </summary>
        public static readonly string ReadLinesImpossible;


        /// <summary>
        /// Статический конструктор
        /// </summary>
        static SerialPortUtils()
        {
            // инициализция фраз
            if (Localization.UseRussian)
            {
                WriteDataImpossible = "Отправка данных невозможна, т.к. порт не инициализирован";
                WriteLineImpossible = "Отправка строки невозможна, т.к. порт не инициализирован";
                ReadDataImpossible = "Приём данных невозможен, т.к. порт не инициализирован";
                ReadLinesImpossible = "Приём строки невозможен, т.к. порт не инициализирован";
            }
            else
            {
                WriteDataImpossible = "Sending data is impossible because the port is not initialized";
                WriteLineImpossible = "Sending string is impossible because the port is not initialized";
                ReadDataImpossible = "Receiving data is impossible because the port is not initialized";
                ReadLinesImpossible = "Receiving string is impossible because the port is not initialized";
            }
        }


        /// <summary>
        /// Записать данные в последовательный порт
        /// </summary>
        /// <param name="serialPort">Последовательный порт</param>
        /// <param name="buffer">Буфер передаваемых данных</param>
        /// <param name="index">Начальный индекс в буфере</param>
        /// <param name="count">Количество передаваемых байт</param>
        /// <param name="logFormat">Формат вывода данных в журнал</param>
        /// <param name="logText">Строка для вывода в журнал</param>
        public static void WriteToSerialPort(SerialPort serialPort, byte[] buffer, int index, int count,
            CommUtils.ProtocolLogFormats logFormat, out string logText)
        {
            try
            {
                if (serialPort == null)
                {
                    logText = WriteDataImpossible;
                }
                else
                {
                    serialPort.DiscardInBuffer();
                    serialPort.DiscardOutBuffer();
                    serialPort.Write(buffer, index, count);
                    logText = CommPhrases.SendNotation + " (" + count + "): " + 
                        (logFormat == CommUtils.ProtocolLogFormats.Hex ? 
                            CommUtils.BytesToHex(buffer, index, count) : 
                            CommUtils.BytesToString(buffer, index, count));
                }
            }
            catch (Exception ex)
            {
                logText = (Localization.UseRussian ?
                    "Ошибка при отправке данных: " : "Error sending data: ") + ex.Message;
            }
        }

        /// <summary>
        /// Записать данные в последовательный порт
        /// </summary>
        /// <param name="serialPort">Последовательный порт</param>
        /// <param name="buffer">Буфер передаваемых данных</param>
        /// <param name="index">Начальный индекс в буфере</param>
        /// <param name="count">Количество передаваемых байт</param>
        /// <param name="logText">Строка для вывода в журнал</param>
        public static void WriteToSerialPort(SerialPort serialPort, byte[] buffer, int index, int count, out string logText)
        {
            WriteToSerialPort(serialPort, buffer, index, count, CommUtils.ProtocolLogFormats.Hex, out logText);
        }

        /// <summary>
        /// Записать строку в последовательный порт
        /// </summary>
        /// <param name="serialPort">Последовательный порт</param>
        /// <param name="text">Записываемая в порт строка</param>
        /// <param name="logText">Строка для вывода в журнал</param>
        public static void WriteLineToSerialPort(SerialPort serialPort, string text, out string logText)
        {
            try
            {
                if (serialPort == null)
                {
                    logText = WriteLineImpossible;
                }
                else
                {
                    serialPort.DiscardInBuffer();
                    serialPort.DiscardOutBuffer();
                    serialPort.WriteLine(text);
                    logText = CommPhrases.SendNotation + ": " + text;
                }
            }
            catch (Exception ex)
            {
                logText = (Localization.UseRussian ?
                    "Ошибка при отправке строки: " : "Error sending string: ") + ex.Message;
            }
        }


        /// <summary>
        /// Считать данные из последовательного порта
        /// </summary>
        /// <param name="serialPort">Последовательный порт</param>
        /// <param name="buffer">Буфер принимаемых данных</param>
        /// <param name="index">Начальный индекс в буфере</param>
        /// <param name="count">Количество принимаемых байт</param>
        /// <param name="timeout">Таймаут чтения данных, мс</param>
        /// <param name="wait">Ожидать завершения таймаута после окончания чтения</param>
        /// <param name="logFormat">Формат вывода данных в журнал</param>
        /// <param name="logText">Строка для вывода в журнал</param>
        /// <returns>Количество считанных байт</returns>
        public static int ReadFromSerialPort(SerialPort serialPort, byte[] buffer, int index, int count,
            int timeout, bool wait, CommUtils.ProtocolLogFormats logFormat, out string logText)
        {
            int readCnt = 0;

            if (serialPort == null)
            {
                logText = ReadDataImpossible;
            }
            else
            {
                // данный способ чтения данных необходим для избежания исключения 
                // System.ObjectDisposedException при прерывании потока линии связи
                DateTime nowDT = DateTime.Now;
                DateTime startDT = nowDT;
                DateTime stopDT = startDT.AddMilliseconds(timeout);
                serialPort.ReadTimeout = 0;

                while (readCnt < count && startDT <= nowDT && nowDT <= stopDT)
                {
                    try { readCnt += serialPort.Read(buffer, index + readCnt, count - readCnt); }
                    catch { /*The operation has timed out*/ }

                    if (readCnt < count)
                        Thread.Sleep(100); // накопление входных данных в буфере порта

                    nowDT = DateTime.Now;
                }

                logText = CommPhrases.ReceiveNotation + " (" + readCnt + "/" + count + "): " + 
                    (logFormat == CommUtils.ProtocolLogFormats.Hex ?
                        CommUtils.BytesToHex(buffer, index, readCnt) :
                        CommUtils.BytesToString(buffer, index, readCnt));

                if (wait && startDT <= nowDT)
                {
                    int delay = (int)(stopDT - nowDT).TotalMilliseconds;
                    if (delay > 0)
                        Thread.Sleep(delay);
                }
            }

            return readCnt;
        }

        /// <summary>
        /// Считать данные из последовательного порта
        /// </summary>
        /// <param name="serialPort">Последовательный порт</param>
        /// <param name="buffer">Буфер принимаемых данных</param>
        /// <param name="index">Начальный индекс в буфере</param>
        /// <param name="count">Количество принимаемых байт</param>
        /// <param name="timeout">Таймаут чтения данных, мс</param>
        /// <param name="wait">Ожидать завершения таймаута после окончания чтения</param>
        /// <param name="logText">Строка для вывода в журнал</param>
        /// <returns>Количество считанных байт</returns>
        public static int ReadFromSerialPort(SerialPort serialPort, byte[] buffer, int index, int count,
            int timeout, bool wait, out string logText)
        {
            return ReadFromSerialPort(serialPort, buffer, index, count,
                timeout, wait, CommUtils.ProtocolLogFormats.Hex, out logText);
        }

        /// <summary>
        /// Считать данные из последовательного порта
        /// </summary>
        /// <param name="serialPort">Последовательный порт</param>
        /// <param name="buffer">Буфер принимаемых данных</param>
        /// <param name="index">Начальный индекс в буфере</param>
        /// <param name="maxCount">Максимальное количество принимаемых байт</param>
        /// <param name="stopCode">Байт, означающий окончание считывания данных</param>
        /// <param name="timeout">Таймаут чтения данных, мс</param>
        /// <param name="wait">Ожидать завершения таймаута после окончания чтения</param>
        /// <param name="logFormat">Формат вывода данных в журнал</param>
        /// <param name="logText">Строка для вывода в журнал</param>
        /// <returns>Количество считанных байт</returns>
        public static int ReadFromSerialPort(SerialPort serialPort, byte[] buffer, int index, int maxCount,
            byte stopCode, int timeout, bool wait, CommUtils.ProtocolLogFormats logFormat, out string logText)
        {
            int readCnt = 0;

            if (serialPort == null)
            {
                logText = ReadDataImpossible;
            }
            else
            {
                DateTime nowDT = DateTime.Now;
                DateTime startDT = nowDT;
                DateTime stopDT = startDT.AddMilliseconds(timeout);

                bool stop = false;
                int curInd = index;
                serialPort.ReadTimeout = 0;

                while (readCnt <= maxCount && !stop && startDT <= nowDT && nowDT <= stopDT)
                {
                    bool readOk;
                    try { readOk = serialPort.Read(buffer, curInd, 1) > 0; }
                    catch { readOk = false; }

                    if (readOk)
                    {
                        stop = buffer[curInd] == stopCode;
                        curInd++;
                        readCnt++;
                    }
                    else
                    {
                        Thread.Sleep(100); // накопление входных данных в буфере порта
                    }

                    nowDT = DateTime.Now;
                }

                logText = CommPhrases.ReceiveNotation + " (" + readCnt + "): " + 
                    (logFormat == CommUtils.ProtocolLogFormats.Hex ?
                        CommUtils.BytesToHex(buffer, index, readCnt) :
                        CommUtils.BytesToString(buffer, index, readCnt));

                if (wait && startDT <= nowDT)
                {
                    int delay = (int)(stopDT - nowDT).TotalMilliseconds;
                    if (delay > 0)
                        Thread.Sleep(delay);
                }
            }

            return readCnt;
        }

        /// <summary>
        /// Считать данные из последовательного порта
        /// </summary>
        /// <param name="serialPort">Последовательный порт</param>
        /// <param name="buffer">Буфер принимаемых данных</param>
        /// <param name="index">Начальный индекс в буфере</param>
        /// <param name="maxCount">Максимальное количество принимаемых байт</param>
        /// <param name="stopCode">Байт, означающий окончание считывания данных</param>
        /// <param name="timeout">Таймаут чтения данных, мс</param>
        /// <param name="wait">Ожидать завершения таймаута после окончания чтения</param>
        /// <param name="logText">Строка для вывода в журнал</param>
        /// <returns>Количество считанных байт</returns>
        public static int ReadFromSerialPort(SerialPort serialPort, byte[] buffer, int index, int maxCount,
            byte stopCode, int timeout, bool wait, out string logText)
        {
            return ReadFromSerialPort(serialPort, buffer, index, maxCount,
                stopCode, timeout, wait, CommUtils.ProtocolLogFormats.Hex, out logText);
        }

        /// <summary>
        /// Считать строки из последовательного порта
        /// </summary>
        /// <param name="serialPort">Последовательный порт</param>
        /// <param name="timeout">Таймаут чтения данных, мс</param>
        /// <param name="wait">Ожидать завершения таймаута после окончания чтения</param>
        /// <param name="endLine">Строка, при получении которой завершить приём данных</param>
        /// <param name="endFound">Призкак получения завершающей строки</param>
        /// <param name="logText">Строка для вывода в журнал</param>
        /// <returns>Считанные из последовательного порта строки</returns>
        public static List<string> ReadLinesFromSerialPort(SerialPort serialPort, int timeout, bool wait,
            string endLine, out bool endFound, out string logText)
        {
            return ReadLinesFromSerialPort(serialPort, timeout, wait,
                string.IsNullOrEmpty(endLine) ? null : new string[] { endLine }, out endFound, out logText);
        }

        /// <summary>
        /// Считать строки из последовательного порта
        /// </summary>
        /// <param name="serialPort">Последовательный порт</param>
        /// <param name="timeout">Таймаут чтения данных, мс</param>
        /// <param name="wait">Ожидать завершения таймаута после окончания чтения</param>
        /// <param name="endLines">Массив строк, при получении которых завершить приём данных</param>
        /// <param name="endFound">Призкак получения завершающей строки</param>
        /// <param name="logText">Строка для вывода в журнал</param>
        /// <returns>Считанные из последовательного порта строки</returns>
        public static List<string> ReadLinesFromSerialPort(SerialPort serialPort, int timeout, bool wait,
            string[] endLines, out bool endFound, out string logText)
        {
            List<string> inDataList = new List<string>(); // входные данные
            StringBuilder inDataSB = new StringBuilder(); // строковое представление входных данных
            int endLinesLen = endLines == null ? 0 : endLines.Length;
            endFound = false;

            if (serialPort == null)
            {
                logText = ReadLinesImpossible;
            }
            else
            {
                DateTime nowDT = DateTime.Now;
                DateTime startDT = nowDT;
                DateTime stopDT = startDT.AddMilliseconds(timeout);
                serialPort.ReadTimeout = 0;

                while (!endFound && startDT <= nowDT && nowDT <= stopDT)
                {
                    string line;
                    try { line = serialPort.ReadLine().Trim(); }
                    catch { line = ""; /*The operation has timed out*/ }

                    if (line != "")
                    {
                        inDataList.Add(line);
                        inDataSB.AppendLine(line);

                        for (int i = 0; i < endLinesLen && !endFound; i++)
                            endFound = line.EndsWith(endLines[i], StringComparison.OrdinalIgnoreCase);
                    }

                    if (!endFound)
                        Thread.Sleep(100); // накопление входных данных в буфере serialPort

                    nowDT = DateTime.Now;
                }

                logText = CommPhrases.ReceiveNotation + ": " +
                    (inDataList.Count > 0 ?
                        inDataSB.ToString() :
                        (Localization.UseRussian ? "нет данных" : "no data"));

                if (wait && startDT <= nowDT)
                {
                    int delay = (int)(stopDT - nowDT).TotalMilliseconds;
                    if (delay > 0)
                        Thread.Sleep(delay);
                }
            }

            return inDataList;
        }
    }
}
