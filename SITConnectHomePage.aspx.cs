using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Security.Cryptography;
using System.Text;
using System.Data;
using System.Data.SqlClient;

using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using System.Web.Services;

//send gmail
using System.Net.Mail;
using System.Configuration;

namespace _204703Q_AS_CodingAssignment_Ver2
{
    public partial class SITConnectHomePage : System.Web.UI.Page
    {
        string AssignDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ASAssignmentDBConnection"].ConnectionString;
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
                    string Email = Request.Cookies["LoggedIn"].Value;
                    //string userid = HttpUtility.HtmlEncode(loginEmail.Text.ToString().Trim());
                    DateTime currentTime = DateTime.Now; // Current Time
                    DateTime getMPCountdown = Convert.ToDateTime(getMustChangePasswordTimer(Email));

                    //define message to change password before 3 minutes

                    if (DateTime.Compare(currentTime, getMPCountdown).Equals(-1)) //current time earlier than 5 min
                    {
                        MustChangePassword.Text = "Please change your password in " + getMustChangePasswordTimer(Email);
                    }
                    else
                    {
                        //MustChangePassword.Text = "Please change your password now to secure your account.";
                        Response.Redirect("ChangePassword.aspx", false);
                    }
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


        protected string getMustChangePasswordTimer(string userid)
        {

            string s = null;

            SqlConnection connection = new SqlConnection(AssignDBConnectionString);
            string sql = "select MustChangePasswordCountdown FROM USERINFO WHERE Email=@EMAIL";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@EMAIL", userid);

            try
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["MustChangePasswordCountdown"] != null)
                        {
                            if (reader["MustChangePasswordCountdown"] != DBNull.Value)
                            {
                                s = reader["MustChangePasswordCountdown"].ToString();
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally { connection.Close(); }
            return s;

        }

    }
}