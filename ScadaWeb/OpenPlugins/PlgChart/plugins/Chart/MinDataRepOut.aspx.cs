/*
 * Copyright 2019 Mikhail Shiryaev
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
 * Summary  : Minute data report output web form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2019
 */

using Scada.Client;
using System;
using Utils.Report;

namespace Scada.Web.Plugins.Chart
{
    /// <summary>
    /// Minute data report output web form
    /// <para>Выходная веб-форма отчёта по минутным данным</para>
    /// </summary>
    /// <remarks>
    /// URL example: 
    /// http://webserver/scada/plugins/Chart/MinDataRepOut.aspx?cnlNums=1,2&viewIDs=1,1&year=2016&month=1&day=1&period=2
    /// </remarks>
    public partial class WFrmMinDataRepOut : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AppData appData = AppData.GetAppData();
            UserData userData = UserData.GetUserData();

            // проверка входа в систему
            if (!userData.LoggedOn)
                throw new ScadaException(WebPhrases.NotLoggedOn);

            // получение параметров запроса
            int[] cnlNums = Request.QueryString.GetParamAsIntArray("cnlNums");
            int[] viewIDs = Request.QueryString.GetParamAsIntArray("viewIDs");
            DateTime startDate = Request.QueryString.GetParamAsDate();
            int period = Request.QueryString.GetParamAsInt("period");

            // проверка прав и получение представления, если оно единственное
            if (!userData.UserRights.CheckInCnlRights(cnlNums, viewIDs, out int singleViewID))
                throw new ScadaException(CommonPhrases.NoRights);

            BaseView singleView = userData.UserViews.GetView(singleViewID, true, true);
            string viewTitle = singleView == null ? "" : singleView.Title;

            // генерация отчёта
            RepBuilder repBuilder = new MinDataRepBuilder(appData.DataAccess);
            RepUtils.WriteGenerationAction(appData.Log, repBuilder.RepName, userData);
            repBuilder.Generate(
                new object[] { cnlNums, startDate, period, viewTitle },
                Server.MapPath("~/plugins/Chart/templates/"),
                RepUtils.BuildFileName("MinData", "xml"),
                Response);
        }
    }
}