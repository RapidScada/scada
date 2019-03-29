/*
 * Copyright 2019 Mikhail Shiryaev
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
 * Module   : ScadaCommCommon
 * Summary  : Specifies the communication line commands
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
 * Modified : 2019
 */

namespace Scada.Comm
{
    /// <summary>
    /// Specifies the communication line commands.
    /// <para>Задает команды линии связи.</para>
    /// </summary>
    public enum CommLineCmd
    {
        /// <summary>
        /// Start communication line.
        /// </summary>
        StartLine,

        /// <summary>
        /// Stop communication line.
        /// </summary>
        StopLine,

        /// <summary>
        /// Restart communication line.
        /// </summary>
        RestartLine
    }
}
