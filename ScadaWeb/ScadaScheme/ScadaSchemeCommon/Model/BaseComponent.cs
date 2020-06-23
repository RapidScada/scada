/*
 * Copyright 2020 Mikhail Shiryaev
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
 * Modified : 2020
 */

using Scada.Scheme.Model.DataTypes;
using Scada.Scheme.Model.PropertyGrid;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Text;
using System.Web.Script.Serialization;
using System.Xml;
using CM = System.ComponentModel;

namespace Scada.Scheme.Model
{
    /// <summary>
    /// The base class for scheme component.
    /// <para>Базовый класс компонента схемы.</para>
    /// </summary>
    [Serializable]
    public abstract class BaseComponent : IObservableItem, ISchemeViewAvailable
    {
        /// <summary>
        /// Макс. длина произвольного текста в отображаемом имени.
        /// </summary>
        protected const int MaxAuxTextLength = 20;

        /// <summary>
        /// Ссылка на представление схемы.
        /// </summary>
        [NonSerialized]
        protected SchemeView schemeView;
        /// <summary>
        /// Ссылка на объект, контролирующий загрузку классов при клонировании.
        /// </summary>
        [NonSerialized]
        protected SerializationBinder serBinder;


        /// <summary>
        /// Конструктор
        /// </summary>
        public BaseComponent()
        {
            schemeView = null;
            serBinder = null;

            BackColor = "";
            BorderColor = "";
            BorderWidth = 0;
            ToolTip = "";
            ID = 0;
            Name = "";
            Location = Point.Default;
            Size = Size.Default;
            ZIndex = 0;
        }


        /// <summary>
        /// Получить или установить цвет фона.
        /// </summary>
        #region Attributes
        [DisplayName("Background color"), Category(Categories.Appearance)]
        [Description("The background color of the component.")]
        [CM.Editor(typeof(ColorEditor), typeof(UITypeEditor))]
        #endregion
        public string BackColor { get; set; }

        /// <summary>
        /// Получить или установить цвет границы.
        /// </summary>
        #region Attributes
        [DisplayName("Border color"), Category(Categories.Appearance)]
        [Description("The border color of the component.")]
        [CM.Editor(typeof(ColorEditor), typeof(UITypeEditor))]
        #endregion
        public string BorderColor { get; set; }

        /// <summary>
        /// Получить или установить ширину границы.
        /// </summary>
        #region Attributes
        [DisplayName("Border width"), Category(Categories.Appearance)]
        [Description("The border width of the component in pixels.")]
        #endregion
        public int BorderWidth { get; set; }

        /// <summary>
        /// Получить или установить подсказку.
        /// </summary>
        #region Attributes
        [DisplayName("Tooltip"), Category(Categories.Behavior)]
        [Description("The pop-up hint that displays when user rests the pointer on the component.")]
        [CM.Editor(typeof(MultilineStringEditor), typeof(UITypeEditor))]
        #endregion
        public string ToolTip { get; set; }

        /// <summary>
        /// Получить или установить идентификатор.
        /// </summary>
        #region Attributes
        [DisplayName("ID"), Category(Categories.Design), CM.ReadOnly(true)]
        [Description("The unique identifier of the scheme component.")]
        #endregion
        public int ID { get; set; }

        /// <summary>
        /// Получить или установить наименование.
        /// </summary>
        #region Attributes
        [DisplayName("Name"), Category(Categories.Design)]
        [Description("The name of the scheme component.")]
        #endregion
        public string Name { get; set; }

        /// <summary>
        /// Получить имя типа компонента.
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
        /// Получить или установить положение.
        /// </summary>
        #region Attributes
        [DisplayName("Location"), Category(Categories.Layout)]
        [Description("The coordinates of the upper-left corner of the scheme component.")]
        #endregion
        public Point Location { get; set; }

        /// <summary>
        /// Получить или установить размер.
        /// </summary>
        #region Attributes
        [DisplayName("Size"), Category(Categories.Layout)]
        [Description("The size of the scheme component in pixels.")]
        #endregion
        public Size Size { get; set; }

        /// <summary>
        /// Получить или установить порядок отображения.
        /// </summary>
        #region Attributes
        [DisplayName("ZIndex"), Category(Categories.Layout), CM.DefaultValue(0)]
        [Description("The stack order of the scheme component.")]
        #endregion
        public int ZIndex { get; set; }

        /// <summary>
        /// Получить или установить ссылку на представление схемы.
        /// </summary>
        [CM.Browsable(false), ScriptIgnore]
        public SchemeView SchemeView
        {
            get
            {
                return schemeView;
            }
            set
            {
                schemeView = value;
            }
        }


        /// <summary>
        /// Builds the display name for an editor.
        /// </summary>
        protected string BuildDisplayName(string auxText = "")
        {
            return BuildDisplayName(ID, Name, auxText, GetType().Name);
        }

        /// <summary>
        /// Builds the display name for an editor.
        /// </summary>
        public static string BuildDisplayName(int id, string name, string auxText, string typeName)
        {
            return new StringBuilder()
                .Append("[").Append(id).Append("] ")
                .Append(auxText == null || auxText.Length <= MaxAuxTextLength ?
                    auxText : auxText.Substring(0, MaxAuxTextLength) + "...")
                .Append(string.IsNullOrEmpty(auxText) ? "" : " - ")
                .Append(name)
                .Append(string.IsNullOrEmpty(name) ? "" : " - ")
                .Append(typeName)
                .ToString();
        }

        /// <summary>
        /// Gets the input channel numbers associated with the component.
        /// </summary>
        public virtual List<int> GetInCnlNums()
        {
            return null;
        }

        /// <summary>
        /// Gets the output channel numbers associated with the component.
        /// </summary>
        public virtual List<int> GetCtrlCnlNums()
        {
            return null;
        }

        /// <summary>
        /// Загрузить конфигурацию компонента из XML-узла.
        /// </summary>
        public virtual void LoadFromXml(XmlNode xmlNode)
        {
            if (xmlNode == null)
                throw new ArgumentNullException("xmlNode");

            BackColor = xmlNode.GetChildAsString("BackColor");
            BorderColor = xmlNode.GetChildAsString("BorderColor");
            BorderWidth = xmlNode.GetChildAsInt("BorderWidth");
            BorderWidth = xmlNode.GetChildAsInt("BorderWidth",
                string.IsNullOrEmpty(BorderColor) ? 0 : 1 /*для обратной совместимости*/);
            ToolTip = xmlNode.GetChildAsString("ToolTip");
            ID = xmlNode.GetChildAsInt("ID");
            Name = xmlNode.GetChildAsString("Name");
            Location = Point.GetChildAsPoint(xmlNode, "Location");
            Size = Size.GetChildAsSize(xmlNode, "Size");
            ZIndex = xmlNode.GetChildAsInt("ZIndex");
        }

        /// <summary>
        /// Сохранить конфигурацию компонента в XML-узле.
        /// </summary>
        public virtual void SaveToXml(XmlElement xmlElem)
        {
            if (xmlElem == null)
                throw new ArgumentNullException("xmlElem");

            xmlElem.AppendElem("BackColor", BackColor);
            xmlElem.AppendElem("BorderColor", BorderColor);
            xmlElem.AppendElem("BorderWidth", BorderWidth);
            xmlElem.AppendElem("ToolTip", ToolTip);
            xmlElem.AppendElem("ID", ID);
            xmlElem.AppendElem("Name", Name);
            Point.AppendElem(xmlElem, "Location", Location);
            Size.AppendElem(xmlElem, "Size", Size);
            xmlElem.AppendElem("ZIndex", ZIndex);
        }

        /// <summary>
        /// Клонировать объект.
        /// </summary>
        public virtual BaseComponent Clone()
        {
            BaseComponent clonedComponent = ScadaUtils.DeepClone(this, serBinder);
            clonedComponent.schemeView = SchemeView;
            clonedComponent.serBinder = serBinder;
            clonedComponent.ItemChanged += ItemChanged;
            return clonedComponent;
        }

        /// <summary>
        /// Вернуть строковое представление объекта.
        /// </summary>
        public override string ToString()
        {
            return BuildDisplayName();
        }

        /// <summary>
        /// Вызвать событие ItemChanged.
        /// </summary>
        public void OnItemChanged(SchemeChangeTypes changeType, object changedObject, object oldKey = null)
        {
            ItemChanged?.Invoke(this, changeType, changedObject, oldKey);
        }


        /// <summary>
        /// Событие возникающее при изменении компонента.
        /// </summary>
        [field: NonSerialized]
        public event ItemChangedEventHandler ItemChanged;
    }
}
