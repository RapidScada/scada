/*
 * Copyright 2018 Mikhail Shiryaev
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
 * Module   : ScadaAdminCommon
 * Summary  : Deployment profile
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using Scada.Agent.Connector;
using System;
using System.Xml;

namespace Scada.Admin.Deployment
{
    /// <summary>
    /// Deployment profile.
    /// <para>Профиль развёртывания.</para>
    /// </summary>
    public class DeploymentProfile
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DeploymentProfile()
        {
            Name = "";
            ConnectionSettings = new ConnectionSettings() { ScadaInstance = "" } ;
            DownloadSettings = new TransferSettings();
            UploadSettings = new TransferSettings();
        }


        /// <summary>
        /// Gets or sets the profile name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets the connection settings.
        /// </summary>
        public ConnectionSettings ConnectionSettings { get; protected set; }

        /// <summary>
        /// Gets the download settings.
        /// </summary>
        public TransferSettings DownloadSettings { get; protected set; }

        /// <summary>
        /// Gets the upload settings.
        /// </summary>
        public TransferSettings UploadSettings { get; protected set; }
        
        
        /// <summary>
        /// Loads the settings from the XML node.
        /// </summary>
        public void LoadFromXml(XmlNode xmlNode)
        {
            if (xmlNode == null)
                throw new ArgumentNullException("xmlNode");

            Name = xmlNode.GetChildAsString("Name");

            XmlNode connectionSettingsNode = xmlNode.SelectSingleNode("ConnectionSettings");
            if (connectionSettingsNode != null)
                ConnectionSettings.LoadFromXml(connectionSettingsNode);

            XmlNode downloadSettingsNode = xmlNode.SelectSingleNode("DownloadSettings");
            if (downloadSettingsNode != null)
                DownloadSettings.LoadFromXml(downloadSettingsNode);

            XmlNode uploadSettingsNode = xmlNode.SelectSingleNode("UploadSettings");
            if (uploadSettingsNode != null)
                UploadSettings.LoadFromXml(uploadSettingsNode);
        }

        /// <summary>
        /// Saves the settings into the XML node.
        /// </summary>
        public void SaveToXml(XmlElement xmlElem)
        {
            if (xmlElem == null)
                throw new ArgumentNullException("xmlElem");

            xmlElem.AppendElem("Name", Name);
            ConnectionSettings.SaveToXml(xmlElem.AppendElem("ConnectionSettings"));
            DownloadSettings.SaveToXml(xmlElem.AppendElem("DownloadSettings"));
            UploadSettings.SaveToXml(xmlElem.AppendElem("UploadSettings"));
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        public override string ToString()
        {
            return Name;
        }
    }
}
