/*
 * Copyright 2021 Mikhail Shiryaev
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
 * Module   : PlgConfig
 * Summary  : Web application configuration web form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2021
 */

using Scada.Client;
using Scada.UI;
using Scada.Web.Shell;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.UI.WebControls;

namespace Scada.Web.Plugins.Config
{
    /// <summary>
    /// Web application configuration web form.
    /// <para>Веб-форма конфигурации веб-приложения.</para>
    /// </summary>
    public partial class WFrmWebConfig : System.Web.UI.Page
    {
        private AppData appData;   // общие данные веб-приложения
        private UserData userData; // данные пользователя приложения


        /// <summary>
        /// Загрузить и отобразить настройки
        /// </summary>
        private void LoadSettings()
        {
            WebSettings webSettings = new WebSettings();

            if (!webSettings.LoadFromFile(Path.Combine(appData.AppDirs.ConfigDir, WebSettings.DefFileName), 
                out string errMsg))
            {
                appData.Log.WriteError(errMsg);
                pnlErrMsg.ShowAlert(errMsg);
            }

            SettingsToControls(webSettings);
        }

        /// <summary>
        /// Сохранить настройки
        /// </summary>
        private void SaveSettings()
        {
            // загрузка актуальных настроек
            WebSettings webSettings = new WebSettings();
            string fileName = Path.Combine(appData.AppDirs.ConfigDir, WebSettings.DefFileName);

            if (!webSettings.LoadFromFile(fileName, out string errMsg))
                appData.Log.WriteError(errMsg);

            // проверка и сохранение настроек
            if (ControlsToSettings(webSettings, out errMsg))
            {
                if (webSettings.SaveToFile(fileName, out errMsg))
                {
                    userData.ReLogin();
                    userData.CheckLoggedOn(true);
                    pnlSuccMsg.ShowAlert(PlgPhrases.ConfigSaved);
                }
                else
                {
                    appData.Log.WriteError(errMsg);
                    pnlErrMsg.ShowAlert(errMsg);
                }
            }
            else
            {
                pnlErrMsg.ShowAlert(errMsg);
            }
        }

        /// <summary>
        /// Отобразить настройки
        /// </summary>
        private void SettingsToControls(WebSettings webSettings)
        {
            CommSettings commSettings = webSettings.CommSettings;
            txtServerHost.Text = commSettings.ServerHost;
            txtServerPort.Text = commSettings.ServerPort.ToString();
            txtServerTimeout.Text = commSettings.ServerTimeout.ToString();
            txtServerUser.Text = commSettings.ServerUser;
            txtServerPwd.Text = "";

            txtCulture.Text = webSettings.Culture;
            txtDataRefrRate.Text = webSettings.DataRefrRate.ToString();
            txtArcRefrRate.Text = webSettings.ArcRefrRate.ToString();
            txtDispEventCnt.Text = webSettings.DispEventCnt.ToString();
            txtChartGap.Text = webSettings.ChartGap.ToString();
            txtStartPage.Text = webSettings.StartPage;
            chkCmdEnabled.Checked = webSettings.CmdEnabled;
            chkCmdPassword.Checked = webSettings.CmdPassword;
            chkRemEnabled.Checked = webSettings.RemEnabled;
            chkViewsFromBase.Checked = webSettings.ViewsFromBase;
            chkShareStats.Checked = webSettings.ShareStats;

            // заполнение списков плагинов
            ddlChartScript.Items.Clear();
            ddlCmdScript.Items.Clear();
            ddlEventAckScript.Items.Clear();
            ddlUserProfile.Items.Clear();

            ddlChartScript.Items.Add("");
            ddlCmdScript.Items.Add("");
            ddlEventAckScript.Items.Add("");
            ddlUserProfile.Items.Add("");

            string curChartScriptPath = webSettings.ScriptPaths.ChartScriptPath;
            string curCmdScriptPath = webSettings.ScriptPaths.CmdScriptPath;
            string curEventAckScriptPath = webSettings.ScriptPaths.EventAckScriptPath;
            string curUserProfilePath = webSettings.ScriptPaths.UserProfilePath;
            int chartScriptInd = 0;
            int cmdScriptInd = 0;
            int eventAckScriptInd = 0;
            int userProfileInd = 0;

            foreach (PluginSpec pluginSpec in userData.PluginSpecs)
            {
                ScriptPaths scriptPaths = pluginSpec.ScriptPaths;

                if (scriptPaths != null)
                {
                    if (!string.IsNullOrEmpty(scriptPaths.ChartScriptPath))
                    {
                        ddlChartScript.Items.Add(new ListItem(pluginSpec.Name, scriptPaths.ChartScriptPath));
                        if (string.Equals(curChartScriptPath, scriptPaths.ChartScriptPath, 
                            StringComparison.OrdinalIgnoreCase))
                            chartScriptInd = ddlChartScript.Items.Count - 1;
                    }

                    if (!string.IsNullOrEmpty(scriptPaths.CmdScriptPath))
                    {
                        ddlCmdScript.Items.Add(new ListItem(pluginSpec.Name, scriptPaths.CmdScriptPath));
                        if (string.Equals(curCmdScriptPath, scriptPaths.CmdScriptPath, 
                            StringComparison.OrdinalIgnoreCase))
                            cmdScriptInd = ddlCmdScript.Items.Count - 1;
                    }

                    if (!string.IsNullOrEmpty(scriptPaths.EventAckScriptPath))
                    {
                        ddlEventAckScript.Items.Add(new ListItem(pluginSpec.Name, scriptPaths.EventAckScriptPath));
                        if (string.Equals(curEventAckScriptPath, scriptPaths.EventAckScriptPath, 
                            StringComparison.OrdinalIgnoreCase))
                            eventAckScriptInd = ddlEventAckScript.Items.Count - 1;
                    }

                    if (!string.IsNullOrEmpty(scriptPaths.UserProfilePath))
                    {
                        ddlUserProfile.Items.Add(new ListItem(pluginSpec.Name, scriptPaths.UserProfilePath));
                        if (string.Equals(curUserProfilePath, scriptPaths.UserProfilePath,
                            StringComparison.OrdinalIgnoreCase))
                            userProfileInd = ddlUserProfile.Items.Count - 1;
                    }
                }
            }

            // установка выбранных элементов выпадающих списков
            if (chartScriptInd > 0 || string.IsNullOrEmpty(curChartScriptPath))
            {
                ddlChartScript.SelectedIndex = chartScriptInd;
            }
            else
            {
                ddlChartScript.Items.Add(new ListItem(PlgPhrases.UnknownPlugin, curChartScriptPath));
                ddlChartScript.SelectedIndex = ddlChartScript.Items.Count - 1;
            }

            if (cmdScriptInd > 0 || string.IsNullOrEmpty(curCmdScriptPath))
            {
                ddlCmdScript.SelectedIndex = cmdScriptInd;
            }
            else
            {
                ddlCmdScript.Items.Add(new ListItem(PlgPhrases.UnknownPlugin, curCmdScriptPath));
                ddlCmdScript.SelectedIndex = ddlCmdScript.Items.Count - 1;
            }

            if (eventAckScriptInd > 0 || string.IsNullOrEmpty(curEventAckScriptPath))
            {
                ddlEventAckScript.SelectedIndex = eventAckScriptInd;
            }
            else
            {
                ddlEventAckScript.Items.Add(new ListItem(PlgPhrases.UnknownPlugin, curEventAckScriptPath));
                ddlEventAckScript.SelectedIndex = ddlEventAckScript.Items.Count - 1;
            }

            if (userProfileInd > 0 || string.IsNullOrEmpty(curUserProfilePath))
            {
                ddlUserProfile.SelectedIndex = userProfileInd;
            }
            else
            {
                ddlUserProfile.Items.Add(new ListItem(PlgPhrases.UnknownPlugin, curUserProfilePath));
                ddlUserProfile.SelectedIndex = ddlUserProfile.Items.Count - 1;
            }
        }

        /// <summary>
        /// Получить параметры настроек из элементов управления
        /// </summary>
        private bool ControlsToSettings(WebSettings webSettings, out string errMsg)
        {
            // проверка текстовых полей и установка числовых настроек
            CommSettings commSettings = webSettings.CommSettings;
            List<string> errFields = new List<string>();

            if (ValidateIntField(pnlServerPort, lblServerPort, txtServerPort, errFields, out int val))
                commSettings.ServerPort = val;

            if (ValidateIntField(pnlServerTimeout, lblServerTimeout, txtServerTimeout, errFields, out val))
                commSettings.ServerTimeout = val;

            if (ValidateIntField(pnlDataRefrRate, lblDataRefrRate, txtDataRefrRate, errFields, out val))
                webSettings.DataRefrRate = val;

            if (ValidateIntField(pnlArcRefrRate, lblArcRefrRate, txtArcRefrRate, errFields, out val))
                webSettings.ArcRefrRate = val;

            if (ValidateIntField(pnlDispEventCnt, lblDispEventCnt, txtDispEventCnt, errFields, out val))
                webSettings.DispEventCnt = val;

            if (ValidateIntField(pnlChartGap, lblChartGap, txtChartGap, errFields, out val))
                webSettings.ChartGap = val;

            if (errFields.Count > 0)
            {
                errMsg = PlgPhrases.IncorrectFields + "\n" + string.Join(",\n", errFields.ToArray());
                return false;
            }
            else
            {
                // установка настроек, не требующих проверки
                commSettings.ServerHost = txtServerHost.Text;
                commSettings.ServerUser = txtServerUser.Text;
                if (txtServerPwd.Text != "")
                    commSettings.ServerPwd = txtServerPwd.Text;

                webSettings.Culture = txtCulture.Text;
                webSettings.StartPage = txtStartPage.Text;
                webSettings.CmdEnabled = chkCmdEnabled.Checked;
                webSettings.CmdPassword = chkCmdPassword.Checked;
                webSettings.RemEnabled = chkRemEnabled.Checked;
                webSettings.ViewsFromBase = chkViewsFromBase.Checked;
                webSettings.ShareStats = chkShareStats.Checked;

                webSettings.ScriptPaths.ChartScriptPath = ddlChartScript.SelectedValue;
                webSettings.ScriptPaths.CmdScriptPath = ddlCmdScript.SelectedValue;
                webSettings.ScriptPaths.EventAckScriptPath = ddlEventAckScript.SelectedValue;
                webSettings.ScriptPaths.UserProfilePath = ddlUserProfile.SelectedValue;

                errMsg = "";
                return true;
            }
        }

        /// <summary>
        /// Проверить целочисленное поле
        /// </summary>
        private bool ValidateIntField(Panel pnlField, Label lblFieldCaption, TextBox txtFieldVal, 
            List<string> errFields, out int val)
        {
            if (int.TryParse(txtFieldVal.Text, out val))
            {
                return true;
            }
            else
            {
                errFields.Add(lblFieldCaption.Text);
                pnlField.CssClass += " has-error";
                return false;
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            appData = AppData.GetAppData();
            userData = UserData.GetUserData();

            // проверка входа в систему и прав
            userData.CheckLoggedOn(true);

            if (!userData.UserRights.ConfigRight)
                throw new ScadaException(CommonPhrases.NoRights);

            // скрытие сообщений
            pnlErrMsg.HideAlert();
            pnlSuccMsg.HideAlert();

            if (!IsPostBack)
            {
                // перевод веб-страницы
                Translator.TranslatePage(Page, "Scada.Web.Plugins.Config.WFrmWebConfig");

                // загрузка и отображение настроек
                LoadSettings();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveSettings();
        }
    }
}
