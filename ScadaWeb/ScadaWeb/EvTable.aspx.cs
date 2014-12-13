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
 * Summary  : Event table web form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2012
 * Modified : 2014
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Web.UI.WebControls;
using Scada.Client;
using Scada.Data;

namespace Scada.Web
{
    /// <summary>
    /// Event table web form
    /// <para>Веб-форма таблицы событий</para>
    /// </summary>
    public partial class WFrmEvTable: System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // отключение кэширования страницы
            ScadaUtils.DisablePageCache(Response);

            // получение данных пользователя
            UserData userData = UserData.GetUserData();

            // проверка входа в систему
            userData.CheckLoggedOn(Context, false);
            if (!userData.LoggedOn)
                throw new Exception(WebPhrases.NotLoggedOn);

            // перевод веб-страницы
            Localization.TranslatePage(this, "Scada.Web.WFrmEvTable");

            // определение индексов выбранного представления
            int viewSetIndex, viewIndex;
            if (!int.TryParse(Request["viewSet"], out viewSetIndex))
                viewSetIndex = -1;
            if (!int.TryParse(Request["view"], out viewIndex))
                viewIndex = -1;

            // получение представления и прав пользователя на него
            BaseView view;
            MainData.Right right;
            userData.GetView(null, viewSetIndex, viewIndex, out view, out right);

            // вывод сообщения о загрузке
            if (view == null && right.ViewRight)
            {
                lblLoading.Visible = true;
                tblEv.Visible = false;
                hidEvStamp.Value = "-1";
                return;
            }

            // определение фильтра событий
            bool showAllEvents = Request["filter"] == "all";

            // проверка загрузки представления и прав на просмотр информации
            if (view == null)
                throw new Exception(WebPhrases.UnableLoadView);
            else if (!right.ViewRight || showAllEvents && userData.Role == ServerComm.Roles.Custom)
                throw new Exception(CommonPhrases.NoRights);

            // определение параметров запроса
            int year, month, day;
            int.TryParse(Request["year"], out year);
            int.TryParse(Request["month"], out month);
            int.TryParse(Request["day"], out day);

            DateTime reqDate;
            try
            {
                reqDate = new DateTime(year, month, day);
            }
            catch
            {
                throw new Exception(WebPhrases.IncorrectDate);
            }

            // получение обновлённых данных событий
            EventTableLight eventTable;
            AppData.MainData.RefreshEvents(reqDate, out eventTable);
            hidEvStamp.Value = eventTable.FileModTime.Ticks.ToString();

            List<int> cnlList = showAllEvents ? null : view.CnlList;
            List<MainData.EventView> events = AppData.MainData.ConvertEvents(
                AppData.MainData.GetLastEvents(eventTable, cnlList, AppData.WebSettings.EventCnt));

            if (events.Count > 0)
            {
                for (int eventInd = events.Count - 1; eventInd >= 0; eventInd--)
                {
                    MainData.EventView eventView = events[eventInd];
                    TableRow row = new TableRow();
                    WFrmTableView.NewCell(row, eventView.Num, "c");
                    WFrmTableView.NewCell(row, eventView.Date, "c");
                    WFrmTableView.NewCell(row, eventView.Time, "c");
                    WFrmTableView.NewCell(row, eventView.Obj);
                    WFrmTableView.NewCell(row, eventView.KP);
                    WFrmTableView.NewCell(row, eventView.Cnl);
                    WFrmTableView.NewCell(row, eventView.Text);

                    // установка номера события для воспроизведения звука при загрузке страницы
                    if (eventView.Sound && string.IsNullOrEmpty(hidSndEvNum.Value))
                        hidSndEvNum.Value = eventView.Num;

                    // формирование ячейки квитирования
                    if (right.CtrlRight && !eventView.Check)
                    {
                        string chkCellText = "<a href=\"javascript:CheckEvent(" + viewSetIndex + ", " + viewIndex + 
                            ", " + year + ", " + month + ", " + day + ", " + eventView.Num + 
                            ")\" style=\"color: " + eventView.Color + "\">" + eventView.User + "</a>";
                        WFrmTableView.NewCell(row, chkCellText, "c");
                    }
                    else
                    {
                        WFrmTableView.NewCell(row, eventView.User, "c");
                    }

                    // установка свойств строки таблицы
                    WFrmTableView.SetCssClass(row, eventInd % 3 == 2, eventInd == 0);
                    string color = eventView.Color;
                    if (!(color == "" || color.ToLower() == "black"))
                        row.ForeColor = Color.FromName(color);
                    tblEv.Rows.Add(row);
                }
            }
            else
            {
                lblNoData.Visible = true;
                tblEv.Visible = false;
            }
        }
    }
}