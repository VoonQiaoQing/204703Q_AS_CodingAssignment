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
    public partial class EmailConfirmation : System.Web.UI.Page
    {
        string AssignDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ASAssignmentDBConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies["Email"] != null)
            {
                EmailLabel.Text = Response.Cookies["Email"].Value;
            } 
        }

        protected void Submit_Click(object sender, EventArgs e)
        {
            string email = Response.Cookies["Email"].Value;


            if (getEmailOTP(email).Equals(EmailOTP.Text))
            {
                Response.Redirect("SITConnectHomePage.aspx", false);
            }
            
        }

        protected void Back_Click(object sender, EventArgs e)
        {
            Response.Redirect("SITConnectLogin.aspx", false);
        }

        protected void ResendCode_Click(object sender, EventArgs e)
        {
            Random ran = new Random();
            int newEmailOTP = ran.Next(1, 999999);

            updateEmailOTP(newEmailOTP.ToString(), Response.Cookies["LoggedIn"].Value);

            string ToEmail = Response.Cookies["LoggedIn"].Value;
            string Subj = "Confirm your email for SITConnect Login!";
            string Message = "Your 6 Digit OTP is " + newEmailOTP;

            SendEmail(ToEmail, Subj, Message);

            if (getEmailOTP(Response.Cookies["Email"].Value).Equals(EmailOTP.Text))
            {
                Response.Redirect("SITConnectHomePage.aspx", false);
            }

        }


        protected static void SendEmail(string ToEmail, string Subj, string Message)
        {
            string HostAdd = ConfigurationManager.AppSettings["Host"].ToString();
            string FromEmailId = ConfigurationManager.AppSettings["FromMail"].ToString();
            string Password = ConfigurationManager.AppSettings["Password"].ToString();

            MailMessage mailMessage = new MailMessage();

            mailMessage.From = new MailAddress(FromEmailId);
            mailMessage.Subject = Subj;
            mailMessage.Body = Message;
            mailMessage.IsBodyHtml = true;
            mailMessage.To.Add(new MailAddress(ToEmail));

            SmtpClient smtp = new SmtpClient();
            smtp.Host = HostAdd;

            smtp.EnableSsl = true;
            NetworkCredential NetworkCred = new NetworkCredential();
            NetworkCred.UserName = FromEmailId;
            NetworkCred.Password = Password;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = NetworkCred;
            smtp.Port = 587;
            smtp.Send(mailMessage);
        }

        protected string getEmailOTP(string userid)
        {

            string s = null;

            SqlConnection connection = new SqlConnection(AssignDBConnectionString);
            string sql = "select EMAILOTP FROM USERINFO WHERE Email=@EMAIL";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@EMAIL", userid);

            try
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["EMAILOTP"] != null)
                        {
                            if (reader["EMAILOTP"] != DBNull.Value)
                            {
                                s = reader["EMAILOTP"].ToString();
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


        protected string updateEmailOTP(string OTP, string userid)
        {

            string s = null;

            SqlConnection connection = new SqlConnection(AssignDBConnectionString);
            string sql = "update USERINFO set EmailOTP=@VALUE WHERE Email=@EMAIL";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@VALUE", OTP);
            command.Parameters.AddWithValue("@EMAIL", userid);

            try
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["EmailOTP"] != null)
                        {
                            if (reader["EmailOTP"] != DBNull.Value)
                            {
                                s = reader["EmailOTP"].ToString();
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