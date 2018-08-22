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
 * Module   : Server Shell
 * Summary  : Represents an environment of the Server shell
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using Scada.Server.Modules;
using System;
using System.Collections.Generic;

namespace Scada.Server.Shell.Code
{
    /// <summary>
    /// Represents an environment of the Server shell.
    /// <para>Представляет среду оболочки Сервера.</para>
    /// </summary>
    public class ServerEnvironment
    {
        /// <summary>
        /// Gets or sets the application directories.
        /// </summary>
        public AppDirs AppDirs { get; set; }

        /// <summary>
        /// Gets or sets the user interface of the modules accessed by short file name.
        /// </summary>
        public Dictionary<string, ModView> ModuleViews { get; set; }


        /// <summary>
        /// Validates the environment and throws an exception if it is incorrect.
        /// </summary>
        public void Validate()
        {
            if (AppDirs == null)
                throw new InvalidOperationException("AppDirs must not be null.");

            if (ModuleViews == null)
                throw new InvalidOperationException("ModuleViews must not be null.");
        }
    }
}
