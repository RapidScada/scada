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
 * Summary  : Represents an application.
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using System;
using System.Xml;

namespace Scada.Admin.Project
{
    /// <summary>
    /// Represents an application.
    /// <para>Представляет приложение.</para>
    /// </summary>
    public abstract class ScadaApp
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ScadaApp()
        {
            Enabled = true;
            AppDir = "";
        }


        /// <summary>
        /// Gets or sets a value indicating whether the application is present in the instance.
        /// </summary>
        public bool Enabled { get; set; }
        
        /// <summary>
        /// Gets or sets the directory of the application files.
        /// </summary>
        public string AppDir { get; set; }


        /// <summary>
        /// Loads the application configuration from the XML node.
        /// </summary>
        public void LoadFromXml(XmlElement xmlElem)
        {
            if (xmlElem == null)
                throw new ArgumentNullException("xmlElem");

            Enabled = xmlElem.GetAttrAsBool("enabled");
        }

        /// <summary>
        /// Saves the application configuration into the XML node.
        /// </summary>
        public void SaveToXml(XmlElement xmlElem)
        {
            if (xmlElem == null)
                throw new ArgumentNullException("xmlElem");

            xmlElem.SetAttribute("enabled", Enabled);
        }

        /// <summary>
        /// Creates project files required for the application.
        /// </summary>
        public abstract bool CreateAppFiles(out string errMsg);

        /// <summary>
        /// Delete project files of the application.
        /// </summary>
        public abstract bool DeleteAppFiles(out string errMsg);

        /// <summary>
        /// Clears the application settings.
        /// </summary>
        public abstract void ClearSettings();
    }
}
