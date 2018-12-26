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
 * Module   : ScadaAgentTest
 * Summary  : Simple application for testing Agent
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using Scada.Agent.Engine;
using Scada.Agent.Test.AgentSvcRef;
using System;
using System.IO;
using System.Windows.Forms;

namespace Scada.Agent.Test
{
    /// <summary>
    /// Simple application for testing Agent
    /// <para>Простое приложение для тестирования Агента</para>
    /// </summary>
    public partial class FrmMain : Form
    {
        private const string SecretKey = "5ABF5A7FD01752A2F1DFD21370B96EA462B0AE5C66A64F8901C9E1E2A06E40F1";
        private const string DefConfigArc = @"C:\SCADA\config.zip";

        private long sessionID = 0;


        public FrmMain()
        {
            InitializeComponent();
        }


        private void btnCreateSession_Click(object sender, EventArgs e)
        {
            AgentSvcClient client = new AgentSvcClient();

            try
            {
                if (client.CreateSession(out sessionID))
                {
                    MessageBox.Show("Session created: " + sessionID);

                    string encPwd = CryptoUtils.EncryptPassword("12345", sessionID, ScadaUtils.HexToBytes(SecretKey));

                    if (client.Login(sessionID, "admin", encPwd, "Default", out string errMsg))
                        MessageBox.Show("Logged on.");
                    else
                        MessageBox.Show(errMsg);
                }
                else
                {
                    MessageBox.Show("Unable to create session.");
                }
            }
            finally
            {
                client.Close();
            }
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            AgentSvcClient client = new AgentSvcClient();

            try
            {
                Stream stream = client.DownloadConfig(sessionID, new ConfigOptions());

                if (stream == null)
                {
                    MessageBox.Show("Download stream is null.");
                }
                else
                {
                    DateTime t0 = DateTime.UtcNow;
                    byte[] buf = new byte[1024];

                    using (FileStream fileStream = 
                        File.Open(DefConfigArc, FileMode.Create, FileAccess.Write, FileShare.Read))
                    {
                        stream.CopyTo(fileStream);
                    }

                    stream.Dispose();
                    MessageBox.Show("Done in " + (int)(DateTime.UtcNow - t0).TotalMilliseconds + " ms");
                }
            }
            finally
            {
                client.Close();
            }
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            AgentSvcClient client = new AgentSvcClient();

            try
            {
                DateTime t0 = DateTime.UtcNow;

                using (FileStream fileStream = 
                    File.Open(DefConfigArc, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    client.UploadConfig(new ConfigOptions(), sessionID, fileStream);
                }

                MessageBox.Show("Done in " + (int)(DateTime.UtcNow - t0).TotalMilliseconds + " ms");
            }
            finally
            {
                client.Close();
            }
        }
    }
}
