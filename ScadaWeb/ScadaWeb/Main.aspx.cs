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
 * Summary  : Main page of the application web form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2012
 * Modified : 2014
 */

using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Scada.Web
{
    /// <summary>
    /// Main page of the application web form
    /// <para>Веб-форма главной страницы приложения</para>
    /// </summary>
    public partial class WFrmMain : System.Web.UI.Page
    {
        protected string viewSetIndStr;      // строковая запись индекса набора представлений
        protected string viewTypeArrStr;     // строковая запись массива типов представлений
        protected string viewFileNameArrStr; // строковая запись массива имён файлов представлений

        protected void Page_Load(object sender, EventArgs e)
        {
            // отключение кэширования страницы
            ScadaUtils.DisablePageCache(Response);

            // получение данных пользователя
            UserData userData = UserData.GetUserData();

            // проверка входа в систему
            userData.CheckLoggedOn(Context);

            // перевод веб-страницы
            Localization.TranslatePage(this, "Scada.Web.WFrmMain");

            // определение индекса выбранного при входе набора представлений
            int viewSetInd;
            int.TryParse(Request.Params["viewSet"], out viewSetInd);
            viewSetIndStr = viewSetInd.ToString();

            // получение выбранного набора представлений, если на него есть права
            ViewSettings.ViewSet viewSet = null;
            int viewCnt = 0;

            if (userData.GetViewSetRight(viewSetInd).ViewRight)
            {
                List<ViewSettings.ViewSet> viewSetList = userData.ViewSettings.ViewSetList;

                if (0 <= viewSetInd && viewSetInd < viewSetList.Count)
                {
                    viewSet = viewSetList[viewSetInd];
                    viewCnt = viewSet.Count;

                    // вывод заголовка страницы
                    Title = "SCADA - " + viewSet.Name;

                    // заполнение списка представлений
                    bool first = true;
                    for (int i = 0; i < viewCnt; i++)
                    {
                        if (userData.GetViewRight(viewSetInd, i).ViewRight)
                        {
                            ListItem item = new ListItem(viewSet[i].Title, i.ToString());
                            item.Selected = first;
                            ddlView.Items.Add(item);
                            first = false;
                        }
                    }
                }
            }

            // заполнение списков дней, месяцев и годов, выбор текущей даты
            DateTime today = DateTime.Today;
            int curDay = today.Day;

            for (int day = 1; day <= 31; day++)
            {
                string dayStr = day.ToString();
                ListItem item = new ListItem(day < 10 ? "0" + dayStr : dayStr, dayStr);
                item.Selected = day == curDay;
                ddlDay.Items.Add(item);
            }

            int curMonth = today.Month;
            string[] monthNames = Localization.Culture.DateTimeFormat.MonthNames;

            for (int month = 1; month <= 12; month++)
            {
                ListItem item = new ListItem(monthNames[month - 1], month.ToString());
                item.Selected = month == curMonth;
                ddlMonth.Items.Add(item);
            }

            for (int i = 0, year = today.Year; i < 10; i++, year--)
            {
                string yearStr = year.ToString();
                ListItem item = new ListItem(yearStr, yearStr);
                item.Selected = i == 0;
                ddlYear.Items.Add(item);
            }

            // добавление скрипта перемещения выпадающего списка дней после списка месяцев, если необходимо
            string pattern = Localization.Culture.DateTimeFormat.ShortDatePattern.ToLower();
            if (pattern.IndexOf('m') < pattern.IndexOf('d'))
                ClientScript.RegisterStartupScript(this.GetType(), "Startup", "PlaceDayAfterMonth();", true);

            // привязка событий
            ddlView.Attributes["onchange"] = "ShowData()";
            ddlDay.Attributes["onchange"] = "ShowData()";
            ddlMonth.Attributes["onchange"] = "ShowData()";
            ddlYear.Attributes["onchange"] = "ShowData()";
            rbStage1.Attributes["onclick"] = "ShowView()";
            rbStage2.Attributes["onclick"] = "ShowView()";
            rbStageFull.Attributes["onclick"] = "ShowView()";

            // настройка отображения событий и фрейма для проверка входа в систему
            pnlEvents.Visible = AppData.WebSettings.EventCnt > 0;
            frameLoginChecker.Visible = !pnlEvents.Visible;

            // формирование массивов, описывающих представления
            if (viewCnt > 0)
            {
                string[] viewTypeArr = new string[viewCnt];
                string[] viewFileNameArr = new string[viewCnt];

                for (int i = 0; i < viewCnt; i++)
                {
                    ViewSettings.ViewInfo viewInfo = viewSet[i];
                    viewTypeArr[i] = "\"" + viewInfo.Type + "\"";
                    viewFileNameArr[i] = "\"" + viewInfo.FileName + "\"";
                }

                viewTypeArrStr = "[" + string.Join(", ", viewTypeArr) + "]";
                viewFileNameArrStr = "[" + string.Join(", ", viewFileNameArr) + "]";
            }
            else
            {
                viewTypeArrStr = "\"\"";
                viewFileNameArrStr = "\"\"";
            }
        }
    }
}