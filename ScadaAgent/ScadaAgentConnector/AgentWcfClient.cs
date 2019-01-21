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
 * Module   : ScadaAgentConnector
 * Summary  : Represents a client of the Agent service
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using Scada.Agent.Connector.AgentSvcRef;
using Scada.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using System.Text;

namespace Scada.Agent.Connector
{
    /// <summary>
    /// Represents a client of the Agent service.
    /// <para>Представляет клиента службы Агента.</para>
    /// </summary>
    public class AgentWcfClient : IAgentClient
    {
        /// <summary>
        /// The time span of checking connection.
        /// </summary>
        protected readonly TimeSpan CheckConnectionSpan = TimeSpan.FromSeconds(10);

        /// <summary>
        /// The WCF service client.
        /// </summary>
        internal AgentSvcClient client;
        /// <summary>
        /// The connection settings.
        /// </summary>
        protected ConnectionSettings connSettings;
        /// <summary>
        /// The ID of the communication session.
        /// </summary>
        protected long sessionID;
        /// <summary>
        /// The timestamp of the last successful communication.
        /// </summary>
        protected DateTime activityDT;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public AgentWcfClient(ConnectionSettings connSettings)
        {
            this.connSettings = connSettings ?? throw new ArgumentNullException("connSettings");
            sessionID = 0;
            activityDT = DateTime.MinValue;
            IsLocal = string.Equals(connSettings.Host, "localhost", StringComparison.OrdinalIgnoreCase);
            InitSvcClient();
        }


        /// <summary>
        /// Gets a value indicating whether the connection is local.
        /// </summary>
        public bool IsLocal { get; protected set; }


        /// <summary>
        /// Initializes the WCF service client.
        /// </summary>
        protected void InitSvcClient()
        {
            client = new AgentSvcClient();
            client.Endpoint.Address = new EndpointAddress(string.Format(
                "http://{0}:{1}/ScadaAgent/ScadaAgentSvc/", connSettings.Host, connSettings.Port));
        }

        /// <summary>
        /// Connects and authenticates with Agent
        /// </summary>
        protected void Connect()
        {
            if (!client.CreateSession(out sessionID))
            {
                throw new ScadaException(Localization.UseRussian ?
                    "Не удалось создать сессию." :
                    "Unable to create session.");
            }

            string encryptedPassword = ScadaUtils.Encrypt(connSettings.Password, 
                connSettings.SecretKey, CreateIV(sessionID));

            if (!client.Login(out string errMsg, sessionID, connSettings.Username, encryptedPassword, 
                connSettings.ScadaInstance))
            {
                throw new ScadaException(string.Format(Localization.UseRussian ?
                    "Не удалось войти в систему - {0}." :
                    "Unable to login - {0}", errMsg));
            }
        }

        /// <summary>
        /// Restores a connection with Agent.
        /// </summary>
        protected void RestoreConnection()
        {
            if (sessionID == 0 || 
                DateTime.UtcNow - activityDT > CheckConnectionSpan && !client.IsLoggedOn(sessionID))
            {
                Connect();
                RegisterActivity();
            }
        }

        /// <summary>
        /// Registers communication activity.
        /// </summary>
        protected void RegisterActivity()
        {
            activityDT = DateTime.UtcNow;
        }

        /// <summary>
        /// Creates an initialization vector based on the session ID.
        /// </summary>
        protected static byte[] CreateIV(long sessionID)
        {
            byte[] iv = new byte[ScadaUtils.IVSize];
            byte[] sessBuf = BitConverter.GetBytes(sessionID);
            int sessBufLen = sessBuf.Length;

            for (int i = 0; i < ScadaUtils.IVSize; i++)
            {
                iv[i] = sessBuf[i % sessBufLen];
            }

            return iv;
        }


        /// <summary>
        /// Tests the connection with Agent.
        /// </summary>
        public bool TestConnection(out string errMsg)
        {
            try
            {
                RestoreConnection();
                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Sends the command to the service.
        /// </summary>
        public bool ControlService(ServiceApp serviceApp, ServiceCommand command)
        {
            RestoreConnection();
            bool result = client.ControlService(sessionID, serviceApp, command);
            RegisterActivity();
            return result;
        }

        /// <summary>
        /// Gets the current status of the specified service.
        /// </summary>
        public bool GetServiceStatus(ServiceApp serviceApp, out ServiceStatus serviceStatus)
        {
            RestoreConnection();
            bool result = client.GetServiceStatus(out serviceStatus, sessionID, serviceApp);
            RegisterActivity();
            return result;
        }

        /// <summary>
        /// Gets available parts of the configuration.
        /// </summary>
        public bool GetAvailableConfig(out ConfigParts configParts)
        {
            RestoreConnection();
            bool result = client.GetAvailableConfig(out configParts, sessionID);
            RegisterActivity();
            return result;
        }

        /// <summary>
        /// Downloads the configuration to the file.
        /// </summary>
        public void DownloadConfig(string destFileName, ConfigOptions configOptions)
        {
            if (string.IsNullOrEmpty(destFileName))
                throw new ArgumentException("destFileName must not be empty.", "destFileName");
            if (configOptions == null)
                throw new ArgumentNullException("configOptions");

            RestoreConnection();

            if (IsLocal)
            {
                // copy the configuration locally
                if (!client.PackConfig(sessionID, destFileName, configOptions))
                {
                    throw new ScadaException(Localization.UseRussian ?
                        "Не удалось упаковать конфигурацию в архив." :
                        "Unable to pack the configuration in the archive.");
                }
            }
            else
            {
                // transfer the configuration over the network
                Stream downloadStream = client.DownloadConfig(sessionID, configOptions);

                if (downloadStream == null)
                {
                    throw new ScadaException(Localization.UseRussian ?
                        "Отсутствуют данные для скачивания." :
                        "No data to download.");
                }

                try
                {
                    using (FileStream destStream =
                        new FileStream(destFileName, FileMode.Create, FileAccess.Write, FileShare.Read))
                    {
                        downloadStream.CopyTo(destStream);
                    }
                }
                finally
                {
                    downloadStream.Dispose();
                }
            }

            RegisterActivity();
        }

        /// <summary>
        /// Uploads the configuration from the file.
        /// </summary>
        public void UploadConfig(string srcFileName, ConfigOptions configOptions)
        {
            if (string.IsNullOrEmpty(srcFileName))
                throw new ArgumentException("srcFileName must not be empty.", "destFileName");
            if (configOptions == null)
                throw new ArgumentNullException("configOptions");

            RestoreConnection();

            if (IsLocal)
            {
                // copy the configuration locally
                if (!client.UnpackConfig(sessionID, srcFileName, configOptions))
                {
                    throw new ScadaException(Localization.UseRussian ?
                        "Не удалось распаковать архив конфигурации." :
                        "Unable to unpack the configuration archive.");
                }
            }
            else
            {
                // transfer the configuration over the network
                using (FileStream outStream = 
                    new FileStream(srcFileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    client.UploadConfig(configOptions, sessionID, outStream);
                }
            }

            RegisterActivity();
        }

        /// <summary>
        /// Reads the log file.
        /// </summary>
        public bool ReadLog(RelPath relPath, ref DateTime fileAge, out ICollection<string> lines)
        {
            return ReadLog(relPath, -1, ref fileAge, out lines);
        }

        /// <summary>
        /// Reads the rest of the log file.
        /// </summary>
        public bool ReadLog(RelPath relPath, long offsetFromEnd, ref DateTime fileAge, out ICollection<string> lines)
        {
            if (relPath == null)
                throw new ArgumentNullException("relPath");

            RestoreConnection();
            DateTime newFileAge = client.GetFileAgeUtc(sessionID, relPath);

            if (newFileAge == DateTime.MinValue) // log doesn't exist
            {
                fileAge = newFileAge;
                lines = new string[] { CommonPhrases.FileNotFound };
                return true;
            }
            else if (fileAge != newFileAge)
            {
                Stream downloadStream = client.DownloadFileRest(sessionID, relPath, offsetFromEnd);

                if (downloadStream == null) // error opening the log
                {
                    lines = new string[] { CommonPhrases.Error };
                    return true;
                }
                else
                {
                    List<string> lineList = new List<string>();

                    try
                    {
                        using (StreamReader reader = new StreamReader(downloadStream, Encoding.UTF8))
                        {
                            // add or skip the first line
                            if (!reader.EndOfStream)
                            {
                                string s = reader.ReadLine();
                                if (offsetFromEnd < 0)
                                    lineList.Add(s);
                            }

                            // read the rest lines
                            while (!reader.EndOfStream)
                            {
                                lineList.Add(reader.ReadLine());
                            }
                        }
                    }
                    finally
                    {
                        downloadStream.Dispose();
                    }

                    if (lineList.Count == 0)
                        lineList.Add(CommonPhrases.NoData);

                    RegisterActivity();
                    fileAge = newFileAge;
                    lines = lineList;
                    return true;
                }
            }
            else
            {
                lines = null;
                return false;
            }
        }

        /// <summary>
        /// Creates new settings for connecting to Server based on the connection settings of the Agent.
        /// </summary>
        public CommSettings CreateCommSettings()
        {
            return new CommSettings()
            {
                ServerHost = connSettings.Host,
                ServerUser = connSettings.Username,
                ServerPwd = connSettings.Password
            };
        }
    }
}
