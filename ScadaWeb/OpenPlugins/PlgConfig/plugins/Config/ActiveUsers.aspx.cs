using System;

namespace Scada.Web.Plugins.Config
{
    public partial class WFrmActiveUsers : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AppData appData = AppData.GetAppData();
            UserData[] activeUsers = appData.UserMonitor.GetActiveUsers();

            repActiveUsers.DataSource = activeUsers;
            repActiveUsers.DataBind();
        }
    }
}