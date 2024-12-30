using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
//using System.ServiceModel;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using Assessment.ServiceReference1;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Xml.Serialization;
using System.ServiceModel;

namespace Assessment.WebService
{
    /// <summary>
    /// Summary description for UserRegistration
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class UserRegistration : System.Web.Services.WebService
    {

        [System.Web.Services.WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public int RegisterDetails(string firstName, string lastName, string emailId, string mobileNo, string password)
        {
            try
            {

                return BLL.CommonBLL.RegistrationDetails(firstName, lastName, emailId, mobileNo, password);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return -1;
            }
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public UserClass LoginDetails(string emailId, string password)
        {
            UserClass user = new UserClass();
            try
            {
                // Fetch UserId and RoleId from the database
                user = BLL.CommonBLL.LoginDetails(emailId, password);

                if (user != null && user.UserId != 0)
                {

                    return user; // Successful login
                }
                else
                {
                    // Log failed login attempt
                    user = BLL.CommonBLL.LogFailedLoginAttempt(emailId, password);
                    // Failed login

                    return user;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw new FaultException($"An error occurred while logging in: {ex.Message}");

            }
        }
        //logFailed
        [System.Web.Services.WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public UserBindInformation LogInfo(string _email, string _password)
        {
            //try
            //{
            string userControlPath = ConfigurationManager.AppSettings["UserControls"].TrimEnd('/');
            UserBindInformation ctrl = new UserBindInformation();
            System.Web.UI.Page page = new Page();
            HtmlForm form = new HtmlForm();

            List<LogFailed> userList = BLL.CommonBLL.GetLogInfo(_email, _password);
            if (userList.Count > 0)
            {
                foreach (LogFailed user in userList)
                {
                    UserControls.LogFailedSummary uclogSummary = (UserControls.LogFailedSummary)page.LoadControl(userControlPath + "/LogFailedSummary.ascx");
                    uclogSummary.BindLogSummary(user);
                    form.Controls.Add(uclogSummary);

                }

                page.Controls.Add(form);
                using (StringWriter writer = new StringWriter())
                {
                    HttpContext.Current.Server.Execute(page, writer, false);
                    ctrl.HTMLDataList = CleanHtml(writer.ToString());
                    writer.Close();
                }
            }
            else
            {
                ctrl.HTMLDataList = "0";
            }

            //return ctrl;
            //}
            //catch(Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
            return ctrl;
        }

        //logFailed


        //Search by username in login in failed 
        [System.Web.Services.WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public UserBindInformation SearchFailedLoginLogs(string username)
        {
            string userControlPath = ConfigurationManager.AppSettings["UserControls"].TrimEnd('/');
            UserBindInformation ctrl = new UserBindInformation();
            System.Web.UI.Page page = new Page();
            HtmlForm form = new HtmlForm();
            List<LogFailed> userList = BLL.CommonBLL.SearchLogInfo(username);
            if (userList.Count > 0)
            {
                foreach (LogFailed user in userList)
                {
                    UserControls.LogFailedSummary uclogSummary = (UserControls.LogFailedSummary)page.LoadControl(userControlPath + "/LogFailedSummary.ascx");
                    uclogSummary.BindLogSummary(user);
                    form.Controls.Add(uclogSummary);

                }

                page.Controls.Add(form);
                using (StringWriter writer = new StringWriter())
                {
                    HttpContext.Current.Server.Execute(page, writer, false);
                    ctrl.HTMLDataList = CleanHtml(writer.ToString());
                    writer.Close();
                }
            }
            else
            {
                ctrl.HTMLDataList = "0";
            }
            return ctrl;

        }
        //Search by username in login in failed 

        [System.Web.Services.WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public int UserDetails(int userId, string city, string state, string country, string gender, DateTime dob, string profileImage)
        {
            try
            {
                byte[] profileImageBytes = Convert.FromBase64String(profileImage);
                return BLL.CommonBLL.UserDetails(userId, city, state, country, gender, dob, profileImageBytes);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return -1;
            }
        }

        public static string CleanHtml(string html)

        {
            html = Regex.Replace(html, @"<[/]?(form|[ovwxp]:\w+)[^>]*?>", "", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"<script\b[^<]*(?:(?!<\/script>)<[^<]*)*<\/script>", "", RegexOptions.IgnoreCase);
            html = html.Replace("class=\"aspNetHidden\"", "class='aspNetHidden'");
            html = Regex.Replace(html.Trim(), @"<div class='aspNetHidden'>[^&;]*?</div>", "", RegexOptions.IgnoreCase);
            return html.Trim();
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public UserBindInformation AdminData()
        {
            string userControlPath = ConfigurationManager.AppSettings["UserControls"].TrimEnd('/');
            UserBindInformation ctrl = new UserBindInformation();
            System.Web.UI.Page page = new Page();
            HtmlForm form = new HtmlForm();

            List<DataListInfo> userList = BLL.CommonBLL.GetUserInfo();
            if (userList.Count > 0)
            {
                foreach (DataListInfo user in userList)
                {
                    UserControls.DataSummary ucUserSummary = (UserControls.DataSummary)page.LoadControl(userControlPath + "/DataSummary.ascx");

                    HtmlGenericControl EditAction = ucUserSummary.FindControl("Edit") as HtmlGenericControl;
                    HtmlGenericControl DeleteAction = ucUserSummary.FindControl("Delete") as HtmlGenericControl;

                    EditAction.Attributes.Add("onclick", "javascript:EditUserDetails(" + user.UserId + ");return false;");
                    DeleteAction.Attributes.Add("onclick", $"DeleteUserDetails({user.UserId}); return false;");

                    ucUserSummary.BindUserSummary(user);
                    form.Controls.Add(ucUserSummary);

                }

                page.Controls.Add(form);
                using (StringWriter writer = new StringWriter())
                {
                    HttpContext.Current.Server.Execute(page, writer, false);
                    ctrl.HTMLDataList = CleanHtml(writer.ToString());
                    writer.Close();
                }
            }
            else
            {
                ctrl.HTMLDataList = "0";
            }

            return ctrl;
        }

        //GetUserInfo
        [System.Web.Services.WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public EditData GetUserInfo(int userID)
        {
            return BLL.CommonBLL.GetUserDetail(userID);
        }

        //to update user details
        [System.Web.Services.WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public bool UpdateUserInfo(EditData userInfo)
        {
            return BLL.CommonBLL.UpdateUser(userInfo);
        }

        //delete
        [System.Web.Services.WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public string DeleteUser(int userid)
        {
            return BLL.CommonBLL.DeleteUser(userid);
        }

        //Chnage the role from dropdown
        [System.Web.Services.WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public bool UpdateRole(int userId, string role)
        {
            return BLL.CommonBLL.UpdateChangeRole(userId, role);
        }
        //Chnage the role from dropdown

        //download and upload excel part 
        //[System.Web.Services.WebMethod(EnableSession = true)]
        //[System.Web.Script.Services.ScriptMethod]
        //public string DownloadAllPPE()
        //{
        //    return BLL.CommonBLL.DownloadAllPPE();
        //}

        //[System.Web.Services.WebMethod(EnableSession = true)]
        //[System.Web.Script.Services.ScriptMethod]
        //public bool CheckPPEFileExist(string fileName)
        //{
        //    string logFilePath = ConfigurationManager.AppSettings["LogFileLocation"] + fileName;
        //    if (File.Exists(logFilePath))
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        //download and upload 
        //[System.Web.Services.WebMethod(EnableSession = true)]
        //[System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        //public bool DownloadUserInfoTemplate()
        //{
        //        return BLL.CommonBLL.DownloadUserInfoTemplate();

    }



    [XmlRoot]
    public class UserBindInformation
    {
        public string HTMLDataList { get; set; }
    }
}


