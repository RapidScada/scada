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
 * Module   : PlgTable
 * Summary  : Table view specification
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2019
 */

using Scada.Table;
using System;

namespace Scada.Web.Plugins.Table
{
    /// <summary>
    /// Table view specification
    /// <para>Спецификация табличного представления</para>
    /// </summary>
    public class TableViewSpec : ViewSpec
    {
        /// <summary>
        /// Получить код типа представления
        /// </summary>
        public override string TypeCode
        {
            get
            {
                // TODO: заменить на TableView после добавления поля ViewTypeCode в базу конфигурации
                return "tbl";
            }
        }

        /// <summary>
        /// Получить ссылку на иконку типа представлений
        /// </summary>
        public override string IconUrl
        {
            get
            {
                return "~/plugins/Table/images/tableicon.png";
            }
        }
        
        /// <summary>
        /// Получить тип представления
        /// </summary>
        public override Type ViewType
        {
            get
            {
                return typeof(TableView);
            }
        }


        /// <summary>
        /// Получить ссылку на представление с заданным идентификатором
        /// </summary>
        public override string GetUrl(int viewID)
        {
            return "~/plugins/Table/Table.aspx?viewID=" + viewID;
        }
    }
}