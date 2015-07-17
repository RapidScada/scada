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
 * Summary  : The base class for connection with physical device
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
using Utils;

namespace Scada.Comm.Layers
{
    /// <summary>
    /// The base class for connection with physical device
    /// <para>Родительский класс соединения с физическим КП</para>
    /// </summary>
    public abstract class Connection
    {
        /// <summary>
        /// Условие остановки считывания данных в бинарном формате
        /// </summary>
        public class BinStopCondition
        {
            /// <summary>
            /// Конструктор, ограничивающий создание объекта без параметров
            /// </summary>
            protected BinStopCondition()
            {
            }
            /// <summary>
            /// Конструктор
            /// </summary>
            public BinStopCondition(byte stopCode)
            {
                StopCode = stopCode;
            }
            /// <summary>
            /// Получить код, приём которого останавливает считывание данных
            /// </summary>
            public byte StopCode { get; protected set; }
        }

        /// <summary>
        /// Условие остановки считывания данных в формате
        /// </summary>
        public class TextStopCondition
        {
            /// <summary>
            /// Конструктор, ограничивающий создание объекта без параметров
            /// </summary>
            protected TextStopCondition()
            {
            }
            /// <summary>
            /// Конструктор
            /// </summary>
            public TextStopCondition(string[] stopEndings)
            {
                StopEndings = stopEndings;
            }
            /// <summary>
            /// Конструктор
            /// </summary>
            public TextStopCondition(string stopEnding)
            {
                StopEndings = new string[] { stopEnding };
            }
            /// <summary>
            /// Получить окончания строк, приём которых останавливает считывание данных
            /// </summary>
            public string[] StopEndings { get; protected set; }
        }

        /// <summary>
        /// Задержка потока для накопления данных во внутреннем буфере соединения, мс
        /// </summary>
        protected const int DataAccumThreadDelay = 10;

        /// <summary>
        /// Метод записи в журнал линии связи
        /// </summary>
        protected Log.WriteLineDelegate writeToLog;


        /// <summary>
        /// Конструктор
        /// </summary>
        public Connection()
        {
            writeToLog = text => { }; // заглушка
            DefaultLogFormat = CommUtils.ProtocolLogFormats.Hex;
            NewLine = "\n";
            LocalAddress = "";
            RemoteAddress = "";
        }


        /// <summary>
        /// Получить признак, что соединение установлено
        /// </summary>
        public virtual bool Connected
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Формат вывода протокола обмена данными в журнал по умлочанию
        /// </summary>
        public CommUtils.ProtocolLogFormats DefaultLogFormat { get; set; }

        /// <summary>
        /// Получить или установить метод записи в журнал линии связи
        /// </summary>
        public Log.WriteLineDelegate WriteToLog
        {
            get
            {
                return writeToLog;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                writeToLog = value;
            }
        }

        /// <summary>
        /// Получить или установить локальный адрес соединения
        /// </summary>
        public string LocalAddress { get; set; }

        /// <summary>
        /// Получить или установить удалённый адрес соединения
        /// </summary>
        public string RemoteAddress { get; set; }

        /// <summary>
        /// Получить или установить окончание строки для методов считывания и записи строк
        /// </summary>
        public string NewLine { get; set; }


        /// <summary>
        /// Сформировать текст считывания данных для вывода в журнал
        /// </summary>
        public static string BuildReadLogText(byte[] buffer, int offset, int count, int readCnt,
            CommUtils.ProtocolLogFormats logFormat)
        {
            return CommPhrases.ReceiveNotation + " (" + readCnt + "/" + count + "): " +
                (logFormat == CommUtils.ProtocolLogFormats.Hex ?
                    CommUtils.BytesToHex(buffer, offset, readCnt) :
                    CommUtils.BytesToString(buffer, offset, readCnt));
        }

        /// <summary>
        /// Сформировать текст считывания данных для вывода в журнал
        /// </summary>
        public static string BuildReadLogText(byte[] buffer, int offset, int readCnt,
            CommUtils.ProtocolLogFormats logFormat)
        {
            return CommPhrases.ReceiveNotation + " (" + readCnt + "): " +
                (logFormat == CommUtils.ProtocolLogFormats.Hex ?
                    CommUtils.BytesToHex(buffer, offset, readCnt) :
                    CommUtils.BytesToString(buffer, offset, readCnt));
        }

        /// <summary>
        /// Сформировать текст считывания данных для вывода в журнал
        /// </summary>
        public static string BuildReadLinesLogText(List<string> lines)
        {
            return CommPhrases.ReceiveNotation + ": " +
                (lines.Count > 0 ? string.Join(Environment.NewLine, lines) :
                    (Localization.UseRussian ? "нет данных" : "no data"));
        }

        /// <summary>
        /// Сформировать текст записи данных для вывода в журнал
        /// </summary>
        public static string BuildWriteLogText(byte[] buffer, int offset, int count, 
            CommUtils.ProtocolLogFormats logFormat)
        {
            return CommPhrases.SendNotation + " (" + count + "): " +
                (logFormat == CommUtils.ProtocolLogFormats.Hex ?
                    CommUtils.BytesToHex(buffer, offset, count) :
                    CommUtils.BytesToString(buffer, offset, count));
        }


        /// <summary>
        /// Считать данные
        /// </summary>
        /// <returns>Количество считанных байт</returns>
        public abstract int Read(byte[] buffer, int offset, int count, int timeout,
            CommUtils.ProtocolLogFormats logFormat, out string logText);

        /// <summary>
        /// Считать данные и вывести информацию в журнал
        /// </summary>
        public virtual int Read(byte[] buffer, int offset, int count, int timeout)
        {
            string logText;
            int readCnt = Read(buffer, offset, count, timeout, DefaultLogFormat, out logText);
            WriteToLog(logText);
            return readCnt;
        }

        /// <summary>
        /// Считать данные с условиями остановки чтения
        /// </summary>
        public abstract int Read(byte[] buffer, int offset, int maxCount, int timeout, BinStopCondition stopCond,
            out bool stopReceived, CommUtils.ProtocolLogFormats logFormat, out string logText);

        /// <summary>
        /// Считать данные с условиями остановки чтения и вывести информацию в журнал
        /// </summary>
        public virtual int Read(byte[] buffer, int offset, int maxCount, int timeout, BinStopCondition stopCond)
        {
            bool stopReceived;
            string logText;
            int readCnt = Read(buffer, offset, maxCount, timeout, stopCond,
                out stopReceived, DefaultLogFormat, out logText);
            WriteToLog(logText);
            return readCnt;
        }

        /// <summary>
        /// Считать строки
        /// </summary>
        public abstract List<string> ReadLines(int timeout, TextStopCondition stopCond,
            out bool stopReceived, out string logText);

        /// <summary>
        /// Считать строки и вывести информацию в журнал
        /// </summary>
        public virtual List<string> ReadLines(int timeout, TextStopCondition stopCond)
        {
            bool stopReceived;
            string logText;
            List<string> lines = ReadLines(timeout, stopCond, out stopReceived, out logText);
            WriteToLog(logText);
            return lines;
        }


        /// <summary>
        /// Записать данные
        /// </summary>
        public abstract void Write(byte[] buffer, int offset, int count,
            CommUtils.ProtocolLogFormats logFormat, out string logText);

        /// <summary>
        /// Записать данные и вывести информацию в журнал
        /// </summary>
        public virtual void Write(byte[] buffer, int offset, int count)
        {
            string logText;
            Write(buffer, offset, count, DefaultLogFormat, out logText);
            WriteToLog(logText);
        }

        /// <summary>
        /// Записать строку
        /// </summary>
        public abstract void WriteLine(string text, out string logText);

        /// <summary>
        /// Записать строку и вывести информацию в журнал
        /// </summary>
        public virtual void WriteLine(string text)
        {
            string logText;
            WriteLine(text, out logText);
            WriteToLog(logText);
        }
    }
}
