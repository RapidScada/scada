/*
 * Copyright 2020 Mikhail Shiryaev
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
 * Summary  : Application error web form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2020
 */

using Scada.UI;
using System;
using System.Text;
using System.Web;
using Utils;

namespace Scada.Web
{
    /// <summary>
    /// Application error web form.
    /// <para>Веб-форма ошибки приложения.</para>
    /// </summary>
    public partial class WFrmError : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string errDetails = "";

            try
            {
                // настройка выходного потока
                Response.ClearContent();
                Response.TrySkipIisCustomErrors = true;

                // перевод веб-страницы
                Translator.TranslatePage(this, "Scada.Web.WFrmError");
                lblProductName.Text = CommonPhrases.ProductName;

                // определение сообщения об ошибке
                Exception ex = Server.GetLastError();

                if (ex != null)
                {
                    if (ex.InnerException != null)
                        ex = ex.InnerException;

                    if (ex is HttpException)
                        Response.StatusCode = ((HttpException)ex).GetHttpCode();

                    errDetails = ex.Message;
                }

                // вывод информации об ошибке в журнал приложения
                StringBuilder sbLogMsg = new StringBuilder();
                sbLogMsg
                    .Append(Localization.UseRussian ? "Ошибка приложения: " : "Application error: ");

                if (ex != null)
                    sbLogMsg
                        .AppendLine()
                        .Append(ex.ToString());

                sbLogMsg
                    .AppendLine()
                    .Append(Localization.UseRussian ? "Страница: " : "Page: ")
                    .Append(Request.Url.AbsoluteUri);

                if (Context.Session != null)
                {
                    UserData userData = UserData.GetUserData();
                    if (userData != null)
                        sbLogMsg
                            .AppendLine()
                            .Append(Localization.UseRussian ? "Пользователь: " : "User: ")
                            .Append(userData.LoggedOn ?
                                userData.UserProps.UserName :
                                (Localization.UseRussian ? "вход не выполнен" : "not logged on"));
                }

                AppData.GetAppData().Log.WriteAction(sbLogMsg.ToString(), Log.ActTypes.Exception);
            }
            finally
            {
                // вывод сообщения об ошибке на форму
                if (errDetails != "")
                {
                    lblErrDetails.Text = WebUtils.HtmlEncodeWithBreak(errDetails);
                }
                else
                {
                    lblErrDetailsCaption.Visible = lblErrDetails.Visible = false;
                }

                Server.ClearError();
            }
        }
    }
}