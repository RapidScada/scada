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
 * Module   : SCADA-Scheme Editor
 * Summary  : Main form of the application
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2017
 */

using System;
using System.Diagnostics;
using System.ServiceModel;
using System.Windows.Forms;

namespace Scada.Scheme.Editor
{
    /// <summary>
    /// Main form of the application
    /// <para>Главная форма приложения</para>
    /// </summary>
    public partial class FrmMain : Form
    {
        private ServiceHost schemeEditorSvcHost;


        /// <summary>
        /// Конструктор
        /// </summary>
        public FrmMain()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Process.Start("chrome");
            Process.Start("file:///D:/Misha/My%20progs/SCADA/Source/scada/ScadaWeb/ScadaScheme/ScadaSchemeEditor/bin/Debug/Web/page.html");
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            try
            {
                schemeEditorSvcHost = new ServiceHost(typeof(SchemeEditorSvc));
                ServiceBehaviorAttribute behavior =
                    schemeEditorSvcHost.Description.Behaviors.Find<ServiceBehaviorAttribute>();
                behavior.InstanceContextMode = InstanceContextMode.Single;
                behavior.UseSynchronizationContext = false;
                schemeEditorSvcHost.Open();

                MessageBox.Show("OK");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (schemeEditorSvcHost != null)
            {
                try { schemeEditorSvcHost.Close(); }
                catch { schemeEditorSvcHost.Abort(); }
                MessageBox.Show("Closed");
            }
        }

        private void FrmMain_MouseMove(object sender, MouseEventArgs e)
        {
            // активировать форму при наведении мыши
            if (ActiveForm != this)
                BringToFront();
        }

        private void btnHelpAbout_Click(object sender, EventArgs e)
        {
            // отображение формы о программе
            //FrmAbout.ShowAbout(appDirs.ExeDir, log);
        }
    }
}
