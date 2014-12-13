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
 * Module   : SCADA-Scheme Web
 * Summary  : Web form containing the Silverlight application
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2012
 * Modified : 2014
 */

using System;
using Scada.Web;

namespace Scada.Scheme.Web.scheme
{
    /// <summary>
    /// Web form containing the Silverlight application
    /// <para>Веб-форма, содержащая silverlight-приложение</para>
    /// </summary>
    public partial class ScadaScheme : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // проверка входа в систему
            UserData userData = UserData.GetUserData();
            if (!userData.LoggedOn)
                throw new Exception(CommonPhrases.NoRights);

            // инициализировать данные приложения SCADA-Схема
            SchemeApp.InitSchemeApp(SchemeApp.WorkModes.Monitor);

            // вход в систему для отладки приложения
            /*if (!userData.LoggedOn)
            {
                string errMsg;
                userData.Login("admin", "12345", out errMsg);
            }*/
        }
    }
}