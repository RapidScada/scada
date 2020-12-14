/*
 * Copyright 2020 Mikhail Shiryaev
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
 * Summary  : Global web application events processing
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2005
 * Modified : 2020
 */

using Scada.UI;
using System;
using System.Web;
using Utils;

namespace Scada.Web
{
    /// <summary>
    /// Global web application events processing.
    /// <para>Обработка глобальных событий веб-приложения.</para>
    /// </summary>
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            // инициализация общих данных веб-приложения
            AppData.GetAppData().Init(Server.MapPath("~"));
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            // добавление информации о пользователе 
            AppData.GetAppData().UserMonitor.AddUser(UserData.GetUserData());
            // setup the translator
            Translator.AppendProductName = true;
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            if (!WebUtils.IsAjaxRequest(HttpContext.Current.Request))
                Server.Transfer("~/Error.aspx");
        }

        protected void Session_End(object sender, EventArgs e)
        {
            // удаление информации о пользователе
            AppData.GetAppData().UserMonitor.RemoveUser(Session.SessionID);
        }

        protected void Application_End(object sender, EventArgs e)
        {
            Log log = AppData.GetAppData().Log;
            log.WriteAction(Localization.UseRussian ?
                "Завершение работы приложения" :
                "Application shutdown");
            log.WriteBreak();
        }
    }
}