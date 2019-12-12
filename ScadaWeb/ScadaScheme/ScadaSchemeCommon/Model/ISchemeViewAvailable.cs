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
 * Module   : ScadaSchemeCommon
 * Summary  : Defines the scheme view reference
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
 * Modified : 2019
 */

namespace Scada.Scheme.Model
{
    /// <summary>
    /// Defines the scheme view reference.
    /// <para>Определяет ссылку на представление схемы.</para>
    /// </summary>
    public interface ISchemeViewAvailable
    {
        /// <summary>
        /// Gets or sets the reference to a scheme view.
        /// </summary>
        SchemeView SchemeView { get; set; }
    }
}
