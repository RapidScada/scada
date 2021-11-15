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
 * Summary  : Represents an export target configuration
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

using Scada.Config;
using System;
using System.Collections;
using System.Xml;

namespace Scada.Server.Modules.DbExport.Config
{
    /// <summary>
    /// Represents an export target configuration.
    /// <para>Представляет конфигурацию цели экспорта.</para>
    /// </summary>
    [Serializable]
    internal class ExportTargetConfig : ITreeNode, IComparable<ExportTargetConfig>
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ExportTargetConfig()
        {
            GeneralOptions = new GeneralOptions();
            ConnectionOptions = new DbConnectionOptions();
            Triggers = new TriggerOptionList { Parent = this };
            ArcUploadOptions = new ArcUploadOptions();
            Parent = null;
        }


        /// <summary>
        /// Gets the general options.
        /// </summary>
        public GeneralOptions GeneralOptions { get; private set; }

        /// <summary>
        /// Gets the database connection options.
        /// </summary>
        public DbConnectionOptions ConnectionOptions { get; private set; }

        /// <summary>
        /// Gets the configuration of the triggers.
        /// </summary>
        public TriggerOptionList Triggers { get; }

        /// <summary>
        /// Gets the archive upload options.
        /// </summary>
        public ArcUploadOptions ArcUploadOptions { get; private set; }

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
                return Triggers;
            }
        }


        /// <summary>
        /// Loads the configuration from the XML node.
        /// </summary>
        public void LoadFromXml(XmlElement xmlElem)
        {
            if (xmlElem == null)
                throw new ArgumentNullException("xmlElem");

            if (xmlElem.SelectSingleNode("GeneralOptions") is XmlNode generalOptionsNode)
                GeneralOptions.LoadFromXml(generalOptionsNode);

            if (xmlElem.SelectSingleNode("ConnectionOptions") is XmlNode connectionOptionsNode)
                ConnectionOptions.LoadFromXml(connectionOptionsNode);

            if (xmlElem.SelectSingleNode("Triggers") is XmlNode triggersNode)
                Triggers.LoadFromXml(triggersNode);

            if (xmlElem.SelectSingleNode("ArcUploadOptions") is XmlElement arcUploadOptionsElem)
                ArcUploadOptions.LoadFromXml(arcUploadOptionsElem);
        }

        /// <summary>
        /// Saves the configuration into the XML node.
        /// </summary>
        public void SaveToXml(XmlElement xmlElem)
        {
            if (xmlElem == null)
                throw new ArgumentNullException("xmlElem");

            GeneralOptions.SaveToXml(xmlElem.AppendElem("GeneralOptions"));
            ConnectionOptions.SaveToXml(xmlElem.AppendElem("ConnectionOptions"));
            Triggers.SaveToXml(xmlElem.AppendElem("Triggers"));
            ArcUploadOptions.SaveToXml(xmlElem.AppendElem("ArcUploadOptions"));
        }

        /// <summary>
        /// Compares the current instance with another object of the same type.
        /// </summary>
        /// <remarks>Required for the ModConfig.Validate method.</remarks>
        public int CompareTo(ExportTargetConfig other)
        {
            return GeneralOptions.ID.CompareTo(other.GeneralOptions.ID);
        }
    }
}
