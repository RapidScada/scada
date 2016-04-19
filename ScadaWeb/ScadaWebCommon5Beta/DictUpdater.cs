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
 * Summary  : Updates dictionary on file change
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
    /// Updates dictionary on file change
    /// <para>Обновляет словарь при изменении файла</para>
    /// </summary>
    public class DictUpdater
    {
        /// <summary>
        /// Имя файла словаря
        /// </summary>
        protected readonly string fileName;
        /// <summary>
        /// Ссылка на метод инициализации, которые берутся из загруженного словаря
        /// </summary>
        protected Action initPhrasesAction;
        /// <summary>
        /// Журнал
        /// </summary>
        protected readonly Log log;


        /// <summary>
        /// Конструктор, ограничивающий создание объекта без параметров
        /// </summary>
        protected DictUpdater()
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public DictUpdater(string directory, string fileNamePrefix, Action initPhrasesAction, Log log = null)
        {
            if (directory == null)
                throw new ArgumentNullException("directory");
            if (fileNamePrefix == null)
                throw new ArgumentNullException("fileNamePrefix");

            fileName = Localization.GetDictionaryFileName(directory, fileNamePrefix);
            this.initPhrasesAction = initPhrasesAction;
            this.log = log;

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

            if (FileAge == newFileAge)
            {
                changed = false;
                errMsg = "";
                return true;
            }
            else if (Localization.LoadDictionaries(fileName, out errMsg))
            {
                if (initPhrasesAction != null)
                    initPhrasesAction();

                FileAge = newFileAge;
                changed = true;
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
