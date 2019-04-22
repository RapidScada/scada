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
 * Module   : Administrator
 * Summary  : Generates channel map
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
 * Modified : 2019
 */

using Scada.Admin.Project;
using Scada.Data.Entities;
using Scada.Data.Tables;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Scada.Admin.App.Code
{
    /// <summary>
    /// Generates channel map.
    /// <para>Генерирует карту каналов.</para>
    /// </summary>
    internal class CnlMap
    {
        /// <summary>
        /// The file name of newly created maps.
        /// </summary>
        private const string MapFileName = "ScadaAdmin_CnlMap.txt";

        private readonly ConfigBase configBase; // the configuration database
        private readonly AppData appData;       // the common data of the application


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CnlMap(ConfigBase configBase, AppData appData)
        {
            this.configBase = configBase ?? throw new ArgumentNullException("configBase");
            this.appData = appData ?? throw new ArgumentNullException("appData");

            IncludeInCnls = true;
            IncludeOutCnls = true;
            GroupByDevices = true;
        }


        /// <summary>
        /// Gets or sets a value indicating whether to include input channels in a map.
        /// </summary>
        public bool IncludeInCnls { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include output channels in a map.
        /// </summary>
        public bool IncludeOutCnls { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to group channels by devices. Otherwise group by objects.
        /// </summary>
        public bool GroupByDevices { get; set; }


        /// <summary>
        /// Writes input channels having the specified index key.
        /// </summary>
        private void WriteCnls(StreamWriter writer, TableIndex index, int indexKey)
        {
            writer.WriteLine(index.ItemGroups.TryGetValue(indexKey, out SortedDictionary<int, object> group) ?
                RangeUtils.RangeToStr(group.Keys) :
                AppPhrases.NoChannels);
        }

        /// <summary>
        /// Generates a channel map.
        /// </summary>
        public void Generate()
        {
            try
            {
                string mapFileName = Path.Combine(appData.AppDirs.LogDir, MapFileName);
                string indexedColumn = GroupByDevices ? "KPNum" : "ObjNum";

                using (StreamWriter writer = new StreamWriter(mapFileName, false, Encoding.UTF8))
                {
                    string title = GroupByDevices ? AppPhrases.CnlMapByDevice : AppPhrases.CnlMapByObject;
                    writer.WriteLine(title);
                    writer.WriteLine(new string('-', title.Length));

                    if (configBase.InCnlTable.TryGetIndex(indexedColumn, out TableIndex index1) &&
                        configBase.CtrlCnlTable.TryGetIndex(indexedColumn, out TableIndex index2))
                    {
                        if (GroupByDevices)
                        {
                            foreach (KP kp in configBase.KPTable.EnumerateItems())
                            {
                                writer.WriteLine(string.Format(AppPhrases.DeviceCaption, kp.KPNum, kp.Name));

                                if (IncludeInCnls)
                                {
                                    writer.Write(AppPhrases.InCnlsCaption);
                                    WriteCnls(writer, index1, kp.KPNum);
                                }

                                if (IncludeOutCnls)
                                {
                                    writer.Write(AppPhrases.OutCnlsCaption);
                                    WriteCnls(writer, index2, kp.KPNum);
                                }

                                writer.WriteLine();
                            }

                            writer.WriteLine(AppPhrases.EmptyDevice);
                        }
                        else
                        {
                            foreach (Obj obj in configBase.ObjTable.EnumerateItems())
                            {
                                writer.WriteLine(string.Format(AppPhrases.ObjectCaption, obj.ObjNum, obj.Name));

                                if (IncludeInCnls)
                                {
                                    writer.Write(AppPhrases.InCnlsCaption);
                                    WriteCnls(writer, index1, obj.ObjNum);
                                }

                                if (IncludeOutCnls)
                                {
                                    writer.Write(AppPhrases.OutCnlsCaption);
                                    WriteCnls(writer, index2, obj.ObjNum);
                                }

                                writer.WriteLine();
                            }

                            writer.WriteLine(AppPhrases.EmptyObject);
                        }

                        // channels with unspecified device or object
                        if (IncludeInCnls)
                        {
                            writer.Write(AppPhrases.InCnlsCaption);
                            WriteCnls(writer, index1, 0);
                        }

                        if (IncludeOutCnls)
                        {
                            writer.Write(AppPhrases.OutCnlsCaption);
                            WriteCnls(writer, index2, 0);
                        }
                    }
                    else
                    {
                        throw new ScadaException(AdminPhrases.IndexNotFound);
                    }
                }

                AppUtils.OpenTextFile(mapFileName);
            }
            catch (Exception ex)
            {
                appData.ProcError(ex, AppPhrases.GenerateCnlMapError);
            }
        }
    }
}
