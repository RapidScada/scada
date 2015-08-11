/*
 * Copyright 2015 Mikhail Shiryaev
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
 * Summary  : Output channel properties
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2008
 * Modified : 2015
 */

using System;

namespace Scada.Data
{
    /// <summary>
    /// Output channel properties
    /// <para>Свойства канала управления</para>
    /// </summary>
    public class CtrlCnlProps : IComparable<CtrlCnlProps>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public CtrlCnlProps()
            : this(0, "", BaseValues.CmdTypes.Standard)
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public CtrlCnlProps(int ctrlCnlNum, string ctrlCnlName, int cmdTypeID)
            : this()
        {
            CtrlCnlNum = ctrlCnlNum;
            CtrlCnlName = ctrlCnlName;
            CmdTypeID = cmdTypeID;
            ObjNum = 0;
            ObjName = "";
            KPNum = 0;
            KPName = "";
            CmdNum = 0;
            CmdValName = "";
            CmdValArr = null;
            FormulaUsed = false;
            Formula = "";
            EvEnabled = false;
        }


        /// <summary>
        /// Получить или установить номер канала управления
        /// </summary>
        public int CtrlCnlNum { get; set; }

        /// <summary>
        /// Получить или установить наименование канала управления
        /// </summary>
        public string CtrlCnlName { get; set; }

        /// <summary>
        /// Получить или установить идентификатор типа команды
        /// </summary>
        public int CmdTypeID { get; set; }

        /// <summary>
        /// Получить или установить номер объекта
        /// </summary>
        public int ObjNum { get; set; }

        /// <summary>
        /// Получить или установить наименование объекта
        /// </summary>
        public string ObjName { get; set; }

        /// <summary>
        /// Получить или установить номер КП
        /// </summary>
        public int KPNum { get; set; }

        /// <summary>
        /// Получить или установить наименование КП
        /// </summary>
        public string KPName { get; set; }
        
        /// <summary>
        /// Получить или установить номер команды
        /// </summary>
        public int CmdNum { get; set; }

        /// <summary>
        /// Получить или установить наименование значений команды
        /// </summary>
        public string CmdValName { get; set; }

        /// <summary>
        /// Получить или установить значения команды
        /// </summary>
        public string[] CmdValArr { get; set; }

        /// <summary>
        /// Получить или установить признак использования формулы
        /// </summary>
        public bool FormulaUsed { get; set; }

        /// <summary>
        /// Получить или установить формулу
        /// </summary>
        public string Formula { get; set; }

        /// <summary>
        /// Получить или установить признак записи событий
        /// </summary>
        public bool EvEnabled { get; set; }


        /// <summary>
        /// Сравнить текущий объект с другим объектом того же типа
        /// </summary>
        public int CompareTo(CtrlCnlProps other)
        {
            return CtrlCnlNum.CompareTo(other.CtrlCnlNum);
        }
    }
}