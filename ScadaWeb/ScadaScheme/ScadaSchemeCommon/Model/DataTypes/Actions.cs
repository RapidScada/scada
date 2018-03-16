/*
 * Copyright 2018 Mikhail Shiryaev
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
 * Summary  : Actions of a dynamic component
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2018
 */

using Scada.Scheme.Model.PropertyGrid;
using CM = System.ComponentModel;

namespace Scada.Scheme.Model.DataTypes
{
    /// <summary>
    /// Actions of a dynamic component
    /// <para>Действия динамического элемента</para>
    /// </summary>
    [CM.TypeConverter(typeof(EnumConverter))]
    public enum Actions
    {
        /// <summary>
        /// Не задано
        /// </summary>
        [Description("None")]
        None,

        /// <summary>
        /// Построить график
        /// </summary>
        [Description("Draw diagram")]
        DrawDiagram,

        /// <summary>
        /// Отправить команду
        /// </summary>
        [Description("Send command")]
        SendCommand,

        /// <summary>
        /// Отправить команду сразу
        /// </summary>
        [Description("Send command now")]
        SendCommandNow
    }
}
