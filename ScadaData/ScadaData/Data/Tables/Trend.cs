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
 * Summary  : Trend for fast reading one input channel data
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2006
 * Modified : 2006
 */

using System;
using System.Collections.Generic;

namespace Scada.Data.Tables
{
    /// <summary>
    /// Trend for fast reading one input channel data
    /// <para>Тренд для быстрого чтения данных одного входного канала</para>
    /// </summary>
    public class Trend
    {
        /// <summary>
        /// Точка тренда
        /// </summary>
        public struct Point : IComparable<Point>
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            public Point(DateTime dateTime, double val, int stat)
            {
                DateTime = dateTime;
                Val = val;
                Stat = stat;
            }

            /// <summary>
            /// Временная метка
            /// </summary>
            public DateTime DateTime;
            /// <summary>
            /// Значение
            /// </summary>
            public double Val;
            /// <summary>
            /// Статус
            /// </summary>
            public int Stat;

            /// <summary>
            /// Сравнить текущий объект с другим объектом такого же типа
            /// </summary>
            public int CompareTo(Point other)
            {
                return DateTime.CompareTo(other.DateTime);
            }
        }

        /// <summary>
        /// Имя таблицы, к которой относится тренд
        /// </summary>
        protected string tableName;
        /// <summary>
        /// Время последнего изменения файла таблицы, к которой относится тренд
        /// </summary>
        protected DateTime fileModTime;
        /// <summary>
        /// Время последего успешного заполнения тренда
        /// </summary>
        protected DateTime lastFillTime;
        /// <summary>
        /// Точки тренда
        /// </summary>
        protected List<Point> points;
        /// <summary>
        /// Номер входного канала тренда
        /// </summary>
        protected int cnlNum;


        /// <summary>
        /// Конструктор
        /// </summary>
        protected Trend()
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="cnlNum">номер входного канала тренда</param>
        public Trend(int cnlNum)
        {
            tableName = "";
            fileModTime = DateTime.MinValue;
            lastFillTime = DateTime.MinValue;

            points = new List<Point>();
            this.cnlNum = cnlNum;
        }


        /// <summary>
        /// Получить или установить имя файла таблицы, к которой относится тренд
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
        /// Получить или установить время последнего изменения файла таблицы, к которой относится тренд
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
        /// Получить или установить время последего успешного заполнения тренда
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
        /// Точки тренда
        /// </summary>
        public List<Point> Points
        {
            get
            {
                return points;
            }        
        }

        /// <summary>
        /// Номер входного канала тренда
        /// </summary>
        public int CnlNum
        {
            get
            {
                return cnlNum;
            }
        }


        /// <summary>
        /// Отсортировать точки тренда по времени
        /// </summary>
        public void Sort()
        {
            points.Sort();
        }

        /// <summary>
        /// Очистить тренд
        /// </summary>
        public void Clear()
        {
            points.Clear();
        }
    }
}