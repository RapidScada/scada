/*
 * Copyright 2017 Mikhail Shiryaev
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
 * Module   : ScadaSchemeCommon
 * Summary  : Single scheme change
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2017
 */

using Scada.Scheme.Model;

namespace Scada.Scheme.Editor
{
    /// <summary>
    /// Single scheme change
    /// <para>Одно изменение схемы</para>
    /// </summary>
    internal class Change
    {
        /// <summary>
        /// Конструктор, ограничивающий создание объекта без параметров
        /// </summary>
        protected Change()
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public Change(SchemeChangeTypes changeType)
        {
            ChangeType = changeType;
            Stamp = 0;
            ChangedObject = null;
            DeletedComponentID = -1;
            OldImageName = "";
            ImageName = "";
        }


        /// <summary>
        /// Получить тип изменения схемы
        /// </summary>
        public SchemeChangeTypes ChangeType { get; private set; }

        /// <summary>
        /// Получить или установить уникальную метку изменения в пределах открытой схемы
        /// </summary>
        /// <remarks>Каждая следующая метка больше, чем предыдущая</remarks>
        public long Stamp { get; set; }

        /// <summary>
        /// Получить или установить изменившийся объект
        /// </summary>
        public object ChangedObject { get; set; }

        /// <summary>
        /// Получить или установить ид. удалённого компонента схемы
        /// </summary>
        public int DeletedComponentID { get; set; }

        /// <summary>
        /// Получить или установить старое наименование изображения в случае переименования
        /// </summary>
        public string OldImageName { get; set; }

        /// <summary>
        /// Получить или установить новое наименование изображения в случае переименования или 
        /// наименование удалённого изображения
        /// </summary>
        public string ImageName { get; set; }
    }
}
