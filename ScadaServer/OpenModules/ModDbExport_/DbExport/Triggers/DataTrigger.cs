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
 * Summary  : Represents a data trigger
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

using Scada.Db;
using Scada.Server.Modules.DbExport.Config;
using System;
using System.Data.Common;

namespace Scada.Server.Modules.DbExport.Triggers
{
    /// <summary>
    /// Represents a data trigger.
    /// <para>Представляет триггер на данные.</para>
    /// </summary>
    internal abstract class DataTrigger : Trigger
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DataTrigger(TriggerOptions triggerOptions, DataSource dataSource)
            : base(triggerOptions, dataSource)
        {
            DataTriggerOptions = (DataTriggerOptions)triggerOptions;
        }


        /// <summary>
        /// Gets the trigger options.
        /// </summary>
        public DataTriggerOptions DataTriggerOptions { get; }

        /// <summary>
        /// Gets the command parameter that represents date and time.
        /// </summary>
        public DbParameter DateTimeParam { get; protected set; }

        /// <summary>
        /// Gets the command parameter that represents a device number.
        /// </summary>
        public DbParameter KpNumParam { get; protected set; }

        /// <summary>
        /// Gets the command parameter that represents a channel number.
        /// </summary>
        public DbParameter CnlNumParam { get; protected set; }

        /// <summary>
        /// Gets the command parameter that represents a value.
        /// </summary>
        public DbParameter ValParam { get; protected set; }

        /// <summary>
        /// Gets the command parameter that represents a status.
        /// </summary>
        public DbParameter StatParam { get; protected set; }


        /// <summary>
        /// Initializes the command parameters.
        /// </summary>
        protected override void InitParams()
        {
            DataTriggerOptions options = (DataTriggerOptions)Options;
            DateTimeParam = DataSource.SetParam(Command, "dateTime", DateTime.MinValue);
            KpNumParam = DataSource.SetParam(Command, "kpNum", 0);

            if (options.SingleQuery)
            {
                foreach (int cnlNum in options.CnlNums)
                {
                    string cnlNumStr = cnlNum.ToString();
                    DataSource.SetParam(Command, "val" + cnlNumStr, 0.0);
                    DataSource.SetParam(Command, "stat" + cnlNumStr, 0);
                }
            }
            else
            {
                CnlNumParam = DataSource.SetParam(Command, "cnlNum", 0);
                ValParam = DataSource.SetParam(Command, "val", 0.0);
                StatParam = DataSource.SetParam(Command, "stat", 0);
            }
        }

        /// <summary>
        /// Sets the command parameter that represents a value of the specified channel number.
        /// </summary>
        public void SetValParam(int cnlNum, double val)
        {
            DataSource.SetParam(Command, "val" + cnlNum, val);
        }

        /// <summary>
        /// Sets the command parameter that represents a status of the specified channel number.
        /// </summary>
        public void SetStatParam(int cnlNum, int stat)
        {
            DataSource.SetParam(Command, "stat" + cnlNum, stat);
        }
    }
}
