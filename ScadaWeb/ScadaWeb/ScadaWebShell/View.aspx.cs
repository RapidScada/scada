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
 * Summary  : View web form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2020
 */

using Scada.UI;
using Scada.Web.Plugins;
using Scada.Web.Shell;
using System;
using System.Text;

namespace Scada.Web
{
    /// <summary>
    /// View web form
    /// <para>Веб-форма представления</para>
    /// </summary>
    public partial class WFrmView : System.Web.UI.Page
    {
        private UserData userData;       // данные пользователя приложения
        protected int initialViewID;     // ид. первоначального представления
        protected string initialViewUrl; // ссылка первоначального представления


        /// <summary>
        /// Генерировать HTML-код нижних закладок
        /// </summary>
        protected string GenerateBottomTabsHtml()
        {
            const string TabTemplate = 
                "<div class='tab{0}' data-code='{1}' data-url='{2}' data-depends='{3}'>{4}</div>";
            StringBuilder sbHtml = new StringBuilder();

            foreach (DataWndItem dataWndItem in userData.UserContent.DataWndItems)
            {
                DataWndSpec dataWndSpec = dataWndItem.DataWndSpec;
                if (dataWndSpec == null)
                    sbHtml.AppendFormat(TabTemplate, " disabled", "", "", "", dataWndItem.Text);
                else
                    sbHtml.AppendFormat(TabTemplate, "", dataWndSpec.TypeCode, ResolveUrl(dataWndItem.Url),
                        dataWndSpec.DependsOnView ? "true" : "false", dataWndItem.Text);
            }

            return sbHtml.ToString();
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            userData = UserData.GetUserData();
            userData.CheckLoggedOn(true);

            // перевод веб-страницы
            Translator.TranslatePage(Page, "Scada.Web.WFrmView");

            // получение ид. и ссылки представления для загрузки
            initialViewID = Request.QueryString.GetParamAsInt("viewID");
            ViewNode viewNode;

            if (initialViewID > 0)
            {
                viewNode = userData.UserViews.GetViewNode(initialViewID);
            }
            else
            {
                viewNode = userData.UserViews.GetFirstViewNode();
                initialViewID = viewNode == null ? 0 : viewNode.ViewID;
            }

            initialViewUrl = viewNode == null || string.IsNullOrEmpty(viewNode.ViewUrl) ?
                ResolveUrl(UrlTemplates.NoView) : viewNode.ViewUrl;

            ((MasterMain)Master).SelectedViewID = initialViewID;
        }
    }
}