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
        /// Игнорируемые пути файлов, специфичные для экземпляра системы
        /// </summary>
        private static readonly RelPath[] IgnoredPaths;


        /// <summary>
        /// Статический конструктор
        /// </summary>
        static DownloadUpload()
        {
            IgnoredPaths = new RelPath[]
            {
                new RelPath() { ConfigPart = ConfigParts.Comm, AppFolder = AppFolder.Config, Path = "*_Reg.xml" },
                new RelPath() { ConfigPart = ConfigParts.Comm, AppFolder = AppFolder.Config, Path = "CompCode.txt" },
                new RelPath() { ConfigPart = ConfigParts.Server, AppFolder = AppFolder.Config, Path = "*_Reg.xml" },
                new RelPath() { ConfigPart = ConfigParts.Server, AppFolder = AppFolder.Config, Path = "CompCode.txt" },
                new RelPath() { ConfigPart = ConfigParts.Web, AppFolder = AppFolder.Config, Path = "*_Reg.xml" },
                new RelPath() { ConfigPart = ConfigParts.Web, AppFolder = AppFolder.Storage, Path = "" },
            };
        }


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
                writer?.WriteLine(AppPhrases.SessionCreated, sessionID);
            else
                throw new ScadaException(AppPhrases.UnableCreateSession);

            // вход в систему
            string encryptedPassword = ScadaUtils.Encrypt(connectionSettings.Password,
                connectionSettings.SecretKey, CreateIV(sessionID));

            if (client.Login(out string errMsg, sessionID, connectionSettings.Username,
                encryptedPassword, connectionSettings.ScadaInstance))
                writer?.WriteLine(AppPhrases.LoggedOn);
            else
                throw new ScadaException(string.Format(AppPhrases.UnableLogin, errMsg));
        }

        /// <summary>
        /// Упаковать конфигурацию во временный файл
        /// </summary>
        private static void PackConfig(string srcDir, List<string> selectedFiles,
            out string outFileName, out ConfigParts configParts)
        {
            srcDir = ScadaUtils.NormalDir(srcDir);
            int srcDirLen = srcDir.Length;
            outFileName = srcDir + "upload-config_" +
                DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".zip";
            configParts = ConfigParts.None;

            using (ZipFile zipFile = new ZipFile(outFileName))
            {
                foreach (string relPath in selectedFiles)
                {
                    string path = srcDir + relPath;
                    configParts = configParts | GetConfigPart(relPath);

                    if (Directory.Exists(path)) // путь является директорией
                    {
                        string[] filesInDir = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
                        foreach (string fileName in filesInDir)
                        {
                            if (Path.GetExtension(fileName) != ".bak")
                            {
                                string dirInArc = Path.GetDirectoryName(fileName.Substring(srcDirLen))
                                    .Replace('\\', '/');
                                zipFile.AddFile(fileName, dirInArc);
                            }
                        }
                    }
                    else if (File.Exists(path))
                    {
                        string dirInArc = Path.GetDirectoryName(relPath).Replace('\\', '/');
                        zipFile.AddFile(path, dirInArc);
                    }
                }

                zipFile.Save();
            }
        }

        /// <summary>
        /// Получить часть конфигурации, которая соответствует пути
        /// </summary>
        private static ConfigParts GetConfigPart(string relPath)
        {
            if (relPath.StartsWith("BaseDAT", StringComparison.Ordinal))
                return ConfigParts.Base;
            else if (relPath.StartsWith("Interface", StringComparison.Ordinal))
                return ConfigParts.Interface;
            else if (relPath.StartsWith("ScadaComm", StringComparison.Ordinal))
                return ConfigParts.Comm;
            else if (relPath.StartsWith("ScadaServer", StringComparison.Ordinal))
                return ConfigParts.Server;
            else if (relPath.StartsWith("ScadaWeb", StringComparison.Ordinal))
                return ConfigParts.Web;
            else
                return ConfigParts.None;
        }

        /// <summary>
        /// Получить части конфигурации, которые содержатся в архиве
        /// </summary>
        private static ConfigParts GetConfigParts(string arcFileName)
        {
            ConfigParts configParts = ConfigParts.None;

            using (ZipFile zipFile = new ZipFile(arcFileName))
            {
                foreach (ZipEntry zipEntry in zipFile.Entries)
                {
                    configParts = configParts | GetConfigPart(zipEntry.FileName);
                }
            }

            return configParts;
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
                ServersSettings.DownloadSettings downloadSettings = serverSettings.Download;
                ConfigOptions configOptions = new ConfigOptions() { ConfigParts = ConfigParts.All };

                if (!downloadSettings.IncludeSpecificFiles)
                    configOptions.IgnoredPaths = IgnoredPaths;

                Stream downloadStream = client.DownloadConfig(sessionID, configOptions);

                if (downloadStream == null)
                    throw new ScadaException(AppPhrases.DownloadDataEmpty);

                if (downloadSettings.SaveToDir)
                {
                    // сохранение в директорию
                    string destDir = downloadSettings.DestDir;
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
                    string destFile = downloadSettings.DestFile;
                    Directory.CreateDirectory(Path.GetDirectoryName(destFile));

                    using (FileStream destStream = File.Create(destFile))
                    {
                        downloadStream.CopyTo(destStream);
                    }
                }

                downloadStream.Close();
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
        public static bool UploadConfig(ServersSettings.ServerSettings serverSettings,
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
                    string.Format(AppPhrases.UploadTitle, DateTime.Now.ToString("G", Localization.Culture)));
                writer.WriteLine(AppPhrases.ConnectionName, serverSettings.Connection.Name);
                writer.WriteLine();

                // соединение с Агентом
                Connect(serverSettings.Connection, writer, out client, out long sessionID);

                // подготовка конфигурации для передачи
                ServersSettings.UploadSettings uploadSettings = serverSettings.Upload;
                ConfigOptions configOptions = new ConfigOptions();
                ConfigParts configParts;
                string outFileName;
                bool deleteOutFile;

                if (uploadSettings.GetFromDir)
                {
                    PackConfig(uploadSettings.SrcDir, uploadSettings.SelectedFiles, 
                        out outFileName, out configParts);
                    configOptions.ConfigParts = configParts;
                    deleteOutFile = true;
                }
                else
                {
                    outFileName = uploadSettings.SrcFile;
                    configOptions.ConfigParts = configParts = GetConfigParts(outFileName);
                    deleteOutFile = false;
                }

                if (configOptions.ConfigParts == ConfigParts.None)
                    throw new ScadaException(AppPhrases.NoConfigInSrc);

                if (!uploadSettings.ClearSpecificFiles)
                    configOptions.IgnoredPaths = IgnoredPaths;

                // передача конфигурации
                using (Stream outStream = File.Open(outFileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    client.UploadConfig(configOptions, sessionID, outStream);
                    writer.WriteLine(AppPhrases.ConfigUploaded);
                }

                // удаление временного файла
                if (deleteOutFile)
                    File.Delete(outFileName);

                // перезапуск служб на удалённом сервере
                if (configParts.HasFlag(ConfigParts.Base) || configParts.HasFlag(ConfigParts.Server))
                {
                    if (client.ControlService(sessionID, ServiceApp.Server, ServiceCommand.Restart))
                        writer.WriteLine(AppPhrases.ServerRestarted);
                    else
                        writer.WriteLine(AppPhrases.UnableRestartServer);
                }

                if (configParts.HasFlag(ConfigParts.Base) || configParts.HasFlag(ConfigParts.Comm))
                {
                    if (client.ControlService(sessionID, ServiceApp.Comm, ServiceCommand.Restart))
                        writer.WriteLine(AppPhrases.CommRestarted);
                    else
                        writer.WriteLine(AppPhrases.UnableRestartComm);
                }

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
        }

        /// <summary>
        /// Соединиться с Агентом
        /// </summary>
        public static bool Connect(ServersSettings.ConnectionSettings connectionSettings,
            out AgentSvcClient client, out long sessionID, out string errMsg)
        {
            try
            {
                Connect(connectionSettings, null, out client, out sessionID);
                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                client = null;
                sessionID = 0;
                errMsg = AppPhrases.ConnectAgentError + ":\r\n" + ex.Message;
                return false;
            }
        }
    }
}
