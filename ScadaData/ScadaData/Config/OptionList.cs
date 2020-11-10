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
 * Module   : ScadaData
 * Summary  : Represents a list of custom options
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using System;
using System.Collections.Generic;
using System.Xml;

namespace Scada.Config
{
    /// <summary>
    /// Represents a list of custom options.
    /// <para>Представляет список пользовательских параметров.</para>
    /// </summary>
    public class OptionList : SortedList<string, string>
    {
        /// <summary>
        /// Loads the options from the XML node.
        /// </summary>
        public void LoadFromXml(XmlNode xmlNode)
        {
            if (xmlNode == null)
                throw new ArgumentNullException(nameof(xmlNode));

            foreach (XmlElement optionElem in xmlNode.SelectNodes("Option"))
            {
                this[optionElem.GetAttrAsString("name")] = optionElem.GetAttrAsString("value");
            }
        }

        /// <summary>
        /// Saves the options into the XML node.
        /// </summary>
        public void SaveToXml(XmlElement xmlElem)
        {
            if (xmlElem == null)
                throw new ArgumentNullException(nameof(xmlElem));

            foreach (KeyValuePair<string, string> pair in this)
            {
                xmlElem.AppendOptionElem(pair.Key, pair.Value);
            }
        }
    }
}
