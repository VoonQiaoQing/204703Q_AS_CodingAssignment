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
            //int result = Convert.ToInt32(Request.Cookies["wassup"].Value);
            string Email = Request.Cookies["LoggedIn"].Value;
            updateLog(Email);

            //LockdownMessage.Text = "Account is locked down. Please try again after 15 minutes." + Email;

            if (Session["LoggedIn"] != null && Session["AuthToken"] != null && Request.Cookies["AuthToken"] != null)
            {
                if (!Session["AuthToken"].ToString().Equals(Request.Cookies["AuthToken"].Value))
                {
                    //Add 1 minute to datetime var and add to database
                    //Define now time
                    //Compare database val to now time

                    Session.Clear();
                    Session.Abandon();
                    Session.RemoveAll();

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

                    updateLoginAttempts(HttpUtility.HtmlEncode(Email));

                    Response.Redirect("SITConnectLogin.aspx", false);
                }
                else
                {
                    //compare
                    //LockdownMessage.Text = "Account is locked down. Please try again after 15 minutes." + Email;

                    DateTime currentTime = DateTime.Now;
                    DateTime countDown = Convert.ToDateTime(getRecoveryTime(Email));
                        
                    int result = DateTime.Compare(currentTime, countDown);

                    if (result > 0)
                    {
                        Session.Clear();
                        Session.Abandon();
                        Session.RemoveAll();

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
                        
                        updateLoginAttempts(Email);
                        Response.Redirect("SITConnectLogin.aspx", false);
                    }
                    else
                    {
                        LockdownMessage.Text = Email + " account is locked down. DO NOT LEAVE this page. Refresh this page after " + getRecoveryTime(Email) + ".";
                        LockdownMessage.ForeColor = System.Drawing.Color.Red;
                    }
                }
            }

            else
            {
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

                //updateLoginAttempts(Email);

                Response.Redirect("SITConnectLogin.aspx", false);
            }
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

        protected string getRecoveryTime(string userid)
        {

            string s = null;

            SqlConnection connection = new SqlConnection(AssignDBConnectionString);
            string sql = "select RecoveryTime FROM USERINFO WHERE Email=@EMAIL";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@EMAIL", userid);

            try
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["RecoveryTime"] != null)
                        {
                            if (reader["RecoveryTime"] != DBNull.Value)
                            {
                                s = reader["RecoveryTime"].ToString();
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

        protected void updateLog(string Email)
        {

            try
            {
                using (SqlConnection con = new SqlConnection(AssignDBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO AuditLog VALUES(@User, @Changes, @Operation, @OccurredAt)"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@User", Email);
                            cmd.Parameters.AddWithValue("@Changes", "LOCKED");
                            cmd.Parameters.AddWithValue("@Operation", "Account is locked from fail attempts");
                            cmd.Parameters.AddWithValue("@OccurredAt", DateTime.Now.ToString());
                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

    }
}