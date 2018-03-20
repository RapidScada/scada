using Scada.Agent.Wcf;
using System;
using System.ServiceModel;

namespace ScadaAgentMono
{
    class Program
    {
        private static ServiceHost agentSvcHost; // хост WCF-службы для взаимодействия с агентом

        /// <summary>
        /// Запустить WCF-службу для взаимодействия с агентом
        /// </summary>
        private static bool StartWcfService(out string serviceUrl)
        {
            try
            {
                agentSvcHost = new ServiceHost(typeof(AgentSvc));
                ServiceBehaviorAttribute behavior =
                    agentSvcHost.Description.Behaviors.Find<ServiceBehaviorAttribute>();
                behavior.ConcurrencyMode = ConcurrencyMode.Multiple;
                behavior.InstanceContextMode = InstanceContextMode.Single;
                behavior.UseSynchronizationContext = false;
                agentSvcHost.Open();
                serviceUrl = agentSvcHost.BaseAddresses.Count > 0 ?
                    agentSvcHost.BaseAddresses[0].AbsoluteUri : "";

                //Log.WriteAction(Localization.UseRussian ?
                //    "WCF-служба запущена" :
                //    "WCF service is started");
                Console.WriteLine("WCF service is started");

                return true;
            }
            catch (Exception ex)
            {
                //Log.WriteException(ex, Localization.UseRussian ?
                //    "Ошибка при запуске WCF-службы" :
                //    "Error starting WCF service");
                Console.WriteLine(ex.ToString());
                serviceUrl = "";
                return false;
            }
        }

        /// <summary>
        /// Остановить WCF-службу, взаимодействующую с веб-интерфейсом
        /// </summary>
        private static void StopWcfService()
        {
            if (agentSvcHost != null)
            {
                try
                {
                    agentSvcHost.Close();
                    //Log.WriteAction(Localization.UseRussian ?
                    //    "WCF-служба остановлена" :
                    //    "WCF service is stopped");
                    Console.WriteLine("WCF service is stopped");
                }
                catch
                {
                    agentSvcHost.Abort();
                    //Log.WriteAction(Localization.UseRussian ?
                    //    "WCF-служба прервана" :
                    //    "WCF service is aborted");
                    Console.WriteLine("WCF service is aborted");
                }

                agentSvcHost = null;
            }
        }

        static void Main(string[] args)
        {
            if (StartWcfService(out string serviceUrl))
                Console.WriteLine("serviceUrl = " + serviceUrl);

            Console.WriteLine("Press a key to stop service...");
            Console.ReadKey(true);

            StopWcfService();
        }
    }
}
