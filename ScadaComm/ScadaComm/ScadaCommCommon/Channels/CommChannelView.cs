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
 * Summary  : The base class for communication channel user interface
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2015
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scada.Comm.Channels
{
    /// <summary>
    /// The base class for communication channel user interface
    /// <para>Родительский класс пользовательского интерфейса канала связи</para>
    /// </summary>
    public abstract class CommChannelView
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public CommChannelView()
        {
            LangDir = "";
            CanShowProps = false;
        }


        /// <summary>
        /// Получить наименование типа канала связи
        /// </summary>
        public abstract string TypeName { get; }

        /// <summary>
        /// Получить наименование канала связи
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Получить описание канала связи
        /// </summary>
        public abstract string Descr { get; }

        /// <summary>
        /// Получить или установить директорию языковых файлов
        /// </summary>
        public string LangDir { get; set; }

        /// <summary>
        /// Получить возможность отображения свойств канала связи
        /// </summary>
        public bool CanShowProps { get; protected set; }


        /// <summary>
        /// Построить строку информации о свойствах канала связи
        /// </summary>
        protected string BuildPropsInfo(SortedList<string, string> commCnlParams, 
            string[] paramNames, object[] defParamVals)
        {
            // проверка параметров метода
            if (commCnlParams == null)
                throw new ArgumentNullException("commCnlParams");
            if (paramNames == null)
                throw new ArgumentNullException("paramNames");
            if (defParamVals == null)
                throw new ArgumentNullException("defParamVals");
            if (paramNames.Length != defParamVals.Length)
                throw new ArgumentException("Lengths of paramNames and defParamVals must be equal.");

            // формирование строки вида "Param1 = val1, Param2 = val2"
            StringBuilder sbPropsInfo = new StringBuilder();
            int len = paramNames.Length;
            int last = len - 1;

            for (int i = 0; i < len; i++)
            {
                string paramName = paramNames[i];
                string paramVal;
                sbPropsInfo.Append(paramName).Append(" = ")
                    .Append(commCnlParams.TryGetValue(paramName, out paramVal) ? paramVal : defParamVals[i])
                    .Append(i < last ? Environment.NewLine : "");
            }

            return sbPropsInfo.ToString();
        }


        /// <summary>
        /// Установить параметры канала связи по умолчанию
        /// </summary>
        public virtual void SetCommCnlParamsToDefault(SortedList<string, string> commCnlParams)
        {

        }

        /// <summary>
        /// Отобразить свойства канала связи
        /// </summary>
        public virtual void ShowProps(SortedList<string, string> commCnlParams, out bool modified)
        {
            modified = false;
        }
        
        /// <summary>
        /// Получить информацию о свойствах канала связи
        /// </summary>
        public abstract string GetPropsInfo(SortedList<string, string> commCnlParams);

        /// <summary>
        /// Возвращает строковое представление текущего объекта
        /// </summary>
        public override string ToString()
        {
            return Name;
        }
    }
}
