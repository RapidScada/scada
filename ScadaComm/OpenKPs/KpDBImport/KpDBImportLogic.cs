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
 * Module   : KpDBImport
 * Summary  : Device communication logic
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using Scada.Comm.Devices.DbImport.Configuration;
using Scada.Comm.Devices.DbImport.Data;
using System;
using System.Text;
using System.Threading;

namespace Scada.Comm.Devices
{
    /// <summary>
    /// Device communication logic.
    /// <para>Логика работы КП.</para>
    /// </summary>
    public class KpDbImportLogic : KPLogic
    {
        private DataSource dataSource; // the data source


        /// <summary>
        /// Performs a communication session with the device.
        /// </summary>
        public override void Session()
        {
            if (dataSource == null)
            {
                WriteToLog(Localization.UseRussian ?
                    "Нормальное взаимодействие с КП невозможно, т.к. источник данных не определён" :
                    "Normal device communication is impossible because data source is undefined");
                Thread.Sleep(ReqParams.Delay);
                lastCommSucc = false;
            }
            else
            {
                Thread.Sleep(ReqParams.Delay);
            }

            // calculate session stats
            CalcSessStats();
        }

        /// <summary>
        /// Performs actions after adding the device to communication line.
        /// </summary>
        public override void OnAddedToCommLine()
        {
            // load configuration
            Config config = new Config();

            if (config.Load(Config.GetFileName(AppDirs.ConfigDir, Number), out string errMsg))
            {

            }
            else
            {
                dataSource = null;
                WriteToLog(errMsg);
            }
        }

        /// <summary>
        /// Performs actions on communication line start.
        /// </summary>
        public override void OnCommLineStart()
        {
        }
    }
}
