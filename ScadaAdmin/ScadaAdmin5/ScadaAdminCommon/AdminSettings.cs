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
 * Summary  : The class contains utility methods for Administrator and its libraries
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

namespace Scada.Admin
{
    /// <summary>
    /// Administrator settings.
    /// <para>Настройки Администратора.</para>
    /// </summary>
    public class AdminSettings
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public AdminSettings()
        {
            // TODO: load settings from file
            if (ScadaUtils.IsRunningOnWin)
            {
                ServerDir = @"C:\SCADA\ScadaServer\";
                CommDir = @"C:\SCADA\ScadaComm\";
            }
            else
            {
                ServerDir = "/opt/scada/ScadaServer/";
                CommDir = "/opt/scada/ScadaComm/";
            }
        }


        /// <summary>
        /// Gets or sets the directory of the Server application.
        /// </summary>
        public string ServerDir { get; set; }

        /// <summary>
        /// Gets or sets the directory of the Communicator application.
        /// </summary>
        public string CommDir { get; set; }
    }
}
