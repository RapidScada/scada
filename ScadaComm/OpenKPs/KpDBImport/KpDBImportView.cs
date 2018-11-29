/*
 * Copyright 2018 Mikhail Shiryaev
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
 * Modified : 2018
 */

using Scada.Comm.Devices.DbImport.UI;

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
        internal const string KpVersion = "5.0.0.0";


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
                    "Импорт из сторонней базы данных." :
                    "Import from a third-party database.";
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
        /// Shows the driver properties.
        /// </summary>
        public override void ShowProps()
        {
            new FrmConfig(AppDirs, Number).ShowDialog();
        }
    }
}
