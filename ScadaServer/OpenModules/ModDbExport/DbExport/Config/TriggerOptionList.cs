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
 * Summary  : Represents a list of trigger options
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
    /// Represents a list of trigger options.
    /// <para>Представляет список параметров триггера.</para>
    /// </summary>
    [Serializable]
    internal class TriggerOptionList : List<TriggerOptions>, ITreeNode
    {
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
                return this;
            }
        }


        /// <summary>
        /// Loads the options from the XML node.
        /// </summary>
        public void LoadFromXml(XmlNode xmlNode)
        {
            if (xmlNode == null)
                throw new ArgumentNullException(nameof(xmlNode));

            foreach (XmlElement triggerElem in xmlNode.SelectNodes("Trigger"))
            {
                TriggerOptions triggerOptions = null;
                string triggerType = triggerElem.GetAttrAsString("type").ToLowerInvariant();

                if (triggerType == "curdatatrigger")
                    triggerOptions = new CurDataTriggerOptions();
                else if (triggerType == "arcdatatrigger")
                    triggerOptions = new ArcDataTriggerOptions();
                else if (triggerType == "eventtrigger")
                    triggerOptions = new EventTriggerOptions();

                if (triggerOptions != null)
                {
                    triggerOptions.Parent = this;
                    triggerOptions.LoadFromXml(triggerElem);
                    Add(triggerOptions);
                }
            }
        }

        /// <summary>
        /// Saves the options into the XML node.
        /// </summary>
        public void SaveToXml(XmlElement xmlElem)
        {
            if (xmlElem == null)
                throw new ArgumentNullException(nameof(xmlElem));

            foreach (TriggerOptions triggerOptions in this)
            {
                triggerOptions.SaveToXml(xmlElem.AppendElem("Trigger"));
            }
        }
    }
}
