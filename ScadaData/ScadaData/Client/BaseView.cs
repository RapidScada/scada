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
 * Module   : ScadaData
 * Summary  : The base class for view
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2011
 * Modified : 2019
 */

using Scada.Data.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace Scada.Client
{
    /// <summary>
    /// The base class for view.
    /// <para>Базовый класс представления.</para>
    /// </summary>
    /// <remarks>
    /// Derived views must provide thread safe read access in case that the object is not being changed. 
    /// Write operations must be synchronized.
    /// <para>Дочерние представления должны обеспечивать потокобезопасный доступ на чтение при условии, 
    /// что объект не изменяется. Операции записи должны синхронизироваться.</para></remarks>
    public abstract class BaseView : ISupportLoading
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public BaseView()
        {
            Title = "";
            Path = "";
            Args = new SortedList<string, string>();
            CnlSet = new HashSet<int>();
            CnlList = new List<int>();
            CtrlCnlSet = new HashSet<int>();
            CtrlCnlList = new List<int>();
            StoredOnServer = true;
            BaseAge = DateTime.MinValue;
            Stamp = 0;
        }


        /// <summary>
        /// Получить или установить заголовок представления.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Получить или установить путь файла представления.
        /// </summary>
        /// <remarks>Если файл представления хранится на сервере, 
        /// то путь указывается относительно директории интерфейса.</remarks>
        public string Path { get; set; }

        /// <summary>
        /// Получить имя файла представления.
        /// </summary>
        public string FileName
        {
            get
            {
                return System.IO.Path.GetFileName(Path);
            }
        }
        
        /// <summary>
        /// Gets the view arguments.
        /// </summary>
        public SortedList<string, string> Args { get; protected set; }

        /// <summary>
        /// Получить множество номеров входных каналов, которые используются в представлении.
        /// </summary>
        public HashSet<int> CnlSet { get; protected set; }

        /// <summary>
        /// Получить упорядоченный без повторений список номеров входных каналов, 
        /// которые используются в представлении.
        /// </summary>
        public List<int> CnlList { get; protected set; }

        /// <summary>
        /// Получить множество номеров каналов управления, которые используются в представлении.
        /// </summary>
        public HashSet<int> CtrlCnlSet { get; protected set; }

        /// <summary>
        /// Получить упорядоченный без повторений список номеров каналов управления, 
        /// которые используются в представлении.
        /// </summary>
        public List<int> CtrlCnlList { get; protected set; }

        /// <summary>.
        /// Получить признак хранения файла представления на сервере (в директории интерфейса)
        /// </summary>
        public bool StoredOnServer { get; protected set; }

        /// <summary>
        /// Получить или установить время последнего изменения базы конфигурации, 
        /// для которого выполнена привязка каналов.
        /// </summary>
        public DateTime BaseAge { get; set; }

        /// <summary>
        /// Получить или установить уникальную метку объекта в пределах некоторого набора данных.
        /// </summary>
        /// <remarks>Используется для контроля целостности данных при получении представления из кэша.</remarks>
        public long Stamp { get; set; }

        /// <summary>
        /// Получить объект для синхронизации доступа к представлению.
        /// </summary>
        public object SyncRoot
        {
            get
            {
                return this;
            }
        }


        /// <summary>
        /// Добавить номер входного канала в множество и в список.
        /// </summary>
        protected void AddCnlNum(int cnlNum)
        {
            if (cnlNum > 0 && CnlSet.Add(cnlNum))
            {
                int index = CnlList.BinarySearch(cnlNum);
                if (index < 0)
                    CnlList.Insert(~index, cnlNum);
            }
        }

        /// <summary>
        /// Добавить номер канала управления в множество и в список.
        /// </summary>
        protected void AddCtrlCnlNum(int ctrlCnlNum)
        {
            if (ctrlCnlNum > 0 && CtrlCnlSet.Add(ctrlCnlNum))
            {
                int index = CtrlCnlList.BinarySearch(ctrlCnlNum);
                if (index < 0)
                    CtrlCnlList.Insert(~index, ctrlCnlNum);
            }
        }


        /// <summary>
        /// Sets the view arguments.
        /// </summary>
        public virtual void SetArgs(string args)
        {
            Args.Clear();

            if (!string.IsNullOrEmpty(args))
            {
                Args = new SortedList<string, string>();
                string[] parts = args.Split('&');

                foreach (string part in parts)
                {
                    string key;
                    string val;
                    int idx = part.IndexOf("=");

                    if (idx >= 0)
                    {
                        key = part.Substring(0, idx).Trim();
                        val = part.Substring(idx + 1).Trim();
                    }
                    else
                    {
                        key = part.Trim();
                        val = "";
                    }

                    Args[key] = val;
                }
            }
        }

        /// <summary>
        /// Updates the view title.
        /// </summary>
        public virtual void UpdateTitle(string s)
        {
            if (string.IsNullOrEmpty(Title))
                Title = s ?? "";
        }

		/// <summary>
		/// Загрузить представление из потока.
		/// </summary>
        public virtual void LoadFromStream(Stream stream)
        {
        }
		
        /// <summary>
		/// Привязать свойства входных каналов к элементам представления.
		/// </summary>
        public virtual void BindCnlProps(InCnlProps[] cnlPropsArr)
        {
        }

        /// <summary>
        /// Привязать свойства каналов управления к элементам представления.
        /// </summary>
        public virtual void BindCtrlCnlProps(CtrlCnlProps[] ctrlCnlPropsArr)
        {
        }

        /// <summary>
        /// Определить, что входной канал используется в представлении.
        /// </summary>
        public virtual bool ContainsCnl(int cnlNum)
        {
            return CnlSet.Contains(cnlNum);
        }

        /// <summary>
        /// Определить, что все заданные входные каналы используются в представлении.
        /// </summary>
        public virtual bool ContainsAllCnls(IEnumerable<int> cnlNums)
        {
            return CnlSet.Count > 0 && CnlSet.IsSupersetOf(cnlNums);
        }

        /// <summary>
        /// Определить, что канал управления используется в представлении.
        /// </summary>
        public virtual bool ContainsCtrlCnl(int ctrlCnlNum)
        {
            return CtrlCnlSet.Contains(ctrlCnlNum);
        }
        
        /// <summary>
        /// Очистить представление.
        /// </summary>
        public virtual void Clear()
        {
            Title = "";
            Path = "";
            CnlList.Clear();
            CtrlCnlList.Clear();
            CnlSet.Clear();
            CtrlCnlSet.Clear();
        }
    }
}
