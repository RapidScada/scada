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
 * Summary  : Represents a project of a configuration
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2020
 */

using Scada.Admin.Deployment;
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
                Name = Path.GetFileNameWithoutExtension(fileName);

                if (string.IsNullOrEmpty(fileName))
                {
                    ConfigBase.BaseDir = "";
                    Interface.InterfaceDir = "";
                    DeploymentSettings.ProjectDir = "";

                    foreach (Instance instance in Instances)
                    {
                        instance.InstanceDir = "";
                    }
                }
                else
                {
                    string projectDir = Path.GetDirectoryName(FileName);
                    ConfigBase.BaseDir = Path.Combine(projectDir, "BaseXML");
                    Interface.InterfaceDir = Path.Combine(projectDir, "Interface");
                    DeploymentSettings.ProjectDir = projectDir;

                    foreach (Instance instance in Instances)
                    {
                        instance.InstanceDir = Path.Combine(projectDir, "Instances", instance.Name);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the project directory.
        /// </summary>
        public string ProjectDir
        {
            get
            {
                return Path.GetDirectoryName(FileName);
            }
        }

        /// <summary>
        /// Gets or sets the project version.
        /// </summary>
        public ProjectVersion Version { get; set; }

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
        /// Gets the deployment settings.
        /// </summary>
        public DeploymentSettings DeploymentSettings { get; protected set; }


        /// <summary>
        /// Sets the default values.
        /// </summary>
        private void SetToDefault()
        {
            fileName = "";

            Name = DefaultName;
            Version = ProjectVersion.Default;
            Description = "";
            ConfigBase = new ConfigBase();
            Interface = new Interface();
            Instances = new List<Instance>();
            DeploymentSettings = new DeploymentSettings();
        }

        /// <summary>
        /// Clears the project properties.
        /// </summary>
        private void Clear()
        {
            fileName = "";

            Name = "";
            Description = "";
            ConfigBase = new ConfigBase();
            Interface = new Interface();
            Instances.Clear();
            DeploymentSettings = new DeploymentSettings();
        }

        /// <summary>
        /// Gets a file name to store a project.
        /// </summary>
        private static string GetProjectFileName(string projectDir, string projectName)
        {
            return Path.Combine(projectDir, projectName + AdminUtils.ProjectExt);
        }

        /// <summary>
        /// Copies the content of the directory.
        /// </summary>
        private static void CopyDirectory(DirectoryInfo source, DirectoryInfo dest)
        {
            Directory.CreateDirectory(dest.FullName);

            foreach (DirectoryInfo sourceSubdir in source.GetDirectories())
            {
                DirectoryInfo destSubdir = dest.CreateSubdirectory(sourceSubdir.Name);
                CopyDirectory(sourceSubdir, destSubdir);
            }

            foreach (FileInfo fileInfo in source.GetFiles())
            {
                fileInfo.CopyTo(Path.Combine(dest.FullName, fileInfo.Name), true);
            }
        }

        /// <summary>
        /// Gets the maximum ID of existing instances.
        /// </summary>
        private int GetMaxInstanceID()
        {
            int maxID = 0;

            foreach (Instance instance in Instances)
            {
                if (instance.ID > maxID)
                    maxID = instance.ID;
            }

            return maxID;
        }


        /// <summary>
        /// Loads the project from the specified file.
        /// </summary>
        public void Load(string fileName)
        {
            Clear();
            FileName = fileName;

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(fileName);

            XmlElement rootElem = xmlDoc.DocumentElement;
            Version = ProjectVersion.Parse(rootElem.GetChildAsString("ProjectVersion"));
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

                    // fix instance ID
                    if (instance.ID <= 0)
                        instance.ID = Instances.Count;
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
            FileName = fileName;

            XmlDocument xmlDoc = new XmlDocument();
            XmlDeclaration xmlDecl = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
            xmlDoc.AppendChild(xmlDecl);

            XmlElement rootElem = xmlDoc.CreateElement("ScadaProject");
            rootElem.AppendElem("AdminVersion", AdminUtils.AppVersion);
            rootElem.AppendElem("ProjectVersion", Version);
            rootElem.AppendElem("Description", Description);
            xmlDoc.AppendChild(rootElem);

            // save intances
            XmlElement instancesElem = xmlDoc.CreateElement("Instances");
            rootElem.AppendChild(instancesElem);

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
        /// Renames the instance.
        /// </summary>
        public bool Rename(string name, out string errMsg)
        {
            try
            {
                if (!AdminUtils.NameIsValid(name))
                    throw new ArgumentException(AdminPhrases.IncorrectProjectName);

                string projectDir = Path.GetDirectoryName(FileName);
                DirectoryInfo directoryInfo = new DirectoryInfo(projectDir);
                string newProjectDir = Path.Combine(directoryInfo.Parent.FullName, name);

                if (Directory.Exists(newProjectDir))
                    throw new ScadaException(AdminPhrases.ProjectDirectoryExists);

                File.Move(FileName, GetProjectFileName(projectDir, name));
                Directory.Move(projectDir, newProjectDir);
                FileName = GetProjectFileName(newProjectDir, name);

                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = AdminPhrases.RenameProjectError + ": " + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Creates a new instance that is not added to any project.
        /// </summary>
        public Instance CreateInstance(string name)
        {
            string projectDir = Path.GetDirectoryName(FileName);
            return new Instance()
            {
                ID = GetMaxInstanceID() + 1,
                Name = name,
                InstanceDir = Path.Combine(projectDir, "Instances", name)
            };
        }

        /// <summary>
        /// Determines whether an instance is in the project.
        /// </summary>
        public bool ContainsInstance(string name)
        {
            foreach (Instance instance in Instances)
            {
                if (string.Equals(instance.Name, name, StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Gets the existing instance names.
        /// </summary>
        public HashSet<string> GetInstanceNames(bool lowerCase, string exceptName = null)
        {
            HashSet<string> instanceNames = new HashSet<string>();
            exceptName = exceptName == null ? null : (lowerCase ? exceptName.ToLowerInvariant() : exceptName);

            foreach (Instance instance in Instances)
            {
                string instanceName = lowerCase ? instance.Name.ToLowerInvariant() : instance.Name;
                if (exceptName == null || instanceName != exceptName)
                    instanceNames.Add(instanceName);
            }

            return instanceNames;
        }

        /// <summary>
        /// Gets the project info.
        /// </summary>
        public string GetInfo()
        {
            StringBuilder sbInfo = new StringBuilder();

            if (Localization.UseRussian)
            {
                sbInfo
                    .Append("Наименование проекта : ").AppendLine(Name)
                    .Append("Версия проекта       : ").AppendLine(Version.ToString())
                    .Append("Метка времени        : ").AppendLine(DateTime.Now.ToLocalizedString());
            }
            else
            {
                sbInfo
                    .Append("Project name    : ").AppendLine(Name)
                    .Append("Project version : ").AppendLine(Version.ToString())
                    .Append("Timestamp       : ").AppendLine(DateTime.Now.ToLocalizedString());
            }

            return sbInfo.ToString();
        }


        /// <summary>
        /// Creates a new project with the specified parameters.
        /// </summary>
        public static bool Create(string name, string location, string template, 
            out ScadaProject project, out string errMsg)
        {
            try
            {
                // validate arguments
                if (!AdminUtils.NameIsValid(name))
                    throw new ArgumentException(AdminPhrases.IncorrectProjectName);

                string projectDir = Path.Combine(location, name);

                if (Directory.Exists(projectDir))
                    throw new ScadaException(AdminPhrases.ProjectDirectoryExists);

                // copy template
                FileInfo templateFileInfo = new FileInfo(template);
                string projectFileName = GetProjectFileName(projectDir, name);

                if (templateFileInfo.Exists)
                {
                    CopyDirectory(templateFileInfo.Directory, new DirectoryInfo(projectDir));
                    File.Move(Path.Combine(projectDir, templateFileInfo.Name), projectFileName);
                }

                // create project
                project = new ScadaProject { Name = name };

                if (File.Exists(projectFileName))
                {
                    // load from template
                    project.Load(projectFileName);
                    project.Description = "";
                }

                project.Save(project.FileName);

                // create the necessary directories
                Directory.CreateDirectory(projectDir);
                Directory.CreateDirectory(project.ConfigBase.BaseDir);
                Directory.CreateDirectory(project.Interface.InterfaceDir);

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
