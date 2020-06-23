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
 * Module   : ScadaServerCommon
 * Summary  : Snapshot types
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2020
 */

namespace Scada.Server
{
    /// <summary>
    /// Snapshot types
    /// <para>Типы срезов</para>
    /// </summary>
    public enum SnapshotType
    {
        /// <summary>
        /// Текущий
        /// </summary>
        Cur,
        /// <summary>
        /// Минутный
        /// </summary>
        Min,
        /// <summary>
        /// Часовой
        /// </summary>
        Hour
    }
}
