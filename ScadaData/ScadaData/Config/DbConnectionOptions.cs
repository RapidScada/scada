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
 * Module   : ScadaData
 * Summary  : Represents database connection options
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2021
 */

using System;
using System.Xml;

namespace Scada.Config
{
    /// <summary>
    /// Represents database connection options.
    /// <para>Представляет параметры подключения к базе данных.</para>
    /// </summary>
    [Serializable]
    public class DbConnectionOptions
    {
        private string dbms;         // the database management system
        private KnownDBMS knownDBMS; // the one of known DBMSs


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DbConnectionOptions()
        {
            dbms = "";
            knownDBMS = KnownDBMS.Undefined;

            Name = "";
            Server = "";
            Database = "";
            Username = "";
            Password = "";
            ConnectionString = "";
        }


        /// <summary>
        /// Gets or sets the connection name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the database management system.
        /// </summary>
        public string DBMS
        {
            get
            {
                return dbms;
            }
            set
            {
                dbms = value;
                knownDBMS = Enum.TryParse(value, true, out KnownDBMS result) ? result : KnownDBMS.Undefined;
            }
        }

        /// <summary>
        /// Gets or sets the database management system, if it is one of the known DBMSs.
        /// </summary>
        public KnownDBMS KnownDBMS
        {
            get
            {
                return knownDBMS;
            }
            set
            {
                dbms = value.ToString();
                knownDBMS = value;
            }
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
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the database user password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        public string ConnectionString { get; set; }


        /// <summary>
        /// Loads the options from the XML node.
        /// </summary>
        public void LoadFromXml(XmlNode xmlNode)
        {
            if (xmlNode == null)
                throw new ArgumentNullException(nameof(xmlNode));

            Name = xmlNode.GetChildAsString("Name");
            DBMS = xmlNode.GetChildAsString("DBMS");
            Server = xmlNode.GetChildAsString("Server");
            Database = xmlNode.GetChildAsString("Database");
            Username = xmlNode.GetChildAsString("Username");
            Password = ScadaUtils.Decrypt(xmlNode.GetChildAsString("Password"));
            ConnectionString = xmlNode.GetChildAsString("ConnectionString");
        }

        /// <summary>
        /// Saves the options into the XML node.
        /// </summary>
        public void SaveToXml(XmlElement xmlElem)
        {
            if (xmlElem == null)
                throw new ArgumentNullException(nameof(xmlElem));

            xmlElem.AppendElem("Name", Name);
            xmlElem.AppendElem("DBMS", DBMS);
            xmlElem.AppendElem("Server", Server);
            xmlElem.AppendElem("Database", Database);
            xmlElem.AppendElem("Username", Username);
            xmlElem.AppendElem("Password", ScadaUtils.Encrypt(Password));
            xmlElem.AppendElem("ConnectionString", ConnectionString);
        }
    }
}
