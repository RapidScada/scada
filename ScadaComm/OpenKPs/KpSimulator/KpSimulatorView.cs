/*
 * Copyright 2019 Mikhail Shiryaev
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
 * Module   : KpSimulator
 * Summary  : Device driver user interface
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
 * Modified : 2019
 */

using Scada.Data.Configuration;
using System.Collections.Generic;

namespace Scada.Comm.Devices
{
    /// <summary>
    /// Device driver user interface.
    /// <para>Пользовательский интерфейс драйвера КП.</para>
    /// </summary>
    public class KpSimulatorView : KPView
    {
        /// <summary>
        /// The driver version.
        /// </summary>
        internal const string KpVersion = "5.0.0.0";


        /// <summary>
        /// Initializes a new instance of the class. Designed for general configuring.
        /// </summary>
        public KpSimulatorView()
            : this(0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the class. Designed for configuring a particular device.
        /// </summary>
        public KpSimulatorView(int number)
            : base(number)
        {
        }


        /// <summary>
        /// Gets the driver description.
        /// </summary>
        public override string KPDescr
        {
            get
            {
                return Localization.UseRussian ?
                    "Симулятор устройства.\n\n" +
                    "Команды ТУ:\n" +
                    "4 (бинарная) - установить состояние реле;\n" +
                    "5 (бинарная) - установить аналоговый выход." :

                    "Device simulator.\n\n" +
                    "Commands:\n" +
                    "4 (standard) - set relay state;\n" +
                    "5 (standard) - set analog output.";
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
        /// Gets the channel prototypes.
        /// </summary>
        public override KPCnlPrototypes DefaultCnls
        {
            get
            {
                KPCnlPrototypes prototypes = new KPCnlPrototypes();
                List<InCnlPrototype> inCnls = prototypes.InCnls;
                List<CtrlCnlPrototype> ctrlCnls = prototypes.CtrlCnls;

                // output channels
                ctrlCnls.Add(new CtrlCnlPrototype("Set Relay State", BaseValues.CmdTypes.Standard)
                {
                    CmdNum = 4,
                    CmdVal = BaseValues.CmdValNames.OffOn
                });

                ctrlCnls.Add(new CtrlCnlPrototype("Set Analog Output", BaseValues.CmdTypes.Standard)
                {
                    CmdNum = 5
                });

                // input channels
                inCnls.Add(new InCnlPrototype("Sine", BaseValues.CnlTypes.TI)
                {
                    Signal = 1
                });

                inCnls.Add(new InCnlPrototype("Square", BaseValues.CnlTypes.TS)
                {
                    Signal = 2,
                    ShowNumber = false,
                    UnitName = BaseValues.UnitNames.OffOn
                });

                inCnls.Add(new InCnlPrototype("Triangle", BaseValues.CnlTypes.TI)
                {
                    Signal = 3
                });

                inCnls.Add(new InCnlPrototype("Relay State", BaseValues.CnlTypes.TS)
                {
                    Signal = 4,
                    ShowNumber = false,
                    UnitName = BaseValues.UnitNames.OffOn,
                    CtrlCnlProps = ctrlCnls[0]
                });

                inCnls.Add(new InCnlPrototype("Analog Output", BaseValues.CnlTypes.TI)
                {
                    Signal = 5,
                    DecDigits = 1,
                    CtrlCnlProps = ctrlCnls[1]
                });

                return prototypes;
            }
        }

        /// <summary>
        /// Gets the default device request parameters.
        /// </summary>
        public override KPReqParams DefaultReqParams
        {
            get
            {
                return new KPReqParams(0, 1000);
            }
        }
    }
}
