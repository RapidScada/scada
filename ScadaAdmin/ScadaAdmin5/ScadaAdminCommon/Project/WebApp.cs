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
 * Summary  : Represents the Webstation application
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using Scada.Web;
using System.IO;

namespace Scada.Admin.Project
{
    /// <summary>
    /// Represents the Webstation application
    /// <para>Представляет приложение Вебстанция</para>
    /// </summary>
    public class WebApp : ScadaApp
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public WebApp()
            : base()
        {
            Settings = new WebSettings();
        }


        /// <summary>
        /// Gets the settings of the application
        /// </summary>
        public WebSettings Settings { get; protected set; }


        /// <summary>
        /// Gets the directory of the application configuration.
        /// </summary>
        public override string GetConfigDir(string parentDir)
        {
            return Path.Combine(parentDir, "ScadaWeb", "config");
        }

        /// <summary>
        /// Gets the full file name of the application settings.
        /// </summary>
        public override string GetSettingsPath(string parentDir)
        {
            return Path.Combine(GetConfigDir(parentDir), WebSettings.DefFileName);
        }
    }
}
