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
 * Module   : Agent Service
 * Summary  : Agent service for Windows
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using System.ServiceProcess;

namespace Scada.Agent.Svc
{
    /// <summary>
    /// Agent service for Windows
    /// <para>Служба агента для Windows</para>
    /// </summary>
    static class Program
    {
        /// <summary>
        /// The main entry point for the application
        /// <para>Основная точка входа для приложения</para>
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] { new SvcMain() };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
