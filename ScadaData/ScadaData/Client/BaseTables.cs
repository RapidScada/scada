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
 * Module   : ScadaData
 * Summary  : The tables of the configuration database
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

using System.Data;

namespace Scada.Client
{
    /// <summary>
    /// The tables of the configuration database
    /// <para>Таблицы базы конфигурации</para>
    /// </summary>
    public class BaseTables
    {
        /// <summary>
        /// Конструктор
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
                UserTable = new DataTable("User"),
                InterfaceTable = new DataTable("Interface"),
                RightTable = new DataTable("Right"),
                CnlTypeTable = new DataTable("CnlType"),
                CmdTypeTable = new DataTable("CmdType"),
                EvTypeTable = new DataTable("EvType"),
                ParamTable = new DataTable("Param"),
                UnitTable = new DataTable("Unit"),
                CmdValTable = new DataTable("CmdVal"),
                FormatTable = new DataTable("Format"),
                FormulaTable = new DataTable("Formula")
            };
        }


        /// <summary>
        /// Получить таблицу объектов
        /// </summary>
        public DataTable ObjTable { get; protected set; }

        /// <summary>
        /// Получить таблицу линий связи
        /// </summary>
        public DataTable CommLineTable { get; protected set; }

        /// <summary>
        /// Получить таблицу КП
        /// </summary>
        public DataTable KPTable { get; protected set; }

        /// <summary>
        /// Получить таблицу входных каналов
        /// </summary>
        public DataTable InCnlTable { get; protected set; }

        /// <summary>
        /// Получить таблицу каналов управления
        /// </summary>
        public DataTable CtrlCnlTable { get; protected set; }

        /// <summary>
        /// Получить таблицу ролей
        /// </summary>
        public DataTable RoleTable { get; protected set; }

        /// <summary>
        /// Получить таблицу пользователей
        /// </summary>
        public DataTable UserTable { get; protected set; }

        /// <summary>
        /// Получить таблицу объектов интерфейса
        /// </summary>
        public DataTable InterfaceTable { get; protected set; }

        /// <summary>
        /// Получить таблицу прав на объекты интерфейса
        /// </summary>
        public DataTable RightTable { get; protected set; }

        /// <summary>
        /// Получить таблицу типов входных каналов
        /// </summary>
        public DataTable CnlTypeTable { get; protected set; }

        /// <summary>
        /// Получить таблицу типов команд
        /// </summary>
        public DataTable CmdTypeTable { get; protected set; }

        /// <summary>
        /// Получить таблицу типов событий
        /// </summary>
        public DataTable EvTypeTable { get; protected set; }

        /// <summary>
        /// Получить таблицу величин (параметров)
        /// </summary>
        public DataTable ParamTable { get; protected set; }

        /// <summary>
        /// Получить таблицу размерностей
        /// </summary>
        public DataTable UnitTable { get; protected set; }

        /// <summary>
        /// Получить таблицу значений команд
        /// </summary>
        public DataTable CmdValTable { get; protected set; }

        /// <summary>
        /// Получить таблицу форматов чисел
        /// </summary>
        public DataTable FormatTable { get; protected set; }

        /// <summary>
        /// Получить таблицу формул
        /// </summary>
        public DataTable FormulaTable { get; protected set; }

        /// <summary>
        /// Получить массив ссылок на все таблицы базы конфигурации
        /// </summary>
        public DataTable[] AllTables { get; protected set; }

    }
}
