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
 * Module   : PlgChart
 * Summary  : Minute data report builder
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

using Scada.Client;
using System;
using Utils.Report;

namespace Scada.Web.Plugins.Chart
{
    /// <summary>
    /// Minute data report builder
    /// <para>Формирует отчёт по минутным данным</para>
    /// </summary>
    public class MinDataRepBuilder : ExcelRepBuilder
    {
        private DataAccess dataAccess;       // объект для доступа к данным


        /// <summary>
        /// Конструктор
        /// </summary>
        public MinDataRepBuilder(DataAccess dataAccess)
            : base()
        {
            if (dataAccess == null)
                throw new ArgumentNullException("dataAccess");

        }
        
        
        /// <summary>
        /// Получить имя отчёта
        /// </summary>
        public override string RepName
        {
            get
            {
                return Localization.UseRussian ?
                    "Минутные данные" :
                    "Minute data";
            }
        }

        /// <summary>
        /// Получить имя файла шаблона
        /// </summary>
        public override string TemplateFileName
        {
            get
            {
                return "MinDataRep.xml";
            }
        }
    }
}