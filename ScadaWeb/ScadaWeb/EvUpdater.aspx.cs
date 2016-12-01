/*
 * Copyright 2014 Mikhail Shiryaev
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
 * Module   : SCADA-Web
 * Summary  : Updating event table control web form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2012
 * Modified : 2012
 */

using System;
using Scada.Data;

namespace Scada.Web
{
    /// <summary>
    /// Updating event table control web form
    /// <para>Веб-форма, управляющая обновлением таблицы событий</para>
    /// </summary>
    public partial class WFrmEvUpdater : System.Web.UI.Page
    {
        /// <summary>
        /// Добавить скрипт переадресации на страницу входа в систему при загрузке
        /// </summary>
        private void AddLoginScript()
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Startup1",
                "if (window.parent && window.parent.parent) window.parent.parent.location = 'Login.aspx';", true);
        }

        /// <summary>
        /// Добавить скрипт обновления данной страницы по таймауту
        /// </summary>
        private void AddRefreshScript(int timeout)
        {
            if (timeout > 0)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Startup2",
                    "setTimeout('window.location = window.location', " + timeout + ");", true);
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            // отключение кэширования страницы
            ScadaUtils.DisablePageCache(Response);

            // получение данных пользователя
            UserData userData = UserData.GetUserData();

            // проверка входа в систему
            userData.CheckLoggedOn(Context, false);            
            if (!userData.LoggedOn)
            {
                AddLoginScript();
                return;
            }

            // определение даты запрашиваемых событий
            DateTime reqDate;
            try 
            {
                reqDate = new DateTime(int.Parse(Request["year"]), 
                    int.Parse(Request["month"]), int.Parse(Request["day"])); 
            }
            catch 
            { 
                reqDate = DateTime.MinValue; 
            }

            // обновление событий, получение их временной метки
            if (reqDate > DateTime.MinValue)
            {
                EventTableLight eventTable;
                AppData.MainData.RefreshEvents(reqDate, out eventTable);
                hidEvStamp.Value = eventTable.FileModTime.Ticks.ToString();
            }

            // добавление скрипта обновления данной страницы
            AddRefreshScript(AppData.WebSettings.EventRefrFreq * 1000);
        }
    }
}