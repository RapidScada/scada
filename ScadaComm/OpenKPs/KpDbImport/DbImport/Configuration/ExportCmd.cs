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
 * Module   : KpDBImport
 * Summary  : Configuration of a data export command
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
 * Modified : 2019
 */

using System;
using System.Xml;

namespace Scada.Comm.Devices.DbImport.Configuration
{
    /// <summary>
    /// Configuration of a data export command.
    /// <para>Конфигурация команды экспорта данных.</para>
    /// </summary>
    internal class ExportCmd : IComparable<ExportCmd>
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ExportCmd()
        {
            CmdNum = 1;
            Name = "";
            Query = "";
        }


        /// <summary>
        /// Gets or sets the command number.
        /// </summary>
        public int CmdNum { get; set; }

        /// <summary>
        /// Gets or sets the command name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the SQL-query of the command.
        /// </summary>
        public string Query { get; set; }


        /// <summary>
        /// Loads the command from the XML node.
        /// </summary>
        public void LoadFromXml(XmlNode xmlNode)
        {
            if (xmlNode == null)
                throw new ArgumentNullException("xmlNode");

            CmdNum = xmlNode.GetChildAsInt("CmdNum");
            Name = xmlNode.GetChildAsString("Name");
            Query = xmlNode.GetChildAsString("Query");
        }

        /// <summary>
        /// Saves the command into the XML node.
        /// </summary>
        public void SaveToXml(XmlElement xmlElem)
        {
            if (xmlElem == null)
                throw new ArgumentNullException("xmlElem");

            xmlElem.AppendElem("CmdNum", CmdNum);
            xmlElem.AppendElem("Name", Name);
            xmlElem.AppendElem("Query", Query);
        }

        /// <summary>
        /// Compares the current instance with another object of the same type.
        /// </summary>
        public int CompareTo(ExportCmd other)
        {
            return CmdNum.CompareTo(other.CmdNum);
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        public override string ToString()
        {
            return string.Format("[{0}] {1}", CmdNum, Name);
        }
    }
}
