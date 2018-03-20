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
        public bool ControlService(long sessionID, ScadaApps service, ServiceCommands command)
        {
            return true;
        }

        [OperationContract]
        public bool GetInstalledApps(long sessionID, out ScadaApps installedApps)
        {
            installedApps = ScadaApps.None;
            return true;
        }

        [OperationContract]
        public bool GetConfig(long sessionID, ScadaApps app, out Stream stream)
        {
            stream = null;
            return true;
        }
    }
}
