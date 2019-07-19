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
 * Module   : Report Builder
 * Summary  : Specifies the possible report start dates
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
 * Modified : 2019
 */

#pragma warning disable 1591 // Missing XML comment for publicly visible type or member

namespace Utils.Report
{
    /// <summary>
    /// Specifies the possible report start dates.
    /// <para>Задает возможные даты начала отчёта.</para>
    /// </summary>
    public enum RepStartDate
    {
        Today,
        Yesterday
    }
}
