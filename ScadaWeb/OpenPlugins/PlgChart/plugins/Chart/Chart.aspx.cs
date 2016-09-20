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
 * Module   : PlgChart
 * Summary  : Chart web form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

using Scada.Client;
using Scada.Data.Models;
using Scada.Data.Tables;
using Scada.UI;
using System;
using System.Globalization;
using System.Text;
using System.Web;

namespace Scada.Web.Plugins.Chart
{
    /// <summary>
    /// Chart web form
    /// <para>Веб-форма графика</para>
    /// </summary>
    public partial class WFrmChart : System.Web.UI.Page
    {
        protected ChartDataBuilder chartDataBuilder; // объект, задающий данные графика

        protected void Page_Load(object sender, EventArgs e)
        {
            AppData appData = AppData.GetAppData();
            UserData userData = UserData.GetUserData();

#if DEBUG
            userData.LoginForDebug();
#endif

            // перевод веб-страницы
            Translator.TranslatePage(Page, "Scada.Web.Plugins.Chart.WFrmChart");

            // получение параметров запроса
            int cnlNum;
            int.TryParse(Request.QueryString["cnlNum"], out cnlNum);
            int viewID;
            int.TryParse(Request.QueryString["viewID"], out viewID);
            DateTime startDate = WebUtils.GetDateFromQueryString(Request);

            // проверка прав
            if (!userData.LoggedOn ||
                !userData.UserRights.GetUiObjRights(viewID).ViewRight)
                throw new ScadaException(CommonPhrases.NoRights);

#if !DEBUG
            // в режиме отладки невозможно получить тип представления, т.к. плагины не загружены
            Type viewType = userData.UserViews.GetViewType(viewID);
            BaseView view = appData.ViewCache.GetView(viewType, viewID, true);

            if (!view.ContainsCnl(cnlNum))
                throw new ScadaException(CommonPhrases.NoRights);

            // вывод заголовков
            Title = cnlNum + " - " + Title;
            lblTitle.Text = view.Title;
#endif

            // вывод дополнительной информации
            lblStartDate.Text = (string.IsNullOrEmpty(lblTitle.Text) ? "" : ", ") + startDate.ToLocalizedDateString();
            lblGenDT.Text = DateTime.Now.ToLocalizedString();

            // подготовка данных графика
            chartDataBuilder = new ChartDataBuilder(
                new int[] { cnlNum }, startDate, 1, userData.WebSettings.ChartGap, appData.DataAccess);
            chartDataBuilder.FillData();
        }
    }
}