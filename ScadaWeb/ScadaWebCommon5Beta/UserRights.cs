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
        /// Получить права на все представления
        /// </summary>
        public EntityRights AllViewsRights { get; protected set; }

        /// <summary>
        /// Получить права на весь контент
        /// </summary>
        public EntityRights AllContentRights { get; protected set; }

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

            AllViewsRights = EntityRights.NoRights;
            AllContentRights = EntityRights.NoRights;
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
                AllViewsRights = new EntityRights(true, true);
                AllContentRights = new EntityRights(true, true);
                ConfigRight = true;
            }
            else if (roleID == BaseValues.Roles.Dispatcher)
            {
                AllViewsRights = new EntityRights(true, true);
                AllContentRights = new EntityRights(true, true);
            }
            else if (roleID == BaseValues.Roles.Guest)
            {
                AllViewsRights = new EntityRights(true, false);
                AllContentRights = new EntityRights(true, false);
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
            if (AllViewsRights.ViewRight)
            {
                return AllViewsRights;
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
            if (AllContentRights.ViewRight)
            {
                return AllContentRights;
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