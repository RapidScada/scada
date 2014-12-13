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
 * Summary  : Login checking web form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2012
 * Modified : 2013
 */

using System;

namespace Scada.Web
{
    /// <summary>
    /// Login checking web form
    /// <para>Веб-форма для проверка входа в систему</para>
    /// </summary>
    public partial class WFrmLoginChecker : System.Web.UI.Page
    {
        /// <summary>
        /// Таймаут обновления данной страницы
        /// </summary>
        private const string RefreshTimeout = "5000";

        /// <summary>
        /// Добавить скрипт переадресации на страницу входа в систему при загрузке
        /// </summary>
        private void AddLoginScript()
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Startup1",
                "if (window.parent) window.parent.location = 'Login.aspx';", true);
        }

        /// <summary>
        /// Добавить скрипт обновления данной страницы по таймауту
        /// </summary>
        private void AddRefreshScript()
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Startup2",
                "setTimeout('window.location = window.location', " + RefreshTimeout + ");", true);
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            // отключение кэширования страницы
            ScadaUtils.DisablePageCache(Response);

            // получение данных пользователя
            UserData userData = UserData.GetUserData();

            // проверка входа в систему
            userData.CheckLoggedOn(Context, false);
            if (userData.LoggedOn)
                AddRefreshScript();
            else
                AddLoginScript();
        }
    }
}