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
 * Module   : AgentWcfClient
 * Summary  : Represents a client of the Agent service
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using Scada.Agent.Connector.AgentSvcRef;
using System;
using System.IO;
using System.ServiceModel;

namespace Scada.Agent.Connector
{
    /// <summary>
    /// Represents a client of the Agent service.
    /// <para>Представляет клиент службы Агента.</para>
    /// </summary>
    public class AgentWcfClient
    {
        /// <summary>
        /// The connection settings.
        /// </summary>
        protected ConnectionSettings connSettings;
        /// <summary>
        /// The WCF service client.
        /// </summary>
        protected AgentSvcClient client;
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
            InitSvcClient();
        }


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

            if (!client.Login(sessionID, connSettings.Username, encryptedPassword, connSettings.ScadaInstance,
                out string errMsg))
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
            if (DateTime.UtcNow - activityDT > TimeSpan.FromSeconds(30) /*&& !client.IsLoggedOn()*/) // TODO: const
            {
                Connect();
                RegisterActivity();
            }
        }
        
        /// <summary>
        /// Registers communication activity.
        /// </summary>
        public void RegisterActivity()
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
        /// Gets available parts of the configuration.
        /// </summary>
        public ConfigParts GetAvailableConfig()
        {
            RestoreConnection();

            if (!client.GetAvailableConfig(sessionID, out ConfigParts configParts))
            {
                throw new ScadaException(Localization.UseRussian ?
                    "Не удалось получить доступные части конфигурации." :
                    "Unable to get available parts of the configuration.");
            }

            RegisterActivity();

            return configParts;
        }

        /// <summary>
        /// Downloads the configuration to the file.
        /// </summary>
        public void DownloadConfig(string destFileName, ConfigOptions configOptions)
        {
            RestoreConnection();

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

                RegisterActivity();
            }
            finally
            {
                downloadStream.Close();
            }
        }

        /// <summary>
        /// Uploads the configuration from the file.
        /// </summary>
        public void UploadConfig(string sourceFileName, ConfigOptions configOptions)
        {
            RestoreConnection();

            using (FileStream outStream = new FileStream(sourceFileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                client.UploadConfig(configOptions, sessionID, outStream);
            }

            RegisterActivity();
        }
    }
}
