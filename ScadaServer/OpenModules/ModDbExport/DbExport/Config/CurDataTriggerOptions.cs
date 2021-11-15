/*
 * Copyright 2021 Mikhail Shiryaev
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
 * Module   : ModDbExport
 * Summary  : Represents current data trigger options
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

using System;

namespace Scada.Server.Modules.DbExport.Config
{
    /// <summary>
    /// Represents current data trigger options.
    /// <para>Представляет параметры триггера на текущие данные.</para>
    /// </summary>
    [Serializable]
    internal class CurDataTriggerOptions : DataTriggerOptions
    {
        /// <summary>
        /// Gets the trigger type name.
        /// </summary>
        public override string TriggerType => "CurDataTrigger";
    }
}
