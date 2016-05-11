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
 * Module   : SCADA-Web
 * Summary  : View web form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

using Scada.Web.Shell;
using System;

namespace Scada.Web
{
    /// <summary>
    /// View web form
    /// <para>Веб-форма представления</para>
    /// </summary>
    public partial class WFrmView : System.Web.UI.Page
    {
        /// <summary>
        /// Добавить на страницу скрипт загрузки представления
        /// </summary>
        private void AddLoadViewScript(string viewUrl)
        {
            ClientScript.RegisterStartupScript(GetType(), "Startup", 
                "scada.view.load('" + ResolveUrl(viewUrl) + "');", true);
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            UserData userData = UserData.GetUserData();
            userData.CheckLoggedOn(true);

            if (!IsPostBack)
            {
                // получение ид. представления для загрузки
                int viewID;
                int.TryParse(Request.QueryString["viewID"], out viewID);

                // получение ссылки представления
                string viewUrl = viewID > 0 ? 
                    userData.UserViews.GetViewUrl(viewID) :
                    userData.UserViews.GetFirstViewUrl();

                if (string.IsNullOrEmpty(viewUrl))
                    // переход на страницу отсутствующего представления
                    Response.Redirect(UrlTemplates.NoView);
                else
                    // добавление скрипта загрузки представления
                    AddLoadViewScript(viewUrl);
            }
        }
    }
}