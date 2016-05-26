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
 * Module   : PlgSchemeCommon
 * Summary  : Scheme layout web form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

//#define DEBUG_MODE

using Scada.Scheme;
using Scada.UI;
using Scada.Web.Shell;
using System;

namespace Scada.Web.Plugins.Scheme
{
    /// <summary>
    /// Scheme layout web form
    /// <para>Веб-форма компоновки схемы</para>
    /// </summary>
    public partial class WFrmScheme : System.Web.UI.Page
    {
        /// <summary>
        /// Имя словаря с фразами для формирования JavaScript
        /// </summary>
        private const string DictName = "Scada.Web.Plugins.Scheme.WFrmScheme.Js";

        // Переменные для вывода на веб-страницу
        protected bool debugMode;  // режим отладки
        protected int viewID;      // ид. представления
        protected int refrRate;    // частота обновления данных
        protected string phrases;  // локализованные фразы

        private AppData appData;   // общие данные веб-приложения
        private UserData userData; // данные пользователя приложения


        /// <summary>
        /// Установить или отключить режим отладки
        /// </summary>
        private void SetupDebugMode()
        {
#if DEBUG_MODE
            debugMode = true;
            appData.Init(Server.MapPath("~"));
            appData.UserMonitor.AddUser(userData);
            string errMsg;
            userData.Login("admin", out errMsg);
#else
            debugMode = false;
#endif
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            appData = AppData.GetAppData();
            userData = UserData.GetUserData();
            SetupDebugMode();

            // перевод веб-страницы
            Translator.TranslatePage(Page, "Scada.Web.Plugins.Scheme.WFrmScheme");

            // получение ид. представления из параметров запроса
            int.TryParse(Request.QueryString["viewID"], out viewID);

            // проверка прав на просмотр представления
            if (!(userData.LoggedOn && userData.UserRights.GetViewRights(viewID).ViewRight))
                Response.Redirect(UrlTemplates.NoView);

            // подготовка данных для веб-страницы
            refrRate = userData.WebSettings.DataRefrRate;

            Localization.Dict dict;
            Localization.Dictionaries.TryGetValue(DictName, out dict);
            phrases = WebUtils.DictionaryToJs(dict);

            // загрузка представления в кеш, чтобы проверить, что оно доступно, присвоить метку
            // и обеспечить возможность получения данных входных каналов через API 
            SchemeView schemeView = appData.ViewCache.GetView<SchemeView>(viewID, true);
            appData.AssignStamp(schemeView);
        }
    }
}