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
 * Summary  : Specifies objects which provide scheme document properties
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2017
 */

namespace Scada.Scheme.Model
{
    /// <summary>
    /// Specifies objects which provide scheme document properties
    /// <para>Определяет объекты, который предоставляет свойства документа схемы</para>
    /// </summary>
    interface ISchemeDocAvailable
    {
        /// <summary>
        /// Получить ссылку на свойства документа схемы
        /// </summary>
        SchemeDocument SchemeDoc { get; }
    }
}
