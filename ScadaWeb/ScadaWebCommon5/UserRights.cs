/*
 * Copyright 2018 Mikhail Shiryaev
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
 * Summary  : Application user rights
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2018
 */

using Scada.Client;
using Scada.Data.Configuration;
using Scada.Data.Models;
using System;
using System.Collections.Generic;

namespace Scada.Web
{
    /// <summary>
    /// Application user rights
    /// <para>Права пользователя приложения</para>
    /// </summary>
    public class UserRights
    {
        /// <summary>
        /// Кэш представлений
        /// </summary>
        protected readonly ViewCache viewCache;

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
            : this(null)
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public UserRights(ViewCache viewCache)
        {
            this.viewCache = viewCache;
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
        /// Проверить корректность массивов номеров входных каналов и ид. представлений
        /// </summary>
        protected void CheckArrays(int[] cnlNums, int[] viewIDs)
        {
            if (cnlNums == null)
                throw new ArgumentNullException("cnlNums");

            if (viewIDs == null)
                throw new ArgumentNullException("viewIDs");

            if (cnlNums.Length == 0)
                throw new ArgumentException(WebPhrases.CnlNumsEmptyError);

            if (cnlNums.Length != viewIDs.Length)
                throw new ScadaException(WebPhrases.CnlsViewsCountError);
        }

        /// <summary>
        /// Проверить, что все элементы непустого массива равны
        /// </summary>
        protected bool ElementsEqual(int[] arr)
        {
            int first = arr[0];
            int len = arr.Length;
            for (int i = 1; i < len; i++)
                if (arr[i] != first)
                    return false;
            return true;
        }

        /// <summary>
        /// Проверить права на входные каналы, которые относятся к указанным представлениям
        /// </summary>
        protected bool CheckRights(int[] cnlNums, int[] viewIDs, out int singleViewID)
        {
            if (ElementsEqual(viewIDs))
            {
                singleViewID = viewIDs[0];
                return CheckRights(cnlNums, singleViewID);
            }
            else
            {
                singleViewID = -1;

                for (int i = 0, cnt = cnlNums.Length; i < cnt; i++)
                {
                    if (!CheckRights(cnlNums[i], viewIDs[i]))
                        return false;
                }

                return true;
            }
        }

        /// <summary>
        /// Проверить права на входной канал, который относится к указанному представлению
        /// </summary>
        protected bool CheckRights(int cnlNum, int viewID)
        {
            if (GetUiObjRights(viewID).ViewRight)
            {
#if DEBUG
                return true;
#else
                BaseView view = viewCache == null ? null : viewCache.GetViewFromCache(viewID);
                return view != null && view.ContainsCnl(cnlNum);
#endif
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Проверить права на входные каналы, все из которых относятся к указанному представлению
        /// </summary>
        protected bool CheckRights(int[] cnlNums, int viewID)
        {
            if (GetUiObjRights(viewID).ViewRight)
            {
#if DEBUG
                return true;
#else
                BaseView view = viewCache == null ? null : viewCache.GetViewFromCache(viewID);
                return view != null && view.ContainsAllCnls(cnlNums);
#endif
            }
            else
            {
                return false;
            }
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
            if (ViewAllRight)
            {
                return globalRights;
            }
            else if (uiObjID <= 0)
            {
                return EntityRights.NoRights;
            }
            else
            {
                return uiObjRightsDict != null && uiObjRightsDict.TryGetValue(uiObjID, out EntityRights rights) ?
                    rights : EntityRights.NoRights;
            }
        }

        /// <summary>
        /// Проверить права на входные каналы, которые относятся к указанным представлениям
        /// </summary>
        public bool CheckInCnlRights(int[] cnlNums, int[] viewIDs)
        {
            CheckArrays(cnlNums, viewIDs);

            if (ViewAllRight)
            {
                return true;
            }
            else
            {
                int singleViewID;
                return CheckRights(cnlNums, viewIDs, out singleViewID);
            }
        }

        /// <summary>
        /// Проверить права на входные каналы, которые относятся к указанным представлениям,
        /// и вернуть ид. представления, если оно единственное
        /// </summary>
        public bool CheckInCnlRights(int[] cnlNums, int[] viewIDs, out int singleViewID)
        {
            CheckArrays(cnlNums, viewIDs);

            if (ViewAllRight)
            {
                singleViewID = ElementsEqual(viewIDs) ? viewIDs[0] : -1;
                return true;
            }
            else
            {
                return CheckRights(cnlNums, viewIDs, out singleViewID);
            }
        }
    }
}