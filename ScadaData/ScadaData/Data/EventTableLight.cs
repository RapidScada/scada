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
 * Module   : ScadaData
 * Summary  : Event table for fast read data access
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2007
 * Modified : 2012
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Scada.Data
{
    /// <summary>
    /// Event table for fast read data access
    /// <para>Таблица событий для быстрого доступа к данным на чтение</para>
    /// </summary>
    public class EventTableLight
    {
        /// <summary>
        /// Данные события
        /// </summary>
        public class Event
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            public Event()
            {
                Number = 0;
                DateTime = DateTime.MinValue;
                ObjNum = 0;
                KPNum = 0;
                ParamID = 0;
                CnlNum = 0;
                OldCnlVal = 0.0;
                OldCnlStat = 0;
                NewCnlVal = 0.0;
                NewCnlStat = 0;
                Checked = false;
                UserID = 0;
                Descr = "";
                Data = "";
            }

            /// <summary>
            /// Получить или установить порядковый номер события в файле
            /// </summary>
            public int Number { get; set; }
            /// <summary>
            /// Получить или установить временную метку события
            /// </summary>
            public DateTime DateTime { get; set; }
            /// <summary>
            /// Получить или установить номер объекта
            /// </summary>
            public int ObjNum { get; set; }
            /// <summary>
            /// Получить или установить номер КП
            /// </summary>
            public int KPNum { get; set; }
            /// <summary>
            /// Получить или установить идентификатор параметра
            /// </summary>
            public int ParamID { get; set; }
            /// <summary>
            /// Получить или установить номер входного канала
            /// </summary>
            public int CnlNum { get; set; }
            /// <summary>
            /// Получить или установить предыдущее значение канала
            /// </summary>
            public double OldCnlVal { get; set; }
            /// <summary>
            /// Получить или установить предыдущий статус канала
            /// </summary>
            public int OldCnlStat { get; set; }
            /// <summary>
            /// Получить или установить новое значение канала
            /// </summary>
            public double NewCnlVal { get; set; }
            /// <summary>
            /// Получить или установить новый статус канала
            /// </summary>
            public int NewCnlStat { get; set; }
            /// <summary>
            /// Получить или установить признак, что событие квитировано
            /// </summary>
            public bool Checked { get; set; }
            /// <summary>
            /// Получить или установить идентификатор квитировавшего событие пользователя
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
        /// Фильтры таблицы событий
        /// </summary>
        [FlagsAttribute]
        public enum EventFilters
        {
            /// <summary>
            /// Пустой фильтр
            /// </summary>
            None = 0,
            /// <summary>
            /// Фильтр по объекту
            /// </summary>
            Obj = 1,
            /// <summary>
            /// Фильтр по КП
            /// </summary>
            KP = 2,
            /// <summary>
            /// Фильтр по параметру
            /// </summary>
            Param = 4,
            /// <summary>
            /// Фильтр по каналам
            /// </summary>
            Cnls = 8
        }

        /// <summary>
        /// Имя таблицы
        /// </summary>
        protected string tableName;
        /// <summary>
        /// Время последнего изменения файла таблицы
        /// </summary>
        protected DateTime fileModTime;
        /// <summary>
        /// Время последего успешного заполнения таблицы
        /// </summary>
        protected DateTime lastFillTime;

        /// <summary>
        /// Список всех событий
        /// </summary>
        protected List<Event> allEvents;
        /// <summary>
        /// Отфильтрованный список событий
        /// </summary>
        protected List<Event> filteredEvents;
        /// <summary>
        /// Сохранённая для повторного доступа часть отфильтрованного списка событий, 
        /// начиная с определённого номера события
        /// </summary>
        protected List<Event> eventsCache;
        /// <summary>
        /// Сохранённая для повторного доступа конечная часть отфильтрованного списка событий
        /// </summary>
        protected List<Event> lastEventsCache;
        /// <summary>
        /// Начальный номер события, заданный при получении eventsCache
        /// </summary>
        protected int startEvNum;
        /// <summary>
        /// Количество запрошенных событий при получении lastEventsCache
        /// </summary>
        protected int lastEvCnt;

        /// <summary>
        /// Фильтры таблицы событий
        /// </summary>
        protected EventFilters filters;
        /// <summary>
        /// Номер объекта, по которому фильтруется таблица
        /// </summary>
        protected int objNumFilter;
        /// <summary>
        /// Номер КП, по которому фильтруется таблица
        /// </summary>
        protected int kpNumFilter;
        /// <summary>
        /// Номер параметра, по которому фильтруется таблица
        /// </summary>
        protected int paramNumFilter;
        /// <summary>
        /// Упорядоченный список каналов, по которым фильтруется таблица
        /// </summary>
        protected List<int> cnlsFilter;


        /// <summary>
        /// Конструктор
        /// </summary>
        public EventTableLight()
        {
            tableName = "";
            fileModTime = DateTime.MinValue;
            lastFillTime = DateTime.MinValue;

            allEvents = new List<Event>();
            filteredEvents = null;
            eventsCache = null;
            lastEventsCache = null;
            startEvNum = 0;
            lastEvCnt = 0;

            filters = EventFilters.None;
            objNumFilter = 0;
            kpNumFilter = 0;
            paramNumFilter = 0;
            cnlsFilter = null;
        }


        /// <summary>
        /// Получить или установить имя файла таблицы
        /// </summary>
        public string TableName
        {
            get
            {
                return tableName;
            }
            set
            {
                if (tableName != value)
                {
                    tableName = value;
                    fileModTime = DateTime.MinValue;
                    lastFillTime = DateTime.MinValue;
                }
            }
        }

        /// <summary>
        /// Получить или установить время последнего изменения файла таблицы
        /// </summary>
        public DateTime FileModTime
        {
            get
            {
                return fileModTime;
            }
            set
            {
                fileModTime = value;
            }
        }

        /// <summary>
        /// Получить или установить время последего успешного заполнения таблицы
        /// </summary>
        public DateTime LastFillTime
        {
            get
            {
                return lastFillTime;
            }
            set
            {
                fileModTime = value;
            }
        }
        

        /// <summary>
        /// Получить список всех событий
        /// </summary>
        public List<Event> AllEvents
        {
            get
            {
                return allEvents;
            }
        }

        /// <summary>
        /// Получить отфильтрованный список событий
        /// </summary>
        public List<Event> FilteredEvents
        {
            get
            {
                if (filteredEvents == null)
                {
                    // создание и заполнение отфильтрованного списка событий
                    filteredEvents = new List<Event>();
                    foreach (Event ev in allEvents)
                    {
                        if (EventVisible(ev))
                            filteredEvents.Add(ev);
                    }
                }                
                return filteredEvents;
            }
        }

        /// <summary>
        /// Получить или установить фильтры таблицы событий
        /// </summary>
        public EventFilters Filters
        {
            get
            {
                return filters;
            }
            set
            {
                if (filters != value)
                {
                    filters = value;
                    filteredEvents = null;
                    eventsCache = null;
                    lastEventsCache = null;
                }
            }
        }

        /// <summary>
        /// Получить или установить номер объекта, по которому фильтруется таблица
        /// </summary>
        public int ObjNumFilter
        {
            get
            {
                return objNumFilter;
            }
            set
            {
                if (objNumFilter != value)
                {
                    objNumFilter = value;
                    filteredEvents = null;
                    eventsCache = null;
                    lastEventsCache = null;
                }
            }
        }

        /// <summary>
        /// Получить или установить номер КП, по которому фильтруется таблица
        /// </summary>
        public int KPNumFilter
        {
            get
            {
                return kpNumFilter;
            }
            set
            {
                if (kpNumFilter != value)
                {
                    kpNumFilter = value;
                    filteredEvents = null;
                    eventsCache = null;
                    lastEventsCache = null;
                }
            }
        }

        /// <summary>
        /// Получить или установить номер параметра, по которому фильтруется таблица
        /// </summary>
        public int ParamNumFilter
        {
            get
            {
                return paramNumFilter;
            }
            set
            {
                if (paramNumFilter != value)
                {
                    paramNumFilter = value;
                    filteredEvents = null;
                    eventsCache = null;
                    lastEventsCache = null;
                }
            }
        }

        /// <summary>
        /// Получить или установить список каналов, по которому фильтруется таблица
        /// </summary>
        public List<int> CnlsFilter
        {
            get
            {
                return cnlsFilter;
            }
            set
            {
                cnlsFilter = value;
                if (cnlsFilter != null)
                    cnlsFilter.Sort();
                filteredEvents = null;
                eventsCache = null;
                lastEventsCache = null;
            }
        }


        /// <summary>
        /// Проверить, что событие является видимым с установленными фильтрами
        /// </summary>
        protected bool EventVisible(Event ev)
        {
            return !((filters & EventFilters.Obj) > 0 && ev.ObjNum != objNumFilter ||
                (filters & EventFilters.KP) > 0 && ev.KPNum != kpNumFilter ||
                (filters & EventFilters.Param) > 0 && ev.ParamID != paramNumFilter ||
                (filters & EventFilters.Cnls) > 0 && (cnlsFilter == null || cnlsFilter.BinarySearch(ev.CnlNum) < 0));
        }

        /// <summary>
        /// Добавить событие в таблицу
        /// </summary>
        public void AddEvent(Event ev)
        {
            allEvents.Add(ev);
            filteredEvents = null;
            eventsCache = null;
            lastEventsCache = null;
        }

        /// <summary>
        /// Очистить таблицу событий
        /// </summary>
        public void Clear()
        {
            allEvents.Clear();
            filteredEvents = null;
            eventsCache = null;
            lastEventsCache = null;
        }

        /// <summary>
        /// Получить часть отфильтрованного списка событий, начиная с заданного номера события
        /// </summary>
        public List<Event> GetEvents(int startEvNum)
        {
            if (eventsCache == null || this.startEvNum != startEvNum)
            {
                eventsCache = new List<Event>();
                this.startEvNum = startEvNum;

                int ind = startEvNum < 0 ? 0 : startEvNum - 1;
                int cnt = allEvents.Count;

                while (ind < cnt)
                {
                    Event ev = allEvents[ind];
                    if (EventVisible(ev))
                        eventsCache.Add(ev);
                    ind++;
                }
            }

            return eventsCache;
        }

        /// <summary>
        /// Получить конечную часть отфильтрованного списка событий
        /// </summary>
        public List<Event> GetLastEvents(int count)
        {
            if (lastEventsCache == null || lastEvCnt != count)
            {
                lastEventsCache = new List<Event>();
                lastEvCnt = count;

                if (count > 0)
                {
                    if (filteredEvents == null)
                    {
                        int ind = allEvents.Count - 1;
                        int cnt = 0; // количество полученных событий

                        while (ind >= 0 && cnt < count)
                        {
                            Event ev = allEvents[ind];
                            if (EventVisible(ev))
                            {
                                lastEventsCache.Insert(0, ev);
                                cnt++;
                            }
                            ind--;
                        }
                    }
                    else
                    {
                        int cnt = filteredEvents.Count < count ? filteredEvents.Count : count;
                        if (cnt > 0)
                            lastEventsCache.AddRange(filteredEvents.GetRange(filteredEvents.Count - cnt, cnt));
                    }
                }
            }

            return lastEventsCache;
        }
    }
}
