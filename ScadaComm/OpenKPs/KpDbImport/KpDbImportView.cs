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
 * Module   : KpDBImport
 * Summary  : Device library user interface
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2020
 */

using Scada.Comm.Devices.DbImport.Configuration;
using Scada.Comm.Devices.DbImport.UI;
using Scada.Data.Configuration;
using System.IO;

namespace Scada.Comm.Devices
{
    /// <summary>
    /// Device library user interface.
    /// <para>Пользовательский интерфейс библиотеки КП.</para>
    /// </summary>
    public class KpDbImportView : KPView
    {
        /// <summary>
        /// The driver version.
        /// </summary>
        internal const string KpVersion = "5.0.3.0";


        /// <summary>
        /// Initializes a new instance of the class. Designed for general configuring.
        /// </summary>
        public KpDbImportView()
            : this(0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the class. Designed for configuring a particular device.
        /// </summary>
        public KpDbImportView(int number)
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
                    "Импорт из сторонней базы данных.\n\n" +
                    "Команды определяются в конфигурации КП.\n" +
                    "Пример команды:\n" +
                    "UPDATE table SET column1 = @cmdVal WHERE column2 = @cmdNum" :

                    "Import from a third-party database.\n\n" +
                    "Commands are defined in the device configuration.\n" +
                    "Command example:\n" +
                    "UPDATE table SET column1 = @cmdVal WHERE column2 = @cmdNum";
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
        /// Gets the default request parameters.
        /// </summary>
        public override KPReqParams DefaultReqParams
        {
            get
            {
                return new KPReqParams(0, 500);
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
                KpConfig config = new KpConfig();
                string fileName = KpConfig.GetFileName(AppDirs.ConfigDir, Number);

                if (!File.Exists(fileName))
                    return null;
                else if (!config.Load(fileName, out string errMsg))
                    throw new ScadaException(errMsg);

                // create channel prototypes
                KPCnlPrototypes prototypes = new KPCnlPrototypes();
                string[] tagNames = KpDbImportLogic.GetTagNames(config);
                int signal = 1;

                foreach (string tagName in tagNames)
                {
                    prototypes.InCnls.Add(new InCnlPrototype(tagName, BaseValues.CnlTypes.TI) { Signal = signal++ });
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
