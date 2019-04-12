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
 * Module   : Log
 * Summary  : Log file implementation
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2005
 * Modified : 2019
 */

using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;

namespace Utils
{
    /// <summary>
    /// Log file implementation
    /// <para>Реализация файла журнала</para>
    /// </summary>
    public class Log : ILog
    {
        /// <summary>
        /// Типы действий, записываемые в журнал
        /// </summary>
        public enum ActTypes
        {
            /// <summary>
            /// Информация
            /// </summary>
            Information,
            /// <summary>
            /// Действие
            /// </summary>
            Action,
            /// <summary>
            /// Ошибка
            /// </summary>
            Error,
            /// <summary>
            /// Исключение
            /// </summary>
            Exception,
        }

        /// <summary>
        /// Форматы журнала
        /// </summary>
        public enum Formats
        {
            /// <summary>
            /// Простой (дата, время, описание)
            /// </summary>
            Simple,
            /// <summary>
            /// Полный (дата, время, компьютер, пользователь, действие, описание)
            /// </summary>
            Full
        }

        /// <summary>
        /// Делегат записи строки в журнал
        /// </summary>
        public delegate void WriteLineDelegate(string text);
        /// <summary>
        /// Делегат записи действия в журнал
        /// </summary>
        public delegate void WriteActionDelegate(string text, ActTypes actType);

        /// <summary>
        /// Вместимость (макс. размер) файла по умолчанию, 1 МБ
        /// </summary>
        public const int DefCapacity = 1048576;

        private readonly Formats format;   // формат
        private readonly object writeLock; // объект для синхронизации обращения к журналу из разных потоков
        private StreamWriter writer;       // объект для записи в файл
        private FileInfo fileInfo;         // информация о файле


        /// <summary>
        /// Создать новый экземпляр класса Log
        /// </summary>
        protected Log()
        {
            format = Formats.Simple;
            writeLock = new object();
            writer = null;
            fileInfo = null;

            FileName = "";
            Encoding = Encoding.UTF8;
            Capacity = DefCapacity;
            CompName = Environment.MachineName;
            UserName = Environment.UserName;
            Break = new string('-', 80);
            DateTimeFormat = "yyyy'-'MM'-'dd' 'HH':'mm':'ss";
        }

        /// <summary>
        /// Создать новый экземпляр класса Log с заданным форматом записи
        /// </summary>
        public Log(Formats logFormat)
            : this()
        {
            format = logFormat;
        }


        /// <summary>
        /// Получить или установить имя журнала
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Получить или установить кодировку журнала
        /// </summary>
        public Encoding Encoding { get; set; }

        /// <summary>
        /// Получить или установить вместимость (макс. размер) журнала
        /// </summary>
        public int Capacity { get; set; }

        /// <summary>
        /// Получить имя компьютера
        /// </summary>
        public string CompName { get; private set; }

        /// <summary>
        /// Получить имя пользователя
        /// </summary>
        public string UserName { get; private set; }

        /// <summary>
        /// Получить или установить разделитель
        /// </summary>
        public string Break { get; set; }

        /// <summary>
        /// Получить или установить формат даты и времени
        /// </summary>
        public string DateTimeFormat { get; set; }


        /// <summary>
        /// Открыть журнал для добавления информации
        /// </summary>
        protected void Open()
        {
            try
            {
                writer = new StreamWriter(FileName, true, Encoding);
                fileInfo = new FileInfo(FileName);
            }
            catch
            {
            }
        }

        /// <summary>
        /// Закрыть журнал
        /// </summary>
        protected void Close()
        {
            try
            {
                if (writer != null)
                {
                    writer.Close();
                    writer = null;
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Получить строковое представления типа действия
        /// </summary>
        protected string ActTypeToStr(ActTypes actType)
        {
            switch (actType)
            {
                case ActTypes.Exception:
                    return "EXC";
                case ActTypes.Error:
                    return "ERR";
                case ActTypes.Action:
                    return "ACT";
                default: // ActTypes.Information:
                    return "INF";
            }
        }


        /// <summary>
        /// Записать действие определённого типа в журнал
        /// </summary>
        public void WriteAction(string text, ActTypes actType)
        {
            StringBuilder sb = new StringBuilder(DateTime.Now.ToString(DateTimeFormat));

            if (format == Formats.Simple)
            {
                WriteLine(sb.Append(" ").Append(text).ToString());
            }
            else
            {
                WriteLine(sb.Append(" <")
                    .Append(CompName).Append("><")
                    .Append(UserName).Append("><")
                    .Append(ActTypeToStr(actType)).Append("> ")
                    .Append(text).ToString());
            }
        }

        /// <summary>
        /// Записать информационное действие в журнал
        /// </summary>
        public void WriteInfo(string text)
        {
            WriteAction(text, ActTypes.Information);
        }

        /// <summary>
        /// Записать обычное действие в журнал
        /// </summary>
        public void WriteAction(string text)
        {
            WriteAction(text, ActTypes.Action);
        }

        /// <summary>
        /// Записать ошибку в журнал
        /// </summary>
        public void WriteError(string text)
        {
            WriteAction(text, ActTypes.Error);
        }

        /// <summary>
        /// Записать исключение в журнал
        /// </summary>
        public void WriteException(Exception ex, string errMsg = "", params object[] args)
        {
            if (string.IsNullOrEmpty(errMsg))
            {
                WriteAction(ex.ToString(), ActTypes.Exception);
            }
            else
            {
                WriteAction(new StringBuilder()
                    .Append(args == null || args.Length == 0 ? errMsg : string.Format(errMsg, args))
                    .Append(":").Append(Environment.NewLine)
                    .Append(ex.ToString()).ToString(), 
                    ActTypes.Exception);
            }
        }

        /// <summary>
        /// Записать строку в журнал
        /// </summary>
        public void WriteLine(string text = "")
        {
            try
            {
                Monitor.Enter(writeLock);
                Open();
                if (fileInfo.Length > Capacity)
                {
                    string bakName = FileName + ".bak";
                    writer.Close();
                    File.Delete(bakName);
                    File.Move(FileName, bakName);

                    writer = new StreamWriter(FileName, true, Encoding);
                }
                writer.WriteLine(text);
                writer.Flush();
            }
            catch
            {
            }
            finally
            {
                Close();
                Monitor.Exit(writeLock);
            }
        }

        /// <summary>
        /// Записать разделитель в журнал
        /// </summary>
        public void WriteBreak()
        {
            WriteLine(Break);
        }
    }
}