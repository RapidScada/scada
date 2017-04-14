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
using System;
using System.Text;
using Scada.Scheme.Model.PropertyGrid;
using CM = System.ComponentModel;

namespace Scada.Scheme.Model
{
    /// <summary>
    /// The base class for scheme component
    /// <para>Базовый класс компонента схемы</para>
    /// </summary>
    public abstract class BaseComponent : IComparable<BaseComponent>
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
        #region Attributes
        [DisplayName("ID"), Category(Categories.Design), CM.ReadOnly(true)]
        [Description("The unique identifier of the scheme component.")]
        #endregion
        public int ID { get; set; }

        /// <summary>
        /// Получить или установить наименование
        /// </summary>
        #region Attributes
        [DisplayName("Name"), Category(Categories.Design)]
        [Description("The name of the scheme component.")]
        #endregion
        public string Name { get; set; }

        /// <summary>
        /// Получить имя типа компонента
        /// </summary>
        #region Attributes
        [DisplayName("Type name"), Category(Categories.Design), CM.ReadOnly(true)]
        [Description("The full name of the scheme component type.")]
        #endregion
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
        #region Attributes
        [DisplayName("Location"), Category(Categories.Layout)]
        [Description("The coordinates of the upper-left corner of the scheme component.")]
        #endregion
        public Point Location { get; set; }

        /// <summary>
        /// Получить или установить размер
        /// </summary>
        #region Attributes
        [DisplayName("Size"), Category(Categories.Layout)]
        [Description("The size of the scheme component in pixels.")]
        #endregion
        public Size Size { get; set; }

        /// <summary>
        /// Получить или установить порядок отображения
        /// </summary>
        #region Attributes
        [DisplayName("ZIndex"), Category(Categories.Layout), CM.DefaultValue(0)]
        [Description("The stack order of the scheme component.")]
        #endregion
        public int ZIndex { get; set; }


        /// <summary>
        /// Загрузить конфигурацию компонента из XML-узла
        /// </summary>
        public virtual void LoadFromXml(XmlNode xmlNode)
        {
            if (xmlNode == null)
                throw new ArgumentNullException("xmlNode");
        }

        /// <summary>
        /// Вернуть строковое представление объекта
        /// </summary>
        public override string ToString()
        {
            return (new StringBuilder())
                .Append("[").Append(ID).Append("] ")
                .Append(Name)
                .Append(string.IsNullOrEmpty(Name) ? "" : " - ")
                .Append(GetType().Name)
                .ToString();
        }

        /// <summary>
        /// Сравнить текущий объект с другим объектом такого же типа
        /// </summary>
        public int CompareTo(BaseComponent other)
        {
            return ID.CompareTo(other.ID);
        }
    }
}
