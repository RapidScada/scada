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
 * Module   : ScadaServerCommon
 * Summary  : The base class for server module logic
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2013
 * Modified : 2015
 */

using System;
using System.IO;
using System.Reflection;
using System.Text;
using Scada.Data;
using Utils;

namespace Scada.Server.Module
{
    /// <summary>
    /// The base class for server module logic
    /// <para>Родительский класс логики работы серверного модуля</para>
    /// </summary>
    public abstract class ModLogic
    {
        /// <summary>
        /// Команда ТУ
        /// </summary>
        [Serializable]
        public class Command
        {
            private int kpNum;
            private double cmdVal;

            /// <summary>
            /// Конструктор
            /// </summary>
            public Command()
                : this(BaseValues.CmdTypes.Standard)
            {
            }
            /// <summary>
            /// Конструктор
            /// </summary>
            public Command(int cmdTypeID)
            {
                CreateDT = DateTime.Now;
                CmdTypeID = cmdTypeID;
                KPNum = 0;
                CmdNum = 0;
                CmdVal = 0.0;
                CmdData = null;
            }

            /// <summary>
            /// Получить дату и время создания команды
            /// </summary>
            public DateTime CreateDT { get; protected set; }
            /// <summary>
            /// Получить или установить идентификатор типа команды
            /// </summary>
            public int CmdTypeID { get; set; }
            /// <summary>
            /// Получить или установить номер КП
            /// </summary>
            public int KPNum
            {
                get
                {
                    return kpNum;
                }
                set
                {
                    kpNum = value;
                    if (CmdTypeID == BaseValues.CmdTypes.Request)
                        CmdData = BitConverter.GetBytes((UInt16)kpNum);
                }
            }
            /// <summary>
            /// Получить или установить номер команды
            /// </summary>
            public int CmdNum { get; set; }
            /// <summary>
            /// Получить или установить значение команды
            /// </summary>
            public double CmdVal
            {
                get
                {
                    return cmdVal;
                }
                set
                {
                    cmdVal = value;
                    if (CmdTypeID == BaseValues.CmdTypes.Standard)
                        CmdData = BitConverter.GetBytes(cmdVal);
                }
            }
            /// <summary>
            /// Получить или установить данные команды
            /// </summary>
            public byte[] CmdData { get; set; }

            /// <summary>
            /// Получить данные команды, преобразованные в строку
            /// </summary>
            public string GetCmdDataStr()
            {
                try { return Encoding.Default.GetString(CmdData); }
                catch { return ""; }
            }
        }

        /// <summary>
        /// Делегат записи в журнал приложения
        /// </summary>
        public delegate void WriteToLogDelegate(string actText, Log.ActTypes actType);

        /// <summary>
        /// Делегат передачи команды ТУ
        /// </summary>
        public delegate void PassCommandDelegate(Command cmd);

        
        /// <summary>
        /// Конструктор
        /// </summary>
        public ModLogic()
        {
            ConfigDir = "";
            LangDir = "";
            LogDir = "";
            Settings = null;
            WriteToLog = null;
            PassCommand = null;
        }


        /// <summary>
        /// Получить имя модуля
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Получить или установить директорию конфигурации
        /// </summary>
        public string ConfigDir { get; set; }

        /// <summary>
        /// Получить или установить директорию языковых файлов
        /// </summary>
        public string LangDir { get; set; }

        /// <summary>
        /// Получить или установить директорию журналов
        /// </summary>
        public string LogDir { get; set; }

        /// <summary>
        /// Получить или установить настройки SCADA-Сервера
        /// </summary>
        public Settings Settings { get; set; }

        /// <summary>
        /// Получить или установить метод записи в журнал приложения
        /// </summary>
        public WriteToLogDelegate WriteToLog { get; set; }

        /// <summary>
        /// Получить или установить метод передачи команды ТУ
        /// </summary>
        public PassCommandDelegate PassCommand { get; set; }


        /// <summary>
        /// Выполнить действия при запуске работы сервера
        /// </summary>
        public virtual void OnServerStart()
        {
            if (WriteToLog != null)
                WriteToLog(string.Format(Localization.UseRussian ? "Запуск работы модуля {0}" : 
                    "Start {0} module", Name), Log.ActTypes.Action);
        }

        /// <summary>
        /// Выполнить действия при остановке работы сервера
        /// </summary>
        public virtual void OnServerStop()
        {
            if (WriteToLog != null)
                WriteToLog(string.Format(Localization.UseRussian ? "Завершение работы модуля {0}" :
                    "Stop {0} module", Name), Log.ActTypes.Action);
        }

        /// <summary>
        /// Выполнить действия после обработки новых текущих данных
        /// </summary>
        /// <remarks>Номера каналов упорядочены по возрастанию.
        /// Вычисление дорасчётных каналов текущего среза в момент вызова метода не выполнено</remarks>
        public virtual void OnCurDataProcessed(int[] cnlNums, SrezTableLight.Srez curSrez)
        {
        }

        /// <summary>
        /// Выполнить действия после вычисления дорасчётных каналов текущего среза
        /// </summary>
        /// <remarks>Номера каналов упорядочены по возрастанию</remarks>
        public virtual void OnCurDataCalculated(int[] cnlNums, SrezTableLight.Srez curSrez)
        {
        }

        /// <summary>
        /// Выполнить действия после обработки новых архивных данных
        /// </summary>
        /// <remarks>Номера каналов упорядочены по возрастанию.
        /// Вычисление дорасчётных каналов архивного среза в момент вызова метода завершено</remarks>
        public virtual void OnArcDataProcessed(int[] cnlNums, SrezTableLight.Srez arcSrez)
        {
        }

        /// <summary>
        /// Выполнить действия при создании события
        /// </summary>
        /// <remarks>Событие вызывается до записи на диск, поэтому свойства события можно изменить</remarks>
        public virtual void OnEventCreating(EventTableLight.Event ev)
        {
        }

        /// <summary>
        /// Выполнить действия после создания события и записи на диск
        /// </summary>
        /// <remarks>Событие вызывается после записи на диск</remarks>
        public virtual void OnEventCreated(EventTableLight.Event ev)
        {
        }

        /// <summary>
        /// Выполнить действия после квитирования события
        /// </summary>
        public virtual void OnEventChecked(DateTime date, int evNum, int userID)
        {
        }

        /// <summary>
        /// Выполнить действия после приёма команды ТУ
        /// </summary>
        /// <remarks>Метод вызывается после приёма команды ТУ от подключенных клиентов и 
        /// не вызывается после передачи команды ТУ серверными модулями</remarks>
        public virtual void OnCommandReceived(int ctrlCnlNum, Command cmd, int userID, ref bool passToClients)
        {
        }
    }
}