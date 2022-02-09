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
using System.Diagnostics;

using System.Text.RegularExpressions;
using System.Drawing;

namespace _204703Q_AS_CodingAssignment_Ver2
{
    public partial class SITConnectLogin : System.Web.UI.Page
    {
        string AssignDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ASAssignmentDBConnection"].ConnectionString;

        public class MyObject
        {
            public string success { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ResendCode_Click(object sender, EventArgs e)
        {
            if (ValidateCaptcha())
            {
                string userid = HttpUtility.HtmlEncode(tb_loginEmail.Text.ToString().Trim());
                Random ran = new Random();

                int EmailOTP = ran.Next(100000, 999999);

                string ToEmail = userid;
                string Subj = "Confirm your email for SITConnect Login!";
                string Message = "Your 6 Digit OTP is " + EmailOTP;

                SendEmail(ToEmail, Subj, Message);

                updateEmailOTP(EmailOTP.ToString(), userid);
            }
        }


        protected void loginSubmit_Click(object sender, EventArgs e)
        {     
            string pwd = HttpUtility.HtmlEncode(tb_loginPassword.Text.ToString().Trim());
            string userid = HttpUtility.HtmlEncode(tb_loginEmail.Text.ToString().Trim());

            SHA512Managed hashing = new SHA512Managed();
            string dbHash = getDBHash(userid);
            string dbSalt = getDBSalt(userid);

            try
            {
                if (dbSalt != null && dbSalt.Length > 0 && dbHash != null && dbHash.Length > 0)
                {

                    string pwdWithSalt = pwd + dbSalt;
                    byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
                    string userHash = Convert.ToBase64String(hashWithSalt);

                    if (ValidateCaptcha())
                    {
                        if (userHash.Equals(dbHash)) //Credentails match
                        {
                            if (Convert.ToInt32(getAttempts(userid)) < 3) //Attempts lower than 3
                            {
                                //PLEASE ADD UPDATE ATTEMPTS TO 0 TO CLEAR IT
                                updateLoginAttemptsto0(userid);

                                Random ran = new Random();

                                int EmailOTP = ran.Next(100000, 999999);

                                string ToEmail = userid;
                                string Subj = "Confirm your email for SITConnect Login!";
                                string Message = "Your 6 Digit OTP is " + EmailOTP;

                                //REOPEN
                                SendEmail(ToEmail, Subj, Message);

                                updateEmailOTP(EmailOTP.ToString(), userid);

                                OTPField.Visible = true;

                            }
                            else  //Attempts higher than 3
                            {
                                DateTime endCountdown = DateTime.Now.AddMinutes(1);
                                string endCountdownString = endCountdown.ToString();
                                DateTime currentTime = DateTime.Now;

                                if (DateTime.Compare(currentTime, Convert.ToDateTime(getRecoveryTime(userid))).Equals(1))
                                {
                                    updateRecoveryTime(endCountdownString, userid);
                                }

                                Session["LoggedIn"] = userid;

                                string guid = Guid.NewGuid().ToString();
                                Session["AuthToken"] = guid;

                                Response.Cookies.Add(new HttpCookie("LoggedIn", userid));
                                Response.Cookies.Add(new HttpCookie("AuthToken", guid));

                                Response.Redirect("LockAccount.aspx", false);
                            }
                        }
                        else //Credentails does not match
                        {
                            if (Convert.ToInt32(getAttempts(userid)) > 2)
                            {
                                DateTime endCountdown = DateTime.Now.AddMinutes(1);
                                string endCountdownString = endCountdown.ToString();
                                DateTime currentTime = DateTime.Now;

                                if (DateTime.Compare(currentTime, Convert.ToDateTime(getRecoveryTime(userid))).Equals(1))
                                {
                                    updateRecoveryTime(endCountdownString, userid);
                                }

                                Session["LoggedIn"] = userid;

                                string guid = Guid.NewGuid().ToString();
                                Session["AuthToken"] = guid;

                                Response.Cookies.Add(new HttpCookie("LoggedIn", userid));
                                Response.Cookies.Add(new HttpCookie("AuthToken", guid));

                                Response.Redirect("LockAccount.aspx", false);
                            }
                            else
                            {
                                int increase = Convert.ToInt32(getAttempts(userid)) + 1;
                                increaseAttempts(increase, userid);

                                loginFail.Text = "Email or Password is wrong. Please try again. Attempts: " + getAttempts(userid).ToString() + "/3";
                                loginFail.ForeColor = Color.Red;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());

            }

            finally
            {

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
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            NetworkCredential NetworkCred = new NetworkCredential();
            NetworkCred.UserName = FromEmailId;
            NetworkCred.Password = Password;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = NetworkCred;
            smtp.Port = 587;
            //smtp.Port = 25;
            smtp.Send(mailMessage);
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

        protected string updateLoginAttemptsto0(string userid)
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


        protected string updateRecoveryTime(string RevTime, string userid)
        {

            string s = null;

            SqlConnection connection = new SqlConnection(AssignDBConnectionString);
            string sql = "update USERINFO set RecoveryTime=@VALUE WHERE Email=@EMAIL";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@VALUE", RevTime);
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

        protected string updateRecoveryTime(int attemptvalues, string userid)
        {

            string s = null;

            SqlConnection connection = new SqlConnection(AssignDBConnectionString);
            string sql = "update USERINFO set LoginAttempts=@VALUE WHERE Email=@EMAIL";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@VALUE", attemptvalues);
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

        protected string getEmail(string userid)
        {

            string s = null;

            SqlConnection connection = new SqlConnection(AssignDBConnectionString);
            string sql = "select EMAIL FROM USERINFO WHERE Email=@EMAIL";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@EMAIL", userid);

            try
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["EMAIL"] != null)
                        {
                            if (reader["EMAIL"] != DBNull.Value)
                            {
                                s = reader["EMAIL"].ToString();
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

        protected string getAttempts(string userid)
        {

            string s = null;

            SqlConnection connection = new SqlConnection(AssignDBConnectionString);
            string sql = "select LOGINATTEMPTS FROM USERINFO WHERE Email=@EMAIL";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@EMAIL", userid);

            try
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["LOGINATTEMPTS"] != null)
                        {
                            if (reader["LOGINATTEMPTS"] != DBNull.Value)
                            {
                                s = reader["LOGINATTEMPTS"].ToString();
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

        protected string increaseAttempts(int attemptvalues, string userid)
        {

            string s = null;

            SqlConnection connection = new SqlConnection(AssignDBConnectionString);
            string sql = "update USERINFO set LoginAttempts=@VALUE WHERE Email=@EMAIL";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@VALUE", attemptvalues);
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

        protected string getDBSalt(string userid)
        {

            string s = null;

            SqlConnection connection = new SqlConnection(AssignDBConnectionString);
            string sql = "select PASSWORDSALT FROM USERINFO WHERE Email=@EMAIL";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@EMAIL", userid);

            try
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["PASSWORDSALT"] != null)
                        {
                            if (reader["PASSWORDSALT"] != DBNull.Value)
                            {
                                s = reader["PASSWORDSALT"].ToString();
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

        protected string getDBHash(string userid)
        {

            string h = null;

            SqlConnection connection = new SqlConnection(AssignDBConnectionString);
            string sql = "select PasswordHash FROM UserInfo WHERE Email=@EMAIL";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@EMAIL", userid);

            try
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        if (reader["PasswordHash"] != null)
                        {
                            if (reader["PasswordHash"] != DBNull.Value)
                            {
                                h = reader["PasswordHash"].ToString();
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
            return h;
        }

        public bool ValidateCaptcha()
        {
            bool result = true;

            //When user submits the recaptcha form, the user gets a response POST parameter. 
            //captchaResponse consist of the user click pattern. Behaviour analytics! AI :) 
            string captchaResponse = Request.Form["g-recaptcha-response"];
            string reCaptchaSecret = ConfigurationManager.AppSettings["CaptchaSecret"].ToString();
            //To send a GET request to Google along with the response and Secret key.
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create
           (" https://www.google.com/recaptcha/api/siteverify?secret=" + reCaptchaSecret +"&response=" + captchaResponse);


            try
            {

                //Codes to receive the Response in JSON format from Google Server
                using (WebResponse wResponse = req.GetResponse())
                {
                    using (StreamReader readStream = new StreamReader(wResponse.GetResponseStream()))
                    {
                        //The response in JSON format
                        string jsonResponse = readStream.ReadToEnd();

                        //To show the JSON response string for learning purpose

                        JavaScriptSerializer js = new JavaScriptSerializer();

                        //Create jsonObject to handle the response e.g success or Error
                        //Deserialize Json
                        MyObject jsonObject = js.Deserialize<MyObject>(jsonResponse);

                        //Convert the string "False" to bool false or "True" to bool true
                        result = Convert.ToBoolean(jsonObject.success);//

                    }
                }

                return result;
            }
            catch (WebException ex)
            {
                throw ex;
            }
        }

        protected void registerUser_Click(object sender, EventArgs e)
        {
            Response.Redirect("SITConnectRegistration.aspx", false);
        }

        protected void OTPSubmit_Click(object sender, EventArgs e)
        {
            string userid = HttpUtility.HtmlEncode(tb_loginEmail.Text.ToString().Trim());

            if (getEmailOTP(userid).Equals(HttpUtility.HtmlEncode(tb_EmailOTP.Text.ToString().Trim())))
            {
                Session["LoggedIn"] = userid;

                string guid = Guid.NewGuid().ToString();
                Session["AuthToken"] = guid;

                //Session["Email"] = userid;
                Response.Cookies.Add(new HttpCookie("LoggedIn", userid));
                Response.Cookies.Add(new HttpCookie("AuthToken", guid));

                Response.Redirect("SITConnectHomePage.aspx", false);
            }
            else
            {
                EmailLabel.Text = "Email OTP is wrong. Please check your OTP or request a new one.";
            }
        }
    }
}