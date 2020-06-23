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
 * Module   : KpDBImport
 * Summary  : DB connection settings
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2020
 */

using System;
using System.Xml;

namespace Scada.Comm.Devices.DbImport.Configuration
{
    /// <summary>
    /// DB connection settings.
    /// <para>Настройки соединения с БД.</para>
    /// </summary>
    internal class DbConnSettings
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DbConnSettings()
        {
            Server = "";
            Database = "";
            User = "";
            Password = "";
            ConnectionString = "";
        }


        /// <summary>
        /// Gets or sets the server host.
        /// </summary>
        public string Server { get; set; }

        /// <summary>
        /// Gets or sets the database name.
        /// </summary>
        public string Database { get; set; }

        /// <summary>
        /// Gets or sets the database username.
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// Gets or sets the database user password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        public string ConnectionString { get; set; }


        /// <summary>
        /// Loads the settings from the XML node.
        /// </summary>
        public void LoadFromXml(XmlNode xmlNode)
        {
            if (xmlNode == null)
                throw new ArgumentNullException("xmlNode");

            Server = xmlNode.GetChildAsString("Server");
            Database = xmlNode.GetChildAsString("Database");
            User = xmlNode.GetChildAsString("User");
            Password = ScadaUtils.Decrypt(xmlNode.GetChildAsString("Password"));
            ConnectionString = xmlNode.GetChildAsString("ConnectionString");
        }

        /// <summary>
        /// Saves the settings into the XML node.
        /// </summary>
        public void SaveToXml(XmlElement xmlElem)
        {
            if (xmlElem == null)
                throw new ArgumentNullException("xmlElem");

            xmlElem.AppendElem("Server", Server);
            xmlElem.AppendElem("Database", Database);
            xmlElem.AppendElem("User", User);
            xmlElem.AppendElem("Password", ScadaUtils.Encrypt(Password));
            xmlElem.AppendElem("ConnectionString", ConnectionString);
        }
    }
}
