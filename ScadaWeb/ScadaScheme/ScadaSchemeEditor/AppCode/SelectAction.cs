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
 * Module   : Scheme Editor
 * Summary  : Specifies the actions when selecting scheme components
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2019
 */

namespace Scada.Scheme.Editor
{
    /// <summary>
    /// Specifies the actions when selecting scheme components.
    /// <para>Задаёт действия при выборе компонентов схемы.</para>
    /// </summary>
    public enum SelectAction
    {
        /// <summary>
        /// Select component.
        /// </summary>
        Select,

        /// <summary>
        /// Append component to selection.
        /// </summary>
        Append,

        /// <summary>
        /// Deselect component.
        /// </summary>
        Deselect,

        /// <summary>
        /// Deselect all components.
        /// </summary>
        DeselectAll
    }
}
