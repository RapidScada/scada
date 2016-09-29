/*
 * Copyright 2016 Mikhail Shiryaev
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
 * Module   : ScadaWebCommon
 * Summary  : Rights of the web application user
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2016
 */

using Scada.Client;
using Scada.Data.Models;
using Scada.Data.Tables;
using System;
using System.Collections.Generic;

namespace Scada.Web
{
    /// <summary>
    /// Rights of the web application user
    /// <para>Права пользователя веб-приложения</para>
    /// </summary>
    public class UserRights
    {
        /// <summary>
        /// Общие права пользователя на все сущности
        /// </summary>
        protected EntityRights globalRights;
        /// <summary>
        /// Права на объекты пользовательского интерфейса
        /// </summary>
        protected Dictionary<int, EntityRights> uiObjRightsDict;


        /// <summary>
        /// Конструктор
        /// </summary>
        public UserRights()
        {
            SetToDefault();
        }


        /// <summary>
        /// Получить право на просмотр всех данных
        /// </summary>
        public bool ViewAllRight { get; protected set; }

        /// <summary>
        /// Получить право на любое управление
        /// </summary>
        public bool ControlAllRight { get; protected set; }

        /// <summary>
        /// Получить право конфигурирования системы
        /// </summary>
        public bool ConfigRight { get; protected set; }


        /// <summary>
        /// Установить значения прав по умолчанию
        /// </summary>
        protected void SetToDefault()
        {
            globalRights = EntityRights.NoRights;
            uiObjRightsDict = null;

            ViewAllRight = false;
            ControlAllRight = false;
            ConfigRight = false;
        }


        /// <summary>
        /// Инициализировать только основные права пользователя, исключая права на представления и контент
        /// </summary>
        public void InitGeneralRights(int roleID)
        {
            SetToDefault();

            if (roleID == BaseValues.Roles.Admin)
            {
                ViewAllRight = true;
                ControlAllRight = true;
                ConfigRight = true;
            }
            else if (roleID == BaseValues.Roles.Dispatcher)
            {
                ViewAllRight = true;
                ControlAllRight = true;
            }
            else if (roleID == BaseValues.Roles.Guest)
            {
                ViewAllRight = true;
            }

            globalRights = new EntityRights(ViewAllRight, ControlAllRight);
        }

        /// <summary>
        /// Инициализировать права пользователя
        /// </summary>
        public void Init(int roleID, DataAccess dataAccess)
        {
            if (dataAccess == null)
                throw new ArgumentNullException("dataAccess");

            InitGeneralRights(roleID);

            if (BaseValues.Roles.Custom <= roleID && roleID < BaseValues.Roles.Err)
                uiObjRightsDict = dataAccess.GetUiObjRights(roleID);
        }

        /// <summary>
        /// Получить права на объект пользовательского интерфейса
        /// </summary>
        public EntityRights GetUiObjRights(int uiObjID)
        {
            if (uiObjID <= 0)
            {
                return EntityRights.NoRights;
            }
            else if (ViewAllRight)
            {
                return globalRights;
            }
            else
            {
                EntityRights rights;
                return uiObjRightsDict != null && uiObjRightsDict.TryGetValue(uiObjID, out rights) ?
                    rights : EntityRights.NoRights;
            }
        }
    }
}