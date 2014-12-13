/*
 * Copyright 2014 Mikhail Shiryaev
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
 * Module   : SCADA-Scheme Editor
 * Summary  : Interface of the WCF service for cross domain access
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2012
 * Modified : 2012
 */

using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;

namespace Scada.Scheme.Editor
{
    /// <summary>
    /// Interface of the WCF service for cross domain access
    /// Интерфейс WCF-службы для кросс-доменного доступа
    /// </summary>
    [ServiceContract]
    public interface ICrossDomainSvc
    {
        [OperationContract]
        [WebGet(UriTemplate = "clientaccesspolicy.xml")]
        Message ProvidePolicyFile();
    }
}
