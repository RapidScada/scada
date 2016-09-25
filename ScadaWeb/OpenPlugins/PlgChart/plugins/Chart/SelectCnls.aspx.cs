using System;

namespace Scada.Web.Plugins.Chart
{
    public partial class WFrmSelectCnls : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            // завершить выбор каналов
            string cnlNums = txtCnls.Text;
            string viewIDs = txtViewIDs.Text;
            ClientScript.RegisterStartupScript(GetType(), "CloseModalScript",
                string.Format("closeModal('{0}', '{1}');", cnlNums, viewIDs), true);
        }
    }
}