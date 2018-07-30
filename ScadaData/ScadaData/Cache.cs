/*
 * Copyright 2018 Mikhail Shiryaev
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
 * Modified : 2018
 */

using System;
using System.Collections.Generic;

namespace Scada
{
    /// <summary>
    /// Generic cache
    /// <para>Универсальный кэш</para>
    /// </summary>
    /// <remarks>The class is thread safe
    /// <para>Класс является потокобезопасным</para></remarks>
    public class Cache<TKey, TValue>
    {
        /// <summary>
        /// Кэшированный элемент
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
            protected internal CacheItem(TKey key, TValue value, DateTime valueAge, DateTime nowDT)
            {
                Key = key;
                Value = value;
                ValueAge = valueAge;
                ValueRefrDT = nowDT;
                AccessDT = nowDT;
            }

            /// <summary>
            /// Получить или установить ключ
            /// </summary>
            public TKey Key { get; set; }
            /// <summary>
            /// Получить или установить кэшированное значение
            /// </summary>
            public TValue Value { get; set; }
            /// <summary>
            /// Получить или установить время изменения значения в источнике
            /// </summary>
            public DateTime ValueAge { get; set; }
            /// <summary>
            /// Получить или установить время обновления значения в кэше
            /// </summary>
            public DateTime ValueRefrDT { get; set; }
            /// <summary>
            /// Получить или установить дату и время последнего доступа к элементу
            /// </summary>
            public DateTime AccessDT { get; set; }
        }


        /// <summary>
        /// Кэшированные элементы 
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
        /// Добавить значение в кэш
        /// </summary>
        public CacheItem AddValue(TKey key, TValue value)
        {
            return AddValue(key, value, DateTime.MinValue, DateTime.Now);
        }

        /// <summary>
        /// Добавить значение в кэш
        /// </summary>
        public CacheItem AddValue(TKey key, TValue value, DateTime valueAge)
        {
            return AddValue(key, value, valueAge, DateTime.Now);
        }

        /// <summary>
        /// Добавить значение в кэш
        /// </summary>
        public CacheItem AddValue(TKey key, TValue value, DateTime valueAge, DateTime nowDT)
        {
            if (key == null)
                throw new ArgumentNullException("key");

            lock (this)
            {
                CacheItem cacheItem = new CacheItem(key, value, valueAge, nowDT);
                items.Add(key, cacheItem);
                return cacheItem;
            }
        }


        /// <summary>
        /// Получить элемент по ключу, обновив время доступа
        /// </summary>
        public CacheItem GetItem(TKey key)
        {
            return GetItem(key, DateTime.Now);
        }

        /// <summary>
        /// Получить элемент по ключу, обновив время доступа
        /// </summary>
        public CacheItem GetItem(TKey key, DateTime nowDT)
        {
            if (key == null)
                throw new ArgumentNullException("key");

            lock (this)
            {
                // получение запрошенного элемента
                CacheItem item;
                if (items.TryGetValue(key, out item))
                    item.AccessDT = nowDT;

                // автоматическая очистка устаревших элементов
                if (nowDT - LastRemoveDT > StorePeriod)
                    RemoveOutdatedItems(nowDT);

                return item;
            }
        }

        /// <summary>
        /// Получить элемент по ключу, обновив время доступа, 
        /// или создать новый пустой элемент, если ключ не содержится в кэше
        /// </summary>
        public CacheItem GetOrCreateItem(TKey key, DateTime nowDT)
        {
            lock (this)
            {
                CacheItem cacheItem = GetItem(key, nowDT);
                if (cacheItem == null)
                    cacheItem = AddValue(key, default(TValue), DateTime.MinValue, nowDT);
                return cacheItem;
            }
        }

        /// <summary>
        /// Получить все элементы для просмотра без обновления времени доступа
        /// </summary>
        public CacheItem[] GetAllItemsForWatching()
        {
            lock (this)
            {
                CacheItem[] itemsCopy = new CacheItem[items.Count];
                int i = 0;
                foreach (CacheItem item in items.Values)
                    itemsCopy[i++] = item;
                return itemsCopy;
            }
        }


        /// <summary>
        /// Потокобезопасно обновить свойства элемента
        /// </summary>
        public void UpdateItem(CacheItem cacheItem, TValue value)
        {
            UpdateItem(cacheItem, value, DateTime.MinValue, DateTime.Now);
        }
        
        /// <summary>
        /// Потокобезопасно обновить свойства элемента
        /// </summary>
        public void UpdateItem(CacheItem cacheItem, TValue value, DateTime valueAge)
        {
            UpdateItem(cacheItem, value, valueAge, DateTime.Now);
        }

        /// <summary>
        /// Потокобезопасно обновить свойства элемента
        /// </summary>
        public void UpdateItem(CacheItem cacheItem, TValue value, DateTime valueAge, DateTime nowDT)
        {
            if (cacheItem == null)
                throw new ArgumentNullException("cacheItem");

            lock (this)
            {
                cacheItem.Value = value;
                cacheItem.ValueAge = valueAge;
                cacheItem.ValueRefrDT = nowDT;
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

        /// <summary>
        /// Удалить элемент по ключу
        /// </summary>
        public void RemoveItem(TKey key)
        {
            lock (this)
            {
                items.Remove(key);
            }
        }

        /// <summary>
        /// Очистить кэш
        /// </summary>
        public void Clear()
        {
            lock (this)
            {
                items.Clear();
            }
        }
    }
}
