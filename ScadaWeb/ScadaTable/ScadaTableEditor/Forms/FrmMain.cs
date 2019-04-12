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
 * Module   : Table Editor
 * Summary  : Main form of the application
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
 * Modified : 2019
 */

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
using Utils;

namespace Scada.Table.Editor.Forms
{
    /// <summary>
    /// Main form of the application.
    /// <para>Главная форма приложения.</para>
    /// </summary>
    public partial class FrmMain : Form
    {
        /// <summary>
        /// Short name of the application error log file.
        /// </summary>
        private const string ErrFileName = "ScadaTableEditor.err";

        private readonly string exeDir;  // the directory of the executable file
        private readonly string langDir; // the directory of language files
        private readonly Log errLog;     // the application error log


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmMain()
        {
            InitializeComponent();

            exeDir = Path.GetDirectoryName(Application.ExecutablePath);
            langDir = Path.Combine(exeDir, "Lang");
            errLog = new Log(Log.Formats.Full) { FileName = Path.Combine(exeDir, "Log", ErrFileName) } ;
        }


        /// <summary>
        /// Applies localization to the form.
        /// </summary>
        private void LocalizeForm()
        {
            if (!Localization.LoadDictionaries(langDir, "ScadaData", out string errMsg))
                errLog.WriteError(errMsg);

            bool tableDictLoaded = Localization.LoadDictionaries(langDir, "ScadaTable", out errMsg);
            if (!tableDictLoaded)
                errLog.WriteError(errMsg);

            CommonPhrases.Init();
            TablePhrases.Init();

            if (tableDictLoaded)
            {
                Translator.TranslateForm(this, GetType().FullName);
            }
        }


        private void FrmMain_Load(object sender, EventArgs e)
        {
            LocalizeForm();
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void FrmMain_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void miFileNew_Click(object sender, EventArgs e)
        {

        }

        private void miFileOpen_Click(object sender, EventArgs e)
        {

        }

        private void miFileSave_Click(object sender, EventArgs e)
        {

        }

        private void miFileSaveAs_Click(object sender, EventArgs e)
        {

        }

        private void miFileExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void miHelpAbout_Click(object sender, EventArgs e)
        {
            FrmAbout.ShowAbout(exeDir);
        }
    }
}
