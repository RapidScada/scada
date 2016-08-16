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
 * Module   : PlgTable
 * Summary  : Event acknowledgement dialog web form
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
using System.Drawing;
using System.Web.UI.WebControls;

namespace Scada.Web.plugins.Table
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

        
        /// <summary>
        /// Скрыть сообщение об ошибке
        /// </summary>
        private void HideErrMsg()
        {
            pnlErrMsg.Visible = false;
            lblEventNotFound.Visible = false;
            lblAckNotSent.Visible = false;
            lblAckRejected.Visible = false;
        }

        /// <summary>
        /// Вывести сообщение об ошибке
        /// </summary>
        private void ShowErrMsg(Label lblMessage)
        {
            pnlErrMsg.Visible = true;
            lblMessage.Visible = true;
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            appData = AppData.GetAppData();
            userData = UserData.GetUserData();

            // проверка входа в систему
            if (!userData.LoggedOn)
                throw new ScadaException(CommonPhrases.NoRights);

            // скрытие сообщения об ошибке
            HideErrMsg();

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
                evDate = WebUtils.GetDateFromQueryString(Request);
                int.TryParse(Request.QueryString["evNum"], out evNum);
                ViewState["EvDate"] = evDate;
                ViewState["EvNum"] = evNum;

                int viewID;
                int.TryParse(Request.QueryString["viewID"], out viewID);

                // получение события
                EventTableLight tblEvent = appData.DataAccess.DataCache.GetEventTable(evDate);
                EventTableLight.Event ev = tblEvent.GetEventByNum(evNum);

                if (ev == null)
                {
                    ShowErrMsg(lblEventNotFound);
                    btnSubmit.Enabled = false;
                }
                else
                {
                    // проверка прав
                    EntityRights rights = userData.UserRights.GetViewRights(viewID);
                    if (!rights.ViewRight)
                        throw new ScadaException(CommonPhrases.NoRights);

                    Type viewType = userData.UserViews.GetViewType(viewID);
                    BaseView view = appData.ViewCache.GetView(viewType, viewID, true);

                    if (!view.ContainsCnl(ev.CnlNum))
                        throw new ScadaException(CommonPhrases.NoRights);

                    btnSubmit.Visible = pnlTip.Visible = 
                        rights.ControlRight && !ev.Checked;

                    // вывод информации по событию
                    pnlInfo.Visible = true;
                    DispEventProps evProps = appData.DataAccess.GetDispEventProps(ev, new DataFormatter());
                    lblNum.Text = evProps.Num.ToString();
                    lblTime.Text = evProps.Time;
                    lblObj.Text = evProps.Obj;
                    lblDev.Text = evProps.KP;
                    lblCnl.Text = evProps.Cnl;
                    lblText.Text = evProps.Text;
                    lblAck.Text = evProps.Ack;
                    lblAck.CssClass = ev.Checked ? "ack-yes" : "ack-no";

                    if (ev.Checked && ev.UserID > 0)
                    {
                        string userName = appData.DataAccess.GetUserName(ev.UserID);
                        lblByUser.Text = string.Format(lblByUser.Text, userName);
                        lblByUser.Visible = userName != "";
                    }

                    if (evProps.Color != "")
                    {
                        try
                        {
                            lblNum.ForeColor = lblTime.ForeColor = lblObj.ForeColor = 
                                lblDev.ForeColor = lblCnl.ForeColor = lblText.ForeColor = 
                                ColorTranslator.FromHtml(evProps.Color);
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
                ShowErrMsg(lblAckRejected);
            }
            else
            {
                ShowErrMsg(lblAckNotSent);
            }
        }
    }
}