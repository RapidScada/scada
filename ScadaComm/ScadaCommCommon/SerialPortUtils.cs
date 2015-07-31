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

using Scada.Comm.Channels;
using System;
using System.Collections.Generic;
using System.IO.Ports;

namespace Scada.Comm
{
    /// <summary>
    /// Serial port utilities
    /// <para>Вспомогательные методы для работы с последовательным портом</para>
    /// </summary>
    [Obsolete("Use communication channels")]
    public static class SerialPortUtils
    {
        /// <summary>
        /// Вспомогательный класс соединения через последовательный порт, позволяющий изменять порт
        /// </summary>
        private class AuxSerialConnection : SerialConnection
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            public AuxSerialConnection()
            {
                SerialPort = null;
            }
            /// <summary>
            /// Установить последовательный порт соединения
            /// </summary>
            public void SetSerialPort(SerialPort serialPort)
            {
                SerialPort = serialPort;
            }
        }

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
        /// Соединение через последовательный порт
        /// </summary>
        private static AuxSerialConnection serialConn;


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

            // создание объекта соединения
            serialConn = new AuxSerialConnection();
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
                    serialConn.SetSerialPort(serialPort);
                    serialConn.Write(buffer, index, count, logFormat, out logText);
                }
            }
            catch (Exception ex)
            {
                logText = ex.Message;
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
                    serialConn.SetSerialPort(serialPort);
                    serialConn.WriteLine(text, out logText);
                }
            }
            catch (Exception ex)
            {
                logText = ex.Message;
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
            int timeout, bool wait /*игнорируется*/, CommUtils.ProtocolLogFormats logFormat, out string logText)
        {
            try
            {
                if (serialPort == null)
                {
                    logText = ReadDataImpossible;
                    return 0;
                }
                else
                {
                    serialConn.SetSerialPort(serialPort);
                    return serialConn.Read(buffer, index, count, timeout, logFormat, out logText);
                }
            }
            catch (Exception ex)
            {
                logText = ex.Message;
                return 0;
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
        /// <param name="logText">Строка для вывода в журнал</param>
        /// <returns>Количество считанных байт</returns>
        public static int ReadFromSerialPort(SerialPort serialPort, byte[] buffer, int index, int count,
            int timeout, bool wait /*игнорируется*/, out string logText)
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
            byte stopCode, int timeout, bool wait /*игнорируется*/, CommUtils.ProtocolLogFormats logFormat, 
            out string logText)
        {
            try
            {
                if (serialPort == null)
                {
                    logText = ReadDataImpossible;
                    return 0;
                }
                else
                {
                    serialConn.SetSerialPort(serialPort);
                    bool stopReceived;
                    return serialConn.Read(buffer, index, maxCount, timeout, new Connection.BinStopCondition(stopCode),
                        out stopReceived, logFormat, out logText);
                }
            }
            catch (Exception ex)
            {
                logText = ex.Message;
                return 0;
            }
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
            byte stopCode, int timeout, bool wait /*игнорируется*/, out string logText)
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
        public static List<string> ReadLinesFromSerialPort(SerialPort serialPort, int timeout, 
            bool wait /*игнорируется*/, string endLine, out bool endFound, out string logText)
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
        public static List<string> ReadLinesFromSerialPort(SerialPort serialPort, int timeout, 
            bool wait /*игнорируется*/, string[] endLines, out bool endFound, out string logText)
        {
            try
            {
                if (serialPort == null)
                {
                    logText = ReadLinesImpossible;
                    endFound = false;
                    return new List<string>();
                }
                else
                {
                    serialConn.SetSerialPort(serialPort);
                    return serialConn.ReadLines(timeout, new Connection.TextStopCondition(endLines),
                        out endFound, out logText);
                }
            }
            catch (Exception ex)
            {
                logText = ex.Message;
                endFound = false;
                return new List<string>();
            }
        }
    }
}