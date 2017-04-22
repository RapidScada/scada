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
 * Summary  : The class for transfer scheme document properties
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2017
 */

using Scada.Scheme.Model;

namespace Scada.Scheme.DataTransfer
{
    /// <summary>
    /// The class for transfer scheme document properties
    /// <para>Класс для передачи свойств документа схемы</para>
    /// </summary>
    public class SchemeDocDTO : SchemeDTO
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public SchemeDocDTO()
        {
            SchemeDoc = null;
        }

        /// <summary>
        /// Получить или установить свойства схемы
        /// </summary>
        public SchemeDocument SchemeDoc { get; set; }
    }
}
