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
 * Summary  : Main master page
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

using Scada.Web.Shell;
using System;
using System.Text;

namespace Scada.Web
{
    /// <summary>
    /// Main master page
    /// <para>Основная страница-шаблон</para>
    /// </summary>
    public partial class MasterMain : System.Web.UI.MasterPage
    {
        private AppData appData;   // общие данные веб-приложения
        private UserData userData; // данные пользователя приложения


        /// <summary>
        /// Генерировать HTML-код меню
        /// </summary>
        protected string GenerateMenuHtml()
        {
            const string ItemTemplate = "<div class='{0}'>{1}{2}</div>";
            const string LinkTemplate = "<a href='{0}'>{1}</a>";
            const string Expander = "<div class='expander collapsed'></div>";

            StringBuilder sbHtml = new StringBuilder();
            string curUrl = Request.Url.AbsolutePath;

            foreach (MenuItem menuItem in userData.UserMenu.LinearMenuItems)
            {
                string text = Server.HtmlEncode(menuItem.Text);
                string url = ResolveUrl(menuItem.Url);
                bool topLevel = menuItem.Level <= 0;
                bool selected = string.Equals(url, curUrl, StringComparison.OrdinalIgnoreCase);
                bool containsSubitems = topLevel && menuItem.Subitems.Count > 0;

                string cssClass = (topLevel ? "menu-item" : "menu-subitem") + (selected ? " selected" : "");
                string linkOrText = containsSubitems || !string.IsNullOrEmpty(url) ? 
                    string.Format(LinkTemplate, url, text) : text;
                string expander = containsSubitems ? Expander : "";

                sbHtml.AppendLine(string.Format(ItemTemplate, cssClass, linkOrText, expander));
            }

            return sbHtml.ToString();
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            appData = AppData.GetAppData();
            userData = UserData.GetUserData();

            // проверка входа в систему
            userData.CheckLoggedOn(true);
        }

        protected void lbtnMainLogout_Click(object sender, EventArgs e)
        {
            // выход из системы
            userData.Logout();
            appData.RememberMe.ForgetUser(Context);
            Response.Redirect("~/Login.aspx");
        }
    }
}