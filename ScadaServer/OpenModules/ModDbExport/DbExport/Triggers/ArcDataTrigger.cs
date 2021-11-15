/*
 * Copyright 2021 Mikhail Shiryaev
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
 * Module   : ModDbExport
 * Summary  : Represents an archive data trigger
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

using Scada.Db;
using Scada.Server.Modules.DbExport.Config;

namespace Scada.Server.Modules.DbExport.Triggers
{
    /// <summary>
    /// Represents an archive data trigger.
    /// <para>Представляет триггер на архивные данные.</para>
    /// </summary>
    internal class ArcDataTrigger : DataTrigger
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ArcDataTrigger(TriggerOptions triggerOptions, DataSource dataSource)
            : base(triggerOptions, dataSource)
        {
            ArcDataTriggerOptions = (ArcDataTriggerOptions)triggerOptions;
        }

        /// <summary>
        /// Gets the trigger options.
        /// </summary>
        public ArcDataTriggerOptions ArcDataTriggerOptions { get; }
    }
}
