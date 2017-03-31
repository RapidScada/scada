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
 * Summary  : The base class for scheme component
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2017
 */

using Scada.Scheme.Model.DataTypes;
using System.Xml;

namespace Scada.Scheme.Model
{
    /// <summary>
    /// The base class for scheme component
    /// <para>Базовый класс компонента схемы</para>
    /// </summary>
    public abstract class BaseComponent
    {
        /// <summary>
        /// Положение компонента по умолчанию
        /// </summary>
        public static readonly Point DefaultLocation = new Point(0, 0);
        /// <summary>
        /// Размер компонента по умолчанию
        /// </summary>
        public static readonly Size DefaultSize = new Size(100, 100);


        /// <summary>
        /// Конструктор
        /// </summary>
        public BaseComponent()
        {
            ID = 0;
            Name = "";
            Location = DefaultLocation;
            Size = DefaultSize;
            ZIndex = 0;
        }


        /// <summary>
        /// Получить или установить идентификатор
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Получить или установить наименование
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Получить имя типа компонента
        /// </summary>
        public string TypeName
        {
            get
            {
                return GetType().FullName;
            }
        }

        /// <summary>
        /// Получить или установить положение
        /// </summary>
        public Point Location { get; set; }

        /// <summary>
        /// Получить или установить размер
        /// </summary>
        public Size Size { get; set; }

        /// <summary>
        /// Получить или установить порядок отображения
        /// </summary>
        public int ZIndex { get; set; }


        /// <summary>
        /// Загрузить конфигурацию компонента из XML-узла
        /// </summary>
        public abstract void LoadFromXml(XmlNode xmlNode);
    }
}
