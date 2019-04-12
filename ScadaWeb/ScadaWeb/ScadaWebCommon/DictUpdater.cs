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
 * Module   : ScadaWebCommon
 * Summary  : Updates dictionary on file change
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2019
 */

using System;
using Utils;

namespace Scada.Web
{
    /// <summary>
    /// Updates dictionary on file change.
    /// <para>Обновляет словарь при изменении файла.</para>
    /// </summary>
    public class DictUpdater
    {
        /// <summary>
        /// Имя файла словаря
        /// </summary>
        protected readonly string fileName;
        /// <summary>
        /// Имя файла словаря по умолчанию
        /// </summary>
        protected readonly string defaultFileName;
        /// <summary>
        /// Ссылка на метод инициализации, которые берутся из загруженного словаря
        /// </summary>
        protected Action initPhrasesAction;
        /// <summary>
        /// Журнал
        /// </summary>
        protected readonly ILog log;
        /// <summary>
        /// Первое обновление настроек
        /// </summary>
        protected bool initialUpdate;


        /// <summary>
        /// Конструктор, ограничивающий создание объекта без параметров
        /// </summary>
        protected DictUpdater()
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public DictUpdater(string directory, string fileNamePrefix, Action initPhrasesAction, ILog log = null)
        {
            if (directory == null)
                throw new ArgumentNullException("directory");
            if (fileNamePrefix == null)
                throw new ArgumentNullException("fileNamePrefix");

            fileName = Localization.GetDictionaryFileName(directory, fileNamePrefix);
            defaultFileName = Localization.Dict.GetFileName(directory, fileNamePrefix, Localization.DefaultCultureName);
            this.initPhrasesAction = initPhrasesAction;
            this.log = log;
            initialUpdate = true;

            FileAge = DateTime.MinValue;
        }


        /// <summary>
        /// Получить время изменения файла словаря при обновлении
        /// </summary>
        public DateTime FileAge { get; protected set; }


        /// <summary>
        /// Обновить словарь из файла, если файл изменился
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
            else if (Localization.LoadDictionaries(fileName, out errMsg))
            {
                initPhrasesAction?.Invoke();
                initialUpdate = false;
                FileAge = newFileAge;
                changed = true;
                return true;
            }
            else
            {
                // вывод ошибки в журнал
                log?.WriteError(errMsg);

                // загрузка словаря по умолчанию
                if (initialUpdate)
                {
                    initialUpdate = false;
                    Localization.LoadDictionaries(defaultFileName, out string errMsg2);
                    initPhrasesAction?.Invoke();
                }

                changed = false;
                return false;
            }
        }

        /// <summary>
        /// Обновить словарь из файла, если файл изменился
        /// </summary>
        public bool Update(out bool changed)
        {
            string errMsg;
            return Update(out changed, out errMsg);
        }

        /// <summary>
        /// Обновить словарь из файла, если файл изменился
        /// </summary>
        public bool Update(out string errMsg)
        {
            bool changed;
            return Update(out changed, out errMsg);
        }

        /// <summary>
        /// Обновить словарь из файла, если файл изменился
        /// </summary>
        public bool Update()
        {
            bool changed;
            string errMsg;
            return Update(out changed, out errMsg);
        }
    }
}
