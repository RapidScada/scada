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
 * Summary  : Minute data report parameters web form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

using Scada.UI;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Scada.Web.Plugins.Chart
{
    /// <summary>
    /// Minute data report parameters web form
    /// <para>Веб-форма параметров отчёта по минутным данным</para>
    /// </summary>
    public partial class WFrmMinDataRep : System.Web.UI.Page
    {
        private AppData appData;           // общие данные веб-приложения
        private UserData userData;         // данные пользователя приложения
        private List<CnlViewPair> selCnls; // выбранные каналы


        /// <summary>
        /// Скрыть все сообщения
        /// </summary>
        private void HideAllMsgs()
        {
            pnlErrMsg.Visible = false;
            pnlWarnMsg.Visible = false;
        }

        /// <summary>
        /// Вывести сообщение об ошибке
        /// </summary>
        private void ShowErrMsg(string errMsg)
        {
            lblErrMsg.Text = errMsg;
            pnlErrMsg.Visible = true;
        }

        /// <summary>
        /// Вывести предупреждение
        /// </summary>
        private void ShowWarnMsg(string warnMsg)
        {
            lblWarnMsg.Text = warnMsg;
            pnlWarnMsg.Visible = true;
        }

        /// <summary>
        /// Отобразить выбранные каналы
        /// </summary>
        private void ShowSelCnls()
        {
            repSelCnls.DataSource = selCnls;
            repSelCnls.DataBind();
            btnGenReport.Enabled = selCnls.Count > 0;
            lblNoSelCnls.Visible = !btnGenReport.Enabled;

            if (selCnls.Count > ChartUtils.NormalChartCnt)
                ShowWarnMsg(ChartPhrases.PerfWarning);
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            appData = AppData.GetAppData();
            userData = UserData.GetUserData();

            // скрытие всех сообщений
            HideAllMsgs();

            if (IsPostBack)
            {
                // восстановление заголовка при работе с AJAX
                Title = (string)ViewState["Title"];

                // получение выбранных каналов
                selCnls = (List<CnlViewPair>)ViewState["SelCnls"];
            }
            else
            {
                // перевод веб-страницы
                Translator.TranslatePage(Page, "Scada.Web.Plugins.Chart.WFrmMinDataRep");
                ViewState["Title"] = Title;

                // установка периода по умолчанию
                txtDateFrom.Text = txtDateTo.Text = DateTime.Today.ToLocalizedDateString();

                // создание списка выбранных каналов
                selCnls = new List<CnlViewPair>();
                ViewState.Add("SelCnls", selCnls);

                // отображение выбранных каналов
                ShowSelCnls();
            }
        }

        protected void btnApplyAddedCnls_Click(object sender, EventArgs e)
        {
            // добавление каналов
            if (hidAddedCnlNums.Value != "")
            {
                int[] addedCnls = WebUtils.QueryParamToIntArray(hidAddedCnlNums.Value);
                int[] addedViewIDs = WebUtils.QueryParamToIntArray(hidAddedViewIDs.Value);
                ChartUtils.CheckArrays(addedCnls, addedViewIDs);
                HashSet<int> selCnlSet = ChartUtils.GetCnlSet(selCnls);

                for (int i = 0, cnt = addedCnls.Length; i < cnt; i++)
                {
                    int cnlNum = addedCnls[i];
                    if (!selCnlSet.Contains(cnlNum))
                    {
                        CnlViewPair pair = new CnlViewPair(cnlNum, addedViewIDs[i]);
                        pair.FillInfo(appData.DataAccess.GetCnlProps(cnlNum), userData.UserViews);
                        selCnls.Add(pair);
                    }
                }

                ViewState.Add("SelCnls", selCnls);
                ShowSelCnls();

                hidAddedCnlNums.Value = "";
                hidAddedViewIDs.Value = "";
            }
        }

        protected void repSelCnls_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            // удаление выбранного канала из списка
            if (e.CommandName == "RemoveCnl")
            {
                int index = int.Parse((string)e.CommandArgument);
                if (index < selCnls.Count)
                {
                    selCnls.RemoveAt(index);
                    ViewState.Add("SelCnls", selCnls);
                    ShowSelCnls();
                }
            }
        }

        protected void btnGenReport_Click(object sender, EventArgs e)
        {
            // проверка параметров отчёта и добавление скрипта генерации отчёта
            if (selCnls.Count > 0)
            {
                DateTime dateFrom;
                DateTime dateTo;
                int period;
                string errMsg;

                if (RepUtils.ParseDates(txtDateFrom.Text, txtDateTo.Text, out dateFrom, out dateTo, out errMsg) &&
                    RepUtils.CheckDayPeriod(dateFrom, dateTo, out period, out errMsg))
                {
                    string cnlNums;
                    string viewIDs;
                    ChartUtils.GetSelection(selCnls, out cnlNums, out viewIDs);

                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "GenerateReportScript",
                        string.Format("generateReport('{0}', '{1}', {2}, {3}, {4}, {5});",
                        cnlNums, viewIDs, dateFrom.Year, dateFrom.Month, dateFrom.Day, period), true);
                }
                else
                {
                    ShowErrMsg(errMsg);
                }
            }
        }
    }
}