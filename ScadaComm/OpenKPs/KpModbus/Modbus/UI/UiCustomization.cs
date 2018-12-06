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
 * Module   : KpModbus
 * Summary  : Makes the driver UI flexible
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using Scada.Comm.Devices.Modbus.Protocol;

namespace Scada.Comm.Devices.Modbus.UI
{
    /// <summary>
    /// Makes the driver UI flexible.
    /// <para>Обеспечивает гибкость пользовательского интерфейса драйвера.</para>
    /// </summary>
    public class UiCustomization
    {
        /// <summary>
        /// Gets the device template factory.
        /// </summary>
        public virtual DeviceTemplateFactory TemplateFactory
        {
            get
            {
                return KpUtils.TemplateFactory;
            }
        }

        /// <summary>
        /// Gets a value indicating whether to display the extended settings button.
        /// </summary>
        public virtual bool ExtendedSettingsAvailable
        {
            get
            {
                return false;
            }
        }


        /// <summary>
        /// Shows the extended settings form as a modal dialog box.
        /// </summary>
        /// <returns>Returns true if the settings changed.</returns>
        public virtual bool ShowExtendedSettings(DeviceTemplate deviceTemplate)
        {
            return false;
        }
    }
}
