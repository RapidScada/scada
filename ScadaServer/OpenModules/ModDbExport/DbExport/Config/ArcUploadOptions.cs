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
 * Summary  : Represents archive upload options
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
    /// Represents archive upload options.
    /// <para>Представляет параметры загрузки архивов.</para>
    /// </summary>
    [Serializable]
    internal class ArcUploadOptions
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ArcUploadOptions()
        {
            Enabled = true;
            SnapshotType = SnapshotType.Min;
            Delay = 10000;
            MaxAge = 1;
        }


        /// <summary>
        /// Gets or sets a value indicating whether to upload data.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets the snapshot code.
        /// </summary>
        public SnapshotType SnapshotType { get; set; }

        /// <summary>
        /// Gets or sets the delay before sending archive in milliseconds.
        /// </summary>
        public int Delay { get; set; }

        /// <summary>
        /// Gets or sets the maximum age of archive in days.
        /// </summary>
        public int MaxAge { get; set; }


        /// <summary>
        /// Loads the options from the XML node.
        /// </summary>
        public void LoadFromXml(XmlElement xmlElem)
        {
            if (xmlElem == null)
                throw new ArgumentNullException(nameof(xmlElem));

            Enabled = xmlElem.GetAttrAsBool("enabled");
            SnapshotType = xmlElem.GetChildAsEnum("SnapshotType", SnapshotType.Min);
            Delay = xmlElem.GetChildAsInt("Delay", Delay);
            MaxAge = xmlElem.GetChildAsInt("MaxAge", MaxAge);
        }

        /// <summary>
        /// Saves the options into the XML node.
        /// </summary>
        public void SaveToXml(XmlElement xmlElem)
        {
            if (xmlElem == null)
                throw new ArgumentNullException(nameof(xmlElem));

            xmlElem.SetAttribute("enabled", Enabled);
            xmlElem.AppendElem("SnapshotType", SnapshotType);
            xmlElem.AppendElem("Delay", Delay);
            xmlElem.AppendElem("MaxAge", MaxAge);
        }
    }
}
