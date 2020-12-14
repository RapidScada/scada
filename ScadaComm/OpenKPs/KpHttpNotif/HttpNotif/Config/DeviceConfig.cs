/*
 * Copyright 2020 Mikhail Shiryaev
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
 * Module   : KpHttpNotif
 * Summary  : Represents a device configuration
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Scada.Comm.Devices.HttpNotif.Config
{
    /// <summary>
    /// Represents a device configuration.
    /// <para>Представляет конфигурацию КП.</para>
    /// </summary>
    internal class DeviceConfig
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DeviceConfig()
        {
            SetToDefault();
        }


        /// <summary>
        /// Gets or sets the request URI.
        /// </summary>
        public string Uri { get; set; }

        /// <summary>
        /// Gets or sets the request HTTP method.
        /// </summary>
        public RequestMethod Method { get; set; }

        /// <summary>
        /// Gets the request headers.
        /// </summary>
        public List<Header> Headers { get; private set; }

        /// <summary>
        /// Gets or sets the contents of the HTTP message.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets the content type.
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Gets or sets the escaping method for content parameters.
        /// </summary>
        public EscapingMethod ContentEscaping { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether URI and content can include parameters.
        /// </summary>
        public bool ParamEnabled { get; set; }

        /// <summary>
        /// Gets or sets the character that marks the beginning of a parameter.
        /// </summary>
        public char ParamBegin { get; set; }

        /// <summary>
        /// Gets or sets the character that marks the end of a parameter.
        /// </summary>
        public char ParamEnd { get; set; }


        /// <summary>
        /// Sets the default values.
        /// </summary>
        private void SetToDefault()
        {
            Uri = "";
            Method = RequestMethod.Get;
            Headers = new List<Header>();
            Content = "";
            ContentType = "";
            ContentEscaping = EscapingMethod.None;
            ParamEnabled = true;
            ParamBegin = ParamString.DefaultParamBegin;
            ParamEnd = ParamString.DefaultParamEnd;
        }

        /// <summary>
        /// Sets the character that marks the beginning of a parameter.
        /// </summary>
        public void SetParamBegin(string s)
        {
            ParamBegin = string.IsNullOrEmpty(s) ? ParamString.DefaultParamBegin : s[0];
        }

        /// <summary>
        /// Sets the character that marks the end of a parameter.
        /// </summary>
        public void SetParamEnd(string s)
        {
            ParamEnd = string.IsNullOrEmpty(s) ? ParamString.DefaultParamEnd : s[0];
        }

        /// <summary>
        /// Loads the configuration from the specified file.
        /// </summary>
        public bool Load(string fileName, out string errMsg)
        {
            try
            {
                SetToDefault();

                if (!File.Exists(fileName))
                    throw new FileNotFoundException(string.Format(CommonPhrases.NamedFileNotFound, fileName));

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(fileName);
                XmlElement rootElem = xmlDoc.DocumentElement;

                Uri = rootElem.GetChildAsString("Uri");
                Method = rootElem.GetChildAsEnum("Method", RequestMethod.Get);

                if (rootElem.SelectSingleNode("Headers") is XmlNode headersNode)
                {
                    foreach (XmlElement headerElem in headersNode.SelectNodes("Header"))
                    {
                        Headers.Add(new Header
                        {
                            Name = headerElem.GetAttrAsString("name"),
                            Value = headerElem.GetAttrAsString("value")
                        });
                    }
                }

                Content = rootElem.GetChildAsString("Content");
                ContentType = rootElem.GetChildAsString("ContentType");
                ContentEscaping = rootElem.GetChildAsEnum("ContentEscaping", EscapingMethod.None);
                ParamEnabled = rootElem.GetChildAsBool("ParamEnabled", true);
                SetParamBegin(rootElem.GetChildAsString("ParamBegin"));
                SetParamEnd(rootElem.GetChildAsString("ParamEnd"));

                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = CommPhrases.LoadKpSettingsError + ": " + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Saves the configuration to the specified file.
        /// </summary>
        public bool Save(string fileName, out string errMsg)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                XmlDeclaration xmlDecl = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
                xmlDoc.AppendChild(xmlDecl);

                XmlElement rootElem = xmlDoc.CreateElement("KpHttpNotif");
                xmlDoc.AppendChild(rootElem);

                rootElem.AppendElem("Uri", Uri);
                rootElem.AppendElem("Method", Method);
                XmlElement headersElem = rootElem.AppendElem("Headers");

                foreach (Header header in Headers)
                {
                    XmlElement headerElem = headersElem.AppendElem("Header");
                    headerElem.SetAttribute("name", header.Name);
                    headerElem.SetAttribute("value", header.Value);
                }

                rootElem.AppendElem("Content", Content);
                rootElem.AppendElem("ContentType", ContentType);
                rootElem.AppendElem("ContentEscaping", ContentEscaping);
                rootElem.AppendElem("ParamEnabled", ParamEnabled);
                rootElem.AppendElem("ParamBegin", ParamBegin);
                rootElem.AppendElem("ParamEnd", ParamEnd);

                xmlDoc.Save(fileName);
                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = CommPhrases.SaveKpSettingsError + ": " + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Gets the configuration file name.
        /// </summary>
        public static string GetFileName(string configDir, int kpNum)
        {
            return Path.Combine(configDir, "KpHttpNotif_" + CommUtils.AddZeros(kpNum, 3) + ".xml");
        }
    }
}
