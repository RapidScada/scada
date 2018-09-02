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
 * Summary  : Represents the Server application
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using System.IO;

namespace Scada.Admin.Project
{
    /// <summary>
    /// Represents the Server application.
    /// <para>Представляет приложение Сервер.</para>
    /// </summary>
    public class ServerApp : ScadaApp
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ServerApp()
            : base()
        {
            Settings = new Server.Settings { CreateBakFile = false };
        }


        /// <summary>
        /// Gets the settings of the application.
        /// </summary>
        public Server.Settings Settings { get; protected set; }


        /// <summary>
        /// Gets the directory of the application configuration.
        /// </summary>
        public override string GetConfigDir(string parentDir)
        {
            return Path.Combine(parentDir, "ScadaServer", "Config");
        }

        /// <summary>
        /// Gets the full file name of the application settings.
        /// </summary>
        public override string GetSettingsPath(string parentDir)
        {
            return Path.Combine(GetConfigDir(parentDir), Server.Settings.DefFileName);
        }
    }
}
