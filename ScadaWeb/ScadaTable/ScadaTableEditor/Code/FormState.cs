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
 * Module   : Administrator
 * Summary  : State of the main form controls
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2019
 */

using Scada.Table.Editor.Forms;
using Scada.UI;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace Scada.Table.Editor.Code
{
    /// <summary>
    /// State of the main form controls.
    /// <para>Состояние элементов управления главной формы.</para>
    /// </summary>
    public class FormState
    {
        /// <summary>
        /// The default state file name.
        /// </summary>
        public const string DefFileName = "ScadaTableEditorState.xml";


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FormState()
        {
            SetToDefault();
        }


        /// <summary>
        /// Tests whether the form state is empty.
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return Width <= 0 && Height <= 0;
            }
        }

        /// <summary>
        /// Gets or sets the form horizontal position.
        /// </summary>
        public int Left { get; set; }

        /// <summary>
        /// Gets or sets the form vertical position.
        /// </summary>
        public int Top { get; set; }

        /// <summary>
        /// Gets or sets the form width.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the form height.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the form is maximized.
        /// </summary>
        public bool Maximized { get; set; }

        /// <summary>
        /// Gets or sets the explorer width.
        /// </summary>
        public int ExplorerWidth { get; set; }


        /// <summary>
        /// Sets the default values.
        /// </summary>
        private void SetToDefault()
        {
            Left = 0;
            Top = 0;
            Width = 0;
            Height = 0;
            Maximized = true;
            ExplorerWidth = 250;
        }

        /// <summary>
        /// Loads the form state from the specified file.
        /// </summary>
        public bool Load(string fileName, out string errMsg)
        {
            try
            {
                SetToDefault();

                if (File.Exists(fileName))
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(fileName);
                    XmlElement rootElem = xmlDoc.DocumentElement;

                    Left = rootElem.GetChildAsInt("Left");
                    Top = rootElem.GetChildAsInt("Top");
                    Width = rootElem.GetChildAsInt("Width");
                    Height = rootElem.GetChildAsInt("Height");
                    Maximized = rootElem.GetChildAsBool("Maximized");
                    ExplorerWidth = rootElem.GetChildAsInt("ExplorerWidth");
                }

                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = TablePhrases.LoadFormStateError + ":" + Environment.NewLine + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Saves the state to the specified file.
        /// </summary>
        public bool Save(string fileName, out string errMsg)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                XmlDeclaration xmlDecl = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
                xmlDoc.AppendChild(xmlDecl);

                XmlElement rootElem = xmlDoc.CreateElement("ScadaTableEditorState");
                xmlDoc.AppendChild(rootElem);

                rootElem.AppendElem("Left", Left);
                rootElem.AppendElem("Top", Top);
                rootElem.AppendElem("Width", Width);
                rootElem.AppendElem("Height", Height);
                rootElem.AppendElem("Maximized", Maximized);
                rootElem.AppendElem("ExplorerWidth", ExplorerWidth);

                xmlDoc.Save(fileName);
                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = TablePhrases.SaveFormStateError + ":" + Environment.NewLine + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Applies the state to the specified form.
        /// </summary>
        public void Apply(FrmMain form)
        {
            if (form == null)
                throw new ArgumentNullException("form");

            if (!IsEmpty && ScadaUiUtils.AreaIsVisible(Left, Top, Width, Height))
            {
                form.SetBounds(Left, Top, Width, Height);
                form.WindowState = Maximized ? FormWindowState.Maximized : FormWindowState.Normal;
                form.ExplorerWidth = ExplorerWidth;
            }
        }

        /// <summary>
        /// Retrieves the state from the specified form.
        /// </summary>
        public void Retrieve(FrmMain form)
        {
            if (form == null)
                throw new ArgumentNullException("form");

            Rectangle bounds = form.WindowState == FormWindowState.Normal ? form.Bounds : form.RestoreBounds;
            Left = bounds.Left;
            Top = bounds.Top;
            Width = bounds.Width;
            Height = bounds.Height;
            Maximized = form.WindowState == FormWindowState.Maximized;
            ExplorerWidth = form.ExplorerWidth;
        }
    }
}
