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
 * Summary  : Represents a system instance that consists of one or more applications
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2019
 */

using System;
using System.IO;
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
        /// The directory of the instance files
        /// </summary>
        protected string instnaceDir;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public Instance()
        {
            ID = 0;
            Name = "";
            ServerApp = new ServerApp();
            CommApp = new CommApp();
            WebApp = new WebApp();
            DeploymentProfile = "";
            InstanceDir = "";
            AppSettingsLoaded = false;
        }


        /// <summary>
        /// Gets or sets the instance identifier.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the name of the instance.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets the object represents the Server application.
        /// </summary>
        public ServerApp ServerApp { get; protected set; }

        /// <summary>
        /// Gets the object represents the Communicator application.
        /// </summary>
        public CommApp CommApp { get; protected set; }

        /// <summary>
        /// Gets the object represents the Webstation application.
        /// </summary>
        public WebApp WebApp { get; protected set; }

        /// <summary>
        /// Gets or sets the name of the deployment profile.
        /// </summary>
        public string DeploymentProfile { get; set; }

        /// <summary>
        /// Gets or sets the directory of the instance files.
        /// </summary>
        public string InstanceDir
        {
            get
            {
                return instnaceDir;
            }
            set
            {
                instnaceDir = value;

                if (string.IsNullOrEmpty(instnaceDir))
                {
                    ServerApp.AppDir = "";
                    CommApp.AppDir = "";
                    WebApp.AppDir = "";
                }
                else
                {
                    ServerApp.AppDir = ServerApp.GetAppDir(instnaceDir);
                    CommApp.AppDir = CommApp.GetAppDir(instnaceDir);
                    WebApp.AppDir = WebApp.GetAppDir(instnaceDir);
                }
            }
        }

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

            ID = xmlNode.GetChildAsInt("ID");
            Name = xmlNode.GetChildAsString("Name");

            if (xmlNode.SelectSingleNode("ServerApp") is XmlElement serverAppElem)
                ServerApp.LoadFromXml(serverAppElem);

            if (xmlNode.SelectSingleNode("CommApp") is XmlElement commAppElem)
                CommApp.LoadFromXml(commAppElem);

            if (xmlNode.SelectSingleNode("WebApp") is XmlElement webAppElem)
                WebApp.LoadFromXml(webAppElem);

            DeploymentProfile = xmlNode.GetChildAsString("DeploymentProfile");
        }

        /// <summary>
        /// Saves the instance configuration into the XML node.
        /// </summary>
        public void SaveToXml(XmlElement xmlElem)
        {
            if (xmlElem == null)
                throw new ArgumentNullException("xmlElem");

            xmlElem.AppendElem("ID", ID);
            xmlElem.AppendElem("Name", Name);
            ServerApp.SaveToXml(xmlElem.AppendElem("ServerApp"));
            CommApp.SaveToXml(xmlElem.AppendElem("CommApp"));
            WebApp.SaveToXml(xmlElem.AppendElem("WebApp"));
            xmlElem.AppendElem("DeploymentProfile", DeploymentProfile);
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
            else if ((!ServerApp.Enabled || ServerApp.LoadSettings(out errMsg)) &&
                (!CommApp.Enabled || CommApp.LoadSettings(out errMsg)))
            {
                AppSettingsLoaded = true;
                errMsg = "";
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Creates all project files required for the instance.
        /// </summary>
        public bool CreateInstanceFiles(out string errMsg)
        {
            try
            {
                Directory.CreateDirectory(InstanceDir);
                ScadaApp[] scadaApps = new ScadaApp[] { ServerApp, CommApp, WebApp };

                foreach (ScadaApp scadaApp in scadaApps)
                {
                    if (scadaApp.Enabled && !scadaApp.CreateAppFiles(out errMsg))
                        return false;
                }

                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = AdminPhrases.CreateInstanceFilesError + ": " + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Deletes all project files of the instance.
        /// </summary>
        public bool DeleteInstanceFiles(out string errMsg)
        {
            try
            {
                if (Directory.Exists(InstanceDir))
                    Directory.Delete(InstanceDir, true);

                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = AdminPhrases.DeleteInstanceFilesError + ": " + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Renames the instance.
        /// </summary>
        public bool Rename(string name, out string errMsg)
        {
            try
            {
                if (!AdminUtils.NameIsValid(name))
                    throw new ArgumentException("The specified name is incorrect.");

                DirectoryInfo directoryInfo = new DirectoryInfo(InstanceDir);                
                string newInstanceDir = Path.Combine(directoryInfo.Parent.FullName, name);
                directoryInfo.MoveTo(newInstanceDir);
                InstanceDir = newInstanceDir;
                Name = name;

                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = AdminPhrases.RenameInstanceError + ": " + ex.Message;
                return false;
            }
        }
    }
}
