using System;

namespace Scada.Web
{
    public partial class WFrmLogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            UserData userData = UserData.GetUserData();
            string errMsg;
            if (userData.Login(txtUsername.Text, txtPassword.Text, out errMsg))
                Response.Redirect("~/Views.aspx");
            else
                throw new Exception(errMsg);
        }
    }
}