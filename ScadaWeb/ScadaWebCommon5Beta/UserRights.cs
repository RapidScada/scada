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
using Scada.Data;
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
        /// Права на все представления
        /// </summary>
        protected EntityRights allViewsRights;
        /// <summary>
        /// Права на весь контент
        /// </summary>
        protected EntityRights allContentRights;
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
        /// Получить право конфигурирования системы
        /// </summary>
        public bool ConfigRight { get; protected set; }


        /// <summary>
        /// Установить значения прав по умолчанию
        /// </summary>
        protected void SetToDefault()
        {
            allViewsRights = EntityRights.NoRights;
            allContentRights = EntityRights.NoRights;
            viewRightsDict = null;
            contentRightsDict = null;

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
                allViewsRights = new EntityRights(true, true);
                allContentRights = new EntityRights(true, true);
                ConfigRight = true;
            }
            else if (roleID == BaseValues.Roles.Dispatcher)
            {
                allViewsRights = new EntityRights(true, true);
                allContentRights = new EntityRights(true, true);
            }
            else if (roleID == BaseValues.Roles.Guest)
            {
                allViewsRights = new EntityRights(true, false);
                allContentRights = new EntityRights(true, false);
            }
            else if (BaseValues.Roles.Custom <= roleID && roleID < BaseValues.Roles.Err)
            {
                viewRightsDict = dataAccess.GetViewRights(roleID);
                contentRightsDict = dataAccess.GetContentRights(roleID);
            }
        }

        /// <summary>
        /// Получить права на предсталение
        /// </summary>
        public EntityRights GetViewRights(int viewID)
        {
            if (allViewsRights.ViewRight)
            {
                return allViewsRights;
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
            if (allContentRights.ViewRight)
            {
                return allContentRights;
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