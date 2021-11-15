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
 * Summary  : Stores triggers separated by classes
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

using Scada.Db;
using Scada.Server.Modules.DbExport.Config;
using System;
using System.Collections.Generic;

namespace Scada.Server.Modules.DbExport.Triggers
{
    /// <summary>
    /// Stores triggers separated by classes.
    /// <para>Хранит триггеры, разделённые на классы.</para>
    /// </summary>
    internal class ClassifiedTriggers
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ClassifiedTriggers(List<TriggerOptions> triggersOptionsList, DataSource dataSource)
        {
            if (triggersOptionsList == null)
                throw new ArgumentNullException(nameof(triggersOptionsList));
            if (dataSource == null)
                throw new ArgumentNullException(nameof(dataSource));

            CurDataTriggers = new List<CurDataTrigger>();
            ArcDataTriggers = new List<ArcDataTrigger>();
            EventTriggers = new List<EventTrigger>();
            CreateTriggers(triggersOptionsList, dataSource);
        }


        /// <summary>
        /// Gets the current data triggers.
        /// </summary>
        public List<CurDataTrigger> CurDataTriggers { get; private set; }

        /// <summary>
        /// Gets the archive data triggers.
        /// </summary>
        public List<ArcDataTrigger> ArcDataTriggers { get; private set; }

        /// <summary>
        /// Gets the event triggers.
        /// </summary>
        public List<EventTrigger> EventTriggers { get; private set; }


        /// <summary>
        /// Creates the triggers according to the trigger options.
        /// </summary>
        private void CreateTriggers(List<TriggerOptions> triggersOptionsList, DataSource dataSource)
        {
            foreach (TriggerOptions triggerOptions in triggersOptionsList)
            {
                if (triggerOptions.Active)
                {
                    if (triggerOptions is CurDataTriggerOptions options1)
                        CurDataTriggers.Add(new CurDataTrigger(options1, dataSource));
                    else if (triggerOptions is ArcDataTriggerOptions options2)
                        ArcDataTriggers.Add(new ArcDataTrigger(options2, dataSource));
                    else if (triggerOptions is EventTriggerOptions options3)
                        EventTriggers.Add(new EventTrigger(options3, dataSource));
                }
            }
        }
    }
}
