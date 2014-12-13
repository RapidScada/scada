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
 * Summary  : Table view web form
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
    /// Table view web form
    /// <para>Веб-форма табличного представления</para>
    /// </summary>
    public partial class WFrmTableView : System.Web.UI.Page
    {
        /// <summary>
        /// Создать ячейку таблицы и добавить её в заданную строку
        /// </summary>
        internal static TableCell NewCell(TableRow row, string text, string cssClass = "")
        {
            TableCell cell = new TableCell();
            cell.Text = text;
            if (cssClass != "")
                cell.CssClass = cssClass;
            row.Cells.Add(cell);
            return cell;
        }

        /// <summary>
        /// Установить класс строки таблицы
        /// </summary>
        internal static void SetCssClass(TableRow row, bool alt, bool bot)
        {
            string cssClass = row.CssClass;
            if (alt)
                cssClass += cssClass == "" ? "alt" : " alt";
            if (bot)
                cssClass += cssClass == "" ? "bot" : " bot";
            row.CssClass = cssClass;
        }


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
            Localization.TranslatePage(this, "Scada.Web.WFrmTableView");

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

            // проверка загрузки представления и прав на просмотр информации
            if (tableView == null)
                throw new Exception(WebPhrases.UnableLoadView);
            else if (!right.ViewRight)
                throw new Exception(CommonPhrases.NoRights);

            // определение параметров запроса
            int year, month, day, stage;
            int.TryParse(Request["year"], out year);
            int.TryParse(Request["month"], out month);
            int.TryParse(Request["day"], out day);
            int.TryParse(Request["stage"], out stage);

            DateTime reqDate;
            try
            {
                reqDate = new DateTime(year, month, day);
            }
            catch
            {
                throw new Exception(WebPhrases.IncorrectDate);
            }

            // формирование таблицы отображаемых элементов представления
            int itemCnt = tableView.VisibleCount;
            int itemLastInd = itemCnt - 1;

            if (itemCnt > 0)
            {
                // формирование таблицы обозначений элементов представления
                TableRow row = new TableRow();
                row.CssClass = "hdr";
                NewCell(row, WebPhrases.ItemColumn);
                tblCap.Rows.Add(row);

                const string textHtmlTempl = "<table class=\"cap\" cellpadding=\"0\" cellspacing=\"0\"><tr>" +
                    "<td>{0}</td><td>{1}</td><td>{2}</td></tr></table>";
                const string hintHtmlTempl = "<div class=\"hint\">{0}</div>";
                const string iconHtmlTempl = "<img src=\"images/paramIcons/{0}\" alt=\"\" />";
                const string capHtmlTempl = "<span>{0}</span>";
                const string diagHtmlTempl = "<a href=\"javascript:ShowDiag({0}, {1}, {2}, {3}, {4}, {5})\">";
                const string cmdHtmlTempl = "<a href=\"javascript:SendCmd({0}, {1}, {2})\">" + 
                    "<img src=\"images/cmd.gif\" alt=\"\" /></a>";
                bool canSendCmd = AppData.WebSettings.CmdEnabled && right.CtrlRight;

                for (int itemInd = 0; itemInd < itemCnt; itemInd++)
                {
                    TableView.Item item = tableView.VisibleItems[itemInd];
                    int cnlNum = item.CnlNum;
                    int cntrlCnlNum = item.CtrlCnlNum;
                    string text;

                    if (cnlNum > 0 || cntrlCnlNum > 0)
                    {
                        CnlProps cnlProps = item.CnlProps;

                        List<string> hintLines = new List<string>();
                        if (cnlNum > 0)
                            hintLines.Add(WebPhrases.InCnlNumHint + cnlNum);
                        if (cntrlCnlNum > 0)
                            hintLines.Add(WebPhrases.OutCnlNumHint + cntrlCnlNum);
                        if (cnlProps != null)
                        {
                            hintLines.Add(WebPhrases.ObjectHint + cnlProps.ObjName);
                            hintLines.Add(WebPhrases.KPHint + cnlProps.KPName);
                            hintLines.Add(WebPhrases.ParamHint + cnlProps.ParamName);
                            if (cnlProps.ShowNumber && cnlProps.UnitArr != null && cnlProps.UnitArr.Length > 0)
                                hintLines.Add(WebPhrases.UnitHint + cnlProps.UnitArr[0]);
                        }

                        string hint = string.Join("<br />", hintLines.ToArray());
                        string hintHtml = string.Format(hintHtmlTempl, hint);
                        string iconFileName = cnlProps == null || cnlProps.IconFileName == "" ?
                            "undef.gif" : cnlProps.IconFileName;
                        string iconHtml = string.Format(iconHtmlTempl, iconFileName);
                        string capHtml = string.Format(capHtmlTempl, item.Caption);
                        string cmdHtml = canSendCmd && cntrlCnlNum > 0 ? 
                            string.Format(cmdHtmlTempl, viewSetIndex, viewIndex, cntrlCnlNum) : "";

                        if (cnlNum > 0)
                        {
                            string diagHtml = string.Format(diagHtmlTempl,
                                viewSetIndex, viewIndex, year, month, day, cnlNum);
                            iconHtml = diagHtml + iconHtml + "</a>";
                            capHtml = diagHtml + capHtml + "</a>";
                        }

                        text = string.Format(textHtmlTempl, iconHtml + hintHtml, capHtml, cmdHtml);
                    }
                    else
                    {
                        text = item.Caption == "" ? "<p>&nbsp;</p>" : "<p>" + item.Caption + "</p>";
                    }

                    row = new TableRow();
                    SetCssClass(row, itemInd % 3 == 2, itemInd == itemLastInd);
                    NewCell(row, text);
                    tblCap.Rows.Add(row);
                }

                // получение обновлённых данных часовых срезов
                SrezTableLight hourTable;
                AppData.MainData.RefreshData(reqDate, out hourTable);

                // формирование ячейки текущего среза
                row = new TableRow();
                row.CssClass = "hdr";
                NewCell(row, WebPhrases.CurColumn);
                tblCur.Rows.Add(row);

                row = new TableRow();
                NewCell(row, "<iframe id=\"frameCurVal\" src=\"" + 
                    "CurVals.aspx?viewSet=" + viewSetIndex +  "&view=" + viewIndex + "&year=" + year + 
                    "&month=" + month + "&day=" + day + "&hourStamp=" + hourTable.FileModTime.Ticks + 
                    "\" frameborder=\"0\" scrolling=\"no\"></iframe>");
                tblCur.Rows.Add(row);

                // формирование заголовка таблицы часовых значений
                int firstHour = stage == 2 ? 12 : 0;
                int lastHour = stage == 1 ? 11 : 23;
                row = new TableRow();
                row.CssClass = "hdr";
                tblHour.Rows.Add(row);

                for (int hour = firstHour; hour <= lastHour; hour++)
                    NewCell(row, hour.ToString());

                // формирование содержимого таблицы часовых значений
                // для оптимизации доступа к данным порядок заполнения следующий: по часам, по элементам
                for (int hour = firstHour; hour <= lastHour; hour++)
                {
                    bool isFirstHour = hour == firstHour;
                    DateTime dateTime = new DateTime(year, month, day, hour, 0, 0);
                    int rowInd = 1;

                    for (int itemInd = 0; itemInd < itemCnt; itemInd++)
                    {
                        TableView.Item item = tableView.VisibleItems[itemInd];
                        string text;
                        string color;

                        if (item.CnlNum > 0)
                        {
                            text = AppData.MainData.GetCnlVal(hourTable, item.CnlNum, dateTime, false, out color);
                        }
                        else
                        {
                            text = "";
                            color = "";
                        }

                        if (isFirstHour)
                        {
                            row = new TableRow();
                            SetCssClass(row, itemInd % 3 == 2, itemInd == itemLastInd);
                            tblHour.Rows.Add(row);
                            text = text == "" ? "<p>&nbsp;</p>" : "<p>" + text + "</p>";
                        }
                        else
                        {
                            row = tblHour.Rows[rowInd++];
                        }

                        TableCell cell = NewCell(row, text);

                        if (!(color == "" || color.ToLower() == "black"))
                            cell.ForeColor = Color.FromName(color);
                    }
                }
            }
            else
            {
                tblCap.Visible = false;
                tblCur.Visible = false;
                tblHour.Visible = false;
                lblNoData.Visible = true;
            }
        }
    }
}