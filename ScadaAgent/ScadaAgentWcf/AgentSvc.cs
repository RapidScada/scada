using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel;

namespace Scada.Agent.Wcf
{
    [ServiceContract]
    public class AgentSvc
    {
        [OperationContract]
        public bool GetSessionID(out long sessionID)
        {
            sessionID = CryptoUtils.GetRandomLong();
            return true;
        }

        [OperationContract]
        public bool Login(long sessionID, string username, string encryptedPassword, string scadaInstanceName)
        {
            return true;
        }

        [OperationContract]
        public bool ControlService(long sessionID, ScadaApps service, ServiceCommand command)
        {
            return true;
        }

        [OperationContract]
        public bool GetServiceStatus(long sessionID, ScadaApps service, out bool isRunning)
        {
            isRunning = true;
            return true;
        }

        [OperationContract]
        public bool GetInstalledApps(long sessionID, out ScadaApps installedApps)
        {
            installedApps = ScadaApps.None;
            return true;
        }

        [OperationContract]
        public Stream DownloadConfig(long sessionID, ConfigOptions configOptions)
        {
            return null;
        }

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

        [OperationContract]
        public bool FindFiles(long sessionID, AppPath appPath, out ICollection<string> paths)
        {
            paths = null;
            return true;
        }

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

        [OperationContract]
        public Stream DownloadFileRest(long sessionID, AppPath appPath, long position)
        {
            return null;
        }
    }
}
