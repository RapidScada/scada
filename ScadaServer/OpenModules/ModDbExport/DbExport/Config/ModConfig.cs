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
 * Summary  : Represents a module configuration
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2021
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;

namespace Scada.Server.Modules.DbExport.Config
{
    /// <summary>
    /// Represents a module configuration.
    /// <para>Представляет конфигурацию модуля.</para>
    /// </summary>
    [Serializable]
    internal class ModConfig : ITreeNode
    {
        /// <summary>
        /// The configuration file name.
        /// </summary>
        public const string ConfigFileName = "ModDbExport.xml";


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ModConfig()
        {
            SetToDefault();
        }


        /// <summary>
        /// Gets the configuration of the export targets.
        /// </summary>
        public List<ExportTargetConfig> ExportTargets { get; private set; }

        /// <summary>
        /// Gets or sets the parent node.
        /// </summary>
        ITreeNode ITreeNode.Parent
        {
            get
            {
                return null;
            }
            set
            {
                throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Get a list of child nodes.
        /// </summary>
        IList ITreeNode.Children
        {
            get
            {
                return ExportTargets;
            }
        }


        /// <summary>
        /// Sets the default values.
        /// </summary>
        private void SetToDefault()
        {
            ExportTargets = new List<ExportTargetConfig>();
        }

        /// <summary>
        /// Loads the configuration from the specified file.
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

                foreach (XmlElement exportTargetElem in xmlDoc.DocumentElement.SelectNodes("ExportTarget"))
                {
                    ExportTargetConfig exportTargetConfig = new ExportTargetConfig() { Parent = this };
                    exportTargetConfig.LoadFromXml(exportTargetElem);
                    ExportTargets.Add(exportTargetConfig);
                }

                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = ModPhrases.LoadModSettingsError + ": " + ex.Message;
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

                XmlElement rootElem = xmlDoc.CreateElement("ModDbExport");
                xmlDoc.AppendChild(rootElem);

                foreach (ExportTargetConfig exportTargetConfig in ExportTargets)
                {
                    exportTargetConfig.SaveToXml(rootElem.AppendElem("ExportTarget"));
                }

                xmlDoc.Save(fileName);
                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = ModPhrases.SaveModSettingsError + ": " + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Briefly validates the configuration.
        /// </summary>
        public bool Validate(out List<ExportTargetConfig> activeExportConfigs, out string errMsg)
        {
            activeExportConfigs = new List<ExportTargetConfig>(ExportTargets.Where(x => x.GeneralOptions.Active));

            if (activeExportConfigs.Count <= 0)
            {
                errMsg = Localization.UseRussian ?
                    "Отсутствуют активные цели экспорта." :
                    "No active export targets.";
                return false;
            }
            else if (ExportTargets.Distinct().Count() < ExportTargets.Count)
            {
                errMsg = Localization.UseRussian ?
                    "Дублируются идентификаторы целей экспорта." :
                    "Export target IDs are duplicated.";
                return false;
            }
            else
            {
                errMsg = "";
                return true;
            }
        }

        /// <summary>
        /// Clones the module configuration.
        /// </summary>
        public ModConfig Clone()
        {
            return ScadaUtils.DeepClone(this, new SerializationBinder(Assembly.GetExecutingAssembly()));
        }
    }
}
