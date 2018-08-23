/*
 * Copyright 2016 Mikhail Shiryaev
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
 * Module   : SCADA-Communicator Service
 * Summary  : Service installer
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

using Scada.Svc;
using System.ComponentModel;

namespace Scada.Comm.Svc
{
    /// <summary>
    /// Service installer
    /// <para>Инсталлятор службы</para>
    /// </summary>
    [RunInstaller(true)]
    public class SvcInstaller : BaseSvcInstaller
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public SvcInstaller()
        {
            Init(
                "ScadaCommService",
                "SCADA-Communicator interacts with controllers, transmits data to SCADA-Server");
        }
    }
}
