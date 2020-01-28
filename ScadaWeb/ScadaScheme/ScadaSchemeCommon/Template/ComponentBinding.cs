/*
 * Copyright 2019 Mikhail Shiryaev
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
 * Summary  : Represents component bindings to channels
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
 * Modified : 2019
 */

using System;
using System.Xml;

namespace Scada.Scheme.Template
{
    /// <summary>
    /// Represents component bindings to channels.
    /// <para>Представляет привязки компонента к каналам.</para>
    /// </summary>
    public class ComponentBinding
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ComponentBinding()
        {
            CompID = 0;
            InCnlNum = 0;
            CtrlCnlNum = 0;
        }


        /// <summary>
        /// Gets or sets the component ID.
        /// </summary>
        public int CompID { get; set; }

        /// <summary>
        /// Gets or sets the input channel number to which the component is bound.
        /// </summary>
        public int InCnlNum { get; set; }

        /// <summary>
        /// Gets or sets the output channel number to which the component is bound.
        /// </summary>
        public int CtrlCnlNum { get; set; }

        /// <summary>
        /// Loads the bindongs from the XML node.
        /// </summary>
        public void LoadFromXml(XmlElement xmlElem)
        {
            if (xmlElem == null)
                throw new ArgumentNullException("xmlElem");

            CompID = xmlElem.GetAttrAsInt("compID");
            InCnlNum = xmlElem.GetAttrAsInt("inCnlNum");
            CtrlCnlNum = xmlElem.GetAttrAsInt("ctrlCnlNum");
        }
    }
}
