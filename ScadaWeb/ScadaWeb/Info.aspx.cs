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
 * Summary  : Application information web form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2005
 * Modified : 2014
 */

using System;

namespace Scada.Web
{
    /// <summary>
    /// Application information web form
    /// <para>Веб-форма информации о приложении</para>
    /// </summary>
    public partial class WFrmInfo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // перевод веб-страницы
            Localization.TranslatePage(this, "Scada.Web.WFrmInfo");

            // вывод информации на форму
            UserData userData = UserData.GetUserData();
            lblServer.Text = AppData.MainData.ServerComm.CommSettings.ServerHost;

            if (userData.LoggedOn)
            {
                lblUser.Text = userData.UserLogin + " (" + userData.RoleName + ")";
            }
            else
            {
                lblUser.Visible = false;
                lblUserNotLoggedOn.Visible = true;
            }
        }
    }
}