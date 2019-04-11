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
 * Module   : SCADA-Web
 * Summary  : User info web form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

using Scada.Data.Models;
using Scada.UI;
using System;

namespace Scada.Web
{
    /// <summary>
    /// User info web form
    /// <para>Веб-форма информации о пользователе</para>
    /// </summary>
    public partial class WFrmUser : System.Web.UI.Page
    {
        /// <summary>
        /// Преобразовать наличие права в строку
        /// </summary>
        private string RightToStr(bool right)
        {
            return right ? WebPhrases.RightYes : WebPhrases.RightNo;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                AppData appData = AppData.GetAppData();
                UserData userData = UserData.GetUserData();
                userData.CheckLoggedOn(true);

                // получение ид. пользователя из параметров запроса
                int userID;
                if (!int.TryParse(Request.QueryString["userID"], out userID))
                    userID = userData.UserProps.UserID;

                // проверка прав
                bool self = userData.UserProps.UserID == userID;
                if (!(self || userData.UserRights.ConfigRight))
                    throw new ScadaException(CommonPhrases.NoRights);

                // перевод веб-страницы
                Translator.TranslatePage(this, "Scada.Web.WFrmUser");

                // получение свойств и прав пользователя
                UserProps userProps;
                UserRights userRights;

                if (self)
                {
                    userProps = userData.UserProps;
                    userRights = userData.UserRights;
                }
                else
                {
                    userProps = appData.DataAccess.GetUserProps(userID);

                    if (userProps == null)
                    {
                        throw new ScadaException(Localization.UseRussian ?
                            "Пользователь не найден" :
                            "User not found");
                    }
                    else
                    {
                        userRights = new UserRights();
                        userRights.InitGeneralRights(userProps.RoleID);
                    }
                }

                // вывод информации на форму
                lblUserName.Text = userProps.UserName;
                lblRoleName.Text = userProps.RoleName;

                lblViewAllRight.Text = RightToStr(userRights.ViewAllRight);
                lblControlAllRight.Text = RightToStr(userRights.ControlAllRight);
                lblConfigRight.Text = RightToStr(userRights.ConfigRight);
            }
        }
    }
}