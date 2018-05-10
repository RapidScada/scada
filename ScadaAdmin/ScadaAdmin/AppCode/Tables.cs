/*
 * Copyright 2016 Mikhail Shiryaev
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
 * Module   : SCADA-Administrator
 * Summary  : Data access to the configuration tables
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2010
 * Modified : 2016
 */

using Scada;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlServerCe;
using System.Text;
using System.Windows.Forms;
using Utils;

namespace ScadaAdmin
{
    /// <summary>
    /// Data access to the configuration tables
    /// <para>Доступ к данным таблиц конфигурации</para>
    /// </summary>
    internal static class Tables
    {
        /// <summary>
        /// Информация о таблице
        /// </summary>
        public class TableInfo
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            public TableInfo(string name, string header, GetTableDelegate getTable, string idColName, 
                bool hasIntID = true)
            {
                Name = name;
                Header = header;
                GetTable = getTable;
                IDColName = idColName;
                HasIntID = !string.IsNullOrEmpty(idColName) && hasIntID;
            }

            /// <summary>
            /// Получить имя таблицы
            /// </summary>
            public string Name { get; private set; }
            /// <summary>
            /// Получить заголовок (пользовательское наименование) таблицы
            /// </summary>
            public string Header { get; private set; }
            /// <summary>
            /// Получить имя файла таблицы в формате DAT
            /// </summary>
            public string FileName
            {
                get
                {
                    return Name.ToLowerInvariant() + ".dat";
                }
            }
            /// <summary>
            /// Получить метод получения таблицы
            /// </summary>
            public GetTableDelegate GetTable { get; private set; }
            /// <summary>
            /// Получить наименование столбца идентификатора, если он числовой и определяется одним столбцом
            /// </summary>
            public string IDColName { get; private set; }
            /// <summary>
            /// Таблица имеет целочисленный идентификатор
            /// </summary>
            public bool HasIntID { get; private set; }

            /// <summary>
            /// Получить строковое представление объекта
            /// </summary>
            public override string ToString()
            {
                return Header;
            }
        }

        /// <summary>
        /// Делегат получения таблицы
        /// </summary>
        public delegate DataTable GetTableDelegate();


        /// <summary>
        /// Запрос входных каналов
        /// </summary>
        private const string InCnlSql = "select CnlNum, Active, Name, CnlTypeID, ObjNum, KPNum, Signal, " +
            "FormulaUsed, Formula, Averaging, ParamID, FormatID, UnitID, CtrlCnlNum, EvEnabled, EvSound, " +
            "EvOnChange, EvOnUndef, LimLowCrash, LimLow, LimHigh, LimHighCrash, ModifiedDT from InCnl ";
        
        /// <summary>
        /// Запрос каналов управления
        /// </summary>
        private const string CtrlCnlSql = "select CtrlCnlNum, Active, Name, CmdTypeID, ObjNum, KPNum, " +
            "CmdNum, CmdValID, FormulaUsed, Formula, EvEnabled, ModifiedDT from CtrlCnl ";


        /// <summary>
        /// Статический конструктор
        /// </summary>
        static Tables()
        {
            TableInfoList = new List<TableInfo>();
            TableInfoList.Add(new TableInfo("Obj", CommonPhrases.ObjTable, GetObjTable, "ObjNum"));
            TableInfoList.Add(new TableInfo("CommLine", CommonPhrases.CommLineTable, GetCommLineTable, "CommLineNum"));
            TableInfoList.Add(new TableInfo("KP", CommonPhrases.KPTable, GetKPTable, "KPNum"));
            TableInfoList.Add(new TableInfo("InCnl", CommonPhrases.InCnlTable, GetInCnlTable, "CnlNum"));
            TableInfoList.Add(new TableInfo("CtrlCnl", CommonPhrases.CtrlCnlTable, GetCtrlCnlTable, "CtrlCnlNum"));
            TableInfoList.Add(new TableInfo("Role", CommonPhrases.RoleTable, GetRoleTable, "RoleID"));
            TableInfoList.Add(new TableInfo("User", CommonPhrases.UserTable, GetUserTable, "UserID"));
            TableInfoList.Add(new TableInfo("Interface", CommonPhrases.InterfaceTable, GetInterfaceTable, "ItfID"));
            TableInfoList.Add(new TableInfo("Right", CommonPhrases.RightTable, GetRightTable, ""));
            TableInfoList.Add(new TableInfo("CnlType", CommonPhrases.CnlTypeTable, GetCnlTypeTable, "CnlTypeID"));
            TableInfoList.Add(new TableInfo("CmdType", CommonPhrases.CmdTypeTable, GetCmdTypeTable, "CmdTypeID"));
            TableInfoList.Add(new TableInfo("EvType", CommonPhrases.EvTypeTable, GetEvTypeTable, "CnlStatus"));
            TableInfoList.Add(new TableInfo("KPType", CommonPhrases.KPTypeTable, GetKPTypeTable, "KPTypeID"));
            TableInfoList.Add(new TableInfo("Param", CommonPhrases.ParamTable, GetParamTable, "ParamID"));
            TableInfoList.Add(new TableInfo("Unit", CommonPhrases.UnitTable, GetUnitTable, "UnitID"));
            TableInfoList.Add(new TableInfo("CmdVal", CommonPhrases.CmdValTable, GetCmdValTable, "CmdValID"));
            TableInfoList.Add(new TableInfo("Format", CommonPhrases.FormatTable, GetFormatTable, "FormatID"));
            TableInfoList.Add(new TableInfo("Formula", CommonPhrases.FormulaTable, GetFormulaTable, "Name", false));
        }


        /// <summary>
        /// Получить список информации о таблицах
        /// </summary>
        public static List<TableInfo> TableInfoList { get; private set; }


        /// <summary>
        /// Перевести имя таблицы
        /// </summary>
        private static string TranslateTableName(string tableName)
        {
            foreach (TableInfo tableInfo in TableInfoList)
                if (tableInfo.Name == tableName)
                    return tableInfo.Header;
            return tableName;
        }

        /// <summary>
        /// Перевести имя столбца
        /// </summary>
        private static string TranslateColName(string colName, DataTable dataTable)
        {
            DataGridViewColumn[] cols = dataTable == null ? null : 
                dataTable.ExtendedProperties["Columns"] as DataGridViewColumn[];
            if (cols != null)
            {
                foreach (DataGridViewColumn col in cols)
                {
                    if (col.Name.Equals(colName, StringComparison.OrdinalIgnoreCase))
                        return col.HeaderText;
                }
            }
            return colName;
        }
        
        /// <summary>
        /// Перевести имя ограничения
        /// </summary>
        private static string TranslateConstrName(string constrName, DataTable dataTable)
        {
            if (dataTable != null)
            {
                if (constrName.StartsWith("PK_"))
                {
                    // получение из заданной таблицы и перевод имён столбцов, входящих в первичный ключ
                    DataColumnCollection cols = dataTable.Columns;
                    if (constrName == "PK_Right")
                    {
                        if (cols.Count >= 2)
                            return TranslateColName(cols[0].ColumnName, dataTable) + " - " +
                                TranslateColName(cols[1].ColumnName, dataTable);
                    }
                    else
                    {
                        if (cols.Count >= 1)
                            return TranslateColName(cols[0].ColumnName, dataTable);
                    }
                }
                else if (constrName.StartsWith("fk_"))
                {
                    // получение из имени ограничения и перевод имени таблицы или столбца
                    string[] parts = constrName.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length >= 3)
                        return string.Equals(dataTable.TableName, parts[1], StringComparison.OrdinalIgnoreCase) ?
                            TranslateColName(parts[2], dataTable) : TranslateTableName(parts[1]);
                }
            }

            return constrName;
        }

        /// <summary>
        /// Перевести заголовки столбцов таблицы
        /// </summary>
        private static DataGridViewColumn[] TranslateColHeaders(string dictName, DataGridViewColumn[] cols)
        {
            Localization.Dict dict;
            if (Localization.Dictionaries.TryGetValue(dictName, out dict))
            {
                foreach (DataGridViewColumn col in cols)
                    if (dict.Phrases.ContainsKey(col.Name))
                        col.HeaderText = dict.Phrases[col.Name];
            }
            return cols;
        }


        /// <summary>
        /// Заполнить таблицу на основе заданного запроса, создать адаптер данных и команды на изменение данных
        /// </summary>
        private static void FillTable(DataTable dataTable, string sql)
        {
            SqlCeDataAdapter adapter = new SqlCeDataAdapter(sql, AppData.Conn);
            adapter.ContinueUpdateOnError = true;
            adapter.FillSchema(dataTable, SchemaType.Source); // для определения свойств столбцов AllowDBNull, MaxLength
            adapter.Fill(dataTable);

            dataTable.ExtendedProperties.Add("DataAdapter", adapter);
            dataTable.BeginLoadData(); // выключить контроль ограничений вводимых данных

            SqlCeCommandBuilder builder = new SqlCeCommandBuilder(adapter);
            builder.ConflictOption = ConflictOption.OverwriteChanges;
        }

        /// <summary>
        /// Создать столбец с текстовыми ячейками
        /// </summary>
        private static DataGridViewTextBoxColumn NewTextBoxColumn(string headerText, string dataPropertyName, 
            bool readOnly = false)
        {
            DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();
            col.Name = dataPropertyName;
            col.HeaderText = headerText;
            col.DataPropertyName = dataPropertyName;
            col.ReadOnly = readOnly;
            return col;
        }

        /// <summary>
        /// Создать столбец с ячейками, значения которых выбираются из списка
        /// </summary>
        private static DataGridViewComboBoxColumn NewComboBoxColumn(string headerText, string dataPropertyName,
            DataTable dataSource, string displayMember, string valueMember)
        {
            DataGridViewComboBoxColumn col = new DataGridViewComboBoxColumn();
            col.Name = dataPropertyName;
            col.HeaderText = headerText;
            col.DataPropertyName = dataPropertyName;
            col.DataSource = dataSource;
            col.DisplayMember = displayMember;
            col.ValueMember = valueMember;
            col.SortMode = DataGridViewColumnSortMode.Automatic;
            col.DisplayStyleForCurrentCellOnly = true;
            return col;
        }

        /// <summary>
        /// Создать столбец с ячейками, которые могут принимать значения истина или ложь
        /// </summary>
        private static DataGridViewCheckBoxColumn NewCheckBoxColumn(string headerText, string dataPropertyName)
        {
            DataGridViewCheckBoxColumn col = new DataGridViewCheckBoxColumn();
            col.Name = dataPropertyName;
            col.HeaderText = headerText;
            col.DataPropertyName = dataPropertyName;
            col.SortMode = DataGridViewColumnSortMode.Automatic;
            return col;
        }


        /// <summary>
        /// Получить колонки для таблицы объектов
        /// </summary>
        private static DataGridViewColumn[] GetObjTableCols()
        {
            return TranslateColHeaders("ScadaAdmin.Tables.ObjTable", new DataGridViewColumn[]
            {
                NewTextBoxColumn("Номер", "ObjNum"),
                NewTextBoxColumn("Наименование", "Name"),
                NewTextBoxColumn("Описание", "Descr")
            });
        }

        /// <summary>
        /// Получить колонки для таблицы линий связи
        /// </summary>
        private static DataGridViewColumn[] GetCommLineTableCols()
        {
            return TranslateColHeaders("ScadaAdmin.Tables.CommLineTable", new DataGridViewColumn[]
            {
                NewTextBoxColumn("Номер", "CommLineNum"),
                NewTextBoxColumn("Наименование", "Name"),
                NewTextBoxColumn("Описание", "Descr")
            });
        }

        /// <summary>
        /// Получить колонки для таблицы КП
        /// </summary>
        private static DataGridViewColumn[] GetKPTableCols()
        {
            return TranslateColHeaders("ScadaAdmin.Tables.KPTable", new DataGridViewColumn[]
            {
                NewTextBoxColumn("Номер", "KPNum"),
                NewTextBoxColumn("Наименование", "Name"),
                NewComboBoxColumn("Тип КП", "KPTypeID", GetKPTypeTable(), "Name", "KPTypeID"),
                NewTextBoxColumn("Адрес", "Address"),
                NewTextBoxColumn("Позывной", "CallNum"),
                NewComboBoxColumn("Линия связи", "CommLineNum", 
                    GetCommLineTable().Rows.Add(DBNull.Value, " ").Table, "Name", "CommLineNum"),
                NewTextBoxColumn("Описание", "Descr")
            });
        }

        /// <summary>
        /// Получить колонки для таблицы входных каналов
        /// </summary>
        private static DataGridViewColumn[] GetInCnlTableCols()
        {
            return TranslateColHeaders("ScadaAdmin.Tables.InCnlTable", new DataGridViewColumn[]
            {
                NewTextBoxColumn("Номер", "CnlNum"),
                NewCheckBoxColumn("Активный", "Active"),
                NewTextBoxColumn("Наименование", "Name"),
                NewComboBoxColumn("Тип канала", "CnlTypeID", GetCnlTypeTable(), "Name", "CnlTypeID"),
                NewComboBoxColumn("Объект", "ObjNum", 
                    GetObjTable().Rows.Add(DBNull.Value, " ").Table, "Name", "ObjNum"),
                NewComboBoxColumn("КП", "KPNum",
                    GetKPTable().Rows.Add(DBNull.Value, " ").Table, "Name", "KPNum"),
                NewTextBoxColumn("Сигнал", "Signal"),
                NewCheckBoxColumn("Исп. формулу", "FormulaUsed"),
                NewTextBoxColumn("Формула", "Formula"),
                NewCheckBoxColumn("Усреднение", "Averaging"),
                NewComboBoxColumn("Величина", "ParamID",
                    GetParamTable().Rows.Add(DBNull.Value, " ").Table, "Name", "ParamID"),
                NewComboBoxColumn("Формат", "FormatID",
                    GetFormatTable().Rows.Add(DBNull.Value, " ").Table, "Name", "FormatID"),
                NewComboBoxColumn("Размерность", "UnitID",
                    GetUnitTable().Rows.Add(DBNull.Value, " ").Table, "Name", "UnitID"),
                NewTextBoxColumn("Номер канала упр.", "CtrlCnlNum"),
                NewCheckBoxColumn("Запись событий", "EvEnabled"),
                NewCheckBoxColumn("Звук события", "EvSound"),
                NewCheckBoxColumn("Соб. по изм.", "EvOnChange"),
                NewCheckBoxColumn("Соб. по неопр. сост.", "EvOnUndef"),
                NewTextBoxColumn("Ниж. авар. гр.", "LimLowCrash"),
                NewTextBoxColumn("Ниж. гр.", "LimLow"),
                NewTextBoxColumn("Верх. гр.", "LimHigh"),
                NewTextBoxColumn("Верх. авар. гр.", "LimHighCrash"),
                NewTextBoxColumn("Время изменения", "ModifiedDT", true)
            });
        }

        /// <summary>
        /// Получить колонки для таблицы каналов управления
        /// </summary>
        private static DataGridViewColumn[] GetCtrlCnlTableCols()
        {
            return TranslateColHeaders("ScadaAdmin.Tables.CtrlCnlTable", new DataGridViewColumn[]
            {
                NewTextBoxColumn("Номер", "CtrlCnlNum"),
                NewCheckBoxColumn("Активный", "Active"),
                NewTextBoxColumn("Наименование", "Name"),
                NewComboBoxColumn("Тип команды", "CmdTypeID", GetCmdTypeTable(), "Name", "CmdTypeID"),
                NewComboBoxColumn("Объект", "ObjNum",
                    GetObjTable().Rows.Add(DBNull.Value, " ").Table, "Name", "ObjNum"),
                NewComboBoxColumn("КП", "KPNum",
                    GetKPTable().Rows.Add(DBNull.Value, " ").Table, "Name", "KPNum"),
                NewTextBoxColumn("Номер команды", "CmdNum"),
                NewComboBoxColumn("Значения команды", "CmdValID",
                    GetCmdValTable().Rows.Add(DBNull.Value, " ").Table, "Name", "CmdValID"),
                NewCheckBoxColumn("Исп. формулу", "FormulaUsed"),
                NewTextBoxColumn("Формула", "Formula"),
                NewCheckBoxColumn("Запись событий", "EvEnabled"),
                NewTextBoxColumn("Время изменения", "ModifiedDT", true)
            });
        }

        /// <summary>
        /// Получить колонки для таблицы ролей
        /// </summary>
        private static DataGridViewColumn[] GetRoleTableCols()
        {
            return TranslateColHeaders("ScadaAdmin.Tables.RoleTable", new DataGridViewColumn[]
            {
                NewTextBoxColumn("Идент.", "RoleID"),
                NewTextBoxColumn("Наименование", "Name"),
                NewTextBoxColumn("Описание", "Descr")
            });
        }

        /// <summary>
        /// Получить колонки для таблицы пользователей
        /// </summary>
        private static DataGridViewColumn[] GetUserTableCols()
        {
            return TranslateColHeaders("ScadaAdmin.Tables.UserTable", new DataGridViewColumn[]
            {
                NewTextBoxColumn("Идент.", "UserID"),
                NewTextBoxColumn("Имя", "Name"),
                NewTextBoxColumn("Пароль", "Password"),
                NewComboBoxColumn("Роль", "RoleID", GetRoleTable(), "Name", "RoleID"),
                NewTextBoxColumn("Описание", "Descr")
            });
        }

        /// <summary>
        /// Получить колонки для таблицы интерфейса
        /// </summary>
        private static DataGridViewColumn[] GetInterfaceTableCols()
        {
            return TranslateColHeaders("ScadaAdmin.Tables.InterfaceTable", new DataGridViewColumn[]
            {
                NewTextBoxColumn("Идент.", "ItfID"),
                NewTextBoxColumn("Путь", "Name"),
                NewTextBoxColumn("Заголовок", "Descr")
            });
        }

        /// <summary>
        /// Получить колонки для таблицы прав
        /// </summary>
        private static DataGridViewColumn[] GetRightTableCols()
        {
            return TranslateColHeaders("ScadaAdmin.Tables.RightTable", new DataGridViewColumn[]
            {
                NewComboBoxColumn("Объект интерфейса", "ItfID", GetInterfaceTable(), "Name", "ItfID"),
                NewComboBoxColumn("Роль", "RoleID", GetRoleTable(), "Name", "RoleID"),
                NewCheckBoxColumn("Просмотр", "ViewRight"),
                NewCheckBoxColumn("Управление", "CtrlRight")
            });
        }

        /// <summary>
        /// Получить колонки для таблицы типов каналов
        /// </summary>
        private static DataGridViewColumn[] GetCnlTypeTableCols()
        {
            return TranslateColHeaders("ScadaAdmin.Tables.CnlTypeTable", new DataGridViewColumn[]
            {
                NewTextBoxColumn("Идент.", "CnlTypeID"),
                NewTextBoxColumn("Наименование", "Name"),
                NewTextBoxColumn("Сокращение", "ShtName"),
                NewTextBoxColumn("Описание", "Descr")
            });
        }

        /// <summary>
        /// Получить колонки для таблицы типов каналов
        /// </summary>
        private static DataGridViewColumn[] GetCmdTypeTableCols()
        {
            return TranslateColHeaders("ScadaAdmin.Tables.CmdTypeTable", new DataGridViewColumn[]
            {
                NewTextBoxColumn("Идент.", "CmdTypeID"),
                NewTextBoxColumn("Наименование", "Name"),
                NewTextBoxColumn("Описание", "Descr")
            });
        }

        /// <summary>
        /// Получить колонки для таблицы типов событий
        /// </summary>
        private static DataGridViewColumn[] GetEvTypeTableCols()
        {
            return TranslateColHeaders("ScadaAdmin.Tables.EvTypeTable", new DataGridViewColumn[]
            {
                NewTextBoxColumn("Статус", "CnlStatus"),
                NewTextBoxColumn("Наименование", "Name"),
                NewTextBoxColumn("Цвет", "Color"),
                NewTextBoxColumn("Описание", "Descr")
            });
        }

        /// <summary>
        /// Получить колонки для таблицы типов КП
        /// </summary>
        private static DataGridViewColumn[] GetKPTypeTableCols()
        {
            return TranslateColHeaders("ScadaAdmin.Tables.KPTypeTable", new DataGridViewColumn[]
            {
                NewTextBoxColumn("Идент.", "KPTypeID"),
                NewTextBoxColumn("Наименование", "Name"),
                NewTextBoxColumn("Имя файла DLL", "DllFileName"),
                NewTextBoxColumn("Описание", "Descr")
            });
        }

        /// <summary>
        /// Получить колонки для таблицы величин (параметров)
        /// </summary>
        private static DataGridViewColumn[] GetParamTableCols()
        {
            return TranslateColHeaders("ScadaAdmin.Tables.ParamTable", new DataGridViewColumn[]
            {
                NewTextBoxColumn("Идент.", "ParamID"),
                NewTextBoxColumn("Наименование", "Name"),
                NewTextBoxColumn("Обозначение", "Sign"),
                NewTextBoxColumn("Имя файла значка", "IconFileName")
            });
        }

        /// <summary>
        /// Получить колонки для таблицы размерностей
        /// </summary>
        private static DataGridViewColumn[] GetUnitTableCols()
        {
            return TranslateColHeaders("ScadaAdmin.Tables.UnitTable", new DataGridViewColumn[]
            {
                NewTextBoxColumn("Идент.", "UnitID"),
                NewTextBoxColumn("Наименование", "Name"),
                NewTextBoxColumn("Обозначение", "Sign"),
                NewTextBoxColumn("Описание", "Descr")
            });
        }

        /// <summary>
        /// Получить колонки для таблицы значений команд
        /// </summary>
        private static DataGridViewColumn[] GetCmdValTableCols()
        {
            return TranslateColHeaders("ScadaAdmin.Tables.CmdValTable", new DataGridViewColumn[]
            {
                NewTextBoxColumn("Идент.", "CmdValID"),
                NewTextBoxColumn("Наименование", "Name"),
                NewTextBoxColumn("Значение", "Val"),
                NewTextBoxColumn("Описание", "Descr")
            });
        }

        /// <summary>
        /// Получить колонки для таблицы форматов чисел
        /// </summary>
        private static DataGridViewColumn[] GetFormatTableCols()
        {
            return TranslateColHeaders("ScadaAdmin.Tables.FormatTable", new DataGridViewColumn[]
            {
                NewTextBoxColumn("Идент.", "FormatID"),
                NewTextBoxColumn("Наименование", "Name"),
                NewCheckBoxColumn("Числовой", "ShowNumber"),
                NewTextBoxColumn("Кол-во знаков", "DecDigits")
            });
        }

        /// <summary>
        /// Получить колонки для таблицы формул
        /// </summary>
        private static DataGridViewColumn[] GetFormulaTableCols()
        {
            return TranslateColHeaders("ScadaAdmin.Tables.FormulaTable", new DataGridViewColumn[]
            {
                NewTextBoxColumn("Наименование", "Name"),
                NewTextBoxColumn("Исходный код", "Source"),
                NewTextBoxColumn("Описание", "Descr")
            });
        }


        /// <summary>
        /// Сохранить изменения таблицы в БД
        /// </summary>
        public static bool UpdateData(DataTable dataTable, out string errMsg)
        {
            try
            {
                if (dataTable != null)
                {
                    SqlCeDataAdapter adapter = dataTable.ExtendedProperties["DataAdapter"] as SqlCeDataAdapter;
                    if (adapter != null)
                    {
                        adapter.Update(dataTable);

                        if (dataTable.HasErrors)
                        {
                            DataRow[] rowsInError = dataTable.GetErrors();
                            StringBuilder sb = new StringBuilder();
                            foreach (DataRow row in rowsInError)
                            {
                                string rowError = TranlateErrorMessage(row.RowError, dataTable);
                                row.RowError = rowError;
                                sb.AppendLine(rowError);
                            }

                            errMsg = AppPhrases.UpdateDataError + ":\r\n" + sb.ToString().TrimEnd();
                            return false;
                        }
                    }
                }

                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = AppPhrases.UpdateDataError + ":\r\n" + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Проверить корректность значения ячейки
        /// </summary>
        public static bool ValidateCell(DataTable dataTable, DataColumn dataColumn, string cellVal, out string errMsg)
        {
            bool result = true;
            errMsg = "";

            if (!string.IsNullOrEmpty(cellVal) && dataTable != null && dataColumn != null)
            {
                if (dataColumn.DataType == typeof(string))
                {
                    // проверка строковых значений для всех таблиц
                    result = AppUtils.ValidateStr(cellVal, dataColumn.MaxLength, out errMsg);
                }
                else
                {
                    string tableName = dataTable.TableName;
                    string colName = dataColumn.ColumnName;

                    // проверка для численных значений для конкретных таблиц
                    if (tableName == "Obj")
                    {
                        if (colName == "ObjNum")
                            result = AppUtils.ValidateInt(cellVal, 1, ushort.MaxValue, out errMsg);
                    }
                    else if (tableName == "CommLine")
                    {
                        if (colName == "CommLineNum")
                            result = AppUtils.ValidateInt(cellVal, 1, ushort.MaxValue, out errMsg);
                    }
                    else if (tableName == "KP")
                    {
                        if (colName == "KPNum")
                            result = AppUtils.ValidateInt(cellVal, 1, ushort.MaxValue, out errMsg);
                        else if (colName == "Address")
                            result = AppUtils.ValidateInt(cellVal, 0, int.MaxValue, out errMsg);
                    }
                    else if (tableName == "InCnl")
                    {
                        if (colName == "CnlNum")
                            result = AppUtils.ValidateInt(cellVal, 1, ushort.MaxValue, out errMsg);
                        else if (colName == "Signal")
                            result = AppUtils.ValidateInt(cellVal, 1, ushort.MaxValue, out errMsg);
                        else if (colName == "CtrlCnlNum")
                            result = AppUtils.ValidateInt(cellVal, 1, ushort.MaxValue, out errMsg);
                        else if (colName == "LimLowCrash" || colName == "LimLow" || colName == "LimHigh" || colName == "LimHighCrash")
                            result = AppUtils.ValidateDouble(cellVal, out errMsg);
                    }
                    else if (tableName == "CtrlCnl")
                    {
                        if (colName == "CtrlCnlNum")
                            result = AppUtils.ValidateInt(cellVal, 1, ushort.MaxValue, out errMsg);
                        else if (colName == "CmdNum")
                            result = AppUtils.ValidateInt(cellVal, 1, ushort.MaxValue, out errMsg);
                    }
                    else if (tableName == "Role")
                    {
                        if (colName == "RoleID")
                            result = AppUtils.ValidateInt(cellVal, 0, ushort.MaxValue, out errMsg);
                    }
                    else if (tableName == "User")
                    {
                        if (colName == "UserID")
                            result = AppUtils.ValidateInt(cellVal, 1, ushort.MaxValue, out errMsg);
                    }
                    else if (tableName == "Interface")
                    {
                        if (colName == "ItfID")
                            result = AppUtils.ValidateInt(cellVal, 1, ushort.MaxValue, out errMsg);
                    }
                    else if (tableName == "CnlType")
                    {
                        if (colName == "CnlTypeID")
                            result = AppUtils.ValidateInt(cellVal, 1, ushort.MaxValue, out errMsg);
                    }
                    else if (tableName == "CmdType")
                    {
                        if (colName == "CmdTypeID")
                            result = AppUtils.ValidateInt(cellVal, 0, ushort.MaxValue, out errMsg);
                    }
                    else if (tableName == "EvType")
                    {
                        if (colName == "CnlStatus")
                            result = AppUtils.ValidateInt(cellVal, 0, byte.MaxValue, out errMsg);
                    }
                    else if (tableName == "KPType")
                    {
                        if (colName == "KPTypeID")
                            result = AppUtils.ValidateInt(cellVal, 1, ushort.MaxValue, out errMsg);
                    }
                    else if (tableName == "Param")
                    {
                        if (colName == "ParamID")
                            result = AppUtils.ValidateInt(cellVal, 1, ushort.MaxValue, out errMsg);
                    }
                    else if (tableName == "Unit")
                    {
                        if (colName == "UnitID")
                            result = AppUtils.ValidateInt(cellVal, 1, ushort.MaxValue, out errMsg);
                    }
                    else if (tableName == "CmdVal")
                    {
                        if (colName == "CmdValID")
                            result = AppUtils.ValidateInt(cellVal, 1, ushort.MaxValue, out errMsg);
                    }
                    else if (tableName == "Format")
                    {
                        if (colName == "FormatID")
                            result = AppUtils.ValidateInt(cellVal, 0, ushort.MaxValue, out errMsg);
                        else if (colName == "DecDigits")
                            result = AppUtils.ValidateInt(cellVal, 0, 100, out errMsg);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Заполнить схему данных таблицы
        /// </summary>
        public static void FillTableSchema(DataTable dataTable)
        {
            try
            {
                if (dataTable != null)
                {
                    string sql = "select * from [" + dataTable.TableName + "]";
                    SqlCeDataAdapter adapter = new SqlCeDataAdapter(sql, AppData.Conn);
                    adapter.ContinueUpdateOnError = true;
                    adapter.FillSchema(dataTable, SchemaType.Source);

                    dataTable.ExtendedProperties.Add("DataAdapter", adapter);
                    dataTable.BeginLoadData(); // выключить контроль ограничений вводимых данных

                    SqlCeCommandBuilder builder = new SqlCeCommandBuilder(adapter);
                    builder.ConflictOption = ConflictOption.OverwriteChanges;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(AppPhrases.FillSchemaError + ":\r\n" + ex.Message);
            }
        }

        /// <summary>
        /// Перевести сообщение об ошибке
        /// </summary>
        public static string TranlateErrorMessage(string errMsg, DataTable dataTable)
        {
            string result;

            try
            {
                if (errMsg.StartsWith("The column cannot contain null values"))
                {
                    int pos1 = errMsg.IndexOf("Column name = ");
                    int pos2 = pos1 > 0 ? errMsg.IndexOf(",", pos1 + 14) : -1;
                    string colName = 0 < pos1 && pos1 < pos2 ?
                        TranslateColName(errMsg.Substring(pos1 + 14, pos2 - pos1 - 14), dataTable) : "";
                    result = string.Format(AppPhrases.DataRequired, colName);
                }
                else if (errMsg.StartsWith("A duplicate value cannot be inserted into a unique index"))
                {
                    int pos1 = errMsg.IndexOf("Constraint name = ");
                    int pos2 = pos1 > 0 ? errMsg.IndexOf(" ", pos1 + 18) : -1;
                    string colName = 0 < pos1 && pos1 < pos2 ?
                        TranslateConstrName(errMsg.Substring(pos1 + 18, pos2 - pos1 - 18), dataTable) : "";
                    result = string.Format(AppPhrases.UniqueRequired, colName);
                }
                else if (errMsg.StartsWith("The primary key value cannot be deleted because references to this key still exist"))
                {
                    int pos1 = errMsg.IndexOf("Foreign key constraint name = ");
                    int pos2 = pos1 > 0 ? errMsg.IndexOf(" ", pos1 + 30) : -1;
                    string tblName = 0 < pos1 && pos1 < pos2 ?
                        TranslateConstrName(errMsg.Substring(pos1 + 30, pos2 - pos1 - 30), dataTable) : "";
                    result = string.Format(AppPhrases.UnableDeleteRow, tblName);
                }
                else if (errMsg.StartsWith("A foreign key value cannot be inserted because a corresponding primary key value does not exist"))
                {
                    int pos1 = errMsg.IndexOf("Foreign key constraint name = ");
                    int pos2 = pos1 > 0 ? errMsg.IndexOf(" ", pos1 + 30) : -1;
                    string colName = 0 < pos1 && pos1 < pos2 ?
                        TranslateConstrName(errMsg.Substring(pos1 + 30, pos2 - pos1 - 30), dataTable) : "";
                    result = string.Format(AppPhrases.UnableAddRow, colName);
                }
                else
                {
                    result = errMsg;
                }
            }
            catch (Exception ex)
            {
                AppData.ErrLog.WriteAction(AppPhrases.TranslateError + ":\r\n" + ex.Message, Log.ActTypes.Exception);
                result = errMsg;
            }

            return result;
        }


        /// <summary>
        /// Получить таблицу объектов
        /// </summary>
        public static DataTable GetObjTable()
        {
            DataTable table = new DataTable("Obj");

            try
            {
                table.ExtendedProperties.Add("Columns", GetObjTableCols());
                FillTable(table, "select ObjNum, Name, Descr from Obj order by ObjNum");
                table.DefaultView.Sort = "ObjNum";
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(AppPhrases.GetTableError, CommonPhrases.ObjTable) + 
                    ":\r\n" + ex.Message);
            }

            return table;
        }

        /// <summary>
        /// Получить таблицу линий связи
        /// </summary>
        public static DataTable GetCommLineTable()
        {
            DataTable table = new DataTable("CommLine");

            try
            {
                table.ExtendedProperties.Add("Columns", GetCommLineTableCols());
                FillTable(table, "select CommLineNum, Name, Descr from CommLine order by CommLineNum");
                table.DefaultView.Sort = "CommLineNum";
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(AppPhrases.GetTableError, CommonPhrases.CommLineTable) +
                    ":\r\n" + ex.Message);
            }

            return table;
        }

        /// <summary>
        /// Получить таблицу КП
        /// </summary>
        public static DataTable GetKPTable()
        {
            DataTable table = new DataTable("KP");

            try
            {
                table.ExtendedProperties.Add("Columns", GetKPTableCols());
                FillTable(table, "select KPNum, Name, KPTypeID, Address, CallNum, CommLineNum, Descr " +
                    "from KP order by KPNum");
                table.DefaultView.Sort = "KPNum";
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(AppPhrases.GetTableError, CommonPhrases.KPTable) +
                    ":\r\n" + ex.Message);
            }

            return table;
        }

        /// <summary>
        /// Получить таблицу входных каналов
        /// </summary>
        public static DataTable GetInCnlTable()
        {
            DataTable table = new DataTable("InCnl");

            try
            {
                table.ExtendedProperties.Add("Columns", GetInCnlTableCols());
                FillTable(table, InCnlSql + "order by CnlNum");
                table.DefaultView.Sort = "CnlNum";
                table.Columns["Active"].DefaultValue = true;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(AppPhrases.GetTableError, CommonPhrases.InCnlTable) +
                    ":\r\n" + ex.Message);
            }

            return table;
        }

        /// <summary>
        /// Получить таблицу входных каналов по номеру объекта
        /// </summary>
        public static DataTable GetInCnlTableByObjNum(int objNum)
        {
            DataTable table = new DataTable("InCnl");

            try
            {
                table.ExtendedProperties.Add("Columns", GetInCnlTableCols());
                FillTable(table, InCnlSql + 
                    "where " + (objNum > 0 ? "ObjNum = " + objNum : "ObjNum is null") + " order by CnlNum");
                table.DefaultView.Sort = "CnlNum";
                table.Columns["Active"].DefaultValue = true;
                if (objNum > 0)
                    table.Columns["ObjNum"].DefaultValue = objNum;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(AppPhrases.GetTableByObjError, CommonPhrases.InCnlTable) +
                    ":\r\n" + ex.Message);
            }

            return table;
        }

        /// <summary>
        /// Получить таблицу входных каналов по номеру КП
        /// </summary>
        public static DataTable GetInCnlTableByKPNum(int kpNum)
        {
            DataTable table = new DataTable("InCnl");

            try
            {
                table.ExtendedProperties.Add("Columns", GetInCnlTableCols());
                FillTable(table, InCnlSql + 
                    "where " + (kpNum > 0 ? "KPNum = " + kpNum : "KPNum is null") + " order by CnlNum");
                table.DefaultView.Sort = "CnlNum";
                table.Columns["Active"].DefaultValue = true;
                if (kpNum > 0)
                    table.Columns["KPNum"].DefaultValue = kpNum;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(AppPhrases.GetTableByKPError, CommonPhrases.InCnlTable) +
                    ":\r\n" + ex.Message);
            }

            return table;
        }

        /// <summary>
        /// Получить таблицу каналов управления
        /// </summary>
        public static DataTable GetCtrlCnlTable()
        {
            DataTable table = new DataTable("CtrlCnl");

            try
            {
                table.ExtendedProperties.Add("Columns", GetCtrlCnlTableCols());
                FillTable(table, CtrlCnlSql + "order by CtrlCnlNum");
                table.DefaultView.Sort = "CtrlCnlNum";
                table.Columns["Active"].DefaultValue = true;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(AppPhrases.GetTableError, CommonPhrases.CtrlCnlTable) +
                    ":\r\n" + ex.Message);
            }

            return table;
        }

        /// <summary>
        /// Получить таблицу каналов управления по номеру объекта
        /// </summary>
        public static DataTable GetCtrlCnlTableByObjNum(int objNum)
        {
            DataTable table = new DataTable("CtrlCnl");

            try
            {
                table.ExtendedProperties.Add("Columns", GetCtrlCnlTableCols());
                FillTable(table, CtrlCnlSql + 
                    "where " + (objNum > 0 ? "ObjNum = " + objNum : "ObjNum is null") + " order by CtrlCnlNum");
                table.DefaultView.Sort = "CtrlCnlNum";
                table.Columns["Active"].DefaultValue = true;
                if (objNum > 0)
                    table.Columns["ObjNum"].DefaultValue = objNum;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(AppPhrases.GetTableByObjError, CommonPhrases.CtrlCnlTable) +
                    ":\r\n" + ex.Message);
            }

            return table;
        }

        /// <summary>
        /// Получить таблицу каналов управления по номеру КП
        /// </summary>
        public static DataTable GetCtrlCnlTableByKPNum(int kpNum)
        {
            DataTable table = new DataTable("CtrlCnl");

            try
            {
                table.ExtendedProperties.Add("Columns", GetCtrlCnlTableCols());
                FillTable(table, CtrlCnlSql +
                    "where " + (kpNum > 0 ? "KPNum = " + kpNum : "KPNum is null") + " order by CtrlCnlNum");
                table.DefaultView.Sort = "CtrlCnlNum";
                table.Columns["Active"].DefaultValue = true;
                if (kpNum > 0)
                    table.Columns["KPNum"].DefaultValue = kpNum;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(AppPhrases.GetTableByKPError, CommonPhrases.CtrlCnlTable) +
                    ":\r\n" + ex.Message);
            }

            return table;
        }

        /// <summary>
        /// Получить таблицу ролей
        /// </summary>
        public static DataTable GetRoleTable()
        {
            DataTable table = new DataTable("Role");

            try
            {
                table.ExtendedProperties.Add("Columns", GetRoleTableCols());
                FillTable(table, "select RoleID, Name, Descr from Role order by RoleID");
                table.DefaultView.Sort = "RoleID";
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(AppPhrases.GetTableError, CommonPhrases.RoleTable) +
                    ":\r\n" + ex.Message);
            }

            return table;
        }

        /// <summary>
        /// Получить таблицу пользователей
        /// </summary>
        public static DataTable GetUserTable()
        {
            DataTable table = new DataTable("User");

            try
            {
                table.ExtendedProperties.Add("Columns", GetUserTableCols());
                FillTable(table, "select UserID, Name, Password, RoleID, Descr from [User] order by UserID");
                table.DefaultView.Sort = "UserID";
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(AppPhrases.GetTableError, CommonPhrases.UserTable) +
                    ":\r\n" + ex.Message);
            }

            return table;
        }

        /// <summary>
        /// Получить таблицу интерфейса
        /// </summary>
        public static DataTable GetInterfaceTable()
        {
            DataTable table = new DataTable("Interface");

            try
            {
                table.ExtendedProperties.Add("Columns", GetInterfaceTableCols());
                FillTable(table, "select ItfID, Name, Descr from Interface order by ItfID");
                table.DefaultView.Sort = "ItfID";
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(AppPhrases.GetTableError, CommonPhrases.InterfaceTable) +
                    ":\r\n" + ex.Message);
            }

            return table;
        }

        /// <summary>
        /// Получить таблицу прав
        /// </summary>
        public static DataTable GetRightTable()
        {
            DataTable table = new DataTable("Right");

            try
            {
                table.ExtendedProperties.Add("Columns", GetRightTableCols());
                FillTable(table, "select ItfID, RoleID, ViewRight, CtrlRight from [Right] order by ItfID, RoleID");
                table.DefaultView.Sort = "ItfID, RoleID";
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(AppPhrases.GetTableError, CommonPhrases.RightTable) +
                    ":\r\n" + ex.Message);
            }

            return table;
        }

        /// <summary>
        /// Получить таблицу типов каналов
        /// </summary>
        public static DataTable GetCnlTypeTable()
        {
            DataTable table = new DataTable("CnlType");

            try
            {
                table.ExtendedProperties.Add("Columns", GetCnlTypeTableCols());
                FillTable(table, "select CnlTypeID, Name, ShtName, Descr from CnlType order by CnlTypeID");
                table.DefaultView.Sort = "CnlTypeID";
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(AppPhrases.GetTableError, CommonPhrases.CnlTypeTable) +
                    ":\r\n" + ex.Message);
            }

            return table;
        }

        /// <summary>
        /// Получить таблицу типов команд
        /// </summary>
        public static DataTable GetCmdTypeTable()
        {
            DataTable table = new DataTable("CmdType");

            try
            {
                table.ExtendedProperties.Add("Columns", GetCmdTypeTableCols());
                FillTable(table, "select CmdTypeID, Name, Descr from CmdType order by CmdTypeID");
                table.DefaultView.Sort = "CmdTypeID";
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(AppPhrases.GetTableError, CommonPhrases.CmdTypeTable) +
                    ":\r\n" + ex.Message);
            }

            return table;
        }

        /// <summary>
        /// Получить таблицу типов событий
        /// </summary>
        public static DataTable GetEvTypeTable()
        {
            DataTable table = new DataTable("EvType");

            try
            {
                table.ExtendedProperties.Add("Columns", GetEvTypeTableCols());
                FillTable(table, "select CnlStatus, Name, Color, Descr from EvType order by CnlStatus");
                table.DefaultView.Sort = "CnlStatus";
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(AppPhrases.GetTableError, CommonPhrases.EvTypeTable) +
                    ":\r\n" + ex.Message);
            }

            return table;
        }

        /// <summary>
        /// Получить таблицу типов КП
        /// </summary>
        public static DataTable GetKPTypeTable()
        {
            DataTable table = new DataTable("KPType");

            try
            {
                table.ExtendedProperties.Add("Columns", GetKPTypeTableCols());
                FillTable(table, "select KPTypeID, Name, DllFileName, Descr from KPType order by KPTypeID");
                table.DefaultView.Sort = "KPTypeID";
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(AppPhrases.GetTableError, CommonPhrases.KPTypeTable) +
                    ":\r\n" + ex.Message);
            }

            return table;
        }

        /// <summary>
        /// Получить таблицу величин (параметров)
        /// </summary>
        public static DataTable GetParamTable()
        {
            DataTable table = new DataTable("Param");

            try
            {
                table.ExtendedProperties.Add("Columns", GetParamTableCols());
                FillTable(table, "select ParamID, Name, Sign, IconFileName from Param order by ParamID");
                table.DefaultView.Sort = "ParamID";
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(AppPhrases.GetTableError, CommonPhrases.ParamTable) +
                    ":\r\n" + ex.Message);
            }

            return table;
        }

        /// <summary>
        /// Получить таблицу размерностей
        /// </summary>
        public static DataTable GetUnitTable()
        {
            DataTable table = new DataTable("Unit");

            try
            {
                table.ExtendedProperties.Add("Columns", GetUnitTableCols());
                FillTable(table, "select UnitID, Name, Sign, Descr from Unit order by UnitID");
                table.DefaultView.Sort = "UnitID";
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(AppPhrases.GetTableError, CommonPhrases.UnitTable) +
                    ":\r\n" + ex.Message);
            }

            return table;
        }

        /// <summary>
        /// Получить таблицу значений команд
        /// </summary>
        public static DataTable GetCmdValTable()
        {
            DataTable table = new DataTable("CmdVal");

            try
            {
                table.ExtendedProperties.Add("Columns", GetCmdValTableCols());
                FillTable(table, "select CmdValID, Name, Val, Descr from CmdVal order by CmdValID");
                table.DefaultView.Sort = "CmdValID";
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(AppPhrases.GetTableError, CommonPhrases.CmdValTable) +
                    ":\r\n" + ex.Message);
            }

            return table;
        }

        /// <summary>
        /// Получить таблицу форматов чисел
        /// </summary>
        public static DataTable GetFormatTable()
        {
            DataTable table = new DataTable("Format");

            try
            {
                table.ExtendedProperties.Add("Columns", GetFormatTableCols());
                FillTable(table, "select FormatID, Name, ShowNumber, DecDigits from Format order by FormatID");
                table.DefaultView.Sort = "FormatID";
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(AppPhrases.GetTableError, CommonPhrases.FormatTable) +
                    ":\r\n" + ex.Message);
            }

            return table;
        }

        /// <summary>
        /// Получить таблицу формул
        /// </summary>
        public static DataTable GetFormulaTable()
        {
            DataTable table = new DataTable("Formula");

            try
            {
                table.ExtendedProperties.Add("Columns", GetFormulaTableCols());
                FillTable(table, "select Name, Source, Descr from Formula order by Name");
                table.DefaultView.Sort = "Name";
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(AppPhrases.GetTableError, CommonPhrases.FormulaTable) +
                    ":\r\n" + ex.Message);
            }

            return table;
        }


        /// <summary>
        /// Получить наименование канала управления по его номеру
        /// </summary>
        public static string GetCtrlCnlName(int ctrlCnlNum)
        {
            string name = "";
            try
            {
                SqlCeCommand cmd = new SqlCeCommand("select Name from CtrlCnl where CtrlCnlNum = @num", AppData.Conn);
                cmd.Parameters.Add("@num", SqlDbType.Int).Value = ctrlCnlNum;
                object obj = cmd.ExecuteScalar();
                if (obj != null)
                    name = obj.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception(AppPhrases.GetCtrlCnlNameError + ":\r\n" + ex.Message);
            }

            return name;
        }

        /// <summary>
        /// Получить список номеров входных каналов, упорядоченный по возрастанию
        /// </summary>
        public static List<int> GetInCnlNums()
        {
            List<int> inCnlNums = new List<int>();
            SqlCeDataReader reader = null;

            try
            {
                SqlCeCommand cmd = new SqlCeCommand("select CnlNum from InCnl order by CnlNum", AppData.Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                    inCnlNums.Add(reader.GetInt32(0));
            }
            catch (Exception ex)
            {
                throw new Exception(AppPhrases.GetInCnlNumsError + ":\r\n" + ex.Message);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return inCnlNums;
        }

        /// <summary>
        /// Получить список номеров каналов управления, упорядоченный по возрастанию
        /// </summary>
        public static List<int> GetCtrlCnlNums()
        {
            List<int> ctrlCnlNums = new List<int>();
            SqlCeDataReader reader = null;

            try
            {
                SqlCeCommand cmd = new SqlCeCommand("select CtrlCnlNum from CtrlCnl order by CtrlCnlNum", AppData.Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                    ctrlCnlNums.Add(reader.GetInt32(0));
            }
            catch (Exception ex)
            {
                throw new Exception(AppPhrases.GetCtrlCnlNumsError + ":\r\n" + ex.Message);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return ctrlCnlNums;
        }
    }
}
