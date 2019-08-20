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
 * Summary  : Represents the channel generation options
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
 * Modified : 2019
 */

using System;
using System.Xml;

namespace Scada.Admin.Config
{
    /// <summary>
    /// Represents the channel generation options.
    /// <para>Представляет параметры генерации каналов.</para>
    /// </summary>
    public class ChannelOptions
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ChannelOptions()
        {
            CnlMult = 100;
            CnlShift = 1;
            CnlGap = 10;
            PrependDeviceName = true;
        }


        /// <summary>
        /// Gets or sets the multiplicity of the first channel of a device.
        /// </summary>
        public int CnlMult { get; set; }

        /// <summary>
        /// Gets or sets the shift of the first channel of a device.
        /// </summary>
        public int CnlShift { get; set; }

        /// <summary>
        /// Gets or sets the gap between channel numbers of different devices.
        /// </summary>
        public int CnlGap { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to prepend a device name in channel names.
        /// </summary>
        public bool PrependDeviceName { get; set; }


        /// <summary>
        /// Loads the options from the XML node.
        /// </summary>
        public void LoadFromXml(XmlNode xmlNode)
        {
            if (xmlNode == null)
                throw new ArgumentNullException("xmlNode");

            foreach (XmlElement paramElem in xmlNode)
            {
                string name = paramElem.GetAttribute("name");
                string nameL = name.ToLowerInvariant();
                string val = paramElem.GetAttribute("value");

                try
                {
                    if (nameL == "cnlmult")
                        CnlMult = int.Parse(val);
                    else if (nameL == "cnlshift")
                        CnlShift = int.Parse(val);
                    else if (nameL == "cnlgap")
                        CnlGap = int.Parse(val);
                    else if (nameL == "prependdevicename")
                        PrependDeviceName = bool.Parse(val);
                }
                catch
                {
                    throw new Exception(string.Format(CommonPhrases.IncorrectXmlParamVal, name));
                }
            }
        }

        /// <summary>
        /// Saves the options into the XML node.
        /// </summary>
        public void SaveToXml(XmlElement xmlElem)
        {
            if (xmlElem == null)
                throw new ArgumentNullException("xmlElem");

            xmlElem.AppendParamElem("CnlMult", CnlMult,
                "Кратность первого канала устройства",
                "Multiplicity of the first channel of a device");
            xmlElem.AppendParamElem("CnlShift", CnlShift,
                "Смещение первого канала устройства",
                "Shift of the first channel of a device");
            xmlElem.AppendParamElem("CnlGap", CnlGap,
                "Промежуток между номерами каналов разных устройств",
                "Gap between channel numbers of different devices");
            xmlElem.AppendParamElem("PrependDeviceName", PrependDeviceName,
                "Добавлять наименование КП в имена каналов",
                "To prepend a device name in channel names");
        }
    }
}
