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
 * Summary  : The configuration database and the basis of a configuration
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using Scada.Data.Entities;

namespace Scada.Admin.Project
{
    /// <summary>
    /// The configuration database and the basis of a configuration.
    /// <para>База данных конфигурации и основа конфигурации.</para>
    /// </summary>
    public class ConfigBase
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ConfigBase()
        {
            CommLineTable = new BaseTable<CommLine>("CommLine", "CommLineNum", CommonPhrases.CommLineTable);
            KPTable = new BaseTable<KP>("KP", "KPNum", CommonPhrases.KPTable);
            KPTypeTable = new BaseTable<KPType>("KPType", "KPTypeID", CommonPhrases.KPTypeTable);
            ObjTable = new BaseTable<Obj>("Obj", "ObjNum", CommonPhrases.ObjTable);
            BaseDir = "";
        }


        /// <summary>
        /// Gets the device table.
        /// </summary>
        public BaseTable<CommLine> CommLineTable { get; protected set; }

        /// <summary>
        /// Gets the device table.
        /// </summary>
        public BaseTable<KP> KPTable { get; protected set; }

        /// <summary>
        /// Gets the device type table.
        /// </summary>
        public BaseTable<KPType> KPTypeTable { get; protected set; }

        /// <summary>
        /// Gets the object (location) table.
        /// </summary>
        public BaseTable<Obj> ObjTable { get; protected set; }

        /// <summary>
        /// Gets or sets the directory of the configuration database files.
        /// </summary>
        public string BaseDir { get; set; }
    }
}
