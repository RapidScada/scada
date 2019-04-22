/*
 * Copyright 2019 Mikhail Shiryaev
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
 * Summary  : Table view web form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2019
 */

using Scada.Data.Models;
using Scada.Table;
using Scada.UI;
using Scada.Web.Shell;
using System;
using System.Text;
using System.Web;

namespace Scada.Web.Plugins.Table
{
    /// <summary>
    /// Table view web form
    /// <para>Веб-форма табличного представления</para>
    /// </summary>
    public partial class WFrmTable : System.Web.UI.Page
    {
        /// <summary>
        /// Относительный путь к иконкам величин
        /// </summary>
        private const string QuantityIconsPath = "images/quantityicons/";
        /// <summary>
        /// Иконка величины по умолчанию
        /// </summary>
        private const string DefQuantityIcon = "quantity.png";

        // Переменные для вывода на веб-страницу
        protected bool debugMode = false; // режим отладки
        protected int viewID;             // ид. представления
        protected string viewTitle;       // заголовок представления
        protected int dataRefrRate;       // частота обновления текущих данных
        protected int arcRefrRate;        // частота обновления архивных данных
        protected string phrases;         // локализованные фразы
        protected string today;           // текущая дата
        protected string selTimeFromHtml; // HTML-код выбора начального времени
        protected string selTimeToHtml;   // HTML-код выбора конечного времени
        protected string tableViewHtml;   // HTML-код табличного представления


        /// <summary>
        ///Получить час в соответствии с текущей культурой
        /// </summary>
        internal static string GetLocalizedHour(int hour)
        {
            return 
                (hour >= 0 ? "" : TablePhrases.PrevDayItem) + 
                DateTime.MinValue.AddHours(hour >= 0 ? hour : hour + 24).ToString("t", Localization.Culture);
        }

        /// <summary>
        /// Добавить группу элементов в выпадающий список выбора времени
        /// </summary>
        private void AppendOptGroup(StringBuilder sbHtml, string label, int startHour, int endHour, int selectedHour)
        {
            sbHtml.Append("<optgroup label='").Append(label).AppendLine("'>");
            for (int hour = startHour; hour <= endHour; hour++)
            {
                sbHtml.Append("<option value='").Append(hour).Append("'")
                    .Append(hour == selectedHour ? " selected" : "").Append(">")
                    .Append(GetLocalizedHour(hour)).Append("</option>");
            }
            sbHtml.AppendLine("</optgroup>");
        }

        /// <summary>
        /// Генерировать HTML-код выпадающего списка выбора времени
        /// </summary>
        private string GenerateTimeSelectHtml(string elemID, bool addPrevDay, int selectedHour)
        {
            StringBuilder sbHtml = new StringBuilder();
            sbHtml.Append("<select id='").Append(elemID).AppendLine("'>");

            if (addPrevDay)
                AppendOptGroup(sbHtml, TablePhrases.PreviousDay, -24, -1, selectedHour);

            AppendOptGroup(sbHtml, TablePhrases.SelectedDay, 0, 23, selectedHour);
            sbHtml.AppendLine("</select>");
            return sbHtml.ToString();
        }

        /// <summary>
        /// Добавить ячейку в табличное представление
        /// </summary>
        private void AppendCell(StringBuilder sbHtml, string cssClass, string innerHtml, string hour = null)
        {
            sbHtml.Append("<td");

            if (!string.IsNullOrEmpty(cssClass))
                sbHtml.Append(" class='").Append(cssClass).Append("'");

            if (hour != null)
                sbHtml.Append(" data-hour='").Append(hour).Append("'");

            sbHtml.Append(">").Append(innerHtml).Append("</td>");
        }

        /// <summary>
        /// Добавить текст подсказки в табличное представление
        /// </summary>
        private void AppendHint(StringBuilder sbHtml, bool addBreak, string label, int num, string name)
        {
            if (addBreak)
                sbHtml.Append("<br />");
            sbHtml.Append(label);
            if (num > 0)
                sbHtml.Append("[").Append(num).Append("] ");
            sbHtml.Append(HttpUtility.HtmlEncode(name));
        }

        /// <summary>
        /// Генерировать HTML-код табличного представления
        /// </summary>
        private string GenerateTableViewHtml(TableView tableView, bool cmdEnabled, int timeFrom, int timeTo)
        {
            const int FirstHour = -24;
            const int LastHour = 23;

            string[] hourStrings = new string[LastHour - FirstHour + 1];
            for (int hour = FirstHour, hourInd = 0; hour <= LastHour; hour++, hourInd++)
                hourStrings[hourInd] = hour.ToString();

            StringBuilder sbHtml = new StringBuilder();
            sbHtml.AppendLine("<table>");

            // заголовок таблицы
            sbHtml.AppendLine("<tr class='hdr'>");
            AppendCell(sbHtml, "cap", "<span>" + TablePhrases.ItemCol + "</span>");
            AppendCell(sbHtml, "cur", "<span>" + TablePhrases.CurCol + "</span>");
            for (int hour = FirstHour, hourInd = 0; hour <= LastHour; hour++, hourInd++)
            {
                AppendCell(sbHtml, timeFrom <= hour && hour <= timeTo ? "hour" : "hour hidden", 
                    "<span>" + GetLocalizedHour(hour) + "</span>", hourStrings[hourInd]);
            }
            sbHtml.AppendLine().AppendLine("</tr>");

            // строки таблицы
            bool altRow = false;
            foreach(TableView.Item item in tableView.VisibleItems)
            {
                InCnlProps cnlProps = item.CnlProps;
                int cnlNum = item.CnlNum;
                int ctrlCnlNum = item.CtrlCnlNum;

                // тег начала строки
                sbHtml.Append(altRow ? "<tr class='item alt'" : "<tr class='item'");
                if (cnlNum > 0)
                    sbHtml.Append(" data-cnl='").Append(cnlNum).Append("'");
                if (ctrlCnlNum > 0)
                    sbHtml.Append(" data-ctrl='").Append(ctrlCnlNum).Append("'");
                sbHtml.AppendLine(">");

                // ячейка наименования
                string caption = string.IsNullOrEmpty(item.Caption) ? "&nbsp;" : HttpUtility.HtmlEncode(item.Caption);
                if (cnlNum > 0 || ctrlCnlNum > 0)
                {
                    StringBuilder sbCapHtml = new StringBuilder();

                    // иконка и обозначение
                    string iconFileName = cnlProps == null || cnlProps.IconFileName == "" ?
                        DefQuantityIcon : cnlProps.IconFileName;
                    sbCapHtml.Append("<img src='" + QuantityIconsPath + iconFileName + "' class='icon' alt='' />")
                        .Append("<a href='' class='lbl'>").Append(caption).Append("</a>");

                    // команда
                    if (ctrlCnlNum > 0 && cmdEnabled)
                        sbCapHtml.Append("<span class='cmd' title='Send Command'></span>");

                    // всплывающая подсказка
                    sbCapHtml.Append("<span class='hint'>");
                    if (cnlNum > 0)
                        AppendHint(sbCapHtml, false, TablePhrases.InCnlHint, cnlNum, 
                            cnlProps == null ? "" : cnlProps.CnlName);
                    if (ctrlCnlNum > 0)
                        AppendHint(sbCapHtml, cnlNum > 0, TablePhrases.CtrlCnlHint, ctrlCnlNum,
                          item.CtrlCnlProps == null ? "" : item.CtrlCnlProps.CtrlCnlName);
                    if (cnlProps != null)
                    {
                        if (cnlProps.ObjNum > 0)
                            AppendHint(sbCapHtml, true, TablePhrases.ObjectHint, cnlProps.ObjNum, cnlProps.ObjName);
                        if (cnlProps.KPNum > 0)
                            AppendHint(sbCapHtml, true, TablePhrases.DeviceHint, cnlProps.KPNum, cnlProps.KPName);
                        if (cnlProps.ParamID > 0)
                            AppendHint(sbCapHtml, true, TablePhrases.QuantityHint, 0, cnlProps.ParamName);
                        if (cnlProps.UnitID > 0 && cnlProps.ShowNumber)
                            AppendHint(sbCapHtml, true, TablePhrases.UnitHint, 0, cnlProps.UnitSign);
                    }
                    sbCapHtml.Append("</span>");

                    AppendCell(sbHtml, "cap", sbCapHtml.ToString());
                }
                else
                {
                    AppendCell(sbHtml, "cap", caption);
                }

                // ячейки текущих и часовых данных
                AppendCell(sbHtml, "cur", "");
                for (int hour = FirstHour, hourInd = 0; hour <= LastHour; hour++, hourInd++)
                    AppendCell(sbHtml, timeFrom <= hour && hour <= timeTo ? "hour" : "hour hidden", 
                        "", hourStrings[hourInd]);

                // тег окончания строки
                sbHtml.AppendLine().AppendLine("</tr>");
                altRow = !altRow;
            }

            sbHtml.AppendLine("</table>");
            return sbHtml.ToString();
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            AppData appData = AppData.GetAppData();
            UserData userData = UserData.GetUserData();

#if DEBUG
            debugMode = true;
            userData.LoginForDebug();
#endif

            // перевод веб-страницы
            Translator.TranslatePage(Page, "Scada.Web.Plugins.Table.WFrmTable");

            // получение ид. представления из параметров запроса
            viewID = Request.QueryString.GetParamAsInt("viewID");

            // проверка прав на просмотр представления
            EntityRights rights = userData.LoggedOn ? 
                userData.UserRights.GetUiObjRights(viewID) : EntityRights.NoRights;
            if (!rights.ViewRight)
                Response.Redirect(UrlTemplates.NoView);

            // загрузка представления в кэш для последующего получения данных через API
            TableView tableView = appData.ViewCache.GetView<TableView>(viewID);
            if (tableView == null)
                Response.Redirect(UrlTemplates.NoView);
            else
                appData.AssignStamp(tableView);

            // получение периода времени из cookies
            HttpCookie cookie = Request.Cookies["Table.TimeFrom"];
            int timeFrom;
            if (cookie == null || !int.TryParse(cookie.Value, out timeFrom))
                timeFrom = 0;

            cookie = Request.Cookies["Table.TimeTo"];
            int timeTo;
            if (cookie == null || !int.TryParse(cookie.Value, out timeTo))
                timeTo = 23;

            // подготовка данных для вывода на веб-страницу
            viewTitle = HttpUtility.JavaScriptStringEncode(tableView.Title);
            dataRefrRate = userData.WebSettings.DataRefrRate;
            arcRefrRate = userData.WebSettings.ArcRefrRate;
            phrases = WebUtils.DictionaryToJs("Scada.Web.Plugins.Table.WFrmTable.Js");

            DateTime nowDT = DateTime.Now;
            today = string.Format("new Date({0}, {1}, {2})", nowDT.Year, nowDT.Month - 1, nowDT.Day);

            selTimeFromHtml = GenerateTimeSelectHtml("selTimeFrom", true, timeFrom);
            selTimeToHtml = GenerateTimeSelectHtml("selTimeTo", false, timeTo);
            bool cmdEnabled = userData.WebSettings.CmdEnabled && rights.ControlRight;
            tableViewHtml = GenerateTableViewHtml(tableView, cmdEnabled, timeFrom, timeTo);
        }
    }
}