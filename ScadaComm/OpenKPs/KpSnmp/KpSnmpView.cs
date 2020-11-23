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
 * Module   : KpSnmp
 * Summary  : Device library user interface
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2020
 */

using Scada.Comm.Devices.KpSnmp;
using Scada.Data.Configuration;
using System;
using System.Collections.Generic;
using System.IO;

namespace Scada.Comm.Devices
{
    /// <summary>
    /// Device library user interface.
    /// <para>Пользовательский интерфейс библиотеки КП.</para>
    /// </summary>
    public class KpSnmpView : KPView
    {
        /// <summary>
        /// Версия библиотеки КП.
        /// </summary>
        internal const string KpVersion = "5.0.2.0";


        /// <summary>
        /// Конструктор для общей настройки библиотеки КП.
        /// </summary>
        public KpSnmpView()
            : this(0)
        {
        }

        /// <summary>
        /// Конструктор для настройки конкретного КП.
        /// </summary>
        public KpSnmpView(int number)
            : base(number)
        {
            CanShowProps = number > 0;
        }


        /// <summary>
        /// Описание библиотеки КП.
        /// </summary>
        public override string KPDescr
        {
            get
            {
                return Localization.UseRussian ?
                    "Взаимодействие с контроллерами по протоколу SNMP.\n\n" +
                    "Необходимо выбрать тип канала связи \"Не задан\". " +
                    "IP-адрес и порт (опционально) указываются в поле Позывной.\n\n" +
                    "Команды ТУ:\n" +
                    "Отдельная команда для установки каждой переменной. " +
                    "Номер команды равен номеру сигнала КП.\n" +
                    "Стандартная команда позволяет установить целое значение переменной.\n" +
                    "Бинарная команда имеет формат TYPE VALUE, где TYPE принимает значения:\n" +
                    "i - целое со знаком,\n" +
                    "u - мера (целое без знака),\n" +
                    "t - таймер (целое без знака),\n" +
                    "a - IP-адрес,\n" +
                    "o - идентификатор объекта (OID),\n" +
                    "s - строка,\n" +
                    "x - байты в 16-ричной форме через пробел,\n" +
                    "d - байты в десятичной форме через пробел,\n" +
                    "n - пустое значение (null)." :

                    "Interacting with controllers via SNMP protocol.\n\n" +
                    "Must be selected \"Undefined\" type of communication channel. " +
                    "IP address and port (optional) are defined in Call number field.\n\n" +
                    "Commands:\n" +
                    "A separate command for setting each variable. " +
                    "Command number is equal to a signal number of a device tag.\n" +
                    "Standard command allows to set integer variable.\n" +
                    "Binary command has the format TYPE VALUE, where TYPE is:\n" +
                    "i - signed integer,\n" +
                    "u - gauge (unsigned integer),\n" +
                    "t - time ticks (unsigned integer),\n" +
                    "a - IP address,\n" +
                    "o - object identifier (OID),\n" +
                    "s - string,\n" +
                    "x - bytes in the hexadecimal form separated by spaces,\n" +
                    "d - bytes in the decimal form separated by spaces,\n" +
                    "n - null value.";
            }
        }

        /// <summary>
        /// Получить версию библиотеки КП.
        /// </summary>
        public override string Version
        {
            get
            {
                return KpVersion;
            }
        }

        /// <summary>
        /// Получить прототипы каналов КП по умолчанию.
        /// </summary>
        public override KPCnlPrototypes DefaultCnls
        {
            get
            {
                // получение имени файла шаблона устройства
                string configFileName = KpConfig.GetFileName(AppDirs.ConfigDir, Number, KPProps?.CmdLine);
                if (!File.Exists(configFileName))
                    return null;

                // загрузка конфигурации КП
                KpConfig config = new KpConfig();
                if (!config.Load(configFileName, out string errMsg))
                    throw new Exception(errMsg);

                // создание прототипов входных каналов
                KPCnlPrototypes prototypes = new KPCnlPrototypes();
                List<InCnlPrototype> inCnls = prototypes.InCnls;
                int signal = 1;

                foreach (KpConfig.VarGroup varGroup in config.VarGroups)
                {
                    foreach (KpConfig.Variable variable in varGroup.Variables)
                    {
                        inCnls.Add(new InCnlPrototype(variable.Name, BaseValues.CnlTypes.TI) { Signal = signal++ });
                    }
                }

                return prototypes;
            }
        }

        
        /// <summary>
        /// Отобразить свойства КП.
        /// </summary>
        public override void ShowProps()
        {
            FrmConfig.ShowDialog(AppDirs, Number, KPProps?.CmdLine);
        }
    }
}
