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
            //btnDownloadPPE.Attributes.Add("onclick", "javascript:DownloadPPECode('btnDownloadPPE.ClientID');return false;");

            //if (HttpContext.Current.User.Identity.IsAuthenticated)
            //{
            //    string loggedInUser = HttpContext.Current.User.Identity.Name;
            //    lblAdminName.Text = "Admin: " + loggedInUser;

            //}
            //if (!IsPostBack)
            //{
            //    #region Script_Resource_Refernce
            //    ScriptReference scriptReference = new ScriptReference();
            //    scriptReference.Path = ConfigurationManager.AppSettings["jsPath"].ToString().TrimEnd('/') + "/ScriptResources/UserScriptResource_v2.js";

            //    BLL.GetCultures(ref scriptReference, this.CurrentUser.Language);
            //    scriptReference.Path = scriptReference.Path + "?v=" + VersionNumber;
            //    ScriptManager1.Scripts.Add(scriptReference);
            //    #endregion

            //    btnDownloadUserInfo.Attributes.Add("onclick", "javascript:DownloadAllUserInfo('" + btnDownloadUserInfo.ClientID + "' );return false;");
            //}
           
        }
    }
}