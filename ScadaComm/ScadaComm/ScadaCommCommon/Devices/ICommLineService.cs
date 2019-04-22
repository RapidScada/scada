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
 * Module   : ScadaCommCommon
 * Summary  : Communication line service interface provided for device logic instances
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2019
 */

using Scada.Client;
using Scada.Data.Models;

namespace Scada.Comm.Devices
{
    /// <summary>
    /// Communication line service interface provided for device logic instances
    /// <para>Интерфейс сервиса линии связи, предоставляемый объектам логики КП</para>
    /// </summary>
    public interface ICommLineService
    {
        /// <summary>
        /// Получить номер линии связи
        /// </summary>
        int Number { get; }

        /// <summary>
        /// Gets the client to communicate with Server.
        /// </summary>
        ServerComm ServerComm { get; }


        /// <summary>
        /// Найти КП на линии связи по адресу и позывному
        /// </summary>
        /// <remarks>Если address меньше 0, то он не учитывается при поиске.
        /// Если позывной равен null, то он не учитывается при поиске.</remarks>
        KPLogic FindKPLogic(int address, string callNum);

        /// <summary>
        /// Форсированно передать текущие данные SCADA-Серверу
        /// </summary>
        bool FlushCurData(KPLogic kpLogic);

        /// <summary>
        /// Форсированно передать архивные данные и события SCADA-Серверу
        /// </summary>
        bool FlushArcData(KPLogic kpLogic);

        /// <summary>
        /// Передать команду ТУ, адресованную КП на данной линии связи
        /// </summary>
        void PassCmd(Command cmd);
    }
}
