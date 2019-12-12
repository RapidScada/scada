/*
 * Copyright 2019 Mikhail Shiryaev
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
 * Modified : 2019
 */

using System;
using System.Collections.Generic;
using System.Text;
using Utils;

namespace Scada.Comm.Channels
{
    /// <summary>
    /// The base class for connection with physical device.
    /// <para>Родительский класс соединения с физическим КП.</para>
    /// </summary>
    public abstract class Connection
    {
        /// <summary>
        /// The condition to stop reading data in a binary format.
        /// <para>Условие остановки считывания данных в бинарном формате.</para>
        /// </summary>
        public class BinStopCondition
        {
            /// <summary>
            /// Initializes a new instance of the class.
            /// </summary>
            protected BinStopCondition()
            {
                StopCode = 0;
                StopSeq = null;
            }
            /// <summary>
            /// Initializes a new instance of the class.
            /// </summary>
            public BinStopCondition(byte stopCode)
            {
                StopCode = stopCode;
                StopSeq = null;
            }
            /// <summary>
            /// Initializes a new instance of the class.
            /// </summary>
            public BinStopCondition(byte[] stopSeq)
            {
                StopCode = 0;
                StopSeq = stopSeq;
            }

            /// <summary>
            /// Получить код, приём которого останавливает считывание данных.
            /// </summary>
            public byte StopCode { get; protected set; }
            /// <summary>
            /// Gets the sequence of bytes that stops reading.
            /// </summary>
            public byte[] StopSeq { get; protected set; }

            /// <summary>
            /// Check if the stop condition is satisfied.
            /// </summary>
            public virtual bool CheckCondition(byte[] buffer, int index)
            {
                if (StopSeq == null)
                {
                    return buffer[index] == StopCode;
                }
                else
                {
                    for (int i = index, j = StopSeq.Length - 1; i >= 0 && j >= 0; i--, j--)
                    {
                        if (buffer[i] != StopSeq[j])
                            return false;
                    }

                    return true;
                }
            }
        }

        /// <summary>
        /// The condition to stop reading data in a text format.
        /// <para>Условие остановки считывания данных в текстовом формате.</para>
        /// </summary>
        public class TextStopCondition
        {
            /// <summary>
            /// Initializes a new instance of the class.
            /// </summary>
            protected TextStopCondition()
            {
                StopEndings = null;
                MaxLineCount = int.MaxValue;
            }
            /// <summary>
            /// Initializes a new instance of the class.
            /// </summary>
            public TextStopCondition(params string[] stopEndings)
            {
                StopEndings = stopEndings;
                MaxLineCount = int.MaxValue;
            }
            /// <summary>
            /// Initializes a new instance of the class.
            /// </summary>
            public TextStopCondition(int maxLineCount)
            {
                StopEndings = null;
                MaxLineCount = maxLineCount;
            }
            /// <summary>
            /// Initializes a new instance of the class.
            /// </summary>
            public TextStopCondition(string[] stopEndings, int maxLineCount = int.MaxValue)
            {
                StopEndings = stopEndings;
                MaxLineCount = maxLineCount;
            }
            /// <summary>
            /// Initializes a new instance of the class.
            /// </summary>
            public TextStopCondition(string stopEnding, int maxLineCount = int.MaxValue)
            {
                StopEndings = new string[] { stopEnding };
                MaxLineCount = maxLineCount;
            }

            /// <summary>
            /// Получить окончания строк, приём которых останавливает считывание данных.
            /// </summary>
            public string[] StopEndings { get; protected set; }
            /// <summary>
            /// Получить максимальное количество считываемых строк.
            /// </summary>
            public int MaxLineCount { get; protected set; }

            /// <summary>
            /// Проверить выполнение условия остановки.
            /// </summary>
            public virtual bool CheckCondition(List<string> lines, string lastLine)
            {
                bool stopReceived = false;

                if (lines.Count >= MaxLineCount)
                {
                    stopReceived = true;
                }
                else if (StopEndings != null)
                {
                    for (int i = 0, len = StopEndings.Length; i < len && !stopReceived; i++)
                    {
                        stopReceived = lastLine.EndsWith(StopEndings[i], StringComparison.OrdinalIgnoreCase);
                    }
                }

                return stopReceived;
            }
        }

        /// <summary>
        /// Задержка потока для накопления данных во внутреннем буфере соединения, мс
        /// </summary>
        protected const int DataAccumThreadDelay = 10;
        /// <summary>
        /// Таймаут чтения данных по умолчанию, мс
        /// </summary>
        protected const int DefaultReadTimeout = 5000;
        /// <summary>
        /// Таймаут записи данных по умолчанию, мс
        /// </summary>
        protected const int DefaultWriteTimeout = 5000;
        /// <summary>
        /// Условие остановки считывания одной строки
        /// </summary>
        protected static readonly TextStopCondition OneLineStopCondition = new TextStopCondition(1);

        /// <summary>
        /// Метод записи в журнал линии связи
        /// </summary>
        protected Log.WriteLineDelegate writeToLog;
        /// <summary>
        /// Окончание строки для методов считывания и записи строк
        /// </summary>
        protected string newLine;


        /// <summary>
        /// Конструктор
        /// </summary>
        public Connection()
        {
            writeToLog = text => { }; // заглушка
            newLine = "\r"; // 0x0D
            DefaultLogFormat = CommUtils.ProtocolLogFormats.Hex;
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
        /// Получить или установить окончание строки для методов считывания и записи строк
        /// </summary>
        public virtual string NewLine
        {
            get
            {
                return newLine;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                if (value == "")
                    throw new ArgumentException("New line must not be empty.", "value");

                newLine = value;
            }
        }

        /// <summary>
        /// Формат вывода протокола обмена данными в журнал по умлочанию
        /// </summary>
        public CommUtils.ProtocolLogFormats DefaultLogFormat { get; set; }

        /// <summary>
        /// Получить или установить метод записи в журнал линии связи
        /// </summary>
        /// <remarks>Свойство устанавливается из класса, унаследованного от KPLogic, 
        /// для последующего вызова методов чтения и записи данных</remarks>
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
        /// Проверить, что содержимое конструктора строк заканчивается на заданное значение
        /// </summary>
        protected static bool StringBuilderEndsWith(StringBuilder sb, string value, bool ignoreCase = false)
        {
            int sbInd = sb.Length - 1;
            int valLen = value.Length;
            int valInd = valLen - 1;

            for (int i = 0; i < valLen; i++)
            {
                if (sbInd <= 0)
                    return false;
                if (sb[sbInd] != value[valInd])
                    return false;
                sbInd--;
                valInd--;
            }

            return true;
        }


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
        public virtual int Read(byte[] buffer, int offset, int maxCount, int timeout, BinStopCondition stopCond,
            out bool stopReceived)
        {
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
        public virtual List<string> ReadLines(int timeout, TextStopCondition stopCond,
            out bool stopReceived)
        {
            string logText;
            List<string> lines = ReadLines(timeout, stopCond, out stopReceived, out logText);
            WriteToLog(logText);
            return lines;
        }

        /// <summary>
        /// Считать одну строку
        /// </summary>
        public virtual string ReadLine(int timeout, out string logText)
        {
            bool stopReceived;
            List<string> lines = ReadLines(timeout, OneLineStopCondition, out stopReceived, out logText);
            return lines.Count > 0 ? lines[0] : null;
        }

        /// <summary>
        /// Считать одну строку и вывести информацию в журнал
        /// </summary>
        public virtual string ReadLine(int timeout)
        {
            string logText;
            string line = ReadLine(timeout, out logText);
            WriteToLog(logText);
            return line;
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
