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
 * Summary  : Types of scheme changes
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2017
 */

namespace Scada.Scheme.Model
{
    /// <summary>
    /// Types of scheme changes
    /// <para>Типы изменений схемы</para>
    /// </summary>
    public enum SchemeChangeTypes
    {
        /// <summary>
        /// Нет изменений
        /// </summary>
        None,

        /// <summary>
        /// Изменён документ схемы
        /// </summary>
        SchemeDocChanged,

        /// <summary>
        /// Добавлен компонент
        /// </summary>
        ComponentAdded,

        /// <summary>
        /// Изменён компонент
        /// </summary>
        ComponentChanged,

        /// <summary>
        /// Удалён компонент
        /// </summary>
        ComponentDeleted,

        /// <summary>
        /// Добавлено изображение
        /// </summary>
        ImageAdded,

        /// <summary>
        /// Переименовано изображение
        /// </summary>
        ImageRenamed,

        /// <summary>
        /// Удалено изображение
        /// </summary>
        ImageDeleted
    }
}
