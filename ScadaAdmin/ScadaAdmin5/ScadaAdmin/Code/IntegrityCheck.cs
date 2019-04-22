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
 * Summary  : Provides integrity check of the configuration database
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
 * Modified : 2019
 */

using Scada.Admin.Project;
using Scada.Data.Tables;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Scada.Admin.App.Code
{
    /// <summary>
    /// Provides integrity check of the configuration database.
    /// <para>Обеспечивает проверку целостности базы конфигурации.</para>
    /// </summary>
    internal class IntegrityCheck
    {
        /// <summary>
        /// The name of the result file.
        /// </summary>
        private const string ResultFileName = "ScadaAdmin_IntegrityCheck.txt";

        private readonly ConfigBase configBase; // the configuration database
        private readonly AppData appData;       // the common data of the application

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public IntegrityCheck(ConfigBase configBase, AppData appData)
        {
            this.configBase = configBase ?? throw new ArgumentNullException("configBase");
            this.appData = appData ?? throw new ArgumentNullException("appData");
        }

        /// <summary>
        /// Executes integrity check.
        /// </summary>
        public void Execute()
        {
            try
            {
                string resultFileName = Path.Combine(appData.AppDirs.LogDir, ResultFileName);

                using (StreamWriter writer = new StreamWriter(resultFileName, false, Encoding.UTF8))
                {
                    writer.WriteLine(AppPhrases.IntegrityCheckTitle);
                    writer.WriteLine(new string('-', AppPhrases.IntegrityCheckTitle.Length));
                    bool hasErrors = false;

                    foreach (IBaseTable baseTable in configBase.AllTables)
                    {
                        writer.Write(baseTable.Title);
                        writer.Write("...");

                        SortedSet<int> requiredKeys = new SortedSet<int>();
                        List<int> lostKeys = new List<int>();

                        foreach (TableRelation relation in baseTable.Dependent)
                        {
                            if (relation.ChildTable.TryGetIndex(relation.ChildColumn, out TableIndex index))
                            {
                                foreach (int indexKey in index.ItemGroups.Keys)
                                {
                                    // if index.AllowNull then 0 means NULL, otherwise 0 is 0
                                    if (indexKey != 0 || !index.AllowNull)
                                        requiredKeys.Add(indexKey);
                                }
                            }
                            else
                            {
                                throw new ScadaException(AdminPhrases.IndexNotFound);
                            }
                        }

                        foreach (int key in requiredKeys)
                        {
                            if (!baseTable.PkExists(key))
                                lostKeys.Add(key);
                        }

                        if (lostKeys.Count > 0)
                        {
                            hasErrors = true;
                            writer.WriteLine(AppPhrases.TableHasErrors);
                            writer.WriteLine(AppPhrases.LostPrimaryKeys + string.Join(", ", lostKeys));
                            writer.WriteLine();
                        }
                        else
                        {
                            writer.WriteLine(AppPhrases.TableCorrect);
                        }
                    }

                    writer.WriteLine(hasErrors ? AppPhrases.BaseHasErrors : AppPhrases.BaseCorrect);
                }

                AppUtils.OpenTextFile(resultFileName);
            }
            catch (Exception ex)
            {
                appData.ProcError(ex, AppPhrases.IntegrityCheckError);
            }
        }
    }
}
