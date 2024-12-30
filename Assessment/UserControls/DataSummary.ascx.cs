using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using Assessment.ServiceReference1;

namespace Assessment.UserControls
{
    public partial class DataSummary : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public void BindUserSummary(DataListInfo data)
        {

            tdFirstName.InnerText = data.FirstName;
            tdLastName.InnerText = data.LastName;
            tdEmailId.InnerText = data.Email;
            //tdRole.InnerText = data.Role;
            //ddlRole.SelectedValue = data.Role;
            

            // Set the selected value, only if it exists in the dropdown
            if (ddlRole.Items.FindByValue(data.Role) != null)
            {
                ddlRole.SelectedValue = data.Role;
            }

            ddlRole.Attributes["data-userid"] = data.UserId.ToString();


        }
    }
}