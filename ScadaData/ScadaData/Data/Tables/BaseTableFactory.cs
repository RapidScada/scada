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
 * Summary  : Factory for creating instances of configuration database tables
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
 * Modified : 2019
 */

using System;

namespace Scada.Data.Tables
{
    /// <summary>
    /// Factory for creating instances of configuration database tables.
    /// <para>Фабрика для создания экземпляров таблиц базы конфигурации.</para>
    /// </summary>
    public static class BaseTableFactory
    {
        /// <summary>
        /// Gets a new instance of the table having the specified item type.
        /// </summary>
        public static IBaseTable GetBaseTable(Type itemType, string name, string primaryKey, string title)
        {
            Type genericType = typeof(BaseTable<>);
            Type constructedType = genericType.MakeGenericType(itemType);
            return (IBaseTable)Activator.CreateInstance(constructedType, name, primaryKey, title);
        }

        /// <summary>
        /// Gets a new instance of the table, similar to the specified.
        /// </summary>
        public static IBaseTable GetBaseTable(IBaseTable templateTable)
        {
            return GetBaseTable(templateTable.ItemType, 
                templateTable.Name, templateTable.PrimaryKey, templateTable.Title);
        }
    }
}
