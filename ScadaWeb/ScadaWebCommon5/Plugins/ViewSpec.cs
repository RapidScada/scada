/*
 * Copyright 2016 Mikhail Shiryaev
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
 * Module   : ScadaWebCommon
 * Summary  : The base class for view specification
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

using System;

namespace Scada.Web.Plugins
{
    /// <summary>
    /// The base class for view specification
    /// <para>Родительский класс спецификации представления</para>
    /// </summary>
    public abstract class ViewSpec : UiObjSpec
    {
        /// <summary>
        /// Получить ссылку на иконку типа представлений
        /// </summary>
        public abstract string IconUrl { get; }

        /// <summary>
        /// Получить тип представления
        /// </summary>
        public abstract Type ViewType { get; }
    }
}
