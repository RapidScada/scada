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
 * Summary  : Image of a scheme
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2017
 */

using Scada.Scheme.Model.PropertyGrid;
using System.ComponentModel;
using System.Drawing.Design;

namespace Scada.Scheme.Model
{
    /// <summary>
    /// Image of a scheme
    /// <para>Изображение схемы</para>
    /// </summary>
    [TypeConverter(typeof(ImageConverter))]
    //[Editor(typeof(ImageEditor), typeof(UITypeEditor))]
    public class Image
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public Image()
        {
            Name = "";
            Data = null;
        }


        /// <summary>
        /// Получить или установить наименование
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Получить или установить данные
        /// </summary>
        public byte[] Data { get; set; }


        /// <summary>
        /// Клонировать объект, передав ссылку на существующие данные
        /// </summary>
        public Image ShallowClone()
        {
            return new Image()
            {
                Name = Name,
                Data = Data
            };
        }
    }
}
