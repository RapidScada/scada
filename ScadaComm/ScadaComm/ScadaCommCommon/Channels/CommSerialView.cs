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
 * Summary  : Serial port communication channel user interface
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
    /// Serial port communication channel user interface
    /// <para>Пользовательский интерфейс канала связи через последовательный порт</para>
    /// </summary>
    public class CommSerialView : CommChannelView
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public CommSerialView()
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
                return CommSerialLogic.CommCnlType;
            }
        }

        /// <summary>
        /// Получить наименование канала связи
        /// </summary>
        public override string Name
        {
            get
            {
                return Localization.UseRussian ? "Последовательный порт" : "Serial port";
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
                    "Канал связи через последовательный порт.\n\n" +
                    "Параметры канала связи:\n" +
                    "PortName - имя последовательного порта (например, COM1),\n" +
                    "BaudRate - скорость обмена по порту,\n" +
                    "DataBits - биты данных (5, 6, 7, 8),\n" +
                    "Parity - контроль чётности (None, Odd, Even, Mark, Space),\n" +
                    "StopBits - стоповые биты (None, One, Two, OnePointFive),\n" +
                    "DtrEnable - использование сигнала DTR (false, true),\n" +
                    "RtsEnable - использование сигнала RTS (false, true),\n" +
                    "Behavior - поведение (Master, Slave)." :

                    "Serial port communication channel.\n\n" +
                    "Communication channel parameters:\n" +
                    "PortName - serial port name (for example, COM1),\n" +
                    "BaudRate - serial baud rate,\n" +
                    "DataBits - length of data bits per byte (5, 6, 7, 8),\n" +
                    "Parity - parity-checking protocol (None, Odd, Even, Mark, Space),\n" +
                    "StopBits -  number of stopbits per byte (None, One, Two, OnePointFive),\n" +
                    "DtrEnable - value that enables the DTR signal (false, true),\n" +
                    "RtsEnable - value that enables the RTS signal (false, true),\n" +
                    "Behavior - operating behavior (Master, Slave).";
            }
        }


        /// <summary>
        /// Установить параметры канала связи по умолчанию
        /// </summary>
        public override void SetCommCnlParamsToDefault(SortedList<string, string> commCnlParams)
        {
            CommSerialLogic.Settings settings = new CommSerialLogic.Settings();
            settings.SetCommCnlParams(commCnlParams);
        }

        /// <summary>
        /// Отобразить свойства модуля
        /// </summary>
        public override void ShowProps(SortedList<string, string> commCnlParams, out bool modified)
        {
            FrmCommSerialProps.ShowDialog(commCnlParams, out modified);
        }

        /// <summary>
        /// Получить информацию о свойствах канала связи
        /// </summary>
        public override string GetPropsInfo(SortedList<string, string> commCnlParams)
        {
            CommSerialLogic.Settings defSett = new CommSerialLogic.Settings();
            return BuildPropsInfo(commCnlParams,
                new string[] { "PortName", "BaudRate", "DataBits", "Parity", "StopBits", 
                    "DtrEnable", "RtsEnable", "Behavior" },
                new object[] { defSett.PortName, defSett.BaudRate, defSett.DataBits, defSett.Parity, defSett.StopBits,
                    defSett.DtrEnable, defSett.RtsEnable, defSett.Behavior });
        }
    }
}
