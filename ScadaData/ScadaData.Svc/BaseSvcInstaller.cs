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
 * Created  : 2016
 * Modified : 2016
 */

using System;
using System.Configuration.Install;
using System.ServiceProcess;

namespace Scada.Svc
{
    /// <summary>
    /// The base class for Windows service installer
    /// <para>Базовый класс установщика службы Windows</para>
    /// </summary>
    public abstract class BaseSvcInstaller : Installer
    {
        /// <summary>
        /// Инициализировать установщик службы
        /// </summary>
        protected void Init(string defSvcName, string defDescr)
        {
            // загрузка и проверка свойств службы
            SvcProps svcProps = new SvcProps();

            if (!svcProps.LoadFromFile())
            {
                svcProps.ServiceName = defSvcName;
                svcProps.Description = defDescr;
            }

            if (string.IsNullOrEmpty(svcProps.ServiceName))
                throw new ScadaException(SvcProps.ServiceNameEmptyError);

            // настройка установщика
            ServiceInstaller serviceInstaller = new ServiceInstaller();
            ServiceProcessInstaller serviceProcessInstaller = new ServiceProcessInstaller();

            serviceInstaller.ServiceName = svcProps.ServiceName;
            serviceInstaller.DisplayName = svcProps.ServiceName;
            serviceInstaller.Description = svcProps.Description ?? "";
            serviceInstaller.StartType = ServiceStartMode.Automatic;

            serviceProcessInstaller.Account = ServiceAccount.LocalSystem;
            serviceProcessInstaller.Password = null;
            serviceProcessInstaller.Username = null;

            Installers.AddRange(new Installer[] {serviceInstaller, serviceProcessInstaller});
        }
    }
}
