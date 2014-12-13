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
 * Summary  : Sending command web form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2008
 * Modified : 2014
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using Scada.Client;

namespace Scada.Web
{
    /// <summary>
    /// Sending command web form
    /// <para>Веб-форма отправки команды ТУ</para>
    /// </summary>
    public partial class WFrmCmdSend : System.Web.UI.Page
    {
        /// <summary>
        /// Информация о значении команды
        /// </summary>
        private struct CmdValInfo
        {
            public int CndInd { get; set; }
            public int CmdVal { get; set; }
            public string CmdText { get; set; }
        }


        /// <summary>
        /// Отправить команду, получив её параметры из элементов управления
        /// </summary>
        private void SendCmd(int ctrlCnlNum, double cmdVal, byte[] cmdData)
        {
            bool sendOk;
            bool result;
            ServerComm serverComm = AppData.MainData.ServerComm;
            int userID = UserData.GetUserData().UserID;
            View activeView = mvCommand.Visible ? mvCommand.GetActiveView() : null;

            if (activeView == viewStandCmd1 || activeView == viewStandCmd1Simple || activeView == viewStandCmd2)
            {
                sendOk = serverComm.SendStandardCommand(userID, ctrlCnlNum, cmdVal, out result);
            }
            else if (activeView == viewBinCmd)
            {
                sendOk = serverComm.SendBinaryCommand(userID, ctrlCnlNum, cmdData, out result);
            }
            else
            {
                int kpNum;
                try { kpNum = (int)ViewState["KPNum"]; }
                catch { kpNum = 0; }
                sendOk = serverComm.SendRequestCommand(userID, ctrlCnlNum, kpNum, out result);
            }

            if (sendOk)
            {
                mvMain.SetActiveView(viewResult);

                if (result)
                {
                    // добавление скрипта, чтобы закрыть окно команды при простой отправке
                    if (activeView == viewStandCmd1Simple)
                        ClientScript.RegisterStartupScript(this.GetType(), "Startup", "window.close();", true);
                }
                else
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


        protected void Page_Load(object sender, EventArgs e)
        {
            // получение данных пользователя
            UserData userData = UserData.GetUserData();

            // проверка входа в систему
            if (!userData.LoggedOn)
                throw new Exception(WebPhrases.NotLoggedOn);

            // определение индексов выбранного представления
            int viewSetIndex, viewIndex;
            if (!int.TryParse(Request["viewSet"], out viewSetIndex))
                viewSetIndex = -1;
            if (!int.TryParse(Request["view"], out viewIndex))
                viewIndex = -1;

            // определение номера канала управления
            int ctrlCnlNum;
            int.TryParse(Request["ctrlCnlNum"], out ctrlCnlNum);

            // получение представления и прав пользователя на него
            BaseView view;
            MainData.Right right;
            userData.GetView(null, viewSetIndex, viewIndex, out view, out right);

            // проверка загрузки представления и прав на отправку команды
            if (view == null)
                throw new Exception(WebPhrases.UnableLoadView);
            else if (!right.CtrlRight || !AppData.WebSettings.CmdEnabled || !view.CtrlCnlList.Contains(ctrlCnlNum))
                throw new Exception(CommonPhrases.NoRights);

            // очистка сообщения об ошибке
            lblMessage.Text = "";
            lblMessage.Visible = false;

            // установка кнопки по умолчанию
            Form.DefaultButton = btnExecute.UniqueID;

            if (IsPostBack)
            {
                // восстановление заголовка страницы
                Title = (string)ViewState["Title"];

                // установка активного элемента формы по умолчанию
                if (pnlPassword.Visible)
                {
                    Form.DefaultFocus = txtPassword.ClientID;
                }
                else
                {
                    View activeView = mvCommand.Visible ? mvCommand.GetActiveView() : null;
                    if (activeView == viewStandCmd2)
                        Form.DefaultFocus = txtCmdVal.ClientID;
                    else if (activeView == viewBinCmd)
                        Form.DefaultFocus = txtCmdData.ClientID;
                }
            }
            else
            {
                // перевод веб-страницы
                Localization.TranslatePage(this, "Scada.Web.WFrmCmdSend");
                ViewState["Title"] = Title;

                // вывод свойств канала управления
                CtrlCnlProps ctrlCnlProps = AppData.MainData.GetCtrlCnlProps(ctrlCnlNum);

                if (ctrlCnlProps == null)
                {
                    throw new Exception(string.Format(WebPhrases.OutCnlNotFound, ctrlCnlNum));
                }
                else
                {
                    lblCtrlCnl.Text = ctrlCnlProps.CtrlCnlName;
                    lblObj.Text = ctrlCnlProps.ObjName;
                    lblKP.Text = ctrlCnlProps.KPName;

                    ViewState.Add("CtrlCnlNum", ctrlCnlProps.CtrlCnlNum);
                    ViewState.Add("KPNum", ctrlCnlProps.KPNum);

                    // установка видимости поля для ввода пароля
                    pnlPassword.Visible = !AppData.WebSettings.SimpleCmd;

                    // установка типа команды и определение фокуса по умолчанию
                    string defaultFocus = "";
                    if (ctrlCnlProps.CmdTypeID == 0 /*стандартная команда*/)
                    {
                        if (ctrlCnlProps.CmdValArr == null)
                        {
                            // установка представления для ввода значения команды вручную
                            mvCommand.SetActiveView(viewStandCmd2);
                            defaultFocus = txtCmdVal.ClientID;
                        }
                        else if (AppData.WebSettings.SimpleCmd)
                        {
                            // настройка элементов для выполнения команды по нажатию кнопки со значением
                            List<CmdValInfo> cmdValInfoList = new List<CmdValInfo>();
                            for (int i = 0; i < ctrlCnlProps.CmdValArr.Length; i++)
                            {
                                string cmdText = ctrlCnlProps.CmdValArr[i];
                                if (cmdText != "")
                                    cmdValInfoList.Add(new CmdValInfo() { 
                                        CndInd = cmdValInfoList.Count, CmdVal = i, CmdText = cmdText });
                            }
                            
                            mvCommand.SetActiveView(viewStandCmd1Simple);
                            repCmdVal.DataSource = cmdValInfoList;
                            repCmdVal.DataBind();
                            btnExecute.Visible = false;
                        }
                        else
                        {
                            // настройка элементов для выбора значения команды с помощью радиокнопок
                            mvCommand.SetActiveView(viewStandCmd1);

                            for (int i = 0; i < ctrlCnlProps.CmdValArr.Length; i++)
                            {
                                string cmdVal = ctrlCnlProps.CmdValArr[i];
                                if (cmdVal != "")
                                    rblCmdVal.Items.Add(new ListItem(cmdVal, i.ToString()));
                            }
                        }
                    }
                    else if (ctrlCnlProps.CmdTypeID == 1 /*бинарная команда*/)
                    {
                        mvCommand.SetActiveView(viewBinCmd);
                        defaultFocus = txtCmdData.ClientID;
                    }
                    else // команда опроса КП
                    {
                        mvCommand.Visible = false;
                    }

                    // установка активного элемента формы по умолчанию
                    if (pnlPassword.Visible)
                        Form.DefaultFocus = txtPassword.ClientID;
                    else if (defaultFocus != "")
                        Form.DefaultFocus = defaultFocus;
                }
            }
        }

        protected void repCmdVal_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            // настройка кнопки, соответствующей значению команды
            CmdValInfo cmdValInfo = (CmdValInfo)e.Item.DataItem;
            Button btnCmd = (Button)e.Item.FindControl("btnCmd");
            btnCmd.CommandArgument = cmdValInfo.CmdVal.ToString();
            btnCmd.Text = cmdValInfo.CmdText;
            if (cmdValInfo.CndInd % 2 == 0) // не больше 2 кнопок в строке
                btnCmd.CssClass = "lineBegin";
        }

        protected void repCmdVal_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            // выполнение стандартной команды по кнопке со значением
            if (e.CommandName == "Exec")
                SendCmd((int)ViewState["CtrlCnlNum"], int.Parse(e.CommandArgument.ToString()), null);
        }

        protected void btnExecute_Click(object sender, EventArgs e)
        {
            // проверка пароля
            if (pnlPassword.Visible)
            {
                int roleID;
                string errMsg;
                AppData.MainData.CheckUser(UserData.GetUserData().UserLogin, txtPassword.Text, true,
                    out roleID, out errMsg);
                if (errMsg != "")
                    lblMessage.Text = string.Format(WebPhrases.ErrorFormat, errMsg);
            }

            // получение и проверка параметров команды
            int ctrlCnlNum = (int)ViewState["CtrlCnlNum"];
            double cmdVal = 0.0;
            byte[] cmdData = null;
            View activeView = mvCommand.Visible ? mvCommand.GetActiveView() : null;

            if (lblMessage.Text == "")
            {
                if (activeView == viewStandCmd1)
                {
                    if (!double.TryParse(rblCmdVal.SelectedValue, out cmdVal))
                        lblMessage.Text = string.Format(WebPhrases.ErrorFormat, WebPhrases.CmdNotSelected);
                }
                else if (activeView == viewStandCmd2)
                {
                    cmdVal = ScadaUtils.StrToDouble(txtCmdVal.Text);
                    if (double.IsNaN(cmdVal))
                        lblMessage.Text = string.Format(WebPhrases.ErrorFormat, WebPhrases.IncorrectCmdVal);
                }
                else if (activeView == viewBinCmd)
                {
                    if (txtCmdData.Text.Length > 0)
                    {
                        if (rbStr.Checked)
                        {
                            cmdData = Encoding.Default.GetBytes(txtCmdData.Text);
                        }
                        else
                        {
                            if (!ScadaUtils.HexToBytes(txtCmdData.Text, out cmdData))
                                lblMessage.Text = string.Format(WebPhrases.ErrorFormat, 
                                    WebPhrases.IncorrectCmdData);
                        }
                    }
                    else
                    {
                        lblMessage.Text = string.Format(WebPhrases.ErrorFormat, WebPhrases.EmptyCmdData);
                    }
                }
            }

            // отправка команды
            if (lblMessage.Text == "")
                SendCmd(ctrlCnlNum, cmdVal, cmdData);

            // отображение сообщения об ошибке
            if (lblMessage.Text != "")
                lblMessage.Visible = true;
        }
    }
}