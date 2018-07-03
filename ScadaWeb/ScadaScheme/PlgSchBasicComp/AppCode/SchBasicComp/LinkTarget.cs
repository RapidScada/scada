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
 * Module   : PlgSchBasicComp
 * Summary  : Navigation targets for a link
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using Scada.Scheme.Model.PropertyGrid;
using CM = System.ComponentModel;

namespace Scada.Web.Plugins.SchBasicComp
{
    /// <summary>
    /// Navigation targets for a link
    /// <para>Цели перехода по ссылке</para>
    /// </summary>
    [CM.TypeConverter(typeof(EnumConverter))]
    public enum LinkTarget
    {
        /// <summary>
        /// То же окно
        /// </summary>
        #region Attributes
        [Description("Same frame")]
        #endregion
        Self,

        /// <summary>
        /// Новая вкладка
        /// </summary>
        #region Attributes
        [Description("New tab")]
        #endregion
        Blank,

        /// <summary>
        /// Всплывающее окно
        /// </summary>
        #region Attributes
        [Description("Popup")]
        #endregion
        Popup
    }
}
