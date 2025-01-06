using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Assessment
{
    public partial class Admin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["userId"] != null)
                {
                    int userId = Convert.ToInt32(Request.QueryString["userId"]);
                    string adminFirstName = BLL.CommonBLL.GetAdminFirstName(userId);

                    ClientScript.RegisterStartupScript(this.GetType(), "setAdminName",
                        $"document.getElementById('lblAdminName').innerHTML = 'Admin {adminFirstName}';", true);
                }

                #region Script_Resource_Refernce
                ScriptReference scriptReference = new ScriptReference();
                scriptReference.Path = ConfigurationManager.AppSettings["jsPath"].ToString().TrimEnd('/') + "/ScriptResources/AdminScriptResource.js";

                scriptReference.Path = scriptReference.Path;
                ScriptManager1.Scripts.Add(scriptReference);
                #endregion
                uploadFile.Attributes.Add("onchange", "javascript:UploadExcelFile('" + uploadFile.ClientID + "');");
                //btnDownloadUserInfo.Attributes.Add("onclick", "javascript:DownloadAllLogFail('" + btnDownloadUserInfo.ClientID + "' );return false;");
            }


        }
    }
}