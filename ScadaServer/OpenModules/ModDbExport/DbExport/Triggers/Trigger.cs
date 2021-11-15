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
 * Summary  : Represents a trigger
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

using Scada.Db;
using Scada.Server.Modules.DbExport.Config;
using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Scada.Server.Modules.DbExport.Triggers
{
    /// <summary>
    /// Represents a trigger.
    /// <para>Представляет триггер.</para>
    /// </summary>
    internal abstract class Trigger
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public Trigger(TriggerOptions triggerOptions, DataSource dataSource)
        {
            DataSource = dataSource ?? throw new ArgumentNullException(nameof(dataSource));
            Options = triggerOptions ?? throw new ArgumentNullException(nameof(triggerOptions));
            CnlNums = new HashSet<int>(Options.CnlNums);
            DeviceNums = new HashSet<int>(Options.DeviceNums);
            Command = dataSource.CreateCommand();
            Command.CommandText = triggerOptions.Query;
            InitParams();
        }


        /// <summary>
        /// Gets the data source.
        /// </summary>
        protected DataSource DataSource { get; }

        /// <summary>
        /// Gets the trigger options.
        /// </summary>
        public TriggerOptions Options { get; }

        /// <summary>
        /// Gets the input channel numbers as a set.
        /// </summary>
        public HashSet<int> CnlNums { get; }

        /// <summary>
        /// Gets the device numbers as a set.
        /// </summary>
        public HashSet<int> DeviceNums { get; private set; }

        /// <summary>
        /// Gets or sets the database command corresponding to the trigger query.
        /// </summary>
        public DbCommand Command { get; set; }


        /// <summary>
        /// Initializes the command parameters.
        /// </summary>
        protected virtual void InitParams()
        {
        }
    }
}
