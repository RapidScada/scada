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
 * Summary  : TCP server communication layer user interface
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2015
 */

using System.Collections.Generic;

namespace Scada.Comm.Layers
{
    /// <summary>
    /// TCP server communication layer user interface
    /// <para>Пользовательский интерфейс слоя связи TCP-сервер</para>
    /// </summary>
    public class CommTcpServerView : CommLayerView
    {
        /// <summary>
        /// Получить наименование слоя связи
        /// </summary>
        public override string Name
        {
            get
            {
                return Localization.UseRussian ? "TCP-сервер" : "TCP server";
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
                    "Слой связи TCP-сервер.\n\n" +
                    "Параметры слоя связи:\n" +
                    "TcpPort - TCP-порт для входящих соединений,\n" +
                    "InactiveTime - время неактивности соединения до отключения, с,\n" +
                    "Behavior - режим работы слоя связи (Master, Slave),\n" +
                    "ConnMode - режим соединения (Individual, Shared)," +
                    "DevSelMode - режим выбора КП в режиме соединения Individual " + 
                    "(ByIPAddress, ByFirstPackage, ByDeviceLibrary)." :

                    "TCP server communication layer.\n\n" +
                    "Communication layer parameters:\n" +
                    "TcpPort - TCP port for incoming connections," +
                    "InactiveTime - duration of inactivity before disconnect, sec,\n" +
                    "Behavior - work mode of connection layer (Master, Slave),\n" +
                    "DevSelMode - device selection mode in Individual connection mode " + 
                    "(ByIPAddress, ByFirstPackage, ByDeviceLibrary).";
            }
        }

        /// <summary>
        /// Получить информацию о свойствах слоя связи
        /// </summary>
        public override string GetPropsInfo(Dictionary<string, string> layerParams)
        {
            CommTcpServerLogic.Settings defSett = new CommTcpServerLogic.Settings();
            return BuildPropsInfo(layerParams, 
                new string[] { "TcpPort", "InactiveTime", "Behavior", "DevSelMode" },
                new object[] { defSett.TcpPort, defSett.InactiveTime, defSett.Behavior, defSett.DevSelMode });
        }
    }
}
