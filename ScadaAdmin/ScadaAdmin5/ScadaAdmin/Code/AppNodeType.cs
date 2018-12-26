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
 * Summary  : Types of the application tree nodes
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

namespace Scada.Admin.App.Code
{
    /// <summary>
    /// Types of the application tree nodes.
    /// <para>Типы узлов дерева приложения.</para>
    /// </summary>
    internal static class AppNodeType
    {
        public const string Project = "Project";
        public const string Base = "Base";
        public const string BaseTable = "BaseTable";
        public const string Interface = "Interface";
        public const string Instances = "Instances";
        public const string Instance = "Instance";
        public const string ServerApp = "ServerApp";
        public const string CommApp = "CommApp";
        public const string WebApp = "WebApp";
        public const string Directory = "Directory";
        public const string File = "File";
    }
}
