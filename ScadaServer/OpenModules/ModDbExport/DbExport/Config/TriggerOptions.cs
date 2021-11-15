/*
 * Copyright 2021 Mikhail Shiryaev
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
 * Module   : ModDbExport
 * Summary  : Represents trigger options
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace Scada.Server.Modules.DbExport.Config
{
    /// <summary>
    /// Represents trigger options.
    /// <para>Представляет параметры триггера.</para>
    /// </summary>
    [Serializable]
    internal abstract class TriggerOptions : ITreeNode
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public TriggerOptions()
        {
            Active = true;
            Name = "";
            CnlNums = new List<int>();
            DeviceNums = new List<int>();
            Query = "";
            Parent = null;
        }


        /// <summary>
        /// Gets or sets a value indicating whether the trigger is active.
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Gets or sets the trigger name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets the trigger type name.
        /// </summary>
        public abstract string TriggerType { get; }

        /// <summary>
        /// Gets the input channel numbers.
        /// </summary>
        public List<int> CnlNums { get; private set; }

        /// <summary>
        /// Gets the device numbers.
        /// </summary>
        public List<int> DeviceNums { get; private set; }

        /// <summary>
        /// Gets or sets the SQL query that is called when the trigger is fired.
        /// </summary>
        public string Query { get; set; }

        /// <summary>
        /// Gets or sets the parent node.
        /// </summary>
        public ITreeNode Parent { get; set; }

        /// <summary>
        /// Get a list of child nodes.
        /// </summary>
        public IList Children
        {
            get
            {
                return null;
            }
        }


        /// <summary>
        /// Loads the options from the XML node.
        /// </summary>
        public virtual void LoadFromXml(XmlElement xmlElem)
        {
            if (xmlElem == null)
                throw new ArgumentNullException(nameof(xmlElem));

            Active = xmlElem.GetAttrAsBool("active", true);
            Name = xmlElem.GetAttrAsString("name");
            CnlNums.AddRange(RangeUtils.StrToRange(xmlElem.GetChildAsString("CnlNums"), true, true));
            DeviceNums.AddRange(RangeUtils.StrToRange(xmlElem.GetChildAsString("DeviceNums"), true, true));
            Query = xmlElem.GetChildAsString("Query");
        }

        /// <summary>
        /// Saves the options into the XML node.
        /// </summary>
        public virtual void SaveToXml(XmlElement xmlElem)
        {
            if (xmlElem == null)
                throw new ArgumentNullException(nameof(xmlElem));

            xmlElem.SetAttribute("active", Active);
            xmlElem.SetAttribute("name", Name);
            xmlElem.SetAttribute("type", TriggerType);
            xmlElem.AppendElem("CnlNums", RangeUtils.RangeToStr(CnlNums));
            xmlElem.AppendElem("DeviceNums", RangeUtils.RangeToStr(DeviceNums));
            xmlElem.AppendElem("Query", Query);
        }

        /// <summary>
        /// Gets the names of the available query parameters.
        /// </summary>
        public virtual List<string> GetParamNames()
        {
            return new List<string>();
        }
    }
}
