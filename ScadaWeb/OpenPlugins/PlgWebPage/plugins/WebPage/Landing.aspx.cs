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
 * Module   : PlgWebForm
 * Summary  : Load the web page specified by the view
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2020
 */

using Scada.Data.Models;
using Scada.UI;
using Scada.Web.Shell;
using System;
using System.Text;

namespace Scada.Web.Plugins.WebPage
{
    /// <summary>
    /// Loads a view for later navigation to the specified page.
    /// <para>Загружает представление для последующего перехода на заданную страницу.</para>
    /// </summary>
    public partial class WFrmLanding : System.Web.UI.Page
    {
        protected StringBuilder sbClientScript; // the script to add to a web page

        protected void Page_Load(object sender, EventArgs e)
        {
            AppData appData = AppData.GetAppData();
            UserData userData = UserData.GetUserData();

            // translate the web page
            Translator.TranslatePage(Page, typeof(WFrmLanding).FullName);

            // получение ид. представления из параметров запроса
            int viewID = Request.QueryString.GetParamAsInt("viewID");
            
            // проверка прав на просмотр представления
            EntityRights rights = userData.LoggedOn ?
                userData.UserRights.GetUiObjRights(viewID) : EntityRights.NoRights;

            if (!rights.ViewRight)
                Response.Redirect(UrlTemplates.NoView);

            // load view
            WebPageView webPageView = appData.ViewCache.GetView<WebPageView>(viewID);

            if (webPageView == null)
                Response.Redirect(UrlTemplates.NoView);

            appData.AssignStamp(webPageView);

            // set the page title
            Title = webPageView.Title + " - " + CommonPhrases.ProductName;

            // build client script
            sbClientScript = new StringBuilder();

            sbClientScript
                .AppendLine($"var viewPath = '{webPageView.Path}';")
                .AppendLine();
        }
    }
}
