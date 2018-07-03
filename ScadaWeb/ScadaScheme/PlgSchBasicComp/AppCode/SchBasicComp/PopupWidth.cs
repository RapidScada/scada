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
 * Summary  : Possible widths of a popup
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
    /// Possible widths of a popup
    /// <para>Варианты ширины всплывающего окна</para>
    /// </summary>
    [CM.TypeConverter(typeof(EnumConverter))]
    public enum PopupWidth
    {
        /// <summary>
        /// Нормальная
        /// </summary>
        #region Attributes
        [Description("Normal")]
        #endregion
        Normal,

        /// <summary>
        /// Маленькая
        /// </summary>
        #region Attributes
        [Description("Small")]
        #endregion
        Small,

        /// <summary>
        /// Большая
        /// </summary>
        #region Attributes
        [Description("Large")]
        #endregion
        Large
    }
}
