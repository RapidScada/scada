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
 * Module   : ScadaData
 * Summary  : The tables of the configuration database
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2019
 */

using System;
using System.Data;

namespace Scada.Data.Configuration
{
    /// <summary>
    /// The tables of the configuration database.
    /// <para>Таблицы базы конфигурации.</para>
    /// </summary>
    /// <remarks>
    /// After using DataTable.DefaultView.RowFilter restore the empty value.
    /// <para>После использования DataTable.DefaultView.RowFilter нужно вернуть пустое значение.</para></remarks>
    public class BaseTables
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public BaseTables()
        {
            AllTables = new DataTable[] 
            {
                ObjTable = new DataTable("Obj"),
                CommLineTable = new DataTable("CommLine"),
                KPTable = new DataTable("KP"),
                InCnlTable = new DataTable("InCnl"),
                CtrlCnlTable = new DataTable("CtrlCnl"),
                RoleTable = new DataTable("Role"),
                RoleRefTable = new DataTable("RoleRef"),
                UserTable = new DataTable("User"),
                InterfaceTable = new DataTable("Interface"),
                RightTable = new DataTable("Right"),
                CnlTypeTable = new DataTable("CnlType"),
                CmdTypeTable = new DataTable("CmdType"),
                EvTypeTable = new DataTable("EvType"),
                KPTypeTable = new DataTable("KPType"),
                ParamTable = new DataTable("Param"),
                UnitTable = new DataTable("Unit"),
                CmdValTable = new DataTable("CmdVal"),
                FormatTable = new DataTable("Format"),
                FormulaTable = new DataTable("Formula")
            };

            BaseAge = DateTime.MinValue;
        }


        /// <summary>
        /// Получить таблицу объектов.
        /// </summary>
        public DataTable ObjTable { get; protected set; }

        /// <summary>
        /// Получить таблицу линий связи.
        /// </summary>
        public DataTable CommLineTable { get; protected set; }

        /// <summary>
        /// Получить таблицу КП.
        /// </summary>
        public DataTable KPTable { get; protected set; }

        /// <summary>
        /// Получить таблицу входных каналов.
        /// </summary>
        public DataTable InCnlTable { get; protected set; }

        /// <summary>
        /// Получить таблицу каналов управления.
        /// </summary>
        public DataTable CtrlCnlTable { get; protected set; }

        /// <summary>
        /// Получить таблицу ролей.
        /// </summary>
        public DataTable RoleTable { get; protected set; }

        /// <summary>
        /// Получить таблицу наследования ролей.
        /// </summary>
        public DataTable RoleRefTable { get; protected set; }

        /// <summary>
        /// Получить таблицу пользователей.
        /// </summary>
        public DataTable UserTable { get; protected set; }

        /// <summary>
        /// Получить таблицу объектов интерфейса.
        /// </summary>
        public DataTable InterfaceTable { get; protected set; }

        /// <summary>
        /// Получить таблицу прав на объекты интерфейса.
        /// </summary>
        public DataTable RightTable { get; protected set; }

        /// <summary>
        /// Получить таблицу типов входных каналов.
        /// </summary>
        public DataTable CnlTypeTable { get; protected set; }

        /// <summary>
        /// Получить таблицу типов команд.
        /// </summary>
        public DataTable CmdTypeTable { get; protected set; }

        /// <summary>
        /// Получить таблицу типов событий.
        /// </summary>
        public DataTable EvTypeTable { get; protected set; }

        /// <summary>
        /// Получить таблицу типов КП.
        /// </summary>
        public DataTable KPTypeTable { get; protected set; }

        /// <summary>
        /// Получить таблицу величин (параметров).
        /// </summary>
        public DataTable ParamTable { get; protected set; }

        /// <summary>
        /// Получить таблицу размерностей.
        /// </summary>
        public DataTable UnitTable { get; protected set; }

        /// <summary>
        /// Получить таблицу значений команд.
        /// </summary>
        public DataTable CmdValTable { get; protected set; }

        /// <summary>
        /// Получить таблицу форматов чисел.
        /// </summary>
        public DataTable FormatTable { get; protected set; }

        /// <summary>
        /// Получить таблицу формул.
        /// </summary>
        public DataTable FormulaTable { get; protected set; }

        /// <summary>
        /// Получить массив ссылок на все таблицы базы конфигурации.
        /// </summary>
        public DataTable[] AllTables { get; protected set; }

        /// <summary>
        /// Получить или установить время последнего изменения успешно считанной базы конфигурации.
        /// </summary>
        public DateTime BaseAge { get; set; }

        /// <summary>
        /// Получить объект для синхронизации доступа к таблицам.
        /// </summary>
        public object SyncRoot
        {
            get
            {
                return this;
            }
        }


        /// <summary>
        /// Получить имя файла таблицы без директории.
        /// </summary>
        public static string GetFileName(DataTable dataTable)
        {
            if (dataTable == null)
                throw new ArgumentNullException("dataTable");

            return dataTable.TableName.ToLowerInvariant() + ".dat";
        }

        /// <summary>
        /// Проверить, что колонки таблицы существуют.
        /// </summary>
        public static bool CheckColumnsExist(DataTable dataTable, bool throwOnFail = false)
        {
            if (dataTable == null)
                throw new ArgumentNullException("dataTable");

            if (dataTable.Columns.Count > 0)
            {
                return true;
            }
            else if (throwOnFail)
            {
                throw new ScadaException(string.Format(Localization.UseRussian ?
                    "Таблица [{0}] не содержит колонок." :
                    "The table [{0}] does not contain columns.", dataTable.TableName));
            }
            else
            {
                return false;
            }
        }
    }
}
