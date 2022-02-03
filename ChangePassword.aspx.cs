﻿using System;
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
    public partial class ChangePassword : System.Web.UI.Page
    {
        string AssignDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ASAssignmentDBConnection"].ConnectionString;
        static string finalHash;
        static string finalHash0;
        static string finalHash1;
        static string finalHash2;
        static string salt;
        byte[] Key;
        byte[] IV;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Retrieve cookie value, replace Response to Request
            comparePassword.Text = Request.Cookies["LoggedIn"].Value;
        }

        protected void btn_ConfirmPassword_Click(object sender, EventArgs e)
        {
            //string pwd = get value from your Textbox
            string pwd = tb_ChangePassword.Text.ToString().Trim();
            //Generate random "salt" 
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] saltByte = new byte[8];
            //Fills array of bytes with a cryptographically strong sequence of random values.
            rng.GetBytes(saltByte);
            salt = Convert.ToBase64String(saltByte);
            SHA512Managed hashing = new SHA512Managed();

            string pwdWithSalt = pwd + salt;
            byte[] plainHash = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwd));
            byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
            finalHash = Convert.ToBase64String(hashWithSalt);

            string email = Request.Cookies["LoggedIn"].Value;

            //Retrieve old passwords (hash+salt)
            string getLatestOldPassHash = getPasswordHash(email);
            string getLatestOldPassSalt = getPasswordSalt(email);
            string getFirstOldPassHash = getPassword1Hash(email);
            string getFirstOldPassSalt = getPassword1Salt(email);
            string getSecOldPassHash = getPassword2Hash(email);
            string getSecOldPassSalt = getPassword2Salt(email);

            //Attach old passwords (hash+salt)
            string compareHash0 = pwd + getLatestOldPassSalt;
            string compareHash1 = pwd + getFirstOldPassSalt;
            string compareHash2 = pwd + getSecOldPassSalt;
            byte[] hashWithSalt0 = hashing.ComputeHash(Encoding.UTF8.GetBytes(compareHash0));
            byte[] hashWithSalt1 = hashing.ComputeHash(Encoding.UTF8.GetBytes(compareHash1));
            byte[] hashWithSalt2 = hashing.ComputeHash(Encoding.UTF8.GetBytes(compareHash2));
            finalHash0 = Convert.ToBase64String(hashWithSalt0);
            finalHash1 = Convert.ToBase64String(hashWithSalt1);
            finalHash2 = Convert.ToBase64String(hashWithSalt2);

            RijndaelManaged cipher = new RijndaelManaged();
            cipher.GenerateKey();
            Key = cipher.Key;
            IV = cipher.IV;

/*          DateTime defineCountDown = DateTime.Now.AddMinutes(1); //Define 1 min countdown
            string newdateTime = defineCountDown.ToString();
            updateChangePasswordTimer(newdateTime, email); //Update to DB
*/
            DateTime currentTime = DateTime.Now; // Current Time
            DateTime getCountdown = Convert.ToDateTime(getChangePasswordTimer(email));
                
            //int result = DateTime.Compare(currentTime, countDown);

            /*            The minimum age is the number of days before users are allowed to change a password. 
             *            The maximum is the number of days after which users must change their password.
             *            The default minimum is one day, both for Windows and the security baselines; 
             *            the maximum defaults to 42 days for Windows and, until recently, 60 days in the 
             *            security baselines.These settings are enabled in almost all default configurations.*/

            if (DateTime.Compare(currentTime, getCountdown).Equals(1)) //if countdown over, can change password
            {
                if (getFirstOldPassHash.Equals(finalHash1)) //if match first old pass
                {
                    comparePassword.Text = "Please use a new password different from your last two passwords.";
                }
                else
                {
                    if (getSecOldPassHash.Equals(finalHash2)) // if match second old pass
                    {
                        comparePassword.Text = "Please use a new password different from your last two passwords.";
                    }
                    else
                    {
                        if (getLatestOldPassHash.Equals(finalHash0))
                        {
                            comparePassword.Text = "Please use a new password different from your last two passwords.";
                        }
                        else
                        {
                            if (tb_ChangePassword.Text.ToString().Trim().Equals(tb_ConfirmPassword.Text.ToString().Trim()))
                            {
                                //replace with latest old password
                                updatePassword1Hash(getPasswordHash(email), email);
                                updatePassword1Salt(getPasswordSalt(email), email);
                                //replace with latest second password
                                updatePassword2Hash(getFirstOldPassHash, email);
                                updatePassword2Salt(getFirstOldPassSalt, email);
                                //update to new password
                                updatePasswordHash(HttpUtility.HtmlEncode(finalHash), email);
                                updatePasswordSalt(HttpUtility.HtmlEncode(salt), email);

                                DateTime defineCountDown = DateTime.Now.AddMinutes(1); //Define 1 min countdown
                                string newdateTime = defineCountDown.ToString();
                                updateChangePasswordTimer(newdateTime, email); //Update to DB

                                //updateChangePasswordTimer(newdateTime, email);
                                //getChangePasswordTimer(email);

                                Response.Redirect("SITConnectHomePage.aspx", false);

                            }
                            else
                            {
                                comparePassword.Text = "Passwords do not match.";
                            }
                        }
                    }
                }
            }
            else // if countdown not over, cannot change password
            {
                comparePassword.Text = "You cannot change your password yet. You may do so after " + getChangePasswordTimer(email);
            }
        }

        protected string getChangePasswordTimer(string userid)
        {

            string s = null;

            SqlConnection connection = new SqlConnection(AssignDBConnectionString);
            string sql = "select ChangePasswordCountdown FROM USERINFO WHERE Email=@EMAIL";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@EMAIL", userid);

            try
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["ChangePasswordCountdown"] != null)
                        {
                            if (reader["ChangePasswordCountdown"] != DBNull.Value)
                            {
                                s = reader["ChangePasswordCountdown"].ToString();
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

        protected string updateChangePasswordTimer(string timer, string userid)
        {

            string s = null;

            SqlConnection connection = new SqlConnection(AssignDBConnectionString);
            string sql = "update USERINFO set ChangePasswordCountdown=@VALUE WHERE Email=@EMAIL";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@VALUE", timer);
            command.Parameters.AddWithValue("@EMAIL", userid);

            try
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["ChangePasswordCountdown"] != null)
                        {
                            if (reader["ChangePasswordCountdown"] != DBNull.Value)
                            {
                                s = reader["ChangePasswordCountdown"].ToString();
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

        //password salt and hash change pass 1
        //pass 1 change pass 2
        //pass new pass to password salt and hash

        protected string getPassword1Hash(string userid)
        {

            string s = null;

            SqlConnection connection = new SqlConnection(AssignDBConnectionString);
            string sql = "select OldPassword1Hash FROM USERINFO WHERE Email=@EMAIL";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@EMAIL", userid);

            try
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["OldPassword1Hash"] != null)
                        {
                            if (reader["OldPassword1Hash"] != DBNull.Value)
                            {
                                s = reader["OldPassword1Hash"].ToString();
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

        protected string getPassword1Salt(string userid)
        {

            string s = null;

            SqlConnection connection = new SqlConnection(AssignDBConnectionString);
            string sql = "select OldPassword1Salt FROM USERINFO WHERE Email=@EMAIL";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@EMAIL", userid);

            try
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["OldPassword1Salt"] != null)
                        {
                            if (reader["OldPassword1Salt"] != DBNull.Value)
                            {
                                s = reader["OldPassword1Salt"].ToString();
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

        protected string getPassword2Hash(string userid)
        {

            string s = null;

            SqlConnection connection = new SqlConnection(AssignDBConnectionString);
            string sql = "select OldPassword2Hash FROM USERINFO WHERE Email=@EMAIL";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@EMAIL", userid);

            try
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["OldPassword2Hash"] != null)
                        {
                            if (reader["OldPassword2Hash"] != DBNull.Value)
                            {
                                s = reader["OldPassword2Hash"].ToString();
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

        protected string getPassword2Salt(string userid)
        {

            string s = null;

            SqlConnection connection = new SqlConnection(AssignDBConnectionString);
            string sql = "select OldPassword2Salt FROM USERINFO WHERE Email=@EMAIL";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@EMAIL", userid);

            try
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["OldPassword2Salt"] != null)
                        {
                            if (reader["OldPassword2Salt"] != DBNull.Value)
                            {
                                s = reader["OldPassword2Salt"].ToString();
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

        protected string updatePasswordHash(string passwordhash, string userid)
        {

            string s = null;

            SqlConnection connection = new SqlConnection(AssignDBConnectionString);
            string sql = "update USERINFO set PasswordHash=@VALUE WHERE Email=@EMAIL";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@VALUE", passwordhash);
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
                                s = reader["PasswordHash"].ToString();
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


        protected string updatePasswordSalt(string passwordsalt, string userid)
        {

            string s = null;

            SqlConnection connection = new SqlConnection(AssignDBConnectionString);
            string sql = "update USERINFO set PasswordSalt=@VALUE WHERE Email=@EMAIL";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@VALUE", passwordsalt);
            command.Parameters.AddWithValue("@EMAIL", userid);

            try
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["PasswordSalt"] != null)
                        {
                            if (reader["PasswordSalt"] != DBNull.Value)
                            {
                                s = reader["PasswordSalt"].ToString();
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

        protected string getPasswordHash(string userid)
        {

            string s = null;

            SqlConnection connection = new SqlConnection(AssignDBConnectionString);
            string sql = "select PasswordHash FROM USERINFO WHERE Email=@EMAIL";
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
                                s = reader["PasswordHash"].ToString();
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

        protected string getPasswordSalt(string userid)
        {

            string s = null;

            SqlConnection connection = new SqlConnection(AssignDBConnectionString);
            string sql = "select PasswordSalt FROM USERINFO WHERE Email=@EMAIL";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@EMAIL", userid);

            try
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["PasswordSalt"] != null)
                        {
                            if (reader["PasswordSalt"] != DBNull.Value)
                            {
                                s = reader["PasswordSalt"].ToString();
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

        protected string updatePassword1Hash(string passwordhash, string userid)
        {

            string s = null;

            SqlConnection connection = new SqlConnection(AssignDBConnectionString);
            string sql = "update USERINFO set OldPassword1Hash=@VALUE WHERE Email=@EMAIL";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@VALUE", passwordhash);
            command.Parameters.AddWithValue("@EMAIL", userid);

            try
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["OldPassword1Hash"] != null)
                        {
                            if (reader["OldPassword1Hash"] != DBNull.Value)
                            {
                                s = reader["OldPassword1Hash"].ToString();
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


        protected string updatePassword1Salt(string passwordsalt, string userid)
        {

            string s = null;

            SqlConnection connection = new SqlConnection(AssignDBConnectionString);
            string sql = "update USERINFO set OldPassword1Salt=@VALUE WHERE Email=@EMAIL";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@VALUE", passwordsalt);
            command.Parameters.AddWithValue("@EMAIL", userid);

            try
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["OldPassword1Salt"] != null)
                        {
                            if (reader["OldPassword1Salt"] != DBNull.Value)
                            {
                                s = reader["OldPassword1Salt"].ToString();
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

        protected string updatePassword2Hash(string passwordhash, string userid)
        {

            string s = null;

            SqlConnection connection = new SqlConnection(AssignDBConnectionString);
            string sql = "update USERINFO set OldPassword2Hash=@VALUE WHERE Email=@EMAIL";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@VALUE", passwordhash);
            command.Parameters.AddWithValue("@EMAIL", userid);

            try
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["OldPassword2Hash"] != null)
                        {
                            if (reader["OldPassword2Hash"] != DBNull.Value)
                            {
                                s = reader["OldPassword2Hash"].ToString();
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


        protected string updatePassword2Salt(string passwordsalt, string userid)
        {

            string s = null;

            SqlConnection connection = new SqlConnection(AssignDBConnectionString);
            string sql = "update USERINFO set OldPassword2Salt=@VALUE WHERE Email=@EMAIL";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@VALUE", passwordsalt);
            command.Parameters.AddWithValue("@EMAIL", userid);

            try
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["OldPassword2Salt"] != null)
                        {
                            if (reader["OldPassword2Salt"] != DBNull.Value)
                            {
                                s = reader["OldPassword2Salt"].ToString();
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