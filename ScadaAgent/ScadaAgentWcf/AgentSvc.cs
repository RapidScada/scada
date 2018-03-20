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
    }
}
