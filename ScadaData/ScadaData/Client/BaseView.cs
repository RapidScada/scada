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
 * Summary  : The base class for view
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2011
 * Modified : 2013
 */

using System;
using System.IO;
using System.Collections.Generic;

namespace Scada.Client
{
    /// <summary>
    /// The base class for view
    /// <para>Базовый класс представления</para>
    /// </summary>
    public abstract class BaseView
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public BaseView()
        {
            ItfObjName = "";
            CnlList = new List<int>();
            CtrlCnlList = new List<int>();
            StoredOnServer = true;
        }


        /// <summary>
        /// Получить заголовок представления
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Получить или установить наименование объекта интерфейса
        /// </summary>
        public string ItfObjName { get; set; }

        /// <summary>
        /// Получить упорядоченный без повторений список номеров входных каналов, 
        /// которые используются в представлении
        /// </summary>
        public List<int> CnlList { get; protected set; }

        /// <summary>
        /// Получить упорядоченный без повторений список номеров каналов управления, 
        /// которые используются в представлении
        /// </summary>
        public List<int> CtrlCnlList { get; protected set; }

        /// <summary>
        /// Получить признак хранения файла представления на сервере (в директории интерфейса)
        /// </summary>
        public bool StoredOnServer { get; protected set; }


        /// <summary>
        /// Добавить номер входного канала в список, сохраняя упорядоченность и уникальность его элементов
        /// </summary>
        protected void AddCnlNum(int cnlNum)
        {
            if (cnlNum > 0)
            {
                int index = CnlList.BinarySearch(cnlNum);
                if (index < 0)
                    CnlList.Insert(~index, cnlNum);
            }
        }

        /// <summary>
        /// Добавить номер канала управления в список, сохраняя упорядоченность и уникальность его элементов
        /// </summary>
        protected void AddCtrlCnlNum(int ctrlCnlNum)
        {
            if (ctrlCnlNum > 0)
            {
                int index = CtrlCnlList.BinarySearch(ctrlCnlNum);
                if (index < 0)
                    CtrlCnlList.Insert(~index, ctrlCnlNum);
            }
        }


		/// <summary>
		/// Загрузить представление из потока
		/// </summary>
        public virtual void LoadFromStream(Stream stream)
        {
        }
		
        /// <summary>
		/// Привязать свойства входных каналов к элементам представления
		/// </summary>
        public virtual void BindCnlProps(CnlProps[] cnlPropsArr)
        {
        }
        
        /// <summary>
        /// Очистить представление
        /// </summary>
        public virtual void Clear()
        {
            Title = "";
            ItfObjName = "";
            CnlList.Clear();
            CtrlCnlList.Clear();
        }
    }
}
