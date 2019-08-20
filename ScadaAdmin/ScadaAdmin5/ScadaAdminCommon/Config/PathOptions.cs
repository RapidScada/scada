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
 * Summary  : Represents the location options of external applications
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
    /// Represents the location options of external applications.
    /// <para>Представляет параметры расположения внешних приложений.</para>
    /// </summary>
    public class PathOptions
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public PathOptions()
        {
            if (ScadaUtils.IsRunningOnWin)
            {
                ServerDir = @"C:\SCADA\ScadaServer\";
                CommDir = @"C:\SCADA\ScadaComm\";
                SchemeEditorPath = @"C:\SCADA\ScadaSchemeEditor\ScadaSchemeEditor.exe";
                TableEditorPath = @"C:\SCADA\ScadaTableEditor\ScadaTableEditor.exe";
                TextEditorPath = "";
            }
            else
            {
                ServerDir = "/opt/scada/ScadaServer/";
                CommDir = "/opt/scada/ScadaComm/";
                SchemeEditorPath = "/opt/scada/ScadaSchemeEditor/ScadaSchemeEditor.exe";
                TableEditorPath = "/opt/scada/ScadaTableEditor/ScadaTableEditor.exe";
                TextEditorPath = "";
            }
        }


        /// <summary>
        /// Gets or sets the directory of the Server application.
        /// </summary>
        public string ServerDir { get; set; }

        /// <summary>
        /// Gets or sets the directory of the Communicator application.
        /// </summary>
        public string CommDir { get; set; }

        /// <summary>
        /// Gets or sets the full file name of a scheme editor.
        /// </summary>
        public string SchemeEditorPath { get; set; }

        /// <summary>
        /// Gets or sets the full file name of a table editor.
        /// </summary>
        public string TableEditorPath { get; set; }

        /// <summary>
        /// Gets or sets the full file name of a text editor.
        /// </summary>
        public string TextEditorPath { get; set; }
        
        
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

                if (nameL == "serverdir")
                    ServerDir = ScadaUtils.NormalDir(val);
                else if (nameL == "commdir")
                    CommDir = ScadaUtils.NormalDir(val);
                else if (nameL == "schemeeditorpath")
                    SchemeEditorPath = val;
                else if (nameL == "tableeditorpath")
                    TableEditorPath = val;
                else if (nameL == "texteditorpath")
                    TextEditorPath = val;
            }
        }

        /// <summary>
        /// Saves the options into the XML node.
        /// </summary>
        public void SaveToXml(XmlElement xmlElem)
        {
            if (xmlElem == null)
                throw new ArgumentNullException("xmlElem");

            xmlElem.AppendParamElem("ServerDir", ServerDir,
                "Директория Сервера",
                "Server directory");
            xmlElem.AppendParamElem("CommDir", CommDir,
                "Директория Коммуникатора",
                "Communicator directory");
            xmlElem.AppendParamElem("SchemeEditorPath", SchemeEditorPath,
                "Полное имя файла редактора схем",
                "Full file name of a scheme editor");
            xmlElem.AppendParamElem("TableEditorPath", TableEditorPath,
                "Полное имя файла редактора таблиц",
                "Full file name of a table editor");
            xmlElem.AppendParamElem("TextEditorPath", TextEditorPath,
                "Полное имя файла текстового редактора",
                "Full file name of a text editor");
        }
    }
}
