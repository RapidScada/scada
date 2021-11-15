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
 * Summary  : Represents an event trigger
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

using Scada.Db;
using Scada.Server.Modules.DbExport.Config;
using System;

namespace Scada.Server.Modules.DbExport.Triggers
{
    /// <summary>
    /// Represents an event trigger.
    /// <para>Представляет триггер на событие.</para>
    /// </summary>
    internal class EventTrigger : Trigger
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public EventTrigger(TriggerOptions triggerOptions, DataSource dataSource)
            : base(triggerOptions, dataSource)
        {
            EventTriggerOptions = (EventTriggerOptions)triggerOptions;
        }

        /// <summary>
        /// Gets the trigger options.
        /// </summary>
        public EventTriggerOptions EventTriggerOptions { get; }

        /// <summary>
        /// Initializes the command parameters.
        /// </summary>
        protected override void InitParams()
        {
            DataSource.SetParam(Command, "dateTime", DateTime.MinValue);
            DataSource.SetParam(Command, "objNum", 0);
            DataSource.SetParam(Command, "kpNum", 0);
            DataSource.SetParam(Command, "paramID", 0);
            DataSource.SetParam(Command, "cnlNum", 0);
            DataSource.SetParam(Command, "oldCnlVal", 0.0);
            DataSource.SetParam(Command, "oldCnlStat", 0);
            DataSource.SetParam(Command, "newCnlVal", 0.0);
            DataSource.SetParam(Command, "newCnlStat", 0);
            DataSource.SetParam(Command, "checked", false);
            DataSource.SetParam(Command, "userID", 0);
            DataSource.SetParam(Command, "descr", "");
            DataSource.SetParam(Command, "data", "");
        }
    }
}
