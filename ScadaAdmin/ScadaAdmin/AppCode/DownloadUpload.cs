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
 * Module   : SCADA-Administrator
 * Summary  : Downloading and uploading configuration
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using Ionic.Zip;
using Scada;
using ScadaAdmin.AgentSvcRef;
using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using System.Text;

namespace ScadaAdmin
{
    /// <summary>
    /// Downloading and uploading configuration
    /// <para>Скачивание и передача конфигурации</para>
    /// </summary>
    internal static class DownloadUpload
    {
        /// <summary>
        /// Создать вектор инициализации на освнове ид. сессии
        /// </summary>
        private static byte[] CreateIV(long sessionID)
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
        /// Сформировать адрес для подключения к Агенту
        /// </summary>
        private static EndpointAddress GetEpAddress(string host, int port)
        {
            return new EndpointAddress(
                string.Format("http://{0}:{1}/ScadaAgent/ScadaAgentSvc/", host, port));
        }

        /// <summary>
        /// Соединиться с Агентом
        /// </summary>
        private static void Connect(ServersSettings.ConnectionSettings connectionSettings, 
            StreamWriter writer, out AgentSvcClient client, out long sessionID)
        {
            // настройка соединения
            client = new AgentSvcClient();
            client.Endpoint.Address = GetEpAddress(connectionSettings.Host, connectionSettings.Port);

            // создание сессии
            if (client.CreateSession(out sessionID))
                writer.WriteLine(AppPhrases.SessionCreated, sessionID);
            else
                throw new ScadaException(AppPhrases.UnableCreateSession);

            // вход в систему
            string encryptedPassword = ScadaUtils.Encrypt(connectionSettings.Password,
                connectionSettings.SecretKey, CreateIV(sessionID));

            if (client.Login(out string errMsg, sessionID, connectionSettings.Username,
                encryptedPassword, connectionSettings.ScadaInstance))
                writer.WriteLine(AppPhrases.LoggedOn);
            else
                throw new ScadaException(string.Format(AppPhrases.UnableLogin, errMsg));
        }


        /// <summary>
        /// Скачать конфигурацию
        /// </summary>
        public static bool DownloadConfig(ServersSettings.ServerSettings serverSettings,
            string logFileName, out bool logCreated, out string msg)
        {
            if (serverSettings == null)
                throw new ArgumentNullException("serverSettings");
            if (logFileName == null)
                throw new ArgumentNullException("logFileName");

            logCreated = false;
            StreamWriter writer = null;
            AgentSvcClient client = null;

            try
            {
                DateTime t0 = DateTime.UtcNow;
                writer = new StreamWriter(logFileName, false, Encoding.UTF8);
                logCreated = true;

                AppUtils.WriteTitle(writer,
                    string.Format(AppPhrases.DownloadTitle, DateTime.Now.ToString("G", Localization.Culture)));
                writer.WriteLine(AppPhrases.ConnectionName, serverSettings.Connection.Name);
                writer.WriteLine();

                // соединение с Агентом
                Connect(serverSettings.Connection, writer, out client, out long sessionID);

                // скачивание конфигурации
                Stream downloadStream = client.DownloadConfig(sessionID,
                    new ConfigOptions() { ConfigParts = ConfigParts.All });

                if (downloadStream == null)
                    throw new ScadaException(AppPhrases.DownloadDataEmpty);

                if (serverSettings.Download.SaveToDir)
                {
                    // сохранение в директорию
                    string destDir = serverSettings.Download.DestDir;
                    Directory.CreateDirectory(destDir);
                    string tempFileName = destDir + "download-config_" + 
                        DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".zip";

                    try
                    {
                        // сохранение во временный файл, т.к. распаковка из MemoryStream не работает
                        using (FileStream destStream = File.Create(tempFileName))
                        {
                            downloadStream.CopyTo(destStream);
                        }

                        // распаковка
                        using (ZipFile zipFile = ZipFile.Read(tempFileName))
                        {
                            foreach (ZipEntry zipEntry in zipFile)
                            {
                                zipEntry.Extract(destDir, ExtractExistingFileAction.OverwriteSilently);
                            }
                        }
                    }
                    finally
                    {
                        try { File.Delete(tempFileName); }
                        catch { }
                    }
                }
                else
                {
                    // сохранение в файл
                    string destFile = serverSettings.Download.DestFile;
                    Directory.CreateDirectory(Path.GetDirectoryName(destFile));

                    using (FileStream destStream = File.Create(destFile))
                    {
                        downloadStream.CopyTo(destStream);
                    }
                }

                msg = string.Format(AppPhrases.DownloadSuccessful, (int)(DateTime.UtcNow - t0).TotalSeconds);
                writer.WriteLine(msg);
                return true;
            }
            catch (Exception ex)
            {
                msg = AppPhrases.DownloadError + ":\r\n" + ex.Message;

                try { writer?.WriteLine(msg); }
                catch { }

                return false;
            }
            finally
            {
                try { writer?.Close(); }
                catch { }

                try { client?.Close(); }
                catch { }
            }
        }

        /// <summary>
        /// Передать конфигурацию
        /// </summary>
        public static bool UploadConfig(ServersSettings.ConnectionSettings connectionSettings,
            List<string> fileNames, ConfigParts configParts, string logFileName, out bool logCreated, out string msg)
        {
            if (connectionSettings == null)
                throw new ArgumentNullException("connectionSettings");
            if (fileNames == null)
                throw new ArgumentNullException("fileNames");
            if (logFileName == null)
                throw new ArgumentNullException("logFileName");

            logCreated = false;
            StreamWriter writer = null;
            AgentSvcClient client = null;

            try
            {
                DateTime t0 = DateTime.UtcNow;

                writer = new StreamWriter(logFileName, false, Encoding.UTF8);
                logCreated = true;

                AppUtils.WriteTitle(writer,
                    string.Format(AppPhrases.UploadTitle, DateTime.Now.ToString("G", Localization.Culture)));
                writer.WriteLine(AppPhrases.ConnectionName, connectionSettings.Name);
                writer.WriteLine();

                // соединение с Агентом
                Connect(connectionSettings, writer, out client, out long sessionID);

                // архивирование конфигурации
                MemoryStream outStream = new MemoryStream(); // поток закрывается автоматически с помощью WCF

                using (ZipFile zipFile = new ZipFile())
                {
                    zipFile.Save(outStream);
                }

                // передача конфигурации
                client.UploadConfig(new ConfigOptions() { ConfigParts = configParts }, sessionID, outStream);

                msg = string.Format(AppPhrases.UploadSuccessful, (int)(DateTime.UtcNow - t0).TotalSeconds);
                writer.WriteLine(msg);
                return true;
            }
            catch (Exception ex)
            {
                msg = AppPhrases.UploadError + ":\r\n" + ex.Message;

                try { writer?.WriteLine(msg); }
                catch { }

                return false;
            }
            finally
            {
                try { writer?.Close(); }
                catch { }

                try { client?.Close(); }
                catch { }
            }

            msg = "!!!";
            return true;
        }
    }
}
