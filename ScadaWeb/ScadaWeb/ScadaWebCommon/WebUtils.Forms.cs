using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Scada.Web
{
    partial class WebUtils
    {
        /// <summary>
        /// Скрыть сообщение на панели
        /// </summary>
        public static void HideAlert(this Panel pnlMessage)
        {
            pnlMessage.Visible = false;

            foreach (Control control in pnlMessage.Controls)
            {
                if (control is Label)
                    ((Label)control).Visible = false;
            }
        }

        /// <summary>
        /// Отобразить сообщение на панели
        /// </summary>
        public static void ShowAlert(this Panel pnlMessage, string text)
        {
            foreach (Control control in pnlMessage.Controls)
            {
                if (control is Label)
                {
                    pnlMessage.Visible = true;
                    Label lblMessage = (Label)control;
                    lblMessage.Text = HtmlEncodeWithBreak(text);
                    lblMessage.Visible = true;
                    break;
                }
            }
        }

        /// <summary>
        /// Отобразить текст исключения на панели
        /// </summary>
        public static void ShowAlert(this Panel pnlMessage, Exception ex)
        {
            pnlMessage.ShowAlert((ex.InnerException ?? ex).Message);
        }

        /// <summary>
        /// Сделать видимым сообщение на панели
        /// </summary>
        public static void ShowAlert(this Panel pnlMessage, Label lblMessage)
        {
            if (lblMessage != null)
            {
                pnlMessage.Visible = true;
                lblMessage.Visible = true;
            }
        }
    }
}
