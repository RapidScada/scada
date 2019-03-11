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
 * Module   : ScadaCommCommon
 * Summary  : Subset of the configuration database used by Communicator
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
 * Modified : 2019
 */

using Scada.Data.Entities;
using Scada.Data.Tables;

namespace Scada.Comm
{
    /// <summary>
    /// Subset of the configuration database used by Communicator.
    /// <para>Подмножество базы конфигурации, используемое Коммуникатором.</para>
    /// </summary>
    public class ConfigBaseSubset
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ConfigBaseSubset()
        {
            InCnlTable = new BaseTable<InCnl>("InCnl", "CnlNum", CommonPhrases.InCnlTable);
            InCnlTable.AddIndex("ObjNum");
            InCnlTable.AddIndex("KPNum");

            KPTable = new BaseTable<KP>("KP", "KPNum", CommonPhrases.KPTable);
            KPTable.AddIndex("KPTypeID");
            KPTable.AddIndex("CommLineNum");
        }


        /// <summary>
        /// Gets the input channel table.
        /// </summary>
        public BaseTable<InCnl> InCnlTable { get; protected set; }

        /// <summary>
        /// Gets the device table.
        /// </summary>
        public BaseTable<KP> KPTable { get; protected set; }
    }
}
