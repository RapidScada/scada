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
 * Summary  : Represents an input channel as the configuration database entity
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
    /// Represents an input channel as the configuration database entity.
    /// <para>Представляет входной канал как сущность базы конфигурации.</para>
    /// </summary>
    [Serializable]
    public class InCnl
    {
        public int CnlNum { get; set; }

        public bool Active { get; set; }

        public string Name { get; set; }

        public int CnlTypeID { get; set; }

        public int? ObjNum { get; set; }

        public int? KPNum { get; set; }

        public int? Signal { get; set; }

        public bool FormulaUsed { get; set; }

        public string Formula { get; set; }

        public bool Averaging { get; set; }

        public int? ParamID { get; set; }

        public int? FormatID { get; set; }

        public int? UnitID { get; set; }

        public int? CtrlCnlNum { get; set; }

        public bool EvEnabled { get; set; }

        public bool EvSound { get; set; }

        public bool EvOnChange { get; set; }

        public bool EvOnUndef { get; set; }

        public double? LimLowCrash { get; set; }

        public double? LimLow { get; set; }

        public double? LimHigh { get; set; }

        public double? LimHighCrash { get; set; }
    }
}
