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
 * Summary  : The base class for device communication logic
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2006
 * Modified : 2019
 */

using Scada.Comm.Channels;
using Scada.Data.Models;
using Scada.Data.Tables;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Reflection;
using System.Text;
using System.Threading;
using Utils;

namespace Scada.Comm.Devices
{
    /// <summary>
    /// The base class for device communication logic.
    /// <para>Родительский класс логики взаимодействия с КП.</para>
    /// </summary>
    public abstract partial class KPLogic
    {
        /// <summary>
        /// Размер списка последних архивных срезов КП
        /// </summary>
        protected const int LastSrezListSize = 10;
        /// <summary>
        /// Размер списка последних событий КП
        /// </summary>
        protected const int LastEventListSize = 10;
        /// <summary>
        /// Размер списка последних команд ТУ
        /// </summary>
        protected const int LastCmdListSize = 10;

        private Connection conn;                  // соединение с физическим КП
        private AppDirs appDirs;                  // директории приложения
        private Log.WriteLineDelegate writeToLog; // метод записи в журнал линии связи
        private volatile bool terminated;         // признак завершения работы линии связи
        private string caption;                   // обозначение КП
        private string sessText;                  // текст для вывода в журнал в начале сеанса опроса
        private string sendCmdText;               // текст для вывода в журнал при отправке команды ТУ
        private string[][] tagTable;              // таблица текущих значений тегов КП
        private int[] tagTableColLen;             // ширина столбцов таблицы значений тегов

        /// <summary>
        /// Текущие данные тегов КП
        /// </summary>
        protected SrezTableLight.CnlData[] curData;
        /// <summary>
        /// Признаки изменения текущих данных с момента передачи
        /// </summary>
        protected bool[] curDataModified;
        /// <summary>
        /// All device tags with access by signals.
        /// </summary>
        protected Dictionary<int, KPTag> tagsBySignal;
        /// <summary>
        /// Список не переданных архивных срезов КП
        /// </summary>
        protected List<TagSrez> arcSrezList;
        /// <summary>
        /// Список не переданных событий КП
        /// </summary>
        protected List<KPEvent> eventList;
        /// <summary>
        /// Список последних архивных срезов КП
        /// </summary>
        protected List<TagSrez> lastArcSrezList;
        /// <summary>
        /// Список последних событий КП
        /// </summary>
        protected List<KPEvent> lastEventList;
        /// <summary>
        /// Список последних команд КП
        /// </summary>
        protected List<Command> lastCmdList;
        /// <summary>
        /// Признак, что последний сеанс связи завершён успешно
        /// </summary>
        protected bool lastCommSucc;
        /// <summary>
        /// Статистика работы КП
        /// </summary>
        protected KPStats kpStats;


        /// <summary>
        /// Конструктор, ограничивающий создание объекта без параметров
        /// </summary>
        protected KPLogic()
            : this(0)
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public KPLogic(int number)
        {
            // private fields
            conn = null;
            appDirs = new AppDirs();
            writeToLog = text => { }; // заглушка
            terminated = false;
            caption = "";
            sessText = "";
            sendCmdText = "";
            tagTable = null;
            tagTableColLen = null;

            // protected fields
            curData = new SrezTableLight.CnlData[0];
            curDataModified = new bool[0];
            tagsBySignal = new Dictionary<int, KPTag>();
            arcSrezList = new List<TagSrez>();
            eventList = new List<KPEvent>();
            lastArcSrezList = new List<TagSrez>();
            lastEventList = new List<KPEvent>();
            lastCmdList = new List<Command>();
            lastCommSucc = false;
            kpStats.Reset();

            // public properties
            Bound = false;
            Number = number;
            Name = "";
            Dll = GetType().Assembly.GetName().Name;
            Address = 0;
            CallNum = "";
            ReqParams = KPReqParams.Default;
            ReqTriesCnt = 1;
            SerialPort = null;
            CustomParams = null;
            CommonProps = null;
            CommLineSvc = null;

            CanSendCmd = false;
            ConnRequired = true;
            KPTags = new KPTag[0];
            TagGroups = new TagGroup[0];
            WorkState = WorkStates.Undefined;
            LastSessDT = DateTime.MinValue;
            LastCmdDT = DateTime.MinValue;
        }


        /// <summary>
        /// Gets or sets a value indicating whether the device is bound to Server.
        /// </summary>
        public bool Bound { get; set; }

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
                    caption = (Localization.UseRussian ? "КП " : "Device ") + Number + 
                        (string.IsNullOrEmpty(Name) ? "" : " \"" + Name + "\"");
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
        public KPReqParams ReqParams { get; set; }

        /// <summary>
        /// Получить или установить количество попыток перезапроса КП при ошибке
        /// </summary>
        public int ReqTriesCnt { get; set; }

        /// <summary>
        /// Получить или установить соединение с физическим КП
        /// </summary>
        public Connection Connection
        {
            get
            {
                return conn;
            }
            set
            {
                conn = value;
                ExecOnConnectionSet();
            }
        }

        /// <summary>
        /// Получить или установить ссылку на последовательный порт
        /// </summary>
        [Obsolete("Use Connection property")]
        public SerialPort SerialPort { get; set; }

        /// <summary>
        /// Получить или установить ссылку на пользовательские параметры линии связи
        /// </summary>
        public SortedList<string, string> CustomParams { get; set; }

        /// <summary>
        /// Получить или установить ссылку на общие свойства линии связи
        /// </summary>
        /// <remarks>Объект не является потокобезопасным</remarks>
        public SortedList<string, object> CommonProps { get; set; }

        /// <summary>
        /// Получить или установить директории приложения
        /// </summary>
        public AppDirs AppDirs
        {
            get
            {
                return appDirs;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                appDirs = value;
            }
        }

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
        /// Получить или установить сервис линии связи
        /// </summary>
        public ICommLineService CommLineSvc { get; set; }

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
        /// Получить требование наличия соединения для выполнения сеанса опроса КП и отправки команды ТУ
        /// </summary>
        public bool ConnRequired { get; protected set; }

        /// <summary>
        /// Получить все теги КП без разбивки на группы
        /// </summary>
        public KPTag[] KPTags { get; protected set; }

        /// <summary>
        /// Получить группы тегов КП
        /// </summary>
        public TagGroup[] TagGroups { get; protected set; }

        /// <summary>
        /// Получить или установить состояние работы КП
        /// </summary>
        /// <remarks>Установка состояния работы извне объекта требуется 
        /// при возникновении исключений в переопределяемых методах</remarks>
        public WorkStates WorkState { get; set; }

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
        /// Получить дату и время последнего сеанса связи
        /// </summary>
        public DateTime LastSessDT { get; protected set; }

        /// <summary>
        /// Получить дату и время последней отправки команды ТУ
        /// </summary>
        public DateTime LastCmdDT { get; protected set; }

        /// <summary>
        /// Получить статистику работы КП
        /// </summary>
        public KPStats Stats
        {
            get
            {
                return kpStats;
            }
        }


        /// <summary>
        /// Выполнить метод OnConnectionSet с обработкой исключений
        /// </summary>
        private void ExecOnConnectionSet()
        {
            try
            {
                OnConnectionSet();
            }
            catch (Exception ex)
            {
                WriteToLog(string.Format(Localization.UseRussian ?
                    "Ошибка при выполнении действий после установки соединения: " :
                    "Error executing actions after connection: ") + ex.Message);
            }
        }

        /// <summary>
        /// Добавить в конструктор строки описание КП
        /// </summary>
        private void AppendKPDescr(StringBuilder sb)
        {
            if (Localization.UseRussian)
            {
                sb.Append(Caption).Append(", тип: ").Append(Dll);
                if (Address > 0)
                    sb.Append(", адрес: ").Append(Address);
                if (!string.IsNullOrEmpty(CallNum))
                    sb.Append(", позывной: ").Append(CallNum);
            }
            else
            {
                sb.Append(Caption).Append(", type: ").Append(Dll);
                if (Address > 0)
                    sb.Append(", address: ").Append(Address);
                if (!string.IsNullOrEmpty(CallNum))
                    sb.Append(", call number: ").Append(CallNum);
            }
        }

        /// <summary>
        /// Получить текст для вывода в журнал при обработке входящего запроса
        /// </summary>
        private string GetProcReqText(Connection conn, KPLogic targetKP)
        {
            StringBuilder sb = new StringBuilder(DateTime.Now.ToString(CommUtils.CommLineDTFormat));

            if (Localization.UseRussian)
            {
                sb.Append(" Обработка входящего запроса");
                if (targetKP != null)
                {
                    sb.Append(" от ");
                    AppendKPDescr(sb);
                }
                if (!string.IsNullOrEmpty(conn.RemoteAddress))
                    sb.Append(", удалённый адрес: ").Append(conn.RemoteAddress);
            }
            else
            {
                sb.Append(" Process incoming request");
                if (targetKP != null)
                {
                    sb.Append(" from the ");
                    AppendKPDescr(sb);
                }
                if (!string.IsNullOrEmpty(conn.RemoteAddress))
                    sb.Append(", remote address: ").Append(conn.RemoteAddress);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Получить максимальную длину строки в массиве
        /// </summary>
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
        /// Добавить в конструктор строки информацию о тегах КП
        /// </summary>
        private void AppendKPTagsInfo(StringBuilder sb)
        {
            sb.AppendLine();

            if (KPTags.Length == 0)
            {
                sb.AppendLine(Localization.UseRussian ? "Теги КП отсутствуют" : "No device tags");
            }
            else
            {
                sb.AppendLine(Localization.UseRussian ? "Текущие данные тегов КП" : "Current device tags data");

                bool writeGroups = TagGroups.Length > 0; // вывод тегов КП по группам
                string[] colSig;  // столбец сигналов
                string[] colName; // столбец наименований
                string[] colVal;  // столбец значений
                string[] colCnl;  // столбец каналов
                int rowCnt;       // количество строк

                if (tagTable == null)
                {
                    // формирование таблицы текущих значений тегов КП
                    tagTable = new string[4][];
                    tagTableColLen = new int[4];
                    rowCnt = writeGroups ? KPTags.Length + TagGroups.Length + 1 : KPTags.Length + 1;

                    tagTable[0] = colSig = new string[rowCnt];
                    tagTable[1] = colName = new string[rowCnt];
                    tagTable[2] = colVal = new string[rowCnt];
                    tagTable[3] = colCnl = new string[rowCnt];

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
                        foreach (TagGroup tagGroup in TagGroups)
                        {
                            colSig[i] = "*";
                            colName[i] = tagGroup.Name;
                            colVal[i] = "";
                            colCnl[i] = "";
                            i++;

                            foreach (KPTag kpTag in tagGroup.KPTags)
                            {
                                colSig[i] = kpTag.Signal.ToString();
                                colCnl[i] = kpTag.CnlNum <= 0 ? "" : kpTag.CnlNum.ToString();
                                i++;
                            }
                        }
                    }
                    else
                    {
                        for (int i = 1; i < rowCnt; i++)
                        {
                            KPTag kpTag = KPTags[i - 1];
                            colSig[i] = kpTag.Signal.ToString();
                            colCnl[i] = kpTag.CnlNum <= 0 ? "" : kpTag.CnlNum.ToString();
                        }
                    }

                    // вычисление ширины столбцов
                    tagTableColLen[0] = GetMaxLength(colSig);
                    tagTableColLen[3] = GetMaxLength(colCnl);
                }
                else
                {
                    colSig = tagTable[0];
                    colName = tagTable[1];
                    colVal = tagTable[2];
                    colCnl = tagTable[3];
                    rowCnt = colSig.Length;
                }

                // заполнение столбцов наименований и значений, вычисление их ширины
                for (int i = 1, paramInd = 0; i < rowCnt; i++)
                {
                    if (colSig[i] != "*")
                    {
                        KPTag kpTag = KPTags[paramInd];
                        colName[i] = kpTag.Name;
                        colVal[i] = ConvertTagDataToStr(kpTag, curData[paramInd]);
                        paramInd++;
                    }
                }

                tagTableColLen[1] = GetMaxLength(colName);
                tagTableColLen[2] = GetMaxLength(colVal);

                // добавление текстового представления таблицы в конструктор строки
                int lenSig = tagTableColLen[0];
                int lenName = tagTableColLen[1];
                int lenVal = tagTableColLen[2];
                int lenCnl = tagTableColLen[3];
                string br = (new StringBuilder()).Append("+-")
                    .Append(new string('-', lenSig)).Append("-+-")
                    .Append(new string('-', lenName)).Append("-+-")
                    .Append(new string('-', lenVal)).Append("-+-")
                    .Append(new string('-', lenCnl)).Append("-+").ToString();

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
                        sb.Append("| ")
                            .Append(colSig[i].PadLeft(lenSig)).Append(" | ")
                            .Append(colName[i].PadRight(lenName)).Append(" | ")
                            .Append(colVal[i].PadLeft(lenVal)).Append(" | ")
                            .Append(colCnl[i].PadLeft(lenCnl)).Append(" |").AppendLine();
                    }
                    sb.AppendLine(br);
                }
            }
        }

        /// <summary>
        /// Добавить в конструктор строки информацию о последних архивных срезах КП
        /// </summary>
        private void AppendLastArcSrezInfo(StringBuilder sb)
        {
            sb.AppendLine();

            if (lastArcSrezList.Count == 0)
            {
                sb.AppendLine(Localization.UseRussian ? "Архивные данные отсутствуют" : "No archive data");
            }
            else
            {
                // заполнение массивов значений ячеек таблицы
                sb.AppendLine(string.Format(Localization.UseRussian ?
                    "Архивные данные (последние {0} срезов)" : 
                    "Archive data (recent {0} snapshots)", LastSrezListSize));

                int rowCnt = lastArcSrezList.Count + 1;
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
                foreach (TagSrez tagSrez in lastArcSrezList)
                {
                    numArr[rowNum] = rowNum.ToString();
                    timeArr[rowNum] = tagSrez.DateTime.ToLocalizedString();
                    descrArr[rowNum] = tagSrez.Descr;
                    rowNum++;
                }

                // добавление таблицы в конструктор строки
                AppendTable(sb, new bool[] { false, true, true }, numArr, timeArr, descrArr);
            }
        }

        /// <summary>
        /// Добавить в конструктор строки информацию о последних событях КП
        /// </summary>
        private void AppendLastEventsInfo(StringBuilder sb)
        {
            sb.AppendLine();

            if (lastEventList.Count == 0)
            {
                sb.AppendLine(Localization.UseRussian ? "События отсутствуют" : "No events");
            }
            else
            {
                // заполнение массивов значений ячеек таблицы
                sb.AppendLine(string.Format(Localization.UseRussian ?
                    "События (последние {0})" : "Events (recent {0})", LastEventListSize));

                int rowCnt = lastEventList.Count + 1;
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
                foreach (KPEvent kpEvent in lastEventList)
                {
                    numArr[rowNum] = rowNum.ToString();
                    timeArr[rowNum] = kpEvent.DateTime.ToLocalizedString();
                    sigArr[rowNum] = kpEvent.KPTag == null || kpEvent.KPTag.Signal <= 0 ? 
                        "" : kpEvent.KPTag.Signal.ToString();
                    descrArr[rowNum] = kpEvent.Descr.Replace("\n", "\\").Replace("\r", "");
                    rowNum++;
                }

                // добавление таблицы в конструктор строки
                AppendTable(sb, new bool[] { false, true, false, true }, numArr, timeArr, sigArr, descrArr);
            }
        }

        /// <summary>
        /// Добавить в конструктор строки информацию о последних командах КП
        /// </summary>
        private void AppendLastCmdsInfo(StringBuilder sb)
        {
            sb.AppendLine();

            if (lastCmdList.Count == 0)
            {
                sb.AppendLine(Localization.UseRussian ? "Команды ТУ отсутствуют" : "No commands");
            }
            else
            {
                // заполнение массивов значений ячеек таблицы
                sb.AppendLine(string.Format(Localization.UseRussian ?
                    "Команды ТУ (последние {0} команд)" :
                    "Commands (recent {0} commands)", LastSrezListSize));

                int rowCnt = lastCmdList.Count + 1;
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
                foreach (Command cmd in lastCmdList)
                {
                    numArr[rowNum] = rowNum.ToString();
                    timeArr[rowNum] = cmd.CreateDT.ToLocalizedString();
                    descrArr[rowNum] = cmd.GetCmdDescr();
                    rowNum++;
                }

                // добавление таблицы в конструктор строки
                AppendTable(sb, new bool[] { false, true, true }, numArr, timeArr, descrArr );
            }
        }

        /// <summary>
        /// Добавить таблицу в конструктор строки
        /// </summary>
        private void AppendTable(StringBuilder sb, bool[] leftAlign, params string[][] columns)
        {
            int colCnt = columns.Length;
            int lastColInd = colCnt - 1;
            int[] colWidthArr = new int[colCnt];

            // формирование разделителя строк
            StringBuilder sbBreak = new StringBuilder();
            sbBreak.Append("+-");

            for (int colInd = 0; colInd < colCnt; colInd++)
            {
                int colWidth = GetMaxLength(columns[colInd]);
                colWidthArr[colInd] = colWidth;
                sbBreak
                    .Append(new string('-', colWidth))
                    .Append(colInd < lastColInd ? "-+-" : "-+");
            }

            string br = sbBreak.ToString();

            // добавление строк таблицы в конструктор строки
            int rowCnt = columns[0].Length;
            sb.AppendLine(br);

            for (int rowInd = 0; rowInd < rowCnt; rowInd++)
            {
                sb.Append("| ");
                for (int colInd = 0; colInd < colCnt; colInd++)
                {
                    string cell = columns[colInd][rowInd];
                    int colWidth = colWidthArr[colInd];
                    cell = rowInd == 0 || leftAlign[colInd] ? cell.PadRight(colWidth) : cell.PadLeft(colWidth);
                    sb.Append(cell).Append(colInd < lastColInd ? " | " : " |");
                }
                sb.AppendLine().AppendLine(br);
            }
        }


        /// <summary>
        /// Преобразовать данные тега КП в строку
        /// </summary>
        protected virtual string ConvertTagDataToStr(int signal, SrezTableLight.CnlData tagData)
        {
            return tagData.Stat > 0 ? tagData.Val.ToString("N3", Localization.Culture) : "---";
        }

        /// <summary>
        /// Converts the tag data to string.
        /// </summary>
        protected virtual string ConvertTagDataToStr(KPTag kpTag, SrezTableLight.CnlData tagData)
        {
            return ConvertTagDataToStr(kpTag.Signal, tagData);
        }

        /// <summary>
        /// Инициализировать группы тегов, теги КП, их текущие данные и признаки изменения
        /// </summary>
        /// <remarks>В результате работы метода элементы списков TagGroups и KPTags не могут быть null</remarks>
        protected void InitKPTags(List<TagGroup> srcTagGroups)
        {
            if (srcTagGroups == null)
                throw new ArgumentNullException("srcTagGroups");

            // подсчёт количества тегов
            int tagCnt = 0;
            foreach (TagGroup tagGroup in srcTagGroups)
            {
                if (tagGroup != null)
                    tagCnt += tagGroup.KPTags.Count;
            }

            // инициализация данных
            TagGroups = srcTagGroups.ToArray();
            KPTags = new KPTag[tagCnt];
            curData = new SrezTableLight.CnlData[tagCnt];
            curDataModified = new bool[tagCnt];
            tagsBySignal.Clear();
            tagTable = null;

            int groupCnt = TagGroups.Length;
            int tagIndex = 0;
            for (int groupIndex = 0; groupIndex < groupCnt; groupIndex++)
            {
                TagGroup tagGroup = TagGroups[groupIndex];
                if (tagGroup == null)
                {
                    TagGroups[groupIndex] = new TagGroup();
                }
                else
                {
                    foreach (KPTag kpTag in tagGroup.KPTags)
                    {
                        KPTag destTag = kpTag ?? new KPTag();
                        destTag.Index = tagIndex;
                        KPTags[tagIndex] = destTag;
                        tagIndex++;

                        if (destTag.Signal > 0)
                            tagsBySignal[destTag.Signal] = destTag;
                    }
                }
            }

            for (int i = 0; i < tagCnt; i++)
            {
                curData[i] = SrezTableLight.CnlData.Empty;
                curDataModified[i] = true;
            }
        }

        /// <summary>
        /// Инициализировать теги КП, их текущие данные и признаки изменения
        /// </summary>
        /// <remarks>В результате работы метода элементы списка KPTags не могут быть null</remarks>
        protected void InitKPTags(List<KPTag> srcKPTags)
        {
            if (srcKPTags == null)
                throw new ArgumentNullException("srcKPTags");

            TagGroups = new TagGroup[0];
            int tagCnt = srcKPTags.Count;
            KPTags = new KPTag[tagCnt];
            curData = new SrezTableLight.CnlData[tagCnt];
            curDataModified = new bool[tagCnt];
            tagsBySignal.Clear();
            tagTable = null;

            int tagIndex = 0;
            foreach (KPTag srcTag in srcKPTags)
            {
                KPTag destTag = srcTag ?? new KPTag();
                destTag.Index = tagIndex;
                KPTags[tagIndex] = destTag;
                tagIndex++;

                if (destTag.Signal > 0)
                    tagsBySignal[destTag.Signal] = destTag;
            }

            for (int i = 0; i < tagCnt; i++)
            {
                curData[i] = SrezTableLight.CnlData.Empty;
                curDataModified[i] = true;
            }
        }

        /// <summary>
        /// Потокобезопасно установить текущие данные тега КП и признак их изменения
        /// </summary>
        protected void SetCurData(int tagIndex, double newVal, int newStat)
        {
            SetCurData(tagIndex, new SrezTableLight.CnlData(newVal, newStat));
        }

        /// <summary>
        /// Потокобезопасно установить текущие данные тега КП и признак их изменения
        /// </summary>
        protected void SetCurData(int tagIndex, SrezTableLight.CnlData newData)
        {
            lock (curData)
            {
                if (0 <= tagIndex && tagIndex < curData.Length)
                {
                    SrezTableLight.CnlData curTagData = curData[tagIndex];
                    curDataModified[tagIndex] |= curTagData.Val != newData.Val || curTagData.Stat != newData.Stat;
                    curData[tagIndex] = newData;
                }
            }
        }

        /// <summary>
        /// Потокобезопасно увеличить текущее значение тега КП без изменения статуса
        /// </summary>
        protected void IncCurData(int tagIndex, double number)
        {
            lock (curData)
            {
                if (0 <= tagIndex && tagIndex < curData.Length)
                {
                    SrezTableLight.CnlData curTagData = curData[tagIndex];
                    double newVal = curTagData.Val + number;
                    curDataModified[tagIndex] |= number != 0;
                    curData[tagIndex] = new SrezTableLight.CnlData(newVal, curTagData.Stat);
                }
            }
        }

        /// <summary>
        /// Потокобезопасно установить текущие данные как недостоверные
        /// </summary>
        protected void InvalidateCurData(int tagIndex, int tagCount)
        {
            lock (curData)
            {
                SrezTableLight.CnlData newData = SrezTableLight.CnlData.Empty;
                for (int i = 0, len = curData.Length; i < tagCount && tagIndex < len; i++)
                {
                    if (tagIndex >= 0)
                    {
                        SrezTableLight.CnlData curTagData = curData[tagIndex];
                        curDataModified[tagIndex] |= curTagData.Val != newData.Val || curTagData.Stat != newData.Stat;
                        curData[tagIndex] = newData;
                    }
                    tagIndex++;
                }
            }
        }

        /// <summary>
        /// Потокобезопасно добавить архивный срез в список срезов КП
        /// </summary>
        protected void AddArcSrez(TagSrez tagSrez)
        {
            lock (arcSrezList)
            {
                // добавление среза в список не переданных срезов
                arcSrezList.Add(tagSrez);
                
                // добавление среза в список последних срезов
                lastArcSrezList.Add(tagSrez);
                while (lastArcSrezList.Count > LastSrezListSize)
                {
                    lastArcSrezList.RemoveAt(0);
                }
            }
        }

        /// <summary>
        /// Потокобезопасно добавить событие в список событий КП
        /// </summary>
        protected void AddEvent(KPEvent kpEvent)
        {
            lock (eventList)
            {
                // добавление события в список не переданных событий
                eventList.Add(kpEvent);

                // добавление события в список последних событий
                lastEventList.Add(kpEvent);
                while (lastEventList.Count > LastSrezListSize)
                {
                    lastEventList.RemoveAt(0);
                }
            }
        }

        /// <summary>
        /// Проверить необходимость выполнения запроса
        /// </summary>
        protected bool RequestNeeded(ref int tryNum)
        {
            return !lastCommSucc && tryNum < ReqTriesCnt && !Terminated;
        }

        /// <summary>
        /// Выполнить действия, необходимые для завершения запроса
        /// </summary>
        protected void FinishRequest()
        {
            Thread.Sleep(ReqParams.Delay);
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
                WorkState = WorkStates.Normal;
            }
            else
            {
                kpStats.SessErrCnt++;
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
                WorkState = WorkStates.Normal;
            }
            else
            {
                kpStats.CmdErrCnt++;
                WorkState = WorkStates.Error;
            }
        }


        /// <summary>
        /// Выполнить сеанс опроса КП
        /// </summary>
        /// <remarks>Метод выполняется в потоке линии связи циклически или по времени</remarks>
        public virtual void Session()
        {
            LastSessDT = DateTime.Now;
            lastCommSucc = true;

            if (sessText == "")
            {
                StringBuilder sb = new StringBuilder(Localization.UseRussian ?
                    " Сеанс связи с " : " Communication session with the ");
                AppendKPDescr(sb);
                sessText = sb.ToString();
            }

            WriteToLog("");
            WriteToLog(LastSessDT.ToString(CommUtils.CommLineDTFormat) + sessText);
        }

        /// <summary>
        /// Отправить команду ТУ
        /// </summary>
        /// <remarks>Метод выполняется в потоке линии связи при появлении команды ТУ</remarks>
        public virtual void SendCmd(Command cmd)
        {
            LastCmdDT = DateTime.Now;
            lastCommSucc = CanSendCmd;

            if (sendCmdText == "")
            {
                StringBuilder sb = new StringBuilder(Localization.UseRussian ? " Команда " : " Command to the ");
                AppendKPDescr(sb);
                sendCmdText = sb.ToString();
            }

            WriteToLog("");
            WriteToLog(LastCmdDT.ToString(CommUtils.CommLineDTFormat) + sendCmdText);

            if (CanSendCmd)
            {
                // добавление команды в список последних команд
                lastCmdList.Add(cmd);
                while (lastCmdList.Count > LastCmdListSize)
                    lastCmdList.RemoveAt(0);
            }
            else
            {
                WriteToLog(Localization.UseRussian ?
                    "КП не поддерживает отправку команд" :
                    "The device does not support sending commands");
            }
        }
        
        /// <summary>
        /// Обработать уже считанный входящий запрос, относящийся к произвольному КП на линии связи
        /// </summary>
        /// <remarks>Если targetKP равен null, значит метод должен вернуть КП, которому адресован запрос. 
        /// Возвращает true, если запрос успешно разобран.
        /// Метод выполняется в потоке канала связи</remarks>
        public virtual bool ProcIncomingReq(byte[] buffer, int offset, int count, ref KPLogic targetKP)
        {
            WriteToLog("");
            WriteToLog(GetProcReqText(Connection, targetKP));
            WriteToLog(Connection.BuildReadLogText(buffer, offset, count, Connection.DefaultLogFormat));
            return false;
        }

        /// <summary>
        /// Обработать не считанный входящий запрос, относящийся к произвольному КП на линии связи
        /// </summary>
        /// <remarks>Если targetKP равен null, значит метод должен вернуть КП, которому адресован запрос. 
        /// Возвращает true, если запрос успешно считан и разобран.
        /// Метод выполняется в потоке канала связи</remarks>
        public virtual bool ProcUnreadIncomingReq(Connection conn, ref KPLogic targetKP)
        {
            WriteToLog("");
            WriteToLog(GetProcReqText(conn, targetKP));
            return false;
        }


        /// <summary>
        /// Выполнить действия после добавления КП на линию связи
        /// </summary>
        /// <remarks>Метод вызывается после установки необходимых для работы КП свойств, 
        /// перед привязкой тегов КП к входным каналам</remarks>
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
        /// Выполнить действия после установки соединения
        /// </summary>
        public virtual void OnConnectionSet()
        {
        }

        /// <summary>
        /// Binds the device to the configuration database.
        /// </summary>
        public virtual void Bind(ConfigBaseSubset configBase)
        {
        }

        /// <summary>
        /// Привязать тег КП к входному каналу базы конфигурации
        /// </summary>
        public virtual void BindTag(int signal, int cnlNum, int objNum, int paramID)
        {
            if (cnlNum > 0)
            {
                KPTag tagToBind = null;

                if (signal > 0)
                {
                    // поиск тега КП по сигналу при их последовательной нумерации
                    if (signal <= KPTags.Length)
                    {
                        KPTag kpTag = KPTags[signal - 1];
                        if (kpTag.Signal == signal)
                            tagToBind = kpTag;
                    }

                    // поиск тега КП по сигналу при их произвольной нумерации
                    if (tagToBind == null)
                        tagsBySignal.TryGetValue(signal, out tagToBind);
                }

                // привязка тега КП
                if (tagToBind != null)
                {
                    tagToBind.CnlNum = cnlNum;
                    tagToBind.ObjNum = objNum;
                    tagToBind.ParamID = paramID;
                }
            }
        }

        /// <summary>
        /// Проверить поддержку режима работы канала связи
        /// </summary>
        public virtual bool CheckBehaviorSupport(CommChannelLogic.OperatingBehaviors behavior)
        {
            return behavior == CommChannelLogic.OperatingBehaviors.Master;
        }

        /// <summary>
        /// Установить текущие данные как недостоверные
        /// </summary>
        /// <remarks>Метод вызывается при обрыве соединения, если ConnRequired равно true</remarks>
        public virtual void InvalidateCurData()
        {
            lock (curData)
            {
                SrezTableLight.CnlData newData = SrezTableLight.CnlData.Empty;
                int tagCnt = curData.Length;
                for (int tagInd = 0; tagInd < tagCnt; tagInd++)
                {
                    SrezTableLight.CnlData curTagData = curData[tagInd];
                    curDataModified[tagInd] |= curTagData.Val != newData.Val || curTagData.Stat != newData.Stat;
                    curData[tagInd] = newData;
                }
            }

            WorkState = WorkStates.Error;
        }

        /// <summary>
        /// Получить информацию о работе КП
        /// </summary>
        public virtual string GetInfo()
        {
            StringBuilder sbInfo = new StringBuilder();
            sbInfo.AppendLine(Caption).AppendLine(new string('-', Caption.Length));

            if (Localization.UseRussian)
            {
                sbInfo
                    .Append("DLL         : ").AppendLine(Dll)
                    .Append("Состояние   : ").AppendLine(WorkStateStr)
                    .Append("Сеанс связи : ")
                    .AppendLine(LastSessDT > DateTime.MinValue ? 
                        LastSessDT.ToLocalizedString() : "время неопределено")
                    .Append("Команда ТУ  : ")
                    .AppendLine(LastCmdDT > DateTime.MinValue ? 
                        LastCmdDT.ToLocalizedString() : "время неопределено")
                    .AppendLine()
                    .Append("Сеансы связи (всего / ошибок) : ")
                    .Append(kpStats.SessCnt).Append(" / ").Append(kpStats.SessErrCnt).AppendLine()
                    .Append("Команды ТУ   (всего / ошибок) : ")
                    .Append(kpStats.CmdCnt).Append(" / ").Append(kpStats.CmdErrCnt).AppendLine()
                    .Append("Запросы      (всего / ошибок) : ")
                    .Append(kpStats.ReqCnt).Append(" / ").Append(kpStats.ReqErrCnt).AppendLine();
            }
            else
            {
                sbInfo
                    .Append("DLL           : ").AppendLine(Dll)
                    .Append("State         : ").AppendLine(WorkStateStr)
                    .Append("Comm. session : ")
                    .AppendLine(LastSessDT > DateTime.MinValue ?
                        LastSessDT.ToLocalizedString() : "time is undefined")
                    .Append("Command       : ")
                    .AppendLine(LastCmdDT > DateTime.MinValue ?
                        LastCmdDT.ToLocalizedString() : "time is undefined")
                    .AppendLine()
                    .Append("Comm. sessions (total / errors) : ").
                        Append(kpStats.SessCnt).Append(" / ").Append(kpStats.SessErrCnt).AppendLine()
                    .Append("Commands       (total / errors) : ")
                    .Append(kpStats.CmdCnt).Append(" / ").Append(kpStats.CmdErrCnt).AppendLine()
                    .Append("Requests       (total / errors) : ")
                    .Append(kpStats.ReqCnt).Append(" / ").Append(kpStats.ReqErrCnt).AppendLine();
            }

            AppendKPTagsInfo(sbInfo);
            AppendLastArcSrezInfo(sbInfo);
            AppendLastEventsInfo(sbInfo);
            if (CanSendCmd)
                AppendLastCmdsInfo(sbInfo);

            return sbInfo.ToString();
        }


        /// <summary>
        /// Gets the current data of the device and clears the modification flags.
        /// </summary>
        public virtual TagSrez GetCurData(bool allTags)
        {
            lock (curData)
            {
                TagSrez curSrez = null;
                int tagCnt = curData.Length;

                if (tagCnt > 0)
                {
                    if (allTags)
                    {
                        curSrez = new TagSrez(tagCnt);
                        for (int i = 0; i < tagCnt; i++)
                        {
                            curSrez.KPTags[i] = KPTags[i];
                            curSrez.TagData[i] = curData[i];
                        }
                    }
                    else
                    {
                        int modTagCnt = 0; // number of modified tags
                        for (int i = 0; i < tagCnt; i++)
                        {
                            if (curDataModified[i])
                                modTagCnt++;
                        }

                        if (modTagCnt > 0)
                        {
                            curSrez = new TagSrez(modTagCnt);
                            for (int i = 0, j = 0; i < tagCnt; i++)
                            {
                                if (curDataModified[i])
                                {
                                    curSrez.KPTags[j] = KPTags[i];
                                    curSrez.TagData[j] = curData[i];
                                    j++;
                                }
                            }
                        }
                    }

                    Array.Clear(curDataModified, 0, tagCnt);
                }

                return curSrez;
            }
        }

        /// <summary>
        /// Moves existing archives from the internal list to the destination.
        /// </summary>
        public void MoveArcData(List<TagSrez> destSrezList)
        {
            if (destSrezList == null)
                throw new ArgumentNullException("destSrezList");

            lock (arcSrezList)
            {
                destSrezList.AddRange(arcSrezList);
                arcSrezList.Clear();
            }
        }

        /// <summary>
        /// Moves existing events from the internal list to the destination.
        /// </summary>
        public void MoveEvents(List<KPEvent> destEventList)
        {
            if (destEventList == null)
                throw new ArgumentNullException("destEventList");

            lock (eventList)
            {
                destEventList.AddRange(eventList);
                eventList.Clear();
            }
        }
    }
}
