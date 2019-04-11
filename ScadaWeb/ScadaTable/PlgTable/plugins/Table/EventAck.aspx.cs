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
 * Summary  : Event acknowledgement dialog web form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2019
 */

using Scada.Client;
using Scada.Data.Models;
using Scada.Data.Tables;
using Scada.UI;
using System;
using System.Drawing;

namespace Scada.Web.Plugins.Table
{
    /// <summary>
    /// Event acknowledgement dialog web form
    /// <para>Веб-форма диалога квитирования события</para>
    /// </summary>
    public partial class WFrmEventAck : System.Web.UI.Page
    {
        private AppData appData;   // общие данные веб-приложения
        private UserData userData; // данные пользователя приложения
        private DateTime evDate;   // дата события
        private int evNum;         // номер события


        protected void Page_Load(object sender, EventArgs e)
        {
            appData = AppData.GetAppData();
            userData = UserData.GetUserData();

            // проверка входа в систему
            if (!userData.LoggedOn)
                throw new ScadaException(WebPhrases.NotLoggedOn);

            // скрытие сообщения об ошибке
            pnlErrMsg.HideAlert();

            if (IsPostBack)
            {
                evDate = (DateTime)ViewState["EvDate"];
                evNum = (int)ViewState["EvNum"];
            }
            else
            {
                // перевод веб-страницы
                Translator.TranslatePage(Page, "Scada.Web.Plugins.Table.WFrmEventAck");

                // получение параметров запроса и сохранение во ViewState
                evDate = Request.QueryString.GetParamAsDate(DateTime.Today);
                evNum = Request.QueryString.GetParamAsInt("evNum");
                ViewState["EvDate"] = evDate;
                ViewState["EvNum"] = evNum;

                int viewID = Request.QueryString.GetParamAsInt("viewID");

                // получение события
                EventTableLight tblEvent = appData.DataAccess.DataCache.GetEventTable(evDate);
                EventTableLight.Event ev = tblEvent.GetEventByNum(evNum);

                if (ev == null)
                {
                    pnlErrMsg.ShowAlert(lblEventNotFound);
                    btnSubmit.Enabled = false;
                }
                else
                {
                    // проверка прав
                    EntityRights rights = userData.UserRights.GetUiObjRights(viewID);
                    if (!rights.ViewRight)
                        throw new ScadaException(CommonPhrases.NoRights);

                    if (!userData.UserRights.ViewAllRight)
                    {
                        BaseView view = userData.UserViews.GetView(viewID, true);
                        if (!view.ContainsCnl(ev.CnlNum))
                            throw new ScadaException(CommonPhrases.NoRights);
                    }

                    btnSubmit.Visible = pnlTip.Visible = 
                        rights.ControlRight && !ev.Checked;

                    // вывод информации по событию
                    pnlInfo.Visible = true;
                    DispEvent dispEvent = appData.DataAccess.GetDispEvent(ev, new DataFormatter());
                    lblNum.Text = dispEvent.Num.ToString();
                    lblTime.Text = dispEvent.Time;
                    lblObj.Text = dispEvent.Obj;
                    lblDev.Text = dispEvent.KP;
                    lblCnl.Text = dispEvent.Cnl;
                    lblText.Text = dispEvent.Text;
                    lblAck.Text = dispEvent.Ack;
                    lblAck.CssClass = ev.Checked ? "ack-yes" : "ack-no";

                    if (ev.Checked && ev.UserID > 0)
                    {
                        string userName = appData.DataAccess.GetUserName(ev.UserID);
                        lblByUser.Text = string.Format(lblByUser.Text, userName);
                        lblByUser.Visible = userName != "";
                    }

                    if (dispEvent.Color != "")
                    {
                        try
                        {
                            lblNum.ForeColor = lblTime.ForeColor = lblObj.ForeColor = 
                                lblDev.ForeColor = lblCnl.ForeColor = lblText.ForeColor = 
                                ColorTranslator.FromHtml(dispEvent.Color);
                        }
                        catch { }
                    }
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            // отправка команды квитирования
            bool result;
            bool sendOK = appData.ServerComm.CheckEvent(userData.UserProps.UserID, evDate, evNum, out result);

            btnSubmit.Enabled = false;
            pnlInfo.Visible = false;
            pnlTip.Visible = false;

            if (sendOK && result)
            {
                ClientScript.RegisterStartupScript(GetType(), "Startup", "closeModal();", true);
            }
            else if (sendOK)
            {
                pnlErrMsg.ShowAlert(lblAckRejected);
            }
            else
            {
                pnlErrMsg.ShowAlert(lblAckNotSent);
            }
        }
    }
}