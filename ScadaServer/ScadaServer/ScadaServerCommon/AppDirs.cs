/*
 * Copyright 2019 Mikhail Shiryaev
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
 * Module   : ScadaServerCommon
 * Summary  : Application directories
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2019
 */

using System.IO;

namespace Scada.Server
{
    /// <summary>
    /// Application directories.
    /// <para>Директории приложения.</para>
    /// </summary>
    public class AppDirs
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public AppDirs()
        {
            ExeDir = "";
            ConfigDir = "";
            LangDir = "";
            LogDir = "";
            ModDir = "";
            StorageDir = "";
        }


        /// <summary>
        /// Gets the directory of the executable file.
        /// </summary>
        public string ExeDir { get; protected set; }

        /// <summary>
        /// Gets the directory of the application configuration.
        /// </summary>
        public string ConfigDir { get; protected set; }

        /// <summary>
        /// Gets the directory of language files.
        /// </summary>
        public string LangDir { get; protected set; }

        /// <summary>
        /// Gets the directory of log files.
        /// </summary>
        public string LogDir { get; protected set; }

        /// <summary>
        /// Gets the modules directory.
        /// </summary>
        public string ModDir { get; protected set; }

        /// <summary>
        /// Gets the storage directory.
        /// </summary>
        public string StorageDir { get; protected set; }

        /// <summary>
        /// Checks that the directories exist.
        /// </summary>
        public bool Exist
        {
            get
            {
                foreach (string dir in GetRequiredDirs())
                {
                    if (!Directory.Exists(dir))
                        return false;
                }

                return true;
            }
        }


        /// <summary>
        /// Initializes the directories based on the directory of the executable file.
        /// </summary>
        public void Init(string exeDir)
        {
            ExeDir = ScadaUtils.NormalDir(exeDir);
            ConfigDir = ExeDir + "Config" + Path.DirectorySeparatorChar;
            LangDir = ExeDir + "Lang" + Path.DirectorySeparatorChar;
            LogDir = ExeDir + "Log" + Path.DirectorySeparatorChar;
            ModDir = ExeDir + "Mod" + Path.DirectorySeparatorChar;
            StorageDir = ExeDir + "Storage" + Path.DirectorySeparatorChar;
        }

        /// <summary>
        /// Gets the directories required for Server.
        /// </summary>
        public string[] GetRequiredDirs()
        {
            return new string[] { ConfigDir, LangDir, LogDir, ModDir, StorageDir };
        }
    }
}
