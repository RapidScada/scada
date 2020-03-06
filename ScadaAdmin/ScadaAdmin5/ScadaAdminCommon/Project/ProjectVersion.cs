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
 * Summary  : Represents a project version
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
 * Modified : 2020
 */

namespace Scada.Admin.Project
{
    /// <summary>
    /// Represents a project version.
    /// <para>Представляет версию проекта.</para>
    /// </summary>
    public struct ProjectVersion
    {
        /// <summary>
        /// The default project version.
        /// </summary>
        public static ProjectVersion Default = new ProjectVersion(1, 0);


        /// <summary>
        /// Initializes a new instance of the structure.
        /// </summary>
        public ProjectVersion(int major, int minor)
        {
            Major = major;
            Minor = minor;
        }


        /// <summary>
        /// Gets or sets the major version.
        /// </summary>
        public int Major { get; set; }

        /// <summary>
        /// Gets or sets the minor version.
        /// </summary>
        public int Minor { get; set; }


        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        public override string ToString()
        {
            return Major + "." + Minor;
        }

        /// <summary>
        /// Converts the string representation to a version.
        /// </summary>
        public static ProjectVersion Parse(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return Default;
            }
            else
            {
                string[] parts = s.Split('.');
                return new ProjectVersion(
                    parts.Length > 0 && int.TryParse(parts[0], out int n) ? n : 0,
                    parts.Length > 1 && int.TryParse(parts[1], out n) ? n : 0);
            }
        }
    }
}
