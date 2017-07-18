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
 * Summary  : Access to the configuration database
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2017
 */

using Scada.Data.Models;
using System;
using System.Collections.Generic;

namespace Scada.Data.Configuration
{
    /// <summary>
    /// Access to the configuration database
    /// <para>Доступ к данным базы конфигурации</para>
    /// </summary>
    public class ConfDAO
    {
        /// <summary>
        /// Таблицы базы конфигурации
        /// </summary>
        protected BaseTables baseTables;


        /// <summary>
        /// Конструктор, ограничивающий создание объекта без параметров
        /// </summary>
        protected ConfDAO()
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public ConfDAO(BaseTables baseTables)
        {
            if (baseTables == null)
                throw new ArgumentNullException("baseTables");

            this.baseTables = baseTables;
        }


        /// <summary>
        /// Получить свойства входных каналов, упорядоченные по возрастанию номеров каналов
        /// </summary>
        public List<InCnlProps> GetInCnlProps()
        {
            return null;
        }

        /// <summary>
        /// Получить свойства каналов управления, упорядоченные по возрастанию номеров каналов
        /// </summary>
        public List<CtrlCnlProps> GetCtrlCnlProps()
        {
            return null;
        }

        /// <summary>
        /// Получить наименования объектов, ключ - номер объекта
        /// </summary>
        public SortedList<int, string> GetObjNames()
        {
            return null;
        }

        /// <summary>
        /// Получить наименования КП, ключ - номер КП
        /// </summary>
        public SortedList<int, string> GetKPNames()
        {
            return null;
        }

        /// <summary>
        /// Получить свойства статусов входных каналов, ключ - статус
        /// </summary>
        public SortedList<int, CnlStatProps> GetCnlStatProps()
        {
            return null;
        }
    }
}
