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
 * Module   : Communicator Shell
 * Summary  : Types of Communicator tree nodes
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

#pragma warning disable 1591 // Missing XML comment for publicly visible type or member

namespace Scada.Comm.Shell.Code
{
    /// <summary>
    /// Types of Communicator tree nodes.
    /// <para>Типы узлов дерева Коммуникатора.</para>
    /// </summary>
    public static class CommNodeType
    {
        public const string CommonParams = "CommonParams";
        public const string Drivers = "Drivers";
        public const string CommLines = "CommLines";
        public const string CommLine = "CommLine";
        public const string LineParams = "LineParams";
        public const string LineStats = "LineStats";
        public const string Device = "Device";
        public const string Stats = "Stats";
    }
}
