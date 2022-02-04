using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

//Password Checker
using System.Text.RegularExpressions;
using System.Drawing;
//Password with Hash and Salt
using System.Security.Cryptography;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace _204703Q_AS_CodingAssignment_Ver2
{
    public partial class SITConnectRegistration : System.Web.UI.Page
    {
        string AssignDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ASAssignmentDBConnection"].ConnectionString;
        static string finalHash;
        static string salt;
        byte[] Key;
        byte[] IV;

        //Display the Picture in Image control.
        //ImageDisplay.ImageUrl = "~/Images/" + Path.GetFileName(ImageUpload.FileName);

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private int checkPassword(string password)
        {
            int score = 0;
            //Score 0 very weak!

            //Score 1 very weak!
            if (password.Length < 12)
            {
                return 1;
            }
            else
            {
                score = 1;
            }

            //Score 2 weak!
            if (Regex.IsMatch(password, "[a-z]"))
            {
                score++;

            }
            //Score 3 Medium!
            if (Regex.IsMatch(password, "[A-Z]"))
            {
                score++;

            }
            //Score 4 Strong!
            if (Regex.IsMatch(password, "[0-9]"))
            {
                score++;
            }
            //Score 5 Excellent!
            if (Regex.IsMatch(password, "[^a-zA-Z0-9]"))
            {
                score++;

            }
            return score;
        }

        protected void confirmPassword_Click(object sender, EventArgs e)
        {
            int scores = checkPassword(Password.Text);
            string status = "";

            switch (scores)
            {
                case 1:
                    status = "Very Weak!";
                    break;

                case 2:
                    status = "Weak!";
                    break;

                case 3:
                    status = "Medium!";
                    break;

                case 4:
                    status = "Strong!";
                    break;

                case 5:
                    status = "Excellent!";
                    break;

                default:
                    break;
            }
            lbl_pwdchecker.Text = "Status: " + status;
            if (scores < 4)
            {
                lbl_pwdchecker.ForeColor = Color.Red;
                return;
            }
            lbl_pwdchecker.ForeColor = Color.Green;

            /*          @FirstName
                        @LastName
                        @CreditCardInfo
                        @Email
                        @PasswordHash
                        @PasswordSalt
                        @DateOfBirth
                        @Photo
                        @IV
                        @Key
                        @EmailVerified*/

            if (Regex.IsMatch(firstName.Text.Trim(), "^[ a-zA-Z]+$"))
            {
                firstNameMsg.Text = "";

                if (Regex.IsMatch(lastName.Text.Trim(), "^[ a-zA-Z]+$"))
                {
                    lastNameMsg.Text = "";

                    if (Regex.IsMatch(CreditCardName.Text.Trim(), "^[ a-zA-Z]+$"))
                    {
                        creditNameMsg.Text = "";

                        if (Regex.IsMatch(CreditCardNumber.Text.Trim(), "^[0-9]{16}$"))
                        {
                            creditNumberMsg.Text = "";

                            if (Regex.IsMatch(CreditCardExpireMonth.Text.Trim(), "^((0[1-9])|(1[0-2]))$"))
                            {
                                creditExpireMonthMsg.Text = "";

                                if (Regex.IsMatch(CreditCardExpireYear.Text.Trim(), "^[0-9]{2}$"))
                                {
                                    creditExpireYearMsg.Text = "";

                                    if (Regex.IsMatch(CreditCardCCV.Text.Trim(), "^[0-9]{3}$"))
                                    {
                                        creditExpireYearMsg.Text = "";

                                        if (Regex.IsMatch(Email.Text.Trim(), "^[a-zA-Z0-9+_.-]+@[a-zA-Z0-9.-]+$"))
                                        {
                                            emailMsg.Text = "";
                                            
                                            if (Email.Text.Trim().Equals(getEmail(Email.Text.Trim())))
                                            {
                                                AccountCreated.Text = "Gmail Account already used. Please use a new Gmail account!";
                                                AccountCreated.ForeColor = Color.Red;
                                            }
                                            else
                                            {
                                                AccountCreated.Text = "";
                                                //string pwd = get value from your Textbox
                                                string pwd = Password.Text.ToString().Trim();
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
                                                RijndaelManaged cipher = new RijndaelManaged();
                                                cipher.GenerateKey();
                                                Key = cipher.Key;
                                                IV = cipher.IV;
                                                createAccount();

                                                updateLog(Email.Text.Trim());

                                                string folderPath = Server.MapPath("~/Images/");

                                                //Check whether Directory (Folder) exists.
                                                if (!Directory.Exists(folderPath))
                                                {
                                                    //If Directory (Folder) does not exists Create it.
                                                    Directory.CreateDirectory(folderPath);
                                                }

                                                //Save the File to the Directory (Folder).
                                                ImageUpload.SaveAs(folderPath + Path.GetFileName(ImageUpload.FileName));

                                                Response.Redirect("SITConnectLogin.aspx", false);
                                            }
                                        }
                                        else
                                        {
                                            emailMsg.Text = "Invalid Email.";
                                            emailMsg.ForeColor = Color.Red;
                                        }
                                    }
                                    else
                                    {
                                        creditCCVMsg.Text = "Invalid CCV.";
                                        creditCCVMsg.ForeColor = Color.Red;
                                    }
                                }
                                else
                                {
                                    creditExpireYearMsg.Text = "Invalid Year.";
                                    creditExpireYearMsg.ForeColor = Color.Red;
                                }
                            }
                            else
                            {
                                creditExpireMonthMsg.Text = "Invalid Month.";
                                creditExpireMonthMsg.ForeColor = Color.Red;
                            }
                        }
                        else
                        {
                            creditNumberMsg.Text = "Only 16 Digit Numbers allowed.";
                            creditNumberMsg.ForeColor = Color.Red;
                        }
                    }
                    else
                    {
                        creditNameMsg.Text = "Only alphabets allowed.";
                        creditNameMsg.ForeColor = Color.Red;
                    }
                }
                else
                {
                    lastNameMsg.Text = "Only alphabets allowed.";
                    lastNameMsg.ForeColor = Color.Red;
                }
            }
            else
            {
                firstNameMsg.Text = "Only alphabets allowed.";
                firstNameMsg.ForeColor = Color.Red;
            }
        }

        protected void Submit_Click(object sender, EventArgs e)
        {

        }

        protected void createAccount()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(AssignDBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO UserInfo VALUES(@FirstName, @LastName, @CreditCardNumber, @CreditCardName, @CreditCardSecurityCode, @CreditCardExpireMonth, @CreditCardExpireYear, @Email, @PasswordHash, @PasswordSalt, @DateOfBirth, @Photo, @IV, @Key, @LoginAttempts, @RecoveryTime, @EmailOTP, @OldPassword1Hash, @OldPassword1Salt, @OldPassword2Hash, @OldPassword2Salt, @ChangePasswordCountdown, @MustChangePasswordCountdown)"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@FirstName", HttpUtility.HtmlEncode(firstName.Text.Trim()));
                            cmd.Parameters.AddWithValue("@LastName", HttpUtility.HtmlEncode(lastName.Text.Trim()));
                            cmd.Parameters.AddWithValue("@CreditCardNumber", HttpUtility.HtmlEncode(Convert.ToBase64String(encryptData(CreditCardNumber.Text.Trim()))));
                            cmd.Parameters.AddWithValue("@CreditCardName", HttpUtility.HtmlEncode(Convert.ToBase64String(encryptData(CreditCardName.Text))));
                            cmd.Parameters.AddWithValue("@CreditCardSecurityCode", HttpUtility.HtmlEncode(Convert.ToBase64String(encryptData(CreditCardCCV.Text.Trim()))));
                            cmd.Parameters.AddWithValue("@CreditCardExpireMonth", HttpUtility.HtmlEncode(Convert.ToBase64String(encryptData(CreditCardExpireMonth.Text.Trim()))));
                            cmd.Parameters.AddWithValue("@CreditCardExpireYear", HttpUtility.HtmlEncode(Convert.ToBase64String(encryptData(CreditCardExpireYear.Text.Trim()))));
                            cmd.Parameters.AddWithValue("@Email", HttpUtility.HtmlEncode(Email.Text.Trim()));
                            cmd.Parameters.AddWithValue("@PasswordHash", HttpUtility.HtmlEncode(finalHash));
                            cmd.Parameters.AddWithValue("@PasswordSalt", HttpUtility.HtmlEncode(salt));
                            cmd.Parameters.AddWithValue("@DateOfBirth", HttpUtility.HtmlEncode(DateofBirth.Text.Trim()));
                            string ImageName = ImageUpload.FileName;
                            cmd.Parameters.AddWithValue("@Photo", HttpUtility.HtmlEncode(ImageName));
                            cmd.Parameters.AddWithValue("@IV", HttpUtility.HtmlEncode(Convert.ToBase64String(IV)));
                            cmd.Parameters.AddWithValue("@Key", HttpUtility.HtmlEncode(Convert.ToBase64String(Key)));
                            cmd.Parameters.AddWithValue("@LoginAttempts", 0);
                            DateTime dateTime = DateTime.Now;
                            string newdateTime = dateTime.ToString();
                            cmd.Parameters.AddWithValue("@RecoveryTime", newdateTime);
                            cmd.Parameters.AddWithValue("@EmailOTP", "");
                            cmd.Parameters.AddWithValue("@OldPassword1Hash", "");
                            cmd.Parameters.AddWithValue("@OldPassword1Salt", "");
                            cmd.Parameters.AddWithValue("@OldPassword2Salt", ""); 
                            cmd.Parameters.AddWithValue("@OldPassword2Hash", "");
                            DateTime dateTimeMin = DateTime.Now.AddMinutes(1);
                            DateTime dateTimeMax = DateTime.Now.AddMinutes(30);
                            string newdateTimeMin = dateTime.ToString();
                            string newdateTimeMax = dateTime.ToString();
                            cmd.Parameters.AddWithValue("@ChangePasswordCountdown", newdateTimeMin);
                            cmd.Parameters.AddWithValue("@MustChangePasswordCountdown", newdateTimeMax);
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
                            cmd.Parameters.AddWithValue("@Changes", "CREATING");
                            cmd.Parameters.AddWithValue("@Operation", "New User Registered");
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

        protected byte[] encryptData(string data)
        {
            byte[] cipherText = null;
            try
            {
                RijndaelManaged cipher = new RijndaelManaged();
                cipher.IV = IV;
                cipher.Key = Key;
                ICryptoTransform encryptTransform = cipher.CreateEncryptor();
                ICryptoTransform decryptTransform = cipher.CreateDecryptor();
                byte[] plainText = Encoding.UTF8.GetBytes(data);
                cipherText = encryptTransform.TransformFinalBlock(plainText, 0, plainText.Length);


                //Encrypt
                //cipherText = encryptTransform.TransformFinalBlock(plainText, 0, plainText.Length);
                //cipherString = Convert.ToBase64String(cipherText);
                //Console.WriteLine("Encrypted Text: " + cipherString);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally { }
            return cipherText;
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
    }
}