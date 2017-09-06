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
 * Module   : ScadaData
 * Summary  : The class contains utility methods for the whole system. XML utilities
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2014
 * Modified : 2017
 */

using System;
using System.Globalization;
using System.Xml;

namespace Scada
{
	partial class ScadaUtils
	{
        /// <summary>
        /// Создать исключение FormatException для XML-узла
        /// </summary>
        private static FormatException NewXmlNodeFormatException(string nodeName)
        {
            return new FormatException(string.Format(CommonPhrases.IncorrectXmlNodeVal, nodeName));
        }

        /// <summary>
        /// Создать исключение FormatException для XML-атрибута
        /// </summary>
        private static FormatException NewXmlAttrFormatException(string attrName)
        {
            return new FormatException(string.Format(CommonPhrases.IncorrectXmlAttrVal, attrName));
        }


        /// <summary>
        /// Преобразовать значение, предназначенное для записи в XML-файл, в строку
        /// </summary>
        public static string XmlValToStr(object value)
        {
            if (value == null)
                return "";
            else if (value is bool)
                return value.ToString().ToLowerInvariant();
            else if (value is double)
                return ((double)value).ToString(NumberFormatInfo.InvariantInfo);
            else if (value is DateTime)
                return ((DateTime)value).ToString(DateTimeFormatInfo.InvariantInfo);
            else if (value is TimeSpan)
                return ((TimeSpan)value).ToString("", DateTimeFormatInfo.InvariantInfo);
            else
                return value.ToString();
        }

        /// <summary>
        /// Преобразовать строку, считанную из XML-документа, в вещественное число
        /// </summary>
        public static double XmlParseDouble(string s)
        {
            return double.Parse(s, NumberFormatInfo.InvariantInfo);
        }
        
        /// <summary>
        /// Преобразовать строку, считанную из XML-документа, в дату и время
        /// </summary>
        public static DateTime XmlParseDateTime(string s)
        {
            return DateTime.Parse(s, DateTimeFormatInfo.InvariantInfo);
        }

        /// <summary>
        /// Преобразовать строку, считанную из XML-документа, в дату
        /// </summary>
        public static DateTime XmlParseDate(string s)
        {
            return DateTime.Parse(s, DateTimeFormatInfo.InvariantInfo).Date;
        }

        /// <summary>
        /// Преобразовать строку, считанную из XML-документа, в интервал времени
        /// </summary>
        public static TimeSpan XmlParseTimeSpan(string s)
        {
            return TimeSpan.Parse(s, DateTimeFormatInfo.InvariantInfo);
        }

        /// <summary>
        /// Преобразовать строку, считанную из XML-документа, в перечислимое значение
        /// </summary>
        public static T XmlParseEnum<T>(string s) where T : struct
        {
            return (T)Enum.Parse(typeof(T), s);
        }


        /// <summary>
        /// Создать и добавить XML-элемент
        /// </summary>
        public static XmlElement AppendElem(this XmlElement parentXmlElem, string elemName, object innerText = null)
        {
            XmlElement xmlElem = parentXmlElem.OwnerDocument.CreateElement(elemName);
            string val = XmlValToStr(innerText);
            if (!string.IsNullOrEmpty(val))
                xmlElem.InnerText = val;
            return (XmlElement)parentXmlElem.AppendChild(xmlElem);
        }

        /// <summary>
        /// Создать и добавить XML-элемент параметра
        /// </summary>
        public static XmlElement AppendParamElem(this XmlElement parentXmlElem, string paramName, object value,
            string descr = "")
        {
            XmlElement paramElem = parentXmlElem.OwnerDocument.CreateElement("Param");
            paramElem.SetAttribute("name", paramName);
            paramElem.SetAttribute("value", XmlValToStr(value));
            if (!string.IsNullOrEmpty(descr))
                paramElem.SetAttribute("descr", descr);
            return (XmlElement)parentXmlElem.AppendChild(paramElem);
        }

        /// <summary>
        /// Создать и добавить XML-элемент параметра, автоматически выбрав язык описания
        /// </summary>
        public static XmlElement AppendParamElem(this XmlElement parentXmlElem, string paramName, object value,
            string descrRu, string descrEn)
        {
            return parentXmlElem.AppendParamElem(paramName, value, Localization.UseRussian ? descrRu : descrEn);
        }

        /// <summary>
        /// Получить XML-элемент параметра
        /// </summary>
        public static XmlElement GetParamElem(this XmlElement parentXmlElem, string paramName)
        {
            XmlNodeList xmlNodes = parentXmlElem.SelectNodes(string.Format("Param[@name='{0}'][1]", paramName));
            return xmlNodes.Count > 0 ? xmlNodes[0] as XmlElement : null;
        }

        /// <summary>
        /// Получить строковое значение дочернего XML-узла
        /// </summary>
        /// <remarks>Если XML-узел не существует, вызывается исключение InvalidOperationException</remarks>
        public static string GetChildAsString(this XmlNode parentXmlNode, string childNodeName, string defaultVal = "")
        {
            XmlNode node = parentXmlNode.SelectSingleNode(childNodeName);
            return node == null ? defaultVal : node.InnerText;
        }

        /// <summary>
        /// Получить логическое значение дочернего XML-узла
        /// </summary>
        public static bool GetChildAsBool(this XmlNode parentXmlNode, string childNodeName, bool defaultVal = false)
        {
            try
            {
                XmlNode node = parentXmlNode.SelectSingleNode(childNodeName);
                return node == null ? defaultVal : bool.Parse(node.InnerText);
            }
            catch (FormatException)
            {
                throw NewXmlNodeFormatException(childNodeName);
            }
        }

        /// <summary>
        /// Получить целое значение дочернего XML-узла
        /// </summary>
        public static int GetChildAsInt(this XmlNode parentXmlNode, string childNodeName, int defaultVal = 0)
        {
            try
            {
                XmlNode node = parentXmlNode.SelectSingleNode(childNodeName);
                return node == null ? defaultVal : int.Parse(node.InnerText);
            }
            catch (FormatException)
            {
                throw NewXmlNodeFormatException(childNodeName);
            }
        }

        /// <summary>
        /// Получить 64-битное целое значение дочернего XML-узла
        /// </summary>
        public static long GetChildAsLong(this XmlNode parentXmlNode, string childNodeName, long defaultVal = 0)
        {
            try
            {
                XmlNode node = parentXmlNode.SelectSingleNode(childNodeName);
                return node == null ? defaultVal : long.Parse(node.InnerText);
            }
            catch (FormatException)
            {
                throw NewXmlNodeFormatException(childNodeName);
            }
        }

        /// <summary>
        /// Получить вещественное значение дочернего XML-узла
        /// </summary>
        public static double GetChildAsDouble(this XmlNode parentXmlNode, string childNodeName, double defaultVal = 0)
        {
            try
            {
                XmlNode node = parentXmlNode.SelectSingleNode(childNodeName);
                return node == null ? defaultVal : XmlParseDouble(node.InnerText);
            }
            catch (FormatException)
            {
                throw NewXmlNodeFormatException(childNodeName);
            }
        }

        /// <summary>
        /// Получить значение даты и времени дочернего XML-узла
        /// </summary>
        public static DateTime GetChildAsDateTime(this XmlNode parentXmlNode, string childNodeName)
        {
            try
            {
                XmlNode node = parentXmlNode.SelectSingleNode(childNodeName);
                return node == null ? DateTime.MinValue : XmlParseDateTime(node.InnerText);
            }
            catch (FormatException)
            {
                throw NewXmlNodeFormatException(childNodeName);
            }
        }

        /// <summary>
        /// Получить значение интервала времени дочернего XML-узла
        /// </summary>
        public static TimeSpan GetChildAsTimeSpan(this XmlNode parentXmlNode, string childNodeName)
        {
            try
            {
                XmlNode node = parentXmlNode.SelectSingleNode(childNodeName);
                return node == null ? TimeSpan.Zero : XmlParseTimeSpan(node.InnerText);
            }
            catch (FormatException)
            {
                throw NewXmlNodeFormatException(childNodeName);
            }
        }

        /// <summary>
        /// Получить перечислимое значение дочернего XML-узла
        /// </summary>
        public static T GetChildAsEnum<T>(this XmlNode parentXmlNode, string childNodeName) where T : struct
        {
            try
            {
                XmlNode node = parentXmlNode.SelectSingleNode(childNodeName);
                return node == null ? default(T) : XmlParseEnum<T>(node.InnerText);
            }
            catch (FormatException)
            {
                throw NewXmlNodeFormatException(childNodeName);
            }
        }


        /// <summary>
        /// Установить значение атрибута XML-элемента
        /// </summary>
        public static void SetAttribute(this XmlElement xmlElem, string attrName, object value)
        {
            xmlElem.SetAttribute(attrName, XmlValToStr(value));
        }

        /// <summary>
        /// Получить строковое значение атрибута XML-элемента
        /// </summary>
        public static string GetAttrAsString(this XmlElement xmlElem, string attrName, string defaultVal = "")
        {
            return xmlElem.HasAttribute(attrName) ?
                xmlElem.GetAttribute(attrName) : defaultVal;
        }

        /// <summary>
        /// Получить логическое значение атрибута XML-элемента
        /// </summary>
        public static bool GetAttrAsBool(this XmlElement xmlElem, string attrName, bool defaultVal = false)
        {
            try
            {
                return xmlElem.HasAttribute(attrName) ? 
                    bool.Parse(xmlElem.GetAttribute(attrName)) : defaultVal;
            }
            catch (FormatException)
            {
                throw NewXmlAttrFormatException(attrName);
            }
        }

        /// <summary>
        /// Получить целое значение атрибута XML-элемента
        /// </summary>
        public static int GetAttrAsInt(this XmlElement xmlElem, string attrName, int defaultVal = 0)
        {
            try
            {
                return xmlElem.HasAttribute(attrName) ? 
                    int.Parse(xmlElem.GetAttribute(attrName)) : defaultVal;
            }
            catch (FormatException)
            {
                throw NewXmlAttrFormatException(attrName);
            }
        }

        /// <summary>
        /// Получить 64-битное целое значение атрибута XML-элемента
        /// </summary>
        public static long GetAttrAsLong(this XmlElement xmlElem, string attrName, long defaultVal = 0)
        {
            try
            {
                return xmlElem.HasAttribute(attrName) ?
                    long.Parse(xmlElem.GetAttribute(attrName)) : defaultVal;
            }
            catch (FormatException)
            {
                throw NewXmlAttrFormatException(attrName);
            }
        }

        /// <summary>
        /// Получить вещественное значение атрибута XML-элемента
        /// </summary>
        public static double GetAttrAsDouble(this XmlElement xmlElem, string attrName, double defaultVal = 0)
        {
            try
            {
                return xmlElem.HasAttribute(attrName) ? 
                    XmlParseDouble(xmlElem.GetAttribute(attrName)) : defaultVal;
            }
            catch (FormatException)
            {
                throw NewXmlAttrFormatException(attrName);
            }
        }

        /// <summary>
        /// Получить значение даты и времени атрибута XML-элемента
        /// </summary>
        public static DateTime GetAttrAsDateTime(this XmlElement xmlElem, string attrName)
        {
            try
            {
                return xmlElem.HasAttribute(attrName) ? 
                    XmlParseDateTime(xmlElem.GetAttribute(attrName)) : DateTime.MinValue;
            }
            catch (FormatException)
            {
                throw NewXmlAttrFormatException(attrName);
            }
        }

        /// <summary>
        /// Получить значение интервала времени атрибута XML-элемента
        /// </summary>
        public static TimeSpan GetAttrAsTimeSpan(this XmlElement xmlElem, string attrName)
        {
            try
            {
                return xmlElem.HasAttribute(attrName) ? 
                    XmlParseTimeSpan(xmlElem.GetAttribute(attrName)) : TimeSpan.Zero;
            }
            catch (FormatException)
            {
                throw NewXmlAttrFormatException(attrName);
            }
        }

        /// <summary>
        /// Получить перечислимое значение атрибута XML-элемента
        /// </summary>
        public static T GetAttrAsEnum<T>(this XmlElement xmlElem, string attrName) where T : struct
        {
            try
            {
                return xmlElem.HasAttribute(attrName) ?
                    XmlParseEnum<T>(xmlElem.GetAttribute(attrName)) : default(T);
            }
            catch (FormatException)
            {
                throw NewXmlAttrFormatException(attrName);
            }
        }
    }
}
