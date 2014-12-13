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
 * Summary  : Events web form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2012
 * Modified : 2014
 */

using System;
using Scada.Client;

namespace Scada.Web
{
    /// <summary>
    /// Events web form
    /// <para>Веб-форма событий</para>
    /// </summary>
    public partial class WFrmEvents: System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // отключение кэширования страницы
            ScadaUtils.DisablePageCache(Response);

            // перевод веб-страницы
            Localization.TranslatePage(this, "Scada.Web.WFrmEvents");

            // получение данных пользователя
            UserData userData = UserData.GetUserData();

            // установка фильтра событий
            if (userData.Role == ServerComm.Roles.Custom)
            {
                rbEvView.Checked = true;
                rbEvAll.Enabled = false;
            }
            else if (AppData.WebSettings.EventFltr)
            {
                rbEvView.Checked = true;
            }
            else
            {
                rbEvAll.Checked = true;
            }
        }
    }
}