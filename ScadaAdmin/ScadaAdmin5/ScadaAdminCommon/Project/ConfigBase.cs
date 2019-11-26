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
 * Module   : ScadaAdminCommon
 * Summary  : The configuration database and the basis of a configuration
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2019
 */

using Scada.Data.Entities;
using Scada.Data.Tables;
using System;
using System.IO;

namespace Scada.Admin.Project
{
    /// <summary>
    /// The configuration database and the basis of a configuration.
    /// <para>База данных конфигурации и основа конфигурации.</para>
    /// </summary>
    public class ConfigBase
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ConfigBase()
        {
            CreateAllTables();
            AddRelations();
            BaseDir = "";
            Loaded = false;
        }


        /// <summary>
        /// Gets the command type table.
        /// </summary>
        public BaseTable<CmdType> CmdTypeTable { get; protected set; }

        /// <summary>
        /// Gets the command value table.
        /// </summary>
        public BaseTable<CmdVal> CmdValTable { get; protected set; }

        /// <summary>
        /// Gets the input channel type table.
        /// </summary>
        public BaseTable<CnlType> CnlTypeTable { get; protected set; }

        /// <summary>
        /// Gets the communication line table.
        /// </summary>
        public BaseTable<CommLine> CommLineTable { get; protected set; }

        /// <summary>
        /// Gets the output channel table.
        /// </summary>
        public BaseTable<CtrlCnl> CtrlCnlTable { get; protected set; }

        /// <summary>
        /// Gets the event type table.
        /// </summary>
        public BaseTable<EvType> EvTypeTable { get; protected set; }

        /// <summary>
        /// Gets the format table.
        /// </summary>
        public BaseTable<Format> FormatTable { get; protected set; }

        /// <summary>
        /// Gets the formula table.
        /// </summary>
        public BaseTable<Formula> FormulaTable { get; protected set; }

        /// <summary>
        /// Gets the input channel table.
        /// </summary>
        public BaseTable<InCnl> InCnlTable { get; protected set; }

        /// <summary>
        /// Gets the interface table.
        /// </summary>
        public BaseTable<Data.Entities.Interface> InterfaceTable { get; protected set; }

        /// <summary>
        /// Gets the device table.
        /// </summary>
        public BaseTable<KP> KPTable { get; protected set; }

        /// <summary>
        /// Gets the device type table.
        /// </summary>
        public BaseTable<KPType> KPTypeTable { get; protected set; }

        /// <summary>
        /// Gets the object (location) table.
        /// </summary>
        public BaseTable<Obj> ObjTable { get; protected set; }

        /// <summary>
        /// Gets the quantity table.
        /// </summary>
        public BaseTable<Param> ParamTable { get; protected set; }

        /// <summary>
        /// Gets the right table.
        /// </summary>
        public BaseTable<Right> RightTable { get; protected set; }

        /// <summary>
        /// Gets the role table.
        /// </summary>
        public BaseTable<Role> RoleTable { get; protected set; }

        /// <summary>
        /// Gets the role inheritance table.
        /// </summary>
        public BaseTable<RoleRef> RoleRefTable { get; protected set; }

        /// <summary>
        /// Gets the unit table.
        /// </summary>
        public BaseTable<Unit> UnitTable { get; protected set; }

        /// <summary>
        /// Gets the user table.
        /// </summary>
        public BaseTable<User> UserTable { get; protected set; }

        /// <summary>
        /// Gets all the tables of the configuration database.
        /// </summary>
        public IBaseTable[] AllTables { get; protected set; }


        /// <summary>
        /// Gets or sets the directory of the configuration database files.
        /// </summary>
        public string BaseDir { get; set; }

        /// <summary>
        /// Gets a value indicating whether the tables are loaded.
        /// </summary>
        public bool Loaded { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether at least one table was modified.
        /// </summary>
        public bool Modified
        {
            get
            {
                foreach (IBaseTable baseTable in AllTables)
                {
                    if (baseTable.Modified)
                        return true;
                }

                return false;
            }
        }


        /// <summary>
        /// Creates all tables of the configuration database.
        /// </summary>
        private void CreateAllTables()
        {
            AllTables = new IBaseTable[]
            {
                CmdTypeTable = new BaseTable<CmdType>("CmdType", "CmdTypeID", CommonPhrases.CmdTypeTable),
                CmdValTable = new BaseTable<CmdVal>("CmdVal", "CmdValID", CommonPhrases.CmdValTable),
                CnlTypeTable = new BaseTable<CnlType>("CnlType", "CnlTypeID", CommonPhrases.CnlTypeTable),
                CommLineTable = new BaseTable<CommLine>("CommLine", "CommLineNum", CommonPhrases.CommLineTable),
                CtrlCnlTable = new BaseTable<CtrlCnl>("CtrlCnl", "CtrlCnlNum", CommonPhrases.CtrlCnlTable),
                EvTypeTable = new BaseTable<EvType>("EvType", "CnlStatus", CommonPhrases.EvTypeTable),
                FormatTable = new BaseTable<Format>("Format", "FormatID", CommonPhrases.FormatTable),
                FormulaTable = new BaseTable<Formula>("Formula", "FormulaID", CommonPhrases.FormulaTable),
                InCnlTable = new BaseTable<InCnl>("InCnl", "CnlNum", CommonPhrases.InCnlTable),
                InterfaceTable = new BaseTable<Data.Entities.Interface>("Interface", "ItfID",
                    CommonPhrases.InterfaceTable),
                KPTable = new BaseTable<KP>("KP", "KPNum", CommonPhrases.KPTable),
                KPTypeTable = new BaseTable<KPType>("KPType", "KPTypeID", CommonPhrases.KPTypeTable),
                ObjTable = new BaseTable<Obj>("Obj", "ObjNum", CommonPhrases.ObjTable),
                ParamTable = new BaseTable<Param>("Param", "ParamID", CommonPhrases.ParamTable),
                RightTable = new BaseTable<Right>("Right", "RightID", CommonPhrases.RightTable),
                RoleTable = new BaseTable<Role>("Role", "RoleID", CommonPhrases.RoleTable),
                RoleRefTable = new BaseTable<RoleRef>("RoleRef", "RoleRefID", CommonPhrases.RoleRefTable),
                UnitTable = new BaseTable<Unit>("Unit", "UnitID", CommonPhrases.UnitTable),
                UserTable = new BaseTable<User>("User", "UserID", CommonPhrases.UserTable)
            };
        }

        /// <summary>
        /// Adds relations between the tables.
        /// </summary>
        private void AddRelations()
        {
            // relations of the Devices table
            AddRelation(KPTypeTable, KPTable, "KPTypeID");
            AddRelation(CommLineTable, KPTable, "CommLineNum");

            // relations of the Input channels table
            AddRelation(CnlTypeTable, InCnlTable, "CnlTypeID");
            AddRelation(ObjTable, InCnlTable, "ObjNum");
            AddRelation(KPTable, InCnlTable, "KPNum");
            AddRelation(ParamTable, InCnlTable, "ParamID");
            AddRelation(FormatTable, InCnlTable, "FormatID");
            AddRelation(UnitTable, InCnlTable, "UnitID");

            // relations of the Output channels table
            AddRelation(CmdTypeTable, CtrlCnlTable, "CmdTypeID");
            AddRelation(ObjTable, CtrlCnlTable, "ObjNum");
            AddRelation(KPTable, CtrlCnlTable, "KPNum");
            AddRelation(CmdValTable, CtrlCnlTable, "CmdValID");

            // relations of the Interface table
            AddRelation(ObjTable, InterfaceTable, "ObjNum");

            // relations of the Users table
            AddRelation(RoleTable, UserTable, "RoleID");

            // relations of the Rights table
            AddRelation(InterfaceTable, RightTable, "ItfID");
            AddRelation(RoleTable, RightTable, "RoleID");

            // relations of the Role inheritance table
            AddRelation(RoleTable, RoleRefTable, "ParentRoleID");
            AddRelation(RoleTable, RoleRefTable, "ChildRoleID");
        }

        /// <summary>
        /// Adds a relation to the configuration database.
        /// </summary>
        private void AddRelation(IBaseTable parentTable, IBaseTable childTable, string childColumn)
        {
            TableRelation relation = new TableRelation(parentTable, childTable, childColumn);
            childTable.AddIndex(childColumn);
            childTable.DependsOn.Add(relation);
            parentTable.Dependent.Add(relation);
        }


        /// <summary>
        /// Loads the configuration database if needed.
        /// </summary>
        public bool Load(out string errMsg)
        {
            try
            {
                if (!Loaded)
                {
                    foreach (IBaseTable baseTable in AllTables)
                    {
                        string fileName = Path.Combine(BaseDir, baseTable.FileName);

                        if (File.Exists(fileName))
                        {
                            try
                            {
                                baseTable.Load(fileName);
                            }
                            catch (Exception ex)
                            {
                                throw new ScadaException(string.Format(
                                    AdminPhrases.LoadBaseTableError, baseTable.Title), ex);
                            }
                        }
                    }

                    Loaded = true;
                }

                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = AdminPhrases.LoadConfigBaseError + ": " + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Saves all the modified tables of the configuration database.
        /// </summary>
        public bool Save(out string errMsg)
        {
            try
            {
                Directory.CreateDirectory(BaseDir);

                foreach (IBaseTable baseTable in AllTables)
                {
                    if (baseTable.Modified)
                    {
                        try
                        {
                            string fileName = Path.Combine(BaseDir, baseTable.FileName);
                            baseTable.Save(fileName);
                        }
                        catch (Exception ex)
                        {
                            throw new ScadaException(string.Format(
                                AdminPhrases.SaveBaseTableError, baseTable.Title), ex);
                        }
                    }
                }

                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = AdminPhrases.SaveConfigBaseError + ": " + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Saves the specified table of the configuration database.
        /// </summary>
        public bool SaveTable(IBaseTable baseTable, out string errMsg)
        {
            try
            {
                Directory.CreateDirectory(BaseDir);
                string fileName = Path.Combine(BaseDir, baseTable.FileName);
                baseTable.Save(fileName);
                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = string.Format(AdminPhrases.SaveBaseTableError, baseTable.Title) + ":" + ex.Message;
                return false;
            }
        }
    }
}
