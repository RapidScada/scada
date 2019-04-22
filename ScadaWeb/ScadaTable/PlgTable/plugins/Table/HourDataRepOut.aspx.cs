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
 * Module   : PlgTable
 * Summary  : Hourly data report output web form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2019
 */

using Scada.Table;
using System;
using Utils.Report;

namespace Scada.Web.Plugins.Table
{
    /// <summary>
    /// Hourly data report output web form
    /// <para>Выходная веб-форма отчёта по часовым данным</para>
    /// </summary>
    /// <remarks>
    /// URL example: 
    /// http://webserver/scada/plugins/Table/HourDataRepOut.aspx?viewID=1&year=2016&month=1&day=1&startHour=0&endHour=23
    /// </remarks>
    public partial class WFrmHourDataRepOut : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AppData appData = AppData.GetAppData();
            UserData userData = UserData.GetUserData();

            // получение ид. представления из параметров запроса
            int viewID = Request.QueryString.GetParamAsInt("viewID");

            // проверка входа в систему и прав
            if (!userData.LoggedOn)
                throw new ScadaException(WebPhrases.NotLoggedOn);

            if (!userData.UserRights.GetUiObjRights(viewID).ViewRight)
                throw new ScadaException(CommonPhrases.NoRights);

            // загрузка представления
            TableView tableView = appData.ViewCache.GetView<TableView>(viewID, true);

            // получение оставшихся параметров запроса
            DateTime reqDate = Request.QueryString.GetParamAsDate(DateTime.Today);
            int startHour = Request.QueryString.GetParamAsInt("startHour");
            int endHour = Request.QueryString.GetParamAsInt("endHour");

            // генерация отчёта
            RepBuilder repBuilder = new HourDataRepBuilder(appData.DataAccess);
            RepUtils.WriteGenerationAction(appData.Log, repBuilder.RepName, userData);
            repBuilder.Generate(
                new object[] { tableView, reqDate, startHour, endHour },
                Server.MapPath("~/plugins/Table/templates/"),
                RepUtils.BuildFileName("HourData", "xml"),
                Response);
        }
    }
}