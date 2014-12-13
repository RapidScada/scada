/*
 * Copyright 2014 Mikhail Shiryaev
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
 * Module   : KP
 * Summary  : The base class for device library user interface
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2006
 * Modified : 2012
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Scada.Comm.KP
{
    /// <summary>
    /// The base class for device library user interface
    /// <para>Родительский класс пользовательского интерфейса библиотеки КП</para>
    /// </summary>
    public abstract class KPView
    {
        /// <summary>
        /// Типы входных каналов
        /// </summary>
        public enum CnlType
        {
            /// <summary>
            /// Телесигнал
            /// </summary>
            TS = 1,
            /// <summary>
            /// Телеизмерение
            /// </summary>
            TI = 2,
            /// <summary>
            /// Дорасчётный ТИ
            /// </summary>
            TIDR = 3,
            /// <summary>
            /// Минутный ТИ
            /// </summary>
            TIDRM = 4,
            /// <summary>
            /// Часовой ТИ
            /// </summary>
            TIDRH = 5,
            /// <summary>
            /// Количество переключений
            /// </summary>
            SWCNT = 6,
            /// <summary>
            /// Дорасчетный ТС
            /// </summary>
            TSDR = 7,
            /// <summary>
            /// Минутный ТС
            /// </summary>
            TSDRM = 8,
            /// <summary>
            /// Часовой ТС
            /// </summary>
            TSDRH = 9
        }

        /// <summary>
        /// Свойства входного канала, необходимого для работы КП
        /// </summary>
        public class InCnlProps
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            public InCnlProps()
                : this("", CnlType.TS)
            {
            }
            /// <summary>
            /// Конструктор
            /// </summary>
            /// <param name="name">Наименование входного канала</param>
            /// <param name="cnlType">Тип входного канала</param>
            public InCnlProps(string name, CnlType cnlType)
            {
                Name = name;
                CnlType = cnlType;
                Signal = 0;
                FormulaUsed = false;
                Formula = "";
                Averaging = false;
                ParamName = "";
                ShowNumber = true;
                DecDigits = 3;
                UnitName = "";
                CtrlCnlProps = null;
                EvEnabled = false;
                EvSound = false;
                EvOnChange = false;
                EvOnUndef = false;
                LimLowCrash = double.NaN;
                LimLow = double.NaN;
                LimHigh = double.NaN;
                LimHighCrash = double.NaN;
            }

            /// <summary>
            /// Получить или установить наименование входного канала
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// Получить или установить тип входного канала
            /// </summary>
            public CnlType CnlType { get; set; }
            /// <summary>
            /// Получить или установить сигнал (номер параметра КП)
            /// </summary>
            public int Signal { get; set; }
            /// <summary>
            /// Получить или установить признак использования формулы
            /// </summary>
            public bool FormulaUsed { get; set; }
            /// <summary>
            /// Получить или установить формулу
            /// </summary>
            public string Formula { get; set; }
            /// <summary>
            /// Получить или установить признак усреднения
            /// </summary>
            public bool Averaging { get; set; }
            /// <summary>
            /// Получить или установить наименование параметра
            /// </summary>
            public string ParamName { get; set; }
            /// <summary>
            /// Получить или установить признак числового формата вывода значения канала
            /// </summary>
            public bool ShowNumber { get; set; }
            /// <summary>
            /// Получить или установить количество знаков дробной части формата вывода значения канала
            /// </summary>
            public int DecDigits { get; set; }
            /// <summary>
            /// Получить или установить наименование размерности
            /// </summary>
            public string UnitName { get; set; }
            /// <summary>
            /// Получить или установить свойства канала управления, привязанного к входному каналу
            /// </summary>
            public CtrlCnlProps CtrlCnlProps { get; set; }
            /// <summary>
            /// Получить или установить признак записи событий
            /// </summary>
            public bool EvEnabled { get; set; }
            /// <summary>
            /// Получить или установить признак звука события
            /// </summary>
            public bool EvSound { get; set; }
            /// <summary>
            /// Получить или установить признак события по изменению
            /// </summary>
            public bool EvOnChange { get; set; }
            /// <summary>
            /// Получить или установить признак события по неопределённому состоянию
            /// </summary>
            public bool EvOnUndef { get; set; }
            /// <summary>
            /// Получить или установить нижнюю аварийную границу
            /// </summary>
            public double LimLowCrash { get; set; }
            /// <summary>
            /// Получить или установить нижнюю границу
            /// </summary>
            public double LimLow { get; set; }
            /// <summary>
            /// Получить или установить верхнюю границу
            /// </summary>
            public double LimHigh { get; set; }
            /// <summary>
            /// Получить или установить верхнюю аварийную границу
            /// </summary>
            public double LimHighCrash { get; set; }
        }

        /// <summary>
        /// Свойства канала управления, необходимого для работы КП
        /// </summary>
        public class CtrlCnlProps
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            public CtrlCnlProps()
                : this("", KPLogic.CmdType.Standard)
            {
            }
            /// <summary>
            /// Конструктор
            /// </summary>
            /// <param name="name">Наименование канала управления</param>
            /// <param name="cmdType">Тип команды</param>
            public CtrlCnlProps(string name, KPLogic.CmdType cmdType)
            {
                Name = name;
                CmdType = cmdType;
                CmdNum = 0;
                CmdValName = "";
                FormulaUsed = false;
                Formula = "";
                EvEnabled = false;
            }

            /// <summary>
            /// Получить или установить наименование канала управления
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// Получить или установить тип команды
            /// </summary>
            public KPLogic.CmdType CmdType { get; set; }
            /// <summary>
            /// Получить или установить номер команды
            /// </summary>
            public int CmdNum { get; set; }
            /// <summary>
            /// Получить или установить наименование значений команды
            /// </summary>
            public string CmdValName { get; set; }
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
        }


        /// <summary>
        /// Количество каналов управления КП по умолчанию
        /// </summary>
        protected int defaultCtrlCnlCount;


        /// <summary>
        /// Конструктор для общей настройки библиотеки КП
        /// </summary>
        public KPView() : this(0)
        {
        }

        /// <summary>
        /// Конструктор для настройки конкретного КП
        /// </summary>
        /// <param name="number">Номер настраемого КП</param>
        public KPView(int number)
        {
            defaultCtrlCnlCount = -1;

            Number = number;
            ConfigDir = "";
            LangDir = "";
            CmdDir = "";
            CanShowProps = false;
            DefaultCnls = null;
            DefaultReqParams = new KPLogic.ReqParams(true);
        }


        /// <summary>
        /// Описание библиотеки КП
        /// </summary>
        public abstract string KPDescr
        {
            get;
        }

        /// <summary>
        /// Получить номер КП
        /// </summary>
        public int Number { get; protected set; }

        /// <summary>
        /// Получить или установить директорию конфигурации программы
        /// </summary>
        public string ConfigDir { get; set; }

        /// <summary>
        /// Получить или установить директорию языковых файлов
        /// </summary>
        public string LangDir { get; set; }

        /// <summary>
        /// Получить или установить директорию команд
        /// </summary>
        public string CmdDir { get; set; }

        /// <summary>
        /// Получить возможность отображения свойств КП
        /// </summary>
        public bool CanShowProps { get; protected set; }

        /// <summary>
        /// Получить список входных каналов КП по умолчанию, 
        /// а также привязанных к ним каналов управления (зависит только от библиотеки КП, не зависит от его номера)
        /// </summary>
        public List<InCnlProps> DefaultCnls { get; protected set; }

        /// <summary>
        /// Получить параметры опроса КП по умолчанию
        /// </summary>
        /// <remarks>Не должны зависеть от номера КП</remarks>
        public KPLogic.ReqParams DefaultReqParams { get; protected set; }

        /// <summary>
        /// Получить количество каналов управления КП по умолчанию
        /// </summary>
        public int DefaultCtrlCnlCount
        {
            get
            {
                if (defaultCtrlCnlCount < 0)
                {
                    defaultCtrlCnlCount = 0;
                    if (DefaultCnls != null)
                    {
                        foreach (InCnlProps inCnlProps in DefaultCnls)
                            if (inCnlProps.CtrlCnlProps != null)
                                defaultCtrlCnlCount++;
                    }
                }
                return defaultCtrlCnlCount;
            }
        }


        /// <summary>
        /// Отобразить свойства КП
        /// </summary>
        public virtual void ShowProps()
        {
        }
    }
}
