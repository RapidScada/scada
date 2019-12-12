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
 * Summary  : Specifies scheme components bound to input or output channels
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2019
 */

using Scada.Scheme.Model.DataTypes;

namespace Scada.Scheme.Model
{
    /// <summary>
    /// Specifies scheme components bound to input or output channels.
    /// <para>Определяет компоненты схемы, привязаные к входным каналам или каналам управления.</para>
    /// </summary>
    public interface IDynamicComponent
    {
        /// <summary>
        /// Получить или установить действие.
        /// </summary>
        Actions Action { get; set; }

        /// <summary>
        /// Получить или установить номер входного канала.
        /// </summary>
        int InCnlNum { get; set; }

        /// <summary>
        /// Получить или установить номер канала управления.
        /// </summary>
        int CtrlCnlNum { get; set; }
    }
}
