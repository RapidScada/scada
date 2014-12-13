/*
 * Copyright 2014 Mikhail Shiryaev
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
 * Modified : 2014
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
    /// <para>Реализация лог-файла</para>
    /// </summary>
    public class Log
    {
        /// <summary>
        /// Типы действий, записываемые в лог-файл
        /// </summary>
        public enum ActTypes
        {
            /// <summary>
            /// Исключение
            /// </summary>
            Exception,
            /// <summary>
            /// Ошибка
            /// </summary>
            Error,
            /// <summary>
            /// Действие
            /// </summary>
            Action,
            /// <summary>
            /// Информация
            /// </summary>
            Information
        }

        /// <summary>
        /// Форматы лог-файла
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
        /// Вместимость (макс. размер) файла по умолчанию, 1 МБ
        /// </summary>
        public const int DefCapacity = 1048576;

        private Formats format;      // формат
        private StreamWriter writer; // объект для записи в файл
        private FileInfo fileInfo;   // информация о файле
        private Object writeLock;    // объект для синхронизации обращения к лог-файлу из разных потоков


        /// <summary>
        /// Создать новый экземпляр класса Log
        /// </summary>
        protected Log()
        {
            format = Formats.Simple;
            writer = null;
            fileInfo = null;
            writeLock = new object();

            FileName = "";
            Encoding = Encoding.Default;
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
        /// Получить или установить имя лог-файла
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Получить или установить кодировку лог-файла
        /// </summary>
        public Encoding Encoding { get; set; }

        /// <summary>
        /// Получить или установить вместимость (макс. размер) лог-файла
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
        /// Открыть лог-файл для добавления информации
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
        /// Закрыть лог-файл
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
        /// Записать действие в лог-файл
        /// </summary>
        public void WriteAction(string actText)
        {
            WriteAction(actText, ActTypes.Information);
        }

        /// <summary>
        /// Записать действие в лог-файл, указав его тип
        /// </summary>
        public void WriteAction(string actText, ActTypes actType)
        {
            string nowStr = DateTime.Now.ToString(DateTimeFormat);
            if (format == Formats.Simple)
                WriteLine(nowStr + " " + actText);
            else
                WriteLine(new StringBuilder(nowStr).Append(" <").Append(CompName).Append("><").Append(UserName).
                    Append("><").Append(ActTypeToStr(actType)).Append("> ").Append(actText).ToString());
        }

        /// <summary>
        /// Записать строку в лог-файл
        /// </summary>
        public void WriteLine(string s)
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
                writer.WriteLine(s);
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
        /// Записать пустую строку в лог-файл
        /// </summary>
        public void WriteLine()
        {
            WriteLine("");
        }

        /// <summary>
        /// Записать разделитель в лог-файл
        /// </summary>
        public void WriteBreak()
        {
            WriteLine(Break);
        }
    }
}