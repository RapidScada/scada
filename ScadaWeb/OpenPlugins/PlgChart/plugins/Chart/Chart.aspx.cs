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
 * Module   : PlgChart
 * Summary  : Chart web form
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
using System.Globalization;
using System.Text;
using System.Web;

namespace Scada.Web.Plugins.Chart
{
    /// <summary>
    /// Chart web form
    /// <para>Веб-форма графика</para>
    /// </summary>
    public partial class WFrmChart : System.Web.UI.Page
    {
        // Переменные для вывода на веб-страницу
        protected int chartGap;        // расстояние разрыва графика
        protected DateTime startDate;  // дата отображаемых данных
        protected int cnlNum;          // номер канала отображаемого графика
        protected string cnlName;      // имя канала
        protected string quantityName; // имя величины с указанием размерности
        protected string timePoints;   // массив меток времени тренда
        protected string trendPoints;  // массив значений тренда


        /// <summary>
        /// Заполнить массивы меток времени и значений тренда
        /// </summary>
        private void FillPoints(Trend trend, InCnlProps cnlProps, DataAccess dataAccess)
        {
            StringBuilder sbTimePoints = new StringBuilder("[");
            StringBuilder sbTrendPoints = new StringBuilder("[");
            DataFormatter dataFormatter = new DataFormatter();

            foreach (Trend.Point point in trend.Points)
            {
                // метка времени
                double time = point.DateTime.TimeOfDay.TotalDays;
                sbTimePoints.Append(time.ToString(CultureInfo.InvariantCulture)).Append(", ");

                // значение
                double val = point.Val;
                int stat = point.Stat;
                double chartVal = point.Stat > 0 ? val : double.NaN;
                string text;
                string textWithUnit;
                dataFormatter.FormatCnlVal(val, stat, cnlProps, out text, out textWithUnit);
                CnlStatProps cnlStatProps = dataAccess.GetCnlStatProps(stat);
                string color = dataFormatter.GetCnlValColor(val, stat, cnlProps, cnlStatProps);

                sbTrendPoints.Append(TrendPointToJs(chartVal, text, textWithUnit, color)).Append(", ");
            }

            sbTimePoints.Append("]");
            sbTrendPoints.Append("]");
            timePoints = sbTimePoints.ToString();
            trendPoints = sbTrendPoints.ToString();
        }

        /// <summary>
        /// Преобразовать точку тренда в запись JavaScript
        /// </summary>
        private string TrendPointToJs(double val, string text, string textWithUnit, string color)
        {
            // для text и textWithUnit было бы корректно использовать метод HttpUtility.JavaScriptStringEncode(),
            // но он опускается для повышения скорости
            return (new StringBuilder("[")
                .Append(double.IsNaN(val) ? "NaN" : val.ToString(CultureInfo.InvariantCulture))
                .Append(", \"")
                .Append(text) 
                .Append("\", \"")
                .Append(textWithUnit)
                .Append("\", \"")
                .Append(color)
                .Append("\"]")).ToString();
        }

        /// <summary>
        /// Получить имя величины с указанием размерности
        /// </summary>
        private string GetQuantityName(string paramName, string singleUnit)
        {
            return !string.IsNullOrEmpty(paramName) && !string.IsNullOrEmpty(singleUnit) ?
                paramName + ", " + singleUnit :
                paramName + singleUnit;
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            AppData appData = AppData.GetAppData();
            UserData userData = UserData.GetUserData();

#if DEBUG
            userData.LoginForDebug();
#endif

            // перевод веб-страницы
            Translator.TranslatePage(Page, "Scada.Web.Plugins.Chart.WFrmChart");

            // получение параметров запроса
            int viewID;
            int.TryParse(Request.QueryString["viewID"], out viewID);
            startDate = WebUtils.GetDateFromQueryString(Request);
            int.TryParse(Request.QueryString["cnlNum"], out cnlNum);

            // проверка прав
            if (!userData.LoggedOn ||
                !userData.UserRights.GetViewRights(viewID).ViewRight)
                throw new ScadaException(CommonPhrases.NoRights);

#if !DEBUG
            // в режиме отладки невозможно получить тип представления, т.к. плагины не загружены
            Type viewType = userData.UserViews.GetViewType(viewID);
            BaseView view = appData.ViewCache.GetView(viewType, viewID, true);

            if (!view.ContainsCnl(cnlNum))
                throw new ScadaException(CommonPhrases.NoRights);

            // вывод заголовка
            lblTitle.Text = view.Title;
#endif

            // вывод дополнительной информации
            lblStartDate.Text = (string.IsNullOrEmpty(lblTitle.Text) ? "" : ", ") + 
                startDate.ToString("d", Localization.Culture);
            lblGenDT.Text = DateTime.Now.ToLocalizedString();

            // получение данных, по которым строится график
            InCnlProps cnlProps = appData.DataAccess.GetCnlProps(cnlNum);
            Trend trend = appData.DataAccess.DataCache.GetMinTrend(startDate, cnlNum);

            // подготовка данных для вывода на веб-страницу
            chartGap = userData.WebSettings.ChartGap;

            if (cnlProps == null)
            {
                cnlName = "";
                quantityName = "";
            }
            else
            {
                cnlName = HttpUtility.JavaScriptStringEncode(cnlProps.CnlName);
                quantityName = HttpUtility.JavaScriptStringEncode(
                    GetQuantityName(cnlProps.ParamName, cnlProps.SingleUnit));
            }

            FillPoints(trend, cnlProps, appData.DataAccess);
        }
    }
}