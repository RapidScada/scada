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
 * Summary  : Command dialog web form
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
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Scada.Web.Plugins.Table
{
    /// <summary>
    /// Command dialog web form
    /// <para>Веб-форма диалога отправки команды ТУ</para>
    /// </summary>
    public partial class WFrmCommand : System.Web.UI.Page
    {
        /// <summary>
        /// Дискретная команда
        /// </summary>
        private struct DiscreteCmd
        {
            public int Val { get; set; }
            public string Text { get; set; }
        }


        private AppData appData;   // общие данные веб-приложения
        private UserData userData; // данные пользователя приложения
        private int ctrlCnlNum;    // номер канала управления


        /// <summary>
        /// Получить список дискретных команд
        /// </summary>
        private List<DiscreteCmd> GetDiscreteCmds(string[] cmdValArr)
        {
            List<DiscreteCmd> discreteCmds = new List<DiscreteCmd>();
            for (int i = 0, len = cmdValArr.Length; i < len; i++)
            {
                discreteCmds.Add(new DiscreteCmd() { Val = i, Text = cmdValArr[i] });
            }
            return discreteCmds;
        }

        /// <summary>
        /// Проверить пароль, если он используется
        /// </summary>
        private bool CheckPassword()
        {
            if (pnlPassword.Visible)
            {
                //bool pwdOK = appData.
                return true;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Отправить стандартную команду
        /// </summary>
        private void SendStandardCmd(double cmdVal)
        {
            bool result;
            bool sendOK = appData.ServerComm.SendStandardCommand(
                userData.UserProps.UserID, ctrlCnlNum, cmdVal, out result);
            ShowCmdResult(sendOK, result);
        }

        /// <summary>
        /// Отобразить результат выполнения команды
        /// </summary>
        private void ShowCmdResult(bool sendOK, bool result)
        {
            mvCommand.SetActiveView(viewCmdResult);

            if (sendOK && result)
            {
                pnlSuccess.Visible = true;
                ClientScript.RegisterStartupScript(GetType(), "Startup", "startDowncount();", true);
            }
            else
            {
                pnlError.Visible = true;
                if (sendOK)
                    lblCmdRejected.Visible = true;
                else
                    lblCmdNotSent.Visible = true;
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            appData = AppData.GetAppData();
            userData = UserData.GetUserData();

            // получение параметров запроса
            int viewID;
            int.TryParse(Request.QueryString["viewID"], out viewID);
            int.TryParse(Request.QueryString["ctrlCnlNum"], out ctrlCnlNum);

            // проверка прав
            if (!userData.LoggedOn ||
                !userData.UserRights.GetViewRights(viewID).ControlRight ||
                !userData.WebSettings.CmdEnabled)
                throw new ScadaException(CommonPhrases.NoRights);

            Type viewType = userData.UserViews.GetViewType(viewID);
            BaseView view = appData.ViewCache.GetView(viewType, viewID, true);

            if (!view.ContainsCtrlCnl(ctrlCnlNum))
                throw new ScadaException(CommonPhrases.NoRights);

            if (!IsPostBack)
            {
                // перевод веб-страницы
                Translator.TranslatePage(Page, "Scada.Web.Plugins.Table.WFrmCommand");

                // получение канала управления
                CtrlCnlProps ctrlCnlProps = appData.DataAccess.GetCtrlCnlProps(ctrlCnlNum);

                if (ctrlCnlProps == null)
                {
                    // вывести сообщение, что канал не найден, 
                    // элементы управления ввода команды скрыты по умолчанию
                    lblCtrlCnl.Visible = false;
                    lblCtrlCnlNotFound.Visible = true;
                }
                else
                {
                    // вывод информации по каналу управления
                    lblCtrlCnl.Text = string.Format("[{0}] {1}", ctrlCnlProps.CtrlCnlNum, ctrlCnlProps.CtrlCnlName);
                    lblObj.Text = ctrlCnlProps.ObjNum > 0 ? 
                        string.Format("[{0}] {1}", ctrlCnlProps.ObjNum, ctrlCnlProps.ObjName) : "";
                    lblDev.Text = ctrlCnlProps.KPNum > 0 ?
                        string.Format("[{0}] {1}", ctrlCnlProps.KPNum, ctrlCnlProps.KPName) : "";

                    // установка видимости поля для ввода пароля
                    pnlPassword.Visible = userData.WebSettings.CmdPassword;

                    // настройка элементов управления в зависимости от типа команды
                    switch (ctrlCnlProps.CmdTypeID)
                    {
                        case BaseValues.CmdTypes.Standard:
                            if (ctrlCnlProps.CmdValArr == null)
                            {
                                pnlRealValue.Visible = true;
                            }
                            else
                            {
                                repCommands.DataSource = GetDiscreteCmds(ctrlCnlProps.CmdValArr);
                                repCommands.DataBind();
                                pnlDiscreteValue.Visible = true;
                            }
                            hidCmdEnabled.Value = "true";
                            break;
                        case BaseValues.CmdTypes.Binary:
                            break;
                        case BaseValues.CmdTypes.Request:
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            appData.Log.WriteLine("!!!btnSubmit_Click");
        }

        protected void repCommands_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (CheckPassword())
            {
                Button btn = (Button)e.CommandSource;
                int cmdVal = int.Parse(btn.Attributes["data-cmdval"]);
                SendStandardCmd(cmdVal);
            }
        }
    }
}