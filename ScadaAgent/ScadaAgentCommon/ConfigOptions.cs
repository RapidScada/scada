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
 * Module   : ScadaAgentCommon
 * Summary  : Configuration transfer options
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using System.Collections.Generic;

namespace Scada.Agent
{
    /// <summary>
    /// Configuration transfer options.
    /// <para>Параметры передачи конфигурации.</para>
    /// </summary>
    public class ConfigOptions
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ConfigOptions()
        {
            ConfigParts = ConfigParts.All;
            IgnoredPaths = new List<RelPath>();
        }


        /// <summary>
        /// Gets or sets the configuration parts.
        /// </summary>
        public ConfigParts ConfigParts { get; set; }

        /// <summary>
        /// Gets or sets the ignored paths.
        /// </summary>
        public ICollection<RelPath> IgnoredPaths { get; set; }
    }
}
