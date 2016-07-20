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
using Scada.Data.Tables;
using Scada.UI;
using System;
using System.Web.UI.WebControls;

namespace Scada.Web.plugins.Table
{
    /// <summary>
    /// Event acknowledgement dialog web form
    /// <para>Веб-форма диалога квитирования события</para>
    /// </summary>
    public partial class WFrmEventAck : System.Web.UI.Page
    {
        private DateTime evDate; // дата события
        private int evNum;       // номер события

        
        /// <summary>
        /// Скрыть сообщение об ошибке
        /// </summary>
        private void HideErrMsg()
        {
            pnlErrMsg.Visible = false;
            lblEventNotFound.Visible = false;
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
            AppData appData = AppData.GetAppData();
            UserData userData = UserData.GetUserData();

            // проверка входа в систему
            if (!userData.LoggedOn)
                throw new ScadaException(CommonPhrases.NoRights);

            // скрытие сообщения об ошибке
            HideErrMsg();

            if (!IsPostBack)
            {
                // перевод веб-страницы
                Translator.TranslatePage(Page, "Scada.Web.Plugins.Table.WFrmEventAck");

                // получение параметров запроса
                int viewID;
                int.TryParse(Request.QueryString["viewID"], out viewID);
                evDate = WebUtils.GetDateFromQueryString(Request);
                int.TryParse(Request.QueryString["evNum"], out evNum);

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
                    /*if (!userData.UserRights.GetViewRights(viewID).ControlRight ||
                        !userData.WebSettings.CmdEnabled)
                        throw new ScadaException(CommonPhrases.NoRights);

                    Type viewType = userData.UserViews.GetViewType(viewID);
                    BaseView view = appData.ViewCache.GetView(viewType, viewID, true);

                    if (!view.ContainsCtrlCnl(ev.CnlNum))
                        throw new ScadaException(CommonPhrases.NoRights);*/

                    // вывод информации по событию
                    pnlInfo.Visible = true;
                    lblNum.Text = ev.Number.ToString();
                    lblTime.Text = ev.DateTime.ToLocalizedString();
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {

        }
    }
}