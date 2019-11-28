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
 * Module   : SCADA-Web
 * Summary  : Web form for displaying a view in a popup
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2019
 */

using Scada.Web.Shell;
using System;

namespace Scada.Web
{
    /// <summary>
    /// Web form for displaying a view in a frame.
    /// <para>Веб-форма для отображения представления во фрейме.</para>
    /// </summary>
    public partial class WFrmViewFrame : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            UserData userData = UserData.GetUserData();
            userData.CheckLoggedOn(false);

            // получение информации о представлении
            int viewId = Request.QueryString.GetParamAsInt("viewID");
            ViewNode viewNode = userData.UserViews.GetViewNode(viewId);

            // переход на соответствующую веб-страницу
            Response.Redirect(viewNode == null || string.IsNullOrEmpty(viewNode.ViewUrl) ? 
                UrlTemplates.NoView : viewNode.ViewUrl);
        }
    }
}