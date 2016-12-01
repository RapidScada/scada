/*
 * Copyright 2014 Mikhail Shiryaev
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
 * Summary  : Table view items current values web form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2012
 * Modified : 2013
 */

using System;
using System.Drawing;
using System.Web.UI.WebControls;
using Scada.Client;
using Scada.Data;

namespace Scada.Web
{
    /// <summary>
    /// Table view items current values web form
    /// <para>Веб-форма текущих значений элементов табличного представления</para>
    /// </summary>
    public partial class WFrmCurVals : System.Web.UI.Page
    {
        /// <summary>
        /// Добавить скрипт обновления текущих значений по таймауту
        /// </summary>
        private void AddRefreshCurValsScript(int timeout)
        {
            if (timeout > 0)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Startup1",
                    "setTimeout('window.location = window.location', " + timeout + ");", true);
            }
        }

        /// <summary>
        /// Добавить скрипт обновления часовых значений при загрузке
        /// </summary>
        private void AddRefreshHourValsScript()
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Startup2", 
                "if (window.parent) window.parent.location = window.parent.location;", true);
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            // отключение кэширования страницы
            ScadaUtils.DisablePageCache(Response);

            // получение данных пользователя
            UserData userData = UserData.GetUserData();

            // определение индексов выбранного представления
            int viewSetIndex, viewIndex;
            if (!int.TryParse(Request["viewSet"], out viewSetIndex))
                viewSetIndex = -1;
            if (!int.TryParse(Request["view"], out viewIndex))
                viewIndex = -1;

            // получение табличного представления и прав пользователя на него
            BaseView baseView;
            MainData.Right right;
            TableView tableView = userData.GetView(typeof(TableView), viewSetIndex, viewIndex,
                out baseView, out right) ? (TableView)baseView : null;

            // проверка входа в систему и прав на просмотр информации
            if (userData.LoggedOn && tableView != null && right.ViewRight)
            {
                // определение параметров запроса
                int year, month, day;
                int.TryParse(Request["year"], out year);
                int.TryParse(Request["month"], out month);
                int.TryParse(Request["day"], out day);
                string hourStamp = Request["hourStamp"];

                DateTime reqDate;
                try
                {
                    reqDate = new DateTime(year, month, day);
                }
                catch
                {
                    throw new Exception(WebPhrases.IncorrectDate);
                }

                // обновление данных
                SrezTableLight hourTable;
                AppData.MainData.RefreshData(reqDate, out hourTable);

                // формирование таблицы текущих значений элементов представления
                int itemCnt = tableView.VisibleCount;
                int itemLastInd = itemCnt - 1;

                for (int itemInd = 0; itemInd < itemCnt; itemInd++)
                {
                    TableView.Item item = tableView.VisibleItems[itemInd];
                    string text;
                    string color;

                    if (item.CnlNum > 0)
                    {
                        text = AppData.MainData.GetCnlVal(item.CnlNum, false, out color);
                    }
                    else
                    {
                        text = "";
                        color = "";
                    }

                    TableRow row = new TableRow();
                    TableCell cell = WFrmTableView.NewCell(row, text == "" ? "<p>&nbsp;</p>" : "<p>" + text + "</p>");
                    WFrmTableView.SetCssClass(row, itemInd % 3 == 2, itemInd == itemLastInd);
                    tblCurVals.Rows.Add(row);

                    if (!(color == "" || color.ToLower() == "black"))
                        cell.ForeColor = Color.FromName(color);
                }


                if (string.IsNullOrEmpty(hourStamp) || hourStamp == hourTable.FileModTime.Ticks.ToString())
                    // добавление скрипта обновления текущих значений по таймауту
                    AddRefreshCurValsScript(AppData.WebSettings.SrezRefrFreq * 1000);
                else
                    // добавление скрипта обновления часовых значений при загрузке 
                    AddRefreshHourValsScript();
            }
            else
            {
                // добавление скрипта обновления часовых значений при загрузке 
                // для перехода на страницу входа в систему
                AddRefreshHourValsScript();
            }
        }
    }
}