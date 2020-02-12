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
 * Summary  : Snapshot table for fast read data access
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2006
 * Modified : 2020
 */

using System;
using System.Collections.Generic;

namespace Scada.Data.Tables
{
    /// <summary>
    /// Snapshot table for fast read data access.
    /// <para>Таблица срезов для быстрого доступа к данным на чтение.</para>
    /// </summary>
    public class SrezTableLight
    {
        /// <summary>
        /// Represents input channel data.
        /// <para>Представляет данные входного канала.</para>
        /// </summary>
        public struct CnlData
        {
            /// <summary>
            /// Пустые данные входного канала.
            /// </summary>
            public static readonly CnlData Empty = new CnlData(0.0, 0);

            /// <summary>
            /// Конструктор.
            /// </summary>
            public CnlData(double val, int stat)
                : this()
            {
                Val = val;
                Stat = stat;
            }

            /// <summary>
            /// Получить или установить значение.
            /// </summary>
            public double Val { get; set; }
            /// <summary>
            /// Получить или установить статус.
            /// </summary>
            public int Stat { get; set; }

            /// <summary>
            /// Determines whether the specified object is equal to the current object.
            /// </summary>
            public override bool Equals(object obj)
            {
                return this is CnlData cnlData && this == cnlData;
            }
            /// <summary>
            /// Returns the hash code for this instance.
            /// </summary>
            public override int GetHashCode()
            {
                return Tuple.Create(Val, Stat).GetHashCode();
            }
            /// <summary>
            /// Returns a value that indicates whether two specified inctances are equal.
            /// </summary>
            public static bool operator ==(CnlData x, CnlData y)
            {
                return x.Val == y.Val && x.Stat == y.Stat;
            }
            /// <summary>
            /// Returns a value that indicates whether two specified inctances are not equal.
            /// </summary>
            public static bool operator !=(CnlData x, CnlData y)
            {
                return !(x == y);
            }
        }

        /// <summary>
        /// Срез данных входных каналов за определённый момент времени
        /// </summary>
        public class Srez : IComparable<Srez>
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            protected Srez()
            {
            }
            /// <summary>
            /// Конструктор
            /// </summary>
            public Srez(DateTime dateTime, int cnlCnt)
            {
                if (cnlCnt < 0)
                    throw new ArgumentOutOfRangeException("cnlCnt");

                InitData(dateTime, cnlCnt);
            }
            /// <summary>
            /// Конструктор
            /// </summary>
            public Srez(DateTime dateTime, Srez sourceSrez)
            {
                if (sourceSrez == null)
                    throw new ArgumentNullException("sourceSrez");

                InitData(dateTime, sourceSrez.CnlNums.Length);
                sourceSrez.CnlNums.CopyTo(CnlNums, 0);
                sourceSrez.CnlData.CopyTo(CnlData, 0);
            }
            /// <summary>
            /// Конструктор
            /// </summary>
            /// <remarks>Номера каналов должны быть упорядочены по возростанию</remarks>
            public Srez(DateTime dateTime, int[] cnlNums)
            {
                if (cnlNums == null)
                    throw new ArgumentNullException("cnlNums");

                InitData(dateTime, cnlNums.Length);
                cnlNums.CopyTo(CnlNums, 0);
            }
            /// <summary>
            /// Конструктор
            /// </summary>
            /// <remarks>Номера каналов должны быть упорядочены по возростанию</remarks>
            public Srez(DateTime dateTime, int[] cnlNums, Srez sourceSrez)
            {
                if (cnlNums == null)
                    throw new ArgumentNullException("cnlNums");
                if (sourceSrez == null)
                    throw new ArgumentNullException("sourceSrez");

                int cnlCnt = cnlNums.Length;
                InitData(dateTime, cnlCnt);

                for (int i = 0; i < cnlCnt; i++)
                {
                    int cnlNum = cnlNums[i];
                    CnlData cnlData;
                    sourceSrez.GetCnlData(cnlNum, out cnlData);

                    CnlNums[i] = cnlNum;
                    CnlData[i] = cnlData;
                }
            }

            /// <summary>
            /// Получить временную метку среза
            /// </summary>
            public DateTime DateTime { get; protected set; }
            /// <summary>
            /// Получить номера каналов среза, упорядоченные по возростанию
            /// </summary>
            public int[] CnlNums { get; protected set; }
            /// <summary>
            /// Получить данные среза, соответствующие его номерам каналов
            /// </summary>
            public CnlData[] CnlData { get; protected set; }

            /// <summary>
            /// Инициализировать данные среза
            /// </summary>
            protected void InitData(DateTime dateTime, int cnlCnt)
            {
                DateTime = dateTime;
                CnlNums = new int[cnlCnt];
                CnlData = new CnlData[cnlCnt];
            }
            /// <summary>
            /// Получить индекс входного канала по номеру
            /// </summary>
            public int GetCnlIndex(int cnlNum)
            {
                return Array.BinarySearch<int>(CnlNums, cnlNum);
            }
            /// <summary>
            /// Получить данные входного канала по номеру
            /// </summary>
            public bool GetCnlData(int cnlNum, out CnlData cnlData)
            {
                int index = GetCnlIndex(cnlNum);
                if (index < 0)
                {
                    cnlData = SrezTableLight.CnlData.Empty;
                    return false;
                }
                else
                {
                    cnlData = CnlData[index];
                    return true;
                }
            }
            /// <summary>
            /// Получить данные входного канала по номеру
            /// </summary>
            public CnlData GetCnlData(int cnlNum)
            {
                CnlData cnlData;
                GetCnlData(cnlNum, out cnlData);
                return cnlData;
            }
            /// <summary>
            /// Получить значение и статус входного канала по номеру
            /// </summary>
            public bool GetCnlData(int cnlNum, out double val, out int stat)
            {
                CnlData cnlData;
                bool result = GetCnlData(cnlNum, out cnlData);
                val = cnlData.Val;
                stat = cnlData.Stat;
                return result;
            }
            /// <summary>
            /// Установить данные входного канала
            /// </summary>
            public bool SetCnlData(int cnlNum, CnlData cnlData)
            {
                int index = GetCnlIndex(cnlNum);
                if (index < 0)
                {
                    return false;
                }
                else
                {
                    CnlData[index] = cnlData;
                    return true;
                }
            }
            /// <summary>
            /// Сравнить текущий объект с другим объектом такого же типа
            /// </summary>
            public int CompareTo(Srez other)
            {
                return DateTime.CompareTo(other == null ? DateTime.MinValue : other.DateTime);
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
        /// Конструктор
        /// </summary>
        public SrezTableLight()
        {
            tableName = "";
            fileModTime = DateTime.MinValue;
            lastFillTime = DateTime.MinValue;
            SrezList = new SortedList<DateTime, Srez>();
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
        /// Получить или установить время последнего успешного заполнения таблицы
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
        /// Получить упорядоченный список срезов
        /// </summary>
        public SortedList<DateTime, Srez> SrezList { get; protected set; }


        /// <summary>
        /// Добавить срез в таблицу
        /// </summary>
        /// <remarks>Если в таблице уже существует срез с заданной меткой времени, 
        /// то добавление нового среза не происходит</remarks>
        public virtual bool AddSrez(Srez srez)
        {
            if (srez == null)
                throw new ArgumentNullException("srez");

            if (SrezList.ContainsKey(srez.DateTime))
            {
                return false;
            }
            else
            {
                SrezList.Add(srez.DateTime, srez);
                return true;
            }
        }

        /// <summary>
        /// Получить срез за определённое время
        /// </summary>
        public Srez GetSrez(DateTime dateTime)
        {
            Srez srez;
            return SrezList.TryGetValue(dateTime, out srez) ? srez : null;
        }

        /// <summary>
        /// Очистить таблицу срезов
        /// </summary>
        public virtual void Clear()
        {
            SrezList.Clear();
        }
    }
}