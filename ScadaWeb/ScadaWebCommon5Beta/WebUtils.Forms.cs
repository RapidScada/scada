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
 * Module   : ScadaWebCommon
 * Summary  : The class contains utility methods for web applications. Web form utilities
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

using System.Web.UI;
using System.Web.UI.WebControls;

namespace Scada.Web
{
    partial class WebUtils
    {
        /// <summary>
        /// Скрыть сообщение на панели
        /// </summary>
        public static void HideAlert(this Panel pnlMessage)
        {
            pnlMessage.Visible = false;

            foreach (Control control in pnlMessage.Controls)
            {
                if (control is Label)
                    ((Label)control).Visible = false;
            }
        }

        /// <summary>
        /// Отобразить сообщение на панели
        /// </summary>
        public static void ShowAlert(this Panel pnlMessage, string text)
        {
            foreach (Control control in pnlMessage.Controls)
            {
                if (control is Label)
                {
                    pnlMessage.Visible = true;
                    Label lblMessage = (Label)control;
                    lblMessage.Text = text;
                    lblMessage.Visible = true;
                    break;
                }
            }
        }

        /// <summary>
        /// Отобразить сообщение на панели
        /// </summary>
        public static void ShowAlert(this Panel pnlMessage, Label lblMessage)
        {
            if (lblMessage != null)
            {
                pnlMessage.Visible = true;
                lblMessage.Visible = true;
            }
        }
    }
}
