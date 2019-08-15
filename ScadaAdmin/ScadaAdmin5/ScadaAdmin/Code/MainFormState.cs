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

using Scada.Admin.App.Forms;
using Scada.UI;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

namespace Scada.Admin.App.Code
{
    /// <summary>
    /// State of the main form controls.
    /// <para>Состояние элементов управления главной формы.</para>
    /// </summary>
    public class MainFormState
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public MainFormState()
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
        /// Loads the state from the XML node.
        /// </summary>
        public void LoadFromXml(XmlNode xmlNode)
        {
            if (xmlNode == null)
                throw new ArgumentNullException("xmlNode");

            Left = xmlNode.GetChildAsInt("Left");
            Top = xmlNode.GetChildAsInt("Top");
            Width = xmlNode.GetChildAsInt("Width");
            Height = xmlNode.GetChildAsInt("Height");
            Maximized = xmlNode.GetChildAsBool("Maximized");
            ExplorerWidth = xmlNode.GetChildAsInt("ExplorerWidth");
        }

        /// <summary>
        /// Saves the state into the XML node.
        /// </summary>
        public void SaveToXml(XmlElement xmlElem)
        {
            if (xmlElem == null)
                throw new ArgumentNullException("xmlElem");

            xmlElem.AppendElem("Left", Left);
            xmlElem.AppendElem("Top", Top);
            xmlElem.AppendElem("Width", Width);
            xmlElem.AppendElem("Height", Height);
            xmlElem.AppendElem("Maximized", Maximized);
            xmlElem.AppendElem("ExplorerWidth", ExplorerWidth);
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
            else
            {
                form.WindowState = FormWindowState.Maximized;
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
