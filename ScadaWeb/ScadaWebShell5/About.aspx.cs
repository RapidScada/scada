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
 * Summary  : About web form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

using Scada.UI;
using System;

namespace Scada.Web
{
    /// <summary>
    /// About web form
    /// <para>Веб-форма о программе</para>
    /// </summary>
    public partial class WFrmAbout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // перевод веб-страницы
            Translator.TranslatePage(this, "Scada.Web.WFrmAbout");
            // вывод версии
            lblVersion.Text = WebUtils.AppVersion;
        }
    }
}