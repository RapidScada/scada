/*
 * Библиотека слоёв связи
 * Соединение с физическим КП
 * 
 * Разработчик:
 * 2015, Ширяев Михаил
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils;

namespace Scada.Comm.Layers
{
    /// <summary>
    /// Соединение с физическим КП
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
        /// Метод записи в журнал линии связи
        /// </summary>
        protected Log.WriteLineDelegate writeToLog;


        /// <summary>
        /// Конструктор
        /// </summary>
        public Connection()
        {
            writeToLog = text => { }; // заглушка
            DefaultLogFormat = CommUtils.ProtocolLogFormat.Hex;
            NewLine = "\n";
            LocalAddress = "";
            RemoteAddress = "";
        }


        /// <summary>
        /// Формат вывода протокола обмена данными в журнал по умлочанию
        /// </summary>
        public CommUtils.ProtocolLogFormat DefaultLogFormat { get; set; }

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
            CommUtils.ProtocolLogFormat logFormat)
        {
            return CommPhrases.ReceiveNotation + " (" + readCnt + "/" + count + "): " +
                (logFormat == CommUtils.ProtocolLogFormat.Hex ?
                    CommUtils.BytesToHex(buffer, offset, readCnt) :
                    CommUtils.BytesToString(buffer, offset, readCnt));
        }

        /// <summary>
        /// Сформировать текст считывания данных для вывода в журнал
        /// </summary>
        public static string BuildReadLogText(byte[] buffer, int offset, int readCnt,
            CommUtils.ProtocolLogFormat logFormat)
        {
            return CommPhrases.ReceiveNotation + " (" + readCnt + "): " +
                (logFormat == CommUtils.ProtocolLogFormat.Hex ?
                    CommUtils.BytesToHex(buffer, offset, readCnt) :
                    CommUtils.BytesToString(buffer, offset, readCnt));
        }

        /// <summary>
        /// Сформировать текст записи данных для вывода в журнал
        /// </summary>
        public static string BuildWriteLogText(byte[] buffer, int offset, int count, 
            CommUtils.ProtocolLogFormat logFormat)
        {
            return CommPhrases.SendNotation + " (" + count + "): " +
                (logFormat == CommUtils.ProtocolLogFormat.Hex ?
                    CommUtils.BytesToHex(buffer, offset, count) :
                    CommUtils.BytesToString(buffer, offset, count));
        }


        /// <summary>
        /// Считать данные
        /// </summary>
        /// <returns>Количество считанных байт</returns>
        public abstract int Read(byte[] buffer, int offset, int count, int timeout,
            CommUtils.ProtocolLogFormat logFormat, out string logText);

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
            out bool stopReceived, CommUtils.ProtocolLogFormat logFormat, out string logText);

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
            CommUtils.ProtocolLogFormat logFormat, out string logText);

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
