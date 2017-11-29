/*
 * Copyright 2017 Mikhail Shiryaev
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
 * Module   : PlgScheme
 * Summary  : Scheme layout web form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2017
 */

using Scada.Data.Models;
using Scada.Scheme;
using Scada.UI;
using Scada.Web.Shell;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scada.Web.Plugins.Scheme
{
    /// <summary>
    /// Scheme layout web form
    /// <para>Веб-форма компоновки схемы</para>
    /// </summary>
    public partial class WFrmScheme : System.Web.UI.Page
    {
        /// <summary>
        /// Путь к директории плагинов
        /// </summary>
        private const string PluginsRoot = "~/plugins/";

        // Переменные для вывода на веб-страницу
        protected string compStyles;      // стили компонентов схемы
        protected string compScripts;     // скрипты компонентов схемы
        protected bool debugMode = false; // режим отладки
        protected int viewID;             // ид. представления
        protected int refrRate;           // частота обновления данных
        protected string phrases;         // локализованные фразы
        protected bool controlRight;      // право на управление представлением


        /// <summary>
        /// Получить стили компонентов схемы
        /// </summary>
        private string GetCompStyles()
        {
            StringBuilder sbCompStyles = new StringBuilder();
            CompManager compManager = CompManager.GetInstance();
            List<string> compStyles = compManager.GetAllStyles();

            foreach (string stylePath in compStyles)
            {
                sbCompStyles.AppendFormat(WebUtils.StyleTemplate, ResolveUrl(PluginsRoot + stylePath)).AppendLine();
            }

            return sbCompStyles.ToString();
        }

        /// <summary>
        /// Получить скрипты компонентов схемы
        /// </summary>
        private string GetCompScripts()
        {
            StringBuilder sbCompScripts = new StringBuilder();
            CompManager compManager = CompManager.GetInstance();
            List<string> compScripts = compManager.GetAllScripts();

            foreach (string scriptPath in compScripts)
            {
                sbCompScripts.AppendFormat(WebUtils.ScriptTemplate, ResolveUrl(PluginsRoot + scriptPath)).AppendLine();
            }

            return sbCompScripts.ToString();
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            AppData appData = AppData.GetAppData();
            UserData userData = UserData.GetUserData();

#if DEBUG
            debugMode = true;
            userData.LoginForDebug();
#endif

            // перевод веб-страницы
            Translator.TranslatePage(Page, "Scada.Web.Plugins.Scheme.WFrmScheme");

            // получение ид. представления из параметров запроса
            viewID = Request.QueryString.GetParamAsInt("viewID");

            // проверка прав на просмотр представления
            EntityRights rights = userData.LoggedOn ?
                userData.UserRights.GetUiObjRights(viewID) : EntityRights.NoRights;
            if (!rights.ViewRight)
                Response.Redirect(UrlTemplates.NoView);

            // загрузка представления в кеш, чтобы проверить, что оно доступно, присвоить метку
            // и обеспечить возможность получения данных входных каналов через API,
            // ошибка будет записана в журнал приложения
            SchemeView schemeView = appData.ViewCache.GetView<SchemeView>(viewID);
            if (schemeView == null)
                Response.Redirect(UrlTemplates.NoView);
            else
                appData.AssignStamp(schemeView);

            // подготовка данных для вывода на веб-страницу
            compStyles = GetCompStyles();
            compScripts = GetCompScripts();
            refrRate = userData.WebSettings.DataRefrRate;
            phrases = WebUtils.DictionaryToJs("Scada.Web.Plugins.Scheme.WFrmScheme.Js");
            controlRight = rights.ControlRight;
        }
    }
}