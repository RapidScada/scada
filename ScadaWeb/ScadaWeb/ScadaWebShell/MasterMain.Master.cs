/*
 * Copyright 2021 Mikhail Shiryaev
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
 * Modified : 2021
 */

using Scada.UI;
using Scada.Web.Shell;
using System;

namespace Scada.Web
{
    /// <summary>
    /// Main master page.
    /// <para>Основная страница-шаблон.</para>
    /// </summary>
    public partial class MasterMain : System.Web.UI.MasterPage
    {
        // Дерево представлений
        private const string FolderImageUrl = "~/images/treeview/folder.png";
        private const string DocumentImageUrl = "~/images/treeview/document.png";
        private static readonly TreeViewRenderer treeViewRenderer = new TreeViewRenderer();

        private AppData appData;     // общие данные веб-приложения
        protected UserData userData; // данные пользователя приложения


        /// <summary>
        /// Ид. выбранного представления
        /// </summary>
        public int SelectedViewID = 0;


        /// <summary>
        /// Gets a user profile URL.
        /// </summary>
        private string GetUserProfileUrl()
        {
            string userProfileUrl = userData.WebSettings.ScriptPaths.UserProfilePath;

            if (string.IsNullOrEmpty(userProfileUrl))
                userProfileUrl = UrlTemplates.DefaultUserProfile;

            return string.Format(userProfileUrl, userData.UserProps.UserID);
        }

        /// <summary>
        /// Генерировать HTML-код дополнительных скриптов
        /// </summary>
        protected string GenScriptPathsHtml()
        {
            return userData.WebSettings.ScriptPaths.GenerateHtml();
        }

        /// <summary>
        /// Генерировать HTML-код главного меню
        /// </summary>
        protected string GenMainMenuHtml()
        {
            TreeViewRenderer.Options options = new TreeViewRenderer.Options() { ShowIcons = false };
            return treeViewRenderer.GenerateHtml(userData.UserMenu.MenuItems, Request.Url.PathAndQuery, options);
        }

        /// <summary>
        /// Генерировать HTML-код проводника представлений
        /// </summary>
        protected string GenExplorerHtml()
        {
            TreeViewRenderer.Options options = new TreeViewRenderer.Options()
            {
                ShowIcons = true,
                FolderImageUrl = ResolveUrl(FolderImageUrl),
                DocumentImageUrl = ResolveUrl(DocumentImageUrl)
            };
            return treeViewRenderer.GenerateHtml(userData.UserViews.ViewNodes, SelectedViewID, options);
        }

        /// <summary>
        /// Генерировать HTML-код для передачи обезличенной статистики команде разработчиков 
        /// </summary>
        protected string GenStatsHtml()
        {
            return appData.Stats.GenerateHtml(userData.WebSettings.ShareStats, Request.IsSecureConnection);
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            appData = AppData.GetAppData();
            userData = UserData.GetUserData();
            userData.CheckLoggedOn(true);

            if (!IsPostBack)
            {
                // перевод веб-страницы
                Translator.TranslatePage(Page, "Scada.Web.MasterMain");

                // настройка элементов управления
                lblProductName.Text = CommonPhrases.ProductName;
                hlMainUser.Text = userData.UserProps.UserName;
                hlMainUser.NavigateUrl = GetUserProfileUrl();
            }
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