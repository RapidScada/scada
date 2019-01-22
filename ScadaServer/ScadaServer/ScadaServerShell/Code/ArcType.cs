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
 * Module   : Server Shell
 * Summary  : Specifies the type of archive
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
 * Modified : 2019
 */

namespace Scada.Server.Shell.Code
{
    /// <summary>
    /// Specifies the type of archive.
    /// <para>Задает тип архива.</para>
    /// </summary>
    public enum ArcType
    {
        /// <summary>
        /// Current data.
        /// </summary>
        CurData,

        /// <summary>
        /// Minute data.
        /// </summary>
        MinData,

        /// <summary>
        /// Hourly data.
        /// </summary>
        HourData,

        /// <summary>
        /// Events.
        /// </summary>
        Events
    };
}
