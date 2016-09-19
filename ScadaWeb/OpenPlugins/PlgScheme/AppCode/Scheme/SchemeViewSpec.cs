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
 * Module   : PlgScheme
 * Summary  : Scheme view specification
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

using Scada.Scheme;
using System;

namespace Scada.Web.Plugins.Scheme
{
    /// <summary>
    /// Scheme view specification
    /// <para>Спецификация представления схем</para>
    /// </summary>
    public class SchemeViewSpec : ViewSpec
    {
        /// <summary>
        /// Получить код типа представления
        /// </summary>
        public override string TypeCode
        {
            get
            {
                // TODO: заменить на SchemeView после добавления поля ViewTypeCode в базу конфигурации
                return "sch";
            }
        }

        /// <summary>
        /// Получить ссылку на иконку типа представлений
        /// </summary>
        public override string IconUrl
        {
            get
            {
                return "~/plugins/Scheme/images/schemeicon.png";
            }
        }

        /// <summary>
        /// Получить тип представления
        /// </summary>
        public override Type ViewType
        {
            get
            {
                return typeof(SchemeView);
            }
        }


        /// <summary>
        /// Получить ссылку на представление с заданным идентификатором
        /// </summary>
        public override string GetUrl(int viewID)
        {
            return "~/plugins/Scheme/Scheme.aspx?viewID=" + viewID;
        }
    }
}