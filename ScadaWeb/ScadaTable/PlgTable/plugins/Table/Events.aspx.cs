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
 * Module   : PlgTable
 * Summary  : Events web form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

using Scada.Client;
using Scada.UI;
using Scada.Web.Shell;
using System;

namespace Scada.Web.Plugins.Table
{
    /// <summary>
    /// Events web form
    /// <para>Веб-форма событий</para>
    /// </summary>
    public partial class WFrmEvents : System.Web.UI.Page
    {
        // Переменные для вывода на веб-страницу
        protected bool debugMode = false; // режим отладки
        protected int viewID;             // ид. представления
        protected int dataRefrRate;       // частота обновления текущих данных
        protected int arcRefrRate;        // частота обновления архивных данных
        protected string phrases;         // локализованные фразы
        protected string today;           // текущая дата
        protected int dispEventCnt;       // количество отображаемых событий
        protected bool viewAllRight;      // право на просмотр всех данных

        protected void Page_Load(object sender, EventArgs e)
        {
            AppData appData = AppData.GetAppData();
            UserData userData = UserData.GetUserData();

#if DEBUG
            debugMode = true;
            userData.LoginForDebug();
#endif

            // перевод веб-страницы
            Translator.TranslatePage(Page, "Scada.Web.Plugins.Table.WFrmEvents");

            // получение ид. представления из параметров запроса
            viewID = Request.QueryString.GetParamAsInt("viewID");

            // проверка прав на просмотр представления
            if (!(userData.LoggedOn && userData.UserRights.GetUiObjRights(viewID).ViewRight))
                Response.Redirect(UrlTemplates.NoView);

            // загрузка представления в кэш для последующего получения событий через API
            BaseView view = userData.UserViews.GetView(viewID);
            if (view == null)
                Response.Redirect(UrlTemplates.NoView);

            // подготовка данных для вывода на веб-страницу
            dataRefrRate = userData.WebSettings.DataRefrRate;
            arcRefrRate = userData.WebSettings.ArcRefrRate;
            phrases = WebUtils.DictionaryToJs("Scada.Web.Plugins.Table.WFrmEvents.Js");

            DateTime nowDT = DateTime.Now;
            today = string.Format("new Date({0}, {1}, {2})", nowDT.Year, nowDT.Month - 1, nowDT.Day);

            dispEventCnt = userData.WebSettings.DispEventCnt;
            viewAllRight = userData.UserRights.ViewAllRight;
        }
    }
}