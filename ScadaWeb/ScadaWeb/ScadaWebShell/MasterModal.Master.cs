/*
 * Copyright 2021 Mikhail Shiryaev
 * 
 * Product  : Rapid SCADA
 * Module   : SCADA-Web
 * Summary  : Represents a master page of modal forms
 * 
 * Author   : SCADA-Web
 * Created  : 2021
 * Modified : 2021
 */

using System;

namespace Scada.Web
{
    /// <summary>
    /// Represents a master page of modal forms.
    /// <para>Представляет страницу-шаблон модальных форм.</para>
    /// </summary>
    public partial class MasterModal : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // login check
            if (!UserData.GetUserData().LoggedOn)
                throw new ScadaException(WebPhrases.NotLoggedOn);
        }
    }
}
