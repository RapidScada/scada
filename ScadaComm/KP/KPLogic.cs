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
 * Module   : KP
 * Summary  : The base class for device communication logic
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2006
 * Modified : 2014
 */

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO.Ports;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Scada.Comm.KP
{
    /// <summary>
    /// The base class for device communication logic
    /// <para>Родительский класс логики взаимодействия с КП</para>
    /// </summary>
    public abstract class KPLogic
    {
        #region Public Types
        /// <summary>
        /// Параметры опроса КП
        /// </summary>
        public struct ReqParams
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            /// <param name="isEmpty">Признак, определяющий, что параметры опроса не заданы</param>
            public ReqParams(bool isEmpty)
                : this()
            {
                IsEmpty = isEmpty;
                Timeout = 0;
                Delay = 0;
                Time = DateTime.MinValue;
                Period = TimeSpan.Zero;
                CmdLine = "";
            }

            /// <summary>
            /// Получить или установить признак, определяющий, что параметры опроса не заданы
            /// </summary>
            public bool IsEmpty { get; set; }
            /// <summary>
            /// Получить или установить таймаут запросов, мс
            /// </summary>
            public int Timeout { get; set; }
            /// <summary>
            /// Получить или установить задержку после запросов, мс
            /// </summary>
            public int Delay { get; set; }
            /// <summary>
            /// Получить или установить время опроса
            /// </summary>
            public DateTime Time { get; set; }
            /// <summary>
            /// Получить или установить период опроса, начиная с времени опроса
            /// </summary>
            public TimeSpan Period { get; set; }
            /// <summary>
            /// Получить или установить командную строку
            /// </summary>
            public string CmdLine { get; set; }
        }

        /// <summary>
        /// Параметры линии связи
        /// </summary>
        public struct LineParams
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            /// <param name="triesCnt">Количество попыток перезапроса при ошибке</param>
            /// <param name="maxCommErrCnt">Количество неудачных сеансов связи подряд до объявления КП неработающим</param>
            public LineParams(int triesCnt, int maxCommErrCnt)
                : this()
            {
                TriesCnt = triesCnt;
                MaxCommErrCnt = maxCommErrCnt;
            }

            /// <summary>
            /// Получить или установить количество попыток перезапроса при ошибке
            /// </summary>
            public int TriesCnt { get; set; }
            /// <summary>
            /// Получить или установить количество неудачных сеансов связи подряд до объявления КП неработающим
            /// </summary>
            public int MaxCommErrCnt { get; set; }
        }

        /// <summary>
        /// Статистика работы КП
        /// </summary>
        public struct Stats
        {
            /// <summary>
            /// Получить или установить количество сеансов опроса
            /// </summary>
            public int SessCnt { get; set; }
            /// <summary>
            /// Получить или установить количество неудачных сеансов опроса
            /// </summary>
            public int SessErrCnt { get; set; }
            /// <summary>
            /// Получить или установить количество команд ТУ
            /// </summary>
            public int CmdCnt { get; set; }
            /// <summary>
            /// Получить или установить количество неудачных команд ТУ
            /// </summary>
            public int CmdErrCnt { get; set; }
            /// <summary>
            /// Получить или установить количество запросов
            /// </summary>
            public int ReqCnt { get; set; }
            /// <summary>
            /// Получить или установить количество неудачных запросов
            /// </summary>
            public int ReqErrCnt { get; set; }
        }

        /// <summary>
        /// Данные параметра КП
        /// </summary>
        public struct ParamData
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            /// <param name="val">Значение</param>
            /// <param name="stat">Статус</param>
            public ParamData(double val, int stat)
                : this()
            {
                Val = val;
                Stat = stat;
            }

            /// <summary>
            /// Получить или установить значение
            /// </summary>
            public double Val { get; set; }
            /// <summary>
            /// Получить или установить статус
            /// </summary>
            public int Stat { get; set; }
        }

        /// <summary>
        /// Параметр КП
        /// </summary>
        public class Param
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            public Param()
                : this(0, "")
            {
            }
            /// <summary>
            /// Конструктор
            /// </summary>
            /// <param name="signal">Сигнал (номер параметра КП)</param>
            /// <param name="name">Наименование параметра</param>
            public Param(int signal, string name)
            {
                Signal = signal;
                Name = name;
                CnlNum = 0;
                ObjNum = 0;
                ParamID = 0;
            }
            
            /// <summary>
            /// Получить или установить сигнал (номер параметра КП)
            /// </summary>
            public int Signal { get; set; }
            /// <summary>
            /// Получить или установить наименование параметра
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// Получить или установить номер входного канала из базы конфигурации, привязанного к параметру
            /// </summary>
            public int CnlNum { get; set; }
            /// <summary>
            /// Получить или установить номер объекта из базы конфигурации
            /// </summary>
            public int ObjNum { get; set; }
            /// <summary>
            /// Получить или установить идентификатор параметра из базы конфигурации
            /// </summary>
            public int ParamID { get; set; }
        }

        /// <summary>
        /// Архивный срез параметров
        /// </summary>
        public class ParamSrez
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            private ParamSrez()
            {
            }
            /// <summary>
            /// Конструктор
            /// </summary>
            /// <param name="paramCnt">Количество параметров в срезе</param>
            public ParamSrez(int paramCnt)
            {
                this.DateTime = DateTime.MinValue;
                if (paramCnt > 0)
                {
                    KPParams = new Param[paramCnt];
                    Data = new ParamData[paramCnt];
                }
                else
                {
                    KPParams = null;
                    Data = null;
                }
                Descr = "";
            }

            /// <summary>
            /// Получить или установить временную метку
            /// </summary>
            public DateTime DateTime { get; set; }
            /// <summary>
            /// Получить ссылки на параметры КП, входящие в срез
            /// </summary>
            public Param[] KPParams { get; private set; }
            /// <summary>
            /// Получить данные параметров среза
            /// </summary>
            public ParamData[] Data { get; private set; }
            /// <summary>
            /// Получить или установить описание среза (записывается в файл информации о работе КП)
            /// </summary>
            public string Descr { get; set; }

            /// <summary>
            /// Получить массив индексов параметров среза, привязанных к входным каналам
            /// </summary>
            /// <returns>Массив индексов параметров среза</returns>
            public List<int> GetBindedParamIndexes()
            {
                List<int> indexes = new List<int>();
                if (KPParams != null)
                {
                    for (int i = 0; i < KPParams.Length; i++)
                    {
                        Param param = KPParams[i];
                        if (param != null && param.CnlNum > 0)
                            indexes.Add(i);
                    }
                }

                return indexes;
            }
        }

        /// <summary>
        /// Группа параметров КП
        /// </summary>
        public class ParamGroup
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            private ParamGroup()
            {
            }
            /// <summary>
            /// Конструктор
            /// </summary>
            /// <param name="paramCnt">Количество параметров в группе</param>
            public ParamGroup(int paramCnt)
                : this("", paramCnt)
            {
            }
            /// <summary>
            /// Конструктор
            /// </summary>
            /// <param name="name">Наименование группы</param>
            /// <param name="paramCnt">Количество параметров в группе</param>
            public ParamGroup(string name, int paramCnt)
            {
                Name = name;
                KPParams = paramCnt > 0 ? new Param[paramCnt] : null;
            }

            /// <summary>
            /// Получить или установить наименование группы
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// Получить ссылки на параметры КП, входящие в группу
            /// </summary>
            public Param[] KPParams { get; private set; }
        }

        /// <summary>
        /// Событие
        /// </summary>
        public class Event
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            public Event()
                : this(DateTime.MinValue, 0, null)
            {
            }
            /// <summary>
            /// Конструктор
            /// </summary>
            /// <param name="dateTime">Временная метка события</param>
            /// <param name="kpNum">Номер КП из базы конфигурации</param>
            /// <param name="kpParam">Параметр КП</param>
            public Event(DateTime dateTime, int kpNum, Param kpParam)
            {
                DateTime = dateTime;
                KPNum = kpNum;
                KPParam = kpParam;
                OldData = new ParamData(0.0, 0);
                NewData = new ParamData(0.0, 0);
                Checked = false;
                UserID = 0;
                Descr = "";
                Data = "";
            }

            /// <summary>
            /// Получить или установить временную метку
            /// </summary>
            public DateTime DateTime { get; set; }
            /// <summary>
            /// Получить или установить номер КП из базы конфигурации
            /// </summary>
            public int KPNum { get; set; }
            /// <summary>
            /// Получить или установить ссылку на параметр КП
            /// </summary>
            public Param KPParam { get; set; }
            /// <summary>
            /// Получить или установить старые данные параметра КП
            /// </summary>
            public ParamData OldData { get; set; }
            /// <summary>
            /// Получить или установить новые данные параметра КП
            /// </summary>
            public ParamData NewData { get; set; }
            /// <summary>
            /// Получить или установить признак квитирования события
            /// </summary>
            public bool Checked { get; set; }
            /// <summary>
            /// Получить или установить идентификатор пользователя из базы конфигурации, квитировавшего событие
            /// </summary>
            public int UserID { get; set; }
            /// <summary>
            /// Получить или установить описание события
            /// </summary>
            public string Descr { get; set; }
            /// <summary>
            /// Получить или установить дополнительные данные события
            /// </summary>
            public string Data { get; set; }
        }

        /// <summary>
        /// Тип команды КП
        /// </summary>
        public enum CmdType
        {
            /// <summary>
            /// Стандартная команда (ТУ)
            /// </summary>
            Standard = 0,
            /// <summary>
            /// Бинарная команда
            /// </summary>
            Binary = 1,
            /// <summary>
            /// Внеочередной опрос КП
            /// </summary>
            Request = 2
        }

        /// <summary>
        /// Команда КП
        /// </summary>
        public class Command
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            public Command()
                : this(CmdType.Standard)
            {
            }
            /// <summary>
            /// Конструктор
            /// </summary>
            /// <param name="cmdType">Тип команды</param>
            public Command(CmdType cmdType)
            {
                CmdType = cmdType;
                KPNum = 0;
                CmdNum = 0;
                CmdVal = 0.0;
                CmdData = null;
            }

            /// <summary>
            /// Получить или установить тип команды
            /// </summary>
            public CmdType CmdType { get; set; }
            /// <summary>
            /// Получить или установить номер КП, для которого предназначена команда
            /// </summary>
            public int KPNum { get; set; }
            /// <summary>
            /// Получить или установить номер команды
            /// </summary>
            public int CmdNum { get; set; }
            /// <summary>
            /// Получить или установить значение команды
            /// </summary>
            public double CmdVal { get; set; }
            /// <summary>
            /// Получить или установить бинарные данные команды (вместо значения)
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
        /// Состояния работы КП
        /// </summary>
        public enum WorkStates
        {
            /// <summary>
            /// Неопределено
            /// </summary>
            Undefined,
            /// <summary>
            /// Норма
            /// </summary>
            Normal,
            /// <summary>
            /// Ошибка
            /// </summary>
            Error
        }

        /// <summary>
        /// Делегат форсированной передачи архивов срезов и событий 
        /// во время сеанса опроса или отправки команды ТУ
        /// </summary>
        public delegate bool FlushArcDelegate(KPLogic kpLogic);

        /// <summary>
        /// Делегат записи в журнал линии связи
        /// </summary>
        public delegate void WriteToLogDelegate(string text);

        /// <summary>
        /// Делегат передачи команды КП
        /// </summary>
        public delegate void PassCmdDelegate(Command cmd);
        #endregion

        #region Non-Public Fields
        /// <summary>
        /// Размер кольцевого буфера архивных срезов
        /// </summary>
        protected const int SrezBufSize = 10;
        /// <summary>
        /// Размер кольцевого буфера событий
        /// </summary>
        protected const int EventBufSize = 10;
        /// <summary>
        /// Формат даты и времени для вывода в журнал линии связи
        /// </summary>
        protected const string DateTimeFormat = "yyyy'-'MM'-'dd' 'HH':'mm':'ss";

        private string caption;        // обозначение КП
        private string sessDescr;      // описание старта сеанса опроса
        private string cmdDescr;       // описание отправки команды ТУ
        private string[][] paramTable; // таблица текущих значений параметров КП
        private int[] paramColLen;     // ширина столбцов таблицы параметров
        private NumberFormatInfo nfi;  // используемый формат строковой записи вещественных чисел

        /// <summary>
        /// Статистика работы КП
        /// </summary>
        protected Stats kpStats;
        /// <summary>
        /// Последний сеанс связи завершён успешно
        /// </summary>
        protected bool lastCommSucc;
        /// <summary>
        /// Количество неудачных сеансов связи подряд (опрос или команда ТУ)
        /// </summary>
        protected int commErrRepeat;
        /// <summary>
        /// Кольцевой буфер архивных срезов
        /// </summary>
        protected List<ParamSrez> srezBufList;
        /// <summary>
        /// Кольцевой буфер событий
        /// </summary>
        protected List<Event> eventBufList;
        /// <summary>
        /// Признак завершения работы линии связи
        /// </summary>
        protected volatile bool terminated;
        #endregion

        #region Constructors
        /// <summary>
        /// Конструктор
        /// </summary>
        private KPLogic()
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="number">Номер КП</param>
        public KPLogic(int number)
        {
            // private fields
            caption = "";
            sessDescr = "";
            cmdDescr = "";
            paramTable = null;
            paramColLen = null;

            nfi = new NumberFormatInfo();
            nfi.NumberDecimalDigits = 3;
            nfi.NumberDecimalSeparator = Localization.Culture.NumberFormat.NumberDecimalSeparator;
            nfi.NumberGroupSeparator = Localization.Culture.NumberFormat.NumberGroupSeparator;

            // protected fields
            kpStats.SessCnt = 0;
            kpStats.SessErrCnt = 0;
            kpStats.CmdCnt = 0;
            kpStats.CmdErrCnt = 0;
            kpStats.ReqCnt = 0;
            kpStats.ReqErrCnt = 0;

            lastCommSucc = false;
            commErrRepeat = 0;
            srezBufList = new List<ParamSrez>();
            eventBufList = new List<Event>();
            terminated = false;

            // public properties
            CanSendCmd = false;

            Bind = false;
            Number = number;
            Name = "";
            Dll = Assembly.GetCallingAssembly().GetName().Name;
            Address = 0;
            CallNum = "";

            SerialPort = null;
            UserParams = null;
            CommonProps = null;

            WorkState = WorkStates.Undefined;
            LastSessDT = DateTime.MinValue;
            LastCmdDT = DateTime.MinValue;

            KPParams = null;
            ParamGroups = null;

            CurData = null;
            CurModified = null;
            SrezList = new List<ParamSrez>();
            EventList = new List<Event>();

            ConfigDir = "";
            LangDir = "";
            LogDir = "";
            CmdDir = "";
            FlushArc = null;
            WriteToLog = null;
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Получить или установить признак завершения работы линии связи
        /// </summary>
        /// <remarks>Если значение равно true, то необходимо прервать сеанс опроса или отправку команды КП. 
        /// Установка значения в false во время сеанса опроса приостанавливает завершение работы линии связи</remarks>
        public bool Terminated
        {
            get
            {
                return terminated;
            }
            set
            {
                terminated = value;
            }
        }

        /// <summary>
        /// Получить возможность отправки команд ТУ
        /// </summary>
        public bool CanSendCmd { get; protected set; }


        /// <summary>
        /// Получить или установить привязку к базе конфигурации
        /// </summary>
        public bool Bind { get; set; }

        /// <summary>
        /// Получить номер КП
        /// </summary>
        public int Number { get; private set; }

        /// <summary>
        /// Получить или установить наименование КП
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Получить обозначение КП
        /// </summary>
        public string Caption
        {
            get
            {
                if (caption == "")
                    caption = (Localization.UseRussian ? "КП " : "Device ") + 
                        Number + (string.IsNullOrEmpty(Name) ? "" : " \"" + Name + "\"");
                return caption;
            }
        }

        /// <summary>
        /// Получить наименование библиотеки КП
        /// </summary>
        public string Dll { get; private set; }

        /// <summary>
        /// Получить или установить адрес КП
        /// </summary>
        public int Address { get; set; }

        /// <summary>
        /// Получить или установить позывной (строковый адрес) КП
        /// </summary>
        public string CallNum { get; set; }

        /// <summary>
        /// Получить или установить параметры опроса КП
        /// </summary>
        public ReqParams KPReqParams { get; set; }


        /// <summary>
        /// Получить или установить ссылку на последовательный порт
        /// </summary>
        public SerialPort SerialPort { get; set; }

        /// <summary>
        /// Получить или установить параметры линии связи
        /// </summary>
        public LineParams CommLineParams { get; set; }

        /// <summary>
        /// Получить или установить ссылку на пользовательские параметры линии связи
        /// </summary>
        public SortedList<string, string> UserParams { get; set; }

        /// <summary>
        /// Получить или установить ссылку на общие свойства линии связи, доступные относящимся к ней КП
        /// </summary>
        public SortedList<string, object> CommonProps { get; set; }
        

        /// <summary>
        /// Получить статистику работы КП
        /// </summary>
        public Stats KPStats
        {
            get
            {
                return kpStats;
            }
        }

        /// <summary>
        /// Получить состояние работы КП
        /// </summary>
        public WorkStates WorkState { get; protected set; }

        /// <summary>
        /// Получить строковое представление состояния работы КП
        /// </summary>
        public string WorkStateStr
        {
            get
            {
                switch (WorkState)
                {
                    case WorkStates.Undefined:
                        return Localization.UseRussian ? "неопределено" : "undefined";
                    case WorkStates.Normal:
                        return Localization.UseRussian ? "норма" : "normal";
                    case WorkStates.Error:
                        return Localization.UseRussian ? "ошибка" : "error";
                    default:
                        return "";
                }
            }
        }

        /// <summary>
        /// Получить дату и время последнего сеанса опроса
        /// </summary>
        public DateTime LastSessDT { get; protected set; }

        /// <summary>
        /// Получить дату и время последней отправки команды ТУ
        /// </summary>
        public DateTime LastCmdDT { get; protected set; }


        /// <summary>
        /// Получить параметры КП
        /// </summary>
        public Param[] KPParams { get; protected set; }

        /// <summary>
        /// Получить группы параметров КП
        /// </summary>
        public ParamGroup[] ParamGroups { get; protected set; }


        /// <summary>
        /// Получить текущие данные параметров
        /// </summary>
        public ParamData[] CurData { get; protected set; }

        /// <summary>
        /// Получить признаки изменения текущих данных параметров
        /// </summary>
        public bool[] CurModified { get; protected set; }

        /// <summary>
        /// Получить список архивных срезов
        /// </summary>
        public List<ParamSrez> SrezList { get; protected set; }

        /// <summary>
        /// Получить список событий
        /// </summary>
        public List<Event> EventList { get; protected set; }


        /// <summary>
        /// Получить или установить директорию конфигурации программы
        /// </summary>
        public string ConfigDir { get; set; }

        /// <summary>
        /// Получить или установить директорию языковых файлов
        /// </summary>
        public string LangDir { get; set; }

        /// <summary>
        /// Получить или установить директория файлов журналов программы
        /// </summary>
        public string LogDir { get; set; }

        /// <summary>
        /// Получить или установить директорию команд
        /// </summary>
        public string CmdDir { get; set; }

        /// <summary>
        /// Получить или установить метод форсированной передачи архивов срезов и событий
        /// </summary>
        public FlushArcDelegate FlushArc { get; set; }

        /// <summary>
        /// Получить или установить метод записи в журнал линии связи
        /// </summary>
        public WriteToLogDelegate WriteToLog { get; set; }

        /// <summary>
        /// Получить или установить метод передачи команды КП
        /// </summary>
        public PassCmdDelegate PassCmd { get; set; }
        #endregion

        #region Non-Public Methods
        /// <summary>
        /// Получить максимальную длину строки в массиве
        /// </summary>
        /// <param name="strings">Массив строк</param>
        /// <returns>Максимальная длина строки в массиве</returns>
        private int GetMaxLength(string[] strings)
        {
            int max = 0;

            if (strings != null)
            {
                foreach (string s in strings)
                    if (s != null && s.Length > max)
                        max = s.Length;
            }

            return max;
        }

        /// <summary>
        /// Добавить к конструктору строки текущие значения параметров КП
        /// </summary>
        private void AppendKPParams(StringBuilder sb)
        {
            sb.AppendLine();

            if (KPParams == null || KPParams.Length == 0)
            {
                sb.AppendLine(Localization.UseRussian ? "Теги КП отсутствуют" : "No device tags");
            }
            else
            {
                sb.AppendLine(Localization.UseRussian ? "Текущие данные" : "Current data");

                bool writeGroups = ParamGroups != null && ParamGroups.Length > 0; // вывод параметров по группам
                string[] colSig;  // столбец сигналов
                string[] colName; // столбец наименований
                string[] colVal;  // столбец значений
                string[] colCnl;  // столбец каналов
                int rowCnt;       // количество строк

                if (paramTable == null)
                {
                    // формирование таблицы текущих значений параметров КП
                    paramTable = new string[4][];
                    paramColLen = new int[4];
                    rowCnt = writeGroups ? KPParams.Length + ParamGroups.Length + 1 : KPParams.Length + 1;

                    paramTable[0] = colSig = new string[rowCnt];
                    paramTable[1] = colName = new string[rowCnt];
                    paramTable[2] = colVal = new string[rowCnt];
                    paramTable[3] = colCnl = new string[rowCnt];

                    if (Localization.UseRussian)
                    {
                        colSig[0] = "Сигнал";
                        colName[0] = "Наименование";
                        colVal[0] = "Значение";
                        colCnl[0] = "Канал";
                    }
                    else
                    {
                        colSig[0] = "Signal";
                        colName[0] = "Name";
                        colVal[0] = "Value";
                        colCnl[0] = "Channel";
                    }

                    if (writeGroups)
                    {
                        int i = 1;

                        foreach (ParamGroup group in ParamGroups)
                        {
                            colSig[i] = "*";
                            colName[i] = group.Name;
                            colVal[i] = "";
                            colCnl[i] = "";
                            i++;

                            foreach (Param param in group.KPParams)
                            {
                                colSig[i] = param.Signal.ToString();
                                colCnl[i] = param.CnlNum > 0 ? param.CnlNum.ToString() : "";
                                i++;
                            }
                        }
                    }
                    else
                    {
                        for (int i = 1; i < rowCnt; i++)
                        {
                            Param param = KPParams[i - 1];
                            colSig[i] = param.Signal.ToString();
                            colCnl[i] = param.CnlNum > 0 ? param.CnlNum.ToString() : "";
                        }
                    }

                    // вычисление ширины столбцов
                    paramColLen[0] = GetMaxLength(colSig);
                    paramColLen[3] = GetMaxLength(colCnl);
                }
                else
                {
                    colSig = paramTable[0];
                    colName = paramTable[1];
                    colVal = paramTable[2];
                    colCnl = paramTable[3];
                    rowCnt = colSig.Length;
                }

                // заполение столбцов наименований и текущих параметров, вычисление их ширины
                for (int i = 1, paramInd = 0; i < rowCnt; i++)
                {
                    if (colSig[i] != "*")
                    {
                        Param param = KPParams[paramInd];
                        colName[i] = param.Name;
                        colVal[i] = ParamDataToStr(param.Signal, CurData[paramInd]);
                        paramInd++;
                    }
                }

                paramColLen[1] = GetMaxLength(colName);
                paramColLen[2] = GetMaxLength(colVal);

                // представление таблицы текущих значений параметров КП в виде строк
                int lenSig = paramColLen[0];
                int lenName = paramColLen[1];
                int lenVal = paramColLen[2];
                int lenCnl = paramColLen[3];
                string br = (new StringBuilder()).Append("+-").Append(new string('-', lenSig)).Append("-+-").
                    Append(new string('-', lenName)).Append("-+-").Append(new string('-', lenVal)).Append("-+-").
                    Append(new string('-', lenCnl)).Append("-+").ToString();

                sb.AppendLine(br);
                string grBeg = new string('*', lenSig + 2) + " ";

                for (int i = 0; i < rowCnt; i++)
                {
                    if (colSig[i] == "*")
                    {
                        if (colName[i] == "")
                        {
                            sb.Append("| ").Append(new string('*', br.Length - 4)).Append(" |").AppendLine();
                        }
                        else
                        {
                            string s = "| " + grBeg + colName[i] + " ";
                            sb.Append(s).Append(new string('*', br.Length - s.Length - 2)).Append(" |").AppendLine();
                        }
                    }
                    else
                    {
                        sb.Append("| ").Append(colSig[i].PadLeft(lenSig)).Append(" | ").
                            Append(colName[i].PadRight(lenName)).Append(" | ").Append(colVal[i].PadLeft(lenVal)).
                            Append(" | ").Append(colCnl[i].PadLeft(lenCnl)).Append(" |").AppendLine();
                    }
                    sb.AppendLine(br);
                }
            }
        }

        /// <summary>
        /// Добавить к конструктору строки архивные срезы КП
        /// </summary>
        private void AppendSrezBufList(StringBuilder sb)
        {
            sb.AppendLine();

            if (srezBufList.Count == 0)
            {
                sb.AppendLine(Localization.UseRussian ? "Архивные данные отсутствуют" : "No archive data");
            }
            else
            {
                sb.AppendLine(string.Format(Localization.UseRussian ?
                    "Архивные данные (последние {0} срезов)" : "Archive data (recent {0} snapshots)", SrezBufSize));

                int rowCnt = srezBufList.Count + 1;
                string[] numArr = new string[rowCnt];
                string[] timeArr = new string[rowCnt];
                string[] descrArr = new string[rowCnt];

                if (Localization.UseRussian)
                {
                    numArr[0] = "№";
                    timeArr[0] = "Дата и время";
                    descrArr[0] = "Описание";
                }
                else
                {
                    numArr[0] = "Num.";
                    timeArr[0] = "Date and Time";
                    descrArr[0] = "Description";
                }

                int rowNum = 1;
                foreach (ParamSrez paramSrez in srezBufList)
                {
                    numArr[rowNum] = rowNum.ToString();
                    timeArr[rowNum] = paramSrez.DateTime.ToString("d") + " " + paramSrez.DateTime.ToString("T");
                    descrArr[rowNum] = paramSrez.Descr;
                    rowNum++;
                }

                // представление таблицы архивных срезов в виде строк
                int lenNum = GetMaxLength(numArr);
                int lenTime = GetMaxLength(timeArr);
                int lenDescr = GetMaxLength(descrArr);
                string br = (new StringBuilder()).Append("+-").Append(new string('-', lenNum)).Append("-+-").
                    Append(new string('-', lenTime)).Append("-+-").Append(new string('-', lenDescr)).Append("-+").
                    ToString();

                sb.AppendLine(br).
                    Append("| ").Append(numArr[0].PadRight(lenNum)).Append(" | ").Append(timeArr[0].PadRight(lenTime)).
                    Append(" | ").Append(descrArr[0].PadRight(lenDescr)).Append(" |").AppendLine().
                    AppendLine(br);

                for (int i = 1; i < rowCnt; i++)
                {
                    sb.Append("| ").Append(numArr[i].PadLeft(lenNum)).Append(" | ").
                        Append(timeArr[i].PadRight(lenTime)).Append(" | ").
                        Append(descrArr[i].PadRight(lenDescr)).Append(" |").AppendLine().
                        AppendLine(br);
                }
            }
        }

        /// <summary>
        /// Добавить к конструктору строки события КП
        /// </summary>
        private void AppendEventBufList(StringBuilder sb)
        {
            sb.AppendLine();

            if (eventBufList.Count == 0)
            {
                sb.AppendLine(Localization.UseRussian ? "События отсутствуют" : "No events");
            }
            else
            {
                sb.AppendLine(string.Format(Localization.UseRussian ?
                    "События (последние {0})" : "Events (recent {0})", EventBufSize));

                int rowCnt = eventBufList.Count + 1;
                string[] numArr = new string[rowCnt];
                string[] timeArr = new string[rowCnt];
                string[] sigArr = new string[rowCnt];
                string[] descrArr = new string[rowCnt];

                if (Localization.UseRussian)
                {
                    numArr[0] = "№";
                    timeArr[0] = "Дата и время";
                    sigArr[0] = "Сигнал";
                    descrArr[0] = "Описание";
                }
                else
                {
                    numArr[0] = "Num.";
                    timeArr[0] = "Date and Time";
                    sigArr[0] = "Signal";
                    descrArr[0] = "Description";
                }

                int rowNum = 1;
                foreach (Event ev in eventBufList)
                {
                    numArr[rowNum] = rowNum.ToString();
                    timeArr[rowNum] = ev.DateTime.ToString("d") + " " + ev.DateTime.ToString("T");
                    sigArr[rowNum] = ev.KPParam == null ? "" : ev.KPParam.Signal.ToString();
                    descrArr[rowNum] = ev.Descr.Replace("\n", "\\").Replace("\r", "");
                    rowNum++;
                }

                // вывод таблицы событий в файл
                int lenNum = GetMaxLength(numArr);
                int lenTime = GetMaxLength(timeArr);
                int lenSig = GetMaxLength(sigArr);
                int lenDescr = GetMaxLength(descrArr);
                string br = (new StringBuilder()).Append("+-").Append(new string('-', lenNum)).Append("-+-").
                    Append(new string('-', lenTime)).Append("-+-").Append(new string('-', lenSig)).Append("-+-").
                    Append(new string('-', lenDescr)).Append("-+").ToString();

                sb.AppendLine(br).
                    Append("| ").Append(numArr[0].PadRight(lenNum)).Append(" | ").Append(timeArr[0].PadRight(lenTime)).
                    Append(" | ").Append(sigArr[0].PadRight(lenSig)).Append(" | ").
                    Append(descrArr[0].PadRight(lenDescr)).Append(" |").AppendLine().
                    AppendLine(br);

                for (int i = 1; i < rowCnt; i++)
                {
                    sb.Append("| ").Append(numArr[i].PadLeft(lenNum)).Append(" | ").
                        Append(timeArr[i].PadRight(lenTime)).Append(" | ").Append(sigArr[i].PadLeft(lenSig)).
                        Append(" | ").Append(descrArr[i].PadRight(lenDescr)).Append(" |").AppendLine().
                        AppendLine(br);
                }
            }
        }
             

        /// <summary>
        /// Инициализировать массивы параметров КП, групп параметров КП, текущих данных и признаков их изменения
        /// </summary>
        /// <param name="paramCnt">Количество параметров КП</param>
        /// <param name="groupCnt">Количество групп параметров КП</param>
        protected void InitArrays(int paramCnt, int groupCnt)
        {
            KPParams = new Param[paramCnt];
            CurData = new ParamData[paramCnt];
            CurModified = new bool[paramCnt];

            for (int i = 0; i < paramCnt; i++)
            {
                KPParams[i] = null;
                CurData[i].Val = 0.0;
                CurData[i].Stat = 0;
                CurModified[i] = true;
            }

            if (groupCnt > 0)
            {
                ParamGroups = new ParamGroup[groupCnt];
                for (int i = 0; i < groupCnt; i++)
                    ParamGroups[i] = null;
            }
        }

        /// <summary>
        /// Копировать параметры КП из групп в массив параметров
        /// </summary>
        protected void CopyParamsFromGroups()
        {
            if (ParamGroups != null)
            {
                int i = 0;

                foreach (ParamGroup group in ParamGroups)
                {
                    if (group.KPParams != null)
                    {
                        foreach (Param param in group.KPParams)
                            KPParams[i++] = param;
                    }
                }
            }
        }

        /// <summary>
        /// Установить текущие данные параметра КП и признак их изменения
        /// </summary>
        /// <param name="paramIndex">Индекс параметра КП</param>
        /// <param name="newVal">Новое значение параметра КП</param>
        /// <param name="newStat">Новый статус параметра КП</param>
        protected void SetParamData(int paramIndex, double newVal, int newStat)
        {
            if (0 <= paramIndex && paramIndex < KPParams.Length)
            {
                CurModified[paramIndex] = CurData[paramIndex].Val != newVal || CurData[paramIndex].Stat != newStat;
                CurData[paramIndex].Val = newVal;
                CurData[paramIndex].Stat = newStat;
            }
        }

        /// <summary>
        /// Установить текущие данные параметра КП и признак их изменения
        /// </summary>
        /// <param name="paramIndex">Индекс параметра КП</param>
        /// <param name="newData">Новые данные параметра КП</param>
        protected void SetParamData(int paramIndex, ParamData newData)
        {
            if (0 <= paramIndex && paramIndex < KPParams.Length)
            {
                CurModified[paramIndex] = CurData[paramIndex].Val != newData.Val || CurData[paramIndex].Stat != newData.Stat;
                CurData[paramIndex] = newData;
            }
        }

        /// <summary>
        /// Получить массив аргументов командной строки
        /// </summary>
        /// <returns>Массив аргументов командной строки</returns>
        protected string[] GetCmdLineArgs()
        {
            return KPReqParams.CmdLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// Выполнить действия, необходимые для завершения запроса
        /// </summary>
        protected void FinishRequest()
        {
            Thread.Sleep(KPReqParams.Delay);
            kpStats.ReqCnt++;
            if (!lastCommSucc)
                kpStats.ReqErrCnt++;
        }

        /// <summary>
        /// Рассчитать статистику сеансов опроса и состояние работы КП
        /// </summary>
        protected void CalcSessStats()
        {
            kpStats.SessCnt++;
            if (lastCommSucc)
            {
                commErrRepeat = 0;
                WorkState = WorkStates.Normal;
            }
            else
            {
                kpStats.SessErrCnt++;
                commErrRepeat++;
                if (commErrRepeat >= CommLineParams.MaxCommErrCnt)
                    WorkState = WorkStates.Error;
            }
        }

        /// <summary>
        /// Рассчитать статистику команд ТУ и состояние работы КП
        /// </summary>
        protected void CalcCmdStats()
        {
            kpStats.CmdCnt++;
            if (lastCommSucc)
            {
                commErrRepeat = 0;
                WorkState = WorkStates.Normal;
            }
            else
            {
                kpStats.CmdErrCnt++;
                commErrRepeat++;
                if (commErrRepeat >= CommLineParams.MaxCommErrCnt)
                    WorkState = WorkStates.Error;
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Выполнить сеанс опроса КП
        /// </summary>
        public virtual void Session()
        {
            LastSessDT = DateTime.Now;
            lastCommSucc = true;

            if (WriteToLog != null)
            {
                if (sessDescr == "")
                {
                    sessDescr = Localization.UseRussian ?
                        " Сеанс связи с " + Caption + ", тип: " + Dll +
                        (Address <= 0 ? "" : ", адрес: " + Address) +
                        (string.IsNullOrEmpty(CallNum) ? "" : ", позывной: " + CallNum) :

                        " Communication session with " + Caption + ", type: " + Dll +
                        (Address <= 0 ? "" : ", address: " + Address) +
                        (string.IsNullOrEmpty(CallNum) ? "" : ", call number: " + CallNum);
                }

                WriteToLog("");
                WriteToLog(LastSessDT.ToString(DateTimeFormat) + sessDescr);
            }
        }

        /// <summary>
        /// Отправить команду ТУ
        /// </summary>
        /// <param name="cmd">Команда ТУ</param>
        public virtual void SendCmd(Command cmd)
        {
            LastCmdDT = DateTime.Now;
            lastCommSucc = CanSendCmd;

            if (WriteToLog != null)
            {
                if (cmdDescr == "")
                {
                    cmdDescr = Localization.UseRussian ?
                        " Команда " + Caption + ", тип: " + Dll +
                        (Address <= 0 ? "" : ", адрес: " + Address) +
                        (string.IsNullOrEmpty(CallNum) ? "" : ", позывной: " + CallNum) :

                        " Command to " + Caption + ", type: " + Dll +
                        (Address <= 0 ? "" : ", address: " + Address) +
                        (string.IsNullOrEmpty(CallNum) ? "" : ", call number: " + CallNum);
                }

                WriteToLog("");
                WriteToLog(LastSessDT.ToString(DateTimeFormat) + cmdDescr);

                if (!CanSendCmd)
                    WriteToLog(Localization.UseRussian ? 
                        "КП не поддерживает отправку команд" : "The device does not support sending command");
            }
        }


        /// <summary>
        /// Выполнить действия после добавления КП на линию связи
        /// </summary>
        /// <remarks>Метод вызывается после установки необходимых для работы КП свойств, 
        /// перед привязкой параметров КП к входным каналам</remarks>
        public virtual void OnAddedToCommLine()
        {
        }

        /// <summary>
        /// Выполнить действия при запуске линии связи
        /// </summary>
        public virtual void OnCommLineStart()
        {
        }

        /// <summary>
        /// Выполнить действия при завершении работы линии связи
        /// </summary>
        public virtual void OnCommLineTerminate()
        {
        }

        /// <summary>
        /// Выполнить действия при прерывании работы линии связи
        /// </summary>
        public virtual void OnCommLineAbort()
        {
            OnCommLineTerminate();
        }


        /// <summary>
        /// Привязать параметр КП к входному каналу из базы конфигурации
        /// </summary>
        /// <param name="signal">Сигнал (номер параметра КП), нумерация от 1</param>
        /// <param name="cnlNum">Номер входного канала</param>
        /// <param name="objNum">Номер объекта</param>
        /// <param name="paramID">Идентификатор параметра</param>
        public virtual void BindParam(int signal, int cnlNum, int objNum, int paramID)
        {
            if (KPParams != null && cnlNum > 0)
            {
                Param kpParam = null;

                if (signal > 0)
                {
                    // поиск параметра по сигналу при их последовательной нумерации
                    if (signal <= KPParams.Length)
                    {
                        Param p = KPParams[signal - 1];
                        if (p != null && p.Signal == signal)
                            kpParam = p;
                    }

                    // поиск параметра по сигналу при их произвольной нумерации
                    if (kpParam == null)
                    {
                        foreach (Param p in KPParams)
                        {
                            if (p != null && p.Signal == signal)
                            {
                                kpParam = p;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    // поиск параметра по номеру канала
                    foreach (Param p in KPParams)
                    {
                        if (p != null && p.CnlNum == cnlNum)
                        {
                            kpParam = p;
                            break;
                        }
                    }
                }

                // привязка параметра
                if (kpParam != null)
                {
                    kpParam.CnlNum = cnlNum;
                    kpParam.ObjNum = objNum;
                    kpParam.ParamID = paramID;
                }
            }
        }

        /// <summary>
        /// Преобразовать данные параметра КП в строку
        /// </summary>
        /// <param name="signal">Сигнал (номер параметра КП)</param>
        /// <param name="paramData">Данные параметра КП</param>
        /// <returns>Строковое представление данных параметра КП</returns>
        public virtual string ParamDataToStr(int signal, ParamData paramData)
        {
            return paramData.Stat > 0 ? paramData.Val.ToString("N", nfi) : "---";
        }

        /// <summary>
        /// Получить информацию о работе КП
        /// </summary>
        /// <returns>Информация о работе КП</returns>
        public virtual string GetInfo()
        {
            StringBuilder sbInfo = new StringBuilder();
            string title = Caption + ", DLL: " + Dll;
            sbInfo.AppendLine(title).
                AppendLine(new string('-', title.Length));

            if (Localization.UseRussian)
            {
                sbInfo.AppendLine("Состояние   : " + WorkStateStr).
                    Append("Сеанс связи : ").
                    Append(LastSessDT > DateTime.MinValue ? 
                        LastSessDT.ToLocalizedString() : "время неопределено").AppendLine().
                    Append("Команда ТУ  : ").
                    Append(LastCmdDT > DateTime.MinValue ? 
                        LastCmdDT.ToLocalizedString() : "время неопределено").AppendLine().
                    AppendLine().
                    Append("Сеансы связи (всего / ошибок) : ").
                        Append(kpStats.SessCnt).Append(" / ").Append(kpStats.SessErrCnt).AppendLine().
                    Append("Команды ТУ   (всего / ошибок) : ").
                        Append(kpStats.CmdCnt).Append(" / ").Append(kpStats.CmdErrCnt).AppendLine().
                    Append("Запросы      (всего / ошибок) : ").
                        Append(kpStats.ReqCnt).Append(" / ").Append(kpStats.ReqErrCnt).AppendLine();
            }
            else
            {
                sbInfo.AppendLine("State         : " + WorkStateStr).
                    Append("Comm. session : ").
                    Append(LastSessDT > DateTime.MinValue ?
                        LastSessDT.ToLocalizedString() : "time is undefined").AppendLine().
                    Append("Command       : ").
                    Append(LastCmdDT > DateTime.MinValue ?
                        LastCmdDT.ToLocalizedString() : "time is undefined").AppendLine().
                    AppendLine().
                    Append("Comm. sessions (total / errors) : ").
                        Append(kpStats.SessCnt).Append(" / ").Append(kpStats.SessErrCnt).AppendLine().
                    Append("Commands       (total / errors) : ").
                        Append(kpStats.CmdCnt).Append(" / ").Append(kpStats.CmdErrCnt).AppendLine().
                    Append("Requests       (total / errors) : ").
                        Append(kpStats.ReqCnt).Append(" / ").Append(kpStats.ReqErrCnt).AppendLine();
            }

            AppendKPParams(sbInfo);
            AppendSrezBufList(sbInfo);
            AppendEventBufList(sbInfo);

            return sbInfo.ToString();
        }


        /// <summary>
        /// Получить массив индексов параметров КП, привязанных к входным каналам, текущие данные которых изменились
        /// </summary>
        /// <returns>Массив индексов параметров КП</returns>
        public List<int> GetModifiedParamIndexes()
        {
            List<int> indexes = new List<int>();

            try
            {
                if (KPParams != null)
                {
                    for (int i = 0; i < KPParams.Length; i++)
                    {
                        Param param = KPParams[i];
                        if (param != null && param.CnlNum > 0 && CurModified[i])
                            indexes.Add(i);
                    }
                }
            }
            catch (Exception ex)
            {
                if (WriteToLog != null)
                    WriteToLog((Localization.UseRussian ? 
                        "Ошибка при получении индексов изменившихся тегов КП: " : 
                        "Error getting changed device tag indexes: ") + ex.Message);
            }

            return indexes;
        }
        
        /// <summary>
        /// Форсировано передать архивы срезов и событий данного КП
        /// </summary>
        public bool FlushKPArc()
        {
            return FlushArc(this);
        }

        /// <summary>
        /// Копировать архивы срезов и события в буферы для последующей записи в файл информации о работе КП
        /// </summary>
        public void CopyArcToBuf()
        {
            try
            {
                // копирование архивных срезов
                srezBufList.AddRange(SrezList);
                if (srezBufList.Count > SrezBufSize)
                    srezBufList.RemoveRange(0, srezBufList.Count - SrezBufSize);

                // копирование событий
                eventBufList.AddRange(EventList);
                if (eventBufList.Count > EventBufSize)
                    eventBufList.RemoveRange(0, eventBufList.Count - EventBufSize);
            }
            catch (Exception ex)
            {
                if (WriteToLog != null)
                    WriteToLog((Localization.UseRussian ? 
                        "Ошибка при копировании архивных данных и событий в буферы: " :
                        "Error copying archive data and events to buffers: ") + ex.Message);
            }
        }

        /// <summary>
        /// Копировать архивный срез в буфер для последующей записи в файл информации о работе КП
        /// </summary>
        /// <param name="srezIndex">Индекс копируемого архивного среза</param>
        public void CopySrezToBuf(int srezIndex)
        {
            try
            {
                srezBufList.Add(SrezList[srezIndex]);
                if (srezBufList.Count > SrezBufSize)
                    srezBufList.RemoveAt(0);
            }
            catch (Exception ex)
            {
                if (WriteToLog != null)
                    WriteToLog((Localization.UseRussian ? 
                        "Ошибка при копировании архивного среза в буфер: " :
                        "Error copying archive data to buffer: ") + ex.Message);
            }
        }

        /// <summary>
        /// Копировать событие в буфер для последующей записи в файл информации о работе КП
        /// </summary>
        /// <param name="eventIndex">Индекс копируемого события</param>
        public void CopyEventToBuf(int eventIndex)
        {
            try
            {
                eventBufList.Add(EventList[eventIndex]);
                if (eventBufList.Count > EventBufSize)
                    eventBufList.RemoveAt(0);
            }
            catch (Exception ex)
            {
                if (WriteToLog != null)
                    WriteToLog((Localization.UseRussian ? 
                        "Ошибка при копировании события в буфер: " :
                        "Error copying event to buffer: ") + ex.Message);
            }
        }
        #endregion
    }
}