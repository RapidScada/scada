/*
 * Copyright 2021 Mikhail Shiryaev
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
 * Module   : ModDbExport
 * Summary  : Represents data trigger options
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

using System;
using System.Collections.Generic;
using System.Xml;

namespace Scada.Server.Modules.DbExport.Config
{
    /// <summary>
    /// Represents data trigger options.
    /// <para>Представляет параметры триггера на данные.</para>
    /// </summary>
    [Serializable]
    internal abstract class DataTriggerOptions : TriggerOptions
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DataTriggerOptions()
            : base()
        {
            SingleQuery = false;
        }


        /// <summary>
        /// Gets or sets a value indicating whether to execute a single SQL query according to the trigger.
        /// </summary>
        public bool SingleQuery { get; set; }


        /// <summary>
        /// Loads the options from the XML node.
        /// </summary>
        public override void LoadFromXml(XmlElement xmlElem)
        {
            base.LoadFromXml(xmlElem);
            SingleQuery = xmlElem.GetChildAsBool("SingleQuery");
        }

        /// <summary>
        /// Saves the options into the XML node.
        /// </summary>
        public override void SaveToXml(XmlElement xmlElem)
        {
            base.SaveToXml(xmlElem);
            xmlElem.AppendElem("SingleQuery", SingleQuery);
        }

        /// <summary>
        /// Gets the names of the available query parameters.
        /// </summary>
        public override List<string> GetParamNames()
        {
            List<string> paramNames = new List<string> { "dateTime" };
            paramNames.Add("kpNum");

            if (SingleQuery)
            {
                foreach (int cnlNum in CnlNums)
                {
                    string cnlNumStr = cnlNum.ToString();
                    paramNames.Add("val" + cnlNumStr);
                    paramNames.Add("stat" + cnlNumStr);
                }
            }
            else
            {
                paramNames.Add("cnlNum");
                paramNames.Add("val");
                paramNames.Add("stat");
            }

            return paramNames;
        }
    }
}
