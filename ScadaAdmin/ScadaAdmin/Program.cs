/*
 * Copyright 2017 Mikhail Shiryaev
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
 * Module   : SCADA-Administrator
 * Summary  : The main entry point for the application
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2010
 * Modified : 2017
 */

using Scada;
using Scada.UI;
using System;
using System.Threading;
using System.Windows.Forms;

namespace ScadaAdmin
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ThreadException += Application_ThreadException;
            Application.Run(new FrmMain());
        }

        public static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            AppData.ErrLog.WriteException(e.Exception, CommonPhrases.UnhandledException);
            ScadaUiUtils.ShowError(CommonPhrases.UnhandledException + ":\r\n" + e.Exception.Message);
        }
    }
}