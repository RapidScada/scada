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
 * Summary  : Represents a project of a configuration
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace Scada.Admin.Project
{
    /// <summary>
    /// Represents a project of a configuration.
    /// <para>Представляет проект конфигурации.</para>
    /// </summary>
    public class ScadaProject
    {
        /// <summary>
        /// The default project name.
        /// </summary>
        public const string DefaultName = "NewProject";


        /// <summary>
        /// The project file name.
        /// </summary>
        protected string fileName;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ScadaProject()
        {
            SetToDefault();
        }


        /// <summary>
        /// Gets or sets the name of the project.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets the project file name.
        /// </summary>
        public string FileName
        {
            get
            {
                return fileName;
            }
            protected set
            {
                fileName = value;

                if (string.IsNullOrEmpty(fileName))
                {
                    ConfigBase.BaseDir = "";
                    Interface.InterfaceDir = "";
                }
                else
                {
                    string projectDir = Path.GetDirectoryName(FileName);
                    ConfigBase.BaseDir = Path.Combine(projectDir, "BaseXML");
                    Interface.InterfaceDir = Path.Combine(projectDir, "Interface");

                    foreach (Instance instance in Instances)
                    {
                        instance.InstanceDir = Path.Combine(projectDir, "Instances", instance.Name);
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the project description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets the configuration database.
        /// </summary>
        public ConfigBase ConfigBase { get; protected set; }

        /// <summary>
        /// Gets the interface metadata.
        /// </summary>
        public Interface Interface { get; protected set; }

        /// <summary>
        /// Gets the instances including the appropriate settings of the applications.
        /// </summary>
        public List<Instance> Instances { get; protected set; }


        /// <summary>
        /// Sets the default values.
        /// </summary>
        private void SetToDefault()
        {
            fileName = "";

            Name = DefaultName;
            Description = "";
            ConfigBase = new ConfigBase();
            Interface = new Interface();
            Instances = new List<Instance>();
        }

        /// <summary>
        /// Clears the project properties.
        /// </summary>
        private void Clear()
        {
            Name = "";
            FileName = "";
            Description = "";
            ConfigBase = new ConfigBase();
            Interface = new Interface();
            Instances.Clear();
        }

        /// <summary>
        /// Loads the project from the specified file.
        /// </summary>
        public void Load(string fileName)
        {
            Clear();
            Name = Path.GetFileNameWithoutExtension(fileName);
            FileName = fileName;

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(fileName);

            XmlElement rootElem = xmlDoc.DocumentElement;
            Description = rootElem.GetChildAsString("Description");

            // load instances
            XmlNode instancesNode = rootElem.SelectSingleNode("Instances");

            if (instancesNode != null)
            {
                XmlNodeList instanceNodes = instancesNode.SelectNodes("Instance");
                string projectDir = Path.GetDirectoryName(FileName);

                foreach (XmlNode instanceNode in instanceNodes)
                {
                    Instance instance = new Instance();
                    instance.LoadFromXml(instanceNode);
                    instance.InstanceDir = Path.Combine(projectDir, "Instances", instance.Name);
                    Instances.Add(instance);
                }
            }
        }

        /// <summary>
        /// Tries to load the project from the specified file.
        /// </summary>
        public bool Load(string fileName, out string errMsg)
        {
            try
            {
                Load(fileName);
                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = AdminPhrases.LoadProjectError + ": " + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Saves the project to the specified file.
        /// </summary>
        public void Save(string fileName)
        {
            Name = Path.GetFileNameWithoutExtension(fileName);
            FileName = fileName;

            XmlDocument xmlDoc = new XmlDocument();
            XmlDeclaration xmlDecl = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
            xmlDoc.AppendChild(xmlDecl);

            XmlElement rootElem = xmlDoc.CreateElement("ScadaProject");
            rootElem.AppendElem("Version", AdminUtils.AppVersion);
            rootElem.AppendElem("Description", Description);
            xmlDoc.AppendChild(rootElem);

            // save intances
            XmlElement instancesElem = xmlDoc.CreateElement("Instances");
            xmlDoc.AppendChild(instancesElem);

            foreach (Instance instance in Instances)
            {
                XmlElement instanceElem = xmlDoc.CreateElement("Instance");
                instance.SaveToXml(instanceElem);
                instancesElem.AppendChild(instanceElem);
            }

            xmlDoc.Save(fileName);
        }

        /// <summary>
        /// Tries to save the project to the specified file.
        /// </summary>
        public bool Save(string fileName, out string errMsg)
        {
            try
            {
                Save(fileName);
                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = AdminPhrases.SaveProjectError + ": " + ex.Message;
                return false;
            }
        }


        /// <summary>
        /// Creates a new project with the specified parameters.
        /// </summary>
        public static bool Create(string name, string location, string template, 
            out ScadaProject project, out string errMsg)
        {
            try
            {
                project = new ScadaProject() { Name = name };

                if (File.Exists(template))
                    project.Load(template);

                string projectDir = Path.Combine(location, name);

                if (Directory.Exists(projectDir))
                    Directory.Delete(projectDir, true);

                Directory.CreateDirectory(projectDir);
                project.Description = "";
                project.Save(Path.Combine(projectDir, name + AdminUtils.ProjectExt));

                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                project = null;
                errMsg = AdminPhrases.CreateProjectError + ": " + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Loads the project description from the specified project file.
        /// </summary>
        public static bool LoadDescription(string fileName, out string description, out string errMsg)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(fileName);
                description = xmlDoc.DocumentElement.GetChildAsString("Description");
                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                description = "";
                errMsg = AdminPhrases.LoadProjectDescrError + ": " + ex.Message;
                return false;
            }
        }
    }
}
