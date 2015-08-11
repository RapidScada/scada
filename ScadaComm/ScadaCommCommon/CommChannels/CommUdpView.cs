/*
 * Copyright 2015 Mikhail Shiryaev
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
 * Module   : ScadaCommCommon
 * Summary  : UDP communication channel user interface
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2015
 */

using System.Collections.Generic;

namespace Scada.Comm.Channels
{
    /// <summary>
    /// UDP communication channel user interface
    /// <para>Пользовательский интерфейс канала связи UDP</para>
    /// </summary>
    public class CommUdpView : CommChannelView
    {
        /// <summary>
        /// Получить наименование канала связи
        /// </summary>
        public override string Name
        {
            get
            {
                return "UDP";
            }
        }

        /// <summary>
        /// Получить описание канала связи
        /// </summary>
        public override string Descr
        {
            get
            {
                return Localization.UseRussian ?
                    "Канал связи UDP.\n\n" +
                    "Параметры канала связи:\n" +
                    "LocalUdpPort - локальный UDP-порт для входящих соединений,\n" +
                    "RemoteUdpPort - удалённый UDP-порт единый для всех устройств в режиме Master,\n" +
                    "RemoteIpAddress - удалённый IP-адрес по умолчанию,\n" +
                    "Behavior - режим работы канала связи (Master, Slave),\n" +
                    "DevSelMode - режим выбора КП в режиме работы Slave (ByIPAddress, ByDeviceLibrary)." :

                    "UDP communication channel.\n\n" +
                    "Communication channel parameters:\n" +
                    "LocalUdpPort - local UDP port for incoming connections." +
                    "RemoteUdpPort - remote UDP port common for all the devices in Master mode,\n" +
                    "RemoteIpAddress - remote IP address by default,\n" +
                    "Behavior - work mode of connection channel (Master, Slave),\n" +
                    "DevSelMode - device selection mode in Slave work mode (ByIPAddress, ByDeviceLibrary).";
            }
        }

        /// <summary>
        /// Получить информацию о свойствах канала связи
        /// </summary>
        public override string GetPropsInfo(Dictionary<string, string> commCnlParams)
        {
            CommUdpLogic.Settings defSett = new CommUdpLogic.Settings();
            return BuildPropsInfo(commCnlParams,
                new string[] { "LocalUdpPort", "RemoteUdpPort", "Behavior", "DevSelMode" },
                new object[] { defSett.LocalUdpPort, defSett.RemoteUdpPort, defSett.Behavior, defSett.DevSelMode });
        }
    }
}
