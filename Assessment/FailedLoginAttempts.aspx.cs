using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Assessment
{
    public partial class FailedLoginAttempts : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    int userId = 0;
                    string emailId = string.Empty;
                    string password = string.Empty;

                    // Extracting query string parameters safely
                    if (Request.QueryString["userId"] != null)
                    {
                        userId = Convert.ToInt32(Request.QueryString["userId"]);
                    }

                    if (Request.QueryString["email"] != null)
                    {
                        emailId = Request.QueryString["email"];
                    }

                    if (Request.QueryString["password"] != null)
                    {
                        password = Request.QueryString["password"];
                    }

                    // Check if the required values are not empty
                    if (!string.IsNullOrEmpty(emailId) && !string.IsNullOrEmpty(password))
                    {
                        //string script = $"LoadUserDetails({userId}, {emailId},{password})";
                        string script = $"LoadUserDetails({userId}, '{emailId}', '{password}');";
                        Page.ClientScript.RegisterStartupScript(GetType(), "LoadUserDetails", script, true);
                    }
                }
                catch (Exception ex)
                {
                    // Handle any errors gracefully
                    Console.WriteLine($"Error in Page_Load: {ex.Message}");
                }
            }
        }
    }
}