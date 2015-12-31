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

using Scada.Comm.Channels.UI;
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
        /// Конструктор
        /// </summary>
        public CommUdpView()
        {
            CanShowProps = true;
        }

        
        /// <summary>
        /// Получить наименование типа канала связи
        /// </summary>
        public override string TypeName
        {
            get
            {
                return CommUdpLogic.CommCnlType;
            }
        }

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
                    "RemoteUdpPort - удалённый UDP-порт одинаковый для всех устройств,\n" +
                    "RemoteIpAddress - удалённый IP-адрес по умолчанию,\n" +
                    "Behavior - поведение (Master, Slave),\n" +
                    "DevSelMode - режим выбора КП в режиме Slave (ByIPAddress, ByDeviceLibrary)." :

                    "UDP communication channel.\n\n" +
                    "Communication channel parameters:\n" +
                    "LocalUdpPort - local UDP port for incoming connections." +
                    "RemoteUdpPort - remote UDP port the same for all the devices,\n" +
                    "RemoteIpAddress - remote IP address by default,\n" +
                    "Behavior - operating behavior (Master, Slave),\n" +
                    "DevSelMode - device selection mode in Slave mode (ByIPAddress, ByDeviceLibrary).";
            }
        }


        /// <summary>
        /// Установить параметры канала связи по умолчанию
        /// </summary>
        public override void SetCommCnlParamsToDefault(SortedList<string, string> commCnlParams)
        {
            CommUdpLogic.Settings settings = new CommUdpLogic.Settings();
            settings.SetCommCnlParams(commCnlParams);
        }

        /// <summary>
        /// Отобразить свойства модуля
        /// </summary>
        public override void ShowProps(SortedList<string, string> commCnlParams, out bool modified)
        {
            FrmCommUdpProps.ShowDialog(commCnlParams, out modified);
        }

        /// <summary>
        /// Получить информацию о свойствах канала связи
        /// </summary>
        public override string GetPropsInfo(SortedList<string, string> commCnlParams)
        {
            CommUdpLogic.Settings defSett = new CommUdpLogic.Settings();
            return BuildPropsInfo(commCnlParams,
                new string[] { "LocalUdpPort", "RemoteUdpPort", "Behavior", "DevSelMode" },
                new object[] { defSett.LocalUdpPort, defSett.RemoteUdpPort, defSett.Behavior, defSett.DevSelMode });
        }
    }
}
