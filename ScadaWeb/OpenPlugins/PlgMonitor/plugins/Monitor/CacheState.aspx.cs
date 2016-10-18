using Scada.Client;
using Scada.Data.Tables;
using System;

namespace Scada.Web.Plugins.Monitor
{
    public partial class WFrmCacheState : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            const string InfoFormat = "StorePeriod: {0}, Capacity: {1}, LastRemoveDT: {2}";
            AppData appData = AppData.GetAppData();

            Cache<DateTime, SrezTableLight> hourTableCache = appData.DataAccess.DataCache.HourTableCache;
            lblHourTableCacheInfo.Text = string.Format(InfoFormat, 
                hourTableCache.StorePeriod, hourTableCache.Capacity, hourTableCache.LastRemoveDT);
            repHourTableCache.DataSource = hourTableCache.GetAllItemsForWatching();
            repHourTableCache.DataBind();

            Cache<int, BaseView> viewCache = appData.ViewCache.Cache;
            lblViewCacheInfo.Text = string.Format(InfoFormat,
                viewCache.StorePeriod, viewCache.Capacity, viewCache.LastRemoveDT);
            repViewCache.DataSource = viewCache.GetAllItemsForWatching();
            repViewCache.DataBind();
        }
    }
}