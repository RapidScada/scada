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
 * Module   : Scheme Editor
 * Summary  : The class for transfer scheme changes
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2017
 */

using Scada.Scheme.DataTransfer;
using System.Collections.Generic;

namespace Scada.Scheme.Editor
{
    /// <summary>
    /// The class for transfer scheme changes
    /// <para>Класс для передачи изменений схемы</para>
    /// </summary>
    internal class ChangesDTO : SchemeDTO
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public ChangesDTO()
            : base()
        {
            Changes = null;
            SelCompIDs = null;
            NewCompMode = false;
            EditorTitle = "";
            FormState = null;
        }


        /// <summary>
        /// Получить или установить изменения схемы
        /// </summary>
        public ICollection<Change> Changes { get; set; }

        /// <summary>
        /// Получить или установить идентификаторы выбранных компонентов
        /// </summary>
        public ICollection<int> SelCompIDs { get; set; }

        /// <summary>
        /// Получить или установить режим добавления нового компонента
        /// </summary>
        public bool NewCompMode { get; set; }

        /// <summary>
        /// Получить или установить заголовок редактора
        /// </summary>
        public string EditorTitle { get; set; }

        /// <summary>
        /// Получить или установить состояние формы
        /// </summary>
        public FormStateDTO FormState { get; set; }
    }
}
