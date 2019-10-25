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
 * Module   : ScadaAdminCommon
 * Summary  : Deployment profile
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2019
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
            InstanceID = 0;
            Name = "";
            WebUrl = "";
            ConnectionSettings = new ConnectionSettings() { ScadaInstance = "" } ;
            DownloadSettings = new DownloadSettings();
            UploadSettings = new UploadSettings();
        }


        /// <summary>
        /// Gets or set the reference to the instance to which the profile belongs.
        /// </summary>
        public int InstanceID { get; set; }

        /// <summary>
        /// Gets or sets the profile name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the web application address.
        /// </summary>
        public string WebUrl { get; set; }

        /// <summary>
        /// Gets the connection settings.
        /// </summary>
        public ConnectionSettings ConnectionSettings { get; protected set; }

        /// <summary>
        /// Gets the download settings.
        /// </summary>
        public DownloadSettings DownloadSettings { get; protected set; }

        /// <summary>
        /// Gets the upload settings.
        /// </summary>
        public UploadSettings UploadSettings { get; protected set; }
        
        
        /// <summary>
        /// Loads the settings from the XML node.
        /// </summary>
        public void LoadFromXml(XmlNode xmlNode)
        {
            if (xmlNode == null)
                throw new ArgumentNullException("xmlNode");

            InstanceID = xmlNode.GetChildAsInt("InstanceID");
            Name = xmlNode.GetChildAsString("Name");
            WebUrl = xmlNode.GetChildAsString("WebUrl");

            if (xmlNode.SelectSingleNode("ConnectionSettings") is XmlNode connectionSettingsNode)
                ConnectionSettings.LoadFromXml(connectionSettingsNode);

            if (xmlNode.SelectSingleNode("DownloadSettings") is XmlNode downloadSettingsNode)
                DownloadSettings.LoadFromXml(downloadSettingsNode);

            if (xmlNode.SelectSingleNode("UploadSettings") is XmlNode uploadSettingsNode)
                UploadSettings.LoadFromXml(uploadSettingsNode);
        }

        /// <summary>
        /// Saves the settings into the XML node.
        /// </summary>
        public void SaveToXml(XmlElement xmlElem)
        {
            if (xmlElem == null)
                throw new ArgumentNullException("xmlElem");

            xmlElem.AppendElem("InstanceID", InstanceID);
            xmlElem.AppendElem("Name", Name);
            xmlElem.AppendElem("WebUrl", WebUrl);
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
