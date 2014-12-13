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
 * Summary  : WCF service for cross domain access
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2012
 * Modified : 2012
 */

using System;
using System.IO;
using System.ServiceModel.Channels;
using System.Xml;

namespace Scada.Scheme.Editor
{
    /// <summary>
    /// WCF service for cross domain access
    /// <para>WCF-служба для кросс-доменного доступа</para>
    /// </summary>
    public class CrossDomainSvc : ICrossDomainSvc
    {
        private const string ClientAccessPolicy =
            "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
            "<access-policy>" +
            "<cross-domain-access>" +
            "<policy>" +
            "<allow-from http-request-headers=\"*\">" +
            "<domain uri=\"*\"/>" +
            "</allow-from>" +
            "<grant-to>" +
            "<resource path=\"/\" include-subpaths=\"true\"/>" +
            "</grant-to>" +
            "</policy>" +
            "</cross-domain-access>" +
            "</access-policy>";

        public Message ProvidePolicyFile()
        {
            StringReader stringReader = new StringReader(ClientAccessPolicy);
            XmlReader xmlReader = XmlReader.Create(stringReader);
            return Message.CreateMessage(MessageVersion.None, "", xmlReader);
        }
    }
}
