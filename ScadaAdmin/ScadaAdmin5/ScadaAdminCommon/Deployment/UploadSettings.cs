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
 * Module   : ScadaAdminCommon
 * Summary  : Configuration upload settings
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
 * Modified : 2019
 */

using System.Collections.Generic;
using System.Xml;

namespace Scada.Admin.Deployment
{
    /// <summary>
    /// Configuration upload settings.
    /// <para>Настройки передачи конфигурации на сервер.</para>
    /// </summary>
    public class UploadSettings : TransferSettings
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public UploadSettings()
            : base()
        {
            RestartServer = true;
            RestartComm = true;
            ObjNums = new List<int>();
        }


        /// <summary>
        /// Gets or sets a value indicating whether to restart Server after upload is complete.
        /// </summary>
        public bool RestartServer { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to restart Communicator after upload is complete.
        /// </summary>
        public bool RestartComm { get; set; }

        /// <summary>
        /// Gets the object numbers to filter the uploaded configuration.
        /// </summary>
        public List<int> ObjNums { get; protected set; }


        /// <summary>
        /// Sets the object numbers.
        /// </summary>
        public void SetObjNums(ICollection<int> objNums)
        {
            ObjNums.Clear();

            if (objNums != null)
                ObjNums.AddRange(objNums);
        }
                
        /// <summary>
        /// Loads the settings from the XML node.
        /// </summary>
        public override void LoadFromXml(XmlNode xmlNode)
        {
            base.LoadFromXml(xmlNode);
            RestartServer = xmlNode.GetChildAsBool("RestartServer");
            RestartComm = xmlNode.GetChildAsBool("RestartComm");
            SetObjNums(RangeUtils.StrToRange(xmlNode.GetChildAsString("ObjNums"), true, true));
        }

        /// <summary>
        /// Saves the settings into the XML node.
        /// </summary>
        public override void SaveToXml(XmlElement xmlElem)
        {
            base.SaveToXml(xmlElem);
            xmlElem.AppendElem("RestartServer", RestartServer);
            xmlElem.AppendElem("RestartComm", RestartComm);
            xmlElem.AppendElem("ObjNums", RangeUtils.RangeToStr(ObjNums));
        }
    }
}
