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
 * Module   : Communicator Shell
 * Summary  : Converts entities to Communicator settings
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
 * Modified : 2019
 */

using Scada.Data.Tables;
using System;
using Entities = Scada.Data.Entities;

namespace Scada.Comm.Shell.Code
{
    /// <summary>
    /// Converts entities to Communicator settings.
    /// <para>Преобразует сущности в настройки Коммуникатора.</para>
    /// </summary>
    public static class SettingsConverter
    {
        /// <summary>
        /// Creates a communication line settings based on the entity.
        /// </summary>
        public static Settings.CommLine CreateCommLine(Entities.CommLine commLineEntity)
        {
            if (commLineEntity == null)
                throw new ArgumentNullException("commLineEntity");

            return new Settings.CommLine
            {
                Number = commLineEntity.CommLineNum,
                Name = commLineEntity.Name
            };
        }

        /// <summary>
        /// Creates a device settings based on the entity.
        /// </summary>
        public static Settings.KP CreateKP(Entities.KP kpEntity, BaseTable<Entities.KPType> kpTypeTable)
        {
            if (kpEntity == null)
                throw new ArgumentNullException("kpEntity");

            Settings.KP kpSettings = new Settings.KP() { Number = kpEntity.KPNum };
            Copy(kpEntity, kpSettings, kpTypeTable);
            return kpSettings;
        }

        /// <summary>
        /// Copies properties from the device entity to the device settings.
        /// </summary>
        public static void Copy(Entities.KP srcKP, Settings.KP destKP, BaseTable<Entities.KPType> kpTypeTable)
        {
            if (srcKP == null)
                throw new ArgumentNullException("srcKP");
            if (destKP == null)
                throw new ArgumentNullException("destKP");

            destKP.Name = srcKP.Name;
            destKP.Dll = kpTypeTable != null && 
                kpTypeTable.Items.TryGetValue(srcKP.KPTypeID, out Entities.KPType kpType) ?
                kpType.DllFileName : "";
            destKP.Address = srcKP.Address ?? 0;
            destKP.CallNum = srcKP.CallNum;
        }
    }
}
