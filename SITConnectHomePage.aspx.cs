using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _204703Q_AS_CodingAssignment_Ver2
{
    public partial class SITConnectHomePage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["LoggedIn"] != null && Session["AuthToken"] != null && Request.Cookies["AuthToken"] != null)
            {
                if (!Session["AuthToken"].ToString().Equals(Request.Cookies["AuthToken"].Value))
                {
                    Response.Redirect("LoginForm.aspx", false);
                }
                else
                {
                    /* lblMessage.Text = "congratulations! you are logged in";
                    lblMessage.ForeColor = System.Drawing.Color.Green;
                    btnLogout.Visible = true;*/
                }
            }
            else
            {
                Response.Redirect("SITConnectLogin.aspx", false);
            }
        }

        protected void logOut_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();

            Response.Redirect("SITConnectLogin.aspx", false);

            if (Request.Cookies["ASP.NET_SessionId"] != null)
            {
                Response.Cookies["ASP.NET_SessionId"].Value = string.Empty;
                Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddMonths(-20);
            }
            if (Request.Cookies["AuthToken"] != null)
            {
                Response.Cookies["AuthToken"].Value = string.Empty;
                Response.Cookies["AuthToken"].Expires = DateTime.Now.AddMonths(-20);
            }
            if (Request.Cookies["LoggedIn"] != null)
            {
                Response.Cookies["LoggedIn"].Value = string.Empty;
                Response.Cookies["LoggedIn"].Expires = DateTime.Now.AddMonths(-20);
            }
            /*            if (Request.Cookies["LoggedIn"] != null)
                        {
                            Response.Cookies["LoggedIn"].Value = string.Empty;
                            Response.Cookies["LoggedIn"].Expires = DateTime.Now.AddMonths(-20);
                        }*/

        }

        protected void changePassword_Click(object sender, EventArgs e)
        {
            Response.Redirect("ChangePassword.aspx", false);
        }



    }
}