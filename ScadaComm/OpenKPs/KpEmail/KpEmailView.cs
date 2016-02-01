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
 * Module   : KpEmail
 * Summary  : Device library user interface
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

using Scada.Comm.Devices.AddressBook;
using Scada.Comm.Devices.KpEmail;

namespace Scada.Comm.Devices
{
    /// <summary>
    /// Device library user interface
    /// <para>Пользовательский интерфейс библиотеки КП</para>
    /// </summary>
    public class KpEmailView : KPView
    {
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
                    "1 (бинарная) - отправка уведомления.\n\n" +
                    "Примеры текста команды:\n" +
                    "group_name;subject;message\n" +
                    "contact_name;subject;message\n" +
                    "email;subject;message" :

                    "Sending email notifications.\n\n" +
                    "Commands:\n" +
                    "1 (binary) - send the notification.\n\n" +
                    "Command text examples:\n" +
                    "group_name;subject;message\n" +
                    "contact_name;subject;message\n" +
                    "email;subject;message";
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
