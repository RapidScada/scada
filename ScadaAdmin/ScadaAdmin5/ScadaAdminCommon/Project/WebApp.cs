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
        }


        /// <summary>
        /// Gets the directory of the application configuration.
        /// </summary>
        public string GetConfigDir()
        {
            return Path.Combine(AppDir, "config");
        }

        /// <summary>
        /// Gets the directory of the application storage.
        /// </summary>
        public string GetStorageDir()
        {
            return Path.Combine(AppDir, "storage");
        }

        /// <summary>
        /// Gets the directory of the application.
        /// </summary>
        public static string GetAppDir(string parentDir)
        {
            return Path.Combine(parentDir, "ScadaWeb");
        }
    }
}
