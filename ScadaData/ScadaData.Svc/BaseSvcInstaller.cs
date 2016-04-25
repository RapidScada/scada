/*
 * Copyright 2016 Mikhail Shiryaev
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
 * Module   : ScadaData.Svc
 * Summary  : The base class for Windows service installer
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2007
 * Modified : 2016
 */

using System;
using System.Configuration.Install;
using System.IO;
using System.Reflection;
using System.ServiceProcess;
using System.Xml;

namespace Scada.Svc
{
    /// <summary>
    /// The base class for Windows service installer
    /// <para>Базовый класс установщика службы Windows</para>
    /// </summary>
    public abstract class BaseSvcInstaller : Installer
    {
        /// <summary>
        /// Имя файла, содержащего свойства службы
        /// </summary>
        private const string SvcPropsFileName = "svc_config.xml";


        /// <summary>
        /// Загрузить свойства службы
        /// </summary>
        private bool LoadSeriveProps(out string svcName, out string svcDescr)
        {
            try
            {
                string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string fileName = path + Path.DirectorySeparatorChar + SvcPropsFileName;

                if (File.Exists(fileName))
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(fileName);

                    svcName = xmlDoc.DocumentElement.GetChildAsString("ServiceName");
                    svcDescr = xmlDoc.DocumentElement.GetChildAsString("Description");
                    return true;
                }
                else
                {
                    svcName = "";
                    svcDescr = "";
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error loading service properties", ex);
            }
        }

        /// <summary>
        /// Инициализировать установщик службы
        /// </summary>
        protected void Init(string defSvcName, string defDescr)
        {
            string svcName;
            string svcDescr;

            if (!LoadSeriveProps(out svcName, out svcDescr))
            {
                svcName = defSvcName;
                svcDescr = defDescr;
            }

            if (string.IsNullOrEmpty(svcName))
                throw new ScadaException("Service name must not be null or empty.");

            ServiceInstaller serviceInstaller = new ServiceInstaller();
            ServiceProcessInstaller serviceProcessInstaller = new ServiceProcessInstaller();

            serviceInstaller.ServiceName = svcName;
            serviceInstaller.DisplayName = svcName;
            serviceInstaller.Description = svcDescr ?? "";
            serviceInstaller.StartType = ServiceStartMode.Automatic;

            serviceProcessInstaller.Account = ServiceAccount.LocalSystem;
            serviceProcessInstaller.Password = null;
            serviceProcessInstaller.Username = null;

            Installers.AddRange(new Installer[] {serviceInstaller, serviceProcessInstaller});
        }
    }
}
