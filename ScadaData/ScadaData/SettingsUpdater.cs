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
 * Module   : ScadaData
 * Summary  : Updates settings on file change
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

using System;

namespace Scada
{
    /// <summary>
    /// Updates settings on file change
    /// <para>Обновляет настройки при изменении файла</para>
    /// </summary>
    public class SettingsUpdater
    {
        /// <summary>
        /// Конструктор, ограничивающий создание объекта без параметров
        /// </summary>
        protected SettingsUpdater()
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public SettingsUpdater(ISettings settings, string fileName, bool recreate)
        {
            if (settings == null)
                throw new ArgumentNullException("settings");
            if (fileName == null)
                throw new ArgumentNullException("fileName");

            Settings = settings;
            FileName = fileName;
            Recreate = recreate;
            FileAge = DateTime.MinValue;
        }


        /// <summary>
        /// Получить обновляемые настройки
        /// </summary>
        public ISettings Settings { get; protected set; }

        /// <summary>
        /// Получить имя файла настроек
        /// </summary>
        public string FileName { get; protected set; }

        /// <summary>
        /// Получить признак, что настройки необходимо создавать заново, если они изменились
        /// </summary>
        public bool Recreate { get; protected set; }

        /// <summary>
        /// Получить время изменения файла настроек при обновлении
        /// </summary>
        public DateTime FileAge { get; protected set; }


        /// <summary>
        /// Обновить настройки из файла, если файл изменился
        /// </summary>
        public bool Update(out bool changed, out string errMsg)
        {
            DateTime newFileAge = ScadaUtils.GetLastWriteTime(FileName);

            if (FileAge == newFileAge)
            {
                changed = false;
                errMsg = "";
                return true;
            }
            else
            {
                ISettings settings = Recreate ? Settings.Create() : Settings;

                if (settings.LoadFromFile(FileName, out errMsg))
                {
                    FileAge = newFileAge;

                    if (Recreate)
                    {
                        changed = !Settings.Equals(settings);
                        Settings = settings;
                    }
                    else
                    {
                        changed = true;
                    }

                    return true;
                }
                else
                {
                    changed = false;
                    return false;
                }
            }
        }

        /// <summary>
        /// Обновить настройки из файла, если файл изменился
        /// </summary>
        public bool Update(out string errMsg)
        {
            bool changed;
            return Update(out changed, out errMsg);
        }
    }
}
