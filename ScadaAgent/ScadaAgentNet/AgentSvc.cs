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
 * Module   : ScadaAgentNet
 * Summary  : WCF service for interacting with the agent
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Utils;

namespace Scada.Agent.Net
{
    /// <summary>
    /// WCF service for interacting with the agent
    /// <para>WCF-сервис для взаимодействия с агентом</para>
    /// </summary>
    [ServiceContract]
    public class AgentSvc
    {
        /// <summary>
        /// Данные приложения
        /// </summary>
        private static readonly AppData AppData = AppData.GetInstance();
        /// <summary>
        /// Журнал приложения
        /// </summary>
        private static readonly ILog Log = AppData.Log;
        /// <summary>
        /// Менеджер сессий
        /// </summary>
        private static readonly SessionManager SessionManager = AppData.SessionManager;


        /// <summary>
        /// Получить IP-адрес текущего подключения
        /// </summary>
        private string GetClientIP()
        {
            try
            {
                OperationContext context = OperationContext.Current;
                MessageProperties props = context.IncomingMessageProperties;
                RemoteEndpointMessageProperty remoteEndPoint =
                       (RemoteEndpointMessageProperty)props[RemoteEndpointMessageProperty.Name];
                return remoteEndPoint.Address;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// Попытаться получить сессию по идентификатору
        /// </summary>
        private bool TryGetSession(long sessionID, out Session session)
        {
            session = SessionManager.GetSession(sessionID);

            if (session == null)
            {
                Log.WriteError(string.Format(Localization.UseRussian ?
                    "Сессия с ид. {0} не найдена" :
                    "Session with ID {0} not found"));
                return false;
            }
            else
            {
                session.RegisterActivity();
                return true;
            }
        }


        /// <summary>
        /// Создать новую сессию
        /// </summary>
        [OperationContract]
        public bool CreateSession(out long sessionID)
        {
            Session session = SessionManager.CreateSession();

            if (session == null)
            {
                sessionID = 0;
                return false;
            }
            else
            {
                session.IpAddress = GetClientIP();
                sessionID = session.ID;
                return true;
            }
        }

        /// <summary>
        /// Войти в систему
        /// </summary>
        [OperationContract]
        public bool Login(long sessionID, string username, string encryptedPassword, string scadaInstanceName)
        {
            if (TryGetSession(sessionID, out Session session))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Управлять службой
        /// </summary>
        [OperationContract]
        public bool ControlService(long sessionID, ScadaApps service, ServiceCommand command)
        {
            return true;
        }

        /// <summary>
        /// Получить статус службы
        /// </summary>
        [OperationContract]
        public bool GetServiceStatus(long sessionID, ScadaApps service, out bool isRunning)
        {
            isRunning = true;
            return true;
        }

        /// <summary>
        /// Получить установленные приложения экземпляра системы
        /// </summary>
        [OperationContract]
        public bool GetInstalledApps(long sessionID, out ScadaApps installedApps)
        {
            installedApps = ScadaApps.None;
            return true;
        }

        /// <summary>
        /// Скачать конфигурацию
        /// </summary>
        [OperationContract]
        public Stream DownloadConfig(long sessionID, ConfigOptions configOptions)
        {
            return null;
        }

        /// <summary>
        /// Загрузить конфигурацию
        /// </summary>
        [OperationContract]
        public void UploadConfig(ConfigUploadMessage configUploadMessage)
        {
            if (configUploadMessage.Stream == null)
            {
                System.Console.WriteLine("configUploadMessage.Stream is null");
            }
            else
            {
                /*byte[] buf = new byte[100];
                int cnt = configUploadMessage.Stream.Read(buf, 0, buf.Length);
                string s = System.Text.Encoding.ASCII.GetString(buf, 0, cnt);
                System.Console.WriteLine(s);*/

                DateTime t0 = DateTime.UtcNow;
                byte[] buf = new byte[1024];
                Stream saver = File.Create("file2.txt");
                int cnt;

                while ((cnt = configUploadMessage.Stream.Read(buf, 0, buf.Length)) > 0)
                {
                    saver.Write(buf, 0, cnt);
                }

                saver.Close();
                Console.WriteLine("Done in " + (int)(DateTime.UtcNow - t0).TotalMilliseconds + " ms");
            }

            //return true;
        }

        /// <summary>
        /// Найти файлы
        /// </summary>
        [OperationContract]
        public bool FindFiles(long sessionID, AppPath appPath, out ICollection<string> paths)
        {
            paths = null;
            return true;
        }

        /// <summary>
        /// Скачать файл
        /// </summary>
        [OperationContract]
        public Stream DownloadFile(long sessionID, AppPath appPath)
        {
            /*byte[] buffer = System.Text.Encoding.ASCII.GetBytes("hello");
            MemoryStream stream = new MemoryStream(buffer.Length);
            stream.Write(buffer, 0, buffer.Length);
            stream.Position = 0;*/

            Stream stream = File.Open("big.txt", FileMode.Open);
            return stream;
        }

        /// <summary>
        /// Скачать часть файла с заданной позиции
        /// </summary>
        [OperationContract]
        public Stream DownloadFileRest(long sessionID, AppPath appPath, long position)
        {
            return null;
        }
    }
}
