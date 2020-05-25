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
 * Module   : ScadaAgentConnector
 * Summary  : Specifies all settings required to make a connection to the Agent service
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2020
 */

using System;
using System.Xml;

namespace Scada.Agent.Connector
{
    /// <summary>
    /// Specifies all settings required to make a connection to the Agent service.
    /// <para>Задает все настройки, необходимые для подключения к службе Агента.</para>
    /// </summary>
    [Serializable]
    public class ConnectionSettings
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ConnectionSettings()
        {
            SetToDefault();
        }


        /// <summary>
        /// Gets or sets the computer name or IP address.
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Gets or sets the TCP port.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the system instance name.
        /// </summary>
        public string ScadaInstance { get; set; }

        /// <summary>
        /// Gets or sets the secret key.
        /// </summary>
        public byte[] SecretKey { get; set; }


        /// <summary>
        /// Sets the default values.
        /// </summary>
        protected void SetToDefault()
        {
            Host = "";
            Port = 10002;
            Username = "ScadaAdmin";
            Password = "";
            ScadaInstance = "Default";
            SecretKey = new byte[0];
        }


        /// <summary>
        /// Loads the settings from the XML node.
        /// </summary>
        public void LoadFromXml(XmlNode xmlNode)
        {
            if (xmlNode == null)
                throw new ArgumentNullException("xmlNode");

            Host = xmlNode.GetChildAsString("Host");
            Port = xmlNode.GetChildAsInt("Port", 10002);
            Username = xmlNode.GetChildAsString("Username", "admin");
            string encryptedPassword = xmlNode.GetChildAsString("Password");
            Password = ScadaUtils.Decrypt(encryptedPassword);
            ScadaInstance = xmlNode.GetChildAsString("ScadaInstance");
            SecretKey = ScadaUtils.HexToBytes(xmlNode.GetChildAsString("SecretKey"));
        }

        /// <summary>
        /// Saves the settings into the XML node.
        /// </summary>
        public void SaveToXml(XmlElement xmlElem)
        {
            if (xmlElem == null)
                throw new ArgumentNullException("xmlElem");

            xmlElem.AppendElem("Host", Host);
            xmlElem.AppendElem("Port", Port);
            xmlElem.AppendElem("Username", Username);
            xmlElem.AppendElem("Password", ScadaUtils.Encrypt(Password));
            xmlElem.AppendElem("ScadaInstance", ScadaInstance);
            xmlElem.AppendElem("SecretKey", ScadaUtils.BytesToHex(SecretKey));
        }

        /// <summary>
        /// Clones the object.
        /// </summary>
        public ConnectionSettings Clone()
        {
            return ScadaUtils.DeepClone(this);
        }

        /// <summary>
        /// Determines whether two specified objects have the same value.
        /// </summary>
        public static bool Equals(ConnectionSettings a, ConnectionSettings b)
        {
            if (a == b)
            {
                return true;
            }
            else if (a == null || b == null)
            {
                return false;
            }
            else
            {
                return
                    a.Host == b.Host &&
                    a.Port == b.Port &&
                    a.Username == b.Username &&
                    a.Password == b.Password &&
                    a.ScadaInstance == b.ScadaInstance &&
                    ScadaUtils.ArraysEqual(a.SecretKey, b.SecretKey);
            }
        }
    }
}
