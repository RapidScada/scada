/*
 * Copyright 2020 Mikhail Shiryaev
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
 * Module   : PlgChartCommon
 * Summary  : The class contains utility methods for charts
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2020
 */

using Scada.Client;
using Scada.Web.Shell;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Scada.Web.Plugins.Chart
{
    /// <summary>
    /// The class contains utility methods for charts
    /// <para>Класс, содержащий вспомогательные методы для графиков</para>
    /// </summary>
    public static class ChartUtils
    {
        /// <summary>
        /// Версия графиков
        /// </summary>
        public const string ChartVersion = "5.1.0.1";
        /// <summary>
        /// Количество графиков, выше которого могут быть проблемы с производительностью
        /// </summary>
        public const int NormalChartCnt = 10;


        /// <summary>
        /// Получить можество номеров канала из списка пар канал/представление
        /// </summary>
        public static HashSet<int> GetCnlSet(List<CnlViewPair> cnlViewPairs)
        {
            HashSet<int> cnlSet = new HashSet<int>();
            foreach (CnlViewPair pair in cnlViewPairs)
                cnlSet.Add(pair.CnlNum);
            return cnlSet;
        }

        /// <summary>
        /// Получить выбранные каналы и соответствующие им представления из списка
        /// </summary>
        public static void GetSelection(this List<CnlViewPair> cnlViewPairs, out string cnlNums, out string viewIDs)
        {
            StringBuilder sbCnlNums = new StringBuilder();
            StringBuilder sbViewIDs = new StringBuilder();

            for (int i = 0, lastInd = cnlViewPairs.Count - 1; i <= lastInd; i++)
            {
                CnlViewPair pair = cnlViewPairs[i];
                sbCnlNums.Append(pair.CnlNum);
                sbViewIDs.Append(pair.ViewID);

                if (i < lastInd)
                {
                    sbCnlNums.Append(",");
                    sbViewIDs.Append(",");
                }
            }

            cnlNums = sbCnlNums.ToString();
            viewIDs = sbViewIDs.ToString();
        }

        /// <summary>
        /// Получить список пар канал/представление по ид. представления
        /// </summary>
        public static List<CnlViewPair> GetCnlViewPairsByView(
            int viewID, DataAccess dataAccess, UserViews userViews)
        {
            BaseView view = userViews.GetView(viewID, false, true);

            if (view == null)
            {
                return null;
            }
            else
            {
                List<CnlViewPair> cnlsByView = new List<CnlViewPair>();
                foreach (int cnlNum in view.CnlList)
                {
                    CnlViewPair pair = new CnlViewPair(cnlNum, 0);
                    pair.FillInfo(dataAccess.GetCnlProps(cnlNum), null);
                    cnlsByView.Add(pair);
                }
                return cnlsByView;
            }
        }

        /// <summary>
        /// Заполнить выпадающий список представлений
        /// </summary>
        public static void FillViewList(DropDownList ddlView, int preferableViewID, UserViews userViews)
        {
            int selInd1 = -1; // индекс выбранного элемента, соответствующего непустому представлению
            int selInd2 = -1; // индекс выбранного элемента, соответствующего предпочтительному представлению
            List<ViewNode> viewNodes = userViews.GetLinearViewNodes();
            int viewNodesCnt = viewNodes.Count;

            // заполнение списка представлений и определение индексов выбранного элемента
            ddlView.Items.Clear();
            for (int i = 0; i < viewNodesCnt; i++)
            {
                ViewNode viewNode = viewNodes[i];

                if (selInd1 <= 0 && !viewNode.IsEmpty)
                    selInd1 = i;
                if (selInd2 <= 0 && preferableViewID > 0 && viewNode.ViewID == preferableViewID)
                    selInd2 = i;

                string text = new string('-', viewNode.Level) + " " + viewNode.Text;
                ddlView.Items.Add(new ListItem(text, viewNode.ViewID.ToString()));
            }

            // установка выбранного элемента
            if (selInd2 >= 0)
                ddlView.SelectedIndex = selInd2;
            else if (selInd1 >= 0)
                ddlView.SelectedIndex = selInd1;
        }

        /// <summary>
        /// Добавить на страницу скрипт обновления высоты диалогового окна
        /// </summary>
        public static void AddUpdateModalHeightScript(Page page)
        {
            if (!page.ClientScript.IsStartupScriptRegistered(page.GetType(), "UpdateModalHeightScript"))
                page.ClientScript.RegisterStartupScript(
                    page.GetType(), "UpdateModalHeightScript", "updateModalHeight();", true);
        }
    }
}
