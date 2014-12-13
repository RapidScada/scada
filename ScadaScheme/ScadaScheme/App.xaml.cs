/*
 * Copyright 2014 Mikhail Shiryaev
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
 * Module   : SCADA-Scheme
 * Summary  : The Silverlight application class
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2012
 * Modified : 2012
 */

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Browser;

namespace Scada.Scheme
{
    /// <summary>
    /// The Silverlight application class
    /// <para>Класс Silverlight-приложения</para>
    /// </summary>
    public partial class App : Application
    {

        public App()
        {
            this.Startup += this.Application_Startup;
            this.Exit += this.Application_Exit;
            this.UnhandledException += this.Application_UnhandledException;

            InitializeComponent();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            MainPage mainPage = new MainPage();
            this.RootVisual = mainPage;

            // получение параметров работы приложения
            IDictionary<string, string> initParams = e.InitParams;
            if (initParams.ContainsKey("schemeSvcURI"))
                mainPage.SchemeSvcURI = initParams["schemeSvcURI"];

            int maxMsgSize;
            if (initParams.ContainsKey("maxMsgSize") && int.TryParse(initParams["maxMsgSize"], out maxMsgSize))
                mainPage.MaxMsgSize = maxMsgSize;

            IDictionary<string, string> queryString = HtmlPage.Document.QueryString;
            int viewSetIndex;
            if (queryString.ContainsKey("viewSet") && int.TryParse(queryString["viewSet"], out viewSetIndex))
                mainPage.ViewSetIndex = viewSetIndex;

            int viewIndex;
            if (queryString.ContainsKey("view") && int.TryParse(queryString["view"], out viewIndex))
                mainPage.ViewIndex = viewIndex;

            try { mainPage.DiagDate = new DateTime(int.Parse(queryString["year"]), 
                int.Parse(queryString["month"]), int.Parse(queryString["day"])); }
            catch { }

            bool editMode;
            string editModeStr;
            if (queryString.TryGetValue("editMode", out editModeStr) && bool.TryParse(editModeStr, out editMode))
                mainPage.EditMode = editMode;
        }

        private void Application_Exit(object sender, EventArgs e)
        {

        }

        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            // If the app is running outside of the debugger then report the exception using
            // the browser's exception mechanism. On IE this will display it a yellow alert 
            // icon in the status bar and Firefox will display a script error.
            if (!System.Diagnostics.Debugger.IsAttached)
            {

                // NOTE: This will allow the application to continue running after an exception has been thrown
                // but not handled. 
                // For production applications this error handling should be replaced with something that will 
                // report the error to the website and stop the application.
                e.Handled = true;
                Deployment.Current.Dispatcher.BeginInvoke(delegate { ReportErrorToDOM(e); });
            }
        }

        private void ReportErrorToDOM(ApplicationUnhandledExceptionEventArgs e)
        {
            try
            {
                string errorMsg = e.ExceptionObject.Message + e.ExceptionObject.StackTrace;
                errorMsg = errorMsg.Replace('"', '\'').Replace("\r\n", @"\n");

                HtmlPage.Window.Eval("throw new Error(\"Unhandled Error in Silverlight Application " + errorMsg + "\");");
            }
            catch (Exception)
            {
            }
        }
    }
}
