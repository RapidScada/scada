using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScadaAgentMono
{
    class Program
    {
        //private ServiceHost schemeEditorSvcHost; // хост WCF-службы для взаимодействия с веб-интерфейсом

        /*private bool StartWcf()
        {
            try
            {
                ScadaSchemeSvc schemeSvc = new ScadaSchemeSvc();
                schemeSvcHost = new ServiceHost(schemeSvc);
                ServiceBehaviorAttribute behavior =
                    schemeSvcHost.Description.Behaviors.Find<ServiceBehaviorAttribute>();
                behavior.InstanceContextMode = InstanceContextMode.Single;
                behavior.UseSynchronizationContext = false;
                schemeSvcHost.Open();

                domainSvcHost = new ServiceHost(typeof(CrossDomainSvc));
                domainSvcHost.Open();

                return true;
            }
            catch (Exception ex)
            {
                log.WriteAction("Ошибка при запуске WCF-служб: " + ex.Message, Log.ActTypes.Exception);
                ScadaUtils.ShowError(
                    "Ошибка при запуске служб обмена данными.\nНормальная работа программы невозможна.");
                return false;
            }
        }

        private void StopWcf()
        {
            // остановка WCF-служб
            if (schemeSvcHost != null)
            {
                try { schemeSvcHost.Close(); }
                catch { schemeSvcHost.Abort(); }
            }

            if (domainSvcHost != null)
            {
                try { domainSvcHost.Close(); }
                catch { domainSvcHost.Abort(); }
            }
        }*/

        /*
        /// <summary>
        /// Запустить WCF-службу для взаимодействия с веб-интерфейсом
        /// </summary>
        private bool StartWcfService(out string serviceUrl)
        {
            try
            {
                schemeEditorSvcHost = new ServiceHost(typeof(SchemeEditorSvc));
                ServiceBehaviorAttribute behavior =
                    schemeEditorSvcHost.Description.Behaviors.Find<ServiceBehaviorAttribute>();
                behavior.InstanceContextMode = InstanceContextMode.Single;
                behavior.UseSynchronizationContext = false;
                schemeEditorSvcHost.Open();
                serviceUrl = schemeEditorSvcHost.BaseAddresses.Count > 0 ?
                    schemeEditorSvcHost.BaseAddresses[0].AbsoluteUri : "";

                Log.WriteAction(Localization.UseRussian ?
                    "WCF-служба запущена" :
                    "WCF service is started");

                return true;
            }
            catch (Exception ex)
            {
                Log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при запуске WCF-службы" :
                    "Error starting WCF service");
                serviceUrl = "";
                return false;
            }
        }

        /// <summary>
        /// Остановить WCF-службу, взаимодействующую с веб-интерфейсом
        /// </summary>
        private void StopWcfService()
        {
            if (schemeEditorSvcHost != null)
            {
                try
                {
                    schemeEditorSvcHost.Close();
                    Log.WriteAction(Localization.UseRussian ?
                        "WCF-служба остановлена" :
                        "WCF service is stopped");
                }
                catch
                {
                    schemeEditorSvcHost.Abort();
                    Log.WriteAction(Localization.UseRussian ?
                        "WCF-служба прервана" :
                        "WCF service is aborted");
                }

                schemeEditorSvcHost = null;
            }
        }*/

        static void Main(string[] args)
        {
        }
    }
}
