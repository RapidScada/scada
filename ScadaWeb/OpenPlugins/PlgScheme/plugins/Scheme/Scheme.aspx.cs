#define DEBUG_MODE

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Scada.Web.Plugins.Scheme
{
    public partial class WFrmScheme : System.Web.UI.Page
    {
        protected int viewID;

        protected void Page_Load(object sender, EventArgs e)
        {
#if DEBUG_MODE
            AppData.Init(Server.MapPath("~"));
#endif

            viewID = 3; // ServerRoom.sch
        }
    }
}