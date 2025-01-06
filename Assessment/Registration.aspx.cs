using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Assessment
{
    public partial class Registration : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int userID = 0;

                if (int.TryParse(Request.QueryString["Id"], out userID))
                {
                    if (userID > 0)
                    {
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "UpdateUserInfoScript", $"UpdateUserInfo({userID});", true);
                        btnSubmit.InnerText = "Update";
                        btnSubmit.Attributes.Add("onclick", $"UpdateUserData({userID}); return false;");
                    }
                    else
                    {
                        btnSubmit.Attributes.Add("onclick", "Register(event); return false;");
                        btnSubmit.InnerText = "Add Register";
                    }
                }

            }
        }
        //protected void btnSubmit_Click(object sender, EventArgs e)
        //{
        //    // Perform any necessary validation or save logic here.

        //    // Redirect to Login.aspx after successful registration.
        //    Response.Redirect("Login.aspx");
        //}

    }
}