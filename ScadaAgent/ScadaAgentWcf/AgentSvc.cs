using System.Collections.Generic;
using System.IO;
using System.ServiceModel;

namespace Scada.Agent.Wcf
{
    [ServiceContract]
    public class AgentSvc
    {
        private static ScadaManager mngr = new ScadaManager();

        [OperationContract]
        public double Sum(double a, double b)
        {
            return mngr.Sum(a, b);
        }

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
        public bool UploadConfig(/*long sessionID, ConfigOptions configOptions,*/ Stream stream)
        {
            stream = null;
            return true;
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
            byte[] buffer = System.Text.Encoding.ASCII.GetBytes("hello");
            MemoryStream stream = new MemoryStream(buffer.Length);
            stream.Write(buffer, 0, buffer.Length);
            stream.Position = 0;
            return stream;
        }

        [OperationContract]
        public Stream DownloadFileRest(long sessionID, AppPath appPath, long position)
        {
            return null;
        }
    }
}
