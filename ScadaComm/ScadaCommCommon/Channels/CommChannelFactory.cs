/*
 * Copyright 2017 Mikhail Shiryaev
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
 * Summary  : Factory for creating communication channel instances
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2017
 */

using System;

namespace Scada.Comm.Channels
{
    /// <summary>
    /// Factory for creating communication channel instances
    /// <para>Фабрика для создания экземпляров классов каналов связи</para>
    /// </summary>
    public static class CommChannelFactory
    {
        /// <summary>
        /// Получить экземпляр класса логики канала связи
        /// </summary>
        public static CommChannelLogic GetCommChannel(string commCnlType)
        {
            try
            {
                if (commCnlType.Equals(CommSerialLogic.CommCnlType, StringComparison.OrdinalIgnoreCase))
                    return new CommSerialLogic();
                else if (commCnlType.Equals(CommTcpClientLogic.CommCnlType, StringComparison.OrdinalIgnoreCase))
                    return new CommTcpClientLogic();
                else if (commCnlType.Equals(CommTcpServerLogic.CommCnlType, StringComparison.OrdinalIgnoreCase))
                    return new CommTcpServerLogic();
                else if (commCnlType.Equals(CommUdpLogic.CommCnlType, StringComparison.OrdinalIgnoreCase))
                    return new CommUdpLogic();
                else
                    throw new ScadaException(Localization.UseRussian ?
                        "Неизвестный канал связи." :
                        "Unknown communication channel.");
            }
            catch (Exception ex)
            {
                throw new ScadaException(string.Format(Localization.UseRussian ?
                    "Ошибка при создании экземпляра класса канала связи {0}: {1}" :
                    "Error creating communication channel logic instance of the class {0}: {1}", 
                    commCnlType, ex.Message), ex);
            }
        }
    }
}
