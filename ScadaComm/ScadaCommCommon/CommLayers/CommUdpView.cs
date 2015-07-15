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
 * Summary  : UDP communication layer user interface
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2015
 */

using System.Collections.Generic;

namespace Scada.Comm.Layers
{
    /// <summary>
    /// UDP communication layer user interface
    /// <para>Пользовательский интерфейс слоя связи UDP</para>
    /// </summary>
    public class CommUdpView : CommLayerView
    {
        /// <summary>
        /// Получить наименование слоя связи
        /// </summary>
        public override string Name
        {
            get
            {
                return "UDP";
            }
        }

        /// <summary>
        /// Получить описание слоя связи
        /// </summary>
        public override string Descr
        {
            get
            {
                return Localization.UseRussian ?
                    "Слой связи UDP.\n\n" +
                    "Параметры слоя связи:\n" +
                    "LocalUdpPort - локальный UDP-порт для входящих соединений,\n" +
                    "RemoteUdpPort - удалённый UDP-порт для всех устройств в режиме Master,\n" +
                    "Behavior - режим работы слоя связи (Master, Slave),\n" +
                    "DevSelMode - режим выбора КП (ByIPAddress, ByDeviceLibrary)." :

                    "UDP communication layer.\n\n" +
                    "Communication layer parameters:\n" +
                    "LocalUdpPort - local UDP port for incoming connections." +
                    "RemoteUdpPort - remote UDP port for all the devices in Master mode,\n" +
                    "Behavior - work mode of connection layer (Master, Slave),\n" +
                    "DevSelMode - device selection mode (ByIPAddress, ByDeviceLibrary).";
            }
        }

        /// <summary>
        /// Получить информацию о свойствах слоя связи
        /// </summary>
        public override string GetPropsInfo(Dictionary<string, string> layerParams)
        {
            CommUdpLogic.Settings defSett = new CommUdpLogic.Settings();
            return BuildPropsInfo(layerParams,
                new string[] { "LocalUdpPort", "RemoteUdpPort", "Behavior", "DevSelMode" },
                new object[] { defSett.LocalUdpPort, defSett.RemoteUdpPort, defSett.Behavior, defSett.DevSelMode });
        }
    }
}
