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
 * Summary  : Device request parameters
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2015
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scada.Comm.Devices
{
    /// <summary>
    /// Device request parameters
    /// <para>Параметры опроса КП</para>
    /// </summary>
    public struct KPReqParams
    {
        /// <summary>
        /// Параметры опроса КП по умолчанию
        /// </summary>
        public static readonly KPReqParams Default = new KPReqParams(1000, 200);

        /// <summary>
        /// Нулевые параметры опроса КП
        /// </summary>
        public static readonly KPReqParams Zero = new KPReqParams(0, 0);


        /// <summary>
        /// Получить или установить таймаут запросов, мс
        /// </summary>
        public int Timeout { get; set; }

        /// <summary>
        /// Получить или установить задержку после запросов, мс
        /// </summary>
        public int Delay { get; set; }

        /// <summary>
        /// Получить или установить время опроса
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// Получить или установить период опроса
        /// </summary>
        public TimeSpan Period { get; set; }

        /// <summary>
        /// Получить или установить командную строку
        /// </summary>
        public string CmdLine { get; set; }


        /// <summary>
        /// Конструктор
        /// </summary>
        public KPReqParams(int timeout, int delay)
            : this()
        {
            Timeout = timeout;
            Delay = delay;
            Time = DateTime.MinValue;
            Period = TimeSpan.Zero;
            CmdLine = "";
        }

        /// <summary>
        /// Получить аргументы командной строки
        /// </summary>
        public string[] GetCmdLineArgs()
        {
            return CmdLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
