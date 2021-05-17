using System;
using System.Web;
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

        /// <summary>
        /// Sets the selected value of the drop down list if possible.
        /// </summary>
        public static void SetSelectedValue(this DropDownList dropDownList, object value)
        {
            try 
            { 
                dropDownList.SelectedValue = value == null ? "" : value.ToString();
            } 
            catch
            { 
            }
        }

        /// <summary>
        /// Updates the modal dialog height.
        /// </summary>
        public static void UpdateModalHeight(this Page page)
        {
            page.ClientScript.RegisterStartupScript(page.GetType(), "UpdateModalHeight", "updateModalHeight();", true);
        }

        /// <summary>
        /// Sets the modal dialog title.
        /// </summary>
        public static void SetModalTitle(this Page page, string title)
        {
            page.ClientScript.RegisterStartupScript(page.GetType(), "SetModalTitle",
                $"setModalTitle('{ HttpUtility.JavaScriptStringEncode(title) }');", true);
        }

        /// <summary>
        /// Closes the modal dialog.
        /// </summary>
        public static void CloseModal(this Page page, bool dialogResult = true, string extraParams = null)
        {
            string resultStr = dialogResult ? "true" : "false";
            string script = string.IsNullOrEmpty(extraParams) ?
                string.Format("closeModal({0});", resultStr) :
                string.Format("closeModal({0}, '{1}');", resultStr, extraParams);
            page.ClientScript.RegisterStartupScript(page.GetType(), "CloseModal", script, true);
        }
    }
}
