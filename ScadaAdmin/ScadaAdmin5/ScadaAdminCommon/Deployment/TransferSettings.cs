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
 * Summary  : Configuration transfer settings
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2019
 */

using Scada.Agent;
using System;
using System.Collections.Generic;
using System.Xml;

namespace Scada.Admin.Deployment
{
    /// <summary>
    /// Configuration transfer settings.
    /// <para>Настройки передачи конфигурации.</para>
    /// </summary>
    public abstract class TransferSettings
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public TransferSettings()
        {
            IncludeBase = true;
            IncludeInterface = true;
            IncludeServer = true;
            IncludeComm = true;
            IncludeWeb = true;
            IgnoreRegKeys = false;
            IgnoreWebStorage = true;
        }


        /// <summary>
        /// Gets or sets a value indicating whether to transfer the configuration database.
        /// </summary>
        public bool IncludeBase { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to transfer interface files.
        /// </summary>
        public bool IncludeInterface { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to transfer Server settings.
        /// </summary>
        public bool IncludeServer { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to transfer Communicator settings.
        /// </summary>
        public bool IncludeComm { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to transfer Webstation settings.
        /// </summary>
        public bool IncludeWeb { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to ignore registration keys.
        /// </summary>
        public bool IgnoreRegKeys { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to ignore Webstation storage.
        /// </summary>
        public bool IgnoreWebStorage { get; set; }


        /// <summary>
        /// Loads the settings from the XML node.
        /// </summary>
        public virtual void LoadFromXml(XmlNode xmlNode)
        {
            if (xmlNode == null)
                throw new ArgumentNullException("xmlNode");

            IncludeBase = xmlNode.GetChildAsBool("IncludeBase");
            IncludeInterface = xmlNode.GetChildAsBool("IncludeInterface");
            IncludeServer = xmlNode.GetChildAsBool("IncludeServer");
            IncludeComm = xmlNode.GetChildAsBool("IncludeComm");
            IncludeWeb = xmlNode.GetChildAsBool("IncludeWeb");
            IgnoreRegKeys = xmlNode.GetChildAsBool("IgnoreRegKeys");
            IgnoreWebStorage = xmlNode.GetChildAsBool("IgnoreWebStorage");
        }

        /// <summary>
        /// Saves the settings into the XML node.
        /// </summary>
        public virtual void SaveToXml(XmlElement xmlElem)
        {
            if (xmlElem == null)
                throw new ArgumentNullException("xmlElem");

            xmlElem.AppendElem("IncludeBase", IncludeBase);
            xmlElem.AppendElem("IncludeInterface", IncludeInterface);
            xmlElem.AppendElem("IncludeServer", IncludeServer);
            xmlElem.AppendElem("IncludeComm", IncludeComm);
            xmlElem.AppendElem("IncludeWeb", IncludeWeb);
            xmlElem.AppendElem("IgnoreRegKeys", IgnoreRegKeys);
            xmlElem.AppendElem("IgnoreWebStorage", IgnoreWebStorage);
        }

        /// <summary>
        /// Convert this transfer settings to Agent transfer options.
        /// </summary>
        public ConfigOptions ToConfigOpions()
        {
            ConfigParts configParts = ConfigParts.None;
            List<RelPath> ignoredPaths = new List<RelPath>();

            if (IncludeBase)
                configParts |= ConfigParts.Base;
            if (IncludeInterface)
                configParts |= ConfigParts.Interface;
            if (IncludeServer)
                configParts |= ConfigParts.Server;
            if (IncludeComm)
                configParts |= ConfigParts.Comm;
            if (IncludeWeb)
                configParts |= ConfigParts.Web;

            if (IgnoreRegKeys)
            {
                ignoredPaths.Add(new RelPath(ConfigParts.Server, AppFolder.Config, "*_Reg.xml"));
                ignoredPaths.Add(new RelPath(ConfigParts.Server, AppFolder.Config, "CompCode.txt"));
                ignoredPaths.Add(new RelPath(ConfigParts.Comm, AppFolder.Config, "*_Reg.xml"));
                ignoredPaths.Add(new RelPath(ConfigParts.Comm, AppFolder.Config, "CompCode.txt"));
                ignoredPaths.Add(new RelPath(ConfigParts.Web, AppFolder.Config, "*_Reg.xml"));
            }

            if (IgnoreWebStorage)
                ignoredPaths.Add(new RelPath(ConfigParts.Web, AppFolder.Storage));

            return new ConfigOptions()
            {
                ConfigParts = configParts,
                IgnoredPaths = ignoredPaths
            };
        }
    }
}
