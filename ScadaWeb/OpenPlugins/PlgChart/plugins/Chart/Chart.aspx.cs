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
 * Module   : PlgChart
 * Summary  : Chart web form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2020
 */

using Scada.Client;
using Scada.UI;
using System;
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
        /// <summary>
        /// The script to add to a web page.
        /// </summary>
        protected StringBuilder sbClientScript;

        protected void Page_Load(object sender, EventArgs e)
        {
            AppData appData = AppData.GetAppData();
            UserData userData = UserData.GetUserData();

#if DEBUG
            userData.LoginForDebug();
            string chartTitle = "Debug";
#endif

            // перевод веб-страницы
            Translator.TranslatePage(Page, "Scada.Web.Plugins.Chart.WFrmChart");

            // получение параметров запроса
            // получить номера как массивы для корректной работы в составе дэшборда
            int[] cnlNums = Request.QueryString.GetParamAsIntArray("cnlNum"); 
            int[] viewIDs = Request.QueryString.GetParamAsIntArray("viewID");
            int cnlNum = cnlNums.Length > 0 ? cnlNums[0] : 0;
            int viewID = viewIDs.Length > 0 ? viewIDs[0] : 0;
            DateTime startDate = Request.QueryString.GetParamAsDate(DateTime.Today);

            // проверка входа в систему и прав
            if (!userData.LoggedOn)
                throw new ScadaException(WebPhrases.NotLoggedOn);

            if (!userData.UserRights.GetUiObjRights(viewID).ViewRight)
                throw new ScadaException(CommonPhrases.NoRights);

#if !DEBUG
            // в режиме отладки невозможно получить представление, т.к. плагины не загружены
            BaseView view = userData.UserViews.GetView(viewID, true);

            if (!view.ContainsCnl(cnlNum))
                throw new ScadaException(CommonPhrases.NoRights);

            // вывод заголовка
            Title = cnlNum + " - " + Title;
            string chartTitle = view.Title;
#endif

            // вывод дополнительной информации
            chartTitle += (string.IsNullOrEmpty(chartTitle) ? "" : ", ") + startDate.ToLocalizedDateString();
            string chartStatus = DateTime.Now.ToLocalizedString();

            // подготовка данных графика
            ChartDataBuilder dataBuilder = new ChartDataBuilder(new int[] { cnlNum }, startDate, 1, appData.DataAccess);
            dataBuilder.FillCnlProps();
            dataBuilder.FillData();

            // build client script
            sbClientScript = new StringBuilder();
            dataBuilder.ToJs(sbClientScript);

            sbClientScript
                .AppendFormat("var locale = '{0}';", Localization.Culture.Name).AppendLine()
                .AppendFormat("var gapBetweenPoints = {0};", userData.WebSettings.ChartGap).AppendLine()
                .AppendFormat("var chartTitle = '{0}';", HttpUtility.JavaScriptStringEncode(chartTitle)).AppendLine()
                .AppendFormat("var chartStatus = '{0}';", chartStatus).AppendLine()
                .AppendLine();
        }
    }
}
