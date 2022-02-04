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
            string Email = Request.Cookies["Email"].Value;

            //LockdownMessage.Text = "Account is locked down. Please try again after 15 minutes." + Email;

            if (Session["Email"] != null && Request.Cookies["Email"] != null)
            {
                if (!Session["Email"].ToString().Equals(Request.Cookies["Email"].Value))
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

                    //updateLoginAttempts(Email);

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

                        updateLoginAttempts(Email);
                        Response.Redirect("SITConnectLogin.aspx", false);
                    }
                    else
                    {
                        LockdownMessage.Text = Email + "account is locked down. DO NOT LEAVE this page. Refresh this page after " + getRecoveryTime(Email) + ".";
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

    }
}