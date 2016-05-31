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
 * Summary  : Table view web form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

using Scada.UI;
using Scada.Web.Shell;
using System;
using System.Web.UI.WebControls;

namespace Scada.Web.Plugins.Table
{
    /// <summary>
    /// Table view web form
    /// <para>Веб-форма табличного представления</para>
    /// </summary>
    public partial class WFrmTable : System.Web.UI.Page
    {
        // Переменные для вывода на веб-страницу
        protected bool debugMode = false; // режим отладки
        protected int viewID;             // ид. представления
        protected int refrRate;           // частота обновления данных
        protected string phrases;         // локализованные фразы


        /// <summary>
        /// Заполнить выпадающие списки выбора времени
        /// </summary>
        private void FillTimeDropdowns()
        {
            DateTime date = DateTime.MinValue;

            for (int i = 0; i < 24; i++)
            {
                string text = date.AddHours(i).ToString("t", Localization.Culture);
                string value = i.ToString();
                ddlTimeFrom.Items.Add(new ListItem(text, value));
                ddlTimeTo.Items.Add(new ListItem(text, value));
            }

            for (int i = 0, val = -24; i < 24; i++, val++)
            {
                string text = PlgPhrases.PrevDay + " " + date.AddHours(i).ToString("t", Localization.Culture);
                ddlTimeFrom.Items.Add(new ListItem(text, val.ToString()));
            }
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
            Translator.TranslatePage(Page, "Scada.Web.Plugins.Table.WFrmTable");

            // получение ид. представления из параметров запроса
            int.TryParse(Request.QueryString["viewID"], out viewID);

            // проверка прав на просмотр представления
            if (!(userData.LoggedOn && userData.UserRights.GetViewRights(viewID).ViewRight))
                Response.Redirect(UrlTemplates.NoView);

            // загрузка представления в кеш,
            // ошибка будет записана в журнал приложения
            try
            {
                TableView tableView = appData.ViewCache.GetView<TableView>(viewID, true);
                appData.AssignStamp(tableView);
            }
            catch
            {
                Response.Redirect(UrlTemplates.NoView);
            }

            // подготовка данных для вывода на веб-страницу
            refrRate = userData.WebSettings.DataRefrRate;

            Localization.Dict dict;
            Localization.Dictionaries.TryGetValue("Scada.Web.Plugins.Table.WFrmTable.Js", out dict);
            phrases = WebUtils.DictionaryToJs(dict);

            FillTimeDropdowns();
        }
    }
}