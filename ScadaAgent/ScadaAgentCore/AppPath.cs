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
 * Module   : ScadaAgentCore
 * Summary  : Path relative to an application
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

namespace Scada.Agent
{
    /// <summary>
    /// Path relative to an application
    /// <para>Путь относительно приложения</para>
    /// </summary>
    public class AppPath
    {
        /// <summary>
        /// Путь базы конфигурации
        /// </summary>
        public static AppPath Base = new AppPath(ScadaApps.None, AppFolder.Config, "");


        /// <summary>
        /// Конструктор
        /// </summary>
        public AppPath() 
            : this(ScadaApps.None, AppFolder.Undef, "")
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public AppPath(ScadaApps scadaApp, AppFolder appFolder, string path)
        {
            ScadaApp = scadaApp;
            AppFolder = appFolder;
            Path = path ?? "";
        }


        /// <summary>
        /// Получить или установить приложение
        /// </summary>
        public ScadaApps ScadaApp { get; set; }

        /// <summary>
        /// Получить или установить папку приложения
        /// </summary>
        public AppFolder AppFolder { get; set; }

        /// <summary>
        /// Получить или установить путь относительно папки приложения
        /// </summary>
        public string Path { get; set; }
    }
}
