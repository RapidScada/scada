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
 * Module   : SCADA-Administrator
 * Summary  : Application directories
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using Scada;
using System.IO;

namespace ScadaAdmin
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
        /// Инициализировать директории на основе директории исполняемого файла приложения
        /// </summary>
        public void Init(string exeDir)
        {
            ExeDir = ScadaUtils.NormalDir(exeDir);
            ConfigDir = ExeDir + "Config" + Path.DirectorySeparatorChar;
            LangDir = ExeDir + "Lang" + Path.DirectorySeparatorChar;
            LogDir = ExeDir + "Log" + Path.DirectorySeparatorChar;
        }
    }
}
