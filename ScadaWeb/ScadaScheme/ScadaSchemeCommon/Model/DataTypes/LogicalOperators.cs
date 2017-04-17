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
 * Summary  : Logical operators
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2017
 */

using Scada.Scheme.Model.PropertyGrid;
using CM = System.ComponentModel;

namespace Scada.Scheme.Model.DataTypes
{
    /// <summary>
    /// Logical operators
    /// <para>Логические операторы</para>
    /// </summary>
    [CM.TypeConverter(typeof(EnumConverter))]
    public enum LogicalOperators
    {
        /// <summary>
        /// Не задан
        /// </summary>
        [Description("None")]
        None,

        /// <summary>
        /// И
        /// </summary>
        [Description("And")]
        And,

        /// <summary>
        /// Или
        /// </summary>
        [Description("Or")]
        Or
    }
}
