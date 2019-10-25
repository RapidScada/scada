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
 * Summary  : Deployment settings
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Scada.Admin.Deployment
{
    /// <summary>
    /// Deployment settings.
    /// <para>Настройки развёртывания.</para>
    /// </summary>
    public class DeploymentSettings
    {
        /// <summary>
        /// The default settings file name.
        /// </summary>
        public const string DefFileName = "Deployment.xml";


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DeploymentSettings()
        {
            Profiles = new SortedList<string, DeploymentProfile>();
            ProjectDir = "";
            Loaded = false;
        }


        /// <summary>
        /// Gets the deployment profiles.
        /// </summary>
        public SortedList<string, DeploymentProfile> Profiles { get; protected set; }
        
        /// <summary>
        /// Gets or sets the project directory which contains the settings.
        /// </summary>
        public string ProjectDir { get; set; }

        /// <summary>
        /// Gets the settings file name.
        /// </summary>
        public string FileName
        {
            get
            {
                return Path.Combine(ProjectDir, DefFileName);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the settings are loaded.
        /// </summary>
        public bool Loaded { get; protected set; }


        /// <summary>
        /// Loads the deployment settings from the project directory if needed.
        /// </summary>
        public bool Load(out string errMsg)
        {
            try
            {
                if (!Loaded)
                {
                    Profiles.Clear();
                    string fileName = FileName;

                    if (!File.Exists(fileName))
                        throw new FileNotFoundException(string.Format(CommonPhrases.NamedFileNotFound, fileName));

                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(fileName);

                    XmlNodeList profileNodeList = xmlDoc.DocumentElement.SelectNodes("DeploymentProfile");
                    foreach (XmlNode profileNode in profileNodeList)
                    {
                        DeploymentProfile profile = new DeploymentProfile();
                        profile.LoadFromXml(profileNode);
                        Profiles[profile.Name] = profile;
                    }

                    Loaded = true;
                }

                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = AdminPhrases.LoadDeploymentSettingsError + ": " + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Saves the deployment settings to the project directory.
        /// </summary>
        public bool Save(out string errMsg)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();

                XmlDeclaration xmlDecl = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
                xmlDoc.AppendChild(xmlDecl);

                XmlElement rootElem = xmlDoc.CreateElement("DeploymentSettings");
                xmlDoc.AppendChild(rootElem);

                foreach (DeploymentProfile profile in Profiles.Values)
                {
                    profile.SaveToXml(rootElem.AppendElem("DeploymentProfile"));
                }

                xmlDoc.Save(FileName);
                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = AdminPhrases.SaveDeploymentSettingsError + ": " + ex.Message;
                return false;
            }
        }
        
        /// <summary>
        /// Gets the existing profile names.
        /// </summary>
        public HashSet<string> GetExistingProfileNames(string exceptName = null)
        {
            HashSet<string> existingNames = new HashSet<string>();

            foreach (DeploymentProfile profile in Profiles.Values)
            {
                if (exceptName == null || profile.Name != exceptName)
                    existingNames.Add(profile.Name);
            }

            return existingNames;
        }

        /// <summary>
        /// Removes profiles belong to the specified instance.
        /// </summary>
        public void RemoveProfilesByInstance(int instanceID, out bool profilesAffected)
        {
            profilesAffected = false;

            for (int i = Profiles.Count - 1; i >= 0; i--)
            {
                if (Profiles.Values[i].InstanceID == instanceID)
                {
                    Profiles.RemoveAt(i);
                    profilesAffected = true;
                }
            }
        }
    }
}
