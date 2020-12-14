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
 * Summary  : Select input channels web form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

using Scada.Client;
using Scada.UI;
using Scada.Web.Shell;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Scada.Web.Plugins.Chart
{
    /// <summary>
    /// Select input channels web form
    /// <para>Веб-форма выбора входных каналов</para>
    /// </summary>
    public partial class WFrmSelectCnls : System.Web.UI.Page
    {
        private AppData appData;           // общие данные веб-приложения
        private UserData userData;         // данные пользователя приложения
        private List<CnlViewPair> selCnls; // выбранные каналы
        protected HashSet<int> selCnlSet;  // множество выбранных каналов


        /// <summary>
        /// Отобразить каналы выбранного представления
        /// </summary>
        private void ShowCnlsByView()
        {
            int viewID;
            int.TryParse(ddlView.SelectedValue, out viewID);
            List<CnlViewPair> cnlsByView = ChartUtils.GetCnlViewPairsByView(
                viewID, appData.DataAccess, userData.UserViews);

            repCnlsByView.DataSource = cnlsByView;
            repCnlsByView.DataBind();
            lblUnableLoadView.Visible = cnlsByView == null;
            lblNoCnlsByView.Visible = cnlsByView != null && cnlsByView.Count == 0;
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            appData = AppData.GetAppData();
            userData = UserData.GetUserData();

            // проверка входа в систему
            if (!userData.LoggedOn)
                throw new ScadaException(WebPhrases.NotLoggedOn);

            if (IsPostBack)
            {
                // получение выбранных каналов
                selCnls = (List<CnlViewPair>)ViewState["SelCnls"];
                selCnlSet = ChartUtils.GetCnlSet(selCnls);
            }
            else
            {
                // перевод веб-страницы
                Translator.TranslatePage(Page, "Scada.Web.Plugins.Chart.WFrmSelectCnls");
                lblPerfWarn.Text = ChartPhrases.PerfWarning;

                // настройка элементов управления
                btnSubmit.Enabled = false;
                pnlPerfWarn.Visible = false;

                // создание списка выбранных каналов
                selCnls = new List<CnlViewPair>();
                ViewState.Add("SelCnls", selCnls);
                selCnlSet = ChartUtils.GetCnlSet(selCnls);

                // заполнение выпадающего списка представлений и отображение каналов по представлению
                ChartUtils.FillViewList(ddlView, 0, userData.UserViews);
                ShowCnlsByView();
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            // завершить выбор каналов
            string cnlNums;
            string viewIDs;
            selCnls.GetSelection(out cnlNums, out viewIDs);
            ClientScript.RegisterStartupScript(GetType(), "CloseModalScript",
                string.Format("closeModal('{0}', '{1}');", cnlNums, viewIDs), true);
        }

        protected void ddlView_SelectedIndexChanged(object sender, EventArgs e)
        {
            // выбор каналов по представлению
            ShowCnlsByView();
            ChartUtils.AddUpdateModalHeightScript(this);
        }

        protected void repCnlsByView_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            // добавление выбранного канала в список
            if (e.CommandName == "AddCnl")
            {
                int cnlNum = int.Parse((string)e.CommandArgument);

                if (!selCnlSet.Contains(cnlNum))
                {
                    int viewID = int.Parse(ddlView.SelectedValue);
                    CnlViewPair pair = new CnlViewPair(cnlNum, viewID);
                    pair.FillInfo(appData.DataAccess.GetCnlProps(cnlNum), null);

                    selCnls.Add(pair);
                    ViewState.Add("SelCnls", selCnls);

                    btnSubmit.Enabled = true;
                    bool warnWasVisible = pnlPerfWarn.Visible;
                    pnlPerfWarn.Visible = selCnls.Count > ChartUtils.NormalChartCnt;

                    if (pnlPerfWarn.Visible != warnWasVisible)
                        ChartUtils.AddUpdateModalHeightScript(this);
                }

                Label lblCnlAdded = (Label)e.Item.FindControl("lblCnlAdded");
                lblCnlAdded.Visible = true;
            }
        }
    }
}