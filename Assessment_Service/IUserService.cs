using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Assessment_Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IUserService" in both code and config file together.
    [ServiceContract]
    public interface IUserService
    {
        [OperationContract]
        int RegistrationDetails(string firstName, string lastName, string emailId, string mobileNo, string password, string role);

        [OperationContract]
        UserClass LoginDetails(string emailId, string password);
        //Loginfailed insert
        [OperationContract]
        UserClass LogFailedLoginAttempt(string emailId, string password);

        [OperationContract]
        //loginFailedFetch
        //List<LogFailed> GetLogInfo(string emailId, string password);
        List<LogFailed> GetLogInfo(string emailId, string password);

        [OperationContract]
        //search log failed
        List<LogFailed> SearchLogInfo(string username);

        [OperationContract]
        int UserDetails(int userId,string city, string state, string country, string gender, DateTime dob, byte[] profileImage);

        [OperationContract]
        List<DataListInfo> GetAdminData();

        //GetUserInfo
        [OperationContract]
        //DataListInfo GetUsers(int userID);
        EditData GetUserDetail(int userID);

        //TO UPDATE USER DATA
        [OperationContract]
        //bool UpdateUserData(DataListInfo userInfo);
        bool UpdateUserInfo(EditData userInfo);

        //Update Role from DropDown
        [OperationContract]
        bool UpdateChangeRole(int userId, string role);
        //Update Role from DropDown

        //Delete User
        [OperationContract]
        string DeleteUser(int userid);

        //Get admin first name
        [OperationContract]
        string GetAdminFirstName(int userId);

        //[OperationContract]
        //[WebInvoke(UriTemplate = "DownloadAllPPE", Method = "GET", RequestFormat = WebMessageFormat.Xml)]
        //string DownloadAllPPE();

        [OperationContract]
        [WebInvoke(UriTemplate = "UploadUserDetails?fileName={fileName}", Method = "POST", RequestFormat = WebMessageFormat.Xml)]
        string UploadUserDetails(string fileName);


        [OperationContract]
        [WebInvoke(UriTemplate = "DownloadUserInfoTemplate", Method = "GET", RequestFormat = WebMessageFormat.Xml)]
        string DownloadUserInfoTemplate();
    }
}
