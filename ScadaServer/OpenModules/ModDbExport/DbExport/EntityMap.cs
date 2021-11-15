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
 * Summary  : Represents a map of channels and devices
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

using Scada.Data.Entities;
using Scada.Data.Tables;
using System.Collections.Generic;

namespace Scada.Server.Modules.DbExport
{
    /// <summary>
    /// Represents a map of channels and devices.
    /// <para>Представляет карту каналов и устройств.</para>
    /// </summary>
    internal class EntityMap
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public EntityMap()
        {
            DeviceByCnlNum = new Dictionary<int, int>();
            CnlNumGroups = new List<int[]>();
        }


        /// <summary>
        /// Gets the device numbers accessed by channel number.
        /// </summary>
        public Dictionary<int, int> DeviceByCnlNum { get; }

        /// <summary>
        /// Gets the groups of input channel numbers split by device.
        /// </summary>
        public List<int[]> CnlNumGroups { get; }


        /// <summary>
        /// Initializes the entity map.
        /// </summary>
        public void Init(BaseTable<InCnl> inCnlTable)
        {
            SortedDictionary<int, List<int>> cnlsByDevice = new SortedDictionary<int, List<int>>();

            foreach (InCnl inCnl in inCnlTable.EnumerateItems())
            {
                int deviceNum = inCnl.KPNum ?? 0;

                if (deviceNum > 0)
                    DeviceByCnlNum[inCnl.CnlNum] = deviceNum;

                if (!cnlsByDevice.TryGetValue(deviceNum, out List<int> cnlNums))
                {
                    cnlNums = new List<int>();
                    cnlsByDevice.Add(deviceNum, cnlNums);
                }

                cnlNums.Add(inCnl.CnlNum);
            }

            foreach (List<int> cnlNums in cnlsByDevice.Values)
            {
                CnlNumGroups.Add(cnlNums.ToArray());
            }
        }
    }
}
