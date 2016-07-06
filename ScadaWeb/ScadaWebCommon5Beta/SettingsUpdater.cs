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
 * Summary  : Updates settings on file change
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

using System;
using Utils;

namespace Scada.Web
{
    /// <summary>
    /// Updates settings on file change
    /// <para>Обновляет настройки при изменении файла</para>
    /// </summary>
    public class SettingsUpdater
    {
        /// <summary>
        /// Имя файла настроек
        /// </summary>
        protected readonly string fileName;
        /// <summary>
        /// Признак, что настройки необходимо создавать заново, если они изменились
        /// </summary>
        protected readonly bool recreate;
        /// <summary>
        /// Журнал
        /// </summary>
        protected readonly Log log;


        /// <summary>
        /// Конструктор, ограничивающий создание объекта без параметров
        /// </summary>
        protected SettingsUpdater()
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public SettingsUpdater(ISettings settings, string fileName, bool recreate, Log log = null)
        {
            if (settings == null)
                throw new ArgumentNullException("settings");
            if (fileName == null)
                throw new ArgumentNullException("fileName");

            this.fileName = fileName;
            this.recreate = recreate;
            this.log = log;

            Settings = settings;
            FileAge = DateTime.MinValue;
        }


        /// <summary>
        /// Получить обновляемые настройки
        /// </summary>
        public ISettings Settings { get; protected set; }

        /// <summary>
        /// Получить время изменения файла настроек при обновлении
        /// </summary>
        public DateTime FileAge { get; protected set; }


        /// <summary>
        /// Обновить настройки из файла, если файл изменился
        /// </summary>
        public bool Update(out bool changed, out string errMsg)
        {
            DateTime newFileAge = ScadaUtils.GetLastWriteTime(fileName);

            if (newFileAge > DateTime.MinValue && FileAge == newFileAge)
            {
                changed = false;
                errMsg = "";
                return true;
            }
            else
            {
                ISettings settings = recreate ? Settings.Create() : Settings;

                if (settings.LoadFromFile(fileName, out errMsg))
                {
                    FileAge = newFileAge;

                    if (recreate)
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
                    if (log != null)
                        log.WriteError(errMsg);

                    changed = false;
                    return false;
                }
            }
        }

        /// <summary>
        /// Обновить настройки из файла, если файл изменился
        /// </summary>
        public bool Update(out bool changed)
        {
            string errMsg;
            return Update(out changed, out errMsg);
        }

        /// <summary>
        /// Обновить настройки из файла, если файл изменился
        /// </summary>
        public bool Update(out string errMsg)
        {
            bool changed;
            return Update(out changed, out errMsg);
        }

        /// <summary>
        /// Обновить настройки из файла, если файл изменился
        /// </summary>
        public bool Update()
        {
            bool changed;
            string errMsg;
            return Update(out changed, out errMsg);
        }

        /// <summary>
        /// Сбросить время изменения файла настроек
        /// </summary>
        public void ResetFileAge()
        {
            FileAge = DateTime.MinValue;
        }
    }
}
