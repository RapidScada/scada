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
 * Summary  : Administrator settings
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2019
 */

using System;
using System.IO;
using System.Xml;

namespace Scada.Admin
{
    /// <summary>
    /// Administrator settings.
    /// <para>Настройки Администратора.</para>
    /// </summary>
    public class AdminSettings
    {
        /// <summary>
        /// The default settings file name.
        /// </summary>
        public const string DefFileName = "ScadaAdminConfig.xml";


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public AdminSettings()
        {
            SetToDefault();
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
        /// Sets the default values.
        /// </summary>
        private void SetToDefault()
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
                SchemeEditorPath = "";
                TableEditorPath = "";
                TextEditorPath = "";
            }

            CnlMult = 100;
            CnlShift = 1;
            CnlGap = 10;
        }

        /// <summary>
        /// Loads the settings from the specified file.
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

                // load path options
                if (rootElem.SelectSingleNode("PathOptions") is XmlNode pathOptionsNode)
                {
                    foreach (XmlElement paramElem in pathOptionsNode)
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

                // load channel numbering options
                if (rootElem.SelectSingleNode("CnlNumOptions") is XmlNode cnlNumOptionsNode)
                {
                    foreach (XmlElement paramElem in cnlNumOptionsNode)
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
                        }
                        catch
                        {
                            throw new Exception(string.Format(CommonPhrases.IncorrectXmlParamVal, name));
                        }
                    }
                }

                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = CommonPhrases.LoadAppSettingsError + ": " + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Saves the settings to the specified file.
        /// </summary>
        public bool Save(string fileName, out string errMsg)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                XmlDeclaration xmlDecl = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
                xmlDoc.AppendChild(xmlDecl);

                XmlElement rootElem = xmlDoc.CreateElement("ScadaAdminConfig");
                xmlDoc.AppendChild(rootElem);

                // save path options
                XmlElement pathOptionsElem = rootElem.AppendElem("PathOptions");
                pathOptionsElem.AppendParamElem("ServerDir", ServerDir, 
                    "Директория Сервера", 
                    "Server directory");
                pathOptionsElem.AppendParamElem("CommDir", CommDir, 
                    "Директория Коммуникатора", 
                    "Communicator directory");
                pathOptionsElem.AppendParamElem("SchemeEditorPath", SchemeEditorPath,
                    "Полное имя файла редактора схем",
                    "Full file name of a scheme editor");
                pathOptionsElem.AppendParamElem("TableEditorPath", TableEditorPath,
                    "Полное имя файла редактора таблиц",
                    "Full file name of a table editor");
                pathOptionsElem.AppendParamElem("TextEditorPath", TextEditorPath,
                    "Полное имя файла текстового редактора",
                    "Full file name of a text editor");

                // save channel numbering options
                XmlElement cnlNumOptionsElem = rootElem.AppendElem("CnlNumOptions");
                pathOptionsElem.AppendParamElem("CnlMult", CnlMult,
                    "Кратность первого канала устройства", 
                    "Multiplicity of the first channel of a device");
                pathOptionsElem.AppendParamElem("CnlShift", CnlShift,
                    "Смещение первого канала устройства", 
                    "Shift of the first channel of a device");
                pathOptionsElem.AppendParamElem("CnlGap", CnlGap,
                    "Промежуток между номерами каналов разных устройств", 
                    "Gap between channel numbers of different devices");

                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = CommonPhrases.SaveAppSettingsError + ": " + ex.Message;
                return false;
            }
        }
    }
}
