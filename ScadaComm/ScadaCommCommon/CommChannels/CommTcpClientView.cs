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
 * Summary  : TCP client communication channel user interface
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2015
 */

using System.Collections.Generic;

namespace Scada.Comm.Channels
{
    /// <summary>
    /// TCP client communication channel user interface
    /// <para>Пользовательский интерфейс канала связи TCP-клиент</para>
    /// </summary>
    public class CommTcpClientView : CommChannelView
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public CommTcpClientView()
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
                return CommTcpClientLogic.CommCnlType;
            }
        }

        /// <summary>
        /// Получить наименование канала связи
        /// </summary>
        public override string Name
        {
            get
            {
                return Localization.UseRussian ? "TCP-клиент" : "TCP client";
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
                    "Канал связи TCP-клиент.\n\n" +
                    "Параметры канала связи:\n" +
                    "IpAddress - удалённый IP-адрес в режиме соединения Shared,\n" +
                    "TcpPort - удалённый TCP-порт по умолчанию,\n" +
                    "Behavior - поведение (Master, Slave),\n" +
                    "ConnMode - режим соединения (Individual, Shared)." :

                    "TCP client communication channel.\n\n" +
                    "Communication channel parameters:\n" +
                    "IpAddress - remote IP address in Shared connection mode,\n" +
                    "TcpPort - remote TCP port by default," +
                    "Behavior - operating behavior (Master, Slave),\n" +
                    "ConnMode - connection mode (Individual, Shared).";
            }
        }


        /// <summary>
        /// Отобразить свойства модуля
        /// </summary>
        public override void ShowProps(SortedList<string, string> commCnlParams, out bool modified)
        {
            //FrmCommSerialProps.ShowDialog(commCnlParams, out modified);
            (new FrmCommTcpClientProps()).ShowDialog();
            modified = false;
        }

        /// <summary>
        /// Получить информацию о свойствах канала связи
        /// </summary>
        public override string GetPropsInfo(SortedList<string, string> commCnlParams)
        {
            CommTcpClientLogic.Settings defSett = new CommTcpClientLogic.Settings();
            return BuildPropsInfo(commCnlParams,
                new string[] { "IpAddress", "TcpPort", "Behavior", "ConnMode" },
                new object[] { defSett.IpAddress, defSett.TcpPort, defSett.Behavior, defSett.ConnMode });
        }
    }
}
