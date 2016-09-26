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
 * Module   : PlgChartCommon
 * Summary  : Defines a input channel/view pair
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

using Scada.Data.Models;
using Scada.Web.Shell;
using System;
using System.Text;

namespace Scada.Web.Plugins.Chart
{
    /// <summary>
    /// Defines a input channel/view pair
    /// <para>Определяет пару входной канал/представление</para>
    /// </summary>
    [Serializable]
    public class CnlViewPair
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public CnlViewPair()
            : this(0, 0)
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public CnlViewPair(int cnlNum, int viewID)
        {
            CnlNum = cnlNum;
            ViewID = viewID;
            CnlName = "";
            Info = "";
        }


        /// <summary>
        /// Получить или установить номер входного канала
        /// </summary>
        public int CnlNum { get; set; }

        /// <summary>
        /// Получить или установить ид. представления
        /// </summary>
        public int ViewID { get; set; }


        /// <summary>
        /// Получить или установить наименование входного канала
        /// </summary>
        public string CnlName { get; set; }

        /// <summary>
        /// Получить или установить информацию о канале и представлении
        /// </summary>
        public string Info { get; set; }


        /// <summary>
        /// Заполнить наименование канала и информацию
        /// </summary>
        public void FillInfo(InCnlProps cnlProps, UserViews userViews)
        {
            StringBuilder sbInfo = new StringBuilder();

            if (cnlProps == null)
            {
                CnlName = "";
            }
            else
            {
                CnlName = cnlProps.CnlName;
                if (cnlProps.ObjNum > 0)
                    sbInfo.Append(ChartPhrases.ObjectHint).Append("[").Append(cnlProps.ObjNum).Append("] ")
                        .AppendLine(cnlProps.ObjName);
                if (cnlProps.KPNum > 0)
                    sbInfo.Append(ChartPhrases.DeviceHint).Append("[").Append(cnlProps.KPNum).Append("] ")
                        .AppendLine(cnlProps.KPName);
            }

            if (ViewID > 0 && userViews != null)
            {
                ViewNode viewNode = userViews.GetViewNode(ViewID);
                if (viewNode != null)
                    sbInfo.Append(ChartPhrases.ViewHint).Append(viewNode.Text);
            }

            Info = sbInfo.ToString().TrimEnd();
        }
    }
}