using Scada.Data;
using Scada.UI;
using Scada.Web.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Scada.Web.Plugins.Table
{
    public partial class WFrmEvents : System.Web.UI.Page
    {
        // Переменные для вывода на веб-страницу
        protected bool debugMode = false; // режим отладки
        protected int viewID;             // ид. представления
        protected int dataRefrRate;       // частота обновления текущих данных
        protected int arcRefrRate;        // частота обновления архивных данных
        protected string phrases;         // локализованные фразы
        protected string today;           // текущая дата
        protected string eventsTableHtml; // HTML-код таблицы событий

        protected void Page_Load(object sender, EventArgs e)
        {
            AppData appData = AppData.GetAppData();
            UserData userData = UserData.GetUserData();

#if DEBUG
            debugMode = true;
            userData.LoginForDebug();
#endif

            // перевод веб-страницы
            Translator.TranslatePage(Page, "Scada.Web.Plugins.Table.WFrmEvents");

            // получение ид. представления из параметров запроса
            int.TryParse(Request.QueryString["viewID"], out viewID);

            // проверка прав на просмотр представления
            EntityRights rights = userData.LoggedOn ?
                userData.UserRights.GetViewRights(viewID) : EntityRights.NoRights;
            if (!rights.ViewRight)
                Response.Redirect(UrlTemplates.NoView);

            // подготовка данных для вывода на веб-страницу
            dataRefrRate = userData.WebSettings.DataRefrRate;
            arcRefrRate = userData.WebSettings.ArcRefrRate;

            Localization.Dict dict;
            Localization.Dictionaries.TryGetValue("Scada.Web.Plugins.Table.WFrmEvents.Js", out dict);
            phrases = WebUtils.DictionaryToJs(dict);

            DateTime nowDT = DateTime.Now;
            today = string.Format("new Date({0}, {1}, {2})", nowDT.Year, nowDT.Month - 1, nowDT.Day);

        }
    }
}