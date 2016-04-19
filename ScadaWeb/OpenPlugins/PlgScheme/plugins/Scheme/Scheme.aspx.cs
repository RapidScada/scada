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
using System;
using System.Web;

namespace Scada.Web.Plugins.Scheme
{
    /// <summary>
    /// Scheme layout web form
    /// <para>Веб-форма компоновки схемы</para>
    /// </summary>
    public partial class WFrmScheme : System.Web.UI.Page
    {
        /// <summary>
        /// Имя словаря с фразами для JavaScript
        /// </summary>
        private const string DictName = "Scada.Web.Plugins.Scheme.WFrmScheme.Js";

        // Переменные для вывода на веб-страницу
        protected bool debugMode; // режим отладки
        protected int viewID;     // ид. представления
        protected int refrRate;   // частота обновления данных
        protected string phrases; // локализованные фразы

        private AppData appData;  // общие данные веб-приложения

        /// <summary>
        /// Зарузить словари, используемые плагином
        /// </summary>
        [Obsolete("Move to plugin specification")]
        private void LoadDictionaries()
        {
            string langDir = Server.MapPath("~/plugins/Scheme/lang/");
            string errMsg;
            if (!Localization.LoadDictionaries(langDir, "PlgScheme", out errMsg))
                appData.Log.WriteError(errMsg);
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            appData = AppData.GetAppData();
            UserData userData = UserData.GetUserData();

#if DEBUG_MODE
            appData.Init(Server.MapPath("~"));
            debugMode = true;
#else
            debugMode = false;
#endif

            int.TryParse(Request["viewID"], out viewID);
            refrRate = userData.WebSettings.DataRefrRate;

            LoadDictionaries();
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