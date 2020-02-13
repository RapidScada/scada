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
 * Module   : ScadaServerCommon
 * Summary  : Interface that defines access to server data
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2020
 */

using Scada.Data.Tables;
using System;
using System.Collections.Generic;

namespace Scada.Server.Modules
{
    /// <summary>
    /// Interface that defines access to server data
    /// <para>Интерфейс, определяющий доступ к данным сервера</para>
    /// </summary>
    public interface IServerData
    {
        /// <summary>
        /// Gets active channel numbers.
        /// </summary>
        int[] GetCnlNums();

        /// <summary>
        /// Получить текущий срез, содержащий данные заданных каналов
        /// </summary>
        /// <remarks>Номера каналов должны быть упорядочены по возрастанию</remarks>
        SrezTableLight.Srez GetCurSnapshot(int[] cnlNums);

        /// <summary>
        /// Получить срез, содержащий данные заданных каналов
        /// </summary>
        /// <remarks>Номера каналов должны быть упорядочены по возрастанию</remarks>
        SrezTableLight.Srez GetSnapshot(DateTime dateTime, SnapshotType snapshotType, int[] cnlNums);

        /// <summary>
        /// Gets timestamps of snapshots available on the specified date.
        /// </summary>
        DateTime[] GetAvailableSnapshots(DateTime date, SnapshotType snapshotType);

        /// <summary>
        /// Обработать (записать) новые текущие данные
        /// </summary>
        bool ProcCurData(SrezTableLight.Srez snapshot);

        /// <summary>
        /// Обработать (записать) новые архивные данные
        /// </summary>
        bool ProcArcData(SrezTableLight.Srez snapshot);

        /// <summary>
        /// Обработать (записать) новое событие
        /// </summary>
        bool ProcEvent(EventTableLight.Event ev);
    }
}
