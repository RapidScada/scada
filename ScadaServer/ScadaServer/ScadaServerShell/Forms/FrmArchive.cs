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
 * Module   : Server Shell
 * Summary  : Form for displaying list of archive files
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
 * Modified : 2019
 */

using Scada.Server.Shell.Code;
using Scada.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scada.Server.Shell.Forms
{
    /// <summary>
    /// Form for displaying list of archive files.
    /// <para>Форма для отображения списка архивных файлов.</para>
    /// </summary>
    public partial class FrmArchive : Form
    {
        /// <summary>
        /// List item representing a file.
        /// <para>Элемент списка, представляющий файл.</para>
        /// </summary>
        private class FileItem
        {
            public string FileName { get; set; }
            public string FilePath { get; set; }

            public override string ToString()
            {
                return FileName;
            }
        }

        private readonly Settings settings; // the application settings
        private readonly ServerEnvironment environment; // the application environment
        private ArcType arcType; // the archive type


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmArchive()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmArchive(Settings settings, ServerEnvironment environment, ArcType arcType)
            : this()
        {
            this.settings = settings ?? throw new ArgumentNullException("settings");
            this.environment = environment ?? throw new ArgumentNullException("environment");
            this.arcType = arcType;
        }


        /// <summary>
        /// Sets the form title.
        /// </summary>
        private void SetFormTitle()
        {
            switch (arcType)
            {
                case ArcType.CurData:
                    Text = ServerShellPhrases.CurDataTitle;
                    break;
                case ArcType.MinData:
                    Text = ServerShellPhrases.MinDataTitle;
                    break;
                case ArcType.HourData:
                    Text = ServerShellPhrases.HourDataTitle;
                    break;
                case ArcType.Events:
                    Text = ServerShellPhrases.EventsTitle;
                    break;
            }
        }

        /// <summary>
        /// Disables the form controls.
        /// </summary>
        private void DisableControls()
        {
            btnView.Enabled = false;
            btnEdit.Enabled = false;
            cbDataKind.Enabled = false;
            txtDir.Enabled = false;
            lbFiles.Enabled = false;
        }

        /// <summary>
        /// Gets the directory of the required archive files.
        /// </summary>
        private string GetDirectory()
        {
            string arcDir = cbDataKind.SelectedIndex == 0 ? settings.ArcDir : settings.ArcCopyDir;

            switch (arcType)
            {
                case ArcType.CurData:
                    return Path.Combine(arcDir, "Cur");
                case ArcType.MinData:
                    return Path.Combine(arcDir, "Min");
                case ArcType.HourData:
                    return Path.Combine(arcDir, "Hour");
                case ArcType.Events:
                    return Path.Combine(arcDir, "Events");
                default:
                    return "";
            }
        }

        /// <summary>
        /// Fills the list of files.
        /// </summary>
        private void FillFileList()
        {
            try
            {
                string dir = GetDirectory();
                txtDir.Text = ScadaUtils.NormalDir(dir);
                lbFiles.BeginUpdate();
                lbFiles.Items.Clear();

                if (Directory.Exists(dir))
                {
                    DirectoryInfo dirInfo = new DirectoryInfo(dir);
                    FileInfo[] fileInfoArr = dirInfo.GetFiles("*.dat", SearchOption.TopDirectoryOnly);

                    foreach (FileInfo fileInfo in fileInfoArr)
                    {
                        lbFiles.Items.Add(new FileItem()
                        {
                            FileName = fileInfo.Name,
                            FilePath = fileInfo.FullName
                        });
                    }
                }

                if (lbFiles.Items.Count > 0)
                {
                    lbFiles.SelectedIndex = 0;
                    btnView.Enabled = true;
                    btnEdit.Enabled = true;
                }
                else
                {
                    btnView.Enabled = false;
                    btnEdit.Enabled = false;
                }
            }
            finally
            {
                lbFiles.EndUpdate();
            }
        }

        /// <summary>
        /// Opens the selected file.
        /// </summary>
        private void OpenFile(bool allowEdit)
        {
            if (lbFiles.SelectedItem is FileItem fileItem)
            {
                if (arcType == ArcType.Events)
                {
                    new FrmEventTable(environment.ErrLog)
                    {
                        FileName = fileItem.FilePath,
                        AllowEdit = allowEdit
                    }
                    .ShowDialog();
                }
                else
                {
                    new FrmSnapshotTable(environment.ErrLog)
                    {
                        FileName = fileItem.FilePath,
                        AllowEdit = allowEdit
                    }
                    .ShowDialog();
                }
            }
        }


        private void FrmArchive_Load(object sender, EventArgs e)
        {
            Translator.TranslateForm(this, GetType().FullName);
            SetFormTitle();

            if (environment.AgentClient == null)
            {
                DisableControls();
                pnlWarning.Visible = true;
                lblWarning.Text = ServerShellPhrases.SetProfile;
            }
            else if (environment.AgentClient.IsLocal)
            {
                cbDataKind.SelectedIndex = 0;
            }
            else
            {
                DisableControls();
                pnlWarning.Visible = true;
                lblWarning.Text = ServerShellPhrases.ArcLocal;
            }
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            OpenFile(false);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            OpenFile(true);
        }

        private void cbDataKind_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillFileList();
        }

        private void lbFiles_DoubleClick(object sender, EventArgs e)
        {
            OpenFile(false);
        }

        private void lbFiles_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                OpenFile(false);
        }
    }
}
