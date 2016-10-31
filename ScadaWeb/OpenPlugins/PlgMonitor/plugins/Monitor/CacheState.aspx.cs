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
 * Module   : PlgMonitor
 * Summary  : Cache state web form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

using Scada.Client;
using Scada.Data.Tables;
using System;
using System.Collections;

namespace Scada.Web.Plugins.Monitor
{
    /// <summary>
    /// Cache state web form
    /// <para>Веб-форма состояния кэша</para>
    /// </summary>
    public partial class WFrmCacheState : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AppData appData = AppData.GetAppData();
            UserData userData = UserData.GetUserData();

            // проверка входа в систему и прав
            userData.CheckLoggedOn(true);

            if (!userData.UserRights.ConfigRight)
                throw new ScadaException(CommonPhrases.NoRights);

            // вывод состояния кэша таблиц часовых срезов
            const string InfoFormat = "Store period: {0}. Count: {1} / {2}. Cleaned up (UTC): {3}";

            Cache<DateTime, SrezTableLight> hourTableCache = appData.DataAccess.DataCache.HourTableCache;
            IList items = hourTableCache.GetAllItemsForWatching();
            lblHourTableCacheInfo.Text = string.Format(InfoFormat, 
                hourTableCache.StorePeriod, items.Count, hourTableCache.Capacity, hourTableCache.LastRemoveDT);

            if (items.Count > 0)
            {
                repHourTableCache.DataSource = items;
                repHourTableCache.DataBind();
            }
            else
            {
                repHourTableCache.Visible = false;
            }

            // вывод состояния кэша представлений
            Cache<int, BaseView> viewCache = appData.ViewCache.Cache;
            items = viewCache.GetAllItemsForWatching();
            lblViewCacheInfo.Text = string.Format(InfoFormat,
                viewCache.StorePeriod, items.Count, viewCache.Capacity, viewCache.LastRemoveDT);

            if (items.Count > 0)
            {
                repViewCache.DataSource = items;
                repViewCache.DataBind();
            }
            else
            {
                repViewCache.Visible = false;
            }
        }
    }
}