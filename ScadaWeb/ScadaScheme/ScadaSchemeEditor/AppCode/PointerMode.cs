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
 * Summary  : Specifies the modes of the editor mouse pointer
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2019
 */

namespace Scada.Scheme.Editor
{
    /// <summary>
    /// Specifies the modes of the editor mouse pointer.
    /// <para>Определяет режимы указателя мыши редактора.</para>
    /// </summary>
    public enum PointerMode
    {
        /// <summary>
        /// Select components.
        /// </summary>
        Select,

        /// <summary>
        /// Create a new component.
        /// </summary>
        Create,

        /// <summary>
        /// Paste of the copied components.
        /// </summary>
        Paste,

        /// <summary>
        /// Special paste of the copied components.
        /// </summary>
        PasteSpecial
    }
}
