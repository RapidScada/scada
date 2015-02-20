/*
 * Copyright 2014 Mikhail Shiryaev
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
 * Module   : KpSms
 * Summary  : Device library user interface
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2009
 * Modified : 2014
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Scada.Comm.KP
{
    /// <summary>
    /// Device library user interface
    /// <para>Пользовательский интерфейс библиотеки КП</para>
    /// </summary>
    public sealed class KpSmsView : KPView
    {
        /// <summary>
        /// Конструктор для общей настройки библиотеки КП
        /// </summary>
        public KpSmsView()
            : this(0)
        {
        }

        /// <summary>
        /// Конструктор для настройки конкретного КП
        /// </summary>
        public KpSmsView(int number)
            : base(number)
        {
            // определение каналов КП по умолчанию
            DefaultCnls = new List<InCnlProps>();
            InCnlProps inCnlProps = new InCnlProps(Localization.UseRussian ? "Связь" : "Connection", CnlType.TS);
            inCnlProps.Signal = 1;
            inCnlProps.ParamName = Localization.UseRussian ? "Связь" : "Connection";
            inCnlProps.ShowNumber = false;
            inCnlProps.UnitName = Localization.UseRussian ? "Нет - Есть" : "No - Yes";
            inCnlProps.EvEnabled = true;
            inCnlProps.EvOnChange = true;
            DefaultCnls.Add(inCnlProps);

            inCnlProps = new InCnlProps(Localization.UseRussian ? "Кол-во событий" : "Event count", CnlType.TI);
            inCnlProps.Signal = 2;
            inCnlProps.ParamName = Localization.UseRussian ? "Событие" : "Event";
            inCnlProps.DecDigits = 0;
            inCnlProps.UnitName = Localization.UseRussian ? "Шт." : "Count";
            DefaultCnls.Add(inCnlProps);

            string cnlName = Localization.UseRussian ? "Отправка SMS" : "Send SMS message";
            inCnlProps = new InCnlProps(cnlName, CnlType.TS);
            inCnlProps.CtrlCnlProps = new CtrlCnlProps(cnlName, KPLogic.CmdType.Binary);
            inCnlProps.CtrlCnlProps.CmdNum = 1;
            DefaultCnls.Add(inCnlProps);

            cnlName = Localization.UseRussian ? "AT-команда" : "AT command";
            inCnlProps = new InCnlProps(cnlName, CnlType.TS);
            inCnlProps.CtrlCnlProps = new CtrlCnlProps(cnlName, KPLogic.CmdType.Binary);
            inCnlProps.CtrlCnlProps.CmdNum = 2;
            DefaultCnls.Add(inCnlProps);

            // определение параметров опроса КП по умолчанию
            KPLogic.ReqParams reqParams = new KPLogic.ReqParams(false);
            reqParams.Timeout = 5000;
            reqParams.Delay = 500;
            DefaultReqParams = reqParams;
        }

        /// <summary>
        /// Описание библиотеки КП
        /// </summary>
        public override string KPDescr
        {
            get
            {
                return Localization.UseRussian ? 
                    "Отправка и приём SMS с использованием AT-команд.\n\n" +
                    "Параметр командной строки:\n" +
                    "primary - основной КП на линии связи, обмен данными с GSM-терминалом.\n\n" +
                    "Команды ТУ:\n" +
                    "1 (бинарная) - отправка SMS;\n" +
                    "2 (бинарная) - произвольная AT-команда." :

                    "Sending and receiving SMS messages using AT commands.\n\n" +
                    "Command line parameter:\n" +
                    "primary - main device of the communication line that communicates with GSM terminal.\n\n" +
                    "Commands:\n" +
                    "1 (binary) - send SMS message;\n" +
                    "2 (binary) - custom AT command.";
            }
        }
    }
}