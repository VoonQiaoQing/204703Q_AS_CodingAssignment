using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Security.Cryptography;
using System.Text;
using System.Data;

using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Data.SqlClient;


namespace _204703Q_AS_CodingAssignment_Ver2
{
    public partial class LockAccount : System.Web.UI.Page
    {
        string AssignDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ASAssignmentDBConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            int result = Convert.ToInt32(Request.Cookies["wassup"].Value);
            string Email = Request.Cookies["Email"].Value;

            //updateLoginAttempts(Email);

            //updateLoginAttempts(Email);

            LockdownMessage.Text = "Account is locked down. Please try again after 15 minutes." + Email;

            if (Session["wassup"] != null && Request.Cookies["wassup"] != null)
            {
                if (!Session["wassup"].ToString().Equals(Request.Cookies["wassup"].Value))
                {
                    //Add 1 minute to datetime var and add to database
                    //Define now time
                    //Compare database val to now time

                    Session.Clear();
                    Session.Abandon();
                    Session.RemoveAll();

                    if (Request.Cookies["wassup"] != null)
                    {
                        Response.Cookies["wassup"].Value = string.Empty;
                        Response.Cookies["wassup"].Expires = DateTime.Now.AddMonths(-20);
                    }

                    if (Request.Cookies["Email"] != null)
                    {
                        Response.Cookies["Email"].Value = string.Empty;
                        Response.Cookies["Email"].Expires = DateTime.Now.AddMonths(-20);
                    }

/*                    if (Request.Cookies["ASP.NET_SessionId"] != null)
                    {
                        Response.Cookies["ASP.NET_SessionId"].Value = string.Empty;
                        Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddMonths(-20);
                    }*/

                    updateLoginAttempts(Email);

                    Response.Redirect("SITConnectLogin.aspx", false);
                }
            }

            else
            {
                Session.Clear();
                Session.Abandon();
                Session.RemoveAll();

                //Response.Redirect("LoginForm.aspx", false);

                if (Request.Cookies["wassup"] != null)
                {
                    Response.Cookies["wassup"].Value = string.Empty;
                    Response.Cookies["wassup"].Expires = DateTime.Now.AddMonths(-20);
                }
                if (Request.Cookies["Email"] != null)
                {
                    Response.Cookies["Email"].Value = string.Empty;
                    Response.Cookies["Email"].Expires = DateTime.Now.AddMonths(-20);
                }

/*                if (Request.Cookies["ASP.NET_SessionId"] != null)
                {
                    Response.Cookies["ASP.NET_SessionId"].Value = string.Empty;
                    Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddMonths(-20);
                }*/
                updateLoginAttempts(Email);

                Response.Redirect("SITConnectLogin.aspx", false);
            }
            //updateLoginAttempts(Email);
        }

        protected string updateLoginAttempts(string userid)
        {

            string s = null;

            SqlConnection connection = new SqlConnection(AssignDBConnectionString);
            string sql = "update USERINFO set LoginAttempts=@VALUE WHERE Email=@EMAIL";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@VALUE", 0);
            command.Parameters.AddWithValue("@EMAIL", userid);

            try
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["LoginAttempts"] != null)
                        {
                            if (reader["LoginAttempts"] != DBNull.Value)
                            {
                                s = reader["LoginAttempts"].ToString();
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