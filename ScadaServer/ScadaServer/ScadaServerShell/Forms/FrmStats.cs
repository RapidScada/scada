/*
 * Copyright 2018 Mikhail Shiryaev
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
 * Summary  : Form for displaying Server stats
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using Scada.Server.Shell.Code;
using Scada.UI;
using System;
using System.Windows.Forms;

namespace Scada.Server.Shell.Forms
{
    /// <summary>
    /// Form for displaying Server stats.
    /// <para>Форма для отображения статистики Сервера.</para>
    /// </summary>
    public partial class FrmStats : Form
    {
        private readonly ServerEnvironment environment; // the application environment
        private LogBox stateBox; // object to refresh state
        private LogBox logBox;   // object to refresh log


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmStats()
        {
            InitializeComponent();
        }
        
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmStats(ServerEnvironment environment)
            : this()
        {
            this.environment = environment ?? throw new ArgumentNullException("environment");
            stateBox = new LogBox(rtbState);
            logBox = new LogBox(rtbLog);
        }


        private void FrmStats_Load(object sender, EventArgs e)
        {
            Translator.TranslateForm(this, "Scada.Server.Shell.Forms.FrmStats");
        }

        private void tmrRefresh_Tick(object sender, EventArgs e)
        {

        }
    }
}
