/*
 * Copyright 2020 Mikhail Shiryaev
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
 * Modified : 2020
 */

using System;
using System.Collections.Generic;

namespace Scada.Data.Tables
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
        [Serializable]
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
        /// Фильтры (типы фильтров) событий
        /// </summary>
        [Flags]
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
            /// Фильтр по одному или нескольким параметрам
            /// </summary>
            Param = 4,
            /// <summary>
            /// Фильтр по каналам
            /// </summary>
            Cnls = 8,
            /// <summary>
            /// Фильтр по статусам
            /// </summary>
            Stat = 16,
            /// <summary>
            /// Фильтр по квитированию
            /// </summary>
            Ack = 32
        }

        /// <summary>
        /// Фильтр событий
        /// </summary>
        public class EventFilter
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            public EventFilter()
                : this(EventFilters.None)
            {
            }
            /// <summary>
            /// Конструктор
            /// </summary>
            public EventFilter(EventFilters filters)
            {
                Filters = filters;
                ObjNum = 0;
                KPNum = 0;
                ParamID = 0;
                ParamIDs = null;
                CnlNums = null;
                Statuses = null;
                Checked = false;
            }

            /// <summary>
            /// Получить или установить типы применяемых фильтров
            /// </summary>
            public EventFilters Filters { get; set; }
            /// <summary>
            /// Получить или установить номер объекта для фильтрации
            /// </summary>
            public int ObjNum { get; set; }
            /// <summary>
            /// Получить или установить номер КП для фильтрации
            /// </summary>
            public int KPNum { get; set; }
            /// <summary>
            /// Получить или установить ид. параметра для фильтрации
            /// </summary>
            public int ParamID { get; set; }
            /// <summary>
            /// Получить или установить ид. параметров для фильтрации
            /// </summary>
            public ISet<int> ParamIDs { get; set; }
            /// <summary>
            /// Получить или установить номера входных каналов для фильтрации
            /// </summary>
            public ISet<int> CnlNums { get; set; }
            /// <summary>
            /// Получить или установить статусы для фильтрации
            /// </summary>
            public ISet<int> Statuses { get; set; }
            /// <summary>
            /// Получить или установить признак квитирования для фильтрации
            /// </summary>
            public bool Checked { get; set; }

            /// <summary>
            /// Проверить корректность фильтра
            /// </summary>
            public bool Check(bool throwOnFail = true)
            {
                if (Filters.HasFlag(EventFilters.Cnls) && CnlNums == null)
                {
                    if (throwOnFail)
                        throw new ScadaException("Event filter is incorrect.");
                    else
                        return false;
                }
                else
                {
                    return true;
                }
            }
            /// <summary>
            /// Проверить, что событие удовлетворяет фильтру
            /// </summary>
            public bool Satisfied(Event ev)
            {
                // если используется фильтр только по номерам каналов, CnlNums должно быть не равно null
                if (Filters == EventFilters.Cnls)
                {
                    // быстрая проверка только по номерам каналов
                    return CnlNums.Contains(ev.CnlNum);
                }
                else
                {
                    // полная проверка условий фильтра
                    return
                        (!Filters.HasFlag(EventFilters.Obj) || ObjNum == ev.ObjNum) &&
                        (!Filters.HasFlag(EventFilters.KP) || KPNum == ev.KPNum) &&
                        (!Filters.HasFlag(EventFilters.Param) || ParamID > 0 && ParamID == ev.ParamID ||
                            ParamIDs != null && ParamIDs.Contains(ev.ParamID)) &&
                        (!Filters.HasFlag(EventFilters.Cnls) || CnlNums != null && CnlNums.Contains(ev.CnlNum)) &&
                        (!Filters.HasFlag(EventFilters.Stat) || Statuses != null && Statuses.Contains(ev.NewCnlStat)) &&
                        (!Filters.HasFlag(EventFilters.Ack) || Checked == ev.Checked);
                }
            }
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
        /// Конструктор
        /// </summary>
        public EventTableLight()
        {
            tableName = "";
            fileModTime = DateTime.MinValue;
            lastFillTime = DateTime.MinValue;

            allEvents = new List<Event>();
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
                lastFillTime = value;
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
        /// Добавить событие в таблицу
        /// </summary>
        public void AddEvent(Event ev)
        {
            allEvents.Add(ev);
        }

        /// <summary>
        /// Очистить таблицу событий
        /// </summary>
        public void Clear()
        {
            allEvents.Clear();
        }

        /// <summary>
        /// Получить отфильтрованные события
        /// </summary>
        public List<Event> GetFilteredEvents(EventFilter filter)
        {
            bool reversed;
            return GetFilteredEvents(filter, 0, 0, out reversed);
        }

        /// <summary>
        /// Получить отфильтрованные события в указанном диапазоне
        /// </summary>
        public List<Event> GetFilteredEvents(EventFilter filter, int lastCount, int startEvNum, out bool reversed)
        {
            if (filter == null)
                throw new ArgumentNullException("filter");
            filter.Check();

            reversed = false;
            List<Event> filteredEvents = lastCount > 0 ? new List<Event>(lastCount) : new List<Event>();
            int startEvInd = Math.Max(0, startEvNum - 1);
            int allEventsCnt = allEvents.Count;

            Action<int> addEventAction = delegate(int i) 
            {
                Event ev = allEvents[i];
                if (filter.Satisfied(ev))
                    filteredEvents.Add(ev);
            };

            if (lastCount > 0)
            {
                for (int i = allEventsCnt - 1; i >= startEvInd && filteredEvents.Count < lastCount; i--)
                    addEventAction(i);
                reversed = true;
            }
            else
            {
                for (int i = startEvInd; i < allEventsCnt; i++)
                    addEventAction(i);
            }

            return filteredEvents;
        }

        /// <summary>
        /// Получить событие по номеру
        /// </summary>
        public Event GetEventByNum(int evNum)
        {
            if (1 <= evNum && evNum <= allEvents.Count)
            {
                Event ev = allEvents[evNum - 1];
                return ev.Number == evNum ? ev : null;
            }
            else
            {
                return null;
            }
        }
    }
}
