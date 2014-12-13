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
 * Summary  : Choosing date web form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2007
 * Modified : 2014
 */

using System;
using System.Globalization;
using System.Web.UI.WebControls;

namespace Scada.Web
{
    /// <summary>
    /// Choosing date web form
    /// <para>Веб-форма выбора даты</para>
    /// </summary>
    public partial class WFrmCalendar : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                Title = (string)ViewState["Title"];
            }
            else
            {
                // перевод веб-страницы
                Localization.TranslatePage(this, "Scada.Web.WFrmCalendar");
                ViewState["Title"] = Title;

                // выбор даты, заданной в параметрах запроса
                DateTime date;
                if (DateTime.TryParse(Request["date"], Localization.Culture, DateTimeStyles.None, out date))
                    Calendar.VisibleDate = Calendar.SelectedDate = date;
            }
        }

        protected void Calendar_SelectionChanged(object sender, EventArgs e)
        {
            string txtID = Request["txtID"] ?? "";
            ClientScript.RegisterStartupScript(this.GetType(), "Startup", "DoSelect('" + txtID + "', '" + 
                Calendar.SelectedDate.ToString("d", Localization.Culture) + "')", true);
        }
    }
}