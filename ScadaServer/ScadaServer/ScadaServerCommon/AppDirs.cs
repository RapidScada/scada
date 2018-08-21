/*
 * Copyright 2015 Mikhail Shiryaev
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
 * Modified : 2015
 */

using System.IO;

namespace Scada.Server
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
            ConfigDir = "";
            LangDir = "";
            LogDir = "";
            ModDir = "";
        }


        /// <summary>
        /// Получить директорию исполняемого файла
        /// </summary>
        public string ExeDir { get; protected set; }
        
        /// <summary>
        /// Получить директорию конфигурации
        /// </summary>
        public string ConfigDir { get; protected set; }
        
        /// <summary>
        /// Получить директорию языковых файлов
        /// </summary>
        public string LangDir { get; protected set; }
        
        /// <summary>
        /// Получить директорию журналов
        /// </summary>
        public string LogDir { get; protected set; }

        /// <summary>
        /// Получить директорию подключаемых модулей
        /// </summary>
        public string ModDir { get; protected set; }


        /// <summary>
        /// Инициализировать директории на основе директории исполняемого файла приложения
        /// </summary>
        public void Init(string exeDir)
        {
            ExeDir = ScadaUtils.NormalDir(exeDir);
            ConfigDir = ExeDir + "Config" + Path.DirectorySeparatorChar;
            LangDir = ExeDir + "Lang" + Path.DirectorySeparatorChar;
            LogDir = ExeDir + "Log" + Path.DirectorySeparatorChar;
            ModDir = ExeDir + "Mod" + Path.DirectorySeparatorChar;
        }
    }
}
