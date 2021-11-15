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
 * Summary  : Represents general export target options
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

using System;
using System.Xml;

namespace Scada.Server.Modules.DbExport.Config
{
    /// <summary>
    /// Represents general export target options.
    /// <para>Представляет основные параметры цели экспорта.</para>
    /// </summary>
    [Serializable]
    internal class GeneralOptions
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public GeneralOptions()
        {
            Active = true;
            ID = 0;
            Name = "";
            MaxQueueSize = 1000;
            DataLifetime = 3600;
            OutCnlNum = 0;
        }


        /// <summary>
        /// Gets or sets a value indicating whether the export target is active.
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Gets or sets the export target ID.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the export target name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the maximum queue size.
        /// </summary>
        public int MaxQueueSize { get; set; }

        /// <summary>
        /// Gets or sets the data lifetime in the queue, in seconds.
        /// </summary>
        public int DataLifetime { get; set; }

        /// <summary>
        /// Gets or sets the output channel number to control the export target.
        /// </summary>
        public int OutCnlNum { get; set; }


        /// <summary>
        /// Loads the options from the XML node.
        /// </summary>
        public void LoadFromXml(XmlNode xmlNode)
        {
            if (xmlNode == null)
                throw new ArgumentNullException("xmlNode");

            Active = xmlNode.GetChildAsBool("Active");
            ID = xmlNode.GetChildAsInt("ID");
            Name = xmlNode.GetChildAsString("Name");
            MaxQueueSize = xmlNode.GetChildAsInt("MaxQueueSize");
            DataLifetime = xmlNode.GetChildAsInt("DataLifetime");
            OutCnlNum = xmlNode.GetChildAsInt("OutCnlNum");
        }

        /// <summary>
        /// Saves the options into the XML node.
        /// </summary>
        public void SaveToXml(XmlElement xmlElem)
        {
            if (xmlElem == null)
                throw new ArgumentNullException("xmlElem");

            xmlElem.AppendElem("Active", Active);
            xmlElem.AppendElem("ID", ID);
            xmlElem.AppendElem("Name", Name);
            xmlElem.AppendElem("MaxQueueSize", MaxQueueSize);
            xmlElem.AppendElem("DataLifetime", DataLifetime);
            xmlElem.AppendElem("OutCnlNum", OutCnlNum);
        }
    }
}
