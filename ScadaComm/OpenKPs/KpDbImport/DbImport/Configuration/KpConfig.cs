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
 * Module   : KpDBImport
 * Summary  : Driver configuration
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2020
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Scada.Comm.Devices.DbImport.Configuration
{
    /// <summary>
    /// Driver configuration.
    /// <para>Конфигурация драйвера.</para>
    /// </summary>
    internal class KpConfig
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public KpConfig()
        {
            SetToDefault();
        }


        /// <summary>
        /// Gets or sets the data source type.
        /// </summary>
        public DataSourceType DataSourceType { get; set; }

        /// <summary>
        /// Gets the DB connection settings.
        /// </summary>
        public DbConnSettings DbConnSettings { get; private set; }

        /// <summary>
        /// Gets or sets the SQL-query to retrieve data.
        /// </summary>
        public string SelectQuery { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to calculate tag count automatically by parsing the query.
        /// </summary>
        public bool AutoTagCount { get; set; }

        /// <summary>
        /// Gets or sets the exact number of tags.
        /// </summary>
        public int TagCount { get; set; }

        /// <summary>
        /// Gets the export commands.
        /// </summary>
        public List<ExportCmd> ExportCmds { get; private set; }


        /// <summary>
        /// Sets the default values.
        /// </summary>
        private void SetToDefault()
        {
            DataSourceType = DataSourceType.Undefined;
            DbConnSettings = new DbConnSettings();
            SelectQuery = "";
            AutoTagCount = true;
            TagCount = 0;
            ExportCmds = new List<ExportCmd>();
        }


        /// <summary>
        /// Loads the configuration from the specified file.
        /// </summary>
        public bool Load(string fileName, out string errMsg)
        {
            SetToDefault();

            try
            {
                if (!File.Exists(fileName))
                    throw new FileNotFoundException(string.Format(CommonPhrases.NamedFileNotFound, fileName));

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(fileName);
                XmlElement rootElem = xmlDoc.DocumentElement;

                DataSourceType = rootElem.GetChildAsEnum<DataSourceType>("DataSourceType");
                DbConnSettings.LoadFromXml(rootElem.SelectSingleNode("DbConnSettings"));
                SelectQuery = rootElem.GetChildAsString("SelectQuery");
                AutoTagCount = rootElem.GetChildAsBool("AutoTagCount");
                TagCount = rootElem.GetChildAsInt("TagCount");

                if (rootElem.SelectSingleNode("ExportCmds") is XmlNode exportCmdsNode)
                {
                    foreach (XmlNode exportCmdNode in exportCmdsNode.SelectNodes("ExportCmd"))
                    {
                        ExportCmd exportCmd = new ExportCmd();
                        exportCmd.LoadFromXml(exportCmdNode);
                        ExportCmds.Add(exportCmd);
                    }

                    ExportCmds.Sort();
                }

                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = CommPhrases.LoadKpSettingsError + ":" + Environment.NewLine + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Saves the configuration to the specified file.
        /// </summary>
        public bool Save(string fileName, out string errMsg)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                XmlDeclaration xmlDecl = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
                xmlDoc.AppendChild(xmlDecl);

                XmlElement rootElem = xmlDoc.CreateElement("KpDbImportConfig");
                xmlDoc.AppendChild(rootElem);

                rootElem.AppendElem("DataSourceType", DataSourceType);
                DbConnSettings.SaveToXml(rootElem.AppendElem("DbConnSettings"));
                rootElem.AppendElem("SelectQuery", SelectQuery);
                rootElem.AppendElem("AutoTagCount", AutoTagCount);
                rootElem.AppendElem("TagCount", TagCount);

                XmlElement exportCmdsElem = rootElem.AppendElem("ExportCmds");
                foreach (ExportCmd exportCmd in ExportCmds)
                {
                    exportCmd.SaveToXml(exportCmdsElem.AppendElem("ExportCmd"));
                }

                xmlDoc.Save(fileName);
                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = CommPhrases.SaveKpSettingsError + ":" + Environment.NewLine + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Calculates tag count based on the SQL-query.
        /// </summary>
        public int CalcTagCount()
        {
            int tagCount = 0;

            if (!string.IsNullOrEmpty(SelectQuery))
            {
                // count the number of words between select and from separated by commas
                int selectInd = SelectQuery.IndexOf("select", StringComparison.OrdinalIgnoreCase);
                int fromInd = SelectQuery.IndexOf("from", StringComparison.OrdinalIgnoreCase);

                if (selectInd >= 0)
                {
                    if (fromInd < 0)
                        fromInd = SelectQuery.Length - 1;

                    for (int i = selectInd + "select".Length; i < fromInd; i++)
                    {
                        if (SelectQuery[i] == ',')
                            tagCount++;
                    }

                    tagCount++;
                }
            }

            return tagCount;
        }

        /// <summary>
        /// Gets the configuration file name.
        /// </summary>
        public static string GetFileName(string configDir, int kpNum)
        {
            return Path.Combine(configDir, "KpDbImport_" + CommUtils.AddZeros(kpNum, 3) + ".xml");
        }
    }
}
