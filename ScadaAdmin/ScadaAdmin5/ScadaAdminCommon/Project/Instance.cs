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
 * Summary  : Represents a system instance that consists of one or more applications
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using Scada.Web;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Scada.Admin.Project
{
    /// <summary>
    /// Represents a system instance that consists of one or more applications.
    /// <para>Представляет экземпляр системы, состоящий из одного или нескольких приложений.</para>
    /// </summary>
    public class Instance
    {
        /// <summary>
        /// The default instance name.
        /// </summary>
        public const string DefaultName = "Default";


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public Instance()
        {
            Name = "";
            ServerApp = new ServerApp();
            CommApp = new CommApp();
            WebApp = new WebApp();
            InstanceDir = "";
            AppSettingsLoaded = false;
        }


        /// <summary>
        /// Gets or sets the name of the instance.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the object represents the Server application.
        /// </summary>
        public ServerApp ServerApp { get; set; }

        /// <summary>
        /// Gets or sets the object represents the Communicator application.
        /// </summary>
        public CommApp CommApp { get; set; }

        /// <summary>
        /// Gets or sets the object represents the Webstation application.
        /// </summary>
        public WebApp WebApp { get; set; }

        /// <summary>
        /// Gets or sets the directory of the instance files.
        /// </summary>
        public string InstanceDir { get; set; }

        /// <summary>
        /// Gets a value indicating whether the application settings are loaded.
        /// </summary>
        public bool AppSettingsLoaded { get; protected set; }


        /// <summary>
        /// Loads the instance configuration from the XML node.
        /// </summary>
        public void LoadFromXml(XmlNode xmlNode)
        {
            if (xmlNode == null)
                throw new ArgumentNullException("xmlNode");

            Name = xmlNode.GetChildAsString("Name");

            if (xmlNode.SelectSingleNode("ServerApp") is XmlElement serverAppElem)
                ServerApp.LoadFromXml(serverAppElem);

            if (xmlNode.SelectSingleNode("CommApp") is XmlElement commAppElem)
                CommApp.LoadFromXml(commAppElem);

            if (xmlNode.SelectSingleNode("WebApp") is XmlElement webAppElem)
                WebApp.LoadFromXml(webAppElem);
        }

        /// <summary>
        /// Saves the instance configuration into the XML node.
        /// </summary>
        public void SaveToXml(XmlElement xmlElem)
        {
            if (xmlElem == null)
                throw new ArgumentNullException("xmlElem");

            ServerApp.SaveToXml(xmlElem.AppendElem("ServerApp"));
            CommApp.SaveToXml(xmlElem.AppendElem("CommApp"));
            WebApp.SaveToXml(xmlElem.AppendElem("WebApp"));
        }

        /// <summary>
        /// Loads the application settings if needed.
        /// </summary>
        public bool LoadAppSettings(out string errMsg)
        {
            if (AppSettingsLoaded)
            {
                errMsg = "";
                return true;
            }
            else if (ServerApp.Settings.Load(ServerApp.GetSettingsPath(InstanceDir), out errMsg) &&
                CommApp.Settings.Load(CommApp.GetSettingsPath(InstanceDir), out errMsg) &&
                WebApp.Settings.LoadFromFile(WebApp.GetSettingsPath(InstanceDir), out errMsg))
            {
                AppSettingsLoaded = true;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
