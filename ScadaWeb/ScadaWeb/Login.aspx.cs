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
 * Summary  : Login web form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2005
 * Modified : 2014
 */

using System;
using System.Collections.Generic;
using System.Web;

namespace Scada.Web
{
    /// <summary>
    /// Login web form
    /// <para>Веб-форма входа в систему</para>
    /// </summary>
    public partial class WFrmLogin : System.Web.UI.Page
    {
        /// <summary>
        /// Добавить скрипт отображения сообщения об ошибке при загрузке
        /// </summary>
        private void AddShowErrorScript(string message)
        {
            message = message.Replace("'", "\\'");
            ClientScript.RegisterStartupScript(this.GetType(), "Startup", "alert('" + message + "');", true);
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            // получение данных пользователя
            UserData userData = UserData.GetUserData();

            // установка активного элемента формы и кнопки по умолчанию
            Form.DefaultFocus = txtLogin.ClientID;
            Form.DefaultButton = btnEnter.UniqueID;

            if (IsPostBack)
            {
                Title = (string)ViewState["Title"];
            }
            else
            {
                // проверка входа в систему
                userData.CheckLoggedOn(Context, false);

                // перевод веб-страницы
                Localization.TranslatePage(this, "Scada.Web.WFrmLogin");
                ViewState["Title"] = Title;

                if (userData.LoggedOn)
                {
                    // обработка повторного входа в систему
                    lblLoginTitle.Visible = false;
                    lblLoggedOnTitle.Visible = true;
                    txtLogin.Text = userData.UserLogin;
                    txtLogin.Enabled = txtPassword.Enabled = false;
                    btnExit.Visible = true;
                }
                else
                {
                    // извлечение из cookie адреса имени пользователя
                    txtLogin.Text = UserDataExt.RestoreUserLogin(Context);
                }

                // настройка элементов управления
                List<ViewSettings.ViewSet> viewSetList = userData.ViewSettings.ViewSetList;
                foreach (ViewSettings.ViewSet viewSet in viewSetList)
                    ddlViewSet.Items.Add(viewSet.Name);

                int viewSetCnt = viewSetList.Count;
                ddlViewSet.Enabled = viewSetCnt > 1;
                btnEnter.Enabled = viewSetCnt > 0;
                chkRememberUser.Visible = AppData.WebSettings.RemEnabled && !btnExit.Visible;
                chkRememberUser.Enabled = btnEnter.Enabled;

                // получение имени выбранного представления из параметров запроса
                string viewSetName = Request["viewSetName"];
                bool viewSetSelected = !string.IsNullOrEmpty(viewSetName);

                if (viewSetCnt == 1)
                {
                    ddlViewSet.SelectedIndex = 0;
                }
                else if (viewSetCnt > 1)
                {
                    // установка выбранного набора представлений в соответствии с параметрами запроса или cookie
                    int selInd = 0;

                    if (string.IsNullOrEmpty(viewSetName))
                    {
                        HttpCookie cookie = Request.Cookies["ScadaViewSet"];
                        if (cookie != null)
                            viewSetName = Server.UrlDecode(cookie.Value);
                    }

                    if (!string.IsNullOrEmpty(viewSetName))
                    {
                        viewSetName = viewSetName.ToLower();

                        for (int i = 0; i < viewSetCnt; i++)
                        {
                            if (viewSetList[i].Name.ToLower() == viewSetName)
                            {
                                selInd = i;
                                break;
                            }
                        }
                    }

                    ddlViewSet.SelectedIndex = selInd;
                }

                // переход на страницу данных, если пользователь уже вошёл в систему и 
                // набор представлений задан в параметрах запроса
                if (userData.LoggedOn && viewSetSelected && viewSetName == ddlViewSet.SelectedValue.ToLower())
                    btnEnter_Click(null, null);
            }
        }

        protected void btnEnter_Click(object sender, System.EventArgs e)
        {
            UserData userData = UserData.GetUserData();
            bool alreadyLoggedOn = userData.LoggedOn;
            string errMsg = "";

            if (alreadyLoggedOn || userData.Login(txtLogin.Text, txtPassword.Text, out errMsg))
            {
                // проверка прав пользователя на выбранный набор представлений
                int viewSetIndex = ddlViewSet.SelectedIndex;
                string viewSetName = userData.ViewSettings.ViewSetList[viewSetIndex].Name;
                MainData.Right viewSetRight = userData.GetViewSetRight(viewSetIndex);

                if (viewSetRight.ViewRight)
                {
                    // сохранение в cookie информации о входе пользователя
                    if (!alreadyLoggedOn)
                        userData.RememberUser(Context, !chkRememberUser.Checked);

                    // сохранение в cookie выбранного набора представлений
                    HttpCookie cookie = new HttpCookie("ScadaViewSet");
                    cookie.Value = Server.UrlEncode(viewSetName);
                    cookie.Expires = DateTime.Now.Add(ScadaUtils.CookieExpiration);
                    Response.SetCookie(cookie);

                    // переход на страницу данных
                    Response.Redirect("~/Main.aspx?viewSet=" + viewSetIndex, false);
                }
                else
                {
                    errMsg = WebPhrases.NoViewSetRights;
                }
            }
            
            if (!string.IsNullOrEmpty(errMsg))
            {
                AddShowErrorScript(string.Format(WebPhrases.UnableLogin, errMsg));
            }
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            UserData userData = UserData.GetUserData();
            userData.Logout();
            UserDataExt.ForgetUserLoggedOn(Context);

            lblLoginTitle.Visible = true;
            lblLoggedOnTitle.Visible = false;
            txtLogin.Text = "";
            txtLogin.Enabled = txtPassword.Enabled = true;
            chkRememberUser.Visible = AppData.WebSettings.RemEnabled;
            btnExit.Visible = false;
        }
    }
}