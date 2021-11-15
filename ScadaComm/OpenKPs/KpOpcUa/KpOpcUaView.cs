/*
 * Copyright 2019 Mikhail Shiryaev
 * All rights reserved
 * 
 * Product  : Rapid SCADA
 * Module   : KpOpcUa
 * Summary  : Device driver user interface
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
 * Modified : 2019
 */

using Scada.Comm.Devices.OpcUa.Config;
using Scada.Comm.Devices.OpcUa.UI;
using Scada.Data.Configuration;
using System.Collections.Generic;
using System.IO;

namespace Scada.Comm.Devices
{
    /// <summary>
    /// Device driver user interface.
    /// <para>Пользовательский интерфейс драйвера КП.</para>
    /// </summary>
    public class KpOpcUaView : KPView
    {
        /// <summary>
        /// The driver version.
        /// </summary>
        internal const string KpVersion = "5.0.0.2";


        /// <summary>
        /// Initializes a new instance of the class. Designed for general configuring.
        /// </summary>
        public KpOpcUaView()
            : this(0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the class. Designed for configuring a particular device.
        /// </summary>
        public KpOpcUaView(int number)
            : base(number)
        {
            CanShowProps = number > 0;
        }


        /// <summary>
        /// Gets the driver description.
        /// </summary>
        public override string KPDescr
        {
            get
            {
                return Localization.UseRussian ?
                    "Взаимодействие с контроллерами по спецификации OPC UA.\n\n" +
                    "Команды ТУ:\n" +
                    "определяются конфигурацией КП (только стандартные)." :

                    "Interacting with controllers according to the OPC UA specification.\n\n" +
                    "Commands:\n" +
                    "defined by device configuration (only standard).";
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
                // load configuration
                DeviceConfig deviceConfig = new DeviceConfig();
                string fileName = DeviceConfig.GetFileName(AppDirs.ConfigDir, Number);

                if (!File.Exists(fileName))
                    return null;
                else if (!deviceConfig.Load(fileName, out string errMsg))
                    throw new ScadaException(errMsg);

                // create channel prototypes
                KPCnlPrototypes prototypes = new KPCnlPrototypes();
                List<InCnlPrototype> inCnls = prototypes.InCnls;
                List<CtrlCnlPrototype> ctrlCnls = prototypes.CtrlCnls;

                // input channels
                int signal = 1;
                foreach (SubscriptionConfig subscriptionConfig in deviceConfig.Subscriptions)
                {
                    foreach (ItemConfig itemConfig in subscriptionConfig.Items)
                    {
                        string tagNamePrefix = string.IsNullOrEmpty(itemConfig.DisplayName) ?
                            (Localization.UseRussian ? "Безымянный тег" : "Unnamed tag") : 
                            itemConfig.DisplayName;
                        bool isArray = itemConfig.IsArray;
                        int tagCntByItem = isArray && itemConfig.ArrayLen > 0 ? itemConfig.ArrayLen : 1;

                        for (int k = 0; k < tagCntByItem; k++)
                        {
                            if (itemConfig.CnlNum <= 0)
                            {
                                string tagName = isArray ? tagNamePrefix + "[" + k + "]" : tagNamePrefix;
                                inCnls.Add(new InCnlPrototype(tagName, BaseValues.CnlTypes.TI) { Signal = signal });
                            }
                            signal++;
                        }

                    }
                }

                // output channels
                foreach (CommandConfig commandConfig in deviceConfig.Commands)
                {
                    ctrlCnls.Add(new CtrlCnlPrototype(commandConfig.DisplayName, BaseValues.CmdTypes.Standard)
                    {
                        CmdNum = commandConfig.CmdNum
                    });
                }

                return prototypes;
            }
        }


        /// <summary>
        /// Shows the driver properties.
        /// </summary>
        public override void ShowProps()
        {
            new FrmConfig(AppDirs, Number).ShowDialog();
        }
    }
}
