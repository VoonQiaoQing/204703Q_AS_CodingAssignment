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

//Password Checker
using System.Text.RegularExpressions;
using System.Drawing;

namespace _204703Q_AS_CodingAssignment_Ver2
{

    public partial class AuditLog : System.Web.UI.Page
    {
        SqlDataAdapter da;
        DataSet ds = new DataSet();
        StringBuilder htmlTable = new StringBuilder();

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
                    if (!Page.IsPostBack)
                        BindData();
                }
            }
            else
            {
                Response.Redirect("LoginForm.aspx", false);
            }
        }
        private void BindData()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ASAssignmentDBConnection"].ConnectionString.ToString();
            SqlCommand cmd = new SqlCommand("SELECT * FROM AuditLog", con);
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

            htmlTable.Append("<table border='1' style='color: black;border: thin; width: 1100px;'>");
            htmlTable.Append("<tr><th>AuditId</th><th>Changes</th><th>User</th><th>Operation</th><th>OccurredAt</th></tr>");

            if (!object.Equals(ds.Tables[0], null))
            {
                if (ds.Tables[0].Rows.Count > 0)
                {

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        htmlTable.Append("<tr>");
                        htmlTable.Append("<td>" + ds.Tables[0].Rows[i]["AuditId"] + "</td>");
                        htmlTable.Append("<td>" + ds.Tables[0].Rows[i]["Changes"] + "</td>");
                        htmlTable.Append("<td>" + ds.Tables[0].Rows[i]["User"] + "</td>");
                        htmlTable.Append("<td>" + ds.Tables[0].Rows[i]["Operation"] + "</td>");
                        htmlTable.Append("<td>" + ds.Tables[0].Rows[i]["OccurredAt"] + "</td>");
                        htmlTable.Append("</tr>");
                    }
                    htmlTable.Append("</table>");
                    DBDataPlaceHolder.Controls.Add(new Literal { Text = htmlTable.ToString() });
                }
                else
                {
                    htmlTable.Append("<tr>");
                    htmlTable.Append("<td>There is no Record.</td>");
                    htmlTable.Append("</tr>");
                }
            }
        }

    }
}