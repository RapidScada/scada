/*
 * Copyright 2017 Mikhail Shiryaev
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
 * Summary  : Snapshot table for read and write data access
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2013
 * Modified : 2017
 */

using System;
using System.Collections.Generic;
using System.Data;

namespace Scada.Data.Tables
{
    /// <summary>
    /// Snapshot table for read and write data access
    /// <para>Таблица срезов для доступа к данным на чтение и запись</para>
    /// </summary>
    public class SrezTable : SrezTableLight
    {
        /// <summary>
        /// Описание структуры среза
        /// </summary>
        public class SrezDescr
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            protected SrezDescr()
            {
            }
            /// <summary>
            /// Конструктор
            /// </summary>
            public SrezDescr(int cnlCnt)
            {
                if (cnlCnt <= 0)
                    throw new ArgumentOutOfRangeException("cnlCnt");

                CnlNums = new int[cnlCnt];
                CS = 0;
            }

            /// <summary>
            /// Получить номера каналов среза, упорядоченные по возростанию
            /// </summary>
            public int[] CnlNums { get; protected set; }
            /// <summary>
            /// Контрольная сумма
            /// </summary>
            public ushort CS { get; protected set; }

            /// <summary>
            /// Вычислить контрольную сумму
            /// </summary>
            public ushort CalcCS()
            {
                int cs = 1;
                foreach (int cnlNum in CnlNums)
                    cs += cnlNum;
                CS = (ushort)cs;
                return CS;
            }
            /// <summary>
            /// Проверить, идентичен ли заданный объект текущему
            /// </summary>
            public bool Equals(SrezDescr srezDescr)
            {
                if (srezDescr == this)
                {
                    return true;
                }
                else if (srezDescr != null && CS == srezDescr.CS && CnlNums.Length == srezDescr.CnlNums.Length)
                {
                    int len = CnlNums.Length;
                    for (int i = 0; i < len; i++)
                    {
                        if (CnlNums[i] != srezDescr.CnlNums[i])
                            return false;
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            /// <summary>
            /// Проверить, идентичены ли заданные номера каналов среза текущим
            /// </summary>
            public bool Equals(int[] cnlNums)
            {
                if (cnlNums == CnlNums)
                {
                    return true;
                }
                else if (cnlNums != null && CnlNums.Length == cnlNums.Length)
                {
                    int len = CnlNums.Length;
                    for (int i = 0; i < len; i++)
                    {
                        if (CnlNums[i] != cnlNums[i])
                            return false;
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Срез данных входных каналов за определённый момент времени
        /// </summary>
        new public class Srez : SrezTableLight.Srez
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            public Srez(DateTime dateTime, int cnlCnt)
                : base(dateTime, cnlCnt)
            {
                SrezDescr = new SrezDescr(cnlCnt);
                State = DataRowState.Detached;
                Position = -1;
            }
            /// <summary>
            /// Конструктор
            /// </summary>
            public Srez(DateTime dateTime, SrezDescr srezDescr)
            {
                if (srezDescr == null)
                    throw new ArgumentNullException("srezDescr");

                DateTime = dateTime;
                int cnlCnt = srezDescr.CnlNums.Length;
                CnlNums = new int[cnlCnt];
                srezDescr.CnlNums.CopyTo(CnlNums, 0);
                CnlData = new CnlData[cnlCnt];

                SrezDescr = srezDescr;
                State = DataRowState.Detached;
                Position = -1;
            }
            /// <summary>
            /// Конструктор
            /// </summary>
            public Srez(DateTime dateTime, SrezDescr srezDescr, SrezTableLight.Srez srcSrez)
                : this(dateTime, srezDescr)
            {
                CopyDataFrom(srcSrez);
            }
            /// <summary>
            /// Конструктор
            /// </summary>
            public Srez(DateTime dateTime, Srez srcSrez)
                : this(dateTime, srcSrez.SrezDescr, srcSrez)
            {
            }

            /// <summary>
            /// Получить описание структуры среза, но основе которого он был создан
            /// </summary>
            public SrezDescr SrezDescr { get; protected set; }
            /// <summary>
            /// Получить или установить текущее состояние среза
            /// </summary>
            public DataRowState State { get; protected internal set; }
            /// <summary>
            /// Получить или установить позицию временной метки среза в файле или потоке
            /// </summary>
            public long Position { get; protected internal set; }

            /// <summary>
            /// Копировать данные из исходного среза в текущий
            /// </summary>
            public void CopyDataFrom(SrezTableLight.Srez srcSrez)
            {
                if (srcSrez != null)
                {
                    if (SrezDescr.Equals(srcSrez.CnlNums))
                    {
                        srcSrez.CnlData.CopyTo(CnlData, 0);
                    }
                    else
                    {
                        int srcCnlCnt = srcSrez.CnlData.Length;
                        for (int i = 0; i < srcCnlCnt; i++)
                            SetCnlData(srcSrez.CnlNums[i], srcSrez.CnlData[i]);
                    }
                }
            }
        }

        
        /// <summary>
        /// Признак выполнения загрузки данных в таблицу срезов
        /// </summary>
        protected bool dataLoading;


        /// <summary>
        /// Конструктор
        /// </summary>
        public SrezTable()
            : base()
        {
            dataLoading = false;
            Descr = "";
            LastStoredSrez = null;
            AddedSrezList = new List<Srez>();
            ModifiedSrezList = new List<Srez>();
        }


        /// <summary>
        /// Получить или установить описание таблицы
        /// </summary>
        public string Descr { get; set; }

        /// <summary>
        /// Получить последний записанный срез
        /// </summary>
        public Srez LastStoredSrez { get; protected internal set; }

        /// <summary>
        /// Получить список добавленных срезов
        /// </summary>
        public List<Srez> AddedSrezList { get; protected set; }

        /// <summary>
        /// Получить список изменённых срезов
        /// </summary>
        public List<Srez> ModifiedSrezList { get; protected set; }

        /// <summary>
        /// Получить признак изменения таблицы срезов
        /// </summary>
        public bool Modified
        {
            get
            {
                return AddedSrezList.Count > 0 || ModifiedSrezList.Count > 0;
            }
        }


        /// <summary>
        /// Добавить срез в таблицу
        /// </summary>
        /// <remarks>Если в таблице уже существует срез с заданной меткой времени, 
        /// то добавление нового среза не происходит</remarks>
        public override bool AddSrez(SrezTableLight.Srez srez)
        {
            if (!(srez is Srez))
                throw new ArgumentException("Srez type is incorrect.", "srez");

            if (base.AddSrez(srez))
            {
                MarkSrezAsAdded((Srez)srez);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Добавить копию среза в таблицу
        /// </summary>
        /// <remarks>Если в таблице уже существует срез с заданной меткой времени, 
        /// то в него копируются данные из исходного среза</remarks>
        public Srez AddSrezCopy(Srez srcSrez, DateTime srezDT)
        {
            if (srcSrez == null)
                throw new ArgumentNullException("srcSrez");

            Srez srez;
            SrezTableLight.Srez lightSrez;

            if (SrezList.TryGetValue(srezDT, out lightSrez))
            {
                // изменение существующего в таблице среза
                srez = (Srez)lightSrez; // возможно исключение InvalidCastException
                srez.CopyDataFrom(srcSrez);

                if (srez.State == DataRowState.Unchanged)
                {
                    srez.State = DataRowState.Modified;
                    ModifiedSrezList.Add(srez);
                }
            }
            else
            {
                // создание и добавление нового среза в таблицу
                srez = new SrezTable.Srez(srezDT, srcSrez);
                srez.State = DataRowState.Added;
                AddedSrezList.Add(srez);
                SrezList.Add(srezDT, srez);
            }

            return srez;
        }

        /// <summary>
        /// Получить срез за определённое время
        /// </summary>
        public new Srez GetSrez(DateTime dateTime)
        {
            SrezTableLight.Srez srez;
            return SrezList.TryGetValue(dateTime, out srez) ? srez as Srez : null;
        }

        /// <summary>
        /// Начать загрузку данных в таблицу
        /// </summary>
        /// <remarks>Приостанавливается отслеживание добавления записей в таблицу</remarks>
        public void BeginLoadData()
        {
            dataLoading = true;
        }

        /// <summary>
        /// Завершить загрузку данных в таблицу
        /// </summary>
        /// <remarks>Возобновляется отслеживание добавления записей в таблицу</remarks>
        public void EndLoadData()
        {
            dataLoading = false;
        }
        
        /// <summary>
        /// Принять изменения таблицы срезов
        /// </summary>
        public void AcceptChanges()
        {
            foreach (SrezTableLight.Srez lightSrez in SrezList.Values)
            {
                Srez srez = lightSrez as Srez;
                if (srez != null)
                    srez.State = DataRowState.Unchanged;
            }

            AddedSrezList.Clear();
            ModifiedSrezList.Clear();
        }

        /// <summary>
        /// Отметить срез как добавленный
        /// </summary>
        public void MarkSrezAsAdded(Srez srez)
        {
            if (srez != null)
            {
                if (dataLoading)
                {
                    srez.State = DataRowState.Unchanged;
                }
                else
                {
                    srez.State = DataRowState.Added;
                    srez.Position = -1;
                    AddedSrezList.Add(srez);
                }
            }
        }

        /// <summary>
        /// Отметить срез как изменённый
        /// </summary>
        public void MarkSrezAsModified(Srez srez)
        {
            if (srez != null && (srez.State == DataRowState.Unchanged || srez.State == DataRowState.Deleted))
            {
                srez.State = DataRowState.Modified;
                ModifiedSrezList.Add(srez);
            }
        }

        /// <summary>
        /// Очистить таблицу срезов
        /// </summary>
        public override void Clear()
        {
            base.Clear();
            LastStoredSrez = null;
            AddedSrezList.Clear();
            ModifiedSrezList.Clear();
        }
    }
}