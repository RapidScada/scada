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
 * Module   : Administrator
 * Summary  : Directories of the Server application
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using Scada.Admin.Project;

namespace Scada.Admin.App.Code
{
    /// <summary>
    /// Directories of the Server application.
    /// <para>Директории приложения Сервер.</para>
    /// </summary>
    internal class ServerDirs : Server.AppDirs
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ServerDirs(string serverDir, Instance instance)
            : base()
        {
            Init(serverDir);
            ConfigDir = ScadaUtils.NormalDir(instance.ServerApp.GetConfigDir());
        }
    }
}
