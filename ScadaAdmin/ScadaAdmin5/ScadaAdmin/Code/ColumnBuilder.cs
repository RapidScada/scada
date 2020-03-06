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
 * Summary  : Creates columns for a DataGridView control
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2019
 */

using Scada.Admin.Project;
using Scada.Data.Entities;
using Scada.Data.Tables;
using System;
using System.Data;
using System.Windows.Forms;

namespace Scada.Admin.App.Code
{
    /// <summary>
    /// Creates columns for a DataGridView control.
    /// <para>Создает столбцы для элемента управления DataGridView.</para>
    /// </summary>
    internal class ColumnBuilder
    {
        private readonly ConfigBase configBase; // the reference to the configuration database


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <remarks>The configuration database is required for creating combo box columns.</remarks>
        public ColumnBuilder(ConfigBase configBase)
        {
            this.configBase = configBase ?? throw new ArgumentNullException("configBase");
        }


        /// <summary>
        /// Creates a new column that hosts text cells.
        /// </summary>
        private DataGridViewColumn NewTextBoxColumn(string dataPropertyName, ColumnOptions options = null)
        {
            DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn
            {
                Name = dataPropertyName,
                HeaderText = dataPropertyName,
                DataPropertyName = dataPropertyName,
                Tag = options
            };

            if (options != null && options.MaxLength > 0)
                column.MaxInputLength = options.MaxLength;

            return column;
        }

        /// <summary>
        /// Creates a new column that hosts cells with checkboxes.
        /// </summary>
        private DataGridViewColumn NewCheckBoxColumn(string dataPropertyName, ColumnOptions options = null)
        {
            return new DataGridViewCheckBoxColumn
            {
                Name = dataPropertyName,
                HeaderText = dataPropertyName,
                DataPropertyName = dataPropertyName,
                Tag = options,
                SortMode = DataGridViewColumnSortMode.Automatic
            };
        }

        /// <summary>
        /// Creates a new column that hosts cells with buttons.
        /// </summary>
        private DataGridViewColumn NewButtonColumn(string dataPropertyName, ColumnOptions options = null)
        {
            return new DataGridViewButtonColumn
            {
                Name = dataPropertyName + (options == null ? ColumnKind.Button : options.Kind),
                HeaderText = "",
                DataPropertyName = dataPropertyName,
                Tag = options,
                Text = dataPropertyName,
                UseColumnTextForButtonValue = true
            };
        }

        /// <summary>
        /// Creates a new column that hosts cells which values are selected from a combo box.
        /// </summary>
        private DataGridViewColumn NewComboBoxColumn(string dataPropertyName, string valueMember, 
            string displayMember, object dataSource, bool addEmptyRow = false, bool prependID = false)
        {
            if (ScadaUtils.IsRunningOnMono)
            {
                return NewTextBoxColumn(dataPropertyName); /*because of the bugs in Mono*/
            }
            else
            {
                if (dataSource is IBaseTable baseTable)
                    dataSource = CreateComboBoxSource(baseTable, valueMember, ref displayMember, addEmptyRow, prependID);

                return new DataGridViewComboBoxColumn
                {
                    Name = dataPropertyName,
                    HeaderText = dataPropertyName,
                    DataPropertyName = dataPropertyName,
                    ValueMember = valueMember,
                    DisplayMember = displayMember,
                    DataSource = dataSource,
                    SortMode = DataGridViewColumnSortMode.Automatic,
                    DisplayStyleForCurrentCellOnly = true
                };
            }
        }

        /// <summary>
        /// Creates a new column that hosts cells which values are selected from a combo box.
        /// </summary>
        private DataGridViewColumn NewComboBoxColumn(string dataPropertyName, 
            string displayMember, IBaseTable dataSource, bool addEmptyRow = false, bool prependID = false)
        {
            return NewComboBoxColumn(dataPropertyName, dataPropertyName, 
                displayMember, dataSource, addEmptyRow, prependID);
        }

        /// <summary>
        /// Creates a data table for using as a data source of a combo box.
        /// </summary>
        private DataTable CreateComboBoxSource(
            IBaseTable baseTable, string valueMember, ref string displayMember, bool addEmptyRow, bool prependID)
        {
            DataTable dataTable = baseTable.ToDataTable(true);

            if (prependID)
            {
                // display ID and name
                string columnName = valueMember + "_" + displayMember;
                dataTable.Columns.Add(columnName, typeof(string), 
                    string.Format("'[' + {0} + '] ' + {1}", valueMember, displayMember));
                displayMember = columnName;
                dataTable.DefaultView.Sort = valueMember;
            }
            else
            {
                dataTable.DefaultView.Sort = displayMember;
            }

            if (addEmptyRow)
            {
                DataRow emptyRow = dataTable.NewRow();
                emptyRow[valueMember] = DBNull.Value;
                emptyRow[displayMember] = " ";
                dataTable.Rows.Add(emptyRow);
            }

            return dataTable;
        }

        /// <summary>
        /// Translates the column headers.
        /// </summary>
        private DataGridViewColumn[] TranslateHeaders(string tableName, DataGridViewColumn[] columns)
        {
            if (Localization.Dictionaries.TryGetValue("Scada.Admin.App.Code.ColumnBuilder." + tableName, 
                out Localization.Dict dict))
            {
                foreach (DataGridViewColumn col in columns)
                {
                    if (dict.Phrases.TryGetValue(col.Name, out string header))
                    {
                        if (col is DataGridViewButtonColumn buttonColumn)
                            buttonColumn.Text = header;
                        col.HeaderText = header;
                    }
                }
            }

            return columns;
        }


        /// <summary>
        /// Creates columns for the command type table.
        /// </summary>
        private DataGridViewColumn[] CreateCmdTypeTableColumns()
        {
            return TranslateHeaders("CmdTypeTable", new DataGridViewColumn[]
            {
                NewTextBoxColumn("CmdTypeID", new ColumnOptions(0, ushort.MaxValue)),
                NewTextBoxColumn("Name", new ColumnOptions(ColumnLength.Name)),
                NewTextBoxColumn("Descr", new ColumnOptions(ColumnLength.Description))
            });
        }

        /// <summary>
        /// Creates columns for the command value table.
        /// </summary>
        private DataGridViewColumn[] CreateCmdValTableColumns()
        {
            return TranslateHeaders("CmdValTable", new DataGridViewColumn[]
            {
                NewTextBoxColumn("CmdValID", new ColumnOptions(1, ushort.MaxValue)),
                NewTextBoxColumn("Name", new ColumnOptions(ColumnLength.Name)),
                NewTextBoxColumn("Val", new ColumnOptions(ColumnLength.Enumeration)),
                NewTextBoxColumn("Descr", new ColumnOptions(ColumnLength.Description))
            });
        }

        /// <summary>
        /// Creates columns for the input channel type table.
        /// </summary>
        private DataGridViewColumn[] CreateCnlTypeTableColumns()
        {
            return TranslateHeaders("CnlTypeTable", new DataGridViewColumn[]
            {
                NewTextBoxColumn("CnlTypeID", new ColumnOptions(1, ushort.MaxValue)),
                NewTextBoxColumn("Name", new ColumnOptions(ColumnLength.Name)),
                NewTextBoxColumn("ShtName", new ColumnOptions(ColumnLength.Name)),
                NewTextBoxColumn("Descr", new ColumnOptions(ColumnLength.Description))
            });
        }

        /// <summary>
        /// Creates columns for the communication line table.
        /// </summary>
        private DataGridViewColumn[] CreateCommLineTableColumns()
        {
            return TranslateHeaders("CommLineTable", new DataGridViewColumn[]
            {
                NewTextBoxColumn("CommLineNum", new ColumnOptions(1, ushort.MaxValue)),
                NewTextBoxColumn("Name", new ColumnOptions(ColumnLength.Name)),
                NewTextBoxColumn("Descr", new ColumnOptions(ColumnLength.Description))
            });
        }

        /// <summary>
        /// Creates columns for the output channel table.
        /// </summary>
        private DataGridViewColumn[] CreateCtrlCnlTableColumns()
        {
            return TranslateHeaders("CtrlCnlTable", new DataGridViewColumn[]
            {
                NewTextBoxColumn("CtrlCnlNum", new ColumnOptions(1, AdminUtils.MaxCnlNum)),
                NewCheckBoxColumn("Active", new ColumnOptions() { DefaultValue = true }),
                NewTextBoxColumn("Name", new ColumnOptions(ColumnLength.Name)),
                NewComboBoxColumn("CmdTypeID", "Name", configBase.CmdTypeTable),
                NewComboBoxColumn("ObjNum", "Name", configBase.ObjTable, true),
                NewComboBoxColumn("KPNum", "Name", configBase.KPTable, true),
                NewTextBoxColumn("CmdNum"),
                NewComboBoxColumn("CmdValID", "Name", configBase.CmdValTable, true),
                NewCheckBoxColumn("FormulaUsed"),
                NewTextBoxColumn("Formula", new ColumnOptions(ColumnLength.Default)),
                NewCheckBoxColumn("EvEnabled")
            });
        }

        /// <summary>
        /// Creates columns for the event type table.
        /// </summary>
        private DataGridViewColumn[] CreateEvTypeTableColumns()
        {
            return TranslateHeaders("EvTypeTable", new DataGridViewColumn[]
            {
                NewTextBoxColumn("CnlStatus", new ColumnOptions(0, ushort.MaxValue)),
                NewTextBoxColumn("Name", new ColumnOptions(ColumnLength.Name)),
                NewTextBoxColumn("Color", new ColumnOptions(ColumnKind.Color, ColumnLength.Default)),
                NewButtonColumn("Color"),
                NewTextBoxColumn("Descr", new ColumnOptions(ColumnLength.Description))
            });
        }

        /// <summary>
        /// Creates columns for the format table.
        /// </summary>
        private DataGridViewColumn[] CreateFormatTableColumns()
        {
            return TranslateHeaders("FormatTable", new DataGridViewColumn[]
            {
                NewTextBoxColumn("FormatID", new ColumnOptions(0, ushort.MaxValue)),
                NewTextBoxColumn("Name", new ColumnOptions(ColumnLength.Name)),
                NewCheckBoxColumn("ShowNumber"),
                NewTextBoxColumn("DecDigits")
            });
        }

        /// <summary>
        /// Creates columns for the formula table.
        /// </summary>
        private DataGridViewColumn[] CreateFormulaTableColumns()
        {
            return TranslateHeaders("FormulaTable", new DataGridViewColumn[]
            {
                NewTextBoxColumn("FormulaID", new ColumnOptions(0, ushort.MaxValue)),
                NewTextBoxColumn("Name", new ColumnOptions(ColumnLength.Name)),
                NewTextBoxColumn("Source", new ColumnOptions(ColumnKind.SourceCode, ColumnLength.SourceCode)),
                NewButtonColumn("Source"),
                NewTextBoxColumn("Descr", new ColumnOptions(ColumnLength.Description))
            });
        }

        /// <summary>
        /// Creates columns for the input channel table.
        /// </summary>
        private DataGridViewColumn[] CreateInCnlTableColumns()
        {
            return TranslateHeaders("InCnlTable", new DataGridViewColumn[]
            {
                NewTextBoxColumn("CnlNum", new ColumnOptions(1, AdminUtils.MaxCnlNum)),
                NewCheckBoxColumn("Active", new ColumnOptions() { DefaultValue = true }),
                NewTextBoxColumn("Name", new ColumnOptions(ColumnLength.Name)),
                NewComboBoxColumn("CnlTypeID", "Name", configBase.CnlTypeTable),
                NewComboBoxColumn("ObjNum","Name", configBase.ObjTable, true),
                NewComboBoxColumn("KPNum", "Name", configBase.KPTable, true),
                NewTextBoxColumn("Signal"),
                NewCheckBoxColumn("FormulaUsed"),
                NewTextBoxColumn("Formula", new ColumnOptions(ColumnLength.Default)),
                NewCheckBoxColumn("Averaging"),
                NewComboBoxColumn("ParamID", "Name", configBase.ParamTable, true),
                NewComboBoxColumn("FormatID", "Name", configBase.FormatTable, true),
                NewComboBoxColumn("UnitID", "Name", configBase.UnitTable, true),
                NewTextBoxColumn("CtrlCnlNum"),
                NewCheckBoxColumn("EvEnabled"),
                NewCheckBoxColumn("EvSound"),
                NewCheckBoxColumn("EvOnChange"),
                NewCheckBoxColumn("EvOnUndef"),
                NewTextBoxColumn("LimLowCrash"),
                NewTextBoxColumn("LimLow"),
                NewTextBoxColumn("LimHigh"),
                NewTextBoxColumn("LimHighCrash")
            });
        }

        /// <summary>
        /// Creates columns for the interface table.
        /// </summary>
        private DataGridViewColumn[] CreateInterfaceTableColumns()
        {
            return TranslateHeaders("InterfaceTable", new DataGridViewColumn[]
            {
                NewTextBoxColumn("ItfID", new ColumnOptions(1, ushort.MaxValue)),
                NewTextBoxColumn("Name", new ColumnOptions(ColumnKind.Path, ColumnLength.Long)),
                NewButtonColumn("Name", new ColumnOptions(ColumnKind.SelectFileButton)),
                NewButtonColumn("Name", new ColumnOptions(ColumnKind.SelectFolderButton)),
                NewTextBoxColumn("Args", new ColumnOptions(ColumnLength.Default)),
                NewTextBoxColumn("TypeCode", new ColumnOptions(ColumnLength.Default)),
                NewTextBoxColumn("Descr", new ColumnOptions(ColumnLength.Long)), // Title
                NewCheckBoxColumn("Hidden"),
                NewComboBoxColumn("ObjNum","Name", configBase.ObjTable, true)
            });
        }

        /// <summary>
        /// Creates columns for the devices table.
        /// </summary>
        private DataGridViewColumn[] CreateKPTableColumns()
        {
            return TranslateHeaders("KPTable", new DataGridViewColumn[]
            {
                NewTextBoxColumn("KPNum", new ColumnOptions(1, ushort.MaxValue)),
                NewTextBoxColumn("Name", new ColumnOptions(ColumnLength.Name)),
                NewComboBoxColumn("KPTypeID", "Name", configBase.KPTypeTable),
                NewTextBoxColumn("Address"),
                NewTextBoxColumn("CallNum", new ColumnOptions(ColumnLength.Default)),
                NewComboBoxColumn("CommLineNum", "Name", configBase.CommLineTable, true),
                NewTextBoxColumn("Descr", new ColumnOptions(ColumnLength.Description))
            });
        }

        /// <summary>
        /// Creates columns for the device type table.
        /// </summary>
        private DataGridViewColumn[] CreateKPTypeTableColumns()
        {
            return TranslateHeaders("KPTypeTable", new DataGridViewColumn[]
            {
                NewTextBoxColumn("KPTypeID", new ColumnOptions(1, ushort.MaxValue)),
                NewTextBoxColumn("Name", new ColumnOptions(ColumnLength.Name)),
                NewTextBoxColumn("DllFileName", new ColumnOptions(ColumnLength.Default)),
                NewTextBoxColumn("Descr", new ColumnOptions(ColumnLength.Description))
            });
        }

        /// <summary>
        /// Creates columns for the object table.
        /// </summary>
        private DataGridViewColumn[] CreateObjTableColumns()
        {
            return TranslateHeaders("ObjTable", new DataGridViewColumn[]
            {
                NewTextBoxColumn("ObjNum", new ColumnOptions(1, ushort.MaxValue)),
                NewTextBoxColumn("Name", new ColumnOptions(ColumnLength.Name)),
                NewTextBoxColumn("Descr", new ColumnOptions(ColumnLength.Description))
            });
        }

        /// <summary>
        /// Creates columns for the quantity table.
        /// </summary>
        private DataGridViewColumn[] CreateParamTableColumns()
        {
            return TranslateHeaders("ParamTable", new DataGridViewColumn[]
            {
                NewTextBoxColumn("ParamID", new ColumnOptions(1, ushort.MaxValue)),
                NewTextBoxColumn("Name", new ColumnOptions(ColumnLength.Name)),
                NewTextBoxColumn("Sign", new ColumnOptions(ColumnLength.Default)),
                NewTextBoxColumn("IconFileName", new ColumnOptions(ColumnLength.Default))
            });
        }

        /// <summary>
        /// Creates columns for the right table.
        /// </summary>
        private DataGridViewColumn[] CreateRightTableColumns()
        {
            return TranslateHeaders("RightTable", new DataGridViewColumn[]
            {
                NewTextBoxColumn("RightID", new ColumnOptions(1, ushort.MaxValue)),
                NewComboBoxColumn("ItfID", "Descr", configBase.InterfaceTable, false, true),
                NewComboBoxColumn("RoleID", "Name", configBase.RoleTable),
                NewCheckBoxColumn("ViewRight"),
                NewCheckBoxColumn("CtrlRight")
            });
        }

        /// <summary>
        /// Creates columns for the role table.
        /// </summary>
        private DataGridViewColumn[] CreateRoleTableColumns()
        {
            return TranslateHeaders("RoleTable", new DataGridViewColumn[]
            {
                NewTextBoxColumn("RoleID", new ColumnOptions(0, ushort.MaxValue)),
                NewTextBoxColumn("Name", new ColumnOptions(ColumnLength.Name)),
                NewTextBoxColumn("Descr", new ColumnOptions(ColumnLength.Description))
            });
        }

        /// <summary>
        /// Creates columns for the role inheritance table.
        /// </summary>
        private DataGridViewColumn[] CreateRoleRefTableColumns()
        {
            return TranslateHeaders("RoleRefTable", new DataGridViewColumn[]
            {
                NewTextBoxColumn("RoleRefID", new ColumnOptions(0, ushort.MaxValue)),
                NewComboBoxColumn("ParentRoleID", "RoleID", "Name", configBase.RoleTable),
                NewComboBoxColumn("ChildRoleID", "RoleID", "Name", configBase.RoleTable)
            });
        }

        /// <summary>
        /// Creates columns for the unit table.
        /// </summary>
        private DataGridViewColumn[] CreateUnitTableColumns()
        {
            return TranslateHeaders("UnitTable", new DataGridViewColumn[]
            {
                NewTextBoxColumn("UnitID", new ColumnOptions(1, ushort.MaxValue)),
                NewTextBoxColumn("Name", new ColumnOptions(ColumnLength.Name)),
                NewTextBoxColumn("Sign", new ColumnOptions(ColumnLength.Enumeration)),
                NewTextBoxColumn("Descr", new ColumnOptions(ColumnLength.Description))
            });
        }

        /// <summary>
        /// Creates columns for the user table.
        /// </summary>
        private DataGridViewColumn[] CreateUserTableColumns()
        {
            return TranslateHeaders("UserTable", new DataGridViewColumn[]
            {
                NewTextBoxColumn("UserID", new ColumnOptions(1, ushort.MaxValue)),
                NewTextBoxColumn("Name", new ColumnOptions(ColumnLength.Name)),
                NewTextBoxColumn("Password", new ColumnOptions(ColumnKind.Password, ColumnLength.Password)),
                NewComboBoxColumn("RoleID", "Name", configBase.RoleTable),
                NewTextBoxColumn("Descr", new ColumnOptions(ColumnLength.Description))
            });
        }


        /// <summary>
        /// Creates columns for the specified table
        /// </summary>
        public DataGridViewColumn[] CreateColumns(Type itemType)
        {
            if (itemType == typeof(CmdType))
                return CreateCmdTypeTableColumns();
            else if (itemType == typeof(CmdVal))
                return CreateCmdValTableColumns();
            else if (itemType == typeof(CnlType))
                return CreateCnlTypeTableColumns();
            else if (itemType == typeof(CommLine))
                return CreateCommLineTableColumns();
            else if (itemType == typeof(CtrlCnl))
                return CreateCtrlCnlTableColumns();
            else if (itemType == typeof(EvType))
                return CreateEvTypeTableColumns();
            else if (itemType == typeof(Format))
                return CreateFormatTableColumns();
            else if (itemType == typeof(Formula))
                return CreateFormulaTableColumns();
            else if (itemType == typeof(InCnl))
                return CreateInCnlTableColumns();
            else if (itemType == typeof(Data.Entities.Interface))
                return CreateInterfaceTableColumns();
            else if (itemType == typeof(KP))
                return CreateKPTableColumns();
            else if (itemType == typeof(KPType))
                return CreateKPTypeTableColumns();
            else if (itemType == typeof(Obj))
                return CreateObjTableColumns();
            else if (itemType == typeof(Param))
                return CreateParamTableColumns();
            else if (itemType == typeof(Right))
                return CreateRightTableColumns();
            else if (itemType == typeof(Role))
                return CreateRoleTableColumns();
            else if (itemType == typeof(RoleRef))
                return CreateRoleRefTableColumns();
            else if (itemType == typeof(Unit))
                return CreateUnitTableColumns();
            else if (itemType == typeof(User))
                return CreateUserTableColumns();
            else
                return new DataGridViewColumn[0];
        }
    }
}
