using System.ServiceModel;

namespace ScadaAgentWcf
{
    [ServiceContract]
    public class AgentSvc
    {
        [OperationContract]
        public double Sum(double a, double b)
        {
            return a + b;
        }
    }
}
