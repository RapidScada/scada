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
 * Summary  : Represents a system instance ready to change
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using Scada.Admin.Project;
using Scada.Comm.Shell.Code;
using Scada.Server.Shell.Code;
using System;

namespace Scada.Admin.App.Code
{
    /// <summary>
    /// Represents a system instance ready to change.
    /// <para>Представляет экземпляр системы, готовый к изменениям.</para>
    /// </summary>
    internal class LiveInstance
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public LiveInstance(Instance instance)
        {
            Instance = instance ?? throw new ArgumentNullException("instance");
            ServerEnvironment = null;
            CommEnvironment = null;
        }


        /// <summary>
        /// Gets or sets the system instance.
        /// </summary>
        public Instance Instance { get; set; }
        
        /// <summary>
        /// Gets or sets the Server environment.
        /// </summary>
        public ServerEnvironment ServerEnvironment { get; set; }

        /// <summary>
        /// Gets or sets the Communicator environment.
        /// </summary>
        public CommEnvironment CommEnvironment { get; set; }
    }
}
