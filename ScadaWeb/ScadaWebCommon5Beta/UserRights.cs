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
        /// Права на предсталения
        /// </summary>
        protected Dictionary<int, EntityRights> viewRightsDict;
        /// <summary>
        /// Права на контент
        /// </summary>
        protected Dictionary<string, EntityRights> contentRightsDict;


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
            viewRightsDict = null;
            contentRightsDict = null;
            globalRights = EntityRights.NoRights;

            ViewAllRight = false;
            ControlAllRight = false;
            ConfigRight = false;
        }

        /// <summary>
        /// Инициализировать права пользователя
        /// </summary>
        public void Init(int roleID, DataAccess dataAccess)
        {
            if (dataAccess == null)
                throw new ArgumentNullException("dataAccess");

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
            else if (BaseValues.Roles.Custom <= roleID && roleID < BaseValues.Roles.Err)
            {
                viewRightsDict = dataAccess.GetViewRights(roleID);
                contentRightsDict = dataAccess.GetContentRights(roleID);
            }

            globalRights = new EntityRights(ViewAllRight, ControlAllRight);
        }

        /// <summary>
        /// Получить права на предсталение
        /// </summary>
        public EntityRights GetViewRights(int viewID)
        {
            if (viewID <= 0)
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
                return viewRightsDict != null && viewRightsDict.TryGetValue(viewID, out rights) ?
                    rights : EntityRights.NoRights;
            }
        }

        /// <summary>
        /// Получить права на контент
        /// </summary>
        public EntityRights GetContentRights(string contentTypeCode)
        {
            if (string.IsNullOrEmpty(contentTypeCode))
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
                return contentRightsDict != null && contentRightsDict.TryGetValue(contentTypeCode, out rights) ?
                    rights : EntityRights.NoRights;
            }
        }
    }
}