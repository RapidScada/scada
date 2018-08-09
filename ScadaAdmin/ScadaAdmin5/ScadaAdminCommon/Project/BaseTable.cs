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
 * Module   : ScadaAdminCommon
 * Summary  : Represents the table of the configuration database
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace Scada.Admin.Project
{
    /// <summary>
    /// Represents the table of the configuration database.
    /// <para>Представляет таблицу базы конфигурации.</para>
    /// </summary>
    public class BaseTable<T>
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public BaseTable()
            : this("", "")
        {
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public BaseTable(string name, string title)
        {
            Name = name ?? "";
            Title = title ?? "";
            Items = new List<T>();
        }


        /// <summary>
        /// Gets or sets the table name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the table title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets the type of the table items.
        /// </summary>
        public Type ItemType
        {
            get
            {
                return typeof(T);
            }
        }

        /// <summary>
        /// Gets the table items.
        /// </summary>
        public List<T> Items { get; protected set; }


        /// <summary>
        /// Saves the table to the specified file.
        /// </summary>
        public void Save(string fileName)
        {
            XmlSerializer serializer = new XmlSerializer(Items.GetType());
            XmlWriterSettings writerSettings = new XmlWriterSettings() { Indent = true };

            using (XmlWriter writer = XmlWriter.Create(fileName, writerSettings))
            {
                serializer.Serialize(writer, Items);
            }
        }
    }
}
