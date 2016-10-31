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
 * Module   : PlgTable
 * Summary  : Sample data window specification
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

namespace Scada.Web.Plugins.Table
{
    /// <summary>
    /// Sample data window specification
    /// <para>Спецификация-пример окна данных</para>
    /// </summary>
    public class SampleWndSpec : DataWndSpec
    {
        /// <summary>
        /// Получить код типа контента
        /// </summary>
        public override string TypeCode
        {
            get
            {
                return "SampleWnd";
            }
        }

        /// <summary>
        /// Получить наименование окна данных
        /// </summary>
        public override string Name
        {
            get
            {
                return "Sample Window";
            }
        }

        /// <summary>
        /// Получить признак, что окно данных доступно всем ролям и не требует назначения прав
        /// </summary>
        public override bool ForEveryone
        {
            get
            {
                return true;
            }
        }


        /// <summary>
        /// Получить ссылку на окно данных с заданным идентификатором
        /// </summary>
        public override string GetUrl(int dataWndID)
        {
            return "~/plugins/Table/SampleData.aspx";
        }
    }
}