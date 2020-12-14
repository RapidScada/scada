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
 * Module   : KpEmail
 * Summary  : Device library user interface
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2020
 */

using Scada.Comm.Devices.AB;
using Scada.Comm.Devices.KpEmail;
using Scada.Data.Configuration;

namespace Scada.Comm.Devices
{
    /// <summary>
    /// Device library user interface
    /// <para>Пользовательский интерфейс библиотеки КП</para>
    /// </summary>
    public class KpEmailView : KPView
    {
        /// <summary>
        /// Версия библиотеки КП
        /// </summary>
        internal const string KpVersion = "5.0.4.0";


        /// <summary>
        /// Конструктор для общей настройки библиотеки КП
        /// </summary>
        public KpEmailView()
            : this(0)
        {
        }

        /// <summary>
        /// Конструктор для настройки конкретного КП
        /// </summary>
        public KpEmailView(int number)
            : base(number)
        {
            CanShowProps = true;
        }


        /// <summary>
        /// Описание библиотеки КП
        /// </summary>
        public override string KPDescr
        {
            get
            {
                return Localization.UseRussian ?
                    "Отправка уведомлений по электронной почте.\n\n" +
                    "Команды ТУ:\n" +
                    "1 (бинарная) - отправка электронного письма;\n" +
                    "2 (бинарная) - отправка электронного письма с вложениями.\n\n" +
                    "Примеры текста команды:\n" +
                    "имя_группы;тема;сообщение;вложения\n" +
                    "имя_контакта;тема;сообщение;вложения\n" +
                    "эл_почта;тема;сообщение;вложения\n" +
                    "вложения - это список путей к файлам, разделенных запятыми.":

                    "Sending email notifications.\n\n" +
                    "Commands:\n" +
                    "1 (binary) - send the email;\n" +
                    "2 (binary) - send the email with attachments.\n\n" +
                    "Command text examples:\n" +
                    "group_name;subject;message;attachments\n" +
                    "contact_name;subject;message;attachments\n" +
                    "email;subject;message;attachments\n" +
                    "attachments is a comma-separated list of file paths.";
            }
        }

        /// <summary>
        /// Получить версию библиотеки КП
        /// </summary>
        public override string Version
        {
            get
            {
                return KpVersion;
            }
        }

        /// <summary>
        /// Получить прототипы каналов КП по умолчанию
        /// </summary>
        public override KPCnlPrototypes DefaultCnls
        {
            get
            {
                KPCnlPrototypes prototypes = new KPCnlPrototypes();

                prototypes.CtrlCnls.Add(new CtrlCnlPrototype(
                    Localization.UseRussian ? "Отправка письма" : "Send email",
                    BaseValues.CmdTypes.Binary)
                {
                    CmdNum = 1
                });

                prototypes.InCnls.Add(new InCnlPrototype(
                    Localization.UseRussian ? "Отправлено писем" : "Sent emails",
                    BaseValues.CnlTypes.TI)
                {
                    Signal = 1,
                    DecDigits = 0,
                    UnitName = BaseValues.UnitNames.Pcs,
                    CtrlCnlProps = prototypes.CtrlCnls[0]
                });

                return prototypes;
            }
        }

        /// <summary>
        /// Получить параметры опроса КП по умолчанию
        /// </summary>
        public override KPReqParams DefaultReqParams
        {
            get
            {
                return new KPReqParams(10000, 200);
            }
        }

        /// <summary>
        /// Отобразить свойства КП
        /// </summary>
        public override void ShowProps()
        {
            if (Number > 0)
                // отображение формы настройки свойств КП
                FrmConfig.ShowDialog(AppDirs, Number);
            else
                // отображение адресной книги
                FrmAddressBook.ShowDialog(AppDirs);
        }
    }
}
