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
 * Module   : ScadaData
 * Summary  : View properties
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

using System;
using System.IO;

namespace Scada.Data.Models
{
    /// <summary>
    /// View properties
    /// <para>Свойства представления</para>
    /// </summary>
    public class ViewProps
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public ViewProps()
            : this(0)
        {

        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public ViewProps(int viewID)
        {
            ViewID = viewID;
            FileName = "";
            ViewTypeCode = "";
        }


        /// <summary>
        /// Получить или установить идентификатор представления
        /// </summary>
        public int ViewID { get; set; }

        /// <summary>
        /// Получить или установить имя файла представления
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Получить или установить код типа представления
        /// </summary>
        public string ViewTypeCode { get; set; }


        /// <summary>
        /// Получить код типа представления на основе имени файла представления
        /// </summary>
        public static string GetViewTypeCode(string fileName)
        {
            if (fileName.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                fileName.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                return "WebPageView";
            }
            else
            {
                string ext = Path.GetExtension(fileName);
                return ext == null ? "" : ext.TrimStart('.');
            }
        }
    }
}
