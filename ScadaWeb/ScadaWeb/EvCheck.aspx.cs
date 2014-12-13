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
 * Summary  : Event check web form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2008
 * Modified : 2014
 */

using System;
using System.Drawing;
using Scada.Client;
using Scada.Data;

namespace Scada.Web
{
    /// <summary>
    /// Event check web form
    /// <para>Веб-форма квитирования события</para>
    /// </summary>
    public partial class WFrmEvCheck : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // получение данных пользователя
            UserData userData = UserData.GetUserData();

            // проверка входа в систему
            if (!userData.LoggedOn)
                throw new Exception(WebPhrases.NotLoggedOn);

            // очистка сообщения об ошибке
            lblMessage.Text = "";
            lblMessage.Visible = false;

            // установка активного элемента формы и кнопки по умолчанию
            Form.DefaultFocus = btnCheck.ClientID;
            Form.DefaultButton = btnCheck.UniqueID;

            if (IsPostBack)
            {
                Title = (string)ViewState["Title"];
            }
            else
            {
                // перевод веб-страницы
                Localization.TranslatePage(this, "Scada.Web.WFrmEvCheck");

                // определение параметров квитирования
                DateTime date;
                int evNum;

                try
                {
                    date = new DateTime(int.Parse(Request["year"]), int.Parse(Request["month"]),
                        int.Parse(Request["day"]));
                    ViewState.Add("Date", date);
                }
                catch
                {
                    throw new Exception(WebPhrases.IncorrectEvDate);
                }

                try
                {
                    evNum = int.Parse(Request["evNum"]);
                    ViewState.Add("EvNum", evNum);
                }
                catch
                {
                    throw new Exception(WebPhrases.IncorrectEvNum);
                }

                // вывод номера и даты события
                lblNum.Text = evNum.ToString();
                lblDate.Text = date.ToString("d", Localization.Culture);

                // получение квитируемого события
                EventTableLight eventTable;
                AppData.MainData.RefreshEvents(date, out eventTable);
                EventTableLight.Event ev = AppData.MainData.GetEventByNum(eventTable, evNum);

                if (ev == null)
                {
                    lblMessage.Text = string.Format(WebPhrases.ErrorFormat, WebPhrases.EventNotFound);
                    lblMessage.Visible = true;

                    lblTime.Text = lblObj.Text = lblKP.Text = lblCnl.Text = lblEvent.Text = "";
                    btnCheck.Enabled = false;
                }
                else
                {
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

                    // проверка загрузки представления и прав на квитирование
                    if (view == null)
                        throw new Exception(WebPhrases.UnableLoadView);
                    else if (!right.CtrlRight || userData.Role == ServerComm.Roles.Custom && 
                        !view.CnlList.Contains(ev.CnlNum))
                        throw new Exception(CommonPhrases.NoRights);

                    MainData.EventView eventView = AppData.MainData.ConvertEvent(ev);
                    lblTime.Text = eventView.Time;
                    lblObj.Text = eventView.Obj;
                    lblKP.Text = eventView.KP;
                    lblCnl.Text = eventView.Cnl;
                    lblEvent.Text = eventView.Text;

                    lblNum.ForeColor = lblDate.ForeColor = lblTime.ForeColor = lblObj.ForeColor =
                        lblCnl.ForeColor = lblEvent.ForeColor = Color.FromName(eventView.Color);
                }
            }
        }

        protected void btnCheck_Click(object sender, EventArgs e)
        {
            // отправка команды квитирования
            bool result;

            if (AppData.MainData.ServerComm.CheckEvent(UserData.GetUserData().UserID, 
                (DateTime)ViewState["Date"], (int)ViewState["EvNum"], out result))
            {
                mvMain.ActiveViewIndex = 1;

                if (!result)
                {
                    lblResultSuccessful.Visible = false;
                    lblResultFailed.Visible = true;
                }
            }
            else
            {
                lblMessage.Text = string.Format(WebPhrases.ErrorFormat, WebPhrases.ServerUnavailable);
                lblMessage.Visible = true;
            }
        }
    }
}