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
 * Module   : ScadaData
 * Summary  : Represents a device as the configuration database entity
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

#pragma warning disable 1591 // Missing XML comment for publicly visible type or member

using System;

namespace Scada.Data.Entities
{
    /// <summary>
    /// Represents a device as the configuration database entity.
    /// <para>Представляет КП как сущность базы конфигурации.</para>
    /// </summary>
    /// <remarks>KP is kontrollierter punkt (German).</remarks>
    [Serializable]
    public class KP
    {
        public int KPNum { get; set; }

        public string Name { get; set; }

        public int KPTypeID { get; set; }

        public int? Address { get; set; }

        public string CallNum { get; set; }

        public int? CommLineNum { get; set; }

        public string Descr { get; set; }
    }
}
