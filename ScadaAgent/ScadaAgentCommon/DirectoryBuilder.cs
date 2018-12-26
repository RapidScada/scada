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
 * Module   : ScadaAgentCommon
 * Summary  : Builds configuration directories
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using System;
using System.IO;

namespace Scada.Agent
{
    /// <summary>
    /// Builds configuration directories.
    /// <para>Строит директории конфигурации.</para>
    /// </summary>
    public static class DirectoryBuilder
    {
        /// <summary>
        /// Gets a directory corresponding to the configuration part.
        /// </summary>
        public static string GetDirectory(ConfigParts configPart)
        {
            return GetDirectory(configPart, null);
        }

        /// <summary>
        /// Gets a directory corresponding to the configuration part.
        /// </summary>
        public static string GetDirectory(ConfigParts configPart, char? directorySeparator)
        {
            switch (configPart)
            {
                case ConfigParts.Base:
                    return "BaseDAT" + directorySeparator;
                case ConfigParts.Interface:
                    return "Interface" + directorySeparator;
                case ConfigParts.Server:
                    return "ScadaServer" + directorySeparator;
                case ConfigParts.Comm:
                    return "ScadaComm" + directorySeparator;
                case ConfigParts.Web:
                    return "ScadaWeb" + directorySeparator;
                default:
                    throw new ArgumentException("Unknown configuration part.");
            }
        }

        /// <summary>
        /// Gets a directory corresponding to the application folder.
        /// </summary>
        public static string GetDirectory(AppFolder appFolder, bool lowerCase = false)
        {
            return GetDirectory(appFolder, null, lowerCase);
        }

        /// <summary>
        /// Gets a directory corresponding to the application folder.
        /// </summary>
        public static string GetDirectory(AppFolder appFolder, char? directorySeparator, bool lowerCase = false)
        {
            string dir;

            switch (appFolder)
            {
                case AppFolder.Root:
                    dir = "";
                    break;
                case AppFolder.Config:
                    dir = "Config" + directorySeparator;
                    break;
                case AppFolder.Log:
                    dir = "Log" + directorySeparator;
                    break;
                case AppFolder.Storage:
                    dir = "Storage" + directorySeparator;
                    break;
                default:
                    throw new ArgumentException("Unknown application folder.");
            }

            return lowerCase ? dir.ToLowerInvariant() : dir;
        }

        /// <summary>
        /// Gets a directory corresponding to the configuration part and application folder.
        /// </summary>
        public static string GetDirectory(ConfigParts configPart, AppFolder appFolder)
        {
            return Path.Combine(GetDirectory(configPart), GetDirectory(appFolder, configPart == ConfigParts.Web));
        }

        /// <summary>
        /// Gets a directory corresponding to the configuration part and application folder.
        /// </summary>
        public static string GetDirectory(ConfigParts configPart, AppFolder appFolder, char directorySeparator)
        {
            return GetDirectory(configPart, directorySeparator) +
                GetDirectory(appFolder, directorySeparator, configPart == ConfigParts.Web);
        }

        /// <summary>
        /// Gets a directory corresponding to the service application.
        /// </summary>
        public static string GetDirectory(ServiceApp serviceApp)
        {
            switch (serviceApp)
            {
                case ServiceApp.Server:
                    return GetDirectory(ConfigParts.Server);
                case ServiceApp.Comm:
                    return GetDirectory(ConfigParts.Comm);
                default:
                    throw new ArgumentException("Unknown service application.");
            }
        }
    }
}
