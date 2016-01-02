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
 * Module   : KpSnmp
 * Summary  : Device library user interface
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2016
 */

using Scada.Comm.Devices.KpSnmp;
namespace Scada.Comm.Devices
{
    /// <summary>
    /// Device library user interface
    /// <para>Пользовательский интерфейс библиотеки КП</para>
    /// </summary>
    public class KpSnmpView : KPView
    {
        /// <summary>
        /// Конструктор для общей настройки библиотеки КП
        /// </summary>
        public KpSnmpView()
            : this(0)
        {
        }

        /// <summary>
        /// Конструктор для настройки конкретного КП
        /// </summary>
        public KpSnmpView(int number)
            : base(number)
        {
            CanShowProps = number > 0;
        }


        /// <summary>
        /// Описание библиотеки КП
        /// </summary>
        public override string KPDescr
        {
            get
            {
                return Localization.UseRussian ?
                    "Взаимодействие с контроллерами по протоколу SNMP v2c.\n\n" +
                    "Команды ТУ:\n" +
                    "Отдельная команда для установки каждой переменной (стандартная или бинарная). " + 
                    "Номер команды равен номеру сигнала КП." :

                    "Interacting with controllers via SNMP v2c protocol.\n\n" +
                    "Commands:\n" +
                    "A separate command for setting each variable (standard or binary). " +
                    "Command number is equal to a signal number of a device tag.";
            }
        }

        
        /// <summary>
        /// Отобразить свойства КП
        /// </summary>
        public override void ShowProps()
        {
            FrmConfig.ShowDialog(AppDirs, Number);
        }
    }
}
