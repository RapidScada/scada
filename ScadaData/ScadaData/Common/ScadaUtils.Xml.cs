/*
 * Copyright 2014 Mikhail Shiryaev
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
 * Modified : 2014
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
                return value.ToString().ToLower();
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
        /// Создать и добавить XML-элемент
        /// </summary>
        public static XmlElement AppendElem(this XmlElement parentXmlElem, string elemName, object innerText)
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
        /// Получить строковое значение дочернего XML-узла
        /// </summary>
        /// <remarks>Если XML-узел не существует, вызывается исключение InvalidOperationException</remarks>
        public static string GetChildAsString(this XmlNode parentXmlNode, string childNodeName)
        {
            XmlNode node = parentXmlNode.SelectSingleNode(childNodeName);
            if (node == null)
                throw new InvalidOperationException(
                    string.Format(CommonPhrases.XmlNodeNotFound, childNodeName, parentXmlNode.Name));
            else
                return node.InnerText;
        }

        /// <summary>
        /// Получить логическое значение дочернего XML-узла
        /// </summary>
        public static bool GetChildAsBool(this XmlNode parentXmlNode, string childNodeName)
        {
            try
            {
                return bool.Parse(parentXmlNode.GetChildAsString(childNodeName));
            }
            catch (FormatException)
            {
                throw NewXmlNodeFormatException(childNodeName);
            }
        }

        /// <summary>
        /// Получить целое значение дочернего XML-узла
        /// </summary>
        public static int GetChildAsInt(this XmlNode parentXmlNode, string childNodeName)
        {
            try
            {
                return int.Parse(parentXmlNode.GetChildAsString(childNodeName));
            }
            catch (FormatException)
            {
                throw NewXmlNodeFormatException(childNodeName);
            }
        }

        /// <summary>
        /// Получить вещественное значение дочернего XML-узла
        /// </summary>
        public static double GetChildAsDouble(this XmlNode parentXmlNode, string childNodeName)
        {
            try
            {
                return XmlParseDouble(parentXmlNode.GetChildAsString(childNodeName));
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
                return XmlParseDateTime(parentXmlNode.GetChildAsString(childNodeName));
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
                return XmlParseTimeSpan(parentXmlNode.GetChildAsString(childNodeName));
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
        /// Получить логическое значение атрибута XML-элемента
        /// </summary>
        public static bool GetAttrAsBool(this XmlElement xmlElem, string attrName)
        {
            try
            {
                return bool.Parse(xmlElem.GetAttribute(attrName));
            }
            catch (FormatException)
            {
                throw NewXmlAttrFormatException(attrName);
            }
        }

        /// <summary>
        /// Получить целое значение атрибута XML-элемента
        /// </summary>
        public static int GetAttrAsInt(this XmlElement xmlElem, string attrName)
        {
            try
            {
                return int.Parse(xmlElem.GetAttribute(attrName));
            }
            catch (FormatException)
            {
                throw NewXmlAttrFormatException(attrName);
            }
        }

        /// <summary>
        /// Получить вещественное значение атрибута XML-элемента
        /// </summary>
        public static double GetAttrAsDouble(this XmlElement xmlElem, string attrName)
        {
            try
            {
                return XmlParseDouble(xmlElem.GetAttribute(attrName));
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
                return XmlParseDateTime(xmlElem.GetAttribute(attrName));
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
                return XmlParseTimeSpan(xmlElem.GetAttribute(attrName));
            }
            catch (FormatException)
            {
                throw NewXmlAttrFormatException(attrName);
            }
        }
    }
}
