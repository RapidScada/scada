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
using System.Text;

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
        /// Initializes a new instance of the class.
        /// </summary>
        public ScadaProject()
        {
            Name = DefaultName;
            ConfigBase = new ConfigBase();
            Interface = new Interface();
            Instances = new List<Instance>
            {
                new Instance() { Name = Instance.DefaultName }
            };
        }


        /// <summary>
        /// Gets or sets the name of the project.
        /// </summary>
        public string Name { get; set; }

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
        /// Loads the project from the specified file.
        /// </summary>
        public void Load(string fileName)
        {

        }

        /// <summary>
        /// Saves the project to the specified file.
        /// </summary>
        public void Save(string fileName)
        {
        }


        /// <summary>
        /// Creates a new project with the specified parameters.
        /// </summary>
        public static ScadaProject Create(string name, string location, string template)
        {
            return new ScadaProject();
        }

        /// <summary>
        /// Loads the project description from the specified project file.
        /// </summary>
        public static bool LoadDescription(string fileName, out string description, out string errMsg)
        {
            description = fileName;
            errMsg = "";
            return true;
        }
    }
}
