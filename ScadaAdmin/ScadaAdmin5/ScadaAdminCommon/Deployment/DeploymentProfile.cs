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
 * Summary  : Deployment profile
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using Scada.Agent.Connector;

namespace Scada.Admin.Deployment
{
    /// <summary>
    /// Deployment profile.
    /// <para>Профиль развёртывания.</para>
    /// </summary>
    public class DeploymentProfile
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DeploymentProfile()
        {
            Name = "";
            ConnectionSettings = new ConnectionSettings();
            DownloadSettings = new DownloadSettings();
            UploadSettings = new UploadSettings();
        }


        /// <summary>
        /// Gets or sets the profile name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets the connection settings.
        /// </summary>
        public ConnectionSettings ConnectionSettings { get; protected set; }

        /// <summary>
        /// Gets the download settings.
        /// </summary>
        public DownloadSettings DownloadSettings { get; protected set; }

        /// <summary>
        /// Gets the upload settings.
        /// </summary>
        public UploadSettings UploadSettings { get; protected set; }
    }
}
