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
 * Module   : Table Editor
 * Summary  : Represents the configuration database tables
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
 * Modified : 2019
 */

using Scada.Data.Entities;
using Scada.Data.Tables;
using System.IO;

namespace Scada.Table.Editor.Code
{
    /// <summary>
    /// Represents the configuration database tables.
    /// <para>Представляет таблицы базы конфигурации.</para>
    /// </summary>
    public class BaseTables
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public BaseTables()
        {
            InCnlTable = null;
            CtrlCnlTable = null;
            ObjTable = null;
            KPTable = null;
        }


        /// <summary>
        /// Gets the input channel table.
        /// </summary>
        public BaseTable<InCnl> InCnlTable { get; private set; }

        /// <summary>
        /// Gets the output channel table.
        /// </summary>
        public BaseTable<CtrlCnl> CtrlCnlTable { get; private set; }

        /// <summary>
        /// Gets the object table.
        /// </summary>
        public BaseTable<Obj> ObjTable { get; private set; }

        /// <summary>
        /// Gets the device table.
        /// </summary>
        public BaseTable<KP> KPTable { get; private set; }


        /// <summary>
        /// Initializes the configuration database tables.
        /// </summary>
        private void Init()
        {
            InCnlTable = new BaseTable<InCnl>("InCnl", "CnlNum", CommonPhrases.InCnlTable);
            CtrlCnlTable = new BaseTable<CtrlCnl>("CtrlCnl", "CtrlCnlNum", CommonPhrases.CtrlCnlTable);
            ObjTable = new BaseTable<Obj>("Obj", "ObjNum", CommonPhrases.ObjTable);
            KPTable = new BaseTable<KP>("KP", "KPNum", CommonPhrases.KPTable);

            InCnlTable.AddIndex("KPNum");
            CtrlCnlTable.AddIndex("KPNum");
        }

        /// <summary>
        /// Loads the configuration database table.
        /// </summary>
        private void LoadBaseTable(IBaseTable baseTable, string baseDir)
        {
            string fileName = Path.Combine(baseDir, baseTable.FileName);

            if (File.Exists(fileName))
                baseTable.Load(fileName);
        }

        /// <summary>
        /// Loads the configuration database tables.
        /// </summary>
        public void Load(string baseDir)
        {
            Init();
            LoadBaseTable(InCnlTable, baseDir);
            LoadBaseTable(CtrlCnlTable, baseDir);
            LoadBaseTable(ObjTable, baseDir);
            LoadBaseTable(KPTable, baseDir);
        }
    }
}
