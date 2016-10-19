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
 * Module   : PlgConfig
 * Summary  : Web application configuration web form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

using Scada.UI;
using Scada.Web.Shell;
using System;
using System.Web.UI.WebControls;

namespace Scada.Web.Plugins.Config
{
    /// <summary>
    /// Web application configuration web form
    /// <para>Веб-форма конфигурации веб-приложения</para>
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
            string errMsg;

            if (!webSettings.LoadFromFile(appData.AppDirs.ConfigDir + WebSettings.DefFileName, out errMsg))
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

        }

        /// <summary>
        /// Отобразить настройки
        /// </summary>
        private void SettingsToControls(WebSettings webSettings)
        {
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

            ddlChartScript.Items.Add("");
            ddlCmdScript.Items.Add("");
            ddlEventAckScript.Items.Add("");

            string curChartScriptPath = webSettings.ScriptPaths.ChartScriptPath;
            string curCmdScriptPath = webSettings.ScriptPaths.CmdScriptPath;
            string curEventAckScriptPath = webSettings.ScriptPaths.EventAckScriptPath;
            int chartScriptInd = 0;
            int cmdScriptInd = 0;
            int eventAckScriptInd = 0;

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
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            appData = AppData.GetAppData();
            userData = UserData.GetUserData();

            // проверка входа в систему и прав
            userData.CheckLoggedOn(true);

            if (!userData.UserRights.ConfigRight)
                throw new ScadaException(CommonPhrases.NoRights);

            // скрытие сообщения об ошибке
            pnlErrMsg.HideAlert();

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