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
 * Summary  : The class contains utility methods for the driver
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

namespace Scada.Comm.Devices.Modbus
{
    /// <summary>
    /// The class contains utility methods for the driver.
    /// <para>Класс, содержащий вспомогательные методы драйвера.</para>
    /// </summary>
    internal static class KpUtils
    {
        /// <summary>
        /// The default device template factory.
        /// </summary>
        public static readonly DeviceTemplateFactory TemplateFactory = new DeviceTemplateFactory();
    }
}
