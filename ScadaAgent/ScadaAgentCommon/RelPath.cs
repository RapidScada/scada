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
 * Summary  : Relative path
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

namespace Scada.Agent
{
    /// <summary>
    /// Relative path
    /// <para>Относительный путь</para>
    /// </summary>
    public class RelPath
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public RelPath() 
            : this(ConfigParts.None, AppFolder.Root, "")
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public RelPath(ConfigParts configPart, AppFolder appFolder, string path = "")
        {
            ConfigPart = configPart;
            AppFolder = appFolder;
            Path = path ?? "";
        }


        /// <summary>
        /// Получить или установить часть конфигурации
        /// </summary>
        public ConfigParts ConfigPart { get; set; }

        /// <summary>
        /// Получить или установить папку приложения
        /// </summary>
        public AppFolder AppFolder { get; set; }

        /// <summary>
        /// Получить или установить путь относительно папки приложения
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Получить признак, что путь является маской для поиска файлов
        /// </summary>
        public bool IsMask
        {
            get
            {
                return Path != null && (Path.IndexOf('*') >= 0 || Path.IndexOf('?') >= 0);
            }
        }
    }
}
