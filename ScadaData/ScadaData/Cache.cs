/*
 * Copyright 2016 Mikhail Shiryaev
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
 * Summary  : Generic cache
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

using System;
using System.Collections.Generic;

namespace Scada
{
    /// <summary>
    /// Generic cache
    /// <para>Универсальный кеш</para>
    /// </summary>
    /// <remarks>The class is not thread safe
    /// <para>Класс не является потокобезопасным</para></remarks>
    public class Cache<TKey, TValue>
    {
        /// <summary>
        /// Кешированный элемент
        /// </summary>
        public class CacheItem
        {
            /// <summary>
            /// Конструктор, ограничивающий создание объекта без параметров
            /// </summary>
            protected CacheItem()
            {
            }
            /// <summary>
            /// Конструктор
            /// </summary>
            protected internal CacheItem(TValue value, DateTime valueAge, DateTime accessDT)
            {
                Value = value;
                ValueAge = valueAge;
                AccessDT = accessDT;
            }

            /// <summary>
            /// Получить или установить кешированное значение
            /// </summary>
            public TValue Value { get; set; }
            /// <summary>
            /// Получить или установить время изменения значения в источнике
            /// </summary>
            public DateTime ValueAge { get; set; }
            /// <summary>
            /// Получить или установить дату и время последнего доступа к элементу
            /// </summary>
            public DateTime AccessDT { get; set; }
        }


        /// <summary>
        /// Кешированные элементы 
        /// </summary>
        /// <remarks>SortedDictionary по сравнению с SortedList: 
        /// вставка и удаление элементов быстрее, скорость извлечения аналогична</remarks>
        protected SortedDictionary<TKey, CacheItem> items;


        /// <summary>
        /// Конструктор, ограничивающий создание объекта без параметров
        /// </summary>
        protected Cache()
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public Cache(TimeSpan storePeriod, int capacity)
        {
            if (capacity < 1)
                throw new ArgumentException("Capacity must be positive.", "capacity");

            items = new SortedDictionary<TKey, CacheItem>();
            StorePeriod = storePeriod;
            Capacity = capacity;
            LastRemoveDT = DateTime.MinValue;
        }


        /// <summary>
        /// Получить период хранения элементов с момента последнего доступа
        /// </summary>
        public TimeSpan StorePeriod { get; protected set; }

        /// <summary>
        /// Получить вместимость
        /// </summary>
        public int Capacity { get; protected set; }

        /// <summary>
        /// Получить время последнего удаления устаревших элементов
        /// </summary>
        public DateTime LastRemoveDT { get; protected set; }


        /// <summary>
        /// Добавить значение в кеш
        /// </summary>
        public void AddValue(TKey key, TValue value)
        {
            AddValue(key, value, DateTime.MinValue, DateTime.Now);
        }

        /// <summary>
        /// Добавить значение в кеш
        /// </summary>
        public void AddValue(TKey key, TValue value, DateTime valueAge)
        {
            AddValue(key, value, valueAge, DateTime.Now);
        }

        /// <summary>
        /// Добавить значение в кеш
        /// </summary>
        public void AddValue(TKey key, TValue value, DateTime valueAge, DateTime accessDT)
        {
            if (key == null)
                throw new ArgumentNullException("key");

            lock (this)
            {
                CacheItem cacheItem = new CacheItem(value, valueAge, accessDT);
                items.Add(key, cacheItem);
            }
        }


        /// <summary>
        /// Получить элемент по ключу, обновив время доступа
        /// </summary>
        public CacheItem GetItem(TKey key)
        {
            return GetItem(DateTime.Now);
        }

        /// <summary>
        /// Получить элемент по ключу, обновив время доступа
        /// </summary>
        public CacheItem GetItem(TKey key, DateTime accessDT)
        {
            if (key == null)
                throw new ArgumentNullException("key");

            lock (this)
            {
                CacheItem item;
                if (items.TryGetValue(key, out item))
                {
                    item.AccessDT = accessDT;
                    return item;
                }
                else
                {
                    return null;
                }
            }
        }


        /// <summary>
        /// Потокобезопасно обновить свойства элемента
        /// </summary>
        public void UpdateItem(CacheItem cacheItem, TValue value)
        {
            UpdateItem(cacheItem, value, DateTime.MinValue);
        }

        /// <summary>
        /// Потокобезопасно обновить свойства элемента
        /// </summary>
        public void UpdateItem(CacheItem cacheItem, TValue value, DateTime valueAge)
        {
            if (cacheItem == null)
                throw new ArgumentNullException("cacheItem");

            lock (this)
            {
                cacheItem.Value = value;
                cacheItem.ValueAge = valueAge;
            }
        }


        /// <summary>
        /// Удалить устаревшие элементы
        /// </summary>
        public void RemoveOutdatedItems()
        {
            RemoveOutdatedItems(DateTime.Now);
        }

        /// <summary>
        /// Удалить устаревшие элементы
        /// </summary>
        public void RemoveOutdatedItems(DateTime nowDT)
        {
            lock (this)
            {
                // удаление элементов по времени последнего доступа
                List<TKey> keysToRemove = new List<TKey>();

                foreach (KeyValuePair<TKey, CacheItem> pair in items)
                {
                    if (nowDT - pair.Value.AccessDT > StorePeriod)
                        keysToRemove.Add(pair.Key);
                }

                foreach (TKey key in keysToRemove)
                    items.Remove(key);

                // удаление элементов, если превышена вместимость, с учётом времени доступа
                int itemsCnt = items.Count;

                if (itemsCnt > Capacity)
                {
                    TKey[] keys = new TKey[itemsCnt];
                    DateTime[] accessDTs = new DateTime[itemsCnt];
                    int i = 0;

                    foreach (KeyValuePair<TKey, CacheItem> pair in items)
                    {
                        keys[i] = pair.Key;
                        accessDTs[i] = pair.Value.AccessDT;
                        i++;
                    }

                    Array.Sort(accessDTs, keys);
                    int delCnt = itemsCnt - Capacity;

                    for (int j = 0; j < delCnt; j++)
                        items.Remove(keys[j]);
                }

                LastRemoveDT = nowDT;
            }
        }
    }
}
