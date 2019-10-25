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
 * Summary  : Represents a start page
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
 * Modified : 2019
 */

using Scada.Admin.App.Code;
using Scada.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using WinControl;

namespace Scada.Admin.App.Forms
{
    /// <summary>
    /// Represents a start page.
    /// <para>Представляет стартовую страницу.</para>
    /// </summary>
    public partial class FrmStartPage : Form, IChildForm
    {
        /// <summary>
        /// List item representing a recently open project.
        /// <para>Элемент списка, представляющий недавно открытый проект.</para>
        /// </summary>
        private class ProjectItem
        {
            public ProjectItem(string path)
            {
                IsEmpty = false;
                Path = path;
                FileName = System.IO.Path.GetFileName(path);
                Directory = System.IO.Path.GetDirectoryName(path);
            }

            public bool IsEmpty { get; private set; }
            public string Path { get; set; }
            public string FileName { get; private set; }
            public string Directory { get; private set; }
        }

        private AppState appState;  // the application state
        private Font itemMainFont;  // the font of the item main text
        private Font itemHintFont;  // the font of the item hints
        private int hoverItemIndex; // the hovered item index


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmStartPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmStartPage(AppState appState)
            : this()
        {
            this.appState = appState ?? throw new ArgumentNullException("appState");
            itemMainFont = new Font(lbRecentProjects.Font.FontFamily, 10, FontStyle.Bold);
            itemHintFont = new Font(lbRecentProjects.Font.FontFamily, 8);
            hoverItemIndex = -1;
        }


        /// <summary>
        /// Gets or sets the object associated with the form.
        /// </summary>
        public ChildFormTag ChildFormTag { get; set; }


        /// <summary>
        /// Fills the list of recent projects.
        /// </summary>
        private void FillRecentProjectList()
        {
            if (appState.RecentProjects.Count > 0)
            {
                lbRecentProjects.Visible = true;
                lblNoRecentProjects.Visible = false;

                try
                {
                    lbRecentProjects.BeginUpdate();
                    lbRecentProjects.Items.Clear();

                    for (int i = appState.RecentProjects.Count - 1; i >= 0; i--)
                    {
                        lbRecentProjects.Items.Add(new ProjectItem(appState.RecentProjects[i]));
                    }
                }
                finally
                {
                    lbRecentProjects.EndUpdate();
                }
            }
            else
            {
                lbRecentProjects.Visible = false;
                lblNoRecentProjects.Visible = true;
            }
        }

        /// <summary>
        /// Opens the project.
        /// </summary>
        private void OpenProject(ProjectItem item)
        {
            if (!item.IsEmpty)
            {
                if (File.Exists(item.Path))
                {
                    ChildFormTag.SendMessage(this, AppMessage.OpenProject,
                        new Dictionary<string, object> { { "Path", item.Path } });
                }
                else
                {
                    ScadaUiUtils.ShowWarning(string.Format(CommonPhrases.NamedFileNotFound, item.Path));
                    RemoveProjectFromList(item);
                }
            }
        }

        /// <summary>
        /// Removes the project from the list of recent projects.
        /// </summary>
        private void RemoveProjectFromList(ProjectItem item)
        {
            lbRecentProjects.Items.Remove(item);
            appState.RemoveRecentProject(item.Path);

            if (lbRecentProjects.Items.Count == 0)
            {
                lbRecentProjects.Visible = false;
                lblNoRecentProjects.Visible = true;
            }
        }

        /// <summary>
        /// Saves the settings.
        /// </summary>
        public void Save()
        {
            // do nothing
        }


        private void FrmStartPage_Load(object sender, EventArgs e)
        {
            Translator.TranslateForm(this, GetType().FullName, null, cmsProjectList);
            FillRecentProjectList();
        }

        private void FrmStartPage_FormClosed(object sender, FormClosedEventArgs e)
        {
            itemMainFont.Dispose();
            itemHintFont.Dispose();
        }

        private void FrmStartPage_Resize(object sender, EventArgs e)
        {
            pnlContent.Height = Height;
            pnlContent.Left = Math.Max(0, (Width - pnlContent.Width) / 2);
        }


        private void cmsProjectList_Opening(object sender, CancelEventArgs e)
        {
            if (hoverItemIndex >= 0)
            {
                cmsProjectList.Tag = lbRecentProjects.Items[hoverItemIndex];
            }
            else
            {
                cmsProjectList.Tag = null;
                e.Cancel = true;
            }
        }

        private void miRemoveFromList_Click(object sender, EventArgs e)
        {
            if (cmsProjectList.Tag is ProjectItem item)
                RemoveProjectFromList(item);
        }

        private void miCopyPath_Click(object sender, EventArgs e)
        {
            if (cmsProjectList.Tag is ProjectItem item && !string.IsNullOrEmpty(item.Path))
                Clipboard.SetText(item.Path);
        }


        private void lbRecentProjects_MouseMove(object sender, MouseEventArgs e)
        {
            int prevIndex = hoverItemIndex;
            Point point = lbRecentProjects.PointToClient(Cursor.Position);
            hoverItemIndex = lbRecentProjects.IndexFromPoint(point);
            lbRecentProjects.Cursor = hoverItemIndex >= 0 ? Cursors.Hand : Cursors.Default;

            if (hoverItemIndex != prevIndex)
                lbRecentProjects.Invalidate();
        }

        private void lbRecentProjects_MouseLeave(object sender, EventArgs e)
        {
            if (hoverItemIndex != -1)
            {
                hoverItemIndex = -1;
                lbRecentProjects.Invalidate();
            }
        }

        private void lbRecentProjects_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index >= 0)
            {
                const int PaddingLeft = 5;
                ProjectItem item = (ProjectItem)lbRecentProjects.Items[e.Index];

                e.Graphics.FillRectangle(e.Index == hoverItemIndex ?
                    SystemBrushes.ControlLight : SystemBrushes.Window, e.Bounds);

                e.Graphics.DrawString(item.FileName, itemMainFont, SystemBrushes.WindowText,
                    e.Bounds.Left + PaddingLeft, e.Bounds.Top + 9);

                e.Graphics.DrawString(item.Directory, itemHintFont, SystemBrushes.GrayText,
                    e.Bounds.Left + PaddingLeft, e.Bounds.Top + 27);

                e.DrawFocusRectangle();
            }
        }

        private void lbRecentProjects_MouseClick(object sender, MouseEventArgs e)
        {
            int itemIndex = lbRecentProjects.IndexFromPoint(e.Location);

            if (itemIndex >= 0)
            {
                ProjectItem item = (ProjectItem)lbRecentProjects.Items[itemIndex];
                OpenProject(item);
            }
        }

        private void lbRecentProjects_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && lbRecentProjects.SelectedItem is ProjectItem item)
            {
                OpenProject(item);
            }
        }

        private void btnNewProject_Click(object sender, EventArgs e)
        {
            ChildFormTag.SendMessage(this, AppMessage.NewProject);
        }

        private void btnOpenProject_Click(object sender, EventArgs e)
        {
            ChildFormTag.SendMessage(this, AppMessage.OpenProject);
        }
    }
}
