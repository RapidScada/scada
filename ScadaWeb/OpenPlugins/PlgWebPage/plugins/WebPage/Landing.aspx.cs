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
 * Module   : PlgWebForm
 * Summary  : Load the web page specified by the view
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

using Scada.Data.Models;
using Scada.Web.Shell;
using System;

namespace Scada.Web.Plugins.WebPage
{
    /// <summary>
    /// Load the page specified by the view
    /// <para>Загружает веб-страницу, заданную для представления </para>
    /// </summary>
    public partial class WFrmLanding : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AppData appData = AppData.GetAppData();
            UserData userData = UserData.GetUserData();

            // получение ид. представления из параметров запроса
            int viewID;
            int.TryParse(Request.QueryString["viewID"], out viewID);
            
            // проверка прав на просмотр представления
            EntityRights rights = userData.LoggedOn ?
                userData.UserRights.GetViewRights(viewID) : EntityRights.NoRights;
            if (!rights.ViewRight)
                Response.Redirect(UrlTemplates.NoView);

            // загрузка представления
            WebPageView view = appData.ViewCache.GetView<WebPageView>(viewID);

            // переход на соответствующую веб-страницу
            Response.Redirect(view == null ? UrlTemplates.NoView : view.ItfObjName);
        }
    }
}