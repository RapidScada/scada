/*
 * Copyright 2020 Mikhail Shiryaev
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
 * Module   : KpHttpNotif
 * Summary  : Device driver user interface
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2020
 */

using Scada.Comm.Devices.AB;
using Scada.Comm.Devices.HttpNotif.UI;
using Scada.Data.Configuration;
using Scada.UI;

namespace Scada.Comm.Devices
{
    /// <summary>
    /// Device driver user interface.
    /// <para>Пользовательский интерфейс драйвера КП.</para>
    /// </summary>
    public class KpHttpNotifView : KPView
    {
        /// <summary>
        /// The driver version.
        /// </summary>
        internal const string KpVersion = "5.0.1.0";


        /// <summary>
        /// Initializes a new instance of the class. Designed for general configuring.
        /// </summary>
        public KpHttpNotifView()
            : this(0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the class. Designed for configuring a particular device.
        /// </summary>
        public KpHttpNotifView(int number)
            : base(number)
        {
            CanShowProps = true;
        }


        /// <summary>
        /// Gets the driver description.
        /// </summary>
        public override string KPDescr
        {
            get
            {
                return Localization.UseRussian ?
                    "Отправка уведомлений с помощью HTTP-запросов.\n\n" +
                    "Команды ТУ:\n" +
                    "1 (бинарная) - отправка уведомления.\n" +
                    "Примеры текста команды:\n" +
                    "имя_группы;сообщение\n" +
                    "имя_контакта;сообщение\n" +
                    "эл_почта;сообщение\n\n" +
                    "2 (бинарная) - отправка произвольного запроса.\n" +
                    "Текст команды содержит аргументы:\n" +
                    "arg1=value1\\n\n" +
                    "arg2=value2\n" :

                    "Sending notifications via HTTP requests.\n\n" +
                    "Commands:\n" +
                    "1 (binary) - send notification.\n" +
                    "Command text examples:\n" +
                    "group_name;message\n" +
                    "contact_name;message\n" +
                    "email;message\n\n" +
                    "2 (binary) - send custom request.\n" +
                    "Command text contains arguments:\n" +
                    "arg1=value1\\n\n" +
                    "arg2=value2\n";

            }
        }

        /// <summary>
        /// Gets the driver version.
        /// </summary>
        public override string Version
        {
            get
            {
                return KpVersion;
            }
        }

        /// <summary>
        /// Gets the default channel prototypes.
        /// </summary>
        public override KPCnlPrototypes DefaultCnls
        {
            get
            {
                KPCnlPrototypes prototypes = new KPCnlPrototypes();

                // output channels
                prototypes.CtrlCnls.Add(new CtrlCnlPrototype(
                    Localization.UseRussian ? "Отправка уведомления" : "Send notification",
                    BaseValues.CmdTypes.Binary)
                {
                    CmdNum = 1
                });

                prototypes.CtrlCnls.Add(new CtrlCnlPrototype(
                    Localization.UseRussian ? "Отправка запроса" : "Send request",
                    BaseValues.CmdTypes.Binary)
                {
                    CmdNum = 2
                });

                // input channels
                prototypes.InCnls.Add(new InCnlPrototype(
                    Localization.UseRussian ? "Отправлено уведомлений" : "Notifications sent",
                    BaseValues.CnlTypes.TI)
                {
                    Signal = 1,
                    DecDigits = 0,
                    UnitName = BaseValues.UnitNames.Pcs,
                    CtrlCnlProps = prototypes.CtrlCnls[0]
                });

                prototypes.InCnls.Add(new InCnlPrototype(
                    Localization.UseRussian ? "Статус ответа" : "Response status",
                    BaseValues.CnlTypes.TI)
                {
                    Signal = 2,
                    DecDigits = 0
                });

                return prototypes;
            }
        }

        /// <summary>
        /// Gets the default request parameters.
        /// </summary>
        public override KPReqParams DefaultReqParams
        {
            get
            {
                return new KPReqParams(10000, 200);
            }
        }


        /// <summary>
        /// Shows the driver properties.
        /// </summary>
        public override void ShowProps()
        {
            if (Number > 0)
            {
                // show configuration form
                new FrmConfig(AppDirs, Number)
                {
                    DefaultUri = KPProps.CmdLine
                }
                .ShowDialog();
            }
            else
            {
                // show address book
                FrmAddressBook.ShowDialog(AppDirs);
            }
        }
    }
}
