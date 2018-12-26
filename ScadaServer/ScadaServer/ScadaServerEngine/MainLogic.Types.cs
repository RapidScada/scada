/*
 * Copyright 2017 Mikhail Shiryaev
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
 * Module   : SCADA-Server Service
 * Summary  : Main server logic implementation. Derived types
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2013
 * Modified : 2017
 */

using System;
using System.IO;
using Scada.Data.Models;
using Scada.Data.Tables;

namespace Scada.Server.Engine
{
    partial class MainLogic
    {
        /// <summary>
        /// Входной канал
        /// </summary>
        private class InCnl : InCnlProps
        {
            /// <summary>
            /// Метод вычисления данных входного канала
            /// </summary>
            public Calculator.CalcCnlDataDelegate CalcCnlData;
        }

        /// <summary>
        /// Канал управления
        /// </summary>
        internal class CtrlCnl : CtrlCnlProps
        {
            /// <summary>
            /// Метод вычисления значения стандартной команды
            /// </summary>
            public Calculator.CalcCmdValDelegate CalcCmdVal;
            /// <summary>
            /// Метод вычисления данных бинарной команды
            /// </summary>
            public Calculator.CalcCmdDataDelegate CalcCmdData;

            /// <summary>
            /// Клонировать канал управления
            /// </summary>
            /// <remarks>Клонируются только свойства, используемые приложением</remarks>
            public CtrlCnl Clone()
            {
                return new CtrlCnl() 
                { 
                    CtrlCnlNum = this.CtrlCnlNum,
                    CmdTypeID = this.CmdTypeID,
                    ObjNum = this.ObjNum,
                    KPNum = this.KPNum, 
                    CmdNum = this.CmdNum,
                    FormulaUsed = this.FormulaUsed,
                    Formula = this.Formula,
                    EvEnabled = this.EvEnabled,
                    CalcCmdVal = this.CalcCmdVal,
                    CalcCmdData = this.CalcCmdData
                };
            }
        }

        /// <summary>
        /// Пользователь
        /// </summary>
        /// <remarks>Класс содержит только те свойства, которые используются приложением</remarks>
        internal class User
        {
            /// <summary>
            /// Получить или установить имя
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// Получить или установить пароль
            /// </summary>
            public string Password { get; set; }
            /// <summary>
            /// Получить или установить идентификатор роли
            /// </summary>
            public int RoleID { get; set; }

            /// <summary>
            /// Клонировать пользователя
            /// </summary>
            public User Clone()
            {
                return new User() { Name = this.Name, Password = this.Password, RoleID = this.RoleID };
            }
        }

        /// <summary>
        /// Кэш таблиц срезов
        /// </summary>
        private class SrezTableCache
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            private SrezTableCache()
            {
            }
            /// <summary>
            /// Конструктор
            /// </summary>
            public SrezTableCache(DateTime date)
            {
                AccessDT = DateTime.Now;
                Date = date;
                SrezTable = new SrezTable();
                SrezTableCopy = new SrezTable();
                SrezAdapter = new SrezAdapter();
                SrezCopyAdapter = new SrezAdapter();
            }

            /// <summary>
            /// Получить или установить дату и время последнего доступа к объекту
            /// </summary>
            public DateTime AccessDT { get; set; }
            /// <summary>
            /// Получить дату таблиц среза
            /// </summary>
            public DateTime Date { get; private set; }
            /// <summary>
            /// Получить таблицу срезов
            /// </summary>
            public SrezTable SrezTable { get; private set; }
            /// <summary>
            /// Получить таблицу копий срезов
            /// </summary>
            public SrezTable SrezTableCopy { get; private set; }
            /// <summary>
            /// Получить адаптер таблицы срезов
            /// </summary>
            public SrezAdapter SrezAdapter { get; private set; }
            /// <summary>
            /// Получить адаптер таблицы копий срезов
            /// </summary>
            public SrezAdapter SrezCopyAdapter { get; private set; }

            /// <summary>
            /// Заполнить таблицу срезов или таблицу копий срезов данного кэша
            /// </summary>
            public void FillSrezTable(bool copy = false)
            {
                if (copy)
                    FillSrezTable(SrezTableCopy, SrezCopyAdapter);
                else
                    FillSrezTable(SrezTable, SrezAdapter);
            }
            /// <summary>
            /// Заполнить таблицу срезов
            /// </summary>
            public static void FillSrezTable(SrezTable srezTable, SrezAdapter srezAdapter)
            {
                string fileName = srezAdapter.FileName;

                if (File.Exists(fileName))
                {
                    // определение времени последнего изменения файла таблицы срезов
                    DateTime fileModTime = File.GetLastWriteTime(fileName);

                    // загрузка данных, если файл был изменён
                    if (srezTable.FileModTime != fileModTime)
                    {
                        srezAdapter.Fill(srezTable);
                        srezTable.FileModTime = fileModTime;
                    }
                }
                else
                {
                    srezTable.Clear();
                }
            }
        }

        /// <summary>
        /// Данные для усреднения
        /// </summary>
        private struct AvgData
        {
            /// <summary>
            /// Сумма значений канала
            /// </summary>
            public double Sum { get; set; }
            /// <summary>
            /// Количество значений канала
            /// </summary>
            public int Cnt { get; set; }
        }
    }
}
