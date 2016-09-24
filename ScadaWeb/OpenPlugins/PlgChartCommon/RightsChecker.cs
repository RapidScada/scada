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
 * Module   : PlgChartCommon
 * Summary  : Checks access rights for a chart
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

using Scada.Client;
using System;

namespace Scada.Web.Plugins.Chart
{
    /// <summary>
    /// Checks access rights for a chart
    /// <para>Проверяет права доступа к графику</para>
    /// </summary>
    public class RightsChecker
    {
        /// <summary>
        /// Кэш представлений
        /// </summary>
        protected readonly ViewCache viewCache;

        /// <summary>
        /// Данные пользователя приложения
        /// </summary>
        protected UserData userData;
        /// <summary>
        /// Права пользователя веб-приложения
        /// </summary>
        protected UserRights userRights;


        /// <summary>
        /// Конструктор, ограничивающий создание объекта без параметров
        /// </summary>
        protected RightsChecker()
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public RightsChecker(ViewCache viewCache)
        {
            if (viewCache == null)
                throw new ArgumentNullException("viewCache");

            this.viewCache = viewCache;
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
        /// Получить представление по идентификатору
        /// </summary>
        protected BaseView GetView(int viewID)
        {
#if DEBUG
            return null;
#else
            if (userData == null)
            {
                // получение представления из кеша для WCF-сервиса
                return viewCache.GetViewFromCache(viewID, true);
            }
            else
            {
                // получение представления из кеша или от сервера для веб-формы
                Type viewType = userData.UserViews.GetViewType(viewID);
                return viewCache.GetView(viewType, viewID, true);
            }
#endif
        }

        /// <summary>
        /// Проверить права на представления и принадлежность каналов
        /// </summary>
        protected void CheckRights(int[] cnlNums, int[] viewIDs, out BaseView singleView)
        {
            if (ElementsEqual(viewIDs))
            {
                CheckRights(cnlNums, viewIDs[0], out singleView);
            }
            else
            {
                singleView = null;
                int cnlCnt = cnlNums.Length;
                for (int i = 0; i < cnlCnt; i++)
                    CheckRights(cnlNums[i], viewIDs[i]);
            }
        }

        /// <summary>
        /// Проверить права на одно представление и принадлежность каналов
        /// </summary>
        protected void CheckRights(int[] cnlNums, int viewID, out BaseView singleView)
        {
            if (!userRights.GetUiObjRights(viewID).ViewRight)
                throw new ScadaException(CommonPhrases.NoRights);

#if DEBUG
            singleView = null;
#else
            singleView = GetView(viewID);
            if (!singleView.ContainsAllCnls(cnlNums))
                throw new ScadaException(CommonPhrases.NoRights);
#endif
        }

        /// <summary>
        /// Проверить права на одно представление и принадлежность одного канала
        /// </summary>
        protected void CheckRights(int cnlNum, int viewID)
        {
            if (!userRights.GetUiObjRights(viewID).ViewRight)
                throw new ScadaException(CommonPhrases.NoRights);

#if !DEBUG
            BaseView view = GetView(viewID);
            if (!view.ContainsCnl(cnlNum))
                throw new ScadaException(CommonPhrases.NoRights);
#endif
        }


        /// <summary>
        /// Проверить заданные права на возможность просмотра входных каналов, входящих в указанные представления
        /// </summary>
        /// <remarks>Если проверка не пройдена, вызывается исключение</remarks>
        public void CheckRights(UserRights userRights, int[] cnlNums, int[] viewIDs)
        {
            if (userRights == null)
                throw new ArgumentNullException("userRights");

            ChartUtils.CheckArrays(cnlNums, viewIDs);
            userData = null;
            this.userRights = userRights;

            if (!userRights.ViewAllRight)
            {
                BaseView singleView;
                CheckRights(cnlNums, viewIDs, out singleView);
            }
        }

        /// <summary>
        /// Проверить права текущего пользователя на просмотр входных каналов, входящих в указанные представления,
        /// а также получить представление, если оно единственное
        /// </summary>
        /// <remarks>Если проверка не пройдена, вызывается исключение</remarks>
        public void CheckRights(UserData userData, int[] cnlNums, int[] viewIDs, out BaseView singleView)
        {
            if (userData == null)
                throw new ArgumentNullException("userData");
            if (!userData.LoggedOn)
                throw new ScadaException(CommonPhrases.NoRights);
            ChartUtils.CheckArrays(cnlNums, viewIDs);

            this.userData = userData;
            userRights = userData.UserRights;

            if (userRights.ViewAllRight)
            {
                singleView = ElementsEqual(viewIDs) ? GetView(viewIDs[0]) : null;
            }
            else
            {
                CheckRights(cnlNums, viewIDs, out singleView);
            }
        }
    }
}