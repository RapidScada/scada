/*
 * Copyright 2020 Mikhail Shiryaev
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
 * Module   : ScadaCommCommon
 * Summary  : Application directories
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2020
 */

using System.IO;

namespace Scada.Comm
{
    /// <summary>
    /// Application directories
    /// <para>Директории приложения</para>
    /// </summary>
    public class AppDirs
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public AppDirs()
        {
            ExeDir = "";
            CmdDir = "";
            ConfigDir = "";
            KPDir = "";
            LangDir = "";
            LogDir = "";
            StorageDir = "";
        }


        /// <summary>
        /// Получить директорию исполняемого файла
        /// </summary>
        public string ExeDir { get; protected set; }

        /// <summary>
        /// Получить директорию команд
        /// </summary>
        public string CmdDir { get; protected set; }

        /// <summary>
        /// Получить директорию конфигурации
        /// </summary>
        public string ConfigDir { get; protected set; }

        /// <summary>
        /// Получить директорию библиотек КП
        /// </summary>
        public string KPDir { get; protected set; }

        /// <summary>
        /// Получить директорию языковых файлов
        /// </summary>
        public string LangDir { get; protected set; }
        
        /// <summary>
        /// Получить директорию журналов
        /// </summary>
        public string LogDir { get; protected set; }
        
        /// <summary>
        /// Gets the storage directory.
        /// </summary>
        public string StorageDir { get; protected set; }

        /// <summary>
        /// Проверить существование директорий
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
        /// Инициализировать директории на основе директории исполняемого файла приложения
        /// </summary>
        public void Init(string exeDir)
        {
            ExeDir = ScadaUtils.NormalDir(exeDir);
            CmdDir = ExeDir + "Cmd" + Path.DirectorySeparatorChar;
            ConfigDir = ExeDir + "Config" + Path.DirectorySeparatorChar;
            KPDir = ExeDir + "KP" + Path.DirectorySeparatorChar;
            LangDir = ExeDir + "Lang" + Path.DirectorySeparatorChar;
            LogDir = ExeDir + "Log" + Path.DirectorySeparatorChar;
            StorageDir = ExeDir + "Storage" + Path.DirectorySeparatorChar;
        }

        /// <summary>
        /// Получить необходимые директории
        /// </summary>
        public string[] GetRequiredDirs()
        {
            return new string[] { CmdDir, ConfigDir, KPDir, LangDir, LogDir, StorageDir };
        }
    }
}
