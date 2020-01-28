/*
 * Copyright 2020 Mikhail Shiryaev
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
 * Summary  : Administrator settings
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2020
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Scada.Admin.Config
{
    /// <summary>
    /// Administrator settings.
    /// <para>Настройки Администратора.</para>
    /// </summary>
    public class AdminSettings
    {
        /// <summary>
        /// The default settings file name.
        /// </summary>
        public const string DefFileName = "ScadaAdminConfig.xml";


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public AdminSettings()
        {
            SetToDefault();
        }


        /// <summary>
        /// Gets the external application path options.
        /// </summary>
        public PathOptions PathOptions { get; private set; }

        /// <summary>
        /// Gets the channel generation options.
        /// </summary>
        public ChannelOptions ChannelOptions { get; private set; }

        /// <summary>
        /// Gets the associations between file extensions and editors.
        /// </summary>
        public SortedList<string, string> FileAssociations { get; private set; }


        /// <summary>
        /// Sets the default values.
        /// </summary>
        private void SetToDefault()
        {
            PathOptions = new PathOptions();
            ChannelOptions = new ChannelOptions();
            FileAssociations = new SortedList<string, string>();

            if (ScadaUtils.IsRunningOnWin)
            {
                FileAssociations.Add("sch", @"C:\SCADA\ScadaSchemeEditor\ScadaSchemeEditor.exe");
                FileAssociations.Add("tbl", @"C:\SCADA\ScadaTableEditor\ScadaTableEditor.exe");
            }
            else
            {
                FileAssociations.Add("sch", "/opt/scada/ScadaSchemeEditor/ScadaSchemeEditor.exe");
                FileAssociations.Add("tbl", "/opt/scada/ScadaTableEditor/ScadaTableEditor.exe");
            }
        }

        /// <summary>
        /// Loads the settings from the specified file.
        /// </summary>
        public bool Load(string fileName, out string errMsg)
        {
            try
            {
                SetToDefault();

                if (!File.Exists(fileName))
                    throw new FileNotFoundException(string.Format(CommonPhrases.NamedFileNotFound, fileName));

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(fileName);
                XmlElement rootElem = xmlDoc.DocumentElement;

                if (rootElem.SelectSingleNode("PathOptions") is XmlNode pathOptionsNode)
                    PathOptions.LoadFromXml(pathOptionsNode);

                if (rootElem.SelectSingleNode("ChannelOptions") is XmlNode channelOptionsNode)
                    ChannelOptions.LoadFromXml(channelOptionsNode);

                if (rootElem.SelectSingleNode("FileAssociations") is XmlNode fileAssociationsNode)
                {
                    foreach (XmlElement associationElem in fileAssociationsNode.SelectNodes("Association"))
                    {
                        string ext = associationElem.GetAttrAsString("ext").ToLowerInvariant();
                        string path = associationElem.GetAttrAsString("path");
                        FileAssociations[ext] = path;
                    }
                }

                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = CommonPhrases.LoadAppSettingsError + ": " + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Saves the settings to the specified file.
        /// </summary>
        public bool Save(string fileName, out string errMsg)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                XmlDeclaration xmlDecl = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
                xmlDoc.AppendChild(xmlDecl);

                XmlElement rootElem = xmlDoc.CreateElement("ScadaAdminConfig");
                xmlDoc.AppendChild(rootElem);

                PathOptions.SaveToXml(rootElem.AppendElem("PathOptions"));
                ChannelOptions.SaveToXml(rootElem.AppendElem("ChannelOptions"));

                XmlElement fileAssociationsElem = rootElem.AppendElem("FileAssociations");
                foreach (KeyValuePair<string, string> pair in FileAssociations)
                {
                    XmlElement associationElem = fileAssociationsElem.AppendElem("Association");
                    associationElem.SetAttribute("ext", pair.Key);
                    associationElem.SetAttribute("path", pair.Value);
                }

                xmlDoc.Save(fileName);
                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = CommonPhrases.SaveAppSettingsError + ": " + ex.Message;
                return false;
            }
        }
    }
}
