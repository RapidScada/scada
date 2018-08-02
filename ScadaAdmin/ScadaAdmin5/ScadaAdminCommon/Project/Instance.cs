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
 * Summary  : Represents a system instance that consists of one or more applications
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scada.Admin.Project
{
    /// <summary>
    /// Represents a system instance that consists of one or more applications
    /// <para>Представляет экземпляр системы, состоящий из одного или нескольких приложений</para>
    /// </summary>
    public class Instance
    {
        /// <summary>
        /// Gets or sets the object represents the Server application
        /// </summary>
        public ServerApp ServerApp { get; set; }

        /// <summary>
        /// Gets or sets the object represents the Communicator application
        /// </summary>
        public CommApp CommApp { get; set; }

        /// <summary>
        /// Gets or sets the object represents the Webstation application
        /// </summary>
        public WebApp WebApp { get; set; }
    }
}
