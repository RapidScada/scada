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
 * Summary  : Data window item
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

using Scada.Web.Plugins;

namespace Scada.Web.Shell
{
    /// <summary>
    /// Data window item
    /// <para>Элемент окна данных</para>
    /// </summary>
    public class DataWndItem : ContentItem
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public DataWndItem()
            : base()
        {
            DataWndSpec = null;
        }


        /// <summary>
        /// Получить или установить спецификацию окна данных
        /// </summary>
        public DataWndSpec DataWndSpec { get; set; }

        /// <summary>
        /// Получить спецификацию контента
        /// </summary>
        public override ContentSpec ContentSpec
        {
            get
            {
                return DataWndSpec;
            }
        }
    }
}
