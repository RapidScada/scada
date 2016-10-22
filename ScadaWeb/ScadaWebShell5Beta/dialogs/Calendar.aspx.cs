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
 * Summary  : Calendar popup web form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

using System;
using System.Globalization;
using System.Threading;
using System.Web.UI.WebControls;

namespace Scada.Web.Dialogs
{
    /// <summary>
    /// Calendar popup web form
    /// <para>Всплывающая веб-форма календаря</para>
    /// </summary>
    public partial class WFrmCalendar : System.Web.UI.Page
    {
        protected override void InitializeCulture()
        {
            // установить культуру для отображения календаря
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = Localization.Culture;
            base.InitializeCulture();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // установка выбранной даты, если она задана в параметрах запроса
            DateTime date;
            if (ScadaUtils.TryParseDateTime(Request.QueryString["date"], out date))
                Calendar.VisibleDate = Calendar.SelectedDate = date;

            // убрать всплывающую подсказку по умолчанию
            Calendar.Attributes["title"] = ""; 
        }

        protected void Calendar_DayRender(object sender, DayRenderEventArgs e)
        {
            DateTime date = e.Day.Date;
            e.Cell.Text = string.Format("<a href='javascript:selectDate({0}, {1}, {2}, \"{3}\");'>{2}</a>", 
                date.Year, date.Month, date.Day, date.ToLocalizedDateString());
        }
    }
}