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
 * Module   : ScadaCommCommon
 * Summary  : The base class for device library user interface
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2006
 * Modified : 2015
 */

using Scada.Data;
using System.Collections.Generic;

namespace Scada.Comm.Devices
{
    /// <summary>
    /// The base class for device library user interface
    /// <para>Родительский класс пользовательского интерфейса библиотеки КП</para>
    /// </summary>
    public abstract class KPView
    {
        /// <summary>
        /// Прототип входного канала
        /// </summary>
        public class InCnlPrototype : InCnlProps
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            public InCnlPrototype()
                : base()
            {
            }
            /// <summary>
            /// Конструктор
            /// </summary>
            public InCnlPrototype(string cnlName, int cnlTypeID)
                : base(0, cnlName, cnlTypeID)
            {
                CtrlCnlProps = null;
            }

            /// <summary>
            /// Получить или установить ссылку на прототип канала управления, связанного со входным каналом
            /// </summary>
            public CtrlCnlPrototype CtrlCnlProps { get; set; }
        }

        /// <summary>
        /// Прототип канала управления
        /// </summary>
        public class CtrlCnlPrototype : CtrlCnlProps
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            public CtrlCnlPrototype()
                : base()
            {
            }
            /// <summary>
            /// Конструктор
            /// </summary>
            public CtrlCnlPrototype(string ctrlCnlName, int cmdTypeID)
                : base(0, ctrlCnlName, cmdTypeID)
            {
            }
        }

        /// <summary>
        /// Прототипы каналов КП
        /// </summary>
        public class KPCnlPrototypes
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            public KPCnlPrototypes()
            {
                InCnls = new List<InCnlPrototype>();
                CtrCnls = new List<CtrlCnlPrototype>();
            }

            /// <summary>
            /// Получить прототипы входных каналов
            /// </summary>
            public List<InCnlPrototype> InCnls { get; protected set; }
            /// <summary>
            /// Получить прототипы каналов управления
            /// </summary>
            public List<CtrlCnlPrototype> CtrCnls { get; protected set; }
        }


        /// <summary>
        /// Конструктор для общей настройки библиотеки КП
        /// </summary>
        public KPView() 
            : this(0)
        {
        }

        /// <summary>
        /// Конструктор для настройки конкретного КП
        /// </summary>
        /// <param name="number">Номер настраемого КП</param>
        public KPView(int number)
        {
            Number = number;
            ConfigDir = "";
            LangDir = "";
            LogDir = "";
            CmdDir = "";
            CmdLine = "";
            CanShowProps = false;
        }


        /// <summary>
        /// Описание библиотеки КП
        /// </summary>
        public abstract string KPDescr { get; }

        /// <summary>
        /// Получить прототипы каналов КП по умолчанию
        /// </summary>
        public virtual KPCnlPrototypes DefaultCnls
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// Получить параметры опроса КП по умолчанию
        /// </summary>
        public virtual KPReqParams DefaultReqParams
        {
            get
            {
                return KPReqParams.Default;
            }
        }


        /// <summary>
        /// Получить номер КП
        /// </summary>
        public int Number { get; private set; }

        /// <summary>
        /// Получить или установить директорию конфигурации программы
        /// </summary>
        public string ConfigDir { get; set; }

        /// <summary>
        /// Получить или установить директорию языковых файлов
        /// </summary>
        public string LangDir { get; set; }

        /// <summary>
        /// Получить или установить директорию файлов журналов программы
        /// </summary>
        public string LogDir { get; set; }

        /// <summary>
        /// Получить или установить директорию команд
        /// </summary>
        public string CmdDir { get; set; }

        /// <summary>
        /// Получить или установить командную строку
        /// </summary>
        public string CmdLine { get; set; }

        /// <summary>
        /// Получить возможность отображения свойств КП
        /// </summary>
        public bool CanShowProps { get; protected set; }


        /// <summary>
        /// Отобразить свойства КП
        /// </summary>
        public virtual void ShowProps()
        {
        }
    }
}