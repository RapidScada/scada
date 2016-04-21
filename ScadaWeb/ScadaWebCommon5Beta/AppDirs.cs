/*
 * Copyright 2016 Mikhail Shiryaev
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
 * Module   : ScadaWebCommon
 * Summary  : Web application directories
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

using System.IO;

namespace Scada.Web
{
    /// <summary>
    /// Web application directories
    /// <para>Директории веб-приложения</para>
    /// </summary>
    public class AppDirs
    {
        /// <summary>
        /// Директория веб-приложения по умолчанию
        /// </summary>
        public const string DefWebAppDir = @"C:\SCADA\ScadaWeb\";


        /// <summary>
        /// Конструктор
        /// </summary>
        public AppDirs()
        {
            Init(DefWebAppDir);
        }


        /// <summary>
        /// Получить директорию веб-приложения
        /// </summary>
        public string WebAppDir { get; protected set; }

        /// <summary>
        /// Получить директорию исполняемых файлов
        /// </summary>
        public string BinDir { get; protected set; }

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
        /// Получить директорию плагинов
        /// </summary>
        public string PluginsDir { get; protected set; }

        /// <summary>
        /// Получить директорию хранилища приложения
        /// </summary>
        public string StorageDir { get; protected set; }


        /// <summary>
        /// Инициализировать директории относительно директории веб-приложения
        /// </summary>
        public void Init(string webAppDir)
        {
            WebAppDir = ScadaUtils.NormalDir(webAppDir);
            BinDir = WebAppDir + "bin" + Path.DirectorySeparatorChar;
            ConfigDir = WebAppDir + "config" + Path.DirectorySeparatorChar;
            LangDir = WebAppDir + "lang" + Path.DirectorySeparatorChar;
            LogDir = WebAppDir + "log" + Path.DirectorySeparatorChar;
            PluginsDir = WebAppDir + "plugins" + Path.DirectorySeparatorChar;
            StorageDir = WebAppDir + "storage" + Path.DirectorySeparatorChar;
        }
    }
}
